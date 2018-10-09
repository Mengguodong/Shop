using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web.Mvc;

namespace SFO2O.Framework.Uitl
{

    /// <summary>   
    /// 生成验证码的类   
    /// </summary>   
    public class ValidateCodeHelper
    {
        #region  数字验证码
        public ValidateCodeHelper()
        {
        }
        /// <summary>   
        /// 验证码的最大长度   
        /// </summary>   
        public int MaxLength
        {
            get { return 10; }
        }
        /// <summary>   
        /// 验证码的最小长度   
        /// </summary>   
        public int MinLength
        {
            get { return 1; }
        }
        /// <summary>   
        /// 生成验证码   
        /// </summary>   
        /// <param name="length">指定验证码的长度</param>   
        /// <returns></returns>   
        public string CreateValidateCode(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值   
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字   
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字   
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码   
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }
        /// <summary>   
        /// 创建验证码的图片   
        /// </summary>   
        /// <param name="containsPage">要输出到的page对象</param>   
        /// <param name="validateNum">验证码</param>   
        public byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器   
                Random random = new Random();
                //清空图片背景色   
                g.Clear(Color.White);
                //画图片的干扰线   
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点   
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线   
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据   
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流   
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>   
        /// 得到验证码图片的长度   
        /// </summary>   
        /// <param name="validateNumLength">验证码的长度</param>   
        /// <returns></returns>   
        public static int GetImageWidth(int validateNumLength)
        {
            return (int)(validateNumLength * 12.0);
        }
        /// <summary>   
        /// 得到验证码的高度   
        /// </summary>   
        /// <returns></returns>   
        public static double GetImageHeight()
        {
            return 22.5;
        }
        #endregion

        #region 数字加字母

        /// <summary>
        /// 生成数字加字母验证码
        /// </summary>
        /// <param name="Numberlength">验证码长度</param>
        /// <returns></returns>
        [AllowAnonymous]
        public string MakeVerifyCode(int Numberlength)
        {
            string strTemp = "";
            string randomchars = "abcdefghjkmnpqrstuvwxyz23456789";
            randomchars = randomchars.ToUpper();
            int iRandNum;
            Random rnd = new Random();
            for (int i = 0; i < Numberlength; i++)
            {
                iRandNum = rnd.Next(randomchars.Length);
                strTemp += randomchars[iRandNum];
            }
            return strTemp;
        }
        public byte[] CreatePicCode(string VNum)
        {
            int gWidth = (int)(VNum.Length * 17);// Gwidth为图片宽度,根据字符长度自动更改图片宽度
            int gHeight = 20;
            System.Drawing.Bitmap Img = new System.Drawing.Bitmap(gWidth, gHeight);

            Graphics g;
            MemoryStream ms = new MemoryStream();

            string tmpstr = "";
            for (int mm = 0; mm < VNum.Length; mm++)
            {
                tmpstr += VNum[mm] + " ";
            }
            VNum = tmpstr;


            g = Graphics.FromImage(Img);

            SolidBrush drawBrushNew = new SolidBrush(Color.White);
            g.FillRectangle(drawBrushNew, 0, 0, Img.Width, Img.Height);

            // Create font and brush.	

            FontStyle style = FontStyle.Bold;
            //style |= FontStyle.Bold;
            Font drawFont = new Font("Arial", 12, style);

            SolidBrush drawBrush = new SolidBrush(Color.Red);


            Color clr = Color.Red;
            Pen p = new Pen(clr, StrDeal.LoginRandNum(1, 4));
            int x1 = StrDeal.LoginRandNum(1, gWidth);
            int y1 = StrDeal.LoginRandNum(1, gHeight);
            int x2 = x1 + StrDeal.LoginRandNum(15, gWidth - 15);
            int y2 = y1 + StrDeal.LoginRandNum(0, gHeight);
            PointF drawPoint1 = new PointF(x1, y1);
            PointF drawPoint2 = new PointF(x2, y2);

            for (int i = 0; i < Convert.ToInt32(StrDeal.LoginRandNum(300, 415)); i++)
            {
                // 生成一个随机宽度
                clr = Color.FromArgb(StrDeal.LoginRandNum(100, 255), StrDeal.LoginRandNum(51, 255), StrDeal.LoginRandNum(11, 255));
                p.Color = clr;
                p.Width = StrDeal.LoginRandNum(1, 4);

                x1 = StrDeal.LoginRandNum(1, gWidth);
                y1 = StrDeal.LoginRandNum(1, gHeight);
                x2 = x1 + StrDeal.LoginRandNum(15, gWidth - 15);
                y2 = y1 + StrDeal.LoginRandNum(0, gHeight);
                drawPoint1.X = x1;
                drawPoint1.Y = y1;
                drawPoint2.X = x2;
                drawPoint2.Y = y2;
                g.DrawLine(p, drawPoint1, drawPoint2);

            }
            p.Dispose();
            PointF drawPoint = new PointF(3, 3);
            g.DrawString(VNum, drawFont, drawBrush, drawPoint); //在矩形内绘制字串（字串，字体，画笔颜色，左上x.左上y）

            Img.Save(ms, ImageFormat.Jpeg);

            return ms.ToArray();


        }
        public class StrDeal
        {
            public StrDeal()
            {
                //
                // TODO: 在此处添加构造函数逻辑
                //
            }

            /// <summary>
            ///     将指定的字符串中危险字符替换，返回已处理完的字符串。
            ///     <remarks>主要处理SQL语句中危险字符</remarks> 
            ///     <param name="strContent">要处理的字符串。</param>
            /// </summary>
            public static String FilterSql(object strContent)
            {
                if (strContent == null)
                {
                    return "";
                }
                if (strContent.ToString() == "")
                {
                    return "";
                }
                else
                {
                    string rStr = "";
                    rStr = ((string)strContent).Trim().Replace("'", "‘");


                    // 处理xml的保留字符
                    //				rStr = rStr.Replace("<","〈");
                    //				rStr = rStr.Replace(">","〉");
                    //
                    //				rStr = rStr.Replace("\"","〝");
                    //				rStr = rStr.Replace("&","﹠");

                    return rStr;
                }
            }


            public static string[] StringSplit(string strSource, string strSplit)
            {
                string[] textArray1 = new string[1];
                int num1 = strSource.IndexOf(strSplit, 0);
                if (num1 < 0)
                {
                    textArray1[0] = strSource;
                    return textArray1;
                }
                textArray1[0] = strSource.Substring(0, num1);
                return StrDeal.StringSplit(strSource.Substring(num1 + strSplit.Length), strSplit, textArray1);
            }

            public static string[] StringSplit(string strSource, string strSplit, string[] attachArray)
            {
                string[] textArray1 = new string[attachArray.Length + 1];
                attachArray.CopyTo(textArray1, 0);
                int num1 = strSource.IndexOf(strSplit, 0);
                if (num1 < 0)
                {
                    textArray1[attachArray.Length] = strSource;
                    return textArray1;
                }
                textArray1[attachArray.Length] = strSource.Substring(0, num1);
                return StrDeal.StringSplit(strSource.Substring(num1 + strSplit.Length), strSplit, textArray1);


            }


            /// <summary>
            ///     判断获得的字符是否能转换为数字
            ///     <remarks></remarks> 
            ///     <param name="strContent">要处理的字符串。</param>
            /// </summary>
            public static bool IsNum(string strContent)
            {
                try
                {
                    Decimal.Parse(strContent);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            /// <summary>
            ///     判断获得的字符是否能转换为数字
            ///     <remarks></remarks> 
            ///     <param name="strContent">要处理的字符串。</param>
            /// </summary>
            public static bool IsInt(string strContent)
            {
                try
                {
                    int.Parse(strContent);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            ///     返回一个登录随机数
            ///     <remarks></remarks> 
            ///     <param name="minValue">随机数下限。</param>
            ///     <param name="maxValue">随机数上限。</param>
            /// </summary>
            public static int LoginRandNum(int minValue, int maxValue)
            {
                try
                {

                    Random ro = new Random();
                    return ro.Next(minValue, maxValue);

                }
                catch
                {
                    return 8888;
                }
            }

            //格式化一个单号
            //by wmj
            public string formatBillId(string formatStr, int MaxlengthOfNum, string billValue)
            {
                if (!(formatStr == "yyyyMMdd") | (formatStr == "yyyyMM") | (formatStr == "yyyy"))
                {
                    throw new Exception("无效的参数formatStr，formatBillId函数调用失败！");
                }

                return DateTime.Now.ToString(formatStr).Trim() + billValue.PadLeft(MaxlengthOfNum, '0').Trim();
            }
        }

        #endregion
    }
}