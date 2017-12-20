
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDMSCommon
{
    public class MailHelper
    {
        #region 从WebConfig中取得发送邮件参数

        /// <summary>
        /// 发送邮件的 SMTP 服务器
        /// </summary>
        public static string MailSmtpServer
        {
            get { return ConfigManage.MailSmtpServer; }
        }

        /// <summary>
        /// 系统发件地址
        /// </summary>
        public static string MailServerFromEmail
        {
            get { return ConfigManage.MailServerFromEmail; }
        }

        /// <summary>
        /// 系统邮件账号
        /// </summary>
        public static string MailFromAccount
        {
            get { return ConfigManage.MailFromAccount; }
        }

        /// <summary>
        /// 系统邮件密码
        /// </summary>
        public static string MailFromPassword
        {
            get
            {
                return ConfigManage.MailFromPassword;
            }
        }

        /// <summary>
        /// 系统调试时
        /// </summary>
        public static string DebugMail
        {
            get { return ConfigManage.DebugMail; }
        }

        #endregion

        /// <summary>
        /// 私有构造方法，不允许创建实例
        /// </summary>
        private MailHelper()
        {
            // TODO: Add constructor logic here
        }

        public static void SendConfigNetMail(string mailTo, string mailSubject, string mailBody, string mailAttch, string mailPriority, string mailCC, out string resultMessage)
        {
            string mailFrom = MailServerFromEmail;
            string mailCode = MailFromPassword;
            string mailAccount = MailFromAccount;
            SendNetMail(mailFrom, mailTo, mailSubject, mailBody, mailAttch, mailAccount, mailCode, mailPriority, mailCC, out resultMessage);
        }

        /// <summary>
        /// SendNetMail,多个收件人、抄送人、附件其参数用";"隔开,最后一个不能有";"
        /// </summary>
        /// <param name="mailFrom">发件人</param>
        /// <param name="mailTo">收件人(多个收件人用"；"隔开，最后一个不能有"；")</param>
        /// <param name="mailSubject">主题</param>
        /// <param name="mailBody">内容</param>
        /// <param name="mailAttch">附件（多个附件用"；"隔开，最后一个不能有"；"）</param>
        /// <param name="mailAccount">用户名（对加密过的）</param>
        /// <param name="mailCode">密码（对加密过的）</param>
        /// <param name="mailPriority">优先级</param>
        /// <param name="mailCC">抄送(多个抄送人用"；"隔开，最后一个不能有"；")</param>
        /// <param name="resultMessage">输出信息</param>
        public static void SendNetMail(string mailFrom, string mailTo, string mailSubject, string mailBody, string mailAttch, string mailAccount, string mailCode, string mailPriority, string mailCC, out string resultMessage)
        {
            //初始化输出参数
            resultMessage = "";

            //发件人和收件人不为空
            if (string.IsNullOrEmpty(mailFrom) || string.IsNullOrEmpty(mailTo))
            {
                resultMessage = "Please Fill Email Addresser Or Addressee . ";
                return;
            }

            System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
            System.Net.Mail.MailAddress emailFrom = new System.Net.Mail.MailAddress(mailFrom);

            //发件人
            email.From = emailFrom;
            //收件人
            if (string.IsNullOrEmpty(DebugMail))
            {
                string[] toUsers = mailTo.Split(';');
                foreach (string to in toUsers)
                {
                    if (!string.IsNullOrEmpty(to)) email.To.Add(to);
                }
            }
            else
            {
                email.To.Add(DebugMail);
                mailSubject += "(MailTo " + mailTo + ")";
            }
            //抄送
            if (string.IsNullOrEmpty(DebugMail))
            {
                if (!string.IsNullOrEmpty(mailCC))
                {
                    string[] ccUsers = mailCC.Split(';');
                    foreach (string cc in ccUsers)
                    {
                        if (!string.IsNullOrEmpty(cc)) email.CC.Add(cc);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(mailCC))
                    mailSubject += "(MailCC " + mailCC + ")";
            }
            //主题
            email.Subject = mailSubject;
            //内容
            email.Body = mailBody;
            //附件
            if (!string.IsNullOrEmpty(mailAttch))
            {
                string[] attachments = mailAttch.Split(';');
                foreach (string file in attachments)
                {
                    System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(file, System.Net.Mime.MediaTypeNames.Application.Octet);
                    //为附件添加发送时间
                    System.Net.Mime.ContentDisposition disposition = attach.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(file);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                    //添加附件
                    email.Attachments.Add(attach);
                }
            }
            //优先级
            email.Priority = (mailPriority == "High") ? System.Net.Mail.MailPriority.High : System.Net.Mail.MailPriority.Normal;
            //内容编码、格式
            email.BodyEncoding = System.Text.Encoding.UTF8;
            email.IsBodyHtml = true;
            //SMTP服务器
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(MailSmtpServer);

            //验证（Credentials 凭证）
            if (!string.IsNullOrEmpty(mailAccount)) client.Credentials = new System.Net.NetworkCredential(mailAccount, mailCode);

            //处理待发的电子邮件的方法 (Delivery 发送,传输)
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

            try
            {
                //发送邮件
                client.Send(email);
                resultMessage = "Sent Successfully";
            }
            catch (Exception ex)
            {
                resultMessage = "Send Faile,Bring Error :" + ex.Message;
            }
        }
    }
}
