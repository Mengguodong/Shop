using System;
using System.Security.Cryptography;
using System.Text;

namespace SFO2O.Utility.Security
{
    public static class AES
    {
        /// <summary>
        /// DES encrpt default key
        /// </summary>
        public static readonly string DESEncryptionKey = "zhsbkjyxgsbsmlgt";

        public static string AESEncrypt(this string text)
        {

            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] result = null;

            using (DESCryptoServiceProvider DES = new DESCryptoServiceProvider())
            {
                DES.Key = Encoding.UTF8.GetBytes(DESEncryptionKey);
                //DES.IV = Encoding.UTF8.GetBytes(encryptionIV);
                ICryptoTransform desencrypt = DES.CreateEncryptor();
                result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            }
            return BitConverter.ToString(result).Replace("-", "");
        }

        public static string AESDecrypt(this string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            //byte[] data = new byte[text.Length / 2];
            //for (int i = 0; i < data.Length; i++)
            //{
            //    data[i] = byte.Parse(text.Substring(2 * i, 2), NumberStyles.HexNumber);
            //}

            byte[] result = null;
            using (AesCryptoServiceProvider AES = new AesCryptoServiceProvider())
            {
                AES.Key = Encoding.UTF8.GetBytes(DESEncryptionKey);
                //AES.IV = Encoding.UTF8.GetBytes(encryptionIV);
                ICryptoTransform desencrypt = AES.CreateDecryptor();
                result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            }
            return Encoding.UTF8.GetString(result);
        }
    }
}
