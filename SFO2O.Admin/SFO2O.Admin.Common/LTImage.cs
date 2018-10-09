using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Common
{
    public static class LTImage
    {
        public static Bitmap ResizeImage(System.Drawing.Image imgOrigImage, int nNewWidth, int nNewHeight)
        {
            return ResizeImage(imgOrigImage, nNewWidth, nNewHeight, true);
        }

        public static Bitmap ResizeImage(System.Drawing.Image imgOrigImage, int nNewWidth, int nNewHeight, bool isSetInterpolationMode, bool isHighQuality = true)
        {
            ImageFormat fmtImageFormat = imgOrigImage.RawFormat;

            int nWidth = 0;
            int nHeight = 0;


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


            Bitmap bmpTmpBitmap = new Bitmap(nWidth, nHeight);
            Graphics gTmpGraphics = Graphics.FromImage(bmpTmpBitmap);

            // 插值算法的质量
            if (isSetInterpolationMode)
                gTmpGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            if (isHighQuality)
                gTmpGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            else
                gTmpGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;

            //设置高质量,低速度呈现平滑程度
            if (isHighQuality)
                gTmpGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            else
                gTmpGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            gTmpGraphics.DrawImage(imgOrigImage, new Rectangle(0, 0, nWidth, nHeight), new Rectangle(0, 0, imgOrigImage.Width, imgOrigImage.Height), GraphicsUnit.Pixel);
            gTmpGraphics.Dispose();

            return bmpTmpBitmap;

        }

        public static Bitmap ResizeImage_(System.Drawing.Image imgOrigImage, int nNewWidth, int nNewHeight, bool isSetInterpolationMode, bool isHighQuality = true)
        {
            //var nWidth = imgOrigImage.Width < nNewWidth ? imgOrigImage.Width : nNewWidth;
            //var nHeight = imgOrigImage.Height < nNewHeight ? imgOrigImage.Height : nNewHeight;

            int towidth = nNewWidth;
            int toheight = nNewHeight;

            int x = 0; //缩略图在画布上的X放向起始点  
            int y = 0; //缩略图在画布上的Y放向起始点  
            int ow = imgOrigImage.Width;
            int oh = imgOrigImage.Height;
            int dw = 0;
            int dh = 0;

            if ((double)imgOrigImage.Width / (double)imgOrigImage.Height > (double)towidth / (double)toheight)
            {
                //宽比高大，以宽为准                  
                dw = imgOrigImage.Width * towidth / imgOrigImage.Width;
                dh = imgOrigImage.Height * toheight / imgOrigImage.Width;
                x = 0;
                y = (toheight - dh) / 2;
            }
            else
            {
                //高比宽大，以高为准  
                dw = imgOrigImage.Width * towidth / imgOrigImage.Height;
                dh = imgOrigImage.Height * toheight / imgOrigImage.Height;
                y = 0;
                if (imgOrigImage.Width == imgOrigImage.Height && imgOrigImage.Width <= towidth)
                {
                    dw = imgOrigImage.Width;
                    dh = imgOrigImage.Height;
                    y = (toheight - imgOrigImage.Height) / 2;
                }
                if (imgOrigImage.Width == imgOrigImage.Height && imgOrigImage.Width > towidth)
                {
                    dw = towidth;
                    dh = toheight;
                }
                x = (towidth - dw) / 2;

            }

            Bitmap bmpTmpBitmap = new Bitmap(imgOrigImage, nNewWidth, nNewHeight);
            Graphics gTmpGraphics = Graphics.FromImage(bmpTmpBitmap);

            // 插值算法的质量
            if (isSetInterpolationMode)
                gTmpGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            if (isHighQuality)
                gTmpGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            else
                gTmpGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;

            //设置高质量,低速度呈现平滑程度
            if (isHighQuality)
                gTmpGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            else
                gTmpGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            gTmpGraphics.Clear(Color.White);
            gTmpGraphics.DrawImage(imgOrigImage, new Rectangle(x, y, dw, dh), new Rectangle(0, 0, ow, oh), GraphicsUnit.Pixel);
            gTmpGraphics.Dispose();


            return bmpTmpBitmap;
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
