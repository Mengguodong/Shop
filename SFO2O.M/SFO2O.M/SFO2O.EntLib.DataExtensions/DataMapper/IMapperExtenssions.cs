using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.EntLib.DataExtensions.DataMapper
{
    /// <summary>
    /// 数据映射扩展方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class IMapperExtenssions
    {
        #region ToList<T>

        /// <summary>
        /// 把DataTable转换成 IList&lt;<typeparamref name="T"/>&gt;集合
        /// </summary>
        /// <param name="table">System.Data.DataTable</param>
        /// <returns>IList&lt;<typeparamref name="T"/>&gt;集合。</returns>
        public static List<T> ToList<T>(this IMapper<T> mapper, DataTable table)
        {
            if (table == null || table.Rows.Count < 1)
                return new List<T>(0);

            List<T> list = new List<T>(table.Rows.Count);
            var properties = mapper.GetProperties(table.Columns);
            mapper.Load<T>(list, table, properties);
            // 将对象置为修改状态
            SetDataMapToModified<T>(list);
            // 
            return list;
        }

        /// <summary>
        /// 把IDataReader转换成 IList&lt;<typeparamref name="T"/>&gt;集合
        /// </summary>
        /// <param name="table">System.Data.DataTable</param>
        /// <returns>IList&lt;<typeparamref name="T"/>&gt;集合。</returns>
        public static IList<T> ToList<T>(this IMapper<T> mapper, IDataReader reader)
        {
            if (reader == null)
                return new List<T>(0);

            int capacity = reader.RecordsAffected > 0 ? reader.RecordsAffected : 4;
            List<T> list = new List<T>(capacity);
            var properties = mapper.GetProperties(reader);
            mapper.Load<T>(list, reader, properties);
            // 将对象置为修改状态
            SetDataMapToModified<T>(list);
            //
            return list;
        }

        /// <summary>
        /// 把itemValues转换成 IList&lt;<typeparamref name="T"/>&gt;集合
        /// </summary>
        /// <param name="table">System.Data.DataTable</param>
        /// <returns>IList&lt;<typeparamref name="T"/>&gt;集合。</returns>
        public static IList<T> ToList<T>(this IMapper<T> mapper, IList<ItemValue<string, ItemValue[]>> itemValues)
        {
            if (itemValues == null || itemValues.Count < 1)
                return new List<T>(0);
            List<T> list = new List<T>(itemValues.Count);
            foreach (ItemValue<string, ItemValue[]> item in itemValues)
            {
                T t = mapper.Create();
                mapper.Load(t, item.Value);
                list.Add(t);
            }
            // 将对象置为修改状态
            SetDataMapToModified<T>(list);
            //
            return list;
        }

        #endregion

        #region Load<T>

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="dr">一行数据</param>
        public static void Load<T>(this IMapper<T> mapper, T output, DataRow dr)
        {
            if (output == null || dr == null)
                return;

            var cols = dr.Table.Columns;
            // 开始导入数据
            if (output is IEntity)
                (output as IEntity).Importing();

            foreach (var field in mapper.Schema.GetAllFields())
            {
                int index = cols.IndexOf(field);
                if (index < 0)
                    continue;
                mapper[field].SetValue(output, dr.ItemArray[index]);
            }

            if (output is IEntity)
                (output as IEntity).Imported();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="dr">一行数据</param>
        public static void Load<T>(this IMapper<T> mapper, T output, IDataRecord record)
        {
            if (output == null || record == null)
                return;

            int length = record.FieldCount;
            var fields = mapper.Schema.GetAllFields();
            // 开始导入数据
            if (output is IEntity)
                (output as IEntity).Importing();

            for (int i = 0; i < length; i++)
            {
                string field = record.GetName(i);
                field = fields.SingleOrDefault(x => x == field || x.ToLower() == field.ToLower());
                if (field != null)
                    mapper[field].SetValue(output, record[i]);
            }

            if (output is IEntity)
                (output as IEntity).Imported();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="items">成员字段的值</param>
        /// <remarks>不可以包含重复的字段，如果包含重复的字段将抛出异常。</remarks>
        public static void Load<T>(this IMapper<T> mapper, T output, ItemValue[] items)
        {
            if (output == null || items == null)
                return;
            // 开始导入数据
            if (output is IEntity)
                (output as IEntity).Importing();

            for (int i = 0; i < items.Length; i++)
            {
                ItemValue itv = items[i];
                var setter = mapper[itv.Item];
                if (setter != null)
                    setter.SetValue(output, itv.Value);
            }

            if (output is IEntity)
                (output as IEntity).Imported();
        }

        private static void Load<T>(this IMapper<T> mapper, IList<T> output, DataTable source, IPropertyValue<T>[] properties)
        {
            foreach (DataRow row in source.Rows)
            {
                T target = mapper.Create();
                // 开始导入数据
                if (target is IEntity)
                    (target as IEntity).Importing();

                for (int i = 0; i < source.Columns.Count; i++)
                {
                    var property = properties[i];
                    if (properties[i] != null)
                        properties[i].SetValue(target, row[i]);
                }
                output.Add(target);

                if (target is IEntity)
                    (target as IEntity).Imported();
            }
        }

        private static void Load<T>(this IMapper<T> mapper, IList<T> output, IDataReader source, IPropertyValue<T>[] properties)
        {
            object[] values = new object[source.FieldCount];
            while (source.Read())
            {
                T target = mapper.Create();
                // 开始导入数据
                if (target is IEntity)
                    (target as IEntity).Importing();

                source.GetValues(values);
                for (int i = 0; i < source.FieldCount; i++)
                {
                    var property = properties[i];
                    if (properties[i] != null)
                        properties[i].SetValue(target, values[i]);
                }
                output.Add(target);

                if (target is IEntity)
                    (target as IEntity).Imported();
            }
        }

        #endregion

        #region First

        /// <summary>
        /// 把DataTable第一行的值转换成 T 类型
        /// </summary>
        /// <typeparam name="T">从DataMap或DataReadonly继承的类型</typeparam>
        /// <param name="table">System.Data.DataTable</param>
        /// <returns></returns>
        public static T First<T>(this IMapper<T> mapper, DataTable table)
        {
            if (table == null || table.Rows.Count < 1)
                return default(T);// 没有数据，返回空。

            T t = mapper.Create();
            mapper.Load(t, table.Rows[0]);
            // 将对象置为修改状态
            SetDataMapToModified<T>(t);

            return t;
        }

        /// <summary>
        /// 把IDataReader第一行的值转换成 T 类型
        /// </summary>
        /// <typeparam name="T">从DataMap或DataReadonly继承的类型</typeparam>
        /// <param name="reader">System.Data.IDataReader</param>
        /// <returns></returns>
        public static T First<T>(this IMapper<T> mapper, IDataReader reader)
        {
            if (reader == null)
                return default(T);
            while (reader.Read())
            {
                T t = mapper.Create();
                mapper.Load(t, reader);
                // 将对象置为修改状态
                SetDataMapToModified<T>(t);

                return t;
            }
            return default(T);// 没有数据，返回空。
        }

        #endregion

        #region SetDataMapToModified<T>

        /// <summary>
        /// 将对象置为修改状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataMap"></param>
        private static void SetDataMapToModified<T>(T dataMap)
        {
            IEntity map = dataMap as IEntity;
            if (map != null)
                map.OperationState = DataState.Modify;
        }
        /// <summary>
        /// 将对象置为修改状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataMaps"></param>
        private static void SetDataMapToModified<T>(IList<T> dataMaps)
        {
            bool isDataMap = false;
            foreach (T t in dataMaps)
            {
                if (isDataMap == false)
                {
                    isDataMap = (t as IEntity) != null;
                    if (isDataMap == false)
                        return;
                }
                IEntity map = t as IEntity;
                map.OperationState = DataState.Modify;
            }
        }

        #endregion

        #region indexer

        /// <summary>
        /// 计算数据导入DataMap中的顺序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <returns>返回的数组中记录取数的下标（DataRow中数据的下标），如果值为-1表示DataMap的成员字段没有被包含在cols中，所以请注意在从DataRow中取值之前要对值进行验证是否为-1。</returns>
        /// <remarks></remarks>
        private static IPropertyValue<T>[] GetProperties<T>(this IMapper<T> mapper, DataColumnCollection cols)
        {
            IList<IPropertyValue<T>> list = new List<IPropertyValue<T>>(cols.Count);
            for (int i = 0; i < cols.Count; i++)
                list.Add(null);
            // 考虑数据库里面不区分大小写，但.net里面区分。
            foreach (var field in mapper.Schema.GetAllFields())
            {
                int index = cols.IndexOf(field);//这是一个不区分大小写的方法
                if (index < 0)
                    continue;
                list[index] = mapper[field];
            }
            //
            return list.ToArray();
        }

        /// <summary>
        /// 计算数据导入DataMap中的顺序。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns>返回的数组中记录取数的下标（IDataReader中数据的下标），如果值为-1表示DataMap的成员字段没有被包含在reader中，所以请注意在从IDataRecord中取值之前要对值进行验证是否为-1。</returns>
        private static IPropertyValue<T>[] GetProperties<T>(this IMapper<T> mapper, IDataReader reader)
        {
            IList<IPropertyValue<T>> list = new List<IPropertyValue<T>>(reader.FieldCount);
            var fields = mapper.Schema.GetAllFields();
            // 考虑数据库里面不区分大小写，但.net里面区分。
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string field = reader.GetName(i);
                // 大部分时候返回的数据字段，与定义的字段大小写基本都是一致的，所以要考虑2/8原则（因为作大小写转换有性能损耗）。
                field = fields.SingleOrDefault(x => x == field || x.ToLower() == field.ToLower());
                if (field == null)
                    list.Add(null);
                else
                    list.Add(mapper[field]);
            }
            //
            return list.ToArray();
        }

        #endregion
    }
}
