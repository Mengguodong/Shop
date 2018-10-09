using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using SFO2O.Framework.Extensions;

//using System.Web.Script.Serialization;

namespace SFO2O.Utility.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// 判断是否有效手机号码，以13或15开头的11位定长数字串
        /// </summary>
        /// <param name="mobile">要判断的mobile字符串</param>
        /// <param name="isChina">null 全部验证，true 国内格式，false 香港格式</param>
        /// <returns></returns>
        public static bool IsMobilePhoneNum(this string mobile, bool? isChina = null)
        {
            if (mobile == null)
            {
                return false;
            }
            if (isChina == null)
            {

                return Regex.IsMatch(mobile, @"1(3|4|5|7|8)\d{9}") || Regex.IsMatch(mobile, @"(5|6|9)\d{7}");
            }
            else if (isChina == true)
            {
                return Regex.IsMatch(mobile, @"1(3|4|5|7|8)\d{9}");

            }
            else
            {
                return Regex.IsMatch(mobile, @"(5|6|9)\d{7}");
            }
        }
        public static bool IsValidEmail(this string email)
        {
            string pattern = "^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$";
            var match = Regex.Match(email, pattern, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static string SubString(this string str, int len, string suffix)
        {
            string temp = str.Substring(0, (str.Length < len + 1) ? str.Length : len + 1);
            byte[] encodedBytes = System.Text.Encoding.ASCII.GetBytes(temp);

            string outputStr = "";
            int count = 0;

            for (int i = 0; i < temp.Length; i++)
            {
                if ((int)encodedBytes[i] == 63)
                    count += 2;
                else
                    count += 1;

                if (count <= len - suffix.Length)
                    outputStr += temp.Substring(i, 1);
                else if (count > len)
                    break;
            }

            if (count <= len)
            {
                outputStr = temp;
                suffix = "";
            }

            outputStr += suffix;

            return outputStr;
        }

        public static string SafeString(this string str, int startIndex, int length, char replacer = '*')
        {
            if (str.IsNullOrWhiteSpace())
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
                for (int i = 0; i < str.Length; i++)
                {
                    sb.Append(replacer);
                }
                return sb.ToString();
            }

        }
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 如果字符串是空，则返回指定的值。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string IsNull(this string str, string defaultValue)
        {
            return str.IsNullOrEmpty() ? defaultValue : str;
        }

        ///// <summary>
        ///// 判断字符串是否包含指定的字符串。(好像有问题!!!!!!)
        ///// </summary>
        ///// <param name="str"></param>
        ///// <param name="parameter"></param>
        ///// <returns></returns>
        //public static bool Contains(this string str, params string[] parameter)
        //{
        //    if (str.IsNullOrEmpty())
        //    {
        //        return false;
        //    }

        //    if (parameter == null || parameter.Length <= 0)
        //    {
        //        return false;
        //    }

        //    foreach (var item in parameter)
        //    {
        //        if (!str.Contains(item))
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        /// <summary>
        /// 反序列化JSON对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T GetObjFromJson<T>(this string json)
        {
            if (!string.IsNullOrWhiteSpace(json))
            {
                return new JavaScriptSerializer().Deserialize<T>(json);
            }

            return default(T);
        }

        /// <summary>
        /// 转换为数字格式。
        /// </summary>
        /// <param name="number"></param>
        /// <param name="decimalPlacesReserved">保留小数位。</param>
        /// <returns></returns>
        public static string ToNumberString(this int number)
        {
            return ToNumberString(number, false);
        }

        /// <summary>
        /// 转换为数字格式。
        /// </summary>
        /// <param name="number"></param>
        /// <param name="decimalPlacesReserved">保留小数位。</param>
        /// <returns></returns>
        public static string ToNumberString(this decimal number, bool decimalPlacesReserved = false)
        {
            if (decimalPlacesReserved)
            {
                return number.ToString("#,##0.00");
            }
            else
            {
                return number.ToString("#,##0");
            }
        }
        /// <summary>
        /// 带小数则输出小数，否则输出整数
        /// </summary>
        /// <param name="number"></param> 
        /// <returns></returns>
        public static string ToNumberStringIntelligent(this decimal number)
        {
            var intValue = number.As(0);

            bool isInt = (number - intValue) == 0M;

            return number.ToString(!isInt ? "#,##0.00" : "#,##0");
        }
    }
}
