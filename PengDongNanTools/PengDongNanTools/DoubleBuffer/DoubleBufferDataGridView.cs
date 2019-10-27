using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace PengDongNanTools.DoubleBuffer
{
    /// <summary>
    /// 利用映射方法对 DataGridView 控件进行双缓冲处理，解决闪烁问题
    /// </summary>
    public static class DoubleBufferDataGridView
    {
        /// <summary>
        /// 利用映射方法对 DataGridView 控件进行双缓冲处理，解决闪烁问题
        /// </summary>
        /// <param name="TargetDataGridView">目标DataGridView控件</param>
        /// <param name="EnableDoubleBuffer">是否启用双缓冲处理</param>
        public static void DoubleBufferedDataGirdView(this DataGridView TargetDataGridView, bool EnableDoubleBuffer)
        {
            Type dgvType = TargetDataGridView.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(TargetDataGridView, EnableDoubleBuffer, null);
        }

    }//class
}
