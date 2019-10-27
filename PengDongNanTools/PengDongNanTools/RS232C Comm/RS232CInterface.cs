#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;

#endregion

//ok

namespace PengDongNanTools
    {

    //RS232C接口
    /// <summary>
    /// RS232C接口【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    public interface RS232CInterface
        {

        /// <summary>
        /// 搜索可用RS232C串口
        /// </summary>
        /// <returns>返回搜索到的串口名称字符串数组</returns>
        string[] SearchAvailableRS232Ports();

        /// <summary>
        /// 搜索计算机上可用RS232C串口数量
        /// </summary>
        /// <returns></returns>
        int AvailableRS232CPortCount();

        /// <summary>
        /// 将ASCII码字符串转换为16进制码【HEX】
        /// </summary>
        /// <param name="TargetString">字符串</param>
        /// <returns></returns>
        string StringConvertToHEX(string TargetString);
                
        }

    //RS232CInterface接口的实现类
    /// <summary>
    /// RS232CInterface接口的实现类【软件作者：彭东南, southeastofstar@163.com
    /// </summary>
    public class RS232C : RS232CInterface
        {

        #region "变量定义"

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }

        string[] AvailablePortNames;
        int AvailableRS232CPorts;

        private Button ButtonUseForInvoke = new Button();

        string ErrorMessage = "";
        //string TempErrorMessage = "";

        /// <summary>
        /// 当错误信息相同时，是否重复显示
        /// </summary>
        public bool UpdatingSameMessage = true;

        bool SuccessBuiltNew = false;
        bool PasswordIsCorrect= false;

        //利用委托和代理进行跨线程安全操作控件，以此避免跨线程操作异常
        //*****************************
        private delegate void AddMessageToRichTextBox(ref System.Windows.Forms.RichTextBox TargetRichTextBox, string TargetText);
        //private AddMessageToRichTextBox ActualAddMessageToRichTextBox;
        private delegate void AddMessageToTextBox(ref System.Windows.Forms.TextBox TargetTextBox, string TargetText);
        //private AddMessageToTextBox ActualAddMessageToTextBox;
        private delegate void ClearTextBoxDelegate(ref System.Windows.Forms.TextBox TargetTextBox);
        private delegate void ClearRichTextBoxDelegate(ref System.Windows.Forms.RichTextBox TargetRichTextBox);

        //*****************************
        private delegate void ChangeButtonEnableStatusDelegate(ref System.Windows.Forms.Button TargetControl, bool TargetEnabledStatus);
        private delegate void ChangeMenuEnableStatusDelegate(ref System.Windows.Forms.ToolStripMenuItem TargetControl, bool TargetEnabledStatus);
        private delegate void ChangeStatusLabelEnableStatusDelegate(ref System.Windows.Forms.ToolStripStatusLabel TargetControl, bool TargetEnabledStatus);
        //*****************************
        private delegate void ChangeStatusLabelTextDelegate(ref System.Windows.Forms.ToolStripStatusLabel TargetControl, string TargetText);
        //*****************************
        private delegate void ChangeStatusLabelBackColorDelegate(ref System.Windows.Forms.ToolStripStatusLabel TargetControl, System.Drawing.Color TargetColor);
        private delegate void ChangeMenuBackColorDelegate(ref System.Windows.Forms.ToolStripMenuItem TargetControl, System.Drawing.Color TargetColor);
        private delegate void ChangeButtonBackColorDelegate(ref System.Windows.Forms.Button TargetControl, System.Drawing.Color TargetColor);
        //*****************************
        
        #endregion

        #region "已完成代码"

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <returns></returns>
        public string ErrorMessageString()
            {
            return ErrorMessage;
            }

        //创建实现RS232C接口类的实例
        /// <summary>
        ///  创建实现RS232C接口类的实例
        /// </summary>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public RS232C(string DLLPassword)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;

            if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan") || (DLLPassword == "彭东南"))
                {
                PasswordIsCorrect = true;
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
        
        #endregion

        }//interface

    }//namespace