using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SFO2O.Supplier.Common
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 是否有上传的文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasFile(this HttpPostedFileBase file)
        {
            return file != null && file.ContentLength > 0;
        }

        #region DateTime

        /// <summary>
        /// 格式化时间为yyyy-MM-dd
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns>年月日形式串:如2013-12-31、2014-01-01</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 格式化时间为yyyy-MM-dd，若dateTime为null则返回""
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns>年月日形式串:如2013-12-31、2014-01-01</returns>
        public static string ToDateString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return "";
            else
                return dateTime.Value.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 格式化时间为yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static String ToDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 格式化时间为yyyy-MM-dd HH:mm:ss，若dateTime为null则返回String.Empty
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static String ToDateTimeString(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToDateTimeString();
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion

        /// <summary>
        /// str为null时返回null，否则返回string.Trim()的结果
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static String SafeTrim(this String str)
        {
            return str == null ? null : str.Trim();
        }

        public static String Omit(this String str, int byteLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(str) || byteLength <= 0) return string.Empty;

            string result = str;
            int pos = 0, realLen = 0;
            Char[] arrChars = str.ToCharArray();
            for (int m = 0; m < arrChars.Length; m++)
            {
                realLen += Encoding.UTF8.GetByteCount(arrChars, m, 1);
                pos = m;
                if (realLen > byteLength) { break; }
            }
            if (pos < str.Length - 1)
            {
                result = str.Substring(0, pos) + suffix;
            }

            return result;
        }

        public static void AddIfNotEmpty(this Dictionary<string, string> dic, String key, String value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                dic.Add(key, value.Replace("$", ","));
            }
        }
    }
}
