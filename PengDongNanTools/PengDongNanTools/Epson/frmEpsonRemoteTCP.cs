#region "using"

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.VisualBasic;
using System.Timers;

#endregion

namespace PengDongNanTools
    {

    //EPSON机械手远程以太网操作界面【软件作者：彭东南, southeastofstar@163.com】
    /// <summary>
    /// EPSON机械手远程以太网操作界面【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    public partial class frmEpsonRemoteTCP : Form
        {
        
        #region "变量定义"

        frmLogin Login = new frmLogin("EpsonRemoteTCP");
        frmAbout About = new frmAbout();

        private SaveFileDialog SaveFile = new SaveFileDialog();

        private int DynamicPicture = 0;

        /// <summary>
        /// 关闭窗体时是否只是隐藏，默认是
        /// </summary>
        public bool HideWhenClose = true;

        /// <summary>
        /// 关闭窗体时是否弹出确认对话框，默认是
        /// </summary>
        public bool ShowPromptWhenClose = true;

        private bool SingleInputBitStatus=false;

        PictureBox TempPictureBox;
        int TempOutputBit = 0;

        string RemoteRobotLoginPassword = "", RobotStatusCode="";

        EpsonRemoteTCP.EpsonStatusBits RCStatusBits=new EpsonRemoteTCP.EpsonStatusBits();
        EpsonRemoteTCP.Power RCPower = new EpsonRemoteTCP.Power();
        
        /// <summary>
        /// Excel文件类型
        /// </summary>
        public enum ExcelFileType 
            {
            /// <summary>
            /// xls格式Excel文件
            /// </summary>
            xls,

            /// <summary>
            /// xlsx格式Excel文件
            /// </summary>
            xlsx,

            /// <summary>
            /// csv格式Excel文件
            /// </summary>
            csv            
            }
        
        private Thread ThreadUpdateRobotStatus;

        private bool LoadedPointData = false;

        /// <summary>
        /// 是否更新显示错误信息
        /// </summary>
        public bool ShowErrorMessage = false;

        /// <summary>
        /// 界面是否用中文显示
        /// </summary>
        public bool InterfaceInChinese = false;

        /// <summary>
        /// 用户登录代码
        /// </summary>
        private int LoggedCode = 10;

        private EpsonRemoteTCP.EpsonPoint RCPoint = new EpsonRemoteTCP.EpsonPoint();

        private EpsonRemoteTCP RC;

        private string ErrorMessage="";

        /// <summary>
        /// 当错误信息相同时,是否重复显示
        /// </summary>
        public bool UpdatingSameMessage=true;

        private PictureBox[] InputSignalPictureBoxArray,OutputSignalPictureBoxArray;

        private bool LoggedSuccess = false;

        private ushort OutputWordStatus=0;
        private bool[] OutputBitStatus=new bool[16];

        private bool SuccessBuiltNew = false, PasswordIsCorrect = false;

        ListViewOperation LV = new ListViewOperation("彭东南");
        Microsoft.VisualBasic.Devices.Computer PC = new Microsoft.VisualBasic.Devices.Computer();
        CommonFunction FC = new CommonFunction("彭东南");

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }

        #endregion

        #region "函数代码"

        //EPSON机械手远程以太网操作界面类的实例
        /// <summary>
        /// EPSON机械手远程以太网操作界面类的实例
        /// </summary>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public frmEpsonRemoteTCP(string DLLPassword)
            {
            
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;

            try
                {

                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan") || (DLLPassword == "彭东南"))
                    {

                    PasswordIsCorrect = true;

                    InitializeComponent();

                    SuccessBuiltNew = true;

                    }
                else
                    {
                    PasswordIsCorrect = false;
                    SuccessBuiltNew = false;
                    MessageBox.Show("Right Prohibited.\return\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                    }

                }
            catch (Exception ex)
                {
                SuccessBuiltNew = false;
                MessageBox.Show("创建类的实例时出现错误！\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }    

            }

        /// <summary>
        /// 进行界面中文和英文切换
        /// </summary>
        private void ChangeLanguageForInterface() 
            {

            try
                {

                if (InterfaceInChinese == false)
                    {
                    gpbPointData.Text = "Point Data";
                    gpbOperation.Text = "Operation";
                    gpbProgram.Text = "Program";
                    gpbSetting.Text = "Setting";

                    lblSpeed.Text = "Speed";
                    lblStepDistance.Text = "mm";

                    lblRobot.Text = "Robot";
                    lblSError.Text = "SError";
                    lblTest.Text = "Test";
                    lblTeach.Text = "Teach";
                    lblWarning.Text = "Warning";
                    lblAuto.Text = "Auto";
                    lblSafeguard.Text = "Safeguard";
                    lblEStop.Text = "EStop";
                    lblError.Text = "Error";
                    lblPaused.Text = "Paused";
                    lblRunning.Text = "Running";
                    lblReady.Text = "Ready";
                    lblPowerStatus.Text = "Power Status";
                    lblHandStyle.Text = "Hand：";
                    lblAccelSpeed.Text = "Accel\r\n" + "Speed:";
                    lblPassword.Text = "Password:";
                    lblIPAddress.Text = "Robot IP:";
                    lblPoint.Text = "Point:";
                    lblPointQty.Text = "Point Q'ty:";

                    if (RC != null) 
                        {
                        if (RC.RobotPaused == true)
                            {
                            btnPauseContinue.Text = "Continue";
                            }
                        else 
                            {
                            btnPauseContinue.Text = "Pause";
                            }
                        }

                    btnWJogNegative.Text = "W-";
                    btnWJogPositive.Text = "W+";
                    btnVJogNegative.Text = "V-";
                    btnVJogPositive.Text = "V+";
                    btnURotateNegative.Text = "U-";
                    btnURotatePositive.Text = "U+";
                    btnZDownward.Text = "Z-";
                    btnZUpward.Text = "Z+";
                    btnYJogNegative.Text = "Y-";
                    btnYJogPositive.Text = "Y+";
                    btnXJogNegative.Text = "X-";
                    btnXJogPositive.Text = "X+";

                    btnRun.Text = "Run";
                    btnJump.Text = "Jump";
                    btnAbort.Text = "Abort";
                    btnMotorOff.Text = "Motor Off";
                    btnMotorOn.Text = "Motor On";
                    btnResetRobot.Text = "Reset";
                    btnStop.Text = "Stop";
                    btnExecute.Text = "Execute";
                    btnPowerHigh.Text = "Power High";
                    btnSetACCELSpeed.Text = "Set";
                    btnAllBrakeOff.Text = "AllBrakeOff";
                    btnBrakeOff.Text = "Brake Off";
                    btnAllBrakeOn.Text = "AllBrakeOn";
                    btnBrakeOn.Text = "Brake On";
                    btnSFreeAll.Text = "SFreeAll";
                    btnSFree.Text = "SFree";
                    btnSLockAll.Text = "SLockAll";
                    btnSLock.Text = "SLock";
                    btnSetSpeed.Text = "Set";
                    btnPowerLow.Text = "Power Low";
                    btnTeach.Text = "Teach";
                    btnSavePoints.Text = "Save Points";
                    btnLoadPoints.Text = "Load Points";
                    btnLogout.Text = "Logout";
                    btnLogin.Text = "Login";

                    rbtnLeftHand.Text = "Left Hand";
                    rbtnRightHand.Text = "Right Hand";

                    dgvPointData.Columns[0].HeaderText = "Point Name";
                    dgvPointData.Columns[1].HeaderText = "X";
                    dgvPointData.Columns[2].HeaderText = "Y";
                    dgvPointData.Columns[3].HeaderText = "Z";
                    dgvPointData.Columns[4].HeaderText = "U";
                    dgvPointData.Columns[5].HeaderText = "V";
                    dgvPointData.Columns[6].HeaderText = "W";
                    dgvPointData.Columns[7].HeaderText = "Hand Style";

                    btnConnectRobot.Text = "Connect";

                    lblInputSignal.Text = "Input Signal:";
                    lblOutputSignal.Text = "Output Signal:";

                    }
                else 
                    {

                    lblInputSignal.Text = "输入信号:";
                    lblOutputSignal.Text = "输出信号:";

                    btnConnectRobot.Text = "连接";

                    gpbPointData.Text = "点数据";
                    gpbOperation.Text = "操作";
                    gpbProgram.Text = "程序";
                    gpbSetting.Text = "设置";

                    lblSpeed.Text = "速度";
                    lblStepDistance.Text = "毫米";

                    lblRobot.Text = "机器人";
                    lblSError.Text = "严重错误";
                    lblTest.Text = "测试";
                    lblTeach.Text = "示教";
                    lblWarning.Text = "警告";
                    lblAuto.Text = "自动";
                    lblSafeguard.Text = "安全门";
                    lblEStop.Text = "急停";
                    lblError.Text = "错误";
                    lblPaused.Text = "暂停中";
                    lblRunning.Text = "运行";
                    lblReady.Text = "准备好";
                    lblPowerStatus.Text = "功率状态";
                    lblHandStyle.Text = "手势";
                    lblAccelSpeed.Text = "加速度:";
                    lblPassword.Text = "密码:";
                    lblIPAddress.Text = "机器人IP:";
                    lblPoint.Text = "点:";
                    lblPointQty.Text = "点数量:";

                    if (RC != null)
                        {
                        if (RC.RobotPaused == true)
                            {
                            btnPauseContinue.Text = "继续";
                            }
                        else
                            {
                            btnPauseContinue.Text = "暂停";
                            }
                        }

                    btnWJogNegative.Text = "W反转";
                    btnWJogPositive.Text = "W正转";
                    btnVJogNegative.Text = "V反转";
                    btnVJogPositive.Text = "V正转";
                    btnURotateNegative.Text = "U反转";
                    btnURotatePositive.Text = "U正转";
                    btnZDownward.Text = "Z下降";
                    btnZUpward.Text = "Z上升";
                    btnYJogNegative.Text = "Y反转";
                    btnYJogPositive.Text = "Y正转";
                    btnXJogNegative.Text = "X反转";
                    btnXJogPositive.Text = "X正转";

                    btnRun.Text = "运行";
                    btnJump.Text = "执行";
                    btnAbort.Text = "终止";
                    btnMotorOff.Text = "关电机";
                    btnMotorOn.Text = "开电机";
                    btnResetRobot.Text = "复位";
                    btnStop.Text = "停止";
                    btnExecute.Text = "执行";
                    btnPowerHigh.Text = "高功率";
                    btnSetACCELSpeed.Text = "设置";
                    btnAllBrakeOff.Text = "全部刹车关";
                    btnBrakeOff.Text = "关刹车";
                    btnAllBrakeOn.Text = "全部刹车开";
                    btnBrakeOn.Text = "开刹车";
                    btnSFreeAll.Text = "释放全部";
                    btnSFree.Text = "释放";
                    btnSLockAll.Text = "锁定全部";
                    btnSLock.Text = "锁定";
                    btnSetSpeed.Text = "设置";
                    btnPowerLow.Text = "低功率";
                    btnTeach.Text = "示教";
                    btnSavePoints.Text = "保存点位";
                    btnLoadPoints.Text = "导入点位";
                    btnLogout.Text = "退出";
                    btnLogin.Text = "登录";

                    rbtnLeftHand.Text = "左手势";
                    rbtnRightHand.Text = "右手势";

                    dgvPointData.Columns[0].HeaderText = "点名称";
                    dgvPointData.Columns[1].HeaderText = "X坐标";
                    dgvPointData.Columns[2].HeaderText = "Y坐标";
                    dgvPointData.Columns[3].HeaderText = "Z坐标";
                    dgvPointData.Columns[4].HeaderText = "U坐标";
                    dgvPointData.Columns[5].HeaderText = "V坐标";
                    dgvPointData.Columns[6].HeaderText = "W坐标";
                    dgvPointData.Columns[7].HeaderText = "手势";

                    }

                }
            catch (Exception ex) 
                {
                MessageBox.Show(ex.Message);
                }
            
            }

        //添加输入信号更新显示的图片控件数组
        /// <summary>
        /// 添加输入信号更新显示的图片控件数组
        /// </summary>
        private void AddInputSignalPictureBoxArray() 
            {

            try
                {

                InputSignalPictureBoxArray = new System.Windows.Forms.PictureBox[] { picIn0, 
                picIn1, picIn2, picIn3, picIn4, picIn5, picIn6, picIn7, picIn8, picIn9
                , picIn10, picIn11, picIn12, picIn13, picIn14, picIn15, picIn16, picIn17,
                picIn18, picIn19, picIn20, picIn21, picIn22, picIn23};

                for (int a = 0; a < InputSignalPictureBoxArray.Length; a++)
                    {
                    InputSignalPictureBoxArray[a].Tag = a;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }
            
            }

        //添加输出信号更新显示的图片控件数组
        /// <summary>
        /// 添加输出信号更新显示的图片控件数组
        /// </summary>
        private void AddOutputSignalPictureBoxArray() 
            {

            try
                {

                OutputSignalPictureBoxArray = new PictureBox[] { picOut0, picOut1, picOut2
                , picOut3, picOut4, picOut5, picOut6, picOut7, picOut8, picOut9, picOut10
                , picOut11, picOut12, picOut13, picOut14, picOut15};

                for (int a = 0; a < OutputSignalPictureBoxArray.Length; a++) 
                    {
                    OutputSignalPictureBoxArray[a].Tag = a;
                    OutputSignalPictureBoxArray[a].Click += new EventHandler(OutPutSignalPictureBoxArray_Click);
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }
            
            }

        //图片控件数组单击事件
        /// <summary>
        /// 图片控件数组单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutPutSignalPictureBoxArray_Click(object sender, EventArgs e) 
            {
            
            try
                {

                TempPictureBox = (PictureBox)sender;
                TempOutputBit=Convert.ToUInt16(TempPictureBox.Tag);

                if (OutputBitStatus[TempOutputBit] == false)
                    {
                    if (RC.SetOutputBit(TempOutputBit, true) == false)
                        {
                        FC.AddRichText(ref rtbHistory,"Failed to set output bit " + (TempOutputBit + 1) + " ON...");
                        MessageBox.Show("Failed to set output bit " + (TempOutputBit + 1) + " ON...");
                        }
                    else
                        {
                        OutputBitStatus[TempOutputBit] = true;
                        OutputSignalPictureBoxArray[TempOutputBit].Image = global::PengDongNanTools.Properties.Resources.GreenOn;
                        }
                    }
                else 
                    {
                    if (RC.SetOutputBit(TempOutputBit, false) == false)
                        {
                        FC.AddRichText(ref rtbHistory, "Failed to set output bit " + (TempOutputBit + 1) + " OFF...");
                        MessageBox.Show("Failed to set output bit " + (TempOutputBit + 1) + " OFF...");
                        }
                    else
                        {
                        OutputBitStatus[TempOutputBit] = false;
                        OutputSignalPictureBoxArray[TempOutputBit].Image = global::PengDongNanTools.Properties.Resources.GreenOff;
                        }
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        /// <summary>
        /// 函数：跨线程安全更新EPSON机械手状态到界面
        /// </summary>
        private void UpdateRobotStatusWithSafeThread()
            {

            while (true)
                {

                try
                    {

                    if (LoggedSuccess == true & (LoggedCode > 0 & LoggedCode < 10))
                        {
                        FC.ChangeButtonEnableStatus(ref btnLoadPoints, true);
                        FC.ChangeButtonEnableStatus(ref btnSavePoints, true);
                        }
                    else
                        {
                        FC.ChangeButtonEnableStatus(ref btnLoadPoints, false);
                        FC.ChangeButtonEnableStatus(ref btnSavePoints, false);
                        }

                    if (RC != null)
                        {

                        if (RC.ExecuteBusy == false)
                            {
                            if (RC.ConnectedWithRemoteEpson == true)
                                {

                                if (LoggedSuccess == false)
                                    {
                                    if (RC.Login(RemoteRobotLoginPassword) == true)
                                        {
                                        LoggedSuccess = true;
                                        }
                                    else
                                        {
                                        LoggedSuccess = false;
                                        }
                                    }

                                if (RC.ConnectedWithRemoteEpson == true)
                                    {
                                    FC.ChangeStatusLabelBackColor(ref btnAbort,
                                        ref lblRobot, Color.Green);
                                    }
                                else
                                    {
                                    FC.ChangeStatusLabelBackColor(ref btnAbort,
                                        ref lblRobot, Color.Red);
                                    }

                                //Test/Teach/Auto/Warning/SError/Safeguard/EStop/Error/Paused/Running/Ready 1 为开/0 为关
                                if (RC.GetRobotStatus(ref RobotStatusCode) == true)
                                    {
                                    RCStatusBits = RC.NewProcessStatusCode(RobotStatusCode);

                                    //0 - Test            在TEST模式下打开
                                    if (RCStatusBits.Test == true)
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblTest, Color.Green);
                                        }
                                    else
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblTest, Color.Red);
                                        }

                                    //1 - Teach           在TEACH模式下打开
                                    if (RCStatusBits.Teach == true)
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblTeach, Color.Green);
                                        }
                                    else
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblTeach, Color.Red);
                                        }

                                    //2 - Auto            在远程输入接受条件下打开
                                    if (RCStatusBits.Auto == true)
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblAuto, Color.Green);
                                        }
                                    else
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblAuto, Color.Red);
                                        }

                                    //3 - Warnig   在警告条件下打开,甚至在警告条件下也可以像往常一样执行任务。但是,应尽快采取警告行动。
                                    if (RCStatusBits.Warning == true)
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblWarning, Color.Red);
                                        }
                                    else
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblWarning, Color.Green);
                                        }

                                    //4 - SError   在严重错误状态下打开,发生严重错误时,重新启动控制器,以便从错误状态中恢复。“Reset 输入”不可用。
                                    if (RCStatusBits.SError == true)
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblSError, Color.Red);
                                        }
                                    else
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblSError, Color.Green);
                                        }

                                    //5 - Safeguard       安全门打开时打开
                                    if (RCStatusBits.Safeguard == true)
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblSafeguard, Color.Red);
                                        }
                                    else
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblSafeguard, Color.Green);
                                        }

                                    //6 - EStop           在紧急状态下打开
                                    if (RCStatusBits.EStop == true)
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblEStop, Color.Red);
                                        }
                                    else
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblEStop, Color.Green);
                                        }

                                    //7 - Error           在错误状态下打开,使用“Reset 输入”从错误状态中恢复。
                                    if (RCStatusBits.Error == true)
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblError, Color.Red);
                                        }
                                    else
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblError, Color.Green);
                                        }

                                    //8 - Paused          打开暂停的任务
                                    if (RCStatusBits.Paused == true)
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblPaused, Color.Red);
                                        }
                                    else
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblPaused, Color.Green);
                                        }

                                    //9 - Running         执行任务时打开,在“Paused 输出”为开时关闭。
                                    if (RCStatusBits.Running == true)
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblRunning, Color.Green);
                                        }
                                    else
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblRunning, Color.Red);
                                        }

                                    //10 - Ready           控制器完成启动且无任务执行时打开
                                    if (RCStatusBits.Ready == true)
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblReady, Color.Green);
                                        }
                                    else
                                        {
                                        FC.ChangeStatusLabelBackColor(ref btnAbort, ref lblReady, Color.Red);
                                        }

                                    }

                                if (RC.GetCurrentPos(ref RCPoint) == true)
                                    {
                                    FC.ChangeStatusLabelText(ref btnAbort, ref lblXPos, RCPoint.X.ToString());
                                    FC.ChangeStatusLabelText(ref btnAbort, ref lblYPos, RCPoint.Y.ToString());
                                    FC.ChangeStatusLabelText(ref btnAbort, ref lblZPos, RCPoint.Z.ToString());
                                    FC.ChangeStatusLabelText(ref btnAbort, ref lblUPos, RCPoint.U.ToString());
                                    FC.ChangeStatusLabelText(ref btnAbort, ref lblVPos, RCPoint.V.ToString());
                                    FC.ChangeStatusLabelText(ref btnAbort, ref lblWPos, RCPoint.W.ToString());

                                    if (InterfaceInChinese == true)
                                        {
                                        FC.ChangeStatusLabelText(ref btnAbort, ref lblWPos,
                                            (RCPoint.HandStyle == EpsonRemoteTCP.Hand.LeftHand) ? "左手势" : "右手势");
                                        }
                                    else
                                        {
                                        FC.ChangeStatusLabelText(ref btnAbort, ref lblWPos,
                                            (RCPoint.HandStyle == EpsonRemoteTCP.Hand.LeftHand) ? "Lefty" : "Righty");
                                        }

                                    }
                                RCPower = RC.GetPowerStatus();
                                if (RCPower == EpsonRemoteTCP.Power.Low)
                                    {
                                    if (InterfaceInChinese == true)
                                        {
                                        FC.ChangeStatusLabelText(ref btnAbort, ref lblWPos, "低功率");
                                        }
                                    else
                                        {
                                        FC.ChangeStatusLabelText(ref btnAbort, ref lblWPos, "Power Low");
                                        }
                                    }
                                else if (RCPower == EpsonRemoteTCP.Power.High)
                                    {
                                    FC.ChangeStatusLabelText(ref btnAbort, ref lblWPos, "高功率");
                                    }
                                else
                                    {
                                    FC.ChangeStatusLabelText(ref btnAbort, ref lblWPos, "Power High");
                                    }
                                }

                            for (int a = 0; a < 24; a++)
                                {
                                if (RC.GetInputBit(a, ref SingleInputBitStatus) == true)
                                    {
                                    if (SingleInputBitStatus == true)
                                        {
                                        InputSignalPictureBoxArray[a].Image = global::PengDongNanTools.Properties.Resources.GreenOn;
                                        }
                                    else
                                        {
                                        InputSignalPictureBoxArray[a].Image = global::PengDongNanTools.Properties.Resources.GreenOff;
                                        }
                                    }
                                }
                            }
                        else
                            {
                            LoggedSuccess = false;
                            }

                        }

                    }
                catch (Exception)// ex)
                    {
                    //MessageBox.Show(ex.Message);
                    }

                }

            }

        //利用在字符串中用 Tab 键作为分隔符，写入EXCEL后，
        //EXCEL自动以用 Tab 键作为分隔符进行跳列，以回车换行符进行换行
        //快速写数据到Excel文件【写单行数据】
        /// <summary>
        /// 快速写数据到Excel文件【写单行数据】
        /// </summary>
        /// <param name="ExcelFileName">保存的Excel文件名称</param>
        /// <param name="FileType">Excel文件保存类型</param>
        /// <param name="DataToBeSaved">需要保存的数据字符串数组</param>
        /// <returns></returns>
        private bool QuickExportDataToExcelSingleRow(string ExcelFileName, 
            ExcelFileType FileType, string[] DataToBeSaved) 
            {

            bool TempJudgement=false;

            try
                {

                string StrRecordsToBeSavedExcel = "", TempExcelFileName="";

                if (ExcelFileName == "") 
                    {
                    MessageBox.Show("The parameter 'ExcelFileName' can't be empty, please input the correct file name.");
                    return false;
                    }

                if (PC.FileSystem.FileExists(ExcelFileName) == false) 
                    {
                    if (MessageBox.Show("The file '" + ExcelFileName + "' is not exist, do you want to create it?" +
                        "文件 '" + ExcelFileName + "' 不存在，你需要新建一个吗？", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                        return false;
                        }
                    //else 
                    //    {
                          //后续可以考虑弹出保存对话框
                    //    }
                    }

                switch(FileType)
                    {
                    case ExcelFileType.csv:
                        TempExcelFileName = ExcelFileName + ".csv";
                        break;

                    case ExcelFileType.xls:
                        TempExcelFileName = ExcelFileName + ".xls";
                        break;

                    case ExcelFileType.xlsx:
                        TempExcelFileName = ExcelFileName + ".xlsx";
                        break;                    
                    }

                for (int a = 0; a < DataToBeSaved.Length; a++) 
                    {
                    StrRecordsToBeSavedExcel = DataToBeSaved[a] + Strings.Chr(9);
                    }

                StrRecordsToBeSavedExcel = StrRecordsToBeSavedExcel + "\r\n";
                
                Microsoft.VisualBasic.FileSystem.FileOpen(1, TempExcelFileName, OpenMode.Append);
                TempJudgement = true;//用于判断是否发生异常，如果成功打开文件后操作发生错误，然后在异常处理里面关闭文件
                Microsoft.VisualBasic.FileSystem.Print(1, StrRecordsToBeSavedExcel);
                Microsoft.VisualBasic.FileSystem.FileClose(1);

                return true;

                }
            catch (Exception ex) 
                {
                //如果成功打开文件后的操作发生错误，然后在异常处理里面关闭文件
                if (TempJudgement==true) 
                    {
                    Microsoft.VisualBasic.FileSystem.FileClose(1);
                    }
                MessageBox.Show(ex.Message);
                return false;
                }
            
            }

        //快速写数据到Excel文件【写多行数据，每列数据之间用TAB符号隔开，以回车换行符结束】
        /// <summary>
        /// 快速写数据到Excel文件【写多行数据，每列数据之间用TAB符号隔开，以回车换行符结束】
        /// </summary>
        /// <param name="ExcelFileName">保存的Excel文件名称</param>
        /// <param name="FileType">Excel文件保存类型</param>
        /// <param name="DataToBeSaved">需要保存的数据字符串数组，每个下标字符串由每列数据之间用TAB符号隔开，以回车换行符结束</param>
        /// <returns></returns>
        private bool QuickExportDataToExcelMultiRows(string ExcelFileName,
            ExcelFileType FileType, string[] DataToBeSaved)
            {

            bool TempJudgement = false;

            try
                {

                string TempExcelFileName = "";

                if (ExcelFileName == "")
                    {
                    MessageBox.Show("The parameter 'ExcelFileName' can't be empty, please input the correct file name.");
                    return false;
                    }

                if (PC.FileSystem.FileExists(ExcelFileName) == false)
                    {
                    if (MessageBox.Show("The file '" + ExcelFileName + "' is not exist, do you want to create it?" +
                        "文件 '" + ExcelFileName + "' 不存在，你需要新建一个吗？", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                        return false;
                        }
                    //else 
                    //    {
                    //后续可以考虑弹出保存对话框
                    //    }
                    }

                switch (FileType)
                    {
                    case ExcelFileType.csv:
                        TempExcelFileName = ExcelFileName + ".csv";
                        break;

                    case ExcelFileType.xls:
                        TempExcelFileName = ExcelFileName + ".xls";
                        break;

                    case ExcelFileType.xlsx:
                        TempExcelFileName = ExcelFileName + ".xlsx";
                        break;
                    }

                Microsoft.VisualBasic.FileSystem.FileOpen(1, TempExcelFileName, OpenMode.Append);
                TempJudgement = true;//用于判断是否发生异常，如果成功打开文件后操作发生错误，然后在异常处理里面关闭文件

                for (int a = 0; a < DataToBeSaved.Length; a++)
                    {
                    Microsoft.VisualBasic.FileSystem.Print(1, DataToBeSaved[a]);
                    }
                
                Microsoft.VisualBasic.FileSystem.FileClose(1);

                return true;

                }
            catch (Exception ex)
                {
                //如果成功打开文件后的操作发生错误，然后在异常处理里面关闭文件
                if (TempJudgement == true)
                    {
                    Microsoft.VisualBasic.FileSystem.FileClose(1);
                    }
                MessageBox.Show(ex.Message);
                return false;
                }

            }

        //快速写数据到Excel文件【写多行数据，每列数据之间用TAB符号隔开，以回车换行符结束】
        /// <summary>
        /// 快速写数据到Excel文件【写多行数据，每列数据之间用TAB符号隔开，以回车换行符结束】
        /// </summary>
        /// <param name="ExcelFileName">保存的Excel文件名称</param>
        /// <param name="DataToBeSaved">需要保存的数据字符串数组，每个下标字符串由每列数据之间用TAB符号隔开，以回车换行符结束</param>
        /// <returns></returns>
        private bool QuickExportDataToExcelMultiRows(string ExcelFileName,
            string[] DataToBeSaved)
            {

            bool TempJudgement = false;

            try
                {

                string TempExcelFileName = "";

                if (ExcelFileName == "")
                    {
                    MessageBox.Show("The parameter 'ExcelFileName' can't be empty, please input the correct file name.");
                    return false;
                    }

                if (PC.FileSystem.FileExists(ExcelFileName) == false)
                    {
                    if (MessageBox.Show("The file '" + ExcelFileName + "' is not exist, do you want to create it?" +
                        "文件 '" + ExcelFileName + "' 不存在，你需要新建一个吗？", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                        return false;
                        }
                    //else 
                    //    {
                    //后续可以考虑弹出保存对话框
                    //    }
                    }
                
                Microsoft.VisualBasic.FileSystem.FileOpen(1, TempExcelFileName, OpenMode.Append);
                TempJudgement = true;//用于判断是否发生异常，如果成功打开文件后操作发生错误，然后在异常处理里面关闭文件

                for (int a = 0; a < DataToBeSaved.Length; a++)
                    {
                    Microsoft.VisualBasic.FileSystem.Print(1, DataToBeSaved[a]);
                    }

                Microsoft.VisualBasic.FileSystem.FileClose(1);

                return true;

                }
            catch (Exception ex)
                {
                //如果成功打开文件后的操作发生错误，然后在异常处理里面关闭文件
                if (TempJudgement == true)
                    {
                    Microsoft.VisualBasic.FileSystem.FileClose(1);
                    }
                MessageBox.Show(ex.Message);
                return false;
                }

            }

        #endregion

        #region "窗体事件"
        
        private void frmEpsonRemoteTCP_Load(object sender, EventArgs e)
            {

            try
                {

                RC = new EpsonRemoteTCP("彭东南", txtRemoteEpsonIPAddress.Text, 5000, "", ref rtbHistory);
                RC.ShowMessage = false;
                ShowErrorMessage = false;

                for(int a=0;a<=999;a++)
                    {
                    cmbPointName.Items.Add("P" + a);
                    dgvPointData.Rows.Add();
                    dgvPointData.Rows[a].Cells[0].Value = "P" + a.ToString();
                    }
                cmbPointName.SelectedIndex = 0;

                for(int a=0;a<8;a++)
                    {
                    cmbMainFunctionNo.Items.Add(a);
                    }
                cmbMainFunctionNo.SelectedIndex = 0;

                for (int a = 1; a <= 4; a++) 
                    {
                    cmbSLockJoint.Items.Add(a);
                    cmbSFreeJoint.Items.Add(a);
                    cmbBrakeOnJoint.Items.Add(a);
                    cmbBrakeOffJoint.Items.Add(a);
                    }

                cmbSLockJoint.SelectedIndex = 0;
                cmbSFreeJoint.SelectedIndex = 0;
                cmbBrakeOnJoint.SelectedIndex = 0;
                cmbBrakeOffJoint.SelectedIndex = 0;

                AddInputSignalPictureBoxArray();

                AddOutputSignalPictureBoxArray();
                
                for (int a = 0; a<dgvPointData.Columns.Count; a++) 
                    {
                    dgvPointData.Columns[a].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvPointData.Columns[a].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                ScanningTimer.Enabled = true;
                ScanningTimer.Start();
                InterfaceInChinese = true;
                ChangeLanguageForInterface();

                ThreadUpdateRobotStatus = new Thread(UpdateRobotStatusWithSafeThread);
                ThreadUpdateRobotStatus.IsBackground = true;
                ThreadUpdateRobotStatus.Start();

                }
            catch (Exception ex) 
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        
        private void frmEpsonRemoteTCP_FormClosing(object sender, FormClosingEventArgs e)
            {            
            try
                {

                if(HideWhenClose==true)
                    {
                    e.Cancel = true;
                    return;
                    }

                if(ShowPromptWhenClose==true)
                    {
                    if(MessageBox.Show("Are you sure to exit now?\r\n" +
                        "你确定要退出程序界面吗？","提示",MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question)==DialogResult.No)
                        {
                        e.Cancel = true;
                        return;
                        }
                    }

                if(RC!=null)
                    {
                    RC.Logout();
                    RC.Dispose();
                    }

                LV = null;
                PC = null;
                FC.Dispose();
                Login.Dispose();
                About.Dispose();
                InputSignalPictureBoxArray = null;
                OutputSignalPictureBoxArray = null;
                SaveFile.Dispose();               

                if (ThreadUpdateRobotStatus!=null) 
                    {
                    ThreadUpdateRobotStatus.Abort();
                    ThreadUpdateRobotStatus = null;
                    }

                ScanningTimer.Stop();
                ScanningTimer.Dispose();

                }
            catch (Exception)// ex)
                {
                //MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnLoadPoints_Click(object sender, EventArgs e)
            {
            try
                {
                btnLoadPoints.Enabled = false;
                ScanningTimer.Stop();

                double TempX = 0.0, TempY = 0.0, TempZ = 0.0, TempU = 0.0, TempV = 0.0, TempW = 0.0;
                EpsonRemoteTCP.Hand TempHand = EpsonRemoteTCP.Hand.LeftHand;

                for (int a = 0; a <= 999; a++) 
                    {
                    if (RC.GetPointPos(cmbPointName.Items[a].ToString(), ref TempX,
                        ref TempY, ref TempZ, ref TempU, ref TempV, ref TempW, 
                        ref TempHand) == true) 
                        {

                        if (TempX == 0)
                            {
                            dgvPointData.Rows[a].Cells[1].Value = "";
                            }
                        else 
                            {
                            dgvPointData.Rows[a].Cells[1].Value = TempX;
                            }
                        
                        if (TempY == 0)
                            {
                            dgvPointData.Rows[a].Cells[2].Value = "";
                            }
                        else
                            {
                            dgvPointData.Rows[a].Cells[2].Value = TempY;
                            }

                        if (TempZ == 0)
                            {
                            dgvPointData.Rows[a].Cells[3].Value = "";
                            }
                        else
                            {
                            dgvPointData.Rows[a].Cells[3].Value = TempZ;
                            }

                        if (TempU == 0)
                            {
                            dgvPointData.Rows[a].Cells[4].Value = "";
                            }
                        else
                            {
                            dgvPointData.Rows[a].Cells[4].Value = TempU;
                            }

                        if (TempV == 0)
                            {
                            dgvPointData.Rows[a].Cells[5].Value = "";
                            }
                        else
                            {
                            dgvPointData.Rows[a].Cells[5].Value = TempV;
                            }

                        if (TempW == 0)
                            {
                            dgvPointData.Rows[a].Cells[6].Value = "";
                            }
                        else
                            {
                            dgvPointData.Rows[a].Cells[6].Value = TempW;
                            }                        

                        if(TempHand==EpsonRemoteTCP.Hand.Unknow)
                            {
                            dgvPointData.Rows[a].Cells[7].Value = "";
                            }
                        else if (TempHand == EpsonRemoteTCP.Hand.LeftHand) 
                            {
                            dgvPointData.Rows[a].Cells[7].Value = "/L";
                            }
                        else if (TempHand == EpsonRemoteTCP.Hand.RightHand) 
                            {
                            dgvPointData.Rows[a].Cells[7].Value = "/R";
                            }
                        }

                    LoadedPointData = true;

                    }

                FC.AddRichText(ref rtbHistory, "导入点数据成功...");
                btnLoadPoints.Enabled = true;
                ScanningTimer.Start();

                }
            catch (Exception ex)
                {
                FC.AddRichText(ref rtbHistory, "导入点数据出错：" + ex.Message);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnLoadPoints.Enabled = true;
                }
            }

        private void btnSavePoints_Click(object sender, EventArgs e)
            {
            try
                {
                btnSavePoints.Enabled = false;
                ScanningTimer.Stop();

                double TempX = 0.0, TempY = 0.0, TempZ = 0.0, TempU = 0.0, TempV = 0.0, TempW = 0.0;
                EpsonRemoteTCP.Hand TempHand = EpsonRemoteTCP.Hand.LeftHand;

                int SetPointOK=0,SetPointFailed=0;

                for (int a = 0; a <= 999; a++)
                    {

                    if (dgvPointData.Rows[a].Cells[1].Value.ToString() == "")
                        {
                        TempX = 0;
                        }
                    else
                        {
                        TempX = (double)dgvPointData.Rows[a].Cells[1].Value;
                        }

                    if (dgvPointData.Rows[a].Cells[2].Value.ToString() == "")
                        {
                        TempY = 0;
                        }
                    else
                        {
                        TempY = (double)dgvPointData.Rows[a].Cells[2].Value;
                        }

                    if (dgvPointData.Rows[a].Cells[3].Value.ToString() == "")
                        {
                        TempZ = 0;
                        }
                    else
                        {
                        TempZ = (double)dgvPointData.Rows[a].Cells[3].Value;
                        }

                    if (dgvPointData.Rows[a].Cells[4].Value.ToString() == "")
                        {
                        TempU = 0;
                        }
                    else
                        {
                        TempU = (double)dgvPointData.Rows[a].Cells[4].Value;
                        }

                    if (dgvPointData.Rows[a].Cells[5].Value.ToString() == "")
                        {
                        TempV = 0;
                        }
                    else
                        {
                        TempV = (double)dgvPointData.Rows[a].Cells[5].Value;
                        }

                    if (dgvPointData.Rows[a].Cells[6].Value.ToString() == "")
                        {
                        TempW = 0;
                        }
                    else
                        {
                        TempW = (double)dgvPointData.Rows[a].Cells[6].Value;
                        }

                    if (dgvPointData.Rows[a].Cells[7].Value.ToString() == "")
                        {
                        TempHand = EpsonRemoteTCP.Hand.Unknow;
                        }
                    else
                        {
                        if (dgvPointData.Rows[a].Cells[7].Value.ToString() == "/L") 
                            {
                            TempHand = EpsonRemoteTCP.Hand.LeftHand;
                            }
                        else if (dgvPointData.Rows[a].Cells[7].Value.ToString() == "/R") 
                            {
                            TempHand = EpsonRemoteTCP.Hand.RightHand;
                            }
                        }

                    if (RC.SetPointPos(cmbPointName.Items[a].ToString(), TempX,
                        TempY, TempZ, TempU, TempV, TempW,
                        TempHand) == true)
                        {
                        SetPointOK += 1;
                        }
                    else
                        {
                        SetPointFailed += 1;
                        }

                    }

                RC.SavePointPos();

                FC.AddRichText(ref rtbHistory, "保存点数据 " + SetPointOK + " 成功, " 
                    + SetPointFailed + " 失败...");
                btnSavePoints.Enabled = true;
                ScanningTimer.Start();

                }
            catch (Exception ex)
                {
                FC.AddRichText(ref rtbHistory, "保存点数据出错：" + ex.Message);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSavePoints.Enabled = true;
                }
            }

        private void btnExportPointDataToExcel_Click(object sender, EventArgs e)
            {
            try
                {
                if (LoadedPointData == false) 
                    {
                    MessageBox.Show("Please load the point data from EPSON robot first before executing export the data to excel file.");
                    return;
                    }

                btnSavePoints.Enabled = false;

                SaveFile.DefaultExt = "xls";
                SaveFile.Filter = "*.xls|*.xls|*.xlsx|*.xlsx|*.csv|*.csv";
                SaveFile.AddExtension = true;

                string[] PointData = new string[1001];
                string TempPointName, TempX, TempY, TempZ, TempU, TempV, TempW, TempHand;

                PointData[0] = "Point No." + Strings.Chr(9) + "X坐标" + Strings.Chr(9) + "Y坐标" + Strings.Chr(9) +
                    "Z坐标" + Strings.Chr(9) + "U坐标" + Strings.Chr(9) + "V坐标" + Strings.Chr(9) + "W坐标" 
                    + Strings.Chr(9) + "手势\r\n";

                if (SaveFile.ShowDialog() == DialogResult.OK) 
                    {
                    if (SaveFile.FileName != "")
                        {
                        for (int a = 0; a <= 999; a++) 
                            {
                            TempPointName = ((dgvPointData.Rows[a].Cells[0].Value == null)
                                ? "" : dgvPointData.Rows[a].Cells[0].Value.ToString());

                            TempX = ((dgvPointData.Rows[a].Cells[1].Value == null)
                                ? "" : dgvPointData.Rows[a].Cells[1].Value.ToString());

                            TempY = ((dgvPointData.Rows[a].Cells[2].Value == null)
                                ? "" : dgvPointData.Rows[a].Cells[2].Value.ToString());

                            TempZ = ((dgvPointData.Rows[a].Cells[3].Value == null)
                                ? "" : dgvPointData.Rows[a].Cells[3].Value.ToString());

                            TempU = ((dgvPointData.Rows[a].Cells[4].Value == null)
                                ? "" : dgvPointData.Rows[a].Cells[4].Value.ToString());

                            TempV = ((dgvPointData.Rows[a].Cells[5].Value == null)
                                ? "" : dgvPointData.Rows[a].Cells[5].Value.ToString());

                            TempW = ((dgvPointData.Rows[a].Cells[6].Value == null)
                                ? "" : dgvPointData.Rows[a].Cells[6].Value.ToString());

                            TempHand = ((dgvPointData.Rows[a].Cells[7].Value == null)
                                ? "" : dgvPointData.Rows[a].Cells[7].Value.ToString());

                            PointData[a + 1] = TempPointName + Strings.Chr(9) + TempX + Strings.Chr(9) + 
                                TempY + Strings.Chr(9) + TempZ + Strings.Chr(9) + TempU + Strings.Chr(9) 
                                + TempV + Strings.Chr(9) + TempW + Strings.Chr(9) + TempHand + "\r\n";
                            }

                        if (QuickExportDataToExcelMultiRows(SaveFile.FileName, PointData) == true)
                            {
                            FC.AddRichText(ref rtbHistory, "保存点坐标数据到EXCEL文件成功...");
                            }
                        else 
                            {
                            FC.AddRichText(ref rtbHistory, "保存点坐标数据到EXCEL文件出错...");
                            }

                        }
                    else 
                        {
                        return;
                        }
                    }

                }
            catch (Exception ex)
                {
                btnSavePoints.Enabled = true;
                FC.AddRichText(ref rtbHistory, ex.Message);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnXJogPositive_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "X轴正转: " + RC.XMovePositive(Conversion.Val(txtJogDistance.Text)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnXJogNegative_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "X轴反转: " + RC.XMoveNegative(Conversion.Val(txtJogDistance.Text)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnYJogPositive_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "Y轴正转: " + RC.YMovePositive(Conversion.Val(txtJogDistance.Text)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnYJogNegative_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "Y轴反转: " + RC.YMoveNegative(Conversion.Val(txtJogDistance.Text)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnZUpward_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "Z轴上升: " + RC.ZMoveUp(Conversion.Val(txtJogDistance.Text)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnZDownward_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "Z轴下降: " + RC.ZMoveDown(Conversion.Val(txtJogDistance.Text)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnURotatePositive_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "U轴正转: " + RC.URotatePositive(Conversion.Val(txtJogDistance.Text)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnURotateNegative_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "U轴反转: " + RC.URotateNegative(Conversion.Val(txtJogDistance.Text)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnVJogPositive_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "V轴正转: " + RC.VMovePositive(Conversion.Val(txtJogDistance.Text)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnVJogNegative_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "V轴反转: " + RC.VMoveNegative(Conversion.Val(txtJogDistance.Text)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnWJogPositive_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "W轴正转: " + RC.WMovePositive(Conversion.Val(txtJogDistance.Text)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnWJogNegative_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "W轴反转: " + RC.WMoveNegative(Conversion.Val(txtJogDistance.Text)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnMotorOn_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "打开电机: " + RC.MotorOn());
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnMotorOff_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "关闭电机: " + RC.MotorOff());
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnPowerHigh_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, RC.GetPowerStatus().ToString());
                FC.AddRichText(ref rtbHistory, "设定高功率: " + RC.PowerHigh());
                FC.AddRichText(ref rtbHistory, RC.GetPowerStatus().ToString());
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnPowerLow_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, RC.GetPowerStatus().ToString());
                FC.AddRichText(ref rtbHistory, "设定低功率: " + RC.PowerLow());
                FC.AddRichText(ref rtbHistory, RC.GetPowerStatus().ToString());
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnSLock_Click(object sender, EventArgs e)
            {
            try
                {
                if(cmbSLockJoint.SelectedIndex==-1)
                    {
                    MessageBox.Show("Please select an axis to be locked.\r\n" +
                        "请先选择一个需要锁定的轴。");
                    cmbSLockJoint.Focus();
                    return;
                    }
                FC.AddRichText(ref rtbHistory, "锁定轴: " + RC.SLock(Convert.ToInt32(cmbSLockJoint.SelectedItem)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnSFree_Click(object sender, EventArgs e)
            {
            try
                {
                if (cmbSFreeJoint.SelectedIndex == -1)
                    {
                    MessageBox.Show("Please select an axis to be free.\r\n" +
                        "请先选择一个需要松开使能的轴。");
                    cmbSFreeJoint.Focus();
                    return;
                    }
                FC.AddRichText(ref rtbHistory, "松开使能轴: " + RC.SFree(Convert.ToInt32(cmbSFreeJoint.SelectedItem)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnBrakeOn_Click(object sender, EventArgs e)
            {
            try
                {
                if (cmbBrakeOnJoint.SelectedIndex == -1)
                    {
                    MessageBox.Show("Please select an axis to be braked on.\r\n" +
                        "请先选择一个需要启用刹车的轴。");
                    cmbBrakeOnJoint.Focus();
                    return;
                    }
                FC.AddRichText(ref rtbHistory, "启用轴刹车: " + RC.BrakeOn(Convert.ToInt32(cmbBrakeOnJoint.SelectedItem)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnBrakeOff_Click(object sender, EventArgs e)
            {
            try
                {
                if (cmbBrakeOffJoint.SelectedIndex == -1)
                    {
                    MessageBox.Show("Please select an axis to be braked off.\r\n" +
                        "请先选择一个需要松开刹车的轴。");
                    cmbBrakeOffJoint.Focus();
                    return;
                    }
                FC.AddRichText(ref rtbHistory, "松开轴刹车: " + RC.BrakeOff(Convert.ToInt32(cmbBrakeOffJoint.SelectedItem)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnSLockAll_Click(object sender, EventArgs e)
            {
            try
                {
                //看能否从机械手获取机械手型号，从而判断是几轴机械手
                FC.AddRichText(ref rtbHistory, "锁定所有4轴: " + RC.SLockAll(4));
                FC.AddRichText(ref rtbHistory, "锁定所有6轴: " + RC.SLockAll(6));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnSFreeAll_Click(object sender, EventArgs e)
            {
            try
                {
                //看能否从机械手获取机械手型号，从而判断是几轴机械手
                FC.AddRichText(ref rtbHistory, "所有4轴松开使能: " + RC.SFreeAll(4));
                FC.AddRichText(ref rtbHistory, "所有6轴松开使能: " + RC.SFreeAll(6));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnAllBrakeOn_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "所有4轴启用刹车: " + RC.AllBrakeOn(4));
                FC.AddRichText(ref rtbHistory, "所有6轴启用刹车: " + RC.AllBrakeOn(6));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnAllBrakeOff_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "所有4轴松开刹车: " + RC.AllBrakeOff(4));
                FC.AddRichText(ref rtbHistory, "所有6轴松开刹车: " + RC.AllBrakeOff(6));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnRun_Click(object sender, EventArgs e)
            {
            try
                {
                if (cmbMainFunctionNo.SelectedIndex == -1)
                    {
                    MessageBox.Show("Please select a program [0~7] to be run.\r\n" +
                        "请先选择一个需要运行的EPSON程序编号。");
                    cmbBrakeOffJoint.Focus();
                    return;
                    }

                FC.AddRichText(ref rtbHistory, "运行程序: " + RC.StartMission(Convert.ToInt32(cmbMainFunctionNo.SelectedItem)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnStop_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "停止程序: " + RC.StopMission());
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnPauseContinue_Click(object sender, EventArgs e)
            {
            try
                {
                if (RC != null) 
                    {

                    if (RC.RobotPaused == true)
                        {
                        if (RC.RobotContinue() == true)
                            {
                            FC.AddRichText(ref rtbHistory, "执行 '继续'成功...");
                            if (InterfaceInChinese == true)
                                {
                                btnPauseContinue.Text = "暂停";
                                }
                            else 
                                {
                                btnPauseContinue.Text = "Pause";
                                }
                            }
                        else 
                            {
                            FC.AddRichText(ref rtbHistory, "执行 '继续'出错...");
                            }
                        }
                    else 
                        {
                        if (RC.RobotPause() == true)
                            {
                            FC.AddRichText(ref rtbHistory, "执行 '暂停'成功...");
                            if (InterfaceInChinese == true)
                                {
                                btnPauseContinue.Text = "继续";
                                }
                            else 
                                {
                                btnPauseContinue.Text = "Continue";
                                }
                            }
                        else 
                            {
                            FC.AddRichText(ref rtbHistory, "执行 '暂停'出错...");
                            }
                        }
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnAbort_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "终止命令: " + RC.Abort());
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnResetRobot_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "复位机器人: " + RC.ResetRobot());
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnJump_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "JUMP拱形运动至点位: " +
                    cmbPointName.SelectedItem.ToString() + "  " + 
                    RC.Jump(cmbPointName.SelectedItem.ToString()));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void rbtnRightHand_CheckedChanged(object sender, EventArgs e)
            {

            try
                {



                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

        private void rbtnLeftHand_CheckedChanged(object sender, EventArgs e)
            {
            try
                {



                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnTeach_Click(object sender, EventArgs e)
            {
            try
                {

                if (cmbPointName.SelectedIndex != -1) 
                    {
                    MessageBox.Show("Please select a point at the point data list before you execute 'Teach'.\r\n"
                        + "请在示教位置前先在点数据列表中选中某个点位。");
                    return;
                    }

                if(RC.GetCurrentPos(ref RCPoint.X, ref RCPoint.Y, ref RCPoint.Z,
                    ref RCPoint.U, ref RCPoint.V, ref RCPoint.W, ref RCPoint.HandStyle)==true)
                    {
                    if (RC.SetPointPos(cmbPointName.SelectedItem.ToString(), RCPoint) == true)
                        {
                        if (RCPoint.HandStyle == EpsonRemoteTCP.Hand.LeftHand)
                            {
                            rbtnLeftHand.Checked = true;
                            rbtnRightHand.Checked = false;
                            }
                        else if (RCPoint.HandStyle == EpsonRemoteTCP.Hand.RightHand)
                            {
                            rbtnLeftHand.Checked = false;
                            rbtnRightHand.Checked = true;
                            }
                        else 
                            {
                            rbtnLeftHand.Checked = false;
                            rbtnRightHand.Checked = false;
                            }

                        dgvPointData.Rows[cmbPointName.SelectedIndex].Cells[1].Value = RCPoint.X;
                        dgvPointData.Rows[cmbPointName.SelectedIndex].Cells[2].Value = RCPoint.X;
                        dgvPointData.Rows[cmbPointName.SelectedIndex].Cells[3].Value = RCPoint.X;
                        dgvPointData.Rows[cmbPointName.SelectedIndex].Cells[4].Value = RCPoint.X;
                        dgvPointData.Rows[cmbPointName.SelectedIndex].Cells[5].Value = RCPoint.X;
                        dgvPointData.Rows[cmbPointName.SelectedIndex].Cells[6].Value = RCPoint.X;

                        if (RCPoint.HandStyle == EpsonRemoteTCP.Hand.LeftHand)
                            {
                            dgvPointData.Rows[cmbPointName.SelectedIndex].Cells[7].Value = "/L";
                            }
                        else if (RCPoint.HandStyle == EpsonRemoteTCP.Hand.RightHand)
                            {
                            dgvPointData.Rows[cmbPointName.SelectedIndex].Cells[7].Value = "/R";
                            }
                        else if (RCPoint.HandStyle == EpsonRemoteTCP.Hand.Unknow)
                            {
                            dgvPointData.Rows[cmbPointName.SelectedIndex].Cells[7].Value = "";
                            }

                        FC.AddRichText(ref rtbHistory, "示教点位: " + cmbPointName.SelectedItem.ToString() + " true...");
                        }
                    else 
                        {
                        FC.AddRichText(ref rtbHistory, "示教点位: " + cmbPointName.SelectedItem.ToString() + " false...");
                        }
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnSetSpeed_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "设置速度: " + txtSpeed.Text 
                    + "  " + RC.SetSpeed(Convert.ToInt32(txtSpeed.Text)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnSetACCELSpeed_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "设置加速度: " + txtAccelSpeed.Text
                     + "  " + "减速度: " + txtDecelSpeed.Text + "  " 
                    + RC.SetACCELSpeed(Convert.ToUInt16(txtAccelSpeed.Text),
                    Convert.ToUInt16(txtDecelSpeed.Text)));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnLogin_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "登录: " + RC.Login(txtPassword.Text));
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnLogout_Click(object sender, EventArgs e)
            {
            try
                {
                FC.AddRichText(ref rtbHistory, "退出登录: " + RC.Logout());
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnConnectRobot_Click(object sender, EventArgs e)
            {
            try
                {
                if(RC != null)
                    {
                    if(RC.ConnectedWithRemoteEpson==true)
                        {
                        if(MessageBox.Show("Are you sure to disconnect the remote EPSON controller right now?"
                            + "\r\n正在和EPSON进行远程以太网控制中，确定要断开当前连接吗？")==DialogResult.No)
                            {
                            return;
                            }
                        else
                            {
                            RC.Dispose();
                            }
                        }
                    }

                RC = null;

                if(FC.VerifyIPAddressNew(txtRemoteEpsonIPAddress.Text)==false)
                    {
                    MessageBox.Show("The IP address of remote EPSON controller is not correct, please revise it and retry."
                        + "\r\n你输入的EPSON远程以太网控制器的IP地址不正确，请改正后再试。");
                    return;
                    }

                RC = new EpsonRemoteTCP("彭东南", txtRemoteEpsonIPAddress.Text, 
                    Convert.ToUInt16(txtRobotRemotePort.Text), txtPassword.Text);

                FC.AddRichText(ref rtbHistory, "成功启动连接机器人…");
                System.Diagnostics.Debug.WriteLine("成功启动连接机器人...");
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnExecute_Click(object sender, EventArgs e)
            {
            try
                {
                if (txtExecute.Text == "") 
                    {
                    MessageBox.Show("Please input the correct SPEL command in the textbox before you press 'Execute'.");
                    return;
                    }

                string TempStr = RC.SendCommand("$Execute,\"" + txtExecute.Text + "\"");

                FC.AddRichText(ref rtbHistory, TempStr);
                if (TempStr.IndexOf("") == -1)
                    {
                    FC.AddRichText(ref rtbHistory, "执行命令:" + txtExecute.Text + "成功...");
                    }
                else 
                    {
                    FC.AddRichText(ref rtbHistory, "执行命令:" + txtExecute.Text + "出错...");
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void txtPointQty_TextChanged(object sender, EventArgs e)
            {

            try
                {



                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

        private void txtPointQty_KeyPress(object sender, KeyPressEventArgs e)
            {
            try
                {
                if (!(System.Char.IsNumber(e.KeyChar)
                    || System.Char.IsControl(e.KeyChar)))
                    {
                    e.Handled = true;
                    FC.AddRichText(ref rtbHistory, "需要操作的EPSON点位数量只能是数字，请重新输入。");
                    MessageBox.Show("The point q'ty of EPSON robot can only be number, please revise it.\r\n"
                        + "需要操作的EPSON点位数量只能是数字，请重新输入。");
                    return;
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void txtPointQty_KeyDown(object sender, KeyEventArgs e)
            {
            try
                {

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void txtSpeed_KeyPress(object sender, KeyPressEventArgs e)
            {
            try
                {
                if (!(System.Char.IsNumber(e.KeyChar)
                    || System.Char.IsControl(e.KeyChar)))
                    {
                    e.Handled = true;
                    FC.AddRichText(ref rtbHistory, "EPSON机械手的速度只能是数字，请重新输入。");
                    MessageBox.Show("The speed of EPSON robot can only be number, please revise it.\r\n"
                        + "EPSON机械手的速度只能是数字，请重新输入。");
                    return;
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void txtAccelSpeed_KeyPress(object sender, KeyPressEventArgs e)
            {
            try
                {
                if (!(System.Char.IsNumber(e.KeyChar)
                    || System.Char.IsControl(e.KeyChar)))
                    {
                    e.Handled = true;
                    FC.AddRichText(ref rtbHistory, "EPSON机械手的加速度只能是数字，请重新输入。");
                    MessageBox.Show("The accel speed of EPSON robot can only be number, please revise it.\r\n"
                        + "EPSON机械手的加速度只能是数字，请重新输入。");
                    return;
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void txtDecelSpeed_KeyPress(object sender, KeyPressEventArgs e)
            {
            try
                {
                if (!(System.Char.IsNumber(e.KeyChar)
                    || System.Char.IsControl(e.KeyChar)))
                    {
                    e.Handled = true;
                    FC.AddRichText(ref rtbHistory, "EPSON机械手的减速度只能是数字，请重新输入。");
                    MessageBox.Show("The decel speed of EPSON robot can only be number, please revise it.\r\n"
                        + "EPSON机械手的减速度只能是数字，请重新输入。");
                    return;
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void txtJogDistance_KeyPress(object sender, KeyPressEventArgs e)
            {
            try
                {
                //点位数据只能是数字、控制键和'.'
                if (!(System.Char.IsNumber(e.KeyChar)
                    || System.Char.IsControl(e.KeyChar)
                    || e.KeyChar == '.'))
                    {
                    //此次输入无效
                    e.Handled = true;
                    FC.AddRichText(ref rtbHistory, "点动距离只能是数字和'.'，请重新输入...");
                    MessageBox.Show("点动距离只能是数字和'.'，请重新输入...");
                    return;
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void txtJogDistance_KeyDown(object sender, KeyEventArgs e)
            {
            try
                {
                int TempRet = 0;
                TempRet = Strings.InStr(txtJogDistance.Text, ".");

                //点位数据不能有两个及以上'.'号
                if (TempRet != -1)
                    {
                    TempRet = Strings.InStr(TempRet, txtJogDistance.Text, ".");
                    if (TempRet != -1)
                        {
                        e.Handled = true;
                        FC.AddRichText(ref rtbHistory, "点动距离不能有两个及以上'.'号，请重新输入...");
                        MessageBox.Show("点动距离不能有两个及以上'.'号，请重新输入...");
                        txtJogDistance.Focus();
                        return;
                        }
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void txtJogDistance_TextChanged(object sender, EventArgs e)
            {

            }

        private void ScanningTimer_Tick(object sender, EventArgs e)
            {
            try
                {

                FC.ChangeLabelText(ref lblDateAndTime, DateAndTime.Now.ToString());

                if (LoggedSuccess == true && (LoggedCode > 0 && LoggedCode < 10))
                    {
                    FC.ChangeButtonEnableStatus(ref btnLoadPoints, true);
                    FC.ChangeButtonEnableStatus(ref btnSavePoints, true);
                    }
                else 
                    {
                    FC.ChangeButtonEnableStatus(ref btnLoadPoints, false);
                    FC.ChangeButtonEnableStatus(ref btnSavePoints, false);
                    }

                //if(RC!=null)
                //    {
                //    if(RC.ConnectedWithRemoteEpson==true)
                //        {
                        
                //        }
                //    }

                DynamicPicture += 1;
                if (DynamicPicture % 2 == 0)
                    {
                    FC.ChangeImageOfPicture(ref picDynamicPicture,
                        global::PengDongNanTools.Properties.Resources.GreenOff);
                    }
                else 
                    {
                    FC.ChangeImageOfPicture(ref picDynamicPicture,
                        global::PengDongNanTools.Properties.Resources.GreenOn);
                    }

                FC.ChangeLabelPosition(ref lblReminder, 
                    lblReminder.Left + DynamicPicture * 2, lblReminder.Top);

                if (DynamicPicture > 10)
                    {
                    lblReminder.Left = 0;
                    DynamicPicture = 0;
                    }

                }
            catch (Exception ex)
                {
                //MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FC.AddRichText(ref rtbHistory, ex.Message);
                }
            }
        
        private void txtPassword_TextChanged(object sender, EventArgs e)
            {
            RemoteRobotLoginPassword = txtPassword.Text;
            }

        private void dgvPointData_CellContentClick(object sender,
            DataGridViewCellEventArgs e)
            {
            try
                {
                if (e.RowIndex != -1)
                    {
                    cmbPointName.SelectedIndex = e.RowIndex;

                    if (dgvPointData.Rows[e.RowIndex].Cells[7].Value != null)
                        {
                        if (dgvPointData.Rows[e.RowIndex].Cells[7].Value.ToString() == "/L")
                            {
                            rbtnLeftHand.Checked = true;
                            rbtnRightHand.Checked = false;
                            }
                        else if (dgvPointData.Rows[e.RowIndex].Cells[7].Value.ToString() == "/R")
                            {
                            rbtnLeftHand.Checked = false;
                            rbtnRightHand.Checked = true;
                            }
                        }
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void dgvPointData_KeyPress(object sender, KeyPressEventArgs e)
            {
            try
                {
                if (LoggedCode <= 0 || LoggedCode >= 10)
                    {
                    e.Handled = true;
                    MessageBox.Show("You don't have the right to edit the point data!\r\n" +
                        "你没有权限修改点位数据。");
                    return;
                    }

                //点位数据只能是数字、控制键和'.'
                if (!(System.Char.IsNumber(e.KeyChar)
                    || System.Char.IsControl(e.KeyChar)
                    || e.KeyChar == '.'))
                    {
                    //此次输入无效
                    e.Handled = true;
                    FC.AddRichText(ref rtbHistory, "点位数据只能是数字和'.'，请重新输入...");
                    MessageBox.Show("点位数据只能是数字和'.'，请重新输入...");
                    return;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void dgvPointData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
            {
            try
                {
                if (LoggedCode <= 0 || LoggedCode >= 10)
                    {
                    MessageBox.Show("You don't have the right to edit the point data!\r\n" +
                        "你没有权限修改点位数据。");
                    return;
                    }

                int TempRet = 0;
                TempRet = Strings.InStr(dgvPointData.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), ".");

                //点位数据不能有两个及以上'.'号
                if (TempRet != -1)
                    {
                    TempRet = Strings.InStr(TempRet, dgvPointData.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), ".");
                    if (TempRet != -1)
                        {
                        FC.AddRichText(ref rtbHistory, "点位数据不能有两个及以上'.'号，请重新输入...");
                        MessageBox.Show("点位数据不能有两个及以上'.'号，请重新输入...");
                        dgvPointData.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                        return;
                        }
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void rtbHistory_TextChanged(object sender, EventArgs e)
            {

            }

        private void rtbHistory_MouseDoubleClick(object sender, MouseEventArgs e)
            {
            if (rtbHistory.Text == "")
                {
                return;
                }

            if (MessageBox.Show("Are you sure to clear all the running log?"
                + "Please click right button of mouse to save it first.\r\n"
                + "点击鼠标右键进行保存，确定要清除所有的运行记录吗？", "提示",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                rtbHistory.Text = "";
                }
            }

        private void rtbHistory_MouseDown(object sender, MouseEventArgs e)
            {
            try
                {

                if (rtbHistory.Text == "")
                    {
                    return;
                    }

                if (e.Button == MouseButtons.Right)
                    {
                    SaveFile.DefaultExt = "txt";
                    SaveFile.Filter = "TXT文本文件 (*.txt)|*.txt";
                    SaveFile.Title = "保存运行记录至文件";

                    //'s'--将日期和时间格式化为可排序的索引。例如 2008-03-12T11:07:31。 s 字符以用户定义的时间格式显示秒钟。
                    //'u'--将日期和时间格式化为 GMT 可排序索引。如 2008-03-12 11:07:31Z。
                    SaveFile.FileName = "EpsonRunLog" + "-" + Strings.Format(DateAndTime.Now, "yyyy'年'MM'月'dd'日' HH'点'mm'分'ss'秒'"); // "yyyy-MM-dd HH%h-mm%m-ss%s") //"s")
                    SaveFile.RestoreDirectory = true;

                    if (SaveFile.ShowDialog() == DialogResult.OK
                        && SaveFile.FileName != "")
                        {
                        rtbHistory.SaveFile(SaveFile.FileName, RichTextBoxStreamType.TextTextOleObjs);
                        }

                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        
        private void picDynamicPicture_Click(object sender, EventArgs e)
            {
            if (ShowErrorMessage == true)
                {
                ShowErrorMessage = false;
                }
            else 
                {
                ShowErrorMessage = true;
                }
            }

        private void picDynamicPicture_MouseDown(object sender, MouseEventArgs e)
            {
            if (e.Button == MouseButtons.Right) 
                {
                Login.ShowDialog();
                LoggedCode = Login.LoginCode;
                }
            }

        private void picHelp_Click(object sender, EventArgs e)
            {
            About.ShowDialog();
            }

        private void txtExecute_KeyPress(object sender, KeyPressEventArgs e)
            {

            }

        private void txtExecute_KeyDown(object sender, KeyEventArgs e)
            {
            if (e.KeyCode == Keys.Enter) 
                {
                e.Handled = true;
                btnExecute.Focus();
                }
            }

        private void picChinese_Click(object sender, EventArgs e)
            {
            if (InterfaceInChinese == false) 
                {
                InterfaceInChinese = true;
                ChangeLanguageForInterface();
                }
            }

        private void picEnglish_Click(object sender, EventArgs e)
            {
            if (InterfaceInChinese == true)
                {
                InterfaceInChinese = false;
                ChangeLanguageForInterface();
                }
            }

        private void txtRobotRemotePort_KeyPress(object sender, KeyPressEventArgs e)
            {
            try
                {
                if (!(System.Char.IsNumber(e.KeyChar)
                    || System.Char.IsControl(e.KeyChar)))
                    {
                    e.Handled = true;
                    FC.AddRichText(ref rtbHistory, "EPSON远程以太网通讯的端口号只能是数字，请重新输入。");
                    MessageBox.Show("The remote TCP/IP control port of EPSON can only be number, please revise it.\r\n"
                        + "EPSON远程以太网通讯的端口号只能是数字，请重新输入。");
                    return;
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void txtRobotRemotePort_KeyDown(object sender, KeyEventArgs e)
            {
            try
                {
                
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        #endregion

        }//class frmEpsonRemoteTCP

    }//namespace PengDongNanTools