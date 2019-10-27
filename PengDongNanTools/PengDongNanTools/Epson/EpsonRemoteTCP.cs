#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Microsoft.Office.Interop;
using System.Threading;
using Microsoft.VisualBasic;

#endregion

namespace PengDongNanTools
    {

    //用于对EPSON机械手进行远程以太网控制【软件作者：彭东南, southeastofstar@163.com】
    /// <summary>
    /// 用于对EPSON机械手进行远程以太网控制【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    class EpsonRemoteTCP
        {

#region "命令说明"

        //*******************************************************************
        //String .IndexOf 方法 (Char)    报告指定 Unicode 字符在此字符串中的第一个匹配项的索引。

        //命名空间：   System
        //程序集：   mscorlib（在 mscorlib.dll 中） 

        //Public Function IndexOf( ByVal value As Char ) As Integer

        //参数
        //value    类型:    System.Char    要查找的 Unicode 字符。

        //返回值  类型:     System.Int32   如果找到该字符,则为 value 的从零开始的索引位置；如果未找到,则为 -1。 
        //*******************************************************************

#endregion

#region "EPSON远程以太网控制命令响应说明"

        //如果控制器接收到正确的命令,将在执行命令时显示下列格式的响应。
        //---------------------------------------------------------------------------------------------------------------
        //命令                                            格式
        //---------------------------------------------------------------------------------------------------------------
        //获取数值的远程命令GetIO、
        //GetVariable和GetStatus 除外                     #[远程命令],[0]终端
        //---------------------------------------------------------------------------------------------------------------
        //GetIO                                           #GetIO,[0 | 1]终端
        //---------------------------------------------------------------------------------------------------------------
        //GetMemIO                                        #GetMemIO,[0 | 1]终端 *1
        //---------------------------------------------------------------------------------------------------------------
        //GetIOByte                                       #GetIOByte,[字节（8 位）的十六进制字符串（00到 FF）]终端
        //---------------------------------------------------------------------------------------------------------------
        //GetMemIOByte                                    #GetMemIOByte,[字节（8 位）的十六进制字符串（00 到 FF）]终端
        //---------------------------------------------------------------------------------------------------------------
        //GetIOWord                                       #GetIOWord,[字（16 位）的十六进制字符串（0000 至 FFFF）]终端
        //---------------------------------------------------------------------------------------------------------------
        //GetIOMemWord                                    #GetMemIOWord,[字（16 位）的十六进制字符串（0000 至 FFFF）]终端
        //---------------------------------------------------------------------------------------------------------------
        //GetVariable                                     #GetVariable,[参数值] 终端
        //---------------------------------------------------------------------------------------------------------------
        //GetVariable（如果是数组）                       #GetVariable,[参数值 1],[参数值 2],...,终端 *4
        //---------------------------------------------------------------------------------------------------------------
        //GetStatus                                       #GetStatus,[状态],[错误,警告代码]终端
        //                                                例如） #GetStatus,aaaaaaaaaa,bbbb
        //                                                                       *2     *3
        //---------------------------------------------------------------------------------------------------------------
        //Execute                                        如果作为命令执行的结果返回数值  #Execute,“[执行结果]” 终端
        //---------------------------------------------------------------------------------------------------------------

        //*1 [0 | 1] I/O 位 开：1/关：0
        //----------------------------------------------------------------------------------------
        //*2 状态
        //在上例中,10 位数字“aaaaaaaaaa”用于以下 10 个标志。
        //Test/Teach/Auto/Warning/SError/Safeguard/EStop/Error/Paused/Running/Ready 1 为开/0 为关
        //如果 Teach 和 Auto 为开,则为 1100000000。
        //----------------------------------------------------------------------------------------
        //*3 错误/警告代码
        //以 4 位数字表示。如果没有错误和警告,则为 0000。

        //例如）1： #GetStatus,0100000001,0000
        //Auto 位和 Ready 位为开（1）。
        //表示自动模式开启并处于准备就绪状态。已启用命令执行。

        //例如）2： #GetStatus,0110000010,0517
        //这意味着运行过程中发生警告。对警告代码采取适当的行动。（在这种情况下,警告代码为 0517）

        //标志                 内容
        //----------------------------------------------------------------------------------------
        //Test            在TEST模式下打开
        //----------------------------------------------------------------------------------------
        //Teach           在TEACH模式下打开
        //----------------------------------------------------------------------------------------
        //Auto            在远程输入接受条件下打开
        //----------------------------------------------------------------------------------------
        //Warnig          在警告条件下打开
        //                甚至在警告条件下也可以像往常一样执行任务。但是,应尽快采取警告行动。
        //----------------------------------------------------------------------------------------
        //SError          在严重错误状态下打开
        //                发生严重错误时,重新启动控制器,以便从错误状态中恢复。“Reset 输入”不可用。
        //----------------------------------------------------------------------------------------
        //Safeguard       安全门打开时打开
        //----------------------------------------------------------------------------------------
        //EStop           在紧急状态下打开
        //----------------------------------------------------------------------------------------
        //Error           在错误状态下打开
        //                使用“Reset 输入”从错误状态中恢复。
        //----------------------------------------------------------------------------------------
        //Paused          打开暂停的任务
        //----------------------------------------------------------------------------------------
        //Running         执行任务时打开
        //                在“Paused 输出”为开时关闭。
        //----------------------------------------------------------------------------------------
        //Ready           控制器完成启动且无任务执行时打开
        //----------------------------------------------------------------------------------------

        //*4 返回要获取的编号中指定编号的值。

        //*******************************************************************
        
#endregion

#region "EPSON远程以太网控制说明"

        //执行远程以太网控制
        //通过以下步骤可设置远程控制。
        //(1) 从客户端设备连接到控制器远程以太网中指定的端口上。
        //(2) 将远程以太网中设置的密码指定到该参数上并发送Login 命令。
        //(3) 执行远程命令前,客户端设备须等到Auto（GetStatus 命令响应）为ON 为止。
        //(4) 现在,远程命令将被接受。
        //每个命令执行输入接受条件的功能。

        //调试远程以太网控制
        //从EPSON RC+ 7.0 开发环境中调试程序的能力如下所述。
        //(1) 照例创建一个程序。
        //(2) 打开“运行”窗口,单击<Ethernet 启用>按钮。
        //如果您使用远程以太网控制只是获得了该值,则不显示<Ethernet 启用>按钮。单
        //击指定为控制装置的设备的<开始>按钮。
        //(3) 现在,远程命令将被接受。
        //“运行”窗口中的断点设置和输出是可用的。
        //如果5分钟内未从外部设备中Login,该连接将被自动切断。Login后,如果命令未在
        //远程以太网的超时时间内发送出去,连接将被切断。在这种情况下,重新建立连
        //接。
        //如果发生错误,执行操作命令之前请执行复位命令,以清除错误条件。若要通过监
        //控从外部设备上清除错误条件,执行“GetStatus”和“Reset”命令。

        //如果在(超时)框中设置“0”,则超时时间为无限。在这种情况下,该任务继续执
        //行,即使没有来自客户端的通信。这意味着机械手可能会继续移动并给设备造成
        //意外损坏。确保使用除通信以外的方式来停止该任务。

        //-----------*********************===================
        //远程以太网命令格式：$ 远程命令{, parameter....} 终端
        //---------------------------------------------------------------------------------------------------
        //远程命令            参数                   内容                                              输入接受条件
        //Login               密码                    启动控制器远程以太网功能   
        //                                           通过密码验证
        //                                           正确执行登录,并执行命令,直到退出                随时可用
        //---------------------------------------------------------------------------------------------------
        //Logout                                     退出控制器远程以太网功能
        //                                           退出登录后,执行登录命令来启动远程以太网功能。
        //                                           在任务执行期间退出会导致错误发生。                随时可用
        //---------------------------------------------------------------------------------------------------
        //Start               功能编号               执行指定编号的功能                                Auto开/Ready开/Error关/EStop关/Safeguard开
        //---------------------------------------------------------------------------------------------------
        //Stop                                       停止所有的任务和命令。                            Auto开
        //---------------------------------------------------------------------------------------------------
        //Pause                                      暂停所有任务                                      Auto 开/Running 开
        //---------------------------------------------------------------------------------------------------
        //Continue                                   继续暂停了的任务                                  Auto 开/Paused 开
        //---------------------------------------------------------------------------------------------------
        //Reset                                      清除紧急停止和错误                                Auto 开/Ready 开
        //---------------------------------------------------------------------------------------------------
        //SetMotorsOn         机械手编号             打开机械手电机                                    Auto开/Ready开/EStop关/Safeguard关
        //---------------------------------------------------------------------------------------------------
        //SetMotorsOff        机械手编号             关闭机械手电机                                    Auto开/Ready开
        //---------------------------------------------------------------------------------------------------
        //SetCurRobot         机械手编号             选择机械手                                        Auto开/Ready开
        //---------------------------------------------------------------------------------------------------
        //GetCurRobot                                获取当前的机械手编号                              随时可用
        //---------------------------------------------------------------------------------------------------
        //Home                机械手编号             将机械手手臂移动到由用户定义的起始点位置上        Auto开/Ready开/Error关/EStop关/Safeguard 关
        //---------------------------------------------------------------------------------------------------
        //GetIO               I/O位号                获得指定的I/O位                                   随时可用
        //---------------------------------------------------------------------------------------------------
        //SetIO               I/O位号和值            设置I/O指定的位
        //                                             1：打开此位
        //                                             0：关闭此位                                     Ready开
        //---------------------------------------------------------------------------------------------------
        //GetIOByte           I/O位号                获得指定的I/O端口（8位）                          随时可用
        //---------------------------------------------------------------------------------------------------
        //SetIOByte           I/O端口号和值          设置I/O指定的端口（8位）                          Ready开
        //---------------------------------------------------------------------------------------------------
        //GetIOWord           I/O字端口号            获得指定的I/O字端口（16位）                       随时可用
        //---------------------------------------------------------------------------------------------------
        //SetIOWord           I/O字端口号和值        设置I/O指定的端口（16位）                         Auto开/Ready开
        //---------------------------------------------------------------------------------------------------
        //GetMemIO            内存I/O位号            获得指定的内存I/O位                               随时可用
        //---------------------------------------------------------------------------------------------------
        //SetMemIO            内存I/O位号和值        设置指定的内存I/O位
        //                                               1: 打开此位
        //                                               0: 关闭此位                                   Auto开/Ready开
        //---------------------------------------------------------------------------------------------------
        //GetMemIOByte        内存I/O端口号          获得指定的内存I/O端口（8位）                      随时可用
        //---------------------------------------------------------------------------------------------------
        //SetMemIOByte        内存I/O端口号和值      设置指定的内存I/O端口（8位）                      Auto开/Ready开
        //---------------------------------------------------------------------------------------------------
        //GetMemIOWord        内存I/O字端口号        获得指定的内存I/O字端口（16位）                   随时可用
        //---------------------------------------------------------------------------------------------------
        //SetMemIOWord        内存I/O字端口号和值    设置指定的内存I/O字端口（16位）                   Auto开/Ready开
        //---------------------------------------------------------------------------------------------------
        //GetVariable         参数名称{, type}       获取备份（Global Preserve）参数的值               随时可用
        //                    ***********************************************************
        //                    (参数名称)（数组元
        //                    素）,(参数名称类
        //                    型),(获取的编号)      获取备份（Global Preserve）数组参数的值
        //---------------------------------------------------------------------------------------------------
        //SetVariable        参数名称和值{,类型}    设置备份（Global Preserve）参数中的值             Auto开/Ready开
        //---------------------------------------------------------------------------------------------------
        //GetStatus                                  获取控制器的状态                                  随时可用
        //---------------------------------------------------------------------------------------------------
        //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
        //---------------------------------------------------------------------------------------------------
        //Abort                                      中止命令的执行                                    Auto开
        //---------------------------------------------------------------------------------------------------

        //(*7) 如果“0”被指定为机械手编号,所有的机械手将进行操作。
        //如果您想操作具体的机械手,指定目标机械手的机械手编号（1到16）。
        //(*8) 参数类型是指{Boolean | Byte | Double | Integer | Long | Real | String}。
        //指定的类型： 在参数名称和类型相同时用于备份参数。
        //未指定的类型： 在参数名称相同时用于备份参数。
        //(*9) 对于数组元素,指定以下您想获取的元素：
        //如果是从数组头处获取的,您需要指定一个元素。
        //1维数组 参数名称 (0) 从头部获取。
        //参数名称（元素编号） 从指定的元素编号中获取。
        //2维数组 参数名称 (0,0) 从头部获取。
        //参数名称（元素编号1,2） 从指定的元素编号中获取。
        //3维数组 参数名称 (0,0,0) 从头部获取。
        //参数名称（元素编号1,2,3） 从指定的元素编号中获取。
        //您不能忽略要获取的参数类型和编号。
        //您不能指定一个参数类型string。
        //可获取的可用数量多达100个。如果您在数组元素编号上指定一个号码,会发生错误。
        //如）“$GetVariable,gby2(3,0),Byte,3”
        //它获得字节型2维数组参数gby2的gby2(3,0)、gby2(3,1)、gby2(3,2)的值。
        //(*10) 在双引号中指定命令和参数。
        //待执行的命令字符串和执行结果字符串被限制在4060字节。
        //机械手动作命令将被执行到所选的机械手上。执行命令之前检查使用GetCurRobot选中的机械手。

        //运行Execute时可用的命令
        //远程命令
        //Abort
        //GetStatus
        //SetIO
        //SetIOByte
        //SetIOWord
        //SetMemIO
        //SetMemIOByte
        //SetMemIOWord
        //Execute执行命令和输出命令
        //如果在（SetIO, SetIOByte, SetIOWord, SetMemIO, SetMemIOByte, SetMemIOWrod）
        //中指定的命令是相同的并且同时执行,后来执行的命令将导致错误。确保在执行
        //Execute命令和输出命令后使用正在执行Execute命令的GetStatus来检查执行结果。
        //(*11) 若要执行PCDaemon功能的命令,务必要在连接了RC+ 7.0时执行。如果未连
        //接RC+ 7.0,执行命令将会导致错误。

        //************************************************

        //$Execute,"print realpos"
        //#Execute," X: -182.632 Y:  240.114 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
        //$Execute,"move here +x(-1)"
        //#Execute,0
        //$Execute,"print realpos"
        //#Execute," X: -183.630 Y:  240.115 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
        //$Execute,"move here -x(1)"
        //#Execute,0
        //$Execute,"print realpos"
        //#Execute," X: -184.631 Y:  240.114 Z:  -20.002 U:  -36.155 V:    0.000 W:    0.000 /R /0

        //************************************************
        //【move here -x(1)和move here +x(1)的作用是一样的】
        //************************************************

#endregion

#region "已经处理好事项"

        //1、需要对进行点位运动的命令参数进行验证,验证正确后再执行,否则提示错误；【OK】
        //2、实例化时需要将相关参数进行验证；【OK】

        //6、添加错误处理代码；【OK】
        //7、添加GetStatus返回值得判断处理代码；【OK】
        //8、添加所有的返回值处理代码；【OK】
        //9、想办法将信息集中更新到新添加的RichTextBox中；【OK】

        //12、用FeedBackMessageFromRobot来保存从EPSON返回的任何结果,可在外部进行读写,公共变量；【OK】
        //13、execute,的后面不能有空格,否则就执行错误；【OK】

#endregion

#region "待处理事项"

        //3、对点位运动的命令添加可选参数,例如Z轴限位高度；【】
        //4、函数GetToolSetting不对,需要仔细检查或者如果EPSON SPEL语音中没有就去掉此函数；【已经处理了一部分,需要检查返回的值然后再修改程序】
        //5、添加导出点数据到Excel文件的代码；【】
        //10、在EPSON控制器中位、字节和字是从0开始,但是在给用户使用此DLL时,为避免误解,全部改为1开始,然后在相应函数里面全部-1；【】
        //11、需要矫正内存操作的参数值范围；【】
        //14、需要检查GET命令返回后的值处理是否正确，因为返回的命令里面最后有结束符；【OK】
        //15、添加LoadPoints;【OK】
        //16、添加十进制转换为2进制的代码；【】
        //17、添加设置机械手模式【自动/程序模式】的代码；【】
        //18、旋转U轴需要在 U 前面加上 “:” 号，否则报错，与X、Y、Z轴不同；【OK】
        //    【现在又报错，改方法：先获取当前位置，然后再绝对定位+U的角度】【OK】
        //19、因为在调试过程中出现EPSON断开远程服务器，所以改为Private NewClientForConnectRemoteEpson 
        //    As SimpleClientStation来与EPSON服务器建立通讯；待测试；【】
        //20、需要监控对比远程IO的实际IO信号的变化，与远程以太网相比较，哪些IO是无法手动控制的，是自动输出的；【】
        //21、GetOutByteStatus和GetOutWordStatus执行指令时报错；暂时设置为private【】
        //22、对各个点位运动的距离进行最大/最小值检查，避免超范围运动；【】

#endregion
        
#region "变量声明"

        private bool RobotWasPaused=false;

        /// <summary>
        /// EPSON机械手是否已经暂停
        /// </summary>
        public bool RobotPaused 
            {
            get { return RobotWasPaused; }
            }

        /// <summary>
        /// IO输入/输出信号的状态
        /// </summary>
        public enum IOStatus
            {
            IsOn,
            IsOff
            };

         /// <summary>
        /// 枚举:进行通讯时发送字符的结尾符号
        /// </summary>
        public enum Endings
            {

            /// <summary>
            /// 无
            /// </summary>
            None = 0,

            /// <summary>
            /// 换行符
            /// </summary>
            LF = 1,

            /// <summary>
            /// 回车符
            /// </summary>
            CR = 2,

            /// <summary>
            /// 回车换行符
            /// </summary>
            CRLF = 3

            }

        /// <summary>
        /// 结束符设置
        /// </summary>
        public Endings SuffixSetting
            {
            
            set
                {
                switch(value)
                    {
                    case Endings.None:
                        Suffix="";
                        break;

                    case Endings.CR:
                        Suffix="\r";
                        break;

                    case Endings.LF:
                        Suffix="\n";
                        break;

                    case Endings.CRLF:
                        Suffix="\r\n";
                        break;
                    }                
                }
            
            }

        private string Suffix="\r\n";

        private delegate void AddMessageToRichTextBox(string TargetText);
        string TempErrorMessage = "";

        private bool ExecutingBusy = false;
        
        /// <summary>
        /// EPSON机械手当前是否在执行指令中
        /// </summary>
        public bool ExecuteBusy 
            {
            get { return ExecutingBusy; }            
            }

        /// <summary>
        /// EPSON机械手的手势
        /// </summary>
        public enum Hand 
            {
            /// <summary>
            /// 左手势
            /// </summary>
            LeftHand,

            /// <summary>
            /// 右手势
            /// </summary>
            RightHand,

            /// <summary>
            /// 未知
            /// </summary>
            Unknow
            };

        /// <summary>
        /// EPSON机械手加速度数据结构
        /// </summary>
        public struct Accel
            {
            /// <summary>
            /// 加速度
            /// </summary>
            public int AccelSpeed;

            /// <summary>
            /// 减速度
            /// </summary>
            public int DecelSpeed;
            }

        /// <summary>
        /// EPSON机械手的功率
        /// </summary>
        public enum Power
            {
            /// <summary>
            /// 高功率
            /// </summary>
            High,

            /// <summary>
            /// 低功率
            /// </summary>
            Low,

            /// <summary>
            /// 未知
            /// </summary>
            Unknow
            };

        /// <summary>
        /// EPSON变量枚举
        /// </summary>
        public enum EpsonVariable
            {
            RCBoolean,
            RCByte,
            RCDouble,
            RCInteger,
            RCLong,
            RCReal,
            RCString,
            RCShort,
            RCUByte,
            RCUShort,
            RCInt32,
            RCUInt32,
            RCInt64,
            RCUInt64
            };
        
        /// <summary>
        /// EPSON机械手点数据结构
        /// </summary>
        public struct EpsonPoint
            {
            
            /// <summary>
            /// 轴X坐标值
            /// </summary>
            public double X;

            /// <summary>
            /// 轴Y坐标值
            /// </summary>
            public double Y;

            /// <summary>
            /// 轴Z坐标值
            /// </summary>
            public double Z;

            /// <summary>
            /// 轴U坐标值
            /// </summary>
            public double U;

            /// <summary>
            /// 轴V坐标值
            /// </summary>
            public double V;

            /// <summary>
            /// 轴W坐标值
            /// </summary>
            public double W;

            /// <summary>
            /// 手势
            /// </summary>
            public Hand HandStyle;
            
            };

        /// <summary>
        /// EPSON机械手状态、点坐标、手势、速度、数据结构
        /// </summary>
        public struct EpsonData
            {
            //Test/Teach/Auto/Warning/SError/Safeguard/EStop/Error/Paused/Running/Ready 
            //1 为开/0 为关
            /// <summary>
            /// 测试
            /// </summary>
            public bool Test;

            /// <summary>
            /// 示教
            /// </summary>
            public bool Teach;

            /// <summary>
            /// 自动
            /// </summary>
            public bool Auto;

            /// <summary>
            /// 报警
            /// </summary>
            public bool Warning;

            /// <summary>
            /// 严重错误
            /// </summary>
            public bool SError;

            /// <summary>
            /// 安全保护
            /// </summary>
            public bool Safeguard;

            /// <summary>
            /// 急停
            /// </summary>
            public bool EStop;

            /// <summary>
            /// 错误
            /// </summary>
            public bool Error;

            /// <summary>
            /// 暂停
            /// </summary>
            public bool Paused;

            /// <summary>
            /// 运行中
            /// </summary>
            public bool Running;

            /// <summary>
            /// 准备好
            /// </summary>
            public bool Ready;

            /// <summary>
            /// 轴X坐标值
            /// </summary>
            public double X;

            /// <summary>
            /// 轴Y坐标值
            /// </summary>
            public double Y;

            /// <summary>
            /// 轴Z坐标值
            /// </summary>
            public double Z;

            /// <summary>
            /// 轴U坐标值
            /// </summary>
            public double U;

            /// <summary>
            /// 轴V坐标值
            /// </summary>
            public double V;

            /// <summary>
            /// 轴W坐标值
            /// </summary>
            public double W;

            /// <summary>
            /// 手势
            /// </summary>
            public Hand HandStyle;

            /// <summary>
            /// 功率
            /// </summary>
            public Power PowerStatus;

            /// <summary>
            /// 速度
            /// </summary>
            public  ushort Speed;

            /// <summary>
            /// 加速度
            /// </summary>
            public ushort AccelSpeed;

            /// <summary>
            /// 减速度
            /// </summary>
            public ushort DecelSpeed;

            };

        /// <summary>
        /// EPSON机械手状态数据结构
        /// </summary>
        public struct EpsonStatusBits
            {
            //Test/Teach/Auto/Warning/SError/Safeguard/EStop/Error/Paused/Running/Ready 
            //1 为开/0 为关
            /// <summary>
            /// 测试
            /// </summary>
            public bool Test;

            /// <summary>
            /// 示教
            /// </summary>
            public bool Teach;

            /// <summary>
            /// 自动
            /// </summary>
            public bool Auto;

            /// <summary>
            /// 报警
            /// </summary>
            public bool Warning;

            /// <summary>
            /// 严重错误
            /// </summary>
            public bool SError;

            /// <summary>
            /// 安全保护
            /// </summary>
            public bool Safeguard;

            /// <summary>
            /// 急停
            /// </summary>
            public bool EStop;

            /// <summary>
            /// 错误
            /// </summary>
            public bool Error;

            /// <summary>
            /// 暂停
            /// </summary>
            public bool Paused;

            /// <summary>
            /// 运行中
            /// </summary>
            public bool Running;

            /// <summary>
            /// 准备好
            /// </summary>
            public bool Ready;

            };

        private bool SuccessBuiltNew = false, PasswordIsCorrect = false;

        private RichTextBox rtbShowMessageViaDelegate = null;

        /// <summary>
        /// 是否需要通过代理和委托更新显示信息到RichTextBox【外部传入】
        /// </summary>
        private bool ShowMessageInRichTextBoxViaDelegate = false;

        //***********************************
        /// <summary>
        /// 连接EPSON机械手进行远程以太网通讯
        /// </summary>
        private TcpClient ClientConnectToRemoteEpson;

        /// <summary>
        /// 连接EPSON机械手进行远程以太网通讯
        /// </summary>
        private NetworkStream StreamConnectToRemoteEpson;

        /// <summary>
        /// 连接EPSON机械手进行远程以太网通讯
        /// </summary>
        private Thread ThreadConnectRemoteEpsonTCPIP;
        //***********************************

        private SimpleTCPIPClient ClientConnectToRobot;
        private SimpleTCPIPServer ServerAcceptConnectionFromRobot;
        //***********************************

        private ushort EpsonRemoteControlPort=0;
        private string EpsonRemoteControlIP = "";

        /// <summary>
        /// 是否已经与EPSON机械手控制器建立远程以太网通讯
        /// </summary>
        public bool ConnectedWithRemoteEpson 
            {
            get 
                {
                if (ClientConnectToRemoteEpson != null) 
                    {
                    if (ClientConnectToRemoteEpson.Connected == true)
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
            }

        private bool ConnectRemoteEpsonTCPIPOk=false;

        /// <summary>
        /// Excel软件操作接口
        /// </summary>
        public Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();

        /// <summary>
        /// Excel工作薄操作接口
        /// </summary>
        public Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;// = new Microsoft.Office.Interop.Excel.Workbook();

        /// <summary>
        /// Excel工作表操作接口
        /// </summary>
        public Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;// = new Microsoft.Office.Interop.Excel.Worksheet();

        private bool ConnectedRemoteEpsonTCPIP = false;

        /// <summary>
        /// 是否更新显示与EPSON通讯的相关信息
        /// </summary>
        public bool ShowMessage = false;

        /// <summary>
        /// 当错误信息相同时,是否重复显示
        /// </summary>
        public bool UpdatingSameMessage = true;

        private string strLoginRobotPassword;//strStartTime, strEndTime, strResult,

        /// <summary>
        /// 从EPSON机械手返回的信息
        /// </summary>
        public string FeedBackMessageFromRobot = "";

        public string ErrorMessage = "";

        ListViewOperation LV = new ListViewOperation("彭东南");
        Microsoft.VisualBasic.Devices.Computer PC = new Microsoft.VisualBasic.Devices.Computer();
        CommonFunction FC = new CommonFunction("彭东南");

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }

#endregion

#region "程序代码"

        //建立新实例【利用EPSON机械手的IP地址、远程以太网控制端口号、登录密码建立远程以太网控制通讯】
        /// <summary>
        /// 建立新实例【利用EPSON机械手的IP地址、远程以太网控制端口号、登录密码建立远程以太网控制通讯】
        /// </summary>
        /// <param name="DLLPassword">使用此DLL文件的密码</param>
        /// <param name="IPAddressOfEpsonRobot">EPSON机械手控制器的IP地址</param>
        /// <param name="PortOfRemoteEpson">EPSON远程控制的端口号</param>
        /// <param name="RobotLoginPassword">EPSON机械手控制器登录密码</param>
        /// <param name="PortOfEpsonServer">EPSON普通TCP/IP通讯的监听端口号</param>
        public EpsonRemoteTCP(string DLLPassword, string IPAddressOfEpsonRobot,
            ushort PortOfRemoteEpson, string RobotLoginPassword,
            ushort PortOfEpsonServer)
            {

            try
                {

                SuccessBuiltNew = false;
                PasswordIsCorrect = false;

                if (IPAddressOfEpsonRobot == "") 
                    {
                    MessageBox.Show("The IP address of TCP/IP Server can't be set as empty, please revise it.\r\n" +
                           "服务器的IP地址不能为空,请修改.");
                    return;
                    }

                //判断输入的服务器IP地址是否正确
                if (FC.VerifyIPAddressNew(IPAddressOfEpsonRobot) == false) 
                    {
                    MessageBox.Show("The format of IP address for the TCP/IP Server is not correct, please check and correct it."
                        + "\r\n服务器IP地址格式错误,请检查后输入正确IP地址再重新建立新实例.");
                    return;
                    }

                if (DLLPassword == "ThomasPeng"
                    || (DLLPassword == "pengdongnan")
                    || (DLLPassword == "彭东南"))
                    {

                    PasswordIsCorrect = true;

                    EpsonRemoteControlIP = IPAddressOfEpsonRobot;
                    EpsonRemoteControlPort = PortOfRemoteEpson;
                    strLoginRobotPassword = RobotLoginPassword;

                    ThreadConnectRemoteEpsonTCPIP = new Thread(ConnectRemoteEpsonTCPIP);
                    ThreadConnectRemoteEpsonTCPIP.IsBackground = true;
                    ThreadConnectRemoteEpsonTCPIP.Start();
                    
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
                MessageBox.Show("创建类的实例时出现错误！" + ex.Message);
                return;
                }
            
            }

        //建立新实例【利用EPSON机械手的IP地址、远程以太网控制端口号、登录密码建立远程以太网控制通讯】
        /// <summary>
        /// 建立新实例【利用EPSON机械手的IP地址、远程以太网控制端口号、登录密码建立远程以太网控制通讯】
        /// </summary>
        /// <param name="DLLPassword">使用此DLL文件的密码</param>
        /// <param name="IPAddressOfEpsonRobot">EPSON机械手控制器的IP地址</param>
        /// <param name="PortOfRemoteEpson">EPSON机械手控制器的远程以太网控制端口号</param>
        /// <param name="RobotLoginPassword">EPSON机械手控制器登录密码</param>
        public EpsonRemoteTCP(string DLLPassword, string IPAddressOfEpsonRobot,
            ushort PortOfRemoteEpson, string RobotLoginPassword)
            {

            try
                {

                SuccessBuiltNew = false;
                PasswordIsCorrect = false;

                if (IPAddressOfEpsonRobot == "")
                    {
                    MessageBox.Show("The IP address of TCP/IP Server can't be set as empty, please revise it.\r\n" +
                           "服务器的IP地址不能为空,请修改.");
                    return;
                    }

                //判断输入的服务器IP地址是否正确
                if (FC.VerifyIPAddressNew(IPAddressOfEpsonRobot) == false)
                    {
                    MessageBox.Show("The format of IP address for the TCP/IP Server is not correct, please check and correct it."
                        + "\r\n服务器IP地址格式错误,请检查后输入正确IP地址再重新建立新实例.");
                    return;
                    }

                if (DLLPassword == "ThomasPeng"
                    || (DLLPassword == "pengdongnan")
                    || (DLLPassword == "彭东南"))
                    {

                    PasswordIsCorrect = true;

                    EpsonRemoteControlIP = IPAddressOfEpsonRobot;
                    EpsonRemoteControlPort = PortOfRemoteEpson;
                    strLoginRobotPassword = RobotLoginPassword;

                    ThreadConnectRemoteEpsonTCPIP = new Thread(ConnectRemoteEpsonTCPIP);
                    ThreadConnectRemoteEpsonTCPIP.IsBackground = true;
                    ThreadConnectRemoteEpsonTCPIP.Start();

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
                MessageBox.Show("创建类的实例时出现错误！" + ex.Message);
                return;
                }

            }

        //建立新实例【利用EPSON机械手的IP地址、远程以太网控制端口号、登录密码建立远程以太网控制通讯】
        /// <summary>
        /// 建立新实例【利用EPSON机械手的IP地址、远程以太网控制端口号、登录密码建立远程以太网控制通讯】
        /// </summary>
        /// <param name="DLLPassword">使用此DLL文件的密码</param>
        /// <param name="IPAddressOfEpsonRobot">EPSON机械手控制器的IP地址</param>
        /// <param name="PortOfRemoteEpson">EPSON远程控制的端口号</param>
        /// <param name="RobotLoginPassword">EPSON机械手控制器登录密码</param>
        /// <param name="TargetRichTextBoxShowMessage">需要显示更新信息的RichTextBox控件</param>
        public EpsonRemoteTCP(string DLLPassword, string IPAddressOfEpsonRobot,
            ushort PortOfRemoteEpson, string RobotLoginPassword,
            ref RichTextBox TargetRichTextBoxShowMessage)
            {

            try
                {

                SuccessBuiltNew = false;
                PasswordIsCorrect = false;

                if (IPAddressOfEpsonRobot == "")
                    {
                    MessageBox.Show("The IP address of TCP/IP Server can't be set as empty, please revise it.\r\n" +
                           "服务器的IP地址不能为空,请修改.");
                    return;
                    }

                //判断输入的服务器IP地址是否正确
                if (FC.VerifyIPAddressNew(IPAddressOfEpsonRobot) == false)
                    {
                    MessageBox.Show("The format of IP address for the TCP/IP Server is not correct, please check and correct it."
                        + "\r\n服务器IP地址格式错误,请检查后输入正确IP地址再重新建立新实例.");
                    return;
                    }

                if (DLLPassword == "ThomasPeng"
                    || (DLLPassword == "pengdongnan")
                    || (DLLPassword == "彭东南"))
                    {

                    PasswordIsCorrect = true;

                    EpsonRemoteControlIP = IPAddressOfEpsonRobot;
                    EpsonRemoteControlPort = PortOfRemoteEpson;
                    strLoginRobotPassword = RobotLoginPassword;
                    rtbShowMessageViaDelegate = TargetRichTextBoxShowMessage;
                    ShowMessageInRichTextBoxViaDelegate = true;

                    ThreadConnectRemoteEpsonTCPIP = new Thread(ConnectRemoteEpsonTCPIP);
                    ThreadConnectRemoteEpsonTCPIP.IsBackground = true;
                    ThreadConnectRemoteEpsonTCPIP.Start();

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
                MessageBox.Show("创建类的实例时出现错误！" + ex.Message);
                return;
                }

            }

        //利用委托和代理进行跨线程操作富文本控件添加内容 [含信息发生的日期和时间]，以此避免跨线程操作异常
        /// <summary>
        /// 利用委托和代理进行跨线程操作富文本控件添加内容 [含信息发生的日期和时间]，以此避免跨线程操作异常
        /// </summary>
        /// <param name="TargetText">要添加的文本内容</param>
        /// <returns>是否执行成功</returns>
        private void AddText(string TargetText)
            {

            if(rtbShowMessageViaDelegate==null)
                {
                return;
                }

            if (PasswordIsCorrect == false)
                {
                MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "非法操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
                }

            try
                {

                if (TargetText == "")
                    {
                    return;
                    }

                string TempStr = "";

                //**********************
                // 去掉尾部的回车换行符号
                if (TargetText.Length == 1)
                    {
                    TempStr = TargetText;
                    }
                else if (TempStr.Length == 2)
                    {
                    if ((Strings.Asc(Strings.Mid(TargetText, TargetText.Length, 1)) == 13) |
                        (Strings.Asc(Strings.Mid(TargetText, TargetText.Length, 1)) == 10))
                        {
                        TempStr = Strings.Mid(TargetText, 1, TargetText.Length - 1);
                        }
                    else
                        {
                        TempStr = TargetText;
                        }
                    }
                else
                    {
                    if ((Strings.Asc(Strings.Mid(TargetText, TargetText.Length - 1, 1)) == 13) &
                    (Strings.Asc(Strings.Mid(TargetText, TargetText.Length, 1)) == 10))
                        {
                        TempStr = Strings.Mid(TargetText, 1, TargetText.Length - 2);
                        }
                    else
                        {
                        TempStr = TargetText;
                        }
                    }
                TargetText = "";

                //**********************
                if (rtbShowMessageViaDelegate.InvokeRequired == true)
                    {
                    AddMessageToRichTextBox ActualAddMessageToRichTextBox = new AddMessageToRichTextBox(AddText);
                    rtbShowMessageViaDelegate.Invoke(ActualAddMessageToRichTextBox, new object[] { TempStr });
                    }
                else
                    {

                    if (UpdatingSameMessage == true)
                        {
                        TempErrorMessage = TempStr;
                        //TargetRichTextBox.AppendText(DateTime.Now + "   " + TempStr + "\r\n");
                        //if (ShowDateTimeForMessage == true)
                        //    {
                        rtbShowMessageViaDelegate.AppendText(DateTime.Now + "   " + TempStr + "\r\n");
                        //    }
                        //else
                        //    {
                        //TargetRichTextBox.AppendText(TempStr + "\r\n");
                            //}
                        TempStr = "";
                        }
                    else
                        {

                        if (TempErrorMessage == TempStr)
                            {
                            return;
                            }
                        else
                            {
                            TempErrorMessage = TempStr;
                            //TargetRichTextBox.AppendText(DateTime.Now + "   " + TempStr + "\r\n");
                            //if (ShowDateTimeForMessage == true)
                            //    {
                            rtbShowMessageViaDelegate.AppendText(DateTime.Now + "   " + TempStr + "\r\n");
                            //    }
                            //else
                            //    {
                            //TargetRichTextBox.AppendText(TempStr + "\r\n");
                                //}
                            TempStr = "";
                            }

                        }

                    }
                //**********************

                }
            catch (Exception)// ex)
                {
                //MessageBox.Show(ex.Message);
                return;
                }

            }

        string strReceived="";

        //发送命令到EPSON机械手
        /// <summary>
        /// 发送命令到EPSON机械手
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public string SendCommand(string Command) 
            {

            try
                {

                if(Command=="")
                    {
                    if(ShowMessageInRichTextBoxViaDelegate==true)
                        {
                        //FC.AddRichText(ref rtbShowMessageViaDelegate,"The parameter 'Command' is empty, please send correct command.");
                        AddText("The parameter 'Command' is empty, please send correct command.");
                        }
                    return "";
                    }

                if(ConnectRemoteEpsonTCPIPOk==false)
                    {
                    if(ShowMessageInRichTextBoxViaDelegate==true)
                        {
                        //FC.AddRichText(ref rtbShowMessageViaDelegate,"Haven't connected to the remote EPSON robot via TCP/IP yet, please check the reason and retry.");
                        AddText("Haven't connected to the remote EPSON robot via TCP/IP yet, please check the reason and retry.");
                        }
                    return "";
                    }

                if(ClientConnectToRemoteEpson==null)
                    {
                    return "";
                    }

                if(StreamConnectToRemoteEpson==null)
                    {
                    return "";
                    }

                byte[] BytesReceived=new byte[ClientConnectToRemoteEpson.ReceiveBufferSize];
                byte[] SendBytes=new byte[ClientConnectToRemoteEpson.SendBufferSize];

                //发送
                SendBytes=System.Text.Encoding.ASCII.GetBytes(Command  + Suffix);                
                StreamConnectToRemoteEpson.Write(SendBytes,0,SendBytes.Length);
                AddText("Sent to Epson: " + Command  + Suffix);
                strReceived = "";

                //发送完之后接收EPSON返回内容
                FeedBackMessageFromRobot = "";
                int i = StreamConnectToRemoteEpson.Read(BytesReceived,0,ClientConnectToRemoteEpson.ReceiveBufferSize);
                FeedBackMessageFromRobot = System.Text.Encoding.ASCII.GetString(BytesReceived, 0, i);
                strReceived = FeedBackMessageFromRobot;

                AddText("Got from Epson: " + strReceived);
                return strReceived;

                }
            catch (Exception ex) 
                {
                //MessageBox.Show(ex.Message);
                AddText(ex.Message);
                return strReceived;
                }
            
            }

        //登录EPSON机械手控制系统的远程以太网通讯
        /// <summary>
        /// 登录EPSON机械手控制系统的远程以太网通讯
        /// </summary>
        /// <param name="LoginRobotPassword"></param>
        /// <returns></returns>
        public bool Login(string LoginRobotPassword)
            {

            try
                {

            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Login    密码       启动控制器远程以太网功能   
            //                    通过密码验证
            //                    正确执行登录,并执行命令,直到退出          随时可用
            //----------------------------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                FeedBackMessageFromRobot = SendCommand("$Login," + LoginRobotPassword + Suffix);
                if(FeedBackMessageFromRobot.IndexOf("!")==-1)
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //退出与EPSON机械手控制系统的远程以太网通讯
        /// <summary>
        /// 退出与EPSON机械手控制系统的远程以太网通讯
        /// </summary>
        /// <returns></returns>
        public bool Logout()
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Logout    退出控制器远程以太网功能
            //          退出登录后,执行登录命令来启动远程以太网功能。
            //          在任务执行期间退出会导致错误发生。                随时可用
            //--------------------------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                FeedBackMessageFromRobot = SendCommand("$Logout" + Suffix);
                if(FeedBackMessageFromRobot.IndexOf("!")==-1)
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //启动EPSON机械手指定编号的任务
        /// <summary>
        /// 启动EPSON机械手指定编号的任务
        /// </summary>
        /// <param name="NumberOfMission">任务编号【0~7】</param>
        /// <returns></returns>
        public bool StartMission(int NumberOfMission)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Start               功能编号               执行指定编号的功能                                Auto开/Ready开/Error关/EStop关/Safeguard开
            //--------------------------------------------------------------

                   if(NumberOfMission<0 | NumberOfMission >7)
                    {
                    AddText("The value of parameter 'NoOfMission' must be 0 to 7 according to the mission number in Epson program, please correct it and retry.");
                    MessageBox.Show("The value of parameter 'NoOfMission' must be 0 to 7 according to the mission number in Epson program, please correct it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if(FeedBackMessageFromRobot[i + 3]=='1' 
                    & FeedBackMessageFromRobot[i + 11]=='1' )
                    {
                    FeedBackMessageFromRobot = SendCommand("$Start," + NumberOfMission + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                if(FeedBackMessageFromRobot.IndexOf("!")==-1)
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //获取机械手的状态: 此函数已经包括在其他函数里面，所以不能添加ExecutingBusy
        /// <summary>
        /// 获取机械手的状态
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool GetRobotStatus(ref string Status)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //GetStatus     获取控制器的状态                                  随时可用
            //-----------------------------------------------------------------------
            //GetStatus      #GetStatus,[状态],[错误,警告代码]终端
            //               例如） #GetStatus,aaaaaaaaaa,bbbb
            //                       *2     *3
            //------------------------------------------------------------------------

                FeedBackMessageFromRobot = SendCommand("$GetStatus" + Suffix);

                //#GetStatus,00100000001,0000
                //Test/Teach/Auto/Warning/SError/Safeguard/EStop/Error/Paused/Running/Ready 1 为开/0 为关
                if(FeedBackMessageFromRobot.IndexOf("!")==-1)
                    {
                    string [] TempStr;
                    try
                        {
                        TempStr = Strings.Split(FeedBackMessageFromRobot, ",");
                        if(TempStr.Length==3)
                            {
                            Status = TempStr[1];
                            return true;
                            }
                        else
                            {
                            Status=FeedBackMessageFromRobot;
                            return false;
                            }
                        }
                    catch(Exception ex)
                        {
                        AddText(ex.Message);
                        return false;
                        }
                    }
                else
                    {
                    Status = ProcessResponse(FeedBackMessageFromRobot);
                    return false;                    
                    }
                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //处理EPSON控制器返回的执行结果，从而判断是成功执行或者是什么错误原因
        /// <summary>
        /// 处理EPSON控制器返回的执行结果，从而判断是成功执行或者是什么错误原因
        /// </summary>
        /// <param name="ResponseString">EPSON控制器返回的执行结果</param>
        /// <returns></returns>
        public string ProcessResponse(string ResponseString)
            {

            string TempResponse="", TempStr="";

            try
                {
            //错误响应
            //如果控制器不能正确接收远程命令,将以下列格式显示错误响应。
            //格式：![远程命令],[错误代码]终端

                if(Strings.InStr(ResponseString, "!") == -1 )
                    {
                    AddText(ResponseString + "   " + "成功执行完远程命令");
                    return ResponseString + "   " + "成功执行完远程命令";
                    }
                else
                    {
                    ushort ResponseCode=0;
                    string[] StrArray;
                    StrArray = Strings.Split(ResponseString, ",");
                    ResponseCode = Convert.ToUInt16(StrArray[1]);

                    switch(ResponseCode)
                        {
                        case 10:
                            //10              远程命令未以$开头
                            TempResponse = ResponseString + "   " + "远程命令未以$开头";
                            break;

                        case 11:
                            //11              远程命令错误 / 未执行Login
                            TempResponse = ResponseString + "   " + "远程命令错误 / 未执行Login";
                            break;

                        case 12:
                            //12              远程命令格式错误
                            TempResponse = ResponseString + "   " + "远程命令格式错误";
                            break;

                        case 13:
                            //13              Login命令密码错误
                            TempResponse = ResponseString + "   " + "Login命令密码错误";
                            break;

                        case 14:
                            //14              要获取的指定数量超出范围(小于1或大于100) / 忽略了要获取的数量 / 指定了一个字符串参数
                            TempResponse = ResponseString + "   " + "要获取的指定数量超出范围(小于1或大于100) / 忽略了要获取的数量 / 指定了一个字符串参数";
                            break;

                        case 15:
                            //15              参数不存在 / 参数尺寸错误 / 调用了超出了范围的元素
                            TempResponse = ResponseString + "   " + "参数不存在 / 参数尺寸错误 / 调用了超出了范围的元素";
                            break;

                        case 19:
                            //19              请求超时
                            TempResponse = ResponseString + "   " + "请求超时";
                            break;

                        case 20:
                            //20              控制器未准备好
                            TempResponse = ResponseString + "   " + "控制器未准备好";
                            break;

                        case 21:
                            //21              因为正在运行Execute,所以无法执行
                            TempResponse = ResponseString + "   " + "因为正在运行Execute,所以无法执行";
                            break;

                        case 99:
                            //99              系统错误 / 通信错误
                            TempResponse = ResponseString + "   " + "系统错误 / 通信错误";
                            break;

                        default:
                            TempResponse = ResponseString + "   " + "未知错误";
                            break;
                        }
                    }
                TempStr = TempResponse;
                AddText(TempStr);
                return TempResponse;
                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return "";
                //MessageBox.Show(ex.Message);
                }

            }

        //获取当前位置的坐标值
        /// <summary>
        /// 获取当前位置的坐标值
        /// </summary>
        /// <param name="CurrentX">当前位置的 X 坐标值</param>
        /// <param name="CurrentY">当前位置的 Y 坐标值</param>
        /// <param name="CurrentZ">当前位置的 Z 坐标值</param>
        /// <param name="CurrentU">当前位置的 U 坐标值</param>
        /// <param name="CurrentV">当前位置的 V 坐标值</param>
        /// <param name="CurrentW">当前位置的 W 坐标值</param>
        /// <param name="CurrentHand">当前位置的坐标手势</param>
        /// <returns></returns>
        public bool GetCurrentPos(ref double CurrentX, ref double CurrentY,
            ref double CurrentZ, ref double CurrentU, ref double CurrentV,
            ref double CurrentW, ref Hand CurrentHand)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
            //----------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if(FeedBackMessageFromRobot[i + 3]=='1' 
                    & FeedBackMessageFromRobot[i + 11]=='1' )
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Print RealPos\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

             //$Execute,"Print RealPos"
             //#Execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0

             //$execute,"print curpos"
             //#execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {

                    try
                        {

                        i = FeedBackMessageFromRobot.IndexOf(":", 0);
                        CurrentX =Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText("X: " + CurrentX);

                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        CurrentY = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText("Y: " + CurrentY);
                        
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        CurrentZ = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText("Z: " + CurrentZ);
                        
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        CurrentU = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText("U: " + CurrentU);
                        
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        CurrentV = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText("V: " + CurrentV);
                        
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        CurrentW = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText("W: " + CurrentW);

                        //判断左右手势
                        if(FeedBackMessageFromRobot.ToUpper().IndexOf("/L")!=-1)
                            {
                            CurrentHand = Hand.LeftHand;
                            AddText("Left Hand");
                            }
                        
                        if(FeedBackMessageFromRobot.ToUpper().IndexOf("/R")!=-1)
                            {
                            CurrentHand = Hand.RightHand;
                            AddText("Right Hand");
                            }

                        }
                    catch(Exception ex)
                        {
                        AddText(ex.Message);
                        ExecutingBusy=false;
                        return false;
                        }

                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //获取当前位置的坐标值
        /// <summary>
        /// 获取当前位置的坐标值
        /// </summary>
        /// <param name="EpsonPointData">EPSON点数据结构</param>
        /// <returns></returns>
        public bool GetCurrentPos(ref EpsonPoint EpsonPointData)
            {

            try
                {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
                //----------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1'
                    & FeedBackMessageFromRobot[i + 11] == '1')
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Print RealPos\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                //$Execute,"Print RealPos"
                //#Execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0

                //$execute,"print curpos"
                //#execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {

                    try
                        {

                        i = FeedBackMessageFromRobot.IndexOf(":", 0);
                        EpsonPointData.X = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText("X: " + EpsonPointData.X);

                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        EpsonPointData.Y = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText("Y: " + EpsonPointData.Y);

                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        EpsonPointData.Z = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText("Z: " + EpsonPointData.Z);

                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        EpsonPointData.U = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText("U: " + EpsonPointData.U);

                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        EpsonPointData.V = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText("V: " + EpsonPointData.V);

                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        EpsonPointData.W = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText("W: " + EpsonPointData.W);

                        //判断左右手势
                        if (FeedBackMessageFromRobot.ToUpper().IndexOf("/L") != -1)
                            {
                            EpsonPointData.HandStyle = Hand.LeftHand;
                            AddText("Left Hand");
                            }

                        if (FeedBackMessageFromRobot.ToUpper().IndexOf("/R") != -1)
                            {
                            EpsonPointData.HandStyle = Hand.RightHand;
                            AddText("Right Hand");
                            }

                        }
                    catch (Exception ex)
                        {
                        AddText(ex.Message);
                        ExecutingBusy = false;
                        return false;
                        }

                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //获取某个点位的坐标值
        /// <summary>
        /// 获取某个点位的坐标值
        /// </summary>
        /// <param name="PointName">EPSON坐标点名称【P0~P999】</param>
        /// <param name="XPos">点 X 坐标</param>
        /// <param name="YPos">点 Y 坐标</param>
        /// <param name="ZPos">点 Z 坐标</param>
        /// <param name="UPos">点 U 坐标</param>
        /// <param name="VPos">点 V 坐标</param>
        /// <param name="WPos">点 W 坐标</param>
        /// <param name="HandStyle">点手势</param>
        /// <returns></returns>
        public bool GetPointPos(string PointName, ref double XPos, ref double YPos,
            ref double ZPos, ref double UPos, ref double VPos, ref double WPos,
            ref Hand HandStyle)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令     Auto开/Ready开/Error关/EStop关/Safeguard关
            //----------------------------------------------------

                //如果PointName的第一个字母不是P或者数字大于999或者小于0
                if(Strings.Mid(PointName.ToUpper(),1,1)!="P"
                    | (Convert.ToUInt16(Strings.Mid(PointName.ToUpper(),2, PointName.Length - 1)) < 0 
                    & Convert.ToUInt16(Strings.Mid(PointName.ToUpper(),2, PointName.Length - 1)) > 999))
                    {
                    AddText("参数''不正确，正确范围是P0~P999");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1' 
                    & FeedBackMessageFromRobot[i + 11]=='1' )
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Print " 
                        + PointName + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                //$Execute,"Print RealPos"
             //#Execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0

             //$execute,"print curpos"
             //#execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {

                    try
                        {

                        i = FeedBackMessageFromRobot.IndexOf(":", 0);
                        XPos =Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText(PointName + " X: " + XPos);

                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        YPos = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText(PointName + " Y: " + YPos);
                        
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        ZPos = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText(PointName + " Z: " + ZPos);
                        
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        UPos = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText(PointName + " U: " + UPos);
                        
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        VPos = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText(PointName + " V: " + VPos);
                        
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        WPos = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText(PointName + " W: " + WPos);

                        //判断左右手势
                        if(FeedBackMessageFromRobot.ToUpper().IndexOf("/L")!=-1)
                            {
                            HandStyle = Hand.LeftHand;
                            AddText(PointName + " Left Hand");
                            }
                        
                        if(FeedBackMessageFromRobot.ToUpper().IndexOf("/R")!=-1)
                            {
                            HandStyle = Hand.RightHand;
                            AddText(PointName + " Right Hand");
                            }

                        }
                    catch(Exception ex)
                        {
                        AddText(ex.Message);
                        ExecutingBusy=false;
                        return false;
                        }

                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //获取某个点位的坐标值
        /// <summary>
        /// 获取某个点位的坐标值
        /// </summary>
        /// <param name="PointName">EPSON坐标点名称【P0~P999】</param>
        /// <param name="PointData">返回的点数据结构</param>
        /// <returns></returns>
        public bool GetPointPos(string PointName, ref EpsonPoint PointData)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令     Auto开/Ready开/Error关/EStop关/Safeguard关
            //----------------------------------------------------

                //如果PointName的第一个字母不是P或者数字大于999或者小于0
                if(Strings.Mid(PointName.ToUpper(),1,1)!="P"
                    | (Convert.ToUInt16(Strings.Mid(PointName.ToUpper(),2, PointName.Length - 1)) < 0 
                    & Convert.ToUInt16(Strings.Mid(PointName.ToUpper(),2, PointName.Length - 1)) > 999))
                    {
                    AddText("参数''不正确，正确范围是P0~P999");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1' 
                    & FeedBackMessageFromRobot[i + 11]=='1' )
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Print " 
                        + PointName + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                //$Execute,"Print RealPos"
             //#Execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0

             //$execute,"print curpos"
             //#execute," X: -181.629 Y:  240.117 Z:  -20.002 U:  -36.154 V:    0.000 W:    0.000 /R /0
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {

                    try
                        {

                        i = FeedBackMessageFromRobot.IndexOf(":", 0);
                        PointData.X =Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText(PointName + " X: " + PointData.X);

                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        PointData.Y = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText(PointName + " Y: " + PointData.Y);
                        
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        PointData.Z = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText(PointName + " Z: " + PointData.Z);
                        
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        PointData.U = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText(PointName + " U: " + PointData.U);
                        
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        PointData.V = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText(PointName + " V: " + PointData.V);
                        
                        i = FeedBackMessageFromRobot.IndexOf(":", i + 1);
                        PointData.W = Conversion.Val(FeedBackMessageFromRobot.Substring(i + 1, 9));
                        AddText(PointName + " W: " + PointData.W);

                        //判断左右手势
                        if(FeedBackMessageFromRobot.ToUpper().IndexOf("/L")!=-1)
                            {
                            PointData.HandStyle = Hand.LeftHand;
                            AddText(PointName + " Left Hand");
                            }
                        
                        if(FeedBackMessageFromRobot.ToUpper().IndexOf("/R")!=-1)
                            {
                            PointData.HandStyle = Hand.RightHand;
                            AddText(PointName + " Right Hand");
                            }

                        }
                    catch(Exception ex)
                        {
                        AddText(ex.Message);
                        ExecutingBusy=false;
                        return false;
                        }

                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //设置某个点位的坐标值
        /// <summary>
        /// 设置某个点位的坐标值
        /// </summary>
        /// <param name="PointName">EPSON坐标点名称【P0~P999】</param>
        /// <param name="NewX">新 X 坐标</param>
        /// <param name="NewY">新 Y 坐标</param>
        /// <param name="NewZ">新 Z 坐标</param>
        /// <param name="NewU">新 U 坐标</param>
        /// <param name="NewV">新 V 坐标</param>
        /// <param name="NewW">新 W 坐标</param>
        /// <param name="NewHandStyle">新手势</param>
        /// <returns></returns>
        public bool SetPointPos(string PointName, double NewX, double NewY, double NewZ,
            double NewU, double NewV, double NewW, Hand NewHandStyle)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
            //-----------------------------------------------------

                 //如果PointName的第一个字母不是P或者数字大于999或者小于0
                if(Strings.Mid(PointName.ToUpper(),1,1)!="P"
                    | (Convert.ToUInt16(Strings.Mid(PointName.ToUpper(),2, PointName.Length - 1)) < 0 
                    & Convert.ToUInt16(Strings.Mid(PointName.ToUpper(),2, PointName.Length - 1)) > 999))
                    {
                    AddText("参数''不正确，正确范围是P0~P999");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                string TempStr="";
                if(NewHandStyle==Hand.LeftHand)
                    {
                    TempStr="/L";
                    }
                else if (NewHandStyle == Hand.RightHand)
                    {
                    TempStr="/R";
                    }
                else if (NewHandStyle == Hand.Unknow) 
                    {
                    TempStr = "";
                    }

                if(FeedBackMessageFromRobot[i + 3]=='1' 
                    & FeedBackMessageFromRobot[i + 11]=='1' )
                    {
                    // X: -156.600 Y:  274.958 Z:  -14.076 U:  -48.417 V:    0.000 W:    0.000 /L /0
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"" + PointName + "=XY(" + NewX.ToString() + "," 
                        + NewY.ToString() + "," + NewZ.ToString() + "," + NewU.ToString() + "," + NewV.ToString() 
                        + "," + NewW.ToString() + ")" + TempStr + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //设置某个点位的坐标值
        /// <summary>
        /// 设置某个点位的坐标值
        /// </summary>
        /// <param name="PointName">EPSON坐标点名称【P0~P999】</param>
        /// <param name="NewPointData">坐标点的新坐标数据结构</param>
        /// <returns></returns>
        public bool SetPointPos(string PointName, EpsonPoint NewPointData)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
            //-----------------------------------------------------

                 //如果PointName的第一个字母不是P或者数字大于999或者小于0
                if(Strings.Mid(PointName.ToUpper(),1,1)!="P"
                    | (Convert.ToUInt16(Strings.Mid(PointName.ToUpper(),2, PointName.Length - 1)) < 0 
                    & Convert.ToUInt16(Strings.Mid(PointName.ToUpper(),2, PointName.Length - 1)) > 999))
                    {
                    AddText("参数''不正确，正确范围是P0~P999");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                string TempStr="";
                if (NewPointData.HandStyle == Hand.LeftHand)
                    {
                    TempStr = "/L";
                    }
                else if (NewPointData.HandStyle == Hand.RightHand)
                    {
                    TempStr = "/R";
                    }
                else if (NewPointData.HandStyle == Hand.Unknow)
                    {
                    TempStr = "";
                    }

                if(FeedBackMessageFromRobot[i + 3]=='1' 
                    & FeedBackMessageFromRobot[i + 11]=='1' )
                    {
                    // X: -156.600 Y:  274.958 Z:  -14.076 U:  -48.417 V:    0.000 W:    0.000 /L /0
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"" + PointName + "=XY(" + NewPointData.X.ToString() + "," 
                        + NewPointData.Y.ToString() + "," + NewPointData.Z.ToString() + "," + NewPointData.U.ToString() + ","
                        + NewPointData.V.ToString() + "," + NewPointData.W.ToString() + ")" + TempStr + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //执行JUMP命令到某个点位
        /// <summary>
        /// 执行JUMP命令到某个点位
        /// </summary>
        /// <param name="PointName">EPSON坐标点名称【P0~P999】</param>
        /// <returns></returns>
        public bool Jump(string PointName)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
            //----------------------------------------------------

                 //如果PointName的第一个字母不是P或者数字大于999或者小于0
                if(Strings.Mid(PointName.ToUpper(),1,1)!="P"
                    | (Convert.ToUInt16(Strings.Mid(PointName.ToUpper(),2, PointName.Length - 1)) < 0 
                    & Convert.ToUInt16(Strings.Mid(PointName.ToUpper(),2, PointName.Length - 1)) > 999))
                    {
                    AddText("参数''不正确，正确范围是P0~P999");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1' 
                    & FeedBackMessageFromRobot[i + 11]=='1' )
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Jump " + PointName + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //执行MOVE命令到某个点位
        /// <summary>
        /// 执行MOVE命令到某个点位
        /// </summary>
        /// <param name="PointName">EPSON坐标点名称【P0~P999】</param>
        /// <returns></returns>
        public bool Move(string PointName)
            {

            try
                {

            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
            //----------------------------------------------------

                 //如果PointName的第一个字母不是P或者数字大于999或者小于0
                if(Strings.Mid(PointName.ToUpper(),1,1)!="P"
                    | (Convert.ToUInt16(Strings.Mid(PointName.ToUpper(),2, PointName.Length - 1)) < 0 
                    & Convert.ToUInt16(Strings.Mid(PointName.ToUpper(),2, PointName.Length - 1)) > 999))
                    {
                    AddText("参数''不正确，正确范围是P0~P999");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1' 
                    & FeedBackMessageFromRobot[i + 11]=='1' )
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move " + PointName + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //执行MOVE绝对运动到某个点位
        /// <summary>
        /// 执行MOVE绝对运动到某个点位
        /// </summary>
        /// <param name="X">X 轴绝对坐标值</param>
        /// <param name="Y">Y 轴绝对坐标值</param>
        /// <param name="Z">Z 轴绝对坐标值</param>
        /// <param name="U">U 轴绝对坐标值</param>
        /// <param name="V">V 轴绝对坐标值</param>
        /// <param name="W">W 轴绝对坐标值</param>
        /// <param name="HandStyle">手势</param>
        /// <returns></returns>
        public bool MoveABS(double X, double Y, double Z, double U, 
            double V, double W, Hand HandStyle)
            {

            try
                {

            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
            //----------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                string TempStr="";
                if(HandStyle==Hand.LeftHand)
                    {
                    TempStr="/L";
                    }
                else
                    {
                    TempStr="/R";
                    }

                if(FeedBackMessageFromRobot[i + 3]=='1' 
                    & FeedBackMessageFromRobot[i + 11]=='1' )
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move XY(" + X.ToString() + "," 
                        + Y.ToString() + "," + Z.ToString() + "," + U.ToString() + "," + V.ToString() 
                        + "," + W.ToString() + ")" + TempStr + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //执行MOVE绝对运动到某个点位
        /// <summary>
        /// 执行MOVE绝对运动到某个点位
        /// </summary>
        /// <param name="TargetPointData">目标点位的坐标值数据结构</param>
        /// <returns></returns>
        public bool MoveABS(EpsonPoint TargetPointData)
            {

            try
                {

            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
            //----------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                string TempStr="";
                if(TargetPointData.HandStyle==Hand.LeftHand)
                    {
                    TempStr="/L";
                    }
                else
                    {
                    TempStr="/R";
                    }

                if(FeedBackMessageFromRobot[i + 3]=='1' 
                    & FeedBackMessageFromRobot[i + 11]=='1' )
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move XY(" + TargetPointData.X.ToString() + "," 
                        + TargetPointData.Y.ToString() + "," + TargetPointData.Z.ToString() + "," + TargetPointData.U.ToString() 
                        + "," + TargetPointData.V.ToString() + "," + TargetPointData.W.ToString() + ")" + TempStr + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //执行GO命令到某个点位
        /// <summary>
        /// 执行GO命令到某个点位
        /// </summary>
        /// <param name="PointName">EPSON坐标点名称【P0~P999】</param>
        /// <returns></returns>
        public bool Go(string PointName)
            {

            try
                {

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
            //----------------------------------------------------

                 //如果PointName的第一个字母不是P或者数字大于999或者小于0
                if(Strings.Mid(PointName.ToUpper(),1,1)!="P"
                    | (Convert.ToUInt16(Strings.Mid(PointName.ToUpper(),2, PointName.Length - 1)) < 0 
                    & Convert.ToUInt16(Strings.Mid(PointName.ToUpper(),2, PointName.Length - 1)) > 999))
                    {
                    AddText("参数''不正确，正确范围是P0~P999");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1' 
                    & FeedBackMessageFromRobot[i + 11]=='1' )
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Go " + PointName + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //执行GO绝对运动到某个点位
        /// <summary>
        /// 执行GO绝对运动到某个点位
        /// </summary>
        /// <param name="X">X 轴绝对坐标值</param>
        /// <param name="Y">Y 轴绝对坐标值</param>
        /// <param name="Z">Z 轴绝对坐标值</param>
        /// <param name="U">U 轴绝对坐标值</param>
        /// <param name="V">V 轴绝对坐标值</param>
        /// <param name="W">W 轴绝对坐标值</param>
        /// <param name="HandStyle">手势</param>
        /// <returns></returns>
        public bool GoABS(double X, double Y, double Z, double U, 
            double V, double W, Hand HandStyle)
            {

            try
                {

            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
            //----------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                string TempStr="";
                if(HandStyle==Hand.LeftHand)
                    {
                    TempStr="/L";
                    }
                else
                    {
                    TempStr="/R";
                    }

                if(FeedBackMessageFromRobot[i + 3]=='1' 
                    & FeedBackMessageFromRobot[i + 11]=='1' )
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Go XY(" + X.ToString() + "," 
                        + Y.ToString() + "," + Z.ToString() + "," + U.ToString() + "," + V.ToString() 
                        + "," + W.ToString() + ")" + TempStr + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //执行GO绝对运动到某个点位
        /// <summary>
        /// 执行GO绝对运动到某个点位
        /// </summary>
        /// <param name="TargetPointData">目标点位的坐标值数据结构</param>
        /// <returns></returns>
        public bool GoABS(EpsonPoint TargetPointData)
            {

            try
                {

            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
            //----------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                string TempStr="";
                if(TargetPointData.HandStyle==Hand.LeftHand)
                    {
                    TempStr="/L";
                    }
                else
                    {
                    TempStr="/R";
                    }

                if(FeedBackMessageFromRobot[i + 3]=='1' 
                    & FeedBackMessageFromRobot[i + 11]=='1' )
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Go XY(" + TargetPointData.X.ToString() + "," 
                        + TargetPointData.Y.ToString() + "," + TargetPointData.Z.ToString() + "," + TargetPointData.U.ToString() 
                        + "," + TargetPointData.V.ToString() + "," + TargetPointData.W.ToString() + ")" + TempStr + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //保存点数据到文件
        /// <summary>
        /// 保存点数据到文件
        /// </summary>
        /// <returns></returns>
        public bool SavePointPos()
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
            //----------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1' 
                    & FeedBackMessageFromRobot[i + 11]=='1' )
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"SavePoints Points.pts" + "\"" + Suffix);
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"LoadPoints Points.pts" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //保存点数据到文件,弹出保存文件对话框
        /// <summary>
        /// 保存点数据到文件,弹出保存文件对话框 
        /// </summary>
        /// <returns></returns>
        public bool SavePointPosWithSaveDialog()
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
            //----------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                SaveFileDialog TempSaveFile = new SaveFileDialog();
                TempSaveFile.AddExtension=true;
                TempSaveFile.CheckFileExists=true;
                TempSaveFile.DefaultExt="pts";
                TempSaveFile.Filter="EPSON点数据文件|*.pts";
                
                if(TempSaveFile.ShowDialog()==DialogResult.OK)
                    {
                    if(TempSaveFile.FileName!="")
                        {
                        //文件名称需要进行处理或者验证此方法是否可行
                        int i;
                        i = FeedBackMessageFromRobot.IndexOf(",");
                        
                        if(FeedBackMessageFromRobot[i + 3]=='1' 
                            & FeedBackMessageFromRobot[i + 11]=='1' )
                            {
                            FeedBackMessageFromRobot = SendCommand("$Execute,\"SavePoints " + TempSaveFile.FileName + "\"" + Suffix);
                            FeedBackMessageFromRobot = SendCommand("$Execute,\"LoadPoints " + TempSaveFile.FileName + "\"" + Suffix);
                            }
                        else
                            {
                            ExecutingBusy=false;
                            return false;
                            }
                        }
                    else
                        {
                        ExecutingBusy=false;
                        return false;
                        }
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //停止机械手停止所有的任务和命令
        /// <summary>
        /// 停止机械手停止所有的任务和命令
        /// </summary>
        /// <returns></returns>
        public bool StopMission()
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Stop    停止所有的任务和命令。     Auto开
            //---------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Stop" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //复位机械手: 清除紧急停止和错误
        /// <summary>
        /// 复位机械手: 清除紧急停止和错误
        /// </summary>
        /// <returns></returns>
        public bool ResetRobot()
            {

            try
                {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //           Reset      清除紧急停止和错误            Auto 开/Ready 开
                //           -------------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Reset" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //查询EPSON机械手返回的状态位的某位代表的含义
        /// <summary>
        /// 查询EPSON机械手返回的状态位的某位代表的含义
        /// </summary>
        /// <param name="StatusCodeBit">目标位【1~11】</param>
        /// <param name="IsOn">目标位的状态【0：OFF,1：ON】</param>
        /// <returns>返回状态位的某位代表的含义</returns>
        public string ProcessStatusCode(int StatusCodeBit, ushort IsOn)
            {

            try
                {

                if(StatusCodeBit>11 | StatusCodeBit<1)
                    {
                    AddText("The parameter 'StatusCodeBit' should be 1~11, please revise it.");
                    MessageBox.Show("The parameter 'StatusCodeBit' should be 1~11, please revise it.");
                    return "Invalid parameter 'StatusCodeBit', it should be 1~11.";
                    }

                if(IsOn!=0)
                    {
                    IsOn=1;
                    }

                //Test/Teach/Auto/Warning/SError/Safeguard/EStop/Error/Paused/Running/Ready 1 为开/0 为关

                string TempStr="";

                switch(StatusCodeBit)
                    {
                    case 0:
                        //0 - Test            在TEST模式下打开
                        if(IsOn==0)
                            {
                            TempStr = "Test Mode OFF";
                            AddText("Test Mode OFF");
                            }
                        else
                            {
                            TempStr = "Test Mode ON";
                            AddText("Test Mode ON");
                            }
                        break;

                    case 1:
                        //1 - Teach           在TEACH模式下打开
                        if(IsOn==0)
                            {
                            TempStr = "Teach Mode OFF";
                            AddText("Teach Mode OFF");
                            }
                        else
                            {
                            TempStr = "Teach Mode ON";
                            AddText("Teach Mode ON");
                            }
                        break;

                    case 2:
                        //2 - Auto            在远程输入接受条件下打开
                        if(IsOn==0)
                            {
                            TempStr = "Auto Mode OFF";
                            AddText("Auto Mode OFF");
                            }
                        else
                            {
                            TempStr = "Auto Mode ON";
                            AddText("Auto Mode ON");
                            }
                        break;
                    case 3:
                        //3 - Warnig 在警告条件下打开,甚至在警告条件下也可以像往常一样执行任务。
                        //但是,应尽快采取警告行动。
                        if(IsOn==0)
                            {
                            TempStr = "Warnig OFF";
                            AddText("Warnig OFF");
                            }
                        else
                            {
                            TempStr = "Warnig ON";
                            AddText("Warnig ON");
                            }
                            break;

                    case 4:
                        //4 - SError   在严重错误状态下打开,发生严重错误时,重新启动控制器,
                        //以便从错误状态中恢复。“Reset 输入”不可用。
                        if(IsOn==0)
                            {
                            TempStr = "SError OFF";
                            AddText("SError OFF");
                            }
                        else
                            {
                            TempStr = "SError ON";
                            AddText("SError ON");
                            }
                        break;

                    case 5:
                        //5 - Safeguard       安全门打开时打开
                        if(IsOn==0)
                            {
                            TempStr = "Safeguard OFF";
                            AddText("Safeguard OFF");
                            }
                        else
                            {
                            TempStr = "Safeguard ON";
                            AddText("Safeguard ON");
                            }
                        break;

                    case 6:
                        //6 - EStop           在紧急状态下打开
                        if(IsOn==0)
                            {
                            TempStr = "EStop OFF";
                            AddText("EStop OFF");
                            }
                        else
                            {
                            TempStr = "EStop ON";
                            AddText("EStop ON");
                            }
                        break;

                    case 7:
                        //7 - Error           在错误状态下打开,使用“Reset 输入”从错误状态中恢复。
                        if(IsOn==0)
                            {
                            TempStr = "Error OFF";
                            AddText("Error OFF");
                            }
                        else
                            {
                            TempStr = "Error ON";
                            AddText("Error ON");
                            }
                        break;

                    case 8:
                        //8 - Paused          打开暂停的任务
                        if(IsOn==0)
                            {
                            TempStr = "Paused OFF";
                            AddText("Paused OFF");
                            }
                        else
                            {
                            TempStr = "Paused ON";
                            AddText("Paused ON");
                            }
                        break;

                    case 9:
                        //9 - Running         执行任务时打开,在“Paused 输出”为开时关闭。
                        if(IsOn==0)
                            {
                            TempStr = "Running OFF";
                            AddText("Running OFF");
                            }
                        else
                            {
                            TempStr = "Running ON";
                            AddText("Running ON");
                            }
                        break;

                    case 10:
                        //10 - Ready           控制器完成启动且无任务执行时打开
                        if(IsOn==0)
                            {
                            TempStr = "Ready OFF";
                            AddText("Ready OFF");
                            }
                        else
                            {
                            TempStr = "Ready ON";
                            AddText("Ready ON");
                            }
                        break;

                    default:
                        TempStr = "Invalid bit";
                        break;                    
                    
                    }

                return TempStr;

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                return ex.Message;
                //MessageBox.Show(ex.Message);
                }
            }

        //查询EPSON机械手返回的状态位的所有位代表的含义
        /// <summary>
        /// 查询EPSON机械手返回的状态位的所有位代表的含义
        /// </summary>
        /// <param name="StatusCode">EPSON机械手返回的状态位: 11位长度0和1组合的状态位</param>
        /// <returns>返回状态位的所有位代表的含义</returns>
        public string ProcessStatusCode(string StatusCode)
            {

            try
                {

            //*3 错误/警告代码
            //以 4 位数字表示。如果没有错误和警告,则为 0000。

            //例如）1： #GetStatus,0100000001,0000
            //Auto 位和 Ready 位为开（1）。
            //表示自动模式开启并处于准备就绪状态。已启用命令执行。

            //例如）2： #GetStatus,0110000010,0517
            //这意味着运行过程中发生警告。对警告代码采取适当的行动。（在这种情况下,警告代码为 0517）

            //标志(内容)
            //----------------------------------------------------------------------------------------
            //Test(在TEST模式下打开)
            //----------------------------------------------------------------------------------------
            //Teach(在TEACH模式下打开)
            //----------------------------------------------------------------------------------------
            //Auto(在远程输入接受条件下打开)
            //----------------------------------------------------------------------------------------
            //Warnig(在警告条件下打开)
            //                甚至在警告条件下也可以像往常一样执行任务。但是,应尽快采取警告行动。
            //----------------------------------------------------------------------------------------
            //SError(在严重错误状态下打开)
            //                发生严重错误时,重新启动控制器,以便从错误状态中恢复。“Reset 输入”不可用。
            //----------------------------------------------------------------------------------------
            //Safeguard(安全门打开时打开)
            //----------------------------------------------------------------------------------------
            //EStop(在紧急状态下打开)
            //----------------------------------------------------------------------------------------
            //Error 在错误状态下打开
            //                使用“Reset 输入”从错误状态中恢复。
            //----------------------------------------------------------------------------------------
            //Paused(打开暂停的任务)
            //----------------------------------------------------------------------------------------
            //Running(执行任务时打开)
            //                在“Paused 输出”为开时关闭。
            //----------------------------------------------------------------------------------------
            //Ready(控制器完成启动且无任务执行时打开)
            //----------------------------------------------------------------------------------------

                if(StatusCode.Length != 11)
                    {
                    AddText("The length of parameter is not equal to 11, please check it.");
                    MessageBox.Show("The length of parameter is not equal to 11, please check it.");
                    return "";
                    }

                //bool TempCheck=false;
                for(int a=0; a < StatusCode.Length; a++)
                    {
                    if(StatusCode[a]!='0' & StatusCode[a]!='1')
                        {
                        //TempCheck=false;
                        AddText("The format of status code should be  1 or 0, for example: 00100000001. Please revise it.");
                        //MessageBox.Show("The format of status code should be  1 or 0, for example: 00100000001. Please revise it.");
                        return "";
                        }
                    
                    }


                //Test/Teach/Auto/Warning/SError/Safeguard/EStop/Error/Paused/Running/Ready 1 为开/0 为关

                string TempStr="";
                //0 - Test            在TEST模式下打开
                if(StatusCode[0]=='0')
                    {
                    TempStr = "Test Mode OFF" + ",   ";
                    AddText("Test Mode OFF");
                    }
                else if(StatusCode[0]=='1')
                    {
                    TempStr = "Test Mode ON" + ",   ";
                    AddText("Test Mode ON");
                    }

                //1 - Teach           在TEACH模式下打开
                if(StatusCode[1]=='0')
                    {
                    TempStr += "Teach Mode OFF" + ",   ";
                    AddText("Teach Mode OFF");
                    }
                else if(StatusCode[1]=='1')
                    {
                    TempStr += "Teach Mode ON" + ",   ";
                    AddText("Teach Mode ON");
                    }
                
                //2 - Auto            在远程输入接受条件下打开
                if(StatusCode[2]=='0')
                    {
                    TempStr += "Auto Mode OFF" + ",   ";
                    AddText("Auto Mode OFF");
                    }
                else if(StatusCode[2]=='1')
                    {
                    TempStr += "Auto Mode ON" + ",   ";
                    AddText("Auto Mode ON");
                    }
                
                //3 - Warnig 在警告条件下打开,甚至在警告条件下也可以像往常一样执行任务。
                //但是,应尽快采取警告行动。
                if(StatusCode[3]=='0')
                    {
                    TempStr += "Warnig OFF" + ",   ";
                    AddText("Warnig OFF");
                    }
                else if(StatusCode[3]=='1')
                    {
                    TempStr += "Warnig ON" + ",   ";
                    AddText("Warnig ON");
                    }
                
                //4 - SError   在严重错误状态下打开,发生严重错误时,重新启动控制器,
                //以便从错误状态中恢复。“Reset 输入”不可用。
                if(StatusCode[4]=='0')
                    {
                    TempStr += "SError OFF" + ",   ";
                    AddText("SError OFF");
                    }
                else if(StatusCode[4]=='1')
                    {
                    TempStr += "SError ON" + ",   ";
                    AddText("SError ON");
                    }
              
                //5 - Safeguard       安全门打开时打开
                if(StatusCode[5]=='0')
                    {
                    TempStr += "Safeguard OFF" + ",   ";
                    AddText("Safeguard OFF");
                    }
                else if(StatusCode[5]=='1')
                    {
                    TempStr += "Safeguard ON" + ",   ";
                    AddText("Safeguard ON");
                    }
                
                //6 - EStop           在紧急状态下打开
                if(StatusCode[6]=='0')
                    {
                    TempStr += "EStop OFF" + ",   ";
                    AddText("EStop OFF");
                    }
                else if(StatusCode[6]=='1')
                    {
                    TempStr += "EStop ON" + ",   ";
                    AddText("EStop ON");
                    }
                
                //7 - Error           在错误状态下打开,使用“Reset 输入”从错误状态中恢复。
                if(StatusCode[7]=='0')
                    {
                    TempStr += "Error OFF" + ",   ";
                    AddText("Error OFF");
                    }
                else if(StatusCode[7]=='1')
                    {
                    TempStr += "Error ON" + ",   ";
                    AddText("Error ON");
                    }
                
                //8 - Paused          打开暂停的任务
                if(StatusCode[8]=='0')
                    {
                    TempStr += "Paused OFF" + ",   ";
                    AddText("Paused OFF");
                    }
                else if(StatusCode[8]=='1')
                    {
                    TempStr += "Paused ON" + ",   ";
                    AddText("Paused ON");
                    }
                
                //9 - Running         执行任务时打开,在“Paused 输出”为开时关闭。
                if(StatusCode[9]=='0')
                    {
                    TempStr += "Running OFF" + ",   ";
                    AddText("Running OFF");
                    }
                else if(StatusCode[9]=='1')
                    {
                    TempStr += "Running ON" + ",   ";
                    AddText("Running ON");
                    }
                
                //10 - Ready           控制器完成启动且无任务执行时打开
                if(StatusCode[10]=='0')
                    {
                    TempStr += "Ready OFF";
                    AddText("Ready OFF");
                    }
                else if(StatusCode[10]=='1')
                    {
                    TempStr += "Ready ON";
                    AddText("Ready ON");
                    }

                return TempStr;

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                return ex.Message;
                //MessageBox.Show(ex.Message);
                }
            }

        //查询EPSON机械手返回的状态位的所有位代表的含义
        /// <summary>
        /// 查询EPSON机械手返回的状态位的所有位代表的含义
        /// </summary>
        /// <param name="StatusCode">EPSON机械手返回的状态位: 11位长度0和1组合的状态位</param>
        /// <returns>返回所有位状态位的数据结构</returns>
        public EpsonStatusBits NewProcessStatusCode(string StatusCode)
            {
            
            EpsonStatusBits TempStatus;
            TempStatus.Auto=false;
            TempStatus.Error=false;
            TempStatus.EStop=false;
            TempStatus.Paused=false;
            TempStatus.Ready=false;
            TempStatus.Running=false;
            TempStatus.Safeguard=false;
            TempStatus.SError=false;
            TempStatus.Teach=false;
            TempStatus.Test=false;
            TempStatus.Warning=false;

            try
                {

            //*3 错误/警告代码
            //以 4 位数字表示。如果没有错误和警告,则为 0000。

            //例如）1： #GetStatus,0100000001,0000
            //Auto 位和 Ready 位为开（1）。
            //表示自动模式开启并处于准备就绪状态。已启用命令执行。

            //例如）2： #GetStatus,0110000010,0517
            //这意味着运行过程中发生警告。对警告代码采取适当的行动。（在这种情况下,警告代码为 0517）

            //标志(内容)
            //----------------------------------------------------------------------------------------
            //Test(在TEST模式下打开)
            //----------------------------------------------------------------------------------------
            //Teach(在TEACH模式下打开)
            //----------------------------------------------------------------------------------------
            //Auto(在远程输入接受条件下打开)
            //----------------------------------------------------------------------------------------
            //Warnig(在警告条件下打开)
            //                甚至在警告条件下也可以像往常一样执行任务。但是,应尽快采取警告行动。
            //----------------------------------------------------------------------------------------
            //SError(在严重错误状态下打开)
            //                发生严重错误时,重新启动控制器,以便从错误状态中恢复。“Reset 输入”不可用。
            //----------------------------------------------------------------------------------------
            //Safeguard(安全门打开时打开)
            //----------------------------------------------------------------------------------------
            //EStop(在紧急状态下打开)
            //----------------------------------------------------------------------------------------
            //Error 在错误状态下打开
            //                使用“Reset 输入”从错误状态中恢复。
            //----------------------------------------------------------------------------------------
            //Paused(打开暂停的任务)
            //----------------------------------------------------------------------------------------
            //Running(执行任务时打开)
            //                在“Paused 输出”为开时关闭。
            //----------------------------------------------------------------------------------------
            //Ready(控制器完成启动且无任务执行时打开)
            //----------------------------------------------------------------------------------------

                if(StatusCode.Length != 11)
                    {
                    AddText("The length of parameter is not equal to 11, please check it.");
                    MessageBox.Show("The length of parameter is not equal to 11, please check it.");
                    return TempStatus;
                    }

                //bool TempCheck=false;
                for(int a=0; a < StatusCode.Length; a++)
                    {
                    if(StatusCode[a]!='0' & StatusCode[a]!='1')
                        {
                        //TempCheck=false;
                        AddText("The format of status code should be  1 or 0, for example: 00100000001. Please revise it.");
                        //MessageBox.Show("The format of status code should be  1 or 0, for example: 00100000001. Please revise it.");
                        return TempStatus;
                        }
                    
                    }


                //Test/Teach/Auto/Warning/SError/Safeguard/EStop/Error/Paused/Running/Ready 1 为开/0 为关

                //string TempStr="";
                //0 - Test            在TEST模式下打开
                if(StatusCode[0]=='0')
                    {
                    TempStatus.Test=false;
                    }
                else if(StatusCode[0]=='1')
                    {
                    TempStatus.Test=true;
                    }

                //1 - Teach           在TEACH模式下打开
                if(StatusCode[1]=='0')
                    {
                    TempStatus.Teach=false;
                    }
                else if(StatusCode[1]=='1')
                    {
                    TempStatus.Teach=true;
                    }
                
                //2 - Auto            在远程输入接受条件下打开
                if(StatusCode[2]=='0')
                    {
                    TempStatus.Auto=false;
                    }
                else if(StatusCode[2]=='1')
                    {
                    TempStatus.Auto=true;
                    }
                
                //3 - Warnig 在警告条件下打开,甚至在警告条件下也可以像往常一样执行任务。
                //但是,应尽快采取警告行动。
                if(StatusCode[3]=='0')
                    {
                    TempStatus.Warning=false;
                    }
                else if(StatusCode[3]=='1')
                    {
                    TempStatus.Warning=true;
                    }
                
                //4 - SError   在严重错误状态下打开,发生严重错误时,重新启动控制器,
                //以便从错误状态中恢复。“Reset 输入”不可用。
                if(StatusCode[4]=='0')
                    {
                    TempStatus.SError=false;
                    }
                else if(StatusCode[4]=='1')
                    {
                    TempStatus.SError=true;
                    }
              
                //5 - Safeguard       安全门打开时打开
                if(StatusCode[5]=='0')
                    {
                    TempStatus.Safeguard=false;
                    }
                else if(StatusCode[5]=='1')
                    {
                    TempStatus.Safeguard=true;
                    }
                
                //6 - EStop           在紧急状态下打开
                if(StatusCode[6]=='0')
                    {
                    TempStatus.EStop=false;
                    }
                else if(StatusCode[6]=='1')
                    {
                    TempStatus.EStop=true;
                    }
                
                //7 - Error           在错误状态下打开,使用“Reset 输入”从错误状态中恢复。
                if(StatusCode[7]=='0')
                    {
                    TempStatus.Error=false;
                    }
                else if(StatusCode[7]=='1')
                    {
                    TempStatus.Error=true;
                    }
                
                //8 - Paused          打开暂停的任务
                if(StatusCode[8]=='0')
                    {
                    TempStatus.Paused=false;
                    }
                else if(StatusCode[8]=='1')
                    {
                    TempStatus.Paused=true;
                    }
                
                //9 - Running         执行任务时打开,在“Paused 输出”为开时关闭。
                if(StatusCode[9]=='0')
                    {
                    TempStatus.Running=false;
                    }
                else if(StatusCode[9]=='1')
                    {
                    TempStatus.Running=true;
                    }
                
                //10 - Ready           控制器完成启动且无任务执行时打开
                if(StatusCode[10]=='0')
                    {
                    TempStatus.Ready=false;
                    }
                else if(StatusCode[10]=='1')
                    {
                    TempStatus.Ready=true;
                    }

                return TempStatus;

                }
            catch (Exception ex)
                {
                AddText(ex.Message);
                return TempStatus;
                //MessageBox.Show(ex.Message);
                }
            }

        //连接EPSON远程以太网控制的线程
        /// <summary>
        /// 连接EPSON远程以太网控制的线程
        /// </summary>
        public void ConnectRemoteEpsonTCPIP()
            {

            while(true)
                {
                
                try
                    {

                    if(ConnectedRemoteEpsonTCPIP==false)
                        {
                        AddText("Connecting to the remote Epson robot....");
                        ClientConnectToRemoteEpson = new TcpClient();
                        ClientConnectToRemoteEpson.Connect(EpsonRemoteControlIP,EpsonRemoteControlPort);
                        AddText("Connected to the remote Epson robot(IP: " + EpsonRemoteControlIP 
                            + " , port: " + EpsonRemoteControlPort + ")");

                        StreamConnectToRemoteEpson=ClientConnectToRemoteEpson.GetStream();
                        StreamConnectToRemoteEpson.ReadTimeout=5000;
                        ConnectedRemoteEpsonTCPIP=true;

                        if(Login(strLoginRobotPassword)==true)
                            {
                            ConnectedRemoteEpsonTCPIP=true;
                            AddText("Login to the remote Epson robot success");
                            }
                        else
                            {
                            ConnectedRemoteEpsonTCPIP=false;
                            ClientConnectToRemoteEpson.Close();
                            AddText("Login to the remote Epson robot failure");
                            }

                        }
                    
                    }
                catch (Exception ex)
                    {
                    ConnectedRemoteEpsonTCPIP=false;
                    ClientConnectToRemoteEpson.Close();
                    AddText(ex.Message);
                    //return;
                    //MessageBox.Show(ex.Message);
                    }

                }

            }
        
        //释放所有资源
        /// <summary>
        /// 释放所有资源
        /// </summary>
        public void Dispose()
            {

            try
                {

                LV = null;
                PC = null;
                FC.Dispose();

                if (rtbShowMessageViaDelegate != null)
                    {
                    rtbShowMessageViaDelegate = null;
                    }

                if(ExcelWorkSheet!=null)
                    {
                    ExcelWorkSheet=null;
                    }
                
                if(ExcelWorkBook!=null)
                    {
                    ExcelWorkBook.Close();
                    ExcelWorkBook=null;
                    }

               if(ExcelApp!=null)
                    {
                    ExcelApp.Quit();
                    ExcelApp=null;
                    }

                if(StreamConnectToRemoteEpson!=null)
                    {
                    StreamConnectToRemoteEpson.Close();
                    StreamConnectToRemoteEpson=null;
                    }
                
                if(ClientConnectToRemoteEpson!=null)
                    {
                    ClientConnectToRemoteEpson.Close();
                    ClientConnectToRemoteEpson=null;
                    }

                if(ClientConnectToRobot!=null)
                    {
                    ClientConnectToRobot.Close();
                    ClientConnectToRobot=null;
                    }

                if(ThreadConnectRemoteEpsonTCPIP!=null)
                    {
                    ThreadConnectRemoteEpsonTCPIP.Abort();
                    ThreadConnectRemoteEpsonTCPIP=null;
                    }

                if(ClientConnectToRemoteEpson!=null)
                    {
                    ClientConnectToRemoteEpson.Close();
                    ClientConnectToRemoteEpson=null;
                    }

                if(ClientConnectToRobot!=null)
                    {
                    ClientConnectToRobot.Close();
                    ClientConnectToRobot=null;
                    }

                if(ServerAcceptConnectionFromRobot!=null)
                    {
                    ServerAcceptConnectionFromRobot.Close();
                    ServerAcceptConnectionFromRobot=null;
                    }

                GC.Collect();
                
                }
            catch (Exception)// ex)
                {
                //AddText(ex.Message);
                //return;
                //MessageBox.Show(ex.Message);
                }
            }
        
        //打开EPSON机械手的电源
        /// <summary>
        /// 打开EPSON机械手的电源
        /// </summary>
        /// <param name="NumberOfRobot">机械手编号【0~16】,0代表所有可用机械手</param>
        /// <returns></returns>
        public bool MotorOn(int NumberOfRobot = 1)
            {
            
            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //SetMotorsOn         机械手编号             打开机械手电机                                    Auto开/Ready开/EStop关/Safeguard关
            //---------------------------------------------------------

                if(NumberOfRobot<0 | NumberOfRobot>16)
                    {
                    AddText("The correct number of  robot is from 0 to 16, 0 is for all available robots, please revise it and retry.");
                    //MessageBox.Show("The correct number of  robot is from 0 to 16, 0 is for all available robots, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$SetMotorsOn," + NumberOfRobot + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }
            }

        //关闭EPSON机械手的电源
        /// <summary>
        /// 关闭EPSON机械手的电源
        /// </summary>
        /// <param name="NumberOfRobot">机械手编号【0~16】,0代表所有可用机械手</param>
        /// <returns></returns>
        public bool MotorOff(int NumberOfRobot = 1)
            {
            
            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //SetMotorsOff         机械手编号             关闭机械手电机                                    Auto开/Ready开/EStop关/Safeguard关
            //---------------------------------------------------------

                if(NumberOfRobot<0 | NumberOfRobot>16)
                    {
                    AddText("The correct number of  robot is from 0 to 16, 0 is for all available robots, please revise it and retry.");
                    //MessageBox.Show("The correct number of  robot is from 0 to 16, 0 is for all available robots, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$SetMotorsOff," + NumberOfRobot + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }
            }
        
        //获取EPSON机械手控制器中某个变量的值
        /// <summary>
        /// 获取EPSON机械手控制器中某个变量的值
        /// </summary>
        /// <param name="VariableName">变量名</param>
        /// <param name="Value">变量名的值</param>
        /// <returns></returns>
        public bool GetVariable(ref string VariableName, ref string[] Value)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //GetVariable         参数名称{, type}       获取备份（Global Preserve）参数的值               随时可用
            //                    ***********************************************************
            //                    (参数名称)（数组元
            //                    素）,(参数名称类
            //                    型),(获取的编号)      获取备份（Global Preserve）数组参数的值
            //(*8) 参数类型是指{Boolean | Byte | Double | Integer | Long | Real | String}。
            //指定的类型： 在参数名称和类型相同时用于备份参数。
            //未指定的类型： 在参数名称相同时用于备份参数。
            //(*9) 对于数组元素,指定以下您想获取的元素：
            //如果是从数组头处获取的,您需要指定一个元素。
            //1维数组 参数名称 (0) 从头部获取。
            //参数名称（元素编号） 从指定的元素编号中获取。
            //2维数组 参数名称 (0,0) 从头部获取。
            //参数名称（元素编号1,2） 从指定的元素编号中获取。
            //3维数组 参数名称 (0,0,0) 从头部获取。
            //参数名称（元素编号1,2,3） 从指定的元素编号中获取。
            //您不能忽略要获取的参数类型和编号。
            //您不能指定一个参数类型string。
            //可获取的可用数量多达100个。如果您在数组元素编号上指定一个号码,会发生错误。
            //如）“$GetVariable,gby2(3,0),Byte,3”
            //它获得字节型2维数组参数gby2的gby2(3,0)、gby2(3,1)、gby2(3,2)的值。
            //--------------------------------------------------------------------------------

                if(VariableName=="")
                    {
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$GetVariable," + VariableName + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    try
                        {                        
                    //GetVariable                    #GetVariable,[参数值] 终端
                    //---------------------------------------------------------------------------------
                    //GetVariable（如果是数组）      #GetVariable,[参数值 1],[参数值 2],...,终端 *4
                    //*4 返回要获取的编号中指定编号的值。

                        string[] TempStr=FeedBackMessageFromRobot.Split(',');

                        if(TempStr[0]!="#GetVariable")
                            {
                            Value=null;
                            return false;
                            }

                        if(TempStr.Length==2)
                            {
                            Value=new string[1];
                            Value[0]=TempStr[1];
                            }
                        else{
                            Value=new string[TempStr.Length];
                            for(int a=0; a<TempStr.Length;a++)
                                {
                                Value[a]=TempStr[a];
                                }
                            }                        
                        }
                    catch(Exception ex)
                        {
                        ExecutingBusy=false;
                        AddText(ex.Message);
                        return false;
                        }

                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }
            }

        //给EPSON机械手控制器中某个变量赋值
        /// <summary>
        /// 给EPSON机械手控制器中某个变量赋值
        /// </summary>
        /// <param name="VariableName">变量名</param>
        /// <param name="Value">变量名的值</param>
        /// <returns></returns>
        public bool SetVariable(string VariableName, string Value)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //SetVariable        参数名称和值{,类型}    设置备份（Global Preserve）参数中的值             Auto开/Ready开
            //---------------------------------------------------------------------------------------------------
            //(*8) 参数类型是指{Boolean | Byte | Double | Integer | Long | Real | String | Short | UByte | UShort | Int32 |
            //UInt32 | Int64 | UInt64}。
            //指定类型：在参数名称和类型相同时用于备份参数。
            //未指定类型：在参数名称相同时用于备份参数。

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$SetVariable," 
                        + VariableName + "," + Value + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }
            }
        
        //给EPSON机械手控制器中某个变量赋值
        /// <summary>
        /// 给EPSON机械手控制器中某个变量赋值
        /// </summary>
        /// <param name="VariableName">变量名</param>
        /// <param name="Value">变量名的值</param>
        /// <param name="VariableType">变量的类型</param>
        /// <returns></returns>
        public bool SetVariable(string VariableName, string Value,
            EpsonVariable VariableType)
            {
            
            try
                {

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //SetVariable        参数名称和值{,类型}    设置备份（Global Preserve）参数中的值             Auto开/Ready开
            //---------------------------------------------------------------------------------------------------
            //(*8) 参数类型是指{Boolean | Byte | Double | Integer | Long | Real | String | Short | UByte | UShort | Int32 |
            //UInt32 | Int64 | UInt64}。
            //指定类型：在参数名称和类型相同时用于备份参数。
            //未指定类型：在参数名称相同时用于备份参数。

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$SetVariable," 
                        + VariableName + "," + Value + "," +
                        Strings.Mid(VariableType.ToString(), 3, 
                        VariableType.ToString().Length - 2)+ Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }
            }
        
        //获取工具坐标设置,* 执行后暂时没有发现返回具体值,只是：#Execute,0
        /// <summary>
        /// 获取工具坐标设置
        /// </summary>
        /// <param name="NumberOfTool">工具坐标编号【1~15】</param>
        /// <returns></returns>
        public bool GetToolSetting(int NumberOfTool)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
            //----------------------------------------------------

            //TLSet
            //用于设置/显示工具坐标系。

            //格式
            //(1) TLSet 工具坐标系编号, 工具设置数据
            //(2) TLSet 工具坐标系编号
            //(3) TLSet

            //参数
            //工具坐标系编号 以1～15 的整数值指定要设置的工具。（Tool 0 为默认工具,不能变更。）
            //工具设置数据 以P 编号、P（表达式）、点标签点或表达式指定要设置的工具坐标系的原点和
            //方向。

            //结果
            //如果省略所有参数,则显示所有的TLSet 设置。
            //如果只指定工具编号,则显示指定的TLSet 设置。

            //说明
            //指定针对Tool 0 坐标系（夹具末端坐标系）的相对原点位置和相对旋转角度,定义工具坐标系
            //Tool 1、Tool 2 、Tool 3。
            //        TLSet(1, XY(50, 100, -20, 30)) ----  【X,Y,Z,U】
            //TLSet(2, P10 + X(20))
            //上述情况时,引用坐标值P10 并在X 值上加上20。无视机械臂属性和本地坐标系编号。
            //ツール座標系の原点のZ(軸方向位置)
            //ツール座標系の原点のY(軸方向位置(次図b))
            //ツール座標系の原点のX(軸方向位置(次図a))
            //ツール座標系番号
            //TLSET(1, XY(100, 60, -20, 30))

                if(NumberOfTool<1 | NumberOfTool>15)
                    {
                    AddText("The parameter 'NumberOfTool' is over range, it must be 1 to 15, please revise it and retry.");
                    //MessageBox.Show("The parameter 'NumberOfTool' is over range, it must be 1 to 15, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"print TLSET(" + NumberOfTool + ")\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {

                // 需要得到返回值得样本再进行处理
                // $Execute,"tlset 1"
                // !Execute, 99
                // $Execute,"tlset"
                // #Execute,0
                // $Execute,"print tlset"
                // !Execute, 99
                // $Execute,"print tlset 0"
                // !Execute, 99
                // $Execute,"print tlset 1"
                // !Execute, 99
                // $Execute,"tlset 1"
                // !Execute, 99
                // i = FeedBackMessageFromRobot.IndexOf(":", 0)
                // ToolX = FeedBackMessageFromRobot.Substring(i + 1, 10)
                // i = FeedBackMessageFromRobot.IndexOf(":", i + 1)
                // ToolY = FeedBackMessageFromRobot.Substring(i + 1, 10)

                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }
            }
        
        //设置工具坐标
        /// <summary>
        /// 设置工具坐标
        /// </summary>
        /// <param name="NumberOfTool">工具坐标编号【1~15】</param>
        /// <param name="X">工具坐标轴 X 值</param>
        /// <param name="Y">工具坐标轴 Y 值</param>
        /// <param name="Z">工具坐标轴 Z 值</param>
        /// <param name="U">工具坐标轴 U 值</param>
        /// <returns></returns>
        public bool SetTool(int NumberOfTool, double X, double Y,
            double Z, double U)
            {

            try
                {

            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Execute           命令字符串              执行命令                                          Auto开/Ready开/Error关/EStop关/Safeguard关
            //----------------------------------------------------

            //TLSet
            //用于设置/显示工具坐标系。

            //格式
            //(1) TLSet 工具坐标系编号, 工具设置数据
            //(2) TLSet 工具坐标系编号
            //(3) TLSet

            //参数
            //工具坐标系编号 以1～15 的整数值指定要设置的工具。（Tool 0 为默认工具,不能变更。）
            //工具设置数据 以P 编号、P（表达式）、点标签点或表达式指定要设置的工具坐标系的原点和
            //方向。

            //结果
            //如果省略所有参数,则显示所有的TLSet 设置。
            //如果只指定工具编号,则显示指定的TLSet 设置。

            //说明
            //指定针对Tool 0 坐标系（夹具末端坐标系）的相对原点位置和相对旋转角度,定义工具坐标系
            //Tool 1、Tool 2 、Tool 3。
            //        TLSet(1, XY(50, 100, -20, 30)) ----  【X,Y,Z,U】
            //TLSet(2, P10 + X(20))
            //上述情况时,引用坐标值P10 并在X 值上加上20。无视机械臂属性和本地坐标系编号。
            //ツール座標系の原点のZ(軸方向位置)
            //ツール座標系の原点のY(軸方向位置(次図b))
            //ツール座標系の原点のX(軸方向位置(次図a))
            //ツール座標系番号
            //TLSET(1, XY(100, 60, -20, 30))

                if(NumberOfTool<1 | NumberOfTool>15)
                    {
                    AddText("The parameter 'NumberOfTool' is over range, it must be 1 to 15, please revise it and retry.");
                    //MessageBox.Show("The parameter 'NumberOfTool' is over range, it must be 1 to 15, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                string strToolSetting="";
                strToolSetting="XY(" + X.ToString() + "," + Y.ToString() + "," +
                    Z.ToString() + "," + U.ToString() + ")";

                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"TLSET(" + NumberOfTool 
                        + "," + strToolSetting + ")\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {                    
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }
            }

        //暂停机械手的任务
        /// <summary>
        /// 暂停机械手的任务
        /// </summary>
        /// <returns></returns>
        public bool RobotPause()
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Pause                                      暂停所有任务                                      Auto 开/Running 开
            //-------------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Pause" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    RobotWasPaused=true;
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    RobotWasPaused=false;
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }
            }
        
        //继续执行暂停了的任务
        /// <summary>
        /// 继续执行暂停了的任务
        /// </summary>
        /// <returns></returns>
        public bool RobotContinue()
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Continue           继续暂停了的任务      Auto 开/Paused 开
            //---------------------------------------------------------

                if(RobotWasPaused==false)
                    {
                    AddText("The robot was not paused, so no need to execute 'Continue' operation.");
                    //MessageBox.Show("The robot was not paused, so no need to execute 'Continue' operation.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Continue" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    RobotWasPaused=false;
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }
            }
        
        //选择机械手
        /// <summary>
        /// 选择机械手
        /// </summary>
        /// <param name="NumberOfRobot">机械手编号: 0~16</param>
        /// <returns></returns>
        public bool SetCurrentRobot(int NumberOfRobot)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //SetCurRobot         机械手编号             选择机械手                                        Auto开/Ready开
            //-----------------------------------------------------

                if(NumberOfRobot<0 | NumberOfRobot>16)
                    {
                    AddText("The number of  robot is from 0 to 16, 0 is for all available robots, please revise it and retry.");
                    //MessageBox.Show("The number of  robot is from 0 to 16, 0 is for all available robots, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$SetCurRobot," + NumberOfRobot + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }
            }

        //获取当前的机械手编号
        /// <summary>
        /// 获取当前的机械手编号
        /// </summary>
        /// <returns>-1代表执行出错,否则返回当前的机械手编号</returns>
        public int GetCurrentRobot()
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //GetCurRobot      获取当前的机械手编号              随时可用
            //--------------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return -1;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return -1;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$GetCurRobot" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return -1;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                //#[远程命令],[0]终端
                //$GetCurRobot
                //#GetCurRobot,1

                    string[] TempStr;
                    TempStr=FeedBackMessageFromRobot.Split(',');

                    if(TempStr[0]!="#GetCurRobot")
                        {
                        ExecutingBusy=false;
                        return -1;
                        }

                    ExecutingBusy=false;
                    return Convert.ToInt32(TempStr[1]);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return -1;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return -1;
                //MessageBox.Show(ex.Message);
                }
            }
        
        //将机械手手臂移动到由用户定义的起始点位置上
        /// <summary>
        /// 将机械手手臂移动到由用户定义的起始点位置上
        /// </summary>
        /// <param name="NumberOfRobot">机械手编号: 0~16</param>
        /// <returns></returns>
        public bool GoHome(int NumberOfRobot = 1)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Home    机械手编号   将机械手手臂移动到由用户定义的起始点位置上   Auto开/Ready开/Error关/EStop关/Safeguard 关
            //---------------------------------------------------------------------------------------------------

                if(NumberOfRobot<0 | NumberOfRobot>16)
                    {
                    AddText("The number of  robot is from 0 to 16, 0 is for all available robots, please revise it and retry.");
                    //MessageBox.Show("The number of  robot is from 0 to 16, 0 is for all available robots, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Home," + NumberOfRobot + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }
            }

        //中止命令的执行
        /// <summary>
        /// 中止命令的执行
        /// </summary>
        /// <returns></returns>
        public bool Abort()
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //Abort     中止命令的执行       Auto开
            //-------------------------------------

                //此处是否不需要考虑其它命令正在执行，待验证
                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Abort" + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }
            }
        
        //设置指定输出位的状态【true：打开此位,false：关闭此位】
        /// <summary>
        /// 设置指定输出位的状态【true：打开此位,false：关闭此位】
        /// </summary>
        /// <param name="TargetOutputBit">操作的目标位【0~15】</param>
        /// <param name="TurnOn">true：打开此位,false：关闭此位</param>
        /// <returns></returns>
        public bool SetOutputBit(int TargetOutputBit, bool TurnOn)
            {

            try
                {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //SetIO     I/O位号和值      设置I/O指定的位
                //1:        打开此位
                //0:        关闭此位(Ready开)
                //---------------------------------------------------------

                if(TargetOutputBit<0 | TargetOutputBit>15)
                    {
                    AddText("The parameter 'TargetOutputBit' should be 0~15, please revise it and retry.");
                    MessageBox.Show("The parameter 'TargetOutputBit' should be 0~15, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if(ExecutingBusy==true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy=true;
                    }

                if(GetRobotStatus(ref FeedBackMessageFromRobot)==false)
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                int TempOut = 0;
                TempOut = (TurnOn==true) ? 1 : 0;

                if(FeedBackMessageFromRobot[i + 3]=='1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$SetIO," + TargetOutputBit + "," + TempOut + Suffix);
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }
                
                if(FeedBackMessageFromRobot.IndexOf("!")==-1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy=false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy=false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }
            }

        //获得指定输入位的当前状态
        /// <summary>
        /// 获得指定输入位的当前状态
        /// </summary>
        /// <param name="TargetInputBit">读取的目标位【0~23】</param>
        /// <param name="IsOn">返回值【true：ON,false：OFF】</param>
        /// <returns></returns>
        public bool GetInputBit(int TargetInputBit, ref bool IsOn)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //GetIO       I/O位号    获得指定的I/O位       随时可用
            //-----------------------------------------------------
                
                if(TargetInputBit<0 | TargetInputBit > 23)
                    {
                    AddText("The parameter 'TargetInputBit' should be 0~23, please revise it and retry.");
                    //MessageBox.Show("The parameter 'TargetInputBit' should be 0~23, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$GetIO," + TargetInputBit + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    //在此添加判断返回的位的值为0或者1()
                    //GetIO    #GetIO,[0 | 1]终端

                    string[] TempStr=FeedBackMessageFromRobot.Split(',');
                    if(TempStr[0]!="#GetIO")
                        {
                        ExecutingBusy = false;
                        IsOn=false;
                        return false;
                        }

                    if(TempStr[1][0]=='1')
                        {
                        IsOn=true;
                        }
                    else
                        {
                        IsOn=false;
                        }

                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy=false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //获得指定的输入端口字节（8位）的当前状态
        /// <summary>
        /// 获得指定的输入端口字节（8位）的当前状态
        /// </summary>
        /// <param name="TargetInputByte">目标输入信号所在字节【0~3】</param>
        /// <param name="ByteStatus">返回输入端口字节（8位）的当前状态</param>
        /// <returns></returns>
        public bool GetInputByte(int TargetInputByte, ref byte ByteStatus)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //GetIOByte   I/O位号   获得指定的I/O端口（8位）   随时可用
            //-----------------------------------------------------------

                ByteStatus=0;

                if(TargetInputByte > 3 | TargetInputByte < 0)
                    {
                    AddText("The parameter 'TargetInputByte' should be 0~3, please revise it and retry.");
                    //MessageBox.Show("The parameter 'TargetInputByte' should be 0~3, please revise it and retry.");
                    //ByteStatus=0;
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    //ByteStatus=0;
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    //ByteStatus=0;
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$GetIOByte," + TargetInputByte + Suffix);
                    }
                else
                    {
                    //ByteStatus=0;
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    //在此添加判断返回的位的值为0或者1()
                    //GetIOByte   #GetIOByte,[字节（8 位）的十六进制字符串（00到 FF）]终端

                    string[] TempStr=FeedBackMessageFromRobot.Split(',');
                    
                    if(TempStr[0]!="#GetIOByte")
                        {
                        //ByteStatus=0;
                        ExecutingBusy = false;
                        return false;
                        }

                    //截取后两个字节，转化为10进制，然后转化为byte
                    //ByteStatus=Convert.ToByte(FC.HexToDecimal(Strings.Right(FeedBackMessageFromRobot, 2)));
                    ByteStatus=Convert.ToByte(FC.HexToDecimal(Strings.Right(TempStr[1], 2)));

                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    //ByteStatus=0;
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                //ByteStatus=0;
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //设置指定输出端口字节（8位）的状态
        /// <summary>
        /// 设置指定输出端口字节（8位）的状态
        /// </summary>
        /// <param name="TargetOutputByte">目标输出端口字节【0~1】</param>
        /// <param name="ByteSetStatus">需要设置的端口字节状态（8位）</param>
        /// <returns></returns>
        public bool SetOutputByte(int TargetOutputByte, byte ByteSetStatus)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //SetIOByte    I/O端口号和值    设置I/O指定的端口（8位）    Ready开
            //-----------------------------------------------------------------

                if(TargetOutputByte<0 | TargetOutputByte>1)
                    {
                    AddText("The parameter 'TargetOutputByte' should be 0~1, please revise it and retry.");
                    //MessageBox.Show("The parameter 'TargetOutputByte' should be 0~1, please revise it and retry.");
                    return false;
                    }
                
                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$SetIOByte," + 
                        TargetOutputByte + "," + ByteSetStatus + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }
        
        //获得指定输入字（16位）端口的当前状态
        /// <summary>
        /// 获得指定输入字（16位）端口的当前状态
        /// </summary>
        /// <param name="TargetInputWord">目标输入字（16位）端口【0~1】</param>
        /// <param name="WordStatus">返回的字端口状态</param>
        /// <returns></returns>
        public bool GetInputWord(int TargetInputWord, ref ushort WordStatus)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //GetIOWord   I/O字端口号   获得指定的I/O字端口（16位）   随时可用
            //----------------------------------------------------------------

                WordStatus=0;

                if(TargetInputWord>1 | TargetInputWord<0)
                    {
                    AddText("The parameter 'TargetInputWord' should be 0~1, please revise it and retry.");
                    //MessageBox.Show("The parameter 'TargetInputWord' should be 0~1, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$GetIOWord," + TargetInputWord + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    //在此添加判断返回的位的值为0或者1
                    //GetIOWord    #GetIOWord,[字（16 位）的十六进制字符串（0000 至 FFFF）]终端
                    
                    string[] TempStr=FeedBackMessageFromRobot.Split(',');
                    
                    if(TempStr[0]!="#GetIOWord")
                        {
                        ExecutingBusy = false;
                        return false;
                        }

                    WordStatus =Convert.ToUInt16(FC.HexToDecimal(Strings.Right(FeedBackMessageFromRobot, 4)));

                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //设置指定输出端口字（16位）的状态
        /// <summary>
        /// 设置指定输出端口字（16位）的状态
        /// </summary>
        /// <param name="TargetOutputWord">目标输出端口字（16位）【0】</param>
        /// <param name="WordSetStatus">需要设置的端口字状态</param>
        /// <returns></returns>
        public bool SetOutputWord(int TargetOutputWord, ushort WordSetStatus)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //SetIOWord  I/O字端口号和值  设置I/O指定的端口（16位）  Auto开/Ready开
            //---------------------------------------------------------------------

                if(TargetOutputWord!=0)
                    {
                    AddText("The parameter 'TargetOutputWord' should be 0, please revise it and retry.");
                    //MessageBox.Show("The parameter 'TargetOutputWord' should be 0, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$SetIOWord," + TargetOutputWord + "," + WordSetStatus + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }
        
        //获得指定内存位的当前状态
        /// <summary>
        /// 获得指定内存位的当前状态
        /// </summary>
        /// <param name="TargetMemoryBit">目标内存位【0~1023】</param>
        /// <param name="IsOn">返回内存位的当前状态【true:1, false:0】</param>
        /// <returns></returns>
        public bool GetMemoryBit(int TargetMemoryBit, ref bool IsOn)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //GetMemIO   内存I/O位号   获得指定的内存I/O位      随时可用
            //----------------------------------------------------------

                IsOn=false;

                if(TargetMemoryBit<0 | TargetMemoryBit>1023)
                    {
                    AddText("The parameter 'TargetMemoryBit' should be 0~65535, please revise it and retry.");
                    //MessageBox.Show("The parameter 'TargetMemoryBit' should be 0~65535, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$GetMemIO," + TargetMemoryBit + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    //在此添加判断返回的位的值为0或者1
                    //GetMemIO          #GetMemIO,[0 | 1]终端 *1
                    
                    string[] TempStr=FeedBackMessageFromRobot.Split(',');
                    
                    if(TempStr[0]!="#GetMemIO")
                        {
                        ExecutingBusy = false;
                        return false;
                        }

                    if(Strings.Right(FeedBackMessageFromRobot, 1) == "0")
                        {
                        IsOn=false;
                        }
                    else
                        {
                        IsOn=true;
                        }

                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //设置指定内存位的状态【true：打开此位,false：关闭此位】
        /// <summary>
        /// 设置指定内存位的状态【true：打开此位,false：关闭此位】
        /// </summary>
        /// <param name="TargetMemoryBit">操作的目标内存位【0~1023】</param>
        /// <param name="TurnOn">设定值【true：打开此位,false：关闭此位】</param>
        /// <returns></returns>
        public bool SetMemoryBit(int TargetMemoryBit, bool TurnOn)
            {

            try
                {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //            SetMemIO   内存I/O位号和值  设置指定的内存I/O位
                //1:          打开此位
                //0:          关闭此位(Auto开 / Ready开)
                //----------------------------------------------------------
                
                if(TargetMemoryBit<0 | TargetMemoryBit>1023)
                    {
                    AddText("The parameter 'TargetMemoryBit' should be 0~65535, please revise it and retry.");
                    //MessageBox.Show("The parameter 'TargetMemoryBit' should be 0~65535, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                int TempInt=0;

                if(TurnOn==true)
                    {
                    TempInt=1;
                    }
                else
                    {
                    TempInt=0;
                    }

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$SetMemIO," + TargetMemoryBit + "," + TempInt + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //获得指定的内存字节（8位）的当前状态
        /// <summary>
        /// 获得指定的内存字节（8位）的当前状态
        /// </summary>
        /// <param name="TargetMemoryByte">目标内存字节【0~127】</param>
        /// <param name="ByteStatus">返回内存字节（8位）的当前状态</param>
        /// <returns></returns>
        public bool GetMemoryByte(int TargetMemoryByte, ref  byte ByteStatus)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //GetMemIOByte   内存I/O端口号   获得指定的内存I/O端口（8位）  随时可用
            //---------------------------------------------------------------------

                ByteStatus=0;

                if(TargetMemoryByte > 127 | TargetMemoryByte < 0)
                    {
                    AddText("The parameter 'TargetMemoryByte' should be 0~127, please revise it and retry.");
                    MessageBox.Show("The parameter 'TargetMemoryByte' should be 0~127, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$GetMemIOByte," + TargetMemoryByte + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    //在此添加判断返回的位的值为0或者1
                    //GetMemIOByte   #GetMemIOByte,[字节（8 位）的十六进制字符串（00 到 FF）]终端
                    
                    string[] TempStr=FeedBackMessageFromRobot.Split(',');
                    
                    if(TempStr[0]!="#GetMemIOByte")
                        {
                        ExecutingBusy = false;
                        return false;
                        }

                    //ByteStatus = Convert.ToByte(FC.HexToDecimal(Strings.Right(FeedBackMessageFromRobot, 2)));
                    ByteStatus = Convert.ToByte(FC.HexToDecimal(TempStr[1]));

                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }
        
        //设置指定内存字节（8位）的状态
        /// <summary>
        /// 设置指定内存字节（8位）的状态
        /// </summary>
        /// <param name="TargetMemoryByte">目标内存字节（8位）: 【0~127】</param>
        /// <param name="ByteSetStatus">需要设置的内存字节状态</param>
        /// <returns></returns>
        public bool SetMemoryByte(int TargetMemoryByte, byte ByteSetStatus)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //SetMemIOByte    内存I/O端口号和值   设置指定的内存I/O端口（8位）   Auto开/Ready开
            //-----------------------------------------------------------------------------

                if(TargetMemoryByte < 0 | TargetMemoryByte > 127)
                    {
                    AddText("The parameter 'TargetMemoryByte' should be 0~127, please revise it and retry.");
                    MessageBox.Show("The parameter 'TargetMemoryByte' should be 0~127, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$SetMemIOByte," 
                        + TargetMemoryByte + "," + ByteSetStatus + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }
        
        //获得指定内存字（16位）的当前状态
        /// <summary>
        /// 获得指定内存字（16位）的当前状态
        /// </summary>
        /// <param name="TargetMemoryWord">目标内存字（16位）: 【0~63】</param>
        /// <param name="WordStatus">返回的内存字状态</param>
        /// <returns></returns>
        public bool GetMemoryWord(int TargetMemoryWord, ref ushort WordStatus)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //GetMemIOWord   内存I/O字端口号   获得指定的内存I/O字端口（16位）  随时可用
            //--------------------------------------------------------------------------

                if(TargetMemoryWord > 63 | TargetMemoryWord < 0)
                    {
                    AddText("The parameter 'TargetMemoryWord' should be 0~63, please revise it and retry.");
                    MessageBox.Show("The parameter 'TargetMemoryWord' should be 0~63, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$GetMemIOWord," + TargetMemoryWord + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    //在此添加判断返回的位的值为0或者1
                    //GetIOMemWord   #GetMemIOWord,[字（16 位）的十六进制字符串（0000 至 FFFF）]终端

                    string[] TempStr=FeedBackMessageFromRobot.Split(',');
                    
                    if(TempStr[0]!="#GetMemIOWord")
                        {
                        ExecutingBusy = false;
                        return false;
                        }

                    //WordStatus =Convert.ToUInt16(FC.HexToDecimal(Strings.Right(FeedBackMessageFromRobot, 4)));
                    WordStatus =Convert.ToUInt16(FC.HexToDecimal(TempStr[1]));

                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }
        
        //设置指定内存字（16位）的状态
        /// <summary>
        /// 设置指定内存字（16位）的状态
        /// </summary>
        /// <param name="TargetMemoryWord">目标内存字（16位）: 【0~63】</param>
        /// <param name="WordSetStatus">需要设置的内存字状态</param>
        /// <returns></returns>
        public bool SetMemoryWord(int TargetMemoryWord, ushort WordSetStatus)
            {

            try
                {
            //远程以太网命令格式：$ 远程命令{, parameter....} 终端
            //SetMemIOWord    内存I/O字端口号和值    设置指定的内存I/O字端口（16位）  Auto开/Ready开
            //--------------------------------------------------------------------------------------

                if(TargetMemoryWord > 63 | TargetMemoryWord < 0)
                    {
                    AddText("The parameter 'TargetMemoryWord' should be 0~63, please revise it and retry.");
                    //MessageBox.Show("The parameter 'TargetMemoryWord' should be 0~63, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$SetMemIOWord," + 
                        TargetMemoryWord + "," + WordSetStatus + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }
        
        //将功率模式设为High【高功率】
        /// <summary>
        /// 将功率模式设为High【高功率】
        /// </summary>
        /// <returns></returns>
        public bool PowerHigh()
            {

            try
                {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

            //Power
            //用于将功率模式设为High 或Low，并显示当前的模式。
            //格式
            //(1) Power { High | Low }
            //(2) Power
            //参数
            //High | Low 设置High 或Low。默认设置为Low。
            //结果
            //如果省略参数，则显示当前的功率模式。
            //说明
            //用于将功率模式设为High 或Low。另外，显示当前的功率模式。
            //Low ： 如果将功率模式设为Low，低功率模式则会变为ON 状态。这表示机械手缓慢地（250mm/sec
            //以下的速度）进行动作。另外，将电动机功率输出限制在较低水平。
            //High ： 如果将功率模式设为High，低功率模式则会变为OFF状态。这表示机械手以由Speed、Accel、
            //SpeedS、AccelS 指定的速度、加减速度进行动作。

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute, \"Power High\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //将功率模式设为Low【低功率】
        /// <summary>
        /// 将功率模式设为Low【低功率】
        /// </summary>
        /// <returns></returns>
        public bool PowerLow()
            {

            try
                {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                //Power
                //用于将功率模式设为High 或Low，并显示当前的模式。
                //格式
                //(1) Power { High | Low }
                //(2) Power
                //参数
                //High | Low 设置High 或Low。默认设置为Low。
                //结果
                //如果省略参数，则显示当前的功率模式。
                //说明
                //用于将功率模式设为High 或Low。另外，显示当前的功率模式。
                //Low ： 如果将功率模式设为Low，低功率模式则会变为ON 状态。这表示机械手缓慢地（250mm/sec
                //以下的速度）进行动作。另外，将电动机功率输出限制在较低水平。
                //High ： 如果将功率模式设为High，低功率模式则会变为OFF状态。这表示机械手以由Speed、Accel、
                //SpeedS、AccelS 指定的速度、加减速度进行动作。

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Power Low\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //设置Speed速度值
        /// <summary>
        /// 设置Speed速度值
        /// </summary>
        /// <param name="TargetSpeed">目标速度: 【1~100%】</param>
        /// <returns></returns>
        public bool SetSpeed(int TargetSpeed)
            {

            try
                {
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------

                if(TargetSpeed > 100 | TargetSpeed < 1)
                    {
                    AddText("The parameter 'TargetSpeed' should be 1 to 100, please revise it and retry.");
                    MessageBox.Show("The parameter 'TargetSpeed' should be 1 to 100, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

            //Speed
            //用于设置/显示利用Go、Jump、Pulse 命令等的PTP 动作速度。
            //格式
            //(1) Speed 速度设定值 [, 转移速度, 接近速度]
            //(2) Speed
            //参数
            //速度设定值 以表达式或数值指定相对于最大动作速度（PTP 动作）的比例（1～100 的整数，单
            //位：%）。
            //转移速度 以表达式或数值指定Jump 命令时的转移动作速度（1～100 的整数，单位：%）。
            //可省略。仅Jump 命令时可设置。
            //接近速度 以表达式或数值指定Jump 命令时的接近动作速度（1～100 的整数，单位：%）。
            //可省略。仅Jump 命令时可设置。
            //结果
            //如果省略参数，则显示当前的Speed 设定值。
            //说明
            //Speed 用于指定所有PTP 动作命令的速度。其中包括有关Go、Jump、Pulse 等动作命令的速度设置。
            //速度设置是指以1～100 的整数指定相对于最大速度的比例（%）。如果指定“100”，则以最大速度进
            //行动作。

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Speed " + TargetSpeed + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //用于解除指定关节的SFree，并重新开启电机励磁/使能
        /// <summary>
        /// 用于解除指定关节的SFree，并重新开启电机励磁/使能
        /// </summary>
        /// <param name="TargetAxis">目标轴: 【1~6】</param>
        /// <returns></returns>
        public bool SLock(int TargetAxis)
            {

            try
                {

                if(TargetAxis < 0 | TargetAxis > 6)
                    {
                    AddText("The parameter 'TargetAxis' should be 1~6, please revise it and retry.");
                    //MessageBox.Show("The parameter 'TargetAxis' should be 1~6, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------

                //SLock
                //用于解除指定关节的SFree，并重新开始电动机励磁。
                //格式
                //SLock 关节编号 [关节编号,...]
                //参数
                //关节编号 以表达式或数值指定关节编号（1～9 的整数）。
                //附加轴的S 轴为8，T 轴为9。
                //说明
                //SLock 用于重新开始对因SFree 命令而进入SFree 状态的关节的电动机进行励磁，以便进行直接示教
                //或安装工件等。
                //如果省略关节编号，则重新开始对所有关节的电动机进行励磁。
                //如果对第3 关节重新进行励磁，电磁制动器则会被解除。
                //可替代SLock，使用Motor On 进行所有关节的励磁。
                //如果在Motor Off 状态下执行SLock，则会发生错误。
                //如果执行SLock 命令，则会对机械手控制参数进行初始化。

                //SLock 1, 2 '对J1 和J2 进行励磁

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"SLock " +
                        TargetAxis + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }
        
        //用于解除所有关节的SFree，并重新开启电机励磁/使能
        /// <summary>
        /// 用于解除所有关节的SFree，并重新开启电机励磁/使能
        /// </summary>
        /// <param name="AxisQty">轴数量：【4或6】</param>
        /// <returns></returns>
        public bool SLockAll(int AxisQty)
            {

            try
                {

                if(AxisQty != 4 & AxisQty !=6)
                    {
                    AddText("The parameter 'AxisQty' should be 4 or 6, please revise it and retry.");
                    //MessageBox.Show("The parameter 'AxisQty' should be 4 or 6, please revise it and retry.");
                    return false;
                    }

                bool TempResult=true;
                for(ushort a=1; a<=AxisQty;a++)
                    {
                    TempResult &=SLock(a);                    
                    }

                if (TempResult == true)
                    {
                    //ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    //ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                //ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //关闭指定关节的电机励磁/使能
        /// <summary>
        /// 关闭指定关节的电机励磁/使能
        /// </summary>
        /// <param name="TargetAxis">目标轴: 【1~6】</param>
        /// <returns></returns>
        public bool SFree(int TargetAxis)
            {

            try
                {
                
                if(TargetAxis < 0 | TargetAxis > 6)
                    {
                    AddText("The parameter 'TargetAxis' should be 1~6, please revise it and retry.");
                    //MessageBox.Show("The parameter 'TargetAxis' should be 1~6, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");
                
                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //SFree
                //用于切断指定关节的电动机电源。
                //格式
                //SFree 关节编号[, 关节编号,...]
                //参数
                //关节编号 以表达式或数值指定关节编号（1～9 的整数）。
                //附加轴的S 轴为8，T 轴为9。
                //说明
                //SFree 用于切断指定关节的电动机电源。此时的状态称为SFree。该命令用于进行直接示教，或仅切
                //断特定关节的励磁进行嵌入等。要再次对该关节进行励磁时，执行SLock 命令或Motor On。
                //如果执行SFree 命令，则会对机械手控制参数进行初始化。
                //详情请参阅Motor On。
                //注意
                //执行SFree 时，部分系统设置会被初始化
                //SFree 用于对有关机械手动作速度或加减速度的参数（Speed、SpeedS、Accel、AccelS）和LimZ 参
                //数进行初始化，以确保安全。

                //SFree 1, 2 '将J1 和J2 设为非励磁状态，然后移动Z 和U 关节以安装部件

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"SFree " + TargetAxis + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //关闭所有关节的电机励磁/使能
        /// <summary>
        /// 关闭所有关节的电机励磁/使能
        /// </summary>
        /// <param name="AxisQty">轴数量：【4或6】</param>
        /// <returns></returns>
        public bool SFreeAll(int AxisQty)
            {

            try
                {

                if (AxisQty != 4 & AxisQty != 6)
                    {
                    AddText("The parameter 'AxisQty' should be 4 or 6, please revise it and retry.");
                    //MessageBox.Show("The parameter 'AxisQty' should be 4 or 6, please revise it and retry.");
                    return false;
                    }

                bool TempResult = true;
                for (ushort a = 1; a <= AxisQty; a++)
                    {
                    TempResult &= SFree(a);
                    }

                if (TempResult == true)
                    {
                    //ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    //ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                //ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //锁定指定关节的刹车
        /// <summary>
        /// 锁定指定关节的刹车
        /// </summary>
        /// <param name="TargetAxis">目标轴: 【1~6】</param>
        /// <returns></returns>
        public bool BrakeOn(int TargetAxis)
            {

            try
                {

                if (TargetAxis < 1 | TargetAxis > 6)
                    {
                    AddText("The parameter 'TargetAxis' should be 1~6, please revise it and retry.");
                    //MessageBox.Show("The parameter 'TargetAxis' should be 1~6, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //Brake
                //用于打开和关闭当前机械手指定关节的制动器。
                //格式
                //Brake 状态, 关节编号
                //参数
                //状态 施加制动时：使用On。
                //解除制动时：使用Off。
                //关节编号 指定1～6 的关节编号。
                //说明
                //Brake 命令用于对垂直6 轴型机械手的一个关节施加或解除制动。这是仅可通过命令使用的命令。此
                //命令设计为只有维修作业人员才可以使用。
                //如果执行Brake 命令，则会对机械手控制参数进行初始化。
                
                //brake on, 1
                //brake off, 1

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Brake on, " + 
                        TargetAxis + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //锁定机械手所有关节的刹车
        /// <summary>
        /// 锁定机械手所有关节的刹车
        /// </summary>
        /// <param name="AxisQtyOfRobot">目标机械手的总轴数量: 【4或者6】</param>
        /// <returns></returns>
        public bool AllBrakeOn(int AxisQtyOfRobot)
            {

            try
                {

                if (AxisQtyOfRobot != 4 & AxisQtyOfRobot != 6)
                    {
                    AddText("The parameter 'AxisQtyOfRobot' should be 4 or 6, please revise it and retry.");
                    //MessageBox.Show("The parameter 'AxisQtyOfRobot' should be 4 or 6, please revise it and retry.");
                    return false;
                    }

                bool TempRet = true;

                if (AxisQtyOfRobot == 4)
                    {
                    for (int a = 0; a < 4; a++)
                        {
                        //TempRet &= BrakeOn(a + 1);
                        TempRet =TempRet & BrakeOn((a + 1));
                        }
                    }
                else if (AxisQtyOfRobot == 6)
                    {
                    for (int a = 0; a < 6; a++)
                        {
                        //TempRet &= BrakeOn(a + 1);
                        TempRet = TempRet & BrakeOn((a + 1));
                        }
                    }

                if (TempRet == true)
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
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //松开指定关节的刹车
        /// <summary>
        /// 松开指定关节的刹车
        /// </summary>
        /// <param name="TargetAxis">目标轴: 【1~6】</param>
        /// <returns></returns>
        public bool BrakeOff(int TargetAxis)
            {

            try
                {

                if (TargetAxis < 1 | TargetAxis > 6)
                    {
                    AddText("The parameter 'TargetAxis' should be 1~6, please revise it and retry.");
                    //MessageBox.Show("The parameter 'TargetAxis' should be 1~6, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //Brake
                //用于打开和关闭当前机械手指定关节的制动器。
                //格式
                //Brake 状态, 关节编号
                //参数
                //状态 施加制动时：使用On。
                //解除制动时：使用Off。
                //关节编号 指定1～6 的关节编号。
                //说明
                //Brake 命令用于对垂直6 轴型机械手的一个关节施加或解除制动。这是仅可通过命令使用的命令。此
                //命令设计为只有维修作业人员才可以使用。
                //如果执行Brake 命令，则会对机械手控制参数进行初始化。
                
                //brake on, 1
                //brake off, 1

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Brake off, " + 
                        TargetAxis + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //松开机械手所有关节的刹车
        /// <summary>
        /// 松开机械手所有关节的刹车
        /// </summary>
        /// <param name="AxisQtyOfRobot">目标机械手的总轴数量: 【4或者6】</param>
        /// <returns></returns>
        public bool AllBrakeOff(int AxisQtyOfRobot)
            {

            try
                {

                if (AxisQtyOfRobot != 4 & AxisQtyOfRobot != 6)
                    {
                    AddText("The parameter 'AxisQtyOfRobot' should be 4 or 6, please revise it and retry.");
                    //MessageBox.Show("The parameter 'AxisQtyOfRobot' should be 4 or 6, please revise it and retry.");
                    return false;
                    }

                bool TempRet = true;

                if (AxisQtyOfRobot == 4)
                    {
                    for (int a = 0; a < 4; a++)
                        {
                        //TempRet &= BrakeOn(a + 1);
                        TempRet = TempRet & BrakeOn((a + 1));
                        }
                    }
                else if (AxisQtyOfRobot == 6)
                    {
                    for (int a = 0; a < 6; a++)
                        {
                        //TempRet &= BrakeOn(a + 1);
                        TempRet = TempRet & BrakeOff((a + 1));
                        }
                    }

                if (TempRet == true)
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
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //设置利用Go、Jump、Pulse等的PTP动作的加减速度
        /// <summary>
        /// 设置利用Go、Jump、Pulse等的PTP动作的加减速度
        /// </summary>
        /// <param name="TargetAccelSpeed">目标加速度: 【1~100】</param>
        /// <param name="TargetDecelSpeed">目标减速度: 【1~100】</param>
        /// <returns></returns>
        public bool SetACCELSpeed(ushort TargetAccelSpeed, ushort TargetDecelSpeed)
            {

            try
                {

                if (TargetAccelSpeed < 1 | TargetAccelSpeed > 100)
                    {
                    AddText("The parameter 'TargetAccelSpeed/TargetDecelSpeed' should be 1~100, please revise it and retry.");
                    //MessageBox.Show("The parameter 'TargetAccelSpeed/TargetDecelSpeed' should be 1~100, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //Accel
                //用于设置和显示利用Go、Jump、Pulse 等的PTP 动作的加减速度。
                //格式
                //(1) Accel 加速设定值, 减速设定值, [转移加速设定值, 转移减速设定值, 接近加速设定值, 接近减
                //速设定值]
                //(2) Accel
                //参数
                //加速设定值 以大于1 的整数指定相对于最大加速度的比例。（单位：%）
                //减速设定值 以大于1 的整数指定相对于最大减速度的比例。（单位：%）
                //转移加速设定值 以大于1 的整数指定Jump 时的转移加速度。
                //可省略。仅Jump 命令时可设置。
                //转移减速设定值 以大于1 的整数指定Jump 时的转移减速度。
                //可省略。仅Jump 命令时可设置。
                //接近加速设定值 以大于1 的整数指定Jump 时的接近加速度。
                //可省略。仅Jump 命令时可设置。
                //接近减速设定值 以大于1 的整数指定Jump 时的接近减速度。
                //可省略。仅Jump 命令时可设置。
                //结果
                //如果省略参数，将返回当前的Accel 参数。
                
                //加速设定值 以大于1 的整数指定相对于最大加速度的比例。（单位：%）
                //减速设定值 以大于1 的整数指定相对于最大减速度的比例。（单位：%）

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute\"Accel " +
                        TargetAccelSpeed + "," + TargetDecelSpeed + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //X轴正(+)方向MOVE一定距离
        /// <summary>
        /// X轴+方向MOVE一定距离
        /// </summary>
        /// <param name="Distance">移动距离</param>
        /// <returns></returns>
        public bool XMovePositive(double Distance)
            {

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //$Execute,"print realpos"
                //#Execute," X: -182.632 Y:  240.114 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here +x(-1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -183.630 Y:  240.115 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here -x(1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -184.631 Y:  240.114 Z:  -20.002 U:  -36.155 V:    0.000 W:    0.000 /R /0
                
                //************************************************
                //【move here -x(1)和move here +x(1)的作用是一样的??????】
                //************************************************
                
                //> move here +x(1)
                //> move here -x(1)

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move Here +X(" +
                        Distance + ")" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //X轴-方向MOVE一定距离
        /// <summary>
        /// X轴负(-)方向MOVE一定距离
        /// </summary>
        /// <param name="Distance">移动距离</param>
        /// <returns></returns>
        public bool XMoveNegative(double Distance)
            {

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //$Execute,"print realpos"
                //#Execute," X: -182.632 Y:  240.114 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here +x(-1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -183.630 Y:  240.115 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here -x(1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -184.631 Y:  240.114 Z:  -20.002 U:  -36.155 V:    0.000 W:    0.000 /R /0
                
                //************************************************
                //【move here -x(1)和move here +x(1)的作用是一样的??????】
                //************************************************
                
                //> move here +x(1)
                //> move here -x(1)

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move Here -X(" +
                        Distance + ")" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //Y轴正(+)方向MOVE一定距离
        /// <summary>
        /// Y轴正(+)方向MOVE一定距离
        /// </summary>
        /// <param name="Distance">移动距离</param>
        /// <returns></returns>
        public bool YMovePositive(double Distance)
            {

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //$Execute,"print realpos"
                //#Execute," X: -182.632 Y:  240.114 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here +x(-1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -183.630 Y:  240.115 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here -x(1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -184.631 Y:  240.114 Z:  -20.002 U:  -36.155 V:    0.000 W:    0.000 /R /0
                
                //************************************************
                //【move here -x(1)和move here +x(1)的作用是一样的??????】
                //************************************************
                
                //> move here +y(1)
                //> move here -y(1)

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move Here +Y(" +
                        Distance + ")" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //Y轴负(-)方向MOVE一定距离
        /// <summary>
        /// Y轴负(-)方向MOVE一定距离
        /// </summary>
        /// <param name="Distance">移动距离</param>
        /// <returns></returns>
        public bool YMoveNegative(double Distance)
            {

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //$Execute,"print realpos"
                //#Execute," X: -182.632 Y:  240.114 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here +x(-1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -183.630 Y:  240.115 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here -x(1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -184.631 Y:  240.114 Z:  -20.002 U:  -36.155 V:    0.000 W:    0.000 /R /0
                
                //************************************************
                //【move here -x(1)和move here +x(1)的作用是一样的??????】
                //************************************************
                
                //> move here +y(1)
                //> move here -y(1)

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move Here -Y(" +
                        Distance + ")" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //Z轴向上(+)运动一定距离
        /// <summary>
        /// Z轴向上(+)运动一定距离
        /// </summary>
        /// <param name="Distance">移动距离</param>
        /// <returns></returns>
        public bool ZMoveUp(double Distance)
            {

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //$Execute,"print realpos"
                //#Execute," X: -182.632 Y:  240.114 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here +x(-1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -183.630 Y:  240.115 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here -x(1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -184.631 Y:  240.114 Z:  -20.002 U:  -36.155 V:    0.000 W:    0.000 /R /0
                
                //************************************************
                //【move here -x(1)和move here +x(1)的作用是一样的??????】
                //************************************************
                
                //> move here +Z(1)
                //> move here -Z(1)

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move Here +Z(" +
                        Distance + ")" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //Z轴向下(-)运动一定距离
        /// <summary>
        /// Z轴向下(-)运动一定距离
        /// </summary>
        /// <param name="Distance">移动距离</param>
        /// <returns></returns>
        public bool ZMoveDown(double Distance)
            {

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //$Execute,"print realpos"
                //#Execute," X: -182.632 Y:  240.114 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here +x(-1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -183.630 Y:  240.115 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here -x(1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -184.631 Y:  240.114 Z:  -20.002 U:  -36.155 V:    0.000 W:    0.000 /R /0
                
                //************************************************
                //【move here -x(1)和move here +x(1)的作用是一样的??????】
                //************************************************
                
                //> move here +Z(1)
                //> move here -Z(1)

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move Here -Z(" +
                        Distance + ")" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //U轴正转(+)一定角度
        /// <summary>
        /// U轴正转(+)一定角度
        /// </summary>
        /// <param name="Degree">旋转角度</param>
        /// <returns></returns>
        public bool URotatePositive(double Degree)
            {

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //$Execute,"print realpos"
                //#Execute," X: -182.632 Y:  240.114 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here +x(-1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -183.630 Y:  240.115 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here -x(1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -184.631 Y:  240.114 Z:  -20.002 U:  -36.155 V:    0.000 W:    0.000 /R /0
                
                //************************************************
                //【move here -x(1)和move here +x(1)的作用是一样的??????】
                //************************************************
                
                //> move here +U(1)
                //> move here -U(1)

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move Here +U(" +
                        Degree + ")" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //U轴反转(-)一定角度
        /// <summary>
        /// U轴反转(-)一定角度
        /// </summary>
        /// <param name="Degree">旋转角度</param>
        /// <returns></returns>
        public bool URotateNegative(double Degree)
            {

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //$Execute,"print realpos"
                //#Execute," X: -182.632 Y:  240.114 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here +x(-1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -183.630 Y:  240.115 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here -x(1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -184.631 Y:  240.114 Z:  -20.002 U:  -36.155 V:    0.000 W:    0.000 /R /0
                
                //************************************************
                //【move here -x(1)和move here +x(1)的作用是一样的??????】
                //************************************************
                
                //> move here +U(1)
                //> move here -U(1)

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move Here -U(" +
                        Degree + ")" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //V轴正(+)方向MOVE一定距离
        /// <summary>
        /// V轴正(+)方向MOVE一定距离
        /// </summary>
        /// <param name="Distance">移动距离</param>
        /// <returns></returns>
        public bool VMovePositive(double Distance)
            {

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //$Execute,"print realpos"
                //#Execute," X: -182.632 Y:  240.114 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here +x(-1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -183.630 Y:  240.115 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here -x(1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -184.631 Y:  240.114 Z:  -20.002 U:  -36.155 V:    0.000 W:    0.000 /R /0
                
                //************************************************
                //【move here -x(1)和move here +x(1)的作用是一样的??????】
                //************************************************
                
                //> move here +V(1)
                //> move here -V(1)

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move Here +V(" +
                        Distance + ")" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //V轴负(-)方向MOVE一定距离
        /// <summary>
        /// V轴负(-)方向MOVE一定距离
        /// </summary>
        /// <param name="Distance">移动距离</param>
        /// <returns></returns>
        public bool VMoveNegative(double Distance)
            {

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //$Execute,"print realpos"
                //#Execute," X: -182.632 Y:  240.114 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here +x(-1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -183.630 Y:  240.115 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here -x(1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -184.631 Y:  240.114 Z:  -20.002 U:  -36.155 V:    0.000 W:    0.000 /R /0
                
                //************************************************
                //【move here -x(1)和move here +x(1)的作用是一样的??????】
                //************************************************
                
                //> move here +V(1)
                //> move here -V(1)

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move Here -V(" +
                        Distance + ")" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //W轴正(+)方向MOVE一定距离
        /// <summary>
        /// W轴正(+)方向MOVE一定距离
        /// </summary>
        /// <param name="Distance">移动距离</param>
        /// <returns></returns>
        public bool WMovePositive(double Distance)
            {

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //$Execute,"print realpos"
                //#Execute," X: -182.632 Y:  240.114 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here +x(-1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -183.630 Y:  240.115 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here -x(1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -184.631 Y:  240.114 Z:  -20.002 U:  -36.155 V:    0.000 W:    0.000 /R /0
                
                //************************************************
                //【move here -x(1)和move here +x(1)的作用是一样的??????】
                //************************************************
                
                //> move here +W(1)
                //> move here -W(1)

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move Here +W(" +
                        Distance + ")" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //W轴负(-)方向MOVE一定距离
        /// <summary>
        /// W轴负(-)方向MOVE一定距离
        /// </summary>
        /// <param name="Distance">移动距离</param>
        /// <returns></returns>
        public bool WMoveNegative(double Distance)
            {

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //$Execute,"print realpos"
                //#Execute," X: -182.632 Y:  240.114 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here +x(-1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -183.630 Y:  240.115 Z:  -20.002 U:  -36.156 V:    0.000 W:    0.000 /R /0"
                //$Execute,"move here -x(1)"
                //#Execute,0
                //$Execute,"print realpos"
                //#Execute," X: -184.631 Y:  240.114 Z:  -20.002 U:  -36.155 V:    0.000 W:    0.000 /R /0
                
                //************************************************
                //【move here -x(1)和move here +x(1)的作用是一样的??????】
                //************************************************
                
                //> move here +W(1)
                //> move here -W(1)

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move Here -W(" +
                        Distance + ")" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //相对运动：在当前位置执行MOVE运动到某个位置
        /// <summary>
        /// 相对运动：在当前位置执行MOVE运动到某个位置
        /// </summary>
        /// <param name="X">X 轴方向移动距离</param>
        /// <param name="Y">Y 轴方向移动距离</param>
        /// <param name="Z">Z 轴方向移动距离</param>
        /// <param name="U">U 轴方向移动距离</param>
        /// <param name="V">V 轴方向移动距离</param>
        /// <param name="W">W 轴方向移动距离</param>
        /// <param name="HandStyle">手势</param>
        /// <returns></returns>
        public bool MoveRelative(double X=0.0, double Y=0.0, double Z=0.0, 
            double U=0.0, double V=0.0, double W=0.0, Hand HandStyle=Hand.LeftHand)
            {

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                string TempStr="";
                if(HandStyle==Hand.LeftHand)
                    {
                    TempStr="/L";
                    }
                else
                    {
                    TempStr="/R";
                    }

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //相对运动
                //> move here +x(1)
                //> move here -x(1)

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    //FeedBackMessageFromRobot = SendCommand("$Execute,\"Move Here +X(" + X + 
                    //    ") +Y(" + Y + ") +Z(" + Z + ") +U(" + U + ") +V(" + V 
                    //    + ") +W(" + W + ")" + TempStr + "\"" + Suffix);
                    
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Move Here +XY(" + X + 
                        "," + Y + "," + Z + "," + U + "," + V + "," + W + ")" + TempStr + "\"" + Suffix);

                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //相对定位：在当前位置执行GO运动到某个位置
        /// <summary>
        /// 相对定位：在当前位置执行GO运动到某个位置
        /// </summary>
        /// <param name="X">X 轴方向移动距离</param>
        /// <param name="Y">Y 轴方向移动距离</param>
        /// <param name="Z">Z 轴方向移动距离</param>
        /// <param name="U">U 轴方向移动距离</param>
        /// <param name="V">V 轴方向移动距离</param>
        /// <param name="W">W 轴方向移动距离</param>
        /// <param name="HandStyle">手势</param>
        /// <returns></returns>
        public bool GoRelative(double X=0.0, double Y=0.0, double Z=0.0,
            double U=0.0, double V=0.0, double W=0.0, Hand HandStyle=Hand.LeftHand)
            {

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                string TempStr="";
                if(HandStyle==Hand.LeftHand)
                    {
                    TempStr="/L";
                    }
                else
                    {
                    TempStr="/R";
                    }

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //相对运动
                //> Go here +x(1)
                //> Go here -x(1)

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    //FeedBackMessageFromRobot = SendCommand("$Execute,\"Go Here +X(" + X + 
                    //    ") +Y(" + Y + ") +Z(" + Z + ") +U(" + U + ") +V(" + V 
                    //    + ") +W(" + W + ")" + TempStr + "\"" + Suffix);
                    
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Go Here +XY(" + X + 
                        "," + Y + "," + Z + "," + U + "," + V + "," + W + ")" + TempStr + "\"" + Suffix);

                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }
        
        //获取当前的Speed 设定值
        /// <summary>
        /// 获取当前的Speed 设定值
        /// </summary>
        /// <returns>返回-1代表获取失败</returns>
        public int GetSpeed()
            {

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return -1;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return -1;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                                
                //Speed
                //用于设置/显示利用Go、Jump、Pulse 命令等的PTP 动作速度。
                //格式
                //(2) Speed
                //如果省略参数，则显示当前的Speed 设定值。
                //说明
                //Speed 用于指定所有PTP 动作命令的速度。其中包括有关Go、Jump、Pulse 等动作命令的速度设置。
                //速度设置是指以1～100 的整数指定相对于最大速度的比例（%）。如果指定“100”，则以最大速度进
                //行动作。

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Speed" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return -1;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    
                    //$Execute,"Speed"
                    //#Execute,"Low Power Mode
                    //1
                    //1	   1

                    string[] TempStr=Strings.Split(FeedBackMessageFromRobot,"\r\n");
                    
                    if(TempStr.Length!=3)//(TempStr[0]!="#Execute")
                        {
                        ExecutingBusy = false;
                        return -1;
                        }

                    ExecutingBusy = false;
                    return Convert.ToUInt16(TempStr[1]);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return -1;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return -1;
                //MessageBox.Show(ex.Message);
                }

            }

        //返回利用Go、Jump、Pulse 等的PTP 动作的加减速度
        /// <summary>
        /// 返回利用Go、Jump、Pulse 等的PTP 动作的加减速度
        /// </summary>
        /// <returns>返回-1代表获取失败</returns>
        public Accel GetAccelSpeed()
            {

            Accel TempAccel;
            TempAccel.AccelSpeed=-1;
            TempAccel.DecelSpeed=-1;

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return TempAccel;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return TempAccel;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //Accel
                //用于设置和显示利用Go、Jump、Pulse 等的PTP 动作的加减速度。
                //格式
                //(2) Accel
                //如果省略参数，将返回当前的Accel 参数
                
                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Accel" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return TempAccel;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    //加速设定值 以大于1 的整数指定相对于最大加速度的比例。（单位：%）
                    //减速设定值 以大于1 的整数指定相对于最大减速度的比例。（单位：%）
                    //$Execute,"Accel"
                    //#Execute,"Low Power Mode
                    //10	  10
                    //10	  10
                    //10	  10

                    //添加返回结果值的代码，需要测试返回结果才能处理【需要验证】
                    string[] TempStr;//,TempStr2;
                    TempStr=Strings.Split(FeedBackMessageFromRobot,"\r\n");
                    //TempStr2=TempStr[1].Split(',');
                    //if(TempStr2[0]!="#Execute")
                    //    {
                    //    ExecutingBusy = false;                        
                    //    return TempAccel;
                    //    }

                    if(TempStr.Length < 4)
                        {
                        ExecutingBusy = false;
                        return TempAccel;
                        }

                    TempStr=TempStr[1].Split(' ');

                    TempAccel.AccelSpeed=Convert.ToInt32(TempStr[0]);
                    TempAccel.DecelSpeed=Convert.ToInt32(TempStr[1]);

                    ExecutingBusy = false;
                    return TempAccel;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return TempAccel;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return TempAccel;
                //MessageBox.Show(ex.Message);
                }

            }

        //获取当前的功率模式
        /// <summary>
        /// 获取当前的功率模式
        /// </summary>
        /// <returns></returns>
        public Power GetPowerStatus()
            {

            Power TempPower = Power.Unknow;

            try
                {

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return TempPower;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return TempPower;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //Power
                //用于将功率模式设为High 或Low，并显示当前的模式。
                //格式
                //(2) Power
                //参数
                //High | Low 设置High 或Low。默认设置为Low。
                //结果
                //如果省略参数，则显示当前的功率模式。

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Speed" + "\"" + Suffix);
                    //FeedBackMessageFromRobot = SendCommand("$Execute,\"Power" + "\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return TempPower;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    //$Execute,"Power"
                    //!Execute,99
                    
                    //$Execute,"Speed"
                    //#Execute,"Low Power Mode
                    //1
                    //1	   1
                    
                    //**************
                    //注意：用Split(\r\n)之后，其它的前面都有\r\n,所有要去掉这个才能得到正确值【需要验证】
                    //**************

                    string[] TempStr;
                    string TempRet;
                    TempStr=Strings.Split(FeedBackMessageFromRobot,"\r\n");
                    TempRet=TempStr[0].ToUpper();

                    if(Strings.InStr(TempRet,"LOW")!=-1)
                        {
                        TempPower=Power.Low;
                        }
                    else if(Strings.InStr(TempRet,"HIGH")!=-1)
                        {
                        TempPower=Power.High;
                        }
                    else
                        {
                        TempPower=Power.Unknow;
                        }

                    ExecutingBusy = false;
                    return TempPower;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return TempPower;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return TempPower;
                //MessageBox.Show(ex.Message);
                }

            }

        //【需要验证】?????
        //获取某个输出字节的当前状态
        /// <summary>
        /// 获取某个输出字节的当前状态
        /// </summary>
        /// <param name="TargetByteNumber">目标输出字【0:(代表位：0-7),1:(代表位：8-15)】</param>
        /// <param name="CurrentByteStatus">目标输出字的当前状态值【字节】</param>
        /// <returns></returns>
        public bool GetOutByteStatus(ushort TargetByteNumber, ref byte CurrentByteStatus)
            {

            try
                {

                if (TargetByteNumber < 0 | TargetByteNumber > 1)
                    {
                    AddText("The parameter 'TargetByteNumber' should be 0~1, please revise it and retry.");
                    //MessageBox.Show("The parameter 'TargetByteNumber' should be 0~1, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //Out(函数)
                //用于以字节为单位返回输出端口状态。
                //格式
                //Out(端口编号)
                //参数
                //端口编号 指定 I/O 的输出字节。按如下所述，指定数值对应各自的输出位。
                //端口编号(输出位)
                //0 0-7
                //1 8-15
                //... ...
                //返回值
                //以字节为单位返回指定输出端口的状态。

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"Out(" + TargetByteNumber + ")\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    //$Execute,"Out 0"
                    //!Execute,99

                    //【需要验证】?????
                    string[] TempStr;
                    TempStr=Strings.Split(FeedBackMessageFromRobot,",");
                    CurrentByteStatus=Convert.ToByte(TempStr[1]);

                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

        //【需要验证】?????
        //获取某个输出字的当前状态
        /// <summary>
        /// 获取某个输出字的当前状态
        /// </summary>
        /// <param name="TargetWordNumber">目标输出字: 【0】</param>
        /// <param name="CurrentWordStatus">目标输出字的当前状态值</param>
        /// <returns></returns>
        public bool GetOutWordStatus(ushort TargetWordNumber, ref ushort CurrentWordStatus)
            {

            try
                {

                CurrentWordStatus = 0;

                if (TargetWordNumber != 6)
                    {
                    AddText("The parameter 'TargetWordNumber' should be 0, please revise it and retry.");
                    //MessageBox.Show("The parameter 'TargetWordNumber' should be 0, please revise it and retry.");
                    return false;
                    }

                //判断是否正在执行命令，不然会丢失指令
                if (ExecutingBusy == true)
                    {
                    AddText("正在执行其它命令...");
                    return false;
                    }
                else
                    {
                    ExecutingBusy = true;
                    }

                if (GetRobotStatus(ref FeedBackMessageFromRobot) == false)
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                int i;
                i = FeedBackMessageFromRobot.IndexOf(",");

                //远程以太网命令格式：$ 远程命令{, parameter....} 终端
                //Execute  命令字符串    执行命令       Auto开/Ready开/Error关/EStop关/Safeguard关
                //---------------------------------------------------------------------------
                
                //OutW 函数
                //用于以字为单位（双字节）返回输出端口状态。
                //格式
                //OutW（字端口编号）
                //参数
                //字端口编号 指定 I/O 的输出字。
                //返回值
                //以 16 位返回指定输出端口的状态。

                if (FeedBackMessageFromRobot[i + 3] == '1')  //& FeedBackMessageFromRobot[i + 11]=='1' 
                    {
                    FeedBackMessageFromRobot = SendCommand("$Execute,\"OutW(" + TargetWordNumber + ")\"" + Suffix);
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                if (FeedBackMessageFromRobot.IndexOf("!") == -1) //& FeedBackMessageFromRobot.Length > 40
                    {
                    //$Execute,"OutW 0"
                    //!Execute,99

                    //【需要验证】?????
                    string[] TempStr;
                    TempStr = Strings.Split(FeedBackMessageFromRobot, ",");
                    CurrentWordStatus = Convert.ToUInt16(TempStr[1]);

                    ExecutingBusy = false;
                    return true;
                    }
                else
                    {
                    ExecutingBusy = false;
                    return false;
                    }

                }
            catch (Exception ex)
                {
                ExecutingBusy = false;
                AddText(ex.Message);
                return false;
                //MessageBox.Show(ex.Message);
                }

            }

#endregion

        }//class

    }//namespace