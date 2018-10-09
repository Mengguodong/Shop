using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace SFO2O.Utility.Security
{
    public static class DES
    {
        /// <summary>
        /// DES encrpt default key
        /// </summary>
        public static readonly string DESEncryptionKey = "meydarin";

        /// <summary>
        /// DES encrpt offset
        /// </summary>
        public static readonly string DESEncryptionIV = "y3ee2h1a";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(this string text)
        {
            return Encrypt(text, DESEncryptionKey, DESEncryptionIV);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <returns></returns>
        public static string Encrypt(this string text, string encryptionKey, string encryptionIV)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            else
            {
                byte[] data = Encoding.UTF8.GetBytes(text);
                byte[] result = null;

                using (DESCryptoServiceProvider DES = new DESCryptoServiceProvider())
                {
                    DES.Key = Encoding.UTF8.GetBytes(encryptionKey);
                    DES.IV = Encoding.UTF8.GetBytes(encryptionIV);
                    ICryptoTransform desencrypt = DES.CreateEncryptor();
                    result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                }
                return BitConverter.ToString(result).Replace("-", "");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decrypt(this string text)
        {
            return Decrypt(text, DESEncryptionKey, DESEncryptionIV);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <returns></returns>
        public static string Decrypt(this string text, string encryptionKey, string encryptionIV)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            else
            {
                byte[] data = new byte[text.Length / 2];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = byte.Parse(text.Substring(2 * i, 2), NumberStyles.HexNumber);
                }

                byte[] result = null;
                using (DESCryptoServiceProvider DES = new DESCryptoServiceProvider())
                {
                    DES.Key = Encoding.UTF8.GetBytes(encryptionKey);
                    DES.IV = Encoding.UTF8.GetBytes(encryptionIV);
                    ICryptoTransform desencrypt = DES.CreateDecryptor();
                    result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                }
                return Encoding.UTF8.GetString(result);
            }
        }
    }
}
