using SFO2O.Utility.Uitl;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace SFO2O.Utility.Security
{
    public static class MD5Hash
    {
        private static readonly string saltVal = "";

        // Hash an input string and return the hash as
        // a 32 character hexadecimal string.
        public static string GetMD5Hash(string input)
        {
            input = input + saltVal;
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        /// <summary>
        /// 将输入字符串用md5加密并返回
        /// </summary>
        /// <param name="Str">输入字符串</param>
        /// <returns>返回字符串</returns>
        public static string GetMd5String(string Str)
        {
            //32位md5加密算法
            byte[] byteString = null;
            MD5 md5 = MD5.Create();
            byteString = md5.ComputeHash(Encoding.Unicode.GetBytes(Str));
            md5.Clear();

            string NewStr = Convert.ToBase64String(byteString);

            return NewStr;
        }

        // Verify a hash against a string.
        public static bool VerifyMD5Hash(string input, string hash)
        {
            string hashOfInput = GetMd5String(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }

        public static string GetMD5(this string myString)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(myString);
            //byte[] bytes = { 0x35, 0x24, 0x76, 0x12 };
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x2"));
            }
            return sb.ToString();
        }


        /// <summary>
        /// 生成md5校验值
        /// </summary>
        /// <param name="parameterString">参数字符串</param>
        /// <returns></returns>
        public static string GetMD5String(string parameterString)
        {
            string MD5Key = ConfigHelper.GetAppConfigString("MD5Key");

            string MD5String = CommonMd5Encrypt(Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(parameterString + MD5Key)));


            return MD5String;

        }



        #region 字符串返回MD5值(未做处理普通MD5加密)
        /// <summary>
        /// 
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
                    //sb.Append(temp[i].ToString("x"));
                }
            }
            return sb.ToString();
        }
        #endregion


        /// <summary>
        ///     加密数据
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public static string Md5Encrypt(string Text, string sKey = "winegame")
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
    }
}
