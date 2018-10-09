using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.BLL.Common;
using SFO2O.Utility.Uitl;

namespace SFO2O.M.Controllers.Common
{
    public class CookieManager
    {
        /// <summary>
        /// 设置温馨提示：每单商品税低于￥50（含）免税
        /// </summary>
        public static void SetShoppingTaxTip()
        {
            CookieHelper.SetCookie(ConstClass.ShoppingTaxTipCookieKey, "1");
        }
        /// <summary>
        /// 是否展示温馨提示：每单商品税低于￥50（含）免税
        /// </summary>
        /// <returns></returns>
        public static bool IsShowShoppingTaxTip()
        {
            var cookie = CookieHelper.GetCookie(ConstClass.ShoppingTaxTipCookieKey);
            if (string.IsNullOrEmpty(cookie))
            {
                SetShoppingTaxTip();
                return true;
            }
            else if (cookie == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 清理 温馨提示：每单商品税低于￥50（含）免税
        /// </summary>
        public static void ClearShoppingTaxTip()
        {
            CookieHelper.SetCookie(ConstClass.ShoppingTaxTipCookieKey, "0");
        }
    }
}
