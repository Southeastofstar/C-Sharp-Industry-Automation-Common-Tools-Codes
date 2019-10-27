#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.IO;
using System.Threading;
using System.Reflection;
using Microsoft.Office.Interop;
using System.Drawing;
using System.Drawing.Imaging;
using System.Management;

#endregion

#region "待处理事项"

//1、下面标注？的代理委托函数，目前有报跨线程操作的问题，即使本类继承formm然后用控件去代理也会报没有创建窗体句柄的错；
//   尝试传入用来执行委托的控件，此方法可行；【ok】

//2、改变原来void为bool型返回值，因为在需要委托invoke条件下return false就会返回false【此处返回值改为true】,真正执行完委托后的return true没有返回到，
//   【所以取消变更返回值类型】；【ok】

//3、添加各种进制之间的转换操作；【】

//4、截屏函数 待完善，目前无法截取整个显示器屏幕的内容；【】

#endregion

namespace PengDongNanTools
    {

    //通用函数库【软件作者：彭东南, southeastofstar@163.com】
    /// <summary>
    /// 通用函数库【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    public class CommonFunction 
        {

        #region "代理变量定义"

        private System.Windows.Forms.TextBox DelegateTextbox = new TextBox();
        private System.Windows.Forms.Button DelegateButton = new System.Windows.Forms.Button();

        //利用委托和代理进行跨线程安全操作控件，以此避免跨线程操作异常
        //*****************************
        private delegate bool UpdateTextBoxDelegate(ref System.Windows.Forms.TextBox TargetTextBox, string TargetText);
        private delegate bool UpdateRichTextBoxDelegate(ref System.Windows.Forms.RichTextBox TargetRichTextBox, string TargetText);
        private delegate bool AddTextDelegate(string TargetText);
        private delegate bool AddMessageToRichTextBox(ref System.Windows.Forms.RichTextBox TargetRichTextBox, string TargetText);
        //private AddMessageToRichTextBox ActualAddMessageToRichTextBox;
        private delegate bool AddMessageToTextBox(ref System.Windows.Forms.TextBox TargetTextBox, string TargetText);
        //private AddMessageToTextBox ActualAddMessageToTextBox;
        private delegate bool ClearTextBoxDelegate(ref System.Windows.Forms.TextBox TargetTextBox);
        private delegate bool ClearRichTextBoxDelegate(ref System.Windows.Forms.RichTextBox TargetRichTextBox);

        //*****************************
        private delegate bool ChangeButtonEnableStatusDelegate(ref System.Windows.Forms.Button TargetControl, bool TargetEnabledStatus);

        //private delegate void ChangeMenuEnableStatusDelegate(ref System.Windows.Forms.ToolStripMenuItem TargetControl, bool TargetEnabledStatus);
        private delegate bool ChangeMenuEnableStatusDelegate(ref Button InvokeButton, ref System.Windows.Forms.ToolStripMenuItem TargetControl, bool TargetEnabledStatus);

        //private delegate void ChangeMenuImageDelegate(ref System.Windows.Forms.ToolStripMenuItem TargetControl, System.Drawing.Image TargetImage);
        private delegate bool ChangeMenuImageDelegate(ref Button InvokeButton, ref ToolStripMenuItem TargetControl, System.Drawing.Image TargetImage);

        //private delegate void ChangeStatusLabelEnableStatusDelegate(ref System.Windows.Forms.ToolStripStatusLabel TargetControl, bool TargetEnabledStatus);
        private delegate bool ChangeStatusLabelEnableStatusDelegate(ref Button InvokeButton, ref System.Windows.Forms.ToolStripStatusLabel TargetControl, bool TargetEnabledStatus);
        //*****************************
        //private delegate void ChangeStatusLabelTextDelegate(ref System.Windows.Forms.ToolStripStatusLabel TargetControl, string TargetText);
        private delegate bool ChangeStatusLabelTextDelegate(ref Button InvokeButton, ref System.Windows.Forms.ToolStripStatusLabel TargetControl, string TargetText);

        private delegate bool ChangeLabelTextDelegate(ref System.Windows.Forms.Label TargetLabel, string TargetText);
        private delegate bool ChangeLabelPositionDelegate(ref Label TargetLabel, int Left, int Top);
        private delegate bool ChangeLabelBackColorDelegate(ref Label TargetLabel, Color NewColor);
        //*****************************
        //private delegate void ChangeStatusLabelBackColorDelegate(ref System.Windows.Forms.ToolStripStatusLabel TargetControl, System.Drawing.Color TargetColor);
        private delegate bool ChangeStatusLabelBackColorDelegate(ref Button InvokeButton, ref System.Windows.Forms.ToolStripStatusLabel TargetControl, System.Drawing.Color TargetColor);

        //private delegate void ChangeMenuBackColorDelegate(ref System.Windows.Forms.ToolStripMenuItem TargetControl, System.Drawing.Color TargetColor);
        private delegate bool ChangeMenuBackColorDelegate(ref Button InvokeButton, ref System.Windows.Forms.ToolStripMenuItem TargetControl, System.Drawing.Color TargetColor);

        private delegate bool ChangeButtonBackColorDelegate(ref System.Windows.Forms.Button TargetControl, System.Drawing.Color TargetColor);

        private delegate bool ChangeListViewRowBackColorDelegate(ref ListView TargetListView, int TargetRow, Color NewColor);

        private delegate bool ChangeImageOfPictureDelegate(ref PictureBox TargetPictreBox, System.Drawing.Image NewImage);
        private delegate bool ChangeBackColorOfPictureDelegate(ref PictureBox TargetPictreBox, Color NewColor);
        //*****************************
        private delegate bool ChangePositionDelegate(ref Control TargetControl, int Left, int Top);
        private delegate bool ChangeSizeDelegate(ref Control TargetControl, int Width, int Height);
        private delegate bool ChangeBackColorDelegate(ref Control TargetControl, Color TargetBackColor);
        private delegate bool ChangeForeColorDelegate(ref Control TargetControl, Color TargetForeColor);
        private delegate bool ChangeEnableDelegate(ref Control TargetControl, bool TargetEnableStatus);
        private delegate bool ChangeTextDelegate(ref Control TargetControl, string TargetText);
        private delegate bool ChangeBackgroundImageDelegate(ref Control TargetControl, Image TargetBackgroundImage);
        //*****************************


        #endregion

        #region "变量定义"

        /// <summary>
        /// Excel文件类型
        /// </summary>
        public enum ExcelFileType
            {
            /// <summary>
            /// xls格式Excel文件
            /// </summary>
            xls,

            /// <summary>
            /// xlsx格式Excel文件
            /// </summary>
            xlsx,

            /// <summary>
            /// csv格式Excel文件
            /// </summary>
            csv
            }

        /// <summary>
        /// 当前网络中可用IP地址明细数组
        /// </summary>
        public string[] AvailableIPAddress 
            {
            get { return SearchAvailableIPAddresses(); }
            }

        private bool NetworkConnected = true;

        /// <summary>
        /// 是否已经建立网络物理连接
        /// </summary>
        public bool NetworkIsAvailable 
            {
            get { return NetworkConnected; }
            }
        
        private bool EnabledUSB = true;

        /// <summary>
        /// USB是否启用【true：启用；false：禁用】
        /// </summary>
        public bool USBEnableStatus 
            {
            get { return EnabledUSB; }
            }

        /// <summary>
        /// 驱动器信息数据结构
        /// </summary>
        public struct DriveMessage
            {
            /// <summary>
            /// 驱动器名称
            /// </summary>
            public string Name;

            /// <summary>
            /// 文件系统名称
            /// </summary>
            public string FileSystem;

            /// <summary>
            /// 驱动器卷标
            /// </summary>
            public string VolumeLabel;

            /// <summary>
            /// 驱动器的总大小【单位：MB】
            /// </summary>
            public double TotalSpaceInMB;

            /// <summary>
            /// 驱动器上的剩余空间【单位：MB】
            /// </summary>
            public double TotalFreeSpaceInMB;
            }

        /// <summary>
        /// 操作系统数据结构
        /// </summary>
        public struct OS 
            {
            /// <summary>
            /// 操作系统的全名
            /// </summary>
            public string OSName;

            /// <summary>
            /// 计算机的操作系统的平台标识符
            /// </summary>
            public string OSID;

            /// <summary>
            /// 计算机操作系统的版本
            /// </summary>
            public string OSVer;

            /// <summary>
            /// 计算机的可用虚拟地址空间的总量【单位：MB】
            /// </summary>
            public ulong TotalVirtualMemory;

            /// <summary>
            /// 计算机的物理内存总量【单位：MB】
            /// </summary>
            public ulong TotalPhysicalMemory;

            /// <summary>
            /// 计算机的可用物理内存总量【单位：MB】
            /// </summary>
            public ulong AvailablePhysicalMemory;

            /// <summary>
            /// 计算机的可用虚拟地址空间的总量【单位：MB】
            /// </summary>
            public ulong AvailableVirtualMemory;

            }

        Microsoft.VisualBasic.Devices.Computer PC = new Microsoft.VisualBasic.Devices.Computer();

        ///// <summary>
        ///// Excel软件操作接口
        ///// </summary>
        //public Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
        
        ///// <summary>
        ///// Excel工作薄操作接口
        ///// </summary>
        //public Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;// = new Microsoft.Office.Interop.Excel.Workbook();
        
        ///// <summary>
        ///// Excel工作表操作接口
        ///// </summary>
        //public Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;// = new Microsoft.Office.Interop.Excel.Worksheet();

        private int AvailableRS232CPorts = 0;
        private string[] AvailablePortNames;

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }

        /// <summary>
        /// 当错误信息相同时，是否重复显示
        /// </summary>
        public bool UpdatingSameMessage = true;

        /// <summary>
        /// 更新信息至文本框时是否显示日期和时间，默认为True
        /// </summary>
        public bool ShowDateTimeForMessage = true;

        string TempErrorMessage = "";

        /// <summary>
        /// 程序运行过程中的错误信息提示
        /// </summary>
        public string ErrorMessage = "";

        /// <summary>
        /// 用于记录实例化时输入密码是否正确
        /// </summary>
        private bool PasswordIsCorrect;//=false;

        /// <summary>
        /// 成功建立新的实例
        /// </summary>
        private bool SuccessBuiltNew = false;

        #endregion
        
        //通用函数库实例化
        /// <summary>
        /// 通用函数库实例化
        /// </summary>
        /// <param name="DLLPassword">使用此类的密码</param>
        public CommonFunction(string DLLPassword)
            {

            SuccessBuiltNew = false;
            PasswordIsCorrect = false;

            try
                {

                if (DLLPassword == "ThomasPeng"
                    || (DLLPassword == "pengdongnan")
                    || (DLLPassword == "彭东南"))
                    {
                    
                    //this.Controls.Add(this.DelegateButton);
                    //this.Controls.Add(this.DelegateTextbox);
                    PasswordIsCorrect = true;

                    if (PC.Network.IsAvailable == true)
                        {
                        NetworkConnected = true;
                        }
                    else 
                        {
                        NetworkConnected = false;
                        }
                    AddNetworkChange_EventHandler();
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
                MessageBox.Show("创建串口窗体类的实例时出现错误！\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }

            }

        #region "委托和代理的实现代码"
        
        //****************************
        //修改ListView控件某行的背景颜色
        /// <summary>
        /// 修改ListView控件某行的背景颜色
        /// </summary>
        /// <param name="TargetListView">目标ListView控件</param>
        /// <param name="TargetRow">目标行1~N</param>
        /// <param name="NewRowColor">新背景颜色</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeListViewRowBackColor(ref ListView TargetListView, int TargetRow,
            Color NewRowColor)
            {

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return false;
                    }

                if (TargetRow <= 0)
                    {
                    return false;
                    }

                if (TargetListView == null)
                    {
                    return false;
                    }

                //1、根据现有行的数量，无行或者目标行超出现有行数则提示并退出；
                if (TargetListView.Items.Count < 1
                    || TargetListView.Items.Count < TargetRow)
                    {
                    //MessageBox.Show("ListView has " + TargetListView.Items.Count + " row(s), the q'ty of contents" +
                    //"is overrange, please revise it and retry.\r\n ListView控件有" +
                    //TargetListView.Items.Count + "行, 添加的内容已经大于可用行的数量，请修改参数后重试.");
                    ErrorMessage = "ListView控件有" + TargetListView.Items.Count +
                        "行, 添加的内容已经大于可用行的数量，请修改参数后重试.";
                    return false;
                    }
                
                //2、直接将当前新值修改到ListView中的目标行
                //ChangeListViewRowBackColorDelegate(ref ListView TargetListView, int TargetRow, Color NewColor);

                if (TargetListView.InvokeRequired == true)
                    {
                    ChangeListViewRowBackColorDelegate DoChangeListViewRowBackColor = new ChangeListViewRowBackColorDelegate(ChangeListViewRowBackColor);
                    TargetListView.Invoke(DoChangeListViewRowBackColor, new object[] { TargetListView, TargetRow, NewRowColor });
                    }
                else 
                    {
                    TargetListView.BeginUpdate();
                    //行的索引是从0，故-1
                    TargetListView.Items[TargetRow - 1].BackColor = NewRowColor;
                    TargetListView.EndUpdate();
                    }
                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            return true;

            }

        //***********************************

        //通过代理和委托改变PictreBox控件的图片【跨线程安全】
        /// <summary>
        /// 通过代理和委托改变PictreBox控件的图片【跨线程安全】
        /// </summary>
        /// <param name="TargetPictreBox">目标PictreBox控件</param>
        /// <param name="TargetImage">新图片</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeImageOfPicture(ref PictureBox TargetPictreBox,
            System.Drawing.Image TargetImage)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library," +
                //    "please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！" +
                //    "请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作",
                //    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (TargetPictreBox.InvokeRequired == true)
                    {
                    ChangeImageOfPictureDelegate ExecuteChangeImageDelegate = new ChangeImageOfPictureDelegate(ChangeImageOfPicture);
                    TargetPictreBox.Invoke(ExecuteChangeImageDelegate, new object[] {TargetPictreBox, TargetImage});
                    return true;
                    }
                else
                    {
                    TargetPictreBox.Image = TargetImage;
                    return true;
                    }

                }
            catch (Exception)
                {
                return false;
                }

            }

        //通过代理和委托改变PictreBox控件的背景色【跨线程安全】
        /// <summary>
        /// 通过代理和委托改变PictreBox控件的背景色【跨线程安全】
        /// </summary>
        /// <param name="TargetPictreBox">目标PictreBox控件</param>
        /// <param name="NewColor">新背景色</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeBackColorOfPicture(ref PictureBox TargetPictreBox,
            Color NewColor)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library," +
                //    "please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！" +
                //    "请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作",
                //    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (TargetPictreBox.InvokeRequired == true)
                    {
                    ChangeBackColorOfPictureDelegate ExecuteChangeBackColorOfPicDelegate = new ChangeBackColorOfPictureDelegate(ChangeBackColorOfPicture);
                    TargetPictreBox.Invoke(ExecuteChangeBackColorOfPicDelegate, new object[] {TargetPictreBox, NewColor});
                    return true;
                    }
                else
                    {
                    TargetPictreBox.BackColor = NewColor;
                    return true;
                    }

                }
            catch (Exception)
                {
                return false;
                }

            }

        //? -- ok
        //通过代理和委托改变控件的背景色【跨线程安全】
        /// <summary>
        /// 通过代理和委托改变控件的背景色【跨线程安全】
        /// </summary>
        /// <param name="InvokeButton">执行Invoke[委托]的控件</param>
        /// <param name="TargetControl">目标控件</param>
        /// <param name="TargetColor">目标颜色</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeStatusLabelBackColor(ref Button InvokeButton, 
            ref System.Windows.Forms.ToolStripStatusLabel TargetControl, 
            System.Drawing.Color TargetColor)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (InvokeButton == null) 
                    {
                    return false;
                    }

                if (InvokeButton.InvokeRequired == true)
                    {
                    ChangeStatusLabelBackColorDelegate ExecuteChangeBackColorDelegate = new ChangeStatusLabelBackColorDelegate(ChangeStatusLabelBackColor);
                    InvokeButton.Invoke(ExecuteChangeBackColorDelegate, new object[] {InvokeButton, TargetControl, TargetColor });
                    return true;
                    }
                else
                    {
                    TargetControl.BackColor = TargetColor;
                    return true;
                    }

                }
            catch (Exception)
                {
                return false;
                }

            }

        ////? -- ok
        ////通过代理和委托改变控件的背景色【跨线程安全】
        ///// <summary>
        ///// 通过代理和委托改变控件的背景色【跨线程安全】
        ///// </summary>
        ///// <param name="TargetControl">目标控件</param>
        ///// <param name="TargetColor">目标颜色</param>
        //private void OldChangeStatusLabelBackColor(ref System.Windows.Forms.ToolStripStatusLabel TargetControl, System.Drawing.Color TargetColor)
        //    {
            
        //    if (PasswordIsCorrect == false)
        //        {
        //        MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return;
        //        }
            
        //    try
        //        {
                
        //        if (DelegateButton.InvokeRequired == true)
        //            {
        //            ChangeStatusLabelBackColorDelegate ExecuteChangeBackColorDelegate = new ChangeStatusLabelBackColorDelegate(ChangeStatusLabelBackColor);
        //            DelegateButton.Invoke(ExecuteChangeBackColorDelegate, new object[] { TargetControl, TargetColor });
        //            }
        //        else
        //            {
        //            TargetControl.BackColor = TargetColor;
        //            }

        //        }
        //    catch (Exception)
        //        {
        //        return;
        //        }

        //    }
        //***********************************

        //? -- ok
        //通过代理和委托改变控件的背景色【跨线程安全】
        
        /// <summary>
        /// 通过代理和委托改变控件的背景色【跨线程安全】
        /// </summary>
        /// <param name="InvokeButton">执行Invoke[委托]的控件</param>
        /// <param name="TargetControl">目标控件</param>
        /// <param name="TargetColor">目标颜色</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeMenuBackColor(ref Button InvokeButton, 
            ref System.Windows.Forms.ToolStripMenuItem TargetControl, 
            System.Drawing.Color TargetColor)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (InvokeButton == null)
                    {
                    return false;
                    }

                if (InvokeButton.InvokeRequired == true)
                    {
                    ChangeMenuBackColorDelegate ExecuteChangeBackColorDelegate = new ChangeMenuBackColorDelegate(ChangeMenuBackColor);
                    InvokeButton.Invoke(ExecuteChangeBackColorDelegate, new object[] {InvokeButton, TargetControl, TargetColor});
                    return true;
                    }
                else
                    {
                    TargetControl.BackColor = TargetColor;
                    return true;
                    }

                }
            catch (Exception)
                {
                return false;
                }

            }

        ////? -- ok
        ////通过代理和委托改变控件的背景色【跨线程安全】
        ///// <summary>
        ///// 通过代理和委托改变控件的背景色【跨线程安全】
        ///// </summary>
        ///// <param name="TargetControl">目标控件</param>
        ///// <param name="TargetColor">目标颜色</param>
        //private void OldChangeMenuBackColor(ref System.Windows.Forms.ToolStripMenuItem TargetControl, System.Drawing.Color TargetColor)
        //    {

        //    if (PasswordIsCorrect == false)
        //        {
        //        MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return;
        //        }
            
        //    try
        //        {

        //        if (DelegateButton.InvokeRequired == true)
        //            {
        //            ChangeMenuBackColorDelegate ExecuteChangeBackColorDelegate = new ChangeMenuBackColorDelegate(ChangeMenuBackColor);
        //            DelegateButton.Invoke(ExecuteChangeBackColorDelegate, new object[] { TargetControl, TargetColor });
        //            }
        //        else
        //            {
        //            TargetControl.BackColor = TargetColor;
        //            }

        //        }
        //    catch (Exception)
        //        {
        //        return;
        //        }

        //    }

        //***********************************
        //--ok
        //通过代理和委托改变菜单项控件的图片【跨线程安全】
        
        /// <summary>
        /// 通过代理和委托改变菜单项控件的图片【跨线程安全】
        /// </summary>
        /// <param name="InvokeButton">执行Invoke[委托]的控件</param>
        /// <param name="TargetControl"></param>
        /// <param name="TargetImage"></param>
        /// <returns>是否执行成功</returns>
        public bool ChangeMenuImage(ref Button InvokeButton, 
            ref System.Windows.Forms.ToolStripMenuItem TargetControl, 
            System.Drawing.Image TargetImage)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library," +
                //    "please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！" +
                //    "请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作",
                //    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (InvokeButton == null)
                    {
                    return false;
                    }

                if (InvokeButton.InvokeRequired == true)
                    {
                    ChangeMenuImageDelegate ExecuteChangeImageDelegate = new ChangeMenuImageDelegate(ChangeMenuImage);
                    InvokeButton.Invoke(ExecuteChangeImageDelegate, new object[] {InvokeButton, TargetControl, TargetImage });
                    return true;
                    }
                else
                    {
                    TargetControl.Image = TargetImage;
                    return true;
                    }

                }
            catch (Exception)
                {
                return false;
                }

            }

        ////? -- ok
        ////通过代理和委托改变菜单项控件的图片【跨线程安全】
        ///// <summary>
        ///// 通过代理和委托改变菜单项控件的图片【跨线程安全】
        ///// </summary>
        ///// <param name="TargetControl"></param>
        ///// <param name="TargetImage"></param>
        //private void OldChangeMenuImage(ref System.Windows.Forms.ToolStripMenuItem TargetControl, System.Drawing.Image TargetImage)
        //    {

        //    if (PasswordIsCorrect == false)
        //        {
        //        MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library,"+
        //            "please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！"+
        //            "请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作",
        //            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return;
        //        }
            
        //    try
        //        {
                
        //        if (DelegateButton.InvokeRequired == true)
        //            {
        //            ChangeMenuImageDelegate ExecuteChangeImageDelegate = new ChangeMenuImageDelegate(ChangeMenuImage);
        //            DelegateButton.Invoke(ExecuteChangeImageDelegate, new object[] {TargetControl, TargetImage});
        //            }
        //        else
        //            {
        //            TargetControl.Image = TargetImage;
        //            }

        //        }
        //    catch (Exception)
        //        {
        //        return;
        //        }

        //    }

        //private void New_ChangeMenuImage(ref System.Windows.Forms.ToolStripMenuItem TargetControl, System.Drawing.Image TargetImage) 
        //    {
        //    ChangeMenuImageDelegate ExecuteChangeImageDelegate = new ChangeMenuImageDelegate(ChangeMenuImage);
        //    DelegateTextbox.Invoke(ExecuteChangeImageDelegate, new object[] { TargetControl, TargetImage });//DelegateButton
            
        //    }

        //***********************************
        //通过代理和委托改变控件的背景色【跨线程安全】
        
        /// <summary>
        /// 通过代理和委托改变控件的背景色【跨线程安全】
        /// </summary>
        /// <param name="TargetControl">目标控件</param>
        /// <param name="TargetColor">目标颜色</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeButtonBackColor(ref System.Windows.Forms.Button TargetControl, System.Drawing.Color TargetColor)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (TargetControl == null)
                    {
                    return false;
                    }

                if (TargetControl.InvokeRequired == true)
                    {
                    ChangeButtonBackColorDelegate ExecuteChangeBackColorDelegate = new ChangeButtonBackColorDelegate(ChangeButtonBackColor);
                    TargetControl.Invoke(ExecuteChangeBackColorDelegate, new object[] { TargetControl, TargetColor });
                    return true;
                    }
                else
                    {
                    TargetControl.BackColor = TargetColor;
                    return true;
                    }

                }
            catch (Exception)
                {
                return false;
                }

            }

        //通过代理和委托改变目标窗体控件的Enabled属性【跨线程安全】
        /// <summary>
        /// 通过代理和委托改变目标窗体控件的Enabled属性【跨线程安全】
        /// </summary>
        /// <param name="TargetControl">目标控件</param>
        /// <param name="TargetEnabledStatus">启用：True/False</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeButtonEnableStatus(ref System.Windows.Forms.Button TargetControl, bool TargetEnabledStatus)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }
            
            try
                {

                if (TargetControl == null)
                    {
                    return false;
                    }

                if (TargetControl.InvokeRequired == true)
                    {
                    ChangeButtonEnableStatusDelegate ExecuteChangeEnableStatus = new ChangeButtonEnableStatusDelegate(ChangeButtonEnableStatus);
                    TargetControl.Invoke(ExecuteChangeEnableStatus, new object[] { TargetControl, TargetEnabledStatus });
                    return true;
                    }
                else
                    {
                    TargetControl.Enabled = TargetEnabledStatus;
                    return true;
                    }

                }
            catch (Exception)
                {
                return false;
                }

            }

        //***********************************
        //? -- ok
        //通过代理和委托改变目标窗体控件的Enabled属性【跨线程安全】
        /// <summary>
        /// 通过代理和委托改变目标窗体控件的Enabled属性【跨线程安全】
        /// </summary>
        /// <param name="InvokeButton">执行Invoke[委托]的控件</param>
        /// <param name="TargetControl">目标控件</param>
        /// <param name="TargetEnabledStatus">启用：True/False</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeMenuEnableStatus(ref Button InvokeButton, 
            ref System.Windows.Forms.ToolStripMenuItem TargetControl, 
            bool TargetEnabledStatus)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (InvokeButton == null)
                    {
                    return false;
                    }

                if (InvokeButton.InvokeRequired == true)
                    {
                    ChangeMenuEnableStatusDelegate ExecuteChangeEnableStatus = new ChangeMenuEnableStatusDelegate(ChangeMenuEnableStatus);
                    InvokeButton.Invoke(ExecuteChangeEnableStatus, new object[] {InvokeButton, TargetControl, TargetEnabledStatus});
                    return true;
                    }
                else
                    {
                    TargetControl.Enabled = TargetEnabledStatus;
                    return true;
                    }

                }
            catch (Exception)
                {
                return false;
                }

            }

        ////? -- ok
        ////通过代理和委托改变目标窗体控件的Enabled属性【跨线程安全】
        ///// <summary>
        ///// 通过代理和委托改变目标窗体控件的Enabled属性【跨线程安全】
        ///// </summary>
        ///// <param name="TargetControl">目标控件</param>
        ///// <param name="TargetEnabledStatus">启用：True/False</param>
        //private void OldChangeMenuEnableStatus(ref System.Windows.Forms.ToolStripMenuItem TargetControl, bool TargetEnabledStatus)
        //    {

        //    if (PasswordIsCorrect == false)
        //        {
        //        MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return;
        //        }
            
        //    try
        //        {

        //        if (DelegateButton.InvokeRequired == true)
        //            {
        //            ChangeMenuEnableStatusDelegate ExecuteChangeEnableStatus = new ChangeMenuEnableStatusDelegate(ChangeMenuEnableStatus);
        //            DelegateButton.Invoke(ExecuteChangeEnableStatus, new object[] { TargetControl, TargetEnabledStatus });
        //            }
        //        else
        //            {
        //            TargetControl.Enabled = TargetEnabledStatus;
        //            }

        //        }
        //    catch (Exception)
        //        {
        //        return;
        //        }

        //    }

        //***********************************
        //? -- ok
        //通过代理和委托改变目标窗体控件的Enabled属性【跨线程安全】
        
        /// <summary>
        /// 通过代理和委托改变目标窗体控件的Enabled属性【跨线程安全】
        /// </summary>
        /// <param name="InvokeButton">执行Invoke[委托]的控件</param>
        /// <param name="TargetControl">目标控件</param>
        /// <param name="TargetEnabledStatus">启用：True/False</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeStatusLabelEnableStatus(ref Button InvokeButton, 
            ref System.Windows.Forms.ToolStripStatusLabel TargetControl, 
            bool TargetEnabledStatus)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (InvokeButton == null)
                    {
                    return false;
                    }

                if (InvokeButton.InvokeRequired == true)
                    {
                    ChangeStatusLabelEnableStatusDelegate ExecuteChangeEnableStatus = new ChangeStatusLabelEnableStatusDelegate(ChangeStatusLabelEnableStatus);
                    InvokeButton.Invoke(ExecuteChangeEnableStatus, new object[] {InvokeButton, TargetControl, TargetEnabledStatus});
                    return true;
                    }
                else
                    {
                    TargetControl.Enabled = TargetEnabledStatus;
                    return true;
                    }

                }
            catch (Exception)
                {
                return false;
                }

            }

        ////? -- ok
        ////通过代理和委托改变目标窗体控件的Enabled属性【跨线程安全】
        ///// <summary>
        ///// 通过代理和委托改变目标窗体控件的Enabled属性【跨线程安全】
        ///// </summary>
        ///// <param name="TargetControl">目标控件</param>
        ///// <param name="TargetEnabledStatus">启用：True/False</param>
        //private void OldChangeStatusLabelEnableStatus(ref System.Windows.Forms.ToolStripStatusLabel TargetControl, bool TargetEnabledStatus)
        //    {

        //    if (PasswordIsCorrect == false)
        //        {
        //        MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return;
        //        }
            
        //    try
        //        {

        //        if (DelegateButton.InvokeRequired == true)
        //            {
        //            ChangeStatusLabelEnableStatusDelegate ExecuteChangeEnableStatus = new ChangeStatusLabelEnableStatusDelegate(ChangeStatusLabelEnableStatus);
        //            DelegateButton.Invoke(ExecuteChangeEnableStatus, new object[] { TargetControl, TargetEnabledStatus });
        //            }
        //        else
        //            {
        //            TargetControl.Enabled = TargetEnabledStatus;
        //            }

        //        }
        //    catch (Exception)
        //        {
        //        return;
        //        }

        //    }

        //***********************************
        //? -- ok
        //跨线程安全修改目标窗体中状态条的文字
        
        /// <summary>
        /// 跨线程安全修改目标窗体中状态条的文字
        /// </summary>
        /// <param name="InvokeButton">执行Invoke[委托]的控件</param>
        /// <param name="TargetControl">目标状态条</param>
        /// <param name="TargetText">需要变更的新文字内容</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeStatusLabelText(ref Button InvokeButton, 
            ref System.Windows.Forms.ToolStripStatusLabel TargetControl,
            string TargetText)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (InvokeButton == null)
                    {
                    return false;
                    }

                if (InvokeButton.InvokeRequired == true)
                    {
                    ChangeStatusLabelTextDelegate ExecuteChangeTextDelegate = new ChangeStatusLabelTextDelegate(ChangeStatusLabelText);
                    InvokeButton.Invoke(ExecuteChangeTextDelegate, new object[] {InvokeButton, TargetControl, TargetText});
                    return true;
                    }
                else
                    {
                    TargetControl.Text = TargetText;
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }

            }

        ////? -- ok
        ////跨线程安全修改目标窗体中状态条的文字
        ///// <summary>
        ///// 跨线程安全修改目标窗体中状态条的文字
        ///// </summary>
        ///// <param name="TargetControl">目标状态条</param>
        ///// <param name="TargetText">需要变更的新文字内容</param>
        //private void OldChangeStatusLabelText(ref System.Windows.Forms.ToolStripStatusLabel TargetControl,
        //    string TargetText)
        //    {

        //    if (PasswordIsCorrect == false)
        //        {
        //        MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return;
        //        }
            
        //    try
        //        {
        //        if (DelegateButton.InvokeRequired == true)
        //            {
        //            ChangeStatusLabelTextDelegate ExecuteChangeTextDelegate = new ChangeStatusLabelTextDelegate(ChangeStatusLabelText);
        //            DelegateButton.Invoke(ExecuteChangeTextDelegate, new object[] { TargetControl, TargetText });
        //            }
        //        else
        //            {
        //            TargetControl.Text = TargetText;
        //            }
        //        }
        //    catch (Exception)
        //        {
        //        return;
        //        }

        //    }

        //***********************************
        //跨线程安全修改目标窗体中Label控件的文字
        
        /// <summary>
        /// 跨线程安全修改目标窗体中Label控件的文字
        /// </summary>
        /// <param name="TargetLabel">目标Label控件</param>
        /// <param name="TargetText">需要变更的新文字内容</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeLabelText(ref System.Windows.Forms.Label TargetLabel,
            string TargetText)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }
            
            try
                {

                if (TargetLabel == null)
                    {
                    return false;
                    }

                if (TargetLabel.InvokeRequired == true)
                    {
                    ChangeLabelTextDelegate ExecuteChangeLabelTextDelegate = new ChangeLabelTextDelegate(ChangeLabelText);
                    TargetLabel.Invoke(ExecuteChangeLabelTextDelegate, new object[] { TargetLabel, TargetText });
                    return true;
                    }
                else
                    {
                    TargetLabel.Text = TargetText;
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }

            }
        
        //跨线程安全修改目标窗体中Label控件的位置
        /// <summary>
        /// 跨线程安全修改目标窗体中Label控件的位置
        /// </summary>
        /// <param name="TargetLabel">目标Label控件</param>
        /// <param name="Left">左侧位置</param>
        /// <param name="Top">顶部位置</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeLabelPosition(ref Label TargetLabel, 
            int Left=0, int Top=0)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }
            
            try
                {

                if (TargetLabel == null)
                    {
                    return false;
                    }

                if (TargetLabel.InvokeRequired == true)
                    {
                    ChangeLabelPositionDelegate ExecuteChangeLabelPositionDelegate 
                        = new ChangeLabelPositionDelegate(ChangeLabelPosition);
                    TargetLabel.Invoke(ExecuteChangeLabelPositionDelegate, 
                        new object[] {TargetLabel, Left,Top});
                    return true;
                    }
                else
                    {
                    TargetLabel.Left=Left;
                    TargetLabel.Top = Top;
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }

            }

        //跨线程安全修改目标窗体中Label控件的背景色
        /// <summary>
        /// 跨线程安全修改目标窗体中Label控件的背景色
        /// </summary>
        /// <param name="TargetLabel">目标Label控件</param>
        /// <param name="NewColor">新背景色</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeLabelBackColor(ref Label TargetLabel, 
            Color NewColor)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n" + 
                //    "     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n"
                //    + "你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }
            
            try
                {

                if (TargetLabel == null)
                    {
                    return false;
                    }

                if (TargetLabel.InvokeRequired == true)
                    {
                    ChangeLabelBackColorDelegate ExecuteChangeLabelBackColorDelegate
                        = new ChangeLabelBackColorDelegate(ChangeLabelBackColor);
                    TargetLabel.Invoke(ExecuteChangeLabelBackColorDelegate, 
                        new object[] {TargetLabel, NewColor});
                    return true;
                    }
                else
                    {
                    TargetLabel.BackColor = NewColor;
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }
            }
        //********************************

        //跨线程安全修改目标窗体中控件的位置
        /// <summary>
        /// 跨线程安全修改目标窗体中控件的位置
        /// 方法：先建立一个Control的实例，然后将需要变更位置的窗体控件赋值给这个新Control实例，就可以调用函数.
        /// Control xx = button1;FC.ChangeControlPosition(ref xx, x, y);
        /// </summary>
        /// <param name="TargetControl">目标窗体控件</param>
        /// <param name="Left">左侧位置</param>
        /// <param name="Top">顶部位置</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeControlPosition(ref Control TargetControl, 
            int Left=0, int Top=0)
            {
            //private delegate bool ChangePositionDelegate(ref Control TargetControl, int Left, int Top);

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }            
            try
                {
                if (TargetControl == null)
                    {
                    return false;
                    }

                if (TargetControl.InvokeRequired == true)
                    {
                    ChangePositionDelegate ExecuteChangePositionDelegate
                        = new ChangePositionDelegate(ChangeControlPosition);
                    TargetControl.Invoke(ExecuteChangePositionDelegate,
                        new object[] { TargetControl, Left, Top });
                    return true;
                    }
                else
                    {
                    TargetControl.Left = Left;
                    TargetControl.Top = Top;
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //跨线程安全修改目标窗体中控件的大小
        /// <summary>
        /// 跨线程安全修改目标窗体中控件的大小
        /// 方法：先建立一个Control的实例，然后将需要变更位置的窗体控件赋值给这个新Control实例，就可以调用函数.
        /// Control xx = button1;FC.ChangeControlSize(ref xx, x, y);
        /// </summary>
        /// <param name="TargetControl">目标窗体控件</param>
        /// <param name="Width">宽度</param>
        /// <param name="Height">高度</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeControlSize(ref Control TargetControl,
            int Width = 20, int Height = 20)
            {
            //private delegate bool ChangeSizeDelegate(ref Control TargetControl, int Width, int Height);

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }            
            try
                {
                if (TargetControl == null)
                    {
                    return false;
                    }

                if (TargetControl.InvokeRequired == true)
                    {
                    ChangeSizeDelegate ExecuteChangeSizeDelegate
                        = new ChangeSizeDelegate(ChangeControlSize);
                    TargetControl.Invoke(ExecuteChangeSizeDelegate,
                        new object[] { TargetControl, Width, Height });
                    return true;
                    }
                else
                    {
                    TargetControl.Width = Width;
                    TargetControl.Height = Height;
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }
        
        //跨线程安全修改目标窗体中控件的背景色
        /// <summary>
        /// 跨线程安全修改目标窗体中控件的背景色
        /// 方法：先建立一个Control的实例，然后将需要变更的窗体控件赋值给这个新Control实例，就可以调用函数.
        /// Control xx = button1;FC.ChangeControlBackColor(ref xx, Color.Red);
        /// </summary>
        /// <param name="TargetControl">目标窗体控件</param>
        /// <param name="TargetBackColor">新背景色</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeControlBackColor(ref Control TargetControl, Color TargetBackColor)
            {
            //private delegate bool ChangeBackColorDelegate(ref Control TargetControl, Color TargetBackColor);

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }            
            try
                {
                if (TargetControl == null)
                    {
                    return false;
                    }

                if (TargetControl.InvokeRequired == true)
                    {
                    ChangeBackColorDelegate ExecuteChangeBackColorDelegate
                        = new ChangeBackColorDelegate(ChangeControlBackColor);
                    TargetControl.Invoke(ExecuteChangeBackColorDelegate,
                        new object[] { TargetControl, TargetBackColor });
                    return true;
                    }
                else
                    {
                    TargetControl.BackColor = TargetBackColor;
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //跨线程安全修改目标窗体中控件的前景色
        /// <summary>
        /// 跨线程安全修改目标窗体中控件的前景色
        /// 方法：先建立一个Control的实例，然后将需要变更的窗体控件赋值给这个新Control实例，就可以调用函数.
        /// Control xx = button1;FC.ChangeControlForeColor(ref xx, Color.Red);
        /// </summary>
        /// <param name="TargetControl">目标窗体控件</param>
        /// <param name="TargetForeColor">新前景色</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeControlForeColor(ref Control TargetControl, Color TargetForeColor)
            {
            //private delegate bool ChangeForeColorDelegate(ref Control TargetControl, Color TargetForeColor);

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }            
            try
                {
                if (TargetControl == null)
                    {
                    return false;
                    }

                if (TargetControl.InvokeRequired == true)
                    {
                    ChangeForeColorDelegate ExecuteChangeForeColorDelegate
                        = new ChangeForeColorDelegate(ChangeControlForeColor);
                    TargetControl.Invoke(ExecuteChangeForeColorDelegate,
                        new object[] { TargetControl, TargetForeColor });
                    return true;
                    }
                else
                    {
                    TargetControl.ForeColor = TargetForeColor;
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //跨线程安全修改目标窗体中控件的Enable属性
        /// <summary>
        /// 跨线程安全修改目标窗体中控件的Enable属性
        /// 方法：先建立一个Control的实例，然后将需要变更的窗体控件赋值给这个新Control实例，就可以调用函数.
        /// Control xx = button1;FC.ChangeControlEnableStatus(ref xx, true);
        /// </summary>
        /// <param name="TargetControl">目标窗体控件</param>
        /// <param name="TargetEnableStatus">Enable属性</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeControlEnableStatus(ref Control TargetControl, bool TargetEnableStatus)
            {
            //private delegate bool ChangeEnableDelegate(ref Control TargetControl, bool TargetEnableStatus);

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }            
            try
                {
                if (TargetControl == null)
                    {
                    return false;
                    }

                if (TargetControl.InvokeRequired == true)
                    {
                    ChangeEnableDelegate ExecuteChangeEnableStatusDelegate
                        = new ChangeEnableDelegate(ChangeControlEnableStatus);
                    TargetControl.Invoke(ExecuteChangeEnableStatusDelegate,
                        new object[] { TargetControl, TargetEnableStatus });
                    return true;
                    }
                else
                    {
                    TargetControl.Enabled = TargetEnableStatus;
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //跨线程安全修改目标窗体中控件的Text属性
        /// <summary>
        /// 跨线程安全修改目标窗体中控件的Text属性
        /// 方法：先建立一个Control的实例，然后将需要变更的窗体控件赋值给这个新Control实例，就可以调用函数.
        /// Control xx = button1;FC.ChangeControlText(ref xx, "new text");
        /// </summary>
        /// <param name="TargetControl">目标窗体控件</param>
        /// <param name="TargetText">Text属性的新字符串</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeControlText(ref Control TargetControl, string TargetText)
            {
            //private delegate bool ChangeTextDelegate(ref Control TargetControl, string TargetText);

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }            
            try
                {
                if (TargetControl == null)
                    {
                    return false;
                    }

                if (TargetControl.InvokeRequired == true)
                    {
                    ChangeTextDelegate ExecuteChangeTextDelegate
                        = new ChangeTextDelegate(ChangeControlText);
                    TargetControl.Invoke(ExecuteChangeTextDelegate,
                        new object[] { TargetControl, TargetText });
                    return true;
                    }
                else
                    {
                    TargetControl.Text = TargetText;
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //跨线程安全修改目标窗体中控件的背景图片
        /// <summary>
        /// 跨线程安全修改目标窗体中控件的背景图片
        /// 方法：先建立一个Control的实例，然后将需要变更的窗体控件赋值给这个新Control实例，就可以调用函数.
        /// Control xx = button1;FC.ChangeControlBackgroundImage(ref xx, 图片变量);
        /// </summary>
        /// <param name="TargetControl">目标窗体控件</param>
        /// <param name="TargetBackgroundImage">新背景图片</param>
        /// <returns>是否执行成功</returns>
        public bool ChangeControlBackgroundImage(ref Control TargetControl, Image TargetBackgroundImage)
            {
            //private delegate bool ChangeBackgroundImageDelegate(ref Control TargetControl, Image TargetBackgroundImage);

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }            
            try
                {
                if (TargetControl == null || TargetBackgroundImage == null)
                    {
                    return false;
                    }

                if (TargetControl.InvokeRequired == true)
                    {
                    ChangeBackgroundImageDelegate ExecuteChangeBackgroundImageDelegate
                        = new ChangeBackgroundImageDelegate(ChangeControlBackgroundImage);
                    TargetControl.Invoke(ExecuteChangeBackgroundImageDelegate,
                        new object[] { TargetControl, TargetBackgroundImage });
                    return true;
                    }
                else
                    {
                    TargetControl.BackgroundImage = TargetBackgroundImage;
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }
        
        //********************************

        //通过委托和代理清除文本控件中的内容，在跨线程操作时保证线程安全
        /// <summary>
        /// 通过委托和代理清除文本控件中的内容，在跨线程操作时保证线程安全
        /// </summary>
        /// <param name="TargetRichTextBox"></param>
        /// <returns>是否执行成功</returns>
        public bool ClearRichTextBoxContents(ref System.Windows.Forms.RichTextBox TargetRichTextBox)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (TargetRichTextBox == null)
                    {
                    return false;
                    }

                if (TargetRichTextBox.InvokeRequired == true)
                    {
                    ClearRichTextBoxDelegate TempClear = new ClearRichTextBoxDelegate(ClearRichTextBoxContents);
                    TargetRichTextBox.Invoke(TempClear, new object[] { TargetRichTextBox });
                    return true;
                    }
                else
                    {
                    TargetRichTextBox.Text = "";
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }

            }

        //通过委托和代理清除文本控件中的内容，在跨线程操作时保证线程安全
        /// <summary>
        /// 通过委托和代理清除文本控件中的内容，在跨线程操作时保证线程安全
        /// </summary>
        /// <param name="TargetTextBox"></param>
        /// <returns>是否执行成功</returns>
        public bool ClearTextBoxContents(ref System.Windows.Forms.TextBox TargetTextBox)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            if (TargetTextBox == null)
                {
                return false;
                }

            if (TargetTextBox.InvokeRequired == true)
                {
                ClearTextBoxDelegate TempClear = new ClearTextBoxDelegate(ClearTextBoxContents);
                TargetTextBox.Invoke(TempClear, new object[] { TargetTextBox });
                return true;
                }
            else
                {
                TargetTextBox.Text = "";
                return true;
                }

            }

        //利用委托和代理进行跨线程操作富文本控件添加内容 [含信息发生的日期和时间]，以此避免跨线程操作异常
        /// <summary>
        /// 利用委托和代理进行跨线程操作富文本控件添加内容 [含信息发生的日期和时间]，以此避免跨线程操作异常
        /// </summary>
        /// <param name="TargetRichTextBox">需要添加内容的文本控件</param>
        /// <param name="TargetText">要添加的文本内容</param>
        /// <returns>是否执行成功</returns>
        public bool AddRichText(ref System.Windows.Forms.RichTextBox TargetRichTextBox,
            string TargetText)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (TargetRichTextBox == null)
                    {
                    return false;
                    }

                if (TargetText == "")
                    {
                    return false;
                    }

                string TempStr = "";// Strings.UCase(TargetText);
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
                if (TargetRichTextBox.InvokeRequired == true)
                    {
                    AddMessageToRichTextBox ActualAddMessageToRichTextBox = new AddMessageToRichTextBox(AddRichText);
                    TargetRichTextBox.Invoke(ActualAddMessageToRichTextBox, new object[] { TargetRichTextBox, TempStr });
                    }
                else
                    {

                    if (UpdatingSameMessage == true)
                        {
                        TempErrorMessage = TempStr;
                        //TargetRichTextBox.AppendText(DateTime.Now + "   " + TempStr + "\r\n");
                        //if (ShowDateTimeForMessage == true)
                        //    {
                        TargetRichTextBox.AppendText(DateTime.Now + "  " + DateTime.Now.Millisecond + "   " + TempStr + "\r\n");
                        //    }
                        //else
                        //    {
                        //TargetRichTextBox.AppendText(TempStr + "\r\n");
                            //}
                        TempStr = "";
                        }
                    else
                        {

                        if (TempErrorMessage == TempStr)
                            {
                            return false;
                            }
                        else
                            {
                            TempErrorMessage = TempStr;
                            //TargetRichTextBox.AppendText(DateTime.Now + "   " + TempStr + "\r\n");
                            //if (ShowDateTimeForMessage == true)
                            //    {
                            TargetRichTextBox.AppendText(DateTime.Now + "  " + DateTime.Now.Millisecond + "   " + TempStr + "\r\n");
                            //    }
                            //else
                            //    {
                            //TargetRichTextBox.AppendText(TempStr + "\r\n");
                                //}
                            TempStr = "";
                            }

                        }

                    }
                TargetText = "";
                return true;
                //**********************

                }
            catch (Exception)// ex)
                {
                //MessageBox.Show(ex.Message);
                return false;
                }

            }

        //利用委托和代理进行跨线程操作文本控件添加内容 [含信息发生的日期和时间]，以此避免跨线程操作异常
        /// <summary>
        /// 利用委托和代理进行跨线程操作文本控件添加内容 [含信息发生的日期和时间]，以此避免跨线程操作异常
        /// </summary>
        /// <param name="TargetTextBox">需要添加内容的文本控件</param>
        /// <param name="TargetText">要添加的文本内容</param>
        /// <returns>是否执行成功</returns>
        public bool AddTextBox(ref System.Windows.Forms.TextBox TargetTextBox, string TargetText)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (TargetTextBox == null)
                    {
                    return false;
                    }

                if (TargetText == "") return false;

                string TempStr = "";// Strings.UCase(TargetText);
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

                TargetText = "";

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
                if (TargetTextBox.InvokeRequired == true)
                    {
                    AddMessageToTextBox ActualAddMessageToTextBox = new AddMessageToTextBox(AddTextBox);
                    TargetTextBox.Invoke(ActualAddMessageToTextBox, new object[] { TargetTextBox, TempStr });
                    }
                else
                    {

                    if (UpdatingSameMessage == true)
                        {
                        TempErrorMessage = TempStr;
                        //TargetTextBox.AppendText(DateTime.Now + "   " + TempStr + "\r\n");
                        //if (ShowDateTimeForMessage == true)
                        //    {
                        TargetTextBox.AppendText(DateTime.Now + "  " + DateTime.Now.Millisecond + "   " + TempStr + "\r\n");
                        //    }
                        //else
                        //    {
                        //TargetTextBox.AppendText(TempStr + "\r\n");
                            //}
                        TempStr = "";
                        }
                    else
                        {

                        if (TempErrorMessage == TempStr)
                            {
                            return false;
                            }
                        else
                            {
                            TempErrorMessage = TempStr;
                            //TargetTextBox.AppendText(DateTime.Now + "   " + TempStr + "\r\n");
                            //if (ShowDateTimeForMessage == true)
                            //    {
                            TargetTextBox.AppendText(DateTime.Now + "  " + DateTime.Now.Millisecond + "   " + TempStr + "\r\n");
                            //    }
                            //else
                            //    {
                            //TargetTextBox.AppendText(TempStr + "\r\n");
                                //}
                            TempStr = "";
                            }

                        }

                    }
                return true;
                //**********************

                }
            catch (Exception)
                {
                return false;
                }

            }

        //利用委托和代理进行跨线程操作文本控件添加内容 [不包括发生的日期和时间]，以此避免跨线程操作异常
        /// <summary>
        /// 利用委托和代理进行跨线程操作文本控件添加内容 [不包括发生的日期和时间]，以此避免跨线程操作异常
        /// </summary>
        /// <param name="TargetRichTextBox">需要添加内容的文本控件</param>
        /// <param name="TargetText">要添加的文本内容</param>
        /// <returns>是否执行成功</returns>
        public bool AddRichTextWithoutTimeMark(ref System.Windows.Forms.RichTextBox TargetRichTextBox,
            string TargetText)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (TargetRichTextBox == null)
                    {
                    return false;
                    }

                if (TargetText == "") return false;

                string TempStr = "";// Strings.UCase(TargetText);
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
                TargetText = "";

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
                if (TargetRichTextBox.InvokeRequired == true)
                    {
                    AddMessageToRichTextBox ActualAddMessageToRichTextBox = new AddMessageToRichTextBox(AddRichTextWithoutTimeMark);
                    TargetRichTextBox.Invoke(ActualAddMessageToRichTextBox, new object[] { TargetRichTextBox, TempStr });
                    }
                else
                    {

                    if (UpdatingSameMessage == true)
                        {
                        TempErrorMessage = TempStr;
                        //TargetRichTextBox.AppendText(TempStr + "\r\n");
                        TargetRichTextBox.AppendText(TempStr + "\r\n");
                        TempStr = "";
                        }
                    else
                        {

                        if (TempErrorMessage == TempStr)
                            {
                            return false;
                            }
                        else
                            {
                            TempErrorMessage = TempStr;
                            //TargetRichTextBox.AppendText(TempStr + "\r\n");
                            TargetRichTextBox.AppendText(TempStr + "\r\n");                             
                            TempStr = "";
                            }

                        }

                    }
                return true;
                //**********************

                }
            catch (Exception)
                {
                return false;
                }

            }

        //利用委托和代理进行跨线程操作文本控件添加内容 [不包括发生的日期和时间]，以此避免跨线程操作异常
        /// <summary>
        /// 利用委托和代理进行跨线程操作文本控件添加内容 [不包括发生的日期和时间]，以此避免跨线程操作异常
        /// </summary>
        /// <param name="TargetTextBox">需要添加内容的文本控件</param>
        /// <param name="TargetText">要添加的文本内容</param>
        /// <returns>是否执行成功</returns>
        public bool AddTextWithoutTimeMark(ref System.Windows.Forms.TextBox TargetTextBox,
            string TargetText)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (TargetTextBox == null)
                    {
                    return false;
                    }

                if (TargetText == "") return false;

                string TempStr = "";// Strings.UCase(TargetText);
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
                TargetText = "";

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
                if (TargetTextBox.InvokeRequired == true)
                    {
                    AddMessageToTextBox ActualAddMessageToTextBox = new AddMessageToTextBox(AddTextWithoutTimeMark);
                    TargetTextBox.Invoke(ActualAddMessageToTextBox, new object[] { TargetTextBox, TempStr });
                    }
                else
                    {

                    if (UpdatingSameMessage == true)
                        {
                        TempErrorMessage = TempStr;
                        //TargetTextBox.AppendText(TempStr + "\r\n");
                        TargetTextBox.AppendText(TempStr + "\r\n");                          
                        TempStr = "";
                        }
                    else
                        {

                        if (TempErrorMessage == TempStr)
                            {
                            return false;
                            }
                        else
                            {
                            TempErrorMessage = TempStr;
                            //TargetTextBox.AppendText(TempStr + "\r\n");   
                            TargetTextBox.AppendText(TempStr + "\r\n");                               
                            TempStr = "";
                            }

                        }

                    }
                return true;
                //**********************

                }
            catch (Exception)
                {
                return false;
                }

            }

        //利用委托和代理进行跨线程用新文本替换RichTextBox控件的原有内容 [不包括发生的日期和时间]，以此避免跨线程操作异常
        /// <summary>
        /// 利用委托和代理进行跨线程用新文本替换RichTextBox控件的原有内容 [不包括发生的日期和时间]，以此避免跨线程操作异常
        /// </summary>
        /// <param name="TargetRichTextBox">需要替换内容的RichTextBox文本控件</param>
        /// <param name="TargetText">要替换的新文本内容</param>
        /// <returns>是否执行成功</returns>
        public bool UpdateRichTextBox(ref System.Windows.Forms.RichTextBox TargetRichTextBox,
            string TargetText)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (TargetRichTextBox == null)
                    {
                    return false;
                    }

                if (TargetRichTextBox.InvokeRequired == true)
                    {
                    UpdateRichTextBoxDelegate ActualUpdateTextBox = new UpdateRichTextBoxDelegate(UpdateRichTextBox);
                    TargetRichTextBox.Invoke(ActualUpdateTextBox, new object[] { TargetRichTextBox, TargetText });
                    return true;
                    }
                else
                    {
                    TargetRichTextBox.Text = TargetText;
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }

            }

        //利用委托和代理进行跨线程用新文本替换TextBox文本控件的原有内容 [不包括发生的日期和时间]，以此避免跨线程操作异常
        /// <summary>
        ///  利用委托和代理进行跨线程用新文本替换TextBox文本控件的原有内容[不包括发生的日期和时间]，以此避免跨线程操作异常
        /// </summary>
        /// <param name="TargetTextBox">需要替换内容的TextBox文本控件</param>
        /// <param name="TargetText">要替换的新文本内容</param>
        /// <returns>是否执行成功</returns>
        public bool UpdateTextBox(ref System.Windows.Forms.TextBox TargetTextBox,
            string TargetText)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (TargetTextBox == null)
                    {
                    return false;
                    }

                if (TargetTextBox.InvokeRequired == true)
                    {
                    UpdateTextBoxDelegate ActualUpdateTextBox = new UpdateTextBoxDelegate(UpdateTextBox);
                    TargetTextBox.Invoke(ActualUpdateTextBox, new object[] { TargetTextBox, TargetText });
                    return true;
                    }
                else
                    {
                    TargetTextBox.Text = TargetText;
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }

            }

        //检查是否通过了权限认证
        /// <summary>
        /// 检查是否通过了权限认证
        /// </summary>
        /// <returns></returns>
        private bool VerifyAuthorization() 
            {

            if (PasswordIsCorrect == false)
                {
                MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library," +
                    "please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！" +
                    "请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }
            else 
                {
                return true;
                }
            
            }

        #endregion

        #region "通用TCP/IP函数"

        //添加网络IP地址变更和网络断开、连接的监听句柄
        /// <summary>
        /// 添加网络IP地址变更和网络断开、连接的监听句柄
        /// </summary>
        private void AddNetworkChange_EventHandler()
            {
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(AddressChangedCallback);
            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkAvailabilityChangedCallback);
            }

        //网络IP地址变更事件的代理
        /// <summary>
        /// 网络IP地址变更事件的代理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddressChangedCallback(object sender, EventArgs e)
            {
            //AddText("网络中有IP地址的变更...");
            }

        //网络断开、连接事件的代理
        /// <summary>
        /// 网络断开、连接事件的代理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NetworkAvailabilityChangedCallback(object sender,
            NetworkAvailabilityEventArgs e)
            {

            //    Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
            //    Dim n As NetworkInterface

            //    NetworkInterface.OperationalStatus(属性)
            //    获取网络连接的当前操作状态。
            //    Public MustOverride ReadOnly Property OperationalStatus As OperationalStatus

            //    OperationalStatus 枚举
            //    Up	            网络接口已运行，可以传输数据包。
            //    Down	        网络接口无法传输数据包。
            //    Testing	    网络接口正在运行测试。
            //    Unknown	    网络接口的状态未知。
            //    Dormant	    网络接口不处于传输数据包的状态；它正等待外部事件。
            //    NotPresent	    由于缺少组件（通常为硬件组件），网络接口无法传输数据包。
            //    LowerLayerDown	网络接口无法传输数据包，因为它运行在一个或多个其他接口之上，而这些“低层”接口中至少有一个已关闭。

            if (e.IsAvailable == false)
                {
                NetworkConnected = false;
                }
            else
                {
                NetworkConnected = true;
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
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
                }

            try
                {

                if (TargetPort < 0 | TargetPort > 65535)
                    {
                    //MessageBox.Show("The port value for the Server is not correct, please retry.\r\n你输入的IP地址无效，正确值得范围为0~65535，请修改后再重试.");
                    ErrorMessage = "The port value for the Server is not correct, please retry.\r\n你输入的IP地址无效，正确值得范围为0~65535，请修改后再重试.";
                    return false;
                    }
                else
                    {
                    return true;
                    }

                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
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
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ErrorMessage = "You don't have the given right to use this DLL library";
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
                    //MessageBox.Show("The format of IP address is not correct, please retry.\r\nIP地址格式不正确， 请修改参数.");
                    ErrorMessage = "The format of IP address is not correct, please retry.\r\nIP地址格式不正确， 请修改参数.";
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
                            //MessageBox.Show("The IP address you input is invalid, the value is 0~255, please revise and retry.\r\n你输入的IP地址无效，正确值得范围为0~255，请修改后再重试.");
                            ErrorMessage = "The IP address you input is invalid, the value is 0~255, please revise and retry.\r\n你输入的IP地址无效，正确值得范围为0~255，请修改后再重试.";
                            return null;
                            }

                        }

                    }

                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }

            string TempStr = Convert.ToString(TempGetIPAddress[0]) + "." + Convert.ToString(TempGetIPAddress[1]) + "." + Convert.ToString(TempGetIPAddress[2]) + "." + Convert.ToString(TempGetIPAddress[3]);
            return IPAddress.Parse(TempStr);

            //return IPAddress.Parse(Convert.ToString(TempGetIPAddress[0]) + "." + Convert.ToString(TempGetIPAddress[1]) + "." + Convert.ToString(TempGetIPAddress[2]) + "." + Convert.ToString(TempGetIPAddress[3]));

            }

        //验证IP地址的有效性
        /// <summary>
        /// 验证IP地址的有效性
        /// </summary>
        /// <param name="TargetIPAddress">待验证IP地址字符串</param>
        /// <returns></returns>
        public bool VerifyIPAddressNew(string TargetIPAddress)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ErrorMessage = "You don't have the given right to use this DLL library";
                return false;
                }

            IPAddress TempIPAddress = IPAddress.Parse("127.0.0.1");
            string[] GetCorrectIPAddress = new string[4];
            Int16[] TempGetIPAddress = new Int16[4];

            try
                {

                //1、判断输入的服务器IP地址是否正确
                if (IPAddress.TryParse(TargetIPAddress, out TempIPAddress) == false)
                    {
                    //MessageBox.Show("The format of IP address is not correct, please retry.\r\nIP地址格式不正确， 请修改参数.");
                    ErrorMessage = "The format of IP address is not correct, please retry.";
                    return false;
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
                            //MessageBox.Show("The IP address you input is invalid, the value is 0~255, please revise and retry.\r\n你输入的IP地址无效，正确值得范围为0~255，请修改后再重试.");
                            ErrorMessage = "The IP address you input is invalid, the value is 0~255, please revise and retry";
                            return false;
                            }

                        }

                    }

                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }

            return true;

            }

        //验证IP地址的有效性，如果验证成功则返回正确IP地址
        /// <summary>
        /// 验证IP地址的有效性，如果验证成功则返回正确IP地址
        /// </summary>
        /// <param name="TargetIPAddress">待验证IP地址字符串</param>
        /// <param name="GetBackIPAddress">返回正确的IP地址</param>
        /// <returns></returns>
        public bool VerifyIPAddress(string TargetIPAddress, out IPAddress GetBackIPAddress)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                GetBackIPAddress = null;
                return false;
                }

            IPAddress TempIPAddress = IPAddress.Parse("127.0.0.1");
            string[] GetCorrectIPAddress = new string[4];
            Int16[] TempGetIPAddress = new Int16[4];

            try
                {

                //1、判断输入的服务器IP地址是否正确
                if (IPAddress.TryParse(TargetIPAddress, out TempIPAddress) == false)
                    {
                    //MessageBox.Show("The format of IP address is not correct, please retry.\r\nIP地址格式不正确， 请修改参数.");
                    ErrorMessage = "The format of IP address is not correct, please retry.";
                    GetBackIPAddress = null;
                    return false;
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
                            //MessageBox.Show("The IP address you input is invalid, the value is 0~255, please revise and retry.\r\n你输入的IP地址无效，正确值得范围为0~255，请修改后再重试.");
                            ErrorMessage = "The IP address you input is invalid, the value is 0~255, please revise and retry.";
                            GetBackIPAddress = null;
                            return false;
                            }

                        }

                    }

                string TempStr = Convert.ToString(TempGetIPAddress[0]) + "." + Convert.ToString(TempGetIPAddress[1]) + "." + Convert.ToString(TempGetIPAddress[2]) + "." + Convert.ToString(TempGetIPAddress[3]);
                GetBackIPAddress = IPAddress.Parse(TempStr);

                }
            catch (Exception ex)
                {
                GetBackIPAddress = null;
                ErrorMessage = ex.Message;
                return false;
                }

            return true;

            }

        //验证IP地址和端口的有效性
        /// <summary>
        /// 验证IP地址和端口的有效性
        /// </summary>
        /// <param name="TargetIPAddress">目标IP地址字符串</param>
        /// <param name="TargetPort">目标端口</param>
        /// <returns>是否验证成功</returns>
        public bool VerifyIPAddressAndPort(string TargetIPAddress, ushort TargetPort)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ErrorMessage = "You don't have the given right to use this DLL library";
                return false;
                }

            IPAddress TempIPAddress = IPAddress.Parse("127.0.0.1");
            string[] GetCorrectIPAddress = new string[4];
            Int16[] TempGetIPAddress = new Int16[4];
            bool IPAddressCorrect, PortCorrect;

            IPAddressCorrect = false;
            PortCorrect = false;

            try
                {

                //1、判断输入的服务器IP地址是否正确
                if (IPAddress.TryParse(TargetIPAddress, out TempIPAddress) == false)
                    {
                    //MessageBox.Show("The format of IP address is not correct, please retry.\r\nIP地址格式不正确， 请修改参数.");
                    ErrorMessage = "The format of IP address is not correct, please retry.";
                    //TargetIPAddress = "";
                    IPAddressCorrect = false;
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
                            //MessageBox.Show("The IP address you input is invalid, the value is 0~255, please revise and retry.\r\n你输入的IP地址无效，正确值得范围为0~255，请修改后再重试.");
                            ErrorMessage = "The IP address you input is invalid, the value is 0~255, please revise and retry.";
                            //TargetIPAddress = "";
                            IPAddressCorrect = false;
                            }

                        }

                    }

                IPAddressCorrect = true;

                if (TargetPort < 0 | TargetPort > 65535)
                    {
                    //MessageBox.Show("The port value for the Server is not correct, please retry.\r\n你输入的IP地址无效，正确值得范围为0~65535，请修改后再重试.");
                    ErrorMessage = "The port value for the Server is not correct, please retry.";
                    PortCorrect = false;
                    }
                else
                    {
                    PortCorrect = true;
                    }

                }
            catch (Exception ex)
                {
                //TargetIPAddress = "";
                ErrorMessage = ex.Message;
                return false;
                }

            if (IPAddressCorrect == false | PortCorrect == false)
                {
                return false;
                }
            else if (IPAddressCorrect == true & PortCorrect == true)
                return true;
            else
                return false;

            }

        //验证IP地址和端口的有效性
        /// <summary>
        /// 验证IP地址和端口的有效性
        /// </summary>
        /// <param name="TargetIPAddress">目标IP地址字符串</param>
        /// <param name="TargetPort">目标端口</param>
        /// <param name="GetBackIPAddress">返回正确的IP地址</param>
        /// <returns></returns>
        public bool VerifyIPAddressAndPort(string TargetIPAddress, ushort TargetPort,
            out IPAddress GetBackIPAddress)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ErrorMessage = "You don't have the given right to use this DLL library";
                GetBackIPAddress = null;
                return false;
                }

            IPAddress TempIPAddress = IPAddress.Parse("127.0.0.1");
            string[] GetCorrectIPAddress = new string[4];
            Int16[] TempGetIPAddress = new Int16[4];
            bool IPAddressCorrect, PortCorrect;

            IPAddressCorrect = false;
            PortCorrect = false;

            try
                {

                //1、判断输入的服务器IP地址是否正确
                if (IPAddress.TryParse(TargetIPAddress, out TempIPAddress) == false)
                    {
                    //MessageBox.Show("The format of IP address is not correct, please retry.\r\nIP地址格式不正确， 请修改参数.");
                    ErrorMessage = "The format of IP address is not correct, please retry.";
                    GetBackIPAddress = null;
                    IPAddressCorrect = false;
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
                            //MessageBox.Show("The IP address you input is invalid, the value is 0~255, please revise and retry.\r\n你输入的IP地址无效，正确值得范围为0~255，请修改后再重试.");
                            ErrorMessage = "The IP address you input is invalid, the value is 0~255, please revise and retry.";
                            GetBackIPAddress = null;
                            IPAddressCorrect = false;
                            }

                        }

                    }

                IPAddressCorrect = true;

                if (TargetPort < 0 | TargetPort > 65535)
                    {
                    //MessageBox.Show("The port value for the Server is not correct, please retry.\r\n你输入的IP地址无效，正确值得范围为0~65535，请修改后再重试.");
                    ErrorMessage = "The port value for the Server is not correct, please retry.";
                    PortCorrect = false;
                    }
                else
                    {
                    PortCorrect = true;
                    }

                }
            catch (Exception)
                {
                GetBackIPAddress = null;
                return false;
                }

            if (IPAddressCorrect == false)
                {
                GetBackIPAddress = null;
                return false;
                }
            else if (IPAddressCorrect == true & PortCorrect == true)
                {
                string TempStr = Convert.ToString(TempGetIPAddress[0]) + "." + Convert.ToString(TempGetIPAddress[1]) + "." + Convert.ToString(TempGetIPAddress[2]) + "." + Convert.ToString(TempGetIPAddress[3]);
                GetBackIPAddress = IPAddress.Parse(TempStr);
                return true;
                }
            else
                {
                GetBackIPAddress = null;
                return false;
                }

            }

        // 获取TCP/IP网络中所有可用的IP地址
        /// <summary>
        /// 获取TCP/IP网络中所有可用的IP地址
        /// </summary>
        /// <returns>返回TCP/IP网络中所有可用IP地址的字符串数组</returns>
        public string[] SearchAvailableIPAddresses()
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ErrorMessage = "You don't have the given right to use this DLL library";
                return null;
                }

            string InstructionsForSearchingIPAddress = "";
            bool ChineseLanguage = false;

            //定义变量存储文件名
            string BATFileNameForGetIPAddress = "C:\\Windows\\Temp\\SearchIPAddress.bat";
            string TXTFileNameForSaveIPAddress = "C:\\Windows\\Temp\\GotIPAddress.txt";

            //Int16 iMaxCountOfComputers = 100;
            string[] sAllIPAddress;  //存储网络中实际存在的IP地址
            //Int16 iWaitTimeCount;    //定义最大等待时间，超时就退出扫描等待

            //定义类用于操作计算机相关信息，类似于VB .NET中的my.computer
            Microsoft.VisualBasic.Devices.Computer pcFileSystem = new Microsoft.VisualBasic.Devices.Computer();

            try
                {

                //System.Net.NetworkInformation.TcpConnectionInformation xx;
                Microsoft.VisualBasic.Devices.Network TempNetwork = new Microsoft.VisualBasic.Devices.Network();

                //bool TempBool;
                //TempBool = TempNetwork.IsAvailable;

                if (TempNetwork.IsAvailable == false)
                    {
                    //MessageBox.Show("This compueter has not connected to the network yet, please check the reason and retry.\r\n计算机未连接到网络，请检查原因后再尝试搜索.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ErrorMessage = "This compueter has not connected to the network yet, please check the reason and retry.";
                    return null;
                    }

                //1、清除目录下的原有BAT/TXT文件记录【如果存在的话】
                if (pcFileSystem.FileSystem.FileExists(BATFileNameForGetIPAddress) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(BATFileNameForGetIPAddress);
                    }

                if (pcFileSystem.FileSystem.FileExists(TXTFileNameForSaveIPAddress) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(TXTFileNameForSaveIPAddress);
                    }

                //*****************************
                //以下是通过批处理命令arp -a得到的结果：
                //
                //接口: 192.168.0.125 --- 0x3
                //  Internet 地址         物理地址              类型
                //  192.168.0.1           8c-a6-df-7e-81-fa     动态        
                //  192.168.0.101         74-04-2b-d3-db-1e     动态        
                //  192.168.0.124         fc-d7-33-3a-5d-cc     动态        
                //  192.168.0.255         ff-ff-ff-ff-ff-ff     静态        
                //  224.0.0.2             01-00-5e-00-00-02     静态        
                //  224.0.0.22            01-00-5e-00-00-16     静态        
                //  224.0.0.251           01-00-5e-00-00-fb     静态        
                //  224.0.0.252           01-00-5e-00-00-fc     静态        
                //255.255.255.255       ff-ff-ff-ff-ff-ff     静态
                //以上总共3列，Tokens=1得到IP地址这一列，Tokens=2得到物理地址这一列，Tokens=3得到类型这一列，而且上面的内容是以空格为分隔符；

                //**********************************
                //Tokens用于获取第n列的内容；
                //delims用于定义分隔符；
                //skip=n 忽略文本开头的前n行；
                //eol=c  忽略以某字符开头的行；
                //**********************************

                //2、组织BAT命令
                //将DOS的批处理命令写入文件BAT中

                //>：批处理命令中的>是将内容导出到某文件或其它目标，并进行覆盖，比如：ECHO Done>C:\Example.txt,即将文字Done写入文件C:\Example.txt，如果原来存在此文件，则会覆盖
                //>>：同样是导出内容，但是执行的是添加的动作，如果原来存在某文件或目标，则会在最后添加相应内容

                //方法二：在批处理文件的最后一句添加一条删除自身的语句，执行后就扫描此文件是否存在，如果不存在就读取保存的txt文件进行IP地址读取处理；

                if (ChineseLanguage == true)
                    {

                    InstructionsForSearchingIPAddress = "\r\nchcp 936\r\nCOLOR 0A\r\nCLS\r\n@ECHO Off\r\nfor /f \"skip=3 tokens=1,* delims= \" %%i in ('arp -a') do ECHO %%i>> " + TXTFileNameForSaveIPAddress + "\r\nECHO.\r\ndel " + BATFileNameForGetIPAddress;

                    //COLOR 0A
                    //'CLS
                    //'@ECHO Off
                    //'Title 查询局域网内在线电脑IP
                    //':send
                    //'@ECHO off&setlocal enabledelayedexpansion
                    //'ECHO 正在获取本机的IP地址，请稍等... 
                    //'for /f "tokens=2 skip=1 delims=: " %%i in ('nbtstat -n') do ( 
                    //'set "IP=%%i" 
                    //'set IP=!IP:~1,-1! 
                    //'ECHO 本机IP为：!IP! 
                    //'goto :next 
                    //')
                    //':next 
                    //'for /f "delims=. tokens=1,2,3,4,5" %%i in ("%IP%") do setrange=%%i.%%j.%%k 
                    //'ECHO.&ECHO 正在获取本网段内的其它在线计算机名，请稍等...
                    //'ECHO 本网段【%range%.*】内的计算机有： 
                    //'for /f "delims=" %%i in ('net view') do ( 
                    //'set "var=%%i" 
                    //'::查询在线计算机名称 
                    //'if "!var:~0,2!"=="\\" ( 
                    //'set "var=!var:~2!" 
                    //'ECHO !var! 
                    //'ping -n 1 !var!>nul ECHO !var!
                    //')) 
                    //'ECHO.
                    //'ECHO 正在获取本网段内的其它在线计算机IP，请稍等... 
                    //'arp -a
                    //'for /f "skip=1 tokens=1,* delims= " %%i in ('arp -a') do ECHO IP： %%i 正在使用
                    //'ECHO.
                    //'ECHO 查询完毕，按任意键退出...
                    //'pause>nul

                    }
                else
                    {
                    //转换为简体中文，才能够正确显示绿色字体，否则白色字体
                    InstructionsForSearchingIPAddress = "\r\nchcp 936\r\nCOLOR 0A\r\nCLS\r\n@ECHO Off\r\nfor /f \"skip=3 tokens=1,* delims= \" %%i in ('arp -a') do ECHO %%i>> " + TXTFileNameForSaveIPAddress + "\r\nECHO.\r\ndel " + BATFileNameForGetIPAddress;

                    }

                //3、写入BAT命令至文件
                //保存的中文会乱码，不支持GB码
                //pcFileSystem.FileSystem.WriteAllText(BATFileNameForGetIPAddress, InstructionsForSearchingIPAddress, false, System.Text.Encoding.ASCII) //System.Text.Encoding.GetEncoding(936));

                //保存的中文不会乱码，但是在DOS环境下会乱码
                pcFileSystem.FileSystem.WriteAllText(BATFileNameForGetIPAddress, InstructionsForSearchingIPAddress, false);

                //4、执行BAT命令【隐藏DOS命令窗口】
                Microsoft.VisualBasic.Interaction.Shell(BATFileNameForGetIPAddress, Microsoft.VisualBasic.AppWinStyle.Hide);
                //Microsoft.VisualBasic.Interaction.Shell(BATFileNameForGetIPAddress, Microsoft.VisualBasic.AppWinStyle.Hide,true,2000);

                //5、等待BAT命令完成
                //方法一：读取指定文件，并判断完成标志

                //方法二：在批处理文件的最后一句添加一条删除自身的语句，执行后就扫描此文件是否存在，如果不存在就读取保存的txt文件进行IP地址读取处理；
                Int16 iOverTimeCounter = 0;

                do
                    {

                    Thread.Sleep(1);
                    System.Windows.Forms.Application.DoEvents();
                    iOverTimeCounter += 1;
                    if (iOverTimeCounter >= 5000)
                        {

                        if (MessageBox.Show("It took a bit long time to search but haven't finished yet, are you sure to continue?\r\n已经花了比较长的时间进行搜索，但还没有完成，还要继续吗？",
                            "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                            {
                            iOverTimeCounter = 0;
                            }
                        else
                            {
                            return null;
                            }

                        }

                    } while (pcFileSystem.FileSystem.FileExists(BATFileNameForGetIPAddress) == true);

                //6、判断是否已经联网，并处理没有网络就退出函数
                //如果记录IP地址的TXT文件不存在，则证明电脑没有和其它网络相连

                if (pcFileSystem.FileSystem.FileExists(TXTFileNameForSaveIPAddress) == false)
                    {
                    //MessageBox.Show("Maybe you don't run this program with the Administrator right, or there was an system error. Please check the reason and retry.\r\n你没有以管理员身份运行此程序或者已经引发了系统错误，请检查是否开启网络发现和其他原因后再尝试.",
                    //    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ErrorMessage = "Maybe you don't run this program with the Administrator right, or there was an system error. Please check the reason and retry.";
                    return null;
                    }

                //7、从文本文件读取记录进行处理
                //用File.ReadAllLines方法读取文件会执行关闭动作，会导致DOS BAT文件执行写入时出现异常

                //读取存储IP地址的文件
                sAllIPAddress = System.IO.File.ReadAllLines(TXTFileNameForSaveIPAddress);

                //8、清除目录下的原有BAT/TXT文件记录【如果存在的话】
                if (pcFileSystem.FileSystem.FileExists(BATFileNameForGetIPAddress) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(BATFileNameForGetIPAddress);
                    }

                if (pcFileSystem.FileSystem.FileExists(TXTFileNameForSaveIPAddress) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(TXTFileNameForSaveIPAddress);
                    }

                //9、执行BAT文件时可能会出现权限不够无法执行或者执行中遇到错误的情况，导致保存执行结果的TXT文件为空，在此进行判断。

                if (sAllIPAddress.Length == 0)
                    {
                    //MessageBox.Show("Maybe you don't run this program with the Administrator right, or there was an system error. Please check the reason and retry.\r\n你没有以管理员身份运行此程序或者已经引发了系统错误，请检查是否开启网络发现和其他原因后再尝试.",
                    //    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ErrorMessage = "Maybe you don't run this program with the Administrator right, or there was an system error. Please check the reason and retry.";
                    return null;
                    }

                //10、临时存储IP地址，按照最大的额度来定义
                string[] TempAllIPAddress = new string[sAllIPAddress.Length];

                //11、计算实际有效的IP地址个数：去除空字符串，可能会有最后一个是回车换行符号
                Int16 ActualNumberOfIPAddress = 0;
                int TempLength = sAllIPAddress.Length;

                for (int b = 0; b < TempLength; b++)
                    {

                    if ((sAllIPAddress[b] != "") & (sAllIPAddress[b] != "\r\n"))
                        {
                        TempAllIPAddress[ActualNumberOfIPAddress] = sAllIPAddress[b];
                        ActualNumberOfIPAddress += 1;
                        }
                    }

                //12、重新定义AllIPAddress的长度，用于保存IP地址
                sAllIPAddress = new string[ActualNumberOfIPAddress];

                //13、将读取到的IP地址复制到AllIPAddress
                for (int b = 0; b < ActualNumberOfIPAddress; b++)
                    {
                    sAllIPAddress[b] = TempAllIPAddress[b];
                    }

                //14、去掉与本机IP地址前3节不同的IP地址
                //Array.Resize<string>(ref TempAllIPAddress, 1);
                string LocalIPAddress = GetLocalIP4Address();
                string[] LocalIP, OtherIP;
                LocalIP = LocalIPAddress.Split('.');
                int CountForCorrectIP = 0;
                for (int a = 0; a < sAllIPAddress.Length; a++) 
                    {
                    OtherIP = sAllIPAddress[a].Split('.');
                    if (LocalIP[0] == OtherIP[0]
                        && LocalIP[1] == OtherIP[1]
                        && LocalIP[2] == OtherIP[2]) 
                        {
                        CountForCorrectIP += 1;
                        Array.Resize<string>(ref TempAllIPAddress, CountForCorrectIP);
                        TempAllIPAddress[CountForCorrectIP - 1] = sAllIPAddress[a];
                        }
                    }

                //15、重新定义返回数组大小并将正确IP地址复制给返回数组
                Array.Resize<string>(ref sAllIPAddress, TempAllIPAddress.Length);

                for (int b = 0; b < TempAllIPAddress.Length; b++) 
                    {
                    sAllIPAddress[b] = TempAllIPAddress[b];
                    }

                }
            catch (Exception ex)
                {
                //清除目录下的原有BAT/TXT文件记录【如果存在的话】
                if (pcFileSystem.FileSystem.FileExists(BATFileNameForGetIPAddress) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(BATFileNameForGetIPAddress);
                    }

                if (pcFileSystem.FileSystem.FileExists(TXTFileNameForSaveIPAddress) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(TXTFileNameForSaveIPAddress);
                    }
                //MessageBox.Show(ex.Message);
                ErrorMessage = ex.Message;
                return null;
                }

            return sAllIPAddress;

            }
        
        //获取网络中已经联网的MAC地址【字符串数组】
        /// <summary>
        /// 获取网络中已经联网的MAC地址【字符串数组】
        /// </summary>
        /// <returns></returns>
        public string[] SearchAvailableMACAddress()
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ErrorMessage = "You don't have the given right to use this DLL library, please contact with ThomasPeng.";
                return null;
                }

            string GetIPAndMACAddress = "";
            //bool ChineseLanguage = false;

            //定义变量存储文件名
            string BATFileNameForGetIPAndMACAddress = "C:\\Windows\\Temp\\SearchMACAddress.bat";
            string TXTFileNameForSaveIPAndMACAddress = "C:\\Windows\\Temp\\GotMACAddress.txt";

            //Int16 iMaxCountOfComputers = 100;
            string[] sAllMACAddress;  //存储网络中实际存在的IP地址
            //Int16 iWaitTimeCount;    //定义最大等待时间，超时就退出扫描等待

            //定义类用于操作计算机相关信息，类似于VB .NET中的my.computer
            Microsoft.VisualBasic.Devices.Computer pcFileSystem = new Microsoft.VisualBasic.Devices.Computer();

            try
                {

                //System.Net.NetworkInformation.TcpConnectionInformation xx;
                Microsoft.VisualBasic.Devices.Network TempNetwork = new Microsoft.VisualBasic.Devices.Network();

                //bool TempBool;
                //TempBool = TempNetwork.IsAvailable;

                if (TempNetwork.IsAvailable == false)
                    {
                    //MessageBox.Show("This compueter has not connected to the network yet, please check the reason and retry.\r\n计算机未连接到网络，请检查原因后再尝试搜索.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ErrorMessage = "This compueter has not connected to the network yet, please check the reason and retry.";
                    return null;
                    }

                //1、清除目录下的原有BAT/TXT文件记录【如果存在的话】
                if (pcFileSystem.FileSystem.FileExists(BATFileNameForGetIPAndMACAddress) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(BATFileNameForGetIPAndMACAddress);
                    }

                if (pcFileSystem.FileSystem.FileExists(TXTFileNameForSaveIPAndMACAddress) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(TXTFileNameForSaveIPAndMACAddress);
                    }

                //*****************************
                //以下是通过批处理命令arp -a得到的结果：
                //
                //接口: 192.168.0.125 --- 0x3
                //  Internet 地址         物理地址              类型
                //  192.168.0.1           8c-a6-df-7e-81-fa     动态        
                //  192.168.0.101         74-04-2b-d3-db-1e     动态        
                //  192.168.0.124         fc-d7-33-3a-5d-cc     动态        
                //  192.168.0.255         ff-ff-ff-ff-ff-ff     静态        
                //  224.0.0.2             01-00-5e-00-00-02     静态        
                //  224.0.0.22            01-00-5e-00-00-16     静态        
                //  224.0.0.251           01-00-5e-00-00-fb     静态        
                //  224.0.0.252           01-00-5e-00-00-fc     静态        
                //255.255.255.255       ff-ff-ff-ff-ff-ff     静态
                //以上总共3列，Tokens=1得到IP地址这一列，Tokens=2得到物理地址这一列，Tokens=3得到类型这一列，而且上面的内容是以空格为分隔符；

                //**********************************
                //Tokens用于获取第n列的内容；
                //delims用于定义分隔符；
                //skip=n 忽略文本开头的前n行；
                //eol=c  忽略以某字符开头的行；
                //**********************************

                //2、组织BAT命令
                //将DOS的批处理命令写入文件BAT中

                //>：批处理命令中的>是将内容导出到某文件或其它目标，并进行覆盖，比如：ECHO Done>C:\Example.txt,即将文字Done写入文件C:\Example.txt，如果原来存在此文件，则会覆盖
                //>>：同样是导出内容，但是执行的是添加的动作，如果原来存在某文件或目标，则会在最后添加相应内容

                GetIPAndMACAddress = "@ECHO Off\r\nfor /f \"skip=3 tokens=2,* delims= \" %%i in ('arp -a') do ECHO %%i>>\r\n" + TXTFileNameForSaveIPAndMACAddress + "\r\ndel " + BATFileNameForGetIPAndMACAddress;

                //3、写入BAT命令至文件
                //保存的中文会乱码，不支持GB码
                //pcFileSystem.FileSystem.WriteAllText(BATFileNameForGetIPAddress, InstructionsForSearchingIPAddress, false, System.Text.Encoding.ASCII) //System.Text.Encoding.GetEncoding(936));

                //保存的中文不会乱码，但是在DOS环境下会乱码
                pcFileSystem.FileSystem.WriteAllText(BATFileNameForGetIPAndMACAddress, GetIPAndMACAddress, false);

                //4、执行BAT命令【隐藏DOS命令窗口】
                Microsoft.VisualBasic.Interaction.Shell(BATFileNameForGetIPAndMACAddress, Microsoft.VisualBasic.AppWinStyle.Hide);
                //Microsoft.VisualBasic.Interaction.Shell(BATFileNameForGetIPAddress, Microsoft.VisualBasic.AppWinStyle.Hide,true,2000);

                //5、等待BAT命令完成
                //方法一：读取指定文件，并判断完成标志

                //方法二：在批处理文件的最后一句添加一条删除自身的语句，执行后就扫描此文件是否存在，如果不存在就读取保存的txt文件进行IP地址读取处理；
                Int16 iOverTimeCounter = 0;

                do
                    {

                    Thread.Sleep(1);
                    System.Windows.Forms.Application.DoEvents();
                    iOverTimeCounter += 1;
                    if (iOverTimeCounter >= 5000)
                        {

                        if (MessageBox.Show("It took a bit long time to search but haven't finished yet, are you sure to continue?\r\n已经花了比较长的时间进行搜索，但还没有完成，还要继续吗？",
                            "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                            {
                            iOverTimeCounter = 0;
                            }
                        else
                            {
                            return null;
                            }

                        }

                    } while (pcFileSystem.FileSystem.FileExists(BATFileNameForGetIPAndMACAddress) == true);

                //6、判断是否已经联网，并处理没有网络就退出函数
                //如果记录IP地址的TXT文件不存在，则证明电脑没有和其它网络相连

                if (pcFileSystem.FileSystem.FileExists(TXTFileNameForSaveIPAndMACAddress) == false)
                    {
                    //MessageBox.Show("Maybe you didn't run this program with the Administrator right, or there was an system error. Please check the reason and retry.\r\n你没有以管理员身份运行此程序或者已经引发了系统错误，请检查是否开启网络发现和其他原因后再尝试.",
                    //    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ErrorMessage = "Maybe you didn't run this program with the Administrator right, or there was an system error. Please check the reason and retry.";
                    return null;
                    }

                //7、从文本文件读取记录进行处理
                //用File.ReadAllLines方法读取文件会执行关闭动作，会导致DOS BAT文件执行写入时出现异常

                //读取存储IP地址的文件
                sAllMACAddress = System.IO.File.ReadAllLines(TXTFileNameForSaveIPAndMACAddress);

                //清除目录下的原有BAT/TXT文件记录【如果存在的话】
                if (pcFileSystem.FileSystem.FileExists(BATFileNameForGetIPAndMACAddress) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(BATFileNameForGetIPAndMACAddress);
                    }

                if (pcFileSystem.FileSystem.FileExists(TXTFileNameForSaveIPAndMACAddress) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(TXTFileNameForSaveIPAndMACAddress);
                    }

                //执行BAT文件时可能会出现权限不够无法执行或者执行中遇到错误的情况，导致保存执行结果的TXT文件为空，在此进行判断。

                if (sAllMACAddress.Length == 0)
                    {
                    //MessageBox.Show("Maybe you didn't run this program with the Administrator right, or there was an system error. Please check the reason and retry.\r\n你没有以管理员身份运行此程序或者已经引发了系统错误，请检查是否开启网络发现和其他原因后再尝试.",
                    //    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ErrorMessage = "Maybe you didn't run this program with the Administrator right, or there was an system error. Please check the reason and retry.";
                    return null;
                    }

                //临时存储IP地址，按照最大的额度来定义
                string[] TempAllIPAddress = new string[sAllMACAddress.Length];

                //计算实际有效的IP地址个数：去除空字符串，可能会有最后一个是回车换行符号
                Int16 ActualNumberOfIPAddress = 0;
                int TempLength = sAllMACAddress.Length;

                for (int b = 0; b < TempLength; b++)
                    {

                    if ((sAllMACAddress[b] != "") & (sAllMACAddress[b] != "\r\n"))
                        {
                        TempAllIPAddress[ActualNumberOfIPAddress] = sAllMACAddress[b];
                        ActualNumberOfIPAddress += 1;
                        }
                    }

                //重新定义AllIPAddress的长度，用于保存IP地址
                sAllMACAddress = new string[ActualNumberOfIPAddress];

                //将读取到的IP地址复制到AllIPAddress
                for (int b = 0; b < ActualNumberOfIPAddress; b++)
                    {
                    sAllMACAddress[b] = TempAllIPAddress[b];
                    }

                }
            catch (Exception ex)
                {
                //清除目录下的原有BAT/TXT文件记录【如果存在的话】
                if (pcFileSystem.FileSystem.FileExists(BATFileNameForGetIPAndMACAddress) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(BATFileNameForGetIPAndMACAddress);
                    }

                if (pcFileSystem.FileSystem.FileExists(TXTFileNameForSaveIPAndMACAddress) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(TXTFileNameForSaveIPAndMACAddress);
                    }
                //MessageBox.Show(ex.Message);
                ErrorMessage = ex.Message;
                return null;
                }

            return sAllMACAddress;

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
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ErrorMessage = "You don't have the given right to use this DLL library, please contact with ThomasPeng.";
                return "";
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
                    //MessageBox.Show("No other computer/device in the network...\r\n此时网络中没有其它设备...",
                    //    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ErrorMessage = "No other computer/device in the network...";
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
                //MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorMessage = ex.Message;
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
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ErrorMessage = "You don't have the given right to use this DLL library, please contact with ThomasPeng.";
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
                //MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorMessage = ex.Message;
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
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ErrorMessage = "You don't have the given right to use this DLL library, please contact with ThomasPeng.";
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
                    //MessageBox.Show("No other computer/device in the network...\r\n此时网络中没有其它设备...",
                    //    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ErrorMessage = "No other computer/device in the network...";
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
                //MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorMessage = ex.Message;
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
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ErrorMessage = "You don't have the given right to use this DLL library, please contact with ThomasPeng.";
                return null;
                }

            string TempPCName = "";

            try
                {
                TempPCName = System.Net.Dns.GetHostName();
                }
            catch (Exception ex)
                {
                //MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorMessage = ex.Message;
                return "";
                }

            return TempPCName;

            }

        //按照输入的IP地址区段搜索以太网设备
        /// <summary>
        /// 按照输入的IP地址区段搜索以太网设备
        /// </summary>
        /// <param name="TargetIPSegment">需要进行搜索的IP地址目标分段
        /// 例如搜索 "192.168.1.0~255" 整个区段的可用IP地址，则参数为 "192.168.1"
        /// </param>
        /// <returns>返回已经搜索到以太网设备的IP地址字符串数组</returns>
        public string[] SearchIPAddressForCertainSegment(string TargetIPSegment)
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ErrorMessage = "You don't have the given right to use this DLL library, please contact with ThomasPeng.";
                return null;
                }

            if (TargetIPSegment == "")
                {
                //MessageBox.Show("The parameter \"TargetIPSegment\" is empty, please pass the correct parameter and retry.\r\n参数\"TargetIPSegment\"为空，请传递正确参数后再尝试.",
                //    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorMessage = "The parameter \"TargetIPSegment\" is empty, please pass the correct parameter and retry.";
                return null;
                }

            //查找是否有两个"."号，如果没有则判断为参数不正确
            if (Strings.InStr(TargetIPSegment, ".") > 0)
                {

                //先找到第一个"."的位置，然后以这个位置为起点找下一个"."的位置；
                if (Strings.InStr(Strings.InStr(TargetIPSegment, ".") + 1, TargetIPSegment, ".") <= 0)
                    {
                    ErrorMessage = "参数\"TargetIPSegment\"不正确，例如：设置\"192.168.1\"来搜索区段\"192.168.1.0~255\", 请修改参数后再重试.";
                    //MessageBox.Show("The value of parameter \"TargetIPSegment\" is not correct. For example: set \"192.168.1\" to search segment \"192.168.1.0~255\", please revise it and retry.\rn参数\"TargetIPSegment\"不正确，例如：设置\"192.168.1\"来搜索区段\"192.168.1.0~255\", 请修改参数后再重试.",
                    //    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                    }

                }
            else
                {
                //MessageBox.Show("The value of parameter \"TargetIPSegment\" is not correct. For example: set \"192.168.1\" to search segment \"192.168.1.0~255\", please revise it and retry.\rn参数\"TargetIPSegment\"不正确，例如：设置\"192.168.1\"来搜索区段\"192.168.1.0~255\", 请修改参数后再重试.",
                //    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorMessage = "The value of parameter \"TargetIPSegment\" is not correct. For example: set \"192.168.1\" to search segment \"192.168.1.0~255\", please revise it and retry.";
                return null;
                }

            //******************
            ushort usIP4, usIP3, usIP2;
            ushort usAllScannedIPCount = 0;
            string[] sAllScannedIPAddress = null;
            //string sTempLocalIPAddress = "";
            string[] sTempAddress;

            //******************

            //最好是添加代码验证参数全部是数字和"."，没有其它非法字符

            try
                {

                //System.Net.NetworkInformation.TcpConnectionInformation xx;
                Microsoft.VisualBasic.Devices.Network TempNetwork = new Microsoft.VisualBasic.Devices.Network();

                //bool TempBool;
                //TempBool = TempNetwork.IsAvailable;

                if (TempNetwork.IsAvailable == false)
                    {
                    //MessageBox.Show("This compueter has not connected to the network yet, please check the reason and retry.\r\n计算机未连接到网络，请检查原因后再尝试搜索.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ErrorMessage = "This compueter has not connected to the network yet, please check the reason and retry.";
                    return null;
                    }

                sTempAddress = Strings.Split(TargetIPSegment, ".");

                if (sTempAddress.Length < 3 | sTempAddress.Length > 3)
                    {
                    ErrorMessage = "参数\"TargetIPSegment\"不正确，例如：设置\"192.168.1\"来搜索区段\"192.168.1.0~255\", 请修改参数后再重试.";
                    //MessageBox.Show("The value of parameter \"TargetIPSegment\" is not correct. For example: set \"192.168.1\" to search segment \"192.168.1.0~255\", please revise it and retry.\rn参数\"TargetIPSegment\"不正确，例如：设置\"192.168.1\"来搜索区段\"192.168.1.0~255\", 请修改参数后再重试.",
                    //    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                    }

                if (sTempAddress[0] == "" | sTempAddress[1] == "" | sTempAddress[2] == "")
                    {
                    ErrorMessage = "IP地址段的值必须为0~255";
                    //MessageBox.Show("The IP address section can't be empty, it must be 0~255.\r\nIP地址段的值必须为0~255",
                    //    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                    }

                //三个分段任意一个不在0~255就报错
                if ((Convert.ToInt16(sTempAddress[0]) < 0 | Convert.ToInt16(sTempAddress[0]) > 255) |
                    (Convert.ToInt16(sTempAddress[1]) < 0 | Convert.ToInt16(sTempAddress[1]) > 255) |
                    (Convert.ToInt16(sTempAddress[2]) < 0 | Convert.ToInt16(sTempAddress[2]) > 255))
                    {
                    ErrorMessage = "IP地址段的值必须为0~255";
                    //MessageBox.Show("The IP address section can't be empty, it must be 0~255.\r\nIP地址段的值必须为0~255",
                    //    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                    }

                ErrorMessage = "正在搜索网络中的以太网设备...";

                usIP4 = Convert.ToUInt16(sTempAddress[0]);
                usIP3 = Convert.ToUInt16(sTempAddress[1]);
                usIP2 = Convert.ToUInt16(sTempAddress[2]);

                //******************
                DateTime BeginTime, FinishTime, TempTime, OverTime;
                TimeSpan SpentTimeForSearching;

                FinishTime = DateTime.Now;
                BeginTime = DateTime.Now;
                OverTime = DateTime.Now;
                //******************

                Ping PingSender = new Ping();
                IPAddress Address;
                PingReply Reply;

                for (Int16 IP1 = 0; IP1 <= 255; IP1++)
                    {

                    Address = IPAddress.Parse(usIP4 + "." + usIP3 + "." + usIP2 + "." + IP1);
                    Reply = PingSender.Send(Address, 3);

                    System.Windows.Forms.Application.DoEvents();

                    if (Reply.Status == IPStatus.Success)
                        {
                        sAllScannedIPAddress = new string[usAllScannedIPCount];
                        sAllScannedIPAddress[usAllScannedIPCount] = Reply.Address.ToString();
                        }
                    else
                        {
                        }

                    usAllScannedIPCount += 1;

                    TempTime = DateTime.Now;
                    SpentTimeForSearching = TempTime - OverTime;

                    if (SpentTimeForSearching.Minutes > 1)
                        {

                        if (MessageBox.Show("It already took " + SpentTimeForSearching.Minutes + "minute(s) to search the TCP/IP device, are you sure to continue?\r\n已经耗时 " + SpentTimeForSearching.Minutes + "分钟，是否还要继续进行搜索？",
                            "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                            {
                            ErrorMessage = "已经耗时 " + SpentTimeForSearching.Minutes + " 分钟，退出搜索...";
                            break;
                            }
                        else
                            {
                            ErrorMessage = "已经耗时 " + SpentTimeForSearching.Minutes + " 分钟，继续进行搜索...";
                            OverTime = DateTime.Now;
                            System.Windows.Forms.Application.DoEvents();
                            }

                        }

                    }

                FinishTime = DateTime.Now;
                SpentTimeForSearching = FinishTime - BeginTime;

                if (sAllScannedIPAddress.Length == 0)
                    {
                    ErrorMessage = "总计搜索到0个以太网设备." + " 耗时：" + SpentTimeForSearching.Minutes + "分" + SpentTimeForSearching.Seconds + "秒" +
                       SpentTimeForSearching.Milliseconds + "毫秒";

                    //MessageBox.Show("There is no available TCP/IP device in the network, the searching spent: " + SpentTimeForSearching.Minutes + "m" +
                    //       SpentTimeForSearching.Seconds + "s" + SpentTimeForSearching.Milliseconds + "ms\r\n 总计搜索到0个以太网设备." +
                    //       " 耗时：" + SpentTimeForSearching.Minutes + "分" + SpentTimeForSearching.Seconds + "秒"
                    //       + SpentTimeForSearching.Milliseconds + "毫秒", "Overtime", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                else
                    {

                    ErrorMessage = "总计搜索到 " + sAllScannedIPAddress.Length + " 个以太网设备， 耗时：" + SpentTimeForSearching.Minutes + "分" +
                              SpentTimeForSearching.Seconds + "秒" + SpentTimeForSearching.Milliseconds + "毫秒";

                    //MessageBox.Show("There is(are) " + sAllScannedIPAddress.Length + " available TCP/IP device(s) in the network, the searching spent: " + SpentTimeForSearching.Minutes + "m" +
                    //    SpentTimeForSearching.Seconds + "s" + SpentTimeForSearching.Milliseconds + "ms\r\n 总计搜索到0个以太网设备." +
                    //    " 耗时：" + SpentTimeForSearching.Minutes + "分" + SpentTimeForSearching.Seconds + "秒"
                    //    + SpentTimeForSearching.Milliseconds + "毫秒", "Overtime", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                for (Int16 a = 0; a < sAllScannedIPAddress.Length; a++)
                    {
                    ErrorMessage = "Found device-- No.: " + (a + 1) + " IP address: " + sAllScannedIPAddress[a];
                    }

                }
            catch (Exception ex)
                {
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorMessage = ex.Message;
                return null;
                }

            return sAllScannedIPAddress;

            }

        //搜索网络中所有可用计算机的名称
        /// <summary>
        /// 搜索网络中所有可用计算机的名称
        /// </summary>
        /// <returns>返回搜索到的计算机名称数组</returns>
        public string[] SearchAvailablePCNames()
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ErrorMessage = "You don't have the given right to use this DLL library, please contact with ThomasPeng.";
                return null;
                }

            string InstructionsForSearchingPCNames = "";
            //bool ChineseLanguage = false;

            //定义变量存储文件名
            string BATFileNameForGetPCNames = "C:\\Windows\\Temp\\SearchPCNames.bat";
            string TXTFileNameForSavePCNames = "C:\\Windows\\Temp\\GotPCNames.txt";

            //Int16 iMaxCountOfComputers = 100;
            string[] sAllPCNames;  //存储网络中实际存在的IP地址
            //Int16 iWaitTimeCount;    //定义最大等待时间，超时就退出扫描等待

            //******************************
            DateTime BeginTime, FinishTime;
            TimeSpan SpentTimeForSearching;

            FinishTime = DateTime.Now;
            BeginTime = DateTime.Now;
            //******************************

            //定义类用于操作计算机相关信息，类似于VB .NET中的my.computer
            Microsoft.VisualBasic.Devices.Computer pcFileSystem = new Microsoft.VisualBasic.Devices.Computer();

            try
                {

                //System.Net.NetworkInformation.TcpConnectionInformation xx;
                Microsoft.VisualBasic.Devices.Network TempNetwork = new Microsoft.VisualBasic.Devices.Network();

                //bool TempBool;
                //TempBool = TempNetwork.IsAvailable;

                if (TempNetwork.IsAvailable == false)
                    {
                    //MessageBox.Show("This compueter has not connected to the network yet, please check the reason and retry.\r\n计算机未连接到网络，请检查原因后再尝试搜索.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ErrorMessage = "This compueter has not connected to the network yet, please check the reason and retry.";
                    return null;
                    }

                //1、清除目录下的原有BAT/TXT文件记录【如果存在的话】
                if (pcFileSystem.FileSystem.FileExists(BATFileNameForGetPCNames) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(BATFileNameForGetPCNames);
                    }

                if (pcFileSystem.FileSystem.FileExists(TXTFileNameForSavePCNames) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(TXTFileNameForSavePCNames);
                    }

                //**********************************
                //Tokens用于获取第n列的内容；
                //delims用于定义分隔符；
                //skip=n 忽略文本开头的前n行；
                //eol=c  忽略以某字符开头的行；
                //**********************************

                //'以下为执行net view的结果：
                //'**************************
                //'服务器名称            注解

                //'-------------------------------------------------------------------------------
                //'\\2FP                  IQC                                                     
                //'\\CCD01                CCD                                             
                //'\\ED20150604NY                                                              
                //'\\ED20150907OQ                                                              
                //'\\ED20160414DH                                                              
                //'\\EDJK                                                                      
                //'\\EDFILE1                                                                   
                //'\\EDFILE2                                                                   
                //'\\EDERP                                                                      
                //'\\EDPRN              edacprn                                                 
                //'\\FD06                 仓库                                           
                //'\\FD07                 仓库                                           
                //'\\6T7CJF                                                              
                //'\\ID02                                                                         
                //'\\MD12                                                                         
                //'\\MD13                 MD                                              
                //'\\PL04                电控                                            
                //'\\PL06                电控                                             
                //'\\PL08                电控                                              
                //'\\RNPF11F8F                                                                    
                //'命令成功完成。
                //'**************************

                //'其它几种可能的情况：
                // '1、出现的内容为：列表是空的。【没有连接网络时】
                // '2、出现错误代码：6118【没有开启网络发现时】

                // '根据以上情况，正确的结果是：
                // '1、命令执行完后文件内容的行数必须大于4行及以上；
                // '2、行以\\开头

                // '处理方法：
                // '1、读取所有行到数组；
                // '2、导入内存后，去掉前两行和最后一行；
                // '3、跳过\\，找到字符串中第一个空格字符' '的位置,
                // '   取得从\\后的第一个字符到整个字符串中第一个空格字符的位置-1之间的字符串，即为计算机在网络中的名称；
                // '4、从第一个空格字符的位置到字符串整长，取得字符串，进行判断，如果全部为空格，则备注名为空；否则去掉前缀空格和后缀空格得到计算机的备注名；

                // '2、组织BAT命令
                // '将DOS的批处理命令写入文件BAT中

                // '>：批处理命令中的>是将内容导出到某文件或其它目标，并进行覆盖，比如：ECHO Done>C:\Example.txt,即将文字Done写入文件C:\Example.txt，如果原来存在此文件，则会覆盖
                // '>>：同样是导出内容，但是执行的是添加的动作，如果原来存在某文件或目标，则会在最后添加相应内容

                // '方法：在批处理文件的最后一句添加一条删除自身的语句，执行后就扫描此文件是否存在，如果不存在就读取保存的txt文件进行IP地址读取处理；

                InstructionsForSearchingPCNames = "@ECHO Off\r\nnet view>> " + TXTFileNameForSavePCNames + "\r\ndel " + BATFileNameForGetPCNames;

                //3、写入BAT命令至文件
                //保存的中文会乱码，不支持GB码
                //pcFileSystem.FileSystem.WriteAllText(BATFileNameForGetIPAddress, InstructionsForSearchingIPAddress, false, System.Text.Encoding.ASCII) //System.Text.Encoding.GetEncoding(936));

                //保存的中文不会乱码，但是在DOS环境下会乱码
                pcFileSystem.FileSystem.WriteAllText(BATFileNameForGetPCNames, InstructionsForSearchingPCNames, false);

                //4、执行BAT命令【隐藏DOS命令窗口】
                Microsoft.VisualBasic.Interaction.Shell(BATFileNameForGetPCNames, Microsoft.VisualBasic.AppWinStyle.Hide);
                //Microsoft.VisualBasic.Interaction.Shell(BATFileNameForGetIPAddress, Microsoft.VisualBasic.AppWinStyle.Hide,true,2000);

                //5、等待BAT命令完成
                //方法二：在批处理文件的最后一句添加一条删除自身的语句，执行后就扫描此文件是否存在，如果不存在就读取保存的txt文件进行IP地址读取处理；

                Int16 iOverTimeCounter = 0;

                do
                    {

                    Thread.Sleep(1);
                    System.Windows.Forms.Application.DoEvents();
                    iOverTimeCounter += 1;
                    if (iOverTimeCounter >= 5000)
                        {

                        if (MessageBox.Show("It took a bit long time to search but haven't finished yet, are you sure to continue?\r\n已经花了比较长的时间进行搜索，但还没有完成，还要继续吗？",
                            "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                            {
                            iOverTimeCounter = 0;
                            }
                        else
                            {
                            return null;
                            }

                        }

                    } while (pcFileSystem.FileSystem.FileExists(BATFileNameForGetPCNames) == true);

                //6、判断是否已经联网，并处理没有网络就退出函数
                //如果记录IP地址的TXT文件不存在，则证明电脑没有和其它网络相连

                if (pcFileSystem.FileSystem.FileExists(TXTFileNameForSavePCNames) == false)
                    {
                    //MessageBox.Show("Maybe you didn't run this program with the Administrator right, or there was an system error. Please check the reason and retry.\r\n你没有以管理员身份运行此程序或者已经引发了系统错误，请检查是否开启网络发现和其他原因后再尝试.",
                    //    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ErrorMessage = "Maybe you didn't run this program with the Administrator right, or there was an system error. Please check the reason and retry.";
                    return null;
                    }

                //7、从文本文件读取记录进行处理
                //用File.ReadAllLines方法读取文件会执行关闭动作，会导致DOS BAT文件执行写入时出现异常

                //读取存储IP地址的文件
                sAllPCNames = System.IO.File.ReadAllLines(TXTFileNameForSavePCNames);

                //清除目录下的原有BAT/TXT文件记录【如果存在的话】
                if (pcFileSystem.FileSystem.FileExists(BATFileNameForGetPCNames) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(BATFileNameForGetPCNames);
                    }

                if (pcFileSystem.FileSystem.FileExists(TXTFileNameForSavePCNames) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(TXTFileNameForSavePCNames);
                    }

                //执行BAT文件时可能会出现权限不够无法执行或者执行中遇到错误的情况，导致保存执行结果的TXT文件为空，在此进行判断。

                if (sAllPCNames.Length == 0)
                    {
                    //MessageBox.Show("Maybe you didn't run this program with the Administrator right, or there was an system error. Please check the reason and retry.\r\n你没有以管理员身份运行此程序或者已经引发了系统错误，请检查是否开启网络发现和其他原因后再尝试.",
                    //    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ErrorMessage = "Maybe you didn't run this program with the Administrator right, or there was an system error. Please check the reason and retry.";
                    return null;
                    }

                //临时存储IP地址，按照最大的额度来定义
                string[] TempAllPCNames = new string[sAllPCNames.Length];

                //计算实际有效的计算机名称数组个数：去除空字符串，可能会有最后一个是回车换行符号
                Int16 ActaulNumberOfPCNames = 0;
                int TempLength = sAllPCNames.Length;

                for (int b = 0; b < TempLength; b++)
                    {

                    if ((sAllPCNames[b] != "") & (sAllPCNames[b] != "\r\n"))
                        {
                        TempAllPCNames[ActaulNumberOfPCNames] = sAllPCNames[b];
                        ActaulNumberOfPCNames += 1;
                        }
                    }

                //重新定义AllPCNames的长度，用于保存计算机名称数组
                sAllPCNames = new string[ActaulNumberOfPCNames];

                //将读取到的计算机名称复制到AllPCNames
                for (int b = 0; b < ActaulNumberOfPCNames; b++)
                    {
                    sAllPCNames[b] = TempAllPCNames[b];
                    }

                }
            catch (Exception ex)
                {
                //清除目录下的原有BAT/TXT文件记录【如果存在的话】
                if (pcFileSystem.FileSystem.FileExists(BATFileNameForGetPCNames) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(BATFileNameForGetPCNames);
                    }

                if (pcFileSystem.FileSystem.FileExists(TXTFileNameForSavePCNames) == true)
                    {
                    pcFileSystem.FileSystem.DeleteFile(TXTFileNameForSavePCNames);
                    }
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorMessage = ex.Message;
                return null;
                }

            FinishTime = DateTime.Now;
            SpentTimeForSearching = FinishTime - BeginTime;

            //if (sAllPCNames.Length == 1)
            //    {
            //    MessageBox.Show("There is " + sAllPCNames.Length + " available PC in the network, the searching spent: " +
            //      SpentTimeForSearching.Minutes + "m" + SpentTimeForSearching.Seconds + "s" + SpentTimeForSearching.Milliseconds + "ms"
            //      + "\r\n总计搜索到 " + sAllPCNames.Length + " 个PC， 耗时：" + SpentTimeForSearching.Minutes + "分" +
            //      SpentTimeForSearching.Seconds + "秒" + SpentTimeForSearching.Milliseconds + "毫秒", "Information",
            //      MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //else
            //    {
            //    MessageBox.Show("There are " + sAllPCNames.Length + " available PCs in the network, the searching spent: " +
            //    SpentTimeForSearching.Minutes + "m" + SpentTimeForSearching.Seconds + "s" + SpentTimeForSearching.Milliseconds + "ms"
            //    + "\r\n总计搜索到 " + sAllPCNames.Length + " 个PC， 耗时：" + SpentTimeForSearching.Minutes + "分" +
            //    SpentTimeForSearching.Seconds + "秒" + SpentTimeForSearching.Milliseconds + "毫秒", "Information",
            //    MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }

            //返回获取的计算机名称数组
            return sAllPCNames;

            }

        #endregion

        #region "获取TcpClient连接的本地/对方IP地址和端口"

        //*********************************
        //方法一：

        //获取客户端的本地连接端口,-1表示未连接
        /// <summary>
        /// 获取客户端的本地连接端口号,-1表示未连接
        /// </summary>
        /// <param name="Client">TcpClient客户端</param>
        /// <returns></returns>
        public int GetLocalPort(TcpClient Client)
            {
            try
                {
                if (PasswordIsCorrect == false
                    || SuccessBuiltNew == false)
                    {
                    return 0;
                    }

                if (Client != null)
                    {
                    //如果之前已经建立了连接，即使当前是断开的，其RemoteEndPoint属性还是存在的
                    //即使没有建立连接，在初始化时就已经指定了RemoteEndPoint
                    //if (Client.Connected == true)
                    //    {
                        //获取的远程终端字符串格式为：127.0.0.1:12466
                        Socket TempSocket = Client.Client;
                        EndPoint TempEndPoint = TempSocket.LocalEndPoint;

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
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return -1;
                }
            }

        //获取客户端的本地连接端口,-1表示未连接
        /// <summary>
        /// 获取客户端的本地连接端口,-1表示未连接
        /// </summary>
        /// <param name="Client">Socket客户端</param>
        /// <returns></returns>
        public int GetLocalPort(Socket Client)
            {
            try
                {
                if (PasswordIsCorrect == false
                    || SuccessBuiltNew == false)
                    {
                    return 0;
                    }

                if (Client != null)
                    {
                    //如果之前已经建立了连接，即使当前是断开的，其RemoteEndPoint属性还是存在的
                    //即使没有建立连接，在初始化时就已经指定了RemoteEndPoint
                    //if (Client.Connected == true)
                    //    {
                        //获取的远程终端字符串格式为：127.0.0.1:12466
                        EndPoint TempEndPoint = Client.LocalEndPoint;

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
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return -1;
                }
            }

        //获取客户端的服务器IP地址,空返回值表示未连接
        /// <summary>
        /// 获取客户端的服务器IP地址,空返回值表示未连接
        /// </summary>
        /// <param name="Client">TcpClient客户端</param>
        /// <returns></returns>
        public string GetRemoteIP(TcpClient Client)
            {
            try
                {
                if (PasswordIsCorrect == false
                    || SuccessBuiltNew == false)
                    {
                    return "";
                    }

                if (Client != null)
                    {
                    //if (Client.Connected == true)
                    //    {
                    //如果之前已经建立了连接，即使当前是断开的，其RemoteEndPoint属性还是存在的
                    //即使没有建立连接，在初始化时就已经指定了RemoteEndPoint
                        //获取的远程终端字符串格式为：127.0.0.1:12466
                        Socket TempSocket = Client.Client;
                        EndPoint TempEndPoint = TempSocket.RemoteEndPoint;

                        //方法一：
                        return TempEndPoint.ToString().Split(':')[0];

                        ////方法二：
                        //string TempIPAddress = TempEndPoint.ToString();
                        //string[] TempResult = TempIPAddress.Split(':');
                        //return TempResult[0];
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
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return "";
                }
            }

        //获取客户端的服务器IP地址,空返回值表示未连接
        /// <summary>
        /// 获取客户端的服务器IP地址,空返回值表示未连接
        /// </summary>
        /// <param name="Client">Socket客户端</param>
        /// <returns></returns>
        public string GetRemoteIP(Socket Client)
            {
            try
                {
                if (PasswordIsCorrect == false
                    || SuccessBuiltNew == false)
                    {
                    return "";
                    }

                if (Client != null)
                    {
                    //如果之前已经建立了连接，即使当前是断开的，其RemoteEndPoint属性还是存在的
                    //即使没有建立连接，在初始化时就已经指定了RemoteEndPoint
                    //if (Client.Connected == true)
                    //    {
                        //获取的远程终端字符串格式为：127.0.0.1:12466
                        EndPoint TempEndPoint = Client.RemoteEndPoint;

                        //方法一：
                        return TempEndPoint.ToString().Split(':')[0];

                        ////方法二：
                        //string TempIPAddress = TempEndPoint.ToString();
                        //string[] TempResult = TempIPAddress.Split(':');
                        //return TempResult[0];
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
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return "";
                }
            }

        //获取客户端的服务器连接端口,-1表示未连接
        /// <summary>
        /// 获取客户端的服务器连接端口号,-1表示未连接
        /// </summary>
        /// <param name="Client">TcpClient客户端</param>
        /// <returns></returns>
        public int GetRemotePort(TcpClient Client)
            {
            try
                {
                if (PasswordIsCorrect == false
                    || SuccessBuiltNew == false)
                    {
                    return 0;
                    }

                if (Client != null)
                    {
                    //如果之前已经建立了连接，即使当前是断开的，其RemoteEndPoint属性还是存在的
                    //即使没有建立连接，在初始化时就已经指定了RemoteEndPoint
                    //if (Client.Connected == true)
                    //    {
                        //获取的远程终端字符串格式为：127.0.0.1:12466
                        Socket TempSocket = Client.Client;
                        EndPoint TempEndPoint = TempSocket.RemoteEndPoint;

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
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return -1;
                }
            }

        //获取客户端的服务器连接端口,-1表示未连接
        /// <summary>
        /// 获取客户端的服务器连接端口号,-1表示未连接
        /// </summary>
        /// <param name="Client">Socket客户端</param>
        /// <returns></returns>
        public int GetRemotePort(Socket Client)
            {
            try
                {
                if (PasswordIsCorrect == false
                    || SuccessBuiltNew == false)
                    {
                    return 0;
                    }

                if (Client != null)
                    {
                    //如果之前已经建立了连接，即使当前是断开的，其RemoteEndPoint属性还是存在的
                    //即使没有建立连接，在初始化时就已经指定了RemoteEndPoint
                    //if (Client.Connected == true)
                    //    {
                        //获取的远程终端字符串格式为：127.0.0.1:12466
                        EndPoint TempEndPoint = Client.RemoteEndPoint;

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
        /// <param name="Client">TcpClient客户端</param>
        /// <returns></returns>
        public Socket GetSocket(TcpClient Client)
            {
            try
                {
                if (PasswordIsCorrect == false
                    || SuccessBuiltNew == false)
                    {
                    return null;
                    }

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

        //获取客户端的服务器IP地址
        /// <summary>
        /// 获取客户端的服务器IP地址
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        private string OldGetRemoteIP(TcpClient Client)
            {
            try
                {
                if (PasswordIsCorrect == false
                    || SuccessBuiltNew == false)
                    {
                    return "";
                    }

                string TempStr = GetSocket(Client).RemoteEndPoint.ToString().Split(':')[0];
                return TempStr;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return "";
                }
            }

        //获取客户端的服务器连接端口
        /// <summary>
        /// 获取客户端的服务器连接端口
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        private int OldGetRemotePort(TcpClient Client)
            {
            try
                {
                if (PasswordIsCorrect == false
                    || SuccessBuiltNew == false)
                    {
                    return 0;
                    }

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

        #region "通用RS232C函数"

        //搜索计算机上可用RS232C串口数量
        /// <summary>
        /// 搜索计算机上可用RS232C串口数量
        /// </summary>
        /// <returns></returns>
        public int AvailableRS232CPortCount()
            {

            if (PasswordIsCorrect == false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
                }

            if (SuccessBuiltNew == false)
                {
                //MessageBox.Show("You failed to initialize this class, invalid operation.", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        #endregion

        #region "进制转换"

        //将字节转换为二进制的字符串表示
        /// <summary>
        /// 将字节转换为二进制的字符串表示
        /// </summary>
        /// <param name="TargetByte">目标字节</param>
        /// <returns>二进制的字符串</returns>
        public string ConvertByteToBinaryStringFormat(byte TargetByte)
            {
            try
                {
                //十进制   - decimal
                //十六进制 - hex; hexadecimal; sexadecimal; 
                //二进制   - binary
                //byte ok = 12;
                //string ss = Convert.ToString(ok, 16);  输出的是C，即16进制字符
                //string ss = Convert.ToString(ok, 10);  输出的是10，即10进制字符
                //string ss = Convert.ToString(ok, 2);  输出的是1100，即2进制字符

                //if (TargetByte == null) 
                //    {
                //    ErrorMessage = "字节 " + TargetByte.ToString() + " 的值为空(null)";
                //    return "";
                //    }

                string TempStr = Convert.ToString(TargetByte, 2);
                //长度不足8位数，在前面补一定位数凑够8位数
                if (TempStr.Length != 8)
                    {
                    TempStr = Strings.StrDup(8 - TempStr.Length, "0") + TempStr;
                    }
                return TempStr.ToUpper();
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return "";
                }
            }

        //将整型数转换为二进制的字符串表示
        /// <summary>
        /// 将整型数转换为二进制的字符串表示
        /// </summary>
        /// <param name="TargetInt">目标整型数</param>
        /// <returns>二进制的字符串</returns>
        public string ConvertIntToBinaryStringFormat(int TargetInt)
            {
            try
                {
                //十进制   - decimal
                //十六进制 - hex; hexadecimal; sexadecimal; 
                //二进制   - binary
                //byte ok = 12;
                //string ss = Convert.ToString(ok, 16);  输出的是C，即16进制字符
                //string ss = Convert.ToString(ok, 10);  输出的是10，即10进制字符
                //string ss = Convert.ToString(ok, 2);  输出的是1100，即2进制字符

                string TempStr = Convert.ToString(TargetInt, 2);
                //长度不足32位数，在前面补一定位数凑够32位数
                if (TempStr.Length != 32)
                    {
                    TempStr = Strings.StrDup(32 - TempStr.Length, "0") + TempStr;
                    }
                return TempStr.ToUpper();
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return "";
                }
            }

        //将字节转换为十六进制的字符串表示
        /// <summary>
        /// 将字节转换为十六进制的字符串表示
        /// </summary>
        /// <param name="TargetByte">目标字节</param>
        /// <returns>十六进制的字符串</returns>
        public string ConvertByteToHexStringFormat(byte TargetByte)
            {
            try
                {
                //十进制   - decimal
                //十六进制 - hex; hexadecimal; sexadecimal; 
                //二进制   - binary
                //byte ok = 12;
                //string ss = Convert.ToString(ok, 16);  输出的是C，即16进制字符
                //string ss = Convert.ToString(ok, 10);  输出的是10，即10进制字符
                //string ss = Convert.ToString(ok, 2);  输出的是1100，即2进制字符

                //if (TargetByte == null)
                //    {
                //    ErrorMessage = "字节 " + TargetByte.ToString() + " 的值为空(null)";
                //    return "";
                //    }

                string TempStr = Convert.ToString(TargetByte, 16);
                //长度不足两位数，在前面补一位数
                if (TempStr.Length == 1)
                    {
                    TempStr = "0" + TempStr;
                    }
                return TempStr.ToUpper();
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return "";
                }
            }

        //将整型数转换为十六进制的字符串表示
        /// <summary>
        /// 将整型数转换为十六进制的字符串表示
        /// </summary>
        /// <param name="TargetInt">目标整型数</param>
        /// <returns>十六进制的字符串</returns>
        public string ConvertIntToHexStringFormat(int TargetInt)
            {
            try
                {
                //十进制   - decimal
                //十六进制 - hex; hexadecimal; sexadecimal; 
                //二进制   - binary
                //byte ok = 12;
                //string ss = Convert.ToString(ok, 16);  输出的是C，即16进制字符
                //string ss = Convert.ToString(ok, 10);  输出的是10，即10进制字符
                //string ss = Convert.ToString(ok, 2);  输出的是1100，即2进制字符

                string TempStr = Convert.ToString(TargetInt, 16);
                //长度不足8位数，在前面补足位数
                if (TempStr.Length < 8)
                    {
                    TempStr = Strings.StrDup(8 - TempStr.Length, "0") + TempStr;
                    }
                return TempStr.ToUpper();
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return "";
                }
            }

        //十六进制转换为10进制
        /// <summary>
        /// 十六进制转换为10进制
        /// </summary>
        /// <param name="TargetHex">需要转换的十六进制内容</param>
        /// <returns>返回转换后的10进制数值, -1代表转换失败</returns>
        public double HexToDecimal(string TargetHex) 
            {
            if (PasswordIsCorrect == false
                || SuccessBuiltNew == false)
                {
                return 0;
                }
            
            //十六----> 十 
            //（19.A）（十六）            
            //整数部分:
            //1*16（1）+9*16（0）=25 
            //小数部分:
            //10*16（-1）=0.625 
            //所以(19.A)(十六) = (25.625)(十)

            try
                {
                TargetHex = TargetHex.ToUpper();
                double TempInt = 0.0;

                //检查字符串是否为0~9,A~F
                for (int a = 0; a < TargetHex.Length; a++) 
                    {
                    if(TargetHex[a] !='0' & TargetHex[a] !='1' &
                        TargetHex[a] !='2' & TargetHex[a] !='3' &
                        TargetHex[a] !='4' & TargetHex[a] !='5' &
                        TargetHex[a] !='6' & TargetHex[a] !='7' &
                        TargetHex[a] !='8' & TargetHex[a] !='9' &
                        TargetHex[a] !='A' & TargetHex[a] !='B' &
                        TargetHex[a] !='C' & TargetHex[a] !='D' &
                        TargetHex[a] !='E' & TargetHex[a] !='F')
                        {
                        ErrorMessage = "The value for parameter 'TargetHex' should be 0~9 and A~F.";
                        return -1;
                        }
                    else if (TargetHex[a] != '.')
                        {
                        TempInt += 1;
                        }                    
                    }

                //只能有一个点号
                if (TempInt > 1) 
                    {
                    ErrorMessage = "The parameter 'TargetHex' has " + TempInt + ".('dots'), invalid operation.";
                    return -1;
                    }

                TempInt = 0;

                //判断是否有小数点
                if (TargetHex.IndexOf(".") == -1)
                    {
                    //无小数点号
                    //计算转换16进制
                    for (int a = 0; a < TargetHex.Length; a++) 
                        {                        
                        switch(TargetHex[a])
                            {                            
                            case '0':
                                TempInt += 0;
                                break;

                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                TempInt += Conversion.Val(TargetHex[a]) * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'A':
                                TempInt += 10 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'B':
                                TempInt += 11 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'C':
                                TempInt += 12 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'D':
                                TempInt += 13 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'E':
                                TempInt += 14 * (Math.Pow(16, (TargetHex.Length - a - 1)));
                                break;

                            case 'F':
                                TempInt += 15 * (Math.Pow(16, (TargetHex.Length - a - 1)));
                                break;                            
                            }
                        }
                    }
                else 
                    {
                    //有小数点号
                    //先计算转换16进制整数部分：
                    string TempStr = Strings.Mid(TargetHex, 1, TargetHex.IndexOf("."));

                    for (int a = 0; a < TempStr.Length; a++) 
                        {

                        switch (TempStr[a])
                            {

                            case '0':
                                TempInt += 0;
                                break;

                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                TempInt += Conversion.Val(TargetHex[a]) * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'A':
                                TempInt += 10 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'B':
                                TempInt += 11 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'C':
                                TempInt += 12 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'D':
                                TempInt += 13 * Math.Pow(16, (TargetHex.Length - a - 1));
                                break;

                            case 'E':
                                TempInt += 14 * (Math.Pow(16, (TargetHex.Length - a - 1)));
                                break;

                            case 'F':
                                TempInt += 15 * (Math.Pow(16, (TargetHex.Length - a - 1)));
                                break;
                            }
                        }

                    //再计算转换16进制小数部分：
                    TempStr = Strings.Mid(TargetHex, TargetHex.IndexOf(".") + 2, TargetHex.Length - TargetHex.IndexOf(".") - 1);

                    for (int a = 0; a < TempStr.Length; a++)
                        {

                        switch (TempStr[a])
                            {

                            case '0':
                                TempInt += 0;
                                break;

                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                TempInt += Conversion.Val(TargetHex[a]) * Math.Pow(16, -(a + 1));
                                break;

                            case 'A':
                                TempInt += 10 * Math.Pow(16, -(a + 1));
                                break;

                            case 'B':
                                TempInt += 11 * Math.Pow(16, -(a + 1));
                                break;

                            case 'C':
                                TempInt += 12 * Math.Pow(16, -(a + 1));
                                break;

                            case 'D':
                                TempInt += 13 * Math.Pow(16, -(a + 1));
                                break;

                            case 'E':
                                TempInt += 14 * Math.Pow(16, -(a + 1));
                                break;

                            case 'F':
                                TempInt += 15 * Math.Pow(16, -(a + 1));
                                break;
                            }
                        }
                    }
                return TempInt;

                }
            catch (Exception ex) 
                {
                ErrorMessage = ex.Message;
                return -1;
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

            if (PasswordIsCorrect == false
                || SuccessBuiltNew==false)
                {
                //MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return "";
                }

            if (TargetString == "")
                {
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

        #endregion

        //查找一个字符串中含有多少个另外一个字符串
        /// <summary>
        /// 查找一个字符串中含有多少个另外一个字符串
        /// </summary>
        /// <param name="TargetToBeFound">需要查找的字符串</param>
        /// <param name="Source">源字符串【用来查找含有多少个TargetToBeFound】</param>
        /// <returns></returns>
        public int FindStringInAnotherString(string Source, 
            string TargetToBeFound) 
            {
            if (PasswordIsCorrect == false
                || SuccessBuiltNew==false)
                {
                return 0;
                }

            if (Source.Length == 0 || TargetToBeFound.Length == 0) 
                {
                return 0;
                }

            if (TargetToBeFound.Length > Source.Length) 
                {
                MessageBox.Show("The length of parameter 'TargetToBeFound' should be shorter than 'Source'.\r\n"
                    + "被查找的字符串参数'TargetToBeFound'应该小于搜索的源字符串 'Source'");
                return 0;
                }

            int TempCount = 0;
            int TempPos = 0;
            TempPos = Strings.InStr(Source ,TargetToBeFound);

            if (TempPos < 1)
                {
                return 0;
                }
            else 
                {
                TempCount = TempCount + 1;
                }
            //减去(搜索到的位置+字符串长度)，等于需要再搜索的字符串长度，可变性
            //或者从找到的位置+字符串长度，开始找下一个匹配的字符串，直到剩余字符串长度小于需要查找的字符串就不再查找
            do
                {
                TempPos = Strings.InStr(TempPos+TargetToBeFound.Length, Source, TargetToBeFound);
                if (TempPos == 0)
                    {
                    return TempCount;
                    }
                else 
                    {
                    TempCount += 1;
                    }
                }
            while((Source.Length-TempPos)>TargetToBeFound.Length);

            return TempCount;
            
            //for (int a = 0; a < ((Source.Length / TargetToBeFound.Length)-1); a++) 
            //    {
            //    }            
            }

        #region "操作系统和文件相关操作"
        
        //获取远程计算机所有进程的名称
        /// <summary>
        /// 获取远程计算机所有进程的名称
        /// </summary>
        /// <param name="RemoteComputerName">远程计算机名称或者IP地址</param>
        /// <returns>远程计算机所有进程的名称数组</returns>
        public string[] GetRemoteProcess(string RemoteComputerName)
            {
            try
                {
                System.Diagnostics.Process[] AllSameNameProcess;
                AllSameNameProcess = System.Diagnostics.Process.GetProcesses(RemoteComputerName);
                string[] ProcessName;

                if (AllSameNameProcess != null)
                    {
                    if (AllSameNameProcess.Length > 0)
                        {
                        ProcessName = new string[AllSameNameProcess.Length];
                        for (int a = 0; a < AllSameNameProcess.Length; a++)
                            {
                            ProcessName[a] = AllSameNameProcess[a].ProcessName + "," + AllSameNameProcess[a].Id.ToString();
                            }
                        }
                    else
                        {
                        return null;
                        }
                    }
                else
                    {
                    return null;
                    }

                return ProcessName;

                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }

        //获取远程计算机上指定名称的进程
        /// <summary>
        /// 获取远程计算机上指定名称的进程
        /// </summary>
        /// <param name="RemoteComputerName">远程计算机名称或者IP地址</param>
        /// <param name="TargetProcessName">需要获取的进程名称</param>
        /// <returns>远程计算机上指定名称进程的名称数组</returns>
        public string[] GetRemoteProcess(string RemoteComputerName, string TargetProcessName)
            {
            try
                {
                System.Diagnostics.Process[] AllSameNameProcess;
                AllSameNameProcess = System.Diagnostics.Process.GetProcessesByName(TargetProcessName,RemoteComputerName);
                string[] ProcessName;

                if (AllSameNameProcess != null)
                    {
                    if (AllSameNameProcess.Length > 0)
                        {
                        ProcessName = new string[AllSameNameProcess.Length];
                        for (int a = 0; a < AllSameNameProcess.Length; a++)
                            {
                            ProcessName[a] = AllSameNameProcess[a].ProcessName + "," + AllSameNameProcess[a].Id.ToString();
                            }
                        }
                    else
                        {
                        return null;
                        }
                    }
                else
                    {
                    return null;
                    }

                return ProcessName;

                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }

        //获取本地计算机所有进程的名称
        /// <summary>
        /// 获取本地计算机所有进程的名称
        /// </summary>
        /// <returns>本地计算机所有进程的名称数组</returns>
        public string[] GetLocalProcess()
            {
            try
                {
                System.Diagnostics.Process[] AllSameNameProcess;
                AllSameNameProcess = System.Diagnostics.Process.GetProcesses();
                string[] ProcessName;

                if (AllSameNameProcess != null)
                    {
                    if (AllSameNameProcess.Length > 0)
                        {
                        ProcessName = new string[AllSameNameProcess.Length];
                        for (int a = 0; a < AllSameNameProcess.Length; a++)
                            {
                            ProcessName[a] = AllSameNameProcess[a].ProcessName + "," + AllSameNameProcess[a].Id.ToString();
                            }
                        }
                    else
                        {
                        return null;
                        }
                    }
                else
                    {
                    return null;
                    }

                return ProcessName;

                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }

        //获取本地计算机上指定名称的进程
        /// <summary>
        /// 获取本地计算机上指定名称的进程
        /// </summary>
        /// <param name="TargetProcessName">需要获取的进程名称</param>
        /// <returns>本地计算机上指定名称进程的名称数组</returns>
        public string[] GetLocalProcess(string TargetProcessName)
            {
            try
                {
                System.Diagnostics.Process[] AllSameNameProcess;
                AllSameNameProcess = System.Diagnostics.Process.GetProcessesByName(TargetProcessName);
                string[] ProcessName;

                if (AllSameNameProcess != null)
                    {
                    if (AllSameNameProcess.Length > 0)
                        {
                        ProcessName = new string[AllSameNameProcess.Length];
                        for (int a = 0; a < AllSameNameProcess.Length; a++)
                            {
                            ProcessName[a] = AllSameNameProcess[a].ProcessName + "," + AllSameNameProcess[a].Id.ToString();
                            }
                        }
                    else
                        {
                        return null;
                        }
                    }
                else
                    {
                    return null;
                    }

                return ProcessName;

                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }        

        //关闭所有同名的进程
        /// <summary>
        /// 关闭所有同名的进程
        /// </summary>
        /// <param name="ProcessName">需要关闭的进程名称</param>
        /// <returns>是否执行成功</returns>
        public bool KillProcess(string ProcessName)
            {
            try
                {
                System.Diagnostics.Process[] AllSameNameProcess;
                System.Diagnostics.Process TempProcess = new System.Diagnostics.Process();
                AllSameNameProcess = System.Diagnostics.Process.GetProcessesByName(ProcessName);

                for (int a = AllSameNameProcess.Length - 1; a >= 0; a--) 
                    {
                    TempProcess = System.Diagnostics.Process.GetProcessById(AllSameNameProcess[a].Id);
                    TempProcess.Kill();//关闭进程
                    TempProcess.WaitForExit();//等待进程退出
                    }
                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //等待指定时间，在线程里面使用，如果在界面里面使用就会阻塞当前进程
        /// <summary>
        /// 等待指定时间，在线程里面使用，如果在界面里面使用就会阻塞当前进程
        /// </summary>
        /// <param name="WaitTimeInMS">等待时间【单位：毫秒】</param>
        /// <returns>等待时间到达</returns>
        public bool Wait(uint WaitTimeInMS)
            {
            try
                {
                System.Threading.Thread.Sleep((int)WaitTimeInMS); 
                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false ;
                }
            }

        //【待完善】截屏：将电脑当前屏幕截取然后弹出保存对话框
        /// <summary>
        /// 【待完善】截屏：将电脑当前屏幕截取然后弹出保存对话框
        /// </summary>
        /// <returns>是否执行成功</returns>
        public bool ScreenShot()
            {
            try
                {
                int Width = 0, Height = 0;

                //*******************
                //System.Windows.Forms - Screen .WorkingArea 属性
                //获取显示器的工作区。工作区是显示器的桌面区域，不包括任务栏、停靠窗口和停靠工具栏。
                System.Windows.Forms.Screen TempScreen = System.Windows.Forms.Screen.PrimaryScreen; ;
                Width = TempScreen.Bounds.Width;//.WorkingArea.Width;
                Height = PC.Screen.Bounds.Height;//.WorkingArea.Height;
                //*******************

                //Width = PC.Screen.Bounds.Width;//.WorkingArea.Width;
                //Height = PC.Screen.Bounds.Height;//.WorkingArea.Height;

                //*******************
                
                //截屏
                Bitmap TempBitMap = new Bitmap(Width, Height);
                Graphics TempGraphic = Graphics.FromImage(TempBitMap);
                TempGraphic.CopyFromScreen(0, 0, 0, 0, new Size(Width, Height));

                SaveFileDialog TempSaveFile = new SaveFileDialog();
                TempSaveFile.AddExtension = true;
                TempSaveFile.CheckPathExists = true;
                TempSaveFile.DefaultExt = "BMP";
                TempSaveFile.Filter = "BMP文件|*.BMP|PNG文件|*.PNG";

                //使用 FilterIndex【第一个筛选器条目的索引值为 1】 属性设置第一个显示给用户的筛选选项。
                TempSaveFile.FilterIndex = 1;

                if (TempSaveFile.ShowDialog() == DialogResult.OK && TempSaveFile.FileName != "")
                    {
                    TempBitMap.Save(TempSaveFile.FileName);
                    TempBitMap.Dispose();
                    TempGraphic.Dispose();
                    TempSaveFile.Dispose();
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

        //获取操作系统的信息
        /// <summary>
        /// 获取操作系统的信息
        /// </summary>
        /// <returns>操作系统信息的数据结构</returns>
        public OS GetOSInfo ()
            {
            OS TempOS = new OS();
            TempOS.AvailablePhysicalMemory = 0;
            TempOS.AvailableVirtualMemory = 0;
            TempOS.OSID = "";
            TempOS.OSName = "";
            TempOS.OSVer = "";
            TempOS.TotalPhysicalMemory = 0;
            TempOS.TotalVirtualMemory = 0;

            try
                {

                //ComputerInfo .AvailableVirtualMemory 属性
                //获取计算机的可用虚拟地址空间的总量。【单位：字节数】

                //ComputerInfo .AvailablePhysicalMemory 属性
                //获取计算机的可用物理内存总量。【单位：字节数】

                //ComputerInfo .TotalVirtualMemory 属性
                //获取计算机的可用虚拟地址空间的总量。【单位：字节数】

                //ComputerInfo .TotalPhysicalMemory 属性
                //获取计算机的物理内存总量。【单位：字节数】
                //此 API 不兼容 CLS。 

                Microsoft.VisualBasic.Devices.ComputerInfo TempPCInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();

                TempOS.OSName = TempPCInfo.OSFullName;
                TempOS.OSID = TempPCInfo.OSPlatform;
                TempOS.OSVer = TempPCInfo.OSVersion;
                TempOS.TotalPhysicalMemory = TempPCInfo.TotalPhysicalMemory / 1024 / 1024;//【字节转换为MB】
                TempOS.TotalVirtualMemory=TempPCInfo.TotalVirtualMemory/ 1024 / 1024;//【字节转换为MB】
                TempOS.AvailablePhysicalMemory=TempPCInfo.AvailablePhysicalMemory/ 1024 / 1024;//【字节转换为MB】
                TempOS.AvailableVirtualMemory=TempPCInfo.AvailableVirtualMemory/ 1024 / 1024;//【字节转换为MB】

                TempPCInfo = null;
                return TempOS;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return TempOS;
                }
            }

        //启用/禁用USB
        /// <summary>
        /// 启用/禁用USB
        /// </summary>
        /// <param name="EnableStatus">true：启用；false：禁用</param>
        /// <returns>是否执行成功</returns>
        public bool EnableUSB(bool EnableStatus)
            {
            string USBBatFileName="C:\\Windows\\Temp\\USBOperation.BAT";
            string CommandStr = "";
            try
                {
                //********************
                //--修改注册表
                
                //-启用:
                //[HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\usbstor]
                //"Type"=dword:00000001
                //"Start"=dword:00000003
                //"ErrorControl"=dword:00000001
                //"DisplayName"="USB 大容量存储设备"
                //"ImagePath"=hex(2):73,00,79,00,73,00,74,00,65,00,6d,00,33,00,32,00,5c,00,44,00,\
                //  52,00,49,00,56,00,45,00,52,00,53,00,5c,00,55,00,53,00,42,00,53,00,54,00,4f,\
                //  00,52,00,2e,00,53,00,59,00,53,00,00,00
                
                //[HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\usbstor\Enum]
                //"Count"=dword:00000001
                //"NextInstance"=dword:00000001
                //"0"="USB\\Vid_0951&Pid_1642\\001CC0EC34C2AC40E70E3310"
                
                //-禁用:
                //[HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer]
                //"NoDriveTypeAutoRun"=dword:000000dd
                //*********************
                
                //******************
                //--BAT文件
                
                //-启用:
                //reg add HKLM\SYSTEM\CurrentControlSet\Services\UsbStor /v Start /t REG_DWORD /d 3 /f 
                //reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies /v WriteProtect /t reg_dword /d 00000000 /f
                //copy c:\windows\usbstor.pnf c:\windows\inf\ /y
                //copy c:\windows\usbstor.inf c:\windows\inf\ /y
                
                //-禁用:
                //reg add HKLM\SYSTEM\CurrentControlSet\Services\UsbStor /v Start /t REG_DWORD /d 4 /f 
                //reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies /v WriteProtect /t reg_dword /d 00000001 /f
                //copy c:\windows\inf\usbstor.pnf c:\windows\ /y
                //copy c:\windows\inf\usbstor.inf c:\windows\ /y
                //del c:\windows\inf\usbstor.pnf /q
                //del c:\windows\inf\usbstor.inf /q                
                //******************

                //1、清除原有BAT文件记录
                if (File.Exists(USBBatFileName) == true) 
                    {
                    File.Delete(USBBatFileName);
                    }

                //2、依照需要进行BAT批处理命令编辑
                if (EnableStatus == true)
                    {
                    //启用USB
                    //如果前面不加  "" &，则在执行的时候会出错，具体原因不知道，
                    //只是在执行命令时reg的r就变成乱码了，BAT执行就报错
                    CommandStr = "\r\n" + "reg add HKLM\\SYSTEM\\CurrentControlSet\\Services\\UsbStor /v Start /t REG_DWORD /d 3 /f \r\n"
                        + "reg add HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\StorageDevicePolicies /v WriteProtect /t reg_dword /d 00000000 /f \r\n"
                    + "copy c:\\windows\\usbstor.pnf c:\\windows\\inf\\ /y \r\n" + "copy c:\\windows\\usbstor.inf c:\\windows\\inf\\ /y \r\n"
                    + "Del " + USBBatFileName;   // + "pause"
                    }
                else 
                    {
                    //禁用USB
                    //如果前面不加  "" &，则在执行的时候会出错，具体原因不知道，
                    //只是在执行命令时reg的r就变成乱码了，BAT执行就报错
                    CommandStr = "\r\n" + "reg add HKLM\\SYSTEM\\CurrentControlSet\\Services\\UsbStor /v Start /t REG_DWORD /d 4 /f \r\n"
                        + "reg add HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\StorageDevicePolicies /v WriteProtect /t reg_dword /d 00000001 /f \r\n"
                    + "copy c:\\windows\\inf\\usbstor.pnf c:\\windows\\ /y \r\n" + "copy c:\\windows\\inf\\usbstor.inf c:\\windows\\ /y \r\n"
                    + "del c:\\windows\\inf\\usbstor.pnf /q \r\n" + "del c:\\windows\\inf\\usbstor.inf /q \r\n" + "Del " + USBBatFileName; //+ "pause"
                    }

                //3、将BAT批处理命令写入至文件
                //保存的中文会乱码，不支持GB码
                //Microsoft.VisualBasic.FileIO.FileSystem.WriteAllText(USBBatFileName, CommandStr, false, System.Text.Encoding.ASCII); //System.Text.Encoding.GetEncoding(936))

                //保存的中文不会乱码，但是在DOS环境下会乱码
                PC.FileSystem.WriteAllText(USBBatFileName, CommandStr, false);

                //4、执行BAT批处理命令
                Microsoft.VisualBasic.Interaction.Shell(USBBatFileName, Microsoft.VisualBasic.AppWinStyle.Hide); //.NormalFocus)

                //5、删除BAT批处理文件
                if (File.Exists(USBBatFileName) == true) 
                    {
                    File.Delete(USBBatFileName);
                    }

                if (EnableStatus == true)
                    {
                    EnabledUSB = true;
                    }
                else 
                    {
                    EnabledUSB = false;
                    }

                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //将字符串写入到文本文件，会自动添加文件扩展名和回车换行符
        /// <summary>
        /// 将字符串写入到文本文件，会自动添加文件扩展名
        /// </summary>
        /// <param name="TXTToBeSaved">需要被保存的文本字符串</param>
        /// <param name="TXTFileName">文本文件名称，可以不包括 '.TXT' 文件扩展名</param>
        /// <param name="AddToOriginalFile">是否添加到原有文件，默认是</param>
        /// <returns>是否执行成功</returns>
        public bool SaveStringToTXTFile(string TXTToBeSaved, string TXTFileName, bool AddToOriginalFile = true)
            {
            try
                {
                if (TXTToBeSaved == "") 
                    {
                    ErrorMessage = "需要被保存的文本字符串不能为空";
                    return false;
                    }

                //TXTToBeSaved += "\r\n";

                if (TXTFileName == "")
                    {
                    ErrorMessage = "需要保存的文本名称不能为空";
                    return false;
                    }

                //如果文件名称中没有".TXT"，就添加
                if (TXTFileName.ToUpper().IndexOf(".TXT") == -1) 
                    {
                    TXTFileName = TXTFileName + ".txt";
                    }

                //**********************
                //FileSystemProxy .WriteAllText 方法 (String, String, Boolean)
                //FileSystemProxy .WriteAllText 方法 (String, String, Boolean, Encoding)
                //向文件写入文本
                //使用 UTF-8 编码方式来写入此文件。若要指定其他编码，请使用 WriteAllText () 方法的其他重载。

                //如果指定的文件不存在，则创建该文件。

                //如果指定的编码方式与文件的现有编码方式不匹配，则忽略指定的编码方式。
                //WriteAllText 方法将打开一个文件，向其写入内容，然后将其关闭。 
                //使用 WriteAllText 方法的代码比使用 StreamWriter 对象的代码更加简单。 
                //但是，如果您使用循环将字符串添加到文件中，则 StreamWriter 对象能够提供更优异的性能，因为您只需打开和关闭该文件一次。

                ////用中文GB2312保存，或者UTF-8也行
                //PC.FileSystem.WriteAllText(TXTFileName, TXTToBeSaved, AddToOriginalFile,Encoding.GetEncoding("GB2312"));
                ////PC.FileSystem.WriteAllText(TXTFileName, TXTToBeSaved, AddToOriginalFile, Encoding.UTF8);
                
                //**********************
                //WriteLine(String)	将后跟行结束符的字符串写入文本流。 （继承自 TextWriter。）
                System.IO.StreamWriter TempStreamWriter = new StreamWriter(TXTFileName, AddToOriginalFile, Encoding.GetEncoding(936));//与Encoding.GetEncoding("GB2312")等效
                TempStreamWriter.WriteLine(TXTToBeSaved);
                TempStreamWriter.Flush();
                TempStreamWriter.Close();
                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //弹出“保存文件”对话框，将字符串写入到文本文件，会自动添加文件扩展名和回车换行符
        /// <summary>
        /// 弹出“保存文件”对话框，将字符串写入到文本文件，会自动添加文件扩展名
        /// </summary>
        /// <param name="TXTToBeSaved">需要被保存的文本字符串</param>
        /// <param name="TXTFileName">文本文件名称，可以不包括 '.TXT' 文件扩展名</param>
        /// <param name="AddToOriginalFile">是否添加到原有文件，默认是</param>
        /// <returns>是否执行成功</returns>
        public bool SaveStringToTXTFile(string TXTToBeSaved, bool AddToOriginalFile = true)
            {
            string TXTFileName = "";
            try
                {
                if (TXTToBeSaved == "") 
                    {
                    ErrorMessage = "需要被保存的文本字符串不能为空";
                    return false;
                    }

                //TXTToBeSaved += "\r\n";

                OpenFileDialog TempOpenFile = new OpenFileDialog();
                TempOpenFile.Title = "保存文本文件";
                TempOpenFile.DefaultExt = "txt";
                TempOpenFile.Filter = "TXT文本文件|*.TXT";
                TempOpenFile.CheckFileExists = true;

                if (TempOpenFile.ShowDialog() == DialogResult.No || TempOpenFile.FileName == "")
                    {
                    return false;
                    }

                TXTFileName = TempOpenFile.FileName;

                if (TXTFileName == "")
                    {
                    ErrorMessage = "需要保存的文本名称不能为空";
                    return false;
                    }

                //如果文件名称中没有".TXT"，就添加
                if (TXTFileName.ToUpper().IndexOf(".TXT") == -1) 
                    {
                    TXTFileName = TXTFileName + ".txt";
                    }

                //**********************
                //FileSystemProxy .WriteAllText 方法 (String, String, Boolean)
                //FileSystemProxy .WriteAllText 方法 (String, String, Boolean, Encoding)
                //向文件写入文本
                //使用 UTF-8 编码方式来写入此文件。若要指定其他编码，请使用 WriteAllText () 方法的其他重载。

                //如果指定的文件不存在，则创建该文件。

                //如果指定的编码方式与文件的现有编码方式不匹配，则忽略指定的编码方式。
                //WriteAllText 方法将打开一个文件，向其写入内容，然后将其关闭。 
                //使用 WriteAllText 方法的代码比使用 StreamWriter 对象的代码更加简单。 
                //但是，如果您使用循环将字符串添加到文件中，则 StreamWriter 对象能够提供更优异的性能，因为您只需打开和关闭该文件一次。

                ////用中文GB2312保存，或者UTF-8也行
                //PC.FileSystem.WriteAllText(TXTFileName, TXTToBeSaved, AddToOriginalFile,Encoding.GetEncoding("GB2312"));
                ////PC.FileSystem.WriteAllText(TXTFileName, TXTToBeSaved, AddToOriginalFile, Encoding.UTF8);
                
                //**********************
                //WriteLine(String)	将后跟行结束符的字符串写入文本流。 （继承自 TextWriter。）
                System.IO.StreamWriter TempStreamWriter = new StreamWriter(TXTFileName, AddToOriginalFile, Encoding.GetEncoding(936));//与Encoding.GetEncoding("GB2312")等效
                TempStreamWriter.WriteLine(TXTToBeSaved);
                TempStreamWriter.Flush();
                TempStreamWriter.Close(); 
                TempStreamWriter.Dispose();
                TempOpenFile.Dispose();
                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //将字符串数组写入到文本文件，会自动添加文件扩展名和回车换行符
        /// <summary>
        /// 将字符串数组写入到文本文件，会自动添加文件扩展名
        /// </summary>
        /// <param name="TXTToBeSaved">需要被保存的文本字符串数组</param>
        /// <param name="TXTFileName">文本文件名称，可以不包括 '.TXT' 文件扩展名</param>
        /// <param name="AddToOriginalFile">是否添加到原有文件，默认是</param>
        /// <returns>是否执行成功</returns>
        public bool SaveStringToTXTFile(string[] TXTToBeSaved, string TXTFileName, bool AddToOriginalFile = true)
            {
            try
                {
                if (TXTToBeSaved == null)
                    {
                    ErrorMessage = "需要被保存的文本字符串数组不能为空";
                    return false;
                    }

                //TXTToBeSaved += "\r\n";

                if (TXTFileName == "")
                    {
                    ErrorMessage = "需要保存的文本名称不能为空";
                    return false;
                    }

                //如果文件名称中没有".TXT"，就添加
                if (TXTFileName.ToUpper().IndexOf(".TXT") == -1)
                    {
                    TXTFileName = TXTFileName + ".txt";
                    }
                
                //**********************
                //WriteLine(String)	将后跟行结束符的字符串写入文本流。 （继承自 TextWriter。）
                System.IO.StreamWriter TempStreamWriter = new StreamWriter(TXTFileName, AddToOriginalFile, Encoding.GetEncoding(936));//与Encoding.GetEncoding("GB2312")等效
                for (int a = 0; a < TXTToBeSaved.Length; a++) 
                    {
                    //此处不添加判断某个[]是否为""，这样对于处理有规律数据在读取的时候有很大好处
                    TempStreamWriter.WriteLine(TXTToBeSaved[a]);
                    }
                
                TempStreamWriter.Flush();
                TempStreamWriter.Close();
                TempStreamWriter.Dispose();
                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }
        
        //弹出“保存文件”对话框，将字符串数组写入到文本文件，会自动添加文件扩展名和回车换行符
        /// <summary>
        /// 弹出“保存文件”对话框，将字符串数组写入到文本文件，会自动添加文件扩展名
        /// </summary>
        /// <param name="TXTToBeSaved">需要被保存的文本字符串数组</param>
        /// <param name="TXTFileName">文本文件名称，可以不包括 '.TXT' 文件扩展名</param>
        /// <param name="AddToOriginalFile">是否添加到原有文件，默认是</param>
        /// <returns>是否执行成功</returns>
        public bool SaveStringToTXTFile(string[] TXTToBeSaved, bool AddToOriginalFile = true)
            {
            string TXTFileName = "";
            try
                {
                if (TXTToBeSaved == null)
                    {
                    ErrorMessage = "需要被保存的文本字符串数组不能为空";
                    return false;
                    }

                OpenFileDialog TempOpenFile = new OpenFileDialog();
                TempOpenFile.Title = "保存文本文件";
                TempOpenFile.DefaultExt = "txt";
                TempOpenFile.Filter = "TXT文本文件|*.TXT";
                TempOpenFile.CheckFileExists = true;

                if (TempOpenFile.ShowDialog() == DialogResult.No || TempOpenFile.FileName == "")
                    {
                    return false;
                    }

                TXTFileName = TempOpenFile.FileName;

                //TXTToBeSaved += "\r\n";

                if (TXTFileName == "")
                    {
                    ErrorMessage = "需要保存的文本名称不能为空";
                    return false;
                    }

                //如果文件名称中没有".TXT"，就添加
                if (TXTFileName.ToUpper().IndexOf(".TXT") == -1)
                    {
                    TXTFileName = TXTFileName + ".txt";
                    }
                
                //**********************
                //WriteLine(String)	将后跟行结束符的字符串写入文本流。 （继承自 TextWriter。）
                System.IO.StreamWriter TempStreamWriter = new StreamWriter(TXTFileName, AddToOriginalFile, Encoding.GetEncoding(936));//与Encoding.GetEncoding("GB2312")等效
                for (int a = 0; a < TXTToBeSaved.Length; a++) 
                    {
                    //此处不添加判断某个[]是否为""，这样对于处理有规律数据在读取的时候有很大好处
                    TempStreamWriter.WriteLine(TXTToBeSaved[a]);
                    }
                
                TempStreamWriter.Flush();
                TempStreamWriter.Close();
                TempStreamWriter.Dispose();
                TempOpenFile.Dispose();
                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //将RichTextBox控件的字符串写入到文本文件【如果此文件存在就会覆盖原文件】，会自动添加文件扩展名
        /// <summary>
        /// 将RichTextBox控件的字符串写入到文本文件【如果此文件存在就会覆盖原文件】，会自动添加文件扩展名
        /// </summary>
        /// <param name="TargetRichTextBox">需要被保存的文本字符串RichTextBox控件</param>
        /// <param name="TXTFileName">文本文件名称，可以不包括 '.TXT' 文件扩展名</param>
        /// <returns>是否执行成功</returns>
        public bool SaveStringToTXTFile(ref RichTextBox TargetRichTextBox, string TXTFileName)
            {
            try
                {
                if (TargetRichTextBox.Text == "")
                    {
                    ErrorMessage = "需要被保存的文本字符串不能为空";
                    return false;
                    }

                if (TXTFileName == "")
                    {
                    ErrorMessage = "需要保存的文本名称不能为空";
                    return false;
                    }

                //如果文件名称中没有".TXT"，就添加
                if (TXTFileName.ToUpper().IndexOf(".TXT") == -1)
                    {
                    TXTFileName = TXTFileName + ".txt";
                    }

                TargetRichTextBox.SaveFile(TXTFileName, RichTextBoxStreamType.TextTextOleObjs);//UnicodePlainText
                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //弹出“保存文件”对话框，将RichTextBox控件的字符串写入到文本文件【如果此文件存在就会覆盖原文件】，会自动添加文件扩展名
        /// <summary>
        /// 弹出“保存文件”对话框，将RichTextBox控件的字符串写入到文本文件【如果此文件存在就会覆盖原文件】，会自动添加文件扩展名
        /// </summary>
        /// <param name="TargetRichTextBox">需要被保存的文本字符串RichTextBox控件</param>
        /// <param name="TXTFileName">文本文件名称，可以不包括 '.TXT' 文件扩展名</param>
        /// <returns>是否执行成功</returns>
        public bool SaveStringToTXTFile(ref RichTextBox TargetRichTextBox)
            {
            string TXTFileName = "";
            try
                {
                if (TargetRichTextBox == null)
                    {
                    ErrorMessage = "文本控件不能为空";
                    return false;
                    }

                if (TargetRichTextBox.Text == "")
                    {
                    ErrorMessage = "需要被保存的文本字符串不能为空";
                    return false;
                    }

                OpenFileDialog TempOpenFile = new OpenFileDialog();
                TempOpenFile.Title = "保存文本文件";
                TempOpenFile.DefaultExt = "txt";
                TempOpenFile.Filter = "TXT文本文件|*.TXT";
                TempOpenFile.CheckFileExists = true;

                if (TempOpenFile.ShowDialog() == DialogResult.No || TempOpenFile.FileName == "")
                    {
                    return false;
                    }

                TXTFileName = TempOpenFile.FileName;

                if (TXTFileName == "")
                    {
                    ErrorMessage = "需要保存的文本名称不能为空";
                    return false;
                    }

                //如果文件名称中没有".TXT"，就添加
                if (TXTFileName.ToUpper().IndexOf(".TXT") == -1)
                    {
                    TXTFileName = TXTFileName + ".txt";
                    }

                TargetRichTextBox.SaveFile(TXTFileName, RichTextBoxStreamType.TextTextOleObjs);//UnicodePlainText
                TempOpenFile.Dispose();
                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //从TXT文本文件读取字符串，以行为单位，跳过空行
        /// <summary>
        /// 从TXT文本文件读取字符串，以行为单位，跳过空行
        /// </summary>
        /// <param name="TXTFileName">TXT文本文件名称</param>
        /// <returns></returns>
        public string[] ReadLinesFromTXTFile(string TXTFileName)
            {
            string[] ReadLines = { "" };
            try
                {
                if (TXTFileName == "")
                    {
                    ErrorMessage = "需要保存的文本名称不能为空";
                    return null;
                    }

                //如果文件名称中没有".TXT"，就添加
                if (TXTFileName.ToUpper().IndexOf(".TXT") == -1)
                    {
                    TXTFileName = TXTFileName + ".txt";
                    }

                if (PC.FileSystem.FileExists(TXTFileName) == false) 
                    {
                    ErrorMessage = "指定的文本文件 " + TXTFileName + " 不存在";
                    return null;
                    }
                object LockObject = new object();

                ////**************************
                //【不支持中文】
                //Microsoft.VisualBasic.FileIO.TextFieldParser TempTXTReader = new Microsoft.VisualBasic.FileIO.TextFieldParser(TXTFileName);

                //lock (LockObject) 
                //    {
                //    //字段是分隔的
                //    //TempTXTReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;//.FixedWidth固定宽度的

                //    //以空格为分格符读取
                //    //TempTXTReader.SetDelimiters(" ");

                //    int Lines = 0;
                //    string TempStr = "";

                //    //如果在当前光标位置到文件末尾之间没有非空、非注释行，则返回 True。
                //    while (!TempTXTReader.EndOfData) 
                //        {
                //        TempStr = TempTXTReader.ReadLine();
                //        if (TempStr != null && TempStr != "\r\n" 
                //            && TempStr != "\r" && TempStr != "\n"
                //            && TempStr != "")
                //            {
                //            //重新设置数组大小，之前的值保留
                //            Array.Resize<string>(ref ReadLines, Lines + 1);

                //            ReadLines[Lines] = TempStr;
                //            Lines += 1;
                //            }                        
                //        }
                //    }
                
                //**************************
                //【支持中文】
                StreamReader TempSteamReader = new StreamReader(TXTFileName, Encoding.GetEncoding(936));
                lock (LockObject)
                    {
                    int Lines = 0;
                    string TempStr = "";

                    //如果在当前光标位置到文件末尾之间没有非空、非注释行，则返回 True。
                    while (!TempSteamReader.EndOfStream)
                        {
                        TempStr = TempSteamReader.ReadLine();
                        if (TempStr != null && TempStr != "\r\n"
                            && TempStr != "\r" && TempStr != "\n"
                            && TempStr != "")
                            {
                            //重新设置数组大小，之前的值保留
                            Array.Resize<string>(ref ReadLines, Lines + 1);

                            ReadLines[Lines] = TempStr;
                            Lines += 1;
                            }
                        }
                    }
                LockObject = null;
                TempSteamReader.Close();
                TempSteamReader.Dispose();
                return ReadLines;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return ReadLines;
                }
            }

        //弹出“打开文件”对话框，从TXT文本文件读取字符串，以行为单位，跳过空行
        /// <summary>
        /// 弹出“打开文件”对话框，从TXT文本文件读取字符串，以行为单位，跳过空行
        /// </summary>
        /// <param name="TXTFileName">TXT文本文件名称</param>
        /// <returns></returns>
        public string[] ReadLinesFromTXTFile()
            {
            string[] ReadLines = { "" };
            string TXTFileName = "";
            try
                {
                OpenFileDialog TempOpenFile = new OpenFileDialog();
                TempOpenFile.Title = "打开文本文件";
                TempOpenFile.DefaultExt = "txt";
                TempOpenFile.Filter = "TXT文本文件|*.TXT";
                TempOpenFile.CheckFileExists = true;

                if (TempOpenFile.ShowDialog() == DialogResult.No || TempOpenFile.FileName == "") 
                    {
                    return null;
                    }

                TXTFileName = TempOpenFile.FileName;

                if (TXTFileName == "")
                    {
                    ErrorMessage = "需要保存的文本名称不能为空";
                    return null;
                    }

                //如果文件名称中没有".TXT"，就添加
                if (TXTFileName.ToUpper().IndexOf(".TXT") == -1)
                    {
                    TXTFileName = TXTFileName + ".txt";
                    }

                if (PC.FileSystem.FileExists(TXTFileName) == false)
                    {
                    ErrorMessage = "指定的文本文件 " + TXTFileName + " 不存在";
                    return null;
                    }
                object LockObject = new object();

                ////**************************
                //【不支持中文】
                //Microsoft.VisualBasic.FileIO.TextFieldParser TempTXTReader = new Microsoft.VisualBasic.FileIO.TextFieldParser(TXTFileName);

                //lock (LockObject) 
                //    {
                //    //字段是分隔的
                //    //TempTXTReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;//.FixedWidth固定宽度的

                //    //以空格为分格符读取
                //    //TempTXTReader.SetDelimiters(" ");

                //    int Lines = 0;
                //    string TempStr = "";

                //    //如果在当前光标位置到文件末尾之间没有非空、非注释行，则返回 True。
                //    while (!TempTXTReader.EndOfData) 
                //        {
                //        TempStr = TempTXTReader.ReadLine();
                //        if (TempStr != null && TempStr != "\r\n" 
                //            && TempStr != "\r" && TempStr != "\n"
                //            && TempStr != "")
                //            {
                //            //重新设置数组大小，之前的值保留
                //            Array.Resize<string>(ref ReadLines, Lines + 1);

                //            ReadLines[Lines] = TempStr;
                //            Lines += 1;
                //            }                        
                //        }
                //    }

                //**************************
                //【支持中文】
                StreamReader TempSteamReader = new StreamReader(TXTFileName, Encoding.GetEncoding(936));
                lock (LockObject)
                    {
                    int Lines = 0;
                    string TempStr = "";

                    //如果在当前光标位置到文件末尾之间没有非空、非注释行，则返回 True。
                    while (!TempSteamReader.EndOfStream)
                        {
                        TempStr = TempSteamReader.ReadLine();
                        if (TempStr != null && TempStr != "\r\n"
                            && TempStr != "\r" && TempStr != "\n"
                            && TempStr != "")
                            {
                            //重新设置数组大小，之前的值保留
                            Array.Resize<string>(ref ReadLines, Lines + 1);

                            ReadLines[Lines] = TempStr;
                            Lines += 1;
                            }
                        }
                    }

                TempOpenFile.Dispose();
                LockObject = null;
                TempSteamReader.Close();
                TempSteamReader.Dispose();
                return ReadLines;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return ReadLines;
                }
            }

        //复制源文件到目标文件【写2进制方式】
        /// <summary>
        /// 复制源文件到目标文件【写2进制方式】
        /// </summary>
        /// <param name="SourceFile">源文件</param>
        /// <param name="TargetFile">目标文件</param>
        /// <param name="OverWrite">是否覆盖</param>
        /// <returns>是否执行成功</returns>
        public bool CopyFileByBinary(string SourceFile,string TargetFile,
            bool OverWrite = false)
            {
            try
                {
                if (SourceFile == "") 
                    {
                    ErrorMessage = "The parameter 'SourceFile' can't be empty, please check and revise it.";
                    return false;
                    }

                if (TargetFile == "")
                    {
                    ErrorMessage = "The parameter 'TargetFile' can't be empty, please check and revise it.";
                    return false;
                    }

                if (System.IO.File.Exists(SourceFile) == false) 
                    {
                    ErrorMessage = "The source file '" + SourceFile + "' doesn't exist, please check.";
                    return false;
                    }

                //执行覆盖写操作，所以先删除原有目标文件
                if (OverWrite == true && System.IO.File.Exists(TargetFile) == true) 
                    {
                    System.IO.File.Delete(TargetFile);
                    }

                //如果不是执行覆盖写操作，且目标文件不存在就新建文件
                if (OverWrite == false && System.IO.File.Exists(TargetFile) == false)
                    {
                    System.IO.File.Create(TargetFile);
                    }

                byte[] WriteBuffer = new byte[1024];
                FileStream OpenFileStream = System.IO.File.Open(SourceFile, FileMode.Open);

                //按照一定长度读取源文件的字节并写入到目标文件中
                while (OpenFileStream.Read(WriteBuffer, 0, 1024) > 0) 
                    {
                    PC.FileSystem.WriteAllBytes(TargetFile, WriteBuffer, true);
                    }
                OpenFileStream.Close();
                OpenFileStream.Dispose();
                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }
                
        //获取计算机中所有磁盘驱动器的相关信息
        /// <summary>
        /// 获取计算机中所有磁盘驱动器的相关信息
        /// </summary>
        /// <returns></returns>
        public DriveMessage[] GetDriveMessage()
            {

            System.IO.DriveInfo[] TempDriveInfo = System.IO.DriveInfo.GetDrives();
            DriveMessage[] TempDriveMessage = new DriveMessage[TempDriveInfo.Length];

            for (int a = 0; a < TempDriveInfo.Length; a++)
                {
                TempDriveMessage[a].FileSystem = "";
                TempDriveMessage[a].Name = "";
                TempDriveMessage[a].TotalFreeSpaceInMB = 0;
                TempDriveMessage[a].TotalSpaceInMB = 0;
                TempDriveMessage[a].VolumeLabel = "";
                }

            try
                {

                //DriveInfo .DriveType 属性 -- 获取驱动器类型
                //DriveType 属性指示驱动器是否是以下任意类型： CDRom、 Fixed、 Unknown、 Network、
                //NoRootDirectory、 Ram、 Removable 或 Unknown。 值在 DriveType 枚举中列出。

                //DriveInfo .DriveFormat 属性
                //获取文件系统的名称，例如 NTFS 或 FAT32

                //DriveInfo .AvailableFreeSpace 属性  -- long 
                //指示驱动器上的可用空闲空间量（以字节为单位）
                //因为此属性将磁盘配额考虑在内，所以此数量可能与 TotalFreeSpace 数量不同

                //DriveInfo .TotalSize 属性  -- long 
                //此属性指示驱动器的总大小（以字节为单位），而不只是当前用户可用的驱动器大小

                //DriveInfo .Name 属性
                //获取驱动器的名称
                //此属性是分配给驱动器的名称，例如 C:\ 或 E:\

                //DriveInfo .TotalFreeSpace 属性  -- long 
                //驱动器上的可用空闲空间总量（以字节为单位）
                //此属性指示驱动器上的可用空闲空间总量，而不只是当前用户可用的空闲空间量

                //DriveInfo .VolumeLabel 属性
                //获取或设置驱动器的卷标
                //卷标的长度由操作系统确定。例如，NTFS 允许卷标最长达到 32 个字符。注意， null 是有效的 VolumeLabel

                for (int a = 0; a < TempDriveInfo.Length; a++)
                    {
                    TempDriveMessage[a].FileSystem = TempDriveInfo[a].DriveFormat;
                    TempDriveMessage[a].Name = TempDriveInfo[a].Name[0].ToString().ToUpper();//返回的Name是C:\\，所以取第一个字符作为盘符
                    //TempDriveMessage[a].TotalFreeSpaceInMB = (uint)(TempDriveInfo[a].AvailableFreeSpace / 1024 / 1024);
                    TempDriveMessage[a].TotalFreeSpaceInMB = (double)(TempDriveInfo[a].TotalFreeSpace / 1024 / 1024);
                    TempDriveMessage[a].TotalSpaceInMB = (double)(TempDriveInfo[a].TotalSize / 1024 / 1024);
                    TempDriveMessage[a].VolumeLabel = TempDriveInfo[a].VolumeLabel;
                    }
                TempDriveInfo = null;
                return TempDriveMessage;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return TempDriveMessage;
                }
            }

        //获取计算机中某个磁盘驱动器的剩余空间【单位：GB】
        /// <summary>
        /// 获取计算机中某个磁盘驱动器的剩余空间【单位：GB】
        /// </summary>
        /// <param name="TargetDrive">目标磁盘驱动器</param>
        /// <returns>磁盘驱动器的剩余空间【单位：GB】，如果是0就是驱动器满或者不存在</returns>
        public double GetDriveFreeSpaceInGB(char TargetDrive)
            {
            double FreeSpace = 0;
            try
                {
                //A的ASCII码是65
                //Z的ASCII码是90

                if (Strings.Asc(TargetDrive.ToString().ToUpper()) < 65
                    || Strings.Asc(TargetDrive.ToString().ToUpper()) > 90) 
                    {
                    ErrorMessage = "指定的磁盘驱动器盘符不正确，正确范围：A~Z";
                    return 0;
                    }

                DriveMessage[] TempDriveMessage = GetDriveMessage();
                bool FoundDrive = false;
                string DriveName = TargetDrive.ToString().ToUpper();
                for (int a = 0; a < TempDriveMessage.Length; a++)
                    {
                    if (TempDriveMessage[a].Name.IndexOf(DriveName) != -1) 
                        {
                        FreeSpace = TempDriveMessage[a].TotalFreeSpaceInMB / 1024;
                        FoundDrive = true;
                        break;
                        }
                    }

                if (FoundDrive == false) 
                    {
                    ErrorMessage = "指定的磁盘驱动器盘符不存在";
                    return 0;
                    }
                TempDriveMessage = null;
                return FreeSpace;//Strings.Format(FreeSpace,"##########.##")
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return FreeSpace;
                }
            }

        //获取计算机中某个磁盘驱动器的剩余空间【单位：MB】
        /// <summary>
        /// 获取计算机中某个磁盘驱动器的剩余空间【单位：MB】
        /// </summary>
        /// <param name="TargetDrive">目标磁盘驱动器</param>
        /// <returns>磁盘驱动器的剩余空间【单位：MB】，如果是0就是驱动器满或者不存在</returns>
        public double GetDriveFreeSpaceInMB(char TargetDrive)
            {
            double FreeSpace = 0;
            try
                {
                //A的ASCII码是65
                //Z的ASCII码是90

                if (Strings.Asc(TargetDrive.ToString().ToUpper()) < 65
                    || Strings.Asc(TargetDrive.ToString().ToUpper()) > 90)
                    {
                    ErrorMessage = "指定的磁盘驱动器盘符不正确，正确范围：A~Z";
                    return 0;
                    }

                DriveMessage[] TempDriveMessage = GetDriveMessage();
                bool FoundDrive = false;
                string DriveName = TargetDrive.ToString().ToUpper();
                for (int a = 0; a < TempDriveMessage.Length; a++)
                    {
                    if (TempDriveMessage[a].Name.IndexOf(DriveName) != -1)
                        {
                        FreeSpace = TempDriveMessage[a].TotalFreeSpaceInMB;
                        FoundDrive = true;
                        break;
                        }
                    }

                if (FoundDrive == false)
                    {
                    ErrorMessage = "指定的磁盘驱动器盘符不存在";
                    return 0;
                    }

                TempDriveMessage = null;
                return FreeSpace;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return FreeSpace;
                }
            }

        //Ping服务器并指定超时
        /// <summary>
        /// Ping服务器并指定超时
        /// </summary>
        /// <param name="TargetRemoteServer">服务器IP地址或者URI或者计算机名称</param>
        /// <param name="PingOverTime">连接超时【单位：毫秒】</param>
        /// <returns>是否连接成功</returns>
        public bool Ping(string TargetRemoteServer, uint PingOverTime = 10)
            {
            try
                {
                if (PC.Network.Ping(TargetRemoteServer, (int)PingOverTime) == true)
                    {
                    return true;
                    }
                else 
                    {
                    ErrorMessage = "没有建立网络连接，或者服务器IP地址/URI/计算机名称不正确";
                    return false;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //Ping服务器并指定超时，显示提示信息到RichTextBox控件
        /// <summary>
        /// Ping服务器并指定超时，显示提示信息到RichTextBox控件
        /// </summary>
        /// <param name="TargetRemoteServer">服务器IP地址或者URI或者计算机名称</param>
        /// <param name="TargetRichTextBox">显示提示信息的RichTextBox控件</param>
        /// <param name="PingOverTime">连接超时【单位：毫秒】</param>
        /// <returns>是否连接成功</returns>
        public bool Ping(string TargetRemoteServer, ref RichTextBox TargetRichTextBox,
            uint PingOverTime = 10)
            {
            try
                {
                if (PC.Network.Ping(TargetRemoteServer, (int)PingOverTime) == true)
                    {
                    return true;
                    }
                else 
                    {
                    ErrorMessage = "没有建立网络连接，或者服务器IP地址/URI/计算机名称不正确";
                    AddRichText(ref TargetRichTextBox, "没有建立网络连接，或者服务器IP地址/URI/计算机名称不正确");
                    return false;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                AddRichText(ref TargetRichTextBox, ex.Message);
                return false;
                }
            }

        //重新设置Image的尺寸
        /// <summary>
        /// 重新设置Image的尺寸
        /// </summary>
        /// <param name="OriginalImage">原图像Image</param>
        /// <param name="NewWidth">新宽度</param>
        /// <param name="NewHeight">新高度</param>
        /// <returns></returns>
        public Image ResizeImage(Image OriginalImage, uint NewWidth, uint NewHeight)
            {
            Image TempImage = null;
            try
                {
                TempImage = new System.Drawing.Bitmap(OriginalImage);
                TempImage = new System.Drawing.Bitmap(TempImage, (int)NewWidth, (int)NewHeight);
                return TempImage;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return TempImage;
                }
            }

        //保存Image图像至BMP文件
        /// <summary>
        /// 保存Image图像至BMP文件
        /// </summary>
        /// <param name="TargetImage">目标图像Image对象</param>
        /// <param name="FileName">文件名称</param>
        /// <returns>是否成功保存</returns>
        public bool SaveImageToFile(Image TargetImage, string FileName)//, System.Drawing.Imaging.ImageFormat【不能继承】 TargetImageFormat=System.Drawing.Imaging.ImageFormat.Bmp)
            {
            Image TempImage = null;
            try
                {
                if (TargetImage == null) 
                    {
                    ErrorMessage = "The parameter 'TargetImage' is null";
                    return false;
                    }

                if (FileName == "")
                    {
                    ErrorMessage = "The parameter 'FileName' can't be empty";
                    return false;
                    }

                if (FileName.ToUpper().IndexOf(".BMP") == -1)
                    {
                    FileName = FileName + ".BMP";
                    }

                TempImage = new System.Drawing.Bitmap(TargetImage);
                TempImage.Save(FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                TempImage.Dispose();
                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }






        
        #endregion

        #region "Excel快速添加数据"

        //************************
        //利用在字符串中用 Tab 键作为分隔符，写入EXCEL后，
        //EXCEL自动以用 Tab 键作为分隔符进行跳列，以回车换行符进行换行
        //快速添加单行数据到Excel文件【写单行数据】
        /// <summary>
        /// 快速添加单行数据到Excel文件【写单行数据】
        /// </summary>
        /// <param name="ExcelFileName">保存的Excel文件名称</param>
        /// <param name="FileType">Excel文件保存类型</param>
        /// <param name="DataToBeSaved">需要保存的数据字符串数组</param>
        /// <param name="OverWrite">是否覆盖原有Excel文件</param>
        /// <returns></returns>
        public bool QuickAppendSingleRowDataToExcel(string ExcelFileName,
            ExcelFileType FileType, string[] DataToBeSaved, bool OverWrite = false)
            {
            bool TempJudgement = false;
            try
                {
                if (ExcelFileName == "")
                    {
                    //MessageBox.Show("The parameter 'ExcelFileName' can't be empty, please input the correct file name.");
                    ErrorMessage = "The parameter 'ExcelFileName' can't be empty, please input the correct file name.";
                    return false;
                    }

                string TempExcelFileName = ExcelFileName;
                string StrRecordsToBeSavedExcel = "";

                //如果在文件名称中没有找到对应的文件扩展名称,就依照文件类型添加扩展名
                if (TempExcelFileName.ToUpper().IndexOf(".CSV") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLS") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLSX") == -1)
                    {
                    switch (FileType)
                        {
                        case ExcelFileType.csv:
                            TempExcelFileName = TempExcelFileName + ".csv";
                            break;

                        case ExcelFileType.xls:
                            TempExcelFileName = TempExcelFileName + ".xls";
                            break;

                        case ExcelFileType.xlsx:
                            TempExcelFileName = TempExcelFileName + ".xlsx";
                            break;
                        }
                    }

                ////找出文件的扩展名
                //string TempStr = FileType.ToString().ToUpper();// Strings.Mid(FileType.ToString().ToUpper(), FileType.ToString().IndexOf('.'));
                //int TempPos = ExcelFileName.ToUpper().IndexOf(TempStr);

                ////如果在文件名称中没有找到对应的文件扩展名称,就查找其它两种是否对应，如果全部没有对应上就新建文件
                //if (TempPos == -1)
                //    {
                //    TempExcelFileName = ExcelFileName + "." + TempStr;
                //    }

                if (PC.FileSystem.FileExists(TempExcelFileName) == false)
                    {

                    //不存在，新建一个
                    FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                    TempFileStream.Close();

                    ////用上面的指令新建文件后，会在相同目录下留下一个同名的无扩展名的空文件，故需要删除；【是上面创建文件时用的ExcelFileName导致出错】
                    //System.IO.File.Delete(Strings.Mid(ExcelFileName, 1, Strings.InStr(ExcelFileName, ".")));

                    //if (MessageBox.Show("The file '" + TempExcelFileName + "' is not exist, do you want to create it?" +
                    //    "文件 '" + TempExcelFileName + "' 不存在，你需要新建一个吗？", "提示",
                    //    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    //    {
                    //    return false;
                    //    }
                    //else 
                    //    {
                    //后续可以考虑弹出保存对话框
                    //    }
                    }
                else 
                    {
                    if (OverWrite == true)
                        {
                        //先删除
                        System.IO.File.Delete(TempExcelFileName);
                        //再新建
                        FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                        TempFileStream.Close();
                        }
                    }

                for (int a = 0; a < DataToBeSaved.Length; a++)
                    {
                    StrRecordsToBeSavedExcel = StrRecordsToBeSavedExcel + DataToBeSaved[a] + "\t";// +Strings.Chr(9);// Tab 键
                    }

                StrRecordsToBeSavedExcel = StrRecordsToBeSavedExcel + "\r\n"; //"\r\n" + 

                Microsoft.VisualBasic.FileSystem.FileOpen(1, TempExcelFileName, OpenMode.Append);
                TempJudgement = true;//用于判断是否发生异常，如果成功打开文件后操作发生错误，然后在异常处理里面关闭文件
                Microsoft.VisualBasic.FileSystem.Print(1, StrRecordsToBeSavedExcel);
                Microsoft.VisualBasic.FileSystem.FileClose(1);

                return true;

                }
            catch (Exception ex)
                {
                //如果成功打开文件后的操作发生错误，然后在异常处理里面关闭文件
                if (TempJudgement == true)
                    {
                    Microsoft.VisualBasic.FileSystem.FileClose(1);
                    }
                //MessageBox.Show(ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            }

        //利用在字符串中用 Tab 键作为分隔符，写入EXCEL后，
        //EXCEL自动以用 Tab 键作为分隔符进行跳列，以回车换行符进行换行
        //快速添加单行数据到Excel文件【写单行数据】
        /// <summary>
        /// 快速添加单行数据到Excel文件【写单行数据】
        /// </summary>
        /// <param name="ExcelFileName">保存的Excel文件名称</param>
        /// <param name="FileType">Excel文件保存类型</param>
        /// <param name="DataToBeSaved">需要保存的数据double数组</param>
        /// <param name="OverWrite">是否覆盖原有Excel文件</param>
        /// <returns></returns>
        public bool QuickAppendSingleRowDataToExcel(string ExcelFileName,
            ExcelFileType FileType, double[] DataToBeSaved, bool OverWrite = false)
            {
            bool TempJudgement = false;
            try
                {
                if (ExcelFileName == "")
                    {
                    //MessageBox.Show("The parameter 'ExcelFileName' can't be empty, please input the correct file name.");
                    ErrorMessage = "The parameter 'ExcelFileName' can't be empty, please input the correct file name.";
                    return false;
                    }

                string TempExcelFileName = ExcelFileName;
                string StrRecordsToBeSavedExcel = "";

                //如果在文件名称中没有找到对应的文件扩展名称,就依照文件类型添加扩展名
                if (TempExcelFileName.ToUpper().IndexOf(".CSV") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLS") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLSX") == -1)
                    {
                    switch (FileType)
                        {
                        case ExcelFileType.csv:
                            TempExcelFileName = TempExcelFileName + ".csv";
                            break;

                        case ExcelFileType.xls:
                            TempExcelFileName = TempExcelFileName + ".xls";
                            break;

                        case ExcelFileType.xlsx:
                            TempExcelFileName = TempExcelFileName + ".xlsx";
                            break;
                        }
                    }

                if (PC.FileSystem.FileExists(TempExcelFileName) == false)
                    {
                    //不存在，新建一个
                    FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                    TempFileStream.Close();
                    }
                else
                    {
                    if (OverWrite == true)
                        {
                        //先删除
                        System.IO.File.Delete(TempExcelFileName);
                        //再新建
                        FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                        TempFileStream.Close();
                        }
                    }

                for (int a = 0; a < DataToBeSaved.Length; a++)
                    {
                    StrRecordsToBeSavedExcel = StrRecordsToBeSavedExcel + DataToBeSaved[a] + Strings.Chr(9);// Tab 键
                    }

                StrRecordsToBeSavedExcel = StrRecordsToBeSavedExcel + "\r\n"; //"\r\n" + 

                Microsoft.VisualBasic.FileSystem.FileOpen(1, TempExcelFileName, OpenMode.Append);
                TempJudgement = true;//用于判断是否发生异常，如果成功打开文件后操作发生错误，然后在异常处理里面关闭文件
                Microsoft.VisualBasic.FileSystem.Print(1, StrRecordsToBeSavedExcel);
                Microsoft.VisualBasic.FileSystem.FileClose(1);

                return true;

                }
            catch (Exception ex)
                {
                //如果成功打开文件后的操作发生错误，然后在异常处理里面关闭文件
                if (TempJudgement == true)
                    {
                    Microsoft.VisualBasic.FileSystem.FileClose(1);
                    }
                //MessageBox.Show(ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //利用在字符串中用 Tab 键作为分隔符，写入EXCEL后，
        //EXCEL自动以用 Tab 键作为分隔符进行跳列，以回车换行符进行换行
        //快速添加单行数据到Excel文件【写单行数据】
        /// <summary>
        /// 快速添加单行数据到Excel文件【写单行数据】
        /// </summary>
        /// <param name="ExcelFileName">保存的Excel文件名称</param>
        /// <param name="FileType">Excel文件保存类型</param>
        /// <param name="DataToBeSaved">需要保存的数据int数组</param>
        /// <param name="OverWrite">是否覆盖原有Excel文件</param>
        /// <returns></returns>
        public bool QuickAppendSingleRowDataToExcel(string ExcelFileName,
            ExcelFileType FileType, int[] DataToBeSaved, bool OverWrite = false)
            {
            bool TempJudgement = false;
            try
                {
                if (ExcelFileName == "")
                    {
                    //MessageBox.Show("The parameter 'ExcelFileName' can't be empty, please input the correct file name.");
                    ErrorMessage = "The parameter 'ExcelFileName' can't be empty, please input the correct file name.";
                    return false;
                    }

                string TempExcelFileName = ExcelFileName;
                string StrRecordsToBeSavedExcel = "";

                //如果在文件名称中没有找到对应的文件扩展名称,就依照文件类型添加扩展名
                if (TempExcelFileName.ToUpper().IndexOf(".CSV") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLS") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLSX") == -1)
                    {
                    switch (FileType)
                        {
                        case ExcelFileType.csv:
                            TempExcelFileName = TempExcelFileName + ".csv";
                            break;

                        case ExcelFileType.xls:
                            TempExcelFileName = TempExcelFileName + ".xls";
                            break;

                        case ExcelFileType.xlsx:
                            TempExcelFileName = TempExcelFileName + ".xlsx";
                            break;
                        }
                    }

                if (PC.FileSystem.FileExists(TempExcelFileName) == false)
                    {
                    //不存在，新建一个
                    FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                    TempFileStream.Close();
                    }
                else
                    {
                    if (OverWrite == true)
                        {
                        //先删除
                        System.IO.File.Delete(TempExcelFileName);
                        //再新建
                        FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                        TempFileStream.Close();
                        }
                    }

                for (int a = 0; a < DataToBeSaved.Length; a++)
                    {
                    StrRecordsToBeSavedExcel = StrRecordsToBeSavedExcel + DataToBeSaved[a] + Strings.Chr(9);// Tab 键
                    }

                StrRecordsToBeSavedExcel = StrRecordsToBeSavedExcel + "\r\n"; //"\r\n" + 

                Microsoft.VisualBasic.FileSystem.FileOpen(1, TempExcelFileName, OpenMode.Append);
                TempJudgement = true;//用于判断是否发生异常，如果成功打开文件后操作发生错误，然后在异常处理里面关闭文件
                Microsoft.VisualBasic.FileSystem.Print(1, StrRecordsToBeSavedExcel);
                Microsoft.VisualBasic.FileSystem.FileClose(1);

                return true;

                }
            catch (Exception ex)
                {
                //如果成功打开文件后的操作发生错误，然后在异常处理里面关闭文件
                if (TempJudgement == true)
                    {
                    Microsoft.VisualBasic.FileSystem.FileClose(1);
                    }
                //MessageBox.Show(ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //************************

        //快速添加多行数据到Excel文件【写多行数据，每列数据之间用TAB符号隔开，以回车换行符结束】
        /// <summary>
        /// 快速添加多行数据到Excel文件【写多行数据，每列数据之间用TAB符号隔开，以回车换行符结束】
        /// </summary>
        /// <param name="ExcelFileName">保存的Excel文件名称</param>
        /// <param name="FileType">Excel文件保存类型</param>
        /// <param name="DataToBeSaved">需要保存的数据字符串数组，每个下标字符串由每列数据之间用TAB符号隔开，以回车换行符结束</param>
        /// <param name="OverWrite">是否覆盖原有Excel文件</param>
        /// <returns></returns>
        public bool QuickAppendMultiRowsDataToExcel(string ExcelFileName,
            ExcelFileType FileType, string[] DataToBeSaved, bool OverWrite = false)
            {

            bool TempJudgement = false;

            try
                {
                if (ExcelFileName == "")
                    {
                    //MessageBox.Show("The parameter 'ExcelFileName' can't be empty, please input the correct file name.");
                    ErrorMessage = "The parameter 'ExcelFileName' can't be empty, please input the correct file name.";
                    return false;
                    }

                string TempExcelFileName = ExcelFileName;

                //如果在文件名称中没有找到对应的文件扩展名称,就依照文件类型添加扩展名
                if (TempExcelFileName.ToUpper().IndexOf(".CSV") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLS") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLSX") == -1) 
                    {
                    switch (FileType)
                        {
                        case ExcelFileType.csv:
                            TempExcelFileName = TempExcelFileName + ".csv";
                            break;

                        case ExcelFileType.xls:
                            TempExcelFileName = TempExcelFileName + ".xls";
                            break;

                        case ExcelFileType.xlsx:
                            TempExcelFileName = TempExcelFileName + ".xlsx";
                            break;
                        }
                    }

                if (PC.FileSystem.FileExists(TempExcelFileName) == false)
                    {
                    //不存在，新建一个
                    FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                    TempFileStream.Close();
                    }
                else
                    {
                    if (OverWrite == true)
                        {
                        //先删除
                        System.IO.File.Delete(TempExcelFileName);
                        //再新建
                        FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                        TempFileStream.Close();
                        }
                    }

                Microsoft.VisualBasic.FileSystem.FileOpen(1, TempExcelFileName, OpenMode.Append);
                TempJudgement = true;//用于判断是否发生异常，如果成功打开文件后操作发生错误，然后在异常处理里面关闭文件

                for (int a = 0; a < DataToBeSaved.Length; a++)
                    {
                    //如果没有回车+换行符号结尾，就添加
                    if (DataToBeSaved[a].IndexOf("\r\n") == -1) 
                        {
                        DataToBeSaved[a] += "\r\n";
                        }

                    if (a == 0)
                        {
                        Microsoft.VisualBasic.FileSystem.Print(1, DataToBeSaved[a]); //"\r\n" + 
                        }
                    else
                        {
                        Microsoft.VisualBasic.FileSystem.Print(1, DataToBeSaved[a]);
                        }
                    }

                Microsoft.VisualBasic.FileSystem.FileClose(1);

                return true;

                }
            catch (Exception ex)
                {
                //如果成功打开文件后的操作发生错误，然后在异常处理里面关闭文件
                if (TempJudgement == true)
                    {
                    Microsoft.VisualBasic.FileSystem.FileClose(1);
                    }
                //MessageBox.Show(ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            }

        //快速添加多行数据到Excel文件【写多行数据，每列数据之间用TAB符号隔开，以回车换行符结束】
        /// <summary>
        /// 快速添加多行数据到Excel文件【写多行数据，自动将每列数据之间用TAB符号隔开，以回车换行符结束】
        /// </summary>
        /// <param name="ExcelFileName">保存的Excel文件名称</param>
        /// <param name="FileType">Excel文件保存类型</param>
        /// <param name="DataToBeSaved">需要保存的数据字符串数组，每个下标字符串由每列数据之间用TAB符号隔开，以回车换行符结束</param>
        /// <param name="OverWrite">是否覆盖原有Excel文件</param>
        /// <returns></returns>
        public bool QuickAppendMultiRowsDataToExcel(string ExcelFileName,
            ExcelFileType FileType, string[][] DataToBeSaved, bool OverWrite = false)
            {
            bool TempJudgement = false;
            try
                {
                if (ExcelFileName == "")
                    {
                    //MessageBox.Show("The parameter 'ExcelFileName' can't be empty, please input the correct file name.");
                    ErrorMessage = "The parameter 'ExcelFileName' can't be empty, please input the correct file name.";
                    return false;
                    }

                string TempExcelFileName = ExcelFileName;

                //如果在文件名称中没有找到对应的文件扩展名称,就依照文件类型添加扩展名
                if (TempExcelFileName.ToUpper().IndexOf(".CSV") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLS") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLSX") == -1)
                    {
                    switch (FileType)
                        {
                        case ExcelFileType.csv:
                            TempExcelFileName = TempExcelFileName + ".csv";
                            break;

                        case ExcelFileType.xls:
                            TempExcelFileName = TempExcelFileName + ".xls";
                            break;

                        case ExcelFileType.xlsx:
                            TempExcelFileName = TempExcelFileName + ".xlsx";
                            break;
                        }
                    }
                
                if (PC.FileSystem.FileExists(TempExcelFileName) == false)
                    {
                    //不存在，新建一个
                    FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                    TempFileStream.Close();
                    }
                else
                    {
                    if (OverWrite == true)
                        {
                        //先删除
                        System.IO.File.Delete(TempExcelFileName);
                        //再新建
                        FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                        TempFileStream.Close();
                        }
                    }

                Microsoft.VisualBasic.FileSystem.FileOpen(1, TempExcelFileName, OpenMode.Append);
                TempJudgement = true;//用于判断是否发生异常，如果成功打开文件后操作发生错误，然后在异常处理里面关闭文件

                string TempStr;
                for (int a = 0; a < DataToBeSaved.Length; a++)
                    {
                    TempStr = "";
                    if (DataToBeSaved[a] != null) 
                        {
                        //添加回车+换行符号结尾
                        for (int b = 0; b < DataToBeSaved[a].Length; b++)
                            {
                            TempStr += DataToBeSaved[a][b] + "\t";
                            }
                        }

                    TempStr += "\r\n";
                    Microsoft.VisualBasic.FileSystem.Print(1, TempStr);

                    }

                Microsoft.VisualBasic.FileSystem.FileClose(1);

                return true;

                }
            catch (Exception ex)
                {
                //如果成功打开文件后的操作发生错误，然后在异常处理里面关闭文件
                if (TempJudgement == true)
                    {
                    Microsoft.VisualBasic.FileSystem.FileClose(1);
                    }
                //MessageBox.Show(ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            }

        //快速添加多行数据到Excel文件【写多行数据，每列数据之间用TAB符号隔开，以回车换行符结束】
        /// <summary>
        /// 快速添加多行数据到Excel文件【写多行数据，自动将每列数据之间用TAB符号隔开，以回车换行符结束】
        /// </summary>
        /// <param name="ExcelFileName">保存的Excel文件名称</param>
        /// <param name="FileType">Excel文件保存类型</param>
        /// <param name="DataToBeSaved">需要保存的数据double数组，每个下标字符串由每列数据之间用TAB符号隔开，以回车换行符结束</param>
        /// <param name="OverWrite">是否覆盖原有Excel文件</param>
        /// <returns></returns>
        public bool QuickAppendMultiRowsDataToExcel(string ExcelFileName,
            ExcelFileType FileType, double[][] DataToBeSaved, bool OverWrite = false)
            {

            bool TempJudgement = false;

            try
                {
                if (ExcelFileName == "")
                    {
                    //MessageBox.Show("The parameter 'ExcelFileName' can't be empty, please input the correct file name.");
                    ErrorMessage = "The parameter 'ExcelFileName' can't be empty, please input the correct file name.";
                    return false;
                    }

                string TempExcelFileName = ExcelFileName;

                //如果在文件名称中没有找到对应的文件扩展名称,就依照文件类型添加扩展名
                if (TempExcelFileName.ToUpper().IndexOf(".CSV") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLS") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLSX") == -1)
                    {
                    switch (FileType)
                        {
                        case ExcelFileType.csv:
                            TempExcelFileName = TempExcelFileName + ".csv";
                            break;

                        case ExcelFileType.xls:
                            TempExcelFileName = TempExcelFileName + ".xls";
                            break;

                        case ExcelFileType.xlsx:
                            TempExcelFileName = TempExcelFileName + ".xlsx";
                            break;
                        }
                    }

                if (PC.FileSystem.FileExists(TempExcelFileName) == false)
                    {
                    //不存在，新建一个
                    FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                    TempFileStream.Close();
                    }
                else
                    {
                    if (OverWrite == true)
                        {
                        //先删除
                        System.IO.File.Delete(TempExcelFileName);
                        //再新建
                        FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                        TempFileStream.Close();
                        }
                    }

                Microsoft.VisualBasic.FileSystem.FileOpen(1, TempExcelFileName, OpenMode.Append);
                TempJudgement = true;//用于判断是否发生异常，如果成功打开文件后操作发生错误，然后在异常处理里面关闭文件

                string TempStr;
                for (int a = 0; a < DataToBeSaved.Length; a++)
                    {
                    TempStr = "";
                    if (DataToBeSaved[a] != null)
                        {
                        //添加回车+换行符号结尾
                        for (int b = 0; b < DataToBeSaved[a].Length; b++)
                            {
                            TempStr += DataToBeSaved[a][b] + "\t";
                            }
                        }

                    TempStr += "\r\n";
                    Microsoft.VisualBasic.FileSystem.Print(1, TempStr);

                    }

                Microsoft.VisualBasic.FileSystem.FileClose(1);

                return true;

                }
            catch (Exception ex)
                {
                //如果成功打开文件后的操作发生错误，然后在异常处理里面关闭文件
                if (TempJudgement == true)
                    {
                    Microsoft.VisualBasic.FileSystem.FileClose(1);
                    }
                //MessageBox.Show(ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            }

        //快速添加多行数据到Excel文件【写多行数据，每列数据之间用TAB符号隔开，以回车换行符结束】
        /// <summary>
        /// 快速添加多行数据到Excel文件【写多行数据，自动将每列数据之间用TAB符号隔开，以回车换行符结束】
        /// </summary>
        /// <param name="ExcelFileName">保存的Excel文件名称</param>
        /// <param name="FileType">Excel文件保存类型</param>
        /// <param name="DataToBeSaved">需要保存的数据int数组，每个下标字符串由每列数据之间用TAB符号隔开，以回车换行符结束</param>
        /// <param name="OverWrite">是否覆盖原有Excel文件</param>
        /// <returns></returns>
        public bool QuickAppendMultiRowsDataToExcel(string ExcelFileName,
            ExcelFileType FileType, int[][] DataToBeSaved, bool OverWrite = false)
            {
            bool TempJudgement = false;
            try
                {
                if (ExcelFileName == "")
                    {
                    //MessageBox.Show("The parameter 'ExcelFileName' can't be empty, please input the correct file name.");
                    ErrorMessage = "The parameter 'ExcelFileName' can't be empty, please input the correct file name.";
                    return false;
                    }

                string TempExcelFileName = ExcelFileName;

                //如果在文件名称中没有找到对应的文件扩展名称,就依照文件类型添加扩展名
                if (TempExcelFileName.ToUpper().IndexOf(".CSV") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLS") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLSX") == -1)
                    {
                    switch (FileType)
                        {
                        case ExcelFileType.csv:
                            TempExcelFileName = TempExcelFileName + ".csv";
                            break;

                        case ExcelFileType.xls:
                            TempExcelFileName = TempExcelFileName + ".xls";
                            break;

                        case ExcelFileType.xlsx:
                            TempExcelFileName = TempExcelFileName + ".xlsx";
                            break;
                        }
                    }

                if (PC.FileSystem.FileExists(TempExcelFileName) == false)
                    {
                    //不存在，新建一个
                    FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                    TempFileStream.Close();
                    }
                else
                    {
                    if (OverWrite == true)
                        {
                        //先删除
                        System.IO.File.Delete(TempExcelFileName);
                        //再新建
                        FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                        TempFileStream.Close();
                        }
                    }

                Microsoft.VisualBasic.FileSystem.FileOpen(1, TempExcelFileName, OpenMode.Append);
                TempJudgement = true;//用于判断是否发生异常，如果成功打开文件后操作发生错误，然后在异常处理里面关闭文件

                string TempStr;
                for (int a = 0; a < DataToBeSaved.Length; a++)
                    {
                    TempStr = "";
                    if (DataToBeSaved[a] != null)
                        {
                        //添加回车+换行符号结尾
                        for (int b = 0; b < DataToBeSaved[a].Length; b++)
                            {
                            TempStr += DataToBeSaved[a][b] + "\t";
                            }
                        }

                    TempStr += "\r\n";
                    Microsoft.VisualBasic.FileSystem.Print(1, TempStr);

                    }

                Microsoft.VisualBasic.FileSystem.FileClose(1);

                return true;

                }
            catch (Exception ex)
                {
                //如果成功打开文件后的操作发生错误，然后在异常处理里面关闭文件
                if (TempJudgement == true)
                    {
                    Microsoft.VisualBasic.FileSystem.FileClose(1);
                    }
                //MessageBox.Show(ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            }

        //************************

        //快速添加多行数据到Excel文件【写多行数据，每列数据之间用TAB符号隔开，以回车换行符结束】
        /// <summary>
        /// 快速添加多行数据到Excel文件【写多行数据，每列数据之间用TAB符号隔开，以回车换行符结束】
        /// </summary>
        /// <param name="ExcelFileName">保存的Excel文件名称</param>
        /// <param name="DataToBeSaved">需要保存的数据字符串数组，每个下标字符串由每列数据之间用TAB符号隔开，以回车换行符结束</param>
        /// <param name="OverWrite">是否覆盖原有Excel文件</param>
        /// <returns></returns>
        private bool QuickAppendMultiRowsDataToExcel(string ExcelFileName,
            string[] DataToBeSaved, bool OverWrite = false)
            {
            bool TempJudgement = false;
            try
                {                                
                if (ExcelFileName == "")
                    {
                    //MessageBox.Show("The parameter 'ExcelFileName' can't be empty, please input the correct file name.");
                    ErrorMessage = "The parameter 'ExcelFileName' can't be empty, please input the correct file name.";
                    return false;
                    }

                string TempExcelFileName = ExcelFileName;

                if (TempExcelFileName.ToUpper().IndexOf(".CSV") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLS") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLSX") == -1)
                    {
                    TempExcelFileName += ".xls";
                    //MessageBox.Show("The file " + TempExcelFileName + " is not excel file.");
                    //return false;
                    }

                if (PC.FileSystem.FileExists(TempExcelFileName) == false)
                    {
                    //不存在，新建一个
                    FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                    TempFileStream.Close();
                    }
                else
                    {
                    if (OverWrite == true)
                        {
                        //先删除
                        System.IO.File.Delete(TempExcelFileName);
                        //再新建
                        FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                        TempFileStream.Close();
                        }
                    }

                Microsoft.VisualBasic.FileSystem.FileOpen(1, TempExcelFileName, OpenMode.Append);
                TempJudgement = true;//用于判断是否发生异常，如果成功打开文件后操作发生错误，然后在异常处理里面关闭文件

                for (int a = 0; a < DataToBeSaved.Length; a++)
                    {
                    Microsoft.VisualBasic.FileSystem.Print(1, DataToBeSaved[a]);
                    }

                Microsoft.VisualBasic.FileSystem.FileClose(1);

                return true;

                }
            catch (Exception ex)
                {
                //如果成功打开文件后的操作发生错误，然后在异常处理里面关闭文件
                if (TempJudgement == true)
                    {
                    Microsoft.VisualBasic.FileSystem.FileClose(1);
                    }
                //MessageBox.Show(ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            }

        //快速添加多行数据到Excel文件【写多行数据，每列数据之间用TAB符号隔开，以回车换行符结束】
        /// <summary>
        /// 快速添加多行数据到Excel文件【写多行数据，自动将每列数据之间用TAB符号隔开，以回车换行符结束】
        /// </summary>
        /// <param name="ExcelFileName">保存的Excel文件名称</param>
        /// <param name="DataToBeSaved">需要保存的数据字符串数组，每个下标字符串由每列数据之间用TAB符号隔开，以回车换行符结束</param>
        /// <param name="OverWrite">是否覆盖原有Excel文件</param>
        /// <returns></returns>
        public bool QuickAppendMultiRowsDataToExcel(string ExcelFileName, 
            string[][] DataToBeSaved, bool OverWrite = false)
            {

            bool TempJudgement = false;

            try
                {
                if (ExcelFileName == "")
                    {
                    //MessageBox.Show("The parameter 'ExcelFileName' can't be empty, please input the correct file name.");
                    ErrorMessage = "The parameter 'ExcelFileName' can't be empty, please input the correct file name.";
                    return false;
                    }

                string TempExcelFileName = ExcelFileName;

                //如果在文件名称中没有找到对应的文件扩展名称,就依照文件类型添加扩展名
                if (TempExcelFileName.ToUpper().IndexOf(".CSV") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLS") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLSX") == -1)
                    {
                    TempExcelFileName = TempExcelFileName + ".xls";
                    }

                if (PC.FileSystem.FileExists(TempExcelFileName) == false)
                    {
                    //不存在，新建一个
                    FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                    TempFileStream.Close();
                    }
                else
                    {
                    if (OverWrite == true)
                        {
                        //先删除
                        System.IO.File.Delete(TempExcelFileName);
                        //再新建
                        FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                        TempFileStream.Close();
                        }
                    }

                Microsoft.VisualBasic.FileSystem.FileOpen(1, TempExcelFileName, OpenMode.Append);
                TempJudgement = true;//用于判断是否发生异常，如果成功打开文件后操作发生错误，然后在异常处理里面关闭文件

                string TempStr;
                for (int a = 0; a < DataToBeSaved.Length; a++)
                    {
                    TempStr = "";
                    if (DataToBeSaved[a] != null)
                        {
                        //添加回车+换行符号结尾
                        for (int b = 0; b < DataToBeSaved[a].Length; b++)
                            {
                            TempStr += DataToBeSaved[a][b] + "\t";
                            }
                        }

                    TempStr += "\r\n";
                    Microsoft.VisualBasic.FileSystem.Print(1, TempStr);

                    }

                Microsoft.VisualBasic.FileSystem.FileClose(1);

                return true;

                }
            catch (Exception ex)
                {
                //如果成功打开文件后的操作发生错误，然后在异常处理里面关闭文件
                if (TempJudgement == true)
                    {
                    Microsoft.VisualBasic.FileSystem.FileClose(1);
                    }
                //MessageBox.Show(ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            }

        //快速添加多行数据到Excel文件【写多行数据，每列数据之间用TAB符号隔开，以回车换行符结束】
        /// <summary>
        /// 快速添加多行数据到Excel文件【写多行数据，自动将每列数据之间用TAB符号隔开，以回车换行符结束】
        /// </summary>
        /// <param name="ExcelFileName">保存的Excel文件名称</param>
        /// <param name="DataToBeSaved">需要保存的数据double数组，每个下标字符串由每列数据之间用TAB符号隔开，以回车换行符结束</param>
        /// <param name="OverWrite">是否覆盖原有Excel文件</param>
        /// <returns></returns>
        public bool QuickAppendMultiRowsDataToExcel(string ExcelFileName,
            double[][] DataToBeSaved, bool OverWrite = false)
            {
            bool TempJudgement = false;
            try
                {
                if (ExcelFileName == "")
                    {
                    //MessageBox.Show("The parameter 'ExcelFileName' can't be empty, please input the correct file name.");
                    ErrorMessage = "The parameter 'ExcelFileName' can't be empty, please input the correct file name.";
                    return false;
                    }

                string TempExcelFileName = ExcelFileName;

                //如果在文件名称中没有找到对应的文件扩展名称,就依照文件类型添加扩展名
                if (TempExcelFileName.ToUpper().IndexOf(".CSV") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLS") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLSX") == -1)
                    {
                    TempExcelFileName = TempExcelFileName + ".xls";
                    }

                if (PC.FileSystem.FileExists(TempExcelFileName) == false)
                    {
                    //不存在，新建一个
                    FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                    TempFileStream.Close();
                    }
                else
                    {
                    if (OverWrite == true)
                        {
                        //先删除
                        System.IO.File.Delete(TempExcelFileName);
                        //再新建
                        FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                        TempFileStream.Close();
                        }
                    }

                Microsoft.VisualBasic.FileSystem.FileOpen(1, TempExcelFileName, OpenMode.Append);
                TempJudgement = true;//用于判断是否发生异常，如果成功打开文件后操作发生错误，然后在异常处理里面关闭文件

                string TempStr;
                for (int a = 0; a < DataToBeSaved.Length; a++)
                    {
                    TempStr = "";
                    if (DataToBeSaved[a] != null)
                        {
                        //添加回车+换行符号结尾
                        for (int b = 0; b < DataToBeSaved[a].Length; b++)
                            {
                            TempStr += DataToBeSaved[a][b] + "\t";
                            }
                        }

                    TempStr += "\r\n";
                    Microsoft.VisualBasic.FileSystem.Print(1, TempStr);

                    }

                Microsoft.VisualBasic.FileSystem.FileClose(1);

                return true;

                }
            catch (Exception ex)
                {
                //如果成功打开文件后的操作发生错误，然后在异常处理里面关闭文件
                if (TempJudgement == true)
                    {
                    Microsoft.VisualBasic.FileSystem.FileClose(1);
                    }
                //MessageBox.Show(ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            }

        //快速添加多行数据到Excel文件【写多行数据，每列数据之间用TAB符号隔开，以回车换行符结束】
        /// <summary>
        /// 快速添加多行数据到Excel文件【写多行数据，自动将每列数据之间用TAB符号隔开，以回车换行符结束】
        /// </summary>
        /// <param name="ExcelFileName">保存的Excel文件名称</param>
        /// <param name="DataToBeSaved">需要保存的数据int数组，每个下标字符串由每列数据之间用TAB符号隔开，以回车换行符结束</param>
        /// <param name="OverWrite">是否覆盖原有Excel文件</param>
        /// <returns></returns>
        public bool QuickAppendMultiRowsDataToExcel(string ExcelFileName, 
            int[][] DataToBeSaved, bool OverWrite = false)
            {

            bool TempJudgement = false;

            try
                {
                if (ExcelFileName == "")
                    {
                    //MessageBox.Show("The parameter 'ExcelFileName' can't be empty, please input the correct file name.");
                    ErrorMessage = "The parameter 'ExcelFileName' can't be empty, please input the correct file name.";
                    return false;
                    }

                string TempExcelFileName = ExcelFileName;

                //如果在文件名称中没有找到对应的文件扩展名称,就依照文件类型添加扩展名
                if (TempExcelFileName.ToUpper().IndexOf(".CSV") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLS") == -1
                    && TempExcelFileName.ToUpper().IndexOf(".XLSX") == -1)
                    {
                    TempExcelFileName = TempExcelFileName + ".xls";
                    }

                if (PC.FileSystem.FileExists(TempExcelFileName) == false)
                    {
                    //不存在，新建一个
                    FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                    TempFileStream.Close();
                    }
                else
                    {
                    if (OverWrite == true)
                        {
                        //先删除
                        System.IO.File.Delete(TempExcelFileName);
                        //再新建
                        FileStream TempFileStream = System.IO.File.Create(TempExcelFileName);
                        TempFileStream.Close();
                        }
                    }

                Microsoft.VisualBasic.FileSystem.FileOpen(1, TempExcelFileName, OpenMode.Append);
                TempJudgement = true;//用于判断是否发生异常，如果成功打开文件后操作发生错误，然后在异常处理里面关闭文件

                string TempStr;
                for (int a = 0; a < DataToBeSaved.Length; a++)
                    {
                    TempStr = "";
                    if (DataToBeSaved[a] != null)
                        {
                        //添加回车+换行符号结尾
                        for (int b = 0; b < DataToBeSaved[a].Length; b++)
                            {
                            TempStr += DataToBeSaved[a][b] + "\t";
                            }
                        }

                    TempStr += "\r\n";
                    Microsoft.VisualBasic.FileSystem.Print(1, TempStr);

                    }

                Microsoft.VisualBasic.FileSystem.FileClose(1);

                return true;

                }
            catch (Exception ex)
                {
                //如果成功打开文件后的操作发生错误，然后在异常处理里面关闭文件
                if (TempJudgement == true)
                    {
                    Microsoft.VisualBasic.FileSystem.FileClose(1);
                    }
                //MessageBox.Show(ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            }

        //************************

        #endregion

        //释放类的相关资源
        /// <summary>
        /// 释放类的相关资源
        /// </summary>
        public void Dispose()
            {
            try
                {
                DelegateTextbox.Dispose();
                DelegateButton.Dispose();
                PC = null;
                //ExcelWorkSheet = null;
                //ExcelWorkBook = null;//.Close();
                //ExcelApp.Quit();
                return;
                }
            catch (Exception)// ex)
                {
                //ErrorMessage = ex.Message;
                return;
                }
            }

        //9、索引器
        //  索引器 (indexer) 是这样一个成员：它支持按照索引数组的方法来索引对象。
        //  索引器的声明与属性类似，不同的是该成员的名称是 this，后跟一个位于定界符 [ 和 ] 之间的参数列表。
        //  在索引器的访问器中可以使用这些参数。与属性类似，索引器可以是读写、只读和只写的，
        //  并且索引器的访问器可以是虚的。
        //  该 List 类声明了单个读写索引器，该索引器接受一个 int 参数。
        //  该索引器使得通过 int 值对 List 实例进行索引成为可能。例如
        //  List<string> names = new List<string>();
        //  names.Add("Liz");
        //  names.Add("Martha");
        //  names.Add("Beth");
        //  for (int i = 0; i < names.Count; i++) {
        //    string s = names[i];
        //    names[i] = s.ToUpper();
        //  }
        //  索引器可以被重载，这意味着一个类可以声明多个索引器，只要其参数的数量和类型不同即可。

        /// <summary>
        /// 验证XML文件名称的有效性，不存在就新建文件
        /// </summary>
        /// <param name="TargetXMLFileName">XML文件名称含路径</param>
        /// <param name="CreateFileIfNotExist">如果文件不存在，是否创建文件(默认创建)</param>
        /// <returns>文件是否存在</returns>
        public static bool VerifyXMLFileExist(ref string TargetXMLFileName, bool CreateFileIfNotExist = true)
        {
            try
            {
                if (null == TargetXMLFileName || "" == TargetXMLFileName)
                {
                    return false;
                }
                else
                {
                    try
                    {
                        string TempPath = Path.GetDirectoryName(TargetXMLFileName);
                        if (Directory.Exists(TempPath) == false)
                        {
                            Directory.CreateDirectory(TempPath);
                        }
                    }
                    catch (Exception)
                    {
                    }

                    FileInfo TempFileInfo = new FileInfo(TargetXMLFileName);
                    if (TempFileInfo.Extension.ToString().ToUpper() != ".XML")
                    {
                        string str = Strings.Right(TargetXMLFileName, 4);
                        if (str.IndexOf('.') == -1)
                        {
                            TargetXMLFileName += ".XML";
                        }
                        else
                        {
                            str = Strings.Left(TargetXMLFileName, TargetXMLFileName.Length - 4);
                            str += ".XML";
                            TargetXMLFileName = str;
                        }
                    }
                }

                if (File.Exists(TargetXMLFileName) == false)
                {
                    if (CreateFileIfNotExist == true)
                    {
                        FileStream TempFileStream = File.Create(TargetXMLFileName);
                        TempFileStream.Close();
                        TempFileStream.Dispose();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证INI文件名称的有效性，不存在就新建文件
        /// </summary>
        /// <param name="TargetIniFileName">Ini文件名称含路径</param>
        /// <param name="CreateFileIfNotExist">如果文件不存在，是否创建文件(默认创建)</param>
        /// <returns>文件是否存在</returns>
        public static bool VerifyIniFileExist(ref string TargetIniFileName, bool CreateFileIfNotExist = true)
        {
            try
            {
                if (null == TargetIniFileName || "" == TargetIniFileName)
                {
                    return false;
                }
                else
                {
                    try
                    {
                        string TempPath = Path.GetDirectoryName(TargetIniFileName);
                        if (Directory.Exists(TempPath) == false)
                        {
                            Directory.CreateDirectory(TempPath);
                        }
                    }
                    catch (Exception)
                    {
                    }

                    FileInfo TempFileInfo = new FileInfo(TargetIniFileName);
                    if (TempFileInfo.Extension.ToString().ToUpper() != ".INI")
                    {

                        string str = Strings.Right(TargetIniFileName, 4);
                        if (str.IndexOf('.') == -1)
                        {
                            TargetIniFileName += ".INI";
                        }
                        else
                        {
                            str = Strings.Left(TargetIniFileName, TargetIniFileName.Length - 4);
                            str += ".INI";
                            TargetIniFileName = str;
                        }
                    }
                }

                if (File.Exists(TargetIniFileName) == false)
                {
                    if (CreateFileIfNotExist == true)
                    {
                        FileStream TempFileStream = File.Create(TargetIniFileName);
                        TempFileStream.Close();
                        TempFileStream.Dispose();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证IP地址的合法性
        /// </summary>
        /// <param name="IPAddressToBeVerified">待验证的IP地址</param>
        /// <returns>IP地址是否正确</returns>
        public static bool VerifyIPAddress(string IPAddressToBeVerified)
        {
            try
            {
                if (null == IPAddressToBeVerified || "" == IPAddressToBeVerified)
                {
                    return false;
                }

                string[] sIP = IPAddressToBeVerified.Split('.');
                if (null == sIP)
                {
                    return false;
                }
                else
                {
                    if (sIP.Length != 4)
                    {
                        return false;
                    }
                }

                IPAddress TempIPAddress = null;

                if (IPAddress.TryParse(IPAddressToBeVerified, out TempIPAddress) == true)
                {
                    if (null != TempIPAddress)
                    {
                        TempIPAddress = null;
                    }
                    return true;
                }
                else
                {
                    if (null != TempIPAddress)
                    {
                        TempIPAddress = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        /// <summary>
        /// 检查目录是否存在，如果目录不存在，是否创建目录
        /// </summary>
        /// <param name="TargetPath">目录路径字符串，如果不指定带盘符的绝对路径，就会在当前程序的目录下创建目录</param>
        /// <param name="CreateDirectoryIfNotExist">如果目录不存在，是否创建目录(默认创建)</param>
        /// <returns>目录是否存在</returns>
        public static bool VerifyPathExist(string TargetPath, bool CreateDirectoryIfNotExist = true)
        {
            try
            {
                if (null == TargetPath || "" == TargetPath)
                {
                    return false;
                }

                string sPath = Path.GetDirectoryName(TargetPath);
                if (null == sPath || "" == sPath)
                {
                    return false;
                }

                if (Directory.Exists(sPath) == false)
                {
                    if (CreateDirectoryIfNotExist == false)
                    {
                        return false;
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(TargetPath);
                    }
                }

                //下面的方法有缺陷
                //System.IO.DirectoryInfo PathInfo = new DirectoryInfo(TargetPath);
                //if (PathInfo.Exists == false)
                //{
                //if (CreateDirectoryIfNotExist == false)
                //{
                //    return false;
                //}
                //else
                //{
                //    System.IO.Directory.CreateDirectory(TargetPath);
                //}
                //}

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 检查目录是否存在，如果目录不存在，是否创建目录
        /// </summary>
        /// <param name="TargetPath">目录路径字符串，如果不指定带盘符的绝对路径，就会在当前程序的目录下创建目录</param>
        /// <param name="VerifiedPath">验证后的目录路径</param>
        /// <param name="CreateDirectoryIfNotExist">如果目录不存在，是否创建目录(默认创建)</param>
        /// <returns></returns>
        public static bool VerifyPathExist(string TargetPath, out string VerifiedPath, bool CreateDirectoryIfNotExist = true)
        {
            try
            {
                if (null == TargetPath || "" == TargetPath)
                {
                    VerifiedPath = "";
                    return false;
                }

                string sPath = Path.GetDirectoryName(TargetPath);

                if (null == sPath || "" == sPath)
                {
                    VerifiedPath = "";
                    return false;
                }

                if (Directory.Exists(sPath) == false)
                {
                    if (CreateDirectoryIfNotExist == false)
                    {
                        VerifiedPath = "";
                        return false;
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(TargetPath);
                    }
                }

                //下面的方法有缺陷
                //System.IO.DirectoryInfo PathInfo = new DirectoryInfo(Path);
                //if (PathInfo.Exists == false)
                //{
                //    if (CreateDirectoryIfNotExist == false)
                //    {
                //        VerifiedPath = "";
                //        return false;
                //    }
                //    else
                //    {
                //        System.IO.Directory.CreateDirectory(Path);
                //    }
                //}

                VerifiedPath = sPath;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                VerifiedPath = "";
                return false;
            }
        }

        /// <summary>
        /// 将源文件夹下的文件含子文件夹复制到目标文件夹中
        /// </summary>
        /// <param name="SourcePath"></param>
        /// <param name="TargetPath"></param>
        /// <returns></returns>
        public static bool CopyDirectory(string SourcePath, string TargetPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加
                if (TargetPath[TargetPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                {
                    TargetPath += System.IO.Path.DirectorySeparatorChar;
                }

                // 判断目标目录是否存在如果不存在则新建
                if (System.IO.Directory.Exists(TargetPath) == false)
                {
                    System.IO.Directory.CreateDirectory(TargetPath);
                }

                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                // string[] fileList = Directory.GetFiles（SourcePath）；
                string[] fileList = System.IO.Directory.GetFileSystemEntries(SourcePath);

                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (System.IO.Directory.Exists(file) == true)
                    {
                        CopyDirectory(file, TargetPath + System.IO.Path.GetFileName(file));
                    }
                    // 否则直接Copy文件
                    else
                    {
                        System.IO.File.Copy(file, TargetPath + System.IO.Path.GetFileName(file), true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("备份参数文件发生错误：" + ex.Message + " ;" + ex.StackTrace, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取系统可用内存大小(单位：G)
        /// </summary>
        /// <returns></returns>
        public static double AvailableMemoryInGB()
        {
            double AvailableMemory = 0;

            try
            {
                ManagementClass managementClassOs = new ManagementClass("Win32_OperatingSystem");
                foreach (var managementBaseObject in managementClassOs.GetInstances())
                {
                    if (managementBaseObject["FreePhysicalMemory"] != null)
                    {
                        //这里要除以 2 个 1024 才能得到 G
                        AvailableMemory = Convert.ToDouble(long.Parse(managementBaseObject["FreePhysicalMemory"].ToString())) / 1024 / 1024; //g
                    }
                }
            }
            catch (Exception)
            {
            }

            return AvailableMemory;
        }

        /// <summary>
        /// 获取系统总内存大小(单位：G)
        /// </summary>
        /// <returns></returns>
        public static double TotalPhysicalMemoryInGB()
        {
            double TotalPhysical = 0;

            try
            {
                ManagementClass managementClassOs = new ManagementClass("Win32_PhysicalMemory");
                foreach (var managementBaseObject in managementClassOs.GetInstances())
                {
                    if (managementBaseObject["Capacity"] != null)
                    {
                        //这里要除以 3 个 1024 才能得到 G
                        TotalPhysical += Convert.ToDouble(long.Parse(managementBaseObject["Capacity"].ToString())) / 1024 / 1024 / 1024; //g
                    }
                }

                ////方法二
                //string st = "";
                //ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                //ManagementObjectCollection moc = mc.GetInstances();
                //foreach (ManagementObject mo in moc)
                //{
                //    st = mo["TotalPhysicalMemory"].ToString();
                //}
                //double memory = Math.Round(Convert.ToDouble(st) / 1024 / 1024 / 1024);
            }
            catch (Exception)
            {
            }

            return TotalPhysical;
        }

        /// <summary>
        /// 已使用内存的百分比(%)
        /// </summary>
        /// <returns></returns>
        public static double UsedMemoryPercent()
        {
            double dUsedMemoryPercent = 0.0;

            try
            {
                double dAvailableFreeMemoryInGB = AvailableMemoryInGB();
                double dTotalPhysicalMemoryInGB = TotalPhysicalMemoryInGB();

                if (dAvailableFreeMemoryInGB <= 0.0)
                {
                    dUsedMemoryPercent = 100;
                }
                else
                {
                    dUsedMemoryPercent = dAvailableFreeMemoryInGB / dTotalPhysicalMemoryInGB;
                }
            }
            catch (Exception)
            {

            }
            return Math.Round(dUsedMemoryPercent, 2);
        }

        /// <summary>
        /// 获取当前系统类型
        /// </summary>
        /// <returns></returns>
        public static string GetSystemType()
        {
            string st = "";
            try
            {
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["SystemType"].ToString();
                }
                moc = null;
                mc = null;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
            return st;
        }

        /// <summary>
        /// 获取当前系统CPU序列号
        /// </summary>
        /// <returns></returns>
        public static string GetCPUID()
        {
            string cpuInfo = "";
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
            return cpuInfo;
        }

        /// <summary>
        /// 保存记录到 txt 文件
        /// </summary>
        /// <param name="TargetTextFileName">目标 txt 文件名称和路径(必须带.txt扩展名)</param>
        /// <param name="ContentsToBeWrote">需要保存到文件的内容</param>
        /// <param name="AddDateAsPrefix">是否添加日期前缀到记录</param>
        /// <returns></returns>
        public static bool SaveTxtFile(string TargetTextFileName, string ContentsToBeWrote, bool AddDateAsPrefix = false)
        {
            try
            {
                if (null == TargetTextFileName || null == ContentsToBeWrote
                    || "" == TargetTextFileName || "" == ContentsToBeWrote)
                {
                    return false;
                }

                if (VerifyPathExist(TargetTextFileName) == false)
                {
                    return false;
                }

                FileInfo tempInfo = new FileInfo(TargetTextFileName);
                if (tempInfo.Extension.ToUpper() != ".TXT")
                {
                    return false;
                }
                if (tempInfo.Exists == false)
                {
                    FileStream tempStream = tempInfo.Create();
                    tempStream.Close();
                    tempStream.Dispose();
                }

                tempInfo = null;

                string[] sTempArray = new string[1];
                if (AddDateAsPrefix == true)
                {
                    sTempArray[0] = DateAndTime.Now.ToString() + "\t" + ContentsToBeWrote;
                }
                else
                {
                    sTempArray[0] = ContentsToBeWrote;
                }

                File.AppendAllLines(TargetTextFileName, sTempArray, Encoding.Unicode);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保存控件名字到 txt 文件
        /// </summary>
        /// <param name="TargetTextFileName">目标 txt 文件名称和路径(必须带.txt扩展名)</param>
        /// <param name="TargetControl">目标控件</param>
        public static void SaveAllControlNamesAndItsText(string TargetTextFileName, Control TargetControl)
        {
            try
            {
                //SaveTxtFile(TargetTextFileName, TargetControl.Name + "\t" + TargetControl.Text);

                if (TargetControl is MenuStrip)
                {
                    //菜单
                    MenuStrip tempMenu = TargetControl as MenuStrip;
                    SaveTxtFile(TargetTextFileName, tempMenu.Name + "\t" + tempMenu.Text);
                    foreach (ToolStripMenuItem item in tempMenu.Items)
                    {
                        if (null != item)
                        {
                            SaveTxtFile(TargetTextFileName, item.Name + "\t" + item.Text);
                            if (null != item.DropDownItems && item.DropDownItems.Count > 0)
                            {
                                foreach (ToolStripMenuItem subitem in item.DropDownItems)
                                {
                                    if (null != subitem)
                                    {
                                        SaveTxtFile(TargetTextFileName, subitem.Name + "\t" + subitem.Text);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (TargetControl is StatusStrip)
                {
                    //状态栏
                    StatusStrip tempStatusStrip = TargetControl as StatusStrip;
                    SaveTxtFile(TargetTextFileName, tempStatusStrip.Name + "\t" + tempStatusStrip.Text);
                    foreach (ToolStripStatusLabel item in tempStatusStrip.Items)
                    {
                        if (null != item)
                        {
                            SaveTxtFile(TargetTextFileName, item.Name + "\t" + item.Text);
                        }
                    }
                }
                else if (TargetControl is ToolStrip)//如果传入的控件是StatusStrip，这里的判断条件也是 true，这可能是因为继承的关系
                {
                    //工具栏按钮
                    ToolStrip tempToolStrip = TargetControl as ToolStrip;
                    SaveTxtFile(TargetTextFileName, tempToolStrip.Name + "\t" + tempToolStrip.Text);
                    foreach (ToolStripButton item in tempToolStrip.Items)
                    {
                        if (null != item)
                        {
                            SaveTxtFile(TargetTextFileName, item.Name + "\t" + item.Text);
                        }
                    }
                }
                else if (TargetControl is Form)
                {
                    //窗体
                    SaveTxtFile(TargetTextFileName, TargetControl.Name + "\t" + TargetControl.Text);
                    foreach (Control item in TargetControl.Controls)
                    {
                        if (null != item)
                        {
                            if (item.HasChildren == true)
                            {
                                SaveTxtFile(TargetTextFileName, item.Name + "\t" + item.Text);
                                SaveAllControlNamesAndItsText(TargetTextFileName, item);
                            }
                            else
                            {
                                SaveTxtFile(TargetTextFileName, item.Name + "\t" + item.Text);
                            }
                        }
                    }
                }
                else if (TargetControl is DataGridView)
                {
                    //DataGridView
                    DataGridView tempDataGridView = TargetControl as DataGridView;
                    SaveTxtFile(TargetTextFileName, tempDataGridView.Name + "\t" + tempDataGridView.Text);
                    foreach (DataGridViewColumn item in tempDataGridView.Columns)
                    {
                        if (null != item)
                        {
                            SaveTxtFile(TargetTextFileName, item.Name + "\t" + item.HeaderText);
                        }
                    }
                }
                else if (TargetControl is TreeView)
                {
                    //TreeView
                    TreeView tempTreeView = TargetControl as TreeView;
                    SaveTxtFile(TargetTextFileName, tempTreeView.Name + "\t" + tempTreeView.Text);
                    if (tempTreeView.Nodes.Count > 0)
                    {
                        tempTreeView.SuspendLayout();
                        TreeNode[] tempTreeNodes = new TreeNode[tempTreeView.Nodes.Count];
                        for (int i = 0; i < tempTreeView.Nodes.Count; i++)
                        {
                            SaveTxtFile(TargetTextFileName, tempTreeView.Nodes[i].Name + "\t" + tempTreeView.Nodes[i].Text);
                        }
                    }
                }
                else if (TargetControl is CheckedListBox)
                {
                    //CheckedListBox
                    CheckedListBox tempCheckedListBox = TargetControl as CheckedListBox;
                    SaveTxtFile(TargetTextFileName, tempCheckedListBox.Name + "\t" + tempCheckedListBox.Text);
                    if (tempCheckedListBox.Items.Count > 0)
                    {
                        for (int i = 0; i < tempCheckedListBox.Items.Count; i++)
                        {
                            SaveTxtFile(TargetTextFileName, tempCheckedListBox.Items[i].ToString());
                        }
                    }
                }
                else if (TargetControl is ListBox)
                {
                    //ListBox
                    ListBox tempListBox = TargetControl as ListBox;
                    SaveTxtFile(TargetTextFileName, tempListBox.Name + "\t" + tempListBox.Text);
                    if (tempListBox.Items.Count > 0)
                    {
                        //发生错误：值不能为 null。
                        //参数名: item;    在 System.Windows.Forms.ListBox.ObjectCollection.AddInternal(Object item)

                        for (int i = 0; i < tempListBox.Items.Count; i++)
                        {
                            SaveTxtFile(TargetTextFileName, tempListBox.Items[i] + "\t");
                        }
                    }
                }
                else if (TargetControl is ListView)
                {
                    //ListView
                    ListView tempListView = TargetControl as ListView;
                    SaveTxtFile(TargetTextFileName, tempListView.Name + "\t" + tempListView.Text);
                    if (tempListView.Items.Count > 0)
                    {
                        //ListViewItem[] tempTreeNodes = new ListViewItem[tempListView.Items.Count];
                        for (int i = 0; i < tempListView.Items.Count; i++)
                        {
                            SaveTxtFile(TargetTextFileName, tempListView.Items[i].Name + "\t" + tempListView.Items[i].Text);
                        }
                    }
                    else if (TargetControl is ComboBox)
                    {
                        //ComboBox
                        ComboBox tempComboBox = TargetControl as ComboBox;
                        SaveTxtFile(TargetTextFileName, tempComboBox.Name + "\t" + tempComboBox.Text);
                        if (tempComboBox.Items.Count > 0)
                        {
                            // 发生错误：值不能为 null。
                            // 参数名: item;    在 System.Windows.Forms.ComboBox.ObjectCollection.AddInternal(Object item)

                            for (int i = 0; i < tempComboBox.Items.Count; i++)
                            {
                                SaveTxtFile(TargetTextFileName, tempComboBox.Items[i].ToString() + "\t");
                            }
                        }
                    }
                    else
                    {
                        if (TargetControl.HasChildren == true)
                        {
                            SaveTxtFile(TargetTextFileName, TargetControl.Name + "\t" + TargetControl.Text);
                            foreach (Control item in TargetControl.Controls)
                            {
                                if (null != item)
                                {
                                    SaveTxtFile(TargetTextFileName, item.Name + "\t" + item.Text);
                                    if (item.HasChildren == true)
                                    {
                                        SaveAllControlNamesAndItsText(TargetTextFileName, item);
                                    }
                                }
                            }
                        }
                        else
                        {
                            SaveTxtFile(TargetTextFileName, TargetControl.Name + "\t" + TargetControl.Text);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //throw ex;
                //ErrorMessage.Enqueue(DateTime.Now.ToString() + "*-*" + "发生错误：" + ex.Message + "; " + ex.StackTrace);
            }

        }

        /// <summary>
        /// 在源字符串中查找目标字符串的重复数量
        /// </summary>
        /// <param name="SourceString">源字符串</param>
        /// <param name="TargetString">目标字符串</param>
        /// <returns></returns>
        public static int FindTargetStringCountInSource(string SourceString, string TargetString)
        {
            int iCountOfPercentSplitChar = 0;
            int iStart = 0;

            try
            {
                do
                {
                    if (iStart + 1 < SourceString.Length)
                    {
                        if ((iStart = Strings.InStr(iStart + 1, SourceString, TargetString)) != -1)
                        {
                            iCountOfPercentSplitChar++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                } while (true);

                return iCountOfPercentSplitChar;
            }
            catch (Exception)
            {
                return iCountOfPercentSplitChar;
            }
        }
        
        }//class

    }//namespace