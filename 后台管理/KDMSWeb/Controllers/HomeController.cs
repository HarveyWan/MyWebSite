using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KDMSModel;
using System;

namespace KDMSWeb.Controllers
{
    public class HomeController : Controller
    {
        KDMSEntities db = new KDMSEntities();
        public ActionResult Index()
        {
            List<QueryRestaurantsTotal> listQueryDay = QueryRecordTotal("DAY");
            List<QueryRestaurantsTotal> listQueryWeek = QueryRecordTotal("WEEK");
            List<QueryRestaurantsTotal> listQueryMonth = QueryRecordTotal("MONTH");
            List<QueryRestaurantsTotal> listQueryYear = QueryRecordTotal("YEAR");
            //今天餐厅总数
            ViewBag.DayTotal = listQueryDay[0].RtsTotal;

            //本周餐厅总数
            ViewBag.WeekTotal = listQueryWeek[0].RtsTotal;
            //本月餐厅总数
            ViewBag.MonthTotal = listQueryMonth[0].RtsTotal;
            //今年餐厅总数
            ViewBag.YearTotal = listQueryYear[0].RtsTotal;
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        //获取权限菜单
        public ActionResult GetMenuList()
        {
            try
            {
                string RoleID = Request.Cookies["UserInfo"].Values["RoleID"].ToString();
                if (RoleID==null)
                {
                    return null;
                }
                List<Sys_Menu> list = db.Database.SqlQuery<Sys_Menu>(string.Format("SELECT * FROM Sys_Menu WHERE IsEnable=1 AND EXISTS(SELECT sr.* FROM Sys_Resources sr LEFT JOIN Sys_RoleResources srr ON sr.ResID=srr.ResID LEFT JOIN Sys_Roles rol ON srr.RoleCode=rol.RoleCode  LEFT JOIN Sys_UserRole sru ON rol.RoleCode=sru.RoleCode WHERE Sys_Menu.MenuID=sr.MenuID AND rol.RoleCode in({0})) ORDER BY ParentId,Sort", RoleID)).ToList();
                return Json(list);
            }
            catch(Exception ex)
            {
                return null;
            }            
        }
        //获取当前登录人页面权限
        public ActionResult GetRoleByPage(string url)
        {
            try
            {
                string RoleID = Request.Cookies["UserInfo"].Values["RoleID"].ToString();
                if (RoleID == null)
                {
                    return null;
                }
                string strsql = string.Format("SELECT sr.* FROM Sys_Resources sr LEFT JOIN Sys_RoleResources srr ON sr.ResID=srr.ResID LEFT JOIN Sys_Roles rol ON srr.RoleCode=rol.RoleCode WHERE  rol.RoleCode in({0}) AND sr.[Url]='{1}'", RoleID, url);
                List<Sys_Resources> listRs = db.Database.SqlQuery<Sys_Resources>(strsql).ToList();

                return Json(listRs);//JsonHelper.SerializeObject(dt)
            }
            catch
            {
                return Json("");
            }
        }

        /// <summary>
        /// 按时间统计餐厅总数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<QueryRestaurantsTotal> QueryRecordTotal(string time)
        {
            string strsql = string.Format("select 'RtsTotal' = COUNT(1),'MenuTotal' = sum(a.cds) from(select a.Name, COUNT(1) cds from Restaurant a left join RestaurantMenu b on a.Id = b.RestaurantId where DATEDIFF({0}, a.created, getdate()) = 0 group by a.Name) a", time);
            List<QueryRestaurantsTotal> listRs = db.Database.SqlQuery<QueryRestaurantsTotal>(strsql).ToList();
            return listRs;
        }
        /// <summary>
        /// 按时间统计每家餐厅总数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public ActionResult  QueryCateRecordTotal(string time)
        {
            string strsql = string.Format("select rt.Creator, rt.Name,sum(case when rt.RestaurantId is null then 0 else 1 end) CateCount ,rt.created from ( select a.Name,a.Creator,a.created,b.RestaurantId from Restaurant a left join RestaurantMenu b on a.Id=b.RestaurantId where DATEDIFF({0},a.created,getdate())=0) rt group by rt.Name,rt.Creator,rt.created order by rt.created desc", time);
            List<QueryCateTotal> listRs = db.Database.SqlQuery<QueryCateTotal>(strsql).ToList();
            return Json(listRs);
        }
    }
    public class QueryRestaurantsTotal
    {
        public int RtsTotal { get; set; }
        public int CateTotal { get; set; }
    }

    public class QueryCateTotal
    {
        public string Creator { get; set; }

        public string Name { get; set; }
        public int CateCount { get; set; }

    }
}
