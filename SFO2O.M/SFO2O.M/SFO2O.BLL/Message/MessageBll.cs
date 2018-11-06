using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SFO2O.Model.Common;
using SFO2O.DAL.Message;
using SFO2O.Utility.Uitl;
using SFO2O.BLL.com.hksmspro.api3;
using System.Xml;
using SFO2O.Model.SMS;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core;
using Aliyun.Acs.Sms.Model.V20160927;
using Aliyun.Acs.Core.Exceptions;

namespace SFO2O.BLL.Message
{
    public class MessageBll
    {
        private static readonly MessageDal messageDal = new MessageDal();

        SFO2O.BLL.com.hksmspro.api3.SmsAPI5 smsClient = new SmsAPI5();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regionCode">86:中国大陆， 852：中华人民共和国大陆地区</param>
        /// <param name="smsContent"></param>
        /// <param name="receiverMobiles"></param>
        /// <param name="senderUserId"></param>
        /// <param name="senderSupplierId"></param>
        /// <param name="pushType"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public bool SendSms(string regionCode, string smsContent, List<string> receiverMobiles, int senderUserId, int senderSupplierId, int pushType = 2, long objectId = 0)
        {
            var result = false;
            if (receiverMobiles.Count > 0)
            {
                var messageInfo = new MessageInfo
                {
                    Title = "",
                    Content = smsContent,
                    CreateBy = senderUserId.ToString(),
                    CreateTime = DateTime.Now,
                    Timing = Convert.ToDateTime("1900-1-1"),
                    MesasgeTypeContent = string.Join(",", receiverMobiles).TrimEnd(','),
                    MessageType = 1,
                    SendWay = SendWay.Auto.GetHashCode(),
                    SendType = SendType.ByMobile.GetHashCode(),
                    CreateUserType = 1,
                    Status = (int)MessageStatus.Wait,
                    Sender = senderSupplierId,
                    ContentType = 0,
                    PushType = pushType,
                    ObjectId = objectId
                };
                try
                {
                    using (TransactionScope unitOfWork = new TransactionScope())
                    {
                        //创建消息并获取ID
                        messageInfo.ID = messageDal.Insert(messageInfo);
                        string mobileNo = messageInfo.MesasgeTypeContent;
                        if (!string.IsNullOrEmpty(mobileNo))
                        {
                            //开发阶段暂时不发短信
                            string username = "sf-o2o";
                            string password = "sf168";
                            string content = smsContent;
                            //string content = System.Web.HttpUtility.UrlEncode(string.Format("健康绿氧购物商城{0}", smsContent), Encoding.GetEncoding("UTF-8"));

                            string doResult = smsClient.SendMessage(username, password, string.Empty, content, regionCode + mobileNo, string.Empty);
                            if (!string.IsNullOrEmpty(doResult))
                            {
                                //<ReturnValue>
                                //  <State>1</State>
                                //  <Count>2</Count>
                                //  <ResponseID>14979274</ResponseID>
                                //</ReturnValue>

                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(doResult);
                                string state = doc.FirstChild.ChildNodes[0].InnerText;
                                if (state == "1")
                                {
                                    //短信发送成功，更新消息状态
                                    result = messageDal.UpdateMessageStatus(messageInfo.ID, (int)MessageStatus.Success);
                                }
                            }
                        }
                        unitOfWork.Complete();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.ErrorMsg(string.Format("发送消息异常:{0}", ex));
                }
            }
            return result;
        }

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="smsRequest"></param>
        /// <returns></returns>
        public bool SendSmsNew(SMSCodeRequest smsRequest)
        {
            bool result = false;
            IClientProfile profile = DefaultProfile.GetProfile("cn-beijing", "LTAIVpTlM5V7bC8j", "f3R43cN5IWv2Hd4CscciiojCsWDPQs");
            IAcsClient client = new DefaultAcsClient(profile);
            SingleSendSmsRequest requestSms = new SingleSendSmsRequest();
            try
            {
                requestSms.SignName = "健康绿氧";
                requestSms.TemplateCode = "SMS_70170128";
                requestSms.RecNum = smsRequest.Phone;
                requestSms.ParamString = "{'code':'" + smsRequest.Code + "'}";
                SingleSendSmsResponse httpResponse = client.GetAcsResponse(requestSms);
                result = true;
            }
            catch (ServerException e)
            {
                result = false;
                LogHelper.Error(string.Format("手机号：{0}，{1}", smsRequest.Phone, e.ErrorMessage));
            }
            catch (ClientException e)
            {
                result = false;
                LogHelper.Error(string.Format("手机号：{0}，{1}", smsRequest.Phone, e.ErrorMessage));
            }
            return result;
        }
    }
}
