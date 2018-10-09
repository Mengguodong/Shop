using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace SFO2O.Utility.Uitl
{
    /// <summary>
    /// 枚举工具类
    /// </summary>
    public static class EnumUtils
    {
        private static readonly Hashtable HtCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 获取枚举对应的Description内容
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> GetEnumDescriptions(Type type)
        {
            if (HtCache.Contains(type))
                return HtCache[type] as IList<KeyValuePair<string, string>>;
            IList<KeyValuePair<string, string>> dict = GetDictionary(type);
            HtCache[type] = dict;
            return dict;
        }

        ///<summary>
        /// 根据枚举值的文本形式，获取该枚举值的描述文本。
        ///</summary>
        ///<param name="type">枚举类型</param>
        ///<param name="fieldName">一个枚举值的文本形式(ToString())</param>
        ///<returns></returns>
        public static string GetEnumDescriptionByText(Type type, string fieldName)
        {
            IList<KeyValuePair<string, string>> dictionary = GetEnumDescriptions(type);
            foreach (var keyValuePair in dictionary)
            {
                if (keyValuePair.Key == ((int)(Enum.Parse(type, fieldName))).ToString())
                {
                    return keyValuePair.Value;
                }
            }
            return fieldName;
        }

        /// <summary>
        /// 获取枚举类型的值，描述键值对
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IList<KeyValuePair<string, string>> GetDictionary(Type type)
        {
            FieldInfo[] fields = type.GetFields();
            IList<KeyValuePair<string, string>> dict = new List<KeyValuePair<string, string>>();
            for (int i = 1; i < fields.Length; i++)
            {
                FieldInfo item = fields[i];
                string desription;
                object[] objs = item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs != null && objs.Length != 0)
                {
                    var da = (DescriptionAttribute)objs[0];
                    desription = da.Description;
                }
                else
                {
                    desription = item.Name;
                }
                dict.Add(new KeyValuePair<string, string>(((int)Enum.Parse(type, item.Name)).ToString(), desription));
            }
            return dict;
        }

        /// <summary>
        /// 对于标记为Flags的枚举，判断枚举值A是否包含在枚举值B中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionA"></param>
        /// <param name="optionB"></param>
        /// <returns></returns>
        public static bool IsInOption<T>(T optionA, T optionB)
        {
            //int i = optionB.GetHashCode() - optionA.GetHashCode();
            //return (i & (i - 1)) == 0;//是否2的乘方
            var options = new List<string>();
            options.AddRange(optionB.ToString().Split(','));
            return options.Contains(optionA.ToString());
        }

        ///<summary>
        /// 根据一个枚举类型的描述，获取该枚举类型对应的HashCode
        ///</summary>
        ///<param name="type">枚举类型</param>
        ///<param name="description">描述</param>
        ///<returns></returns>
        public static string GetEnumHashCode(Type type, string description)
        {
            IList<KeyValuePair<string, string>> dictionary = GetEnumDescriptions(type);
            foreach (var keyValuePair in dictionary)
            {
                if (keyValuePair.Value == description)
                {
                    return keyValuePair.Key;
                }
            }
            return description;
        }

        ///<summary>
        /// 根据一个枚举类型，获取该枚举类型对应的描述文本信息
        ///</summary>
        ///<param name="enumSubitem">枚举值</param>
        ///<returns>描述文本</returns>
        public static string GetEnumDescription(object enumSubitem)
        {
            string strValue = enumSubitem.ToString();
            FieldInfo fieldinfo = enumSubitem.GetType().GetField(strValue);
            if (fieldinfo != null)
            {
                Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs == null || objs.Length == 0)
                {
                    return strValue;
                }
                var da = (DescriptionAttribute)objs[0];
                return da.Description;
            }
            return strValue;
        }

        /// <summary>
        /// 对于标记为Flags的枚举值，返回对应的HashCode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="names"></param>
        /// <returns></returns>
        public static IList<int> GetCodesFromEnumString<T>(string names)
        {
            var codes = new List<int>();
            foreach (string name in names.Split(','))
            {
                var t2 = (T)Enum.Parse(typeof(T), name);
                codes.Add(t2.GetHashCode());
            }
            return codes;
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