#region "using"

using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Automation.BDaq;
using System.Windows.Forms;

#endregion

namespace PengDongNanTools
    {

    //研华数据采集卡PCI1754更新输入信号类
    /// <summary>
    /// 研华数据采集卡PCI1754更新输入信号类
    /// 【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    class PCI1754
        {

        #region "变量定义"

        /// <summary>
        /// IO状态开/关枚举
        /// </summary>
        public enum IO
            {
            /// <summary>
            /// 信号开
            /// </summary>
            On = 0,

            /// <summary>
            /// 信号关
            /// </summary>
            Off = 1
            }

        public string ErrorMessage = "";
        private bool SuccessBuiltNew = false, PasswordIsCorrect = false;

        private ErrorCode ErrCode;
        private BDaqDevice TargetDevice = null;
        private BDaqDio TargetDICard = null;
        private InstantDiCtrl TargetPCI1754Card = null;

        /// <summary>
        /// 是否需要窗体控件，根据实例化时的条件进行判断
        /// </summary>
        private bool NeedFormControlFlag = false;
        
        /// <summary>
        /// 输入位结构
        /// </summary>
        public unsafe struct Bits
            {
            /// <summary>
            /// 64个输出位标志数组【0~63】
            /// </summary>
            public fixed bool InBits[64];
            }

        private bool[] ReadInStatus = new bool[64];

        /// <summary>
        /// 读取当前IO输入的状态
        /// </summary>
        public bool[] ReadCurrentInputStatus
            {
            get
                {
                //判断扫描线程是否工作，没有工作就执行函数
                if (UpdateInputSignal != null) 
                    {
                    if (UpdateInputSignal.IsAlive == true) 
                        {
                        return ReadInStatus;
                        }
                    }
                GetInputStatus();
                return ReadInStatus;
                }
            }

        /// <summary>
        /// 是否成功实例化
        /// </summary>
        public bool SuccessBuilt
            {
            get { return SuccessBuiltNew; }
            }

        private int TempDeviceNumber = 0;
        private byte PortData = 0;
        private Thread UpdateInputSignal = null;

        /// <summary>
        /// 当前打开的PCI1754卡设备号
        /// </summary>
        public int DeviceNumber
            {
            get
                {
                if (NeedFormControlFlag == true)
                    {
                    return TargetPCI1754Card.SelectedDevice.DeviceNumber;
                    }
                else
                    {
                    return TempDeviceNumber;
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
        
        #endregion
        
        #region "函数代码"

        //创建PCI1754更新输入类的实例
        /// <summary>
        /// 创建PCI1754更新输入类的实例
        /// </summary>
        /// <param name="TargetDeviceNumber">目标PCI1754设备卡号</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public PCI1754(int TargetDeviceNumber, string DLLPassword)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;
            try
                {
                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan") 
                    || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;

                    if (TargetDeviceNumber < 0) 
                        {
                        MessageBox.Show("'TargetDeviceNumber'设备卡号不能小于0，请改为正确参数。",
                            "参数错误");
                        return;
                        }

                    ErrCode = Automation.BDaq.BDaqDevice.Open(TargetDeviceNumber,
                        AccessMode.ModeWriteWithReset, out TargetDevice);

                    if (ErrCode == ErrorCode.Success)
                        {
                        ErrCode = TargetDevice.GetModule(0, out TargetDICard);
                        if (ErrCode == ErrorCode.Success)
                            {
                            TempDeviceNumber = TargetDeviceNumber;
                            SuccessBuiltNew = true;
                            NeedFormControlFlag = false;
                            }
                        else 
                            {
                            SuccessBuiltNew = false;
                            return;
                            }
                        }
                    else 
                        {
                        SuccessBuiltNew = false;
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
                MessageBox.Show("创建类的实例时出现错误！\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }            
            }

        //【重载】创建PCI1754更新输入类的实例
        /// <summary>
        /// 【重载】创建PCI1754更新输入类的实例
        /// </summary>
        /// <param name="TargetCard">目标PCI1754卡【窗体控件的形式】</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public PCI1754(ref Automation.BDaq.InstantDiCtrl TargetCard, string DLLPassword)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;
            try
                {
                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan")
                    || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;
                    TargetPCI1754Card = TargetCard;
                    if (TargetPCI1754Card.Initialized == true)
                        {
                        NeedFormControlFlag = true;
                        SuccessBuiltNew = true;
                        }
                    else
                        {
                        MessageBox.Show("参数'TargetCard'传递的PCI1754控件初始化失败，没有选择设备或者是设备打开失败，请检查具体原因。", "错误");
                        SuccessBuiltNew = false;
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
                MessageBox.Show("创建类的实例时出现错误！\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }
            }

        //研华输入卡PCI1754刷新输入
        /// <summary>
        /// 研华输入卡PCI1754刷新输入
        /// </summary>
        /// <returns>返回输入信号数组【数组长度64】：true - ON; false - OFF</returns>
        public bool[] GetInputStatus()
            {
            //initial the return value
            for (int a = 0; a < 64; a++) 
                {
                ReadInStatus[a] = false;
                }

            if (SuccessBuiltNew == false)
                {
                //MessageBox.Show("未成功建立类的新实例，无法开启线程进行输入信号扫描");
                ErrorMessage = "未成功建立类的新实例，无法开启线程进行输入信号扫描";
                return ReadInStatus;
                }

            try
                {
                for (int Port = 0; Port <= 7; Port++) 
                    {
                    if (NeedFormControlFlag == true)
                        {
                        TargetPCI1754Card.Read(Port, out PortData);
                        }
                    else 
                        {
                        TargetDICard.DiRead(Port, out PortData);
                        }
                    for (int Bit = 0; Bit <= 7; Bit++) 
                        {
                        ReadInStatus[Port * 8 + Bit] = ((PortData >> Bit)==1) ? true : false;
                        }
                    }

                return ReadInStatus;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return ReadInStatus;
                }
            }

        //开启线程监控PCI1754输入信号并实时更新
        /// <summary>
        /// 开启线程监控PCI1754输入信号并实时更新
        /// </summary>
        public void StartMonitor()
            {
            try
                {
                if (SuccessBuiltNew == false) 
                    {
                    //MessageBox.Show("未成功建立类的新实例，无法开启线程进行输入信号扫描");
                    ErrorMessage = "未成功建立类的新实例，无法开启线程进行输入信号扫描";
                    return;
                    }

                if (UpdateInputSignal != null) 
                    {
                    return;
                    }

                UpdateInputSignal = new Thread(ScanningInputStatus);
                UpdateInputSignal.IsBackground = true;
                UpdateInputSignal.Start();
                
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                }
            }
        
        //研华输入卡PCI1754刷新输入
        /// <summary>
        /// 研华输入卡PCI1754刷新输入
        /// </summary>
        private void ScanningInputStatus()
            {
            while (true) 
                {
                try
                    {
                    for (int Port = 0; Port <= 7; Port++)
                        {
                        if (NeedFormControlFlag == true)
                            {
                            TargetPCI1754Card.Read(Port, out PortData);
                            }
                        else
                            {
                            TargetDICard.DiRead(Port, out PortData);
                            }
                        for (int Bit = 0; Bit <= 7; Bit++)
                            {
                            ReadInStatus[Port * 8 + Bit] = ((PortData >> Bit) == 1) ? true : false;
                            }
                        }
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    }
                }
            }

        //释放相关资源
        /// <summary>
        /// 释放相关资源
        /// </summary>
        public void Dispose()
            {
            try
                {
                if (NeedFormControlFlag == true)
                    {
                    TargetPCI1754Card.Dispose();
                    TargetPCI1754Card = null;
                    }

                if (UpdateInputSignal != null)
                    {
                    UpdateInputSignal.Abort();
                    UpdateInputSignal = null;
                    }

                TargetDICard = null;
                TargetDevice.Close();
                TargetDevice.Dispose();

                }
            catch (Exception)// ex)
                {
                //ErrorMessage = ex.Message;;
                }
            }

        #endregion
        
        }//class
    }//namespace