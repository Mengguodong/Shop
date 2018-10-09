using System.Net;

namespace SFO2O.Framework.Uitl
{
    public class ZBase64
	{
        /// <summary>
        /// 对输入byte串编码 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string EncodeTo64(byte[] b)
        {
            return System.Convert.ToBase64String(b);
        }

        /// <summary>
        /// 对一个输入字符串进行base64编码
        /// </summary>
        /// <param name="toEncode"></param>
        /// <returns></returns>
        public static string EncodeTo64(string toEncode)
        {

            byte[] toEncodeAsBytes = System.Text.UTF8Encoding.UTF8.GetBytes(toEncode);

            return EncodeTo64(toEncodeAsBytes);

        }

        /// <summary>
        /// 对一个输入字符串进行base64解码
        /// </summary>
        /// <param name="encodedData"></param>
        /// <returns></returns>
        public static string DecodeFrom64(string encodedData)
        {

            byte[] encodedDataAsBytes

                = System.Convert.FromBase64String(encodedData);

            string returnValue =

               System.Text.UTF8Encoding.UTF8.GetString(encodedDataAsBytes);

            return returnValue;

        }
	}
}