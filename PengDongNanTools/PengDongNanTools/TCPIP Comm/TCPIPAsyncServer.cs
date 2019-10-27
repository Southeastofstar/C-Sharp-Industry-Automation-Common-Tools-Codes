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

namespace PengDongNanTools
    {

    //TCP/IP异步通讯服务器类【软件作者：彭东南, southeastofstar@163.com】
    /// <summary>
    /// TCP/IP异步通讯服务器类【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    public class TCPIPAsyncServer
        {

        #region "变量定义"

        /// <summary>
        /// Socket存放客户端Socket套接字
        /// </summary>
        Dictionary<string, Socket> SocketDict = new Dictionary<string, Socket>();//存放套接字

        /// <summary>
        /// 返回服务器已经建立连接的客户端IP地址和端口号的字符串数组
        /// </summary>
        public string[] ConnectedClients 
            {
            get 
                {
                if (SocketDict != null)
                    {
                    return SocketDict.Keys.ToArray();
                    }
                else 
                    {
                    return null;
                    }
                }
            }

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
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

        Socket ServerSocket = null;
        int ListenPort;
        int ListenQty;
        string ClientName = "", ClientIPAddress = "";
        int ClientPort = 0;
        private IPAddress ClientIP = null;

        /// <summary>
        /// 从客户端收到的字符串内容：客户端IP地址 + TAB + 客户端端口号 + TAB + 字符串内容
        /// </summary>
        public string ReceiveMsg = "";

        /// <summary>
        /// 发送到客户端的字符串内容
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
        /// 发送到客户端的字符串内容
        /// </summary>
        private string[] SendMsg = { "Server" }; //new string[1];

        /// <summary>
        /// 自动发送到客户端的字符串内容【做缓存用】
        /// </summary>
        private string[] AutoSendMsg = { "" };

        /// <summary>
        /// 是否接收中文字符
        /// </summary>
        public bool ReceiveGB2312Code = false;

        private RichTextBox TempRichTextBox = null;

        private byte[] SendBuffer, ReceiveBuffer;

        SocketInformation ServerInfo = new SocketInformation();

        Thread ServerConnectClientThread = null;

        public string ErrorMessage = "";
        private bool SuccessBuiltNew = false, PasswordIsCorrect = false;

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

        #endregion

        #region "函数代码"

        //创建以太网TCP/IP异步通讯服务器类的实例
        /// <summary>
        /// 创建以太网TCP/IP异步通讯服务器类的实例
        /// </summary>
        /// <param name="TargetListenPort">异步服务器监听端口号</param>
        /// <param name="TargetListenQty">异步服务器监听队列的数量</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public TCPIPAsyncServer(int TargetListenPort, int TargetListenQty, 
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

                    ListenPort = TargetListenPort;
                    ListenQty = TargetListenQty;

                    ////创建一个新的Socket,基于TCP的Stream Socket（流式套接字）
                    //ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    ////将该socket绑定到主机上面的某个端口
                    //ServerSocket.Bind(new IPEndPoint(IPAddress.Any, TargetListenPort));

                    ////启动监听并设置监听队列长度
                    //ServerSocket.Listen(TargetListenQty);

                    InitalSocet();

                    ////启动线程接受客户端连接
                    //if (ServerConnectClientThread == null) 
                    //    {
                    //    ServerConnectClientThread = new Thread(StartServer);
                    //    ServerConnectClientThread.IsBackground = true;
                    //    ServerConnectClientThread.Start();
                    //    }

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

        //创建以太网TCP/IP异步通讯服务器类的实例
        /// <summary>
        /// 创建以太网TCP/IP异步通讯服务器类的实例
        /// </summary>
        /// <param name="TargetListenPort">异步服务器监听端口号</param>
        /// <param name="TargetListenQty">异步服务器监听队列的数量</param>
        /// <param name="TargetRichTextBox">显示提示信息的RichTextBox控件</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public TCPIPAsyncServer(int TargetListenPort, int TargetListenQty, 
            ref RichTextBox TargetRichTextBox, string DLLPassword)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;
            try
                {
                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan")
                    || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;

                    ListenPort = TargetListenPort;
                    ListenQty = TargetListenQty;

                    ////创建一个新的Socket,基于TCP的Stream Socket（流式套接字）
                    //ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    ////将该socket绑定到主机上面的某个端口
                    //ServerSocket.Bind(new IPEndPoint(IPAddress.Any, TargetListenPort));

                    ////启动监听并设置监听队列长度
                    //ServerSocket.Listen(TargetListenQty);

                    TempRichTextBox = TargetRichTextBox;

                    InitalSocet();

                    ////启动线程接受客户端连接
                    //if (ServerConnectClientThread == null)
                    //    {
                    //    ServerConnectClientThread = new Thread(StartServer);
                    //    ServerConnectClientThread.IsBackground = true;
                    //    ServerConnectClientThread.Start();
                    //    }

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

        //初始化服务器Socket
        /// <summary>
        /// 初始化服务器Socket
        /// </summary>
        private void InitalSocet() 
            {
            try
                {

                AutoTimer.Elapsed += new System.Timers.ElapsedEventHandler(AutoTimer_Elapsed);
                AutoTimer.Enabled = true;
                AutoTimer.Start();

                //创建一个新的Socket,基于TCP的Stream Socket（流式套接字）
                ServerSocket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                //将该socket绑定到主机上面的某个端口
                ServerSocket.Bind(new IPEndPoint(IPAddress.Any, ListenPort));

                //启动监听并设置监听队列长度
                ServerSocket.Listen(ListenQty);

                ErrorMessage = "异步 Server is listening the client now...";
                FC.AddRichText(ref TempRichTextBox, "异步 Server is listening the client now...");

                //开始接受客户端连接请求
                ServerSocket.BeginAccept(new AsyncCallback(ServerAcceptClient), ServerSocket);

                }
            catch (Exception ex) 
                {
                ErrorMessage = ex.Message;
                //FC.AddRichText(ref TempRichTextBox, ex.Message);
                }
            }

        //启动线程接受客户端连接
        /// <summary>
        /// 启动线程接受客户端连接
        /// </summary>
        private void StartServer() 
            {
            try
                {
                AutoTimer.Enabled = true;
                AutoTimer.Start();

                //创建一个新的Socket,基于TCP的Stream Socket（流式套接字）
                ServerSocket = new Socket(AddressFamily.InterNetwork, 
                    SocketType.Stream, ProtocolType.Tcp);

                //将该socket绑定到主机上面的某个端口
                ServerSocket.Bind(new IPEndPoint(IPAddress.Any, ListenPort));

                //启动监听并设置监听队列长度
                ServerSocket.Listen(ListenQty);

                ErrorMessage = "Server is listening the client now...";
                FC.AddRichText(ref TempRichTextBox, "Server is listening the client now...");

                while (true) 
                    {
                    try
                        {
                        //开始接受客户端连接请求
                        ServerSocket.BeginAccept(new AsyncCallback(ServerAcceptClient), ServerSocket);
                        }
                    catch (Exception ex)
                        {
                        ErrorMessage = ex.Message;
                        FC.AddRichText(ref TempRichTextBox, ex.Message);
                        }
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                FC.AddRichText(ref TempRichTextBox, ex.Message);
                }
            }

        Socket TempUseSocket = null;

        //接受客户端连接
        /// <summary>
        /// 接受客户端连接
        /// </summary>
        /// <param name="TargetServerSocket"></param>
        private void ServerAcceptClient(IAsyncResult TargetServerSocket) 
            {

            try
                {
                Socket TempServerSocket = (Socket)TargetServerSocket.AsyncState;// as Socket;

                //获取客户端的实例
                Socket TempClientSocket = TempServerSocket.EndAccept(TargetServerSocket);
                
                //将客户端Socket复制一份，用来处理断开连接后从字典里去除
                TempUseSocket = TempClientSocket;

                try
                    {
                    TempClientSocket.SendBufferSize = 1024;
                    TempClientSocket.ReceiveBufferSize = 1024;

                    ReceiveBuffer = new byte[TempClientSocket.ReceiveBufferSize];
                    SendBuffer = new byte[TempClientSocket.SendBufferSize];

                    TempClientSocket.Send(Encoding.UTF8.GetBytes("Message from Server at " + DateTime.Now.ToString() + ".\r\n"));

                    //如果字典中不含当前连接的客户端就添加客户端IP地址和端口号到字典中
                    if (SocketDict.ContainsKey(FC.GetRemoteIP(TempClientSocket) + "," + FC.GetRemotePort(TempClientSocket)) == false)
                        {
                        SocketDict.Add(FC.GetRemoteIP(TempClientSocket) + "," + FC.GetRemotePort(TempClientSocket), TempClientSocket);
                        }

                    //**************************
                    //发送字符串到客户端
                    if (SendMsg != null)
                        {
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
                                    //TempClient.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                                    TempClientSocket.Send(SendBuffer);
                                    }
                                else
                                    {
                                    SendBuffer = Encoding.GetEncoding("GB2312").GetBytes(SendMsg[a]);
                                    //TempClient.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                                    TempClientSocket.Send(SendBuffer);
                                    }

                                FC.AddRichText(ref TempRichTextBox, "异步服务器 " + FC.GetLocalIP4Address() + "  " 
                                    + FC.GetLocalPort(TempClientSocket) + " 发送到客户端 " + FC.GetRemoteIP(TempClientSocket) + "  "
                                            + FC.GetRemotePort(TempClientSocket) + "，内容：" + SendMsg[a]);

                                SendMsg[a] = "";
                                }
                            }
                        }

                    //**************************

                    //使用定时器实现每隔一定时间间隔给服务器发一个消息
                    System.Timers.Timer HeartBeatTimer = new System.Timers.Timer();
                    HeartBeatTimer.Interval = AutoInterval;

                    //与下面没有被注释掉的代码行作用相同
                    //HeartBeatTimer.Elapsed += (o, a) =>
                    HeartBeatTimer.Elapsed += (object Sender, System.Timers.ElapsedEventArgs e) =>
                    {
                    if (AutoSend == true) 
                        {
                        if (TempClientSocket.Connected == true)
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
                                        TempClientSocket.Send(Encoding.GetEncoding("GB2312").GetBytes(FC.GetLocalIP4Address()
                                            + "    " + FC.GetLocalPort(TempClientSocket) + "    "
                                            + AutoSendMsg[a] + "\r\n"));
                                        }
                                    else
                                        {
                                        TempClientSocket.Send(Encoding.UTF8.GetBytes(FC.GetLocalIP4Address()
                                            + "    " + FC.GetLocalPort(TempClientSocket) + "    "
                                            + AutoSendMsg[a] + "\r\n"));
                                        }

                                    FC.AddRichText(ref TempRichTextBox, "异步服务器 " + FC.GetLocalIP4Address() + "  "
                                        + FC.GetLocalPort(TempClientSocket) + " 发送到客户端 " + FC.GetRemoteIP(TempClientSocket) + "  "
                                            + FC.GetRemotePort(TempClientSocket) + "，内容：" + SendMsg[a]);

                                    }

                                }
                            catch (Exception ex)
                                {
                                ErrorMessage = ex.Message;
                                FC.AddRichText(ref TempRichTextBox, ex.Message);
                                }
                            }
                        else
                            {
                            HeartBeatTimer.Stop();
                            }
                        }
                        
                    };
                    HeartBeatTimer.Enabled = true;

                    //开始异步接收客户端的消息
                    TempClientSocket.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length,
                        SocketFlags.None, new AsyncCallback(ReceiveMessage), TempClientSocket);

                    if (TempClientSocket.Connected == false) 
                        {
                        SocketDict.Remove(FC.GetRemoteIP(TempClientSocket) + "," + FC.GetRemotePort(TempClientSocket));
                        }

                    //接受下一个客户端请求
                    TempServerSocket.BeginAccept(new AsyncCallback(ServerAcceptClient), TempServerSocket);

                    //TempClientSocket = null;

                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    //FC.AddRichText(ref TempRichTextBox, ex.Message);

                    if (TempClientSocket != null) 
                        {
                        if (TempClientSocket.Connected == false)
                            {
                            TempDictKey = FC.GetRemoteIP(TempClientSocket) + ","
                                + FC.GetRemotePort(TempClientSocket).ToString();
                            if (SocketDict.Remove(TempDictKey) == false)
                                {
                                ErrorMessage = "Failed to remove the client key: " + TempDictKey;
                                //FC.AddRichText(ref TempRichTextBox, ErrorMessage);
                                }
                            //SocketDict.Remove(FC.GetRemoteIP(TempClientSocket) + "," 
                            //    + FC.GetRemotePort(TempClientSocket).ToString());
                            }
                        }
                    }
                }
            catch (Exception ex) 
                {
                ErrorMessage = ex.Message;
                //FC.AddRichText(ref this.TempRichTextBox, ex.Message);

                if (TempUseSocket != null) 
                    {
                    if (TempUseSocket.Connected == false)
                        {
                        TempDictKey = FC.GetRemoteIP(TempUseSocket) + ","
                            + FC.GetRemotePort(TempUseSocket).ToString();
                        if (SocketDict.Remove(TempDictKey) == false)
                            {
                            ErrorMessage = "Failed to remove the client key: " + TempDictKey;
                            //FC.AddRichText(ref TempRichTextBox, ErrorMessage);
                            }
                        //SocketDict.Remove(FC.GetRemoteIP(TempUseSocket) + ","
                        //    + FC.GetRemotePort(TempUseSocket).ToString());
                        }
                    }
                }
            }

        string TempStr = "";
        string TempDictKey = "";
        int ReceiveLength = 0;

        //接收客户端的消息
        /// <summary>
        /// 接收客户端的消息
        /// </summary>
        /// <param name="ServerSocketAsyncOperation">传入的异步操作对象</param>
        private void ReceiveMessage(IAsyncResult ServerSocketAsyncOperation)
            {
            try
                {
                string TempString = "";
                Socket TempClient = (Socket)ServerSocketAsyncOperation.AsyncState;// as Socket;

                TempUseSocket = TempClient;

                try
                    {
                    
                    if (TempClient != null)
                        {
                        if (TempClient.Connected == true)
                            {
                            //结束挂起的异步读取并返回读取到的数据长度
                            ReceiveLength = TempClient.EndReceive(ServerSocketAsyncOperation);

                            if (ReceiveLength > 0)
                                {

                                if (ReceiveGB2312Code == false)
                                    {
                                    TempString = Encoding.UTF8.GetString(ReceiveBuffer,
                                        0, ReceiveLength);
                                    }
                                else
                                    {
                                    TempString = Encoding.GetEncoding("GB2312").GetString(ReceiveBuffer,
                                        0, ReceiveLength);
                                    }

                                ReceiveMsg = FC.GetRemoteIP(TempClient) + "   " + FC.GetRemotePort(TempClient) + "   " + TempString;
                                TempStr = TempString;
                                TempString = "";
                                FC.AddRichText(ref TempRichTextBox, "异步服务器 " + FC.GetLocalIP4Address() + "  " 
                                    + FC.GetLocalPort(TempClient) + " 从客户端 " + FC.GetRemoteIP(TempClient) + "  "
                                    + FC.GetRemotePort(TempClient) + "，收到：" + TempStr);

                                TempStr = "";

                                }

                            //**************************
                            //发送字符串到客户端
                            if (SendMsg != null)
                                {
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
                                            //TempClient.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                                            TempClient.Send(SendBuffer);
                                            }
                                        else
                                            {
                                            SendBuffer = Encoding.GetEncoding("GB2312").GetBytes(SendMsg[a]);
                                            //TempClient.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, null, null);
                                            TempClient.Send(SendBuffer);
                                            }

                                        FC.AddRichText(ref TempRichTextBox, "异步服务器 " + FC.GetLocalIP4Address() + "  " 
                                            + FC.GetLocalPort(TempClient) + " 发送到客户端 " + FC.GetRemoteIP(TempClient) + "  "
                                            + FC.GetRemotePort(TempClient) + "，内容：" + SendMsg[a]);

                                        SendMsg[a] = "";
                                        }
                                    }
                                }

                            ////**************************
                            TempClient.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length,
                                SocketFlags.None, new AsyncCallback(ReceiveMessage), TempClient);

                            }
                        else
                            {
                            //SocketDict.Remove(FC.GetRemoteIP(TempClient) + "," 
                            //    + FC.GetRemotePort(TempClient).ToString());

                            TempDictKey = FC.GetRemoteIP(TempClient) + ","
                                + FC.GetRemotePort(TempClient).ToString();

                            if (SocketDict.Remove(TempDictKey) == false)
                                {
                                ErrorMessage = "Failed to remove the client key: " + TempDictKey;
                                //FC.AddRichText(ref TempRichTextBox, ErrorMessage);
                                }

                            //TempClient.Close();
                            }
                        }

                    //此处屏蔽和不屏蔽没有不同
                    //TempClient = null;

                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    //FC.AddRichText(ref TempRichTextBox, ex.Message);

                    //SocketDict.Remove(FC.GetRemoteIP(TempUseSocket) + ","
                    //+ FC.GetRemotePort(TempUseSocket).ToString());

                    if (TempClient != null)
                        {
                        if (TempClient.Connected == false)
                            {
                            TempDictKey = FC.GetRemoteIP(TempClient) + ","
                                + FC.GetRemotePort(TempClient).ToString();
                            if (SocketDict.Remove(TempDictKey) == false)
                                {
                                ErrorMessage = "Failed to remove the client key: " + TempDictKey;
                                FC.AddRichText(ref TempRichTextBox, ErrorMessage);
                                }

                            //SocketDict.Remove(FC.GetRemoteIP(TempClient) + ","
                            //    + FC.GetRemotePort(TempClient).ToString());
                            //TempClient.Close();
                            }
                        }

                    return;

                    //ServerSocket.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length,
                    //            SocketFlags.None, new AsyncCallback(ReceiveMessage), ServerSocket);
                    }
                }
            catch (Exception ex) 
                {
                ErrorMessage = ex.Message;
                //FC.AddRichText(ref TempRichTextBox, ErrorMessage);
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

                AutoTimer.Enabled = false;
                AutoTimer.Close();

                //AutoSendTimer.Dispose();

                SocketDict.Clear();

                ServerConnectClientThread = null;
                ClientIP = null;

                ServerSocket.Close();
                ServerSocket = null;

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
                        foreach (var xx in SocketDict) 
                            {
                            
                            }
                        for (int a = 0; a < AutoSendMsg.Length; a++)
                            {
                            SendMsg[a] = AutoSendMsg[a];
                            }
                        }
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                FC.AddRichText(ref TempRichTextBox, ErrorMessage);
                }
            }

        #endregion

        }//class

    }//namespace