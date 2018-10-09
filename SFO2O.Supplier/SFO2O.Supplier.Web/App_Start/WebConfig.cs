using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SFO2O.Supplier.Web
{
    public class WebConfig
    {
        public static readonly int PageSize = 10;
        /// <summary>
        /// 管理后台站点
        /// </summary>
        public static string WebURL {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["HouTaiWebURL"];
            }
        }
    }
}