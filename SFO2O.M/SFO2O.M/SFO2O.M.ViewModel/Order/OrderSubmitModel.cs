using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Order
{
    public class OrderSubmitModel
    {
        public OrderSubmitModel()
        {
            Sku = "";
            Quy = 0;
            AddressId = 0;
            OrderCode = "";
            Type = 1;
            hasActivity = 0;
            GiftCardId = 0;
            pid = 0;
            TeamCode = "";
        }

        public string Sku { get; set; }
        public int Quy { get; set; }
        public int AddressId { get; set; }
        public string OrderCode { get; set; }
        /// <summary>
        /// 1立即下单，2购物车下单，3从新下单
        /// </summary>
        public int Type { get; set; }

        public int hasActivity { get; set; }

        /// <summary>
        /// 2016.6.1 使用的优惠券ID
        /// </summary>
        public int GiftCardId { get; set; }

        public int pid { get; set; }

        public string TeamCode { get; set; }
    }
}
