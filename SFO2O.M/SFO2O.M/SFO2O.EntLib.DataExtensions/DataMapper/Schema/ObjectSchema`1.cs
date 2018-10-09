using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Microsoft.Practices.Unity;
using SFO2O.EntLib.DataExtensions.Basic;

namespace SFO2O.EntLib.DataExtensions.DataMapper.Schema
{
    /// <summary>
    /// 数据对象的架构映射(对象属性与数据字段的关系等信息描述)
    /// </summary>
    public class ObjectSchema<T> : BaseSchema
    {
        /// <summary>
        /// 默认初始化，此对象无法实现数据的添加删除，此初始化后的对象实体只能用于数据读取。
        /// <remarks>如果使用默认构造函数初始，则表示数据只从源（IDataReader/DataTable...）转换成实体，不能将实体数据通过“insert/update”等操作写回至数据源。</remarks>
        /// </summary>
        public ObjectSchema()
        {

        }

        /// <summary>
        /// 初始化，此对象可以实现数据的添加删除。
        /// </summary>
        /// <remarks>数据实体的映射是双向的，即可以将数据（IDataReader/DataTable...）转换成实体，也可以将实体中的数据通过“insert/update”等操作写回数据源。</remarks>
        /// <param name="name">对象名称（表名）。</param>
        public ObjectSchema(string name)
        {
            Check.IsNullOrEmpty(name, "对象名称（表名）。");
            // 指定对象的名称后,不但可以将数据从源转换成实体，还可以将实体数据写回数据源。
            this.Name = name;
        }

        /// <summary>
        /// 增加一个成员字段
        /// </summary>
        /// <param name="exp">成员字段映射的属性（Lambda表达式）</param>
        /// <param name="field">成员字段名称（请保证惟一）</param>
        /// <param name="isKey">是否作为主键使用</param>
        /// <returns></returns>
        public FieldMap<T> AddField(Expression<Func<T, object>> exp, string field)
        {
            return this.AddField(exp, field, false);
        }

        /// <summary>
        /// 增加一个成员字段
        /// </summary>
        /// <param name="exp">成员字段映射的属性（Lambda表达式）</param>
        /// <param name="field">成员字段名称（请保证惟一）</param>
        /// <param name="isKey">是否作为主键使用</param>
        /// <returns></returns>
        public FieldMap<T> AddField(Expression<Func<T, object>> exp, string field, bool isKey)
        {
            FieldMap<T> fieldMap = new FieldMap<T>(this, exp, field, isKey);
            this.AddField(fieldMap);
            return fieldMap;
        }

        ///// <summary>
        ///// 增加一个成员字段
        ///// </summary>
        ///// <param name="exp">成员字段映射的属性（Lambda表达式）</param>
        ///// <param name="field">成员字段名称（请保证惟一）</param>
        ///// <param name="isKey">是否作为主键使用</param>
        ///// <param name="owners">指示成员字段引用的数据实体信息。如：数据自动装载的时候装将依据froms架构实体信息。</param>
        ///// <returns></returns>
        //public FieldMap<T> AddField(Expression<Func<T, object>> exp, string field, bool isKey, params DataMapSchema[] owners)
        //{
        //    FieldMap<T> fieldMap = new FieldMap<T>(this, exp, field, isKey);
        //    this.AddField(fieldMap);
        //    _FromSchemas = owners;
        //    return fieldMap;
        //}

        public override void Compile()
        {
            if (IsReadonly)
                return;

            foreach (FieldMap<T> fieldMap in this.FieldMaps())
            {
                // 获取增量标识
                if (fieldMap.Identity != null)
                {
                    Check.Valid(this.Identity != null, "只允许有一个增量标识。");
                    Check.IsNull(this.Name, "Name", "未指定对象名称，无法设置对象的自增长标识。");

                    this.Identity = fieldMap.Identity;
                }
            }

            base.Compile();

            //
            Mapping.Register<T>(new ObjectMapper<T>(this));
        }
    }
}
