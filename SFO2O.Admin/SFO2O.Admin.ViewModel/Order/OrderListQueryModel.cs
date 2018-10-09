using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel.Order
{
    public class OrderListQueryModel
    {
        public DateTime CreateTimeStart { get; set; }
        public DateTime CreateTimeEnd { get; set; }
        public int OrderStatus { get; set; }
        public string SKU { get; set;}
        public string BuyerAccount { get;set; }
        public int SellerId{get;set;}
        public string OrderCode{get;set;}

        /// <summary>
        /// 城市编码 1、大陆 2、香港
        /// </summary>
        public int CountryCode { get; set; }
        
        /// <summary>
        /// 是否包含无效订单（交易关闭订单）
        /// </summary>
        public int IsExcludeCloseOrder { get; set; }
    }
}
