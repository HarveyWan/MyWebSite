using KDMSCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KDMSModel;
using System.Data.Entity;

namespace KDMSWeb.Controllers
{
    public class AppController : Controller
    {
        KDMSEntities db = new KDMSEntities();
        // GET: App

        public ActionResult ShopList()
        {
            return View();
        }
        public ActionResult ShopRecord()
        {
            return View();
        }

        public ActionResult test()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }


        /// <summary>
        /// 查询餐厅基础与环境列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadShopRecordList()
        {
            ServiceReturnMsg srm = new ServiceReturnMsg();
           
            try
            {
                List<V_RestaurantEnvInfoKv> listEnvInfoKv = db.V_RestaurantEnvInfoKv.OrderByDescending(t=>t.created).ToList();
                srm.ReturnCode = 1;
                srm.ReturnData = listEnvInfoKv;
            }
            catch (Exception ex)
            {
                srm.ReturnCode = 0;
                srm.ReturnMsg = "查询餐厅基础与环境列表出错," + ex.Message;
            }

            return Json(srm);
        }
        /// <summary>
        /// 查询餐厅基础与环境数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadShopRecord(Guid? id)
        {
            ServiceReturnMsg srm = new ServiceReturnMsg();
            RestaurantEnvInfo model = new RestaurantEnvInfo();

            try
            {
                model = db.RestaurantEnvInfo.Find(id);
                Restaurant resModel = db.Restaurant.FirstOrDefault(t => t.RestaurantEnvInfoId == id);
                srm.ReturnCode = 1;
                srm.ReturnData = new
                {
                    envinfo = model,
                    res = resModel
                };
            }
            catch (Exception ex)
            {
                srm.ReturnCode = 0;
                srm.ReturnMsg = "查询餐厅基础与环境数据出错," + ex.Message;
            }

            return Json(srm);
        }
        /// <summary>
        /// 添加探店档案
        /// </summary>
        /// <param name="restaurant"></param>
        /// <returns></returns>
        [HttpPost]        
        public ActionResult AddShopRecord(RestaurantEnvInfo model)
        {
            ServiceReturnMsg srm = new ServiceReturnMsg();
            Restaurant restaurant = new Restaurant();
            string rname = Request.Form["RestaurantName"];
            string DPId = Request.Form["DPId"];
            string MeanPrice = Request.Form["MeanPrice"];
            string Address = Request.Form["Address"];

                      
            try
            {
                if (!ModelState.IsValid)
                {
                    var msg = string.Empty;
                    foreach (var value in ModelState.Values)
                    {
                        if (value.Errors.Count > 0)
                        {
                            foreach (var error in value.Errors)
                            {
                                //msg = msg + error.ErrorMessage;
                                srm.ReturnCode = 0;
                                srm.ReturnMsg = error.ErrorMessage;
                                return Json(srm);
                            }
                        }
                    }                    
                }
                if (ModelState.IsValid)
                {
                    if (db.Restaurant.FirstOrDefault(r => r.Name == rname) != null)
                    {
                        srm.ReturnCode = 0;
                        srm.ReturnMsg = "该餐厅已存在";
                        return Json(srm);
                    }
                           
                    string fwryxx = Request["服务人员形象"];
                    string cjts = Request["餐具特色"];
                    string zm = Request["照明"];
                    string cdcx = Request["菜单呈现"];              
                    string wsjsn = Request["卫生间室内"];
                    string wsjxst = Request["卫生间洗手台"];
                    string wsjzcj = Request["卫生间坐厕间"];
                    #region 保存餐厅环境数据                    
                    model.服务人员形象 = SumValue(fwryxx);
                    model.餐具特色 = SumValue(cjts);
                    model.照明 = SumValue(zm);
                    model.菜单呈现 = SumValue(cdcx);                    
                    model.卫生间室内 = SumValue(wsjsn);
                    model.卫生间洗手台 = SumValue(wsjxst);
                    model.卫生间坐厕间 = SumValue(wsjzcj);

                    string xc = model.新菜 == null ? "" : model.新菜;
                    string dnmj = model.店内面积 == null ? "" : model.店内面积;
                    model.新菜 = xc;
                    model.店内面积 = dnmj;
                    model.RestaurantId= Guid.NewGuid();
                    db.RestaurantEnvInfo.Add(model);
                    db.SaveChanges();                    
                    #endregion
                    //=============================================
                    #region 保存餐厅基础数据                    
                    restaurant.Name = rname;
                    restaurant.DPId = DPId;
                    restaurant.MeanPrice =Convert.ToInt32(MeanPrice);
                    restaurant.Address = Address;
                    string UserName = Request.Cookies["UserInfo"].Values["UserName"].ToString();
                    string UserId = Request.Cookies["UserInfo"].Values["UserId"].ToString();
                    restaurant.Creator = HttpUtility.UrlDecode(UserName);
                    restaurant.UserId = Convert.ToInt32(UserId);
                    restaurant.Id = Guid.NewGuid();
                    restaurant.RestaurantEnvInfoId = model.RestaurantId;
                    
                    restaurant.created = DateTime.Now;          
                    db.Restaurant.Add(restaurant);
                    int row = db.SaveChanges();
                    #endregion
                    //=============================================
                    srm.ReturnCode = row > 0 ? 1 : 0;
                    srm.ReturnMsg = row > 0 ? "添加成功！" : "添加失败！";
                }
            }
            catch (Exception ex)
            {
                srm.ReturnCode = 0;
                srm.ReturnMsg = "添加出错," + ex.Message;
            }

            return Json(srm); //View(restaurant);
        }

        /// <summary>
        /// 修改探店档案
        /// </summary>
        /// <param name="restaurant"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateShopRecord(RestaurantEnvInfo model)
        {
            ServiceReturnMsg srm = new ServiceReturnMsg();                       
            string rname = Request.Form["RestaurantName"];
            string DPId = Request.Form["DPId"];
            string MeanPrice = Request.Form["MeanPrice"];
            string Address = Request.Form["Address"];
            Guid resid = Guid.Parse(Request.Form["Resid"]);

            try
            {
                if (!ModelState.IsValid)
                {
                    var msg = string.Empty;
                    foreach (var value in ModelState.Values)
                    {
                        if (value.Errors.Count > 0)
                        {
                            foreach (var error in value.Errors)
                            {
                                //msg = msg + error.ErrorMessage;
                                srm.ReturnCode = 0;
                                srm.ReturnMsg = error.ErrorMessage;
                                return Json(srm);
                            }
                        }
                    }
                }
                if (ModelState.IsValid)
                {
                    Restaurant resEntity = db.Restaurant.FirstOrDefault(t => t.RestaurantEnvInfoId == resid);
                    //RestaurantEnvInfo envInfoEntity = db.RestaurantEnvInfo.FirstOrDefault(t => t.RestaurantId == model.RestaurantId);

                    //保存餐厅环境数据                    
                    string fwryxx = Request["服务人员形象"];
                    string cjts = Request["餐具特色"];
                    string zm = Request["照明"];
                    string cdcx = Request["菜单呈现"];
                    string wsjsn = Request["卫生间室内"];
                    string wsjxst = Request["卫生间洗手台"];
                    string wsjzcj = Request["卫生间坐厕间"];
                    #region 保存餐厅环境数据                    
                    model.服务人员形象 = SumValue(fwryxx);
                    model.餐具特色 = SumValue(cjts);
                    model.照明 = SumValue(zm);
                    model.菜单呈现 = SumValue(cdcx);
                    model.卫生间室内 = SumValue(wsjsn);
                    model.卫生间洗手台 = SumValue(wsjxst);
                    model.卫生间坐厕间 = SumValue(wsjzcj);

                    string xc = model.新菜 == null ? "" : model.新菜;
                    string dnmj = model.店内面积 == null ? "" : model.店内面积;
                    model.新菜 = xc;
                    model.店内面积 = dnmj;

                    model.RestaurantId = resid;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    #endregion
                    //=============================================
                    #region 保存餐厅基础数据                    
                    resEntity.Name = rname;
                    resEntity.DPId = DPId;
                    resEntity.MeanPrice = Convert.ToInt32(MeanPrice);
                    resEntity.Address = Address;
                    resEntity.modified = DateTime.Now;

                    db.Entry(resEntity).State = EntityState.Modified;
                    int row = db.SaveChanges();
                    #endregion
                    //=============================================
                    srm.ReturnCode = row > 0 ? 1 : 0;
                    srm.ReturnMsg = row > 0 ? "修改成功！" : "修改失败！";
                }
            }
            catch (Exception ex)
            {
                srm.ReturnCode = 0;
                srm.ReturnMsg = "修改出错," + ex.Message;
            }

            return Json(srm);
        }

        #region  登录
        /// <summary>
        /// app用户登录
        /// </summary>
        /// <returns></returns>
        public ActionResult UserLogin()
        {
            KDMSEntities db = new KDMSEntities();
            Sys_Users model = new Sys_Users();

            UpdateModel(model, Request.Form.AllKeys);
            if (model.Password == null)
            {
                return Redirect("/App/Login?success=-1");
            }
            else
            {
                string passWord = EncryptHelper.AESEncrypt(model.Password);
                var User = db.Sys_Users.Where(t => t.LoginName == model.LoginName && t.Password == passWord);

                if (User.FirstOrDefault() != null)
                {
                    HttpCookie userCookie = new HttpCookie("UserInfo");
                    model = User.FirstOrDefault();
                    //用户信息
                    userCookie["LoginName"] = model.LoginName;
                    userCookie["UserName"] = HttpUtility.UrlEncode(model.UserName);
                    userCookie["UserId"] = model.Id.ToString();
                                       
                    //个人权限
                    var ListUserRole = db.Sys_Roles.Join(db.Sys_UserRole, r => r.RoleCode, ur => ur.RoleCode, (r, ur) => new { r, ur.UserId })
                        .Where(t => t.UserId == model.Id).ToList();
                    int IsApp = 0;
                    if (ListUserRole.Count > 0)
                    {
                        string RoleID = "";
                        foreach (var rol in ListUserRole)
                        {
                            RoleID += "'" + rol.r.RoleCode + "',";
                            //得到app页面权限
                            if (rol.r.IsApp==1)
                            {
                                IsApp = 1;
                            }
                        }
                        if (RoleID != "")
                        {
                            userCookie["RoleID"] = RoleID.Remove(RoleID.Length - 1);
                        }
                    }
                    userCookie["IsApp"] = IsApp.ToString();
                    userCookie.HttpOnly = true;
                    Response.Cookies["UserInfo"].Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(userCookie);
                    return Redirect("/App/ShopList");
                }
                else
                    return Redirect("/App/Login?success=0");
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
            return Redirect("/App/Login");
        }
        #endregion

        public int SumValue(string txt)
        {
            int sum = 0;
            if (!string.IsNullOrEmpty(txt))
            {
                string[] arrNum = txt.Split(',');
                foreach (var i in arrNum)
                {
                    sum += Convert.ToInt32(i);
                }
            }
            return sum;
        }
    }
}