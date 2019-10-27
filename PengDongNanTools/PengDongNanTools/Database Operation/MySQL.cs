#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient; 

#endregion

namespace PengDongNanTools
    {

    #region "待处理事项"

    //######################################################################################################
    //暂时进行到此，目前除了需要连接数据库、写记录和读取记录等简单的用途外，其它还没有需要，故暂时不进行下去；
    //######################################################################################################

    //1、添加数据和变更数据时只考虑了数字和文字两种情况，不知道日期等他数据进行更改时是否需要加‘’号；【】

    //2、打开数据库连接，在处理后再关闭，不要重复执行ExecuteSQL【因为它是开一次就关一次，没有必要且浪费时间】

    //3、然后获取数据库的记录，和传递进来的TargetDatagridView进行比较，看列名称是否一致，否则就提醒并退出；

    //4、最好是在编辑DataGridView的事件中添加状态数组，用来保存标志看某一行是否已经变更了，如果没有变更则在保存的时候就可以跳过；

    //5、还要解决一个问题：如果来界定WHERE的是对应的行；

    #endregion

    //MySQL数据库操作
    /// <summary>
    /// MySQL数据库操作
    /// </summary>
    class MySQL
        {

        #region "变量定义"

        private string ConnectStr="";
        //private string NameOfDatabase = "";
        private MySqlConnection MyConnection;
        private MySqlCommand MyCommand;
        private string ServerName, UserID, Password, DatabaseName;
        private bool SuccessBulitNew = false;
        private bool PasswordIsCorrect = false;

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }

        #endregion

        #region "已完成代码"

        //创建MySQL数据库连接类的实例
        /// <summary>
        /// 创建MySQL数据库连接类的实例
        /// </summary>
        /// <param name="TargetServerName">数据库服务器名</param>
        /// <param name="TargetUserID">登录的用户标识符</param>
        /// <param name="TargetDatabasePassword">数据库密码</param>
        /// <param name="TargetDatabaseName">需要连接的数据库名称</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public MySQL(string TargetServerName, string TargetUserID,
            string TargetDatabasePassword, string TargetDatabaseName,
            string DLLPassword) 
            {

            try
                {

                SuccessBulitNew = false;
                PasswordIsCorrect = false;

                if (DLLPassword == "ThomasPeng" | DLLPassword == "pengdongnan"
                    | DLLPassword == "彭东南")
                    {
                    PasswordIsCorrect = true;

                    try
                        {
                         ConnectStr = "server=" + TargetServerName + "; user id=" + TargetUserID + "; password=" + TargetDatabasePassword +
                        "; database=" + TargetDatabaseName + ";Persist Security Info=false";
                         MyConnection = new MySqlConnection(ConnectStr);
                         MyCommand = new MySqlCommand();
                        MyCommand.Connection = MyConnection;
                        MyCommand.Connection.Open();
                        //MyConnection.Close();

                        SuccessBulitNew = true;
                        ServerName = TargetServerName;
                        UserID = TargetUserID;
                        Password = TargetDatabasePassword;
                        DatabaseName = TargetDatabaseName;

                        }
                    catch (Exception ex)
                        {
                        SuccessBulitNew = false;
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    finally
                        {
                        MyConnection.Close();
                        }

                    }
                else
                    {
                    PasswordIsCorrect = false;
                    MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com" +
                        "                                                                版权所有： 彭东南",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                    }

                }
            catch (Exception ex)
                {
                SuccessBulitNew = false;
                MessageBox.Show("创建类的实例时出现错误！\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
                }
            
            }

        //执行MySQL数据库的SQL指令
        /// <summary>
        /// 执行MySQL数据库的SQL指令
        /// </summary>
        /// <param name="SQLCommand">SQL指令</param>
        /// <returns></returns>
        public object ExecuteSQL(string SQLCommand) 
            {

            //如果在实例化时失败，则直接退出函数
            if (SuccessBulitNew == false | PasswordIsCorrect==false)
                {
                MessageBox.Show("Failed to initialize this MySQL Class, please check the reason and retry.\r\n" +
                    "此MySQL类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            try
                {

                DataSet MyDataSet = new DataSet();
                MySqlDataAdapter Adapter = new MySqlDataAdapter(SQLCommand, MyConnection);
                lock(Adapter)
                    {
                    MyCommand.Connection = MyConnection;
                    MyCommand.Connection.Open();
                    MyCommand.CommandText = SQLCommand;
                    MyCommand.ExecuteNonQuery();
                    if (SQLCommand.ToLower().StartsWith("s"))
                        {
                        Adapter.Fill(MyDataSet, Strings.Split(SQLCommand, Strings.Space(1)).Last());
                        return MyDataSet.Tables;
                        }
                    MyConnection.Close();
                    return true;
                    }

                }
            catch (Exception ex)
                {
                MyConnection.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }
            
            }

        //读取数据库表中所有的记录
        /// <summary>
        /// 读取数据库表中所有的记录
        /// </summary>
        /// <param name="TableName">数据库表名称</param>
        /// <returns>返回数据表中所有记录的DataTable</returns>
        public DataTable ReadTable(string TableName)
            {

            //如果在实例化时失败，则直接退出函数
            if (SuccessBulitNew == false)
                {
                MessageBox.Show("Failed to initialize this MySQL Class, please check the reason and retry.\r\n" +
                    "此MySQL类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
                }

            DataTable tmpTable = new DataTable();
            tmpTable = null;

            //这样是为了防止未成功连接服务器导致返回false，而此类型为Data.DataTable，从而导致类型不匹配出错，
            try
                {
                tmpTable = (DataTable)ExecuteSQL("SELECT   " + TableName + ".*  FROM  " + TableName);
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
                }

            if (tmpTable != null)
                {
                tmpTable.TableName = TableName;
                return tmpTable;
                }
            else 
                {
                return null;
                }

            }

        //删除数据库中的数据表
        /// <summary>
        /// 删除数据库中的数据表
        /// </summary>
        /// <param name="TableName">数据表名称</param>
        /// <returns></returns>
        public bool DelTable(string TableName)
            {

            //如果在实例化时失败，则直接退出函数
            if (SuccessBulitNew == false)
                {
                MessageBox.Show("Failed to initialize this MySQL Class, please check the reason and retry.\r\n" +
                    "此MySQL类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            try
                {

                if (MessageBox.Show(null,"Are you sure to delete the whole table " +
                    TableName + "? If so, the data will be lost.","Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                    if ((bool)ExecuteSQL("Drop Table " + TableName))
                        {
                        return true;
                        }
                    else
                        {
                        return false;
                        }
                    }
                else 
                    {
                    return false;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            }

        //删除数据库
        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="DatabaseName">需要删除的数据库名称</param>
        /// <returns></returns>
        public bool DelDatabase(string DatabaseName)
            {

            //如果在实例化时失败，则直接退出函数
            if (SuccessBulitNew == false)
                {
                MessageBox.Show("Failed to initialize this MySQL Class, please check the reason and retry.\r\n" +
                    "此MySQL类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            try
                {

                if (MessageBox.Show(null, "Are you sure to delete the whole database " + 
                    DatabaseName + "? If so, the data will be lost.", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                    if ((bool)ExecuteSQL("Drop Database " + DatabaseName))
                        {
                        return true;
                        }
                    else
                        {
                        return false;
                        }
                    }
                else
                    {
                    return false;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            }

        //删除数据表中满足特定条件的记录
        /// <summary>
        /// 删除数据表中满足特定条件的记录
        /// </summary>
        /// <param name="TargetTable">目标数据表名称</param>
        /// <param name="TargetRecordBySQL">SQL语言表达的删除条件</param>
        /// <returns></returns>
        public bool DelTargetRecord(string TargetTable, string TargetRecordBySQL)
            {

            //如果在实例化时失败，则直接退出函数
            if (SuccessBulitNew == false)
                {
                MessageBox.Show("Failed to initialize this MySQL Class, please check the reason and retry.\r\n" +
                    "此MySQL类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            try
                {

                if ((bool)ExecuteSQL("DELETE FROM " + TargetTable + " WHERE " + TargetRecordBySQL))
                    {
                    return true;
                    }
                else
                    {
                    return false;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            }

        //删除数据库表中的所有记录
        /// <summary>
        /// 删除数据库表中的所有记录
        /// </summary>
        /// <param name="TableName">数据库表名称</param>
        /// <returns></returns>
        public bool DelAllRecord(string TableName)
            {

            //如果在实例化时失败，则直接退出函数
            if (SuccessBulitNew == false)
                {
                MessageBox.Show("Failed to initialize this MySQL Class, please check the reason and retry.\r\n" +
                    "此MySQL类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            try
                {

                if ((bool)ExecuteSQL("DELETE FROM " + TableName))
                    {
                    return true;
                    }
                else
                    {
                    return false;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            }

        //添加一条数据记录到数据表中
        /// <summary>
        /// 添加一条数据记录到数据表中
        /// </summary>
        /// <param name="TargetTable">目标数据表</param>
        /// <param name="InsertData">需要添加的数据记录的数组</param>
        /// <returns>是否执行成功</returns>
        public bool AddRecord(string TargetTable, object[] InsertData)
            {

            //如果在实例化时失败，则直接退出函数
            if (SuccessBulitNew == false)
                {
                MessageBox.Show("Failed to initialize this MySQL Class, please check the reason and retry.\r\n" +
                    "此MySQL类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            string TempStr = "";

            try
                {

                for (int a = 0; a < InsertData.Length; a++) 
                    {

                    if (InsertData[a].GetType() == typeof(string))
                        {

                        //如果是第一个数据是字符串，则不加，号；否则就要加，号；
                        if (a == 0)
                            {
                            TempStr = TempStr + "'" + InsertData[a] + "'";
                            }
                        else
                            {
                            TempStr = TempStr + ",'" + InsertData[a] + "'";
                            }

                        }
                    else
                        {
                        if (a == 0)
                            {
                            TempStr = (string)InsertData[a];
                            }
                        else
                            {
                            TempStr = TempStr + "," + InsertData[a];
                            }
                        }

                    }

                if ((bool)ExecuteSQL("INSERT INTO " + TargetTable + " VALUES( "
                    + TempStr + ") ") == true)
                    {
                    return true;
                    }
                else
                    {
                    return false;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            }

        //更新单条记录到数据表中
        /// <summary>
        /// 更新单条记录到数据表中
        /// </summary>
        /// <param name="TargetTable">目标数据表</param>
        /// <param name="NameOfTargetRow">数组：字段名称</param>
        /// <param name="NewDataForTargetRow">数组：字段的新数据值</param>
        /// <param name="SQLConditionForTargetRow">某个记录的SQL条件表达式</param>
        /// <returns></returns>
        public bool UpdateRecord(string TargetTable, string[] NameOfTargetRow,
            object[] NewDataForTargetRow, string SQLConditionForTargetRow)
            {

            //如果在实例化时失败，则直接退出函数
            if (SuccessBulitNew == false)
                {
                MessageBox.Show("Failed to initialize this MySQL Class, please check the reason and retry.\r\n" +
                    "此MySQL类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            string TempStr = "";

            if (NameOfTargetRow.Length != NewDataForTargetRow.Length)
                {
                MessageBox.Show("The count of new data is not the same as the count of columns, please correct them and retry.\r\n" +
                    "参数中数据表的字段数量与需要更新新数据的数据数量不一致，请纠正参数后重试。",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;

                }

            try
                {

                for (int a = 0; a < NewDataForTargetRow.Length; a++)
                    {
                    if (NewDataForTargetRow[a].GetType() == typeof(string))
                        {
                        //如果是第一个数据是字符串，则不加，号；否则就要加，号；
                        if (a == 0)
                            {
                            TempStr = NameOfTargetRow[a] + "=" + "'" + NewDataForTargetRow[a] + "'";
                            }
                        else
                            {
                            TempStr = TempStr + ", " + NameOfTargetRow[a] + "=" +
                                "'" + NewDataForTargetRow[a] + "'";
                            }

                        }
                    else
                        {

                        if (a == 0)
                            {
                            TempStr = NameOfTargetRow[a] + "=" + NewDataForTargetRow[a];
                            }
                        else
                            {
                            TempStr = TempStr + ", " + NameOfTargetRow[a] + "=" + NewDataForTargetRow[a];
                            }

                        }

                    }

                if ((bool)ExecuteSQL("Update " + TargetTable + " set " + TempStr +
                    " where " + SQLConditionForTargetRow) == true)
                    {
                    return true;
                    }
                else
                    {
                    return false;
                    }

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            }

        //将DataGridView控件中的数据保存到数据库对应的表中
        /// <summary>
        /// 将DataGridView控件中的数据保存到数据库对应的表中
        /// </summary>
        /// <param name="TargetTable"></param>
        /// <param name="TargetDatagridView"></param>
        /// <returns></returns>
        public bool Save(string TargetTable, DataGridView TargetDatagridView)
            {

            //如果在实例化时失败，则直接退出函数
            if (SuccessBulitNew == false)
                {
                MessageBox.Show("Failed to initialize this MySQL Class, please check the reason and retry.\r\n" +
                    "此MySQL类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            DataTable TempTable = new DataTable();
            string CommandStr = "";
            DataSet MyDataSet = new DataSet();

            try
                {

                //*********************************
                //1、打开数据库连接，在处理后再关闭，不要重复执行ExecuteSQL【因为它是开一次就关一次，没有必要且浪费时间】
                //2、然后获取数据库的记录，和传递进来的TargetDatagridView进行比较，看列名称是否一致，否则就提醒并退出；
                //3、最好是在编辑DataGridView的事件中添加状态数组，用来保存标志看某一行是否已经变更了，如果没有变更则在保存的时候就可以跳过；
                //4、还要解决一个问题：如果来界定WHERE的是对应的行；
                //*********************************

                //将参数TargetDatagridView传递给TempTable
                TempTable = (DataTable)TargetDatagridView.DataSource;

                //连接数据库
                //MyConnection = new OleDbConnection(ConnectStr);
                //MyCommand = new OleDbCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.Connection.Open();

                //更新参数:把TargetDatagridView中每行的每个数据存放到数据表中对应的位置
                for (int j = 0; j < TargetDatagridView.Rows.Count; j++)
                    {

                    for (int a = 0; a < TargetDatagridView.Columns.Count; a++)
                        {
                        if (a == 0)
                            {
                            CommandStr = "Update " + TargetTable + " set " +
                                TargetDatagridView.Columns[a].HeaderText +
                                " = " + TargetDatagridView.Rows[j].Cells[a].Value;
                            }
                        else
                            {
                            CommandStr = CommandStr + "," + TargetDatagridView.Columns[a].HeaderText +
                                " = " + TargetDatagridView.Rows[j].Cells[a].Value;
                            }
                        }

                    MyCommand.CommandText = CommandStr;
                    MyCommand.ExecuteNonQuery();

                    }
                return true;

                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }
            finally 
                {
                MyCommand.Connection.Close();
                }

            }

        #endregion

        }//class

    }//namespace