using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;

#region "VB与C#差异"

//1、
//VB中数组定义 dim a(2) as string, 数组的长度为3，下标从0~2；
//C#中数组定义 string[] a=new string[2]，数组的长度为2，下标从0~1；

//2、
//CR -- Carriage Return:回车， 用\r表示；
//LF -- Line Feed: 换行， 用\n表示;

//3、NetworkStatus_Menu.Image = global::PengDongNanTools.Properties.Resources.NoNetwork;

//4、System.Net.ServicePointManager.SetTcpKeepAlive(true, 1000, 1000);

//5、逻辑“与”	    x & y	    整型按位 AND，布尔逻辑 AND
     //逻辑 XOR	    x ^ y	    整型按位 XOR，布尔逻辑 XOR
     //逻辑 OR	    x | y	    整型按位 OR，布尔逻辑 OR
     //条件 AND	    x && y	    仅当 x 为 true 时，才对 y 求值
     //条件 OR	    x || y	    仅当 x 为 false 时，才对 y 求值
     //null 合并	X ?? y	    如果 x 为 null，则计算结果为 y，否则计算结果为 x
     //条件	        x ? y : z	如果 x 为 true，则对 y 求值；如果 x 为 false，则对 z 求值

     //移位	x << y	左移
     //     x >> y	右移

     //关系和类型检测
     //x is T	如果 x 为 T，则返回 true，否则返回 false
     //x as T	返回转换为类型 T 的 x，如果 x 不是 T 则返回 null

    //加减	x + y	加法、字符串串联、委托组合
	//      x – y	减法、委托移除

    //移位	x << y	左移
	//      x >> y	右移

    //乘法	x * y	乘法
	//      x / y	除法
	//      x % y	求余

    //一元	
    //    +x	    恒等
    //    -x	    求相反数
    //    !x	    逻辑求反
    //    ~x	    按位求反
    //    ++x	    前增量
    //    --x	    前减量
    //    (T)x	    将 x 显式转换为类型 T
    //    await x	异步等待 x 完成

    //基本	x.m	成员访问
    //    x(...)	        方法和委托调用
    //    x[...]	        数组和索引器访问
    //    x++	            后增量
    //    x--	            后减量
    //    new T(...)	    对象和委托创建
    //    new T(...){...}	使用初始值设定项创建对象
    //    new {...}	        匿名对象初始值设定项
    //    new T[...]	    数组创建
    //    typeof(T)	        获取 T 的 System.Type 对象
    //    checked(x)	    在 checked 上下文中计算表达式
    //    unchecked(x)	    在 unchecked 上下文中计算表达式
    //    default(T)	    获取类型 T 的默认值
    //    delegate {...}	匿名函数（匿名方法）

//VB    &H表示16进制，&O表示8进制
//C#    0x表示16进制【零x】

//6、C#中在struct中创建数组时，必须加fixed前缀，在使用此数据结构的函数添加unsafe， private unsafe void 
//    unsafe 关键字表示不安全上下文，该上下文是任何涉及指针的操作所必需的。
//    例如：
//      private unsafe struct Te
//      {
//      public fixed bool yee[64];
//      }
       //unsafe static void SquarePtrParam(int* p)
       //{
       //   *p *= *p;
       //}

//7、
       //byte ok = 12;
       //string ss = Convert.ToString(ok, 16);  输出的是C，即16进制字符
       //string ss = Convert.ToString(ok, 10);  输出的是10，即10进制字符
       //string ss = Convert.ToString(ok, 2);  输出的是1100，即2进制字符

       //PadLeft(Int32, Char)	返回一个新字符串，
       //该字符串通过在此实例中的字符左侧填充指定的 Unicode 字符来达到指定的总长度，
       //从而使这些字符右对齐。

//8、C#中TAB键是\t；

//9、索引器
//  索引器 (indexer) 是这样一个成员：它支持按照索引数组的方法来索引对象。
//  索引器的声明与属性类似，不同的是该成员的名称是 this，后跟一个位于定界符 [ 和 ] 之间的参数列表。
//  在索引器的访问器中可以使用这些参数。与属性类似，索引器可以是读写、只读和只写的，
//  并且索引器的访问器可以是虚的。
//  该 List 类声明了单个读写索引器，该索引器接受一个 int 参数。
//  该索引器使得通过 int 值对 List 实例进行索引成为可能。例如
//  List<string> names = new List<string>();
//  names.Add("Liz");
//  names.Add("Martha");
//  names.Add("Beth");
//  for (int i = 0; i < names.Count; i++) {
//    string s = names[i];
//    names[i] = s.ToUpper();
//  }
//  索引器可以被重载，这意味着一个类可以声明多个索引器，只要其参数的数量和类型不同即可。

//10、事件
//  事件 (event) 是一种使类或对象能够提供通知的成员。事件的声明与字段类似，
//  不同的是事件的声明包含 event 关键字，并且类型必须是委托类型。
//  在声明事件成员的类中，事件的行为就像委托类型的字段（前提是该事件不是抽象的并且未声明访问器）。
//  该字段存储对一个委托的引用，该委托表示已添加到该事件的事件处理程序。
//  如果尚未添加事件处理程序，则该字段为 null。
//  List<T> 类声明了一个名为 Changed 的事件成员，它指示已将一个新项添加到列表中。
//  Changed 事件由 OnChanged 虚方法引发，后者先检查该事件是否为 null（表明没有处理程序）。
//  “引发一个事件”与“调用一个由该事件表示的委托”这两个概念完全等效，
//  因此没有用于引发事件的特殊语言构造。
//  客户端通过事件处理程序 (event handler) 来响应事件。事件处理程序使用 += 运算符附加，
//  使用 -= 运算符移除。下面的示例向 List<string> 类的 Changed 事件附加一个事件处理程序。
//  using System;
//  class Test
//  {
//    static int changeCount;
//    static void ListChanged(object sender, EventArgs e) {
//        changeCount++;
//      }
//    static void Main() {
//        List<string> names = new List<string>();
//        names.Changed += new EventHandler(ListChanged);
//        names.Add("Liz");
//        names.Add("Martha");
//        names.Add("Beth");
//        Console.WriteLine(changeCount);		// Outputs "3"
//    }
//  }
//  对于要求控制事件的底层存储的高级情形，事件声明可以显式提供 add 和 remove 访问器，它们在某种程度上类似于属性的 set 访问器。

//11、如果需要在一个类的里面再创建一个类，而且需要将外面类的变量在里面的类进行引用，
//    则需要将外面的变量定义为【internal static】；

//12、System.Threading.Timer AutoSendTimer = new System.Threading.Timer(null);
//    可以利用此方法顶时调用某个函数进行处理；

//13、在程序中如果对公共属性【具有读写性质】进行赋值，会发生编译错误；

//14、如果在发送数据包时用到[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
//    和[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
//    就需要引用：using System.Runtime.InteropServices;

//15、Dictionary<string, Socket> SocketDict = new Dictionary<string, Socket>();//存放套接字

//16、使用线程计时器 【System.Threading.Timer】的TimerCallback委托进行定时调用函数
    //发生错误提示：TimerCallBack不能为空
    //System.Threading.Timer AutoSendTimer = new System.Threading.Timer(null);

    //public void StartTimer(int dueTime)
    //{
    //    Timer t = new Timer(new TimerCallback(TimerProc));
    //    t.Change(dueTime, 0);
    //}

    //private void TimerProc(object state)
    //{
    //    // The state object is the Timer object.
    //    Timer t = (Timer) state;
    //    t.Dispose();
    //    Console.WriteLine("The timer callback executes.");
    //}

//17、预防一个程序多次开启运行
    //bool SoftwareAlreadyOpened = false;
    //Mutex MutexCheck = new Mutex(false, "software", out SoftwareAlreadyOpened);
    //if (!SoftwareAlreadyOpened)
    //    {
    //    MessageBox.Show("The software was already opened, you can't run another copy at the same time.\r\n出于安全考虑，不允许同时运行此软件的另外一个副本.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    //    return;
    //    }

//18、如果不知道返回的数组大小，可以先定义一个数组，然后将返回的数组赋值给定义的数组即可；
    //这样不会报错
    //string[] y;
    //y= XML.FindInnerTextInXMLFile("data", "TestPara");

//19、为了在以太网通讯时发送以下格式的文本，可以将要发送的内容先写入文件，然后再发送除去即可；
    //<?xml version="1.0" encoding="gb2312"?>
    //<root>
    //  <ccdPara>
    //    <para>
    //      <id>0</id>
    //      <p>0</p>
    //      <dataPart>60</dataPart>
    //      <TestPara>
    //        <index>0</index>
    //        <testName>FAI 4</testName>
    //        <normal>002.390</normal>
    //        <min>0.030</min>
    //        <max>0.050</max>
    //        <pixV>0.000</pixV>
    //        <Bc>000.000</Bc>
    //      </TestPara>
    //    </para>
    //  </ccdPara>
    //  <one>
    //    <id>1087</id>
    //    <flag>NG</flag>
    //    <machineId>MachineID:001</machineId>
    //    <mould />
    //    <Date>2016/12/15 21:47:34</Date>
    //    <str>+0000000.0,+0000000.00</str>
    //  </one>
    //</root>

//20、可以通过DOS命令tasklist >d:\t.txt得到当前进程清单，然后找出对应的exe/com文件，
    //执行taskkill /F /IM 文件全名，就可以关闭相关进程

//21、在建立以太网通讯的连接时，可以添加到字典变量中，
    //System.Collections.Generic.Dictionary<string, System.Net.Sockets.TcpClient> xx = new Dictionary<string, System.Net.Sockets.TcpClient>();
    //if (xx != null) 
    //    {
    //    foreach (System.Net.Sockets.TcpClient a in xx.Values) 
    //        {
    //        //发送内容的代码部分
    //        }
    //    }

//22、使用非托管 DLL 函数
    //利用平台调用这种服务，托管代码可以调用在动态链接库 (DLL)（如 Win32 API 中的 DLL）中实现的非托管函数。
    //此服务将查找并调用导出的函数，然后根据需要跨越互用边界封送其参数（整数、字符串、数组、结构等）。
    //使用导出的 DLL 函数

    //标识 DLL 中的函数。

    //最低限度上，必须指定函数的名称和包含该函数的 DLL 的名称。

    //创建用于容纳 DLL 函数的类。

    //可以使用现有类，为每一非托管函数创建单独的类，或者创建包含一组相关的非托管函数的一个类。

    //在托管代码中创建原型。

    //[Visual Basic] 使用带 Function 和 Lib 关键字的 Declare 语句。 在某些少见的情况下，可以使用带 Shared Function 关键字的 DllImportAttribute。 这些情况在本节后面部分进行说明。

    //[C#] 使用 DllImportAttribute 标识 DLL 和函数。 用 static 和 extern 修饰符标记方法。

    //[C++] 使用 DllImportAttribute 标识 DLL 和函数。 用 extern "C" 标记包装方法或函数。

    //调用 DLL 函数。

    //像处理其他任何托管方法一样调用托管类上的方法。传递结构和 实现回调函数属于特殊情况。

//23、Marshal 类
    //提供了一个方法集，这些方法用于分配非托管内存、复制非托管内存块、将托管类型转换为非托管类型，
    //此外还提供了在与非托管代码交互时使用的其他杂项方法。
    //Marshal 类中定义的 static 方法对于处理非托管代码至关重要。 此类中定义的大多数方法通常由需要
    //在托管和非托管编程模型之间提供桥梁的开发人员使用。
    //例如， StringToHGlobalAnsi 方法将 ANSI 字符从指定的字符串（在托管堆中）复制到非托管堆中的缓冲区。
    //该方法还分配大小正确的目标堆。
    //Marshal 类中的 Read 和 Write 方法支持对齐和未对齐的访问。


//24、在一个窗体中同一个区域显示不同窗体或者控件，使用panel控件；
    //先将需要操作的对象窗体或者控件添加到panel控件中，然后再设置此控件对应子项的Hide属性
    //例如在一个窗体的panel控件中添加窗体：
    //form1.TopLevel=false;
    //panelControls.Add(form1);
    //form1.size=panel1.size;
    //form1.mainForm = this;
    //form1.Hide();
    //将所有需要的窗体全部按照上述方式添加至panel后，就可以视需要将某个窗体Show();

//25、FileDialog .Filter 属性(OPEN/SAVE)
    //获取或设置当前文件名筛选器字符串，该字符串决定对话框的“另存为文件类型”或“文件类型”框中出现的选择内容。
    //对于每个筛选选项，筛选器字符串都包含筛选器说明，后接一垂直线条 (|) 和筛选器模式。不同筛选选项的字符串由垂直线条隔开。

    //下面是一个筛选器字符串的示例：
    //Text files (*.txt)|*.txt|All files (*.*)|*.*

    //通过使用分号来分隔文件类型，可将多个筛选器模式添加到筛选器中，例如：
    //Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*

    //使用 FilterIndex【第一个筛选器条目的索引值为 1】 属性设置第一个显示给用户的筛选选项。

//26、进制转换算法
#region "进制转换算法"

//        1. 十 -----> 二 
//        （25.625）（十） 
//        整数部分： 
//        25/2=12......1 余数
//        12/2=6 ......0 这里被整除了，所以0
//        6/2=3 ......0 
//        3/2=1 ......1 
//        1/2=0 ......1 
//        然后我们将余数按从下往上的顺序书写就是：11001，那么这个11001就是十进制25的二进制形式 
//        小数部分： 
//        0.625*2=1.25 
//        0.25 *2=0.5 
//        0.5 *2=1.0 
//        然后我们将整数部分按从上往下的顺序书写就是：101，那么这个101就是十进制0.625的二进制形式 
//        所以：（25.625）（十）=（11001.101）（二） 
//        十进制转成二进制是这样: 
//        把这个十进制数做二的整除运算,并将所得到的余数倒过来． 
//        例如将十进制的10转为二进制是这样： 
//        (1) 10/2,商5余0； 
//        (2) 5/2,商2余1； 
//        (3)2/2,商1余0； 
//        (4)1／2，商0余1． 
//        (5)将所得的余数侄倒过来，就是1010，所以十进制的10转化为二进制就是1010 
//        2. 二 ----> 十 

//        （11001.101）（二） 
//        整数部分： 下面的出现的2（x）表示的是2的x次方的意思 
//        1*2（4）+1*2（3）+0*2（2）+0*2（1）+1*2（0）=25 
//        小数部分： 
//        1*2（-1）+0*2（-2）+1*2（-3）=0.625 
//        所以：（11001.101）（二）=（25.625）（十） 
//        二进制转化为十进制是这样的： 
//        这里可以用8421码的方法．这个方法是将你所要转化的二进制从右向左数，从0开始数（这个数我们叫N），在位数是1的地方停下，并将1乘以2的N次方，最后将这些1乘以2的N次方相加，就是这个二进数的十进制了． 
//        还是举个例子吧： 
//        求110101的十进制数．从右向左开始了 
//        (1) 1乘以2的0次方，等于1； 
//        (2) 1乘以2的2次方，等于4； 
//        (3) 1乘以2的4次方，等于16； 
//        (4) 1乘以2的5次方，等于32； 
//        (5) 将这些结果相加：1＋4＋16＋32＝53 
//        3. 十 ----> 八 
//        （25.625）（十） 
//        整数部分： 
//        25/8=3......1 
//        3/8 =0......3 
//        然后我们将余数按从下往上的顺序书写就是：31，那么这个31就是十进制25的八进制形式 
//        小数部分： 
//        0.625*8=5 
//        然后我们将整数部分按从上往下的顺序书写就是：5，那么这个0.5就是十进制0.625的八进制形式 
//        所以：（25.625）（十）=（31.5）（八） 
//        4. 八 ----> 十 
//        （31.5）（八） 
//        整数部分： 
//        3*8（1）+1*8（0）=25 
//        小数部分： 
//        5*8（-1）=0.625 
//        所以（31.5）（八）=（25.625）（十） 
//        5. 十 ----> 十六 
//        （25.625）（十） 
//        整数部分： 
//        25/16=1......9 
//        1/16 =0......1 
//        然后我们将余数按从下往上的顺序书写就是：19，那么这个19就是十进制25的十六进制形式 
//        小数部分： 
//        0.625*16=10（即十六进制的A或a） 
//        然后我们将整数部分按从上往下的顺序书写就是：A，那么这个A就是十进制0.625的十六进制形式 
//        所以：（25.625）（十）=（19.A）（十六） 
//        6. 十六----> 十 
//        （19.A）（十六） 
//        整数部分： 
//        1*16（1）+9*16（0）=25 
//        小数部分： 
//        10*16（-1）=0.625 
//        所以（19.A）（十六）=（25.625）（十） 
//        如何将带小数的二进制与八进制、十六进制数之间的转化问题 
//        我们以（11001.101）（二）为例讲解一下进制之间的转化问题 
//        说明：小数部份的转化计算机二级是不考的，有兴趣的人可以看一看 
//        1. 二 ----> 八 
//        （11001.101）（二） 
//        整数部分： 从后往前每三位一组，缺位处用0填补，然后按十进制方法进行转化， 则有： 
//        001=1 
//        011=3 
//        然后我们将结果按从下往上的顺序书写就是：31，那么这个31就是二进制11001的八进制形式 
//        小数部分： 从前往后每三位一组，缺位处用0填补，然后按十进制方法进行转化， 则有： 
//        101=5 
//        然后我们将结果部分按从上往下的顺序书写就是：5，那么这个5就是二进制0.101的八进制形式 
//        所以：（11001.101）（二）=（31.5）（八） 
//        2. 八 ----> 二 
//        （31.5）（八） 
//        整数部分：从后往前每一位按十进制转化方式转化为三位二进制数，缺位处用0补充 则有： 
//        1---->1---->001 
//        3---->11 
//        然后我们将结果按从下往上的顺序书写就是：11001，那么这个11001就是八进制31的二进制形式 
//        说明，关于十进制的转化方式我这里就不再说了，上一篇文章我已经讲解了！ 
//        小数部分：从前往后每一位按十进制转化方式转化为三位二进制数，缺位处用0补充 则有： 
//        5---->101 
//        然后我们将结果按从下往上的顺序书写就是：101，那么这个101就是八进制5的二进制形式 
//        所以：（31.5）（八）=（11001.101）（二） 
//        3. 十六 ----> 二 
//        （19.A）（十六） 
//        整数部分：从后往前每位按十进制转换成四位二进制数，缺位处用0补充 则有： 
//        9---->1001 
//        1---->0001（相当于1） 
//        则结果为00011001或者11001 
//        小数部分：从前往后每位按十进制转换成四位二进制数，缺位处用0补充 则有： 
//        A(即10)---->1010 
//        所以：（19.A）（十六）=（11001.1010）（二）=（11001.101）（二） 
//        4. 二 ----> 十六 
//        （11001.101）（二） 
//        整数部分：从后往前每四位按十进制转化方式转化为一位数，缺位处用0补充 则有： 
//        1001---->9 
//        0001---->1 
//        则结果为19 
//        小数部分：从前往后每四位按十进制转化方式转化为一位数，缺位处用0补充 则有： 
//        1010---->10---->A 
//        则结果为A 
//        所以：（11001.101）（二）=（19.A）（十六） 
//        [编辑本段]二、负数 
//        负数的进制转换稍微有些不同。 
//        先把负数写为其补码形式（在此不议），然后再根据二进制转换其它进制的方法进行。 
//        例：要求把-9转换为八进制形式。则有： 
//        -9的补码为11111001。然后三位一划 
//        001---->1 
//        111---->157 
//        011---->3 
//        然后我们将结果按从下往上的顺序书写就是：31571，那么31571就是十进制数-9的八进制形式。 
//        补充： 
//        最近有些朋友提了这样的问题“0.8的十六进制是多少？” 
//        我想在我的空间里已经有了详细的讲解，为什么他还要问这样的问题那 
//        于是我就动手算了一下，发现0.8、0.6、0.2... ...一些数字在进制之间的转化 
//        过程中确实存在麻烦。 
//        就比如“0.8的十六进制”吧！ 
//        无论你怎么乘以16，它的余数总也乘不尽，总是余8 
//        这可怎么办啊，我也没辙了 
//        第二天，我请教了我的老师才知道，原来这么简单啊！ 
//        具体方法如下： 
//        0.8*16=12.8 
//        0.8*16=12.8 
//        . 
//        . 
//        . 
//        . 
//        . 
//        取每一个结果的整数部分为12既十六进制的C 
//        如果题中要求精确到小数点后3位那结果就是0.CCC 
//        如果题中要求精确到小数点后4位那结果就是0.CCCC 
//        现在OK了，我想我的朋友再也不会因为进制的问题烦愁了！ 

#endregion
#endregion

//27、string类型的变量在引用某个位置的字符时，可以直接加[位置号]，
    //例如：string str="1234", 则str[0]就是指字符'1'


//28、FileSystemProxy .WriteAllText 方法 (String, String, Boolean)
    //向文件写入文本
    //使用 UTF-8 编码方式来写入此文件。若要指定其他编码，请使用 WriteAllText () 方法的其他重载。

    //如果指定的文件不存在，则创建该文件。

    //如果指定的编码方式与文件的现有编码方式不匹配，则忽略指定的编码方式。
    //WriteAllText 方法将打开一个文件，向其写入内容，然后将其关闭。 
    //使用 WriteAllText 方法的代码比使用 StreamWriter 对象的代码更加简单。 
    //但是，如果您使用循环将字符串添加到文件中，则 StreamWriter 对象能够提供更优异的性能，因为您只需打开和关闭该文件一次。


//29、Array 类
    //提供创建、操作、搜索和排序数组的方法，因而在公共语言运行时中用作所有数组的基类。
    //Array 类是支持数组的语言实现的基类。 但是，只有系统和编译器可以从 Array 类显式派生。 用户应当使用由语言提供的数组构造。
    //一个元素就是 Array 中的一个值。 Array 的长度是它可包含的元素总数。 Array 的秩是 Array 中的维数。
    //Array 中维度的下限是 Array 中该维度的起始索引，多维 Array 的各个维度可以有不同的界限。 数组最多可以有 32 个维。
    
    //Type 对象提供有关数组类型声明的信息。 具有相同数组类型的 Array 对象共享同一 Type 对象。

    //Type .IsArray 和 Type .GetElementType 可能不返回所预期的 Array 形式的结果，
    //因为如果某个数组被强制转换为 Array 类型，则结果是对象，而非数组。 
    //即， typeof(System.Array).IsArray 返回 false，而 typeof(System.Array).GetElementType 返回 null。

    //与大多数类不同， Array 提供 CreateInstance 方法，以便允许后期绑定访问，而不是提供公共构造函数。

    //Array .Copy 方法不仅可在同一类型的数组之间复制元素，而且可在不同类型的标准数组之间复制元素；它会自动处理强制转换。

    //有些方法（如 CreateInstance、 Copy、 CopyTo、 GetValue 和 SetValue）提供接受 64 位整数作为参数的重载，以适应大容量数组。
    //LongLength 和 GetLongLength 返回 64 位整数，表示数组的长度。

    //不保证会对 Array 进行排序。 在执行需要对 Array 进行排序的操作（如 BinarySearch）之前，必须对 Array 进行排序。

    //不支持在本机代码中使用指针的 Array 对象，这种用法将对几种方法引发 NotSupportedException。

//30、Array .Copy 方法 (Array, Int32, Array, Int32, Int32)
    //从指定的源索引开始，复制 Array 中的一系列元素，将它们粘贴到另一 Array 中（从指定的目标索引开始）。 长度和索引指定为 32 位整数。
    //public static void Copy(Array sourceArray,int sourceIndex,
    //    Array destinationArray,int destinationIndex, int length)
    //sourceArray         类型： System .Array     Array，它包含要复制的数据。 
    //sourceIndex         类型： System .Int32     一个 32 位整数，它表示 sourceArray 中复制开始处的索引。 
    //destinationArray    类型： System .Array     Array，它接收数据。 
    //destinationIndex    类型： System .Int32     一个 32 位整数，它表示 destinationArray 中存储开始处的索引。 
    //length              类型： System .Int32     一个 32 位整数，它表示要复制的元素数目。

//31、Ping 服务器并指定超时
    //Microsoft.VisualBasic.Devices.Network NT = new Microsoft.VisualBasic.Devices.Network();
    //NT.Ping("WWW.BAIDU.COM", 100);

//32、lock和 SyncLock 关键字 “锁定”语句【提供给 lock 关键字的参数必须为基于引用类型的对象，该对象用来定义锁的范围。 】
    //lock 关键字将语句块标记为临界区，方法是获取给定对象的互斥锁，执行语句，然后释放该锁。 此语句的形式如下：

    //lock (C#) 和 SyncLock (Visual Basic) 语句可以用来确保代码块完成运行，而不会被其他线程中断。 这是通过在代码块运行期间为给定对象获取互斥锁来实现的。

    //lock 或 SyncLock 语句有一个作为参数的对象，在该参数的后面还有一个一次只能由一个线程执行的代码块。 例如：

    //Object thisLock = new Object();【提供给 lock 关键字的参数必须为基于引用类型的对象，该对象用来定义锁的范围。 】
    //lock (thisLock)
    //{
        // Critical code section.
    //}

    //lock 关键字可确保当一个线程位于代码的临界区时，另一个线程不会进入该临界区。 
    //如果其他线程尝试进入锁定的代码，则它将一直等待（即被阻止），直到该对象被释放。

    //lock 关键字在块的开始处调用 Enter，而在块的结尾处调用 Exit。

    //通常，应避免锁定 public 类型，否则实例将超出代码的控制范围。 
    //常见的结构 lock (this)、 lock (typeof (MyType)) 和 lock ("myLock") 违反此准则：

    //如果实例可以被公共访问，将出现 lock (this) 问题。

    //如果 MyType 可以被公共访问，将出现 lock (typeof (MyType)) 问题。

    //由于进程中使用同一字符串的任何其他代码都将共享同一个锁，所以出现 lock(“myLock”) 问题。

    //最佳做法是定义 private 对象来锁定, 或 private static 对象变量来保护所有实例所共有的数据。

    //提供给 lock 关键字的参数必须为基于引用类型的对象，该对象用来定义锁的范围。 
    //在上面的示例中，锁的范围限定为此函数，因为函数外不存在任何对对象 lockThis 的引用。 
    //如果确实存在此类引用，锁的范围将扩展到该对象。严格地说，提供的对象只是用来唯一地标识由多个线程共享的资源，所以它可以是任意类实例。
    //然而，实际上，此对象通常表示需要进行线程同步的资源。例如，如果一个容器对象将被多个线程使用，则可以将该容器传递给 lock，而 lock 后面的同步代码块将访问该容器。
    //只要其他线程在访问该容器前先锁定该容器，则对该对象的访问将是安全同步的。

    //通常，最好避免锁定 public 类型或锁定不受应用程序控制的对象实例。 例如，如果该实例可以被公开访问，则 lock(this) 可能会有问题，因为不受控制的代码也可能会锁定该对象。 
    //这可能导致死锁，即两个或更多个线程等待释放同一对象。出于同样的原因，锁定公共数据类型（相比于对象）也可能导致问题。
    //锁定字符串尤其危险，因为字符串被公共语言运行时 (CLR)“暂留”。这意味着整个程序中任何给定字符串都只有一个实例，
    //就是这同一个对象表示了所有运行的应用程序域的所有线程中的该文本。因此，只要在应用程序进程中的任何位置处具有相同内容的字符串上放置了锁，
    //就将锁定应用程序中该字符串的所有实例。因此，最好锁定不会被暂留的私有或受保护成员。某些类提供专门用于锁定的成员。
    //例如， Array 类型提供 SyncRoot。 许多集合类型也提供 SyncRoot。


    //与 lock 和 SyncLock 关键字类似，监视器防止多个线程同时执行代码块。
    //Enter 方法允许一个且仅一个线程继续执行后面的语句；其他所有线程都将被阻止，直到执行语句的线程调用 Exit。 这与使用 lock 关键字一样。 例如：
    //lock (x)
    //{
    //    DoSomething();
    //}

    //这等效于：
    //System.Object obj = (System.Object)x;
    //System.Threading.Monitor.Enter(obj);
    //try
    //{
    //    DoSomething();
    //}
    //finally
    //{
    //    System.Threading.Monitor.Exit(obj);
    //}

    //使用 lock (C#) 或 SyncLock (Visual Basic) 关键字通常比直接使用 Monitor 类更可取，
    //一方面是因为 lock 或 SyncLock 更简洁，另一方面是因为 lock 或 SyncLock 确保了即使受保护的代码引发异常，
    //也可以释放基础监视器。 这是通过 finally 关键字来实现的，无论是否引发异常它都执行关联的代码块。

//33、同步事件和等待句柄
    //使用锁或监视器对于防止同时执行区分线程的代码块很有用，但是这些构造不允许一个线程向另一个线程传达事件。
    //这需要“同步事件”，它是有两个状态（终止和非终止）的对象，可以用来激活和挂起线程。
    //让线程等待非终止的同步事件可以将线程挂起，将事件状态更改为终止可以将线程激活。
    //如果线程尝试等待已经终止的事件，则线程将继续执行，而不会延迟。

    //同步事件有两种： AutoResetEvent 和 ManualResetEvent。 
    //它们之间唯一的不同在于，无论何时，只要 AutoResetEvent 激活线程，它的状态将自动从终止变为非终止。 
    //相反， ManualResetEvent 允许它的终止状态激活任意多个线程，只有当它的 Reset 方法被调用时才还原到非终止状态。

    //可以通过调用 WaitOne、 WaitAny 或 WaitAll 等中的某个等待方法使线程等待事件。 
    //WaitHandle .WaitOne () 使线程一直等待，直到单个事件变为终止状态； 
    //WaitHandle .WaitAny () 阻止线程，直到一个或多个指示的事件变为终止状态； 
    //WaitHandle .WaitAll () 阻止线程，直到所有指示的事件都变为终止状态。 
    //当调用事件的 Set 方法时，事件将变为终止状态。

    //在下面的示例中，创建了一个线程，并由 Main 函数启动该线程。 新线程使用 WaitOne 方法等待一个事件。
    //在该事件被执行 Main 函数的主线程终止之前，该线程一直处于挂起状态。 一旦该事件终止，辅助线程将返回。
    //在本示例中，因为事件只用于一个线程的激活，所以使用 AutoResetEvent 或 ManualResetEvent 类都可以。

    //using System;
    //using System.Threading;

    //class ThreadingExample
    //    {
    //    static AutoResetEvent autoEvent;

    //    static void DoWork()
    //        {
    //        Console.WriteLine("   worker thread started, now waiting on event...");
    //        autoEvent.WaitOne();
    //        Console.WriteLine("   worker thread reactivated, now exiting...");
    //        }

    //    static void Main()
    //        {
    //        autoEvent = new AutoResetEvent(false);

    //        Console.WriteLine("main thread starting worker thread...");
    //        Thread t = new Thread(DoWork);
    //        t.Start();

    //        Console.WriteLine("main thread sleeping for 1 second...");
    //        Thread.Sleep(1000);

    //        Console.WriteLine("main thread signaling worker thread...");
    //        autoEvent.Set();
    //        }
    //    }

//34、Array .Resize <T> 方法  【类似于VB中的preserve】
    //将数组的大小更改为指定的新大小
    //public static void Resize<T>(ref T[] array,int newSize)
    //类型参数  T       数组元素的类型

    //参数      
    //array     类型： T[]%   要调整大小的一维数组，该数组从零开始；如果为 null 则新建具有指定大小的数组。 
    //newSize   类型： System .Int32   新数组的大小。

//35、 Microsoft.VisualBasic.FileIO.TextFieldParser .ReadLine 方法
    //将当前行作为字符串返回，并将光标前进到下一行
    //ReadLine 方法不执行分析；分隔字段中的行尾字符被解释为实际的行尾。
    //如果到达文件的结尾，则会返回 Nothing

//36、Microsoft.VisualBasic.FileIO.TextFieldParser .ReadFields 方法
    //读取当前行的所有字段，以字符串数组的形式返回这些字段，并将光标前进到包含数据的下一行。
    //为了使用户能够分析具有多种格式的文本文件， 
    //ReadFields 方法在每次调用时检查 TextFieldType、 Delimiters 和 FieldWidths 的值（如果指定了它们）。 
    //用户需要正确配置 TextFieldType 和 FieldWidths 或 Delimiters 属性。 
    //如果 TextFieldType 设为 Delimited，并且没有设置 Delimiters，或者如果 TextFieldType 设为 FixedWidth 和 FieldWidths，将引发异常。

    //如果 ReadFields 遇到空行，则跳过空行并返回下一个非空行。

    //如果 ReadFields 方法不能分析当前行，则会引发异常，而且不移到下一行。 这使您的应用程序能够尝试重新分析该行。

//37、 System.IO.StreamWriter 构造函数 (String, Boolean, Encoding)
    //使用指定编码和默认缓冲区大小，为指定路径上的指定文件初始化 StreamWriter 类的新实例。
    //如果该文件存在，则可以将其覆盖或向其追加。如果该文件不存在，则此构造函数将创建一个新文件。
    //public StreamWriter(string path,bool append,Encoding encoding)
    //参数
    //path       类型： System .String          要写入的完整文件路径。
    //append     类型： System .Boolean         确定是否将数据追加到文件。如果该文件存在，并且 append 为 false，则该文件被覆盖。
                                                //如果该文件存在，并且 append 为 true，则数据被追加到该文件中。 否则，将创建新文件。
    //encoding   类型： System.Text .Encoding   要使用的字符编码。

//38、 System.IO.StreamReader 构造函数 (String, Encoding)
    //用指定的字符编码，为指定的文件名初始化 StreamReader 类的一个新实例。
    //public StreamReader(string path,Encoding encoding)
    //参数
    //path       类型： System .String          要读取的完整文件路径。
    //encoding   类型： System.Text .Encoding   要使用的字符编码。

//39、System.Xml       命名空间	   包含与 Xml 读取和写入相关的类。
      //System.Xaml    命名空间	   提供与 XAML 读取器和 XAML 编写器相关的类型。这包括 .NET XAML 服务 XAML 读取器和 XAML 编写器的默认实现。还包含与 XAML 类型系统相关的类型以及其他支持类型。
      //System.IO      命名空间	   System.IO 命名空间包含允许读写文件和数据流的类型以及提供基本文件和目录支持的类型。
      //System.Dynamic 命名空间	   System.Dynamic 命名空间提供支持动态语言运行时的类和接口。
      //System.Device.Location    命名空间 	System.Device.Location 命名空间使应用程序开发人员可通过一个 API 方便地访问计算机的位置。 位置信息可能来自多个提供程序，例如 GPS、Wi-Fi 三角测量和移动电话塔三角测量。System.Device.Location 类提供一个 API，用于在一台计算机上封装多个位置提供程序，并支持在这些提供程序之间无缝地区分优先级和转换。 使用此 API 的应用程序开发人员无需知道特定计算机上有哪些位置感知技术，从而避免针对特定硬件配置来编写应用程序的工作负担。

                                            //GeoCoordinateWatcher 类提供基于纬度和经度坐标的位置数据。 CivicAddressResolver 和 ICivicAddressResolver 类型提供从坐标位置中解析市政地址的能力，且可以实行 IGeoPositionWatcher <T > 接口扩展支持的位置数据类型。

                                            //在 Windows 7 中，如果位置提供程序已经安装并能够确定计算机的位置，则所有 System.Device.Location 类都完全正常。 对于 Windows 7 Starter 版，唯一支持的位置提供程序是可以在控制面板中设置的默认位置提供程序。

      //System.Activities 命名空间	System.Activities 命名空间包含创建和使用活动所需的所有类。 使用此命名空间中的类，可以定义活动，定义流入和流出活动的数据以及定义变量。

//40、System.Diagnostics.Process 类	提供对本地和远程进程的访问并使您能够启动和停止本地系统进程。
    //Threads	                获取在关联进程中运行的一组线程。
    //TotalProcessorTime	    获取此进程的总的处理器时间。
    //UserProcessorTime	        获取此进程的用户处理器时间。
    //	StandardError	        获取用于读取应用程序错误输出的流。
    //StandardInput	            获取用于写入应用程序输入的流。
    //StandardOutput	        获取用于读取应用程序输出的流。
    //StartInfo	                获取或设置要传递给 Process 的 Start 方法的属性。
    //StartTime	                获取关联进程启动的时间。
    //PrivilegedProcessorTime	获取此进程的特权处理器时间。
    //ProcessName	            获取该进程的名称。
    //ProcessorAffinity	        获取或设置一些处理器，此进程中的线程可以按计划在这些处理器上运行。
    //Responding	            获取指示进程的用户界面当前是否响应的值。
    //SessionId	                获取关联的进程的终端服务会话标识符。
    //PriorityBoostEnabled	    获取或设置一个值，该值指示主窗口拥有焦点时是否由操作系统暂时提升关联进程的优先级别。
    //ExitCode	                获取关联进程终止时指定的值。
    //ExitTime	                获取关联进程退出的时间。
    //Handle	                获取关联进程的本机句柄。
    //HandleCount	            获取由进程打开的句柄数。
    //HasExited	                获取指示关联进程是否已终止的值。
    //Id	                    获取关联进程的唯一标识符。
    //MachineName	            获取关联进程正在其上运行的计算机的名称。
    //MainModule	            获取关联进程的主模块。
    //MainWindowHandle	        获取关联进程主窗口的窗口句柄。
    //MainWindowTitle	        获取进程的主窗口标题。
    //MaxWorkingSet	            获取或设置关联进程的允许的最大工作集大小。
    //MinWorkingSet	            获取或设置关联进程的允许的最小工作集大小。
    //Modules	                获取已由关联进程加载的模块。

//41、System.Diagnostics.Process .Kill 方法	  立即停止关联的进程。
    //Kill 强制终止进程，而 CloseMainWindow 只是请求终止。 有图形界面的进程在执行时，其消息循环处于等待状态。
    //每当操作系统向该进程发送 Windows 消息时，该消息循环执行。调用 CloseMainWindow 会向主窗口发送关闭请求，
    //在一个格式良好的应用程序中，该请求会关闭子窗口并撤消此应用程序所有正在运行的消息循环。 
    //通过调用 CloseMainWindow 发出的退出进程的请求不强制应用程序退出。 
    //应用程序可以在退出前请求用户验证，也可以拒绝退出。若要强制应用程序退出，请使用 Kill 方法。 
    //CloseMainWindow 的行为与用户使用系统菜单关闭应用程序主窗口的行为一样。 因此，通过关闭主窗口发出的退出进程的请求不强制应用程序立即退出。

    //Kill 方法将异步执行。 在调用 Kill 方法后，请调用 WaitForExit 方法等待进程退出，或者检查 HasExited 属性以确定进程是否已经退出。

    //如果调用 Kill，则可能丢失该进程编辑的数据或分配给该进程的资源。 Kill 导致异常的进程终止，并只在必要时才应使用。 
    //CloseMainWindow 支持进程的有序终止，并关闭所有窗口，以便更好地用于带界面的应用程序。 
    //如果 CloseMainWindow 失败，您可以使用 Kill 终止进程。 Kill 是终止没有图形化界面的进程的唯一方法。

    //只能对在本地计算机上运行的进程调用 Kill 和 CloseMainWindow。 无法使远程计算机上的进程退出。仅可查看在远程计算机上运行的进程的信息。

    //如果在进程正要终止的同时调用了 Kill 方法，则将会因访问被拒绝而引发 Win32Exception

//42、System.Diagnostics.Process .GetProcessesByName 方法 (String, String)	  创建新的 Process 组件的数组，并将它们与远程计算机上共享指定进程名称的所有进程资源关联。
    //public static Process[] GetProcessesByName(string processName,string machineName)
    //参数
    //processName    类型： System .String                   该进程的友好名称。
    //machineName    类型： System .String                   网络上计算机的名称。
    //返回值         类型： System.Diagnostics .Process []   Process 类型的数组，它表示运行指定应用程序或文件的进程资源。
  
//43、System.Diagnostics.Process .GetProcesses 方法 (String)	为指定计算机上的每个进程资源创建一个新的 Process 组件。
    //public static Process[] GetProcesses(string machineName)
    //参数
    //machineName      类型： System .String                      从其读取进程列表的计算机。
    //返回值           类型： System.Diagnostics .Process []      Process 类型的数组，它表示指定计算机上运行的所有进程资源。 

//44、Microsoft.VisualBasic.Strings .InStr 方法 (String, String, CompareMethod)	
    //返回一个整数，该整数指定一个字符串中另一个字符串的第一个匹配项的起始位置。
    //public static int InStr(string String1,string String2,CompareMethod Compare)
    //参数    
    //String1    类型： System .String                         必需。正在搜索的 String 表达式。 
    //String2    类型： System .String                         必需。查找到的 String 表达式。 
    //Compare    类型： Microsoft.VisualBasic .CompareMethod   可选。指定字符串比较的类型。如果省略 Compare，则 Option Compare 设置确定比较类型。 
    //返回值     类型： System .Int32    

//45、System.IO.File .Create 方法 (String)	在指定路径中创建或覆盖文件。
    //public static FileStream Create(string path)
    //参数
    //path      类型： System .String              要创建的文件的路径及名称。
    //返回值    类型： System.IO .FileStream       一个 FileStream，它提供对 path 中指定的文件的读/写访问。

    //由此方法创建的 FileStream 对象的 FileShare 值默认为 None；直到关闭原始文件句柄后，其他进程或代码才能访问这个创建的文件。

    //此方法等效于使用默认缓冲区大小的 Create(String, Int32) 方法重载。

    //允许 path 参数指定相对或绝对路径信息。 相对路径信息被解释为相对于当前工作目录。若要获取当前工作目录，请参见 GetCurrentDirectory。

    //如果指定的文件不存在，则创建该文件；如果存在并且不是只读的，则将覆盖其内容。

    //默认情况下，将向所有用户授予对新文件的完全读/写访问权限。文件是用读/写访问权限打开的，必须关闭后才能由其他应用程序打开。

//46、System.IO.FileStream 类	公开以文件为主的 Stream，既支持同步读写操作，也支持异步读写操作。
    //使用 FileStream 类对文件系统上的文件进行读取、写入、打开和关闭操作，并对其他与文件相关的操作系统句柄进行操作，
    //如管道、标准输入和标准输出。 可以指定读写操作是同步还是异步。FileStream 缓冲输入和输出以获得更好的性能。

    //FileStream 对象支持使用 Seek 方法对文件进行随机访问。 Seek 允许将读取/写入位置移动到文件中的任意位置。 
    //这是通过字节偏移参考点参数完成的。字节偏移量是相对于查找参考点而言的，该参考点可以是基础文件的开始、当前位置或结尾，分别由 SeekOrigin 类的三个属性表示。

    //磁盘文件始终支持随机访问。在构造时， CanSeek 属性值设置为 true 或 false，具体取决于基础文件类型。 
    //具体地说，就是当基础文件类型是 FILE_TYPE_DISK（如 winbase.h 中所定义）时， CanSeek 属性值为 true。 否则， CanSeek 属性值为 false。
    //Flush()	         清除此流的缓冲区，使得所有缓冲的数据都写入到文件中。 （重写 Stream .Flush ()。）
    //GetAccessControl	 获取 FileSecurity 对象，该对象封装当前 FileStream 对象所描述的文件的访问控制列表 (ACL) 项。
    //Lock	             防止其他进程更改 FileStream。
    //Read	             从流中读取字节块并将该数据写入给定缓冲区中。 （重写 Stream .Read( Byte [], Int32, Int32) 。）
    //ReadByte	         从文件中读取一个字节，并将读取位置提升一个字节。 （重写 Stream .ReadByte ()。）
    //Seek	             将该流的当前位置设置为给定值。 （重写 Stream .Seek(Int64, SeekOrigin) 。）
    //SetAccessControl	 将 FileSecurity 对象所描述的访问控制列表 (ACL) 项应用于当前 FileStream 对象所描述的文件。
    //SetLength	         将该流的长度设置为给定值。 （重写 Stream .SetLength(Int64) 。）
    //Unlock	         允许其他进程访问以前锁定的某个文件的全部或部分。
    //Write	             使用从缓冲区读取的数据将字节块写入该流。 （重写 Stream .Write( Byte [], Int32, Int32) 。）
    //WriteByte	         将一个字节写入文件流的当前位置。 （重写 Stream .WriteByte(Byte) 。）

//47、二维数组操作：在定义时只能先声明第一维度的大小，然后再逐个定义第二维度的大小，如下：
    //string[][] ttt = new string[20][];
    //for (int a = 0; a < 20; a++)
    //    {
    //    ttt[a] = new string[3]; iii[a] = new int[3];
    //    for (int b = 0; b < 3; b++)
    //        {
    //        ttt[a][b] = a.ToString() + " " + b.ToString();
    //        }
    //    }









namespace PengDongNanTools
    {
    class 重要提示
        {

        /// <summary>
        /// 提示信息
        /// </summary>
        public string ErrorMessage = "";

        private bool SuccessBuiltNew = false, PasswordIsCorrect = false;

        /// <summary>
        /// 软件作者
        /// </summary>
        public string Author
            {
            get { return "【软件作者：彭东南, southeastofstar@163.com】"; }
            }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public 重要提示(string DLLPassword)
            {

            SuccessBuiltNew = false;
            PasswordIsCorrect = false;

            try
                {

                if (DLLPassword == "ThomasPeng" || (DLLPassword == "pengdongnan") 
                    || (DLLPassword == "彭东南"))
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
        
        //System.Windows.Forms.Control xx;
        //System.Windows.Forms.Button aa = new Button();
        //xx = aa;

        //System.Collections.Generic.Dictionary<string, System.Net.Sockets.TcpClient> xx = new Dictionary<string, System.Net.Sockets.TcpClient>();
        //if (xx != null) 
        //    {
        //    foreach (System.Net.Sockets.TcpClient a in xx.Values) 
        //        {
        //        //发送内容的代码部分
        //        }
        //    }

        public int Test()
            {
            try
                {


                int Return = 0;

                return Return;
                }
            catch (Exception ex)
                {
                ErrorMessage = ex.Message;
                return 1;
                }
            }
        
        #region ""



        #endregion

        //数据包结构体
        /// <summary>
        /// 数据包结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct Package
            {
            /// <summary>
            /// 确定为命令包的标识
            /// </summary>
            public int commandFlag;
            /// <summary>
            /// 命令
            /// </summary>
            public int command;
            /// <summary>
            ///数据长度（数据段不包括包头）
            /// </summary>
            public int dataLength;
            /// <summary>
            /// 通道编号
            /// </summary>
            public short channelNo;
            /// <summary>
            /// 块编号
            /// </summary>
            public short blockNo;
            /// <summary>
            /// 开始标记
            /// </summary>
            public int startFlag;
            /// <summary>
            /// 结束标记0x0D0A为结束符
            /// </summary>
            public int finishFlag;
            /// <summary>
            /// 校验码
            /// </summary>
            public int checksum;
            /// <summary>
            /// 保留 char数组，SizeConst表示数组个数，在转成
            /// byte数组前必须先初始化数组，再使用，初始化
            /// 的数组长度必须和SizeConst一致，例：test=new char[4];
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] reserve;
            }

        //Byte数组转结构体
        /// <summary>
        /// Byte数组转结构体
        /// </summary>
        /// <param name="bytes">byte数组</param>
        /// <param name="type">结构体类型</param>
        /// <returns>转换后的结构体</returns>
        public static object BytesToStuct(byte[] bytes, Type type)
            {
            //得到结构体的大小
            int size = Marshal.SizeOf(type);
            //byte数组长度小于结构体的大小
            if (size > bytes.Length)
            { return null; }
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, structPtr, size);
            object obj = Marshal.PtrToStructure(structPtr, type);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            return obj;
            }

        }//class
    }//namespace
