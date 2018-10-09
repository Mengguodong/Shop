using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using SFO2O.Admin.Models.Enums;
using SFO2O.Admin.Models.Product;

namespace SFO2O.Admin.DAO.Product
{
    public class CategoryDAL : BaseDao
    {
        public IList<CategoryModel> GetChildrenCategories(LanguageEnum languageVersion, int categoryLevel, int parentID)
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
            parameters.Append("CategoryLevel", categoryLevel);
            parameters.Append("parentID", parentID);

            return db.ExecuteSqlList<CategoryModel>(sql, parameters);
        }

    }
}
