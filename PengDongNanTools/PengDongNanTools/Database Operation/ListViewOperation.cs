#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

#endregion

namespace PengDongNanTools
    {

    //针对ListView控件的各项增加、删除、查找操作
    /// <summary>
    /// 针对ListView控件的各项增加、删除、查找操作
    /// </summary>
    class ListViewOperation
        {

        #region "变量定义"

        /// <summary>
        /// 是否需要验证ListView控件的View种类，即控件的显示方式，默认显示方式是Details
        /// </summary>
        public bool NeedToVerifyViewType = false;

        /// <summary>
        /// 在执行删除操作时是否弹出提示对话框,默认true
        /// </summary>
        public bool ShowPromptWhenDelOrAdd = true;

        /// <summary>
        /// 是否添加相同记录到ListView控件
        /// </summary>
        public bool AddSameRecord = false;

        private bool SuccessBuiltNew = false;
        private bool PasswordIsCorrect = false;

        /// <summary>
        /// 如果实例化时已经传入目标ListView控件作为参数，则为false
        /// </summary>
        private bool NeedListView = true;
        private ListView TempListView=new ListView();

        /// <summary>
        /// 信息
        /// </summary>
        public string ErrorMessage = "";

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }

        #endregion

        #region "函数程序代码"

        //****************************
        //创建类的实例
        /// <summary>
        /// 创建类的实例
        /// </summary>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public ListViewOperation(string DLLPassword)
            {

            SuccessBuiltNew = false;
            PasswordIsCorrect = false;

            try
                {

                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan") || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;
                    SuccessBuiltNew = true;
                    NeedListView = true;
                    }
                else
                    {
                    PasswordIsCorrect = false;
                    SuccessBuiltNew = false;
                    MessageBox.Show("Right Prohibited.\return\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                    }

                }
            catch(Exception ex)
                {
                SuccessBuiltNew = false;
                MessageBox.Show("创建类的实例时出现错误！\r\n" +
                    ex.Message);
                }

            }

        //创建类的实例: 传入目标ListView控件作为参数
        /// <summary>
        /// 【重载】创建类的实例: 传入目标ListView控件作为参数
        /// </summary>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        /// <param name="TargetListView">目标ListView控件</param>
        public ListViewOperation(string DLLPassword,
            ref ListView TargetListView)
            {

            SuccessBuiltNew = false;
            PasswordIsCorrect = false;

            try
                {

                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan") || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;
                    SuccessBuiltNew = true;
                    TempListView = TargetListView;
                    NeedListView = false;
                    }
                else
                    {
                    PasswordIsCorrect = false;
                    SuccessBuiltNew = false;
                    MessageBox.Show("Right Prohibited.\return\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                    }

                }
            catch (Exception ex)
                {
                SuccessBuiltNew = false;
                MessageBox.Show("创建类的实例时出现错误！\r\n" +
                    ex.Message);
                }

            }

        //释放类的相关资源
        /// <summary>
        /// 释放类的相关资源
        /// </summary>
        public void Dispose()
            {
            try
                {

                TempListView = null;

                return;
                }
            catch (Exception)// ex)
                {
                //ErrorMessage = ex.Message;
                return;
                }
            }

        //****************************
        //删除ListView中的目标行
        /// <summary>
        /// 删除ListView中的目标行,从1开始计数
        /// </summary>
        /// <param name="TargetListView">目标ListView控件</param>
        /// <param name="TargetRow">ListView中需要删除的目标行</param>
        /// <returns></returns>
        public bool DelRow(ref ListView TargetListView,
            int TargetRow) 
            {

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return false;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TargetListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";

                        return false;
                        }
                    }

                //1、根据现有项的数量，无项目或者目标行超出现有行数则提示并退出；
                //行的选中索引【.FocusedItem.Index 】为-1代表没有选中任何行
                if (TargetRow <= 0 
                    | TargetRow > TargetListView.Items.Count
                    | TargetListView.Items.Count<=0)
                    {
                    //MessageBox.Show("The target row: " + TargetRow + " is overrange, please revise it and retry.\r\n" +
                    //   "目标行已经超出范围，请修改参数后重试.", "Warning",
                    //   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ErrorMessage = "目标行已经超出范围或者目标ListView控件没有数据行，请修改参数后重试.";
                    return false;
                    }

                if (ShowPromptWhenDelOrAdd == true)
                    {
                    if (MessageBox.Show("Are you sure to delete the row: " +
                        TargetRow + " ?\r\n" + "确定要删除行: " + TargetRow
                        + " 吗？", "请确认", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.No)
                        {
                        return false;
                        }
                    }

                //2、移除此项【实际的索引号是从0开始的，故在下面指令中需要-1】
                TargetListView.BeginUpdate();
                TargetListView.Items.RemoveAt(TargetRow-1);
                TargetListView.EndUpdate();

                //3、删除一行以后会产生事件：ItemSelectionChanged，需要考虑变更后的总行数并重新致焦点
                if (TargetListView.Items.Count > 1 & TargetRow >= 2)
                    {
                    TargetListView.Items[TargetRow - 2].Selected = true;
                    //TargetListView.Items[TargetRow - 2].Focused = true;
                    }
                else if (TargetListView.Items.Count == 1)
                    {
                    //TargetListView.Items[0].Focused = true;
                    TargetListView.Items[0].Selected = true;
                    }
                
                //if (TargetRow >= TargetListView.Items.Count &
                //    TargetListView.Items.Count > 1)
                //    {
                //    TargetListView.Items[TargetRow - 1].Focused = true;
                //    }
                //else if(TargetRow < TargetListView.Items.Count 
                //    & TargetRow > 1 & TargetListView.Items.Count > 1)
                //    {
                //    TargetListView.Items[TargetRow].Focused = true;
                //    }

                

                }
            catch (Exception ex) 
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            return true;

            }

        //删除实例化时传入的ListView中的目标行,从1开始计数
        /// <summary>
        /// 删除实例化时传入的ListView中的目标行,从1开始计数
        /// </summary>
        /// <param name="TargetRow"></param>
        /// <returns></returns>
        public bool DelRow(int TargetRow)
            {

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return false;
                    }

                //实例化时没有导入ListView控件参数
                if(NeedListView==true)
                    {
                    //MessageBox.Show("实例化时没有导入ListView控件参数");
                    ErrorMessage = "实例化时没有导入ListView控件参数";
                    return false;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TempListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";

                        return false;
                        }
                    }

                //1、根据现有项的数量，无项目或者目标行超出现有行数则提示并退出；
                //行的选中索引【.FocusedItem.Index 】为-1代表没有选中任何行
                if (TargetRow <= 0
                    | TargetRow > TempListView.Items.Count
                    | TempListView.Items.Count <= 0)
                    {
                    //MessageBox.Show("The target row: " + TargetRow + " is overrange, please revise it and retry.\r\n" +
                    //   "目标行已经超出范围，请修改参数后重试.", "Warning",
                    //   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ErrorMessage = "目标行已经超出范围或者目标ListView控件没有数据行，请修改参数后重试.";
                    return false;
                    }

                if (ShowPromptWhenDelOrAdd == true)
                    {
                    if (MessageBox.Show("Are you sure to delete the row: " +
                        TargetRow + " ?\r\n" + "确定要删除行: " + TargetRow
                        + " 吗？", "请确认", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.No)
                        {
                        return false;
                        }
                    }

                //2、移除此项【实际的索引号是从0开始的，故在下面指令中需要-1】
                TempListView.BeginUpdate();
                TempListView.Items.RemoveAt(TargetRow - 1);

                //3、删除一行以后会产生事件：ItemSelectionChanged，需要考虑变更后的总行数并重新致焦点
                if (TempListView.Items.Count > 1)
                    {
                    TempListView.Items[TargetRow - 1].Focused = true;
                    }
                else
                    {
                    TempListView.Items[0].Focused = true;
                    }

                //if (TargetRow >= TempListView.Items.Count &
                //    TempListView.Items.Count > 1)
                //    {
                //    TempListView.Items[TargetRow - 1].Focused = true;
                //    }
                //else if(TargetRow < TempListView.Items.Count 
                //    & TargetRow > 1 & TempListView.Items.Count > 1)
                //    {
                //    TempListView.Items[TargetRow].Focused = true;
                //    }

                TempListView.EndUpdate();

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            return true;

            }

        //****************************
        //添加单行记录至ListView控件
        /// <summary>
        /// 添加单行记录至ListView控件
        /// </summary>
        /// <param name="TargetListView">目标ListView控件</param>
        /// <param name="Contens">要添加至单行的内容</param>
        /// <returns></returns>
        public bool AddRowRecordInListView(ref ListView TargetListView, 
            string[] Contens) 
            {

            if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                {
                //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                return false;
                }

            try
                {

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TargetListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";

                        return false;
                        }
                    }

                //Int16 TempCount=0;

                //1、根据现有列的数量，无列或者目标列超出现有列数则提示并退出；
                if(TargetListView.Columns.Count == 1 
                    | TargetListView.Columns.Count < Contens.Length)
                    {
                    //MessageBox.Show("ListView has " + TargetListView.Columns.Count + " column(s), the q'ty of contents" +
                    //"is overrange, please revise it and retry.\r\n ListView控件有" + 
                    //TargetListView.Columns.Count + "列, 添加的内容已经大于可用列的数量，请修改参数后重试.");
                    ErrorMessage="ListView控件有" + TargetListView.Columns.Count + 
                        "列, 添加的内容已经大于可用列的数量，请修改参数后重试.";
                    return false;
                    }

                //2、首先查找ListView中第一列是否已经存在相同的内容，
                //如果存在则提示是否要添加重复的项并退出函数；否则进行添加操作
                //ushort TempReturnRowIndex=0;

                if (AddSameRecord == false)
                    {

                    if (SearchItemInRowOfListView(ref TargetListView, Contens[0], 1) == true)
                        {

                        if (SearchItemInRowOfListView(ref TargetListView, Contens[1], 2) == false)
                            {

                            if (MessageBox.Show("The " + Contens[0] + " is already exist in the first column, but the 2nd column" +
                                "is different, are you sure to add it?\r\n" + Contens[0] + " 已经存在于第一列,但是第二列不同，确定要添加吗？",
                                "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                {
                                return false;
                                }

                            }
                        else
                            {
                            ErrorMessage = "第一列和第二列中已经存在相同的记录，未添加.";
                            //MessageBox.Show("第一列和第二列中已经存在相同的记录，未添加.");
                            return false;
                            }

                        }

                    }

                //3、添加此新记录到列表
                TargetListView.BeginUpdate();
                ListViewItem AddNewItemsForTargetListView=new ListViewItem();
                for(int a=0;a<Contens.Length;a++)
                    {
                    AddNewItemsForTargetListView.SubItems.Add(Contens[a]);
                    }
                TargetListView.Items.Add(AddNewItemsForTargetListView);
                TargetListView.EndUpdate();

                }
            catch (Exception ex) 
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            return true;
            
            }

        //添加单行记录至实例化时传入的ListView控件
        /// <summary>
        /// 添加单行记录至实例化时传入的ListView控件
        /// </summary>
        /// <param name="Contens"></param>
        /// <returns></returns>
        public bool AddRowRecordInListView(string[] Contens)
            {

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return false;
                    }

                //实例化时没有导入ListView控件参数
                if (NeedListView == true)
                    {
                    //MessageBox.Show("实例化时没有导入ListView控件参数");
                    ErrorMessage = "实例化时没有导入ListView控件参数";
                    return false;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TempListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";

                        return false;
                        }
                    }

                //Int16 TempCount=0;

                //1、根据现有列的数量，无列或者目标列超出现有列数则提示并退出；
                if(TempListView.Columns.Count == 1 
                    | TempListView.Columns.Count < Contens.Length)
                    {
                    //MessageBox.Show("ListView has " + TempListView.Columns.Count + " column(s), the q'ty of contents" +
                    //"is overrange, please revise it and retry.\r\n ListView控件有" + 
                    //TempListView.Columns.Count + "列, 添加的内容已经大于可用列的数量，请修改参数后重试.");
                    ErrorMessage="ListView控件有" + TempListView.Columns.Count + 
                        "列, 添加的内容已经大于可用列的数量，请修改参数后重试.";
                    return false;
                    }

                //2、首先查找ListView中第一列是否已经存在相同的内容，
                //如果存在则提示是否要添加重复的项并退出函数；否则进行添加操作
                //ushort TempReturnRowIndex=0;

                if (AddSameRecord == false)
                    {

                    if (SearchItemInRowOfListView(ref TempListView, Contens[0], 1) == true)
                        {

                        if (SearchItemInRowOfListView(ref TempListView, Contens[1], 2) == false)
                            {

                            //if (ShowPromptWhenDelOrAdd == true)
                                //{
                                if (MessageBox.Show("The " + Contens[0] + " is already exist in the first column, but the 2nd column" +
                               "is different, are you sure to add it?\r\n" + Contens[0] + " 已经存在于第一列,但是第二列不同，确定要添加吗？",
                               "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                    {
                                    return false;
                                    }
                                //}
                            }
                        else
                            {
                            ErrorMessage = "第一列和第二列中已经存在相同的记录，未添加.";
                            //MessageBox.Show("第一列和第二列中已经存在相同的记录，未添加.");
                            return false;
                            }

                        }

                    }
                
                //3、添加此新记录到列表
                TempListView.BeginUpdate();
                ListViewItem AddNewItemsForTargetListView=new ListViewItem();
                for(int a=0;a<Contens.Length;a++)
                    {
                    AddNewItemsForTargetListView.SubItems.Add(Contens[a]);
                    }
                TempListView.Items.Add(AddNewItemsForTargetListView);
                TempListView.EndUpdate();

                }
            catch (Exception ex)
                {
                MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            return true;

            }

        //****************************
        //修改ListView控件某行的记录
        /// <summary>
        /// 修改ListView控件某行的记录
        /// </summary>
        /// <param name="TargetListView">目标ListView控件</param>
        /// <param name="Contens">要修改行的新内容</param>
        /// <param name="TargetRow">目标行的索引</param>
        /// <returns>是否执行成功</returns>
        public bool ModifyRowRecord(ref ListView TargetListView,
            string[] Contens, int TargetRow)
            {

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return false;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TargetListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";
                        return false;
                        }
                    }

                //Int16 TempCount=0;

                //1、根据现有列的数量，无列或者目标列超出现有列数则提示并退出；
                if(TargetListView.Columns.Count == 1 
                    | TargetListView.Columns.Count < Contens.Length)
                    {
                    //MessageBox.Show("ListView has " + TargetListView.Columns.Count + " column(s), the q'ty of contents" +
                    //"is overrange, please revise it and retry.\r\n ListView控件有" + 
                    //TargetListView.Columns.Count + "列, 添加的内容已经大于可用列的数量，请修改参数后重试.");
                    ErrorMessage="ListView控件有" + TargetListView.Columns.Count + 
                        "列, 添加的内容已经大于可用列的数量，请修改参数后重试.";
                    return false;
                    }

                //2、如果目标列大于实际已有的总列数，则提示超出范围【可以考虑是否添加到最后】
                if(TargetListView.Items.Count < TargetRow)
                    {
                    //MessageBox.Show("ListView has " + TargetListView.Items.Count + " row(s), the targetRow:" +
                    //    TargetRow + "is over range, please revise it and retry.\r\n" + 
                    //    "ListView控件有 " + TargetListView.Items.Count +
                    //    " 行, 目标行已经大于可用行的数量，请修改参数后重试.");
                    ErrorMessage = "ListView控件有 " + TargetListView.Items.Count +
                        " 行, 目标行已经大于可用行的数量，请修改参数后重试.";
                    return false;
                    }

                //3、直接将当前新值修改到ListView中的目标列
                TargetListView.BeginUpdate();
                for(int a=0; a<Contens.Length;a++)
                    {
                    //列是从1开始放字符串，第0列是默认放图标的，故+1
                    //行的索引是从0，故-1
                    TargetListView.Items[TargetRow - 1].SubItems[a + 1].Text = Contens[a];
                    }
                TargetListView.EndUpdate();
                ErrorMessage="已经成功修改第 " + TargetRow + " 行的记录.";

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            return true;

            }

        //修改实例化时传入的ListView控件某行的记录
        /// <summary>
        /// 修改实例化时传入的ListView控件某行的记录
        /// </summary>
        /// <param name="Contens">要修改行的新内容</param>
        /// <param name="TargetRow">目标行的索引</param>
        /// <returns>是否执行成功</returns>
        public bool ModifyRowRecord(string[] Contens, int TargetRow)
            {

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return false;
                    }

                //实例化时没有导入ListView控件参数
                if (NeedListView == true)
                    {
                    //MessageBox.Show("实例化时没有导入ListView控件参数");
                    ErrorMessage = "实例化时没有导入ListView控件参数";
                    return false;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TempListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";

                        return false;
                        }
                    }        

                //int TempCount=0;

                //1、根据现有列的数量，无列或者目标列超出现有列数则提示并退出；
                if(TempListView.Columns.Count == 1 
                    | TempListView.Columns.Count < Contens.Length)
                    {
                    //MessageBox.Show("ListView has " + TempListView.Columns.Count + " column(s), the q'ty of contents" +
                    //"is overrange, please revise it and retry.\r\n ListView控件有" + 
                    //TempListView.Columns.Count + "列, 添加的内容已经大于可用列的数量，请修改参数后重试.");
                    ErrorMessage="ListView控件有" + TempListView.Columns.Count + 
                        "列, 添加的内容已经大于可用列的数量，请修改参数后重试.";
                    return false;
                    }

                //2、如果目标列大于实际已有的总列数，则提示超出范围【可以考虑是否添加到最后】
                if(TempListView.Items.Count < TargetRow)
                    {
                    //MessageBox.Show("ListView has " + TempListView.Items.Count + " row(s), the targetRow:" +
                    //    TargetRow + "is over range, please revise it and retry.\r\n" + 
                    //    "ListView控件有 " + TempListView.Items.Count +
                    //    " 行, 目标行已经大于可用行的数量，请修改参数后重试.");
                    ErrorMessage = "ListView控件有 " + TempListView.Items.Count + 
                        " 行, 目标行已经大于可用行的数量，请修改参数后重试.";
                    return false;
                    }

                //3、直接将当前新值修改到ListView中的目标列
                TempListView.BeginUpdate();
                for(int a=0; a<Contens.Length;a++)
                    {
                    //列是从1开始放字符串，第0列是默认放图标的，故+1
                    //行的索引是从0，故-1
                    TempListView.Items[TargetRow - 1].SubItems[a + 1].Text = Contens[a];
                    }
                TempListView.EndUpdate();
                ErrorMessage="已经成功修改第 " + TargetRow + " 行的记录.";

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            return true;

            }
        
        //****************************
        //在ListView的所有列中查找内容并返回是否找到及找到第一个匹配项的位置索引号
        /// <summary>
        /// 在ListView的所有列中查找内容并返回是否找到及找到第一个匹配项的位置索引号
        /// </summary>
        /// <param name="TargetListView">要搜索的目标ListView控件</param>
        /// <param name="ContentsToBeSearched">要搜索的内容</param>
        /// <param name="RowIndexInListViewAfterSearch">返回搜索到的第一个匹配项的行索引号</param>
        /// <returns>是否找到匹配项</returns>
        public bool SearchItemInRowOfListView(ref ListView TargetListView,
            string ContentsToBeSearched, out int RowIndexInListViewAfterSearch)
            {

            bool TempSearchingResult=false;
            int FoundIndex=0;

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    RowIndexInListViewAfterSearch = 0;
                    return false;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TargetListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";
                        RowIndexInListViewAfterSearch = 0;
                        return false;
                        }
                    }                      

                //1、根据现有项的数量，无项目则提示并退出；
                if(TargetListView.Items.Count==0)
                    {
                    //MessageBox.Show("There is no item in the ListView Control, cancel the searching operation.\r\n" +
                    //   "ListView控件为空，取消搜索操作.");
                    ErrorMessage = "ListView控件为空，取消搜索操作.";
                    RowIndexInListViewAfterSearch = 0;
                    return false;
                    }

                //2、首先查找ListView中是否已经存在相同的内容，如果存在则提示并退出函数；
                for(int a=0; a<TargetListView.Items.Count;a++)
                    {
                    
                    for(int b=0; b<TargetListView.Items[a].SubItems.Count;b++)
                        {

                        if(TargetListView.Items[a].SubItems[b].Text == ContentsToBeSearched)
                            {
                            TempSearchingResult=true;
                            FoundIndex=a+1;
                            break;
                            }

                        }

                    }

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                RowIndexInListViewAfterSearch=0;
                return false;
                }

            if(TempSearchingResult==true)
                {
                RowIndexInListViewAfterSearch = FoundIndex;
                return true;
                }
            else
                {
                RowIndexInListViewAfterSearch=0;
                return false;
                }

            }

        //在实例化时传入的ListView的所有列中查找内容并返回是否找到及找到第一个匹配项的位置索引号
        /// <summary>
        /// 在实例化时传入的ListView的所有列中查找内容并返回是否找到及找到第一个匹配项的位置索引号
        /// </summary>
        /// <param name="ContentsToBeSearched">要搜索的内容</param>
        /// <param name="RowIndexInListViewAfterSearch">返回搜索到的第一个匹配项的行索引号</param>
        /// <returns>是否找到匹配项</returns>
        public bool SearchItemInRowOfListView(string ContentsToBeSearched, 
            out int RowIndexInListViewAfterSearch)
            {

           bool TempSearchingResult=false;
            int FoundIndex=0;

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    RowIndexInListViewAfterSearch = 0;
                    return false;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TempListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";
                        RowIndexInListViewAfterSearch = 0;
                        return false;
                        }
                    }

                //1、根据现有项的数量，无项目则提示并退出；
                if(TempListView.Items.Count==0)
                    {
                    //MessageBox.Show("There is no item in the ListView Control, cancel the searching operation.\r\n" +
                    //   "ListView控件为空，取消搜索操作.");
                    ErrorMessage = "ListView控件为空，取消搜索操作.";
                    RowIndexInListViewAfterSearch = 0;
                    return false;
                    }

                //2、首先查找ListView中是否已经存在相同的内容，如果存在则提示并退出函数；
                for(int a=0; a<TempListView.Items.Count;a++)
                    {
                    
                    for(int b=0; b<TempListView.Items[a].SubItems.Count;b++)
                        {

                        if(TempListView.Items[a].SubItems[b].Text == ContentsToBeSearched)
                            {
                            TempSearchingResult=true;
                            FoundIndex=a+1;
                            break;
                            }

                        }

                    }

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                RowIndexInListViewAfterSearch = 0;
                return false;
                }

            if(TempSearchingResult == true)
                {
                RowIndexInListViewAfterSearch = FoundIndex;
                return true;
                }
            else
                {
                RowIndexInListViewAfterSearch = 0;
                return false;
                }

            }

        //****************************
        //在ListView的所有列中查找内容并返回是否找到
        /// <summary>
        /// 在ListView的所有列中查找内容并返回是否找到
        /// </summary>
        /// <param name="TargetListView">要搜索的目标ListView控件</param>
        /// <param name="ContentsToBeSearched">要搜索的内容</param>
        /// <returns>是否找到匹配项</returns>
        public bool SearchItemInRowOfListView(ref ListView TargetListView,
            string ContentsToBeSearched)//, int ColumnIndexInListViewForSearch
            {

            bool TempSearchingResult = false;
            int FoundIndex = 0;

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return false;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TargetListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";
                        return false;
                        }
                    }                

                //1、根据现有项的数量，无项目则提示并退出；
                if (TargetListView.Items.Count == 0)
                    {
                    //MessageBox.Show("There is no item in the ListView Control, cancel the searching operation.\r\n" +
                    //   "ListView控件为空，取消搜索操作.");
                    ErrorMessage = "ListView控件为空，取消搜索操作.";
                    return false;
                    }

                //2、首先查找ListView中是否已经存在相同的内容，如果存在则提示并退出函数；
                for (int a = 0; a < TargetListView.Items.Count; a++)
                    {

                    for (int b = 0; b < TargetListView.Items[a].SubItems.Count; b++)
                        {

                        if (TargetListView.Items[a].SubItems[b].Text == ContentsToBeSearched)
                            {
                            TempSearchingResult = true;
                            FoundIndex = a + 1;
                            break;
                            }

                        }

                    }

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            if (TempSearchingResult == true)
                {
                return true;
                }
            else
                {
                return false;
                }

            }

        //在实例化时传入的ListView的所有列中查找内容并返回是否找到
        /// <summary>
        /// 在实例化时传入的ListView的所有列中查找内容并返回是否找到
        /// </summary>
        /// <param name="ContentsToBeSearched">要搜索的内容</param>        
        /// <returns>是否找到匹配项</returns>
        public bool SearchItemInRowOfListView(string ContentsToBeSearched)
            //,            int ColumnIndexInListViewForSearch)
            {

            bool TempSearchingResult = false;
            int FoundIndex = 0;

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return false;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TempListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";
                        return false;
                        }
                    }                

                //1、根据现有项的数量，无项目则提示并退出；
                if (TempListView.Items.Count == 0)
                    {
                    //MessageBox.Show("There is no item in the ListView Control, cancel the searching operation.\r\n" +
                    //   "ListView控件为空，取消搜索操作.");
                    return false;
                    }

                //2、首先查找ListView中是否已经存在相同的内容，如果存在则提示并退出函数；
                for (int a = 0; a < TempListView.Items.Count; a++)
                    {

                    for (int b = 0; b < TempListView.Items[a].SubItems.Count; b++)
                        {

                        if (TempListView.Items[a].SubItems[b].Text == ContentsToBeSearched)
                            {
                            TempSearchingResult = true;
                            FoundIndex = a + 1;
                            break;
                            }

                        }

                    }

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            if (TempSearchingResult == true)
                {
                return true;
                }
            else
                {
                return false;
                }

            }

        //****************************
        //在ListView的某个列中查找内容并返回是否找到
        /// <summary>
        /// 在ListView的某个列中查找内容并返回是否找到
        /// </summary>
        /// <param name="TargetListView">要搜索的目标ListView控件</param>
        /// <param name="ContentsToBeSearched">要搜索的内容</param>
        /// <param name="ColumnIndexInListViewForSearch">搜索匹配项的行索引号</param>
        /// <returns>是否找到匹配项</returns>
        public bool SearchItemInRowOfListView(ref ListView TargetListView,
            string ContentsToBeSearched, int ColumnIndexInListViewForSearch)
            {
            
            bool TempSearchingResult = false;
            int FoundIndex = 0;

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return false;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TargetListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";
                        return false;
                        }
                    }                

                //1、根据现有项的数量，无项目则提示并退出；
                if (TargetListView.Items.Count == 0)
                    {
                    //MessageBox.Show("There is no item in the ListView Control, cancel the searching operation.\r\n" +
                    //   "ListView控件为空，取消搜索操作.");
                    return false;
                    }

                //2、首先查找ListView中是否已经存在相同的内容，如果存在则提示并退出函数；
                for (int a = 0; a < TargetListView.Items.Count; a++)
                    {
                    if (TargetListView.Items[a].SubItems[ColumnIndexInListViewForSearch].Text == ContentsToBeSearched)
                        {
                        TempSearchingResult = true;
                        FoundIndex = a + 1;
                        break;
                        }
                    }

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            if (TempSearchingResult == true)
                {
                return true;
                }
            else
                {
                return false;
                }

            }

        //在实例化时传入的ListView的某个列中查找内容并返回是否找到
        /// <summary>
        /// 在实例化时传入的ListView的某个列中查找内容并返回是否找到
        /// </summary>
        /// <param name="ContentsToBeSearched">要搜索的内容</param>
        /// <param name="ColumnIndexInListViewForSearch">搜索匹配项的行索引号</param>
        /// <returns>是否找到匹配项</returns>
        public bool SearchItemInRowOfListView(string ContentsToBeSearched,
            int ColumnIndexInListViewForSearch)
            {
            
            bool TempSearchingResult = false;
            int FoundIndex = 0;

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return false;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TempListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";
                        return false;
                        }
                    }                

                //1、根据现有项的数量，无项目则提示并退出；
                if (TempListView.Items.Count == 0)
                    {
                    //MessageBox.Show("There is no item in the ListView Control, cancel the searching operation.\r\n" +
                    //   "ListView控件为空，取消搜索操作.");
                    return false;
                    }

                //2、首先查找ListView中是否已经存在相同的内容，如果存在则提示并退出函数；
                for (int a = 0; a < TempListView.Items.Count; a++)
                    {
                    if (TempListView.Items[a].SubItems[ColumnIndexInListViewForSearch].Text == ContentsToBeSearched)
                        {
                        TempSearchingResult = true;
                        FoundIndex = a + 1;
                        break;
                        }
                    }

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            if (TempSearchingResult == true)
                {
                return true;
                }
            else
                {
                return false;
                }

            }

        //****************************
        //移除ListView控件中的所有项
        /// <summary>
        /// 移除ListView控件中的所有项
        /// </summary>
        /// <param name="TargetListView">目标ListView控件</param>
        /// <returns></returns>
        public bool DelAllRows(ref ListView TargetListView)
            {

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return false;
                    }

                if (ShowPromptWhenDelOrAdd == true)
                    {
                    if(MessageBox.Show("Are you sure to delete all rows?\r\n" +
                        "确定要删除所有行吗？","请确认",MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question)==DialogResult.No)
                        {
                        return false;
                        }
                    }

                TargetListView.Items.Clear();
                return true;

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            }

        //移除实例化时传入的ListView控件中的所有项
        /// <summary>
        /// 移除实例化时传入的ListView控件中的所有项
        /// </summary>
        /// <returns></returns>
        public bool DelAllRows()
            {

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return false;
                    }

                //实例化时没有导入ListView控件参数
                if (NeedListView == true)
                    {
                    //MessageBox.Show("实例化时没有导入ListView控件参数");
                    ErrorMessage = "实例化时没有导入ListView控件参数";
                    return false;
                    }

                if (ShowPromptWhenDelOrAdd == true)
                    {
                    if (MessageBox.Show("Are you sure to delete all rows?\r\n" +
                        "确定要删除所有行吗？", "请确认", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.No)
                        {
                        return false;
                        }
                    }

                TempListView.Items.Clear();
                return true;

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            }

        //****************************
        //查找ListView控件中某列相同项的数量
        /// <summary>
        /// 查找ListView控件中某列相同项的数量
        /// </summary>
        /// <param name="TargetListView">要搜索的目标ListView控件</param>
        /// <param name="ContentsToBeSearched">要搜索的内容</param>
        /// <param name="ColumnInListViewForSearch">搜索匹配项的行索引号</param>
        /// <returns>返回查找到的匹配项数量</returns>
        public int FindSameItemQty(ref ListView TargetListView,
            string ContentsToBeSearched, int ColumnInListViewForSearch) 
            {

            bool TempSearchingResult = false;
            int FoundQty = 0;

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return 0;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TargetListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";
                        return 0;
                        }
                    }

                //1、根据现有项的数量，无项目则提示并退出；
                if (TargetListView.Items.Count == 0)
                    {
                    //MessageBox.Show("There is no item in the ListView Control, cancel the searching operation.\r\n" +
                    //   "ListView控件为空，取消搜索操作.");
                    ErrorMessage = "ListView控件为空，取消搜索操作.";
                    return 0;
                    }

                //2、首先查找ListView中是否已经存在相同的内容，如果存在则提示并退出函数；
                for (int a = 0; a < TargetListView.Items.Count; a++)
                    {
                    if (TargetListView.Items[a].SubItems[ColumnInListViewForSearch].Text == ContentsToBeSearched)
                        {
                        TempSearchingResult = true;
                        FoundQty = FoundQty + 1;
                        }
                    }

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return 0;
                }

            if (TempSearchingResult == true)
                {
                return FoundQty;
                }
            else
                {
                return 0;
                }

            }

        //查找实例化时传入的ListView控件中某列相同项的数量
        /// <summary>
        /// 查找实例化时传入的ListView控件中某列相同项的数量
        /// </summary>
        /// <param name="ContentsToBeSearched">要搜索的内容</param>
        /// <param name="ColumnInListViewForSearch">搜索匹配项的行索引号</param>
        /// <returns>返回查找到的匹配项数量</returns>
        public int FindSameItemQty(string ContentsToBeSearched, int ColumnInListViewForSearch)
            {

            bool TempSearchingResult = false;
            int FoundQty = 0;

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return 0;
                    }

                //实例化时没有导入ListView控件参数
                if (NeedListView == true)
                    {
                    //MessageBox.Show("实例化时没有导入ListView控件参数");
                    ErrorMessage = "实例化时没有导入ListView控件参数";
                    return 0;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TempListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";
                        return 0;
                        }
                    }

                //1、根据现有项的数量，无项目则提示并退出；
                if (TempListView.Items.Count == 0)
                    {
                    //MessageBox.Show("There is no item in the ListView Control, cancel the searching operation.\r\n" +
                    //   "ListView控件为空，取消搜索操作.");
                    ErrorMessage = "ListView控件为空，取消搜索操作.";
                    return 0;
                    }

                //2、首先查找ListView中是否已经存在相同的内容，如果存在则提示并退出函数；
                for (int a = 0; a < TempListView.Items.Count; a++)
                    {
                    if (TempListView.Items[a].SubItems[ColumnInListViewForSearch].Text == ContentsToBeSearched)
                        {
                        TempSearchingResult = true;
                        FoundQty = FoundQty + 1;
                        }
                    }

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return 0;
                }

            if (TempSearchingResult == true)
                {
                return FoundQty;
                }
            else
                {
                return 0;
                }

            }

        //****************************
        //查找ListView控件中所有列相同项的数量
        /// <summary>
        /// 查找ListView控件中所有列相同项的数量
        /// </summary>
        /// <param name="TargetListView">要搜索的目标ListView控件</param>
        /// <param name="ContentsToBeSearched">要搜索的内容</param>
        /// <returns>返回查找到的匹配项数量</returns>
        public int FindAllSameItemQty(ref ListView TargetListView,
            string ContentsToBeSearched)
            {

            bool TempSearchingResult = false;
            int FoundQty = 0;

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return 0;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TargetListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";
                        return 0;
                        }
                    }

                //1、根据现有项的数量，无项目则提示并退出；
                if (TargetListView.Items.Count == 0)
                    {
                    //MessageBox.Show("There is no item in the ListView Control, cancel the searching operation.\r\n" +
                    //   "ListView控件为空，取消搜索操作.");
                    ErrorMessage = "ListView控件为空，取消搜索操作.";
                    return 0;
                    }

                //2、首先查找ListView中是否已经存在相同的内容，如果存在则提示并退出函数；
                for (int a = 0; a < TargetListView.Items.Count; a++)
                    {
                    for (int b = 0; b < TargetListView.Items[a].SubItems.Count; b++)
                        {
                        if (TargetListView.Items[a].SubItems[b].Text == ContentsToBeSearched)
                            {
                            TempSearchingResult = true;
                            FoundQty = FoundQty + 1;
                            }
                        }
                    }

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return FoundQty;
                }

            if (TempSearchingResult == true)
                {
                return FoundQty;
                }
            else
                {
                return 0;
                }

            }

        //查找实例化时传入的ListView控件中所有列相同项的数量
        /// <summary>
        /// 查找实例化时传入的ListView控件中所有列相同项的数量
        /// </summary>
        /// <param name="ContentsToBeSearched">要搜索的内容</param>
        /// <returns>返回查找到的匹配项数量</returns>
        public int FindAllSameItemQty(string ContentsToBeSearched)
            {

            bool TempSearchingResult = false;
            int FoundQty = 0;

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return 0;
                    }

                //实例化时没有导入ListView控件参数
                if (NeedListView == true)
                    {
                    //MessageBox.Show("实例化时没有导入ListView控件参数");
                    ErrorMessage = "实例化时没有导入ListView控件参数";
                    return 0;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TempListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";
                        return 0;
                        }
                    }                

                //1、根据现有项的数量，无项目则提示并退出；
                if (TempListView.Items.Count == 0)
                    {
                    //MessageBox.Show("There is no item in the ListView Control, cancel the searching operation.\r\n" +
                    //   "ListView控件为空，取消搜索操作.");
                    ErrorMessage = "ListView控件为空，取消搜索操作.";
                    return 0;
                    }

                //2、首先查找ListView中是否已经存在相同的内容，如果存在则提示并退出函数；
                for (int a = 0; a < TempListView.Items.Count; a++)
                    {
                    for (int b = 0; b < TempListView.Items[a].SubItems.Count; b++)
                        {
                        if (TempListView.Items[a].SubItems[b].Text == ContentsToBeSearched)
                            {
                            TempSearchingResult = true;
                            FoundQty = FoundQty + 1;
                            }
                        }
                    }

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return FoundQty;
                }

            if (TempSearchingResult == true)
                {
                return FoundQty;
                }
            else
                {
                return 0;
                }

            }

        //****************************
        //修改ListView控件某行的背景颜色
        /// <summary>
        /// 修改ListView控件某行的背景颜色
        /// </summary>
        /// <param name="TargetListView">目标ListView控件</param>
        /// <param name="TargetRow">目标行1~N</param>
        /// <param name="NewRowColor">新背景颜色</param>
        /// <returns>是否执行成功</returns>
        public bool ModifyRowBackColor(ref ListView TargetListView, int TargetRow,
            Color NewRowColor)
            {

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return false;
                    }

                if (TargetRow <= 0)
                    {
                    return false;
                    }

                if (TargetListView == null)
                    {
                    return false;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TargetListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";
                        return false;
                        }
                    }

                //Int16 TempCount=0;

                //1、根据现有行的数量，无行或者目标行超出现有行数则提示并退出；
                if (TargetListView.Items.Count < 1 
                    || TargetListView.Items.Count < TargetRow)
                    {
                    //MessageBox.Show("ListView has " + TargetListView.Items.Count + " row(s), the q'ty of contents" +
                    //"is overrange, please revise it and retry.\r\n ListView控件有" +
                    //TargetListView.Items.Count + "行, 添加的内容已经大于可用行的数量，请修改参数后重试.");
                    ErrorMessage = "ListView控件有" + TargetListView.Items.Count +
                        "行, 添加的内容已经大于可用行的数量，请修改参数后重试.";
                    return false;
                    }

                ////2、如果目标列大于实际已有的总列数，则提示超出范围【可以考虑是否添加到最后】
                //if (TargetListView.Items.Count < TargetRow)
                //    {
                //    MessageBox.Show("ListView has " + TargetListView.Items.Count + " row(s), the targetRow:" +
                //        TargetRow + "is over range, please revise it and retry.\r\n" +
                //        "ListView控件有 " + TargetListView.Items.Count +
                //        " 行, 目标行已经大于可用行的数量，请修改参数后重试.");
                //    return false;
                //    }

                //3、直接将当前新值修改到ListView中的目标行
                TargetListView.BeginUpdate();
                //行的索引是从0，故-1
                TargetListView.Items[TargetRow - 1].BackColor=NewRowColor;
                TargetListView.EndUpdate();

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            return true;

            }

        //修改实例化时传入的ListView控件某行的背景颜色
        /// <summary>
        /// 修改实例化时传入的ListView控件某行的背景颜色
        /// </summary>
        /// <param name="TargetRow">目标行1~N</param>
        /// <param name="NewRowColor">新背景颜色</param>
        /// <returns>是否执行成功</returns>
        public bool ModifyRowBackColor(int TargetRow, Color NewRowColor)
            {

            try
                {
                if (SuccessBuiltNew == false | PasswordIsCorrect == false)
                    {
                    //MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    //    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorMessage = "此类实例化失败，请检查原因后再尝试.";
                    return false;
                    }

                if (TargetRow <= 0)
                    {
                    return false;
                    }

                //实例化时没有导入ListView控件参数
                if (NeedListView == true)
                    {
                    //MessageBox.Show("实例化时没有导入ListView控件参数");
                    ErrorMessage = "实例化时没有导入ListView控件参数";
                    return false;
                    }

                if (NeedToVerifyViewType == true)
                    {
                    //判断ListView控件的"View"属性是不是 "Details"
                    if (TempListView.View != View.Details)
                        {
                        //MessageBox.Show("The style of target ListView is not \'Details\', so the operation is aborted.\r\n" +
                        //    "ListView控件的\'View\'属性不是 \'Details\'，终止操作.", "Warning",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ErrorMessage = "ListView控件的\'View\'属性不是 \'Details\'，终止操作.";

                        return false;
                        }
                    }

                //1、如果目标列大于实际已有的总列数，则提示超出范围【可以考虑是否添加到最后】
                if (TempListView.Items.Count < TargetRow
                    || TempListView.Items.Count < 1)
                    {
                    //MessageBox.Show("ListView has " + TempListView.Items.Count + " row(s), the targetRow:" +
                    //    TargetRow + "is over range, please revise it and retry.\r\n" +
                    //    "ListView控件有 " + TempListView.Items.Count +
                    //    " 行, 目标行已经大于可用行的数量，请修改参数后重试.");
                    ErrorMessage = "ListView控件有 " + TempListView.Items.Count 
                        + " 行, 目标行已经大于可用行的数量，请修改参数后重试.";
                    return false;
                    }

                //3、直接将当前新值修改到ListView中的目标行
                TempListView.BeginUpdate();
                //行的索引是从0，故-1
                TempListView.Items[TargetRow - 1].BackColor=NewRowColor;
                TempListView.EndUpdate();

                }
            catch (Exception ex)
                {
                //MessageBox.Show("" + ex.Message);
                ErrorMessage = ex.Message;
                return false;
                }

            return true;

            }
        //*************************

        #endregion

        }//class

    }//namespace