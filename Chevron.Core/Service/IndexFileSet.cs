using Chevron.Core.PackageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using Chevron.Core.Dictionaries;
using Chevron.Core.Extensions;

namespace Chevron.Core.Service
{
  public interface IIndexesService {
    Settings Settings { get; set; }
    Dictionaries.Keys Keys { get; set; }
    Indexes Indexes { get; set; }
    long FileTargetBatchSize { get; set; }
    int TargetZipPartCount { get; set; }
    KeyModel AddKey(KeyModel Key);
    void RemoveKey(Guid KeyId);
    IndexModel AddIndex(IndexModel Index);
    void RemoveIndex(Guid IndexId);
    void MergeThisSetWith(IndexFileSet OtherSet);
    void RemoveThisSetFrom(IndexFileSet OtherSet);
  }
  public class IndexFileSet : IIndexesService {
    private readonly ISettingsService _settingsService;
    private readonly string _fileName;
    private IndexPackage _package;
    private Settings _settings;
    public IndexFileSet(ISettingsService settingsService, string FileName) {      
      _fileName = FileName;
      _package = new IndexPackage();
      _settingsService = settingsService;
      Load();
      _settings = Settings;
    }

    public void Load() {
      Task.Run(async () => await this.LoadAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
    }

    public async Task LoadAsync() {
      if (File.Exists(_fileName)) {
        var encoded = await _fileName.ReadAllTextAsync();
        var decoded = Convert.FromBase64String(encoded);
        _package = MessagePackSerializer.Deserialize<IndexPackage>(decoded);
      }
    }

    public void Save() {
      Task.Run(async () => await this.SaveAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
    }
    public async Task SaveAsync() {
      _package.SettingsList = _settings.AsList;
      var encoded = MessagePackSerializer.Serialize(_package);
      var decoded = Convert.ToBase64String(encoded);
      await decoded.WriteAllTextAsync(_fileName);
    }


    public Settings Settings {
      get {
        return new Settings(_package.SettingsList);
      }
      set {
        _package.SettingsList = value.AsList;
      }
    }
    public Dictionaries.Keys Keys {
      get {
        return new Dictionaries.Keys(_package.KeyList);
      }
      set {
        _package.KeyList = value.AsList;
      }
    }
    public Indexes Indexes {
      get {
        return new Indexes(_package.IndexList);
      }
      set {
        _package.IndexList = value.AsList;
      }
    }


    private long _fileTargetBatchSize=0;
    public long FileTargetBatchSize {
      get {
        if (_fileTargetBatchSize>0) return _fileTargetBatchSize;
        _fileTargetBatchSize = (_settings[Ss.FileTargetBatchSize].Value == "" ? _settingsService.ZipBatchSize.AsString() : _settings[Ss.FileTargetBatchSize].Value).AsInt64();
        return _fileTargetBatchSize;
      }
      set {
        _settings[Ss.FileTargetBatchSize].Value = value.AsString();
      }
    }

    private int _targetZipPartCount = 0;
    public int TargetZipPartCount {
      get {
        if (_targetZipPartCount > 0) return _targetZipPartCount;
        _targetZipPartCount = (_settings[Ss.TargetZipPartCount].Value == "" ? "1" : _settings[Ss.TargetZipPartCount].Value).AsInt32();
        return _targetZipPartCount;
      }
      set {
        _settings[Ss.TargetZipPartCount].Value = value.AsString();
      }
    } 

    public KeyModel AddKey(KeyModel Key) {
      Dictionaries.Keys targetKeys = Keys;
      if (Key.Id == Guid.Empty) Key.Id = Guid.NewGuid();
      targetKeys[Key.Id] = Key;
      Keys = targetKeys;
      return Key;
    }

    public void RemoveKey(Guid KeyId) {
      Dictionaries.Keys targetKeys = Keys;
      targetKeys.Remove(KeyId);
      Keys = targetKeys;
    }

    public IndexModel AddIndex(IndexModel Index) {
      Indexes targetIndexes = Indexes;
      if (Index.Id == Guid.Empty) Index.Id = Guid.NewGuid();
      targetIndexes[Index.Id] = Index;
      Indexes = targetIndexes;
      return Index;
    }

    public void RemoveIndex(Guid IndexId) {
      Indexes targetIndexes = Indexes;
      targetIndexes.Remove(IndexId);
      Indexes = targetIndexes;
    }

    public void MergeThisSetWith(IndexFileSet OtherSet) {
      foreach (KeyModel key in OtherSet.Keys.AsList) {
        AddKey(key);
      }
      foreach (IndexModel index in OtherSet.Indexes.AsList) {
        AddIndex(index);
      }
    }

    public void RemoveThisSetFrom(IndexFileSet OtherSet) {
      foreach (KeyModel key in Keys.AsList) {
        OtherSet.RemoveKey(key.Id);
      }
      foreach (IndexModel index in Indexes.AsList) {
        OtherSet.RemoveIndex(index.Id);
      }
    }

    public IndexModel GetFolderIndex(string FolderPath) {
      return Indexes.AsList.FirstOrDefault(i => i.Location == FolderPath && i.IsFolder);
    }

    public IndexModel GetFileIndex(string FilePath) {
      return Indexes.AsList.FirstOrDefault(i => i.Location == FilePath && !i.IsFolder);
    }

    private List<IndexModel> SyncFilesInFolder(IndexModel folder) {
      List<IndexModel> fileIndexes = new();
      Directory.EnumerateFiles(folder.Location).ToList().ForEach(file => {
        var alreadyIndexed = GetFileIndex(file);
        if (alreadyIndexed == null) {
          var fileIndex = new IndexModel() {
            Id = Guid.NewGuid(),
            ParentId = folder.Id,
            IsFolder = false,
            Location = file,
            FileName = Path.GetFileName(file),
            FileHash = DataConvertExt.ComputeFileHash(file),
            FileSize = new FileInfo(file).Length,
            LastModified = File.GetLastWriteTime(file),
            ZipPart = "",
            IsArchived = false,
            Notes = ""
          };
          fileIndexes.Add(fileIndex);
        } else {
          fileIndexes.Add(alreadyIndexed);
        }
      });
      return fileIndexes;
    }

    private List<IndexModel> SyncFolderIndexes(string folderPath, Guid parentId, bool includeSubFolders) {
      List<IndexModel> folderIndexes = new();

      // Add the root folder itself if not already indexed
      var rootFolder = GetFolderIndex(folderPath);
      if (rootFolder == null) {
        rootFolder = new IndexModel {
          Id = Guid.NewGuid(),
          ParentId = parentId,
          IsFolder = true,
          Location = folderPath,
          FileName = Path.GetFileName(folderPath),
          FileHash = "",
          FileSize = 0,
          LastModified = Directory.GetLastWriteTime(folderPath),
          ZipPart = "",
          IsArchived = false,
          Notes = ""
        };
      }
      folderIndexes.Add(rootFolder);

      folderIndexes.AddRange(SyncFilesInFolder(rootFolder));

      if (includeSubFolders) {
        foreach(var dir in Directory.EnumerateDirectories(folderPath)) { 
          folderIndexes.AddRange(SyncFolderIndexes(dir, rootFolder.Id, includeSubFolders));
        }
      }
      
      return folderIndexes;
    }

    public void SyncIndexToFolder(WatchedFolder folder) {

      var newIndexes = SyncFolderIndexes(folder.FolderPath, folder.Id, folder.IncludeSubFolders);
      var mainIndex = Indexes;

      foreach (var index in newIndexes) {
        mainIndex[index.Id] = index;
      }

      Indexes = mainIndex;
      Save();
    }
  }

}
