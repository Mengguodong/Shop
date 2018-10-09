using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System.Data;

namespace SFO2O.EntLib.DataExtensions.DataMapper
{
    /// <summary>
    /// 对象映射
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMapper<T>
    {
        /// <summary>
        /// 获取对象指定成员对象的值
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        IPropertyValue<T> this[string field] { get; }
        /// <summary>
        /// 获取对象架构映射信息
        /// </summary>
        /// <returns></returns>
        BaseSchema Schema { get; }
        /// <summary>
        /// 创建一个类型为<see cref="T"/>实体对象
        /// </summary>
        /// <returns></returns>
        T Create();
    }

    /// <summary>
    /// 内部数据映射的实现。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ObjectMapper<T> : IMapper<T>
    {
        private readonly BaseSchema _schema;
        private readonly IPropertyValue<T>[] _propertyValues;
        private readonly Func<T> _creator;
        private readonly Type _Type;

        public ObjectMapper(ObjectSchema<T> schema)
        {
            _schema = schema;
            if (!_schema.IsReadonly)
                _schema.Compile();

            _propertyValues = new IPropertyValue<T>[schema.FieldMaps().Count];

            foreach (FieldMap<T> fieldMap in schema.FieldMaps())
            {
                _propertyValues[schema.IndexOf(fieldMap.Field)] = new PropertyValueForLambda<T>(fieldMap.Expression);
            }
            //

            if (this._schema.Name == null)
            {
                BlockExpression be = BlockExpression.Block(Expression.New(typeof(T)));
                _creator = Expression.Lambda<Func<T>>(be).Compile();
            }
            else
            {
                _Type = EntityBuilder.BuilderEntityClass<T>(schema.FieldMaps().Select(x => (FieldMap<T>)x), schema);
                BlockExpression be = BlockExpression.Block(Expression.New(_Type));
                _creator = Expression.Lambda<Func<T>>(be).Compile();
            }
        }

        public IPropertyValue<T> this[string field]
        {
            get
            {
                int i = _schema.IndexOf(field);
                return i > -1 ? _propertyValues[i] : null;
            }
        }

        public BaseSchema Schema
        {
            get
            {
                return _schema;
            }
        }

        public T Create()
        {
            return _creator();
        }
    }

}
