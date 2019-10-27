namespace PengDongNanTools
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
        this.components = new System.ComponentModel.Container();
        this.btnNew = new System.Windows.Forms.Button();
        this.richTextBox1 = new System.Windows.Forms.RichTextBox();
        this.timer1 = new System.Windows.Forms.Timer(this.components);
        this.btnSend = new System.Windows.Forms.Button();
        this.richTextBox2 = new System.Windows.Forms.RichTextBox();
        this.button1 = new System.Windows.Forms.Button();
        this.button2 = new System.Windows.Forms.Button();
        this.button3 = new System.Windows.Forms.Button();
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.dddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.fffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.listToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.googolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.threadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.ccToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.label1 = new System.Windows.Forms.Label();
        this.button4 = new System.Windows.Forms.Button();
        this.button5 = new System.Windows.Forms.Button();
        this.button6 = new System.Windows.Forms.Button();
        this.button7 = new System.Windows.Forms.Button();
        this.btnRS232C = new System.Windows.Forms.Button();
        this.btnRS232CSend = new System.Windows.Forms.Button();
        this.ddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.menuStrip1.SuspendLayout();
        this.SuspendLayout();
        // 
        // btnNew
        // 
        this.btnNew.Location = new System.Drawing.Point(54, 342);
        this.btnNew.Name = "btnNew";
        this.btnNew.Size = new System.Drawing.Size(75, 23);
        this.btnNew.TabIndex = 0;
        this.btnNew.Text = "New";
        this.btnNew.UseVisualStyleBackColor = true;
        this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
        // 
        // richTextBox1
        // 
        this.richTextBox1.Location = new System.Drawing.Point(385, 12);
        this.richTextBox1.Name = "richTextBox1";
        this.richTextBox1.Size = new System.Drawing.Size(780, 459);
        this.richTextBox1.TabIndex = 1;
        this.richTextBox1.Text = "\n456\n465\n456\n456\n546";
        // 
        // timer1
        // 
        this.timer1.Enabled = true;
        this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        // 
        // btnSend
        // 
        this.btnSend.Location = new System.Drawing.Point(211, 342);
        this.btnSend.Name = "btnSend";
        this.btnSend.Size = new System.Drawing.Size(75, 23);
        this.btnSend.TabIndex = 2;
        this.btnSend.Text = "Send";
        this.btnSend.UseVisualStyleBackColor = true;
        this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
        // 
        // richTextBox2
        // 
        this.richTextBox2.Location = new System.Drawing.Point(12, 49);
        this.richTextBox2.Name = "richTextBox2";
        this.richTextBox2.Size = new System.Drawing.Size(330, 121);
        this.richTextBox2.TabIndex = 3;
        this.richTextBox2.Text = "";
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(267, 269);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(75, 23);
        this.button1.TabIndex = 4;
        this.button1.Text = "button1";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.button1_Click);
        // 
        // button2
        // 
        this.button2.Location = new System.Drawing.Point(178, 448);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(75, 23);
        this.button2.TabIndex = 5;
        this.button2.Text = "button2";
        this.button2.UseVisualStyleBackColor = true;
        this.button2.Click += new System.EventHandler(this.button2_Click);
        // 
        // button3
        // 
        this.button3.Location = new System.Drawing.Point(298, 495);
        this.button3.Name = "button3";
        this.button3.Size = new System.Drawing.Size(75, 23);
        this.button3.TabIndex = 6;
        this.button3.Text = "Server";
        this.button3.UseVisualStyleBackColor = true;
        this.button3.Click += new System.EventHandler(this.button3_Click);
        // 
        // menuStrip1
        // 
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.ccToolStripMenuItem,
            this.ddToolStripMenuItem});
        this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Size = new System.Drawing.Size(1200, 28);
        this.menuStrip1.TabIndex = 7;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // fileToolStripMenuItem
        // 
        this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dddToolStripMenuItem,
            this.fffToolStripMenuItem,
            this.listToolStripMenuItem,
            this.googolToolStripMenuItem,
            this.threadToolStripMenuItem});
        this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
        this.fileToolStripMenuItem.Text = "File";
        // 
        // dddToolStripMenuItem
        // 
        this.dddToolStripMenuItem.Name = "dddToolStripMenuItem";
        this.dddToolStripMenuItem.Size = new System.Drawing.Size(132, 24);
        this.dddToolStripMenuItem.Text = "ddd";
        this.dddToolStripMenuItem.Click += new System.EventHandler(this.dddToolStripMenuItem_Click);
        // 
        // fffToolStripMenuItem
        // 
        this.fffToolStripMenuItem.Name = "fffToolStripMenuItem";
        this.fffToolStripMenuItem.Size = new System.Drawing.Size(132, 24);
        this.fffToolStripMenuItem.Text = "fff";
        this.fffToolStripMenuItem.Click += new System.EventHandler(this.fffToolStripMenuItem_Click);
        // 
        // listToolStripMenuItem
        // 
        this.listToolStripMenuItem.Name = "listToolStripMenuItem";
        this.listToolStripMenuItem.Size = new System.Drawing.Size(132, 24);
        this.listToolStripMenuItem.Text = "list";
        this.listToolStripMenuItem.Click += new System.EventHandler(this.listToolStripMenuItem_Click);
        // 
        // googolToolStripMenuItem
        // 
        this.googolToolStripMenuItem.Name = "googolToolStripMenuItem";
        this.googolToolStripMenuItem.Size = new System.Drawing.Size(132, 24);
        this.googolToolStripMenuItem.Text = "googol";
        this.googolToolStripMenuItem.Click += new System.EventHandler(this.googolToolStripMenuItem_Click);
        // 
        // threadToolStripMenuItem
        // 
        this.threadToolStripMenuItem.Name = "threadToolStripMenuItem";
        this.threadToolStripMenuItem.Size = new System.Drawing.Size(132, 24);
        this.threadToolStripMenuItem.Text = "thread";
        this.threadToolStripMenuItem.Click += new System.EventHandler(this.threadToolStripMenuItem_Click);
        // 
        // ccToolStripMenuItem
        // 
        this.ccToolStripMenuItem.Name = "ccToolStripMenuItem";
        this.ccToolStripMenuItem.Size = new System.Drawing.Size(37, 24);
        this.ccToolStripMenuItem.Text = "cc";
        this.ccToolStripMenuItem.Click += new System.EventHandler(this.ccToolStripMenuItem_Click);
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(83, 225);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(55, 15);
        this.label1.TabIndex = 8;
        this.label1.Text = "label1";
        // 
        // button4
        // 
        this.button4.Location = new System.Drawing.Point(410, 495);
        this.button4.Name = "button4";
        this.button4.Size = new System.Drawing.Size(75, 23);
        this.button4.TabIndex = 9;
        this.button4.Text = "Send";
        this.button4.UseVisualStyleBackColor = true;
        this.button4.Click += new System.EventHandler(this.button4_Click);
        // 
        // button5
        // 
        this.button5.Location = new System.Drawing.Point(535, 504);
        this.button5.Name = "button5";
        this.button5.Size = new System.Drawing.Size(114, 42);
        this.button5.TabIndex = 10;
        this.button5.Text = "CloseServer";
        this.button5.UseVisualStyleBackColor = true;
        this.button5.Click += new System.EventHandler(this.button5_Click);
        // 
        // button6
        // 
        this.button6.Location = new System.Drawing.Point(54, 427);
        this.button6.Name = "button6";
        this.button6.Size = new System.Drawing.Size(84, 44);
        this.button6.TabIndex = 11;
        this.button6.Text = "CloseClient";
        this.button6.UseVisualStyleBackColor = true;
        this.button6.Click += new System.EventHandler(this.button6_Click);
        // 
        // button7
        // 
        this.button7.Location = new System.Drawing.Point(701, 514);
        this.button7.Name = "button7";
        this.button7.Size = new System.Drawing.Size(75, 23);
        this.button7.TabIndex = 12;
        this.button7.Text = "button7";
        this.button7.UseVisualStyleBackColor = true;
        this.button7.Click += new System.EventHandler(this.button7_Click);
        // 
        // btnRS232C
        // 
        this.btnRS232C.Location = new System.Drawing.Point(805, 514);
        this.btnRS232C.Name = "btnRS232C";
        this.btnRS232C.Size = new System.Drawing.Size(75, 23);
        this.btnRS232C.TabIndex = 13;
        this.btnRS232C.Text = "RS232C";
        this.btnRS232C.UseVisualStyleBackColor = true;
        this.btnRS232C.Click += new System.EventHandler(this.btnRS232C_Click);
        // 
        // btnRS232CSend
        // 
        this.btnRS232CSend.Location = new System.Drawing.Point(906, 514);
        this.btnRS232CSend.Name = "btnRS232CSend";
        this.btnRS232CSend.Size = new System.Drawing.Size(75, 23);
        this.btnRS232CSend.TabIndex = 14;
        this.btnRS232CSend.Text = "Send";
        this.btnRS232CSend.UseVisualStyleBackColor = true;
        this.btnRS232CSend.Click += new System.EventHandler(this.btnRS232CSend_Click);
        // 
        // ddToolStripMenuItem
        // 
        this.ddToolStripMenuItem.Name = "ddToolStripMenuItem";
        this.ddToolStripMenuItem.Size = new System.Drawing.Size(41, 24);
        this.ddToolStripMenuItem.Text = "dd";
        this.ddToolStripMenuItem.Click += new System.EventHandler(this.ddToolStripMenuItem_Click);
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1200, 558);
        this.Controls.Add(this.btnRS232CSend);
        this.Controls.Add(this.btnRS232C);
        this.Controls.Add(this.button7);
        this.Controls.Add(this.button6);
        this.Controls.Add(this.button5);
        this.Controls.Add(this.button4);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.button3);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.richTextBox2);
        this.Controls.Add(this.btnSend);
        this.Controls.Add(this.richTextBox1);
        this.Controls.Add(this.btnNew);
        this.Controls.Add(this.menuStrip1);
        this.MainMenuStrip = this.menuStrip1;
        this.Name = "Form1";
        this.Text = "Form1";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
        this.Load += new System.EventHandler(this.Form1_Load);
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem dddToolStripMenuItem;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button btnRS232C;
        private System.Windows.Forms.Button btnRS232CSend;
        private System.Windows.Forms.ToolStripMenuItem fffToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem googolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem threadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ccToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ddToolStripMenuItem;
    }
}

