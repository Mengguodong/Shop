using SFO2O.Supplier.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace SFO2O.Supplier.Web
{
    public static class Extensions
    {
        static String Version = "?v=" + ConfigHelper.Version;
        /// <summary>
        /// 渲染脚本文件 path 的带版本号的 script 标签
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IHtmlString Script(this HtmlHelper htmlHelper, string path)
        {
            return Scripts.Render(path + Version);
        }

        /// <summary>
        /// 渲染样式表文件 path 的带版本号的 link 标签
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IHtmlString Style(this HtmlHelper htmlHelper, string path)
        {
            return Styles.Render(path + Version);
        }
    }
}