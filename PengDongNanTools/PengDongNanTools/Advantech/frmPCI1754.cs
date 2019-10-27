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
using System.Threading;

#endregion

namespace PengDongNanTools
    {

    //显示更新PCI1754输入信号
    /// <summary>
    /// 显示更新PCI1754输入信号
    /// 【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    public partial class frmPCI1754 : Form
        {

        #region "变量定义"

        bool EnableUpdate = true;

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
        //private BDaqDio TargetDICard = null;
        //private InstantDiCtrl TargetPCI1754Card = null;

        private PCI1754 NewPCI1754 = null;
        private CommonFunction FC = new CommonFunction("彭东南");

        /// <summary>
        /// 是否需要窗体控件，根据实例化时的条件进行判断
        /// </summary>
        private bool NeedFormControlFlag = false;

        private PictureBox[] InputPicArray = new PictureBox[64];
        private Label[] InputLabelArray = new Label[64];
        private ToolTip ShowTips = new ToolTip();

        /// <summary>
        /// 变更每列PictureBox/Label的距离
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
                        int LeftPos = InputPicArray[0].Left, DistanceOfColumn = value;
                        for (int a = 0; a <= 7; a++)
                            {
                            for (int b = 0; b <= 7; b++)
                                {
                                InputPicArray[a * 8 + b].Left = LeftPos;
                                InputLabelArray[a * 8 + b].Left = LeftPos + 30;
                                }
                            LeftPos = LeftPos + DistanceOfColumn;
                            }
                        TempHeight = this.Height;
                        TempWidth = (InputPicArray[0].Width + InputLabelArray[0].Width + DistanceOfColumn) * 8;
                        this.Height = TempHeight;
                        this.MaximumSize = new Size(TempWidth, TempHeight + 30);
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
        /// 变更每行PictureBox/Label的距离
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
                        int TopPos = InputPicArray[0].Top, DistanceOfRow = value;
                        for (int a = 0; a <= 7; a++)
                            {
                            TopPos = InputPicArray[a * 8].Top;
                            for (int b = 0; b <= 7; b++)
                                {
                                InputPicArray[a * 8 + b].Top = TopPos;
                                InputLabelArray[a * 8 + b].Top = TopPos + 5;
                                TopPos = TopPos + DistanceOfRow;
                                }
                            }
                        TempHeight = InputPicArray[0].Height + value * 8;
                        this.Height = TempHeight;
                        TempWidth = this.Width;
                        this.MaximumSize = new Size(TempWidth, TempHeight + 20);
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
        /// 设置Label的显示文本
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
                        InputLabelArray[a].Text = value[a];
                        ShowTips.SetToolTip(InputPicArray[a], value[a]);
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
        
        private bool[] CurrentInputStatus = new bool[64];

        private Thread UpdateInputThread = null;

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }

        #endregion

        #region "函数代码"

        //创建PCI1754更新输入状态窗体类的实例
        /// <summary>
        /// 创建PCI1754更新输入状态窗体类的实例
        /// 【软件作者：彭东南, southeastofstar@163.com】
        /// </summary>
        /// <param name="TargetDeviceNumber">目标PCI1754设备卡号</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public frmPCI1754(int TargetDeviceNumber, string DLLPassword)
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

                    NewPCI1754 = new PCI1754(TargetDeviceNumber, "彭东南");

                    //if (NewPCI1754.SuccessBuilt == false)
                    //    {
                    //    SuccessBuiltNew = false;
                    //    throw new Exception("Error: Failed to open the PCI1754 card: " + TargetDeviceNumber
                    //        + "\r\n错误：打开PCI1754卡【 " + TargetDeviceNumber + " 】 失败，请检查卡是否存在或者已经正确安装。");
                    //    }

                    InitializeComponent();//*********
                    if (AddPicAndLabelArrayControls() == false) 
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
                //this.Dispose();
                return;
                }

            }

        //【重载】创建PCI1754更新输入状态窗体类的实例
        /// <summary>
        /// 【重载】创建PCI1754更新输入状态窗体类的实例
        /// 【软件作者：彭东南, southeastofstar@163.com】
        /// </summary>
        /// <param name="TargetCard">目标PCI1754卡【窗体控件的形式】</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public frmPCI1754(ref Automation.BDaq.InstantDiCtrl TargetCard, string DLLPassword)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;
            try
                {
                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan")
                    || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;

                    NewPCI1754 = new PCI1754(ref TargetCard, "彭东南");

                    //if (NewPCI1754.SuccessBuilt == false)
                    //    {
                    //    SuccessBuiltNew = false;
                    //    throw new Exception("Error: Failed to open the PCI1754 card: " + TargetCard.SelectedDevice.DeviceNumber
                    //        + "\r\n错误：打开PCI1754卡【 " + TargetCard.SelectedDevice.DeviceNumber + " 】 失败，请检查卡是否存在或者已经正确安装。");
                    //    }

                    InitializeComponent();//*********
                    if (AddPicAndLabelArrayControls() == false)
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
                //this.Dispose();
                return;
                }
            }

        //添加窗体控件
        /// <summary>
        /// 添加窗体控件
        /// </summary>
        /// <returns></returns>
        private bool AddPicAndLabelArrayControls()
            {
            try
                {
                //Add CheckBox array to the form
                int LeftPos = 10, TopPos = 10, DistanceOfColumn = 120, DistanceOfRow = 40;

                for (int a = 0; a <= 7; a++)
                    {
                    //LeftPos = LeftPos + DistanceOfColumn;// *a;
                    TopPos = 5;
                    for (int b = 0; b <= 7; b++)
                        {
                        //TopPos = TopPos + DistanceOfRow;// *b;
                        InputPicArray[a * 8 + b] = new PictureBox();
                        InputPicArray[a * 8 + b].Top = TopPos;
                        InputPicArray[a * 8 + b].Left = LeftPos;
                        InputPicArray[a * 8 + b].Tag = a * 8 + b;
                        //InputPicArray[a * 8 + b].BackColor = Color.Green;
                        InputPicArray[a * 8 + b].Size = new Size(25, 25);
                        InputPicArray[a * 8 + b].SizeMode = PictureBoxSizeMode.AutoSize;
                        //InputPicArray[a * 8 + b].Text = "InBit-" + (a * 8 + b);
                        InputPicArray[a * 8 + b].Image = global::PengDongNanTools.Properties.Resources.GreenOff;
                        
                        //this.Width = OutputChkBoxArray[0].Width;
                        this.Controls.Add(InputPicArray[a * 8 + b]);
                        ShowTips.SetToolTip(InputPicArray[a * 8 + b], "输入端口：" + a + ", 位：" + b);

                        InputLabelArray[a * 8 + b] = new Label();
                        InputLabelArray[a * 8 + b].Text = "InBit-" + (a * 8 + b);
                        InputLabelArray[a * 8 + b].Top = TopPos + 10;
                        InputLabelArray[a * 8 + b].Left = LeftPos + 30;
                        InputLabelArray[a * 8 + b].AutoSize = true;
                        InputLabelArray[a * 8 + b].AutoEllipsis = true;
                        //InputLabelArray[a * 8 + b].BackColor = Color.Green;
                        this.Controls.Add(InputLabelArray[a * 8 + b]);

                        TopPos = TopPos + DistanceOfRow;// *b;
                        }

                    LeftPos = LeftPos + DistanceOfColumn;// *a;
                    }

                TempHeight = InputPicArray[0].Height + DistanceOfRow * 8;
                TempWidth = InputPicArray[0].Width + DistanceOfColumn * 8;
                this.Width = TempWidth;
                this.Height = TempHeight + 20;
                this.MaximumSize = new Size(TempWidth, TempHeight + 20);
                this.Text = "PCI1754 设备号：" + NewPCI1754.DeviceNumber;

                return true;

                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //更新输入信号
        /// <summary>
        /// 更新输入信号
        /// </summary>
        private void UpdateInputSignal()
            {
            bool[] InputSignals = new bool[64];
            while (true) 
                {
                try
                    {
                    if (EnableUpdate == true) 
                        {
                        InputSignals = NewPCI1754.GetInputStatus();
                        for (int a = 0; a < 64; a++) 
                            {
                            if (InputSignals[a] == true)
                                {
                                FC.ChangeLabelBackColor(ref InputLabelArray[a], Color.Red);
                                FC.ChangeImageOfPicture(ref InputPicArray[a], 
                                    global::PengDongNanTools.Properties.Resources.GreenOn);
                                }
                            else 
                                {
                                FC.ChangeLabelBackColor(ref InputLabelArray[a], Color.Gray);
                                FC.ChangeImageOfPicture(ref InputPicArray[a],
                                    global::PengDongNanTools.Properties.Resources.GreenOff);
                                }
                            }
                        }
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    }
                }
            }

        #endregion

        #region "窗体事件"

        private void frmPCI1754_Load(object sender, EventArgs e)
            {
            EnableUpdate = true;
            if (UpdateInputThread == null) 
                {
                UpdateInputThread = new Thread(UpdateInputSignal);
                UpdateInputThread.IsBackground = true;
                UpdateInputThread.Start();
                }
            }

        private void frmPCI1754_FormClosing(object sender, FormClosingEventArgs e)
            {
            try
                {
                if (JustHideFormAtClosing == true)
                    {
                    e.Cancel = true;
                    EnableUpdate = false;
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

                InputLabelArray = null;
                InputPicArray = null;
                NewPCI1754.Dispose();
                e.Cancel = false;

                }
            catch (Exception)
                {

                }
            }

        private void pictureBox1_Click(object sender, EventArgs e)
            {
            SetColumnDistance = 160;
            }

        private void pictureBox2_Click(object sender, EventArgs e)
            {
            SetRowDistance = 35;
            }

        #endregion

        }//class

    }//namespace