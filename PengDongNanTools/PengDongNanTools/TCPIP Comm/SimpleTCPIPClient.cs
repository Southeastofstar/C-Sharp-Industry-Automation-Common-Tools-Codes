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

#endregion

//ok

namespace PengDongNanTools
{
    // 简化后的TCP/IP客户端
    /// <summary>
    /// 简化后的TCP/IP客户端【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
   public class SimpleTCPIPClient
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
               if (TCPClientStation != null)
                   {
                   if (TCPClientStation.Connected == true)
                       {
                       //获取的远程终端字符串格式为：127.0.0.1:12466
                       Socket TempSocket = TCPClientStation.Client;
                       EndPoint TempEndPoint = TempSocket.LocalEndPoint;

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

       ///// <summary>
       ///// 获取与服务器建立通讯后的客户端IP地址
       ///// </summary>
       //public string ClientIPAddress
       //    {
       //    get
       //        {
       //        if (TCPClientStation != null)
       //            {
       //            if (TCPClientStation.Connected == true)
       //                {
       //                //获取的远程终端字符串格式为：127.0.0.1:12466
       //                Socket TempSocket = TCPClientStation.Client;
       //                EndPoint TempEndPoint = TempSocket.LocalEndPoint;

       //                //方法一：
       //                return TempEndPoint.ToString().Split(':')[0];

       //                ////方法二：
       //                //string TempIPAddress = TempEndPoint.ToString();
       //                //string[] TempResult=TempIPAddress.Split(':');
       //                //return TempResult[0];
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

       /// <summary>
       /// 目标服务器的IP地址
       /// </summary>
       public string ServerIPAddress
           {
           get { return TargetServerIPAddress.ToString(); }
           }

       /// <summary>
       /// 目标服务器的监听端口
       /// </summary>
       public UInt16 ServerListenPort
           {
           get { return TargetServerPort; }
           }

       /// <summary>
       /// 更新信息至文本框时是否显示日期和时间，默认为True
       /// </summary>
       public bool ShowDateTimeForMessage = true;

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

       private Endings CurrentEndingSetting = Endings.CRLF;

       /// <summary>
       /// 自定义的结束符[用于发送时在发送的字符串最后加上]
       /// </summary>
       public string EndingCustomerizeSetting;

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

       private Button ButtonUseForInvoke = new Button();

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

        private bool NotPauseFlag = true;

        private System.Threading.Thread ClientConnectToServerThread;

        private TcpClient TCPClientStation;
        private NetworkStream TCPClientStream;

        /// <summary>
        /// 服务器以太网端口号
        /// </summary>
        private UInt16 TargetServerPort = 8000;

        /// <summary>
        /// 服务器以太网IP地址
        /// </summary>
        private IPAddress TargetServerIPAddress;

        ///// <summary>
        ///// 用于接受服务器发来的字节
        ///// </summary>
        //private byte[] ClientReadDataBytesFromServer;

        ///// <summary>
        ///// 字符串用于存储从服务器发来的ASCII码.
        ///// </summary>
        //private string ReceivedDataFromServer;

        /// <summary>
        /// 以太网客户端接收缓冲区大小
        /// </summary>
        public Int32 ReceivedBufferSize = 1024;

        /// <summary>
        /// 以太网客户端发送缓冲区大小
        /// </summary>
        public Int32 SendBufferSize = 1024;

        /// <summary>
        /// 以太网通讯程序运行过程中的错误信息提示
        /// </summary>
        public string ErrorMessage;

        /// <summary>
        /// 发送字符串的后缀【结束符】
        /// </summary>
        public string Suffix = "\r\n";

        /// <summary>
        /// 是否发送/接收GB2312编码
        /// </summary>
        public bool GB2312Coding = false;

        //private string TempErrorMessage;// = "";

        /// <summary>
        /// 是否以16进制发送信息
        /// </summary>
        public bool SendMessageInHEX = false;

        /// <summary>
        /// 客户端从服务器收到的字符串内容
        /// </summary>
        public string ReceivedString = "";

        /// <summary>
        /// 用于记录实例化时输入密码是否正确
        /// </summary>
        private bool PasswordIsCorrect;//=false;

        /// <summary>
        /// 成功建立新的实例
        /// </summary>
        private bool SuccessBuiltNew = false;

        //************读取属性的方式***************
        /// <summary>
        /// 以太网客户端口是否成功连接到服务器端口的标志
        /// </summary>
        /// <returns></returns>
        public bool Connected()
        {
            if (!(TCPClientStation == null))
            {
                return TCPClientStation.Connected;
            }
            else
                return false;
        }

        #endregion

        #region "函数代码"

        // 创建TCP/IP通讯的客户端实例
        /// <summary>
        /// 创建TCP/IP通讯的客户端实例
        /// </summary>
        /// <param name="IPAddressOfTargetServer">服务器IP地址</param>
        /// <param name="PortOfTargetServer">服务器监听端口</param>
        /// <param name="Password">使用此DLL的密码</param>
        public SimpleTCPIPClient(string IPAddressOfTargetServer, ushort PortOfTargetServer, 
            string Password)
        {

            try
            {
                SuccessBuiltNew = false;
                PasswordIsCorrect = false;

                if (IPAddressOfTargetServer == "")
                {
                    MessageBox.Show("The IP address of TCP/IP Server can't be set as empty, please revise it and retry.\r\n服务器的IP地址不能为空，请修改.","",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Password == "ThomasPeng" | Password == "pengdongnan" | Password == "彭东南")
                {
                    PasswordIsCorrect = true;

                    //IPAddress TempIPAddress = null;
                    string[] GetCorrectIPAddress = new string[4];
                    UInt16[] TempGetIPAddress = new UInt16[4];

                    //判断输入的服务器IP地址是否正确
                    if (!IPAddress.TryParse(IPAddressOfTargetServer, out TargetServerIPAddress))
                    {
                        MessageBox.Show("The format of IP address for the TCP/IP Server is not correct, please check and correct it.\r\n服务器IP地址格式错误，请检查后输入正确IP地址再重新建立新实例.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        //此处解析IP地址不对："192.168.000.024"解析为"192.168.000.20"
                        GetCorrectIPAddress = Strings.Split(IPAddressOfTargetServer, ".");
                        for (Int16 a = 0; a < 4; a++)
                        {
                            TempGetIPAddress[a] = Convert.ToUInt16(GetCorrectIPAddress[a]);
                            if (TempGetIPAddress[a] > 254 | TempGetIPAddress[a] < 0)
                            {
                                string TempMsg = "";
                                TempMsg = "The IP address of server: " + TempGetIPAddress[a] + " is over the range, the correct range for IP address is between 0~255, please correct it.\r\n服务器IP地址: " + TempGetIPAddress[a] + " 超出有效范围【0~255】，请输入正确IP地址.";
                                MessageBox.Show(TempMsg, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        //设置IP地址时如果某节点不是全部为零0值，则必须去掉此节点的前缀0，否则会报错
                        string Str = TempGetIPAddress[0].ToString() + "." + TempGetIPAddress[1].ToString() + "." + TempGetIPAddress[2].ToString() + "." + TempGetIPAddress[3].ToString();
                        TargetServerIPAddress = IPAddress.Parse(Str);
                    }

                    //以太网的有效端口从0~65535，刚好是UShort的值范围，所以不需要进行验证有效性
                    TargetServerPort = PortOfTargetServer;

                    SuccessBuiltNew = true;

                }
                else 
                {
                    PasswordIsCorrect = false;
                    MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n 你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;                
                }
                
            //命名空间:           System.Net
            //程序集：   System在 System.dll 中
            //语法
            //Public Shared Sub SetTcpKeepAlive(ByVal enabled As Boolean, ByVal keepAliveTime As Integer, ByVal keepAliveInterval As Integer)
            //参数
            //enabled
            //类型:     System.Boolean
            //如果设置为 true，则将使用指定的 keepAliveTime 和 keepAliveInterval 值启用 TCP 连接上的 TCP keep-alive 选项。 
            //如果设置为 false，则将禁用 TCP keep-alive 选项，并忽略剩余参数。
            //默认值为 false。
            //keepAliveTime
            //类型:     System.Int32
            //指定发送第一个 keep-alive 数据包之前没有活动的超时时间（以毫秒为单位）。
            //该值必须大于 0。如果传递的值小于或等于零，则会引发 ArgumentOutOfRangeException。 
            //keepAliveInterval
            //类型:     System.Int32
            //指定当未收到确认消息时发送连续 keep-alive 数据包之间的间隔（以毫秒为单位）。
            //该值必须大于 0。如果传递的值小于或等于零，则会引发 ArgumentOutOfRangeException。 
            //'System.Net.ServicePointManager.SetTcpKeepAlive(True, 1000, 1000)

            }

            catch (Exception ex)
            {
                SuccessBuiltNew = false;
                MessageBox.Show("创建以太网客户端类的实例时出现错误！\r\n" + ex.Message);
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

        //发送字符串到服务器
        /// <summary>
        /// 发送字符串到服务器
        /// </summary>
        /// <param name="StringsToBeSentToServer"></param>
        /// <returns></returns>
        public bool Send(string StringsToBeSentToServer)
            {

            if (PasswordIsCorrect == false)
                {
                MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            string TempString = StringsToBeSentToServer;
            StringsToBeSentToServer = "";

            try
                {
                //如果字符串为空，则退出函数
                if (TempString == "")
                    return false;

                if (SuccessBuiltNew == true)
                    {
                    if (!(TCPClientStation == null))
                        {
                        if (!(TCPClientStream == null))
                            {
                            if (SendMessageInHEX == true)
                                {
                                TempString = StringConvertToHEX(TempString + Suffix);
                                TCPClientStream.Write(System.Text.Encoding.UTF8.GetBytes(TempString), 0, 
                                    System.Text.Encoding.UTF8.GetBytes(TempString).Length);
                                ErrorMessage = "Client Sent: " + TempString + Suffix;
                                }
                            else
                                {
                                if (GB2312Coding == true)
                                    {
                                    TCPClientStream.Write(System.Text.Encoding.GetEncoding(936).GetBytes(TempString + Suffix), 0,
                                        System.Text.Encoding.GetEncoding(936).GetBytes(TempString + Suffix).Length);
                                    }
                                else 
                                    {
                                    TCPClientStream.Write(System.Text.Encoding.UTF8.GetBytes(TempString + Suffix), 0,
                                        System.Text.Encoding.UTF8.GetBytes(TempString + Suffix).Length);
                                    }

                                ErrorMessage = "Client Sent: " + TempString + Suffix;
                                }
                            }
                        }
                    else
                        return false;
                    }

                else
                    return false;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }

            return true;

            }

        //关闭以太网客户端并释放相关资源
        /// <summary>
        /// 关闭以太网客户端并释放相关资源
        /// </summary>
        public void Close()
            {

            if (PasswordIsCorrect == false)
                {
                MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
                }

            try
                {
                //释放客户端以太网线程
                if (!(ClientConnectToServerThread == null))
                    {
                    ClientConnectToServerThread.Abort();
                    ClientConnectToServerThread = null;
                    }

                //释客户端的相关资源
                if (!(TCPClientStream == null))
                    {
                    TCPClientStream.Close();
                    TCPClientStream = null;
                    }

                if (!(TCPClientStation == null))
                    {
                    TCPClientStation.Close();
                    TCPClientStation = null;
                    }

                //强制对所有代进行垃圾回收
                GC.Collect();

                }
            catch (Exception)
                {
                }

            }

        //启动以太网客户端与服务器进行通讯
        /// <summary>
        /// 启动以太网客户端与服务器进行通讯
        /// </summary>
        public void Start()
            {

            if (PasswordIsCorrect == false)
                {
                MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
                }

            try
                {
                if (SuccessBuiltNew == true)
                    {
                    if (ClientConnectToServerThread == null)
                        {
                        ClientConnectToServerThread = new System.Threading.Thread(ClientReadMessageFromServer);
                        ClientConnectToServerThread.IsBackground = true;
                        ClientConnectToServerThread.Start();
                        ErrorMessage = "启动以太网客户端...";
                        }
                    else
                        {
                        ErrorMessage = "已经启动了以太网客户端，不需要重复开启...";
                        }
                    }
                else 
                    {
                    ErrorMessage = "未成功创建以太网客户端类的实例！";
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }
            
            }

        //从以太网服务器读取内容
       /// <summary>
        /// 从以太网服务器读取内容
       /// </summary>
        private void ClientReadMessageFromServer()
            {
            string TempRecord = "";
            byte[] ClientReadDataBytesFromServer = new byte[1024];
            int bytes = 0;
            //tcpClient=new TcpClient();
           
            while (true)
                {
                if (NotPauseFlag)
                    {
                    try
                        {
                        TCPClientStation = new TcpClient();
                        ErrorMessage = "TCP/IP客户端等待连接服务器中...";
                        TCPClientStation.Connect(TargetServerIPAddress.ToString(), TargetServerPort);
                        ErrorMessage = "TCP/IP客户端已成功连接到服务器...";
                        TCPClientStream = TCPClientStation.GetStream();

                        while (TCPClientStation.Connected==true)
                            {
                            bytes = TCPClientStream.Read(ClientReadDataBytesFromServer, 0, ClientReadDataBytesFromServer.Length);
                            if (bytes == 0)
                                {
                                TCPClientStream.Write(System.Text.Encoding.UTF8.GetBytes("Client"), 
                                    0, System.Text.Encoding.UTF8.GetBytes("Client").Length);
                                }
                            else
                                {
                                //TempRecord = Encoding.ASCII.GetString(ClientReadDataBytesFromServer, 0, bytes);
                                TempRecord = System.Text.Encoding.UTF8.GetString(ClientReadDataBytesFromServer, 0, bytes);
                                ReceivedString = TempRecord;
                                //ErrorMessage = "Client got: " + TempRecord;
                                ErrorMessage = "Client port " + FC.GetLocalPort(TCPClientStation) + " from server IP: "
                                    + FC.GetRemoteIP(TCPClientStation) + ", port: " + FC.GetRemotePort(TCPClientStation) + ", got: " + TempRecord;
                                }
                            }

                        //while (true)
                        //    {
                        //    bytes = TCPClientStream.Read(ClientReadDataBytesFromServer, 0, ClientReadDataBytesFromServer.Length);
                        //    if (bytes == 0)
                        //        {
                        //        TCPClientStation.Close();
                        //        break;
                        //        }
                        //    else
                        //        {
                        //        //TempRecord = Encoding.ASCII.GetString(ClientReadDataBytesFromServer, 0, bytes);
                        //        TempRecord = System.Text.Encoding.UTF8.GetString(ClientReadDataBytesFromServer, 0, bytes);
                        //        ReceivedString = TempRecord;
                        //        ErrorMessage = "Client got: " + TempRecord;
                        //        }
                        //    //System.Threading.Thread.Sleep(1);
                        //    }
                        
                        //**********************************
                        //if (!(TCPClientStation == null))
                        //    {
                        //    if(TCPClientStation.Connected==true)
                        //        {
                        //        TCPClientStream = TCPClientStation.GetStream();

                        //        //存储从服务器读取的字节
                        //        ClientReadDataBytesFromServer = new byte[TCPClientStation.ReceiveBufferSize];

                        //        //if (TCPClientStation.Connected == false)
                        //        //    {
                        //        //    TCPClientStream.Close();
                        //        //    TCPClientStation.Close();
                        //        //    //continue;  //***用来替代exit try，要看效果是不是这样***
                        //        //    break;
                        //        //    }
                        //        //else 
                        //        if (TCPClientStation.Connected == true)
                        //            {
                        //            Int32 bytes = TCPClientStream.Read(ClientReadDataBytesFromServer, 0, ClientReadDataBytesFromServer.Length);
                        //            if (bytes > 0)
                        //                {
                        //                TempRecord = System.Text.Encoding.UTF8.GetString(ClientReadDataBytesFromServer, 0, bytes);
                        //                ReceivedDataFromServer = TempRecord;
                        //                ReceivedString = TempRecord;
                        //                ErrorMessage = "Client got: " + ReceivedDataFromServer;
                        //                }
                        //            //else 
                        //            //    {
                        //            //    TCPClientStream.Write(System.Text.Encoding.UTF8.GetBytes("Client\r\n"), 0, System.Text.Encoding.UTF8.GetBytes("Client\r\n").Length);
                        //            //    }
                        //            }
                        //        }


                        //    }
                        //else
                        //    {
                        //    TCPClientStation = new TcpClient(TargetServerIPAddress.ToString(), TargetServerPort);
                        //    TCPClientStation.NoDelay = true;
                        //    TCPClientStation.ReceiveBufferSize = ReceivedBufferSize;
                        //    TCPClientStation.SendBufferSize = SendBufferSize;
                        //    if (TCPClientStation.Connected == true)
                        //        {
                        //        ErrorMessage = "TCP/IP客户端已成功连接到服务器...";
                        //        }
                        //    }
                        }

                    //catch (InvalidOperationException expp)
                    //    {
                    //    ErrorMessage = "Client ErrorCode: " + expp.Message;
                    //    if (!(TCPClientStation == null))
                    //        {
                    //        if (!(TCPClientStream == null))
                    //            {
                    //            TCPClientStream.Close();
                    //            TCPClientStream = null;
                    //            }
                    //        }
                    //    else
                    //        TCPClientStation = null;
                    //    }

                    //catch (SocketException exp) 
                    //    {
                    //    ErrorMessage = "Client ErrorCode: " + exp.NativeErrorCode + ", " + exp.Message;
                    //    if (!(TCPClientStation == null))
                    //        {
                    //        if (!(TCPClientStream == null))
                    //            {
                    //            TCPClientStream.Close();
                    //            TCPClientStream = null;
                    //            }
                    //        }
                    //    else
                    //        TCPClientStation = null;
                    //    }

                        //**********************************
                    catch (Exception ex)
                        {
                        ErrorMessage = "Client Error: " + ex.Message;
                        //TCPClientStation.Close();
                        if (!(TCPClientStation == null))
                            {
                            TCPClientStation.Close();
                            if (!(TCPClientStream == null))
                                {
                                TCPClientStation.Close();
                                TCPClientStream.Close();
                                TCPClientStream = null;
                                }
                            }
                        else
                            TCPClientStation = null;
                        }

                    }


                }


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

}  //namespace