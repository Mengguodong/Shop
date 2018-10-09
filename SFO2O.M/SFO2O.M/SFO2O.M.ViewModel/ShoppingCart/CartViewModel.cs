using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.ShoppingCart
{
    public partial class CartViewModel
    {
        public decimal ExchangeRate { get; private set; }

        /// <summary>
        /// 总金额
        /// </summary>

        public decimal TotalPrice
        {
            get
            {
                return Items == null ? 0 : Items.Where(n => n.IsChecked).Where(x=>x.IsOnSaled==true).Sum(n => n.TotalPriceExchanged);
            }
        }

        /// <summary>
        /// 总商品税
        /// </summary>
        public decimal TotalTariff
        {
            get { return Items == null ? 0 : Items.Where(n => n.IsChecked).Where(x => x.IsOnSaled == true).Sum(n => n.TotalTaxAmountExchanged); }
        }

        /// <summary>
        /// 中华人民共和国大陆地区1号仓库
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductItem> HKOneWareHouseItems { get; set; }

        /// <summary>
        /// 中华人民共和国大陆地区2号仓库
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductItem> HKSecWareHouseItems { get; set; }

        /// <summary>
        /// 中华人民共和国大陆地区1号已经下架仓库
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductItem> HKOneWareHouseInvalidItems { get; set; }

        /// <summary>
        /// 中华人民共和国大陆地区2号已经下架仓库
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductItem> HKSecWareHouseInvalidItems { get; set; }

        /// <summary>
        /// 订单1
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductItem> ItemsOne { get; set; }

        /// <summary>
        /// 订单2
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductItem> ItemSec { get; set; }

        /// <summary>
        /// 获取用户已经购买的商品
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductItem> Items { get; set; }
        /// <summary>
        /// 已经下架的商品
        /// </summary>
        public IEnumerable<ProductItem> InvalidItems { get; set; }

        public CartViewModel(decimal rate)
        {
            this.ExchangeRate = rate;
        }
    }
}
