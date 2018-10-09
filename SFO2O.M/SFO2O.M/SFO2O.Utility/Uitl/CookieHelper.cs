using System;
using System.Collections.Generic;
using System.Web;

namespace SFO2O.Utility.Uitl
{
    public class CookieHelper
    {
        #region 读写cookie
        /// <summary>
        /// 写cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetCookie(string key, string value)
        {
            HttpContext.Current.Response.SetCookie(new HttpCookie(key, value));
        }

        public static void SetCookie(string key, string value = "", Dictionary<string, string> dic = null)
        {
            HttpCookie cookie = new HttpCookie(key);
            if (!string.IsNullOrEmpty(value)) cookie.Value = HttpContext.Current.Server.UrlEncode(value);

            if (dic != null)
            {
                System.Collections.Specialized.NameValueCollection cookieColl = new System.Collections.Specialized.NameValueCollection();
                foreach (string item in dic.Keys)
                {
                    cookieColl.Add(item, HttpContext.Current.Server.UrlEncode(dic[item]));
                }
                cookie.Values.Add(cookieColl);
            }
            HttpContext.Current.Response.SetCookie(cookie);
        }

        /// <summary>
        /// 读cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCookie(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                return HttpContext.Current.Server.UrlDecode(cookie.Value);
            }
            return string.Empty;
        }

        public static string GetCookie(string key, string subKey = "")
        {
            string result = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                result = HttpContext.Current.Server.UrlDecode(cookie.Value);
                if (subKey != "")
                {
                    System.Collections.Specialized.NameValueCollection cookieColl = cookie.Values;
                    if (cookieColl != null)
                    {
                        result = HttpContext.Current.Server.UrlDecode(cookieColl[subKey]);
                    }
                }
            }
            if (!string.IsNullOrEmpty(result)) result = HttpContext.Current.Server.UrlDecode(result);
            return result;
        }

        /// <summary>
        /// 清除cookie
        /// </summary>
        /// <param name="key"></param>
        public static void ClearCookie(string key)
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[key];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddMinutes(-1);
            }
        }
        #endregion
    }
}
