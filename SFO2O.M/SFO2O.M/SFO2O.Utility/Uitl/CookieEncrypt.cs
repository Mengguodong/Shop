using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SFO2O.Utility.Uitl
{
    public class CookieEncrypt
    {

        public static void SetSessionCookie(string cookie_key, string cookie_value)
        {
            string domain = HttpContext.Current.Request.Url.Host;
            string[] s = domain.Split('.');
            if (domain.Contains("milangang.com"))
                domain = s[s.Length - 2] + "." + s[s.Length - 1];
            else
                domain = "";

            HttpCookie cookie = new HttpCookie(cookie_key);
            cookie.Domain = domain;
            cookie.Name = cookie_key;
            cookie.Value =EncryptCookies(cookie_value);
            cookie.Expires = DateTime.Now.AddMinutes(ConfigHelper.SessionExpireMinutes);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        #region 对Cookies进行加密解密的方法
        /// <summary>
        /// 对Cookies进行加密的方法
        /// </summary>
        /// <param name="inputText">要加密的字符串</param>
        /// <returns></returns>
        public static string EncryptCookies(String inputText)
        {
            return Encrypt(inputText, "&%#@?,:*");
        }
        /// <summary>
        /// 对Cookies进行解密的方法
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        public static string DecryptCookies(String inputText)
        {
            return Decrypt(inputText, "&%#@?,:*");
        }
        /// <summary>
        /// 加密函数
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="strEncrKey"></param>
        /// <returns></returns>
        private static String Encrypt(String inputText, String strEncrKey)
        {
            byte[] byKey = { };
            byte[] iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                byKey = Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] inputByteArray = Encoding.UTF8.GetBytes(inputText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, iv), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 解密函数
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="sDecrKey"></param>
        /// <returns></returns>
        private static string Decrypt(string inputText, string sDecrKey)
        {
            Byte[] byKey = { };
            Byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            Byte[] inputByteArray = new byte[inputText.Length];
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(inputText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        #endregion
    }
}
