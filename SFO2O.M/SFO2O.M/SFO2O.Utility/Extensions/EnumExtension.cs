using System;
using System.ComponentModel;
using System.Linq;

namespace SFO2O.Utility.Extensions
{
    public static class EnumExtension
    {
        public static int ToInt(this Enum obj)
        {
            return Convert.ToInt32(obj);
        }

        public static T ToEnum<T>(this string obj) where T : struct
        {
            if (string.IsNullOrEmpty(obj))
            {
                return default(T);
            }
            try
            {
                return (T)Enum.Parse(typeof(T), obj, true);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static string ToDescriptionString(this Enum obj)
        {
            var attribs = (DescriptionAttribute[])obj.GetType().GetField(obj.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attribs.Length > 0 ? attribs[0].Description : obj.ToString();
        }

        public static string GetDescriptionString(this Type type, int? id)
        {
            var values = from Enum e in Enum.GetValues(type)
                         select new { id = e.ToInt(), name = e.ToDescriptionString() };

            if (!id.HasValue) id = 0;

            return values.ToList().Find(c => c.id == id).name;
        }
        /// <summary>
        /// 获取枚举的显示名称。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayString(this Enum value)
        {
            var attribute = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

            if (attribute != null)
            {
                return attribute.Description;
            }

            return value.ToString();
        }
    }
}
