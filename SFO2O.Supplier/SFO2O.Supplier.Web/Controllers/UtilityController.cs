
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Drawing.Imaging;
using SFO2O.Supplier.Common;
using SFO2O.Supplier.ViewModels;

namespace SFO2O.Supplier.Web.Controllers
{
    public class UtilityController : Controller
    {
        IList<string> allowFileExts = new List<string> { ".jpg", ".jpeg", ".png" }.AsReadOnly();
        /// <summary>
        /// 验证图片扩展名
        /// </summary>
        /// <param name="imageExtension"></param>
        /// <returns></returns>
        bool ValidateImageExtension(string imageExtension)
        {
            foreach (var ext in allowFileExts)
            {
                if (string.Equals(imageExtension, ext, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return base.Json(data, "text/plain", contentEncoding, behavior);
        }

        public ImageFormat GetImageFormat(string imageExtension)
        {
            switch (imageExtension)
            {
                case ".png":
                    return ImageFormat.Png;
                default:
                    return ImageFormat.Jpeg;
            }
        }

        /// <summary>
        /// 上传商品图片
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadImage(bool isTemp = false, int cutWidth = 0)
        {
            //相对路径
            string relativePath = string.Empty;
            string url = string.Empty;
            int width = 0, height = 0;
            HttpPostedFileBase fileData = Request.Files[0];
            if (!fileData.HasFile())
            {
                return Json(new ImageView() { Error = 1, Message = "上載的圖片无效！" });
            }
            if (fileData.ContentLength > (2 * 1024 * 1024))
            {
                return Json(new ImageView() { Error = 1, Message = "上載的圖片尺寸不能超过2M！！" });
            }
            try
            {
                string uploadFileName = fileData.FileName;
                string imageExtension = Path.GetExtension(uploadFileName);

                if (!ValidateImageExtension(imageExtension))
                {
                    return Json(new ImageView() { Error = 1, Message = "不允許上載“" + imageExtension + "”格式的圖片！" });
                }
                else
                {
                    string relativeDir = PathHelper.GetSaveSjPathImg();

                    if (isTemp)//保存到临时路径下
                        relativeDir = PathHelper.GetSavePathTemp();

                    CheckOrCreateDirectory(ConfigHelper.SharePath + relativeDir);

                    var fileName = StringHelper.GetRandomString(12);
                    //保存原图
                    relativePath = relativeDir + fileName + imageExtension;
                    var absolutePath = ConfigHelper.SharePath + relativePath;
                    fileData.SaveAs(absolutePath);

                    //重新压缩图片
                    if (fileData.ContentLength > (1024 * 500))//如果大于500k，那么就需要重新压缩图片了
                    {
                        string relativeDirNew = PathHelper.GetSaveSjPathImg(); ;
                        if (isTemp)//保存到临时路径下
                            relativeDirNew = PathHelper.GetSavePathTemp();

                        CheckOrCreateDirectory(ConfigHelper.SharePath + relativeDirNew);

                        fileName = StringHelper.GetRandomString(12);
                        string relativePathNew = relativeDirNew + fileName + imageExtension;
                        string absolutePathnew = ConfigHelper.SharePath +  relativePathNew;
                        string urlNew = ConfigHelper.ImageServer + relativePathNew.Replace('\\', '/');

                        ReCompressImage(absolutePath, absolutePathnew);

                        relativePath = relativePathNew;
                        absolutePath = absolutePathnew;
                        url = urlNew;
                    }

                    //这个为了剪切功能生成图片大小
                    Bitmap originalImg = new Bitmap(absolutePath);
                    width = originalImg.Width;
                    height = originalImg.Height;
                    if (width < 300 || height < 300)
                    {
                        return Json(new ImageView { Error = 1, Message = "上載的圖片尺寸不能小於300x300" });
                    }
#region 生成缩略图
                    var thumbsDir = Path.GetDirectoryName(absolutePath);

                    EncoderParameters parms = new EncoderParameters(1);
                    var imgCodecInfo = GetImageCodecInfo(GetImageFormat(imageExtension));
                    parms.Param[0] = new EncoderParameter(Encoder.Quality, ((long)80));

                    //切640的图
                    Bitmap b640 = LTImage.ResizeImage(originalImg, 640, 640, true, true, true);
                    string imgPath640 = thumbsDir + "\\" + fileName + "_640" + imageExtension;
                    b640.Save(imgPath640, imgCodecInfo, parms);
                    b640.Dispose();
                    //切180的图
                    Bitmap b180 = LTImage.ResizeImage(originalImg, 180, 180, true, true, true);
                    originalImg.Dispose();
                    string imgPath180 = thumbsDir + "\\" + fileName + "_180" + imageExtension;
                    b180.Save(imgPath180, imgCodecInfo, parms);
                    b180.Dispose();
#endregion
                    url = ConfigHelper.ImageServer + relativePath.Replace('\\', '/');
                }
            }
            catch (Exception ex)
            {
                // LogHelper.GetLogger(typeof(UtilityController)).Error("上传失败:" + ex.ToString());
                return Json(new ImageView { Error = 1, Message = ex.Message });
            }

            return Json(new ImageView { Url = url, Path = relativePath, Width = width, Height = height });
        }
        /// <summary>
        /// 上传图片到临时路径
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadImageTemp()
        {
            return UploadImage(true, 218);
        }



        /// <summary>
        /// Kindeditor上传商品描述图片
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public JsonResult UploadProductDescribeImage()
        {
            HttpPostedFileBase fileData = Request.Files[0];
            string relativePath;
            if (!fileData.HasFile())
            {
                return Json(new { error = 1, message = "上載的圖片无效！" });
            }
            try
            {
                string uploadFileName = fileData.FileName;
                string imageExtension = Path.GetExtension(uploadFileName);

                if (!ValidateImageExtension(imageExtension))
                {
                    return Json(new { error = 1, message = "不允許上載“" + imageExtension + "”格式的圖片！" });
                }
                else
                {
                    string relativeDir = PathHelper.GetSaveSjPathImg();

                    CheckOrCreateDirectory(ConfigHelper.SharePath + relativeDir);

                    relativePath = relativeDir + StringHelper.GetRandomString(12) + imageExtension;

                    fileData.SaveAs(ConfigHelper.SharePath + relativePath);
                }
            }
            catch
            {
                relativePath = string.Empty;
            }

            string saveFileUrl = ConfigHelper.ImageServer + relativePath.Replace('\\', '/');
            var image = new { error = 0, url = saveFileUrl };

            return Json(image);
        }

        /// <summary>
        /// 商品上传及自动剪切
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadImageAndCut()
        {
            string relativePath = string.Empty;
            string url = string.Empty;
            ImageView imageView = new ImageView();
            HttpPostedFileBase fileData = Request.Files[0];
            if (!fileData.HasFile())
            {
                return Json(new ImageView() { Error = 1, Message = "上載的圖片无效！" });
            }
            if (fileData.ContentLength > (2 * 1024 * 1024))
            {
                return Json(new ImageView() { Error = 1, Message = "上載的圖片尺寸不能超过2M！！" });
            }
            try
            {
                string uploadFileName = fileData.FileName;
                string imageExtension = Path.GetExtension(uploadFileName);

                if (!ValidateImageExtension(imageExtension))
                {
                    return Json(new ImageView() { Error = 1, Message = "不允許上載“" + imageExtension + "”格式的圖片！" });
                }
                else
                {
                    string relativeDir = PathHelper.GetSaveSjPathImg();

                    CheckOrCreateDirectory(ConfigHelper.SharePath + relativeDir);

                    //保存原图
                    relativePath = relativeDir + StringHelper.GetRandomString(12) + imageExtension;
                    var absolutePath = ConfigHelper.SharePath + relativePath;
                    fileData.SaveAs(absolutePath);

                    url = ConfigHelper.ImageServer + relativePath.Replace('\\', '/');


                    //重新压缩图片
                    if (fileData.ContentLength > (1024 * 500)) //如果大于500k，那么就需要重新压缩图片了
                    {
                        string relativeDirNew = PathHelper.GetSavePathTemp();
                        string relativePathNew = relativeDirNew + StringHelper.GetRandomString(12) + imageExtension;
                        string absolutePathnew = ConfigHelper.SharePath + relativePathNew;
                        string urlNew = ConfigHelper.ImageServer + relativePathNew.Replace('\\', '/');

                        ReCompressImage(absolutePath, absolutePathnew);

                        relativePath = relativePathNew;
                        url = urlNew;
                    }

                    //这个为了剪切功能生成图片大小
                    imageView = CutImageAndReturnPath(false, absolutePath, 0);

                }
            }
            catch (Exception ex)
            {
                // LogHelper.GetLogger(typeof(UtilityController)).Error("上传失败:" + ex.ToString());
            }

            return Json(imageView);
        }

        /// <summary>
        /// 剪切功能
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CutImage()
        {
            ImageView data;
            try
            {
                double x = Convert.ToDouble(Request.Params["l"]);
                double y = Convert.ToDouble(Request.Params["t"]);
                double w = Convert.ToDouble(Request.Params["w"]);
                double h = Convert.ToDouble(Request.Params["h"]);
                string i = Request.Params["i"];
                string imgsrc = Request.Params["imgsrc"];
                if (imgsrc.IndexOf('?') != -1)
                {
                    imgsrc = imgsrc.Substring(0, imgsrc.IndexOf('?'));
                }

                data = CutImageAndReturnPath(true, imgsrc, x, y, w, h);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                data = new ImageView { Error = 1, Message = ex.Message };
            }
            return Json(data);
        }
        private ImageView CutImageAndReturnPath(bool isCut, string imgsrc, double x, double y = 0, double w = 150, double h = 150)
        {
            Bitmap originalImg = new Bitmap(imgsrc);
            int origWidth = originalImg.Width;
            int origHeight = originalImg.Height;
            Bitmap newImg = LTImage.ResizeImage(originalImg, 300, 300, true, false);

            int newWidth = newImg.Width;
            int newHeight = newImg.Height;
            if (!isCut)
            {
                x = 0; //(newWidth - 150) / 2;
                if (newWidth < newHeight)
                {
                    w = newWidth;
                    h = newWidth;
                }
                else
                {
                    w = newHeight;
                    h = newHeight;
                }
            }
            //转换系数
            double zoom = Math.Round((double)originalImg.Width / (double)newImg.Width, 2);
            //转换后新坐标
            Point p = new Point(Convert.ToInt32(x * zoom), Convert.ToInt32(y * zoom));
            //转换后新截图范围
            Size s = new Size(Convert.ToInt32(w * zoom), Convert.ToInt32(h * zoom));
            if (originalImg.Width > p.X + s.Width && originalImg.Height > p.Y + s.Height)
            {
                if (s.Width < 218 && s.Height < 218)
                {
                    s.Width = s.Height = 218;
                }
            }

            Bitmap newCutImg = LTImage.Cut(originalImg, p.X, p.Y, s.Width, s.Height);
            var avatarDir = Path.GetDirectoryName(imgsrc);
            var ext = ".jpg";//Path.GetExtension(imgsrc);

            EncoderParameters parms = new EncoderParameters(1);
            var jpgCodecInfo = GetImageCodecInfo(ImageFormat.Jpeg);
            EncoderParameter parm = new EncoderParameter(Encoder.Quality, ((long)80));

            parms.Param[0] = parm;

            //if (origWidth < 640 || origHeight < 640)
            //{
            Bitmap b640 = LTImage.ResizeImage(newCutImg, 640, 640, true, true);
            string big640 = avatarDir + @"\img_0" + ext;
            b640.Save(big640, jpgCodecInfo, parms);
            //}
            //else
            //{
            Bitmap b320 = LTImage.ResizeImage(newCutImg, 320, 320, true, true);
            string big320 = avatarDir + @"\img_320" + ext;
            b320.Save(big320, jpgCodecInfo, parms);
            //}
            Bitmap b218 = LTImage.ResizeImage(newCutImg, 218, 218, true, true);
            Bitmap b100 = LTImage.ResizeImage(newCutImg, 100, 100, true, true);
            Bitmap b350 = LTImage.ResizeImage(newCutImg, 350, 350, true, true);
            string imginit = avatarDir + @"\img_0_init" + ext;
            string bigAvatarDir = avatarDir + @"\img_0_218" + ext;
            string middleAvatarDir = avatarDir + @"\img_0_100" + ext;
            string storePAvatarDir = avatarDir + @"\img_0_350" + ext;
            //用户上传的图片重新命名
            if (!System.IO.File.Exists(imginit))
                originalImg.Save(imginit, jpgCodecInfo, parms);
            //切218的图
            b218.Save(bigAvatarDir, jpgCodecInfo, parms);
            //切100的图
            b100.Save(middleAvatarDir, jpgCodecInfo, parms);
            b350.Save(storePAvatarDir, jpgCodecInfo, parms);
            originalImg.Dispose();
            newImg.Dispose();
            newCutImg.Dispose();
            b218.Dispose();
            b100.Dispose();
            b350.Dispose();
            return new ImageView
                {
                    Height = origHeight,
                    Width = origWidth,
                    Name = Path.GetFileName(imgsrc),
                    Url = imgsrc.Replace(ConfigHelper.SharePath, ConfigHelper.ImageServer).Replace('\\', '/') + "?v=" + DateTime.Now.ToString("HHMMddss"),
                    Path = imgsrc.Replace(ConfigHelper.SharePath, "")
                };
        }

        /// <summary>
        /// 上传店铺简介图片
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public JsonResult UploadStoreImg()
        {
            HttpPostedFileBase fileData = Request.Files[0];
            string saveFilePath;
            if (!fileData.HasFile())
            {
                return Json(new ImageView() { Error = 1, Message = "上載的圖片无效！" });
            }
            try
            {
                string uploadFileName = fileData.FileName;
                string imageExtension = Path.GetExtension(uploadFileName);

                if (!ValidateImageExtension(imageExtension))
                {
                    return Json(new ImageView() { Error = 1, Message = "不允許上載“" + imageExtension + "”格式的圖片！" });
                }
                else
                {
                    string relativeDir = PathHelper.GetSaveSjPathImg();

                    CheckOrCreateDirectory(ConfigHelper.SharePath + relativeDir);

                    string tempStr = StringHelper.GetRandomString(12) + imageExtension;
                    saveFilePath = relativeDir + StringHelper.GetRandomString(12) + imageExtension;

                    fileData.SaveAs(ConfigHelper.SharePath + saveFilePath);
                }
            }
            catch
            {
                saveFilePath = string.Empty;
            }

            string saveFileUrl = ConfigHelper.ImageServer + saveFilePath.Replace(ConfigHelper.SharePath, "").Replace('\\', '/');
            var image = new ImageView { Path = saveFilePath.Replace('\\', '/'), Url = saveFileUrl };

            return Json(image);
        }

        /// <summary>
        /// 重新压缩图片
        /// </summary>
        /// <param name="imagePath">全路径</param>
        /// <param name="imageSave"></param>
        public void ReCompressImage(string imagePath, string imageSave)
        {
            ImageHelper.ZoomSmallPic(imagePath, imageSave, int.MaxValue, int.MaxValue);
        }

        public static ImageCodecInfo GetImageCodecInfo(ImageFormat format)
        {

            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo icf in encoders)
            {

                if (icf.FormatID == format.Guid)
                {

                    return icf;

                }

            }

            return null;

        }

        #region 保存图片

        public JsonResult UploadSupplierLogo()
        {
            HttpPostedFileBase fileData = Request.Files[0];
            int newImgWidth = 0;
            int newImgHeight = 0;

            if (!fileData.HasFile())
            {
                return Json(new ImageView() { Error = 1, Message = "上載的圖片无效！" });
            }
            if (fileData.ContentLength > (512 * 1024))
            {
                return Json(new ImageView() { Error = 1, Message = "上載的圖片尺寸不能超过500KB！" });
            }

            string uploadFileName = fileData.FileName;
            string imageExtension = Path.GetExtension(uploadFileName);

            if (!ValidateImageExtension(imageExtension))
            {
                return Json(new ImageView() { Error = 1, Message = "不允許上載“" + imageExtension + "”格式的圖片！" });
            }


            string saveFilePath = string.Empty;
            string saveFileDir = ConfigHelper.SharePath + PathHelper.GetSavePathImg();

            if (!Directory.Exists(saveFileDir))
                Directory.CreateDirectory(saveFileDir);

            saveFilePath = saveFileDir + StringHelper.GetRandomString(12) + imageExtension;
            fileData.SaveAs(saveFilePath);

            Bitmap aImg = new Bitmap(saveFilePath);
            Bitmap b = LTImage.ResizeImage(aImg, 300, 300);
            newImgWidth = b.Width;
            newImgHeight = b.Height;

            if (fileData.ContentLength > (1024 * 500))//如果大于500k，那么就需要重新压缩图片了
            {
                string saveFileDirNew = ConfigHelper.SharePath + PathHelper.GetSavePathImg();

                string saveFilePathNew = saveFileDirNew + StringHelper.GetRandomString(12) + imageExtension;

                ReCompressImage(saveFilePath, saveFilePathNew);

                saveFilePath = saveFilePathNew;
            }

            string saveImg200 = saveFilePath.Replace(imageExtension, "_218" + imageExtension);
            SaveProductImageSizeOld(saveFilePath.Replace(ConfigHelper.SharePath, ""), saveImg200, 218, 218);


            ImageView result = new ImageView()
            {
                Url = saveFilePath.Replace(ConfigHelper.SharePath, ConfigHelper.ImageServer).Replace("\\", "//"),
                Path = saveFilePath.Replace(ConfigHelper.SharePath, ""),
            };

            return Json(result);

        }

        private void SaveProductImageSizeOld(string productImagePath, string savePath = "", int width = 100,
                                            int height = 100)
        {
            string strFileDir = Path.GetDirectoryName(productImagePath);
            var fileName = Path.GetFileName(productImagePath);

            if (fileName != null)
            {
                string[] strFielNameExtName = fileName.Split('.');

                //同路径下，保存100*100的小图
                string productImagePath100 = ConfigHelper.SharePath + strFileDir + "\\" + strFielNameExtName[0] +
                                             "_100." + strFielNameExtName[1];
                if (!string.IsNullOrEmpty(savePath))
                    productImagePath100 = savePath;

                ImageHelper.ZoomSmallPic(ConfigHelper.SharePath + productImagePath, productImagePath100, width, height);
            }
        }

        #endregion

        /// <summary>
        /// 当目录不存在时创建目录
        /// </summary>
        /// <param name="dir"></param>
        public static void CheckOrCreateDirectory(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
    }
}
