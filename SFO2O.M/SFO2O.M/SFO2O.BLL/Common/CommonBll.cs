using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.DAL.Common;
using SFO2O.Utility.Cache;

namespace SFO2O.BLL.Common
{
    public class CommonBll
    {
        public static readonly CommonDal DAL = new CommonDal();
        /// <summary>
        /// 获取汇率信息，缓存一天时间
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static decimal GetExchangeRates(int pattern)
        {
            return RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, ConstClass.RedisKey4ExchangeRates, () => DAL.GetExchangeRateByPattern(pattern), ConstClass.RedisCacheMinutesOneDay);
        }

        /// <summary>
        /// 获取汇率信息
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static decimal GetExchangeRatesByPattern(int pattern)
        {
            return DAL.GetExchangeRateByPattern(pattern);
        }

        /// <summary>
        /// 获取用户所在地区
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns>1.大陆 2.中华人民共和国大陆地区</returns>
        public static int GetUserRegion(int userid)
        {
            return DAL.GetUserRegion(userid);
        }
        public string getProductDetailName(string MainName, string SubName, string NetWeightUnit)
        {
            if (!(MainName.Equals("净重") || MainName.Equals("淨重") || SubName.Equals("净含量") || SubName.Equals("淨含量")))
            {
                NetWeightUnit = "";
            }
            return NetWeightUnit;
        }
    }
}
