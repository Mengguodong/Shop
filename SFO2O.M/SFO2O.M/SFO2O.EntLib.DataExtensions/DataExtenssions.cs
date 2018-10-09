using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SFO2O.EntLib.DataExtensions.DataMapper;

namespace SFO2O.EntLib.DataExtensions
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class DataExtenssions
    {
        #region DataSet

        /// <summary>
        /// 返回DataSet中的第一个DataTable对象。
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static DataTable First(this DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        #endregion

        #region DataTable

        private static int IndexOfColumn(this DataColumnCollection cols, string columnName)
        {
            int index = cols.IndexOf(columnName);
            if (index < 0)
                throw new MapperException(string.Format("不包含列名：{0}。", columnName));
            else
                return index;
        }

        /// <summary>
        /// 获取DataTable中第一行列columnName的值，如果DataTable没有数据时，返回<typeparamref name="T"/>的默认值。
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="columnName">要获取的字段</param>
        /// <returns>columnName字段中的数据</returns>
        public static T FirstRowVal<T>(this DataTable tb, string columnName)
        {
            if (tb.Rows.Count < 1)
                return default(T);

            var val = tb.Rows[0][columnName];
            if (val is T)
                return (T)val;
            else if (val == DBNull.Value || val == null)
                return default(T);
            else
                return (T)Convert.ChangeType(val, typeof(T));
        }
        /// <summary>
        /// 获取DataTable中的行转换成ItemValue对象。
        /// </summary>
        /// <typeparam name="TItem">键的类型</typeparam>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="itemColumn">对应Item的列名称</param>
        /// <param name="valueColumn">对应Value的列名称</param>
        /// <returns></returns>
        public static IList<KeyValuePair<TItem, TValue>> ToItemValues<TItem, TValue>(this DataTable tb, string itemColumn, string valueColumn)
        {
            return tb.ToItemValues<TItem, TValue>(itemColumn, valueColumn, x => (TItem)x, y => (TValue)y);
        }
        /// <summary>
        /// 获取DataTable中的行转换成ItemValue对象。
        /// </summary>
        /// <typeparam name="TItem">键的类型</typeparam>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="itemColumn">对应Item的列名称</param>
        /// <param name="valueColumn">对应Value的列名称</param>
        /// <param name="itemConvert">键转换，自定义<paramref name="itemColumn"/>字段的转换</param>
        /// <param name="valueConvert">值转换，自定义<paramref name="valueColumn"/>字段的转换</param>
        /// <returns></returns>
        public static IList<KeyValuePair<TItem, TValue>> ToItemValues<TItem, TValue>(this DataTable tb, string itemColumn, string valueColumn, Func<object, TItem> itemConvert, Func<object, TValue> valueConvert)
        {
            if (tb.Rows.Count == 0)
                return new List<KeyValuePair<TItem, TValue>>(0);

            List<KeyValuePair<TItem, TValue>> ls = new List<KeyValuePair<TItem, TValue>>(tb.Rows.Count);
            int index1 = tb.Columns.IndexOfColumn(itemColumn);
            int index2 = tb.Columns.IndexOfColumn(valueColumn);
            foreach (DataRow row in tb.Rows)
            {
                ls.Add(new KeyValuePair<TItem, TValue>(itemConvert(row[index1]), valueConvert(row[index2])));
            }
            return ls;
        }
        /// <summary>
        /// 将DataTable指定的列以集合的方式取出
        /// </summary>
        /// <typeparam name="T">字段的数据类型</typeparam>
        /// <param name="tb">System.Data.DataTable</param>
        /// <param name="columnName">字段名称</param>
        /// <returns>columnName字段中的数据集合</returns>
        public static IList<T> ToValues<T>(this DataTable tb, string columnName)
        {
            return tb.ToValues<T>(columnName, x => (T)x);
        }
        /// <summary>
        /// 将DataTable指定的列以集合的方式取出
        /// </summary>
        /// <typeparam name="T">字段的数据类型</typeparam>
        /// <param name="tb">System.Data.DataTable</param>
        /// <param name="columnName">字段名称</param>
        /// <returns>columnName字段中的数据集合</returns>
        public static IList<T> ToValues<T>(this DataTable tb, string columnName, Func<object, T> convert)
        {
            if (tb.Rows.Count == 0)
                return new List<T>(0);

            List<T> ls = new List<T>(tb.Rows.Count);
            int index1 = tb.Columns.IndexOfColumn(columnName);
            foreach (DataRow row in tb.Rows)
            {
                ls.Add(convert(row[index1]));
            }
            return ls;
        }
        /// <summary>
        /// 把DataTable第一行的值转换成 T 类型
        /// </summary>
        /// <typeparam name="T">从DataMap或DataReadonly继承的类型</typeparam>
        /// <param name="tb">System.Data.DataTable</param>
        /// <returns></returns>
        public static T First<T>(this DataTable tb)
        {
            var mapper = Mapping.GetIMapper<T>();
            return mapper.First(tb);
        }
        /// <summary>
        /// 把DataTable转换成 T 类型的list集合
        /// </summary>
        /// <typeparam name="T">从DataMap或DataReadonly继承的类型</typeparam>
        /// <param name="tb">System.Data.DataTable</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable tb)
        {
            var mapper = Mapping.GetIMapper<T>();
            return mapper.ToList<T>(tb);
        }

        #endregion

        #region IDataReader

        private static int IndexOfColumn(this IDataReader reader, string columnName)
        {
            try
            {
                return reader.GetOrdinal(columnName);
            }
            catch (Exception ex)
            {
                throw new MapperException(string.Format("不包含列名：{0}。", columnName), ex);
            }
        }

        /// <summary>
        /// 获取IDataReader中第一行列columnName的值，如果IDataReader没有数据时，返回<typeparamref name="T"/>的默认值。
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="columnName">要获取的字段</param>
        /// <returns>columnName字段中的数据</returns>
        public static T FirstRowVal<T>(this IDataReader reader, string columnName)
        {
            if (reader.Read())
            {
                var val = reader[columnName];
                if (val is T)
                    return (T)val;
                else if (val == DBNull.Value || val == null)
                    return default(T);
                else
                    return (T)Convert.ChangeType(val, typeof(T));
            }
            else
                return default(T);
        }
        /// <summary>
        /// 获取IDataReader中的行转换成ItemValue对象。
        /// </summary>
        /// <typeparam name="TItem">键的类型</typeparam>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="itemColumn">对应Item的列名称</param>
        /// <param name="valueColumn">对应Value的列名称</param>
        /// <returns></returns>
        public static IList<KeyValuePair<TItem, TValue>> ToItemValues<TItem, TValue>(this IDataReader reader, string itemColumn, string valueColumn)
        {
            return reader.ToItemValues<TItem, TValue>(itemColumn, valueColumn, x => (TItem)x, y => (TValue)y);
        }
        /// <summary>
        /// 获取IDataReader中的行转换成ItemValue对象。
        /// </summary>
        /// <typeparam name="TItem">键的类型</typeparam>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="itemColumn">对应Item的列名称</param>
        /// <param name="valueColumn">对应Value的列名称</param>
        /// <param name="itemConvert">键转换，自定义<paramref name="itemColumn"/>字段的转换</param>
        /// <param name="valueConvert">值转换，自定义<paramref name="valueColumn"/>字段的转换</param>
        /// <returns></returns>
        public static IList<KeyValuePair<TItem, TValue>> ToItemValues<TItem, TValue>(this IDataReader reader, string itemColumn, string valueColumn, Func<object, TItem> itemConvert, Func<object, TValue> valueConvert)
        {
            if (!reader.Read())
                return new List<KeyValuePair<TItem, TValue>>(0);

            List<KeyValuePair<TItem, TValue>> ls = new List<KeyValuePair<TItem, TValue>>(reader.RecordsAffected == 0 ? 4 : reader.RecordsAffected);
            int index1 = reader.IndexOfColumn(itemColumn);
            int index2 = reader.IndexOfColumn(valueColumn);
            do
            {
                ls.Add(new KeyValuePair<TItem, TValue>(itemConvert(reader[index1]), valueConvert(reader[index2])));
            } while (reader.Read());
            return ls;
        }
        /// <summary>
        /// 将IDataReader指定的列以集合的方式取出
        /// </summary>
        /// <typeparam name="T">字段的数据类型</typeparam>
        /// <param name="tb">System.Data.DataTable</param>
        /// <param name="columnName">字段名称</param>
        /// <returns>columnName字段中的数据集合</returns>
        public static IList<T> ToValues<T>(this IDataReader reader, string columnName)
        {
            return reader.ToValues<T>(columnName, x => (T)x);
        }
        /// <summary>
        /// 将IDataReader指定的列以集合的方式取出
        /// </summary>
        /// <typeparam name="T">字段的数据类型</typeparam>
        /// <param name="tb">System.Data.DataTable</param>
        /// <param name="columnName">字段名称</param>
        /// <returns>columnName字段中的数据集合</returns>
        public static IList<T> ToValues<T>(this IDataReader reader, string columnName, Func<object, T> convert)
        {
            if (!reader.Read())
                return new List<T>(0);

            List<T> ls = new List<T>(reader.RecordsAffected == 0 ? 4 : reader.RecordsAffected);
            int index1 = reader.IndexOfColumn(columnName);
            do
            {
                ls.Add(convert(reader[index1]));
            } while (reader.Read());
            return ls;
        }
        /// <summary>
        /// 把IDataReader第一行的值转换成 T 类型
        /// </summary>
        /// <typeparam name="T">从DataMap或DataReadonly继承的类型</typeparam>
        /// <param name="table">System.Data.IDataReader</param>
        /// <returns></returns>
        public static T First<T>(this IDataReader reader)
        {
            var mapper = Mapping.GetIMapper<T>();
            return mapper.First(reader);
        }
        /// <summary>
        /// 把IDataReader转换成 T 类型的list集合
        /// </summary>
        /// <typeparam name="T">从DataMap或DataReadonly继承的类型</typeparam>
        /// <param name="result">System.Data.DataTable</param>
        /// <returns></returns>
        public static IList<T> ToList<T>(this IDataReader result)
        {
            var mapper = Mapping.GetIMapper<T>();
            return mapper.ToList<T>(result);
        }

        #endregion
    }
}
