#region "using"

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Threading;

#endregion

namespace PengDongNanTools
    {

    #region "已处理事项"

    //1、可以设置一个动态数组，将设置的串口参数返回主窗口；【ok】
    //2、目前接收只能以回车+换行为结束符【readline方法】，后续添加代码改为可以用其它方式来定义结束符；需要用读取byte来进行判断【OK】
    //3、在 “串口”的 .Encoding = System.Text.Encoding.GetEncoding(936)，就可以收发中文；【OK】
    //                         或者System.Text.Encoding.UTF8
    //                         或者System.Text.Encoding.GetEncoding("GB2312")
    //4、需要设置自动发送的功能，以及添加timer用来发送内容；【OK】
    //5、添加发送文件的功能；当发送大数据时，会有部分内容对方收不到，应该是超过缓冲区的大小，故要分成多次进行发送；【OK】

    //7、A、可以考虑有多少个RS232C串口端口就可以进行多少个RS232C串口连接，用动态数组来控制各个RS232C串口的通讯状况，用线程来扫描收到的信息【OK】
    //   B、点击ListView对应的行，首先判断是否已经在进行通信，没有就可以点击“打开”按钮并建立对应的线程；
    //      点击ListView对应的行，首先判断是否已经在进行通信，进行中则可以点击“关闭”来结束相应的线程；
    //   C、需要将传递进来的串口进行参数对应, 比如有的是发送二进制字节, 而有的是发送字符
    //   D、需要将传递进来的串口进行参数对应, 比如有的是接收二进制字节, 而有的是接收字符
    //      事件数组【OK】
    //8、当焦点离开ComboBoxCOMPort后，SelectedIndex会变为-1，会导致后面的ComboBox代码出错，需要想办法解决此问题，可以用一个变量来做记忆；【OK】
    //9、处理变更串口时，其它参数跟随变更，程序不对；【OK】

    #endregion

    #region "待处理事项"

    //6、在发送超过缓冲区大小的数据或者文件时，如果是中文，则在接收端会出现一个中文乱码的情况，因为中文是两个字节表示，待后续考虑如果优化；【】
    //10、运行中如果有接线错误出现"硬件检测到一个组帧错误"，就会出现软件卡死的情况，
    //    原本想在错误接收事件里面设置错误标志，在接收事件里判断有错误就不接收，但是那不知道在哪里清除这个错误标志，就无法实现
    
    #endregion

    //RS232CForm类【软件作者：彭东南, southeastofstar@163.com】
    /// <summary>
    /// RS232CForm类【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    public partial class RS232CForm : Form
        {

#region "代理变量定义"

        //利用委托和代理进行跨线程安全操作控件，以此避免跨线程操作异常
        //*****************************
        private delegate void AddTextDelegate(string TargetText);
 
#endregion

#region "变量定义"

        CommonFunction FC = new CommonFunction("彭东南");

        bool[] TempGB2312Flag;

        /// <summary>
        /// 保存返回的参数
        /// </summary>
        public Parameters[] SavedParaForAvailablePorts;
     
        /// <summary>
        /// 待处理事项，后续再添加代码
        /// </summary>
        public string PendingIssue
            {
            get { return ""; }
            }

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
        
        //定义类用于操作计算机相关信息，类似于VB .NET中的my.computer
        private Microsoft.VisualBasic.Devices.Computer pcFileSystem = new Microsoft.VisualBasic.Devices.Computer();
          
        private System.Timers.Timer tmrAutoSend = new System.Timers.Timer();

        /// <summary>
        /// 串口通讯程序运行过程中的错误信息提示
        /// </summary>
        public string ErrorMessage;

        string TempErrorMessage = "";

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
        /// 用于记录实例化时输入密码是否正确
        /// </summary>
        private bool PasswordIsCorrect;//=false;

        /// <summary>
        /// 成功建立新的实例
        /// </summary>
        private bool SuccessBuiltNew = false;

        private int AvailableRS232CPorts = 0;
        private string[] AvailablePortNames;

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
        /// 更新信息至文本框时是否显示日期和时间，默认为True
        /// </summary>
        public bool ShowDateTimeForMessage = true;

        //private Int16 TempCOMPortIndex=0;
        private bool[] TempAutoSendFlag, TempSendFileFlag, TempSendHEXFlag,
            TempReadLine, TempCustomSuffixFlag, TempErrorFlag;
        private Int32[] TempAutoSendInterval;
        private string[] TempSendText, TempCustomSuffix, ReceivedString;
        private Endings[] TempEnding;

        /// <summary>
        /// 串口数组
        /// </summary>
        private System.IO.Ports.SerialPort[] RS232CArray;
        
        private OpenFileDialog[] newOpenFile=null;
        private frmAbout AboutDialog = new frmAbout();
        private System.Windows.Forms.SaveFileDialog SaveLogAsTXTFile = new SaveFileDialog();

        /// <summary>
        /// 在关闭窗体时是否只是隐藏窗体【默认True】
        /// </summary>
        public bool JustHideFormAtClosing = true;

        /// <summary>
        /// 在关闭窗体时是否提示【默认True】
        /// </summary>
        public bool ShowPromptAtClosing = true;

        /// <summary>
        /// RS232C通讯参数数据结构
        /// </summary>
        public struct Parameters
            {
            /// <summary>
            /// 波特率设置
            /// </summary>
            public int BaudRateSetting;

            /// <summary>
            /// 数据位设置
            /// </summary>
            public int DataBitsSetting;

            /// <summary>
            /// 数据终端就绪设置
            /// </summary>
            public bool DTREnableSetting;

            /// <summary>
            /// 握手协议设置
            /// </summary>
            public System.IO.Ports.Handshake HandShakeSetting;

            /// <summary>
            /// 奇偶校验设置
            /// </summary>
            public System.IO.Ports.Parity ParitySetting;

            /// <summary>
            /// 端口名称设置
            /// </summary>
            public string PortNameSetting;

            /// <summary>
            /// 读取缓冲区大小设置
            /// </summary>
            public int ReadBufferSizeSetting;

            /// <summary>
            /// DataReceived 事件发生前内部输入缓冲区中的字节数
            /// </summary>
            public int ReceivedBytesThresholdSetting;

            /// <summary>
            /// 请求发送设置
            /// </summary>
            public bool RTSEnableSetting;

            /// <summary>
            /// 停止位设置
            /// </summary>
            public System.IO.Ports.StopBits StopBitsSetting;

            /// <summary>
            /// 发送缓冲区大小设置
            /// </summary>
            public int WriteBufferSizeSetting;

            /// <summary>
            /// 发送的结束符设置
            /// </summary>
            public Endings Suffix;

            /// <summary>
            /// 发送的编码设置
            /// </summary>
            public System.Text.Encoding EncodingSetting;            
            }

        /// <summary>
        /// 界面的显示语言
        /// </summary>
        public bool LanguageInChinese = true;
                
#endregion
           
#region "窗体事件代码"

        private void chkGB2312_CheckedChanged(object sender, EventArgs e)
            {

            if (chkGB2312.Checked == true)
                {
                RS232CArray[ComboBoxCOMPort.SelectedIndex].Encoding = System.Text.Encoding.GetEncoding(936);
                TempGB2312Flag[ComboBoxCOMPort.SelectedIndex] = true;
                //RS232CArray[ComboBoxCOMPort.SelectedIndex].Encoding = System.Text.Encoding.GetEncoding("GB2312");
                }
            else
                {
                RS232CArray[ComboBoxCOMPort.SelectedIndex].Encoding = System.Text.Encoding.UTF8;
                TempGB2312Flag[ComboBoxCOMPort.SelectedIndex] = false;
                }

            }

        private void TmrAutoSend_Elasped(object sender, System.Timers.ElapsedEventArgs e)
            {

            try
                {

                for (int j = 0; j < AvailableRS232CPorts; j++)
                    {
                    //如果某个串口为null,则跳过此端口进入下个端口
                    if (RS232CArray[j] == null)
                        {
                        continue;
                        }

                    if (RS232CArray[j].IsOpen == true)
                        {

                        if (TempAutoSendFlag[j] == true)
                            {

                            if (TempSendFileFlag[j] == true)
                                {
                                //Send file
                                //int TempCount = 0;
                                byte[] Data;
                                string TempStr = "";
                                bool TempLeft = false;//判断是否有尾数没有发送完成，因为不可能刚刚好被整除
                                double TotalSend = 0.0;
                                int TempQty = 0;

                                if (newOpenFile[j].FileNames.Length > 0)
                                    {
                                    for (int a = 0; a < newOpenFile[j].FileNames.Length; a++)
                                        {

                                        try
                                            {

                                            Data = pcFileSystem.FileSystem.ReadAllBytes(newOpenFile[j].FileNames[a]);
                                            AddText("File: " + newOpenFile[j].FileNames[a] + ". total: " + Data.Length + " bytes...");
                                            TempStr = System.Text.Encoding.UTF8.GetString(Data);

                                            TotalSend = Data.Length / RS232CArray[j].WriteBufferSize;
                                            TempQty = (int)TotalSend;
                                            TempLeft = false;

                                            if (TempQty > 1)
                                                {

                                                for (int b = 0; b < TempQty; b++)
                                                    {
                                                    if (b == 0)
                                                        {
                                                        RS232CArray[j].Write(Data, 0, RS232CArray[j].WriteBufferSize);
                                                        }
                                                    else
                                                        {
                                                        RS232CArray[j].Write(Data, b * RS232CArray[j].WriteBufferSize - 1,
                                                                             RS232CArray[j].WriteBufferSize);
                                                        }
                                                    AddText(RS232CArray[j].PortName + ": " + (b + 1) + ", sent " +
                                                            RS232CArray[j].WriteBufferSize + "bytes...");

                                                    while (RS232CArray[j].WriteTimeout > 0)
                                                        {
                                                        System.Windows.Forms.Application.DoEvents();
                                                        }

                                                    Thread.Sleep(5);

                                                    if ((b + 1) * RS232CArray[j].WriteBufferSize < Data.Length)
                                                        {
                                                        TempLeft = true;
                                                        }

                                                    }

                                                //如果有尾数，则将尾数部分发送出去
                                                if (TempLeft == true)
                                                    {
                                                    RS232CArray[j].Write(Data, TempQty * RS232CArray[j].WriteBufferSize - 1,
                                                        Data.Length - TempQty * RS232CArray[j].WriteBufferSize + 1);
                                                    AddText(RS232CArray[j].PortName + ": " + (TempQty + 1) + ", sent " +
                                                            (Data.Length - TempQty * RS232CArray[j].WriteBufferSize) + "bytes...");
                                                    Thread.Sleep(5);
                                                    TempLeft = false;
                                                    }

                                                }
                                            else
                                                {
                                                //发送的字节小于等于发送缓冲区的大小
                                                RS232CArray[j].Write(Data, 0, Data.Length);
                                                AddText(RS232CArray[j].PortName + ": " + "Sent " + Data.Length + "bytes...");
                                                }

                                            }
                                        catch (Exception ex)
                                            {
                                            AddText(RS232CArray[j].PortName + " Error:\r\n " + ex.Message);
                                            }

                                        }
                                    }
                                }
                            else
                                {
                                //Not send file
                                if (TempSendText[j] != "")
                                    {
                                    string TempString = TempSendText[j];
                                    if (TempSendHEXFlag[j] == true)
                                        {
                                        TempString = StringConvertToHEX(TempString + TempCustomSuffix[j]);
                                        RS232CArray[j].Write(TempString);
                                        AddText(RS232CArray[j].PortName + " sent: " + TempString);
                                        }
                                    else
                                        {
                                        RS232CArray[j].Write(TempString + TempCustomSuffix[j]);
                                        AddText(RS232CArray[j].PortName + " sent: " + TempString + TempCustomSuffix[j]);
                                        }
                                    }
                                }

                            }

                        }
                    else
                        {

                        }

                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void CheckboxRTSEnable_CheckedChanged(object sender, EventArgs e)
        {
        
                try
                {

                    if(CheckboxRTSEnable.Checked==true)
                        {
                        RS232CArray[ComboBoxCOMPort.SelectedIndex].RtsEnable=true;
                        }
                    else if(CheckboxRTSEnable.Checked==false)
                        {
                        RS232CArray[ComboBoxCOMPort.SelectedIndex].RtsEnable=false;
                        }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

        }

        private void CheckboxDTREnable_CheckedChanged(object sender, EventArgs e)
        {
        
                try
                {

                    if(CheckboxDTREnable.Checked ==true)
                        {
                        RS232CArray[ComboBoxCOMPort.SelectedIndex].DtrEnable =true;
                        }
                    else if(CheckboxDTREnable.Checked ==false)
                        {
                        RS232CArray[ComboBoxCOMPort.SelectedIndex].DtrEnable =false;
                        }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

        }

        private void chkReadLineOrByte_CheckedChanged(object sender, EventArgs e)
        {
        
                try
                {

                    if(chkReadLineOrByte.Checked==true)
                        {
                        TempReadLine[ComboBoxCOMPort.SelectedIndex] = false;
                        }
                    else
                        {
                        TempReadLine[ComboBoxCOMPort.SelectedIndex] = true;
                        }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

        }
        
        private System.IO.Ports.SerialPort TempSerialPort = new System.IO.Ports.SerialPort();
        //串口错误事件的函数，在窗体load时添加事件绑定
        /// <summary>
        /// 串口错误事件的函数，在窗体load时添加事件绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RS232CPort_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
            {

            int TempPortNumber;

            try
                {
                //将事件传递到临时串口
                TempSerialPort = (System.IO.Ports.SerialPort)sender;

                //从传递的串口端口名称查出在串口数组中的序
                TempPortNumber = ComboBoxCOMPort.FindStringExact(TempSerialPort.PortName);

                TempErrorFlag[TempPortNumber]=true;

                switch (e.EventType)
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

                AddText(ErrorMessage);

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                }

            }

        //串口接收事件的函数，在窗体load时添加事件绑定
        /// <summary>
        /// 串口接收事件的函数，在窗体load时添加事件绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RS232CPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
            {

            byte[] ReadByte;
            string TempStr = "";
            int TempPortNumber;

            try
                {
                //将事件传递到临时串口
                TempSerialPort = (System.IO.Ports.SerialPort)sender;

                //从传递的串口端口名称查出在串口数组中的序
                TempPortNumber = ComboBoxCOMPort.FindStringExact(TempSerialPort.PortName);

                if (TempReadLine[TempPortNumber] == true)
                    {
                    TempSerialPort.NewLine = RS232CArray[TempPortNumber].NewLine;
                    ReceivedString[TempPortNumber] = TempSerialPort.ReadLine();
                    ErrorMessage = TempSerialPort.PortName + " received: " + ReceivedString[TempPortNumber];
                    if (richtxtHistory != null)
                        {
                        AddText(TempSerialPort.PortName + " received: " + ReceivedString[TempPortNumber]);
                        }

                    FC.ClearRichTextBoxContents(ref rtbCurrentReceived);
                    FC.AddRichTextWithoutTimeMark(ref rtbCurrentReceived, ReceivedString[TempPortNumber]);

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

                    if (TempSerialPort.BytesToRead > 0)
                        {
                        ReadByte = new byte[TempSerialPort.BytesToRead];
                        TempStr = "";
                        TempSerialPort.Read(ReadByte, 0, TempSerialPort.BytesToRead);

                        if (TempSendHEXFlag[TempPortNumber] == true)
                            {

                            for (int i = Information.LBound(ReadByte); i < Information.UBound(ReadByte); i++)
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

                            }
                        else
                            {
                            TempStr = System.Text.Encoding.UTF8.GetString(ReadByte);
                            }                        

                        ReceivedString[TempPortNumber] = TempStr;
                        ErrorMessage = TempSerialPort.PortName + " received: " + ReceivedString[TempPortNumber];

                        if (richtxtHistory != null)
                            {
                            AddText(TempSerialPort.PortName + " received: " + ReceivedString[TempPortNumber]);
                            }
                        
                        FC.ClearRichTextBoxContents(ref rtbCurrentReceived);
                        FC.AddRichTextWithoutTimeMark(ref rtbCurrentReceived, ReceivedString[TempPortNumber]);

                        }

                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                ErrorMessage = ex.Message;
                }

            }          

        private void RS232CForm_FormClosing(object sender, FormClosingEventArgs e)
            {
            
                try
                {
                
                if(JustHideFormAtClosing==true)
                    {
                    e.Cancel=true;
                    this.Hide();
                    return;
                    }

                if (ShowPromptAtClosing == true)
                    {
                    if (MessageBox.Show("Are you sure to exit?\r\n" +
                        "确定要退出吗？", "Info", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)
                        {

                        //if(SaveLogAsTXTFile!=null)
                        //    {
                        //    SaveLogAsTXTFile.Dispose();
                        //    }

                        // if(AboutDialog!=null)
                        //    {
                        //    AboutDialog.Dispose();
                        //    }

                        //for(int a=0;a<RS232CArray.Length;a++)
                        //    {
                        //    if(RS232CArray[a]!=null)
                        //        {
                        //        if(RS232CArray[a].IsOpen==true)
                        //            {
                        //            RS232CArray[a].Close();
                        //            }
                        //        RS232CArray[a].Dispose();
                        //        RS232CArray[a]=null;
                        //        }

                        //    if(newOpenFile[a]!=null)
                        //        {
                        //        newOpenFile[a].Dispose();
                        //        newOpenFile[a]=null;
                        //        }

                        //    }

                        //tmrAutoSend.Stop();
                        //tmrAutoSend.Dispose();

                        //GC.Collect();

                        FreeAllResources();
                        e.Cancel = false;

                        }
                    else
                        {
                        e.Cancel = true;
                        }
                    }
                else 
                    {
                    FreeAllResources();
                    e.Cancel = false;
                    }

                }
            catch (Exception)// ex)
                {
                //AddText(ex.Message);
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            
            }

        private void RS232CForm_Load(object sender, EventArgs e)
            {

            //AlreadySetSerialPortParametersForRS232C = false;   //是否已经为主窗口的串口通讯设置参数

            LanguageInChinese = true;
            LanguageChange();

            txtAutoSendInterval.Text="500";
            tmrAutoSend.Start();

            //if (AvailableRS232CPorts > 0)
            //    {
            //    AddText("计算机有：" + AvailableRS232CPorts + " 个串口.");
            //    //将可用串口名字输出到文本控件
            //    string TempStr = "";
            //    for (int a = 0; a < AvailablePortNames.Length; a++)
            //        {
            //        TempStr += AvailablePortNames[a] + "  ";
            //        }
            //    AddText("计算机可用串口：" + TempStr);

            //    //将可用串口名字添加到ComboBox控件
            //    ComboBoxCOMPort.Items.Clear();
            //    ComboBoxCOMPort.Items.AddRange(AvailablePortNames);
            //    ComboBoxCOMPort.SelectedIndex = 0;
            //    }
            //else 
            //    {
            //    ComboBoxCOMPort.Items.Clear();
            //    AddText("此计算机无可用RS232C串口.");
            //    }

            }

        private void ComboBoxCOMPort_SelectedIndexChanged(object sender, EventArgs e)
            {

            try 
                {
                
                if(ComboBoxCOMPort.SelectedIndex != -1)
                    {
                    
                    if(AvailablePortCount > 0)
                        {
                        
                        //************************************
                        if (TempGB2312Flag[ComboBoxCOMPort.SelectedIndex] == true)
                            {
                            chkGB2312.Checked = true;
                            }
                        else 
                            {
                            chkGB2312.Checked = false;
                            }

                        richtxtSend.Text = TempSendText[ComboBoxCOMPort.SelectedIndex];
                        ComboBoxBaudRate.SelectedIndex = ComboBoxBaudRate.FindStringExact(RS232CArray[ComboBoxCOMPort.SelectedIndex].BaudRate.ToString());
                        //switch(RS232CArray[ComboBoxCOMPort.SelectedIndex].BaudRate)
                        //    {
                            
                        //    case 75:
                        //        break;
                        //    case 110:
                        //        break;
                        //    case 134:
                        //        break;
                        //    case 150:
                        //        break;
                        //    case 300:
                        //        break;
                        //    case 600:
                        //        break;
                        //    case 1200:
                        //        break;
                        //    case 1800:
                        //        break;
                        //    case 2400:
                        //        break;
                        //    case 4800:
                        //        break;
                        //    case 7200:
                        //        break;
                        //    case 9600:
                        //        break;
                        //    case 14400:
                        //        break;
                        //    case 19200:
                        //        break;
                        //    case 38400:
                        //        break;
                        //    case 57600:
                        //        break;
                        //    case 115200:
                        //        break;
                        //    case 128000:
                        //        break;
                        //    }
                        //************************************
                        ComboBoxDataBits.SelectedIndex = ComboBoxDataBits.FindStringExact(RS232CArray[ComboBoxCOMPort.SelectedIndex].DataBits.ToString());
                        
                        //************************************
                        ComboBoxParity.SelectedIndex = ComboBoxParity.FindStringExact(RS232CArray[ComboBoxCOMPort.SelectedIndex].Parity.ToString());

                        //************************************
                        //ComboBoxStopBit.SelectedIndex = ComboBoxStopBit.FindStringExact(RS232CArray[ComboBoxCOMPort.SelectedIndex].StopBits.ToString());
                        switch(RS232CArray[ComboBoxCOMPort.SelectedIndex].StopBits)
                            {
                            
                            case System.IO.Ports.StopBits.None:
                                ComboBoxStopBit.SelectedIndex = 3;
                                break;

                            case System.IO.Ports.StopBits.One:
                                ComboBoxStopBit.SelectedIndex = 0;
                                break;

                            case System.IO.Ports.StopBits.OnePointFive:
                                ComboBoxStopBit.SelectedIndex = 2;
                                break;

                            case System.IO.Ports.StopBits.Two:
                                ComboBoxStopBit.SelectedIndex = 1;
                                break;
                            
                            }

                        //************************************
                        switch(RS232CArray[ComboBoxCOMPort.SelectedIndex].Handshake)
                            {
                            
                            case System.IO.Ports.Handshake.None:
                                ComboBoxStopBit.SelectedIndex = 0;
                                break;

                            case System.IO.Ports.Handshake.RequestToSend:
                                ComboBoxStopBit.SelectedIndex = 1;
                                break;

                            case System.IO.Ports.Handshake.RequestToSendXOnXOff:
                                ComboBoxStopBit.SelectedIndex = 2;
                                break;

                            case System.IO.Ports.Handshake.XOnXOff:
                                ComboBoxStopBit.SelectedIndex = 3;
                                break;
                            
                            }
                        
                        //************************************
                        txtForCustomizedEndingCode.Text = TempCustomSuffix[ComboBoxCOMPort.SelectedIndex];
                        if(TempCustomSuffixFlag[ComboBoxCOMPort.SelectedIndex]==true)
                            {
                            txtForCustomizedEndingCode.Visible=true;
                            }
                        else
                            {
                            txtForCustomizedEndingCode.Visible=false;
                            }

                        switch(TempEnding[ComboBoxCOMPort.SelectedIndex])
                            {
                            case Endings.None:
                                ComboBoxSuffix.SelectedIndex = 0;
                                break;

                            case Endings.LF:
                                ComboBoxSuffix.SelectedIndex = 1;
                                break;

                            case Endings.CR:
                                ComboBoxSuffix.SelectedIndex = 2;
                                break;

                            case Endings.CRLF:
                                ComboBoxSuffix.SelectedIndex = 3;
                                break;

                            case Endings.Customerize:
                                ComboBoxSuffix.SelectedIndex = 4;
                                break;
                            }

                        //************************************
                        if(RS232CArray[ComboBoxCOMPort.SelectedIndex].RtsEnable==true)
                            {
                            CheckboxRTSEnable.Checked =true;
                            }
                        else
                            {
                            CheckboxRTSEnable.Checked =false;
                            }

                          //************************************
                        if( RS232CArray[ComboBoxCOMPort.SelectedIndex].DtrEnable==true)
                            {
                            CheckboxDTREnable.Checked =true;
                            }
                        else
                            {
                            CheckboxDTREnable.Checked =false;
                            }

                        //************************************
                        if(TempReadLine[ComboBoxCOMPort.SelectedIndex] ==false )
                            {
                            chkReadLineOrByte.Checked =true;
                            }
                        else
                            {
                            chkReadLineOrByte.Checked =false;
                            }

                        //************************************
                        if(TempAutoSendFlag[ComboBoxCOMPort.SelectedIndex]==true)
                            {
                            chkAutoSend.Checked =true;
                            }
                        else
                            {
                            chkAutoSend.Checked =false;
                            }

                        //************************************
                        if(TempSendFileFlag[ComboBoxCOMPort.SelectedIndex]==true)
                            {
                            chkSendFile.Checked =true;
                            }
                        else
                            {
                            chkSendFile.Checked =false;
                            }

                        //************************************
                        if(TempSendHEXFlag[ComboBoxCOMPort.SelectedIndex]==true)
                            {
                            chkSendHEX.Checked =true;
                            }
                        else
                            {
                            chkSendHEX.Checked =false;
                            }

                        //************************************
                        if( RS232CArray[ComboBoxCOMPort.SelectedIndex].IsOpen==true)
                            {
                            this.btnSendMessage.Enabled = true;
                            //chkReadLineOrByte.Enabled = false;
                            CheckboxRTSEnable.Enabled = false;
                            CheckboxDTREnable.Enabled = false;
                            ComboBoxBaudRate.Enabled = false;
                            ComboBoxDataBits.Enabled = false;
                            ComboBoxParity.Enabled = false;
                            ComboBoxStopBit.Enabled = false;
                            ComboBoxHandShake.Enabled = false;
                            ComboBoxSuffix.Enabled = false;

                            if(LanguageInChinese==true)
                                {
                                btnOpenPort.Text = "关闭串口";
                                }
                            else
                                {
                                btnOpenPort.Text = "Close Port";
                                }

                            }
                        else
                            {
                            
                            this.btnSendMessage.Enabled = false;
                            //chkReadLineOrByte.Enabled = true;
                            CheckboxRTSEnable.Enabled = true;
                            CheckboxDTREnable.Enabled = true;
                            ComboBoxBaudRate.Enabled = true;
                            ComboBoxDataBits.Enabled = true;
                            ComboBoxParity.Enabled = true;
                            ComboBoxStopBit.Enabled = true;
                            ComboBoxHandShake.Enabled = true;
                            ComboBoxSuffix.Enabled = true;

                            if(LanguageInChinese==true)
                                {
                                btnOpenPort.Text = "打开串口";
                                }
                            else
                                {
                                btnOpenPort.Text = "Open Port";
                                }

                            }

                        }
                    
                    }
                
                }
            catch(Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }

            }

        private void ComboBoxBaudRate_SelectedIndexChanged(object sender, EventArgs e)
            {

            try
                {
                
                if(ComboBoxBaudRate.SelectedIndex != -1)
                    {
                    //RS232CArray[ComboBoxCOMPort.SelectedIndex].BaudRate =(int)ComboBoxBaudRate.SelectedItem;//Conversion.Val(ComboBoxBaudRate.SelectedItem);
                    RS232CArray[ComboBoxCOMPort.SelectedIndex].BaudRate =Convert.ToInt32(ComboBoxBaudRate.SelectedItem);
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void ComboBoxDataBits_SelectedIndexChanged(object sender, EventArgs e)
            {

            try
                {

                    if(ComboBoxDataBits.SelectedIndex != -1)
                    {
                     //RS232CArray[ComboBoxCOMPort.SelectedIndex].DataBits =(int)ComboBoxDataBits.SelectedItem;//Conversion.Val(ComboBoxDataBits.SelectedItem);
                    RS232CArray[ComboBoxCOMPort.SelectedIndex].DataBits = Convert.ToInt32(ComboBoxDataBits.SelectedItem);
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void ComboBoxParity_SelectedIndexChanged(object sender, EventArgs e)
            {

            try
                {

                    if(ComboBoxParity.SelectedIndex!=-1)
                    {
                    
                        switch(ComboBoxParity.SelectedIndex)
                            {
                            

                            case 0:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].Parity = System.IO.Ports.Parity.Even;
                                break;
                                
                            case 1:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].Parity = System.IO.Ports.Parity.Mark;
                                break;

                            case 2:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].Parity = System.IO.Ports.Parity.None;
                                break;

                            case 3:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].Parity = System.IO.Ports.Parity.Odd;
                                break;

                            case 4:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].Parity = System.IO.Ports.Parity.Space;
                                break;

                            default:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].Parity = System.IO.Ports.Parity.None;
                                ComboBoxParity.SelectedIndex = 2;
                                break;
                            
                            }

                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void ComboBoxStopBit_SelectedIndexChanged(object sender, EventArgs e)
            {

            try
                {

                    if(ComboBoxStopBit.SelectedIndex!=-1)
                    {
                    
                         switch(ComboBoxStopBit.SelectedIndex)
                            {
                            
                            case 0:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].StopBits = System.IO.Ports.StopBits.One;
                                break;
                                
                            case 1:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].StopBits = System.IO.Ports.StopBits.Two;
                                break;

                            case 2:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].StopBits = System.IO.Ports.StopBits.OnePointFive;
                                break;

                            case 3:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].StopBits = System.IO.Ports.StopBits.None;
                                break;

                            default:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].StopBits = System.IO.Ports.StopBits.One;
                                ComboBoxParity.SelectedIndex = 0;
                                break;
                            
                            }

                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void ComboBoxHandShake_SelectedIndexChanged(object sender, EventArgs e)
            {

            try
                {

                    if(ComboBoxHandShake.SelectedIndex!=-1)
                    {
                    
                          switch(ComboBoxHandShake.SelectedIndex)
                            {
                            
                            case 0:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].Handshake = System.IO.Ports.Handshake.None;
                                break;
                                
                            case 1:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].Handshake = System.IO.Ports.Handshake.RequestToSend;
                                break;

                            case 2:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].Handshake = System.IO.Ports.Handshake.RequestToSendXOnXOff;
                                break;

                            case 3:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].Handshake = System.IO.Ports.Handshake.XOnXOff;
                                break;

                            default:
                                RS232CArray[ComboBoxCOMPort.SelectedIndex].Handshake = System.IO.Ports.Handshake.None;
                                ComboBoxParity.SelectedIndex = 0;
                                break;
                            
                            }

                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void ComboBoxSuffix_SelectedIndexChanged(object sender, EventArgs e)
            {

            try
                {

                    if(ComboBoxSuffix.SelectedIndex!=-1)
                    {

                            switch(ComboBoxSuffix.SelectedIndex)
                            {
                            //\r回车符，\n换行符
                            case 0:
                                    txtForCustomizedEndingCode.Visible = false;
                                    TempCustomSuffixFlag[ComboBoxCOMPort.SelectedIndex]=false;
                                    RS232CArray[ComboBoxCOMPort.SelectedIndex].NewLine = null;
                                    TempEnding[ComboBoxCOMPort.SelectedIndex]= Endings.None;
                                    TempCustomSuffix[ComboBoxCOMPort.SelectedIndex] = null;
                                    break;
                                
                            case 1:
                                    txtForCustomizedEndingCode.Visible = false;
                                    TempCustomSuffixFlag[ComboBoxCOMPort.SelectedIndex]=false;
                                    RS232CArray[ComboBoxCOMPort.SelectedIndex].NewLine = "\n";
                                    TempEnding[ComboBoxCOMPort.SelectedIndex]= Endings.LF;
                                    TempCustomSuffix[ComboBoxCOMPort.SelectedIndex] = "\n";
                                    break;

                            case 2:
                                    txtForCustomizedEndingCode.Visible = false;
                                    TempCustomSuffixFlag[ComboBoxCOMPort.SelectedIndex]=false;
                                    RS232CArray[ComboBoxCOMPort.SelectedIndex].NewLine = "\r";
                                    TempEnding[ComboBoxCOMPort.SelectedIndex]= Endings.CR;
                                    TempCustomSuffix[ComboBoxCOMPort.SelectedIndex] = "\r";
                                    break;

                            case 3:
                                    txtForCustomizedEndingCode.Visible = false;
                                    TempCustomSuffixFlag[ComboBoxCOMPort.SelectedIndex]=false;
                                    RS232CArray[ComboBoxCOMPort.SelectedIndex].NewLine = "\r\n";
                                    TempEnding[ComboBoxCOMPort.SelectedIndex]= Endings.CRLF;
                                    TempCustomSuffix[ComboBoxCOMPort.SelectedIndex] = "\r\n";
                                    break;

                            case 4:
                                    txtForCustomizedEndingCode.Visible = true;
                                    txtForCustomizedEndingCode.Focus();
                                    txtForCustomizedEndingCode.Text=TempCustomSuffix[ComboBoxCOMPort.SelectedIndex];
                                    TempEnding[ComboBoxCOMPort.SelectedIndex]= Endings.Customerize;
                                    if(txtForCustomizedEndingCode.Text!="")
                                        {
                                        RS232CArray[ComboBoxCOMPort.SelectedIndex].NewLine = txtForCustomizedEndingCode.Text;
                                        }
                                    else
                                        {
                                        MessageBox.Show("Please make sure to input the customerized ending text in the textbox.","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
                                        }
                                    TempCustomSuffix[ComboBoxCOMPort.SelectedIndex] =txtForCustomizedEndingCode.Text;
                                    TempCustomSuffixFlag[ComboBoxCOMPort.SelectedIndex]=true;
                                    break;
                            
                            }  

                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void txtForCustomizedEndingCode_TextChanged(object sender, EventArgs e)
            {

            try
                {

                    if(TempCustomSuffixFlag[ComboBoxCOMPort.SelectedIndex]==true)
                    {
                    TempCustomSuffix[ComboBoxCOMPort.SelectedIndex] = txtForCustomizedEndingCode.Text;
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void btnOpenPort_Click(object sender, EventArgs e)
            {

            try
                {

                if(AvailableRS232CPorts <= 0)
                    {
                    MessageBox.Show("此电脑无可用RS232C端口.");
                    btnSendMessage.Enabled=false;
                    return;
                    }

                if (ComboBoxCOMPort.SelectedIndex==-1) 
                    {
                    MessageBox.Show("在执行打开/关闭前，请先选中相应的端口。");
                    return;
                    }

                lblTargetComPort.Text = RS232CArray[ComboBoxCOMPort.SelectedIndex].PortName;

                if(RS232CArray[ComboBoxCOMPort.SelectedIndex].IsOpen==true)
                    {
                    RS232CArray[ComboBoxCOMPort.SelectedIndex].Close();
                    AddText(RS232CArray[ComboBoxCOMPort.SelectedIndex].PortName + "已关闭");
                    if (LanguageInChinese==true)
                        {
                        btnOpenPort.Text = "打开端口";
                        }
                    else
                        {
                        btnOpenPort.Text = "Open Port";
                        }

                    this.btnSendMessage.Enabled = false;
                    chkReadLineOrByte.Enabled = true;

                    CheckboxRTSEnable.Enabled = true;
                    CheckboxDTREnable.Enabled = true;
                    ComboBoxBaudRate.Enabled = true;
                    ComboBoxDataBits.Enabled = true;
                    ComboBoxParity.Enabled = true;
                    ComboBoxStopBit.Enabled = true;
                    ComboBoxHandShake.Enabled = true;
                    //ComboBoxCOMPort.Enabled = true;
                    ComboBoxSuffix.Enabled = true;

                    }
                else
                    {
                    RS232CArray[ComboBoxCOMPort.SelectedIndex].Open();
                    AddText(RS232CArray[ComboBoxCOMPort.SelectedIndex].PortName + "已打开");
                    if(LanguageInChinese==true)
                        {
                         btnOpenPort.Text = "关闭端口";
                        }
                    else
                        {
                        btnOpenPort.Text = "Close Port";
                        }
                    this.btnSendMessage.Enabled = true;
                    //chkReadLineOrByte.Enabled = false;

                    CheckboxRTSEnable.Enabled = false;
                    CheckboxDTREnable.Enabled = false;
                    ComboBoxBaudRate.Enabled = false;
                    ComboBoxDataBits.Enabled = false;
                    ComboBoxParity.Enabled = false;
                    ComboBoxStopBit.Enabled = false;
                    ComboBoxHandShake.Enabled = false;
                    //ComboBoxCOMPort.Enabled = false;
                    ComboBoxSuffix.Enabled = false;
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
               
        private void btnSaveRS232CPara_Click(object sender, EventArgs e)
            {

            try
                {
                //用于保存返回可用端口的参数数组的参数
                if(AvailableRS232CPorts > 0)
                    {
                    
                    if(RS232CArray[ComboBoxCOMPort.SelectedIndex].IsOpen ==true)
                        {
                        SavedParaForAvailablePorts[ComboBoxCOMPort.SelectedIndex].BaudRateSetting=RS232CArray[ComboBoxCOMPort.SelectedIndex].BaudRate;
                        SavedParaForAvailablePorts[ComboBoxCOMPort.SelectedIndex].DataBitsSetting = RS232CArray[ComboBoxCOMPort.SelectedIndex].DataBits;
                        SavedParaForAvailablePorts[ComboBoxCOMPort.SelectedIndex].DTREnableSetting = RS232CArray[ComboBoxCOMPort.SelectedIndex].DtrEnable;
                        SavedParaForAvailablePorts[ComboBoxCOMPort.SelectedIndex].HandShakeSetting = RS232CArray[ComboBoxCOMPort.SelectedIndex].Handshake;
                        SavedParaForAvailablePorts[ComboBoxCOMPort.SelectedIndex].ParitySetting = RS232CArray[ComboBoxCOMPort.SelectedIndex].Parity;
                        SavedParaForAvailablePorts[ComboBoxCOMPort.SelectedIndex].PortNameSetting = AvailablePortNames[ComboBoxCOMPort.SelectedIndex];
                        SavedParaForAvailablePorts[ComboBoxCOMPort.SelectedIndex].ReadBufferSizeSetting = RS232CArray[ComboBoxCOMPort.SelectedIndex].ReadBufferSize;
                        SavedParaForAvailablePorts[ComboBoxCOMPort.SelectedIndex].ReceivedBytesThresholdSetting = RS232CArray[ComboBoxCOMPort.SelectedIndex].ReceivedBytesThreshold;
                        SavedParaForAvailablePorts[ComboBoxCOMPort.SelectedIndex].RTSEnableSetting = RS232CArray[ComboBoxCOMPort.SelectedIndex].RtsEnable;
                        SavedParaForAvailablePorts[ComboBoxCOMPort.SelectedIndex].StopBitsSetting = RS232CArray[ComboBoxCOMPort.SelectedIndex].StopBits;
                        SavedParaForAvailablePorts[ComboBoxCOMPort.SelectedIndex].WriteBufferSizeSetting = RS232CArray[ComboBoxCOMPort.SelectedIndex].WriteBufferSize;
                        SavedParaForAvailablePorts[ComboBoxCOMPort.SelectedIndex].Suffix = TempEnding[ComboBoxCOMPort.SelectedIndex]; // RS232CArray[ComboBoxCOMPort.SelectedIndex].NewLine;
                        SavedParaForAvailablePorts[ComboBoxCOMPort.SelectedIndex].EncodingSetting = RS232CArray[ComboBoxCOMPort.SelectedIndex].Encoding;

                        ErrorMessage = "已经为串口: " + RS232CArray[ComboBoxCOMPort.SelectedIndex].PortName + " 保存通讯参数...";
                        AddText("已经为串口: " + RS232CArray[ComboBoxCOMPort.SelectedIndex].PortName + " 保存通讯参数...");
                        //AlreadySetSerialPortParametersForRS232C = true;    //是否已经为串口通讯设置参数;

                        }
                    else
                        {
                        if(LanguageInChinese==true)
                            {
                            AddText("请打开串口后再点击'保存参数'.");
                            }
                        else
                            {
                            AddText("Please open the port before you execute 'Save Para'.");
                            }
                        MessageBox.Show("You did not test the RS232C port yet, please open it and test it before you save the parameters.\r\n" +
                            "你还没有测试RS232C端口，请在保存参数前先打开端口进行测试。", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    
                    }
                else
                    {
                    MessageBox.Show("此电脑无可用RS232C串口.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                
                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void picChinese_Click(object sender, EventArgs e)
            {

            try
                {

                if(LanguageInChinese ==false)
                    {
                    LanguageInChinese=true;
                    LanguageChange();
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void picFresh_Click(object sender, EventArgs e)
            {

            try
                {

                SearchingAvailableCOMPort();

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void picEnglish_Click(object sender, EventArgs e)
            {

            try
                {

                    if(LanguageInChinese ==true)
                    {
                    LanguageInChinese=false;
                    LanguageChange();
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void picHelp_Click(object sender, EventArgs e)
            {

            try
                {

                AboutDialog.ShowDialog();

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void richtxtSend_TextChanged(object sender, EventArgs e)
            {

            try
                {

                TempSendText[ComboBoxCOMPort.SelectedIndex] = richtxtSend.Text;

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        //private string[] ReadBufferInput;
        //private byte[] Read_Buffer_Input_Bytes;
        private void btnSendMessage_Click(object sender, EventArgs e)
            {

            //ReadBufferInput=new string[5000];
            //Read_Buffer_Input_Bytes=new byte[5000];

            try
                {

                if(TempAutoSendFlag[ComboBoxCOMPort.SelectedIndex]==true)
                    {

                    if(MessageBox.Show("Now the port: " + RS232CArray[ComboBoxCOMPort.SelectedIndex].PortName 
                        + " is under auto-send mode, are you sure to send it manually?\r\n" +
                        "端口 " + RS232CArray[ComboBoxCOMPort.SelectedIndex].PortName + 
                        "正在自动发送中，确定要进行手动发送？","Info",MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information)==DialogResult.No)
                        {
                        return;
                        }
                    else
                        {
                        TempAutoSendFlag[ComboBoxCOMPort.SelectedIndex] = false;
                        chkAutoSend.Checked = false;
                        }
                    
                    }

                if(TempSendFileFlag[ComboBoxCOMPort.SelectedIndex]==false)
                    {
                    //不发送文件
                    if(ComboBoxSuffix.SelectedIndex == 3 & txtForCustomizedEndingCode.Text == "")
                        {
                        AddText("自定义的结束符不能为空，请输入相应的文本。");
                        txtForCustomizedEndingCode.Focus();
                        MessageBox.Show("The text for the custom Ending can't be empty, please enter the text you want to be set.\r\n" +
                            "自定义的结束符不能为空，请输入相应的文本。","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        return;
                        }
                    else if(ComboBoxSuffix.SelectedIndex == 3 & txtForCustomizedEndingCode.Text != "")
                        {
                        TempCustomSuffix[ComboBoxCOMPort.SelectedIndex] = txtForCustomizedEndingCode.Text;
                        RS232CArray[ComboBoxCOMPort.SelectedIndex].NewLine = TempCustomSuffix[ComboBoxCOMPort.SelectedIndex]; //txtForCustomizedEndingCode.Text
                        }

                    if(richtxtSend.Text!="")
                        {
                        lblTargetComPort.Text = this.RS232CArray[ComboBoxCOMPort.SelectedIndex].PortName;

                        if(TempSendHEXFlag[ComboBoxCOMPort.SelectedIndex] ==true)
                            {
                            string TempString = richtxtSend.Text;
                            TempString = StringConvertToHEX(TempString);
                            RS232CArray[ComboBoxCOMPort.SelectedIndex].Write(TempString + TempCustomSuffix[ComboBoxCOMPort.SelectedIndex]);
                            AddText(RS232CArray[ComboBoxCOMPort.SelectedIndex].PortName + " Sent: " + TempString);
                            }
                        else
                            {
                            RS232CArray[ComboBoxCOMPort.SelectedIndex].Write(this.richtxtSend.Text + TempCustomSuffix[ComboBoxCOMPort.SelectedIndex]);
                            AddText(RS232CArray[ComboBoxCOMPort.SelectedIndex].PortName + " Sent: " + this.richtxtSend.Text + TempCustomSuffix[ComboBoxCOMPort.SelectedIndex]);
                            }
                        //richtxtHistory.Focus();
                        //richtxtSend.Focus();
                        }
                    
                    }
                else
                    {
                    //发送文件
                    //int TempCount=0;
                    byte[] Data;
                    string TempStr="";
                    double TotalSend=0.0;
                    bool TempLeft=false;
                    int TempQty=0;
                    //string TempStrToBeSent="";
                    //byte[] TempBytes=null;

                    if(newOpenFile[ComboBoxCOMPort.SelectedIndex].FileNames.Length > 0)
                        {
                        
                        for(int a=0;a<newOpenFile[ComboBoxCOMPort.SelectedIndex].FileNames.Length;a++)
                            {
                            
                            try
                                {
                                Data=pcFileSystem.FileSystem.ReadAllBytes(newOpenFile[ComboBoxCOMPort.SelectedIndex].FileNames[a]);
                                AddText("File: " + newOpenFile[ComboBoxCOMPort.SelectedIndex].FileNames[a] + 
                                    ". total: " + Data.Length + " bytes...");
                                TempStr = System.Text.Encoding.UTF8.GetString(Data);
                                TotalSend=0.0;
                                //判断是否有尾数没有发送完成，因为不可能刚刚好被整除
                                TotalSend = Data.Length / RS232CArray[ComboBoxCOMPort.SelectedIndex].WriteBufferSize;
                                TempLeft=false;
                                TempQty=(int)TotalSend;
                                //TempStrToBeSent="";
                                //TempBytes=null;

                                if(TempQty>1)
                                    {
                                    //发送的字节大于发送缓冲区的大小，需要进行切割，分开发送并中间要延时
                                    for(int b=0;b<TempQty;b++)
                                        {
                                        if(b==0)
                                            {
                                            RS232CArray[ComboBoxCOMPort.SelectedIndex].Write(Data, 0, 
                                                RS232CArray[ComboBoxCOMPort.SelectedIndex].WriteBufferSize);
                                            }
                                        else
                                            {
                                            RS232CArray[ComboBoxCOMPort.SelectedIndex].Write(Data, 
                                                b * RS232CArray[ComboBoxCOMPort.SelectedIndex].WriteBufferSize - 1,
                                                RS232CArray[ComboBoxCOMPort.SelectedIndex].WriteBufferSize);
                                            }

                                        AddText(RS232CArray[ComboBoxCOMPort.SelectedIndex].PortName + ": " + b + 1 + ", sent " + 
                                            RS232CArray[ComboBoxCOMPort.SelectedIndex].WriteBufferSize + "bytes...");

                                        while(RS232CArray[ComboBoxCOMPort.SelectedIndex].WriteTimeout > 0)
                                            {
                                            System.Windows.Forms.Application.DoEvents();
                                            }

                                        Thread.Sleep(5);

                                        if((b + 1) * RS232CArray[ComboBoxCOMPort.SelectedIndex].WriteBufferSize < Data.Length )
                                            {
                                            TempLeft = true;
                                            }

                                        }

                                    //如果有尾数，则将尾数部分发送出去
                                    if(TempLeft==true)
                                        {
                                        RS232CArray[ComboBoxCOMPort.SelectedIndex].Write(Data, 
                                            (TempQty * RS232CArray[ComboBoxCOMPort.SelectedIndex].WriteBufferSize) - 1, 
                                            Data.Length - (TempQty * RS232CArray[ComboBoxCOMPort.SelectedIndex].WriteBufferSize) + 1);
                                        AddText(RS232CArray[ComboBoxCOMPort.SelectedIndex].PortName + ": " + 
                                            TempQty + 1 + ", sent " + (Data.Length - TempQty * RS232CArray[ComboBoxCOMPort.SelectedIndex].WriteBufferSize) + "bytes...");
                                        Thread.Sleep(5);
                                        TempLeft=false;
                                        }

                                    }
                                else
                                    {
                                    //发送的字节小于等于发送缓冲区的大小
                                    RS232CArray[ComboBoxCOMPort.SelectedIndex].Write(Data, 0, Data.Length);
                                    AddText(RS232CArray[ComboBoxCOMPort.SelectedIndex].PortName + "sent " + Data.Length + " bytes...");                                    
                                    }

                                }
                            catch (Exception ex)
                                {
                                AddText(ex.Message);
                                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            }
                                                
                        }
                    
                    }
                
                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void richtxtHistory_MouseDoubleClick_1(object sender, MouseEventArgs e)
            {

            try
                {

                if(richtxtHistory.Text == "")
                    {
                    return;
                    }

                if(MessageBox.Show("Are you sure to clear all the log?\r\n" +
                    "确定要清除通讯记录吗？","Info",MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information)==DialogResult.Yes)
                    {
                    richtxtHistory.Text = "";
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void richtxtHistory_MouseDown_1(object sender, MouseEventArgs e)
            {

            try
                {

                    if(richtxtHistory.Text == "")
                    {
                    return;
                    }

                if(e.Button==MouseButtons.Right)
                    {
                    SaveLogAsTXTFile.DefaultExt="txt";
                    SaveLogAsTXTFile.Filter="TXT文本文件 (*.txt)|*.txt";
                    SaveLogAsTXTFile.Title = "保存运行记录至文件";

                    //'s'--将日期和时间格式化为可排序的索引。例如 2008-03-12T11:07:31。 s 字符以用户定义的时间格式显示秒钟。
                    //'u'--将日期和时间格式化为 GMT 可排序索引。如 2008-03-12 11:07:31Z。
                    SaveLogAsTXTFile.FileName = "RS232CLog" + "-" + Strings.Format(DateAndTime.Now, "yyyy'年'MM'月'dd'日' HH'点'mm'分'ss'秒'"); // "yyyy-MM-dd HH%h-mm%m-ss%s") //"s")
                    SaveLogAsTXTFile.RestoreDirectory=true;

                    if(SaveLogAsTXTFile.ShowDialog()==DialogResult.OK 
                        & SaveLogAsTXTFile.FileName!="")
                        {
                        richtxtHistory.SaveFile(SaveLogAsTXTFile.FileName, RichTextBoxStreamType.TextTextOleObjs);
                        }
                    
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void richtxtHistory_TextChanged_1(object sender, EventArgs e)
            {

            //try
            //    {


            //    }
            //catch (Exception ex)
            //    {
            //    AddText(ex.Message);
            //    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }

            }

        private void txtAutoSendInterval_TextChanged(object sender, EventArgs e)
            {

            try
                {

                if(txtAutoSendInterval.Text!="")
                    {
                    TempAutoSendInterval[ComboBoxCOMPort.SelectedIndex] = System.Convert.ToInt32(txtAutoSendInterval.Text);
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

         private void txtAutoSendInterval_KeyPress(object sender, KeyPressEventArgs e)
            {

            try
                {

                //如果按下的不是数字键和控制键，则删除总长度的一个字符，然后聚焦文本框【如果输入的第一个字符不是数字，则清除】
                if(!(char.IsNumber(e.KeyChar) | char.IsNumber(e.KeyChar)))
                    {
                    AddText("只能输入数字，请重新输入...");
                    e.Handled = true;
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void chkAutoSend_CheckedChanged(object sender, EventArgs e)
            {

            try
                {

                if(ComboBoxCOMPort.SelectedIndex!=-1)
                    {
                    
                    if(chkAutoSend.Checked==true)
                        {

                          if(txtAutoSendInterval.Text == "")
                            {
                            MessageBox.Show("Please set the auto-send interval first.\r\n" +
                                "请先设置自动发送的间隔时间.","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            txtAutoSendInterval.Text = "500";
                          }
                       
                          tmrAutoSend.Interval =Microsoft.VisualBasic.Conversion.Val(txtAutoSendInterval.Text);
                          TempAutoSendFlag[this.ComboBoxCOMPort.SelectedIndex] = true;
                            
                        //****************************
                        //if(txtAutoSendInterval.Text == "")
                        //    {
                        //    MessageBox.Show("Please set the auto-send interval first.\r\n" +
                        //        "请先设置自动发送的间隔时间.","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        //    txtAutoSendInterval.Text = "500";
                        //     tmrAutoSend.Interval =Microsoft.VisualBasic.Conversion.Val(txtAutoSendInterval.Text);
                        //    TempAutoSendFlag[this.ComboBoxCOMPort.SelectedIndex] = true;
                        //    }
                        //else
                        //    {
                        //    tmrAutoSend.Interval =Microsoft.VisualBasic.Conversion.Val(txtAutoSendInterval.Text);
                        //    TempAutoSendFlag[this.ComboBoxCOMPort.SelectedIndex] = true;
                        //    }
                        //****************************
                        
                        }
                    else
                        {
                        TempAutoSendFlag[this.ComboBoxCOMPort.SelectedIndex] = false;
                        }
                    
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void chkSendFile_CheckedChanged(object sender, EventArgs e)
            {

            try
                {

                if(chkSendFile.Checked==true)
                    {
                    newOpenFile[ComboBoxCOMPort.SelectedIndex].Filter = "所有文件|*.*";
                    newOpenFile[ComboBoxCOMPort.SelectedIndex].Multiselect = true;

                    if(newOpenFile[ComboBoxCOMPort.SelectedIndex].ShowDialog()==DialogResult.OK)
                        {

                        if(newOpenFile[ComboBoxCOMPort.SelectedIndex].FileNames.Length>0)
                            {
                            TempSendFileFlag[ComboBoxCOMPort.SelectedIndex] = true;
                            }
                        else
                            {
                            chkSendFile.Checked=false;
                            TempSendFileFlag[ComboBoxCOMPort.SelectedIndex] = false;
                            }
                        
                        }

                    }
                else
                    {
                    TempSendFileFlag[ComboBoxCOMPort.SelectedIndex] = false;
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void chkSendHEX_CheckedChanged(object sender, EventArgs e)
            {

            try
                {

                if(chkSendHEX.Checked==true)
                    {
                    TempSendHEXFlag[ComboBoxCOMPort.SelectedIndex] = true;
                    }
                else
                    {
                     TempSendHEXFlag[ComboBoxCOMPort.SelectedIndex] = false;
                    }

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        
        #endregion

 #region "已完成代码"

        /// <summary>
        /// 释放所有资源
        /// </summary>
        private void FreeAllResources() 
            {
            try
                {
                if (SaveLogAsTXTFile != null)
                    {
                    SaveLogAsTXTFile.Dispose();
                    }

                if (AboutDialog != null)
                    {
                    AboutDialog.Dispose();
                    }

                for (int a = 0; a < RS232CArray.Length; a++)
                    {
                    if (RS232CArray[a] != null)
                        {
                        if (RS232CArray[a].IsOpen == true)
                            {
                            RS232CArray[a].Close();
                            }
                        RS232CArray[a].Dispose();
                        RS232CArray[a] = null;
                        }

                    if (newOpenFile[a] != null)
                        {
                        newOpenFile[a].Dispose();
                        newOpenFile[a] = null;
                        }

                    }

                tmrAutoSend.Stop();
                tmrAutoSend.Dispose();

                GC.Collect();
                }
            catch (Exception) 
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
                for (Int16 a = 0; a < TargetString.Length - 1; a++)
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
        
        //搜索可用串口
        /// <summary>
        /// 搜索可用串口
        /// </summary>
        /// <returns>返回搜索到的串口名称字符串数组</returns>
        public string[] SearchAvailableRS232Port()
            {
            
                try
                {
                    string[] TempStr;
                    TempStr=System.IO.Ports.SerialPort.GetPortNames();
                    if(TempStr.GetLength(0)>0)
                        {
                        return TempStr;
                        }
                    else
                        {
                        return null;
                        }  

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
                }
            
            }

        //刷新计算机中可用串口并更新相应参数
        /// <summary>
        /// 刷新计算机中可用串口并更新相应参数
        /// </summary>
        private void SearchingAvailableCOMPort()
          {
          
                try
                {
                    AvailablePortNames=System.IO.Ports.SerialPort.GetPortNames();
                    AvailableRS232CPorts=AvailablePortNames.GetLength(0);

                    if(AvailableRS232CPorts>0)
                        {
                        
                          RS232CArray = new System.IO.Ports.SerialPort[AvailableRS232CPorts];
                        TempAutoSendFlag = new bool[AvailableRS232CPorts];
                        TempSendFileFlag = new bool[AvailableRS232CPorts];
                        TempSendHEXFlag = new bool[AvailableRS232CPorts];
                        TempReadLine = new bool[AvailableRS232CPorts];
                        TempCustomSuffixFlag = new bool[AvailableRS232CPorts];
                        TempAutoSendInterval = new int[AvailableRS232CPorts];
                        TempSendText = new string[AvailableRS232CPorts];
                        TempCustomSuffix = new string[AvailableRS232CPorts];
                        ReceivedString = new string[AvailableRS232CPorts];
                        TempEnding = new Endings[AvailableRS232CPorts];
                        SavedParaForAvailablePorts = new Parameters[AvailableRS232CPorts];

                        for (int a = 0; a < AvailableRS232CPorts; a++)
                            {
                            RS232CArray[a] = new System.IO.Ports.SerialPort();
                            RS232CArray[a].BaudRate = 115200;
                            RS232CArray[a].Parity = System.IO.Ports.Parity.None;
                            RS232CArray[a].Handshake = System.IO.Ports.Handshake.None;
                            RS232CArray[a].DataBits = 8;
                            RS232CArray[a].StopBits = System.IO.Ports.StopBits.One;
                            RS232CArray[a].NewLine = "\r\n";
                            RS232CArray[a].DtrEnable = false;
                            RS232CArray[a].RtsEnable = false;
                            RS232CArray[a].PortName = AvailablePortNames[a];
                            RS232CArray[a].ReadBufferSize = 1024;
                            RS232CArray[a].WriteBufferSize = 1024;
                            //RS232CArray[a].ReceivedBytesThreshold = 1;
                            RS232CArray[a].Encoding = System.Text.Encoding.UTF8;
                            //RS232CArray[a].Encoding = System.Text.Encoding.GetEncoding(936);  
                            //RS232CArray[a].Encoding = System.Text.Encoding.GetEncoding("GB2312");

                            //添加RS232CPort的DataReceived事件关联函数
                            RS232CArray[a].DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(RS232CPort_DataReceived);

                            //添加RS232CPort的ErrorReceived事件关联函数
                            RS232CArray[a].ErrorReceived += new System.IO.Ports.SerialErrorReceivedEventHandler(RS232CPort_ErrorReceived);

                            TempAutoSendFlag[a] = false;
                            TempSendFileFlag[a] = false;
                            TempSendHEXFlag[a] = false;
                            TempReadLine[a] = true;
                            TempCustomSuffixFlag[a] = false;
                            ReceivedString[a] = "";
                            TempAutoSendInterval[a] = 500;
                            TempSendText[a] = "";
                            TempCustomSuffix[a] = "\r\n";
                            TempEnding[a] = Endings.CRLF;

                            SavedParaForAvailablePorts[a].BaudRateSetting = 115200;
                            SavedParaForAvailablePorts[a].DataBitsSetting = 8;
                            SavedParaForAvailablePorts[a].DTREnableSetting = false;
                            SavedParaForAvailablePorts[a].HandShakeSetting = System.IO.Ports.Handshake.None;
                            SavedParaForAvailablePorts[a].ParitySetting = System.IO.Ports.Parity.None;
                            SavedParaForAvailablePorts[a].PortNameSetting = AvailablePortNames[a];
                            SavedParaForAvailablePorts[a].ReadBufferSizeSetting = 1024;
                            SavedParaForAvailablePorts[a].RTSEnableSetting = false;
                            SavedParaForAvailablePorts[a].StopBitsSetting = System.IO.Ports.StopBits.One;
                            SavedParaForAvailablePorts[a].WriteBufferSizeSetting = 1024;
                            SavedParaForAvailablePorts[a].EncodingSetting = System.Text.Encoding.UTF8;
                            //SavedParaForAvailablePorts[a].ReceivedBytesThresholdSetting = 1;
                            }

                    AddText("刷新后计算机有：" + AvailableRS232CPorts + " 个串口.");
                    //将可用串口名字输出到文本控件
                    string TempStr = "";
                    for (int a = 0; a < AvailablePortNames.Length; a++)
                        {
                        TempStr += AvailablePortNames[a] + "  ";
                        }
                    AddText("刷新后计算机可用串口：" + TempStr);

                    //将可用串口名字添加到ComboBox控件
                    ComboBoxCOMPort.Items.Clear();
                    ComboBoxCOMPort.Items.AddRange(AvailablePortNames);
                    ComboBoxCOMPort.SelectedIndex = 0;
                        
                        }
                    else
                        {
                        ComboBoxCOMPort.Items.Clear();
                        AddText("刷新后此计算机无可用RS232C串口.");
                        MessageBox.Show("There is no any RS232C port available on this computer.\r\n\r\n" +
                            "刷新后此电脑上没有任何可用的RS232C串口.","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        return;
                        }                

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }          
          
          }

        //切换界面语言
        /// <summary>
        /// 切换界面语言
        /// </summary>
        private void LanguageChange()
            {

            if (LanguageInChinese == true)
                {

                this.Text = "RS232C串口通讯软件";

                grpRS232CSetting.Text = "设置";

                lblBaudrate.Text = "波特率:";
                lblCommunicationHistory.Text = "端口通讯历史记录:";
                lblCOMPort.Text = "端口:";

                lblDataBits.Text = "数据位:";
                lblHandshake.Text = "握手协议:";

                lblParity.Text = "奇偶校验:";
                lblStopBit.Text = "停止位:";

                lblEndingForSendingMessage.Text = "结束符:";

                btnSendMessage.Text = "发送";
                btnSaveRS232CPara.Text = "保存参数";

                CheckboxRTSEnable.Text = "RTS有效";
                CheckboxDTREnable.Text = "DTR有效";

                chkSendHEX.Text = "16进制";

                chkReadLineOrByte.Text = "读字节";

                if (RS232CArray[ComboBoxCOMPort.SelectedIndex].IsOpen == true)
                    {
                    btnOpenPort.Text = "关闭端口";
                    }
                else
                    {
                    btnOpenPort.Text = "打开端口";
                    }

                chkSendFile.Text = "发送文件";

                lblAutoSendInterval.Text = "自动发送周期:";

                chkAutoSend.Text = "自动发送";
                chkGB2312.Text = "收发中文";

                }
            else
                {

                chkGB2312.Text = "GB2312";
                chkAutoSend.Text = "Auto Send";

                lblAutoSendInterval.Text = "Auto Send Interval:";

                chkSendFile.Text = "Send File";


                if (RS232CArray[ComboBoxCOMPort.SelectedIndex].IsOpen == true)
                    {
                    btnOpenPort.Text = "Close Port";
                    }
                else
                    {
                    btnOpenPort.Text = "Open Port";
                    }

                chkReadLineOrByte.Text = "ReadByte";

                chkSendHEX.Text = "HEX";

                this.Text = "RS232C Communication Software";

                grpRS232CSetting.Text = "Setting";

                lblBaudrate.Text = "Baud Rate:";
                lblCommunicationHistory.Text = "Communication history  of port:";
                lblCOMPort.Text = "COM port:";

                lblDataBits.Text = "Data Bits:";
                lblHandshake.Text = "Handshake:";

                lblParity.Text = "Parity:";
                lblStopBit.Text = "Stop Bit:";

                lblEndingForSendingMessage.Text = "Ending:";

                btnSendMessage.Text = "Send";
                btnSaveRS232CPara.Text = "Save Para";
                CheckboxRTSEnable.Text = "RTS Enable";
                CheckboxDTREnable.Text = "DTR Enable";

                }

            }

        //创建RS232CForm类的实例
        /// <summary>
        /// 创建RS232CForm类的实例
        /// </summary>
        /// <param name="DLLPassword"></param>
        public RS232CForm(string DLLPassword)
            {

            SuccessBuiltNew = false;
            PasswordIsCorrect = false;

            try
                {

                if (DLLPassword == "ThomasPeng"
                    || (DLLPassword == "pengdongnan")
                    || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;

                    AvailablePortNames = System.IO.Ports.SerialPort.GetPortNames();
                    AvailableRS232CPorts = AvailablePortNames.GetLength(0);

                    if (AvailableRS232CPorts > 0)
                        {

                        InitializeComponent();

                        RS232CArray = new System.IO.Ports.SerialPort[AvailableRS232CPorts];
                        TempAutoSendFlag = new bool[AvailableRS232CPorts];
                        TempSendFileFlag = new bool[AvailableRS232CPorts];
                        TempSendHEXFlag = new bool[AvailableRS232CPorts];
                        TempReadLine = new bool[AvailableRS232CPorts];
                        TempCustomSuffixFlag = new bool[AvailableRS232CPorts];
                        TempErrorFlag=new bool[AvailableRS232CPorts];
                        TempAutoSendInterval = new int[AvailableRS232CPorts];
                        TempSendText = new string[AvailableRS232CPorts];
                        TempCustomSuffix = new string[AvailableRS232CPorts];
                        ReceivedString = new string[AvailableRS232CPorts];
                        TempEnding = new Endings[AvailableRS232CPorts];
                        SavedParaForAvailablePorts = new Parameters[AvailableRS232CPorts];
                        TempGB2312Flag=new bool[AvailableRS232CPorts];
                        
                        for (int a = 0; a < AvailableRS232CPorts; a++)
                            {
                            RS232CArray[a] = new System.IO.Ports.SerialPort();
                            RS232CArray[a].BaudRate = 115200;
                            RS232CArray[a].Parity = System.IO.Ports.Parity.None;
                            RS232CArray[a].Handshake = System.IO.Ports.Handshake.None;
                            RS232CArray[a].DataBits = 8;
                            RS232CArray[a].StopBits = System.IO.Ports.StopBits.One;
                            RS232CArray[a].NewLine = "\r\n";
                            RS232CArray[a].DtrEnable = false;
                            RS232CArray[a].RtsEnable = false;
                            RS232CArray[a].PortName = AvailablePortNames[a];
                            RS232CArray[a].ReadBufferSize = 1024;
                            RS232CArray[a].WriteBufferSize = 1024;
                            //RS232CArray[a].ReceivedBytesThreshold = 1;
                            RS232CArray[a].Encoding = System.Text.Encoding.UTF8;
                            //RS232CArray[a].Encoding = System.Text.Encoding.GetEncoding(936);  
                            //RS232CArray[a].Encoding = System.Text.Encoding.GetEncoding("GB2312");

                            //添加RS232CPort的DataReceived事件关联函数
                            RS232CArray[a].DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(RS232CPort_DataReceived);

                            //添加RS232CPort的ErrorReceived事件关联函数
                            RS232CArray[a].ErrorReceived += new System.IO.Ports.SerialErrorReceivedEventHandler(RS232CPort_ErrorReceived);

                            TempAutoSendFlag[a] = false;
                            TempSendFileFlag[a] = false;
                            TempSendHEXFlag[a] = false;
                            TempReadLine[a] = true;
                            TempCustomSuffixFlag[a] = false;
                            ReceivedString[a] = "";
                            TempAutoSendInterval[a] = 500;
                            TempSendText[a] = "";
                            TempCustomSuffix[a] = "\r\n";
                            TempEnding[a] = Endings.CRLF;
                            TempGB2312Flag[a] = false;

                            SavedParaForAvailablePorts[a].BaudRateSetting = 115200;
                            SavedParaForAvailablePorts[a].DataBitsSetting = 8;
                            SavedParaForAvailablePorts[a].DTREnableSetting = false;
                            SavedParaForAvailablePorts[a].HandShakeSetting = System.IO.Ports.Handshake.None;
                            SavedParaForAvailablePorts[a].ParitySetting = System.IO.Ports.Parity.None;
                            SavedParaForAvailablePorts[a].PortNameSetting = AvailablePortNames[a];
                            SavedParaForAvailablePorts[a].ReadBufferSizeSetting = 1024;
                            SavedParaForAvailablePorts[a].RTSEnableSetting = false;
                            SavedParaForAvailablePorts[a].StopBitsSetting = System.IO.Ports.StopBits.One;
                            SavedParaForAvailablePorts[a].WriteBufferSizeSetting = 1024;
                            SavedParaForAvailablePorts[a].EncodingSetting = System.Text.Encoding.UTF8;
                            //SavedParaForAvailablePorts[a].ReceivedBytesThresholdSetting = 1;
                            }

                        PasswordIsCorrect = true;
                        SuccessBuiltNew = true;

                        tmrAutoSend.Elapsed += new System.Timers.ElapsedEventHandler(TmrAutoSend_Elasped);
                              
                        AddText("计算机有：" + AvailableRS232CPorts + " 个串口.");
                        //将可用串口名字输出到文本控件
                        string TempStr = "";
                        for (int a = 0; a < AvailablePortNames.Length; a++)
                            {
                            TempStr += AvailablePortNames[a] + "  ";
                            }
                        AddText("计算机可用串口：" + TempStr);

                        //将可用串口名字添加到ComboBox控件
                        ComboBoxCOMPort.Items.Clear();
                        ComboBoxCOMPort.Items.AddRange(AvailablePortNames);
                        ComboBoxCOMPort.SelectedIndex = 0;               

                        }
                    else
                        {
                        ComboBoxCOMPort.Items.Clear();
                        ComboBoxCOMPort.Enabled=false;
                        this.btnSendMessage.Enabled = false;
                        //chkReadLineOrByte.Enabled = false;
                        CheckboxRTSEnable.Enabled = false;
                        CheckboxDTREnable.Enabled = false;
                        ComboBoxBaudRate.Enabled = false;
                        ComboBoxDataBits.Enabled = false;
                        ComboBoxParity.Enabled = false;
                        ComboBoxStopBit.Enabled = false;
                        ComboBoxHandShake.Enabled = false;
                        ComboBoxSuffix.Enabled = false;

                        AddText("此电脑上没有任何可用的RS232C串口.");

                        PasswordIsCorrect = true;
                        SuccessBuiltNew = false ;

                        MessageBox.Show("There is no any RS232C port available on this computer.\r\n此电脑上没有任何可用的RS232C串口.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("创建串口窗体类的实例时出现错误！\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }

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

                string TempStr="";//new string();//= TargetText；//Strings.UCase(TargetText);
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
                if (richtxtHistory.InvokeRequired == true)
                    {
                    AddTextDelegate ActualAddMessageToRichTextBox = new AddTextDelegate(AddText);
                    richtxtHistory.Invoke(ActualAddMessageToRichTextBox, new object[] { TempStr });
                    }
                else
                    {

                    if (UpdatingSameMessage == true)
                        {
                        TempErrorMessage = TempStr;
                        if (ShowDateTimeForMessage == true)
                            {
                            richtxtHistory.AppendText(DateTime.Now + "   " + TempStr + "\r\n");
                            }
                        else 
                            {
                            richtxtHistory.AppendText(TempStr + "\r\n");
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
                                richtxtHistory.AppendText(DateTime.Now + "   " + TempStr + "\r\n");
                                }
                            else
                                {
                                richtxtHistory.AppendText(TempStr + "\r\n");
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
        
#endregion
        
        }//class

    }//namespace