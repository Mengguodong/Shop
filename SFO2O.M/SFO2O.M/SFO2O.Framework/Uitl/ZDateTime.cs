using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFO2O.Framework.Uitl
{
    // <summary>
    /// 日期和时间相关的函数
    /// </summary>
    public static class ZDateTime
    {

        /// <summary>
        /// Unix时间戳的原点
        /// </summary>
        private static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 获得当前时间的Unix时间戳
        /// </summary>
        /// <returns></returns>
        public static UInt32 GetUnixTimeStamp()
        {
            return GetUnixTimeStamp(DateTime.Now);
        }

        /// <summary>
        /// 根据一个DateTime的值获得一个时间的Unix时间戳
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static UInt32 GetUnixTimeStamp(DateTime datetime)
        {
            return (uint)(datetime.ToUniversalTime() - epoch).TotalSeconds;
        }

        /// <summary>
        /// 将Unix时间戳变为DateTime
        /// </summary>
        /// <param name="uts">Unix的时间戳</param>
        /// <returns></returns>
        public static DateTime GetDateTimeByUnixTimeStamp(UInt32 uts)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(epoch).AddSeconds(uts);
        }
    }
}
