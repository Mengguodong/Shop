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
using SFO2O.EntLib.DataExtensions;

namespace SFO2O.EntLib.DataExtensions
{
    public static partial class DatabaseExtensions
    {
        #region ExecuteSql

        /// <summary>
        /// 执行sql语句，将结果集的第一行转换成类型<typeparamref name="T"/>实例对象并返回。
        /// </summary>
        /// <typeparam name="T"><typeparamref name="T"/></typeparam>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="sqlStatments">sql语句</param>
        /// <param name="parameters">sql语句的参数</param>
        /// <returns>返回类型<typeparamref name="T"/>的实例，如果数据库无返回的结集，则返回空。</returns>
        public static T ExecuteSqlFirst<T>(this Database db, string sqlStatments)
        {
            using (var reader = db.ExecuteReader(CommandType.Text, sqlStatments))
            {
                return reader.First<T>();
            }
        }

        /// <summary>
        /// 执行sql语句，将结果集的第一行转换成类型<typeparamref name="T"/>实例对象并返回。
        /// </summary>
        /// <typeparam name="T"><typeparamref name="T"/></typeparam>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="sqlStatments">sql语句</param>
        /// <param name="parameters">sql语句的参数</param>
        /// <returns>返回类型<typeparamref name="T"/>的实例，如果数据库无返回的结集，则返回空。</returns>
        public static T ExecuteSqlFirst<T>(this Database db, string sqlStatments, ParameterCollection parameters)
        {
            using (var reader = db.ExecuteReader(CommandType.Text, sqlStatments, parameters))
            {
                return reader.First<T>();
            }
        }

        /// <summary>
        /// 执行sql语句，将结果集转换成IList&lt;<typeparamref name="T"/>&gt; 集合，并返回。
        /// </summary>
        /// <typeparam name="T"><typeparamref name="T"/></typeparam>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="sqlStatments">sql语句</param>
        /// <param name="parameters">sql语句的参数</param>
        /// <returns>IList&lt;<typeparamref name="T"/>&gt; 集合</returns>
        public static IList<T> ExecuteSqlList<T>(this Database db, string sqlStatments)
        {
            using (var reader = db.ExecuteReader(CommandType.Text, sqlStatments))
            {
                return reader.ToList<T>();
            }
        }

        /// <summary>
        /// 执行sql语句，将结果集转换成IList&lt;<typeparamref name="T"/>&gt; 集合，并返回。
        /// </summary>
        /// <typeparam name="T"><typeparamref name="T"/></typeparam>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="sqlStatments">sql语句</param>
        /// <param name="parameters">sql语句的参数</param>
        /// <returns>IList&lt;<typeparamref name="T"/>&gt; 集合</returns>
        public static IList<T> ExecuteSqlList<T>(this Database db, string sqlStatments, ParameterCollection parameters)
        {
            using (var reader = db.ExecuteReader(CommandType.Text, sqlStatments, parameters))
            {
                return reader.ToList<T>();
            }
        }

        /// <summary>
        /// 执行sql语句，将标值转换成<typeparamref name="T"/>并返回。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="sqlStatments">sql语句</param>
        /// <param name="parameters">sql参数集合</param>
        /// <returns>如果是转换失败将抛出异常，表示sql返回的数据不符开发人员指定的类型。所以开发人员需要自己把握异常处理。</returns>
        public static T ExecuteSqlScalar<T>(this Database db, string sqlStatments)
        {
            return db.ExecuteSqlScalar<T>(sqlStatments, (ParameterCollection)null);
        }

        /// <summary>
        /// 执行sql语句，将标值转换成<typeparamref name="T"/>并返回。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="sqlStatments">sql语句</param>
        /// <param name="parameters">sql参数集合</param>
        /// <returns>如果是转换失败将抛出异常，表示sql返回的数据不符开发人员指定的类型。所以开发人员需要自己把握异常处理。</returns>
        public static T ExecuteSqlScalar<T>(this Database db, string sqlStatments, ParameterCollection parameters)
        {
            object val = db.ExecuteScalar(CommandType.Text, sqlStatments, parameters);
            if (val == null || val == DBNull.Value)
                return default(T);
            else if (val is T)
                return (T)val;
            else
                return (T)Convert.ChangeType(val, typeof(T));
        }

        /// <summary>
        /// 执行sql语句，将标值转换成<typeparamref name="T"/>并返回。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="sqlStatments">sql语句</param>
        /// <param name="parameters">sql参数集合</param>
        /// <returns>如果是转换失败将抛出异常，表示sql返回的数据不符开发人员指定的类型。所以开发人员需要自己把握异常处理。</returns>
        public static T ExecuteSqlScalar<T>(this Database db, string sqlStatments, params object[] parameters)
        {
            ParameterCollection para = db.CreateParameterCollection(parameters.Length);
            sqlStatments = string.Format(sqlStatments, para.Fill(parameters));

            return db.ExecuteSqlScalar<T>(sqlStatments, para);
        }

        /// <summary>
        /// 执行sql语句，返回IDataReader。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="sqlStatments">sql语句</param>
        /// <param name="parameters">sql语句的参数</param>
        /// <returns>返回IDataReader</returns>
        public static IDataReader ExecuteSqlReader(this Database db, string sqlStatments)
        {
            return db.ExecuteSqlReader(sqlStatments, (ParameterCollection)null);
        }

        /// <summary>
        /// 执行sql语句，返回IDataReader。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="sqlStatments">sql语句</param>
        /// <param name="parameters">sql语句的参数</param>
        /// <returns>返回IDataReader</returns>
        public static IDataReader ExecuteSqlReader(this Database db, string sqlStatments, ParameterCollection parameters)
        {
            return db.ExecuteReader(CommandType.Text, sqlStatments, parameters);
        }

        /// <summary>
        /// 执行sql语句，返回DataSet。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="sqlStatments">sql语句</param>
        /// <param name="parameters">sql语句的参数</param>
        /// <returns>返回DataSet</returns>
        public static DataSet ExecuteSqlDataSet(this Database db, string sqlStatments)
        {
            return db.ExecuteSqlDataSet(sqlStatments, (ParameterCollection)null);
        }

        /// <summary>
        /// 执行sql语句，返回DataSet。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="sqlStatments">sql语句</param>
        /// <param name="parameters">sql语句的参数</param>
        /// <returns>返回DataSet</returns>
        public static DataSet ExecuteSqlDataSet(this Database db, string sqlStatments, ParameterCollection parameters)
        {
            return db.ExecuteDataSet(CommandType.Text, sqlStatments, parameters);
        }

        /// <summary>
        /// 执行sql语句，返回DataSet(批量执行,没有事物支持不能保证数据的一致性和完整性)。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="batch">sql批次</param>
        /// <returns>返回一个DataSet</returns>
        public static DataSet ExecuteSqlDataSet(this Database db, BatchSQL batch)
        {
            StringBuilder sql = new StringBuilder();
            foreach (string sqlStatment in batch)
            {
                sql.AppendLine(sqlStatment);
            }

            DbCommand command = db.BuilderDbCommand(CommandType.Text, sql.ToString(), batch.Parameters);
            return db.ExecuteDataSet(command);
        }

        /// <summary>
        /// 执行sql语句，返回DataSet(批量执行,没有事物支持不能保证数据的一致性和完整性)。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="batch">sql批次</param>
        /// <param name="transaction">事务处理</param>
        /// <returns>返回一个DataSet</returns>
        public static DataSet ExecuteSqlDataSet(this Database db, BatchSQL batch, DbTransaction transaction)
        {
            StringBuilder sql = new StringBuilder();
            foreach (string sqlStatment in batch)
            {
                sql.AppendLine(sqlStatment);
            }

            DbCommand command = db.BuilderDbCommand(CommandType.Text, sql.ToString(), batch.Parameters);
            return db.ExecuteDataSet(command, transaction);
        }

        /// <summary>
        /// 执行sql语句，返回DataTable。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="sqlStatments">sql语句</param>
        /// <param name="parameters">sql语句的参数</param>
        /// <returns>返回DataTable</returns>
        public static DataTable ExecuteSqlTable(this Database db, string sqlStatments)
        {
            return db.ExecuteSqlTable(sqlStatments, (ParameterCollection)null);
        }

        /// <summary>
        /// 执行sql语句，返回DataTable。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="sqlStatments">sql语句</param>
        /// <param name="parameters">sql语句的参数</param>
        /// <returns>返回DataTable</returns>
        public static DataTable ExecuteSqlTable(this Database db, string sqlStatments, ParameterCollection parameters)
        {
            var ds = db.ExecuteDataSet(CommandType.Text, sqlStatments, parameters);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        /// <summary>
        /// 执行sql语句，不返回结果集。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="sqlStatments">sql语句</param>
        /// <param name="parameters">sql语句的参数</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteSqlNonQuery(this Database db, string sqlStatments)
        {
            return db.ExecuteSqlNonQuery(sqlStatments, (ParameterCollection)null);
        }

        /// <summary>
        /// 执行sql语句，不返回结果集。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="sqlStatments">sql语句</param>
        /// <param name="parameters">sql语句的参数</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteSqlNonQuery(this Database db, string sqlStatments, ParameterCollection parameters)
        {
            return db.ExecuteNonQuery(CommandType.Text, sqlStatments, parameters);
        }
        public static int ExecuteSqlNonQuery(this Database db, string sqlStatments, ParameterCollection parameters, DbTransaction transaction)
        {
            DbCommand command = db.BuilderDbCommand(CommandType.Text, sqlStatments, parameters);
            return db.ExecuteNonQuery(command, transaction);
        }
        /// <summary>
        /// 执行sql语句，不返回任何结果集。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="batch">sql批次</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteSqlNonQuery(this Database db, BatchSQL batch)
        {
            StringBuilder sql = new StringBuilder();
            foreach (string sqlStatment in batch)
            {
                sql.AppendLine(sqlStatment);
            }

            DbCommand command = db.BuilderDbCommand(CommandType.Text, sql.ToString(), batch.Parameters);
            return db.ExecuteNonQuery(command);
        }

        /// <summary>
        /// 执行sql语句，不返回任何结果集。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="batch">sql批次</param>
        /// <param name="transaction">事务处理</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteSqlNonQuery(this Database db, BatchSQL batch, DbTransaction transaction)
        {
            StringBuilder sql = new StringBuilder();
            foreach (string sqlStatment in batch)
            {
                sql.AppendLine(sqlStatment);
            }

            DbCommand command = db.BuilderDbCommand(CommandType.Text, sql.ToString(), batch.Parameters);
            return db.ExecuteNonQuery(command, transaction);
        }

        #endregion

        #region ExecuteProcdure

        /// <summary>
        /// 执行一个存储过程，不返回结果集。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="parameters">存储过程的参数</param>
        /// <returns>影响到的数据行数</returns>
        public static int ExecuteProcdureNonQuery(this Database db, string spName, ParameterCollection parameters)
        {
            return db.ExecuteNonQuery(CommandType.StoredProcedure, spName, parameters);
        }

        /// <summary>
        /// 执行一个存储过程，返回一个DataSet。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="parameters">存储过程的参数</param>
        /// <returns>返回一个不为空的DataSet</returns>
        public static DataSet ExecuteProcdureDataSet(this Database db, string spName, ParameterCollection parameters)
        {
            return db.ExecuteDataSet(CommandType.StoredProcedure, spName, parameters);
        }

        /// <summary>
        /// 执行一个存储过程，返回一个IDataReader。
        /// </summary>
        /// <param name="db">企业库扩展：<typeparamref name="Database"/></param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="parameters">存储过程的参数</param>
        /// <returns>返回一个不为空的IDataReader</returns>
        public static IDataReader ExecuteProcdureReader(this Database db, string spName, ParameterCollection parameters)
        {
            return db.ExecuteReader(CommandType.StoredProcedure, spName, parameters);
        }

        #endregion
    }
}
