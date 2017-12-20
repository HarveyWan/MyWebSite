using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Web;
using System.IO;

namespace KDMSCommon
{
    public sealed class LogHelper
    {
        //记录DAL层的日志
        private static readonly ILog loggerDal = LogManager.GetLogger("DALLogger");

        //记录WebService的日志
        private static readonly ILog loggerWeb = LogManager.GetLogger("WebLogger");

        public static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// 记录DAL层的消息日志
        /// </summary>
        /// <param name="message"></param>
        public static void WriteDalInfoLog(string message)
        {
            loggerDal.Info(message);
        }

        /// <summary>
        /// 记录DAL层的错误日志
        /// </summary>
        /// <param name="message"></param>
        public static void WriteDalErrorLog(string message)
        {
            loggerDal.Error(message);
        }

        /// <summary>
        /// 记录Web层的消息日志
        /// </summary>
        /// <param name="message"></param>
        public static void WriteWebInfoLog(string message)
        {
            loggerWeb.Info(message);
        }

        /// <summary>
        /// 记录Web层的错误日志
        /// </summary>
        /// <param name="message"></param>
        public static void WriteWebErrorLog(string message)
        {
            loggerWeb.Error(message);
        }

        public static void LogMsg(string msg)
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            //string path = HttpContext.Current.Server.MapPath("./log/");
            string path = ConfigManage.LogFolder;
            //判断Log目录是否存在，不存在则创建
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + date + ".log";
            //使用StreamWriter写日志，包含时间，错误路径，错误信息
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine("-----------------" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-----------------");
                //sw.WriteLine(HttpContext.Current.Request.Url.ToString());
                //sw.WriteLine(HttpContext.Current.Server.MapPath("./log/"));
                sw.WriteLine(msg);
                sw.WriteLine("\r\n");
            }
        }
    }
}
