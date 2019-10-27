#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop;
using Microsoft.VisualBasic;

#endregion

#region "NET导出Excel遇到的80070005错误的解决方法"

//'NET导出Excel遇到的80070005错误的解决方法: 
//一：
//1、在"开始"->"运行"中输入dcomcnfg.exe启动"组件服务" ；
//2、依次双击"组件服务"->"计算机"->"我的电脑"->"DCOM配置"；
//3、右击"WPS表格 工作簿"，选中"属性"--"标识"--"交互式用户"就OK了；

//二：
//1、在命令行中输入：dcomcnfg，会显示出“组件服务”管理器 
//2、打开“组件服务->计算机->我的电脑->DCOM 配置”，找到“Microsoft Word文档”，单击右键，选择“属性” 
//3、在“属性”对话框中单击“安全”选项卡，在“启动和激活权限”处选择“自定义”，再单击右边的“编辑”，
//   在弹出的对话框中添加“ASPNET”（在IIS6中是NETWORD SERVICE）用户，给予“本地启动”和“本地激活”的
//   权限，单击“确定
//4、在“属性”对话框中单击“安全”选项卡，在“访问权限”处选择“自定义”，再单击右边的“编辑”，在弹出
//   的对话框中添加“ASPNET”（在IIS6中是NETWORD SERVICE）用户，给予“本地访问”的权限，单击“确定”，
//   关闭“组件服务”管理器。 

//三：
//消息筛选器显示应用程序正在使用中。  
//说明: 执行当前 Web 请求期间，出现未处理的异常。请检查堆栈跟踪信息，以了解有关该错误以及代码中导致错误的出处的详细信息。  
//异常详细信息: System.Runtime.InteropServices.COMException: 消息筛选器显示应用程序正在使用中。 
//1、在命令行中输入：dcomcnfg，会显示出“组件服务”管理器 
//2、打开“组件服务->计算机->我的电脑->DCOM 配置”，找到“Microsoft Word文档”，单击右键，选择“属性”
//3、在“属性”对话框中单击“标识”选项卡，选择“交互式用户””，关闭“组件服务”管理器。

//修改好之后如果还不行，在 组件服务->计算机->我的电脑 上右键 "停止MS DTC"服务，然后再重启就可以了


#endregion

namespace PengDongNanTools
    {

    //Excel文件的相关操作类【软件作者：彭东南, southeastofstar@163.com】
    /// <summary>
    /// Excel文件的相关操作类【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    class ExcelOperation
        {
        
        #region "变量定义"

        /// <summary>
        /// Excel工作表计算接口
        /// </summary>
        public Microsoft.Office.Interop.Excel.WorksheetFunction ExcelCalculate;

        /// <summary>
        /// Excel软件操作接口
        /// </summary>
        public Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();

        /// <summary>
        /// Excel工作薄操作接口
        /// </summary>
        public Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;// = new Microsoft.Office.Interop.Excel.Workbook();

        /// <summary>
        /// Excel工作表操作接口
        /// </summary>
        public Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;// = new Microsoft.Office.Interop.Excel.Worksheet();

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
        /// 提示信息
        /// </summary>
        public string ErrorMessage = "";

        private bool SuccessBuiltNew = false, PasswordIsCorrect = false;

        Microsoft.VisualBasic.Devices.Computer PC = new Microsoft.VisualBasic.Devices.Computer();
        CommonFunction FC = new CommonFunction("彭东南");
        
        #endregion
        
        #region "函数代码"
        
        //实例化函数：建立Excel文件操作的新实例
        /// <summary>
        /// 实例化函数：建立Excel文件操作的新实例
        /// </summary>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public ExcelOperation(string DLLPassword)
            {

            SuccessBuiltNew = false;
            PasswordIsCorrect = false;

            try
                {

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
            catch (Exception ex)
                {
                SuccessBuiltNew = false;
                MessageBox.Show("创建类的实例时出现错误！\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }
            
            }

        
        public bool SetConditionColor()
            {
            try
                {
                
                int Return = 0;

                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }

            }







        
        //--------------重新整理的CP/CPK计算公式---------------
        //USL=Normal+MaxTolerance                                 【正常值+上限允差范围值】
        //LSL=Normal+MinTolerance[负数值，如果为正数值则是-]      【正常值+下限允差范围值】 
        //CP=(MaxTolerance+ABS(MinTolerance))/(6*StdDev)
        //UCPK=(MAX-MEAN)/(3*StdDev)
        //LCPK=(MEAN-MIN)/(3*StdDev)
        //CPK=UCPK和LCPK之中小的值
        //标准差S，用STDEN求出


        public bool CalculateCPK()
            {
            try
                {

                int Return = 0;

                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        public bool Max()
            {
            try
                {

                int Return = 0;

                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }


        public bool Min()
            {
            try
                {

                int Return = 0;

                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }


        public bool Average()
            {
            try
                {

                int Return = 0;

                return true;
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
                FC.Dispose();
                FC = null;
                
                ExcelWorkSheet = null;
                ExcelWorkBook = null;
                ExcelApp.Quit();

                }
            catch (Exception)// ex)
                {
                }
            }

        #endregion

        }//class

    }//namespace