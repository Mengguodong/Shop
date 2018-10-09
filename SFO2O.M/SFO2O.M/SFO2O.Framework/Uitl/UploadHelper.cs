using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SFO2O.Framework.Uitl
{
    /// <summary>
    /// 获取上传文件路径方法类
    /// </summary>
    /// <remarks>
    /// 图片格式：
    /// 最终URL地址：
    /// http:///www.biyao.com/files/data0/2013/10/09/18/MODEL/337296f22ae13100.jpg
    /// http:///www.biyao.com/files/data1/2013/10/09/18/MODEL/337296f22ae13101.jpg
    /// http:///www.biyao.com/files/temp/7f/7f6bd813951c4145_mid.jpg
    /// 文件存储地址：
    /// data0: \\192.168.100.223\img_lib\Lib\2013\10\09\18\MODEL\337296f22ae13100.jpg
    /// data1: \\192.168.100.224\img_lib\Lib\2013\10\09\18\MODEL\337296f22ae13101.jpg
    /// temp: \\192.168.100.223\img_lib\temp\7f\7f6bd813951c4145.jpg
    /// IIS虚拟路径：
    /// files\data0 映射到 \\192.168.100.223\img_lib\lib\
    /// files\data0 映射到 \\192.168.100.224\img_lib\lib\
    /// files\temp 映射到 \\最新文件服务器\img_lib\lib\
    /// 数据库保存地址：
    /// /data0/2013/10/09/18/MODEL/337296f22ae13100.jpg
    /// /data1/2013/10/09/18/MODEL/337296f22ae13101.jpg
    /// /temp/7f/7f6bd813951c4145.jpg
    /// 非图片数据库保存地址：
    /// \data0\2013\10\09\18\MODEL\337296f22ae13100.obj
    /// \data1\2013\10\09\18\MODEL\337296f22ae13101.obj
    /// \temp\7f\7f6bd813951c4145.obj
    /// 提示：
    /// 非图片地址转换成绝对路径通过IIS虚拟路径来转，如
    /// \data0\2013\10\09\18\MODEL\337296f22ae13100.obj地址，首先通过iis找到虚拟路径data0对应的绝对路径\\192.168.100.223\img_lib\lib\，然后转换成真正路径：
    /// \\192.168.100.223\img_lib\lib\2013\10\09\18\MODEL\337296f22ae13100.obj
    /// </remarks>
    /// <updateby>wangyc</updateby>
    /// <updatetime>2013/10/10</updatetime>
    /// <updatecontent></updatecontent>
    public class UploadHelper
    {
        private const string TempRootDirectory = "temp";

        #region Temp

        /// <summary>
        /// 获取文件服务器下一个可用的临时目录虚拟路径（不带服务器IP地址）,从\temp开始；
        /// <example>\temp\5c\5cbtdprs6eu36bd1\</example>
        /// </summary>
        /// <returns>返回从/temp开始的临时目录虚拟路径</returns>
        /// <example>\Temp\5c\5cbtdprs6eu36bd1\</example>
        public static string GetSavePathTemp()
        {
            return
                Combine(TempRootDirectory, StringHelper.Get00_FFstring(2), StringHelper.GetRandomString(8))
                    .RemoveStartSlash()
                    .InsertFileSlash();
            StringHelper.GetRandomString(8);
            //string guidString = StringHelper.GuidString();
            //return
            //    UploadHelper.Combine(TempRootDirectory, guidString.Substring(0, 2), guidString)
            //        .RemoveStartSlash()
            //        .InsertFileSlash();
        }

        /// <summary>
        /// 获取文件服务器下一个可用的临时目录绝对地址
        /// </summary>
        /// <returns>返回文件服务器下一个可用的临时目录绝对地址</returns>
        /// <example>\\192.168.96.166\Share\Temp\5c\5cbtdprs6eu36bd1\</example>
        public static string GetTempDirectoryPath()
        {
            string guidString = StringHelper.GuidString();
            return GetTempDirectoryPath(guidString);
        }

        /// <summary>
        /// 为指定目录名 在文件服务器临时目录下分配一个路径，返回该路径的绝对地址
        /// </summary>
        /// <param name="guidDirectoryName">16*16的指定目录名, <example>5cbtdprs6eu36bd1</example></param>
        /// <returns>返回为指定目录名分配的绝对路径地址</returns>
        /// <example>
        /// \\192.168.96.166\Share\Temp\5c\5cbtdprs6eu36bd1\
        /// </example>
        public static string GetTempDirectoryPath(string guidDirectoryName)
        {
            return GetTempDirectoryPath(guidDirectoryName, false);
        }

        /// <summary>
        /// 为指定目录名 在文件服务器临时目录下分配一个路径，返回该路径的绝对地址，如:
        /// <example>
        /// \\192.168.96.166\Share\Temp\5c\5cbtdprs6eu36bd1\
        /// </example>
        /// </summary>
        /// <param name="guidDirectoryName">16*16的指定目录名</param>
        /// <param name="notExistsCreate">如果分配的目录不存在，则自动创建文件夹</param>
        /// <returns>返回为指定目录名分配的绝对路径地址</returns>
        /// <example>
        /// \\192.168.96.166\Share\Temp\5c\5cbtdprs6eu36bd1\
        /// ConfigHelper.SharePath:\\192.168.96.166\Share\
        /// TempRootDirectory:temp
        /// </example>
        public static string GetTempDirectoryPath(string guidDirectoryName, bool notExistsCreate)
        {
            if (string.IsNullOrEmpty(guidDirectoryName) || guidDirectoryName.Length <= 2)
            {
                return string.Empty;
            }
            guidDirectoryName = guidDirectoryName.ToLower();
            string directoryPath = UploadHelper.Combine(ConfigHelper.SharePath, TempRootDirectory,
                                                guidDirectoryName.Substring(0, 2), guidDirectoryName);
            if (notExistsCreate && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return directoryPath;
        }

        /// <summary>
        /// 为指定文件名 在文件服务器临时目录下分配一个文件路径，返回该路径的绝对地址
        /// <example>\\192.168.96.166\Share\Temp\1d\1d0125895b1e1465.jpg</example>
        /// </summary>
        /// <param name="guidFileName">16*16的指定文件名<example>1d0125895b1e1465.jpg</example></param>
        /// <returns>返回为指定文件在临时目录下分配的路径地址</returns>
        public static string GetTempFilePath(string guidFileName)
        {
            return GetTempFilePath(guidFileName, true);
        }

        /// <summary>
        /// 为指定文件名 在文件服务器临时目录下分配一个可用的文件路径
        /// <example>\\192.168.96.166\Share\Temp\1d\1d0125895b1e1465.jpg</example>
        /// </summary>
        /// <param name="guidFileName">16*16的指定文件名,如<example>1d0125895b1e1465.jpg</example></param>
        /// <param name="autoCreateDirectory">如果分配的文件路径不存在，创建上级目录</param>
        /// <returns>返回为指定文件在临时目录下分配的绝对路径地址，如
        /// <example>\\192.168.96.166\Share\Temp\1d\1d0125895b1e1465.jpg</example>
        /// </returns>
        public static string GetTempFilePath(string guidFileName, bool autoCreateDirectory)
        {
            if (string.IsNullOrEmpty(guidFileName) || guidFileName.Length <= 2)
            {
                return string.Empty;
            }
            guidFileName = guidFileName.ToLower();
            string directoryPath = UploadHelper.Combine(ConfigHelper.SharePath, TempRootDirectory, guidFileName.Substring(0, 2));
            if (autoCreateDirectory && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return UploadHelper.Combine(directoryPath, guidFileName);
        }

        #endregion

        #region Lib

        /// <summary>
        /// 为指定文件在文件服务器永久目录下分配一个路径，返回该路径的绝对地址
        /// <example>\\192.168.96.166\Share\lib\2013\10\09\18\BBS\4482cf3084b60be9.jpg</example>
        /// </summary>
        /// <param name="guidFileName">16*16位的文件名，如4482cf3084b60be9.jpg</param>
        /// <param name="bussinessType">业务类型，如BBS</param>
        /// <param name="autoCreateDirectory">如果该文件分配的上级目录不存在，是否自动创建之</param>
        /// <returns>返回永久文件服务器目录下的绝对文件路径</returns>
        public static string GetLibFilePath(string guidFileName, string bussinessType, bool autoCreateDirectory)
        {
            if (string.IsNullOrEmpty(guidFileName))
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(bussinessType))
            {
                bussinessType = "other";
            }
            DateTime currentDate = DateTime.Now;
            string directoryPath = UploadHelper.Combine(ConfigHelper.SharePath, "Lib",
                                                currentDate.Year.ToString(CultureInfo.InvariantCulture),
                                                currentDate.Month.ToString("#00"), currentDate.Day.ToString("#00"),
                                                currentDate.Hour.ToString("#00"), bussinessType);
            if (autoCreateDirectory && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return UploadHelper.Combine(directoryPath, guidFileName);
        }



        /// <summary>
        /// 为指定目录名在文件服务器永久目录下分配一个路径
        /// <example>\\192.168.96.166\Share\lib\2013\10\09\18\BBS\4482cf3084b60be9\</example>
        /// </summary>
        /// <param name="guidDirectoryName">16*16的目录名，如4482cf3084b60be9</param>
        /// <param name="bussinessType">业务类型，如BBS</param>
        /// <param name="notExistsCreate">如果目录不存在，是否自动创建</param>
        /// <returns>返回为指定目录名在文件服务器永久目录下分配的的文件夹路径</returns>
        public static string GetLibDirectoryPath(string guidDirectoryName, string bussinessType, bool notExistsCreate)
        {
            if (string.IsNullOrEmpty(guidDirectoryName))
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(bussinessType))
            {
                bussinessType = "other";
            }
            DateTime currentDate = DateTime.Now;
            string directoryPath = UploadHelper.Combine(ConfigHelper.SharePath, "Lib",
                                                currentDate.Year.ToString(CultureInfo.InvariantCulture),
                                                currentDate.Month.ToString("#00"), currentDate.Day.ToString("#00"),
                                                currentDate.Hour.ToString("#00"), bussinessType, guidDirectoryName);
            if (notExistsCreate && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return directoryPath;
        }

        /// <summary>
        /// 获取背景颜色图片所有路径，渲染专用，请勿改动
        /// </summary>
        /// <param name="colorCode"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string GetBackgroundColorImage(string colorCode, int width, int height)
        {
            string directoryPath = UploadHelper.Combine(ConfigHelper.SharePath, "Lib", "Background", colorCode);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string filePath = Combine(directoryPath, width + "_" + height + ".png");
            if (!File.Exists(filePath)) //如果这个背景文件还没有，则生成之
            {
                Bitmap b = ZImage.DrawBackgroundImg(1, 1, ZImage.Hex2Color(colorCode), 0.7);
                b.Save(filePath, ImageFormat.Png);
            }
            return filePath;
        }

        #endregion

        #region Copy
        public static string CopyDirectoryToLib(string sourceDirectoryPath, string businessType = "design")
        {
            if (!Directory.Exists(sourceDirectoryPath))
            {
                return string.Empty;
            }
            var libPath = GetLibDirectoryPath(StringHelper.GuidString(), businessType, true);
            CopyDirectory(sourceDirectoryPath, libPath);
            return GetImageLocalVirtualPath(libPath);
        }


        public static void CopyDirectory(string sourceDirectoryPath, string destDirectoryPath)
        {
            if (!Directory.Exists(sourceDirectoryPath))
            {
                return;
            }
            if (!Directory.Exists(destDirectoryPath))
            {
                Directory.CreateDirectory(destDirectoryPath);
            }
            var sourceDirectory = new DirectoryInfo(sourceDirectoryPath);
            var fileInfos = sourceDirectory.GetFiles();
            if (fileInfos.Length > 0)
            {
                foreach (FileInfo fileInfo in fileInfos)
                {
                    fileInfo.CopyTo(Combine(destDirectoryPath, fileInfo.Name), true);
                }
            }
            var directoryInfos = sourceDirectory.GetDirectories();
            if (directoryInfos.Length > 0)
            {
                foreach (DirectoryInfo directoryInfo in directoryInfos)
                {
                    var newDirectory = Combine(destDirectoryPath, directoryInfo.Name);

                    CopyDirectory(directoryInfo.FullName, newDirectory);
                }
            }
        }
        public static string MoveDirectoryToLib(string tempDirectoryPath, string businessName = "design")
        {
            if (!Directory.Exists(tempDirectoryPath))
            {
                return string.Empty;
            }
            var libDirectoryPath = GetLibDirectoryPath(StringHelper.GuidString(), businessName, false);
            Directory.Move(tempDirectoryPath, libDirectoryPath);
            return GetImageLocalVirtualPath(libDirectoryPath);
        }
        public static string CopyFileToLib(string tempFilePath, bool renameFile = true, string bussinessType = "MODEL")
        {
            string fileName = Path.GetFileName(tempFilePath);
            if (renameFile)
            {
                fileName = StringHelper.GuidString() + Path.GetExtension(tempFilePath);
            }
            string libFilePath = GetLibFilePath(fileName, bussinessType, true);
            try
            {
                File.Copy(tempFilePath, libFilePath, true);
            }
            catch
            {
            }
            return libFilePath;
        }

        /// <summary>
        ///  根据文件路径上传到正确的目录下面，主要用在logo图上传
        /// </summary>
        /// <param name="imagepath"></param>
        /// <param name="tempOrLib"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public static string UploadFilesToLib(string imagepath, string tempOrLib, string businessType)
        {
            string filePath = "";
            try
            {
                string fileName = Path.GetFileName(imagepath);
                //后缀
                var fileExtension = fileName.Substring(fileName.LastIndexOf('.'),
                                                       fileName.Length - fileName.LastIndexOf('.'));
                filePath = tempOrLib.ToLower() == "lib"
                                     ? GetLibFilePath(StringHelper.GuidString() + fileExtension,
                                                                   businessType,
                                                                   true)
                                     : GetTempFilePath(StringHelper.GuidString() + fileExtension, true);


                var stopwatch = new Stopwatch();
                stopwatch.Start();

                Image img = Image.FromFile(imagepath);
                img.Save(filePath);

                stopwatch.Stop();

            }
            catch (Exception ex)
            {

            }
            return GetImageWebPath(filePath);
        }

        #endregion

        #region Web Url SharePath

        /// <summary>
        /// 把一个路径转换成WEB绝对路径(带服务器地址)
        /// <example>http:///www.biyao.com/files/data0/2013/10/09/18/MODEL/337296f22ae13100.jpg</example>
        /// </summary>
        /// <param name="fullpath">/data0/2013/10/09/18/MODEL/337296f22ae13100.jpg</param>
        /// <returns></returns>
        public static string GetImageWebPath(string fullpath)
        {

            if (string.IsNullOrWhiteSpace(fullpath))
            {
                return string.Empty;
            }
            try
            {
                return fullpath.RemoveImageServer()
                               .RemoveSharePath()
                               .ReplaceFileSlashToWebSlash()
                    //.ReplaceTempToVirtureDirectory()
                               .ReplaceLibToVirtualDirectory()
                               .RemoveStartSlash()
                               .InsertImageServer();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 把一个路径转换成WEB虚拟路径(不带服务器地址)
        /// <example>/data0/2013/10/09/18/MODEL/337296f22ae13100.jpg</example>
        /// </summary>
        /// <param name="imgpath">\data0\2013\10\09\18\MODEL\337296f22ae13100.jpg</param>
        /// <returns></returns>
        public static string GetImageWebVirtualPath(string imgpath)
        {
            if (string.IsNullOrEmpty(imgpath))
            {
                return string.Empty;
            }
            return imgpath.RemoveImageServer()
                          .RemoveSharePath()
                          .ReplaceLibToVirtualDirectory()
                //.ReplaceTempToVirtureDirectory()
                          .ReplaceFileSlashToWebSlash()
                          .RemoveStartSlash()
                          .InsertWebSlash();
        }

        /// <summary>
        /// 把一个路径转换成存储路径(带服务器)
        /// <example>\\192.168.96.166\Share\lib\2013\10\09\18\MODEL\337296f22ae13100.jpg</example>
        /// </summary>
        /// <param name="fullpath">web全路径</param>
        /// <returns></returns>
        public static string GetImageLocalPath(string imgpath)
        {
            if (string.IsNullOrEmpty(imgpath))
            {
                return string.Empty;
            }
            try
            {
                string temp = imgpath.RemoveImageServer().RemoveSharePath().RemoveStartSlash().ReplaceWebSlashToFileSlash();
                if (temp.StartsWith("data"))
                {
                    int slashIndex = temp.IndexOf(@"\");
                    temp = "lib" + temp.Substring(slashIndex);
                }
                return temp.InsertSharePath();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 把一个路径转换成存储路径(不带服务器)
        /// <example>\lib\2013\10\09\18\MODEL\337296f22ae13100.jpg</example>
        /// </summary>
        /// <param name="imgpath"></param>
        /// <returns></returns>
        public static string GetImageLocalVirtualPath(string imgpath)
        {
            if (string.IsNullOrEmpty(imgpath))
            {
                return string.Empty;
            }
            string temp = imgpath.RemoveImageServer().RemoveSharePath().RemoveStartSlash().ReplaceWebSlashToFileSlash();
            if (temp.StartsWith("data"))
            {
                int slashIndex = temp.IndexOf(@"\");
                temp = "lib" + temp.Substring(slashIndex);
            }
            return temp.InsertFileSlash();
        }

        #endregion

        #region Combine

        public static string Combine(params string[] paths)
        {
            if (paths == null || paths.Length == 0)
            {
                return string.Empty;
            }
            for (var i = 1; i < paths.Length; i++)
            {
                paths[i] = paths[i].RemoveStartSlash();
            }
            return Path.Combine(paths);
        }

        public static string Combine(string path1, string path2)
        {
            return Combine(new[] { path1, path2 });
        }

        public static string Combine(string path1, string path2, string path3)
        {
            return Combine(new[] { path1, path2, path3 });
        }

        public static string Combine(string path1, string path2, string path3, string path4)
        {
            return Combine(new[] { path1, path2, path3, path4 });
        }

        #endregion

        public static string DeleteDirectoryFromLib(string sourceDirectoryPath)
        {
            if (!Directory.Exists(sourceDirectoryPath))
            {
                return string.Empty;
            }
            Directory.Delete(sourceDirectoryPath, true);
            return "clear";
        }

        public static string GetPreRenderDirectoryPath(int mainModelId)
        {
            string preRenderDirectory = UploadHelper.Combine(ConfigHelper.SharePath, "Lib", "prerender",
                                                             mainModelId.ToString());
            if (!Directory.Exists(preRenderDirectory))
            {
                Directory.CreateDirectory(preRenderDirectory);
            }
            return preRenderDirectory;
        }
    }

    public static class StringExtension
    {
        public static string ReplaceWebSlashToFileSlash(this string url)
        {
            return url.Replace("/", @"\");
        }

        public static string ReplaceFileSlashToWebSlash(this string url)
        {
            return url.Replace(@"\", "/");
        }

        public static string RemoveImageServer(this string url)
        {
            return url.ToLower().Replace(ConfigHelper.ImageServer.ToLower(), string.Empty);
        }

        public static string RemoveSharePath(this string url)
        {
            return url.ToLower().Replace(ConfigHelper.SharePath.ToLower(), string.Empty);
        }

        public static string ReplaceTempToVirtureDirectory(this string url)
        {
            return url.ToLower().Replace("temp", ConfigHelper.CurrentVirtualDirectory.ToLower());
        }

        public static string ReplaceLibToVirtualDirectory(this string url)
        {
            return url.ToLower().Replace("lib", ConfigHelper.CurrentVirtualDirectory.ToLower());
        }

        public static string RemoveStartSlash(this string url)
        {
            return url.TrimStart('/').TrimStart('\\');
        }

        public static string InsertImageServer(this string url)
        {
            return ConfigHelper.ImageServer + url;
        }

        public static string InsertSharePath(this string url)
        {
            return UploadHelper.Combine(ConfigHelper.SharePath, url);
        }

        public static string InsertFileSlash(this string url)
        {
            return "\\" + url;
        }

        public static string InsertWebSlash(this string url)
        {
            return "/" + url;
        }
    }
}
