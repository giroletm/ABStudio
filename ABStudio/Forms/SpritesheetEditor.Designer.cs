
namespace ABStudio.Forms
{
    partial class SpritesheetEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpritesheetEditor));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openInNewInstanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importSpritesheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importSpritesFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSpritesheetImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCroppedSpritesheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllSpritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesGroupBox = new System.Windows.Forms.GroupBox();
            this.selectedSpriteGroupBox = new System.Windows.Forms.GroupBox();
            this.spriteOrigPtMapLabel = new System.Windows.Forms.Label();
            this.spriteOrigPtMapBRButton = new System.Windows.Forms.Button();
            this.spriteOrigPtMapBButton = new System.Windows.Forms.Button();
            this.spriteOrigPtMapBLButton = new System.Windows.Forms.Button();
            this.spriteOrigPtMapTRButton = new System.Windows.Forms.Button();
            this.spriteOrigPtMapTButton = new System.Windows.Forms.Button();
            this.spriteOrigPtMapTLButton = new System.Windows.Forms.Button();
            this.spriteOrigPtMapRButton = new System.Windows.Forms.Button();
            this.spriteOrigPtMapCButton = new System.Windows.Forms.Button();
            this.spriteOrigPtMapLButton = new System.Windows.Forms.Button();
            this.spriteOrigPtLabel = new System.Windows.Forms.Label();
            this.spriteOrigPtYLabel = new System.Windows.Forms.Label();
            this.spriteOrigPtXLabel = new System.Windows.Forms.Label();
            this.spriteOrigPtYNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.spriteOrigPtXNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.spriteRectHLabel = new System.Windows.Forms.Label();
            this.spriteRectWLabel = new System.Windows.Forms.Label();
            this.spriteRectHNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.spriteRectWNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.spriteRectLabel = new System.Windows.Forms.Label();
            this.spriteRectYLabel = new System.Windows.Forms.Label();
            this.spriteRectXLabel = new System.Windows.Forms.Label();
            this.spriteRectYNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.spriteRectXNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.spriteNameLabel = new System.Windows.Forms.Label();
            this.spriteNameTextBox = new System.Windows.Forms.TextBox();
            this.reloadPictureButton = new System.Windows.Forms.Button();
            this.filenameLabel = new System.Windows.Forms.Label();
            this.filenameTextBox = new System.Windows.Forms.TextBox();
            this.imageViewPanel = new ABStudio.Controls.CustomPanel();
            this.insideViewPanel = new System.Windows.Forms.Panel();
            this.spritesheetPictureBox = new ABStudio.Controls.PictureBoxDB();
            this.menuStrip1.SuspendLayout();
            this.propertiesGroupBox.SuspendLayout();
            this.selectedSpriteGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spriteOrigPtYNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spriteOrigPtXNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spriteRectHNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spriteRectWNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spriteRectYNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spriteRectXNumUpDown)).BeginInit();
            this.imageViewPanel.SuspendLayout();
            this.insideViewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spritesheetPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(931, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openHereToolStripMenuItem,
            this.openInNewInstanceToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.openToolStripMenuItem.Text = "Open...";
            // 
            // openHereToolStripMenuItem
            // 
            this.openHereToolStripMenuItem.Name = "openHereToolStripMenuItem";
            this.openHereToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openHereToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.openHereToolStripMenuItem.Text = "Here";
            this.openHereToolStripMenuItem.Click += new System.EventHandler(this.openHereToolStripMenuItem_Click);
            // 
            // openInNewInstanceToolStripMenuItem
            // 
            this.openInNewInstanceToolStripMenuItem.Name = "openInNewInstanceToolStripMenuItem";
            this.openInNewInstanceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.openInNewInstanceToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.openInNewInstanceToolStripMenuItem.Text = "In new instance";
            this.openInNewInstanceToolStripMenuItem.Click += new System.EventHandler(this.openInNewInstanceToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(188, 6);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importSpritesheetToolStripMenuItem,
            this.importSpritesFolderToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.importToolStripMenuItem.Text = "Import...";
            // 
            // importSpritesheetToolStripMenuItem
            // 
            this.importSpritesheetToolStripMenuItem.Name = "importSpritesheetToolStripMenuItem";
            this.importSpritesheetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.importSpritesheetToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.importSpritesheetToolStripMenuItem.Text = "Spritesheet";
            this.importSpritesheetToolStripMenuItem.Click += new System.EventHandler(this.importSpritesheetToolStripMenuItem_Click);
            // 
            // importSpritesFolderToolStripMenuItem
            // 
            this.importSpritesFolderToolStripMenuItem.Name = "importSpritesFolderToolStripMenuItem";
            this.importSpritesFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.I)));
            this.importSpritesFolderToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.importSpritesFolderToolStripMenuItem.Text = "Sprites folder";
            this.importSpritesFolderToolStripMenuItem.Click += new System.EventHandler(this.importSpritesFolderToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportSpritesheetImageToolStripMenuItem,
            this.exportCroppedSpritesheetToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.exportToolStripMenuItem.Text = "Export...";
            // 
            // exportSpritesheetImageToolStripMenuItem
            // 
            this.exportSpritesheetImageToolStripMenuItem.Name = "exportSpritesheetImageToolStripMenuItem";
            this.exportSpritesheetImageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exportSpritesheetImageToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.exportSpritesheetImageToolStripMenuItem.Text = "Spritesheet to image";
            this.exportSpritesheetImageToolStripMenuItem.Click += new System.EventHandler(this.exportSpritesheetImageToolStripMenuItem_Click);
            // 
            // exportCroppedSpritesheetToolStripMenuItem
            // 
            this.exportCroppedSpritesheetToolStripMenuItem.Name = "exportCroppedSpritesheetToolStripMenuItem";
            this.exportCroppedSpritesheetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.E)));
            this.exportCroppedSpritesheetToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.exportCroppedSpritesheetToolStripMenuItem.Text = "Cropped spritesheet";
            this.exportCroppedSpritesheetToolStripMenuItem.Click += new System.EventHandler(this.exportCroppedSpritesheetToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(188, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearAllSpritesToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // clearAllSpritesToolStripMenuItem
            // 
            this.clearAllSpritesToolStripMenuItem.Name = "clearAllSpritesToolStripMenuItem";
            this.clearAllSpritesToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.clearAllSpritesToolStripMenuItem.Text = "Clear all sprites";
            this.clearAllSpritesToolStripMenuItem.Click += new System.EventHandler(this.clearAllSpritesToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // propertiesGroupBox
            // 
            this.propertiesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertiesGroupBox.Controls.Add(this.selectedSpriteGroupBox);
            this.propertiesGroupBox.Controls.Add(this.reloadPictureButton);
            this.propertiesGroupBox.Controls.Add(this.filenameLabel);
            this.propertiesGroupBox.Controls.Add(this.filenameTextBox);
            this.propertiesGroupBox.Location = new System.Drawing.Point(547, 27);
            this.propertiesGroupBox.Name = "propertiesGroupBox";
            this.propertiesGroupBox.Size = new System.Drawing.Size(372, 503);
            this.propertiesGroupBox.TabIndex = 2;
            this.propertiesGroupBox.TabStop = false;
            this.propertiesGroupBox.Text = "Properties";
            // 
            // selectedSpriteGroupBox
            // 
            this.selectedSpriteGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedSpriteGroupBox.Controls.Add(this.spriteOrigPtMapLabel);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteOrigPtMapBRButton);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteOrigPtMapBButton);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteOrigPtMapBLButton);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteOrigPtMapTRButton);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteOrigPtMapTButton);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteOrigPtMapTLButton);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteOrigPtMapRButton);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteOrigPtMapCButton);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteOrigPtMapLButton);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteOrigPtLabel);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteOrigPtYLabel);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteOrigPtXLabel);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteOrigPtYNumUpDown);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteOrigPtXNumUpDown);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteRectHLabel);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteRectWLabel);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteRectHNumUpDown);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteRectWNumUpDown);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteRectLabel);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteRectYLabel);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteRectXLabel);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteRectYNumUpDown);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteRectXNumUpDown);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteNameLabel);
            this.selectedSpriteGroupBox.Controls.Add(this.spriteNameTextBox);
            this.selectedSpriteGroupBox.Location = new System.Drawing.Point(9, 65);
            this.selectedSpriteGroupBox.Name = "selectedSpriteGroupBox";
            this.selectedSpriteGroupBox.Size = new System.Drawing.Size(357, 432);
            this.selectedSpriteGroupBox.TabIndex = 4;
            this.selectedSpriteGroupBox.TabStop = false;
            this.selectedSpriteGroupBox.Text = "Selected sprite";
            // 
            // spriteOrigPtMapLabel
            // 
            this.spriteOrigPtMapLabel.AutoSize = true;
            this.spriteOrigPtMapLabel.Location = new System.Drawing.Point(229, 162);
            this.spriteOrigPtMapLabel.Name = "spriteOrigPtMapLabel";
            this.spriteOrigPtMapLabel.Size = new System.Drawing.Size(31, 13);
            this.spriteOrigPtMapLabel.TabIndex = 26;
            this.spriteOrigPtMapLabel.Text = "Map:";
            // 
            // spriteOrigPtMapBRButton
            // 
            this.spriteOrigPtMapBRButton.Location = new System.Drawing.Point(323, 215);
            this.spriteOrigPtMapBRButton.Name = "spriteOrigPtMapBRButton";
            this.spriteOrigPtMapBRButton.Size = new System.Drawing.Size(23, 23);
            this.spriteOrigPtMapBRButton.TabIndex = 25;
            this.spriteOrigPtMapBRButton.Text = "↘";
            this.spriteOrigPtMapBRButton.UseVisualStyleBackColor = true;
            this.spriteOrigPtMapBRButton.Click += new System.EventHandler(this.spriteOrigPtMapBRButton_Click);
            // 
            // spriteOrigPtMapBButton
            // 
            this.spriteOrigPtMapBButton.Location = new System.Drawing.Point(294, 215);
            this.spriteOrigPtMapBButton.Name = "spriteOrigPtMapBButton";
            this.spriteOrigPtMapBButton.Size = new System.Drawing.Size(23, 23);
            this.spriteOrigPtMapBButton.TabIndex = 24;
            this.spriteOrigPtMapBButton.Text = "↓";
            this.spriteOrigPtMapBButton.UseVisualStyleBackColor = true;
            this.spriteOrigPtMapBButton.Click += new System.EventHandler(this.spriteOrigPtMapBButton_Click);
            // 
            // spriteOrigPtMapBLButton
            // 
            this.spriteOrigPtMapBLButton.Location = new System.Drawing.Point(265, 215);
            this.spriteOrigPtMapBLButton.Name = "spriteOrigPtMapBLButton";
            this.spriteOrigPtMapBLButton.Size = new System.Drawing.Size(23, 23);
            this.spriteOrigPtMapBLButton.TabIndex = 23;
            this.spriteOrigPtMapBLButton.Text = "↙";
            this.spriteOrigPtMapBLButton.UseVisualStyleBackColor = true;
            this.spriteOrigPtMapBLButton.Click += new System.EventHandler(this.spriteOrigPtMapBLButton_Click);
            // 
            // spriteOrigPtMapTRButton
            // 
            this.spriteOrigPtMapTRButton.Location = new System.Drawing.Point(323, 157);
            this.spriteOrigPtMapTRButton.Name = "spriteOrigPtMapTRButton";
            this.spriteOrigPtMapTRButton.Size = new System.Drawing.Size(23, 23);
            this.spriteOrigPtMapTRButton.TabIndex = 22;
            this.spriteOrigPtMapTRButton.Text = "↗";
            this.spriteOrigPtMapTRButton.UseVisualStyleBackColor = true;
            this.spriteOrigPtMapTRButton.Click += new System.EventHandler(this.spriteOrigPtMapTRButton_Click);
            // 
            // spriteOrigPtMapTButton
            // 
            this.spriteOrigPtMapTButton.Location = new System.Drawing.Point(294, 157);
            this.spriteOrigPtMapTButton.Name = "spriteOrigPtMapTButton";
            this.spriteOrigPtMapTButton.Size = new System.Drawing.Size(23, 23);
            this.spriteOrigPtMapTButton.TabIndex = 21;
            this.spriteOrigPtMapTButton.Text = "↑";
            this.spriteOrigPtMapTButton.UseVisualStyleBackColor = true;
            this.spriteOrigPtMapTButton.Click += new System.EventHandler(this.spriteOrigPtMapTButton_Click);
            // 
            // spriteOrigPtMapTLButton
            // 
            this.spriteOrigPtMapTLButton.Location = new System.Drawing.Point(265, 157);
            this.spriteOrigPtMapTLButton.Name = "spriteOrigPtMapTLButton";
            this.spriteOrigPtMapTLButton.Size = new System.Drawing.Size(23, 23);
            this.spriteOrigPtMapTLButton.TabIndex = 20;
            this.spriteOrigPtMapTLButton.Text = "↖";
            this.spriteOrigPtMapTLButton.UseVisualStyleBackColor = true;
            this.spriteOrigPtMapTLButton.Click += new System.EventHandler(this.spriteOrigPtMapTLButton_Click);
            // 
            // spriteOrigPtMapRButton
            // 
            this.spriteOrigPtMapRButton.Location = new System.Drawing.Point(323, 186);
            this.spriteOrigPtMapRButton.Name = "spriteOrigPtMapRButton";
            this.spriteOrigPtMapRButton.Size = new System.Drawing.Size(23, 23);
            this.spriteOrigPtMapRButton.TabIndex = 19;
            this.spriteOrigPtMapRButton.Text = "→ ";
            this.spriteOrigPtMapRButton.UseVisualStyleBackColor = true;
            this.spriteOrigPtMapRButton.Click += new System.EventHandler(this.spriteOrigPtMapRButton_Click);
            // 
            // spriteOrigPtMapCButton
            // 
            this.spriteOrigPtMapCButton.Location = new System.Drawing.Point(294, 186);
            this.spriteOrigPtMapCButton.Name = "spriteOrigPtMapCButton";
            this.spriteOrigPtMapCButton.Size = new System.Drawing.Size(23, 23);
            this.spriteOrigPtMapCButton.TabIndex = 18;
            this.spriteOrigPtMapCButton.Text = "•";
            this.spriteOrigPtMapCButton.UseVisualStyleBackColor = true;
            this.spriteOrigPtMapCButton.Click += new System.EventHandler(this.spriteOrigPtMapCButton_Click);
            // 
            // spriteOrigPtMapLButton
            // 
            this.spriteOrigPtMapLButton.Location = new System.Drawing.Point(265, 186);
            this.spriteOrigPtMapLButton.Name = "spriteOrigPtMapLButton";
            this.spriteOrigPtMapLButton.Size = new System.Drawing.Size(23, 23);
            this.spriteOrigPtMapLButton.TabIndex = 17;
            this.spriteOrigPtMapLButton.Text = "←";
            this.spriteOrigPtMapLButton.UseVisualStyleBackColor = true;
            this.spriteOrigPtMapLButton.Click += new System.EventHandler(this.spriteOrigPtMapLButton_Click);
            // 
            // spriteOrigPtLabel
            // 
            this.spriteOrigPtLabel.AutoSize = true;
            this.spriteOrigPtLabel.Location = new System.Drawing.Point(6, 133);
            this.spriteOrigPtLabel.Name = "spriteOrigPtLabel";
            this.spriteOrigPtLabel.Size = new System.Drawing.Size(63, 13);
            this.spriteOrigPtLabel.TabIndex = 16;
            this.spriteOrigPtLabel.Text = "Origin point:";
            // 
            // spriteOrigPtYLabel
            // 
            this.spriteOrigPtYLabel.AutoSize = true;
            this.spriteOrigPtYLabel.Location = new System.Drawing.Point(243, 133);
            this.spriteOrigPtYLabel.Name = "spriteOrigPtYLabel";
            this.spriteOrigPtYLabel.Size = new System.Drawing.Size(17, 13);
            this.spriteOrigPtYLabel.TabIndex = 15;
            this.spriteOrigPtYLabel.Text = "Y:";
            // 
            // spriteOrigPtXLabel
            // 
            this.spriteOrigPtXLabel.AutoSize = true;
            this.spriteOrigPtXLabel.Location = new System.Drawing.Point(105, 133);
            this.spriteOrigPtXLabel.Name = "spriteOrigPtXLabel";
            this.spriteOrigPtXLabel.Size = new System.Drawing.Size(17, 13);
            this.spriteOrigPtXLabel.TabIndex = 14;
            this.spriteOrigPtXLabel.Text = "X:";
            // 
            // spriteOrigPtYNumUpDown
            // 
            this.spriteOrigPtYNumUpDown.Location = new System.Drawing.Point(266, 131);
            this.spriteOrigPtYNumUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.spriteOrigPtYNumUpDown.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.spriteOrigPtYNumUpDown.Name = "spriteOrigPtYNumUpDown";
            this.spriteOrigPtYNumUpDown.Size = new System.Drawing.Size(85, 20);
            this.spriteOrigPtYNumUpDown.TabIndex = 13;
            this.spriteOrigPtYNumUpDown.ValueChanged += new System.EventHandler(this.spriteOrigPtYNumUpDown_ValueChanged);
            // 
            // spriteOrigPtXNumUpDown
            // 
            this.spriteOrigPtXNumUpDown.Location = new System.Drawing.Point(128, 131);
            this.spriteOrigPtXNumUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.spriteOrigPtXNumUpDown.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.spriteOrigPtXNumUpDown.Name = "spriteOrigPtXNumUpDown";
            this.spriteOrigPtXNumUpDown.Size = new System.Drawing.Size(85, 20);
            this.spriteOrigPtXNumUpDown.TabIndex = 12;
            this.spriteOrigPtXNumUpDown.ValueChanged += new System.EventHandler(this.spriteOrigPtXNumUpDown_ValueChanged);
            // 
            // spriteRectHLabel
            // 
            this.spriteRectHLabel.AutoSize = true;
            this.spriteRectHLabel.Location = new System.Drawing.Point(219, 89);
            this.spriteRectHLabel.Name = "spriteRectHLabel";
            this.spriteRectHLabel.Size = new System.Drawing.Size(41, 13);
            this.spriteRectHLabel.TabIndex = 11;
            this.spriteRectHLabel.Text = "Height:";
            // 
            // spriteRectWLabel
            // 
            this.spriteRectWLabel.AutoSize = true;
            this.spriteRectWLabel.Location = new System.Drawing.Point(84, 89);
            this.spriteRectWLabel.Name = "spriteRectWLabel";
            this.spriteRectWLabel.Size = new System.Drawing.Size(38, 13);
            this.spriteRectWLabel.TabIndex = 10;
            this.spriteRectWLabel.Text = "Width:";
            // 
            // spriteRectHNumUpDown
            // 
            this.spriteRectHNumUpDown.Location = new System.Drawing.Point(266, 87);
            this.spriteRectHNumUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.spriteRectHNumUpDown.Name = "spriteRectHNumUpDown";
            this.spriteRectHNumUpDown.Size = new System.Drawing.Size(85, 20);
            this.spriteRectHNumUpDown.TabIndex = 9;
            this.spriteRectHNumUpDown.ValueChanged += new System.EventHandler(this.spriteRectHNumUpDown_ValueChanged);
            // 
            // spriteRectWNumUpDown
            // 
            this.spriteRectWNumUpDown.Location = new System.Drawing.Point(128, 87);
            this.spriteRectWNumUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.spriteRectWNumUpDown.Name = "spriteRectWNumUpDown";
            this.spriteRectWNumUpDown.Size = new System.Drawing.Size(85, 20);
            this.spriteRectWNumUpDown.TabIndex = 8;
            this.spriteRectWNumUpDown.ValueChanged += new System.EventHandler(this.spriteRectWNumUpDown_ValueChanged);
            // 
            // spriteRectLabel
            // 
            this.spriteRectLabel.AutoSize = true;
            this.spriteRectLabel.Location = new System.Drawing.Point(6, 63);
            this.spriteRectLabel.Name = "spriteRectLabel";
            this.spriteRectLabel.Size = new System.Drawing.Size(59, 13);
            this.spriteRectLabel.TabIndex = 7;
            this.spriteRectLabel.Text = "Rectangle:";
            // 
            // spriteRectYLabel
            // 
            this.spriteRectYLabel.AutoSize = true;
            this.spriteRectYLabel.Location = new System.Drawing.Point(243, 63);
            this.spriteRectYLabel.Name = "spriteRectYLabel";
            this.spriteRectYLabel.Size = new System.Drawing.Size(17, 13);
            this.spriteRectYLabel.TabIndex = 6;
            this.spriteRectYLabel.Text = "Y:";
            // 
            // spriteRectXLabel
            // 
            this.spriteRectXLabel.AutoSize = true;
            this.spriteRectXLabel.Location = new System.Drawing.Point(105, 63);
            this.spriteRectXLabel.Name = "spriteRectXLabel";
            this.spriteRectXLabel.Size = new System.Drawing.Size(17, 13);
            this.spriteRectXLabel.TabIndex = 5;
            this.spriteRectXLabel.Text = "X:";
            // 
            // spriteRectYNumUpDown
            // 
            this.spriteRectYNumUpDown.Location = new System.Drawing.Point(266, 61);
            this.spriteRectYNumUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.spriteRectYNumUpDown.Name = "spriteRectYNumUpDown";
            this.spriteRectYNumUpDown.Size = new System.Drawing.Size(85, 20);
            this.spriteRectYNumUpDown.TabIndex = 4;
            this.spriteRectYNumUpDown.ValueChanged += new System.EventHandler(this.spriteRectYNumUpDown_ValueChanged);
            // 
            // spriteRectXNumUpDown
            // 
            this.spriteRectXNumUpDown.Location = new System.Drawing.Point(128, 61);
            this.spriteRectXNumUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.spriteRectXNumUpDown.Name = "spriteRectXNumUpDown";
            this.spriteRectXNumUpDown.Size = new System.Drawing.Size(85, 20);
            this.spriteRectXNumUpDown.TabIndex = 3;
            this.spriteRectXNumUpDown.ValueChanged += new System.EventHandler(this.spriteRectXNumUpDown_ValueChanged);
            // 
            // spriteNameLabel
            // 
            this.spriteNameLabel.AutoSize = true;
            this.spriteNameLabel.Location = new System.Drawing.Point(6, 22);
            this.spriteNameLabel.Name = "spriteNameLabel";
            this.spriteNameLabel.Size = new System.Drawing.Size(66, 13);
            this.spriteNameLabel.TabIndex = 2;
            this.spriteNameLabel.Text = "Sprite name:";
            // 
            // spriteNameTextBox
            // 
            this.spriteNameTextBox.Location = new System.Drawing.Point(78, 19);
            this.spriteNameTextBox.Name = "spriteNameTextBox";
            this.spriteNameTextBox.Size = new System.Drawing.Size(273, 20);
            this.spriteNameTextBox.TabIndex = 0;
            this.spriteNameTextBox.TextChanged += new System.EventHandler(this.spriteNameTextBox_TextChanged);
            // 
            // reloadPictureButton
            // 
            this.reloadPictureButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.reloadPictureButton.Location = new System.Drawing.Point(304, 18);
            this.reloadPictureButton.Name = "reloadPictureButton";
            this.reloadPictureButton.Size = new System.Drawing.Size(62, 23);
            this.reloadPictureButton.TabIndex = 2;
            this.reloadPictureButton.Text = "Reload";
            this.reloadPictureButton.UseVisualStyleBackColor = true;
            this.reloadPictureButton.Click += new System.EventHandler(this.reloadPictureButton_Click);
            // 
            // filenameLabel
            // 
            this.filenameLabel.AutoSize = true;
            this.filenameLabel.Location = new System.Drawing.Point(6, 24);
            this.filenameLabel.Name = "filenameLabel";
            this.filenameLabel.Size = new System.Drawing.Size(85, 13);
            this.filenameLabel.TabIndex = 1;
            this.filenameLabel.Text = "Picture filename:";
            // 
            // filenameTextBox
            // 
            this.filenameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filenameTextBox.Location = new System.Drawing.Point(97, 19);
            this.filenameTextBox.Name = "filenameTextBox";
            this.filenameTextBox.Size = new System.Drawing.Size(201, 20);
            this.filenameTextBox.TabIndex = 3;
            this.filenameTextBox.TextChanged += new System.EventHandler(this.filenameTextBox_TextChanged);
            // 
            // imageViewPanel
            // 
            this.imageViewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageViewPanel.AutoScroll = true;
            this.imageViewPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.imageViewPanel.Controls.Add(this.insideViewPanel);
            this.imageViewPanel.Location = new System.Drawing.Point(12, 27);
            this.imageViewPanel.Name = "imageViewPanel";
            this.imageViewPanel.Size = new System.Drawing.Size(529, 503);
            this.imageViewPanel.TabIndex = 0;
            this.imageViewPanel.CTRLMouseWheel += new System.Windows.Forms.MouseEventHandler(this.imageViewPanel_CTRLMouseWheel);
            this.imageViewPanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.imageViewPanel_Scroll);
            // 
            // insideViewPanel
            // 
            this.insideViewPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.insideViewPanel.Controls.Add(this.spritesheetPictureBox);
            this.insideViewPanel.Location = new System.Drawing.Point(3, 3);
            this.insideViewPanel.Name = "insideViewPanel";
            this.insideViewPanel.Size = new System.Drawing.Size(523, 497);
            this.insideViewPanel.TabIndex = 1;
            // 
            // spritesheetPictureBox
            // 
            this.spritesheetPictureBox.AllowSRectCreation = true;
            this.spritesheetPictureBox.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.spritesheetPictureBox.Location = new System.Drawing.Point(3, 3);
            this.spritesheetPictureBox.Name = "spritesheetPictureBox";
            this.spritesheetPictureBox.SelectedSRect = null;
            this.spritesheetPictureBox.Size = new System.Drawing.Size(100, 50);
            this.spritesheetPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.spritesheetPictureBox.TabIndex = 0;
            this.spritesheetPictureBox.TabStop = false;
            this.spritesheetPictureBox.RectResize += new System.EventHandler<ABStudio.Controls.PictureBoxDB.SizableRectEventArgs>(this.spritesheetPictureBox_RectResize);
            this.spritesheetPictureBox.SelectedSRectChanged += new System.EventHandler(this.spritesheetPictureBox_SelectedSRectChanged);
            this.spritesheetPictureBox.SRectCreated += new System.EventHandler<ABStudio.Controls.PictureBoxDB.SizableRectEventArgs>(this.spritesheetPictureBox_SRectCreated);
            this.spritesheetPictureBox.Click += new System.EventHandler(this.spritesheetPictureBox_Click);
            this.spritesheetPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.spritesheetPictureBox_Paint);
            // 
            // SpritesheetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 542);
            this.Controls.Add(this.propertiesGroupBox);
            this.Controls.Add(this.imageViewPanel);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SpritesheetEditor";
            this.Text = "AB Classic Spritesheet Editor";
            this.Load += new System.EventHandler(this.SpritesheetEditor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpritesheetEditor_KeyDown);
            this.Resize += new System.EventHandler(this.SpritesheetEditor_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.propertiesGroupBox.ResumeLayout(false);
            this.propertiesGroupBox.PerformLayout();
            this.selectedSpriteGroupBox.ResumeLayout(false);
            this.selectedSpriteGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spriteOrigPtYNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spriteOrigPtXNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spriteRectHNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spriteRectWNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spriteRectYNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spriteRectXNumUpDown)).EndInit();
            this.imageViewPanel.ResumeLayout(false);
            this.insideViewPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spritesheetPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ABStudio.Controls.CustomPanel imageViewPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openHereToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openInNewInstanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportSpritesheetImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportCroppedSpritesheetToolStripMenuItem;
        private ABStudio.Controls.PictureBoxDB spritesheetPictureBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importSpritesheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importSpritesFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Panel insideViewPanel;
        private System.Windows.Forms.GroupBox propertiesGroupBox;
        private System.Windows.Forms.TextBox filenameTextBox;
        private System.Windows.Forms.Label filenameLabel;
        private System.Windows.Forms.Button reloadPictureButton;
        private System.Windows.Forms.GroupBox selectedSpriteGroupBox;
        private System.Windows.Forms.TextBox spriteNameTextBox;
        private System.Windows.Forms.Label spriteNameLabel;
        private System.Windows.Forms.Label spriteRectYLabel;
        private System.Windows.Forms.Label spriteRectXLabel;
        private System.Windows.Forms.NumericUpDown spriteRectYNumUpDown;
        private System.Windows.Forms.NumericUpDown spriteRectXNumUpDown;
        private System.Windows.Forms.Label spriteRectHLabel;
        private System.Windows.Forms.Label spriteRectWLabel;
        private System.Windows.Forms.NumericUpDown spriteRectHNumUpDown;
        private System.Windows.Forms.NumericUpDown spriteRectWNumUpDown;
        private System.Windows.Forms.Label spriteRectLabel;
        private System.Windows.Forms.Label spriteOrigPtLabel;
        private System.Windows.Forms.Label spriteOrigPtYLabel;
        private System.Windows.Forms.Label spriteOrigPtXLabel;
        private System.Windows.Forms.NumericUpDown spriteOrigPtYNumUpDown;
        private System.Windows.Forms.NumericUpDown spriteOrigPtXNumUpDown;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllSpritesToolStripMenuItem;
        private System.Windows.Forms.Label spriteOrigPtMapLabel;
        private System.Windows.Forms.Button spriteOrigPtMapBRButton;
        private System.Windows.Forms.Button spriteOrigPtMapBButton;
        private System.Windows.Forms.Button spriteOrigPtMapBLButton;
        private System.Windows.Forms.Button spriteOrigPtMapTRButton;
        private System.Windows.Forms.Button spriteOrigPtMapTButton;
        private System.Windows.Forms.Button spriteOrigPtMapTLButton;
        private System.Windows.Forms.Button spriteOrigPtMapRButton;
        private System.Windows.Forms.Button spriteOrigPtMapCButton;
        private System.Windows.Forms.Button spriteOrigPtMapLButton;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}