using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
namespace SFO2O.Framework.Uitl
{
    public class Mail
    {
        /// <summary>
        /// 邮件服务器
        /// </summary>
        static string Mail_SMTP = System.Configuration.ConfigurationManager.AppSettings["Mail_SMTP"];

        /// <summary>
        /// 发送带附件的邮件，返回FALSE发送失败，返回TRUE发送成功
        /// </summary>
        /// <param name="mailSubject">邮件主题</param>
        /// <param name="mailBody">邮件内容</param>
        /// <param name="mailAddress">接收人邮箱(多个邮箱用分号隔开)</param>
        /// <param name="filePathes">附件列表</param>
        public static bool SendMailByAttachments(string mailSubject, string mailContent, string mailAddress, IList<string> filesPath)
        {
            try
            {
                System.Net.Mail.MailMessage myMail = new System.Net.Mail.MailMessage();

                string[] arrAddress = mailAddress.Split(';');
                foreach (string address in arrAddress)
                {
                    if (!string.IsNullOrEmpty(address))
                    {
                        myMail.To.Add(address);
                    }
                }
                foreach (string path in filesPath)
                {
                    myMail.Attachments.Add(new Attachment(path));
                }
                myMail.Subject = mailSubject;
                myMail.SubjectEncoding = Encoding.GetEncoding("gb2312");
                myMail.Body = mailContent;
                myMail.BodyEncoding = Encoding.GetEncoding("gb2312");
                myMail.IsBodyHtml = true;
                SmtpClient sc = new SmtpClient();
                sc.Host = Mail_SMTP;
                sc.Credentials = new System.Net.NetworkCredential();
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.Send(myMail);

                myMail.Dispose();
            }
            catch
            {
                //LogHelper.Log("发送普通邮件时出错，错误信息如下：" + e.Message + e.StackTrace);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 给用户发送邮件，返回FALSE发送失败，返回TRUE发送成功
        /// </summary>
        /// <param name="mailSubject">邮件主题</param>
        /// <param name="mailContent">邮件内容</param>
        /// <param name="mailAddress">接收人邮箱(多个邮箱用分号隔开)</param>
        public static bool SendMailToUser(string mailSubject, string mailContent, string mailAddress)
        {
            try
            {
                MailMessage myMail = new MailMessage();
                myMail.From = new MailAddress("milangang@idstaff.com", "米兰港", Encoding.UTF8);

                string[] arrAddress = arrAddress = mailAddress.Split(';');

                foreach (string address in arrAddress)
                {
                    if (!string.IsNullOrEmpty(address))
                    {
                        myMail.To.Add(address);
                    }
                }
                myMail.Subject = mailSubject;
                myMail.SubjectEncoding = Encoding.GetEncoding("gb2312");
                myMail.Body = mailContent;
                myMail.BodyEncoding = Encoding.GetEncoding("gb2312");
                myMail.IsBodyHtml = true;
                SmtpClient sc = new SmtpClient();
                sc.Host = Mail_SMTP;
                sc.Credentials = new System.Net.NetworkCredential("milangang@idstaff.com", "PznDJF3M5a");
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.Send(myMail);
                myMail.Dispose();
            }
            catch 
            {
                return false;
            }
            return true;
        }
    }
}