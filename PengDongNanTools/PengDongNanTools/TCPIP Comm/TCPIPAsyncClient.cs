#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Threading;
using Microsoft.VisualBasic;

#endregion

#region "待处理事项"

//如果没有和服务器连接上就断开连接
//目前情况：服务器端断开后客户端不会自动再次连接服务器
//          客户端必须发送一个内容给服务器才能再次建立连接

#endregion

namespace PengDongNanTools
    {

    //以太网TCP/IP异步通讯客户端类【软件作者：彭东南, southeastofstar@163.com】
    /// <summary>
    /// 以太网TCP/IP异步通讯客户端类【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    public class TCPIPAsyncClient
        {

        #region "变量定义"

        //发生错误提示：TimerCallBack不能为空
        //System.Threading.Timer AutoSendTimer = new System.Threading.Timer(null);

        System.Timers.Timer AutoTimer = new System.Timers.Timer();

        private double AutoInterval = 500;

        /// <summary>
        /// 自动发送的时间间隔
        /// </summary>
        public uint AutoSendInterval 
            {
            get { return (uint)AutoInterval; }
            set 
                {
                if (value > 0) 
                    {
                    AutoInterval = (double)value;
                    }
                }
            }

        /// <summary>
        /// 是否自动发送，默认否【如需自动发送，请先设置】
        /// </summary>
        public bool AutoSend 
            {
            get { return AutoTimer.Enabled; }
            set { AutoTimer.Enabled = value; }
            }

        /// <summary>
        /// 用于在已经建立连接后发生错误端口连接后重新建立连接
        /// </summary>
        private bool ConnectSuccess = false;

        /// <summary>
        /// 用于在释放此类的实例时停止线程扫描，防止产生异常
        /// </summary>
        private bool StopThread = false;

        private bool SendBusy = false;

        CommonFunction FC = new CommonFunction("彭东南");

        Socket ClientSocket = null;
        string ServerName = "", ServerIPAddress = "";
        int ServerPort = 0;
        private IPAddress ServerIP = null;

        /// <summary>
        /// 从服务器收到的字符串内容：服务器IP地址 + TAB + 服务器端口号 + TAB + 字符串内容
        /// </summary>
        public string ReceiveMsg = "";

        /// <summary>
        /// 发送到服务器的字符串内容
        /// </summary>
        public string[] SendMessage 
            {
            set 
                {
                if (value == null)
                    {
                    return;
                    }
                else 
                    {
                    SendMsg = new string[value.Length];
                    AutoSendMsg = new string[value.Length];
                    for (int a = 0; a < value.Length; a++) 
                        {
                        SendMsg[a] = value[a];
                        AutoSendMsg[a] = value[a];
                        }
                    }
                }
            }

        /// <summary>
        /// 发送到服务器的字符串内容
        /// </summary>
        private string[] SendMsg = { "Client" }; //new string[1];
        
        /// <summary>
        /// 自动发送到服务器的字符串内容【做缓存用】
        /// </summary>
        private string[] AutoSendMsg = { "" };

        /// <summary>
        /// 是否接收中文字符
        /// </summary>
        public bool ReceiveGB2312Code = false;

        private RichTextBox TempRichTextBox = null;

        private byte[] SendBuffer, ReceiveBuffer;

        /// <summary>
        /// 是否通过服务器的IP建立连接
        /// </summary>
        private bool ConnectToServerByIP = false;

        SocketInformation ClientInfo = new SocketInformation();

        Thread ClientConnectServerThread = null;

        public string ErrorMessage = "";
        private bool SuccessBuiltNew = false, PasswordIsCorrect = false;

        /// <summary>
        /// 是否与服务器建立连接
        /// </summary>
        public bool Connected 
            {
            get 
                {
                if (ClientSocket == null)
                    {
                    return false;
                    }
                else 
                    {
                    if (ClientSocket.Connected == true)
                        {
                        return true;
                        }
                    else 
                        {
                        return false;
                        }
                    }
                }
            }

        /// <summary>
        /// 获取已经建立连接的服务器IP地址,""表示未连接
        /// </summary>
        public string RemoteServerIPAddress 
            {
            get 
                {
                if (ClientSocket != null)
                    {
                    //if (ClientSocket.Connected == true)
                    //    {
                        //获取的远程终端字符串格式为：127.0.0.1:12466
                        EndPoint TempEndPoint = ClientSocket.RemoteEndPoint;

                        //方法一：
                        return TempEndPoint.ToString().Split(':')[0];

                        ////方法二：
                        //string TempIPAddress = TempEndPoint.ToString();
                        //string[] TempResult = TempIPAddress.Split(':');
                        //return TempResult[1];
                    //    }
                    //else
                    //    {
                    //    return "";
                    //    }
                    }
                else
                    {
                    return "";
                    }
                }
            }

        /// <summary>
        /// 获取建立通讯后的服务器端口号,-1表示未连接
        /// </summary>
        public int RemoteServerPort
            {
            get
                {
                if (ClientSocket != null)
                    {
                    //if (ClientSocket.Connected == true)
                    //    {
                    //获取的远程终端字符串格式为：127.0.0.1:12466
                    EndPoint TempEndPoint = ClientSocket.RemoteEndPoint;

                    //方法一：
                    return Convert.ToInt32(TempEndPoint.ToString().Split(':')[1]);

                    ////方法二：
                    //string TempIPAddress = TempEndPoint.ToString();
                    //string[] TempResult = TempIPAddress.Split(':');
                    //return Convert.ToInt32(TempResult[1]);
                    //    }
                    //else
                    //    {
                    //    return -1;
                    //    }
                    }
                else
                    {
                    return -1;
                    }
                }

            }

        /// <summary>
        /// 获取与服务器建立通讯后的客户端端口号,-1表示未连接
        /// </summary>
        public int ClientPort
            {
            get
                {
                if (ClientSocket != null)
                    {
                    //if (ClientSocket.Connected == true)
                    //    {
                        //获取的远程终端字符串格式为：127.0.0.1:12466
                        EndPoint TempEndPoint = ClientSocket.LocalEndPoint;

                        //方法一：
                        return Convert.ToInt32(TempEndPoint.ToString().Split(':')[1]);

                        ////方法二：
                        //string TempIPAddress = TempEndPoint.ToString();
                        //string[] TempResult = TempIPAddress.Split(':');
                        //return Convert.ToInt32(TempResult[1]);
                    //    }
                    //else
                    //    {
                    //    return -1;
                    //    }
                    }
                else
                    {
                    return -1;
                    }
                }

            }

        /// <summary>
        /// 获取客户端本机IP地址
        /// </summary>
        public string ClientIPAddress 
            {
            get { return FC.GetLocalIP4Address(); }
            }

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }

        #endregion

        #region "函数代码"

        //创建以太网TCP/IP异步通讯客户端类的实例：通过服务器名称与服务器建立连接
        /// <summary>
        /// 创建以太网TCP/IP异步通讯客户端类的实例：通过服务器名称与服务器建立连接
        /// </summary>
        /// <param name="TargetServerName">服务器的主机名</param>
        /// <param name="TargetServerPort">服务器的端口号</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public TCPIPAsyncClient(string TargetServerName, int TargetServerPort,
            string DLLPassword)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;
            try
                {

                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan")
                    || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;

                    if (TargetServerName == "")
                        {
                        ErrorMessage = "The parameter 'TargetServerName' can't be empty.";
                        MessageBox.Show("The parameter 'TargetServerName' can't be empty.", "Error");
                        return;
                        }

                    ServerName = TargetServerName;
                    ServerPort = (int)TargetServerPort;
                    ConnectToServerByIP = false;
                    
                    ClientSocket = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

                    if (ClientConnectServerThread == null)
                        {
                        ClientConnectServerThread = new Thread(ClientReadDataFromServer);
                        ClientConnectServerThread.IsBackground = true;
                        ClientConnectServerThread.Start();
                        }

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

        //创建以太网TCP/IP异步通讯客户端类的实例：通过服务器名称与服务器建立连接
        /// <summary>
        /// 创建以太网TCP/IP异步通讯客户端类的实例：通过服务器名称与服务器建立连接
        /// </summary>
        /// <param name="TargetServerName">服务器的主机名</param>
        /// <param name="TargetServerPort">服务器的端口号</param>
        /// <param name="TargetRichTextBox">显示通讯信息的RichTextBox控件</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public TCPIPAsyncClient(string TargetServerName, int TargetServerPort,
            ref RichTextBox  TargetRichTextBox, string DLLPassword)
            {
            
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;

            try
                {

                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan") 
                    || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;

                    if (TargetServerName == "") 
                        {
                        ErrorMessage = "The parameter 'TargetServerName' can't be empty.";
                        MessageBox.Show("The parameter 'TargetServerName' can't be empty.","Error");
                        return;
                        }

                    ServerName = TargetServerName;
                    ServerPort = (int)TargetServerPort;
                    ConnectToServerByIP = false;

                    TempRichTextBox = TargetRichTextBox;
                    FC.UpdatingSameMessage = true;

                    ClientSocket = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

                    if (ClientConnectServerThread == null) 
                        {
                        ClientConnectServerThread = new Thread(ClientReadDataFromServer);
                        ClientConnectServerThread.IsBackground = true;
                        ClientConnectServerThread.Start();
                        }

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

        //创建以太网TCP/IP异步通讯客户端类的实例：通过IP地址与服务器建立连接
        /// <summary>
        /// 创建以太网TCP/IP异步通讯客户端类的实例：通过IP地址与服务器建立连接
        /// </summary>
        /// <param name="TargetServerIPAddress">服务器的IP地址</param>
        /// <param name="ConnectByIPAddress">通过IP地址与服务器建立连接</param>
        /// <param name="TargetServerPort">服务器的端口号</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public TCPIPAsyncClient(string TargetServerIPAddress, bool ConnectByIPAddress,
            int TargetServerPort, string DLLPassword)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;
            try
                {

                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan")
                    || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;

                    if (TargetServerIPAddress == "")
                        {
                        ErrorMessage = "The parameter 'TargetServerIPAddress' can't be empty.";
                        MessageBox.Show("The parameter 'TargetServerIPAddress' can't be empty.", "Error");
                        return;
                        }


                    string[] GetCorrectIPAddress = new string[4];
                    UInt16[] TempGetIPAddress = new UInt16[4];
                    IPAddress TempServerIP = null;

                    //判断输入的服务器IP地址是否正确
                    if (!IPAddress.TryParse(TargetServerIPAddress, out TempServerIP))
                        {
                        MessageBox.Show("The format of IP address for the TCP/IP Server "
                            + TargetServerIPAddress + " is not correct, please check and correct it.\r\n"
                        + "服务器IP地址格式错误，请检查后输入正确IP地址再重新建立新实例.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                        }
                    else
                        {
                        //此处解析IP地址不对："192.168.000.024"解析为"192.168.000.20"
                        GetCorrectIPAddress = Strings.Split(TargetServerIPAddress, ".");
                        for (Int16 a = 0; a < 4; a++)
                            {
                            TempGetIPAddress[a] = Convert.ToUInt16(GetCorrectIPAddress[a]);
                            if (TempGetIPAddress[a] > 254 | TempGetIPAddress[a] < 0)
                                {
                                string TempMsg = "";
                                TempMsg = "The IP address of server: " + TempGetIPAddress[a]
                                    + " is over the range, the correct range for IP address is between 0~255, please correct it.\r\n服务器IP地址: "
                                    + TempGetIPAddress[a] + " 超出有效范围【0~255】，请输入正确IP地址.";
                                MessageBox.Show(TempMsg, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                                }
                            }

                        //设置IP地址时如果某节点不是全部为零0值，则必须去掉此节点的前缀0，否则会报错
                        string Str = TempGetIPAddress[0].ToString() + "." + TempGetIPAddress[1].ToString() + "." + TempGetIPAddress[2].ToString() + "." + TempGetIPAddress[3].ToString();
                        ServerIP = IPAddress.Parse(Str);

                        ServerIPAddress = Str;
                        //ServerIPAddress = TargetServerIPAddress.ToString();

                        }

                    ServerPort = TargetServerPort;
                    ConnectToServerByIP = true;

                    ClientSocket = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

                    if (ClientConnectServerThread == null)
                        {
                        ClientConnectServerThread = new Thread(ClientReadDataFromServer);
                        ClientConnectServerThread.IsBackground = true;
                        ClientConnectServerThread.Start();
                        }

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

        //创建以太网TCP/IP异步通讯客户端类的实例：通过IP地址与服务器建立连接
        /// <summary>
        /// 创建以太网TCP/IP异步通讯客户端类的实例：通过IP地址与服务器建立连接
        /// </summary>
        /// <param name="TargetServerIPAddress">服务器的IP地址</param>
        /// <param name="ConnectByIPAddress">通过IP地址与服务器建立连接</param>
        /// <param name="TargetServerPort">服务器的端口号</param>
        /// <param name="TargetRichTextBox">显示通讯信息的RichTextBox控件</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public TCPIPAsyncClient(string TargetServerIPAddress, bool ConnectByIPAddress,
            int TargetServerPort, ref RichTextBox TargetRichTextBox, string DLLPassword)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;
            try
                {

                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan")
                    || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;

                    if (TargetServerIPAddress == "")
                        {
                        ErrorMessage = "The parameter 'TargetServerIPAddress' can't be empty.";
                        MessageBox.Show("The parameter 'TargetServerIPAddress' can't be empty.", "Error");
                        return;
                        }

                    
                    string[] GetCorrectIPAddress = new string[4];
                    UInt16[] TempGetIPAddress = new UInt16[4];
                    IPAddress TempServerIP=null;

                    //判断输入的服务器IP地址是否正确
                    if (!IPAddress.TryParse(TargetServerIPAddress, out TempServerIP))
                        {
                        MessageBox.Show("The format of IP address for the TCP/IP Server "
                            + TargetServerIPAddress + " is not correct, please check and correct it.\r\n"
                        + "服务器IP地址格式错误，请检查后输入正确IP地址再重新建立新实例.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                        }
                    else
                        {
                        //此处解析IP地址不对："192.168.000.024"解析为"192.168.000.20"
                        GetCorrectIPAddress = Strings.Split(TargetServerIPAddress, ".");
                        for (Int16 a = 0; a < 4; a++)
                            {
                            TempGetIPAddress[a] = Convert.ToUInt16(GetCorrectIPAddress[a]);
                            if (TempGetIPAddress[a] > 254 | TempGetIPAddress[a] < 0)
                                {
                                string TempMsg = "";
                                TempMsg = "The IP address of server: " + TempGetIPAddress[a]
                                    + " is over the range, the correct range for IP address is between 0~255, please correct it.\r\n服务器IP地址: "
                                    + TempGetIPAddress[a] + " 超出有效范围【0~255】，请输入正确IP地址.";
                                MessageBox.Show(TempMsg, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                                }
                            }

                        //设置IP地址时如果某节点不是全部为零0值，则必须去掉此节点的前缀0，否则会报错
                        string Str = TempGetIPAddress[0].ToString() + "." + TempGetIPAddress[1].ToString() + "." + TempGetIPAddress[2].ToString() + "." + TempGetIPAddress[3].ToString();
                        ServerIP = IPAddress.Parse(Str);

                        ServerIPAddress = Str;
                        //ServerIPAddress = TargetServerIPAddress.ToString();

                        }

                    ServerPort = (int)TargetServerPort;
                    ConnectToServerByIP = true;

                    TempRichTextBox = TargetRichTextBox;
                    FC.UpdatingSameMessage = true;

                    ClientSocket = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

                    if (ClientConnectServerThread == null)
                        {
                        ClientConnectServerThread = new Thread(ClientReadDataFromServer);
                        ClientConnectServerThread.IsBackground = true;
                        ClientConnectServerThread.Start();
                        }

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

        //异步通讯客户端与服务器通讯的函数
        /// <summary>
        /// 异步通讯客户端与服务器通讯的函数
        /// </summary>
        private void ClientReadDataFromServer() 
            {
            //int Return = 0;

            ClientSocket.SendBufferSize = 1024;
            ClientSocket.ReceiveBufferSize = 1024;

            SendBuffer = new byte[ClientSocket.SendBufferSize];
            ReceiveBuffer = new byte[ClientSocket.ReceiveBufferSize];

            AutoTimer.Elapsed += new System.Timers.ElapsedEventHandler(AutoTimer_Elapsed);
            AutoTimer.Interval = 500;

            //使用定时器实现每隔一定时间间隔给服务器发一个消息
            System.Timers.Timer HeartBeatTimer = new System.Timers.Timer();
            HeartBeatTimer.Interval = AutoInterval;

            //与下面没有被注释掉的代码行作用相同
            //HeartBeatTimer.Elapsed += (o, a) =>
            HeartBeatTimer.Elapsed += (object Sender, System.Timers.ElapsedEventArgs e) =>
            {
                if (AutoSend == true)
                    {
                    if (ClientSocket != null) 
                        {
                        if (ClientSocket.Connected == true)
                        {
                        try
                            {
                            //TempClientSocket

                            for (int a = 0; a < AutoSendMsg.Length; a++)
                                {
                                if (AutoSendMsg[a] == "")
                                    {
                                    continue;
                                    }

                                if (ReceiveGB2312Code == true)
                                    {
                                    ClientSocket.Send(Encoding.GetEncoding("GB2312").GetBytes(FC.GetLocalIP4Address()
                                        + "    " + FC.GetLocalPort(ClientSocket) + "    "
                                        + AutoSendMsg[a] + "\r\n"));
                                    }
                                else
                                    {
                                    ClientSocket.Send(Encoding.UTF8.GetBytes(FC.GetLocalIP4Address()
                                        + "    " + FC.GetLocalPort(ClientSocket) + "    "
                                        + AutoSendMsg[a] + "\r\n"));
                                    }

                                FC.AddRichText(ref TempRichTextBox, "异步服务器 " + FC.GetLocalIP4Address() + "  "
                                    + FC.GetLocalPort(ClientSocket) + " 发送到客户端 " + FC.GetRemoteIP(ClientSocket) + "  "
                                        + FC.GetRemotePort(ClientSocket) + "，内容：" + SendMsg[a]);

                                }
                            }
                        catch (Exception ex)
                            {
                            ErrorMessage = ex.Message;
                            FC.AddRichText(ref TempRichTextBox, ex.Message);
                            }
                        }

                        //如果没有及时收到服务器的心跳包就断开当前连接
                        if (ReceiveMsg == "" && ConnectSuccess==true) 
                            {
                            ConnectSuccess = false;
                            ClientSocket.Close();
                            }
                        }
                    
                    else
                        {
                        HeartBeatTimer.Stop();
                        }
                    }

            };
            HeartBeatTimer.Enabled = true;

            //ClientSocket.UseOnlyOverlappedIO = true;

            //while (true)
            //    {
                while (StopThread == false)
                    {
                    try
                        {
                        if (ClientSocket != null)
                            {
                            if (ClientSocket.Connected == false)
                                {
                                //Connect to remote server
                                if (ConnectToServerByIP == false)
                                    {
                                    ClientSocket.Connect(ServerName, ServerPort);
                                    }
                                else
                                    {
                                    ClientSocket.Connect(ServerIP, ServerPort);
                                    }
                                }
                            }


                        if (ClientSocket != null)
                            {

                            if (ClientSocket.Connected == true)
                                {
                                //已经和服务器成功建立异步连接
                                ConnectSuccess = true;
                                
                                ClientSocket.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length,
                                    SocketFlags.None, new AsyncCallback(ReceiveMessage), ClientSocket);

                                //SendBuffer = Encoding.UTF8.GetBytes("Client..");
                                //ClientSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);

                                ////**************************
                                ////发送字符串到服务器
                                //if (SendMsg.Length != 0)
                                //    {
                                //    for (int a = 0; a < SendMsg.Length; a++)
                                //        {
                                //        if (SendMsg[a] == "") 
                                //            {
                                //            continue;
                                //            }
                                //        if (ReceiveGB2312Code == false)
                                //            {
                                //            SendBuffer = Encoding.UTF8.GetBytes(SendMsg[a]);
                                //            ClientSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                                //            }
                                //        else
                                //            {
                                //            SendBuffer = Encoding.GetEncoding("GB2312").GetBytes(SendMsg[a]);
                                //            ClientSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                                //            }
                                //        SendMsg[a] = "";
                                //        }
                                //    }
                                ////**************************
                                ////因为如果此时发送数据给服务器，就出现服务器收不到正确数量的内容
                                ////故加一个发送忙的标志SendBusy
                                //do
                                //    {
                                //    ;
                                //    }
                                //while (SendBusy == true);

                                //**************************

                                //if (SendMsg != "") 
                                //    {
                                //    if (ReceiveGB2312Code == false)
                                //        {
                                //        SendBuffer = Encoding.UTF8.GetBytes(SendMsg);
                                //        ClientSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                                //        }
                                //    else
                                //        {
                                //        SendBuffer = Encoding.GetEncoding("GB2312").GetBytes(SendMsg);
                                //        ClientSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                                //        }
                                //    SendMsg = "";
                                //    }

                                //if (TempStr.Length != 0 && TempStr != "") 
                                //    {
                                //    FC.AddRichText(ref TempRichTextBox, "客户端从本地端口 " + FC.GetLocalPort(ClientSocket)
                                //    + " 从服务器 " + FC.GetRemoteIP(ClientSocket) + " 的端口： "
                                //    + FC.GetRemotePort(ClientSocket) + "，收到文本内容：" + TempStr);
                                //    TempStr = "";
                                //    }
                                }
                            //else 
                            //    {
                            //    //ClientSocket.BeginSend(
                            //    ClientSocket = null;
                            //    }
                            }
                        //else 
                        //    {
                        //    ClientSocket = new Socket(AddressFamily.InterNetwork,
                        //    SocketType.Stream, ProtocolType.Tcp);
                        //    //ClientSocket.Close();
                        //    }
                        }
                    catch (Exception ex)
                        {
                        ErrorMessage = ex.Message;
                        //FC.AddRichText(ref TempRichTextBox, ex.Message);

                        if (ClientSocket != null)//  (TempSocket != null)
                            {
                            if (ConnectSuccess == true && ClientSocket.Connected == false)// TempSocket.Connected==false)
                                {
                                //出现异常后重新与服务器建立连接
                                ClientSocket = null;
                                ClientSocket = new Socket(AddressFamily.InterNetwork,
                                    SocketType.Stream, ProtocolType.Tcp);
                                ConnectSuccess = false;
                                }
                            }
                        }
                    }
                //}
            }

        string TempStr = "";
        int ReceiveLength = 0;

        Socket TempSocket = null;

        //接收服务器消息的异步委托执行函数
        /// <summary>
        /// 接收服务器消息的异步委托执行函数
        /// </summary>
        /// <param name="ClientAsyncOperation"></param>
        private void ReceiveMessage(IAsyncResult ClientAsyncOperation) 
            {
            try
                {
                string TempString = "";
                TempSocket = (Socket)ClientAsyncOperation.AsyncState;// as Socket;
                
                try
                    {
                    if (TempSocket != null)
                        {
                        if (TempSocket.Connected == true)
                            {
                            ReceiveLength = TempSocket.EndReceive(ClientAsyncOperation);

                            if (ReceiveLength > 0)
                                {

                                if (ReceiveGB2312Code == false)
                                    {
                                    TempString = Encoding.UTF8.GetString(ReceiveBuffer,
                                        0, ReceiveLength);
                                    //TempStr = ReceiveStr;
                                    //FC.AddRichText(ref TempRichTextBox, TempStr);
                                    }
                                else
                                    {
                                    TempString = Encoding.GetEncoding("GB2312").GetString(ReceiveBuffer,
                                        0, ReceiveLength);
                                    //TempStr = ReceiveStr;
                                    //FC.AddRichText(ref TempRichTextBox, TempStr);
                                    }

                                ReceiveMsg = FC.GetRemoteIP(ClientSocket) + "   " + FC.GetRemotePort(ClientSocket) + "   " + TempString;
                                TempStr = TempString;
                                TempString = "";
                                FC.AddRichText(ref TempRichTextBox, "异步客户端从本地端口 " + FC.GetLocalPort(ClientSocket)
                                    + " 从服务器 " + FC.GetRemoteIP(ClientSocket) + " 的端口： "
                                    + FC.GetRemotePort(ClientSocket) + "，收到文本内容：" + TempStr);

                                TempStr = "";

                                //****************
                                //因为之前在写程序调试的时候，发现很多时候收到54个'\0'的字节，故增加以下进行排除
                                //后来在线程里面加入判断是否与服务器建立连接，建立连接就不新建实例，否则断开连接就新建实例
                                //之前收到的54个'\0'的字节，可能就是没有加上面的判断，所以一直新建socket实例而导致
                                //string TempS = Strings.StrDup(ReceiveLength, '\0');
                                //if (Strings.StrComp(TempS, TempString,CompareMethod.Binary) != 0)
                                //    {
                                //    ReceiveMsg = FC.GetRemoteIP(ClientSocket) + "   " + FC.GetRemotePort(ClientSocket) + "   " + TempString;
                                //    TempStr = TempString;
                                //    TempString = "";
                                //    FC.AddRichText(ref TempRichTextBox, "客户端从本地端口 " + FC.GetLocalPort(ClientSocket)
                                //        + " 从服务器 " + FC.GetRemoteIP(ClientSocket) + " 的端口： "
                                //        + FC.GetRemotePort(ClientSocket) + "，收到文本内容：" + TempStr);

                                //    TempStr = "";
                                //    }
                                }

                            //**************************
                            //发送字符串到服务器
                            if (SendMsg.Length != 0)
                                {
                                for (int a = 0; a < SendMsg.Length; a++)
                                    {
                                    if (SendMsg[a] == "")
                                        {
                                        continue;
                                        }
                                    if (ReceiveGB2312Code == false)
                                        {
                                        SendBuffer = Encoding.UTF8.GetBytes(SendMsg[a]);
                                        //ClientSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                                        TempSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                                        }
                                    else
                                        {
                                        SendBuffer = Encoding.GetEncoding("GB2312").GetBytes(SendMsg[a]);
                                        //ClientSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                                        TempSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                                        }

                                    FC.AddRichText(ref TempRichTextBox, "异步客户端 " + FC.GetLocalIP4Address() + "  " + FC.GetLocalPort(TempSocket)
                                                + " 发送到服务器 " + FC.GetRemoteIP(TempSocket) + "  "
                                                + FC.GetRemotePort(TempSocket) + "，内容：" + SendMsg[a]);

                                    SendMsg[a] = "";
                                    }
                                }

                            //**************************
                            ////发送字符串到服务器
                            //if (SendMsg != "")
                            //    {
                            //    if (ReceiveGB2312Code == false)
                            //        {
                            //        SendBuffer = Encoding.UTF8.GetBytes(SendMsg);
                            //        ClientSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                            //        }
                            //    else
                            //        {
                            //        SendBuffer = Encoding.GetEncoding("GB2312").GetBytes(SendMsg);
                            //        ClientSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                            //        }
                            //    SendMsg = "";
                            //    }

                            //**************************
                            ////因为如果此时发送数据给服务器，就出现服务器收不到正确数量的内容
                            ////故加一个发送忙的标志SendBusy
                            //do
                            //    {
                            //    ;
                            //    }
                            //while (SendBusy == true);

                            SendBuffer = Encoding.UTF8.GetBytes("Client..");
                            TempSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                            
                            //**************************
                            TempSocket.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length,
                                SocketFlags.None, new AsyncCallback(ReceiveMessage), TempSocket);

                            }
                        else
                            {
                            //如果没有和服务器连接上就断开连接
                            //目前情况：服务器端断开后客户端不会自动再次连接服务器
                            //          客户端必须发送一个内容给服务器才能再次建立连接
                            TempSocket.Close();
                            //return;
                            }
                        }
                    else 
                        {
                        return;
                        }
                    //此处屏蔽和不屏蔽没有不同
                    TempSocket = null;

                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    //FC.AddRichText(ref TempRichTextBox, ex.Message);

                    if (TempSocket != null)
                        {
                        if (TempSocket.Connected == false) 
                            {
                            TempSocket.Close();
                            //return;
                            }
                        }
                    //出现异常后重新与服务器建立连接
                    //ClientSocket = null;
                    //ClientSocket = new Socket(AddressFamily.InterNetwork,
                    //    SocketType.Stream, ProtocolType.Tcp);
                    //return;
                    //FC.AddRichText(ref TempRichTextBox, ex.Message);
                    }
                }
            catch (Exception ex) 
                {
                ErrorMessage = ex.Message;
                //FC.AddRichText(ref TempRichTextBox, ex.Message);
                return;
                }            
            }

        //发送字符串给服务器
        //【因为是异步发送，如果服务器在发送数据给客户端，这时发送给服务器就可能会造成堵塞】
        /// <summary>
        /// 发送字符串给服务器
        /// 【因为是异步发送，如果服务器在发送数据给客户端，这时发送给服务器就可能会造成堵塞】
        /// 【建议给属性'SendMessage'赋值，经过特殊处理后可以保证发送成功】
        /// </summary>
        /// <param name="MessageToBeSent">发送的字符串</param>
        /// <returns>是否发送成功</returns>
        private bool Send(string MessageToBeSent)
            {
            if (MessageToBeSent == "") 
                {
                //ErrorMessage = "The message to be sent can't be empty.";
                //FC.AddRichText(ref TempRichTextBox, "The message to be sent can't be empty.");
                return false;
                }
            try
                {
                if (TempSocket != null)//(ClientSocket != null)
                    {
                    if (TempSocket.Connected == true)//(ClientSocket.Connected == true)
                        {
                        SendBusy = true;
                        if (ReceiveGB2312Code == false)
                            {
                            SendBuffer = Encoding.UTF8.GetBytes(MessageToBeSent);
                            //ClientSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                            TempSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);

                            //FC.AddRichText(ref TempRichTextBox, "客户端从本地端口 " + FC.GetLocalPort(ClientSocket) 
                            //    + " 成功发送到服务器 " + FC.GetRemoteIP(ClientSocket) + " 的端口： " 
                            //    + FC.GetRemotePort(ClientSocket) + "，文本内容：" + MessageToBeSent);
                            }
                        else
                            {
                            SendBuffer = Encoding.GetEncoding("GB2312").GetBytes(MessageToBeSent);
                            //ClientSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                            TempSocket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);

                            //FC.AddRichText(ref TempRichTextBox, "客户端从本地端口 " + FC.GetLocalPort(ClientSocket)
                            //    + " 成功发送到服务器 " + FC.GetRemoteIP(ClientSocket) + " 的端口： "
                            //    + FC.GetRemotePort(ClientSocket) + "，文本内容：" + MessageToBeSent);
                            }
                        FC.AddRichText(ref TempRichTextBox, "客户端 " + FC.GetLocalIP4Address() + "  " + FC.GetLocalPort(TempSocket)
                                + " 发送到服务器 " + FC.GetRemoteIP(TempSocket) + "  "
                                + FC.GetRemotePort(TempSocket) + "，内容：" + MessageToBeSent);
                        
                        //FC.AddRichText(ref TempRichTextBox, "客户端 " + FC.GetLocalIP4Address() + "  " + FC.GetLocalPort(ClientSocket)
                        //        + " 发送到服务器 " + FC.GetRemoteIP(ClientSocket) + "  "
                        //        + FC.GetRemotePort(ClientSocket) + "，内容：" + MessageToBeSent);

                        SendBusy = false;
                        return true;
                        }
                    else 
                        {
                        return false;
                        }
                    }
                else
                    {
                    return false;
                    }
                }
            catch (Exception ex)
                {
                SendBusy = false;
                ErrorMessage = ex.Message;
                FC.AddRichText(ref TempRichTextBox, ex.Message);
                return false;
                }
            }

        //关闭异步通讯客户端并释放相关资源
        /// <summary>
        /// 关闭异步通讯客户端并释放相关资源
        /// </summary>
        public void Close() 
            {
            try
                {
                FC.Dispose();
                StopThread = true;

                AutoTimer.Close();

                ClientConnectServerThread = null;
                ServerIP = null;
                ClientSocket.Close();
                ClientSocket = null;

                if (TempRichTextBox != null) 
                    {
                    TempRichTextBox = null;
                    }
                GC.Collect();
                }
            catch (Exception)
                {
                }
            }

        //自动发送的Timer事件函数
        /// <summary>
        /// 自动发送的Timer事件函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
            {
            try
                {
                if (AutoSendMsg != null) 
                    {
                    if (AutoSendMsg.Length > 0) 
                        {
                        for (int a = 0; a < AutoSendMsg.Length; a++)
                            {
                            SendMsg[a] = AutoSendMsg[a];
                            //Send(SendMsg[a]);
                            }
                        }
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                FC.AddRichText(ref TempRichTextBox, ex.Message);
                }
            }

        #endregion

        }//class

    }//namespace