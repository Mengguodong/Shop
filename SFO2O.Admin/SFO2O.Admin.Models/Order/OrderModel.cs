using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Models.Order
{
    public class OrderModel
    {
        public OrderInfoModel MainOrder { get; set; }

        public List<OrderDetailModel> OrderDetails { get; set; }

        public List<OrderLogisticsModel> OrderLogistics { get; set; }
    }
}
