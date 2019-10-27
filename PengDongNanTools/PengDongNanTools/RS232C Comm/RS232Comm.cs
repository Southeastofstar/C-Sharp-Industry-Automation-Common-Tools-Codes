#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;

#endregion

//ok

namespace PengDongNanTools
    {

    //RS232C串口通讯【软件作者：彭东南, southeastofstar@163.com】
    /// <summary>
    /// RS232C串口通讯【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    class RS232Comm
        {

        #region "变量定义"

        /// <summary>
        /// 更新信息至文本框时是否显示日期和时间，默认为True
        /// </summary>
        public bool ShowDateTimeForMessage = true;

        private OpenFileDialog newOpenFile = new OpenFileDialog();

        //定义类用于操作计算机相关信息，类似于VB .NET中的my.computer
        private Microsoft.VisualBasic.Devices.Computer pcFileSystem = new Microsoft.VisualBasic.Devices.Computer();
        
        private string AutoSendMessage;

        private System.IO.Ports.SerialPort TempSerialPort=new System.IO.Ports.SerialPort();

        private System.Timers.Timer tmrAutoSend=new System.Timers.Timer();

        private System.Windows.Forms.RichTextBox TempRichTextBox=new System.Windows.Forms.RichTextBox();

        private bool AutoSendFlag = false;
        private string[] FileNameToBeSent;
        private bool SendFileFlag = false;

        /// <summary>
        /// 以16进制进行发送
        /// </summary>
        public bool RS232CSendMessageInHEX = false;

        /// <summary>
        /// 是否执行串口数据行读取，即读取以回车+换行为结束符或其它定义的结束符的一整行【即以回车+换行为结束符】
        /// 默认是True执行读取整行数据；设置为False则读取字节，返回用空格分开的字节内容
        /// </summary>
        public bool ExecuteReadLine = true;

        private Button ButtonUseForInvoke = new Button();

        /// <summary>
        /// 串口通讯程序运行过程中的错误信息提示
        /// </summary>
        public string ErrorMessage;

        //string TempErrorMessage = "";

        /// <summary>
        /// 当错误信息相同时，是否重复显示
        /// </summary>
        public bool UpdatingSameMessage = true;

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }

        /// <summary>
        /// 客户端从服务器收到的字符串内容
        /// </summary>
        public string ReceivedString;

        /// <summary>
        /// 自定义的结束符[用于发送时在发送的字符串最后加上]
        /// </summary>
        public string EndingCustomerizeSetting="";

        /// <summary>
        /// 用于记录实例化时输入密码是否正确
        /// </summary>
        private bool PasswordIsCorrect;//=false;

        /// <summary>
        /// 成功建立新的实例
        /// </summary>
        private bool SuccessBuiltNew = false;
                
        private static System.IO.Ports.SerialPort RS232CPort = new System.IO.Ports.SerialPort();
        
        private int AvailableRS232CPorts=0;
        private string[] AvailablePortNames;

        private System.IO.Ports.SerialError SerialErrorMsg=0;

        /// <summary>
        /// 串口错误信息
        /// </summary>
        public System.IO.Ports.SerialError PortErrorMessage
            {
            get{
                return SerialErrorMsg;   
                }
            }

        /// <summary>
        /// 枚举:进行通讯时发送字符的结尾符号
        /// </summary>
        public enum Endings
            {

            /// <summary>
            /// 无
            /// </summary>
            None = 0,

            /// <summary>
            /// 换行符
            /// </summary>
            LF = 1,

            /// <summary>
            /// 回车符
            /// </summary>
            CR=2,

            /// <summary>
            /// 回车换行符
            /// </summary>
            CRLF =3,

            /// <summary>
            /// 自定义
            /// </summary>
            Customerize = 4
            }

        /// <summary>
        /// 发送字符串的后缀【结束符】
        /// </summary>
        private string Suffix = "\r\n";

        private Endings CurrentEndingSetting = Endings.CRLF;

        /// <summary>
        /// 待处理事项，后续再添加代码
        /// </summary>
        public string PendingIssue 
            {
            get { return ""; }
            }

        /// <summary>
        /// RS232C通讯参数数据结构
        /// </summary>
        public struct Parameters
            {
            public int BaudRateSetting
                {
                get { return RS232CPort.BaudRate; }
                set { RS232CPort.BaudRate = value; }
                }

            public int DataBitsSetting
                {
                get { return RS232CPort.DataBits; }
                set { RS232CPort.DataBits = value; }
                }

            public bool DTREnableSetting
                {
                get { return RS232CPort.DtrEnable; }
                set { RS232CPort.DtrEnable = value; }
                }

            public System.IO.Ports.Handshake HandShakeSetting
                {
                get { return RS232CPort.Handshake; }
                set { RS232CPort.Handshake = value; }
                }

            public System.IO.Ports.Parity ParitySetting
                {
                get { return RS232CPort.Parity; }
                set { RS232CPort.Parity = value; }
                }

            public string PortNameSetting
                {

                get { return RS232CPort.PortName; }

                set
                    {
                    string[] TempPorts;
                    TempPorts = System.IO.Ports.SerialPort.GetPortNames();
                    bool FoundName = false;
                    for (int a = 0; a < TempPorts.Length;a++ ) 
                        {
                        if(TempPorts[a]==value)
                            {
                            FoundName = true;
                            break;
                            }
                        
                        }

                    if (FoundName == true)
                        {
                        RS232CPort.PortName = value;
                        }
                    else
                        {
                        MessageBox.Show("The port name " + value + " is not exist in the PC, please check and revise it.",
                            "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }
                    
                    }
                }

            public int ReadBufferSizeSetting
                {
                get { return RS232CPort.ReadBufferSize; }
                set { RS232CPort.ReadBufferSize = value; }
                }

            public int ReceivedBytesThresholdSetting
                {
                get { return RS232CPort.ReceivedBytesThreshold; }
                set { RS232CPort.ReceivedBytesThreshold = value; }
                }

            public bool RTSEnableSetting
                {
                get { return RS232CPort.RtsEnable; }
                set { RS232CPort.RtsEnable = value; }
                }

            public System.IO.Ports.StopBits StopBitsSetting
                {
                get { return RS232CPort.StopBits; }
                set { RS232CPort.StopBits = value; }
                }

            public int WriteBufferSizeSetting
                {
                get { return RS232CPort.WriteBufferSize; }
                set { RS232CPort.WriteBufferSize = value; }
                }

            public Endings Suffix
                {
                //get { return CurrentEndingSetting; }
                set { this.Suffix = value; }
                }

            public System.Text.Encoding EncodingSetting
                {
                get { return RS232CPort.Encoding; }
                set
                    {                    
                    RS232CPort.Encoding = value;
                    }
                }


            //public int BaudRateSetting;
            //public int DataBitsSetting;
            //public bool DTREnableSetting;
            //public System.IO.Ports.Handshake HandShakeSetting;
            //public System.IO.Ports.Parity ParitySetting;
            //public string PortNameSetting;
            //public int ReadBufferSizeSetting;
            //public int ReceivedBytesThresholdSetting;
            //public bool RTSEnableSetting;
            //public System.IO.Ports.StopBits StopBitsSetting;
            //public int WriteBufferSizeSetting;
            //public Endings Suffix;
            //public System.Text.Encoding EncodingSetting;            
            }

        ///// <summary>
        ///// 设置或获取当前RS232C串口通讯的参数
        ///// </summary>
        //public Parameters Settings;

        /// <summary>
        /// 可用串口名称[数组]
        /// </summary>
        public string[] AvailablePorts 
            {
            get { return SearchAvailableRS232Ports(); }            
            }

        /// <summary>
        /// 可用串口数量
        /// </summary>
        public int AvailablePortCount 
            {
            get { return AvailableRS232CPortCount(); }
            }

        /// <summary>
        /// 实例化后的串口名称
        /// </summary>
        public string PortName 
            {
            get 
                {
                if (RS232CPort != null)
                    {
                    return RS232CPort.PortName;
                    }
                else 
                    {
                    return "";
                    }
                    
                }
            }

        /// <summary>
        /// 串口是否成功打开的标志
        /// </summary>
        public bool OpenPortSuccess 
            {
            get 
                {
                if (RS232CPort != null)
                    {
                    return RS232CPort.IsOpen;
                    }
                else 
                    {
                    return false;
                    }
                }
            }

        /// <summary>
        /// 串口接收缓冲区大小
        /// </summary>
        public int ReceiveBufferSize 
            {
            get 
                {
                return RS232CPort.ReadBufferSize;
                }

            set 
                {
                RS232CPort.ReadBufferSize = value;
                }
            }

        /// <summary>
        /// 串口发送缓冲区大小
        /// </summary>
        public int SendBufferSize
            {
            get
                {
                return RS232CPort.WriteBufferSize;
                }

            set
                {
                RS232CPort.WriteBufferSize = value;
                }
            }

        /// <summary>
        /// 设置串口进行通讯时发送字符的结尾符号,默认为"回车+换行"
        /// </summary>
        public Endings EndingSetting 
            {

            set 
                {
                
                switch(value)
                    {
                    case Endings.None:
                        Suffix = "";
                        CurrentEndingSetting = Endings.None;
                        RS232CPort.NewLine = "\r\n";
                        break;

                    //\r回车符，\n换行符
                    case Endings.LF:
                        Suffix = "\n";
                        CurrentEndingSetting = Endings.LF;
                        break;

                    case Endings.CR:
                        Suffix = "\r";
                        CurrentEndingSetting = Endings.CR;
                        break;

                    case Endings.CRLF:
                        Suffix = "\r\n";
                        CurrentEndingSetting = Endings.CRLF;
                        RS232CPort.NewLine = "\r\n";
                        break;

                    case Endings.Customerize:

                        if (EndingCustomerizeSetting != "")
                            {
                            Suffix = EndingCustomerizeSetting;
                            CurrentEndingSetting = Endings.Customerize;
                            RS232CPort.NewLine = EndingCustomerizeSetting;
                            }
                        else 
                            {
                            Suffix = "\r\n";
                            CurrentEndingSetting = Endings.CRLF;
                            MessageBox.Show("你已经设置通讯字符串的结束符为自定义，所以参数 'EndingCustomerizeSetting' 不能为空.",
                                "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            }
                        
                        break;

                    default:
                        break;
                    
                    
                    }

                }

            }

        /// <summary>
        /// 获取当前设定的发送字符串的结束符
        /// </summary>
        public Endings ReadEndingSetting 
            {
            get { return CurrentEndingSetting; }
            }
        
        #endregion
        
        //*****************************************************************

        #region "已完成代码"

        //释放相关资源
        /// <summary>
        /// 释放相关资源
        /// </summary>
        public void Dispose()
            {
            try
                {
                newOpenFile.Dispose();
                pcFileSystem=null;
                TempSerialPort.Close();
                tmrAutoSend.Close();
                tmrAutoSend.Dispose();
                TempRichTextBox.Dispose();

                RS232CPort.Close();

                }
            catch (Exception)// ex)
                {
                }
            }

        //将ASCII码转换为16进制码【HEX】
        /// <summary>
        /// 将ASCII码转换为16进制码【HEX】
        /// </summary>
        /// <param name="TargetString">目标字符串</param>
        /// <returns></returns>
        public string StringConvertToHEX(string TargetString)
            {

            if (PasswordIsCorrect == false)
                {
                MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return "";
                }

            string TempResult = "";

            try
                {
                for (Int16 a = 0; a < TargetString.Length; a++)
                    {
                    TempResult += Conversion.Hex(Strings.Asc(Strings.Mid(TargetString, 1 + a, 1)));
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                return "";
                }

            return TempResult;
            }

        //搜索计算机上可用RS232C串口数量
        /// <summary>
        /// 搜索计算机上可用RS232C串口数量
        /// </summary>
        /// <returns></returns>
        public int AvailableRS232CPortCount()
            {

            if (PasswordIsCorrect == false)
                {
                MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
                }

            if (SuccessBuiltNew == false)
                {
                MessageBox.Show("You failed to initialize this class, invalid operation.", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
                }

            try
                {
                AvailablePortNames = System.IO.Ports.SerialPort.GetPortNames();
                AvailableRS232CPorts = AvailablePortNames.GetLength(0);
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                return 0;
                }

            return AvailableRS232CPorts;

            }

        //搜索计算机上可用串口名称字符串数组
        /// <summary>
        /// 搜索计算机上可用串口名称字符串数组
        /// </summary>
        /// <returns>返回搜索到的串口名称字符串数组</returns>
        public string[] SearchAvailableRS232Ports()
            {

            if (PasswordIsCorrect == false)
                {
                MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
                }

            try
                {

                AvailablePortNames = System.IO.Ports.SerialPort.GetPortNames();
                AvailableRS232CPorts = AvailablePortNames.GetLength(0);

                if (AvailableRS232CPorts > 0)
                    {
                    if (AvailableRS232CPorts == 1)
                        {
                        MessageBox.Show("There is  " + AvailableRS232CPorts + "  RS232C port available on this computer.\r\n此电脑上有 " + AvailableRS232CPorts + " 个可用的RS232C串口.");
                        }
                    else
                        {
                        MessageBox.Show("There is  " + AvailableRS232CPorts + "  RS232C ports available on this computer.\r\n此电脑上有 " + AvailableRS232CPorts + " 个可用的RS232C串口.");
                        }
                    }
                else
                    {
                    MessageBox.Show("There is no any RS232C port available on this computer.\r\n此电脑上没有任何可用的RS232C串口.");
                    return null;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                return null;
                }

            return AvailablePortNames;

            }
                
        //创建RS232C类的实例
        /// <summary>
        ///  创建RS232C类的实例
        /// </summary>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public RS232Comm(string DLLPassword)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;

            try
                {

                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan") || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;
                  
                    AvailablePortNames = System.IO.Ports.SerialPort.GetPortNames();
                    AvailableRS232CPorts = AvailablePortNames.GetLength(0);

                    if (AvailableRS232CPorts > 0)
                        {
                        RS232CPort.BaudRate = 115200;
                        RS232CPort.Parity = System.IO.Ports.Parity.None;
                        RS232CPort.Handshake = System.IO.Ports.Handshake.None;
                        RS232CPort.DataBits = 8;
                        RS232CPort.StopBits = System.IO.Ports.StopBits.One;
                        RS232CPort.ReadBufferSize = 1024;
                        RS232CPort.WriteBufferSize = 1024;
                        RS232CPort.Encoding = System.Text.Encoding.UTF8;
                        //RS232CPort.Encoding = System.Text.Encoding.GetEncoding("GB2312");
                        //RS232CPort.Encoding = System.Text.Encoding.GetEncoding(936);
                        }
                    else 
                        {
                        MessageBox.Show("There is no any RS232C port available on this computer.\r\n此电脑上没有任何可用的RS232C串口.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                        }

                    PasswordIsCorrect = true;
                    SuccessBuiltNew = true;
                    TempRichTextBox = null;

                    //添加RS232CPort的DataReceived事件关联函数
                    RS232CPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(RS232CPort_DataReceived); ;

                    //添加RS232CPort的ErrorReceived事件关联函数
                    RS232CPort.ErrorReceived+=new System.IO.Ports.SerialErrorReceivedEventHandler(RS232CPort_ErrorReceived);

                    tmrAutoSend.Elapsed += new System.Timers.ElapsedEventHandler(TmrAutoSend_Elasped);

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
                MessageBox.Show("创建串口类的实例时出现错误！\r\n"+ ex.Message, "错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
                }            

            }

        //创建RS232C类的实例
        /// <summary>
        ///  创建RS232C类的实例
        /// </summary>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        /// <param name="TargetRichTextBoxToShowMessage">需要显示文本信息的RichTextBox控件</param>
        public RS232Comm(string DLLPassword, ref System.Windows.Forms.RichTextBox TargetRichTextBoxToShowMessage)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;

            try
                {

                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan") || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;
                    
                    AvailablePortNames = System.IO.Ports.SerialPort.GetPortNames();
                    AvailableRS232CPorts = AvailablePortNames.GetLength(0);

                    if (AvailableRS232CPorts > 0)
                        {
                        RS232CPort.BaudRate = 115200;
                        RS232CPort.Parity = System.IO.Ports.Parity.None;
                        RS232CPort.Handshake = System.IO.Ports.Handshake.None;
                        RS232CPort.DataBits = 8;
                        RS232CPort.StopBits = System.IO.Ports.StopBits.One;
                        RS232CPort.ReadBufferSize = 1024;
                        RS232CPort.WriteBufferSize = 1024;
                        RS232CPort.Encoding = System.Text.Encoding.UTF8;
                        //RS232CPort.Encoding = System.Text.Encoding.GetEncoding("GB2312");
                        //RS232CPort.Encoding = System.Text.Encoding.GetEncoding(936);
                        }
                    else
                        {
                        MessageBox.Show("There is no any RS232C port available on this computer.\r\n此电脑上没有任何可用的RS232C串口.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                        }

                    PasswordIsCorrect = true;
                    SuccessBuiltNew = true;
                    TempRichTextBox = TargetRichTextBoxToShowMessage;

                    //添加RS232CPort的DataReceived事件关联函数
                    RS232CPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(RS232CPort_DataReceived); ;

                    //添加RS232CPort的ErrorReceived事件关联函数
                    RS232CPort.ErrorReceived += new System.IO.Ports.SerialErrorReceivedEventHandler(RS232CPort_ErrorReceived);

                    tmrAutoSend.Elapsed += new System.Timers.ElapsedEventHandler(TmrAutoSend_Elasped);

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
                MessageBox.Show("创建串口类的实例时出现错误！\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }

            }

        //用预设的参数初始化串口
        /// <summary>
        /// 用预设的参数初始化串口
        /// </summary>
        public void InitialWithoutPara() 
            {

            try
                {

                if (AvailableRS232CPorts > 0)
                    {
                    RS232CPort.BaudRate = 115200;
                    RS232CPort.Parity = System.IO.Ports.Parity.None;
                    RS232CPort.Handshake = System.IO.Ports.Handshake.None;
                    RS232CPort.DataBits = 8;
                    RS232CPort.StopBits = System.IO.Ports.StopBits.One;
                    RS232CPort.ReadBufferSize = 1024;
                    RS232CPort.WriteBufferSize = 1024;
                    RS232CPort.Encoding = System.Text.Encoding.UTF8;
                    //RS232CPort.Encoding = System.Text.Encoding.GetEncoding("GB2312");
                    //RS232CPort.Encoding = System.Text.Encoding.GetEncoding(936);
                    }
                else
                    {
                    ErrorMessage = "There is no any RS232C port available on this computer.";
                    MessageBox.Show("There is no any RS232C port available on this computer.\r\n此电脑上没有任何可用的RS232C串口.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                    }
                  
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }              
            
            }

        //传入参数对串口进行初始化
        /// <summary>
        /// 传入参数对串口进行初始化
        /// </summary>
        /// <param name="ParaSetting">传入的参数</param>
        public void InitialWithPara(Parameters ParaSetting)
            {

            try
                {

                if (AvailableRS232CPorts > 0)
                    {
                    RS232CPort.PortName = ParaSetting.PortNameSetting;
                    RS232CPort.BaudRate = ParaSetting.BaudRateSetting;
                    RS232CPort.Parity = ParaSetting.ParitySetting;
                    RS232CPort.Handshake = ParaSetting.HandShakeSetting;
                    RS232CPort.DataBits = ParaSetting.DataBitsSetting;
                    RS232CPort.StopBits = ParaSetting.StopBitsSetting;
                    RS232CPort.ReadBufferSize = ParaSetting.ReadBufferSizeSetting;
                    RS232CPort.WriteBufferSize = ParaSetting.WriteBufferSizeSetting;
                    RS232CPort.Encoding = ParaSetting.EncodingSetting;
                    }
                else
                    {
                    ErrorMessage = "There is no any RS232C port available on this computer.";
                    MessageBox.Show("There is no any RS232C port available on this computer.\r\n此电脑上没有任何可用的RS232C串口.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                    }

                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }

            }

        //打开串口
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="PortNumber">需要打开的串口编号[1~最大可用数量]</param>
        /// <returns></returns>
        public bool OpenPort(ushort PortNumber) 
            {

            try
                {

                if (AvailableRS232CPorts<=0) 
                    {
                    MessageBox.Show("There is no any RS232C port available.", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                    }

                if (SuccessBuiltNew == true)
                    {

                    if (PortNumber > AvailableRS232CPorts | PortNumber < 0)
                        {
                        MessageBox.Show("There is(are) " + AvailableRS232CPorts +
                            "Port(s）available, the port number you set is over range, please revise it.\r\n有 " +
                            AvailableRS232CPorts + " 个端口可用， 你设置的端口号已经超出范围，请修改参数.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                        }
                    else 
                        {
                        RS232CPort.PortName = AvailablePortNames[PortNumber - 1];

                        if (RS232CPort.IsOpen == true)
                            {
                            //如果被其它软件打开的话，可能会执行关闭时出错
                            if (MessageBox.Show(RS232CPort.PortName + " is used by other software now, do you want to use it right now\r\n " +
                                RS232CPort.PortName + " 现在已经被其它软件打开使用，你确定要马上使用它吗？", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                                {
                                RS232CPort.Close();
                                RS232CPort.Open();
                                }
                            else
                                {
                                ErrorMessage = "打开串口 " + AvailablePortNames[PortNumber - 1] + " 失败，此串口已经被其它软件打开.";
                                return false;
                                }

                            }
                        else
                            {
                            RS232CPort.Open();
                            ErrorMessage = "成功打开串口 " + AvailablePortNames[PortNumber - 1] + ".";
                            return true;
                            }

                        }

                    }
                else 
                    {
                    ErrorMessage = "未建立类的实例，无法打开串口.";
                    MessageBox.Show("未建立类的实例，无法打开串口.", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false ;
                    }

                }
            catch (Exception ex) 
                {
                ErrorMessage = ex.Message;
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false ;                
                }

            return true;
            
            }

        //关闭串口
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public bool ClosePort() 
            {

            try
                {

                if (AvailableRS232CPorts <= 0)
                    {
                    MessageBox.Show("There is no any RS232C port available.", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                    }

                if (SuccessBuiltNew == true)
                    {

                    if (RS232CPort.IsOpen == false)
                        {
                        ErrorMessage = "串口已经被关闭.";
                        return true;
                        }
                    else
                        {
                        RS232CPort.Close();
                        ErrorMessage = "成功关闭串口." + RS232CPort.PortName;
                        return true;
                        }

                    }                    
                else
                    {
                    ErrorMessage = "未建立类的实例，无法关闭串口.";
                    MessageBox.Show("未建立类的实例，无法关闭串口.", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }
            
            }

        //发送字符串到串口
        /// <summary>
        /// 发送字符串到串口
        /// </summary>
        /// <param name="StringToBeSent"></param>
        /// <returns></returns>
        public bool Send(string StringToBeSent) 
            {

            try
                {

                if (AvailableRS232CPorts <= 0)
                    {
                    MessageBox.Show("There is no any RS232C port available.", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                    }

                //如果字符串为空，则退出函数
                if (StringToBeSent == "")
                    {
                    ErrorMessage = "发送的字符串为空，未执行发送.";
                    return false;
                    }

                if (SuccessBuiltNew == true)
                    {

                    if (RS232CPort != null)
                        {

                        if (RS232CPort.IsOpen == true)
                            {

                            SendFileFlag = false;

                            if (RS232CSendMessageInHEX == true)
                                {
                                StringToBeSent = StringConvertToHEX(StringToBeSent);
                                RS232CPort.Write(StringToBeSent + Suffix);
                                ErrorMessage = RS232CPort.PortName + " sent: " + StringToBeSent;
                                StringToBeSent = "";
                                }
                            else
                                {
                                RS232CPort.Write(StringToBeSent + Suffix);
                                ErrorMessage = RS232CPort.PortName + " sent: " + StringToBeSent + Suffix;
                                StringToBeSent = "";
                                }

                            }
                        else
                            {
                            RS232CPort.Open();
                            ErrorMessage = "成功打开串口 " + RS232CPort.PortName;
                            SendFileFlag = false;

                            if (RS232CSendMessageInHEX == true)
                                {
                                StringToBeSent = StringConvertToHEX(StringToBeSent);
                                RS232CPort.Write(StringToBeSent + Suffix);
                                ErrorMessage = RS232CPort.PortName + " sent: " + StringToBeSent;
                                StringToBeSent = "";
                                }
                            else
                                {
                                RS232CPort.Write(StringToBeSent + Suffix);
                                ErrorMessage = RS232CPort.PortName + " sent: " + StringToBeSent + Suffix;
                                StringToBeSent = "";
                                }

                            return true;
                            }

                        }
                    else 
                        {
                        ErrorMessage = "未建立类的实例，无法打开串口进行发送字符.";
                        return false;
                        }

                    }
                else
                    {
                    ErrorMessage = "未建立类的实例，无法打开串口.";
                    MessageBox.Show("未建立类的实例，无法打开串口.", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            return true;
            
            }

        //串口接收信息事件
        /// <summary>
        /// 串口接收信息事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RS232CPort_DataReceived(object sender,
            System.IO.Ports.SerialDataReceivedEventArgs e) 
            {

            byte[] ReadByte;
            string TempStr = "";

            try
                {

                if (ExecuteReadLine == true)
                    {
                    ReceivedString = RS232CPort.ReadLine();
                    ErrorMessage = RS232CPort.PortName + " received: " + ReceivedString;

                    //if (RS232CPort.BytesToRead > 0) 
                    //    {
                    //    ReadByte=new byte[RS232CPort.BytesToRead];                        
                    //    RS232CPort.Read(ReadByte,0,RS232CPort.BytesToRead);
                    //    ReceivedString = System.Text.Encoding.UTF8.GetString(ReadByte);
                    //    ErrorMessage = RS232CPort.PortName + " received: " + ReceivedString;
                    //    }

                    }
                else 
                    {

                    if (RS232CPort.BytesToRead > 0)
                        {
                        ReadByte = new byte[RS232CPort.BytesToRead];
                        RS232CPort.Read(ReadByte, 0, RS232CPort.BytesToRead);

                        for (int i = Information.LBound(ReadByte); i < Information.UBound(ReadByte);i++ ) 
                            {

                            if (Conversion.Hex(ReadByte[i]).Length < 2)
                                {
                                TempStr += "0" + Conversion.Hex(ReadByte[i]);
                                }
                            else 
                                {
                                TempStr += Conversion.Hex(ReadByte[i]);
                                }

                            //在两个16进制字符与下两个16进制字符之间加空格
                            if (i < Information.UBound(ReadByte)) 
                                {
                                TempStr += Strings.Chr(32);
                                }

                            }

                        ReceivedString = TempStr;
                        ErrorMessage = RS232CPort.PortName + " received: " + ReceivedString;

                        }
                    
                    }

                }
            catch (Exception ex) 
                {
                ErrorMessage = ex.Message;
                }
            
            }

        //串口通信错误事件
        /// <summary>
        /// 串口通信错误事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RS232CPort_ErrorReceived(object sender, 
            System.IO.Ports.SerialErrorReceivedEventArgs e)
            {

            try
                {

                //将事件传递到临时串口
                TempSerialPort = (System.IO.Ports.SerialPort)sender;

                switch(e.EventType)
                    {
                    case System.IO.Ports.SerialError.Frame:
                        ErrorMessage = "串口：" + TempSerialPort.PortName + "，硬件检测到一个组帧错误.";
                        break;

                    case System.IO.Ports.SerialError.Overrun:
                        ErrorMessage = "串口：" + TempSerialPort.PortName + "，发生字符缓冲区溢出,下一个字符将丢失.";
                        break;

                    case System.IO.Ports.SerialError.RXOver:
                        ErrorMessage = "串口：" + TempSerialPort.PortName + "，发生输入缓冲区溢出,输入缓冲区空间不足，或在文件尾 (EOF) 字符之后接收到字符.";
                        break;

                    case System.IO.Ports.SerialError.RXParity:
                        ErrorMessage = "串口：" + TempSerialPort.PortName + "，硬件检测到奇偶校验错误.";
                        break;

                    case System.IO.Ports.SerialError.TXFull:
                        ErrorMessage = "串口：" + TempSerialPort.PortName + "，应用程序尝试传输一个字符，但是输出缓冲区已满.";
                        break;
                    
                    }

                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                }

            }

        //开始自动发送，内容为参数【AutoSendMessage】
        /// <summary>
        /// 开始自动发送，内容为参数【AutoSendMessage】
        /// </summary>
        /// <param name="SendInterval">发送的时间间隔，单位：ms</param>
        public void StartAutoSend(int SendInterval) 
            {

            try
                {

                SendFileFlag = false;
                AutoSendFlag = true;
                tmrAutoSend.Interval = SendInterval;
                if(tmrAutoSend.Enabled==false)
                    {
                    tmrAutoSend.Start();
                    }

                }
            catch (Exception)
                {
                } 
            
            }

        //开始自动发送
        /// <summary>
        /// 开始自动发送
        /// </summary>
        /// <param name="SendInterval">发送的时间间隔，单位：ms</param>
        /// <param name="StringToBeSent">需要发送的字符串</param>
        public void StartAutoSend(int SendInterval, string StringToBeSent)
            {

            try
                {

                SendFileFlag = false;
                AutoSendFlag = true;
                AutoSendMessage = StringToBeSent;
                tmrAutoSend.Interval = SendInterval;
                if (tmrAutoSend.Enabled == false)
                    {
                    tmrAutoSend.Start();
                    }

                }
            catch (Exception)
                {
                } 

            }

        //开始自动发送文件
        /// <summary>
        /// 开始自动发送文件
        /// </summary>
        /// <param name="SendInterval">发送的时间间隔，最小100ms，单位：ms</param>
        /// <param name="FileName">需要发送的文件名称数组</param>
        public void StartAutoSendFile(int SendInterval, string[] FileName) 
            {

            try
                {

                if(SendInterval<100)
                    {
                    MessageBox.Show("请将自动发送文件的间隔设置大于100ms，如果文件比较大，请适当增加自动发送的间隔...",
                        "Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                    }

                FileNameToBeSent=new string[FileName.Length];

                for (int a = 0; a < FileName.Length;a++ ) 
                    {

                    if (pcFileSystem.FileSystem.FileExists(FileName[a]) == false)
                        {
                        MessageBox.Show("指定文件不存在，请确保文件名称正确且包含正确路径...", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                        }
                    else 
                        {
                        FileNameToBeSent[a] = FileName[a];
                        }
                    
                    }

                SendFileFlag = true;
                AutoSendFlag = true;
                tmrAutoSend.Interval = SendInterval;
                if(tmrAutoSend.Enabled==false)
                    {
                    tmrAutoSend.Start();
                    }

                }
            catch (Exception)
                {
                } 
            
            }

        //开始自动发送文件,此函数会打开一个选择文件的对话框
        /// <summary>
        /// 开始自动发送文件,此函数会打开一个选择文件的对话框
        /// </summary>
        /// <param name="SendInterval">发送的时间间隔，最小100ms，单位：ms</param>
        public void StartAutoSendFile(int SendInterval)
            {

            try
                {

                if (SendInterval < 100)
                    {
                    MessageBox.Show("请将自动发送文件的间隔设置大于100ms，如果文件比较大，请适当增加自动发送的间隔...",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                    }

                newOpenFile.Filter = "所有文件|*.*";
                newOpenFile.Multiselect = true;

                if(newOpenFile.ShowDialog()==DialogResult.OK)
                    {

                    if (newOpenFile.FileNames.Length > 0)
                        {
                        FileNameToBeSent = new string[newOpenFile.FileNames.Length];
                        for (int a = 0; a < newOpenFile.FileNames.Length; a++)
                            {
                            FileNameToBeSent[a] = newOpenFile.FileNames[a];
                            }
                        SendFileFlag = true;
                        }
                    else 
                        {
                        SendFileFlag = false;
                        return;
                        }
                    
                    }
                else
                    {
                    SendFileFlag = false;
                    return;
                    }

                SendFileFlag = true;
                AutoSendFlag = true;
                tmrAutoSend.Interval = SendInterval;
                if (tmrAutoSend.Enabled == false)
                    {
                    tmrAutoSend.Start();
                    }

                }
            catch (Exception)
                {
                } 

            }

        //private Int16 TempCount = 0;
        private string TempString;
        private double TotalSend = 0.0;
        private bool TempLeft = false;//判断是否有尾数没有发送完成，因为不可能刚刚好被整除
        private int TempQty;
        //private byte[] TempBytes;
        private byte[] Data;

        //System.Timers.Timer启动时钟，以一定间隔执行某个动作【发送字符或者文件的两者之一】
        /// <summary>
        /// System.Timers.Timer启动时钟，以一定间隔执行某个动作【发送字符或者文件的两者之一】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TmrAutoSend_Elasped(object sender, System.Timers.ElapsedEventArgs e)
            {
            //基于服务器的 Timer 是为在多线程环境中用于辅助线程而设计的
            try
                {

                if(AutoSendFlag==true)                  
                    {
                    //发送文件
                    if (SendFileFlag == true)
                        {

                        //TempCount = 0;

                        if (FileNameToBeSent.Length > 0)
                            {

                            for (int a = 0; a < FileNameToBeSent.Length;a++ ) 
                                {

                                TempString = "";

                                try 
                                    {

                                    Data = pcFileSystem.FileSystem.ReadAllBytes(FileNameToBeSent[a]);
                                    ErrorMessage = "File: " + FileNameToBeSent[a] + ". total: " + Data.Length + " bytes...";
                                    TempString = System.Text.Encoding.UTF8.GetString(Data);
                                    TotalSend = 0.0;
                                    TotalSend = Data.Length / RS232CPort.WriteBufferSize;
                                    TempLeft = false;
                                    TempQty = Convert.ToUInt16(TotalSend);

                                    if (TempQty > 1)
                                        {
                                        //发送的字节大于发送缓冲区的大小，需要进行切割，分开发送并中间要延时
                                        for (int b = 0; b < TempQty;b++ ) 
                                            {

                                            if (b == 0)
                                                {
                                                RS232CPort.Write(Data, 0, RS232CPort.WriteBufferSize);
                                                }
                                            else 
                                                {
                                                RS232CPort.Write(Data, b*RS232CPort.WriteBufferSize-1,RS232CPort.WriteBufferSize);
                                                }

                                            while(RS232CPort.WriteTimeout>0)
                                                {
                                                System.Windows.Forms.Application.DoEvents();
                                                }

                                            System.Threading.Thread.Sleep(5);

                                            if((b+1)*RS232CPort.WriteBufferSize<Data.Length)
                                                {
                                                TempLeft = true;
                                                }

                                            }

                                        //如果有尾数，则将尾数部分发送出去
                                        if(TempLeft==true)
                                            {
                                            RS232CPort.Write(Data, (TempQty * RS232CPort.WriteBufferSize - 1), 
                                                          (Data.Length - TempQty * RS232CPort.WriteBufferSize + 1));
                                            ErrorMessage = (TempQty + 1) + ", sent " + (Data.Length - TempQty * RS232CPort.WriteBufferSize) 
                                                + "bytes...";
                                            System.Threading.Thread.Sleep(5);
                                            TempLeft = false;
                                            }

                                        }
                                    else 
                                        {
                                        //发送的字节小于等于发送缓冲区的大小
                                        RS232CPort.Write(Data, 0, Data.Length);
                                        ErrorMessage = "Sent " + Data.Length + "bytes...";
                                        }

                                    }
                                catch(Exception ex)
                                    {
                                    ErrorMessage = ex.Message;
                                    }

                                }

                            }
                        else 
                            {
                            tmrAutoSend.Stop();
                            return;
                            }


                        }
                    else 
                        {
                        //发送字符
                        if(AutoSendMessage!="")
                            {

                            TempString = AutoSendMessage;

                            if(RS232CPort!=null)
                                {

                                if(RS232CPort.IsOpen==false)
                                    {
                                    RS232CPort.Open();
                                    }

                                if (RS232CSendMessageInHEX == true)
                                    {
                                    TempString = StringConvertToHEX(TempString + Suffix);
                                    RS232CPort.Write(TempString);
                                    }
                                else 
                                    {
                                    RS232CPort.Write(TempString + Suffix);
                                    }

                                }

                            }
                        
                        }

                    }
                
                }
            catch (Exception ex) 
                {
                ErrorMessage = ex.Message;
                tmrAutoSend.Stop();                
                }  
            
            }

        //停止自动发送
        /// <summary>
        /// 停止自动发送
        /// </summary>
        public void StopAutoSend()
            {

            try
                {

                AutoSendFlag = false;
                tmrAutoSend.Stop();

                }
            catch (Exception)
                {
                }

            }

        #endregion

        }//class

    }//namespace