using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDMSCommon
{
    public class SqlFilterHelpler
    {
        #region 过滤SQL语句,防止注入
        /// <summary>
        /// 过滤SQL语句,防止注入
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns>0 - 没有注入, 1 - 有注入 </returns>
        public static bool FilterSql(string sSql)
        {
            int srcLen, decLen = 0;
            sSql = sSql.ToLower().Trim();
            srcLen = sSql.Length;
            sSql = sSql.Replace("exec", "");
            sSql = sSql.Replace("delete", "");
            sSql = sSql.Replace("master", "");
            sSql = sSql.Replace("truncate", "");
            sSql = sSql.Replace("declare", "");
            sSql = sSql.Replace("create", "");
            sSql = sSql.Replace("xp_", "no");
            decLen = sSql.Length;
            if (srcLen == decLen) return false; else return true;
        }
        #endregion
    }
}
