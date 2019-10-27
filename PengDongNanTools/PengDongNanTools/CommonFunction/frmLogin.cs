#region "using"

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.AccessControl;

#endregion

//可以设计一个图形界面，然后用来进行点击来代替键盘的密码输入【相当于加密】，然后在文本框中由程序添加*号，这样密码就不会很容易被木马病毒窃取，
//而且图形界面的位置每次登录的位置都随机变化，只

namespace PengDongNanTools
    {

    //登录界面
    /// <summary>
    /// 登录界面
    /// </summary>
    public partial class frmLogin : Form
        {

        #region "变量声明"

        RegistryKey ModifyRegistry;
        string User = Environment.UserDomainName + "\\" + Environment.UserName;
        RegistrySecurity ModifyRegisterSecurity = new RegistrySecurity();

        Microsoft.VisualBasic.Devices.Computer PC = new Microsoft.VisualBasic.Devices.Computer();

        private int LoggedCode = 10;
        private bool LoggedSuccess = false;
        private string strSoftwareNameToBeAuthorizeVerify="";

        /// <summary>
        /// 是否登录成功
        /// </summary>
        public bool LoginSuccess 
            {
            get { return LoggedSuccess; }            
            }

        /// <summary>
        /// 当前登录编号
        /// </summary>
        public int LoginCode 
            {
            get { return LoggedCode; }
            }

        private int TotalVerifyPasswordCount = 0;

        /// <summary>
        /// 登录用户名
        /// </summary>
        public enum LoggedUserName
            {
            /// <summary>
            /// 普通操作员
            /// </summary>
            User,

            /// <summary>
            /// 工程师
            /// </summary>
            Engineer,

            /// <summary>
            /// 管理员
            /// </summary>
            Administrator,

            /// <summary>
            /// 授权者【软件作者】
            /// </summary>
            Authorizer

            };

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }

        #endregion

        #region "窗体事件"

        private void Login_Load(object sender, EventArgs e)
            {

            try
                {

                lblTryPWDLeftCount.Visible = false;
                lblTryPWDLeftCount.Text = "";
                txtPassword.Text = "";

                if (LoggedCode == 10)
                    {
                    btnLogout.Visible = false;
                    cmbUserName.SelectedIndex = 0;
                    txtPassword.Text = "";
                    }
                else 
                    {
                    btnLogout.Visible = true;
                    switch (LoggedCode) 
                        {
                        case 0:
                            cmbUserName.SelectedIndex = 0;
                            break;

                        case 1:
                            cmbUserName.SelectedIndex = 1;
                            break;

                        case 2:
                            cmbUserName.SelectedIndex = 2;
                            break;
                        }
                    }

                TotalVerifyPasswordCount = 0;
                txtPassword.Focus();

                AuthorizeAtLoadingStage();

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
            {

            try
                {

                e.Cancel = true;
                this.Hide();

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        private void btnLogout_Click(object sender, EventArgs e)
            {

            try
                {

                if(MessageBox.Show("Are you sure to logout? Therefore, you will not have the right to operate" +
                    "the software before you re-login.\r\n" + "确定要退出登录码？退出后，在再次登录前你将失去" +
                    "操作软件的相关权限！","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                    {
                    LoggedCode = 10;
                    LoggedSuccess = false;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        private void btnCancel_Click(object sender, EventArgs e)
            {

            try
                {

                this.Hide();

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }
        
        private void btnLogin_Click(object sender, EventArgs e)
            {

            try
                {

                if (txtPassword.Text == "") 
                    {
                    MessageBox.Show("The password can't be empty, please input it first before login.\r\n" +
                        "用户密码不能为空！请输入密码后按 \"确定\" 进行登录。","提示",MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                    }

                switch (cmbUserName.SelectedIndex) 
                    {
                    case 0:
                        if (txtPassword.Text == "666666")
                            {
                            LoggedCode = 0;
                            LoggedSuccess = true;
                            }
                        else 
                            {
                            LoggedCode = 10;
                            }
                        break;

                    case 1:
                        if (txtPassword.Text == "888888")
                            {
                            LoggedCode = 1;
                            LoggedSuccess = true;
                            }
                        else
                            {
                            LoggedCode = 10;
                            }
                        break;

                    case 2:
                        if (txtPassword.Text == "999999")
                            {
                            LoggedCode = 2;
                            LoggedSuccess = true;
                            }
                        else
                            {
                            LoggedCode = 10;
                            }
                        break;

                    case 3:
                        if (txtPassword.Text == "pengdongnan" 
                            | txtPassword.Text == "彭东南" 
                            | txtPassword.Text == "deletepengdongnan")
                            {
                            LoggedCode = 3;
                            LoggedSuccess = true;

                            //授权使用
                            SetAuthorization();

                            }
                        else
                            {
                            LoggedCode = 10;
                            }
                        break;
                    
                    }

                if (LoggedCode == 10)
                    {
                    LoggedSuccess = false;
                    TotalVerifyPasswordCount += 1;
                    if (TotalVerifyPasswordCount > 5)
                        {
                        MessageBox.Show("You have tried 5 times but the password is still wrong."
                            + "You don't have the right to use this software.\r\n" +
                            "你已经输错密码超过5次，你没有被授权使用此密码，软件会马上强制终止！", "警告",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        System.Windows.Forms.Application.Exit();
                        }

                    lblTryPWDLeftCount.Visible = true;
                    lblTryPWDLeftCount.Text = "尝试密码剩余次数: " + (5 - TotalVerifyPasswordCount);

                    MessageBox.Show("The password is wrong, you have " + (5 - TotalVerifyPasswordCount) +
                        " chances left to retry it.\r\n" + "密码错误，请重新输入密码，最多可以尝试5次，否则会强制终止软件！" +
                       "尝试密码剩余次数: " + (5 - TotalVerifyPasswordCount), "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPassword.Focus();
                    return;
                    }
                else 
                    {
                    TotalVerifyPasswordCount = 0;
                    this.Hide();
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        private void txtPassword_TextChanged(object sender, EventArgs e)
            {

            }

        private void cmbUserName_SelectedIndexChanged(object sender, EventArgs e)
            {

            if (cmbUserName.SelectedIndex == -1)
                {
                return;
                }

            switch (cmbUserName.SelectedIndex)
                {
                case 0:

                    break;

                case 1:

                    break;

                case 2:

                    break;

                case 3:

                    break;

                }

            txtPassword.Focus();

            }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
            {

            try
                {

                if (e.KeyCode == Keys.Enter)
                    {
                    btnLogin.Focus();
                    e.Handled = true;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }

            }

        #endregion

        #region "函数代码"

        //登录界面实例化函数
        /// <summary>
        /// 登录界面实例化函数
        /// </summary>
        /// <param name="SoftwareNameToBeAuthorizeVerify">用来授权和验证的软件名称</param>
        public frmLogin(string SoftwareNameToBeAuthorizeVerify)
            {

            if (SoftwareNameToBeAuthorizeVerify == "") 
                {
                MessageBox.Show("The parameter 'SoftwareNameToBeAuthorizeVerify' can't be empty." +
                    "参数'SoftwareNameToBeAuthorizeVerify'不能为空，这是用来授权和验证的软件名称。",
                    "提醒",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
                }

            InitializeComponent();
            strSoftwareNameToBeAuthorizeVerify=SoftwareNameToBeAuthorizeVerify;

            }

        /// <summary>
        /// 验证用户登录的权限
        /// </summary>
        protected void LoginVerify() 
            {
            
            }

        /// <summary>
        /// 在窗体装载时检查授权状态
        /// </summary>
        private void AuthorizeAtLoadingStage() 
            {

            try
                {
                
                ModifyRegistry = PC.Registry.CurrentUser.OpenSubKey("SOFTWARE\\" 
                    + strSoftwareNameToBeAuthorizeVerify + "\\Authorization\\CurrentUser");

                //键值不存在
                if (ModifyRegistry == null) 
                    {
                    MessageBox.Show("此软件未经授权，最多可以试用20次！","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);

                    ModifyRegisterSecurity.AddAccessRule(new RegistryAccessRule(User,RegistryRights.WriteKey,
                        InheritanceFlags.None,PropagationFlags.None,AccessControlType.Allow));

                    ModifyRegistry = PC.Registry.CurrentUser.CreateSubKey("SOFTWARE",
                        Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);

                    ModifyRegistry.OpenSubKey("SOFTWARE", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);
                    ModifyRegistry.SetAccessControl(ModifyRegisterSecurity);

                    ModifyRegistry.CreateSubKey(strSoftwareNameToBeAuthorizeVerify, 
                        Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree,
                                            Microsoft.Win32.RegistryOptions.None);

                    ModifyRegistry = ModifyRegistry.OpenSubKey(strSoftwareNameToBeAuthorizeVerify, 
                        Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);
                    ModifyRegistry.CreateSubKey("Authorization");

                    ModifyRegistry = ModifyRegistry.OpenSubKey("Authorization", 
                        Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);
                    ModifyRegistry.CreateSubKey("CurrentUser");

                    ModifyRegistry = ModifyRegistry.OpenSubKey("CurrentUser", 
                        Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);

                    ModifyRegistry.SetValue("LoggedCount", "1");
                    ModifyRegistry.SetValue("Authorized", "No");
                    ModifyRegistry.SetValue("MaxTryLogCount", "100");
                    ModifyRegistry.SetValue("DisabledUSB", "False");
                    return;

                    }

                if (ModifyRegistry.GetValue("Authorized").ToString().ToUpper() == "NO") 
                    {
                    
                    if(ModifyRegistry.GetValue("LoggedCount") !=null)
                        {
                        
                        if(Convert.ToInt16(ModifyRegistry.GetValue("LoggedCount")) > 20)
                        {
                            MessageBox.Show("已经超过未经授权试用次数，软件会马上终止！", "警告", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            System.Windows.Forms.Application.Exit();
                        }
                        else
                            {
                            ModifyRegistry = PC.Registry.CurrentUser.OpenSubKey("SOFTWARE\\" + 
                                strSoftwareNameToBeAuthorizeVerify + "\\Authorization\\CurrentUser", 
                                Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);
                            ModifyRegistry.SetValue("LoggedCount", 
                                Convert.ToInt16(ModifyRegistry.GetValue("LoggedCount")) + 1);
                            }

                        }

                    }
                else if (ModifyRegistry.GetValue("Authorized").ToString().ToUpper() == "YES")
                    {
                    ModifyRegistry = PC.Registry.CurrentUser.OpenSubKey("SOFTWARE\\" + 
                        strSoftwareNameToBeAuthorizeVerify + "\\Authorization\\CurrentUser", 
                        Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);
                    ModifyRegistry.SetValue("LoggedCount", Convert.ToInt16(ModifyRegistry.GetValue("LoggedCount")) + 1);                    
                    }
                else
                    {
                    MessageBox.Show("由于未经授权的非法操作导致严重错误，软件会马上终止！",
                        "警告",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    System.Windows.Forms.Application.Exit();
                    }

                ModifyRegistry.Close();

                }
            catch (Exception ex) 
                {
                MessageBox.Show(ex.Message);
                }
            
            }

        /// <summary>
        /// 授权使用
        /// </summary>
        private void SetAuthorization() 
            {

            try
                {

                ModifyRegistry = PC.Registry.CurrentUser.OpenSubKey("SOFTWARE\\" 
                    + strSoftwareNameToBeAuthorizeVerify + "\\Authorization\\CurrentUser",
                    Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);

                if (txtPassword.Text == "deletepengdongnan") 
                    {
                    ModifyRegistry.SetValue("Authorized", "NO");
                    return;
                    }

                if (ModifyRegistry.GetValue("Authorized").ToString().ToUpper() == "NO")
                    {
                    ModifyRegistry.SetValue("Authorized", "YES");
                    MessageBox.Show("已经授权成功，取消使用次数的限制，你可以无限制地使用此软件！",
                        "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                else 
                    {
                    MessageBox.Show("已经授权，取消重复授权操作！","提示",MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    }

                }
            catch (Exception ex) 
                {
                MessageBox.Show(ex.Message);
                }
            
            }

        #endregion
        
        }//class

    }//namespace