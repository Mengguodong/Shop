using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace SFO2O.Framework.Extensions
{
    public static class ObjectExtension
    {
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
