namespace PengDongNanTools
    {
    partial class frmAbout
        {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
            {
            if (disposing && (components != null))
                {
                components.Dispose();
                }
            base.Dispose(disposing);
            }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
            {
            this.RichTextBox1 = new System.Windows.Forms.RichTextBox();
            this.OK_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RichTextBox1
            // 
            this.RichTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.RichTextBox1.Location = new System.Drawing.Point(0, 0);
            this.RichTextBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RichTextBox1.Name = "RichTextBox1";
            this.RichTextBox1.ReadOnly = true;
            this.RichTextBox1.Size = new System.Drawing.Size(379, 222);
            this.RichTextBox1.TabIndex = 3;
            this.RichTextBox1.Text = "作者：\t  彭东南\nE-Mail:\t  southeastofstar@163.com\nQQ:\t  865207152\n\n版权声明：  \n此软件/代码之版权归彭东" +
                "南所有，任何个人与公司在使用前必须得到软件作者的书面或邮件正式同意，否则被视为侵权。请尊重作者的心血，本人保留法律赋予的一切权益和权力。\n\nAll rights" +
                " are reserved to Thomas Peng.";
            this.RichTextBox1.TextChanged += new System.EventHandler(this.RichTextBox1_TextChanged);
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.OK_Button.Location = new System.Drawing.Point(268, 244);
            this.OK_Button.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(89, 28);
            this.OK_Button.TabIndex = 2;
            this.OK_Button.Text = "确定";
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(379, 273);
            this.Controls.Add(this.RichTextBox1);
            this.Controls.Add(this.OK_Button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "关于作者";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.About_FormClosing);
            this.Load += new System.EventHandler(this.About_Load);
            this.ResumeLayout(false);

            }

        #endregion

        internal System.Windows.Forms.RichTextBox RichTextBox1;
        internal System.Windows.Forms.Button OK_Button;
        }
    }