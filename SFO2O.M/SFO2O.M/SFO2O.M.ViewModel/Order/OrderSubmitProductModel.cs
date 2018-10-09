using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFO2O.M.ViewModel.ShoppingCart;
using SFO2O.Model.Shopping;

namespace SFO2O.M.ViewModel.Order
{
    public class OrderSubmitProductModel
    {
        public OrderSubmitProductModel(decimal rate)
        {
            this.ExchangeRate = rate;
            this.AddressId = 0;
            this.OrderStatus = 0;
        }

        public decimal ExchangeRate { get; private set; }

        /// <summary>
        /// 总金额
        /// </summary>

        public decimal TotalPrice
        {
            get { return TotalProductPrice + BuyTariff; }
        }

        /// <summary>
        /// 商品总金额
        /// </summary>

        public decimal TotalProductPrice
        {
            get { return Items == null ? 0 : Items.Sum(n => n.TotalPriceExchanged); }
        }

        /// <summary>
        /// 总商品税
        /// </summary>
        public decimal TotalTariff
        {
            get
            {
                if (this.OrderStatus > 0)
                {
                    // return Items == null ? 0 : Items.Sum(n => n.TaxAmountDisplay);
                    return Items == null ? 0 : Items.Sum(n => n.TotalTaxAmountExchanged);
                }
                else
                {
                    //return Items == null ? 0 : Items.Sum(n => n.TaxAmountDisplay * n.CartQuantity);
                    return Items == null ? 0 : Items.Sum(n => n.TotalTaxAmountExchanged);
                }
            }
        }

        /// <summary>
        /// 卖家承担总商品税
        /// </summary>
        public decimal TotalSellTariff
        {
            get
            {
                if (this.OrderStatus > 0)
                {
                    //return Items == null ? 0 : Items.Where(s => s.IsDutyOnSeller == 1).Sum(n => n.TaxAmountDisplay);
                    return Items == null ? 0 : Items.Where(s => s.IsDutyOnSeller == 1).Sum(n => n.TotalTaxAmountExchanged);
                }
                else
                {
                    //return Items == null ? 0 : Items.Where(s => s.IsDutyOnSeller == 1).Sum(n => n.TaxAmountDisplay * n.CartQuantity);
                    return Items == null ? 0 : Items.Where(s => s.IsDutyOnSeller == 1).Sum(n => n.TotalTaxAmountExchanged);
                }

            }
        }

        /// <summary>
        /// 买家承担的商品税
        /// </summary>
        public decimal BuyTariff
        {

            get
            {
                if (Items.FirstOrDefault().RealTaxType == 1)
                {
                    return TotalTariff - TotalSellTariff;
                }
                return TotalTariff <= 50 ? 0 : TotalTariff - TotalSellTariff;
            }

            
        }

        /// <summary>
        /// 总件数
        /// </summary>
        public int TotaCount
        {
            get { return Items == null ? 0 : Items.Sum(n => n.CartQuantity); }
        }

        /// <summary>
        /// 获取用户已经购买的商品
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductItem> Items { get; set; }

        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int AddressId { get; set; }
        public string OrderCode { get; set; }
        public bool IsFixed { get; set; }
        public int OrderStatus { get; set; }
    }
}
