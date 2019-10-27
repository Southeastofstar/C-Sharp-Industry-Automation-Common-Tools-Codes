using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PengDongNanTools
    {
    /// <summary>
    /// 关于软件作者
    /// </summary>
    public partial class frmAbout : Form
        {

        //关于软件作者的新实例构造函数
        /// <summary>
        /// 关于软件作者的新实例构造函数
        /// </summary>
        public frmAbout()
            {
            InitializeComponent();
            }

        private void OK_Button_Click(object sender, System.EventArgs e)
            {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            }

        private void About_Load(object sender, System.EventArgs e)
            {

            }

        private void About_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
            {
            this.Hide();
            e.Cancel = true;
            }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
            {

            }

        }
    }
