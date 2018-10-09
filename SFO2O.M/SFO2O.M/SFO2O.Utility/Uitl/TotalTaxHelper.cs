using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Utility.Uitl
{
    public class TotalTaxHelper
    {
        /// <summary>
        /// 获取总税金
        /// </summary>
        /// <param name="RealTaxType">真实的税的类型（1、综合税 2、行邮税）</param>
        /// <param name="salePrice">销售价</param>
        /// <param name="CBEBTaxRate">商品税</param>
        /// <param name="ConsumerTaxRate">消费税</param>
        /// <param name="VATTaxRate">增值税</param>
        /// <param name="PPATaxRate">行邮税</param>
        /// <returns>返回总税金</returns>
        public static decimal GetTotalTaxAmount(int RealTaxType,decimal salePrice,decimal CBEBTaxRate, decimal ConsumerTaxRate, decimal VATTaxRate,decimal PPATaxRate = 0)
        {
            //if (RealTaxType == 1)
            //{
            //    var greatTax = salePrice * CBEBTaxRate;
            //    var cusTax = (salePrice + greatTax) / (1 - ConsumerTaxRate) * ConsumerTaxRate;
            //    var vvTax = (salePrice + greatTax + cusTax) * VATTaxRate;
            //    return Math.Round(((greatTax + cusTax + vvTax) * Utility.Uitl.ConfigHelper.CustomsDutiesRate), 2, MidpointRounding.AwayFromZero);
            //}
            //return Math.Round(salePrice * PPATaxRate, 2, MidpointRounding.AwayFromZero);
            return 0;//商品税始终为0
        }

        /// <summary>
        /// 获取SKU的真实的税费类型
        /// </summary>
        /// <param name="isReport"></param>
        /// <param name="taxType"></param>
        /// <param name="validPrice"></param>
        /// <returns></returns>
        public static int GetRealTaxType(int isReport, int taxType, decimal validPrice)
        {
            //该sku已报备且商品税类型为且综合税有效商品金额不超过￥2000（≤￥2000）
            if (isReport == 1 && taxType == 1 && validPrice <= Utility.Uitl.ConfigHelper.ConsolidatedPrice)
            {
                return 1;
            }
            return 2;
        }

    }
}
