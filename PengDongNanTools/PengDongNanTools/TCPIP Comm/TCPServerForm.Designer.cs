namespace PengDongNanTools
    {
    partial class TCPServerForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCPServerForm));
            this.Server_GroupBox2 = new System.Windows.Forms.GroupBox();
            this.picHelp = new System.Windows.Forms.PictureBox();
            this.chkGB2312 = new System.Windows.Forms.CheckBox();
            this.picEnglish = new System.Windows.Forms.PictureBox();
            this.chkSendFile = new System.Windows.Forms.CheckBox();
            this.picChinese = new System.Windows.Forms.PictureBox();
            this.chkAutoSend = new System.Windows.Forms.CheckBox();
            this.lblMS = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.lblAutoSendInterval = new System.Windows.Forms.Label();
            this.txtAutoSendInterval = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtMultiServerSend = new System.Windows.Forms.TextBox();
            this.chkMultiServerSendHEX = new System.Windows.Forms.CheckBox();
            this.btnMultiListen = new System.Windows.Forms.Button();
            this.lblCurrentIndexForServerListView = new System.Windows.Forms.Label();
            this.chkSingleServerSendHEX = new System.Windows.Forms.CheckBox();
            this.btnReviseServerRecord = new System.Windows.Forms.Button();
            this.btnDelServerRecordInListView = new System.Windows.Forms.Button();
            this.txtLocalServerIPAddress = new System.Windows.Forms.TextBox();
            this.txtServerListenPort = new System.Windows.Forms.TextBox();
            this.btnAddPortForServer = new System.Windows.Forms.Button();
            this.ServerListView2 = new System.Windows.Forms.ListView();
            this.ColumnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ServerListenPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ServerListenPortTested = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeadServreEnding = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblEndingForServerSendingMessage = new System.Windows.Forms.Label();
            this.txtCustomizedEndingCodeForServer = new System.Windows.Forms.TextBox();
            this.cmbSuffixForServer = new System.Windows.Forms.ComboBox();
            this.lblLocalServerIPAddress = new System.Windows.Forms.Label();
            this.btnServerSendToClient = new System.Windows.Forms.Button();
            this.txtSingleServerSend = new System.Windows.Forms.TextBox();
            this.btnCloseServer = new System.Windows.Forms.Button();
            this.btnListen = new System.Windows.Forms.Button();
            this.lblServerListeningPort = new System.Windows.Forms.Label();
            this.lblAbout = new System.Windows.Forms.Label();
            this.rtbTCPIPHistory = new System.Windows.Forms.RichTextBox();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rtbCurrentReceived = new System.Windows.Forms.RichTextBox();
            this.Server_GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEnglish)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picChinese)).BeginInit();
            this.SuspendLayout();
            // 
            // Server_GroupBox2
            // 
            this.Server_GroupBox2.Controls.Add(this.picHelp);
            this.Server_GroupBox2.Controls.Add(this.chkGB2312);
            this.Server_GroupBox2.Controls.Add(this.picEnglish);
            this.Server_GroupBox2.Controls.Add(this.chkSendFile);
            this.Server_GroupBox2.Controls.Add(this.picChinese);
            this.Server_GroupBox2.Controls.Add(this.chkAutoSend);
            this.Server_GroupBox2.Controls.Add(this.lblMS);
            this.Server_GroupBox2.Controls.Add(this.Label2);
            this.Server_GroupBox2.Controls.Add(this.lblAutoSendInterval);
            this.Server_GroupBox2.Controls.Add(this.txtAutoSendInterval);
            this.Server_GroupBox2.Controls.Add(this.Label1);
            this.Server_GroupBox2.Controls.Add(this.txtMultiServerSend);
            this.Server_GroupBox2.Controls.Add(this.chkMultiServerSendHEX);
            this.Server_GroupBox2.Controls.Add(this.btnMultiListen);
            this.Server_GroupBox2.Controls.Add(this.lblCurrentIndexForServerListView);
            this.Server_GroupBox2.Controls.Add(this.chkSingleServerSendHEX);
            this.Server_GroupBox2.Controls.Add(this.btnReviseServerRecord);
            this.Server_GroupBox2.Controls.Add(this.btnDelServerRecordInListView);
            this.Server_GroupBox2.Controls.Add(this.txtLocalServerIPAddress);
            this.Server_GroupBox2.Controls.Add(this.txtServerListenPort);
            this.Server_GroupBox2.Controls.Add(this.btnAddPortForServer);
            this.Server_GroupBox2.Controls.Add(this.ServerListView2);
            this.Server_GroupBox2.Controls.Add(this.lblEndingForServerSendingMessage);
            this.Server_GroupBox2.Controls.Add(this.txtCustomizedEndingCodeForServer);
            this.Server_GroupBox2.Controls.Add(this.cmbSuffixForServer);
            this.Server_GroupBox2.Controls.Add(this.lblLocalServerIPAddress);
            this.Server_GroupBox2.Controls.Add(this.btnServerSendToClient);
            this.Server_GroupBox2.Controls.Add(this.txtSingleServerSend);
            this.Server_GroupBox2.Controls.Add(this.btnCloseServer);
            this.Server_GroupBox2.Controls.Add(this.btnListen);
            this.Server_GroupBox2.Controls.Add(this.lblServerListeningPort);
            this.Server_GroupBox2.Location = new System.Drawing.Point(11, 23);
            this.Server_GroupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Server_GroupBox2.Name = "Server_GroupBox2";
            this.Server_GroupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Server_GroupBox2.Size = new System.Drawing.Size(473, 540);
            this.Server_GroupBox2.TabIndex = 19;
            this.Server_GroupBox2.TabStop = false;
            this.Server_GroupBox2.Text = "Be Server";
            // 
            // picHelp
            // 
            this.picHelp.Image = global::PengDongNanTools.Properties.Resources.Help;
            this.picHelp.Location = new System.Drawing.Point(119, 498);
            this.picHelp.Margin = new System.Windows.Forms.Padding(4);
            this.picHelp.Name = "picHelp";
            this.picHelp.Size = new System.Drawing.Size(25, 25);
            this.picHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHelp.TabIndex = 161;
            this.picHelp.TabStop = false;
            this.ToolTip1.SetToolTip(this.picHelp, "帮助信息");
            this.picHelp.Click += new System.EventHandler(this.picHelp_Click);
            // 
            // chkGB2312
            // 
            this.chkGB2312.AutoSize = true;
            this.chkGB2312.Location = new System.Drawing.Point(176, 505);
            this.chkGB2312.Margin = new System.Windows.Forms.Padding(4);
            this.chkGB2312.Name = "chkGB2312";
            this.chkGB2312.Size = new System.Drawing.Size(77, 19);
            this.chkGB2312.TabIndex = 152;
            this.chkGB2312.Text = "GB2312";
            this.ToolTip1.SetToolTip(this.chkGB2312, "对应串口端口是否读取16进制");
            this.chkGB2312.UseVisualStyleBackColor = true;
            this.chkGB2312.CheckedChanged += new System.EventHandler(this.chkGB2312_CheckedChanged);
            // 
            // picEnglish
            // 
            this.picEnglish.Image = global::PengDongNanTools.Properties.Resources.FLGUK;
            this.picEnglish.Location = new System.Drawing.Point(69, 498);
            this.picEnglish.Margin = new System.Windows.Forms.Padding(4);
            this.picEnglish.Name = "picEnglish";
            this.picEnglish.Size = new System.Drawing.Size(25, 25);
            this.picEnglish.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picEnglish.TabIndex = 160;
            this.picEnglish.TabStop = false;
            this.ToolTip1.SetToolTip(this.picEnglish, "Show the interface in English");
            this.picEnglish.Click += new System.EventHandler(this.picEnglish_Click);
            // 
            // chkSendFile
            // 
            this.chkSendFile.AutoSize = true;
            this.chkSendFile.Location = new System.Drawing.Point(283, 505);
            this.chkSendFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkSendFile.Name = "chkSendFile";
            this.chkSendFile.Size = new System.Drawing.Size(101, 19);
            this.chkSendFile.TabIndex = 149;
            this.chkSendFile.Text = "Send File";
            this.chkSendFile.UseVisualStyleBackColor = true;
            this.chkSendFile.CheckedChanged += new System.EventHandler(this.chkSendFile_CheckedChanged);
            // 
            // picChinese
            // 
            this.picChinese.Image = global::PengDongNanTools.Properties.Resources.ChineseFlag;
            this.picChinese.Location = new System.Drawing.Point(19, 498);
            this.picChinese.Margin = new System.Windows.Forms.Padding(4);
            this.picChinese.Name = "picChinese";
            this.picChinese.Size = new System.Drawing.Size(25, 25);
            this.picChinese.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picChinese.TabIndex = 159;
            this.picChinese.TabStop = false;
            this.ToolTip1.SetToolTip(this.picChinese, "界面语言切换为中文");
            this.picChinese.Click += new System.EventHandler(this.picChinese_Click);
            // 
            // chkAutoSend
            // 
            this.chkAutoSend.AutoSize = true;
            this.chkAutoSend.Location = new System.Drawing.Point(283, 469);
            this.chkAutoSend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkAutoSend.Name = "chkAutoSend";
            this.chkAutoSend.Size = new System.Drawing.Size(101, 19);
            this.chkAutoSend.TabIndex = 150;
            this.chkAutoSend.Text = "Auto Send";
            this.chkAutoSend.UseVisualStyleBackColor = true;
            this.chkAutoSend.CheckedChanged += new System.EventHandler(this.chkAutoSend_CheckedChanged);
            // 
            // lblMS
            // 
            this.lblMS.AutoSize = true;
            this.lblMS.Location = new System.Drawing.Point(236, 469);
            this.lblMS.Name = "lblMS";
            this.lblMS.Size = new System.Drawing.Size(23, 15);
            this.lblMS.TabIndex = 151;
            this.lblMS.Text = "ms";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(11, 386);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(15, 15);
            this.Label2.TabIndex = 23;
            this.Label2.Text = "S";
            // 
            // lblAutoSendInterval
            // 
            this.lblAutoSendInterval.AutoSize = true;
            this.lblAutoSendInterval.Location = new System.Drawing.Point(12, 469);
            this.lblAutoSendInterval.Name = "lblAutoSendInterval";
            this.lblAutoSendInterval.Size = new System.Drawing.Size(159, 15);
            this.lblAutoSendInterval.TabIndex = 147;
            this.lblAutoSendInterval.Text = "Auto Send Interval:";
            // 
            // txtAutoSendInterval
            // 
            this.txtAutoSendInterval.Location = new System.Drawing.Point(177, 462);
            this.txtAutoSendInterval.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAutoSendInterval.MaxLength = 10;
            this.txtAutoSendInterval.Name = "txtAutoSendInterval";
            this.txtAutoSendInterval.Size = new System.Drawing.Size(53, 25);
            this.txtAutoSendInterval.TabIndex = 148;
            this.txtAutoSendInterval.Text = "500";
            this.txtAutoSendInterval.TextChanged += new System.EventHandler(this.txtAutoSendInterval_TextChanged);
            this.txtAutoSendInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAutoSendInterval_KeyPress);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(11, 419);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(15, 15);
            this.Label1.TabIndex = 20;
            this.Label1.Text = "M";
            // 
            // txtMultiServerSend
            // 
            this.txtMultiServerSend.BackColor = System.Drawing.Color.White;
            this.txtMultiServerSend.ForeColor = System.Drawing.Color.Black;
            this.txtMultiServerSend.Location = new System.Drawing.Point(29, 414);
            this.txtMultiServerSend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMultiServerSend.Multiline = true;
            this.txtMultiServerSend.Name = "txtMultiServerSend";
            this.txtMultiServerSend.Size = new System.Drawing.Size(329, 25);
            this.txtMultiServerSend.TabIndex = 22;
            this.ToolTip1.SetToolTip(this.txtMultiServerSend, "多端口监听服务器的发送内容");
            this.txtMultiServerSend.TextChanged += new System.EventHandler(this.txtMultiServerSend_TextChanged);
            this.txtMultiServerSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMultiServerSend_KeyDown);
            // 
            // chkMultiServerSendHEX
            // 
            this.chkMultiServerSendHEX.AutoSize = true;
            this.chkMultiServerSendHEX.Location = new System.Drawing.Point(383, 414);
            this.chkMultiServerSendHEX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkMultiServerSendHEX.Name = "chkMultiServerSendHEX";
            this.chkMultiServerSendHEX.Size = new System.Drawing.Size(53, 19);
            this.chkMultiServerSendHEX.TabIndex = 21;
            this.chkMultiServerSendHEX.Text = "HEX";
            this.chkMultiServerSendHEX.UseVisualStyleBackColor = true;
            this.chkMultiServerSendHEX.CheckedChanged += new System.EventHandler(this.chkMultiServerSendHEX_CheckedChanged);
            // 
            // btnMultiListen
            // 
            this.btnMultiListen.ForeColor = System.Drawing.Color.Black;
            this.btnMultiListen.Location = new System.Drawing.Point(29, 340);
            this.btnMultiListen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMultiListen.Name = "btnMultiListen";
            this.btnMultiListen.Size = new System.Drawing.Size(120, 29);
            this.btnMultiListen.TabIndex = 17;
            this.btnMultiListen.Text = "Multi Listen";
            this.ToolTip1.SetToolTip(this.btnMultiListen, "多端口监听：先勾选两个以上需要监听的端口");
            this.btnMultiListen.UseVisualStyleBackColor = true;
            this.btnMultiListen.Click += new System.EventHandler(this.btnMultiListen_Click);
            // 
            // lblCurrentIndexForServerListView
            // 
            this.lblCurrentIndexForServerListView.AutoSize = true;
            this.lblCurrentIndexForServerListView.Location = new System.Drawing.Point(385, 20);
            this.lblCurrentIndexForServerListView.Name = "lblCurrentIndexForServerListView";
            this.lblCurrentIndexForServerListView.Size = new System.Drawing.Size(67, 15);
            this.lblCurrentIndexForServerListView.TabIndex = 8;
            this.lblCurrentIndexForServerListView.Text = "当前索引";
            // 
            // chkSingleServerSendHEX
            // 
            this.chkSingleServerSendHEX.AutoSize = true;
            this.chkSingleServerSendHEX.Location = new System.Drawing.Point(384, 382);
            this.chkSingleServerSendHEX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkSingleServerSendHEX.Name = "chkSingleServerSendHEX";
            this.chkSingleServerSendHEX.Size = new System.Drawing.Size(53, 19);
            this.chkSingleServerSendHEX.TabIndex = 16;
            this.chkSingleServerSendHEX.Text = "HEX";
            this.chkSingleServerSendHEX.UseVisualStyleBackColor = true;
            this.chkSingleServerSendHEX.CheckedChanged += new System.EventHandler(this.chkSingleServerSendHEX_CheckedChanged);
            // 
            // btnReviseServerRecord
            // 
            this.btnReviseServerRecord.ForeColor = System.Drawing.Color.Black;
            this.btnReviseServerRecord.Location = new System.Drawing.Point(387, 295);
            this.btnReviseServerRecord.Margin = new System.Windows.Forms.Padding(4);
            this.btnReviseServerRecord.Name = "btnReviseServerRecord";
            this.btnReviseServerRecord.Size = new System.Drawing.Size(67, 29);
            this.btnReviseServerRecord.TabIndex = 5;
            this.btnReviseServerRecord.Text = "Revise";
            this.ToolTip1.SetToolTip(this.btnReviseServerRecord, "修改列表中当前选中的记录");
            this.btnReviseServerRecord.UseVisualStyleBackColor = true;
            this.btnReviseServerRecord.Click += new System.EventHandler(this.btnReviseServerRecord_Click);
            // 
            // btnDelServerRecordInListView
            // 
            this.btnDelServerRecordInListView.ForeColor = System.Drawing.Color.Black;
            this.btnDelServerRecordInListView.Location = new System.Drawing.Point(384, 205);
            this.btnDelServerRecordInListView.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelServerRecordInListView.Name = "btnDelServerRecordInListView";
            this.btnDelServerRecordInListView.Size = new System.Drawing.Size(67, 29);
            this.btnDelServerRecordInListView.TabIndex = 3;
            this.btnDelServerRecordInListView.Text = "Del";
            this.ToolTip1.SetToolTip(this.btnDelServerRecordInListView, "删除列表中当前选中的记录");
            this.btnDelServerRecordInListView.UseVisualStyleBackColor = true;
            this.btnDelServerRecordInListView.Click += new System.EventHandler(this.btnDelServerRecordInListView_Click);
            // 
            // txtLocalServerIPAddress
            // 
            this.txtLocalServerIPAddress.Location = new System.Drawing.Point(177, 208);
            this.txtLocalServerIPAddress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtLocalServerIPAddress.Name = "txtLocalServerIPAddress";
            this.txtLocalServerIPAddress.ReadOnly = true;
            this.txtLocalServerIPAddress.Size = new System.Drawing.Size(141, 25);
            this.txtLocalServerIPAddress.TabIndex = 7;
            this.txtLocalServerIPAddress.TextChanged += new System.EventHandler(this.txtLocalServerIPAddress_TextChanged);
            // 
            // txtServerListenPort
            // 
            this.txtServerListenPort.Location = new System.Drawing.Point(177, 254);
            this.txtServerListenPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtServerListenPort.MaxLength = 5;
            this.txtServerListenPort.Name = "txtServerListenPort";
            this.txtServerListenPort.Size = new System.Drawing.Size(140, 25);
            this.txtServerListenPort.TabIndex = 8;
            this.txtServerListenPort.TextChanged += new System.EventHandler(this.txtServerListenPort_TextChanged);
            // 
            // btnAddPortForServer
            // 
            this.btnAddPortForServer.ForeColor = System.Drawing.Color.Black;
            this.btnAddPortForServer.Location = new System.Drawing.Point(384, 250);
            this.btnAddPortForServer.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddPortForServer.Name = "btnAddPortForServer";
            this.btnAddPortForServer.Size = new System.Drawing.Size(67, 29);
            this.btnAddPortForServer.TabIndex = 4;
            this.btnAddPortForServer.Text = "Add";
            this.ToolTip1.SetToolTip(this.btnAddPortForServer, "添加记录到列表");
            this.btnAddPortForServer.UseVisualStyleBackColor = true;
            this.btnAddPortForServer.Click += new System.EventHandler(this.btnAddPortForServer_Click);
            // 
            // ServerListView2
            // 
            this.ServerListView2.AutoArrange = false;
            this.ServerListView2.BackColor = System.Drawing.SystemColors.Control;
            this.ServerListView2.CheckBoxes = true;
            this.ServerListView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader2,
            this.ServerListenPort,
            this.ServerListenPortTested,
            this.ColumnHeadServreEnding});
            this.ServerListView2.ForeColor = System.Drawing.Color.Black;
            this.ServerListView2.FullRowSelect = true;
            this.ServerListView2.HideSelection = false;
            this.ServerListView2.Location = new System.Drawing.Point(15, 46);
            this.ServerListView2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ServerListView2.MultiSelect = false;
            this.ServerListView2.Name = "ServerListView2";
            this.ServerListView2.Size = new System.Drawing.Size(435, 146);
            this.ServerListView2.TabIndex = 11;
            this.ServerListView2.UseCompatibleStateImageBehavior = false;
            this.ServerListView2.View = System.Windows.Forms.View.Details;
            this.ServerListView2.SelectedIndexChanged += new System.EventHandler(this.ServerListView2_SelectedIndexChanged);
            // 
            // ColumnHeader2
            // 
            this.ColumnHeader2.Text = "";
            this.ColumnHeader2.Width = 0;
            // 
            // ServerListenPort
            // 
            this.ServerListenPort.Text = "Listen Port";
            this.ServerListenPort.Width = 100;
            // 
            // ServerListenPortTested
            // 
            this.ServerListenPortTested.Text = "Tested";
            this.ServerListenPortTested.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ServerListenPortTested.Width = 80;
            // 
            // ColumnHeadServreEnding
            // 
            this.ColumnHeadServreEnding.Text = "Ending";
            this.ColumnHeadServreEnding.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeadServreEnding.Width = 80;
            // 
            // lblEndingForServerSendingMessage
            // 
            this.lblEndingForServerSendingMessage.AutoSize = true;
            this.lblEndingForServerSendingMessage.Location = new System.Drawing.Point(27, 302);
            this.lblEndingForServerSendingMessage.Name = "lblEndingForServerSendingMessage";
            this.lblEndingForServerSendingMessage.Size = new System.Drawing.Size(63, 15);
            this.lblEndingForServerSendingMessage.TabIndex = 14;
            this.lblEndingForServerSendingMessage.Text = "Ending:";
            // 
            // txtCustomizedEndingCodeForServer
            // 
            this.txtCustomizedEndingCodeForServer.Location = new System.Drawing.Point(267, 300);
            this.txtCustomizedEndingCodeForServer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCustomizedEndingCodeForServer.MaxLength = 10;
            this.txtCustomizedEndingCodeForServer.Name = "txtCustomizedEndingCodeForServer";
            this.txtCustomizedEndingCodeForServer.Size = new System.Drawing.Size(51, 25);
            this.txtCustomizedEndingCodeForServer.TabIndex = 10;
            this.txtCustomizedEndingCodeForServer.Visible = false;
            this.txtCustomizedEndingCodeForServer.TextChanged += new System.EventHandler(this.txtCustomizedEndingCodeForServer_TextChanged);
            this.txtCustomizedEndingCodeForServer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCustomizedEndingCodeForServer_KeyDown);
            // 
            // cmbSuffixForServer
            // 
            this.cmbSuffixForServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSuffixForServer.FormattingEnabled = true;
            this.cmbSuffixForServer.Items.AddRange(new object[] {
            "None",
            "LF",
            "CR",
            "CR+LF",
            "自定义"});
            this.cmbSuffixForServer.Location = new System.Drawing.Point(177, 300);
            this.cmbSuffixForServer.Margin = new System.Windows.Forms.Padding(4);
            this.cmbSuffixForServer.Name = "cmbSuffixForServer";
            this.cmbSuffixForServer.Size = new System.Drawing.Size(76, 23);
            this.cmbSuffixForServer.TabIndex = 9;
            this.cmbSuffixForServer.SelectedIndexChanged += new System.EventHandler(this.cmbSuffixForServer_SelectedIndexChanged);
            // 
            // lblLocalServerIPAddress
            // 
            this.lblLocalServerIPAddress.AutoSize = true;
            this.lblLocalServerIPAddress.Location = new System.Drawing.Point(27, 218);
            this.lblLocalServerIPAddress.Name = "lblLocalServerIPAddress";
            this.lblLocalServerIPAddress.Size = new System.Drawing.Size(143, 15);
            this.lblLocalServerIPAddress.TabIndex = 12;
            this.lblLocalServerIPAddress.Text = "Local IP Address:";
            // 
            // btnServerSendToClient
            // 
            this.btnServerSendToClient.ForeColor = System.Drawing.Color.Black;
            this.btnServerSendToClient.Location = new System.Drawing.Point(383, 340);
            this.btnServerSendToClient.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnServerSendToClient.Name = "btnServerSendToClient";
            this.btnServerSendToClient.Size = new System.Drawing.Size(67, 29);
            this.btnServerSendToClient.TabIndex = 2;
            this.btnServerSendToClient.Text = "Send";
            this.ToolTip1.SetToolTip(this.btnServerSendToClient, "发送内容到客服端【包括单端口和多端口监听】");
            this.btnServerSendToClient.UseVisualStyleBackColor = true;
            this.btnServerSendToClient.Click += new System.EventHandler(this.btnServerSendToClient_Click);
            // 
            // txtSingleServerSend
            // 
            this.txtSingleServerSend.BackColor = System.Drawing.Color.White;
            this.txtSingleServerSend.ForeColor = System.Drawing.Color.Black;
            this.txtSingleServerSend.Location = new System.Drawing.Point(29, 382);
            this.txtSingleServerSend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSingleServerSend.Multiline = true;
            this.txtSingleServerSend.Name = "txtSingleServerSend";
            this.txtSingleServerSend.Size = new System.Drawing.Size(329, 25);
            this.txtSingleServerSend.TabIndex = 6;
            this.ToolTip1.SetToolTip(this.txtSingleServerSend, "单端口监听服务器的发送内容");
            this.txtSingleServerSend.TextChanged += new System.EventHandler(this.txtSingleServerSend_TextChanged);
            this.txtSingleServerSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSingleServerSend_KeyDown);
            // 
            // btnCloseServer
            // 
            this.btnCloseServer.ForeColor = System.Drawing.Color.Black;
            this.btnCloseServer.Location = new System.Drawing.Point(283, 340);
            this.btnCloseServer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCloseServer.Name = "btnCloseServer";
            this.btnCloseServer.Size = new System.Drawing.Size(75, 29);
            this.btnCloseServer.TabIndex = 1;
            this.btnCloseServer.Text = "Close";
            this.ToolTip1.SetToolTip(this.btnCloseServer, "服务器关闭单个监听端口");
            this.btnCloseServer.UseVisualStyleBackColor = true;
            this.btnCloseServer.Click += new System.EventHandler(this.btnCloseServer_Click);
            // 
            // btnListen
            // 
            this.btnListen.ForeColor = System.Drawing.Color.Black;
            this.btnListen.Location = new System.Drawing.Point(183, 340);
            this.btnListen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(75, 29);
            this.btnListen.TabIndex = 0;
            this.btnListen.Text = "Listen";
            this.ToolTip1.SetToolTip(this.btnListen, "服务器开始启动监听单个端口");
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // lblServerListeningPort
            // 
            this.lblServerListeningPort.AutoSize = true;
            this.lblServerListeningPort.Location = new System.Drawing.Point(27, 260);
            this.lblServerListeningPort.Name = "lblServerListeningPort";
            this.lblServerListeningPort.Size = new System.Drawing.Size(127, 15);
            this.lblServerListeningPort.TabIndex = 13;
            this.lblServerListeningPort.Text = "Listening Port:";
            // 
            // lblAbout
            // 
            this.lblAbout.AutoSize = true;
            this.lblAbout.Font = new System.Drawing.Font("宋体", 8F);
            this.lblAbout.Location = new System.Drawing.Point(355, 9);
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.Size = new System.Drawing.Size(119, 14);
            this.lblAbout.TabIndex = 21;
            this.lblAbout.Text = "软件作者：彭东南";
            this.ToolTip1.SetToolTip(this.lblAbout, "点击显示作者的信息");
            // 
            // rtbTCPIPHistory
            // 
            this.rtbTCPIPHistory.BackColor = System.Drawing.Color.Black;
            this.rtbTCPIPHistory.ForeColor = System.Drawing.Color.Lime;
            this.rtbTCPIPHistory.Location = new System.Drawing.Point(499, 23);
            this.rtbTCPIPHistory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtbTCPIPHistory.Name = "rtbTCPIPHistory";
            this.rtbTCPIPHistory.ReadOnly = true;
            this.rtbTCPIPHistory.Size = new System.Drawing.Size(811, 462);
            this.rtbTCPIPHistory.TabIndex = 20;
            this.rtbTCPIPHistory.Text = "";
            this.rtbTCPIPHistory.TextChanged += new System.EventHandler(this.rtbTCPIPHistory_TextChanged);
            this.rtbTCPIPHistory.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.rtbTCPIPHistory_MouseDoubleClick);
            this.rtbTCPIPHistory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rtbTCPIPHistory_MouseDown);
            // 
            // rtbCurrentReceived
            // 
            this.rtbCurrentReceived.BackColor = System.Drawing.Color.Black;
            this.rtbCurrentReceived.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbCurrentReceived.ForeColor = System.Drawing.Color.Lime;
            this.rtbCurrentReceived.Location = new System.Drawing.Point(499, 491);
            this.rtbCurrentReceived.Margin = new System.Windows.Forms.Padding(4);
            this.rtbCurrentReceived.Name = "rtbCurrentReceived";
            this.rtbCurrentReceived.ReadOnly = true;
            this.rtbCurrentReceived.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtbCurrentReceived.Size = new System.Drawing.Size(811, 73);
            this.rtbCurrentReceived.TabIndex = 158;
            this.rtbCurrentReceived.Text = "";
            this.ToolTip1.SetToolTip(this.rtbCurrentReceived, "当前接收内容");
            // 
            // TCPServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(1325, 568);
            this.Controls.Add(this.rtbCurrentReceived);
            this.Controls.Add(this.Server_GroupBox2);
            this.Controls.Add(this.lblAbout);
            this.Controls.Add(this.rtbTCPIPHistory);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1331, 603);
            this.Name = "TCPServerForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TCP/IP Server Multi Listening Software";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TCPServerForm_FormClosing);
            this.Load += new System.EventHandler(this.TCPServerForm_Load);
            this.Server_GroupBox2.ResumeLayout(false);
            this.Server_GroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEnglish)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picChinese)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion

        internal System.Windows.Forms.GroupBox Server_GroupBox2;
        internal System.Windows.Forms.CheckBox chkSendFile;
        internal System.Windows.Forms.CheckBox chkAutoSend;
        internal System.Windows.Forms.Label lblMS;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label lblAutoSendInterval;
        internal System.Windows.Forms.TextBox txtAutoSendInterval;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox txtMultiServerSend;
        internal System.Windows.Forms.ToolTip ToolTip1;
        internal System.Windows.Forms.CheckBox chkMultiServerSendHEX;
        internal System.Windows.Forms.Button btnMultiListen;
        internal System.Windows.Forms.Label lblCurrentIndexForServerListView;
        internal System.Windows.Forms.CheckBox chkSingleServerSendHEX;
        internal System.Windows.Forms.Button btnReviseServerRecord;
        internal System.Windows.Forms.Button btnDelServerRecordInListView;
        internal System.Windows.Forms.TextBox txtLocalServerIPAddress;
        internal System.Windows.Forms.TextBox txtServerListenPort;
        internal System.Windows.Forms.Button btnAddPortForServer;
        internal System.Windows.Forms.ListView ServerListView2;
        internal System.Windows.Forms.ColumnHeader ColumnHeader2;
        internal System.Windows.Forms.ColumnHeader ServerListenPort;
        internal System.Windows.Forms.ColumnHeader ServerListenPortTested;
        internal System.Windows.Forms.ColumnHeader ColumnHeadServreEnding;
        internal System.Windows.Forms.Label lblEndingForServerSendingMessage;
        internal System.Windows.Forms.TextBox txtCustomizedEndingCodeForServer;
        internal System.Windows.Forms.ComboBox cmbSuffixForServer;
        internal System.Windows.Forms.Label lblLocalServerIPAddress;
        internal System.Windows.Forms.Button btnServerSendToClient;
        internal System.Windows.Forms.TextBox txtSingleServerSend;
        internal System.Windows.Forms.Button btnCloseServer;
        internal System.Windows.Forms.Button btnListen;
        internal System.Windows.Forms.Label lblServerListeningPort;
        internal System.Windows.Forms.Label lblAbout;
        internal System.Windows.Forms.RichTextBox rtbTCPIPHistory;
        internal System.Windows.Forms.RichTextBox rtbCurrentReceived;
        internal System.Windows.Forms.CheckBox chkGB2312;
        internal System.Windows.Forms.PictureBox picHelp;
        internal System.Windows.Forms.PictureBox picEnglish;
        internal System.Windows.Forms.PictureBox picChinese;
        }
    }