#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using Microsoft.VisualBasic;

#endregion

namespace PengDongNanTools
    {

    //微软Access数据库操作
    /// <summary>
    /// 微软Access数据库操作
    /// </summary>
    class AccessDBOperation
        {

        #region "变量定义"

        private string ConnectStr,Password;//, ServerName, UserID, 
            //, NameOfDatabase, DatabaseName;
        private bool SuccessBuiltNew = false;
        private bool PasswordIsCorrect = false;

        private OleDbConnection MyConnection;
        private OleDbCommand MyCommand;

        /// <summary>
        /// 用于操作计算机组件【文件系统、音频、时钟、键盘等】
        /// </summary>
        public Microsoft.VisualBasic.Devices.Computer MyPC=new Microsoft.VisualBasic.Devices.Computer();

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }

        #endregion
        
        #region "代码"

        //创建Microsoft Access数据库连接类的实例
        /// <summary>
        /// 创建Microsoft Access数据库连接类的实例
        /// </summary>
        /// <param name="FullAccessFileName">Access数据库文件全名【含完整路径】</param>
        /// <param name="TargetDatabasePassword">数据库密码</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public AccessDBOperation(string FullAccessFileName, 
            string TargetDatabasePassword, string DLLPassword)
            {

            try 
                {
                if (MyPC.FileSystem.FileExists(FullAccessFileName)==false) 
                    {
                    MessageBox.Show("The Access database file \'" + FullAccessFileName + "\' doesn't exist, please check the get the correct full file name include the path.\r\nAccess数据库文件不存在，请检查文件全名及完整路径是否正确.",
                        "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                    }

                 SuccessBuiltNew = false;
                 PasswordIsCorrect = false;

                 if (DLLPassword == "ThomasPeng" | DLLPassword == "pengdongnan"
                     | DLLPassword == "彭东南")
                     {
                     PasswordIsCorrect = true;
                     System.IO.FileInfo TempFileInfo = MyPC.FileSystem.GetFileInfo(FullAccessFileName);

                     if (TempFileInfo.Extension == ".mdb")
                         {
                         ConnectStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FullAccessFileName +
                         ";Jet OLEDB:Database Password=" + TargetDatabasePassword + ";Persist Security Info=false"; //【报错】Integrated Security=SSPI;
                         }

                     if (TempFileInfo.Extension == ".accdb")
                         {
                         ConnectStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FullAccessFileName +
                        ";Jet OLEDB:Database Password=" + TargetDatabasePassword + ";Persist Security Info=false";//【报错】Integrated Security=SSPI;
                         }

                     try
                         {
                         MyConnection = new OleDbConnection(ConnectStr);
                         MyCommand = new OleDbCommand();
                         MyCommand.Connection = MyConnection;
                         MyCommand.Connection.Open();
                         SuccessBuiltNew = true;
                         Password = TargetDatabasePassword;
                         }
                     catch (Exception ex)
                         {
                         SuccessBuiltNew = false;
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
                         "Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                     return;
                     }
                
                }
            catch(Exception ex)
                {
                SuccessBuiltNew = false;
                MessageBox.Show("创建类的实例时出现错误！\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;                
                }

             }

        //创建Microsoft Access数据库连接类的实例【弹出对话框选择需要操作的文件】
        /// <summary>
        /// 创建Microsoft Access数据库连接类的实例【弹出对话框选择需要操作的文件】
        /// </summary>
        /// <param name="TargetDatabasePassword">数据库密码</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public AccessDBOperation(string TargetDatabasePassword, string DLLPassword)
            {

            //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\数据库1.accdb;Jet OLEDB:Database Password=888888
            //"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\Data.mdb;Jet OLEDB:Database Password=888888;Integrated Security=SSPI;Persist Security Info=false"
            
            try
                {
     
                SuccessBuiltNew = false;
                PasswordIsCorrect = false;

                if (DLLPassword == "ThomasPeng" | DLLPassword == "pengdongnan"
                    | DLLPassword == "彭东南")
                    {
                    PasswordIsCorrect = true;

                    OpenFileDialog TempOpenFile = new OpenFileDialog();
                    TempOpenFile.CheckFileExists = true;
                    TempOpenFile.DefaultExt = "mdb";
                    TempOpenFile.FilterIndex = 0;
                    TempOpenFile.Filter = "Access 2002-2003数据库(*.mdb)|*.mdb|Access数据库(*.accdb)|*.accdb";
                    TempOpenFile.Multiselect = false;

                    if (TempOpenFile.ShowDialog() == DialogResult.OK)
                        {
                        System.IO.FileInfo TempFileInfo = MyPC.FileSystem.GetFileInfo(TempOpenFile.FileName);

                        if (TempFileInfo.Extension == ".mdb")
                            {
                            ConnectStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + TempOpenFile.FileName +
                            ";Jet OLEDB:Database Password=" + TargetDatabasePassword + ";Persist Security Info=false"; //【报错】Integrated Security=SSPI;
                            }

                        if (TempFileInfo.Extension == ".accdb")
                            {
                            ConnectStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + TempOpenFile.FileName +
                           ";Jet OLEDB:Database Password=" + TargetDatabasePassword + ";Persist Security Info=false";//【报错】Integrated Security=SSPI;
                            }
                        }
                    else
                        {
                        return;
                        }                                      

                    try
                        {
                        MyConnection = new OleDbConnection(ConnectStr);
                        MyCommand = new OleDbCommand();
                        MyCommand.Connection = MyConnection;
                        MyCommand.Connection.Open();
                        SuccessBuiltNew = true;
                        Password = TargetDatabasePassword;
                        }
                    catch (Exception ex)
                        {
                        SuccessBuiltNew = false;
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
                SuccessBuiltNew = false;
                MessageBox.Show("创建类的实例时出现错误！\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
                }

            }

        //执行数据库的SQL指令
        /// <summary>
        /// 执行数据库的SQL指令
        /// </summary>
        /// <param name="SQLCommand">SQL指令</param>
        /// <returns></returns>
        public object ExecuteSQL(string SQLCommand) 
            {
                //如果在实例化时失败，则直接退出函数
            if (SuccessBuiltNew == false | PasswordIsCorrect==false) 
                    {
                    MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                        "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                    }

                DataSet MyDataSet = new DataSet();
                //string TableName = "";

                MyConnection = new OleDbConnection(ConnectStr);
                MyCommand = new OleDbCommand();

                try
                    {
                    //object TestLock = new object() ;
                    //lock(TestLock)
                    //    {
                    //    } 
                    OleDbDataAdapter Adapter = new OleDbDataAdapter(SQLCommand, MyConnection);
                    lock (Adapter)
                        {
                        MyCommand.Connection = MyConnection;
                        MyCommand.Connection.Open();
                        MyCommand.CommandText = SQLCommand;
                        MyCommand.ExecuteNonQuery();
                        if (SQLCommand.ToLower().StartsWith("s"))
                            {
                            Adapter.Fill(MyDataSet, Strings.Split(SQLCommand, Strings.Space(1)).Last());
                            return MyDataSet.Tables;
                            //DataTableCollection TempTables;// = new DataTableCollection();
                            //TempTables = MyDataSet.Tables;//Strings.Split(SQLCommand,Strings.Space(1)).Last);
                            //return TempTables;
                            }
                        MyConnection.Close();
                        return true;
                        }
                    }
                catch (Exception ex)
                    {
                    MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return null;
                    }
                finally 
                    {
                    MyConnection.Close();
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
            if (SuccessBuiltNew == false)
                {
                MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
                }

            try
                {
                DataTable TempTable=new DataTable();
                TempTable = null;
                TempTable =(DataTable) ExecuteSQL("SELECT   " + TableName + ".*  FROM  " + TableName);
                return TempTable;
                }
            catch (Exception ex)
                {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (SuccessBuiltNew == false)
                {
                MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            try
                {
                if (MessageBox.Show("Are you sure to delete the whole table " +
                    TableName + "? If so, the data will be lost.\r\n" +
                    "确定要删除表格" + TableName + "?") == DialogResult.Yes)
                    {
                    bool TempResult = false;
                    TempResult = (bool)ExecuteSQL("Drop Table " + TableName);
                    if (TempResult == true)
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
            if (SuccessBuiltNew == false)
                {
                MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            try
                {

                if (MessageBox.Show("Are you sure to delete the whole database " +
                    DatabaseName + "? If so, the data will be lost.\r\n" +
                    "确定要删除数据库： " + DatabaseName + "?") == DialogResult.Yes)
                    {
                    bool TempResult = false;
                    TempResult = (bool)ExecuteSQL("Drop Database " + DatabaseName);
                    if (TempResult == true)
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
            if (SuccessBuiltNew == false)
                {
                MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            try
                {

                if ((bool)ExecuteSQL("DELETE FROM " + TargetTable + " WHERE " + TargetRecordBySQL) == true)
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
            if (SuccessBuiltNew == false)
                {
                MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            try
                {

                if ((bool)ExecuteSQL("DELETE FROM " + TableName)==true)
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
        /// <returns></returns>
        public bool AddRecord(string TargetTable, object[] InsertData) 
            {

            //如果在实例化时失败，则直接退出函数
            if (SuccessBuiltNew == false)
                {
                MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            try
                {

                string TempStr = "";
                for (int a = 0; a < InsertData.Length; a++) 
                    {
                    //System.Type TempType;
                    ////TempType=typeof(InsertData[a]);
                    //TempType=InsertData[a].GetType();

                    //if (TempType==typeof(string)) 
                    //    {
                        
                    //    }

                    //if (TempType.ToString() ==System.TypeCode.String.ToString())
                    //    {
                    //    }

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
                            TempStr =(string) InsertData[a];
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
        public bool UpdateSingleRecord(string TargetTable, string[] NameOfTargetRow,
            object[] NewDataForTargetRow, string SQLConditionForTargetRow) 
            {

            //如果在实例化时失败，则直接退出函数
            if (SuccessBuiltNew == false)
                {
                MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
                }

            try
                {

                string TempStr = "";

                if(NameOfTargetRow.Length != NewDataForTargetRow.Length)
                    {
                    MessageBox.Show("The count of new data is not the same as the count of columns, please correct them and retry.\r\n" +
                        "参数中数据表的字段数量与需要更新新数据的数据数量不一致，请纠正参数后重试。",
                        "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return false;
                    
                    }

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

                if((bool)ExecuteSQL("Update " + TargetTable + " set " + TempStr + 
                    " where " + SQLConditionForTargetRow)==true)
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
        /// <param name="TargetTable">目标数据表</param>
        /// <param name="TargetDatagridView">目标DatagridView</param>
        /// <returns></returns>
        private bool Save(string TargetTable, DataGridView TargetDatagridView) 
            {

            //如果在实例化时失败，则直接退出函数
            if (SuccessBuiltNew == false)
                {
                MessageBox.Show("Failed to initialize this  Class, please check the reason and retry.\r\n" +
                    "此类实例化失败，请检查原因后再尝试.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
               // MyConnection.Close();
                }
            
            }
        
        #endregion
        
        }//class

    }//namespace