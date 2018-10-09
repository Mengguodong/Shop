using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using NPOI.OpenXmlFormats.Dml;

namespace SFO2O.Utility.Extensions
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 判断是否有效手机号码，以13或15开头的11位定长数字串
        /// </summary>
        /// <param name="mobile">要判断的mobile字符串</param>
        /// <param name="isChina">null 全部验证，true 国内格式，false 中华人民共和国大陆地区格式</param>
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
            string pattern = "/^([0-9a-zA-Z-._]+)@(([0-9a-zA-Z-]+\\.)+[a-zA-Z]{2,4})$/";
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
        public static decimal ToNumberRound(this decimal number, int position = 2)
        {
            var value = Math.Round(number, position,MidpointRounding.AwayFromZero);
            return value;
        }
        public static string ToNumberRoundString(this decimal number, int position = 2)
        {
            number = Math.Round(number, position);
            var intValue = number.As(0);

            bool isInt = (number - intValue) == 0M;

            return number.ToString(!isInt ? "#,##0.00" : "#,##0");
        }
        /// <summary>
        /// 带2位小数
        /// </summary>
        /// <param name="number"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string ToNumberRoundStringWithPoint(this decimal number, int position = 2)
        {
            number = Math.Round(number, position, MidpointRounding.AwayFromZero);
            var intValue = number.As(0);

            return number.ToString("#,##0.00");
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
        public static string ToNumberStringNoZero(this decimal number)
        {
            var intValue = number.As(0);

            bool isInt = (number - intValue) == 0M;
            string returnStr =  number.ToString(!isInt ? "#,##0.00" : "#,##0");
            if (returnStr.EndsWith("0"))
            {
                string returnStrOne = number.ToString(!isInt ? "#,##0.0" : "#,##0");
                return returnStrOne;
            }
            return returnStr;
        }
        /// <summary>
        /// 带小数则输出小数，否则输出整数
        /// </summary>
        /// <param name="number"></param> 
        /// <returns></returns>
        public static string ToNumberStringNoComma(this decimal number)
        {
            var intValue = number.As(0);

            bool isInt = (number - intValue) == 0M;

            return number.ToString(!isInt ? "###0.00" : "###0");
        }
        public static string ToNumberStringFloat(this decimal number)
        {
            var intValue = number.As(0);

            bool isInt = (number - intValue) == 0M;

            string str = string.Empty;
            if (isInt)
            {
                str = number.ToString("###0");
            }
            else
            {
                str = number.ToString("0.##");  
            }
            return str;
        }
        public static string ToCeilingNumberStringIntelligent(this decimal number)
        {
            number = Math.Ceiling(number * 100) / 100M;//2位进位
            var intValue = number.As(0);

            bool isInt = (number - intValue) == 0M;

            return number.ToString(!isInt ? "#,##0.00" : "#,##0");
        }
        /// <summary>
        /// 返回格式化流逝时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="now">参考值(now)</param>
        /// <returns></returns>
        public static string Elapse(this DateTime dateTime, DateTime now)
        {

            var diff = now - dateTime;
            string diffstring = string.Empty;
            if (diff.Days > 0)
            {
                diffstring += diff.Days.ToString() + "天";
                return diffstring;
            }
            if (diff.Hours > 0)
            {
                diffstring += diff.Hours.ToString() + "小时";
                return diffstring;
            }
            if (diff.Minutes > 0)
            {
                diffstring += diff.Minutes.ToString() + "分钟";
            }
            if (diffstring.IsNullOrEmpty())
            {

                diffstring = "约1分钟";
            }
            return diffstring;


        }
        /// <summary>
        /// 返回格式化流逝时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string Elapse(this DateTime dateTime)
        {
            return dateTime.Elapse(DateTime.Now);
        }
        #region 数据转换
        /// <summary>
        /// 数据转换。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns></returns>
        public static bool AsBoolean(this object obj, bool defaultValue = false)
        {
            if (obj == null || obj is DBNull)
            {
                return defaultValue;
            }

            bool result = defaultValue;

            try
            {
                result = Convert.ToBoolean(obj);
            }
            catch
            {
                bool.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        /// <summary>
        /// 数据转换。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns></returns>
        public static byte AsByte(this object obj, byte defaultValue = 0)
        {
            if (obj == null || obj is DBNull)
            {
                return defaultValue;
            }

            byte result = defaultValue;

            try
            {
                result = Convert.ToByte(obj);
            }
            catch
            {
                byte.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        /// <summary>
        /// 数据转换。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns></returns>
        public static short AsInt16(this object obj, short defaultValue = 0)
        {
            if (obj == null || obj is DBNull)
            {
                return defaultValue;
            }

            short result = defaultValue;

            try
            {
                result = Convert.ToInt16(obj);
            }
            catch
            {
                short.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        /// <summary>
        /// 数据转换。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns></returns>
        public static int AsInt32(this object obj, int defaultValue = 0)
        {
            if (obj == null || obj is DBNull)
            {
                return defaultValue;
            }

            int result = defaultValue;

            try
            {
                result = Convert.ToInt32(obj);
            }
            catch
            {
                int.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        /// <summary>
        /// 数据转换。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns></returns>
        public static long AsInt64(this object obj, long defaultValue = 0)
        {
            if (obj == null || obj is DBNull)
            {
                return defaultValue;
            }

            long result = defaultValue;

            try
            {
                result = Convert.ToInt64(obj);
            }
            catch
            {
                long.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        /// <summary>
        /// 数据转换。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns></returns>
        public static float AsSingle(this object obj, float defaultValue = 0)
        {
            if (obj == null || obj is DBNull)
            {
                return defaultValue;
            }

            float result = defaultValue;

            try
            {
                result = Convert.ToSingle(obj);
            }
            catch
            {
                float.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        /// <summary>
        /// 数据转换。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns></returns>
        public static double AsDouble(this object obj, double defaultValue = 0)
        {
            if (obj == null || obj is DBNull)
            {
                return defaultValue;
            }

            double result = defaultValue;

            try
            {
                result = Convert.ToDouble(obj);
            }
            catch
            {
                double.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        /// <summary>
        /// 数据转换。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns></returns>
        public static decimal AsDecimal(this object obj, decimal defaultValue = 0)
        {
            if (obj == null || obj is DBNull)
            {
                return defaultValue;
            }

            decimal result = defaultValue;

            try
            {
                result = Convert.ToDecimal(obj);
            }
            catch
            {
                decimal.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        public static string As9Plus(this int value)
        {
            if (value > 99)
            {
                return "9+";
            }
            else
            {
                return value.As("");
            }
        }

        /// <summary>
        /// 数据转换。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值。</param>
        /// <param name="after">转换完成后要处理的一些工作。</param>
        /// <returns></returns>
        public static T As<T>(this object obj, T defaultValue = default(T), Action after = null)
        {
            if (obj == null || obj is DBNull)
            {
                return defaultValue;
            }

            T result;

            try
            {
                result = (T)Convert.ChangeType(obj, typeof(T));
            }
            catch
            {
                result = defaultValue;
            }

            if (after != null)
            {
                after();
            }

            return result;
        }
        #endregion

        #region 克隆对象
        private static Dictionary<Type, XmlSerializer> serializer = new Dictionary<Type, XmlSerializer>();

        /// <summary>
        /// 克隆一个对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="extra">克隆完成后要处理的一些工作。</param>
        /// <returns></returns>
        public static T Clone<T>(this T source, Action<T> extra = null)
            where T : class
        {
            Type type = typeof(T);
            XmlSerializer xs;

            if (!serializer.TryGetValue(type, out xs))
            {
                xs = new XmlSerializer(type);

                serializer.Add(type, xs);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                xs.Serialize(ms, source);

                ms.Position = 0;

                T result = xs.Deserialize(ms) as T;

                if (extra != null)
                {
                    extra(result);
                }

                return result;
            }
        }
        #endregion

        #region 序列化
        public static byte[] SerializeByJsonSerializer<T>(this T obj)
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

            using (MemoryStream ms = new MemoryStream())
            {

                serializer.WriteObject(ms, obj);

                return ms.ToArray();
            }
        }

        public static string SerializeByJsonSerializer<T>(this T obj, Encoding encoding)
            where T : class
        {
            byte[] bytes = SerializeByJsonSerializer(obj);

            return encoding.GetString(bytes);
        }

        public static byte[] SerializeByXmlSerializer<T>(this T obj)
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            DataContractSerializer serializer = new DataContractSerializer(typeof(T));

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);

                return ms.ToArray();
            }
        }

        public static string SerializeByXmlSerializer<T>(this T obj, Encoding encoding)
            where T : class
        {
            byte[] bytes = SerializeByXmlSerializer(obj);

            return encoding.GetString(bytes);
        }

        public static T DeserializeByJsonSerializer<T>(this byte[] bytes)
            where T : class
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return serializer.ReadObject(ms) as T;
            }
        }

        public static T DeserializeByJsonSerializer<T>(this string str, Encoding encoding)
            where T : class
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            byte[] bytes = encoding.GetBytes(str);

            return DeserializeByJsonSerializer<T>(bytes);
        }

        public static T DeserializeByXmlSerializer<T>(this byte[] bytes)
            where T : class
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            DataContractSerializer serializer = new DataContractSerializer(typeof(T));

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return serializer.ReadObject(ms) as T;
            }
        }

        public static T DeserializeByXmlSerializer<T>(this string str, Encoding encoding)
            where T : class
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            byte[] bytes = encoding.GetBytes(str);

            return DeserializeByXmlSerializer<T>(bytes);
        }
        #endregion
    }
}
