using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Promotion
{
    /// <summary>
    /// SkuPromotion信息
    /// </summary>
    public class PromotionItem
    {

        public int PromotionId { get; set; }

        public string Sku { get; set; }

        public decimal DiscountRate { get; set; }
        /// <summary>
        /// 折扣价-汇前
        /// </summary>
        public decimal DiscountPrice { get; set; }
        /// <summary>
        /// 折扣价-汇后
        /// </summary>
        public decimal DiscountPriceExchanged { get; set; }

        public int SupplierId { get; set; }

        public string PromotionName { get; set; }


        public DateTime StartTime { get; set; }


        public DateTime EndTime { get; set; }


        public string PromotionLable { get; set; }


        public int PromotionStatus { get; set; }

        public int PromotionType { get; set; }

    }
}
