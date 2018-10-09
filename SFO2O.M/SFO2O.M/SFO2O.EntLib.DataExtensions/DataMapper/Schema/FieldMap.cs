using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using SFO2O.EntLib.DataExtensions.Basic;

namespace SFO2O.EntLib.DataExtensions.DataMapper.Schema
{
    /// <summary>
    /// 成员字段描述
    /// </summary>
    public abstract class FieldMap
    {
        private readonly BaseSchema _Owner;
        /// <summary>
        /// 隶属的DataSchema
        /// </summary>
        public BaseSchema Owner
        {
            get { return this._Owner; }
        }

        private readonly string _Field;
        /// <summary>
        /// 映射的成员字段
        /// </summary>
        public string Field
        {
            get { return _Field; }
        }

        private readonly bool _IsKey;
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsKey
        {
            get { return _IsKey; }
        }

        private short _index;
        /// <summary>
        /// 成员字段编号
        /// </summary>
        public short Index
        {
            get { return _index; }
        }

        protected FieldMap(BaseSchema schema, string field, bool isKey)
        {
            this._Owner = schema;
            this._Field = field;
            this._IsKey = isKey;
        }

        /// <summary>
        /// 设置成员字段的编号
        /// </summary>
        /// <param name="index"></param>
        internal void SetIndexOf(short index)
        {
            _index = index;
        }
    }
    /// <summary>
    /// 成员字段描述
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FieldMap<T> : FieldMap
    {
        /// <summary>
        /// 实体类型成员映射的原始Lambda表达式
        /// </summary>
        public Expression<Func<T, object>> Expression { get; private set; }

        /// <summary>
        /// 增量标识
        /// </summary>
        public Identity Identity
        {
            get;
            private set;
        }

        internal FieldMap(BaseSchema schema, Expression<Func<T, object>> exp, string field, bool isKey)
            : base(schema, field, isKey)
        {
            this.Expression = exp;
        }

        /// <summary>
        /// 创建增量标识
        /// </summary>
        /// <returns></returns>
        public Identity CreateIdentity()
        {
            Check.Valid(Owner.IsReadonly, "只读状态下的可以创建增量标识。");

            this.Identity = new Identity(this.Field);
            return this.Identity;
        }
    }
}
