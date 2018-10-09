using System;

namespace SFO2O.Utility.Uitl
{
    public class ParseHelper
    {

        #region ToInt

        /// <summary>
        /// 将参数中的值转换成int类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns></returns>
        public static int ToInt(object o)
        {
            return ToInt(o, 0);
        }

        /// <summary>
        /// 将参数中的值转换成int类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <param name="DefaultValue">转换失败后返回的默认值</param>
        /// <returns></returns>
        public static int ToInt(object o, int DefaultValue)
        {
            int result = 0;

            if (!int.TryParse(Convert.ToString(o), out result))
            {
                try
                {
                    result = Convert.ToInt32(o);
                }
                catch 
                {
                    result = DefaultValue;
                }
            }

            return result;
        }

        #endregion


        #region ToLong

        /// <summary>
        /// 将参数中的值转换成long类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns></returns>
        public static long ToLong(object o)
        {
            return ToLong(o, 0);
        }

        /// <summary>
        /// 将参数中的值转换成long类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <param name="DefaultValue">转换失败后返回的默认值</param>
        /// <returns></returns>
        public static long ToLong(object o, long DefaultValue)
        {
            long result = 0;
            if (!long.TryParse(Convert.ToString(o), out result))
                result = DefaultValue;
            return result;
        }

        #endregion


        #region ToDecimal

        /// <summary>
        /// 将参数中的值转换成decimal类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns></returns>
        public static decimal ToDecimal(object o)
        {
            return ToDecimal(o, 0);
        }

        /// <summary>
        /// 将参数中的值转换成decimal类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <param name="DefaultValue">转换失败后返回的默认值</param>
        /// <returns></returns>
        public static decimal ToDecimal(object o, decimal DefaultValue)
        {
            decimal result = 0;
            if (!decimal.TryParse(Convert.ToString(o), out result))
                result = DefaultValue;
            return result;
        }

        #endregion


        #region ToDouble

        /// <summary>
        /// 将参数中的值转换成double类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns></returns>
        public static double ToDouble(object o)
        {
            return ToDouble(o, 0);
        }

        /// <summary>
        /// 将参数中的值转换成double类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <param name="DefaultValue">转换失败后返回的默认值</param>
        /// <returns></returns>
        public static double ToDouble(object o, double DefaultValue)
        {
            double result = 0;
            if (!double.TryParse(Convert.ToString(o), out result))
                result = DefaultValue;
            return result;
        }

        #endregion


        #region ToBool

        /// <summary>
        /// 将参数中的值转换成bool类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns></returns>
        public static bool ToBool(object o)
        {
            return ToBool(o, false);
        }

        /// <summary>
        /// 将参数中的值转换成bool类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <param name="DefaultValue">转换失败后返回的默认值</param>
        /// <returns></returns>
        public static bool ToBool(object o, bool DefaultValue)
        {
            bool result = false;
            if (!bool.TryParse(Convert.ToString(o), out result))
                result = DefaultValue;
            return result;
        }

        #endregion


        #region ToDatetime

        /// <summary>
        /// 将参数中的值转换成DateTime类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns></returns>
        public static DateTime ToDatetime(object o)
        {
            return ToDatetime(o, DateTime.MinValue);
        }

        /// <summary>
        /// 将参数中的值转换成DateTime类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <param name="DefaultValue">转换失败后返回的默认值</param>
        /// <returns></returns>
        public static DateTime ToDatetime(object o, DateTime DefaultValue)
        {
            DateTime result = DateTime.MinValue;
            if (!DateTime.TryParse(Convert.ToString(o), out result))
                result = DefaultValue;
            return result;
        }

        public static string ToDateStringFromNow(DateTime dt)
        {
            TimeSpan span = dt -DateTime.Now;
            if (span.TotalSeconds < 0)
            {
                return "已结束";
            }
            else if (span.TotalSeconds < 60)
            {
                return string.Format("{0}秒后", (int)Math.Floor(span.TotalSeconds));
            }
            else if (span.TotalMinutes < 60)
            {
                return string.Format("{0}分钟后", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalHours < 24)
            {
                return string.Format("{0}小时后", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalDays < 60)
            {
                return string.Format("{0}天后", (int)Math.Floor(span.TotalDays));
            }
            else
            {
                return dt.ToShortDateString();
            }
        }


        #endregion


        #region ToString

        /// <summary>
        /// 将参数中的值转换成string类型
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToString(object o)
        {
            if (o != null && o != DBNull.Value)
                return Convert.ToString(o);
            return "";
        }

        #endregion
    }
}
