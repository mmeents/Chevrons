namespace Chevron.Core.Forms{
  partial class Form1 {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      splitContainer1 = new SplitContainer();
      tcRootMain = new TabControl();
      tpSetup = new TabPage();
      button1 = new Button();
      lbWatchConfigLocation = new Label();
      lbDefaultDir = new Label();
      tpWatching = new TabPage();
      splitContainer2 = new SplitContainer();
      tvWatching = new TreeView();
      tvWatchingMenuStrip = new ContextMenuStrip(components);
      addWatchedFolderToolStripMenuItem = new ToolStripMenuItem();
      resyncFolderToolStripMenuItem = new ToolStripMenuItem();
      removeWatchedFolderToolStripMenuItem = new ToolStripMenuItem();
      tcWatchingRight = new TabControl();
      tpWatchedFolder = new TabPage();
      edShowPassword = new CheckBox();
      edMbPreZipSize = new NumericUpDown();
      label6 = new Label();
      label5 = new Label();
      edZipPassword = new TextBox();
      edEncryptZip = new CheckBox();
      btnSaveWatchedFolderChanges = new Button();
      label4 = new Label();
      edFileFilter = new TextBox();
      edIncludeSubFolders = new CheckBox();
      btnBrowseWatchedFolder = new Button();
      edWatchedFolder = new TextBox();
      label2 = new Label();
      tpIndexItem = new TabPage();
      label3 = new Label();
      tpEmpty = new TabPage();
      label1 = new Label();
      tpChanges = new TabPage();
      splitContainer3 = new SplitContainer();
      tvChanges = new TreeView();
      tvChangesMenuStrip = new ContextMenuStrip(components);
      tcChangesRight = new TabControl();
      tpChangesFolder = new TabPage();
      tpChangesItem = new TabPage();
      tpChangesEmpty = new TabPage();
      tpVirtual = new TabPage();
      tpExitApp = new TabPage();
      tcBottom = new TabControl();
      tpErrors = new TabPage();
      edLog = new TextBox();
      tpWatchedStatus = new TabPage();
      tpHideTabs = new TabPage();
      openFileDialog1 = new OpenFileDialog();
      BrowseFolderDialog = new FolderBrowserDialog();
      ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
      splitContainer1.Panel1.SuspendLayout();
      splitContainer1.Panel2.SuspendLayout();
      splitContainer1.SuspendLayout();
      tcRootMain.SuspendLayout();
      tpSetup.SuspendLayout();
      tpWatching.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
      splitContainer2.Panel1.SuspendLayout();
      splitContainer2.Panel2.SuspendLayout();
      splitContainer2.SuspendLayout();
      tvWatchingMenuStrip.SuspendLayout();
      tcWatchingRight.SuspendLayout();
      tpWatchedFolder.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)edMbPreZipSize).BeginInit();
      tpIndexItem.SuspendLayout();
      tpEmpty.SuspendLayout();
      tpChanges.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
      splitContainer3.Panel1.SuspendLayout();
      splitContainer3.Panel2.SuspendLayout();
      splitContainer3.SuspendLayout();
      tcChangesRight.SuspendLayout();
      tcBottom.SuspendLayout();
      tpErrors.SuspendLayout();
      SuspendLayout();
      // 
      // splitContainer1
      // 
      splitContainer1.Dock = DockStyle.Fill;
      splitContainer1.Location = new Point(0, 0);
      splitContainer1.Name = "splitContainer1";
      splitContainer1.Orientation = Orientation.Horizontal;
      // 
      // splitContainer1.Panel1
      // 
      splitContainer1.Panel1.Controls.Add(tcRootMain);
      splitContainer1.Panel1.Padding = new Padding(0, 6, 0, 0);
      // 
      // splitContainer1.Panel2
      // 
      splitContainer1.Panel2.Controls.Add(tcBottom);
      splitContainer1.Panel2.Padding = new Padding(0, 3, 0, 0);
      splitContainer1.Size = new Size(833, 725);
      splitContainer1.SplitterDistance = 542;
      splitContainer1.TabIndex = 0;
      // 
      // tcRootMain
      // 
      tcRootMain.Appearance = TabAppearance.FlatButtons;
      tcRootMain.Controls.Add(tpSetup);
      tcRootMain.Controls.Add(tpWatching);
      tcRootMain.Controls.Add(tpChanges);
      tcRootMain.Controls.Add(tpVirtual);
      tcRootMain.Controls.Add(tpExitApp);
      tcRootMain.Dock = DockStyle.Fill;
      tcRootMain.Location = new Point(0, 6);
      tcRootMain.Margin = new Padding(3, 4, 3, 3);
      tcRootMain.Name = "tcRootMain";
      tcRootMain.Padding = new Point(6, 4);
      tcRootMain.SelectedIndex = 0;
      tcRootMain.Size = new Size(833, 536);
      tcRootMain.TabIndex = 0;
      tcRootMain.Selecting += tcRootMain_Selecting;
      // 
      // tpSetup
      // 
      tpSetup.BackColor = SystemColors.ButtonFace;
      tpSetup.Controls.Add(button1);
      tpSetup.Controls.Add(lbWatchConfigLocation);
      tpSetup.Controls.Add(lbDefaultDir);
      tpSetup.Location = new Point(4, 34);
      tpSetup.Name = "tpSetup";
      tpSetup.Padding = new Padding(3);
      tpSetup.Size = new Size(825, 498);
      tpSetup.TabIndex = 0;
      tpSetup.Text = "Setup";
      // 
      // button1
      // 
      button1.Location = new Point(729, 28);
      button1.Name = "button1";
      button1.Size = new Size(70, 33);
      button1.TabIndex = 6;
      button1.Text = "Browse";
      button1.UseVisualStyleBackColor = true;
      // 
      // lbWatchConfigLocation
      // 
      lbWatchConfigLocation.AutoSize = true;
      lbWatchConfigLocation.Location = new Point(85, 65);
      lbWatchConfigLocation.Name = "lbWatchConfigLocation";
      lbWatchConfigLocation.Size = new Size(50, 20);
      lbWatchConfigLocation.TabIndex = 1;
      lbWatchConfigLocation.Text = "label2";
      // 
      // lbDefaultDir
      // 
      lbDefaultDir.AutoSize = true;
      lbDefaultDir.Location = new Point(85, 34);
      lbDefaultDir.Name = "lbDefaultDir";
      lbDefaultDir.Size = new Size(50, 20);
      lbDefaultDir.TabIndex = 0;
      lbDefaultDir.Text = "label1";
      // 
      // tpWatching
      // 
      tpWatching.Controls.Add(splitContainer2);
      tpWatching.Location = new Point(4, 34);
      tpWatching.Name = "tpWatching";
      tpWatching.Padding = new Padding(3);
      tpWatching.Size = new Size(825, 498);
      tpWatching.TabIndex = 1;
      tpWatching.Text = "Watching";
      tpWatching.UseVisualStyleBackColor = true;
      // 
      // splitContainer2
      // 
      splitContainer2.Dock = DockStyle.Fill;
      splitContainer2.Location = new Point(3, 3);
      splitContainer2.Name = "splitContainer2";
      // 
      // splitContainer2.Panel1
      // 
      splitContainer2.Panel1.Controls.Add(tvWatching);
      // 
      // splitContainer2.Panel2
      // 
      splitContainer2.Panel2.Controls.Add(tcWatchingRight);
      splitContainer2.Size = new Size(819, 492);
      splitContainer2.SplitterDistance = 370;
      splitContainer2.TabIndex = 0;
      // 
      // tvWatching
      // 
      tvWatching.ContextMenuStrip = tvWatchingMenuStrip;
      tvWatching.Dock = DockStyle.Fill;
      tvWatching.Location = new Point(0, 0);
      tvWatching.Name = "tvWatching";
      tvWatching.Size = new Size(370, 492);
      tvWatching.TabIndex = 0;
      // 
      // tvWatchingMenuStrip
      // 
      tvWatchingMenuStrip.ImageScalingSize = new Size(20, 20);
      tvWatchingMenuStrip.Items.AddRange(new ToolStripItem[] { addWatchedFolderToolStripMenuItem, resyncFolderToolStripMenuItem, removeWatchedFolderToolStripMenuItem });
      tvWatchingMenuStrip.Name = "tvWatchingMenuStrip";
      tvWatchingMenuStrip.Size = new Size(241, 76);
      tvWatchingMenuStrip.Opening += tvWatchingMenuStrip_Opening;
      // 
      // addWatchedFolderToolStripMenuItem
      // 
      addWatchedFolderToolStripMenuItem.Name = "addWatchedFolderToolStripMenuItem";
      addWatchedFolderToolStripMenuItem.Size = new Size(240, 24);
      addWatchedFolderToolStripMenuItem.Text = "Add Watched Folder";
      // 
      // resyncFolderToolStripMenuItem
      // 
      resyncFolderToolStripMenuItem.Name = "resyncFolderToolStripMenuItem";
      resyncFolderToolStripMenuItem.Size = new Size(240, 24);
      resyncFolderToolStripMenuItem.Text = "Resync Folder";
      resyncFolderToolStripMenuItem.Click += resyncFolderToolStripMenuItem_Click;
      // 
      // removeWatchedFolderToolStripMenuItem
      // 
      removeWatchedFolderToolStripMenuItem.Name = "removeWatchedFolderToolStripMenuItem";
      removeWatchedFolderToolStripMenuItem.Size = new Size(240, 24);
      removeWatchedFolderToolStripMenuItem.Text = "Remove Watched Folder";
      // 
      // tcWatchingRight
      // 
      tcWatchingRight.Appearance = TabAppearance.FlatButtons;
      tcWatchingRight.Controls.Add(tpWatchedFolder);
      tcWatchingRight.Controls.Add(tpIndexItem);
      tcWatchingRight.Controls.Add(tpEmpty);
      tcWatchingRight.Dock = DockStyle.Fill;
      tcWatchingRight.Location = new Point(0, 0);
      tcWatchingRight.Name = "tcWatchingRight";
      tcWatchingRight.SelectedIndex = 0;
      tcWatchingRight.Size = new Size(445, 492);
      tcWatchingRight.TabIndex = 0;
      // 
      // tpWatchedFolder
      // 
      tpWatchedFolder.BackColor = SystemColors.ButtonFace;
      tpWatchedFolder.Controls.Add(edShowPassword);
      tpWatchedFolder.Controls.Add(edMbPreZipSize);
      tpWatchedFolder.Controls.Add(label6);
      tpWatchedFolder.Controls.Add(label5);
      tpWatchedFolder.Controls.Add(edZipPassword);
      tpWatchedFolder.Controls.Add(edEncryptZip);
      tpWatchedFolder.Controls.Add(btnSaveWatchedFolderChanges);
      tpWatchedFolder.Controls.Add(label4);
      tpWatchedFolder.Controls.Add(edFileFilter);
      tpWatchedFolder.Controls.Add(edIncludeSubFolders);
      tpWatchedFolder.Controls.Add(btnBrowseWatchedFolder);
      tpWatchedFolder.Controls.Add(edWatchedFolder);
      tpWatchedFolder.Controls.Add(label2);
      tpWatchedFolder.Location = new Point(4, 32);
      tpWatchedFolder.Name = "tpWatchedFolder";
      tpWatchedFolder.Padding = new Padding(3);
      tpWatchedFolder.Size = new Size(437, 456);
      tpWatchedFolder.TabIndex = 0;
      tpWatchedFolder.Text = "Watched";
      // 
      // edShowPassword
      // 
      edShowPassword.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      edShowPassword.AutoSize = true;
      edShowPassword.Location = new Point(362, 151);
      edShowPassword.Name = "edShowPassword";
      edShowPassword.Size = new Size(67, 24);
      edShowPassword.TabIndex = 13;
      edShowPassword.Text = "Show";
      edShowPassword.UseVisualStyleBackColor = true;
      edShowPassword.CheckedChanged += edShowPassword_CheckedChanged;
      // 
      // edMbPreZipSize
      // 
      edMbPreZipSize.Location = new Point(32, 196);
      edMbPreZipSize.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
      edMbPreZipSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
      edMbPreZipSize.Name = "edMbPreZipSize";
      edMbPreZipSize.Size = new Size(69, 27);
      edMbPreZipSize.TabIndex = 12;
      edMbPreZipSize.Value = new decimal(new int[] { 25, 0, 0, 0 });
      edMbPreZipSize.ValueChanged += edMbPreZipSize_ValueChanged;
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Location = new Point(10, 152);
      label6.Name = "label6";
      label6.Size = new Size(106, 20);
      label6.TabIndex = 11;
      label6.Text = ".szip Password:";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new Point(107, 198);
      label5.Name = "label5";
      label5.Size = new Size(218, 20);
      label5.TabIndex = 10;
      label5.Text = "Max Data Size before zip in MB";
      // 
      // edZipPassword
      // 
      edZipPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      edZipPassword.Location = new Point(119, 149);
      edZipPassword.Name = "edZipPassword";
      edZipPassword.Size = new Size(237, 27);
      edZipPassword.TabIndex = 8;
      edZipPassword.UseSystemPasswordChar = true;
      edZipPassword.TextChanged += edZipPassword_TextChanged;
      // 
      // edEncryptZip
      // 
      edEncryptZip.AutoSize = true;
      edEncryptZip.Location = new Point(7, 119);
      edEncryptZip.Name = "edEncryptZip";
      edEncryptZip.Size = new Size(152, 24);
      edEncryptZip.TabIndex = 7;
      edEncryptZip.Text = "Encrypt zip to szip";
      edEncryptZip.UseVisualStyleBackColor = true;
      edEncryptZip.CheckedChanged += edEncryptZip_CheckedChanged;
      // 
      // btnSaveWatchedFolderChanges
      // 
      btnSaveWatchedFolderChanges.Location = new Point(32, 263);
      btnSaveWatchedFolderChanges.Name = "btnSaveWatchedFolderChanges";
      btnSaveWatchedFolderChanges.Size = new Size(118, 29);
      btnSaveWatchedFolderChanges.TabIndex = 6;
      btnSaveWatchedFolderChanges.Text = "Save Changes";
      btnSaveWatchedFolderChanges.UseVisualStyleBackColor = true;
      btnSaveWatchedFolderChanges.Click += btnSaveWatchedFolderChanges_Click;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(7, 77);
      label4.Name = "label4";
      label4.Size = new Size(97, 20);
      label4.TabIndex = 5;
      label4.Text = "Include Filter:";
      // 
      // edFileFilter
      // 
      edFileFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      edFileFilter.Location = new Point(107, 74);
      edFileFilter.Name = "edFileFilter";
      edFileFilter.Size = new Size(322, 27);
      edFileFilter.TabIndex = 4;
      edFileFilter.TextChanged += edFileFilter_TextChanged;
      // 
      // edIncludeSubFolders
      // 
      edIncludeSubFolders.AutoSize = true;
      edIncludeSubFolders.Location = new Point(63, 43);
      edIncludeSubFolders.Name = "edIncludeSubFolders";
      edIncludeSubFolders.Size = new Size(163, 24);
      edIncludeSubFolders.TabIndex = 3;
      edIncludeSubFolders.Text = "Include subfolders? ";
      edIncludeSubFolders.UseVisualStyleBackColor = true;
      edIncludeSubFolders.CheckedChanged += edIncludeSubFolders_CheckedChanged;
      // 
      // btnBrowseWatchedFolder
      // 
      btnBrowseWatchedFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      btnBrowseWatchedFolder.Location = new Point(367, 9);
      btnBrowseWatchedFolder.Name = "btnBrowseWatchedFolder";
      btnBrowseWatchedFolder.Size = new Size(67, 29);
      btnBrowseWatchedFolder.TabIndex = 2;
      btnBrowseWatchedFolder.Text = "Browse";
      btnBrowseWatchedFolder.UseVisualStyleBackColor = true;
      btnBrowseWatchedFolder.Click += btnBrowseWatchedFolder_Click;
      // 
      // edWatchedFolder
      // 
      edWatchedFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      edWatchedFolder.Location = new Point(63, 10);
      edWatchedFolder.Name = "edWatchedFolder";
      edWatchedFolder.Size = new Size(302, 27);
      edWatchedFolder.TabIndex = 1;
      edWatchedFolder.TextChanged += edWatchedFolder_TextChanged;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(6, 13);
      label2.Name = "label2";
      label2.Size = new Size(51, 20);
      label2.TabIndex = 0;
      label2.Text = "Folder";
      // 
      // tpIndexItem
      // 
      tpIndexItem.Controls.Add(label3);
      tpIndexItem.Location = new Point(4, 32);
      tpIndexItem.Name = "tpIndexItem";
      tpIndexItem.Padding = new Padding(3);
      tpIndexItem.Size = new Size(437, 456);
      tpIndexItem.TabIndex = 1;
      tpIndexItem.Text = "Item";
      tpIndexItem.UseVisualStyleBackColor = true;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(58, 57);
      label3.Name = "label3";
      label3.Size = new Size(39, 20);
      label3.TabIndex = 0;
      label3.Text = "item";
      // 
      // tpEmpty
      // 
      tpEmpty.Controls.Add(label1);
      tpEmpty.Location = new Point(4, 32);
      tpEmpty.Name = "tpEmpty";
      tpEmpty.Size = new Size(437, 456);
      tpEmpty.TabIndex = 2;
      tpEmpty.Text = "Empty";
      tpEmpty.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(82, 58);
      label1.Name = "label1";
      label1.Size = new Size(132, 20);
      label1.TabIndex = 0;
      label1.Text = "select item to start";
      // 
      // tpChanges
      // 
      tpChanges.Controls.Add(splitContainer3);
      tpChanges.Location = new Point(4, 34);
      tpChanges.Name = "tpChanges";
      tpChanges.Size = new Size(825, 498);
      tpChanges.TabIndex = 2;
      tpChanges.Text = "Changes";
      tpChanges.UseVisualStyleBackColor = true;
      // 
      // splitContainer3
      // 
      splitContainer3.Dock = DockStyle.Fill;
      splitContainer3.Location = new Point(0, 0);
      splitContainer3.Name = "splitContainer3";
      // 
      // splitContainer3.Panel1
      // 
      splitContainer3.Panel1.Controls.Add(tvChanges);
      // 
      // splitContainer3.Panel2
      // 
      splitContainer3.Panel2.Controls.Add(tcChangesRight);
      splitContainer3.Size = new Size(825, 498);
      splitContainer3.SplitterDistance = 362;
      splitContainer3.TabIndex = 0;
      // 
      // tvChanges
      // 
      tvChanges.ContextMenuStrip = tvChangesMenuStrip;
      tvChanges.Dock = DockStyle.Fill;
      tvChanges.Location = new Point(0, 0);
      tvChanges.Name = "tvChanges";
      tvChanges.Size = new Size(362, 498);
      tvChanges.TabIndex = 0;
      // 
      // tvChangesMenuStrip
      // 
      tvChangesMenuStrip.ImageScalingSize = new Size(20, 20);
      tvChangesMenuStrip.Name = "tvChangesMenuStrip";
      tvChangesMenuStrip.Size = new Size(211, 32);
      tvChangesMenuStrip.Opening += tvChangesMenuStrip_Opening;
      // 
      // tcChangesRight
      // 
      tcChangesRight.Controls.Add(tpChangesFolder);
      tcChangesRight.Controls.Add(tpChangesItem);
      tcChangesRight.Controls.Add(tpChangesEmpty);
      tcChangesRight.Dock = DockStyle.Fill;
      tcChangesRight.Location = new Point(0, 0);
      tcChangesRight.Name = "tcChangesRight";
      tcChangesRight.SelectedIndex = 0;
      tcChangesRight.Size = new Size(459, 498);
      tcChangesRight.TabIndex = 0;
      // 
      // tpChangesFolder
      // 
      tpChangesFolder.Location = new Point(4, 29);
      tpChangesFolder.Name = "tpChangesFolder";
      tpChangesFolder.Padding = new Padding(3);
      tpChangesFolder.Size = new Size(451, 465);
      tpChangesFolder.TabIndex = 0;
      tpChangesFolder.Text = "Watched Folder";
      tpChangesFolder.UseVisualStyleBackColor = true;
      // 
      // tpChangesItem
      // 
      tpChangesItem.Location = new Point(4, 29);
      tpChangesItem.Name = "tpChangesItem";
      tpChangesItem.Padding = new Padding(3);
      tpChangesItem.Size = new Size(451, 465);
      tpChangesItem.TabIndex = 1;
      tpChangesItem.Text = "Item";
      tpChangesItem.UseVisualStyleBackColor = true;
      // 
      // tpChangesEmpty
      // 
      tpChangesEmpty.Location = new Point(4, 29);
      tpChangesEmpty.Name = "tpChangesEmpty";
      tpChangesEmpty.Size = new Size(451, 465);
      tpChangesEmpty.TabIndex = 2;
      tpChangesEmpty.Text = "Empty";
      tpChangesEmpty.UseVisualStyleBackColor = true;
      // 
      // tpVirtual
      // 
      tpVirtual.Location = new Point(4, 34);
      tpVirtual.Name = "tpVirtual";
      tpVirtual.Size = new Size(825, 498);
      tpVirtual.TabIndex = 3;
      tpVirtual.Text = "Virtual";
      tpVirtual.UseVisualStyleBackColor = true;
      // 
      // tpExitApp
      // 
      tpExitApp.BackColor = SystemColors.ButtonFace;
      tpExitApp.Location = new Point(4, 34);
      tpExitApp.Name = "tpExitApp";
      tpExitApp.Size = new Size(825, 498);
      tpExitApp.TabIndex = 4;
      tpExitApp.Text = "Exit App";
      // 
      // tcBottom
      // 
      tcBottom.Appearance = TabAppearance.FlatButtons;
      tcBottom.Controls.Add(tpErrors);
      tcBottom.Controls.Add(tpWatchedStatus);
      tcBottom.Controls.Add(tpHideTabs);
      tcBottom.Dock = DockStyle.Fill;
      tcBottom.Location = new Point(0, 3);
      tcBottom.Name = "tcBottom";
      tcBottom.SelectedIndex = 0;
      tcBottom.Size = new Size(833, 176);
      tcBottom.TabIndex = 0;
      tcBottom.SelectedIndexChanged += tcBottom_SelectedIndexChanged;
      // 
      // tpErrors
      // 
      tpErrors.Controls.Add(edLog);
      tpErrors.Location = new Point(4, 32);
      tpErrors.Name = "tpErrors";
      tpErrors.Padding = new Padding(3);
      tpErrors.Size = new Size(825, 140);
      tpErrors.TabIndex = 0;
      tpErrors.Text = "Error Log";
      tpErrors.UseVisualStyleBackColor = true;
      // 
      // edLog
      // 
      edLog.Dock = DockStyle.Fill;
      edLog.Location = new Point(3, 3);
      edLog.Multiline = true;
      edLog.Name = "edLog";
      edLog.Size = new Size(819, 134);
      edLog.TabIndex = 0;
      // 
      // tpWatchedStatus
      // 
      tpWatchedStatus.Location = new Point(4, 32);
      tpWatchedStatus.Name = "tpWatchedStatus";
      tpWatchedStatus.Padding = new Padding(3);
      tpWatchedStatus.Size = new Size(825, 140);
      tpWatchedStatus.TabIndex = 1;
      tpWatchedStatus.Text = "Portal Status";
      tpWatchedStatus.UseVisualStyleBackColor = true;
      // 
      // tpHideTabs
      // 
      tpHideTabs.Location = new Point(4, 32);
      tpHideTabs.Name = "tpHideTabs";
      tpHideTabs.Size = new Size(825, 140);
      tpHideTabs.TabIndex = 2;
      tpHideTabs.Text = "Hide";
      tpHideTabs.UseVisualStyleBackColor = true;
      // 
      // openFileDialog1
      // 
      openFileDialog1.FileName = "openFileDialog1";
      // 
      // BrowseFolderDialog
      // 
      BrowseFolderDialog.Description = "Select folder to watch.";
      BrowseFolderDialog.ShowHiddenFiles = true;
      BrowseFolderDialog.UseDescriptionForTitle = true;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(8F, 20F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(833, 725);
      Controls.Add(splitContainer1);
      Icon = (Icon)resources.GetObject("$this.Icon");
      Name = "Form1";
      Text = "ChevronCoil";
      FormClosing += Form1_FormClosing;
      Load += Form1_Load;
      splitContainer1.Panel1.ResumeLayout(false);
      splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
      splitContainer1.ResumeLayout(false);
      tcRootMain.ResumeLayout(false);
      tpSetup.ResumeLayout(false);
      tpSetup.PerformLayout();
      tpWatching.ResumeLayout(false);
      splitContainer2.Panel1.ResumeLayout(false);
      splitContainer2.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
      splitContainer2.ResumeLayout(false);
      tvWatchingMenuStrip.ResumeLayout(false);
      tcWatchingRight.ResumeLayout(false);
      tpWatchedFolder.ResumeLayout(false);
      tpWatchedFolder.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)edMbPreZipSize).EndInit();
      tpIndexItem.ResumeLayout(false);
      tpIndexItem.PerformLayout();
      tpEmpty.ResumeLayout(false);
      tpEmpty.PerformLayout();
      tpChanges.ResumeLayout(false);
      splitContainer3.Panel1.ResumeLayout(false);
      splitContainer3.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
      splitContainer3.ResumeLayout(false);
      tcChangesRight.ResumeLayout(false);
      tcBottom.ResumeLayout(false);
      tpErrors.ResumeLayout(false);
      tpErrors.PerformLayout();
      ResumeLayout(false);
    }

    #endregion

    private SplitContainer splitContainer1;
    private TabControl tcRootMain;
    private TabPage tpSetup;
    private TabPage tpWatching;
    private TabPage tpChanges;
    private TabPage tpVirtual;
    private SplitContainer splitContainer2;
    private TreeView tvWatching;
    private Label lbWatchConfigLocation;
    private Label lbDefaultDir;
    private Button button1;
    private ContextMenuStrip tvWatchingMenuStrip;
    private ToolStripMenuItem addWatchedFolderToolStripMenuItem;
    private ToolStripMenuItem removeWatchedFolderToolStripMenuItem;
    private TabControl tcBottom;
    private TabPage tpErrors;
    private TabPage tpWatchedStatus;
    private TextBox edLog;
    private ToolStripMenuItem resyncFolderToolStripMenuItem;
    private OpenFileDialog openFileDialog1;
    private TabPage tpHideTabs;
    private TabPage tpExitApp;
    private TabControl tcWatchingRight;
    private TabPage tpWatchedFolder;
    private TabPage tpIndexItem;
    private Label label2;
    private Label label3;
    private TabPage tpEmpty;
    private Label label1;
    private TextBox edWatchedFolder;
    private CheckBox edIncludeSubFolders;
    private Button btnBrowseWatchedFolder;
    private FolderBrowserDialog BrowseFolderDialog;
    private Label label4;
    private TextBox edFileFilter;
    private Button btnSaveWatchedFolderChanges;
    private Label label5;    
    private TextBox edZipPassword;
    private CheckBox edEncryptZip;
    private Label label6;
    private NumericUpDown edMbPreZipSize;
    private CheckBox edShowPassword;
    private SplitContainer splitContainer3;
    private TreeView tvChanges;
    private TabControl tcChangesRight;
    private TabPage tpChangesFolder;
    private TabPage tpChangesItem;
    private TabPage tpChangesEmpty;
    private ContextMenuStrip tvChangesMenuStrip;
  }
}
