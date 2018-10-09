using System;
using System.IO;
using System.Text;
using System.Web;
using SFO2O.Framework.Uitl;

namespace SFO2O.Framework.Common.Extensions
{
    /// <summary>
    /// 文件类型
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// 图片
        /// </summary>
        Image = 0,
        /// <summary>
        /// 文件
        /// </summary>
        File = 1,
    }

    /// <summary>
    /// 默认存储文件服务器
    /// </summary>
    public static class SiteExtensions
    {
         
        /// <summary>
        /// 是否有上传文件
        /// </summary>
        /// <param name="file">HttpPostedFileBase实例</param>
        /// <returns></returns>
        public static bool HasFile(this HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0);
        }



        public static string UploadServer = ConfigHelper.GetAppSetting<string>("ImgServer");


        public static string AllowedImageExt = ConfigHelper.GetAppSetting<string>("AllowedImageExt", ".jpg,.jpeg,.bmp,.gif,.png");

        public static string AllowedFileExt = ConfigHelper.GetAppSetting<string>("AllowedFileExt", ".jpg,.jpeg,.bmp,.gif,.png");

        /// <summary>
        /// 允许上传文件的最大容量
        /// </summary>
        public static int DefaultFileMaxSize = Convert.ToInt32(ConfigHelper.GetAppSetting("DefaultFileMaxSize", 10)) * 1048576;//(1M)

        //public static bool UploadFile(HttpPostedFileBase hpf, string uploadpath, out string orginalfilename, out string filesize, out string newfilename)
        //{
        //    return UploadFile(hpf, uploadpath, "file", DefaultFileMaxSize, out orginalfilename, out filesize, out newfilename);
        //}



        private static bool CreateDirectory(string pDirectory)
        {
            try
            {
                if (!Directory.Exists(pDirectory))
                {
                    Directory.CreateDirectory(pDirectory);
                }

                return true;
            }
            catch (IOException ex)
            {
                LogHelper.Error(ex);
                return false;
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                return false;
            }
        }

        /// <summary>
        /// 检测文件扩展名
        /// </summary>
        /// <param name="filetype">文件类型</param>
        /// <param name="ext">扩展名</param>
        /// <returns></returns>
        private static bool CheckValidFile(FileType filetype, string ext)
        {
            if (ext.Contains("."))
            {
                ext = ext.Trim('.');
            }
            switch (filetype)
            {
                case FileType.Image:
                    return (AllowedImageExt.IndexOf(ext.ToLower(), StringComparison.OrdinalIgnoreCase) != -1);
                case FileType.File:
                    return (AllowedFileExt.IndexOf(ext.ToLower(), StringComparison.OrdinalIgnoreCase) != -1);
            }
            return false;
        }

        /// <summary>
        /// 检测文件大小
        /// </summary>
        /// <param name="maxfilesize">最大允许的文件大小</param>
        /// <param name="filesize">实际文件大小</param>
        /// <returns></returns>
        private static bool CheckFileSize(int maxfilesize, int filesize)
        {
            return (filesize <= maxfilesize);
        }

        private static string GeneralFileName()
        {
            return DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.Millisecond.ToString() + StringHelper.Right(StringHelper.GuidString(), 4);
        }

        public static string UploadFile(out string[] orginalfilename, out string[] filesize, out string[] newfilename, HttpFileCollectionBase hpfs)
        {
            return Upload(FileType.File, DefaultFileMaxSize, out orginalfilename, out filesize, out newfilename, hpfs);
        }
        public static string UploadImage(out string[] orginalfilename, out string[] filesize, out string[] newfilename, HttpFileCollectionBase hpfs)
        {
            return Upload(FileType.Image, DefaultFileMaxSize, out orginalfilename, out filesize, out newfilename, hpfs);
        }
        public static string Upload(FileType fileType, int maxFileSize, out string[] orginalFileName, out string[] fileSize, out string[] newFileName, HttpFileCollectionBase hpfs)
        {
            var i = 0;
            StringBuilder builder = new StringBuilder();
            if (hpfs.Count > 0)
            {
                var rank = hpfs.Count;
                orginalFileName = new string[rank];
                fileSize = new string[rank];
                newFileName = new string[rank];

                for (var j = 0; j < rank; j++)
                {
                    orginalFileName[j] = fileSize[j] = newFileName[j] = string.Empty;
                }

                bool flag = true;
                string uploadpath = GetUploadPath(fileType);


                for (var l = 0; l < hpfs.Count; l++)
                {
                    HttpPostedFileBase hpf = hpfs[l];

                    flag = ExecUploadFile(hpf, uploadpath, fileType, maxFileSize, out orginalFileName[i], out fileSize[i], out newFileName[i]);

                    i++;
                    if (!flag && hpf.HasFile())
                    {
                        builder.AppendFormat(@"第{0}个文件上传出错:
                                            文件类型-{1};
                                            文件大小-{2}k;
                                            允许上传的文件类型:{3};
                                            允许上传的文件大小:{4}M",
                                                           (i),
                                                           Path.GetExtension(hpf.FileName),
                                                           hpf.ContentLength / 1024,
                                                           GetAllowExt(fileType),
                                                           DefaultFileMaxSize / (1024 * 1024)
                            );

                        continue;
                    }
                }
            }
            else
            {
                orginalFileName = null;
                fileSize = null;
                newFileName = null;
                builder.Append("没有上传文件！");
            }

            return builder.ToString();
        }



        private static bool ExecUploadFile(HttpPostedFileBase hpf, string uploadPath, FileType fileType, int maxSize, out string orginalName, out string fileSize, out string newFileName)
        {
            orginalName = string.Empty;
            fileSize = string.Empty;
            newFileName = string.Empty;

            try
            {
                if (!hpf.HasFile())
                {
                    return true;
                } 

                string fileName = Path.GetFileName(hpf.FileName);
                string fileExt = Path.GetExtension(hpf.FileName);
                int filesize = hpf.ContentLength;

                //检测文件类型
                if (!CheckValidFile(fileType, fileExt))
                {
                    return false;
                }

                //检测文件大小
                if (!CheckFileSize(maxSize, filesize))
                {
                    return false;
                }

                //CreateDirectory(path);//创建文件夹
                orginalName = fileName;//原始名字
                fileSize = filesize.ToString();
                fileName = GeneralFileName() + fileExt;//新名字
                newFileName = Path.Combine(uploadPath, fileName); 
                hpf.SaveAs(newFileName);

                return true;
            }
            catch (IOException e)
            {
                LogHelper.Error(e);
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }


        private static string GetAllowExt(FileType filetype)
        {
            switch (filetype)
            {
                case FileType.File:
                    return AllowedFileExt;
                case FileType.Image:
                    return AllowedImageExt;
                default:
                    return AllowedImageExt;
            }
        }

        private static string GetUploadPath(FileType filetype)
        {
            switch (filetype)
            {
                case FileType.File:
                    return UploadHelper.GetLibDirectoryPath(StringHelper.GuidString(), "FILE", true);
                case FileType.Image:
                    return UploadHelper.GetLibDirectoryPath(StringHelper.GuidString(), "IMG", true);
                default:
                    return UploadHelper.GetLibDirectoryPath(StringHelper.GuidString(), "OTHER", true);
            }
        }
    }
}
