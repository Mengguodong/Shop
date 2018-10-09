using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SFO2O.Framework.Uitl
{
    public static class ClassMapHelper
    {
        /// <summary>
        /// 父类转化为子类
        /// </summary>
        /// <param name="baseClass"></param>
        /// <param name="childClass"></param>
        /// <returns></returns>
        public static object BaseToChildClass(object baseClass, object childClass)
        {
            foreach (PropertyInfo baseProp in baseClass.GetType().GetProperties())
            {
                foreach (PropertyInfo childProp in childClass.GetType().GetProperties())
                {
                    if (baseProp.Name == childProp.Name)
                        childProp.SetValue(childProp, baseProp.GetValue(baseClass, null), null);
                }
            }
            return childClass;
        }

        /// <summary>
        /// 父类转化为子类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseClass"></param>
        /// <returns></returns>
        public static T BaseToChildClass<T>(object baseClass)
        {
            Type type = typeof(T);
            T t = (T)type.Assembly.CreateInstance(type.FullName);

            foreach (var baseProp in baseClass.GetType().GetProperties())
            {
                var childProperties = type.GetProperties().Where(p => p.Name.ToLower() == baseProp.Name.ToLower());
                var propertyInfos = childProperties as IList<PropertyInfo> ?? childProperties.ToList();
                if (propertyInfos.Any())
                {
                    try
                    {
                        PropertyInfo pi = propertyInfos.First();
                        pi.SetValue(t, baseProp.GetValue(baseClass, null), null);
                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }
            }

            return t;
        }
    }
}