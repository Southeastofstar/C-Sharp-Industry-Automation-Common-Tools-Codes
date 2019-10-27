#region "using"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

#endregion

#region "Marshal类"

    //Marshal 类 - System.Runtime.InteropServices
    //提供了一个方法集，这些方法用于分配非托管内存、复制非托管内存块、将托管类型转换为非托管类型，
    //此外还提供了在与非托管代码交互时使用的其他杂项方法。
    //Marshal 类中定义的 static 方法对于处理非托管代码至关重要。 此类中定义的大多数方法通常由需要
    //在托管和非托管编程模型之间提供桥梁的开发人员使用。
    //例如， StringToHGlobalAnsi 方法将 ANSI 字符从指定的字符串（在托管堆中）复制到非托管堆中的缓冲区。
    //该方法还分配大小正确的目标堆。
    //Marshal 类中的 Read 和 Write 方法支持对齐和未对齐的访问。

#endregion

namespace PengDongNanTools
    {
    class MarshalOperation
        {

        // This is a platform invoke prototype. SetLastError is true, which allows 
        // the GetLastWin32Error method of the Marshal class to work correctly.    
        [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
        static extern Boolean CloseHandle(IntPtr h);


        }
    }
