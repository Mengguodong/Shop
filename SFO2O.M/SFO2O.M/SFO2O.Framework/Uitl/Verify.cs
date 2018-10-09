using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Data;
using System.IO;
using System.Text;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net;


namespace SFO2O.Framework.Uitl
{
    /// <summary>
    /// 系统中的各种合法性验证函数
    /// </summary>
    public static class Verify
    {
        /// <summary>
        /// 判断一个输入字符串是否合法的商品ID，合法的商品ID是全数字或者“数字_数字”
        /// </summary>
        /// <param name="inStr">输入的字符串</param>
        /// <returns>true表示合法</returns>
        public static bool isValidProductID(string inStr)
        {
            if (string.IsNullOrWhiteSpace(inStr) || inStr.Length < 6)
                return false;

            if (inStr.IndexOf('_') == -1)
                return isAllNumber(inStr);
            else
                return (isAllNumber(inStr.Substring(0, inStr.IndexOf('_'))) && isAllNumber(inStr.Substring(inStr.IndexOf('_') + 1)));
        }
        /// <summary>
        /// True或者TRUE会被认为是true，其他的都是false 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="default_value"></param>
        public static double ZParseDouble(object obj, double default_value)
        {
            if (obj == null)
                return default_value;

            string s = Convert.ToString(obj);
            double x = 0.0;
            if (double.TryParse(s, out x))
                return x;

            return default_value;
        }
        public static DateTime LTParseDateTime(object obj)
        {
            DateTime result = DateTime.MaxValue;
            DateTime.TryParse(Convert.ToString(obj), out result);
            return result;
        }
        /// <summary>
        /// 判断一个输入字符串是不是一个手机号
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>返回true表示合法</returns>
        public static bool isValidMobile(string inStr)
        {
            Regex rTel = new Regex(@"(^(\+86)?1[3,4,5,7,8](\d{9})$)");
            Match mTel = rTel.Match(inStr);
            if (mTel.Success)
                return true;
            else
                return false;
        }

      
   
        public static void Export2Excel(DataTable dtTemp, string fileName, bool append = false, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            using (var writer = new StreamWriter(fileName, append, encoding))
            {
                foreach (DataColumn dc in dtTemp.Columns)
                {
                    writer.Write(dc.ToString().Replace("\r", "").Replace("\n", "").Replace("\t", ""));
                    writer.Write('\t');
                }
                writer.Write("\r\n");
                var values = new object[dtTemp.Columns.Count];
                foreach (DataRow drTemp in dtTemp.Rows)
                {
                    values = drTemp.ItemArray;
                    foreach (var t in values)
                    {
                        writer.Write(t.ToString().Replace("\r", "").Replace("\n", "").Replace("\t", ""));
                        writer.Write('\t');
                    }
                    writer.Write("\r\n");
                }
                writer.Write("\r\n");
                writer.Close();
            }
        }
        /// <summary>
        /// 获得一个DataTable的某一页，分页用的，PageNum从0开始
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="PageNum"></param>
        /// <param name="PageStep"></param>
        /// <returns></returns>
        public static DataTable GetDTPage(DataTable dt_in, int PageNum, int PageStep)
        {
            if (dt_in == null)
                return null;

            DataTable dt = dt_in.Clone();
            //for (int i = PageNum * PageStep; i < dt_in.Rows.Count && i < (PageNum + 1) * PageStep; i++)
            for (int i = PageNum * PageStep; i < dt_in.Rows.Count && i < (PageNum+1 ) * PageStep; i++)
            {
                dt.ImportRow(dt_in.Rows[i]);
            }

            return dt;

        }
        /// <summary>
        /// 重新对一个DT的列排序，必须确认这些列都存在。。。
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="target_columns"></param>
        public static void DTReArrangeColumns(DataTable dt, string[] target_columns)
        {
            for (int i = 0; i < target_columns.Length; i++)
            {
                if (dt.Columns[target_columns[i].ToString()] == null)
                    continue;

                dt.Columns[target_columns[i].ToString()].SetOrdinal(i);
            }
        }


        
        /// <summary>
        /// 从一个dt中选择一系列columns，注意，如果输入的column不存在，则不可能选上
        /// </summary>
        /// <param name="dt_in"></param>
        /// <param name="ColumnFilter"></param>
        /// <returns></returns>
        public static void CutAndRearrangeDTColumns(DataTable dt_in, string[] ColumnFilter)
        {
            if (dt_in == null)
                return;


            Hashtable ht_filter = new Hashtable();
            for (int i = 0; i < ColumnFilter.Length; i++)
            {
                string one_column = ColumnFilter[i].ToUpper();
                if (!ht_filter.Contains(one_column))
                    ht_filter.Add(one_column, i);
            }

            for (int i = 0; i < dt_in.Columns.Count; i++)
            {
                string this_column = dt_in.Columns[i].ColumnName.ToUpper();
                if (!ht_filter.Contains(this_column))
                    dt_in.Columns.RemoveAt(i--);
            }

            foreach (DictionaryEntry de in ht_filter)
            {
                string one_column = (string)de.Key;
                int this_ordial = (int)de.Value;

                if (dt_in.Columns.Contains(one_column))
                {
                    dt_in.Columns[one_column].ColumnName = one_column;
                    dt_in.Columns[one_column].SetOrdinal(this_ordial);
                }
            }
        }
        /// <summary>
        /// 判断一个输入字符串是否合法的电话号码（只能含有数字，空格，括号，减号）
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isValidPhoneNumber(string inStr)
        {
            if (inStr == null || inStr.Trim().Length == 0)
                return false;

            for (int i = 0; i < inStr.Length; i++)
            {
                if (!IsNumber(inStr, i) && inStr[i] != ' ' && inStr[i] != '-' && inStr[i] != '(' && inStr[i] != ')' && inStr[i] != '（' && inStr[i] != '）')
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 将一个DT的一列进行翻译
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="column_name"></param>
        /// <param name="translate_table"></param>
        public static void DTTranslateColumn(DataTable dt, string column_name, Hashtable translate_table)
        {
            if (!dt.Columns.Contains(column_name))
                return;

            //int orig_column_pos = dt.Columns[column_name].Ordinal;

            dt.Columns.Add(new DataColumn("__TEMP_CONVERT", typeof(string)));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][column_name] == DBNull.Value)
                    continue;

                string orig_id = dt.Rows[i][column_name].ToString().Trim();

                if (translate_table.Contains(orig_id))
                    dt.Rows[i]["__TEMP_CONVERT"] = translate_table[orig_id].ToString();
                else
                    dt.Rows[i]["__TEMP_CONVERT"] = "未知";
            }
            dt.Columns.Remove(dt.Columns[column_name]);
            dt.Columns["__TEMP_CONVERT"].ColumnName = column_name;

            // dt.Columns[column_name].SetOrdinal(orig_column_pos); //恢复原先位置
        }

              //生成一个简单的返回信息
        public static ArrayList MakeMSG(string msg)
        {
            ArrayList al = new ArrayList();
            al.Add(msg);
            return al;
        }


          /// <summary>
        /// 从一个dt中选择一系列columns，注意，如果输入的column不存在，则不可能选上
        /// </summary>
        /// <param name="dt_in"></param>
        /// <param name="ColumnFilter"></param>
        /// <returns></returns>
        public static DataTable GetDTColumns(DataTable dt_in, string[] ColumnFilter)
        {
            if (dt_in == null)
                return null;

            DataTable dt = dt_in.Copy();


            for (int i = 0; i < dt.Columns.Count; i++)
            {
                bool column_selected = false;

                for (int j = 0; j < ColumnFilter.Length; j++)
                {
                    if (dt.Columns[i].ColumnName.ToString().ToLower() == ColumnFilter[j].ToLower())
                    {
                        dt.Columns[i].ColumnName = ColumnFilter[j];
                        column_selected = true;
                    }
                }

                if (!column_selected)
                    dt.Columns.Remove(dt.Columns[i--].ColumnName);

            }

            return dt;
        }
        /// <summary>
        /// 判断一个输入字符串是否是一个合法的浮点数
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isValidDouble(string inStr)
        {
            if (inStr == null || inStr.Trim().Length == 0)
                return false;

            try
            {
                double ddd = double.Parse(inStr.Trim());
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 对于一些输入，将其中可能造成问题的字符替换为下横线，目前仅包括单引号
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        /// 
        private static string _SpaceMatcher(Match m)
        {
            GroupCollection gc = m.Groups;
            return null;
        }

        private static string[] spacePattern = new string[] { 
            null,   //默认的
            @"(\s{2,})"   //字符之间有多于两个的空格
        };


        private static string normalizeSpace(string inStr, int normalizeSpaceStatus)
        {
            string pattern = spacePattern[normalizeSpaceStatus] ?? null;
            if (pattern == null)
            {
                return inStr;
            }
            string outStr = Regex.Replace(inStr, pattern, " ");

            return outStr;
        }


        public static string normalizeString(string inStr)
        {
            return normalizeString(inStr, 0);
        }

        public static string normalizeString(string inStr, int normalizeSpaceStatus)
        {
            if (inStr == null || inStr.Trim().Length == 0)
                return inStr;

            inStr = normalizeSpace(inStr, normalizeSpaceStatus);

            string new_str = "";
            for (int i = 0; i < inStr.Length; i++)
            {
                if (inStr[i] == '\'')
                    new_str += '_';
                else
                    new_str += inStr[i];
            }

            return new_str;
        }

        /// <summary>
        /// 判断一个输入字符串是否完全是数字，null和空字符串会返回false
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>true表示合法</returns>
        public static bool isAllNumber(string inStr)
        {
            if (inStr == null || inStr.Length == 0)
                return false;

            for (int i = 0; i < inStr.Length; i++)
            {
                if (!IsNumber(inStr, i))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 判断一个输入字符串是否完全是数字或者字母，null和空字符串会返回false
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>true表示合法</returns>
        public static bool IsAllNumberOrLetter(string inStr)
        {
            if (inStr == null || inStr.Length == 0)
                return false;

            for (int i = 0; i < inStr.Length; i++)
            {
                if (!IsLetterOrNumber(inStr[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 判断一个字符串中的某个字符是不是数字
        /// </summary>
        /// <param name="s"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool IsNumber(string s, int i)
        {
            char c = s.ToCharArray()[i];
            return IsNumber(c);
        }

        /// <summary>
        /// 判断一个字符是否数字或者字符，字符只允许a-z和A-Z
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsLetterOrNumber(char c)
        {
            if (IsNumber(c) || IsLetter(c))
                return true;

            return false;
        }

        /// <summary>
        /// 判断是否是数字，跟char.isnumber不同，它不认为中文数字是合法数字
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsNumber(char c)
        {
            if (c < '0' || c > '9')
                return false;

            return true;
        }

        /// <summary>
        /// 判断是否a-Z的letter
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsLetter(char c)
        {
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                return true;

            return false;
        }

        /// <summary>
        /// 判断一个输入字符串是否是合法的Double类型
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>true表示合法</returns>
        public static bool isDouble(string inStr)
        {
            try
            {
                Double.Parse(inStr);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断一个输入字符串是否是合法的Int32类型
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>true表示合法</returns>
        public static bool isInt32(string inStr)
        {
            try
            {
                Int32.Parse(inStr);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断一个输入字符串是否是一个合法的邮编
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isValidPostalCode(string inStr)
        {
            if (inStr == null || inStr.Length == 0)
                return false;

            return ((inStr.Trim().Length == 6) && isAllNumber(inStr));
        }

        /// <summary>
        /// 判断一个输入字符串是否是合法的Int64类型
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>true表示合法</returns>
        public static bool isInt64(string inStr)
        {
            try
            {
                Int64.Parse(inStr);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断一个字符串是否是合法的用户账号
        /// </summary>
        /// <param name="inStr">输入字符串，用户账号</param>
        /// <returns>返回true表示合法</returns>
        //public static bool isValidUserLogin(string inStr)
        //{
        //    if (!isValidChar(inStr))
        //        return false;

        //    return (isValidEmail(inStr) || isValidMobile(inStr) || isAllNumber(inStr));
        //}

        ///// <summary>
        ///// 判断一个字符串是否是合法的用户账号，上面的isValidUserLogin里加个isAllNumber是什么意思？
        ///// </summary>
        ///// <param name="inStr"></param>
        ///// <returns></returns>
        //public static bool isValidUserLogin_New(string inStr)
        //{
        //    if (!isValidChar(inStr))
        //        return false;

        //    return (isValidEmail(inStr) || isValidMobile(inStr));
        //}

        /// <summary>
        /// 得到某个时间的目录
        /// </summary>
        /// <param name="repository_time"></param>
        /// <returns></returns>
        public static string GetSetRepositoryDir(DateTime repository_time)
        {
            string this_year_str = repository_time.Year.ToString("D4");
            string this_month_str = repository_time.Month.ToString("D2");
            string this_day_str = repository_time.Day.ToString("D2");

            string dir_base = ConfigurationManager.AppSettings["SharePath"].ToString() + "/img/";//System.Web.HttpContext.Current.Server.MapPath("/img/");
            string year_dir = dir_base + this_year_str + "\\";
            string month_dir = year_dir + this_month_str + "\\";
            string day_dir = month_dir + this_day_str + "\\";

            if (!Directory.Exists(year_dir))   ///这里将来要加一些容错
                Directory.CreateDirectory(year_dir);

            if (!Directory.Exists(month_dir))
                Directory.CreateDirectory(month_dir);

            if (!Directory.Exists(day_dir))
                Directory.CreateDirectory(day_dir);

            
            return "/img/" + this_year_str + "/" + this_month_str + "/" + this_day_str + "/";
        }
        /// <summary>
        /// 返回一个随机数字类型的字符串，最长为20位，以非0开头
        /// </summary>
        /// <param name="random_length">要返回的字符串长度，最长为20，如果长度不符合要求，返回null</param>
        /// <returns></returns>
        public static string GetRandomNum(int random_length)
        {
            return GetRandomNum(random_length, false);

        }
        /// <summary>
        /// 允许0开头，如果allow_zero是true
        /// </summary>
        /// <param name="random_length"></param>
        /// <param name="alow_zero"></param>
        /// <returns></returns>
        public static string GetRandomNum(int random_length, bool allow_zero)
        {
            int i, p;
            string r_string;

            if (random_length <= 0 || random_length > 20)
                return null;

            Random rd = GetRandomSeed();

            //再用一个随机因子，时间。。。
            long tick = DateTime.Now.Ticks;
            Random rd2 = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            if (!allow_zero)
                while ((p = GetNextRandDigit(rd, rd2)) == 0)
                { }
            else
                p = GetNextRandDigit(rd, rd2);

            r_string = p.ToString();

            for (i = 0; i < random_length - 1; i++)
            {
                p = GetNextRandDigit(rd, rd2);
                r_string += p.ToString();
            }

            return r_string;
        }
        /// <summary>
        /// 为更强的随机数准备的
        /// </summary>
        /// <param name="rd1"></param>
        /// <param name="rd2"></param>
        /// <returns></returns>
        private static int GetNextRandDigit(Random rd1, Random rd2)
        {
            return (rd1.Next(10) + rd2.Next(10)) % 10;
        }
        /// <summary>
        /// 获取一个长整数，可以是负值
        /// </summary>
        /// <returns></returns>
        public static Int64 GetRandomInt64()
        {
            return GetRandomInt64(true);
        }

        /// <summary>
        /// 生成一个随机长整数,allow_negative决定是否可以是负值
        /// </summary>
        /// <returns></returns>
        public static Int64 GetRandomInt64(bool allow_negative)
        {
            byte[] bytes = new byte[8];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);

            if (allow_negative)
                return BitConverter.ToInt64(bytes, 0);
            else
                return Math.Abs(BitConverter.ToInt64(bytes, 0));
        }

        /// <summary>
        /// 生成随机的字母（小写英文字母）
        /// </summary>
        /// <param name="RanNum">生成字母的个数</param>
        /// <returns>string</returns>
        public static string GetRandomString(int RanNum)
        {
            //定义用于生成分享联盟用户名
            char[] AllArray = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            StringBuilder randomcode = new StringBuilder();
            Random rd = GetRandomSeed();
            //生成4位验证码字符串
            for (int i = 0; i < RanNum; i++)
            {
                randomcode.Append(AllArray[rd.Next(AllArray.Length)]);
            }
            return randomcode.ToString();

        }
        /// <summary>
        /// 获取一个真正的随机种子，长度32位
        /// </summary>
        /// <returns></returns>
        public static Random GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return new Random(BitConverter.ToInt32(bytes, 0));
        }

        /// <summary>
        /// 不存在就创建
        /// </summary>
        /// <param name="dir_path"></param>
        /// <returns></returns>
        private static int TrySetDir(string dir_path)
        {
            if (Directory.Exists(dir_path))
                return 1;

            DirectoryInfo di = null;
            try
            {
                di = Directory.CreateDirectory(dir_path);
            }
            catch
            {
                return 0;
            }

            if (di == null)
                return 0;

            return 1;
        }
        /// <summary>
        /// 判断一个输入是否合法的用户密码
        /// </summary>
        /// <param name="inStr">输入密码</param>
        /// <returns>true表示合法，false不合法</returns>
        public static bool isValidUserPassword(string inStr)
        {
            if (inStr == null)
                return false;

            if (inStr.Length > 32 || inStr.Length < 3)
                return false;

            return isValidChar(inStr);
        }

        /// <summary>
        /// 判断一个字符串是否合法的UnionID
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isValidUnionName(string inStr)
        {
            if (inStr == null || inStr.Trim() == "")
                return false;

            if (inStr.IndexOf("%") != -1 || inStr.IndexOf(" ") != -1)
                return false;

            return isValidInputString(inStr);
        }

        /// <summary>
        /// 一个更为安全的判断字符串是否合法的方法
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool isSafeString(string inStr)
        {
            if (inStr == null || inStr.Trim() == "")
                return true;

            if (inStr.IndexOf("'") != -1)
                return false;

            return isValidInputString(inStr);
        }

        /// <summary>
        /// 判断一个输入串含有可疑的注入词，公有方法
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>true表示都合法，false表示有，不合法</returns>
        public static bool isValidInputString(string inStr)
        {
            if (inStr == null || inStr.Length == 0 || inStr.Trim() == "")
                return true;

            string Sql_1 = "exec|insert+|insert|select+|delete+|delete|update+|master+|master|truncate|declare|drop+|drop|drop+table|table";
            string[] sql_c = Sql_1.Split('|');

            foreach (string sl in sql_c)
            {
                if (inStr.ToLower().IndexOf(sl.Trim()) >= 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 判断一个输入串是否都是合法字符，私有方法
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>true表示都合法，false表示有不合法</returns>
        private static bool isValidChar(string inStr)
        {
            const string invalid_char = " |,|;|\"|'|[|]|{|}|<|>|(|)";

            if (inStr == null)
                return false;

            foreach (string str_t in invalid_char.Split('|'))
            {
                if (inStr.IndexOf(str_t) > -1)
                    return false;
            }
            return true;
        }

        ///// <summary>
        ///// 判断一个输入字符串是不是一个手机号
        ///// </summary>
        ///// <param name="inStr">输入字符串</param>
        ///// <returns>返回true表示合法</returns>
        //public static bool isValidMobile(string inStr)
        //{
        //    Regex rTel = new Regex(LTConfig.strMobile);
        //    Match mTel = rTel.Match(inStr);

        //    if (mTel.Success)
        //        return true;
        //    else
        //        return false;
        //}

        ///// <summary>
        ///// 判断一个输入字符串是不是一个座机号
        ///// </summary>
        ///// <param name="inStr">输入字符串</param>
        ///// <returns>返回true表示合法</returns>
        //public static bool isValidTelephone(string inStr)
        //{
        //    Regex rTel = new Regex(LTConfig.strTelephone);
        //    Match mTel = rTel.Match(inStr);

        //    if (mTel.Success)
        //        return true;
        //    else
        //        return false;
        //}

        ///// <summary>
        ///// 判断一个输入字符串是不是一个合法的邮件地址
        ///// </summary>
        ///// <param name="inStr">输入字符串</param>
        ///// <returns>返回true表示合法</returns>
        //public static bool isValidEmail(string inStr)
        //{
        //    Regex r = new Regex(LTConfig.strEmail);
        //    Match mEmail = r.Match(inStr);

        //    if (mEmail.Success)
        //        return true;
        //    else
        //        return false;
        //}

        public static bool IsValidIpAddress(string ipString)
        {
            bool returnValue = true;
            if (string.IsNullOrWhiteSpace(ipString))
                return false;
            if (!isSafeString(ipString))
                return false;
            string[] ipList = ipString.Split(new char[] { '.' });
            if (ipList.Length == 4 || ipList.Length == 6)
            {
                foreach (string item in ipList)
                {
                    if (isAllNumber(item))
                    {
                        int i = Convert.ToInt32(item);
                        if (i >= 0 || i <= 255)
                            continue;
                        else
                        {
                            returnValue = false;
                            break;
                        }
                    }
                    else
                    {
                        returnValue = false;
                        break;
                    }
                }
            }
            else
            {
                return false;
            }
            return returnValue;
        }
        /// <summary>
        /// 如果direct_output为true，则直接将DT输出到Response.Write里去而不
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="direct_output"></param>
        /// <returns></returns>
        public static void DT2CSV_Output(DataTable dt)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {

                if (dt.Columns[i].Caption.ToString().Trim() != "")
                    System.Web.HttpContext.Current.Response.Write(dt.Columns[i].Caption.ToString().Trim());
                else
                    System.Web.HttpContext.Current.Response.Write(dt.Columns[i].ColumnName);
                if (i < dt.Columns.Count - 1)
                    System.Web.HttpContext.Current.Response.Write(",");
            }

            System.Web.HttpContext.Current.Response.Write("\n");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string column_content = dt.Rows[i][dt.Columns[j].ColumnName].ToString().Trim(' ');

                    column_content.Replace("\"", "\"\"");

                    if (column_content.IndexOf(",") != -1 || column_content.IndexOf("\n") != -1)
                        column_content = "\"" + column_content + "\"";

                    System.Web.HttpContext.Current.Response.Write(column_content);

                    if (j < dt.Columns.Count - 1)
                        System.Web.HttpContext.Current.Response.Write(",");
                }

                System.Web.HttpContext.Current.Response.Write("\n");
            }

        }

        /// <summary>
        /// 验证网址
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsUrl(string source)
        {
            return Regex.IsMatch(source, @"^(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?$", RegexOptions.IgnoreCase);
        }
             
        /// <summary>
        /// 判断一个对象是否是一组对象之一
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool checkMember(object x, object[] y)
        {
            if (x == null)
                return false;

            for (int i = 0; i < y.Length; i++)
            {
                if (x.ToString() == y[i].ToString())
                    return true;
            }

            return false;
        }
       
        /// <summary>
        /// 字符串中是否含有中文
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static bool isLiveChinese(string words)
        {
            string TmmP;
            for (int i = 0; i < words.Length; i++)
            {
                TmmP = words.Substring(i, 1);
                byte[] sarr = System.Text.Encoding.GetEncoding("gb2312").GetBytes(TmmP);
                if (sarr.Length == 2)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 验证是否是合法的COID
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public static bool IsValidCOID(string co_id)
        {
            if (string.IsNullOrEmpty(co_id))
                return false;

            Regex reg = new Regex(@"^[1,2]\d{11}$");
            return reg.IsMatch(co_id);
        }

        /// <summary>
        /// 验证是否是合法的SOID
        /// </summary>
        /// <param name="so_id"></param>
        /// <returns></returns>
        public static bool IsValidSOID(string so_id)
        {
            if (string.IsNullOrEmpty(so_id))
                return false;

            Regex reg = new Regex(@"^3\d{11}$");
            return reg.IsMatch(so_id);
        }

        /// <summary>
        /// 验证是否是合法的鞋码，如41.5
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool IsShoeSize(string inStr)
        {
            if (string.IsNullOrEmpty(inStr))
                return false;

            Regex reg = new Regex(@"^[1-4]\d(\.5)?$");
            return reg.IsMatch(inStr);
        }

        /// <summary>
        /// 验证是否是合法的鞋码，多个鞋码用'/'分割
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static bool IsMoreShoeSize(string inStr)
        {
            if (string.IsNullOrEmpty(inStr))
                return false;

            Regex reg = new Regex(@"^([1-4]\d(\.5)?\/)*[1-4]\d(\.5)?$");
            return reg.IsMatch(inStr);
        }



        /// <summary>
        /// 验证是否包含黑词
        /// </summary>
        /// <param name="text">需验证的字符串</param>
        /// <returns>true表示有黑词</returns>
        public static bool HasBadWords(string text)
        {
            return keyWordFilter.HasBadWords(text);
        }
        private static KeyWordFilter keyWordFilter = new KeyWordFilter(HttpContext.Current.Server.MapPath("~/App_Data/badwords.xml"));
    }

    /// <summary>
    /// 黑词过滤器
    /// </summary>
    public class KeyWordFilter
    {
        /// <summary>
        /// 存储所有黑词的首个字
        /// </summary>
        private BitArray HEAD_CHAR_CHECK = new BitArray(char.MaxValue);
        /// <summary>
        /// 存储所有黑词出现的字
        /// </summary>
        private BitArray ALL_CHAR_CHECK = new BitArray(char.MaxValue);
        /// <summary>
        /// 黑词最大长度，默认为0
        /// </summary>
        private int MAX_WORD_LENGTH = 0;
        /// <summary>
        /// 黑词列表内存
        /// </summary>
        public HashSet<string> BADWORD_LIST = new HashSet<string>();
        /// <summary>
        /// 黑词库文件名
        /// </summary>
        private string xmlFileName = "";
        /// <summary>
        /// 干扰字符
        /// </summary>
        private char[] NOISE_WORDS = { ' ', '\r', '\n' };
        /// <summary>
        /// 存储干扰字符
        /// </summary>
        private BitArray NOISE_CHAR_CHECK = new BitArray(char.MaxValue);

        public KeyWordFilter(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                return;

            xmlFileName = filename;
            XmlTextReader xmlReader = new XmlTextReader(xmlFileName);

            try
            {
                while (xmlReader.Read())
                    if (xmlReader.NodeType == XmlNodeType.Text && !BADWORD_LIST.Contains(xmlReader.Value))
                        add(xmlReader.Value);

                foreach (char c in NOISE_WORDS)
                    NOISE_CHAR_CHECK[c] = true;
            }
            catch { }

            xmlReader.Close();
        }

        /// <summary>
        /// 黑词列表增加
        /// </summary>
        /// <param name="word"></param>
        private void add(string word)
        {
            if (String.IsNullOrEmpty(word))
                return;

            BADWORD_LIST.Add(word);
            MAX_WORD_LENGTH = Math.Max(MAX_WORD_LENGTH, word.Length);
            HEAD_CHAR_CHECK[word[0]] = true;

            foreach (char c in word)
                ALL_CHAR_CHECK[c] = true;
        }

        /// <summary>
        /// 快速判断是否包括黑词
        /// </summary>
        /// <param name="text">需检验的内容</param>
        /// <returns>包含返回true,否则返回false</returns>
        public bool HasBadWords(string text)
        {
            if (String.IsNullOrEmpty(text))
                return false;

            int index = 0;
            while (index < text.Length)
            {
                if (!HEAD_CHAR_CHECK[text[index]])
                    while (index < text.Length - 1 && !HEAD_CHAR_CHECK[text[++index]])
                        ;

                int N = Math.Min(MAX_WORD_LENGTH, text.Length - index);
                for (int j = 1; j <= N; j++)
                {
                    if (index + j - 1 >= text.Length)
                        break;

                    if (NOISE_CHAR_CHECK[text[index + j - 1]])  //跳过干扰字符
                    {
                        N++;
                        continue;
                    }

                    if (!ALL_CHAR_CHECK[text[index + j - 1]])
                        break;

                    string bad = text.Substring(index, j);
                    foreach (char c in NOISE_WORDS)             //去掉干扰字符，找出黑词本尊
                        bad = bad.Replace(c.ToString(), "");

                    if (BADWORD_LIST.Contains(bad))
                        return true;
                }

                index++;
            }

            return false;
        }

        /// <summary>
        /// 查询是否包括黑词详细信息
        /// </summary>
        /// <param name="text">需检验的内容</param>
        /// <returns>返回黑词出现次数信息</returns>
        public Hashtable HasBadWordsInfo(string text)
        {
            Hashtable ht = new Hashtable();
            if (String.IsNullOrEmpty(text))
                return ht;

            int index = 0;
            while (index < text.Length)
            {
                if (!HEAD_CHAR_CHECK[text[index]])
                    while (index < text.Length - 1 && !HEAD_CHAR_CHECK[text[++index]])
                        ;

                int N = Math.Min(MAX_WORD_LENGTH, text.Length - index);
                for (int j = 1; j <= N; j++)
                {
                    if (index + j - 1 >= text.Length)
                        break;

                    if (NOISE_CHAR_CHECK[text[index + j - 1]])
                    {
                        N++;
                        continue;
                    }

                    if (!ALL_CHAR_CHECK[text[index + j - 1]])
                        break;

                    string bad = text.Substring(index, j);
                    foreach (char c in NOISE_WORDS)
                        bad = bad.Replace(c.ToString(), "");

                    if (BADWORD_LIST.Contains(bad))
                    {
                        if (ht.ContainsKey(bad))    //再次查出，次数+1
                            ht[bad] = (int)ht[bad] + 1;
                        else
                            ht.Add(bad, 1);
                    }
                }

                index++;
            }

            return ht;
        }

        /// <summary>
        /// 增加黑词
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool AddBadWord(string word)
        {
            if (String.IsNullOrEmpty(word))
                return false;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFileName);
                XmlNode xn = doc["BadWords"];

                if (xn.InnerXml.Contains(word))//判断存在
                    return false;

                XmlElement xe = doc.CreateElement("i");
                xe.InnerText = word;
                xn.AppendChild(xe);
                doc.Save(xmlFileName);

                if (!BADWORD_LIST.Contains(word))
                    add(word);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 删除黑词
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool DeleteBadWord(string word)
        {
            if (String.IsNullOrEmpty(word))
                return false;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFileName);
                XmlNode xn = doc["BadWords"];

                bool b = false;
                foreach (XmlNode node in xn.ChildNodes)
                {
                    if (node.InnerText == word)
                    {
                        xn.RemoveChild(node);
                        b = true;
                        break;
                    }
                }

                doc.Save(xmlFileName);

                if (BADWORD_LIST.Contains(word))
                    BADWORD_LIST.Remove(word);
                return b;
            }
            catch { return false; }
        }

        /// <summary>
        /// 修改黑词
        /// </summary>
        /// <param name="oldWord">原词</param>
        /// <param name="newWord">新词</param>
        /// <returns></returns>
        public bool ChangeBadWord(string oldWord, string newWord)
        {
            if (String.IsNullOrEmpty(oldWord) || String.IsNullOrEmpty(newWord))
                return false;

            if (oldWord == newWord) //排除无效修改
                return false;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFileName);
                XmlNode xn = doc["BadWords"];

                if (xn.InnerXml.Contains(newWord))//判断新词存在
                    return false;

                bool b = false;
                foreach (XmlNode node in xn.ChildNodes)
                {
                    if (node.InnerText == oldWord)
                    {
                        node.InnerText = newWord;
                        b = true;
                        break;
                    }
                }

                doc.Save(xmlFileName);

                if (BADWORD_LIST.Contains(oldWord))
                    BADWORD_LIST.Remove(oldWord);
                if (!BADWORD_LIST.Contains(newWord))
                    add(newWord);
                return b;
            }
            catch { return false; }
        }
    }
}
