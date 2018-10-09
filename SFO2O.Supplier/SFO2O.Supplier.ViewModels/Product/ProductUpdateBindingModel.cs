using SFO2O.Supplier.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.ViewModels.Product
{
    public class ProductUpdateBindingModel
    {
        /// <summary>
        /// Product/product_Temp表数据
        /// 繁体
        /// </summary>
        public Dictionary<string, string> ProductInfo_T;

        /// <summary>
        /// Product/product_Temp表数据
        /// 简体
        /// </summary>
        public Dictionary<string, string> ProductInfo_S;

        /// <summary>
        /// Product/product_Temp表数据
        /// 英文
        /// </summary>
        public Dictionary<string, string> ProductInfo_E;



        /// <summary>
        /// ProductInfoExpand/ProductInfoExpand_Temp表数据
        /// 繁体
        /// </summary>
        public Dictionary<string, string> ProductInfoExpand_T;

        /// <summary>
        /// ProductInfoExpand/ProductInfoExpand_Temp表数据
        /// 简体
        /// </summary>
        public Dictionary<string, string> ProductInfoExpand_S;

        /// <summary>
        /// ProductInfoExpand/ProductInfoExpand_Temp表数据
        /// 英文
        /// </summary>
        public Dictionary<string, string> ProductInfoExpand_E;


        /// <summary>
        /// SkuInfo/SkuInfo_Temp表数据
        /// 繁体
        /// </summary>
        public List<Dictionary<string, string>> SkuInfo_T;

        /// <summary>
        /// SkuInfo/SkuInfo_Temp表数据
        /// 简体
        /// </summary>
        public List<Dictionary<string, string>> SkuInfo_S;

        /// <summary>
        /// SkuInfo/SkuInfo_Temp表数据
        /// 英文
        /// </summary>
        public List<Dictionary<string, string>> SkuInfo_E;

        /// <summary>
        /// 商品图片列表
        /// </summary>
        public List<ProductImgModel> Imgs;
    }
}
