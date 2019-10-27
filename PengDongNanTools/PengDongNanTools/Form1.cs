using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace PengDongNanTools
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Form1 : Form
    {
        TCPIPClient Test;

        private string xx;

        /// <summary>
        /// 
        /// </summary>
        public string yy
            {
            get { return xx; }
            set { xx=value;}//value是系统默认的
            }

        /// <summary>
        /// 
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, EventArgs e)
            {

            if (Test == null)
                {
                //Test = new SimpleTCPIPClient("192.168.1.112", 8888, "ThomasPeng");
                Test = new TCPIPClient("127.0.0.1", 2000, "ThomasPeng");
                Test.Start();
                }
            else 
                {
                MessageBox.Show("You already opened the client station.");
                }
            
            }

        private void timer1_Tick(object sender, EventArgs e)
            {

            //if (ll.ReceiveStr != "") 
            //    {
            //    richTextBox1.AppendText(ll.ReceiveStr  + "\r\n");
            //    ll.ReceiveStr = "";
            //    }

            if (!(Test == null)) 
                {
                if (Test.ReceivedString != "") 
                    {
                    richTextBox1.AppendText(Test.ReceivedString + "\r\n");
                    Test.ReceivedString = "";
                    }

                if (Test.ErrorMessage != "") 
                    {
                    richTextBox1.AppendText(Test.ErrorMessage + "\r\n");
                    Test.ErrorMessage = "";
                    }
                }

            if (!(xxxx == null))
                {
                if (xxxx.ReceivedString != "")
                    {
                    richTextBox1.AppendText(xxxx.ReceivedString + "\r\n");
                    xxxx.ReceivedString = "";
                    }

                if (xxxx.ErrorMessage != "")
                    {
                    richTextBox1.AppendText(xxxx.ErrorMessage + "\r\n");
                    xxxx.ErrorMessage = "";
                    }
                }

            if (!(RSPort == null))
                {
                if (RSPort.ReceivedString != "")
                    {
                    richTextBox1.AppendText(RSPort.ReceivedString + "\r\n");
                    RSPort.ReceivedString = "";
                    }

                if (RSPort.ErrorMessage != "")
                    {
                    richTextBox1.AppendText(RSPort.ErrorMessage + "\r\n");
                    RSPort.ErrorMessage = "";
                    }
                }

            }

        private void Form1_Load(object sender, EventArgs e)
            {

            int[] x=new int[5];

            richTextBox1.AppendText("\r\nint[] x=new int[5]: length is " + x.Length.ToString() + "\r\n");
            
            for (int a = 0; a < x.Length;a++ ) 
                {
                richTextBox1.AppendText(a.ToString()+"\r\n");
                }

            for (int a = 0; a <= x.Length; a++)
                {
                richTextBox1.AppendText(a.ToString()+"\r\n");
                }

            int w = 1;
            for (int a = 0; a <= 16; a++) 
                {
                richTextBox1.AppendText("1 左移" + a +"位的结果是：" + (w<<a) + "\r\n");
                }

            for (int a = 0; a <= 16; a++)
                {
                richTextBox1.AppendText("1 右移" + a + "位的结果是：" + (w >> a) + "\r\n");
                }

            int u = 65535;
            for (int a = 0; a <= 16; a++)
                {
                richTextBox1.AppendText("65535 左移" + a + "位的结果是：" + (u << a) + "\r\n");
                }

            for (int a = 0; a <= 16; a++)
                {
                richTextBox1.AppendText("65535 右移" + a + "位的结果是：" + (u >> a) + "\r\n");
                }

            u = 65536;
            for (int a = 0; a <= 16; a++)
                {
                richTextBox1.AppendText("65536 左移" + a + "位的结果是：" + (u << a) + "\r\n");
                }

            for (int a = 0; a <= 16; a++)
                {
                richTextBox1.AppendText("65536 右移" + a + "位的结果是：" + (u >> a) + "\r\n");
                }

            timer1.Start();

            //ll = new TCPIPAsyncClient("localhost", 8000, ref this.richTextBox1, "彭东南");
            pp = new TCPIPAsyncServer(8000, 5, ref this.richTextBox1, "彭东南");
            
            string[] xt = { "1" + System.DateTime.Now };
            pp.SendMessage=xt;

            pp.AutoSend = true;

            //ReadData = new Thread(ReadDataFromServer);
            //ReadData.IsBackground = true;
            //ReadData.Start();

            }

        Thread ReadData;

        //private void ReadDataFromServer() 
        //    {
        //    while (true) 
        //        {
        //        try
        //            {
        //            //int ss=Strings.InStr(ll.ReceiveStr,"\0");
        //            int ss = ll.ReceiveMsg.IndexOf("\0");
        //            if (ll.ReceiveMsg != "" && ss==-1)
        //                {
        //                //MessageBox.Show(ll.ReceiveStr);

        //                ww.AddRichText(ref richTextBox1, ll.ReceiveMsg + "\r\n");
        //                //ll.ReceiveStr = "";
        //                }
        //            ll.ReceiveMsg = "";
        //            }
        //        catch (Exception)// ex)
        //            {

        //            }
        //        }

        //    }

        private void btnSend_Click(object sender, EventArgs e)
            {
            if (!(Test == null))
                {
                if (richTextBox2.Text != "")
                    {
                    Test.Send(richTextBox2.Text);
                    }
                else 
                    {
                    MessageBox.Show("The contents you want to send is empty.");
                    richTextBox2.Focus();
                    }                
                }
            else
                {
                MessageBox.Show("You did not open the client station.");
                }
            }

        private void button1_Click(object sender, EventArgs e)
            {
            //MessageBox.Show("This is \" a \'test'.");
            }

        CommonFunction ww = new CommonFunction("ThomasPeng");
        
        private void button2_Click(object sender, EventArgs e)
            {

            //System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;

            ww.ClearRichTextBoxContents(ref richTextBox1);
            //ww.AddRichText(ref richTextBox1, "a\r");

            

            ww.ChangeButtonEnableStatus(ref button1 , false);

            var xx = new System.Timers.Timer();
            xx.Interval = 200;
            xx.Enabled = false;
            xx.Elapsed += (o, a) =>
                {

                //zz.AddText(ref richTextBox1, "a\r\n");

                ww.ClearRichTextBoxContents(ref richTextBox1);

                //ww.ChangeEnableStatus(Form1,ref button1, false);

                ww.AddRichText(ref richTextBox1, "  a\r\n");


                //xx.SynchronizingObject = richTextBox1;
                //richTextBox1.AppendText(DateTime.Now + "   a\r\n");
                };

            


            //System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;

            //System.Threading.Timer xx = new System.Threading.Timer();
            //StartTimer(200);

            //public static void Main()
            //{
            //    // Create an instance of the Example class, and start two
            //    // timers.
            //    Example ex = new Example();
            //    ex.StartTimer(2000);
            //    ex.StartTimer(1000);

            //    Console.WriteLine("Press Enter to end the program.");
            //    Console.ReadLine();
            //}

            //public void StartTimer(int dueTime)
            //{
            //    Timer t = new Timer(new TimerCallback(TimerProc));
            //    t.Change(dueTime, 0);
            //}

            //private void TimerProc(object state)
            //{
            //    // The state object is the Timer object.
            //    Timer t = (Timer) state;
            //    t.Dispose();
            //    Console.WriteLine("The timer callback executes.");
            //}


            }

        /// <summary>
        /// 启动计时器
        /// </summary>
        /// <param name="dueTime"></param>
        public void StartTimer(int dueTime) 
            {
            System.Threading.Timer t = new System.Threading.Timer(new System.Threading.TimerCallback (AddText));
            t.Change(dueTime,100);
            }

        private void AddText(object state) 
            {
            System.Threading.Timer t=(System.Threading.Timer)state;
            
           
            richTextBox1.AppendText(DateTime.Now + "   a");

            t.Dispose();

            }

        private TCPIPServer xxxx;

        private void button3_Click(object sender, EventArgs e)
            {

             xxxx = new TCPIPServer(2000, "ThomasPeng");
            xxxx.Start();

            }

        private void button4_Click(object sender, EventArgs e)
            {

            if (!(xxxx == null))
                {
                if (richTextBox2.Text != "")
                    {
                    xxxx.Send(richTextBox2.Text);
                    }
                else
                    {
                    MessageBox.Show("The contents you want to send is empty.");
                    richTextBox2.Focus();
                    }
                }
            else
                {
                MessageBox.Show("You did not open the client station.");
                }

            }

        private void button6_Click(object sender, EventArgs e)
            {

            if (Test != null)
                {         
                Test.Close();
                Test = null;
                }

            }

        private void button5_Click(object sender, EventArgs e)
            {


            if (xxxx != null)
                {
                xxxx.Close();
                xxxx = null;
                }

            }

        RS232C bb;
        RS232CInterface cc;
        private void button7_Click(object sender, EventArgs e)
            {

            bb = new RS232C("ThomasPeng");
            cc = bb;
            //以下效果相同,接口的实现方式与VB中不同
            //MessageBox.Show(cc.AvailableRS232CPortCount().ToString());
            MessageBox.Show(bb.AvailableRS232CPortCount().ToString());

            }

        private PengDongNanTools.RS232Comm RSPort;

        private void btnRS232C_Click(object sender, EventArgs e)
            {

            RSPort = new RS232Comm("ThomasPeng",ref this.richTextBox2);
            RSPort.OpenPort(1);

            }

        private void btnRS232CSend_Click(object sender, EventArgs e)
            {

            RSPort.Send("0123456\r\n");

            }

        XMLOperation mmm = new XMLOperation("ThomasPeng");
        private void dddToolStripMenuItem_Click(object sender, EventArgs e)
            {
            string[] aaa,bbb,ccc;
            aaa=new string[2];
            bbb=new string[2];
            ccc=new string[2];

            aaa[0] = "aaa?kkk";//:xxx?yyy?rrr?ccc";
            aaa[1] = "ddd?kkk?f111:nnn?xxx?yyy*rrr?ccc*mmm?jj*www?rrr?iii*ppp";
            bbb[0] = "aaa?kkk";//:xxx?yyy?rrr?ccc";
            bbb[1] = "ddd?kkk?g222:nnn?xxx?yyy*rrr?ccc*mmm?jj*www?rrr?iii*ppp";
            ccc[0] = "ddd?aaa:xxx?yyy?rrr?ccc";

            richTextBox1.AppendText(aaa[1] + " include " + FindStringInAnotherString(aaa[1], "??") + " \r\n");

            //XmlDocument xx=new XmlDocument();
            //XmlElement gg;
            //xx.Load("ss.XML");
            //gg=xx.CreateElement("iooooooo");
            //gg.InnerText = "2222";
            //xx.AppendChild(gg);//xx.CreateElement("iooooooo"));
            //xx.DocumentElement.AppendChild(xx.CreateElement("iooooooo","555","0000"));//gg);
            
            //xx.Save("ss.XML");

            //aaa[0] = "aaa";
            //bbb[0] = "bbb";
            //ccc[0] = "ccc";

            mmm.SaveXMLFile("xxx","Hello", aaa, bbb);

            mmm.AddDataToXMLFile("xxx", aaa, bbb);

            XmlDocument XMLDoc=new XmlDocument();
            XMLDoc.Load("xxx.xml");

            richTextBox1.AppendText("LocalName " + XMLDoc.LocalName + "  \r\n");
            richTextBox1.AppendText("DocumentElement " + XMLDoc.DocumentElement + "  \r\n");
            XmlNodeList ff = mmm.OpenXMLFile("xxx");
            if (ff.Count != 0) 
                {
                richTextBox1.AppendText(ff.Count.ToString() + "  \r\n");
                for (int a = 0; a < ff.Count; a++) 
                    {
                    richTextBox1.AppendText("LocalName " + ff.Item(a).LocalName + "  \r\n");
                    }
                }

            XMLDoc.Prefix = "dddddddddd";
            XMLDoc.Save("xxx.xml");

            mmm.ClearDataRecordsInXMLFile("xxx");

            }

        

        //查找一个字符串中含有多少个另外一个字符串
        /// <summary>
        /// 查找一个字符串中含有多少个另外一个字符串
        /// </summary>
        /// <param name="TargetToBeFound">需要查找的字符串</param>
        /// <param name="Source">原字符串【用来查找含有多少个TargetToBeFound】</param>
        /// <returns></returns>
        public int FindStringInAnotherString(string Source, string TargetToBeFound)
            {
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
            TempPos = Strings.InStr(Source, TargetToBeFound);

            if (TempPos < 1)
                {
                return 0;
                }
            else
                {
                TempCount = TempCount + 1;
                }
            //减去(搜索到的位置+字符串长度)，等于需要再搜索的字符串长度，可变性
            do
                {
                TempPos = Strings.InStr(TempPos + TargetToBeFound.Length, Source, TargetToBeFound);
                if (TempPos == 0)
                    {
                    return TempCount;
                    }
                else
                    {
                    TempCount += 1;
                    }
                }
            while ((Source.Length - TempPos) > TargetToBeFound.Length);

            return TempCount;

            //for (int a = 0; a < ((Source.Length / TargetToBeFound.Length)-1); a++) 
            //    {
            //    }            
            }
        ExcelOperation excl = new ExcelOperation("彭东南");
        CommonFunction FC = new CommonFunction("彭东南");
        private void fffToolStripMenuItem_Click(object sender, EventArgs e)
            {
            string[] str = new string[2];
            string[] ss = new string[2];
            ss[0] = "1";
            ss[1] = "2";
            str[0] = "a\tb\tc\td\te\tf\tg\r\n";
            str[1] = "1\t2\t3\t4\t5\t6\t7\r\n";
            FC.QuickAppendMultiRowsDataToExcel("D:\\E", CommonFunction.ExcelFileType.xls, str);
            FC.QuickAppendSingleRowDataToExcel("D:\\f", CommonFunction.ExcelFileType.xls, ss);
            }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
            {
            
            if (excl != null) 
                {
                excl.Dispose();
                }
            excl = null;

            if (mmm != null)
                {
                mmm.Dispose();
                }
            mmm = null;

            if (RSPort != null)
                {
                RSPort.ClosePort();
                }
            RSPort = null;

            bb = null;
            cc = null;

            if (xxxx != null)
                {
                xxxx.Close();
                }
            xxxx = null;

            if (ww != null)
                {
                ww.Dispose();
                }
            ww = null;
            
            if (Test != null)
                {
                Test.Close();
                }
            Test=null;

            ReadData = null;

            //ll.Close();

            }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
            {
            List<string> names = new List<string>();
            names.Add("Liz");
            names.Add("Martha");
            names.Add("Beth");
            for (int i = 0; i < names.Count; i++)
                {
                string s = names[i];
                names[i] = s.ToUpper();
                }
            }

        private void googolToolStripMenuItem_Click(object sender, EventArgs e)
            {
            try
                {
            GoogolTechGTSCard xx = new GoogolTechGTSCard(1, 4, "彭东南");
            xx.Axises[0].ErrorMessage = "";
            MessageBox.Show(xx.Axises[0].Author);
                }
            catch (Exception ex) 
                {
                MessageBox.Show(ex.Message);
                }

            }

        Thread TestT;
        private void threadToolStripMenuItem_Click(object sender, EventArgs e)
            {

            IPAddress xx;
            IPAddress.TryParse("192.168.000.024", out xx);

            MessageBox.Show(xx.ToString());

            if (TestT == null)
                {
                TestT = new Thread(TT);
                TestT.IsBackground = true;
                TestT.Start();
                }
            else 
                {
                richTextBox1.AppendText("thread is under going...\r\n");
                }
            }

        private void TT() 
            {
            try
                {
                Thread.Sleep(5500);
                //richTextBox1.AppendText("thread is executing...\r\n");
                ww.AddRichText(ref richTextBox1, "thread is executing...\r\n");

                //TestT.Abort();
                TestT = null;
                
                //richTextBox1.AppendText("Release the thread after executing is done...\r\n");
                ww.AddRichText(ref richTextBox1, "Release the thread after executing is done...\r\n");
                }
            catch (Exception ex) 
                {
                MessageBox.Show(ex.Message);
                }
            
            
            }
        //TCPIPAsyncClient ll;
        TCPIPAsyncServer pp;
        private void ccToolStripMenuItem_Click(object sender, EventArgs e)
            {
            string[] xx = new string[25];
            for (int a = 0; a < 25; a++) 
                {
                //ll.Send(a + " Thomas...\r\n");
                //ll.SendMsg = a + " Thomas...\r\n";
                xx[a] = a + " Thomas...\r\n";
                }
            //ll.SendMessage = xx;
            //ll.Send(xx);
            }

        private void ddToolStripMenuItem_Click(object sender, EventArgs e)
            {
            //ll.Send("222");
            string[] xx= {"222"};
            //ll.SendMessage = xx;
            }

    }
}
