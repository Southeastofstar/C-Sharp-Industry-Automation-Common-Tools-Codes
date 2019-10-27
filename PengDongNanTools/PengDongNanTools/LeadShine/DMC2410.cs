using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
 
//雷赛运动卡DMC2410C#库函数调用
namespace MyTools
{
    //雷赛运动卡DMC2410C#库函数调用
    /// <summary>
    /// 雷赛运动卡DMC2410C#库函数调用
    /// </summary>
    public class DMC2410
    {
        //---------------------板卡初始和配置函数DMC2480 ----------------------
        /********************************************************************************
        ** 函数名称: d2410_board_init
        ** 功能描述: 控制板初始化，设置初始化和速度等设置
        ** 输　  入: 无
        ** 返 回 值: 0：无卡； 1-8：成功(实际卡数) 
        **           1001 + j: j号卡初始化出错 从1001开始。
        ** 修    改:  
        ** 修改日期: 
        *********************************************************************************/
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_board_init", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt16 d2410_board_init();

        /********************************************************************************
        ** 函数名称: d2410_board_close
        ** 功能描述: 关闭所有卡
        ** 输　  入: 无
        ** 返 回 值: 无
        ** 日    期: 
        *********************************************************************************/
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_board_close", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_board_close();

        /********************************************************************************
        ** 函数名称: 控制卡复位
        ** 功能描述: 复位所有卡，只能在初始化完成之后调用．
        ** 输　  入: 无
        ** 返 回 值: 无
        ** 日    期: 
        *********************************************************************************/
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_board_rest", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_board_rest();

        //脉冲输入输出配置
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_set_pulse_outmode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_set_pulse_outmode(UInt16 axis, UInt16 outmode);

        //专用信号设置函数        
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_ALM_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_config_ALM_PIN(UInt16 axis, UInt16 alm_logic, UInt16 alm_action);
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_ALM_PIN_Extern", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_config_ALM_PIN_Extern(UInt16 axis, UInt16 alm_enbale, UInt16 alm_logic, UInt16 alm_all, UInt16 alm_action);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_EL_MODE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_config_EL_MODE(UInt16 axis, UInt16 el_mode);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_set_HOME_pin_logic", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_set_HOME_pin_logic(UInt16 axis, UInt16 org_logic, UInt16 filter);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_write_SEVON_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_write_SEVON_PIN(UInt16 axis, UInt16 on_off);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_read_SEVON_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_read_SEVON_PIN(UInt16 axis);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_read_RDY_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_read_RDY_PIN(UInt16 axis);

        //通用输入/输出控制函数
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_read_inbit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_read_inbit(UInt16 cardno, UInt16 bitno);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_write_outbit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_write_outbit(UInt16 cardno, UInt16 bitno, UInt16 on_off);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_read_outbit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_read_outbit(UInt16 cardno, UInt16 bitno);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_read_inport", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_read_inport(UInt16 cardno);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_read_outport", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_read_outport(UInt16 cardno);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_write_outport", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_write_outport(UInt16 cardno, UInt32 port_value);

        //制动函数
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_decel_stop", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_decel_stop(UInt16 axis, double Tdec);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_imd_stop", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_imd_stop(UInt16 axis);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_emg_stop", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_emg_stop();

        //位置设置和读取函数
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_get_position", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_get_position(UInt16 axis);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_set_position", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_set_position(UInt16 axis, Int32 current_position);	

        //状态检测函数
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_check_done", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt16 d2410_check_done(UInt16 axis);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_axis_io_status", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt16 d2410_axis_io_status(UInt16 axis);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_get_rsts", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_get_rsts(UInt16 axis);

        //速度设置和读取函数              
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_set_vector_profile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_set_vector_profile(double Min_Vel, double Max_Vel, double Tacc, double Tdec);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_set_profile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_set_profile(UInt16 axis, double Min_Vel, double Max_Vel, double Tacc, double Tdec);
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_set_profile_Extern", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_set_profile_Extern(UInt16 axis, double Min_Vel, double Max_Vel, double Tacc, double Tdec,double Stop_Vel);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_set_s_profile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_set_s_profile(UInt16 axis, double Min_Vel, double Max_Vel, double Tacc, double Tdec, Int32 Sacc, Int32 Sdec);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_set_st_profile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_set_st_profile(UInt16 axis, double Min_Vel, double Max_Vel, double Tacc, double Tdec, double Tsacc, double Tsdec);
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_read_current_speed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern double d2410_read_current_speed(UInt16 axis); 
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_read_vector_speed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern double d2410_read_vector_speed(UInt16 card);

	    //在线变速/变位
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_change_speed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_change_speed(UInt16 axis, double Curr_Vel);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_reset_target_position", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_reset_target_position(UInt16 axis, Int32 dist);

        //单轴定长运动
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_t_pmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_t_pmove(UInt16 axis, Int32 Dist, UInt16 posi_mode);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_ex_t_pmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_ex_t_pmove(UInt16 axis, Int32 Dist, UInt16 posi_mode);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_s_pmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_s_pmove(UInt16 axis, Int32 Dist, UInt16 posi_mode);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_ex_s_pmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_ex_s_pmove(UInt16 axis, Int32 Dist, UInt16 posi_mode);

        //单轴连续运动
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_s_vmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_s_vmove(UInt16 axis, UInt16 dir);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_t_vmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_t_vmove(UInt16 axis, UInt16 dir);

        //直线插补
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_t_line2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_t_line2(UInt16 axis1, Int32 Dist1, UInt16 axis2, Int32 Dist2, UInt16 posi_mode);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_t_line3", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_t_line3(UInt16[] axis, Int32 Dist1, Int32 Dist2, Int32 Dist3, UInt16 posi_mode);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_t_line4", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_t_line4(UInt16 cardno, Int32 Dist1, Int32 Dist2, Int32 Dist3, Int32 Dist4, UInt16 posi_mode);

	    //圆弧插补
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_arc_move", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_arc_move(UInt16[] axis, Int32[] target_pos, Int32[] cen_pos, UInt16 arc_dir);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_rel_arc_move", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_rel_arc_move(UInt16[] axis, Int32[] rel_pos, Int32[] rel_cen, UInt16 arc_dir);

        //手轮运动
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_set_handwheel_inmode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_set_handwheel_inmode(UInt16 axis, UInt16 inmode, double multi); 
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_handwheel_move", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_handwheel_move(UInt16 axis);

        //找原点
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_home_mode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_config_home_mode(UInt16 axis, UInt16 mode, UInt16 EZ_count);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_home_move", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_home_move(UInt16 axis, UInt16 home_mode, UInt16 vel_mode);

	    //原点锁存
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_home_mode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_set_homelatch_mode(UInt16 axis,UInt16 enable,UInt16 logic);
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_home_mode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_get_homelatch_mode(UInt16 axis,ref UInt16 enable,ref UInt16 logic);
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_home_mode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_get_homelatch_flag(UInt16 axis);
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_home_mode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_reset_homelatch_flag(UInt16 axis);
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_home_mode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_get_homelatch_value(UInt16 axis);       
       
        //多组位置比较函数
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_compare_config_Extern", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32  d2410_compare_config_Extern(UInt16 card,UInt16 queue, UInt16 enable, UInt16 axis,  UInt16 cmp_source);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_compare_get_config_Extern", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32  d2410_compare_get_config_Extern(UInt16 card,UInt16 queue, ref UInt16 enable, ref UInt16 axis,  ref UInt16 cmp_source);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_compare_clear_points_Extern", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32  d2410_compare_clear_points_Extern(UInt16 card,UInt16 queue);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_compare_add_point_Extern", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32  d2410_compare_add_point_Extern(UInt16 card,UInt16 queue, UInt32  pos, UInt16 dir,  UInt16 action, UInt32  actpara);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_compare_get_current_point_Extern", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  d2410_compare_get_current_point_Extern(UInt16 card,UInt16 queue);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_compare_get_points_runned_Extern", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  d2410_compare_get_points_runned_Extern(UInt16 card,UInt16 queue);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_compare_get_points_remained_Extern", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  d2410_compare_get_points_remained_Extern(UInt16 card,UInt16 queue);

	    //高速位置比较
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_CMP_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32  d2410_config_CMP_PIN(UInt16 axis, UInt16 cmp_enable,Int32 cmp_pos,  UInt16 CMP_logic);
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_get_config_CMP_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32  d2410_get_config_CMP_PIN(UInt16 axis, ref UInt16 cmp_enable,ref Int32 cmp_pos,  ref UInt16 CMP_logic);
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_read_CMP_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  d2410_read_CMP_PIN(UInt16 axis);
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_write_CMP_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32  d2410_write_CMP_PIN(UInt16 axis,UInt16 on_off);
       
        //编码器计数功能
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_get_encoder", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_get_encoder(UInt16 axis);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_set_encoder", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_set_encoder(UInt16 axis, UInt32 encoder_value);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_EZ_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_config_EZ_PIN(UInt16 axis, UInt16 ez_logic, UInt16 ez_mode);
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_get_counter_flag", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_get_counter_flag(UInt16 cardno);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_reset_counter_flag", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_reset_counter_flag(UInt16 cardno);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_reset_clear_flag", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_reset_clear_flag(UInt16 cardno);

	    //高速锁存
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_LTC_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_config_LTC_PIN(UInt16 axis, UInt16 ltc_logic, UInt16 ltc_mode);
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_LTC_PIN_Extern", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_config_LTC_PIN_Extern(UInt16 axis, UInt16 ltc_logic, UInt16 ltc_mode,double ltc_filter);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_latch_mode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_config_latch_mode(UInt16 cardno, UInt16 all_enable);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_counter_config", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_counter_config(UInt16 axis, UInt16 mode);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_get_latch_value", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_get_latch_value(UInt16 axis);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_get_latch_flag", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_get_latch_flag(UInt16 cardno);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_reset_latch_flag", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_reset_latch_flag(UInt16 cardno);        
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_triger_chunnel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_triger_chunnel(UInt16 cardno, UInt16 num);

        [DllImport("Dmc2410.dll", EntryPoint = "d2410_set_speaker_logic", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_set_speaker_logic(UInt16 cardno, UInt16 logic);

	    //EMG设置
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_EMG_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_config_EMG_PIN(UInt16 cardno, UInt16 enable, UInt16 emg_logic);

	    //软件限位功能
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_config_softlimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_config_softlimit(UInt16 axis,UInt16 ON_OFF, UInt16 source_sel,UInt16 SL_action, Int32 N_limit,Int32 P_limit);
	    [DllImport("Dmc2410.dll", EntryPoint = "d2410_get_config_softlimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_get_config_softlimit(UInt16 axis,ref UInt16 ON_OFF, ref UInt16 source_sel,ref UInt16 SL_action, ref Int32 N_limit,ref Int32 P_limit);

	    //脉冲当量设置
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_get_equiv", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_get_equiv(UInt16 axis, ref double equiv);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_set_equiv", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_set_equiv(UInt16 axis, double new_equiv);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_get_position_unitmm", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_get_position_unitmm(UInt16 axis, ref double pos_by_mm);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_set_position_unitmm", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_set_position_unitmm(UInt16 axis, double pos_by_mm);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_read_current_speed_unitmm", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_read_current_speed_unitmm(UInt16 axis, ref double current_speed);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_get_encoder_unitmm", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_get_encoder_unitmm(UInt16 axis, ref double encoder_pos_by_mm);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_set_encoder_unitmm", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2410_set_encoder_unitmm(UInt16 axis, double encoder_pos_by_mm);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_arc_move_unitmm", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_arc_move_unitmm(UInt16[] axis, double[] target_pos, double[] cen_pos, UInt16 arc_dir);
        [DllImport("Dmc2410.dll", EntryPoint = "d2410_rel_arc_move_unitmm", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2410_rel_arc_move_unitmm(UInt16[] axis, double[] rel_pos, double[] rel_cen, UInt16 arc_dir);

    }
}
