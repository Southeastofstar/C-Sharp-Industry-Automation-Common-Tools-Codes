using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PengDongNanTools
{
    /// <summary>
    /// 常量信息
    /// </summary>
    public static class CConstant
    {
        /// <summary>
        /// 时间和信息之间的分割字符串：2个空格
        /// </summary>
        public const string SplitStringForTimeAndMsg = "  ";

        /// <summary>
        /// 对方IP地址和端口号之间的分割字符串：-
        /// </summary>
        public const string SplitStringForIPAddressAndPort = "-";

        /// <summary>
        /// 对方端口号和信息之间的分割字符串：--::
        /// </summary>
        public const string SplitStringForPortAndMsg = "--::";

        /// <summary>
        /// 是否使用提示对话框显示信息，默认 true
        /// </summary>
        public static bool ShowMessageDialog = true;

    }//class

    /// <summary>
    /// 通讯过程中接收和发送信息的处理方式
    /// </summary>
    public enum HandleMsgMode
    {
        /// <summary>
        /// 原始信息，例如：信息内容
        /// </summary>
        OriginalMsg = 0,

        /// <summary>
        /// 原始信息加上日期，例如：日期 + 信息内容
        /// </summary>
        MsgWithDateTime = 1,

        /// <summary>
        /// 原始信息加上日期、对方IP地址和端口，例如：日期 + 对方IP地址 + 对方端口 + 信息内容
        /// </summary>
        MsgWithDateTimeRemoteInfo = 2

        ///// <summary>
        ///// XML格式传递字符串
        ///// </summary>
        //XMLFormat = 3,

        ///// <summary>
        ///// Json格式传递字符串
        ///// </summary>
        //JsonFormat = 4
    }



}//namespace