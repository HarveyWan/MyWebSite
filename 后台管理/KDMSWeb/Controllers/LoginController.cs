using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KDMSModel;
using KDMSCommon;

namespace KDMSWeb.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        #region  登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        public ActionResult UserLogin()
        {
            KDMSEntities db = new KDMSEntities();
            Sys_Users model = new Sys_Users();

            UpdateModel(model, Request.Form.AllKeys);
            if (model.Password == null)
            {
                return Redirect("/Home/Login?success=-1");
            }
            else
            {
                string passWord = EncryptHelper.AESEncrypt(model.Password);

                //List<User_Account> list = new User_AccountBLL().UserLogin(model.LoginName, passWord);
                var User = db.Sys_Users.Where(t => t.LoginName == model.LoginName && t.Password == passWord);

                if (User.FirstOrDefault()!=null)
                {
                    HttpCookie userCookie = new HttpCookie("UserInfo");

                    //用户已禁用
                    //if (list[0].Enabled == true)
                    //{
                    //    return Redirect("/Home/Login?success=1");
                    //}

                    //bCompany company = new bCompanyBLL().GetModelByUserID(model.Id);
                    //if (company != null)
                    //{
                    //    HttpCookieHelper.SetCookie("AppCompanyNo", company.companyNO, DateTime.Now.AddYears(1));
                    //}
                    model = User.FirstOrDefault();
                    //用户信息
                    userCookie["LoginName"] = model.LoginName;                    
                    userCookie["UserName"] = HttpUtility.UrlEncode(model.UserName);
                    userCookie["UserId"] = model.Id.ToString();

                    //HttpCookieHelper.SetCookie("LoginName", model.LoginName, DateTime.Now.AddYears(1));
                    //HttpCookieHelper.SetCookie("PassWord", model.Password, DateTime.Now.AddYears(1));
                    //HttpCookieHelper.SetCookie("UserName", HttpUtility.UrlEncode(model.UserName), DateTime.Now.AddYears(1));
                    //HttpCookieHelper.SetCookie("UserId", model.Id.ToString(), DateTime.Now.AddYears(1));
                    //个人权限
                    var ListUserRole = db.Sys_Roles.Join(db.Sys_UserRole,r=>r.RoleCode,ur=>ur.RoleCode,(r,ur)=>new {r.RoleCode,ur.UserId })
                        .Where(t=>t.UserId==model.Id).ToList();

                    if (ListUserRole.Count > 0)
                    {
                        string RoleID = "";
                        foreach (var rol in ListUserRole)
                        {                           
                            RoleID += "'" + rol.RoleCode + "',";
                        }
                        if (RoleID != "")
                        {
                            userCookie["RoleID"] = RoleID.Remove(RoleID.Length - 1);
                        }
                    }

                    userCookie.HttpOnly = true;
                    Response.Cookies["UserInfo"].Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(userCookie);
                    return Redirect("/Home/Index");
                }
                else
                    return Redirect("/Home/Login?success=0");
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            HttpCookie aCookie;
            string cookieName;
            int limit = Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(aCookie);
            }
            return Redirect("/Home/Login");
        }
        #endregion
    }
}