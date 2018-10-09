using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SFO2O.EntLib.DataExtensions.Basic
{
    /// <summary>
    /// 一组类型<typeparamref name="T"/>的实例对象集合，均衡的向外提供<typeparamref name="T"/>实例对象。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pools<T>
    {
        private readonly T[] _List;
        private int _Next = -1;

        public Pools(IEnumerable<T> values)
        {
            if (values == null || values.Count() == 0)
                throw new ArgumentException("参数values不可以为空，且values集合中的集合数不能小于1。", "values");

            _List = values.ToArray();
        }

        /// <summary>
        /// 获取共享池中的所有数据
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            return (T[])_List.Clone();
        }

        /// <summary>
        /// 获取列表其中的一个 T
        /// </summary>
        /// <returns></returns>
        public T NextValue()
        {
            if (_List.Length == 1)
                return _List[0];

        begin:
            // 保证多线程安全的“加法运算”
            int index = Interlocked.Add(ref _Next, 1);
            if (index < 0 || index >= _List.Length)
            {
                // 将_Next置为-1，重新开始。
                if (_Next >= _List.Length)
                    Interlocked.Exchange(ref _Next, -1);
                /**
                 * 以上的代码，在多线程上的均衡分配时还是会有些问题的。
                 * 因为Add和Exchange方法不是原子级的方法。有可能出现多线程一起更新 _Next将其置为 -1，
                 *      在这样的情况下_List[0]的使用频率会远远高出其它对象的使用频率。但是当前可以保证多线程下是安全且不会使用_List下标越界。
                 */
                goto begin; // 重新计算
            }
            T t = _List[index];
            return t;
        }
    }
}
