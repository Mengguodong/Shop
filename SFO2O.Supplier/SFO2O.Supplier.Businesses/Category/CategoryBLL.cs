using SFO2O.Supplier.DAO.Category;
using SFO2O.Supplier.Models;
using SFO2O.Supplier.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Businesses.Category
{
    public class CategoryBLL
    {
        private static readonly CategoryDAL categoryDAL = new CategoryDAL();

        public static List<CategoryModel> GetFristLevelCategories(LanguageEnum languageVersion)
        {
            return categoryDAL.GetFristLevelCategories(languageVersion).ToList();
        }

        public static List<CategoryModel> GetChildrenCategories(LanguageEnum languageVersion, CategoryLevelEnum categoryLevel, int parentID)
        {
            return categoryDAL.GetChildrenCategories(languageVersion, categoryLevel, parentID).ToList();
        }

        public static List<CategoryHistoryModel> GetCategoryHistories(LanguageEnum languageVersion, int supplierID)
        {
            return categoryDAL.GetCategoryHistoriesBySupplierID(languageVersion, supplierID).ToList();
        }

        public static List<CategoryHistoryModel> GetCategoriesByCategoryName(LanguageEnum languageVersion, string categoryName)
        {
            return categoryDAL.GetCategoriesByProductName(languageVersion, categoryName).ToList();
        }

        public static CategoryHistoryModel GetCategoriesInfoByID(LanguageEnum languageEnum, int categoryId)
        {
            return categoryDAL.GetCategoriesInfoByID(languageEnum, categoryId);
        }
    }
}
