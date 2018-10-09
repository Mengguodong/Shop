using System.Collections.Generic;
using System.Linq;

namespace SFO2O.Model.Shopping
{
    public partial class CartModel
    {
        public decimal ExchangeRate { get; private set; }

        /// <summary>
        /// 总金额
        /// </summary>

        public decimal TotalPrice
        {
            get
            {
                return Items == null ? 0 : Items.Where(n => n.IsChecked).Sum(n => n.TotalPrice);
            }
        }

        /// <summary>
        /// 总关税
        /// </summary>
        public decimal TotalTariff
        {
            get { return Items == null ? 0 : Items.Where(n => n.IsChecked).Sum(n => n.TotalTaxAmount); }
        }


        /// <summary>
        /// 获取用户已经购买的商品
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductItem> Items { get; set; }
        /// <summary>
        /// 已经下架的商品
        /// </summary>
        public IEnumerable<ProductItem> InvalidItems { get; set; }

        public CartModel(decimal rate)
        {
            this.ExchangeRate = rate;
        }
    }
}
