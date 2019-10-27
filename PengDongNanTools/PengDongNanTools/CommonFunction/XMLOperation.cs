#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.VisualBasic;
using System.IO;

#endregion

#region "待处理事项"

//1、考虑不要频繁load/save XML文件操作，比如传入ref XMLDocument，只管对它进行操作，就可以省去load/save XML文件操作的时间；【】
//2、考虑增加代码来处理删除记录；【】

#endregion

namespace PengDongNanTools
    {

    //XML文件操作
    /// <summary>
    /// XML文件操作【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    class XMLOperation
        {

        #region "变量定义"

        private string ErrorMessage = "", XMLFileName = "";
        private bool SuccessBuiltNew = false, PasswordIsCorrect = false;

        private CommonFunction FC = new CommonFunction("彭东南");

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }

        #endregion

        #region "函数代码"

        // XML文件操作的实例化函数
        /// <summary>
        /// XML文件操作的实例化函数
        /// </summary>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public XMLOperation(string DLLPassword)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;
            try
                {
                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan") || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;
                    SuccessBuiltNew = true;
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
                MessageBox.Show("创建类的实例时出现错误！\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }
            }

        // XML文件操作的实例化函数
        /// <summary>
        /// XML文件操作的实例化函数
        /// </summary>
        /// <param name="TargetXMLFileName">XML文件名称</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public XMLOperation(string TargetXMLFileName, string DLLPassword)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;
            try
                {
                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan") || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;

                    if (TargetXMLFileName == "")
                        {
                        //MessageBox.Show("The parameter 'TargetXMLFileName' can't be empty, please revise it.","Error");
                        ErrorMessage = "The parameter 'TargetXMLFileName' can't be empty, please revise it.";
                        return;
                        }

                    if (TargetXMLFileName.ToUpper().IndexOf(".XML") == -1)
                        {
                        TargetXMLFileName = TargetXMLFileName + ".xml";
                        }

                    //判断文件是否存在
                    if (System.IO.File.Exists(TargetXMLFileName) == false)
                        {
                        //MessageBox.Show("The target XML file " + TargetXMLFileName +
                        //     " is not exist, please check.", "Error");
                        ErrorMessage = "The target XML file " + TargetXMLFileName +
                             " is not exist, please check.";
                        return;
                        }

                    XMLFileName = TargetXMLFileName;

                    SuccessBuiltNew = true;
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
                MessageBox.Show("创建类的实例时出现错误！\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }
            }

        //清除原有数据文件中的数据记录，并备份原有数据文件
        /// <summary>
        /// 清除原有数据文件中的数据记录，并备份原有数据文件
        /// </summary>
        /// <param name="TargetXMLFileName">XML文件名称</param>
        /// <param name="BackupOriginalFile">在删除原有记录前是否需要备份原有XML文件</param>
        /// <returns>是否执行成功</returns>
        public bool ClearDataRecordsInXMLFile(string TargetXMLFileName,
            bool BackupOriginalFile = true)
            {
            try
                {

                if (TargetXMLFileName == "")
                    {
                    //MessageBox.Show("The parameter 'TargetXMLFileName' can't be empty, please revise it.","Error");
                    ErrorMessage = "The parameter 'TargetXMLFileName' can't be empty, please revise it.";
                    return false;
                    }

                if (TargetXMLFileName.ToUpper().IndexOf(".XML") == -1)
                    {
                    TargetXMLFileName = TargetXMLFileName + ".xml";
                    }

                //判断文件是否存在
                if (System.IO.File.Exists(TargetXMLFileName) == false)
                    {
                    //MessageBox.Show("The target XML file " + TargetXMLFileName +
                    //     " is not exist, please check.", "Error");
                    ErrorMessage = "The target XML file " + TargetXMLFileName +
                         " is not exist, please check.";
                    return false;
                    }

                //备份原有数据文件
                if (BackupOriginalFile == true)
                    {
                    string TempNewFileName = TargetXMLFileName + " 备份"
                        + Strings.Format(DateAndTime.Now,
                        "yyyy'年'MM'月'dd'日'HH'点'mm'分'ss'秒'")
                        + ".XML";
                    File.Copy(TargetXMLFileName, TempNewFileName);
                    ErrorMessage = "已备份原有数据文件至：" + TempNewFileName;
                    }

                XmlDocument TempXMLDoc = new XmlDocument();
                XmlNode NodeToBeRemoved;
                XmlNodeList NodeListToBeRemoved;

                TempXMLDoc.Load(TargetXMLFileName);
                NodeListToBeRemoved = TempXMLDoc.ChildNodes;
                
                //移除原有数据文件中指定节点的所有数据记录
                //默认根目录为NodeListToBeRemoved.Item(0)
                //故在这里跳过0，然后直接删除后面的节点
                for (int a = 1; a < NodeListToBeRemoved.Count; a++)
                    {
                    NodeListToBeRemoved.Item(a).RemoveAll();
                    }

                //**********************
                //旧的思路
                //for (int a = 0; a < NodeListToBeRemoved.Count; a++)
                //    {
                //    //跳过XML节点
                //    if (NodeListToBeRemoved.Item(a).LocalName.ToUpper() == "XML") 
                //        {
                //        continue;
                //        }
                //    NodeListToBeRemoved.Item(a).RemoveAll();
                //    }
                //**********************

                TempXMLDoc.Save(TargetXMLFileName);

                TempXMLDoc = null;
                NodeToBeRemoved = null;
                NodeListToBeRemoved = null;

                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //清除原有数据文件中的数据记录，并备份原有数据文件
        /// <summary>
        /// 清除原有数据文件中的数据记录，并备份原有数据文件
        /// </summary>
        /// <param name="BackupOriginalFile">在删除原有记录前是否需要备份原有XML文件</param>
        /// <returns>是否执行成功</returns>
        public bool ClearDataRecordsInXMLFile(bool BackupOriginalFile = true)
            {
            try
                {

                if (XMLFileName == "")
                    {
                    //MessageBox.Show("The parameter 'XMLFileName' can't be empty, please revise it.","Error");
                    ErrorMessage = "The parameter 'XMLFileName' can't be empty, please revise it.";
                    return false;
                    }

                if (XMLFileName.ToUpper().IndexOf(".XML") == -1)
                    {
                    XMLFileName = XMLFileName + ".xml";
                    }

                //判断文件是否存在
                if (System.IO.File.Exists(XMLFileName) == false)
                    {
                    //MessageBox.Show("The target XML file " + XMLFileName +
                    //     " is not exist, please check.", "Error");
                    ErrorMessage = "The target XML file " + XMLFileName +
                         " is not exist, please check.";
                    return false;
                    }

                //备份原有数据文件
                if (BackupOriginalFile == true)
                    {
                    string TempNewFileName = XMLFileName + " 备份"
                        + Strings.Format(DateAndTime.Now,
                        "yyyy'年'MM'月'dd'日'HH'点'mm'分'ss'秒'")
                        + ".XML";
                    File.Copy(XMLFileName, TempNewFileName);
                    ErrorMessage = "已备份原有数据文件至：" + TempNewFileName;
                    }

                XmlDocument TempXMLDoc = new XmlDocument();
                XmlNode NodeToBeRemoved;
                XmlNodeList NodeListToBeRemoved;

                TempXMLDoc.Load(XMLFileName);
                NodeListToBeRemoved = TempXMLDoc.ChildNodes;
                
                //移除原有数据文件中指定节点的所有数据记录
                //默认根目录为NodeListToBeRemoved.Item(0)
                //故在这里跳过0，然后直接删除后面的节点
                for (int a = 1; a < NodeListToBeRemoved.Count; a++)
                    {
                    NodeListToBeRemoved.Item(a).RemoveAll();
                    }

                //**********************
                //旧的思路
                //for (int a = 0; a < NodeListToBeRemoved.Count; a++)
                //    {
                //    //跳过XML节点
                //    if (NodeListToBeRemoved.Item(a).LocalName.ToUpper() == "XML") 
                //        {
                //        continue;
                //        }
                //    NodeListToBeRemoved.Item(a).RemoveAll();
                //    }
                //**********************

                TempXMLDoc.Save(XMLFileName);

                TempXMLDoc = null;
                NodeToBeRemoved = null;
                NodeListToBeRemoved = null;

                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //清除原有数据文件中某个名称的节点数据记录，并备份原有数据文件
        /// <summary>
        /// 清除原有数据文件中某个名称的节点数据记录，并备份原有数据文件
        /// </summary>
        /// <param name="TargetXMLFileName">XML文件名称</param>
        /// <param name="NodeNameToBeDeleted">XML文件中第一层子节点的名称</param>
        /// <param name="BackupOriginalFile">在删除原有记录前是否需要备份原有XML文件</param>
        /// <returns>是否执行成功</returns>
        public bool DelDataRecordsInXMLFile(string TargetXMLFileName,
            string NodeNameToBeDeleted, bool BackupOriginalFile = true) 
            {
            try
                {
                if (TargetXMLFileName == "")
                    {
                    //MessageBox.Show("The parameter 'TargetXMLFileName' can't be empty, please revise it.");
                    ErrorMessage = "The parameter 'TargetXMLFileName' can't be empty, please revise it.";
                    return false;
                    }

                if (TargetXMLFileName.ToUpper().IndexOf(".XML") == -1)
                    {
                    TargetXMLFileName = TargetXMLFileName + ".xml";
                    }

                //判断文件是否存在
                if (System.IO.File.Exists(TargetXMLFileName) == false) 
                    {
                    //MessageBox.Show("The target XML file " + TargetXMLFileName +
                    //     " is not exist, please check.");
                    ErrorMessage = "The target XML file " + TargetXMLFileName +
                         " is not exist, please check.";
                    return false;
                    }

                XmlDocument TempXMLDoc = new XmlDocument();
                XmlNode NodeToBeRemoved;
                XmlNodeList NodeListToBeRemoved;

                TempXMLDoc.Load(TargetXMLFileName);
                NodeListToBeRemoved = TempXMLDoc.GetElementsByTagName(NodeNameToBeDeleted);
                
                //判断需要删除的节点是否存在
                if (NodeListToBeRemoved.Count == 0) 
                    {
                    //MessageBox.Show("The target XML file " + TargetXMLFileName +
                    //    " does not include the node : " + NodeNameToBeDeleted 
                    //    + ", please check.");
                    ErrorMessage = "The target XML file " + TargetXMLFileName +
                        " does not include the node : " + NodeNameToBeDeleted
                        + ", please check.";
                    return false;
                    }

                //备份原有数据文件
                if (BackupOriginalFile == true) 
                    {
                    string TempNewFileName= TargetXMLFileName + " 备份" 
                        + Strings.Format(DateAndTime.Now, 
                        "yyyy'年'MM'月'dd'日'HH'点'mm'分'ss'秒'") + ".XML";
                    File.Copy(TargetXMLFileName, TempNewFileName);
                    ErrorMessage="已备份原有数据文件至：" + TempNewFileName;
                    }
                //int CountTobeRemoved = NodeListToBeRemoved.Count;
                ////移除原有数据文件中指定节点的所有数据记录
                for (int a = NodeListToBeRemoved.Count - 1; a >= 0; a--)//for (int a = 0; a < NodeListToBeRemoved.Count; a++)
                    {
                    NodeToBeRemoved = NodeListToBeRemoved.Item(a).ParentNode;
                    NodeToBeRemoved.RemoveChild(NodeListToBeRemoved.Item(a));

                    //以下指令是int a=0;a<NodeListToBeRemoved.Count或者CountTobeRemoved，
                    //但是实际需要删除的节点数量是递减的，就会出现最后有一个无法删除,
                    //改为上面的新方法后就可以了
                    //下面这条指令只是清除此节点下面所有的节点，但是不包括节点本身
                    //NodeListToBeRemoved.Item(a).RemoveAll();

                    //以下不起正确作用
                    //NodeToBeRemoved = TempXMLDoc.DocumentElement.SelectSingleNode(NodeNameToBeDeleted);
                    //TempXMLDoc.DocumentElement.RemoveChild(NodeToBeRemoved);
                    //TempXMLDoc.DocumentElement.RemoveChild(NodeListToBeRemoved.Item(a));//要移除的节点不是此节点的子级。
                    }

                TempXMLDoc.Save(TargetXMLFileName);

                TempXMLDoc = null;
                NodeToBeRemoved = null;
                NodeListToBeRemoved = null;
                
                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //清除原有数据文件中某个名称的节点数据记录，并备份原有数据文件
        /// <summary>
        /// 清除原有数据文件中某个名称的节点数据记录，并备份原有数据文件
        /// </summary>
        /// <param name="NodeNameToBeDeleted">XML文件中第一层子节点的名称</param>
        /// <param name="BackupOriginalFile">在删除原有记录前是否需要备份原有XML文件</param>
        /// <returns>是否执行成功</returns>
        public bool DelDataRecordsInXMLFile(string NodeNameToBeDeleted, bool BackupOriginalFile = true) 
            {
            try
                {
                if (XMLFileName == "")
                    {
                    //MessageBox.Show("The parameter 'XMLFileName' can't be empty, please revise it.");
                    ErrorMessage = "The parameter 'XMLFileName' can't be empty, please revise it.";
                    return false;
                    }

                if (XMLFileName.ToUpper().IndexOf(".XML") == -1)
                    {
                    XMLFileName = XMLFileName + ".xml";
                    }

                //判断文件是否存在
                if (System.IO.File.Exists(XMLFileName) == false) 
                    {
                    //MessageBox.Show("The target XML file " + XMLFileName +
                    //     " is not exist, please check.");
                    ErrorMessage = "The target XML file " + XMLFileName +
                         " is not exist, please check.";
                    return false;
                    }

                XmlDocument TempXMLDoc = new XmlDocument();
                XmlNode NodeToBeRemoved;
                XmlNodeList NodeListToBeRemoved;

                TempXMLDoc.Load(XMLFileName);
                NodeListToBeRemoved = TempXMLDoc.GetElementsByTagName(NodeNameToBeDeleted);
                
                //判断需要删除的节点是否存在
                if (NodeListToBeRemoved.Count == 0) 
                    {
                    //MessageBox.Show("The target XML file " + TargetXMLFileName +
                    //    " does not include the node : " + NodeNameToBeDeleted 
                    //    + ", please check.");
                    ErrorMessage = "The target XML file " + XMLFileName +
                        " does not include the node : " + NodeNameToBeDeleted
                        + ", please check.";
                    return false;
                    }

                //备份原有数据文件
                if (BackupOriginalFile == true) 
                    {
                    string TempNewFileName = XMLFileName + " 备份" 
                        + Strings.Format(DateAndTime.Now, 
                        "yyyy'年'MM'月'dd'日'HH'点'mm'分'ss'秒'") + ".XML";
                    File.Copy(XMLFileName, TempNewFileName);
                    ErrorMessage="已备份原有数据文件至：" + TempNewFileName;
                    }
                //int CountTobeRemoved = NodeListToBeRemoved.Count;
                ////移除原有数据文件中指定节点的所有数据记录
                for (int a = NodeListToBeRemoved.Count - 1; a >= 0; a--)//for (int a = 0; a < NodeListToBeRemoved.Count; a++)
                    {
                    NodeToBeRemoved = NodeListToBeRemoved.Item(a).ParentNode;
                    NodeToBeRemoved.RemoveChild(NodeListToBeRemoved.Item(a));

                    //以下指令是int a=0;a<NodeListToBeRemoved.Count或者CountTobeRemoved，
                    //但是实际需要删除的节点数量是递减的，就会出现最后有一个无法删除,
                    //改为上面的新方法后就可以了
                    //下面这条指令只是清除此节点下面所有的节点，但是不包括节点本身
                    //NodeListToBeRemoved.Item(a).RemoveAll();

                    //以下不起正确作用
                    //NodeToBeRemoved = TempXMLDoc.DocumentElement.SelectSingleNode(NodeNameToBeDeleted);
                    //TempXMLDoc.DocumentElement.RemoveChild(NodeToBeRemoved);
                    //TempXMLDoc.DocumentElement.RemoveChild(NodeListToBeRemoved.Item(a));//要移除的节点不是此节点的子级。
                    }

                TempXMLDoc.Save(XMLFileName);

                TempXMLDoc = null;
                NodeToBeRemoved = null;
                NodeListToBeRemoved = null;
                                
                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //【目前只支持两层节点】将特定的节点和对应的数据保存至XML文件中
        /// <summary>
        /// 【目前只支持两层节点】将特定的节点和对应的数据保存至XML文件中,如果节点不存在就创建
        /// 节点之间的关系是并列，即数组的[编号],
        /// 但是下面有节点'：'隔开，例如：str[1]="xx:";,
        /// 后面有并列子节点的用'?'号隔开，例如：str[1]="xx:rrr?sss?ttt?yyy";,
        /// 第一个*表示第二级子节点，第二个*表示第三级子节点
        /// </summary>
        /// <param name="TargetXMLFileName">保存数据的XML文件</param>
        /// <param name="NameOfRootNode">保存数据的XML文件根节点名称</param>
        /// <param name="XMLNodes">节点从第一个[0]开始，下面有节点的用'：'隔开，后面并列子节点数据用'?'号隔开
        /// 例如："ddd?kkk?f111:nnn?xxx?yyy*rrr?ccc*mmm?jj"
        ///   <ddd>ddd</ddd>
        ///   <kkk>kkk</kkk>
        ///   <f111>
        ///         <nnn>nnn</nnn>
        ///         <xxx>xxx</xxx>
        ///         <yyy>
        ///             <rrr>rrr</rrr>
        ///             <ccc>
        ///                 <mmm>mmm</mmm>
        ///                 <jj>jj</jj>
        ///             </ccc>
        ///         </yyy>
        ///</f111>
        /// </param>
        /// <param name="Data">对应节点的数据，结构要相同，然后用：隔开，不足部分取值""</param>
        /// <returns></returns>
        public bool SaveXMLFile(string TargetXMLFileName, string NameOfRootNode,string[] XMLNodes,
            string[] Data)//, string[] Comments) /// <param name="Comments">对应节点的备注，结构要相同，然后用：隔开，不足部分取值""</param>
            {
            try
                {
                if (TargetXMLFileName == "")
                    {
                    MessageBox.Show("The parameter 'TargetXMLFileName' can't be empty, please revise it.");
                    return false;
                    }
                //可以考虑如果数据个数小于节点个数，可以赋值""
                if (XMLNodes.Length != Data.Length)
                    {
                    MessageBox.Show("The length of parameter array 'XMLNodes' and 'Data' is not the same, please revise it.");
                    return false;
                    }

                if (TargetXMLFileName.ToUpper().IndexOf(".XML") == -1)
                    {
                    TargetXMLFileName = TargetXMLFileName + ".xml";
                    }

                //判断文件是否存在
                if (System.IO.File.Exists(TargetXMLFileName) == false)//&& System.IO.File.Exists(TargetXMLFileName + ".xml") == false)
                    {
                    //MessageBox.Show("The target XML file " + TargetXMLFileName +
                    //     " is not exist, please check.");
                    //return false;
                    if (MessageBox.Show("The target XML file " + TargetXMLFileName +
                         " is not exist, do you want to create it?", "提示",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                         == DialogResult.No)
                        {
                        return false;
                        }
                    }

                XmlDocument TempXMLDoc = new XmlDocument();
                XmlElement[] FirstNode = new XmlElement[XMLNodes.Length];
                //XmlElement[] TempSubNodes, Temp2dSubNodes, Temp3rdSubNodes;
                //string FirstNodeName, DataStr;
                string[] TempNodesName, TempData;//, SubNodesName, TempComments;
                string[] Temp2ndNodesName, Temp2ndData;//, Sub2ndNodesName, Temp2ndComments;
                //string[] Sub3rdNodesName, Temp3rdNodesName, Temp3rdData, Temp3rdComments;

                //新建XML文件写的类
                XmlWriter TempXMLWrite;
                XmlWriterSettings TempXmlWriterSettings = new XmlWriterSettings();

                //设置缩进
                TempXmlWriterSettings.Indent = true;

                //设置缩进字符为空格
                TempXmlWriterSettings.IndentChars = "   ";
                //TempXmlWriterSettings.Encoding = System.Text.Encoding.ASCII;
                //TempXmlWriterSettings.Encoding = System.Text.Encoding.UTF8;

                //用GB2312编码保存XML文件
                TempXmlWriterSettings.Encoding = System.Text.Encoding.GetEncoding("GB2312");
                //TempXmlWriterSettings.Encoding = System.Text.Encoding.GetEncoding(936);
                TempXmlWriterSettings.OmitXmlDeclaration = false;

                //XML符合性设置
                TempXmlWriterSettings.ConformanceLevel = System.Xml.ConformanceLevel.Auto;

                //新建XML文件
                TempXMLWrite = XmlWriter.Create(TargetXMLFileName, TempXmlWriterSettings);
                TempXMLWrite.WriteStartDocument();
                TempXMLWrite.WriteStartElement(NameOfRootNode);//"XMLDocument");
                TempXMLWrite.WriteComment("This file was wrote by PengDongNan[彭东南].");


                //-- XMLNodes：节点名称，各个[]之间是并列关系，
                //  下面如果有子节点，先用'：'分开，各个子节点之间用'？'隔开

                //-- Data：节点数据，各个[]之间是并列关系，
                //  下面如果有子节点，先用'：'分开，各个子节点之间用'？'隔开

                for (int a = 0; a < XMLNodes.Length; a++)
                    {
                    //如果有子节点，先用'：'分开，各个子节点之间用'？'隔开
                    //例如：str[1]="xx:rrr?sss?ttt?uuu?yyy?zzz";,
                    //  <xx>
                    //      <rrr>111</rrr>
                    //      <sss>222</sss>
                    //      <ttt>333</ttt>
                    //      <uuu>444<uuu/>
                    //      <yyy>555</yyy>
                    //      <zzz>666</zzz>
                    //</xx>
                    //**************************************
                    //:表示第一级子节点，*表示第二级子节点，#表示第三级子节点
                    //方式一：
                    //例如：str[1]="xx:aaa*vvv#www?rrr?gg?hh?sss?ttt?ee?uuu?yyy?zzz";,


                    //方式二：
                    //例如：str[1]="xx:aaa?vvv?www*rrr?gg#hh?sss?ttt*ee#uuu?yyy?zzz";,
                    //1、先用:进行分割，找出第一级子节点的名称；
                    //2、再用.IndexOf("*") != -1找出有第二级子节点，
                    //   用split计算出有多少个第二级子节点，数组缓存；
                    //3、针对第二级子节点的缓存数组，再用.IndexOf("#") != -1找出有第三级子节点，
                    //   用split计算出有多少个第三级子节点，数组缓存；
                    //4、如果没有第三级子节点，且第二级子节点之前没有?，即没有并列第二级子节点，
                    //   则第二级子节点的名字为*之前的字符串，否则就需要将*之前的?隔开的
                    //   字符串进行分割，取最后那个值为第二级子节点的名字；
                    //5、如果有第三级子节点，且第三级子节点之前没有?，即没有并列第三级子节点，
                    //   则第三级子节点的名字为#之前的字符串，否则就需要将#之前的?隔开的
                    //   字符串进行分割，取最后那个值为第二级子节点的名字；
                    //  <xx>
                    //      <aaa>
                    //      </aaa>
                    //      <vvv>
                    //      </vvv>
                    //      <www>
                    //          <rrr>222</rrr>
                    //          <gg>
                    //              <hh>
                    //              </hh>
                    //              <sss>
                    //              </sss>
                    //              <ttt>
                    //              </ttt>
                    //              <eee>
                    //              </eee>
                    //              <uuu>
                    //              </uuu>
                    //          </gg>
                    
                    
                    //      </www>

                    //<xx>
                    //      <ttt>333</ttt>
                    //      <uuu>444<uuu/>
                    //      <yyy>555</yyy>
                    //      <zzz>666</zzz>
                    //</xx>

                    if (XMLNodes[a].IndexOf(':') != -1 && Data[a].IndexOf(':') != -1)
                        {
                        //如果有子节点，那就要用WriteStartElement开始节点，
                        //用WriteEndElement结束节点，要匹配；
                        TempNodesName = Strings.Split(XMLNodes[a], ":");
                        TempData=Strings.Split(Data[a], ":");

                        //4、如果没有第三级子节点，且第二级子节点之前没有?，即没有并列第二级子节点，
                        //   则第二级子节点的名字为*之前的字符串，否则就需要将*之前的?隔开的
                        //   字符串进行分割，取最后那个值为第二级子节点的名字；
                        if (TempNodesName[0].IndexOf("?") != -1
                            && TempData[0].IndexOf("?") != -1)
                            {
                            string[] Temp1stNodeName,Temp1stData;
                            Temp1stNodeName=Strings.Split(TempNodesName[0],"?");
                            Temp1stData=Strings.Split(TempData[0],"?");
                            //将最后一个？前面的字符串当作第一个子节点的名字
                            //其它只是写值
                            for(int x=0;x<Temp1stNodeName.Length-1;x++)
                                {
                                TempXMLWrite.WriteElementString(Temp1stNodeName[x],Temp1stData[x]);
                                }
                            
                            //将最后一个？前面的字符串当作第一个子节点的名字
                            TempXMLWrite.WriteStartElement(Temp1stNodeName[Temp1stNodeName.Length-1]);
                            //TempXMLWrite.WriteValue(Temp1stData[Temp1stData.Length-1]);
                            
                            }
                        else
                            {
                            TempXMLWrite.WriteStartElement(TempNodesName[0]);
                            //TempXMLWrite.WriteValue(TempData[0]);//如果不屏蔽掉，输出的XML文件是一整行，屏蔽就是各自单独一行；
                            }
                        
                        //查找第二级子节点
                        if (TempNodesName[1].IndexOf("*") != -1 && TempData[1].IndexOf("*") != -1)
                            {
                            //2、再用.IndexOf("*") != -1找出有第二级子节点，
                            //   用split计算出有多少个第二级子节点，数组缓存；
                            Temp2ndNodesName=Strings.Split(TempNodesName[1],"*");
                            Temp2ndData=Strings.Split(TempData[1],"*");

                            int CountOfStar = FC.FindStringInAnotherString(TempNodesName[1], "*");
                            int TempCountOfStar = 0;
                            bool[] FlagOfStar = new bool[CountOfStar];

                            //例如：ss[0]="sss:ccc?fff*ggg?hhh*eee";
                            string[] TempStar =new string[Temp2ndNodesName.Length];
                            string NameForNext2ndNode = "";

                            //4、如果没有第三级子节点，且第二级子节点之前没有?，即没有并列第二级子节点，
                            //   则第二级子节点的名字为*之前的字符串，否则就需要将*之前的?隔开的
                            //   字符串进行分割，取最后那个值为第二级子节点的名字；

                            for(int _2ndNode = 0; _2ndNode < Temp2ndNodesName.Length; _2ndNode++)
                                {
                                if (Temp2ndNodesName[_2ndNode].IndexOf("?") != -1)
                                    {
                                    string[] TempStr = Strings.Split(Temp2ndNodesName[_2ndNode], "?");
                                    string[] TempDataStr = Strings.Split(Temp2ndData[_2ndNode], "?");

                                    //TempXMLWrite.WriteStartElement(TempStr[TempStr.Length - 1]);
                                    //TempXMLWrite.WriteValue(TempDataStr[TempStr.Length - 1]);

                                    for (int u = 0; u < (TempStr.Length) - 1; u++) // 
                                        {
                                        TempXMLWrite.WriteElementString(TempStr[u], TempDataStr[u]);
                                        }

                                    //if (TempCountOfStar < CountOfStar)
                                    //    {
                                    //    for (int u = 0; u < (TempStr.Length) - 1; u++) // 
                                    //        {
                                    //        //TempXMLWrite.WriteStartElement(TempStr[u]);
                                    //        //if (u <= TempDataStr.Length)
                                    //        //{
                                    //        TempXMLWrite.WriteElementString(TempStr[u], TempDataStr[u]);
                                    //        //    }
                                    //        //else 
                                    //        //    {
                                    //        //    TempXMLWrite.WriteElementString(TempStr[u], "");
                                    //        //    }
                                    //        }
                                    //    }
                                    //else
                                    //    {
                                    //    for (int u = 0; u < (TempStr.Length); u++) // 
                                    //        {
                                    //        TempXMLWrite.WriteElementString(TempStr[u], TempDataStr[u]);
                                    //        }
                                    //    }

                                    TempCountOfStar += 1;

                                    if (TempCountOfStar <= CountOfStar)
                                        {
                                        TempXMLWrite.WriteStartElement(TempStr[TempStr.Length - 1]);
                                        }
                                    }
                                else
                                    {
                                    //*之前没有？，就当作下个节点的名字
                                    if (_2ndNode < Temp2ndNodesName.Length - 1)
                                        {
                                        NameForNext2ndNode = Temp2ndNodesName[_2ndNode];
                                        TempXMLWrite.WriteStartElement(NameForNext2ndNode);
                                        }
                                    else 
                                        {
                                        TempXMLWrite.WriteElementString(Temp2ndNodesName[_2ndNode],Temp2ndData[_2ndNode]);
                                        }
                                    }
                                }
                            TempXMLWrite.WriteEndElement();
                            }
                        else 
                            {
                            //例如：ss[0]="sss:fff?eee";
                            //没有第二级子节点，判断有无？分隔符，没有就直接写值，有就进行分割后再写值
                            if (TempNodesName[1].IndexOf("?") != -1 && TempData[1].IndexOf("?") != -1)
                                {
                                string[] Temp1,Temp2;
                                Temp1=Strings.Split(TempNodesName[1],"?");
                                Temp2=Strings.Split(TempData[1],"?");
                                for(int y=0;y<Temp1.Length;y++)
                                    {
                                    if(y<Temp2.Length)
                                        {
                                        TempXMLWrite.WriteElementString(Temp1[y],Temp2[y]);
                                        }
                                    else
                                        {
                                        //数值部分不够的用空值代替
                                        TempXMLWrite.WriteElementString(Temp1[y],"");
                                        }
                                    }
                                }
                            else
                                {
                                TempXMLWrite.WriteElementString(TempNodesName[1],TempData[1]);
                                }
                            }

                        //SubNodesName = Strings.Split(TempNodesName[1], "?");

                        ////添加备注
                        //if (a <= Comments.Length)
                        //    {
                        //    if (Comments[a].IndexOf(':') != -1)
                        //        {
                        //        TempComments = Strings.Split(Comments[a], ":");
                        //        TempXMLWrite.WriteComment(TempComments[0]);
                        //        TempComments = Strings.Split(TempComments[1], "?");
                        //        }
                        //    else
                        //        {
                        //        TempComments=Strings.Split(Comments[a], "?");
                        //        }
                        //    }

                        //用WriteEndElement结束节点，要匹配：定义的子节点；
                        TempXMLWrite.WriteEndElement();
                        //**************************************
                        }
                    else
                        {
                        //如果没有子节点，就直接写进XML文件中
                        //**************************************
                        if (XMLNodes[a].IndexOf('?') != -1 && Data[a].IndexOf('?') != -1)
                            {
                            //如果有子节点，那就要用WriteStartElement开始节点，
                            //用WriteEndElement结束节点，要匹配；
                            TempNodesName = Strings.Split(XMLNodes[a], "?");
                            TempData = Strings.Split(Data[a], "?");
                            for (int r = 0; r < TempData.Length; r++) 
                                {
                                if (TempData.Length >= TempNodesName.Length)
                                    {
                                    TempXMLWrite.WriteElementString(TempNodesName[r], TempData[r]);
                                    }
                                else 
                                    {
                                    TempXMLWrite.WriteElementString(TempNodesName[r], "");
                                    }                                
                                }
                            }
                        else 
                            {
                            TempXMLWrite.WriteElementString(XMLNodes[a], Data[a]);
                            }
                        }
                    }
                TempXMLWrite.WriteEndElement();
                TempXMLWrite.WriteEndDocument();
                TempXMLWrite.Flush();
                TempXMLWrite.Close();                
                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }
  
        /// <summary>
        /// 【目前只支持两层节点】将特定的节点和对应的数据保存至XML文件中,如果节点不存在就创建
        /// 节点之间的关系是并列，即数组的[编号],
        /// 但是下面有节点'：'隔开，例如：str[1]="xx:";,
        /// 后面有并列子节点的用'?'号隔开，例如：str[1]="xx:rrr?sss?ttt?yyy";,
        /// *表示第二级子节点，#表示第三级子节点
        /// </summary>
        /// <param name="TargetXMLFileName">保存数据的XML文件</param>
        /// <param name="XMLNodes">节点从第一个[0]开始，下面有节点的用'：'隔开，后面并列子节点数据用'?'号隔开</param>
        /// <param name="Data">对应节点的数据，结构要相同，然后用：隔开，不足部分取值""</param>
        /// <returns></returns>
        private bool OldSaveXMLFile(string TargetXMLFileName, string[] XMLNodes,
            string[] Data)//, string[] Comments) /// <param name="Comments">对应节点的备注，结构要相同，然后用：隔开，不足部分取值""</param>
            {
            try
                {
                if (TargetXMLFileName == "")
                    {
                    MessageBox.Show("The parameter 'TargetXMLFileName' can't be empty, please revise it.");
                    return false;
                    }
                //可以考虑如果数据个数小于节点个数，可以赋值""
                if (XMLNodes.Length != Data.Length)
                    {
                    MessageBox.Show("The length of parameter array 'XMLNodes' and 'Data' is not the same, please revise it.");
                    return false;
                    }

                //判断文件是否存在
                if (System.IO.File.Exists(TargetXMLFileName) == false
                    && System.IO.File.Exists(TargetXMLFileName + ".xml") == false)
                    {
                    //MessageBox.Show("The target XML file " + TargetXMLFileName +
                    //     " is not exist, please check.");
                    //return false;
                    if (MessageBox.Show("The target XML file " + TargetXMLFileName +
                         " is not exist, do you want to create it?", "提示",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                         == DialogResult.No)
                        {
                        return false;
                        }
                    }

                XmlDocument TempXMLDoc = new XmlDocument();
                XmlElement[] FirstNode = new XmlElement[XMLNodes.Length];
                XmlElement[] TempSubNodes, Temp2dSubNodes, Temp3rdSubNodes;
                string FirstNodeName, DataStr;
                string[] SubNodesName, TempNodesName, TempData, TempComments;
                string[] Sub2ndNodesName, Temp2ndNodesName, Temp2ndData, Temp2ndComments;
                string[] Sub3rdNodesName, Temp3rdNodesName, Temp3rdData, Temp3rdComments;

                if (TargetXMLFileName.ToUpper().IndexOf(".XML") == -1)
                    {
                    TargetXMLFileName = TargetXMLFileName + ".xml";
                    }

                //新建XML文件写的类
                XmlWriter TempXMLWrite;
                XmlWriterSettings TempXmlWriterSettings = new XmlWriterSettings();

                //设置缩进
                TempXmlWriterSettings.Indent = true;

                //设置缩进字符为空格
                TempXmlWriterSettings.IndentChars = "   ";
                //TempXmlWriterSettings.Encoding = System.Text.Encoding.ASCII;
                //TempXmlWriterSettings.Encoding = System.Text.Encoding.UTF8;

                //用GB2312编码保存XML文件
                TempXmlWriterSettings.Encoding = System.Text.Encoding.GetEncoding("GB2312");
                //TempXmlWriterSettings.Encoding = System.Text.Encoding.GetEncoding(936);
                TempXmlWriterSettings.OmitXmlDeclaration = false;

                //XML符合性设置
                TempXmlWriterSettings.ConformanceLevel = System.Xml.ConformanceLevel.Auto;

                //新建XML文件
                TempXMLWrite = XmlWriter.Create(TargetXMLFileName, TempXmlWriterSettings);
                TempXMLWrite.WriteStartDocument();
                TempXMLWrite.WriteStartElement("XMLDocument");
                TempXMLWrite.WriteComment("This file was wrote by PengDongNan[彭东南].");


                //-- XMLNodes：节点名称，各个[]之间是并列关系，
                //  下面如果有子节点，先用'：'分开，各个子节点之间用'？'隔开

                //-- Data：节点数据，各个[]之间是并列关系，
                //  下面如果有子节点，先用'：'分开，各个子节点之间用'？'隔开

                for (int a = 0; a < XMLNodes.Length; a++)
                    {
                    //如果有子节点，先用'：'分开，各个子节点之间用'？'隔开
                    //例如：str[1]="xx:rrr?sss?ttt?uuu?yyy?zzz";,
                    //  <xx>
                    //      <rrr>111</rrr>
                    //      <sss>222</sss>
                    //      <ttt>333</ttt>
                    //      <uuu>444<uuu/>
                    //      <yyy>555</yyy>
                    //      <zzz>666</zzz>
                    //</xx>
                    //**************************************
                    //:表示第一级子节点，*表示第二级子节点，#表示第三级子节点
                    //方式一：
                    //例如：str[1]="xx:aaa*vvv#www?rrr?gg?hh?sss?ttt?ee?uuu?yyy?zzz";,


                    //方式二：
                    //例如：str[1]="xx:aaa?vvv?www*rrr?gg#hh?sss?ttt*ee#uuu?yyy?zzz";,
                    //1、先用:进行分割，找出第一级子节点的名称；
                    //2、再用.IndexOf("*") != -1找出有第二级子节点，
                    //   用split计算出有多少个第二级子节点，数组缓存；
                    //3、针对第二级子节点的缓存数组，再用.IndexOf("#") != -1找出有第三级子节点，
                    //   用split计算出有多少个第三级子节点，数组缓存；
                    //4、如果没有第三级子节点，且第二级子节点之前没有?，即没有并列第二级子节点，
                    //   则第二级子节点的名字为*之前的字符串，否则就需要将*之前的?隔开的
                    //   字符串进行分割，取最后那个值为第二级子节点的名字；
                    //5、如果有第三级子节点，且第三级子节点之前没有?，即没有并列第三级子节点，
                    //   则第三级子节点的名字为#之前的字符串，否则就需要将#之前的?隔开的
                    //   字符串进行分割，取最后那个值为第二级子节点的名字；
                    //  <xx>
                    //      <aaa>
                    //      </aaa>
                    //      <vvv>
                    //      </vvv>
                    //      <www>
                    //          <rrr>222</rrr>
                    //          <gg>
                    //              <hh>
                    //              </hh>
                    //              <sss>
                    //              </sss>
                    //              <ttt>
                    //              </ttt>
                    //              <eee>
                    //              </eee>
                    //              <uuu>
                    //              </uuu>
                    //          </gg>


                    //      </www>

                    //      <ttt>333</ttt>
                    //      <uuu>444<uuu/>
                    //      <yyy>555</yyy>
                    //      <zzz>666</zzz>
                    //</xx>

                    if (XMLNodes[a].IndexOf(':') != -1 && Data[a].IndexOf(':') != -1)
                        {
                        //如果有子节点，那就要用WriteStartElement开始节点，
                        //用WriteEndElement结束节点，要匹配；
                        TempNodesName = Strings.Split(XMLNodes[a], ":");
                        TempData = Strings.Split(Data[a], ":");

                        //4、如果没有第三级子节点，且第二级子节点之前没有?，即没有并列第二级子节点，
                        //   则第二级子节点的名字为*之前的字符串，否则就需要将*之前的?隔开的
                        //   字符串进行分割，取最后那个值为第二级子节点的名字；
                        if (TempNodesName[0].IndexOf("?") != -1
                            && TempData[0].IndexOf("?") != -1)
                            {
                            string[] Temp1stNodeName, Temp1stData;
                            Temp1stNodeName = Strings.Split(TempNodesName[0], "?");
                            Temp1stData = Strings.Split(TempData[0], "?");
                            //将最后一个？前面的字符串当作第一个子节点的名字
                            //其它只是写值
                            for (int x = 0; x < Temp1stNodeName.Length - 1; x++)
                                {
                                TempXMLWrite.WriteElementString(Temp1stNodeName[x], Temp1stData[x]);
                                }

                            //将最后一个？前面的字符串当作第一个子节点的名字
                            TempXMLWrite.WriteStartElement(Temp1stNodeName[Temp1stNodeName.Length - 1]);
                            TempXMLWrite.WriteValue(Temp1stData[Temp1stData.Length - 1]);

                            }
                        else
                            {
                            TempXMLWrite.WriteStartElement(TempNodesName[0]);
                            TempXMLWrite.WriteValue(TempData[0]);
                            }

                        //查找第二级子节点
                        if (TempNodesName[1].IndexOf("*") != -1 && TempData[1].IndexOf("*") != -1)
                            {
                            //2、再用.IndexOf("*") != -1找出有第二级子节点，
                            //   用split计算出有多少个第二级子节点，数组缓存；
                            Temp2ndNodesName = Strings.Split(TempNodesName[1], "*");
                            Temp2ndData = Strings.Split(TempData[1], "*");

                            //例如：ss[0]="sss:ccc?fff*ggg?hhh*eee";
                            string[] TempStar = new string[Temp2ndNodesName.Length];
                            for (int _2ndNode = 0; _2ndNode < Temp2ndNodesName.Length; _2ndNode++)
                                {
                                //3、针对第二级子节点的缓存数组，再用.IndexOf("#") != -1找出有第三级子节点，
                                //   用split计算出有多少个第三级子节点，数组缓存；

                                if (Temp2ndNodesName[a].IndexOf("#") != -1)
                                    {



                                    }
                                else
                                    {
                                    //4、如果没有第三级子节点，且第二级子节点之前没有?，即没有并列第二级子节点，
                                    //   则第二级子节点的名字为*之前的字符串，否则就需要将*之前的?隔开的
                                    //   字符串进行分割，取最后那个值为第二级子节点的名字；
                                    if (Temp2ndNodesName[a].IndexOf("?") != -1)
                                        {
                                        string[] Temp3rd = Strings.Split(Temp2ndNodesName[a], "?");
                                        //如果经过#分割后的字符串还有？，就将？分割数组的最后一个当作下个节点的名字
                                        for (int y = 0; y < (Temp3rd.Length - 1); y++)
                                            {

                                            }
                                        }
                                    //else
                                    //    {

                                    //    }
                                    }

                                //5、如果有第三级子节点，且第三级子节点之前没有?，即没有并列第三级子节点，
                                //   则第三级子节点的名字为#之前的字符串，否则就需要将#之前的?隔开的
                                //   字符串进行分割，取最后那个值为第二级子节点的名字；
                                }
                            }
                        else
                            {
                            //例如：ss[0]="sss:fff?eee";
                            //没有第二级子节点，判断有无？分隔符，没有就直接写值，有就进行分割后再写值
                            if (TempNodesName[1].IndexOf("?") != -1 && TempData[1].IndexOf("?") != -1)
                                {
                                string[] Temp1, Temp2;
                                Temp1 = Strings.Split(TempNodesName[1], "?");
                                Temp2 = Strings.Split(TempData[1], "?");
                                for (int y = 0; y < Temp1.Length; y++)
                                    {
                                    if (y < Temp2.Length)
                                        {
                                        TempXMLWrite.WriteElementString(Temp1[y], Temp2[y]);
                                        }
                                    else
                                        {
                                        //数值部门不够的用空值代替
                                        TempXMLWrite.WriteElementString(Temp1[y], "");
                                        }
                                    }
                                }
                            else
                                {
                                TempXMLWrite.WriteElementString(TempNodesName[1], TempData[1]);
                                }
                            }

                        //SubNodesName = Strings.Split(TempNodesName[1], "?");

                        ////添加备注
                        //if (a <= Comments.Length)
                        //    {
                        //    if (Comments[a].IndexOf(':') != -1)
                        //        {
                        //        TempComments = Strings.Split(Comments[a], ":");
                        //        TempXMLWrite.WriteComment(TempComments[0]);
                        //        TempComments = Strings.Split(TempComments[1], "?");
                        //        }
                        //    else
                        //        {
                        //        TempComments=Strings.Split(Comments[a], "?");
                        //        }
                        //    }

                        //if (a <= Data.Length)
                        //    {
                        //    //找出数据中的':'号，然后再找出'?'分割的数据值
                        //    if (Data[a].IndexOf(':') != -1)
                        //        {
                        //        TempData = Strings.Split(Data[a], ":");
                        //        TempData = Strings.Split(TempData[1], "?");
                        //        }
                        //    else
                        //        {
                        //        TempData = Strings.Split(Data[a], "?");
                        //        }

                        //    //添加节点和节点数据
                        //    if (SubNodesName.Length <= TempData.Length)
                        //        {
                        //        for (int c = 0; c < SubNodesName.Length; c++) 
                        //            {
                        //            TempXMLWrite.WriteElementString(SubNodesName[c], TempData[c]);
                        //            //if (c <= TempComments.Length) 
                        //            //    {

                        //            //    }
                        //            }
                        //        }
                        //    else 
                        //        {
                        //        for (int c = 0; c < SubNodesName.Length; c++) 
                        //            {
                        //            if (c <= TempData.Length)
                        //                {
                        //                TempXMLWrite.WriteElementString(SubNodesName[c], TempData[c]);
                        //                }
                        //            else 
                        //                {
                        //                //数据长度少于节点个数时赋空值
                        //                TempXMLWrite.WriteElementString(SubNodesName[c], "");
                        //                }                                    
                        //            }
                        //        }
                        //    }

                        //用WriteEndElement结束节点，要匹配：定义的子节点；
                        TempXMLWrite.WriteEndElement();
                        //**************************************
                        }
                    else
                        {
                        //如果没有子节点，就直接写进XML文件中
                        //**************************************
                        TempXMLWrite.WriteStartElement(XMLNodes[a]);

                        //if (a <= Comments.Length)
                        //    {
                        //    TempXMLWrite.WriteComment(Comments[a]);
                        //    }

                        if (a <= Data.Length)
                            {
                            TempXMLWrite.WriteValue(Data[a]);
                            //TempXMLWrite.WriteElementString(XMLNodes[a], Data[a]);
                            //TempXMLWrite.WriteCData(Data[a]);
                            }
                        else
                            {
                            TempXMLWrite.WriteValue("");
                            //TempXMLWrite.WriteElementString(XMLNodes[a], "");
                            //TempXMLWrite.WriteCData("");
                            }
                        TempXMLWrite.WriteEndElement();

                        }
                    }



                TempXMLWrite.WriteEndElement();
                TempXMLWrite.WriteEndDocument();
                TempXMLWrite.Flush();
                TempXMLWrite.Close();



                //TempXMLDoc.Load(TargetXMLFileName);

                ////-- XMLNodes：节点名称，各个[]之间是并列关系，
                ////  下面如果有子节点，先用'：'分开，各个子节点之间用'？'隔开

                ////-- Data：节点数据，各个[]之间是并列关系，
                ////  下面如果有子节点，先用'：'分开，各个子节点之间用'？'隔开

                //for (int a = 0; a < XMLNodes.Length; a++)
                //    {
                //    //如果有子节点，先用'：'分开，各个子节点之间用'？'隔开
                //    //例如：str[1]="xx:rrr?sss?ttt?uuu?yyy?zzz";,
                //    //  <xx>
                //    //      <rrr>111</rrr>
                //    //      <sss>222</sss>
                //    //      <ttt>333</ttt>
                //    //      <uuu>444<uuu/>
                //    //      <yyy>555</yyy>
                //    //      <zzz>666</zzz>
                //    //</xx>
                //    //**************************************
                //    if (XMLNodes[a].IndexOf(':') != -1)
                //        {
                //        TempNodesName = Strings.Split(XMLNodes[a], ":");
                //        FirstNodeName = TempNodesName[0];
                //        FirstNode[a] = TempXMLDoc.CreateElement(FirstNodeName);

                //        SubNodesName = Strings.Split(TempNodesName[1], "?");

                //        TempSubNodes = new XmlElement[SubNodesName.Length];

                //        FirstNode[a].InnerText = TempNodesName[0];

                //        //找出数据中的':'号，然后再找出'?'分割的数据值
                //        if (Data[a].IndexOf(':') != -1)
                //            {
                //            TempData = Strings.Split(Data[a], ":");
                //            TempData = Strings.Split(TempData[1], "?");
                //            }
                //        else
                //            {
                //            TempData = Strings.Split(Data[a], "?");
                //            }

                //        //添加节点和节点数据
                //        for (int b = 0; b < TempSubNodes.Length; b++)
                //            {
                //            TempSubNodes[b] = TempXMLDoc.CreateElement(SubNodesName[b]);
                //            if (b <= TempData.Length)
                //                {
                //                TempSubNodes[b].InnerText = TempData[b];
                //                }
                //            else
                //                {
                //                //数据长度少于节点个数时赋空值
                //                TempSubNodes[b].InnerText = "";
                //                }
                //            FirstNode[a].AppendChild(TempSubNodes[b]);
                //            }

                //        TempXMLDoc.DocumentElement.AppendChild(FirstNode[a]);
                //        //**************************************
                //        }
                //    else
                //        {
                //        //**************************************
                //        //如果没有子节点
                //        FirstNodeName = XMLNodes[a];
                //        FirstNode[a] = TempXMLDoc.CreateElement(FirstNodeName);
                //        FirstNode[a].InnerText = Data[a];
                //        }
                //    TempXMLDoc.DocumentElement.AppendChild(FirstNode[a]);

                //    }

                //TempXMLDoc.Save(TargetXMLFileName);

                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //【目前只支持两层节点】将特定的节点和对应的数据记录添加至XML文件中
        /// <summary>
        /// 【目前只支持两层节点】将特定的节点和对应的数据记录添加至XML文件中
        /// 节点之间的关系是并列，即数组的[编号],
        /// 但是下面有节点'：'隔开，例如：str[1]="xx:";,
        /// 后面有并列子节点的用'?'号隔开，例如：str[1]="xx:rrr?sss?ttt?yyy";,
        /// 第一个*表示第二级子节点，第二个*表示第三级子节点
        /// </summary>
        /// <param name="TargetXMLFileName">添加数据的XML文件</param>
        /// <param name="XMLNodes">节点从第一个[0]开始，下面有节点的用'：'隔开，后面并列子节点数据用'?'号隔开
        /// 例如："ddd?kkk?f111:nnn?xxx?yyy*rrr?ccc*mmm?jj"
        ///   <ddd>ddd</ddd>
        ///   <kkk>kkk</kkk>
        ///   <f111>
        ///         <nnn>nnn</nnn>
        ///         <xxx>xxx</xxx>
        ///         <yyy>
        ///             <rrr>rrr</rrr>
        ///             <ccc>
        ///                 <mmm>mmm</mmm>
        ///                 <jj>jj</jj>
        ///             </ccc>
        ///         </yyy>
        ///   </f111>
        /// </param>
        /// <param name="Data">对应节点的数据，结构要相同，然后用：隔开，不足部分取值""</param>
        /// <returns></returns>
        public bool AddDataToXMLFile(string TargetXMLFileName, string[] XMLNodes,
            string[] Data)//, string[] Comments) /// <param name="Comments">对应节点的备注，结构要相同，然后用：隔开，不足部分取值""</param>
            {
            try
                {
                if (TargetXMLFileName == "")
                    {
                    //MessageBox.Show("The parameter 'TargetXMLFileName' can't be empty, please revise it.");
                    ErrorMessage = "The parameter 'TargetXMLFileName' can't be empty, please revise it.";
                    return false;
                    }

                if (TargetXMLFileName.ToUpper().IndexOf(".XML") == -1)
                    {
                    TargetXMLFileName = TargetXMLFileName + ".xml";
                    }

                //判断文件是否存在
                if (System.IO.File.Exists(TargetXMLFileName) == false)//&& System.IO.File.Exists(TargetXMLFileName + ".xml") == false)
                    {
                    return false;
                    }

                //可以考虑如果数据个数小于节点个数，可以赋值""
                if (XMLNodes.Length != Data.Length)
                    {
                    //MessageBox.Show("The length of parameter array 'XMLNodes' and 'Data' is not the same, please revise it.");
                    ErrorMessage = "The length of parameter array 'XMLNodes' and 'Data' is not the same, please revise it.";
                    return false;
                    }

                XmlDocument TempXMLDoc = new XmlDocument();
                XmlElement[] FirstNode = new XmlElement[XMLNodes.Length];
                XmlElement[] TempSubNodes, Temp2dSubNodes, Temp3rdSubNodes;
                string[] TempNodesName, TempData;
                string[] Temp2ndNodesName, Temp2ndData;

                TempXMLDoc.Load(TargetXMLFileName);

                //-- XMLNodes：节点名称，各个[]之间是并列关系，
                //  下面如果有子节点，先用'：'分开，各个子节点之间用'？'隔开

                //-- Data：节点数据，各个[]之间是并列关系，
                //  下面如果有子节点，先用'：'分开，各个子节点之间用'？'隔开

                //TempSubNodes = new XmlElement[XMLNodes.Length];

                for (int a = 0; a < XMLNodes.Length; a++)
                    {
                    //如果有子节点，先用'：'分开，各个子节点之间用'？'隔开
                    //例如：str[1]="xx:rrr?sss?ttt?uuu?yyy?zzz";,
                    //**************************************
                    //:表示第一级子节点，*表示第二级子节点，#表示第三级子节点
                    //例如：str[1]="xx:aaa?vvv?www*rrr?gg#hh?sss?ttt*ee#uuu?yyy?zzz";,
                    //1、先用:进行分割，找出第一级子节点的名称；
                    //2、再用.IndexOf("*") != -1找出有第二级子节点，
                    //   用split计算出有多少个第二级子节点，数组缓存；
                    //3、针对第二级子节点的缓存数组，再用.IndexOf("#") != -1找出有第三级子节点，
                    //   用split计算出有多少个第三级子节点，数组缓存；
                    //4、如果没有第三级子节点，且第二级子节点之前没有?，即没有并列第二级子节点，
                    //   则第二级子节点的名字为*之前的字符串，否则就需要将*之前的?隔开的
                    //   字符串进行分割，取最后那个值为第二级子节点的名字；
                    //5、如果有第三级子节点，且第三级子节点之前没有?，即没有并列第三级子节点，
                    //   则第三级子节点的名字为#之前的字符串，否则就需要将#之前的?隔开的
                    //   字符串进行分割，取最后那个值为第二级子节点的名字；

                    if (FC.FindStringInAnotherString(XMLNodes[a], ":") > 1
                        || FC.FindStringInAnotherString(Data[a], ":") > 1) 
                        {
                        ErrorMessage = "节点名称字符串数组和节点数值数组中只允许有一个：符号，此次数值被忽略。";
                        continue;
                        }

                    if (XMLNodes[a].IndexOf(':') != -1 && Data[a].IndexOf(':') != -1)
                        {
                        TempNodesName = Strings.Split(XMLNodes[a], ":");
                        TempSubNodes = new XmlElement[TempNodesName.Length - 1];
                        TempData = Strings.Split(Data[a], ":");

                        //4、如果没有第三级子节点，且第二级子节点之前没有?，即没有并列第二级子节点，
                        //   则第二级子节点的名字为*之前的字符串，否则就需要将*之前的?隔开的
                        //   字符串进行分割，取最后那个值为第二级子节点的名字；
                        if (TempNodesName[0].IndexOf("?") != -1
                            && TempData[0].IndexOf("?") != -1)
                            {
                            string[] Temp1stNodeName, Temp1stData;
                            Temp1stNodeName = Strings.Split(TempNodesName[0], "?");
                            XmlElement[] TempEle = new XmlElement[Temp1stNodeName.Length];
                            Temp1stData = Strings.Split(TempData[0], "?");
                            //将最后一个？前面的字符串当作第一个子节点的名字
                            //其它只是写值
                            for (int x = 0; x < Temp1stNodeName.Length - 1; x++)
                                {
                                TempEle[x] = TempXMLDoc.CreateElement(Temp1stNodeName[x]);
                                TempEle[x].InnerText = Temp1stData[x];
                                TempXMLDoc.DocumentElement.AppendChild(TempEle[x]);
                                }

                            //将最后一个？前面的字符串当作第一个子节点的名字
                            TempSubNodes[0] = TempXMLDoc.CreateElement(Temp1stNodeName[Temp1stNodeName.Length - 1]);
                            }
                        else
                            {
                            TempSubNodes[0] = TempXMLDoc.CreateElement(TempNodesName[0]);
                            }
                        
                        //4、如果没有第三级子节点，且第二级子节点之前没有?，即没有并列第二级子节点，
                        //   则第二级子节点的名字为*之前的字符串，否则就需要将*之前的?隔开的
                        //   字符串进行分割，取最后那个值为第二级子节点的名字；
                        //查找第二级子节点

                        if (TempNodesName[1].IndexOf("*") != -1 && TempData[1].IndexOf("*") != -1)
                            {
                            //*******************************************
                            //2、再用.IndexOf("*") != -1找出有第二级子节点，
                            //   用split计算出有多少个第二级子节点，数组缓存；
                            Temp2ndNodesName = Strings.Split(TempNodesName[1], "*");
                            Temp2dSubNodes = new XmlElement[Temp2ndNodesName.Length - 1];
                            Temp2ndData = Strings.Split(TempData[1], "*");

                            int CountOfStar = FC.FindStringInAnotherString(TempNodesName[1], "*");
                            bool[] FlagOfStar = new bool[CountOfStar];

                            //例如：ss[0]="sss:ccc?fff*ggg?hhh*eee";
                            string[] TempStar = new string[Temp2ndNodesName.Length];
                            string NameOfUpperNode = "";

                            //思路：先计算有多少个*，然后以*进行分割，建立节点数组，长度等于字符数组长度-1
                            //得到的字符串数组在处理时，从最后面的数组开始，
                            //先将最后数组建立CreateElement，然后作为上一个数组中最后一个的子节点；
                            //以此类推，直到处理完所有节点，然后添加作为：号前面的节点的子节点
                            //例如："ddd?kkk?f111:nnn?xxx?yyy*rrr?ccc*mmm?jj*www?rrr?iii*ppp"

                            for (int _2ndNode = (Temp2ndNodesName.Length - 1); _2ndNode > 0 ; _2ndNode--)
                                {

                                //获取上层父节点的名字，如果有?分割，则选择最后一个作为上层父节点的名字
                                if (FC.FindStringInAnotherString(Temp2ndNodesName[_2ndNode - 1], "?") >= 1)
                                    {
                                    string[] TempStrOfUpperNode = Strings.Split(Temp2ndNodesName[_2ndNode - 1], "?");
                                    NameOfUpperNode = TempStrOfUpperNode[TempStrOfUpperNode.Length - 1];
                                    }
                                else 
                                    {
                                    NameOfUpperNode = Temp2ndNodesName[_2ndNode - 1];
                                    }

                                Temp2dSubNodes[_2ndNode-1] = TempXMLDoc.CreateElement(NameOfUpperNode);                                

                                //#########################################
                                //将数组中下一层的节点进行创建并添加到上层节点
                                if (Temp2ndNodesName[_2ndNode].IndexOf("?") != -1)
                                    {
                                    string[] TempStr = Strings.Split(Temp2ndNodesName[_2ndNode], "?");
                                    Temp3rdSubNodes = new XmlElement[TempStr.Length];
                                    string[] TempDataStr = Strings.Split(Temp2ndData[_2ndNode], "?");

                                    for (int u = 0; u < (TempStr.Length - 1); u++) // 
                                        {
                                        Temp3rdSubNodes[u] = TempXMLDoc.CreateElement(TempStr[u]);
                                        Temp3rdSubNodes[u].InnerText = TempDataStr[u];
                                        Temp2dSubNodes[_2ndNode-1].AppendChild(Temp3rdSubNodes[u]);
                                        }
                                    }
                                else
                                    {
                                    //*之前没有？，就当作下个节点的名字
                                    XmlElement TempElement = TempXMLDoc.CreateElement(Temp2ndNodesName[_2ndNode]);
                                    TempElement.InnerText = Temp2ndData[_2ndNode];
                                    Temp2dSubNodes[_2ndNode - 1].AppendChild(TempElement);
                                    }
                                }

                            //将各级子节点按照反方向顺序逐级添加到上一级的节点
                            for (int x = (Temp2dSubNodes.Length - 1); x > 0 ; x--) 
                                {
                                Temp2dSubNodes[x - 1].AppendChild(Temp2dSubNodes[x]);                                
                                }

                            //判断第一个*号是否有?号，有就执行添加并列节点
                            if (Temp2ndNodesName[0].IndexOf('?') != -1)
                                {
                                string[] TempFirstStar = Strings.Split(Temp2ndNodesName[0],"?");
                                string[] TempFirstStarData = Strings.Split(Temp2ndData[0], "?");
                                XmlElement[] TempFirstStarElement = new XmlElement[TempFirstStar.Length-1];
                                for (int x = 0; x < TempFirstStar.Length - 1; x++) 
                                    {
                                    TempFirstStarElement[x] = TempXMLDoc.CreateElement(TempFirstStar[x]);
                                    if (TempFirstStarData.Length >= x)//TempFirstStar.Length)
                                        {
                                        TempFirstStarElement[x].InnerText = TempFirstStarData[x];
                                        }
                                    else
                                        {
                                        TempFirstStarElement[x].InnerText = "";
                                        }
                                    TempSubNodes[0].AppendChild(TempFirstStarElement[x]);
                                    }
                                }
                            TempSubNodes[0].AppendChild(Temp2dSubNodes[0]);
                            TempXMLDoc.DocumentElement.AppendChild(TempSubNodes[0]);
                            //*******************************************
                            }
                        else
                            {
                            //例如：ss[0]="sss:fff?eee";
                            //没有第二级子节点，判断有无？分隔符，没有就直接写值，有就进行分割后再写值
                            if (TempNodesName[1].IndexOf("?") != -1 && TempData[1].IndexOf("?") != -1)
                                {
                                string[] Temp1, Temp2;
                                Temp1 = Strings.Split(TempNodesName[1], "?");
                                Temp2dSubNodes = new XmlElement[Temp1.Length];
                                Temp2 = Strings.Split(TempData[1], "?");
                                for (int y = 0; y < Temp1.Length; y++)
                                    {
                                    if (y < Temp2.Length)
                                        {
                                        Temp2dSubNodes[y] = TempXMLDoc.CreateElement(Temp1[y]);
                                        Temp2dSubNodes[y].InnerText = Temp2[y];
                                        TempSubNodes[0].AppendChild(Temp2dSubNodes[y]);
                                        }
                                    else
                                        {
                                        //数值部门不够的用空值代替
                                        Temp2dSubNodes[y] = TempXMLDoc.CreateElement(Temp1[y]);
                                        Temp2dSubNodes[y].InnerText = "";
                                        TempSubNodes[0].AppendChild(Temp2dSubNodes[y]);
                                        }
                                    }
                                TempXMLDoc.DocumentElement.AppendChild(TempSubNodes[0]);
                                }
                            else
                                {
                                Temp2dSubNodes = new XmlElement[1];
                                Temp2dSubNodes[0] = TempXMLDoc.CreateElement(TempNodesName[1]);
                                Temp2dSubNodes[0].InnerText = TempData[1];
                                TempSubNodes[TempNodesName.Length - 1].AppendChild(Temp2dSubNodes[0]);
                                }
                            //*******************************************
                            }
                        }
                    else
                        {
                        //如果没有子节点，就直接写进XML文件中
                        //**************************************
                        if (XMLNodes[a].IndexOf('?') != -1 && Data[a].IndexOf('?') != -1)
                            {
                            TempNodesName = Strings.Split(XMLNodes[a], "?");
                            TempSubNodes = new XmlElement[TempNodesName.Length];
                            TempData = Strings.Split(Data[a], "?");
                            for (int r = 0; r < TempData.Length; r++)
                                {
                                if (TempData.Length >= TempNodesName.Length)
                                    {
                                    TempSubNodes[r] = TempXMLDoc.CreateElement(TempNodesName[r]);
                                    TempSubNodes[r].InnerText = TempData[r];
                                    }
                                else
                                    {
                                    TempSubNodes[r] = TempXMLDoc.CreateElement(TempNodesName[r]);
                                    TempSubNodes[r].InnerText = "";
                                    }
                                TempXMLDoc.DocumentElement.AppendChild(TempSubNodes[r]);
                                }
                            }
                        else
                            {
                            TempSubNodes = new XmlElement[1];
                            TempSubNodes[0] = TempXMLDoc.CreateElement(XMLNodes[a]);
                            TempSubNodes[0].InnerText = Data[a];
                            TempXMLDoc.DocumentElement.AppendChild(TempSubNodes[0]);
                            }
                        }
                    }

                TempXMLDoc.Save(TargetXMLFileName);

                TempXMLDoc = null;
                FirstNode = null;
                TempSubNodes = null;
                Temp2dSubNodes = null;
                Temp3rdSubNodes = null;
                TempNodesName = null;
                TempData = null;
                Temp2ndNodesName = null;
                Temp2ndData = null;

                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }
        
        //【目前只支持两层节点】将特定的节点和对应的数据记录添加至XML文件中
        /// <summary>
        /// 【目前只支持两层节点】将特定的节点和对应的数据记录添加至XML文件中
        /// 节点之间的关系是并列，即数组的[编号],
        /// 但是下面有节点'：'隔开，例如：str[1]="xx:";,
        /// 后面有并列子节点的用'?'号隔开，例如：str[1]="xx:rrr?sss?ttt?yyy";,
        /// 第一个*表示第二级子节点，第二个*表示第三级子节点
        /// </summary>
        /// <param name="XMLNodes">节点从第一个[0]开始，下面有节点的用'：'隔开，后面并列子节点数据用'?'号隔开
        /// 例如："ddd?kkk?f111:nnn?xxx?yyy*rrr?ccc*mmm?jj"
        ///   <ddd>ddd</ddd>
        ///   <kkk>kkk</kkk>
        ///   <f111>
        ///         <nnn>nnn</nnn>
        ///         <xxx>xxx</xxx>
        ///         <yyy>
        ///             <rrr>rrr</rrr>
        ///             <ccc>
        ///                 <mmm>mmm</mmm>
        ///                 <jj>jj</jj>
        ///             </ccc>
        ///         </yyy>
        ///   </f111>
        /// </param>
        /// <param name="Data">对应节点的数据，结构要相同，然后用：隔开，不足部分取值""</param>
        /// <returns></returns>
        public bool AddDataToXMLFile(string[] XMLNodes,
            string[] Data)//, string[] Comments) /// <param name="Comments">对应节点的备注，结构要相同，然后用：隔开，不足部分取值""</param>
            {
            try
                {
                if (XMLFileName == "")
                    {
                    //MessageBox.Show("The parameter 'XMLFileName' can't be empty, please revise it.");
                    ErrorMessage = "The parameter 'XMLFileName' can't be empty, please revise it.";
                    return false;
                    }

                if (XMLFileName.ToUpper().IndexOf(".XML") == -1)
                    {
                    XMLFileName = XMLFileName + ".xml";
                    }

                //判断文件是否存在
                if (System.IO.File.Exists(XMLFileName) == false)//&& System.IO.File.Exists(XMLFileName + ".xml") == false)
                    {
                    ErrorMessage = "文件不存在";
                    return false;
                    }

                //可以考虑如果数据个数小于节点个数，可以赋值""
                if (XMLNodes.Length != Data.Length)
                    {
                    //MessageBox.Show("The length of parameter array 'XMLNodes' and 'Data' is not the same, please revise it.");
                    ErrorMessage = "The length of parameter array 'XMLNodes' and 'Data' is not the same, please revise it.";
                    return false;
                    }

                XmlDocument TempXMLDoc = new XmlDocument();
                XmlElement[] FirstNode = new XmlElement[XMLNodes.Length];
                XmlElement[] TempSubNodes, Temp2dSubNodes, Temp3rdSubNodes;
                string[] TempNodesName, TempData;
                string[] Temp2ndNodesName, Temp2ndData;

                TempXMLDoc.Load(XMLFileName);

                //-- XMLNodes：节点名称，各个[]之间是并列关系，
                //  下面如果有子节点，先用'：'分开，各个子节点之间用'？'隔开

                //-- Data：节点数据，各个[]之间是并列关系，
                //  下面如果有子节点，先用'：'分开，各个子节点之间用'？'隔开

                //TempSubNodes = new XmlElement[XMLNodes.Length];

                for (int a = 0; a < XMLNodes.Length; a++)
                    {
                    //如果有子节点，先用'：'分开，各个子节点之间用'？'隔开
                    //例如：str[1]="xx:rrr?sss?ttt?uuu?yyy?zzz";,
                    //**************************************
                    //:表示第一级子节点，*表示第二级子节点，#表示第三级子节点
                    //例如：str[1]="xx:aaa?vvv?www*rrr?gg#hh?sss?ttt*ee#uuu?yyy?zzz";,
                    //1、先用:进行分割，找出第一级子节点的名称；
                    //2、再用.IndexOf("*") != -1找出有第二级子节点，
                    //   用split计算出有多少个第二级子节点，数组缓存；
                    //3、针对第二级子节点的缓存数组，再用.IndexOf("#") != -1找出有第三级子节点，
                    //   用split计算出有多少个第三级子节点，数组缓存；
                    //4、如果没有第三级子节点，且第二级子节点之前没有?，即没有并列第二级子节点，
                    //   则第二级子节点的名字为*之前的字符串，否则就需要将*之前的?隔开的
                    //   字符串进行分割，取最后那个值为第二级子节点的名字；
                    //5、如果有第三级子节点，且第三级子节点之前没有?，即没有并列第三级子节点，
                    //   则第三级子节点的名字为#之前的字符串，否则就需要将#之前的?隔开的
                    //   字符串进行分割，取最后那个值为第二级子节点的名字；

                    if (FC.FindStringInAnotherString(XMLNodes[a], ":") > 1
                        || FC.FindStringInAnotherString(Data[a], ":") > 1) 
                        {
                        ErrorMessage = "节点名称字符串数组和节点数值数组中只允许有一个：符号，此次数值被忽略。";
                        continue;
                        }

                    if (XMLNodes[a].IndexOf(':') != -1 && Data[a].IndexOf(':') != -1)
                        {
                        TempNodesName = Strings.Split(XMLNodes[a], ":");
                        TempSubNodes = new XmlElement[TempNodesName.Length - 1];
                        TempData = Strings.Split(Data[a], ":");

                        //4、如果没有第三级子节点，且第二级子节点之前没有?，即没有并列第二级子节点，
                        //   则第二级子节点的名字为*之前的字符串，否则就需要将*之前的?隔开的
                        //   字符串进行分割，取最后那个值为第二级子节点的名字；
                        if (TempNodesName[0].IndexOf("?") != -1
                            && TempData[0].IndexOf("?") != -1)
                            {
                            string[] Temp1stNodeName, Temp1stData;
                            Temp1stNodeName = Strings.Split(TempNodesName[0], "?");
                            XmlElement[] TempEle = new XmlElement[Temp1stNodeName.Length];
                            Temp1stData = Strings.Split(TempData[0], "?");
                            //将最后一个？前面的字符串当作第一个子节点的名字
                            //其它只是写值
                            for (int x = 0; x < Temp1stNodeName.Length - 1; x++)
                                {
                                TempEle[x] = TempXMLDoc.CreateElement(Temp1stNodeName[x]);
                                TempEle[x].InnerText = Temp1stData[x];
                                TempXMLDoc.DocumentElement.AppendChild(TempEle[x]);
                                }

                            //将最后一个？前面的字符串当作第一个子节点的名字
                            TempSubNodes[0] = TempXMLDoc.CreateElement(Temp1stNodeName[Temp1stNodeName.Length - 1]);
                            }
                        else
                            {
                            TempSubNodes[0] = TempXMLDoc.CreateElement(TempNodesName[0]);
                            }
                        
                        //4、如果没有第三级子节点，且第二级子节点之前没有?，即没有并列第二级子节点，
                        //   则第二级子节点的名字为*之前的字符串，否则就需要将*之前的?隔开的
                        //   字符串进行分割，取最后那个值为第二级子节点的名字；
                        //查找第二级子节点

                        if (TempNodesName[1].IndexOf("*") != -1 && TempData[1].IndexOf("*") != -1)
                            {
                            //*******************************************
                            //2、再用.IndexOf("*") != -1找出有第二级子节点，
                            //   用split计算出有多少个第二级子节点，数组缓存；
                            Temp2ndNodesName = Strings.Split(TempNodesName[1], "*");
                            Temp2dSubNodes = new XmlElement[Temp2ndNodesName.Length - 1];
                            Temp2ndData = Strings.Split(TempData[1], "*");

                            int CountOfStar = FC.FindStringInAnotherString(TempNodesName[1], "*");
                            bool[] FlagOfStar = new bool[CountOfStar];

                            //例如：ss[0]="sss:ccc?fff*ggg?hhh*eee";
                            string[] TempStar = new string[Temp2ndNodesName.Length];
                            string NameOfUpperNode = "";

                            //思路：先计算有多少个*，然后以*进行分割，建立节点数组，长度等于字符数组长度-1
                            //得到的字符串数组在处理时，从最后面的数组开始，
                            //先将最后数组建立CreateElement，然后作为上一个数组中最后一个的子节点；
                            //以此类推，直到处理完所有节点，然后添加作为：号前面的节点的子节点
                            //例如："ddd?kkk?f111:nnn?xxx?yyy*rrr?ccc*mmm?jj*www?rrr?iii*ppp"

                            for (int _2ndNode = (Temp2ndNodesName.Length - 1); _2ndNode > 0 ; _2ndNode--)
                                {

                                //获取上层父节点的名字，如果有?分割，则选择最后一个作为上层父节点的名字
                                if (FC.FindStringInAnotherString(Temp2ndNodesName[_2ndNode - 1], "?") >= 1)
                                    {
                                    string[] TempStrOfUpperNode = Strings.Split(Temp2ndNodesName[_2ndNode - 1], "?");
                                    NameOfUpperNode = TempStrOfUpperNode[TempStrOfUpperNode.Length - 1];
                                    }
                                else 
                                    {
                                    NameOfUpperNode = Temp2ndNodesName[_2ndNode - 1];
                                    }

                                Temp2dSubNodes[_2ndNode-1] = TempXMLDoc.CreateElement(NameOfUpperNode);                                

                                //#########################################
                                //将数组中下一层的节点进行创建并添加到上层节点
                                if (Temp2ndNodesName[_2ndNode].IndexOf("?") != -1)
                                    {
                                    string[] TempStr = Strings.Split(Temp2ndNodesName[_2ndNode], "?");
                                    Temp3rdSubNodes = new XmlElement[TempStr.Length];
                                    string[] TempDataStr = Strings.Split(Temp2ndData[_2ndNode], "?");

                                    for (int u = 0; u < (TempStr.Length - 1); u++) // 
                                        {
                                        Temp3rdSubNodes[u] = TempXMLDoc.CreateElement(TempStr[u]);
                                        Temp3rdSubNodes[u].InnerText = TempDataStr[u];
                                        Temp2dSubNodes[_2ndNode-1].AppendChild(Temp3rdSubNodes[u]);
                                        }
                                    }
                                else
                                    {
                                    //*之前没有？，就当作下个节点的名字
                                    XmlElement TempElement = TempXMLDoc.CreateElement(Temp2ndNodesName[_2ndNode]);
                                    TempElement.InnerText = Temp2ndData[_2ndNode];
                                    Temp2dSubNodes[_2ndNode - 1].AppendChild(TempElement);
                                    }
                                }

                            //将各级子节点按照反方向顺序逐级添加到上一级的节点
                            for (int x = (Temp2dSubNodes.Length - 1); x > 0 ; x--) 
                                {
                                Temp2dSubNodes[x - 1].AppendChild(Temp2dSubNodes[x]);                                
                                }

                            //判断第一个*号是否有?号，有就执行添加并列节点
                            if (Temp2ndNodesName[0].IndexOf('?') != -1)
                                {
                                string[] TempFirstStar = Strings.Split(Temp2ndNodesName[0],"?");
                                string[] TempFirstStarData = Strings.Split(Temp2ndData[0], "?");
                                XmlElement[] TempFirstStarElement = new XmlElement[TempFirstStar.Length-1];
                                for (int x = 0; x < TempFirstStar.Length - 1; x++) 
                                    {
                                    TempFirstStarElement[x] = TempXMLDoc.CreateElement(TempFirstStar[x]);
                                    if (TempFirstStarData.Length >= x)//TempFirstStar.Length)
                                        {
                                        TempFirstStarElement[x].InnerText = TempFirstStarData[x];
                                        }
                                    else
                                        {
                                        TempFirstStarElement[x].InnerText = "";
                                        }
                                    TempSubNodes[0].AppendChild(TempFirstStarElement[x]);
                                    }
                                }
                            TempSubNodes[0].AppendChild(Temp2dSubNodes[0]);
                            TempXMLDoc.DocumentElement.AppendChild(TempSubNodes[0]);
                            //*******************************************
                            }
                        else
                            {
                            //例如：ss[0]="sss:fff?eee";
                            //没有第二级子节点，判断有无？分隔符，没有就直接写值，有就进行分割后再写值
                            if (TempNodesName[1].IndexOf("?") != -1 && TempData[1].IndexOf("?") != -1)
                                {
                                string[] Temp1, Temp2;
                                Temp1 = Strings.Split(TempNodesName[1], "?");
                                Temp2dSubNodes = new XmlElement[Temp1.Length];
                                Temp2 = Strings.Split(TempData[1], "?");
                                for (int y = 0; y < Temp1.Length; y++)
                                    {
                                    if (y < Temp2.Length)
                                        {
                                        Temp2dSubNodes[y] = TempXMLDoc.CreateElement(Temp1[y]);
                                        Temp2dSubNodes[y].InnerText = Temp2[y];
                                        TempSubNodes[0].AppendChild(Temp2dSubNodes[y]);
                                        }
                                    else
                                        {
                                        //数值部门不够的用空值代替
                                        Temp2dSubNodes[y] = TempXMLDoc.CreateElement(Temp1[y]);
                                        Temp2dSubNodes[y].InnerText = "";
                                        TempSubNodes[0].AppendChild(Temp2dSubNodes[y]);
                                        }
                                    }
                                TempXMLDoc.DocumentElement.AppendChild(TempSubNodes[0]);
                                }
                            else
                                {
                                Temp2dSubNodes = new XmlElement[1];
                                Temp2dSubNodes[0] = TempXMLDoc.CreateElement(TempNodesName[1]);
                                Temp2dSubNodes[0].InnerText = TempData[1];
                                TempSubNodes[TempNodesName.Length - 1].AppendChild(Temp2dSubNodes[0]);
                                }
                            //*******************************************
                            }
                        }
                    else
                        {
                        //如果没有子节点，就直接写进XML文件中
                        //**************************************
                        if (XMLNodes[a].IndexOf('?') != -1 && Data[a].IndexOf('?') != -1)
                            {
                            TempNodesName = Strings.Split(XMLNodes[a], "?");
                            TempSubNodes = new XmlElement[TempNodesName.Length];
                            TempData = Strings.Split(Data[a], "?");
                            for (int r = 0; r < TempData.Length; r++)
                                {
                                if (TempData.Length >= TempNodesName.Length)
                                    {
                                    TempSubNodes[r] = TempXMLDoc.CreateElement(TempNodesName[r]);
                                    TempSubNodes[r].InnerText = TempData[r];
                                    }
                                else
                                    {
                                    TempSubNodes[r] = TempXMLDoc.CreateElement(TempNodesName[r]);
                                    TempSubNodes[r].InnerText = "";
                                    }
                                TempXMLDoc.DocumentElement.AppendChild(TempSubNodes[r]);
                                }
                            }
                        else
                            {
                            TempSubNodes = new XmlElement[1];
                            TempSubNodes[0] = TempXMLDoc.CreateElement(XMLNodes[a]);
                            TempSubNodes[0].InnerText = Data[a];
                            TempXMLDoc.DocumentElement.AppendChild(TempSubNodes[0]);
                            }
                        }
                    }

                TempXMLDoc.Save(XMLFileName);

                TempXMLDoc = null;
                FirstNode = null;
                TempSubNodes=null;
                Temp2dSubNodes = null;
                Temp3rdSubNodes = null;
                TempNodesName = null;
                TempData = null;
                Temp2ndNodesName = null;
                Temp2ndData = null;

                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //【目前只支持两层节点】将特定的节点和对应的数据记录添加至XML文件中
        /// <summary>
        /// 【目前只支持两层节点】将特定的节点和对应的数据记录添加至XML文件中
        /// 节点之间的关系是并列，即数组的[编号],
        /// 但是下面有节点'：'隔开，例如：str[1]="xx:";,
        /// 后面有并列子节点的用'?'号隔开，例如：str[1]="xx:rrr?sss?ttt?yyy";,
        /// 第一个*表示第二级子节点，第二个*表示第三级子节点
        /// </summary>
        /// <param name="TargetXMLFileName">添加数据的XML文件</param>
        /// <param name="XMLNodes">节点从第一个[0]开始，下面有节点的用'：'隔开，后面并列子节点数据用'?'号隔开
        /// 例如："ddd?kkk?f111:nnn?xxx?yyy*rrr?ccc*mmm?jj"
        ///   <ddd>ddd</ddd>
        ///   <kkk>kkk</kkk>
        ///   <f111>
        ///         <nnn>nnn</nnn>
        ///         <xxx>xxx</xxx>
        ///         <yyy>
        ///             <rrr>rrr</rrr>
        ///             <ccc>
        ///                 <mmm>mmm</mmm>
        ///                 <jj>jj</jj>
        ///             </ccc>
        ///         </yyy>
        ///   </f111>
        /// </param>
        /// <param name="Data">对应节点的数据，结构要相同，然后用：隔开，不足部分取值""</param>
        /// <returns></returns>
        private bool OldAddDataToXMLFile(string TargetXMLFileName, string[] XMLNodes,
            string[] Data)//, string[] Comments) /// <param name="Comments">对应节点的备注，结构要相同，然后用：隔开，不足部分取值""</param>
            {
            try
                {
                if (TargetXMLFileName == "")
                    {
                    MessageBox.Show("The parameter 'TargetXMLFileName' can't be empty, please revise it.");
                    return false;
                    }

                if (TargetXMLFileName.ToUpper().IndexOf(".XML") == -1)
                    {
                    TargetXMLFileName = TargetXMLFileName + ".xml";
                    }

                //判断文件是否存在
                if (System.IO.File.Exists(TargetXMLFileName) == false)//&& System.IO.File.Exists(TargetXMLFileName + ".xml") == false)
                    {
                    return false;
                    }

                //可以考虑如果数据个数小于节点个数，可以赋值""
                if (XMLNodes.Length != Data.Length)
                    {
                    MessageBox.Show("The length of parameter array 'XMLNodes' and 'Data' is not the same, please revise it.");
                    return false;
                    }

                XmlDocument TempXMLDoc = new XmlDocument();
                XmlElement[] FirstNode = new XmlElement[XMLNodes.Length];
                XmlElement[] TempSubNodes, Temp2dSubNodes, Temp3rdSubNodes;
                string FirstNodeName, DataStr;
                string[] SubNodesName, TempNodesName, TempData, TempComments;
                string[] Sub2ndNodesName, Temp2ndNodesName, Temp2ndData, Temp2ndComments;
                string[] Sub3rdNodesName, Temp3rdNodesName, Temp3rdData, Temp3rdComments;

                TempXMLDoc.Load(TargetXMLFileName);

                //-- XMLNodes：节点名称，各个[]之间是并列关系，
                //  下面如果有子节点，先用'：'分开，各个子节点之间用'？'隔开

                //-- Data：节点数据，各个[]之间是并列关系，
                //  下面如果有子节点，先用'：'分开，各个子节点之间用'？'隔开

                //TempSubNodes = new XmlElement[XMLNodes.Length];

                for (int a = 0; a < XMLNodes.Length; a++)
                    {
                    //如果有子节点，先用'：'分开，各个子节点之间用'？'隔开
                    //例如：str[1]="xx:rrr?sss?ttt?uuu?yyy?zzz";,
                    //**************************************
                    //:表示第一级子节点，*表示第二级子节点，#表示第三级子节点
                    //例如：str[1]="xx:aaa?vvv?www*rrr?gg#hh?sss?ttt*ee#uuu?yyy?zzz";,
                    //1、先用:进行分割，找出第一级子节点的名称；
                    //2、再用.IndexOf("*") != -1找出有第二级子节点，
                    //   用split计算出有多少个第二级子节点，数组缓存；
                    //3、针对第二级子节点的缓存数组，再用.IndexOf("#") != -1找出有第三级子节点，
                    //   用split计算出有多少个第三级子节点，数组缓存；
                    //4、如果没有第三级子节点，且第二级子节点之前没有?，即没有并列第二级子节点，
                    //   则第二级子节点的名字为*之前的字符串，否则就需要将*之前的?隔开的
                    //   字符串进行分割，取最后那个值为第二级子节点的名字；
                    //5、如果有第三级子节点，且第三级子节点之前没有?，即没有并列第三级子节点，
                    //   则第三级子节点的名字为#之前的字符串，否则就需要将#之前的?隔开的
                    //   字符串进行分割，取最后那个值为第二级子节点的名字；

                    if (XMLNodes[a].IndexOf(':') != -1 && Data[a].IndexOf(':') != -1)
                        {
                        TempNodesName = Strings.Split(XMLNodes[a], ":");
                        TempSubNodes = new XmlElement[TempNodesName.Length - 1];
                        TempData = Strings.Split(Data[a], ":");

                        //4、如果没有第三级子节点，且第二级子节点之前没有?，即没有并列第二级子节点，
                        //   则第二级子节点的名字为*之前的字符串，否则就需要将*之前的?隔开的
                        //   字符串进行分割，取最后那个值为第二级子节点的名字；
                        if (TempNodesName[0].IndexOf("?") != -1
                            && TempData[0].IndexOf("?") != -1)
                            {
                            string[] Temp1stNodeName, Temp1stData;
                            Temp1stNodeName = Strings.Split(TempNodesName[0], "?");
                            XmlElement[] TempEle = new XmlElement[Temp1stNodeName.Length];
                            Temp1stData = Strings.Split(TempData[0], "?");
                            //将最后一个？前面的字符串当作第一个子节点的名字
                            //其它只是写值
                            for (int x = 0; x < Temp1stNodeName.Length - 1; x++)
                                {
                                TempEle[x] = TempXMLDoc.CreateElement(Temp1stNodeName[x]);
                                TempEle[x].InnerText = Temp1stData[x];
                                TempXMLDoc.DocumentElement.AppendChild(TempEle[x]);
                                }

                            //将最后一个？前面的字符串当作第一个子节点的名字
                            //TempSubNodes[TempNodesName.Length-1] = TempXMLDoc.CreateElement(Temp1stNodeName[Temp1stNodeName.Length - 1]);
                            TempSubNodes[0] = TempXMLDoc.CreateElement(Temp1stNodeName[Temp1stNodeName.Length - 1]);
                            }
                        else
                            {
                            TempSubNodes[TempNodesName.Length-1] = TempXMLDoc.CreateElement(TempNodesName[0]);
                            TempSubNodes[0] = TempXMLDoc.CreateElement(TempNodesName[0]);}

                        //查找第二级子节点
                        if (TempNodesName[1].IndexOf("*") != -1 && TempData[1].IndexOf("*") != -1)
                            {
                            //2、再用.IndexOf("*") != -1找出有第二级子节点，
                            //   用split计算出有多少个第二级子节点，数组缓存；
                            Temp2ndNodesName = Strings.Split(TempNodesName[1], "*");
                            Temp2dSubNodes = new XmlElement[Temp2ndNodesName.Length];
                            Temp2ndData = Strings.Split(TempData[1], "*");

                            int CountOfStar = FC.FindStringInAnotherString(TempNodesName[1], "*");
                            int TempCountOfStar = 0;
                            bool[] FlagOfStar = new bool[CountOfStar];

                            //例如：ss[0]="sss:ccc?fff*ggg?hhh*eee";
                            string[] TempStar = new string[Temp2ndNodesName.Length];
                            string NameForNext2ndNode = "";

                            //4、如果没有第三级子节点，且第二级子节点之前没有?，即没有并列第二级子节点，
                            //   则第二级子节点的名字为*之前的字符串，否则就需要将*之前的?隔开的
                            //   字符串进行分割，取最后那个值为第二级子节点的名字；

                            for (int _2ndNode = 0; _2ndNode < Temp2ndNodesName.Length; _2ndNode++)
                                {
                                
                                if (Temp2ndNodesName[_2ndNode].IndexOf("?") != -1)
                                    {
                                    string[] TempStr = Strings.Split(Temp2ndNodesName[_2ndNode], "?");
                                    Temp3rdSubNodes = new XmlElement[TempStr.Length];
                                    string[] TempDataStr = Strings.Split(Temp2ndData[_2ndNode], "?");

                                    Temp2dSubNodes[_2ndNode] = TempXMLDoc.CreateElement(TempStr[TempStr.Length-1]);

                                    //TempXMLWrite.WriteStartElement(TempStr[TempStr.Length - 1]);
                                    //TempXMLWrite.WriteValue(TempDataStr[TempStr.Length - 1]);

                                    TempCountOfStar += 1;

                                    if (TempCountOfStar < CountOfStar)
                                        {
                                        for (int u = 0; u < (TempStr.Length- 1) ; u++) // 
                                            {
                                            //TempXMLWrite.WriteStartElement(TempStr[u]);
                                            //if (u <= TempDataStr.Length)
                                            //{
                                            //TempXMLWrite.WriteElementString(TempStr[u], TempDataStr[u]);
                                            Temp3rdSubNodes[u] = TempXMLDoc.CreateElement(TempStr[u]);
                                            Temp3rdSubNodes[u].InnerText = TempDataStr[u];
                                            //Temp2dSubNodes[_2ndNode].AppendChild(Temp3rdSubNodes[u]);
                                            TempSubNodes[0].AppendChild(Temp3rdSubNodes[u]);
                                            //    }
                                            //else 
                                            //    {
                                            //    TempXMLWrite.WriteElementString(TempStr[u], "");
                                            //    }
                                            }

                                        //TempSubNodes[0].AppendChild(Temp2dSubNodes[_2ndNode]);
                                        
                                        }
                                    else
                                        {
                                        for (int u = 0; u < (TempStr.Length); u++) // 
                                            {
                                            //TempXMLWrite.WriteStartElement(TempStr[u]);
                                            //if (u <= TempDataStr.Length)
                                            //{
                                            //TempXMLWrite.WriteElementString(TempStr[u], TempDataStr[u]);
                                            Temp3rdSubNodes[u] = TempXMLDoc.CreateElement(TempStr[u]);
                                            Temp3rdSubNodes[u].InnerText = TempDataStr[u];
                                            Temp2dSubNodes[_2ndNode].AppendChild(Temp3rdSubNodes[u]);
                                            
                                            //    }
                                            //else 
                                            //    {
                                            //    TempXMLWrite.WriteElementString(TempStr[u], "");
                                            //    }

                                            }

                                        TempSubNodes[0].AppendChild(Temp2dSubNodes[_2ndNode]);

                                        }

                                    

                                    //if (TempCountOfStar <= CountOfStar)
                                    //    {
                                    //    TempXMLWrite.WriteStartElement(TempStr[TempStr.Length - 1]);
                                    //    }


                                    }
                                else
                                    {
                                    //*之前没有？，就当作下个节点的名字
                                    NameForNext2ndNode = Temp2ndNodesName[_2ndNode];
                                    //TempXMLWrite.WriteStartElement(NameForNext2ndNode);
                                    //TempXMLWrite.WriteValue(Temp2ndData[_2ndNode]);
                                    Temp2dSubNodes[_2ndNode] = TempXMLDoc.CreateElement(NameForNext2ndNode);
                                    //TempSubNodes[0].AppendChild(Temp2dSubNodes[_2ndNode]);
                                    TempSubNodes[0].AppendChild(Temp2dSubNodes[_2ndNode]);
                                    }
                                
                                //TempSubNodes[TempNodesName.Length - 2].AppendChild(Temp2dSubNodes[_2ndNode]);
                                
                                }
                            //TempXMLWrite.WriteEndElement();

                            TempXMLDoc.DocumentElement.AppendChild(TempSubNodes[0]);

                            }
                        else
                            {
                            //例如：ss[0]="sss:fff?eee";
                            //没有第二级子节点，判断有无？分隔符，没有就直接写值，有就进行分割后再写值
                            if (TempNodesName[1].IndexOf("?") != -1 && TempData[1].IndexOf("?") != -1)
                                {
                                string[] Temp1, Temp2;
                                Temp1 = Strings.Split(TempNodesName[1], "?");
                                Temp2dSubNodes = new XmlElement[Temp1.Length];
                                Temp2 = Strings.Split(TempData[1], "?");
                                for (int y = 0; y < Temp1.Length; y++)
                                    {
                                    if (y < Temp2.Length)
                                        {
                                        //TempXMLWrite.WriteElementString(Temp1[y], Temp2[y]);
                                        Temp2dSubNodes[y] = TempXMLDoc.CreateElement(Temp1[y]);
                                        Temp2dSubNodes[y].InnerText = Temp2[y];
                                        //TempSubNodes[TempNodesName.Length - 1].AppendChild(Temp2dSubNodes[y]);
                                        TempSubNodes[0].AppendChild(Temp2dSubNodes[y]);
                                        }
                                    else
                                        {
                                        //数值部门不够的用空值代替
                                        //TempXMLWrite.WriteElementString(Temp1[y], "");
                                        Temp2dSubNodes[y] = TempXMLDoc.CreateElement(Temp1[y]);
                                        Temp2dSubNodes[y].InnerText = "";
                                        //TempSubNodes[TempNodesName.Length - 1].AppendChild(Temp2dSubNodes[y]);
                                        TempSubNodes[0].AppendChild(Temp2dSubNodes[y]);
                                        }
                                    }
                                TempXMLDoc.DocumentElement.AppendChild(TempSubNodes[0]);
                                }
                            else
                                {
                                //TempXMLWrite.WriteElementString(TempNodesName[1], TempData[1]);
                                Temp2dSubNodes = new XmlElement[1];
                                Temp2dSubNodes[0] = TempXMLDoc.CreateElement(TempNodesName[1]);
                                Temp2dSubNodes[0].InnerText = TempData[1];
                                TempSubNodes[TempNodesName.Length - 1].AppendChild(Temp2dSubNodes[0]);
                                }
                            }

                        //用WriteEndElement结束节点，要匹配：定义的子节点；
                        
                        //TempXMLWrite.WriteEndElement();
                        //**************************************
                        }
                    else
                        {
                        //如果没有子节点，就直接写进XML文件中
                        //**************************************
                        if (XMLNodes[a].IndexOf('?') != -1 && Data[a].IndexOf('?') != -1)
                            {
                            //如果有子节点，那就要用WriteStartElement开始节点，
                            //用WriteEndElement结束节点，要匹配；
                            TempNodesName = Strings.Split(XMLNodes[a], "?");
                            TempSubNodes = new XmlElement[TempNodesName.Length];
                            TempData = Strings.Split(Data[a], "?");
                            for (int r = 0; r < TempData.Length; r++)
                                {
                                if (TempData.Length >= TempNodesName.Length)
                                    {
                                    //TempXMLWrite.WriteElementString(TempNodesName[r], TempData[r]);
                                    TempSubNodes[r] = TempXMLDoc.CreateElement(TempNodesName[r]);
                                    TempSubNodes[r].InnerText = TempData[r];
                                    }
                                else
                                    {
                                    //TempXMLWrite.WriteElementString(TempNodesName[r], "");
                                    TempSubNodes[r] = TempXMLDoc.CreateElement(TempNodesName[r]);
                                    TempSubNodes[r].InnerText = "";
                                    }
                                TempXMLDoc.DocumentElement.AppendChild(TempSubNodes[r]);
                                }
                            }
                        else
                            {
                            TempSubNodes = new XmlElement[1];
                            //TempXMLWrite.WriteElementString(XMLNodes[a], Data[a]);
                            TempSubNodes[0] = TempXMLDoc.CreateElement(XMLNodes[a]);
                            TempSubNodes[0].InnerText = Data[a];
                            TempXMLDoc.DocumentElement.AppendChild(TempSubNodes[0]);
                            }
                        }
                    }

                TempXMLDoc.Save(TargetXMLFileName);

                //TempXMLDoc.Load(TargetXMLFileName);

                ////-- XMLNodes：节点名称，各个[]之间是并列关系，
                ////  下面如果有子节点，先用'：'分开，各个子节点之间用'？'隔开

                ////-- Data：节点数据，各个[]之间是并列关系，
                ////  下面如果有子节点，先用'：'分开，各个子节点之间用'？'隔开

                //for (int a = 0; a < XMLNodes.Length; a++)
                //    {
                //    //如果有子节点，先用'：'分开，各个子节点之间用'？'隔开
                //    //例如：str[1]="xx:rrr?sss?ttt?uuu?yyy?zzz";,
                //    //  <xx>
                //    //      <rrr>111</rrr>
                //    //      <sss>222</sss>
                //    //      <ttt>333</ttt>
                //    //      <uuu>444<uuu/>
                //    //      <yyy>555</yyy>
                //    //      <zzz>666</zzz>
                //    //</xx>
                //    //**************************************
                //    if (XMLNodes[a].IndexOf(':') != -1)
                //        {
                //        TempNodesName = Strings.Split(XMLNodes[a], ":");
                //        FirstNodeName = TempNodesName[0];
                //        FirstNode[a] = TempXMLDoc.CreateElement(FirstNodeName);

                //        SubNodesName = Strings.Split(TempNodesName[1], "?");

                //        TempSubNodes = new XmlElement[SubNodesName.Length];

                //        FirstNode[a].InnerText = TempNodesName[0];

                //        //找出数据中的':'号，然后再找出'?'分割的数据值
                //        if (Data[a].IndexOf(':') != -1)
                //            {
                //            TempData = Strings.Split(Data[a], ":");
                //            TempData = Strings.Split(TempData[1], "?");
                //            }
                //        else
                //            {
                //            TempData = Strings.Split(Data[a], "?");
                //            }

                //        //添加节点和节点数据
                //        for (int b = 0; b < TempSubNodes.Length; b++)
                //            {
                //            TempSubNodes[b] = TempXMLDoc.CreateElement(SubNodesName[b]);
                //            if (b <= TempData.Length)
                //                {
                //                TempSubNodes[b].InnerText = TempData[b];
                //                }
                //            else
                //                {
                //                //数据长度少于节点个数时赋空值
                //                TempSubNodes[b].InnerText = "";
                //                }
                //            FirstNode[a].AppendChild(TempSubNodes[b]);
                //            }

                //        TempXMLDoc.DocumentElement.AppendChild(FirstNode[a]);
                //        //**************************************
                //        }
                //    else
                //        {
                //        //**************************************
                //        //如果没有子节点
                //        FirstNodeName = XMLNodes[a];
                //        FirstNode[a] = TempXMLDoc.CreateElement(FirstNodeName);
                //        FirstNode[a].InnerText = Data[a];
                //        }
                //    TempXMLDoc.DocumentElement.AppendChild(FirstNode[a]);

                //    }

                //TempXMLDoc.Save(TargetXMLFileName);

                return true;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //打开XML文件并返回此文件所有节点的集合
        /// <summary>
        /// 打开XML文件并返回此文件所有节点的集合
        /// </summary>
        /// <param name="TargetXMLFileName">需要打开的XML文件名称</param>
        /// <returns></returns>
        public XmlNodeList OpenXMLFile(string TargetXMLFileName)
            {
            try
                {

                if (TargetXMLFileName == "")
                    {
                    MessageBox.Show("The parameter 'TargetXMLFileName' can't be empty, please revise it.");
                    return null;
                    }

                if (TargetXMLFileName.ToUpper().IndexOf(".XML") == -1)
                    {
                    TargetXMLFileName = TargetXMLFileName + ".xml";
                    }

                //判断文件是否存在
                if (System.IO.File.Exists(TargetXMLFileName) == false)//&& System.IO.File.Exists(TargetXMLFileName + ".xml") == false)
                    {
                    return null;
                    }

                XmlDocument TempXmlDoc=new XmlDocument();
                XmlNodeList TempNodeList;
                TempXmlDoc.Load(TargetXMLFileName);
                TempNodeList = TempXmlDoc.ChildNodes;

                TempXmlDoc = null;

                return TempNodeList;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }

        //打开XML文件并返回此文件所有节点的集合
        /// <summary>
        /// 打开XML文件并返回此文件所有节点的集合
        /// </summary>
        /// <returns></returns>
        public XmlNodeList OpenXMLFile()
            {
            try
                {

                if (XMLFileName == "")
                    {
                    //MessageBox.Show("The parameter 'XMLFileName' can't be empty, please revise it.");
                    ErrorMessage = "The parameter 'XMLFileName' can't be empty, please revise it.";
                    return null;
                    }

                if (XMLFileName.ToUpper().IndexOf(".XML") == -1)
                    {
                    XMLFileName = XMLFileName + ".xml";
                    }

                //判断文件是否存在
                if (System.IO.File.Exists(XMLFileName) == false)//&& System.IO.File.Exists(TargetXMLFileName + ".xml") == false)
                    {
                    ErrorMessage = "文件不存在";
                    return null;
                    }

                XmlDocument TempXmlDoc=new XmlDocument();
                XmlNodeList TempNodeList;
                TempXmlDoc.Load(XMLFileName);
                TempNodeList = TempXmlDoc.ChildNodes;

                TempXmlDoc = null;

                return TempNodeList;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }

        //查找XML文件中某个节点的所有值【InnerText】
        /// <summary>
        /// 查找XML文件中某个节点的所有值【InnerText】
        /// </summary>
        /// <param name="TargetXMLFileName">需要打开的XML文件名称</param>
        /// <param name="TargetNodeName">节点名</param>
        /// <returns>查找到的某个节点名称之值的字符串数组</returns>
        public string[] FindInnerTextInXMLFile(string TargetXMLFileName, string TargetNodeName)
            {
            try
                {

                if (TargetXMLFileName == "")
                    {
                    //MessageBox.Show("The parameter 'TargetXMLFileName' can't be empty, please revise it.");
                    ErrorMessage = "The parameter 'TargetXMLFileName' can't be empty, please revise it.";
                    return null;
                    }

                if (TargetNodeName == "") 
                    {
                    ErrorMessage = "The parameter 'TargetNodeName' can't be empty, please revise it.";
                    return null;
                    }

                if (TargetXMLFileName.ToUpper().IndexOf(".XML") == -1)
                    {
                    TargetXMLFileName = TargetXMLFileName + ".xml";
                    }

                //判断文件是否存在
                if (System.IO.File.Exists(TargetXMLFileName) == false)//&& System.IO.File.Exists(TargetXMLFileName + ".xml") == false)
                    {
                    ErrorMessage =TargetXMLFileName + " 文件不存在";
                    return null;
                    }

                XmlDocument TempXmlDoc = new XmlDocument();
                XmlNodeList TempNodeList;
                string[] FoundNodeInnerText;

                TempXmlDoc.Load(TargetXMLFileName);

                //用GetElementsByTagName(标记名)读取某一类的元素列表，然后赋值给数组
                TempNodeList = TempXmlDoc.GetElementsByTagName(TargetNodeName);//返回一个 XmlNodeList，它包含与指定 Name 匹配的所有子代元素的列表。

                FoundNodeInnerText = new string[TempNodeList.Count];

                for (int a = 0; a < TempNodeList.Count; a++) 
                    {
                    FoundNodeInnerText[a] = TempNodeList.Item(a).InnerText;
                    
                    //想要将下面结构的查找结果用','进行分割，暂时没有找到方法
                    //for (int b = 0; b < TempNodeList.Item(a).InnerText.Length; b++) 
                    //    {
                    //    FoundNodeInnerText[a] = TempNodeList.Item(a).InnerText;
                    //    }
                    }

                TempNodeList = null;
                TempXmlDoc = null;

                //****************
                //节点结构：                
                //<TestPara>
                //  <index>0</index>
                //  <testName>FAI 4</testName>
                //  <normal>002.390</normal>
                //  <min>0.030</min>
                //  <max>0.050</max>
                //  <pixV>0.000</pixV>
                //  <Bc>000.000</Bc>
                //</TestPara>

                //读取的字符串数组结果：
                //0FAI 4002.3900.0300.0500.000000.000
                //0FAI 4002.3900.0300.0500.000000.000
                //0FAI 4002.3900.0300.0500.000000.000

                //****************
                return FoundNodeInnerText;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }
         
        //查找XML文件中某个节点的所有值【InnerText】
        /// <summary>
        /// 查找XML文件中某个节点的所有值【InnerText】
        /// </summary>
        /// <param name="TargetNodeName">节点名</param>
        /// <returns>查找到的某个节点名称之值的字符串数组</returns>
        public string[] FindInnerTextInXMLFile(string TargetNodeName)
            {
            try
                {

                if (XMLFileName == "")
                    {
                    //MessageBox.Show("The parameter 'XMLFileName' can't be empty, please revise it.");
                    ErrorMessage = "The parameter 'XMLFileName' can't be empty, please revise it.";
                    return null;
                    }

                if (TargetNodeName == "") 
                    {
                    ErrorMessage = "The parameter 'TargetNodeName' can't be empty, please revise it.";
                    return null;
                    }

                if (XMLFileName.ToUpper().IndexOf(".XML") == -1)
                    {
                    XMLFileName = XMLFileName + ".xml";
                    }

                //判断文件是否存在
                if (System.IO.File.Exists(XMLFileName) == false)//&& System.IO.File.Exists(TargetXMLFileName + ".xml") == false)
                    {
                    ErrorMessage = XMLFileName + " 文件不存在";
                    return null;
                    }

                XmlDocument TempXmlDoc = new XmlDocument();
                XmlNodeList TempNodeList;
                string[] FoundNodeInnerText;

                TempXmlDoc.Load(XMLFileName);

                //用GetElementsByTagName(标记名)读取某一类的元素列表，然后赋值给数组
                TempNodeList = TempXmlDoc.GetElementsByTagName(TargetNodeName);//返回一个 XmlNodeList，它包含与指定 Name 匹配的所有子代元素的列表。

                FoundNodeInnerText = new string[TempNodeList.Count];

                for (int a = 0; a < TempNodeList.Count; a++) 
                    {
                    FoundNodeInnerText[a] = TempNodeList.Item(a).InnerText;
                    
                    //想要将下面结构的查找结果用','进行分割，暂时没有找到方法
                    //for (int b = 0; b < TempNodeList.Item(a).InnerText.Length; b++) 
                    //    {
                    //    FoundNodeInnerText[a] = TempNodeList.Item(a).InnerText;
                    //    }
                    }

                TempNodeList = null;
                TempXmlDoc = null;

                //****************
                //节点结构：                
                //<TestPara>
                //  <index>0</index>
                //  <testName>FAI 4</testName>
                //  <normal>002.390</normal>
                //  <min>0.030</min>
                //  <max>0.050</max>
                //  <pixV>0.000</pixV>
                //  <Bc>000.000</Bc>
                //</TestPara>

                //读取的字符串数组结果：
                //0FAI 4002.3900.0300.0500.000000.000
                //0FAI 4002.3900.0300.0500.000000.000
                //0FAI 4002.3900.0300.0500.000000.000

                //****************
                return FoundNodeInnerText;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }

        //示例：打开XML文件并读取分层结构的值
        /// <summary>
        /// 示例：打开XML文件并读取分层结构的值
        /// </summary>
        private void ExampleForFindInnerText() 
            {
            try
                {

                XmlDocument XMLDoc = new XmlDocument();
                XmlNodeList XMLRoot = null, TempNodeList = null, 
                    ParaTempNodeList = null, TestParaNodeList = null;
                OpenFileDialog OpenFile = new OpenFileDialog();

                //因为para子节点的TestPara节点是从3开始(数组从0开始起，即前面有3个节点了
                int TempCount = 3;

                string[] ServerPara = new string[500];

                OpenFile.DefaultExt = "xml";
                OpenFile.Filter = "XML文件|*.xml";
                OpenFile.CheckFileExists = true;
                OpenFile.Title = "打开服务器参数设置XML文件";

                if (OpenFile.ShowDialog() == DialogResult.OK && OpenFile.FileName != "") 
                    {
                    XMLDoc.Load(OpenFile.FileName);//装载打开的文件
                    TempNodeList = XMLDoc.ChildNodes;//获取文件的所有子节点

                    for (int a = 0; a < TempNodeList.Count; a++) 
                        {
                        switch (TempNodeList.Item(a).Name) 
                            {
                            case "root":
                                XMLRoot = TempNodeList.Item(a).ChildNodes;

                                for (int b = 0; b < XMLRoot.Count; b++)
                                    {
                                    switch (XMLRoot.Item(b).Name)
                                        {
                                        case "MachineID":
                                            //设备号
                                            ServerPara[0] = XMLRoot.Item(b).InnerText;
                                            break;

                                        case "MaxExcelColumnQty":
                                            //带格式EXCEL表格的最大导出行数设定
                                            ServerPara[1] = XMLRoot.Item(b).InnerText;
                                            break;

                                        case "Statistic":
                                            //导出带格式的EXCEL表格时,是否需要统计信息
                                            ServerPara[2] = XMLRoot.Item(b).InnerText;
                                            break;

                                        case "opNG":
                                            //opNG:无效测量 如放反,没有放平,这时CCD输出-1, 这种NG不计良率, 
                                            //当出现这种情况 2表示N2,3表示N3                                        
                                            ServerPara[3] = XMLRoot.Item(b).InnerText;
                                            break;

                                        case "ContinueAlarmQty":
                                            //连续多少次不良报警
                                            ServerPara[4] = XMLRoot.Item(b).InnerText;
                                            break;

                                        case "JudgementType":
                                            //是判断功能定义
                                            ServerPara[5] = XMLRoot.Item(b).InnerText;
                                            break;

                                        case "AllNG":
                                            //当指定的FAI全为NG时,排到指定的料槽,如果为1,则排NG,
                                            //2为排N2,3为排N3 ..... 这个是当JudgementType==3 时才有的功能
                                            ServerPara[6] = XMLRoot.Item(b).InnerText;
                                            break;

                                        case "USL":
                                            //超标准公差上限
                                            ServerPara[7] = XMLRoot.Item(b).InnerText;
                                            break;

                                        case "LSL":
                                            //超标准公差下限
                                            ServerPara[8] = XMLRoot.Item(b).InnerText;
                                            break;

                                        case "Password":
                                            //密码
                                            ServerPara[9] = XMLRoot.Item(b).InnerText;
                                            break;

                                        case "Para":
                                            ParaTempNodeList = XMLRoot.Item(b).ChildNodes;

                                            for (int c = 0; c < ParaTempNodeList.Count; c++) 
                                                {
                                                switch (ParaTempNodeList.Item(c).Name) 
                                                    {
                                                    case "ID":
                                                        //

                                                        break;
                                                    }
                                                }
                                            break;

                                        }
                                    }
                                break;
                            }
                        }
                    }
                
                }
            catch (Exception ex) 
                {
                ErrorMessage = ex.Message;
                }
            }

        //释放类的相关资源
        /// <summary>
        /// 释放类的相关资源
        /// </summary>
        public void Dispose() 
            {
            FC.Dispose();
            }

        #endregion

        }//Class

    }//namespace