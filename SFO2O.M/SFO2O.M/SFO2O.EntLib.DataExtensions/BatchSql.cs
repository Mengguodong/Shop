using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.ComponentModel;
using SFO2O.EntLib.DataExtensions.DataMapper;
using System.Data.Common;

namespace SFO2O.EntLib.DataExtensions
{
    /// <summary>
    /// SQL语句集合,用于将多条Sql或多个实体对象，一次性执行。以减与数据库的多次链接而提高效率。
    /// </summary>
    /// <remarks>这是一个支持迭代的对象，通过迭代将获取所有的Sql语句（它也包括实体对象的sql）。</remarks>
    public class BatchSQL : IEnumerable<string>
    {
        /// <summary>
        /// 待执行的SQL集合
        /// </summary>
        private List<string> _list;

        /// <summary>
        /// 所有的SQL语句的在执行时需要的参数集合
        /// </summary>
        public ParameterCollection Parameters
        { get; private set; }

        /// <summary>
        /// SQL语句批量的条数
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// 最后一次添加的SQL语句
        /// </summary>
        public string LastSQL { get; protected set; }

        /// <summary>
        /// 初始化
        /// </summary>
        private BatchSQL()
        {
            _list = new List<string>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="capacity">集合数组预分配大小(不能小于0)</param>
        private BatchSQL(int capacity)
        {
            _list = new List<string>(capacity);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="parameter">参数集合</param>
        internal BatchSQL(ParameterCollection parameter)
            : this()
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            this.Parameters = parameter;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="parameter">参数集合</param>
        /// <param name="capacity">集合数组预分配大小(不能小于0)</param>
        internal BatchSQL(ParameterCollection parameter, int capacity)
            : this(capacity)
        {
            if (parameter == null)
                throw new ArgumentNullException("provider");

            this.Parameters = parameter;
        }

        private string ParamFormat(string sql, params object[] values)
        {
            string[] list = Parameters.Fill(values);
            return string.Format(sql, list);
        }
        private string ParamFormat(string sql, IList values)
        {
            string[] list = Parameters.Fill(values);
            return string.Format(sql, list);
        }

        /// <summary>
        /// 向SQL语句集合中追加一个sql
        /// </summary>
        /// <param name="sql">被执行的sql语句</param>
        /// <returns>返回对象本身，它的目的是让编码更顺畅更轻松（Fluent）。</returns>
        public BatchSQL Append(string sql)
        {
            _list.Add(sql);
            LastSQL = sql;
            //
            return this;
        }

        /// <summary>
        /// 向SQL语句集合中追加一个sql
        /// </summary>
        /// <param name="sql">被执行的sql语句</param>
        /// <param name="values">sql参数</param>
        /// <example>sql语句示例：INSERT INTO TableName(col1,col2,col3) VALUES({0},{1},{2});</example>
        /// <returns>返回对象本身，它的目的是让编码更顺畅更轻松（Fluent）。</returns>
        /// <remarks></remarks>
        public virtual BatchSQL AppendFormat(string sql, params object[] values)
        {
            return Append(ParamFormat(sql, values));
        }

        /// <summary>
        /// 向SQL语句集合中追加一个sql
        /// </summary>
        /// <param name="sql">被执行的sql语句</param>
        /// <param name="values">sql参数</param>
        /// <returns>返回对象本身，它的目的是让编码更顺畅更轻松（Fluent）。</returns>
        /// <example>sql语句示例：INSERT INTO TableName(col1,col2,col3) VALUES({0},{1},{2});</example>
        /// <remarks></remarks>
        public virtual BatchSQL AppendFormat(string sql, IList values)
        {
            return Append(ParamFormat(sql, values));
        }

        #region IEnumerable Members

        public IEnumerator<string> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion
    }
}
