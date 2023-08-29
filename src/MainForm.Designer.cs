namespace SWRPre
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.disPathLabel = new System.Windows.Forms.Label();
            this.discretizationPathTextBox = new System.Windows.Forms.TextBox();
            this.discretizationPathBrowseButton = new System.Windows.Forms.Button();
            this.shapefilePathBrowseButton = new System.Windows.Forms.Button();
            this.shapefilePathTextBox = new System.Windows.Forms.TextBox();
            this.shpPathLabel = new System.Windows.Forms.Label();
            this.xCoordinateTextBox = new System.Windows.Forms.TextBox();
            this.xCoordLabel = new System.Windows.Forms.Label();
            this.yCoordinateTextBox = new System.Windows.Forms.TextBox();
            this.yCoordLabel = new System.Windows.Forms.Label();
            this.rotationAngleTextBox = new System.Windows.Forms.TextBox();
            this.gridRotationLabel = new System.Windows.Forms.Label();
            this.createFileButton = new System.Windows.Forms.Button();
            this.comboBoxIdentifier = new System.Windows.Forms.ComboBox();
            this.comboBoxNConn = new System.Windows.Forms.ComboBox();
            this.comboBoxConn = new System.Windows.Forms.ComboBox();
            this.idLabel = new System.Windows.Forms.Label();
            this.numConnLabel = new System.Windows.Forms.Label();
            this.connFieldLabel = new System.Windows.Forms.Label();
            this.groupBoxReachGroups = new System.Windows.Forms.GroupBox();
            this.startReachLabel = new System.Windows.Forms.Label();
            this.startReachTextBox = new System.Windows.Forms.TextBox();
            this.reachGroupButtonEqual = new System.Windows.Forms.RadioButton();
            this.targetLengthLabel = new System.Windows.Forms.Label();
            this.reachGroupLengthTextBox = new System.Windows.Forms.TextBox();
            this.reachGroupButtonExact = new System.Windows.Forms.RadioButton();
            this.reachGroupButtonCombine = new System.Windows.Forms.RadioButton();
            this.orderingGroupBox = new System.Windows.Forms.GroupBox();
            this.comboBoxPreferredDirection = new System.Windows.Forms.ComboBox();
            this.xmlBox = new System.Windows.Forms.CheckBox();
            this.modelGridGroupBox = new System.Windows.Forms.GroupBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_openXML = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnu_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.openFD = new System.Windows.Forms.OpenFileDialog();
            this.hemisphereComboBox = new System.Windows.Forms.ComboBox();
            this.hemisphereLabel = new System.Windows.Forms.Label();
            this.groupBoxReachGroups.SuspendLayout();
            this.orderingGroupBox.SuspendLayout();
            this.modelGridGroupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // disPathLabel
            // 
            this.disPathLabel.AutoSize = true;
            this.disPathLabel.Location = new System.Drawing.Point(28, 73);
            this.disPathLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.disPathLabel.Name = "disPathLabel";
            this.disPathLabel.Size = new System.Drawing.Size(231, 26);
            this.disPathLabel.TabIndex = 0;
            this.disPathLabel.Text = "Discretization file path:";
            // 
            // discretizationPathTextBox
            // 
            this.discretizationPathTextBox.Location = new System.Drawing.Point(30, 104);
            this.discretizationPathTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.discretizationPathTextBox.Name = "discretizationPathTextBox";
            this.discretizationPathTextBox.Size = new System.Drawing.Size(680, 31);
            this.discretizationPathTextBox.TabIndex = 0;
            this.discretizationPathTextBox.TextChanged += new System.EventHandler(this.discretizationPathTextBox_TextChanged);
            // 
            // discretizationPathBrowseButton
            // 
            this.discretizationPathBrowseButton.Image = ((System.Drawing.Image)(resources.GetObject("discretizationPathBrowseButton.Image")));
            this.discretizationPathBrowseButton.Location = new System.Drawing.Point(726, 100);
            this.discretizationPathBrowseButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.discretizationPathBrowseButton.Name = "discretizationPathBrowseButton";
            this.discretizationPathBrowseButton.Size = new System.Drawing.Size(60, 44);
            this.discretizationPathBrowseButton.TabIndex = 1;
            this.discretizationPathBrowseButton.UseVisualStyleBackColor = true;
            this.discretizationPathBrowseButton.Click += new System.EventHandler(this.discretizationPathBrowseButton_Click);
            // 
            // shapefilePathBrowseButton
            // 
            this.shapefilePathBrowseButton.Image = ((System.Drawing.Image)(resources.GetObject("shapefilePathBrowseButton.Image")));
            this.shapefilePathBrowseButton.Location = new System.Drawing.Point(728, 188);
            this.shapefilePathBrowseButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.shapefilePathBrowseButton.Name = "shapefilePathBrowseButton";
            this.shapefilePathBrowseButton.Size = new System.Drawing.Size(60, 44);
            this.shapefilePathBrowseButton.TabIndex = 3;
            this.shapefilePathBrowseButton.UseVisualStyleBackColor = true;
            this.shapefilePathBrowseButton.Click += new System.EventHandler(this.shapefilePathBrowseButton_Click);
            // 
            // shapefilePathTextBox
            // 
            this.shapefilePathTextBox.Location = new System.Drawing.Point(32, 192);
            this.shapefilePathTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.shapefilePathTextBox.Name = "shapefilePathTextBox";
            this.shapefilePathTextBox.Size = new System.Drawing.Size(680, 31);
            this.shapefilePathTextBox.TabIndex = 2;
            this.shapefilePathTextBox.TextChanged += new System.EventHandler(this.shapefilePathTextBox_TextChanged);
            // 
            // shpPathLabel
            // 
            this.shpPathLabel.AutoSize = true;
            this.shpPathLabel.Location = new System.Drawing.Point(28, 162);
            this.shpPathLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.shpPathLabel.Name = "shpPathLabel";
            this.shpPathLabel.Size = new System.Drawing.Size(157, 26);
            this.shpPathLabel.TabIndex = 4;
            this.shpPathLabel.Text = "Shapefile path:";
            this.shpPathLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // xCoordinateTextBox
            // 
            this.xCoordinateTextBox.Location = new System.Drawing.Point(12, 75);
            this.xCoordinateTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.xCoordinateTextBox.Name = "xCoordinateTextBox";
            this.xCoordinateTextBox.Size = new System.Drawing.Size(336, 31);
            this.xCoordinateTextBox.TabIndex = 7;
            // 
            // xCoordLabel
            // 
            this.xCoordLabel.AutoSize = true;
            this.xCoordLabel.Location = new System.Drawing.Point(12, 44);
            this.xCoordLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.xCoordLabel.Name = "xCoordLabel";
            this.xCoordLabel.Size = new System.Drawing.Size(216, 26);
            this.xCoordLabel.TabIndex = 7;
            this.xCoordLabel.Text = "Grid x-coordinate (L):";
            this.xCoordLabel.Click += new System.EventHandler(this.label3_Click);
            // 
            // yCoordinateTextBox
            // 
            this.yCoordinateTextBox.Location = new System.Drawing.Point(396, 75);
            this.yCoordinateTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.yCoordinateTextBox.Name = "yCoordinateTextBox";
            this.yCoordinateTextBox.Size = new System.Drawing.Size(336, 31);
            this.yCoordinateTextBox.TabIndex = 8;
            // 
            // yCoordLabel
            // 
            this.yCoordLabel.AutoSize = true;
            this.yCoordLabel.Location = new System.Drawing.Point(390, 44);
            this.yCoordLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.yCoordLabel.Name = "yCoordLabel";
            this.yCoordLabel.Size = new System.Drawing.Size(216, 26);
            this.yCoordLabel.TabIndex = 9;
            this.yCoordLabel.Text = "Grid y-coordinate (L):";
            // 
            // rotationAngleTextBox
            // 
            this.rotationAngleTextBox.Location = new System.Drawing.Point(12, 181);
            this.rotationAngleTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.rotationAngleTextBox.Name = "rotationAngleTextBox";
            this.rotationAngleTextBox.Size = new System.Drawing.Size(336, 31);
            this.rotationAngleTextBox.TabIndex = 9;
            // 
            // gridRotationLabel
            // 
            this.gridRotationLabel.AutoSize = true;
            this.gridRotationLabel.Location = new System.Drawing.Point(12, 150);
            this.gridRotationLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.gridRotationLabel.Name = "gridRotationLabel";
            this.gridRotationLabel.Size = new System.Drawing.Size(294, 26);
            this.gridRotationLabel.TabIndex = 11;
            this.gridRotationLabel.Text = "Grid rotation angle (degrees):";
            // 
            // createFileButton
            // 
            this.createFileButton.Location = new System.Drawing.Point(1050, 694);
            this.createFileButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.createFileButton.Name = "createFileButton";
            this.createFileButton.Size = new System.Drawing.Size(182, 69);
            this.createFileButton.TabIndex = 14;
            this.createFileButton.Text = "Create File";
            this.createFileButton.UseVisualStyleBackColor = true;
            this.createFileButton.Click += new System.EventHandler(this.createFileButton_Click);
            // 
            // comboBoxIdentifier
            // 
            this.comboBoxIdentifier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIdentifier.FormattingEnabled = true;
            this.comboBoxIdentifier.Location = new System.Drawing.Point(32, 283);
            this.comboBoxIdentifier.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBoxIdentifier.Name = "comboBoxIdentifier";
            this.comboBoxIdentifier.Size = new System.Drawing.Size(752, 33);
            this.comboBoxIdentifier.TabIndex = 4;
            this.comboBoxIdentifier.SelectedIndexChanged += new System.EventHandler(this.comboBoxIdentifier_SelectedIndexChanged);
            // 
            // comboBoxNConn
            // 
            this.comboBoxNConn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNConn.FormattingEnabled = true;
            this.comboBoxNConn.Location = new System.Drawing.Point(32, 377);
            this.comboBoxNConn.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBoxNConn.Name = "comboBoxNConn";
            this.comboBoxNConn.Size = new System.Drawing.Size(752, 33);
            this.comboBoxNConn.TabIndex = 5;
            this.comboBoxNConn.SelectedIndexChanged += new System.EventHandler(this.comboBoxStartConnection_SelectedIndexChanged);
            // 
            // comboBoxConn
            // 
            this.comboBoxConn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConn.FormattingEnabled = true;
            this.comboBoxConn.Location = new System.Drawing.Point(34, 467);
            this.comboBoxConn.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBoxConn.Name = "comboBoxConn";
            this.comboBoxConn.Size = new System.Drawing.Size(752, 33);
            this.comboBoxConn.TabIndex = 6;
            this.comboBoxConn.SelectedIndexChanged += new System.EventHandler(this.comboBoxEndConnection_SelectedIndexChanged);
            // 
            // idLabel
            // 
            this.idLabel.AutoSize = true;
            this.idLabel.Location = new System.Drawing.Point(28, 252);
            this.idLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(86, 26);
            this.idLabel.TabIndex = 21;
            this.idLabel.Text = "ID field:";
            this.idLabel.Click += new System.EventHandler(this.label6_Click);
            // 
            // numConnLabel
            // 
            this.numConnLabel.AutoSize = true;
            this.numConnLabel.Location = new System.Drawing.Point(28, 346);
            this.numConnLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.numConnLabel.Name = "numConnLabel";
            this.numConnLabel.Size = new System.Drawing.Size(288, 26);
            this.numConnLabel.TabIndex = 22;
            this.numConnLabel.Text = "Number of connections field:";
            // 
            // connFieldLabel
            // 
            this.connFieldLabel.AutoSize = true;
            this.connFieldLabel.Location = new System.Drawing.Point(28, 437);
            this.connFieldLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.connFieldLabel.Name = "connFieldLabel";
            this.connFieldLabel.Size = new System.Drawing.Size(543, 26);
            this.connFieldLabel.TabIndex = 23;
            this.connFieldLabel.Text = "Connection field (whitespace and/or comma delimited):";
            // 
            // groupBoxReachGroups
            // 
            this.groupBoxReachGroups.Controls.Add(this.startReachLabel);
            this.groupBoxReachGroups.Controls.Add(this.startReachTextBox);
            this.groupBoxReachGroups.Controls.Add(this.reachGroupButtonEqual);
            this.groupBoxReachGroups.Controls.Add(this.targetLengthLabel);
            this.groupBoxReachGroups.Controls.Add(this.reachGroupLengthTextBox);
            this.groupBoxReachGroups.Controls.Add(this.reachGroupButtonExact);
            this.groupBoxReachGroups.Controls.Add(this.reachGroupButtonCombine);
            this.groupBoxReachGroups.Location = new System.Drawing.Point(808, 65);
            this.groupBoxReachGroups.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxReachGroups.Name = "groupBoxReachGroups";
            this.groupBoxReachGroups.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxReachGroups.Size = new System.Drawing.Size(714, 442);
            this.groupBoxReachGroups.TabIndex = 10;
            this.groupBoxReachGroups.TabStop = false;
            this.groupBoxReachGroups.Text = "Reach Groups:";
            // 
            // startReachLabel
            // 
            this.startReachLabel.AutoSize = true;
            this.startReachLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.startReachLabel.Location = new System.Drawing.Point(116, 371);
            this.startReachLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.startReachLabel.Name = "startReachLabel";
            this.startReachLabel.Size = new System.Drawing.Size(233, 26);
            this.startReachLabel.TabIndex = 6;
            this.startReachLabel.Text = "Starting reach number:";
            this.startReachLabel.Click += new System.EventHandler(this.label10_Click);
            // 
            // startReachTextBox
            // 
            this.startReachTextBox.Location = new System.Drawing.Point(406, 371);
            this.startReachTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.startReachTextBox.Name = "startReachTextBox";
            this.startReachTextBox.Size = new System.Drawing.Size(270, 31);
            this.startReachTextBox.TabIndex = 5;
            this.startReachTextBox.Text = "1";
            this.startReachTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // reachGroupButtonEqual
            // 
            this.reachGroupButtonEqual.AutoSize = true;
            this.reachGroupButtonEqual.Location = new System.Drawing.Point(12, 219);
            this.reachGroupButtonEqual.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.reachGroupButtonEqual.Name = "reachGroupButtonEqual";
            this.reachGroupButtonEqual.Size = new System.Drawing.Size(603, 56);
            this.reachGroupButtonEqual.TabIndex = 2;
            this.reachGroupButtonEqual.TabStop = true;
            this.reachGroupButtonEqual.Text = "Equal - Make reach groups for each polyline equal length.  \r\nThis will be a close" +
    " as possible to the target length.";
            this.reachGroupButtonEqual.UseVisualStyleBackColor = true;
            this.reachGroupButtonEqual.CheckedChanged += new System.EventHandler(this.reachGroupButtonEqual_CheckedChanged);
            // 
            // targetLengthLabel
            // 
            this.targetLengthLabel.AutoSize = true;
            this.targetLengthLabel.ForeColor = System.Drawing.Color.Gray;
            this.targetLengthLabel.Location = new System.Drawing.Point(52, 312);
            this.targetLengthLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.targetLengthLabel.Name = "targetLengthLabel";
            this.targetLengthLabel.Size = new System.Drawing.Size(297, 26);
            this.targetLengthLabel.TabIndex = 4;
            this.targetLengthLabel.Text = "Target reach group length (L):";
            // 
            // reachGroupLengthTextBox
            // 
            this.reachGroupLengthTextBox.Enabled = false;
            this.reachGroupLengthTextBox.Location = new System.Drawing.Point(406, 312);
            this.reachGroupLengthTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.reachGroupLengthTextBox.Name = "reachGroupLengthTextBox";
            this.reachGroupLengthTextBox.Size = new System.Drawing.Size(270, 31);
            this.reachGroupLengthTextBox.TabIndex = 3;
            this.reachGroupLengthTextBox.Text = "0.0";
            this.reachGroupLengthTextBox.TextChanged += new System.EventHandler(this.reachGroupLengthTextBox_TextChanged);
            // 
            // reachGroupButtonExact
            // 
            this.reachGroupButtonExact.Location = new System.Drawing.Point(12, 113);
            this.reachGroupButtonExact.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.reachGroupButtonExact.Name = "reachGroupButtonExact";
            this.reachGroupButtonExact.Size = new System.Drawing.Size(668, 100);
            this.reachGroupButtonExact.TabIndex = 1;
            this.reachGroupButtonExact.TabStop = true;
            this.reachGroupButtonExact.Text = "Exact - Make reach groups for each polyline, except for the last, exactly the tar" +
    "get length.  The last reach group for each polyline will be within 50% of the ta" +
    "rget length";
            this.reachGroupButtonExact.UseVisualStyleBackColor = true;
            this.reachGroupButtonExact.CheckedChanged += new System.EventHandler(this.reachGroupButtonExact_CheckedChanged);
            // 
            // reachGroupButtonCombine
            // 
            this.reachGroupButtonCombine.Checked = true;
            this.reachGroupButtonCombine.Location = new System.Drawing.Point(12, 35);
            this.reachGroupButtonCombine.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.reachGroupButtonCombine.Name = "reachGroupButtonCombine";
            this.reachGroupButtonCombine.Size = new System.Drawing.Size(690, 94);
            this.reachGroupButtonCombine.TabIndex = 0;
            this.reachGroupButtonCombine.TabStop = true;
            this.reachGroupButtonCombine.Text = "Intersection - Make each reach, as segmented by the grid, a separate reach group." +
    "";
            this.reachGroupButtonCombine.UseVisualStyleBackColor = true;
            this.reachGroupButtonCombine.CheckedChanged += new System.EventHandler(this.reachGroupButtonCombine_CheckedChanged);
            // 
            // orderingGroupBox
            // 
            this.orderingGroupBox.Controls.Add(this.comboBoxPreferredDirection);
            this.orderingGroupBox.Location = new System.Drawing.Point(950, 523);
            this.orderingGroupBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.orderingGroupBox.Name = "orderingGroupBox";
            this.orderingGroupBox.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.orderingGroupBox.Size = new System.Drawing.Size(376, 121);
            this.orderingGroupBox.TabIndex = 11;
            this.orderingGroupBox.TabStop = false;
            this.orderingGroupBox.Text = "Preferred Ordering Direction";
            this.orderingGroupBox.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // comboBoxPreferredDirection
            // 
            this.comboBoxPreferredDirection.FormattingEnabled = true;
            this.comboBoxPreferredDirection.Items.AddRange(new object[] {
            "None",
            "Top left",
            "Bottom left",
            "Bottom right",
            "Top right"});
            this.comboBoxPreferredDirection.Location = new System.Drawing.Point(22, 46);
            this.comboBoxPreferredDirection.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBoxPreferredDirection.Name = "comboBoxPreferredDirection";
            this.comboBoxPreferredDirection.Size = new System.Drawing.Size(332, 33);
            this.comboBoxPreferredDirection.TabIndex = 0;
            this.comboBoxPreferredDirection.SelectedIndexChanged += new System.EventHandler(this.comboBoxPreferredDirection_SelectedIndexChanged);
            // 
            // xmlBox
            // 
            this.xmlBox.AutoSize = true;
            this.xmlBox.Checked = true;
            this.xmlBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.xmlBox.Location = new System.Drawing.Point(1000, 650);
            this.xmlBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.xmlBox.Name = "xmlBox";
            this.xmlBox.Size = new System.Drawing.Size(290, 30);
            this.xmlBox.TabIndex = 13;
            this.xmlBox.Text = "Write XML parameters file";
            this.xmlBox.UseVisualStyleBackColor = true;
            this.xmlBox.CheckedChanged += new System.EventHandler(this.xmlBox_CheckedChanged);
            // 
            // modelGridGroupBox
            // 
            this.modelGridGroupBox.Controls.Add(this.hemisphereLabel);
            this.modelGridGroupBox.Controls.Add(this.hemisphereComboBox);
            this.modelGridGroupBox.Controls.Add(this.rotationAngleTextBox);
            this.modelGridGroupBox.Controls.Add(this.gridRotationLabel);
            this.modelGridGroupBox.Controls.Add(this.yCoordinateTextBox);
            this.modelGridGroupBox.Controls.Add(this.yCoordLabel);
            this.modelGridGroupBox.Controls.Add(this.xCoordinateTextBox);
            this.modelGridGroupBox.Controls.Add(this.xCoordLabel);
            this.modelGridGroupBox.Location = new System.Drawing.Point(18, 531);
            this.modelGridGroupBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.modelGridGroupBox.Name = "modelGridGroupBox";
            this.modelGridGroupBox.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.modelGridGroupBox.Size = new System.Drawing.Size(772, 273);
            this.modelGridGroupBox.TabIndex = 24;
            this.modelGridGroupBox.TabStop = false;
            this.modelGridGroupBox.Text = "Model grid origin:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(12, 4, 0, 4);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(1550, 47);
            this.menuStrip1.TabIndex = 25;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_openXML,
            this.toolStripMenuItem1,
            this.mnu_exit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.fileToolStripMenuItem.ShowShortcutKeys = false;
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(64, 39);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // mnu_openXML
            // 
            this.mnu_openXML.Name = "mnu_openXML";
            this.mnu_openXML.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnu_openXML.Size = new System.Drawing.Size(340, 40);
            this.mnu_openXML.Text = "&Open XML File";
            this.mnu_openXML.Click += new System.EventHandler(this.mnu_openXML_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(337, 6);
            // 
            // mnu_exit
            // 
            this.mnu_exit.Name = "mnu_exit";
            this.mnu_exit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.mnu_exit.Size = new System.Drawing.Size(340, 40);
            this.mnu_exit.Text = "E&xit";
            this.mnu_exit.Click += new System.EventHandler(this.mnu_exit_Click);
            // 
            // openFD
            // 
            this.openFD.FileName = "SWRPre.xml";
            this.openFD.Title = "Load SWRPre Parameters";
            this.openFD.FileOk += new System.ComponentModel.CancelEventHandler(this.openFD_FileOk);
            // 
            // hemisphereComboBox
            // 
            this.hemisphereComboBox.AllowDrop = true;
            this.hemisphereComboBox.FormattingEnabled = true;
            this.hemisphereComboBox.Items.AddRange(new object[] {
            "Northern",
            "Southern"});
            this.hemisphereComboBox.Location = new System.Drawing.Point(396, 177);
            this.hemisphereComboBox.Name = "hemisphereComboBox";
            this.hemisphereComboBox.Size = new System.Drawing.Size(336, 33);
            this.hemisphereComboBox.TabIndex = 12;
            this.hemisphereComboBox.Text = "Northern";
            this.hemisphereComboBox.SelectedIndexChanged += new System.EventHandler(this.hemisphereComboBox_SelectedIndexChanged);
            // 
            // hemisphereLabel
            // 
            this.hemisphereLabel.AutoSize = true;
            this.hemisphereLabel.Location = new System.Drawing.Point(395, 149);
            this.hemisphereLabel.Name = "hemisphereLabel";
            this.hemisphereLabel.Size = new System.Drawing.Size(130, 26);
            this.hemisphereLabel.TabIndex = 13;
            this.hemisphereLabel.Text = "Hemisphere";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1550, 810);
            this.Controls.Add(this.modelGridGroupBox);
            this.Controls.Add(this.xmlBox);
            this.Controls.Add(this.orderingGroupBox);
            this.Controls.Add(this.groupBoxReachGroups);
            this.Controls.Add(this.connFieldLabel);
            this.Controls.Add(this.numConnLabel);
            this.Controls.Add(this.idLabel);
            this.Controls.Add(this.comboBoxConn);
            this.Controls.Add(this.comboBoxNConn);
            this.Controls.Add(this.comboBoxIdentifier);
            this.Controls.Add(this.createFileButton);
            this.Controls.Add(this.shapefilePathBrowseButton);
            this.Controls.Add(this.shapefilePathTextBox);
            this.Controls.Add(this.shpPathLabel);
            this.Controls.Add(this.discretizationPathBrowseButton);
            this.Controls.Add(this.discretizationPathTextBox);
            this.Controls.Add(this.disPathLabel);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SWRPre - a preprocessor for the MODFLOW SWR package";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBoxReachGroups.ResumeLayout(false);
            this.groupBoxReachGroups.PerformLayout();
            this.orderingGroupBox.ResumeLayout(false);
            this.modelGridGroupBox.ResumeLayout(false);
            this.modelGridGroupBox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label disPathLabel;
        private System.Windows.Forms.TextBox discretizationPathTextBox;
        private System.Windows.Forms.Button discretizationPathBrowseButton;
        private System.Windows.Forms.Button shapefilePathBrowseButton;
        private System.Windows.Forms.TextBox shapefilePathTextBox;
        private System.Windows.Forms.Label shpPathLabel;
        private System.Windows.Forms.TextBox xCoordinateTextBox;
        private System.Windows.Forms.Label xCoordLabel;
        private System.Windows.Forms.TextBox yCoordinateTextBox;
        private System.Windows.Forms.Label yCoordLabel;
        private System.Windows.Forms.TextBox rotationAngleTextBox;
        private System.Windows.Forms.Label gridRotationLabel;
        private System.Windows.Forms.Button createFileButton;
        private System.Windows.Forms.ComboBox comboBoxIdentifier;
        private System.Windows.Forms.ComboBox comboBoxNConn;
        private System.Windows.Forms.ComboBox comboBoxConn;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.Label numConnLabel;
        private System.Windows.Forms.Label connFieldLabel;
        private System.Windows.Forms.GroupBox groupBoxReachGroups;
        private System.Windows.Forms.RadioButton reachGroupButtonCombine;
        private System.Windows.Forms.RadioButton reachGroupButtonExact;
        private System.Windows.Forms.Label targetLengthLabel;
        private System.Windows.Forms.TextBox reachGroupLengthTextBox;
        private System.Windows.Forms.GroupBox orderingGroupBox;
        private System.Windows.Forms.CheckBox xmlBox;
        private System.Windows.Forms.ComboBox comboBoxPreferredDirection;
        private System.Windows.Forms.RadioButton reachGroupButtonEqual;
        private System.Windows.Forms.GroupBox modelGridGroupBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnu_openXML;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem mnu_exit;
        private System.Windows.Forms.OpenFileDialog openFD;
        private System.Windows.Forms.Label startReachLabel;
        private System.Windows.Forms.TextBox startReachTextBox;
        private System.Windows.Forms.ComboBox hemisphereComboBox;
        private System.Windows.Forms.Label hemisphereLabel;
    }
}

