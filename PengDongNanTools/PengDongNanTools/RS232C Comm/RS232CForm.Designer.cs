namespace PengDongNanTools
    {
    partial class RS232CForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RS232CForm));
            this.chkReadLineOrByte = new System.Windows.Forms.CheckBox();
            this.chkSendHEX = new System.Windows.Forms.CheckBox();
            this.chkSendFile = new System.Windows.Forms.CheckBox();
            this.lblEndingForSendingMessage = new System.Windows.Forms.Label();
            this.chkAutoSend = new System.Windows.Forms.CheckBox();
            this.txtForCustomizedEndingCode = new System.Windows.Forms.TextBox();
            this.lblCOMPort = new System.Windows.Forms.Label();
            this.txtAutoSendInterval = new System.Windows.Forms.TextBox();
            this.btnSaveRS232CPara = new System.Windows.Forms.Button();
            this.ComboBoxCOMPort = new System.Windows.Forms.ComboBox();
            this.ComboBoxSuffix = new System.Windows.Forms.ComboBox();
            this.picChinese = new System.Windows.Forms.PictureBox();
            this.ComboBoxBaudRate = new System.Windows.Forms.ComboBox();
            this.picEnglish = new System.Windows.Forms.PictureBox();
            this.CheckboxRTSEnable = new System.Windows.Forms.CheckBox();
            this.lblBaudrate = new System.Windows.Forms.Label();
            this.lblAutoSendInterval = new System.Windows.Forms.Label();
            this.btnOpenPort = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.CheckboxDTREnable = new System.Windows.Forms.CheckBox();
            this.ComboBoxDataBits = new System.Windows.Forms.ComboBox();
            this.lblDataBits = new System.Windows.Forms.Label();
            this.picHelp = new System.Windows.Forms.PictureBox();
            this.lblHandshake = new System.Windows.Forms.Label();
            this.ComboBoxParity = new System.Windows.Forms.ComboBox();
            this.ComboBoxHandShake = new System.Windows.Forms.ComboBox();
            this.lblParity = new System.Windows.Forms.Label();
            this.picFresh = new System.Windows.Forms.PictureBox();
            this.lblStopBit = new System.Windows.Forms.Label();
            this.ComboBoxStopBit = new System.Windows.Forms.ComboBox();
            this.grpRS232CSetting = new System.Windows.Forms.GroupBox();
            this.chkGB2312 = new System.Windows.Forms.CheckBox();
            this.lblCommunicationHistory = new System.Windows.Forms.Label();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.richtxtSend = new System.Windows.Forms.RichTextBox();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.rtbCurrentReceived = new System.Windows.Forms.RichTextBox();
            this.richtxtHistory = new System.Windows.Forms.RichTextBox();
            this.lblTargetComPort = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picChinese)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEnglish)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFresh)).BeginInit();
            this.grpRS232CSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkReadLineOrByte
            // 
            this.chkReadLineOrByte.AutoSize = true;
            this.chkReadLineOrByte.Location = new System.Drawing.Point(227, 135);
            this.chkReadLineOrByte.Margin = new System.Windows.Forms.Padding(4);
            this.chkReadLineOrByte.Name = "chkReadLineOrByte";
            this.chkReadLineOrByte.Size = new System.Drawing.Size(93, 19);
            this.chkReadLineOrByte.TabIndex = 17;
            this.chkReadLineOrByte.Text = "ReadByte";
            this.ToolTip1.SetToolTip(this.chkReadLineOrByte, "对应串口端口是否读取16进制");
            this.chkReadLineOrByte.UseVisualStyleBackColor = true;
            this.chkReadLineOrByte.CheckedChanged += new System.EventHandler(this.chkReadLineOrByte_CheckedChanged);
            // 
            // chkSendHEX
            // 
            this.chkSendHEX.AutoSize = true;
            this.chkSendHEX.Location = new System.Drawing.Point(1073, 578);
            this.chkSendHEX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkSendHEX.Name = "chkSendHEX";
            this.chkSendHEX.Size = new System.Drawing.Size(53, 19);
            this.chkSendHEX.TabIndex = 151;
            this.chkSendHEX.Text = "HEX";
            this.ToolTip1.SetToolTip(this.chkSendHEX, "是否发送16进制字符");
            this.chkSendHEX.UseVisualStyleBackColor = true;
            this.chkSendHEX.CheckedChanged += new System.EventHandler(this.chkSendHEX_CheckedChanged);
            // 
            // chkSendFile
            // 
            this.chkSendFile.AutoSize = true;
            this.chkSendFile.Location = new System.Drawing.Point(953, 578);
            this.chkSendFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkSendFile.Name = "chkSendFile";
            this.chkSendFile.Size = new System.Drawing.Size(101, 19);
            this.chkSendFile.TabIndex = 153;
            this.chkSendFile.Text = "Send File";
            this.ToolTip1.SetToolTip(this.chkSendFile, "是否发送文件");
            this.chkSendFile.UseVisualStyleBackColor = true;
            this.chkSendFile.CheckedChanged += new System.EventHandler(this.chkSendFile_CheckedChanged);
            // 
            // lblEndingForSendingMessage
            // 
            this.lblEndingForSendingMessage.AutoSize = true;
            this.lblEndingForSendingMessage.Location = new System.Drawing.Point(19, 346);
            this.lblEndingForSendingMessage.Name = "lblEndingForSendingMessage";
            this.lblEndingForSendingMessage.Size = new System.Drawing.Size(63, 15);
            this.lblEndingForSendingMessage.TabIndex = 8;
            this.lblEndingForSendingMessage.Text = "Ending:";
            // 
            // chkAutoSend
            // 
            this.chkAutoSend.AutoSize = true;
            this.chkAutoSend.Location = new System.Drawing.Point(819, 578);
            this.chkAutoSend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkAutoSend.Name = "chkAutoSend";
            this.chkAutoSend.Size = new System.Drawing.Size(101, 19);
            this.chkAutoSend.TabIndex = 154;
            this.chkAutoSend.Text = "Auto Send";
            this.ToolTip1.SetToolTip(this.chkAutoSend, "是否自动发送");
            this.chkAutoSend.UseVisualStyleBackColor = true;
            this.chkAutoSend.CheckedChanged += new System.EventHandler(this.chkAutoSend_CheckedChanged);
            // 
            // txtForCustomizedEndingCode
            // 
            this.txtForCustomizedEndingCode.Location = new System.Drawing.Point(225, 339);
            this.txtForCustomizedEndingCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtForCustomizedEndingCode.MaxLength = 10;
            this.txtForCustomizedEndingCode.Name = "txtForCustomizedEndingCode";
            this.txtForCustomizedEndingCode.Size = new System.Drawing.Size(76, 25);
            this.txtForCustomizedEndingCode.TabIndex = 10;
            this.ToolTip1.SetToolTip(this.txtForCustomizedEndingCode, "对应串口端口的自定义文本");
            this.txtForCustomizedEndingCode.Visible = false;
            this.txtForCustomizedEndingCode.TextChanged += new System.EventHandler(this.txtForCustomizedEndingCode_TextChanged);
            // 
            // lblCOMPort
            // 
            this.lblCOMPort.AutoSize = true;
            this.lblCOMPort.Location = new System.Drawing.Point(19, 38);
            this.lblCOMPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCOMPort.Name = "lblCOMPort";
            this.lblCOMPort.Size = new System.Drawing.Size(79, 15);
            this.lblCOMPort.TabIndex = 11;
            this.lblCOMPort.Text = "COM port:";
            // 
            // txtAutoSendInterval
            // 
            this.txtAutoSendInterval.Location = new System.Drawing.Point(702, 572);
            this.txtAutoSendInterval.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAutoSendInterval.MaxLength = 10;
            this.txtAutoSendInterval.Name = "txtAutoSendInterval";
            this.txtAutoSendInterval.Size = new System.Drawing.Size(53, 25);
            this.txtAutoSendInterval.TabIndex = 143;
            this.ToolTip1.SetToolTip(this.txtAutoSendInterval, "自动发送的时间间隔");
            this.txtAutoSendInterval.TextChanged += new System.EventHandler(this.txtAutoSendInterval_TextChanged);
            this.txtAutoSendInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAutoSendInterval_KeyPress);
            // 
            // btnSaveRS232CPara
            // 
            this.btnSaveRS232CPara.Location = new System.Drawing.Point(199, 424);
            this.btnSaveRS232CPara.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSaveRS232CPara.Name = "btnSaveRS232CPara";
            this.btnSaveRS232CPara.Size = new System.Drawing.Size(100, 29);
            this.btnSaveRS232CPara.TabIndex = 126;
            this.btnSaveRS232CPara.Text = "Save Para";
            this.ToolTip1.SetToolTip(this.btnSaveRS232CPara, "保存参数");
            this.btnSaveRS232CPara.UseVisualStyleBackColor = true;
            this.btnSaveRS232CPara.Click += new System.EventHandler(this.btnSaveRS232CPara_Click);
            // 
            // ComboBoxCOMPort
            // 
            this.ComboBoxCOMPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxCOMPort.FormattingEnabled = true;
            this.ComboBoxCOMPort.Location = new System.Drawing.Point(117, 30);
            this.ComboBoxCOMPort.Margin = new System.Windows.Forms.Padding(4);
            this.ComboBoxCOMPort.MaxDropDownItems = 12;
            this.ComboBoxCOMPort.Name = "ComboBoxCOMPort";
            this.ComboBoxCOMPort.Size = new System.Drawing.Size(75, 23);
            this.ComboBoxCOMPort.TabIndex = 0;
            this.ToolTip1.SetToolTip(this.ComboBoxCOMPort, "需要操作的串口名称");
            this.ComboBoxCOMPort.SelectedIndexChanged += new System.EventHandler(this.ComboBoxCOMPort_SelectedIndexChanged);
            // 
            // ComboBoxSuffix
            // 
            this.ComboBoxSuffix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxSuffix.FormattingEnabled = true;
            this.ComboBoxSuffix.Items.AddRange(new object[] {
            "None",
            "LF",
            "CR",
            "CR+LF",
            "自定义"});
            this.ComboBoxSuffix.Location = new System.Drawing.Point(117, 340);
            this.ComboBoxSuffix.Margin = new System.Windows.Forms.Padding(4);
            this.ComboBoxSuffix.Name = "ComboBoxSuffix";
            this.ComboBoxSuffix.Size = new System.Drawing.Size(76, 23);
            this.ComboBoxSuffix.TabIndex = 9;
            this.ToolTip1.SetToolTip(this.ComboBoxSuffix, "对应串口端口的结束符");
            this.ComboBoxSuffix.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSuffix_SelectedIndexChanged);
            // 
            // picChinese
            // 
            this.picChinese.Image = global::PengDongNanTools.Properties.Resources.China_Flag_2;
            this.picChinese.Location = new System.Drawing.Point(83, 547);
            this.picChinese.Margin = new System.Windows.Forms.Padding(4);
            this.picChinese.Name = "picChinese";
            this.picChinese.Size = new System.Drawing.Size(44, 41);
            this.picChinese.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picChinese.TabIndex = 148;
            this.picChinese.TabStop = false;
            this.ToolTip1.SetToolTip(this.picChinese, "界面语言切换为中文");
            this.picChinese.Click += new System.EventHandler(this.picChinese_Click);
            // 
            // ComboBoxBaudRate
            // 
            this.ComboBoxBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxBaudRate.FormattingEnabled = true;
            this.ComboBoxBaudRate.Items.AddRange(new object[] {
            "75",
            "110",
            "134",
            "150",
            "300",
            "600",
            "1200",
            "1800",
            "2400",
            "4800",
            "7200",
            "9600",
            "14400",
            "19200",
            "38400",
            "57600",
            "115200",
            "128000"});
            this.ComboBoxBaudRate.Location = new System.Drawing.Point(117, 82);
            this.ComboBoxBaudRate.Margin = new System.Windows.Forms.Padding(4);
            this.ComboBoxBaudRate.MaxDropDownItems = 12;
            this.ComboBoxBaudRate.Name = "ComboBoxBaudRate";
            this.ComboBoxBaudRate.Size = new System.Drawing.Size(75, 23);
            this.ComboBoxBaudRate.TabIndex = 1;
            this.ComboBoxBaudRate.TabStop = false;
            this.ToolTip1.SetToolTip(this.ComboBoxBaudRate, "对应串口端口的波段率");
            this.ComboBoxBaudRate.SelectedIndexChanged += new System.EventHandler(this.ComboBoxBaudRate_SelectedIndexChanged);
            // 
            // picEnglish
            // 
            this.picEnglish.Image = global::PengDongNanTools.Properties.Resources.FLGUK;
            this.picEnglish.Location = new System.Drawing.Point(174, 547);
            this.picEnglish.Margin = new System.Windows.Forms.Padding(4);
            this.picEnglish.Name = "picEnglish";
            this.picEnglish.Size = new System.Drawing.Size(44, 41);
            this.picEnglish.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picEnglish.TabIndex = 149;
            this.picEnglish.TabStop = false;
            this.ToolTip1.SetToolTip(this.picEnglish, "Show the interface in English");
            this.picEnglish.Click += new System.EventHandler(this.picEnglish_Click);
            // 
            // CheckboxRTSEnable
            // 
            this.CheckboxRTSEnable.AutoSize = true;
            this.CheckboxRTSEnable.Location = new System.Drawing.Point(227, 35);
            this.CheckboxRTSEnable.Margin = new System.Windows.Forms.Padding(4);
            this.CheckboxRTSEnable.Name = "CheckboxRTSEnable";
            this.CheckboxRTSEnable.Size = new System.Drawing.Size(109, 19);
            this.CheckboxRTSEnable.TabIndex = 6;
            this.CheckboxRTSEnable.Text = "RTS Enable";
            this.ToolTip1.SetToolTip(this.CheckboxRTSEnable, "对应串口端口的请求发送信号有效");
            this.CheckboxRTSEnable.UseVisualStyleBackColor = true;
            this.CheckboxRTSEnable.CheckedChanged += new System.EventHandler(this.CheckboxRTSEnable_CheckedChanged);
            // 
            // lblBaudrate
            // 
            this.lblBaudrate.AutoSize = true;
            this.lblBaudrate.Location = new System.Drawing.Point(19, 90);
            this.lblBaudrate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBaudrate.Name = "lblBaudrate";
            this.lblBaudrate.Size = new System.Drawing.Size(87, 15);
            this.lblBaudrate.TabIndex = 12;
            this.lblBaudrate.Text = "Baud Rate:";
            // 
            // lblAutoSendInterval
            // 
            this.lblAutoSendInterval.AutoSize = true;
            this.lblAutoSendInterval.Location = new System.Drawing.Point(537, 578);
            this.lblAutoSendInterval.Name = "lblAutoSendInterval";
            this.lblAutoSendInterval.Size = new System.Drawing.Size(159, 15);
            this.lblAutoSendInterval.TabIndex = 142;
            this.lblAutoSendInterval.Text = "Auto Send Interval:";
            // 
            // btnOpenPort
            // 
            this.btnOpenPort.Location = new System.Drawing.Point(52, 424);
            this.btnOpenPort.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenPort.Name = "btnOpenPort";
            this.btnOpenPort.Size = new System.Drawing.Size(100, 29);
            this.btnOpenPort.TabIndex = 125;
            this.btnOpenPort.Text = "Open Port";
            this.ToolTip1.SetToolTip(this.btnOpenPort, "打开/关闭端口");
            this.btnOpenPort.UseVisualStyleBackColor = true;
            this.btnOpenPort.Click += new System.EventHandler(this.btnOpenPort_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(761, 578);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(23, 15);
            this.Label1.TabIndex = 155;
            this.Label1.Text = "ms";
            // 
            // CheckboxDTREnable
            // 
            this.CheckboxDTREnable.AutoSize = true;
            this.CheckboxDTREnable.Location = new System.Drawing.Point(227, 88);
            this.CheckboxDTREnable.Margin = new System.Windows.Forms.Padding(4);
            this.CheckboxDTREnable.Name = "CheckboxDTREnable";
            this.CheckboxDTREnable.Size = new System.Drawing.Size(109, 19);
            this.CheckboxDTREnable.TabIndex = 7;
            this.CheckboxDTREnable.Text = "DTR Enable";
            this.ToolTip1.SetToolTip(this.CheckboxDTREnable, "对应串口端口的数据接收请求有效");
            this.CheckboxDTREnable.UseVisualStyleBackColor = true;
            this.CheckboxDTREnable.CheckedChanged += new System.EventHandler(this.CheckboxDTREnable_CheckedChanged);
            // 
            // ComboBoxDataBits
            // 
            this.ComboBoxDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxDataBits.FormattingEnabled = true;
            this.ComboBoxDataBits.Items.AddRange(new object[] {
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.ComboBoxDataBits.Location = new System.Drawing.Point(117, 135);
            this.ComboBoxDataBits.Margin = new System.Windows.Forms.Padding(4);
            this.ComboBoxDataBits.MaxDropDownItems = 12;
            this.ComboBoxDataBits.Name = "ComboBoxDataBits";
            this.ComboBoxDataBits.Size = new System.Drawing.Size(75, 23);
            this.ComboBoxDataBits.TabIndex = 2;
            this.ComboBoxDataBits.TabStop = false;
            this.ToolTip1.SetToolTip(this.ComboBoxDataBits, "对应串口端口的数据位");
            this.ComboBoxDataBits.SelectedIndexChanged += new System.EventHandler(this.ComboBoxDataBits_SelectedIndexChanged);
            // 
            // lblDataBits
            // 
            this.lblDataBits.AutoSize = true;
            this.lblDataBits.Location = new System.Drawing.Point(19, 141);
            this.lblDataBits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDataBits.Name = "lblDataBits";
            this.lblDataBits.Size = new System.Drawing.Size(87, 15);
            this.lblDataBits.TabIndex = 13;
            this.lblDataBits.Text = "Data Bits:";
            // 
            // picHelp
            // 
            this.picHelp.Image = global::PengDongNanTools.Properties.Resources.Help;
            this.picHelp.Location = new System.Drawing.Point(265, 547);
            this.picHelp.Margin = new System.Windows.Forms.Padding(4);
            this.picHelp.Name = "picHelp";
            this.picHelp.Size = new System.Drawing.Size(44, 41);
            this.picHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHelp.TabIndex = 150;
            this.picHelp.TabStop = false;
            this.ToolTip1.SetToolTip(this.picHelp, "帮助信息");
            this.picHelp.Click += new System.EventHandler(this.picHelp_Click);
            // 
            // lblHandshake
            // 
            this.lblHandshake.AutoSize = true;
            this.lblHandshake.Location = new System.Drawing.Point(19, 295);
            this.lblHandshake.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHandshake.Name = "lblHandshake";
            this.lblHandshake.Size = new System.Drawing.Size(87, 15);
            this.lblHandshake.TabIndex = 16;
            this.lblHandshake.Text = "Handshake:";
            // 
            // ComboBoxParity
            // 
            this.ComboBoxParity.BackColor = System.Drawing.SystemColors.Window;
            this.ComboBoxParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxParity.FormattingEnabled = true;
            this.ComboBoxParity.Items.AddRange(new object[] {
            "Even",
            "Mark",
            "None",
            "Odd",
            "Space"});
            this.ComboBoxParity.Location = new System.Drawing.Point(117, 189);
            this.ComboBoxParity.Margin = new System.Windows.Forms.Padding(4);
            this.ComboBoxParity.MaxDropDownItems = 12;
            this.ComboBoxParity.Name = "ComboBoxParity";
            this.ComboBoxParity.Size = new System.Drawing.Size(75, 23);
            this.ComboBoxParity.TabIndex = 3;
            this.ComboBoxParity.TabStop = false;
            this.ToolTip1.SetToolTip(this.ComboBoxParity, "对应串口端口的奇偶校验");
            this.ComboBoxParity.SelectedIndexChanged += new System.EventHandler(this.ComboBoxParity_SelectedIndexChanged);
            // 
            // ComboBoxHandShake
            // 
            this.ComboBoxHandShake.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxHandShake.FormattingEnabled = true;
            this.ComboBoxHandShake.Items.AddRange(new object[] {
            "None",
            "RequestToSend",
            "RequestToSend XOnXOff",
            "X-On X-Off"});
            this.ComboBoxHandShake.Location = new System.Drawing.Point(117, 289);
            this.ComboBoxHandShake.Margin = new System.Windows.Forms.Padding(4);
            this.ComboBoxHandShake.MaxDropDownItems = 12;
            this.ComboBoxHandShake.Name = "ComboBoxHandShake";
            this.ComboBoxHandShake.Size = new System.Drawing.Size(176, 23);
            this.ComboBoxHandShake.TabIndex = 5;
            this.ToolTip1.SetToolTip(this.ComboBoxHandShake, "对应串口端口的握手协议");
            this.ComboBoxHandShake.SelectedIndexChanged += new System.EventHandler(this.ComboBoxHandShake_SelectedIndexChanged);
            // 
            // lblParity
            // 
            this.lblParity.AutoSize = true;
            this.lblParity.Location = new System.Drawing.Point(19, 195);
            this.lblParity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblParity.Name = "lblParity";
            this.lblParity.Size = new System.Drawing.Size(63, 15);
            this.lblParity.TabIndex = 14;
            this.lblParity.Text = "Parity:";
            // 
            // picFresh
            // 
            this.picFresh.Image = global::PengDongNanTools.Properties.Resources.sync;
            this.picFresh.Location = new System.Drawing.Point(23, 547);
            this.picFresh.Margin = new System.Windows.Forms.Padding(4);
            this.picFresh.Name = "picFresh";
            this.picFresh.Size = new System.Drawing.Size(44, 41);
            this.picFresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFresh.TabIndex = 152;
            this.picFresh.TabStop = false;
            this.picFresh.Visible = false;
            this.picFresh.Click += new System.EventHandler(this.picFresh_Click);
            // 
            // lblStopBit
            // 
            this.lblStopBit.AutoSize = true;
            this.lblStopBit.Location = new System.Drawing.Point(19, 249);
            this.lblStopBit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStopBit.Name = "lblStopBit";
            this.lblStopBit.Size = new System.Drawing.Size(79, 15);
            this.lblStopBit.TabIndex = 15;
            this.lblStopBit.Text = "Stop Bit:";
            // 
            // ComboBoxStopBit
            // 
            this.ComboBoxStopBit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxStopBit.FormattingEnabled = true;
            this.ComboBoxStopBit.Items.AddRange(new object[] {
            "1",
            "2",
            "1.5",
            "None"});
            this.ComboBoxStopBit.Location = new System.Drawing.Point(117, 242);
            this.ComboBoxStopBit.Margin = new System.Windows.Forms.Padding(4);
            this.ComboBoxStopBit.MaxDropDownItems = 12;
            this.ComboBoxStopBit.Name = "ComboBoxStopBit";
            this.ComboBoxStopBit.Size = new System.Drawing.Size(75, 23);
            this.ComboBoxStopBit.TabIndex = 4;
            this.ComboBoxStopBit.TabStop = false;
            this.ToolTip1.SetToolTip(this.ComboBoxStopBit, "对应串口端口的停止位");
            this.ComboBoxStopBit.SelectedIndexChanged += new System.EventHandler(this.ComboBoxStopBit_SelectedIndexChanged);
            // 
            // grpRS232CSetting
            // 
            this.grpRS232CSetting.Controls.Add(this.chkGB2312);
            this.grpRS232CSetting.Controls.Add(this.chkReadLineOrByte);
            this.grpRS232CSetting.Controls.Add(this.lblEndingForSendingMessage);
            this.grpRS232CSetting.Controls.Add(this.txtForCustomizedEndingCode);
            this.grpRS232CSetting.Controls.Add(this.lblCOMPort);
            this.grpRS232CSetting.Controls.Add(this.ComboBoxSuffix);
            this.grpRS232CSetting.Controls.Add(this.btnSaveRS232CPara);
            this.grpRS232CSetting.Controls.Add(this.ComboBoxCOMPort);
            this.grpRS232CSetting.Controls.Add(this.ComboBoxBaudRate);
            this.grpRS232CSetting.Controls.Add(this.CheckboxRTSEnable);
            this.grpRS232CSetting.Controls.Add(this.lblBaudrate);
            this.grpRS232CSetting.Controls.Add(this.btnOpenPort);
            this.grpRS232CSetting.Controls.Add(this.CheckboxDTREnable);
            this.grpRS232CSetting.Controls.Add(this.ComboBoxDataBits);
            this.grpRS232CSetting.Controls.Add(this.lblDataBits);
            this.grpRS232CSetting.Controls.Add(this.lblHandshake);
            this.grpRS232CSetting.Controls.Add(this.ComboBoxParity);
            this.grpRS232CSetting.Controls.Add(this.ComboBoxHandShake);
            this.grpRS232CSetting.Controls.Add(this.lblParity);
            this.grpRS232CSetting.Controls.Add(this.lblStopBit);
            this.grpRS232CSetting.Controls.Add(this.ComboBoxStopBit);
            this.grpRS232CSetting.Location = new System.Drawing.Point(6, 24);
            this.grpRS232CSetting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpRS232CSetting.Name = "grpRS232CSetting";
            this.grpRS232CSetting.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpRS232CSetting.Size = new System.Drawing.Size(373, 472);
            this.grpRS232CSetting.TabIndex = 140;
            this.grpRS232CSetting.TabStop = false;
            this.grpRS232CSetting.Text = "Setting";
            // 
            // chkGB2312
            // 
            this.chkGB2312.AutoSize = true;
            this.chkGB2312.Location = new System.Drawing.Point(225, 189);
            this.chkGB2312.Margin = new System.Windows.Forms.Padding(4);
            this.chkGB2312.Name = "chkGB2312";
            this.chkGB2312.Size = new System.Drawing.Size(77, 19);
            this.chkGB2312.TabIndex = 127;
            this.chkGB2312.Text = "GB2312";
            this.ToolTip1.SetToolTip(this.chkGB2312, "对应串口端口是否读取16进制");
            this.chkGB2312.UseVisualStyleBackColor = true;
            this.chkGB2312.CheckedChanged += new System.EventHandler(this.chkGB2312_CheckedChanged);
            // 
            // lblCommunicationHistory
            // 
            this.lblCommunicationHistory.AutoSize = true;
            this.lblCommunicationHistory.Location = new System.Drawing.Point(382, 6);
            this.lblCommunicationHistory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCommunicationHistory.Name = "lblCommunicationHistory";
            this.lblCommunicationHistory.Size = new System.Drawing.Size(255, 15);
            this.lblCommunicationHistory.TabIndex = 146;
            this.lblCommunicationHistory.Text = "Communication history  of port:";
            // 
            // richtxtSend
            // 
            this.richtxtSend.BackColor = System.Drawing.Color.Black;
            this.richtxtSend.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richtxtSend.ForeColor = System.Drawing.Color.Lime;
            this.richtxtSend.Location = new System.Drawing.Point(385, 506);
            this.richtxtSend.Margin = new System.Windows.Forms.Padding(4);
            this.richtxtSend.Name = "richtxtSend";
            this.richtxtSend.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.richtxtSend.Size = new System.Drawing.Size(657, 52);
            this.richtxtSend.TabIndex = 144;
            this.richtxtSend.Text = "";
            this.ToolTip1.SetToolTip(this.richtxtSend, "在此输入需要发送的文本内容");
            this.richtxtSend.TextChanged += new System.EventHandler(this.richtxtSend_TextChanged);
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Font = new System.Drawing.Font("宋体", 10F);
            this.btnSendMessage.Location = new System.Drawing.Point(1058, 511);
            this.btnSendMessage.Margin = new System.Windows.Forms.Padding(4);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(84, 38);
            this.btnSendMessage.TabIndex = 141;
            this.btnSendMessage.Text = "Send";
            this.ToolTip1.SetToolTip(this.btnSendMessage, "发送左侧文本框中的内容到指定串口端口");
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
            // 
            // rtbCurrentReceived
            // 
            this.rtbCurrentReceived.BackColor = System.Drawing.Color.Black;
            this.rtbCurrentReceived.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbCurrentReceived.ForeColor = System.Drawing.Color.Lime;
            this.rtbCurrentReceived.Location = new System.Drawing.Point(384, 423);
            this.rtbCurrentReceived.Margin = new System.Windows.Forms.Padding(4);
            this.rtbCurrentReceived.Name = "rtbCurrentReceived";
            this.rtbCurrentReceived.ReadOnly = true;
            this.rtbCurrentReceived.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtbCurrentReceived.Size = new System.Drawing.Size(757, 73);
            this.rtbCurrentReceived.TabIndex = 157;
            this.rtbCurrentReceived.Text = "";
            this.ToolTip1.SetToolTip(this.rtbCurrentReceived, "当前接收内容");
            // 
            // richtxtHistory
            // 
            this.richtxtHistory.BackColor = System.Drawing.Color.Black;
            this.richtxtHistory.ForeColor = System.Drawing.Color.Lime;
            this.richtxtHistory.Location = new System.Drawing.Point(385, 24);
            this.richtxtHistory.Margin = new System.Windows.Forms.Padding(4);
            this.richtxtHistory.Name = "richtxtHistory";
            this.richtxtHistory.ReadOnly = true;
            this.richtxtHistory.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.richtxtHistory.Size = new System.Drawing.Size(756, 391);
            this.richtxtHistory.TabIndex = 156;
            this.richtxtHistory.Text = "";
            this.ToolTip1.SetToolTip(this.richtxtHistory, "串口通讯的历史记录");
            this.richtxtHistory.TextChanged += new System.EventHandler(this.richtxtHistory_TextChanged_1);
            this.richtxtHistory.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.richtxtHistory_MouseDoubleClick_1);
            this.richtxtHistory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.richtxtHistory_MouseDown_1);
            // 
            // lblTargetComPort
            // 
            this.lblTargetComPort.AutoSize = true;
            this.lblTargetComPort.Location = new System.Drawing.Point(655, 6);
            this.lblTargetComPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTargetComPort.Name = "lblTargetComPort";
            this.lblTargetComPort.Size = new System.Drawing.Size(31, 15);
            this.lblTargetComPort.TabIndex = 147;
            this.lblTargetComPort.Text = "COM";
            // 
            // RS232CForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(1161, 615);
            this.Controls.Add(this.rtbCurrentReceived);
            this.Controls.Add(this.richtxtHistory);
            this.Controls.Add(this.chkSendHEX);
            this.Controls.Add(this.chkSendFile);
            this.Controls.Add(this.chkAutoSend);
            this.Controls.Add(this.txtAutoSendInterval);
            this.Controls.Add(this.picChinese);
            this.Controls.Add(this.picEnglish);
            this.Controls.Add(this.lblAutoSendInterval);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.picHelp);
            this.Controls.Add(this.picFresh);
            this.Controls.Add(this.grpRS232CSetting);
            this.Controls.Add(this.lblCommunicationHistory);
            this.Controls.Add(this.lblTargetComPort);
            this.Controls.Add(this.richtxtSend);
            this.Controls.Add(this.btnSendMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1167, 650);
            this.Name = "RS232CForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RS232CForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RS232CForm_FormClosing);
            this.Load += new System.EventHandler(this.RS232CForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picChinese)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEnglish)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFresh)).EndInit();
            this.grpRS232CSetting.ResumeLayout(false);
            this.grpRS232CSetting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion

        internal System.Windows.Forms.CheckBox chkReadLineOrByte;
        internal System.Windows.Forms.ToolTip ToolTip1;
        internal System.Windows.Forms.CheckBox chkSendHEX;
        internal System.Windows.Forms.CheckBox chkSendFile;
        internal System.Windows.Forms.Label lblEndingForSendingMessage;
        internal System.Windows.Forms.CheckBox chkAutoSend;
        internal System.Windows.Forms.TextBox txtForCustomizedEndingCode;
        internal System.Windows.Forms.Label lblCOMPort;
        internal System.Windows.Forms.TextBox txtAutoSendInterval;
        internal System.Windows.Forms.Button btnSaveRS232CPara;
        internal System.Windows.Forms.ComboBox ComboBoxCOMPort;
        internal System.Windows.Forms.ComboBox ComboBoxSuffix;
        internal System.Windows.Forms.PictureBox picChinese;
        internal System.Windows.Forms.ComboBox ComboBoxBaudRate;
        internal System.Windows.Forms.PictureBox picEnglish;
        internal System.Windows.Forms.CheckBox CheckboxRTSEnable;
        internal System.Windows.Forms.Label lblBaudrate;
        internal System.Windows.Forms.Label lblAutoSendInterval;
        internal System.Windows.Forms.Button btnOpenPort;
        //internal System.Windows.Forms.Timer tmrAutoSend;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.CheckBox CheckboxDTREnable;
        internal System.Windows.Forms.ComboBox ComboBoxDataBits;
        internal System.Windows.Forms.Label lblDataBits;
        internal System.Windows.Forms.PictureBox picHelp;
        internal System.Windows.Forms.Label lblHandshake;
        internal System.Windows.Forms.ComboBox ComboBoxParity;
        internal System.Windows.Forms.ComboBox ComboBoxHandShake;
        internal System.Windows.Forms.Label lblParity;
        internal System.Windows.Forms.PictureBox picFresh;
        internal System.Windows.Forms.Label lblStopBit;
        internal System.Windows.Forms.ComboBox ComboBoxStopBit;
        internal System.Windows.Forms.GroupBox grpRS232CSetting;
        internal System.Windows.Forms.Label lblCommunicationHistory;
        internal System.Windows.Forms.RichTextBox richtxtSend;
        internal System.Windows.Forms.Button btnSendMessage;
        internal System.Windows.Forms.Label lblTargetComPort;
        internal System.Windows.Forms.RichTextBox rtbCurrentReceived;
        internal System.Windows.Forms.RichTextBox richtxtHistory;
        internal System.Windows.Forms.CheckBox chkGB2312;
        }
    }