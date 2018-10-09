using System;

namespace SFO2O.Utility.Uitl
{
    public static class PathHelper
    {

        // 国旗图片扩展名
        public static readonly string NationalFlagImageExtension = ".jpg";

        /// <summary>
        /// 转化为180的小图
        /// </summary>
        /// <param name="orignalPath"></param>
        /// <param name="width"></param>
        /// <param name="defaultPath">默认路径</param>
        /// <returns></returns>
        public static string GetImageSmallUrl(this string orignalPath, int width = 180, string defaultPath = "")
        {
            if (string.IsNullOrEmpty(orignalPath))
            {
                return defaultPath;
            }
            var path = GetImageUrl(orignalPath).Replace("\\", "/");

            var list = path.Split('.');
            if (list.Length > 2)
            {
                list[list.Length - 2] += "_" + width.ToString();
            }
            return string.Join(".", list);
        }
        /// <summary>
        /// 取得图片的Url
        /// </summary>
        /// <returns></returns>
        public static string GetImageUrl(this string imagePath, string defaultPath = "")
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return defaultPath;
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
        /// 根据正式的网络路径返回相对物理路径
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static string ReturnRelativelyImageUrl(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return "";
            }
            else
            {
                return imagePath.Replace(ConfigHelper.ImageServer, "").Replace("/", "\\").TrimStart('\\');
            }
        }

        /// <summary>
        /// 获得国旗图片的url
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="defaultPath"></param>
        /// <returns></returns>
        public static string GetNationalFlagImageUrl(this string imagePath, string defaultPath = "")
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return defaultPath;
            }

            if (imagePath.IndexOf(ConfigHelper.NationalFlagImageServer) == -1)
            {
                return ConfigHelper.NationalFlagImageServer.TrimEnd(new char[] { '/', '\\' }) + "/" + imagePath.TrimStart(new char[] { '/', '\\' }).Replace("\\", "/");
            }
            else
            {
                return imagePath;
            }
        }

    }
}
