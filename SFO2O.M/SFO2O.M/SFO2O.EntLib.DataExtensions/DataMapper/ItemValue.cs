using System;

namespace SFO2O.EntLib.DataExtensions.DataMapper
{
    /// <summary>
    /// 键和值
    /// </summary>
    [Serializable]
    public struct ItemValue
    {
        /// <summary>
        /// Item名称
        /// </summary>
        private string m_Item;

        /// <summary>
        /// Item名称
        /// </summary>
        public string Item
        {
            get { return m_Item; }
            set { m_Item = value; }
        }

        /// <summary>
        /// Item的值
        /// </summary>
        private object m_Value;

        /// <summary>
        /// Item的值
        /// </summary>
        public object Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="item">字段</param>
        /// <param name="value">值</param>
        public ItemValue(string item, object value)
        {
            m_Item = item;
            m_Value = value;
        }

        public override string ToString()
        {
            return string.Concat("[", m_Item, ":", m_Value, "]");
        }
    }
    /// <summary>
    /// 键和值
    /// </summary>
    /// <typeparam name="TItem">键类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    [Serializable]
    public struct ItemValue<TItem, TValue>
    {
        /// <summary>
        /// Item名称
        /// </summary>
        private TItem m_Item;

        /// <summary>
        /// Item名称
        /// </summary>
        public TItem Item
        {
            get { return m_Item; }
            set { m_Item = value; }
        }

        /// <summary>
        /// Item的值
        /// </summary>
        private TValue m_Value;

        /// <summary>
        /// Item的值
        /// </summary>
        public TValue Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="item">字段</param>
        /// <param name="value">值</param>
        public ItemValue(TItem item, TValue value)
        {
            m_Item = item;
            m_Value = value;
        }

        public override string ToString()
        {
            return string.Concat("[", m_Item, ":", m_Value, "]");
        }
    }
}
