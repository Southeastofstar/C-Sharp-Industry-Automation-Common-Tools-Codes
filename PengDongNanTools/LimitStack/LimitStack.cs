using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LimitStack
{
    /// <summary>
    /// 有限长度堆栈类【LIFO - 后进先出】：在初始化时指定堆栈长度，后续插入数据时，超过此长度的最旧的一条数据会被丢弃
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    public class LimitStack<Type> : System.Collections.Generic.Stack<Type>//
    {
        private int iLimitCountOfTheQuene = 10;

        /// <summary>
        /// 有限长度堆栈类的构造函数
        /// </summary>
        /// <param name="LimitCount">指定堆栈的长度，后续插入数据时，超过此长度的最新的一条数据会被丢弃</param>
        public LimitStack(int LimitCount)
        {
            if (LimitCount > 0)
            {
                iLimitCountOfTheQuene = LimitCount;
            }
            else
            {
                MessageBox.Show("堆栈的最大长度值不能小于或等于0，已使用默认长度值：10.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 将对象添加到 System.Collections.Generic.Stack 的顶部。
        /// <param name="item">要添加到 System.Collections.Generic.Stack 顶部的对象。对于引用类型，该值可以为 null。</param>
        /// </summary>
        public new void Push(Type item)
        {
            if (base.Count < iLimitCountOfTheQuene)
            {
                base.Push(item);
            }
            else
            {
                base.Pop();
                base.Push(item);
            }
        }

        /// <summary>
        /// 重载 System.Collections.Generic.Stack 的 public T Pop()：返回最后压入堆栈的值(T类型)，在执行 Pop() 前要判断属性 Count 不为0，否则会抛出异常
        /// </summary>
        /// <returns>返回最后压入堆栈的值(T类型)</returns>
        public new Type Pop()
        {
            return base.Pop();
        }

        /// <summary>
        /// 获取堆栈中 System.Collections.Generic.Stack 元素的数组，可以对每个元素进行索引
        /// </summary>
        public Type[] Elements
        {
            get { return base.ToArray(); }
        }

    }// class

}//namespace