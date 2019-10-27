using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LimitQueue
{
    /// <summary>
    /// 有限长度队列类【FIFO - 先进先出】：在初始化时指定队列长度，后续插入数据时，超过此长度的最旧的一条数据会被丢弃
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    public class LimitQueue<Type> : System.Collections.Generic.Queue<Type>//
    {
        private int iLimitCountOfTheQuene = 10;

        /// <summary>
        /// 有限长度队列类的构造函数
        /// </summary>
        /// <param name="LimitCount">指定队列的长度，后续插入数据时，超过此长度的最旧的一条数据会被丢弃</param>
        public LimitQueue(int LimitCount)
        {
            if (LimitCount > 0)
            {
                iLimitCountOfTheQuene = LimitCount;
            }
            else
            {
                MessageBox.Show("队列的最大长度值不能小于或等于0，已使用默认长度值：10.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //base.Enqueue
        }

        /// <summary>
        /// 将对象添加到 System.Collections.Generic.Queue 的结尾处。
        /// </summary>
        /// <param name="item">要添加到 System.Collections.Generic.Queue 的对象。对于引用类型，该值可以为 null。</param>
        public new void Enqueue(Type item)
        {
            if (base.Count < iLimitCountOfTheQuene)
            {
                base.Enqueue(item);
            }
            else
            {
                base.Dequeue();
                base.Enqueue(item);
            }
        }

        // 保留原有类的 Dequeue() 函数，更有灵活性
        ///// <summary>
        ///// 此函数不起实际作用，只是为了重载 System.Collections.Generic.Queue<T> 的 public T Dequeue()，避免人为删除队列
        ///// </summary>
        ///// <returns>T 类型的默认值</returns>
        //public new Type Dequeue()
        //{
        //    return default(Type);
        //}

        /// <summary>
        /// 重载 System.Collections.Generic.Queue 的 public T Dequeue()：返回最早送入队列的值(T类型)，在执行Dequeue () 前要判断属性 Count 不为0，否则会抛出异常
        /// </summary>
        /// <returns>返回最早送入队列的值(T类型)</returns>
        public new Type Dequeue()
        {
            //if (base.Count <= 0)
            //{
            //    return default(Type);
            //}
            return base.Dequeue();
        }

        /// <summary>
        /// 获取队列中 System.Collections.Generic.Queue 元素的数组，可以对每个元素进行索引
        /// </summary>
        public Type[] Elements
        {
            get { return base.ToArray(); }
        }

    }// class

}//namespace