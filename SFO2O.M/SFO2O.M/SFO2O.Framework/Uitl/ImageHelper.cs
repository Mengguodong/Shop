using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using Encoder = System.Drawing.Imaging.Encoder;

namespace SFO2O.Framework.Uitl
{
    public static class ImageHelper
    {
        /// <summary>
        /// 输出一个图片到输出流
        /// </summary>
        /// <param name="response"></param>
        /// <param name="b"></param>
        /// <param name="imgFormat"></param>
        /// <param name="freshness"></param>
        /// <param name="qualityNum"></param>
        public static void OutPutImage(HttpResponse response, Bitmap b, ImageFormat imgFormat, TimeSpan? freshness, int qualityNum)
        {
            response.Clear();
            response.BufferOutput = false;      //提高效率

            if (freshness.HasValue) //要缓冲
            {
                DateTime now = DateTime.Now;
                response.Cache.SetExpires(now.Add((TimeSpan)freshness));
                response.Cache.SetMaxAge((TimeSpan)freshness);
                response.Cache.SetCacheability(HttpCacheability.Public);
                response.Cache.SetValidUntilExpires(true);
            }
            else
            {
                response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));//特别注意
                response.Cache.SetCacheability(HttpCacheability.NoCache);//特别注意
                response.AppendHeader("  Pragma", "No-Cache"); //特别注意
            }

            ImageCodecInfo myImageCodecInfo = ImageHelper.GetEncoder(imgFormat);

            var myEncoder = Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, (long)qualityNum);
            myEncoderParameters.Param[0] = myEncoderParameter;

            response.ContentType = GetContentTypeFromImageFormat(imgFormat);

            b.Save(response.OutputStream, myImageCodecInfo, myEncoderParameters);

            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }

        /// <summary>
        /// 给ImageResize使用的私有方法，得到一个图形的编码方法
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据图片格式返回图片的content_type
        /// </summary>
        /// <param name="imgFormat"></param>
        /// <returns></returns>
        private static string GetContentTypeFromImageFormat(ImageFormat imgFormat)
        {
            if (Equals(imgFormat, ImageFormat.Jpeg))
                return "image/jpeg";
            else if (Equals(imgFormat, ImageFormat.Gif))
                return "image/gif";
            else if (Equals(imgFormat, ImageFormat.Png))
                return "image/png";
            else
                return "image/jpeg";

        }

        /// <summary>
        /// 将字符串转换为Color
        /// </summary>
        /// <param name="color">带#号的16进制颜色</param>
        /// <returns></returns>
        public static Color Hex2Color(string color)
        {

            int red, green, blue = 0;
            char[] rgb;
            color = color.TrimStart('#');
            color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
            switch (color.Length)
            {
                case 3:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[0].ToString(), 16);
                    green = Convert.ToInt32(rgb[1].ToString() + rgb[1].ToString(), 16);
                    blue = Convert.ToInt32(rgb[2].ToString() + rgb[2].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                case 6:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
                    green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
                    blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                default:
                    return Color.FromName(color);

            }

        }

        /// <summary>
        /// 旋转图片
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="angle"></param>
        /// <param name="bkColor"></param>
        /// <returns></returns>
        public static Bitmap KiRotate(Bitmap bmp, float angle, Color bkColor)
        {
            int w = bmp.Width;
            int h = bmp.Height;

            PixelFormat pf;

            if (bkColor == Color.Transparent)
            {
                pf = PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }

            Bitmap tmp = new Bitmap(w, h, pf);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 0, 0);
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);

            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
            g = Graphics.FromImage(dst);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
            g.Dispose();

            tmp.Dispose();

            return dst;
        }

        /// <summary>
        /// 翻转图片
        /// </summary>
        /// <param name="b"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Bitmap FlipImage(Bitmap b, int direction)
        {
            int scan_height = b.Height;
            int scan_width = b.Width;

            var bitsInput = b.LockBits(new Rectangle(0, 0, scan_width, scan_height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* input_scan = (byte*)bitsInput.Scan0;
                int input_stride = bitsInput.Stride;

                if (direction == 0)
                {
                    int half_range = scan_height / 2;


                    for (int y = 0; y < half_range; y++)
                    {
                        byte* ptrTop = input_scan + y * input_stride;
                        byte* ptrBottom = input_scan + (scan_height - y - 1) * input_stride;

                        for (int x = 0; x < scan_width; x++)
                            ExchangeByte(y, x, ptrTop + 4 * x, ptrBottom + 4 * x, 4);
                    }
                }
                else
                {
                    int half_range = scan_width / 2;

                    for (int y = 0; y < scan_height; y++)
                    {
                        byte* ptrInput = input_scan + y * input_stride;

                        for (int x = 0; x < half_range; x++)
                            ExchangeByte(y, x, ptrInput + 4 * x, ptrInput + 4 * (scan_width - x - 1), 4);
                    }
                }
            }

            b.UnlockBits(bitsInput);
            return b;
        }
        private static unsafe void ExchangeByte(int y, int x, byte* p1, byte* p2, int step)
        {
            byte t;

            for (int i = 0; i < step; i++)
            {
                t = *(p1 + i);
                *(p1 + i) = *(p2 + i);
                *(p2 + i) = t;
            }
        }


        /// <summary>
        /// 根据一个一维数组图中获取某个颜色的子集并放在同一个bitmap里
        /// </summary>
        /// <param name="bInput"></param>
        /// <param name="filterColor"></param>
        /// <param name="replaceColor"></param>
        /// <param name="tolarence"></param>
        /// <returns></returns>
        public static Bitmap FilterImageColor(Bitmap bInput, Color filterColor, Color replaceColor, double tolarence)
        {
            int scan_height = bInput.Height;
            int scan_width = bInput.Width;
            Bitmap b_output = new Bitmap(bInput.Width, bInput.Height, PixelFormat.Format32bppArgb);
            var bitsOutput = b_output.LockBits(new Rectangle(0, 0, bInput.Width, bInput.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            var bitsInput = bInput.LockBits(new Rectangle(0, 0, bInput.Width, bInput.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            byte filter_b = filterColor.B;
            byte filter_g = filterColor.G;
            byte filter_r = filterColor.R;

            byte replace_b = replaceColor.B;
            byte replace_g = replaceColor.G;
            byte replace_r = replaceColor.R;
            byte replace_a = replaceColor.A;

            unsafe
            {
                byte* output_scan = (byte*)bitsOutput.Scan0;
                int output_stride = bitsOutput.Stride;


                byte* input_scan = (byte*)bitsInput.Scan0;
                int input_stride = bitsInput.Stride;

                for (int y = 0; y < scan_height; y++)
                {
                    byte* ptrOutput = output_scan + y * output_stride;
                    byte* ptrInput = input_scan + y * input_stride;

                    for (int x = 0; x < scan_width; x++)
                    {
                        byte input_b = ptrInput[3 * x + 0];
                        byte input_g = ptrInput[3 * x + 1];
                        byte input_r = ptrInput[3 * x + 2];

                        int b_dis = input_b - filter_b;
                        int g_dis = input_g - filter_g;
                        int r_dis = input_r - filter_r;

                        double dis = Math.Sqrt(b_dis * b_dis + g_dis * g_dis + r_dis * r_dis);

                        if (dis <= tolarence) //在允许范围内
                        {
                            ptrOutput[4 * x + 0] = replace_b;
                            ptrOutput[4 * x + 1] = replace_g;
                            ptrOutput[4 * x + 2] = replace_r;
                            ptrOutput[4 * x + 3] = replace_a;
                        }
                        else
                        {
                            ptrOutput[4 * x + 0] = input_b;
                            ptrOutput[4 * x + 1] = input_g;
                            ptrOutput[4 * x + 2] = input_r;
                            ptrOutput[4 * x + 3] = 255;
                        }
                    }
                }
            }

            b_output.UnlockBits(bitsOutput);
            bInput.UnlockBits(bitsInput);

            return b_output;
        }

        /// <summary>
        /// 文件转移到指定路径，可重命名
        /// </summary>
        /// <param name="sourcePath">原图片路径</param>
        /// <param name="targetPath">保存的图片路径</param>
        /// <param name="isSameDir">是否同一个目录的文件移动</param>
        /// <returns></returns>
        public static bool ImageMoveTo(string sourcePath, string targetPath)
        {
            targetPath = targetPath.Replace(ConfigHelper.ImageServer, ConfigHelper.SharePath).Replace("/", "\\");
            sourcePath = sourcePath.Replace(ConfigHelper.ImageServer, ConfigHelper.SharePath).Replace("/", "\\");
            string sourcePathDir = Path.GetDirectoryName(sourcePath);

            if (string.IsNullOrEmpty(sourcePathDir))
                return false;

            //判断来源文件是否存在
            if (!Directory.Exists(sourcePathDir))
                return false;

            //判断目标文件是否存在
            if (File.Exists(targetPath))
            {
                var targetfile = new FileInfo(targetPath);
                if (targetfile.IsReadOnly)
                    targetfile.Attributes = FileAttributes.Normal;
                targetfile.Delete();
            }
            else
            {
                //创建目录
                string targetFileDir = Path.GetDirectoryName(targetPath);

                if (!string.IsNullOrEmpty(targetFileDir))
                {
                    if (!Directory.Exists(targetFileDir))
                        Directory.CreateDirectory(targetFileDir);
                }
                else
                    return false;
            }

            //保存操作
            var sourceFile = new FileInfo(sourcePath);
            sourceFile.CopyTo(targetPath);//MoveTo

            return true;
        }

        /// <summary>
        /// 图片目录文件拷贝
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="targetDir"></param>
        /// <returns></returns>
        public static bool CopyImages(string sourceDir, string targetDir)
        {
            bool returnResult = false;

            //目录操纵
            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);
            else
            {
                try
                {
                    Directory.Delete(targetDir);
                    Directory.CreateDirectory(targetDir);
                }
                catch (Exception ex)
                {
                    string msgError = "图片目录移动失败：" + sourceDir + " " + targetDir + ":" + ex.ToString();
                    LogHelper.GetLogger(typeof(ImageHelper)).Info(msgError);
                }

            }

            IList<string> fileExtnameList = new List<string>() { ".jpg", ".png", ".gif" }; ;

            //提取文件
            List<FileInfo> fileInfoList = GetDirFiles(sourceDir);
            if (fileInfoList != null && fileInfoList.Count > 0)
            {
                foreach (FileInfo fileInfo in fileInfoList)
                {
                    if (fileExtnameList.Contains(fileInfo.Extension))
                    {
                        fileInfo.CopyTo(targetDir + fileInfo.Name, true);
                        returnResult = true;
                    }
                }
            }

            return returnResult;
        }

        /// <summary>
        /// 获取目录的所有文件，包括子文件
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static List<FileInfo> GetDirFiles(string folder)
        {
            var fileInfoList = new List<FileInfo>();
            try
            {
                var directoryInfo = new DirectoryInfo(folder);
                DirectoryInfo[] dirChirdren = directoryInfo.GetDirectories();

                FileInfo[] fis = directoryInfo.GetFiles();
                fileInfoList.AddRange(fis);

                foreach (DirectoryInfo childDirectoryInfo in dirChirdren)
                {
                    List<FileInfo> childFiles = GetDirFiles(childDirectoryInfo.FullName);
                    fileInfoList.AddRange(childFiles);
                }

            }
            catch
            {
                throw new Exception("读取文件失败");
            }

            return fileInfoList;
        }

        /// <summary>
        /// 保存图片到正式路径下，并且返回新路径（部分）
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static string SaveImageToDir(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                string imageExt = Path.GetExtension(imagePath);
                string imagePathPart = PathHelper.GetSavePathImg() + StringHelper.GetRandomString(12) + imageExt;

                if (ImageMoveTo(imagePath, ConfigHelper.SharePath + imagePathPart))
                    return imagePathPart;
                else
                    return string.Empty;

            }

            return string.Empty;
        }

        /// <summary>
        /// 按比例缩小图片，自动计算宽度
        /// </summary>
        /// <param name="strOldPic">源图文件名(包括路径)</param>
        /// <param name="strNewPic">缩小后保存为文件名(包括路径)</param>
        /// <param name="inputWidth">缩小到宽度</param>
        /// <param name="inputHeight">缩小至高度</param>
        public static bool ZoomSmallPic(string strOldPic, string strNewPic, int inputWidth, int inputHeight)
        {
            Bitmap objPic, objNewPic, outBitMap;
            try
            {
                objPic = new Bitmap(strOldPic);

                int imageWidth = objPic.Width;
                int imageHeight = objPic.Height;


                if (imageWidth > inputWidth)
                {
                    imageHeight = ParseHelper.ToInt((double)inputWidth / (double)imageWidth * imageHeight);
                    imageWidth = inputWidth;
                }
                if (imageHeight > inputHeight)
                {
                    imageWidth = ParseHelper.ToInt((double)inputHeight / (double)imageHeight * imageWidth);
                    imageHeight = inputHeight;
                }

                //缩放好的图片
                objNewPic = new Bitmap(objPic, imageWidth, imageHeight);

                if (inputWidth == int.MaxValue || inputHeight == int.MaxValue)
                {
                    inputWidth = objPic.Width;
                    inputHeight = objPic.Height;
                }


                //放到图片的中心位置
                outBitMap = new Bitmap(inputWidth, inputHeight);//输出的图片

                Graphics grap = Graphics.FromImage(outBitMap);
                grap.Clear(Color.White);
                grap.CompositingQuality = CompositingQuality.HighSpeed;

                int newPositonX = (inputWidth - imageWidth) / 2;
                int newPositonY = (inputHeight - imageHeight) / 2;

                grap.DrawImage(objNewPic, newPositonX, newPositonY);

                //检测目录是否存在
                var saveDir = Path.GetDirectoryName(strNewPic);

                if (saveDir != null && !Directory.Exists(saveDir))
                {
                    Directory.CreateDirectory(saveDir);
                }

                string mergeImageExt = Path.GetExtension(strNewPic);
                if (mergeImageExt != null && mergeImageExt.Contains("png"))
                    outBitMap.Save(strNewPic, ImageFormat.Png);
                else if (mergeImageExt != null && mergeImageExt.Contains("gif"))
                    outBitMap.Save(strNewPic, ImageFormat.Gif);
                else
                    outBitMap.Save(strNewPic, ImageFormat.Jpeg);



                grap.Dispose();

                objPic.Dispose();
                objNewPic.Dispose();
                outBitMap.Dispose();

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                objPic = null;
                objNewPic = null;
                outBitMap = null;
            }

            return true;
        }

        /// <summary>
        /// 等比例缩放，不留白
        /// </summary>
        /// <param name="strOldPic"></param>
        /// <param name="strNewPic"></param>
        /// <param name="inputWidth"></param>
        /// <param name="inputHeight"></param>
        /// <returns></returns>
        public static bool ZoomSmallPic2(string strOldPic, string strNewPic, int inputWidth, int inputHeight)
        {
            Bitmap objPic, objNewPic, outBitMap;
            try
            {
                objPic = new Bitmap(strOldPic);

                int imageWidth = objPic.Width;
                int imageHeight = objPic.Height;


                if (imageWidth > inputWidth)
                {
                    imageHeight = ParseHelper.ToInt((double)inputWidth / (double)imageWidth * imageHeight);
                    imageWidth = inputWidth;
                }
                if (imageHeight > inputHeight)
                {
                    imageWidth = ParseHelper.ToInt((double)inputHeight / (double)imageHeight * imageWidth);
                    imageHeight = inputHeight;
                }

                //缩放好的图片
                objNewPic = new Bitmap(objPic, imageWidth, imageHeight);


                //检测目录是否存在
                var saveDir = Path.GetDirectoryName(strNewPic);

                if (saveDir != null && !Directory.Exists(saveDir))
                {
                    Directory.CreateDirectory(saveDir);
                }

                string mergeImageExt = Path.GetExtension(strNewPic);
                if (mergeImageExt != null && mergeImageExt.Contains("png"))
                    objNewPic.Save(strNewPic, ImageFormat.Png);
                else if (mergeImageExt != null && mergeImageExt.Contains("gif"))
                    objNewPic.Save(strNewPic, ImageFormat.Gif);
                else
                    objNewPic.Save(strNewPic, ImageFormat.Jpeg);



                objPic.Dispose();
                objNewPic.Dispose();


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                objPic = null;
                objNewPic = null;
                outBitMap = null;
            }

            return true;
        }


        /// <summary>
        /// 给图片添加文字水印
        /// </summary>
        /// <param name="filePath">图片完整服务器路径</param>
        /// <param name="text">水印文字</param>
        /// <param name="rate"></param>
        public static void AddText(string filePath, string text, double rate = 1.0)
        {
            if (string.IsNullOrEmpty(text))
                return;

            Image image = Image.FromFile(filePath);
            Graphics g = Graphics.FromImage(image);

            //将图片绘制到graphics中
            g.DrawImage(image, 0, 0, image.Width, image.Height);

            //文字的起始位置，x,y坐标,计算坐标
            int fontHeight = ParseHelper.ToInt(50 * rate);

            var family = new FontFamily("微软雅黑");
            const int fontStyle = (int)FontStyle.Regular;


            int emSize = ParseHelper.ToInt(40 * rate);

            int x = image.Width / 2 - text.Length * emSize / 2;
            int y = image.Height / 2 - fontHeight / 2;
            if (x < 0)
                x = 0;

            var origin = new Point(x, y);
            StringFormat format = StringFormat.GenericDefault;

            //设置字体的颜色
            //Brush bFont = new SolidBrush(Color.FromArgb(214, 203, 199));
            Brush bFont = new SolidBrush(Color.WhiteSmoke);

            //描边
            var myPath = new GraphicsPath();
            myPath.AddString(text, family, fontStyle, emSize, origin, format);
            g.FillPath(bFont, myPath);

            //写字（0表示没有，2是以前的参数）
            //g.DrawPath(new Pen(Color.Black, 2), myPath);
            g.DrawPath(new Pen(Color.WhiteSmoke, 0), myPath);

            //释放graphics
            g.Dispose();

            //确定新图片的文件路径
            var oldfileName = Path.GetFileNameWithoutExtension(filePath);
            var tempPath = filePath;

            if (oldfileName != null)
            {
                string newpath = filePath.Replace(oldfileName, "img_X");
                //保存写上字的图片
                image.Save(newpath);
                //释放image
                image.Dispose();
                //删除没加水印的图片，记得一定要放在image释放之后，否则无法删除
                File.Delete(filePath);
                //重命名
                File.Move(newpath, tempPath);
            }
        }

        /// <summary>
        /// 针对218的图片水印
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="text"></param>
        public static void AddText2(string filePath, string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            Image image = Image.FromFile(filePath);
            Graphics g = Graphics.FromImage(image);

            //将图片绘制到graphics中
            g.DrawImage(image, 0, 0, image.Width, image.Height);

            //文字的起始位置，x,y坐标,计算坐标
            const int fontHeight = 20;

            var family = new FontFamily("微软雅黑");
            const int fontStyle = (int)FontStyle.Regular;


            const int emSize = 15;

            int x = image.Width / 2 - text.Length * emSize / 2;
            int y = image.Height / 2 - fontHeight / 2;

            if (x < 0)
                x = 0;


            var origin = new Point(x, y);
            StringFormat format = StringFormat.GenericDefault;

            //设置字体的颜色
            Brush bFont = new SolidBrush(Color.WhiteSmoke);

            //描边
            var myPath = new GraphicsPath();
            myPath.AddString(text, family, fontStyle, emSize, origin, format);
            g.FillPath(bFont, myPath);

            //写字（0表示没有，2是以前的参数）
            //g.DrawPath(new Pen(Color.Black, 2), myPath);
            g.DrawPath(new Pen(Color.WhiteSmoke, 0), myPath);

            //释放graphics
            g.Dispose();

            //确定新图片的文件路径
            var oldfileName = Path.GetFileNameWithoutExtension(filePath);
            var tempPath = filePath;

            if (oldfileName != null)
            {
                string newpath = filePath.Replace(oldfileName, "img_X");
                //保存写上字的图片
                image.Save(newpath);
                //释放image
                image.Dispose();
                //删除没加水印的图片，记得一定要放在image释放之后，否则无法删除
                File.Delete(filePath);
                //重命名
                File.Move(newpath, tempPath);
            }
        }

        /// <summary>
        /// 剪切图片，另存
        /// </summary>
        /// <param name="oldImgPath">原图片地址</param>
        /// <param name="newImgPath">新图片保存地址</param>
        /// <param name="newFileName">新图片名称</param>
        /// <param name="inputWidth">截取宽度</param>
        /// <param name="inputLength">截取长度</param>
        public static void ShearingImg(string oldImgPath, string newImgPath, string newFileName, int inputWidth = 100, int inputLength = 100)
        {
            //原图片路径
            String oldPath = oldImgPath;

            //新图片路径
            String newPath = System.IO.Path.GetExtension(newImgPath);
            string fileName = Path.GetFileName(oldImgPath);
            string[] str = fileName.Split('.');

            //设置截取的坐标和大小
            int x = 0, y = 0, width = inputWidth, height = inputLength;

            //计算新的文件名
            newPath = newImgPath + newFileName + "." + str[1];
            //定义截取矩形
            System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle(x, y, width, height); //要截取的区域大小

            //加载图片
            System.Drawing.Image img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(System.IO.File.ReadAllBytes(oldPath)));

            //判断超出的位置否
            if ((img.Width < x + width) || img.Height < y + height)
            {
                //截取的区域超过了图片本身的高度、宽度.
                img.Dispose();
                return;
            }
            //定义Bitmap对象
            System.Drawing.Bitmap bmpImage = new System.Drawing.Bitmap(img);

            //进行裁剪
            System.Drawing.Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);

            //保存成新文件
            bmpCrop.Save(newPath);

            //释放对象
            img.Dispose();
            bmpCrop.Dispose();
        }



        public static void ShearingImgForCenter(Image originalImage, string newImgPath, int cutWidth, int cutHeight, int marginTop = 0, int marginLeft = 0, int saveWidth = 100, int saveHeight = 100)
        {
            //第一部分：裁剪操作
            //设置截取的坐标和大小
            int x = marginLeft, y = marginTop, width = cutWidth, height = cutHeight;

            //定义截取矩形
            var cropArea = new Rectangle(x, y, width, height); //要截取的区域大小


            //定义Bitmap对象
            var bmpImage = new Bitmap(originalImage);

            //进行裁剪
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);//一定是一个正方形


            //第二部分：缩放操作
            Bitmap objNewPic = new Bitmap(bmpCrop, saveWidth, saveHeight); ;

            //保存成新文件
            objNewPic.Save(newImgPath);

            //释放对象
            originalImage.Dispose();
            bmpCrop.Dispose();
            objNewPic.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productImagePath"></param>
        /// <returns></returns>
        public static string GetImageUrlByProductPath(string productImagePath)
        {
            return ConfigHelper.ImageServer + productImagePath.Replace("\\", "/");
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

    }
}
