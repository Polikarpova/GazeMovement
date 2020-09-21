namespace GazeMovementClient.View
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStripMenuItem showObjClassesAnalisysMenuItem;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.startButton = new System.Windows.Forms.Button();
            this.upperTab = new System.Windows.Forms.TabControl();
            this.experimentTabPage = new System.Windows.Forms.TabPage();
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.isWithPause = new System.Windows.Forms.CheckBox();
            this.sessionName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.selectImagesButton = new System.Windows.Forms.Button();
            this.durationNum = new System.Windows.Forms.NumericUpDown();
            this.durationLabel = new System.Windows.Forms.Label();
            this.eyePathsTabPage = new System.Windows.Forms.TabPage();
            this.gazePlotButton = new System.Windows.Forms.Button();
            this.blocksButton = new System.Windows.Forms.Button();
            this.deleteEyePathButton = new System.Windows.Forms.Button();
            this.eyePathDataList = new System.Windows.Forms.ListBox();
            this.sessionsTabPage = new System.Windows.Forms.TabPage();
            this.deleteSessionButton = new System.Windows.Forms.Button();
            this.commonPaternsButton = new System.Windows.Forms.Button();
            this.sessionsList = new System.Windows.Forms.ListBox();
            this.imageLibraryGroupBox = new System.Windows.Forms.GroupBox();
            this.showObjectsButton = new System.Windows.Forms.Button();
            this.deleteAllObjectsButton = new System.Windows.Forms.Button();
            this.addNewObjectButton = new System.Windows.Forms.Button();
            this.deleteImageButton = new System.Windows.Forms.Button();
            this.downloadImageButton = new System.Windows.Forms.Button();
            this.imageNameList = new System.Windows.Forms.ListBox();
            this.image = new System.Windows.Forms.PictureBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.analisysMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imagesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetImageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadImageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadImagesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showObjClassesAnalisysMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upperTab.SuspendLayout();
            this.experimentTabPage.SuspendLayout();
            this.settingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.durationNum)).BeginInit();
            this.eyePathsTabPage.SuspendLayout();
            this.sessionsTabPage.SuspendLayout();
            this.imageLibraryGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.image)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // showObjClassesAnalisysMenuItem
            // 
            showObjClassesAnalisysMenuItem.Name = "showObjClassesAnalisysMenuItem";
            resources.ApplyResources(showObjClassesAnalisysMenuItem, "showObjClassesAnalisysMenuItem");
            showObjClassesAnalisysMenuItem.Click += new System.EventHandler(this.ShowObjClassesAnalisysMenuItem_Click);
            // 
            // startButton
            // 
            resources.ApplyResources(this.startButton, "startButton");
            this.startButton.Name = "startButton";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // upperTab
            // 
            this.upperTab.Controls.Add(this.experimentTabPage);
            this.upperTab.Controls.Add(this.eyePathsTabPage);
            this.upperTab.Controls.Add(this.sessionsTabPage);
            resources.ApplyResources(this.upperTab, "upperTab");
            this.upperTab.Name = "upperTab";
            this.upperTab.SelectedIndex = 0;
            // 
            // experimentTabPage
            // 
            resources.ApplyResources(this.experimentTabPage, "experimentTabPage");
            this.experimentTabPage.Controls.Add(this.settingsGroupBox);
            this.experimentTabPage.Controls.Add(this.startButton);
            this.experimentTabPage.Name = "experimentTabPage";
            this.experimentTabPage.UseVisualStyleBackColor = true;
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Controls.Add(this.isWithPause);
            this.settingsGroupBox.Controls.Add(this.sessionName);
            this.settingsGroupBox.Controls.Add(this.label2);
            this.settingsGroupBox.Controls.Add(this.label1);
            this.settingsGroupBox.Controls.Add(this.selectImagesButton);
            this.settingsGroupBox.Controls.Add(this.durationNum);
            this.settingsGroupBox.Controls.Add(this.durationLabel);
            resources.ApplyResources(this.settingsGroupBox, "settingsGroupBox");
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.TabStop = false;
            // 
            // isWithPause
            // 
            resources.ApplyResources(this.isWithPause, "isWithPause");
            this.isWithPause.Name = "isWithPause";
            this.isWithPause.UseVisualStyleBackColor = true;
            // 
            // sessionName
            // 
            resources.ApplyResources(this.sessionName, "sessionName");
            this.sessionName.Name = "sessionName";
            this.sessionName.TextChanged += new System.EventHandler(this.SessionName_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // selectImagesButton
            // 
            resources.ApplyResources(this.selectImagesButton, "selectImagesButton");
            this.selectImagesButton.Name = "selectImagesButton";
            this.selectImagesButton.UseVisualStyleBackColor = true;
            this.selectImagesButton.Click += new System.EventHandler(this.SelectImagesButton_Click);
            // 
            // durationNum
            // 
            resources.ApplyResources(this.durationNum, "durationNum");
            this.durationNum.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.durationNum.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.durationNum.Name = "durationNum";
            this.durationNum.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // durationLabel
            // 
            resources.ApplyResources(this.durationLabel, "durationLabel");
            this.durationLabel.Name = "durationLabel";
            // 
            // eyePathsTabPage
            // 
            this.eyePathsTabPage.Controls.Add(this.gazePlotButton);
            this.eyePathsTabPage.Controls.Add(this.blocksButton);
            this.eyePathsTabPage.Controls.Add(this.deleteEyePathButton);
            this.eyePathsTabPage.Controls.Add(this.eyePathDataList);
            resources.ApplyResources(this.eyePathsTabPage, "eyePathsTabPage");
            this.eyePathsTabPage.Name = "eyePathsTabPage";
            this.eyePathsTabPage.UseVisualStyleBackColor = true;
            // 
            // gazePlotButton
            // 
            resources.ApplyResources(this.gazePlotButton, "gazePlotButton");
            this.gazePlotButton.Name = "gazePlotButton";
            this.gazePlotButton.UseVisualStyleBackColor = true;
            this.gazePlotButton.Click += new System.EventHandler(this.GazePlotButton_Click);
            // 
            // blocksButton
            // 
            resources.ApplyResources(this.blocksButton, "blocksButton");
            this.blocksButton.Name = "blocksButton";
            this.blocksButton.UseVisualStyleBackColor = true;
            this.blocksButton.Click += new System.EventHandler(this.BlocksButton_Click);
            // 
            // deleteEyePathButton
            // 
            resources.ApplyResources(this.deleteEyePathButton, "deleteEyePathButton");
            this.deleteEyePathButton.Name = "deleteEyePathButton";
            this.deleteEyePathButton.UseVisualStyleBackColor = true;
            this.deleteEyePathButton.Click += new System.EventHandler(this.DeleteEyePathButton_Click);
            // 
            // eyePathDataList
            // 
            this.eyePathDataList.FormattingEnabled = true;
            resources.ApplyResources(this.eyePathDataList, "eyePathDataList");
            this.eyePathDataList.Name = "eyePathDataList";
            this.eyePathDataList.SelectedIndexChanged += new System.EventHandler(this.EyePathDataList_SelectedIndexChanged);
            // 
            // sessionsTabPage
            // 
            this.sessionsTabPage.Controls.Add(this.deleteSessionButton);
            this.sessionsTabPage.Controls.Add(this.commonPaternsButton);
            this.sessionsTabPage.Controls.Add(this.sessionsList);
            resources.ApplyResources(this.sessionsTabPage, "sessionsTabPage");
            this.sessionsTabPage.Name = "sessionsTabPage";
            this.sessionsTabPage.UseVisualStyleBackColor = true;
            // 
            // deleteSessionButton
            // 
            resources.ApplyResources(this.deleteSessionButton, "deleteSessionButton");
            this.deleteSessionButton.Name = "deleteSessionButton";
            this.deleteSessionButton.UseVisualStyleBackColor = true;
            this.deleteSessionButton.Click += new System.EventHandler(this.DeleteSessionButton_Click);
            // 
            // commonPaternsButton
            // 
            resources.ApplyResources(this.commonPaternsButton, "commonPaternsButton");
            this.commonPaternsButton.Name = "commonPaternsButton";
            this.commonPaternsButton.UseVisualStyleBackColor = true;
            this.commonPaternsButton.Click += new System.EventHandler(this.CommonPaternsButton_Click);
            // 
            // sessionsList
            // 
            this.sessionsList.FormattingEnabled = true;
            resources.ApplyResources(this.sessionsList, "sessionsList");
            this.sessionsList.Name = "sessionsList";
            this.sessionsList.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.sessionsList.SelectedIndexChanged += new System.EventHandler(this.SessionsList_SelectedIndexChanged);
            // 
            // imageLibraryGroupBox
            // 
            this.imageLibraryGroupBox.Controls.Add(this.showObjectsButton);
            this.imageLibraryGroupBox.Controls.Add(this.deleteAllObjectsButton);
            this.imageLibraryGroupBox.Controls.Add(this.addNewObjectButton);
            this.imageLibraryGroupBox.Controls.Add(this.deleteImageButton);
            this.imageLibraryGroupBox.Controls.Add(this.downloadImageButton);
            this.imageLibraryGroupBox.Controls.Add(this.imageNameList);
            resources.ApplyResources(this.imageLibraryGroupBox, "imageLibraryGroupBox");
            this.imageLibraryGroupBox.Name = "imageLibraryGroupBox";
            this.imageLibraryGroupBox.TabStop = false;
            // 
            // showObjectsButton
            // 
            resources.ApplyResources(this.showObjectsButton, "showObjectsButton");
            this.showObjectsButton.Name = "showObjectsButton";
            this.showObjectsButton.UseVisualStyleBackColor = true;
            this.showObjectsButton.Click += new System.EventHandler(this.ShowObjectsButton_Click);
            // 
            // deleteAllObjectsButton
            // 
            resources.ApplyResources(this.deleteAllObjectsButton, "deleteAllObjectsButton");
            this.deleteAllObjectsButton.Name = "deleteAllObjectsButton";
            this.deleteAllObjectsButton.UseVisualStyleBackColor = true;
            this.deleteAllObjectsButton.Click += new System.EventHandler(this.DeleteAllObjectsButton_Click);
            // 
            // addNewObjectButton
            // 
            resources.ApplyResources(this.addNewObjectButton, "addNewObjectButton");
            this.addNewObjectButton.Name = "addNewObjectButton";
            this.addNewObjectButton.UseVisualStyleBackColor = true;
            this.addNewObjectButton.Click += new System.EventHandler(this.AddNewObjectButton_Click);
            // 
            // deleteImageButton
            // 
            resources.ApplyResources(this.deleteImageButton, "deleteImageButton");
            this.deleteImageButton.Name = "deleteImageButton";
            this.deleteImageButton.UseVisualStyleBackColor = true;
            this.deleteImageButton.Click += new System.EventHandler(this.DeleteImageButton_Click);
            // 
            // downloadImageButton
            // 
            resources.ApplyResources(this.downloadImageButton, "downloadImageButton");
            this.downloadImageButton.Name = "downloadImageButton";
            this.downloadImageButton.UseVisualStyleBackColor = true;
            this.downloadImageButton.Click += new System.EventHandler(this.DownloadImageButton_Click);
            // 
            // imageNameList
            // 
            this.imageNameList.FormattingEnabled = true;
            resources.ApplyResources(this.imageNameList, "imageNameList");
            this.imageNameList.Name = "imageNameList";
            this.imageNameList.SelectedIndexChanged += new System.EventHandler(this.ImageNameList_SelectedIndexChanged);
            // 
            // image
            // 
            resources.ApplyResources(this.image, "image");
            this.image.Name = "image";
            this.image.TabStop = false;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.analisysMenuItem,
            this.imagesMenuItem});
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.Name = "menuStrip";
            // 
            // analisysMenuItem
            // 
            this.analisysMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            showObjClassesAnalisysMenuItem});
            this.analisysMenuItem.Name = "analisysMenuItem";
            resources.ApplyResources(this.analisysMenuItem, "analisysMenuItem");
            // 
            // imagesMenuItem
            // 
            this.imagesMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetImageMenuItem,
            this.downloadImageMenuItem,
            this.downloadImagesMenuItem});
            this.imagesMenuItem.Name = "imagesMenuItem";
            resources.ApplyResources(this.imagesMenuItem, "imagesMenuItem");
            // 
            // resetImageMenuItem
            // 
            this.resetImageMenuItem.Name = "resetImageMenuItem";
            resources.ApplyResources(this.resetImageMenuItem, "resetImageMenuItem");
            this.resetImageMenuItem.Click += new System.EventHandler(this.ResetImageMenuItem_Click);
            // 
            // downloadImageMenuItem
            // 
            this.downloadImageMenuItem.Name = "downloadImageMenuItem";
            resources.ApplyResources(this.downloadImageMenuItem, "downloadImageMenuItem");
            this.downloadImageMenuItem.Click += new System.EventHandler(this.DownloadImageButton_Click);
            // 
            // downloadImagesMenuItem
            // 
            this.downloadImagesMenuItem.Name = "downloadImagesMenuItem";
            resources.ApplyResources(this.downloadImagesMenuItem, "downloadImagesMenuItem");
            this.downloadImagesMenuItem.Click += new System.EventHandler(this.DownloadImagesMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.imageLibraryGroupBox);
            this.Controls.Add(this.upperTab);
            this.Controls.Add(this.image);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.upperTab.ResumeLayout(false);
            this.experimentTabPage.ResumeLayout(false);
            this.settingsGroupBox.ResumeLayout(false);
            this.settingsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.durationNum)).EndInit();
            this.eyePathsTabPage.ResumeLayout(false);
            this.sessionsTabPage.ResumeLayout(false);
            this.imageLibraryGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.image)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.PictureBox image;
        private System.Windows.Forms.TabControl upperTab;
        private System.Windows.Forms.TabPage eyePathsTabPage;
        private System.Windows.Forms.TabPage experimentTabPage;
        private System.Windows.Forms.GroupBox imageLibraryGroupBox;
        private System.Windows.Forms.ListBox imageNameList;
        private System.Windows.Forms.Button deleteImageButton;
        private System.Windows.Forms.Button downloadImageButton;
        private System.Windows.Forms.GroupBox settingsGroupBox;
        private System.Windows.Forms.Label durationLabel;
        private System.Windows.Forms.NumericUpDown durationNum;
        private System.Windows.Forms.Button addNewObjectButton;
        private System.Windows.Forms.Button deleteAllObjectsButton;
        private System.Windows.Forms.Button showObjectsButton;
        private System.Windows.Forms.ListBox eyePathDataList;
        private System.Windows.Forms.Button deleteEyePathButton;
        private System.Windows.Forms.Button blocksButton;
        private System.Windows.Forms.Button gazePlotButton;
        private System.Windows.Forms.Button commonPaternsButton;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem analisysMenuItem;
        private System.Windows.Forms.Button selectImagesButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox sessionName;
        private System.Windows.Forms.TabPage sessionsTabPage;
        private System.Windows.Forms.ListBox sessionsList;
        private System.Windows.Forms.Button deleteSessionButton;
        private System.Windows.Forms.CheckBox isWithPause;
        private System.Windows.Forms.ToolStripMenuItem imagesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetImageMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadImageMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadImagesMenuItem;
    }
}

