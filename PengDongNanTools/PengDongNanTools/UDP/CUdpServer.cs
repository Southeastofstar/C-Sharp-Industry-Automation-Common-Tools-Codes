using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace PengDongNanTools
{
    /// <summary>
    /// UDP通讯服务端类
    /// </summary>
    public class CUdpServer// : UdpClient
    {
        /// <summary>
        /// UDP通讯服务端类的构造函数
        /// </summary>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        /// <param name="LocalPort">UDP通讯本地端口</param>
        /// <param name="UdpClientName">UDP通讯客户端DNS名称</param>
        /// <param name="UdpClientPort">UDP通讯客户端端口</param>
        public CUdpServer(string DLLPassword, int LocalPort, string UdpClientName, int UdpClientPort)
            //: base(LocalPort)
        {
            try
            {
                if (DLLPassword == "ThomasPeng" || DLLPassword == "彭东南")
                {
                }
                else
                {
                    for (; ; )
                    {
                    }
                }

                iLocalPort = LocalPort;
                sUdpClientName = UdpClientName;
                iUdpClientPort = UdpClientPort;

                tmrHeartBeatSignal.Enabled = false;
                tmrHeartBeatSignal.Interval = 1000;
                tmrHeartBeatSignal.Tick += tmrHeartBeatSignal_Tick;

                server = new UdpClient(LocalPort);

                server.BeginReceive(new AsyncCallback(ReceiveMsgCallBack), null);

                IPAddress ipAddress = null;
                //if (IPAddress.TryParse(UdpClientName, out ipAddress) == true)
                //{
                //    ServerEndPoint = new IPEndPoint(ipAddress, UdpClientPort);
                //}

                ipAddress = IPAddress.Parse(UdpClientName);
                ClientEndPoint = new IPEndPoint(ipAddress, UdpClientPort);
            }
            catch (Exception ex)
            {
                EnqueueErrorMsg("UDP通讯服务端类实例化时发生错误：" + ex.Message);
                MessageBox.Show("UDP通讯服务端类实例化时发生错误：\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #region "变量定义"

        /// <summary>
        /// 用于发送心跳信号
        /// </summary>
        Timer tmrHeartBeatSignal = new Timer();

        /// <summary>
        /// 心跳间隔时间(ms)
        /// </summary>
        public int HeartBeatInterval
        {
            get { return tmrHeartBeatSignal.Interval; }
            set { tmrHeartBeatSignal.Interval = value; }
        }

        /// <summary>
        /// 是否启用心跳信号
        /// </summary>
        public bool EnabledHeartBeat
        {
            get { return tmrHeartBeatSignal.Enabled; }
            set { tmrHeartBeatSignal.Enabled = value; }
        }

        /// <summary>
        /// 启用心跳信号时发送的内容
        /// </summary>
        string sHeartBeatMessage = "Hello from UDP server";

        /// <summary>
        /// 启用心跳信号时发送的内容
        /// </summary>
        public string HeartBeatMessage
        {
            get { return sHeartBeatMessage; }
            set
            {
                if (string.IsNullOrEmpty(value) == false)
                {
                    sHeartBeatMessage = value;
                }
            }
        }

        /// <summary>
        /// UDP通讯服务端
        /// </summary>
        UdpClient server = null;
        
        ///// <summary>
        ///// 是否使用提示对话框显示信息，默认 true
        ///// </summary>
        //public bool ShowMessageDialog = true;

        /// <summary>
        /// UDP通讯本地端口
        /// </summary>
        int iLocalPort = 0;

        /// <summary>
        /// UDP通讯客户端DNS名称
        /// </summary>
        string sUdpClientName = "";

        /// <summary>
        /// UDP通讯客户端端口
        /// </summary>
        int iUdpClientPort = 0;

        /// <summary>
        /// UDP通讯客户端终端：用于发送信息到客户端
        /// </summary>
        IPEndPoint ClientEndPoint = null;

        /// <summary>
        /// 接收结果队列
        /// </summary>
        Queue<string> ServerReceivedData = new Queue<string>();

        /// <summary>
        /// 接收结果：按照队列出列的方式，调用一次，出一个接收结果
        /// </summary>
        public string ReceivedData
        {
            get
            {
                try
                {
                    return ServerReceivedData.Dequeue();
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 通讯过程中的错误信息
        /// </summary>
        private Queue<string> sErrorMsg = new Queue<string>();

        /// <summary>
        /// 通讯过程中的错误信息
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                try
                {
                    return sErrorMsg.Dequeue();
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        #endregion

        #region "函数代码"

        void tmrHeartBeatSignal_Tick(object sender, EventArgs e)
        {
            try
            {
                if (null != server && null != ClientEndPoint)
                {
                    Send(sHeartBeatMessage);
                }
            }
            catch (Exception ex)
            {
                EnqueueErrorMsg("UDP通讯服务端发送心跳信号时发生错误：" + ex.Message);
            }
        }

        /// <summary>
        /// 发送信息到UDP客户端
        /// </summary>
        /// <param name="MsgToBeSent">要发送的信息</param>
        /// <returns>是否执行成功</returns>
        public bool Send(string MsgToBeSent)
        {
            int iLengthOfDataToBeSent = 0;
            int iLengthOfSentData = 0;

            try
            {
                if (null == ClientEndPoint)
                {
                    EnqueueErrorMsg("UDP通讯服务端实例化时传入的客户端参数不正确");
                    if (CConstant.ShowMessageDialog == true)
                    {
                        MessageBox.Show("UDP通讯服务端实例化时传入的客户端参数不正确", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return false;
                }

                if (string.IsNullOrEmpty(MsgToBeSent) == true)
                {
                    EnqueueErrorMsg("发送信息不能为空");
                    if (CConstant.ShowMessageDialog == true)
                    {
                        MessageBox.Show("发送信息不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return false;
                }

                byte[] data = Encoding.UTF8.GetBytes(MsgToBeSent);
                iLengthOfDataToBeSent = data.Length;
                
                //发送数据
                iLengthOfSentData = server.Send(data, data.Length, ClientEndPoint);
            }
            catch (Exception ex)
            {
                EnqueueErrorMsg("UDP通讯服务端发送信息时发生错误：" + ex.Message);
                if (CConstant.ShowMessageDialog == true)
                {
                    MessageBox.Show("UDP通讯服务端发送信息时发生错误：\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return false;
            }

            if (iLengthOfDataToBeSent == iLengthOfSentData)
            {
                return true;
            }
            else
            {
                EnqueueErrorMsg("UDP通讯服务端发送信息时长度不匹配，数据长度：" + iLengthOfDataToBeSent.ToString() + "， 已发送数据长度：" + iLengthOfSentData.ToString());
                if (CConstant.ShowMessageDialog == true)
                {
                    MessageBox.Show("UDP通讯服务端发送信息时长度不匹配，数据长度：" + iLengthOfDataToBeSent.ToString() + "， 已发送数据长度：" + iLengthOfSentData.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return false;
            }            
        }

        /// <summary>
        /// 错误信息进入队列
        /// </summary>
        /// <param name="MessageContent">错误信息</param>
        private void EnqueueErrorMsg(string MessageContent)
        {
            try
            {
                if (sErrorMsg.Count < int.MaxValue)
                {
                    sErrorMsg.Enqueue(MessageContent);
                }
                else
                {
                    sErrorMsg.Dequeue();
                    sErrorMsg.Enqueue(MessageContent);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 已收到信息进入队列
        /// </summary>
        /// <param name="MessageContent">已收到信息</param>
        private void EnqueueReceivedMsg(string MessageContent)
        {
            try
            {
                if (ServerReceivedData.Count < int.MaxValue)
                {
                    ServerReceivedData.Enqueue(MessageContent);
                }
                else
                {
                    ServerReceivedData.Dequeue();
                    ServerReceivedData.Enqueue(MessageContent);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 在接收信息时的远程主机
        /// </summary>
        IPEndPoint RemoteHostEndPointAtSend = new IPEndPoint(IPAddress.Any, 0);//port：0 以指定任何可用端口。 port 以主机顺序排列

        ///// <summary>
        ///// 是否添加UDP通讯对方的IP地址、端口号和时间到接收信息内容里面，默认：false
        ///// </summary>
        //public bool bAddRemoteHostInfoInReceivedData = false;

        /// <summary>
        /// 接收消息时对信息内容的处理方式
        /// </summary>
        public HandleMsgMode MsgHandleModeOnceReceived = HandleMsgMode.OriginalMsg;

        private void ReceiveMsgCallBack(IAsyncResult ar)
        {
            if (null != server)
            {
                try
                {
                    //接收数据
                    byte[] buffer = server.EndReceive(ar, ref RemoteHostEndPointAtSend);

                    string msg = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    //if (bAddRemoteHostInfoInReceivedData == true)
                    //{
                    //    EnqueueReceivedMsg(RemoteHostEndPointAtSend.Address.ToString() + "-" + RemoteHostEndPointAtSend.Port.ToString() + "--::" + msg);
                    //}
                    //else
                    //{
                    //    EnqueueReceivedMsg(msg);
                    //}
                    switch (MsgHandleModeOnceReceived)
                    {
                        default:
                        case HandleMsgMode.OriginalMsg:
                            EnqueueReceivedMsg(msg);

                            break;

                        case HandleMsgMode.MsgWithDateTime:
                            EnqueueReceivedMsg(DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0') + CConstant.SplitStringForTimeAndMsg + msg);

                            break;

                        case HandleMsgMode.MsgWithDateTimeRemoteInfo:
                            EnqueueReceivedMsg(DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0') + CConstant.SplitStringForTimeAndMsg + RemoteHostEndPointAtSend.Address.ToString() + CConstant.SplitStringForIPAddressAndPort + RemoteHostEndPointAtSend.Port.ToString() + CConstant.SplitStringForPortAndMsg + msg);

                            break;
                    }
                }
                catch (Exception)
                {
                }
            }

            //继续接收数据
            if (null != server)
            {
                try
                {
                    server.BeginReceive(new AsyncCallback(ReceiveMsgCallBack), null);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool Connect()
        {
            try
            {
                server.Connect(sUdpClientName, iUdpClientPort);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion

    }//class

}//namespace