using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace KDMSCommon
{
    public class ConfigManage
    {
        /// <summary>
        /// 系统日志地址
        /// </summary>
        public static string LogFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["LogFolder"].ToString();
            }
        }

        public static string PushCommandFlag
        { 
            get
            {
                return ConfigurationManager.AppSettings["PushCommandFlag"].ToString();
            }
        }
        public static string ImageUploadFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["ImageUploadFolder"].ToString();
            }
        }


        /// <summary>
        /// 浏览图片的服务器地址
        /// </summary>
        public static string ImageUrlHead
        {
            get
            {
                return ConfigurationManager.AppSettings["ImageUrlHead"].ToString();
            }
        }

        /// <summary>
        /// 服务器图片物理地址
        /// </summary>
        public static string ImageFileHead
        {
            get
            {
                return ConfigurationManager.AppSettings["ImageFileHead"].ToString();
            }
        }

        /// <summary>
        /// 邮件服务器
        /// </summary>
        public static string MailSmtpServer
        {
            get
            {
                return ConfigurationManager.AppSettings["MailSmtpServer"].ToString();
            }
        }

        /// <summary>
        /// 系统发送邮件地址
        /// </summary>
        public static string MailServerFromEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["MailServerFromEmail"].ToString();
            }
        }

        /// <summary>
        /// 系统发送邮件账号
        /// </summary>
        public static string MailFromAccount
        {
            get
            {
                return ConfigurationManager.AppSettings["MailFromAccount"].ToString();
            }
        }

        /// <summary>
        /// 系统发送邮件账号
        /// </summary>
        public static string MailFromPassword
        {
            get
            {
                
                return  ConfigurationManager.AppSettings["MailFromPassword"].ToString();
            }
        }

        /// <summary>
        /// 测试邮件地址
        /// </summary>
        public static string DebugMail
        {
            get
            {
                return ConfigurationManager.AppSettings["DebugMail"].ToString();
            }
        }
    }
}
