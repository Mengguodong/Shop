using System;

namespace SFO2O.Model.Shopping
{
    /// <summary>
    ///本Model 所有价格均为换汇之后价格
    /// Cart开头的字段均不参与实际计算，仅作比较使用，CartQuantity除外
    /// </summary>
    public class ProductItem
    {
        public ProductItem(decimal exchangeRate)
        {
            this.ExchangeRate = exchangeRate;
        }
        public int ProductId { get; set; }

        public string Spu { get; set; }

        public int SupplierId { get; set; }

        public string Name { get; set; }

        public int IsDutyOnSeller { get; set; }

        public string Sku { get; set; }

        public decimal TaxRate { get; set; }

        public bool IsOnSaled { get; set; }

        public string MainDicValue { get; set; }

        public string SubDicValue { get; set; }

        /// <summary>
        /// sku主值 eg. color
        /// </summary>
        public string MainValue { get; set; }
        /// <summary>
        /// sku子值 eg. color
        /// </summary>
        public string SubValue { get; set; }

        public int ForOrderQty { get; set; }

        /// <summary>
        /// 购物车存储的价格
        /// </summary>
        public decimal CartUnitPrice { get; set; }
        /// <summary>
        /// 购物车存储的Qty
        /// </summary>
        public int CartQuantity { get; set; }
        /// <summary>
        /// 购物车存储的税率
        /// </summary>
        public decimal CartTaxRate { get; set; }
        /// <summary>
        /// 购物车存储的汇率
        /// </summary>
        public decimal CartExchangeRate { get; set; }
        /// <summary>
        /// 购物车存储的汇率
        /// </summary>
        private decimal ExchangeRate {  get;  set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }




        /// <summary>
        /// 折扣后的实际销售价格。促销返回
        /// </summary>
        public decimal CartDiscountPrice { get; set; }

        /// <summary>
        /// 原价（汇前）
        /// </summary>
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 单价（汇后）
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 促销信息（汇后）
        /// </summary>
        public PromotionItem Promotion { get; set; }
        /// <summary>
        /// 订单创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 实际售价，有促销则返回促销价
        /// </summary>
        public decimal SalePrice
        {
            get
            {
                if (this.Promotion != null)
                {
                    return this.Promotion.DiscountPrice;
                }
                else
                {
                    return this.Price;
                }
            }
        }

        public decimal TaxAmount
        {
            get
            {
                return this.SalePrice * this.TaxRate;

            }
        }
        /// <summary>
        /// 促销价/商品售价 * Qty  单品小计(RMB)（汇后）
        /// </summary>
        public decimal TotalPrice
        {
            get
            {
                return this.SalePrice * this.CartQuantity;
            }
        }

        /// <summary>
        /// 单品总关税
        /// </summary>
        public decimal TotalTaxAmount
        {
            get
            {
                return this.SalePrice * this.CartQuantity * this.TaxRate;

            }
        }
        /// <summary>
        /// 价格是否有变动
        /// </summary>
        public bool IsPriceChange
        {
            get
            {
                if (CartDiscountPrice > 0)
                {
                    return CartDiscountPrice * ExchangeRate != this.SalePrice;
                }
                else
                {
                    return CartUnitPrice * ExchangeRate != this.SalePrice;
                }
                
            }
        }

    }
}
