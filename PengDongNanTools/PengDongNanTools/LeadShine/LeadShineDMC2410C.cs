#region "using"

using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#endregion

#region "已处理事项"


#endregion

#region "待处理事项"

//1、设置函数返回类型同雷赛卡函数返回类型，如果没有成功实例化，就返回1，与雷赛卡的错误[未知错误]；【】
//2、因为需要同时执行很多指令，为避免返回值出现交叉影响，故改为单独设置；【】
//3、写一个函数处理运动卡返回的代码，返回错误信息；【】
//4、待了解回原点的方式1~3【即新类的2~4】回原点方式是否与第一个相同，不需要另外写代码；【】

#endregion

namespace PengDongNanTools
    {

    //在DMC2410C函数库中距离或位置的单位为脉冲；速度单位为脉冲/秒；时间单位为秒。
    //雷赛运动卡DMC2410二次开发, 在实例化后可以直接控制轴运动和IO信号【软件作者：彭东南，联系方式：southeastofstar@163.com】
    /// <summary>
    /// 雷赛运动卡DMC2410二次开发,在实例化后可以直接控制轴运动和IO信号
    /// 【软件作者：彭东南，联系方式：southeastofstar@163.com】
    /// </summary>
    class AxisControlOfLeadShineDMC2410C
        {

        //函数参数必须严格保持一致性
        #region "雷赛运动卡DMC2410C#库函数调用列表"

        //为控制卡分配系统资源，并初始化控制卡
        /// <summary>
        /// 为控制卡分配系统资源，并初始化控制卡，设置初始化和速度等设置
        /// </summary>
        /// <returns>0： 没有找到控制卡，或者控制卡异常;
        /// 1到8： 控制卡数;
        /// 负值： 表述有2张或2张以上控制卡的硬件设置卡号相同；返回值取绝对值后减1即为该卡号
        /// 1001 + j: j号卡初始化出错 从1001开始。
        /// </returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_board_init", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt16 d2410_board_init();

        //关闭所有卡，释放系统资源
        /// <summary>
        /// 关闭所有卡，释放系统资源
        /// </summary>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_board_close", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_board_close();

        //复位所有卡，只能在初始化完成之后调用．
        /// <summary>
        /// 复位所有卡，只能在初始化完成之后调用．
        /// </summary>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_board_rest", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_board_rest();

        //脉冲输入输出配置
        //设置指定轴的脉冲输出模式
        /// <summary>
        /// 设置指定轴的脉冲输出模式
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="outmode">脉冲输出方式选择[0~5]
        /// 注意：在调用运动函数（如：d2410_t_vmove等）输出脉冲之前，一定要根据驱动器接
        /// 收脉冲的模式调用d2410_set_pulse_outmode设置控制卡脉冲输出模式。</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_pulse_outmode", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_set_pulse_outmode(UInt16 axis, 
            UInt16 outmode);

        //专用信号设置函数 

        //设置ALM的有效电平及其工作方式
        /// <summary>
        /// 设置ALM的有效电平及其工作方式
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="alm_logic">ALM信号的输入有效电平：0：低电平有效，1：高电平有效</param>
        /// <param name="alm_action">ALM信号的制动方式：0：立即停止，1：减速停止(保留)</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_config_ALM_PIN", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_config_ALM_PIN(UInt16 axis, 
            UInt16 alm_logic, UInt16 alm_action);

        //读取ALM的有效电平及其工作方式
        /// <summary>
        /// 读取ALM的有效电平及其工作方式
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="alm_logic">ALM信号的输入有效电平：0：低电平有效，1：高电平有效</param>
        /// <param name="alm_action">ALM信号的制动方式：0：立即停止，1：减速停止(保留)</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_config_ALM_PIN",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_get_config_ALM_PIN(UInt16 axis,
            ref UInt16 alm_logic, ref UInt16 alm_action);

        //d2410_config_ALM_PIN扩展函数，增加ALM使能状态、控制方式的设定
        /// <summary>
        /// d2410_config_ALM_PIN扩展函数，增加ALM使能状态、控制方式的设定
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="alm_enable">ALM使能状态：0：禁止，1：允许</param>
        /// <param name="alm_logic">ALM信号的输入有效电平：0：低电平有效，1：高电平有效</param>
        /// <param name="alm_all">ALM信号控制方式：0：停止单轴，1：停止所有轴</param>
        /// <param name="alm_action">ALM信号的制动方式：0：立即停止，1：减速停止(保留)</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_config_ALM_PIN_Extern",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_get_config_ALM_PIN_Extern(UInt16 axis,
            ref UInt16 alm_enable, ref UInt16 alm_logic, ref UInt16 alm_all,
            ref UInt16 alm_action);

        //d2410_config_ALM_PIN扩展函数，增加ALM使能状态、控制方式的设定
        /// <summary>
        /// d2410_config_ALM_PIN扩展函数，增加ALM使能状态、控制方式的设定
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="alm_enbale">ALM使能状态：0：禁止，1：允许</param>
        /// <param name="alm_logic">ALM信号的输入有效电平：0：低电平有效，1：高电平有效</param>
        /// <param name="alm_all">ALM信号控制方式：0：停止单轴，1：停止所有轴</param>
        /// <param name="alm_action">ALM信号的制动方式：0：立即停止，1：减速停止(保留)</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_config_ALM_PIN_Extern", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_config_ALM_PIN_Extern(UInt16 axis, 
            UInt16 alm_enbale, UInt16 alm_logic, UInt16 alm_all, UInt16 alm_action);
        
        //设置EL信号的有效电平及制动方式
        /// <summary>
        /// 设置EL信号的有效电平及制动方式
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="el_mode">EL有效电平和制动方式：
        /// 0：立即停、低有效
        /// 1：减速停、低有效
        /// 2：立即停、高有效
        /// 3：减速停、高有效
        /// </param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_config_EL_MODE", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_config_EL_MODE(UInt16 axis, 
            UInt16 el_mode);

        //设置EL信号的使能状态
        /// <summary>
        /// 设置EL信号的使能状态
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="enable">EL信号的使能状态：0：不使能，1：使能</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_Enable_EL_PIN",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_Enable_EL_PIN(UInt16 axis,
            UInt16 enable);
        
        //设置原点ORG信号的有效电平，以及允许/禁止滤波功能
        /// <summary>
        /// 设置原点ORG信号的有效电平，以及允许/禁止滤波功能
        /// 注意：回零运动中，当选择回零模式为0~4时，用该函数设置原点信号有效电平
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="org_logic">ORG信号的有效电平：0：低电平有效，1：高电平有效</param>
        /// <param name="filter">允许/禁止滤波功能：0：禁止，1：允许</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_HOME_pin_logic", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_set_HOME_pin_logic(UInt16 axis, 
            UInt16 org_logic, UInt16 filter);
        
        //输出对指定轴的伺服使能端子的控制
        /// <summary>
        /// 输出对指定轴的伺服使能端子的控制
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="on_off">设定管脚电平状态：0：低，1：高</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_write_SEVON_PIN", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_write_SEVON_PIN(UInt16 axis, 
            UInt16 on_off);
        
        //读取指定轴的伺服使能端子的电平状态
        /// <summary>
        /// 读取指定轴的伺服使能端子的电平状态
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>0：低电平，1：高电平</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_read_SEVON_PIN", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_read_SEVON_PIN(UInt16 axis);
        
        //读取指定运动轴的“伺服准备好”端子的电平状态
        /// <summary>
        /// 读取指定运动轴的“伺服准备好”端子的电平状态
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>0：低电平，1：高电平</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_read_RDY_PIN", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_read_RDY_PIN(UInt16 axis);

        //通用输入/输出控制函数
        
        //读取指定控制卡的某一位输入口的电平状态
        /// <summary>
        /// 读取指定控制卡的某一位输入口的电平状态
        /// </summary>
        /// <param name="cardno">指定控制卡号, 范围（0~N，N为卡号）</param>
        /// <param name="bitno">指定输入口位号（取值范围：1~32）</param>
        /// <returns>0：低电平，1：高电平</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_read_inbit", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_read_inbit(UInt16 cardno, UInt16 bitno);
        
        //对指定控制卡的某一位输出口置位
        /// <summary>
        /// 对指定控制卡的某一位输出口置位
        /// </summary>
        /// <param name="cardno">指定控制卡号, 范围（0~N，N为卡号）</param>
        /// <param name="bitno">指定输出口位号（取值范围：1~20、25~32）</param>
        /// <param name="on_off">输出电平：0：表示输出低电平，1：表示输出高电平</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_write_outbit", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_write_outbit(UInt16 cardno, UInt16 bitno, UInt16 on_off);
        
        //读取指定控制卡的某一位输出口的电平状态
        /// <summary>
        /// 读取指定控制卡的某一位输出口的电平状态
        /// </summary>
        /// <param name="cardno">指定控制卡号, 范围（0~N，N为卡号）</param>
        /// <param name="bitno">指定输出口位号（取值范围：1~20、25~32）</param>
        /// <returns>0：低电平，1：高电平</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_read_outbit", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_read_outbit(UInt16 cardno, UInt16 bitno);
        
        //读取指定控制卡的全部通用输入口的电平状态
        /// <summary>
        /// 读取指定控制卡的全部通用输入口的电平状态
        /// </summary>
        /// <param name="cardno">指定控制卡号, 范围（0~N，N为卡号）</param>
        /// <returns>bit0~bit31位值分别代表第1~32号输入端口值</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_read_inport", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_read_inport(UInt16 cardno);
        
        //读取指定控制卡的全部通用输出口的电平状态
        /// <summary>
        /// 读取指定控制卡的全部通用输出口的电平状态
        /// </summary>
        /// <param name="cardno">指定控制卡号, 范围（0~N，N为卡号）</param>
        /// <returns>bit0~bit19、bit24~bit31位值分别代表第1~20、25~32号输出端口值</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_read_outport", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_read_outport(UInt16 cardno);
        
        //指定控制卡的全部通用输出口的电平状态
        /// <summary>
        /// 指定控制卡的全部通用输出口的电平状态
        /// </summary>
        /// <param name="cardno">指定控制卡号, 范围（0~N，N为卡号）</param>
        /// <param name="port_value">bit0~bit19、bit24~bit31位值分别代表第1~20、25~32号输出端口值</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_write_outport", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_write_outport(UInt16 cardno, 
            UInt32 port_value);

        //制动函数
        
        //指定轴减速停止，调用此函数时立即减速后停止，停止时的速度是起始速度和停止速度中的较大值。
        /// <summary>
        /// 指定轴减速停止，调用此函数时立即减速后停止，停止时的速度是起始速度和停止速度中的较大值。
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="Tdec">减速时间，单位：s</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_decel_stop", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_decel_stop(UInt16 axis, double Tdec);
        
        //使指定轴立即停止，没有任何减速的过程
        /// <summary>
        /// 使指定轴立即停止，没有任何减速的过程
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_imd_stop", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_imd_stop(UInt16 axis);

        ////使所有的运动轴紧急停止
        /// <summary>
        /// 使所有的运动轴紧急停止
        /// </summary>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_emg_stop", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_emg_stop();

        //位置设置和读取函数
        
        //读取指定轴的指令脉冲位置
        /// <summary>
        /// 读取指定轴的指令脉冲位置
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>指定轴的命令脉冲数，单位：pulse</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_position", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_get_position(UInt16 axis);

        //读取指定轴运动的目标脉冲位置[绝对坐标]
        /// <summary>
        /// 读取指定轴运动的目标脉冲位置[绝对坐标]
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>指定轴运动的目标脉冲位置[绝对坐标]，单位：pulse</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_target_position", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_get_target_position(UInt16 axis);
        
        //设置指定轴的指令脉冲位置
        /// <summary>
        /// 设置指定轴的指令脉冲位置
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="current_position">绝对位置脉冲值</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_position", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_set_position(UInt16 axis, 
            Int32 current_position);

        //状态检测函数
        
        //检测指定轴的运动状态，停止或是在运行中
        /// <summary>
        /// 检测指定轴的运动状态，停止或是在运行中
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>0：指定轴正在运行，1：指定轴已停止</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_check_done", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt16 d2410_check_done(UInt16 axis);
        
        //读取指定轴有关运动信号的状态，包含指定轴的专用I/O状态
        /// <summary>
        /// 读取指定轴有关运动信号的状态，包含指定轴的专用I/O状态
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>位号:11, 信号名称: ALM, 1：表示伺服报警信号 ALM 为 ON
        /// 位号:12, 信号名称: PEL, 1：表示正限位信号 +EL 为 ON
        /// 位号:13, 信号名称: NEL, 1：表示负限位信号–EL为 ON
        /// 位号:14, 信号名称: ORG, 1：表示原点信号 ORG 为 ON
        /// 位号:其它, 信号名称: 保留
        /// </returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_axis_io_status", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt16 d2410_axis_io_status(UInt16 axis);
        
        //读取指定轴的外部信号状态
        /// <summary>
        /// 读取指定轴的外部信号状态
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>位号:0~3, 信号名称: 保留
        /// 位号:7, 信号名称: EMG, 1：表示紧急停止信号（EMG）为 ON
        /// 位号:10, 信号名称: EZ, 1：表示索引信号（EZ）为 ON
        /// 位号:11, 信号名称: +DR(PA), 1：表示 +DR(PA) 信号为 ON
        /// 位号:12, 信号名称: -DR(PB), 1：表示 -DR(PB) 信号为 ON
        /// 位号:其他位, 信号名称: 保留
        /// </returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_rsts", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_get_rsts(UInt16 axis);

        //速度设置和读取函数              
        
        //设定插补矢量运动曲线的S段参数、运行速度、加速时间、减速时间
        /// <summary>
        /// 设定插补矢量运动曲线的S段参数、运行速度、加速时间、减速时间
        /// </summary>
        /// <param name="Min_Vel">保留参数</param>
        /// <param name="Max_Vel">最大速度，单位：pulse/s</param>
        /// <param name="Tacc">总加速时间，单位：s</param>
        /// <param name="Tdec">总减速时间，单位：s</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_vector_profile", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_set_vector_profile(double Min_Vel, 
            double Max_Vel, double Tacc, double Tdec);
        
        //设定梯形曲线的起始速度、运行速度、加速时间、减速时间
        /// <summary>
        /// 设定梯形曲线的起始速度、运行速度、加速时间、减速时间
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="Min_Vel">起始速度，单位：pulse/s</param>
        /// <param name="Max_Vel">最大速度，单位：pulse/s</param>
        /// <param name="Tacc">总加速时间，单位：s</param>
        /// <param name="Tdec">总减速时间，单位：s</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_profile", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_set_profile(UInt16 axis, 
            double Min_Vel, double Max_Vel, double Tacc, double Tdec);
        
        //d2410_set_profile扩展函数，增加停止速度的设定
        /// <summary>
        /// d2410_set_profile扩展函数，增加停止速度的设定
        /// </summary>
        /// <param name="axis">参加运动的轴号</param>
        /// <param name="Min_Vel">起始速度，单位：pulse/s</param>
        /// <param name="Max_Vel">最大速度，单位：pulse/s</param>
        /// <param name="Tacc">总加速时间，单位：s</param>
        /// <param name="Tdec">总减速时间，单位：s</param>
        /// <param name="Stop_Vel">停止速度，单位：pulse/s</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_profile_Extern", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_set_profile_Extern(UInt16 axis, 
            double Min_Vel, double Max_Vel, double Tacc, double Tdec, 
            double Stop_Vel);
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="Min_Vel"></param>
        /// <param name="Max_Vel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <param name="Sacc"></param>
        /// <param name="Sdec"></param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_s_profile", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_set_s_profile(UInt16 axis, 
            double Min_Vel, double Max_Vel, double Tacc, double Tdec, 
            Int32 Sacc, Int32 Sdec);
        
        //设定S形曲线运动，起始速度和停止速度相同
        /// <summary>
        /// 设定S形曲线运动，起始速度和停止速度相同
        /// </summary>
        /// <param name="axis">参加运动的轴号</param>
        /// <param name="Min_Vel">起始速度，单位：pulse/s</param>
        /// <param name="Max_Vel">最大速度，单位：pulse/s</param>
        /// <param name="Tacc">总加速时间，单位：s</param>
        /// <param name="Tdec">总减速时间，单位：s</param>
        /// <param name="Tsacc">S段时间，单位：s，范围[0,50] ms</param>
        /// <param name="Tsdec">保留参数</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_st_profile", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_set_st_profile(UInt16 axis, 
            double Min_Vel, double Max_Vel, double Tacc, double Tdec, 
            double Tsacc, double Tsdec);

        //d2410_set_st_profile扩展函数，增加停止速度的设定
        /// <summary>
        /// d2410_set_st_profile扩展函数，增加停止速度的设定
        /// </summary>
        /// <param name="axis">参加运动的轴号</param>
        /// <param name="Min_Vel">起始速度，单位：pulse/s</param>
        /// <param name="Max_Vel">最大速度，单位：pulse/s</param>
        /// <param name="Tacc">总加速时间，单位：s</param>
        /// <param name="Tdec">总减速时间，单位：s</param>
        /// <param name="Tsacc">S段时间，单位：s，范围[0,50] ms</param>
        /// <param name="Tsdec">保留参数</param>
        /// <param name="Stop_Vel">停止速度</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_st_profile_Extern",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_set_st_profile_Extern(UInt16 axis,
            double Min_Vel, double Max_Vel, double Tacc, double Tdec,
            double Tsacc, double Tsdec, double Stop_Vel);

        //读取当前速度值，单位：pulse/s
        /// <summary>
        /// 读取当前速度值，单位：pulse/s
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>指定轴的速度脉冲数, 注 意：当执行插补运动时，调用该函数读取的为矢量速度</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_read_current_speed", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern double d2410_read_current_speed(UInt16 axis);

        //读取指定卡的矢量速度
        /// <summary>
        /// 读取指定卡的矢量速度
        /// </summary>
        /// <param name="card">指定卡号</param>
        /// <returns>卡的矢量速度</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_read_vector_speed", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern double d2410_read_vector_speed(UInt16 card);

        //在线变速/变位
        
        //在线改变指定轴的当前运动速度。该函数只适用于单轴运动中的变速
        /// <summary>
        /// 在线改变指定轴的当前运动速度。该函数只适用于单轴运动中的变速
        /// </summary>
        /// <param name="axis">参加运动的轴号</param>
        /// <param name="Curr_Vel">新的运行速度，单位：pulse/s
        /// 注意：
        /// (1)变速一旦成立，该轴的默认运行速度将会被改写为Curr_Vel，
        ///    也即当调用get_profile回读速度参数时会发生与set_profile所设置的不一致的现象;
        /// (2)在单轴速度运动中Curr_Vel负值表示往负向变速，正值表示往正向变速。 
        ///    在单轴定长运动中Curr_Vel只允许正值。
        /// </param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_change_speed", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_change_speed(UInt16 axis, double Curr_Vel);
        
        //在单轴绝对运动中改变目标位置
        /// <summary>
        /// 在单轴绝对运动中改变目标位置
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="dist">绝对位置值, 注意：参数dist为绝对位置值，无论当前的运动模式为绝对坐标还是相对坐标模式。</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_reset_target_position", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_reset_target_position(UInt16 axis, 
            Int32 dist);

        //单轴定长运动
        
        //使指定轴以对称梯形速度曲线做点位运动
        /// <summary>
        /// 使指定轴以对称梯形速度曲线做点位运动
        /// </summary>
        /// <param name="axis">参加运动的轴号</param>
        /// <param name="Dist">位移量（绝对/相对），单位：pulse</param>
        /// <param name="posi_mode">位移模式设定：0：相对位移，1：绝对位移</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_t_pmove", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_t_pmove(UInt16 axis, 
            Int32 Dist, UInt16 posi_mode);
        
        //使指定轴以非对称梯形速度曲线做点位运动
        /// <summary>
        /// 使指定轴以非对称梯形速度曲线做点位运动
        /// </summary>
        /// <param name="axis">参加运动的轴号</param>
        /// <param name="Dist">位移量（绝对/相对），单位：pulse</param>
        /// <param name="posi_mode">位移模式设定：0：相对位移，1：绝对位移</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_ex_t_pmove", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_ex_t_pmove(UInt16 axis, 
            Int32 Dist, UInt16 posi_mode);
        
        //使指定轴以对称S形速度曲线做点位运动
        /// <summary>
        /// 使指定轴以对称S形速度曲线做点位运动
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="Dist">位移量（绝对/相对），单位：pulse</param>
        /// <param name="posi_mode">位移模式设定：0：相对位移，1：绝对位移</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_s_pmove", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_s_pmove(UInt16 axis, 
            Int32 Dist, UInt16 posi_mode);
        
        //使指定轴以非对称S形速度曲线做点位运动
        /// <summary>
        /// 使指定轴以非对称S形速度曲线做点位运动
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="Dist">位移量（绝对/相对），单位：pulse</param>
        /// <param name="posi_mode">位移模式设定：0：相对位移，1：绝对位移</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_ex_s_pmove", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_ex_s_pmove(UInt16 axis, 
            Int32 Dist, UInt16 posi_mode);

        //单轴连续运动
        
        //使指定轴以S形速度曲线加速到高速，并持续运行下去
        /// <summary>
        /// 使指定轴以S形速度曲线加速到高速，并持续运行下去
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="dir">指定运动的方向：0：负方向，1：正方向</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_s_vmove", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_s_vmove(UInt16 axis, UInt16 dir);
        
        //使指定轴以T形速度曲线加速到高速，并持续运行下去
        /// <summary>
        /// 使指定轴以T形速度曲线加速到高速，并持续运行下去
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="dir">指定运动的方向：0：负方向，1：正方向</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_t_vmove", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_t_vmove(UInt16 axis, UInt16 dir);

        //直线插补
                
        //指定任意两轴以对称的梯形速度曲线做插补运动
        /// <summary>
        /// 指定任意两轴以对称的梯形速度曲线做插补运动
        /// </summary>
        /// <param name="axis1">指定两轴插补的第一轴</param>
        /// <param name="Dist1">指定axis1的位移值，单位：pulse</param>
        /// <param name="axis2">指定两轴插补的第二轴</param>
        /// <param name="Dist2">指定axis2的位移值，单位：pulse</param>
        /// <param name="posi_mode">位移模式设定：0：相对位移，1：绝对位移</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_t_line2", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_t_line2(UInt16 axis1, Int32 Dist1, 
            UInt16 axis2, Int32 Dist2, UInt16 posi_mode);
        
        //指定任意三轴以对称的梯形速度曲线做插补运动
        /// <summary>
        /// 指定任意三轴以对称的梯形速度曲线做插补运动
        /// </summary>
        /// <param name="axis">轴号列表的指针</param>
        /// <param name="Dist1">指定axis[0]轴的位移值，单位：pulse</param>
        /// <param name="Dist2">指定axis[1]轴的位移值，单位：pulse</param>
        /// <param name="Dist3">指定axis[2]轴的位移值，单位：pulse</param>
        /// <param name="posi_mode">位移模式设定：0：相对位移，1：绝对位移</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_t_line3", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_t_line3(UInt16[] axis, 
            Int32 Dist1, Int32 Dist2, Int32 Dist3, UInt16 posi_mode);
        
        //指定四轴以对称的梯形速度曲线做插补运动
        /// <summary>
        /// 指定四轴以对称的梯形速度曲线做插补运动
        /// </summary>
        /// <param name="cardno">指定插补运动的板卡号, 范围（0~N，N为卡号）</param>
        /// <param name="Dist1">指定第一轴的位移值，单位：pulse</param>
        /// <param name="Dist2">指定第二轴的位移值，单位：pulse</param>
        /// <param name="Dist3">指定第三轴的位移值，单位：pulse</param>
        /// <param name="Dist4">指定第四轴的位移值，单位：pulse</param>
        /// <param name="posi_mode">位移模式设定：0：相对位移，1：绝对位移</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_t_line4", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_t_line4(UInt16 cardno, 
            Int32 Dist1, Int32 Dist2, Int32 Dist3, Int32 Dist4, UInt16 posi_mode);

        //圆弧插补
        
        //指定任意的两轴以当前位置为起点，按指定的圆心、目标绝对位置和方向作圆弧插补运动
        /// <summary>
        /// 指定任意的两轴以当前位置为起点，按指定的圆心、目标绝对位置和方向作圆弧插补运动
        /// </summary>
        /// <param name="axis">轴号列表指针</param>
        /// <param name="target_pos">目标绝对位置列表指针，单位：pulse</param>
        /// <param name="cen_pos">圆心绝对位置列表指针，单位：pulse</param>
        /// <param name="arc_dir">圆弧方向：0：顺时针，1：逆时针</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_arc_move", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_arc_move(UInt16[] axis, 
            Int32[] target_pos, Int32[] cen_pos, UInt16 arc_dir);

        //指定任意的两轴以当前位置为起点，按指定的圆心、目标相对位置和方向作圆弧插补运动
        /// <summary>
        /// 指定任意的两轴以当前位置为起点，按指定的圆心、目标相对位置和方向作圆弧插补运动
        /// </summary>
        /// <param name="axis">轴号列表指针</param>
        /// <param name="rel_pos">目标相对位置列表指针, 单位：pulse</param>
        /// <param name="rel_cen">圆心相对位置列表指针, 单位：pulse</param>
        /// <param name="arc_dir">圆弧方向：0：顺时针，1：逆时针</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_rel_arc_move", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_rel_arc_move(UInt16[] axis, 
            Int32[] rel_pos, Int32[] rel_cen, UInt16 arc_dir);

        //手轮运动
        
        //设置输入手轮脉冲信号的计数方式
        /// <summary>
        /// 设置输入手轮脉冲信号的计数方式
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="inmode">表示输入方式：0：A、B相位正交计数，1：双脉冲信号</param>
        /// <param name="multi">计数器的计数方向及倍率设置：设置手轮的倍率, 正数表示默认方向，负数表示与默认方向相反</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_handwheel_inmode", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_set_handwheel_inmode(UInt16 axis, 
            UInt16 inmode, double multi);
        
        //启动指定轴的手轮脉冲运动
        /// <summary>
        /// 启动指定轴的手轮脉冲运动,
        /// 注 意：当启动手轮运动后，只有发送d2410_decel_stop或d2410_imd_stop命令后才会退出手轮模式
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_handwheel_move", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_handwheel_move(UInt16 axis);

        //找原点
        
        //设定指定轴的回原点模式
        /// <summary>
        /// 设定指定轴的回原点模式
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="mode">回原点的信号模式：
        /// 0：只计home 
        /// 1：计home和EZ，计1个EZ信号 
        /// 2：一次回零加回找 
        /// 3：二次回零
        /// 4：EZ单独回零
        /// 5：原点捕获回零
        /// </param>
        /// <param name="EZ_count">EZ信号出现EZ_count指定的次数后，轴运动停止。
        /// 仅当mode=4时该参数设置有效，取值范围：1－16</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_config_home_mode", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_config_home_mode(UInt16 axis, 
            UInt16 mode, UInt16 EZ_count);
        
        //单轴回原点运动
        /// <summary>
        /// 单轴回原点运动
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="home_mode">回原点方式：1：正方向回原点，2：负方向回原点</param>
        /// <param name="vel_mode">回原点速度：0：低速回原点，1：高速回原点</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_home_move", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_home_move(UInt16 axis, 
            UInt16 home_mode, UInt16 vel_mode);

        //原点锁存

        //设置/读取原点锁存方式
        /// <summary>
        /// 设置/读取原点锁存方式, 注意：回零运动中，当选择回零模式为5时，用d2410_set_homelatch_mode函数设置原点信号锁存方式
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="enable">原点锁存使能状态：0：禁止，2：允许</param>
        /// <param name="logic">原点锁存方式：0：下降沿锁存，1：上升沿锁存</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_homelatch_mode", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_set_homelatch_mode(UInt16 axis, 
            UInt16 enable, UInt16 logic);
        
        //设置/读取原点锁存方式
        /// <summary>
        /// 设置/读取原点锁存方式, 注意：回零运动中，当选择回零模式为5时，用d2410_set_homelatch_mode函数设置原点信号锁存方式
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="enable">原点锁存使能状态：0：禁止，2：允许</param>
        /// <param name="logic">原点锁存方式：0：下降沿锁存，1：上升沿锁存</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_homelatch_mode", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_get_homelatch_mode(UInt16 axis, ref UInt16 enable, ref UInt16 logic);
        
        //读取原点锁存标志
        /// <summary>
        /// 读取原点锁存标志
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>原点锁存标志：0：未触发，1：触发</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_homelatch_flag", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_get_homelatch_flag(UInt16 axis);
        
        //复位原点锁存标志
        /// <summary>
        /// 复位原点锁存标志
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_reset_homelatch_flag", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_reset_homelatch_flag(UInt16 axis);

        //读取原点锁存值（原点锁存位置为指令脉冲位置）
        /// <summary>
        /// 读取原点锁存值（原点锁存位置为指令脉冲位置）
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>原点锁存值</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_homelatch_value", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_get_homelatch_value(UInt16 axis);

        //多组位置比较函数
        
        //设置比较器配置
        /// <summary>
        /// 设置比较器配置
        /// </summary>
        /// <param name="card">卡号</param>
        /// <param name="queue">比较队列号：0、1</param>
        /// <param name="enable">1：使能比较功能，0：禁止比较功能</param>
        /// <param name="axis">轴号</param>
        /// <param name="cmp_source">比较源：0：比较指令位置，1：比较编码器位置</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_compare_config_Extern", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_compare_config_Extern(UInt16 card, 
            UInt16 queue, UInt16 enable, UInt16 axis, UInt16 cmp_source);
        
        //读取比较器配置
        /// <summary>
        /// 读取比较器配置
        /// </summary>
        /// <param name="card">卡号</param>
        /// <param name="queue">比较队列号：0、1</param>
        /// <param name="enable">1：使能比较功能，0：禁止比较功能</param>
        /// <param name="axis">轴号</param>
        /// <param name="cmp_source">比较源：0：比较指令位置，1：比较编码器位置</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_compare_get_config_Extern", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_compare_get_config_Extern(UInt16 card, 
            UInt16 queue, ref UInt16 enable, ref UInt16 axis, ref UInt16 cmp_source);
        
        //清除所有比较点
        /// <summary>
        /// 清除所有比较点
        /// </summary>
        /// <param name="card">卡号</param>
        /// <param name="queue">比较队列号：0、1</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_compare_clear_points_Extern", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_compare_clear_points_Extern(UInt16 card, UInt16 queue);
        
        //添加比较点
        /// <summary>
        /// 添加比较点
        /// </summary>
        /// <param name="card">卡号</param>
        /// <param name="queue">比较队列号：0、1</param>
        /// <param name="pos">位置坐标</param>
        /// <param name="dir">比较方向：0：小于等于，1：大于等于</param>
        /// <param name="action">比较点触发功能</param>
        /// <param name="actpara">比较点触发功能参数
        /// action:1 , actpara: IO号, 功能: IO置为低电平
        /// action:2 , actpara: IO号, 功能: IO置为高电平
        /// action:3 , actpara: IO号, 功能: 取反IO
        /// action:5 , actpara: IO号, 功能: 输出100us 脉冲
        /// action:6 , actpara: IO号, 功能: 输出1ms 脉冲
        /// action:7 , actpara: IO号, 功能: 输出10ms 脉冲
        /// action:8 , actpara: IO号, 功能: 输出100ms 脉冲
        /// action:11 , actpara: 速度值, 功能: 当前轴变速
        /// action:13 , actpara: 轴号, 功能: 停止指定轴
        /// </param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_compare_add_point_Extern", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_compare_add_point_Extern(UInt16 card, 
            UInt16 queue, UInt32 pos, UInt16 dir, UInt16 action, UInt32 actpara);
        
        //读取当前比较点位置
        /// <summary>
        /// 读取当前比较点位置
        /// </summary>
        /// <param name="card">卡号</param>
        /// <param name="queue">比较队列号：0、1</param>
        /// <returns>当前比较点的位置值</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_compare_get_current_point_Extern", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_compare_get_current_point_Extern(UInt16 card, UInt16 queue);
        
        //查询已经比较过的点个数
        /// <summary>
        /// 查询已经比较过的点个数
        /// </summary>
        /// <param name="card">卡号</param>
        /// <param name="queue">比较队列号：0、1</param>
        /// <returns>已经比较过的比较点的数量</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_compare_get_points_runned_Extern", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_compare_get_points_runned_Extern(UInt16 card, UInt16 queue);

        //设置位置误差带
        /// <summary>
        /// 设置位置误差带
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="factor">编码器系数（脉冲当量）
        /// 说 明：假设当前编码器脉冲数为200，指令脉冲数为1002， 当误差带设为2，编码器系数设为5，
        /// 处理如下： 200*5=1000,1000-1002=-2,在误差带范围[-2,2]之内，此时认为编码器到位。
        /// </param>
        /// <param name="errormsg">位置误差带</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_factor_error",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_set_factor_error(UInt16 axis,
            double factor, Int32 errormsg);

        //读取位置误差带
        /// <summary>
        /// 读取位置误差带
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="factor">编码器系数（脉冲当量）</param>
        /// <param name="errormsg">位置误差带
        /// 说 明：假设当前编码器脉冲数为200，指令脉冲数为1002， 当误差带设为2，编码器系数设为5，
        /// 处理如下： 200*5=1000,1000-1002=-2,在误差带范围[-2,2]之内，此时认为编码器到位。
        /// </param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_factor_error",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_get_factor_error(UInt16 axis,
            ref double factor, ref Int32 errormsg);

        //查询可以加入的比较点数量
        /// <summary>
        /// 查询可以加入的比较点数量
        /// </summary>
        /// <param name="card">卡号</param>
        /// <param name="queue">比较队列号：0、1</param>
        /// <returns>剩余可用的比较点数量</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_compare_get_points_remained_Extern", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_compare_get_points_remained_Extern(UInt16 card, UInt16 queue);

        //高速位置比较
        
        //配置高速位置比较器
        /// <summary>
        /// 配置高速位置比较器
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="cmp_enable">比较器使能状态：0：禁止，1：使能</param>
        /// <param name="cmp_pos">比较位置值</param>
        /// <param name="CMP_logic">CMP输出有效电平：0：低电平，1：高电平
        /// 注 意：当设置CMP比较器后，相应CMP输出口的电平会变为与设置的电平相反；
        /// 当位置触发时，CMP端口会输出一个脉冲信号（1~2ms）。
        /// </param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_config_CMP_PIN", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_config_CMP_PIN(UInt16 axis, 
            UInt16 cmp_enable, Int32 cmp_pos, UInt16 CMP_logic);
        
        //读取高速位置比较器
        /// <summary>
        /// 读取高速位置比较器
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="cmp_enable">比较器使能状态：0：禁止，1：使能</param>
        /// <param name="cmp_pos">比较位置值</param>
        /// <param name="CMP_logic">CMP输出有效电平：0：低电平，1：高电平
        /// 注 意：当设置CMP比较器后，相应CMP输出口的电平会变为与设置的电平相反；
        /// 当位置触发时，CMP端口会输出一个脉冲信号（1~2ms）。
        /// </param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_config_CMP_PIN", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_get_config_CMP_PIN(UInt16 axis, 
            ref UInt16 cmp_enable, ref Int32 cmp_pos, ref UInt16 CMP_logic);
        
        //读取高速位置比较输出口状态
        /// <summary>
        /// 读取高速位置比较输出口状态
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>1-高电平，0-低电平</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_read_CMP_PIN", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_read_CMP_PIN(UInt16 axis);
        
        //设置高速位置比较输出口状态
        /// <summary>
        /// 设置高速位置比较输出口状态
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="on_off">1-高电平，0-低电平</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_write_CMP_PIN", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_write_CMP_PIN(UInt16 axis, UInt16 on_off);

        //编码器计数功能
        
        //读取指定轴编码器反馈位置脉冲计数值，范围：28位有符号数
        /// <summary>
        /// 读取指定轴编码器反馈位置脉冲计数值，范围：28位有符号数
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>位置反馈脉冲值，单位：pulse</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_encoder", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_get_encoder(UInt16 axis);
        
        //设置指定轴编码器反馈脉冲计数值，范围：28位有符号数
        /// <summary>
        /// 设置指定轴编码器反馈脉冲计数值，范围：28位有符号数
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="encoder_value">编码器的设定值</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_encoder", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_set_encoder(UInt16 axis, 
            UInt32 encoder_value);
        
        //设置指定轴的EZ信号的有效电平及其作用
        /// <summary>
        /// 设置指定轴的EZ信号的有效电平及其作用
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="ez_logic">EZ信号有效电平：0：低有效，1：高有效</param>
        /// <param name="ez_mode">EZ信号的工作方式：0：EZ信号无效，1：EZ是计数器复位信号</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_config_EZ_PIN", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_config_EZ_PIN(UInt16 axis, 
            UInt16 ez_logic, UInt16 ez_mode);
        
        //读取指定控制卡的计数器的标识位
        /// <summary>
        /// 读取指定控制卡的计数器的标识位
        /// </summary>
        /// <param name="cardno">指定控制卡号</param>
        /// <returns>返回值位号 0: 指定卡的X轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 1: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 2: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 3: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 4~7: 保留
        /// 返回值位号 8: 指定卡的Y轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 9: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 10: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 11: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 12~15: 保留
        /// 返回值位号 16: 指定卡的Z轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 17: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 18: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 19: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 20~23: 保留
        /// 返回值位号 24: 指定卡的U轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 25: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 26: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 27: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 28~31: 保留
        /// </returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_counter_flag", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_get_counter_flag(UInt16 cardno);
        
        //复位计数器的计数标志位, 范围（0~N，N为卡号）
        /// <summary>
        /// 复位计数器的计数标志位, 范围（0~N，N为卡号）
        /// </summary>
        /// <param name="cardno">指定控制卡号</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_reset_counter_flag", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_reset_counter_flag(UInt16 cardno);
        
        //复位计数器的清零标志位, 范围（0~N，N为卡号）
        /// <summary>
        /// 复位计数器的清零标志位, 范围（0~N，N为卡号）
        /// </summary>
        /// <param name="cardno">指定控制卡号</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_reset_clear_flag", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_reset_clear_flag(UInt16 cardno);

        //高速锁存
        
        //设置指定轴“锁存”信号的有效电平及其工作方式
        /// <summary>
        /// 设置指定轴“锁存”信号的有效电平及其工作方式
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="ltc_logic">LTC信号有效电平：0：低有效，1：高有效</param>
        /// <param name="ltc_mode">保留参数</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_config_LTC_PIN", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_config_LTC_PIN(UInt16 axis, 
            UInt16 ltc_logic, UInt16 ltc_mode);

        //读取指定轴“锁存”信号的有效电平及其工作方式
        /// <summary>
        /// 读取指定轴“锁存”信号的有效电平及其工作方式
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="ltc_logic">LTC信号有效电平：0：低有效，1：高有效</param>
        /// <param name="ltc_mode">保留参数</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_config_LTC_PIN",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_get_config_LTC_PIN(UInt16 axis,
            ref UInt16 ltc_logic, ref UInt16 ltc_mode);

        //d2410_config_LTC_PIN扩展函数，增加滤波时间的设定
        /// <summary>
        /// d2410_config_LTC_PIN扩展函数，增加滤波时间的设定
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="ltc_logic">LTC信号有效电平：0：低有效，1：高有效</param>
        /// <param name="ltc_mode">保留参数</param>
        /// <param name="ltc_filter">滤波时间，单位：ms</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_config_LTC_PIN_Extern",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_get_config_LTC_PIN_Extern(UInt16 axis,
            ref UInt16 ltc_logic, ref UInt16 ltc_mode, ref double ltc_filter);

        //d2410_config_LTC_PIN扩展函数，增加滤波时间的设定
        /// <summary>
        /// d2410_config_LTC_PIN扩展函数，增加滤波时间的设定
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="ltc_logic">LTC信号有效电平：0：低有效，1：高有效</param>
        /// <param name="ltc_mode">保留参数</param>
        /// <param name="ltc_filter">滤波时间，单位：ms</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_config_LTC_PIN_Extern", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_config_LTC_PIN_Extern(UInt16 axis, 
            UInt16 ltc_logic, UInt16 ltc_mode, double ltc_filter);
        
        //设置锁存方式为单轴锁存或是四轴同时锁存
        /// <summary>
        /// 设置锁存方式为单轴锁存或是四轴同时锁存
        /// </summary>
        /// <param name="cardno">指定控制卡号[0~N]</param>
        /// <param name="all_enable">锁存方式 ：0：单独锁存，1：四轴同时锁存
        /// 注 意：位置锁存暂时只支持反馈位置锁存
        /// </param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_config_latch_mode", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_config_latch_mode(UInt16 cardno, 
            UInt16 all_enable);
        
        //设置编码器的计数方式
        /// <summary>
        /// 设置编码器的计数方式
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="mode">编码器的计数方式：
        /// 0：非A/B相 (脉冲/方向)
        /// 1：1×A/B
        /// 2：2× A/B
        /// 3：4× A/B
        /// </param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_counter_config", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_counter_config(UInt16 axis, UInt16 mode);
        
        //读取编码器锁存器的值
        /// <summary>
        /// 读取编码器锁存器的值
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>锁存器内的编码器脉冲数，单位：pulse
        /// 注 意：在位置锁存中，多次触发高速锁存口只锁存第一次触发位置，
        /// 只有调用函数清除锁存状态方可再次锁存
        /// </returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_latch_value", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_get_latch_value(UInt16 axis);
        
        //读取指定控制卡的锁存器的标志位
        /// <summary>
        /// 读取指定控制卡的锁存器的标志位
        /// </summary>
        /// <param name="cardno">指定控制卡号, 范围（0~N，N为卡号）</param>
        /// <returns>返回值位号: 0, 描述: 0：指定卡的0轴有触发信号
        /// 返回值位号: 1, 描述: 0：1轴有触发信号
        /// 返回值位号: 2, 描述: 0：2轴有触发信号
        /// 返回值位号: 3, 描述: 0：3轴有触发信号
        /// 返回值位号: 4, 描述: 1：指定卡的0轴有清零信号
        /// 返回值位号: 5, 描述: 1：1轴有清零信号
        /// 返回值位号: 6, 描述: 1：2轴有清零信号
        /// 返回值位号: 7, 描述: 1：3轴有清零信号
        /// 返回值位号: 8, 描述: 1：0轴位置已锁存
        /// 返回值位号: 9, 描述: 1：1轴位置已锁存
        /// 返回值位号: 10, 描述: 1：2轴位置已锁存
        /// 返回值位号: 11, 描述: 1：3轴位置已锁存
        /// 返回值位号: 12, 描述: 1：指定卡的0轴位置已清零
        /// 返回值位号: 13, 描述: 1：1轴位置已清零
        /// 返回值位号: 14, 描述: 1：2轴位置已清零
        /// 返回值位号: 15, 描述: 1：3轴位置已清零
        /// 返回值位号: 16~31, 描述:保留
        /// </returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_latch_flag", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_get_latch_flag(UInt16 cardno);
        
        //复位指定控制卡的锁存器的标志位
        /// <summary>
        /// 复位指定控制卡的锁存器的标志位
        /// </summary>
        /// <param name="cardno">指定控制卡号, 范围（0~N，N为卡号）</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_reset_latch_flag", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_reset_latch_flag(UInt16 cardno);
        
        //选择全部锁存时的外触发信号通道；可以由二个信号通道输入，即LTC1, LTC2；默认为LTC1
        /// <summary>
        /// 选择全部锁存时的外触发信号通道；可以由二个信号通道输入，即LTC1, LTC2；默认为LTC1
        /// </summary>
        /// <param name="cardno">卡号</param>
        /// <param name="num">信号通道选择号：0：LTC1锁存四个轴，1：LTC2锁存四个轴</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_triger_chunnel", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_triger_chunnel(UInt16 cardno, 
            UInt16 num);

        //选择编码器Speaker和LED的输出逻辑，默认为低有效
        /// <summary>
        /// 选择编码器Speaker和LED的输出逻辑，默认为低有效
        /// </summary>
        /// <param name="cardno">卡号</param>
        /// <param name="logic">0：低有效，1：高有效</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_speaker_logic", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_set_speaker_logic(UInt16 cardno, 
            UInt16 logic);

        //检测指令到位【编码器】
        /// <summary>
        /// 检测指令到位【编码器】
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>0：编码器位置在设定的目标位置的误差带之外；
        /// 1：编码器位置在设定的目标位置的误差带之内</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_check_success_encoder",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_check_success_encoder(UInt16 axis);

        //检测指令到位【脉冲】
        /// <summary>
        /// 检测指令到位【脉冲】
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <returns>0：指令位置在设定的目标位置的误差带之外；
        /// 1：指令位置在设定的目标位置的误差带之内</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_check_success_pulse",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_check_success_pulse(UInt16 axis);

        //EMG设置
        
        //EMG信号设置，急停信号有效后会立即停止所有轴
        /// <summary>
        /// EMG信号设置，急停信号有效后会立即停止所有轴
        /// </summary>
        /// <param name="cardno">运动卡卡号</param>
        /// <param name="enable">0：无效，1：有效</param>
        /// <param name="emg_logic">0：低有效，1：高有效</param>
        /// <returns>错误代码</returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_config_EMG_PIN", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_config_EMG_PIN(UInt16 cardno, 
            UInt16 enable, UInt16 emg_logic);

        //软件限位功能
        
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="ON_OFF"></param>
        /// <param name="source_sel"></param>
        /// <param name="SL_action"></param>
        /// <param name="N_limit"></param>
        /// <param name="P_limit"></param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_config_softlimit", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_config_softlimit(UInt16 axis, 
            UInt16 ON_OFF, UInt16 source_sel, UInt16 SL_action, Int32 N_limit, 
            Int32 P_limit);
                
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="ON_OFF"></param>
        /// <param name="source_sel"></param>
        /// <param name="SL_action"></param>
        /// <param name="N_limit"></param>
        /// <param name="P_limit"></param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_config_softlimit", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_get_config_softlimit(UInt16 axis, 
            ref UInt16 ON_OFF, ref UInt16 source_sel, ref UInt16 SL_action, 
            ref Int32 N_limit, ref Int32 P_limit);

        //脉冲当量设置

        //读取脉冲当量【移动1毫米所需要的脉冲数量】
        /// <summary>
        /// 读取脉冲当量【移动1毫米所需要的脉冲数量】
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="equiv">移动1毫米所需要的脉冲数量</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_equiv", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_get_equiv(UInt16 axis, ref double equiv);


        //设置脉冲当量【移动1毫米所需要的脉冲数量】
        /// <summary>
        /// 设置脉冲当量【移动1毫米所需要的脉冲数量】
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="new_equiv">移动1毫米所需要的脉冲数量</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_equiv", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_set_equiv(UInt16 axis, double new_equiv);

        //读取轴的当前位置【单位：mm】
        /// <summary>
        /// 读取轴的当前位置【单位：mm】
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="pos_by_mm">轴的当前位置【单位：mm】</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_position_unitmm", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_get_position_unitmm(UInt16 axis, ref double pos_by_mm);

        //设定指定轴的当前位置【单位：mm】【常用于回到原点后位置清零】
        /// <summary>
        /// 设定指定轴的当前位置【单位：mm】【常用于回到原点后位置清零】
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="pos_by_mm">轴的位置【单位：mm】</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_position_unitmm", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_set_position_unitmm(UInt16 axis, double pos_by_mm);

        //读取当前速度值，单位：mm/s
        /// <summary>
        /// 读取当前速度值，单位：mm/s
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="current_speed">当前速度值，单位：mm/s</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_read_current_speed_unitmm", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_read_current_speed_unitmm(UInt16 axis, 
            ref double current_speed);

        //读取轴编码器反馈位置【单位：mm】
        /// <summary>
        /// 读取轴编码器反馈位置【单位：mm】
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="encoder_pos_by_mm">轴编码器反馈位置【单位：mm】</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_get_encoder_unitmm", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_get_encoder_unitmm(UInt16 axis, 
            ref double encoder_pos_by_mm);

        //设置轴编码器反馈数值【单位：mm】
        /// <summary>
        /// 设置轴编码器反馈数值【单位：mm】
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="encoder_pos_by_mm">轴编码器反馈数设置值【单位：mm】</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_set_encoder_unitmm", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 d2410_set_encoder_unitmm(UInt16 axis, 
            double encoder_pos_by_mm);
        
        //两轴圆弧绝对位置插补【单位：mm】
        /// <summary>
        /// 两轴圆弧绝对位置插补【单位：mm】
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="target_pos">绝对位置列表指针, 单位：mm</param>
        /// <param name="cen_pos">圆心绝对位置列表指针, 单位：mm</param>
        /// <param name="arc_dir">圆弧方向：0：顺时针，1：逆时针</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_arc_move_unitmm", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_arc_move_unitmm(UInt16[] axis, 
            double[] target_pos, double[] cen_pos, UInt16 arc_dir);

        //两轴圆弧相对位置插补【单位：mm】
        /// <summary>
        /// 两轴圆弧相对位置插补【单位：mm】
        /// </summary>
        /// <param name="axis">指定轴号</param>
        /// <param name="rel_pos">目标相对位置列表指针, 单位：mm</param>
        /// <param name="rel_cen">圆心相对位置列表指针, 单位：mm</param>
        /// <param name="arc_dir">圆弧方向：0：顺时针，1：逆时针</param>
        /// <returns></returns>
        [DllImport("DMC2410.dll", EntryPoint = "d2410_rel_arc_move_unitmm", 
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 d2410_rel_arc_move_unitmm(UInt16[] axis, 
            double[] rel_pos, double[] rel_cen, UInt16 arc_dir);
        
        #endregion

        #region "雷赛运动卡说明"

        //【多卡运行】
        //DMC2410C运动控制卡的驱动程序支持最多8块DMC2410C卡同时工作。因此，一台PC机可以同时控制多达32个电机同时运动。 
        //DMC2410C卡支持即插即用模式，用户可不必去关心如何设置卡的基地址和IRQ中断值。在使用多块运动控制卡时，首先要
        //用运动控制卡上的拨码开关设置卡号；系统启动后，系统BIOS为相应的卡自动分配物理空间。 
        
        //【卡号和轴号的对应关系为】：
        //                       0号卡对应0~3号轴；1号卡对应4~7号轴；n号卡对应4n~ 4*（n+1）-1号轴。

        //DMC2410C运动控制卡有4路电机控制信号接口。脉冲信号PUL和方向信号DIR均为差分输出信号。也可以配置为单端输出信号。
        
        //DMC2410C卡可以输出两类指令脉冲信号：
        //       一种为脉冲+方向模式（单脉冲模式）
        //       一种为正脉冲+负脉冲模式（双脉冲模式）
        
        //1）编码器等脉冲输入信号的EA+、EA-、EB+、EB-和EZ+、EZ-的差分信号电压差必须高于3.5V，小于5V，
        //且输出电流不应小于6mA。 
        //2）需要将输入设备的地线和控制卡的GND连接。
        //3）编码器信号接口具体引脚分配情况请见附录1中的插座X2定义表。
        
        //DMC2410C为每个轴都提供了1个原点位置传感器信号的输入端口ORG
        
        //DMC2410C为每个轴提供了2个机械限位信号EL+ 和 EL-，EL+为正向限位信号，EL-为反向限位信号。
        //当运动平台触发限位开关时，EL+或EL-即有效，DMC2410C将禁止运动平台继续向前运动。
        //注意：
        //1. 用户需根据使用的限位开关类型来设置限位开关的有效工作电平。
        //当使用常开型限位开关时，应通过软件选择EL+、EL-信号为低电平有效；
        //当使用常闭型限位开关时，应选择EL+、EL-信号为高电平有效。
        
        //DMC2410C运动控制卡信号RDY和ALM用于监控伺服电机状态，信号SEVON用于设置伺服电机状态。
        
        //位置锁存信号输入接口
        //DMC2410C运动控制卡每一轴都提供一个位置锁存输入信号LTC，信号LTC1～LTC4可以分别锁存4个轴的
        //当前编码器位置或指令位置；也可以通过软件设置，由LTC1或LTC2信号同时锁存4个轴的位置.
        
        //通用数字输入信号接口
        //DMC2410C卡为用户提供了多达32路通用数字输入信号（其中4路和RDY信号复用）
        
        //通用数字输出信号接口
        //DMC2410C卡为用户提供了多达28路通用数字输出接口（其中包括4路和专用信号接口复用），由ULN2803芯片驱动，
        //其最大工作电流为50 mA（5～40Vdc，吸入），可用于控制继电器、电磁阀、信号灯或其它设备。 
        //OUT 1～OUT 16端口可设置上电时的初始电平，OUT17～OUT20上电初始电平为高。
        
        //在软件编程中，DMC2410C运动控制卡上的4个轴的编号和硬件编号不一样，不是从0开始，而是1开始。
        
        //回原点步骤
        //在进行精确的运动控制之前，需要设定运动坐标系的原点。运动平台上都设有原点传感器（也称为原点开关），
        //寻找原点开关的位置并将该位置设为平台的坐标原点，即为回原点运动。
        
        //DMC2410C控制卡共提供了6种回原点方式:
        //其中方式1~5是采用原点电平状态作触发信号，
        //方式6则是采用原点边沿信号作触发信号；
        
        //具体回原点运动主要步骤如下： 
        //1、采用回零方式1~5作回原点运动： 
        //  1）使用d2410_set_HOME_pin_logic函数设置原点开关的有效电平；
        //  2）使用d2410_config_home_mode函数设置回原点方式； 
        //  3）设置回原点运动的曲线速度形式； 
        //  4）使用d2410_home_move函数进行回原点运动； 
        //  5）回到原点后，指令脉冲计数器清零。
        
        //2、采用回零方式6作回原点运动： 
        //  1）使用d2410_set_homelatch_mode函数设置原点信号锁存方式； 
        //  2）使用d2410_config_home_mode函数设置回原点方式； 
        //  3）设置回原点运动的曲线速度形式； 
        //  4）使用d2410_home_move函数进行回原点运动； 
        //  5）回到原点后，指令脉冲计数器清零。
        
        //回原点方式
        //DMC2410C运动控制卡提供了6种回原点运动的方式： 
        
        //方式1：一次回零
        //该方式以低速回原点；适合于行程短、安全性要求高的场合。
        //动作过程为：电机从初始位置以恒定低速度向原点方向运动，当到达原点开关位置，原点信号被触发，
        //电机立即停止（过程0）；将停止位置设为原点位置。
        
        //方式2：一次回零加回找 
        //该方式先进行方式1运动，完成后再反向回找原点开关的边缘位置，当原点信号第一次无效的时候，电机立即停止；
        //将停止位置设为原点位置。
        
        //方式3：两次回零 
        //该方式为方式1和方式2的组合。先进行方式2的回零加反找，完成后再进行方式1的一次回零。
        
        //方式4：一次回零后再记1个EZ脉冲进行回零 
        //该方式在回原点运动过程中，当找到原点信号后，还要等待该轴的EZ信号出现，此时电机停止。
        //回零之前需要清除EZ状态，当EZ信号到来时，电机立即停止。
        
        //方式5：EZ单独回零 
        //该方式在回原点运动过程中，当EZ 信号计数到达指定个数，此时电机停止。
        
        //方式6：原点捕获回零
        //该方式在回原点运动过程中，当原点捕获信号有效时，运动减速到停止，然后反向回到锁存位置。
        //当采用原点捕获回零模式的时候，原点信号的初始状态对回零运动没有影响，该回零模式采用的是边沿触发。
        //每次启动该回零运动的时候，原点锁存标志会自动清除。 
        //注意：在回零方式6中，原点锁存位置为指令脉冲位置。
        
        //名称                                功能
        //d2410_set_HOME_pin_logic         设置原点信号的电平和滤波器使能
        //d2410_config_home_mode           选择回原点模式
        //d2410_home_move                  按指定的方向和速度方式开始回原点
        //d2410_set_homelatch_mode         设置原点锁存方式
        //d2410_set_position               指令脉冲计数器清零
        
        //注意：执行完d2410_home_move函数后，指令脉冲计数器不会自动清零；
        //如需清零可以在回零运动完成后，调用d2410_set_position函数软件清零。
        
        //例程7.1：方式1低速回原点
        //d2410_set_HOME_pin_logic 0,0,1           '设置0号轴原点信号低电平有效，使能滤波功能 
        //d2410_config_home_mode 0,0,0             '设置0号轴回零模式为方式1 
        //d2410_set_profile 0,500,1000,0.1,0.1     '设置0号轴梯形曲线速度，加、减速时间 
        //d2410_home_move 0,2,0                    '设置0号轴为负方向回原点，速度方式为低速回原点 
        //While (d2410_check_done(0) = 0)          '检测运动状态，等待回原点动作完成 
        //DoEvents 
        //Wend 
        //D2410_set_position 0,0                   '设置0号轴的指令脉冲计数器绝对位置为0
        
        //例程7.2：方式6低速回原点
        //d2410_set_homelatch_mode 0,2,0           '设置0号轴原点锁存方式为下降沿锁存 
        //d2410_config_home_mode 0,5,0             '设置0号轴回零模式为方式6 
        //d2410_set_profile 0,500,1000,0.1,0.1     '设置0号轴梯形曲线速度，加、减速时间 
        //d2410_home_move 0,2,0                    '设置0号轴为负方向回原点，速度方式为低速回原点 
        //While (d2410_check_done(0) = 0)          '检测运动状态，等待回原点动作完成 
        //DoEvents 
        //Wend 
        //D2410_set_position 0,0                   '设置0号轴的指令脉冲计数器绝对位置为0
        
        //MC2410C运动控制卡在描述运动轨迹时可以用绝对坐标也可以用相对坐标。
        //两种模式各有优点，
        //如：在绝对坐标模式中用一系列坐标点定义一条曲线，如果要修改中间某点坐标时，不会影响后续点的坐标；
        //在相对坐标模式中，用一系列坐标点定义一条曲线，用循环命令可以重复这条曲线轨迹多次。

        //在DMC2410C函数库中距离或位置的单位为脉冲；速度单位为脉冲/秒；时间单位为秒。
        //最基本的位置控制是指从当前位置运动到另一个位置，一般称为点位运动或定长运动。 
        //DMC2410C卡在执行单轴控制时，可使电机按照梯形速度曲线或S形速度曲线进行点位运动或连续运动。

        //梯形速度曲线下的点位运动
        //d2410_set_profile              设定梯形速度曲线的起始速度、最大速度、加速时间、减速时间
        //d2410_set_profile_Extern       设定梯形速度曲线的起始速度、最大速度、停止速度、加速时间、减速时间
        //d2410_t_pmove                  让指定轴以对称梯形速度曲线作点位运动
        //d2410_ex_t_pmove               让指定轴以非对称梯形速度曲线作点位运动

        //注：“对称梯形速度曲线”是指其加速度和减速度相对，即加速过程和减速过程的曲线斜率对称。

        //例程7.3：执行以非对称梯形速度曲线作点位运动 
        //d2410_set_profile 0,500,6000,0.02,0.01     '设置0号轴起始速度为500脉冲/秒 
        //'运行速度为6000脉冲/秒、加速时间为0.02秒、
        //'减速时间为0.01秒。 
        //d2410_ex_t_pmove 0,50000,0                 '设置0号轴、运

        //**************************
        //在单轴运行过程中，最大速度Max_Vel和目标位置Dist均可以实时改变
        //**************************

        //梯形速度下改变速度、终点的相关函数说明
        //d2410_change_speed                  '单轴运行中改变当前最大速度
        //d2410_reset_target_position         '改变目标位置

        //例程7.4：改变速度、改变终点位置
        //d2410_set_profile 0,500,6000,0.01,0.02     '设置梯形曲线速度、加、减速时间*/ 
        //d2410_ex_t_pmove 0,50000,0                 '设置轴号、运动距离50000、相对坐标模式 
        //If(“改变速度条件”)                       '如果改变速度条件满足，则执行改变速度命令 
        //Curr_Vel= 9000                             '设置新的速度 
        //d2410_change_speed 0,Curr_Vel              '执行改变速度指令 
        //End If 
        //If(“改变终点位置条件”)                   '如果改变终点位置条件满足，则执行改变终点位置命令 
        //d2410_reset_target_position 0,55000        '改变终点位置为55000 
        //End If 

        //如果将运动中的最大速度设置得小于起始速度，整个运动过程中将会以最大速度作恒速运动。 
        //如果运动距离很短，当距离小于或等于(Max_Vel+Min_Vel)×Tacc时，理论上速度曲线将变为三角形；
        //但DMC2410C运动控制卡有自动调整功能，将三角形的尖峰削去，以避免速度变化太大发生冲击现象

        //S形速度曲线运动模式
        //d2410_set_st_profile                '设置S段时间，起始速度和停止速度相同
        //d2410_set_st_profile_Extern         '设置S段时间，有停止速度
        //d2410_s_pmove                       '让指定轴以对称S形速度曲线作点位运动
        //d2410_ex_s_pmove                    '让指定轴以非对称S形速度曲线作点位运动

        //在S形速度曲线下的点位运动过程中，也可以调用d2410_change_speed和d2410_reset_target_position函数
        //实时改变运行速度和目标位置。但多轴插补运行情况下不能实时改变运行速度和目标位置。

        //连续运动模式
        //连续运动模式中，DMC2410C控制卡可以控制电机以梯形或S形速度曲线在指定的加速时间内从起始速度加速至最大速度，
        //然后以该速度连续运行，直至调用停止指令或者该轴遇到限位信号才会按启动时的速度曲线减速停止。

        //连续运动相关函数说明
        //d2410_t_vmove      '让指定轴以梯形速度曲线加速到最大速度后，连续运行
        //d2410_s_vmove      '让指定轴以S形速度曲线加速到最大速度后，连续运行
        //d2410_decel_stop   '指定轴减速停止。调用此函数后立即减速，到达起始速度后停止

        //在单轴执行连续运动过程中，可以调用d2410_change_speed 实时改变速度。
        //注意：在以S形速度曲线连续运动时，改变最大速度最好在加速过程已经完成的恒速段进行。

        //例程7.5：以S形速度曲线加速的连续运动及变速、停止控制
        //d2410_set_st_profile 0,100, 1000,0.1,0.1,0.01,0      '设置S形曲线S段时间 
        //d2410_s_vmove 0,1                                    '0号轴连续运动，方向为正 
        //if(“改变速度条件”)                                 '如果改变速度条件满足，则执行改变速度命令 
        //Curr_Vel= 1200                                       '设置新的速度 
        //d2410_change_speed 0,Curr_Vel                        '执行改变速度指令 
        //End If 
        //if(“停止条件”)                                     '如果运动停止条件满足，则执行减速停止命令 
        //d2410_decel_stop 0,0.1                               '减速停止，减速时间为0.1秒
        //End If

        //多轴运动的实现

        //*************************
        //多轴联动
        //几个轴同时运动，称为多轴联动。
        //DMC2410C运动控制卡单张卡可以控制4个轴以多种方式运动，常用的有：多轴联动、直线插补、圆弧插补。
        //DMC2410C控制卡可以控制多个电机同时执行d2410_t_move、d2410_s_move这类单轴运动函数。
        //所谓同时执行，是在程序中顺序调用d2410_t_move、d2410_s_move等函数，因为程序执行速度很快，
        //在瞬间几个电机都开始运动，给人的感觉就是同时开始运动。 
        //多轴联动在各轴速度设置不当时，各轴停止时间不同、在起点与终点之间运动的轨迹也不是直线
        //*************************

        //直线插补运动
        //DMC2410C卡可以进行任意2轴、3轴和4轴直线插补，插补工作由控制卡的硬件执行，
        //用户只需将插补运动的速度、加速度、终点位置等参数写入相关函数，无需介入插补过程中的计算工作。

        //直线插补运动相关函数说明
        //d2410_t_line2                  '让指定的两轴作对称的梯形加减速插补运动
        //d2410_t_line3                  '让指定的三轴作对称的梯形加减速插补运动
        //d2410_t_line4                  '指定四轴以对称的梯形速度曲线做插补运动
        //d2410_set_vector_profile       '设定插补矢量运动曲线的最大速度、加速时间、减速时间

        //例程7.6：XY轴直线插补 
        //Dim AxisArray(2) as Integer AxisArray(0)=0     '定义插补0轴为X轴 
        //AxisArray(1)=1                                 '定义插补1轴为Y轴 
        //d2410_set_vector_profile 0,5000,0.1,0.2 
        //d2410_t_line2 AxisArray(0),30000,AxisArray(1),40000,0
        //该例程使X，Y轴进行相对模式直线插补运动，
        //其相关参数为： 
        //ΔX = 30000 pulse 
        //ΔY = 40000 pulse 
        //最大矢量速度 = 5000 p/s （0轴,1轴分速度为3000，4000 p/s） 
        //梯形加速时间 = 0.1 s 
        //梯形减速时间 = 0.2 s

        //圆弧插补运动
        //DMC2410C卡的任意两轴之间可以进行圆弧插补，圆弧插补分为相对位置圆弧插补和绝对位置圆弧插补，
        //运动的方向分为顺时针（CW）和逆时针（CCW）。

        //圆弧插补相关函数说明
        //d2410_arc_move             '让指定的二轴作绝对位置圆弧插补运动。
        //d2410_rel_arc_move         '让指定的二轴作相对位置圆弧插补运动。

        //例程7.7：XY轴圆弧插补 
        //Dim AxisArray(2) As Integer
        //Dim Pos(2), Cen(2) As Long
        //AxisArray(0)=0                                  '定义0轴为插补X轴
        //AxisArray(1)=1                                  '定义1轴为插补Y轴 
        //Pos(0) = 5000 Pos(1) = -5000                    '设置终点坐标 
        //Cen(0) = 5000 Cen(1) = 0                        '设置圆心坐标 
        //'d2410_set_vector_profile 1000,3000,0.1,0.2      '设置矢量速度曲线 
        //d2410_arc_move AxisArray(0), Pos(0), Cen(0), 0  'XY轴进行顺时针方向绝对圆弧插补运动， 
        //'终点（5000, -5000），圆心（5000, 0）


        //通用IO相关函数说明
        //d2410_read_inbit           '读取指定控制卡的某一位输入口的电平状态
        //d2410_write_outbit         '对指定控制卡的某一位输出口置位
        //d2410_read_outbit          '取指定控制卡的某一位输出口的电平状态
        //d2410_read_inport          '读取指定控制卡的全部通用输入口的电平状态
        //d2410_read_outport         '读取指定控制卡的全部通用输出口的电平状态
        //d2410_write_outport        '指定控制卡的全部通用输出口的电平状态

        //注意：在使用d2410_write_outport对运动控制卡的全部输出口进行置位，
        //使用d2410_read_inport、d2410_read_outport进行IO电平读取显示时，
        //应该使用十六进制数进行赋值（尽量避免使用十进制数，特别是在不支持无符号变量的开发环境下）。
        //在对IO电平进行控制与读取时，使用十六进制数赋值远比使用十进制数赋值更加直观、方便。

        //例程7.8：读取第0号卡的通用输入口1的电平值，并对通用输出口3置高电平
        //Dim MyCardNo, MyInbitno, MyInValue, MyOutbitno, MyOutValue As Integer
        //MyCardNo = 0                                         '卡号 
        //MyInbitno = 1                                        '定义通用输入口1 
        //MyInValue = d2410_read_inbit (MyCardNo, MyInbitno)   '读取通用输入口1的电平值，并赋值给变量MyInValue 
        //MyOutbitno = 3                                       '定义通用输出口3 
        //MyOutValue = 1                                       '定义输出电平为高 
        //d2410_write_outbit MyCardNo, MyOutbitno, MyOutValue  '对通用输出口3置高电平

        //例7.9：读取全部输入IO口的电平值并进行显示，对全部输出IO口的电平进行初始化
        //Dim MyCardNo As Integer
        //Dim MyInportValue, MyOutportValue As Long
        //Dim MyInportValueTemp As String
        //MyCardNo = 0                                        '卡号 
        //MyInportValue = d2410_read_inport (MyCardNo)        '读取所有输入IO口电平值，并赋值给变量MyInportValue 
        //MyInportValueTemp = Hex(MyInportValue)              '转换成十六进制 
        //MyInTextShow = MyInportValueTemp                    '显示在文本框MyInTextShow中 
        //MyOutportValue = &HFFFFFBFA                         '&H表示十六进制（VB），定义输出口电平值，输出口1、3、11为低电平，其余端口为高电平 
        //d2410_write_outport MyCardNo, MyOutportValue        '对全部输出口进行电平赋值

        //编码器检测的实现
        //DMC2410C的反馈位置计数器是一个32位正负计数器，对通过控制卡编码器接口EA，
        //EB输入的脉冲（如编码器、光栅尺反馈脉冲等）进行计数。

        //编码器检测相关函数说明
        //d2410_counter_config           '设置编码器输入口的计数方式
        //d2410_get_encoder              '读取编码器反馈的脉冲计数值
        //d2410_set_encoder              '设置编码器的脉冲计数值

        //例程7.10：编码器反馈计数的操作
        //d2410_counter_config 0,3            '设置轴0为4倍计数，默认的EA、EB计数方向 
        //d2410_set_encoder 0,0               '设置轴0的计数初始值为0 
        //X_Position = d2410_get_encoder(0)   '读轴0的计数器的数值至变量X_Position

        //位置比较功能的实现
        //DMC2410C运动控制卡提供了位置比较功能，位置比较的一般步骤是： 
        //   1、配置比较器； 
        //   2、清除位置比较数据；
        //   3、添加比较位置点；
        //   4、开始运动并查看比较状态。

        //低速位置比较功能
        //DMC2410C卡提供了两组低速位置比较，每组最多都可以添加8个比较点。低速位置比较的触发延时时间小于1ms。

        //低速位置比较相关函数说明
        //d2410_compare_config_Extern               '设置比较器配置
        //d2410_compare_clear_points_Extern         '清除所有比较点
        //d2410_compare_add_point_Extern            '添加比较点
        //d2410_compare_get_current_point_Extern    '读取当前比较点位置
        //d2410_compare_get_points_runned_Extern    '查询已经比较过的点个数
        //d2410_compare_get_points_remained_Extern  '查询可以加入的比较点数量

        //注意：1）低速位置比较共有两组比较队列，每组队列的位置比较都是独立进行的； 
        //      2）执行位置比较时，每个比较点的触发是按照添加的比较点顺序执行的，
        //         即如果有一个比较点没有被触发比较动作，那么后面的比较点是不会起作用的。

        //例7.11：低速位置比较 
        //Dim MyCardNo, Myqueue, Myenable, Myaxis, Mycmp_source As Integer
        //Dim Mydir, Myaction As Integer
        //Dim Mypos, Myactpara As Long
        //MyCardNo = 0             '卡号 
        //Myqueue = 0              '设置比较序列号为0 
        //Myaxis = 0               '轴号为0 
        //Myenable = 1             '设置比较器使能 
        //Mycmp_source = 0         '设置比较源为指令位置 
        //d2410_compare_config_Extern MyCardNo,Myqueue, Myenable, Myaxis,Mycmp_source '设置比较器 
        //d2410_compare_clear_points_Extern MyCardNo,Myqueue                          '清除比较点 
        //Mypos = 8000                 '设置比较位置为8000pulse 
        //Mydir = 1                    '设置比较方向为大于等于 
        //Myaction = 3                 '触发功能为IO电平取反 
        //Myactpara = 1                '设置输出IO端口1触发功能 
        //d2410_set_position MyAxis, 0 '当前位置设为零点 
        //d2410_compare_add_point_Extern MyCardNo,Myqueue,Mypos,Mydir,Myaction,Myactpara '添加比较点 
        //d2410_ex_t_pmove Myaxis,10000,0     '执行定长运动，位移为10000pulse，相对坐标模式

        //高速位置比较功能
        //DMC2410C控制卡为每个轴提供了一个高速位置比较。高速位置比较基本无触发延时时间
        //（至多延时两个指令脉冲的时间）。

        //高速位置比较相关函数说明
        //d2410_config_CMP_PIN           '配置高速位置比较器
        //d2410_read_CMP_PIN             '读取高速位置比较输出口状态

        //注意：每轴的位置比较都是独立进行的，高速位置比较暂时只支持反馈位置比较。

        //例7.12：高速位置比较 
        //Dim Myaxis, Mycmp_enable, MyCMP_logic As Integer
        //Dim Mycmp_pos As Long
        //Myaxis = 0                         '轴号 
        //Mycmp_enable = 1                   'CMP使能 
        //Mycmp_pos = 8000                   'CMP比较位置为8000pulse 
        //MyCMP_logic = 0                    'CMP输出低电平，脉冲信号 
        //d2410_config_CMP_PIN Myaxis, Mycmp_enable, Mycmp_pos, MyCMP_logic      '设置比较器，0号轴，比较位置为8000pulse，
        //'触发时动作为CMP输出低电平，脉冲信号 
        //d2410_ex_t_pmove Myaxis,10000,1       '执行定长运动，位移为10000pulse，绝对坐标模式

        //位置锁存功能的实现
        //DMC2410C卡提供了高速位置锁存功能，位置锁存无触发延时时间，当捕获到位置锁存信号后立即锁存当前位置。

        //高速锁存相关函数说明
        //d2410_config_latch_mode    '设置锁存方式为单轴锁存或是四轴同时锁存
        //d2410_get_latch_value      '读取编码器锁存器的值
        //d2410_get_latch_flag       '读取指定控制卡的锁存器的标志位
        //d2410_reset_latch_flag     '复位指定控制卡的锁存器的标志位
        //d2410_triger_chunnel       '选择全部锁存时的外触发信号通道

        //注意：1）在位置锁存中，多次触发高速锁存口只锁存第一次触发位置，只有调用函数清除锁存状态方可再次锁存； 
        //      2）位置锁存暂时只支持反馈位置锁存。

        //例7.13：位置锁存 
        //Dim MyCardNo, Myaxis, Myall_enable As Integer
        //Dim Mylatch_value As Long
        //MyCardNo = 0                                        '卡号 
        //Myaxis = 0                                          '轴号 
        //Myall_enable = 0                                    '设置锁存方式为单独锁存 
        //d2410_config_latch_mode MyCardNo,Myall_enable       '设置锁存方式 
        //d2410_reset_latch_flag MyCardNo                     '复位锁存器标志位 
        //d2410_ex_t_pmove Myaxis,10000,0                     '执行定长运动，位移为10000pulse，相对坐标模式 
        //While ((d2410_get_latch_flag(MyCardNo) And (2 ^ (Myaxis + 8))) = 0) '判断设定轴的LTC锁存状态 
        //DoEvents 
        //Wend 
        //Mylatch_value= d2410_get_latch_value(Myaxis)         '读取锁存器的值，并赋值给变量My_latch_Value

        //手轮运动功能的实现
        //DMC2410C运动控制卡支持单轴手轮运动功能。该功能允许用户设置一个手轮通道对应一个运动轴进行运动。

        //手轮运动功能相关函数说明
        //d2410_set_handwheel_inmode       '设置输入手轮脉冲信号的计数方式
        //d2410_handwheel_move             '启动指定轴的手轮脉冲运动

        //注 意：当启动手轮运动后，只有发送d2410_decel_stop或d2410_imd_stop命令后才会退出手轮模式
        //例7.14：手轮运动 
        //Dim Myaxis, Myinmode As Integer
        //Dim Mymulti As Double
        //Myaxis = 0                                             '设置运动轴为0号轴 
        //Myinmode = 0                                           '设置手轮输入方式为AB相 
        //Mymulti = 10                                           '设置手轮输入倍率为10 
        //d2410_set_handwheel_inmode Myaxis,Myinmode, Mymulti    '设置手轮运动 
        //d2410_handwheel_move Myaxis                            '启动手轮运动 
        //d2410_imd_stop Myaxis                                  '立即停止手轮运动

        #endregion

        #region "变量定义"

        Microsoft.VisualBasic.Devices.Computer PC = new Microsoft.VisualBasic.Devices.Computer();

        /// <summary>
        /// 可用卡数量
        /// </summary>
        Int32 AvailableCardQty = 0;

        /// <summary>
        /// 目标轴号
        /// </summary>
        ushort TargetAxis = 0;

        /// <summary>
        /// 目标卡号
        /// </summary>
        ushort TargetCard = 0;

        bool SuccessBuiltNew = false, PasswordIsCorrect = false;

        /// <summary>
        /// 函数执行的返回值【布尔】
        /// </summary>
        bool TempStatus = false;

        Int32 TempErrorCode=0;

        string ErrorMessage = "";

        /// <summary>
        /// 当前错误代码
        /// </summary>
        public Int32 ErrorCode 
            {
            get { return TempErrorCode; }
            }

        /// <summary>
        /// 雷赛运动卡错误信息枚举
        /// </summary>
        public enum ErrorMsg 
            {

            /// <summary>
            /// 成功
            /// </summary>
            Err_NoErr,

            /// <summary>
            /// 未知错误
            /// </summary>
            Err_Unknown,

            /// <summary>
            /// 参数错误
            /// </summary>
            Err_ParaErr,

            /// <summary>
            /// 超时
            /// </summary>
            Err_Timeout,

            /// <summary>
            /// 正在运动中/控制器忙
            /// </summary>
            Err_ControllerBusy,

            /// <summary>
            /// 运动/处理错误
            /// </summary>
            Motion_Err_HandleErr,

            /// <summary>
            /// 发送错误
            /// </summary>
            Err_SendErr,

            /// <summary>
            /// 固件无效参数
            /// </summary>
            Err_FirewareInvalidPara,

            /// <summary>
            /// 运动卡不支持固件
            /// </summary>
            Err_FirewareCardNotSupport

            //以上是在说明书上找到的
            //Motion_ERR_CONNECT_TOOMANY = 5,                    //板卡数过多

            //Motion_ERR_CONTILINE = 6,                        //连续插补错误
            //Motion_ERR_NoThisFunction = 7,                    //暂不支持该功能
            //Motion_ERR_CANNOT_CONNECTETH = 8,                //不能连接
            //Motion_ERR_HANDLEERR = 9,                        //卡资源未能找到
            //Motion_ERR_SENDERR = 10,                        //pci通信错误
            //Motion_ERR_FIRMWAREERR = 12,                    //固件文件错误
            //Motion_ERR_FIRMWAR_MISMATCH = 14,                //固件不匹配

            //Motion_ERR_FIRMWARE_INVALID_PARA    = 20,        //固件参数错误
            //Motion_ERR_FIRMWARE_PARA_ERR    = 20,            //固件参数错误2
            //Motion_ERR_FIRMWARE_STATE_ERR    = 22,            //固件当前状态不允许操作
            //Motion_ERR_FIRMWARE_LIB_STATE_ERR    = 22,        //固件当前状态不允许操作2
            //Motion_ERR_FIRMWARE_CARD_NOT_SUPPORT    = 24,   //固件不支持的功能 控制器不支持的功能
            //Motion_ERR_FIRMWARE_LIB_NOTSUPPORT    = 24,     //固件不支持的功能2
            
            }

        /// <summary>
        /// 执行回原点运动原点锁存使能状态：0：禁止，2：允许
        /// </summary>
        ushort SearchingHomeEnableLatch = 2;

        /// <summary>
        /// 执行回原点运动原点锁存方式：0：下降沿锁存，1：上升沿锁存
        /// </summary>
        ushort SearchingHomeLatchLogic = 0;

        /// <summary>
        /// 回原点运动中，原点信号的有效电平：0：低电平有效，1：高电平有效
        /// </summary>
        ushort SearchingHomeOrgLogic = 1;

        /// <summary>
        /// 回原点运动中，允许/禁止滤波功能：0：禁止，1：允许
        /// </summary>
        ushort SearchingHomeEnableFilter = 0;

        /// <summary>
        /// EZ信号出现EZCount指定的次数后，轴运动停止。
        /// 仅当Mode=5时该参数设置有效，取值范围：1－16
        /// </summary>
        ushort SearchingHomeEZCount = 0;

        /// <summary>
        /// 回原点运动起始速度，单位：pulse/s
        /// </summary>
        double SearchingHomeMinVelocity = 0;

        /// <summary>
        /// 回原点运动最大速度，单位：pulse/s
        /// </summary>
        double SearchingHomeMaxVelocity = 0;

        /// <summary>
        /// 回原点运动总加速时间，单位：s
        /// </summary>
        double SearchingHomeAccTime = 0.0;
        
        /// <summary>
        /// 回原点运动总减速时间，单位：s
        /// </summary>
        double SearchingHomeDecTime = 0.0;

        /// <summary>
        /// 回原点方式：1：正方向回原点，2：负方向回原点
        /// </summary>
        ushort SearchingHomeDirection = 0;

        /// <summary>
        /// 回原点速度：0：低速回原点，1：高速回原点
        /// </summary>
        ushort SearchingHomeVelocityMode = 0;

        /// <summary>
        /// 执行回原点中标志
        /// </summary>
        bool DoingSearchHome = false;

        /// <summary>
        /// 回原点模式【1~6】
        /// </summary>
        ushort SearchingHomeMode = 0;

        /// <summary>
        /// 执行回原点的线程
        /// </summary>
        private Thread SearchHomeThread;

        /// <summary>
        /// 软件作者信息
        /// </summary>
        public string Author 
            {
            get { return "软件作者：彭东南，联系方式：southeastofstar@163.com"; }            
            }

        /// <summary>
        /// 轴是否已经在原点
        /// </summary>
        public bool AtHome 
            {
            get 
                {
                AxisSignal AxisStatus =GetAxisSignalStatus();
                if (AxisStatus.HomeSignal == true)
                    {
                    return true;
                    }
                else 
                    {
                    return false;
                    }
                }
            }

        /// <summary>
        /// 轴伺服报警、正限位、负限位、原点信号数据结构
        /// </summary>
        public struct AxisSignal 
            {
            /// <summary>
            /// 伺服报警信号状态
            /// </summary>
            public bool ServoAlarm;

            /// <summary>
            /// 正限位信号状态
            /// </summary>
            public bool PositiveLimit;

            /// <summary>
            /// 负限位信号状态
            /// </summary>
            public bool NegotiveLimit;

            /// <summary>
            /// 原点信号状态
            /// </summary>
            public bool HomeSignal;            
            }

        /// <summary>
        /// 轴紧急停止信号（EMG）、索引信号(EZ)、+DR(PA)信号、-DR(PB)信号数据结构
        /// </summary>
        public struct AxisOutsideSignal 
            {
            /// <summary>
            /// 紧急停止信号(EMG)
            /// </summary>
            public bool EStop;

            /// <summary>
            /// 索引信号(EZ)
            /// </summary>
            public bool EZIndexSignal;

            /// <summary>
            /// +DR(PA)信号
            /// </summary>
            public bool PositiveDR_PA;

            /// <summary>
            /// -DR(PB)信号
            /// </summary>
            public bool NegativeDR_PB;
            }

        /// <summary>
        /// 控制卡锁存器的标志位数据结构
        /// </summary>
        public struct LatchFlag 
            {
            /// <summary>
            /// 指定卡的0轴有触发信号
            /// </summary>
            public bool Bit0;

            /// <summary>
            /// 指定卡的1轴有触发信号
            /// </summary>
            public bool Bit1;

            /// <summary>
            /// 指定卡的2轴有触发信号
            /// </summary>
            public bool Bit2;

            /// <summary>
            /// 3轴有触发信号
            /// </summary>
            public bool Bit3;

            /// <summary>
            /// 指定卡的0轴有清零信号
            /// </summary>
            public bool Bit4;

            /// <summary>
            /// 指定卡的1轴有清零信号
            /// </summary>
            public bool Bit5;

            /// <summary>
            /// 指定卡的2轴有清零信号
            /// </summary>
            public bool Bit6;

            /// <summary>
            /// 指定卡的3轴有清零信号
            /// </summary>
            public bool Bit7;

            /// <summary>
            /// 指定卡的0轴位置已锁存
            /// </summary>
            public bool Bit8;

            /// <summary>
            /// 指定卡的1轴位置已锁存
            /// </summary>
            public bool Bit9;

            /// <summary>
            /// 指定卡的2轴位置已锁存
            /// </summary>
            public bool Bit10;

            /// <summary>
            /// 指定卡的3轴位置已锁存
            /// </summary>
            public bool Bit11;

            /// <summary>
            /// 指定卡的0轴位置已清零
            /// </summary>
            public bool Bit12;

            /// <summary>
            /// 指定卡的1轴位置已清零
            /// </summary>
            public bool Bit13;

            /// <summary>
            /// 指定卡的2轴位置已清零
            /// </summary>
            public bool Bit14;

            /// <summary>
            /// 指定卡的3轴位置已清零
            /// </summary>
            public bool Bit15;
            }

        /// <summary>
        /// 控制卡的计数器的标识位数据结构
        /// </summary>
        public struct CounterFlag 
            {

            /// <summary>
            /// 控制卡的计数器的标识位0
            /// </summary>
            public bool Bit0;

            /// <summary>
            /// 控制卡的计数器的标识位1
            /// </summary>
            public bool Bit1;

            /// <summary>
            /// 控制卡的计数器的标识位2
            /// </summary>
            public bool Bit2;

            /// <summary>
            /// 控制卡的计数器的标识位3
            /// </summary>
            public bool Bit3;

            /// <summary>
            /// 控制卡的计数器的标识位8
            /// </summary>
            public bool Bit8;

            /// <summary>
            /// 控制卡的计数器的标识位9
            /// </summary>
            public bool Bit9;

            /// <summary>
            /// 控制卡的计数器的标识位10
            /// </summary>
            public bool Bit10;

            /// <summary>
            /// 控制卡的计数器的标识位11
            /// </summary>
            public bool Bit11;

            /// <summary>
            /// 控制卡的计数器的标识位16
            /// </summary>
            public bool Bit16;

            /// <summary>
            /// 控制卡的计数器的标识位17
            /// </summary>
            public bool Bit17;

            /// <summary>
            /// 控制卡的计数器的标识位18
            /// </summary>
            public bool Bit18;

            /// <summary>
            /// 控制卡的计数器的标识位19
            /// </summary>
            public bool Bit19;

            /// <summary>
            /// 控制卡的计数器的标识位24
            /// </summary>
            public bool Bit24;

            /// <summary>
            /// 控制卡的计数器的标识位25
            /// </summary>
            public bool Bit25;

            /// <summary>
            /// 控制卡的计数器的标识位26
            /// </summary>
            public bool Bit26;

            /// <summary>
            /// 控制卡的计数器的标识位27
            /// </summary>
            public bool Bit27;
            
            }

        /// <summary>
        /// 控制卡全部通用输出信号的数据结构
        /// 【Bit0~Bit19、Bit24~Bit31位值分别代表第1~20、25~32号输出端口值】
        /// </summary>
        public struct OutputSignal 
            {

            /// <summary>
            /// 通用输出位0
            /// </summary>
            public bool Bit0;

            /// <summary>
            /// 通用输出位1
            /// </summary>
            public bool Bit1;

            /// <summary>
            /// 通用输出位2
            /// </summary>
            public bool Bit2;

            /// <summary>
            /// 通用输出位3
            /// </summary>
            public bool Bit3;

            /// <summary>
            /// 通用输出位4
            /// </summary>
            public bool Bit4;

            /// <summary>
            /// 通用输出位5
            /// </summary>
            public bool Bit5;

            /// <summary>
            /// 通用输出位6
            /// </summary>
            public bool Bit6;

            /// <summary>
            /// 通用输出位7
            /// </summary>
            public bool Bit7;

            /// <summary>
            /// 通用输出位8
            /// </summary>
            public bool Bit8;

            /// <summary>
            /// 通用输出位9
            /// </summary>
            public bool Bit9;

            /// <summary>
            /// 通用输出位10
            /// </summary>
            public bool Bit10;

            /// <summary>
            /// 通用输出位11
            /// </summary>
            public bool Bit11;

            /// <summary>
            /// 通用输出位12
            /// </summary>
            public bool Bit12;

            /// <summary>
            /// 通用输出位13
            /// </summary>
            public bool Bit13;

            /// <summary>
            /// 通用输出位14
            /// </summary>
            public bool Bit14;

            /// <summary>
            /// 通用输出位15
            /// </summary>
            public bool Bit15;

            /// <summary>
            /// 通用输出位16
            /// </summary>
            public bool Bit16;

            /// <summary>
            /// 通用输出位17
            /// </summary>
            public bool Bit17;

            /// <summary>
            /// 通用输出位18
            /// </summary>
            public bool Bit18;

            /// <summary>
            /// 通用输出位19
            /// </summary>
            public bool Bit19;

            /// <summary>
            /// 通用输出位24
            /// </summary>
            public bool Bit24;
            
            /// <summary>
            /// 通用输出位25
            /// </summary>
            public bool Bit25;

            /// <summary>
            /// 通用输出位26
            /// </summary>
            public bool Bit26;

            /// <summary>
            /// 通用输出位27
            /// </summary>
            public bool Bit27;

            /// <summary>
            /// 通用输出位28
            /// </summary>
            public bool Bit28;

            /// <summary>
            /// 通用输出位29
            /// </summary>
            public bool Bit29;

            /// <summary>
            /// 通用输出位30
            /// </summary>
            public bool Bit30;

            /// <summary>
            /// 通用输出位31
            /// </summary>
            public bool Bit31;
            
            }

        /// <summary>
        /// 控制卡全部输入信号的数据结构【0~31位】
        /// </summary>
        public struct InputSignal 
            {

            /// <summary>
            /// 通用输入位0
            /// </summary>
            public bool Bit0;

            /// <summary>
            /// 通用输入位1
            /// </summary>
            public bool Bit1;

            /// <summary>
            /// 通用输入位2
            /// </summary>
            public bool Bit2;

            /// <summary>
            /// 通用输入位3
            /// </summary>
            public bool Bit3;

            /// <summary>
            /// 通用输入位4
            /// </summary>
            public bool Bit4;

            /// <summary>
            /// 通用输入位5
            /// </summary>
            public bool Bit5;

            /// <summary>
            /// 通用输入位6
            /// </summary>
            public bool Bit6;

            /// <summary>
            /// 通用输入位7
            /// </summary>
            public bool Bit7;

            /// <summary>
            /// 通用输入位8
            /// </summary>
            public bool Bit8;

            /// <summary>
            /// 通用输入位9
            /// </summary>
            public bool Bit9;

            /// <summary>
            /// 通用输入位10
            /// </summary>
            public bool Bit10;

            /// <summary>
            /// 通用输入位11
            /// </summary>
            public bool Bit11;

            /// <summary>
            /// 通用输入位12
            /// </summary>
            public bool Bit12;

            /// <summary>
            /// 通用输入位13
            /// </summary>
            public bool Bit13;

            /// <summary>
            /// 通用输入位14
            /// </summary>
            public bool Bit14;

            /// <summary>
            /// 通用输入位15
            /// </summary>
            public bool Bit15;

            /// <summary>
            /// 通用输入位16
            /// </summary>
            public bool Bit16;

            /// <summary>
            /// 通用输入位17
            /// </summary>
            public bool Bit17;

            /// <summary>
            /// 通用输入位18
            /// </summary>
            public bool Bit18;

            /// <summary>
            /// 通用输入位19
            /// </summary>
            public bool Bit19;
            
            /// <summary>
            /// 通用输入位20
            /// </summary>
            public bool Bit20;

            /// <summary>
            /// 通用输入位21
            /// </summary>
            public bool Bit21;

            /// <summary>
            /// 通用输入位22
            /// </summary>
            public bool Bit22;

            /// <summary>
            /// 通用输入位23
            /// </summary>
            public bool Bit23;

            /// <summary>
            /// 通用输入位24
            /// </summary>
            public bool Bit24;

            /// <summary>
            /// 通用输入位25
            /// </summary>
            public bool Bit25;

            /// <summary>
            /// 通用输入位26
            /// </summary>
            public bool Bit26;

            /// <summary>
            /// 通用输入位27
            /// </summary>
            public bool Bit27;

            /// <summary>
            /// 通用输入位28
            /// </summary>
            public bool Bit28;

            /// <summary>
            /// 通用输入位29
            /// </summary>
            public bool Bit29;

            /// <summary>
            /// 通用输入位30
            /// </summary>
            public bool Bit30;

            /// <summary>
            /// 通用输入位31
            /// </summary>
            public bool Bit31;
            
            }
        
        #endregion
                
        #region "初始化函数"

        //建立单个轴运动控制的新实例
        /// <summary>
        /// 建立单个轴运动控制的新实例
        /// </summary>
        /// <param name="DLLPassword">使用此DLL文件的密码</param>
        /// <param name="NumberOfTargetAxis">目标轴编号【范围：1~卡数量*4】</param>
        /// <param name="NumberOfTargetCard">目标卡[轴所在卡]编号【范围：1~8】</param>
        public AxisControlOfLeadShineDMC2410C(string DLLPassword,
            Int32 NumberOfTargetAxis, Int32 NumberOfTargetCard) 
            {

            try
                {

                SuccessBuiltNew = false;
                PasswordIsCorrect = false;

                if (DLLPassword == "ThomasPeng" 
                    | DLLPassword == "pengdongnan" 
                    | DLLPassword == "彭东南")
                    {
                    PasswordIsCorrect = true;

                    if(PC.FileSystem.FileExists("DMC2410.dll")==false)
                        {
                        MessageBox.Show("Failed to load the 'DMC2410.dll', please copy it to the same directory of this software."
                            + "Otherwise, this software can't run.\r\n" 
                            + "请将 'DMC2410.dll'文件拷贝至软件运行的当前目录中，否则软件无法运行！",
                            "警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;
                        }

                    //返回值：0： 没有找到控制卡，或者控制卡异常 1到8： 控制卡数 
                    //负值： 表述有2张或2张以上控制卡的硬件设置卡号相同；返回值取绝对值后减1即为该卡号
                    AvailableCardQty = d2410_board_init();

                    if (AvailableCardQty == 0) 
                        {
                        MessageBox.Show("There is no any Leadshine Motion Card in the PC system," 
                            + " please double check the possible reasons and retry.\r\n"
                            + "当前PC系统中没有发现可用的雷赛运动卡，请检查可能的原因后再尝试。",
                            "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                        }
                    else if (AvailableCardQty < 0) 
                        {
                        MessageBox.Show("There are two or more motion cards which have the same hardware ID," 
                            + " please carefully check the system and set the unique number for each motion card.\r\n"
                            + "当前PC系统中有两个或两个以上运动卡的硬件设置卡号相同，请检查系统并设置正确硬件卡号再尝试！",
                            "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                        }

                    //【多卡运行】
                    //DMC2410C运动控制卡的驱动程序支持最多8块DMC2410C卡同时工作。因此，一台PC机可以同时控制多达32个电机同时运动。 
                    //DMC2410C卡支持即插即用模式，用户可不必去关心如何设置卡的基地址和IRQ中断值。在使用多块运动控制卡时，首先要
                    //用运动控制卡上的拨码开关设置卡号；系统启动后，系统BIOS为相应的卡自动分配物理空间。 

                    if (NumberOfTargetCard > AvailableCardQty) 
                        {
                        MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                            + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                            "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                        }

                    //【卡号和轴号的对应关系为】： 
                    // 0号卡对应0~3号轴；1号卡对应4~7号轴；n号卡对应4n~ 4*（n+1）-1号轴。
                    //【使用习惯从1开始计算轴数量，在调用函数时需要减去1才是正确的轴号。】
                    if (NumberOfTargetAxis > AvailableCardQty * 4
                        || NumberOfTargetAxis < (AvailableCardQty * 4 - 3))
                        {
                        MessageBox.Show("The value for target axis 'NumberOfTargetAxis' should be : "
                            + (AvailableCardQty * 4 - 3) + "~" + (AvailableCardQty * 4)
                            + " ,please revise the parameter and retry.\r\n"
                            + "目标轴号参数 'NumberOfTargetAxis' 取值范围："
                            + (AvailableCardQty * 4 - 3) + "~" + (AvailableCardQty * 4)
                            + "请修改参数。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                        }
                    else 
                        {
                        TargetAxis = (ushort)(NumberOfTargetAxis - 1);
                        }
                    TargetCard = (ushort)NumberOfTargetCard;
                    SuccessBuiltNew = true;

                    }
                else
                    {
                    SuccessBuiltNew = false;
                    PasswordIsCorrect = false;
                    MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                    }

                }
            catch (Exception ex)
                {
                SuccessBuiltNew = false;
                MessageBox.Show("创建类的实例时出现错误\r\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }
            
            
            }

        //建立单个轴运动控制的新实例
        /// <summary>
        /// 建立单个轴运动控制的新实例
        /// </summary>
        /// <param name="DLLPassword">使用此DLL文件的密码</param>
        /// <param name="NumberOfTargetAxis">目标轴编号【范围：1~卡数量*4】</param>
        public AxisControlOfLeadShineDMC2410C(string DLLPassword,
            Int32 NumberOfTargetAxis)
            {

            try
                {

                SuccessBuiltNew = false;
                PasswordIsCorrect = false;

                if (DLLPassword == "ThomasPeng"
                    | DLLPassword == "pengdongnan"
                    | DLLPassword == "彭东南")
                    {
                    PasswordIsCorrect = true;

                    if (PC.FileSystem.FileExists("DMC2410.dll") == false)
                        {
                        MessageBox.Show("Failed to load the 'DMC2410.dll', please copy it to the same directory of this software."
                            + "Otherwise, this software can't run.\r\n"
                            + "请将 'DMC2410.dll'文件拷贝至软件运行的当前目录中，否则软件无法运行！",
                            "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                        }

                    //返回值：0： 没有找到控制卡，或者控制卡异常 1到8： 控制卡数 
                    //负值： 表述有2张或2张以上控制卡的硬件设置卡号相同；返回值取绝对值后减1即为该卡号
                    AvailableCardQty = d2410_board_init();

                    if (AvailableCardQty == 0)
                        {
                        MessageBox.Show("There is no any Leadshine Motion Card in the PC system,"
                            + " please double check the possible reasons and retry.\r\n"
                            + "当前PC系统中没有发现可用的雷赛运动卡，请检查可能的原因后再尝试。",
                            "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                        }
                    else if (AvailableCardQty < 0)
                        {
                        MessageBox.Show("There are two or more motion cards which have the same hardware ID,"
                            + " please carefully check the system and set the unique number for each motion card.\r\n"
                            + "当前PC系统中有两个或两个以上运动卡的硬件设置卡号相同，请检查系统并设置正确硬件卡号再尝试！",
                            "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                        }

                    //【多卡运行】
                    //DMC2410C运动控制卡的驱动程序支持最多8块DMC2410C卡同时工作。因此，一台PC机可以同时控制多达32个电机同时运动。 
                    //DMC2410C卡支持即插即用模式，用户可不必去关心如何设置卡的基地址和IRQ中断值。在使用多块运动控制卡时，首先要
                    //用运动控制卡上的拨码开关设置卡号；系统启动后，系统BIOS为相应的卡自动分配物理空间。 

                    //【卡号和轴号的对应关系为】： 
                    // 0号卡对应0~3号轴；1号卡对应4~7号轴；n号卡对应4n~ 4*（n+1）-1号轴。
                    //【使用习惯从1开始计算轴数量，在调用函数时需要减去1才是正确的轴号。】
                    if (NumberOfTargetAxis > AvailableCardQty * 4
                        && NumberOfTargetAxis < 0)
                        {
                        MessageBox.Show("The value for target axis 'NumberOfTargetAxis' should be : "
                            + "1~" + (AvailableCardQty * 4) + " ,please revise the parameter and retry.\r\n"
                            + "目标轴号参数 'NumberOfTargetAxis' 取值范围：" + "1~" + (AvailableCardQty * 4)
                            + "请修改参数。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                        }
                    else
                        {
                        TargetAxis = (ushort)(NumberOfTargetAxis - 1);
                        }
                    
                    SuccessBuiltNew = true;

                    }
                else
                    {
                    SuccessBuiltNew = false;
                    PasswordIsCorrect = false;
                    MessageBox.Show("Right Prohibited.\r\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                    }

                }
            catch (Exception ex)
                {
                SuccessBuiltNew = false;
                MessageBox.Show("创建类的实例时出现错误\r\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }

            }

        //关闭控制卡并释放控制卡系统资源
        /// <summary>
        /// 关闭控制卡并释放控制卡系统资源
        /// </summary>
        /// <returns></returns>
        public int CloseCard() 
            {
            try
                {
                if (SuccessBuiltNew == false) 
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。");
                    return 1;
                    }

                UInt32 TempReturn = 0;
                TempReturn = d2410_board_close();
                return (int)TempReturn;

                }
            catch (Exception ex) 
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
        
        #endregion

        #region "脉冲输入输出配置"
        
        // DMC2410C卡可以输出两类指令脉冲信号：
        //一种为脉冲+方向模式（单脉冲模式）
        //一种为正脉冲+负脉冲模式（双脉冲模式）

        //设置轴的脉冲输出模式
        /// <summary>
        /// 设置轴的脉冲输出模式
        /// </summary>
        /// <param name="TargetPulseOutMode">脉冲输出方式选择[0~5]
        /// 0--正方向脉冲[PULSE输出正脉冲，DIR高电平]  负方向脉冲[PULSE输出正脉冲，DIR低电平]
        /// 1--正方向脉冲[PULSE输出负脉冲，DIR高电平]  负方向脉冲[PULSE输出负脉冲，DIR低电平]
        /// 2--正方向脉冲[PULSE输出正脉冲，DIR低电平]  负方向脉冲[PULSE输出正脉冲，DIR高电平]
        /// 3--正方向脉冲[PULSE输出负脉冲，DIR低电平]  负方向脉冲[PULSE输出负脉冲，DIR高电平]
        /// 4--正方向脉冲[PULSE输出正脉冲，DIR电电平]  负方向脉冲[PULSE输出高电平，DIR正脉冲]
        /// 5--正方向脉冲[PULSE输出负脉冲，DIR低电平]  负方向脉冲[PULSE输出高电平，DIR负脉冲]
        /// </param>
        /// <returns></returns>
        public int SetPulseOutMode(ushort TargetPulseOutMode) 
            {
            try
                {
                //注意：在调用运动函数（如：d2410_t_vmove等）输出脉冲之前，一定要根据驱动器接
                //收脉冲的模式调用d2410_set_pulse_outmode设置控制卡脉冲输出模式。
                //d2410_set_pulse_outmode Lib "DMC2410.dll" (ByVal axis As Int16, ByVal outmode As Int16) As Int32

                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                        ,"提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetPulseOutMode < 0
                    || TargetPulseOutMode > 5) 
                    {
                    MessageBox.Show("The parameter 'TargetPulseOutMode' should be 0~5, please revise it and retry.\r\n"
                        + "参数 'TargetPulseOutMode' 取值范围应该为0~5，请修改参数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }
                
                int TempReturn = 0;
                TempReturn = (int)d2410_set_pulse_outmode(TargetAxis, TargetPulseOutMode);
                return TempReturn;
                }
            catch (Exception ex) 
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置编码器的计数方式
        /// <summary>
        /// 设置编码器的计数方式
        /// </summary>
        /// <param name="EncoderCounterMode">编码器的计数方式：
        /// 0：非A/B相 (脉冲/方向)
        /// 1：1×A/B
        /// 2：2× A/B
        /// 3：4× A/B
        /// </param>
        /// <returns></returns>
        public int SetEncoderCounterMode(ushort EncoderCounterMode) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (EncoderCounterMode < 0
                    || EncoderCounterMode > 3) 
                    {
                    MessageBox.Show("The value for parameter 'EncoderCounterMode' should be 0~3, please revise it.\r\n"
                        + "参数 'EncoderCounterMode' 的取值范围是0~3，请修改参数。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_counter_config(TargetAxis, EncoderCounterMode);
                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
                
        #endregion

        #region "制动函数"

        //指定轴减速停止，调用此函数时立即减速后停止，停止时的速度是起始速度和停止速度中的较大值。
        /// <summary>
        /// 指定轴减速停止，调用此函数时立即减速后停止，
        /// 停止时的速度是起始速度和停止速度中的较大值。
        /// </summary>
        /// <param name="DecelerationTime">减速时间，单位：s</param>
        /// <returns></returns>
        public int StopDecel(double DecelerationTime) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_decel_stop(TargetAxis, DecelerationTime);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //使指定轴立即停止，没有任何减速的过程
        /// <summary>
        /// 使指定轴立即停止，没有任何减速的过程
        /// </summary>
        /// <returns></returns>
        public int StopImmediately() 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_imd_stop(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //使所有的运动轴紧急停止
        /// <summary>
        /// 使所有的运动轴紧急停止
        /// </summary>
        /// <returns></returns>
        public int StopAllAxisEmergent() 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_emg_stop();

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
                
        #endregion

        #region "状态检测函数"

        //检测指定轴的运动状态，停止或是在运行中
        /// <summary>
        /// 检测指定轴的运动状态，停止或是在运行中
        /// </summary>
        /// <returns></returns>
        public bool GetRunningStatus() 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                    }

                int TempReturn = 0;
                TempReturn = d2410_check_done(TargetAxis);
                if (TempReturn == 0)
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
                ErrorMessage = ex.Message;
                return false;
                }
            }

        //读取指定轴有关运动信号的状态【轴伺服报警、正限位、负限位、原点信号】，包含指定轴的专用I/O状态
        /// <summary>
        /// 读取指定轴有关运动信号的状态【轴伺服报警、正限位、负限位、原点信号】，
        /// 包含指定轴的专用I/O状态
        /// </summary>
        /// <returns>轴运动信号的状态数据结构</returns>
        public AxisSignal GetAxisSignalStatus() 
            {
            AxisSignal TempAxisSignal;
            TempAxisSignal.HomeSignal = false;
            TempAxisSignal.NegotiveLimit = false;
            TempAxisSignal.PositiveLimit = false;
            TempAxisSignal.ServoAlarm = false;
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return TempAxisSignal;
                    }

                int TempReturn = 0;

                //位号【从0开始】:其它, 信号名称: 保留
                TempReturn = (int)d2410_axis_io_status(TargetAxis);

                //位号:11, 信号名称: ALM, 1：表示伺服报警信号 ALM 为 ON
                //if ((TempReturn & (int)(Math.Pow(2, 11))) == 1)
                //    {
                //    TempAxisSignal.ServoAlarm = true;
                //    }
                //else 
                //    {
                //    TempAxisSignal.ServoAlarm = false;
                //    }

                if ((TempReturn >> (11-1)) == 1)
                    {
                    TempAxisSignal.ServoAlarm = true;
                    }
                else 
                    {
                    TempAxisSignal.ServoAlarm = false;
                    }

                //位号:12, 信号名称: PEL, 1：表示正限位信号 +EL 为 ON
                if ((TempReturn >> (12-1)) == 1)
                    {
                    TempAxisSignal.PositiveLimit = true;
                    }
                else
                    {
                    TempAxisSignal.PositiveLimit = false;
                    }

                //位号:13, 信号名称: NEL, 1：表示负限位信号–EL为 ON
                if ((TempReturn >> (13-1)) == 1)
                    {
                    TempAxisSignal.NegotiveLimit = true;
                    }
                else
                    {
                    TempAxisSignal.NegotiveLimit = false;
                    }

                //位号:14, 信号名称: ORG, 1：表示原点信号 ORG 为 ON
                if ((TempReturn >> (14-1)) == 1)
                    {
                    TempAxisSignal.HomeSignal = true;
                    }
                else
                    {
                    TempAxisSignal.HomeSignal = false;
                    }

                return TempAxisSignal;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return TempAxisSignal;
                }
            }

        //读取指定轴的外部信号状态【紧急停止信号(EMG)、索引信号(EZ)、+DR(PA)信号、-DR(PB)信号】
        /// <summary>
        /// 读取指定轴的外部信号状态
        /// 【紧急停止信号(EMG)、索引信号(EZ)、+DR(PA)信号、-DR(PB)信号】
        /// </summary>
        /// <returns>轴外部信号的状态数据结构</returns>
        public AxisOutsideSignal GetAxisOutsideSignalStatus() 
            {
            AxisOutsideSignal TempAxisOutsideSignal;
            TempAxisOutsideSignal.EStop=false;
            TempAxisOutsideSignal.EZIndexSignal=false;
            TempAxisOutsideSignal.NegativeDR_PB=false;
            TempAxisOutsideSignal.PositiveDR_PA=false;

            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return TempAxisOutsideSignal;
                    }

                int TempReturn = 0;

                //位号:其他位, 信号名称: 保留
                //位号:0~3, 信号名称: 保留
                TempReturn = (int)d2410_axis_io_status(TargetAxis);

                //位号:7, 信号名称: EMG, 1：表示紧急停止信号（EMG）为 ON
                if ((TempReturn >> (7-1)) == 1)
                    {
                    TempAxisOutsideSignal.EStop = true;
                    }
                else 
                    {
                    TempAxisOutsideSignal.EStop = false;
                    }

                //位号:10, 信号名称: EZ, 1：表示索引信号（EZ）为 ON
                if ((TempReturn >> (10-1)) == 1)
                    {
                    TempAxisOutsideSignal.EZIndexSignal = true;
                    }
                else 
                    {
                    TempAxisOutsideSignal.EZIndexSignal = false;
                    }

                //位号:11, 信号名称: +DR(PA), 1：表示 +DR(PA) 信号为 ON
                if ((TempReturn >> (11-1)) == 1)
                    {
                    TempAxisOutsideSignal.PositiveDR_PA = true;
                    }
                else 
                    {
                    TempAxisOutsideSignal.PositiveDR_PA = false;
                    }

                //位号:12, 信号名称: -DR(PB), 1：表示 -DR(PB) 信号为 ON
                if ((TempReturn >> (12-1)) == 1)
                    {
                    TempAxisOutsideSignal.NegativeDR_PB = true;
                    }
                else 
                    {
                    TempAxisOutsideSignal.NegativeDR_PB = false;
                    }

                return TempAxisOutsideSignal;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return TempAxisOutsideSignal;
                }
            }
        
        #endregion
        
        #region "位置设置和读取函数"

        //读取轴的当前位置脉冲数【单位：pulse】
        /// <summary>
        /// 读取轴的当前位置脉冲数【单位：pulse】 
        /// </summary>
        /// <returns></returns>
        public int GetCurrentPosInPulse() 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_position(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //读取轴的当前位置【单位：mm】
        /// <summary>
        /// 读取轴的当前位置【单位：mm】
        /// </summary>
        /// <param name="CurrentPosInMM">轴的当前位置【单位：mm】</param>
        /// <returns></returns>
        public int GetCurrentPosInMM(ref double CurrentPosInMM) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_position_unitmm(TargetAxis, ref CurrentPosInMM);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取轴的目标脉冲位置【绝对坐标】，单位：pulse
        /// <summary>
        /// 读取轴的目标脉冲位置【绝对坐标】，单位：pulse
        /// </summary>
        /// <returns>轴的目标脉冲位置【绝对坐标】，单位：pulse</returns>
        public int GetABSPosInPulse() 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_target_position(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //设定指定轴的当前位置【单位：pulse】【常用于回到原点后位置清零】
        /// <summary>
        /// 设定指定轴的当前位置【单位：pulse】【常用于回到原点后位置清零】
        /// </summary>
        /// <param name="ABSValueForCurrentPosition">绝对位置值【单位：pulse】</param>
        /// <returns></returns>
        public int SetABSPosInPulse(Int32 ABSValueForCurrentPosition) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_position(TargetAxis, ABSValueForCurrentPosition);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设定指定轴的当前位置【单位：mm】【常用于回到原点后位置清零】
        /// <summary>
        /// 设定指定轴的当前位置【单位：mm】【常用于回到原点后位置清零】
        /// </summary>
        /// <param name="ValueForCurrentABSPosInMM">绝对位置值【单位：mm】</param>
        /// <returns></returns>
        public int SetABSPosInMM(double ValueForCurrentABSPosInMM) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_position_unitmm(TargetAxis, ValueForCurrentABSPosInMM);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
        
        #endregion
        
        #region "速度设置和读取"
        
        //设定梯形曲线的起始速度、运行速度、加速时间、减速时间
        /// <summary>
        /// 设定梯形曲线的起始速度、运行速度、加速时间、减速时间
        /// </summary>
        /// <param name="MinVelocity">起始速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="TotalAccTime">总加速时间，单位：s</param>
        /// <param name="TotalDecTime">总减速时间，单位：s</param>
        /// <returns>错误代码</returns>
        public int SetTCurveProfile(double MinVelocity, double MaxVelocity,
            double TotalAccTime, double TotalDecTime) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_profile(TargetAxis, MinVelocity,
                    MaxVelocity, TotalAccTime, TotalDecTime);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //Set_T_Profile扩展函数，增加停止速度的设定
        /// <summary>
        /// Set_T_Profile扩展函数，增加停止速度的设定
        /// </summary>
        /// <param name="MinVelocity">起始速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="TotalAccTime">总加速时间，单位：s</param>
        /// <param name="TotalDecTime">总减速时间，单位：s</param>
        /// <param name="StopVelocity">停止速度，单位：pulse/s</param>
        /// <returns>错误代码</returns>
        public int SetTCurveProfileExtern(double MinVelocity, double MaxVelocity,
            double TotalAccTime, double TotalDecTime, double StopVelocity) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_profile_Extern(TargetAxis, MinVelocity,
                    MaxVelocity, TotalAccTime, TotalDecTime, StopVelocity);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设定ST形曲线运动，起始速度和停止速度相同
        /// <summary>
        /// 设定ST形曲线运动，起始速度和停止速度相同
        /// </summary>
        /// <param name="MinVelocity">起始速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="TotalAccTime">总加速时间，单位：s</param>
        /// <param name="TotalDecTime">总减速时间，单位：s</param>
        /// <param name="STAccTime">S段时间，单位：s，范围[0,50]ms</param>
        /// <param name="STDecTime">保留参数</param>
        /// <returns>错误代码</returns>
        public int SetSTCurveProfile(double MinVelocity, double MaxVelocity,
            double TotalAccTime, double TotalDecTime, double STAccTime, double STDecTime) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_st_profile(TargetAxis, MinVelocity,
                    MaxVelocity, TotalAccTime, TotalDecTime, STAccTime, STDecTime);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设定S形曲线运动，起始速度和停止速度相同
        /// <summary>
        /// 设定S形曲线运动，起始速度和停止速度相同
        /// </summary>
        /// <param name="MinVelocity">起始速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="TotalAccTime">总加速时间，单位：s</param>
        /// <param name="TotalDecTime">总减速时间，单位：s</param>
        /// <param name="SAccTime">S段时间，单位：s，范围[0,50]ms</param>
        /// <param name="SDecTime">保留参数</param>
        /// <returns>错误代码</returns>
        public int SetSCurveProfile(double MinVelocity, double MaxVelocity,
            double TotalAccTime, double TotalDecTime, int SAccTime, int SDecTime) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_s_profile(TargetAxis, MinVelocity,
                    MaxVelocity, TotalAccTime, TotalDecTime, SAccTime, SDecTime);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //Set_ST_Profile扩展函数，增加停止速度的设定
        /// <summary>
        /// Set_ST_Profile扩展函数，增加停止速度的设定
        /// </summary>
        /// <param name="MinVelocity">起始速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="TotalAccTime">总加速时间，单位：s</param>
        /// <param name="TotalDecTime">总减速时间，单位：s</param>
        /// <param name="SAccTime">S段时间，单位：s，范围[0,50] ms</param>
        /// <param name="SDecTime">保留参数</param>
        /// <param name="StopVelocity">停止速度</param>
        /// <returns>错误代码</returns>
        public int SetSTCurveProfileExtern(double MinVelocity, double MaxVelocity,
            double TotalAccTime, double TotalDecTime, double SAccTime,
            double SDecTime, double StopVelocity) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_st_profile_Extern(TargetAxis,
                    MinVelocity, MaxVelocity, TotalAccTime, TotalDecTime, SAccTime, 
                    SDecTime, StopVelocity);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设定插补矢量运动曲线的S段参数、运行速度、加速时间、减速时间
        /// <summary>
        /// 设定插补矢量运动曲线的S段参数、运行速度、加速时间、减速时间
        /// </summary>
        /// <param name="MinVelocity">保留参数</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="TotalAccTime">总加速时间，单位：s</param>
        /// <param name="TotalDecTime">总减速时间，单位：s</param>
        /// <returns>错误代码</returns>
        public int SetVectorProfile(double MinVelocity, double MaxVelocity,
            double TotalAccTime, double TotalDecTime) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_vector_profile(MinVelocity, MaxVelocity,
                    TotalAccTime, TotalDecTime);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取当前速度值，单位：pulse/s
        //【注意：当执行插补运动时，调用该函数读取的为矢量速度】
        /// <summary>
        /// 读取当前速度值，单位：pulse/s
        /// 【注意：当执行插补运动时，调用该函数读取的为矢量速度】
        /// </summary>
        /// <returns>轴的当前运行速度【单位：pulse/s】</returns>
        public double GetCurrentSpeed() 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_read_current_speed(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //读取当前速度值，单位：mm/s
        //【注意：当执行插补运动时，调用该函数读取的为矢量速度】
        /// <summary>
        /// 读取当前速度值，单位：mm/s
        /// 【注意：当执行插补运动时，调用该函数读取的为矢量速度】
        /// </summary>
        /// <param name="CurrentSpeedInMMPerSecond">轴的运行速度【单位：mm/s】</param>
        /// <returns></returns>
        public int GetCurrentSpeed(ref double CurrentSpeedInMMPerSecond) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_read_current_speed_unitmm(TargetAxis, 
                    ref CurrentSpeedInMMPerSecond);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取当前控制卡的矢量速度值，单位：pulse/s
        /// <summary>
        /// 读取当前控制卡的矢量速度值，单位：pulse/s
        /// </summary>
        /// <returns>轴的运行矢量速度【单位：pulse/s】</returns>
        public double GetCurrentVectorSpeed() 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                double TempReturn = 0;
                TempReturn = d2410_read_vector_speed(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //读取指定控制卡的矢量速度值，单位：pulse/s
        /// <summary>
        /// 读取指定控制卡的矢量速度值，单位：pulse/s
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <returns>指定控制卡轴的运行矢量速度【单位：pulse/s】</returns>
        public double GetCurrentVectorSpeed(ushort TargetCardNumber) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }
                
                //【卡号和轴号的对应关系为】： 
                // 0号卡对应0~3号轴；1号卡对应4~7号轴；n号卡对应4n~ 4*（n+1）-1号轴。
                // 为方便编程时理解, 卡号从1开始, 然后在函数里面进行减1得到实际的卡号
                if(TargetCardNumber < 1 
                    || TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("The parameter 'TargetCardNumber' should be 1~" 
                        + AvailableCardQty + ", please revise it.");
                    return 0;
                    }

                ushort TempCard = (ushort)(TargetCardNumber - 1);

                double TempReturn = 0.0;
                TempReturn = d2410_read_vector_speed(TempCard);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        #endregion
        
        #region "在线变速/变位"

        //在线改变轴的当前运动速度【单位：pulse/s】。该函数只适用于单轴运动中的变速
        /// <summary>
        /// 在线改变轴的当前运动速度【单位：pulse/s】。
        /// 该函数只适用于单轴运动中的变速。
        /// 注意：
        /// (1)变速一旦成立，该轴的默认运行速度将会被改写为NewVelocity，
        ///    也即当调用get_profile回读速度参数时会发生与set_profile所设置的不一致的现象;
        /// (2)在单轴速度运动中NewVelocity负值表示往负向变速，正值表示往正向变速。
        ///    在单轴定长运动中NewVelocity只允许正值。
        /// </summary>
        /// <param name="NewVelocity">新的运行速度【单位：pulse/s】</param>
        /// <returns>错误代码</returns>
        public int ChangeSpeed(double NewVelocity) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_change_speed(TargetAxis, NewVelocity);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //在单轴绝对运动中改变目标位置【单位：pulse】
        /// <summary>
        /// 在单轴绝对运动中改变目标位置【单位：pulse】
        /// 注意：参数NewABSPos为绝对位置值，无论当前的运动模式为绝对坐标还是相对坐标模式。
        /// </summary>
        /// <param name="NewABSPos">新绝对位置值</param>
        /// <returns>错误代码</returns>
        public int ChangeTargetABSPos(int NewABSPos) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_reset_target_position(TargetAxis, NewABSPos);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        #endregion
        
        #region "单轴位置控制函数"

        //在DMC2410C函数库中距离或位置的单位为脉冲；速度单位为脉冲/秒；时间单位为秒。
        //最基本的位置控制是指从当前位置运动到另一个位置，一般称为点位运动或定长运动。 
        //DMC2410C卡在执行单轴控制时，可使电机按照梯形速度曲线或S形速度曲线进行点位运动或连续运动。

        //注：“对称梯形速度曲线”是指其加速度和减速度相对，即加速过程和减速过程的曲线斜率对称。

        //梯形速度曲线下的点位运动
        //d2410_set_profile              设定梯形速度曲线的起始速度、最大速度、加速时间、减速时间
        //d2410_set_profile_Extern       设定梯形速度曲线的起始速度、最大速度、停止速度、加速时间、减速时间
        //d2410_t_pmove                  让指定轴以对称梯形速度曲线作点位运动
        //d2410_ex_t_pmove               让指定轴以非对称梯形速度曲线作点位运动

        //例程7.3：执行以非对称梯形速度曲线作点位运动 
        //d2410_set_profile 0,500,6000,0.02,0.01     '设置0号轴起始速度为500脉冲/秒 
        //'运行速度为6000脉冲/秒、加速时间为0.02秒、
        //'减速时间为0.01秒。 
        //d2410_ex_t_pmove 0,50000,0                 '设置0号轴、运

        //**************************
        //在单轴运行过程中，最大速度Max_Vel和目标位置Dist均可以实时改变
        //**************************

        //梯形速度下改变速度、终点的相关函数说明
        //d2410_change_speed                  '单轴运行中改变当前最大速度
        //d2410_reset_target_position         '改变目标位置

        //例程7.4：改变速度、改变终点位置
        //d2410_set_profile 0,500,6000,0.01,0.02     '设置梯形曲线速度、加、减速时间*/ 
        //d2410_ex_t_pmove 0,50000,0                 '设置轴号、运动距离50000、相对坐标模式 
        //If(“改变速度条件”)                       '如果改变速度条件满足，则执行改变速度命令 
        //Curr_Vel= 9000                             '设置新的速度 
        //d2410_change_speed 0,Curr_Vel              '执行改变速度指令 
        //End If 
        //If(“改变终点位置条件”)                   '如果改变终点位置条件满足，则执行改变终点位置命令 
        //d2410_reset_target_position 0,55000        '改变终点位置为55000 
        //End If 

        //如果将运动中的最大速度设置得小于起始速度，整个运动过程中将会以最大速度作恒速运动。 
        //如果运动距离很短，当距离小于或等于(Max_Vel+Min_Vel)×Tacc时，理论上速度曲线将变为三角形；
        //但DMC2410C运动控制卡有自动调整功能，将三角形的尖峰削去，以避免速度变化太大发生冲击现象

        //S形速度曲线运动模式
        //d2410_set_st_profile                '设置S段时间，起始速度和停止速度相同
        //d2410_set_st_profile_Extern         '设置S段时间，有停止速度
        //d2410_s_pmove                       '让指定轴以对称S形速度曲线作点位运动
        //d2410_ex_s_pmove                    '让指定轴以非对称S形速度曲线作点位运动

        //在S形速度曲线下的点位运动过程中，也可以调用d2410_change_speed和d2410_reset_target_position函数
        //实时改变运行速度和目标位置。但多轴插补运行情况下不能实时改变运行速度和目标位置。

        //连续运动模式
        //连续运动模式中，DMC2410C控制卡可以控制电机以梯形或S形速度曲线在指定的加速时间内从起始速度加速至最大速度，
        //然后以该速度连续运行，直至调用停止指令或者该轴遇到限位信号才会按启动时的速度曲线减速停止。

        //连续运动相关函数说明
        //d2410_t_vmove      '让指定轴以梯形速度曲线加速到最大速度后，连续运行
        //d2410_s_vmove      '让指定轴以S形速度曲线加速到最大速度后，连续运行
        //d2410_decel_stop   '指定轴减速停止。调用此函数后立即减速，到达起始速度后停止

        //在单轴执行连续运动过程中，可以调用d2410_change_speed 实时改变速度。
        //注意：在以S形速度曲线连续运动时，改变最大速度最好在加速过程已经完成的恒速段进行。

        //例程7.5：以S形速度曲线加速的连续运动及变速、停止控制
        //d2410_set_st_profile 0,100, 1000,0.1,0.1,0.01,0      '设置S形曲线S段时间 
        //d2410_s_vmove 0,1                                    '0号轴连续运动，方向为正 
        //if(“改变速度条件”)                                 '如果改变速度条件满足，则执行改变速度命令 
        //Curr_Vel= 1200                                       '设置新的速度 
        //d2410_change_speed 0,Curr_Vel                        '执行改变速度指令 
        //End If 
        //if(“停止条件”)                                     '如果运动停止条件满足，则执行减速停止命令 
        //d2410_decel_stop 0,0.1                               '减速停止，减速时间为0.1秒
        //End If

        //
        
        /// <summary>
        /// 使轴以T形速度曲线加速到高速，并持续运行下去
        /// </summary>
        /// <param name="MoveDirection">指定运动的方向：0：负方向，1：正方向</param>
        /// <param name="MinVelocity">最小速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="TotalAccTime">总加速时间，单位：s</param>
        /// <param name="TotalDecTime">总减速时间，单位：s</param>
        /// <returns>错误代码</returns>
        public int MoveTCurve(ushort MoveDirection, double MinVelocity, 
            double MaxVelocity, double TotalAccTime, double TotalDecTime) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if(MoveDirection!=0)
                    {
                    MoveDirection = 1;
                    }
                
                //设定梯形曲线的起始速度、运行速度、加速时间、减速时间
                //<param name="axis">指定轴号</param>
                //<param name="Min_Vel">起始速度，单位：pulse/s</param>
                //<param name="Max_Vel">最大速度，单位：pulse/s</param>
                //<param name="Tacc">总加速时间，单位：s</param>
                //<param name="Tdec">总减速时间，单位：s</param>
                //Declare Function d2410_set_profile Lib "DMC2410.dll" (ByVal axis As Int16, ByVal Min_Vel As Double, 
                //ByVal Max_Vel As Double, ByVal Tacc As Double, ByVal Tdec As Double) As Int32

                int TempReturn = 0;
                TempReturn = (int)d2410_set_profile(TargetAxis, MinVelocity, MaxVelocity,
                    TotalAccTime, TotalDecTime);
                TempReturn += (int)d2410_t_vmove(TargetAxis, MoveDirection);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //使轴以S形速度曲线加速到高速，并持续运行下去
        /// <summary>
        /// 使轴以S形速度曲线加速到高速，并持续运行下去
        /// </summary>
        /// <param name="MoveDirection">指定运动的方向：0：负方向，1：正方向</param>
        /// <param name="MinVelocity">最小速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="TotalAccTime">总加速时间，单位：s</param>
        /// <param name="TotalDecTime">总减速时间，单位：s</param>
        /// <param name="StopVelocity">停止速度，单位：pulse/s</param>
        /// <returns>错误代码</returns>
        public int MoveSCurve(ushort MoveDirection, double MinVelocity, 
            double MaxVelocity, double TotalAccTime, double TotalDecTime,
            double StopVelocity) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }
                
                if(MoveDirection!=0)
                    {
                    MoveDirection = 1;
                    }
                
                //d2410_set_profile扩展函数，增加停止速度的设定
                //<param name="axis">参加运动的轴号</param>
                //<param name="Min_Vel">起始速度，单位：pulse/s</param>
                //<param name="Max_Vel">最大速度，单位：pulse/s</param>
                //<param name="Tacc">总加速时间，单位：s</param>
                //<param name="Tdec">总减速时间，单位：s</param>
                //<param name="Stop_Vel">停止速度，单位：pulse/s</param>
                //Declare Function d2410_set_profile_Extern Lib "DMC2410.dll" (ByVal axis As Int16, ByVal Min_Vel As Double,
                //ByVal Max_Vel As Double, ByVal Tacc As Double, ByVal Tdec As Double, ByVal Stop_Vel As Double) As Int32


                int TempReturn = 0;
                TempReturn = (int)d2410_set_profile_Extern(TargetAxis,
                    MinVelocity, MaxVelocity, TotalAccTime, TotalDecTime, StopVelocity);
                TempReturn = (int)d2410_s_vmove(TargetAxis, MoveDirection);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //使轴以对称梯形速度曲线做点位运动
        /// <summary>
        /// 使轴以对称梯形速度曲线做点位运动
        /// </summary>
        /// <param name="MoveDistance">位移量（绝对/相对），单位：pulse</param>
        /// <param name="MoveMode">位移模式设定：
        /// 0：相对位移
        /// 1：绝对位移
        /// </param>
        /// <param name="MinVelocity">最小速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="TotalAccTime">总加速时间，单位：s</param>
        /// <param name="TotalDecTime">总减速时间，单位：s</param>
        /// <param name="StopVelocity">停止速度，单位：pulse/s</param>
        /// <returns>错误代码</returns>
        public int MoveSymmetricalTCurve(int MoveDistance, ushort MoveMode,
            double MinVelocity, double MaxVelocity, double TotalAccTime, 
            double TotalDecTime, double StopVelocity) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if(MoveMode != 0)
                    {
                    MoveMode = 1;
                    }
                
                //d2410_set_profile扩展函数，增加停止速度的设定
                //<param name="axis">参加运动的轴号</param>
                //<param name="Min_Vel">起始速度，单位：pulse/s</param>
                //<param name="Max_Vel">最大速度，单位：pulse/s</param>
                //<param name="Tacc">总加速时间，单位：s</param>
                //<param name="Tdec">总减速时间，单位：s</param>
                //<param name="Stop_Vel">停止速度，单位：pulse/s</param>
                //Declare Function d2410_set_profile_Extern Lib "DMC2410.dll" (ByVal axis As Int16, ByVal Min_Vel As Double,
                //ByVal Max_Vel As Double, ByVal Tacc As Double, ByVal Tdec As Double, ByVal Stop_Vel As Double) As Int32
                
                int TempReturn = 0;
                TempReturn = (int)d2410_set_profile_Extern(TargetAxis, MinVelocity,
                    MaxVelocity, TotalAccTime, TotalDecTime, StopVelocity);
                TempReturn += (int)d2410_t_pmove(TargetAxis, MoveDistance, MoveMode);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //使轴以对称S形速度曲线做点位运动
        /// <summary>
        /// 使轴以对称S形速度曲线做点位运动
        /// </summary>
        /// <param name="MoveDistance">位移量（绝对/相对），单位：pulse</param>
        /// <param name="MoveMode">>位移模式设定：
        /// 0：相对位移
        /// 1：绝对位移
        /// </param>
        /// <param name="MinVelocity">最小速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="TotalAccTime">总加速时间，单位：s</param>
        /// <param name="TotalDecTime">总减速时间，单位：s</param>
        /// <param name="StopVelocity">停止速度，单位：pulse/s</param>
        /// <returns>错误代码</returns>
        public int MoveSymmetricalSCurve(int MoveDistance, ushort MoveMode,
            double MinVelocity, double MaxVelocity, double TotalAccTime, 
            double TotalDecTime, double StopVelocity) 
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }
                
                if(MoveMode != 0)
                    {
                    MoveMode = 1;
                    }
                
                //d2410_set_profile扩展函数，增加停止速度的设定
                //<param name="axis">参加运动的轴号</param>
                //<param name="Min_Vel">起始速度，单位：pulse/s</param>
                //<param name="Max_Vel">最大速度，单位：pulse/s</param>
                //<param name="Tacc">总加速时间，单位：s</param>
                //<param name="Tdec">总减速时间，单位：s</param>
                //<param name="Stop_Vel">停止速度，单位：pulse/s</param>
                //Declare Function d2410_set_profile_Extern Lib "DMC2410.dll" (ByVal axis As Int16, ByVal Min_Vel As Double,
                //ByVal Max_Vel As Double, ByVal Tacc As Double, ByVal Tdec As Double, ByVal Stop_Vel As Double) As Int32

                int TempReturn = 0;
                TempReturn = (int)d2410_set_profile_Extern(TargetAxis,
                    MinVelocity, MaxVelocity, TotalAccTime, TotalDecTime, StopVelocity);
                TempReturn += (int)d2410_s_pmove(TargetAxis, MoveDistance, MoveMode);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //使轴以非对称梯形速度曲线做点位运动
        /// <summary>
        /// 使轴以非对称梯形速度曲线做点位运动
        /// </summary>
        /// <param name="MoveDistance">位移量（绝对/相对），单位：pulse</param>
        /// <param name="MoveMode">位移模式设定：0：相对位移，1：绝对位移</param>
        /// <param name="MinVelocity">最小速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="TotalAccTime">总加速时间，单位：s</param>
        /// <param name="TotalDecTime">总减速时间，单位：s</param>
        /// <param name="StopVelocity">停止速度，单位：pulse/s</param>
        /// <returns>错误代码</returns>
        public int MoveFreeTCurve(int MoveDistance, ushort MoveMode,
            double MinVelocity, double MaxVelocity, double TotalAccTime,
            double TotalDecTime, double StopVelocity)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (MoveMode != 0)
                    {
                    MoveMode = 1;
                    }
                
                //d2410_set_profile扩展函数，增加停止速度的设定
                //<param name="axis">参加运动的轴号</param>
                //<param name="Min_Vel">起始速度，单位：pulse/s</param>
                //<param name="Max_Vel">最大速度，单位：pulse/s</param>
                //<param name="Tacc">总加速时间，单位：s</param>
                //<param name="Tdec">总减速时间，单位：s</param>
                //<param name="Stop_Vel">停止速度，单位：pulse/s</param>
                //Declare Function d2410_set_profile_Extern Lib "DMC2410.dll" (ByVal axis As Int16, ByVal Min_Vel As Double,
                //ByVal Max_Vel As Double, ByVal Tacc As Double, ByVal Tdec As Double, ByVal Stop_Vel As Double) As Int32

                int TempReturn = 0;
                TempReturn = (int)d2410_set_profile_Extern(TargetAxis,
                    MinVelocity, MaxVelocity, TotalAccTime, TotalDecTime, StopVelocity);
                TempReturn += (int)d2410_ex_t_pmove(TargetAxis, MoveDistance, MoveMode);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //使轴以非对称S形速度曲线做点位运动
        /// <summary>
        /// 使轴以非对称S形速度曲线做点位运动
        /// </summary>
        /// <param name="MoveDistance">位移量（绝对/相对），单位：pulse</param>
        /// <param name="MoveMode">位移模式设定：0：相对位移，1：绝对位移</param>
        /// <param name="MinVelocity">最小速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="TotalAccTime">总加速时间，单位：s</param>
        /// <param name="TotalDecTime">总减速时间，单位：s</param>
        /// <param name="StopVelocity">停止速度，单位：pulse/s</param>
        /// <returns>错误代码</returns>
        public int MoveFreeSCurve(int MoveDistance, ushort MoveMode,
            double MinVelocity, double MaxVelocity, double TotalAccTime,
            double TotalDecTime, double StopVelocity)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (MoveMode != 0)
                    {
                    MoveMode = 1;
                    }
                
                //d2410_set_profile扩展函数，增加停止速度的设定
                //<param name="axis">参加运动的轴号</param>
                //<param name="Min_Vel">起始速度，单位：pulse/s</param>
                //<param name="Max_Vel">最大速度，单位：pulse/s</param>
                //<param name="Tacc">总加速时间，单位：s</param>
                //<param name="Tdec">总减速时间，单位：s</param>
                //<param name="Stop_Vel">停止速度，单位：pulse/s</param>
                //Declare Function d2410_set_profile_Extern Lib "DMC2410.dll" (ByVal axis As Int16, ByVal Min_Vel As Double,
                //ByVal Max_Vel As Double, ByVal Tacc As Double, ByVal Tdec As Double, ByVal Stop_Vel As Double) As Int32

                int TempReturn = 0;
                TempReturn = (int)d2410_set_profile_Extern(TargetAxis, MinVelocity,
                    MaxVelocity, TotalAccTime, TotalDecTime, StopVelocity);
                TempReturn += (int)d2410_ex_s_pmove(TargetAxis, MoveDistance, MoveMode);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //轴正转
        /// <summary>
        /// 轴正转
        /// </summary>
        /// <param name="Velocity">速度，单位：pulse/s</param>
        /// <returns>错误代码</returns>
        public int JogPositive(int Velocity)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }
                
                //设定梯形曲线的起始速度、运行速度、加速时间、减速时间
                //<param name="axis">指定轴号</param>
                //<param name="Min_Vel">起始速度，单位：pulse/s</param>
                //<param name="Max_Vel">最大速度，单位：pulse/s</param>
                //<param name="Tacc">总加速时间，单位：s</param>
                //<param name="Tdec">总减速时间，单位：s</param>
                //Declare Function d2410_set_profile Lib "DMC2410.dll" (ByVal axis As Int16, ByVal Min_Vel As Double, 
                //ByVal Max_Vel As Double, ByVal Tacc As Double, ByVal Tdec As Double) As Int32

                int TempReturn = 0;
                TempReturn = (int)d2410_set_profile(TargetAxis, 100, Velocity, 0.1, 0.1);
                
                //使指定轴以T形速度曲线加速到高速，并持续运行下去
                //<param name="dir">指定运动的方向：0：负方向，1：正方向</param>
                //Declare Function d2410_t_vmove Lib "DMC2410.dll" (ByVal axis As Int16, ByVal dir As Int16) As Int32

                TempReturn += (int)d2410_t_vmove(TargetAxis, 1);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //轴反转
        /// <summary>
        /// 轴反转
        /// </summary>
        /// <param name="Velocity">速度，单位：pulse/s</param>
        /// <returns>错误代码</returns>
        public int JogNegative(int Velocity)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                //设定梯形曲线的起始速度、运行速度、加速时间、减速时间
                //<param name="axis">指定轴号</param>
                //<param name="Min_Vel">起始速度，单位：pulse/s</param>
                //<param name="Max_Vel">最大速度，单位：pulse/s</param>
                //<param name="Tacc">总加速时间，单位：s</param>
                //<param name="Tdec">总减速时间，单位：s</param>
                //Declare Function d2410_set_profile Lib "DMC2410.dll" (ByVal axis As Int16, ByVal Min_Vel As Double, 
                //ByVal Max_Vel As Double, ByVal Tacc As Double, ByVal Tdec As Double) As Int32

                int TempReturn = 0;
                TempReturn = (int)d2410_set_profile(TargetAxis, 100, 
                    Velocity, 0.1, 0.1);

                //使指定轴以T形速度曲线加速到高速，并持续运行下去
                //<param name="dir">指定运动的方向：0：负方向，1：正方向</param>
                //Declare Function d2410_t_vmove Lib "DMC2410.dll" (ByVal axis As Int16, ByVal dir As Int16) As Int32

                TempReturn += (int)d2410_t_vmove(TargetAxis, 0);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        #endregion

        #region "找原点"

        //设定轴的回原点模式
        /// <summary>
        /// 设定轴的回原点模式
        /// </summary>
        /// <param name="SearchHomeMode">回原点的信号模式：
        /// 0：只计home 
        /// 1：计home和EZ，计1个EZ信号
        /// 2：一次回零加回找
        /// 3：二次回零
        /// 4：EZ单独回零 
        /// 5：原点捕获回零
        /// </param>
        /// <param name="EZ_Count">EZ信号出现EZ_count指定的次数后，轴运动停止。
        /// 仅当SearchHomeMode=4时该参数设置有效，取值范围：1－16
        /// </param>
        /// <returns>错误代码</returns>
        public int SetSearchHomeMode(ushort SearchHomeMode, ushort EZ_Count)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (SearchHomeMode < 0
                    || SearchHomeMode > 5) 
                    {
                    MessageBox.Show("The parameter 'SearchHomeMode' should be 0~5, please revise it.");
                    return 1;                    
                    }

                if (SearchHomeMode == 4) 
                    {
                    if (EZ_Count < 1
                        || EZ_Count > 16) 
                        {
                        MessageBox.Show("The parameter 'EZ_Count' should be 1~16, please revise it.");
                        return 1;
                        }
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_config_home_mode(TargetAxis, SearchHomeMode, EZ_Count);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //单轴回原点运动
        /// <summary>
        /// 单轴回原点运动
        /// </summary>
        /// <param name="SearchHomeDirection">回原点方式：1：正方向回原点，2：负方向回原点</param>
        /// <param name="VelocityMode">回原点速度：0：低速回原点，1：高速回原点</param>
        /// <returns>错误代码</returns>
        public int SearchHome(ushort SearchHomeDirection, ushort VelocityMode)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (SearchHomeDirection < 1
                    || SearchHomeDirection > 2)
                    {
                    MessageBox.Show("The parameter 'SearchHomeDirection' should be 1~2, please revise it.");
                    return 1;
                    }

                if (VelocityMode < 0 || VelocityMode > 1) 
                    {
                    MessageBox.Show("The parameter 'VelocityMode' should be 0~1, please revise it.");
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_home_move(TargetAxis, SearchHomeDirection, VelocityMode);

                while (d2410_check_done(TargetAxis) == 0) 
                    {
                    Application.DoEvents();
                    }

                TempReturn = (int)d2410_set_position(TargetAxis, 0);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置原点锁存方式
        /// <summary>
        /// 设置原点锁存方式
        /// 注意：回零运动中，当选择回零模式为5时，用SetHomeLatchMode函数设置原点信号锁存方式
        /// </summary>
        /// <param name="Enable">原点锁存使能状态：0：禁止，2：允许</param>
        /// <param name="Logic">原点锁存方式：0：下降沿锁存，1：上升沿锁存</param>
        /// <returns>错误代码</returns>
        public int SetHomeLatchMode(ushort Enable, ushort Logic)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (Enable != 0 || Enable != 2) 
                    {
                    MessageBox.Show("The parameter 'Enable' should be 0 or 2, please revise it.");
                    return 1;
                    }

                if (Logic != 0) 
                    {
                    Logic = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_homelatch_mode(TargetAxis, Enable, Logic);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取原点锁存方式
        /// <summary>
        /// 读取原点锁存方式
        /// </summary>
        /// <param name="Enable">原点锁存使能状态：0：禁止，2：允许</param>
        /// <param name="Logic">原点锁存方式：0：下降沿锁存，1：上升沿锁存</param>
        /// <returns>错误代码</returns>
        public int GetHomeLatchMode(ref ushort Enable, ref ushort Logic)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_homelatch_mode(TargetAxis, ref Enable, ref Logic);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取原点锁存标志
        /// <summary>
        /// 读取原点锁存标志
        /// </summary>
        /// <returns>原点锁存标志：0：未触发，1：触发</returns>
        public int GetHomeLatchFlag()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_homelatch_flag(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //复位原点锁存标志
        /// <summary>
        /// 复位原点锁存标志
        /// </summary>
        /// <returns>错误代码</returns>
        public int ResetHomeLatchFlag()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_reset_homelatch_flag(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取原点锁存值（原点锁存位置为指令脉冲位置）
        /// <summary>
        /// 读取原点锁存值（原点锁存位置为指令脉冲位置）
        /// </summary>
        /// <returns>原点锁存值</returns>
        public int GetHomeLatchValue()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_homelatch_value(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //执行回原点运动【单独线程】
        /// <summary>
        /// 执行回原点运动【单独线程】
        /// 注意：回原点运动中，当选择回零模式为1~5时，要设置原点信号有效电平
        /// </summary>
        /// <param name="SearchHomeMode">回原点模式【1~6】
        /// 方式1：一次回零
        /// 方式2：一次回零加回找
        /// 方式3：两次回零
        /// 方式4：一次回零后再记1个EZ脉冲进行回零
        /// 方式5：EZ单独回零
        /// 方式6：原点捕获回零
        /// </param>
        /// <param name="SearchHomeDirection">回原点方式：1：正方向回原点，2：负方向回原点</param>
        /// <param name="VelocityMode">回原点速度：0：低速回原点，1：高速回原点</param>
        /// <param name="MinVelocity">回原点起始速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">回原点最大速度，单位：pulse/s</param>
        /// <param name="TotalAccTime">回原点总加速时间，单位：s</param>
        /// <param name="TotalDecTime">回原点总减速时间，单位：s</param>
        /// <param name="OrgLogic">原点信号的有效电平：0：低电平有效，1：高电平有效</param>
        /// <param name="EnableFilter">允许/禁止滤波功能：0：禁止，1：允许</param>
        /// <param name="EZCount">EZ信号出现EZCount指定的次数后，轴运动停止。仅当Mode=5时该参数设置有效，取值范围：1－16</param>
        /// <param name="EnableLatch">原点锁存使能状态：0：禁止，2：允许</param>
        /// <param name="LatchLogic">原点锁存方式：0：下降沿锁存，1：上升沿锁存</param>
        public void GoHome(ushort SearchHomeMode, ushort SearchHomeDirection=1,
            ushort VelocityMode = 0, double MinVelocity = 400, double MaxVelocity=1000,
            double TotalAccTime = 0.1, double TotalDecTime = 0.1, ushort OrgLogic=1,
            ushort EnableFilter = 1, ushort EZCount = 1, ushort EnableLatch=2, ushort LatchLogic=0)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                    }

                //回原点模式【1~6】
                if (SearchHomeMode < 1 || SearchHomeMode > 6) 
                    {
                    MessageBox.Show("The parameter 'SearchHomeMode' should be 1~6, please revise it.");
                    return;
                    }

                //回原点方式：1：正方向回原点，2：负方向回原点
                if (SearchHomeDirection < 1 || SearchHomeDirection > 2) 
                    {
                    MessageBox.Show("The parameter 'SearchHomeDirection' should be 1~2, please revise it.");
                    return;
                    }

                //EZCount 取值范围：1－16
                if (EZCount < 1 || EZCount > 16) 
                    {
                    MessageBox.Show("The parameter 'EZCount' should be 1~16, please revise it.");
                    return;
                    }

                //原点锁存使能状态：0：禁止，2：允许
                if (EnableLatch !=0 && EnableLatch != 2) 
                    {
                    MessageBox.Show("The parameter 'EnableLatch' should be 0 or 2, please revise it.");
                    return;
                    }

                //原点锁存方式：0：下降沿锁存，1：上升沿锁存
                if (LatchLogic != 0) 
                    {
                    LatchLogic = 1;
                    }

                //原点信号的有效电平：0：低电平有效，1：高电平有效
                if (OrgLogic != 0) 
                    {
                    OrgLogic = 1;
                    }

                //回原点速度：0：低速回原点，1：高速回原点
                if (VelocityMode != 0) 
                    {
                    VelocityMode = 1;
                    }

                if (DoingSearchHome == true)
                    {
                    MessageBox.Show("The axis " + (TargetAxis + 1)
                        + " is searching home signal now, please wait until it is done...");
                    return;
                    }
                else 
                    {
                    DoingSearchHome = true;
                    }

                //注意：回零运动中，当选择回零模式为0~4【对应1~5】时，用该函数设置原点信号有效电平
                SearchingHomeMode = SearchHomeMode;
                SearchingHomeDirection = SearchHomeDirection;
                SearchingHomeVelocityMode = VelocityMode;
                SearchingHomeOrgLogic = OrgLogic;
                SearchingHomeEnableFilter = EnableFilter;
                SearchingHomeEZCount = EZCount;
                SearchingHomeMinVelocity = MinVelocity;
                SearchingHomeMaxVelocity = MaxVelocity;
                
                SearchingHomeAccTime = TotalAccTime;
                SearchingHomeDecTime = TotalDecTime;
                
                SearchingHomeEnableLatch = EnableLatch;
                SearchingHomeLatchLogic = LatchLogic;

                SearchHomeThread = null;
                SearchHomeThread = new Thread(SearchHomeFunction);
                SearchHomeThread.IsBackground = true;
                SearchHomeThread.Start();

                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return;
                }
            }

        //执行回原点的函数
        /// <summary>
        /// 执行回原点的函数
        /// </summary>
        private void SearchHomeFunction() 
            {

            // 回原点步骤
            //在进行精确的运动控制之前，需要设定运动坐标系的原点。运动平台上都设有原点传感器（也称为原点开关），
            //寻找原点开关的位置并将该位置设为平台的坐标原点，即为回原点运动。

            //DMC2410C控制卡共提供了6种回原点方式:
            //其中方式1~5是采用原点电平状态作触发信号，
            //方式6则是采用原点边沿信号作触发信号；

            //具体回原点运动主要步骤如下： 

            //1、采用回零方式1~5作回原点运动： 
            //  1）使用d2410_set_HOME_pin_logic函数设置原点开关的有效电平； 
            //  2）使用d2410_config_home_mode函数设置回原点方式； 
            //  3）设置回原点运动的曲线速度形式； 
            //  4）使用d2410_home_move函数进行回原点运动； 
            //  5）回到原点后，指令脉冲计数器清零。

            //设定指定轴的回原点模式
            //<param name="axis">指定轴号</param>
            //<param name="mode">回原点的信号模式： 
            //0：只计home 
            //1：计home和EZ，计1个EZ信号 
            //2：一次回零加回找 
            //3：二次回零 
            //4：EZ单独回零 
            //5：原点捕获回零</param>
            //<param name="EZ_count">EZ信号出现EZ_count指定的次数后，轴运动停止。
            //仅当mode=4时该参数设置有效，取值范围：1－16</param>
            //Declare Function d2410_config_home_mode Lib "DMC2410.dll" (ByVal axis As Int16, ByVal mode As Int16, ByVal EZ_count As Int16) As Int32

            //单轴回原点运动
            //<param name="axis">指定轴号</param>
            //<param name="home_mode">回原点方式：1：正方向回原点，2：负方向回原点</param>
            //<param name="vel_mode">回原点速度：0：低速回原点，1：高速回原点</param>
            //Declare Function d2410_home_move Lib "DMC2410.dll" (ByVal axis As Int16, ByVal home_mode As Int16, ByVal vel_mode As Int16) As Int32

            //原点锁存
            //设置/读取原点锁存方式
            //<param name="axis">指定轴号</param>
            //<param name="enable">原点锁存使能状态：0：禁止，2：允许</param>
            //<param name="logic">原点锁存方式：0：下降沿锁存，1：上升沿锁存</param>
            //<remarks>注意：回零运动中，当选择回零模式为5时，用d2410_set_homelatch_mode函数设置原点信号锁存方式</remarks>
            //Declare Function d2410_set_homelatch_mode Lib "DMC2410.dll" (ByVal axis As Int16, ByVal enable As Int16, ByVal logic As Int16) As Int32

            //复位原点锁存标志
            //<param name="axis">指定轴号</param>
            //Declare Function d2410_reset_homelatch_flag Lib "DMC2410.dll" (ByVal axis As Int16) As Int32

            //设置ORG信号的有效电平，以及允许/禁止滤波功能
            //<param name="axis">指定轴号</param>
            //<param name="org_logic">ORG信号的有效电平：0：低电平有效，1：高电平有效</param>
            //<param name="filter">允许/禁止滤波功能：0：禁止，1：允许</param>
            //<remarks>注意：回零运动中，当选择回零模式为0~4时，用该函数设置原点信号有效电平</remarks>
            //Declare Function d2410_set_HOME_pin_logic Lib "DMC2410.dll" (ByVal axis As Int16, ByVal org_logic As Int16, _
            //                                                        ByVal filter As Int16) As Int32

            //回原点方式
            //DMC2410C运动控制卡提供了6种回原点运动的方式： 

            //名称                                功能
            //d2410_set_HOME_pin_logic         设置原点信号的电平和滤波器使能
            //d2410_config_home_mode           选择回原点模式
            //d2410_home_move                  按指定的方向和速度方式开始回原点
            //d2410_set_homelatch_mode         设置原点锁存方式
            //d2410_set_position               指令脉冲计数器清零

            //注意：执行完d2410_home_move函数后，指令脉冲计数器不会自动清零；
            //如需清零可以在回零运动完成后，调用d2410_set_position函数软件清零。

            int TempReturn = 0;

            try
                {

                switch (SearchingHomeMode) 
                    {
                    case 1:
                        //方式1：一次回零
                        //该方式以低速回原点；适合于行程短、安全性要求高的场合。
                        //动作过程为：电机从初始位置以恒定低速度向原点方向运动，当到达原点开关位置，原点信号被触发，
                        //电机立即停止（过程0）；将停止位置设为原点位置。

                        //例程7.1：方式1低速回原点
                        //d2410_set_HOME_pin_logic 0,0,1           '设置0号轴原点信号低电平有效，使能滤波功能 
                        //d2410_config_home_mode 0,0,0             '设置0号轴回零模式为方式1 
                        //d2410_set_profile 0,500,1000,0.1,0.1     '设置0号轴梯形曲线速度，加、减速时间 
                        //d2410_home_move 0,2,0                    '设置0号轴为负方向回原点，速度方式为低速回原点 
                        //While (d2410_check_done(0) = 0)          '检测运动状态，等待回原点动作完成 
                        //DoEvents 
                        //Wend 
                        //D2410_set_position 0,0                   '设置0号轴的指令脉冲计数器绝对位置为0

                        TempReturn += (int)d2410_set_HOME_pin_logic(TargetAxis, 
                            SearchingHomeOrgLogic, SearchingHomeEnableFilter);
                        TempReturn += (int)d2410_config_home_mode(TargetAxis, 
                            SearchingHomeMode, SearchingHomeEZCount);
                        TempReturn += (int)d2410_set_profile(TargetAxis, 
                            SearchingHomeMinVelocity, SearchingHomeMaxVelocity, 
                            SearchingHomeAccTime, SearchingHomeDecTime);
                        TempReturn += (int)d2410_home_move(TargetAxis, 
                            SearchingHomeMode, SearchingHomeVelocityMode);
                        while (d2410_check_done(TargetAxis) == 0)
                            {
                            Application.DoEvents();
                            }
                        TempReturn += (int)d2410_set_position(TargetAxis, 0);

                        break;

                    case 2:
                        //方式2：一次回零加回找 
                        //该方式先进行方式1运动，完成后再反向回找原点开关的边缘位置，当原点信号第一次无效的时候，电机立即停止；
                        //将停止位置设为原点位置。
                        TempReturn = (int)d2410_set_HOME_pin_logic(TargetAxis, 
                            SearchingHomeOrgLogic, SearchingHomeEnableFilter);
                        TempReturn = (int)d2410_config_home_mode(TargetAxis,
                            SearchingHomeMode, SearchingHomeEZCount);
                        TempReturn = (int)d2410_set_profile(TargetAxis, 
                            SearchingHomeMinVelocity, SearchingHomeMaxVelocity, 
                            SearchingHomeAccTime, SearchingHomeDecTime);
                        TempReturn = (int)d2410_home_move(TargetAxis, 
                            SearchingHomeMode, SearchingHomeVelocityMode);
                        while (d2410_check_done(TargetAxis) == 0)
                            {
                            Application.DoEvents();
                            }
                        TempReturn = (int)d2410_set_position(TargetAxis, 0);
                        break;

                    case 3:
                        //方式3：两次回零 
                        //'该方式为方式1和方式2的组合。先进行方式2的回零加反找，
                        //完成后再进行方式1的一次回零。

                        TempReturn = (int)d2410_set_HOME_pin_logic(TargetAxis,
                            SearchingHomeOrgLogic, SearchingHomeEnableFilter);
                        TempReturn = (int)d2410_config_home_mode(TargetAxis,
                            SearchingHomeMode, SearchingHomeEZCount);
                        TempReturn = (int)d2410_set_profile(TargetAxis,
                            SearchingHomeMinVelocity, SearchingHomeMaxVelocity,
                            SearchingHomeAccTime, SearchingHomeDecTime);
                        TempReturn = (int)d2410_home_move(TargetAxis,
                            SearchingHomeMode, SearchingHomeVelocityMode);
                        while (d2410_check_done(TargetAxis) == 0)
                            { 
                            Application.DoEvents();
                            }
                        TempReturn = (int)d2410_set_position(TargetAxis, 0);

                        break;

                    case 4:
                        //方式4：一次回零后再记1个EZ脉冲进行回零 
                        //该方式在回原点运动过程中，当找到原点信号后，还要等待该轴的EZ信号出现，此时电机停止。
                        //回零之前需要清除EZ状态，当EZ信号到来时，电机立即停止。

                        TempReturn = (int)d2410_set_HOME_pin_logic(TargetAxis,
                            SearchingHomeOrgLogic, SearchingHomeEnableFilter);
                        TempReturn = (int)d2410_config_home_mode(TargetAxis,
                            SearchingHomeMode, SearchingHomeEZCount);
                        TempReturn = (int)d2410_set_profile(TargetAxis,
                            SearchingHomeMinVelocity, SearchingHomeMaxVelocity,
                            SearchingHomeAccTime, SearchingHomeDecTime);
                        TempReturn = (int)d2410_home_move(TargetAxis,
                            SearchingHomeMode, SearchingHomeVelocityMode);
                        while (d2410_check_done(TargetAxis) == 0)
                            {
                            Application.DoEvents();
                            }
                        TempReturn = (int)d2410_set_position(TargetAxis, 0);

                        break;

                    case 5:
                        //方式5：EZ单独回零 
                        //该方式在回原点运动过程中，当EZ 信号计数到达指定个数，此时电机停止。
                        TempReturn = (int)d2410_set_HOME_pin_logic(TargetAxis,
                            SearchingHomeOrgLogic, SearchingHomeEnableFilter);
                        TempReturn = (int)d2410_config_home_mode(TargetAxis,
                            SearchingHomeMode, SearchingHomeEZCount);
                        TempReturn = (int)d2410_set_profile(TargetAxis,
                            SearchingHomeMinVelocity, SearchingHomeMaxVelocity,
                            SearchingHomeAccTime, SearchingHomeDecTime);
                        TempReturn = (int)d2410_home_move(TargetAxis,
                            SearchingHomeMode, SearchingHomeVelocityMode);
                        while (d2410_check_done(TargetAxis) == 0)
                            {
                            Application.DoEvents();
                            }
                        TempReturn = (int)d2410_set_position(TargetAxis, 0);

                        break;

                    case 6:
                        //2、采用回零方式6作回原点运动： 
                        //  1）使用d2410_set_homelatch_mode函数设置原点信号锁存方式； 
                        //  2）使用d2410_config_home_mode函数设置回原点方式； 
                        //  3）设置回原点运动的曲线速度形式； 
                        //  4）使用d2410_home_move函数进行回原点运动； 
                        //  5）回到原点后，指令脉冲计数器清零。

                        //方式6：原点捕获回零
                        //该方式在回原点运动过程中，当原点捕获信号有效时，运动减速到停止，然后反向回到锁存位置。
                        //当采用原点捕获回零模式的时候，原点信号的初始状态对回零运动没有影响，该回零模式采用的是边沿触发。
                        //每次启动该回零运动的时候，原点锁存标志会自动清除。 
                        //注意：在回零方式6中，原点锁存位置为指令脉冲位置。

                        //例程7.2：方式6低速回原点
                        //d2410_set_homelatch_mode 0,2,0           '设置0号轴原点锁存方式为下降沿锁存 
                        //d2410_config_home_mode 0,5,0             '设置0号轴回零模式为方式6 
                        //d2410_set_profile 0,500,1000,0.1,0.1     '设置0号轴梯形曲线速度，加、减速时间 
                        //d2410_home_move 0,2,0                    '设置0号轴为负方向回原点，速度方式为低速回原点 
                        //While (d2410_check_done(0) = 0)          '检测运动状态，等待回原点动作完成 
                        //DoEvents 
                        //Wend 
                        //D2410_set_position 0,0                   '设置0号轴的指令脉冲计数器绝对位置为0

                        TempReturn = (int)d2410_set_homelatch_mode(TargetAxis,
                            SearchingHomeEnableLatch, SearchingHomeLatchLogic);
                        TempReturn = (int)d2410_config_home_mode(TargetAxis,
                            SearchingHomeMode, SearchingHomeEZCount);
                        TempReturn = (int)d2410_set_profile(TargetAxis,
                            SearchingHomeMinVelocity, SearchingHomeMaxVelocity,
                            SearchingHomeAccTime, SearchingHomeDecTime);
                        TempReturn = (int)d2410_home_move(TargetAxis,
                            SearchingHomeMode, SearchingHomeVelocityMode);

                        while (d2410_check_done(TargetAxis) == 0)
                            {
                            Application.DoEvents();
                            }
                        TempReturn = (int)d2410_set_position(TargetAxis, 0);

                        break;                    
                    }

                DoingSearchHome = false;
                return;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                DoingSearchHome = false;
                return;
                }            
            }
        
        #endregion
        
        #region "专用信号设置函数"

        //获取Alarm的有效电平及其工作方式
        /// <summary>
        /// 获取Alarm的有效电平及其工作方式
        /// </summary>
        /// <param name="AlarmLogic">Alarm信号的输入有效电平：
        /// 0：低电平有效，1：高电平有效</param>
        /// <param name="AlarmStopMode">Alarm信号的制动方式：
        /// 0：立即停止，1：减速停止(保留)</param>
        /// <returns>错误代码</returns>
        public int GetAlarmMode(ref ushort AlarmLogic, ref ushort AlarmStopMode)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_config_ALM_PIN(TargetAxis, 
                    ref AlarmLogic, ref AlarmStopMode);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置ALM的有效电平及其工作方式
        /// <summary>
        /// 设置ALM的有效电平及其工作方式
        /// </summary>
        /// <param name="AlarmLogic">Alarm信号的输入有效电平：
        /// 0：低电平有效，1：高电平有效</param>
        /// <param name="AlarmStopMode">Alarm信号的制动方式：
        /// 0：立即停止，1：减速停止(保留)</param>
        /// <returns>错误代码</returns>
        public int SetAlarmMode(ushort AlarmLogic, ushort AlarmStopMode)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (AlarmLogic != 0)
                    {
                    AlarmLogic = 1;
                    }

                if (AlarmStopMode != 0)
                    {
                    AlarmStopMode = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_config_ALM_PIN(TargetAxis, 
                    AlarmLogic, AlarmStopMode);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //SetAlarmMode扩展函数，增加ALM使能状态、控制方式的设定
        /// <summary>
        /// SetAlarmMode扩展函数，增加ALM使能状态、控制方式的设定
        /// </summary>
        /// <param name="AlarmEnabled">ALM使能状态：0：禁止，1：允许</param>
        /// <param name="AlarmLogic">ALM信号的输入有效电平：0：低电平有效，1：高电平有效</param>
        /// <param name="AlarmAll">ALM信号控制方式：0：停止单轴，1：停止所有轴</param>
        /// <param name="AlarmStopMode">ALM信号的制动方式：0：立即停止，1：减速停止(保留)</param>
        /// <returns>错误代码</returns>
        public int SetAlarmModeExtern(ushort AlarmEnabled, ushort AlarmLogic,
            ushort AlarmAll, ushort AlarmStopMode)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (AlarmEnabled != 0) 
                    {
                    AlarmEnabled = 1;
                    }

                if (AlarmLogic != 0) 
                    {
                    AlarmLogic = 1;
                    }

                if (AlarmStopMode != 0)
                    {
                    AlarmStopMode = 1;
                    }

                if (AlarmAll != 0) 
                    {
                    AlarmAll = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_config_ALM_PIN_Extern(TargetAxis, 
                    AlarmEnabled, AlarmLogic, AlarmAll, AlarmStopMode);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //GetAlarmMode扩展函数，增加Alarm使能状态、控制方式的设定
        /// <summary>
        /// GetAlarmMode扩展函数，增加Alarm使能状态、控制方式的设定
        /// </summary>
        /// <param name="AlarmEnabled">Alarm使能状态：0：禁止，1：允许</param>
        /// <param name="AlarmLogic">Alarm信号的输入有效电平：0：低电平有效，1：高电平有效</param>
        /// <param name="AlarmAll">Alarm信号控制方式：0：停止单轴，1：停止所有轴</param>
        /// <param name="AlarmStopMode">Alarm信号的制动方式：0：立即停止，1：减速停止(保留)</param>
        /// <returns>错误代码</returns>
        public int GetAlarmModeExtern(ref ushort AlarmEnabled, ref ushort AlarmLogic,
            ref ushort AlarmAll, ref ushort AlarmStopMode)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_config_ALM_PIN_Extern(TargetAxis,
                    ref AlarmEnabled, ref AlarmLogic, ref AlarmAll, ref AlarmStopMode);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置EL信号的使能状态
        /// <summary>
        /// 设置EL信号的使能状态
        /// </summary>
        /// <param name="Enable">EL信号的使能状态：0：不使能，1：使能</param>
        /// <returns>错误代码</returns>
        public int EnableELPin(ushort Enable)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (Enable != 0) 
                    {
                    Enable = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_Enable_EL_PIN(TargetAxis, Enable);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置EL信号的有效电平及制动方式
        /// <summary>
        /// 设置EL信号的有效电平及制动方式
        /// </summary>
        /// <param name="ELMode">EL有效电平和制动方式：
        /// 0：立即停、低有效
        /// 1：减速停、低有效
        /// 2：立即停、高有效
        /// 3：减速停、高有效
        /// </param>
        /// <returns>错误代码</returns>
        public int SetELMode(ushort ELMode)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (ELMode < 0 || ELMode > 3) 
                    {
                    MessageBox.Show("The parameter 'ELMode' should be 0~3, please revise it.");
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_config_EL_MODE(TargetAxis, ELMode);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置原点ORG信号的有效电平，以及允许/禁止滤波功能
        /// <summary>
        /// 设置原点ORG信号的有效电平，以及允许/禁止滤波功能
        /// 注意：回零运动中，当选择回零模式为0~4时，用该函数设置原点信号有效电平
        /// </summary>
        /// <param name="OrgLogic">ORG信号的有效电平：0：低电平有效，1：高电平有效</param>
        /// <param name="EnableFilter">允许/禁止滤波功能：0：禁止，1：允许</param>
        /// <returns>错误代码</returns>
        public int SetHomeSignalLogic(ushort OrgLogic, ushort EnableFilter)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (OrgLogic != 0) 
                    {
                    OrgLogic = 1;
                    }

                if (EnableFilter != 0) 
                    {
                    EnableFilter = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_HOME_pin_logic(TargetAxis,
                    OrgLogic, EnableFilter);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取轴的伺服使能端子的电平状态
        /// <summary>
        /// 读取轴的伺服使能端子的电平状态
        /// </summary>
        /// <returns>0：低电平，1：高电平</returns>
        public int GetServoOnSignalStatus()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_read_SEVON_PIN(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //启用轴的伺服使能
        /// <summary>
        /// 启用轴的伺服使能
        /// </summary>
        /// <returns></returns>
        public int ServoOn()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_write_SEVON_PIN(TargetAxis,1);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //输出对轴的伺服使能端子的控制
        /// <summary>
        /// 输出对轴的伺服使能端子的控制
        /// </summary>
        /// <param name="SetOn">设定管脚电平状态：0：低，1：高</param>
        /// <returns>错误代码</returns>
        public int ServoOn(ushort SetOn)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (SetOn != 0) 
                    {
                    SetOn = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_write_SEVON_PIN(TargetAxis, SetOn);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
        
        //关闭轴的伺服使能
        /// <summary>
        /// 关闭轴的伺服使能
        /// </summary>
        /// <returns></returns>
        public int ServoOff()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_write_SEVON_PIN(TargetAxis, 0);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取轴的“伺服准备好”端子的电平状态
        /// <summary>
        /// 读取轴的“伺服准备好”端子的电平状态
        /// </summary>
        /// <returns>0：低电平，1：高电平</returns>
        public int GetServoReadyStatus()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_read_RDY_PIN(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //检测指令到位【Pulse位置】
        /// <summary>
        /// 检测指令到位【Pulse位置】检测指令到位【Pulse位置】
        /// </summary>
        /// <returns>0：指令位置在设定的目标位置的误差带之外；
        /// 1：指令位置在设定的目标位置的误差带之内
        /// </returns>
        public int VerifyMotionIsDoneByPulse()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_check_success_pulse(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //检测指令到位【编码器位置】
        /// <summary>
        /// 检测指令到位【编码器位置】
        /// </summary>
        /// <returns>0：编码器位置在设定的目标位置的误差带之外；
        /// 1：编码器位置在设定的目标位置的误差带之内
        /// </returns>
        public int VerifyMotionIsDonebByEncoder()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_check_success_encoder(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //Emergency信号设置，急停信号有效后会立即停止所有轴
        /// <summary>
        /// Emergency信号设置，急停信号有效后会立即停止所有轴
        /// </summary>
        /// <param name="TargetCardNo">卡号, 范围（1~N，N为卡号）</param>
        /// <param name="EnableEmergency">0：无效，1：有效</param>
        /// <param name="EmergencySignalLogic">0：低有效，1：高有效</param>
        /// <returns>错误代码</returns>
        public int SetEmergencySignal(ushort TargetCardNo, ushort EnableEmergency,
            ushort EmergencySignalLogic)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (EnableEmergency != 0) 
                    {
                    EnableEmergency = 1;
                    }

                if (EmergencySignalLogic != 0) 
                    {
                    EmergencySignalLogic = 1;
                    }

                if (TargetCardNo > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_config_EMG_PIN(TargetCardNo, 
                    EnableEmergency, EmergencySignalLogic);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
        
        //Emergency信号设置，急停信号有效后会立即停止所有轴
        /// <summary>
        /// Emergency信号设置，急停信号有效后会立即停止所有轴
        /// </summary>
        /// <param name="EnableEmergency">0：无效，1：有效</param>
        /// <param name="EmergencySignalLogic">0：低有效，1：高有效</param>
        /// <returns>错误代码</returns>
        public int SetEmergencySignal(ushort EnableEmergency,
            ushort EmergencySignalLogic)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (EnableEmergency != 0)
                    {
                    EnableEmergency = 1;
                    }

                if (EmergencySignalLogic != 0)
                    {
                    EmergencySignalLogic = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_config_EMG_PIN(TargetCard,
                    EnableEmergency, EmergencySignalLogic);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
        
        #endregion
        
        #region "位置比较输出功能"

        //位置比较功能的实现
        //DMC2410C运动控制卡提供了位置比较功能，位置比较的一般步骤是： 
        //   1、配置比较器； 
        //   2、清除位置比较数据；
        //   3、添加比较位置点；
        //   4、开始运动并查看比较状态。

        //低速位置比较功能
        //DMC2410C卡提供了两组低速位置比较，每组最多都可以添加8个比较点。低速位置比较的触发延时时间小于1ms。

        //低速位置比较相关函数说明
        //d2410_compare_config_Extern               '设置比较器配置
        //d2410_compare_clear_points_Extern         '清除所有比较点
        //d2410_compare_add_point_Extern            '添加比较点
        //d2410_compare_get_current_point_Extern    '读取当前比较点位置
        //d2410_compare_get_points_runned_Extern    '查询已经比较过的点个数
        //d2410_compare_get_points_remained_Extern  '查询可以加入的比较点数量

        //注意：1）低速位置比较共有两组比较队列，每组队列的位置比较都是独立进行的； 
        //      2）执行位置比较时，每个比较点的触发是按照添加的比较点顺序执行的，
        //         即如果有一个比较点没有被触发比较动作，那么后面的比较点是不会起作用的。

        //例7.11：低速位置比较 
        //Dim MyCardNo,Myqueue, Myenable, Myaxis, Mycmp_source As Integer 
        //Dim Mydir,Myaction As Integer 
        //Dim Mypos,Myactpara As Long 
        //MyCardNo = 0             '卡号 
        //Myqueue = 0              '设置比较序列号为0 
        //Myaxis = 0               '轴号为0 
        //Myenable = 1             '设置比较器使能 
        //Mycmp_source = 0         '设置比较源为指令位置 
        //d2410_compare_config_Extern MyCardNo,Myqueue, Myenable, Myaxis,Mycmp_source '设置比较器 
        //d2410_compare_clear_points_Extern MyCardNo,Myqueue                          '清除比较点 
        //Mypos = 8000                 '设置比较位置为8000pulse 
        //Mydir = 1                    '设置比较方向为大于等于 
        //Myaction = 3                 '触发功能为IO电平取反 
        //Myactpara = 1                '设置输出IO端口1触发功能 
        //d2410_set_position MyAxis, 0 '当前位置设为零点 
        //d2410_compare_add_point_Extern MyCardNo,Myqueue,Mypos,Mydir,Myaction,Myactpara '添加比较点 
        //d2410_ex_t_pmove Myaxis,10000,0     '执行定长运动，位移为10000pulse，相对坐标模式


        /// <summary>
        /// 设置位置误差带【Deviation误差】
        /// </summary>
        /// <param name="EncoderFactor">编码器系数(脉冲当量)</param>
        /// <param name="Deviation">位置误差带
        /// 说明：假设当前编码器脉冲数为200，指令脉冲数为1002， 当误差带设为2，编码器系数设为5，
        /// 处理如下： 200*5=1000,1000-1002=-2,在误差带范围[-2,2]之内，此时认为编码器到位。
        /// </param>
        /// <returns>错误代码</returns>
        public int SetPositionDeviation(double EncoderFactor, int Deviation)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_factor_error(TargetAxis, 
                    EncoderFactor, Deviation);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取位置误差带【Deviation误差】
        /// <summary>
        /// 读取位置误差带【Deviation误差】
        /// </summary>
        /// <param name="EncoderFactor">编码器系数(脉冲当量)</param>
        /// <param name="Deviation">位置误差带</param>
        /// <returns>错误代码</returns>
        public int GetPositionDeviation(ref double EncoderFactor, ref int Deviation)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_factor_error(TargetAxis, ref 
                    EncoderFactor, ref Deviation);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置指定控制卡的轴比较器配置
        /// <summary>
        /// 设置指定控制卡的轴比较器配置
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <param name="QueueNumber">比较队列号：0、1</param>
        /// <param name="EnableCompare">1：使能比较功能，0：禁止比较功能</param>
        /// <param name="TargetAxisNumber">TargetAxisNumber轴号</param>
        /// <param name="CompareSource">比较源：0：比较指令位置，1：比较编码器位置</param>
        /// <returns>错误代码</returns>
        public int SetCompareExtern(ushort TargetCardNumber, ushort QueueNumber,
            ushort EnableCompare, ushort TargetAxisNumber, ushort CompareSource)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                //【多卡运行】
                //DMC2410C运动控制卡的驱动程序支持最多8块DMC2410C卡同时工作。因此，一台PC机可以同时控制多达32个电机同时运动。 
                //DMC2410C卡支持即插即用模式，用户可不必去关心如何设置卡的基地址和IRQ中断值。在使用多块运动控制卡时，首先要
                //用运动控制卡上的拨码开关设置卡号；系统启动后，系统BIOS为相应的卡自动分配物理空间。 

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                //【卡号和轴号的对应关系为】： 
                // 0号卡对应0~3号轴；1号卡对应4~7号轴；n号卡对应4n~ 4*（n+1）-1号轴。
                //【使用习惯从1开始计算轴数量，在调用函数时需要减去1才是正确的轴号。】
                if (TargetAxisNumber > AvailableCardQty * 4
                    || TargetAxisNumber < (AvailableCardQty * 4 - 3))
                    {
                    MessageBox.Show("The value for target axis 'TargetAxisNumber' should be : "
                        + (AvailableCardQty * 4 - 3) + "~" + (AvailableCardQty * 4)
                        + " ,please revise the parameter and retry.\r\n"
                        + "目标轴号参数 'TargetAxisNumber' 取值范围："
                        + (AvailableCardQty * 4 - 3) + "~" + (AvailableCardQty * 4)
                        + "请修改参数。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                if (QueueNumber != 0) 
                    {
                    QueueNumber = 1;
                    }

                if (EnableCompare != 0) 
                    {
                    EnableCompare = 1;
                    }

                if (CompareSource != 0) 
                    {
                    CompareSource = 1;
                    }
                
                int TempReturn = 0;
                TempReturn = (int)d2410_compare_config_Extern((ushort)(TargetCardNumber - 1), 
                    QueueNumber, EnableCompare, (ushort)(TargetAxisNumber - 1), CompareSource);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置当前控制卡的当前轴比较器配置
        /// <summary>
        /// 设置当前控制卡的当前轴比较器配置
        /// </summary>
        /// <param name="QueueNumber">比较队列号：0、1</param>
        /// <param name="EnableCompare">1：使能比较功能，0：禁止比较功能</param>
        /// <param name="CompareSource">比较源：0：比较指令位置，1：比较编码器位置</param>
        /// <returns>错误代码</returns>
        public int SetCompareExtern(ushort QueueNumber, ushort EnableCompare,
            ushort CompareSource)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (QueueNumber != 0)
                    {
                    QueueNumber = 1;
                    }

                if (EnableCompare != 0)
                    {
                    EnableCompare = 1;
                    }

                if (CompareSource != 0)
                    {
                    CompareSource = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_compare_config_Extern(TargetCard, 
                    QueueNumber, EnableCompare, TargetAxis, CompareSource);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取指定控制卡的轴比较器配置
        /// <summary>
        /// 读取指定控制卡的轴比较器配置
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <param name="QueueNumber">比较队列号：0、1</param>
        /// <param name="EnableCompare">1：使能比较功能，0：禁止比较功能</param>
        /// <param name="TargetAxisNumber">目标轴号</param>
        /// <param name="CompareSource">比较源：0：比较指令位置，1：比较编码器位置</param>
        /// <returns>错误代码</returns>
        public int GetCompareSettingExtern(ushort TargetCardNumber, ushort QueueNumber,
            ref ushort EnableCompare, ref ushort TargetAxisNumber, ref ushort CompareSource)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                //【多卡运行】
                //DMC2410C运动控制卡的驱动程序支持最多8块DMC2410C卡同时工作。因此，一台PC机可以同时控制多达32个电机同时运动。 
                //DMC2410C卡支持即插即用模式，用户可不必去关心如何设置卡的基地址和IRQ中断值。在使用多块运动控制卡时，首先要
                //用运动控制卡上的拨码开关设置卡号；系统启动后，系统BIOS为相应的卡自动分配物理空间。 

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                //【卡号和轴号的对应关系为】： 
                // 0号卡对应0~3号轴；1号卡对应4~7号轴；n号卡对应4n~ 4*（n+1）-1号轴。
                //【使用习惯从1开始计算轴数量，在调用函数时需要减去1才是正确的轴号。】
                if (TargetAxisNumber > AvailableCardQty * 4
                    || TargetAxisNumber < (AvailableCardQty * 4 - 3))
                    {
                    MessageBox.Show("The value for target axis 'TargetAxisNumber' should be : "
                        + (AvailableCardQty * 4 - 3) + "~" + (AvailableCardQty * 4)
                        + " ,please revise the parameter and retry.\r\n"
                        + "目标轴号参数 'TargetAxisNumber' 取值范围："
                        + (AvailableCardQty * 4 - 3) + "~" + (AvailableCardQty * 4)
                        + "请修改参数。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }
                
                TargetAxisNumber = (ushort)(TargetAxisNumber - 1);

                if (QueueNumber != 0) 
                    {
                    QueueNumber = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_compare_get_config_Extern((ushort)(TargetCardNumber - 1),
                    QueueNumber, ref EnableCompare, ref TargetAxisNumber, 
                    ref CompareSource);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取当前控制卡的当前轴比较器配置
        /// <summary>
        /// 读取当前控制卡的当前轴比较器配置
        /// </summary>
        /// <param name="QueueNumber">比较队列号：0、1</param>
        /// <param name="EnableCompare">1：使能比较功能，0：禁止比较功能</param>
        /// <param name="CompareSource">比较源：0：比较指令位置，1：比较编码器位置</param>
        /// <returns>错误代码</returns>
        public int GetCompareSettingExtern(ushort QueueNumber,
            ref ushort EnableCompare, ref ushort CompareSource)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                if (QueueNumber != 0)
                    {
                    QueueNumber = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_compare_get_config_Extern(TargetCard, 
                    QueueNumber, ref EnableCompare, ref TargetAxis, 
                    ref CompareSource);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //清除指定控制卡的所有比较点
        /// <summary>
        /// 清除指定控制卡的所有比较点
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <param name="QueueNumber">比较队列号：0、1</param>
        /// <returns>错误代码</returns>
        public int ClearAllComparePos(ushort TargetCardNumber, ushort QueueNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                //【多卡运行】
                //DMC2410C运动控制卡的驱动程序支持最多8块DMC2410C卡同时工作。因此，一台PC机可以同时控制多达32个电机同时运动。 
                //DMC2410C卡支持即插即用模式，用户可不必去关心如何设置卡的基地址和IRQ中断值。在使用多块运动控制卡时，首先要
                //用运动控制卡上的拨码开关设置卡号；系统启动后，系统BIOS为相应的卡自动分配物理空间。 

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                if (QueueNumber != 0) 
                    {
                    QueueNumber = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_compare_clear_points_Extern(TargetCardNumber, QueueNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //清除当前控制卡的所有比较点
        /// <summary>
        /// 清除当前控制卡的所有比较点
        /// </summary>
        /// <param name="QueueNumber">比较队列号：0、1</param>
        /// <returns>错误代码</returns>
        public int ClearAllComparePos(ushort QueueNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }
                
                if (QueueNumber != 0)
                    {
                    QueueNumber = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_compare_clear_points_Extern(TargetCard, QueueNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取指定控制卡的轴当前比较点位置
        /// <summary>
        /// 读取指定控制卡的轴当前比较点位置
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <param name="QueueNumber">比较队列号：0、1</param>
        /// <returns>当前比较点位置</returns>
        public int GetComparePosForCurrentPointExtern(ushort TargetCardNumber, 
            ushort QueueNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                //【多卡运行】
                //DMC2410C运动控制卡的驱动程序支持最多8块DMC2410C卡同时工作。因此，一台PC机可以同时控制多达32个电机同时运动。 
                //DMC2410C卡支持即插即用模式，用户可不必去关心如何设置卡的基地址和IRQ中断值。在使用多块运动控制卡时，首先要
                //用运动控制卡上的拨码开关设置卡号；系统启动后，系统BIOS为相应的卡自动分配物理空间。 

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                if (QueueNumber != 0)
                    {
                    QueueNumber = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_compare_get_current_point_Extern(TargetCardNumber, 
                    QueueNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //读取当前控制卡的轴当前比较点位置
        /// <summary>
        /// 读取当前控制卡的当前轴当前比较点位置
        /// </summary>
        /// <param name="QueueNumber">比较队列号：0、1</param>
        /// <returns>当前比较点位置</returns>
        public int GetComparePosForCurrentPointExtern(ushort QueueNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (QueueNumber != 0)
                    {
                    QueueNumber = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_compare_get_current_point_Extern(TargetCard,
                    QueueNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //查询指定控制卡已经比较过的点个数
        /// <summary>
        /// 查询指定控制卡已经比较过的点个数
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <param name="QueueNumber">比较队列号：0、1</param>
        /// <returns>已经比较过的比较点的数量</returns>
        public int GetQtyOfComparedPos(ushort TargetCardNumber,
            ushort QueueNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                //【多卡运行】
                //DMC2410C运动控制卡的驱动程序支持最多8块DMC2410C卡同时工作。因此，一台PC机可以同时控制多达32个电机同时运动。 
                //DMC2410C卡支持即插即用模式，用户可不必去关心如何设置卡的基地址和IRQ中断值。在使用多块运动控制卡时，首先要
                //用运动控制卡上的拨码开关设置卡号；系统启动后，系统BIOS为相应的卡自动分配物理空间。 

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                if (QueueNumber != 0)
                    {
                    QueueNumber = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_compare_get_points_runned_Extern(TargetCardNumber, 
                    QueueNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }
        
        //查询当前控制卡已经比较过的点个数
        /// <summary>
        /// 查询当前控制卡已经比较过的点个数
        /// </summary>
        /// <param name="QueueNumber">比较队列号：0、1</param>
        /// <returns>已经比较过的比较点的数量</returns>
        public int GetQtyOfComparedPos(ushort QueueNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }
                
                if (QueueNumber != 0)
                    {
                    QueueNumber = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_compare_get_points_runned_Extern(TargetCard,
                    QueueNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //查询指定控制卡可以加入的比较点数量
        /// <summary>
        /// 查询指定控制卡可以加入的比较点数量
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <param name="QueueNumber">比较队列号：0、1</param>
        /// <returns>剩余可用的比较点数量</returns>
        public int GetRemainQtyOfComparedPos(ushort TargetCardNumber,
            ushort QueueNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                //【多卡运行】
                //DMC2410C运动控制卡的驱动程序支持最多8块DMC2410C卡同时工作。因此，一台PC机可以同时控制多达32个电机同时运动。 
                //DMC2410C卡支持即插即用模式，用户可不必去关心如何设置卡的基地址和IRQ中断值。在使用多块运动控制卡时，首先要
                //用运动控制卡上的拨码开关设置卡号；系统启动后，系统BIOS为相应的卡自动分配物理空间。 

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                if (QueueNumber != 0)
                    {
                    QueueNumber = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_compare_get_points_remained_Extern(TargetCardNumber, 
                    QueueNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //查询当前控制卡可以加入的比较点数量
        /// <summary>
        /// 查询当前控制卡可以加入的比较点数量
        /// </summary>
        /// <param name="QueueNumber">比较队列号：0、1</param>
        /// <returns>剩余可用的比较点数量</returns>
        public int GetRemainQtyOfComparedPos(ushort QueueNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (QueueNumber != 0)
                    {
                    QueueNumber = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_compare_get_points_remained_Extern(TargetCard,
                    QueueNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //为指定控制卡添加比较点
        /// <summary>
        /// 为指定控制卡添加比较点
        /// action:1 , actpara: IO号, 功能: IO置为低电平
        /// action:2 , actpara: IO号, 功能: IO置为高电平
        /// action:3 , actpara: IO号, 功能: 取反IO
        /// action:5 , actpara: IO号, 功能: 输出100us 脉冲
        /// action:6 , actpara: IO号, 功能: 输出1ms 脉冲
        /// action:7 , actpara: IO号, 功能: 输出10ms 脉冲
        /// action:8 , actpara: IO号, 功能: 输出100ms 脉冲
        /// action:11 , actpara: 速度值, 功能: 当前轴变速
        /// action:13 , actpara: 轴号, 功能: 停止指定轴
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <param name="QueueNumber">比较队列号：0、1</param>
        /// <param name="Position">位置坐标</param>
        /// <param name="Direction">比较方向：0：小于等于，1：大于等于</param>
        /// <param name="Action">比较点触发功能</param>
        /// <param name="ComparePosTriggerPara">比较点触发功能参数</param>
        /// <returns>错误代码</returns>
        public int AddComparePointExtern(ushort TargetCardNumber,
            ushort QueueNumber, UInt32 Position, ushort Direction,
            ushort Action, UInt32 ComparePosTriggerPara)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                if (QueueNumber != 0)
                    {
                    QueueNumber = 1;
                    }

                if (Direction != 0)
                    {
                    Direction = 1;
                    }

                if (Action < 1 || Action == 4
                    || (Action > 8 && Action < 11)
                    || Action == 12
                    || Action > 13) 
                    {
                    MessageBox.Show("The parameter 'Action' should be 1~3,5~8,11,13, please revise it and retry.");
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_compare_add_point_Extern(TargetCardNumber, 
                    QueueNumber, Position, Direction, Action, ComparePosTriggerPara);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //为当前控制卡添加比较点
        /// <summary>
        /// 为当前控制卡添加比较点
        /// action:1 , actpara: IO号, 功能: IO置为低电平
        /// action:2 , actpara: IO号, 功能: IO置为高电平
        /// action:3 , actpara: IO号, 功能: 取反IO
        /// action:5 , actpara: IO号, 功能: 输出100us 脉冲
        /// action:6 , actpara: IO号, 功能: 输出1ms 脉冲
        /// action:7 , actpara: IO号, 功能: 输出10ms 脉冲
        /// action:8 , actpara: IO号, 功能: 输出100ms 脉冲
        /// action:11 , actpara: 速度值, 功能: 当前轴变速
        /// action:13 , actpara: 轴号, 功能: 停止指定轴
        /// </summary>
       /// <param name="QueueNumber">比较队列号：0、1</param>
        /// <param name="Position">位置坐标</param>
        /// <param name="Direction">比较方向：0：小于等于，1：大于等于</param>
        /// <param name="Action">比较点触发功能</param>
        /// <param name="ComparePosTriggerPara">比较点触发功能参数</param>
        /// <returns>错误代码</returns>
        public int AddComparePointExtern(ushort QueueNumber, 
            UInt32 Position, ushort Direction, ushort Action, 
            UInt32 ComparePosTriggerPara)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }
                
                if (QueueNumber != 0)
                    {
                    QueueNumber = 1;
                    }

                if (Direction != 0)
                    {
                    Direction = 1;
                    }

                if (Action < 1 || Action == 4
                    || (Action > 8 && Action < 11)
                    || Action == 12
                    || Action > 13)
                    {
                    MessageBox.Show("The parameter 'Action' should be 1~3,5~8,11,13, please revise it and retry.");
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_compare_add_point_Extern(TargetCard,
                    QueueNumber, Position, Direction, Action, ComparePosTriggerPara);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        #endregion
        
        #region "高速位置比较"

        //高速位置比较功能
        //DMC2410C控制卡为每个轴提供了一个高速位置比较。高速位置比较基本无触发延时时间
        //（至多延时两个指令脉冲的时间）。

        //高速位置比较相关函数说明
        //d2410_config_CMP_PIN           '配置高速位置比较器
        //d2410_read_CMP_PIN             '读取高速位置比较输出口状态

        //注意：每轴的位置比较都是独立进行的，高速位置比较暂时只支持反馈位置比较。

        //例7.12：高速位置比较 
        //Dim Myaxis,Mycmp_enable,MyCMP_logic As Integer 
        //Dim Mycmp_pos As Long 
        //Myaxis = 0                         '轴号 
        //Mycmp_enable = 1                   'CMP使能 
        //Mycmp_pos = 8000                   'CMP比较位置为8000pulse 
        //MyCMP_logic = 0                    'CMP输出低电平，脉冲信号 
        //d2410_config_CMP_PIN Myaxis, Mycmp_enable, Mycmp_pos, MyCMP_logic      '设置比较器，0号轴，比较位置为8000pulse，
        //                                                                        '触发时动作为CMP输出低电平，脉冲信号 
        //d2410_ex_t_pmove Myaxis,10000,1       '执行定长运动，位移为10000pulse，绝对坐标模式
                
        /// <summary>
        /// 配置高速位置比较器
        /// 注意：当设置CMP比较器后，相应CMP输出口的电平会变为与设置的电平相反；
        /// 当位置触发时，CMP端口会输出一个脉冲信号(1~2ms)。
        /// </summary>
        /// <param name="EnableCompare">使能状态：0：禁止，1：使能</param>
        /// <param name="ComparePos">比较位置值</param>
        /// <param name="CompareLogic">CMP输出有效电平：0：低电平，1：高电平</param>
        /// <returns>错误代码</returns>
        public int SetHighSpeedPosCompare(ushort EnableCompare,
            int ComparePos, ushort CompareLogic)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (EnableCompare != 0) 
                    {
                    EnableCompare = 1;
                    }

                if (CompareLogic != 0) 
                    {
                    CompareLogic = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_config_CMP_PIN(TargetAxis, 
                    EnableCompare, ComparePos, CompareLogic);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取高速位置比较器
        /// <summary>
        /// 读取高速位置比较器
        /// </summary>
        /// <param name="EnableCompare">使能状态：0：禁止，1：使能</param>
        /// <param name="ComparePos">比较位置值</param>
        /// <param name="CompareLogic">CMP输出有效电平：0：低电平，1：高电平</param>
        /// <returns>错误代码</returns>
        public int GetHighSpeedPosCompare(ref ushort EnableCompare,
            ref int ComparePos, ref ushort CompareLogic)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_config_CMP_PIN(TargetAxis, 
                    ref EnableCompare, ref ComparePos, ref CompareLogic);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取高速位置比较输出口状态
        /// <summary>
        /// 读取高速位置比较输出口状态
        /// </summary>
        /// <returns>1-高电平，0-低电平</returns>
        public int GetBitStatusOfHighSpeedPosCompare()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_read_CMP_PIN(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //设置高速位置比较输出口状态
        /// <summary>
        /// 设置高速位置比较输出口状态
        /// </summary>
        /// <param name="SetOn">1-高电平，0-低电平</param>
        /// <returns></returns>
        public int SetOutBitOfHighSpeedPosCompare(ushort SetOn)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (SetOn != 0) 
                    {
                    SetOn = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_write_CMP_PIN(TargetAxis, SetOn);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
        
        #endregion

        #region "通用输入/输出控制函数"

        //通用IO相关函数说明
        //d2410_read_inbit           '读取指定控制卡的某一位输入口的电平状态
        //d2410_write_outbit         '对指定控制卡的某一位输出口置位
        //d2410_read_outbit          '取指定控制卡的某一位输出口的电平状态
        //d2410_read_inport          '读取指定控制卡的全部通用输入口的电平状态
        //d2410_read_outport         '读取指定控制卡的全部通用输出口的电平状态
        //d2410_write_outport        '指定控制卡的全部通用输出口的电平状态

        //注意：在使用d2410_write_outport对运动控制卡的全部输出口进行置位，
        //使用d2410_read_inport、d2410_read_outport进行IO电平读取显示时，
        //应该使用十六进制数进行赋值（尽量避免使用十进制数，特别是在不支持无符号变量的开发环境下）。
        //在对IO电平进行控制与读取时，使用十六进制数赋值远比使用十进制数赋值更加直观、方便。

        //例程7.8：读取第0号卡的通用输入口1的电平值，并对通用输出口3置高电平
        //Dim MyCardNo,MyInbitno,MyInValue,MyOutbitno,MyOutValue As Integer 
        //MyCardNo = 0                                         '卡号 
        //MyInbitno = 1                                        '定义通用输入口1 
        //MyInValue = d2410_read_inbit (MyCardNo, MyInbitno)   '读取通用输入口1的电平值，并赋值给变量MyInValue 
        //MyOutbitno = 3                                       '定义通用输出口3 
        //MyOutValue = 1                                       '定义输出电平为高 
        //d2410_write_outbit MyCardNo, MyOutbitno, MyOutValue  '对通用输出口3置高电平

        //例7.9：读取全部输入IO口的电平值并进行显示，对全部输出IO口的电平进行初始化
        //Dim MyCardNo As Integer 
        //Dim MyInportValue,MyOutportValue As Long 
        //Dim MyInportValueTemp As String 
        //MyCardNo = 0                                        '卡号 
        //MyInportValue = d2410_read_inport (MyCardNo)        '读取所有输入IO口电平值，并赋值给变量MyInportValue 
        //MyInportValueTemp = Hex(MyInportValue)              '转换成十六进制 
        //MyInTextShow = MyInportValueTemp                    '显示在文本框MyInTextShow中 
        //MyOutportValue = &HFFFFFBFA                         '&H表示十六进制（VB），定义输出口电平值，输出口1、3、11为低电平，其余端口为高电平 
        //d2410_write_outport MyCardNo, MyOutportValue        '对全部输出口进行电平赋值
                
        /// <summary>
        /// 读取当前控制卡的某一位输入口的电平状态
        /// </summary>
        /// <param name="TargetBit">指定输入口位号(取值范围：1~32)</param>
        /// <returns>0：低电平，1：高电平</returns>
        public int GetInputBitStatus(ushort TargetBit)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                if (TargetBit < 1 || TargetBit > 32) 
                    {
                    MessageBox.Show("The parameter 'TargetBit' should be 1~32, please revise it.");
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_read_inbit(TargetCard, TargetBit);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //读取指定控制卡的某一位输入口的电平状态
        /// <summary>
        /// 读取指定控制卡的某一位输入口的电平状态
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围(1~N，N为卡号)</param>
        /// <param name="TargetBit">指定输入口位号(取值范围：1~32)</param>
        /// <returns>0：低电平，1：高电平</returns>
        public int GetInputBitStatus(ushort TargetCardNumber, ushort TargetBit)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetBit < 1 || TargetBit > 32)
                    {
                    MessageBox.Show("The parameter 'TargetBit' should be 1~32, please revise it.");
                    return 0;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                int TempReturn = 0;
                TempReturn = (int)d2410_read_inbit(TargetCardNumber, TargetBit);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //对指定控制卡的某一位输出口置位
        /// <summary>
        /// 对指定控制卡的某一位输出口置位
        /// OUT1～OUT16端口可设置上电时的初始电平，OUT17～OUT20上电初始电平为高。
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <param name="TargetBit">指定输出口位号（取值范围：1~20、25~32）</param>
        /// <param name="SetOn">输出电平：0：表示输出低电平，1：表示输出高电平</param>
        /// <returns>错误代码</returns>
        public int SetOutputBit(ushort TargetCardNumber, ushort TargetBit,
            ushort SetOn)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetBit < 1 || TargetBit > 32
                    || (TargetBit > 20 && TargetBit < 25))
                    {
                    MessageBox.Show("The parameter 'TargetBit' should be  1~20 and 25~32, please revise it.");
                    return 0;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                if (SetOn != 0) 
                    {
                    SetOn = 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                int TempReturn = 0;
                TempReturn = (int)d2410_write_outbit(TargetCardNumber, TargetBit, SetOn);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
        
        //对当前控制卡的某一位输出口置位
        /// <summary>
        /// 对当前控制卡的某一位输出口置位
        /// OUT1～OUT16端口可设置上电时的初始电平，OUT17～OUT20上电初始电平为高。
        /// </summary>
        /// <param name="TargetBit">指定输出口位号（取值范围：1~20、25~32）</param>
        /// <param name="SetOn">输出电平：0：表示输出低电平，1：表示输出高电平</param>
        /// <returns>错误代码</returns>
        public int SetOutputBit(ushort TargetBit, ushort SetOn)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetBit < 1 || TargetBit > 32
                    || (TargetBit > 20 && TargetBit < 25))
                    {
                    MessageBox.Show("The parameter 'TargetBit' should be  1~20 and 25~32, please revise it.");
                    return 0;
                    }

                if (SetOn != 0)
                    {
                    SetOn = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_write_outbit(TargetCard, TargetBit, SetOn);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //对当前控制卡的某一位输出口置位ON
        /// <summary>
        /// 对当前控制卡的某一位输出口置位ON
        /// OUT1～OUT16端口可设置上电时的初始电平，OUT17～OUT20上电初始电平为高。
        /// </summary>
        /// <param name="TargetBit">指定输出口位号（取值范围：1~20、25~32）</param>
        /// <returns>错误代码</returns>
        public int SetOutputBitOn(ushort TargetBit)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetBit < 1 || TargetBit > 32
                    || (TargetBit > 20 && TargetBit < 25))
                    {
                    MessageBox.Show("The parameter 'TargetBit' should be  1~20 and 25~32, please revise it.");
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_write_outbit(TargetCard, TargetBit, 1);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //对当前控制卡的某一位输出口置位OFF
        /// <summary>
        /// 对当前控制卡的某一位输出口置位OFF
        /// OUT1～OUT16端口可设置上电时的初始电平，OUT17～OUT20上电初始电平为高。
        /// </summary>
        /// <param name="TargetBit">指定输出口位号（取值范围：1~20、25~32）</param>
        /// <returns>错误代码</returns>
        public int SetOutputBitOff(ushort TargetBit)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetBit < 1 || TargetBit > 32
                    || (TargetBit > 20 && TargetBit < 25))
                    {
                    MessageBox.Show("The parameter 'TargetBit' should be  1~20 and 25~32, please revise it.");
                    return 0;
                    }
                
                int TempReturn = 0;
                TempReturn = (int)d2410_write_outbit(TargetCard, TargetBit, 0);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取指定控制卡的某一位输出口的电平状态
        /// <summary>
        /// 读取指定控制卡的某一位输出口的电平状态
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <param name="TargetBit">指定输出口位号（取值范围：1~20、25~32）</param>
        /// <returns>0：低电平，1：高电平</returns>
        public int GetOutputBitStatus(ushort TargetCardNumber, ushort TargetBit)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                if (TargetBit < 1 || TargetBit > 32
                    || (TargetBit > 20 && TargetBit < 25))
                    {
                    MessageBox.Show("The parameter 'TargetBit' should be  1~20 and 25~32, please revise it.");
                    return 0;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }
                
                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                int TempReturn = 0;
                TempReturn = (int)d2410_read_outbit(TargetCardNumber, TargetBit);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //读取当前控制卡的某一位输出口的电平状态
        /// <summary>
        /// 读取当前控制卡的某一位输出口的电平状态
        /// </summary>
        /// <param name="TargetBit">指定输出口位号（取值范围：1~20、25~32）</param>
        /// <returns>0：低电平，1：高电平</returns>
        public int GetOutputBitStatus(ushort TargetBit)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                if (TargetBit < 1 || TargetBit > 32
                    || (TargetBit > 20 && TargetBit < 25))
                    {
                    MessageBox.Show("The parameter 'TargetBit' should be  1~20 and 25~32, please revise it.");
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_read_outbit(TargetCard, TargetBit);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //读取指定控制卡的全部通用输入口的电平状态
        /// <summary>
        /// 读取指定控制卡的全部通用输入口的电平状态
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <returns>bit0~bit31位值分别代表第1~32号输入端口值</returns>
        public int GetAllInputSignal(ushort TargetCardNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 0;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                int TempReturn = 0;
                TempReturn = (int)d2410_read_inport(TargetCardNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //读取当前控制卡的全部通用输入口的电平状态
        /// <summary>
        /// 读取当前控制卡的全部通用输入口的电平状态
        /// </summary>
        /// <returns>返回int值的bit0~bit31位值分别代表第1~32号输入端口值</returns>
        public int GetAllInputSignal()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_read_inport(TargetCard);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //读取当前控制卡的全部通用输入口的电平状态
        /// <summary>
        /// 读取当前控制卡的全部通用输入口的电平状态
        /// </summary>
        /// <returns>返回InputSignal数据结构的bit0~bit31位值分别代表第1~32号输入端口值</returns>
        public InputSignal GetAllInputSignalNew()
            {
            InputSignal TempInputSignal;
            TempInputSignal.Bit0 = false;
            TempInputSignal.Bit1 = false;
            TempInputSignal.Bit2 = false;
            TempInputSignal.Bit3 = false;
            TempInputSignal.Bit4 = false;
            TempInputSignal.Bit5 = false;
            TempInputSignal.Bit6 = false;
            TempInputSignal.Bit7 = false;
            TempInputSignal.Bit8 = false;
            TempInputSignal.Bit9 = false;
            TempInputSignal.Bit10 = false;
            TempInputSignal.Bit11 = false;
            TempInputSignal.Bit12 = false;
            TempInputSignal.Bit13 = false;
            TempInputSignal.Bit14 = false;
            TempInputSignal.Bit15 = false;
            TempInputSignal.Bit16 = false;
            TempInputSignal.Bit17 = false;
            TempInputSignal.Bit18 = false;
            TempInputSignal.Bit19 = false;
            TempInputSignal.Bit20 = false;
            TempInputSignal.Bit21 = false;
            TempInputSignal.Bit22 = false;
            TempInputSignal.Bit23 = false;
            TempInputSignal.Bit24 = false;
            TempInputSignal.Bit25 = false;
            TempInputSignal.Bit26 = false;
            TempInputSignal.Bit27 = false;
            TempInputSignal.Bit28 = false;
            TempInputSignal.Bit29 = false;
            TempInputSignal.Bit30 = false;
            TempInputSignal.Bit31 = false;

            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return TempInputSignal;
                    }

                int TempReturn = 0;
                TempReturn = d2410_read_inport(TargetCard);

                //Bit0
                if ((TempReturn & 1) == 1)
                    {
                    TempInputSignal.Bit0 = true;
                    }
                //else 
                //    {
                //    TempInputSignal.Bit0 = false;
                //    }

                //Bit1
                if (((TempReturn>>1) & 1) == 1)
                    {
                    TempInputSignal.Bit1 = true;
                    }

                //Bit2
                if (((TempReturn>>2) & 1) == 1)
                    {
                    TempInputSignal.Bit2 = true;
                    }

                //Bit3
                if (((TempReturn>>3) & 1) == 1)
                    {
                    TempInputSignal.Bit3 = true;
                    }

                //Bit4
                if (((TempReturn>>4) & 1) == 1)
                    {
                    TempInputSignal.Bit4 = true;
                    }

                //Bit5
                if (((TempReturn>>5) & 1) == 1)
                    {
                    TempInputSignal.Bit5 = true;
                    }

                //Bit6
                if (((TempReturn >> 6) & 1) == 1)
                    {
                    TempInputSignal.Bit6 = true;
                    }

                //Bit7
                if (((TempReturn >> 7) & 1) == 1)
                    {
                    TempInputSignal.Bit7 = true;
                    }

                //Bit8
                if (((TempReturn >> 8) & 1) == 1)
                    {
                    TempInputSignal.Bit8 = true;
                    }

                //Bit9
                if (((TempReturn >> 9) & 1) == 1)
                    {
                    TempInputSignal.Bit9 = true;
                    }

                //Bit10
                if (((TempReturn >> 10) & 1) == 1)
                    {
                    TempInputSignal.Bit10 = true;
                    }

                //Bit11
                if (((TempReturn >> 11) & 1) == 1)
                    {
                    TempInputSignal.Bit11 = true;
                    }

                //Bit12
                if (((TempReturn >> 12) & 1) == 1)
                    {
                    TempInputSignal.Bit12 = true;
                    }

                //Bit13
                if (((TempReturn >> 13) & 1) == 1)
                    {
                    TempInputSignal.Bit13 = true;
                    }

                //Bit14
                if (((TempReturn >> 14) & 1) == 1)
                    {
                    TempInputSignal.Bit14 = true;
                    }

                //Bit15
                if (((TempReturn >> 15) & 1) == 1)
                    {
                    TempInputSignal.Bit15 = true;
                    }

                //Bit15
                if (((TempReturn >> 15) & 1) == 1)
                    {
                    TempInputSignal.Bit15 = true;
                    }

                //Bit16
                if (((TempReturn >> 16) & 1) == 1)
                    {
                    TempInputSignal.Bit16 = true;
                    }

                //Bit17
                if (((TempReturn >> 17) & 1) == 1)
                    {
                    TempInputSignal.Bit17 = true;
                    }

                //Bit18
                if (((TempReturn >> 18) & 1) == 1)
                    {
                    TempInputSignal.Bit18 = true;
                    }

                //Bit19
                if (((TempReturn >> 19) & 1) == 1)
                    {
                    TempInputSignal.Bit19 = true;
                    }

                //Bit20
                if (((TempReturn >> 20) & 1) == 1)
                    {
                    TempInputSignal.Bit20 = true;
                    }

                //Bit21
                if (((TempReturn >> 21) & 1) == 1)
                    {
                    TempInputSignal.Bit21 = true;
                    }

                //Bit22
                if (((TempReturn >> 22) & 1) == 1)
                    {
                    TempInputSignal.Bit22 = true;
                    }

                //Bit23
                if (((TempReturn >> 23) & 1) == 1)
                    {
                    TempInputSignal.Bit23 = true;
                    }

                //Bit24
                if (((TempReturn >> 24) & 1) == 1)
                    {
                    TempInputSignal.Bit24 = true;
                    }

                //Bit25
                if (((TempReturn >> 25) & 1) == 1)
                    {
                    TempInputSignal.Bit25 = true;
                    }

                //Bit26
                if (((TempReturn >> 26) & 1) == 1)
                    {
                    TempInputSignal.Bit26 = true;
                    }

                //Bit27
                if (((TempReturn >> 27) & 1) == 1)
                    {
                    TempInputSignal.Bit27 = true;
                    }

                //Bit28
                if (((TempReturn >> 28) & 1) == 1)
                    {
                    TempInputSignal.Bit28 = true;
                    }

                //Bit29
                if (((TempReturn >> 29) & 1) == 1)
                    {
                    TempInputSignal.Bit29 = true;
                    }

                //Bit30
                if (((TempReturn >> 30) & 1) == 1)
                    {
                    TempInputSignal.Bit30 = true;
                    }

                //Bit31
                if (((TempReturn >> 31) & 1) == 1)
                    {
                    TempInputSignal.Bit31 = true;
                    }

                return TempInputSignal;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return TempInputSignal;
                }
            }

        //读取指定控制卡的全部通用输出口的电平状态
        /// <summary>
        /// 读取指定控制卡的全部通用输出口的电平状态
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <returns>bit0~bit19、bit24~bit31位值分别代表第1~20、25~32号输出端口值</returns>
        public int GetAllOutputStatus(ushort TargetCardNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 0;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                int TempReturn = 0;
                TempReturn = d2410_read_outport(TargetCardNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取当前控制卡的全部通用输出口的电平状态
        /// <summary>
        /// 读取当前控制卡的全部通用输出口的电平状态
        /// </summary>
        /// <returns>bit0~bit19、bit24~bit31位值分别代表第1~20、25~32号输出端口值</returns>
        public int GetAllOutputStatus()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = d2410_read_outport(TargetCard);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取当前控制卡的全部通用输出口的电平状态
        /// <summary>
        /// 读取当前控制卡的全部通用输出口的电平状态
        /// </summary>
        /// <returns>bit0~bit19、bit24~bit31位值分别代表第1~20、25~32号输出端口值</returns>
        public OutputSignal GetAllOutputStatusNew()
            {
            OutputSignal TempOutputSignal;
            TempOutputSignal.Bit0 = false;
            TempOutputSignal.Bit1 = false;
            TempOutputSignal.Bit2 = false;
            TempOutputSignal.Bit3 = false;
            TempOutputSignal.Bit4 = false;
            TempOutputSignal.Bit5 = false;
            TempOutputSignal.Bit6 = false;
            TempOutputSignal.Bit7 = false;
            TempOutputSignal.Bit8 = false;
            TempOutputSignal.Bit9 = false;
            TempOutputSignal.Bit10 = false;
            TempOutputSignal.Bit11 = false;
            TempOutputSignal.Bit12 = false;
            TempOutputSignal.Bit13 = false;
            TempOutputSignal.Bit14 = false;
            TempOutputSignal.Bit15 = false;
            TempOutputSignal.Bit16 = false;
            TempOutputSignal.Bit17 = false;
            TempOutputSignal.Bit18 = false;
            TempOutputSignal.Bit19 = false;
            TempOutputSignal.Bit24 = false;
            TempOutputSignal.Bit25 = false;
            TempOutputSignal.Bit26 = false;
            TempOutputSignal.Bit27 = false;
            TempOutputSignal.Bit28 = false;
            TempOutputSignal.Bit29 = false;
            TempOutputSignal.Bit30 = false;
            TempOutputSignal.Bit31 = false;

            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return TempOutputSignal;
                    }

                int TempReturn = 0;
                TempReturn = d2410_read_outport(TargetCard);

                //Bit0
                if ((TempReturn & 1) == 1)
                    {
                    TempOutputSignal.Bit0 = true;
                    }
                //else
                //    {
                //    TempOutputSignal.Bit0 = false;
                //    }

                //Bit1
                if (((TempReturn >> 1) & 1) == 1)
                    {
                    TempOutputSignal.Bit1 = true;
                    }

                //Bit2
                if (((TempReturn >> 2) & 1) == 1)
                    {
                    TempOutputSignal.Bit2 = true;
                    }

                //Bit3
                if (((TempReturn >> 3) & 1) == 1)
                    {
                    TempOutputSignal.Bit3 = true;
                    }

                //Bit4
                if (((TempReturn >> 4) & 1) == 1)
                    {
                    TempOutputSignal.Bit4 = true;
                    }

                //Bit5
                if (((TempReturn >> 5) & 1) == 1)
                    {
                    TempOutputSignal.Bit5 = true;
                    }

                //Bit6
                if (((TempReturn >> 6) & 1) == 1)
                    {
                    TempOutputSignal.Bit6 = true;
                    }

                //Bit7
                if (((TempReturn >> 7) & 1) == 1)
                    {
                    TempOutputSignal.Bit7 = true;
                    }

                //Bit8
                if (((TempReturn >> 8) & 1) == 1)
                    {
                    TempOutputSignal.Bit8 = true;
                    }

                //Bit9
                if (((TempReturn >> 9) & 1) == 1)
                    {
                    TempOutputSignal.Bit9 = true;
                    }

                //Bit10
                if (((TempReturn >> 10) & 1) == 1)
                    {
                    TempOutputSignal.Bit10 = true;
                    }

                //Bit11
                if (((TempReturn >> 11) & 1) == 1)
                    {
                    TempOutputSignal.Bit11 = true;
                    }

                //Bit12
                if (((TempReturn >> 12) & 1) == 1)
                    {
                    TempOutputSignal.Bit12 = true;
                    }

                //Bit13
                if (((TempReturn >> 13) & 1) == 1)
                    {
                    TempOutputSignal.Bit13 = true;
                    }

                //Bit14
                if (((TempReturn >> 14) & 1) == 1)
                    {
                    TempOutputSignal.Bit14 = true;
                    }

                //Bit15
                if (((TempReturn >> 15) & 1) == 1)
                    {
                    TempOutputSignal.Bit15 = true;
                    }

                //Bit16
                if (((TempReturn >> 16) & 1) == 1)
                    {
                    TempOutputSignal.Bit16 = true;
                    }

                //Bit17
                if (((TempReturn >> 17) & 1) == 1)
                    {
                    TempOutputSignal.Bit17 = true;
                    }

                //Bit18
                if (((TempReturn >> 18) & 1) == 1)
                    {
                    TempOutputSignal.Bit18 = true;
                    }

                //Bit19
                if (((TempReturn >> 19) & 1) == 1)
                    {
                    TempOutputSignal.Bit19 = true;
                    }
                
                //Bit24
                if (((TempReturn >> 24) & 1) == 1)
                    {
                    TempOutputSignal.Bit24 = true;
                    }

                //Bit25
                if (((TempReturn >> 25) & 1) == 1)
                    {
                    TempOutputSignal.Bit25 = true;
                    }

                //Bit26
                if (((TempReturn >> 26) & 1) == 1)
                    {
                    TempOutputSignal.Bit26 = true;
                    }

                //Bit27
                if (((TempReturn >> 27) & 1) == 1)
                    {
                    TempOutputSignal.Bit27 = true;
                    }

                //Bit28
                if (((TempReturn >> 28) & 1) == 1)
                    {
                    TempOutputSignal.Bit28 = true;
                    }

                //Bit29
                if (((TempReturn >> 29) & 1) == 1)
                    {
                    TempOutputSignal.Bit29 = true;
                    }

                //Bit30
                if (((TempReturn >> 30) & 1) == 1)
                    {
                    TempOutputSignal.Bit30 = true;
                    }

                //Bit31
                if (((TempReturn >> 31) & 1) == 1)
                    {
                    TempOutputSignal.Bit31 = true;
                    }

                return TempOutputSignal;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return TempOutputSignal;
                }
            }

        //读取指定控制卡的全部通用输出口的电平状态
        /// <summary>
        /// 读取指定控制卡的全部通用输出口的电平状态
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <returns>bit0~bit19、bit24~bit31位值分别代表第1~20、25~32号输出端口值</returns>
        public OutputSignal GetAllOutputStatusNew(ushort TargetCardNumber)
            {
            OutputSignal TempOutputSignal;
            TempOutputSignal.Bit0 = false;
            TempOutputSignal.Bit1 = false;
            TempOutputSignal.Bit2 = false;
            TempOutputSignal.Bit3 = false;
            TempOutputSignal.Bit4 = false;
            TempOutputSignal.Bit5 = false;
            TempOutputSignal.Bit6 = false;
            TempOutputSignal.Bit7 = false;
            TempOutputSignal.Bit8 = false;
            TempOutputSignal.Bit9 = false;
            TempOutputSignal.Bit10 = false;
            TempOutputSignal.Bit11 = false;
            TempOutputSignal.Bit12 = false;
            TempOutputSignal.Bit13 = false;
            TempOutputSignal.Bit14 = false;
            TempOutputSignal.Bit15 = false;
            TempOutputSignal.Bit16 = false;
            TempOutputSignal.Bit17 = false;
            TempOutputSignal.Bit18 = false;
            TempOutputSignal.Bit19 = false;
            TempOutputSignal.Bit24 = false;
            TempOutputSignal.Bit25 = false;
            TempOutputSignal.Bit26 = false;
            TempOutputSignal.Bit27 = false;
            TempOutputSignal.Bit28 = false;
            TempOutputSignal.Bit29 = false;
            TempOutputSignal.Bit30 = false;
            TempOutputSignal.Bit31 = false;

            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return TempOutputSignal;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return TempOutputSignal;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                int TempReturn = 0;
                TempReturn = d2410_read_outport(TargetCardNumber);

                //Bit0
                if ((TempReturn & 1) == 1)
                    {
                    TempOutputSignal.Bit0 = true;
                    }
                //else
                //    {
                //    TempOutputSignal.Bit0 = false;
                //    }

                //Bit1
                if (((TempReturn >> 1) & 1) == 1)
                    {
                    TempOutputSignal.Bit1 = true;
                    }

                //Bit2
                if (((TempReturn >> 2) & 1) == 1)
                    {
                    TempOutputSignal.Bit2 = true;
                    }

                //Bit3
                if (((TempReturn >> 3) & 1) == 1)
                    {
                    TempOutputSignal.Bit3 = true;
                    }

                //Bit4
                if (((TempReturn >> 4) & 1) == 1)
                    {
                    TempOutputSignal.Bit4 = true;
                    }

                //Bit5
                if (((TempReturn >> 5) & 1) == 1)
                    {
                    TempOutputSignal.Bit5 = true;
                    }

                //Bit6
                if (((TempReturn >> 6) & 1) == 1)
                    {
                    TempOutputSignal.Bit6 = true;
                    }

                //Bit7
                if (((TempReturn >> 7) & 1) == 1)
                    {
                    TempOutputSignal.Bit7 = true;
                    }

                //Bit8
                if (((TempReturn >> 8) & 1) == 1)
                    {
                    TempOutputSignal.Bit8 = true;
                    }

                //Bit9
                if (((TempReturn >> 9) & 1) == 1)
                    {
                    TempOutputSignal.Bit9 = true;
                    }

                //Bit10
                if (((TempReturn >> 10) & 1) == 1)
                    {
                    TempOutputSignal.Bit10 = true;
                    }

                //Bit11
                if (((TempReturn >> 11) & 1) == 1)
                    {
                    TempOutputSignal.Bit11 = true;
                    }

                //Bit12
                if (((TempReturn >> 12) & 1) == 1)
                    {
                    TempOutputSignal.Bit12 = true;
                    }

                //Bit13
                if (((TempReturn >> 13) & 1) == 1)
                    {
                    TempOutputSignal.Bit13 = true;
                    }

                //Bit14
                if (((TempReturn >> 14) & 1) == 1)
                    {
                    TempOutputSignal.Bit14 = true;
                    }

                //Bit15
                if (((TempReturn >> 15) & 1) == 1)
                    {
                    TempOutputSignal.Bit15 = true;
                    }

                //Bit16
                if (((TempReturn >> 16) & 1) == 1)
                    {
                    TempOutputSignal.Bit16 = true;
                    }

                //Bit17
                if (((TempReturn >> 17) & 1) == 1)
                    {
                    TempOutputSignal.Bit17 = true;
                    }

                //Bit18
                if (((TempReturn >> 18) & 1) == 1)
                    {
                    TempOutputSignal.Bit18 = true;
                    }

                //Bit19
                if (((TempReturn >> 19) & 1) == 1)
                    {
                    TempOutputSignal.Bit19 = true;
                    }

                //Bit24
                if (((TempReturn >> 24) & 1) == 1)
                    {
                    TempOutputSignal.Bit24 = true;
                    }

                //Bit25
                if (((TempReturn >> 25) & 1) == 1)
                    {
                    TempOutputSignal.Bit25 = true;
                    }

                //Bit26
                if (((TempReturn >> 26) & 1) == 1)
                    {
                    TempOutputSignal.Bit26 = true;
                    }

                //Bit27
                if (((TempReturn >> 27) & 1) == 1)
                    {
                    TempOutputSignal.Bit27 = true;
                    }

                //Bit28
                if (((TempReturn >> 28) & 1) == 1)
                    {
                    TempOutputSignal.Bit28 = true;
                    }

                //Bit29
                if (((TempReturn >> 29) & 1) == 1)
                    {
                    TempOutputSignal.Bit29 = true;
                    }

                //Bit30
                if (((TempReturn >> 30) & 1) == 1)
                    {
                    TempOutputSignal.Bit30 = true;
                    }

                //Bit31
                if (((TempReturn >> 31) & 1) == 1)
                    {
                    TempOutputSignal.Bit31 = true;
                    }

                return TempOutputSignal;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return TempOutputSignal;
                }
            }

        //设定指定控制卡的全部通用输出口的电平状态
        /// <summary>
        /// 设定指定控制卡的全部通用输出口的电平状态
        /// OUT1～OUT16端口可设置上电时的初始电平，OUT17～OUT20上电初始电平为高。
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <param name="PortValue">bit0~bit19、bit24~bit31位值分别代表第1~20、25~32号输出端口值</param>
        /// <returns>错误代码</returns>
        public int SetAllOutputStatus(ushort TargetCardNumber, uint PortValue)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                int TempReturn = 0;
                TempReturn = (int)d2410_write_outport(TargetCardNumber, PortValue);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设定当前控制卡的全部通用输出口的电平状态
        /// <summary>
        /// 设定当前控制卡的全部通用输出口的电平状态
        /// OUT1～OUT16端口可设置上电时的初始电平，OUT17～OUT20上电初始电平为高。
        /// </summary>
        /// <param name="PortValue">bit0~bit19、bit24~bit31位值分别代表第1~20、25~32号输出端口值</param>
        /// <returns>错误代码</returns>
        public int SetAllOutputStatus(uint PortValue)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_write_outport(TargetCard, PortValue);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
        
        //设定指定控制卡的全部通用输出口的电平状态
        /// <summary>
        /// 设定指定控制卡的全部通用输出口的电平状态
        /// OUT1～OUT16端口可设置上电时的初始电平，OUT17～OUT20上电初始电平为高。
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <param name="PortValue">bit0~bit19、bit24~bit31位值分别代表第1~20、25~32号输出端口值</param>
        /// <returns>错误代码</returns>
        public int SetAllOutputStatus(ushort TargetCardNumber, OutputSignal PortValue)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                int TempPortValue = 0;

                //Bit0
                if (PortValue.Bit0 == true)
                    {
                    TempPortValue = TempPortValue & 1;
                    }

                //Bit1
                if (PortValue.Bit1 == true)
                    {
                    TempPortValue = TempPortValue & 2;
                    }

                //Bit2
                if (PortValue.Bit2 == true)
                    {
                    TempPortValue = TempPortValue & 4;
                    }

                //Bit3
                if (PortValue.Bit3 == true)
                    {
                    TempPortValue = TempPortValue & 8;
                    }

                //Bit4
                if (PortValue.Bit4 == true)
                    {
                    TempPortValue = TempPortValue & 16;
                    }

                //Bit5
                if (PortValue.Bit5 == true)
                    {
                    TempPortValue = TempPortValue & 32;
                    }

                //Bit6
                if (PortValue.Bit6 == true)
                    {
                    TempPortValue = TempPortValue & 64;
                    }

                //Bit7
                if (PortValue.Bit7 == true)
                    {
                    TempPortValue = TempPortValue & 128;
                    }

                //Bit8
                if (PortValue.Bit8 == true)
                    {
                    TempPortValue = TempPortValue & (2<<8);
                    //TempPortValue = TempPortValue & (1<<9);
                    }

                //Bit9
                if (PortValue.Bit9 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 9);
                    }

                //Bit10
                if (PortValue.Bit10 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 10);
                    }

                //Bit11
                if (PortValue.Bit11 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 11);
                    }

                //Bit12
                if (PortValue.Bit12 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 12);
                    }

                //Bit13
                if (PortValue.Bit13 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 13);
                    }

                //Bit14
                if (PortValue.Bit14 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 14);
                    }

                //Bit15
                if (PortValue.Bit15 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 15);
                    }

                //Bit16
                if (PortValue.Bit16 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 16);
                    }

                //Bit17
                if (PortValue.Bit17 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 17);
                    }

                //Bit18
                if (PortValue.Bit18 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 18);
                    }

                //Bit19
                if (PortValue.Bit19 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 19);
                    }

                //Bit24
                if (PortValue.Bit24 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 24);
                    }

                //Bit25
                if (PortValue.Bit25 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 25);
                    }

                //Bit25
                if (PortValue.Bit25 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 25);
                    }

                //Bit26
                if (PortValue.Bit26 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 26);
                    }

                //Bit27
                if (PortValue.Bit27 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 27);
                    }

                //Bit28
                if (PortValue.Bit28 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 28);
                    }

                //Bit29
                if (PortValue.Bit29 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 29);
                    }

                //Bit30
                if (PortValue.Bit30 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 30);
                    }

                //Bit31
                if (PortValue.Bit31 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 31);
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_write_outport(TargetCardNumber, 
                    (uint)TempPortValue);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设定当前控制卡的全部通用输出口的电平状态
        /// <summary>
        /// 设定当前控制卡的全部通用输出口的电平状态
        /// OUT1～OUT16端口可设置上电时的初始电平，OUT17～OUT20上电初始电平为高。
        /// </summary>
        /// <param name="PortValue">bit0~bit19、bit24~bit31位值分别代表第1~20、25~32号输出端口值</param>
        /// <returns>错误代码</returns>
        public int SetAllOutputStatus(OutputSignal PortValue)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempPortValue = 0;

                //Bit0
                if (PortValue.Bit0 == true)
                    {
                    TempPortValue = TempPortValue & 1;
                    }

                //Bit1
                if (PortValue.Bit1 == true)
                    {
                    TempPortValue = TempPortValue & 2;
                    }

                //Bit2
                if (PortValue.Bit2 == true)
                    {
                    TempPortValue = TempPortValue & 4;
                    }

                //Bit3
                if (PortValue.Bit3 == true)
                    {
                    TempPortValue = TempPortValue & 8;
                    }

                //Bit4
                if (PortValue.Bit4 == true)
                    {
                    TempPortValue = TempPortValue & 16;
                    }

                //Bit5
                if (PortValue.Bit5 == true)
                    {
                    TempPortValue = TempPortValue & 32;
                    }

                //Bit6
                if (PortValue.Bit6 == true)
                    {
                    TempPortValue = TempPortValue & 64;
                    }

                //Bit7
                if (PortValue.Bit7 == true)
                    {
                    TempPortValue = TempPortValue & 128;
                    }

                //Bit8
                if (PortValue.Bit8 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 8);
                    //TempPortValue = TempPortValue & (1<<9);
                    }

                //Bit9
                if (PortValue.Bit9 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 9);
                    }

                //Bit10
                if (PortValue.Bit10 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 10);
                    }

                //Bit11
                if (PortValue.Bit11 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 11);
                    }

                //Bit12
                if (PortValue.Bit12 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 12);
                    }

                //Bit13
                if (PortValue.Bit13 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 13);
                    }

                //Bit14
                if (PortValue.Bit14 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 14);
                    }

                //Bit15
                if (PortValue.Bit15 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 15);
                    }

                //Bit16
                if (PortValue.Bit16 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 16);
                    }

                //Bit17
                if (PortValue.Bit17 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 17);
                    }

                //Bit18
                if (PortValue.Bit18 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 18);
                    }

                //Bit19
                if (PortValue.Bit19 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 19);
                    }

                //Bit24
                if (PortValue.Bit24 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 24);
                    }

                //Bit25
                if (PortValue.Bit25 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 25);
                    }

                //Bit25
                if (PortValue.Bit25 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 25);
                    }

                //Bit26
                if (PortValue.Bit26 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 26);
                    }

                //Bit27
                if (PortValue.Bit27 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 27);
                    }

                //Bit28
                if (PortValue.Bit28 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 28);
                    }

                //Bit29
                if (PortValue.Bit29 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 29);
                    }

                //Bit30
                if (PortValue.Bit30 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 30);
                    }

                //Bit31
                if (PortValue.Bit31 == true)
                    {
                    TempPortValue = TempPortValue & (2 << 31);
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_write_outport(TargetCard,
                    (uint)TempPortValue);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
                
        #endregion

        #region "编码器计数功能"

        //编码器检测的实现
        //DMC2410C的反馈位置计数器是一个32位正负计数器，对通过控制卡编码器接口EA，
        //EB输入的脉冲（如编码器、光栅尺反馈脉冲等）进行计数。

        //编码器检测相关函数说明
        //d2410_counter_config           '设置编码器输入口的计数方式
        //d2410_get_encoder              '读取编码器反馈的脉冲计数值
        //d2410_set_encoder              '设置编码器的脉冲计数值

        //例程7.10：编码器反馈计数的操作
        //d2410_counter_config 0,3            '设置轴0为4倍计数，默认的EA、EB计数方向 
        //d2410_set_encoder 0,0               '设置轴0的计数初始值为0 
        //X_Position = d2410_get_encoder(0)   '读轴0的计数器的数值至变量X_Position

        /// <summary>
        /// 读取轴编码器反馈位置脉冲计数值，范围：28位有符号数
        /// </summary>
        /// <returns>位置反馈脉冲值，单位：pulse</returns>
        public int GetEncoderPosInPulse()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_encoder(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取轴编码器反馈位置【单位：mm】
        /// <summary>
        /// 读取轴编码器反馈位置【单位：mm】
        /// </summary>
        /// <param name="EncoderPosInMM">位置反馈值【单位：mm】</param>
        /// <returns></returns>
        public int GetEncoderPosInMM(ref double EncoderPosInMM)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_encoder_unitmm(TargetAxis, 
                    ref EncoderPosInMM);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置轴编码器反馈脉冲计数值，范围：28位有符号数
        /// <summary>
        /// 设置轴编码器反馈脉冲计数值，范围：28位有符号数
        /// </summary>
        /// <param name="EncoderValueInPulse">编码器反馈脉冲计数的设定值</param>
        /// <returns>错误代码</returns>
        public int SetEncoderPosInPulse(uint EncoderValueInPulse)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_encoder(TargetAxis, EncoderValueInPulse);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置轴编码器反馈数值【单位：mm】
        /// <summary>
        /// 设置轴编码器反馈数值【单位：mm】
        /// </summary>
        /// <param name="EncoderPosValueInMM">编码器的设定值【单位：mm】</param>
        /// <returns>错误代码</returns>
        public int SetEncoderPosInMM(double EncoderPosValueInMM)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_encoder_unitmm(TargetAxis, 
                    EncoderPosValueInMM);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取指定控制卡的计数器的标识位
        /// <summary>
        /// 读取指定控制卡的计数器的标识位
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <returns>返回值位号 0: 指定卡的X轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 1: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 2: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 3: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 4~7: 保留
        /// 返回值位号 8: 指定卡的Y轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 9: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 10: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 11: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 12~15: 保留
        /// 返回值位号 16: 指定卡的Z轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 17: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 18: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 19: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 20~23: 保留
        /// 返回值位号 24: 指定卡的U轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 25: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 26: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 27: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 28~31: 保留
        /// </returns>
        public int GetCounterFlag(ushort TargetCardNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 0;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                int TempReturn = 0;
                TempReturn = (int)d2410_get_counter_flag(TargetCardNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //读取当前控制卡的计数器的标识位
        /// <summary>
        /// 读取当前控制卡的计数器的标识位
        /// </summary>
        /// <returns>返回值位号 0: 指定卡的X轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 1: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 2: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 3: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 4~7: 保留
        /// 返回值位号 8: 指定卡的Y轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 9: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 10: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 11: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 12~15: 保留
        /// 返回值位号 16: 指定卡的Z轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 17: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 18: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 19: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 20~23: 保留
        /// 返回值位号 24: 指定卡的U轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 25: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 26: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 27: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 28~31: 保留
        /// </returns>
        public int GetCounterFlag()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_counter_flag(TargetCard);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //读取指定控制卡的计数器的标识位
        /// <summary>
        /// 读取指定控制卡的计数器的标识位
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <returns>返回值位号 0: 指定卡的X轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 1: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 2: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 3: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 4~7: 保留
        /// 返回值位号 8: 指定卡的Y轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 9: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 10: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 11: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 12~15: 保留
        /// 返回值位号 16: 指定卡的Z轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 17: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 18: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 19: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 20~23: 保留
        /// 返回值位号 24: 指定卡的U轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 25: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 26: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 27: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 28~31: 保留
        /// </returns>
        public CounterFlag GetCounterFlagNew(ushort TargetCardNumber)
            {
            CounterFlag TempCounterFlag;
            TempCounterFlag.Bit0 = false;
            TempCounterFlag.Bit1 = false;
            TempCounterFlag.Bit2 = false;
            TempCounterFlag.Bit3 = false;
            TempCounterFlag.Bit8 = false;
            TempCounterFlag.Bit9 = false;
            TempCounterFlag.Bit10 = false;
            TempCounterFlag.Bit11 = false;
            TempCounterFlag.Bit16 = false;
            TempCounterFlag.Bit17 = false;
            TempCounterFlag.Bit18 = false;
            TempCounterFlag.Bit19 = false;
            TempCounterFlag.Bit24 = false;
            TempCounterFlag.Bit25 = false;
            TempCounterFlag.Bit26 = false;
            TempCounterFlag.Bit27 = false;

            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return TempCounterFlag;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return TempCounterFlag;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);
                int TempReturn = 0;
                TempReturn = (int)d2410_get_counter_flag(TargetCardNumber);

                //Bit0
                if ((TempReturn & 1) == 1)
                    {
                    TempCounterFlag.Bit0 = true;
                    }
                //else
                //    {
                //    TempCounterFlag.Bit0 = false;
                //    }

                //Bit1
                if (((TempReturn >> 1) & 1) == 1)
                    {
                    TempCounterFlag.Bit1 = true;
                    }

                //Bit2
                if (((TempReturn >> 2) & 1) == 1)
                    {
                    TempCounterFlag.Bit2 = true;
                    }

                //Bit3
                if (((TempReturn >> 3) & 1) == 1)
                    {
                    TempCounterFlag.Bit3 = true;
                    }

                //Bit8
                if (((TempReturn >> 8) & 1) == 1)
                    {
                    TempCounterFlag.Bit8 = true;
                    }

                //Bit9
                if (((TempReturn >> 9) & 1) == 1)
                    {
                    TempCounterFlag.Bit9 = true;
                    }

                //Bit10
                if (((TempReturn >> 10) & 1) == 1)
                    {
                    TempCounterFlag.Bit10 = true;
                    }

                //Bit11
                if (((TempReturn >> 11) & 1) == 1)
                    {
                    TempCounterFlag.Bit11 = true;
                    }

                //Bit16
                if (((TempReturn >> 16) & 1) == 1)
                    {
                    TempCounterFlag.Bit16 = true;
                    }

                //Bit17
                if (((TempReturn >> 17) & 1) == 1)
                    {
                    TempCounterFlag.Bit17 = true;
                    }

                //Bit18
                if (((TempReturn >> 18) & 1) == 1)
                    {
                    TempCounterFlag.Bit18 = true;
                    }

                //Bit19
                if (((TempReturn >> 19) & 1) == 1)
                    {
                    TempCounterFlag.Bit19 = true;
                    }

                //Bit24
                if (((TempReturn >> 24) & 1) == 1)
                    {
                    TempCounterFlag.Bit24 = true;
                    }

                //Bit25
                if (((TempReturn >> 25) & 1) == 1)
                    {
                    TempCounterFlag.Bit25 = true;
                    }

                //Bit26
                if (((TempReturn >> 26) & 1) == 1)
                    {
                    TempCounterFlag.Bit26 = true;
                    }

                //Bit27
                if (((TempReturn >> 27) & 1) == 1)
                    {
                    TempCounterFlag.Bit27 = true;
                    }

                return TempCounterFlag;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return TempCounterFlag;
                }
            }

        //读取当前控制卡的计数器的标识位
        /// <summary>
        /// 读取当前控制卡的计数器的标识位
        /// </summary>
        /// <returns>返回值位号 0: 指定卡的X轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 1: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 2: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 3: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 4~7: 保留
        /// 返回值位号 8: 指定卡的Y轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 9: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 10: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 11: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 12~15: 保留
        /// 返回值位号 16: 指定卡的Z轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 17: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 18: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 19: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 20~23: 保留
        /// 返回值位号 24: 指定卡的U轴 借位触发标志：计数器每下溢出时触发一次
        /// 返回值位号 25: 进位触发标志：计数器每上溢出时触发一次
        /// 返回值位号 26: 符号标志：计数器上溢出为0，下溢出为1
        /// 返回值位号 27: 上下计数标志：上计数时为1，下计数时为0
        /// 返回值位号 28~31: 保留
        /// </returns>
        public CounterFlag GetCounterFlagNew()
            {
            CounterFlag TempCounterFlag;
            TempCounterFlag.Bit0 = false;
            TempCounterFlag.Bit1 = false;
            TempCounterFlag.Bit2 = false;
            TempCounterFlag.Bit3 = false;
            TempCounterFlag.Bit8 = false;
            TempCounterFlag.Bit9 = false;
            TempCounterFlag.Bit10 = false;
            TempCounterFlag.Bit11 = false;
            TempCounterFlag.Bit16 = false;
            TempCounterFlag.Bit17 = false;
            TempCounterFlag.Bit18 = false;
            TempCounterFlag.Bit19 = false;
            TempCounterFlag.Bit24 = false;
            TempCounterFlag.Bit25 = false;
            TempCounterFlag.Bit26 = false;
            TempCounterFlag.Bit27 = false;

            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return TempCounterFlag;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_counter_flag(TargetCard);

                //Bit0
                if ((TempReturn & 1) == 1)
                    {
                    TempCounterFlag.Bit0 = true;
                    }
                //else
                //    {
                //    TempCounterFlag.Bit0 = false;
                //    }

                //Bit1
                if (((TempReturn >> 1) & 1) == 1)
                    {
                    TempCounterFlag.Bit1 = true;
                    }

                //Bit2
                if (((TempReturn >> 2) & 1) == 1)
                    {
                    TempCounterFlag.Bit2 = true;
                    }

                //Bit3
                if (((TempReturn >> 3) & 1) == 1)
                    {
                    TempCounterFlag.Bit3 = true;
                    }

                //Bit8
                if (((TempReturn >> 8) & 1) == 1)
                    {
                    TempCounterFlag.Bit8 = true;
                    }

                //Bit9
                if (((TempReturn >> 9) & 1) == 1)
                    {
                    TempCounterFlag.Bit9 = true;
                    }

                //Bit10
                if (((TempReturn >> 10) & 1) == 1)
                    {
                    TempCounterFlag.Bit10 = true;
                    }

                //Bit11
                if (((TempReturn >> 11) & 1) == 1)
                    {
                    TempCounterFlag.Bit11 = true;
                    }

                //Bit16
                if (((TempReturn >> 16) & 1) == 1)
                    {
                    TempCounterFlag.Bit16 = true;
                    }

                //Bit17
                if (((TempReturn >> 17) & 1) == 1)
                    {
                    TempCounterFlag.Bit17 = true;
                    }

                //Bit18
                if (((TempReturn >> 18) & 1) == 1)
                    {
                    TempCounterFlag.Bit18 = true;
                    }

                //Bit19
                if (((TempReturn >> 19) & 1) == 1)
                    {
                    TempCounterFlag.Bit19 = true;
                    }

                //Bit24
                if (((TempReturn >> 24) & 1) == 1)
                    {
                    TempCounterFlag.Bit24 = true;
                    }

                //Bit25
                if (((TempReturn >> 25) & 1) == 1)
                    {
                    TempCounterFlag.Bit25 = true;
                    }

                //Bit26
                if (((TempReturn >> 26) & 1) == 1)
                    {
                    TempCounterFlag.Bit26 = true;
                    }

                //Bit27
                if (((TempReturn >> 27) & 1) == 1)
                    {
                    TempCounterFlag.Bit27 = true;
                    }

                return TempCounterFlag;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return TempCounterFlag;
                }
            }

        //复位指定控制卡计数器的计数标志位, 范围（1~N，N为卡号）
        /// <summary>
        /// 复位指定控制卡计数器的计数标志位, 范围（1~N，N为卡号）
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号</param>
        /// <returns>错误代码</returns>
        public int ResetCounterFlag(ushort TargetCardNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                int TempReturn = 0;
                TempReturn = (int)d2410_reset_counter_flag(TargetCardNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //复位当前控制卡计数器的计数标志位, 范围（1~N，N为卡号）
        /// <summary>
        /// 复位当前控制卡计数器的计数标志位, 范围（1~N，N为卡号）
        /// </summary>
        /// <returns>错误代码</returns>
        public int ResetCounterFlag()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_reset_counter_flag(TargetCard);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //复位指定控制卡计数器的清零标志位, 范围（1~N，N为卡号）
        /// <summary>
        /// 复位指定控制卡计数器的清零标志位, 范围（1~N，N为卡号）
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号</param>
        /// <returns>错误代码</returns>
        public int ResetClearFlag(ushort TargetCardNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                int TempReturn = 0;
                TempReturn = (int)d2410_reset_clear_flag(TargetCardNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //复位当前控制卡计数器的清零标志位, 范围（1~N，N为卡号）
        /// <summary>
        /// 复位当前控制卡计数器的清零标志位, 范围（1~N，N为卡号）
        /// </summary>
        /// <returns>错误代码</returns>
        public int ResetClearFlag()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }
                
                int TempReturn = 0;
                TempReturn = (int)d2410_reset_clear_flag(TargetCard);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置指定轴的EZ信号的有效电平及其作用
        /// <summary>
        /// 设置指定轴的EZ信号的有效电平及其作用
        /// </summary>
        /// <param name="EZLogic">EZ信号有效电平：
        /// 0：低有效，
        /// 1：高有效</param>
        /// <param name="EZMode">EZ信号的工作方式：
        /// 0：EZ信号无效，
        /// 1：EZ是计数器复位信号</param>
        /// <returns></returns>
        public int SetEZSignal(ushort EZLogic, ushort EZMode)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (EZLogic != 0) 
                    {
                    EZLogic = 1;
                    }

                if (EZMode != 0) 
                    {
                    EZMode = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)1;

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
        
        #endregion
        
        #region "高速锁存"

        //位置锁存功能的实现
        //DMC2410C卡提供了高速位置锁存功能，位置锁存无触发延时时间，当捕获到位置锁存信号后立即锁存当前位置。

        //高速锁存相关函数说明
        //d2410_config_latch_mode    '设置锁存方式为单轴锁存或是四轴同时锁存
        //d2410_get_latch_value      '读取编码器锁存器的值
        //d2410_get_latch_flag       '读取指定控制卡的锁存器的标志位
        //d2410_reset_latch_flag     '复位指定控制卡的锁存器的标志位
        //d2410_triger_chunnel       '选择全部锁存时的外触发信号通道

        //注意：1）在位置锁存中，多次触发高速锁存口只锁存第一次触发位置，只有调用函数清除锁存状态方可再次锁存； 
        //      2）位置锁存暂时只支持反馈位置锁存。

        //例7.13：位置锁存 
        //Dim MyCardNo, Myaxis ,Myall_enable As Integer 
        //Dim Mylatch_value As Long 
        //MyCardNo = 0                                        '卡号 
        //Myaxis = 0                                          '轴号 
        //Myall_enable = 0                                    '设置锁存方式为单独锁存 
        //d2410_config_latch_mode MyCardNo,Myall_enable       '设置锁存方式 
        //d2410_reset_latch_flag MyCardNo                     '复位锁存器标志位 
        //d2410_ex_t_pmove Myaxis,10000,0                     '执行定长运动，位移为10000pulse，相对坐标模式 
        //While ((d2410_get_latch_flag(MyCardNo) And (2 ^ (Myaxis + 8))) = 0) '判断设定轴的LTC锁存状态 
        //DoEvents 
        //Wend 
        //Mylatch_value= d2410_get_latch_value(Myaxis)         '读取锁存器的值，并赋值给变量My_latch_Value
                
        /// <summary>
        /// 设置指定轴“锁存”信号的有效电平及其工作方式
        /// </summary>
        /// <param name="LTCLogic">LTC信号有效电平：0：低有效，1：高有效</param>
        /// <param name="LTCMode">保留参数</param>
        /// <returns>错误代码</returns>
        public int SetLTCSignal(ushort LTCLogic, ushort LTCMode)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (LTCLogic != 0) 
                    {
                    LTCLogic = 1;
                    }

                if (LTCMode != 0) 
                    {
                    LTCMode = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_config_LTC_PIN(TargetAxis, 
                    LTCLogic, LTCMode);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置指定轴“锁存”信号的有效电平及其工作方式,SetLTCSignal扩展函数，增加滤波时间的设定
        /// <summary>
        /// 设置指定轴“锁存”信号的有效电平及其工作方式,
        /// SetLTCSignal扩展函数，增加滤波时间的设定
        /// </summary>
        /// <param name="LTCLogic">LTC信号有效电平：0：低有效，1：高有效</param>
        /// <param name="LTCMode">保留参数</param>
        /// <param name="LTCFilter">滤波时间，单位：ms</param>
        /// <returns>错误代码</returns>
        public int SetLTCSignalExtern(ushort LTCLogic, ushort LTCMode,
            double LTCFilter)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (LTCLogic != 0)
                    {
                    LTCLogic = 1;
                    }

                if (LTCMode != 0)
                    {
                    LTCMode = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_config_LTC_PIN_Extern(TargetAxis,
                    LTCLogic, LTCMode, LTCFilter);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取指定轴“锁存”信号的有效电平及其工作方式
        /// <summary>
        /// 读取指定轴“锁存”信号的有效电平及其工作方式
        /// </summary>
        /// <param name="LTCLogic">LTC信号有效电平：0：低有效，1：高有效</param>
        /// <param name="LTCMode">保留参数</param>
        /// <returns>错误代码</returns>
        public int GetLTCSignal(ref ushort LTCLogic, ref ushort LTCMode)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_config_LTC_PIN(TargetAxis, 
                    ref LTCLogic, ref LTCMode);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取指定轴“锁存”信号的有效电平及其工作方式,GetLTCSignal扩展函数，增加滤波时间
        /// <summary>
        /// 读取指定轴“锁存”信号的有效电平及其工作方式,
        /// GetLTCSignal扩展函数，增加滤波时间
        /// </summary>
        /// <param name="LTCLogic">LTC信号有效电平：0：低有效，1：高有效</param>
        /// <param name="LTCMode">保留参数</param>
        /// <param name="LTCFilter">滤波时间，单位：ms</param>
        /// <returns>错误代码</returns>
        public int GetLTCSignalExtern(ref ushort LTCLogic, ref ushort LTCMode,
            ref double LTCFilter)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_config_LTC_PIN_Extern(TargetAxis, 
                    ref LTCLogic, ref LTCMode, ref LTCFilter);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取编码器锁存器的值
        /// <summary>
        /// 读取编码器锁存器的值
        /// 注意：在位置锁存中，多次触发高速锁存口只锁存第一次触发位置，
        ///       只有调用函数清除锁存状态方可再次锁存
        /// </summary>
        /// <returns>锁存器内的编码器脉冲数，单位：pulse</returns>
        public int GetEncoderLatchValue()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_latch_value(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 0;
                }
            }

        //读取指定控制卡的锁存器的标志位
        /// <summary>
        /// 读取指定控制卡的锁存器的标志位
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <returns>返回值位号: 0, 描述: 0：指定卡的0轴有触发信号
        /// 返回值位号: 1, 描述: 0：1轴有触发信号
        /// 返回值位号: 2, 描述: 0：2轴有触发信号
        /// 返回值位号: 3, 描述: 0：3轴有触发信号
        /// 返回值位号: 4, 描述: 1：指定卡的0轴有清零信号
        /// 返回值位号: 5, 描述: 1：1轴有清零信号
        /// 返回值位号: 6, 描述: 1：2轴有清零信号
        /// 返回值位号: 7, 描述: 1：3轴有清零信号
        /// 返回值位号: 8, 描述: 1：0轴位置已锁存
        /// 返回值位号: 9, 描述: 1：1轴位置已锁存
        /// 返回值位号: 10, 描述: 1：2轴位置已锁存
        /// 返回值位号: 11, 描述: 1：3轴位置已锁存
        /// 返回值位号: 12, 描述: 1：指定卡的0轴位置已清零
        /// 返回值位号: 13, 描述: 1：1轴位置已清零
        /// 返回值位号: 14, 描述: 1：2轴位置已清零
        /// 返回值位号: 15, 描述: 1：3轴位置已清零
        /// 返回值位号: 16~31, 描述:保留
        /// </returns>
        public int GetLatchFlag(ushort TargetCardNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 0;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                int TempReturn = 0;
                TempReturn = (int)d2410_get_latch_flag(TargetCardNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取当前控制卡的锁存器的标志位
        /// <summary>
        /// 读取当前控制卡的锁存器的标志位
        /// </summary>
        /// <returns>返回值位号: 0, 描述: 0：指定卡的0轴有触发信号
        /// 返回值位号: 1, 描述: 0：1轴有触发信号
        /// 返回值位号: 2, 描述: 0：2轴有触发信号
        /// 返回值位号: 3, 描述: 0：3轴有触发信号
        /// 返回值位号: 4, 描述: 1：指定卡的0轴有清零信号
        /// 返回值位号: 5, 描述: 1：1轴有清零信号
        /// 返回值位号: 6, 描述: 1：2轴有清零信号
        /// 返回值位号: 7, 描述: 1：3轴有清零信号
        /// 返回值位号: 8, 描述: 1：0轴位置已锁存
        /// 返回值位号: 9, 描述: 1：1轴位置已锁存
        /// 返回值位号: 10, 描述: 1：2轴位置已锁存
        /// 返回值位号: 11, 描述: 1：3轴位置已锁存
        /// 返回值位号: 12, 描述: 1：指定卡的0轴位置已清零
        /// 返回值位号: 13, 描述: 1：1轴位置已清零
        /// 返回值位号: 14, 描述: 1：2轴位置已清零
        /// 返回值位号: 15, 描述: 1：3轴位置已清零
        /// 返回值位号: 16~31, 描述:保留
        /// </returns>
        public int GetLatchFlag()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_latch_flag(TargetCard);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //读取指定控制卡的锁存器的标志位
        /// <summary>
        /// 读取指定控制卡的锁存器的标志位
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <returns>返回值位号: 0, 描述: 0：指定卡的0轴有触发信号
        /// 返回值位号: 1, 描述: 0：1轴有触发信号
        /// 返回值位号: 2, 描述: 0：2轴有触发信号
        /// 返回值位号: 3, 描述: 0：3轴有触发信号
        /// 返回值位号: 4, 描述: 1：指定卡的0轴有清零信号
        /// 返回值位号: 5, 描述: 1：1轴有清零信号
        /// 返回值位号: 6, 描述: 1：2轴有清零信号
        /// 返回值位号: 7, 描述: 1：3轴有清零信号
        /// 返回值位号: 8, 描述: 1：0轴位置已锁存
        /// 返回值位号: 9, 描述: 1：1轴位置已锁存
        /// 返回值位号: 10, 描述: 1：2轴位置已锁存
        /// 返回值位号: 11, 描述: 1：3轴位置已锁存
        /// 返回值位号: 12, 描述: 1：指定卡的0轴位置已清零
        /// 返回值位号: 13, 描述: 1：1轴位置已清零
        /// 返回值位号: 14, 描述: 1：2轴位置已清零
        /// 返回值位号: 15, 描述: 1：3轴位置已清零
        /// 返回值位号: 16~31, 描述:保留
        /// </returns>
        public LatchFlag GetLatchFlagNew(ushort TargetCardNumber)
            {
            LatchFlag TempLatchFlag;
            TempLatchFlag.Bit0 = false;
            TempLatchFlag.Bit1 = false;
            TempLatchFlag.Bit2 = false;
            TempLatchFlag.Bit3 = false;
            TempLatchFlag.Bit4 = false;
            TempLatchFlag.Bit5 = false;
            TempLatchFlag.Bit6 = false;
            TempLatchFlag.Bit7 = false;
            TempLatchFlag.Bit8 = false;
            TempLatchFlag.Bit9 = false;
            TempLatchFlag.Bit10 = false;
            TempLatchFlag.Bit11 = false;
            TempLatchFlag.Bit12 = false;
            TempLatchFlag.Bit13 = false;
            TempLatchFlag.Bit14 = false;
            TempLatchFlag.Bit15 = false;

            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return TempLatchFlag;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return TempLatchFlag;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                int TempReturn = 0;
                TempReturn = (int)d2410_get_latch_flag(TargetCardNumber);

                //Bit0
                if ((TempReturn & 1) == 1)
                    {
                    TempLatchFlag.Bit0 = true;
                    }

                //Bit1
                if (((TempReturn >> 1) & 1) == 1)
                    {
                    TempLatchFlag.Bit1 = true;
                    }

                //Bit2
                if (((TempReturn >> 2) & 1) == 1)
                    {
                    TempLatchFlag.Bit2 = true;
                    }
 
                //Bit3
                if (((TempReturn >> 3) & 1) == 1)
                    {
                    TempLatchFlag.Bit3 = true;
                    }

                //Bit4
                if (((TempReturn >> 4) & 1) == 1)
                    {
                    TempLatchFlag.Bit4 = true;
                    }

                //Bit5
                if (((TempReturn >> 5) & 1) == 1)
                    {
                    TempLatchFlag.Bit5 = true;
                    }

                //Bit6
                if (((TempReturn >> 6) & 1) == 1)
                    {
                    TempLatchFlag.Bit6 = true;
                    }

                //Bit7
                if (((TempReturn >> 7) & 1) == 1)
                    {
                    TempLatchFlag.Bit7 = true;
                    }

                //Bit8
                if (((TempReturn >> 8) & 1) == 1)
                    {
                    TempLatchFlag.Bit8 = true;
                    }

                //Bit9
                if (((TempReturn >> 9) & 1) == 1)
                    {
                    TempLatchFlag.Bit9 = true;
                    }

                //Bit10
                if (((TempReturn >> 10) & 1) == 1)
                    {
                    TempLatchFlag.Bit10 = true;
                    }

                //Bit11
                if (((TempReturn >> 11) & 1) == 1)
                    {
                    TempLatchFlag.Bit11 = true;
                    }

                //Bit12
                if (((TempReturn >> 12) & 1) == 1)
                    {
                    TempLatchFlag.Bit12 = true;
                    }

                //Bit13
                if (((TempReturn >> 13) & 1) == 1)
                    {
                    TempLatchFlag.Bit13 = true;
                    }

                //Bit14
                if (((TempReturn >> 14) & 1) == 1)
                    {
                    TempLatchFlag.Bit14 = true;
                    }

                //Bit15
                if (((TempReturn >> 15) & 1) == 1)
                    {
                    TempLatchFlag.Bit15 = true;
                    }

                //Bit15
                if (((TempReturn >> 15) & 1) == 1)
                    {
                    TempLatchFlag.Bit15 = true;
                    }

                return TempLatchFlag;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return TempLatchFlag;
                }
            }

        //读取当前控制卡的锁存器的标志位
        /// <summary>
        /// 读取当前控制卡的锁存器的标志位
        /// </summary>
        /// <returns>返回值位号: 0, 描述: 0：指定卡的0轴有触发信号
        /// 返回值位号: 1, 描述: 0：1轴有触发信号
        /// 返回值位号: 2, 描述: 0：2轴有触发信号
        /// 返回值位号: 3, 描述: 0：3轴有触发信号
        /// 返回值位号: 4, 描述: 1：指定卡的0轴有清零信号
        /// 返回值位号: 5, 描述: 1：1轴有清零信号
        /// 返回值位号: 6, 描述: 1：2轴有清零信号
        /// 返回值位号: 7, 描述: 1：3轴有清零信号
        /// 返回值位号: 8, 描述: 1：0轴位置已锁存
        /// 返回值位号: 9, 描述: 1：1轴位置已锁存
        /// 返回值位号: 10, 描述: 1：2轴位置已锁存
        /// 返回值位号: 11, 描述: 1：3轴位置已锁存
        /// 返回值位号: 12, 描述: 1：指定卡的0轴位置已清零
        /// 返回值位号: 13, 描述: 1：1轴位置已清零
        /// 返回值位号: 14, 描述: 1：2轴位置已清零
        /// 返回值位号: 15, 描述: 1：3轴位置已清零
        /// 返回值位号: 16~31, 描述:保留
        /// </returns>
        public LatchFlag GetLatchFlagNew()
            {
            LatchFlag TempLatchFlag;
            TempLatchFlag.Bit0 = false;
            TempLatchFlag.Bit1 = false;
            TempLatchFlag.Bit2 = false;
            TempLatchFlag.Bit3 = false;
            TempLatchFlag.Bit4 = false;
            TempLatchFlag.Bit5 = false;
            TempLatchFlag.Bit6 = false;
            TempLatchFlag.Bit7 = false;
            TempLatchFlag.Bit8 = false;
            TempLatchFlag.Bit9 = false;
            TempLatchFlag.Bit10 = false;
            TempLatchFlag.Bit11 = false;
            TempLatchFlag.Bit12 = false;
            TempLatchFlag.Bit13 = false;
            TempLatchFlag.Bit14 = false;
            TempLatchFlag.Bit15 = false;

            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return TempLatchFlag;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_latch_flag(TargetCard);

                //Bit0
                if ((TempReturn & 1) == 1)
                    {
                    TempLatchFlag.Bit0 = true;
                    }

                //Bit1
                if (((TempReturn >> 1) & 1) == 1)
                    {
                    TempLatchFlag.Bit1 = true;
                    }

                //Bit2
                if (((TempReturn >> 2) & 1) == 1)
                    {
                    TempLatchFlag.Bit2 = true;
                    }

                //Bit3
                if (((TempReturn >> 3) & 1) == 1)
                    {
                    TempLatchFlag.Bit3 = true;
                    }

                //Bit4
                if (((TempReturn >> 4) & 1) == 1)
                    {
                    TempLatchFlag.Bit4 = true;
                    }

                //Bit5
                if (((TempReturn >> 5) & 1) == 1)
                    {
                    TempLatchFlag.Bit5 = true;
                    }

                //Bit6
                if (((TempReturn >> 6) & 1) == 1)
                    {
                    TempLatchFlag.Bit6 = true;
                    }

                //Bit7
                if (((TempReturn >> 7) & 1) == 1)
                    {
                    TempLatchFlag.Bit7 = true;
                    }

                //Bit8
                if (((TempReturn >> 8) & 1) == 1)
                    {
                    TempLatchFlag.Bit8 = true;
                    }

                //Bit9
                if (((TempReturn >> 9) & 1) == 1)
                    {
                    TempLatchFlag.Bit9 = true;
                    }

                //Bit10
                if (((TempReturn >> 10) & 1) == 1)
                    {
                    TempLatchFlag.Bit10 = true;
                    }

                //Bit11
                if (((TempReturn >> 11) & 1) == 1)
                    {
                    TempLatchFlag.Bit11 = true;
                    }

                //Bit12
                if (((TempReturn >> 12) & 1) == 1)
                    {
                    TempLatchFlag.Bit12 = true;
                    }

                //Bit13
                if (((TempReturn >> 13) & 1) == 1)
                    {
                    TempLatchFlag.Bit13 = true;
                    }

                //Bit14
                if (((TempReturn >> 14) & 1) == 1)
                    {
                    TempLatchFlag.Bit14 = true;
                    }

                //Bit15
                if (((TempReturn >> 15) & 1) == 1)
                    {
                    TempLatchFlag.Bit15 = true;
                    }

                //Bit15
                if (((TempReturn >> 15) & 1) == 1)
                    {
                    TempLatchFlag.Bit15 = true;
                    }

                return TempLatchFlag;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return TempLatchFlag;
                }
            }

        //复位指定控制卡的锁存器的标志位
        /// <summary>
        /// 复位指定控制卡的锁存器的标志位
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <returns>错误代码</returns>
        public int ResetLatchFlag(ushort TargetCardNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                int TempReturn = 0;
                TempReturn = (int)d2410_reset_latch_flag(TargetCardNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //复位当前控制卡的锁存器的标志位
        /// <summary>
        /// 复位当前控制卡的锁存器的标志位
        /// </summary>
        /// <returns>错误代码</returns>
        public int ResetLatchFlag()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_reset_latch_flag(TargetCard);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置指定控制卡的锁存方式为单轴锁存或是四轴同时锁存
        /// <summary>
        /// 设置指定控制卡的锁存方式为单轴锁存或是四轴同时锁存
        /// 注 意：位置锁存暂时只支持反馈位置锁存
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <param name="EnableForAllAxis">锁存方式 ：0：单独锁存，1：四轴同时锁存</param>
        /// <returns>错误代码</returns>
        public int SetLatchMode(ushort TargetCardNumber, ushort EnableForAllAxis)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                if (EnableForAllAxis != 0) 
                    {
                    EnableForAllAxis = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_config_latch_mode(TargetCardNumber, 
                    EnableForAllAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置当前控制卡的锁存方式为单轴锁存或是四轴同时锁存
        /// <summary>
        /// 设置当前控制卡的锁存方式为单轴锁存或是四轴同时锁存
        /// 注 意：位置锁存暂时只支持反馈位置锁存
        /// </summary>
        /// <param name="EnableForAllAxis">锁存方式 ：0：单独锁存，1：四轴同时锁存</param>
        /// <returns>错误代码</returns>
        public int SetLatchMode(ushort EnableForAllAxis)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }
                
                if (EnableForAllAxis != 0)
                    {
                    EnableForAllAxis = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_config_latch_mode(TargetCard,
                    EnableForAllAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置指定控制卡全部锁存时的外触发信号通道；可以由二个信号通道输入，即LTC1, LTC2；默认为LTC1
        /// <summary>
        /// 设置指定控制卡全部锁存时的外触发信号通道；
        /// 可以由二个信号通道输入，即LTC1, LTC2；默认为LTC1
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <param name="ChannelNumber">信号通道选择号：0：LTC1锁存四个轴，1：LTC2锁存四个轴</param>
        /// <returns>错误代码</returns>
        public int SetLTCTriggerChannel(ushort TargetCardNumber,
            ushort ChannelNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                if (ChannelNumber != 0)
                    {
                    ChannelNumber = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_triger_chunnel(TargetCardNumber,
                    ChannelNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置当前控制卡全部锁存时的外触发信号通道；可以由二个信号通道输入，即LTC1, LTC2；默认为LTC1
        /// <summary>
        /// 设置当前控制卡全部锁存时的外触发信号通道；
        /// 可以由二个信号通道输入，即LTC1, LTC2；默认为LTC1
        /// </summary>
        /// <param name="ChannelNumber">信号通道选择号：0：LTC1锁存四个轴，1：LTC2锁存四个轴</param>
        /// <returns>错误代码</returns>
        public int SetLTCTriggerChannel(ushort ChannelNumber)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (ChannelNumber != 0)
                    {
                    ChannelNumber = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_triger_chunnel(TargetCard,
                    ChannelNumber);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置指定控制卡编码器Speaker和LED的输出逻辑，默认为低有效
        /// <summary>
        /// 设置指定控制卡编码器Speaker和LED的输出逻辑，默认为低有效
        /// </summary>
        /// <param name="TargetCardNumber">指定控制卡号, 范围（1~N，N为卡号）</param>
        /// <param name="OutputSignalLogic">0：低有效，1：高有效</param>
        /// <returns>错误代码</returns>
        public int SetSpeakerLEDOutputLogic(ushort TargetCardNumber,
            ushort OutputSignalLogic)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetCardNumber > AvailableCardQty)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                if (OutputSignalLogic != 0)
                    {
                    OutputSignalLogic = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_speaker_logic(TargetCardNumber,
                    OutputSignalLogic);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置当前控制卡编码器Speaker和LED的输出逻辑，默认为低有效
        /// <summary>
        /// 设置当前控制卡编码器Speaker和LED的输出逻辑，默认为低有效
        /// </summary>
        /// <param name="OutputSignalLogic">0：低有效，1：高有效</param>
        /// <returns>错误代码</returns>
        public int SetSpeakerLEDOutputLogic(ushort OutputSignalLogic)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (OutputSignalLogic != 0)
                    {
                    OutputSignalLogic = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_speaker_logic(TargetCard,
                    OutputSignalLogic);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
        
        #endregion

        #region "脉冲当量设置"

        //脉冲当量为每个脉冲对应于平台的位移量
        //“脉冲当量”参数设置用于设置绘图框中每毫米代表的脉冲数


        /// <summary>
        /// 读取脉冲当量设置值【移动1毫米所需要的脉冲数量】
        /// </summary>
        /// <param name="PulseQtyPerMM">移动1毫米所需要的脉冲数量</param>
        /// <returns>错误代码</returns>
        public int GetPulsePerMM(ref double PulseQtyPerMM)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_get_equiv(TargetAxis, ref PulseQtyPerMM);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //设置脉冲当量【移动1毫米所需要的脉冲数量】
        /// <summary>
        /// 设置脉冲当量【移动1毫米所需要的脉冲数量】
        /// </summary>
        /// <param name="PulseQtyPerMM">移动1毫米所需要的脉冲数量</param>
        /// <returns>错误代码</returns>
        public int SetPulsePerMM(double PulseQtyPerMM)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_equiv(TargetAxis, PulseQtyPerMM);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }












        #endregion
        
        #region "手轮运动"

        //手轮运动功能的实现
        //DMC2410C运动控制卡支持单轴手轮运动功能。该功能允许用户设置一个手轮通道对应一个运动轴进行运动。

        //手轮运动功能相关函数说明
        //d2410_set_handwheel_inmode       '设置输入手轮脉冲信号的计数方式
        //d2410_handwheel_move             '启动指定轴的手轮脉冲运动

        //注 意：当启动手轮运动后，只有发送d2410_decel_stop或d2410_imd_stop命令后才会退出手轮模式
        //例7.14：手轮运动 
        //Dim Myaxis,Myinmode As Integer 
        //Dim Mymulti As Double
        //Myaxis = 0                                             '设置运动轴为0号轴 
        //Myinmode = 0                                           '设置手轮输入方式为AB相 
        //Mymulti = 10                                           '设置手轮输入倍率为10 
        //d2410_set_handwheel_inmode Myaxis,Myinmode, Mymulti    '设置手轮运动 
        //d2410_handwheel_move Myaxis                            '启动手轮运动 
        //d2410_imd_stop Myaxis                                  '立即停止手轮运动


        /// <summary>
        /// 设置输入手轮脉冲信号的计数方式
        /// </summary>
        /// <param name="ModeOfInputSignal">表示输入方式：
        /// 0：A、B相位正交计数，
        /// 1：双脉冲信号</param>
        /// <param name="Multiply">计数器的计数方向及倍率设置：
        /// 设置手轮的倍率, 正数表示默认方向，负数表示与默认方向相反</param>
        /// <returns>错误代码</returns>
        public int SetHandWheelMode(ushort ModeOfInputSignal,
            double Multiply)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (ModeOfInputSignal != 0) 
                    {
                    ModeOfInputSignal = 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_set_handwheel_inmode(TargetAxis, 
                    ModeOfInputSignal, Multiply);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //启动轴的手轮脉冲运动
        /// <summary>
        /// 启动轴的手轮脉冲运动
        /// </summary>
        /// <returns>错误代码</returns>
        public int HandWheelMove()
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                int TempReturn = 0;
                TempReturn = (int)d2410_handwheel_move(TargetAxis);

                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //退出手轮模式
        /// <summary>
        /// 退出手轮模式
        /// </summary>
        /// <param name="StopMode">手轮运动停止方式：
        /// 0：减速停止，
        /// 1：立即停止</param>
        /// <param name="DecelTime">停止时的减速时间</param>
        /// <returns>错误代码</returns>
        public int HandWheelStop(ushort StopMode, double DecelTime)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (StopMode != 0) 
                    {
                    StopMode = 1;
                    }
                //注意: 当启动手轮运动后, 只有发送d2410_decel_stop或d2410_imd_stop命令后才会退出手轮模式
                int TempReturn = 0;
                if (StopMode == 0)
                    {
                    TempReturn = (int)d2410_decel_stop(TargetAxis, DecelTime);
                    }
                else 
                    {
                    TempReturn = (int)d2410_imd_stop(TargetAxis);
                    }
                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
        
        #endregion

        //MC2410C运动控制卡在描述运动轨迹时可以用绝对坐标也可以用相对坐标。
        //两种模式各有优点，
        //如：在绝对坐标模式中用一系列坐标点定义一条曲线，如果要修改中间某点坐标时，不会影响后续点的坐标；
        //在相对坐标模式中，用一系列坐标点定义一条曲线，用循环命令可以重复这条曲线轨迹多次。

        //*************************
        //多轴联动
        //几个轴同时运动，称为多轴联动。
        //DMC2410C运动控制卡单张卡可以控制4个轴以多种方式运动，常用的有：多轴联动、直线插补、圆弧插补。
        //DMC2410C控制卡可以控制多个电机同时执行d2410_t_move、d2410_s_move这类单轴运动函数。
        //所谓同时执行，是在程序中顺序调用d2410_t_move、d2410_s_move等函数，因为程序执行速度很快，
        //在瞬间几个电机都开始运动，给人的感觉就是同时开始运动。 
        //多轴联动在各轴速度设置不当时，各轴停止时间不同、在起点与终点之间运动的轨迹也不是直线
        //*************************

        #region "线性插补"

        //直线插补运动
        //DMC2410C卡可以进行任意2轴、3轴和4轴直线插补，插补工作由控制卡的硬件执行，
        //用户只需将插补运动的速度、加速度、终点位置等参数写入相关函数，无需介入插补过程中的计算工作。

        //直线插补运动相关函数说明
        //d2410_t_line2                  '让指定的两轴作对称的梯形加减速插补运动
        //d2410_t_line3                  '让指定的三轴作对称的梯形加减速插补运动
        //d2410_t_line4                  '指定四轴以对称的梯形速度曲线做插补运动
        //'d2410_set_vector_profile       '设定插补矢量运动曲线的最大速度、加速时间、减速时间

        //例程7.6：XY轴直线插补 
        //Dim AxisArray(2) as Integer AxisArray(0)=0     '定义插补0轴为X轴 
        //AxisArray(1)=1                                 '定义插补1轴为Y轴 
        //d2410_set_vector_profile 0,5000,0.1,0.2 
        //d2410_t_line2 AxisArray(0),30000,AxisArray(1),40000,0
        //该例程使X，Y轴进行相对模式直线插补运动，
        //其相关参数为： 
        //ΔX = 30000 pulse 
        //ΔY = 40000 pulse 
        //最大矢量速度 = 5000 p/s （0轴,1轴分速度为3000，4000 p/s） 
        //梯形加速时间 = 0.1 s 
        //梯形减速时间 = 0.2 s
        
        /// <summary>
        /// 两轴直线插补: 指定任意两轴以对称的梯形速度曲线做线性插补运动
        /// </summary>
        /// <param name="NoOfAxis1">指定两轴插补的第一轴【轴号：从1~N】</param>
        /// <param name="DistanceOfAxis1">指定第一轴的位移值，单位：pulse</param>
        /// <param name="NoOfAxis2">指定两轴插补的第二轴【轴号：从1~N】</param>
        /// <param name="DistanceOfAxis2">指定第二轴的位移值，单位：pulse</param>
        /// <param name="PositionMode">位移模式设定：0：相对位移，1：绝对位移</param>
        /// <param name="MinVelocity">最小速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="AcclTime">总加速时间，单位：s</param>
        /// <param name="DeclTime">总减速时间，单位：s</param>
        /// <returns>错误代码</returns>
        public int TwoAxisLinearMove(ushort NoOfAxis1, int DistanceOfAxis1,
            ushort NoOfAxis2, int DistanceOfAxis2, ushort PositionMode,
            ushort MinVelocity, ushort MaxVelocity, double AcclTime, 
            double DeclTime)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (NoOfAxis1 == NoOfAxis2) 
                    {
                    MessageBox.Show("The parameter 'NoOfAxis1' and 'NoOfAxis2' can't be set as the same axis, please revise it.");
                    return 1;
                    }

                if (NoOfAxis1 < 1 || NoOfAxis1 > (AvailableCardQty * 4)
                    || NoOfAxis2 < 1 || NoOfAxis2 > (AvailableCardQty * 4)) 
                    {
                    MessageBox.Show("The value for target axis should be : 1~" 
                        + (AvailableCardQty * 4) + " ,please revise the parameter.");
                    return 1;
                    }

                NoOfAxis1 = (ushort)(NoOfAxis1 - 1);
                NoOfAxis2 = (ushort)(NoOfAxis2 - 1);

                if (PositionMode != 0) 
                    {
                    PositionMode = 1;
                    }
                
                //设定插补矢量运动曲线的S段参数、运行速度、加速时间、减速时间
                //<param name="Min_Vel">保留参数</param>
                //<param name="Max_Vel">最大速度，单位：pulse/s</param>
                //<param name="Tacc">总加速时间，单位：s</param>
                //<param name="Tdec">总减速时间，单位：s</param>
                //<returns>错误代码</returns>
                //Declare Function d2410_set_vector_profile Lib "DMC2410.dll" 
                //(ByVal Min_Vel As Double, ByVal Max_Vel As Double, 
                //ByVal Tacc As Double, ByVal Tdec As Double) As Int32

                int TempReturn = 0;
                TempReturn = (int)d2410_set_vector_profile(MinVelocity, MaxVelocity, 
                    AcclTime, DeclTime);
                TempReturn += (int)d2410_t_line2(NoOfAxis1, DistanceOfAxis1,
                    NoOfAxis2, DistanceOfAxis2, PositionMode);
                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //三轴直线插补: 指定任意三轴以对称的梯形速度曲线做插补运动
        /// <summary>
        /// 三轴直线插补: 指定任意三轴以对称的梯形速度曲线做插补运动
        /// </summary>
        /// <param name="AxisArray">轴号列表的指针</param>
        /// <param name="DistanceOfAxis1">指定axis[0]轴的位移值，单位：pulse</param>
        /// <param name="DistanceOfAxis2">指定axis[1]轴的位移值，单位：pulse</param>
        /// <param name="DistanceOfAxis3">指定axis[2]轴的位移值，单位：pulse</param>
        /// <param name="PositionMode">位移模式设定：0：相对位移，1：绝对位移</param>
        /// <param name="MinVelocity">最小速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="AcclTime">总加速时间，单位：s</param>
        /// <param name="DeclTime">总减速时间，单位：s</param>
        /// <returns></returns>
        public int ThreeAxisLinearMove(ref ushort[] AxisArray, int DistanceOfAxis1,
            int DistanceOfAxis2, int DistanceOfAxis3, ushort PositionMode,
            ushort MinVelocity, ushort MaxVelocity, double AcclTime, double DeclTime)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (PositionMode != 0) 
                    {
                    PositionMode = 1;
                    }

                if (AxisArray.Length < 3) 
                    {
                    MessageBox.Show("The length of parameter 'AxisArray' doesn't equal to 3, please revise it.");
                    return 1;
                    }

                if (AxisArray[0] == AxisArray[1]
                    || AxisArray[0] == AxisArray[2]
                    || AxisArray[1] == AxisArray[2])
                    {
                    MessageBox.Show("The three axises of parameter 'AxisArray' can't be set as the same axis, please revise it.");
                    return 1;
                    }

                for (int a = 0; a < AxisArray.Length; a++) 
                    {
                    if (AxisArray[a] < 1 || AxisArray[a] > (AvailableCardQty * 4))
                        {
                        MessageBox.Show("The value for target axis should be : 1~"
                            + (AvailableCardQty * 4) + " ,please revise the parameter.");
                        return 1;
                        }
                    else 
                        {
                        AxisArray[a] = (ushort)(AxisArray[a] - 1);
                        }
                    }
                
                //设定插补矢量运动曲线的S段参数、运行速度、加速时间、减速时间
                //<param name="Min_Vel">保留参数</param>
                //<param name="Max_Vel">最大速度，单位：pulse/s</param>
                //<param name="Tacc">总加速时间，单位：s</param>
                //<param name="Tdec">总减速时间，单位：s</param>
                //<returns>错误代码</returns>
                //Declare Function d2410_set_vector_profile Lib "DMC2410.dll" 
                //(ByVal Min_Vel As Double, ByVal Max_Vel As Double, 
                //ByVal Tacc As Double, ByVal Tdec As Double) As Int32

                int TempReturn = 0;
                TempReturn = (int)d2410_set_vector_profile(MinVelocity, 
                    MaxVelocity, AcclTime, DeclTime);
                TempReturn = (int)d2410_t_line3(AxisArray, DistanceOfAxis1, 
                    DistanceOfAxis2, DistanceOfAxis3, PositionMode);
                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //四轴直线插补: 指定控制卡的四轴以对称的梯形速度曲线做插补运动
        /// <summary>
        /// 四轴直线插补: 指定控制卡的四轴以对称的梯形速度曲线做插补运动
        /// </summary>
        /// <param name="TargetCardNumber">指定插补运动的板卡号, 范围（1~N，N为卡号）</param>
        /// <param name="DistanceOfAxis1">指定第一轴的位移值，单位：pulse</param>
        /// <param name="DistanceOfAxis2">指定第二轴的位移值，单位：pulse</param>
        /// <param name="DistanceOfAxis3">指定第三轴的位移值，单位：pulse</param>
        /// <param name="DistanceOfAxis4">指定第四轴的位移值，单位：pulse</param>
        /// <param name="PositionMode">位移模式设定：0：相对位移，1：绝对位移</param>
        /// <param name="MinVelocity">最小速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="AcclTime">总加速时间，单位：s</param>
        /// <param name="DeclTime">总减速时间，单位：s</param>
        /// <returns>错误代码</returns>
        public int FourAxisLinearMove(ushort TargetCardNumber, int DistanceOfAxis1,
            int DistanceOfAxis2, int DistanceOfAxis3, int DistanceOfAxis4, ushort PositionMode,
            ushort MinVelocity, ushort MaxVelocity, double AcclTime, double DeclTime)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (TargetCardNumber > AvailableCardQty
                    || TargetCardNumber < 1)
                    {
                    MessageBox.Show("There is(are) " + AvailableCardQty + " card(s) available in the PC system, please revise the parameter and retry.\r\n"
                        + "PC系统中有 " + AvailableCardQty + "张运动控制卡，请修改参数小于等于此数值。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 1;
                    }

                TargetCardNumber = (ushort)(TargetCardNumber - 1);

                if (PositionMode != 0) 
                    {
                    PositionMode = 1;
                    }
                
                //设定插补矢量运动曲线的S段参数、运行速度、加速时间、减速时间
                //<param name="Min_Vel">保留参数</param>
                //<param name="Max_Vel">最大速度，单位：pulse/s</param>
                //<param name="Tacc">总加速时间，单位：s</param>
                //<param name="Tdec">总减速时间，单位：s</param>
                //<returns>错误代码</returns>
                //Declare Function d2410_set_vector_profile Lib "DMC2410.dll" 
                //(ByVal Min_Vel As Double, ByVal Max_Vel As Double,
                //ByVal Tacc As Double, ByVal Tdec As Double) As Int32

                int TempReturn = 0;
                TempReturn = (int)d2410_set_vector_profile(MinVelocity, MaxVelocity, 
                    AcclTime, DeclTime);
                TempReturn += (int)d2410_t_line4(TargetCardNumber, DistanceOfAxis1,
                    DistanceOfAxis2, DistanceOfAxis3, DistanceOfAxis4, PositionMode);
                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
        
        #endregion


        #region "圆弧插补"

        //圆弧插补运动
        //DMC2410C卡的任意两轴之间可以进行圆弧插补，
        //圆弧插补分为相对位置圆弧插补和绝对位置圆弧插补，
        //运动的方向分为顺时针（CW）和逆时针（CCW）。

        //圆弧插补相关函数说明
        //d2410_arc_move             '让指定的二轴作绝对位置圆弧插补运动。
        //d2410_rel_arc_move         '让指定的二轴作相对位置圆弧插补运动。

        //例程7.7：XY轴圆弧插补 
        //Dim AxisArray(2) As Integer 
        //Dim Pos(2), Cen(2) As Long 
        //AxisArray(0)=0                                  '定义0轴为插补X轴
        //AxisArray(1)=1                                  '定义1轴为插补Y轴 
        //Pos(0) = 5000 Pos(1) = -5000                    '设置终点坐标 
        //Cen(0) = 5000 Cen(1) = 0                        '设置圆心坐标 
        //'d2410_set_vector_profile 1000,3000,0.1,0.2      '设置矢量速度曲线 
        //d2410_arc_move AxisArray(0), Pos(0), Cen(0), 0  'XY轴进行顺时针方向绝对圆弧插补运动， 
        //                                                '终点（5000, -5000），圆心（5000, 0）
        
        /// <summary>
        /// 两轴圆弧绝对位置插补【单位：pulse】：指定任意的两轴以当前位置为起点，
        /// 按指定的圆心、目标绝对位置和方向作圆弧插补运动
        /// </summary>
        /// <param name="AxisArray">轴号列表指针</param>
        /// <param name="TargetABSPos">目标绝对位置列表指针，单位：pulse</param>
        /// <param name="CenterABSPos">圆心绝对位置列表指针，单位：pulse</param>
        /// <param name="ArcDirection">圆弧方向：0：顺时针，1：逆时针</param>
        /// <param name="MinVelocity">最小速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="AcclTime">总加速时间，单位：s</param>
        /// <param name="DeclTime">总减速时间，单位：s</param>
        /// <returns>错误代码</returns>
        public int TwoAxisArcABSMove(ref ushort[] AxisArray, ref int[] TargetABSPos,
            ref int[] CenterABSPos, ushort ArcDirection, ushort MinVelocity,
            ushort MaxVelocity, double AcclTime, double DeclTime)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (ArcDirection != 0)
                    {
                    ArcDirection = 1;
                    }

                if (AxisArray.Length < 2
                    || TargetABSPos.Length < 2
                    || CenterABSPos.Length < 2)
                    {
                    MessageBox.Show("The length of parameter 'AxisArray'/'TargetABSPos'/'CenterABSPos' doesn't equal to 2, please revise it.");
                    return 1;
                    }

                if (AxisArray[0] == AxisArray[1])
                    {
                    MessageBox.Show("The two axises of parameter 'AxisArray' can't be set as the same axis, please revise it.");
                    return 1;
                    }

                for (int a = 0; a < AxisArray.Length; a++)
                    {
                    if (AxisArray[a] < 1 || AxisArray[a] > (AvailableCardQty * 4))
                        {
                        MessageBox.Show("The value for target axis should be : 1~"
                            + (AvailableCardQty * 4) + " ,please revise the parameter.");
                        return 1;
                        }
                    else
                        {
                        AxisArray[a] = (ushort)(AxisArray[a] - 1);
                        }
                    }
                
                //设定插补矢量运动曲线的S段参数、运行速度、加速时间、减速时间
                //<param name="Min_Vel">保留参数</param>
                //<param name="Max_Vel">最大速度，单位：pulse/s</param>
                //<param name="Tacc">总加速时间，单位：s</param>
                //<param name="Tdec">总减速时间，单位：s</param>
                //<returns>错误代码</returns>
                //Declare Function d2410_set_vector_profile Lib "DMC2410.dll" 
                //(ByVal Min_Vel As Double, ByVal Max_Vel As Double,
                //ByVal Tacc As Double, ByVal Tdec As Double) As Int32
                
                int TempReturn = 0;
                TempReturn = (int)d2410_set_vector_profile(MinVelocity, 
                    MaxVelocity, AcclTime, DeclTime);
                TempReturn += (int)d2410_arc_move(AxisArray, TargetABSPos,
                    CenterABSPos, ArcDirection);
                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //两轴圆弧绝对位置插补【单位：mm】
        /// <summary>
        /// 两轴圆弧绝对位置插补【单位：mm】：指定任意的两轴以当前位置为起点，
        /// 按指定的圆心、目标绝对位置和方向作圆弧插补运动
        /// </summary>
        /// <param name="AxisArray">轴号列表指针</param>
        /// <param name="TargetABSPos">目标绝对位置列表指针，单位：mm</param>
        /// <param name="CenterABSPos">圆心绝对位置列表指针，单位：mm</param>
        /// <param name="ArcDirection">圆弧方向：0：顺时针，1：逆时针</param>
        /// <param name="MinVelocity">最小速度，单位：mm/s</param>
        /// <param name="MaxVelocity">最大速度，单位：mm/s</param>
        /// <param name="AcclTime">总加速时间，单位：s</param>
        /// <param name="DeclTime">总减速时间，单位：s</param>
        /// <returns>错误代码</returns>
        public int TwoAxisArcABSMoveInMM(ref ushort[] AxisArray, ref double[] TargetABSPos,
            ref double[] CenterABSPos, ushort ArcDirection, ushort MinVelocity,
            ushort MaxVelocity, double AcclTime, double DeclTime)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (ArcDirection != 0)
                    {
                    ArcDirection = 1;
                    }

                if (AxisArray.Length < 2
                    || TargetABSPos.Length < 2
                    || CenterABSPos.Length < 2)
                    {
                    MessageBox.Show("The length of parameter 'AxisArray'/'TargetABSPos'/'CenterABSPos' doesn't equal to 2, please revise it.");
                    return 1;
                    }

                if (AxisArray[0] == AxisArray[1])
                    {
                    MessageBox.Show("The two axises of parameter 'AxisArray' can't be set as the same axis, please revise it.");
                    return 1;
                    }

                for (int a = 0; a < AxisArray.Length; a++)
                    {
                    if (AxisArray[a] < 1 || AxisArray[a] > (AvailableCardQty * 4))
                        {
                        MessageBox.Show("The value for target axis should be : 1~"
                            + (AvailableCardQty * 4) + " ,please revise the parameter.");
                        return 1;
                        }
                    else
                        {
                        AxisArray[a] = (ushort)(AxisArray[a] - 1);
                        }
                    }

                //设定插补矢量运动曲线的S段参数、运行速度、加速时间、减速时间
                //<param name="Min_Vel">保留参数</param>
                //<param name="Max_Vel">最大速度，单位：pulse/s</param>
                //<param name="Tacc">总加速时间，单位：s</param>
                //<param name="Tdec">总减速时间，单位：s</param>
                //<returns>错误代码</returns>
                //Declare Function d2410_set_vector_profile Lib "DMC2410.dll" 
                //(ByVal Min_Vel As Double, ByVal Max_Vel As Double,
                //ByVal Tacc As Double, ByVal Tdec As Double) As Int32

                int TempReturn = 0;
                TempReturn = (int)d2410_set_vector_profile(MinVelocity,
                    MaxVelocity, AcclTime, DeclTime);
                TempReturn += (int)d2410_arc_move_unitmm(AxisArray, TargetABSPos,
                    CenterABSPos, ArcDirection);
                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //两轴圆弧相对位置插补
        /// <summary>
        /// 两轴圆弧相对位置插补【单位：pulse】：指定任意的两轴以当前位置为起点，
        /// 按指定的圆心、目标绝对位置和方向作圆弧插补运动
        /// </summary>
        /// <param name="AxisArray">轴号列表指针</param>
        /// <param name="TargetRelativePos">目标相对位置列表指针，单位：pulse</param>
        /// <param name="CenterRelativePos">圆心相对位置列表指针，单位：pulse</param>
        /// <param name="ArcDirection">圆弧方向：0：顺时针，1：逆时针</param>
        /// <param name="MinVelocity">最小速度，单位：pulse/s</param>
        /// <param name="MaxVelocity">最大速度，单位：pulse/s</param>
        /// <param name="AcclTime">总加速时间，单位：s</param>
        /// <param name="DeclTime">总减速时间，单位：s</param>
        /// <returns>错误代码</returns>
        public int TwoAxisArcRelativeMove(ref ushort[] AxisArray, ref int[] TargetRelativePos,
            ref int[] CenterRelativePos, ushort ArcDirection, ushort MinVelocity,
            ushort MaxVelocity, double AcclTime, double DeclTime)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (ArcDirection != 0)
                    {
                    ArcDirection = 1;
                    }

                if (AxisArray.Length < 2
                    || TargetRelativePos.Length < 2
                    || CenterRelativePos.Length < 2)
                    {
                    MessageBox.Show("The length of parameter 'AxisArray'/'TargetABSPos'/'CenterABSPos' doesn't equal to 2, please revise it.");
                    return 1;
                    }

                if (AxisArray[0] == AxisArray[1])
                    {
                    MessageBox.Show("The two axises of parameter 'AxisArray' can't be set as the same axis, please revise it.");
                    return 1;
                    }

                for (int a = 0; a < AxisArray.Length; a++)
                    {
                    if (AxisArray[a] < 1 || AxisArray[a] > (AvailableCardQty * 4))
                        {
                        MessageBox.Show("The value for target axis should be : 1~"
                            + (AvailableCardQty * 4) + " ,please revise the parameter.");
                        return 1;
                        }
                    else
                        {
                        AxisArray[a] = (ushort)(AxisArray[a] - 1);
                        }
                    }

                //设定插补矢量运动曲线的S段参数、运行速度、加速时间、减速时间
                //<param name="Min_Vel">保留参数</param>
                //<param name="Max_Vel">最大速度，单位：pulse/s</param>
                //<param name="Tacc">总加速时间，单位：s</param>
                //<param name="Tdec">总减速时间，单位：s</param>
                //<returns>错误代码</returns>
                //Declare Function d2410_set_vector_profile Lib "DMC2410.dll" 
                //(ByVal Min_Vel As Double, ByVal Max_Vel As Double,
                //ByVal Tacc As Double, ByVal Tdec As Double) As Int32

                int TempReturn = 0;
                TempReturn = (int)d2410_set_vector_profile(MinVelocity,
                    MaxVelocity, AcclTime, DeclTime);
                TempReturn += (int)d2410_rel_arc_move(AxisArray, TargetRelativePos,
                    CenterRelativePos, ArcDirection);
                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        //两轴圆弧相对位置插补【单位：mm】
        /// <summary>
        /// 两轴圆弧相对位置插补【单位：mm】：指定任意的两轴以当前位置为起点，
        /// 按指定的圆心、目标绝对位置和方向作圆弧插补运动
        /// </summary>
        /// <param name="AxisArray">轴号列表指针</param>
        /// <param name="TargetRelativePos">目标相对位置列表指针，单位：mm</param>
        /// <param name="CenterRelativePos">圆心相对位置列表指针，单位：mm</param>
        /// <param name="ArcDirection">圆弧方向：0：顺时针，1：逆时针</param>
        /// <param name="MinVelocity">最小速度，单位：mm/s</param>
        /// <param name="MaxVelocity">最大速度，单位：mm/s</param>
        /// <param name="AcclTime">总加速时间，单位：s</param>
        /// <param name="DeclTime">总减速时间，单位：s</param>
        /// <returns>错误代码</returns>
        public int TwoAxisArcRelativeMoveInMM(ref ushort[] AxisArray, ref double[] TargetRelativePos,
            ref double[] CenterRelativePos, ushort ArcDirection, ushort MinVelocity,
            ushort MaxVelocity, double AcclTime, double DeclTime)
            {
            try
                {
                if (SuccessBuiltNew == false)
                    {
                    MessageBox.Show("Invalid operation, because you failed to initialize this class.\r\n"
                        + "无效操作，因为你在实例化此类时初始化失败，无法建立新的实例。"
                         , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                    }

                if (ArcDirection != 0)
                    {
                    ArcDirection = 1;
                    }

                if (AxisArray.Length < 2
                    || TargetRelativePos.Length < 2
                    || CenterRelativePos.Length < 2)
                    {
                    MessageBox.Show("The length of parameter 'AxisArray'/'TargetABSPos'/'CenterABSPos' doesn't equal to 2, please revise it.");
                    return 1;
                    }

                if (AxisArray[0] == AxisArray[1])
                    {
                    MessageBox.Show("The two axises of parameter 'AxisArray' can't be set as the same axis, please revise it.");
                    return 1;
                    }

                for (int a = 0; a < AxisArray.Length; a++)
                    {
                    if (AxisArray[a] < 1 || AxisArray[a] > (AvailableCardQty * 4))
                        {
                        MessageBox.Show("The value for target axis should be : 1~"
                            + (AvailableCardQty * 4) + " ,please revise the parameter.");
                        return 1;
                        }
                    else
                        {
                        AxisArray[a] = (ushort)(AxisArray[a] - 1);
                        }
                    }

                //设定插补矢量运动曲线的S段参数、运行速度、加速时间、减速时间
                //<param name="Min_Vel">保留参数</param>
                //<param name="Max_Vel">最大速度，单位：pulse/s</param>
                //<param name="Tacc">总加速时间，单位：s</param>
                //<param name="Tdec">总减速时间，单位：s</param>
                //<returns>错误代码</returns>
                //Declare Function d2410_set_vector_profile Lib "DMC2410.dll" 
                //(ByVal Min_Vel As Double, ByVal Max_Vel As Double,
                //ByVal Tacc As Double, ByVal Tdec As Double) As Int32

                int TempReturn = 0;
                TempReturn = (int)d2410_set_vector_profile(MinVelocity,
                    MaxVelocity, AcclTime, DeclTime);
                TempReturn += (int)d2410_rel_arc_move_unitmm(AxisArray, TargetRelativePos,
                    CenterRelativePos, ArcDirection);
                return TempReturn;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }

        #endregion

        }
    }