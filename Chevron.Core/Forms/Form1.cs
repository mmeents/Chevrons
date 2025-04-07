using Microsoft.Extensions.DependencyInjection;
using Chevron.Core.PackageModels;
using Chevron.Core.Service;
using System.Xml.Linq;
using Chevron.Core.Extensions;

namespace Chevron.Core.Forms {

  public partial class Form1 : Form {
    private ISettingsService _settings;
    private IVirtualDirectoryService _virtualDirectoryService;

    #region Log Message Delegate ILogMsg
    delegate void SetLogMsgCallback(string msg);
    public void LogMsg(string msg) {
      if (this.edLog.InvokeRequired) {
        SetLogMsgCallback d = new SetLogMsgCallback(LogMsg);
        this.BeginInvoke(d, new object[] { msg });
      } else {
        if (!edLog.Visible) edLog.Visible = true;
        this.edLog.Text = msg + Environment.NewLine + edLog.Text;
      }
    }

    #endregion

    #region Startup 
    public Form1(ISettingsService settingsService, IVirtualDirectoryService virtualDirectoryService) {
      InitializeComponent();
      _settings = settingsService;
      _virtualDirectoryService = virtualDirectoryService;
    }

    private void Form1_Load(object sender, EventArgs e) {
      lbDefaultDir.Text = $"Default Path: {_settings.DefaultPath}";
      lbWatchConfigLocation.Text = $"Watched Events File: {_settings.Settings[Ss.WatchedEventsFileName].Value}";
      ConfigureTvWatching();
      this.Invoke((Action)(async () => {
        await vrWatchingItem.EnsureCoreWebView2Async().ConfigureAwait(false);
      }));
    }

    private bool _InWatchedFolderReset = false;
    private bool _InWatchedFolderEdit = false;
    private bool InWatchedFolderEdit {
      get { return _InWatchedFolderEdit; }
      set {
        _InWatchedFolderEdit = value;
        if (_InWatchedFolderEdit) {

          if (edWatchedFolder.BackColor != Color.LightYellow) edWatchedFolder.BackColor = Color.LightYellow;
          if (edIncludeSubFolders.BackColor != Color.LightYellow) edIncludeSubFolders.BackColor = Color.LightYellow;
          if (edFileFilter.BackColor != Color.LightYellow) edFileFilter.BackColor = Color.LightYellow;
          if (edEncryptZip.BackColor != Color.LightYellow) edEncryptZip.BackColor = Color.LightYellow;
          if (edZipPassword.BackColor != Color.LightYellow) edZipPassword.BackColor = Color.LightYellow;
          if (edMbPreZipSize.BackColor != Color.LightYellow) edMbPreZipSize.BackColor = Color.LightYellow;

          if (!btnSaveWatchedFolderChanges.Visible) btnSaveWatchedFolderChanges.Visible = true;

        } else {

          if (edWatchedFolder.BackColor != Color.White) edWatchedFolder.BackColor = Color.White;
          if (edIncludeSubFolders.BackColor != Color.White) edIncludeSubFolders.BackColor = Color.White;
          if (edFileFilter.BackColor != Color.White) edFileFilter.BackColor = Color.White;
          if (edEncryptZip.BackColor != Color.White) edEncryptZip.BackColor = Color.White;
          if (edZipPassword.BackColor != Color.White) edZipPassword.BackColor = Color.White;
          if (edMbPreZipSize.BackColor != Color.White) edMbPreZipSize.BackColor = Color.White;

          if (btnSaveWatchedFolderChanges.Visible) btnSaveWatchedFolderChanges.Visible = false;

        }
      }
    }

    private void ConfigureTvWatching() {
      tvWatching.BeforeExpand += _virtualDirectoryService.WatchedTree_BeforeExpand;
      tvWatching.AfterSelect += _virtualDirectoryService.WatchedTree_AfterSelect;
      _virtualDirectoryService.WatchedFolderChanged += OnWatchedFolderChanged;
      _virtualDirectoryService.WatchedItemChanged += OnWatchedItemChanged;
      _virtualDirectoryService.WatchedTree_LoadTreeView(tvWatching);
      tcWatchingRight.SelectedTab = tpEmpty;

      tvChanges.BeforeExpand += _virtualDirectoryService.ChangesTree_BeforeExpand;
      tvChanges.AfterSelect += _virtualDirectoryService.ChangesTree_AfterSelect;
      _virtualDirectoryService.ChangesFolderChanged += OnChangesFolderChanged;
      _virtualDirectoryService.ChangesItemChanged += OnChangesItemChanged;
      _virtualDirectoryService.ChangesTree_LoadTreeView(tvChanges);
      tcChangesRight.SelectedTab = tpChangesEmpty;

    }
    #endregion

    #region Form Closeing 
    private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
      if (e.CloseReason == CloseReason.UserClosing) // Only hide on user X click
        {
        e.Cancel = true; // Stop the close
        this.Hide();     // Hide instead
      }
    }
    private void tcRootMain_Selecting(object sender, TabControlCancelEventArgs e) {
      if (tcRootMain.SelectedTab == tpExitApp) {
        e.Cancel = true;
        Application.Exit();
      }
    }
    #endregion

    #region Watched Folders events 

    private void tvWatchingMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e) {

      if (_virtualDirectoryService.CurrentWatchedFolder == null) {
        resyncFolderToolStripMenuItem.Enabled = false;
      } else {
        resyncFolderToolStripMenuItem.Enabled = true;
      }

      if (_virtualDirectoryService.CurrentIndex == null) {
        removeWatchedFolderToolStripMenuItem.Enabled = false;
      } else {
        removeWatchedFolderToolStripMenuItem.Enabled = true;
      }
    }

    private void resyncFolderToolStripMenuItem_Click(object sender, EventArgs e) {
      if (_virtualDirectoryService.CurrentWatchedFolder != null) {
        _virtualDirectoryService.ResyncWatchedFolder(_virtualDirectoryService.CurrentWatchedFolder);
      }
    }

    private void OnWatchedFolderChanged(object sender, WatchedFolder? folder) {
      tcWatchingRight.SelectedTab = tpWatchedFolder;
      ConfigureTpWatchedFolder(folder);
    }

    private void OnWatchedItemChanged(object sender, IndexModel? index) {
      tcWatchingRight.SelectedTab = tpIndexItem;
      ConfigureTpIndexItem(index);
    }

    private void ConfigureTpWatchedFolder(WatchedFolder folder) {

      if (folder == null) {  // No folder selected reset the UI


        edWatchedFolder.Text = "";
        if (edWatchedFolder.Enabled) edWatchedFolder.Enabled = false;
        if (btnBrowseWatchedFolder.Enabled) btnBrowseWatchedFolder.Enabled = false;

        if (edIncludeSubFolders.Enabled) edIncludeSubFolders.Enabled = false;
        if (edFileFilter.Enabled) edFileFilter.Enabled = false;
        if (edEncryptZip.Enabled) edEncryptZip.Enabled = false;
        if (edZipPassword.Enabled) edZipPassword.Enabled = false;
        if (edShowPassword.Enabled) edShowPassword.Enabled = false;
        if (edMbPreZipSize.Enabled) edMbPreZipSize.Enabled = false;

        if (btnSaveWatchedFolderChanges.Visible) btnSaveWatchedFolderChanges.Visible = false;


      } else {

        if (!btnBrowseWatchedFolder.Enabled) btnBrowseWatchedFolder.Enabled = true;
        if (!edWatchedFolder.Enabled) edWatchedFolder.Enabled = true;
        if (!edIncludeSubFolders.Enabled) edIncludeSubFolders.Enabled = true;
        if (!edFileFilter.Enabled) edFileFilter.Enabled = true;
        if (!edEncryptZip.Enabled) edEncryptZip.Enabled = true;
        if (!edZipPassword.Enabled) edZipPassword.Enabled = true;
        if (!edShowPassword.Enabled) edShowPassword.Enabled = true;
        if (!edMbPreZipSize.Enabled) edMbPreZipSize.Enabled = true;

        ResetWatchedPropertyEditors(folder);

        if (btnSaveWatchedFolderChanges.Visible) btnSaveWatchedFolderChanges.Visible = false;

      }
    }

    private string _currentBaseFolder = "";
    private void ConfigureVirtualHost() {
      var currentFolder = _virtualDirectoryService.CurrentWatchedFolder;
      if (currentFolder == null) {
        _currentBaseFolder = null;
        return;
      }
      if (_currentBaseFolder == currentFolder.FolderPath) return; // Avoid redundant calls
      _currentBaseFolder = currentFolder.FolderPath;
      vrWatchingItem.CoreWebView2?.SetVirtualHostNameToFolderMapping(
          Exts.LocalHostName,
          _currentBaseFolder,
          Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);
    }

    private void ConfigureTpIndexItem(IndexModel index) {
      if (index == null) {  // No folder selected reset the UI
        vrWatchingItem.NavigateToString("<html><body><p>Weird Empty null index</p></body></html>");
        return;
      }
      ConfigureVirtualHost();
      var currentFolder = _virtualDirectoryService.CurrentWatchedFolder;
      var html = index.GenerateHtmlView(currentFolder.FolderPath);
      vrWatchingItem.CoreWebView2.NavigateToString(html);
    }
    private void ResetWatchedPropertyEditors(WatchedFolder folder) {
      _InWatchedFolderReset = true;
      try {
        edShowPassword.Checked = false;
        edWatchedFolder.Text = folder.FolderPath;
        edIncludeSubFolders.Checked = folder.IncludeSubFolders;
        edFileFilter.Text = folder.FileFilter;
        edEncryptZip.Checked = _settings.EncryptZip;
        edZipPassword.Text = _settings.EncryptPassword;
        edMbPreZipSize.Value = _settings.FileTargetBatchSize;
      } catch (Exception ex) {
        LogMsg(ex.Message);
      } finally {
        _InWatchedFolderReset = false;
      }
    }

    #region tpWatchedFolder Editor Events 
    private void btnBrowseWatchedFolder_Click(object sender, EventArgs e) {
      BrowseFolderDialog.SelectedPath = edWatchedFolder.Text;
      if (BrowseFolderDialog.ShowDialog() == DialogResult.OK) {
        edWatchedFolder.Text = BrowseFolderDialog.SelectedPath;
        if (btnSaveWatchedFolderChanges.Visible == false) btnSaveWatchedFolderChanges.Visible = true;
      }
    }

    private void edIncludeSubFolders_CheckedChanged(object sender, EventArgs e) {
      if (!_InWatchedFolderReset && _virtualDirectoryService.CurrentWatchedFolder != null &&
          _virtualDirectoryService.CurrentWatchedFolder.IncludeSubFolders != edIncludeSubFolders.Checked) {
        InWatchedFolderEdit = true;
      }
    }

    private void edFileFilter_TextChanged(object sender, EventArgs e) {
      if (!_InWatchedFolderReset && _virtualDirectoryService.CurrentWatchedFolder != null &&
          _virtualDirectoryService.CurrentWatchedFolder.FileFilter != edFileFilter.Text) {
        InWatchedFolderEdit = true;
      }
    }

    private void edWatchedFolder_TextChanged(object sender, EventArgs e) {
      if (!_InWatchedFolderReset && _virtualDirectoryService.CurrentWatchedFolder != null &&
          _virtualDirectoryService.CurrentWatchedFolder.FolderPath != edWatchedFolder.Text) {
        InWatchedFolderEdit = true;
      }
    }
    private void edShowPassword_CheckedChanged(object sender, EventArgs e) {
      edZipPassword.UseSystemPasswordChar = !edShowPassword.Checked;
    }

    private void edEncryptZip_CheckedChanged(object sender, EventArgs e) {
      if (!_InWatchedFolderReset) {
        InWatchedFolderEdit = true;
      }
    }

    private void edZipPassword_TextChanged(object sender, EventArgs e) {
      if (!_InWatchedFolderReset) {
        InWatchedFolderEdit = true;
      }
    }

    private void edMbPreZipSize_ValueChanged(object sender, EventArgs e) {
      if (!_InWatchedFolderReset) {
        InWatchedFolderEdit = true;
      }
    }

    private void btnSaveWatchedFolderChanges_Click(object sender, EventArgs e) {
      if (!_InWatchedFolderReset && _virtualDirectoryService.CurrentWatchedFolder != null) {

        _settings.EncryptZip = edEncryptZip.Checked;
        _settings.EncryptPassword = edZipPassword.Text;
        _settings.FileTargetBatchSize = (int)edMbPreZipSize.Value;

        _virtualDirectoryService.CurrentWatchedFolder.FolderPath = edWatchedFolder.Text;
        _virtualDirectoryService.CurrentWatchedFolder.FileFilter = edFileFilter.Text;
        _virtualDirectoryService.CurrentWatchedFolder.IncludeSubFolders = edIncludeSubFolders.Checked;
        _virtualDirectoryService.SaveWatchedFolderChanges();


        InWatchedFolderEdit = false;
      }
    }

    #endregion
    private void btnMakeChevron_Click(object sender, EventArgs e) {
      //_virtualDirectoryService.BuildChevron(); -- still needs work.
    }
    #endregion 

    #region Changes Folders events
    private void tvChangesMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e) {

    }

    private void OnChangesFolderChanged(object sender, WatchedFolder? folder) {
      tcChangesRight.SelectedTab = tpChangesFolder;
      ConfigureTpChangesFolder(folder);
    }

    private void OnChangesItemChanged(object sender, IndexModel? index) {
      tcChangesRight.SelectedTab = tpChangesItem;
      ConfigureTpChangesIndexItem(index);
    }

    private void ConfigureTpChangesFolder(WatchedFolder folder) {
      if (folder == null) {  // No folder selected reset the UI

      } else {

      }
    }

    private void ConfigureTpChangesIndexItem(IndexModel index) {
      if (index == null) {  // No folder selected reset the UI
      } else {
      }
    }

    #endregion

    #region Bottom Region 
    private void tcBottom_SelectedIndexChanged(object sender, EventArgs e) {
      if (tcBottom.SelectedTab == tpHideTabs) {
        tcBottom.SelectedTab = this.tpErrors;
        splitContainer1.Panel2Collapsed = true;
      }
    }
    #endregion


  }
}
