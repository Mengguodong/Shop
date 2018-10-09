using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.M.ViewModel.Promotion;

namespace SFO2O.M.ViewModel.Product
{
    public class SkuAttributeViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ImgUrl { get; set; }
        public string InitPrice { get; set; }

        public string MainName { get; set; }

        public string MainCode { get; set; }

        public string SubName { get; set; }

        public string SubCode { get; set; }

        /// <summary>
        /// 商品税
        /// </summary>
        public decimal TaxAmount { get; set; }

        public IList<SubSkuAttribute> SubAttributes { get; set; }

        public IList<MainSkuAttribute> MainAttributes { get; set; }

        /// <summary>
        /// 商家承担运费
        /// </summary>
        public int IsDutyOnSeller { get; set; }

        /// <summary>
        /// 税种判断 1：综合税 2：行邮税
        /// </summary>
        public int RealTaxType { get; set; }

    }

    public class MainSkuAttribute
    {
        /// <summary>
        /// 关联用
        /// </summary>
        public string MetaCode { get; set; }

        public string Id { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// -1：不可选，0：可选，1：选中
        /// </summary>
        public int Flag { get; set; }

        public IList<SubSkuAttribute> SubAttributes { get; set; }

        public string Sku { get; set; }

        public int ForOrder { get; set; }

       
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public PromotionItem Promotion { get; set; }

        /// <summary>
        /// 商品税
        /// </summary>
        public decimal TaxAmount { get; set; }
        /// <summary>
        /// NetWeightUnit
        /// </summary>
        public string NetWeightUnit { get; set; }

        /// <summary>
        /// 商家承担运费
        /// </summary>
        public int IsDutyOnSeller { get; set; }

        /// <summary>
        /// 税种判断 1：综合税 2：行邮税
        /// </summary>
        public int RealTaxType { get; set; }
    }
    //MainSkuAttribute 与 SubSkuAttribute 之所以一样是因为会有不存在SubSkuAttribute的情况所以mainsku上会包含sub的所有属性

    public class SubSkuAttribute
    {
        public string MetaCode { get; set; }
        public string Id { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string Name { get; set; }

        public string Sku { get; set; }

        public int ForOrder { get; set; }
        /// <summary>
        /// -1：不可选，0：可选，1：选中
        /// </summary>
        public int Flag { get; set; }

        public decimal Price { get; set; }

        public decimal OriginalPrice { get; set; }
        public PromotionItem Promotion { get; set; }

        /// <summary>
        /// 商品税
        /// </summary>
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// 商家承担运费
        /// </summary>
        public int IsDutyOnSeller { get; set; }

        /// <summary>
        /// 税种判断 1：综合税 2：行邮税
        /// </summary>
        public int RealTaxType { get; set; }
    }
}
