using System;
using System.Collections;
using System.Text;
using System.Web;

namespace SFO2O.Utility.Uitl
{
    /// <summary>
    ///Web 的摘要说明
    /// </summary>
    public static class ZWeb
    {

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UrlEncode(string s)
        {
            return System.Web.HttpUtility.UrlEncode(s, Encoding.UTF8);
        }

        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UrlDecode(string s)
        {
            return System.Web.HttpUtility.UrlDecode(s, Encoding.UTF8);
        }

        /// <summary>
        /// html方式编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HtmlEncode(string s)
        {
            if (s == null)
                return null;

            return HttpContext.Current.Server.HtmlEncode(s);
        }

        /// <summary>
        /// html方式解码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HtmlDecode(string s)
        {
            if (s == null)
                return null;

            return HttpContext.Current.Server.HtmlDecode(s);
        }

        /// <summary>
        /// 获取当前的访问路径
        /// </summary>
        /// <returns></returns>
        public static string GetRequestURL(bool with_query)
        {
            if (with_query)
                return HttpContext.Current.Request.Url.PathAndQuery;
            else
                return HttpContext.Current.Request.Url.AbsolutePath;
        }

        /// <summary>
        /// 获取当前的访问路径
        /// </summary>
        /// <returns></returns>
        public static string GetRequestURL()
        {
            return GetRequestURL(true);
        }



        /// <summary>
        /// 获取当前的Referer
        /// </summary>
        /// <returns></returns>
        public static string GetRequestReferer(bool with_query)
        {
            Uri referer = HttpContext.Current.Request.UrlReferrer;

            if (referer == null)
                return null;

            if (with_query)
                return referer.PathAndQuery;
            else
                return referer.AbsolutePath;
        }


        /// <summary>
        /// 获取当前的Referer
        /// </summary>
        /// <returns></returns>
        public static string GetRequestReferer()
        {
            return GetRequestReferer(true);
        }


        /// <summary>
        /// 获取当前的客户端类型
        /// </summary>
        /// <returns></returns>
        public static string GetRequestUserAgent()
        {
            return HttpContext.Current.Request.UserAgent;
        }


        /// <summary>
        /// 获取一个url的本地映射路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetLocalMappedFilePath(string web_path)
        {
            return HttpContext.Current.Server.MapPath(web_path);
        }



        /// <summary>
        /// 获取客户端的用户IP
        /// HTTP_X_FORWARDED_FOR 是HAPROXY定义的客户端IP地址头，这样可以兼容前端Proxy
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            string proxed_ip = (string)(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            if (proxed_ip == null || proxed_ip.Trim() == "")
                return HttpContext.Current.Request.UserHostAddress;

            int last_comma = proxed_ip.LastIndexOf(',');
            if (last_comma == -1)
                return proxed_ip;

            return proxed_ip.Substring(last_comma + 1, (proxed_ip.Length - last_comma - 1)).Trim();
        }

        /// <summary>
        /// 获取当前被访问主机的主机名
        /// </summary>
        /// <returns></returns>
        public static string GetWebServerName()
        {
            return HttpContext.Current.Request.Url.Host.ToString();
        }


        /// <summary>
        /// 获得当前会话的MasterPage
        /// </summary>
        /// <returns></returns>
        public static System.Web.UI.Page GetCurrentMasterPage()
        {
            if (HttpContext.Current.CurrentHandler is System.Web.UI.Page)
            {
                System.Web.UI.Page page = (System.Web.UI.Page)HttpContext.Current.CurrentHandler;
                return page;
            }
            else
                throw new Exception("此方法不能在非页面模式中调用");
        }
        private const int WS_SUCCESS = 1;
        private const int WS_FAIL = 0;
        /// <summary>
        /// 最简单的成功返回
        /// </summary>
        /// <returns></returns>
        public static Hashtable OK()
        {
            return MakeReturnObject(WS_SUCCESS, null, null);
        }

        /// <summary>
        /// 制作一个错误返回
        /// </summary>
        /// <returns></returns>
        public static Hashtable FAIL()
        {
            return FAIL(null);
        }


        /// <summary>
        /// 制作一个错误返回的对象，return_obj为null
        /// </summary>
        /// <param name="err_msg"></param>
        /// <param name="return_obj"></param>
        /// <returns></returns>
        public static Hashtable FAIL(string err_msg)
        {
            return MakeReturnObject(WS_FAIL, null, err_msg);
        }

        /// <summary>
        /// 做一个正常返回，err_msg为空
        /// </summary>
        /// <param name="return_obj"></param>
        /// <returns></returns>
        public static Hashtable OK(params object[] return_obj_list)
        {
            if (return_obj_list == null)
                return MakeReturnObject(WS_SUCCESS, null, null);
            if (return_obj_list.Length == 1)
                return MakeReturnObject(WS_SUCCESS, return_obj_list[0], null);
            else
                return MakeReturnObject(WS_SUCCESS, new ArrayList(return_obj_list), null);

        }

        /// <summary>
        /// 制作一个返回的对象，其实就是把Return_Code和error_msg做成内置的哈希对象罢了
        /// </summary>
        /// <param name="return_code"></param>
        /// <param name="err_msg"></param>
        /// <param name="ht_return"></param>
        /// <returns></returns>
        public static Hashtable MakeReturnObject(int return_code, object return_obj, string err_msg)
        {
            Hashtable ht = new Hashtable();

            ht.Add("WS_RET_CODE", return_code);
            ht.Add("WS_RET_DATA", return_obj);
            ht.Add("WS_RET_MSG", err_msg);

            return ht;
        }

        ///// <summary>
        ///// 获取客户端的Session
        ///// </summary>
        ///// <returns></returns>
        //public static ZSession GetClientZSession()
        //{
        //    return ((ZMasterPage)GetCurrentMasterPage()).zSession;
        //}
    }
}
