using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFO2O.EntLib.DataExtensions.DataMapper;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using SFO2O.EntLib.DataExtensions.Basic;

namespace SFO2O.EntLib.DataExtensions
{
    public static partial class DatabaseExtensions
    {
        /// <summary>
        /// 创建一个参数集合
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <returns></returns>
        public static ParameterCollection CreateParameterCollection(this Database db)
        {
            return new ParameterCollection(db);
        }

        /// <summary>
        /// 创建一个参数集合
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="capacity">集合默认空间分配大小</param>
        /// <returns></returns>
        public static ParameterCollection CreateParameterCollection(this Database db, int capacity)
        {
            return new ParameterCollection(db, capacity);
        }

        /// <summary>
        /// 创建一个Sql批处理对象
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <returns></returns>
        public static BatchSQL CreateBatch(this Database db)
        {
            return new BatchSQL(db.CreateParameterCollection());
        }

        /// <summary>
        /// 创建一个Sql批处理对象
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="capacity">集合默认空间分配大小</param>
        /// <returns></returns>
        public static BatchSQL CreateBatch(this Database db, int capacity)
        {
            return new BatchSQL(db.CreateParameterCollection(), capacity);
        }

        /// <summary>
        /// 创建DbCommand对象
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static DbCommand BuilderDbCommand(this Database db, CommandType commandType, string commandText, ParameterCollection parameters)
        {
            DbCommand command;
            switch (commandType)
            {
                case CommandType.StoredProcedure:
                    command = db.GetStoredProcCommand(commandText);
                    break;
                default:
                    command = db.GetSqlStringCommand(commandText);
                    break;
            }

            //
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    param.ParameterName = db.BuildParameterName(param.ParameterName);
                    command.Parameters.Add(param);
                }
            }
            return command;
        }
    }
}
