using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Order
{
    public class NoPayOrderModel
    {
        public string OrderCode { get; set; }
        public decimal PayAmount { get; set; }
        public string Message { get; set; }
        public bool HasError { get; set; }
    }
}
