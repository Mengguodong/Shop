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
        #region ExecuteReader

        /// <summary>
        /// 执行sql语句返回一个IDataReader。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="type">命令类型</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回一个IDataReader</returns>
        public static IDataReader ExecuteReader(this Database db, CommandType type, string commandText, ParameterCollection parameters)
        {
            DbCommand command = db.BuilderDbCommand(type, commandText, parameters);
            return db.ExecuteReader(command);
        }

        /// <summary>
        /// 执行sql语句返回一个IDataReader。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="type">命令类型</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="parameters">参数集合</param>
        /// <param name="transaction">事务处理</param>
        /// <returns>返回一个IDataReader</returns>
        public static IDataReader ExecuteReader(this Database db, CommandType type, string commandText, ParameterCollection parameters, DbTransaction transaction)
        {
            DbCommand command = db.BuilderDbCommand(type, commandText, parameters);
            return db.ExecuteReader(command, transaction);
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 执行sql语句返回一个DataSet。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="commandText">sql语句</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回一个DataSet</returns>
        public static DataSet ExecuteDataSet(this Database db, CommandType type, string commandText, ParameterCollection parameters)
        {
            DbCommand command = db.BuilderDbCommand(type, commandText, parameters);
            return db.ExecuteDataSet(command);
        }

        /// <summary>
        /// 执行sql语句返回一个DataSet。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="commandText">sql语句</param>
        /// <param name="parameters">参数集合</param>
        /// <param name="transaction">事务处理</param>
        /// <returns>返回一个DataSet</returns>
        public static DataSet ExecuteDataSet(this Database db, CommandType type, string commandText, ParameterCollection parameters, DbTransaction transaction)
        {
            DbCommand command = db.BuilderDbCommand(type, commandText, parameters);
            return db.ExecuteDataSet(command, transaction);
        }

        #endregion ExecuteDataSet

        #region ExecuteNonQuery

        /// <summary>
        /// 执行sql语句，不返回结果集。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="type">命令类型</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery(this Database db, CommandType type, string commandText, ParameterCollection parameters)
        {
            DbCommand command = db.BuilderDbCommand(type, commandText, parameters);
            return db.ExecuteNonQuery(command);
        }

        /// <summary>
        /// 执行sql语句，不返回结果集。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="type">命令类型</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="parameters">参数集合</param>
        /// <param name="transaction">事务处理</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery(this Database db, CommandType type, string commandText, ParameterCollection parameters, DbTransaction transaction)
        {
            DbCommand command = db.BuilderDbCommand(type, commandText, parameters);
            return db.ExecuteNonQuery(command, transaction);
        }

        #endregion ExecuteNonQuery

        #region ExecuteScalar

        /// <summary>
        /// 执行sql语句，返回标值。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="type">命令类型</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>受影响的行数/或第一行第一列的值</returns>
        public static object ExecuteScalar(this Database db, CommandType type, string commandText, ParameterCollection parameters)
        {
            DbCommand command = db.BuilderDbCommand(type, commandText, parameters);
            return db.ExecuteScalar(command);
        }

        /// <summary>
        /// 执行sql语句，返回标值。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="type">命令类型</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="parameters">参数集合</param>
        /// <param name="transaction">事务处理</param>
        /// <returns>受影响的行数/或第一行第一列的值</returns>
        public static object ExecuteScalar(this Database db, CommandType type, string commandText, ParameterCollection parameters, DbTransaction transaction)
        {
            DbCommand command = db.BuilderDbCommand(type, commandText, parameters);
            return db.ExecuteScalar(command, transaction);
        }

        #endregion
    }
}
