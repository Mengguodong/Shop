using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.Controllers.Common
{
    public class Common
    {
        /// <summary>
        /// 返回支付方式
        /// </summary>
        /// <param name="payType"></param>
        /// <returns></returns>
        public static string GetPayTypeString(int payType)
        {
            if (payType == 1)
            {
                return "易票联支付";
            }
            else if (payType == 3)
            {
                return "支付宝支付";
            }
            else if (payType == 4)
            {
                return "微信支付";
            }
            return string.Empty;
        }
        
    }
}
