using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace SFO2O.EntLib.DataExtensions.Basic
{
    /// <summary>
    /// 本地容器
    /// </summary>
    internal static class DataServiceLocator
    {
        private static readonly IServiceLocator _locator;
        private static readonly IUnityContainer _container;
        /// <summary>
        /// 窗口对象，通过它获取接口的实例。
        /// </summary>
        public static IServiceLocator Current { get { return _locator; } }
        /// <summary>
        /// 对象管理容器
        /// </summary>
        internal static IUnityContainer Container { get { return _container; } }

        static DataServiceLocator()
        {
            _container = new UnityContainer();
            _locator = new UnityServiceLocator(_container);
        }
    }
}
