# AutomationCommonToolsCodes
EPSON Robot Remote TCP/IP Control,Motion Control(GoogolTech/LeadShine),Advantech(PCI1752/PCI1754),Keyence Laser,RS232C,RS485,TCP/IP Server/Client(Sync/Async),OPC Client/Server,CAN, EtherCAT,XML,Excel,MYSQL/ACCESS Database,Common functions,Delay(time), etc.

C#开发的工业自动化控制类，大部分已经写完且验证过:

一、EPSON Robot Remote TCP/IP Control: 用于对EPSON机械手进行远程以太网控制，建立以太网通讯后发送命令给爱普生机械手控制器，然后依照返回的命令处理相应结果;

二、Motion Control(GoogolTech/LeadShine):【二次封装】固高GTS系列运动控制卡运动卡控制类：初始化运动卡和更新IO,控制固高运动控制器轴运动;这是对固高和雷赛运动控制卡的DLL文件中的函数进行第二次封装，使在实际编程的时候更加方便、快捷；

三、Advantech:

PCI1752:研华数据采集卡PCI1752更新输出及输出状态回读;

PCI1754:研华数据采集卡PCI1754更新输入信号;

四、Keyence Laser:基恩士激光读取数据;

五、RS232C:RS232C串口通讯，包括界面类；

六、RS485:RS485串口通讯，还没有完成；

七、TCP/IP Server/Client(Sync/Async)：以太网通讯客户端和服务器端，支持同步和异步通讯;

八、OPC Client/Server：还没有完成；

九、CAN, EtherCAT：还没有完成；

十、XML：XML文件操作，包括写和读取的相关实用函数；

十一、Excel：Excel文件的相关操作，包括快速写大量数据到EXCEL文件；其中处理条件格式的代码还在调试中；

十二、MYSQL/ACCESS Database：数据库操作；

十三、Common functions：操作系统和文件相关操作、获取TcpClient连接的本地/对方IP地址和端口、通用TCP/IP函数、跨线程安全的委托、进制转换、通用RS232C函数；

十四、Delay(time)：延时计时器，等待指定时间，如果计算机不支持高性能计数器，则只支持整数秒的时间

十五、ListView：针对ListView控件的各项增加、删除、查找操作；
