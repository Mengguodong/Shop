using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Common
{
    public static class PathHelper
    {
        /// <summary>
        /// 转化为100的小图
        /// </summary>
        /// <param name="orignalPath"></param>
        /// <returns></returns>
        public static string ConvertToProductSmallPath(string orignalPath, int width = 100)
        {
            var path = PathHelper.GetImageUrl(orignalPath).Replace("\\", "/");

            var list = path.Split('.');
            if (list.Length > 2)
            {
                list[list.Length - 2] += "_" + width.ToString();
            }
            return string.Join(".", list);
        }

        public static string CheckAndGetImageUrl(string imagePath)
        {
            if (imagePath.IndexOf(ConfigHelper.ImageServer) == -1)
            {
                return ConfigHelper.ImageServer.TrimEnd(new char[] { '/', '\\' }) + "/" + imagePath.TrimStart(new char[] { '/', '\\' }).Replace("\\", "/");
            }

            return imagePath;
        }

        /// <summary>
        /// 取得图片的Url
        /// </summary>
        /// <returns></returns>
        public static string GetImageUrl(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return "";
            }

            if (imagePath.IndexOf(ConfigHelper.ImageServer) == -1)
            {
                return ConfigHelper.ImageServer.TrimEnd(new char[] { '/', '\\' }) + "/" + imagePath.TrimStart(new char[] { '/', '\\' }).Replace("\\", "/");
            }
            else
            {
                return imagePath;
            }
        }

        /// <summary>
        /// 得到图片的相对路径
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public static string GetImageRelativeUrl(string imageUrl)
        {
            if (imageUrl != null)
            {
                return imageUrl.Replace(ConfigHelper.ImageServer, "/");
            }
            return null;
        }

        /// <summary>
        /// 得到图片的物理路径
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public static string GetImageLocalUrl(string imageUrl)
        {
            if (imageUrl != null)
            {
                return ConfigHelper.SharePath + imageUrl.Replace(ConfigHelper.ImageServer, "/").Replace("/", "\\").TrimStart('\\');
            }
            return null;
        }
        /// <summary>
        /// 临时文件存放路径
        /// </summary>
        /// <returns></returns>
        public static string GetSavePathTemp()
        {
            return string.Format(@"TEMP\{0}\{1}\", StringHelper.Get00_FFstring(2), StringHelper.GetRandomString(12));
        }



        /// <summary>
        /// 系统生成的流行趋势图片存放路径
        /// </summary>
        /// <returns></returns>
        public static string GetSavePathFashion()
        {
            return string.Format(@"LIB\{0}\{1}\{2}\{3}\Fashion\",
                          DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"), DateTime.Now.ToString("HH"));
        }

        /// <summary>
        /// 材料、配件、设计图片存放路径
        /// </summary>
        /// <returns></returns>
        public static string GetSavePathImg()
        {
            return string.Format(@"LIB\{0}\{1}\{2}\{3}\IMG\{4}\",
                         DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"), DateTime.Now.ToString("HH"),
                         StringHelper.GetRandomString(12));
        }

        /// <summary>
        /// 模型相关文件存放路径
        /// </summary>
        /// <returns></returns>
        public static string GetSavePathModel()
        {
            return string.Format(@"LIB\{0}\{1}\{2}\{3}\MODEL\{4}\",
                         DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"), DateTime.Now.ToString("HH"),
                         StringHelper.GetRandomString(12));
        }
        /// <summary>
        /// 商家后台上传图片路径，除了材料、配件、设计外的图片
        /// </summary>
        /// <returns></returns>
        public static string GetSaveSjPathImg()
        {
            return string.Format(@"LIB\{0}\{1}\{2}\{3}\IMG\SJ\{4}\",
                         DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"), DateTime.Now.ToString("HH"),
                         StringHelper.GetRandomString(12));
        }
        /// <summary>
        /// 管理后台焦点图片
        /// </summary>
        /// <returns></returns>
        public static string GetSaveHouTaiFocusPathImg()
        {
            return string.Format(@"LIB\{0}\{1}\{2}\{3}\IMG\HouTai\Focus\{4}\",
                         DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"), DateTime.Now.ToString("HH"),
                         StringHelper.GetRandomString(12));
        }
        /// <summary>
        /// 管理后台焦点图片
        /// </summary>
        /// <returns></returns>
        public static string GetSaveHouTaiMobileAdsPathImg()
        {
            return string.Format(@"LIB\{0}\{1}\{2}\{3}\IMG\HouTai\MobileAds\{4}\",
                         DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"), DateTime.Now.ToString("HH"),
                         StringHelper.GetRandomString(12));
        }
        public static string GetSaveHouTaiMobileAppPath()
        {
            return string.Format(@"LIB\{0}\{1}\{2}\{3}\MobileApp\{4}\",
                         DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"), DateTime.Now.ToString("HH"),
                         StringHelper.GetRandomString(12));
        }
        /// <summary>
        /// App分类上传图片
        /// </summary>
        /// <returns></returns>
        public static string GetSaveMobileAppCategoryUploadImgPath()
        {
            return string.Format(@"LIB\{0}\{1}\{2}\{3}\IMG\MobileApp\Category\{4}\",
                         DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"), DateTime.Now.ToString("HH"),
                         StringHelper.GetRandomString(12));
        }
        /// <summary>
        /// 生成随机地址
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string ReturnPathString(string ext)
        {
            return ConfigHelper.ImageServer + GetSaveSjPathImg() + StringHelper.GetRandomString(12) + ext;
        }
        /// <summary>
        /// 根据正式的网络路径返回相对物理路径
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static string ReturnRelativelyImageUrl(string imagePath)
        {

            return imagePath.Replace(ConfigHelper.ImageServer, "").Replace("/", "\\").TrimStart('\\');

        }

    }
}
