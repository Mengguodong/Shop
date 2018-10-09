using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace SFO2O.EntLib.DataExtensions
{
    public static class DbCommandExtension
    {
        public static void AppendParam(this DbCommand comm, string name, object value, SqlDbType type)
        {
            comm.AppendParam(name, value, type, ParameterDirection.Input);
        }

        public static void AppendParam(this DbCommand comm, string name, object value, SqlDbType type, int size)
        {
            comm.AppendParam(name, value, type, size, ParameterDirection.Input);
        }

        public static void AppendParam(this DbCommand comm, string name, object value, SqlDbType type, ParameterDirection direction)
        {
            var p = comm.CreateParameter() as SqlParameter;
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            p.Direction = direction;
            p.SqlDbType = type;
            comm.Parameters.Add(p);
        }

        public static void AppendParam(this DbCommand comm, string name, object value, SqlDbType type, int size, ParameterDirection direction)
        {
            var p = comm.CreateParameter() as SqlParameter;
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            p.Direction = direction;
            p.SqlDbType = type;
            p.Size = size;
            comm.Parameters.Add(p);
        }
    }
}
