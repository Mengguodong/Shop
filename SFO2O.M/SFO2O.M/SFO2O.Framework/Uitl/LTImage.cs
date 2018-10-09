using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Configuration;
using System.IO;

namespace SFO2O.Framework.Uitl
{
    /// <summary>
    /// 公用图片处理函数
    /// </summary>
    public static class LTImage
    {
        /// <summary>
        /// 缺省的图片文件名长度
        /// </summary>
        public const int IMG_NAME_LENGTH = 9;

        private static string IMG_ROOT_PATH = ConfigurationManager.AppSettings["SharePath"];//"//192.168.99.45/Share/";
        //string path = @"d:\ftp_root\ftp06\baosheng\DSO\DSO_" + billno + ".txt";
        public static int[,] PRODUCT_IMG_RULEZU = new int[4, 2] { { 570, 382 }, { 368, 276 }, { 160, 120 }, { 40, 40 } };
        public static int[,] PRODUCT_IMG_FR_RULE = new int[2, 2] { { 180, 210 }, { 160, 120 } };
        public static int[,] PRODUCT_IMG_SQUARE_RULE = new int[1, 2] { { 160, 160 } };

        /// <summary>
        /// 获得一个图片文件名在img目录下的路径，例如12345.jpg的返回结果就是/img/12/12345.jpg
        /// </summary>
        /// <param name="img_name">输入的文件名</param>
        /// <param name="img_no">图片编号</param>
        /// <returns>返回的虚拟路径</returns>
        public static string getImgPath(string img_name, int img_no)
        {
            if (img_name == null)
                return null;
            if (img_name.Length < 2)
                return null;

            string lower_case_img_name = img_name.ToLower();
            return IMG_ROOT_PATH + "/" + lower_case_img_name.Replace("\\", "/").Trim('/');

        }

        /// <summary>
        /// 获取图片的真实路径 
        /// </summary>
        /// <param name="img_name"></param>
        /// <param name="img_no"></param>
        /// <returns></returns>
        public static string getImgRealPath(string img_name, int img_no)
        {
            string img_path = getImgPath(img_name, img_no);
            return System.Web.HttpContext.Current.Server.MapPath(img_path);
        }

        /// <summary>
        /// 以只读模式从磁盘上读取一个文件到实体类里
        /// 如果文件不存在，返回空
        /// </summary>
        /// <param name="img_path"></param>
        /// <returns></returns>
        public static System.Drawing.Image GetImgObjectFromFile(string img_path)
        {
            try
            {
                using (FileStream img_fs = new FileStream(img_path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    System.Drawing.Image img_obj = System.Drawing.Image.FromStream(img_fs);
                    img_fs.Close();
                    return img_obj;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 删除一个图片文件的所有四个大小格式
        /// </summary>
        /// <param name="img_file_name"></param>
        /// <returns></returns>
        public static int DeleteImage(string img_file_name)
        {
            if (img_file_name.StartsWith("/")) ///新版
            {
                for (int i = 0; i < 4; i++)
                {
                    string img_web_path = getImgPath(img_file_name, i);
                    string img_path = img_web_path;
                    System.IO.File.Delete(img_path);
                }

                return 0;
            }


            for (int i = 0; i < 4; i++)
            {
                string img_web_path = "~" + getImgPath(img_file_name, i);
                string img_path = img_web_path;
                System.IO.File.Delete(img_path);
            }

            return 0;
        }

        /// <summary>
        /// 将一个上传的图片保存在服务器上的统一图片目录中，并返回新的图片web路径，可以给图片改大小。
        /// </summary>
        /// <param name="strOrigImgPath">图片原始路径，为磁盘上的绝对路径</param>
        /// <param name="childRules">图片规则，允许生成多重规格的图片</param>
        /// <returns>新的图片文件名，未包含路径，路径后期算出</returns>
        public static string SaveUploadImage(string strOrigImgPath, int[,] childRules, int logo_num, int logo_pos)
        {
            string img_name = SaveUploadImage(strOrigImgPath, childRules);

            if (logo_num > 0)
            {

                string img_web_path0 = "~" + getImgPath(img_name, -1);
                string img_path0 = System.Web.HttpContext.Current.Server.MapPath(img_web_path0);

                //AddWaterMark(img_path0, logo_num - 1, 0); //最大图的水印在中间


                string img_web_path = "~" + getImgPath(img_name, 1);
                string img_path = System.Web.HttpContext.Current.Server.MapPath(img_web_path);

                //AddWaterMark(img_path, logo_num - 1, logo_pos);

                string img_web_path1 = "~" + getImgPath(img_name, 0);
                string img_path1 = System.Web.HttpContext.Current.Server.MapPath(img_web_path1);

                //AddWaterMark(img_path1, logo_num - 1, logo_pos);
            };

            return img_name;

        }

        /// <summary>
        /// 将一个上传的图片保存在服务器上的统一图片目录中，并返回新的图片web路径，可以给图片改大小。
        /// </summary>
        /// <param name="strOrigImgPath">图片原始路径，为磁盘上的绝对路径</param>
        /// <param name="childRules">图片规则，允许生成多重规格的图片</param>
        /// <returns>新的图片文件名，未包含路径，路径后期算出</returns>
        public static string SaveUploadImage(string strOrigImgPath, int[,] childRules)
        {
            string repos_dir = Verify.GetSetRepositoryDir(DateTime.Now);

            string target_dir = ConfigurationManager.AppSettings["SharePath"].ToString();//System.Web.HttpContext.Current.Server.MapPath(repos_dir); ///获取这个目录的实际目录

            string new_file_name = Verify.GetRandomNum(IMG_NAME_LENGTH); //10位吧，保险
            string file_ext = System.IO.Path.GetExtension(strOrigImgPath).ToLower();

            string new_file_path = target_dir + repos_dir + new_file_name + file_ext;

            //保存一张原图
            if (childRules == LTImage.PRODUCT_IMG_FR_RULE)
            {
                ResizeImage(strOrigImgPath, new_file_path, 736, 858);
            }
            //保存一张原图
            else if (childRules == LTImage.PRODUCT_IMG_SQUARE_RULE)
            {
                ResizeImage(strOrigImgPath, new_file_path, 1280, 1280);
            }
            else
            {
                ResizeImage(strOrigImgPath, new_file_path, 1280, 856);
            }

            for (int i = 0; i < childRules.Length / 2; i++)
            {
                string thumb_file_path = new_file_path.Replace(file_ext, "_" + i + file_ext);
                ResizeImage(strOrigImgPath, thumb_file_path, childRules[i, 0], childRules[i, 1]); //这里就不加锐了，先动态加吧。。。唉
            }

            return repos_dir + new_file_name + file_ext;

        }


        /// <summary>
        /// 将一个上传的图片保存在服务器上的统一图片目录中，并返回新的图片web路径，可以给图片改大小。
        /// </summary>
        /// <param name="strOrigImgPath"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns>新的图片文件名，虚拟路径</returns>
        public static string SaveUploadImage(string strOrigImgPath, int newWidth, int newHeight)
        {
            string repos_dir = Verify.GetSetRepositoryDir(DateTime.Now);
            string target_dir = System.Web.HttpContext.Current.Server.MapPath(repos_dir); ///获取这个目录的实际目录

            string new_file_name = Verify.GetRandomNum(IMG_NAME_LENGTH);
            string file_ext = System.IO.Path.GetExtension(strOrigImgPath).ToLower();

            string new_file_path = target_dir + new_file_name + file_ext;

            string newImagePath = target_dir + new_file_name + file_ext;
            ResizeImage(strOrigImgPath, newImagePath, newWidth, newHeight);

            return repos_dir + new_file_name + file_ext;

        }


        /// <summary>
        /// 将一个上传的图片保存在服务器上的统一图片目录中，并返回新的图片web路径，不改变图片大小
        /// </summary>
        /// <param name="strOrigImgPath">原始的图片文件名，真实路径</param>
        /// <returns>新的图片文件名，虚拟路径</returns>
        public static string SaveUploadImage(string strOrigImgPath)
        {
            string repos_dir = Verify.GetSetRepositoryDir(DateTime.Now);
            string target_dir = System.Web.HttpContext.Current.Server.MapPath(repos_dir); ///获取这个目录的实际目录

            string new_file_name = Verify.GetRandomNum(IMG_NAME_LENGTH);
            string file_ext = System.IO.Path.GetExtension(strOrigImgPath).ToLower();

            string new_file_path = target_dir + new_file_name + file_ext;
            string newImagePath = target_dir + new_file_name + file_ext;

            System.IO.File.Copy(strOrigImgPath, newImagePath, true);

            return repos_dir + new_file_name + file_ext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strOrigImgPath"></param>
        /// <param name="strNewImgPath"></param>
        /// <param name="nNewWidth"></param>
        /// <param name="nNewHeight"></param>
        /// <param name="sharp_level"></param>
        /// <returns></returns>
        public static int ResizeImage(String strOrigImgPath, String strNewImgPath, int nNewWidth, int nNewHeight)
        {
            return ResizeImage(strOrigImgPath, strNewImgPath, nNewWidth, nNewHeight, 0);
        }

        /// <summary>
        /// 改变一个图片文件的大小，但保留原图的长宽比例，只能缩小图，不能增大，所以要求输入的图片不能比新的图片小。
        /// </summary>
        /// <param name="strOrigImgPath">输入图片的路径</param>
        /// <param name="nNewImgPath">目标图片的路径</param>
        /// <param name="nNewWidth">新的图片宽度</param>
        /// <param name="nNewHeight">系的图片高度</param>
        /// <returns>-1表示失败，0表示成功</returns>
        public static int ResizeImage(String strOrigImgPath, String strNewImgPath, int nNewWidth, int nNewHeight, int sharpen_level)
        {
            System.Drawing.Image imgNewImage = GetImgObjectFromFile(strOrigImgPath);

            ImageFormat fmtImageFormat = imgNewImage.RawFormat;

            ImageCodecInfo myImageCodecInfo = GetEncoder(fmtImageFormat);
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;
            //myImageCodecInfo = ImageCodecInfo.GetImageEncoders()[0];
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, 95L); // 0-100
            myEncoderParameters.Param[0] = myEncoderParameter;


            if (imgNewImage == null)  // 无法读取原图，这个地方可能得try一下。。
                return -1;


            Bitmap bmpTmpBitmap;

            if ((bmpTmpBitmap = ResizeImage(imgNewImage, nNewWidth, nNewHeight)) == null) //调用另一个方法改变图的大小
                return -1;

            //if (sharpen_level > 0)
            //    CSharpFilters.BitmapFilter.Sharpen(bmpTmpBitmap, sharpen_level);

            //保存新的图

            bmpTmpBitmap.Save(strNewImgPath, myImageCodecInfo, myEncoderParameters);

            bmpTmpBitmap.Dispose();
            imgNewImage.Dispose();
            return 0;


        }

        public static Bitmap ResizeImage(String img_name, int nNewWidth, int nNewHeight)
        {
            return ResizeImage(img_name, nNewWidth, nNewHeight, true);
        }
        /// <summary>
        /// 动态改变一个图片大小并返回实体对象,
        /// </summary>
        /// <param name="img_name"></param>
        /// <param name="nNewWidth"></param>
        /// <param name="nNewHeight"></param>
        /// <param name="from_big_img"></param>
        /// <returns></returns>
        public static Bitmap ResizeImage(String img_name, int nNewWidth, int nNewHeight, bool from_big_img)
        {
            return ResizeImage(img_name, nNewWidth, nNewHeight, from_big_img, false);
        }

        /// <summary>
        /// 动态改变一个图片大小并返回实体对象
        /// </summary>
        /// <param name="img_name"></param>
        /// <param name="nNewWidth"></param>
        /// <param name="nNewHeight"></param>
        /// <param name="isSetInterpolationMode"></param>
        /// <returns></returns>
        public static Bitmap ResizeImage(String img_name, int nNewWidth, int nNewHeight, bool from_big_img, bool isSetInterpolationMode)
        {
            if (img_name == null)
                return null;

            ///检查后缀名
            img_name = img_name.Trim();
            string img_extension = Path.GetExtension(img_name).ToLower();
            if (!Verify.checkMember(img_extension, new string[] { ".jpg", ".jpeg", ".gif", ".png" }))
                return null;

            string ImageWebPath = "";

            if (img_name.StartsWith("/img")) //新版的由/img开始，旧版的直接是文件名
            {
                if (!from_big_img)
                {
                    ImageWebPath = img_name.Replace(img_extension, "_0" + img_extension);
                }
                else
                    ImageWebPath = img_name;
            }
            else
            {
                if (!from_big_img)
                    ImageWebPath = IMG_ROOT_PATH + img_name.Substring(0, 2) + "/" + img_name.Replace(img_extension, "_0" + img_extension);
                else
                    ImageWebPath = IMG_ROOT_PATH + img_name.Substring(0, 2) + "/" + img_name;
            }

            string ImgRealPath = System.Web.HttpContext.Current.Server.MapPath(ImageWebPath);

            //这么做也是无可奈何，关键是之前的图片规则定义没定义好，假设都是商品图片，都有_0存在，这就是问题了
            if (!System.IO.File.Exists(ImgRealPath))
                ImgRealPath = System.Web.HttpContext.Current.Server.MapPath(img_name);

            System.Drawing.Image imgNewImage = GetImgObjectFromFile(ImgRealPath);

            if (imgNewImage == null)
                return null;


            ImageFormat fmtImageFormat = imgNewImage.RawFormat;

            ImageCodecInfo myImageCodecInfo = GetEncoder(fmtImageFormat);
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;
            //myImageCodecInfo = ImageCodecInfo.GetImageEncoders()[0];
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, 95L); // 0-100
            myEncoderParameters.Param[0] = myEncoderParameter;

            if (imgNewImage == null)  // 无法读取原图，这个地方可能得try一下。。
                return null;

            Bitmap bmpTmpBitmap;

            if ((bmpTmpBitmap = ResizeImage(imgNewImage, nNewWidth, nNewHeight, isSetInterpolationMode)) == null) //调用另一个方法改变图的大小
                return null;

            imgNewImage.Dispose();

            return bmpTmpBitmap;

        }

        public static Bitmap ResizeImage(System.Drawing.Image imgOrigImage, int nNewWidth, int nNewHeight)
        {
            return ResizeImage(imgOrigImage, nNewWidth, nNewHeight, true);
        }
        /// <summary>
        /// 改变一个图片对象的大小，但保留原图的长宽比例，只能缩小图，不能增大，所以要求输入的图片不能比新的图片小。
        /// </summary>
        /// <param name="imgOrigImage">原始图片定义，在System.Drawing里</param>
        /// <param name="nNewWidth">新的图片宽</param>
        /// <param name="nNewHeight">新的图片高</param>
        /// <returns>返回一个Bitmap类型的图片实例</returns>
        public static Bitmap ResizeImage(System.Drawing.Image imgOrigImage, int nNewWidth, int nNewHeight, bool isSetInterpolationMode)
        {
            ImageFormat fmtImageFormat = imgOrigImage.RawFormat;

            int nWidth = 0;
            int nHeight = 0;

            if (imgOrigImage.Width < nNewWidth && imgOrigImage.Height < nNewHeight)
            {
                nWidth = imgOrigImage.Width;
                nHeight = imgOrigImage.Height;
            }
            else
            {
                if ((double)imgOrigImage.Width / (double)imgOrigImage.Height < (float)nNewWidth / (float)nNewHeight)
                {
                    nWidth = (int)Math.Round(nNewHeight * ((double)imgOrigImage.Width / (double)imgOrigImage.Height));
                    nHeight = nNewHeight;
                }
                else
                {
                    nWidth = nNewWidth;
                    nHeight = (int)Math.Round(nNewWidth * ((double)imgOrigImage.Height / (double)imgOrigImage.Width));
                }
            }

            Bitmap bmpTmpBitmap = new Bitmap(nWidth, nHeight);
            Graphics gTmpGraphics = Graphics.FromImage(bmpTmpBitmap);

            // 插值算法的质量
            if (isSetInterpolationMode)
                gTmpGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            gTmpGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            //设置高质量,低速度呈现平滑程度
            gTmpGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            gTmpGraphics.DrawImage(imgOrigImage, new Rectangle(0, 0, nWidth, nHeight), new Rectangle(0, 0, imgOrigImage.Width, imgOrigImage.Height), GraphicsUnit.Pixel);
            gTmpGraphics.Dispose();


            return bmpTmpBitmap;

        }

        public static Bitmap ResizeImage_(System.Drawing.Image imgOrigImage, int nNewWidth, int nNewHeight, bool isSetInterpolationMode)
        {
            Bitmap bmpTmpBitmap = new Bitmap(nNewWidth, nNewHeight);
            Graphics gTmpGraphics = Graphics.FromImage(bmpTmpBitmap);

            // 插值算法的质量
            if (isSetInterpolationMode)
                gTmpGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            gTmpGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            //设置高质量,低速度呈现平滑程度
            gTmpGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            gTmpGraphics.DrawImage(imgOrigImage, new Rectangle(0, 0, nNewWidth, nNewHeight), new Rectangle(0, 0, imgOrigImage.Width, imgOrigImage.Height), GraphicsUnit.Pixel);
            gTmpGraphics.Dispose();


            return bmpTmpBitmap;
        }

        /// <summary>
        /// 给ImageResize使用的私有方法，得到一个图形的编码方法
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
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
        /// 获取一个特定图片的大小信息
        /// </summary>
        /// <param name="strImgPath">输入图片的绝对路径（服务器路径）</param>
        /// <param name="nWidth">图片的宽度</param>
        /// <param name="nHeight">图片的高度</param>
        /// <returns>返回false表示错误，无法获取图片大小</returns>
        public static bool GetImageSize(String strImgPath, out int nWidth, out int nHeight)
        {
            nWidth = nHeight = 0;

            System.Drawing.Image imgNewImage = GetImgObjectFromFile(strImgPath);

            bool rt = GetImageSize(imgNewImage, out nWidth, out nHeight);

            if (imgNewImage != null)
                imgNewImage.Dispose();

            return rt;

        }

        /// <summary>
        /// 获取一个特定图片的大小信息
        /// </summary>
        /// <param name="strImgPath">输入的图片对象</param>
        /// <param name="nWidth">图片的宽度</param>
        /// <param name="nHeight">图片的高度</param>
        /// <returns>返回false表示错误，无法获取图片大小</returns>
        public static bool GetImageSize(System.Drawing.Image imgImage, out int nWidth, out int nHeight)
        {
            nWidth = nHeight = 0;  // out类型的必须赋值

            if (imgImage == null)
                return false;

            nWidth = imgImage.Width;
            nHeight = imgImage.Height;

            return true;
        }

        /// <summary>
        /// 加水印，对传入的Image对象操作，重载
        /// </summary>
        /// <param name="logo_num">水印图片的编号</param>
        /// <param name="logo_pos">水印图片的位置，0：中间，1：左上，2：右上，3：右下，4：左下</param>
        /// <param name="imgPath">要加水印的图片路径，直接在上面加水印</param>
        public static void AddWaterMark(string strOrigImgPath, int logo_num, int logo_pos)
        {
            System.Drawing.Image imgNewImage = GetImgObjectFromFile(strOrigImgPath);

            Bitmap bmpTmpBitmap = new Bitmap(imgNewImage);

            AddWaterMark(ref bmpTmpBitmap, logo_num, logo_pos);

            ImageFormat fmtImageFormat = imgNewImage.RawFormat;

            ImageCodecInfo myImageCodecInfo = GetEncoder(fmtImageFormat);
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            //myImageCodecInfo = ImageCodecInfo.GetImageEncoders()[0];
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, 95L); // 0-100
            myEncoderParameters.Param[0] = myEncoderParameter;



            //CSharpFilters.BitmapFilter.Sharpen(bmpTmpBitmap, 16);
            //bmpTmpBitmap = CSharpFilters.ColorSpace.Saturation(bmpTmpBitmap, (float)1.05);

            imgNewImage.Dispose();

            //保存新的图
            bmpTmpBitmap.Save(strOrigImgPath, myImageCodecInfo, myEncoderParameters);


            bmpTmpBitmap.Dispose();

        }

        /// <summary>
        /// 加水印，对传入的Image对象操作
        /// </summary>
        /// <param name="logo_num">水印图片的编号</param>
        /// <param name="logo_pos">水印图片的位置，0：中间，1：左上，2：右上，3：右下，4：左下</param>
        /// <param name="imgPhoto">要加水印的Bitmap对象，直接在上面加水印(ref 引用传递)</param>
        public static void AddWaterMark(ref Bitmap imgPhoto, int logo_num, int logo_pos)
        {
            string zu_logo_root = System.Web.HttpContext.Current.Server.MapPath("~/ERP/images/watermark/");

            string[] waterMarkImgs = new string[2] { zu_logo_root + "black_logo.gif",
                                                     zu_logo_root + "white_logo.gif" 
                                                    };

            string watermarkFullPath = waterMarkImgs[logo_num];

            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //用水印BMP文件创建一个image对象
            System.Drawing.Image imgWatermarkRow = new System.Drawing.Bitmap(watermarkFullPath);

            //将水印文件缩放到原始文件的1/3大
            System.Drawing.Image imgWatermark = ResizeImage(imgWatermarkRow, phWidth / 3, phHeight / 3);



            int pos_x_offset, pos_y_offset;  //水印的坐标

            switch (logo_pos)
            {
                case 1: //左上
                    pos_x_offset = phWidth / 8;
                    pos_y_offset = phHeight / 8;
                    break;
                case 2: //右上
                    pos_x_offset = phWidth * 7 / 12;
                    pos_y_offset = phHeight / 8;
                    break;
                case 3: //右下
                    pos_x_offset = phWidth * 7 / 12;
                    pos_y_offset = phHeight * 7 / 11;
                    break;
                case 4: //右上
                    pos_x_offset = phWidth / 8;
                    pos_y_offset = phHeight * 7 / 11;
                    break;
                default:
                    pos_x_offset = phWidth / 3;
                    pos_y_offset = phHeight / 3;
                    break;
            }




            //创建一个与原图尺寸相同的位图
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //位图装载到一个Graphics对象
            Graphics grPhoto = Graphics.FromImage(bmPhoto);


            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;

            //设置图片质量
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //以原始尺寸把照片图像画到此graphics对象
            grPhoto.DrawImage(
            imgPhoto, // Photo Image object
            new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
            0, // x-coordinate of the portion of the source image to draw.
            0, // y-coordinate of the portion of the source image to draw.
            phWidth, // Width of the portion of the source image to draw.
            phHeight, // Height of the portion of the source image to draw.
            GraphicsUnit.Pixel); // Units of measure

            //基于前面已修改的Bitmap创建一个新Bitmap
            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            //Load this Bitmap into a new Graphic Object
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //To achieve a transulcent watermark we will apply (2) color
            //manipulations by defineing a ImageAttributes object and
            //seting (2) of its properties.
            ImageAttributes imageAttributes = new ImageAttributes();

            //第一步是以透明色(Alpha=0, R=0, G=0, B=0)来替换背景色
            //为此我们将使用一个Colormap并用它来定义一个RemapTable
            ColorMap colorMap = new ColorMap();

            //水印被定义为一个100%的绿色背景l
            //这将是我们以transparency来查找并替换的颜色
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            //第二个颜色操作是用来改变水印的透明度
            //用包涵the coordinates for the RGBA space的一个5x5 的矩阵
            //设置第三行第三列to 0.3f we achive a level of opacity
            float[][] colorMatrixElements = {
                    new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f}, 
                    new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f}, 
                    new float[] {0.0f, 0.0f, 1.8f, 0.0f, 0.0f}, 
                    new float[] {0.0f, 0.0f, 0.0f, 0.3f, 0.0f}, 
                    new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}};
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
            ColorAdjustType.Bitmap);


            grWatermark.DrawImage(imgWatermark,
            new Rectangle(pos_x_offset, pos_y_offset, wmWidth, wmHeight), //Set the detination Position
            0, // 源图的横坐标位置
            0, // 源图的纵坐标位置
            wmWidth, // 水印宽度
            wmHeight, // 水印高度
            GraphicsUnit.Pixel, // Unit of measurment
            imageAttributes); //ImageAttributes Object

            //以新图替换原始图
            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();
            imgWatermark.Dispose();
        }

        /// <summary>  
        /// 剪裁 -- 用GDI+   
        /// </summary>  
        /// <param name="b">原始Bitmap</param>  
        /// <param name="StartX">开始坐标X</param>  
        /// <param name="StartY">开始坐标Y</param>  
        /// <param name="iWidth">宽度</param>  
        /// <param name="iHeight">高度</param>  
        /// <returns>剪裁后的Bitmap</returns>  
        public static Bitmap Cut(Bitmap b, int StartX, int StartY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }
            int w = b.Width;
            int h = b.Height;
            if (StartX >= w || StartY >= h)
            {
                return null;
            }
            if (StartX + iWidth > w)
            {
                iWidth = w - StartX;
            }
            if (StartY + iHeight > h)
            {
                iHeight = h - StartY;
            }
            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }
    }
}
