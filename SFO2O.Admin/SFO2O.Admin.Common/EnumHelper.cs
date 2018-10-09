using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace SFO2O.Admin.Common
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
    }
}
