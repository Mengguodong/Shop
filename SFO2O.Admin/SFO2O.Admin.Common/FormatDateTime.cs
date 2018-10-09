using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Common
{
    public static class FormatDateTime
    {

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns>年月日形式串:如2013-12-31、2014-1-1</returns>
        public static string ToDate(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 格式化时间，若dateTime为null则返回""
        /// </summary>
        /// <param name="dateTime?">时间</param>
        /// <returns>年月日形式串:如2013-12-31、2014-1-1</returns>
        public static string ToDate(DateTime? dateTime)
        {
            if (dateTime == null)
                return "";
            else
                return dateTime.Value.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 格式化时间为yyyy-M-d HH:mm:ss
        /// </summary>
        /// <param name="dateTime">时间</param>
        public static string ToDateTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-M-d HH:mm:ss");
        }

        /// <summary>
        /// 格式化时间为yyyy-M-d HH:mm:ss，若dateTime为null则返回""
        /// </summary>
        /// <param name="dateTime?">时间</param>
        public static string ToDateTime(DateTime? dateTime)
        {
            if (dateTime == null)
                return "";
            else
                return dateTime.Value.ToString("yyyy-M-d HH:mm:ss");
        }
        /// <summary>
        /// 格式化时间为yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="dateTime">时间</param>
        public static string ToDateTimeMM(DateTime dateTime)
        {
            if (dateTime.ToString("yyyy-MM-dd HH:mm") == "0001-01-01 00:00")
            {
                return "";
            }
            else
            {
                return dateTime.ToString("yyyy-MM-dd HH:mm");
            }

        }
        /// <summary>
        /// 格式化时间为yyyy-MM-dd HH:mm，若dateTime为null则返回""
        /// </summary>
        /// <param name="dateTime?">时间</param>
        public static string ToDateTimeMM(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return "";
            }
            else
            {

                string time = dateTime.Value.ToString("yyyy-MM-dd HH:mm");
                if (time == "0001-01-01 00:00")
                {
                    return "";
                }
                else
                {
                    return time;
                }
            }
        }


    }
}
