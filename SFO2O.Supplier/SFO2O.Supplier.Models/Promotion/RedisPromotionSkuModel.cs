using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Models.Promotion
{
    public class RedisPromotionSpuModel
    {
        public string Spu { get; set; }

        public DateTime AddTime { get; set; }

        public List<RedisPromotionSkuModel> Skus { get; set; }
    }

    public class RedisPromotionSkuModel
    {
        public string sku { get; set; }

        public decimal PromotionPrice { get; set; }

        public decimal PromotionRate { get; set; }
    }
}
