using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SFO2O.EntLib.DataExtensions.Config
{
    public class CustomProtectedConfigurationProvider : ProtectedConfigurationProvider
    {
        private TripleDESCryptoServiceProvider des =
                    new TripleDESCryptoServiceProvider()
                    {
                        Key = DBKey.Key,
                        IV = DBKey.IV,
                        Mode = CipherMode.ECB
                    };

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="node">明文xml结点</param>
        /// <returns>加密后的xml结点</returns>
        public override System.Xml.XmlNode Encrypt(System.Xml.XmlNode node)
        {
            string encryptedData = EncryptString(node.OuterXml);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.LoadXml("<EncryptedData>" + encryptedData + "</EncryptedData>");

            return xmlDoc.DocumentElement;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptedNode">密文xml结点</param>
        /// <returns>解密后的xml结点</returns>
        public override System.Xml.XmlNode Decrypt(System.Xml.XmlNode encryptedNode)
        {
            string decryptedData = DecryptString(encryptedNode.InnerText);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.LoadXml(decryptedData);

            return xmlDoc.DocumentElement;
        }

        public string EncryptString(string encryptValue)
        {
            byte[] valBytes =
                Encoding.Unicode.GetBytes(encryptValue);
            ICryptoTransform transform = des.CreateEncryptor();

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,
                transform, CryptoStreamMode.Write);
            cs.Write(valBytes, 0, valBytes.Length);
            cs.FlushFinalBlock();
            byte[] returnBytes = ms.ToArray();
            cs.Close();

            return Convert.ToBase64String(returnBytes);
        }

        public string DecryptString(string encryptedValue)
        {
            byte[] valBytes =
                Convert.FromBase64String(encryptedValue);
            ICryptoTransform transform = des.CreateDecryptor();

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,
                transform, CryptoStreamMode.Write);
            cs.Write(valBytes, 0, valBytes.Length);
            cs.FlushFinalBlock();
            byte[] returnBytes = ms.ToArray();
            cs.Close();

            return Encoding.Unicode.GetString(returnBytes);
        }

    }
    public class DBKey
    {
        private const string _iv = "D20E6E7B178BEEF1";
        private const string _key = "7953FF0E408864F61C1302AAD701CB481FE0409DC8C1BA63";
        public static byte[] IV = StringUtil.HexToByte("D20E6E7B178BEEF1");
        public static byte[] Key = StringUtil.HexToByte("7953FF0E408864F61C1302AAD701CB481FE0409DC8C1BA63");
    }
    internal class StringUtil
    {
        public static string ByteToHex(byte[] byteArray)
        {
            string str = "";
            foreach (byte num in byteArray)
            {
                str = str + num.ToString("X2");
            }
            return str;
        }

        public static byte[] HexToByte(string hexString)
        {
            byte[] buffer = new byte[hexString.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 0x10);
            }
            return buffer;
        }
    }
}
