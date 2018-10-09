using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using SFO2O.EntLib.DataExtensions.Basic;

namespace SFO2O.EntLib.DataExtensions.DataMapper.Schema
{
    /// <summary>
    /// 数据架构映射
    /// </summary>
    public abstract class BaseSchema
    {
        private readonly SafeDictionary<string, FieldMap> _Fields;

        /// <summary>
        /// 对象名称(表名)
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// 是否为中读
        /// </summary>
        public bool IsReadonly
        {
            get;
            protected set;
        }

        /// <summary>
        /// 成员字段的个数
        /// </summary>
        public short FieldCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 主键个数
        /// </summary>
        public int KeyCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 增量标识
        /// </summary>
        public Identity Identity
        {
            get;
            protected set;
        }

        public BaseSchema()
        {
            _Fields = new SafeDictionary<string, FieldMap>();
        }

        private string[] _KeyFields;
        /// <summary>
        /// 主键成员
        /// </summary>
        public string[] GetKeyFields()
        {
            return (string[])_KeyFields.Clone();
        }

        private string[] _AllFields;
        /// <summary>
        /// 数据架构中所有的成员字段
        /// </summary>
        public string[] GetAllFields()
        {
            return (string[])_AllFields.Clone();
        }

        /// <summary>
        /// 获取指定成员字段的固定编号(编号从0开始，是一组有序且小于FieldCount的编号)
        /// </summary>
        /// <param name="field">成员字段</param>
        /// <returns>返回-1，表示field在DataMapSchema中不存在。</returns>
        public short IndexOf(string field)
        {
            var _field = _Fields[field];
            return _field == null ? (short)-1 : _field.Index;
        }

        /// <summary>
        /// 验证指定的成员字段是否是主键
        /// </summary>
        /// <param name="field">成员字段名称</param>
        /// <returns></returns>
        public bool IsKey(string field)
        {
            FieldMap fieldMap = _Fields[field];
            if (fieldMap != null)
            {
                return fieldMap.IsKey;
            }
            return false;
        }

        /// <summary>
        /// 增加一个成员字段
        /// </summary>
        /// <param name="fieldMap">成员字段的映射信息</param>
        /// <returns></returns>
        protected FieldMap AddField(FieldMap fieldMap)
        {
            Check.IsNull(fieldMap, "fieldMap");
            Check.Valid(_Fields.ContainsKey(fieldMap.Field), "字段成员名称重复。");

            ValidateReadonly();
            _Fields.Add(fieldMap.Field, fieldMap);
            return fieldMap;
        }

        /// <summary>
        /// 获取一个指定的成员字段名称
        /// </summary>
        /// <param name="index">成员字段编号</param>
        /// <returns></returns>
        public string FieldName(short index)
        {
            if (index < 0 || index >= _AllFields.Length)
                return null;
            else
                return _AllFields[index];
        }

        /// <summary>
        /// 获取一个指定的成员字段的映射信息
        /// </summary>
        /// <param name="field">成员字段名称</param>
        /// <returns></returns>
        public FieldMap FieldMap(string field)
        {
            return _Fields[field];
        }

        /// <summary>
        /// 获取所有的成员字段的映射信息
        /// </summary>
        /// <returns></returns>
        public IList<FieldMap> FieldMaps()
        {
            return _Fields.Values.ToList();
        }

        /// <summary>
        /// 只读状态验证
        /// </summary>
        protected void ValidateReadonly()
        {
            Check.Valid(IsReadonly, "只读状态下的DataMapSchema不可以做修改。");
        }

        /// <summary>
        /// 编译DataMapSchema信息,编译后的DataMapSchema将被切换至只读状态
        /// </summary>
        public virtual void Compile()
        {
            if (IsReadonly)
                return;

            List<string> keys = new List<string>();
            List<string> fields = new List<string>();
            foreach (FieldMap fieldMap in FieldMaps())
            {
                // 获取主键
                if (fieldMap.IsKey)
                    keys.Add(fieldMap.Field);

                // 获取所有的成员字段
                fields.Add(fieldMap.Field);

                // 给成员字段编号(编号从[0]开始)
                fieldMap.SetIndexOf((short)(fields.Count - 1));
            }
            _KeyFields = keys.ToArray();
            _AllFields = fields.ToArray();
            FieldCount = (short)fields.Count;
            KeyCount = keys.Count;

            //
            this.IsReadonly = true;
        }
    }
}
