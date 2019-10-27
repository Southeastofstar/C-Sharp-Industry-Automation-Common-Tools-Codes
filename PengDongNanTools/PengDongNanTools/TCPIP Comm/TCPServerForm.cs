#region "using"

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.IO;
using System.Threading;
using Microsoft.VisualBasic;

#endregion

//VB中数组定义 dim a(2) as string, 数组的长度为3，下标从0~2；
//C#中数组定义 string[] a=new string[2]，数组的长度为2，下标从0~1；

namespace PengDongNanTools
    {

    //TCP/IP服务器监听通讯界面
    /// <summary>
    /// TCP/IP服务器监听通讯界面
    /// </summary>
    public partial class TCPServerForm : Form
        {

        #region "变量定义"

        private int CurrentSelectedItem;

        private string SingleServerSend = "";
        private string MultiServerSend = "";

        frmAbout HelpDlg;

        /// <summary>
        /// 以太网服务器发送缓冲区大小[字节]
        /// </summary>
        public Int32 SendBufferSize = 1024;

        /// <summary>
        /// 以太网服务器接收缓冲区大小[字节]
        /// </summary>
        public Int32 ReceiveBufferSize = 1024;

        private System.Timers.Timer tmrAutoSend = new System.Timers.Timer();

        ListViewOperation LV = new ListViewOperation("彭东南");
        Microsoft.VisualBasic.Devices.Computer PC = new Microsoft.VisualBasic.Devices.Computer();

        private bool SuccessBuiltNew = false;
        private bool PasswordIsCorrect = false;

        /// <summary>
        /// 是否接收GB2312编码
        /// </summary>
        public bool GB2312Coding = false;

        CommonFunction FC = new CommonFunction("彭东南");

        //private bool[] TempAutoSendFlag, TempSendFileFlag, TempSendHEXFlag,
        //    TempReadLine, TempCustomSuffixFlag, TempErrorFlag;
        //private Int32[] TempAutoSendInterval;
        private string[] TempCustomSuffix;//, TempSendText, ReceivedString;
        //private Endings[] TempEnding;
        //private bool[] TempGB2312Flag;

        /// <summary>
        /// 更新信息至文本框时是否显示日期和时间，默认为True
        /// </summary>
        public bool ShowDateTimeForMessage = true;

        /// <summary>
        /// 在关闭窗体时是否只是隐藏窗体【默认True】
        /// </summary>
        public bool JustHideFormAtClosing = true;

        /// <summary>
        /// 在关闭窗体时是否提示【默认True】
        /// </summary>
        public bool ShowPromptAtClosing = true;

        /// <summary>
        /// 是否更新显示相同信息
        /// </summary>
        public bool UpdatingSameMessage = true;

        private System.Threading.Thread ThreadMonitorMultiListen;
        private bool StartMultiListen = false;
        private OpenFileDialog newOpenFile = new OpenFileDialog();
        private SaveFileDialog SaveLogAsTXTFile = new SaveFileDialog();

        bool AutoResendFlag = false;
        bool SendFileFlag = false;
        string TempTestIPAddress;

        SimpleTCPIPServer[] Server;
        //SimpleTCPIPServer SingleServer;

        //System.Timers.Timer NewTime = new System.Timers.Timer();

        /// <summary>
        /// 服务器发送心跳信号的字符串内容
        /// </summary>
        private string ServerHeartBeatPulseText = "";

        /// <summary>
        /// 单端口服务器是否发送16进制内容
        /// </summary>
        public bool SingleServerSendMessageInHEX = false;

        /// <summary>
        /// 多端口服务器是否发送16进制内容
        /// </summary>
        public bool MultiServerSendMessageInHEX = false;

        /// <summary>
        /// 是否已经完成IP地址搜索
        /// </summary>
        public bool SearchIPAddressDone = false;

        /// <summary>
        /// 已经找到的IP地址清单
        /// </summary>
        public string[] FoundIPAddressList;

        //private Endings ServerEndings;
        //private string[] IPAddressList;
        private int CurrentIndexForServerListView = 0;
        //private ushort[] ServerListenPortArray;
        //private bool[] ServerTestedArray;
        //private string[] ServerEndingsArray;

        //private ushort CurrentIndexOfServerListView = 0;

        ///// <summary>
        ///// 用于临时存储服务器监听端口的参数列表，以应对增加自定义监听端口的情况
        ///// </summary>
        //private ushort[] TempFeedBackServerListenPort;

        /// <summary>
        /// 用于保存返回的服务器参数
        /// </summary>
        private SaveServerParameters[] FeedBackServerParameters;

        private bool ServerWorking = false;

        private string SuffixForServerSendingMessage;
        
        /// <summary>
        /// 界面是否用中文显示
        /// </summary>
        public bool ChineseLanguage = true;

        private Endings CurrentEndingSetting = Endings.CRLF;

        /// <summary>
        /// 自定义的结束符[用于发送时在发送的字符串最后加上]
        /// </summary>
        public string EndingCustomerizeSetting;

        //private bool ServerTestedOK = false;

        /// <summary>
        /// 错误和返回信息
        /// </summary>
        public string ErrorMessage = "";

        private string TempErrorMessage = "";

        /// <summary>
        /// 是否更新显示同样的信息【有些错误一直存在，线程会一直更新】
        /// </summary>
        public bool ShowSameErrorMessage = true;

        private Thread ServerAcceptClientConnectionThread;

        /// <summary>
        /// 服务器收到客服端发送的字符串
        /// </summary>
        public string ReceivedDataFromClient = "";

        /// <summary>
        /// 服务器发送给客服端的字符串
        /// </summary>
        public string SendDataToClient = "";

        private TcpListener TCPServerStation;
        private IPAddress TCPServerIPAddress = IPAddress.Parse("127.0.0.1");
        private int TCPServerListeningPort = 8888;
        private TcpClient ClientToBeAcceptedByServer;
        private NetworkStream TCPServerStream;

        ///// <summary>
        ///// 以太网服务器端口接受客户端口标志
        ///// </summary>
        //private bool ServerAcceptClientRequestSuccess = false;

        //利用委托和代理进行跨线程安全操作控件，以此避免跨线程操作异常
        //*****************************
        private delegate void AddTextDelegate(string TargetText);

        #endregion
        
        #region "属性和数据结构"

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
            CR = 2,

            /// <summary>
            /// 回车换行符
            /// </summary>
            CRLF = 3,

            /// <summary>
            /// 自定义
            /// </summary>
            Customerize = 4
            }

        /// <summary>
        /// 设置与客户端进行通讯时发送字符的结尾符号
        /// </summary>
        public Endings EndingSetting
            {
            set
                {
                switch (value)
                    {
                        //\r回车符，\n换行符
                    case Endings.None:
                        SuffixForServerSendingMessage = "";
                        CurrentEndingSetting = Endings.None;
                        break;

                    case Endings.CR:
                        SuffixForServerSendingMessage = "\r";
                        CurrentEndingSetting = Endings.CR;
                        break;

                    case Endings.LF:
                        SuffixForServerSendingMessage = "\n";
                        CurrentEndingSetting = Endings.LF;
                        break;

                    case Endings.CRLF:
                        SuffixForServerSendingMessage = "\r\n";
                        CurrentEndingSetting = Endings.CRLF;
                        break;

                    case Endings.Customerize:
                        if (EndingCustomerizeSetting != "")
                            {
                            SuffixForServerSendingMessage = EndingCustomerizeSetting;
                            CurrentEndingSetting = Endings.Customerize;
                            }
                        else
                            {
                            SuffixForServerSendingMessage = "\r\n";
                            CurrentEndingSetting = Endings.CRLF;
                            MessageBox.Show("You already set the Ending as ‘Customerize’, so the 'EndingCustomerizeSetting' can't be empty, please revise it.\r\n你已经设置通讯字符串的结束符为自定义，所以参数 'EndingCustomerizeSetting' 不能为空.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        break;
                    }
                }
            }

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "软件作者：彭东南, southeastofstar@163.com"; }
            }

        /// <summary>
        /// 保存服务器参数的数据结构
        /// </summary>
        public struct SaveServerParameters
            {

            /// <summary>
            /// 目标服务器端口
            /// </summary>
            public ushort TargetServerPort;

            /// <summary>
            /// 是否已经测试过
            /// </summary>
            public bool Tested;

            /// <summary>
            /// 结束符设置
            /// </summary>
            public Endings EndingSetting;
            }

        /// <summary>
        /// 待处理事项，后续再添加代码
        /// </summary>
        public string PendingIusse
            {
            get { return ""; }
            }

        /// <summary>
        /// 服务器发送心跳信号的字符串内容
        /// </summary>
        public string ServerHeartBeatText
            {
            get { return ServerHeartBeatPulseText; }

            set
                {
                if (value == "")
                    {
                    MessageBox.Show("服务器心跳信号的内容不能为空.");
                    }
                else
                    {
                    ServerHeartBeatPulseText = value;
                    }
                }
            }

        //服务器发送心跳信号的间隔【单位：毫秒(ms)】
        /// <summary>
        /// 服务器发送心跳信号的间隔【单位：毫秒(ms)】
        /// </summary>
        public double IntervalForServerHeartBeat
            {
            get { return this.tmrAutoSend.Interval; }

            set
                {
                if (value == 0)
                    {
                    MessageBox.Show("发送心跳信号的间隔必须大于0【单位：毫秒(ms)】");
                    return;
                    }
                tmrAutoSend.Interval = value;
                }

            }

        /// <summary>
        /// 服务器是否发送心跳信号
        /// </summary>
        public bool ServerEnableHeartBeat
            {
            get { return tmrAutoSend.Enabled; }

            set
                {
                tmrAutoSend.Enabled = value;
                }
            }

        /// <summary>
        /// 获取当前设定的发送字符串的结束符
        /// </summary>
        public Endings GetEndingSetting 
            {
            get { return CurrentEndingSetting; }            
            }

        #endregion
        
        #region "窗体事件"

        private void TCPServerForm_FormClosing(object sender, FormClosingEventArgs e)
            {

            try
                {

                if (JustHideFormAtClosing == true)
                    {
                    e.Cancel = true;
                    //AutoResendFlag = false;
                    //tmrAutoSend.Stop();
                    this.Hide();
                    return;
                    }

                //bool TempResult = false;
                if (ShowPromptAtClosing == true)
                    {
                    if (MessageBox.Show("Are you sure to exit?\r\n确定要退出吗？", "请确认",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                        e.Cancel = true;
                        return;
                        }

                    }

                AutoResendFlag = false;
                tmrAutoSend.Stop();

                if (newOpenFile != null)
                    {
                    newOpenFile.Dispose();
                    newOpenFile = null;
                    }

                if (SaveLogAsTXTFile != null)
                    {
                    SaveLogAsTXTFile.Dispose();
                    SaveLogAsTXTFile = null;
                    }

                //释放客户端的相关资源
                if (TCPServerStream != null)
                    {
                    TCPServerStream.Close();
                    TCPServerStream = null;
                    }

                if (ClientToBeAcceptedByServer != null)
                    {
                    ClientToBeAcceptedByServer.Close();
                    ClientToBeAcceptedByServer = null;
                    }

                //释放客户端线程
                if (ServerAcceptClientConnectionThread != null)
                    {
                    ServerAcceptClientConnectionThread.Abort();
                    ServerAcceptClientConnectionThread = null;
                    }

                if (ThreadMonitorMultiListen != null)
                    {
                    ThreadMonitorMultiListen.Abort();
                    ThreadMonitorMultiListen = null;
                    }

                if (Server != null)
                    {
                    for (int a = 0; a < Server.Length; a++)
                        {
                        Server[a].Close();
                        Server[a] = null;
                        }
                    }

                }
            catch (Exception)//Exception ex)
                {
                //MessageBox.Show(ex.Message);
                }

            }

        private void txtSingleServerSend_TextChanged(object sender, EventArgs e)
            {

            }

        private void txtMultiServerSend_TextChanged(object sender, EventArgs e)
            {

            }

        private void rtbTCPIPHistory_TextChanged(object sender, EventArgs e)
            {

            try
                {
                //如果文本框的字符串超过一定数量，就保存之后再清除
                if (rtbTCPIPHistory.TextLength > 1000000)
                    {

                    // 's'--将日期和时间格式化为可排序的索引。例如 2008-03-12T11:07:31。
                    //     s 字符以用户定义的时间格式显示秒钟。
                    // 'u'--将日期和时间格式化为 GMT 可排序索引。如 2008-03-12 11:07:31Z。

                    string TempFileName = PC.FileSystem.CurrentDirectory + "\\TCPIPServerLog-" +
                        Strings.Format(DateTime.Now, "yyyy'年'MM'月'dd'日' HH'点'mm'分'ss'秒'") + ".txt"; // "yyyy-MM-dd HH%h-mm%m-ss%s") '"s")

                    rtbTCPIPHistory.SaveFile(TempFileName, RichTextBoxStreamType.TextTextOleObjs);
                    //AddText("成功保存运行记录至文件: " + TempFileName);

                    rtbTCPIPHistory.Text = "";
                    AddText("历史记录太多，在清除之前已经备份保存到文件：" + TempFileName);
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                }


            }

        private void txtSingleServerSend_KeyDown(object sender, KeyEventArgs e)
            {
            SingleServerSend = txtSingleServerSend.Text;
            }

        private void txtMultiServerSend_KeyDown(object sender, KeyEventArgs e)
            {
            MultiServerSend = txtMultiServerSend.Text;
            }

        private void txtCustomizedEndingCodeForServer_TextChanged(object sender, EventArgs e)
            {

            }

        private void txtCustomizedEndingCodeForServer_KeyDown(object sender, KeyEventArgs e)
            {
            SuffixForServerSendingMessage = txtCustomizedEndingCodeForServer.Text;
            }

        private void cmbSuffixForServer_SelectedIndexChanged(object sender, EventArgs e)
            {

            if (cmbSuffixForServer.SelectedIndex != -1)
                {
                if (cmbSuffixForServer.SelectedIndex == 4)
                    {
                    txtCustomizedEndingCodeForServer.Enabled = true;
                    txtCustomizedEndingCodeForServer.Visible = true;
                    }
                else
                    {
                    txtCustomizedEndingCodeForServer.Enabled = false;
                    txtCustomizedEndingCodeForServer.Visible = false;
                    }
                }

            }

        private void TCPServerForm_Load(object sender, EventArgs e)
            {

            try
                {

                HelpDlg = new frmAbout();

                tmrAutoSend.Elapsed += new System.Timers.ElapsedEventHandler(tmrAutoSend_Elapsed);
                this.txtServerListenPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtServerListenPort_KeyPress);
                this.txtServerListenPort.KeyDown += new KeyEventHandler(txtServerListenPort_KeyDown);
                this.ServerListView2.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(ServerListView2_ItemSelectionChanged);

                //            命名空间:       System.Net()
                //            程序集：   System在 System.dll 中
                //            语法()
                //            Public Shared Sub SetTcpKeepAlive(ByVal enabled As Boolean, ByVal keepAliveTime As Integer, ByVal keepAliveInterval As Integer)
                //            参数()
                //            enabled()
                //            类型:     System.Boolean()
                //            如果设置为 true，则将使用指定的 keepAliveTime 和 keepAliveInterval 值启用 TCP 连接上的 TCP keep-alive 选项。 
                //            如果设置为 false，则将禁用 TCP keep-alive 选项，并忽略剩余参数。
                //            默认值为 false。
                //            keepAliveTime()
                //            类型:     System.Int32()
                //            指定发送第一个 keep-alive 数据包之前没有活动的超时时间（以毫秒为单位）。
                //            该值必须大于 0。如果传递的值小于或等于零，则会引发 ArgumentOutOfRangeException。 
                //            keepAliveInterval()
                //            类型:     System.Int32()
                //            指定当未收到确认消息时发送连续 keep-alive 数据包之间的间隔（以毫秒为单位）。
                //            该值必须大于 0。如果传递的值小于或等于零，则会引发 ArgumentOutOfRangeException。 
                System.Net.ServicePointManager.SetTcpKeepAlive(true, 1000, 1000);
                 
                //设定界面的语言
                ChineseLanguage = true;
                LanguageForUserInterface(true);

                //**************************
                ServerListView2.BeginUpdate();
                ServerListView2.View = System.Windows.Forms.View.Details;
                ServerListView2.Columns[0].TextAlign = HorizontalAlignment.Center;
                ServerListView2.Columns[0].Width = 30;
                ServerListView2.Columns[1].TextAlign = HorizontalAlignment.Center;
                ServerListView2.Columns[2].TextAlign = HorizontalAlignment.Center;
                ServerListView2.Columns[3].TextAlign = HorizontalAlignment.Center;
                ServerListView2.Items.Clear();

                for (int a = 0; a <= 6; a++)
                    {
                    ListViewItem AddNewItemsForServerPort = new ListViewItem();
                    AddNewItemsForServerPort.SubItems.Add("88" + a.ToString());
                    AddNewItemsForServerPort.SubItems.Add("False");
                    AddNewItemsForServerPort.SubItems.Add("CRLF");
                    ServerListView2.Items.Add(AddNewItemsForServerPort);
                    ServerListView2.Items[a].Checked = true;
                    }
                ServerListView2.EndUpdate();
                //******************************

                TempTestIPAddress = GetLocalIP4Address();
                txtLocalServerIPAddress.Text = TempTestIPAddress; // GetLocalIP4Address();
                txtServerListenPort.Text = "8888";

                //监控网络的断开和连接
                AddNetworkChange_EventHandler();

                chkSingleServerSendHEX.Checked = false;
                this.tmrAutoSend.Interval = 100;

                btnListen.Enabled = true;
                btnCloseServer.Enabled = false;
                btnServerSendToClient.Enabled = false;
                cmbSuffixForServer.SelectedIndex = 3;
                lblCurrentIndexForServerListView.Text = "";
                SuffixForServerSendingMessage = "\r\n";

                //必须等到数据已经添加到列表中才能执行此聚焦的操作，否则出错
                //ServerListView2.Items[0].Focused = true;
                ServerListView2.Items[0].Selected = true;

                //Server = null;
                //ThreadMonitorMultiListen = new Thread(MonitorMultiListen);
                //ThreadMonitorMultiListen.IsBackground = true;
                //ThreadMonitorMultiListen.Start();

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }            

            }

        private void btnListen_Click(object sender, EventArgs e)
            {

            try
                {

                ServerWorking = true;
                //ServerTestedOK = false;

                //IPAddress TempIPAddress = null;
                //string[] GetCorrectIPAddress=new string[3];
                //Int16[] TempGetIPAddress=new Int16[3];

                //判断输入的服务器IP地址是否正确
                if (VerifyIPAddress(txtLocalServerIPAddress.Text) == null)
                    {
                    MessageBox.Show("The IP address for the Server is not correct, please retry." +
                        "\r\n服务器的IP不对，请关闭软件后重新打开再尝试");
                    txtLocalServerIPAddress.Focus();
                    return;
                    }

                if (Convert.ToInt32(txtServerListenPort.Text) < 0
                    | Convert.ToInt32(txtServerListenPort.Text) > 65535)
                    {
                    MessageBox.Show("The listening port for the Server is not correct, please retry." +
                        "\r\n服务器的监听端口范围不正确，正确范围：0~65535");
                    return;
                    }
                else
                    {
                    TCPServerListeningPort = Convert.ToInt32(txtServerListenPort.Text);
                    }

                //发送文本字符
                if (cmbSuffixForServer.SelectedIndex == 4 &
                    txtCustomizedEndingCodeForServer.Text == "")
                    {
                    AddText("自定义的结束符不能为空，请输入相应的文本。");
                    MessageBox.Show("The text for the Ending can't be empty, please enter the text you want to be set." +
                       "\r\n自定义的结束符不能为空，请输入相应的文本。");
                    return;
                    }
                else if (cmbSuffixForServer.SelectedIndex == 4 &
                    txtCustomizedEndingCodeForServer.Text != "")
                    {
                    SuffixForServerSendingMessage = txtCustomizedEndingCodeForServer.Text;
                    }

                bool AlreadyListenedByMultiListener = false;

                //如果在已经勾选的项中找到，且已经在进行多端口监听，
                //则不能进行此端口的监听，以免发生错误
                if (ServerListView2.CheckedItems.Count > 0)
                    {
                    for (int a = 0; a < ServerListView2.CheckedItems.Count; a++)
                        {
                        AlreadyListenedByMultiListener = (ServerListView2.CheckedItems[a].SubItems[1].Text == txtServerListenPort.Text) ? true : false;
                        break;
                        }

                    if (StartMultiListen == true & AlreadyListenedByMultiListener == true)
                        {
                        AddText("此端口已经在多端口监控中被占用，请选择其它端口进行监听...");
                        MessageBox.Show("This port has been listened already, please select other port to be listen." +
                           "\r\n此端口已经在多端口监控中被占用，请选择其它端口进行监听...");
                        return;
                        }
                    }


                TCPServerStation = null;

                if (ServerAcceptClientConnectionThread == null)
                    {
                    ServerAcceptClientConnectionThread = new Thread(StartServerListeningThread);
                    ServerAcceptClientConnectionThread.IsBackground = true;
                    ServerAcceptClientConnectionThread.Start();
                    }

                btnListen.Enabled = false;
                btnCloseServer.Enabled = true;
                btnServerSendToClient.Enabled = true;
                AddText("启动服务器...");

                btnAddPortForServer.Enabled = false;
                txtServerListenPort.Enabled = false;

                //只有发送成功了，才能确定是以太网OK.[服务器不需要]
                //ServerTestedOK = true;

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        private void btnMultiListen_Click(object sender, EventArgs e)
            {

            try
                {

                if (ServerListView2.CheckedItems.Count < 2)
                    {
                    MessageBox.Show("Please check the port you want to listen first before you execute the listening operation." +
                       "\r\n请在执行监听多端口前选中两个及两个以上的端口号。");
                    return;
                    }

                if (StartMultiListen == true)
                    {

                    if (MessageBox.Show("Are you sure to stop the multi-port listening?" +
                        "\r\n确定要停止多端口监听吗？", "Notice", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.No)
                        {
                        return;
                        }
                    else
                        {

                        StartMultiListen = false;
                        ThreadMonitorMultiListen.Abort();
                        ThreadMonitorMultiListen = null;

                        if (Server != null)
                            {

                            for (int a = 0; a < Server.Length; a++)
                                {
                                try
                                    {
                                    Server[a].Close();
                                    Server[a] = null;
                                    AddText("关闭服务器监听端口线程：" + (a + 1));
                                    }
                                catch (Exception ex)
                                    {
                                    AddText("关闭服务器监听端口线程 " + (a + 1) + " 错误：" + ex.Message);
                                    }
                                }

                            }

                        if (ChineseLanguage == true)
                            {
                            btnMultiListen.Text = "开多端口监听";
                            }
                        else
                            {
                            btnMultiListen.Text = "Multi Listen";
                            }

                        AddText("停止监听多端口...");

                        if (ServerWorking == false)
                            {
                            btnServerSendToClient.Enabled = false;
                            }

                        }

                    }
                else //开始监听多端口
                    {

                    //发送文本字符
                    if (cmbSuffixForServer.SelectedIndex == 4 &
                        txtCustomizedEndingCodeForServer.Text == "")
                        {
                        AddText("自定义的结束符不能为空，请输入相应的文本。");
                        SuffixForServerSendingMessage = "";
                        //MessageBox.Show("The text for the Ending can't be empty, please enter the text you want to be set." +
                        //   "\r\n自定义的结束符不能为空，请输入相应的文本。");
                        //return;
                        }
                    else if (cmbSuffixForServer.SelectedIndex == 4 &
                        txtCustomizedEndingCodeForServer.Text != "")
                        {
                        SuffixForServerSendingMessage = txtCustomizedEndingCodeForServer.Text;
                        }

                    //0~4: None, LF, CR, CR+LF,CustomizedEnding
                    TempCustomSuffix=new string[ServerListView2.CheckedItems.Count];
                    //for(int x=0;x<ServerListView2.CheckedItems.Count;x++)
                    //    {
                    //    switch (ServerListView2.CheckedItems[x].SubItems[3].Text) 
                    //        {
                            
                    //        case "":
                    //            TempCustomSuffix[x] = "";
                    //            break;

                    //        case "CR":
                    //            TempCustomSuffix[x] = "\r";
                    //            break;

                    //        case "LF":
                    //            TempCustomSuffix[x] = "\n";
                    //            break;

                    //        case "CRLF":
                    //            TempCustomSuffix[x] = "\r\n";
                    //            break;

                    //        case "CUS":
                    //            //待优化：目前没有做到记忆，因为多端口监听的变化不定，还没有找到合适的方法对应处理
                    //            TempCustomSuffix[x] = txtCustomizedEndingCodeForServer.Text;;
                    //            break;

                    //        }
                    //    }

                    Server = null;
                    ThreadMonitorMultiListen = new Thread(MonitorMultiListen);
                    ThreadMonitorMultiListen.IsBackground = true;
                    ThreadMonitorMultiListen.Start();

                    Server = new SimpleTCPIPServer[ServerListView2.CheckedItems.Count];
                    for (int a = 0; a < Server.Length; a++)
                        {
                        try
                            {
                            Server[a] = new SimpleTCPIPServer(Convert.ToUInt16(ServerListView2.CheckedItems[a].SubItems[1].Text), "彭东南");

                            switch (ServerListView2.CheckedItems[a].SubItems[3].Text)
                                {

                                case "":
                                    TempCustomSuffix[a] = "";
                                    Server[a].Suffix = "";
                                    break;

                                case "CR":
                                    TempCustomSuffix[a] = "\r";
                                    Server[a].Suffix = "\r";
                                    break;

                                case "LF":
                                    TempCustomSuffix[a] = "\n";
                                    Server[a].Suffix = "\n";
                                    break;

                                case "CRLF":
                                    TempCustomSuffix[a] = "\r\n";
                                    Server[a].Suffix = "\r\n";
                                    break;

                                case "CUS":
                                    //待优化：目前没有做到记忆，因为多端口监听的变化不定，还没有找到合适的方法对应处理
                                    TempCustomSuffix[a] = txtCustomizedEndingCodeForServer.Text;
                                    Server[a].Suffix = txtCustomizedEndingCodeForServer.Text;
                                    break;

                                }

                            Server[a].Start();
                            AddText("启动多端口监听服务器 " + (a + 1) + " 线程成功...");
                            }
                        catch (Exception ex)
                            {
                            AddText("启动多端口监听服务器 " + (a + 1) + " 线程错误：" + ex.Message);
                            }
                        }

                    if (ChineseLanguage == true)
                        {
                        btnMultiListen.Text = "关多端口监听";
                        }
                    else
                        {
                        btnMultiListen.Text = "Close Multi";
                        }

                    AddText("启动监听多端口...");
                    StartMultiListen = true;
                    btnServerSendToClient.Enabled = true;

                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        private void btnServerSendToClient_Click(object sender, EventArgs e)
            {

            if (AutoResendFlag == true)
                {
                MessageBox.Show("Now the software is under auto-send mode, please change it if you want to send it manually." +
                    "\r\n软件正处于自动发送模式， 请先切换到手动发送模式。");
                return;
                }

            try
                {

                //发送文件
                if (SendFileFlag == true)
                    {
                    byte[] Data;
                    string TempStr = "";

                    //多端口服务器发送文件的代码
                    if (StartMultiListen == true)
                        {

                        if (Server == null)
                            {
                            AddText("多端口监听服务器未成功创建，请查找原因后再尝试。");
                            MessageBox.Show("The servers have not been built successfully yet, please check the reason and retry." +
                                "\r\n多端口监听服务器未成功创建，请查找原因后再尝试。");
                            }
                        else
                            {
                            for (int a = 0; a < Server.Length; a++)
                                {
                                if (Server[a].Connected == false)
                                    {
                                    continue;
                                    }

                                if (newOpenFile.FileNames.Length > 0)
                                    {
                                    for (int b = 0; b < newOpenFile.FileNames.Length; b++)
                                        {
                                        try
                                            {
                                            Data = PC.FileSystem.ReadAllBytes(newOpenFile.FileNames[b]);
                                            AddText("File: " + newOpenFile.FileNames[b] + ", total: " + Data.Length + " bytes...");

                                            if (GB2312Coding == true)
                                                {
                                                TempStr = System.Text.Encoding.GetEncoding(936).GetString(Data);
                                                }
                                            else 
                                                {
                                                TempStr = System.Text.Encoding.UTF8.GetString(Data);
                                                }                                            

                                            if (Server[a].Send(TempStr))// + TempCustomSuffix[a])) //SuffixForServerSendingMessage
                                                {
                                                AddText("Server " + Server[a].ListenPort + " sent file " + newOpenFile.FileNames[b] + " to client IP: "
                                                    + Server[a].ClientIPAddress + ", port: " + Server[a].ClientPort + "...");
                                                }
                                            else
                                                {
                                                AddText("服务器发送文件到端口 " + Server[a].ListenPort + " 失败");
                                                }
                                            }
                                        catch (Exception ex)
                                            {
                                            AddText(ex.Message);
                                            }
                                        }
                                    }
                                }

                            }

                        }

                    //单端口服务器发送文件的代码
                    if (ClientToBeAcceptedByServer.Connected==true)// (TCPServerStream != null)
                        {
                        if (newOpenFile.FileNames.Length > 0)
                            {
                            for (int b = 0; b < newOpenFile.FileNames.Length; b++)
                                {
                                try
                                    {
                                    Data = PC.FileSystem.ReadAllBytes(newOpenFile.FileNames[b]);
                                    AddText("File: " + newOpenFile.FileNames[b] + ", total: " + Data.Length + " bytes...");

                                    if (GB2312Coding == true)
                                        {
                                        TempStr = System.Text.Encoding.GetEncoding(936).GetString(Data);
                                        }
                                    else
                                        {
                                        TempStr = System.Text.Encoding.UTF8.GetString(Data);
                                        }  

                                    TCPServerStream.Write(System.Text.Encoding.UTF8.GetBytes(TempStr + SuffixForServerSendingMessage), 0,
                                                      System.Text.Encoding.UTF8.GetBytes(TempStr + SuffixForServerSendingMessage).Length);
                                    AddText("Server sent file " + newOpenFile.FileNames[b] + " to client IP: " 
                                        + FC.GetRemoteIP(ClientToBeAcceptedByServer) + ", port: " + FC.GetRemotePort(ClientToBeAcceptedByServer) +"...");
                                    }
                                catch (Exception ex)
                                    {
                                    AddText(ex.Message);
                                    }
                                }
                            }
                        }
                    else
                        {
                        AddText("单端口服务器没有启动工作或者客户端已经断开了连接！");
                        }

                    }
                else
                    {
                    //发送文本字符
                    if (cmbSuffixForServer.SelectedIndex == 4 &
                        txtCustomizedEndingCodeForServer.Text == "")
                        {
                        AddText("自定义的结束符不能为空，请输入相应的文本。");
                        //MessageBox.Show("The text for the Ending can't be empty, please enter the text you want to be set." +
                        //   "\r\n自定义的结束符不能为空，请输入相应的文本。");
                        //return;
                        }
                    else if (cmbSuffixForServer.SelectedIndex == 4 &
                        txtCustomizedEndingCodeForServer.Text == "")
                        {
                        SuffixForServerSendingMessage = txtCustomizedEndingCodeForServer.Text;
                        }

                    if (txtSingleServerSend.Text == "")
                        {
                        AddText("单端口发送的文本不能为空白，请输入文本内容。");
                        txtSingleServerSend.Focus();
                        }

                    if (txtMultiServerSend.Text == "")
                        {
                        AddText("多端口发送的文本不能为空白，请输入文本内容。");
                        txtMultiServerSend.Focus();
                        }

                    string TempString = "";
                    if (txtMultiServerSend.Text != "")
                        {

                        if (Server == null)
                            {
                            AddText("多端口监听服务器未成功创建，请查找原因后再尝试。");
                            MessageBox.Show("The servers have not been built successfully yet, please check the reason and retry." +
                                "\r\n多端口监听服务器未成功创建，请查找原因后再尝试。");
                            }
                        else
                            {
                            for (int a = 0; a < Server.Length; a++)
                                {

                                try
                                    {

                                    if (Server[a].Connected == false)
                                        {
                                        continue;
                                        }

                                    if (MultiServerSendMessageInHEX == true)
                                        {
                                        TempString = StringConvertToHEX(txtMultiServerSend.Text);// + TempCustomSuffix[a]);//SuffixForServerSendingMessage
                                        }
                                    else
                                        {
                                        TempString = txtMultiServerSend.Text;// + TempCustomSuffix[a];//SuffixForServerSendingMessage
                                        //if (GB2312Coding == true)
                                        //    {
                                        //    TempString = System.Text.Encoding.GetEncoding(936).GetString(TempString);
                                        //    }
                                        //else
                                        //    {
                                        //    TempString = System.Text.Encoding.UTF8.GetString(TempString);
                                        //    }  
                                        }
                                    if (Server[a].Send(TempString))
                                        {
                                        AddText("Server " + Server[a].ListenPort + " sent to client IP: " + Server[a].ClientIPAddress 
                                            + ", port: " + Server[a].ClientPort + ", details: " + TempString + " success...");
                                        }
                                    else
                                        {
                                        AddText("服务器发送 " + TempString + " 到端口 " + Server[a].ListenPort + " 失败");
                                        }
                                    }
                                catch (Exception ex)
                                    {
                                    AddText(ex.Message);
                                    }
                                  
                                }

                            }

                        }

                    if (txtSingleServerSend.Text != "")
                        {
                        if (ClientToBeAcceptedByServer != null)
                            {
                            if (ClientToBeAcceptedByServer.Connected == true)// (TCPServerStream != null)
                                {
                                if (SingleServerSendMessageInHEX == true)
                                    {
                                    TempString = StringConvertToHEX(txtSingleServerSend.Text + SuffixForServerSendingMessage);
                                    }
                                else
                                    {
                                    TempString = txtSingleServerSend.Text + SuffixForServerSendingMessage;
                                    }
                                if (GB2312Coding == true)
                                    {
                                    TCPServerStream.Write(System.Text.Encoding.GetEncoding(936).GetBytes(TempString), 0,
                                    System.Text.Encoding.GetEncoding(936).GetBytes(TempString).Length);
                                    }
                                else
                                    {
                                    TCPServerStream.Write(System.Text.Encoding.UTF8.GetBytes(TempString), 0,
                                    System.Text.Encoding.UTF8.GetBytes(TempString).Length);
                                    }
                                AddText("Server port " + TCPServerListeningPort + " sent to client IP: " 
                                    + FC.GetRemoteIP(ClientToBeAcceptedByServer) + ", port: " + 
                                    FC.GetRemotePort(ClientToBeAcceptedByServer) + ", details: " + TempString);
                                txtSingleServerSend.Focus();
                                }
                            else
                                {
                                AddText("单端口服务器没有启动工作或者客户端已经断开了连接！");
                                }
                            }
                        else 
                            {
                            AddText("单端口服务器没有启动工作或者客户端已经断开了连接！");
                            }
                        }

                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        private void picHelp_Click(object sender, EventArgs e)
            {
            HelpDlg.ShowDialog();
            }

        private void picEnglish_Click(object sender, EventArgs e)
            {
            if (ChineseLanguage == true)
                {
                ChineseLanguage = false;
                LanguageForUserInterface(false);
                }
            }

        private void picChinese_Click(object sender, EventArgs e)
            {
            if (ChineseLanguage == false)
                {
                ChineseLanguage = true;
                LanguageForUserInterface(true);
                }
            }

        private void ServerListView2_SelectedIndexChanged(object sender, EventArgs e)
            {

            }
        
        private void ServerListView2_ItemSelectionChanged(object sender,
            ListViewItemSelectionChangedEventArgs e)
            {

            //此事件会在鼠标点击其他行时发生两次, 第一次是离开当前行, 第二次是到达新的目标行
            //根据选择的行数，进行更新地址和端口至文本框，同时统计是否连接的情况，建立数组对应的情况，
            //如果想所有的点都能够同时通信，很难；
            //故只做一个一个的通信, 并且必须要关闭当前通信才能切换至另外一个IP进行通信

            if (e.Item.Index==-1)
                {
                lblCurrentIndexForServerListView.Text = "0";
                return;
                }

            CurrentSelectedItem=e.Item.Index;

            try
                {

                if (ServerListView2.SelectedItems.Count == 0)
                    {
                    CurrentIndexForServerListView = 0;
                    lblCurrentIndexForServerListView.Text = "0";
                    }
                else
                    {
                    CurrentIndexForServerListView = e.ItemIndex + 1;
                    lblCurrentIndexForServerListView.Text = (e.Item.Index + 1).ToString();
                    }

                //只有客户端没有与服务器进行通讯的情况下，才能变更参数
                if (ServerWorking == false)
                    {

                    if (ServerListView2.Items[e.Item.Index].SubItems[1].Text != "")
                        {
                        txtServerListenPort.Text = ServerListView2.Items[e.Item.Index].SubItems[1].Text;

                        //None, LF,CR,CR+LF,自定义
                        switch (ServerListView2.Items[e.Item.Index].SubItems[3].Text)
                            {

                            case "":
                                cmbSuffixForServer.SelectedIndex = 0;
                                break;

                            case "LF":
                                cmbSuffixForServer.SelectedIndex = 1;
                                break;

                            case "CR":
                                cmbSuffixForServer.SelectedIndex = 2;
                                break;

                            case "CRLF":
                                cmbSuffixForServer.SelectedIndex = 3;
                                break;

                            case "CUS":
                                cmbSuffixForServer.SelectedIndex = 4;
                                break;

                            }

                        }

                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        private void txtServerListenPort_KeyDown(object sender, KeyEventArgs e)
            {

            if (e.KeyCode == Keys.Enter)
                {
                btnListen.Focus();
                }

            }

        private void btnDelServerRecordInListView_Click(object sender, EventArgs e)
            {

            try
                {

                if (ServerListView2.Items.Count >= 1)
                    {

                    if (ServerListView2.FocusedItem.Selected == true)
                        {

                        if (MessageBox.Show("Are you sure to delete the row: " + (ServerListView2.FocusedItem.Index + 1)
                            + "?\r\n确定要删除第 " + (ServerListView2.FocusedItem.Index + 1) + " 行吗？", "Notice",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {

                            if (LV.DelRow(ref ServerListView2, Convert.ToInt16(ServerListView2.FocusedItem.Index+1)) == true)
                                {
                                AddText("成功删除");
                                }
                            else
                                {
                                AddText("删除失败");
                                }

                            }

                        }
                    else
                        {
                        AddText("无效操作：未选中任何行.");
                        MessageBox.Show("You did not select any item of the list.\r\n无效操作：未选中任何行.");
                        }

                    }
                else
                    {
                    AddText("无效操作：列表中无任何行数据.");
                    MessageBox.Show("There is no record in the list.\r\n无效操作：列表中无任何行数据.");
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        private void btnAddPortForServer_Click(object sender, EventArgs e)
            {

            try
                {

                string[] TempRecord = new string[3];

                TempRecord[0] = txtServerListenPort.Text;
                TempRecord[1] = "False";

                //0~4: None, LF, CR, CR+LF,CustomizedEnding
                switch (cmbSuffixForServer.SelectedIndex)
                    {

                    case 0:
                        TempRecord[2] = "";
                        break;

                    case 1:
                        TempRecord[2] = "LF";
                        break;

                    case 2:
                        TempRecord[2] = "CR";
                        break;

                    case 3:
                        TempRecord[2] = "CRLF";
                        break;

                    case 4:
                        TempRecord[2] = "CUS";
                        break;

                    }

                if (VerifyPort(Convert.ToUInt16(txtServerListenPort.Text)) == true)
                    {

                    if (LV.AddRowRecordInListView(ref ServerListView2, TempRecord) == true)
                        {
                        AddText("添加成功");
                        }
                    else
                        {
                        AddText("添加失败");
                        }

                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        private void btnReviseServerRecord_Click(object sender, EventArgs e)
            {

            try
                {

                string[] TempRecord = new string[3];

                TempRecord[0] = txtServerListenPort.Text;
                TempRecord[1] = "False";

                //0~4: None, LF, CR, CR+LF,CustomizedEnding
                switch (cmbSuffixForServer.SelectedIndex)
                    {

                    case 0:
                        TempRecord[2] = "";
                        break;

                    case 1:
                        TempRecord[2] = "LF";
                        break;

                    case 2:
                        TempRecord[2] = "CR";
                        break;

                    case 3:
                        TempRecord[2] = "CRLF";
                        break;

                    case 4:
                        TempRecord[2] = "CUS";
                        break;

                    }

                if (LV.ModifyRowRecord(ref ServerListView2, TempRecord,
                                           CurrentSelectedItem + 1))
                    {
                    AddText("成功修改记录");
                    }
                else
                    {
                    AddText("修改记录失败");
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        private void btnCloseServer_Click(object sender, EventArgs e)
            {

            try
                {

                btnAddPortForServer.Enabled = true;
                txtServerListenPort.Enabled = true;
                ServerWorking = false;

                //释放服务器的相关资源
                if (TCPServerStream != null)
                    {
                    TCPServerStream.Close();
                    TCPServerStream = null;
                    }

                if (ClientToBeAcceptedByServer != null)
                    {
                    ClientToBeAcceptedByServer.Close();
                    ClientToBeAcceptedByServer = null;
                    }

                if (TCPServerStation != null)
                    {
                    TCPServerStation.Stop();
                    TCPServerStation = null;
                    }

                if (ServerAcceptClientConnectionThread != null)
                    {
                    ServerAcceptClientConnectionThread.Abort();
                    ServerAcceptClientConnectionThread = null;
                    }

                btnListen.Enabled = true;
                btnCloseServer.Enabled = false;
                btnServerSendToClient.Enabled = false;

                AddText("关闭服务器...");

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        private void rtbTCPIPHistory_MouseDoubleClick(object sender, MouseEventArgs e)
            {

            if (rtbTCPIPHistory.Text == "")
                {
                return;
                }

            if (MessageBox.Show("Are you sure to clear all the log?\r\n确定要清除通讯记录吗？","确认",MessageBoxButtons.YesNo,MessageBoxIcon.Question)
                == DialogResult.Yes)
                {
                rtbTCPIPHistory.Text = "";
                }

            }

        private void rtbTCPIPHistory_MouseDown(object sender, MouseEventArgs e)
            {

            try
                {

                if (rtbTCPIPHistory.Text == "")
                    {
                    return;
                    }

                if (e.Button == MouseButtons.Right)
                    {

                    SaveLogAsTXTFile.DefaultExt = "txt";
                    SaveLogAsTXTFile.Filter = "TXT文本文件 (*.txt)|*.txt";
                    SaveLogAsTXTFile.Title = "保存运行记录至文件";

                    // 's'--将日期和时间格式化为可排序的索引。例如 2008-03-12T11:07:31。
                    //     s 字符以用户定义的时间格式显示秒钟。
                    // 'u'--将日期和时间格式化为 GMT 可排序索引。如 2008-03-12 11:07:31Z。

                    SaveLogAsTXTFile.FileName = "TCPIPServerLog-" + Strings.Format(DateTime.Now, "yyyy'年'MM'月'dd'日' HH'点'mm'分'ss'秒'"); // "yyyy-MM-dd HH%h-mm%m-ss%s") '"s")
                    SaveLogAsTXTFile.RestoreDirectory = true;

                    if (SaveLogAsTXTFile.ShowDialog() == System.Windows.Forms.DialogResult.OK
                        & SaveLogAsTXTFile.FileName != "")
                        {
                        rtbTCPIPHistory.SaveFile(SaveLogAsTXTFile.FileName, RichTextBoxStreamType.TextTextOleObjs);
                        AddText("成功保存运行记录至文件: " + SaveLogAsTXTFile.FileName);
                        }
                    else
                        {
                        AddText("取消保存运行记录...");
                        }

                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                }

            }

        private void chkGB2312_CheckedChanged(object sender, EventArgs e)
            {

            if (chkGB2312.Checked == true)
                {
                GB2312Coding = true;
                }
            else
                {
                GB2312Coding = false;
                }

            }

        private void chkSingleServerSendHEX_CheckedChanged(object sender, EventArgs e)
            {

            if (chkSingleServerSendHEX.Checked == true)
                {
                SingleServerSendMessageInHEX = true;
                }
            else
                {
                SingleServerSendMessageInHEX = false;
                }

            }

        private void chkMultiServerSendHEX_CheckedChanged(object sender, EventArgs e)
            {

            if (chkMultiServerSendHEX.Checked == true)
                {
                MultiServerSendMessageInHEX = true;
                }
            else
                {
                MultiServerSendMessageInHEX = false;
                }

            }

        private void txtLocalServerIPAddress_TextChanged(object sender, EventArgs e)
            {

            }

        private void txtServerListenPort_TextChanged(object sender, EventArgs e)
            {

            }

        private void txtServerListenPort_KeyPress(object sender, KeyPressEventArgs e)
            {

            //如果按下的不是数字键和控制键，则删除总长度的一个字符，
            //然后聚焦文本框【如果输入的第一个字符不是数字，则清除】
            if (!(char.IsNumber(e.KeyChar)| char.IsControl(e.KeyChar)))
                {
                AddText("服务器的监听端口只能是数字，请重新输入...");
                txtServerListenPort.Focus();
                e.Handled = true;
                }

            }
        
        //添加网络IP地址变更和网络断开、连接的监听句柄
        /// <summary>
        /// 添加网络IP地址变更和网络断开、连接的监听句柄
        /// </summary>
        private void AddNetworkChange_EventHandler() 
            {
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(AddressChangedCallback);
            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkAvailabilityChangedCallback);
            }

        //网络IP地址变更事件的代理
        /// <summary>
        /// 网络IP地址变更事件的代理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddressChangedCallback(object sender, EventArgs e) 
            {
            AddText("网络中有IP地址的变更...");
            }

        //网络断开、连接事件的代理
        /// <summary>
        /// 网络断开、连接事件的代理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NetworkAvailabilityChangedCallback(object sender,
            NetworkAvailabilityEventArgs e) 
            {

    //    Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
    //    Dim n As NetworkInterface

    //    NetworkInterface.OperationalStatus(属性)
    //    获取网络连接的当前操作状态。
    //    Public MustOverride ReadOnly Property OperationalStatus As OperationalStatus

    //    OperationalStatus 枚举
    //    Up	            网络接口已运行，可以传输数据包。
    //    Down	        网络接口无法传输数据包。
    //    Testing	    网络接口正在运行测试。
    //    Unknown	    网络接口的状态未知。
    //    Dormant	    网络接口不处于传输数据包的状态；它正等待外部事件。
    //    NotPresent	    由于缺少组件（通常为硬件组件），网络接口无法传输数据包。
    //    LowerLayerDown	网络接口无法传输数据包，因为它运行在一个或多个其他接口之上，而这些“低层”接口中至少有一个已关闭。
            
            //当网络断开后，判断是否为本机自己通信，如果不是，则要关闭所有和其它设备的以太网通信
            //if (TempTestIPAddress!="127.0.0.1") 
            //    {

                if (e.IsAvailable == false)
                    {
                    AddText("警告：网络已断开！");
                    MessageBox.Show("The network is not available now.\r\n网络已断开！", "警告", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    TempTestIPAddress = GetLocalIP4Address();
                    AddText("本机IP地址为：" + TempTestIPAddress);
                    FC.UpdateTextBox(ref txtLocalServerIPAddress, TempTestIPAddress);
                    //FC.ClearTextBoxContents(ref txtLocalServerIPAddress);
                    //FC.AddTextWithoutTimeMark(ref txtLocalServerIPAddress, TempTestIPAddress);

                    if (StartMultiListen == true & TempTestIPAddress != "127.0.0.1") 
                        {
                        
                        if(Server!=null)
                            {

                            for (int a = 0; a < Server.Length;a++ ) 
                                {

                                try 
                                    { 
                                    
                                    if(Server[a]!=null)
                                        {
                                        Server[a].Close();
                                        AddText("成功关闭服务器 " + (a + 1) + " 通讯线程...");
                                        }

                                    }
                                catch(Exception ex)
                                    {
                                    AddText("关闭服务器 " + (a + 1) + " 通讯线程发生错误: " + ex.Message);
                                    //MessageBox.Show(ex.Message);
                                    }

                                }
                            AddText("由于断网而关闭的TCP/IP通讯线程在网络恢复后会自动重新启动...");
                            }

                        }

                    }
                else 
                    {
                    //AddText("The network is available now.");
                    AddText("通知：网络已连接！");
                    MessageBox.Show("通知：网络已连接！","通知",MessageBoxButtons.OK,MessageBoxIcon.Information);

                    TempTestIPAddress = GetLocalIP4Address();
                    AddText("本机IP地址为：" + TempTestIPAddress);
                    FC.UpdateTextBox(ref txtLocalServerIPAddress, TempTestIPAddress);
                    //FC.ClearTextBoxContents(ref txtLocalServerIPAddress);
                    //FC.AddTextWithoutTimeMark(ref txtLocalServerIPAddress, TempTestIPAddress);
                    //txtLocalServerIPAddress.Text = TempTestIPAddress;

                    if (StartMultiListen == true & TempTestIPAddress != "127.0.0.1")
                        {

                        if (Server != null)
                            {

                            for (int a = 0; a < Server.Length; a++)
                                {

                                try
                                    {

                                    if (Server[a] != null)
                                        {
                                        Server[a].Start();
                                        AddText("成功启动服务器 " + (a + 1) + " 通讯线程...");
                                        }

                                    }
                                catch (Exception ex)
                                    {
                                    AddText("启动服务器 " + (a + 1) + " 通讯线程发生错误: " + ex.Message);
                                    //MessageBox.Show(ex.Message);
                                    }

                                }
                            AddText("网络已经重新连接，由于断网而关闭的TCP/IP通讯线程已重新启动...");
                            }

                        }

                    }

                //}

            }

        private void txtAutoSendInterval_TextChanged(object sender, EventArgs e)
            {

            }

        private void txtAutoSendInterval_KeyPress(object sender, KeyPressEventArgs e)
            {

            if (!(char.IsNumber(e.KeyChar) | char.IsControl(e.KeyChar)))
                {
                AddText("只能输入数字，请重新输入...");
                e.Handled = true;
                MessageBox.Show("只能输入数字，请重新输入...");
                }

            }

        private void chkAutoSend_CheckedChanged(object sender, EventArgs e)
            {

            try
                {

                if (chkAutoSend.Checked == true)
                    {

                    if (txtAutoSendInterval.Text == "")
                        {
                        MessageBox.Show("Please set the auto-send interval first.\r\n请先设置自动发送的间隔时间。");
                        txtAutoSendInterval.Focus();
                        chkAutoSend.Checked = false;
                        return;
                        }
                    else
                        {
                        tmrAutoSend.Interval = Convert.ToInt32(txtAutoSendInterval.Text);
                        AutoResendFlag = true;
                        tmrAutoSend.Start();
                        }

                    }
                else
                    {
                    AutoResendFlag = false;
                    tmrAutoSend.Stop();
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        private void chkSendFile_CheckedChanged(object sender, EventArgs e)
            {

            try
                {

                if (chkSendFile.Checked == true)
                    {
                    newOpenFile.Filter = "所有文件|*.*";
                    newOpenFile.Multiselect = true;

                    if (newOpenFile.ShowDialog() == DialogResult.OK)
                        {

                        if (newOpenFile.FileNames.Length > 0)
                            {
                            SendFileFlag = true;
                            }
                        else
                            {
                            chkSendFile.Checked = false;
                            SendFileFlag = false;
                            }

                        }

                    }
                else
                    {
                    SendFileFlag = false;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        private void tmrAutoSend_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
            {

            try
                {

                //发送文件
                if (SendFileFlag == true)
                    {

                    byte[] Data;
                    string TempStr = "";

                    //多端口服务器发送文件的代码
                    if (StartMultiListen == true) 
                        {
                        
                        if(Server!=null)
                            {

                            for (int a = 0; a < Server.Length; a++) 
                                {

                                if (Server[a].Connected == false)
                                    {
                                    continue;
                                    }

                                if (newOpenFile.FileNames.Length > 0) 
                                    {

                                    for (int b = 0; b < newOpenFile.FileNames.Length; b++) 
                                        {

                                        try
                                            {
                                            Data =PC.FileSystem.ReadAllBytes(newOpenFile.FileNames[b]);
                                            AddText("File: " + newOpenFile.FileNames[b] + ". total: " + Data.Length + " bytes...");

                                            if (GB2312Coding == true)
                                                {
                                                TempStr = System.Text.Encoding.GetEncoding(936).GetString(Data);
                                                }
                                            else 
                                                {
                                                TempStr = System.Text.Encoding.UTF8.GetString(Data);
                                                }                                            

                                            if (Server[a].Send(TempStr) == true) // + TempCustomSuffix[a]   SuffixForServerSendingMessage
                                                {
                                                //AddText("Server Sent " + Data.Length + " bytes...");
                                                AddText("Server " + Server[a].ListenPort + " sent file " + newOpenFile.FileNames[b] + " to client IP: "
                                                    + Server[a].ClientIPAddress + ", port: " + Server[a].ClientPort + "...");
                                                }
                                            else 
                                                {
                                                AddText("服务器发送文件到端口 " + Server[a].ListenPort + " 失败");
                                                }

                                            }
                                        catch (Exception ex) 
                                            {
                                            AddText(ex.Message);
                                            }

                                        }
                                    
                                    }

                                }                            
                            
                            }
                        
                        }

                    //单端口服务器发送文件的代码
                    if (ClientToBeAcceptedByServer.Connected == true)// (TCPServerStream != null)
                        {

                        if (newOpenFile.FileNames.Length > 0)
                            {

                            for (int c = 0; c < newOpenFile.FileNames.Length; c++)
                                {

                                try
                                    {
                                    Data = PC.FileSystem.ReadAllBytes(newOpenFile.FileNames[c]);
                                    AddText("File: " + newOpenFile.FileNames[c] + ". total: " + Data.Length + " bytes...");

                                    if (GB2312Coding == true)
                                        {
                                        TempStr = System.Text.Encoding.GetEncoding(936).GetString(Data);
                                        TCPServerStream.Write(Encoding.GetEncoding(936).GetBytes(TempStr + SuffixForServerSendingMessage), 0,
                                            Encoding.GetEncoding(936).GetBytes(TempStr + SuffixForServerSendingMessage).Length);
                                        }
                                    else
                                        {
                                        TempStr = System.Text.Encoding.UTF8.GetString(Data);
                                        TCPServerStream.Write(Encoding.UTF8.GetBytes(TempStr + SuffixForServerSendingMessage), 0,
                                            Encoding.UTF8.GetBytes(TempStr + SuffixForServerSendingMessage).Length);
                                        }
                                    AddText("Server sent file " + newOpenFile.FileNames[c] + " to client IP: " 
                                        + FC.GetRemoteIP(ClientToBeAcceptedByServer) + ", port: " + FC.GetRemotePort(ClientToBeAcceptedByServer) +"...");
                                    }
                                catch (Exception ex)
                                    {
                                    AddText(ex.Message);
                                    }

                                }

                            }

                        }
                    else 
                        {
                        AddText("单端口服务器没有启动工作或者客户端已经断开了连接！");
                        }
                    
                    }
                else
                    {
                    ////发送文本字符
                    //if (cmbSuffixForServer.SelectedIndex == 4 &
                    //    txtCustomizedEndingCodeForServer.Text == "")
                    //    {
                    //    AddText("自定义的结束符不能为空，请输入相应的文本。");
                    //    MessageBox.Show("The text for the Ending can't be empty, please enter the text you want to be set." +
                    //       "\r\n自定义的结束符不能为空，请输入相应的文本。");
                    //    return;
                    //    }
                    //else if (cmbSuffixForServer.SelectedIndex == 4 &
                    //    txtCustomizedEndingCodeForServer.Text == "")
                    //    {
                    //    SuffixForServerSendingMessage = txtCustomizedEndingCodeForServer.Text;
                    //    }

                    if (SingleServerSend == "")
                        {
                        AddText("单端口发送的文本不能为空白，请输入文本内容。");
                        //txtSingleServerSend.Focus();
                        }

                    if (MultiServerSend == "")
                        {
                        AddText("多端口发送的文本不能为空白，请输入文本内容。");
                        //txtMultiServerSend.Focus();
                        }

                    string TempString = "";
                    if (MultiServerSend != "")
                        {

                        if (Server == null)
                            {
                            AddText("多端口监听服务器未成功创建，请查找原因后再尝试。");
                            //MessageBox.Show("The servers have not been built successfully yet, please check the reason and retry." +
                            //    "\r\n多端口监听服务器未成功创建，请查找原因后再尝试。");
                            }
                        else
                            {
                            for (int a = 0; a < Server.Length; a++)
                                {

                                if (Server[a].Connected == false)
                                    {
                                    continue;
                                    }

                                try
                                    {
                                    if (MultiServerSendMessageInHEX == true)
                                        {
                                        TempString = StringConvertToHEX(MultiServerSend);    // + TempCustomSuffix[a]   SuffixForServerSendingMessage
                                        }
                                    else
                                        {
                                        TempString = MultiServerSend;// + TempCustomSuffix[a]   SuffixForServerSendingMessage
                                        }
                                    if (Server[a].Send(TempString))
                                        {
                                        //AddText("Server " + Server[a].ListenPort + " sent " + TempString + " success...");
                                        AddText("Server " + Server[a].ListenPort + " sent to client IP: " + Server[a].ClientIPAddress 
                                            + ", port: " + Server[a].ClientPort + ", details: " + TempString + " success...");
                                        }
                                    else
                                        {
                                        AddText("服务器发送 " + TempString + " 到端口 " + Server[a].ListenPort + " 失败");
                                        }
                                    }
                                catch (Exception ex)
                                    {
                                    AddText(ex.Message);
                                    }
                                
                                }

                            }

                        }

                    if (SingleServerSend != "")
                        {
                        if (TCPServerStream != null)
                            {
                            if (SingleServerSendMessageInHEX == true)
                                {
                                TempString = StringConvertToHEX(SingleServerSend + SuffixForServerSendingMessage);
                                }
                            else
                                {
                                TempString = SingleServerSend + SuffixForServerSendingMessage;
                                }
                            if (GB2312Coding == true)
                                {
                                TCPServerStream.Write(System.Text.Encoding.GetEncoding(936).GetBytes(TempString), 0,
                                System.Text.Encoding.GetEncoding(936).GetBytes(TempString).Length);
                                }
                            else 
                                {
                                TCPServerStream.Write(System.Text.Encoding.UTF8.GetBytes(TempString), 0,
                                System.Text.Encoding.UTF8.GetBytes(TempString).Length);
                                }
                            AddText("Server port " + TCPServerListeningPort + " sent to client IP: "
                                    + FC.GetRemoteIP(ClientToBeAcceptedByServer) + ", port: " +
                                    FC.GetRemotePort(ClientToBeAcceptedByServer) + ", details: " + TempString);
                            //AddText("Server Sent: " + TempString);
                            //txtSingleServerSend.Focus();
                            }
                        else
                            {
                            AddText("单端口服务器没有启动工作或者客户端已经断开了连接！");
                            }
                        }
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                }

            }

        #endregion

        #region "函数程序代码"

        /// <summary>
        /// 服务器进行多端口监听通讯的读取线程
        /// </summary>
        private void MonitorMultiListen()
            {
            while (true)
                {
                try
                    {
                    if (StartMultiListen)
                        {
                        if (Server != null)
                            {
                            for (int a = 0; a < Server.Length; a++)
                                {
                                if (Server[a].ReceivedString != "")
                                    {
                                    FC.UpdateRichTextBox(ref rtbCurrentReceived,Server[a].ReceivedString);
                                    AddText("服务器从端口 " + Server[a].ListenPort + " 收到: "
                                        + Server[a].ReceivedString);
                                    Server[a].ReceivedString = "";
                                    AddText("客户端IP地址为：" + Server[a].ClientIPAddress);
                                    AddText("客户端端口号为：" + Server[a].ClientPort);
                                    }
                                }
                            }
                        }
                    }
                catch (Exception ex)
                    {
                    AddText(ex.Message);
                    }
                };
            }

        //启动服务器监听线程读取和发送内容给客户端
        /// <summary>
        /// 启动服务器监听线程读取和发送内容给客户端
        /// </summary>
        private void StartServerListeningThread()
            {

            byte[] ServerReceivedBytes = new byte[1024];
            Int32 i;
            Int32 Length;
            string TempRecord = "";

            TCPServerStation = null;
            TCPServerStation = new System.Net.Sockets.TcpListener(TCPServerIPAddress, TCPServerListeningPort);
            ClientToBeAcceptedByServer = null;
            TCPServerStream = null;
            TCPServerStation.Start();

            ErrorMessage = "服务器已开始工作，等待客户端的连接...";  //The server is start workinng and waiting for a connection. 
            AddText("服务器已开始工作，等待客户端的连接...");

            while (true)
                {

                try
                    {

                    if (ClientToBeAcceptedByServer == null)
                        {
                        ClientToBeAcceptedByServer = TCPServerStation.AcceptTcpClient();
                        ClientToBeAcceptedByServer.NoDelay = true;
                        ClientToBeAcceptedByServer.ReceiveBufferSize = ReceiveBufferSize;
                        ClientToBeAcceptedByServer.SendBufferSize = SendBufferSize;
                        ServerReceivedBytes = new byte[ReceiveBufferSize];
                        Length = ReceiveBufferSize;
                        }
                    else
                        {

                        i = 0;

                        //while (ClientToBeAcceptedByServer!=null)// (ClientToBeAcceptedByServer.Connected == true)
                        //    {

                        if (ClientToBeAcceptedByServer.Connected == true)
                            {

                            //while (ClientToBeAcceptedByServer.Connected == true)
                            //    {

                                if (TCPServerStream == null)
                                    {
                                    ErrorMessage = "服务器已成功接受客户端的连接请求..."; //Server connected with the client!  
                                    AddText("服务器已成功接受客户端的连接请求...");
                                    TCPServerStream = ClientToBeAcceptedByServer.GetStream();
                                    TCPServerStream.Write(System.Text.Encoding.UTF8.GetBytes("Server"), 0, System.Text.Encoding.UTF8.GetBytes("Server").Length);
                                    ErrorMessage = "Server sent: Server";
                                    AddText("Server sent: Server");
                                    }

                                ReceivedDataFromClient = "";
                                i = TCPServerStream.Read(ServerReceivedBytes, 0, ServerReceivedBytes.Length);

                                if (i > 0)
                                    {
                                    if (GB2312Coding == true)
                                        {
                                        TempRecord = System.Text.Encoding.GetEncoding(936).GetString(ServerReceivedBytes, 0, i);
                                        }
                                    else 
                                        {
                                        TempRecord = System.Text.Encoding.UTF8.GetString(ServerReceivedBytes, 0, i);
                                        }
                                    
                                    ReceivedDataFromClient = TempRecord;
                                    FC.UpdateRichTextBox(ref rtbCurrentReceived, TempRecord);
                                    //ReceivedString = TempRecord;
                                    //ErrorMessage = "Server got: " + TempRecord;
                                    ErrorMessage = "Server port:" + FC.GetLocalPort(ClientToBeAcceptedByServer) +
                                        " got data from client IP: " + FC.GetRemoteIP(ClientToBeAcceptedByServer)
                                        + ", port: " + FC.GetRemotePort(ClientToBeAcceptedByServer) + " details: " + TempRecord;
                                    //AddText("Server got: " + TempRecord);
                                    AddText("Server port:" + FC.GetLocalPort(ClientToBeAcceptedByServer) + 
                                        " got data from client IP: " + FC.GetRemoteIP(ClientToBeAcceptedByServer) 
                                        + ", port: " + FC.GetRemotePort(ClientToBeAcceptedByServer) +  " details: " + TempRecord);
                                    }
                                else 
                                    {
                                    TCPServerStream.Write(System.Text.Encoding.UTF8.GetBytes("Server"), 0, System.Text.Encoding.UTF8.GetBytes("Server").Length);
                                    }

                                //};

                            
                            }
                        else
                            {
                            //TCPServerStation = null;
                            //TCPServerStation = new System.Net.Sockets.TcpListener(TCPServerIPAddress, TCPServerListeningPort);
                            //ClientToBeAcceptedByServer = null;
                            //TCPServerStream = null;
                            //TCPServerStation.Start();

                            //ErrorMessage = "服务器已开始工作，等待客户端的连接...";  //The server is start workinng and waiting for a connection. 
                            //AddText("服务器已开始工作，等待客户端的连接...");

                            TCPServerStream.Close();
                            TCPServerStream = null;
                            ClientToBeAcceptedByServer.Close();
                            ClientToBeAcceptedByServer = null;
                            AddText("客户端已经断开以太网连接");
                            break;
                            //throw new Exception("客户端已经断开以太网连接");                                
                            }

                        }
                    //};

                    }
                catch (Exception ex)
                    {
                    ErrorMessage = "Server Error: " + ex.Message;

                    if (!(TCPServerStream == null))
                        {
                        TCPServerStream.Close();
                        TCPServerStream = null;
                        }

                    if (!(ClientToBeAcceptedByServer == null))
                        {
                        ClientToBeAcceptedByServer.Close();
                        ClientToBeAcceptedByServer = null;
                        }
                    }

                };

            }

        private void OldStartServerListeningThread()
            {

            byte[] ServerReceivedBytes = new byte[1024];

            Int32 i;
            short Length;

            string TempRecord = "";

            while (true)
                {

                try
                    {

                    if (TCPServerStation == null)
                        {
                        TCPServerStation = new System.Net.Sockets.TcpListener(TCPServerIPAddress, TCPServerListeningPort);
                        ClientToBeAcceptedByServer = null;
                        TCPServerStream = null;
                        TCPServerStation.Start();

                        ErrorMessage = "服务器已开始工作，等待客户端的连接...";  //The server is start workinng and waiting for a connection. 
                        AddText("服务器已开始工作，等待客户端的连接...");
                        }
                    else
                        {

                        if (ClientToBeAcceptedByServer == null)
                            {
                            TCPServerStation.Stop();
                            TCPServerStation.Start();
                            ClientToBeAcceptedByServer = TCPServerStation.AcceptTcpClient();
                            ClientToBeAcceptedByServer.NoDelay = true;
                            ClientToBeAcceptedByServer.ReceiveBufferSize = ReceiveBufferSize;
                            ClientToBeAcceptedByServer.SendBufferSize = SendBufferSize;

                            ServerReceivedBytes = new byte[ReceiveBufferSize];
                            Length = (short)ReceiveBufferSize;
                            }
                        else
                            {

                            i = 0;

                            while (ClientToBeAcceptedByServer.Connected == true)
                                {

                                if (ClientToBeAcceptedByServer.Connected == true)
                                    {

                                    if (TCPServerStream == null)
                                        {
                                        ErrorMessage = "服务器已成功接受客户端的连接请求..."; //Server connected with the client!  
                                        AddText("服务器已成功接受客户端的连接请求...");
                                        TCPServerStream = ClientToBeAcceptedByServer.GetStream();
                                        TCPServerStream.Write(System.Text.Encoding.UTF8.GetBytes("Server"), 0, System.Text.Encoding.UTF8.GetBytes("Server").Length);
                                        ErrorMessage = "Server sent: Server";
                                        }

                                    ReceivedDataFromClient = "";
                                    //i = TCPServerStream.Read(ServerReceivedBytes, 0, (Convert.ToInt16(ServerReceivedBytes.Length)));
                                    i = TCPServerStream.Read(ServerReceivedBytes, 0, ServerReceivedBytes.Length);

                                    if (i > 0)
                                        {
                                        TempRecord = System.Text.Encoding.UTF8.GetString(ServerReceivedBytes, 0, i);
                                        ReceivedDataFromClient = TempRecord;
                                        //ReceivedString = TempRecord;
                                        ErrorMessage = "Server got: " + TempRecord;
                                        AddText("Server got: " + TempRecord);
                                        }
                                    //else
                                    //    {
                                    //    //客户端关闭后，服务器无法重新和客户端取得连接并发送数据，虽然客户端已经显示和服务器连接成功。
                                    //    //故在此加入此语句就可以解决此问题，具体原因有待后来查证
                                    //    //原本作用:当收到的数据为空时，发送数据给客户端以确保相互之间是一直保持连接的
                                    //    TCPServerStream.Write(System.Text.Encoding.UTF8.GetBytes("Server"), 0, System.Text.Encoding.UTF8.GetBytes("Server").Length);
                                    //    ErrorMessage = "Server sent: Server";
                                    //    AddText("Server sent: Server");
                                    //    }

                                    }
                                //else
                                //    {
                                //    //如果扫描到客户端没有连接，则释放资源并产生异常
                                //    //if(!(TCPServerStream==null))
                                //    //    {
                                //    //    TCPServerStream.Close();
                                //    //    TCPServerStream = null;
                                //    //    }

                                //    //if (!(ClientToBeAcceptedByServer == null))
                                //    //    {
                                //    //    ClientToBeAcceptedByServer.Close();
                                //    //    ClientToBeAcceptedByServer = null;
                                //    //    }

                                //    throw new Exception("未与客户端建立通讯...");

                                //    }

                                }

                            }

                        }

                    }
                catch (Exception ex)
                    {
                    ErrorMessage = "Server Error: " + ex.Message;

                    if (!(TCPServerStream == null))
                        {
                        TCPServerStream.Close();
                        TCPServerStream = null;
                        }

                    if (!(ClientToBeAcceptedByServer == null))
                        {
                        ClientToBeAcceptedByServer.Close();
                        ClientToBeAcceptedByServer = null;
                        }

                    if (!(TCPServerStation == null))
                        {
                        TCPServerStation.Stop();
                        TCPServerStation = null;
                        }

                    }

                };

            }

        //TCP/IP Server同步监听通讯
        /// <summary>
        /// TCP/IP Server同步监听通讯
        /// </summary>
        public TCPServerForm(string DLLPassword)
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

                    //此处没有意义，只是为了不报警提示说这两个变量没有使用
                    if (PasswordIsCorrect == false | SuccessBuiltNew == false)
                        {
                        return;
                        }

                    }
                else
                    {
                    PasswordIsCorrect = false;
                    SuccessBuiltNew = false;
                    MessageBox.Show("Right Prohibited.\return\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                    }

                }
            catch(Exception ex)
                {
                SuccessBuiltNew = false;
                MessageBox.Show("创建以太网服务器窗体类的实例时出现错误！\r\n" +
                    ex.Message);
                }
            
            }

        //添加记录到窗体中的log文本框
        /// <summary>
        /// 添加记录到窗体中的log文本框
        /// </summary>
        /// <param name="TargetText">要添加的文本内容</param>
        public void AddText(string TargetText)
            {

            if (PasswordIsCorrect == false)
                {
                MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
                }

            try
                {

                if (TargetText == "")
                    {
                    return;
                    }

                string TempStr = "";//new string();//= TargetText；//Strings.UCase(TargetText);
                //TempStr = TargetText;
                //TargetText = "";

                //**********************
                // 去掉尾部的回车换行符号
                if (TargetText.Length == 1)
                    {
                    TempStr = TargetText;
                    }
                else if (TempStr.Length == 2)
                    {
                    if ((Strings.Asc(Strings.Mid(TargetText, TargetText.Length, 1)) == 13) |
                        (Strings.Asc(Strings.Mid(TargetText, TargetText.Length, 1)) == 10))
                        {
                        TempStr = Strings.Mid(TargetText, 1, TargetText.Length - 1);
                        }
                    else
                        {
                        TempStr = TargetText;
                        }
                    }
                else
                    {
                    if ((Strings.Asc(Strings.Mid(TargetText, TargetText.Length - 1, 1)) == 13) &
                    (Strings.Asc(Strings.Mid(TargetText, TargetText.Length, 1)) == 10))
                        {
                        TempStr = Strings.Mid(TargetText, 1, TargetText.Length - 2);
                        }
                    else
                        {
                        TempStr = TargetText;
                        }
                    }

                //if ((Strings.Asc(Strings.Mid(TempStr, TempStr.Length - 1, 1)) == 13) &
                //    (Strings.Asc(Strings.Mid(TempStr, TempStr.Length, 1)) == 10))
                //    {
                //    TempStr = Strings.Mid(TempStr, 1, TempStr.Length - 2);
                //    }
                //else if ((Strings.Asc(Strings.Mid(TempStr, TempStr.Length, 1)) == 13) |
                //    (Strings.Asc(Strings.Mid(TempStr, TempStr.Length, 1)) == 10))
                //    {
                //    TempStr = Strings.Mid(TempStr, 1, TempStr.Length - 1);
                //    }

                //**********************
                if (rtbTCPIPHistory.InvokeRequired == true)
                    {
                    AddTextDelegate ActualAddMessageToRichTextBox = new AddTextDelegate(AddText);
                    rtbTCPIPHistory.Invoke(ActualAddMessageToRichTextBox, new object[] { TempStr });
                    }
                else
                    {

                    if (UpdatingSameMessage == true)
                        {
                        TempErrorMessage = TempStr;
                        if (ShowDateTimeForMessage == true)
                            {
                            rtbTCPIPHistory.AppendText(DateTime.Now + "   " + TempStr + "\r\n");
                            }
                        else
                            {
                            rtbTCPIPHistory.AppendText(TempStr + "\r\n");
                            }
                        TempStr = "";
                        }
                    else
                        {

                        if (TempErrorMessage == TempStr)
                            {
                            return;
                            }
                        else
                            {
                            TempErrorMessage = TempStr;
                            if (ShowDateTimeForMessage == true)
                                {
                                rtbTCPIPHistory.AppendText(DateTime.Now + "   " + TempStr + "\r\n");
                                }
                            else
                                {
                                rtbTCPIPHistory.AppendText(TempStr + "\r\n");
                                }
                            TempStr = "";
                            }

                        }

                    }
                //**********************

                }
            catch (Exception)// ex)
                {
                //MessageBox.Show(ex.Message);
                return;
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

        //验证IP端口的有效性
        /// <summary>
        /// 验证IP端口的有效性
        /// </summary>
        /// <param name="TargetPort">目标端口</param>
        /// <returns>是否验证成功</returns>
        public bool VerifyPort(ushort TargetPort)
            {

            if (PasswordIsCorrect == false)
                {
                MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (TargetPort < 0 | TargetPort > 65535)
                    {
                    MessageBox.Show("The port value for the Server is not correct, please retry.\r\n你输入的IP地址无效，正确值得范围为0~65535，请修改后再重试.");
                    return false;
                    }
                else
                    {
                    return true;
                    }

                }
            catch (Exception)
                {
                return false;
                }

            }

        //验证IP地址的有效性，如果验证成功则返回正确IP地址
        /// <summary>
        /// 验证IP地址的有效性，如果验证成功则返回正确IP地址
        /// </summary>
        /// <param name="TargetIPAddress">待验证IP地址字符串</param>
        /// <returns></returns>
        public IPAddress VerifyIPAddress(string TargetIPAddress)
            {

            if (PasswordIsCorrect == false)
                {
                MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
                }

            IPAddress TempIPAddress = IPAddress.Parse("127.0.0.1");
            string[] GetCorrectIPAddress = new string[4];
            Int16[] TempGetIPAddress = new Int16[4];

            try
                {

                //1、判断输入的服务器IP地址是否正确
                if (IPAddress.TryParse(TargetIPAddress, out TempIPAddress) == false)
                    {
                    MessageBox.Show("The format of IP address is not correct, please retry.\r\nIP地址格式不正确， 请修改参数.");
                    return null;
                    }
                else
                    {
                    //************************************
                    //此处解析IP地址不对："192.168.000.024"解析为"192.168.000.20"
                    //故需要自己写算法得到正确IP地址
                    //string TempStr=Convert.ToString(TempIPAddress);
                    //GetCorrectIPAddress = Strings.Split(TempStr,".");

                    //------------------------------------
                    GetCorrectIPAddress = Strings.Split(TargetIPAddress, ".");
                    //************************************

                    for (Int16 a = 0; a < 4; a++)
                        {

                        TempGetIPAddress[a] = Convert.ToInt16(GetCorrectIPAddress[a]);

                        if (TempGetIPAddress[a] > 255 | TempGetIPAddress[a] < 0)
                            {
                            MessageBox.Show("The IP address you input is invalid, the value is 0~255, please revise and retry.\r\n你输入的IP地址无效，正确值得范围为0~255，请修改后再重试.");
                            return null;
                            }

                        }

                    }

                }
            catch (Exception)
                {
                return null;
                }

            string TempStr = Convert.ToString(TempGetIPAddress[0]) + "." + Convert.ToString(TempGetIPAddress[1]) + "." + Convert.ToString(TempGetIPAddress[2]) + "." + Convert.ToString(TempGetIPAddress[3]);
            return IPAddress.Parse(TempStr);

            //return IPAddress.Parse(Convert.ToString(TempGetIPAddress[0]) + "." + Convert.ToString(TempGetIPAddress[1]) + "." + Convert.ToString(TempGetIPAddress[2]) + "." + Convert.ToString(TempGetIPAddress[3]));

            }

        //从枚举Endings返回字符串，用于进行转义比较
        /// <summary>
        /// 从枚举Endings返回字符串，用于进行转义比较
        /// </summary>
        /// <param name="TargetEndings">目标结束符</param>
        /// <returns>转义比较后返回的字符串</returns>
        public string GetBackStrFromEndings(Endings TargetEndings) 
            {
            string TempStr = "";

            switch (TargetEndings)
                {

                case Endings.None:
                    TempStr = "";
                    break;
                //\r回车符，\n换行符
                case Endings.LF:
                    TempStr = "LF";
                    break;

                case Endings.CR:
                    TempStr = "CR";
                    break;

                case Endings.CRLF:
                    TempStr = "CRLF";
                    break;

                case Endings.Customerize:
                    TempStr = "CUS";
                    break;

                }

            return TempStr;
            
            }

        //用于设置界面的语言
        /// <summary>
        /// 用于设置界面的语言
        /// </summary>
        /// <param name="InChineseLanguage">是否用中文显示界面</param>
        private void LanguageForUserInterface(bool InChineseLanguage) 
            {

            if (InChineseLanguage == true)
                {

                this.Text = "以太网服务器多端口监听通讯软件";
                Server_GroupBox2.Text = "服务器端";
                lblLocalServerIPAddress.Text = "本地IP地址:";
                lblServerListeningPort.Text = "监听端口:";

                btnListen.Text = "监听";
                btnCloseServer.Text = "关闭";
                btnServerSendToClient.Text = "发送";

                lblEndingForServerSendingMessage.Text = "结束符:";

                ServerListView2.Columns[1].Text = "监听端口";
                ServerListView2.Columns[2].Text = "已测试";

                btnAddPortForServer.Text = "添加";

                ServerListView2.Columns[3].Text = "结束符";

                btnDelServerRecordInListView.Text = "删除";

                btnReviseServerRecord.Text = "修改";

                chkSingleServerSendHEX.Text = "16进制";
                chkMultiServerSendHEX.Text = "16进制";

                if (StartMultiListen == true)
                    {
                    btnMultiListen.Text = "关多端口监听";
                    }
                else
                    {
                    btnMultiListen.Text = "开多端口监听";
                    }

                lblAutoSendInterval.Text = "自动发送间隔:";
                lblMS.Text = "毫秒";
                chkAutoSend.Text = "自动发送";
                chkGB2312.Text = "中文编码";
                chkSendFile.Text = "发送文件";

                }
            else
                {
                lblAutoSendInterval.Text = "Auto Send Interval:";
                lblMS.Text = "ms";
                chkAutoSend.Text = "Auto Send";
                chkGB2312.Text = "GB2312";
                chkSendFile.Text = "Send File";

                if (StartMultiListen == true)
                    {
                    btnMultiListen.Text = "Close Multi";
                    }
                else
                    {
                    btnMultiListen.Text = "Multi Listen";
                    }

                btnAddPortForServer.Text = "Add";

                chkSingleServerSendHEX.Text = "HEX";
                chkMultiServerSendHEX.Text = "HEX";

                btnReviseServerRecord.Text = "Revise";

                btnDelServerRecordInListView.Text = "Del";

                this.Text = "TCP/IP Server Multi Listening Software";

                Server_GroupBox2.Text = "Be Server";

                lblLocalServerIPAddress.Text = "Local IP Address:";
                lblServerListeningPort.Text = "Listening Port:";

                btnListen.Text = "Listen";
                btnCloseServer.Text = "Close";
                btnServerSendToClient.Text = "Send";

                lblEndingForServerSendingMessage.Text = "Ending:";

                ServerListView2.Columns[1].Text = "Listen Port";
                ServerListView2.Columns[2].Text = "Tested";
                ServerListView2.Columns[3].Text = "Ending";

                }
                        
            }

        //返回当前电脑IP4地址
        /// <summary>
        /// 返回当前电脑IP4地址
        /// </summary>
        /// <returns></returns>
        public string GetLocalIP4Address()
            {

            if (PasswordIsCorrect == false)
                {
                MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
                }

            //'************************
            //'Dns法子总是以IPHostEntry对象的形式返回信息.它从 Internet 域名系统 (DNS) 检索关于特定主机的信息。它属于System.Net 命令空间
            //'其最常用的几个法子如下:
            //'获取当前电脑名:    System.Net.Dns.GetHostName()
            //'根据电脑名取出整个IP地址：System.Net.Dns.Resolve(电脑名).AddressList 或者 System.Net.Dns.GetHostByName(电脑名).AddressList根据IP地址取出电脑名：System.Net.Dns.Resolve(IP地址).HostName

            //'Public Shared Function GetHostEntry(ByVal address As System.Net.IPAddress) As System.Net.IPHostEntry
            //'System.Net.Dns(的成员)
            //'摘要:
            //'将 IP 地址解析为 System.Net.IPHostEntry 实例。

            //'参数:
            //'address: IP 地址。

            //'返回值:
            //'一个 System.Net.IPHostEntry 实例，包含有关 address 中指定的主机的地址信息。

            //'异常:
            //'System.ArgumentNullException: address 为 null。
            //'System.Net.Sockets.SocketException: 解析 address 时遇到错误。
            //'System.ArgumentException: address 是无效的 IP 地址

            string ActualIPAddress = "";

            try
                {

                //'*******************
                //'如果没有联网，则会返回字符串数组【两行】"::1     127.0.0.1" ////一个ScopeId,然后是IP4地址,可能是0.0.0.0，那这样就要用127.0.0.1代替///
                //'如果电脑已经联网，且只有一个网络，则会返回一个IP6地址【ScopeId】和IP4地址，例如：【两行】fe80::8119:e3fd:5efd:884e%3    192.168.0.125
                //'如果电脑已经联网，并且联到内、外网，则会返回两个IP6地址【ScopeId】,然后是两个IP4地址，第一个是外网IP4地址，第二个是内网IP4地址，如下面所示：
                //'Host(Name) : plc08
                //'IP address List : 
                //'fe80::803a:3983:6633:e634%12
                //'fe80::287b:21e:3f57:f512%11
                //'169.254.187.94
                //'192.168.10.237
                //'2001:0:808:808:287b:21e:3f57:f512

                //'所以必须要对找到的IP地址进行刷选，查找符号'.'，然后判断所查到的IP地址数是否为1个，否则就选择第二个为本机IP地址
                //'*******************

                //获得本地计算机的主机名 
                string HostName = "";
                HostName = System.Net.Dns.GetHostName();

                //解析获得的本地计算机主机名
                IPHostEntry HostInfo = System.Net.Dns.GetHostEntry(HostName);

                if (HostInfo.AddressList[0].ToString() == "::1")
                    {
                    MessageBox.Show("No other computer/device in the network...\r\n此时网络中没有其它设备...",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return "127.0.0.1";
                    }

                int iIdex = 0;
                string[] sTempIPAddress = new string[HostInfo.AddressList.Length];
                Int16 iTempIPCount = 0;

                //找出IP4地址：从获取的字符串搜索.，有符号.则是IP4地址
                for (iIdex = 0; iIdex < HostInfo.AddressList.Length; iIdex++)
                    {

                    if (Strings.InStr(HostInfo.AddressList[iIdex].ToString(), ".") > 0)
                        {
                        sTempIPAddress[iTempIPCount] = HostInfo.AddressList[iIdex].ToString();
                        iTempIPCount += 1;
                        }

                    }

                //进行判断并得到正确的本机IP4地址
                if (iTempIPCount == 1)
                    {
                    ActualIPAddress = sTempIPAddress[0];
                    }
                else
                    {
                    ActualIPAddress = sTempIPAddress[1];
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
                }

            return ActualIPAddress;

            }

        //返回当前电脑MAC地址
        /// <summary>
        /// 返回当前电脑MAC地址
        /// </summary>
        /// <returns></returns>
        public string GetLocalMACAddress()
            {

            if (PasswordIsCorrect == false)
                {
                MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
                }

            string ActualMACAddress = "";

            try
                {

                System.Management.ManagementObjectSearcher GetMAC = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");

                foreach (System.Management.ManagementObject MACObj in GetMAC.Get())
                    {

                    System.Management.ManagementObject TempMAC = new System.Management.ManagementObject("IPEnabled");

                    if (Convert.ToBoolean(MACObj["IPEnabled"]) == true)
                        {
                        ActualMACAddress = MACObj["MACAddress"].ToString();
                        }
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
                }

            return ActualMACAddress;

            }

        //返回当前电脑IP6地址
        /// <summary>
        /// 返回当前电脑IP6地址
        /// </summary>
        /// <returns></returns>
        public string GetLocalIP6Address()
            {

            if (PasswordIsCorrect == false)
                {
                MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
                }

            string ActualIPAddress = "";

            try
                {

                //'*******************
                //'如果没有联网，则会返回字符串数组【两行】"::1     127.0.0.1" ////一个ScopeId,然后是IP4地址,可能是0.0.0.0，那这样就要用127.0.0.1代替///
                //'如果电脑已经联网，且只有一个网络，则会返回一个IP6地址【ScopeId】和IP4地址，例如：【两行】fe80::8119:e3fd:5efd:884e%3    192.168.0.125
                //'如果电脑已经联网，并且联到内、外网，则会返回两个IP6地址【ScopeId】,然后是两个IP4地址，第一个是外网IP4地址，第二个是内网IP4地址，如下面所示：
                //'Host(Name) : plc08
                //'IP address List : 
                //'fe80::803a:3983:6633:e634%12
                //'fe80::287b:21e:3f57:f512%11
                //'169.254.187.94
                //'192.168.10.237
                //'2001:0:808:808:287b:21e:3f57:f512

                //'所以必须要对找到的IP地址进行刷选，查找符号'.'，然后判断所查到的IP地址数是否为1个，否则就选择第二个为本机IP地址
                //'*******************

                //获得本地计算机的主机名 
                string HostName = "";
                HostName = System.Net.Dns.GetHostName();

                //解析获得的本地计算机主机名
                IPHostEntry HostInfo = System.Net.Dns.GetHostEntry(HostName);

                if (HostInfo.AddressList[0].ToString() == "::1")
                    {
                    MessageBox.Show("No other computer/device in the network...\r\n此时网络中没有其它设备...",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return "";
                    }

                int iIdex = 0;
                string[] sTempIPAddress = new string[HostInfo.AddressList.Length];
                Int16 iTempIPCount = 0;

                //找出IP6地址：从获取的字符串搜索::，有符号.则是IP6地址
                for (iIdex = 0; iIdex < HostInfo.AddressList.Length; iIdex++)
                    {

                    if (Strings.InStr(HostInfo.AddressList[iIdex].ToString(), "::") > 0)
                        {
                        sTempIPAddress[iTempIPCount] = HostInfo.AddressList[iIdex].ToString();
                        iTempIPCount += 1;
                        }

                    }

                //进行判断并得到正确的本机IP6地址
                if (iTempIPCount == 1)
                    {
                    ActualIPAddress = sTempIPAddress[0];
                    }
                else
                    {
                    ActualIPAddress = sTempIPAddress[1];
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
                }

            return ActualIPAddress;

            }

        //返回当前电脑名[HOSTNAME]
        /// <summary>
        /// 返回当前电脑名[HOSTNAME]
        /// </summary>
        /// <returns></returns>
        public string GetPCName()
            {

            if (PasswordIsCorrect == false)
                {
                MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
                }

            string TempPCName = "";

            try
                {
                TempPCName = System.Net.Dns.GetHostName();
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
                }

            return TempPCName;

            }

        #endregion
                        
        }//class

    }//namespace