using SFO2O.Admin.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Models.Settlement
{
    public class SettlementInfo
    {
        public SettlementOrderInfo OrderInfo { get; set; }
        public OrderPaymentInfo PaymentInfo {get;set;}
        public List<SettlementProductInfo> OrderProducts { get; set; }
    }
}
