﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Common
{
    public static class DataMapHelper
    {
        /// <summary>
        /// 将Table数据转化为对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static IList<T> DataSetToList<T>(DataSet ds)
        {
            var lstT = new List<T>();

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable tb = ds.Tables[0];

                if (tb.Rows.Count > 0)
                {
                    //Type type = typeof(T);
                    //PropertyInfo[] propertyInfo = type.GetProperties();

                    T t = default(T);

                    t = Activator.CreateInstance<T>(); ////创建指定类型的实例
                    PropertyInfo[] propertyInfo = t.GetType().GetProperties(); //得到类的属性
                    //PropertyInfo[] propertypes = null;
                    //string tempName = string.Empty;
                    foreach (DataRow row in tb.Rows)
                    {

                        //foreach (PropertyInfo pro in propertypes)
                        //{
                        //tempName = pro.Name;
                        //if (tb.Columns.Contains(tempName.ToUpper()))
                        //{
                        //    object value = row[tempName];
                        //    if (value is System.DBNull)
                        //    {
                        //        value = "";
                        //    }
                        //    pro.SetValue(t, value, null);

                        lstT.Add(RetrunObject<T>(tb, propertyInfo, row));
                    }
                    //}

                    //}
                }
            }

            return lstT;
        }

        /// <summary>
        /// 将Table数据转化为对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static T DataSetToObject<T>(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable tb = ds.Tables[0];

                if (tb.Rows.Count > 0)
                {
                    T t = default(T);
                    //Type type = typeof(T);
                    t = Activator.CreateInstance<T>(); ////创建指定类型的实例
                    PropertyInfo[] propertyInfo = t.GetType().GetProperties(); //得到类的属性//type.GetProperties();
                    foreach (DataRow row in tb.Rows)
                    {
                        return RetrunObject<T>(tb, propertyInfo, row);
                    }
                }
            }
            return default(T);
        }


        public static T DataTableToObject<T>(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 0)
                {
                    T t = default(T);
                    //Type type = typeof(T);
                    t = Activator.CreateInstance<T>(); ////创建指定类型的实例
                    PropertyInfo[] propertyInfo = t.GetType().GetProperties(); //得到类的属性//type.GetProperties();
                    foreach (DataRow row in dt.Rows)
                    {
                        return RetrunObject<T>(dt, propertyInfo, row);
                    }
                }
            }
            return default(T);
        }

        public static IList<T> DataTableToList<T>(DataTable dt)
        {
            var lstT = new List<T>();

            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 0)
                {
                    //Type type = typeof(T);
                    //PropertyInfo[] propertyInfo = type.GetProperties();

                    T t = default(T);

                    t = Activator.CreateInstance<T>(); ////创建指定类型的实例
                    PropertyInfo[] propertyInfo = t.GetType().GetProperties(); //得到类的属性
                    //PropertyInfo[] propertypes = null;
                    //string tempName = string.Empty;
                    foreach (DataRow row in dt.Rows)
                    {

                        //foreach (PropertyInfo pro in propertypes)
                        //{
                        //tempName = pro.Name;
                        //if (tb.Columns.Contains(tempName.ToUpper()))
                        //{
                        //    object value = row[tempName];
                        //    if (value is System.DBNull)
                        //    {
                        //        value = "";
                        //    }
                        //    pro.SetValue(t, value, null);

                        lstT.Add(RetrunObject<T>(dt, propertyInfo, row));
                    }
                    //}

                    //}
                }
            }

            return lstT;
        }

        /// <summary>
        /// 将Table数据转化为对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tb"></param>
        /// <param name="propertyInfo"></param>
        /// <param name="row"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static T RetrunObject<T>(DataTable tb, PropertyInfo[] propertyInfo, DataRow row)
        {
            T t = Activator.CreateInstance<T>(); ////创建指定类型的实例//(T) type.Assembly.CreateInstance(type.FullName);

            foreach (DataColumn col in tb.Columns)
            {
                var r = propertyInfo.Where(p => p.Name.ToLower() == col.ColumnName.ToLower());
                if (r.Any())
                {
                    PropertyInfo pi = r.First();
                    object obj = (row[col.ColumnName] == DBNull.Value) ? "" : row[col.ColumnName];
                    if (!string.IsNullOrEmpty(obj.ToString()))
                    {
                        if (col.DataType == typeof(DateTime) && pi.PropertyType == typeof(String))
                            obj = ((DateTime)row[col.ColumnName]).ToString();
                        if (col.DataType == typeof(String) && pi.PropertyType == typeof(DateTime))
                        {
                            DateTime temp;
                            obj = DateTime.TryParse(obj.ToString(), out temp);
                        }
                    }
                    //时间类型或者整形,则跳过赋值
                    if (string.IsNullOrEmpty(obj.ToString())
                        && (pi.PropertyType == typeof(DateTime)
                            || pi.PropertyType == typeof(int)
                            || pi.PropertyType == typeof(Int16)
                            || pi.PropertyType == typeof(Int32)
                            || pi.PropertyType == typeof(Int64))) continue;
                    try
                    {
                        pi.SetValue(t, obj, null);
                    }
                    catch
                    {
                    }
                }
            }

            return t;
        }
    }
}
