using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Common
{
    public static class ImageHelper
    {
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

        public static bool ZoomBigPic(string strOldPic, string strNewPic, int inputWidth, int inputHeight)
        {
            Bitmap objPic, objNewPic, outBitMap;
            try
            {
                //                objPic = new Bitmap(strOldPic);
                //
                //                int imageWidth = objPic.Width;
                //                int imageHeight = objPic.Height;

                objPic = new Bitmap(strOldPic);
                objNewPic = new Bitmap(objPic, inputWidth, inputHeight);
                objNewPic.Save(strNewPic);

                //                //缩放好的图片
                //                objNewPic = new Bitmap(objPic, imageWidth, imageHeight);
                //                //放到图片的中心位置
                //                outBitMap = new Bitmap(inputWidth, inputHeight);//输出的图片
                //
                //                Graphics grap = Graphics.FromImage(outBitMap);
                //                grap.Clear(Color.White);
                //                grap.CompositingQuality = CompositingQuality.HighSpeed;
                //
                //                int newPositonX = (inputWidth - imageWidth) / 2;
                //                int newPositonY = (inputHeight - imageHeight) / 2;
                //
                //                grap.DrawImage(objNewPic, newPositonX, newPositonY);
                //
                //                //检测目录是否存在
                //                var saveDir = Path.GetDirectoryName(strNewPic);
                //
                //                if (saveDir != null && !Directory.Exists(saveDir))
                //                {
                //                    Directory.CreateDirectory(saveDir);
                //                }
                //
                //                string mergeImageExt = Path.GetExtension(strNewPic);
                //                if (mergeImageExt != null && mergeImageExt.Contains("png"))
                //                    outBitMap.Save(strNewPic, ImageFormat.Png);
                //                else if (mergeImageExt != null && mergeImageExt.Contains("gif"))
                //                    outBitMap.Save(strNewPic, ImageFormat.Gif);
                //                else
                //                    outBitMap.Save(strNewPic, ImageFormat.Jpeg);



                //grap.Dispose();

                objPic.Dispose();
                objNewPic.Dispose();
                // outBitMap.Dispose();

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
    }
}
