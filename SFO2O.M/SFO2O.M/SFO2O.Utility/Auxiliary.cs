using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Web.Script.Serialization;

namespace Common
{
    /// <summary>
    ///     静态辅助类
    ///     创建人：
    ///     创建时间：
    /// </summary>
    public static class Auxiliary
    {
        public static Random random = new Random();

        public static string Key
        {
            get { return "winegame"; }
        }



        #region 过滤html中危险标签

        public static string CutDangerousHtmlElement(string html)
        {
            var regex1 = new Regex(@"<script[^>]*>[^>]*<[^>]script[^>]*>", RegexOptions.IgnoreCase);
            var regex4 = new Regex(@"<iframe[^>]*>[^>]*<[^>]iframe[^>]*>", RegexOptions.IgnoreCase);
            var regex5 = new Regex(@"<frameset[^>]*>[^>]*<[^>]frameset[^>]*>", RegexOptions.IgnoreCase);
            var regex6 = new Regex(@"<img (.+?) />", RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记  
            html = regex4.Replace(html, ""); //过滤iframe  
            html = regex5.Replace(html, ""); //过滤frameset 

            html = regex6.Replace(html, "<img  " + "${1}" + " />");

            return html;
        }

        #endregion

        #region 防止Sql注入
        /// <summary>
        ///用来检测用户输入是否带有恶意
        ///--------------------------------
        ///修改人：邓福伟
        ///描述：添加SQL注入验证字符
        ///时间：2015年12月07日 12:00:08
        /// </summary>
        /// <param name="text">用户输入的文字</param>
        /// <returns>返回验证后的文字</returns>
        public static string InputText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            text = Regex.Replace(text, "[\\s]{2,{", " ");
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n"); //<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " "); //&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty); //any other tags
            text = Regex.Replace(text, "=", "");
            text = Regex.Replace(text, "%", "");
            text = Regex.Replace(text, "'", "");
            text = Regex.Replace(text, "select", "");
            text = Regex.Replace(text, "insert", "");
            text = Regex.Replace(text, "delete", "");
            text = Regex.Replace(text, "or", "");
            text = Regex.Replace(text, "exec", "");
            text = Regex.Replace(text, "--", "");
            text = Regex.Replace(text, "and", "");
            text = Regex.Replace(text, "where", "");
            text = Regex.Replace(text, "update", "");
            text = Regex.Replace(text, "script", "");
            text = Regex.Replace(text, "iframe", "");
            text = Regex.Replace(text, "master", "");
            text = Regex.Replace(text, "exec", "");
            text = Regex.Replace(text, "<", "");
            text = Regex.Replace(text, ">", "");
            text = Regex.Replace(text, "\r\n", "");

            return text;
        }

        /// <summary>
        ///     该方法用来检测用户输入是否带有恶意
        ///     --------------------------------
        ///     修改人：徐江安
        ///     描述：添加SQL注入验证字符（此方法慎用）
        ///     时间：2015年9月11日 12:00:08
        /// </summary>
        /// <param name="text">用户输入的文字</param>
        /// <param name="maxlength">最大的长度</param>
        /// <returns>返回验证后的文字</returns>
        public static string InputText(string text, int maxlength)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            else
            {
                text = text.ToLower().Trim();//先转化小写在去两边空格
            }

            if (text.Length > maxlength)
            {
                text = text.Substring(0, maxlength);//截取字符最大长度
            }

            text = Regex.Replace(text, "[\\s]{2,{", " ");
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n"); //<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " "); //&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty); //any other tags
            text = Regex.Replace(text, "=", "");
            text = Regex.Replace(text, "%", "");
            text = Regex.Replace(text, "'", "");
            text = Regex.Replace(text, "select", "");
            text = Regex.Replace(text, "insert", "");
            text = Regex.Replace(text, "delete", "");
            text = Regex.Replace(text, "or", "");
            text = Regex.Replace(text, "exec", "");
            text = Regex.Replace(text, "--", "");
            text = Regex.Replace(text, "and", "");
            text = Regex.Replace(text, "where", "");
            text = Regex.Replace(text, "update", "");
            text = Regex.Replace(text, "script", "");
            text = Regex.Replace(text, "iframe", "");
            text = Regex.Replace(text, "master", "");
            text = Regex.Replace(text, "exec", "");
            text = Regex.Replace(text, "<", "");
            text = Regex.Replace(text, ">", "");
            text = Regex.Replace(text, "\r\n", "");
            //text = Regex.Replace(text,"with t as(","");

            return text;
        }
        #endregion

        #region 根据正则匹配，返回第一个结果

        /// <summary>
        ///     根据正则匹配，返回第一个结果
        /// </summary>
        /// <param name="regTxt"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetSingleValue(string regTxt, string html)
        {
            var regex = new Regex(regTxt);
            var m = regex.Match(html);
            var result = string.Empty;
            if (m.Success)
            {
                if (m.Groups.Count == 2)
                {
                    result = m.Groups[1].Value;
                }
                else
                {
                    result = m.Groups[0].Value;
                }
            }

            return result;
        }

        #endregion

        #region SearchAndReplace

        public static string SearchAndReplace(string HtmlStr, string RegexStr, string ReplaceRegex, bool CutHtml)
        {
            var ReturnStr = "";
            var mymatch = Regex.Match(HtmlStr, RegexStr, RegexOptions.IgnoreCase);
            if (mymatch.Success)
            {
                ReturnStr = mymatch.Value;
            }
            else
            {
                ReturnStr = "";
            }
            ReturnStr = Regex.Replace(ReturnStr, ReplaceRegex, "", RegexOptions.IgnoreCase);
            if (CutHtml)
            {
                ReturnStr = CutAllHtmlElement(ReturnStr);
            }
            return ReturnStr.Trim();
        }

        #endregion

        #region 去除HTML

        /// <summary>
        ///     作者：yjq
        ///     时间：2010-03-05
        ///     去除 htmlCode 中所有的HTML标签(包括标签中的属性)
        /// </summary>
        /// <param name="htmlCode">包含 HTML 代码的字符串</param>
        /// <returns>返回一个不包含 HTML 代码的字符串</returns>
        public static string RemoveHtml(string htmlCode)
        {
            if (null == htmlCode || 0 == htmlCode.Length)
            {
                return string.Empty;
            }
            return
                Regex.Replace(htmlCode, @"<[^>]+>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline)
                    .Replace("&nbsp;", " ");
        }

        #endregion

        #region 适应ISAPI_UrlRewrite获取当前RawUrl

        public static string RawUrl()
        {
            var cUrl = HttpContext.Current.Request.ServerVariables["HTTP_X_REWRITE_URL"];
            if (cUrl == null || cUrl == "")
            {
                cUrl = HttpContext.Current.Request.RawUrl;
            }
            return cUrl;
        }

        #endregion


        #region 字符串返回MD5值(未做处理普通MD5加密)
        /// <summary>
        /// 徐功阳 2015年9月5日13:34:47
        /// </summary>
        /// <param name="cipherText">加密字符串</param>
        /// <returns></returns>
        public static string CommonMd5Encrypt(string cipherText)
        {
            StringBuilder sb = new StringBuilder();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(cipherText);
            using (MD5 md5 = MD5.Create())
            {
                byte[] temp = md5.ComputeHash(bytes);
                md5.Clear();
                for (int i = 0; i < temp.Length; i++)
                {
                    sb.Append(temp[i].ToString("x2"));
                }
            }
            return sb.ToString();
        }
        #endregion


        #region  手机号加*号处理
        /// <summary>
        /// 处理手机号，第4位到第8位加*号
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <returns></returns>
        public static string ConvertMobilePhone(string mobilePhone)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < mobilePhone.Length; i++)
            {
                string temp = mobilePhone[i].ToString();
                if (i>2&&i<7)
                {
                    temp = "*";
                }
                sb.Append(temp);

            }
            return sb.ToString();
            
        }


        #endregion


        /// <summary>
        ///     加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Md5Encrypt(string Text)
        {
            return Md5Encrypt(Text, Key);
        }

        /// <summary>
        ///     加密数据
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public static string Md5Encrypt(string Text, string sKey)
        {
            var des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key =
                Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5")
                    .Substring(0, 8));
            des.IV =
                Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5")
                    .Substring(0, 8));
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var ret = new StringBuilder();
            foreach (var b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        ///     解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Md5Decrypt(string Text)
        {
            return Md5Decrypt(Text, Key);
        }

        /// <summary>
        ///     解密数据
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public static string Md5Decrypt(string Text, string sKey)
        {
            var des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            var inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key =
                Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5")
                    .Substring(0, 8));
            des.IV =
                Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5")
                    .Substring(0, 8));
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }


        #region 过滤文章内容关键字

        /// <summary>
        ///     过滤文章内容关键字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MyReplace(string str)
        {
            str = Regex.Replace(str, "(&lt;)(?:.*?)(&gt;)", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "(<img)(?:.*?)(/>)", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "(<)(?:.*?)(>)", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "<[^{><}]*>", "", RegexOptions.IgnoreCase);
            str = str.Replace("<>", "");
            str = str.Replace("&lt;", "");
            str = str.Replace("&gt;", "");
            str = str.Replace("\r", "");
            str = str.Replace("\n", "");
            str = str.Replace("nbsp;", "");
            str = str.Replace("ldquo;", "");
            str = str.Replace("middot;", "");
            str = str.Replace("rdquo;", "");
            str = str.Replace("amp;", "");
            str = str.Replace("mdash;", "");
            str = str.Replace("hellip;", "");
            str = str.Replace("lsquo;", "");
            str = str.Replace("&", "");
            str = str.Replace("<br>", "");
            str = str.Replace("<br />", "");
            str = str.Replace("<p>", "");
            str = str.Replace("</p>", "");
            str = str.Replace("<img", "");
            str = str.Replace("/>", "");
            str = str.Replace(" ", "");
            str = str.Replace("　", "");
            str = str.Replace("??", "");
            str = str.Replace("rsquo;", "");
            str = str.Replace("&", " ");
            str = str.Replace(" ", "");
            str = str.Replace("lt;", "");
            str = str.Replace("gt;", "");
            str = str.Replace("amp;", "");
            str = str.Replace("apos;", "");
            str = str.Replace("quot;", "");
            str = str.Replace("\"", "“");
            str = str.Replace("'", "‘");
            str = str.Replace("<", "＜");
            str = str.Replace(">", "＞");
            str = str.Replace("<!--EndFragment-->", "");
            //str = str.Replace("这里是换行，请不要删除，一定要记住", "<br />");
            return str;
        }

        #endregion

        public static string ChangeTime(object o)
        {
            return Convert.ToDateTime(o).ToString("yyyy-MM-dd");
        }

        public static string ChangeTimeToMMdd(object o)
        {
            return Convert.ToDateTime(o).ToString("MM-dd");
        }

        /// <summary>
        ///     截取字符串，多余的以“...”结尾
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="src">原内容</param>
        /// <returns></returns>
        public static string CutString(int length, object src)
        {
            if (src != null)
            {
                var str = src.ToString();
                if (str.Length <= length)
                    return str;
                return str.Substring(0, length) + "...";
            }
            return string.Empty;
        }

        /// <summary>
        ///     获取随机名称
        /// </summary>
        /// <param name="num">位数</param>
        /// <returns>名称</returns>
        public static string GetRandomFileName(int num)
        {
            var result = string.Empty;
            char[] chars =
            {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
                'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
            };
            var random = new Random();
            for (var i = 0; i < num; i++)
            {
                var index = random.Next(0, chars.Length);
                result += chars[index].ToString();
            }
            return result;
        }

        #region 正则表达式验证

        /// <summary>
        ///     正则验证
        /// </summary>
        /// <param name="regex">正则</param>
        /// <param name="strObj">需要验证的内容</param>
        /// <returns>满足：true，不满足：false</returns>
        public static bool KYJRegex(string regex, string strObj)
        {
            return Regex.IsMatch(strObj, @regex);
        }

        #endregion

        #region 将字符串转换为Int32

        public static Int32 ToInt32(object str)
        {
            return ToInt32(str, 0);
        }

        public static Int32 ToInt32(object str, Int32 defValue)
        {
            Int32 outValue;

            if (str != null && !String.IsNullOrEmpty(str.ToString()))
            {
                if (Int32.TryParse(str.ToString(), out outValue))
                {
                    return outValue;
                }
            }
            return defValue;
        }

        /// <summary>
        ///     将字符串转换为Float
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(String str, float defValue)
        {
            float outValue;

            if (!String.IsNullOrEmpty(str))
            {
                if (float.TryParse(str, out outValue))
                {
                    return outValue;
                }
            }
            return defValue;
        }

        /// <summary>
        ///     将字符串转换为decimal
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ToDecimal(string str)
        {
            return ToDecimal(str, 0);
        }

        /// <summary>
        ///     将字符串转换为decimal
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ToDecimal(string str, decimal defValue)
        {
            decimal outValue;
            if (!String.IsNullOrEmpty(str))
            {
                if (decimal.TryParse(str, out outValue))
                {
                    return outValue;
                }
            }
            return defValue;
        }

        #endregion

        #region 取Web.Config值

        /// <summary>
        ///     取Web.Config值
        /// </summary>
        /// <param name="KeyName"></param>
        /// <returns></returns>
        public static string ConfigKey(string KeyName)
        {
            return ConfigurationManager.AppSettings[KeyName];
        }

        /// <summary>
        ///     取wenconfig数据库连接字符串的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ConfigConnectionStrings(string name)
        {
            return ConfigurationManager.ConnectionStrings[name] == null ? "" : ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        #endregion

        #region 转换DateTime

        /// <summary>
        ///     将字符串转换为DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(String str, DateTime defValue)
        {
            DateTime outValue;

            if (!String.IsNullOrEmpty(str))
            {
                if (DateTime.TryParse(str, out outValue))
                {
                    return outValue;
                }
            }

            return defValue;
        }

        /// <summary>
        ///     将对象转换为DateTime
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="defValue">The def value.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object obj, DateTime defValue)
        {
            if (obj != null)
            {
                defValue = ToDateTime(obj.ToString(), defValue);
            }

            return defValue;
        }

        #endregion

        #region 过滤掉字符串中所有html元素

        public static string CutAllHtmlElement(string InputStr)
        {
            return Regex.Replace(InputStr, @"<[^>]+>", "");
        }

        public static string CutAllHtmlElement_All(string InputStr)
        {
            return Regex.Replace(InputStr, @"<([\s\S]*?)>", "");
        }

        #endregion

        #region 返回符合正则式的结果集

        public static ArrayList GetLinks(string inputstr, string Regexstr)
        {
            return GetLinks(inputstr, Regexstr, false);
        }

        public static ArrayList GetLinks(string inputstr, string Regexstr, bool IgnoreRepeated)
        {
            var compArray = new ArrayList();
            var myArray = new ArrayList();
            var mymatch = Regex.Match(inputstr, Regexstr, RegexOptions.IgnoreCase);
            while (mymatch.Success)
            {
                var isAdd = true;
                if (IgnoreRepeated && compArray.IndexOf(mymatch.Value) > -1)
                {
                    isAdd = false;
                }
                if (isAdd)
                {
                    myArray.Add(mymatch.Value);
                }
                compArray.Add(mymatch.Value);
                mymatch = mymatch.NextMatch();
            }
            return myArray;
        }

        #endregion

        #region 去除参数值后的 0

        /// <summary>
        ///     2.0 --> 2; 2.00 --> 2; 2.220 --> 2.22; 2.00222 --> 2.00222; 0.00000 --> 0
        ///     <para>author:xuefeng</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatFloat(string value)
        {
            var temp = value;

            if (string.IsNullOrEmpty(temp))
                return value;

            if (!Regex.IsMatch(temp, @"^[+-]?\d+[.]?\d*$"))
                return value;

            if (!temp.Contains("."))
                return value;

            var len = temp.Split('.')[1].Length;

            var temps = temp.Split('.');

            for (var i = temps[1].Length - 1; i >= 0; i--)
            {
                if (temps[1].EndsWith("0"))
                    temps[1] = temps[1].Remove(i);
                else
                    i = -1;
            }
            if (temps[1].Length <= 0)
                return temps[0];
            return temps[0] + "." + temps[1];
        }

        public static bool CheckInt32(string value)
        {
            if (!Regex.IsMatch(value, @"^[+-]?\d+[.]?\d*$"))
                return false;
            return true;
        }

        #endregion

        #region CDN抓取

        /// <summary>
        ///     用于CDN缓存页面
        /// </summary>
        public static void SetPageCacheHead()
        {
            SetPageCacheHead(120);
        }

        public static void SetPageCacheHead(int minutes)
        {
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
            HttpContext.Current.Response.Cache.SetLastModified(DateTime.Now);
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddMinutes(minutes));
            HttpContext.Current.Response.Cache.SetMaxAge(new TimeSpan(0, minutes, 0));
            HttpContext.Current.Response.Cache.SetOmitVaryStar(true);
        }

        #endregion

        #region 序列化

        /// <summary>
        ///     反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (var sr = new StringReader(xml))
                {
                    var xmldes = new XmlSerializer(type);

                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {
                //Log4netService.RecordLog.RecordException("反序列化", xml, e); 
                return null;
            }
        }

        /// <summary>
        ///     序列化XML文件
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {
            var stream = new MemoryStream();
            //创建序列化对象 
            var xml = new XmlSerializer(type);
            try
            {
                //序列化对象 
                xml.Serialize(stream, obj);
            }
            catch (InvalidOperationException e)
            {
                //Log4netService.RecordLog.RecordException("序列化XML文件", obj.ToString(), e); return null;
            }
            stream.Position = 0;
            var sr = new StreamReader(stream);
            var str = sr.ReadToEnd();
            return str;
        }


        public static string SerializeWrite<T>(T instance)
        {
            var serializer = new DataContractSerializer(typeof(T));


            var st = new MemoryStream();

            serializer.WriteObject(st, instance);
            //st.Position = 0;
            // var sr = new StreamReader(st);

            // string _serializeString = sr.ReadToEnd(); ;

            //  sr.Close();
            var array = st.ToArray();
            st.Close();
            var _serializeString = Encoding.UTF8.GetString(array, 0, array.Length);
            return _serializeString;
        }

        public static object SerializeRead<T>(string url)
        {
            var serializer = new DataContractSerializer(typeof(T));

            var xml = XmlReader.Create(url);
            var o = serializer.ReadObject(xml);
            xml.Close();
            xml = null;

            return o;
        }

        //json 序列化  

        public static string ToJsJson(object item)
        {
            var serializer = new DataContractJsonSerializer(item.GetType());
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, item);
                var sb = new StringBuilder();
                sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
                return sb.ToString();
            }
        }
        //JavaScriptSerializer反序列化  
        public static T JsDeserialize<T>(string str)
        {
            var jser = new JavaScriptSerializer();
            try
            {
                var t = jser.Deserialize<T>(str);
                return t;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        //反序列化  

        public static T FromJsonTo<T>(string jsonString)
        {

           return JsonConvert.DeserializeObject<T>(jsonString);

            //var ser = new DataContractJsonSerializer(typeof(T));
            //using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            //{
            //    var jsonObject = (T)ser.ReadObject(ms);
            //    return jsonObject;
            //}
        }

        public static string ToJson<T>(object customer)
        {
           

            var ds = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream();

            ds.WriteObject(ms, customer);

            var strReturn = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return strReturn;
        }

        public static object FromJson<T>(string strJson)
        {
            var ds = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(strJson));

            return ds.ReadObject(ms);
        }

        #endregion


        #region 生存静态文件地址

        /// <summary>
        /// 获取静态文件地址
        /// 创建人：
        /// 创建时间：2014-11-07
        /// </summary>
        /// <param name="path">静态文件路径</param>
        /// <returns>详细静态文件地址</returns>
        public static string GetStaticUrl(string path)
        {
            var url = PubConstant.StaticUrl + path + "?v=" + PubConstant.CurrentVersion;
            return url;
        }
        /// <summary>
        /// 获取富文本编辑器地址
        /// 创建人：zhuzh
        /// 创建时间：2014-11-07
        /// </summary>
        /// <param name="path">静态文件路径</param>
        /// <returns>详细静态文件地址</returns>
        public static string GetUMEditorUrl(string path)
        {
            var url = PubConstant.UMEditorUrl + path + "?v=" + PubConstant.CurrentVersion;
            return url;
        }

       
        #endregion

        #region 获取请求参数
        /// <summary>
        /// 创建人：徐功阳
        /// 创建时间：2015-7-21 13:59:01
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetRequestParam(string url)
        {
            Dictionary<string, string> dic = null;
            string strParam = url.Substring(url.IndexOf("?") + 1);
            string[] strs = strParam.Split('&');
            if (strs.Length > 0)
            {
                dic = new Dictionary<string, string>();
                foreach (var item in strs)
                {
                    var temp = item.Split('=');
                    if (temp != null && temp.Length > 1)
                    {
                        dic.Add(temp[0], temp[1]);
                    }

                }
            }
            return dic;
        }
        #endregion

        #region 获得当前访问者的IP
        /// <summary>
        /// 获得当前访问者的IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string Ip = string.Empty;
            if (HttpContext.Current != null)
            {
                // 穿过代理服务器取远程用户真实IP地址
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
                    {
                        if (HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
                        {
                            Ip = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].ToString();
                        }
                        else
                        {
                            if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                            {
                                Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                            }
                            else
                            {
                                Ip = "";
                            }
                        }
                    }
                    else
                    {
                        Ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    }
                }
                else if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                {
                    Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
                else
                {
                    Ip = "";
                }
            }

            return Ip;

        }
        #endregion


        #region HTTP请求---反向代理的备用
        /// <summary>
        /// HTTP请求---反向代理的备用
        /// date:2015-9-23
        /// user:huyf
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="param">参数</param>
        /// <param name="fileByte">文件流</param>
        /// <returns></returns>
        public static string ImamgePost(string url, Dictionary<object, object> param, byte[] fileByte)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.89 Safari/537.36";
            wr.Method = "POST";
            wr.KeepAlive = false;
            wr.Timeout = 30000;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            Stream rs = wr.GetRequestStream();
            string responseStr = null;

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in param.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, param[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);
            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "Filedata", "233.jpg", "image/png");//image/jpeg  //image/png
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            rs.Write(fileByte, 0, fileByte.Length);

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                responseStr = reader2.ReadToEnd();
            }
            catch
            {
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
            return responseStr;
        }



        #endregion
    }
    public static class ConvertTools<TEntity> where TEntity : class
    {
        /// <summary>
        ///     转换实体
        ///     创建人：zhuzh
        ///     创建时间：2015-01-22
        /// </summary>
        /// <typeparam name="T">被转换实体</typeparam>
        /// <param name="t">需转换实体数据</param>
        /// <returns>Entity操作实体</returns>
        public static TEntity ConvertToEntity<T>(T t) where T : class
        {
            TEntity entity = null;
            try
            {
                if (t != null)
                {
                    var data = JsonConvert.SerializeObject(t);
                    entity = JsonConvert.DeserializeObject<TEntity>(data);
                }
            }
            catch
            {
                entity = null;
            }

            return entity;
        }
    }
}