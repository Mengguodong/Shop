using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using SFO2O.Utility.Extensions;

namespace SFO2O.Utility.Uitl
{
    /// <summary>
    /// SMS����Ϣ������
    /// </summary>
    public static class SMS
    {

        /// <summary> 
        /// ���һ�η��Ͷ��ŵ�ʱ��
        /// </summary>
        public static DateTime LAST_SEND_SMS_TIME = DateTime.MinValue;


        /// <summary>
        /// ��׼�ӿڣ�����һ�����ţ��������ͣ����Ҫ�ٻ����ͣ���������أ�����һ��send_now��־λ
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
                    return SendMsg_159Net(strPhoneNum, strMessage, "1", 2500); //ȱʡͨ���ƶ�����
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strPhoneNum))
                {
                    return SendMsgToHK(strPhoneNum, strMessage, "1", 2500);
                }
            }
            return "����ʧ��";
        }


        /// <summary>
        /// �л����񹲺͹���½��������Ķ��ŷ���ƽ̨
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

            content = System.Web.HttpUtility.UrlEncode(string.Format("�������������̳�{0}", content), Encoding.GetEncoding("UTF-8"));


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
            return "�ֻ��Ų��Ϸ���û����Ϣ���ݣ�����û�з��ͣ�";
        }

        /// <summary>
        /// �°�Ķ��ŷ���ƽ̨
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

            content = System.Web.HttpUtility.UrlEncode(string.Format("�������������̳�{0}", content), Encoding.GetEncoding("UTF-8"));

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
            return "�ֻ��Ų��Ϸ���û����Ϣ���ݣ�����û�з��ͣ�";
        }


        /// <summary>
        /// ��GET������������ȡ���ص�ҳ����Ϣ
        /// ���ߣ�lianglei
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
                if (ex.Message.IndexOf("��ʱ") == -1)
                    return "����SMS Server ����: " + ex.Message;

                first_round_success = false;
            }

            if (first_round_success == false) //��һ��ʧ���ˣ�����һ�ΰ�
            {
                System.Threading.Thread.Sleep(500); //�ڶ��ξͼ����0.5��

                try
                {
                    response = request.GetResponse();
                }
                catch (Exception ex)
                {
                    return "�ڶ��η���SMS Server ����: " + ex.Message;
                }
            }

            if (response == null)
                return "����SMS Server����ResponseΪnull";

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
        /// ������Ϣ���ͺ󷵻ص�״̬
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string getReturnMsg(string msg)
        {
            if (msg == "�ڶ��η���SMS Server ����: ������ʱ")
                return "�ύ���ųɹ�";

            string returnMsg = string.Empty;
            
            try
            {

                if (int.Parse(msg) >= 0)
                {
                    returnMsg = "���ͳɹ�";
                }
                else
                {
                    switch (msg)
                    {
                        case "0":
                            returnMsg = "���ͳɹ�";
                            break;
                        case "-1":
                            returnMsg = "�˺�δע��";
                            break;
                        case "-2":
                            returnMsg = "��������";
                            break;
                        case "-3":
                            returnMsg = "�������";
                            break;
                        case "-4":
                            returnMsg = "�ֻ��Ÿ�ʽ����";
                            break;
                        case "-5":
                            returnMsg = "����";
                            break;
                        case "-6":
                            returnMsg = "��ʱ����ʱ�䲻����Ч��ʱ���ʽ";
                            break;
                        case "-7":
                            returnMsg = "��ֹ10Сʱ������ͬһ�ֻ��ŷ�����ͬ����";
                            break;
                        case "-100":
                            returnMsg = "���ƴ�IP����";
                            break;
                        case "-101":
                            returnMsg = "���ýӿ��ٶ�̫��";
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

