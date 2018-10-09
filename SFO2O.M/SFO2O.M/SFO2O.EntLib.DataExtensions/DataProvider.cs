using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SFO2O.EntLib.DataExtensions.DataMapper;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace SFO2O.EntLib.DataExtensions
{
    /// <summary>
    /// 数据源管理提供器，将Data模块中的数据源进行整合。为子类简单快捷的提供各种对象操作，其所有的成员方法和属性全部为受保护级别。
    /// </summary>
    public abstract class DataProvider
    {
        /// <summary>
        /// 只读库链接字符串名称
        /// </summary>
        protected abstract string DbReadonlyConnection
        {
            get;
        }

        /// <summary>
        /// 只写库链接字符串名称
        /// </summary>
        protected abstract string DbWriteonlyConnection
        {
            get;
        }

        protected DataProvider()
        {
        }

        #region create

        /// <summary>
        /// 创建一个只读库的数据库对象
        /// </summary>
        /// <returns></returns>
        protected Database CreateDbReader()
        {
            return CreateDbReader(this.DbReadonlyConnection);
        }

        /// <summary>
        /// 创建一个只读库的数据库对象
        /// </summary>
        /// <returns></returns>
        protected Database CreateDbReader(string connectionName)
        {
            return EnterpriseLibraryContainer.Current.GetInstance<Database>(connectionName);
        }

        /// <summary>
        /// 创建一个只写库的数据库对象
        /// </summary>
        /// <returns></returns>
        protected Database CreateDbWriter()
        {
            return CreateDbWriter(this.DbWriteonlyConnection);
        }

        /// <summary>
        /// 创建一个只写库的数据库对象
        /// </summary>
        /// <returns></returns>
        protected Database CreateDbWriter(string connectionName)
        {
            return EnterpriseLibraryContainer.Current.GetInstance<Database>(connectionName);
        }

        #endregion

    }
}
