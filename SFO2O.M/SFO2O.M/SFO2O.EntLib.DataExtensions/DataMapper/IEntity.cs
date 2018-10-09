using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.EntLib.DataExtensions.DataMapper
{
    /// <summary>
    /// 数据实体映射接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 数据状态
        /// </summary>
        DataState OperationState { get; set; }
        
        /// <summary>
        /// 增量标识
        /// </summary>
        Identity Identity { get; }
        
        /// <summary>
        /// 对象名称（数据库表名）
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获取或设置成员字段的值
        /// </summary>
        /// <param name="field">成员字段</param>
        /// <returns>当field不存在时返回空。</returns>
        object this[string field] { get; set; }

        #region Fields

        /// <summary>
        /// 对象成员主键字段
        /// </summary>
        string[] KeyFields { get; }
        /// <summary>
        /// 对象成员字段（包含主键字段）
        /// </summary>
        string[] Fields { get; }
        /// <summary>
        /// 返回bool值，实体对象的成员属性是否有过赋值操作
        /// </summary>
        bool ValueModified { get; }
        /// <summary>
        /// 被赋值过的成员字段
        /// </summary>
        IEnumerable<string> GetModifiedFields();
        /// <summary>
        /// 验证成员字段是否作为主键使用
        /// </summary>
        /// <param name="field">成员字段名称</param>
        /// <returns></returns>
        bool IsPrimaryKey(string field);

        #endregion

        /// <summary>
        /// 获取成员字段的值
        /// </summary>
        /// <param name="fields">成员字段（字段名称）</param>
        /// <returns></returns>
        IEnumerable<ItemValue> GetValues(IEnumerable<string> fields);

        /// <summary>
        /// 将实体状态置为数据导入中
        /// </summary>
        void Importing();
        /// <summary>
        /// 将实体状态置为数据导入结束
        /// </summary>
        void Imported();
    }

    /// <summary>
    /// 用于记录成员字段是否被修改过。成员字段有一个下标编号，这是一个在编译时确定从零开始的编码。
    /// <remarks>这种做法有利于节省空间，带来性能上的提高(内部采用位运算实现)。依据字段的多少来难写空间的开销（计算方法：(size / 8 + ((size % 8) > 0 ? 1 : 0))），此公式可以计算出成员字段需用多少个字节来做记录。</remarks>
    /// </summary>
    public struct IndexInscriber
    {
        public const byte itemSize = 8;

        private byte[] _values;
        private short _Size;

        /// <summary>
        /// 初始化一个可以存放大数据量的数组。
        /// </summary>
        /// <param name="size">二进制位大小，size = 8 * 返回值数组长度。因为ushort占用两个字节，所有二进制位大小为8。</param>
        public IndexInscriber(short size)
        {
            _Size = size;
            _values = new byte[(size / itemSize + ((size % itemSize) > 0 ? 1 : 0))];
        }

        /// <summary>
        /// 计算<paramref name="index"/>在数组中的位置，并返回<paramref name="index"/>对应的比特的值。
        /// </summary>
        /// <param name="index">二进制位</param>
        /// <param name="bitValue">数组下标。一个很大的值需要拆分成一个数组保存，如：参数<paramref name="index"/>等于 9 它大于8了，所以这个值会放在数组下标为1的对象中的第0个位置。</param>
        /// <returns>计算出的这个比特位的值</returns>
        private byte IndexBitValue(short index, out int arrayIndex)
        {
            byte bitVal;
            if (index >= itemSize)
            {
                arrayIndex = index / itemSize;
                bitVal = (byte)(1 << (index - (itemSize << (arrayIndex - 1))));
            }
            else
            {
                arrayIndex = 0;
                bitVal = (byte)(1 << index);
            }
            return bitVal;
        }

        public bool HasValue()
        {
            for (int i = 0; i < _values.Length; i++)
            {
                if (_values[i] > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取当前字段成员被修改的数量
        /// </summary>
        /// <returns></returns>
        public short ValueCount()
        {
            short count = 0;
            for (short i = 0; i < _Size; i++)
            {
                if (HasValue(i))
                    count++;
            }
            return count;
        }

        /// <summary>
        /// 被修改的成员字段的下标
        /// </summary>
        /// <returns></returns>
        public short[] ValueIndex()
        {
            IList<short> list = new List<short>(_Size);
            for (short i = 0; i < _Size; i++)
            {
                if (HasValue(i))
                    list.Add(i);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 验证指定的二进制位是否有值
        /// </summary>
        /// <param name="index">比特位，成员字段下标。</param>
        /// <returns>返回true表示修改过，false表示仍未修改。</returns>
        public bool HasValue(short index)
        {
            int arrayIndex;
            byte bitVal = IndexBitValue(index, out arrayIndex);
            return (_values[arrayIndex] & bitVal) > 0;
        }

        /// <summary>
        /// 在指定的二进制位上设置上一个值
        /// </summary>
        /// <param name="index">比特位，成员字段下标。</param>
        public void SetValue(short index)
        {
            int arrayIndex;
            byte bitVal = IndexBitValue(index, out arrayIndex);
            _values[arrayIndex] = (byte)(_values[arrayIndex] | bitVal);
        }

        /// <summary>
        /// 将记录的值清零
        /// </summary>
        public void Clean()
        {
            // 将成员字段是否有修改过的记录痕迹清除
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = 0;
            }
        }
    }
}
