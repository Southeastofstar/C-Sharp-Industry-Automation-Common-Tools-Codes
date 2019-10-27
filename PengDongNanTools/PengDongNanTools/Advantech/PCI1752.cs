#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Automation.BDaq;
using System.Windows.Forms;

#endregion

namespace PengDongNanTools
    {

    //研华数据采集卡PCI1752更新输出及输出状态回读控制类
    /// <summary>
    /// 研华数据采集卡PCI1752更新输出及输出状态回读控制类
    /// 【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    class PCI1752
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
        private BDaqDio TargetDOCard = null;
        private InstantDoCtrl TargetPCI1752Card = null;

        /// <summary>
        /// 是否需要窗体控件，根据实例化时的条件进行判断
        /// </summary>
        private bool NeedFormControlFlag = false;

        /// <summary>
        /// 创建新实例时输出卡是否只能回读，不能写输出
        /// </summary>
        private bool ReadOnlyFlag = false;

        /// <summary>
        /// 当前输出控制卡是否只能回读，不能写输出
        /// </summary>
        public bool CanReadOnly 
            {
            get { return ReadOnlyFlag; }
            }

        /// <summary>
        /// 输出位结构
        /// </summary>
        public unsafe struct Bits 
            {
            /// <summary>
            /// 64个输出位标志数组【0~63】
            /// </summary>
            public fixed bool OutBits[64];
            }

        private bool[] ReadOutStatus = new bool[64];

        /// <summary>
        /// 读取当前IO输出的状态
        /// </summary>
        public bool[] ReadCurrentOutputStatus 
            {
            get 
                {
                GetOutputStatusNew();
                return ReadOutStatus; 
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

        /// <summary>
        /// 当前打开的PCI1752卡设备号
        /// </summary>
        public int DeviceNumber 
            {
            get 
                {
                if (NeedFormControlFlag == true)
                    {
                    return TargetPCI1752Card.SelectedDevice.DeviceNumber;
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

        //创建PCI1752更新输出及输出状态回读类的实例
        /// <summary>
        /// 创建PCI1752更新输出及输出状态回读类的实例
        /// </summary>
        /// <param name="TargetDeviceNumber">目标PCI1752设备卡号</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public PCI1752(int TargetDeviceNumber, string DLLPassword)
            // Automation.BDaq.AccessMode TargetAccessMode,<param name="TargetAccessMode">访问模式</param>
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

                    //if (TargetAccessMode == AccessMode.ModeRead)
                    //    {
                    //    ReadOnlyFlag = true;
                    //    }
                    //else 
                    //    {
                    //    ReadOnlyFlag = false;
                    //    }

                    //ErrCode = Automation.BDaq.BDaqDevice.Open(TargetDeviceNumber, 
                    //    TargetAccessMode, out TargetDevice);

                    ErrCode = Automation.BDaq.BDaqDevice.Open(TargetDeviceNumber,
                        AccessMode.ModeWriteWithReset, out TargetDevice);

                    if (ErrCode == ErrorCode.Success)
                        {
                        ErrCode = TargetDevice.GetModule(0, out TargetDOCard);
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

        //【重载】创建PCI1752更新输出及输出状态回读类的实例
        /// <summary>
        /// 【重载】创建PCI1752更新输出及输出状态回读类的实例
        /// </summary>
        /// <param name="TargetCard">目标PCI1752卡【窗体控件的形式】</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public PCI1752(ref Automation.BDaq.InstantDoCtrl TargetCard, string DLLPassword)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;
            try
                {
                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan")
                    || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;
                    TargetPCI1752Card = TargetCard;
                    if (TargetPCI1752Card.Initialized == true)
                        {
                        NeedFormControlFlag = true;
                        ReadOnlyFlag = false;
                        SuccessBuiltNew = true;
                        }
                    else
                        {
                        MessageBox.Show("参数'TargetCard'传递的PCI1752控件初始化失败，没有选择设备或者是设备打开失败，请检查具体原因。","错误");
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

        //研华输出卡PCI1752刷新输出
        /// <summary>
        /// 研华输出卡PCI1752刷新输出
        /// </summary>
        /// <param name="Out">输出的IO数组，长度必须等于64，对应64位输出位</param>
        /// <returns>是否执行成功</returns>
        public bool SetOutput(IO[] Out)
            {
            if (PasswordIsCorrect == false || SuccessBuiltNew == false)
                {
                return false;
                }
            try
                {
                if(Out.Length != 64)
                    {
                    ErrorMessage = "函数SetOutput的参数'Out'数组长度不等于64";
                    return false;
                    }
                //思路：传入的参数数组总计64个，0~7为端口0，依次类推，直到端口7
                for (int Port = 0; Port <= 7; Port++) 
                    {
                    int PortOutputStatus = 0;
                    int TempByte = 0;
                    for (int a = 7; a >= 0; a--) 
                        {
                        if (Out[Port * 8 + a] == IO.On)
                            {
                            TempByte = 1;
                            }
                        else 
                            {
                            TempByte = 0;
                            }
                        PortOutputStatus = (PortOutputStatus << 1) | TempByte & 0x1;                        
                        }

                    //需要从窗体控件传入引用进行实例化
                    if (NeedFormControlFlag == true)
                        {
                        TargetPCI1752Card.Write(Port, (byte)PortOutputStatus);
                        }
                    else
                        {
                        //直接用设备编号进行实例化
                        TargetDOCard.DoWrite(Port, (byte)PortOutputStatus);
                        }
                    }

                return true;
                }
            catch (Exception)// ex)
                {
                //ErrorMessage = ex.Message;
                return false;
                }
            }

        //【重载】研华输出卡PCI1752刷新输出，在实例化后调用此函数的函数前面要加 unsafe
        /// <summary>
        /// 【重载】研华输出卡PCI1752刷新输出，在实例化后调用此函数的函数前面要加 unsafe
        /// </summary>
        /// <param name="Out">输出位数据结构</param>
        /// <returns>是否执行成功</returns>
        public unsafe bool SetOutput(Bits Out)
            {
            if (PasswordIsCorrect == false || SuccessBuiltNew == false)
                {
                return false;
                }
            try
                {
                //思路：传入的参数数组总计64个，0~7为端口0，依次类推，直到端口7
                for (int Port = 0; Port <= 7; Port++)
                    {
                    int PortOutputStatus = 0;
                    int TempByte = 0;
                    for (int a = 7; a >= 0; a--)
                        {
                        if (Out.OutBits[Port * 8 + a] == true)
                            {
                            TempByte = 1;
                            }
                        else
                            {
                            TempByte = 0;
                            }
                        PortOutputStatus = (PortOutputStatus << 1) | TempByte & 0x1;
                        }

                    //需要从窗体控件传入引用进行实例化
                    if (NeedFormControlFlag == true)
                        {
                        TargetPCI1752Card.Write(Port, (byte)PortOutputStatus);
                        }
                    else
                        {
                        //直接用设备编号进行实例化
                        TargetDOCard.DoWrite(Port, (byte)PortOutputStatus);
                        }
                    }

                return true;
                }
            catch (Exception)// ex)
                {
                //ErrorMessage = ex.Message;
                return false;
                }
            }

        //研华输出卡PCI1752输出状态回读，在实例化后调用此函数的函数前面要加 unsafe
        /// <summary>
        /// 研华输出卡PCI1752输出状态回读，在实例化后调用此函数的函数前面要加 unsafe
        /// </summary>
        /// <returns>返回输出位状态的数据结构</returns>
        public unsafe Bits GetOutputStatus()
            {
            Bits TempBits=new Bits();

            try
                {
                //Initial the return value;
                for (int a = 0; a < 64; a++)
                    {
                    TempBits.OutBits[a] = false;
                    }

                if (PasswordIsCorrect == false || SuccessBuiltNew == false)
                    {
                    return TempBits;
                    }

                //Read the output status first for each port, total 8 ports
                byte ReadPortData = 0;
                for (int Port = 0; Port <= 7; Port++)
                    {
                    if (NeedFormControlFlag == true)
                        {
                        TargetPCI1752Card.Read(Port, out ReadPortData);
                        }
                    else
                        {
                        TargetDOCard.DoRead(Port, out ReadPortData);
                        }

                    //Judge the status and change the return value for each bit of port
                    for (int Bit = 0; Bit <= 7; Bit++) 
                        {
                        //Once the bit is 1, then set the return value bit as true
                        if ((ReadPortData >> Bit & 0x1) == 1)
                            {
                            TempBits.OutBits[Port * 8 + Bit] = true;
                            }
                        else 
                            {
                            TempBits.OutBits[Port * 8 + Bit] = false;
                            }
                        }
                    }
                return TempBits;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return TempBits;
                }
            }
        
        //研华输出卡PCI1752输出状态回读
        /// <summary>
        /// 研华输出卡PCI1752输出状态回读
        /// </summary>
        /// <returns>返回输出位状态的数组【数组长度64】</returns>
        public bool[] GetOutputStatusNew()
            {
            try
                {
                //Initial the value;
                for (int a = 0; a < 64; a++)
                    {
                    ReadOutStatus[a] = false;
                    }

                if (PasswordIsCorrect == false || SuccessBuiltNew == false)
                    {
                    return ReadOutStatus;
                    }

                //Read the output status first for each port, total 8 ports
                byte ReadPortData = 0;
                for (int Port = 0; Port <= 7; Port++)
                    {
                    if (NeedFormControlFlag == true)
                        {
                        TargetPCI1752Card.Read(Port, out ReadPortData);
                        }
                    else
                        {
                        TargetDOCard.DoRead(Port, out ReadPortData);
                        }

                    //Judge the status and change the return value for each bit of port
                    for (int Bit = 0; Bit <= 7; Bit++) 
                        {
                        //Once the bit is 1, then set the return value bit as true
                        if ((ReadPortData >> Bit & 0x1) == 1)
                            {
                            ReadOutStatus[Port * 8 + Bit] = true;
                            }
                        else 
                            {
                            ReadOutStatus[Port * 8 + Bit] = false;
                            }
                        }
                    }
                return ReadOutStatus;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return ReadOutStatus;
                }
            }

        //设置某个输出位的状态【ON/OFF】：true - ON ; false - OFF
        /// <summary>
        /// 设置某个输出位的状态【ON/OFF】：true - ON ; false - OFF
        /// </summary>
        /// <param name="TargetBit">目标输出位【1~64】</param>
        /// <param name="SetOn">需要设置的状态：true - ON ; false - OFF</param>
        /// <returns>是否执行成功</returns>
        public bool SetBit(int TargetBit, bool SetOn)
            {
            if (PasswordIsCorrect == false || SuccessBuiltNew == false) 
                {
                return false;
                }

            if (TargetBit < 1 || TargetBit > 64) 
                {
                ErrorMessage = " 设置某个输出位的状态【ON/OFF】函数SetBit的参数'TargetBit'超出有效范围：1~64";
                return false;
                }
            int TempTargetBit = TargetBit;
            int Port = TempTargetBit / 8;
            int Bit = TempTargetBit % 8;
            byte ReadPortData = 0;
            try
                {
                //Select different device to read the current output status
                if (NeedFormControlFlag == true)
                    {
                    if (TargetPCI1752Card.Read(Port, out ReadPortData) != ErrorCode.Success)
                        {
                        return false;
                        }
                    }
                else
                    {
                    if (TargetDOCard.DoRead(Port, out ReadPortData) != ErrorCode.Success)
                        {
                        return false;
                        }
                    }

                //Set target bit ON or OFF
                if (SetOn == true)
                    {
                    ReadPortData = (byte)(ReadPortData | (1 << Bit));
                    }
                else
                    {
                    ReadPortData = (byte)(ReadPortData & (~(1 << Bit)));
                    }

                ErrorCode TempErr;
                if (NeedFormControlFlag == true)
                    {
                    TempErr = TargetPCI1752Card.Write(Port, ReadPortData);
                    }
                else
                    {
                    TempErr = TargetDOCard.DoWrite(Port, ReadPortData);
                    }

                if (TempErr == ErrorCode.Success)
                    {
                    return true;
                    }
                else
                    {
                    return false;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
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
                    TargetPCI1752Card.Dispose();
                    TargetPCI1752Card = null;
                    }

                TargetDOCard = null;
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