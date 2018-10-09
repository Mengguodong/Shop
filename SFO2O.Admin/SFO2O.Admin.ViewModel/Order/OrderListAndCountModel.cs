using SFO2O.Admin.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel.Order
{
    public class OrderListAndCountModel
    {
        public OrderListTotalModel Total { get; set; }

        public PageOf<OrderListModel> OrderList { get; set; }
    }
}
