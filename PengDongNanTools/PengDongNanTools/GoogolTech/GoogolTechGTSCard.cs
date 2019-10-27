#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Threading;

#endregion

namespace PengDongNanTools
    {

    //【二次封装】固高GTS系列运动控制卡运动卡控制类：初始化运动卡和更新IO
    /// <summary>
    /// 【二次封装】固高GTS系列运动控制卡运动卡控制类：初始化运动卡和更新IO
    /// 【软件作者：彭东南, southeastofstar@163.com】
    /// </summary>
    public class GoogolTechGTSCard
        {
        
        #region "固高函数导入及常量、数据结构定义"

        #region "固高常量及数据结构"

        public const short MC_NONE = -1;

        public const short MC_LIMIT_POSITIVE = 0;
        public const short MC_LIMIT_NEGATIVE = 1;
        public const short MC_ALARM = 2;
        public const short MC_HOME = 3;
        public const short MC_GPI = 4;
        public const short MC_ARRIVE = 5;

        public const short MC_ENABLE = 10;
        public const short MC_CLEAR = 11;
        public const short MC_GPO = 12;

        public const short MC_DAC = 20;
        public const short MC_STEP = 21;
        public const short MC_PULSE = 22;
        public const short MC_ENCODER = 23;
        public const short MC_ADC = 24;

        public const short MC_AXIS = 30;
        public const short MC_PROFILE = 31;
        public const short MC_CONTROL = 32;

        public const short CAPTURE_HOME = 1;
        public const short CAPTURE_INDEX = 2;
        public const short CAPTURE_PROBE = 3;
        public const short CAPTURE_HSIO0 = 6;
        public const short CAPTURE_HSIO1 = 7;

        public const short PT_MODE_STATIC = 0;
        public const short PT_MODE_DYNAMIC = 1;

        public const short PT_SEGMENT_NORMAL = 0;
        public const short PT_SEGMENT_EVEN = 1;
        public const short PT_SEGMENT_STOP = 2;

        public const short GEAR_MASTER_ENCODER = 1;
        public const short GEAR_MASTER_PROFILE = 2;
        public const short GEAR_MASTER_AXIS = 3;

        public const short FOLLOW_MASTER_ENCODER = 1;
        public const short FOLLOW_MASTER_PROFILE = 2;
        public const short FOLLOW_MASTER_AXIS = 3;

        public const short FOLLOW_EVENT_START = 1;
        public const short FOLLOW_EVENT_PASS = 2;

        public const short GEAR_EVENT_START = 1;
        public const short GEAR_EVENT_PASS = 2;
        public const short GEAR_EVENT_AREA = 5;

        public const short FOLLOW_SEGMENT_NORMAL = 0;
        public const short FOLLOW_SEGMENT_EVEN = 1;
        public const short FOLLOW_SEGMENT_STOP = 2;
        public const short FOLLOW_SEGMENT_CONTINUE = 3;

        public const short INTERPOLATION_AXIS_MAX = 4;
        public const short CRD_FIFO_MAX = 4096;
        public const short CRD_MAX = 2;
        public const short CRD_OPERATION_DATA_EXT_MAX = 2;

        public const short CRD_OPERATION_TYPE_NONE = 0;
        public const short CRD_OPERATION_TYPE_BUF_IO_DELAY = 1;
        public const short CRD_OPERATION_TYPE_LASER_ON = 2;
        public const short CRD_OPERATION_TYPE_LASER_OFF = 3;
        public const short CRD_OPERATION_TYPE_BUF_DA = 4;
        public const short CRD_OPERATION_TYPE_LASER_CMD = 5;
        public const short CRD_OPERATION_TYPE_LASER_FOLLOW = 6;
        public const short CRD_OPERATION_TYPE_LMTS_ON = 7;
        public const short CRD_OPERATION_TYPE_LMTS_OFF = 8;
        public const short CRD_OPERATION_TYPE_SET_STOP_IO = 9;
        public const short CRD_OPERATION_TYPE_BUF_MOVE = 10;
        public const short CRD_OPERATION_TYPE_BUF_GEAR = 11;
        public const short CRD_OPERATION_TYPE_SET_SEG_NUM = 12;
        public const short CRD_OPERATION_TYPE_STOP_MOTION = 13;
        public const short CRD_OPERATION_TYPE_SET_VAR_VALUE = 14;
        public const short CRD_OPERATION_TYPE_JUMP_NEXT_SEG = 15;
        public const short CRD_OPERATION_TYPE_SYNCH_PRF_POS = 16;
        public const short CRD_OPERATION_TYPE_VIRTUAL_TO_ACTUAL = 17;
        public const short CRD_OPERATION_TYPE_SET_USER_VAR = 18;
        public const short CRD_OPERATION_TYPE_SET_DO_BIT_PULSE = 19;

        public const short INTERPOLATION_MOTION_TYPE_LINE = 0;
        public const short INTERPOLATION_MOTION_TYPE_CIRCLE = 1;
        public const short INTERPOLATION_MOTION_TYPE_HELIX = 2;

        public const short INTERPOLATION_CIRCLE_PLAT_XY = 0;
        public const short INTERPOLATION_CIRCLE_PLAT_YZ = 1;
        public const short INTERPOLATION_CIRCLE_PLAT_ZX = 2;

        public const short INTERPOLATION_HELIX_CIRCLE_XY_LINE_Z = 0;
        public const short INTERPOLATION_HELIX_CIRCLE_YZ_LINE_X = 1;
        public const short INTERPOLATION_HELIX_CIRCLE_ZX_LINE_Y = 2;

        public const short INTERPOLATION_CIRCLE_DIR_CW = 0;
        public const short INTERPOLATION_CIRCLE_DIR_CCW = 1;

        /// <summary>
        /// 点位模式运动参数数据结构
        /// </summary>
        public struct TTrapPrm
            {
            public double acc;
            public double dec;
            public double velStart;
            public short smoothTime;
            }

        /// <summary>
        /// Jog模式运动参数数据结构
        /// </summary>
        public struct TJogPrm
            {
            public double acc;
            public double dec;
            public double smooth;
            }

        /// <summary>
        /// PID参数数据结构
        /// </summary>
        public struct TPid
            {
            public double kp;
            public double ki;
            public double kd;
            public double kvff;
            public double kaff;

            public int integralLimit;
            public int derivativeLimit;
            public short limit;
            }

        /// <summary>
        /// 线程状态数据结构
        /// </summary>
        public struct TThreadSts
            {
            public short run;
            public short error;
            public double result;
            public short line;
            }

        /// <summary>
        /// 运动程序函数名称和变量名称查询变量标识的数据结构
        /// </summary>
        public struct TVarInfo
            {
            public short id;
            public short dataType;
            public double dumb0;
            public double dumb1;
            public double dumb2;
            public double dumb3;
            }

        /// <summary>
        /// 坐标系相关参数的数据结构
        /// </summary>
        public struct TCrdPrm
            {
            public short dimension;
            public short profile1;
            public short profile2;
            public short profile3;
            public short profile4;
            public short profile5;
            public short profile6;
            public short profile7;
            public short profile8;

            public double synVelMax;
            public double synAccMax;
            public short evenTime;
            public short setOriginFlag;
            public long originPos1;
            public long originPos2;
            public long originPos3;
            public long originPos4;
            public long originPos5;
            public long originPos6;
            public long originPos7;
            public long originPos8;
            }

        /// <summary>
        /// 坐标系缓冲操作的数据结构
        /// </summary>
        public struct TCrdBufOperation
            {
            public short flag;
            public ushort delay;
            public short doType;
            public ushort doMask;
            public ushort doValue;
            public ushort dataExt1;
            public ushort dataExt2;
            }

        /// <summary>
        /// 前瞻缓存区的数据结构
        /// </summary>
        public struct TCrdData
            {
            public short motionType;
            public short circlePlat;
            public long posX;
            public long posY;
            public long posZ;
            public long posA;
            public double radius;
            public short circleDir;
            public double lCenterX;
            public double lCenterY;
            public double vel;
            public double acc;
            public short velEndZero;
            public TCrdBufOperation operation;

            public double cosX;
            public double cosY;
            public double cosZ;
            public double cosA;
            public double velEnd;
            public double velEndAdjust;
            public double r;
            }

        /// <summary>
        /// 
        /// </summary>
        public struct TTrigger
            {
            public short encoder;
            public short probeType;
            public short probeIndex;
            public short offset;
            public short windowOnly;
            public long firstPosition;
            public long lastPosition;
            }

        /// <summary>
        /// 
        /// </summary>
        public struct TTriggerStatus
            {
            public short execute;
            public short done;
            public long position;
            }

        #endregion
        
        #region "固高运动卡函数"

        //切换当前运动控制器卡号
        /// <summary>
        /// 切换当前运动控制器卡号
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="index">将被设置为当前运动控制器的卡号，取值范围：[0,15]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetCardNo(short cardNum, short index);

        //读取当前运动控制器卡号
        /// <summary>
        /// 读取当前运动控制器卡号
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="index">读取的当前运动控制器的卡号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetCardNo(short cardNum, out short index);

        //打开运动控制器
        /// <summary>
        /// 打开运动控制器
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="channel">打开运动控制器的方式，默认为：0
        /// 0：正常打开运动控制器
        /// 1：内部调试方式，用户不能使用
        /// </param>
        /// <param name="param">当channel=1时，该参数有效</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_Open(short cardNum, short channel, short param);
        
        //关闭运动控制器
        /// <summary>
        /// 关闭运动控制器
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_Close(short cardNum);

        //下载配置信息到运动控制器
        /// <summary>
        /// 下载配置信息到运动控制器
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="pFile">配置文件的文件名</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_LoadConfig(short cardNum, string pFile);

        //读取运动控制器固件的版本号
        /// <summary>
        /// 读取运动控制器固件的版本号
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="pVersion">读取的运动控制器的固件版本号字符串
        /// 版本号是一个含有18个字符的字符串：aaa bbbbbb ccc dddddd
        /// aaa -- 固件1的版本号，如100，即表示版本号为：1.00
        /// bbbbbb -- 固件1的版本号的生成时间，如090908，即表示该版本生成于：2009年9月8日
        /// ccc -- 固件2的版本号
        /// dddddd -- 固件2的版本号的生成时间
        /// </param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetVersion(short cardNum, out string pVersion);
        
        //设置数字IO输出状态
        /// <summary>
        /// 设置数字IO输出状态
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="doType">指定数字IO类型
        /// MC_ENABLE(该宏定义为10) 驱动器使能
        /// MC_CLEAR(该宏定义为11) 报警清除
        /// MC_GPO(该宏定义为12) 通用输出
        /// </param>
        /// <param name="value">按位指示数字IO输出电平
        /// 默认情况下，1表示高电平，0表示低电平
        /// </param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetDo(short cardNum, short doType, int value);

        //按位设置数字IO输出状态
        /// <summary>
        /// 按位设置数字IO输出状态
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="doType">指定数字IO类型
        /// MC_ENABLE(该宏定义为10) 驱动器使能
        /// MC_CLEAR(该宏定义为11) 报警清除
        /// MC_GPO(该宏定义为12) 通用输出
        /// </param>
        /// <param name="doIndex">输出IO的索引
        /// 取值范围：
        /// doType=MC_ENABLE时：[1,8]
        /// doType=MC_CLEAR时：[1,8]
        /// doType=MC_GPO时：[1,16]
        /// </param>
        /// <param name="value">设置数字IO输出电平
        /// 默认情况下，1表示高电平，0表示低电平
        /// </param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetDoBit(short cardNum, short doType, short doIndex,
            short value);
        
        //读取数字IO输出状态
        /// <summary>
        /// 读取数字IO输出状态
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="doType">指定数字IO类型
        /// MC_ENABLE(该宏定义为10) 驱动器使能
        /// MC_CLEAR(该宏定义为11) 报警清除
        /// MC_GPO(该宏定义为12) 通用输出</param>
        /// <param name="pValue">数字IO输出状态，按位指示IO输出电平
        /// 默认情况下，1表示高电平，0表示低电平</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetDo(short cardNum, short doType, out int pValue);
        
        //使数字量输出信号输出定时脉冲信号
        /// <summary>
        /// 使数字量输出信号输出定时脉冲信号
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="doType">指定数字IO类型
        /// MC_ENABLE(该宏定义为10) 驱动器使能
        /// MC_CLEAR(该宏定义为11) 报警清除
        /// MC_GPO(该宏定义为12) 通用输出</param>
        /// <param name="doIndex">输出IO的索引
        /// 取值范围：
        /// doType=MC_ENABLE时：[1,8]
        /// doType=MC_CLEAR时：[1,8]
        /// doType=MC_GPO时：[1,16]</param>
        /// <param name="value">设置数字IO输出电平
        /// 默认情况下，1表示高电平，0表示低电平</param>
        /// <param name="reverseTime">维持value所设置电平的时间，取值范围：[0,32767]，单位：250μs</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetDoBitReverse(short cardNum, short doType, 
            short doIndex, short value, short reverseTime);

        //读取数字IO输入状态
        /// <summary>
        /// 读取数字IO输入状态
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="diType">指定数字IO类型
        /// MC_LIMIT_POSITIVE(该宏定义为0) 正限位
        /// MC_LIMIT_NEGATIVE(该宏定义为1) 负限位
        /// MC_ALARM(该宏定义为2) 驱动报警
        /// MC_HOME(该宏定义为3) 原点开关
        /// MC_GPI(该宏定义为4) 通用输入
        /// MC_ARRIVE(该宏定义为5) 电机到位信号(仅适用于GTS-400-PX控制器)</param>
        /// <param name="pValue">数字IO输入状态，按位指示IO输入电平(根据配置工具di的reverse值不同而不同)
        /// 当reverse=0时，1表示高电平，0表示低电平
        /// 当reverse=1时，1表示低电平，0表示高电平</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetDi(short cardNum, short diType, out int pValue);

        //读取数字量输入信号的变化次数
        /// <summary>
        /// 读取数字量输入信号的变化次数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="diType">指定数字IO类型
        /// MC_LIMIT_POSITIVE(该宏定义为0) 正限位
        /// MC_LIMIT_NEGATIVE(该宏定义为1) 负限位
        /// MC_ALARM(该宏定义为2) 驱动报警
        /// MC_HOME(该宏定义为3) 原点开关
        /// MC_GPI(该宏定义为4) 通用输入
        /// MC_ARRIVE(该宏定义为5) 电机到位信号(仅适用于GTS-400-PX控制器)</param>
        /// <param name="diIndex">数字量输入的索引
        /// 取值范围：
        /// diType= MC_LIMIT_POSITIVE时：[1,8]
        /// diType= MC_LIMIT_NEGATIVE时：[1,8]
        /// diType= MC_ALARM时：[1,8]
        /// diType= MC_HOME时：[1,8]
        /// diType= MC_GPI时：[1,16]
        /// diType= MC_ARRIVE时：[1,8]</param>
        /// <param name="reverseCount">读取的数字量输入的变化次数</param>
        /// <param name="count">读取变化次数的数字量输入的个数，默认为1
        /// 1次最多可以读取4个数字量输入的变化次数。</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetDiReverseCount(short cardNum, short diType, 
            short diIndex, out uint reverseCount, short count);
        
        //设置数字量输入信号的变化次数的初值
        /// <summary>
        /// 设置数字量输入信号的变化次数的初值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="diType">指定数字IO类型
        /// MC_LIMIT_POSITIVE(该宏定义为0) 正限位
        /// MC_LIMIT_NEGATIVE(该宏定义为1) 负限位
        /// MC_ALARM(该宏定义为2) 驱动报警
        /// MC_HOME(该宏定义为3) 原点开关
        /// MC_GPI(该宏定义为4) 通用输入
        /// MC_ARRIVE(该宏定义为5) 电机到位信号(仅适用于GTS-400-PX控制器)</param>
        /// <param name="diIndex">数字量输入的索引
        /// 取值范围：
        /// diType= MC_LIMIT_POSITIVE时：[1,8]
        /// diType= MC_LIMIT_NEGATIVE时：[1,8]
        /// diType= MC_ALARM时：[1,8]
        /// diType= MC_HOME时：[1,8]
        /// diType= MC_GPI时：[1,16]
        /// diType= MC_ARRIVE时：[1,8]</param>
        /// <param name="reverseCount">设置的数字量输入的变化次数的初值</param>
        /// <param name="count">设置变化次数初值的数字量输入的个数，默认为1
        /// 1次最多可以设置4个数字量输入的变化次数的初值。</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetDiReverseCount(short cardNum, short diType, 
            short diIndex, ref uint reverseCount, short count);
        
        //读取数字IO输入状态的原始值
        /// <summary>
        /// 读取数字IO输入状态的原始值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="diType">指定数字IO类型
        /// MC_LIMIT_POSITIVE(该宏定义为0) 正限位
        /// MC_LIMIT_NEGATIVE(该宏定义为1) 负限位
        /// MC_ALARM(该宏定义为2) 驱动报警
        /// MC_HOME(该宏定义为3) 原点开关
        /// MC_GPI(该宏定义为4) 通用输入
        /// MC_ARRIVE(该宏定义为5) 电机到位信号(仅适用于GTS-400-PX控制器)</param>
        /// <param name="pValue">数字IO输入状态的原始值，按位指示IO输入电平
        /// 1表示高电平，0表示低电平</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetDiRaw(short cardNum, short diType, out int pValue);

        //设置dac输出电压
        /// <summary>
        /// 设置dac输出电压
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="dac">dac起始通道【轴】号</param>
        /// <param name="value">输出电压
        /// -32768对应-10V；32767对应+10V</param>
        /// <param name="count">设置的通道数，默认为1
        /// 1次最多可以设置8路dac输出</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetDac(short cardNum, short dac, ref short value, short count);
        
        //读取dac输出电压
        /// <summary>
        /// 读取dac输出电压
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="dac">dac起始通道【轴】号</param>
        /// <param name="value">输出电压</param>
        /// <param name="count">读取的通道数，默认为1
        /// 1次最多可以读取8个dac通道【轴】</param>
        /// <param name="pClock">读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetDac(short cardNum, short dac, out short value, 
            short count, out uint pClock);

        //读取模拟量输入的电压值
        /// <summary>
        /// 读取模拟量输入的电压值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="adc">adc起始通道号</param>
        /// <param name="pValue">读取的输入电压值，单位：伏特</param>
        /// <param name="count">读取的通道数，默认为1
        /// 1次最多可以读取8路adc输入电压值</param>
        /// <param name="pClock">读取控制器时钟，默认为：NULL，即不用读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetAdc(short cardNum, short adc, out double pValue, 
            short count, out uint pClock);
        
        //读取模拟量输入的数字转换值
        /// <summary>
        /// 读取模拟量输入的数字转换值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="adc">adc起始通道号</param>
        /// <param name="pValue">读取的输入电压值，单位：bit，
        /// 范围：[-32768,32767]对应的电压值为[-12.5,12.5]伏特</param>
        /// <param name="count">读取的通道数，默认为1
        /// 1次最多可以读取8路adc输入电压值</param>
        /// <param name="pClock">读取控制器时钟，默认为：NULL，即不用读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetAdcValue(short cardNum, short adc, 
            out short pValue, short count, out uint pClock);

        //修改编码器位置
        /// <summary>
        /// 修改编码器位置
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="encoder">编码器轴号</param>
        /// <param name="encPos">编码器位置</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetEncPos(short cardNum, short encoder, int encPos);
        
        //读取编码器位置
        /// <summary>
        /// 读取编码器位置
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="encoder">编码器起始轴号</param>
        /// <param name="pValue">编码器位置</param>
        /// <param name="count">读取的轴数，默认为1,
        /// 1次最多可以读取8个编码器轴</param>
        /// <param name="pClock">读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetEncPos(short cardNum, short encoder, 
            out double pValue, short count, out uint pClock);
        
        //读取编码器速度
        /// <summary>
        /// 读取编码器速度
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="encoder">编码器起始轴号</param>
        /// <param name="pValue">编码器速度</param>
        /// <param name="count">读取的轴数，默认为1
        /// 1次最多可以读取8个编码器轴</param>
        /// <param name="pClock">读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetEncVel(short cardNum, short encoder, 
            out double pValue, short count, out uint pClock);

        //设置编码器捕获方式，并启动捕获
        /// <summary>
        /// 设置编码器捕获方式，并启动捕获
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="encoder">编码器轴号</param>
        /// <param name="mode">编码器捕获模式
        /// CAPTURE_HOME Home捕获
        /// CAPTURE_INDEX Index捕获
        /// CAPTURE_PROBE 探针捕获</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetCaptureMode(short cardNum, short encoder, short mode);
        
        //读取编码器捕获方式
        /// <summary>
        /// 读取编码器捕获方式
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="encoder">编码器起始轴号</param>
        /// <param name="pMode">编码器捕获模式</param>
        /// <param name="count">读取的轴数，默认为1
        /// 1次最多可以读取8个编码器轴</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetCaptureMode(short cardNum, short encoder, 
            out short pMode, short count);
        
        //读取编码器捕获状态
        /// <summary>
        /// 读取编码器捕获状态
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="encoder">编码器起始轴号</param>
        /// <param name="pStatus">读取编码器捕获状态
        /// 为1时表示对应轴捕获触发</param>
        /// <param name="pValue">读取编码器捕获值
        /// 当捕获触发时，编码器捕获值会自动更新</param>
        /// <param name="count">读取的轴数，默认为1
        /// 1次最多可以读取8个编码器轴</param>
        /// <param name="pClock">读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetCaptureStatus(short cardNum, short encoder, 
            out short pStatus, out int pValue, short count, out uint pClock);
        
        //设置捕获电平
        /// <summary>
        /// 设置捕获电平
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="encoder">编码器轴号</param>
        /// <param name="mode">捕获模式
        /// CAPTURE_HOME Home捕获
        /// CAPTURE_INDEX Index捕获
        /// CAPTURE_PROBE 探针捕获
        /// </param>
        /// <param name="sense">捕获电平，可设置0或者1
        /// 0：下降沿触发
        /// 1：上升沿触发</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetCaptureSense(short cardNum, short encoder, 
            short mode, short sense);
        
        //清除捕获状态
        /// <summary>
        /// 清除捕获状态
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="encoder">需要被清除捕获状态的编码器轴号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_ClearCaptureStatus(short cardNum, short encoder);

        //复位运动控制器
        /// <summary>
        /// 复位运动控制器
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_Reset(short cardNum);
        
        //读取运动控制器系统时钟
        /// <summary>
        /// 读取运动控制器系统时钟
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="pClock">读取的运动控制器的时钟，单位：毫秒</param>
        /// <param name="pLoop">内部使用，默认为：NULL，即不读取该值</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetClock(short cardNum, out uint pClock, out uint pLoop);
        
        //读取运动控制器系统高精度时钟
        /// <summary>
        /// 读取运动控制器系统高精度时钟
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="pClock">读取的运动控制器的时钟，单位：125微秒</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetClockHighPrecision(short cardNum, out uint pClock);

        //读取轴状态
        /// <summary>
        /// 读取轴状态
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">起始轴号</param>
        /// <param name="pSts">32位轴状态字，详细定义:
        /// 位 0 -- 保留
        /// 位 1 -- 驱动器报警标志,控制轴连接的驱动器报警时置1
        /// 位 2 -- 保留
        /// 位 3 -- 保留
        /// 位 4 -- 跟随误差越限标志
        ///         控制轴规划位置和实际位置的误差大于设定极限时置1
        /// 位 5 -- 正限位触发标志
        ///         正限位开关电平状态为限位触发电平时置1
        ///         规划位置大于正向软限位时置1
        /// 位 6 -- 负限位触发标志
        ///         负限位开关电平状态为限位触发电平时置1
        ///         规划位置小于负向软限位时置1
        /// 位 7 -- IO平滑停止触发标志
        ///         如果轴设置了平滑停止IO，当其输入为触发电平时置1，并自动平滑停止该轴
        /// 位 8 -- IO急停触发标志
        ///         如果轴设置了急停IO，当其输入为触发电平时置1，并自动急停该轴
        /// 位 9 -- 电机使能标志
        ///         电机使能时置1
        /// 位 10 -- 规划运动标志
        ///          规划器运动时置1
        /// 位 11 -- 电机到位标志
        ///          规划器静止，规划位置和实际位置的误差小于设定误差带，
        ///          并且在误差带内保持设定时间后，置起到位标志
        /// 位 12~31 -- 保留
        /// </param>
        /// <param name="count">读取的轴数，默认为1
        /// 1次最多可以读取8个轴的状态</param>
        /// <param name="pClock">读取控制器时钟，默认为：NULL，即不用读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetSts(short cardNum, short axis, out int pSts, 
            short count, out uint pClock);
        
        //清除驱动器报警标志、跟随误差越限标志、限位触发标志
        /// <summary>
        /// 清除驱动器报警标志、跟随误差越限标志、限位触发标志
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">起始轴号</param>
        /// <param name="count">清除的轴数，默认为1
        /// 1次最多可以清除8个轴的异常状态</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_ClrSts(short cardNum, short axis, short count);
        
        //打开驱动器使能
        /// <summary>
        /// 打开驱动器使能
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">打开伺服使能的轴的编号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_AxisOn(short cardNum, short axis);
        
        //关闭驱动器使能
        /// <summary>
        /// 关闭驱动器使能
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">关闭伺服使能的轴的编号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_AxisOff(short cardNum, short axis);
        
        //停止一个或多个轴的规划运动，停止坐标系运动
        /// <summary>
        /// 停止一个或多个轴的规划运动，停止坐标系运动
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mask">按位指示需要停止运动的轴号或者坐标系号
        /// bit0表示1轴，bit1表示2轴，„，bit7表示8轴
        /// /// bit8表示坐标系1，bit9表示坐标系2
        /// 当bit位为1时表示停止对应的轴或者坐标系</param>
        /// <param name="option">按位指示停止方式
        /// bit0表示1轴，bit1表示2轴，„，bit7表示8轴
        /// bit8表示坐标系1，bit9表示坐标系2
        /// 当bit位为0时表示平滑停止对应的轴或坐标系
        /// 当bit位为1时表示急停对应的轴或坐标系</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_Stop(short cardNum, int mask, int option);
        
        //修改指定轴的规划位置
        /// <summary>
        /// 修改指定轴的规划位置
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴编号</param>
        /// <param name="prfPos">设置的规划位置的值
        /// axis含有profile和encoder的当量变换的功能，
        /// 如果调用了GT_SetPrfPos()指令或者GT_SetEncPos()指令之后，
        /// profile的输出值或者encoder的输出发生了变化，
        /// 如果需要将axis当量变换之后的值与profile或者encoder的值同步，
        /// 需要调用GT_SynchAxisPos()指令
        /// </param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetPrfPos(short cardNum, short profile, int prfPos);
        
        //axis合成规划位置和所关联的profile同步/axis合成编码器位置和所关联的encoder同步
        /// <summary>
        /// axis合成规划位置和所关联的profile同步
        /// axis合成编码器位置和所关联的encoder同步
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mask">按位标识需要进行位置同步的轴号
        /// Bit0对应1轴，bit1对应2轴，„
        /// 0：表示不需要进行位置同步
        /// 1：需要进行位置同步</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SynchAxisPos(short cardNum, int mask);
        
        //清零规划位置和实际位置，并进行零漂补偿
        /// <summary>
        /// 清零规划位置和实际位置，并进行零漂补偿
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">需要位置清零的起始轴号</param>
        /// <param name="count">需要位置清零的轴数</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_ZeroPos(short cardNum, short axis, short count);

        //设置轴正向软限位和负向软限位
        /// <summary>
        /// 设置轴正向软限位和负向软限位
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="positive">正向软限位，当规划位置大于该值时，正限位触发
        /// 默认值为0x7fffffff，表示正向软限位无效</param>
        /// <param name="negative">负向软限位，当规划位置小于该值时，负限位触发
        /// 默认值为0x80000000，表示负向软限位无效</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetSoftLimit(short cardNum, short axis, 
            int positive, int negative);
        
        //读取轴正向软限位和负向软限位
        /// <summary>
        /// 读取轴正向软限位和负向软限位
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="pPositive">读取正向软限位</param>
        /// <param name="pNegative">读取负向软限位</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetSoftLimit(short cardNum, short axis, 
            out int pPositive, out int pNegative);
        
        //设置轴到位误差带
        /// <summary>
        /// 设置轴到位误差带
        /// 规划器静止，规划位置和实际位置的误差小于设定误差带，并
        /// 且在误差带内保持设定时间后，置起到位标志
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="band">设置误差带大小，单位：脉冲</param>
        /// <param name="time">设置误差带保持时间，单位：250微秒</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetAxisBand(short cardNum, short axis, 
            int band, int time);
        
        //读取轴到位误差带
        /// <summary>
        /// 读取轴到位误差带
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="pBand">读取误差带大小，单位：脉冲</param>
        /// <param name="pTime">读取误差带保持时间，单位：250微秒</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetAxisBand(short cardNum, short axis, 
            out int pBand, out int pTime);
        
        //设置反向间隙补偿的相关参数
        /// <summary>
        /// 设置反向间隙补偿的相关参数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">需要进行反向间隙补偿的轴的编号，取值范围：[1,8]</param>
        /// <param name="compValue">反向间隙补偿值，当为0时表示没有使能反向间隙补偿功能,
        /// 取值范围：[0, 1073741824]，单位：脉冲</param>
        /// <param name="compChangeValue">反向间隙补偿的变化量，取值范围：[0, 1073741824]，单位：脉冲/毫秒
        /// 当该参数的值为0或者大于等于compValue时，则反向间隙的补偿量将瞬间叠加在规划位置上，没有渐变的过程</param>
        /// <param name="compDir">反向间隙补偿方向
        /// 0：只补偿负方向，当电机向负方向运动时，将施加补偿量，当电机向正方向运动时，不施加补偿量
        /// 1：只补偿正方向，当电机向正方向运动时，将施加补偿量，当电机向负方向运动时，不施加补偿量</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetBacklash(short cardNum, short axis, 
            int compValue, double compChangeValue, int compDir);
        
        //读取反向间隙补偿的相关参数
        /// <summary>
        /// 读取反向间隙补偿的相关参数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">查询的轴号，取值范围：[1,8]</param>
        /// <param name="pCompValue">读取的反向间隙补偿值，单位：脉冲</param>
        /// <param name="pCompChangeValue">读取的反向间隙补偿值的变化量，单位：脉冲/毫秒</param>
        /// <param name="pCompDir">读取的反向间隙补偿的补偿方向
        /// 0：只补偿负方向
        /// 1：只补偿正方向
        /// </param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetBacklash(short cardNum, short axis, 
            out int pCompValue, out double pCompChangeValue, out int pCompDir);

        //读取规划位置
        /// <summary>
        /// 读取规划位置
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">起始规划轴号</param>
        /// <param name="pValue">规划位置</param>
        /// <param name="count">读取的轴数，默认为1
        /// 1次最多可以读取8个轴的规划位置</param>
        /// <param name="pClock">读取控制器时钟，默认为：NULL，即不用读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetPrfPos(short cardNum, short profile, 
            out double pValue, short count, out uint pClock);
        
        //读取规划速度
        /// <summary>
        /// 读取规划速度
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">起始规划轴号</param>
        /// <param name="pValue">规划速度</param>
        /// <param name="count">读取的轴数，默认为1
        /// 1次最多可以读取8个轴的规划速度</param>
        /// <param name="pClock">读取控制器时钟，默认为：NULL，即不用读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetPrfVel(short cardNum, short profile, 
            out double pValue, short count, out uint pClock);
        
        //读取规划加速度
        /// <summary>
        /// 读取规划加速度
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">起始规划轴号</param>
        /// <param name="pValue">规划加速度</param>
        /// <param name="count">读取的轴数，默认为1
        /// 1次最多可以读取8个轴的规划加速度</param>
        /// <param name="pClock">读取控制器时钟，默认为：NULL，即不用读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetPrfAcc(short cardNum, short profile, 
            out double pValue, short count, out uint pClock);
        
        //读取轴运动模式
        /// <summary>
        /// 读取轴运动模式
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">起始规划轴号</param>
        /// <param name="pValue">轴运动模式
        /// 0：梯形曲线，控制器上电后默认为该模式
        /// 1：Jog模式
        /// 2：PT模式
        /// 3：电子齿轮模式
        /// 4：Follow模式
        /// </param>
        /// <param name="count">读取的规划轴数，默认为1
        /// 1次最多可以读取8个轴的运动模式</param>
        /// <param name="pClock">读取控制器时钟，默认为：NULL，即不用读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetPrfMode(short cardNum, short profile, 
            out int pValue, short count, out uint pClock);

        //读取轴(axis)的规划位置值
        /// <summary>
        /// 读取轴(axis)的规划位置值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">起始轴号</param>
        /// <param name="pValue">轴的规划位置</param>
        /// <param name="count">读取的轴数，默认为1
        /// 1次最多可以读取8个轴的规划位置</param>
        /// <param name="pClock">读取控制器时钟，默认为：NULL，即不用读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetAxisPrfPos(short cardNum, short axis, 
            out double pValue, short count, out uint pClock);
        
        //读取轴(axis)的规划速度值
        /// <summary>
        /// 读取轴(axis)的规划速度值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">起始轴号</param>
        /// <param name="pValue">轴的规划速度</param>
        /// <param name="count">读取的轴数，默认为1
        /// 1次最多可以读取8个轴的规划速度</param>
        /// <param name="pClock">读取控制器时钟，默认为：NULL，即不用读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetAxisPrfVel(short cardNum, short axis, 
            out double pValue, short count, out uint pClock);
        
        //读取轴(axis)的规划加速度值
        /// <summary>
        /// 读取轴(axis)的规划加速度值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">起始轴号</param>
        /// <param name="pValue">轴的规划加速度</param>
        /// <param name="count">读取的轴数，默认为1
        /// 1次最多可以读取8个轴的规划加速度</param>
        /// <param name="pClock">读取控制器时钟，默认为：NULL，即不用读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetAxisPrfAcc(short cardNum, short axis,
            out double pValue, short count, out uint pClock);
        
        //读取轴(axis)的编码器位置值
        /// <summary>
        /// 读取轴(axis)的编码器位置值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">起始轴号</param>
        /// <param name="pValue">轴的编码器位置</param>
        /// <param name="count">读取的轴数，默认为1
        /// 1次最多可以读取8个轴的编码器位置</param>
        /// <param name="pClock">读取控制器时钟，默认为：NULL，即不用读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetAxisEncPos(short cardNum, short axis, 
            out double pValue, short count, out uint pClock);
        
        //读取轴(axis)的编码器速度值
        /// <summary>
        /// 读取轴(axis)的编码器速度值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">起始轴号</param>
        /// <param name="pValue">轴的编码器速度</param>
        /// <param name="count">读取的轴数，默认为1
        /// 1次最多可以读取8个轴的编码器速度</param>
        /// <param name="pClock">读取控制器时钟，默认为：NULL，即不用读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetAxisEncVel(short cardNum, short axis, 
            out double pValue, short count, out uint pClock);
        
        //读取轴(axis)的编码器加速度值
        /// <summary>
        /// 读取轴(axis)的编码器加速度值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">起始轴号</param>
        /// <param name="pValue">轴的编码器加速度</param>
        /// <param name="count">读取的轴数，默认为1
        /// 1次最多可以读取8个轴的编码器加速度</param>
        /// <param name="pClock">读取控制器时钟，默认为：NULL，即不用读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetAxisEncAcc(short cardNum, short axis, 
            out double pValue, short count, out uint pClock);
        
        //读取轴(axis)的规划位置值和编码器位置值的差值
        /// <summary>
        /// 读取轴(axis)的规划位置值和编码器位置值的差值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">起始轴号</param>
        /// <param name="pValue">轴的规划位置与编码器位置的差值</param>
        /// <param name="count">读取的轴数，默认为1
        /// 1次最多可以读取8个轴的规划位置与编码器位置的差值</param>
        /// <param name="pClock">读取控制器时钟，默认为：NULL，即不用读取控制器时钟</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetAxisError(short cardNum, short axis, 
            out double pValue, short count, out uint pClock);

        //设定PID索引，支持3组PID参数
        /// <summary>
        /// 设定PID索引，支持3组PID参数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="control">伺服控制器编号</param>
        /// <param name="index">伺服控制参数的索引号，取值范围：[1,3]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetControlFilter(short cardNum,
            short control, short index);
        
        //读取当前PID索引
        /// <summary>
        /// 读取当前PID索引
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="control">伺服控制器编号</param>
        /// <param name="pIndex">读取的伺服控制参数的索引号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetControlFilter(short cardNum, 
            short control, out short pIndex);

        //设置PID参数
        /// <summary>
        /// 设置PID参数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="control">伺服控制器编号</param>
        /// <param name="index">伺服控制参数的索引号，取值范围：[1,3]</param>
        /// <param name="pPid">设置PID参数
        /// typedef struct Pid
        /// {
        /// double kp;
        /// double ki;
        /// double kd;
        /// double kvff;
        /// double kaff;
        /// long integralLimit;
        /// long derivativeLimit;
        /// short limit;
        /// }TPid;
        /// kp：比例增益；
        /// ki：积分增益；
        /// kd：微分增益；
        /// kvff：速度前馈系数；
        /// kaff：加速度前馈系数；
        /// integralLimit：积分饱和极限；
        /// derivativeLimit：微分饱和极限；
        /// limit：控制量输出饱和极限；
        /// </param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetPid(short cardNum, short control,
            short index, ref TPid pPid);
        
        //读取PID参数
        /// <summary>
        /// 读取PID参数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="control">伺服控制器编号</param>
        /// <param name="index">伺服控制参数的索引号，取值范围：[1,3]</param>
        /// <param name="pPid">读取PID参数</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetPid(short cardNum, short control,
            short index, out TPid pPid);

        //启动Jog模式运动
        /// <summary>
        /// 启动Jog模式运动
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mask">按位指示需要启动点位运动的轴号
        /// bit0表示1轴，bit1表示2轴，…
        /// 当bit位为1时表示启动对应的轴</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_Update(short cardNum, int mask);
        
        //设置目标位置
        /// <summary>
        /// 设置目标位置
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pos">设置目标位置，单位是脉冲</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetPos(short cardNum, short profile, int pos);
        
        //读取目标位置
        /// <summary>
        /// 读取目标位置
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pPos">读取目标位置，单位是脉冲</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetPos(short cardNum, short profile, out int pPos);
        
        //设置目标速度
        /// <summary>
        /// 设置目标速度
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="vel">设置目标速度，单位是“脉冲/毫秒”</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetVel(short cardNum, short profile, double vel);
        
        //读取目标速度
        /// <summary>
        /// 读取目标速度
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pVel">读取目标速度，单位是“脉冲/毫秒”</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetVel(short cardNum, short profile, out double pVel);

        //设置指定轴为点位运动模式
        /// <summary>
        /// 设置指定轴为点位运动模式
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PrfTrap(short cardNum, short profile);
        
        //设置点位模式运动参数
        /// <summary>
        /// 设置点位模式运动参数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pPrm">设置点位模式运动参数
        /// typedef struct TrapPrm
        /// {
        /// double acc; // 加速度，单位“脉冲/毫秒2”
        /// double dec; // 减速度，单位“脉冲/毫秒2”
        /// double velStart; // 起跳速度，单位“脉冲/毫秒”
        /// short smoothTime; // 平滑时间，单位“毫秒”
        /// } TTrapPrm;
        /// </param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetTrapPrm(short cardNum, short profile, ref TTrapPrm pPrm);
        
        //读取点位模式运动参数
        /// <summary>
        /// 读取点位模式运动参数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pPrm">读取点位模式运动参数</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetTrapPrm(short cardNum, short profile, out TTrapPrm pPrm);

        //设置指定轴为Jog模式
        /// <summary>
        /// 设置指定轴为Jog模式
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PrfJog(short cardNum, short profile);
        
        //设置Jog运动参数
        /// <summary>
        /// 设置Jog运动参数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pPrm">设置Jog模式运动参数
        /// typedef struct JogPrm
        /// {
        /// double acc; // 加速度，单位“脉冲/毫秒2”
        /// double dec; // 减速度，单位“脉冲/毫秒2”
        /// double smooth; // 平滑系数，取值范围[0,1)
        /// } TJogPrm;
        /// </param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetJogPrm(short cardNum, short profile, ref TJogPrm pPrm);
        
        //读取Jog运动参数
        /// <summary>
        /// 读取Jog运动参数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pPrm">读取Jog模式指令运动参数</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetJogPrm(short cardNum, short profile, out TJogPrm pPrm);

        //设置指定轴为PT模式
        /// <summary>
        /// 设置指定轴为PT模式
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="mode">指定FIFO使用模式
        /// PT_MODE_STATIC静态模式，默认
        /// PT_MODE_DYNAMIC动态模式</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PrfPt(short cardNum, short profile, short mode);
        
        //设置PT模式循环执行的次数,动态模式下该指令无效
        /// <summary>
        /// 设置PT模式循环执行的次数,动态模式下该指令无效
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="loop">指定PT模式循环执行的次数，如果需要无限循环，设置为0
        /// 动态模式下该参数无效</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetPtLoop(short cardNum, short profile, int loop);
        
        //查询PT模式循环执行的次数,动态模式下该指令无效
        /// <summary>
        /// 查询PT模式循环执行的次数,动态模式下该指令无效
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pLoop">查询PT模式循环已经执行完成的次数
        /// 动态模式下该参数无效</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetPtLoop(short cardNum, short profile, out int pLoop);
        
        //查询PT指定FIFO的剩余空间
        /// <summary>
        /// 查询PT指定FIFO的剩余空间
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pSpace">读取PT指定FIFO的剩余空间</param>
        /// <param name="fifo">指定所要查询的FIFO，默认为0
        /// 动态模式下该参数无效</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PtSpace(short cardNum, short profile, 
            out short pSpace, short fifo);
        
        //向PT指定FIFO增加数据
        /// <summary>
        /// 向PT指定FIFO增加数据
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pos">段末位置，单位脉冲</param>
        /// <param name="time">段末时间，单位毫秒</param>
        /// <param name="type">数据段类型
        /// PT_SEGMENT_NORMAL普通段，默认
        /// PT_SEGMENT_EVEN匀速段
        /// PT_SEGMENT_STOP减速到0段</param>
        /// <param name="fifo">指定存放运动数据的FIFO，默认为0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PtData(short cardNum, short profile, 
            double pos, int time, short type, short fifo);
        
        //清除PT指定FIFO中的数据,运动状态下该指令无效,动态模式下该指令无效
        /// <summary>
        /// 清除PT指定FIFO中的数据,运动状态下该指令无效,动态模式下该指令无效
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="fifo">指定所要清空的FIFO，默认为0
        /// 动态模式下该参数无效</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PtClear(short cardNum, short profile, short fifo);
        
        //启动PT模式运动
        /// <summary>
        /// 启动PT模式运动
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mask">按位指示需要启动PT运动的轴号
        /// bit0表示1轴，bit1表示2轴，…
        /// 当bit位为1时表示启动对应的轴</param>
        /// <param name="option">按位指示所使用的FIFO，默认为0
        /// bit0表示1轴，bit1表示2轴，…
        /// 当bit位为0时表示对应的轴使用FIFO1
        /// 当bit位为1时表示对应的轴使用FIFO2
        /// 动态模式下该参数无效</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PtStart(short cardNum, int mask, int option);
        
        //设置PT运动模式的缓存区大小
        /// <summary>
        /// 设置PT运动模式的缓存区大小
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="memory">PT运动缓存区大小标志：
        /// 0：每个PT运动缓存区有32段空间。
        /// 1：每个PT运动缓存区有1024段空间。</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetPtMemory(short cardNum, short profile, short memory);
        
        //读取PT运动模式的缓存区大小
        /// <summary>
        /// 读取PT运动模式的缓存区大小
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pMemory">读取PT运动缓存区大小标志</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetPtMemory(short cardNum, short profile, out short pMemory);

        //设置指定轴为电子齿轮模式
        /// <summary>
        /// 设置指定轴为电子齿轮模式
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="dir">设置跟随方式
        /// 0表示双向跟随，1表示正向跟随，-1表示负向跟随</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PrfGear(short cardNum, short profile, short dir);
        
        //设置跟随主轴
        /// <summary>
        /// 设置跟随主轴
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="masterIndex">主轴索引</param>
        /// <param name="masterType">主轴类型
        /// GEAR_MASTER_ENCODER表示跟随编码器(encoder)的输出值
        /// GEAR_MASTER_PROFILE表示跟随规划轴(profile)的输出值(默认)
        /// GEAR_MASTER_AXIS表示跟随轴(axis)的输出值</param>
        /// <param name="masterItem">轴类型，当masterType=GEAR_MASTER_AXIS时起作用
        /// 0表示axis的规划位置输出值(默认)
        /// 1表示axis的编码器位置输出值</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetGearMaster(short cardNum, short profile,
            short masterIndex, short masterType, short masterItem);
        
        //读取跟随主轴
        /// <summary>
        /// 读取跟随主轴
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pMasterIndex">主轴索引</param>
        /// <param name="pMasterType">主轴类型</param>
        /// <param name="pMasterItem">轴的输出位置值类型</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetGearMaster(short cardNum, short profile, 
            out short pMasterIndex, out short pMasterType, out short pMasterItem);
        
        //设置电子齿轮比
        /// <summary>
        /// 设置电子齿轮比
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="masterEven">传动比，主轴位移</param>
        /// <param name="slaveEven">传动比，从轴位移</param>
        /// <param name="masterSlope">主轴离合区位移</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetGearRatio(short cardNum, short profile,
            int masterEven, int slaveEven, int masterSlope);
        
        //读取电子齿轮比
        /// <summary>
        /// 读取电子齿轮比
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pMasterEven">主轴位移</param>
        /// <param name="pSlaveEven">从轴位移</param>
        /// <param name="pMasterSlope">主轴离合区位移</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetGearRatio(short cardNum, short profile,
            out int pMasterEven, out int pSlaveEven, out int pMasterSlope);
        
        //启动电子齿轮
        /// <summary>
        /// 启动电子齿轮
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mask">按位指示需要启动Gear运动的轴号
        /// bit0表示1轴，bit1表示2轴，…
        /// 当bit位为1时表示启动对应的轴</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GearStart(short cardNum, int mask);

        //设置指定轴为Follow模式
        /// <summary>
        /// 设置指定轴为Follow模式
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="dir">设置跟随方式
        /// 0表示双向跟随，1表示正向跟随，-1表示负向跟随</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PrfFollow(short cardNum, short profile, short dir);
        
        //设置Follow模式跟随主轴
        /// <summary>
        /// 设置Follow模式跟随主轴
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="masterIndex">主轴索引</param>
        /// <param name="masterType">主轴类型
        /// FOLLOW_MASTER_ENCODER表示跟随编码器(encoder)的输出值
        /// FOLLOW_MASTER_PROFILE表示跟随规划轴(profile)的输出值(默认)
        /// FOLLOW_MASTER_AXIS表示跟随轴(axis)的输出值</param>
        /// <param name="masterItem">轴类型，当masterType=FOLLOW_MASTER_AXIS时起作用
        /// 0表示axis的规划位置输出值(默认)
        /// 1表示axis的编码器位置输出值</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetFollowMaster(short cardNum, short profile,
            short masterIndex, short masterType, short masterItem);
        
        //读取Follow模式跟随主轴
        /// <summary>
        /// 读取Follow模式跟随主轴
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pMasterIndex">主轴索引</param>
        /// <param name="pMasterType">主轴类型</param>
        /// <param name="pMasterItem">合成轴类型</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetFollowMaster(short cardNum, short profile,
            out short pMasterIndex, out short pMasterType, out short pMasterItem);
        
        //设置Follow模式循环次数
        /// <summary>
        /// 设置Follow模式循环次数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="loop">指定Follow模式循环执行的次数</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetFollowLoop(short cardNum, short profile, int loop);
        
        //读取Follow模式循环次数
        /// <summary>
        /// 读取Follow模式循环次数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pLoop">读取Follow模式循环已经执行完成的次数</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetFollowLoop(short cardNum, short profile, out int pLoop);
        
        //设置Follow模式启动跟随条件
        /// <summary>
        /// 设置Follow模式启动跟随条件
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="followEvent">启动跟随条件
        /// FOLLOW_EVENT_START表示调用GT_FollowStart以后立即启动
        /// FOLLOW_EVENT_PASS表示主轴穿越设定位置以后启动跟随</param>
        /// <param name="masterDir">穿越启动时，主轴的运动方向
        /// 1主轴正向运动，-1主轴负向运动</param>
        /// <param name="pos">穿越位置，当event为FOLLOW_EVENT_PASS时有效</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetFollowEvent(short cardNum, short profile,
            short followEvent, short masterDir, int pos);
        
        //读取Follow模式启动跟随条件
        /// <summary>
        /// 读取Follow模式启动跟随条件
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pFollowEvent">启动跟随条件</param>
        /// <param name="pMasterDir">主轴运动方向</param>
        /// <param name="pPos">穿越位置，当event为FOLLOW_EVENT_PASS时有效</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetFollowEvent(short cardNum, short profile,
            out short pFollowEvent, out short pMasterDir, out int pPos);
        
        //查询Follow指定FIFO的剩余空间
        /// <summary>
        /// 查询Follow指定FIFO的剩余空间
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="pSpace">读取FIFO的剩余空间</param>
        /// <param name="fifo">指定所要查询的FIFO，默认为0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_FollowSpace(short cardNum, short profile,
            out short pSpace, short fifo);
        
        //向Follow指定FIFO增加数据
        /// <summary>
        /// 向Follow指定FIFO增加数据
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="masterSegment">主轴位移</param>
        /// <param name="slaveSegment">从轴位移</param>
        /// <param name="type">数据段类型
        /// FOLLOW_SEGMENT_NORMAL普通段，默认
        /// FOLLOW_SEGMENT_EVEN匀速段
        /// FOLLOW_SEGMENT_STOP减速到0段
        /// FOLLOW_SEGMENT_CONTINUE保持FIFO之间速度连续</param>
        /// <param name="fifo">指定存放数据的FIFO，默认为0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_FollowData(short cardNum, short profile, 
            int masterSegment, double slaveSegment, short type, short fifo);
        
        //清除Follow指定FIFO中的数据,运动状态下该指令无效
        /// <summary>
        /// 清除Follow指定FIFO中的数据,运动状态下该指令无效
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="fifo">指定需要清除的FIFO，默认为0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_FollowClear(short cardNum, short profile, short fifo);
        
        //启动Follow模式运动
        /// <summary>
        /// 启动Follow模式运动
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mask">按位指示需要启动Follow运动的轴号
        /// bit0表示1轴，bit1表示2轴，…
        /// 当bit位为1时表示启动对应的轴</param>
        /// <param name="option">按位指示所使用的FIFO，默认为0
        /// bit0表示1轴，bit1表示2轴，…
        /// 当bit位为0时表示对应的轴使用FIFO1
        /// 当bit位为1时表示对应的轴使用FIFO2</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_FollowStart(short cardNum, int mask, int option);
        
        //切换Follow所使用的FIFO
        /// <summary>
        /// 切换Follow所使用的FIFO
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mask">按位指示需要切换Follow工作FIFO的轴号
        /// bit0表示1轴，bit1表示2轴，…
        /// 当bit位为1时表示切换对应的轴的FIFO</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_FollowSwitch(short cardNum, int mask);
        
        //设置Follow运动模式的缓存区大小
        /// <summary>
        /// 设置Follow运动模式的缓存区大小
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="memory">Follow运动缓存区大小标志：
        /// 0：每个Follow运动缓存区有16段空间。
        /// 1：每个Follow运动缓存区有512段空间。</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetFollowMemory(short cardNum, short profile, short memory);
        
        //读取Follow运动模式的缓存区大小
        /// <summary>
        /// 读取Follow运动模式的缓存区大小
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划轴号</param>
        /// <param name="memory">读取Follow运动缓存区大小标志</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetFollowMemory(short cardNum, short profile, out short memory);

        //下载运动程序到运动控制器
        /// <summary>
        /// 下载运动程序到运动控制器
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="pFileName">下载到运动控制器的运动程序文件名</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_Download(short cardNum, string pFileName);

        //读取运动程序中函数的标识
        /// <summary>
        /// 读取运动程序中函数的标识
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="pFunName">运动程序函数名称</param>
        /// <param name="pFunId">根据运动程序函数名称查询函数标识</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetFunId(short cardNum, string pFunName, out short pFunId);
        
        //绑定线程、函数、数据页
        /// <summary>
        /// 绑定线程、函数、数据页
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="thread">线程编号，取值范围[0,31]。</param>
        /// <param name="funId">函数标识，可以调用GT_GetFunId查询</param>
        /// <param name="page">数据页编号，取值范围[0,31]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_Bind(short cardNum, short thread, short funId, short page);

        //启动线程
        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="thread">线程编号，取值范围[0,31]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_RunThread(short cardNum, short thread);
        
        //停止正在运行的线程
        /// <summary>
        /// 停止正在运行的线程
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="thread">线程编号，取值范围[0,31]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_StopThread(short cardNum, short thread);
        
        //暂停正在运行的线程
        /// <summary>
        /// 暂停正在运行的线程
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="thread">线程编号，取值范围[0,31]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PauseThread(short cardNum, short thread);
        
        //读取线程的状态
        /// <summary>
        /// 读取线程的状态
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="thread">线程编号，取值范围[0,31]</param>
        /// <param name="pThreadSts">读取线程状态
        /// typedef struct ThreadSts
        /// {
        /// short run; // 运行状态
        /// short error; // 指令返回值
        /// double result; // 函数返回值
        /// short line; // 当前执行的指令行号
        /// } TThreadSts;</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetThreadSts(short cardNum, short thread,
            out TThreadSts pThreadSts);

        //读取运动程序中变量的标识
        /// <summary>
        /// 读取运动程序中变量的标识
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="pFunName">全局变量输入NULL
        /// 局部变量所在函数的名称</param>
        /// <param name="pVarName">运动程序变量名称</param>
        /// <param name="pVarInfo">根据运动程序函数名称和变量名称查询变量标识</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetVarId(short cardNum, string pFunName,
            string pVarName, out TVarInfo pVarInfo);
        
        //设置运动程序中变量的值
        /// <summary>
        /// 设置运动程序中变量的值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="page">数据页编号
        /// 全局变量为-1
        /// 局部变量取值范围[0,31]</param>
        /// <param name="pVarInfo">需要访问的变量标识</param>
        /// <param name="pValue">需要写入的变量值</param>
        /// <param name="count">需要写入的变量值的数量，取值范围[1,8]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetVarValue(short cardNum, short page,
            ref TVarInfo pVarInfo, ref double pValue, short count);
        
        //读取运动程序中变量的值
        /// <summary>
        /// 读取运动程序中变量的值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="page">数据页编号
        /// 全局变量为-1
        /// 局部变量取值范围[0,31]</param>
        /// <param name="pVarInfo">需要访问的变量标识</param>
        /// <param name="pValue">需要读取的变量值</param>
        /// <param name="count">需要读取的变量值的数量，取值范围[1,8]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetVarValue(short cardNum, short page,
            ref TVarInfo pVarInfo, out double pValue, short count);

        //设置坐标系参数，确立坐标系映射，建立坐标系
        /// <summary>
        /// 设置坐标系参数，确立坐标系映射，建立坐标系
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="pCrdPrm">设置坐标系的相关参数
        /// typedef struct CrdPrm
        /// {
        /// short dimension;
        /// short profile[8];
        /// double synVelMax;
        /// double synAccMax;
        /// short evenTime;
        /// short setOriginFlag;
        /// long originPos[8];
        /// }TCrdPrm;
        /// dimension：坐标系的维数，取值范围：[1,4]。
        /// Profile[8]：坐标系与规划器的映射关系，每个元素的取值范围：[0,4]。
        /// synVelMax：该坐标系的最大合成速度。取值范围：(0,32767)，单位：pulse/ms。
        /// synAccMax：该坐标系的最大合成加速度。取值范围：(0,32767)，单位：pulse/(ms*ms)。
        /// evenTime：每个插补段的最小匀速段时间。取值范围：[0,32767)，单位：ms。
        /// setOriginFlag：表示是否需要指定坐标系的原点坐标的规划位置，
        ///                0：不需要指定原点坐标值，则坐标系的原点在当前规划位置上；
        ///                1：需要指定原点坐标值，坐标系的原点在originPos指定的规划位置上。
        /// originPos[8]：指定的坐标系原点的规划位置值</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetCrdPrm(short cardNum, short crd, ref TCrdPrm pCrdPrm);
        
        //查询坐标系参数
        /// <summary>
        /// 查询坐标系参数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="pCrdPrm">读取坐标系的相关参数
        /// 结构体的成员含义参照GT_SetCrdPrm</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetCrdPrm(short cardNum, short crd, out TCrdPrm pCrdPrm);
        
        //查询插补缓存区剩余空间
        /// <summary>
        /// 查询插补缓存区剩余空间
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="pSpace">读取插补缓存区中的剩余空间</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_CrdSpace(short cardNum, short crd, out int pSpace, short fifo);
                
        //向插补缓存区增加插补数据
        /// <summary>
        /// 向插补缓存区增加插补数据
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="pCrdData">插补数据</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_CrdData(short cardNum, short crd, short pCrdData, short fifo);
        
        //启动插补运动
        /// <summary>
        /// 启动插补运动
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mask">从bit0~bit1按位表示需要启动的坐标系，
        /// 其中，bit0对应坐标系1，bit1对应坐标系2；
        /// 0：不启动该坐标系，1：启动该坐标系。</param>
        /// <param name="option">从bit0~bit1按位表示坐标系需要启动的缓存区的编号，
        /// 其中，bit0对应坐标系1，bit1对应坐标系2；
        /// 0：启动坐标系中FIFO0的运动，1：启动坐标系中FIFO1的运动。</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_CrdStart(short cardNum, short mask, short option);
        
        //设置插补运动目标合成速度倍率
        /// <summary>
        /// 设置插补运动目标合成速度倍率
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="synVelRatio">设置的插补目标速度倍率，取值范围：(0,1]，系统默认该值为：1</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetOverride(short cardNum, short crd, double synVelRatio);
        
        //初始化插补前瞻缓存区
        /// <summary>
        /// 初始化插补前瞻缓存区
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="fifo">插补缓存区编号，取值范围：[0,1]</param>
        /// <param name="T">拐弯时间，单位：ms</param>
        /// <param name="accMax">最大加速度，单位：pulse/(ms*ms)</param>
        /// <param name="n">前瞻缓存区大小，取值范围：[0,32767)</param>
        /// <param name="pLookAheadBuf">前瞻缓存区内存区指针</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_InitLookAhead(short cardNum, short crd, 
            short fifo, double T, double accMax, short n, ref TCrdData pLookAheadBuf);
        
        //清除插补缓存区内的插补数据
        /// <summary>
        /// 清除插补缓存区内的插补数据
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="fifo">所要清除的插补缓存区号，取值范围：[0,1]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_CrdClear(short cardNum, short crd, short fifo);
        
        //查询插补运动坐标系状态
        /// <summary>
        /// 查询插补运动坐标系状态
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="pRun">读取插补运动状态，0：该坐标系的该FIFO没有在运动；
        /// 1：该坐标系的该FIFO正在进行插补运动</param>
        /// <param name="pSegment">读取当前已经完成的插补段数，
        /// 当重新建立坐标系或者调用GT_CrdClear指令后，该值会被清零</param>
        /// <param name="fifo">所要查询运动状态的fifo号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_CrdStatus(short cardNum, short crd, 
            out short pRun, out int pSegment, short fifo);
        
        //缓存区指令，设置自定义插补段段号
        /// <summary>
        /// 缓存区指令，设置自定义插补段段号
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="segNum">设置用户自定义的插补段段号</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetUserSegNum(short cardNum, short crd,
            int segNum, short fifo);
        
        //读取自定义插补段段号
        /// <summary>
        /// 读取自定义插补段段号
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="pSegment">读取的用户自定义的插补段段号</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetUserSegNum(short cardNum, short crd,
            out int pSegment, short fifo);
        
        //读取未完成的插补段段数
        /// <summary>
        /// 读取未完成的插补段段数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="pSegment">读取的剩余插补段的段数</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetRemainderSegNum(short cardNum, short crd,
            out int pSegment, short fifo);
        
        //设置插补运动平滑停止、急停合成加速度
        /// <summary>
        /// 设置插补运动平滑停止、急停合成加速度
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="decSmoothStop">设置的坐标系合成平滑停止加速度，
        /// 取值范围：(0,32767)，单位：pulse/(ms*ms)</param>
        /// <param name="decAbruptStop">设置的坐标系合成急停加速度，
        /// 取值范围：(0,32767)，单位：pulse/(ms*ms)</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetCrdStopDec(short cardNum, short crd,
            double decSmoothStop, double decAbruptStop);
        
        //查询插补运动平滑停止、急停合成加速度
        /// <summary>
        /// 查询插补运动平滑停止、急停合成加速度
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="pDecSmoothStop">查询坐标系合成平滑停止加速度，单位：pulse/(ms*ms)</param>
        /// <param name="pDecAbruptStop">查询坐标系合成急停加速度，单位：pulse/(ms*ms)</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetCrdStopDec(short cardNum, short crd,
            out double pDecSmoothStop, out double pDecAbruptStop);
        
        //查询该坐标系的当前坐标位置值
        /// <summary>
        /// 查询该坐标系的当前坐标位置值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="pPos">读取的坐标系的坐标值，单位：pulse。
        /// 该参数应该为一个数组首元素的指针，数组的元素个数取决于该坐标系的维数</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetCrdPos(short cardNum, short crd, out double pPos);
        
        //查询该坐标系的合成速度值
        /// <summary>
        /// 查询该坐标系的合成速度值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="pSynVel">读取的坐标系的合成速度值，单位：pulse/ms</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetCrdVel(short cardNum, short crd, out double pSynVel);

        //设置指定轴为PVT模式【P-位置，V-速度，T-时间】
        /// <summary>
        /// 设置指定轴为PVT模式【P-位置，V-速度，T-时间】
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">轴号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PrfPvt(short cardNum, short profile);
        
        //设置循环次数
        /// <summary>
        /// 设置循环次数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">轴号</param>
        /// <param name="loop">指定循环执行的次数
        /// 0表示无限循环</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetPvtLoop(short cardNum, short profile, int loop);
        
        //查询循环次数
        /// <summary>
        /// 查询循环次数
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">轴号</param>
        /// <param name="pLoopCount">查询已经循环的次数</param>
        /// <param name="pLoop">查询循环执行的总次数</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetPvtLoop(short cardNum, short profile, 
            out int pLoopCount, out int pLoop);
        
        //读取状态
        /// <summary>
        /// 读取状态
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">轴号</param>
        /// <param name="pTableId">当前正在使用的数据表</param>
        /// <param name="pTime">当前轴已经运动的时间，单位是“毫秒”</param>
        /// <param name="count">读取的轴数</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PvtStatus(short cardNum, short profile, 
            out short pTableId, out double pTime, short count);
        
        //启动运动
        /// <summary>
        /// 启动运动
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mask">按位指示需要启动的轴号
        /// bit0 表示1 轴，bit1 表示2 轴，„„
        /// 当bit 位为1 时表示启动对应的轴
        /// 在启动运动之前，可以调用GT_PvtTableSelect 选择数据表
        /// 如果没有选择数据表，默认使用数据表1
        /// 如果数据表为空，则启动失败</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PvtStart(short cardNum, int mask);
        
        //选择数据表
        /// <summary>
        /// 选择数据表
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">轴号</param>
        /// <param name="tableId">指定数据表
        /// PVT 模式提供32 个数据表，取值范围[1,32]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PvtTableSelect(short cardNum, short profile, short tableId);

        //向指定数据表传送数据，采用PVT描述方式
        /// <summary>
        /// 向指定数据表传送数据，采用PVT描述方式
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="tableId">指定数据表</param>
        /// <param name="count">数据点个数，每个数据表具有1024个存储空间
        /// 每个数据点占用1个存储空间</param>
        /// <param name="pTime">数据点时间数组，单位是“毫秒”，数组长度为count</param>
        /// <param name="pPos">数据点位置数组，单位是“脉冲”，数组长度为count</param>
        /// <param name="pVel">数据点速度数组，单位是“脉冲/毫秒”，数组长度为count</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PvtTable(short cardNum, short tableId, 
            int count, ref double pTime, ref double pPos, ref double pVel);
        
        //GT_PvtTable函数的扩展，向指定数据表传送数据，采用PVT描述方式
        /// <summary>
        /// GT_PvtTable函数的扩展，向指定数据表传送数据，采用PVT描述方式
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="tableId">指定数据表</param>
        /// <param name="count">数据点个数，每个数据表具有1024个存储空间
        /// 每个数据点占用1个存储空间</param>
        /// <param name="pTime">数据点时间数组，单位是“毫秒”，数组长度为count</param>
        /// <param name="pPos">数据点位置数组，单位是“脉冲”，数组长度为count</param>
        /// <param name="pVelBegin">起始速度</param>
        /// <param name="pVelEnd">结束速度</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PvtTableEx(short cardNum, short tableId, 
            int count, ref double pTime, ref double pPos, ref double pVelBegin, ref double pVelEnd);
                
        //向指定数据表传送数据，采用Complete描述方式
        /// <summary>
        /// 向指定数据表传送数据，采用Complete描述方式
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="tableId">指定数据表</param>
        /// <param name="count">数据点个数，每个数据表具有1024个存储空间
        /// 每个数据点占用1个存储空间</param>
        /// <param name="pTime">数据点时间数组，单位是“毫秒”，数组长度为count</param>
        /// <param name="pPos">数据点位置数组，单位是“脉冲”，数组长度为count</param>
        /// <param name="pA">工作数组，内部使用，数组长度为count
        /// 该数组用户不必赋值</param>
        /// <param name="pB">工作数组，内部使用，数组长度为count
        /// 该数组用户不必赋值</param>
        /// <param name="pC">工作数组，内部使用，数组长度为count
        /// 该数组用户不必赋值</param>
        /// <param name="velBegin">起点速度，单位是“脉冲/毫秒”</param>
        /// <param name="velEnd">终点速度，单位是“脉冲/毫秒”</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PvtTableComplete(short cardNum, short tableId, 
            int count, ref double pTime, ref double pPos, ref double pA, ref double pB,
            ref double pC, double velBegin, double velEnd);
        
        //向指定数据表传送数据，采用Percent描述方式
        /// <summary>
        /// 向指定数据表传送数据，采用Percent描述方式
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="tableId">指定数据表</param>
        /// <param name="count">数据点个数，每个数据表具有1024个存储空间
        /// 每个数据点占用1~3个存储空间</param>
        /// <param name="pTime">数据点时间数组，单位是“毫秒”，数组长度为count</param>
        /// <param name="pPos">数据点位置数组，单位是“脉冲”，数组长度为count</param>
        /// <param name="pPercent">数据点百分比数组，数组长度为count
        /// 百分比的取值范围[0,100]</param>
        /// <param name="velBegin">起点速度，单位是“脉冲/毫秒”</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PvtTablePercent(short cardNum, short tableId, 
            int count, ref double pTime, ref double pPos, ref double pPercent, double velBegin);
        
        //计算Percent描述方式下各数据点的速度
        /// <summary>
        /// 计算Percent描述方式下各数据点的速度
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="count">数据点个数，该指令用来计算各数据点的速度，
        /// 不会将数据点下载到运动控制器</param>
        /// <param name="pTime">数据点时间数组，单位是“毫秒”，数组长度为count</param>
        /// <param name="pPos">数据点位置数组，单位是“脉冲”，数组长度为count</param>
        /// <param name="pPercent">数据点百分比数组，数组长度为count
        /// 百分比的取值范围[0,100]</param>
        /// <param name="velBegin">起点速度，单位是“脉冲/毫秒”</param>
        /// <param name="pVel">返回各数据点的速度，单位是“脉冲/毫秒”</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PvtPercentCalculate(short cardNum, int count, 
            ref double pTime, ref double pPos, ref double pPercent, double velBegin, ref double pVel);
        
        //向指定数据表传送数据，采用Continuous描述方式
        /// <summary>
        /// 向指定数据表传送数据，采用Continuous描述方式
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="tableId">指定数据表</param>
        /// <param name="count">数据点个数，每个数据表具有1024个存储空间
        /// 每个数据点占用1~8个存储空间</param>
        /// <param name="pPos">数据点位置数组，单位是“脉冲”，数组长度为count</param>
        /// <param name="pVel">数据点速度数组，单位是“脉冲/毫秒”，数组长度为count</param>
        /// <param name="pPercent">数据点百分比数组，数组长度为count
        /// 百分比的取值范围[0,100]</param>
        /// <param name="pVelMax">数据点最大速度数组，单位是“脉冲/毫秒”，数组长度为count</param>
        /// <param name="pAcc">数据点加速度数组，单位是“脉冲/毫秒2”，数组长度为count</param>
        /// <param name="pDec">数据点减速度数组，单位是“脉冲/毫秒2”，数组长度为count</param>
        /// <param name="timeBegin">起点时间，单位是“毫秒”</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PvtTableContinuous(short cardNum, short tableId,
            int count, ref double pPos, ref double pVel, ref double pPercent, 
            ref double pVelMax, ref double pAcc, ref double pDec, double timeBegin);
        
        //计算Continuous描述方式下各数据点的时间
        /// <summary>
        /// 计算Continuous描述方式下各数据点的时间
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="count">数据点个数，该指令用来计算各数据点时间，不会将数据点下载到运
        /// 动控制器</param>
        /// <param name="pPos">数据点位置数组，单位是“脉冲”，数组长度为count</param>
        /// <param name="pVel">数据点速度数组，单位是“脉冲/毫秒”，数组长度为count</param>
        /// <param name="pPercent">数据点百分比数组，数组长度为count
        /// 百分比的取值范围[0,100]</param>
        /// <param name="pVelMax">数据点最大速度数组，单位是“脉冲/毫秒”，数组长度为count</param>
        /// <param name="pAcc">数据点加速度数组，单位是“脉冲/毫秒2”，数组长度为count</param>
        /// <param name="pDec">数据点减速度数组，单位是“脉冲/毫秒2”，数组长度为count</param>
        /// <param name="pTime">返回各数据点的时间，单位是“毫秒”</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_PvtContinuousCalculate(short cardNum, int count,
            ref double pPos, ref double pVel, ref double pPercent, ref double pVelMax,
            ref double pAcc, ref double pDec, ref double pTime);

        //控制轴驱动报警信号无效
        /// <summary>
        /// 控制轴驱动报警信号无效
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">控制轴号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_AlarmOff(short cardNum, short axis);
        
        //控制轴驱动报警信号有效
        /// <summary>
        /// 控制轴驱动报警信号有效
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">控制轴号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_AlarmOn(short cardNum, short axis);
        
        //控制轴限位信号有效
        /// <summary>
        /// 控制轴限位信号有效
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">控制轴号</param>
        /// <param name="limitType">需要有效的限位类型
        /// MC_LIMIT_POSITIVE(该宏定义为0)：需要将该轴的正限位有效
        /// MC_LIMIT_NEGATIVE(该宏定义为1)：需要将该轴的负限位有效
        /// -1：需要将该轴的正限位和负限位都有效，默认为该值</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_LmtsOn(short cardNum, short axis, short limitType);
        
        //控制轴限位信号无效
        /// <summary>
        /// 控制轴限位信号无效
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">控制轴号</param>
        /// <param name="limitType">需要有效的限位类型
        /// MC_LIMIT_POSITIVE(该宏定义为0)：需要将该轴的正限位有效
        /// MC_LIMIT_NEGATIVE(该宏定义为1)：需要将该轴的负限位有效
        /// -1：需要将该轴的正限位和负限位都有效，默认为该值</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_LmtsOff(short cardNum, short axis, short limitType);
        
        //设置控制轴的规划器当量变换值
        /// <summary>
        /// 设置控制轴的规划器当量变换值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">控制轴号</param>
        /// <param name="alpha">规划器当量的alpha值，取值范围：[-32768,32767]</param>
        /// <param name="beta">规划器当量的beta值，取值范围：[-32768,32767]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_ProfileScale(short cardNum, short axis, 
            short alpha, short beta);
        
        //设置控制轴的编码器当量变换值
        /// <summary>
        /// 设置控制轴的编码器当量变换值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">控制轴号</param>
        /// <param name="alpha">编码器当量的alpha值，取值范围：[-32768,32767]</param>
        /// <param name="beta">编码器当量的beta值，取值范围：[-32768,32767]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_EncScale(short cardNum, short axis, short alpha, short beta);
        
        //将脉冲输出通道的脉冲输出模式设置为“脉冲+方向”
        /// <summary>
        /// 将脉冲输出通道的脉冲输出模式设置为“脉冲+方向”
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="step">脉冲输出通道号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_StepDir(short cardNum, short step);
        
        //将脉冲输出通道的脉冲输出模式设置为“CW/CCW”
        /// <summary>
        /// 将脉冲输出通道的脉冲输出模式设置为“CW/CCW”
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="step">脉冲输出通道号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_StepPulse(short cardNum, short step);
        
        //设置模拟量输出通道的零漂电压补偿值
        /// <summary>
        /// 设置模拟量输出通道的零漂电压补偿值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="dac">模拟量输出通道号</param>
        /// <param name="bias">零漂补偿值，取值范围：[-32768,32767]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetMtrBias(short cardNum, short dac, short bias);
        
        //读取模拟量输出通道的零漂电压补偿值
        /// <summary>
        /// 读取模拟量输出通道的零漂电压补偿值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="dac">模拟量输出通道号</param>
        /// <param name="pBias">读取的零漂补偿值</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetMtrBias(short cardNum, short dac, out short pBias);
        
        //设置模拟量输出通道的输出电压饱和极限值
        /// <summary>
        /// 设置模拟量输出通道的输出电压饱和极限值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="dac">模拟量输出通道号</param>
        /// <param name="limit">输出电压饱和极限值，取值范围：(0,32767]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetMtrLmt(short cardNum, short dac, short limit);
        
        //读取模拟量输出通道的输出电压饱和极限值
        /// <summary>
        /// 读取模拟量输出通道的输出电压饱和极限值
        /// </summary>
        /// <param name="dac">模拟量输出通道号</param>
        /// <param name="pLimit">读取的输出电压饱和极限值</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetMtrLmt(short dac, out short pLimit);
        
        //设置编码器的计数方向
        /// <summary>
        /// 设置编码器的计数方向
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="sense">按位标识编码器的计数方向，
        /// bit0~bit7依次对应编码器1~8，
        /// bit8对应辅助编码器
        /// 0：该编码器计数方向不取反
        /// 1：该编码器计数方向取反</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_EncSns(short cardNum, ushort sense);
        
        //设置为“外部编码器”计数方式
        /// <summary>
        /// 设置为“外部编码器”计数方式
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="encoder">编码器通道号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_EncOn(short cardNum, short encoder);
        
        //设置为“脉冲计数器”计数方式
        /// <summary>
        /// 设置为“脉冲计数器”计数方式
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="encoder">编码器通道号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_EncOff(short cardNum, short encoder);
        
        //设置跟随误差极限值
        /// <summary>
        /// 设置跟随误差极限值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="control">伺服控制器编号</param>
        /// <param name="error">跟随误差极限值，取值范围：(0, 2147483648]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetPosErr(short cardNum, short control, int error);
        
        //读取跟随误差极限值
        /// <summary>
        /// 读取跟随误差极限值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="control">伺服控制器编号</param>
        /// <param name="pError">读取的跟随误差极限值</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetPosErr(short cardNum, short control, out int pError);
        
        //设置平滑停止减速度和急停减速度
        /// <summary>
        /// 设置平滑停止减速度和急停减速度
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划器的编号【轴号】</param>
        /// <param name="decSmoothStop">平滑停止减速度，取值范围：(0,32767]</param>
        /// <param name="decAbruptStop">急停减速度，取值范围：(0,32767]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetStopDec(short cardNum, short profile, 
            double decSmoothStop, double decAbruptStop);
        
        //读取平滑停止减速度和急停减速度
        /// <summary>
        /// 读取平滑停止减速度和急停减速度
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="profile">规划器的编号【轴号】</param>
        /// <param name="pDecSmoothStop">读取的平滑停止减速度</param>
        /// <param name="pDecAbruptStop">读取的急停减速度</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetStopDec(short cardNum, short profile, 
            out double pDecSmoothStop, out double pDecAbruptStop);
        
        //设置运动控制器各轴限位开关触发电平
        /// <summary>
        /// 设置运动控制器各轴限位开关触发电平
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="sense">按位标识轴的限位的触发电平状态</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_LmtSns(short cardNum, ushort sense);
        
        //设置控制轴为模拟量输出或脉冲输出
        /// <summary>
        /// 设置控制轴为模拟量输出或脉冲输出
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">控制轴号</param>
        /// <param name="mode">切换的模式
        /// 0：将指定轴切换为闭环控制模式(电压控制方式)
        /// 1：将指定轴切换为开环控制模式(脉冲控制方式)</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_CtrlMode(short cardNum, short axis, short mode);
        
        //设置平滑停止和紧急停止数字量输入的信息
        /// <summary>
        /// 设置平滑停止和紧急停止数字量输入的信息
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">控制轴号</param>
        /// <param name="stopType">需要设置停止IO信息的停止类型
        /// 0：紧急停止类型
        /// 1：平滑停止类型</param>
        /// <param name="inputType">设置的数字量输入的类型
        /// MC_LIMIT_POSITIVE(该宏定义为0) 正限位
        /// MC_LIMIT_NEGATIVE(该宏定义为1) 负限位
        /// MC_ALARM(该宏定义为2) 驱动报警
        /// MC_HOME(该宏定义为3) 原点开关
        /// MC_GPI(该宏定义为4) 通用输入
        /// MC_ARRIVE(该宏定义为5) 电机到位信号(仅适用于GTS-400-PX控制器)</param>
        /// <param name="inputIndex">设置的数字量输入的索引号，取值范围根据inputType的取值而定
        /// 当inputType= MC_LIMIT_POSITIVE时，取值范围：[1,8]
        /// 当inputType= MC_LIMIT_NEGATIVE时，取值范围：[1,8]
        /// 当inputType= MC_ALARM时，取值范围：[1,8]
        /// 当inputType= MC_HOME时，取值范围：[1,8]
        /// 当inputType= MC_GPI时，取值范围：[1,16]
        /// 当inputType= MC_ARRIVE时，取值范围：[1,8]</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetStopIo(short cardNum, short axis, 
            short stopType, short inputType, short inputIndex);
        
        //设置运动控制器数字量输入的电平逻辑
        /// <summary>
        /// 设置运动控制器数字量输入的电平逻辑
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="sense">按位表示各数量输入的电平逻辑，从bit0~bit15，分别对应数字量输入1到16。
        /// 0：输入电平不取反，通过GT_GetDi()指令读取到0表示输入低电平，通过GT_GetDi()指令读取到1表示输入高电平；
        /// 1：输入电平取反，通过GT_GetDi()指令读取到0表示输入高电平，通过GT_GetDi()指令读取到1表示输入低电平；</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GpiSns(short cardNum, ushort sense);

        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd"></param>
        /// <param name="pCrdData"></param>
        /// <param name="fifo"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_CrdDataCircle(short cardNum, short crd, 
            ref TCrdData pCrdData, short fifo);
        
        //缓存区指令，两维直线插补
        /// <summary>
        /// 缓存区指令，两维直线插补
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="x">插补段x轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="y">插补段y轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="synVel">插补段的目标合成速度。取值范围：(0,32767)，单位：pulse/ms</param>
        /// <param name="synAcc">插补段的合成加速度。取值范围：(0,32767)，单位：pulse/(ms*ms)</param>
        /// <param name="velEnd">插补段的终点速度。取值范围：[0,32767)，单位：pulse/ms。
        /// 该值只有在没有使用前瞻预处理功能时才有意义，否则该值无效。默认为：0</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_LnXY(short cardNum, short crd, int x, 
            int y, double synVel, double synAcc, double velEnd, short fifo);
        
        //缓存区指令，三维直线插补
        /// <summary>
        /// 缓存区指令，三维直线插补
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="x">插补段x轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="y">插补段y轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="z">插补段z轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="synVel">插补段的目标合成速度。取值范围：(0,32767)，单位：pulse/ms</param>
        /// <param name="synAcc">插补段的合成加速度。取值范围：(0,32767)，单位：pulse/(ms*ms)</param>
        /// <param name="velEnd">插补段的终点速度。取值范围：[0,32767)，单位：pulse/ms。
        /// 该值只有在没有使用前瞻预处理功能时才有意义，否则该值无效。默认为：0</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_LnXYZ(short cardNum, short crd, int x, 
            int y, int z, double synVel, double synAcc, double velEnd, short fifo);
        
        //缓存区指令，四维直线插补
        /// <summary>
        /// 缓存区指令，四维直线插补
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="x">插补段x轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="y">插补段y轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="z">插补段z轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="a">插补段a轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="synVel">插补段的目标合成速度。取值范围：(0,32767)，单位：pulse/ms</param>
        /// <param name="synAcc">插补段的合成加速度。取值范围：(0,32767)，单位：pulse/(ms*ms)</param>
        /// <param name="velEnd">插补段的终点速度。取值范围：[0,32767)，单位：pulse/ms。
        /// 该值只有在没有使用前瞻预处理功能时才有意义，否则该值无效。默认为：0</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_LnXYZA(short cardNum, short crd, int x, 
            int y, int z, int a, double synVel, double synAcc, double velEnd, short fifo);
        
        //缓存区指令，两维直线插补(终点速度始终为0)
        /// <summary>
        /// 缓存区指令，两维直线插补(终点速度始终为0)
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="x">插补段x轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="y">插补段y轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="synVel">插补段的目标合成速度。取值范围：(0,32767)，单位：pulse/ms</param>
        /// <param name="synAcc">插补段的合成加速度。取值范围：(0,32767)，单位：pulse/(ms*ms)</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_LnXYG0(short cardNum, short crd, int x,
            int y, double synVel, double synAcc, short fifo);
        
        //缓存区指令，三维直线插补(终点速度始终为0)
        /// <summary>
        /// 缓存区指令，三维直线插补(终点速度始终为0)
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="x">插补段x轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="y">插补段y轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="z">插补段z轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="synVel">插补段的目标合成速度。取值范围：(0,32767)，单位：pulse/ms</param>
        /// <param name="synAcc">插补段的合成加速度。取值范围：(0,32767)，单位：pulse/(ms*ms)</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_LnXYZG0(short cardNum, short crd, int x,
            int y, int z, double synVel, double synAcc, short fifo);
        
        //缓存区指令，四维直线插补(终点速度始终为0)
        /// <summary>
        /// 缓存区指令，四维直线插补(终点速度始终为0)
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="x">插补段x轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="y">插补段y轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="z">插补段z轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="a">插补段a轴终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="synVel">插补段的目标合成速度。取值范围：(0,32767)，单位：pulse/ms</param>
        /// <param name="synAcc">插补段的合成加速度。取值范围：(0,32767)，单位：pulse/(ms*ms)</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_LnXYZAG0(short cardNum, short crd, int x,
            int y, int z, int a, double synVel, double synAcc, short fifo);
        
        //缓存区指令，XY平面圆弧插补(以终点位置和半径为输入参数)
        /// <summary>
        /// 缓存区指令，XY平面圆弧插补(以终点位置和半径为输入参数)
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="x">圆弧插补x轴的终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="y">圆弧插补y轴的终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="radius">圆弧插补的圆弧半径值。取值范围：[-1073741823, 1073741823]，单位：pulse。
        /// 半径为正时，表示圆弧为小于等于180°圆弧
        /// 半径为负时，表示圆弧为大于180°圆弧
        /// 半径描述方式不能用来描述整圆</param>
        /// <param name="circleDir">圆弧的旋转方向
        /// 0：顺时针圆弧
        /// 1：逆时针圆弧</param>
        /// <param name="synVel">插补段的目标合成速度。取值范围：(0,32767)，单位：pulse/ms</param>
        /// <param name="synAcc">插补段的合成加速度。取值范围：(0,32767)，单位：pulse/(ms*ms)</param>
        /// <param name="velEnd">插补段的终点速度。取值范围：[0,32767)，单位：pulse/ms。
        /// 该值只有在没有使用前瞻预处理功能时才有意义，否则该值无效。默认为：0</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_ArcXYR(short cardNum, short crd, int x, 
            int y, double radius, short circleDir, double synVel, double synAcc,
            double velEnd, short fifo);
        
        //缓存区指令，XY平面圆弧插补(以终点位置和圆心位置为输入参数)
        /// <summary>
        /// 缓存区指令，XY平面圆弧插补(以终点位置和圆心位置为输入参数)
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="x">圆弧插补x轴的终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="y">圆弧插补y轴的终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="xCenter">圆弧插补的圆心x方向相对于起点位置的偏移量</param>
        /// <param name="yCenter">圆弧插补的圆心y方向相对于起点位置的偏移量</param>
        /// <param name="circleDir">圆弧的旋转方向
        /// 0：顺时针圆弧
        /// 1：逆时针圆弧</param>
        /// <param name="synVel">插补段的目标合成速度。取值范围：(0,32767)，单位：pulse/ms</param>
        /// <param name="synAcc">插补段的合成加速度。取值范围：(0,32767)，单位：pulse/(ms*ms)</param>
        /// <param name="velEnd">插补段的终点速度。取值范围：[0,32767)，单位：pulse/ms。
        /// 该值只有在没有使用前瞻预处理功能时才有意义，否则该值无效。默认为：0</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_ArcXYC(short cardNum, short crd, int x,
            int y, double xCenter, double yCenter, short circleDir, double synVel,
            double synAcc, double velEnd, short fifo);
        
        //缓存区指令，YZ平面圆弧插补(以终点位置和半径为输入参数)
        /// <summary>
        /// 缓存区指令，YZ平面圆弧插补(以终点位置和半径为输入参数)
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="y">圆弧插补y轴的终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="z">圆弧插补z轴的终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="radius">圆弧插补的圆弧半径值。取值范围：[-1073741823, 1073741823]，单位：pulse。
        /// 半径为正时，表示圆弧为小于等于180°圆弧
        /// 半径为负时，表示圆弧为大于180°圆弧
        /// 半径描述方式不能用来描述整圆</param>
        /// <param name="circleDir">圆弧的旋转方向
        /// 0：顺时针圆弧
        /// 1：逆时针圆弧
        /// </param>
        /// <param name="synVel">插补段的目标合成速度。取值范围：(0,32767)，单位：pulse/ms</param>
        /// <param name="synAcc">插补段的合成加速度。取值范围：(0,32767)，单位：pulse/(ms*ms)</param>
        /// <param name="velEnd">插补段的终点速度。取值范围：[0,32767)，单位：pulse/ms。
        /// 该值只有在没有使用前瞻预处理功能时才有意义，否则该值无效。默认为：0</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_ArcYZR(short cardNum, short crd, int y,
            int z, double radius, short circleDir, double synVel, double synAcc, 
            double velEnd, short fifo);
        
        //缓存区指令，YZ平面圆弧插补(以终点位置和圆心位置为输入参数)
        /// <summary>
        /// 缓存区指令，YZ平面圆弧插补(以终点位置和圆心位置为输入参数)
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="y">圆弧插补y轴的终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="z">圆弧插补z轴的终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="yCenter">圆弧插补的圆心y方向相对于起点位置的偏移量</param>
        /// <param name="zCenter">圆弧插补的圆心z方向相对于起点位置的偏移量</param>
        /// <param name="circleDir">圆弧的旋转方向
        /// 0：顺时针圆弧
        /// 1：逆时针圆弧</param>
        /// <param name="synVel">插补段的目标合成速度。取值范围：(0,32767)，单位：pulse/ms</param>
        /// <param name="synAcc">插补段的合成加速度。取值范围：(0,32767)，单位：pulse/(ms*ms)</param>
        /// <param name="velEnd">插补段的终点速度。取值范围：[0,32767)，单位：pulse/ms。
        /// 该值只有在没有使用前瞻预处理功能时才有意义，否则该值无效。默认为：0</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_ArcYZC(short cardNum, short crd, int y, 
            int z, double yCenter, double zCenter, short circleDir, double synVel,
            double synAcc, double velEnd, short fifo);
        
        //缓存区指令，ZX平面圆弧插补(以终点位置和半径为输入参数)
        /// <summary>
        /// 缓存区指令，ZX平面圆弧插补(以终点位置和半径为输入参数)
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="z">圆弧插补z轴的终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="x">圆弧插补x轴的终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="radius">圆弧插补的圆弧半径值。取值范围：[-1073741823, 1073741823]，单位：pulse。
        /// 半径为正时，表示圆弧为小于等于180°圆弧
        /// 半径为负时，表示圆弧为大于180°圆弧
        /// 半径描述方式不能用来描述整圆</param>
        /// <param name="circleDir">圆弧的旋转方向
        /// 0：顺时针圆弧
        /// 1：逆时针圆弧</param>
        /// <param name="synVel">插补段的目标合成速度。取值范围：(0,32767)，单位：pulse/ms</param>
        /// <param name="synAcc">插补段的合成加速度。取值范围：(0,32767)，单位：pulse/(ms*ms)</param>
        /// <param name="velEnd">插补段的终点速度。取值范围：[0,32767)，单位：pulse/ms。该值只
        /// 有在没有使用前瞻预处理功能时才有意义，否则该值无效。默认为：0</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_ArcZXR(short cardNum, short crd, int z,
            int x, double radius, short circleDir, double synVel, double synAcc,
            double velEnd, short fifo);
        
        //缓存区指令，ZX平面圆弧插补(以终点位置和圆心位置为输入参数)
        /// <summary>
        /// 缓存区指令，ZX平面圆弧插补(以终点位置和圆心位置为输入参数)
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="z">圆弧插补z轴的终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="x">圆弧插补x轴的终点坐标值。取值范围：[-1073741823, 1073741823]，单位：pulse</param>
        /// <param name="zCenter">圆弧插补的圆心z方向相对于起点位置的偏移量</param>
        /// <param name="xCenter">圆弧插补的圆心x方向相对于起点位置的偏移量</param>
        /// <param name="circleDir">圆弧的旋转方向
        /// 0：顺时针圆弧
        /// 1：逆时针圆弧</param>
        /// <param name="synVel">插补段的目标合成速度。取值范围：(0,32767)，单位：pulse/ms</param>
        /// <param name="synAcc">插补段的合成加速度。取值范围：(0,32767)，单位：pulse/(ms*ms)</param>
        /// <param name="velEnd">插补段的终点速度。取值范围：[0,32767)，单位：pulse/ms。
        /// 该值只有在没有使用前瞻预处理功能时才有意义，否则该值无效。默认为：0</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_ArcZXC(short cardNum, short crd, int z,
            int x, double zCenter, double xCenter, short circleDir, double synVel,
            double synAcc, double velEnd, short fifo);
        
        //缓存区指令，缓存区内数字量IO输出设置指令
        /// <summary>
        /// 缓存区指令，缓存区内数字量IO输出设置指令
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="doType">数字量输出的类型：
        /// MC_ENABLE(该宏定义为10)：输出驱动器使能。
        /// MC_CLEAR(该宏定义为11)：输出驱动器报警清除。
        /// MC_GPO(该宏定义为12)：输出通用输出</param>
        /// <param name="doMask">从bit0~bit15按位表示指定的数字量输出是否有操作，
        /// 0：该路数字量输出无操作；1：该路数字量输出有操作</param>
        /// <param name="doValue">从bit0~bit15按位表示指定的数字量输出的值</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_BufIO(short cardNum, short crd, 
            ushort doType, ushort doMask, ushort doValue, short fifo);
        
        //缓存区指令，缓存区内延时设置指令
        /// <summary>
        /// 缓存区指令，缓存区内延时设置指令
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="delayTime">延时时间，取值范围：[0,16383]，单位：ms</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_BufDelay(short cardNum, short crd, 
            ushort delayTime, short fifo);
        
        //缓存区指令，缓存区内输出DA值
        /// <summary>
        /// 缓存区指令，缓存区内输出DA值
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="chn">模拟量输出的通道号，取值范围：[1,8]</param>
        /// <param name="daValue">模拟量输出的值，取值范围：[-32768,32767]，
        /// 其中：-32768对应-10V，32767对应+10V</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_BufDA(short cardNum, short crd,
            short chn, short daValue, short fifo);
        
        //缓存区指令，缓存区内有效限位开关
        /// <summary>
        /// 缓存区指令，缓存区内有效限位开关
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="axis">需要将限位有效的轴的编号，取值范围：[1,8]</param>
        /// <param name="limitType">需要有效的限位类型
        /// MC_LIMIT_POSITIVE(该宏定义为0)：需要将该轴的正限位有效
        /// MC_LIMIT_NEGATIVE(该宏定义为1)：需要将该轴的负限位有效
        /// -1：需要将该轴的正限位和负限位都有效，默认为该值</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_BufLmtsOn(short cardNum, short crd,
            short axis, short limitType, short fifo);
        
        //缓存区指令，缓存区内无效限位开关
        /// <summary>
        /// 缓存区指令，缓存区内无效限位开关
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="axis">需要将限位无效的轴的编号，取值范围：[1,8]</param>
        /// <param name="limitType">需要无效的限位类型
        /// MC_LIMIT_POSITIVE(该宏定义为0)：需要将该轴的正限位无效
        /// MC_LIMIT_NEGATIVE(该宏定义为1)：需要将该轴的负限位无效
        /// -1：需要将该轴的正限位和负限位都无效，默认为该值</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_BufLmtsOff(short cardNum, short crd,
            short axis, short limitType, short fifo);
        
        //缓存区指令，缓存区内设置axis的停止IO信息
        /// <summary>
        /// 缓存区指令，缓存区内设置axis的停止IO信息
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="axis">需要设置停止IO信息的轴的编号，取值范围：[1,8]</param>
        /// <param name="stopType">需要设置停止IO信息的停止类型
        /// 0：紧急停止类型
        /// 1：平滑停止类型</param>
        /// <param name="inputType">设置的数字量输入的类型
        /// MC_LIMIT_POSITIVE(该宏定义为0) 正限位
        /// MC_LIMIT_NEGATIVE(该宏定义为1) 负限位
        /// MC_ALARM(该宏定义为2) 驱动报警
        /// MC_HOME(该宏定义为3) 原点开关
        /// MC_GPI(该宏定义为4) 通用输入
        /// MC_ARRIVE(该宏定义为5) 电机到位信号(仅适用于GTS-400-PX控制器)</param>
        /// <param name="inputIndex">设置的数字量输入的索引号，取值范围根据inputType的取值而定
        /// 当inputType= MC_LIMIT_POSITIVE时，取值范围：[1,8]
        /// 当inputType= MC_LIMIT_NEGATIVE时，取值范围：[1,8]
        /// 当inputType= MC_ALARM时，取值范围：[1,8]
        /// 当inputType= MC_HOME时，取值范围：[1,8]
        /// 当inputType= MC_GPI时，取值范围：[1,16]
        /// 当inputType= MC_ARRIVE时，取值范围：[1,8]</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_BufSetStopIo(short cardNum, short crd,
            short axis, short stopType, short inputType, short inputIndex, short fifo);
        
        //缓存区指令，实现刀向跟随功能，启动某个轴点位运动
        /// <summary>
        /// 缓存区指令，实现刀向跟随功能，启动某个轴点位运动
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="moveAxis">需要进行点位运动的轴号，取值范围：[1,8]，
        /// 该轴不能处于坐标系中</param>
        /// <param name="pos">点位运动的目标位置，单位：pulse</param>
        /// <param name="vel">点位运动的目标速度，单位：pulse/ms</param>
        /// <param name="acc">点位运动的加速度，单位：pulse/(ms*ms)</param>
        /// <param name="modal">点位运动的模式：
        /// 0：该指令为非模态指令，即不阻塞后续的插补缓存区指令的执行
        /// 1：该指令为模态指令，将会阻塞后续的插补缓存区指令的执行</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_BufMove(short cardNum, short crd, short moveAxis,
            int pos, double vel, double acc, short modal, short fifo);
        
        //缓存区指令，实现刀向跟随功能，启动某个轴跟随运动
        /// <summary>
        /// 缓存区指令，实现刀向跟随功能，启动某个轴跟随运动
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="crd">坐标系号，取值范围：[1,2]</param>
        /// <param name="gearAxis">需要进行跟随运动的轴号，取值范围：[1,8]，
        /// 该轴不能处于坐标系中</param>
        /// <param name="pos">跟随运动的位移量，单位：pulse</param>
        /// <param name="fifo">插补缓存区号，取值范围：[0,1]，默认为：0</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_BufGear(short cardNum, short crd, short gearAxis,
            int pos, short fifo);

        //初始化自动回原点功能
        /// <summary>
        /// 初始化自动回原点功能
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_HomeInit(short cardNum);
        
        //启动自动回原点功能
        /// <summary>
        /// 启动自动回原点功能
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">需要进行自动回原点操作的轴号，取值范围：[1,8]</param>
        /// <param name="pos">搜索距离，以当前位置为起点，
        /// 搜索距离为正时向正方向搜索，搜索距离为负时向负方向搜索，单位：脉冲</param>
        /// <param name="vel">搜索速度，单位：脉冲/毫秒</param>
        /// <param name="acc">搜索加速度，单位：脉冲/(毫秒*毫秒)</param>
        /// <param name="offset">原点偏移量，当原点信号触发时，
        /// 将当前轴目标位置自动更新为“原点位置＋原点偏移”。
        /// 如果原点偏移量为0，当原点信号触发时，首先平滑停止减速到0，然后返回原点</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_Home(short cardNum, short axis, int pos,
            double vel, double acc, int offset);
        
        //设置自动回原点功能为home+index模式
        /// <summary>
        /// 设置自动回原点功能为home+index模式
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">需要进行自动home+index回原点操作的轴号，取值范围：[1,8]</param>
        /// <param name="pos">Index信号的搜索距离。
        /// Home信号触发时，以Home位置为起点搜索Index，
        /// 搜索距离为正时向正方向搜索，搜索距离为负时向负方向搜索。
        /// Index搜索速度是Home搜索速度的一半，Index搜索加速度和Home搜索加速度相同</param>
        /// <param name="offset">Index信号偏移量，
        /// 当index信号触发时，将当前轴目标位置自动更新为“index位置＋index偏移”。
        /// 如果index偏移量为0，当index信号触发时，首先平滑停止减速到0，然后返回index位置</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_Index(short cardNum, short axis, int pos, int offset);
        
        //启动原点停止功能
        /// <summary>
        /// 启动原点停止功能
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">需要进行原点急停操作的轴号，取值范围：[1,8]</param>
        /// <param name="pos">搜索距离，单位：脉冲
        /// 以当前位置为起点，搜索距离为正时向正方向搜索，搜索距离为负时向负方向搜索</param>
        /// <param name="vel">搜索速度，单位：脉冲/毫秒</param>
        /// <param name="acc">搜索加速度，单位：脉冲/(毫秒*毫秒)</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_HomeStop(short cardNum, short axis,
            int pos, double vel, double acc);
        
        //查询自动回原点的运行状态
        /// <summary>
        /// 查询自动回原点的运行状态
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="axis">需要查询自动回原点状态的轴号，取值范围：[1,8]</param>
        /// <param name="pStatus">查询到的状态值
        /// 0：自动回原点操作正在执行
        /// 1：自动回原点操作成功执行完毕</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_HomeSts(short cardNum, short axis, out ushort pStatus);

        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="level"></param>
        /// <param name="outputType"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_ComparePulse(short cardNum, short level,
            short outputType, short time);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_CompareStop(short cardNum);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="pStatus"></param>
        /// <param name="pCount"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_CompareStatus(short cardNum, out short pStatus, out int pCount);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="encoder"></param>
        /// <param name="source"></param>
        /// <param name="pulseType"></param>
        /// <param name="startLevel"></param>
        /// <param name="time"></param>
        /// <param name="pBuf1"></param>
        /// <param name="count1"></param>
        /// <param name="pBuf2"></param>
        /// <param name="count2"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_CompareData(short cardNum, short encoder, 
            short source, short pulseType, short startLevel, short time, ref int pBuf1,
            short count1, ref int pBuf2, short count2);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="encoder"></param>
        /// <param name="channel"></param>
        /// <param name="startPos"></param>
        /// <param name="repeatTimes"></param>
        /// <param name="interval"></param>
        /// <param name="time"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_CompareLinear(short cardNum, short encoder,
            short channel, int startPos, int repeatTimes, int interval, short time, short source);
        
        #endregion

        #region "固高扩展模块函数"

        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="pDllName"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_OpenExtMdlGts(short cardNum, string pDllName);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_CloseExtMdlGts(short cardNum);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="card"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SwitchtoCardNoExtMdlGts(short cardNum, short card);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_ResetExtMdlGts(short cardNum);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_LoadExtConfigGts(short cardNum, string fileName);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mdl"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetExtIoValueGts(short cardNum, short mdl, ushort value);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mdl"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetExtIoValueGts(short cardNum, short mdl, ref ushort value);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mdl"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetExtIoBitGts(short cardNum, short mdl, short index, ushort value);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mdl"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetExtIoBitGts(short cardNum, short mdl, short index, ref ushort value);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mdl"></param>
        /// <param name="chn"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetExtAdValueGts(short cardNum, short mdl, short chn, ref ushort value);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mdl"></param>
        /// <param name="chn"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetExtAdVoltageGts(short cardNum, short mdl, short chn, ref double value);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mdl"></param>
        /// <param name="chn"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetExtDaValueGts(short cardNum, short mdl, short chn, ushort value);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mdl"></param>
        /// <param name="chn"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_SetExtDaVoltageGts(short cardNum, short mdl, short chn, double value);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mdl"></param>
        /// <param name="chn"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetStsExtMdlGts(short cardNum, short mdl, short chn, ref ushort value);
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNum">运动控制器卡号</param>
        /// <param name="mdl"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport("gts.dll")]
        private static extern short GT_GetExtDoValueGts(short cardNum, short mdl, ref ushort value);


        #endregion

        #endregion

        #region "运动卡变量定义"

        public string ErrorMessage = "";
        private bool SuccessBuiltNew = false, PasswordIsCorrect = false;

        /// <summary>
        /// 卡的编号
        /// </summary>
        internal static short CardNumber = 0;

        /// <summary>
        /// 当前固高运动卡的编号
        /// </summary>
        public short CurrentCardNumber
            {
            get { return CardNumber; }
            }

        /// <summary>
        /// 卡的轴数
        /// </summary>
        internal static short TotalAxisesOfCard = 0;

        /// <summary>
        /// 当前固高运动卡的轴数
        /// </summary>
        public short TotalAxisesOfCurrentCard
            {
            get { return TotalAxisesOfCard; }
            }

        ///// <summary>
        ///// 模拟量输入[获取电压值对应的数值]
        ///// </summary>
        //private double ADIVoltConvertValue = 0.0;


        //public double CurrentADIVoltConvertValue 
        //    {
        //    get { return ADIVoltConvertValue; }
        //    }

        /// <summary>
        /// 固高运动卡是否初始化成功
        /// </summary>
        internal static bool InitialCardSucceed = false;

        /// <summary>
        /// 固高运动卡是否初始化成功
        /// </summary>
        public bool InitialCardSuccess
            {
            get { return InitialCardSucceed; }
            }

        /// <summary>
        /// 控制卡轴的类数组
        /// </summary>
        public GTSAxisControl[] Axises;

        /// <summary>
        /// 信号电平枚举
        /// </summary>
        public enum EdgeForInputOutputSignals
            {
            /// <summary>
            /// 下降沿
            /// </summary>
            FallingEdge = 0,

            /// <summary>
            /// 上升沿
            /// </summary>
            RisingEdge = 1
            }

        /// <summary>
        /// 设置输入信号为上升沿或下降沿
        /// </summary>
        public EdgeForInputOutputSignals InputSignalEdge
            {
            set
                {
                if (value == EdgeForInputOutputSignals.FallingEdge)
                    {
                    GT_GpiSns(CardNumber, 0);
                    }
                else
                    {
                    GT_GpiSns(CardNumber, 65535);
                    }
                }
            }

        #region "四轴信号"

        /// <summary>
        /// 四轴固高运动卡输入信号
        /// </summary>
        private bool[] DI_GTS400 = new bool[28];

        ///// <summary>
        ///// 四轴固高运动卡输出信号
        ///// </summary>
        //public bool[] DO_GTS400 = new bool[16];

        /// <summary>
        /// 四轴固高运动卡输出信号
        /// </summary>
        private bool[] DO_GTS400 = new bool[16];

        /// <summary>
        /// 设置与读取四轴固高运动卡输出信号
        /// </summary>
        public bool[] SetDO_GTS400 
            {
            set 
                {
                if (value.Length != 16) 
                    {
                    ErrorMessage = "";
                    return;
                    }
                int Return = 0;
                for (int a = 0; a < value.Length; a++) 
                    {
                    if (value[a] == true)
                        {
                        Return += GT_SetDoBit(CardNumber, MC_GPO, (short)(a + 1), 1);
                        }
                    else
                        {
                        Return += GT_SetDoBit(CardNumber, MC_GPO, (short)(a + 1), 0);
                        }
                    }
                if (Return != 0)
                    {
                    ErrorMessage = "更新运动卡输出出错";
                    }
                else 
                    {
                    ErrorMessage = "更新运动卡输出成功";
                    }
                }

            get 
                {
                return ReadDOStatus();                
                }
            }
        
        /// <summary>
        /// 【作废】固高四轴运动卡输入信号枚举
        /// </summary>
        private enum OldDIGTS400 
            {
            
            //伺服原点
            /// <summary>
            /// 轴1原点
            /// </summary>
            Home1 = 0,//Home0

            /// <summary>
            /// 轴2原点
            /// </summary>
            Home2 = 1,//Home1

            /// <summary>
            /// 轴3原点
            /// </summary>
            Home3 = 2,//Home2

            /// <summary>
            /// 轴4原点
            /// </summary>
            Home4 = 3,//Home3

            //伺服限位

            /// <summary>
            /// 轴1正限位开关
            /// </summary>
            PLmt1 = 4,//PLmt0
            
            /// <summary>
            /// 轴1负限位开关
            /// </summary>
            NLmt1 = 5,//NLmt0
            
            /// <summary>
            /// 轴2正限位开关
            /// </summary>
            PLmt2 = 6,//PLmt1

            /// <summary>
            /// 轴2负限位开关
            /// </summary>
            NLmt2 = 7,//NLmt1
            
            /// <summary>
            /// 轴3正限位开关
            /// </summary>
            PLmt3 = 8,//PLmt2

            /// <summary>
            /// 轴3负限位开关
            /// </summary>
            NLmt3 = 9,//NLmt2

            /// <summary>
            /// 轴4正限位开关
            /// </summary>
            PLmt4 = 10,//PLmt3
            
            /// <summary>
            /// 轴4负限位开关
            /// </summary>
            NLmt4 = 11,//NLmt3
            
            //通用IO输入
            /// <summary>
            /// 通用IO输入位0
            /// </summary>
            EXI_0 = 12,

            /// <summary>
            /// 通用IO输入位1
            /// </summary>
            EXI_1 = 13,
            
            /// <summary>
            /// 通用IO输入位2
            /// </summary>
            EXI_2 = 14,
            
            /// <summary>
            /// 通用IO输入位3
            /// </summary>
            EXI_3 = 15,
            
            /// <summary>
            /// 通用IO输入位4
            /// </summary>
            EXI_4 = 16,
            
            /// <summary>
            /// 通用IO输入位5
            /// </summary>
            EXI_5 = 17,

            /// <summary>
            /// 通用IO输入位6
            /// </summary>
            EXI_6 = 18,
            
            /// <summary>
            /// 通用IO输入位7
            /// </summary>
            EXI_7 = 19,
            
            /// <summary>
            /// 通用IO输入位8
            /// </summary>
            EXI_8 = 20,
            
            /// <summary>
            /// 通用IO输入位9
            /// </summary>
            EXI_9 = 21,
            
            /// <summary>
            /// 通用IO输入位10
            /// </summary>
            EXI_10 = 22,
            
            /// <summary>
            /// 通用IO输入位11
            /// </summary>
            EXI_11 = 23,
            
            /// <summary>
            /// 通用IO输入位12
            /// </summary>
            EXI_12 = 24,
            
            /// <summary>
            /// 通用IO输入位13
            /// </summary>
            EXI_13 = 25,
            
            /// <summary>
            /// 通用IO输入位14
            /// </summary>
            EXI_14 = 26,
            
            /// <summary>
            /// 通用IO输入位15
            /// </summary>
            EXI_15 = 27,
            
            }

        /// <summary>
        /// 固高四轴运动卡输出信号枚举
        /// </summary>
        public enum DOGTS400
            {

            //通用IO输出位
            /// <summary>
            /// 通用IO输出位0
            /// </summary>
            EXO_0 = 0,

            /// <summary>
            /// 通用IO输出位1
            /// </summary>
            EXO_1 = 1,

            /// <summary>
            /// 通用IO输出位2
            /// </summary>
            EXO_2 = 2,

            /// <summary>
            /// 通用IO输出位3
            /// </summary>
            EXO_3 = 3,

            /// <summary>
            /// 通用IO输出位4
            /// </summary>
            EXO_4 = 4,

            /// <summary>
            /// 通用IO输出位5
            /// </summary>
            EXO_5 = 5,

            /// <summary>
            /// 通用IO输出位6
            /// </summary>
            EXO_6 = 6,

            /// <summary>
            /// 通用IO输出位7
            /// </summary>
            EXO_7 = 7,

            /// <summary>
            /// 通用IO输出位8
            /// </summary>
            EXO_8 = 8,

            /// <summary>
            /// 通用IO输出位9
            /// </summary>
            EXO_9 = 9,

            /// <summary>
            /// 通用IO输出位10
            /// </summary>
            EXO_10 = 10,

            /// <summary>
            /// 通用IO输出位11
            /// </summary>
            EXO_11 = 11,

            /// <summary>
            /// 通用IO输出位12
            /// </summary>
            EXO_12 = 12,

            /// <summary>
            /// 通用IO输出位13
            /// </summary>
            EXO_13 = 13,

            /// <summary>
            /// 通用IO输出位14
            /// </summary>
            EXO_14 = 14,

            /// <summary>
            /// 通用IO输出位15
            /// </summary>
            EXO_15 = 15

            }

        /// <summary>
        /// 固高四轴运动卡原点输入信号枚举
        /// </summary>
        public enum HomeGTS400
            {
            //****************
            //伺服原点
            /// <summary>
            /// 轴1原点
            /// </summary>
            Home0 = 0,

            /// <summary>
            /// 轴2原点
            /// </summary>
            Home1 = 1,

            /// <summary>
            /// 轴3原点
            /// </summary>
            Home2 = 2,

            /// <summary>
            /// 轴4原点
            /// </summary>
            Home3 = 3
            }

        /// <summary>
        /// 固高四轴运动卡正限位输入信号枚举
        /// </summary>
        public enum PLMTGTS400
            {
            //****************
            //伺服正限位
            /// <summary>
            /// 轴1正限位开关
            /// </summary>
            PLmt0 = 0,

            /// <summary>
            /// 轴2正限位开关
            /// </summary>
            PLmt1 = 1,

            /// <summary>
            /// 轴3正限位开关
            /// </summary>
            PLmt2 = 2,

            /// <summary>
            /// 轴4正限位开关
            /// </summary>
            PLmt3 = 3
            }

        /// <summary>
        /// 固高四轴运动卡负限位输入信号枚举
        /// </summary>
        public enum NLMTGTS400
            {
            //****************
            //伺服负限位

            /// <summary>
            /// 轴1负限位开关
            /// </summary>
            NLmt0 = 0,

            /// <summary>
            /// 轴2负限位开关
            /// </summary>
            NLmt1 = 1,

            /// <summary>
            /// 轴3负限位开关
            /// </summary>
            NLmt2 = 2,

            /// <summary>
            /// 轴4负限位开关
            /// </summary>
            NLmt3 = 3
            }

        /// <summary>
        /// 固高四轴运动卡通用输入信号枚举
        /// </summary>
        public enum DIGTS400
            {
            //******************
            //通用IO输入
            /// <summary>
            /// 通用IO输入位0
            /// </summary>
            EXI_0 = 0,

            /// <summary>
            /// 通用IO输入位1
            /// </summary>
            EXI_1 = 1,

            /// <summary>
            /// 通用IO输入位2
            /// </summary>
            EXI_2 = 2,

            /// <summary>
            /// 通用IO输入位3
            /// </summary>
            EXI_3 = 3,

            /// <summary>
            /// 通用IO输入位4
            /// </summary>
            EXI_4 = 4,

            /// <summary>
            /// 通用IO输入位5
            /// </summary>
            EXI_5 = 5,

            /// <summary>
            /// 通用IO输入位6
            /// </summary>
            EXI_6 = 6,

            /// <summary>
            /// 通用IO输入位7
            /// </summary>
            EXI_7 = 7,

            /// <summary>
            /// 通用IO输入位8
            /// </summary>
            EXI_8 = 8,

            /// <summary>
            /// 通用IO输入位9
            /// </summary>
            EXI_9 = 9,

            /// <summary>
            /// 通用IO输入位10
            /// </summary>
            EXI_10 = 10,

            /// <summary>
            /// 通用IO输入位11
            /// </summary>
            EXI_11 = 11,

            /// <summary>
            /// 通用IO输入位12
            /// </summary>
            EXI_12 = 12,

            /// <summary>
            /// 通用IO输入位13
            /// </summary>
            EXI_13 = 13,

            /// <summary>
            /// 通用IO输入位14
            /// </summary>
            EXI_14 = 14,

            /// <summary>
            /// 通用IO输入位15
            /// </summary>
            EXI_15 = 15
            }

        #endregion

        #region "八轴信号"

        /// <summary>
        /// 八轴固高运动卡输入信号
        /// </summary>
        private bool[] DI_GTS800 = new bool[40];

        ///// <summary>
        ///// 八轴固高运动卡输出信号
        ///// </summary>
        //public bool[] DO_GTS800 = new bool[16];

        /// <summary>
        /// 【作废】固高八轴运动卡输入信号枚举
        /// </summary>
        private enum OldDIGTS800
            {
            //****************
            //伺服原点
            /// <summary>
            /// 轴1原点
            /// </summary>
            Home1 = 0,//Home0

            /// <summary>
            /// 轴2原点
            /// </summary>
            Home2 = 1,//Home1

            /// <summary>
            /// 轴3原点
            /// </summary>
            Home3 = 2,//Home2

            /// <summary>
            /// 轴4原点
            /// </summary>
            Home4 = 3,//Home3
            
            /// <summary>
            /// 轴5原点
            /// </summary>
            Home5 = 4,//Home4

            /// <summary>
            /// 轴6原点
            /// </summary>
            Home6 = 5,//Home5

            /// <summary>
            /// 轴7原点
            /// </summary>
            Home7 = 6,//Home6

            /// <summary>
            /// 轴8原点
            /// </summary>
            Home8 = 7,//Home7

            //****************
            //伺服限位

            /// <summary>
            /// 轴1正限位开关
            /// </summary>
            PLmt1 = 8,//PLmt0

            /// <summary>
            /// 轴1负限位开关
            /// </summary>
            NLmt1 = 9,//NLmt0

            /// <summary>
            /// 轴2正限位开关
            /// </summary>
            PLmt2 = 10,//PLmt1

            /// <summary>
            /// 轴2负限位开关
            /// </summary>
            NLmt2 = 11,//NLmt1

            /// <summary>
            /// 轴3正限位开关
            /// </summary>
            PLmt3 = 12,//PLmt2

            /// <summary>
            /// 轴3负限位开关
            /// </summary>
            NLmt3 = 13,//NLmt2

            /// <summary>
            /// 轴4正限位开关
            /// </summary>
            PLmt4 = 14,//PLmt3

            /// <summary>
            /// 轴4负限位开关
            /// </summary>
            NLmt4 = 15,//NLmt3
            
            /// <summary>
            /// 轴5正限位开关
            /// </summary>
            PLmt5 = 16,//PLmt4

            /// <summary>
            /// 轴5负限位开关
            /// </summary>
            NLmt5 = 17,//NLmt4

            /// <summary>
            /// 轴6正限位开关
            /// </summary>
            PLmt6 = 18,//PLmt5

            /// <summary>
            /// 轴6负限位开关
            /// </summary>
            NLmt6 = 19,//NLmt5

            /// <summary>
            /// 轴7正限位开关
            /// </summary>
            PLmt7 = 20,//PLmt6

            /// <summary>
            /// 轴7负限位开关
            /// </summary>
            NLmt7 = 21,//NLmt6

            /// <summary>
            /// 轴8正限位开关
            /// </summary>
            PLmt8 = 22,//PLmt7

            /// <summary>
            /// 轴8负限位开关
            /// </summary>
            NLmt8 = 23,//NLmt7
            
            //******************
            //通用IO输入
            /// <summary>
            /// 通用IO输入位0
            /// </summary>
            EXI_0 = 24,

            /// <summary>
            /// 通用IO输入位1
            /// </summary>
            EXI_1 = 25,

            /// <summary>
            /// 通用IO输入位2
            /// </summary>
            EXI_2 = 26,

            /// <summary>
            /// 通用IO输入位3
            /// </summary>
            EXI_3 = 27,

            /// <summary>
            /// 通用IO输入位4
            /// </summary>
            EXI_4 = 28,

            /// <summary>
            /// 通用IO输入位5
            /// </summary>
            EXI_5 = 29,

            /// <summary>
            /// 通用IO输入位6
            /// </summary>
            EXI_6 = 30,

            /// <summary>
            /// 通用IO输入位7
            /// </summary>
            EXI_7 = 31,

            /// <summary>
            /// 通用IO输入位8
            /// </summary>
            EXI_8 = 32,

            /// <summary>
            /// 通用IO输入位9
            /// </summary>
            EXI_9 = 33,

            /// <summary>
            /// 通用IO输入位10
            /// </summary>
            EXI_10 = 34,

            /// <summary>
            /// 通用IO输入位11
            /// </summary>
            EXI_11 = 35,

            /// <summary>
            /// 通用IO输入位12
            /// </summary>
            EXI_12 = 36,

            /// <summary>
            /// 通用IO输入位13
            /// </summary>
            EXI_13 = 37,

            /// <summary>
            /// 通用IO输入位14
            /// </summary>
            EXI_14 = 38,

            /// <summary>
            /// 通用IO输入位15
            /// </summary>
            EXI_15 = 39,

            }

        /// <summary>
        /// 固高八轴运动卡原点输入信号枚举
        /// </summary>
        public enum HomeGTS800
            {
            //****************
            //伺服原点
            /// <summary>
            /// 轴1原点
            /// </summary>
            Home0 = 0,

            /// <summary>
            /// 轴2原点
            /// </summary>
            Home1 = 1,

            /// <summary>
            /// 轴3原点
            /// </summary>
            Home2 = 2,

            /// <summary>
            /// 轴4原点
            /// </summary>
            Home3 = 3,

            /// <summary>
            /// 轴5原点
            /// </summary>
            Home4 = 4,

            /// <summary>
            /// 轴6原点
            /// </summary>
            Home5 = 5,

            /// <summary>
            /// 轴7原点
            /// </summary>
            Home6 = 6,

            /// <summary>
            /// 轴8原点
            /// </summary>
            Home7 = 7
            }

        /// <summary>
        /// 固高八轴运动卡正限位输入信号枚举
        /// </summary>
        public enum PLMTGTS800
            {
            //****************
            //伺服正限位
            /// <summary>
            /// 轴1正限位开关
            /// </summary>
            PLmt0 = 0,

            /// <summary>
            /// 轴2正限位开关
            /// </summary>
            PLmt1 = 1,

            /// <summary>
            /// 轴3正限位开关
            /// </summary>
            PLmt2 = 2,

            /// <summary>
            /// 轴4正限位开关
            /// </summary>
            PLmt3 = 3,

            /// <summary>
            /// 轴5正限位开关
            /// </summary>
            PLmt4 = 4,

            /// <summary>
            /// 轴6正限位开关
            /// </summary>
            PLmt5 = 5,

            /// <summary>
            /// 轴7正限位开关
            /// </summary>
            PLmt6 = 6,

            /// <summary>
            /// 轴8正限位开关
            /// </summary>
            PLmt7 = 7
            }

        /// <summary>
        /// 固高八轴运动卡负限位输入信号枚举
        /// </summary>
        public enum NLMTGTS800
            {
            //****************
            //伺服负限位

            /// <summary>
            /// 轴1负限位开关
            /// </summary>
            NLmt0 = 0,

            /// <summary>
            /// 轴2负限位开关
            /// </summary>
            NLmt1 = 1,

            /// <summary>
            /// 轴3负限位开关
            /// </summary>
            NLmt2 = 2,

            /// <summary>
            /// 轴4负限位开关
            /// </summary>
            NLmt3 = 3,

            /// <summary>
            /// 轴5负限位开关
            /// </summary>
            NLmt4 = 4,

            /// <summary>
            /// 轴6负限位开关
            /// </summary>
            NLmt5 = 5,

            /// <summary>
            /// 轴7负限位开关
            /// </summary>
            NLmt6 = 6,

            /// <summary>
            /// 轴8负限位开关
            /// </summary>
            NLmt7 = 7
            }

        /// <summary>
        /// 固高八轴运动卡通用输入信号枚举
        /// </summary>
        public enum DIGTS800
            {
            //******************
            //通用IO输入
            /// <summary>
            /// 通用IO输入位0
            /// </summary>
            EXI_0 = 0,

            /// <summary>
            /// 通用IO输入位1
            /// </summary>
            EXI_1 = 1,

            /// <summary>
            /// 通用IO输入位2
            /// </summary>
            EXI_2 = 2,

            /// <summary>
            /// 通用IO输入位3
            /// </summary>
            EXI_3 = 3,

            /// <summary>
            /// 通用IO输入位4
            /// </summary>
            EXI_4 = 4,

            /// <summary>
            /// 通用IO输入位5
            /// </summary>
            EXI_5 = 5,

            /// <summary>
            /// 通用IO输入位6
            /// </summary>
            EXI_6 = 6,

            /// <summary>
            /// 通用IO输入位7
            /// </summary>
            EXI_7 = 7,

            /// <summary>
            /// 通用IO输入位8
            /// </summary>
            EXI_8 = 8,

            /// <summary>
            /// 通用IO输入位9
            /// </summary>
            EXI_9 = 9,

            /// <summary>
            /// 通用IO输入位10
            /// </summary>
            EXI_10 = 10,

            /// <summary>
            /// 通用IO输入位11
            /// </summary>
            EXI_11 = 11,

            /// <summary>
            /// 通用IO输入位12
            /// </summary>
            EXI_12 = 12,

            /// <summary>
            /// 通用IO输入位13
            /// </summary>
            EXI_13 = 13,

            /// <summary>
            /// 通用IO输入位14
            /// </summary>
            EXI_14 = 14,

            /// <summary>
            /// 通用IO输入位15
            /// </summary>
            EXI_15 = 15
            }

        /// <summary>
        /// 固高八轴运动卡输出信号枚举
        /// </summary>
        public enum DOGTS800
            {

            //通用IO输出位
            /// <summary>
            /// 通用IO输出位0
            /// </summary>
            EXO_0 = 0,

            /// <summary>
            /// 通用IO输出位1
            /// </summary>
            EXO_1 = 1,

            /// <summary>
            /// 通用IO输出位2
            /// </summary>
            EXO_2 = 2,

            /// <summary>
            /// 通用IO输出位3
            /// </summary>
            EXO_3 = 3,

            /// <summary>
            /// 通用IO输出位4
            /// </summary>
            EXO_4 = 4,

            /// <summary>
            /// 通用IO输出位5
            /// </summary>
            EXO_5 = 5,

            /// <summary>
            /// 通用IO输出位6
            /// </summary>
            EXO_6 = 6,

            /// <summary>
            /// 通用IO输出位7
            /// </summary>
            EXO_7 = 7,

            /// <summary>
            /// 通用IO输出位8
            /// </summary>
            EXO_8 = 8,

            /// <summary>
            /// 通用IO输出位9
            /// </summary>
            EXO_9 = 9,

            /// <summary>
            /// 通用IO输出位10
            /// </summary>
            EXO_10 = 10,

            /// <summary>
            /// 通用IO输出位11
            /// </summary>
            EXO_11 = 11,

            /// <summary>
            /// 通用IO输出位12
            /// </summary>
            EXO_12 = 12,

            /// <summary>
            /// 通用IO输出位13
            /// </summary>
            EXO_13 = 13,

            /// <summary>
            /// 通用IO输出位14
            /// </summary>
            EXO_14 = 14,

            /// <summary>
            /// 通用IO输出位15
            /// </summary>
            EXO_15 = 15

            }

        #endregion

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }
        
        /// <summary>
        /// 运动控制器指令返回值
        /// </summary>
        public enum ErrorCode 
            {
            /// <summary>
            /// 指令执行成功
            /// </summary>
            Success=0,

            /// <summary>
            /// 指令执行错误: 检查当前指令的执行条件是否满足
            /// </summary>
            Error=1,

            /// <summary>
            /// license不支持: 如果需要此功能，请与生产厂商联系
            /// </summary>
            LicenseNotSupport=2,

            /// <summary>
            /// 指令参数错误: 检查当前指令输入参数的取值
            /// </summary>
            ParaError=7,

            /// <summary>
            /// 主机和运动控制器通讯失败
            /// 1. 是否正确安装运动控制器驱动程序
            /// 2. 检查运动控制器是否接插牢靠
            /// 3. 更换主机
            /// 4. 更换控制器
            /// 5. 运动控制器的金手指是否干净
            /// </summary>
            CommErr=-1,

            /// <summary>
            /// 打开控制器失败
            /// 1. 是否正确安装运动控制器驱动程序
            /// 2. 是否调用了2次GT_Open()指令
            /// 3. 其他程序是否已经打开运动控制器，或进程中是否还驻留着打开控制器的程序
            /// </summary>
            OpenControllerFailure=-6,

            /// <summary>
            /// 运动控制器没有响应
            /// 更换运动控制器
            /// </summary>
            ControllerNoResponse=-7
            
            }

        #endregion

        #region "运动卡函数代码"
        
        //创建固高运动控制器类的新实例，1代表卡1，2代表卡2...
        /// <summary>
        /// 创建固高运动控制器类的新实例，1代表卡1，2代表卡2...
        /// </summary>
        /// <param name="TargetCardNumber">固高运动控制器卡号[1,2...]</param>
        /// <param name="TotalAxises">固高运动控制器运动卡的总数轴[4或8]</param>
        /// <param name="GTSConfigFile">固高运动控制器配制文件完整文件名【存放在软件当前目录中】</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public GoogolTechGTSCard(short TargetCardNumber, short TotalAxises,
            string GTSConfigFile, string DLLPassword)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;
            try
                {
                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan")
                    || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;
                    }
                else
                    {
                    PasswordIsCorrect = false;
                    SuccessBuiltNew = false;
                    MessageBox.Show("Right Prohibited.\return\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                    }

                if (TargetCardNumber < 1)
                    {
                    MessageBox.Show("参数'TargetCardNumber'目标运动卡编号从1开始。", "参数错误");
                    ErrorMessage = "参数'TargetCardNumber'目标运动卡编号从1开始。";
                    return;
                    }
                else 
                    {
                    CardNumber = (short)(TargetCardNumber-1);
                    }

                if (!(TotalAxises == 4 || TotalAxises == 8))
                    {
                    MessageBox.Show("参数'TotalAxises'目标运动卡控制轴的数量是4或者8。", "参数错误");
                    ErrorMessage = "参数'TotalAxises'目标运动卡控制轴的数量是4或者8。";
                    return;
                    }
                else 
                    {
                    TotalAxisesOfCard = TotalAxises;
                    //Axises = new GoogolTechGTSAxisControl[TotalAxisesOfCard];
                    }

                //判断扩展文件名是否正确
                if (GTSConfigFile.ToUpper().IndexOf(".CFG") == 1) 
                    {
                    MessageBox.Show("参数'GTSConfigFile'不是固高运动卡配置文件，正确文件的扩展名是.CFG。", "参数错误");
                    ErrorMessage = "参数'GTSConfigFile'不是固高运动卡配置文件，正确文件的扩展名是.CFG。";
                    return;
                    }

                //判断文件是否存在
                if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() 
                    + "\\" + GTSConfigFile) == false) 
                    {
                    MessageBox.Show("参数'GTSConfigFile'固高运动卡配置文件"
                        + GTSConfigFile + "不存在于软件的当前文件夹" + 
                        System.IO.Directory.GetCurrentDirectory(), "参数错误");
                    ErrorMessage = "参数'GTSConfigFile'固高运动卡配置文件"
                        + GTSConfigFile + "不存在于软件的当前文件夹" + 
                        System.IO.Directory.GetCurrentDirectory();
                    return;
                    }
                
                //判断文件是否存在
                if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() 
                    + "\\" + "gts.dll") == false) 
                    {
                    MessageBox.Show("固高运动卡DLL文件 'gts.dll' 不存在，请将此文件复制到软件的当前文件夹中：" + 
                        System.IO.Directory.GetCurrentDirectory(), "错误");
                    ErrorMessage = "固高运动卡DLL文件 'gts.dll' 不存在，请将此文件复制到软件的当前文件夹中：" + 
                        System.IO.Directory.GetCurrentDirectory();
                    return;
                    }

                int Return = 0;
                //初始化固高运动控制器，每个程序开始时必须调用的，且只调用1次
                
                //打开运动控制器,参数必须为（0,0），不能修改.
                Return = GT_Open(CardNumber, 0, 0);
                
                //复位运动控制器
                Return += GT_Reset(CardNumber);
                
                //配置运动控制器.配置文件由mct2008软件生成.参数：1.配置文件的全路径.
                Return += GT_LoadConfig(CardNumber, GTSConfigFile);
                
                //清除各轴状态.参数：1.要清除状态轴的起始轴号.2.轴数量
                Return += GT_ClrSts(CardNumber, 1, TotalAxisesOfCard);
                
                //要执行且只执行一次
                Return += GT_HomeInit(CardNumber);

                if (Return != 0)
                    {
                    SuccessBuiltNew = false;
                    ErrorMessage = "初始化固高运动控制器失败, 错误代码：" + Return;
                    MessageBox.Show("初始化固高运动控制器失败, 错误代码：" + Return, "错误");
                    return;
                    }
                else 
                    {
                    InitialCardSucceed = true;
                    Axises = new GTSAxisControl[TotalAxisesOfCard];
                    for (int a = 1; a <= TotalAxises; a++)
                        {
                        Axises[a - 1] = new GTSAxisControl((short)a, "彭东南");
                        //Axises[a - 1] = new GTSAxisControl((short)a, "", 10000, 10);
                        }
                    SuccessBuiltNew = true;
                    }

                }
            catch (Exception ex)
                {
                SuccessBuiltNew = false;
                MessageBox.Show("创建类的实例时出现错误！\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }            
            }
        
        //【重载】创建固高运动控制器类的新实例，1代表卡1，2代表卡2...
        /// <summary>
        /// 【重载】创建固高运动控制器类的新实例，1代表卡1，2代表卡2...
        /// </summary>
        /// <param name="TargetCardNumber">固高运动控制器卡号[1,2...]</param>
        /// <param name="TotalAxises">固高运动控制器运动卡的总数轴[4或8]</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public GoogolTechGTSCard(short TargetCardNumber, short TotalAxises,
            string DLLPassword)
            {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;
            try
                {
                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan")
                    || (DLLPassword == "彭东南"))
                    {
                    PasswordIsCorrect = true;
                    }
                else
                    {
                    PasswordIsCorrect = false;
                    SuccessBuiltNew = false;
                    MessageBox.Show("Right Prohibited.\return\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                    }

                if (TargetCardNumber < 1)
                    {
                    MessageBox.Show("参数'TargetCardNumber'目标运动卡编号从1开始。", "参数错误");
                    ErrorMessage = "参数'TargetCardNumber'目标运动卡编号从1开始。";
                    return;
                    }
                else
                    {
                    CardNumber = (short)(TargetCardNumber - 1);
                    }

                if (!(TotalAxises == 4 || TotalAxises == 8))
                    {
                    MessageBox.Show("参数'TotalAxises'目标运动卡控制轴的数量是4或者8。", "参数错误");
                    ErrorMessage = "参数'TotalAxises'目标运动卡控制轴的数量是4或者8。";
                    return;
                    }
                else
                    {
                    TotalAxisesOfCard = TotalAxises;
                    //Axises = new GoogolTechGTSAxisControl[TotalAxisesOfCard];
                    TotalAxisesOfCard = TotalAxises;
                    }
                
                //判断文件是否存在
                if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory()
                    + "\\" + "gts.dll") == false)
                    {
                    MessageBox.Show("固高运动卡DLL文件 'gts.dll' 不存在，请将此文件复制到软件的当前文件夹中：" +
                        System.IO.Directory.GetCurrentDirectory(), "错误");
                    ErrorMessage = "固高运动卡DLL文件 'gts.dll' 不存在，请将此文件复制到软件的当前文件夹中：" +
                        System.IO.Directory.GetCurrentDirectory();
                    return;
                    }

                int Return = 0;
                //初始化固高运动控制器，每个程序开始时必须调用的，且只调用1次

                //打开运动控制器,参数必须为（0,0），不能修改.
                Return = GT_Open(CardNumber, 0, 0);

                //复位运动控制器
                Return += GT_Reset(CardNumber);

                //清除各轴状态.参数：1.要清除状态轴的起始轴号.2.轴数量
                Return += GT_ClrSts(CardNumber, 1, TotalAxisesOfCard);

                //要执行且只执行一次
                Return += GT_HomeInit(CardNumber);

                if (Return != 0)
                    {
                    SuccessBuiltNew = false;
                    ErrorMessage = "初始化固高运动控制器失败, 错误代码：" + Return;
                    MessageBox.Show("初始化固高运动控制器失败, 错误代码：" + Return, "错误");
                    return;
                    }
                else
                    {
                    InitialCardSucceed = true;
                    Axises = new GTSAxisControl[TotalAxisesOfCard];
                    for (int a = 1; a <= TotalAxises; a++)
                        {
                        Axises[a - 1] = new GTSAxisControl((short)a, "彭东南");
                        //Axises[a - 1] = new GTSAxisControl((short)a, "", 10000, 10);
                        }
                    SuccessBuiltNew = true;
                    }

                }
            catch (Exception ex)
                {
                SuccessBuiltNew = false;
                MessageBox.Show("创建类的实例时出现错误！\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }
            }

        //检查固高运动卡指令执行后的返回值对应的问题及其相应解决办法
        /// <summary>
        /// 检查固高运动卡指令执行后的返回值对应的问题及其相应解决办法
        /// </summary>
        /// <param name="ReturnCode">固高运动卡指令执行后的返回值</param>
        /// <returns></returns>
        public string ErrResult(int ReturnCode)
            {
            string TempStr = "";
            try
                {
                //        运动控制器指令返回值定义
                //返回值           意义                        处理方法
                //---------------------------------------------------
                //0                指令执行成功                无
                //1                指令执行错误                1. 检查当前指令的执行条件是否满足
                //2                license不支持               1. 如果需要此功能，请与生产厂商联系。
                //7                指令参数错误                1．检查当前指令输入参数的取值
                //-1               主机和运动控制器通讯失败    1. 是否正确安装运动控制器驱动程序     2. 检查运动控制器是否接插牢靠  3. 更换主机  4. 更换控制器  5. 运动控制器的金手指是否干净
                //-6               打开控制器失败              1. 是否正确安装运动控制器驱动程序     2. 是否调用了2次GT_Open()指令  3. 其他程序是否已经打开运动控制器，或进程中是否还驻留着打开控制器的程序
                //-7               运动控制器没有响应          1. 更换运动控制器

                switch (ReturnCode) 
                    {
                    case 0:
                        TempStr = "指令返回值：0， 表示指令执行成功";
                        break;
                        
                    case 1:
                        TempStr="指令返回值：1， 表示指令执行错误，检查当前指令的执行条件是否满足";
                        break;

                    case 2:
                        TempStr=ErrorMessage = "指令返回值：2， 表示license不支持，如果需要此功能，请与生产厂商联系";
                        break;

                    case 7:
                        TempStr=ErrorMessage = "指令返回值：7， 表示指令参数错误，检查当前指令输入参数的取值";
                        break;

                    case -1:
                        TempStr="指令返回值：-1， 表示主机和运动控制器通讯失败，请检查：1、是否正确安装运动控制器驱动程序；2、检查运动控制器是否接插牢靠；3、更换主机；4、更换控制器";
                        break;

                    case -6:
                        TempStr="指令返回值：-6， 表示打开控制器失败，请检查：1、是否正确安装运动控制器驱动程序；2、是否调用了2次GT_Open指令；3、其他程序是否已经打开运动控制器";
                        break;

                    case -7:
                        TempStr="指令返回值：-7， 表示运动控制器没有响应，更换运动控制器";
                        break;

                    default:
                        TempStr="指令返回值：其他， 表示未知错误";
                        break;                    
                    }
                return TempStr;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return TempStr;
                }
            }

        //清除所有轴的状态: 驱动器报警标志、跟随误差越限标志、限位触发标志
        /// <summary>
        /// 清除所有轴的状态: 驱动器报警标志、跟随误差越限标志、限位触发标志
        /// </summary>
        /// <returns></returns>
        public bool ClearAllAxisStatus()
            {
            try
                {
                int Return = 0;
                if (TotalAxisesOfCard == 4)
                    {
                    Return = GT_ClrSts(CardNumber, 1, 4);
                    }
                else 
                    {
                    Return = GT_ClrSts(CardNumber, 1, 8);
                    }

                if (Return != 0)
                    {
                    ErrorMessage = "清除轴轴状态指令执行失败，请检查运动卡或指令的参数";
                    return false;
                    }
                else 
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //清除指定轴的状态: 驱动器报警标志、跟随误差越限标志、限位触发标志
        /// <summary>
        /// 清除指定轴的状态: 驱动器报警标志、跟随误差越限标志、限位触发标志
        /// </summary>
        /// <param name="TargetAxis">指定轴号【1~运动卡支持的最大轴数量】</param>
        /// <returns></returns>
        public bool ClearAxisStatus(short TargetAxis)
            {
            try
                {
                if (TargetAxis > TotalAxisesOfCard || TargetAxis < 1) 
                    {
                    ErrorMessage = "指定的轴号超出固高运动控制运动卡支持的轴数，请修改参数";
                    return false;
                    }

                int Return = 0;
                Return = GT_ClrSts(CardNumber, TargetAxis, 1);
                if (Return != 0)
                    {
                    ErrorMessage = "清除轴状态指令执行失败，请检查运动卡或指令的参数";
                    return false;
                    }
                else 
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //取消，避免与其它卡冲突--(重载)清除目标轴报警
        /// <summary>
        /// (重载)清除目标轴报警
        /// </summary>
        /// <param name="TargetCard">目标卡号</param>
        /// <param name="TargetAxis">目标轴号</param>
        /// <param name="SignalEdge">输出电平: 默认1表示高电平，0表示低电平</param>
        /// <returns></returns>
        private bool ClearWarning(short TargetCard, short TargetAxis, 
            EdgeForInputOutputSignals SignalEdge = EdgeForInputOutputSignals.RisingEdge)
            {
            try
                {
                if (TargetCard < 1 || TargetCard > 16) 
                    {
                    ErrorMessage = "参数'TargetCard'目标运动卡编号不能小于1或者大于16";
                    return false;
                    }

                if (TargetAxis < 1 || TargetAxis > 8) 
                    {
                    ErrorMessage = "参数'TargetAxis'目标轴编号不能小于1或者大于8，根据实际运动卡支持的轴数量而定";
                    return false;
                    }

                //GT_SetDoBit(short doType,short doIndex,short value)
                //doType -- 指定数字IO类型
                //MC_ENABLE(该宏定义为10) 驱动器使能
                //MC_CLEAR(该宏定义为11) 报警清除
                //MC_GPO(该宏定义为12) 通用输出
                //doIndex -- 输出IO的索引
                //取值范围：
                //doType=MC_ENABLE时：[1,8]
                //doType=MC_CLEAR时：[1,8]
                //doType=MC_GPO时：[1,16]
                //value - 设置数字IO输出电平
                //默认情况下，1表示高电平，0表示低电平

                int Return = 0;
                short OutEdge;
                if (SignalEdge == EdgeForInputOutputSignals.RisingEdge)
                    {
                    OutEdge = 1;
                    }
                else 
                    {
                    OutEdge = 0;
                    }

                Return = GT_SetDoBit((short)(TargetCard - 1), MC_CLEAR, TargetAxis, OutEdge);
                if (Return != 0)
                    {
                    ErrorMessage = "清除轴报警指令执行失败，请检查运动卡或指令的参数";
                    return false;
                    }
                else 
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //清除目标轴报警
        /// <summary>
        /// 清除目标轴报警
        /// </summary>
        /// <param name="TargetAxis">目标轴号</param>
        /// <param name="SignalEdge">输出电平: 默认1表示高电平，0表示低电平</param>
        /// <returns></returns>
        public bool ClearWarning(short TargetAxis,
            EdgeForInputOutputSignals SignalEdge = EdgeForInputOutputSignals.RisingEdge)
            {
            try
                {
                if (TargetAxis < 1 || TargetAxis > TotalAxisesOfCard)
                    {
                    ErrorMessage = "参数'TargetAxis'目标轴编号不能小于1或者大于" + TotalAxisesOfCard + 
                        "，根据实际运动卡支持的轴数量而定";
                    return false;
                    }

                //GT_SetDoBit(short doType,short doIndex,short value)
                //doType -- 指定数字IO类型
                //MC_ENABLE(该宏定义为10) 驱动器使能
                //MC_CLEAR(该宏定义为11) 报警清除
                //MC_GPO(该宏定义为12) 通用输出
                //doIndex -- 输出IO的索引
                //取值范围：
                //doType=MC_ENABLE时：[1,8]
                //doType=MC_CLEAR时：[1,8]
                //doType=MC_GPO时：[1,16]
                //value - 设置数字IO输出电平
                //默认情况下，1表示高电平，0表示低电平

                int Return = 0;
                short OutEdge;
                if (SignalEdge == EdgeForInputOutputSignals.RisingEdge)
                    {
                    OutEdge = 1;
                    }
                else
                    {
                    OutEdge = 0;
                    }

                Return = GT_SetDoBit(CardNumber, MC_CLEAR, TargetAxis, OutEdge);
                if (Return != 0)
                    {
                    ErrorMessage = "清除轴报警指令执行失败，请检查运动卡或指令的参数";
                    return false;
                    }
                else
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //通用输出复位指令
        /// <summary>
        /// 通用输出复位指令
        /// </summary>
        /// <param name="TargetOutputBit">通用输出:数1~16代表输出点EXO0~EXO15</param>
        /// <returns></returns>
        public bool ResetDOBit(short TargetOutputBit)
            {
            try
                {
                if (TargetOutputBit < 0 || TargetOutputBit > 15) 
                    {
                    ErrorMessage = "输出位超出范围（0~15），请修改参数'TargetOutputBit'";
                    return false;
                    }

                int Return = 0;
                Return = GT_SetDoBit(CardNumber, MC_GPO, (short)(TargetOutputBit + 1), 0);
                if (Return != 0)
                    {
                    ErrorMessage = "输出指令执行失败，请检查运动卡或指令的参数";
                    return false;
                    }
                else
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //取消，避免与其它卡冲突--【重载】通用输出复位指令
        /// <summary>
        /// 【重载】通用输出复位指令
        /// </summary>
        /// <param name="TargetCard">目标运动卡号</param>
        /// <param name="TargetOutputBit">通用输出:数1~16代表输出点EXO0~EXO15</param>
        /// <returns></returns>
        private bool ResetDOBit(ushort TargetCard, short TargetOutputBit)
            {
            try
                {
                if (TargetCard < 1) 
                    {
                    ErrorMessage = "参数'TargetCard'目标运动卡号不能小于1";
                    return false;
                    }

                if (TargetOutputBit < 0 || TargetOutputBit > 15)
                    {
                    ErrorMessage = "输出位超出范围（0~15），请修改参数'TargetOutputBit'";
                    return false;
                    }

                int Return = 0;
                Return = GT_SetDoBit((short)(TargetCard-1), MC_GPO, (short)(TargetOutputBit + 1), 0);
                if (Return != 0)
                    {
                    ErrorMessage = "输出指令执行失败，请检查运动卡或指令的参数";
                    return false;
                    }
                else
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //通用输出置位指令
        /// <summary>
        /// 通用输出置位指令
        /// </summary>
        /// <param name="TargetOutputBit">通用输出:数1~16代表输出点EXO0~EXO15</param>
        /// <returns></returns>
        public bool SetDOBit(short TargetOutputBit)
            {
            try
                {
                if (TargetOutputBit < 0 || TargetOutputBit > 15)
                    {
                    ErrorMessage = "通用输出位超出范围（0~15），请修改参数'TargetOutputBit'";
                    return false;
                    }

                int Return = 0;
                Return = GT_SetDoBit(CardNumber, MC_GPO, (short)(TargetOutputBit + 1), 1);
                if (Return != 0)
                    {
                    ErrorMessage = "通用输出指令执行失败，请检查运动卡或指令的参数";
                    return false;
                    }
                else
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //取消，避免与其它卡冲突--【重载】通用输出置位指令
        /// <summary>
        /// 【重载】通用输出置位指令
        /// </summary>
        /// <param name="TargetCard">目标运动卡号</param>
        /// <param name="TargetOutputBit">通用输出:数1~16代表输出点EXO0~EXO15</param>
        /// <returns></returns>
        private bool SetDOBit(ushort TargetCard, short TargetOutputBit)
            {
            try
                {
                if (TargetCard < 1)
                    {
                    ErrorMessage = "参数'TargetCard'目标运动卡号不能小于1";
                    return false;
                    }

                if (TargetOutputBit < 0 || TargetOutputBit > 15)
                    {
                    ErrorMessage = "通用输出位超出范围（0~15），请修改参数'TargetOutputBit'";
                    return false;
                    }

                int Return = 0;
                Return = GT_SetDoBit((short)(TargetCard - 1), MC_GPO, (short)(TargetOutputBit + 1), 1);
                if (Return != 0)
                    {
                    ErrorMessage = "通用输出指令执行失败，请检查运动卡或指令的参数";
                    return false;
                    }
                else
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //取消，避免与其它卡冲突--【重载】将轴的驱动方式设为双脉冲（CW+CCW）
        /// <summary>
        /// 【重载】将轴的驱动方式设为双脉冲（CW+CCW）
        /// </summary>
        /// <param name="TargetCard">目标运动卡号</param>
        /// <param name="TargetAxis">目标轴号</param>
        /// <returns></returns>
        private bool CWPlusCCWDriving(short TargetCard, short TargetAxis)
            {
            try
                {
                if (TargetCard < 1)
                    {
                    ErrorMessage = "参数'TargetCard'目标运动卡号不能小于1";
                    return false;
                    }

                if (TargetAxis < 1 || TargetAxis > 8)
                    {
                    ErrorMessage = "参数'TargetAxis'目标轴编号不能小于1或者大于8，根据实际运动卡支持的轴数量而定";
                    return false;
                    }

                int Return = 0;
                Return = GT_Stop(TargetCard, 2 << (TargetAxis - 1), 0);
                Return += GT_AxisOff(TargetCard, TargetAxis);
                Return += GT_StepPulse(TargetCard, TargetAxis);
                if (Return != 0)
                    {
                    ErrorMessage = "指令执行失败：未能将卡号 " + TargetCard
                    + " 中的 " + TargetAxis + "轴的驱动方式设为双脉冲（CW+CCW）";
                    return false;
                    }
                else 
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //将轴的驱动方式设为双脉冲（CW+CCW）
        /// <summary>
        /// 将轴的驱动方式设为双脉冲（CW+CCW）
        /// </summary>
        /// <param name="TargetAxis">目标轴号</param>
        /// <returns></returns>
        public bool CWPlusCCWDriving(short TargetAxis)
            {
            try
                {
                if (TargetAxis < 1 || TargetAxis > TotalAxisesOfCard)
                    {
                    ErrorMessage = "参数'TargetAxis'目标轴编号不能小于1或者大于 " 
                        + TotalAxisesOfCard;
                    return false;
                    }

                int Return = 0;
                Return = GT_Stop(CardNumber, 2 << (TargetAxis - 1), 0);
                Return += GT_AxisOff(CardNumber, TargetAxis);
                Return += GT_StepPulse(CardNumber, TargetAxis);
                if (Return != 0)
                    {
                    ErrorMessage = "指令执行失败：未能将卡号 " + CardNumber
                    + " 中的 " + TargetAxis + "轴的驱动方式设为双脉冲（CW+CCW）";
                    return false;
                    }
                else
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //将轴的驱动方式设为脉冲加方向（STEP+DIR）
        /// <summary>
        /// 将轴的驱动方式设为脉冲加方向（STEP+DIR）
        /// </summary>
        /// <param name="TargetAxis">目标轴号</param>
        /// <returns></returns>
        public bool PulsePlusDirectionDriving(short TargetAxis)
            {
            try
                {
                if (TargetAxis < 1 || TargetAxis > TotalAxisesOfCard)
                    {
                    ErrorMessage = "参数'TargetAxis'目标轴编号不能小于1或者大于 "
                        + TotalAxisesOfCard;
                    return false;
                    }

                int Return = 0;
                Return = GT_Stop(CardNumber, 2 << (TargetAxis - 1), 0);
                Return += GT_AxisOff(CardNumber, TargetAxis);
                Return += GT_StepDir(CardNumber, TargetAxis);
                if (Return != 0)
                    {
                    ErrorMessage = "指令执行失败：未能将卡号 " + CardNumber
                    + " 中的 " + TargetAxis + "轴的驱动方式设为脉冲加方向";
                    return false;
                    }
                else
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //取消，避免与其它卡冲突--【重载】将轴的驱动方式设为脉冲加方向（STEP+DIR）
        /// <summary>
        /// 【重载】将轴的驱动方式设为脉冲加方向（STEP+DIR）
        /// </summary>
        /// <param name="TargetCard">目标运动卡号</param>
        /// <param name="TargetAxis">目标轴号</param>
        /// <returns></returns>
        private bool PulsePlusDirectionDriving(short TargetCard, short TargetAxis)
            {
            try
                {
                if (TargetCard < 1)
                    {
                    ErrorMessage = "参数'TargetCard'目标运动卡号不能小于1";
                    return false;
                    }

                if (TargetAxis < 1 || TargetAxis > 8)
                    {
                    ErrorMessage = "参数'TargetAxis'目标轴编号不能小于1或者大于8，根据实际运动卡支持的轴数量而定";
                    return false;
                    }

                int Return = 0;
                Return = GT_Stop(TargetCard, 2 << (TargetAxis - 1), 0);
                Return += GT_AxisOff(TargetCard, TargetAxis);
                Return += GT_StepDir(TargetCard, TargetAxis);
                if (Return != 0)
                    {
                    ErrorMessage = "指令执行失败：未能将卡号 " + TargetCard
                    + " 中的 " + TargetAxis + "轴的驱动方式设为脉冲加方向";
                    return false;
                    }
                else
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //DAO模拟量输出
        /// <summary>
        /// DAO模拟量输出
        /// </summary>
        /// <param name="DAOChannel">DAO模拟量输出通道号【1~8】</param>
        /// <param name="DAOValue">输出电压
        /// -32768对应-10V；32767对应+10V</param>
        /// <returns></returns>
        public bool DAO(short DAOChannel, short DAOValue)
            {
            try
                {
                if (DAOChannel < 1 || DAOChannel > 8) 
                    {
                    ErrorMessage = "参数'DAOChannel'DAO模拟量输出通道号的值应该是1~8";
                    return false;
                    }

                int Return = 0;
                Return = GT_SetDac(CardNumber, DAOChannel, ref DAOValue, 1);
                if (Return != 0)
                    {
                    ErrorMessage = "模拟量输出指令执行失败";
                    return false;
                    }
                else 
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //取消，避免与其它卡冲突--【重载】DAO模拟量输出
        /// <summary>
        /// 【重载】DAO模拟量输出
        /// </summary>
        /// <param name="TargetCard">目标运动卡号</param>
        /// <param name="DAOChannel">DAO模拟量输出通道号【1~8】</param>
        /// <param name="DAOValue">输出电压
        /// -32768对应-10V；32767对应+10V</param>
        /// <returns></returns>
        private bool DAO(short TargetCard, short DAOChannel, short DAOValue)
            {
            try
                {
                if (TargetCard < 1)
                    {
                    ErrorMessage = "参数'TargetCard'目标运动卡号不能小于1";
                    return false;
                    }

                if (DAOChannel < 1 || DAOChannel > 8)
                    {
                    ErrorMessage = "参数'DAOChannel'DAO模拟量输出通道号的值应该是1~8";
                    return false;
                    }

                int Return = 0;
                Return = GT_SetDac(TargetCard, DAOChannel, ref DAOValue, 1);
                if (Return != 0)
                    {
                    ErrorMessage = "模拟量输出指令执行失败";
                    return false;
                    }
                else
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //ADI模拟量输入[获取电压值对应的数值]
        /// <summary>
        /// ADI模拟量输入[获取电压值对应的数值]
        /// </summary>
        /// <param name="ADIChannel">DAO模拟量输入通道号【1~8】</param>
        /// <param name="ADIValue">输入电压
        /// -32768对应-10V；32767对应+10V</param>
        /// <returns></returns>
        public bool ADI(short ADIChannel, out short ADIValue)
            {
            try
                {
                if (ADIChannel < 1 || ADIChannel > 8)
                    {
                    ErrorMessage = "参数'ADIChannel'ADI模拟量输入通道号的值应该是1~8";
                    ADIValue = 0;
                    return false;
                    }

                int Return = 0;
                uint CardClock = 0;
                Return = GT_GetAdcValue(CardNumber, ADIChannel, out ADIValue, (short)1, out CardClock);
                if (Return != 0)
                    {
                    ErrorMessage = "模拟量输入指令[获取电压值对应的数值]执行失败";
                    return false;
                    }
                else
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                ADIValue = 0;
                return false;
                }
            }

        //【重载】ADI模拟量输入[获取电压值对应的数值]
        /// <summary>
        /// 【重载】ADI模拟量输入[获取电压值对应的数值]
        /// </summary>
        /// <param name="TargetCard">目标运动卡号</param>
        /// <param name="ADIChannel">DAO模拟量输入通道号【1~8】</param>
        /// <param name="ADIValue">输入电压
        /// -32768对应-10V；32767对应+10V</param>
        /// <returns></returns>
        public bool ADI(short TargetCard, short ADIChannel, out short ADIValue)
            {
            try
                {
                if (TargetCard < 1)
                    {
                    ErrorMessage = "参数'TargetCard'目标运动卡号不能小于1";
                    ADIValue = 0;
                    return false;
                    }

                if (ADIChannel < 1 || ADIChannel > 8)
                    {
                    ErrorMessage = "参数'ADIChannel'ADI模拟量输入通道号的值应该是1~8";
                    ADIValue = 0;
                    return false;
                    }

                int Return = 0;
                uint CardClock = 0;
                Return = GT_GetAdcValue(TargetCard, ADIChannel, out ADIValue, (short)1, out CardClock);
                if (Return != 0)
                    {
                    ErrorMessage = "模拟量输入指令[获取电压值对应的数值]执行失败";
                    return false;
                    }
                else
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                ADIValue = 0;
                return false;
                }
            }

        //ADI模拟量输入[获取电压值]
        /// <summary>
        /// ADI模拟量输入[获取电压值]
        /// </summary>
        /// <param name="ADIChannel">DAO模拟量输入通道号【1~8】</param>
        /// <param name="ADIValue">输入电压： -10V~+10V</param>
        /// <returns></returns>
        public bool ADIVolt(short ADIChannel, out double ADIValue)
            {
            try
                {
                if (ADIChannel < 1 || ADIChannel > 8)
                    {
                    ErrorMessage = "参数'ADIChannel'ADI模拟量输入通道号的值应该是1~8";
                    ADIValue = 0;
                    return false;
                    }

                int Return = 0;
                uint CardClock = 0;
                Return = GT_GetAdc(CardNumber, ADIChannel, out ADIValue, (short)1, out CardClock);
                if (Return != 0)
                    {
                    ErrorMessage = "模拟量输入指令[获取电压值]执行失败";
                    return false;
                    }
                else
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                ADIValue = 0;
                return false;
                }
            }

        //取消，避免与其它卡冲突--【重载】ADI模拟量输入[获取电压值]
        /// <summary>
        /// 【重载】ADI模拟量输入[获取电压值]
        /// </summary>
        /// <param name="TargetCard">目标运动卡号</param>
        /// <param name="ADIChannel">DAO模拟量输入通道号【1~8】</param>
        /// <param name="ADIVoltValue">输入电压： -10V~+10V</param>
        /// <returns></returns>
        private bool ADIVolt(short TargetCard, short ADIChannel, out double ADIVoltValue)
            {
            try
                {
                if (TargetCard < 1)
                    {
                    ErrorMessage = "参数'TargetCard'目标运动卡号不能小于1";
                    ADIVoltValue = 0;
                    return false;
                    }

                if (ADIChannel < 1 || ADIChannel > 8)
                    {
                    ErrorMessage = "参数'ADIChannel'ADI模拟量输入通道号的值应该是1~8";
                    ADIVoltValue = 0;
                    return false;
                    }

                int Return = 0;
                uint CardClock = 0;
                Return = GT_GetAdc(TargetCard, ADIChannel, out ADIVoltValue, (short)1, out CardClock);
                if (Return != 0)
                    {
                    ErrorMessage = "模拟量输入指令[获取电压值]执行失败";
                    return false;
                    }
                else
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                ADIVoltValue = 0;
                return false;
                }
            }

        //更新运动卡数字IO输出【4/8轴共用】
        /// <summary>
        /// 更新运动卡数字IO输出【4/8轴共用】
        /// </summary>
        /// <param name="OutputStatus">通用输出:数组0~15代表输出点EXO0~EXO15</param>
        /// <returns></returns>
        public bool UpdateOutput(bool[] OutputStatus)
            {
            try
                {
                if (OutputStatus.Length != 16) 
                    {
                    ErrorMessage = "参数'OutputStatus'的数组长度不等于16";
                    return false;
                    }

                int Return = 0;
                for (int a = 0; a < OutputStatus.Length; a++) 
                    {
                    if (OutputStatus[a] == true)
                        {
                        Return += GT_SetDoBit(CardNumber, MC_GPO, (short)(a + 1), 1);
                        }
                    else 
                        {
                        Return += GT_SetDoBit(CardNumber, MC_GPO, (short)(a + 1), 0);
                        }
                    }

                if (Return != 0)
                    {
                    return false;
                    }
                else 
                    {
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //读取运动卡数字DO输出状态【4/8轴共用】
        /// <summary>
        /// 读取运动卡数字DO输出状态【4/8轴共用】
        /// </summary>
        /// <returns></returns>
        public bool[] ReadDOStatus()
            {
            bool[] Status = new bool[16];
            try
                {
                int OutputStatus=0;
                int Return = 0;

                Return = GT_GetDo(CardNumber, MC_GPO, out OutputStatus);
                for (int a = 0; a <= 15; a++) 
                    {
                    if ((OutputStatus >> a) == 1)
                        {
                        Status[a] = true;
                        }
                    else 
                        {
                        Status[a] = false;
                        }
                    }

                return Status;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return Status;
                }
            }

        //取消，避免与其它卡冲突--【重载】更新运动卡输出【4/8轴共用】
        /// <summary>
        /// 【重载】更新运动卡输出【4/8轴共用】
        /// </summary>
        /// <param name="TargetCard">目标运动卡号</param>
        /// <param name="OutputStatus">通用输出:数组0~15代表输出点EXO0~EXO15</param>
        /// <returns></returns>
        private bool UpdateOutput(short TargetCard, bool[] OutputStatus)
            {
            try
                {
                if (TargetCard < 1)
                    {
                    ErrorMessage = "参数'TargetCard'目标运动卡号不能小于1";
                    return false;
                    }

                if (OutputStatus.Length != 16)
                    {
                    ErrorMessage = "参数'OutputStatus'的数组长度不等于16";
                    return false;
                    }

                int Return = 0;
                for (int a = 0; a < OutputStatus.Length; a++)
                    {
                    if (OutputStatus[a] == true)
                        {
                        Return += GT_SetDoBit(TargetCard, MC_GPO, (short)(a + 1), 1);
                        }
                    else
                        {
                        Return += GT_SetDoBit(TargetCard, MC_GPO, (short)(a + 1), 0);
                        }
                    }

                if (Return != 0)
                    {
                    ErrorMessage = "更新运动卡输出出错";
                    return false;
                    }
                else
                    {
                    ErrorMessage = "更新运动卡输出成功";
                    return true;
                    }
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //【作废】返回运动卡输入信号数组
        /// <summary>
        /// 【作废】返回运动卡输入信号数组: 
        /// [原点信号--4轴(0~3) | 8轴(0~7);
        /// 正限位--4轴(偶数4~10) | 8轴(偶数8~22);
        /// 负限位--4轴(奇数5~11) | 8轴(奇数9~23);
        /// 通用输入--4轴(12~27) | 8轴(24~39)]
        /// </summary>
        /// <returns></returns>
        private bool[] UpdateInputSignals()
            {
            int HomeValue = 0, PositiveLMTValue = 0, NegativeLMTValue = 0, DIValue = 0;
            bool[] DI;
            try
                {
                if (TotalAxisesOfCard == 4)
                    {
                    DI = new bool[28];
                    }
                else 
                    {
                    DI = new bool[40];
                    }

                int Return = 0;
                //if (TotalAxisesOfCard == 4)
                //    {
                //    //四轴运动卡输入信号
                //    }
                //else
                //    {
                //    //八轴运动卡输入信号
                //    }

                //原点信号
                Return += GT_GetDi(CardNumber, MC_HOME, out HomeValue);

                //正限位
                Return += GT_GetDi(CardNumber, MC_LIMIT_POSITIVE, out PositiveLMTValue);

                //负限位
                Return += GT_GetDi(CardNumber, MC_LIMIT_NEGATIVE, out NegativeLMTValue);

                //通用输入
                Return += GT_GetDi(CardNumber, MC_GPI, out DIValue);

                for (int AxisNo = 1; AxisNo <= TotalAxisesOfCard; AxisNo++) 
                    {
                    ////原点信号
                    //Return += GT_GetDi(CardNumber, MC_HOME, out HomeValue);
                    if ((HomeValue >> (AxisNo - 1) & 1) == 1)
                        {
                        DI[AxisNo - 1] = true;
                        }
                    else 
                        {
                        DI[AxisNo - 1] = false;
                        }

                    ////正限位
                    //Return += GT_GetDi(CardNumber, MC_LIMIT_POSITIVE, out PositiveLMTValue);
                    if ((PositiveLMTValue >> (AxisNo - 1) & 1) == 1)
                        {
                        DI[TotalAxisesOfCard + (AxisNo - 1) * 2] = true;
                        }
                    else 
                        {
                        DI[TotalAxisesOfCard + (AxisNo - 1) * 2] = false;
                        }

                    ////负限位
                    //Return += GT_GetDi(CardNumber, MC_LIMIT_NEGATIVE, out NegativeLMTValue);
                    if ((NegativeLMTValue >> (AxisNo - 1) & 1) == 1)
                        {
                        DI[TotalAxisesOfCard + (AxisNo - 1) * 2 + 1] = true;
                        }
                    else 
                        {
                        DI[TotalAxisesOfCard + (AxisNo - 1) * 2 + 1] = false;
                        }
                    
                    ////通用输入
                    //Return += GT_GetDi(CardNumber, MC_GPI, out DIValue);
                    for (int a = (AxisNo * 3); a <= AxisNo * 3 + 15; a++) 
                        {
                        if ((DIValue >> (a - AxisNo * 3) & 1) == 1)
                            {
                            DI[a] = true;
                            }
                        else 
                            {
                            DI[a] = false;
                            }                        
                        }
                    }
                
                return DI;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }

        //更新运动卡原点信号状态
        /// <summary>
        /// 更新运动卡原点信号状态
        /// </summary>
        /// <returns>返回运动卡原点信号数组:[4轴(0~3) | 8轴(0~7)</returns>
        public bool[] UpdateHomeSignal()
            {
            int HomeValue = 0;
            bool[] HomeDI;
            try
                {
                if (TotalAxisesOfCard == 4)
                    {
                    HomeDI = new bool[4];
                    }
                else
                    {
                    HomeDI = new bool[8];
                    }

                int Return = 0;
                //if (TotalAxisesOfCard == 4)
                //    {
                //    //四轴运动卡输入信号
                //    }
                //else
                //    {
                //    //八轴运动卡输入信号
                //    }

                //原点信号
                Return += GT_GetDi(CardNumber, MC_HOME, out HomeValue);
                for (int AxisNo = 1; AxisNo <= TotalAxisesOfCard; AxisNo++)
                    {
                    if ((HomeValue >> (AxisNo - 1) & 1) == 1)
                        {
                        HomeDI[AxisNo - 1] = true;
                        }
                    else
                        {
                        HomeDI[AxisNo - 1] = false;
                        }
                    }
                return HomeDI;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }
        
        //取消，避免与其它卡冲突--【重载】更新运动卡原点信号状态
        /// <summary>
        /// 【重载】更新运动卡原点信号状态
        /// </summary>
        /// <param name="TargetCard">目标运动卡号</param>
        /// <param name="TotalAxises">固高运动控制器运动卡的总数轴[4或8]</param>
        /// <returns>返回运动卡原点信号数组:[4轴(0~3) | 8轴(0~7)</returns>
        private bool[] UpdateHomeSignal(short TargetCard, short TotalAxises)
            {
            int HomeValue = 0;
            bool[] HomeDI;
            try
                {
                if (TargetCard < 1)
                    {
                    ErrorMessage = "参数'TargetCard'目标运动卡号不能小于1";
                    return null;
                    }

                if (!(TotalAxises == 4 || TotalAxises == 8))
                    {
                    //MessageBox.Show("参数'TotalAxises'目标运动卡控制轴的数量是4或者8。", "参数错误");
                    ErrorMessage = "参数'TotalAxises'目标运动卡控制轴的数量是4或者8。";
                    return null;
                    }

                if (TotalAxises == 4)
                    {
                    HomeDI = new bool[4];
                    }
                else
                    {
                    HomeDI = new bool[8];
                    }

                int Return = 0;
                //if (TotalAxises == 4)
                //    {
                //    //四轴运动卡输入信号
                //    }
                //else
                //    {
                //    //八轴运动卡输入信号
                //    }

                //原点信号
                Return += GT_GetDi(TargetCard, MC_HOME, out HomeValue);
                for (int AxisNo = 1; AxisNo <= TotalAxises; AxisNo++)
                    {
                    if ((HomeValue >> (AxisNo - 1) & 1) == 1)
                        {
                        HomeDI[AxisNo - 1] = true;
                        }
                    else
                        {
                        HomeDI[AxisNo - 1] = false;
                        }
                    }
                return HomeDI;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }

        //更新运动卡正限位信号状态
        /// <summary>
        /// 更新运动卡正限位信号状态
        /// </summary>
        /// <returns>返回运动卡正限位信号数组:[4轴(0~3) | 8轴(0~7)</returns>
        public bool[] UpdatePositiveLMTSignal()
            {
            int PositiveLMTValue = 0;
            bool[] PLMTDI;
            try
                {
                if (TotalAxisesOfCard == 4)
                    {
                    PLMTDI = new bool[4];
                    }
                else
                    {
                    PLMTDI = new bool[8];
                    }

                int Return = 0;
                //if (TotalAxisesOfCard == 4)
                //    {
                //    //四轴运动卡输入信号
                //    }
                //else
                //    {
                //    //八轴运动卡输入信号
                //    }
                
                //正限位
                Return += GT_GetDi(CardNumber, MC_LIMIT_POSITIVE, out PositiveLMTValue);
                for (int AxisNo = 1; AxisNo <= TotalAxisesOfCard; AxisNo++)
                    {
                    if ((PositiveLMTValue >> (AxisNo - 1) & 1) == 1)
                        {
                        PLMTDI[AxisNo - 1] = true;
                        }
                    else
                        {
                        PLMTDI[AxisNo - 1] = false;
                        }
                    }

                return PLMTDI;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }

        //取消，避免与其它卡冲突--【重载】更新运动卡正限位信号状态
        /// <summary>
        /// //【重载】更新运动卡正限位信号状态
        /// </summary>
        /// <param name="TargetCard">目标运动卡号</param>
        /// <param name="TotalAxises">固高运动控制器运动卡的总数轴[4或8]</param>
        /// <returns>返回运动卡正限位信号数组:[4轴(0~3) | 8轴(0~7)</returns>
        private bool[] UpdatePositiveLMTSignal(short TargetCard, short TotalAxises)
            {
            int PositiveLMTValue = 0;
            bool[] PLMTDI;
            try
                {
                if (TargetCard < 1)
                    {
                    ErrorMessage = "参数'TargetCard'目标运动卡号不能小于1";
                    return null;
                    }

                if (!(TotalAxises == 4 || TotalAxises == 8))
                    {
                    //MessageBox.Show("参数'TotalAxises'目标运动卡控制轴的数量是4或者8。", "参数错误");
                    ErrorMessage = "参数'TotalAxises'目标运动卡控制轴的数量是4或者8。";
                    return null;
                    }

                if (TotalAxises == 4)
                    {
                    PLMTDI = new bool[4];
                    }
                else
                    {
                    PLMTDI = new bool[8];
                    }

                int Return = 0;
                //if (TotalAxises == 4)
                //    {
                //    //四轴运动卡输入信号
                //    }
                //else
                //    {
                //    //八轴运动卡输入信号
                //    }

                //正限位
                Return += GT_GetDi(TargetCard, MC_LIMIT_POSITIVE, out PositiveLMTValue);
                for (int AxisNo = 1; AxisNo <= TotalAxises; AxisNo++)
                    {
                    if ((PositiveLMTValue >> (AxisNo - 1) & 1) == 1)
                        {
                        PLMTDI[AxisNo - 1] = true;
                        }
                    else
                        {
                        PLMTDI[AxisNo - 1] = false;
                        }
                    }

                return PLMTDI;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }

        //更新运动卡负限位信号状态
        /// <summary>
        /// 更新运动卡负限位信号状态
        /// </summary>
        /// <returns>返回运动卡负限位信号数组:[4轴(0~3) | 8轴(0~7)</returns>
        public bool[] UpdateNegativeLMTSignals()
            {
            int NegativeLMTValue = 0;
            bool[] NLMTDI;
            try
                {
                if (TotalAxisesOfCard == 4)
                    {
                    NLMTDI = new bool[4];
                    }
                else
                    {
                    NLMTDI = new bool[8];
                    }

                int Return = 0;
                //if (TotalAxisesOfCard == 4)
                //    {
                //    //四轴运动卡输入信号
                //    }
                //else
                //    {
                //    //八轴运动卡输入信号
                //    }

                //负限位
                Return += GT_GetDi(CardNumber, MC_LIMIT_NEGATIVE, out NegativeLMTValue);
                for (int AxisNo = 1; AxisNo <= TotalAxisesOfCard; AxisNo++)
                    {
                    if ((NegativeLMTValue >> (AxisNo - 1) & 1) == 1)
                        {
                        NLMTDI[AxisNo - 1] = true;
                        }
                    else
                        {
                        NLMTDI[AxisNo - 1] = false;
                        }
                    }

                return NLMTDI;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }

        //取消，避免与其它卡冲突--【重载】更新运动卡负限位信号状态
        /// <summary>
        /// 【重载】更新运动卡负限位信号状态
        /// </summary>
        /// <param name="TargetCard">目标运动卡号</param>
        /// <param name="TotalAxises">固高运动控制器运动卡的总数轴[4或8]</param>
        /// <returns>返回运动卡负限位信号数组:[4轴(0~3) | 8轴(0~7)</returns>
        private bool[] UpdateNegativeLMTSignals(short TargetCard, short TotalAxises)
            {
            int NegativeLMTValue = 0;
            bool[] NLMTDI;
            try
                {
                if (TargetCard < 1)
                    {
                    ErrorMessage = "参数'TargetCard'目标运动卡号不能小于1";
                    return null;
                    }

                if (!(TotalAxises == 4 || TotalAxises == 8))
                    {
                    //MessageBox.Show("参数'TotalAxises'目标运动卡控制轴的数量是4或者8。", "参数错误");
                    ErrorMessage = "参数'TotalAxises'目标运动卡控制轴的数量是4或者8。";
                    return null;
                    }

                if (TotalAxises == 4)
                    {
                    NLMTDI = new bool[4];
                    }
                else
                    {
                    NLMTDI = new bool[8];
                    }

                int Return = 0;
                //if (TotalAxises == 4)
                //    {
                //    //四轴运动卡输入信号
                //    }
                //else
                //    {
                //    //八轴运动卡输入信号
                //    }

                //负限位
                Return += GT_GetDi(TargetCard, MC_LIMIT_NEGATIVE, out NegativeLMTValue);
                for (int AxisNo = 1; AxisNo <= TotalAxises; AxisNo++)
                    {
                    if ((NegativeLMTValue >> (AxisNo - 1) & 1) == 1)
                        {
                        NLMTDI[AxisNo - 1] = true;
                        }
                    else
                        {
                        NLMTDI[AxisNo - 1] = false;
                        }
                    }

                return NLMTDI;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }

        //更新运动卡DI输入信号状态【4/8轴共用】
        /// <summary>
        /// 更新运动卡DI输入信号状态【4/8轴共用】
        /// </summary>
        /// <returns>返回运动卡DI输入信号数组:[4/8轴(0~15)</returns>
        public bool[] UpdateDI()
            {
            int DIValue = 0;
            bool[] DI = new bool[16];
            try
                {
                //if (TotalAxisesOfCard == 4)
                //    {
                //    DI = new bool[16];
                //    }
                //else
                //    {
                //    DI = new bool[16];
                //    }

                int Return = 0;

                //通用输入
                Return += GT_GetDi(CardNumber, MC_GPI, out DIValue);
                for (int a = 1; a <= 16; a++)
                    {
                    if (((DIValue >> (a - 1)) & 1) == 1)
                        {
                        DI[a - 1] = true;
                        }
                    else
                        {
                        DI[a - 1] = false;
                        }
                    }

                return DI;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }

        //取消，避免与其它卡冲突--【重载】更新运动卡DI输入信号状态【4/8轴共用】
        /// <summary>
        /// 【重载】更新运动卡DI输入信号状态【4/8轴共用】
        /// </summary>
        /// <param name="TargetCard">目标运动卡号</param>
        /// <returns>返回运动卡DI输入信号数组:[4/8轴(0~15)</returns>
        private bool[] UpdateDI(short TargetCard)
            {
            int DIValue = 0;
            bool[] DI = new bool[16];
            try
                {
                //if (TotalAxisesOfCard == 4)
                //    {
                //    DI = new bool[16];
                //    }
                //else
                //    {
                //    DI = new bool[16];
                //    }

                if (TargetCard < 1)
                    {
                    ErrorMessage = "参数'TargetCard'目标运动卡号不能小于1";
                    return DI;
                    }

                int Return = 0;

                //通用输入
                Return += GT_GetDi(TargetCard, MC_GPI, out DIValue);
                for (int a = 1; a <= 16; a++)
                    {
                    if (((DIValue >> (a - 1)) & 1) == 1)
                        {
                        DI[a - 1] = true;
                        }
                    else
                        {
                        DI[a - 1] = false;
                        }
                    }

                return DI;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return null;
                }
            }

        #endregion

        //【二次封装】控制固高运动控制器轴运动的类
        //默认电机转一圈的脉冲数量为10000，丝杆螺距【即电机转一圈的运动距离】为10毫米
        /// <summary>
        /// 【二次封装】控制固高运动控制器轴运动的类
        /// 默认电机转一圈的脉冲数量为10000，丝杆螺距【即电机转一圈的运动距离】为10毫米
        /// </summary>
        public class GTSAxisControl
            {

            #region "变量定义"

            private Thread PositiveSearchHomeThread = null, NegativeSearchHomeThread = null;

            public string ErrorMessage = "";
            private bool SuccessBuiltNew = false, PasswordIsCorrect = false;

            /// <summary>
            /// 运动控制卡号
            /// </summary>
            private short CardNumber = GoogolTechGTSCard.CardNumber;

            /// <summary>
            /// 运动控制卡轴号
            /// </summary>
            private short AxisNumber;

            /// <summary>
            /// 运动中标志
            /// </summary>
            private bool RunningFlag = false;

            /// <summary>
            /// 轴是否在运动中标志
            /// </summary>
            public bool Moving 
                {
                get 
                    {
                    GetAxisStatus();
                    return RunningFlag;
                    }
                }

            /// <summary>
            /// 停止标志
            /// </summary>
            private bool StopFlag = true;

            /// <summary>
            /// 轴是否已经停止运动标志
            /// </summary>
            public bool Stopped 
                {
                get 
                    {
                    GetAxisStatus();
                    return StopFlag;
                    }
                }

            /// <summary>
            /// 正限位标志
            /// </summary>
            private bool PositiveLimitFlag = false;

            /// <summary>
            /// 轴是否在正限位标志
            /// </summary>
            public bool AtPLMT 
                {
                get 
                    {
                    GetAxisStatus();
                    return PositiveLimitFlag;
                    }
                }

            /// <summary>
            /// 负限位标志
            /// </summary>
            private bool NegativeLimitFlag = false;

            /// <summary>
            /// 轴是否在负限位标志
            /// </summary>
            public bool AtNLMT
                {
                get
                    {
                    GetAxisStatus();
                    return NegativeLimitFlag;
                    }
                }

            /// <summary>
            /// 原点标志
            /// </summary>
            private bool HomeFlag = false;

            /// <summary>
            /// 轴是否在原点标志
            /// </summary>
            public bool AtHome
                {
                get
                    {
                    GetAxisStatus();
                    return HomeFlag;
                    }
                }

            /// <summary>
            /// 找原点完成标志
            /// </summary>
            private bool FinishedSearchingHome = false;

            /// <summary>
            /// 伺服报警标志
            /// </summary>
            private bool AlarmFlag = true;

            /// <summary>
            /// 轴是否伺服报警标志
            /// </summary>
            public bool Alarm
                {
                get
                    {
                    GetAxisStatus();
                    return AlarmFlag;
                    }
                }

            /// <summary>
            /// 伺服使能标志
            /// </summary>
            private bool AxisServoOnFlag = false;

            /// <summary>
            /// 伺服使能标志
            /// </summary>
            public bool AxisServoOn
                {
                get
                    {
                    GetAxisStatus();
                    return AxisServoOnFlag;
                    }
                }

            /// <summary>
            /// 当前位置（脉冲数）
            /// </summary>
            private double CurrentPosition = 0;

            //?
            /// <summary>
            /// 轴的当前位置【单位：MM】
            /// </summary>
            public double CurrentPosInMM 
                {
                get 
                    {
                    GetAxisStatus();
                    return PulsesToMM((int)CurrentPosition);
                    }
                }

            /// <summary>
            /// 轴的当前位置【单位：Pulses】
            /// </summary>
            public int CurrentPosInPulse 
                {
                get 
                    {
                    GetAxisStatus();
                    return (int)CurrentPosition;
                    }
                }
            
            /// <summary>
            /// 电机到位标志
            /// </summary>
            private bool MovedInPosition = false;

            /// <summary>
            /// 电机到位标志
            /// </summary>
            public bool MovementDoneFlag
                {
                get 
                    {
                    GetAxisStatus();
                    return MovedInPosition;
                    }
                }

            /// <summary>
            /// 丝杆每转的脉冲数
            /// </summary>
            private int PulsesPerRound = 0;

            /// <summary>
            /// 丝杆每转的脉冲数
            /// </summary>
            public int PulsesPerCircleSetting 
                {
                get 
                    {
                    return PulsesPerRound;
                    }
                set 
                    {
                    if (value > 0)
                        {
                        PulsesPerRound = value;
                        }
                    else 
                        {
                        MessageBox.Show("The value for 'PulsesPerRoundSetting' must be greater than 0.", "Error");
                        return;
                        }
                    }
                }

            /// <summary>
            /// 丝杆螺距（单位MM）
            /// </summary>
            private int ScrewPitchInMM = 0;

            /// <summary>
            /// 丝杆螺距（单位MM）
            /// </summary>
            public int ScrewPitchInMMSetting 
                {
                get 
                    {
                    return ScrewPitchInMM;
                    }
                set
                    {
                    if (value > 0)
                        {
                        ScrewPitchInMM = value;
                        }
                    else
                        {
                        MessageBox.Show("The value for 'ScrewPitchInMM' must be greater than 0.", "Error");
                        return;
                        }
                    }
                }

            /// <summary>
            /// 设置Jog模式参数
            /// </summary>
            private TJogPrm JogParameter=new TJogPrm();

            /// <summary>
            /// Jog【点动】速度
            /// </summary>
            private double TargetJogSpeed = 5;

            /// <summary>
            /// Jog速度
            /// </summary>
            public double JogSpeed 
                {
                get 
                    {
                    return TargetJogSpeed;
                    }
                set 
                    {
                    if (value > 0)
                        {
                        TargetJogSpeed = value;
                        }
                    else 
                        {
                        MessageBox.Show("Invalid value, the JogSpeed must be greater than 0.","Error");
                        return;
                        }
                    }
                }

            /// <summary>
            /// 设置点位模式参数
            /// </summary>
            private TTrapPrm TrapParameter = new TTrapPrm();

            /// <summary>
            /// 回原点补偿（单位MM）
            /// </summary>
            private double TargetSearchingHomeOffset = 0.0;

            /// <summary>
            /// 回原点速度（单位Pulse/ms）
            /// </summary>
            private int TargetSearchingHomeSpeed = 500;

            /// <summary>
            /// 回原点加速度
            /// </summary>
            private double SearchingHomeAccelRate = 1;

            /// <summary>
            /// 回原点减速度
            /// </summary>
            private double SearchingHomeDecelRate = 1;

            /// <summary>
            /// 起始速度
            /// </summary>
            private double StartSpeed = 20;

            /// <summary>
            /// 最大速度
            /// </summary>
            private double MaxSpeed = 50;
            
            #endregion

            #region "属性设置"

            /// <summary>
            /// 软件作者
            /// </summary>
            public string Author
                {
                get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
                }

            /// <summary>
            /// 轴停止方式的枚举
            /// </summary>
            public enum StopMethod 
                {
                /// <summary>
                /// 平滑停止
                /// </summary>
                SmoothStop=0,

                /// <summary>
                /// 急停
                /// </summary>
                EmergentStop=1
                }

            /// <summary>
            /// 轴参数结构体定义
            /// </summary>
            public struct AxisPara 
                {
                /// <summary>
                /// 设置运动距离【绝对位置】
                /// </summary>
                public double Position;

                /// <summary>
                /// 起动速度
                /// </summary>
                public int StartSpeed;

                /// <summary>
                /// 运动速度
                /// </summary>
                public int MaxSpeed;

                /// <summary>
                /// 加速率
                /// </summary>
                public double AcclSpd;

                /// <summary>
                /// 减速率
                /// </summary>
                public double DeclSpd;
                }

            /// <summary>
            /// 轴的参数结构体
            /// </summary>
            private AxisPara CurrentAxisPara;
            
            #endregion

            #region "函数代码"

            //创建此类的新实例，控制固高运动控制器相应的轴【限位信号为常闭】
            /// <summary>
            /// 创建此类的新实例，控制固高运动控制器相应的轴【限位信号为常闭】
            /// </summary>
            /// <param name="TargetAxis">目标轴号【1代表轴1...8代表轴8】</param>
            /// <param name="DLLPassword">使用此DLL的密码</param>
            /// <param name="TargetPulsePerCircle">电机转一圈的脉冲数量</param>
            /// <param name="TargetScrewPitchInMM">丝杆螺距【即电机转一圈的运动距离】</param>
            public GTSAxisControl(short TargetAxis, string DLLPassword,
                int TargetPulsePerRound = 10000, int TargetScrewPitchInMM = 10)
                {
                SuccessBuiltNew = false;
                PasswordIsCorrect = false;
                try
                    {
                    if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan")
                        || (DLLPassword == "彭东南"))
                        {
                        PasswordIsCorrect = true;

                        if (TargetAxis<1 || TargetAxis > GoogolTechGTSCard.TotalAxisesOfCard) 
                            {
                            MessageBox.Show("The current motion card just support " +
                                GoogolTechGTSCard.TotalAxisesOfCard 
                                + " axises, please revise the parameter 'TargetAxis'","Error");
                            return;
                            }

                        AxisNumber = TargetAxis;
                        GetAxisStatus();
                        PulsesPerRound = TargetPulsePerRound;
                        ScrewPitchInMM = TargetScrewPitchInMM;
                        
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

            //将轴的驱动方式设为脉冲加方向
            /// <summary>
            /// 将轴的驱动方式设为脉冲加方向
            /// </summary>
            /// <returns></returns>
            public bool PulseDirMode()
                {
                try
                    {

                    int Return = 0;
                    Return += GT_Stop(CardNumber, AxisNumber, 0);
                    Return += GT_AxisOff(CardNumber, AxisNumber);
                    Return += GT_StepDir(CardNumber, AxisNumber);
                    if (Return != 0)
                        {
                        ErrorMessage = "将轴的驱动方式设为脉冲加方向发生错误";
                        return false;
                        }
                    else 
                        {
                        return true;
                        }
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //将轴的驱动方式设为双脉冲（CW+CCW）
            /// <summary>
            /// 将轴的驱动方式设为双脉冲（CW+CCW）
            /// </summary>
            /// <returns></returns>
            public bool CWPlusCCWMode()
                {
                try
                    {

                    int Return = 0;
                    Return += GT_Stop(CardNumber, AxisNumber, 0);
                    Return += GT_AxisOff(CardNumber, AxisNumber);
                    Return += GT_StepPulse(CardNumber, AxisNumber);
                    if (Return != 0)
                        {
                        ErrorMessage = "将轴的驱动方式设为双脉冲（CW+CCW）发生错误";
                        return false;
                        }
                    else
                        {
                        return true;
                        }
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //********************
            //毫米转脉冲
            /// <summary>
            /// 毫米转脉冲
            /// </summary>
            /// <param name="TargetDistanceInMM">距离（单位MM）</param>
            /// <param name="TargetPulsesPerCircle">电机转1圈所需脉冲数</param>
            /// <param name="TargetScrewPitchMM">丝杆螺距</param>
            /// <returns>换算后的脉冲数</returns>
            public int MMToPulses(double TargetDistanceInMM, int TargetPulsesPerRound,
                int TargetScrewPitchMM)
                {
                //公式:脉冲= （距离（毫米）*丝杆转1圈所需脉冲数）/丝杆螺距（毫米）
                try
                    {
                    int Return = 0;
                    Return = (int)((TargetDistanceInMM * TargetPulsesPerRound) / TargetScrewPitchMM);
                    return Return;
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return 0;
                    }
                }

            //【重载】毫米转脉冲:利用实例化时的丝杆螺距和电机转一圈所需脉冲数
            /// <summary>
            /// 【重载】毫米转脉冲:利用实例化时的丝杆螺距和电机转一圈所需脉冲数
            /// </summary>
            /// <param name="TargetDistanceInMM">距离（单位MM）</param>
            /// <returns>换算后的脉冲数</returns>
            public int MMToPulses(double TargetDistanceInMM)
                {
                //公式:脉冲= （距离（毫米）*丝杆转1圈所需脉冲数）/丝杆螺距（毫米）
                try
                    {
                    int Return = 0;
                    Return = (int)((TargetDistanceInMM * PulsesPerRound) / ScrewPitchInMM);
                    return Return;
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return 0;
                    }
                }
            //********************

            //脉冲转毫米
            /// <summary>
            /// 脉冲转毫米
            /// </summary>
            /// <param name="NumberOfPulses">脉冲数</param>
            /// <param name="TargetPulsesPerCircle">电机转1圈所需脉冲数</param>
            /// <param name="TargetScrewPitchMM">丝杆螺距</param>
            /// <returns>换算后的距离【毫米】</returns>
            public double PulsesToMM(int NumberOfPulses, int TargetPulsesPerRound,
                int TargetScrewPitchMM)
                {
                //公式:毫米= 脉冲数*(丝杆螺距 /电机转1圈脉冲数)
                try
                    {
                    double Return = 0;
                    Return = (double)(NumberOfPulses * (TargetPulsesPerRound / TargetScrewPitchMM));
                    return Return;
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return 0;
                    }
                }

            //【重载】脉冲转毫米:利用实例化时的丝杆螺距和电机转一圈所需脉冲数
            /// <summary>
            /// 【重载】脉冲转毫米:利用实例化时的丝杆螺距和电机转一圈所需脉冲数
            /// </summary>
            /// <param name="NumberOfPulses">脉冲数</param>
            /// <returns>换算后的距离【毫米】</returns>
            public double PulsesToMM(int NumberOfPulses)
                {
                //公式:毫米= 脉冲数*(丝杆螺距 /电机转1圈脉冲数)
                try
                    {
                    double Return = 0;
                    Return = (double)(NumberOfPulses * (PulsesPerRound / ScrewPitchInMM));
                    return Return;
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return 0;
                    }
                }
            
            //轴状态更新
            /// <summary>
            /// 轴状态更新
            /// </summary>
            /// <returns>是否执行成功</returns>
            public bool GetAxisStatus()
                {
                try
                    {
                    //Bit      Function
                    //0:       保留
                    //1:       驱动器报警标志 : 控制轴连接的驱动器报警时置1
                    //2:       保留
                    //3:       保留
                    //4:       跟随误差越限标志 :控制轴规划位置和实际的误差大于设定极限时1
                    //5:       正限位触发标志 : 正限位开关电平状态为触发时置 1, 
                    //         规划位置大于正向软限时1
                    //6:       负限位触发标志 : 负限位开关电平状态为触发时置 1, 
                    //         规划位置小于负向软限时1
                    //7:       IO 平滑停止触发标志: 如果轴设置了平滑停止 IO ，
                    //         当其输入为触发电平时置 1，并自动平滑停止该轴
                    //8:       IO 急停触发标志 ： 如果轴设置了急停 IO ，
                    //         当其输入为触发电平时置 1，并自动急停该轴
                    //9:       电机使能标志： 电机使能时置 1
                    //10:      规划运动标志： 规划器运动时置 1
                    //11:      电机到位标志 ：规划器静止，
                    //         位置和实际的误差小于设定带并且在内保持时间后，置起到位标志
                    //12~31    保留

                    int AxisStatusValue = 0;
                    uint PClock = 0;
                    int Return = 0;
                    Return += GT_GetSts(CardNumber, AxisNumber, out AxisStatusValue, 1, out PClock);
                    if (Return == 0)
                        {
                        //忙/定位完成标志
                        if (((AxisStatusValue >> 10) & 0x1) == 1)
                            {
                            RunningFlag = true;
                            StopFlag = false;
                            }
                        else 
                            {
                            RunningFlag = false;
                            StopFlag = true;
                            }

                        //电机到位标志
                        if (((AxisStatusValue >> 11) & 0x1) == 1)
                            {
                            MovedInPosition = true;
                            }
                        else
                            {
                            MovedInPosition = false;
                            }

                        //正限位标志
                        if (((AxisStatusValue >> 5) & 0x1) == 1)
                            {
                            PositiveLimitFlag = true;
                            }
                        else
                            {
                            PositiveLimitFlag = false;
                            }

                        //负限位标志
                        if (((AxisStatusValue >> 6) & 0x1) == 1)
                            {
                            NegativeLimitFlag = true;
                            }
                        else
                            {
                            NegativeLimitFlag = false;
                            }

                        //报警标志
                        if (((AxisStatusValue >> 1) & 0x1) == 1)
                            {
                            AlarmFlag = true;
                            }
                        else
                            {
                            AlarmFlag = false;
                            }

                        //电机使能标志
                        if (((AxisStatusValue >> 9) & 0x1) == 1)
                            {
                            AxisServoOnFlag = true;
                            }
                        else
                            {
                            AxisServoOnFlag = false;
                            }
                        }
                    else 
                        {
                        return false;
                        }

                    //原点标志
                    Return += GT_GetDi(CardNumber, MC_HOME, out AxisStatusValue);
                    if (((AxisStatusValue >> (AxisNumber-1)) & 0x1) == 1)
                        {
                        HomeFlag = true;
                        }
                    else
                        {
                        HomeFlag = false;
                        }

                    //获取轴当前位置
                    Return += GT_GetPrfPos(CardNumber, AxisNumber, out CurrentPosition, 1, out PClock);
                    if (Return != 0)
                        {
                        return false;
                        }
                    else 
                        {
                        return true;
                        }
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //获取轴的当前速度
            /// <summary>
            /// 获取轴的当前速度
            /// </summary>
            /// <returns></returns>
            public double GetCurrentVel()
                {
                try
                    {
                    double CurrentVel=0;
                    uint PClock = 0;
                    int Return = 0;
                    Return = GT_GetPrfVel(CardNumber, AxisNumber, out CurrentVel, 1, out PClock);
                    return CurrentVel;
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return 0;
                    }
                }

            //获取轴当前位置(Pulses)
            /// <summary>
            /// 获取轴当前位置(Pulses)
            /// </summary>
            /// <returns></returns>
            public double GetCurrentPosInPulses()
                {
                try
                    {
                    double CurrentPos = 0;
                    uint PClock = 0;
                    int Return = 0;
                    Return = GT_GetPrfPos(CardNumber, AxisNumber, out CurrentPos, 1, out PClock);
                    return CurrentPos;
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return 0;
                    }
                }

            //获取轴当前位置(MM)
            /// <summary>
            /// 获取轴当前位置(MM)
            /// </summary>
            /// <returns></returns>
            public double GetCurrentPosInMM()
                {
                try
                    {
                    double CurrentPos = 0;
                    uint PClock = 0;
                    int Return = 0;
                    Return = GT_GetPrfPos(CardNumber, AxisNumber, out CurrentPos, 1, out PClock);
                    return PulsesToMM((int)CurrentPos);
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return 0;
                    }
                }

            //轴使能Off
            /// <summary>
            /// 轴使能Off
            /// </summary>
            /// <returns></returns>
            public bool ServoOff()
                {
                try
                    {
                    int Return = 0;
                    Return += GT_AxisOff(CardNumber, AxisNumber);
                    if (Return != 0)
                        {
                        return false;
                        }
                    else 
                        {
                        return true;
                        }
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //轴使能On
            /// <summary>
            /// 轴使能On
            /// </summary>
            /// <returns></returns>
            public bool ServoOn()
                {
                try
                    {
                    int Return = 0;
                    Return += GT_AxisOn(CardNumber, AxisNumber);
                    if (Return != 0)
                        {
                        return false;
                        }
                    else
                        {
                        return true;
                        }
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //相对定位模式运动
            /// <summary>
            /// 相对定位模式运动
            /// </summary>
            /// <param name="Speed">相对定位速度</param>
            /// <param name="DistanceInMM">相对定位距离（单位：MM）</param>
            /// <returns></returns>
            public bool MoveRelativePos(int Speed, double DistanceInMM)
                {
                try
                    {
                    //点位运动：完成点到点运动，在运动过程中可以随时修改目标位置和目标速度
                    int Return = 0;
                    uint PClock = 0;
                    double Pos = 0;

                    GetAxisStatus();
                    if (AxisServoOnFlag == false) 
                        {
                        Return += GT_AxisOn(CardNumber, AxisNumber);//使能ON
                        }
                    
                    Return += GT_GetTrapPrm(CardNumber, AxisNumber, out TrapParameter);//读取运动参数
                    TrapParameter.acc = 1;//加速度
                    TrapParameter.dec = 1;//减速度
                    TrapParameter.smoothTime = 0;//平滑时间s
                    TrapParameter.velStart = 3;//起跳速度

                    Return += GT_ClrSts(CardNumber, AxisNumber, 1);//清除轴状态--运动之前必须调用此函数清除轴状态
                    Return += GT_PrfTrap(CardNumber, AxisNumber);//设置轴为点位模式，参数：轴号
                    Return += GT_SetTrapPrm(CardNumber, AxisNumber, ref TrapParameter);//设置运动参数
                    Return += GT_SetVel(CardNumber, AxisNumber, Speed);//设置运动速度
                    Return += GT_GetPrfPos(CardNumber, AxisNumber, out Pos, 1, out PClock);

                    //把“当前位置+相对运动值”作为目标值
                    Return += GT_SetPos(CardNumber, AxisNumber, (int)Pos + MMToPulses(DistanceInMM));
                    Return += GT_Update(CardNumber, 1 << (AxisNumber - 1));
                    return true;
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //绝对定位模式
            /// <summary>
            /// 绝对定位模式
            /// </summary>
            /// <param name="Speed">绝对定位速度</param>
            /// <param name="TargetAccl">加速度</param>
            /// <param name="TargetDecl">减速度</param>
            /// <param name="DistanceInMM">绝对定位距离（单位：MM）</param>
            /// <returns></returns>
            public bool MoveABSPos(int Speed, double TargetAccl, 
                double TargetDecl, double DistanceInMM)
                {
                try
                    {
                    //点位运动：完成点到点运动，在运动过程中可以随时修改目标位置和目标速度
                    int Return = 0;

                    GetAxisStatus();
                    if (AxisServoOnFlag == false)
                        {
                        Return += GT_AxisOn(CardNumber, AxisNumber);//使能ON
                        }

                    Return += GT_GetTrapPrm(CardNumber, AxisNumber, out TrapParameter);//读取运动参数
                    TrapParameter.acc = TargetAccl;//加速度
                    TrapParameter.dec = TargetDecl;//减速度
                    TrapParameter.smoothTime = 0;//平滑时间s
                    TrapParameter.velStart = 3;//起跳速度

                    Return += GT_ClrSts(CardNumber, AxisNumber, 1);//清除轴状态--运动之前必须调用此函数清除轴状态
                    Return += GT_PrfTrap(CardNumber, AxisNumber);//设置轴为点位模式，参数：轴号
                    Return += GT_SetTrapPrm(CardNumber, AxisNumber, ref TrapParameter);//设置运动参数
                    Return += GT_SetVel(CardNumber, AxisNumber, Speed);//设置运动速度
                    Return += GT_SetPos(CardNumber, AxisNumber, MMToPulses(DistanceInMM));
                    Return += GT_Update(CardNumber, 1 << (AxisNumber - 1));
                    return true;
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //【重载】绝对定位模式
            /// <summary>
            /// 【重载】绝对定位模式
            /// </summary>
            /// <param name="TargetPara">目标参数的数据结构</param>
            /// <returns></returns>
            public bool MoveABSPos(AxisPara TargetPara)
                {
                try
                    {
                    //点位运动：完成点到点运动，在运动过程中可以随时修改目标位置和目标速度
                    int Return = 0;

                    GetAxisStatus();
                    if (AxisServoOnFlag == false)
                        {
                        Return += GT_AxisOn(CardNumber, AxisNumber);//使能ON
                        }

                    Return += GT_GetTrapPrm(CardNumber, AxisNumber, out TrapParameter);//读取运动参数
                    TrapParameter.acc = TargetPara.AcclSpd;//加速度
                    TrapParameter.dec = TargetPara.DeclSpd;//减速度
                    TrapParameter.smoothTime = 0;//平滑时间s
                    TrapParameter.velStart = 3;//起跳速度

                    Return += GT_ClrSts(CardNumber, AxisNumber, 1);//清除轴状态--运动之前必须调用此函数清除轴状态
                    Return += GT_PrfTrap(CardNumber, AxisNumber);//设置轴为点位模式，参数：轴号
                    Return += GT_SetTrapPrm(CardNumber, AxisNumber, ref TrapParameter);//设置运动参数
                    Return += GT_SetVel(CardNumber, AxisNumber, TargetPara.MaxSpeed);//设置运动速度
                    Return += GT_SetPos(CardNumber, AxisNumber, MMToPulses(TargetPara.Position));
                    Return += GT_Update(CardNumber, 1 << (AxisNumber - 1));
                    return true;
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //停止轴运动
            /// <summary>
            /// 停止轴运动
            /// </summary>
            /// <param name="TargetStopMethod">停止方式</param>
            /// <returns></returns>
            public bool Stop(StopMethod TargetStopMethod)
                {
                try
                    {
                    int Return = 0;
                    //参数1：卡号； 参数2：要停止的轴； 参数3：设置平滑停止或急停(0代表平滑停止，1代表急停)。
                    if (TargetStopMethod == StopMethod.SmoothStop)
                        {
                        Return = GT_Stop(CardNumber, 1 << (AxisNumber - 1), 0);
                        }
                    else 
                        {
                        Return = GT_Stop(CardNumber, 1 << (AxisNumber - 1), 1);
                        }
                    return true;
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //急停轴运动
            /// <summary>
            /// 急停轴运动
            /// </summary>
            /// <returns></returns>
            public bool Stop()
                {
                try
                    {
                    int Return = 0;
                    //参数1：卡号； 参数2：要停止的轴； 参数3：设置平滑停止或急停(0代表平滑停止，1代表急停)。
                    Return = GT_Stop(CardNumber, 1 << (AxisNumber - 1), 1);
                    return true;
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //Jog+ 点动正转运动模式
            /// <summary>
            /// Jog+ 点动正转运动模式
            /// </summary>
            /// <param name="JogSpeed">点动速度</param>
            /// <param name="TargetAccl">加速度</param>
            /// <param name="TargetDecl">减速度</param>
            /// <param name="SmoothTime">平滑时间</param>
            /// <returns></returns>
            public bool JogForward(double JogSpeed, double TargetAccl, 
                double TargetDecl, double SmoothTime=5)
                {
                try
                    {
                    int Return = 0;

                    GetAxisStatus();
                    if (AxisServoOnFlag == false)
                        {
                        Return += GT_AxisOn(CardNumber, AxisNumber);//使能ON
                        }

                    TargetJogSpeed = Math.Abs(JogSpeed);
                    Return += GT_GetJogPrm(CardNumber, AxisNumber, out JogParameter);
                    JogParameter.acc = TargetAccl;
                    JogParameter.dec = TargetDecl;
                    JogParameter.smooth = SmoothTime;
                    Return += GT_PrfJog(CardNumber, AxisNumber);//设置轴为Jog模式，参数：轴号
                    Return += GT_SetJogPrm(CardNumber, AxisNumber, ref JogParameter);//设置运动参数
                    Return += GT_SetVel(CardNumber, AxisNumber, TargetJogSpeed);//设置运动速度：正转
                    Return += GT_Update(CardNumber, 1 << (AxisNumber - 1));//启动运动

                    if (Return != 0)
                        {
                        return false;
                        }
                    else 
                        {
                        return true;
                        }
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //Jog+ 点动正转运动模式
            /// <summary>
            /// Jog+ 点动正转运动模式
            /// </summary>
            /// <returns></returns>
            public bool JogForward()
                {
                try
                    {
                    int Return = 0;

                    GetAxisStatus();
                    if (AxisServoOnFlag == false)
                        {
                        Return += GT_AxisOn(CardNumber, AxisNumber);//使能ON
                        }

                    TargetJogSpeed = Math.Abs(JogSpeed);
                    Return += GT_GetJogPrm(CardNumber, AxisNumber, out JogParameter);
                    JogParameter.acc = 1;
                    JogParameter.dec = 1;
                    JogParameter.smooth = 5;
                    Return += GT_PrfJog(CardNumber, AxisNumber);//设置轴为Jog模式，参数：轴号
                    Return += GT_SetJogPrm(CardNumber, AxisNumber, ref JogParameter);//设置运动参数
                    Return += GT_SetVel(CardNumber, AxisNumber, TargetJogSpeed);//设置运动速度：正转
                    Return += GT_Update(CardNumber, 1 << (AxisNumber - 1));//启动运动

                    if (Return != 0)
                        {
                        return false;
                        }
                    else
                        {
                        return true;
                        }
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //Jog- 点动反转运动模式
            /// <summary>
            /// Jog- 点动反转运动模式
            /// </summary>
            /// <param name="JogSpeed">点动速度</param>
            /// <param name="TargetAccl">加速度</param>
            /// <param name="TargetDecl">减速度</param>
            /// <param name="SmoothTime">平滑时间</param>
            /// <returns></returns>
            public bool JogReverse(double JogSpeed, double TargetAccl,
                double TargetDecl, double SmoothTime = 5)
                {
                try
                    {
                    int Return = 0;

                    GetAxisStatus();
                    if (AxisServoOnFlag == false)
                        {
                        Return += GT_AxisOn(CardNumber, AxisNumber);//使能ON
                        }

                    TargetJogSpeed = -Math.Abs(JogSpeed);//取绝对值，然后变为负值，反转
                    Return += GT_GetJogPrm(CardNumber, AxisNumber, out JogParameter);
                    JogParameter.acc = TargetAccl;
                    JogParameter.dec = TargetDecl;
                    JogParameter.smooth = SmoothTime;
                    Return += GT_PrfJog(CardNumber, AxisNumber);//设置轴为Jog模式，参数：轴号
                    Return += GT_SetJogPrm(CardNumber, AxisNumber, ref JogParameter);//设置运动参数
                    Return += GT_SetVel(CardNumber, AxisNumber, TargetJogSpeed);//设置运动速度：反转
                    Return += GT_Update(CardNumber, 1 << (AxisNumber - 1));//启动运动

                    if (Return != 0)
                        {
                        return false;
                        }
                    else
                        {
                        return true;
                        }
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //Jog- 点动反转运动模式
            /// <summary>
            /// Jog- 点动反转运动模式
            /// </summary>
            /// <returns></returns>
            public bool JogReverse()
                {
                try
                    {
                    int Return = 0;

                    GetAxisStatus();
                    if (AxisServoOnFlag == false)
                        {
                        Return += GT_AxisOn(CardNumber, AxisNumber);//使能ON
                        }

                    TargetJogSpeed = -Math.Abs(JogSpeed);//取绝对值，然后变为负值，反转
                    Return += GT_GetJogPrm(CardNumber, AxisNumber, out JogParameter);
                    JogParameter.acc = 1;
                    JogParameter.dec = 1;
                    JogParameter.smooth = 5;
                    Return += GT_PrfJog(CardNumber, AxisNumber);//设置轴为Jog模式，参数：轴号
                    Return += GT_SetJogPrm(CardNumber, AxisNumber, ref JogParameter);//设置运动参数
                    Return += GT_SetVel(CardNumber, AxisNumber, TargetJogSpeed);//设置运动速度：反转
                    Return += GT_Update(CardNumber, 1 << (AxisNumber - 1));//启动运动

                    if (Return != 0)
                        {
                        return false;
                        }
                    else
                        {
                        return true;
                        }
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //原点设置
            /// <summary>
            /// 原点设置
            /// </summary>
            /// <returns></returns>
            public bool SetZeroPos()
                {
                try
                    {
                    int Return = 0;
                    int AxisStatusValue = 0;
                    uint PClock = 0;

                    // 位 10 -- 规划运动标志
                    //          规划器运动时置1
                    Return = GT_GetSts(CardNumber, AxisNumber, out AxisStatusValue, 1, out PClock);

                    //如果轴在运动中，就不执行原点设置
                    if (((AxisStatusValue >> 10) & 0x1) == 1)
                        {
                        ErrorMessage = "轴运动中，不能设置原点";
                        return false;
                        }
                    else 
                        {
                        Return += GT_ZeroPos(CardNumber, AxisNumber, 1);
                        if (Return != 0)
                            {
                            return false;
                            }
                        else 
                            {
                            return true;
                            }
                        }
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //执行正方向回原点操作
            /// <summary>
            /// 执行正方向回原点操作
            /// </summary>
            /// <param name="Speed">回原点速度</param>
            /// <param name="HomeOffset">回到原点后再偏移一定距离，默认为0</param>
            /// <param name="Accl">加速度</param>
            /// <param name="Decl">减速度</param>
            /// <returns>如果发生错误或者正在执行回原点就返回false</returns>
            public bool PositiveSearchHome(int Speed=5, double HomeOffset=0, 
                double Accl =1, double Decl = 1)
                {
                try
                    {
                    int Return = 0;

                    //如果轴在运动中，就停止轴
                    GetAxisStatus();
                    if (RunningFlag == true) 
                        {
                        Return += GT_Stop(CardNumber, 2 ^ (AxisNumber - 1), 0);
                        }

                    if (AxisServoOnFlag == false) 
                        {
                        Return += GT_AxisOn(CardNumber, AxisNumber);
                        }

                    if (Return != 0) 
                        {
                        ErrorMessage = "Error occurs when stop or set the axis servo on.";
                        return false;
                        }

                    TargetSearchingHomeSpeed = Speed;
                    TargetSearchingHomeOffset = HomeOffset;
                    SearchingHomeAccelRate = Accl;
                    SearchingHomeDecelRate = Decl;

                    FinishedSearchingHome = false;

                    if (PositiveSearchHomeThread == null)
                        {
                        PositiveSearchHomeThread = new Thread(PositiveSearchingHome);
                        PositiveSearchHomeThread.IsBackground = true;
                        PositiveSearchHomeThread.Start();
                        return true;
                        }
                    else 
                        {
                        ErrorMessage = "Positive search home is under going...";
                        return false;
                        }
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }
            
            //执行正方向回原点操作
            /// <summary>
            /// 执行正方向回原点操作
            /// </summary>
            private void PositiveSearchingHome()
                {
                try
                    {
                    int Return = 0;
                    int PLMTValue = 0, Pos = 0;
                    uint PClock = 0;
                    short AxisStatusValue = 0;

                    HomeFlag = false;
                    Return += GT_GetDi(CardNumber, MC_LIMIT_POSITIVE, out PLMTValue);

                    //如果不在正限位，就朝正限位运动，直到碰到正限位开关【常闭信号】
                    if (((PLMTValue >> (AxisNumber - 1)) & 0x1) != 1) 
                        {
                        MoveABSPos(TargetSearchingHomeSpeed, SearchingHomeAccelRate, 
                            SearchingHomeDecelRate, 400000);
                        do 
                            {
                            Return += GT_GetDi(CardNumber, MC_LIMIT_POSITIVE, out PLMTValue);
                            }
                        while (((PLMTValue >> (AxisNumber - 1)) & 0x1) == 1);
                        }

                    //停止轴运动
                    Return += GT_Stop(CardNumber, 2 ^ (AxisNumber - 1), 0);

                    //清除轴状态
                    Return += GT_ClrSts(CardNumber, AxisNumber, 1);

                    //设置捕获模式为HOME模式
                    Return += GT_SetCaptureMode(CardNumber, AxisNumber, CAPTURE_HOME);

                    //负方向运动，搜索原点
                    MoveABSPos(TargetSearchingHomeSpeed, SearchingHomeAccelRate,
                            SearchingHomeDecelRate, -400000);

                    do
                        {
                        //获取捕获位置
                        Return += GT_GetCaptureStatus(CardNumber, AxisNumber,
                            out AxisStatusValue, out Pos, 1, out PClock);
                        }
                    while (((AxisStatusValue >> 10) & 0x1) == 1);

                    //回到原点后移动一定距离当成原点
                    if (TargetSearchingHomeOffset != 0) 
                        {
                        MoveABSPos(TargetSearchingHomeSpeed, SearchingHomeAccelRate,
                            SearchingHomeDecelRate, TargetSearchingHomeOffset);
                        }
                    
                    //位置清零，设置成原点
                    Return += GT_ZeroPos(CardNumber, AxisNumber, 1);

                    FinishedSearchingHome = true;
                    HomeFlag = true;

                    //释放线程
                    PositiveSearchHomeThread = null;

                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    }
                }

            //执行负方向回原点操作
            /// <summary>
            /// 执行负方向回原点操作
            /// </summary>
            /// <param name="Speed">回原点速度</param>
            /// <param name="HomeOffset">回到原点后再偏移一定距离，默认为0</param>
            /// <param name="Accl">加速度</param>
            /// <param name="Decl">减速度</param>
            /// <returns>如果发生错误或者正在执行回原点就返回false</returns>
            public bool NegativeSearchHome(int Speed = 5, double HomeOffset = 0,
                double Accl = 1, double Decl = 1)
                {
                try
                    {
                    int Return = 0;

                    //如果轴在运动中，就停止轴
                    GetAxisStatus();
                    if (RunningFlag == true)
                        {
                        Return += GT_Stop(CardNumber, 2 ^ (AxisNumber - 1), 0);
                        }

                    if (AxisServoOnFlag == false)
                        {
                        Return += GT_AxisOn(CardNumber, AxisNumber);
                        }

                    if (Return != 0)
                        {
                        ErrorMessage = "Error occurs when stop or set the axis servo on.";
                        return false;
                        }

                    TargetSearchingHomeSpeed = Speed;
                    TargetSearchingHomeOffset = HomeOffset;
                    SearchingHomeAccelRate = Accl;
                    SearchingHomeDecelRate = Decl;

                    FinishedSearchingHome = false;

                    if (NegativeSearchHomeThread == null)
                        {
                        NegativeSearchHomeThread = new Thread(NegativeSearchingHome);
                        NegativeSearchHomeThread.IsBackground = true;
                        NegativeSearchHomeThread.Start();
                        return true;
                        }
                    else
                        {
                        ErrorMessage = "Negative search home is under going...";
                        return false;
                        }
                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    return false;
                    }
                }

            //执行负方向回原点操作
            /// <summary>
            /// 执行负方向回原点操作
            /// </summary>
            private void NegativeSearchingHome()
                {
                try
                    {
                    int Return = 0;
                    int NLMTValue = 0, Pos = 0;
                    uint PClock = 0;
                    short AxisStatusValue = 0;

                    HomeFlag = false;
                    Return += GT_GetDi(CardNumber, MC_LIMIT_NEGATIVE, out NLMTValue);

                    //如果不在负限位，就朝负限位运动，直到碰到负限位开关【常闭信号】
                    if (((NLMTValue >> (AxisNumber - 1)) & 0x1) != 1)
                        {
                        MoveABSPos(TargetSearchingHomeSpeed, SearchingHomeAccelRate,
                            SearchingHomeDecelRate, -400);
                        do
                            {
                            Return += GT_GetDi(CardNumber, MC_LIMIT_NEGATIVE, out NLMTValue);
                            }
                        while (((NLMTValue >> (AxisNumber - 1)) & 0x1) == 1);
                        }

                    //停止轴运动
                    Return += GT_Stop(CardNumber, 2 ^ (AxisNumber - 1), 0);

                    //清除轴状态
                    Return += GT_ClrSts(CardNumber, AxisNumber, 1);

                    //设置捕获模式为HOME模式
                    Return += GT_SetCaptureMode(CardNumber, AxisNumber, CAPTURE_HOME);

                    //负方向运动，搜索原点
                    MoveABSPos(TargetSearchingHomeSpeed, SearchingHomeAccelRate,
                            SearchingHomeDecelRate, 400);

                    do
                        {
                        //获取捕获位置
                        Return += GT_GetCaptureStatus(CardNumber, AxisNumber,
                            out AxisStatusValue, out Pos, 1, out PClock);
                        }
                    while (((AxisStatusValue >> 10) & 0x1) == 1);

                    //回到原点后移动一定距离当成原点
                    if (TargetSearchingHomeOffset != 0)
                        {
                        MoveABSPos(TargetSearchingHomeSpeed, SearchingHomeAccelRate,
                            SearchingHomeDecelRate, TargetSearchingHomeOffset);
                        }

                    //位置清零，设置成原点
                    Return += GT_ZeroPos(CardNumber, AxisNumber, 1);

                    FinishedSearchingHome = true;
                    HomeFlag = true;

                    //释放线程
                    NegativeSearchHomeThread = null;

                    }
                catch (Exception ex)
                    {
                    ErrorMessage = ex.Message;
                    }
                }
                        
            #endregion

            }//GTSAxisControl class
        
        }//GoogolTechGTSCard class

    }//namespace