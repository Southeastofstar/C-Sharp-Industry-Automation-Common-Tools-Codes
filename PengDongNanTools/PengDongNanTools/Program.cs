using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace PengDongNanTools
    {
    static class Program
        {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
            {

            try
                {
                //***************
                //预防一个程序多次开启运行
                //bool SoftwareAlreadyOpened = false;
                //Mutex MutexCheck = new Mutex(false, "software", out SoftwareAlreadyOpened);
                //if (!SoftwareAlreadyOpened)
                //    {
                //    MessageBox.Show("The software was already opened, you can't run another copy at the same time.\r\n出于安全考虑，不允许同时运行此软件的另外一个副本.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    return;
                //    }
                //***************

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                ////***************
                //frmPCI1754 xx =new frmPCI1754(1, "彭东南");
                //xx.JustHideFormAtClosing = false;
                //xx.ShowPromptAtClosing=true;
                ////Application.Run(new frmPCI1754(1, "彭东南"));
                //Application.Run(xx);
                ////***************

                ////Application.Run(new frmPCI1752(1,"彭东南"));
                //frmPCI1752 yy = new frmPCI1752(1, "彭东南");
                //yy.JustHideFormAtClosing = false;
                //yy.ShowPromptAtClosing = true;
                ////Application.Run(new frmPCI1754(1, "彭东南"));
                //Application.Run(yy);
                ////***************

                //Application.Run(new TCPAsyncClientForm("彭东南"));
                //Application.Run(new TCPAsyncServerForm("彭东南"));
                //Application.Run(new Form2());
                //Application.Run(new Form1());
                //Application.Run(new RS232CForm("彭东南"));
                //Application.Run(new TCPServerForm("彭东南"));
                //Application.Run(new TCPClientForm("彭东南"));

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }

            }//function

        }//class

    }//namespace