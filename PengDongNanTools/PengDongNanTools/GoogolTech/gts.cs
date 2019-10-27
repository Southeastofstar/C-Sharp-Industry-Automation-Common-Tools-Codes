using System.Runtime.InteropServices;
//固高多卡C#库

namespace PengDongNanTools
{
    //固高GTS系列运动卡C#多卡库函数调用
    /// <summary>
    /// 固高GTS系列运动卡C#多卡库函数调用
    /// </summary>
    public static class GTS
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
    public static extern short GT_SetCardNo(short cardNum, short index);

    //读取当前运动控制器卡号
    /// <summary>
    /// 读取当前运动控制器卡号
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="index">读取的当前运动控制器的卡号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetCardNo(short cardNum, out short index);

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
    public static extern short GT_Open(short cardNum, short channel, short param);

    //关闭运动控制器
    /// <summary>
    /// 关闭运动控制器
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_Close(short cardNum);

    //下载配置信息到运动控制器
    /// <summary>
    /// 下载配置信息到运动控制器
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="pFile">配置文件的文件名</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_LoadConfig(short cardNum, string pFile);

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
    public static extern short GT_GetVersion(short cardNum, out string pVersion);

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
    public static extern short GT_SetDo(short cardNum, short doType, int value);

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
    public static extern short GT_SetDoBit(short cardNum, short doType, short doIndex,
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
    public static extern short GT_GetDo(short cardNum, short doType, out int pValue);

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
    public static extern short GT_SetDoBitReverse(short cardNum, short doType,
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
    public static extern short GT_GetDi(short cardNum, short diType, out int pValue);

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
    public static extern short GT_GetDiReverseCount(short cardNum, short diType,
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
    public static extern short GT_SetDiReverseCount(short cardNum, short diType,
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
    public static extern short GT_GetDiRaw(short cardNum, short diType, out int pValue);

    //设置dac输出电压
    /// <summary>
    /// 设置dac输出电压
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="dac">dac起始通道号</param>
    /// <param name="value">输出电压
    /// -32768对应-10V；32767对应+10V</param>
    /// <param name="count">设置的通道数，默认为1
    /// 1次最多可以设置8路dac输出</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_SetDac(short cardNum, short dac, ref short value, short count);

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
    public static extern short GT_GetDac(short cardNum, short dac, out short value,
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
    public static extern short GT_GetAdc(short cardNum, short adc, out double pValue,
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
    public static extern short GT_GetAdcValue(short cardNum, short adc,
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
    public static extern short GT_SetEncPos(short cardNum, short encoder, int encPos);

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
    public static extern short GT_GetEncPos(short cardNum, short encoder,
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
    public static extern short GT_GetEncVel(short cardNum, short encoder,
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
    public static extern short GT_SetCaptureMode(short cardNum, short encoder, short mode);

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
    public static extern short GT_GetCaptureMode(short cardNum, short encoder,
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
    public static extern short GT_GetCaptureStatus(short cardNum, short encoder,
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
    public static extern short GT_SetCaptureSense(short cardNum, short encoder,
        short mode, short sense);

    //清除捕获状态
    /// <summary>
    /// 清除捕获状态
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="encoder">需要被清除捕获状态的编码器轴号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_ClearCaptureStatus(short cardNum, short encoder);

    //复位运动控制器
    /// <summary>
    /// 复位运动控制器
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_Reset(short cardNum);

    //读取运动控制器系统时钟
    /// <summary>
    /// 读取运动控制器系统时钟
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="pClock">读取的运动控制器的时钟，单位：毫秒</param>
    /// <param name="pLoop">内部使用，默认为：NULL，即不读取该值</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetClock(short cardNum, out uint pClock, out uint pLoop);

    //读取运动控制器系统高精度时钟
    /// <summary>
    /// 读取运动控制器系统高精度时钟
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="pClock">读取的运动控制器的时钟，单位：125微秒</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetClockHighPrecision(short cardNum, out uint pClock);

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
    public static extern short GT_GetSts(short cardNum, short axis, out int pSts,
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
    public static extern short GT_ClrSts(short cardNum, short axis, short count);

    //打开驱动器使能
    /// <summary>
    /// 打开驱动器使能
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="axis">打开伺服使能的轴的编号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_AxisOn(short cardNum, short axis);

    //关闭驱动器使能
    /// <summary>
    /// 关闭驱动器使能
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="axis">关闭伺服使能的轴的编号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_AxisOff(short cardNum, short axis);

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
    public static extern short GT_Stop(short cardNum, int mask, int option);

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
    public static extern short GT_SetPrfPos(short cardNum, short profile, int prfPos);

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
    public static extern short GT_SynchAxisPos(short cardNum, int mask);

    //清零规划位置和实际位置，并进行零漂补偿
    /// <summary>
    /// 清零规划位置和实际位置，并进行零漂补偿
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="axis">需要位置清零的起始轴号</param>
    /// <param name="count">需要位置清零的轴数</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_ZeroPos(short cardNum, short axis, short count);

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
    public static extern short GT_SetSoftLimit(short cardNum, short axis,
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
    public static extern short GT_GetSoftLimit(short cardNum, short axis,
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
    public static extern short GT_SetAxisBand(short cardNum, short axis,
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
    public static extern short GT_GetAxisBand(short cardNum, short axis,
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
    public static extern short GT_SetBacklash(short cardNum, short axis,
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
    public static extern short GT_GetBacklash(short cardNum, short axis,
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
    public static extern short GT_GetPrfPos(short cardNum, short profile,
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
    public static extern short GT_GetPrfVel(short cardNum, short profile,
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
    public static extern short GT_GetPrfAcc(short cardNum, short profile,
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
    public static extern short GT_GetPrfMode(short cardNum, short profile,
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
    public static extern short GT_GetAxisPrfPos(short cardNum, short axis,
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
    public static extern short GT_GetAxisPrfVel(short cardNum, short axis,
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
    public static extern short GT_GetAxisPrfAcc(short cardNum, short axis,
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
    public static extern short GT_GetAxisEncPos(short cardNum, short axis,
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
    public static extern short GT_GetAxisEncVel(short cardNum, short axis,
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
    public static extern short GT_GetAxisEncAcc(short cardNum, short axis,
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
    public static extern short GT_GetAxisError(short cardNum, short axis,
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
    public static extern short GT_SetControlFilter(short cardNum,
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
    public static extern short GT_GetControlFilter(short cardNum,
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
    public static extern short GT_SetPid(short cardNum, short control,
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
    public static extern short GT_GetPid(short cardNum, short control,
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
    public static extern short GT_Update(short cardNum, int mask);

    //设置目标位置
    /// <summary>
    /// 设置目标位置
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="profile">规划轴号</param>
    /// <param name="pos">设置目标位置，单位是脉冲</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_SetPos(short cardNum, short profile, int pos);

    //读取目标位置
    /// <summary>
    /// 读取目标位置
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="profile">规划轴号</param>
    /// <param name="pPos">读取目标位置，单位是脉冲</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetPos(short cardNum, short profile, out int pPos);

    //设置目标速度
    /// <summary>
    /// 设置目标速度
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="profile">规划轴号</param>
    /// <param name="vel">设置目标速度，单位是“脉冲/毫秒”</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_SetVel(short cardNum, short profile, double vel);

    //读取目标速度
    /// <summary>
    /// 读取目标速度
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="profile">规划轴号</param>
    /// <param name="pVel">读取目标速度，单位是“脉冲/毫秒”</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetVel(short cardNum, short profile, out double pVel);

    //设置指定轴为点位运动模式
    /// <summary>
    /// 设置指定轴为点位运动模式
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="profile">规划轴号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_PrfTrap(short cardNum, short profile);

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
    public static extern short GT_SetTrapPrm(short cardNum, short profile, ref TTrapPrm pPrm);

    //读取点位模式运动参数
    /// <summary>
    /// 读取点位模式运动参数
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="profile">规划轴号</param>
    /// <param name="pPrm">读取点位模式运动参数</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetTrapPrm(short cardNum, short profile, out TTrapPrm pPrm);

    //设置指定轴为Jog模式
    /// <summary>
    /// 设置指定轴为Jog模式
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="profile">规划轴号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_PrfJog(short cardNum, short profile);

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
    public static extern short GT_SetJogPrm(short cardNum, short profile, ref TJogPrm pPrm);

    //读取Jog运动参数
    /// <summary>
    /// 读取Jog运动参数
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="profile">规划轴号</param>
    /// <param name="pPrm">读取Jog模式指令运动参数</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetJogPrm(short cardNum, short profile, out TJogPrm pPrm);

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
    public static extern short GT_PrfPt(short cardNum, short profile, short mode);

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
    public static extern short GT_SetPtLoop(short cardNum, short profile, int loop);

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
    public static extern short GT_GetPtLoop(short cardNum, short profile, out int pLoop);

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
    public static extern short GT_PtSpace(short cardNum, short profile,
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
    public static extern short GT_PtData(short cardNum, short profile,
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
    public static extern short GT_PtClear(short cardNum, short profile, short fifo);

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
    public static extern short GT_PtStart(short cardNum, int mask, int option);

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
    public static extern short GT_SetPtMemory(short cardNum, short profile, short memory);

    //读取PT运动模式的缓存区大小
    /// <summary>
    /// 读取PT运动模式的缓存区大小
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="profile">规划轴号</param>
    /// <param name="pMemory">读取PT运动缓存区大小标志</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetPtMemory(short cardNum, short profile, out short pMemory);

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
    public static extern short GT_PrfGear(short cardNum, short profile, short dir);

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
    public static extern short GT_SetGearMaster(short cardNum, short profile,
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
    public static extern short GT_GetGearMaster(short cardNum, short profile,
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
    public static extern short GT_SetGearRatio(short cardNum, short profile,
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
    public static extern short GT_GetGearRatio(short cardNum, short profile,
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
    public static extern short GT_GearStart(short cardNum, int mask);

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
    public static extern short GT_PrfFollow(short cardNum, short profile, short dir);

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
    public static extern short GT_SetFollowMaster(short cardNum, short profile,
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
    public static extern short GT_GetFollowMaster(short cardNum, short profile,
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
    public static extern short GT_SetFollowLoop(short cardNum, short profile, int loop);

    //读取Follow模式循环次数
    /// <summary>
    /// 读取Follow模式循环次数
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="profile">规划轴号</param>
    /// <param name="pLoop">读取Follow模式循环已经执行完成的次数</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetFollowLoop(short cardNum, short profile, out int pLoop);

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
    public static extern short GT_SetFollowEvent(short cardNum, short profile,
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
    public static extern short GT_GetFollowEvent(short cardNum, short profile,
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
    public static extern short GT_FollowSpace(short cardNum, short profile,
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
    public static extern short GT_FollowData(short cardNum, short profile,
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
    public static extern short GT_FollowClear(short cardNum, short profile, short fifo);

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
    public static extern short GT_FollowStart(short cardNum, int mask, int option);

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
    public static extern short GT_FollowSwitch(short cardNum, int mask);

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
    public static extern short GT_SetFollowMemory(short cardNum, short profile, short memory);

    //读取Follow运动模式的缓存区大小
    /// <summary>
    /// 读取Follow运动模式的缓存区大小
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="profile">规划轴号</param>
    /// <param name="memory">读取Follow运动缓存区大小标志</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetFollowMemory(short cardNum, short profile, out short memory);

    //下载运动程序到运动控制器
    /// <summary>
    /// 下载运动程序到运动控制器
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="pFileName">下载到运动控制器的运动程序文件名</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_Download(short cardNum, string pFileName);

    //读取运动程序中函数的标识
    /// <summary>
    /// 读取运动程序中函数的标识
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="pFunName">运动程序函数名称</param>
    /// <param name="pFunId">根据运动程序函数名称查询函数标识</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetFunId(short cardNum, string pFunName, out short pFunId);

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
    public static extern short GT_Bind(short cardNum, short thread, short funId, short page);

    //启动线程
    /// <summary>
    /// 启动线程
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="thread">线程编号，取值范围[0,31]</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_RunThread(short cardNum, short thread);

    //停止正在运行的线程
    /// <summary>
    /// 停止正在运行的线程
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="thread">线程编号，取值范围[0,31]</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_StopThread(short cardNum, short thread);

    //暂停正在运行的线程
    /// <summary>
    /// 暂停正在运行的线程
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="thread">线程编号，取值范围[0,31]</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_PauseThread(short cardNum, short thread);

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
    public static extern short GT_GetThreadSts(short cardNum, short thread,
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
    public static extern short GT_GetVarId(short cardNum, string pFunName,
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
    public static extern short GT_SetVarValue(short cardNum, short page,
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
    public static extern short GT_GetVarValue(short cardNum, short page,
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
    public static extern short GT_SetCrdPrm(short cardNum, short crd, ref TCrdPrm pCrdPrm);

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
    public static extern short GT_GetCrdPrm(short cardNum, short crd, out TCrdPrm pCrdPrm);

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
    public static extern short GT_CrdSpace(short cardNum, short crd, out int pSpace, short fifo);

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
    public static extern short GT_CrdData(short cardNum, short crd, short pCrdData, short fifo);

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
    public static extern short GT_CrdStart(short cardNum, short mask, short option);

    //设置插补运动目标合成速度倍率
    /// <summary>
    /// 设置插补运动目标合成速度倍率
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="crd">坐标系号，取值范围：[1,2]</param>
    /// <param name="synVelRatio">设置的插补目标速度倍率，取值范围：(0,1]，系统默认该值为：1</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_SetOverride(short cardNum, short crd, double synVelRatio);

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
    public static extern short GT_InitLookAhead(short cardNum, short crd,
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
    public static extern short GT_CrdClear(short cardNum, short crd, short fifo);

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
    public static extern short GT_CrdStatus(short cardNum, short crd,
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
    public static extern short GT_SetUserSegNum(short cardNum, short crd,
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
    public static extern short GT_GetUserSegNum(short cardNum, short crd,
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
    public static extern short GT_GetRemainderSegNum(short cardNum, short crd,
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
    public static extern short GT_SetCrdStopDec(short cardNum, short crd,
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
    public static extern short GT_GetCrdStopDec(short cardNum, short crd,
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
    public static extern short GT_GetCrdPos(short cardNum, short crd, out double pPos);

    //查询该坐标系的合成速度值
    /// <summary>
    /// 查询该坐标系的合成速度值
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="crd">坐标系号，取值范围：[1,2]</param>
    /// <param name="pSynVel">读取的坐标系的合成速度值，单位：pulse/ms</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetCrdVel(short cardNum, short crd, out double pSynVel);

    //设置指定轴为PVT模式【P-位置，V-速度，T-时间】
    /// <summary>
    /// 设置指定轴为PVT模式【P-位置，V-速度，T-时间】
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="profile">轴号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_PrfPvt(short cardNum, short profile);

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
    public static extern short GT_SetPvtLoop(short cardNum, short profile, int loop);

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
    public static extern short GT_GetPvtLoop(short cardNum, short profile,
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
    public static extern short GT_PvtStatus(short cardNum, short profile,
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
    public static extern short GT_PvtStart(short cardNum, int mask);

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
    public static extern short GT_PvtTableSelect(short cardNum, short profile, short tableId);

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
    public static extern short GT_PvtTable(short cardNum, short tableId,
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
    public static extern short GT_PvtTableEx(short cardNum, short tableId,
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
    public static extern short GT_PvtTableComplete(short cardNum, short tableId,
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
    public static extern short GT_PvtTablePercent(short cardNum, short tableId,
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
    public static extern short GT_PvtPercentCalculate(short cardNum, int count,
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
    public static extern short GT_PvtTableContinuous(short cardNum, short tableId,
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
    public static extern short GT_PvtContinuousCalculate(short cardNum, int count,
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
    public static extern short GT_AlarmOff(short cardNum, short axis);

    //控制轴驱动报警信号有效
    /// <summary>
    /// 控制轴驱动报警信号有效
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="axis">控制轴号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_AlarmOn(short cardNum, short axis);

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
    public static extern short GT_LmtsOn(short cardNum, short axis, short limitType);

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
    public static extern short GT_LmtsOff(short cardNum, short axis, short limitType);

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
    public static extern short GT_ProfileScale(short cardNum, short axis,
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
    public static extern short GT_EncScale(short cardNum, short axis, short alpha, short beta);

    //将脉冲输出通道的脉冲输出模式设置为“脉冲+方向”
    /// <summary>
    /// 将脉冲输出通道的脉冲输出模式设置为“脉冲+方向”
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="step">脉冲输出通道号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_StepDir(short cardNum, short step);

    //将脉冲输出通道的脉冲输出模式设置为“CW/CCW”
    /// <summary>
    /// 将脉冲输出通道的脉冲输出模式设置为“CW/CCW”
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="step">脉冲输出通道号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_StepPulse(short cardNum, short step);

    //设置模拟量输出通道的零漂电压补偿值
    /// <summary>
    /// 设置模拟量输出通道的零漂电压补偿值
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="dac">模拟量输出通道号</param>
    /// <param name="bias">零漂补偿值，取值范围：[-32768,32767]</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_SetMtrBias(short cardNum, short dac, short bias);

    //读取模拟量输出通道的零漂电压补偿值
    /// <summary>
    /// 读取模拟量输出通道的零漂电压补偿值
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="dac">模拟量输出通道号</param>
    /// <param name="pBias">读取的零漂补偿值</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetMtrBias(short cardNum, short dac, out short pBias);

    //设置模拟量输出通道的输出电压饱和极限值
    /// <summary>
    /// 设置模拟量输出通道的输出电压饱和极限值
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="dac">模拟量输出通道号</param>
    /// <param name="limit">输出电压饱和极限值，取值范围：(0,32767]</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_SetMtrLmt(short cardNum, short dac, short limit);

    //读取模拟量输出通道的输出电压饱和极限值
    /// <summary>
    /// 读取模拟量输出通道的输出电压饱和极限值
    /// </summary>
    /// <param name="dac">模拟量输出通道号</param>
    /// <param name="pLimit">读取的输出电压饱和极限值</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetMtrLmt(short dac, out short pLimit);

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
    public static extern short GT_EncSns(short cardNum, ushort sense);

    //设置为“外部编码器”计数方式
    /// <summary>
    /// 设置为“外部编码器”计数方式
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="encoder">编码器通道号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_EncOn(short cardNum, short encoder);

    //设置为“脉冲计数器”计数方式
    /// <summary>
    /// 设置为“脉冲计数器”计数方式
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="encoder">编码器通道号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_EncOff(short cardNum, short encoder);

    //设置跟随误差极限值
    /// <summary>
    /// 设置跟随误差极限值
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="control">伺服控制器编号</param>
    /// <param name="error">跟随误差极限值，取值范围：(0, 2147483648]</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_SetPosErr(short cardNum, short control, int error);

    //读取跟随误差极限值
    /// <summary>
    /// 读取跟随误差极限值
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="control">伺服控制器编号</param>
    /// <param name="pError">读取的跟随误差极限值</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetPosErr(short cardNum, short control, out int pError);

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
    public static extern short GT_SetStopDec(short cardNum, short profile,
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
    public static extern short GT_GetStopDec(short cardNum, short profile,
        out double pDecSmoothStop, out double pDecAbruptStop);

    //设置运动控制器各轴限位开关触发电平
    /// <summary>
    /// 设置运动控制器各轴限位开关触发电平
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="sense">按位标识轴的限位的触发电平状态</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_LmtSns(short cardNum, ushort sense);

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
    public static extern short GT_CtrlMode(short cardNum, short axis, short mode);

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
    public static extern short GT_SetStopIo(short cardNum, short axis,
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
    public static extern short GT_GpiSns(short cardNum, ushort sense);

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
    public static extern short GT_CrdDataCircle(short cardNum, short crd,
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
    public static extern short GT_LnXY(short cardNum, short crd, int x,
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
    public static extern short GT_LnXYZ(short cardNum, short crd, int x,
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
    public static extern short GT_LnXYZA(short cardNum, short crd, int x,
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
    public static extern short GT_LnXYG0(short cardNum, short crd, int x,
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
    public static extern short GT_LnXYZG0(short cardNum, short crd, int x,
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
    public static extern short GT_LnXYZAG0(short cardNum, short crd, int x,
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
    public static extern short GT_ArcXYR(short cardNum, short crd, int x,
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
    public static extern short GT_ArcXYC(short cardNum, short crd, int x,
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
    public static extern short GT_ArcYZR(short cardNum, short crd, int y,
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
    public static extern short GT_ArcYZC(short cardNum, short crd, int y,
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
    public static extern short GT_ArcZXR(short cardNum, short crd, int z,
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
    public static extern short GT_ArcZXC(short cardNum, short crd, int z,
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
    public static extern short GT_BufIO(short cardNum, short crd,
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
    public static extern short GT_BufDelay(short cardNum, short crd,
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
    public static extern short GT_BufDA(short cardNum, short crd,
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
    public static extern short GT_BufLmtsOn(short cardNum, short crd,
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
    public static extern short GT_BufLmtsOff(short cardNum, short crd,
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
    public static extern short GT_BufSetStopIo(short cardNum, short crd,
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
    public static extern short GT_BufMove(short cardNum, short crd, short moveAxis,
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
    public static extern short GT_BufGear(short cardNum, short crd, short gearAxis,
        int pos, short fifo);

    //初始化自动回原点功能
    /// <summary>
    /// 初始化自动回原点功能
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_HomeInit(short cardNum);

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
    public static extern short GT_Home(short cardNum, short axis, int pos,
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
    public static extern short GT_Index(short cardNum, short axis, int pos, int offset);

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
    public static extern short GT_HomeStop(short cardNum, short axis,
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
    public static extern short GT_HomeSts(short cardNum, short axis, out ushort pStatus);

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
    public static extern short GT_ComparePulse(short cardNum, short level,
        short outputType, short time);

    //
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_CompareStop(short cardNum);

    //
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="pStatus"></param>
    /// <param name="pCount"></param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_CompareStatus(short cardNum, out short pStatus, out int pCount);

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
    public static extern short GT_CompareData(short cardNum, short encoder,
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
    public static extern short GT_CompareLinear(short cardNum, short encoder,
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
    public static extern short GT_OpenExtMdlGts(short cardNum, string pDllName);

    //
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_CloseExtMdlGts(short cardNum);

    //
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="card"></param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_SwitchtoCardNoExtMdlGts(short cardNum, short card);

    //
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_ResetExtMdlGts(short cardNum);

    //
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_LoadExtConfigGts(short cardNum, string fileName);

    //
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="mdl"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_SetExtIoValueGts(short cardNum, short mdl, ushort value);

    //
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="mdl"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetExtIoValueGts(short cardNum, short mdl, ref ushort value);

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
    public static extern short GT_SetExtIoBitGts(short cardNum, short mdl, short index, ushort value);

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
    public static extern short GT_GetExtIoBitGts(short cardNum, short mdl, short index, ref ushort value);

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
    public static extern short GT_GetExtAdValueGts(short cardNum, short mdl, short chn, ref ushort value);

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
    public static extern short GT_GetExtAdVoltageGts(short cardNum, short mdl, short chn, ref double value);

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
    public static extern short GT_SetExtDaValueGts(short cardNum, short mdl, short chn, ushort value);

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
    public static extern short GT_SetExtDaVoltageGts(short cardNum, short mdl, short chn, double value);

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
    public static extern short GT_GetStsExtMdlGts(short cardNum, short mdl, short chn, ref ushort value);

    //
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cardNum">运动控制器卡号</param>
    /// <param name="mdl"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [DllImport("gts.dll")]
    public static extern short GT_GetExtDoValueGts(short cardNum, short mdl, ref ushort value);


    #endregion

    #endregion

    }//class

}//namespace