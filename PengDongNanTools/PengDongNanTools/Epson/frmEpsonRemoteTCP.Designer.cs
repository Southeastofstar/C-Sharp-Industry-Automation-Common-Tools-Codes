namespace PengDongNanTools
    {
    partial class frmEpsonRemoteTCP
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEpsonRemoteTCP));
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnConnectRobot = new System.Windows.Forms.Button();
            this.rbtnLeftHand = new System.Windows.Forms.RadioButton();
            this.ScanningTimer = new System.Windows.Forms.Timer(this.components);
            this.btnLogout = new System.Windows.Forms.Button();
            this.rbtnRightHand = new System.Windows.Forms.RadioButton();
            this.gpbSetting = new System.Windows.Forms.GroupBox();
            this.txtRobotRemotePort = new System.Windows.Forms.TextBox();
            this.btnTeach = new System.Windows.Forms.Button();
            this.lblStepDistance = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.lblIPAddress = new System.Windows.Forms.Label();
            this.txtRemoteEpsonIPAddress = new System.Windows.Forms.TextBox();
            this.lblPoint = new System.Windows.Forms.Label();
            this.txtJogDistance = new System.Windows.Forms.TextBox();
            this.btnJump = new System.Windows.Forms.Button();
            this.lblPassword = new System.Windows.Forms.Label();
            this.btnSetSpeed = new System.Windows.Forms.Button();
            this.btnSetACCELSpeed = new System.Windows.Forms.Button();
            this.cmbPointName = new System.Windows.Forms.ComboBox();
            this.lblAccelSpeed = new System.Windows.Forms.Label();
            this.txtAccelSpeed = new System.Windows.Forms.TextBox();
            this.txtDecelSpeed = new System.Windows.Forms.TextBox();
            this.lblPowerStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblXPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblHand = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblReady = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPaused = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblRunning = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblYPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDateAndTime = new System.Windows.Forms.Label();
            this.ToolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblWPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblZPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblHandStyle = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblUPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.picDynamicPicture = new System.Windows.Forms.PictureBox();
            this.txtExecute = new System.Windows.Forms.TextBox();
            this.rtbHistory = new System.Windows.Forms.RichTextBox();
            this.lblError = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblOutputSignal = new System.Windows.Forms.Label();
            this.lblInputSignal = new System.Windows.Forms.Label();
            this.lblEStop = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmbBrakeOffJoint = new System.Windows.Forms.ComboBox();
            this.lblSafeguard = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmbBrakeOnJoint = new System.Windows.Forms.ComboBox();
            this.cmbSFreeJoint = new System.Windows.Forms.ComboBox();
            this.cmbSLockJoint = new System.Windows.Forms.ComboBox();
            this.btnPowerHigh = new System.Windows.Forms.Button();
            this.btnAllBrakeOff = new System.Windows.Forms.Button();
            this.btnMotorOff = new System.Windows.Forms.Button();
            this.btnMotorOn = new System.Windows.Forms.Button();
            this.btnBrakeOff = new System.Windows.Forms.Button();
            this.btnAllBrakeOn = new System.Windows.Forms.Button();
            this.btnWJogNegative = new System.Windows.Forms.Button();
            this.btnSFreeAll = new System.Windows.Forms.Button();
            this.btnVJogPositive = new System.Windows.Forms.Button();
            this.btnURotateNegative = new System.Windows.Forms.Button();
            this.btnWJogPositive = new System.Windows.Forms.Button();
            this.gpbOperation = new System.Windows.Forms.GroupBox();
            this.btnBrakeOn = new System.Windows.Forms.Button();
            this.btnVJogNegative = new System.Windows.Forms.Button();
            this.btnSFree = new System.Windows.Forms.Button();
            this.btnURotatePositive = new System.Windows.Forms.Button();
            this.btnZDownward = new System.Windows.Forms.Button();
            this.btnSLockAll = new System.Windows.Forms.Button();
            this.btnZUpward = new System.Windows.Forms.Button();
            this.btnYJogNegative = new System.Windows.Forms.Button();
            this.btnSLock = new System.Windows.Forms.Button();
            this.btnYJogPositive = new System.Windows.Forms.Button();
            this.btnXJogNegative = new System.Windows.Forms.Button();
            this.btnXJogPositive = new System.Windows.Forms.Button();
            this.btnPowerLow = new System.Windows.Forms.Button();
            this.btnExportPointDataToExcel = new System.Windows.Forms.Button();
            this.Hand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtPointQty = new System.Windows.Forms.TextBox();
            this.lblPointQty = new System.Windows.Forms.Label();
            this.gpbPointData = new System.Windows.Forms.GroupBox();
            this.btnSavePoints = new System.Windows.Forms.Button();
            this.btnLoadPoints = new System.Windows.Forms.Button();
            this.dgvPointData = new System.Windows.Forms.DataGridView();
            this.PointName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Z = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.U = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.V = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.W = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblRobot = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTest = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTeach = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAuto = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblWarning = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSError = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnResetRobot = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.cmbMainFunctionNo = new System.Windows.Forms.ComboBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnPauseContinue = new System.Windows.Forms.Button();
            this.gpbProgram = new System.Windows.Forms.GroupBox();
            this.lblReminder = new System.Windows.Forms.Label();
            this.picChinese = new System.Windows.Forms.PictureBox();
            this.picEnglish = new System.Windows.Forms.PictureBox();
            this.picHelp = new System.Windows.Forms.PictureBox();
            this.picOut15 = new System.Windows.Forms.PictureBox();
            this.picOut14 = new System.Windows.Forms.PictureBox();
            this.picOut13 = new System.Windows.Forms.PictureBox();
            this.picOut12 = new System.Windows.Forms.PictureBox();
            this.picOut11 = new System.Windows.Forms.PictureBox();
            this.picOut10 = new System.Windows.Forms.PictureBox();
            this.picOut9 = new System.Windows.Forms.PictureBox();
            this.picOut8 = new System.Windows.Forms.PictureBox();
            this.picOut7 = new System.Windows.Forms.PictureBox();
            this.picOut6 = new System.Windows.Forms.PictureBox();
            this.picOut5 = new System.Windows.Forms.PictureBox();
            this.picOut4 = new System.Windows.Forms.PictureBox();
            this.picOut3 = new System.Windows.Forms.PictureBox();
            this.picOut2 = new System.Windows.Forms.PictureBox();
            this.picOut1 = new System.Windows.Forms.PictureBox();
            this.picOut0 = new System.Windows.Forms.PictureBox();
            this.picIn23 = new System.Windows.Forms.PictureBox();
            this.picIn22 = new System.Windows.Forms.PictureBox();
            this.picIn21 = new System.Windows.Forms.PictureBox();
            this.picIn20 = new System.Windows.Forms.PictureBox();
            this.picIn19 = new System.Windows.Forms.PictureBox();
            this.picIn18 = new System.Windows.Forms.PictureBox();
            this.picIn17 = new System.Windows.Forms.PictureBox();
            this.picIn16 = new System.Windows.Forms.PictureBox();
            this.picIn15 = new System.Windows.Forms.PictureBox();
            this.picIn14 = new System.Windows.Forms.PictureBox();
            this.picIn13 = new System.Windows.Forms.PictureBox();
            this.picIn12 = new System.Windows.Forms.PictureBox();
            this.picIn11 = new System.Windows.Forms.PictureBox();
            this.picIn10 = new System.Windows.Forms.PictureBox();
            this.picIn9 = new System.Windows.Forms.PictureBox();
            this.picIn8 = new System.Windows.Forms.PictureBox();
            this.picIn7 = new System.Windows.Forms.PictureBox();
            this.picIn6 = new System.Windows.Forms.PictureBox();
            this.picIn5 = new System.Windows.Forms.PictureBox();
            this.picIn4 = new System.Windows.Forms.PictureBox();
            this.picIn3 = new System.Windows.Forms.PictureBox();
            this.picIn2 = new System.Windows.Forms.PictureBox();
            this.picIn1 = new System.Windows.Forms.PictureBox();
            this.picIn0 = new System.Windows.Forms.PictureBox();
            this.gpbSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDynamicPicture)).BeginInit();
            this.gpbOperation.SuspendLayout();
            this.gpbPointData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPointData)).BeginInit();
            this.StatusStrip1.SuspendLayout();
            this.gpbProgram.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picChinese)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEnglish)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn0)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("楷体", 10F);
            this.btnLogin.Location = new System.Drawing.Point(182, 172);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 25);
            this.btnLogin.TabIndex = 119;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(97, 172);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(56, 25);
            this.txtPassword.TabIndex = 119;
            this.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip1.SetToolTip(this.txtPassword, "EPSON机械手远程以太网登录密码");
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // btnConnectRobot
            // 
            this.btnConnectRobot.Font = new System.Drawing.Font("楷体", 10F);
            this.btnConnectRobot.Location = new System.Drawing.Point(285, 205);
            this.btnConnectRobot.Name = "btnConnectRobot";
            this.btnConnectRobot.Size = new System.Drawing.Size(83, 25);
            this.btnConnectRobot.TabIndex = 119;
            this.btnConnectRobot.Text = "Connect";
            this.ToolTip1.SetToolTip(this.btnConnectRobot, "与机器人建立远程以太网通讯");
            this.btnConnectRobot.UseVisualStyleBackColor = true;
            this.btnConnectRobot.Click += new System.EventHandler(this.btnConnectRobot_Click);
            // 
            // rbtnLeftHand
            // 
            this.rbtnLeftHand.AutoSize = true;
            this.rbtnLeftHand.Location = new System.Drawing.Point(182, 38);
            this.rbtnLeftHand.Name = "rbtnLeftHand";
            this.rbtnLeftHand.Size = new System.Drawing.Size(100, 19);
            this.rbtnLeftHand.TabIndex = 118;
            this.rbtnLeftHand.TabStop = true;
            this.rbtnLeftHand.Text = "Left Hand";
            this.rbtnLeftHand.UseVisualStyleBackColor = true;
            this.rbtnLeftHand.CheckedChanged += new System.EventHandler(this.rbtnLeftHand_CheckedChanged);
            // 
            // ScanningTimer
            // 
            this.ScanningTimer.Interval = 1000;
            this.ScanningTimer.Tick += new System.EventHandler(this.ScanningTimer_Tick);
            // 
            // btnLogout
            // 
            this.btnLogout.Font = new System.Drawing.Font("楷体", 10F);
            this.btnLogout.Location = new System.Drawing.Point(285, 173);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(83, 25);
            this.btnLogout.TabIndex = 119;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // rbtnRightHand
            // 
            this.rbtnRightHand.AutoSize = true;
            this.rbtnRightHand.Location = new System.Drawing.Point(182, 16);
            this.rbtnRightHand.Name = "rbtnRightHand";
            this.rbtnRightHand.Size = new System.Drawing.Size(108, 19);
            this.rbtnRightHand.TabIndex = 117;
            this.rbtnRightHand.TabStop = true;
            this.rbtnRightHand.Text = "Right Hand";
            this.rbtnRightHand.UseVisualStyleBackColor = true;
            this.rbtnRightHand.CheckedChanged += new System.EventHandler(this.rbtnRightHand_CheckedChanged);
            // 
            // gpbSetting
            // 
            this.gpbSetting.Controls.Add(this.txtRobotRemotePort);
            this.gpbSetting.Controls.Add(this.btnLogin);
            this.gpbSetting.Controls.Add(this.txtPassword);
            this.gpbSetting.Controls.Add(this.btnConnectRobot);
            this.gpbSetting.Controls.Add(this.btnLogout);
            this.gpbSetting.Controls.Add(this.rbtnLeftHand);
            this.gpbSetting.Controls.Add(this.rbtnRightHand);
            this.gpbSetting.Controls.Add(this.btnTeach);
            this.gpbSetting.Controls.Add(this.lblStepDistance);
            this.gpbSetting.Controls.Add(this.lblSpeed);
            this.gpbSetting.Controls.Add(this.txtSpeed);
            this.gpbSetting.Controls.Add(this.lblIPAddress);
            this.gpbSetting.Controls.Add(this.txtRemoteEpsonIPAddress);
            this.gpbSetting.Controls.Add(this.lblPoint);
            this.gpbSetting.Controls.Add(this.txtJogDistance);
            this.gpbSetting.Controls.Add(this.btnJump);
            this.gpbSetting.Controls.Add(this.lblPassword);
            this.gpbSetting.Controls.Add(this.btnSetSpeed);
            this.gpbSetting.Controls.Add(this.btnSetACCELSpeed);
            this.gpbSetting.Controls.Add(this.cmbPointName);
            this.gpbSetting.Controls.Add(this.lblAccelSpeed);
            this.gpbSetting.Controls.Add(this.txtAccelSpeed);
            this.gpbSetting.Controls.Add(this.txtDecelSpeed);
            this.gpbSetting.Font = new System.Drawing.Font("宋体", 9F);
            this.gpbSetting.ForeColor = System.Drawing.Color.Blue;
            this.gpbSetting.Location = new System.Drawing.Point(485, 342);
            this.gpbSetting.Name = "gpbSetting";
            this.gpbSetting.Size = new System.Drawing.Size(373, 237);
            this.gpbSetting.TabIndex = 172;
            this.gpbSetting.TabStop = false;
            this.gpbSetting.Text = "Setting";
            // 
            // txtRobotRemotePort
            // 
            this.txtRobotRemotePort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRobotRemotePort.Location = new System.Drawing.Point(228, 208);
            this.txtRobotRemotePort.Name = "txtRobotRemotePort";
            this.txtRobotRemotePort.Size = new System.Drawing.Size(44, 25);
            this.txtRobotRemotePort.TabIndex = 120;
            this.txtRobotRemotePort.Text = "5";
            this.txtRobotRemotePort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip1.SetToolTip(this.txtRobotRemotePort, "EPSON机械手远程以太网端口");
            this.txtRobotRemotePort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRobotRemotePort_KeyDown);
            this.txtRobotRemotePort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRobotRemotePort_KeyPress);
            // 
            // btnTeach
            // 
            this.btnTeach.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTeach.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTeach.Location = new System.Drawing.Point(285, 63);
            this.btnTeach.Name = "btnTeach";
            this.btnTeach.Size = new System.Drawing.Size(83, 25);
            this.btnTeach.TabIndex = 116;
            this.btnTeach.Text = "Teach";
            this.btnTeach.UseVisualStyleBackColor = true;
            this.btnTeach.Click += new System.EventHandler(this.btnTeach_Click);
            // 
            // lblStepDistance
            // 
            this.lblStepDistance.AutoSize = true;
            this.lblStepDistance.ForeColor = System.Drawing.Color.Blue;
            this.lblStepDistance.Location = new System.Drawing.Point(47, 32);
            this.lblStepDistance.Name = "lblStepDistance";
            this.lblStepDistance.Size = new System.Drawing.Size(47, 15);
            this.lblStepDistance.TabIndex = 5;
            this.lblStepDistance.Text = "(mm):";
            // 
            // lblSpeed
            // 
            this.lblSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.ForeColor = System.Drawing.Color.Blue;
            this.lblSpeed.Location = new System.Drawing.Point(32, 106);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(62, 15);
            this.lblSpeed.TabIndex = 13;
            this.lblSpeed.Text = "Speed：";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSpeed.Location = new System.Drawing.Point(97, 103);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(56, 25);
            this.txtSpeed.TabIndex = 14;
            this.txtSpeed.Text = "5";
            this.txtSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip1.SetToolTip(this.txtSpeed, "运行速度，百分比");
            this.txtSpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSpeed_KeyPress);
            // 
            // lblIPAddress
            // 
            this.lblIPAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIPAddress.AutoSize = true;
            this.lblIPAddress.ForeColor = System.Drawing.Color.Blue;
            this.lblIPAddress.Location = new System.Drawing.Point(32, 213);
            this.lblIPAddress.Name = "lblIPAddress";
            this.lblIPAddress.Size = new System.Drawing.Size(62, 15);
            this.lblIPAddress.TabIndex = 114;
            this.lblIPAddress.Text = "Robot：";
            // 
            // txtRemoteEpsonIPAddress
            // 
            this.txtRemoteEpsonIPAddress.Location = new System.Drawing.Point(97, 208);
            this.txtRemoteEpsonIPAddress.Margin = new System.Windows.Forms.Padding(2);
            this.txtRemoteEpsonIPAddress.Name = "txtRemoteEpsonIPAddress";
            this.txtRemoteEpsonIPAddress.Size = new System.Drawing.Size(118, 25);
            this.txtRemoteEpsonIPAddress.TabIndex = 66;
            this.txtRemoteEpsonIPAddress.Text = "192.168.0.101";
            this.txtRemoteEpsonIPAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip1.SetToolTip(this.txtRemoteEpsonIPAddress, "EPSON机械手远程以太网IP地址");
            // 
            // lblPoint
            // 
            this.lblPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPoint.AutoSize = true;
            this.lblPoint.ForeColor = System.Drawing.Color.Blue;
            this.lblPoint.Location = new System.Drawing.Point(32, 71);
            this.lblPoint.Name = "lblPoint";
            this.lblPoint.Size = new System.Drawing.Size(62, 15);
            this.lblPoint.TabIndex = 115;
            this.lblPoint.Text = "Point：";
            // 
            // txtJogDistance
            // 
            this.txtJogDistance.Location = new System.Drawing.Point(97, 27);
            this.txtJogDistance.Name = "txtJogDistance";
            this.txtJogDistance.Size = new System.Drawing.Size(56, 25);
            this.txtJogDistance.TabIndex = 4;
            this.txtJogDistance.Text = "1.0";
            this.txtJogDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip1.SetToolTip(this.txtJogDistance, "点位运动距离");
            this.txtJogDistance.TextChanged += new System.EventHandler(this.txtJogDistance_TextChanged);
            this.txtJogDistance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtJogDistance_KeyPress);
            // 
            // btnJump
            // 
            this.btnJump.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnJump.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnJump.Location = new System.Drawing.Point(182, 63);
            this.btnJump.Name = "btnJump";
            this.btnJump.Size = new System.Drawing.Size(75, 25);
            this.btnJump.TabIndex = 19;
            this.btnJump.Text = "Jump";
            this.btnJump.UseVisualStyleBackColor = true;
            this.btnJump.Click += new System.EventHandler(this.btnJump_Click);
            // 
            // lblPassword
            // 
            this.lblPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPassword.AutoSize = true;
            this.lblPassword.ForeColor = System.Drawing.Color.Blue;
            this.lblPassword.Location = new System.Drawing.Point(8, 180);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(86, 15);
            this.lblPassword.TabIndex = 113;
            this.lblPassword.Text = "Password：";
            // 
            // btnSetSpeed
            // 
            this.btnSetSpeed.Font = new System.Drawing.Font("楷体", 10F);
            this.btnSetSpeed.Location = new System.Drawing.Point(182, 99);
            this.btnSetSpeed.Name = "btnSetSpeed";
            this.btnSetSpeed.Size = new System.Drawing.Size(75, 25);
            this.btnSetSpeed.TabIndex = 95;
            this.btnSetSpeed.Text = "Set";
            this.btnSetSpeed.UseVisualStyleBackColor = true;
            this.btnSetSpeed.Click += new System.EventHandler(this.btnSetSpeed_Click);
            // 
            // btnSetACCELSpeed
            // 
            this.btnSetACCELSpeed.Font = new System.Drawing.Font("楷体", 10F);
            this.btnSetACCELSpeed.Location = new System.Drawing.Point(182, 137);
            this.btnSetACCELSpeed.Name = "btnSetACCELSpeed";
            this.btnSetACCELSpeed.Size = new System.Drawing.Size(75, 25);
            this.btnSetACCELSpeed.TabIndex = 104;
            this.btnSetACCELSpeed.Text = "Set";
            this.btnSetACCELSpeed.UseVisualStyleBackColor = true;
            this.btnSetACCELSpeed.Click += new System.EventHandler(this.btnSetACCELSpeed_Click);
            // 
            // cmbPointName
            // 
            this.cmbPointName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPointName.Font = new System.Drawing.Font("楷体", 9F);
            this.cmbPointName.FormattingEnabled = true;
            this.cmbPointName.Location = new System.Drawing.Point(97, 67);
            this.cmbPointName.Name = "cmbPointName";
            this.cmbPointName.Size = new System.Drawing.Size(56, 23);
            this.cmbPointName.TabIndex = 105;
            // 
            // lblAccelSpeed
            // 
            this.lblAccelSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAccelSpeed.AutoSize = true;
            this.lblAccelSpeed.ForeColor = System.Drawing.Color.Blue;
            this.lblAccelSpeed.Location = new System.Drawing.Point(32, 138);
            this.lblAccelSpeed.Name = "lblAccelSpeed";
            this.lblAccelSpeed.Size = new System.Drawing.Size(62, 30);
            this.lblAccelSpeed.TabIndex = 106;
            this.lblAccelSpeed.Text = "Accel\r\nSpeed：";
            // 
            // txtAccelSpeed
            // 
            this.txtAccelSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAccelSpeed.Location = new System.Drawing.Point(97, 141);
            this.txtAccelSpeed.Name = "txtAccelSpeed";
            this.txtAccelSpeed.Size = new System.Drawing.Size(26, 25);
            this.txtAccelSpeed.TabIndex = 107;
            this.txtAccelSpeed.Text = "5";
            this.txtAccelSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip1.SetToolTip(this.txtAccelSpeed, "加速度");
            this.txtAccelSpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAccelSpeed_KeyPress);
            // 
            // txtDecelSpeed
            // 
            this.txtDecelSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDecelSpeed.Location = new System.Drawing.Point(126, 141);
            this.txtDecelSpeed.Name = "txtDecelSpeed";
            this.txtDecelSpeed.Size = new System.Drawing.Size(26, 25);
            this.txtDecelSpeed.TabIndex = 107;
            this.txtDecelSpeed.Text = "5";
            this.txtDecelSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip1.SetToolTip(this.txtDecelSpeed, "减速度");
            this.txtDecelSpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDecelSpeed_KeyPress);
            // 
            // lblPowerStatus
            // 
            this.lblPowerStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblPowerStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblPowerStatus.Name = "lblPowerStatus";
            this.lblPowerStatus.Size = new System.Drawing.Size(107, 19);
            this.lblPowerStatus.Text = "Power Status";
            // 
            // lblXPos
            // 
            this.lblXPos.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblXPos.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblXPos.Name = "lblXPos";
            this.lblXPos.Size = new System.Drawing.Size(59, 19);
            this.lblXPos.Text = "      ";
            // 
            // lblHand
            // 
            this.lblHand.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblHand.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblHand.Name = "lblHand";
            this.lblHand.Size = new System.Drawing.Size(59, 19);
            this.lblHand.Text = "      ";
            // 
            // ToolStripStatusLabel1
            // 
            this.ToolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1";
            this.ToolStripStatusLabel1.Size = new System.Drawing.Size(27, 19);
            this.ToolStripStatusLabel1.Text = "X:";
            // 
            // ToolStripStatusLabel2
            // 
            this.ToolStripStatusLabel2.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2";
            this.ToolStripStatusLabel2.Size = new System.Drawing.Size(27, 19);
            this.ToolStripStatusLabel2.Text = "Y:";
            // 
            // lblReady
            // 
            this.lblReady.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblReady.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblReady.Name = "lblReady";
            this.lblReady.Size = new System.Drawing.Size(51, 19);
            this.lblReady.Text = "Ready";
            // 
            // lblPaused
            // 
            this.lblPaused.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblPaused.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblPaused.Name = "lblPaused";
            this.lblPaused.Size = new System.Drawing.Size(59, 19);
            this.lblPaused.Text = "Paused";
            // 
            // lblRunning
            // 
            this.lblRunning.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblRunning.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblRunning.Name = "lblRunning";
            this.lblRunning.Size = new System.Drawing.Size(67, 19);
            this.lblRunning.Text = "Running";
            // 
            // lblYPos
            // 
            this.lblYPos.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblYPos.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblYPos.Name = "lblYPos";
            this.lblYPos.Size = new System.Drawing.Size(75, 19);
            this.lblYPos.Text = "        ";
            // 
            // lblDateAndTime
            // 
            this.lblDateAndTime.AutoSize = true;
            this.lblDateAndTime.Font = new System.Drawing.Font("楷体", 10F);
            this.lblDateAndTime.ForeColor = System.Drawing.Color.Blue;
            this.lblDateAndTime.Location = new System.Drawing.Point(3, 1);
            this.lblDateAndTime.Name = "lblDateAndTime";
            this.lblDateAndTime.Size = new System.Drawing.Size(125, 17);
            this.lblDateAndTime.TabIndex = 175;
            this.lblDateAndTime.Text = "Date and Time";
            // 
            // ToolStripStatusLabel3
            // 
            this.ToolStripStatusLabel3.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolStripStatusLabel3.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.ToolStripStatusLabel3.Name = "ToolStripStatusLabel3";
            this.ToolStripStatusLabel3.Size = new System.Drawing.Size(27, 19);
            this.ToolStripStatusLabel3.Text = "Z:";
            // 
            // lblWPos
            // 
            this.lblWPos.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblWPos.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblWPos.Name = "lblWPos";
            this.lblWPos.Size = new System.Drawing.Size(75, 19);
            this.lblWPos.Text = "        ";
            // 
            // lblZPos
            // 
            this.lblZPos.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblZPos.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblZPos.Name = "lblZPos";
            this.lblZPos.Size = new System.Drawing.Size(75, 19);
            this.lblZPos.Text = "        ";
            // 
            // ToolStripStatusLabel6
            // 
            this.ToolStripStatusLabel6.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolStripStatusLabel6.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.ToolStripStatusLabel6.Name = "ToolStripStatusLabel6";
            this.ToolStripStatusLabel6.Size = new System.Drawing.Size(27, 19);
            this.ToolStripStatusLabel6.Text = "W:";
            // 
            // lblHandStyle
            // 
            this.lblHandStyle.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblHandStyle.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblHandStyle.Name = "lblHandStyle";
            this.lblHandStyle.Size = new System.Drawing.Size(51, 19);
            this.lblHandStyle.Text = "Hand:";
            // 
            // lblVPos
            // 
            this.lblVPos.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblVPos.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblVPos.Name = "lblVPos";
            this.lblVPos.Size = new System.Drawing.Size(75, 19);
            this.lblVPos.Text = "        ";
            // 
            // ToolStripStatusLabel4
            // 
            this.ToolStripStatusLabel4.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolStripStatusLabel4.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.ToolStripStatusLabel4.Name = "ToolStripStatusLabel4";
            this.ToolStripStatusLabel4.Size = new System.Drawing.Size(27, 19);
            this.ToolStripStatusLabel4.Text = "U:";
            // 
            // ToolStripStatusLabel5
            // 
            this.ToolStripStatusLabel5.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolStripStatusLabel5.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.ToolStripStatusLabel5.Name = "ToolStripStatusLabel5";
            this.ToolStripStatusLabel5.Size = new System.Drawing.Size(27, 19);
            this.ToolStripStatusLabel5.Text = "V:";
            // 
            // lblUPos
            // 
            this.lblUPos.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Top | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblUPos.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblUPos.Name = "lblUPos";
            this.lblUPos.Size = new System.Drawing.Size(75, 19);
            this.lblUPos.Text = "        ";
            // 
            // ToolTip1
            // 
            this.ToolTip1.IsBalloon = true;
            // 
            // picDynamicPicture
            // 
            this.picDynamicPicture.Image = global::PengDongNanTools.Properties.Resources.GreenOff;
            this.picDynamicPicture.Location = new System.Drawing.Point(1032, 1);
            this.picDynamicPicture.Margin = new System.Windows.Forms.Padding(2);
            this.picDynamicPicture.Name = "picDynamicPicture";
            this.picDynamicPicture.Size = new System.Drawing.Size(30, 30);
            this.picDynamicPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDynamicPicture.TabIndex = 173;
            this.picDynamicPicture.TabStop = false;
            this.ToolTip1.SetToolTip(this.picDynamicPicture, "单击切换是否在文本框中显示信息");
            this.picDynamicPicture.Click += new System.EventHandler(this.picDynamicPicture_Click);
            this.picDynamicPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picDynamicPicture_MouseDown);
            // 
            // txtExecute
            // 
            this.txtExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExecute.Location = new System.Drawing.Point(841, 624);
            this.txtExecute.Name = "txtExecute";
            this.txtExecute.Size = new System.Drawing.Size(176, 25);
            this.txtExecute.TabIndex = 170;
            this.txtExecute.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip1.SetToolTip(this.txtExecute, "通过EPSON机械手远程以太网执行的命令");
            this.txtExecute.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtExecute_KeyDown);
            this.txtExecute.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtExecute_KeyPress);
            // 
            // rtbHistory
            // 
            this.rtbHistory.AutoWordSelection = true;
            this.rtbHistory.BackColor = System.Drawing.Color.Black;
            this.rtbHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbHistory.ForeColor = System.Drawing.Color.Lime;
            this.rtbHistory.Location = new System.Drawing.Point(863, 342);
            this.rtbHistory.Margin = new System.Windows.Forms.Padding(2);
            this.rtbHistory.Name = "rtbHistory";
            this.rtbHistory.ReadOnly = true;
            this.rtbHistory.ShowSelectionMargin = true;
            this.rtbHistory.Size = new System.Drawing.Size(216, 237);
            this.rtbHistory.TabIndex = 125;
            this.rtbHistory.Text = "";
            this.ToolTip1.SetToolTip(this.rtbHistory, "程序运行历史记录");
            this.rtbHistory.TextChanged += new System.EventHandler(this.rtbHistory_TextChanged);
            this.rtbHistory.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.rtbHistory_MouseDoubleClick);
            this.rtbHistory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rtbHistory_MouseDown);
            // 
            // lblError
            // 
            this.lblError.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblError.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(51, 19);
            this.lblError.Text = "Error";
            // 
            // lblOutputSignal
            // 
            this.lblOutputSignal.AutoSize = true;
            this.lblOutputSignal.Location = new System.Drawing.Point(5, 630);
            this.lblOutputSignal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblOutputSignal.Name = "lblOutputSignal";
            this.lblOutputSignal.Size = new System.Drawing.Size(126, 15);
            this.lblOutputSignal.TabIndex = 152;
            this.lblOutputSignal.Text = "Output Signal：";
            // 
            // lblInputSignal
            // 
            this.lblInputSignal.AutoSize = true;
            this.lblInputSignal.Location = new System.Drawing.Point(5, 605);
            this.lblInputSignal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblInputSignal.Name = "lblInputSignal";
            this.lblInputSignal.Size = new System.Drawing.Size(118, 15);
            this.lblInputSignal.TabIndex = 151;
            this.lblInputSignal.Text = "Input Signal：";
            // 
            // lblEStop
            // 
            this.lblEStop.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblEStop.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblEStop.Name = "lblEStop";
            this.lblEStop.Size = new System.Drawing.Size(51, 19);
            this.lblEStop.Text = "EStop";
            // 
            // cmbBrakeOffJoint
            // 
            this.cmbBrakeOffJoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBrakeOffJoint.Font = new System.Drawing.Font("楷体", 10F);
            this.cmbBrakeOffJoint.FormattingEnabled = true;
            this.cmbBrakeOffJoint.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.cmbBrakeOffJoint.Location = new System.Drawing.Point(295, 278);
            this.cmbBrakeOffJoint.Name = "cmbBrakeOffJoint";
            this.cmbBrakeOffJoint.Size = new System.Drawing.Size(30, 25);
            this.cmbBrakeOffJoint.TabIndex = 112;
            // 
            // lblSafeguard
            // 
            this.lblSafeguard.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblSafeguard.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblSafeguard.Name = "lblSafeguard";
            this.lblSafeguard.Size = new System.Drawing.Size(83, 19);
            this.lblSafeguard.Text = "Safeguard";
            // 
            // cmbBrakeOnJoint
            // 
            this.cmbBrakeOnJoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBrakeOnJoint.Font = new System.Drawing.Font("楷体", 10F);
            this.cmbBrakeOnJoint.FormattingEnabled = true;
            this.cmbBrakeOnJoint.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.cmbBrakeOnJoint.Location = new System.Drawing.Point(295, 226);
            this.cmbBrakeOnJoint.Name = "cmbBrakeOnJoint";
            this.cmbBrakeOnJoint.Size = new System.Drawing.Size(30, 25);
            this.cmbBrakeOnJoint.TabIndex = 111;
            // 
            // cmbSFreeJoint
            // 
            this.cmbSFreeJoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSFreeJoint.Font = new System.Drawing.Font("楷体", 10F);
            this.cmbSFreeJoint.FormattingEnabled = true;
            this.cmbSFreeJoint.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.cmbSFreeJoint.Location = new System.Drawing.Point(295, 177);
            this.cmbSFreeJoint.Name = "cmbSFreeJoint";
            this.cmbSFreeJoint.Size = new System.Drawing.Size(30, 25);
            this.cmbSFreeJoint.TabIndex = 110;
            // 
            // cmbSLockJoint
            // 
            this.cmbSLockJoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSLockJoint.Font = new System.Drawing.Font("楷体", 10F);
            this.cmbSLockJoint.FormattingEnabled = true;
            this.cmbSLockJoint.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.cmbSLockJoint.Location = new System.Drawing.Point(295, 128);
            this.cmbSLockJoint.Name = "cmbSLockJoint";
            this.cmbSLockJoint.Size = new System.Drawing.Size(30, 25);
            this.cmbSLockJoint.TabIndex = 109;
            // 
            // btnPowerHigh
            // 
            this.btnPowerHigh.Font = new System.Drawing.Font("楷体", 10F);
            this.btnPowerHigh.Location = new System.Drawing.Point(192, 80);
            this.btnPowerHigh.Name = "btnPowerHigh";
            this.btnPowerHigh.Size = new System.Drawing.Size(110, 25);
            this.btnPowerHigh.TabIndex = 93;
            this.btnPowerHigh.Text = "Power High";
            this.btnPowerHigh.UseVisualStyleBackColor = true;
            this.btnPowerHigh.Click += new System.EventHandler(this.btnPowerHigh_Click);
            // 
            // btnAllBrakeOff
            // 
            this.btnAllBrakeOff.Font = new System.Drawing.Font("楷体", 10F);
            this.btnAllBrakeOff.Location = new System.Drawing.Point(354, 276);
            this.btnAllBrakeOff.Name = "btnAllBrakeOff";
            this.btnAllBrakeOff.Size = new System.Drawing.Size(117, 25);
            this.btnAllBrakeOff.TabIndex = 103;
            this.btnAllBrakeOff.Text = "AllBrakeOff";
            this.btnAllBrakeOff.UseVisualStyleBackColor = true;
            this.btnAllBrakeOff.Click += new System.EventHandler(this.btnAllBrakeOff_Click);
            // 
            // btnMotorOff
            // 
            this.btnMotorOff.Font = new System.Drawing.Font("楷体", 10F);
            this.btnMotorOff.Location = new System.Drawing.Point(354, 31);
            this.btnMotorOff.Name = "btnMotorOff";
            this.btnMotorOff.Size = new System.Drawing.Size(117, 25);
            this.btnMotorOff.TabIndex = 91;
            this.btnMotorOff.Text = "Motor Off";
            this.btnMotorOff.UseVisualStyleBackColor = true;
            this.btnMotorOff.Click += new System.EventHandler(this.btnMotorOff_Click);
            // 
            // btnMotorOn
            // 
            this.btnMotorOn.Font = new System.Drawing.Font("楷体", 10F);
            this.btnMotorOn.Location = new System.Drawing.Point(192, 31);
            this.btnMotorOn.Name = "btnMotorOn";
            this.btnMotorOn.Size = new System.Drawing.Size(110, 25);
            this.btnMotorOn.TabIndex = 90;
            this.btnMotorOn.Text = "Motor On";
            this.btnMotorOn.UseVisualStyleBackColor = true;
            this.btnMotorOn.Click += new System.EventHandler(this.btnMotorOn_Click);
            // 
            // btnBrakeOff
            // 
            this.btnBrakeOff.Font = new System.Drawing.Font("楷体", 10F);
            this.btnBrakeOff.Location = new System.Drawing.Point(192, 276);
            this.btnBrakeOff.Name = "btnBrakeOff";
            this.btnBrakeOff.Size = new System.Drawing.Size(91, 25);
            this.btnBrakeOff.TabIndex = 102;
            this.btnBrakeOff.Text = "BrakeOff";
            this.btnBrakeOff.UseVisualStyleBackColor = true;
            this.btnBrakeOff.Click += new System.EventHandler(this.btnBrakeOff_Click);
            // 
            // btnAllBrakeOn
            // 
            this.btnAllBrakeOn.Font = new System.Drawing.Font("楷体", 10F);
            this.btnAllBrakeOn.Location = new System.Drawing.Point(354, 227);
            this.btnAllBrakeOn.Name = "btnAllBrakeOn";
            this.btnAllBrakeOn.Size = new System.Drawing.Size(117, 25);
            this.btnAllBrakeOn.TabIndex = 101;
            this.btnAllBrakeOn.Text = "AllBrakeOn";
            this.btnAllBrakeOn.UseVisualStyleBackColor = true;
            this.btnAllBrakeOn.Click += new System.EventHandler(this.btnAllBrakeOn_Click);
            // 
            // btnWJogNegative
            // 
            this.btnWJogNegative.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.btnWJogNegative.Location = new System.Drawing.Point(97, 276);
            this.btnWJogNegative.Name = "btnWJogNegative";
            this.btnWJogNegative.Size = new System.Drawing.Size(60, 25);
            this.btnWJogNegative.TabIndex = 86;
            this.btnWJogNegative.Text = "W-";
            this.btnWJogNegative.UseVisualStyleBackColor = true;
            this.btnWJogNegative.Click += new System.EventHandler(this.btnWJogNegative_Click);
            // 
            // btnSFreeAll
            // 
            this.btnSFreeAll.Font = new System.Drawing.Font("楷体", 10F);
            this.btnSFreeAll.Location = new System.Drawing.Point(354, 178);
            this.btnSFreeAll.Name = "btnSFreeAll";
            this.btnSFreeAll.Size = new System.Drawing.Size(117, 25);
            this.btnSFreeAll.TabIndex = 99;
            this.btnSFreeAll.Text = "SFreeAll";
            this.btnSFreeAll.UseVisualStyleBackColor = true;
            this.btnSFreeAll.Click += new System.EventHandler(this.btnSFreeAll_Click);
            // 
            // btnVJogPositive
            // 
            this.btnVJogPositive.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.btnVJogPositive.Location = new System.Drawing.Point(97, 129);
            this.btnVJogPositive.Name = "btnVJogPositive";
            this.btnVJogPositive.Size = new System.Drawing.Size(60, 25);
            this.btnVJogPositive.TabIndex = 83;
            this.btnVJogPositive.Text = "V+";
            this.btnVJogPositive.UseVisualStyleBackColor = true;
            this.btnVJogPositive.Click += new System.EventHandler(this.btnVJogPositive_Click);
            // 
            // btnURotateNegative
            // 
            this.btnURotateNegative.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.btnURotateNegative.Location = new System.Drawing.Point(97, 80);
            this.btnURotateNegative.Name = "btnURotateNegative";
            this.btnURotateNegative.Size = new System.Drawing.Size(60, 25);
            this.btnURotateNegative.TabIndex = 82;
            this.btnURotateNegative.Text = "U-";
            this.btnURotateNegative.UseVisualStyleBackColor = true;
            this.btnURotateNegative.Click += new System.EventHandler(this.btnURotateNegative_Click);
            // 
            // btnWJogPositive
            // 
            this.btnWJogPositive.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.btnWJogPositive.Location = new System.Drawing.Point(97, 227);
            this.btnWJogPositive.Name = "btnWJogPositive";
            this.btnWJogPositive.Size = new System.Drawing.Size(60, 25);
            this.btnWJogPositive.TabIndex = 85;
            this.btnWJogPositive.Text = "W+";
            this.btnWJogPositive.UseVisualStyleBackColor = true;
            this.btnWJogPositive.Click += new System.EventHandler(this.btnWJogPositive_Click);
            // 
            // gpbOperation
            // 
            this.gpbOperation.BackColor = System.Drawing.SystemColors.Control;
            this.gpbOperation.Controls.Add(this.cmbBrakeOffJoint);
            this.gpbOperation.Controls.Add(this.cmbBrakeOnJoint);
            this.gpbOperation.Controls.Add(this.cmbSFreeJoint);
            this.gpbOperation.Controls.Add(this.cmbSLockJoint);
            this.gpbOperation.Controls.Add(this.btnPowerHigh);
            this.gpbOperation.Controls.Add(this.btnAllBrakeOff);
            this.gpbOperation.Controls.Add(this.btnMotorOff);
            this.gpbOperation.Controls.Add(this.btnMotorOn);
            this.gpbOperation.Controls.Add(this.btnBrakeOff);
            this.gpbOperation.Controls.Add(this.btnAllBrakeOn);
            this.gpbOperation.Controls.Add(this.btnWJogNegative);
            this.gpbOperation.Controls.Add(this.btnBrakeOn);
            this.gpbOperation.Controls.Add(this.btnWJogPositive);
            this.gpbOperation.Controls.Add(this.btnVJogNegative);
            this.gpbOperation.Controls.Add(this.btnSFreeAll);
            this.gpbOperation.Controls.Add(this.btnVJogPositive);
            this.gpbOperation.Controls.Add(this.btnURotateNegative);
            this.gpbOperation.Controls.Add(this.btnSFree);
            this.gpbOperation.Controls.Add(this.btnURotatePositive);
            this.gpbOperation.Controls.Add(this.btnZDownward);
            this.gpbOperation.Controls.Add(this.btnSLockAll);
            this.gpbOperation.Controls.Add(this.btnZUpward);
            this.gpbOperation.Controls.Add(this.btnYJogNegative);
            this.gpbOperation.Controls.Add(this.btnSLock);
            this.gpbOperation.Controls.Add(this.btnYJogPositive);
            this.gpbOperation.Controls.Add(this.btnXJogNegative);
            this.gpbOperation.Controls.Add(this.btnXJogPositive);
            this.gpbOperation.Controls.Add(this.btnPowerLow);
            this.gpbOperation.Font = new System.Drawing.Font("宋体", 9F);
            this.gpbOperation.ForeColor = System.Drawing.Color.Blue;
            this.gpbOperation.Location = new System.Drawing.Point(486, 23);
            this.gpbOperation.Name = "gpbOperation";
            this.gpbOperation.Size = new System.Drawing.Size(478, 313);
            this.gpbOperation.TabIndex = 123;
            this.gpbOperation.TabStop = false;
            this.gpbOperation.Text = "Operation";
            // 
            // btnBrakeOn
            // 
            this.btnBrakeOn.Font = new System.Drawing.Font("楷体", 10F);
            this.btnBrakeOn.Location = new System.Drawing.Point(192, 227);
            this.btnBrakeOn.Name = "btnBrakeOn";
            this.btnBrakeOn.Size = new System.Drawing.Size(91, 25);
            this.btnBrakeOn.TabIndex = 100;
            this.btnBrakeOn.Text = "Brake On";
            this.btnBrakeOn.UseVisualStyleBackColor = true;
            this.btnBrakeOn.Click += new System.EventHandler(this.btnBrakeOn_Click);
            // 
            // btnVJogNegative
            // 
            this.btnVJogNegative.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.btnVJogNegative.Location = new System.Drawing.Point(97, 178);
            this.btnVJogNegative.Name = "btnVJogNegative";
            this.btnVJogNegative.Size = new System.Drawing.Size(60, 25);
            this.btnVJogNegative.TabIndex = 84;
            this.btnVJogNegative.Text = "V-";
            this.btnVJogNegative.UseVisualStyleBackColor = true;
            this.btnVJogNegative.Click += new System.EventHandler(this.btnVJogNegative_Click);
            // 
            // btnSFree
            // 
            this.btnSFree.Font = new System.Drawing.Font("楷体", 10F);
            this.btnSFree.Location = new System.Drawing.Point(192, 178);
            this.btnSFree.Name = "btnSFree";
            this.btnSFree.Size = new System.Drawing.Size(91, 25);
            this.btnSFree.TabIndex = 98;
            this.btnSFree.Text = "SFree";
            this.btnSFree.UseVisualStyleBackColor = true;
            this.btnSFree.Click += new System.EventHandler(this.btnSFree_Click);
            // 
            // btnURotatePositive
            // 
            this.btnURotatePositive.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.btnURotatePositive.Location = new System.Drawing.Point(97, 31);
            this.btnURotatePositive.Name = "btnURotatePositive";
            this.btnURotatePositive.Size = new System.Drawing.Size(60, 25);
            this.btnURotatePositive.TabIndex = 81;
            this.btnURotatePositive.Text = "U+";
            this.btnURotatePositive.UseVisualStyleBackColor = true;
            this.btnURotatePositive.Click += new System.EventHandler(this.btnURotatePositive_Click);
            // 
            // btnZDownward
            // 
            this.btnZDownward.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.btnZDownward.Location = new System.Drawing.Point(10, 276);
            this.btnZDownward.Name = "btnZDownward";
            this.btnZDownward.Size = new System.Drawing.Size(60, 25);
            this.btnZDownward.TabIndex = 80;
            this.btnZDownward.Text = "Z-";
            this.btnZDownward.UseVisualStyleBackColor = true;
            this.btnZDownward.Click += new System.EventHandler(this.btnZDownward_Click);
            // 
            // btnSLockAll
            // 
            this.btnSLockAll.Font = new System.Drawing.Font("楷体", 10F);
            this.btnSLockAll.Location = new System.Drawing.Point(354, 129);
            this.btnSLockAll.Name = "btnSLockAll";
            this.btnSLockAll.Size = new System.Drawing.Size(117, 25);
            this.btnSLockAll.TabIndex = 97;
            this.btnSLockAll.Text = "SLockAll";
            this.btnSLockAll.UseVisualStyleBackColor = true;
            this.btnSLockAll.Click += new System.EventHandler(this.btnSLockAll_Click);
            // 
            // btnZUpward
            // 
            this.btnZUpward.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.btnZUpward.Location = new System.Drawing.Point(10, 227);
            this.btnZUpward.Name = "btnZUpward";
            this.btnZUpward.Size = new System.Drawing.Size(60, 25);
            this.btnZUpward.TabIndex = 79;
            this.btnZUpward.Text = "Z+";
            this.btnZUpward.UseVisualStyleBackColor = true;
            this.btnZUpward.Click += new System.EventHandler(this.btnZUpward_Click);
            // 
            // btnYJogNegative
            // 
            this.btnYJogNegative.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.btnYJogNegative.Location = new System.Drawing.Point(10, 178);
            this.btnYJogNegative.Name = "btnYJogNegative";
            this.btnYJogNegative.Size = new System.Drawing.Size(60, 25);
            this.btnYJogNegative.TabIndex = 78;
            this.btnYJogNegative.Text = "Y-";
            this.btnYJogNegative.UseVisualStyleBackColor = true;
            this.btnYJogNegative.Click += new System.EventHandler(this.btnYJogNegative_Click);
            // 
            // btnSLock
            // 
            this.btnSLock.Font = new System.Drawing.Font("楷体", 10F);
            this.btnSLock.Location = new System.Drawing.Point(192, 129);
            this.btnSLock.Name = "btnSLock";
            this.btnSLock.Size = new System.Drawing.Size(91, 25);
            this.btnSLock.TabIndex = 96;
            this.btnSLock.Text = "SLock";
            this.btnSLock.UseVisualStyleBackColor = true;
            this.btnSLock.Click += new System.EventHandler(this.btnSLock_Click);
            // 
            // btnYJogPositive
            // 
            this.btnYJogPositive.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.btnYJogPositive.Location = new System.Drawing.Point(10, 129);
            this.btnYJogPositive.Name = "btnYJogPositive";
            this.btnYJogPositive.Size = new System.Drawing.Size(60, 25);
            this.btnYJogPositive.TabIndex = 77;
            this.btnYJogPositive.Text = "Y+";
            this.btnYJogPositive.UseVisualStyleBackColor = true;
            this.btnYJogPositive.Click += new System.EventHandler(this.btnYJogPositive_Click);
            // 
            // btnXJogNegative
            // 
            this.btnXJogNegative.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.btnXJogNegative.Location = new System.Drawing.Point(10, 80);
            this.btnXJogNegative.Name = "btnXJogNegative";
            this.btnXJogNegative.Size = new System.Drawing.Size(60, 25);
            this.btnXJogNegative.TabIndex = 76;
            this.btnXJogNegative.Text = "X-";
            this.btnXJogNegative.UseVisualStyleBackColor = true;
            this.btnXJogNegative.Click += new System.EventHandler(this.btnXJogNegative_Click);
            // 
            // btnXJogPositive
            // 
            this.btnXJogPositive.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.btnXJogPositive.Location = new System.Drawing.Point(10, 31);
            this.btnXJogPositive.Name = "btnXJogPositive";
            this.btnXJogPositive.Size = new System.Drawing.Size(60, 25);
            this.btnXJogPositive.TabIndex = 75;
            this.btnXJogPositive.Text = "X+";
            this.btnXJogPositive.UseVisualStyleBackColor = true;
            this.btnXJogPositive.Click += new System.EventHandler(this.btnXJogPositive_Click);
            // 
            // btnPowerLow
            // 
            this.btnPowerLow.Font = new System.Drawing.Font("楷体", 10F);
            this.btnPowerLow.Location = new System.Drawing.Point(354, 80);
            this.btnPowerLow.Name = "btnPowerLow";
            this.btnPowerLow.Size = new System.Drawing.Size(117, 25);
            this.btnPowerLow.TabIndex = 94;
            this.btnPowerLow.Text = "Power Low";
            this.btnPowerLow.UseVisualStyleBackColor = true;
            this.btnPowerLow.Click += new System.EventHandler(this.btnPowerLow_Click);
            // 
            // btnExportPointDataToExcel
            // 
            this.btnExportPointDataToExcel.Font = new System.Drawing.Font("宋体", 9F);
            this.btnExportPointDataToExcel.Location = new System.Drawing.Point(380, 518);
            this.btnExportPointDataToExcel.Name = "btnExportPointDataToExcel";
            this.btnExportPointDataToExcel.Size = new System.Drawing.Size(58, 25);
            this.btnExportPointDataToExcel.TabIndex = 96;
            this.btnExportPointDataToExcel.Text = "Excel";
            this.btnExportPointDataToExcel.UseVisualStyleBackColor = true;
            this.btnExportPointDataToExcel.Click += new System.EventHandler(this.btnExportPointDataToExcel_Click);
            // 
            // Hand
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Hand.DefaultCellStyle = dataGridViewCellStyle1;
            this.Hand.HeaderText = "Hand";
            this.Hand.Name = "Hand";
            this.Hand.ReadOnly = true;
            this.Hand.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // txtPointQty
            // 
            this.txtPointQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPointQty.Location = new System.Drawing.Point(105, 521);
            this.txtPointQty.Name = "txtPointQty";
            this.txtPointQty.Size = new System.Drawing.Size(56, 25);
            this.txtPointQty.TabIndex = 95;
            this.txtPointQty.Text = "100";
            this.txtPointQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPointQty.TextChanged += new System.EventHandler(this.txtPointQty_TextChanged);
            this.txtPointQty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPointQty_KeyDown);
            this.txtPointQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPointQty_KeyPress);
            // 
            // lblPointQty
            // 
            this.lblPointQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPointQty.AutoSize = true;
            this.lblPointQty.Font = new System.Drawing.Font("宋体", 9F);
            this.lblPointQty.ForeColor = System.Drawing.Color.Blue;
            this.lblPointQty.Location = new System.Drawing.Point(8, 525);
            this.lblPointQty.Name = "lblPointQty";
            this.lblPointQty.Size = new System.Drawing.Size(102, 15);
            this.lblPointQty.TabIndex = 94;
            this.lblPointQty.Text = "Point q\'ty：";
            // 
            // gpbPointData
            // 
            this.gpbPointData.Controls.Add(this.btnExportPointDataToExcel);
            this.gpbPointData.Controls.Add(this.txtPointQty);
            this.gpbPointData.Controls.Add(this.lblPointQty);
            this.gpbPointData.Controls.Add(this.btnSavePoints);
            this.gpbPointData.Controls.Add(this.btnLoadPoints);
            this.gpbPointData.Controls.Add(this.dgvPointData);
            this.gpbPointData.Font = new System.Drawing.Font("宋体", 9F);
            this.gpbPointData.ForeColor = System.Drawing.Color.Blue;
            this.gpbPointData.Location = new System.Drawing.Point(12, 23);
            this.gpbPointData.Name = "gpbPointData";
            this.gpbPointData.Size = new System.Drawing.Size(455, 556);
            this.gpbPointData.TabIndex = 124;
            this.gpbPointData.TabStop = false;
            this.gpbPointData.Text = "Point Data";
            // 
            // btnSavePoints
            // 
            this.btnSavePoints.Font = new System.Drawing.Font("宋体", 9F);
            this.btnSavePoints.Location = new System.Drawing.Point(274, 518);
            this.btnSavePoints.Name = "btnSavePoints";
            this.btnSavePoints.Size = new System.Drawing.Size(95, 25);
            this.btnSavePoints.TabIndex = 93;
            this.btnSavePoints.Text = "Save Points";
            this.btnSavePoints.UseVisualStyleBackColor = true;
            this.btnSavePoints.Click += new System.EventHandler(this.btnSavePoints_Click);
            // 
            // btnLoadPoints
            // 
            this.btnLoadPoints.Font = new System.Drawing.Font("宋体", 9F);
            this.btnLoadPoints.Location = new System.Drawing.Point(168, 518);
            this.btnLoadPoints.Name = "btnLoadPoints";
            this.btnLoadPoints.Size = new System.Drawing.Size(95, 25);
            this.btnLoadPoints.TabIndex = 92;
            this.btnLoadPoints.Text = "Load Points";
            this.btnLoadPoints.UseVisualStyleBackColor = true;
            this.btnLoadPoints.Click += new System.EventHandler(this.btnLoadPoints_Click);
            // 
            // dgvPointData
            // 
            this.dgvPointData.AllowUserToAddRows = false;
            this.dgvPointData.AllowUserToDeleteRows = false;
            this.dgvPointData.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPointData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPointData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPointData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PointName,
            this.X,
            this.Y,
            this.Z,
            this.U,
            this.V,
            this.W,
            this.Hand});
            this.dgvPointData.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvPointData.Location = new System.Drawing.Point(3, 21);
            this.dgvPointData.Name = "dgvPointData";
            this.dgvPointData.RowHeadersVisible = false;
            this.dgvPointData.RowTemplate.Height = 23;
            this.dgvPointData.Size = new System.Drawing.Size(449, 483);
            this.dgvPointData.TabIndex = 18;
            this.dgvPointData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPointData_CellContentClick);
            this.dgvPointData.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPointData_CellValueChanged);
            this.dgvPointData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvPointData_KeyPress);
            // 
            // PointName
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PointName.DefaultCellStyle = dataGridViewCellStyle3;
            this.PointName.HeaderText = "Point Name";
            this.PointName.Name = "PointName";
            this.PointName.ReadOnly = true;
            this.PointName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // X
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.X.DefaultCellStyle = dataGridViewCellStyle4;
            this.X.HeaderText = "X";
            this.X.Name = "X";
            this.X.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Y
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Y.DefaultCellStyle = dataGridViewCellStyle5;
            this.Y.HeaderText = "Y";
            this.Y.Name = "Y";
            this.Y.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Z
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Z.DefaultCellStyle = dataGridViewCellStyle6;
            this.Z.HeaderText = "Z";
            this.Z.Name = "Z";
            this.Z.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // U
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.U.DefaultCellStyle = dataGridViewCellStyle7;
            this.U.HeaderText = "U";
            this.U.Name = "U";
            this.U.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // V
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.V.DefaultCellStyle = dataGridViewCellStyle8;
            this.V.HeaderText = "V";
            this.V.Name = "V";
            this.V.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // W
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.W.DefaultCellStyle = dataGridViewCellStyle9;
            this.W.HeaderText = "W";
            this.W.Name = "W";
            this.W.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btnExecute
            // 
            this.btnExecute.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.btnExecute.ForeColor = System.Drawing.Color.Blue;
            this.btnExecute.Location = new System.Drawing.Point(993, 624);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(86, 25);
            this.btnExecute.TabIndex = 169;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.Location = new System.Drawing.Point(15, 212);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(70, 25);
            this.btnAbort.TabIndex = 92;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.Font = new System.Drawing.Font("楷体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblRobot,
            this.lblTest,
            this.lblTeach,
            this.lblAuto,
            this.lblWarning,
            this.lblSError,
            this.lblSafeguard,
            this.lblEStop,
            this.lblError,
            this.lblPaused,
            this.lblRunning,
            this.lblReady,
            this.ToolStripStatusLabel1,
            this.lblXPos,
            this.ToolStripStatusLabel2,
            this.lblYPos,
            this.ToolStripStatusLabel3,
            this.lblZPos,
            this.ToolStripStatusLabel4,
            this.lblUPos,
            this.ToolStripStatusLabel5,
            this.lblVPos,
            this.ToolStripStatusLabel6,
            this.lblWPos,
            this.lblHandStyle,
            this.lblHand,
            this.lblPowerStatus});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 650);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.ShowItemToolTips = true;
            this.StatusStrip1.Size = new System.Drawing.Size(1090, 24);
            this.StatusStrip1.SizingGrip = false;
            this.StatusStrip1.TabIndex = 126;
            this.StatusStrip1.Text = "Robot";
            // 
            // lblRobot
            // 
            this.lblRobot.AutoToolTip = true;
            this.lblRobot.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblRobot.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblRobot.Name = "lblRobot";
            this.lblRobot.Size = new System.Drawing.Size(51, 19);
            this.lblRobot.Text = "Robot";
            this.lblRobot.ToolTipText = "绿色代表已经与机器人建立远程以太网通讯\r\n红色代表断开与机器人的远程以太网通讯\r\n";
            // 
            // lblTest
            // 
            this.lblTest.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblTest.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblTest.Name = "lblTest";
            this.lblTest.Size = new System.Drawing.Size(43, 19);
            this.lblTest.Text = "Test";
            // 
            // lblTeach
            // 
            this.lblTeach.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblTeach.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblTeach.Name = "lblTeach";
            this.lblTeach.Size = new System.Drawing.Size(51, 19);
            this.lblTeach.Text = "Teach";
            // 
            // lblAuto
            // 
            this.lblAuto.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblAuto.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblAuto.Name = "lblAuto";
            this.lblAuto.Size = new System.Drawing.Size(43, 19);
            this.lblAuto.Text = "Auto";
            // 
            // lblWarning
            // 
            this.lblWarning.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblWarning.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(67, 19);
            this.lblWarning.Text = "Warning";
            // 
            // lblSError
            // 
            this.lblSError.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblSError.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblSError.Name = "lblSError";
            this.lblSError.Size = new System.Drawing.Size(59, 19);
            this.lblSError.Text = "SError";
            // 
            // btnResetRobot
            // 
            this.btnResetRobot.Location = new System.Drawing.Point(15, 258);
            this.btnResetRobot.Name = "btnResetRobot";
            this.btnResetRobot.Size = new System.Drawing.Size(70, 25);
            this.btnResetRobot.TabIndex = 89;
            this.btnResetRobot.Text = "Reset";
            this.btnResetRobot.UseVisualStyleBackColor = true;
            this.btnResetRobot.Click += new System.EventHandler(this.btnResetRobot_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(15, 120);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(70, 25);
            this.btnStop.TabIndex = 88;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // cmbMainFunctionNo
            // 
            this.cmbMainFunctionNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMainFunctionNo.Font = new System.Drawing.Font("楷体", 10F);
            this.cmbMainFunctionNo.FormattingEnabled = true;
            this.cmbMainFunctionNo.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7"});
            this.cmbMainFunctionNo.Location = new System.Drawing.Point(15, 28);
            this.cmbMainFunctionNo.Name = "cmbMainFunctionNo";
            this.cmbMainFunctionNo.Size = new System.Drawing.Size(70, 25);
            this.cmbMainFunctionNo.TabIndex = 21;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Font = new System.Drawing.Font("宋体", 9F);
            this.btnRun.Location = new System.Drawing.Point(15, 74);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(70, 25);
            this.btnRun.TabIndex = 16;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnPauseContinue
            // 
            this.btnPauseContinue.Location = new System.Drawing.Point(15, 166);
            this.btnPauseContinue.Name = "btnPauseContinue";
            this.btnPauseContinue.Size = new System.Drawing.Size(70, 25);
            this.btnPauseContinue.TabIndex = 93;
            this.btnPauseContinue.Text = "Pause";
            this.btnPauseContinue.UseVisualStyleBackColor = true;
            this.btnPauseContinue.Click += new System.EventHandler(this.btnPauseContinue_Click);
            // 
            // gpbProgram
            // 
            this.gpbProgram.Controls.Add(this.btnPauseContinue);
            this.gpbProgram.Controls.Add(this.btnRun);
            this.gpbProgram.Controls.Add(this.cmbMainFunctionNo);
            this.gpbProgram.Controls.Add(this.btnStop);
            this.gpbProgram.Controls.Add(this.btnResetRobot);
            this.gpbProgram.Controls.Add(this.btnAbort);
            this.gpbProgram.Font = new System.Drawing.Font("宋体", 9F);
            this.gpbProgram.ForeColor = System.Drawing.Color.Blue;
            this.gpbProgram.Location = new System.Drawing.Point(984, 36);
            this.gpbProgram.Name = "gpbProgram";
            this.gpbProgram.Size = new System.Drawing.Size(97, 297);
            this.gpbProgram.TabIndex = 171;
            this.gpbProgram.TabStop = false;
            this.gpbProgram.Text = "Program";
            // 
            // lblReminder
            // 
            this.lblReminder.AutoSize = true;
            this.lblReminder.Location = new System.Drawing.Point(15, 586);
            this.lblReminder.Name = "lblReminder";
            this.lblReminder.Size = new System.Drawing.Size(262, 15);
            this.lblReminder.TabIndex = 178;
            this.lblReminder.Text = "确保已经激活机器人的远程以太网控制";
            // 
            // picChinese
            // 
            this.picChinese.Image = global::PengDongNanTools.Properties.Resources.ChineseFlag;
            this.picChinese.Location = new System.Drawing.Point(886, 1);
            this.picChinese.Margin = new System.Windows.Forms.Padding(2);
            this.picChinese.Name = "picChinese";
            this.picChinese.Size = new System.Drawing.Size(30, 30);
            this.picChinese.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picChinese.TabIndex = 177;
            this.picChinese.TabStop = false;
            this.picChinese.Click += new System.EventHandler(this.picChinese_Click);
            // 
            // picEnglish
            // 
            this.picEnglish.Image = global::PengDongNanTools.Properties.Resources.FLGUK;
            this.picEnglish.Location = new System.Drawing.Point(934, 1);
            this.picEnglish.Margin = new System.Windows.Forms.Padding(2);
            this.picEnglish.Name = "picEnglish";
            this.picEnglish.Size = new System.Drawing.Size(30, 30);
            this.picEnglish.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picEnglish.TabIndex = 176;
            this.picEnglish.TabStop = false;
            this.picEnglish.Click += new System.EventHandler(this.picEnglish_Click);
            // 
            // picHelp
            // 
            this.picHelp.Image = global::PengDongNanTools.Properties.Resources.Help;
            this.picHelp.Location = new System.Drawing.Point(984, 1);
            this.picHelp.Margin = new System.Windows.Forms.Padding(2);
            this.picHelp.Name = "picHelp";
            this.picHelp.Size = new System.Drawing.Size(30, 30);
            this.picHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHelp.TabIndex = 174;
            this.picHelp.TabStop = false;
            this.picHelp.Click += new System.EventHandler(this.picHelp_Click);
            // 
            // picOut15
            // 
            this.picOut15.Image = ((System.Drawing.Image)(resources.GetObject("picOut15.Image")));
            this.picOut15.Location = new System.Drawing.Point(632, 629);
            this.picOut15.Margin = new System.Windows.Forms.Padding(2);
            this.picOut15.Name = "picOut15";
            this.picOut15.Size = new System.Drawing.Size(15, 16);
            this.picOut15.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut15.TabIndex = 168;
            this.picOut15.TabStop = false;
            // 
            // picOut14
            // 
            this.picOut14.Image = ((System.Drawing.Image)(resources.GetObject("picOut14.Image")));
            this.picOut14.Location = new System.Drawing.Point(599, 629);
            this.picOut14.Margin = new System.Windows.Forms.Padding(2);
            this.picOut14.Name = "picOut14";
            this.picOut14.Size = new System.Drawing.Size(15, 16);
            this.picOut14.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut14.TabIndex = 167;
            this.picOut14.TabStop = false;
            // 
            // picOut13
            // 
            this.picOut13.Image = ((System.Drawing.Image)(resources.GetObject("picOut13.Image")));
            this.picOut13.Location = new System.Drawing.Point(566, 629);
            this.picOut13.Margin = new System.Windows.Forms.Padding(2);
            this.picOut13.Name = "picOut13";
            this.picOut13.Size = new System.Drawing.Size(15, 16);
            this.picOut13.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut13.TabIndex = 166;
            this.picOut13.TabStop = false;
            // 
            // picOut12
            // 
            this.picOut12.Image = ((System.Drawing.Image)(resources.GetObject("picOut12.Image")));
            this.picOut12.Location = new System.Drawing.Point(533, 629);
            this.picOut12.Margin = new System.Windows.Forms.Padding(2);
            this.picOut12.Name = "picOut12";
            this.picOut12.Size = new System.Drawing.Size(15, 16);
            this.picOut12.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut12.TabIndex = 165;
            this.picOut12.TabStop = false;
            // 
            // picOut11
            // 
            this.picOut11.Image = ((System.Drawing.Image)(resources.GetObject("picOut11.Image")));
            this.picOut11.Location = new System.Drawing.Point(500, 629);
            this.picOut11.Margin = new System.Windows.Forms.Padding(2);
            this.picOut11.Name = "picOut11";
            this.picOut11.Size = new System.Drawing.Size(15, 16);
            this.picOut11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut11.TabIndex = 164;
            this.picOut11.TabStop = false;
            // 
            // picOut10
            // 
            this.picOut10.Image = ((System.Drawing.Image)(resources.GetObject("picOut10.Image")));
            this.picOut10.Location = new System.Drawing.Point(467, 629);
            this.picOut10.Margin = new System.Windows.Forms.Padding(2);
            this.picOut10.Name = "picOut10";
            this.picOut10.Size = new System.Drawing.Size(15, 16);
            this.picOut10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut10.TabIndex = 163;
            this.picOut10.TabStop = false;
            // 
            // picOut9
            // 
            this.picOut9.Image = ((System.Drawing.Image)(resources.GetObject("picOut9.Image")));
            this.picOut9.Location = new System.Drawing.Point(434, 629);
            this.picOut9.Margin = new System.Windows.Forms.Padding(2);
            this.picOut9.Name = "picOut9";
            this.picOut9.Size = new System.Drawing.Size(15, 16);
            this.picOut9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut9.TabIndex = 162;
            this.picOut9.TabStop = false;
            // 
            // picOut8
            // 
            this.picOut8.Image = ((System.Drawing.Image)(resources.GetObject("picOut8.Image")));
            this.picOut8.Location = new System.Drawing.Point(401, 629);
            this.picOut8.Margin = new System.Windows.Forms.Padding(2);
            this.picOut8.Name = "picOut8";
            this.picOut8.Size = new System.Drawing.Size(15, 16);
            this.picOut8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut8.TabIndex = 161;
            this.picOut8.TabStop = false;
            // 
            // picOut7
            // 
            this.picOut7.Image = ((System.Drawing.Image)(resources.GetObject("picOut7.Image")));
            this.picOut7.Location = new System.Drawing.Point(368, 629);
            this.picOut7.Margin = new System.Windows.Forms.Padding(2);
            this.picOut7.Name = "picOut7";
            this.picOut7.Size = new System.Drawing.Size(15, 16);
            this.picOut7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut7.TabIndex = 160;
            this.picOut7.TabStop = false;
            // 
            // picOut6
            // 
            this.picOut6.Image = ((System.Drawing.Image)(resources.GetObject("picOut6.Image")));
            this.picOut6.Location = new System.Drawing.Point(335, 629);
            this.picOut6.Margin = new System.Windows.Forms.Padding(2);
            this.picOut6.Name = "picOut6";
            this.picOut6.Size = new System.Drawing.Size(15, 16);
            this.picOut6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut6.TabIndex = 159;
            this.picOut6.TabStop = false;
            // 
            // picOut5
            // 
            this.picOut5.Image = ((System.Drawing.Image)(resources.GetObject("picOut5.Image")));
            this.picOut5.Location = new System.Drawing.Point(302, 629);
            this.picOut5.Margin = new System.Windows.Forms.Padding(2);
            this.picOut5.Name = "picOut5";
            this.picOut5.Size = new System.Drawing.Size(15, 16);
            this.picOut5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut5.TabIndex = 158;
            this.picOut5.TabStop = false;
            // 
            // picOut4
            // 
            this.picOut4.Image = ((System.Drawing.Image)(resources.GetObject("picOut4.Image")));
            this.picOut4.Location = new System.Drawing.Point(269, 629);
            this.picOut4.Margin = new System.Windows.Forms.Padding(2);
            this.picOut4.Name = "picOut4";
            this.picOut4.Size = new System.Drawing.Size(15, 16);
            this.picOut4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut4.TabIndex = 157;
            this.picOut4.TabStop = false;
            // 
            // picOut3
            // 
            this.picOut3.Image = ((System.Drawing.Image)(resources.GetObject("picOut3.Image")));
            this.picOut3.Location = new System.Drawing.Point(236, 629);
            this.picOut3.Margin = new System.Windows.Forms.Padding(2);
            this.picOut3.Name = "picOut3";
            this.picOut3.Size = new System.Drawing.Size(15, 16);
            this.picOut3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut3.TabIndex = 156;
            this.picOut3.TabStop = false;
            // 
            // picOut2
            // 
            this.picOut2.Image = ((System.Drawing.Image)(resources.GetObject("picOut2.Image")));
            this.picOut2.Location = new System.Drawing.Point(203, 629);
            this.picOut2.Margin = new System.Windows.Forms.Padding(2);
            this.picOut2.Name = "picOut2";
            this.picOut2.Size = new System.Drawing.Size(15, 16);
            this.picOut2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut2.TabIndex = 155;
            this.picOut2.TabStop = false;
            // 
            // picOut1
            // 
            this.picOut1.Image = ((System.Drawing.Image)(resources.GetObject("picOut1.Image")));
            this.picOut1.Location = new System.Drawing.Point(170, 629);
            this.picOut1.Margin = new System.Windows.Forms.Padding(2);
            this.picOut1.Name = "picOut1";
            this.picOut1.Size = new System.Drawing.Size(15, 16);
            this.picOut1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut1.TabIndex = 154;
            this.picOut1.TabStop = false;
            // 
            // picOut0
            // 
            this.picOut0.Image = ((System.Drawing.Image)(resources.GetObject("picOut0.Image")));
            this.picOut0.Location = new System.Drawing.Point(137, 629);
            this.picOut0.Margin = new System.Windows.Forms.Padding(2);
            this.picOut0.Name = "picOut0";
            this.picOut0.Size = new System.Drawing.Size(15, 16);
            this.picOut0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOut0.TabIndex = 153;
            this.picOut0.TabStop = false;
            // 
            // picIn23
            // 
            this.picIn23.Image = ((System.Drawing.Image)(resources.GetObject("picIn23.Image")));
            this.picIn23.Location = new System.Drawing.Point(896, 604);
            this.picIn23.Margin = new System.Windows.Forms.Padding(2);
            this.picIn23.Name = "picIn23";
            this.picIn23.Size = new System.Drawing.Size(15, 16);
            this.picIn23.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn23.TabIndex = 150;
            this.picIn23.TabStop = false;
            // 
            // picIn22
            // 
            this.picIn22.Image = ((System.Drawing.Image)(resources.GetObject("picIn22.Image")));
            this.picIn22.Location = new System.Drawing.Point(863, 604);
            this.picIn22.Margin = new System.Windows.Forms.Padding(2);
            this.picIn22.Name = "picIn22";
            this.picIn22.Size = new System.Drawing.Size(15, 16);
            this.picIn22.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn22.TabIndex = 149;
            this.picIn22.TabStop = false;
            // 
            // picIn21
            // 
            this.picIn21.Image = ((System.Drawing.Image)(resources.GetObject("picIn21.Image")));
            this.picIn21.Location = new System.Drawing.Point(830, 604);
            this.picIn21.Margin = new System.Windows.Forms.Padding(2);
            this.picIn21.Name = "picIn21";
            this.picIn21.Size = new System.Drawing.Size(15, 16);
            this.picIn21.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn21.TabIndex = 148;
            this.picIn21.TabStop = false;
            // 
            // picIn20
            // 
            this.picIn20.Image = ((System.Drawing.Image)(resources.GetObject("picIn20.Image")));
            this.picIn20.Location = new System.Drawing.Point(797, 604);
            this.picIn20.Margin = new System.Windows.Forms.Padding(2);
            this.picIn20.Name = "picIn20";
            this.picIn20.Size = new System.Drawing.Size(15, 16);
            this.picIn20.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn20.TabIndex = 147;
            this.picIn20.TabStop = false;
            // 
            // picIn19
            // 
            this.picIn19.Image = ((System.Drawing.Image)(resources.GetObject("picIn19.Image")));
            this.picIn19.Location = new System.Drawing.Point(764, 604);
            this.picIn19.Margin = new System.Windows.Forms.Padding(2);
            this.picIn19.Name = "picIn19";
            this.picIn19.Size = new System.Drawing.Size(15, 16);
            this.picIn19.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn19.TabIndex = 146;
            this.picIn19.TabStop = false;
            // 
            // picIn18
            // 
            this.picIn18.Image = ((System.Drawing.Image)(resources.GetObject("picIn18.Image")));
            this.picIn18.Location = new System.Drawing.Point(731, 604);
            this.picIn18.Margin = new System.Windows.Forms.Padding(2);
            this.picIn18.Name = "picIn18";
            this.picIn18.Size = new System.Drawing.Size(15, 16);
            this.picIn18.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn18.TabIndex = 145;
            this.picIn18.TabStop = false;
            // 
            // picIn17
            // 
            this.picIn17.Image = ((System.Drawing.Image)(resources.GetObject("picIn17.Image")));
            this.picIn17.Location = new System.Drawing.Point(698, 604);
            this.picIn17.Margin = new System.Windows.Forms.Padding(2);
            this.picIn17.Name = "picIn17";
            this.picIn17.Size = new System.Drawing.Size(15, 16);
            this.picIn17.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn17.TabIndex = 144;
            this.picIn17.TabStop = false;
            // 
            // picIn16
            // 
            this.picIn16.Image = ((System.Drawing.Image)(resources.GetObject("picIn16.Image")));
            this.picIn16.Location = new System.Drawing.Point(665, 604);
            this.picIn16.Margin = new System.Windows.Forms.Padding(2);
            this.picIn16.Name = "picIn16";
            this.picIn16.Size = new System.Drawing.Size(15, 16);
            this.picIn16.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn16.TabIndex = 143;
            this.picIn16.TabStop = false;
            // 
            // picIn15
            // 
            this.picIn15.Image = ((System.Drawing.Image)(resources.GetObject("picIn15.Image")));
            this.picIn15.Location = new System.Drawing.Point(632, 604);
            this.picIn15.Margin = new System.Windows.Forms.Padding(2);
            this.picIn15.Name = "picIn15";
            this.picIn15.Size = new System.Drawing.Size(15, 16);
            this.picIn15.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn15.TabIndex = 142;
            this.picIn15.TabStop = false;
            // 
            // picIn14
            // 
            this.picIn14.Image = ((System.Drawing.Image)(resources.GetObject("picIn14.Image")));
            this.picIn14.Location = new System.Drawing.Point(599, 604);
            this.picIn14.Margin = new System.Windows.Forms.Padding(2);
            this.picIn14.Name = "picIn14";
            this.picIn14.Size = new System.Drawing.Size(15, 16);
            this.picIn14.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn14.TabIndex = 141;
            this.picIn14.TabStop = false;
            // 
            // picIn13
            // 
            this.picIn13.Image = ((System.Drawing.Image)(resources.GetObject("picIn13.Image")));
            this.picIn13.Location = new System.Drawing.Point(566, 604);
            this.picIn13.Margin = new System.Windows.Forms.Padding(2);
            this.picIn13.Name = "picIn13";
            this.picIn13.Size = new System.Drawing.Size(15, 16);
            this.picIn13.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn13.TabIndex = 140;
            this.picIn13.TabStop = false;
            // 
            // picIn12
            // 
            this.picIn12.Image = ((System.Drawing.Image)(resources.GetObject("picIn12.Image")));
            this.picIn12.Location = new System.Drawing.Point(533, 604);
            this.picIn12.Margin = new System.Windows.Forms.Padding(2);
            this.picIn12.Name = "picIn12";
            this.picIn12.Size = new System.Drawing.Size(15, 16);
            this.picIn12.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn12.TabIndex = 139;
            this.picIn12.TabStop = false;
            // 
            // picIn11
            // 
            this.picIn11.Image = ((System.Drawing.Image)(resources.GetObject("picIn11.Image")));
            this.picIn11.Location = new System.Drawing.Point(500, 604);
            this.picIn11.Margin = new System.Windows.Forms.Padding(2);
            this.picIn11.Name = "picIn11";
            this.picIn11.Size = new System.Drawing.Size(15, 16);
            this.picIn11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn11.TabIndex = 138;
            this.picIn11.TabStop = false;
            // 
            // picIn10
            // 
            this.picIn10.Image = ((System.Drawing.Image)(resources.GetObject("picIn10.Image")));
            this.picIn10.Location = new System.Drawing.Point(467, 604);
            this.picIn10.Margin = new System.Windows.Forms.Padding(2);
            this.picIn10.Name = "picIn10";
            this.picIn10.Size = new System.Drawing.Size(15, 16);
            this.picIn10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn10.TabIndex = 137;
            this.picIn10.TabStop = false;
            // 
            // picIn9
            // 
            this.picIn9.Image = ((System.Drawing.Image)(resources.GetObject("picIn9.Image")));
            this.picIn9.Location = new System.Drawing.Point(434, 604);
            this.picIn9.Margin = new System.Windows.Forms.Padding(2);
            this.picIn9.Name = "picIn9";
            this.picIn9.Size = new System.Drawing.Size(15, 16);
            this.picIn9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn9.TabIndex = 136;
            this.picIn9.TabStop = false;
            // 
            // picIn8
            // 
            this.picIn8.Image = ((System.Drawing.Image)(resources.GetObject("picIn8.Image")));
            this.picIn8.Location = new System.Drawing.Point(401, 604);
            this.picIn8.Margin = new System.Windows.Forms.Padding(2);
            this.picIn8.Name = "picIn8";
            this.picIn8.Size = new System.Drawing.Size(15, 16);
            this.picIn8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn8.TabIndex = 135;
            this.picIn8.TabStop = false;
            // 
            // picIn7
            // 
            this.picIn7.Image = ((System.Drawing.Image)(resources.GetObject("picIn7.Image")));
            this.picIn7.Location = new System.Drawing.Point(368, 604);
            this.picIn7.Margin = new System.Windows.Forms.Padding(2);
            this.picIn7.Name = "picIn7";
            this.picIn7.Size = new System.Drawing.Size(15, 16);
            this.picIn7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn7.TabIndex = 134;
            this.picIn7.TabStop = false;
            // 
            // picIn6
            // 
            this.picIn6.Image = ((System.Drawing.Image)(resources.GetObject("picIn6.Image")));
            this.picIn6.Location = new System.Drawing.Point(335, 604);
            this.picIn6.Margin = new System.Windows.Forms.Padding(2);
            this.picIn6.Name = "picIn6";
            this.picIn6.Size = new System.Drawing.Size(15, 16);
            this.picIn6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn6.TabIndex = 133;
            this.picIn6.TabStop = false;
            // 
            // picIn5
            // 
            this.picIn5.Image = ((System.Drawing.Image)(resources.GetObject("picIn5.Image")));
            this.picIn5.Location = new System.Drawing.Point(302, 604);
            this.picIn5.Margin = new System.Windows.Forms.Padding(2);
            this.picIn5.Name = "picIn5";
            this.picIn5.Size = new System.Drawing.Size(15, 16);
            this.picIn5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn5.TabIndex = 132;
            this.picIn5.TabStop = false;
            // 
            // picIn4
            // 
            this.picIn4.Image = ((System.Drawing.Image)(resources.GetObject("picIn4.Image")));
            this.picIn4.Location = new System.Drawing.Point(269, 604);
            this.picIn4.Margin = new System.Windows.Forms.Padding(2);
            this.picIn4.Name = "picIn4";
            this.picIn4.Size = new System.Drawing.Size(15, 16);
            this.picIn4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn4.TabIndex = 131;
            this.picIn4.TabStop = false;
            // 
            // picIn3
            // 
            this.picIn3.Image = ((System.Drawing.Image)(resources.GetObject("picIn3.Image")));
            this.picIn3.Location = new System.Drawing.Point(236, 604);
            this.picIn3.Margin = new System.Windows.Forms.Padding(2);
            this.picIn3.Name = "picIn3";
            this.picIn3.Size = new System.Drawing.Size(15, 16);
            this.picIn3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn3.TabIndex = 130;
            this.picIn3.TabStop = false;
            // 
            // picIn2
            // 
            this.picIn2.Image = ((System.Drawing.Image)(resources.GetObject("picIn2.Image")));
            this.picIn2.Location = new System.Drawing.Point(203, 604);
            this.picIn2.Margin = new System.Windows.Forms.Padding(2);
            this.picIn2.Name = "picIn2";
            this.picIn2.Size = new System.Drawing.Size(15, 16);
            this.picIn2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn2.TabIndex = 129;
            this.picIn2.TabStop = false;
            // 
            // picIn1
            // 
            this.picIn1.Image = ((System.Drawing.Image)(resources.GetObject("picIn1.Image")));
            this.picIn1.Location = new System.Drawing.Point(170, 604);
            this.picIn1.Margin = new System.Windows.Forms.Padding(2);
            this.picIn1.Name = "picIn1";
            this.picIn1.Size = new System.Drawing.Size(15, 16);
            this.picIn1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn1.TabIndex = 128;
            this.picIn1.TabStop = false;
            // 
            // picIn0
            // 
            this.picIn0.Image = ((System.Drawing.Image)(resources.GetObject("picIn0.Image")));
            this.picIn0.Location = new System.Drawing.Point(137, 604);
            this.picIn0.Margin = new System.Windows.Forms.Padding(2);
            this.picIn0.Name = "picIn0";
            this.picIn0.Size = new System.Drawing.Size(15, 16);
            this.picIn0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIn0.TabIndex = 127;
            this.picIn0.TabStop = false;
            // 
            // frmEpsonRemoteTCP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 674);
            this.Controls.Add(this.gpbSetting);
            this.Controls.Add(this.picChinese);
            this.Controls.Add(this.picEnglish);
            this.Controls.Add(this.picHelp);
            this.Controls.Add(this.picDynamicPicture);
            this.Controls.Add(this.gpbProgram);
            this.Controls.Add(this.picOut15);
            this.Controls.Add(this.picOut14);
            this.Controls.Add(this.picOut13);
            this.Controls.Add(this.picOut12);
            this.Controls.Add(this.lblDateAndTime);
            this.Controls.Add(this.picOut11);
            this.Controls.Add(this.picOut10);
            this.Controls.Add(this.picOut9);
            this.Controls.Add(this.picOut8);
            this.Controls.Add(this.picOut7);
            this.Controls.Add(this.picOut6);
            this.Controls.Add(this.picOut5);
            this.Controls.Add(this.picOut4);
            this.Controls.Add(this.picOut3);
            this.Controls.Add(this.picOut2);
            this.Controls.Add(this.picOut1);
            this.Controls.Add(this.picOut0);
            this.Controls.Add(this.lblOutputSignal);
            this.Controls.Add(this.lblInputSignal);
            this.Controls.Add(this.picIn23);
            this.Controls.Add(this.picIn22);
            this.Controls.Add(this.picIn21);
            this.Controls.Add(this.picIn20);
            this.Controls.Add(this.picIn19);
            this.Controls.Add(this.picIn18);
            this.Controls.Add(this.picIn17);
            this.Controls.Add(this.picIn16);
            this.Controls.Add(this.picIn15);
            this.Controls.Add(this.picIn14);
            this.Controls.Add(this.picIn13);
            this.Controls.Add(this.picIn12);
            this.Controls.Add(this.picIn11);
            this.Controls.Add(this.picIn10);
            this.Controls.Add(this.picIn9);
            this.Controls.Add(this.picIn8);
            this.Controls.Add(this.picIn7);
            this.Controls.Add(this.picIn6);
            this.Controls.Add(this.picIn5);
            this.Controls.Add(this.picIn4);
            this.Controls.Add(this.picIn3);
            this.Controls.Add(this.picIn2);
            this.Controls.Add(this.picIn1);
            this.Controls.Add(this.picIn0);
            this.Controls.Add(this.gpbOperation);
            this.Controls.Add(this.gpbPointData);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.txtExecute);
            this.Controls.Add(this.rtbHistory);
            this.Controls.Add(this.StatusStrip1);
            this.Controls.Add(this.lblReminder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmEpsonRemoteTCP";
            this.Text = "EPSON Remote Control Via TCP/IP     软件作者：彭东南";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEpsonRemoteTCP_FormClosing);
            this.Load += new System.EventHandler(this.frmEpsonRemoteTCP_Load);
            this.gpbSetting.ResumeLayout(false);
            this.gpbSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDynamicPicture)).EndInit();
            this.gpbOperation.ResumeLayout(false);
            this.gpbPointData.ResumeLayout(false);
            this.gpbPointData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPointData)).EndInit();
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.gpbProgram.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picChinese)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEnglish)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOut0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIn0)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion

        internal System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtPassword;
        internal System.Windows.Forms.Button btnConnectRobot;
        internal System.Windows.Forms.ToolTip ToolTip1;
        internal System.Windows.Forms.RadioButton rbtnLeftHand;
        internal System.Windows.Forms.Timer ScanningTimer;
        internal System.Windows.Forms.Button btnLogout;
        internal System.Windows.Forms.RadioButton rbtnRightHand;
        internal System.Windows.Forms.GroupBox gpbSetting;
        internal System.Windows.Forms.Button btnTeach;
        private System.Windows.Forms.Label lblStepDistance;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.Label lblIPAddress;
        internal System.Windows.Forms.TextBox txtRemoteEpsonIPAddress;
        private System.Windows.Forms.Label lblPoint;
        private System.Windows.Forms.TextBox txtJogDistance;
        private System.Windows.Forms.Button btnJump;
        private System.Windows.Forms.Label lblPassword;
        internal System.Windows.Forms.Button btnSetSpeed;
        internal System.Windows.Forms.Button btnSetACCELSpeed;
        internal System.Windows.Forms.ComboBox cmbPointName;
        private System.Windows.Forms.Label lblAccelSpeed;
        private System.Windows.Forms.TextBox txtAccelSpeed;
        private System.Windows.Forms.TextBox txtDecelSpeed;
        internal System.Windows.Forms.PictureBox picChinese;
        internal System.Windows.Forms.ToolStripStatusLabel lblPowerStatus;
        internal System.Windows.Forms.PictureBox picEnglish;
        internal System.Windows.Forms.PictureBox picHelp;
        internal System.Windows.Forms.PictureBox picDynamicPicture;
        internal System.Windows.Forms.PictureBox picOut15;
        internal System.Windows.Forms.PictureBox picOut14;
        internal System.Windows.Forms.PictureBox picOut13;
        internal System.Windows.Forms.PictureBox picOut12;
        internal System.Windows.Forms.ToolStripStatusLabel lblXPos;
        internal System.Windows.Forms.ToolStripStatusLabel lblHand;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel1;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel2;
        internal System.Windows.Forms.ToolStripStatusLabel lblReady;
        internal System.Windows.Forms.ToolStripStatusLabel lblPaused;
        internal System.Windows.Forms.ToolStripStatusLabel lblRunning;
        internal System.Windows.Forms.ToolStripStatusLabel lblYPos;
        private System.Windows.Forms.Label lblDateAndTime;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel3;
        internal System.Windows.Forms.ToolStripStatusLabel lblWPos;
        internal System.Windows.Forms.ToolStripStatusLabel lblZPos;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel6;
        internal System.Windows.Forms.ToolStripStatusLabel lblHandStyle;
        internal System.Windows.Forms.ToolStripStatusLabel lblVPos;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel4;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel5;
        internal System.Windows.Forms.PictureBox picOut11;
        internal System.Windows.Forms.ToolStripStatusLabel lblUPos;
        internal System.Windows.Forms.PictureBox picOut10;
        internal System.Windows.Forms.PictureBox picOut9;
        internal System.Windows.Forms.PictureBox picOut8;
        internal System.Windows.Forms.PictureBox picOut7;
        internal System.Windows.Forms.PictureBox picOut6;
        internal System.Windows.Forms.ToolStripStatusLabel lblError;
        internal System.Windows.Forms.PictureBox picOut5;
        internal System.Windows.Forms.PictureBox picOut4;
        internal System.Windows.Forms.PictureBox picOut3;
        internal System.Windows.Forms.PictureBox picOut2;
        internal System.Windows.Forms.PictureBox picOut1;
        internal System.Windows.Forms.PictureBox picOut0;
        internal System.Windows.Forms.Label lblOutputSignal;
        internal System.Windows.Forms.Label lblInputSignal;
        internal System.Windows.Forms.PictureBox picIn23;
        internal System.Windows.Forms.PictureBox picIn22;
        internal System.Windows.Forms.PictureBox picIn21;
        internal System.Windows.Forms.PictureBox picIn20;
        internal System.Windows.Forms.PictureBox picIn19;
        internal System.Windows.Forms.PictureBox picIn18;
        internal System.Windows.Forms.PictureBox picIn17;
        internal System.Windows.Forms.PictureBox picIn16;
        internal System.Windows.Forms.PictureBox picIn15;
        internal System.Windows.Forms.PictureBox picIn14;
        internal System.Windows.Forms.PictureBox picIn13;
        internal System.Windows.Forms.PictureBox picIn12;
        internal System.Windows.Forms.PictureBox picIn11;
        internal System.Windows.Forms.PictureBox picIn10;
        internal System.Windows.Forms.PictureBox picIn9;
        internal System.Windows.Forms.PictureBox picIn8;
        internal System.Windows.Forms.PictureBox picIn7;
        internal System.Windows.Forms.PictureBox picIn6;
        internal System.Windows.Forms.PictureBox picIn5;
        internal System.Windows.Forms.PictureBox picIn4;
        internal System.Windows.Forms.PictureBox picIn3;
        internal System.Windows.Forms.PictureBox picIn2;
        internal System.Windows.Forms.PictureBox picIn1;
        internal System.Windows.Forms.PictureBox picIn0;
        internal System.Windows.Forms.ToolStripStatusLabel lblEStop;
        internal System.Windows.Forms.ComboBox cmbBrakeOffJoint;
        internal System.Windows.Forms.ToolStripStatusLabel lblSafeguard;
        internal System.Windows.Forms.ComboBox cmbBrakeOnJoint;
        internal System.Windows.Forms.ComboBox cmbSFreeJoint;
        internal System.Windows.Forms.ComboBox cmbSLockJoint;
        internal System.Windows.Forms.Button btnPowerHigh;
        internal System.Windows.Forms.Button btnAllBrakeOff;
        internal System.Windows.Forms.Button btnMotorOff;
        internal System.Windows.Forms.Button btnMotorOn;
        internal System.Windows.Forms.Button btnBrakeOff;
        internal System.Windows.Forms.Button btnAllBrakeOn;
        internal System.Windows.Forms.Button btnWJogNegative;
        internal System.Windows.Forms.Button btnSFreeAll;
        internal System.Windows.Forms.Button btnVJogPositive;
        internal System.Windows.Forms.Button btnURotateNegative;
        internal System.Windows.Forms.Button btnWJogPositive;
        internal System.Windows.Forms.GroupBox gpbOperation;
        internal System.Windows.Forms.Button btnBrakeOn;
        internal System.Windows.Forms.Button btnVJogNegative;
        internal System.Windows.Forms.Button btnSFree;
        internal System.Windows.Forms.Button btnURotatePositive;
        internal System.Windows.Forms.Button btnZDownward;
        internal System.Windows.Forms.Button btnSLockAll;
        internal System.Windows.Forms.Button btnZUpward;
        internal System.Windows.Forms.Button btnYJogNegative;
        internal System.Windows.Forms.Button btnSLock;
        internal System.Windows.Forms.Button btnYJogPositive;
        internal System.Windows.Forms.Button btnXJogNegative;
        internal System.Windows.Forms.Button btnXJogPositive;
        internal System.Windows.Forms.Button btnPowerLow;
        internal System.Windows.Forms.Button btnExportPointDataToExcel;
        internal System.Windows.Forms.DataGridViewTextBoxColumn Hand;
        private System.Windows.Forms.TextBox txtPointQty;
        private System.Windows.Forms.Label lblPointQty;
        internal System.Windows.Forms.GroupBox gpbPointData;
        internal System.Windows.Forms.Button btnSavePoints;
        internal System.Windows.Forms.Button btnLoadPoints;
        internal System.Windows.Forms.DataGridView dgvPointData;
        internal System.Windows.Forms.DataGridViewTextBoxColumn PointName;
        internal System.Windows.Forms.DataGridViewTextBoxColumn X;
        internal System.Windows.Forms.DataGridViewTextBoxColumn Y;
        internal System.Windows.Forms.DataGridViewTextBoxColumn Z;
        internal System.Windows.Forms.DataGridViewTextBoxColumn U;
        internal System.Windows.Forms.DataGridViewTextBoxColumn V;
        internal System.Windows.Forms.DataGridViewTextBoxColumn W;
        internal System.Windows.Forms.Button btnExecute;
        protected internal System.Windows.Forms.TextBox txtExecute;
        internal System.Windows.Forms.RichTextBox rtbHistory;
        internal System.Windows.Forms.Button btnAbort;
        internal System.Windows.Forms.StatusStrip StatusStrip1;
        internal System.Windows.Forms.ToolStripStatusLabel lblRobot;
        internal System.Windows.Forms.ToolStripStatusLabel lblTest;
        internal System.Windows.Forms.ToolStripStatusLabel lblTeach;
        internal System.Windows.Forms.ToolStripStatusLabel lblAuto;
        internal System.Windows.Forms.ToolStripStatusLabel lblWarning;
        internal System.Windows.Forms.ToolStripStatusLabel lblSError;
        internal System.Windows.Forms.Button btnResetRobot;
        internal System.Windows.Forms.Button btnStop;
        internal System.Windows.Forms.ComboBox cmbMainFunctionNo;
        internal System.Windows.Forms.Button btnPauseContinue;
        internal System.Windows.Forms.GroupBox gpbProgram;
        internal System.Windows.Forms.Label lblReminder;
        internal System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox txtRobotRemotePort;
        }
    }