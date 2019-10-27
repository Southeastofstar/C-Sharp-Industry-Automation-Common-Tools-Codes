#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace PengDongNanTools
    {

    //【二次封装】控制固高运动控制器轴运动的类
    /// <summary>
    /// 【二次封装】控制固高运动控制器轴运动的类
    /// </summary>
    public class GoogolTechGTSAxisControl
        {

        #region "变量定义"

        public string ErrorMessage = "";
        private bool SuccessBuiltNew = false, PasswordIsCorrect = false;

        /// <summary>
        /// 运动控制卡号
        /// </summary>
        private short CardNumber;

        /// <summary>
        /// 运动控制卡轴号
        /// </summary>
        private short AxisNumber;





        #endregion

        #region "属性设置"

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }












        #endregion

        #region "函数代码"









        #endregion

        }//class

    }//namespace