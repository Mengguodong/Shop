using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.DAL.GiftCard;
using SFO2O.Model.GiftCard;
using SFO2O.M.ViewModel.GiftCard;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Account;

namespace SFO2O.BLL.Holiday
{
    public class HolidayBll
    {
        /// <summary>
        /// 清关规定，不能超过两件，不能超过3Kg，不能超过800元
        /// </summary>
        /// <param name="weight">毛重</param>
        /// <param name="price">单价（最低价）</param>
        /// <param name="maxWeight">清关限重，最大默认3Kg</param>
        /// <param name="maxNum">限量值，最大默认2件</param>
        /// <param name="maxPrice">清关限额，最大默认800元</param>
        /// <returns></returns>
        public decimal GetCanBuyNumber(decimal weight, decimal price, decimal maxWeight = 3M, decimal maxNum = 2M, decimal maxPrice = 800M)
        {
            //清关规定，不能超过两件，不能超过3Kg，不能超过800元
            decimal num = 0M, numByWeight = 0M, numByPrice = 0M;

            //根据毛重判断一次能够买几件
            if (weight > 0M)
            {
                numByWeight = Math.Floor((maxWeight / weight));
                //清关规定，不能超过两件，不能超过3Kg，不能超过800元
                numByWeight = numByWeight >= maxNum ? maxNum : numByWeight;
            }

            //根据价格判断（虽然一般月饼 客单价不会超过这个数（800元）还是加个判断比较靠谱）
            if (price > 0M)
            {
                numByPrice = Math.Floor(maxPrice / price);
                //清关规定，不能超过两件，不能超过3Kg，不能超过800元
                numByPrice = numByPrice >= maxNum ? maxNum : numByPrice;
            }

            //最终价钱算出来的数量和毛重算出来的数量比较，取小整数值
            num = numByWeight <= numByPrice ? numByWeight : numByPrice;            
            return num;
        }

        /// <summary>
        /// 需求变动，暂时只先看重量，最终到提交订单的时候进行判断是否超过规定金额e.g:800元
        /// 只根据重量进行限量判断：1、不能超过两件 2、不能超过3Kg
        /// <param name="weight">毛重</param>
        /// <param name="maxWeight">清关限重，最大默认2Kg</param>
        /// <param name="maxNum">限量值，最大默认2件</param>
        /// </summary>
        public decimal GetCanBuyNumberByWeight(decimal weight, decimal maxWeight = 3M, decimal maxNum = 2M)
        {
            //清关规定，不能超过两件，不能超过3Kg，不能超过800元
            decimal num = 0M, numByWeight = 0M;

            //根据毛重判断一次能够买几件
            if (weight > 0M)
            {
                numByWeight = Math.Floor((maxWeight / weight));
                //清关规定，不能超过两件，不能超过3Kg，不能超过800元
                numByWeight = numByWeight >= maxNum ? maxNum : numByWeight;
            }
            num = numByWeight;
            return num;
        }

        /// <summary>
        /// //清关规定，不能超过两件，不能超过3Kg，不能超过800元
        /// 获取限购商品数量 全条件判断
        /// </summary>
        /// <param name="weight">毛重</param>
        /// <param name="price">单价（最低价）</param>
        /// <param name="maxWeight">清关限重，最大默认2Kg</param>
        /// <param name="maxNum">限量值，最大默认2件</param>
        /// <param name="maxPrice">清关限额，最大默认800元</param>
        /// <returns></returns>
        public decimal GetCanBuyNumberByAll(decimal weight, decimal price, decimal maxWeight = 3M, decimal maxNum = 2M, decimal maxPrice = 800M)
        {
            //清关规定，不能超过两件，不能超过3Kg，不能超过800元
            decimal num = 0M, numByWeight = 0M, numByPrice = 0M;

            //根据毛重判断一次能够买几件
            if (weight > 0M)
            {
                numByWeight = Math.Floor((maxWeight / weight));
                //清关规定，不能超过两件，不能超过3Kg，不能超过800元
                numByWeight = numByWeight >= maxNum ? maxNum : numByWeight;
            }

            //根据价格判断（虽然一般月饼 客单价不会超过这个数（800元）还是加个判断比较靠谱）
            if (price > 0M)
            {
                numByPrice = Math.Floor(maxPrice / price);
                //清关规定，不能超过两件，不能超过3Kg，不能超过800元
                numByPrice = numByPrice >= maxNum ? maxNum : numByPrice;
            }

            //最终价钱算出来的数量和毛重算出来的数量比较，取小整数值
            num = numByWeight <= numByPrice ? numByWeight : numByPrice;
            return num;
        }

    }
}
