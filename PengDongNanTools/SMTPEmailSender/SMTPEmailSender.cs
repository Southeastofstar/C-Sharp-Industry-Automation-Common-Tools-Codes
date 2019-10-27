using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace SMTPEmailSender
{
    /// <summary>
    /// SMTP邮件发送类
    /// </summary>
    public class SMTPEmailSender
    {
        /// <summary>
        /// SMTP邮件发送类的构造函数
        /// </summary>
        /// <param name="TargetSmtpServerNameUri">SMTP邮件服务器地址</param>
        /// <param name="TargetSmtpUserName">SMTP邮件服务器上的用户名，用来登录</param>
        /// <param name="TargetSmtpUserPassword">SMTP邮件服务器上的用户密码，用来登录</param>
        public SMTPEmailSender(string TargetSmtpServerNameUri, string TargetSmtpUserName, string TargetSmtpUserPassword)
        {
            try
            {
                sSmtpServerNameUri = TargetSmtpServerNameUri;
                sSmtpUserName = TargetSmtpUserName;

                client.Host = TargetSmtpServerNameUri;//服务器地址
                //client.Port = port;//服务器端口 -- 如果这里指定了端口号，会导致邮件发送失败

                //身份验证
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                System.Net.NetworkCredential credential = new System.Net.NetworkCredential(TargetSmtpUserName, TargetSmtpUserPassword);
                client.UseDefaultCredentials = false;// true -- 开始发送失败，以为是这里设置为 false 导致的，其实设置为 true 也一样可以发送邮件，不受影响
                client.Credentials = credential;
            }
            catch (Exception ex)
            {
                //if (ShowMessageDialog == true)
                //{
                MessageBox.Show("SMTP邮件发送类实例化时发生错误：\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
            }
        }

        /// <summary>
        /// SMTP邮件发送类的构造函数
        /// </summary>
        /// <param name="TargetSmtpServer">SMTP邮件服务器类型，软件根据枚举会自动使用对应的SMTP服务器，可方便用户，没有必要记住每个SMTP服务器的名字</param>
        /// <param name="TargetSmtpUserName">SMTP邮件服务器上的用户名，用来登录</param>
        /// <param name="TargetSmtpUserPassword">SMTP邮件服务器上的用户密码，用来登录</param>
        /// <param name="TargetSmtpServerNameUri">SMTP邮件服务器地址，如果在SMTP服务器枚举类型中没有找到对应服务器，就需要输入对应SMTP服务器的地址，且枚举类型选择：SmtpOthers</param>
        public SMTPEmailSender(SmtpServerAddress TargetSmtpServer, string TargetSmtpUserName, string TargetSmtpUserPassword, string TargetSmtpServerNameUri = "")
        {
            try
            {
                if (TargetSmtpServer == SmtpServerAddress.SmtpOthers)
                {
                    sSmtpServerNameUri = TargetSmtpServerNameUri;
                }
                else
                {
                    sSmtpServerNameUri = SmtpServerNames[(int)TargetSmtpServer];
                }
                
                sSmtpUserName = TargetSmtpUserName;

                client.Host = sSmtpServerNameUri;//服务器地址
                //client.Port = port;//服务器端口 -- 如果这里指定了端口号，会导致邮件发送失败

                //身份验证
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                System.Net.NetworkCredential credential = new System.Net.NetworkCredential(TargetSmtpUserName, TargetSmtpUserPassword);
                client.UseDefaultCredentials = false;// true -- 开始发送失败，以为是这里设置为 false 导致的，其实设置为 true 也一样可以发送邮件，不受影响
                client.Credentials = credential;
            }
            catch (Exception ex)
            {
                //if (ShowMessageDialog == true)
                //{
                MessageBox.Show("SMTP邮件发送类实例化时发生错误：\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
            }
        }

        #region "变量定义"

        /// <summary>
        /// 软件作者：彭东南，邮箱地址：southeastofstar@163.com
        /// </summary>
        public const string Author = "软件作者：彭东南，邮箱地址：southeastofstar@163.com";

        ///// <summary>
        ///// 创建的实例个数
        ///// </summary>
        //private static int iBuiltInstanceCount = 0;

        /// <summary>
        /// 是否使用提示对话框显示信息，默认 true
        /// </summary>
        public bool ShowMessageDialog = true;

        /// <summary>
        /// SMTP邮件服务器地址
        /// </summary>
        private string sSmtpServerNameUri = "";

        /// <summary>
        /// SMTP邮件服务器地址
        /// </summary>
        public string SmtpServerNameUri
        {
            get { return sSmtpServerNameUri; }
        }

        ///// <summary>
        ///// SMTP邮件服务器端口【不要公开 -- client.Port = port;//服务器端口 -- 如果这里指定了端口号，会导致邮件发送失败】
        ///// </summary>
        //int iSmtpServerPort = 993;

        /// <summary>
        /// SMTP邮件服务器上的用户名，用来登录
        /// </summary>
        string sSmtpUserName = "";

        /// <summary>
        /// SMTP邮件服务器上的用户名，用来登录
        /// </summary>
        public string SmtpUserName
        {
            get { return sSmtpUserName; }
        }

        ///// <summary>
        ///// SMTP邮件服务器上的用户密码，用来登录
        ///// </summary>
        //string SmtpUserPassword = "";

        /// <summary>
        /// 发件人
        /// </summary>
        private string sSmtpEmailSender = "";

        /// <summary>
        /// 发件人
        /// </summary>
        public string SmtpEmailSender
        {
            get { return sSmtpEmailSender; }
            set { sSmtpEmailSender = value; }
        }

        /// <summary>
        /// 收件人
        /// </summary>
        private string sSmtpEmailReceiver = "";

        /// <summary>
        /// 收件人
        /// </summary>
        public string SmtpEmailReceiver
        {
            get { return sSmtpEmailReceiver; }
            set { sSmtpEmailReceiver = value; }
        }

        ///// <summary>
        ///// 邮件标题
        ///// </summary>
        //string SmtpMailSubject = "";

        ///// <summary>
        ///// 邮件内容
        ///// </summary>
        //string SmtpMailBody = "";

        /// <summary>
        /// 实例化 SmtpClient 对象
        /// </summary>
        SmtpClient client = new SmtpClient();

        #endregion

        #region "主流SMTP服务器地址 -- 定义枚举"

        /// <summary>
        /// SMTP主流服务器地址
        /// </summary>
        string[] SmtpServerNames = new string[] { 
                    "smtp.sina.com.cn", 

                    "smtp.vip.sina.com",

                    "smtp.sohu.com",

                    "smtp.126.com",

                    "SMTP.139.com",

                    "smtp.163.com",

                    "smtp.qq.com",

                    "smtp.exmail.qq.com",

                    "smtp.mail.yahoo.com",

                    "smtp.mail.yahoo.com.cn",

                    "smtp.live.com",

                    "smtp.gmail.com",

                    "smtp.263.net",

                    "smtp.263.net.cn",

                    "smtp.21cn.com",

                    "SMTP.foxmail.com",

                    "smtp.china.com",

                    "smtp.tom.com"

                    };

        /// <summary>
        /// SMTP主流服务器地址 -- 定义枚举
        /// </summary>
        public enum SmtpServerAddress
        {
            /// <summary>
            /// 其它未定义SMTP服务器，必须要传入SMTP服务器的地址作为参数
            /// </summary>
            SmtpOthers = -1,

            /// <summary>
            /// SMTP服务器地址:smtp.sina.com.cn（端口：25）;
            /// POP3服务器地址:pop3.sina.com.cn（端口：110）
            /// </summary>
            SmtpSina = 0,

            /// <summary>
            /// SMTP服务器:smtp.vip.sina.com （端口：25）;
            /// POP3服务器:pop3.vip.sina.com （端口：110）
            /// </summary>
            SmtpSinaVip = 1,

            /// <summary>
            /// SMTP服务器地址:smtp.sohu.com（端口：25）;
            /// POP3服务器地址:pop3.sohu.com（端口：110）
            /// </summary>
            SmtpSohu = 2,

            /// <summary>
            /// POP3服务器地址:pop.126.com（端口：110）;
            /// SMTP服务器地址:smtp.126.com（端口：25）
            /// </summary>
            Smtp126 = 3,

            /// <summary>
            /// SMTP服务器地址：SMTP.139.com(端口：25);
            /// POP3服务器地址：POP.139.com（端口：110）
            /// </summary>
            Smtp139 = 4,

            /// <summary>
            /// SMTP服务器地址:smtp.163.com（端口：25）;
            /// POP3服务器地址:pop.163.com（端口：110）
            /// </summary>
            Smtp163 = 5,

            /// <summary>
            /// SMTP服务器地址：smtp.qq.com（端口：25）;
            /// POP3服务器地址：pop.qq.com（端口：110）
            /// </summary>
            SmtpQQ = 6,

            /// <summary>
            /// SMTP服务器地址：smtp.exmail.qq.com（SSL启用 端口：587/465）;
            /// POP3服务器地址：pop.exmail.qq.com （SSL启用 端口：995）
            /// </summary>
            SmtpQQEnterprise = 7,

            /// <summary>
            /// SMTP服务器地址:smtp.mail.yahoo.com;
            /// POP3服务器地址:pop.mail.yahoo.com
            /// </summary>
            SmtpYahoo = 8,

            /// <summary>
            /// SMTP服务器地址:smtp.mail.yahoo.com.cn（端口：587);
            /// POP3服务器地址:pop.mail.yahoo.com.cn（端口：995）
            /// </summary>
            SmtpYahooChina = 9,

            /// <summary>
            /// SMTP服务器地址：smtp.live.com（端口：587）;
            /// POP3服务器地址：pop3.live.com（端口：995）
            /// </summary>
            SmtpHotMail = 10,

            /// <summary>
            /// SMTP服务器地址:smtp.gmail.com（SSL启用 端口：587）;
            /// POP3服务器地址:pop.gmail.com（SSL启用端口：995）
            /// </summary>
            SmtpGmail = 11,

            /// <summary>
            /// SMTP服务器地址:smtp.263.net（端口：25）;
            /// POP3服务器地址:pop3.263.net（端口：110）
            /// </summary>
            Smtp263 = 12,

            /// <summary>
            /// SMTP服务器地址:smtp.263.net.cn（端口：25）;
            /// POP3服务器地址:pop.263.net.cn（端口：110）
            /// </summary>
            Smtp263China = 13,

            /// <summary>
            /// SMTP服务器地址:smtp.21cn.com（端口：25）;
            /// POP3服务器地址:pop.21cn.com（端口：110）
            /// </summary>
            Smtp21cn = 14,

            /// <summary>
            /// SMTP服务器地址:SMTP.foxmail.com（端口：25）;
            /// POP3服务器地址:POP.foxmail.com（端口：110）
            /// </summary>
            SmtpFoxmail = 15,

            /// <summary>
            /// SMTP服务器地址:smtp.china.com（端口：25）;
            /// POP3服务器地址:pop.china.com（端口：110）
            /// </summary>
            SmtpChina = 16,

            /// <summary>
            /// SMTP服务器地址:smtp.tom.com（端口：25）;
            /// POP3服务器地址:pop.tom.com（端口：110）
            /// </summary>
            SmtpTom = 17
        }

        #endregion

        #region "SMTP/POP3主流服务器地址"

        //【sina.com】
        //POP3服务器地址:pop3.sina.com.cn（端口：110）
        //SMTP服务器地址:smtp.sina.com.cn（端口：25） 

        //【sinaVIP】
        //POP3服务器:pop3.vip.sina.com （端口：110）
        //SMTP服务器:smtp.vip.sina.com （端口：25）

        //【sohu.com】
        //POP3服务器地址:pop3.sohu.com（端口：110）
        //SMTP服务器地址:smtp.sohu.com（端口：25）

        //【126邮箱】
        //POP3服务器地址:pop.126.com（端口：110）
        //SMTP服务器地址:smtp.126.com（端口：25）

        //【139邮箱】
        //POP3服务器地址：POP.139.com（端口：110）
        //SMTP服务器地址：SMTP.139.com(端口：25)
        
        //【163.com】
        //POP3服务器地址:pop.163.com（端口：110）
        //SMTP服务器地址:smtp.163.com（端口：25）

        //【QQ邮箱】
        //POP3服务器地址：pop.qq.com（端口：110）
        //SMTP服务器地址：smtp.qq.com（端口：25）

        //【QQ企业邮箱】
        //POP3服务器地址：pop.exmail.qq.com （SSL启用 端口：995）
        //SMTP服务器地址：smtp.exmail.qq.com（SSL启用 端口：587/465）

        //【yahoo.com】
        //POP3服务器地址:pop.mail.yahoo.com
        //SMTP服务器地址:smtp.mail.yahoo.com

        //【yahoo.com.cn】
        //POP3服务器地址:pop.mail.yahoo.com.cn（端口：995）
        //SMTP服务器地址:smtp.mail.yahoo.com.cn（端口：587
        
        //【HotMail】
        //POP3服务器地址：pop3.live.com（端口：995）
        //SMTP服务器地址：smtp.live.com（端口：587）

        //【Gmail】
        //POP3服务器地址:pop.gmail.com（SSL启用端口：995）
        //SMTP服务器地址:smtp.gmail.com（SSL启用 端口：587）
        
        //【263.net】
        //POP3服务器地址:pop3.263.net（端口：110）
        //SMTP服务器地址:smtp.263.net（端口：25）

        //【263.net.cn】
        //POP3服务器地址:pop.263.net.cn（端口：110）
        //SMTP服务器地址:smtp.263.net.cn（端口：25）

        //【21cn.com】
        //POP3服务器地址:pop.21cn.com（端口：110）
        //SMTP服务器地址:smtp.21cn.com（端口：25）

        //【Foxmail】
        //POP3服务器地址:POP.foxmail.com（端口：110）
        //SMTP服务器地址:SMTP.foxmail.com（端口：25）

        //【china.com】
        //POP3服务器地址:pop.china.com（端口：110）
        //SMTP服务器地址:smtp.china.com（端口：25）

        //【tom.com】
        //POP3服务器地址:pop.tom.com（端口：110）
        //SMTP服务器地址:smtp.tom.com（端口：25）

        #endregion

        #region "函数代码"

        /// <summary>
        /// 同步发送SMTP邮件
        /// </summary>
        /// <param name="EmailAddressOfSender">发件人邮箱地址</param>
        /// <param name="EmailAddressOfReceiver">收件人邮箱地址</param>
        /// <param name="MailSubject">邮件主题</param>
        /// <param name="MailBody">邮件正文</param>
        /// <returns>是否执行成功</returns>
        public bool Send(string EmailAddressOfSender, string EmailAddressOfReceiver, string MailSubject, string MailBody)
        {
            try
            {
                client.Send(EmailAddressOfSender, EmailAddressOfReceiver, MailSubject, MailBody);
            }
            catch (Exception ex)
            {
                if (ShowMessageDialog == true)
                {
                    MessageBox.Show("发送SMTP邮件时发生错误：\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// 异步发送SMTP邮件
        /// </summary>
        /// <param name="EmailAddressOfSender">发件人邮箱地址</param>
        /// <param name="EmailAddressOfReceiver">收件人邮箱地址</param>
        /// <param name="MailSubject">邮件主题</param>
        /// <param name="MailBody">邮件正文</param>
        public async void SendAsync(string EmailAddressOfSender, string EmailAddressOfReceiver, string MailSubject, string MailBody)
        {
            try
            {
                await client.SendMailAsync(EmailAddressOfSender, EmailAddressOfReceiver, MailSubject, MailBody);
            }
            catch (Exception ex)
            {
                if (ShowMessageDialog == true)
                {
                    MessageBox.Show("发送SMTP邮件时发生错误：\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 同步发送SMTP邮件【含单个附件】
        /// </summary>
        /// <param name="EmailAddressOfSender">发件人邮箱地址</param>
        /// <param name="EmailAddressOfReceiver">收件人邮箱地址</param>
        /// <param name="MailSubject">邮件主题</param>
        /// <param name="MailBody">邮件正文</param>
        /// <param name="AttachmentFileName">邮件附档的文件名称(绝对路径或相对路径)</param>
        /// <returns>是否执行成功</returns>
        public bool Send(string EmailAddressOfSender, string EmailAddressOfReceiver, string MailSubject, string MailBody, string AttachmentFileName)
        {
            try
            {
                Attachment NewAttachment = null;

                MailMessage NewMailMsg = null;

                if (string.IsNullOrEmpty(AttachmentFileName) == false)
                {
                    NewAttachment = new Attachment(AttachmentFileName);

                    NewMailMsg = new MailMessage(EmailAddressOfSender, EmailAddressOfReceiver, MailSubject, MailBody);
                    NewMailMsg.Attachments.Add(NewAttachment);

                    //发送邮件
                    client.Send(NewMailMsg);
                }
                else
                {
                    client.Send(EmailAddressOfSender, EmailAddressOfReceiver, MailSubject, MailBody);
                }
            }
            catch (Exception ex)
            {
                if (ShowMessageDialog == true)
                {
                    MessageBox.Show("发送SMTP邮件时发生错误：\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// 异步发送SMTP邮件【含单个附件】
        /// </summary>
        /// <param name="EmailAddressOfSender">发件人邮箱地址</param>
        /// <param name="EmailAddressOfReceiver">收件人邮箱地址</param>
        /// <param name="MailSubject">邮件主题</param>
        /// <param name="MailBody">邮件正文</param>
        /// <param name="AttachmentFileName">邮件附档的文件名称(绝对路径或相对路径)</param>
        /// <returns>是否执行成功</returns>
        public async void SendAsync(string EmailAddressOfSender, string EmailAddressOfReceiver, string MailSubject, string MailBody, string AttachmentFileName)
        {
            try
            {
                Attachment NewAttachment = null;

                MailMessage NewMailMsg = null;

                if (string.IsNullOrEmpty(AttachmentFileName) == false)
                {
                    NewAttachment = new Attachment(AttachmentFileName);

                    NewMailMsg = new MailMessage(EmailAddressOfSender, EmailAddressOfReceiver, MailSubject, MailBody);
                    NewMailMsg.Attachments.Add(NewAttachment);

                    //发送邮件
                    await client.SendMailAsync(NewMailMsg);
                }
                else
                {
                    await client.SendMailAsync(EmailAddressOfSender, EmailAddressOfReceiver, MailSubject, MailBody);
                }
            }
            catch (Exception ex)
            {
                if (ShowMessageDialog == true)
                {
                    MessageBox.Show("发送SMTP邮件时发生错误：\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 同步发送SMTP邮件【含多个附件】
        /// </summary>
        /// <param name="EmailAddressOfSender">发件人邮箱地址</param>
        /// <param name="EmailAddressOfReceiver">收件人邮箱地址</param>
        /// <param name="MailSubject">邮件主题</param>
        /// <param name="MailBody">邮件正文</param>
        /// <param name="AttachmentFileName">邮件附档的文件名称(绝对路径或相对路径)</param>
        /// <returns>是否执行成功</returns>
        public bool Send(string EmailAddressOfSender, string EmailAddressOfReceiver, string MailSubject, string MailBody, string[] AttachmentFileName)
        {
            try
            {
                Attachment NewAttachment = null;
                MailMessage NewMailMsg = null;

                if (null != AttachmentFileName && AttachmentFileName.Length > 0)
                {
                    NewMailMsg = new MailMessage(EmailAddressOfSender, EmailAddressOfReceiver, MailSubject, MailBody);

                    for (int i = 0; i < AttachmentFileName.Length; i++)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(AttachmentFileName[i]) == false)
                            {
                                NewAttachment = new Attachment(AttachmentFileName[i]);
                                NewMailMsg.Attachments.Add(NewAttachment);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    //发送邮件
                    client.Send(NewMailMsg);
                }
                else
                {
                    client.Send(EmailAddressOfSender, EmailAddressOfReceiver, MailSubject, MailBody);
                }
            }
            catch (Exception ex)
            {
                if (ShowMessageDialog == true)
                {
                    MessageBox.Show("发送SMTP邮件时发生错误：\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// 异步发送SMTP邮件【含多个附件】
        /// </summary>
        /// <param name="EmailAddressOfSender">发件人邮箱地址</param>
        /// <param name="EmailAddressOfReceiver">收件人邮箱地址</param>
        /// <param name="MailSubject">邮件主题</param>
        /// <param name="MailBody">邮件正文</param>
        /// <param name="AttachmentFileName">邮件附档的文件名称(绝对路径或相对路径)</param>
        /// <returns>是否执行成功</returns>
        public async void SendAsync(string EmailAddressOfSender, string EmailAddressOfReceiver, string MailSubject, string MailBody, string[] AttachmentFileName)
        {
            try
            {
                Attachment NewAttachment = null;
                MailMessage NewMailMsg = null;

                if (null != AttachmentFileName && AttachmentFileName.Length > 0)
                {
                    NewMailMsg = new MailMessage(EmailAddressOfSender, EmailAddressOfReceiver, MailSubject, MailBody);

                    for (int i = 0; i < AttachmentFileName.Length; i++)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(AttachmentFileName[i]) == false)
                            {
                                NewAttachment = new Attachment(AttachmentFileName[i]);
                                NewMailMsg.Attachments.Add(NewAttachment);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    //发送邮件
                    await client.SendMailAsync(NewMailMsg);
                }
                else
                {
                    await client.SendMailAsync(EmailAddressOfSender, EmailAddressOfReceiver, MailSubject, MailBody);
                }
            }
            catch (Exception ex)
            {
                if (ShowMessageDialog == true)
                {
                    MessageBox.Show("发送SMTP邮件时发生错误：\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 打开文件对话框
        /// </summary>
        OpenFileDialog dlgOpenFile = null;// = new OpenFileDialog();

        /// <summary>
        /// 需要添加到附件的文件名称数组
        /// </summary>
        public string[] OpenedAttachments = null;

        /// <summary>
        /// 打开需要添加到附件的文件
        /// </summary>
        /// <returns></returns>
        public string[] OpenAttachments()
        {
            try
            {
                if (null == dlgOpenFile)
                {
                    dlgOpenFile = new OpenFileDialog();
                    dlgOpenFile.Title = "添加件附";
                    dlgOpenFile.Multiselect = true;
                    dlgOpenFile.RestoreDirectory = false;
                    dlgOpenFile.CheckFileExists = true;
                    dlgOpenFile.Filter = "所有文件(*.*)|*.*";
                }

                if (dlgOpenFile.ShowDialog() == DialogResult.OK && dlgOpenFile.FileNames != null)
                {
                    OpenedAttachments = dlgOpenFile.FileNames;
                    return OpenedAttachments;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (ShowMessageDialog == true)
                {
                    MessageBox.Show("打开需要添加到附件的文件时发生错误：\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return null;
            }
        }

        //添加加密和解密的代码，使邮件的密码能够用加密后的内容进行存储或读取，避免明文密码不安全的问题

        //默认密钥向量
        private static byte[] Keys = { 0x2F, 0xE7, 0xA2, 0x43, 0x81, 0x49, 0xDE, 0x53 };

        //DES加密字符串
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="EncryptString">待加密的字符串</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串 </returns>
        private string Encrypt(string EncryptString)//EncryptDES  , string EncryptKey
        {
            try
            {
                //byte[] rgbKey = Encoding.UTF8.GetBytes(rgbKeysForCreateEncryptor.Substring(0, 8));//转换为字节，长度限定为8个字节，否则报错：指定键的大小对于此算法无效。
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(EncryptString);

                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();//实例化数据加密标准
                MemoryStream mStream = new MemoryStream();//实例化内存流

                //将数据流链接到加密转换的流
                //CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(byGgbKeysForCreateEncryptor, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return EncryptString;
            }
        }

        byte[] byGgbKeysForCreateEncryptor = null;

        /// <summary>
        /// DES加密字符串：加密成功返回加密后的字符串，失败返回源串
        /// </summary>
        /// <param name="EncryptString">待加密的字符串</param>
        /// <param name="EncryptPassword">加密密钥,要求为8个字节</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串 </returns>
        public string Encrypt(string EncryptString, string EncryptPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(EncryptPassword) == true || Encoding.UTF8.GetBytes(EncryptPassword).Length != 8)// EncryptPassword.Length > 8)
                {
                    MessageBox.Show("加密密码不能为空，长度为8个字母、数字或符号，总共8个字节.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                    return EncryptString;
                }

                byGgbKeysForCreateEncryptor = Encoding.UTF8.GetBytes(EncryptPassword);
                return Encrypt(EncryptString);
            }
            catch
            {
                return EncryptString;
            }
        }

        //DES解密字符串
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="DecryptString">待解密的字符串</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        private string Decrypt(string DecryptString)//DecryptDES
        {
            try
            {
                //byte[] rgbKey = Encoding.UTF8.GetBytes(srgbKeyForCreateDecryptor.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(DecryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                //CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(byRgbKeyForCreateDecryptor, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return DecryptString;
            }
        }

        private byte[] byRgbKeyForCreateDecryptor = null;

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="DecryptString">待解密的字符串</param>
        /// <param name="DecryptPassword">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public string Decrypt(string DecryptString, string DecryptPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(DecryptPassword) == true || Encoding.UTF8.GetBytes(DecryptPassword).Length != 8)// DecryptPassword.Length > 8)
                {
                    MessageBox.Show("解密密码不能为空，长度为8个字母、数字或符号，总共8个字节.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return DecryptString;
                }

                byRgbKeyForCreateDecryptor = Encoding.UTF8.GetBytes(DecryptPassword);

                return Decrypt(DecryptString);
            }
            catch
            {
                return DecryptString;
            }
        }

        #endregion

    }//class

}//namespace