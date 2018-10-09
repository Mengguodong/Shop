using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SFO2O.Utility.Uitl
{

    /// <summary>
    ///Encryption 的摘要说明
    /// </summary>
    public static class ZEncryption
    {
        #region DES相关方法

        //这个必须8个字符长
        private static byte[] DES_SHARED_KEY = Encoding.UTF8.GetBytes("@)!(#*$&");

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string DesEncrypt(string encryptString)
        {
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(DES_SHARED_KEY, DES_SHARED_KEY), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return BitConverter.ToString(mStream.ToArray()).Replace("-", "");
            //return Convert.ToBase64String();
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DesDecrypt(string decryptString)
        {
            byte[] inputByteArray = FromHexString(decryptString);               // Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(DES_SHARED_KEY, DES_SHARED_KEY), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }

        /// <summary>
        /// 从Hex字符串还原bit array
        /// </summary>
        /// <param name="bit_str"></param>
        /// <returns></returns>
        private static byte[] FromHexString(string bit_str)
        {
            StringBuilder buff = new StringBuilder(bit_str);
            for (int i = buff.Length - 2; i > 0; i -= 2)
                buff.Insert(i, '|');

            string[] arrSplit = buff.ToString().Split('|');

            byte[] byteTemp = new byte[bit_str.Length / 2];
            for (int i = 0; i < byteTemp.Length; i++)
            {
                byteTemp[i] = byte.Parse(arrSplit[i],
                System.Globalization.NumberStyles.AllowHexSpecifier);
            }

            return byteTemp;
        }

        #endregion

        #region MD5相关方法
        /// <summary>
        /// 将输入字符串用md5加密并返回base64的结果串
        /// </summary>
        /// <param name="Str">输入字符串</param>
        /// <returns>返回字符串</returns>
        public static string GetMd5String(string Str)
        {
            var byteArray = GetMD5Bytes(Str);
            return BitConverter.ToString(byteArray).Replace("-", "");
        }


        /// <summary>
        /// 将输入的流用md5加密并返回base64的结果串
        /// </summary>
        /// <param name="Str">输入字符串</param>
        /// <returns>返回字符串</returns>
        public static string GetMd5String(Stream fs)
        {
            var byteArray = GetMD5Bytes(fs);
            return BitConverter.ToString(byteArray).Replace("-", "");
        }

        /// <summary>
        /// 获取一个输入字符串的MD5后64位整数值，相当于简单的签名
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static Int64 GetMD5Int64(string Str)
        {
            var byteString = GetMD5Bytes(Str);
            return BitConverter.ToInt64(byteString, 0);
        }


        /// <summary>
        /// 获取一个输入字符串的MD5后64位整数值，相当于简单的签名
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static UInt64 GetMD5UInt64(string Str)
        {
            var byteString = GetMD5Bytes(Str);
            return BitConverter.ToUInt64(byteString, 0);
        }


        /// <summary>
        /// 获取一个输入文件流的MD5后64位整数值，相当于简单的签名
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static Int64 GetMD5Int64(Stream fs)
        {
            var byteString = GetMD5Bytes(fs);
            return BitConverter.ToInt64(byteString, 0);
        }


        /// <summary>
        /// 获取一个输入文件流的MD5后64位整数值，相当于简单的签名
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static UInt64 GetMD5UInt64(Stream fs)
        {
            var byteString = GetMD5Bytes(fs);
            return BitConverter.ToUInt64(byteString, 0);
        }



        /// <summary>
        /// 获得一个输入字符串的MD5 byte串结果
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static byte[] GetMD5Bytes(string Str)
        {
            using (MD5 md5 = MD5.Create())
            {
                var byteString = md5.ComputeHash(Encoding.UTF8.GetBytes(Str));
                return byteString;
            }
        }



        /// <summary>
        /// 获得一个文件流的md5 byte串结果
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static byte[] GetMD5Bytes(Stream fs)
        {
            MD5 md5 = MD5.Create();
            byte[] byteString = md5.ComputeHash(fs);
            md5.Dispose();

            return byteString;

        }

        #endregion

        #region SHA方法

        /// <summary>
        /// sha，到base64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ComputeSHABase64(string str)
        {
            byte[] dataHashed = ComputeSHABytes(str);
            return ZBase64.EncodeTo64(dataHashed);
        }


        /// <summary>
        /// sha，到hex
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ComputeSHAHex(string str)
        {
            byte[] dataHashed = ComputeSHABytes(str);

            //将运算结果转换成string
            return BitConverter.ToString(dataHashed).Replace("-", "");
        }

        public static byte[] ComputeSHABytes(string str)
        {
            //建立SHA1对象
            SHA1 sha = new SHA1CryptoServiceProvider();

            //将mystr转换成byte[]
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(str);

            //Hash运算
            return sha.ComputeHash(dataToHash);
        }

        #endregion

        #region HMACSHA1方法

        /// <summary>
        /// sha，到base64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ComputeHMACSHA1Base64(string key, string msg)
        {
            byte[] dataHashed = ComputeHMACSHA1Bytes(key, msg);
            return ZBase64.EncodeTo64(dataHashed);
        }


        /// <summary>
        /// sha，到hex
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ComputeHMACSHA1Hex(string key, string msg)
        {
            byte[] dataHashed = ComputeHMACSHA1Bytes(key, msg);

            //将运算结果转换成string
            return BitConverter.ToString(dataHashed).Replace("-", "");
        }


        /// <summary>
        /// 基础获取bytes的方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] ComputeHMACSHA1Bytes(string key, string msg)
        {
            HMACSHA1 hmac = new HMACSHA1();
            hmac.Key = Encoding.ASCII.GetBytes(key);
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(msg));

        }
        #endregion

    }

}