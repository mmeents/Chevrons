using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chevron.Core.PackageModels;
using Chevron.Core.Dictionaries;
using Chevron.Core.Extensions;
using MessagePack;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Chevron.Core.Service
{
    public interface IWatchedEventService {
        WatchedFolders WatchingFolders { get; set; }
        WatchedEvents WatchedEventQueue { get; set; }
        void Load();
        Task LoadAsync();
        Task ReLoadAsync();
        Task SaveAsync();
        void AddWatchedFolder(string FolderPath);
        public WatchedFolder? GetWatchedFolder(string FolderPath);
        void RemoveWatchedFolder(Guid Id);
        void AddEvent(WatchedFolder folder, int EventTypeId, string FileNamePath, string? oldFileNamePath = null);
        void AddEventRange(IEnumerable<WatchedEvent> events);
        public void BuildChevron();
    }
  
    public class WatchedEventService  : IWatchedEventService {
            
      private readonly ISettingsService _settingsService;       
      private readonly string _fileName;
      private WatchedEventsPackage _package;
      private readonly ConcurrentDictionary<string, FileSystemWatcher> _watchers = new();
      private readonly EventTypes _eventTypes = new();

      public WatchedEventService(ISettingsService settingsService) {
        _settingsService = settingsService;        
        _eventTypes = new EventTypes(); 
        _fileName = _settingsService.Settings[Ss.WatchedEventsFileName].Value;
        _package = new WatchedEventsPackage();      
        Load();
      }

      private void StartWatcher(WatchedFolder folder) {
        if (_watchers.ContainsKey(folder.FolderPath)) return;

        var watcher = new FileSystemWatcher {
          Path = folder.FolderPath,
          NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
          Filter =  folder.FileFilter,
          IncludeSubdirectories = folder.IncludeSubFolders,
          EnableRaisingEvents = true
        };
        watcher.Created += (s, e) => HandleEvent(e.FullPath, folder, _eventTypes.FileAdded, _eventTypes.DirectoryAdded);
        watcher.Changed += (s, e) => HandleEvent(e.FullPath, folder, _eventTypes.FileModified, _eventTypes.DirectoryModified);
        watcher.Deleted += (s, e) => HandleEvent(e.FullPath, folder, _eventTypes.FileDeleted, _eventTypes.DirectoryDeleted);
        watcher.Renamed += (s, e) => HandleRenameEvent(e.OldFullPath, e.FullPath, folder);

        _watchers[folder.FolderPath] = watcher;
      }

      private void HandleEvent(string fullPath, WatchedFolder folder, EventType fileEvent, EventType dirEvent) {
        if (Directory.Exists(fullPath)) {
          AddEvent(folder, dirEvent.Id, fullPath);
        } else if (File.Exists(fullPath)) {
          AddEvent(folder, fileEvent.Id, fullPath);
        }
        // Else: Item might’ve been deleted/moved already; skip or log as needed
      }

      private void HandleRenameEvent(string oldFullPath, string newFullPath, WatchedFolder folder) {
        if (Directory.Exists(newFullPath)) {
          AddEvent(folder, _eventTypes.DirectoryRenamed.Id, newFullPath, oldFullPath);
        } else if (File.Exists(newFullPath)) {
          AddEvent(folder, _eventTypes.FileRenamed.Id, newFullPath, oldFullPath);
        }
      }

      public void Load() {
        Task.Run(async () => await this.LoadAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
      }

      public async Task LoadAsync() {
        if (File.Exists(_fileName)) {
          var encoded = await _fileName.ReadAllTextAsync();
          var decoded = Convert.FromBase64String(encoded);
          _package = MessagePackSerializer.Deserialize<WatchedEventsPackage>(decoded);   
          StartWatchingFolders();
        }
      }

      public async Task ReLoadAsync() {
        StopWatchersFolders();
        if (File.Exists(_fileName)) {
          var encoded = await _fileName.ReadAllTextAsync();
          var decoded = Convert.FromBase64String(encoded);
          _package = MessagePackSerializer.Deserialize<WatchedEventsPackage>(decoded);
          StartWatchingFolders();
        }
      }


      public async Task SaveAsync() {
        var encoded = MessagePackSerializer.Serialize(_package);
        var decoded = Convert.ToBase64String(encoded);
        await decoded.WriteAllTextAsync(_fileName);
      }

      private void StartWatchingFolders() {
        foreach (var folder in WatchingFolders.Values) {
          StartWatcher(folder);
        }
      }

      private void StopWatchersFolders() {
        foreach (var watcher in _watchers.Values) {
          watcher.EnableRaisingEvents = false;
          watcher.Dispose();
        }
        _watchers.Clear();
      }

      public WatchedFolders WatchingFolders {
        get {
          return new WatchedFolders(_package.WatchedFolderList);
        }
        set {
          _package.WatchedFolderList = value.AsList;
        }
      }

      public WatchedEvents WatchedEventQueue {
        get {
          return new WatchedEvents(_package.WatchedEventList);
        }
        set {
          _package.WatchedEventList = value.AsList;
        }
      }

      public void AddWatchedFolder(string folderPath) {
        var existing = GetWatchedFolder(folderPath);
        if (existing != null) return;
        
        WatchedFolder wf = new WatchedFolder() { FolderPath = folderPath };
        var List = WatchingFolders;
        List[wf.Id] = wf;
        WatchingFolders = List;
        StartWatcher(wf);
        SaveAsync().GetAwaiter().GetResult();
      }

      public WatchedFolder? GetWatchedFolder(string FolderPath) {
        return WatchingFolders.Values.FirstOrDefault(x => x.FolderPath == FolderPath);
      }

      public void RemoveWatchedFolder(Guid Id) {
        var List = WatchingFolders;

        var folder = List[Id];
        if (folder != null && _watchers.TryGetValue(folder.FolderPath, out var watcher)) {
          watcher.EnableRaisingEvents = false;
          watcher.Dispose();
          _ = _watchers.TryRemove(folder.FolderPath, out _);
        }
        
        List.Remove(Id);
        WatchingFolders = List;
        SaveAsync().GetAwaiter().GetResult();
      }

      public void AddEvent(WatchedFolder folder, int eventTypeId, string fileNamePath, string? oldFileNamePath = null) {
      var queue = WatchedEventQueue;
      WatchedEvent? existing = queue.Values.FirstOrDefault(x => x.SourceLocation == fileNamePath);

      // Handle deletions
      if (eventTypeId == _eventTypes.FileDeleted.Id && existing != null) {
        queue.Remove(existing.Id);
        WatchedEventQueue = queue;
        SaveAsync().GetAwaiter().GetResult();
        return;
      }

      // Handle renames (look up by old path, remove, and add new entry)
      if (eventTypeId == _eventTypes.FileRenamed.Id && !string.IsNullOrEmpty(oldFileNamePath)) {
        existing = queue.Values.FirstOrDefault(x => x.SourceLocation == oldFileNamePath);
        if (existing != null) {
          queue.Remove(existing.Id);
        }
        // Always create a new entry for the renamed file
        var we = new WatchedEvent {
          WatchedFolderId = folder.Id,
          EventTypeId = eventTypeId,
          SourceLocation = fileNamePath,
          FileName = Path.GetFileName(fileNamePath),
          FileHash = File.Exists(fileNamePath) ? DataConvertExt.ComputeFileHash(fileNamePath) : "",
          FileSize = File.Exists(fileNamePath) ? new FileInfo(fileNamePath).Length : 0,
          FileLastModified = File.Exists(fileNamePath) ? File.GetLastWriteTime(fileNamePath) : DateTime.UtcNow,
          IsArchived = false
        };
        queue[we.Id] = we;
        WatchedEventQueue = queue;
        SaveAsync().GetAwaiter().GetResult();
        return;
      }

      // Handle adds or updates (create new or update existing)
      if (existing != null) {
        // Update existing entry
        existing.EventTypeId = eventTypeId;
        existing.FileHash = File.Exists(fileNamePath) ? DataConvertExt.ComputeFileHash(fileNamePath) : existing.FileHash;
        existing.FileSize = File.Exists(fileNamePath) ? new FileInfo(fileNamePath).Length : existing.FileSize;
        existing.FileLastModified = File.Exists(fileNamePath) ? File.GetLastWriteTime(fileNamePath) : existing.FileLastModified;
        existing.IsArchived = false; // Reset if it was archived but changed
        queue[existing.Id] = existing; // Ensure it’s updated in the queue
      } else {
        // Add new entry
        var we = new WatchedEvent {
          WatchedFolderId = folder.Id,
          EventTypeId = eventTypeId,
          SourceLocation = fileNamePath,
          FileName = Path.GetFileName(fileNamePath),
          FileHash = File.Exists(fileNamePath) ? DataConvertExt.ComputeFileHash(fileNamePath) : "",
          FileSize = File.Exists(fileNamePath) ? new FileInfo(fileNamePath).Length : 0,
          FileLastModified = File.Exists(fileNamePath) ? File.GetLastWriteTime(fileNamePath) : DateTime.UtcNow,
          IsArchived = false
        };
        queue[we.Id] = we;
      }

      WatchedEventQueue = queue;
      SaveAsync().GetAwaiter().GetResult();
    }

      public void AddEventRange(IEnumerable<WatchedEvent> events) {
        var Queue = WatchedEventQueue;
        foreach (var we in events) {
          Queue[we.Id] = we;
        }
        WatchedEventQueue = Queue;
        SaveAsync().GetAwaiter().GetResult();
      }

      public void BuildChevron(WatchedFolder folder) {
        // needs tons of work
        var unarchived = WatchedEventQueue.Values.Where(e => e.WatchedFolderId==folder.Id && !e.IsArchived).ToList();
        if (!unarchived.Any()) return;
        long maxSize = _settingsService.Settings[Ss.FileTargetBatchSize].Value.AsInt64();
        var folderGroups = unarchived.GroupBy(e => Path.GetDirectoryName(e.SourceLocation));
        foreach (var group in folderGroups) {
          var folderName = Path.GetFileName(group.Key).ReplaceInvalidFileNameChars("_");
          var baseName = $"{folderName}-{DateTime.Now:yyyyMMddHHmmss}";
          int part = 1;
          long totalSize = 0;
          var batch = new List<WatchedEvent>();

          foreach (var evt in group) {
            if (evt.FileSize > maxSize) {
              // Handle oversized file by splitting it
              var splitFiles = SplitFile(evt, maxSize, baseName, part);
              part += splitFiles.Count; // Increment part number based on splits
              foreach (var splitEvt in splitFiles) {
                // Process each split as a separate batch
                var zipName = $"{baseName}-part{part++}.zip";
                CreateChevron(new List<WatchedEvent> { splitEvt }, zipName);
                EncryptZip(zipName, GetDefaultKey());
              }
            } else {
              // Normal batching logic for files under the limit
              if (totalSize + evt.FileSize > maxSize && batch.Any()) {
                var zipName = $"{baseName}-part{part++}.zip";
                CreateChevron(batch, zipName);
                EncryptZip(zipName, GetDefaultKey());
                batch.Clear();
                totalSize = 0;
              }
              batch.Add(evt);
              totalSize += evt.FileSize;
            }
          }
        }
        SaveAsync().GetAwaiter().GetResult();
      }

      private void CreateChevron(List<WatchedEvent> events, string zipName) {
        var indexFile = "temp_index.msgpack";
        var indexService = new IndexFileSet(_settingsService, indexFile);
        var key = indexService.AddKey(GetDefaultKey());

        using (var zip = ZipFile.Open(zipName, ZipArchiveMode.Create)) {
          foreach (var evt in events) {
            if (File.Exists(evt.SourceLocation)) {
              zip.CreateEntryFromFile(evt.SourceLocation, evt.FileName, CompressionLevel.Optimal);
            }
            evt.IsArchived = true;
            var index = new IndexModel {
              Location = evt.SourceLocation,
              FileName = evt.FileName,
              FileHash = evt.FileHash,
              FileSize = evt.FileSize,
              LastModified = evt.FileLastModified,
              ZipPart = zipName.Replace(".zip", ".szip"), // Reflect final name
              IsArchived = true,
              KeyId = key.Id
            };
            indexService.AddIndex(index);
          }
          indexService.Save();
          zip.CreateEntryFromFile(indexFile, "index.msgpack");
        }

        var queue = WatchedEventQueue;
        foreach (var evt in events) queue[evt.Id] = evt;
        WatchedEventQueue = queue;

        if (File.Exists(indexFile)) File.Delete(indexFile);
      }

      private void EncryptZip(string zipPath, KeyModel key) {
        var encryptedPath = zipPath.Replace(".zip", ".szip");
        using (var inputFile = new FileStream(zipPath, FileMode.Open, FileAccess.Read))
        using (var outputFile = new FileStream(encryptedPath, FileMode.Create, FileAccess.Write))
        using (var aes = Aes.Create()) {
          aes.Key = DeriveKeyFromPassword(key.Value);
          aes.GenerateIV();
          outputFile.Write(aes.IV, 0, aes.IV.Length); // Prepend IV

          using (var cryptoStream = new CryptoStream(outputFile, aes.CreateEncryptor(), CryptoStreamMode.Write)) {
            inputFile.CopyTo(cryptoStream);
          }
        }
        File.Delete(zipPath); // Remove unencrypted zip
      }

      public void OpenEncryptedZip(string szipPath, KeyModel key, Action<string> onDecryptedZip) {
        var tempZip = Path.GetTempFileName().Replace(".tmp", ".zip");
        using (var inputFile = new FileStream(szipPath, FileMode.Open, FileAccess.Read))
        using (var outputFile = new FileStream(tempZip, FileMode.Create, FileAccess.Write))
        using (var aes = Aes.Create()) {
          aes.Key = DeriveKeyFromPassword(key.Value);
          byte[] iv = new byte[aes.IV.Length];
          inputFile.Read(iv, 0, iv.Length); // Read IV
          aes.IV = iv;

          using (var cryptoStream = new CryptoStream(inputFile, aes.CreateDecryptor(), CryptoStreamMode.Read)) {
            cryptoStream.CopyTo(outputFile);
          }
        }

        onDecryptedZip(tempZip); // Pass temp zip to caller
      }

      private byte[] DeriveKeyFromPassword(string password) {
        using SHA256 sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
      }

      private KeyModel GetDefaultKey() => new KeyModel { Id = Guid.NewGuid(), Name = "Default", Value = "test-password-123" };


      // Helper method to split a large file
      private List<WatchedEvent> SplitFile(WatchedEvent originalEvent, long maxSize, string baseName, int startingPart) {
        var splitEvents = new List<WatchedEvent>();
        string sourcePath = originalEvent.SourceLocation;
        long fileSize = originalEvent.FileSize;
        int partCount = (int)Math.Ceiling((double)fileSize / maxSize);
        byte[] buffer = new byte[maxSize];

        using (var fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read)) {
          for (int i = 0; i < partCount; i++) {
            long offset = i * maxSize;
            long bytesToRead = Math.Min(maxSize, fileSize - offset);
            string splitFileName = $"{Path.GetFileNameWithoutExtension(originalEvent.FileName)}_part{i + 1}{Path.GetExtension(originalEvent.FileName)}";
            string tempSplitPath = Path.Combine(Path.GetTempPath(), splitFileName);

            using (var splitFs = new FileStream(tempSplitPath, FileMode.Create, FileAccess.Write)) {
              fs.Seek(offset, SeekOrigin.Begin);
              int bytesRead = fs.Read(buffer, 0, (int)bytesToRead);
              splitFs.Write(buffer, 0, bytesRead);
            }

            var splitEvent = new WatchedEvent {
              Id = Guid.NewGuid(), // New ID for the split part
              SourceLocation = tempSplitPath,
              FileName = splitFileName,
              FileSize = bytesToRead,
              FileHash = DataConvertExt.ComputeFileHash(tempSplitPath), // Assume a method to compute hash
              FileLastModified = originalEvent.FileLastModified,
              IsArchived = false
            };
            splitEvents.Add(splitEvent);
          }
        }

        return splitEvents;
      }


  }





  public class EventTypes : ConcurrentDictionary<int, EventType> {
    public EventType WatchedEventRoot { get; }
    public EventType FileAdded { get; }
    public EventType FileDeleted { get; }
    public EventType FileModified { get; }
    public EventType FileRenamed { get; }
    public EventType DirectoryAdded { get; }
    public EventType DirectoryDeleted { get; }
    public EventType DirectoryModified { get; }
    public EventType DirectoryRenamed { get; }

    public EventTypes() : base() {
       WatchedEventRoot = AddRootType("Watched Event");
       FileAdded = AddChildType(WatchedEventRoot.Id, "File Added");
       FileDeleted = AddChildType(WatchedEventRoot.Id, "File Deleted");
       FileModified = AddChildType(WatchedEventRoot.Id, "File Modified");
       FileRenamed = AddChildType(WatchedEventRoot.Id, "File Renamed");
       DirectoryAdded = AddChildType(WatchedEventRoot.Id, "Directory Added");
       DirectoryDeleted = AddChildType(WatchedEventRoot.Id, "Directory Deleted");
       DirectoryModified = AddChildType(WatchedEventRoot.Id, "Directory Modified");
       DirectoryRenamed = AddChildType(WatchedEventRoot.Id, "Directory Renamed");
    }

    #region EventTypes Dictionary Methods...
    public virtual Boolean Contains(int id) {
      try {
        return !(base[id] is null);
      } catch {
        return false;
      }
    }
    public virtual new EventType this[int id] {
      get { return Contains(id) ? base[id] : base.Values.First<EventType>(x => x.Id > id); }
      set { if (value != null) { base[id] = value; } else { Remove(id); } }
    }
    public int GetNextId() {
      int max = 0;
      if (this.Keys.Count > 0) {
        max = this.Select(x => x.Value).Max(x => x.Id);
      }
      return max + 1;
    }
    public virtual void Remove(int id) { if (Contains(id)) { _ = base.TryRemove(id, out _); } }
    public EventType AddRootType(string name) {
      EventType type = new EventType() { Name = name };
      if (type.Id == 0) {
        type.Id = GetNextId();
      }
      base[type.Id] = type;
      return type;
    }
    public EventType AddChildType(int parentId, string name) {
      EventType type = new EventType() { ParentId = parentId, Name = name };
      if (type.Id == 0) {
        type.Id = GetNextId();
      }
      base[type.Id] = type;
      return type;
    }
    #endregion

  }

  public class EventType {
      public int Id { get; set; }
      public int ParentId { get; set; }
      public string Name { get; set; }      
    }



}
