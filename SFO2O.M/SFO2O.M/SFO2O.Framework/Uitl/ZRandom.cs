using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SFO2O.Framework.Uitl
{
    /// <summary>
    /// ZERP系统的随机相关函数
    /// </summary>
    public static class ZRandom
    {

        /// <summary>
        /// 获得一串只包括数字的字符串，缺省不允许0开头
        /// </summary>
        /// <param name="len">字符串的长度</param>
        /// <param name="allow_zero">是否允许以0开头</param>
        /// <returns></returns>
        public static string GetRandomNumString(uint len)
        {
            return GetRandomNumString(len, false);
        }

        /// <summary>
        /// 获得一个基于从1970-1-1到当前的秒数+六位随机数而生成的随机数
        /// 因为Javascript最长是9007199254740992
        /// 所以随机部分是6位，最终生成的是16位 
        /// </summary>
        /// <returns></returns>
        public static Int64 GetTimeStampBasedRandumNum()
        {
            UInt32 time_stamp = ZDateTime.GetUnixTimeStamp();
            string rand_num = GetRandomNumString(6, true);
            return Int64.Parse(time_stamp.ToString() + rand_num);
        }


        private static string RANDOM_STRING_BASE_LOWER = "0123456789abcdefghijkomnopqrstuvwxyz";
        private static string RANDOM_STRING_BASE_UPPER = "0123456789abcdefghijkomnopqrstuvwxyzABCDEFGHIJKOMNOPQRSTUVWXYZ";

        /// <summary>
        /// 生成一个随机字符串，包括数字和小写字母，主要是给随机文件名之类地方用的
        /// </summary>
        /// <param name="len">字符串长度</param>
        /// <returns></returns>
        public static string GetRandomString(uint len)
        {
            return GetRandomString(len, false);
        }

        /// <summary>
        /// 生成一个随机字符串，包括数字和大小写字母，主要是给随机文件名之类地方用的
        /// </summary>
        /// <param name="len"></param>
        /// <param name="allow_upper_case">允许大写字符</param>
        /// <returns></returns>
        public static string GetRandomString(uint len, bool allow_upper_case)
        {
            char[] use_base = (allow_upper_case ? RANDOM_STRING_BASE_UPPER : RANDOM_STRING_BASE_LOWER).ToCharArray();
            int base_len = use_base.Length;

            string s = "";
            for (int i = 0; i < len; i++)
            {
                int rand_pos = (int)(GetRandomUInt32() % base_len);
                s += use_base[rand_pos];
            }

            return s;
        }

        /// <summary>
        /// 获得一串只包括数字的字符串
        /// </summary>
        /// <param name="len">字符串的长度，为0返回""</param>
        /// <param name="allow_zero">是否允许以0开头</param>
        /// <returns></returns>
        public static string GetRandomNumString(uint len, bool allow_zero)
        {
            if (len == 0)
                return ""; //长度为0返回空

            uint first_num = allow_zero ? GetRandomUInt32() % 10 : (GetRandomUInt32() % 9 + 1);
            string r_string = first_num.ToString();

            for (int i = 1; i < len; i++)
            {
                uint next_num = GetRandomUInt32() % 10;

                r_string += next_num.ToString();
            }

            return r_string;
        }


        /// <summary>
        /// 生成一个16位随机数
        /// </summary>
        /// <returns></returns>
        public static Int16 GetRandomInt16()
        {
            byte[] bytes = GetRandomBytes(2);
            return BitConverter.ToInt16(bytes, 0);
        }


        /// <summary>
        /// 生成一个16位正随机数
        /// </summary>
        /// <returns></returns>
        public static UInt16 GetRandomUInt16()
        {
            byte[] bytes = GetRandomBytes(2);
            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        /// 生成一个32位随机数
        /// </summary>
        /// <returns></returns>
        public static Int32 GetRandomInt32()
        {
            byte[] bytes = GetRandomBytes(4);
            return BitConverter.ToInt32(bytes, 0);

        }

        /// <summary>
        /// 生成一个32位正随机数
        /// </summary>
        /// <returns></returns>
        public static UInt32 GetRandomUInt32()
        {
            byte[] bytes = GetRandomBytes(4);
            return BitConverter.ToUInt32(bytes, 0);

        }

        /// <summary>
        /// 生成一个64位随机整数
        /// </summary>
        /// <returns></returns>
        public static Int64 GetRandomInt64()
        {
            byte[] bytes = GetRandomBytes(8);
            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// 生成一个64位正随机整数
        /// </summary>
        /// <returns></returns>
        public static UInt64 GetRandomUInt64()
        {
            byte[] bytes = GetRandomBytes(8);
            return BitConverter.ToUInt64(bytes, 0);
        }


        /// <summary>
        /// 获取一个随机颜色
        /// </summary>
        /// <returns></returns>
        public static Color GetRandomColor()
        {
            byte[] b = GetRandomBytes(3);
            Color color = Color.FromArgb(b[0], b[1], b[2]);
            return color;
        }

        /// <summary>
        /// 获取一个随机颜色的Hex值，例如3F8A0D，缺省不带#，如果调用时加true则自动前置一个#号
        /// 要获取System.Drawing.Colour对象，可以这样
        /// System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml("#FFCC66");
        /// </summary>
        /// <returns></returns>
        public static string GetRandomColorHex()
        {
            return GetRandomColorHex(false);
        }

        /// <summary>
        /// 获取一个随机颜色的Hex值，例如3F8A0D，缺省不带#，如果调用时加true则自动前置一个#号
        /// 要获取System.Drawing.Colour对象，可以这样
        /// System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml("#FFCC66");
        /// </summary>
        /// <returns></returns>
        public static string GetRandomColorHex(bool with_pound_sign)
        {
            char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

            byte[] b = GetRandomBytes(3);

            char[] chars = new char[b.Length * 2];
            for (int i = 0; i < b.Length; i++)
            {
                int x = b[i];
                chars[i * 2] = hexDigits[x >> 4];
                chars[i * 2 + 1] = hexDigits[x & 0xF];
            }

            string hex_code = new string(chars); ;

            if (!with_pound_sign)
                return hex_code;
            else
                return "#" + hex_code;
        }

        /// <summary>
        /// 获得一串随机的字节
        /// </summary>
        /// <returns></returns>
        private static byte[] GetRandomBytes(int num)
        {
            byte[] bytes = new byte[num];
            using (System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
                return bytes;
            }
        }
    }
}
