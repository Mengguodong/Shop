using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace SFO2O.EntLib.DataExtensions.DataMapper
{
    /// <summary>
    /// 实体对象的取值或赋值
    /// </summary>
    public interface IPropertyValue<T>
    {
        /// <summary>
        /// 向对象的目标属性赋值
        /// </summary>
        /// <param name="handler">引用的对象</param>
        /// <param name="value">将<paramref name="value"/>赋值给对象的属性</param>
        void SetValue(T handler, object value);
        /// <summary>
        /// 获取对象的目标属性的值
        /// </summary>
        /// <param name="handler">引用的对象</param>
        /// <returns></returns>
        object GetValue(T handler);
    }

    /// <summary>
    /// 实体对象内部的取值或赋值实现。此对象通过Lambda表达示树编译成委托实现对象属性的取值或赋值。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class PropertyValueForLambda<T> : IPropertyValue<T>
    {

        Func<T, object> _Getter;
        Action<T, object> _Setter;

        public PropertyValueForLambda(Expression<Func<T, object>> field)
        {
            // 获取属性的值的方法
            _Getter = field.Compile();

            // 向属性赋值的方法
            // Modify（严宇坤）at 2012-04-25 : 扩展，可从数据库映射到嵌套对象的属性 
            var memberExp = EntityBuilder.ExtractMemberExpression<T>(field);
            var param1 = field.Parameters[0];
            var param2 = Expression.Parameter(typeof(object), "value");

            var assignExp = Expression.Assign(
                    Expression.Property(memberExp.Expression, (PropertyInfo)memberExp.Member),
                    Expression.Call(Expression.Constant(this), "To", new Type[] { ((PropertyInfo)memberExp.Member).PropertyType }, new ParameterExpression[] { param2 })
                );
            _Setter = Expression.Lambda<Action<T, object>>(assignExp, param1, param2).Compile();
        }

        public void SetValue(T handler, object value)
        {
            _Setter(handler, value);
        }

        public object GetValue(T handler)
        {
            return _Getter(handler);
        }

        private TPropertyType To<TPropertyType>(object value)
        {

            if (value is TPropertyType)
                return (TPropertyType)value;

            if (value == DBNull.Value || value == null)
                return default(TPropertyType);

            if (typeof(TPropertyType).IsEnum)
            {
                return (TPropertyType)Enum.Parse(typeof(TPropertyType), value.ToString());
            }

            // Modify（严宇坤）at 2012-04-25 : 扩展，可从数据库映射到Nullabe<Enum>类型的属性中
            var enumTypeOfNullableEnum = Nullable.GetUnderlyingType(typeof(TPropertyType));
            if (enumTypeOfNullableEnum != null && enumTypeOfNullableEnum.IsEnum)
            {
                return (TPropertyType)Enum.Parse(enumTypeOfNullableEnum, value.ToString(), true);
            }

            try
            {
                return (TPropertyType)value;
            }
            catch
            {
                return (TPropertyType)Convert.ChangeType(value, typeof(TPropertyType));
            }
        }
    }
}
