using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
//using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SFO2O.Utility.Uitl
{
    public static class JsonConvertTool
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string SerializeObject(object obj)
        {
            string result = "";
            if (obj != null)
            {
                result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                result = JsonDateTimeFormat(result);
            }
            return result;
        }

        #region 这个序列化可以格式化 bootstrap table 前端正常显示时间

        public static string ObjToJson(object obj)
        {
            string result = "";
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, obj);
                    byte[] json = ms.ToArray();
                    result = Encoding.UTF8.GetString(json, 0, json.Length);
                    string p = @"\\/Date\(((-|\+)?\d+)\+\d+\)\\/";
                    MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
                    Regex reg = new Regex(p);
                    bool boolint = reg.IsMatch(result);
                    result = reg.Replace(result, matchEvaluator);
                   // result = JsonDateTimeFormat(result);
                }
            }
            catch
            {
                return result;
            }
            return result;
        }
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }


        #endregion
        

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="str">json字符串</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string str)
        {
            str = JsonDateTimeFormat(str);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 处理Json的时间格式为正常格式
        /// </summary>
        public static string JsonDateTimeFormat(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {

                json = Regex.Replace(json,
                    @"\\/Date\((\d+)\)\\/",
                    match =>
                    {
                        DateTime dt = new DateTime(1970, 1, 1);
                        dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                        dt = dt.ToLocalTime();
                        return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    });
                return json;
            }
            else
            {
                return "";
            }
        }
    }
}
