using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using Microsoft.Practices.Unity;
using SFO2O.EntLib.DataExtensions.Basic;

namespace SFO2O.EntLib.DataExtensions.DataMapper
{
    public static class Mapping
    {
        internal static void Register<T>(IMapper<T> mapper)
        {
            DataServiceLocator.Container.RegisterInstance<IMapper<T>>(mapper);
        }

        /// <summary>
        /// 获取IMapper&lt;<typeparamref name="T"/>&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IMapper<T> GetIMapper<T>()
        {
            try
            {
                return DataServiceLocator.Current.GetInstance<IMapper<T>>();
            }
            catch (Microsoft.Practices.ServiceLocation.ActivationException)
            {
                try
                {
                    try
                    {
                        object obj = Activator.CreateInstance<T>();
                    }
                    catch
                    {
                        // 这是一个与实体的约定，所有的无构造函数或构造函数为私有的实体必须实现的一个静态方法用于创建对象。
                        typeof(T).GetMethod("Create").Invoke(null, new object[] { });
                    }
                    return DataServiceLocator.Current.GetInstance<IMapper<T>>();
                }
                catch (Exception ex)
                {
                    throw new MapperException(string.Format("获取对象的架构信息时发生意外的错误，请确认[{0}]的架构信息或实体的约定。", typeof(T).FullName), ex);
                }
            }
        }
    }
}
