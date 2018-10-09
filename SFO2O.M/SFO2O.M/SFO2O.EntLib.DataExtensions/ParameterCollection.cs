using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SFO2O.EntLib.DataExtensions.DataMapper;
using SFO2O.EntLib.DataExtensions.Basic;

namespace SFO2O.EntLib.DataExtensions
{
    /// <summary>
    /// sql语句参数集合(用于<typeparamref name="DbParameter"/>)
    /// </summary>
    public class ParameterCollection : IEnumerable<DbParameter>
    {
        private List<DbParameter> _List;
        private readonly Database _db;

        /// <summary>
        /// 
        /// </summary>
        internal Database Database
        {
            get { return _db; }
        }

        /// <summary>
        /// 获取集合中对象的个数。
        /// </summary>
        public int Count
        {
            get { return _List.Count; }
        }

        /// <summary>
        /// 获取参数对象
        /// </summary>
        /// <param name="index">指定下标,从0开始的index</param>
        /// <returns>参数对象</returns>
        public DbParameter this[int index]
        {
            get
            {
                return _List[index];
            }
        }

        /// <summary>
        /// 获取参数对象
        /// </summary>
        /// <param name="index">指定参数名称</param>
        /// <returns>参数对象</returns>
        public DbParameter this[string parameterName]
        {
            get
            {
                foreach (DbParameter dbParam in _List)
                {
                    if (dbParam.ParameterName == parameterName)
                        return dbParam;
                }
                return null;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <remarks>通过<typeparamref name="Microsoft.Practices.EnterpriseLibrary.Data.Database"/>的扩展方法可以创建实例。</remarks>
        internal ParameterCollection(Database db)
            : this(db, 4)
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="capacity">集合每次分配空间的增量大小</param>
        /// <remarks>通过<typeparamref name="Microsoft.Practices.EnterpriseLibrary.Data.Database"/>的扩展方法可以创建实例。</remarks>
        internal ParameterCollection(Database db, int capacity)
        {
            Check.IsNull(db, "db");

            _List = new List<DbParameter>(capacity);
            _db = db;
        }

        #region CreateParameter

        /// <summary>
        /// 创建一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>返回一个参数对象</returns>
        protected DbParameter CreateParameter(string name, object value)
        {
            var dbParam = _db.DbProviderFactory.CreateParameter();
            dbParam.ParameterName = _db.BuildParameterName(name);
            dbParam.Value = value ?? DBNull.Value;
            return dbParam;
        }
        /// <summary>
        /// 创建一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="suffix">参数名称的后缀（下标，人为的控制参数惟一性）</param>
        /// <param name="value">参数值</param>
        /// <returns>返回一个参数对象</returns>
        protected DbParameter CreateParameter(string name, int suffix, object value)
        {
            var dbParam = _db.DbProviderFactory.CreateParameter();
            dbParam.ParameterName = _db.BuildParameterName(string.Concat(name, "_", suffix.ToString()));
            dbParam.Value = value ?? DBNull.Value;
            return dbParam;
        }

        #endregion

        #region Fill

        /// <summary>
        /// 向集合中添加参数值
        /// </summary>
        /// <param name="values">参数值</param>
        /// <returns>参数名称，与values中的参数值的下标一 一对应</returns>
        public string[] Fill(object[] values)
        {
            // 不需要做空判断，如果没有参数的话长度为0

            // 因为给的values只有值，没有参数无名称，所以默认情况下将参数的名称用@param0,@param1,@param2...
            IList<string> _list = new List<string>(values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                string name = this.Append("param", values[i] ?? DBNull.Value, this.Count).ParameterName;
                _list.Add(name);
            }
            return _list.ToArray();
        }
        /// <summary>
        /// 向集合中添加参数值
        /// </summary>
        /// <param name="values">参数值</param>
        /// <returns>参数名称，与values中的参数值的下标一一对应</returns>
        public string[] Fill(IList values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            // 因为给的values只有值，没有参数无名称，所以默认情况下将参数的名称用@param0,@param1,@param2...
            IList<string> _list = new List<string>(values.Count);
            for (int i = 0; i < values.Count; i++)
            {
                string name = this.Append("param", values[i] ?? DBNull.Value, this.Count).ParameterName;
                _list.Add(name);
            }
            return _list.ToArray();
        }

        #endregion

        #region Append

        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>新建在集合中的参数对象</returns>
        public DbParameter Append(string name, object value)
        {
            DbParameter param = CreateParameter(name, value);
            this._List.Add(param);
            return param;
        }

        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="suffix">参数名称的后缀（下标，人为的控制参数惟一性）</param>
        /// <returns>新建在集合中的参数对象</returns>
        public DbParameter Append(string name, object value, int suffix)
        {
            DbParameter param = CreateParameter(name, suffix, value);
            this._List.Add(param);
            return param;
        }

        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <param name="direction">参数方向</param>
        /// <param name="size">参数大小</param>
        /// <returns></returns>
        public DbParameter Append(string name, object value, SqlDbType type, ParameterDirection direction, int size) 
        {
            DbParameter param = CreateParameter(name, value);
            System.Data.SqlClient.SqlParameter sqlParameter = param as System.Data.SqlClient.SqlParameter;
            sqlParameter.SqlDbType = type;
            param.Direction = direction;
            param.Size = size;
            this._List.Add(param);
            
            return param;
        }
        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <returns></returns>
        public DbParameter Append(string name, object value, SqlDbType type, int size) 
        {
            return Append(name, value, type, ParameterDirection.Input, size);
        }

        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>      
        /// <returns></returns>
        public DbParameter Append(string name, object value, SqlDbType type)
        {
            DbParameter param = CreateParameter(name, value);
            System.Data.SqlClient.SqlParameter sqlParameter = param as System.Data.SqlClient.SqlParameter;
            sqlParameter.SqlDbType = type;
            param.Direction = ParameterDirection.Input;
          
            this._List.Add(param);

            return param;
        }

        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <param name="direction">参数方向</param>
        /// <returns></returns>
        public DbParameter Append(string name, object value, SqlDbType type, ParameterDirection direction)
        {
            DbParameter param = CreateParameter(name, value);
            System.Data.SqlClient.SqlParameter sqlParameter = param as System.Data.SqlClient.SqlParameter;
            sqlParameter.SqlDbType = type;
            param.Direction = direction;

            this._List.Add(param);

            return param;
        }

        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数映射至数据库中的类型</param>
        /// <returns>新建在集合中的参数对象</returns>
        public DbParameter Append(string name, object value, DbType type)
        {
            DbParameter param = CreateParameter(name, value);
            param.DbType = type;
            this._List.Add(param);
            return param;
        }
        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数映射至数据库中的类型</param>
        /// <param name="precision">指示数值参数的精度</param>
        /// <param name="scale">指示数值参数的小数位数</param>
        /// <returns>新建在集合中的参数对象</returns>
        public DbParameter Append(string name, object value, DbType type, byte precision, byte scale)
        {
            var param = Append(name, value, type);
            var parameter = (IDbDataParameter)param;
            parameter.Precision = precision;
            parameter.Scale = scale;
            return (DbParameter)parameter;

        }
        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="direction">参数方向</param>
        /// <returns>新建在集合中的参数对象</returns>
        public DbParameter Append(string name, object value, ParameterDirection direction)
        {
            DbParameter param = CreateParameter(name, value);
            param.Direction = direction;
            this._List.Add(param);
            return param;
        }
        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="direction">参数方向</param>
        /// <param name="precision">指示数值参数的精度</param>
        /// <param name="scale">指示数值参数的小数位数</param>
        /// <returns>新建在集合中的参数对象</returns>
        public DbParameter Append(string name, object value, ParameterDirection direction, byte precision, byte scale)
        {
            var param = Append(name, value, direction);
            var parameter = (IDbDataParameter)param;
            parameter.Precision = precision;
            parameter.Scale = scale;
            return (DbParameter)parameter;

        }
        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数映射至数据库中的类型</param>
        /// <param name="direction">参数方向</param>
        /// <returns>新建在集合中的参数对象</returns>
        public DbParameter Append(string name, object value, DbType type, ParameterDirection direction)
        {
            DbParameter param = CreateParameter(name, value);
            param.DbType = type;
            param.Direction = direction;
            this._List.Add(param);
            return param;
        }
        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数映射至数据库中的类型</param>
        /// <param name="direction">参数方向</param>
        /// <param name="precision">指示数值参数的精度</param>
        /// <param name="scale">指示数值参数的小数位数</param>
        /// <returns>新建在集合中的参数对象</returns>
        public DbParameter Append(string name, object value, DbType type, ParameterDirection direction, byte precision, byte scale)
        {
            var param = Append(name, value, type, direction);
            var parameter = (IDbDataParameter)param;
            parameter.Precision = precision;
            parameter.Scale = scale;
            return (DbParameter)parameter;

        }
        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数映射至数据库中的类型</param>
        /// <param name="direction">参数方向</param>
        /// <param name="size">参数大小</param>
        /// <returns>新建在集合中的参数对象</returns>
        public DbParameter Append(string name, object value, DbType type, ParameterDirection direction, int size)
        {
            DbParameter param = CreateParameter(name, value);
            param.DbType = type;
            param.Direction = direction;
            param.Size = size;
            this._List.Add(param);
            return param;
        }
        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数映射至数据库中的类型</param>
        /// <param name="direction">参数方向</param>
        /// <param name="size">参数大小</param>
        /// <param name="precision">指示数值参数的精度</param>
        /// <param name="scale">指示数值参数的小数位数</param>
        /// <returns>新建在集合中的参数对象</returns>
        public DbParameter Append(string name, object value, DbType type, ParameterDirection direction, int size, byte precision, byte scale)
        {
            var param = Append(name, value, type, direction,size);
            var parameter = (IDbDataParameter)param;
            parameter.Precision = precision;
            parameter.Scale = scale;
            return (DbParameter)parameter;           
        }
        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数映射至数据库中的类型</param>
        /// <param name="suffix">参数名称的后缀（下标，人为的控制参数惟一性）</param>
        /// <returns>新建在集合中的参数对象</returns>
        public DbParameter Append(string name, object value, DbType type, int suffix)
        {
            DbParameter param = CreateParameter(name, suffix, value);
            param.DbType = type;
            this._List.Add(param);
            return param;
        }
        /// <summary>
        /// 向集合中添加一个参数对象
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数映射至数据库中的类型</param>
        /// <param name="suffix">参数名称的后缀（下标，人为的控制参数惟一性）</param>
        /// <param name="precision">指示数值参数的精度</param>
        /// <param name="scale">指示数值参数的小数位数</param>
        /// <returns>新建在集合中的参数对象</returns>
        public DbParameter Append(string name, object value, DbType type, int suffix, byte precision, byte scale)
        {
            var param = Append(name, value, type, suffix);
            var parameter = (IDbDataParameter)param;
            parameter.Precision = precision;
            parameter.Scale = scale;
            return (DbParameter)parameter;

        }

        public void AddRange(IEnumerable<DbParameter> enumerable) 
        {
            this._List.AddRange(enumerable);
        }
        #endregion

        #region implement IEnumerable<DbParameter>

        public IEnumerator<DbParameter> GetEnumerator()
        {
            return _List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _List.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// 清空所有的参数
        /// </summary>
        public void Clear()
        {
            _List.Clear();
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder();
            foreach (var item in _List)
            {
                content.Append("[").Append(item.ParameterName).Append("=").Append(item.Value).AppendLine("]");
            }
            content.Append(";");
            return content.ToString();
        }
    }
}
