using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;

using SFO2O.Supplier.Models;
using SFO2O.Supplier.Models.Category;
namespace SFO2O.Supplier.DAO.Category
{
    public class CategoryDAL : BaseDao
    {
        /// <summary>
        /// 获取一级分类
        /// </summary>
        /// <returns></returns>
        public IList<CategoryModel> GetFristLevelCategories(LanguageEnum languageVersion)
        {
            var sql = @"SELECT  c.CategoryId,
		                        c.RootId,
		                        c.ParentId,
		                        c.SortValue,
		                        c.Status,
		                        c.CategoryLevel,
		                        cl.CategoryName,
		                        cl.AppImgUrl,
		                        cl.SiteImgUrl
                        FROM	Category c(NOLOCK)
		                        INNER JOIN Category_LanguageVersion cl(NOLOCK) ON cl.CategoryKey = c.CategoryKey
                        WHERE	cl.LanguageVersion=@LanguageVersion
		                        AND c.Status = 1 and CategoryLevel=0";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("LanguageVersion", (int)languageVersion);

            return db.ExecuteSqlList<CategoryModel>(sql, parameters);
        }

        public IList<CategoryModel> GetChildrenCategories(LanguageEnum languageVersion, CategoryLevelEnum categoryLevel, int parentID)
        {
            var sql = @"SELECT  c.CategoryId,
		                        c.RootId,
		                        c.ParentId,
		                        c.SortValue,
		                        c.Status,
		                        c.CategoryLevel,
		                        cl.CategoryName,
		                        cl.AppImgUrl,
		                        cl.SiteImgUrl
                        FROM	Category c(NOLOCK)
		                        INNER JOIN Category_LanguageVersion cl(NOLOCK) ON cl.CategoryKey = c.CategoryKey
                        WHERE	cl.LanguageVersion=@LanguageVersion
		                        AND c.Status = 1 and CategoryLevel=@CategoryLevel AND c.ParentId=@ParentId";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("LanguageVersion", (int)languageVersion);
            parameters.Append("CategoryLevel", (int)categoryLevel);
            parameters.Append("parentID", parentID);

            return db.ExecuteSqlList<CategoryModel>(sql, parameters);
        }

        public IList<CategoryHistoryModel> GetCategoryHistoriesBySupplierID(LanguageEnum languageVersion, int supplierID)
        {
            var sql = @"
                        WITH 
                        FC AS 
                        (
	                        SELECT  c.CategoryId,
		                        cl.CategoryName
                        FROM	Category c(NOLOCK)
		                        INNER JOIN Category_LanguageVersion cl(NOLOCK) ON cl.CategoryKey = c.CategoryKey
                        WHERE	cl.LanguageVersion=@LanguageVersion
		                        AND c.Status = 1 and CategoryLevel=0 
                        ),
                        SC AS
                        (
                        SELECT  c.CategoryId,
		                        cl.CategoryName
                        FROM	Category c(NOLOCK)
		                        INNER JOIN Category_LanguageVersion cl(NOLOCK) ON cl.CategoryKey = c.CategoryKey
                        WHERE	cl.LanguageVersion=@LanguageVersion
		                        AND c.Status = 1 and CategoryLevel=1
                        ),
                        TC AS
                        (
                        SELECT  c.CategoryId,
		                        cl.CategoryName
                        FROM	Category c(NOLOCK)
		                        INNER JOIN Category_LanguageVersion cl(NOLOCK) ON cl.CategoryKey = c.CategoryKey
                        WHERE	cl.LanguageVersion=@LanguageVersion
		                        AND c.Status = 1 and CategoryLevel=2
                        ),
                        CH AS
                        (
	                        SELECT TOP 5 c.*,ch.ModeifyTime FROM CategoryHistory(NOLOCK) ch
	                        INNER JOIN Category(NOLOCK) c ON
	                        ch.ThirdCategoryId=c.CategoryId  WHERE SupplierId=@SupplierId AND c.CategoryLevel=2 ORDER BY ModeifyTime desc
                        )
                        SELECT CH.RootId AS FCategoryID,FC.CategoryName AS FCategoryName,
                        CH.ParentId AS SCategoryID,SC.CategoryName AS SCategoryName,
                        CH.CategoryId AS TCategoryID,TC.CategoryName AS TCategoryName
                        FROM CH
                        INNER JOIN FC
                        ON CH.RootId= FC.CategoryId
                        INNER JOIN SC ON
                        SC.CategoryId = CH.ParentId
                        INNER JOIN TC ON
                        TC.CategoryId = CH.CategoryId
                        ORDER BY CH.ModeifyTime DESC";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("LanguageVersion", (int)languageVersion);
            parameters.Append("SupplierId", supplierID);

            return db.ExecuteSqlList<CategoryHistoryModel>(sql, parameters);
        }

        public IList<CategoryHistoryModel> GetCategoriesByProductName(LanguageEnum languageVersion, string categoryName)
        {
            var sql = @"
SELECT  c.RootId FCategoryID,
        clr.CategoryName FCategoryName,
        c.ParentId SCategoryID,
        cls.CategoryName SCategoryName,
        c.CategoryId TCategoryID,
        cl.CategoryName TCategoryName
FROM	Category c(NOLOCK)
        INNER JOIN Category_LanguageVersion cl(NOLOCK) ON cl.CategoryKey = c.CategoryKey
        INNER JOIN Category cs(NOLOCK) ON c.ParentId = cs.CategoryId
        INNER JOIN Category_LanguageVersion cls(NOLOCK) ON cls.CategoryKey = cs.CategoryKey
        INNER JOIN Category cr(NOLOCK) ON c.RootId = cr.CategoryId
        INNER JOIN Category_LanguageVersion clr(NOLOCK) ON clr.CategoryKey = cr.CategoryKey
WHERE	clr.LanguageVersion=@LanguageVersion AND cls.LanguageVersion=@LanguageVersion AND cl.LanguageVersion=@LanguageVersion
        AND c.Status = 1 and c.CategoryLevel=2 AND cl.CategoryName LIKE @CategoryName";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("LanguageVersion", (int)languageVersion);
            parameters.Append("CategoryLevel", (int)CategoryLevelEnum.Third);
            parameters.Append("CategoryName", "%" + categoryName + "%");

            return db.ExecuteSqlList<CategoryHistoryModel>(sql, parameters);
        }

        public CategoryHistoryModel GetCategoriesInfoByID(LanguageEnum languageVersion, int categoryId)
        {
            var sql = @"SELECT  c.RootId FCategoryID,
                                clr.CategoryName FCategoryName,
                                c.ParentId SCategoryID,
                                cls.CategoryName SCategoryName,
                                c.CategoryId TCategoryID,
                                cl.CategoryName TCategoryName
                        FROM	Category c(NOLOCK)
                                INNER JOIN Category_LanguageVersion cl(NOLOCK) ON cl.CategoryKey = c.CategoryKey
                                INNER JOIN Category cs(NOLOCK) ON c.ParentId = cs.CategoryId
                                INNER JOIN Category_LanguageVersion cls(NOLOCK) ON cls.CategoryKey = cs.CategoryKey
                                INNER JOIN Category cr(NOLOCK) ON c.RootId = cr.CategoryId
                                INNER JOIN Category_LanguageVersion clr(NOLOCK) ON clr.CategoryKey = cr.CategoryKey
                        WHERE	clr.LanguageVersion=@LanguageVersion AND cls.LanguageVersion=@LanguageVersion AND cl.LanguageVersion=@LanguageVersion
                                AND c.Status = 1 and c.CategoryLevel=2 AND c.CategoryId=@CategoryID";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("LanguageVersion", (int)languageVersion);
            parameters.Append("CategoryID", categoryId);

            return db.ExecuteSqlFirst<CategoryHistoryModel>(sql, parameters);

        }
    }
}
