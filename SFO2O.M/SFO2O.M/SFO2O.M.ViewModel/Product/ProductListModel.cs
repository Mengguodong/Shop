using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Model.Category;


namespace SFO2O.M.ViewModel.Product
{
    public class ProductListModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int CategoryLevel { get; set; }

        public List<CategoryAttribute> CategoryAttributes { get; set; }

        public List<CategoryModels> CategorysList { get; set; }

        public List<SFO2O.Model.Product.ProductInfoModel> Products { get; set; }

        public int TotalRecord { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }

    }
    public class CategoryModels : CategoryEntity
    {
        public List<CategoryEntity> ChildCategorys { get; set; }
    }
    /// <summary>
    /// 商品列表页筛选项
    /// </summary>
    public class CateAttribute
    {
        public string KeyName { get; set; }
        public string Name { get; set; }
        public List<string> KeyValues { get; set; }
        public int IsSkuAttr { get; set; }
    }
   
}
