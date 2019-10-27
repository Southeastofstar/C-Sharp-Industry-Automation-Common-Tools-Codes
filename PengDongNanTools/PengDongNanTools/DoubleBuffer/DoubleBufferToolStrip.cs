using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace PengDongNanTools.DoubleBuffer
{
    /// <summary>
    /// 利用映射方法对 ListView 控件进行双缓冲处理，解决闪烁问题
    /// </summary>
    public static class DoubleBufferToolStrip
    {
        /// <summary>
        /// 利用映射方法对 ToolStrip 控件进行双缓冲处理，解决闪烁问题
        /// </summary>
        /// <param name="TargetToolStrip">目标ToolStrip控件</param>
        /// <param name="EnableDoubleBuffer">是否启用双缓冲处理</param>
        public static void DoubleBufferedToolStrip(this ToolStrip TargetToolStrip, bool EnableDoubleBuffer)
        {

            Type lvType = TargetToolStrip.GetType();
            PropertyInfo pi = lvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(TargetToolStrip, EnableDoubleBuffer, null);
        }

    }//class

}
