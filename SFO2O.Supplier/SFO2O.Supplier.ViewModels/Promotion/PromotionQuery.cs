using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.ViewModels.Promotion
{
    public class PromotionQuery
    {
        public PromotionQuery()
        {
            PromotionStatus = -1;
        }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string PromotionName { get; set; }

        public int PromotionStatus { get; set; }
    }
}
