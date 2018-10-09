using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace SFO2O.EntLib.DataExtensions
{
    public static class DatabaseExtension
    {
        /// <summary>
        /// AddInParameter扩展
        /// </summary>
        /// <param name="db">DataBase</param>
        /// <param name="comm">DbCommand</param>
        /// <param name="name">参数名</param>
        /// <param name="size">大小</param>
        /// <param name="type">DbType</param>
        public static void AddInParameter(this Database db, DbCommand comm, string name, int size, DbType type)
        {
            db.AddParameter(comm, name, type, size, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, null);
        }

        /// <summary>
        /// AddInParameter扩展
        /// </summary>
        /// <param name="db">DataBase</param>
        /// <param name="comm">DbCommand</param>
        /// <param name="name">参数名</param>
        /// <param name="precision">数值精度</param>
        /// <param name="scale">小数位数</param>
        /// <param name="type">DbType</param>
        public static void AddInParameter(this Database db, DbCommand comm, string name, byte precision, byte scale, DbType type)
        {
            db.AddParameter(comm, name, type, 0, ParameterDirection.Input, false, precision, scale, String.Empty, DataRowVersion.Default, null);
        }

        /// <summary>
        /// AddInParameter扩展
        /// </summary>
        /// <param name="db">DataBase</param>
        /// <param name="comm">DbCommand</param>
        /// <param name="name">参数名</param>
        /// <param name="size">大小</param>
        /// <param name="type">DbType</param>
        /// <param name="value">参数值</param>
        public static void AddInParameter(this Database db, DbCommand comm, string name, int size, DbType type, object value)
        {
            db.AddParameter(comm, name, type, size, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// AddInParameter扩展
        /// </summary>
        /// <param name="db">DataBase</param>
        /// <param name="comm">DbCommand</param>
        /// <param name="name">参数名</param>
        /// <param name="precision">数值精度</param>
        /// <param name="scale">小数位数</param>
        /// <param name="type">DbType</param>
        /// <param name="value">参数值</param>
        public static void AddInParameter(this Database db, DbCommand comm, string name, byte precision, byte scale, DbType type, object value)
        {
            db.AddParameter(comm, name, type, 0, ParameterDirection.Input, false, precision, scale, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// AddInAndOutParameter扩展
        /// </summary>
        /// <param name="db">DataBase</param>
        /// <param name="comm">DbCommand</param>
        /// <param name="name">参数名</param>
        /// <param name="type">DbType</param>
        /// <param name="value">参数值</param>
        public static void AddInAndOutParameter(this Database db, DbCommand comm, string name, DbType type, object value)
        {
            db.AddParameter(comm, name, type, 0, ParameterDirection.InputOutput, false, 0, 0, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// AddInAndOutParameter扩展
        /// </summary>
        /// <param name="db">DataBase</param>
        /// <param name="comm">DbCommand</param>
        /// <param name="name">参数名</param>
        /// <param name="size">大小</param>
        /// <param name="type">DbType</param>
        /// <param name="value">参数值</param>
        public static void AddInAndOutParameter(this Database db, DbCommand comm, string name, int size, DbType type, object value)
        {
            db.AddParameter(comm, name, type, size, ParameterDirection.InputOutput, false, 0, 0, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// AddInAndOutParameter扩展
        /// </summary>
        /// <param name="db">DataBase</param>
        /// <param name="comm">DbCommand</param>
        /// <param name="name">参数名</param>
        /// <param name="precision">数值精度</param>
        /// <param name="scale">小数位数</param>
        /// <param name="type">DbType</param>
        /// <param name="value">参数值</param>
        public static void AddInAndOutParameter(this Database db, DbCommand comm, string name, byte precision, byte scale, DbType type, object value)
        {
            db.AddParameter(comm, name, type, 0, ParameterDirection.InputOutput, false, precision, scale, String.Empty, DataRowVersion.Default, value);
        }
    }
}
