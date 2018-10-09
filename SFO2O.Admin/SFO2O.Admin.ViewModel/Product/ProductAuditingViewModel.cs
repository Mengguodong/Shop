using SFO2O.Admin.Models.Enums;
using SFO2O.Admin.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel.Product
{
    public class ProductAuditingViewModel
    {
        public Dictionary<LanguageEnum, ProductBaseInfoModel> SpuBaseInfo { get; set; }

        public ProductPackingModel PackingInfo { get; set; }

        public ProductSysPropertyModel SysInfo { get; set; }

        public List<ProductSkuCustomInfoModel> ProductCustomInfos { get; set; }

        public Dictionary<LanguageEnum, Dictionary<string, string>> ProductAttrsInfos { get; set; }

        public List<ProductImgInfoModel> ProductImgs { get; set; }
    }

    public class ProductPackingModel
    {
        public decimal Weight { get; set; }

        public decimal Volume { get; set; }

        public decimal Length { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }
    }

    public class ProductSysPropertyModel
    {
        
        public string Spu { get; set; }

      
        public string CategoryName { get; set; }

      
        public string CompanyName { get; set; }

       
        public string SalesTerritory { get; set; }

      
        public decimal CommissionInCHINA { get; set; }

      
        public decimal CommissionInHK { get; set; }

        
        public int MinForOrder { get; set; }

       
        public int IsExchangeInCHINA { get; set; }

       
        public int IsExchangeInHK { get; set; }

       
        public int IsReturn { get; set; }

       
        public DateTime PreOnSaleTime { get; set; }

       
        public DateTime ModifyTime { get; set; }
    }
}
