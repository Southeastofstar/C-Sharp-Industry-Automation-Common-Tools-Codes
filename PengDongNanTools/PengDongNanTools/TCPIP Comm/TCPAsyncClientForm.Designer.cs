namespace PengDongNanTools
    {
    partial class TCPAsyncClientForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCPAsyncClientForm));
            this.btnConnectToServer = new System.Windows.Forms.Button();
            this.btnCloseClient = new System.Windows.Forms.Button();
            this.chkClientSendHEX = new System.Windows.Forms.CheckBox();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblMS = new System.Windows.Forms.Label();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnReviseClientRecord = new System.Windows.Forms.Button();
            this.btnSearchIP = new System.Windows.Forms.Button();
            this.btnDelClientRecordInListView = new System.Windows.Forms.Button();
            this.btnAddPortForClient = new System.Windows.Forms.Button();
            this.btnClientSendToServer = new System.Windows.Forms.Button();
            this.ShutDownServer_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShutDownClient_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblAutoSendInterval = new System.Windows.Forms.Label();
            this.txtAutoSendInterval = new System.Windows.Forms.TextBox();
            this.Setting_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkAutoSend = new System.Windows.Forms.CheckBox();
            this.cmbSuffixForClient = new System.Windows.Forms.ComboBox();
            this.chkSendFile = new System.Windows.Forms.CheckBox();
            this.txtCustomizedEndingCodeForClient = new System.Windows.Forms.TextBox();
            this.mtxt_2 = new System.Windows.Forms.MaskedTextBox();
            this.mtxt_3 = new System.Windows.Forms.MaskedTextBox();
            this.mtxt_4 = new System.Windows.Forms.MaskedTextBox();
            this.lblIPNumber = new System.Windows.Forms.Label();
            this.lblCurrentIndexForClientListView = new System.Windows.Forms.Label();
            this.Client_GroupBox1 = new System.Windows.Forms.GroupBox();
            this.btnMultiConnect = new System.Windows.Forms.Button();
            this.lblIPAddressSection = new System.Windows.Forms.Label();
            this.txtTargetServerIPAddress = new System.Windows.Forms.TextBox();
            this.txtTargetServerPort = new System.Windows.Forms.TextBox();
            this.ClientListView1 = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ServerIPAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ServerPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ClientTestedWithServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeadClientEnding = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblEndingForClientSendingMessage = new System.Windows.Forms.Label();
            this.lblTargetServerIPAddress = new System.Windows.Forms.Label();
            this.lblTargetServerPort = new System.Windows.Forms.Label();
            this.lblIPCount = new System.Windows.Forms.Label();
            this.rtbTCPIPHistory = new System.Windows.Forms.RichTextBox();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.NetworkStatus_Menu = new System.Windows.Forms.ToolStripMenuItem();
            this.SavePara_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SearchPCName_ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.MACToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LanguageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EnglishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.中文ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtSendToServer = new System.Windows.Forms.TextBox();
            this.lblLocalIPAddress = new System.Windows.Forms.Label();
            this.Client_GroupBox1.SuspendLayout();
            this.MenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnectToServer
            // 
            this.btnConnectToServer.Location = new System.Drawing.Point(252, 501);
            this.btnConnectToServer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnConnectToServer.Name = "btnConnectToServer";
            this.btnConnectToServer.Size = new System.Drawing.Size(100, 29);
            this.btnConnectToServer.TabIndex = 0;
            this.btnConnectToServer.Text = "Connect";
            this.ToolTip.SetToolTip(this.btnConnectToServer, "连接服务器");
            this.btnConnectToServer.UseVisualStyleBackColor = true;
            this.btnConnectToServer.Click += new System.EventHandler(this.btnConnectToServer_Click);
            // 
            // btnCloseClient
            // 
            this.btnCloseClient.Location = new System.Drawing.Point(398, 501);
            this.btnCloseClient.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCloseClient.Name = "btnCloseClient";
            this.btnCloseClient.Size = new System.Drawing.Size(100, 29);
            this.btnCloseClient.TabIndex = 1;
            this.btnCloseClient.Text = "Close";
            this.ToolTip.SetToolTip(this.btnCloseClient, "断开连接");
            this.btnCloseClient.UseVisualStyleBackColor = true;
            this.btnCloseClient.Click += new System.EventHandler(this.btnCloseClient_Click);
            // 
            // chkClientSendHEX
            // 
            this.chkClientSendHEX.AutoSize = true;
            this.chkClientSendHEX.Location = new System.Drawing.Point(1197, 586);
            this.chkClientSendHEX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkClientSendHEX.Name = "chkClientSendHEX";
            this.chkClientSendHEX.Size = new System.Drawing.Size(53, 19);
            this.chkClientSendHEX.TabIndex = 154;
            this.chkClientSendHEX.Text = "HEX";
            this.chkClientSendHEX.UseVisualStyleBackColor = true;
            this.chkClientSendHEX.CheckedChanged += new System.EventHandler(this.chkClientSendHEX_CheckedChanged);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.AutoToolTip = true;
            this.AboutToolStripMenuItem.Image = global::PengDongNanTools.Properties.Resources.Help;
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(83, 24);
            this.AboutToolStripMenuItem.Text = "About";
            this.AboutToolStripMenuItem.ToolTipText = "关于作者和软件的提示";
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // lblMS
            // 
            this.lblMS.AutoSize = true;
            this.lblMS.Location = new System.Drawing.Point(887, 586);
            this.lblMS.Name = "lblMS";
            this.lblMS.Size = new System.Drawing.Size(23, 15);
            this.lblMS.TabIndex = 159;
            this.lblMS.Text = "ms";
            // 
            // ToolTip
            // 
            this.ToolTip.ShowAlways = true;
            // 
            // btnReviseClientRecord
            // 
            this.btnReviseClientRecord.Location = new System.Drawing.Point(400, 439);
            this.btnReviseClientRecord.Margin = new System.Windows.Forms.Padding(4);
            this.btnReviseClientRecord.Name = "btnReviseClientRecord";
            this.btnReviseClientRecord.Size = new System.Drawing.Size(100, 29);
            this.btnReviseClientRecord.TabIndex = 5;
            this.btnReviseClientRecord.Text = "Revise";
            this.ToolTip.SetToolTip(this.btnReviseClientRecord, "修改列表中当前选中的记录");
            this.btnReviseClientRecord.UseVisualStyleBackColor = true;
            this.btnReviseClientRecord.Click += new System.EventHandler(this.btnReviseClientRecord_Click);
            // 
            // btnSearchIP
            // 
            this.btnSearchIP.Location = new System.Drawing.Point(400, 322);
            this.btnSearchIP.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearchIP.Name = "btnSearchIP";
            this.btnSearchIP.Size = new System.Drawing.Size(100, 29);
            this.btnSearchIP.TabIndex = 19;
            this.btnSearchIP.Text = "Search IP";
            this.ToolTip.SetToolTip(this.btnSearchIP, "搜索网络中可用的IP地址");
            this.btnSearchIP.UseVisualStyleBackColor = true;
            this.btnSearchIP.Click += new System.EventHandler(this.btnSearchIP_Click);
            // 
            // btnDelClientRecordInListView
            // 
            this.btnDelClientRecordInListView.Location = new System.Drawing.Point(400, 364);
            this.btnDelClientRecordInListView.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelClientRecordInListView.Name = "btnDelClientRecordInListView";
            this.btnDelClientRecordInListView.Size = new System.Drawing.Size(100, 29);
            this.btnDelClientRecordInListView.TabIndex = 3;
            this.btnDelClientRecordInListView.Text = "Del";
            this.ToolTip.SetToolTip(this.btnDelClientRecordInListView, "删除列表中当前选中的记录");
            this.btnDelClientRecordInListView.UseVisualStyleBackColor = true;
            this.btnDelClientRecordInListView.Click += new System.EventHandler(this.btnDelClientRecordInListView_Click);
            // 
            // btnAddPortForClient
            // 
            this.btnAddPortForClient.Location = new System.Drawing.Point(401, 401);
            this.btnAddPortForClient.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddPortForClient.Name = "btnAddPortForClient";
            this.btnAddPortForClient.Size = new System.Drawing.Size(99, 29);
            this.btnAddPortForClient.TabIndex = 4;
            this.btnAddPortForClient.Text = "Add";
            this.ToolTip.SetToolTip(this.btnAddPortForClient, "添加记录到列表");
            this.btnAddPortForClient.UseVisualStyleBackColor = true;
            this.btnAddPortForClient.Click += new System.EventHandler(this.btnAddPortForClient_Click);
            // 
            // btnClientSendToServer
            // 
            this.btnClientSendToServer.Font = new System.Drawing.Font("宋体", 10F);
            this.btnClientSendToServer.Location = new System.Drawing.Point(1220, 525);
            this.btnClientSendToServer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClientSendToServer.Name = "btnClientSendToServer";
            this.btnClientSendToServer.Size = new System.Drawing.Size(84, 38);
            this.btnClientSendToServer.TabIndex = 148;
            this.btnClientSendToServer.Text = "Send";
            this.ToolTip.SetToolTip(this.btnClientSendToServer, "发送内容到服务器");
            this.btnClientSendToServer.UseVisualStyleBackColor = true;
            this.btnClientSendToServer.Click += new System.EventHandler(this.btnClientSendToServer_Click);
            // 
            // ShutDownServer_ToolStripMenuItem
            // 
            this.ShutDownServer_ToolStripMenuItem.Name = "ShutDownServer_ToolStripMenuItem";
            this.ShutDownServer_ToolStripMenuItem.Size = new System.Drawing.Size(204, 24);
            this.ShutDownServer_ToolStripMenuItem.Text = "ShutDown Server";
            // 
            // ShutDownClient_ToolStripMenuItem
            // 
            this.ShutDownClient_ToolStripMenuItem.Name = "ShutDownClient_ToolStripMenuItem";
            this.ShutDownClient_ToolStripMenuItem.Size = new System.Drawing.Size(204, 24);
            this.ShutDownClient_ToolStripMenuItem.Text = "ShutDown Client";
            // 
            // lblAutoSendInterval
            // 
            this.lblAutoSendInterval.AutoSize = true;
            this.lblAutoSendInterval.Location = new System.Drawing.Point(663, 586);
            this.lblAutoSendInterval.Name = "lblAutoSendInterval";
            this.lblAutoSendInterval.Size = new System.Drawing.Size(159, 15);
            this.lblAutoSendInterval.TabIndex = 155;
            this.lblAutoSendInterval.Text = "Auto Send Interval:";
            // 
            // txtAutoSendInterval
            // 
            this.txtAutoSendInterval.Location = new System.Drawing.Point(828, 579);
            this.txtAutoSendInterval.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAutoSendInterval.MaxLength = 10;
            this.txtAutoSendInterval.Name = "txtAutoSendInterval";
            this.txtAutoSendInterval.Size = new System.Drawing.Size(53, 25);
            this.txtAutoSendInterval.TabIndex = 156;
            this.txtAutoSendInterval.Text = "500";
            this.txtAutoSendInterval.TextChanged += new System.EventHandler(this.txtAutoSendInterval_TextChanged);
            this.txtAutoSendInterval.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAutoSendInterval_KeyDown);
            this.txtAutoSendInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAutoSendInterval_KeyPress);
            // 
            // Setting_ToolStripMenuItem
            // 
            this.Setting_ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShutDownClient_ToolStripMenuItem,
            this.ShutDownServer_ToolStripMenuItem});
            this.Setting_ToolStripMenuItem.Name = "Setting_ToolStripMenuItem";
            this.Setting_ToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.Setting_ToolStripMenuItem.Text = "Setting";
            this.Setting_ToolStripMenuItem.Visible = false;
            // 
            // chkAutoSend
            // 
            this.chkAutoSend.AutoSize = true;
            this.chkAutoSend.Location = new System.Drawing.Point(945, 586);
            this.chkAutoSend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkAutoSend.Name = "chkAutoSend";
            this.chkAutoSend.Size = new System.Drawing.Size(101, 19);
            this.chkAutoSend.TabIndex = 158;
            this.chkAutoSend.Text = "Auto Send";
            this.chkAutoSend.UseVisualStyleBackColor = true;
            this.chkAutoSend.CheckedChanged += new System.EventHandler(this.chkAutoSend_CheckedChanged);
            // 
            // cmbSuffixForClient
            // 
            this.cmbSuffixForClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSuffixForClient.FormattingEnabled = true;
            this.cmbSuffixForClient.Items.AddRange(new object[] {
            "None",
            "LF",
            "CR",
            "CR+LF",
            "自定义"});
            this.cmbSuffixForClient.Location = new System.Drawing.Point(215, 455);
            this.cmbSuffixForClient.Margin = new System.Windows.Forms.Padding(4);
            this.cmbSuffixForClient.Name = "cmbSuffixForClient";
            this.cmbSuffixForClient.Size = new System.Drawing.Size(76, 23);
            this.cmbSuffixForClient.TabIndex = 9;
            this.cmbSuffixForClient.SelectedIndexChanged += new System.EventHandler(this.cmbSuffixForClient_SelectedIndexChanged);
            // 
            // chkSendFile
            // 
            this.chkSendFile.AutoSize = true;
            this.chkSendFile.Location = new System.Drawing.Point(1079, 586);
            this.chkSendFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkSendFile.Name = "chkSendFile";
            this.chkSendFile.Size = new System.Drawing.Size(101, 19);
            this.chkSendFile.TabIndex = 157;
            this.chkSendFile.Text = "Send File";
            this.chkSendFile.UseVisualStyleBackColor = true;
            this.chkSendFile.CheckedChanged += new System.EventHandler(this.chkSendFile_CheckedChanged);
            // 
            // txtCustomizedEndingCodeForClient
            // 
            this.txtCustomizedEndingCodeForClient.Location = new System.Drawing.Point(308, 452);
            this.txtCustomizedEndingCodeForClient.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCustomizedEndingCodeForClient.MaxLength = 10;
            this.txtCustomizedEndingCodeForClient.Name = "txtCustomizedEndingCodeForClient";
            this.txtCustomizedEndingCodeForClient.Size = new System.Drawing.Size(51, 25);
            this.txtCustomizedEndingCodeForClient.TabIndex = 10;
            this.txtCustomizedEndingCodeForClient.Visible = false;
            this.txtCustomizedEndingCodeForClient.TextChanged += new System.EventHandler(this.txtCustomizedEndingCodeForClient_TextChanged);
            this.txtCustomizedEndingCodeForClient.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCustomizedEndingCodeForClient_KeyDown);
            // 
            // mtxt_2
            // 
            this.mtxt_2.Location = new System.Drawing.Point(323, 325);
            this.mtxt_2.Margin = new System.Windows.Forms.Padding(4);
            this.mtxt_2.Name = "mtxt_2";
            this.mtxt_2.PromptChar = ' ';
            this.mtxt_2.Size = new System.Drawing.Size(36, 25);
            this.mtxt_2.TabIndex = 18;
            this.mtxt_2.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.mtxt_2_MaskInputRejected);
            this.mtxt_2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtxt_2_KeyDown);
            this.mtxt_2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mtxt_2_KeyPress);
            // 
            // mtxt_3
            // 
            this.mtxt_3.Location = new System.Drawing.Point(268, 325);
            this.mtxt_3.Margin = new System.Windows.Forms.Padding(4);
            this.mtxt_3.Name = "mtxt_3";
            this.mtxt_3.PromptChar = ' ';
            this.mtxt_3.Size = new System.Drawing.Size(36, 25);
            this.mtxt_3.TabIndex = 17;
            this.mtxt_3.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.mtxt_3_MaskInputRejected);
            this.mtxt_3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtxt_3_KeyDown);
            this.mtxt_3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mtxt_3_KeyPress);
            // 
            // mtxt_4
            // 
            this.mtxt_4.Location = new System.Drawing.Point(215, 325);
            this.mtxt_4.Margin = new System.Windows.Forms.Padding(4);
            this.mtxt_4.Name = "mtxt_4";
            this.mtxt_4.PromptChar = ' ';
            this.mtxt_4.Size = new System.Drawing.Size(36, 25);
            this.mtxt_4.TabIndex = 16;
            this.mtxt_4.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.mtxt_4_MaskInputRejected);
            this.mtxt_4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtxt_4_KeyDown);
            this.mtxt_4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mtxt_4_KeyPress);
            // 
            // lblIPNumber
            // 
            this.lblIPNumber.AutoSize = true;
            this.lblIPNumber.Location = new System.Drawing.Point(360, 42);
            this.lblIPNumber.Name = "lblIPNumber";
            this.lblIPNumber.Size = new System.Drawing.Size(79, 15);
            this.lblIPNumber.TabIndex = 151;
            this.lblIPNumber.Text = "IP Number";
            // 
            // lblCurrentIndexForClientListView
            // 
            this.lblCurrentIndexForClientListView.AutoSize = true;
            this.lblCurrentIndexForClientListView.Location = new System.Drawing.Point(475, 43);
            this.lblCurrentIndexForClientListView.Name = "lblCurrentIndexForClientListView";
            this.lblCurrentIndexForClientListView.Size = new System.Drawing.Size(67, 15);
            this.lblCurrentIndexForClientListView.TabIndex = 152;
            this.lblCurrentIndexForClientListView.Text = "当前索引";
            // 
            // Client_GroupBox1
            // 
            this.Client_GroupBox1.Controls.Add(this.btnMultiConnect);
            this.Client_GroupBox1.Controls.Add(this.lblIPAddressSection);
            this.Client_GroupBox1.Controls.Add(this.btnSearchIP);
            this.Client_GroupBox1.Controls.Add(this.mtxt_2);
            this.Client_GroupBox1.Controls.Add(this.mtxt_3);
            this.Client_GroupBox1.Controls.Add(this.mtxt_4);
            this.Client_GroupBox1.Controls.Add(this.btnReviseClientRecord);
            this.Client_GroupBox1.Controls.Add(this.btnDelClientRecordInListView);
            this.Client_GroupBox1.Controls.Add(this.btnAddPortForClient);
            this.Client_GroupBox1.Controls.Add(this.txtTargetServerIPAddress);
            this.Client_GroupBox1.Controls.Add(this.txtTargetServerPort);
            this.Client_GroupBox1.Controls.Add(this.ClientListView1);
            this.Client_GroupBox1.Controls.Add(this.lblEndingForClientSendingMessage);
            this.Client_GroupBox1.Controls.Add(this.txtCustomizedEndingCodeForClient);
            this.Client_GroupBox1.Controls.Add(this.cmbSuffixForClient);
            this.Client_GroupBox1.Controls.Add(this.btnCloseClient);
            this.Client_GroupBox1.Controls.Add(this.lblTargetServerIPAddress);
            this.Client_GroupBox1.Controls.Add(this.lblTargetServerPort);
            this.Client_GroupBox1.Controls.Add(this.btnConnectToServer);
            this.Client_GroupBox1.Location = new System.Drawing.Point(12, 55);
            this.Client_GroupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Client_GroupBox1.Name = "Client_GroupBox1";
            this.Client_GroupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Client_GroupBox1.Size = new System.Drawing.Size(540, 546);
            this.Client_GroupBox1.TabIndex = 147;
            this.Client_GroupBox1.TabStop = false;
            this.Client_GroupBox1.Text = "Be Client";
            // 
            // btnMultiConnect
            // 
            this.btnMultiConnect.ForeColor = System.Drawing.Color.Black;
            this.btnMultiConnect.Location = new System.Drawing.Point(90, 501);
            this.btnMultiConnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMultiConnect.Name = "btnMultiConnect";
            this.btnMultiConnect.Size = new System.Drawing.Size(120, 29);
            this.btnMultiConnect.TabIndex = 21;
            this.btnMultiConnect.Text = "Multi Connect";
            this.btnMultiConnect.UseVisualStyleBackColor = true;
            this.btnMultiConnect.Click += new System.EventHandler(this.btnMultiListen_Click);
            // 
            // lblIPAddressSection
            // 
            this.lblIPAddressSection.AutoSize = true;
            this.lblIPAddressSection.Location = new System.Drawing.Point(40, 334);
            this.lblIPAddressSection.Name = "lblIPAddressSection";
            this.lblIPAddressSection.Size = new System.Drawing.Size(159, 15);
            this.lblIPAddressSection.TabIndex = 20;
            this.lblIPAddressSection.Text = "IP Address Section:";
            // 
            // txtTargetServerIPAddress
            // 
            this.txtTargetServerIPAddress.Location = new System.Drawing.Point(215, 362);
            this.txtTargetServerIPAddress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTargetServerIPAddress.Name = "txtTargetServerIPAddress";
            this.txtTargetServerIPAddress.Size = new System.Drawing.Size(144, 25);
            this.txtTargetServerIPAddress.TabIndex = 7;
            this.txtTargetServerIPAddress.TextChanged += new System.EventHandler(this.txtTargetServerIPAddress_TextChanged);
            this.txtTargetServerIPAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetServerIPAddress_KeyPress);
            // 
            // txtTargetServerPort
            // 
            this.txtTargetServerPort.Location = new System.Drawing.Point(215, 404);
            this.txtTargetServerPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTargetServerPort.MaxLength = 5;
            this.txtTargetServerPort.Name = "txtTargetServerPort";
            this.txtTargetServerPort.Size = new System.Drawing.Size(144, 25);
            this.txtTargetServerPort.TabIndex = 8;
            this.txtTargetServerPort.TextChanged += new System.EventHandler(this.txtTargetServerPort_TextChanged);
            this.txtTargetServerPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTargetServerPort_KeyDown);
            this.txtTargetServerPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetServerPort_KeyPress);
            // 
            // ClientListView1
            // 
            this.ClientListView1.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.ClientListView1.AutoArrange = false;
            this.ClientListView1.CheckBoxes = true;
            this.ClientListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.ServerIPAddress,
            this.ServerPort,
            this.ClientTestedWithServer,
            this.ColumnHeadClientEnding});
            this.ClientListView1.ForeColor = System.Drawing.Color.Black;
            this.ClientListView1.FullRowSelect = true;
            this.ClientListView1.HideSelection = false;
            this.ClientListView1.Location = new System.Drawing.Point(16, 20);
            this.ClientListView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ClientListView1.MultiSelect = false;
            this.ClientListView1.Name = "ClientListView1";
            this.ClientListView1.Size = new System.Drawing.Size(507, 284);
            this.ClientListView1.TabIndex = 11;
            this.ClientListView1.UseCompatibleStateImageBehavior = false;
            this.ClientListView1.View = System.Windows.Forms.View.Details;
            this.ClientListView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ClientListView1_ItemSelectionChanged);
            this.ClientListView1.SelectedIndexChanged += new System.EventHandler(this.ClientListView1_SelectedIndexChanged);
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "";
            this.ColumnHeader1.Width = 30;
            // 
            // ServerIPAddress
            // 
            this.ServerIPAddress.Text = "IP Address";
            this.ServerIPAddress.Width = 130;
            // 
            // ServerPort
            // 
            this.ServerPort.Text = "Port";
            this.ServerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ClientTestedWithServer
            // 
            this.ClientTestedWithServer.Text = "Tested";
            this.ClientTestedWithServer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ColumnHeadClientEnding
            // 
            this.ColumnHeadClientEnding.Text = "Ending";
            this.ColumnHeadClientEnding.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblEndingForClientSendingMessage
            // 
            this.lblEndingForClientSendingMessage.AutoSize = true;
            this.lblEndingForClientSendingMessage.Location = new System.Drawing.Point(40, 459);
            this.lblEndingForClientSendingMessage.Name = "lblEndingForClientSendingMessage";
            this.lblEndingForClientSendingMessage.Size = new System.Drawing.Size(63, 15);
            this.lblEndingForClientSendingMessage.TabIndex = 14;
            this.lblEndingForClientSendingMessage.Text = "Ending:";
            // 
            // lblTargetServerIPAddress
            // 
            this.lblTargetServerIPAddress.AutoSize = true;
            this.lblTargetServerIPAddress.Location = new System.Drawing.Point(40, 374);
            this.lblTargetServerIPAddress.Name = "lblTargetServerIPAddress";
            this.lblTargetServerIPAddress.Size = new System.Drawing.Size(151, 15);
            this.lblTargetServerIPAddress.TabIndex = 12;
            this.lblTargetServerIPAddress.Text = "Server IP Address:";
            // 
            // lblTargetServerPort
            // 
            this.lblTargetServerPort.AutoSize = true;
            this.lblTargetServerPort.Location = new System.Drawing.Point(40, 415);
            this.lblTargetServerPort.Name = "lblTargetServerPort";
            this.lblTargetServerPort.Size = new System.Drawing.Size(103, 15);
            this.lblTargetServerPort.TabIndex = 13;
            this.lblTargetServerPort.Text = "Server Port:";
            // 
            // lblIPCount
            // 
            this.lblIPCount.AutoSize = true;
            this.lblIPCount.Location = new System.Drawing.Point(276, 42);
            this.lblIPCount.Name = "lblIPCount";
            this.lblIPCount.Size = new System.Drawing.Size(79, 15);
            this.lblIPCount.TabIndex = 150;
            this.lblIPCount.Text = "IP Count:";
            // 
            // rtbTCPIPHistory
            // 
            this.rtbTCPIPHistory.BackColor = System.Drawing.Color.Black;
            this.rtbTCPIPHistory.ForeColor = System.Drawing.Color.Lime;
            this.rtbTCPIPHistory.Location = new System.Drawing.Point(557, 66);
            this.rtbTCPIPHistory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtbTCPIPHistory.Name = "rtbTCPIPHistory";
            this.rtbTCPIPHistory.ReadOnly = true;
            this.rtbTCPIPHistory.Size = new System.Drawing.Size(745, 452);
            this.rtbTCPIPHistory.TabIndex = 149;
            this.rtbTCPIPHistory.Text = "";
            this.rtbTCPIPHistory.TextChanged += new System.EventHandler(this.rtbTCPIPHistory_TextChanged);
            this.rtbTCPIPHistory.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.rtbTCPIPHistory_MouseDoubleClick);
            this.rtbTCPIPHistory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rtbTCPIPHistory_MouseDown);
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NetworkStatus_Menu,
            this.SavePara_ToolStripMenuItem,
            this.RefreshToolStripMenuItem,
            this.SearchPCName_ToolStripMenuItem1,
            this.MACToolStripMenuItem,
            this.LanguageToolStripMenuItem,
            this.AboutToolStripMenuItem,
            this.Setting_ToolStripMenuItem});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.MenuStrip1.ShowItemToolTips = true;
            this.MenuStrip1.Size = new System.Drawing.Size(1312, 28);
            this.MenuStrip1.TabIndex = 146;
            // 
            // NetworkStatus_Menu
            // 
            this.NetworkStatus_Menu.Image = global::PengDongNanTools.Properties.Resources.NoNetwork;
            this.NetworkStatus_Menu.Name = "NetworkStatus_Menu";
            this.NetworkStatus_Menu.Size = new System.Drawing.Size(100, 24);
            this.NetworkStatus_Menu.Text = "Network";
            this.NetworkStatus_Menu.ToolTipText = "网络是否已经连接或者断开";
            // 
            // SavePara_ToolStripMenuItem
            // 
            this.SavePara_ToolStripMenuItem.AutoToolTip = true;
            this.SavePara_ToolStripMenuItem.Image = global::PengDongNanTools.Properties.Resources.DISK12;
            this.SavePara_ToolStripMenuItem.Name = "SavePara_ToolStripMenuItem";
            this.SavePara_ToolStripMenuItem.Size = new System.Drawing.Size(106, 24);
            this.SavePara_ToolStripMenuItem.Text = "Save Para";
            this.SavePara_ToolStripMenuItem.ToolTipText = "保存参数至内存";
            this.SavePara_ToolStripMenuItem.Click += new System.EventHandler(this.SavePara_ToolStripMenuItem_Click);
            // 
            // RefreshToolStripMenuItem
            // 
            this.RefreshToolStripMenuItem.AutoToolTip = true;
            this.RefreshToolStripMenuItem.Image = global::PengDongNanTools.Properties.Resources.sync;
            this.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem";
            this.RefreshToolStripMenuItem.Size = new System.Drawing.Size(109, 24);
            this.RefreshToolStripMenuItem.Text = "Refresh IP";
            this.RefreshToolStripMenuItem.ToolTipText = "刷新网络中可用电脑的IP地址";
            this.RefreshToolStripMenuItem.Click += new System.EventHandler(this.RefreshToolStripMenuItem_Click);
            // 
            // SearchPCName_ToolStripMenuItem1
            // 
            this.SearchPCName_ToolStripMenuItem1.AutoToolTip = true;
            this.SearchPCName_ToolStripMenuItem1.Image = global::PengDongNanTools.Properties.Resources.users;
            this.SearchPCName_ToolStripMenuItem1.Name = "SearchPCName_ToolStripMenuItem1";
            this.SearchPCName_ToolStripMenuItem1.Size = new System.Drawing.Size(103, 24);
            this.SearchPCName_ToolStripMenuItem1.Text = "PC Name";
            this.SearchPCName_ToolStripMenuItem1.ToolTipText = "查找网络中已连接的电脑名称";
            this.SearchPCName_ToolStripMenuItem1.Click += new System.EventHandler(this.SearchPCName_ToolStripMenuItem1_Click);
            // 
            // MACToolStripMenuItem
            // 
            this.MACToolStripMenuItem.AutoToolTip = true;
            this.MACToolStripMenuItem.Image = global::PengDongNanTools.Properties.Resources.Nethood;
            this.MACToolStripMenuItem.Name = "MACToolStripMenuItem";
            this.MACToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.MACToolStripMenuItem.Text = "MAC";
            this.MACToolStripMenuItem.ToolTipText = "查本电脑的物理地址";
            this.MACToolStripMenuItem.Click += new System.EventHandler(this.MACToolStripMenuItem_Click);
            // 
            // LanguageToolStripMenuItem
            // 
            this.LanguageToolStripMenuItem.AutoToolTip = true;
            this.LanguageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EnglishToolStripMenuItem,
            this.中文ToolStripMenuItem});
            this.LanguageToolStripMenuItem.Image = global::PengDongNanTools.Properties.Resources.CurLanguage;
            this.LanguageToolStripMenuItem.Name = "LanguageToolStripMenuItem";
            this.LanguageToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.LanguageToolStripMenuItem.Text = "Language";
            this.LanguageToolStripMenuItem.ToolTipText = "切换界面的语言";
            // 
            // EnglishToolStripMenuItem
            // 
            this.EnglishToolStripMenuItem.AutoToolTip = true;
            this.EnglishToolStripMenuItem.Image = global::PengDongNanTools.Properties.Resources.FLGUK;
            this.EnglishToolStripMenuItem.Name = "EnglishToolStripMenuItem";
            this.EnglishToolStripMenuItem.Size = new System.Drawing.Size(129, 24);
            this.EnglishToolStripMenuItem.Text = "English";
            this.EnglishToolStripMenuItem.ToolTipText = "Change to English";
            this.EnglishToolStripMenuItem.Click += new System.EventHandler(this.EnglishToolStripMenuItem_Click);
            // 
            // 中文ToolStripMenuItem
            // 
            this.中文ToolStripMenuItem.AutoToolTip = true;
            this.中文ToolStripMenuItem.Image = global::PengDongNanTools.Properties.Resources.ChineseFlag;
            this.中文ToolStripMenuItem.Name = "中文ToolStripMenuItem";
            this.中文ToolStripMenuItem.Size = new System.Drawing.Size(129, 24);
            this.中文ToolStripMenuItem.Text = "中文";
            this.中文ToolStripMenuItem.ToolTipText = "界面切换至中文";
            this.中文ToolStripMenuItem.Click += new System.EventHandler(this.中文ToolStripMenuItem_Click);
            // 
            // txtSendToServer
            // 
            this.txtSendToServer.BackColor = System.Drawing.Color.Black;
            this.txtSendToServer.Font = new System.Drawing.Font("宋体", 18F);
            this.txtSendToServer.ForeColor = System.Drawing.Color.Lime;
            this.txtSendToServer.Location = new System.Drawing.Point(557, 522);
            this.txtSendToServer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSendToServer.Multiline = true;
            this.txtSendToServer.Name = "txtSendToServer";
            this.txtSendToServer.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSendToServer.Size = new System.Drawing.Size(639, 43);
            this.txtSendToServer.TabIndex = 153;
            this.txtSendToServer.TextChanged += new System.EventHandler(this.txtSendToServer_TextChanged);
            this.txtSendToServer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSendToServer_KeyDown);
            // 
            // lblLocalIPAddress
            // 
            this.lblLocalIPAddress.AutoSize = true;
            this.lblLocalIPAddress.Location = new System.Drawing.Point(590, 42);
            this.lblLocalIPAddress.Name = "lblLocalIPAddress";
            this.lblLocalIPAddress.Size = new System.Drawing.Size(119, 15);
            this.lblLocalIPAddress.TabIndex = 161;
            this.lblLocalIPAddress.Text = "LocalIPAddress";
            // 
            // TCPAsyncClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(1312, 612);
            this.Controls.Add(this.lblLocalIPAddress);
            this.Controls.Add(this.chkClientSendHEX);
            this.Controls.Add(this.lblMS);
            this.Controls.Add(this.lblAutoSendInterval);
            this.Controls.Add(this.txtAutoSendInterval);
            this.Controls.Add(this.chkAutoSend);
            this.Controls.Add(this.chkSendFile);
            this.Controls.Add(this.lblIPNumber);
            this.Controls.Add(this.lblCurrentIndexForClientListView);
            this.Controls.Add(this.Client_GroupBox1);
            this.Controls.Add(this.lblIPCount);
            this.Controls.Add(this.rtbTCPIPHistory);
            this.Controls.Add(this.MenuStrip1);
            this.Controls.Add(this.btnClientSendToServer);
            this.Controls.Add(this.txtSendToServer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1330, 659);
            this.Name = "TCPAsyncClientForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TCP/IP Async Client Communication Software";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TCPClientForm_FormClosing);
            this.Load += new System.EventHandler(this.TCPClientForm_Load);
            this.Client_GroupBox1.ResumeLayout(false);
            this.Client_GroupBox1.PerformLayout();
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion

        internal System.Windows.Forms.ToolStripMenuItem MACToolStripMenuItem;
        internal System.Windows.Forms.Button btnConnectToServer;
        internal System.Windows.Forms.ToolTip ToolTip;
        internal System.Windows.Forms.ToolStripMenuItem LanguageToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem EnglishToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem 中文ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem SearchPCName_ToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem RefreshToolStripMenuItem;
        internal System.Windows.Forms.Button btnCloseClient;
        internal System.Windows.Forms.ToolStripMenuItem SavePara_ToolStripMenuItem;
        internal System.Windows.Forms.CheckBox chkClientSendHEX;
        internal System.Windows.Forms.ToolStripMenuItem NetworkStatus_Menu;
        internal System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        internal System.Windows.Forms.Label lblMS;
        internal System.Windows.Forms.Button btnReviseClientRecord;
        internal System.Windows.Forms.Button btnSearchIP;
        internal System.Windows.Forms.Button btnDelClientRecordInListView;
        internal System.Windows.Forms.Button btnAddPortForClient;
        internal System.Windows.Forms.Button btnClientSendToServer;
        internal System.Windows.Forms.ToolStripMenuItem ShutDownServer_ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShutDownClient_ToolStripMenuItem;
        internal System.Windows.Forms.Label lblAutoSendInterval;
        internal System.Windows.Forms.TextBox txtAutoSendInterval;
        internal System.Windows.Forms.ToolStripMenuItem Setting_ToolStripMenuItem;
        internal System.Windows.Forms.CheckBox chkAutoSend;
        internal System.Windows.Forms.ComboBox cmbSuffixForClient;
        internal System.Windows.Forms.CheckBox chkSendFile;
        internal System.Windows.Forms.TextBox txtCustomizedEndingCodeForClient;
        internal System.Windows.Forms.MaskedTextBox mtxt_2;
        internal System.Windows.Forms.MaskedTextBox mtxt_3;
        internal System.Windows.Forms.MaskedTextBox mtxt_4;
        internal System.Windows.Forms.Label lblIPNumber;
        internal System.Windows.Forms.Label lblCurrentIndexForClientListView;
        internal System.Windows.Forms.GroupBox Client_GroupBox1;
        internal System.Windows.Forms.Label lblIPAddressSection;
        internal System.Windows.Forms.TextBox txtTargetServerIPAddress;
        internal System.Windows.Forms.TextBox txtTargetServerPort;
        internal System.Windows.Forms.ListView ClientListView1;
        internal System.Windows.Forms.ColumnHeader ColumnHeader1;
        internal System.Windows.Forms.ColumnHeader ServerIPAddress;
        internal System.Windows.Forms.ColumnHeader ServerPort;
        internal System.Windows.Forms.ColumnHeader ClientTestedWithServer;
        internal System.Windows.Forms.ColumnHeader ColumnHeadClientEnding;
        internal System.Windows.Forms.Label lblEndingForClientSendingMessage;
        internal System.Windows.Forms.Label lblTargetServerIPAddress;
        internal System.Windows.Forms.Label lblTargetServerPort;
        internal System.Windows.Forms.Label lblIPCount;
        internal System.Windows.Forms.RichTextBox rtbTCPIPHistory;
        internal System.Windows.Forms.MenuStrip MenuStrip1;
        internal System.Windows.Forms.TextBox txtSendToServer;
        internal System.Windows.Forms.Button btnMultiConnect;
        internal System.Windows.Forms.Label lblLocalIPAddress;
        }
    }