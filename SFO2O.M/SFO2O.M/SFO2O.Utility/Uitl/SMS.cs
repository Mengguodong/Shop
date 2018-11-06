using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using SFO2O.Utility.Extensions;

namespace SFO2O.Utility.Uitl
{
    /// <summary>
    /// SMS短消息发送类
    /// </summary>
    public static class SMS
    {

        /// <summary> 
        /// 最后一次发送短信的时间
        /// </summary>
        public static DateTime LAST_SEND_SMS_TIME = DateTime.MinValue;


        /// <summary>
        /// 标准接口，发送一个短信，立即发送，如果要迟缓发送，则调用重载，增加一个send_now标志位
        /// </summary>
        /// <param name="ltDB"></param>
        /// <param name="strPhoneNum"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        public static string SendSMS(string strPhoneNum, string strMessage,string regionCode)
        {
            string defaultSMSReceiver = System.Configuration.ConfigurationManager.AppSettings["DefaultSMSReceiver"];

            if (defaultSMSReceiver != "1")
                strPhoneNum = defaultSMSReceiver;

            if (regionCode == "86")
            {
                if (!string.IsNullOrEmpty(strPhoneNum) && strPhoneNum.IndexOf("138001380") < 0)
                {
                    return SendMsg_159Net(strPhoneNum, strMessage, "1", 2500); //缺省通过移动网关
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strPhoneNum))
                {
                    return SendMsgToHK(strPhoneNum, strMessage, "1", 2500);
                }
            }
            return "发送失败";
        }


        /// <summary>
        /// 中华人民共和国大陆地区号码的短信发送平台
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content"></param>
        /// <param name="msgType"></param>
        /// <param name="stime"></param>
        /// <returns></returns>
        public static string SendMsgToHK(string mobile, string content, string msgType, int stime)
        {
            string username = "sf-o2o";
            string password = "sf168";
            //string senderId = "SFO2O";

            content = System.Web.HttpUtility.UrlEncode(string.Format("健康绿氧购物商城{0}", content), Encoding.GetEncoding("UTF-8"));


            string Url = string.Format("https://api3.hksmspro.com/service/smsapi5.asmx/SendMessage?Username={0}&Password={1}&Message={3}&Hex=&Telephone={2}&UserDefineNo="
                , username, password, "852"+mobile, content);

            string strResult = "";
            if (!string.IsNullOrEmpty(mobile) && mobile.Length >= 8 && !mobile.IsMobilePhoneNum(false) && !string.IsNullOrEmpty(content))
            {
                strResult = getPageByGet(Url).Replace("\r", "").Replace("\n", "");
            }

            if (!string.IsNullOrEmpty(strResult))
            {
                return getReturnMsg(strResult);
            }
            return "手机号不合法或没有信息内容，所以没有发送！";
        }

        /// <summary>
        /// 新版的短信发送平台
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content"></param>
        /// <param name="msgType"></param>
        /// <param name="stime"></param>
        /// <returns></returns>
        public static string SendMsg_159Net(string mobile, string content, string msgType, int stime)
        {
            string username = "sf-o2o";
            string password = "sf168";

            content = System.Web.HttpUtility.UrlEncode(string.Format("健康绿氧购物商城{0}", content), Encoding.GetEncoding("UTF-8"));

            string Url = string.Format("https://api3.hksmspro.com/service/smsapi5.asmx/SendMessage?Username={0}&Password={1}&Message={3}&Hex=&Telephone={2}&UserDefineNo="
                , username, password, "86" + mobile, content);
            //string Url = "https://api3.hksmspro.com/service/smsapi5.asmx/SendMessage?Username=sf-o2o&Password=sf168&Message=test&Hex=&Telephone=8613810624074&UserDefineNo=SFO2O";

            string strResult = "";
            if (!string.IsNullOrEmpty(mobile) && mobile.Length >= 11 && Verify.isValidMobile(mobile) && !string.IsNullOrEmpty(content))
            {
                strResult = getPageByGet(Url).Replace("\r", "").Replace("\n", "");
            }

            if (!string.IsNullOrEmpty(strResult))
            {
                return getReturnMsg(strResult);
            }
            return "手机号不合法或没有信息内容，所以没有发送！";
        }


        /// <summary>
        /// 以GET方发出请求，提取返回的页面信息
        /// 作者：lianglei
        /// </summary>
        /// <param name="pageUrl"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string getPageByGet(string pageUrl)
        {
            string result = "";
            WebRequest request = WebRequest.Create(pageUrl);            
            WebResponse response = null;

            bool first_round_success = true;

            try
            {
                response = request.GetResponse();
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("超时") == -1)
                    return "访问SMS Server 出错: " + ex.Message;

                first_round_success = false;
            }

            if (first_round_success == false) //第一次失败了，再来一次吧
            {
                System.Threading.Thread.Sleep(500); //第二次就间隔个0.5秒

                try
                {
                    response = request.GetResponse();
                }
                catch (Exception ex)
                {
                    return "第二次访问SMS Server 出错: " + ex.Message;
                }
            }

            if (response == null)
                return "访问SMS Server出错，Response为null";

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    Char[] ReaderBuffer = new Char[256];
                    int nCount = reader.Read(ReaderBuffer, 0, 256);
                    while (nCount > 0)
                    {
                        string str = new string(ReaderBuffer, 0, nCount);
                        result += str;
                        nCount = reader.Read(ReaderBuffer, 0, 256);
                    }

                    reader.Close();
                }

                stream.Close();
            }

            return result;
        }

        /// <summary>
        /// 返回信息发送后返回的状态
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string getReturnMsg(string msg)
        {
            if (msg == "第二次访问SMS Server 出错: 操作超时")
                return "提交短信成功";

            string returnMsg = string.Empty;
            
            try
            {

                if (int.Parse(msg) >= 0)
                {
                    returnMsg = "发送成功";
                }
                else
                {
                    switch (msg)
                    {
                        case "0":
                            returnMsg = "发送成功";
                            break;
                        case "-1":
                            returnMsg = "账号未注册";
                            break;
                        case "-2":
                            returnMsg = "其他错误";
                            break;
                        case "-3":
                            returnMsg = "密码错误";
                            break;
                        case "-4":
                            returnMsg = "手机号格式不对";
                            break;
                        case "-5":
                            returnMsg = "余额不足";
                            break;
                        case "-6":
                            returnMsg = "定时发送时间不是有效的时间格式";
                            break;
                        case "-7":
                            returnMsg = "禁止10小时以内向同一手机号发送相同短信";
                            break;
                        case "-100":
                            returnMsg = "限制此IP访问";
                            break;
                        case "-101":
                            returnMsg = "调用接口速度太快";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnMsg;
        }
    }
}

