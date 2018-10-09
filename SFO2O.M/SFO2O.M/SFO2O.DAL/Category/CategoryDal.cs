using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFO2O.Model.Category;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Utility.Uitl;

namespace SFO2O.DAL.Category
{
    public class CategoryDal:BaseDal
    {
        /// <summary>
        /// 获取全部分类信息
        /// </summary>
        /// <returns></returns>
        public IList<CategoryEntity> GetCategoryAll(int language)
        {
            string sql = @"SELECT   Category.CategoryId, Category.RootId, Category.ParentId, Category.SortValue, Category.Status,
                                    Category_LanguageVersion.CategoryName, Category.CategoryLevel,Category.CreateTime,
                                    Category.CreateBy, Category_LanguageVersion.SiteImgUrl, Category_LanguageVersion.AppImgUrl,
                                    Category_LanguageVersion.Aliases
                           FROM     Category (NOLOCK) LEFT OUTER JOIN Category_LanguageVersion (NOLOCK)
                           ON       Category.CategoryKey = Category_LanguageVersion.CategoryKey 
                           WHERE     Category.Status=1 AND Category_LanguageVersion.LanguageVersion = @LanguageVersion";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("LanguageVersion", language);
            return db.ExecuteSqlList<CategoryEntity>(sql, parameters);
        }
        /// <summary>
        /// 取三级分类的筛选属性
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IList<CategoryAttribute> GetCategoryAttribute(int categoryId, int language)
        {
            string sql = @"select  ca.CategoryId,lan.CategoryName,ca.KeyName, d.KeyValue ,dd.KeyName as SubKeyName,dd.KeyValue as SubKeyValue,ca.IsSkuMainAttr,ca.IsSkuAttr
                            from Category_Attributes ca
                            inner join Dics d on ca.KeyName=d.KeyName and d.LanguageVersion=@LanguageVersion
                            inner join Category_LanguageVersion lan on lan.CategoryKey=ca.CategoryId
                            inner join Dics dd on dd.DicType=d.KeyName and dd.LanguageVersion=@LanguageVersion
                            where  ca.IsFilter=1  and lan.LanguageVersion=@LanguageVersion and d.DicType='ProductAttributes' and ca.CategoryId=@CategoryId
                             order by IsSkuMainAttr desc,IsSkuAttr desc ";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@LanguageVersion",language);
            parameters.Append("@CategoryId",categoryId);
            return db.ExecuteSqlList<CategoryAttribute>(sql, parameters);
        }
        /// <summary>
        /// 取三级分类所属一级分类id
        /// </summary>
        /// <param name="categoryId">三级分类id</param>
        /// <returns></returns>
        public int GetFirstCategoryid(int categoryId)
        {
            string sql = @"Select RootId From Category nolock Where CategoryId=@CategoryId";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@CategoryId", categoryId);
            return db.ExecuteSqlScalar<int>(sql, parameters);
        }
    }
}
