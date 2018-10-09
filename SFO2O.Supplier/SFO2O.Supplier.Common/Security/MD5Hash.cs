﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace SFO2O.Supplier.Common.Security
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
    }
}