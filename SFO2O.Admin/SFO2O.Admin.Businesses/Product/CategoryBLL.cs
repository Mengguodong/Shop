using SFO2O.Admin.DAO.Product;
using SFO2O.Admin.Models.Enums;
using SFO2O.Admin.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Businesses.Product
{
    public class CategoryBLL
    {
        private static readonly CategoryDAL categoryDAL = new CategoryDAL();

        public static List<CategoryModel> GetChildrenCategories(LanguageEnum languageVersion, int categoryLevel, int parentID)
        {
            return categoryDAL.GetChildrenCategories(languageVersion, categoryLevel, parentID).ToList();
        }
    }
}
