#region "using"

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Automation.BDaq;

#endregion

#region "待处理事项"

//1、如何将数据结构中的数组在实例化以后进行值的传递？【】
//2、更新输出显示的思路：在load时初始化，然后每次单击时再刷新；或者写个线程一直进行刷新，可以进行实时监控；【】
//3、如果在主界面已经打开PCI1752控制卡，在实例化此类时再次打开控制卡是否有冲突？需要验证；【】

#endregion

namespace PengDongNanTools
    {

    //类：研华IO控制卡PCI1752输出控制界面
    /// <summary>
    /// 类：研华IO控制卡PCI1752输出控制界面
    /// 【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    public partial class frmPCI1752 : Form
        {

        #region "变量定义"

        private int TempWidth, TempHeight;

        /// <summary>
        /// 在关闭窗体时是否只是隐藏窗体【默认True】
        /// </summary>
        public bool JustHideFormAtClosing = true;

        /// <summary>
        /// 在关闭窗体时是否提示【默认True】
        /// </summary>
        public bool ShowPromptAtClosing = true;

        public string ErrorMessage = "";
        private bool SuccessBuiltNew = false, PasswordIsCorrect = false;

        private ErrorCode ErrCode;
        //private BDaqDevice TargetDevice = null;
        //private BDaqDio TargetDOCard = null;
        //private InstantDoCtrl TargetPCI1752Card = null;

        private PCI1752 NewPCI1752 = null;

        /// <summary>
        /// 是否需要窗体控件，根据实例化时的条件进行判断
        /// </summary>
        private bool NeedFormControlFlag = false;

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

        private unsafe PCI1752.Bits GetOutputBits = new PCI1752.Bits();

        private CheckBox[] OutputChkBoxArray = new CheckBox[64];
        //private fixed CheckBox OutputChkBoxArray[64];
        private ToolTip ShowTips = new ToolTip();

        /// <summary>
        /// 变更每列CheckBox的距离
        /// </summary>
        public int SetColumnDistance 
            {
            set 
                {
                if (value < 110)
                    {
                    MessageBox.Show("The column distance should be equal or over 110, please change it.\r\n"
                        + "列距离应该大于或等于110，请修改参数值。");
                    return;
                    }
                else 
                    {
                    if (SuccessBuiltNew == true)
                        {
                        int LeftPos = OutputChkBoxArray[0].Left, DistanceOfColumn = value;
                        for (int a = 0; a <= 7; a++)
                            {
                            for (int b = 0; b <= 7; b++)
                                {
                                OutputChkBoxArray[a * 8 + b].Left = LeftPos;
                                }
                            LeftPos = LeftPos + DistanceOfColumn;
                            }
                        TempHeight = this.Height;
                        TempWidth = OutputChkBoxArray[0].Width + DistanceOfColumn * 8;
                        this.Width = TempWidth;
                        this.Height = TempHeight;
                        this.MaximumSize = new Size(TempWidth, TempHeight);
                        }
                    else 
                        {
                        MessageBox.Show("Failed to change the column distance becuase you failed to build new class.\r\n"
                            + "变更列距离失败，因为你没有成功创建实例。");
                        return;
                        }
                    }
                }
            }
        
        /// <summary>
        /// 变更每行CheckBox的距离
        /// </summary>
        public int SetRowDistance
            {
            set
                {
                if (value < 30)
                    {
                    MessageBox.Show("The row distance should be equal or over 30, please change it.\r\n"
                        + "行距离应该大于或等于30，请修改参数值。");
                    return;
                    }
                else
                    {
                    if (SuccessBuiltNew == true)
                        {
                        int TopPos = OutputChkBoxArray[0].Top, DistanceOfRow = value;
                        for (int a = 0; a <= 7; a++)
                            {
                            TopPos = OutputChkBoxArray[a * 8].Top;
                            //TempHeight = 0;
                            for (int b = 0; b <= 7; b++)
                                {
                                OutputChkBoxArray[a * 8 + b].Top = TopPos;
                                //TempHeight = TempHeight + OutputChkBoxArray[a * 8 + b].Height + DistanceOfRow;
                                TopPos = TopPos + DistanceOfRow;
                                }
                            }
                        TempHeight = OutputChkBoxArray[0].Height * 7 + DistanceOfRow * 8;
                        TempWidth = this.Width;
                        this.Size = new Size(TempWidth, TempHeight);
                        //this.MaximumSize = new Size(TempWidth, TempHeight);
                        //this.MaximumSize = new Size(this.Width, this.Height);
                        }
                    else
                        {
                        MessageBox.Show("Failed to change the row distance becuase you failed to build new class.\r\n"
                            + "变更行距离失败，因为你没有成功创建实例。");
                        return;
                        }
                    }
                }
            }

        /// <summary>
        /// 设置CheckBox的显示文本
        /// </summary>
        public string[] SetText 
            {
            set 
                {
                if (value.Length < 64)
                    {
                    MessageBox.Show("The length of string array should be 64.", "Error");
                    return;
                    }
                else 
                    {
                    for (int a = 0; a < 64; a++)
                        {
                        OutputChkBoxArray[a].Text = value[a];
                        ShowTips.SetToolTip(OutputChkBoxArray[a], value[a]);
                        }
                    }
                }
            }

        /// <summary>
        /// 是否成功实例化
        /// </summary>
        public bool SuccessBuilt
            {
            get { return SuccessBuiltNew; }
            }

        private CheckBox TempChkBox = new CheckBox();

        private bool[] CurrentOutputStatus = new bool[64];

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }

        #endregion

        #region "函数代码"

        //创建PCI1752更新输出及输出状态回读窗体类的实例
        /// <summary>
        /// 创建PCI1752更新输出及输出状态回读窗体类的实例
        /// 【软件作者：彭东南, southeastofstar@163.com】
        /// </summary>
        /// <param name="TargetDeviceNumber">目标PCI1752设备卡号</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public frmPCI1752(int TargetDeviceNumber, string DLLPassword)
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

                    NewPCI1752 = new PCI1752(TargetDeviceNumber, "彭东南");
                    if (NewPCI1752.SuccessBuilt == false)
                        {
                        SuccessBuiltNew = false;
                        throw new Exception("Error: Failed to open the PCI1752 card: " + TargetDeviceNumber
                            + "\r\n错误：打开PCI1752卡【 " + TargetDeviceNumber + " 】 失败，请检查卡是否存在或者已经正确安装。");
                        }

                    InitializeComponent();//*********
                    if (AddCheckBoxes() == false) 
                        {
                        throw new Exception("Failed to initial the controls of form.\r\n"
                            + "初始化窗体控件失败。");
                        }
                    SuccessBuiltNew = true;
                    NeedFormControlFlag = false;

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
                this.Dispose();
                return;
                }

            }
        
        //【重载】创建PCI1752更新输出及输出状态回读窗体类的实例
        /// <summary>
        /// 【重载】创建PCI1752更新输出及输出状态回读窗体类的实例
        /// 【软件作者：彭东南, southeastofstar@163.com】
        /// </summary>
        /// <param name="TargetCard">目标PCI1752卡【窗体控件的形式】</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public frmPCI1752(ref Automation.BDaq.InstantDoCtrl TargetCard, string DLLPassword)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;
            try
                {
                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan")
                    || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;

                    NewPCI1752 = new PCI1752(ref TargetCard, "彭东南");
                    if (NewPCI1752.SuccessBuilt == false)
                        {
                        SuccessBuiltNew = false;
                        throw new Exception("Error: Failed to open the PCI1752 card: " + TargetCard.SelectedDevice.DeviceNumber
                            + "\r\n错误：打开PCI1752卡【 " + TargetCard.SelectedDevice.DeviceNumber + " 】 失败，请检查卡是否存在或者已经正确安装。");
                        }
                    InitializeComponent();//*********
                    if (AddCheckBoxes() == false)
                        {
                        throw new Exception("Failed to initial the controls of form.\r\n"
                            + "初始化窗体控件失败。");
                        }
                    SuccessBuiltNew = true;
                    NeedFormControlFlag = false;
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
                this.Dispose();
                return;
                }
            }

        //释放所有资源
        /// <summary>
        /// 释放所有资源
        /// </summary>
        private void FreeAllResource() 
            {
            try
                {
                OutputChkBoxArray = null;
                NewPCI1752.Dispose();
                NewPCI1752 = null;
                ShowTips = null;
                TempChkBox.Dispose();
                GC.Collect();
                }
            catch (Exception) 
                {
                
                }
            }

        //初始化CheckBox控件数组
        /// <summary>
        /// 初始化CheckBox控件数组
        /// </summary>
        /// <returns></returns>
        private bool AddCheckBoxes() 
            {
            try
                {
                //Add CheckBox array to the form
                int LeftPos = 10, TopPos = 10, DistanceOfColumn = 120, DistanceOfRow = 40;

                for (int a = 0; a <= 7; a++)
                    {
                    //LeftPos = LeftPos + DistanceOfColumn;// *a;
                    TopPos = 0;
                    for (int b = 0; b <= 7; b++)
                        {
                        //TopPos = TopPos + DistanceOfRow;// *b;
                        OutputChkBoxArray[a * 8 + b] = new CheckBox();
                        OutputChkBoxArray[a * 8 + b].Text = "OutBit-" + (a * 8 + b);
                        OutputChkBoxArray[a * 8 + b].Top = TopPos;
                        OutputChkBoxArray[a * 8 + b].Left = LeftPos;
                        OutputChkBoxArray[a * 8 + b].Tag = a * 8 + b;
                        OutputChkBoxArray[a * 8 + b].AutoEllipsis = true;
                        OutputChkBoxArray[a * 8 + b].CheckedChanged += new EventHandler(OutputChkBoxArray_CheckedChanged);
                        //this.Width = OutputChkBoxArray[0].Width;
                        this.Controls.Add(OutputChkBoxArray[a * 8 + b]);
                        ShowTips.SetToolTip(OutputChkBoxArray[a * 8 + b], "输出端口：" + a + ", 位：" + b);
                        TopPos = TopPos + DistanceOfRow;// *b;
                        }

                    LeftPos = LeftPos + DistanceOfColumn;// *a;
                    }

                TempHeight = OutputChkBoxArray[0].Height + DistanceOfRow * 8;
                TempWidth = OutputChkBoxArray[0].Width + DistanceOfColumn * 8;
                this.Width = TempWidth;
                this.Height = TempHeight;
                this.MaximumSize = new Size(TempWidth, TempHeight);
                this.Text = "PCI1752 设备号：" + NewPCI1752.DeviceNumber;

                return true;

                }
            catch (Exception ex) 
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        #endregion

        #region "窗体事件"

        private void Form2_Load(object sender, EventArgs e)  //unsafe
            {
            try
                {
                ShowTips.Active = true;

                ////Add CheckBox array to the form
                //int LeftPos = 10, TopPos = 10, DistanceOfColumn = 120, DistanceOfRow = 40;
                
                //for (int a = 0; a <= 7; a++)
                //    {
                //    //LeftPos = LeftPos + DistanceOfColumn;// *a;
                //    TopPos = 0;
                //    for (int b = 0; b <= 7; b++)
                //        {
                //        //TopPos = TopPos + DistanceOfRow;// *b;
                //        OutputChkBoxArray[a * 8 + b] = new CheckBox();
                //        OutputChkBoxArray[a * 8 + b].Text = "OutBit-" + (a * 8 + b);
                //        OutputChkBoxArray[a * 8 + b].Top = TopPos;
                //        OutputChkBoxArray[a * 8 + b].Left = LeftPos;
                //        OutputChkBoxArray[a * 8 + b].Tag = a * 8 + b;
                //        OutputChkBoxArray[a * 8 + b].CheckedChanged += new EventHandler(OutputChkBoxArray_CheckedChanged);
                //        //this.Width = OutputChkBoxArray[0].Width;
                //        this.Controls.Add(OutputChkBoxArray[a * 8 + b]);
                //        ShowTips.SetToolTip(OutputChkBoxArray[a * 8 + b], "输出端口：" + a + ", 位：" + b);
                //        TopPos = TopPos + DistanceOfRow;// *b;
                //        }
                    
                //    LeftPos = LeftPos + DistanceOfColumn;// *a;
                //    }

                //TempHeight = OutputChkBoxArray[0].Height + DistanceOfRow * 8;
                //TempWidth = OutputChkBoxArray[0].Width + DistanceOfColumn * 8;
                //this.Width = TempWidth;
                //this.Height = TempHeight;
                //this.MaximumSize = new Size(TempWidth, TempHeight);
                //this.Text = "PCI1752 设备号：" + NewPCI1752.DeviceNumber;                
                
                //GetOutputBits = NewPCI1752.GetOutputStatus();
                //for (int a = 0; a < 64; a++)
                //    {
                //    OutputChkBoxArray[a].Checked = GetOutputBits.OutBits[a];
                //    }

                CurrentOutputStatus = NewPCI1752.GetOutputStatusNew();
                for (int a = 0; a < 64; a++)
                    {
                    OutputChkBoxArray[a].Checked = CurrentOutputStatus[a];
                    if (OutputChkBoxArray[a].Checked == true)
                        {
                        OutputChkBoxArray[a].BackColor = Color.Red;
                        }
                    else 
                        {
                        OutputChkBoxArray[a].BackColor = Color.WhiteSmoke;
                        }
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                }
            }

        private void frmPCI1752_FormClosing(object sender, FormClosingEventArgs e)
            {
            try
                {
                if (JustHideFormAtClosing == true)
                    {
                    e.Cancel = true;
                    this.Hide();
                    return;
                    }

                if (ShowPromptAtClosing == true)
                    {
                    if (MessageBox.Show("Are you sure to exit?\r\n" +
                        "确定要退出吗？", "Info", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.No)
                        {
                        e.Cancel = true;
                        return;
                        }
                    }

                FreeAllResource();
                e.Cancel = false;

                //if (ShowPromptAtClosing == true)
                //    {
                //    if (MessageBox.Show("Are you sure to exit?\r\n" +
                //        "确定要退出吗？", "Info", MessageBoxButtons.YesNo,
                //        MessageBoxIcon.Information) == DialogResult.Yes)
                //        {
                //        FreeAllResource();
                //        e.Cancel = false;
                //        }
                //    else
                //        {
                //        e.Cancel = true;
                //        }
                //    }
                //else 
                //    {
                //    FreeAllResource();
                //    e.Cancel = false;
                //    }
                }
            catch (Exception)
                {

                }
            }

        int TempOutputBit = 0;

        /// <summary>
        /// CheckBox控件数组的事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputChkBoxArray_CheckedChanged(object sender, EventArgs e) 
            {
            try
                {
                TempChkBox = (CheckBox)sender;
                TempOutputBit = (int)TempChkBox.Tag;

                if (TempChkBox.Checked == true)
                    {
                    if (NewPCI1752.SetBit(TempOutputBit, true) == false) 
                        {
                        ErrorMessage = "置位输出位：" + TempOutputBit + " ON失败";
                        }
                    }
                else 
                    {
                    if (NewPCI1752.SetBit(TempOutputBit, false) == false)
                        {
                        ErrorMessage = "置位输出位：" + TempOutputBit + " OFF失败";
                        }
                    }

                CurrentOutputStatus = NewPCI1752.GetOutputStatusNew();
                for (int a = 0; a < 64; a++)
                    {
                    OutputChkBoxArray[a].Checked = CurrentOutputStatus[a];
                    if (OutputChkBoxArray[a].Checked == true)
                        {
                        OutputChkBoxArray[a].BackColor = Color.Red;
                        }
                    else
                        {
                        OutputChkBoxArray[a].BackColor = Color.WhiteSmoke;
                        }
                    }

                }
            catch (Exception ex) 
                {
                ErrorMessage = ex.Message;
                }
            }
        
        #endregion

        private void button1_Click(object sender, EventArgs e)
            {
            MessageBox.Show("Form width: " + this.Width + " \r\n Form height: " +
                this.Height);
            SetColumnDistance = 150;
            SetRowDistance = 50;

            MessageBox.Show("Form width: " + this.Width + " \r\n Form height: " +
                this.Height);
            }
        
        }//class

    }//namespace