using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PengDongNanTools;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using Microsoft.VisualBasic;

namespace PengDongNanTools
    {

    public partial class Form2 : Form
        {
        public Form2()
            {
            InitializeComponent();
            }

        //TCPIPAsyncServer pp;
        //TCPIPAsyncClient ss;
        //TcpClient cc;
        CommonFunction FC = new CommonFunction("彭东南");
        XMLOperation XML = new XMLOperation("彭东南");

        Delay Delay1 = new Delay("彭东南");

        System.Threading.Thread TheadTest = null;

        private void Form2_Load(object sender, EventArgs e)
            {
            //pp = new TCPIPAsyncServer(8000, 5, ref this.richTextBox1, "彭东南");
            //ss = new TCPIPAsyncClient("localhost", 8000, ref this.richTextBox1, "彭东南");
            //cc = new TcpClient("localhost", 8000);
            //string[] aa = { "987645123" };

            //ss.AutoSendInterval = 1000;
            //ss.SendMessage = aa;
            //ss.AutoSend = true;

            //string[] xt = { "1" };//+ System.DateTime.Now };
            //pp.SendMessage = xt;

            //pp.AutoSend = true;




            }
        int x = 0;
        string[] y;
        private void button1_Click(object sender, EventArgs e)
            {
            ////截屏
            //Bitmap tt = new Bitmap(1024, 768);
            //Graphics dd = Graphics.FromImage(tt);
            //dd.CopyFromScreen(0,0,0,0,new Size(1024,768));
            //tt.Save("e:\\aa.bmp");

            //FC.ScreenShot();

            byte a = new byte();
            a = 100;

            richTextBox1.AppendText(FC.ConvertByteToBinaryStringFormat(a) + "\r\n");
            richTextBox1.AppendText(FC.ConvertByteToHexStringFormat(a) + "\r\n");

            richTextBox1.AppendText(FC.ConvertIntToBinaryStringFormat(65535) + "\r\n");
            richTextBox1.AppendText(FC.ConvertIntToHexStringFormat(65535) + "\r\n");

            panel1.Controls.Add(this.richTextBox1);
            panel1.Controls.Add(this.richTextBox2);

            //richTextBox1.AppendText(panel1.);

            //richTextBox1.AppendText(DateTime.Now.ToString() + "  " + DateTime.Now.Millisecond + "\r\n");

            if (TheadTest == null)
                {
                TheadTest = new System.Threading.Thread(ChangePos);
                TheadTest.IsBackground = true;
                TheadTest.Start();
                }

            Delay1.Start();

            richTextBox1.AppendText(Delay1.ErrorMessage + "\r\n");

            Temps = false;

            //richTextBox1.AppendText(XML.OpenXMLFile("data").ToString());

            //XML.ClearDataRecordsInXMLFile("e:\\data");
            //XML.DelDataRecordsInXMLFile("data", "TestPara");

            //y= XML.FindInnerTextInXMLFile("data", "TestPara");

            //for (int a = 0; a < y.Length; a++) 
            //    {
            //    richTextBox1.AppendText(y[a] + "\r\n");
            //    }

            //x += 1;
            //y = new string[x];

            //for (int a = 0; a < y.Length; a++) 
            //    {
            //    y[a] = a.ToString();// +" - " + DateTime.Now;
            //    }
            //if (ss != null) 
            //    {
            //    ss.SendMessage = y;
            //    ss.SendMessage = y;
            //    }
            }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
            {
            //if (xx != null)
            //    {
            //    xx.Dispose();

            //    xx = null;
            //    }


            aa = null;


            //if (pp != null)
            //    {
            //    pp.Close();
            //    pp = null;
            //    }

            //if (ss != null)
            //    {
            //    ss.Close();
            //    ss = null;
            //    }

            //if (cc != null)
            //    {
            //    cc.Close();
            //    cc = null;
            //    }
            FC.Dispose();
            XML.Dispose();
            TheadTest = null;

            }

        string[] Temp = null;
        
        private void timer1_Tick(object sender, EventArgs e)
            {
            //if (pp != null) 
            //    {
            //    //this.richTextBox2.Text = "";
            //    string str = "";
            //    if (pp.ConnectedClients.Length > 0) 
            //        {
            //        for (int a = 0; a < pp.ConnectedClients.Length; a++)
            //            {
            //            str = str + pp.ConnectedClients[a] + "\r\n";
            //            }
            //        this.richTextBox2.Text = str;
            //        }

                //Temp = new string[pp.ConnectedClients.Length];
                

                //}

            //if (cc != null) 
            //    {
            //    this.richTextBox2.AppendText(FC.GetRemoteIP(cc) + " " + FC.GetRemotePort(cc).ToString() + "\r\n");
            //    }
            }

        private void button3_Click(object sender, EventArgs e)
            {
            //if (pp == null) 
            //    {
            //    pp = new TCPIPAsyncServer(8000, 5, ref this.richTextBox1, "彭东南");
            //    pp.AutoSend = true;
            //    }

            //if (ss == null)
            //    {
            //    ss = new TCPIPAsyncClient("localhost", 8000, ref this.richTextBox1, "彭东南");
            //    }  
            //if (cc == null)
            //    {
            //    cc = new TcpClient("localhost", 8000);
            //    }
            }

        private void button2_Click(object sender, EventArgs e)
            {
            
            richTextBox1.AppendText("A的ASCII码是" + Strings.Asc("A") + "\r\n");
            richTextBox1.AppendText("Z的ASCII码是" + Strings.Asc("Z") + "\r\n");

            richTextBox1.AppendText(x.ToString() + "\r\n");

            richTextBox1.AppendText(Strings.Format(FC.GetDriveFreeSpaceInMB('G'), "##########.##") + "\r\n");
            richTextBox1.AppendText(Strings.Format(FC.GetDriveFreeSpaceInMB('G')/1024, "##########.##") + "\r\n");
            richTextBox1.AppendText(FC.ErrorMessage + "\r\n");

            //if (pp != null) 
            //    {
            //    pp.Close();
            //    pp = null;
            //    }

            //if (ss != null)
            //    {
            //    ss.Close();
            //    ss = null;
            //    }

            //if (cc != null)
            //    {
            //    cc.Close();
            //    cc = null;
            //    }
            }
        string[] ass={""};
        int i = 0;

        //ExcelOperation xx = new ExcelOperation("彭东南");
        string[] aa = new string[20];
        double[] bb = new double[20];
        int[] cc = new int[20];
        Random oo = new Random();
        string[][] ttt = new string[20][];
        int[][] iii = new int[20][];
        double[][] ddd = new double[20][];

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
            {

            checkBox1.Enabled = false;

            for (int a = 0; a < 20; a++) 
                {
                aa[a] = a.ToString() +" ::";
                bb[a] = oo.NextDouble();
                cc[a] = a;
                ttt[a] = new string[3];iii[a] = new int[3];
                ddd[a] = new double[3];
                for (int b = 0; b < 3; b++) 
                    {
                    ttt[a][b] =a.ToString() + " " + b.ToString();
                    iii[a][b] = a * b;
                    ddd[a][b] = oo.NextDouble();
                    }
                }
            
            //CommonFunction.ExcelFileType CC =new CommonFunction.ExcelFileType();
            richTextBox1.AppendText("Start: " + DateTime.Now.ToString() + DateTime.Now.Millisecond.ToString() + "\r\n");
            FC.QuickAppendSingleRowDataToExcel("e:\\aaa", CommonFunction.ExcelFileType.xls, aa,true);
            FC.QuickAppendSingleRowDataToExcel("e:\\aaa", CommonFunction.ExcelFileType.xls, bb);
            FC.QuickAppendMultiRowsDataToExcel("e:\\aaa", CommonFunction.ExcelFileType.xls, ttt);
            FC.QuickAppendMultiRowsDataToExcel("e:\\aaa", CommonFunction.ExcelFileType.xls, iii);
            FC.QuickAppendMultiRowsDataToExcel("e:\\aaa", CommonFunction.ExcelFileType.xls, ddd);
            richTextBox1.AppendText("End: " + DateTime.Now.ToString() + DateTime.Now.Millisecond.ToString() + "\r\n");


            //string[] ProcessName = FC.GetLocalProcess("svchost");//FC.GetLocalProcess()

            //if (ProcessName != null)
            //    {
            //    for (int b = 0; b < ProcessName.Length; b++)
            //        {
            //        richTextBox1.AppendText(ProcessName[b] + "\r\n");
            //        }
            //    }

            //string[] AllIP = FC.SearchAvailableIPAddresses();
            //if (AllIP != null) 
            //    {
            //    for (int a = 0; a < AllIP.Length; a++) 
            //        {
            //        string[] ProcessName = FC.GetProcess(AllIP[a]);
            //        if (ProcessName != null) 
            //            {
            //            for (int b = 0; b < ProcessName.Length; b++) 
            //                {
            //                richTextBox1.AppendText(AllIP[a] + "  " + ProcessName[b] + "\r\n");
            //                }
            //            }
            //        }                
            //    }


            if (checkBox1.Checked == true)
                {
                this.richTextBox1.Visible = true;
                }
            else 
                {
                this.richTextBox1.Visible = false;
                }
            //Array.Resize<string>(ref ass, i + 1);
            
            ////ass =new  string[i + 1];
            //ass[i] = i.ToString();

            //i += 1;

            //ass = FC.ReadLinesFromTXTFile("e:\\tt.txt");
            //if (ass != null) 
            //    {
            //    for (int a = 0; a < ass.Length; a++)
            //        {
            //        richTextBox1.AppendText(ass[a] + "\r\n");
            //        }
            //    }

            //FC.SaveStringToTXTFile("a额aa\r\n"+"啊看", "E:\\EEE");

            //string[] aaaa = new string[3];
            //aaaa[0] = "aa导入a";
            //aaaa[1] = "的方法";
            //aaaa[2] = "是撒";



            //FC.SaveStringToTXTFile(aaaa, "E:\\ee");

            //if (richTextBox1.Text != "") 
            //    {
            //    FC.SaveStringToTXTFile(ref richTextBox1, "E:\\tt");
            //    }
            
            //FC.SaveStringToTXTFile("123", "E:\\tt");

            checkBox1.Enabled = true;

            }

        bool Temps = false;
        private void ChangePos() 
            {

            int x = 1, y = 0;
            x = button2.Left;
            y = button2.Top;
            Control xx = button2;

            bool reset = false;
            
            while (true)
                {
                try
                    {



                    FC.ChangeControlText(ref xx, Delay1.PassedTime.ToString());

                    //FC.ChangeControlEnableStatus(ref xx, Delay1.Wait(2.5));//Delay1.Wait(2.5,true));

                    if (Temps == false && Delay1.Wait(2.5, true))//Delay1.Wait(2.5))
                        {
                        FC.AddRichText(ref richTextBox1, Delay1.PassedTime.ToString());
                        //Delay1.Reset();
                        //Delay1.Start();
                        reset = true;
                        Temps = true;
                        }

                    if (reset == true) 
                        {
                        //Delay1.Reset();
                        Delay1.Start();
                        reset = false;
                        Temps = false;
                        }

                    //xx = button2;
                    //FC.ChangeControlPosition(ref xx, x, y);
                    //System.Threading.Thread.Sleep(1000);
                    //x += 11;
                    //y += 10;

                    //if (x / 10 % 2 == 1)
                    //    {
                    //    FC.ChangeControlBackColor(ref xx, Color.Red);
                    //    FC.ChangeControlEnableStatus(ref xx, true); 
                    //    FC.ChangeControlForeColor(ref xx, Color.Yellow);
                    //    FC.ChangeControlText(ref xx, x.ToString());
                    //    }
                    //else 
                    //    {
                    //    FC.ChangeControlBackColor(ref xx, Color.Green); 
                    //    FC.ChangeControlEnableStatus(ref xx, false);
                    //    FC.ChangeControlForeColor(ref xx, Color.Blue);
                    //    FC.ChangeControlText(ref xx, y.ToString());
                    //    }

                    //xx = button3;
                    //FC.ChangeControlPosition(ref xx, x+20, y+30);
                    //if (x / 10 % 2 == 1)
                    //    {
                    //    FC.ChangeControlBackColor(ref xx, Color.Red);
                    //    FC.ChangeControlEnableStatus(ref xx, true);
                    //    FC.ChangeControlForeColor(ref xx, Color.Yellow);
                    //    FC.ChangeControlText(ref xx, x.ToString());
                    //    }
                    //else
                    //    {
                    //    FC.ChangeControlBackColor(ref xx, Color.Green);
                    //    FC.ChangeControlEnableStatus(ref xx, false);
                    //    FC.ChangeControlForeColor(ref xx, Color.Blue);
                    //    FC.ChangeControlText(ref xx, y.ToString());
                    //    }
                    }
                catch (Exception ex)
                    {
                    FC.AddRichText(ref richTextBox1, ex.Message);
                    }
                }
            }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
            {

            }

        }
    }
