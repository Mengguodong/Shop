using SFO2O.Framework.Uitl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SFO2O.Framework
{
    public static class StringUtils
    {
        //private static readonly IAppLog log = AppLogManager.GetLogger(typeof(StringUtils));
        public static readonly int DefaultPricePrecision = 3;
        public static char[] HexDigits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        /// <summary>
        /// 截取长度
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string InputText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            text = text.Trim();
            if (text.Length > maxLength)
            {
                text = text.Substring(0, maxLength);
            }
            text = Regex.Replace(text, "[\\s]{2,}", " ");
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);
            text = text.Replace("'", "''");
            return text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CleanNonWord(string text)
        {
            return Regex.Replace(text, "\\W", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string RepeatChar(char c, int count)
        {
            string text = "";
            for (int i = 0; i < count; i++)
            {
                text += c.ToString();
            }
            return text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string RepeatChar(string c, int count)
        {
            string text = "";
            for (int i = 0; i < count; i++)
            {
                text += c;
            }
            return text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iLen"></param>
        /// <param name="strTemp"></param>
        /// <param name="fillChar"></param>
        /// <param name="bAtBefore"></param>
        /// <returns></returns>
        public static string FillStr(int iLen, string strTemp, char fillChar, bool bAtBefore)
        {
            string text = "";
            int num = strTemp.Length;
            if (num >= iLen)
            {
                return strTemp;
            }
            num = iLen - num;
            for (int i = 0; i < num; i++)
            {
                text += fillChar.ToString();
            }
            if (bAtBefore)
            {
                return text + strTemp;
            }
            return strTemp + text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canNullStr"></param>
        /// <param name="defaultStr"></param>
        /// <returns></returns>
        public static string NotNullStr(object canNullStr, string defaultStr)
        {
            if (canNullStr != null && !(canNullStr is DBNull))
            {
                return Convert.ToString(canNullStr);
            }
            if (string.IsNullOrEmpty(defaultStr))
            {
                return "";
            }
            return defaultStr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canNullStr"></param>
        /// <returns></returns>
        public static string NotNullStr(object canNullStr)
        {
            return StringUtils.NotNullStr(canNullStr, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str0"></param>
        /// <param name="str1"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool IsEqualStr(string str0, string str1, bool ignoreCase)
        {
            if (str0 == null && str1 != null)
            {
                return false;
            }
            if (str0 == null && str1 == null)
            {
                return true;
            }
            if (str0 != null && str1 == null)
            {
                return false;
            }
            if (ignoreCase)
            {
                return str0.Equals(str1, StringComparison.CurrentCultureIgnoreCase);
            }
            return str0.Equals(str1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcStr"></param>
        /// <param name="strOld"></param>
        /// <param name="strNew"></param>
        /// <returns></returns>
        public static string ReplaceStr(string srcStr, string strOld, string strNew)
        {
            if (string.IsNullOrEmpty(srcStr) || string.IsNullOrEmpty(strOld))
            {
                return srcStr;
            }
            return srcStr.Replace(strOld, strNew);
        }
        public static int StrByteLength(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return bytes.Length;
        }
        public static string LimitStrLen(string str, int maxLen)
        {
            return StringUtils.LimitStrLen(str, maxLen, true);
        }
        public static string LimitStrLen(string str, int maxLen, bool isAddDots)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (maxLen <= 0)
            {
                return str;
            }
            if (str.Length <= maxLen)
            {
                return str;
            }
            if (isAddDots)
            {
                return str.Substring(0, maxLen) + "...";
            }
            return str.Substring(0, maxLen);
        }
        public static bool IsInLimitStr(string limitStrs, string str)
        {
            return StringUtils.IsInLimitStr(limitStrs, str, ',', true);
        }
        public static bool IsInLimitStr(string limitStrs, string str, char splitChar, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(limitStrs) || string.IsNullOrEmpty(str))
            {
                return false;
            }
            string text = splitChar.ToString() + limitStrs + splitChar.ToString();
            if (ignoreCase)
            {
                return text.IndexOf(splitChar.ToString() + str + splitChar.ToString(), StringComparison.CurrentCultureIgnoreCase) >= 0;
            }
            return text.IndexOf(splitChar.ToString() + str + splitChar.ToString()) >= 0;
        }
        public static string ToXMLStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return str.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("&", "&amp;");
        }

        /// <summary>
        /// 转int 
        /// </summary>
        /// <param name="objInt">原始值</param>
        /// <param name="defaultValue">异常返回值</param>
        /// <returns></returns>
        public static int ToInt(object objInt, int defaultValue)
        {
            if (objInt == null || objInt is DBNull)
            {
                return defaultValue;
            }
            int result;
            if (!int.TryParse(objInt.ToString(), out result))
            {
                try
                {
                    int result2 = Convert.ToInt32(objInt);
                    return result2;
                }
                catch
                {
                    int result2 = defaultValue;
                    return result2;
                }
            }
            return result;
        }

        /// <summary>
        /// 转int  
        /// </summary>
        /// <param name="objInt"></param>
        /// <returns>异常返回值0</returns>
        public static int ToInt(object objInt)
        {
            return StringUtils.ToInt(objInt, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objLong"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToLong(object objLong, long defaultValue)
        {
            if (objLong == null || objLong is DBNull)
            {
                return defaultValue;
            }
            long result;
            if (!long.TryParse(objLong.ToString(), out result))
            {
                try
                {
                    long result2 = Convert.ToInt64(objLong);
                    return result2;
                }
                catch
                {
                    long result2 = defaultValue;
                    return result2;
                }
            }
            return result;
        }

        /// <summary>
        /// 转int  异常返回值0
        /// </summary>
        /// <param name="objLong"></param>
        /// <returns></returns>
        public static long ToLong(object objLong)
        {
            return StringUtils.ToLong(objLong, 0L);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objBool"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ToBoolean(object objBool, bool defaultValue)
        {
            if (objBool == null || objBool is DBNull)
            {
                return defaultValue;
            }
            string value = objBool.ToString();
            if ("1".Equals(value) || "true".Equals(value, StringComparison.CurrentCultureIgnoreCase) || "y".Equals(value, StringComparison.CurrentCultureIgnoreCase) || "t".Equals(value, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            if ("0".Equals(value) || "false".Equals(value, StringComparison.CurrentCultureIgnoreCase) || "n".Equals(value, StringComparison.CurrentCultureIgnoreCase) || "f".Equals(value, StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }
            bool result;
            if (!bool.TryParse(value, out result))
            {
                try
                {
                    bool result2 = Convert.ToBoolean(objBool);
                    return result2;
                }
                catch
                {
                    bool result2 = defaultValue;
                    return result2;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objBool"></param>
        /// <returns>异常返回值false</returns>
        public static bool ToBoolean(object objBool)
        {
            return StringUtils.ToBoolean(objBool, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objDecimal"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(object objDecimal, decimal defaultValue)
        {
            if (objDecimal == null || objDecimal is DBNull)
            {
                return defaultValue;
            }
            decimal result;
            if (!decimal.TryParse(objDecimal.ToString(), out result))
            {
                try
                {
                    decimal result2 = Convert.ToDecimal(objDecimal);
                    return result2;
                }
                catch
                {
                    decimal result2 = defaultValue;
                    return result2;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objdecimal"></param>
        /// <returns>异常返回值0</returns>
        public static decimal ToDecimal(object objdecimal)
        {
            return StringUtils.ToDecimal(objdecimal, 0m);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objFloat"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ToFloat(object objFloat, float defaultValue)
        {
            if (objFloat == null || objFloat is DBNull)
            {
                return defaultValue;
            }
            float result;
            if (!float.TryParse(objFloat.ToString(), out result))
            {
                try
                {
                    float result2 = Convert.ToSingle(objFloat);
                    return result2;
                }
                catch
                {
                    float result2 = defaultValue;
                    return result2;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objFloat"></param>
        /// <returns>异常返回值0f</returns>
        public static float ToFloat(object objFloat)
        {
            return StringUtils.ToFloat(objFloat, 0f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objDouble"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(object objDouble, double defaultValue)
        {
            if (objDouble == null || objDouble is DBNull)
            {
                return defaultValue;
            }
            double result;
            if (!double.TryParse(objDouble.ToString(), out result))
            {
                try
                {
                    double result2 = Convert.ToDouble(objDouble);
                    return result2;
                }
                catch
                {
                    double result2 = defaultValue;
                    return result2;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objDouble"></param>
        /// <returns>异常返回值0.0</returns>
        public static double ToDouble(object objDouble)
        {
            return StringUtils.ToDouble(objDouble, 0.0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objPrice"></param>
        /// <param name="pricePrecision"></param>
        /// <param name="isShowPreCh"></param>
        /// <returns></returns>
        public static string ToPriceStr(object objPrice, int pricePrecision, bool isShowPreCh)
        {
            if (objPrice == null || objPrice is DBNull)
            {
                return "";
            }
            decimal num = StringUtils.ToDecimal(objPrice);
            if (pricePrecision < 2)
            {
                pricePrecision = 2;
            }
            if (isShowPreCh)
            {
                return "￥" + num.ToString("F" + pricePrecision.ToString());
            }
            return num.ToString("F" + pricePrecision.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objPrice"></param>
        /// <param name="isShowPreCh"></param>
        /// <returns></returns>
        public static string ToPriceStr(object objPrice, bool isShowPreCh)
        {
            return StringUtils.ToPriceStr(objPrice, StringUtils.DefaultPricePrecision, isShowPreCh);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objPrice"></param>
        /// <returns></returns>
        public static string ToPriceStr(object objPrice)
        {
            return StringUtils.ToPriceStr(objPrice, StringUtils.DefaultPricePrecision, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objAmt"></param>
        /// <param name="amtPrecision"></param>
        /// <param name="ceilingOrFloor"></param>
        /// <returns></returns>
        public static decimal ToAmt(object objAmt, int amtPrecision, bool ceilingOrFloor)
        {
            if (objAmt == null || objAmt is DBNull)
            {
                return 0m;
            }
            decimal d = StringUtils.ToDecimal(objAmt);
            if (amtPrecision < 2)
            {
                amtPrecision = 2;
            }
            double x = 10.0;
            double num = Math.Pow(x, (double)amtPrecision);
            decimal d2 = (decimal)num;
            decimal d3 = d * d2;
            long value;
            if (ceilingOrFloor)
            {
                value = (long)Math.Ceiling(d3);
            }
            else
            {
                value = (long)Math.Floor(d3);
            }
            return decimal.Parse((1.0m * value / d2).ToString("F" + amtPrecision.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objAmt"></param>
        /// <param name="ceilingOrFloor"></param>
        /// <returns></returns>
        public static decimal ToAmt(object objAmt, bool ceilingOrFloor)
        {
            return StringUtils.ToAmt(objAmt, 2, ceilingOrFloor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objAmt"></param>
        /// <returns></returns>
        public static decimal ToAmt(object objAmt)
        {
            return StringUtils.ToAmt(objAmt, 2, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objAmt"></param>
        /// <param name="amtPrecision"></param>
        /// <param name="ceilingOrFloor"></param>
        /// <param name="isShowPreCh"></param>
        /// <returns></returns>
        public static string ToAmtStr(object objAmt, int amtPrecision, bool ceilingOrFloor, bool isShowPreCh)
        {
            decimal num = StringUtils.ToAmt(objAmt, amtPrecision, ceilingOrFloor);
            if (isShowPreCh)
            {
                return "￥" + num.ToString("F" + amtPrecision.ToString());
            }
            return num.ToString("F" + amtPrecision.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objAmt"></param>
        /// <param name="ceilingOrFloor"></param>
        /// <param name="isShowPreCh"></param>
        /// <returns></returns>
        public static string ToAmtStr(object objAmt, bool ceilingOrFloor, bool isShowPreCh)
        {
            return StringUtils.ToAmtStr(objAmt, 2, ceilingOrFloor, isShowPreCh);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objAmt"></param>
        /// <param name="isShowPreCh"></param>
        /// <returns></returns>
        public static string ToAmtStr(object objAmt, bool isShowPreCh)
        {
            return StringUtils.ToAmtStr(objAmt, 2, true, isShowPreCh);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objAmt"></param>
        /// <returns></returns>
        public static string ToAmtStr(object objAmt)
        {
            return StringUtils.ToAmtStr(objAmt, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        public static int ToMoneyFen(decimal amt)
        {
            return (int)(amt * 100m);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereSQL"></param>
        /// <param name="addSQL"></param>
        /// <param name="opSQL"></param>
        /// <returns></returns>
        public static string AddToWhereSQL(ref string whereSQL, string addSQL, string opSQL)
        {
            if (string.IsNullOrEmpty(whereSQL))
            {
                whereSQL = " (" + addSQL + ") ";
            }
            else
            {
                whereSQL = string.Concat(new string[]
				{
					whereSQL,
					" ",
					opSQL,
					" (",
					addSQL,
					") "
				});
            }
            return whereSQL;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="thousandSep"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static string FormatCurrency(object amt, bool thousandSep, int precision)
        {
            if (amt == null)
            {
                return "0";
            }
            string result;
            try
            {
                NumberFormatInfo numberFormat = new CultureInfo("zh-CN").NumberFormat;
                numberFormat.CurrencyDecimalDigits = precision;
                if (thousandSep)
                {
                    int[] array = new int[2];
                    array[0] = 3;
                    int[] currencyGroupSizes = array;
                    numberFormat.CurrencyGroupSizes = currencyGroupSizes;
                    result = decimal.Parse(amt.ToString()).ToString("C", numberFormat);
                }
                else
                {
                    int[] array2 = new int[1];
                    int[] currencyGroupSizes2 = array2;
                    numberFormat.CurrencyGroupSizes = currencyGroupSizes2;
                    result = decimal.Parse(amt.ToString()).ToString("F" + precision.ToString(), numberFormat);
                }
            }
            catch
            {
                result = "0";
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="price"></param>
        /// <param name="thousandSep"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static string FormatCurrency1(object price, bool thousandSep, int precision)
        {
            if (price == null)
            {
                return "0";
            }
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberDecimalDigits = precision;
            numberFormatInfo.NumberDecimalSeparator = ".";
            numberFormatInfo.CurrencySymbol = "";
            if (thousandSep)
            {
                numberFormatInfo.NumberGroupSeparator = ",";
                numberFormatInfo.NumberGroupSizes = new int[]
				{
					3
				};
            }
            string result;
            try
            {
                result = Convert.ToDecimal(price).ToString("c", numberFormatInfo);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        public static string FormatCurrency(object amt)
        {
            return StringUtils.FormatCurrency(amt, true, 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static string FormatCurrency(object amt, int precision)
        {
            return StringUtils.FormatCurrency(amt, true, precision);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="thousandSep"></param>
        /// <returns></returns>
        public static string FormatCurrency(object amt, bool thousandSep)
        {
            return StringUtils.FormatCurrency(amt, thousandSep, 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        public static string FormatAmtStr(decimal amt)
        {
            string text = amt.ToString("F4");
            if (text.EndsWith("0"))
            {
                return amt.ToString("F3");
            }
            return amt.ToString("F4");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        public static string FormatAmtStr(object amt)
        {
            return StringUtils.FormatAmtStr(StringUtils.ToDecimal(amt));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        public static string FormatAmtChStr(decimal amt)
        {
            return "￥" + StringUtils.FormatAmtStr(amt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        public static string FormatAmtToChStr(decimal amt)
        {
            string[] array = new string[]
			{
				"分",
				"角",
				"元",
				"拾",
				"佰",
				"仟",
				"万",
				"拾",
				"佰",
				"仟",
				"亿",
				"拾",
				"佰",
				"仟",
				"兆",
				"拾",
				"佰",
				"仟"
			};
            string[] array2 = new string[]
			{
				"零",
				"壹",
				"贰",
				"叁",
				"肆",
				"伍",
				"陆",
				"柒",
				"捌",
				"玖"
			};
            string text = "";
            bool flag = false;
            string text2 = amt.ToString("F2");
            if (text2.IndexOf(".") != -1)
            {
                text2 = text2.Remove(text2.IndexOf("."), 1);
                flag = true;
            }
            for (int i = text2.Length; i > 0; i--)
            {
                int num = (int)Convert.ToInt16(text2[text2.Length - i].ToString());
                text += array2[num];
                if (flag)
                {
                    text += array[i - 1];
                }
                else
                {
                    text += array[i + 1];
                }
            }
            return text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strEmail"></param>
        /// <returns></returns>
        public static bool IsEmailFormat(string strEmail)
        {
            return Regex.IsMatch(strEmail, "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobileFormat(string mobile)
        {
            return Regex.IsMatch(mobile, "(^[1][3-8]\\d{9}$)");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="nullYN"></param>
        /// <param name="strCharList"></param>
        /// <returns></returns>
        public static bool CheckString(string str, bool nullYN, string strCharList)
        {
            if (nullYN && (str == null || str == ""))
            {
                return false;
            }
            for (int i = 0; i < str.Length; i++)
            {
                if (strCharList.IndexOf(str.Substring(i, 1)) < 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strInputNumber"></param>
        /// <param name="boolNull"></param>
        /// <returns></returns>
        public static bool IsNumber(string strInputNumber, bool boolNull)
        {
            return strInputNumber.LastIndexOf('-') <= 0 && StringUtils.CheckString(strInputNumber, boolNull, "-1234567890");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strInputNumber"></param>
        /// <param name="boolNull"></param>
        /// <returns></returns>
        public static bool IsFloat(string strInputNumber, bool boolNull)
        {
            if (strInputNumber.IndexOf("-") >= 0)
            {
                return false;
            }
            string[] array = strInputNumber.Split(new char[]
			{
				'.'
			});
            if (array.Length > 2)
            {
                return false;
            }
            if (array.Length == 2)
            {
                return StringUtils.IsNumber(array[0], boolNull) && StringUtils.IsNumber(array[1], boolNull);
            }
            return StringUtils.IsNumber(array[0], boolNull);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToDBStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return str.Replace("'", "''");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultStr"></param>
        /// <returns></returns>
        public static string ToDBStr(object str, string defaultStr)
        {
            if (str != null && !(str is DBNull))
            {
                return StringUtils.ToDBStr(str.ToString());
            }
            if (defaultStr == null)
            {
                return "";
            }
            return StringUtils.ToDBStr(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToDBStr(object str)
        {
            return StringUtils.ToDBStr(str, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string QuotedToDBStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "''";
            }
            return "'" + str.Replace("'", "''") + "'";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultStr"></param>
        /// <returns></returns>
        public static string QuotedToDBStr(object str, string defaultStr)
        {
            if (str == null || str is DBNull)
            {
                return StringUtils.QuotedToDBStr(defaultStr);
            }
            return StringUtils.QuotedToDBStr(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string QuotedToDBStr(object str)
        {
            return StringUtils.QuotedToDBStr(str, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string BoolToDBStr(bool flag)
        {
            if (flag)
            {
                return "1";
            }
            return "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objFlag"></param>
        /// <returns></returns>
        public static string BoolToDBStr(object objFlag)
        {
            return StringUtils.BoolToDBStr(StringUtils.ToBoolean(objFlag));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static int BoolTo01(bool flag)
        {
            if (flag)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objFlag"></param>
        /// <returns></returns>
        public static int BoolTo01(object objFlag)
        {
            return StringUtils.BoolTo01(StringUtils.ToBoolean(objFlag));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string BoolToYesNo(bool flag)
        {
            if (flag)
            {
                return "是";
            }
            return "否";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objFlag"></param>
        /// <returns></returns>
        public static string BoolToYesNo(object objFlag)
        {
            return StringUtils.BoolToYesNo(StringUtils.ToBoolean(objFlag));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputIP"></param>
        /// <returns></returns>
        public static string SimpleIP(string inputIP)
        {
            if (string.IsNullOrEmpty(inputIP))
            {
                return "";
            }
            string text = "";
            string[] array = inputIP.Split(new char[]
			{
				'.'
			});
            for (int i = 0; i < array.Length; i++)
            {
                int num = StringUtils.ToInt(array[i], 0);
                if (i > 0)
                {
                    text += ".";
                }
                text += num.ToString();
            }
            return text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputIP"></param>
        /// <returns></returns>
        public static string FormatIP(string inputIP)
        {
            if (string.IsNullOrEmpty(inputIP))
            {
                return "";
            }
            if ("::1".Equals(inputIP))
            {
                return "127.000.000.001";
            }
            string text = "";
            string[] array = inputIP.Split(new char[]
			{
				'.'
			});
            for (int i = 0; i < array.Length; i++)
            {
                string str = StringUtils.FillStr(3, array[i], '0', true);
                if (i > 0)
                {
                    text += ".";
                }
                text += str;
            }
            return text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputIP"></param>
        /// <returns></returns>
        public static bool VaildIP(string inputIP)
        {
            if (string.IsNullOrEmpty(inputIP))
            {
                return false;
            }
            string[] array = inputIP.Split(new char[]
			{
				'.'
			});
            if (array.Length == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    int num = StringUtils.ToInt(array[i], -1);
                    if (num < 0 || num > 255)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="limitIPs"></param>
        /// <param name="fromIP"></param>
        /// <returns></returns>
        public static bool IsInLimitIP(string limitIPs, string fromIP)
        {
            if (string.IsNullOrEmpty(limitIPs) || string.IsNullOrEmpty(fromIP))
            {
                return false;
            }
            fromIP = StringUtils.SimpleIP(fromIP);
            string[] array = limitIPs.TrimEnd(new char[]
			{
				','
			}).Split(new char[]
			{
				','
			});
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                if (text.IndexOf('*') > 0)
                {
                    string text2 = text.Replace(".*.", ".");
                    text2 = text2.Replace(".*", "");
                    text2 = text2.Replace("*", "");
                    text2 = StringUtils.SimpleIP(text2);
                    if (fromIP.IndexOf(text2) >= 0)
                    {
                        bool result = true;
                        return result;
                    }
                }
                else
                {
                    if (fromIP.Equals(StringUtils.SimpleIP(text)))
                    {
                        bool result = true;
                        return result;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip1"></param>
        /// <param name="ip2"></param>
        /// <returns></returns>
        public static bool IsEqualIP(string ip1, string ip2)
        {
            if (string.IsNullOrEmpty(ip1) || string.IsNullOrEmpty(ip1))
            {
                return false;
            }
            if (ip1.Equals(ip2))
            {
                return true;
            }
            string text = StringUtils.FormatIP(ip1);
            string value = StringUtils.FormatIP(ip2);
            return text.Equals(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="validStartSelect"></param>
        /// <returns></returns>
        public static bool IsValidSQL(string sql, bool validStartSelect)
        {
            if (string.IsNullOrEmpty(sql))
            {
                return false;
            }
            string text = StringUtils.StandardSQL(sql);
            if (text.StartsWith("xp_") || text.IndexOf(" xp_") > 0 || text.StartsWith("sp_") || text.IndexOf(" sp_") > 0)
            {
                //StringUtils.log.Warn("危险的SQL,sql=" + sql);
                return false;
            }
            string[] array = new string[]
			{
				"delete ",
				"drop ",
				"create ",
				"insert ",
				"truncate ",
				"update ",
				"exec ",
				"--"
			};
            for (int i = 0; i < array.Length; i++)
            {
                if (text.IndexOf(array[i]) >= 0)
                {
                    //StringUtils.log.Warn("危险的SQL,sql=" + sql);
                    return false;
                }
            }
            if (validStartSelect && (text.IndexOf("select ") < 0 || text.IndexOf("select ") > 0))
            {
                //StringUtils.log.Warn("查询SQL必须以select开始,sql=" + sql);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string StandardSQL(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                return "";
            }
            string[] array = sql.Split(new char[]
			{
				' ',
				'\r',
				'\n'
			});
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Trim().Length > 0)
                {
                    stringBuilder.Append(array[i].Trim().ToLower() + " ");
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return "";
            }
            char[] array = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int num = (int)bytes[i];
                array[i * 2] = StringUtils.HexDigits[num >> 4];
                array[i * 2 + 1] = StringUtils.HexDigits[num & 15];
            }
            return new string(array);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="byteLen"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes, int byteLen)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return "";
            }
            int num = bytes.Length;
            if (byteLen < num)
            {
                num = byteLen;
            }
            char[] array = new char[num * 2];
            for (int i = 0; i < num; i++)
            {
                int num2 = (int)bytes[i];
                array[i * 2] = StringUtils.HexDigits[num2 >> 4];
                array[i * 2 + 1] = StringUtils.HexDigits[num2 & 15];
            }
            return new string(array);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexStringToByte(string hexString)
        {
            byte[] result;
            try
            {
                if (string.IsNullOrEmpty(hexString))
                {
                    result = null;
                }
                else
                {
                    int num = hexString.Length / 2;
                    byte[] array = new byte[num];
                    for (int i = 0; i < num; i++)
                    {
                        array[i] = (byte)Convert.ToInt32(hexString.Substring(i * 2, 2), 16);
                    }
                    result = array;
                }
            }
            catch 
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileStr"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetProfileDictionary(string profileStr)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(profileStr))
            {
                return dictionary;
            }
            string[] array = profileStr.Split(new char[]
			{
				';'
			});
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                string[] array3 = text.Split(new char[]
				{
					'='
				});
                if (array3.Length == 2)
                {
                    dictionary[array3[0]] = array3[1];
                }
            }
            return dictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string GetProfileStr(IDictionary<string, string> dict)
        {
            if (dict == null)
            {
                return "";
            }
            StringBuilder stringBuilder = new StringBuilder();
            int num = 0;
            foreach (KeyValuePair<string, string> current in dict)
            {
                if (!string.IsNullOrEmpty(current.Value))
                {
                    if (num > 0)
                    {
                        stringBuilder.Append(";");
                    }
                    stringBuilder.Append(current.Key + "=" + current.Value.Replace("=", "＝").Replace(";", "；"));
                    num++;
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstProfileStr"></param>
        /// <param name="secondProfileStr"></param>
        /// <returns></returns>
        public static string MergeProfileStr(string firstProfileStr, string secondProfileStr)
        {
            if (string.IsNullOrEmpty(secondProfileStr))
            {
                return firstProfileStr;
            }
            if (string.IsNullOrEmpty(firstProfileStr))
            {
                return secondProfileStr;
            }
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            IDictionary<string, string> profileDictionary = StringUtils.GetProfileDictionary(firstProfileStr);
            IDictionary<string, string> profileDictionary2 = StringUtils.GetProfileDictionary(secondProfileStr);
            foreach (KeyValuePair<string, string> current in profileDictionary2)
            {
                dictionary[current.Key] = current.Value;
            }
            foreach (KeyValuePair<string, string> current2 in profileDictionary)
            {
                if (dictionary.ContainsKey(current2.Key.ToLower()))
                {
                    dictionary.Remove(current2.Key.ToLower());
                }
                else
                {
                    if (dictionary.ContainsKey(current2.Key))
                    {
                        dictionary.Remove(current2.Key);
                    }
                    else
                    {
                        if (dictionary.ContainsKey(current2.Key.ToUpper()))
                        {
                            dictionary.Remove(current2.Key.ToUpper());
                        }
                    }
                }
                dictionary[current2.Key] = current2.Value;
            }
            return StringUtils.GetProfileStr(dictionary);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictProfile"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetProfileValue(IDictionary<string, string> dictProfile, string key, string defaultValue)
        {
            if (dictProfile == null || string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }
            if (dictProfile.ContainsKey(key.ToLower()))
            {
                return dictProfile[key.ToLower()];
            }
            if (dictProfile.ContainsKey(key))
            {
                return dictProfile[key];
            }
            if (dictProfile.ContainsKey(key.ToUpper()))
            {
                return dictProfile[key.ToUpper()];
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictProfile"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool GetProfileValue(IDictionary<string, string> dictProfile, string key, bool defaultValue)
        {
            return StringUtils.ToBoolean(StringUtils.GetProfileValue(dictProfile, key, defaultValue.ToString()), defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictProfile"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetProfileValue(IDictionary<string, string> dictProfile, string key, int defaultValue)
        {
            return StringUtils.ToInt(StringUtils.GetProfileValue(dictProfile, key, defaultValue.ToString()), defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictProfile"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal GetProfileValue(IDictionary<string, string> dictProfile, string key, decimal defaultValue)
        {
            return StringUtils.ToDecimal(StringUtils.GetProfileValue(dictProfile, key, defaultValue.ToString()), defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileStr"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetProfileValue(string profileStr, string key, string defaultValue)
        {
            IDictionary<string, string> profileDictionary = StringUtils.GetProfileDictionary(profileStr);
            if (profileDictionary == null || profileDictionary.Count == 0)
            {
                return defaultValue;
            }
            return StringUtils.GetProfileValue(profileDictionary, key, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileStr"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool GetProfileValue(string profileStr, string key, bool defaultValue)
        {
            return StringUtils.ToBoolean(StringUtils.GetProfileValue(profileStr, key, defaultValue.ToString()), defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileStr"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetProfileValue(string profileStr, string key, int defaultValue)
        {
            return StringUtils.ToInt(StringUtils.GetProfileValue(profileStr, key, defaultValue.ToString()), defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileStr"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal GetProfileValue(string profileStr, string key, decimal defaultValue)
        {
            return StringUtils.ToDecimal(StringUtils.GetProfileValue(profileStr, key, defaultValue.ToString()), defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictProfile"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void PutProfileValue(IDictionary<string, string> dictProfile, string key, string value)
        {
            if (dictProfile == null || string.IsNullOrEmpty(key))
            {
                return;
            }
            if (dictProfile.ContainsKey(key.ToLower()))
            {
                dictProfile.Remove(key.ToLower());
            }
            else
            {
                if (dictProfile.ContainsKey(key))
                {
                    dictProfile.Remove(key);
                }
                else
                {
                    if (dictProfile.ContainsKey(key.ToUpper()))
                    {
                        dictProfile.Remove(key.ToUpper());
                    }
                }
            }
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            dictProfile[key] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileStr"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string PutProfileValue(string profileStr, string key, string value)
        {
            IDictionary<string, string> dictionary = StringUtils.GetProfileDictionary(profileStr);
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, string>();
            }
            StringUtils.PutProfileValue(dictionary, key, value);
            return StringUtils.GetProfileStr(dictionary);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static char GetNumChar(int num)
        {
            if (num < 0)
            {
                return '0';
            }
            if (num >= 0 && num <= 9)
            {
                return num.ToString()[0];
            }
            if (num >= 10 && num <= 35)
            {
                int num2 = num - 10 + 65;
                return (char)num2;
            }
            return '0';
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numChar"></param>
        /// <returns></returns>
        public static int GetCharNum(char numChar)
        {
            if (numChar < '\0')
            {
                return 0;
            }
            if (numChar >= '\0' && numChar <= '\t')
            {
                return (int)numChar;
            }
            if (numChar >= '0' && numChar <= '9')
            {
                return (int)(numChar - '0');
            }
            if (numChar >= 'A' && numChar <= 'Z')
            {
                return (int)(numChar - 'A' + '\n');
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetFileLengthStr(long length)
        {
            if (length <= 0L)
            {
                return "";
            }
            if (length < 1024L)
            {
                return "1K";
            }
            if (length < 1024000L)
            {
                return (1.0 * (double)length / 1024.0).ToString("F2").Replace(".00", "") + "K";
            }
            if (length < 1024000000L)
            {
                return (1.0 * (double)length / 1024000.0).ToString("F2").Replace(".00", "") + "M";
            }
            return (1.0 * (double)length / 1024000000.0).ToString("F2").Replace(".00", "") + "G";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public static string GetTimeSpanNote(DateTime dtStart, DateTime dtEnd)
        {
            TimeSpan timeSpan = dtEnd - dtStart;
            string text = "耗时：";
            if (timeSpan.Hours > 0)
            {
                text = text + timeSpan.Hours + "小时";
            }
            if (timeSpan.Minutes > 0)
            {
                text = text + timeSpan.Minutes + "分";
            }
            if (timeSpan.Seconds > 0)
            {
                text = text + timeSpan.Seconds + "秒";
            }
            if (timeSpan.Milliseconds > 0)
            {
                text = text + timeSpan.Milliseconds + "豪秒";
            }
            return text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="beforeShowLen"></param>
        /// <param name="afterShowLen"></param>
        /// <returns></returns>
        public static string GetMiddleHideStr(string str, int beforeShowLen, int afterShowLen)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            if (beforeShowLen + afterShowLen >= str.Length)
            {
                return str;
            }
            int num = str.Length - beforeShowLen - afterShowLen;
            string str2 = str.Substring(0, beforeShowLen);
            string str3 = str.Substring(beforeShowLen + num, afterShowLen);
            return str2 + StringUtils.RepeatChar('*', num) + str3;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="preText"></param>
        /// <param name="afterText"></param>
        /// <param name="srcText"></param>
        /// <returns></returns>
        public static string FindBetweenStr(string preText, string afterText, string srcText)
        {
            if (string.IsNullOrEmpty(srcText))
            {
                return "";
            }
            int num = srcText.IndexOf(preText) + preText.Length;
            if (num < 0 || num < preText.Length)
            {
                return "";
            }
            string text = srcText.Substring(num);
            int num2 = text.IndexOf(afterText);
            if (num2 <= -1)
            {
                return "";
            }
            return text.Substring(0, num2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public static string RemoveHtmlTag(string htmlStr)
        {
            if (string.IsNullOrEmpty(htmlStr))
            {
                return "";
            }
            htmlStr = Regex.Replace(htmlStr, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, "([\\r\\n])[\\s]+", "", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, "-->", "", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, "<!--.*", "", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, "&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, "&(iexcl|#161);", "¡", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, "&(cent|#162);", "¢", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, "&(pound|#163);", "£", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, "&(copy|#169);", "©", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, "&#(\\d+);", "", RegexOptions.IgnoreCase);
            htmlStr.Replace("<", "");
            htmlStr.Replace(">", "");
            htmlStr.Replace("\r\n", "");
            return htmlStr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static string GetClassName(string typeFullName)
        {
            if (string.IsNullOrEmpty(typeFullName))
            {
                return "";
            }
            string[] array = typeFullName.Split(new char[]
			{
				','
			});
            if (array != null && array.Length > 0)
            {
                return array[0];
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static string GetAssemblyName(string typeFullName)
        {
            if (string.IsNullOrEmpty(typeFullName))
            {
                return "";
            }
            string[] array = typeFullName.Split(new char[]
			{
				','
			});
            if (array != null && array.Length > 1)
            {
                return array[1];
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetAssemblyName(Type type)
        {
            string[] array = type.Assembly.FullName.Split(new char[]
			{
				','
			});
            if (array != null)
            {
                return array[0];
            }
            return "";
        }

        /// <summary>
        /// 截取字符串右侧指定数量字符
        /// </summary>
        /// <param name="pString">待截取字符串</param>
        /// <param name="len">截取长度</param>
        /// <returns></returns>
        public static string Right(string pString, int len)
        {
            if (string.IsNullOrEmpty(pString))
            {
                return pString;
            }

            if (pString.Length <= len)
            {
                return pString;
            }

            pString = pString.Remove(0, pString.Length - len);

            return pString;
        }

        /// <summary>
        /// 获得一个唯一字符，16位
        /// </summary>
        /// <returns></returns>
        public static string GuidString()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 允许0开头，如果allow_zero是true
        /// </summary>
        /// <param name="randomLength"></param>
        /// <param name="allowZero"></param>
        /// <returns></returns>
        public static string GetRandomNum(int randomLength, bool allowZero)
        {
            int i, p;
            string r_string;

            if (randomLength <= 0 || randomLength > 20)
                return null;

            Random rd = StringHelper.SYS_RD;

            //再用一个随机因子，时间。。。
            long tick = DateTime.Now.Ticks;
            Random rd2 = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            if (!allowZero)
                while ((p = GetNextRandDigit(rd, rd2)) == 0)
                { }
            else
                p = GetNextRandDigit(rd, rd2);

            r_string = p.ToString();

            for (i = 0; i < randomLength - 1; i++)
            {
                p = GetNextRandDigit(rd, rd2);
                r_string += p.ToString();
            }

            return r_string;
        }

        /// <summary>
        /// 随机生成字符传
        /// </summary>
        /// <param name="codeLen"></param>
        /// <returns>不包括 0-o,2-z,l-1</returns>
        public static string GetRandomString(int codeLen)
        {
            const string codeSerial = "3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,m,n,p,q,r,s,t,u,v,w,x,y";
            string[] arr = codeSerial.Split(',');
            string code = "";

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                int randValue = rand.Next(0, arr.Length - 1);
                code += arr[randValue];
            }

            return code;
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

        public static Random SYS_RD
        {
            get
            {
                return new Random(GetRandomSeed());
            }
        }


        private static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];

            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
