using System;
using System.Text;

namespace SFO2O.Framework.Uitl
{
    public static class StringHelper
    {
        public static string GetEmailUrl(this string mail)
        {
            var tmp = mail.ToLower();
            const string httpHead = "http://";
            const string mailPrefix = "mail.";
            switch (tmp)
            {
                case "gmail.com":
                case "hotmail.com":
                    return string.Concat(httpHead, mail);
                    break;
                default:
                    return string.Concat(httpHead, mailPrefix, mail);
            }
        }
        /// <summary>
        /// 不完全展示邮件内容
        /// </summary>
        /// <param name="email"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="replacer"></param>
        /// <returns></returns>
        public static string GetSafeEmailAddress(this string email, int startIndex, int length, char replacer = '*')
        {
            var prefix = "";
            var end = "";
            if (email.IndexOf('@') > -1)
            {
                prefix = email.Split('@')[0];
                end = email.Split('@')[1];
            }
            prefix = prefix.GetSafeString(startIndex, length, replacer);
            return string.Concat(prefix, end);
        }
        /// <summary>
        /// 展示安全字符串内容
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="replacer"></param>
        /// <returns></returns>
        public static string GetSafeString(this string str, int startIndex, int length, char replacer = '*')
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            int iend = startIndex + length;
            if (startIndex < str.Length - 1)
            {
                if (str.Length < iend)
                {
                    iend = str.Length;
                }
                var sb = new StringBuilder();
                string pre = str.Substring(0, startIndex - 0);
                string end = str.Substring(iend - 1);
                sb.Append(pre);
                for (int i = 0; i < length; i++)
                {
                    sb.Append(replacer);
                }
                sb.Append(end);
                return sb.ToString();
            }
            else
            {
                var sb = new StringBuilder();
                if (str.Length == 1)
                {
                    return str;
                }
                else
                {
                    sb.Append(str[0]);
                    for (int i = 1; i < str.Length; i++)
                    {
                        sb.Append(replacer);
                    }
                    return sb.ToString();

                }
            }
        }
        /// <summary>
        /// 获得字节长度 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int getLengthb(string str)
        {
            return System.Text.Encoding.Default.GetByteCount(str);
        }

        /// <summary>
        /// 中英文混合字符串截取指定长度  
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startidx"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string getStrLenB(string str, int startidx, int len)
        {
            int Lengthb = getLengthb(str);
            if (startidx + 1 > Lengthb)
            {
                return "";
            }
            int j = 0;
            int l = 0;
            int strw = 0;//字符的宽度
            bool b = false;
            string rstr = "";
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (j >= startidx)
                {
                    rstr = rstr + c;
                    b = true;
                }
                if (IsChinese(c))
                {
                    strw = 2;
                }
                else
                {
                    strw = 1;
                }
                j = j + strw;
                if (b)
                {
                    l = l + strw;
                    if ((l + 1) >= len) break;
                }
            }
            return rstr;
        }

        /// <summary>
        /// 是否是中文
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsChinese(char c)
        {
            return (int)c >= 0x4E00 && (int)c <= 0x9FA5;
        }

        /// <summary>
        /// 获得一个唯一字符，16位
        /// </summary>
        /// <returns></returns>
        public static string GuidString()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 截断字符串，保留指定字节数
        /// </summary>
        /// <param name="input"></param>
        /// <param name="keepByteCount"></param>
        /// <returns></returns>
        public static string TruncateString(string input, uint keepByteCount)
        {
            while (Encoding.GetEncoding("gb2312").GetByteCount(input) > keepByteCount)
            {
                input = input.Remove(input.Length - 1);
            }
            return input;
        }
        /// <summary>
        /// 截取字符串右侧指定数量字符
        /// </summary>
        /// <param name="pString">待截取字符串</param>
        /// <param name="len">截取长度</param>
        /// <returns></returns>
        public static string Right(string pString, int len)
        {
            if (string.IsNullOrEmpty(pString))
            {
                return pString;
            }

            if (pString.Length <= len)
            {
                return pString;
            }

            pString = pString.Remove(0, pString.Length - len);

            return pString;
        }
        /// <summary>
        /// 允许0开头，如果allow_zero是true
        /// </summary>
        /// <param name="randomLength"></param>
        /// <param name="allowZero"></param>
        /// <returns></returns>
        public static string GetRandomNum(int randomLength, bool allowZero)
        {
            int i, p;
            string r_string;

            if (randomLength <= 0 || randomLength > 20)
                return null;

            Random rd = StringHelper.SYS_RD;

            //再用一个随机因子，时间。。。
            long tick = DateTime.Now.Ticks;
            Random rd2 = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            if (!allowZero)
                while ((p = GetNextRandDigit(rd, rd2)) == 0)
                { }
            else
                p = GetNextRandDigit(rd, rd2);

            r_string = p.ToString();

            for (i = 0; i < randomLength - 1; i++)
            {
                p = GetNextRandDigit(rd, rd2);
                r_string += p.ToString();
            }

            return r_string;
        }

        /// <summary>
        /// 随机生成字符传
        /// </summary>
        /// <param name="codeLen"></param>
        /// <returns>不包括 0-o,2-z,l-1</returns>
        public static string GetRandomString(int codeLen)
        {
            const string codeSerial = "3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,m,n,p,q,r,s,t,u,v,w,x,y";
            string[] arr = codeSerial.Split(',');
            string code = "";

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                int randValue = rand.Next(0, arr.Length - 1);
                code += arr[randValue];
            }

            return code;
        }

        /// <summary>
        /// 取得00-FF的随机字符串
        /// </summary>
        /// <param name="codeLen"></param>
        /// <returns></returns>
        public static string Get00_FFstring(int codeLen)
        {
            const string codeSerial = "1,2,3,4,5,6,7,8,9,0,a,b,c,d,e,f";
            string[] arr = codeSerial.Split(',');

            return CreateRandomStr(codeLen, arr);
        }

        /// <summary>
        /// 为更强的随机数准备的
        /// </summary>
        /// <param name="rd1"></param>
        /// <param name="rd2"></param>
        /// <returns></returns>
        private static int GetNextRandDigit(Random rd1, Random rd2)
        {
            return (rd1.Next(10) + rd2.Next(10)) % 10;
        }

        public static Random SYS_RD
        {
            get
            {
                return new Random(GetRandomSeed());
            }
        }


        private static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];

            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }


        public static string SubString(string source, int length, string extStr)
        {
            if (string.IsNullOrEmpty(source))
                return source;
            if (source.Length <= length)
                return source;

            return source.Substring(0, length) + extStr;
        }

        /// <summary>
        /// 截取字符串长度，兼容双字节字符，长度以字节长度为准
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <param name="extStr"></param>
        /// <returns></returns>
        public static string SubStr(string source, int length, string extStr = "...")
        {
            if (string.IsNullOrEmpty(source) || length <= 0) return string.Empty;

            string result = source;
            int pos = 0, realLen = 0;
            Char[] arrChars = source.ToCharArray();
            for (int m = 0; m < arrChars.Length; m++)
            {
                realLen += Encoding.Default.GetByteCount(arrChars, m, 1);
                pos = m;
                if (realLen > length) { break; }
            }
            if (pos < source.Length - 1)
            {
                result = source.Substring(0, pos) + extStr;
            }

            return result;
        }


        public static string CreateVerifyCode(int codeLen, bool onlyNumber = false)
        {
            string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            if (onlyNumber) codeSerial = "0,1,2,3,4,5,6,7,8,9";

            string[] arr = codeSerial.Split(',');

            string code = "";

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                int randValue = rand.Next(0, arr.Length - 1);

                code += arr[randValue];
            }

            return code;
        }

        /// <summary>
        /// Gets the PY string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string GetPYString(string str)
        {
            string tempStr = "";
            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {//字母和符号原样保留
                    tempStr += c.ToString();
                }
                else
                {//累加拼音声母
                    tempStr += GetPYChar(c.ToString());
                }
            }
            return tempStr;
        }

        /// <summary>
        /// Gets the PY char.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        private static string GetPYChar(string c)
        {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));

            if (i < 0xB0A1) return "*";
            if (i < 0xB0C5) return "a";
            if (i < 0xB2C1) return "b";
            if (i < 0xB4EE) return "c";
            if (i < 0xB6EA) return "d";
            if (i < 0xB7A2) return "e";
            if (i < 0xB8C1) return "f";
            if (i < 0xB9FE) return "g";
            if (i < 0xBBF7) return "h";
            if (i < 0xBFA6) return "g";
            if (i < 0xC0AC) return "k";
            if (i < 0xC2E8) return "l";
            if (i < 0xC4C3) return "m";
            if (i < 0xC5B6) return "n";
            if (i < 0xC5BE) return "o";
            if (i < 0xC6DA) return "p";
            if (i < 0xC8BB) return "q";
            if (i < 0xC8F6) return "r";
            if (i < 0xCBFA) return "s";
            if (i < 0xCDDA) return "t";
            if (i < 0xCEF4) return "w";
            if (i < 0xD1B9) return "x";
            if (i < 0xD4D1) return "y";
            if (i < 0xD7FA) return "z";

            return "*";
        }


        /// <summary>
        /// Creates the number.
        /// </summary>
        /// <param name="codeLen">The code len.</param>
        /// <returns></returns>
        public static string CreateNumber(int codeLen)
        {
            const string codeSerial = "1,2,3,4,5,6,7,8,9,0";
            string[] arr = codeSerial.Split(',');

            return CreateRandomStr(codeLen, arr);
        }

        /// <summary>
        /// Creates the password.
        /// </summary>
        /// <param name="codeLen">The code len.</param>
        /// <returns></returns>
        public static string CreatePassword(int codeLen)
        {
            const string codeSerial = "1,2,3,4,5,6,7,8,9,0,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] arr = codeSerial.Split(',');

            return CreateRandomStr(codeLen, arr);
        }
        /// <summary>
        /// 生成随机只含字母的字符串
        /// </summary>
        /// <param name="codeLen">The code len.</param>
        /// <returns></returns>
        public static string CreateRandChar(int codeLen)
        {
            const string codeSerial = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] arr = codeSerial.Split(',');

            return CreateRandomStr(codeLen, arr);
        }


        /// <summary>
        /// Creates the random STR.
        /// </summary>
        /// <param name="codeLen">The code len.</param>
        /// <param name="baseSerial">The base serial.</param>
        /// <returns></returns>
        public static string CreateRandomStr(int codeLen, string[] baseSerial)
        {
            string code = "";

            var rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                int randValue = rand.Next(0, baseSerial.Length - 1);

                code += baseSerial[randValue];
            }

            return code;
        }

        /// <summary>
        /// Gets the friendly date.
        /// </summary>
        /// <param name="dtTime">The dt time.</param>
        /// <returns></returns>
        public static string GetFriendlyTime(DateTime dtTime)
        {
            DateTime dtNow = DateTime.Now;
            TimeSpan tsDiff = dtNow - dtTime;
            string lastDateStr = "";
            if (tsDiff.TotalMinutes <= 1)
            {
                lastDateStr = "<span class='s time'>" + (int)tsDiff.TotalSeconds + "秒前</span>";
            }
            else if (tsDiff.TotalHours <= 1)
            {
                lastDateStr = "<span class='m time'>" + (int)tsDiff.TotalMinutes + "分前</span>";
            }
            else if (tsDiff.TotalHours <= 8)
            {
                lastDateStr = "<span class='h time'>" + (int)tsDiff.TotalHours + "小时前</span>";
            }
            else if (tsDiff.Days == 0 && dtNow.Day == dtTime.Day)
            {
                lastDateStr = "<span class='td time'>今天 " + dtTime.ToString("HH:mm") + "</span>";
            }
            else if (tsDiff.Days <= 1 && dtNow.Day == dtTime.Day + 1)
            {
                lastDateStr = "<span class='yt time'>昨天 " + dtTime.ToString("HH:mm") + "</span>";
            }
            else
            {
                lastDateStr = "<span class='t time'>" + dtTime.ToString("yy-MM-dd hh:mm") + "</span>";
            }
            return lastDateStr;
        }

        /// <summary>
        /// 处理金额数据的长度，超过一定长度后自动进行单位转换，目前仅支持万、亿、万亿三个级别
        /// </summary>
        /// <param name="money"></param>
        /// <param name="maxLen"></param>
        /// <returns></returns>
        public static string ConverMoneyUnit(double money, int maxLen = 5)
        {
            int len = money.ToString("G").Length;
            if (len < maxLen)
            {
                return ((decimal)money).ToString();
            }
            if (len > 13) maxLen--;

            double mm = len > 9 ? (double)10000000 : (double)10000;

            decimal aa = (decimal)(money / mm);
            double bb = money % mm;

            mm = 1;
            for (int m = 0; m < ((decimal)bb).ToString().Length; m++)
            {
                mm *= (double)10;
            }
            string result = string.Format("{0}{1}", aa, string.Format("{0:N2}", bb / mm).Substring(1));
            if (result.Length > maxLen - 1) result = result.Substring(0, maxLen - 1);
            if (result.EndsWith(".")) result = result.Substring(0, result.Length - 1);
            result = string.Format("{0}{1}", result, len > 13 ? "万亿" : len > 9 ? "亿" : "万"); ;

            return result;
        }

        public static string GetStars(double ProductGrade)
        {
            if (ProductGrade <= 0.5)
            {
                return "starBgH";
            }
            else if (ProductGrade < 1.5)
            {
                return "starBg1";
            }
            else if (ProductGrade < 2)
            {
                return "starBgH2";
            }
            else if (ProductGrade < 2.5)
            {
                return "starBg2";
            }
            else if (ProductGrade < 3)
            {
                return "starBgH3";
            }
            else if (ProductGrade < 3.5)
            {
                return "starBg3";
            }
            else if (ProductGrade < 4)
            {
                return "starBgH4";
            }
            else if (ProductGrade < 4.5)
            {
                return "starBg4";
            }
            else if (ProductGrade < 5)
            {
                return "starBgH5";
            }
            else
            {
                return "starBg5";
            }
        }


    }
}
