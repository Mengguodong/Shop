using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel.Refund
{
    public class RefundQueryModel
    {
        public int RegionCode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RefundStatus { get; set; }
        public int RefundType { get; set; }
        public string BuyerName { get; set; }
        public int SellerName { get; set; }
        public string Sku { get; set; }
        public string OrderCode { get; set; }
        public string RefundCode { get; set; }
        public int IsFinance { get; set; }
    }
}
