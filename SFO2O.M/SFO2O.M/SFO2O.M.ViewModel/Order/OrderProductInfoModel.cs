using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Order
{
    public class OrderProductInfoModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 配送地址id
        /// </summary>
        public int AddressId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Language { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int DeliveryRegion { get; set; }

        /// <summary>
        /// 立即购买数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 立即购买spu
        /// </summary>
        public string Spu { get; set; }
        /// <summary>
        /// 立即购买Sku
        /// </summary>
        public string Sku { get; set; }
        /// <summary>
        /// ip
        /// </summary>
        public string strIp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal OrderLimitValue { get; set; }

        public int hasActivity { get; set; }

        /// <summary>
        /// 2016.6.1 优惠券ID，如果使用了的话会传过来券ID
        /// </summary>
        public int GiftCardId { get; set; }

        public int pid { get; set; }

        public string StationSource { get; set; }

        public int ChannelId { get; set; }

        public int StationSourceType { get; set; }

        public string TeamCode { get; set; }

        public decimal ExchangeRate { get; set; }
    }
}
