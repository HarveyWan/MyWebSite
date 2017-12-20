using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDMSCommon
{
   public class Cookie
    {
        public static string domain = string.Empty;

        /// <summary>
        /// utf-8编码
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresHour"></param>
        public static void SetCookie(string key, string value, int expiresHour = 0)
        {
            HttpCookie cookie = new HttpCookie(key);
            if (expiresHour > 0)
            {
                cookie.Expires = DateTime.Now.AddHours(expiresHour);
            }
            cookie.Value = Common.UrlEncode(value);
            if (!string.IsNullOrEmpty(domain))
            {
                cookie.Domain = domain;
            }
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// utf-8解码
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookie(string key)
        {
            string str = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                str = Common.UrlDecode(cookie.Value + "");
            }
            return str;
        }

        public static void ClearCookie(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                if (!string.IsNullOrEmpty(domain))
                {
                    cookie.Domain = domain;
                }
                cookie.Expires = DateTime.Now.AddDays(-30);
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        public static void ClearAll()
        {
            int n = HttpContext.Current.Request.Cookies.Count;
            for (int i = 0; i < n; i++)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[i];
                cookie.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }
    }
}
