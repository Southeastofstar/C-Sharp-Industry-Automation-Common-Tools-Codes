#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Threading;
using System.Reflection;

#endregion

//ok

namespace PengDongNanTools
    {
    
    //建立以太网服务器，与客户端进行通讯
    /// <summary>
    /// 建立以太网服务器，与客户端进行通讯【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    class SimpleTCPIPServer
        {

        #region "变量定义"

        CommonFunction FC = new CommonFunction("彭东南");

        /// <summary>
        /// 获取与服务器建立通讯后的客户端端口号,-1表示未连接
        /// </summary>
        public int ClientPort
            {
            get
                {
                if (ClientToBeAcceptedByServer != null)
                    {
                    if (ClientToBeAcceptedByServer.Connected == true)
                        {
                        //获取的远程终端字符串格式为：127.0.0.1:12466
                        Socket TempSocket = ClientToBeAcceptedByServer.Client;
                        EndPoint TempEndPoint = TempSocket.RemoteEndPoint;

                        //方法一：
                        return Convert.ToInt32(TempEndPoint.ToString().Split(':')[1]);

                        ////方法二：
                        //string TempIPAddress = TempEndPoint.ToString();
                        //string[] TempResult = TempIPAddress.Split(':');
                        //return Convert.ToInt32(TempResult[1]);
                        }
                    else
                        {
                        return -1;
                        }

                    }
                else
                    {
                    return -1;
                    }
                }

            }

        /// <summary>
        /// 获取与服务器建立通讯后的客户端IP地址
        /// </summary>
        public string ClientIPAddress
            {
            get
                {
                if (ClientToBeAcceptedByServer != null)
                    {
                    if (ClientToBeAcceptedByServer.Connected == true)
                        {
                        //获取的远程终端字符串格式为：127.0.0.1:12466
                        Socket TempSocket = ClientToBeAcceptedByServer.Client;
                        EndPoint TempEndPoint = TempSocket.RemoteEndPoint;
                        
                        //方法一：
                        return TempEndPoint.ToString().Split(':')[0];

                        ////方法二：
                        //string TempIPAddress = TempEndPoint.ToString();
                        //string[] TempResult=TempIPAddress.Split(':');
                        //return TempResult[0];
                        }
                    else
                        {
                        return "";
                        }
                    //SocketAddress TempClient = ClientToBeAcceptedByServer.Client.RemoteEndPoint.Serialize();
                    //Socket xx = ClientToBeAcceptedByServer.Client;
                    //EndPoint yy = xx.RemoteEndPoint;
                    //return yy;
                    }
                else
                    {
                    return "";
                    }
                }

            }

        ///// <summary>
        ///// 获取与服务器建立通讯后的客户端IP地址
        ///// </summary>
        //public string ClientIPAddress 
        //    {
        //    get 
        //        {
        //        if (ClientToBeAcceptedByServer != null) 
        //            {
        //            if (ClientToBeAcceptedByServer.Connected == true)
        //                {
        //                string TempStr = GetRemoteIP(ClientToBeAcceptedByServer);
        //                return TempStr;
        //                }
        //            else 
        //                {
        //                return "";
        //                }
        //            }
        //        else
        //            {
        //            return "";
        //            }
        //        }            
        //    }

        ///// <summary>
        ///// 获取与服务器建立通讯后的客户端端口号
        ///// </summary>
        //public int ClientPort 
        //    {
        //    get
        //        {
        //        if (ClientToBeAcceptedByServer != null)
        //            {
        //            if (ClientToBeAcceptedByServer.Connected == true)
        //                {
        //                int TempPort =GetRemotePort(ClientToBeAcceptedByServer);
        //                return TempPort;
        //                }
        //            else
        //                {
        //                return -1;
        //                }
        //            }
        //        else
        //            {
        //            return -1;
        //            }
        //        } 
        //    }

        /// <summary>
        /// 是否接收GB2312编码
        /// </summary>
        public bool GB2312Coding = false;

        /// <summary>
        /// 更新信息至文本框时是否显示日期和时间，默认为True
        /// </summary>
        public bool ShowDateTimeForMessage = true;

        //添加心跳包定时器的事件函数，取代Windows.Forms.Timer，用System.Timers.Timer是在ThreadPool中调用的；
        //private System.Windows.Forms.Timer ServerHeartBeatPulseTimer = new System.Windows.Forms.Timer();
        private System.Timers.Timer ServerHeartBeatPulseTimer = new System.Timers.Timer();

        /// <summary>
        /// 以太网服务器接收缓冲区大小[字节]
        /// </summary>
        public Int32 ReceiveBufferSize = 1024;

        /// <summary>
        /// 以太网服务器发送缓冲区大小[字节]
        /// </summary>
        public Int32 SendBufferSize = 1024;

        /// <summary>
        /// 服务器从客户端收到的字符串内容
        /// </summary>
        public string ReceivedString = "";  //这样可以在实例化以后，处理完此字符串后执行清除原有内容

        /// <summary>
        /// 当错误信息相同时，是否重复显示
        /// </summary>
        public bool UpdatingSameMessage = true;

        //private string TempErrorMessage = "";

        /// <summary>
        /// 以太网通讯程序运行过程中的错误信息提示
        /// </summary>
        public string ErrorMessage;

        /// <summary>
        /// 用于记录实例化时输入密码是否正确
        /// </summary>
        private bool PasswordIsCorrect = false;

        /// <summary>
        /// 成功建立新的实例
        /// </summary>
        private bool SuccessBuiltNew = false;

        /// <summary>
        /// 自定义的结束符[用于发送时在发送的字符串最后加上]
        /// </summary>
        public string EndingCustomerizeSetting = "";

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }

        /// <summary>
        /// 发送字符串的结束符
        /// </summary>
        public string Suffix = "\r\n";

        /// <summary>
        /// 字符串用于存储从客户端发来的ASCII码.
        /// </summary>
        private string ReceivedDataFromClient = null;

        ///// <summary>
        ///// 用于接受客户端发来的字节
        ///// </summary>
        //private byte[] ServerReadDataBytesFromClient;

        ///// <summary>
        ///// 服务器发送给客户端的字符串内容，只要不是空""就会自动发送给客户端【实际在发送过程中由于线程的原因导致有延滞情况，故取消公开此变量】
        ///// </summary>
        //private string ServerSendToClientMessage = "";

        private System.Threading.Thread ServerAcceptClientConnectionThread=null;

        ///// <summary>
        ///// 以太网服务器端口接受客服端口标志
        ///// </summary>
        //private bool ServerAcceptClientConnectionSuccess = false;

        private TcpListener TCPServerStation;
        private IPAddress TCPServerIPAddress = IPAddress.Parse("127.0.0.1");
        private Int16 TCPServerListeningPort = 8000;
        private TcpClient ClientToBeAcceptedByServer;
        private NetworkStream TCPServerStream;

        /// <summary>
        /// 是否以16进制发送信息
        /// </summary>
        public bool SendMessageInHEX = false;

        /// <summary>
        /// 服务器发送心跳信号的字符串内容
        /// </summary>
        private string ServerHeartBeatPulseText = "Server";

        /// <summary>
        /// 服务器是否发送心跳信号
        /// </summary>
        public bool ServerEnableHeartBeat 
            {
            get { return ServerHeartBeatPulseTimer.Enabled; }
            set { ServerHeartBeatPulseTimer.Enabled = value; }
            }

        ///// <summary>
        ///// 服务器是否发送心跳信号
        ///// </summary>
        //private bool ServerEnableHeartBeatPulse = false;

        /// <summary>
        /// 服务器发送心跳信号的间隔【单位：毫秒(ms)
        /// </summary>
        public double IntervalForServerHeartBeat 
            {
            get { return ServerHeartBeatPulseTimer.Interval; }
            set
                {
                if (value <= 0)
                    {
                    MessageBox.Show("发送心跳信号的间隔必须大于0【单位：毫秒(ms)");
                    return;
                    }
                else
                    {
                    ServerHeartBeatPulseTimer.Interval = value;
                    }

                }
            }

        /// <summary>
        /// 服务器的监听端口号
        /// </summary>
        public short ListenPort 
            {
            get { return TCPServerListeningPort; }
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
                    return;
                    }
                else
                    {
                    ServerHeartBeatPulseText = value;
                    }
                }
            }

        /// <summary>
        /// 枚举:与客户端进行通讯时发送字符的结尾符号
        /// </summary>
        public enum Endings 
            {

            /// <summary>
            /// 无
            /// </summary>
            None=0,

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

        private Endings CurrentEndingSetting = Endings.CRLF;

        /// <summary>
        /// 设置与客户端进行通讯时发送字符的结尾符号,默认为"回车+换行"
        /// </summary>
        public Endings EndingSetting
            {
            set
                {

                switch (value)
                    {
                    case Endings.None:
                        Suffix = "";
                        CurrentEndingSetting = Endings.None;
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
                        break;

                    case Endings.Customerize:

                        if (EndingCustomerizeSetting != "")
                            {
                            Suffix = EndingCustomerizeSetting;
                            CurrentEndingSetting = Endings.Customerize;
                            }
                        else
                            {
                            Suffix = "\r\n";
                            CurrentEndingSetting = Endings.CRLF;
                            MessageBox.Show("You already set the Ending as ‘Customerize’, so the 'EndingCustomerizeSetting' can't be empty, please revise it.\r\n你已经设置通讯字符串的结束符为自定义，所以参数 'EndingCustomerizeSetting' 不能为空.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public Endings GetEndingSetting 
            {
            get { return CurrentEndingSetting; }
            }

        /// <summary>
        /// 以太网服务器口是否成功连接到客户端端口的标志
        /// </summary>
        public bool Connected
            {
            get
                {
                if (!(ClientToBeAcceptedByServer == null))
                    {
                    return ClientToBeAcceptedByServer.Connected;
                    }
                else
                    {
                    return false;
                    }
                }
            }

        private Microsoft.VisualBasic.Devices.Network PCNetwork=new Microsoft.VisualBasic.Devices.Network();

        /// <summary>
        /// 获取电脑是否联网的信息
        /// </summary>
        public bool NetworkAvailable
            {
            get
                {

                if (PCNetwork.IsAvailable)
                    {
                    return true;
                    }
                else
                    { 
                    return false; 
                    }

                }
            }

        private Button ButtonUseForInvoke = new Button();

   #endregion

        //******************************************
        #region "已完成代码"

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
         
        //创建TCP/IP通讯的服务器实例【程序会自动查找本机IP地址，如果没有接入网络则默认IP为127.0.0.1】
        /// <summary>
        /// 创建TCP/IP通讯的服务器实例【程序会自动查找本机IP地址，如果没有接入网络则默认IP为127.0.0.1】
        /// </summary>
        /// <param name="ListenPortOfServer">服务器监听端口</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public SimpleTCPIPServer(ushort ListenPortOfServer, string DLLPassword) 
            {

            try 
                {

                SuccessBuiltNew = false;
                PasswordIsCorrect = false;

                if (DLLPassword == "ThomasPeng" | DLLPassword == "pengdongnan" | DLLPassword == "彭东南")
                    {
                    PasswordIsCorrect =true;
                    //设置IP地址
                    if (PCNetwork.IsAvailable)
                        {
                        TCPServerIPAddress = VerifyIPAddress(GetLocalIP4Address());
                        }
                    else 
                        {
                        TCPServerIPAddress = VerifyIPAddress("127.0.0.1");
                        }

                    //以太网的有效端口从0~65535，刚好是UShort的值范围，所以不需要进行验证有效性
                    TCPServerListeningPort =Convert.ToInt16(ListenPortOfServer);

                    //添加心跳包定时器的事件函数，取代Windows.Forms.Timer，用System.Timers.Timer是在ThreadPool中调用的；
                    ServerHeartBeatPulseTimer.Elapsed += new System.Timers.ElapsedEventHandler(ServerHeartBeatPulseTimer_Elapsed);

                    SuccessBuiltNew = true;

                    }
                else 
                    {
                    SuccessBuiltNew = false;
                    PasswordIsCorrect = false;
                    MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南",
                        "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                    }                
                
                }
            catch(Exception ex)
                {
                SuccessBuiltNew = false;
                MessageBox.Show("创建以太网服务器类的实例时出现错误\r\n" + ex.Message,"Error",
                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
                }
            
            }

        //服务器发送心跳包的事件代码
        private void ServerHeartBeatPulseTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)//EventArgs e)
            {

            String TempString = "";
            TempString = ServerHeartBeatPulseText;

            try 
                {

                if (TempString!="") 
                    {

                    if (!(ClientToBeAcceptedByServer==null)) 
                        {

                        if (ClientToBeAcceptedByServer.Connected==true) 
                            {

                            if(!(TCPServerStream==null))
                                {

                                if (SendMessageInHEX == true)
                                    {
                                    TempString = StringConvertToHEX(TempString + Suffix);
                                    TCPServerStream.Write(System.Text.Encoding.UTF8.GetBytes(TempString), 0, System.Text.Encoding.UTF8.GetBytes(TempString).Length);
                                    }
                                else 
                                    {
                                    TCPServerStream.Write(System.Text.Encoding.UTF8.GetBytes(TempString + Suffix), 0, System.Text.Encoding.UTF8.GetBytes(TempString + Suffix).Length);
                                    }

                                }

                            }
                        
                        }
                    
                    }


                
                }
            catch(Exception ex)
                {
                ErrorMessage = ex.Message;                
                }

            }

        //关闭以太网服务器并释放相关资源
        /// <summary>
        /// 关闭以太网服务器并释放相关资源
        /// </summary>
        public void Close() 
            {

            try 
                {

                //释放服务器的相关资源
                if(!(TCPServerStream==null))
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

                //释放服务器以太网线程
                if (!(ServerAcceptClientConnectionThread == null))
                    {
                    //如果不屏蔽Abort()，则在实例化后在调用Close()方法时会出现整个程序无反应的现象
                    ServerAcceptClientConnectionThread.Abort();

                    //虽然下面代码有将线程= Nothing，但是经过执行此代码后实际上此线程还是存在并工作.
                    ServerAcceptClientConnectionThread = null;
                    }

                //强制对所有代进行垃圾回收
                GC.Collect();
                
                }
            catch(Exception ex)
                {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            
            }

        //发送字符串到客户端
        /// <summary>
        /// 发送字符串到客户端
        /// </summary>
        /// <param name="StringsToBeSentToClient"></param>
        /// <returns></returns>
        public bool Send(string StringsToBeSentToClient) 
            {

            string TempString = "";

            try 
                {
                //如果字符串为空，则退出函数
                if(StringsToBeSentToClient=="")
                    {
                    ErrorMessage = "需要发送的字符串为空，没有执行发送.";
                    return false;
                    }

                TempString = StringsToBeSentToClient;
                StringsToBeSentToClient = "";

                if (SuccessBuiltNew == false | PasswordIsCorrect==false)
                    {
                    ErrorMessage = "未经授权，你无法使用此DLL库.";
                    MessageBox.Show("未经授权，你无法使用此DLL库.");
                    return false;
                    }

                if (!(ClientToBeAcceptedByServer == null))
                    {

                    if (ClientToBeAcceptedByServer.Connected == true)
                        {

                        if (!(TCPServerStream == null))
                            {

                            if (SendMessageInHEX == true)
                                {
                                TempString = StringConvertToHEX(TempString + Suffix);
                                TCPServerStream.Write(System.Text.Encoding.UTF8.GetBytes(TempString), 0, System.Text.Encoding.UTF8.GetBytes(TempString).Length);
                                }
                            else
                                {
                                TCPServerStream.Write(System.Text.Encoding.UTF8.GetBytes(TempString + Suffix), 0, System.Text.Encoding.UTF8.GetBytes(TempString + Suffix).Length);
                                }

                            }

                        }

                    }

                TempString = "";
                return true;
                
                }
            catch(Exception ex)
                {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;                
                }
            
            }

        //启动以太网服务器与客户端进行通讯
        /// <summary>
        /// 启动以太网服务器与客户端进行通讯
        /// </summary>
        public void Start()
            {

            if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                {
                ErrorMessage = "未经授权，你无法使用此DLL库.";
                MessageBox.Show("未经授权，你无法使用此DLL库.");
                return;
                }

            try
                {

                if (SuccessBuiltNew == true)
                    {

                    if (ServerAcceptClientConnectionThread == null)
                        {
                        ServerAcceptClientConnectionThread = new System.Threading.Thread(StartServerListeningThread);
                        ServerAcceptClientConnectionThread.IsBackground = true;
                        ServerAcceptClientConnectionThread.Start();
                        ErrorMessage = "启动以太网服务器...";
                        }
                    else
                        {
                        ErrorMessage = "已经启动了以太网服务器...";
                        }

                    }
                else
                    {
                    ErrorMessage = "未成功创建以太网服务器类的实例！";
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            
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

                        //while (ClientToBeAcceptedByServer.Connected == true)
                        //    {

                            if (ClientToBeAcceptedByServer.Connected == true)
                                {

                                if (TCPServerStream == null)
                                    {
                                    ErrorMessage = "服务器已成功接受客户端的连接请求..."; //Server connected with the client!
                                    TCPServerStream = ClientToBeAcceptedByServer.GetStream();
                                    TCPServerStream.Write(System.Text.Encoding.UTF8.GetBytes("Server"), 0, System.Text.Encoding.UTF8.GetBytes("Server").Length);
                                    ErrorMessage = "Server sent: Server";
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
                                    ReceivedString = TempRecord;
                                    //ErrorMessage = "Server got: " + TempRecord;
                                    ErrorMessage = "Server port:" + FC.GetLocalPort(ClientToBeAcceptedByServer) +
                                        " got data from client IP: " + FC.GetRemoteIP(ClientToBeAcceptedByServer)
                                        + ", port: " + FC.GetRemotePort(ClientToBeAcceptedByServer) + " details: " + TempRecord;
                                    }
                                else
                                    {
                                    TCPServerStream.Write(System.Text.Encoding.UTF8.GetBytes("Server"), 0, System.Text.Encoding.UTF8.GetBytes("Server").Length);
                                    }

                                }
                            else 
                                {
                                TCPServerStream.Close();
                                TCPServerStream = null;
                                ClientToBeAcceptedByServer.Close();
                                ClientToBeAcceptedByServer = null;

                                //TCPServerStation = null;
                                //TCPServerStation = new System.Net.Sockets.TcpListener(TCPServerIPAddress, TCPServerListeningPort);
                                //ClientToBeAcceptedByServer = null;
                                //TCPServerStream = null;
                                //TCPServerStation.Start();

                                //ErrorMessage = "服务器已开始工作，等待客户端的连接...";  //The server is start workinng and waiting for a connection.

                                }

                            //};

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
                    }

                };

            }

        private void OldStartServerListeningThread()
            {
            
            byte[] ServerReceivedBytes=new byte[1024];

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
                            if (ClientToBeAcceptedByServer.Connected == true)
                                {

                                if (TCPServerStream == null)
                                    {
                                    ErrorMessage = "服务器已成功接受客户端的连接请求..."; //Server connected with the client!  
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
                                    ReceivedString = TempRecord;
                                    ErrorMessage = "Server got: " + TempRecord;
                                    }
                                else
                                    {
                                    //客户端关闭后，服务器无法重新和客户端取得连接并发送数据，虽然客户端已经显示和服务器连接成功。
                                    //故在此加入此语句就可以解决此问题，具体原因有待后来查证
                                    //原本作用:当收到的数据为空时，发送数据给客户端以确保相互之间是一直保持连接的
                                    TCPServerStream.Write(System.Text.Encoding.UTF8.GetBytes("Server"), 0, System.Text.Encoding.UTF8.GetBytes("Server").Length);
                                    ErrorMessage = "Server sent: Server";

                                    }

                                }
                            else 
                                {
                                //如果扫描到客户端没有连接，则释放资源并产生异常
                                //if(!(TCPServerStream==null))
                                //    {
                                //    TCPServerStream.Close();
                                //    TCPServerStream = null;
                                //    }

                                //if (!(ClientToBeAcceptedByServer == null))
                                //    {
                                //    ClientToBeAcceptedByServer.Close();
                                //    ClientToBeAcceptedByServer = null;
                                //    }

                                throw new Exception("未与客户端建立通讯...");

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
        
        #endregion

        #region "获取TcpClient连接的对方IP地址和端口"

        //*********************************
        //方法一：

        //获取客户端的IP地址,空返回值表示未连接
        /// <summary>
        /// 获取客户端的IP地址,空返回值表示未连接
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        public string GetRemoteIP(TcpClient Client)
            {
            try
                {

                if (Client != null)
                    {
                    if (Client.Connected == true)
                        {
                        //获取的远程终端字符串格式为：127.0.0.1:12466
                        Socket TempSocket = Client.Client;
                        EndPoint TempEndPoint = TempSocket.RemoteEndPoint;

                        //方法一：
                        return TempEndPoint.ToString().Split(':')[0];

                        ////方法二：
                        //string TempIPAddress = TempEndPoint.ToString();
                        //string[] TempResult = TempIPAddress.Split(':');
                        //return TempResult[0];
                        }
                    else
                        {
                        return "";
                        }

                    }
                else
                    {
                    return "";
                    }

                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return "";
                }
            }

        //获取客户端的连接端口,-1表示未连接
        /// <summary>
        /// 获取客户端连接的对方端口号,-1表示未连接
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        public int GetRemotePort(TcpClient Client)
            {
            try
                {

                if (Client != null)
                    {
                    if (Client.Connected == true)
                        {
                        //获取的远程终端字符串格式为：127.0.0.1:12466
                        Socket TempSocket = Client.Client;
                        EndPoint TempEndPoint = TempSocket.RemoteEndPoint;

                        //方法一：
                        return Convert.ToInt32(TempEndPoint.ToString().Split(':')[1]);

                        ////方法二：
                        //string TempIPAddress = TempEndPoint.ToString();
                        //string[] TempResult = TempIPAddress.Split(':');
                        //return Convert.ToInt32(TempResult[1]);
                        }
                    else
                        {
                        return -1;
                        }

                    }
                else
                    {
                    return -1;
                    }

                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return -1;
                }
            }

        //TcpClient有一个protected的成员Client，是System.Net.Sockets.Socket类型的对象。
        //System.Net.Sockets.Socket对象是可以得到remote ip和port的,用反射(Reflection).

        //*********************************
        //方法二：
        //获取TcpClient连接的socket
        /// <summary>
        /// 获取TcpClient连接的socket
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        private Socket GetSocket(TcpClient Client)
            {
            try
                {
                PropertyInfo TempResult = Client.GetType().GetProperty("Client");//BindingFlags.NonPublic | BindingFlags.Instance);
                Socket sock = (Socket)TempResult.GetValue(Client, null);
                return sock;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }

        //获取客户端的IP地址
        /// <summary>
        /// 获取客户端的IP地址
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        private string OldGetRemoteIP(TcpClient Client)
            {
            try
                {
                string TempStr = GetSocket(Client).RemoteEndPoint.ToString().Split(':')[0];
                return TempStr;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return "";
                }
            }

        //获取客户端的连接端口
        /// <summary>
        /// 获取客户端的连接端口
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        private int OldGetRemotePort(TcpClient Client)
            {
            try
                {
                string TempStr = GetSocket(Client).RemoteEndPoint.ToString().Split(':')[1];
                UInt16 Port = Convert.ToUInt16(TempStr);
                return Port;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        #endregion

        }//class

    }//namespace