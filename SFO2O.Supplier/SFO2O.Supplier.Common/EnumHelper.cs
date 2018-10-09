using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.Specialized;

namespace SFO2O.Supplier.Common
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public static class EnumHelper
    {
        private static readonly String ENUM_TITLE_SEPARATOR = ",";
        private static readonly ConcurrentDictionary<String, Dictionary<String, String>> EnumTitleCacheData = new ConcurrentDictionary<String, Dictionary<String, String>>();

        private static Dictionary<String, String> GetAllEnumDescription(Type enumType)
        {
            var dic = new Dictionary<String, String>();
            var arrField = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
            var dicField = new Dictionary<String, FieldInfo>();
            foreach (FieldInfo field in arrField)
            {
                var strEnum = field.GetValue(null).ToString();
                Object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs != null && objs.Length > 0)
                {
                    dic[strEnum] = ((DescriptionAttribute)objs[0]).Description;
                }
            }
            return dic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        private static Dictionary<int, String> GetEnumInfo(Type enumType)
        {
            var dic = new Dictionary<int, String>();
            var arrField = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
            var dicField = new Dictionary<String, FieldInfo>();
            foreach (FieldInfo field in arrField)
            {
                var strEnum = (int)field.GetRawConstantValue();
                Object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs != null && objs.Length > 0)
                {
                    dic[strEnum] = ((DescriptionAttribute)objs[0]).Description;
                }
            }
            return dic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static String GetDescription(int value, Type enumType)
        {
            Dictionary<int, String> dic = GetEnumInfo(enumType);

            if (dic != null)
            {
                if (dic.ContainsKey(value))
                {
                    return dic[value];
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 根据枚举值，返回描述字符串
        /// 如果多选枚举，返回以","分割的字符串
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static String GetDescription(this Enum enumValue)
        {
            if (null == enumValue) { return String.Empty; }
            var type = enumValue.GetType();
            var key = type.ToString();
            Dictionary<String, String> dic;
            String title;
            dic = EnumTitleCacheData.GetOrAdd(key, (arg) => GetAllEnumDescription(type));

            var str = enumValue.ToString();
            String[] arrName = str.Split(ENUM_TITLE_SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            String ret = "";
            foreach (String enumName in arrName)
            {
                var strName = enumName.Trim();
                if (dic.TryGetValue(strName, out title))
                {
                    ret += title + ENUM_TITLE_SEPARATOR;
                }
                else
                {
                    ret += strName + ENUM_TITLE_SEPARATOR;
                }
            }
            if (!string.IsNullOrEmpty(ret))
            {
                return ret.TrimEnd(ENUM_TITLE_SEPARATOR.ToCharArray());
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// <para>将枚举转化为Dictionary。</para>
        /// <para>字典的键是枚举的数字值。</para>
        /// <para>字典的值是枚举的DescriptionAttribute值。</para>
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <returns>存储枚举值,枚举说明的字典。</returns>
        public static IDictionary<int, string> ConvertToDict<T>() where T : struct
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            FieldInfo[] fileds = typeof(T).GetFields();
            string description = string.Empty;
            foreach (FieldInfo filed in fileds)
            {
                object[] attrs = filed.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs == null || attrs.Length == 0)
                {
                    continue;
                }

                description = ((DescriptionAttribute)attrs[0]).Description;
                result.Add((int)filed.GetRawConstantValue(), description);
            }

            return result;
        }


    }
}
