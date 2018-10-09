using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Utility.Uitl;

namespace SFO2O.M.Controllers.Common
{
    public static class DomainExtension
    {
        ///// <summary>
        ///// 组装图片服务器
        ///// </summary>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //public static string GetImageServerHref(this string url)
        //{
        //    if (string.IsNullOrEmpty(url))
        //    {
        //        return "";//错误图片处理
        //    }
        //    else
        //    {
        //        return DomainHelper.ImageUrl.TrimEnd('/') + "/" + url.Replace("\\", "/").TrimStart('/');
        //    } 
        //}
        ///// <summary>
        ///// 本站站点下图片资源
        ///// </summary>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //public static string GetLocalSiteHref(this string url)
        //{
        //    if (string.IsNullOrEmpty(url))
        //    {
        //        return ""; //错误图片处理
        //    }
        //    else
        //    {
        //        return DomainHelper.MUrl.TrimEnd('/') + "/" + url.Replace("\\", "/").TrimStart('/');
        //    }
        //}
    }
}
