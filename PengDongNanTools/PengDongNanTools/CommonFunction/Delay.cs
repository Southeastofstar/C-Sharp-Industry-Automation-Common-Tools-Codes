#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#endregion

#region "System.Diagnostics.Stopwatch"

//提供一组方法和属性，可用于准确地测量运行时间：

//Stopwatch 实例可以测量一个时间间隔的运行时间，也可以测量多个时间间隔的总运行时间。 
//在典型的 Stopwatch 方案中，先调用 Start 方法，然后调用 Stop 方法，最后使用 Elapsed 属性检查运行时间。

//Stopwatch 实例或者在运行，或者已停止；使用 IsRunning 可以确定 Stopwatch 的当前状态。 
//使用 Start 可以开始测量运行时间；使用 Stop 可以停止测量运行时间。 
//通过属性 Elapsed、 ElapsedMilliseconds 或 ElapsedTicks 查询运行时间值。 
//当实例正在运行或已停止时，可以查询运行时间属性。运行时间属性在 Stopwatch 运行期间稳固递增；在该实例停止时保持不变。

//默认情况下， Stopwatch 实例的运行时间值相当于所有测量的时间间隔的总和。 
//每次调用 Start 时开始累计运行时间计数；每次调用 Stop 时结束当前时间间隔测量，并冻结累计运行时间值。
//使用 Reset 方法可以清除现有 Stopwatch 实例中的累计运行时间。

//Stopwatch 在基础计时器机制中对计时器的计时周期进行计数，从而测量运行时间。 
//如果安装的硬件和操作系统支持高分辨率性能的计数器，则 Stopwatch 类将使用该计数器来测量运行时间；
//否则， Stopwatch 类将使用系统计数器来测量运行时间。 
//使用 Frequency 和 IsHighResolution 字段可以确定实现 Stopwatch 计时的精度和分辨率。

//Stopwatch 类为托管代码内与计时有关的性能计数器的操作提供帮助。 
//具体说来， Frequency 字段和 GetTimestamp 方法可以用于替换非托管 Win32 API QueryPerformanceFrequency 和 QueryPerformanceCounter。

//[DllImport("Kernel32.dll")]
//private static extern bool QueryPerformanceCounter(
//    out long lpPerformanceCount);
//[DllImport("Kernel32.dll")]
//private static extern bool QueryPerformanceFrequency(
//    out long lpFrequency);

//说明
//在多处理器计算机上，线程在哪个处理器上运行无关紧要。但是，由于 BIOS 或硬件抽象层 (HAL) 中的 bug，
//在不同的处理器上可能会得出不同的计时结果。若要为线程指定处理器关联，请使用 ProcessThread .ProcessorAffinity 方法。

//Stopwatch stopWatch = new Stopwatch();
// stopWatch.Start();
// Thread.Sleep(10000);
// stopWatch.Stop();
// // Get the elapsed time as a TimeSpan value.
// TimeSpan ts = stopWatch.Elapsed;

// // Format and display the TimeSpan value.
// string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
//     ts.Hours, ts.Minutes, ts.Seconds,
//     ts.Milliseconds / 10);
// Console.WriteLine(elapsedTime, "RunTime");

#endregion

namespace PengDongNanTools
{
    //【软件作者：彭东南, southeastofstar@163.com】
    /// <summary>
    /// 延时计时器类
    /// </summary>
    public class CDelay
    {
        #region "变量定义和系统API函数声明"

        //获取高性能计数器的当前值
        /// <summary>
        /// 获取高性能计数器的当前值
        /// </summary>
        /// <param name="lpPerformanceCount">返回高性能计数器的当前值</param>
        /// <returns>是否执行成功：返回值不是0，则执行成功；返回值是0，则通过'GetLastError'</returns>
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);//LARGE_INTEGER

        //返回调用线程的最后一次错误代码值
        /// <summary>
        /// 返回调用线程的最后一次错误代码值
        /// </summary>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        private static extern int GetLastError();//DWORD

        //获取高性能计数器的频率
        /// <summary>
        /// 获取高性能计数器的频率
        /// </summary>
        /// <param name="lpFrequency">返回高性能计数器的频率</param>
        /// <returns>如果计算机的硬件不支持高性能计数器，则返回false且lpFrequency=0</returns>
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);//LARGE_INTEGER

        /// <summary>
        /// 从开始计时已经过了多少秒
        /// </summary>
        private double TempPassedTime = 0;

        /// <summary>
        /// 从开始计时已经过了多少秒
        /// </summary>
        public double PassedTime
        {
            get
            {
                //if (SupportHighPerformanceCounter == true)
                //    {
                //    QueryPerformanceCounter(out Now);
                //    TempPassedTime = (double)(Now - BeginTime) / (double)Freqency;
                //    }
                //else
                //    {
                //    TempPassedTime = DelayWatch.ElapsedMilliseconds / 1000;
                //    }
                return Math.Round(TempPassedTime, 3);
            }
        }

        /// <summary>
        /// 延迟时间
        /// </summary>
        private double DelayTime = 0;

        private bool WaitJobIsDone = false;

        /// <summary>
        /// 延时时间是否到达
        /// </summary>
        public bool WaitDone
        {
            get
            {
                return WaitJobIsDone;
            }
        }

        /// <summary>
        /// 开始时间【高性能计数器】
        /// </summary>
        private long BeginTime = 0;

        /// <summary>
        /// 当前时间【高性能计数器】
        /// </summary>
        private long Now = 0;

        /// <summary>
        /// 高性能计数器的频率
        /// </summary>
        private long Freqency = 0;

        /// <summary>
        /// StopWatch已经过了多少毫秒
        /// </summary>
        private long StopWatchPassedMS = 0;

        private Stopwatch DelayWatch = new Stopwatch();

        private bool SupportHighPerformanceCounter = true;

        /// <summary>
        /// 提示信息
        /// </summary>
        public string ErrorMessage = "";

        /// <summary>
        /// 是否成功建立实例
        /// </summary>
        private bool SuccessBuiltNew = false;

        /// <summary>
        /// 密码是否正确
        /// </summary>
        private bool PasswordIsCorrect = false;

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
        {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
        }

        #endregion

        //建立延时类的新实例  /// <param name="TargetDelayTime">延时时间【单位：秒】</param>
        /// <summary>
        /// 建立延时类的新实例
        /// </summary>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public CDelay(string DLLPassword)//double TargetDelayTime, 
        {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;

            if (DLLPassword == "ThomasPeng" || DLLPassword == "pengdongnan"
                || DLLPassword == "彭东南" || DLLPassword == "PDN")
            {
                PasswordIsCorrect = true;
            }
            else
            {
                PasswordIsCorrect = false;
                SuccessBuiltNew = false;
                //MessageBox.Show("Right Prohibited.\return\n     You don't have the given right to use this DLL library, please contact with ThomasPeng.\r\n你未得到授权的密码，无法使用此DLL进行软件开发！请与作者彭东南联系：southeastofstar@163.com\r\n                                                                版权所有： 彭东南", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //return;
                //throw new InvalidOperationException();
                for (; ; )
                {
                    ;
                }
            }

            try
            {
                //如果计算机硬件不支持高性能计数器，就用Stopwatch
                if (QueryPerformanceFrequency(out Freqency) == false)
                {
                    ErrorMessage = "不支持高性能计数器，就用Stopwatch";
                    SupportHighPerformanceCounter = false;
                    //DelayWatch.Reset();
                    //DelayWatch.Start();
                }
                else
                {
                    ErrorMessage = "支持高性能计数器";
                    SupportHighPerformanceCounter = true;
                    //QueryPerformanceCounter(out BeginTime);
                }

                SuccessBuiltNew = true;
            }
            catch (Exception ex)
            {
                SuccessBuiltNew = false;
                MessageBox.Show("创建类的实例时出现错误！\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        #region "函数代码"

        //开始计时
        /// <summary>
        /// 开始计时
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
                if (SupportHighPerformanceCounter == true)
                {
                    QueryPerformanceCounter(out BeginTime);
                }
                else
                {
                    DelayWatch.Reset();
                    DelayWatch.Start();
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        //等待指定时间，如果计算机不支持高性能计数器，则只支持整数秒的时间
        /// <summary>
        /// 等待指定时间，如果计算机不支持高性能计数器，则只支持整数秒的时间
        /// 此函数包括do...while循环，延时到达后就会返回true并继续往下执行
        /// 不需要在调用函数的程序中添加判断代码；
        /// </summary>
        /// <param name="WaitTime">等待时间【单位：秒】</param>
        /// <returns>等待时间到达</returns>
        public bool Wait(double WaitTime)
        {
            try
            {
                //if (SupportHighPerformanceCounter == true)
                //    {
                //    QueryPerformanceCounter(out Now);
                //    TempPassedTime = (double)(Now - BeginTime) / (double)Freqency;
                //    }
                //else
                //    {
                //    TempPassedTime = DelayWatch.ElapsedMilliseconds / 1000;
                //    }

                do
                {
                    if (SupportHighPerformanceCounter == true)
                    {
                        QueryPerformanceCounter(out Now);
                        TempPassedTime = (double)(Now - BeginTime) / (double)Freqency;
                    }
                    else
                    {
                        TempPassedTime = DelayWatch.ElapsedMilliseconds / 1000;
                    }
                    ;
                }
                while (TempPassedTime < WaitTime);

                return true;

                //if (TempPassedTime > WaitTime)
                //    {
                //    return true;
                //    }
                //else
                //    {
                //    return false;
                //    }

                //if (SupportHighPerformanceCounter == true)
                //    {
                //    QueryPerformanceCounter(out Now);
                //    TempPassedTime = (double)(Now - BeginTime) / (double)Freqency;

                //    if (TempPassedTime > WaitTime)
                //        {
                //        return true;
                //        }
                //    else 
                //        {
                //        return false;
                //        }
                //    }
                //else 
                //    {
                //    TempPassedTime = DelayWatch.ElapsedMilliseconds / 1000;
                //    if (TempPassedTime > WaitTime)
                //        {
                //        return true;
                //        }
                //    else
                //        {
                //        return false;
                //        }
                //    }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        //等待指定时间，如果计算机不支持高性能计数器，则只支持整数秒的时间
        /// <summary>
        /// 【如果在指定时间内某个条件成立，就返回 true；否则返回 false】
        /// 等待指定时间，如果计算机不支持高性能计数器，则只支持整数秒的时间
        /// 需要在调用此函数的程序里面一直进行判断返回值
        /// </summary>
        /// <param name="WaitTime">等待时间【单位：秒】</param>
        /// <param name="JudgementCondition">附加判断条件</param>
        /// <param name="JudgementValue">附加判断条件的判断值</param>
        /// <returns>等待时间到达</returns>
        public bool Wait(double WaitTime, bool JudgementCondition, bool JudgementValue)
        {
            try
            {
                if (SupportHighPerformanceCounter == true)
                {
                    QueryPerformanceCounter(out Now);
                    TempPassedTime = (double)(Now - BeginTime) / (double)Freqency;
                }
                else
                {
                    TempPassedTime = DelayWatch.ElapsedMilliseconds / 1000;
                }

                //do
                //    {
                //    if (SupportHighPerformanceCounter == true)
                //        {
                //        QueryPerformanceCounter(out Now);
                //        TempPassedTime = (double)(Now - BeginTime) / (double)Freqency;
                //        }
                //    else
                //        {
                //        TempPassedTime = DelayWatch.ElapsedMilliseconds / 1000;
                //        }
                //    ;
                //    }
                //while (TempPassedTime < WaitTime);

                //return true;

                if ((TempPassedTime < WaitTime && JudgementCondition == JudgementValue) || (TempPassedTime >= WaitTime && JudgementCondition == JudgementValue))
                {
                    DelayWatch.Reset();
                    TempPassedTime = 0;
                    QueryPerformanceCounter(out BeginTime);
                    return true;
                }
                else if (TempPassedTime >= WaitTime && JudgementCondition != JudgementValue)
                {
                    TempPassedTime = 0;
                    DelayWatch.Reset();
                    QueryPerformanceCounter(out BeginTime);
                    return false;
                }
                else
                {
                    return false;
                }

                //if (SupportHighPerformanceCounter == true)
                //    {
                //    QueryPerformanceCounter(out Now);
                //    TempPassedTime = (double)(Now - BeginTime) / (double)Freqency;

                //    if (TempPassedTime > WaitTime)
                //        {
                //        return true;
                //        }
                //    else 
                //        {
                //        return false;
                //        }
                //    }
                //else 
                //    {
                //    TempPassedTime = DelayWatch.ElapsedMilliseconds / 1000;
                //    if (TempPassedTime > WaitTime)
                //        {
                //        return true;
                //        }
                //    else
                //        {
                //        return false;
                //        }
                //    }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        //等待指定时间，如果计算机不支持高性能计数器，则只支持整数秒的时间
        /// <summary>
        /// 等待指定时间，如果计算机不支持高性能计数器，则只支持整数秒的时间
        /// 需要在调用此函数的程序里面一直进行判断返回值
        /// </summary>
        /// <param name="WaitTime">等待时间【单位：秒】</param>
        /// <param name="JudgementCondition">附加判断条件【没有实际作用，只是为了区分重载】</param>
        /// <returns>等待时间到达</returns>
        public bool Wait(double WaitTime, bool JudgementCondition = false)
        {
            try
            {
                if (SupportHighPerformanceCounter == true)
                {
                    QueryPerformanceCounter(out Now);
                    TempPassedTime = (double)(Now - BeginTime) / (double)Freqency;
                }
                else
                {
                    TempPassedTime = DelayWatch.ElapsedMilliseconds / 1000;
                }

                //do
                //    {
                //    if (SupportHighPerformanceCounter == true)
                //        {
                //        QueryPerformanceCounter(out Now);
                //        TempPassedTime = (double)(Now - BeginTime) / (double)Freqency;
                //        }
                //    else
                //        {
                //        TempPassedTime = DelayWatch.ElapsedMilliseconds / 1000;
                //        }
                //    ;
                //    }
                //while (TempPassedTime < WaitTime);

                //return true;

                if (TempPassedTime >= WaitTime)
                {
                    DelayWatch.Reset();
                    TempPassedTime = 0;
                    QueryPerformanceCounter(out BeginTime);
                    return true;
                }
                else
                {
                    return false;
                }

                //if (SupportHighPerformanceCounter == true)
                //    {
                //    QueryPerformanceCounter(out Now);
                //    TempPassedTime = (double)(Now - BeginTime) / (double)Freqency;

                //    if (TempPassedTime > WaitTime)
                //        {
                //        return true;
                //        }
                //    else 
                //        {
                //        return false;
                //        }
                //    }
                //else 
                //    {
                //    TempPassedTime = DelayWatch.ElapsedMilliseconds / 1000;
                //    if (TempPassedTime > WaitTime)
                //        {
                //        return true;
                //        }
                //    else
                //        {
                //        return false;
                //        }
                //    }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        //复位计时器，准备下一次计时
        /// <summary>
        /// 复位计时，准备下一次计时
        /// </summary>
        /// <returns></returns>
        public bool Reset()
        {
            try
            {
                if (SupportHighPerformanceCounter == true)
                {
                    QueryPerformanceCounter(out BeginTime);
                }
                else
                {
                    //DelayWatch.Stop();
                    DelayWatch.Reset();
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        //关闭延时计时器类并释放相关资源
        /// <summary>
        /// 关闭延时计时器类并释放相关资源
        /// </summary>
        public void Close()
        {
            try
            {
                DelayWatch = null;
            }
            catch (Exception)
            {
            }
        }

        #endregion

    }//class

}//namespace