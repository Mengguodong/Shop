using SFO2O.Admin.Models.Customer;
using SFO2O.Admin.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Models.Refund
{
    public class RefundDetailInfo
    {
        public RefundOrderInfo OrderInfo{get;set;}
        public List<RefundProductInfo> OrderProducts { get; set; }        
        public OrderInfo BuyerInfo { get; set; }
        public Supplier.SupplierAbstractModel SellerInfo { get; set; }
        public int IsFinance { get; set; }
        public int IsAudit { get; set; }
    }
}
