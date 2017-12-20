using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace KDMSCommon
{
    public class Common
    {

        /// <summary>
        /// 输出一组对象
        /// </summary>
        /// <param name="obj"></param>
        public static void Write(params object[] objArr)
        {
            if (objArr.Length == 1)
            {
                HttpContext.Current.Response.Write(objArr[0]);
            }
            else
            {
                foreach (object o in objArr)
                {
                    HttpContext.Current.Response.Write(o);
                    HttpContext.Current.Response.Write("<BR>");
                }
            }
        }

        /// <summary>
        /// 结束输出
        /// </summary>
        /// <param name="objArr"></param>
        public static void End(params object[] objArr)
        {
            Write(objArr);
            HttpContext.Current.Response.End();
        }

        public static void Redirect(string url)
        {
            HttpContext.Current.Response.Redirect(url);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 等价于HttpContext.Current.Server.MapPath(filePath)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string MapPath(string filePath)
        {
            return HttpContext.Current.Server.MapPath(filePath);
        }

        /// <summary>
        /// 判断是否本站提交
        /// </summary>
        /// <returns></returns>
        public static bool IsValidPost()
        {
            bool bl = false;
            string str1 = String.Format("{0}", HttpContext.Current.Request.ServerVariables["HTTP_REFERER"]);
            string str2 = String.Format("{0}", HttpContext.Current.Request.ServerVariables["HTTP_HOST"]);
            if (str1.ToLower().IndexOf(str2.ToLower()) > -1)
            {
                bl = true;
            }
            return bl;
        }
        /// <summary>
        /// 返回以“.”分割后的域名数组
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// 接收Request字符型，替换'引号
        /// </summary>
        /// <param name="RequestParamName"></param>
        /// <returns></returns>
        public static string Request(string key)
        {
            string str = GetString(HttpContext.Current.Request[key]);
            if (!String.IsNullOrEmpty(str))
            {
                str = str.Replace("'", "''");
            }
            return str;
        }
        /// <summary>
        /// 接收Request整型,转换失败返回默认值
        /// </summary>
        /// <param name="RequestParamName"></param>
        /// <param name="DefaultValue">转换失败返回默认值</param>
        /// <returns></returns>
        public static int Request(string key, int DefaultValue)
        {
            int n = DefaultValue;
            string str = GetString(HttpContext.Current.Request[key]);
            if (IsNumber(str))
            {
                n = ConvertToInt(str);
            }
            return n;
        }
        /// <summary>
        /// 接收Request字符型,是否移除HTML字符
        /// </summary>
        /// <param name="RequestParamName"></param>
        /// <param name="IsRemoveHTML">是否移除HTML字符</param>
        /// <returns></returns>

        /// <summary>
        /// 获取object对象的值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetString(object obj)
        {
            return String.Format("{0}", obj).Trim();
        }

        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSession(string name)
        {
            string str = String.Format("{0}", HttpContext.Current.Session[name]).Trim();
            //return UrlDecode(str);
            return str;
        }

        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="IsUTF8Decode">是否需要utf8反编码</param>
        /// <returns></returns>
        public static string GetSession(string name, bool IsUTF8Decode)
        {
            string str = String.Format("{0}", HttpContext.Current.Session[name]).Trim();
            return UrlDecode(str);
        }

        /// <summary>
        /// 设置Session值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void SetSession(string name, string value)
        {
            HttpContext.Current.Session[name] = value;
        }

        /// <summary>
        /// 设置Session值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void SetSession(string name, string value, bool IsUTF8Encode)
        {
            HttpContext.Current.Session[name] = UrlEncode(value);
        }

        /// <summary>
        /// 调整Page值
        /// </summary>
        /// <param name="onePageSize">每页显示记录大小</param>
        /// <param name="AllCount">总记录大小</param>
        /// <returns></returns>
        public static int AdjustPage(object page, int onePageSize, int AllCount, out int MaxPage)
        {
            String CurrentPage = GetString(page);
            int n = 1;
            MaxPage = ((AllCount % onePageSize) == 0) ? (AllCount / onePageSize) : (AllCount / onePageSize + 1);
            if (!String.IsNullOrEmpty(CurrentPage))
            {
                if (Common.IsNumber(CurrentPage))
                {
                    n = ConvertToInt(CurrentPage, 1);
                    if (n > MaxPage)
                    {
                        n = MaxPage;
                    }
                    if (n < 1)
                    {
                        n = 1;
                    }
                }
            }
            return n;
        }

        /// <summary>
        /// 字符串截取
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="length"></param>
        /// <returns></returns>

        /// <summary>
        /// 检查URL是否是http://开头
        /// </summary>
        /// <param name="strURL"></param>
        /// <returns></returns>
        public static string CheckUrl(object strURL)
        {
            string str = Common.GetString(strURL).ToLower();
            if (!str.StartsWith("http://"))
            {
                str = "http://" + str;
            }
            return str;
        }

        /// <summary>
        /// 判断字符是否为数字
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            string s = GetString(str);
            string pattern = @"^\d+$";
            return Regex.IsMatch(s, pattern);
        }

        /// <summary>
        /// 判断字符是否为小数
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(string str)
        {
            string s = GetString(str);
            string pattern = "^-?\\d+$|^(-?\\d+)(\\.\\d+)?$";
            return Regex.IsMatch(s, pattern);
        }

        /// <summary>
        /// 判断字符是否为EMAIL格式
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsEmail(string str)
        {
            string s = GetString(str);
            string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            return Regex.IsMatch(s, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断字符是否是日期格式
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsDateTime(string str)
        {
            bool bl = false;
            if (!String.IsNullOrEmpty(str))
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(str);
                    bl = true;
                }
                catch
                { }
            }
            return bl;
        }

        /// <summary>
        /// 判断字符是否为空
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(string str)
        {
            bool bl = false;
            if (string.IsNullOrEmpty(str))
            {
                bl = true;
            }
            return bl;
        }

        /// <summary>
        /// 判断字符是否属于标准字符[_a-zA-Z0-9]
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsValidChars(string str)
        {
            bool bl = false;
            Regex reg = new Regex(@"[^\w]", RegexOptions.IgnoreCase);
            if (reg.IsMatch(str))
            {
                bl = true;
            }
            return bl;
        }

        /// <summary>
        /// 判断字符是否匹配正则表达式
        /// </summary>
        /// <param name="strRegex">正则表达式字符</param>
        /// <param name="strMatch">要匹配的字符</param>
        /// <returns></returns>
        public static bool IsMatch(string strRegex, string strMatch)
        {
            bool bl = false;
            Regex reg = new Regex(strRegex, RegexOptions.IgnoreCase);
            if (reg.IsMatch(strMatch))
            {
                bl = true;
            }
            return bl;
        }

        /// <summary>
        /// 判断字符匹配正则表达式次数
        /// </summary>
        /// <param name="strRegex">正则表达式字符</param>
        /// <param name="strMatch">要匹配的字符</param>
        /// <returns>返回匹配次数</returns>
        public static int MatchCount(string strRegex, string strMatch)
        {
            Regex reg = new Regex(strRegex, RegexOptions.IgnoreCase);
            MatchCollection mc = reg.Matches(strMatch);
            return mc.Count;
        }


        /// <summary>
        /// 用正则表达式替换匹配的字符
        /// </summary>
        /// <param name="strRegex">正则表达式字符</param>
        /// <param name="strInput">要匹配的字符</param>
        /// <param name="strReplace">替换字符</param>
        /// <returns>返回替换后的字符</returns>
        public static string MatchReplace(string strRegex, string strInput, string strReplace)
        {
            Regex regex = new Regex(strRegex);
            return regex.Replace(strInput, strReplace);
        }

        /// <summary>
        /// 替换'为''
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToDoubleQuote(string str)
        {
            return str.Replace("'", "''");
        }

        /// <summary>
        /// 格式化HTML字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToHTMLString(object s)
        {
            string str = GetString(s);
            StringBuilder builder = new StringBuilder("");
            for (int i = 0; (str != null) && (i < str.Length); i++)
            {
                char ch = Convert.ToChar(str.Substring(i, 1));
                switch (ch)
                {
                    case '&':
                        {
                            builder.Append("&amp;");
                            continue;
                        }
                    case '\'':
                        {
                            builder.Append("&#039;");
                            continue;
                        }
                    case '"':
                        {
                            builder.Append("&#034;");
                            continue;
                        }
                    case '<':
                        {
                            builder.Append("&lt;");
                            continue;
                        }
                    case '>':
                        {
                            builder.Append("&gt;");
                            continue;
                        }
                }
                builder.Append(ch);
            }
            str = builder.ToString().Replace(" ", "&nbsp;");
            str = Regex.Replace(str, @"\n{2,}", "<br /><br />", RegexOptions.Compiled);
            return str.Replace("\n", "<br />");
        }

        /// <summary>
        /// 清除危险脚本
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToSafeHtml(string html)
        {
            string str = GetString(html);
            return HttpContext.Current.Server.HtmlEncode(str);
        }

        /// <summary>
        /// 将字符MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        //public static string ToMD5String(object str)
        //{
        //    string s = String.Format("{0}", str);
        //    return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(s, "MD5");
        //}

        /// <summary>
        /// 用UTF8编码URL
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UrlEncode(string strUrl)
        {
            return HttpUtility.UrlEncode(strUrl, Encoding.UTF8);
        }

        /// <summary>
        /// 用UTF8解码URL
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UrlDecode(string strUrl)
        {
            return HttpUtility.UrlDecode(strUrl, Encoding.UTF8);
        }

        public static string UrlEncodeGB2312(string strUrl)
        {
            return HttpUtility.UrlEncode(strUrl, Encoding.GetEncoding("GB2312"));
        }

        public static string UrlDncodeGB2312(string strUrl)
        {
            return HttpUtility.UrlDecode(strUrl, Encoding.GetEncoding("GB2312"));
        }

        //public static string escape(string str)
        //{
        //    return escape(str, false);
        //}
        /// <summary>
        /// 是否转换为HTML字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="IsHtmlString"></param>
        /// <returns></returns>
        //public static string escape(string str, bool IsHtmlString)
        //{
        //    string s = "";
        //    if (!String.IsNullOrEmpty(str))
        //    {
        //        if (IsHtmlString)
        //        {
        //            s = HttpUtility.UrlEncodeUnicode(ToHTMLString(str));
        //        }
        //        else
        //        {
        //            s = HttpUtility.UrlEncodeUnicode(str.Replace(" ", "&nbsp;"));
        //        }
        //    }
        //    return s;
        //}

        /// <summary>
        /// 格式化文件大小
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string FormatFileSize(Int64 n)
        {
            string str = "";
            if (n > 1073741824)
            {
                str = (n / 1073741824).ToString("###,###") + "G";
            }
            else if (n > 1048576)
            {
                str = (n / 1048576).ToString("###,###") + " M";
            }
            else if (n > 1024)
            {
                str = (n / 1024).ToString("###,###") + " K";
            }
            else if (n > 0)
            {
                str = n.ToString() + " B";
            }
            return str;
        }

        /// <summary>
        /// 返回一个随机数
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int RandomNum(int maxValue)
        {
            Random random = new Random(DateTime.Now.Second + DateTime.Now.Millisecond);
            return random.Next(maxValue);
        }

        /// <summary>
        /// 返回随机字符(t=0:返回数字和字母，1：只返回数字，2：自返回字符)
        /// </summary>
        /// <param name="length"></param>
        /// <param name="t">0:返回数字和字母，1：只返回数字，2：自返回字符</param>
        /// <returns></returns>
        //public static string RndCode(int length, int t = 0)
        //{
        //    string str = Common.ToMD5String(DateTime.Now).ToLower();
        //    if (t == 1)
        //    {
        //        str = Regex.Replace(str, @"[a-zA-Z]*", "");
        //        if (str.Length < length)
        //        {
        //            str = str.PadRight(length, '0');
        //        }
        //    }
        //    else if (t == 2)
        //    {
        //        str = Regex.Replace(str, @"[0-9]*", "");
        //        if (str.Length < length)
        //        {
        //            str = str.PadRight(length, 'a');
        //        }
        //    }
        //    str = str.Substring(0, length);
        //    return str;
        //}

        /// <summary>
        /// 生成随机数(18位长度)
        /// </summary>
        /// <param name="strExt">文件扩展名</param>
        /// <returns></returns>
        public static string RndNum(string strExt)
        {
            Random n = new Random(DateTime.Now.Second + DateTime.Now.Millisecond);
            int i = n.Next(10000);
            string str = String.Format("{0}{1}", FormatDateTime(DateTime.Now, "yyyyMMddHHmmss"), i.ToString().PadLeft(4, '0'));
            if (!strExt.StartsWith("."))
            {
                str = String.Format("{0}.{1}", str, strExt);
            }
            else
            {
                str = String.Format("{0}{1}", str, strExt);
            }
            return str;
        }



        /// <summary>
        /// 生成随机文件名(20位长度)
        /// </summary>
        /// <param name="strExt">文件扩展名</param>
        /// <returns></returns>
        //public static string RndFileName(string FileName, string strExt)
        //{
        //    Random n = new Random(DateTime.Now.Second + DateTime.Now.Millisecond);
        //    int i = n.Next(10000);
        //    string md5 = ToMD5String(FileName + strExt);
        //    string str = String.Format("{0}{1}", FormatDateTime(DateTime.Now, "yyyyMMddHHmmss"), md5.Substring(0, 6));
        //    if (!strExt.StartsWith("."))
        //    {
        //        str = String.Format("{0}.{1}", str, strExt);
        //    }
        //    else
        //    {
        //        str = String.Format("{0}{1}", str, strExt);
        //    }
        //    return str;
        //}

        /// <summary>
        /// 获取上传图片
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string GetImg(object fileName)
        {
            string FileName = GetString(fileName);
            string str = "/Upload/nopic.jpg";
            if (FileName == "" || GetString(FileName).ToLower() == "nopic.jpg")
            {
                str = "/Upload/nopic.jpg";
            }
            else if (FileName.ToLower().StartsWith("http://"))
            {
                str = FileName;
            }
            else
            {
                if (FileName.Length > 8)
                {
                    if (IsNumber(FileName.Substring(0, 8)))
                    {
                        str = String.Format("/Upload/{0}/{1}", FileName.Substring(0, 4), FileName);
                    }
                    else
                    {
                        str = String.Format("/Upload/{0}", FileName);
                    }
                }
                else
                {
                    str = String.Format("/Upload/{0}", FileName);
                }
            }
            return str;
        }

        /// <summary>
        /// 获取用户头像上传图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folderName">person</param>
        /// <returns></returns>
        public static string GetImg(object fileName, string folderName)
        {
            string FileName = GetString(fileName);
            string str = "/work/images/person/default.png";
            if (FileName.Length > 20)
            {
                str = String.Format("/Upload/{0}/{1}", folderName, FileName);
            }
            return str;
        }

        /// <summary>
        /// 获取视频图片
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string GetVideoImg(object fileName)
        {
            string FileName = GetString(fileName);
            string str = "/work/images/video.jpg";
            if (FileName == "" || GetString(FileName).ToLower() == "video.jpg")
            {
                str = "/work/images/video.jpg";
            }
            else
            {
                if (FileName.Length > 8)
                {
                    if (IsNumber(FileName.Substring(0, 8)))
                    {
                        str = String.Format("/Upload/{0}/{1}", FileName.Substring(0, 4), FileName);
                    }
                    else
                    {
                        str = String.Format("/Upload/{0}", FileName);
                    }
                }
                else
                {
                    str = String.Format("/Upload/{0}", FileName);
                }
            }
            return str;
        }

        /// <summary>
        /// 获取上传文件
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string GetFile(object fileName)
        {
            string FileName = GetString(fileName);
            string str = "";
            if (FileName.Length > 8)
            {
                if (IsNumber(FileName.Substring(0, 8)))
                {
                    str = String.Format("/Upload/{0}/{1}", FileName.Substring(0, 4), FileName);
                }
                else
                {
                    str = String.Format("/Upload/{0}", FileName);
                }
            }
            else
            {
                str = String.Format("/Upload/{0}", FileName);
            }
            return str;
        }

        /// <summary>
        /// 获取用户上传文件
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="folderName">Upload下的文件夹名</param>
        /// <returns></returns>
        public static string GetFile(object fileName, string folderName)
        {
            string FileName = GetString(fileName);
            string str = "";
            if (FileName.Length > 20)
            {
                str = String.Format("/Upload/{0}/{1}", folderName, FileName);
            }
            return str;
        }

        /// <summary>
        /// 获取上传文件绝对路径
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string GetFilePath(object fileName)
        {
            string FileName = GetFile(fileName);
            return FileName;
        }

        /// <summary>
        /// 移除HTML标记
        /// </summary>
        /// <param name="strHTML"></param>
        /// <returns></returns>

        /// <summary>
        /// 移除JS脚本
        /// </summary>
        /// <param name="strHTML"></param>
        /// <returns></returns>
        public static string RemoveJS(string strHTML)
        {
            string str = strHTML;
            if (!String.IsNullOrEmpty(str))
            {
                str = Regex.Replace(str, @"<script[^>]*>(?:.|\n)*?</script>", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            return str;
        }

        /// <summary>
        /// 产生密钥/处理加密字符
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        private static string GenerateKey(string Key, string Action)
        {
            string str = Key;
            if (Action == "")
            {
                int n = 8 - str.Length;
                if (n > 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        str += "0";
                    }
                }
                else
                {
                    str = str.Substring(0, 8);
                }
            }
            else if (Action == "Encrypt")
            {
                str = str.Replace("-", "");
            }
            else if (Action == "Decrypt")
            {
                if (Key.Length > 2)
                {
                    str = "";
                    for (int i = 0; i < Key.Length; i++)
                    {
                        str += Key.Substring(i, 2) + "-";
                        i += 1;
                    }
                    str = Regex.Replace(str, @"-$", "");
                }
            }
            return str;
        }

        /// <summary>
        /// 字符串编码转化(utf8转gb2312)
        /// </summary>
        /// <param name="encode1">原编码(utf8)</param>
        /// <param name="encode2">新编码(gb2312)</param>
        /// <returns></returns>
        public static string UTF8ToGb2312(string s)
        {
            string str = s;
            Encoding UTF8 = Encoding.UTF8;
            Encoding GB2312 = Encoding.GetEncoding("gb2312");
            str = GB2312.GetString(UTF8.GetBytes(s));
            return str;
        }

        /// <summary>
        /// 字符串编码转化(gb2312转utf8)
        /// </summary>
        /// <param name="encode1">原编码(gb2312)</param>
        /// <param name="encode2">新编码(utf8)</param>
        /// <returns></returns>
        public static string Gb2312ToUTF8(string s)
        {
            string str = s;
            Encoding UTF8 = Encoding.UTF8;
            Encoding GB2312 = Encoding.GetEncoding("gb2312");
            str = UTF8.GetString(GB2312.GetBytes(s));
            return str;
        }

        /// <summary>
        /// 将字节转化为字符串(UTF8编码)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string ByteToString(byte[] a)
        {
            return Encoding.UTF8.GetString(a);
        }

        /// <summary>
        /// 将字节数组转化为字符串
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string ByteToString(byte[] a, string encodeName)
        {
            return Encoding.GetEncoding(encodeName).GetString(a);
        }

        /// <summary>
        /// 将字符串转换为字节数组(UTF8编码)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] StringToByte(string s)
        {
            return System.Text.Encoding.UTF8.GetBytes(s);
        }

        public static byte[] StringToByte(string s, string encodeName)
        {
            return System.Text.Encoding.GetEncoding(encodeName).GetBytes(s);
        }

        /// <summary>
        /// 转换对象为整型，转换失败返回0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static byte ConvertToByte(object id)
        {
            return ConvertToByte(id, 0);
        }

        /// <summary>
        /// 转换对象为整型，转换失败返回默认值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static byte ConvertToByte(object id, byte DefaultValue)
        {
            byte n = DefaultValue;
            if (id != null)
            {
                bool bl = byte.TryParse(id.ToString(), out n);
                if (!bl)
                {
                    n = DefaultValue;
                }
            }
            return n;
        }

        /// <summary>
        /// 转换对象为整型，转换失败返回0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int ConvertToInt(object id)
        {
            return ConvertToInt(id, 0);
        }

        /// <summary>
        /// 转换对象为整型，转换失败返回默认值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int ConvertToInt(object id, int DefaultValue)
        {
            int n = DefaultValue;
            if (id != null)
            {
                bool bl = int.TryParse(GetString(id), out n);
                if (!bl)
                {
                    n = DefaultValue;
                }
            }
            return n;
        }

        /// <summary>
        /// 转换对象为长整型，转换失败返回0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static double ConvertToDouble(object id)
        {
            return ConvertToDouble(id, 0);
        }

        /// <summary>
        /// 转换对象为长整型，转换失败返回默认值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static double ConvertToDouble(object id, double DefaultValue)
        {
            double n = DefaultValue;
            if (id != null)
            {
                bool bl = Double.TryParse(GetString(id), out n);
                if (!bl)
                {
                    n = DefaultValue;
                }
            }
            return n;
        }


        public static long ConvertToLong(object id)
        {
            return ConvertToLong(id, 0);
        }
        public static long ConvertToLong(object id, long DefaultValue)
        {
            long n = DefaultValue;
            if (id != null)
            {
                bool bl = long.TryParse(GetString(id), out n);
                if (!bl)
                {
                    n = DefaultValue;
                }
            }
            return n;
        }
        /// <summary>
        /// 转换对象为Decimal型，转换失败返回0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static decimal ConvertToDecimal(object id)
        {
            return ConvertToDecimal(id, 0);
        }

        /// <summary>
        /// 转换对象为整型，转换失败返回默认值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static decimal ConvertToDecimal(object id, decimal DefaultValue)
        {
            decimal n = DefaultValue;
            if (id != null)
            {
                bool bl = Decimal.TryParse(GetString(id), out n);
                if (!bl)
                {
                    n = DefaultValue;
                }
            }
            return n;
        }

        /// <summary>
        /// 转换对象为日期，转换失败返回当前时间(yyyy-MM-dd 或者 yyyy-MM-dd HH:mm:ss 两种格式)
        /// </summary>
        /// <param name="datetime">yyyy-MM-dd 或者 yyyy-MM-dd HH:mm:ss 两种格式</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(object datetime)
        {
            return ConvertToDateTime(datetime, DateTime.Now);
        }
        /// <summary>
        /// 转换对象为日期，转换失败返回默认值(yyyy-MM-dd 或者 yyyy-MM-dd HH:mm:ss 两种格式)
        /// </summary>
        /// <param name="datetime">yyyy-MM-dd 或者 yyyy-MM-dd HH:mm:ss 两种格式</param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(object datetime, DateTime DefaultValue)
        {
            DateTime dt = DefaultValue;
            string strDT = GetString(datetime);
            if (strDT.IndexOf(":") > 0)//包含时分秒的24小时制
            {
                bool bl = DateTime.TryParseExact(strDT, "yyyy-MM-dd HH:mm:ss", new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out dt); ;
                if (!bl)
                {
                    dt = DefaultValue;
                }
            }
            else
            {
                bool bl = DateTime.TryParse(strDT, out dt);
                if (!bl)
                {
                    dt = DefaultValue;
                }
            }
            return dt;
        }

        /// <summary>
        /// 将对象格式化为yyyy-MM-dd
        /// </summary>
        /// <param name="datetime">日期对象</param>
        /// <returns></returns>
        public static string FormatDateTime(object datetime)
        {
            return FormatDateTime(datetime, "yyyy-MM-dd");
        }
        /// <summary>
        /// 将对象格式化为日期字符
        /// </summary>
        /// <param name="datetime">日期对象</param>
        /// <param name="format">yyyy-MM-dd HH:mm:ss</param>
        /// <returns></returns>
        public static string FormatDateTime(object datetime, string format)
        {
            string str = "";
            if (datetime != null)
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(datetime);
                    if (dt > DateTime.Now.AddYears(-1000))
                    {
                        str = Convert.ToDateTime(datetime).ToString(format);
                    }
                }
                catch
                { }
            }
            return str;
        }

        /// <summary>
        /// 将字符串翻转
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Reverse(string s)
        {
            char[] chr = s.ToCharArray();
            Array.Reverse(chr);
            return new string(chr);
        }

        /// <summary>
        /// 比较字符串值是否相等
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool FunEqual(object s1, object s2)
        {
            if (GetString(s1) == GetString(s2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 显示钩
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s1"></param>
        /// <returns></returns>
        public static string FunGou(object s1, object s2)
        {
            if (FunEqual(s1, s2))
            {
                return "&radic;";
            }
            else
            {
                return "";
            }
        }

        public static string FunChecked(object s1, object s2)
        {
            if (FunEqual(s1, s2))
            {
                return " checked";
            }
            else
            {
                return "";
            }
        }

        public static string FunSelected(object s1, object s2)
        {
            if (FunEqual(s1, s2))
            {
                return " selected";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 转换成货币格式
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string FormatPrice(object p)
        {
            Decimal d = 0;
            try
            {
                d = Convert.ToDecimal(p.ToString());
            }
            catch
            {
            }
            return d.ToString("C");
        }

        /// <summary>
        /// 获得连接参数字符
        /// </summary>
        /// <param name="PreUrl"></param>
        /// <returns></returns>
        public static string UrlChar(object PreUrl)
        {
            string str = "?";
            if (Common.GetString("PreUrl").IndexOf('?') > -1)
            {
                str = "&";
            }
            return str;
        }
        /// <summary>
        /// 获取XML格式设置值
        /// </summary>
        /// <param name="ParamName">xml标识名称</param>
        /// <returns></returns>
        public static string GetXMLSettingValue(string ParamName, string content)
        {
            string str = "";
            string parten = String.Format("<{0}>(?<v>(.|\n)*?)</{0}>", ParamName);
            Match m = Regex.Match(String.Format("{0}", content), parten, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                str = m.Groups["v"].Value;
            }
            return str;
        }
        /// <summary>
        /// 获取video文件播放格式，返回swf,mp,rm
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetVideoType(object filename)
        {
            string str = "mp";
            string FileName = Common.GetString(filename);
            if (FileName.ToLower().IndexOf("swf") > -1)
            {
                str = "swf";
            }
            return str;
        }
        /// <summary>
        /// 获取video文件播放代码
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetVideoContent(object filename, object ext, object width, object height)
        {
            StringBuilder sb = new StringBuilder("<div  style='text-align:center;'>");
            switch (Common.GetString(ext).ToLower())
            {
                case "swf":
                    sb.AppendFormat("<object width='{0}' height='{1}' classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0'>", width, height);
                    sb.AppendFormat("<param name='movie' value='{0}'>", filename);
                    sb.AppendFormat("<param name='wmode' value='transparent'>");
                    sb.AppendFormat("<param name='quality' value='autohigh'>");
                    sb.AppendFormat("<embed width='{0}' height='{1}' src='{2}' quality='autohigh' wmode='transparent' type='application/x-shockwave-flash' plugspace='http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash' menu='false'></embed>", width, height, filename);
                    sb.AppendFormat("</object>");
                    break;
                case "rm":
                    sb.AppendFormat("<object id='vid' classid='clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA' width={0} height={1}>", width, height);
                    sb.AppendFormat("<param name='AUTOSTART' value='true'>");
                    sb.AppendFormat("<param name='SHUFFLE' value='0'>");
                    sb.AppendFormat("<param name='PREFETCH' value='0'>");
                    sb.AppendFormat("<param name='NOLABELS' value='-1'>");
                    sb.AppendFormat("<param name='SRC' value='{0}'>", filename);
                    sb.AppendFormat("<param name='CONTROLS' value='Imagewindow'>");
                    sb.AppendFormat("<param name='CONSOLE' value='clip1'>");
                    sb.AppendFormat("<param name='CENTER' value='0'>");
                    sb.AppendFormat("</object>");
                    break;
                default://mp
                    sb.AppendFormat("<object id='player' width='{0}' height='{1}' classid='CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6'>", width, height);
                    sb.AppendFormat("<param NAME='AutoStart' VALUE='true'>");
                    sb.AppendFormat("<param name='enabled' value='true'>");
                    sb.AppendFormat("<param NAME='EnableContextMenu' VALUE='true'>");
                    sb.AppendFormat("<param NAME='url' value='{0}'>", filename);
                    sb.AppendFormat("<param name='stretchToFit' value='true'>");
                    sb.AppendFormat("<param name='volume' value='100'>");
                    sb.AppendFormat("<param name='uiMode' value='Full'>");
                    sb.AppendFormat("</object>");
                    break;
            }
            sb.Append("</div>");
            return sb.ToString();
        }




        /// <summary>
        /// 获取计算价格
        /// </summary>
        /// <param name="UnitPrice"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GetPrice(object UnitPrice, object count)
        {
            decimal d1 = Common.ConvertToDecimal(UnitPrice);
            decimal d2 = Common.ConvertToDecimal(count);
            return FormatPrice(d1 * d2);
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        public static string GetFY(int iPage, int PageCount, string fyUrl, out string shortFYString)
        {
            StringBuilder sb = new StringBuilder("");
            string url = string.Empty;
            if (iPage <= 1)
            {
                sb.AppendFormat("<span class=\"page_no\">&lsaquo;&lsaquo;上一页</span>");
            }
            else
            {
                url = string.Format(fyUrl, iPage - 1);
                sb.AppendFormat("<a href=\"{0}\" class=\"page_start\">&lsaquo;&lsaquo;上一页</a>", url);
            }
            if (iPage > 4)
            {
                sb.Append("<span class=\"page_break\">...</span>");
            }
            for (int i = (iPage - 3); i < iPage; i++)
            {
                url = string.Format(fyUrl, i);
                if (i > 0)
                {
                    sb.AppendFormat("<a href=\"{0}\" class=\"page_continue\">{1}</a>", url, i);
                }
            }
            sb.AppendFormat("<span class=\"page_ing\">{0}</span>", iPage);
            for (int i = iPage + 1; i <= iPage + 3; i++)
            {
                url = string.Format(fyUrl, i);
                if (i <= PageCount)
                {
                    sb.AppendFormat("<a href=\"{0}\" class=\"page_continue\">{1}</a>", url, i);
                }
            }
            if (iPage < (PageCount - 3))
            {
                sb.Append("<span class=\"page_break\">...</span>");
            }
            if (iPage < PageCount)
            {
                url = string.Format(fyUrl, iPage + 1);
                sb.AppendFormat("<a href=\"{0}\" class=\"page_next\">下一页&rsaquo;&rsaquo;</a>", url);
            }
            sb.AppendFormat("<span class=\"page_info\">共{0}页，到第", PageCount);
            sb.AppendFormat("<input class=\"page_number\" id=\"pnum\" name=\"pnum\" onkeydow='InputNum(event)' type=\"text\" />页 ", iPage);
            sb.AppendFormat("<input type='hidden' id='fyUrl' value=\"{0}\">", fyUrl);
            sb.Append("<button class=\"btnsubmit\" onclick='goPage()' style='cursor:pointer;'>确定</button></span>");

            //short fy
            shortFYString = string.Format("<h5 class=\"floatleft\">{0}/{1}&nbsp;&nbsp;</h5>", iPage, PageCount);
            if (iPage <= 1)
            {
                shortFYString += "<li>«上一页</li>";
            }
            else
            {
                url = string.Format(fyUrl, iPage - 1);
                shortFYString += string.Format("<li class=\"li_m\"><a href=\"#\">«上一页</a></li>", url);
            }
            if (iPage >= PageCount)
            {
                shortFYString += "<li>下一页 »</li>";
            }
            else
            {
                url = string.Format(fyUrl, iPage + 1);
                shortFYString += string.Format("<li class=\"li_m\"><a href=\"{0}\">下一页 »</a></li>", url);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取html里面的图片
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetHtmlImg(object content)
        {
            string str = "";
            MatchCollection mc = Regex.Matches(Common.GetString(content), @"<img[^>]+?src=['""]?(?<url>[^'""]+)", RegexOptions.IgnoreCase);
            if (mc.Count > 0)
            {
                str = mc[0].Groups["url"].Value;
            }
            else
            {
                str = "/Upload/nopic.gif";
            }
            return str;
        }
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="url"></param>
        /// <param name="msg"></param>
        /// <param name="IsSuccess">是否成功</param>
        /// <param name="blClose">是否显示关闭</param>
        public static void GoBack(string url = "", string msg = "Success", bool IsSuccess = true, bool blClose = false)
        {
            string BgSkin = Cookie.GetCookie("BgSkin");
            if (string.IsNullOrEmpty(BgSkin))
            {
                BgSkin = "blue";
            }
            HttpContext.Current.Response.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            HttpContext.Current.Response.Write("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            HttpContext.Current.Response.Write("<head>");
            HttpContext.Current.Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            HttpContext.Current.Response.Write(string.Format("<link href='../Style/{0}/style.css' type='text/css' rel='stylesheet' />", BgSkin));
            //HttpContext.Current.Response.Write("<meta http-equiv=\"refresh\" content=\"20;url=" + url + "\" />");
            HttpContext.Current.Response.Write("<title>Result</title>");
            HttpContext.Current.Response.Write("</head>");
            HttpContext.Current.Response.Write("<body>");
            string img = IsSuccess ? "success.gif" : "error1.gif";
            string MSG = !String.IsNullOrEmpty(msg) ? msg : " Success ! ";
            string btn = "<input type='button' value=' 返回 ' class='btn' onclick=\"history.go(-1);this.disabled=true;\" />";
            if (IsSuccess)
            {
                if (Common.GetString(url).Length > 0)
                {
                    btn = "<input type='button' value=' 返回 ' class='btn' onclick=\"window.location.href='" + url + "';this.disabled=true;\" />";
                }
                if (blClose)
                {
                    btn = "<input type='button' value=' 返回 ' class='btn' onclick=\"window.opener.location.reload();window.close()\" />";
                }
            }
            HttpContext.Current.Response.Write("<div class=\"Webpart\" style=\"width:600px; margin:auto; margin-top:100px;\">");
            HttpContext.Current.Response.Write("<div class=\"Webpart_title\">");
            HttpContext.Current.Response.Write("<div class=\"Webpart_titleLeft\">&nbsp;&nbsp;※ 信息提示</div>");
            HttpContext.Current.Response.Write("</div>");
            HttpContext.Current.Response.Write("<div class=\"Webpart_content\">");
            HttpContext.Current.Response.Write("<div style=\"height:50px;clear:both;\"></div>");
            HttpContext.Current.Response.Write(String.Format("<div style=\"float:left; width:150px; text-align:center;\"><img src='../images/{0}' /></div>", img));
            HttpContext.Current.Response.Write(String.Format("<div style=\"float:left;font-size:13px;\">{0}<BR /><BR />", MSG));
            HttpContext.Current.Response.Write(String.Format("{0}</div>", btn));
            HttpContext.Current.Response.Write("<div id=\"div_msg\" style=\"display:none;clear:both; padding-left:20px;\"></div>");
            HttpContext.Current.Response.Write("<div style=\"height:50px;clear:both;\"></div>");
            HttpContext.Current.Response.Write("</div>");
            HttpContext.Current.Response.Write("</div>");
            HttpContext.Current.Response.Write("</body>");
            HttpContext.Current.Response.Write("</html>");
            HttpContext.Current.Response.Write("<script type=\"text/javascript\">if(parent.lightboxhidden){var closeTimer;closeTimer=window.setTimeout(function(){parent.lightboxhidden()},1500)}</script>");
            HttpContext.Current.Response.End();
        }

        ///http://detectmobilebrowsers.com/
        ///
        public static bool IsMobileBrowser()
        {
            bool bl = false;
            string u = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|meego.+mobile|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
            {
                bl = true;
            }
            return bl;
        }

        /// <summary>
        /// E:\Upload\yyyy\MM\FileName
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string GetFileSrcPath(string FileName)
        {
            string year = FileName.Substring(0, 4);
            string month = FileName.Substring(4, 2);
            return MapPath(string.Format("~/Upload/{0}/{1}/{2}", year, month, FileName));
        }

        /// <summary>
        /// http://.../Upload/yyyy/MM/FileName
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string GetFileUrlPath(string FileName)
        {
            string year = FileName.Substring(0, 4);
            string month = FileName.Substring(4, 2);
            return string.Format("/Upload/{0}/{1}/{2}", year, month, FileName);
        }




        public static string GetFileExt(string fileName)
        {
            string strExt = System.IO.Path.GetExtension(fileName).ToLower().Replace(".", "");
            return strExt;
        }

    }
    //linq 扩展
    public static class DynamicLinqExpressions//注意static靜態型別
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        //注意this
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
