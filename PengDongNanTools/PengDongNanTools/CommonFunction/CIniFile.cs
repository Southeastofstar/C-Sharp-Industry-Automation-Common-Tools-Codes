using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace PengDongNanTools
{
    /// <summary>
    /// 读写Ini文件：调用Windows API函数
    /// </summary>
    public static class CIniFile
    {
        /// <summary>
        /// Windows API：向INI文件写参数
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键名称</param>
        /// <param name="val">键的值</param>
        /// <param name="filePath">INI文件的完整路径</param>
        /// <returns></returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]//网上查的返回值为bool
        internal static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        /// <summary>
        /// 从INI文件中读取参数
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键名称</param>
        /// <param name="def">默认值，如果读取失败则返回默认值</param>
        /// <param name="retVal">返回的字符串值</param>
        /// <param name="size">目的缓存器的大小</param>
        /// <param name="filePath">INI文件的完整路径</param>
        /// <returns></returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
            int size, string filePath);

        /// <summary>
        /// 向INI文件【INI文件的完整路径】写参数
        /// </summary>
        /// <param name="SectionName">节点名称</param>
        /// <param name="KeyName">键名称</param>
        /// <param name="KeyValue">键的值</param>
        /// <param name="FileNameWithFullPath">INI文件的完整路径</param>
        public static bool Write(string SectionName, string KeyName, string KeyValue, string FileNameWithFullPath)
        {
            long lRet = 0;

            try
            {
                if (null == SectionName || "" == SectionName
                    || null == KeyName || "" == KeyName
                    || null == FileNameWithFullPath || "" == FileNameWithFullPath)
                {
                    return false;
                }

                DirectoryInfo dirInfo = new DirectoryInfo(FileNameWithFullPath);
                if (null == dirInfo.FullName || dirInfo.FullName == "")
                {
                    //return false;
                }

                if (null == KeyValue)
                {
                    KeyValue = "";
                }

                //成功写数据到INI文件时返回的值(多个写操作都是返回同一个值)：
                //52072870691471361
                //52072870691471361
                //写数据到INI文件失败时返回的值(多个写操作都是返回同一个值)：
                //27303072740933633
                //27303072740933633
                lRet = WritePrivateProfileString(SectionName, KeyName, KeyValue, FileNameWithFullPath);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #region "Read"

        /// <summary>
        /// 从INI文件【INI文件的完整路径】中读取参数，返回对应键名称的值
        /// </summary>
        /// <param name="SectionName">节点名称</param>
        /// <param name="KeyName">键名称</param>
        /// <param name="DefaultValue">默认值，如果读取失败则返回默认值</param>
        /// <param name="FileNameWithFullPath">INI文件的完整路径</param>
        /// <param name="StringLengthOfKeyValue">键值字符串长度</param>
        /// <returns>返回对应键名称值的字符串</returns>
        public static string Read(string SectionName, string KeyName, string DefaultValue, string FileNameWithFullPath, Int32 StringLengthOfKeyValue = 500)
        {
            StringBuilder temp = new StringBuilder();
            int i = 0;

            try
            {
                StringLengthOfKeyValue = Math.Abs(StringLengthOfKeyValue);//防止出现负值的情况，避免产生异常
                temp = new StringBuilder(StringLengthOfKeyValue);

                if (null == SectionName || "" == SectionName
                    || null == KeyName || "" == KeyName
                    || null == FileNameWithFullPath || "" == FileNameWithFullPath)
                {
                    return "";
                }

                //从INI文件读取参数失败时返回的值(多个写操作都是返回同一个值)：
                //0

                //从INI文件成功读取参数时返回的值(多个写操作都是返回同一个值)：
                //18
                i = GetPrivateProfileString(SectionName, KeyName, DefaultValue, temp, StringLengthOfKeyValue, FileNameWithFullPath);
            }
            catch (Exception)
            {
            }
            return temp.ToString();
        }

        /// <summary>
        /// 从INI文件【INI文件的完整路径】中读取bool参数，返回对应键名称的bool值
        /// </summary>
        /// <param name="SectionName">节点名称</param>
        /// <param name="KeyName">键名称</param>
        /// <param name="DefaultValue">默认值，如果读取失败则返回默认值</param>
        /// <param name="FileNameWithFullPath">INI文件的完整路径</param>
        /// <param name="StringLengthOfKeyValue">键值字符串长度</param>
        /// <returns></returns>
        public static bool Read(string SectionName, string KeyName, bool DefaultValue, string FileNameWithFullPath, Int32 StringLengthOfKeyValue = 500)
        {
            StringBuilder temp = new StringBuilder();
            int i = 0;

            try
            {
                StringLengthOfKeyValue = Math.Abs(StringLengthOfKeyValue);//防止出现负值的情况，避免产生异常
                temp = new StringBuilder(StringLengthOfKeyValue);

                if (null == SectionName || "" == SectionName
                    || null == KeyName || "" == KeyName
                    || null == FileNameWithFullPath || "" == FileNameWithFullPath)
                {
                    return false;
                }

                //从INI文件读取参数失败时返回的值(多个写操作都是返回同一个值)：
                //0

                //从INI文件成功读取参数时返回的值(多个写操作都是返回同一个值)：
                //18
                i = GetPrivateProfileString(SectionName, KeyName, DefaultValue.ToString(), temp, StringLengthOfKeyValue, FileNameWithFullPath);

                return Convert.ToBoolean(temp.ToString());
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 从INI文件【INI文件的完整路径】中读取byte参数，返回对应键名称的byte值
        /// </summary>
        /// <param name="SectionName">节点名称</param>
        /// <param name="KeyName">键名称</param>
        /// <param name="DefaultValue">默认值，如果读取失败则返回默认值</param>
        /// <param name="FileNameWithFullPath">INI文件的完整路径</param>
        /// <param name="StringLengthOfKeyValue">键值字符串长度</param>
        /// <returns></returns>
        public static byte Read(string SectionName, string KeyName, byte DefaultValue, string FileNameWithFullPath, Int32 StringLengthOfKeyValue = 500)
        {
            StringBuilder temp = new StringBuilder();
            int i = 0;
            byte byResult = 0;

            try
            {
                StringLengthOfKeyValue = Math.Abs(StringLengthOfKeyValue);//防止出现负值的情况，避免产生异常
                temp = new StringBuilder(StringLengthOfKeyValue);

                if (null == SectionName || "" == SectionName
                    || null == KeyName || "" == KeyName
                    || null == FileNameWithFullPath || "" == FileNameWithFullPath)
                {
                    return byResult;
                }

                //从INI文件读取参数失败时返回的值(多个写操作都是返回同一个值)：
                //0

                //从INI文件成功读取参数时返回的值(多个写操作都是返回同一个值)：
                //18
                i = GetPrivateProfileString(SectionName, KeyName, DefaultValue.ToString(), temp, StringLengthOfKeyValue, FileNameWithFullPath);

                return Convert.ToByte(temp.ToString());
            }
            catch (Exception)
            {
                return byResult;
            }
        }

        /// <summary>
        /// 从INI文件【INI文件的完整路径】中读取short参数，返回对应键名称的short值
        /// </summary>
        /// <param name="SectionName">节点名称</param>
        /// <param name="KeyName">键名称</param>
        /// <param name="DefaultValue">默认值，如果读取失败则返回默认值</param>
        /// <param name="FileNameWithFullPath">INI文件的完整路径</param>
        /// <param name="StringLengthOfKeyValue">键值字符串长度</param>
        /// <returns></returns>
        public static short Read(string SectionName, string KeyName, short DefaultValue, string FileNameWithFullPath, Int32 StringLengthOfKeyValue = 500)
        {
            StringBuilder temp = new StringBuilder();
            int i = 0;
            short stResult = 0;

            try
            {
                StringLengthOfKeyValue = Math.Abs(StringLengthOfKeyValue);//防止出现负值的情况，避免产生异常
                temp = new StringBuilder(StringLengthOfKeyValue);

                if (null == SectionName || "" == SectionName
                    || null == KeyName || "" == KeyName
                    || null == FileNameWithFullPath || "" == FileNameWithFullPath)
                {
                    return stResult;
                }

                //从INI文件读取参数失败时返回的值(多个写操作都是返回同一个值)：
                //0

                //从INI文件成功读取参数时返回的值(多个写操作都是返回同一个值)：
                //18
                i = GetPrivateProfileString(SectionName, KeyName, DefaultValue.ToString(), temp, StringLengthOfKeyValue, FileNameWithFullPath);

                return Convert.ToInt16(temp.ToString());
            }
            catch (Exception)
            {
                return stResult;
            }
        }

        /// <summary>
        /// 从INI文件【INI文件的完整路径】中读取int参数，返回对应键名称的int值
        /// </summary>
        /// <param name="SectionName">节点名称</param>
        /// <param name="KeyName">键名称</param>
        /// <param name="DefaultValue">默认值，如果读取失败则返回默认值</param>
        /// <param name="FileNameWithFullPath">INI文件的完整路径</param>
        /// <param name="StringLengthOfKeyValue">键值字符串长度</param>
        /// <returns></returns>
        public static int Read(string SectionName, string KeyName, int DefaultValue, string FileNameWithFullPath, Int32 StringLengthOfKeyValue = 500)
        {
            StringBuilder temp = new StringBuilder();
            int i = 0;
            int iResult = 0;

            try
            {
                StringLengthOfKeyValue = Math.Abs(StringLengthOfKeyValue);//防止出现负值的情况，避免产生异常
                temp = new StringBuilder(StringLengthOfKeyValue);

                if (null == SectionName || "" == SectionName
                    || null == KeyName || "" == KeyName
                    || null == FileNameWithFullPath || "" == FileNameWithFullPath)
                {
                    return iResult;
                }

                //从INI文件读取参数失败时返回的值(多个写操作都是返回同一个值)：
                //0

                //从INI文件成功读取参数时返回的值(多个写操作都是返回同一个值)：
                //18
                i = GetPrivateProfileString(SectionName, KeyName, DefaultValue.ToString(), temp, StringLengthOfKeyValue, FileNameWithFullPath);

                iResult = Convert.ToInt32(temp.ToString());

                return iResult;
            }
            catch (Exception)
            {
                return iResult;
            }
        }

        /// <summary>
        /// 从INI文件【INI文件的完整路径】中读取long参数，返回对应键名称的long值
        /// </summary>
        /// <param name="SectionName">节点名称</param>
        /// <param name="KeyName">键名称</param>
        /// <param name="DefaultValue">默认值，如果读取失败则返回默认值</param>
        /// <param name="FileNameWithFullPath">INI文件的完整路径</param>
        /// <param name="StringLengthOfKeyValue">键值字符串长度</param>
        /// <returns></returns>
        public static long Read(string SectionName, string KeyName, long DefaultValue, string FileNameWithFullPath, Int32 StringLengthOfKeyValue = 500)
        {
            StringBuilder temp = new StringBuilder();
            int i = 0;
            long lResult = 0;

            try
            {
                StringLengthOfKeyValue = Math.Abs(StringLengthOfKeyValue);//防止出现负值的情况，避免产生异常
                temp = new StringBuilder(StringLengthOfKeyValue);

                if (null == SectionName || "" == SectionName
                    || null == KeyName || "" == KeyName
                    || null == FileNameWithFullPath || "" == FileNameWithFullPath)
                {
                    return lResult;
                }

                //从INI文件读取参数失败时返回的值(多个写操作都是返回同一个值)：
                //0

                //从INI文件成功读取参数时返回的值(多个写操作都是返回同一个值)：
                //18
                i = GetPrivateProfileString(SectionName, KeyName, DefaultValue.ToString(), temp, StringLengthOfKeyValue, FileNameWithFullPath);

                lResult = Convert.ToInt64(temp.ToString());

                return lResult;
            }
            catch (Exception)
            {
                return lResult;
            }
        }

        /// <summary>
        /// 从INI文件【INI文件的完整路径】中读取float参数，返回对应键名称的float值
        /// </summary>
        /// <param name="SectionName">节点名称</param>
        /// <param name="KeyName">键名称</param>
        /// <param name="DefaultValue">默认值，如果读取失败则返回默认值</param>
        /// <param name="FileNameWithFullPath">INI文件的完整路径</param>
        /// <param name="StringLengthOfKeyValue">键值字符串长度</param>
        /// <returns></returns>
        public static float Read(string SectionName, string KeyName, float DefaultValue, string FileNameWithFullPath, Int32 StringLengthOfKeyValue = 500)
        {
            StringBuilder temp = new StringBuilder();
            int i = 0;
            float fResult = 0;

            try
            {
                StringLengthOfKeyValue = Math.Abs(StringLengthOfKeyValue);//防止出现负值的情况，避免产生异常
                temp = new StringBuilder(StringLengthOfKeyValue);

                if (null == SectionName || "" == SectionName
                    || null == KeyName || "" == KeyName
                    || null == FileNameWithFullPath || "" == FileNameWithFullPath)
                {
                    return fResult;
                }

                //从INI文件读取参数失败时返回的值(多个写操作都是返回同一个值)：
                //0

                //从INI文件成功读取参数时返回的值(多个写操作都是返回同一个值)：
                //18
                i = GetPrivateProfileString(SectionName, KeyName, DefaultValue.ToString(), temp, StringLengthOfKeyValue, FileNameWithFullPath);

                fResult = Convert.ToSingle(temp.ToString());

                return fResult;
            }
            catch (Exception)
            {
                return fResult;
            }
        }

        /// <summary>
        /// 从INI文件【INI文件的完整路径】中读取double参数，返回对应键名称的double值
        /// </summary>
        /// <param name="SectionName">节点名称</param>
        /// <param name="KeyName">键名称</param>
        /// <param name="DefaultValue">默认值，如果读取失败则返回默认值</param>
        /// <param name="FileNameWithFullPath">INI文件的完整路径</param>
        /// <param name="StringLengthOfKeyValue">键值字符串长度</param>
        /// <returns></returns>
        public static double Read(string SectionName, string KeyName, double DefaultValue, string FileNameWithFullPath, Int32 StringLengthOfKeyValue = 500)
        {
            StringBuilder temp = new StringBuilder();
            int i = 0;
            double dResult = 0;

            try
            {
                StringLengthOfKeyValue = Math.Abs(StringLengthOfKeyValue);//防止出现负值的情况，避免产生异常
                temp = new StringBuilder(StringLengthOfKeyValue);

                if (null == SectionName || "" == SectionName
                    || null == KeyName || "" == KeyName
                    || null == FileNameWithFullPath || "" == FileNameWithFullPath)
                {
                    return dResult;
                }

                //从INI文件读取参数失败时返回的值(多个写操作都是返回同一个值)：
                //0

                //从INI文件成功读取参数时返回的值(多个写操作都是返回同一个值)：
                //18
                i = GetPrivateProfileString(SectionName, KeyName, DefaultValue.ToString(), temp, StringLengthOfKeyValue, FileNameWithFullPath);

                dResult = Convert.ToSingle(temp.ToString());

                return dResult;
            }
            catch (Exception)
            {
                return dResult;
            }
        }

        #endregion

    }//class

}//namespace
