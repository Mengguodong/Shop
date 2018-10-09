using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace SFO2O.Utility.Uitl
{
    public static class ZImage
    {
        private static string IMG_ROOT_URL = "/img/";

        public static string IMG_ROOT_PATH = GetImgRootPath();

        /// <summary>
        /// 获得图片根目录
        /// </summary>
        /// <returns></returns>
        private static string GetImgRootPath()
        {
            try
            {
                return ZWeb.GetLocalMappedFilePath(IMG_ROOT_URL);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 转换一下图片格式
        /// </summary>
        /// <param name="orig_img_path"></param>
        /// <param name="dest_img_path"></param>
        /// <param name="src_fmt"></param>
        /// <param name="dest_fmt"></param>
        public static void ConvertImage(string orig_img_path, string dest_img_path, ImageFormat dest_fmt, int quality_num)
        {
            Bitmap b_orig = new Bitmap(orig_img_path);

            ImageCodecInfo myImageCodecInfo = ZImage.GetEncoder(dest_fmt);
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, (long)quality_num); // 0-100 
            myEncoderParameters.Param[0] = myEncoderParameter;

            b_orig.Save(dest_img_path, myImageCodecInfo, myEncoderParameters);
            b_orig.Dispose();
        }
        /// <summary>
        /// 2014-04-29 增加红蓝3D效果
        /// </summary>
        /// <param name="img_path"></param>
        /// <param name="b"></param>
        /// <param name="img_format"></param>
        /// <param name="quality_num"></param>
        public static void SaveImage(string img_path, Bitmap b, System.Drawing.Imaging.ImageFormat img_format, int quality_num)
        {
            ImageCodecInfo myImageCodecInfo = ZImage.GetEncoder(img_format);
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, (long)quality_num);//quality_num + "L"); // 0-100 
            myEncoderParameters.Param[0] = myEncoderParameter;

            b.Save(img_path, myImageCodecInfo, myEncoderParameters);
        }

        /// <summary>
        /// 从左上角裁剪一块
        /// </summary>
        /// <param name="img_path"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static Bitmap CropImage(string img_path, int width, int height)
        {
            Bitmap b = new Bitmap(img_path);
            if (b.Width <= width && b.Height <= height)
                return b;

            width = b.Width < width ? b.Width : width;
            height = b.Height < height ? b.Height : height;
            Rectangle rec = new Rectangle(0, 0, width, height);
            Bitmap new_b = b.Clone(rec, PixelFormat.Format24bppRgb);
            b.Dispose();
            return new_b;

        }

        /// <summary>
        /// 将一个以数组和长宽表示的图片中的所有颜色做成一个数组,offset表示rgb中的某一个，我也不知道哪个是哪个，自己试吧
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte[] GetImgColorArray(byte[] b, int width, int height, int mask_step)
        {

            List<byte> color_array = new List<byte>();

            int x_step = (int)Math.Floor((double)width / (double)mask_step);
            int y_step = (int)Math.Floor((double)height / (double)mask_step);

            for (int y = 0; y < y_step; y++)
            {
                int line_start = y * mask_step * width;

                for (int x = 0; x < x_step; x++)
                {
                    int[] byte_count = new int[256];
                    int max_count = 0;
                    byte max_byte = 0;

                    int block_start = line_start + x * mask_step;

                    for (int i = 0; i < mask_step; i++)
                    {
                        int block_line_start = block_start + i * width;

                        for (int j = 0; j < mask_step; j++)
                        {
                            byte this_code = (byte)b[block_line_start + j];

                            byte_count[this_code]++;
                            if (byte_count[this_code] > max_count)
                            {
                                max_byte = this_code;
                                max_count = byte_count[this_code];
                            }
                        }
                    }

                    color_array.Add(max_byte);
                }
            }

            return color_array.ToArray();
        }



        /// <summary>
        /// 将一个图片中的所有颜色做成一个数组,offset表示rgb中的某一个，我也不知道哪个是哪个，自己试吧
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte[] GetImgColorArray(Bitmap b, int offset, int mask_step)
        {

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            List<byte> color_array = new List<byte>();

            int x_step = (int)Math.Floor((double)b.Width / (double)mask_step);
            int y_step = (int)Math.Floor((double)b.Height / (double)mask_step);

            unsafe
            {
                byte* bm_scan = (byte*)bmData.Scan0;
                int bm_stride = bmData.Stride;

                for (int y = 0; y < y_step; y++)
                {
                    for (int x = 0; x < x_step; x++)
                    {
                        int[] byte_count = new int[256];
                        int max_count = 0;
                        byte max_byte = 0;

                        for (int i = 0; i < mask_step; i++)
                        {
                            byte* p = bm_scan + (y * mask_step + i) * bm_stride;

                            for (int j = 0; j < mask_step; j++)
                            {
                                byte this_code = (byte)p[3 * x * mask_step + j + offset];

                                byte_count[this_code]++;
                                if (byte_count[this_code] > max_count)
                                {
                                    max_byte = this_code;
                                    max_count = byte_count[this_code];
                                }
                            }
                        }

                        color_array.Add(max_byte);
                    }
                }
            }


            b.UnlockBits(bmData);

            return color_array.ToArray();
        }


        /// <summary>
        /// 根据一个一维数组图中获取某个颜色的子集并放在同一个bitmap里
        /// </summary>
        /// <param name="b"></param>
        /// <param name="offset"></param>
        /// <param name="color_code"></param>
        /// <returns></returns>
        public static Bitmap ExtractColorLayer(byte[] b_input, int width, int height, byte match_code, Color forground_color, Color background_color)
        {
            int scan_height = height;
            int scan_width = width;

            Bitmap b_output = new Bitmap(scan_width, scan_height, PixelFormat.Format32bppArgb);
            var bitsOutput = b_output.LockBits(new Rectangle(0, 0, scan_width, scan_height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* output_scan = (byte*)bitsOutput.Scan0;
                int output_stride = bitsOutput.Stride;

                byte background_b = background_color.B;
                byte background_g = background_color.G;
                byte background_r = background_color.R;
                byte background_a = background_color.A;

                byte forground_b = forground_color.B;
                byte forground_g = forground_color.G;
                byte forground_r = forground_color.R;
                byte forground_a = forground_color.A;

                for (int y = 0; y < scan_height; y++)
                {
                    byte* ptrOutput = output_scan + y * output_stride;
                    int line_start = y * width;

                    for (int x = 0; x < scan_width; x++)
                    {
                        if (b_input[line_start + x] == match_code)
                        {
                            ptrOutput[4 * x + 0] = forground_b;     // blue
                            ptrOutput[4 * x + 1] = forground_g;     // green
                            ptrOutput[4 * x + 2] = forground_r;     // red
                            ptrOutput[4 * x + 3] = forground_a;
                            ;   // alpha
                        }
                        else
                        {
                            ptrOutput[4 * x + 0] = background_b;     // blue
                            ptrOutput[4 * x + 1] = background_g;     // green
                            ptrOutput[4 * x + 2] = background_r;     // red
                            ptrOutput[4 * x + 3] = background_a;
                            ;   // alpha
                        }
                    }
                }
            }

            b_output.UnlockBits(bitsOutput);

            return b_output;
        }



        /// <summary>
        /// 从一张图中获取某个颜色的子集并放在同一个bitmap里
        /// </summary>
        /// <param name="b"></param>
        /// <param name="offset"></param>
        /// <param name="color_code"></param>
        /// <returns></returns>
        public static Bitmap ExtractColorLayer(Bitmap b_input, int offset, byte match_code, Color forground_color, Color background_color)
        {
            int scan_height = b_input.Height;
            int scan_width = b_input.Width;

            Bitmap b_output = new Bitmap(scan_width, scan_height, PixelFormat.Format32bppArgb);

            var bitsInput = b_input.LockBits(new Rectangle(0, 0, scan_width, scan_height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            var bitsOutput = b_output.LockBits(new Rectangle(0, 0, scan_width, scan_height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            unsafe
            {
                byte* input_scan = (byte*)bitsInput.Scan0;
                byte* output_scan = (byte*)bitsOutput.Scan0;
                int input_stride = bitsInput.Stride;
                int output_stride = bitsOutput.Stride;

                for (int y = 0; y < scan_height; y++)
                {
                    byte* ptrInput = input_scan + y * input_stride;
                    byte* ptrOutput = output_scan + y * output_stride;

                    for (int x = 0; x < scan_width; x++)
                    {
                        Color fill_color = Color.Transparent;
                        if (ptrInput[3 * x + offset] == match_code)
                            fill_color = forground_color;
                        else
                            fill_color = background_color;

                        ptrOutput[4 * x + 0] = fill_color.B;   // blue
                        ptrOutput[4 * x + 1] = fill_color.G;   // green
                        ptrOutput[4 * x + 2] = fill_color.R;   // red
                        ptrOutput[4 * x + 3] = fill_color.A;   // red
                    }
                }
            }

            b_input.UnlockBits(bitsInput);
            b_output.UnlockBits(bitsOutput);

            return b_output;
        }




        /// <summary>
        /// 将图片保留到系统的图片库，按照年/月/日/图片名的方式保存，如果目录不存在就创建目录
        /// 返回新的图片的URL，考虑到这个目录可能在NFS目录上，如果需要将图片进行处理，比如修改大小
        /// 或者锐化，需要在本地做好之后再保存到图库里。
        /// 这个可以用来保存swf文件等
        /// </summary>
        /// <param name="img_orig_path"></param>
        /// <returns></returns>
        public static string SaveNewImageToRepository(string img_path)
        {

            string repos_dir = MakeCurrentRepositoryDir(DateTime.Now); //获取/img/2011/03/02/这样的目录

            string target_dir = System.Web.HttpContext.Current.Server.MapPath(repos_dir); ///获取这个目录的实际目录

            string new_img_name = ZRandom.GetRandomString(9); //生成一个随机文件名

            string orig_img_ext = Path.GetExtension(img_path); //文件后缀名

            string new_file_path = target_dir + new_img_name + orig_img_ext;

            string new_file_url = repos_dir + new_img_name + orig_img_ext; ///返回新的路径名

            File.Move(img_path, new_file_path);

            return new_file_url;
        }


        /// <summary>
        /// 得到某个时间的目录，如果成功，返回该目录的URL，如果失败，返回null
        /// </summary>
        /// <param name="repository_time"></param>
        /// <returns></returns>
        private static string MakeCurrentRepositoryDir(DateTime repository_time)
        {
            string this_year_str = repository_time.Year.ToString("D4");
            string this_month_str = repository_time.Month.ToString("D2");
            string this_day_str = repository_time.Day.ToString("D2");

            string year_dir = IMG_ROOT_PATH + this_year_str + "\\";
            string month_dir = year_dir + this_month_str + "\\";
            string day_dir = month_dir + this_day_str + "\\";

            try
            {
                if (!Directory.Exists(year_dir))                  //这里将来要加一些容错
                    Directory.CreateDirectory(year_dir);

                if (!Directory.Exists(month_dir))
                    Directory.CreateDirectory(month_dir);

                if (!Directory.Exists(day_dir))
                    Directory.CreateDirectory(day_dir);
            }
            catch
            {
                return null;
            }

            return IMG_ROOT_URL + this_year_str + "/" + this_month_str + "/" + this_day_str + "/";
        }



        /// <summary>
        /// 以只读模式从磁盘上读取一个文件到实体类里
        /// 如果文件不存在或者有其他错误，返回null
        /// </summary>
        /// <param name="img_path"></param>
        /// <returns></returns>
        public static Bitmap LoadImage(string img_path)
        {
            try
            {
                return new Bitmap(img_path);
            }
            catch
            {
                return null;
            }

        }


        /// <summary>
        /// 按照某种规格载入一个图片，就是又载入又缩放
        /// </summary>
        /// <param name="strOrigImgPath">输入图片的路径</param>
        /// <param name="nNewImgPath">目标图片的路径</param>
        /// <param name="nNewWidth">新的图片宽度</param>
        /// <param name="nNewHeight">系的图片高度</param>
        /// <returns>Bitmap对象，或者null表示失败</returns>
        public static Bitmap LoadImage(string file_path, int nNewWidth, int nNewHeight)
        {
            Bitmap b = LoadImage(file_path);
            if (b == null)
                return null;

            return ResizeImage(b, nNewWidth, nNewHeight);

        }



        /// <summary>
        /// 改变一个图片的大小
        /// </summary>
        /// <param name="b"></param>
        /// <param name="nNewWidth"></param>
        /// <param name="nNewHeight"></param>
        /// <returns></returns>
        public static Bitmap ResizeImage(Bitmap b, int nNewWidth, int nNewHeight)
        {
            ImageFormat fmtImageFormat = b.RawFormat;

            ImageCodecInfo myImageCodecInfo = GetEncoder(fmtImageFormat);
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;
            //myImageCodecInfo = ImageCodecInfo.GetImageEncoders()[0];
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, 95L); // 0-100
            myEncoderParameters.Param[0] = myEncoderParameter;

            int nWidth = 0;
            int nHeight = 0;

            if (b.Width < nNewWidth && b.Height < nNewHeight)
            {
                nWidth = b.Width;
                nHeight = b.Height;
            }
            else
            {
                if ((double)b.Width / (double)b.Height < (float)nNewWidth / (float)nNewHeight)
                {
                    nWidth = (int)Math.Round(nNewHeight * ((double)b.Width / (double)b.Height));
                    nHeight = nNewHeight;
                }
                else
                {
                    nWidth = nNewWidth;
                    nHeight = (int)Math.Round(nNewWidth * ((double)b.Height / (double)b.Width));
                }
            }

            Bitmap b2 = new Bitmap(nWidth, nHeight);
            Graphics gTmpGraphics = Graphics.FromImage(b2);

            // 插值算法的质量
            gTmpGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            gTmpGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            //设置高质量,低速度呈现平滑程度
            gTmpGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            gTmpGraphics.DrawImage(b, new Rectangle(0, 0, nWidth, nHeight), new Rectangle(0, 0, b.Width, b.Height), GraphicsUnit.Pixel);
            gTmpGraphics.Dispose();
            b.Dispose();


            return b2;
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
        /// 根据一个一维数组图中获取某个颜色的子集并放在同一个bitmap里
        /// </summary>
        /// <param name="b"></param>
        /// <param name="offset"></param>
        /// <param name="color_code"></param>
        /// <returns></returns>
        public static Bitmap FilterImageColor(Bitmap b_input, Color filter_color, Color replace_color, double tolarence)
        {
            int scan_height = b_input.Height;
            int scan_width = b_input.Width;
            Bitmap b_output = new Bitmap(b_input.Width, b_input.Height, PixelFormat.Format32bppArgb);
            var bitsOutput = b_output.LockBits(new Rectangle(0, 0, b_input.Width, b_input.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            var bitsInput = b_input.LockBits(new Rectangle(0, 0, b_input.Width, b_input.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            byte filter_b = filter_color.B;
            byte filter_g = filter_color.G;
            byte filter_r = filter_color.R;

            byte replace_b = replace_color.B;
            byte replace_g = replace_color.G;
            byte replace_r = replace_color.R;
            byte replace_a = replace_color.A;

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
            b_input.UnlockBits(bitsInput);

            return b_output;
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
        /// 获取一个特定图片的大小信息
        /// </summary>
        /// <param name="strImgPath">输入图片的绝对路径（服务器路径）</param>
        /// <param name="nWidth">图片的宽度</param>
        /// <param name="nHeight">图片的高度</param>
        /// <returns>返回false表示错误，无法获取图片大小</returns>
        public static bool GetImageSize(String strImgPath, ref int nWidth, ref int nHeight)
        {
            nWidth = nHeight = 0;

            Bitmap b = LoadImage(strImgPath);
            if (b == null)
                return false;

            nWidth = b.Width;
            nHeight = b.Height;

            b.Dispose();

            return true;

        }


        /// <summary>
        /// 锐化一个图片，锐度20左右可以看到明显效果
        /// </summary>
        /// <param name="b"></param>
        /// <param name="sharpen_level"></param>
        /// <returns></returns>
        //public static void SharpenImage(Bitmap b, int sharpen_level)
        //{
        //CSharpFilters.BitmapFilter.Sharpen(b, sharpen_level);
        //}


        /// <summary>
        /// 缺省是不缓冲
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="b"></param>
        /// <param name="img_format"></param>
        /// <param name="freshness"></param>
        public static void OutPutImage(System.Web.HttpResponse Response, Bitmap b, System.Drawing.Imaging.ImageFormat img_format)
        {
            OutPutImage(Response, b, img_format, null);
        }


        /// <summary>
        /// 缺省是不缓冲
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="b"></param>
        /// <param name="img_format"></param>
        /// <param name="freshness"></param>
        public static void OutPutImage(System.Web.HttpResponse Response, Bitmap b, System.Drawing.Imaging.ImageFormat img_format, TimeSpan? freshness)
        {
            OutPutImage(Response, b, img_format, null, 92);
        }


        /// <summary>
        /// 输出一个图片到输出流
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="b"></param>
        /// <param name="img_format"></param>
        public static void OutPutImage(System.Web.HttpResponse Response, Bitmap b, System.Drawing.Imaging.ImageFormat img_format, TimeSpan? freshness, int quality_num)
        {
            Response.Clear();
            Response.BufferOutput = false;      //提高效率

            if (freshness.HasValue) //要缓冲
            {
                DateTime now = DateTime.Now;
                Response.Cache.SetExpires(now.Add((TimeSpan)freshness));
                Response.Cache.SetMaxAge((TimeSpan)freshness);
                Response.Cache.SetCacheability(HttpCacheability.Public);
                Response.Cache.SetValidUntilExpires(true);
            }
            else
            {
                Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));//特别注意
                Response.Cache.SetCacheability(HttpCacheability.NoCache);//特别注意
                Response.AppendHeader("  Pragma", "No-Cache"); //特别注意
            }

            ImageCodecInfo myImageCodecInfo = ZImage.GetEncoder(img_format);
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, (long)quality_num);//quality_num + "L"); // 0-100 
            myEncoderParameters.Param[0] = myEncoderParameter;

            Response.ContentType = GetContentTypeFromImageFormat(img_format);

            b.Save(Response.OutputStream, myImageCodecInfo, myEncoderParameters);

            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }

        #region DrawBackgroundImg 根据一个一维数组图中获取某个颜色的子集并放在同一个bitmap里
        /// <summary>
        /// 根据一个一维数组图中获取某个颜色的子集并放在同一个bitmap里
        /// </summary>
        /// <param name="b"></param>
        /// <param name="offset"></param>
        /// <param name="color_code"></param>
        /// <returns></returns>
        public static Bitmap DrawBackgroundImg(int width, int height, Color background_color)
        {
            return DrawBackgroundImg(width, height, background_color, 0.6);
        }


        /// <summary>
        /// 根据一个一维数组图中获取某个颜色的子集并放在同一个bitmap里
        /// </summary>
        /// <param name="b"></param>
        /// <param name="offset"></param>
        /// <param name="color_code"></param>
        /// <returns></returns>
        public static Bitmap DrawBackgroundImg(int width, int height, Color background_color, double max_ratio)
        {
            int scan_height = height;
            int scan_width = width;
            int center_x = width / 2;
            int center_y = height / 2;
            double radis = Math.Sqrt(width * width + height * height) / 2;

            byte max_b = (byte)(background_color.B + (255 - background_color.B) * max_ratio);
            byte max_g = (byte)(background_color.G + (255 - background_color.G) * max_ratio);
            byte max_r = (byte)(background_color.R + (255 - background_color.R) * max_ratio);


            Bitmap b_output = new Bitmap(scan_width, scan_height, PixelFormat.Format24bppRgb);
            var bitsOutput = b_output.LockBits(new Rectangle(0, 0, scan_width, scan_height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* output_scan = (byte*)bitsOutput.Scan0;
                int output_stride = bitsOutput.Stride;

                byte background_b = background_color.B;
                byte background_g = background_color.G;
                byte background_r = background_color.R;

                for (int y = 0; y < scan_height; y++)
                {
                    byte* ptrOutput = output_scan + y * output_stride;
                    int line_start = y * width;

                    for (int x = 0; x < scan_width; x++)
                    {
                        //int x_pos = Math.Abs(x - center_x);
                        //int y_pos = Math.Abs(y - center_y);

                        if ((background_b + background_g + background_r) == 0)
                        {
                            ptrOutput[3 * x + 0] = 0;
                            ptrOutput[3 * x + 1] = 0;
                            ptrOutput[3 * x + 2] = 0;
                            ptrOutput[3 * x + 3] = 0;
                            continue;
                        }

                        double this_radis = Math.Sqrt((x - center_x) * (x - center_x) + (y - center_y) * (y - center_y));

                        double ratio = this_radis / radis;
                        if (ratio > 1) ratio = 1;

                        ptrOutput[3 * x + 0] = (byte)(max_b - (max_b - background_b) * ratio);     // blue
                        ptrOutput[3 * x + 1] = (byte)(max_g - (max_g - background_g) * ratio);     // green
                        ptrOutput[3 * x + 2] = (byte)(max_r - (max_r - background_r) * ratio);     // red
                    }
                }
            }

            b_output.UnlockBits(bitsOutput);

            return b_output;
        }
        #endregion
        /// <summary>
        /// 根据图片格式返回图片的content_type
        /// </summary>
        /// <param name="img_format"></param>
        /// <returns></returns>
        private static string GetContentTypeFromImageFormat(ImageFormat img_format)
        {
            if (img_format == ImageFormat.Jpeg)
                return "image/jpeg";
            else if (img_format == ImageFormat.Gif)
                return "image/gif";
            else if (img_format == ImageFormat.Png)
                return "image/png";
            else
                return "image/jpeg";

        }


    }
}
