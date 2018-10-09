using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.M.ViewModel.Promotion;
using SFO2O.Utility.Extensions;
using System.Web;
namespace SFO2O.M.ViewModel.ShoppingCart
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
        /// 实际汇率
        /// </summary>
        private decimal ExchangeRate { get; set; }
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
        public decimal Price { get; set; }
        /// <summary>
        /// 单价（汇后）
        /// </summary>
        public decimal PriceExchanged { get; set; }

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
        public decimal SalePriceExchanged
        {
            get
            {
                if (this.Promotion != null)
                {
                    return this.Promotion.DiscountPriceExchanged.ToNumberRound(2);
                }
                else
                {
                    return this.PriceExchanged.ToNumberRound(2);
                }
            }
        }
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
        public decimal CommissionInCHINA { get; set; }

        public decimal CommissionInHK { get; set; }

        public string NetWeightUnit { get; set; }

        public string NetContentUnit { get; set; }
        //增加两个字段  是否报关  是否电商综合税
        //是否报关
        public int ReportStatus { get; set; }
        //是否电商综合税
        public int IsCrossBorderEBTax { get; set; }

        /// <summary>
        /// 排序时间
        /// </summary>
        public DateTime SortTime { get; set; }

        /// <summary>
        /// 真实的汇率类型（1、综合税 2、行邮税）
        /// </summary>
        public int RealTaxType { get; set; }

        /// <summary>
        /// 行邮税
        /// </summary>
        public decimal PPATaxRate { get; set; }

        /// <summary>
        /// 商品税
        /// </summary>
        public decimal CBEBTaxRate { get; set; }
        /// <summary>
        /// 消费税率
        /// </summary>
        public decimal ConsumerTaxRate { get; set; }
        /// <summary>
        /// 增值税率
        /// </summary>
        public decimal VATTaxRate { get; set; }
        /// <summary>
        /// 关口类型
        /// </summary>
        public IEnumerable<Model.Shopping.ShoppingCartGatewayEntity> GatewayCodes { get; set; }

        public decimal Huoli { get; set; }

        public decimal GiftCard { get; set; }

        public decimal TaxAmountExchanged
        {
            get
            {
                return Utility.Uitl.TotalTaxHelper.GetTotalTaxAmount(this.RealTaxType, this.SalePriceExchanged, this.CBEBTaxRate, this.ConsumerTaxRate, this.VATTaxRate, this.PPATaxRate);

            }
        }
        /// <summary>
        /// 促销价/商品售价 * Qty  单品小计(RMB)（汇后）
        /// </summary>
        public decimal TotalPriceExchanged
        {
            get
            {
                return this.SalePriceExchanged * this.CartQuantity;
            }
        }

        /// <summary>
        /// 单品总商品税
        /// </summary>
        public decimal TotalTaxAmountExchanged
        {
            get
            {
                return Utility.Uitl.TotalTaxHelper.GetTotalTaxAmount(this.RealTaxType, this.SalePriceExchanged, this.CBEBTaxRate, this.ConsumerTaxRate, this.VATTaxRate, this.PPATaxRate) * this.CartQuantity;

            }

        }
        /// <summary>
        /// 价格是否有变动
        /// </summary>
        public decimal DifferencePrice
        {
            get
            {
                decimal differencePrice = 0;
                if (CartDiscountPrice > 0)
                {
                    differencePrice = (CartDiscountPrice * CartExchangeRate).ToNumberRound(2) - this.SalePriceExchanged;
                }
                else
                {
                    differencePrice = (CartUnitPrice * CartExchangeRate).ToNumberRound(2) - this.SalePriceExchanged;
                }
                return differencePrice;
            }
        }

    }
}
