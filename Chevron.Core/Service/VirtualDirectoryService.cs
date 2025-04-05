using Chevron.Core.PackageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.Core.Service
{
    public interface IVirtualDirectoryService {

      WatchedFolder? CurrentWatchedFolder { get; set; }
      IndexModel? CurrentIndex { get; set; }
      event EventHandler<WatchedFolder?> WatchedFolderChanged;
      event EventHandler<IndexModel?> WatchedItemChanged;
      void WatchedTree_LoadTreeView(TreeView treeView);
      void WatchedTree_BeforeExpand(object sender, TreeViewCancelEventArgs e);
      void WatchedTree_AfterSelect(object sender, TreeViewEventArgs e);

      WatchedFolder? CurrentChangesFolder { get; set; }
      IndexModel? CurrentChangesIndex { get; set; }
      event EventHandler<WatchedFolder?> ChangesFolderChanged;
      event EventHandler<IndexModel?> ChangesItemChanged;
      void ChangesTree_LoadTreeView(TreeView treeView);
      void ChangesTree_BeforeExpand(object sender, TreeViewCancelEventArgs e);
      void ChangesTree_AfterSelect(object sender, TreeViewEventArgs e);

      WatchedFolder? GetWatchedFolder(TreeNode currentNode);
      void SaveWatchedFolderChanges();
      void ResyncWatchedFolder(WatchedFolder folder);

    }

    public class VirtualDirectoryService : IVirtualDirectoryService  {

      private readonly ISettingsService _settingsService;
      private readonly IWatchedEventService _watchedService;
      private readonly IndexFileSet _indexService;
      private WatchedFolder? _currentFolder = null;
      private WatchedFolder? _currentChangesFolder = null;
      private IndexModel? _currentIndex = null;

      public VirtualDirectoryService(ISettingsService settingsService, IWatchedEventService watchedEventService) {
        _settingsService = settingsService;
        _watchedService = watchedEventService;
        _indexService = new IndexFileSet(_settingsService, _settingsService.IndexesFileName);
      }

      #region Watched Folders 

      // Event to notify subscribers when the watched folder changes
      public event EventHandler<WatchedFolder?> WatchedFolderChanged;
      // Event to notify subscribers when the watched folder changes
      public event EventHandler<IndexModel?> WatchedItemChanged;
      // Property to get/set the watched folder

      public WatchedFolder? CurrentWatchedFolder {
        get => _currentFolder;
        set {
          if (_currentFolder != value) {
            _currentFolder = value;
            OnWatchedFolderChanged(_currentFolder);
          }
        }
      }

      public IndexModel? CurrentIndex {
        get => _currentIndex;
        set {
          if (_currentIndex != value) {
            _currentIndex = value;
            OnWatchedItemChanged(_currentIndex);
          }
        }
      }    

      protected virtual void OnWatchedFolderChanged(WatchedFolder? newFolder) {
        var handler = WatchedFolderChanged;
        handler(this, newFolder);
      }      

      protected virtual void OnWatchedItemChanged(IndexModel? newIndex) {
        var handler = WatchedItemChanged;
        handler(this, newIndex);
      }     

      // main interface for TreeView details.
      public void WatchedTree_LoadTreeView(TreeView treeView) {
        treeView.Nodes.Clear();
        foreach (var folder in _watchedService.WatchingFolders.Values) {
          var node = treeView.Nodes.Add(folder.FolderPath);
          node.Tag = folder; // Store WatchedFolder, not IndexModel
          if (_indexService.Indexes.Values.Any(i => i.ParentId == folder.Id))
            node.Nodes.Add("Loading...");
        }
      }

      public void WatchedTree_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
        var node = e.Node;
        if (node != null && node.Nodes.Count == 1 && node.Nodes[0].Text == "Loading...") {
          node.Nodes.Clear();
          if (node.Tag is WatchedFolder folder) {
            // Top-level: WatchedFolder → Indexes
            var children = _indexService.Indexes.Values.Where(i => i.ParentId == folder.Id);
            foreach (var child in children) {  // this should only be one child, the folder itself

              var childrs = _indexService.Indexes.Values.Where(i => i.ParentId == child.Id);
              foreach (var chld in childrs) {
                var childNode = node.Nodes.Add(chld.FileName);
                childNode.Tag = chld;
                if (_indexService.Indexes.Values.Any(i => i.ParentId == chld.Id))
                  childNode.Nodes.Add("Loading...");
              }
            }
          } else if (node.Tag is IndexModel index) {
            // Sub-level: IndexModel → Indexes
            var children = _indexService.Indexes.Values.Where(i => i.ParentId == index.Id);
            foreach (var child in children) {
              var childNode = node.Nodes.Add(child.FileName);
              childNode.Tag = child;
              if (_indexService.Indexes.Values.Any(i => i.ParentId == child.Id))
                childNode.Nodes.Add("Loading...");
            }
          }
        }
      }

      public void WatchedTree_AfterSelect(object sender, TreeViewEventArgs e) {
        var node = e.Node;
        if (node == null) return;
        CurrentWatchedFolder = GetWatchedFolder(node);
        CurrentIndex = node.Tag is IndexModel index ? index : null;
      }
    #endregion
    #region Changes view 

      public event EventHandler<WatchedFolder?> ChangesFolderChanged;
      public event EventHandler<IndexModel?> ChangesItemChanged;
      protected virtual void OnChangesFolderChanged(WatchedFolder? newFolder) {
        var handler = ChangesFolderChanged;
        handler(this, newFolder);
      }
      protected virtual void OnChangesItemChanged(IndexModel? newIndex) {
        var handler = ChangesItemChanged;
        handler(this, newIndex);
      }

      public WatchedFolder? CurrentChangesFolder {
        get => _currentChangesFolder;
        set {
          if (_currentChangesFolder != value) {
            _currentChangesFolder = value;
            OnChangesFolderChanged(_currentChangesFolder);
          }
        }
      }

      public IndexModel? CurrentChangesIndex {
        get => _currentIndex;
        set {
          if (_currentIndex != value) {
            _currentIndex = value;
            OnWatchedItemChanged(_currentIndex);
          }
        }
      }

      public void ChangesTree_LoadTreeView(TreeView treeView){
        treeView.Nodes.Clear();
        foreach (var folder in _watchedService.WatchingFolders.Values) {
          var node = treeView.Nodes.Add(folder.FolderPath);
          node.Tag = folder; // Store WatchedFolder, not IndexModel
          if (_indexService.Indexes.Values.Any(i => i.ParentId == folder.Id))
            node.Nodes.Add("Loading...");
        }
      }

      public void ChangesTree_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
        var node = e.Node;
        if (node != null && node.Nodes.Count == 1 && node.Nodes[0].Text == "Loading...") {
          node.Nodes.Clear();
          if (node.Tag is WatchedFolder folder) {
            // Top-level: WatchedFolder → Indexes
            var children = _indexService.Indexes.Values.Where(i => i.ParentId == folder.Id);
            foreach (var child in children) {  // this should only be one child, the folder itself
              var childrs = _indexService.Indexes.Values.Where(i => i.ParentId == child.Id && !i.IsArchived);
              foreach (var chld in childrs) {
                var childNode = node.Nodes.Add(chld.FileName);
                childNode.Tag = chld;
                if (_indexService.Indexes.Values.Any(i => i.ParentId == chld.Id))
                  childNode.Nodes.Add("Loading...");
              }
            }
          } else if (node.Tag is IndexModel index) {
            // Sub-level: IndexModel → Indexes
            var children = _indexService.Indexes.Values.Where(i => i.ParentId == index.Id && !i.IsArchived);
            foreach (var child in children) {
              var childNode = node.Nodes.Add(child.FileName);
              childNode.Tag = child;
              if (_indexService.Indexes.Values.Any(i => i.ParentId == child.Id))
                childNode.Nodes.Add("Loading...");
            }
          }
        }
      }

      public void ChangesTree_AfterSelect(object sender, TreeViewEventArgs e){
        var node = e.Node;
        if (node == null) return;
        CurrentWatchedFolder = GetWatchedFolder(node);
        CurrentIndex = node.Tag is IndexModel index ? index : null;
      }
    
      #endregion
      #region Shared methods

      public WatchedFolder? GetWatchedFolder(TreeNode currentNode) {
        while (currentNode != null && !(currentNode.Tag is WatchedFolder)) {
          currentNode = currentNode.Parent;
        }
        return currentNode?.Tag as WatchedFolder;
      }

      public void SaveWatchedFolderChanges() {
        Task.Run( async () => await _watchedService.ReLoadAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
      }

      public void ResyncWatchedFolder(WatchedFolder folder) {
        _indexService.SyncIndexToFolder(folder);
      }

      #endregion

    }

}