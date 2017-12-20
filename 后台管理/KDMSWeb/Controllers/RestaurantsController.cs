using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KDMSModel;
using KDMSCommon;
using System.Data.SqlClient;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace KDMSWeb.Controllers
{
    public class RestaurantsController : Controller
    {
        //private DbHelper db = new DbHelper();
        KDMSEntities db = new KDMSEntities();
        // GET: Restaurant
        
        #region Restaurant
        public ActionResult Index()
        {
            return View(db.Restaurant.ToList());
        }

        // GET: Restaurant/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurant.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // GET: Restaurant/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Restaurant/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DPId,Name,Address,created,modified")] Restaurant restaurant)
        {
            ServiceReturnMsg srm = new ServiceReturnMsg();
            try
            {
                if (ModelState.IsValid)
                {
                    if (db.Restaurant.FirstOrDefault(r => r.Name == restaurant.Name) != null)
                    {
                        srm.ReturnCode = 0;
                        srm.ReturnMsg = "该餐厅已存在";
                        return Json(srm);
                    }                   
                    restaurant.Id = Guid.NewGuid();
                    restaurant.created = DateTime.Now;
                    string UserName = Request.Cookies["UserInfo"].Values["UserName"].ToString();
                    string UserId = Request.Cookies["UserInfo"].Values["UserId"].ToString();
                    restaurant.Creator = HttpUtility.UrlDecode(UserName);
                    restaurant.UserId = Convert.ToInt32(UserId);
                    db.Restaurant.Add(restaurant);                   
                    int row = db.SaveChanges();
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

        // GET: Restaurant/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurant.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurant/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DPId,Name,Address,created,modified")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(restaurant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(restaurant);
        }

        // GET: Restaurant/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurant.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Restaurant restaurant = db.Restaurant.Find(id);
            db.Restaurant.Remove(restaurant);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
        //=============================================================//

        public ActionResult CateRecord()
        {
            BindListCate();

            var menus = new Menus()
            {
                Menu = new List<RestaurantMenu>()
            };
            var menu = new RestaurantMenu()
            {
                Name = "",
                Type = "", Unit = "", Remark = ""
            };
            menus.Menu.Add(menu);            
            return View(menus);
        }

        /// <summary>
        /// 菜品下拉绑定
        /// </summary>
        public void BindListCate()
        {
            List<RestaurantMenu> menuList = db.RestaurantMenu.Take(10).GroupBy(c => c.Type).Select(s=>s.FirstOrDefault()).ToList();            
           
            //var sums2 = from emp in db.RestaurantMenu
            //            group emp by new { emp.Id, emp.Type } into g
            //            select new {  };

            SelectList selList = new SelectList(menuList, "Id", "Type");
            ViewBag.MenuList = selList;
        }
        /// <summary>
        /// 菜品下拉绑定 模糊查询
        /// </summary>
        public ActionResult BindListCatebyType(string q)
        {
            List<RestaurantMenu> menuList = db.RestaurantMenu.Where(t=>t.Type.Contains(q)).GroupBy(c => c.Type).Select(s => s.FirstOrDefault()).Take(10).ToList();

            return Json(menuList);
        }

        public List<Restaurant> ListRestaurantData(int pageSize, int pageNo, string keyWord, string sortName, string sortOrder)
        {
            List<Restaurant> lst = null;
            if (sortName != "" && sortOrder != "")
            {
                lst = db.Restaurant.Where(t => (t.Name.Contains(keyWord) || t.DPId.Contains(keyWord) || t.Address.Contains(keyWord) || t.Creator.Contains(keyWord)))
                                               .SetQueryableOrder(sortName, sortOrder)
                                               //.OrderByDescending(t => t.EnvImgCount)
                                               //.ThenByDescending(t => t.MenuImgCount)
                                               //.ThenByDescending(t => t.MenuCount)
                                               //.ThenByDescending(t => t.DPMenuCount)
                                               .Skip(pageSize * (pageNo - 1))
                                               .Take(pageSize)
                                               .AsQueryable()
                                               .ToList();
            }
            else
            {
                lst = db.Restaurant.Where(t => (t.Name.Contains(keyWord) || t.DPId.Contains(keyWord) || t.Address.Contains(keyWord) || t.Creator.Contains(keyWord)))
                                              .OrderByDescending(t => t.EnvImgCount)
                                              .ThenByDescending(t => t.MenuImgCount)
                                              .ThenByDescending(t => t.MenuCount)
                                              .ThenByDescending(t => t.DPMenuCount)
                                              .Skip(pageSize * (pageNo - 1))
                                              .Take(pageSize)
                                              .AsQueryable()
                                              .ToList();
            }
            return lst;
        }
        public List<Restaurant> ListRestaurantDatabyTime(int pageSize, int pageNo, string keyWord, string sortName, string sortOrder, DateTime sttime, DateTime endtime)
        {
            List<Restaurant> lst = null;
            if (sortName != "" && sortOrder != "")
            {
                lst = db.Restaurant.Where(t => (t.Name.Contains(keyWord) || t.DPId.Contains(keyWord) || t.Address.Contains(keyWord) || t.Creator.Contains(keyWord)) 
                && (t.created.Value.CompareTo(sttime) >= 0 && t.created.Value.CompareTo(endtime) <= 0))
                                                .SetQueryableOrder(sortName, sortOrder)
                                                //.OrderByDescending(t => t.DPMenuCount)
                                                //.ThenByDescending(t => t.EnvImgCount)
                                                //.ThenByDescending(t => t.MenuCount)
                                                //.ThenByDescending(t => t.MenuImgCount)
                                                .Skip(pageSize * (pageNo - 1))
                                                .Take(pageSize)
                                                .AsQueryable()
                                                .ToList();
            }
            else
            {
                lst = db.Restaurant.Where(t => (t.Name.Contains(keyWord) || t.DPId.Contains(keyWord) || t.Address.Contains(keyWord) || t.Creator.Contains(keyWord))
                && (t.created.Value.CompareTo(sttime) >= 0 && t.created.Value.CompareTo(endtime) <= 0))
                                                .OrderByDescending(t => t.EnvImgCount)
                                              .ThenByDescending(t => t.MenuImgCount)
                                              .ThenByDescending(t => t.MenuCount)
                                              .ThenByDescending(t => t.DPMenuCount)
                                                .Skip(pageSize * (pageNo - 1))
                                                .Take(pageSize)
                                                .AsQueryable()
                                                .ToList();
            }
            return lst;
        }
        /// <summary>
        /// 餐厅列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRestaurantList(int pageSize,int pageNo,string sortName="",string sortOrder="desc",string keyWord="",string startTime="",string endTime="")
        {
            DateTime sttime = new DateTime();
            if (startTime != "") { sttime = Convert.ToDateTime(startTime); }
            DateTime endtime = new DateTime();
            if (endTime != "") { endtime = Convert.ToDateTime(endTime).AddDays(1); }
            //IEnumerable<Restaurant> lst = CommonController.Search(Name, "", "", null);
            //拼接条件表达式查询
            //var eps = DynamicLinqExpressions.True<Restaurant>();
            //if (!string.IsNullOrEmpty(keyWord)) {
            //    eps.And(T => T.Name.Contains(keyWord));
            //    eps.And(T => T.DPId.Contains(keyWord));
            //    eps.And(T => T.Address.Contains(keyWord));
            //    eps.And(T => T.created.Value.CompareTo(keyWord) == 0);
            //}           

            //int totalRecord = db.Restaurant.Where(eps).Count();
            //List<Restaurant> lst = new CommonController().LoadPageItems<Restaurant>(10,1,out totalRecord,t=>t. ,t=>t,false);  //db.Restaurant.OrderByDescending(c => c.created).Take(10).ToList();
            int totalRecord = 0;                       

            List<Restaurant> lst = null;
            if (startTime == "" && endTime == "")
            {
                totalRecord = db.Restaurant.Where(t => (t.Name.Contains(keyWord) || t.DPId.Contains(keyWord) || t.Address.Contains(keyWord) || t.Creator.Contains(keyWord))).Count();
                lst = ListRestaurantData(pageSize, pageNo, keyWord, sortName, sortOrder);
            }
            else
            {
                totalRecord = db.Restaurant.Where(t => (t.Name.Contains(keyWord) || t.DPId.Contains(keyWord) || t.Address.Contains(keyWord) || t.Creator.Contains(keyWord)) 
                && (t.created.Value.CompareTo(sttime) >= 0 && t.created.Value.CompareTo(endtime) <= 0)).Count();

                lst = ListRestaurantDatabyTime(pageSize, pageNo, keyWord, sortName, sortOrder, sttime, endtime);
            }           

            //string jsonRes = JsonHelper.SerializeObject(lst);
            var ReList = new
            {
                total = totalRecord,
                rows = lst
            };

            //string ReList= "{\"total\":" + totalRecord + ",\"data\":" + JsonHelper.SerializeObject(lst) + "}";
            return Json(ReList);

        }
        
        /// <summary>
        /// 获取餐厅下拉
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRestaurant()
        {
            List<Restaurant> lst = db.Restaurant.GroupBy(c => c.Name).Select(s => s.FirstOrDefault()).ToList();
            return Json(lst);
        }
        public ActionResult CateList()
        {           
            return View();
        }

        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMenuList()
        {
            Guid sid = new Guid();
            if (!string.IsNullOrEmpty(Request.Form["id"]) && Request.Form["id"] != "0")
            {
                sid = Guid.Parse(Request.Form["id"]);
            }
            List<RestaurantMenu> lst = db.RestaurantMenu.Where(it => it.RestaurantId == sid).OrderByDescending(c => c.created).ToList();
            return Json(lst);
        }

        /// <summary>
        /// 点评菜单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDPMenuList()
        {
            Guid sid = new Guid();
            if (!string.IsNullOrEmpty(Request.Form["id"]) && Request.Form["id"] != "0")
            {
                sid = Guid.Parse(Request.Form["id"]);
            }
            List<RestaurantDPMenu> lst = db.RestaurantDPMenu.Where(it => it.RestaurantId == sid).OrderByDescending(c => c.created).ToList();
            return Json(lst);
        }

        /// <summary>
        /// 菜单数据录入
        /// </summary>
        /// <param name="cte"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MenuAdd(Menus menus)
        {
            //string Message = string.Empty;
            ServiceReturnMsg srm = new ServiceReturnMsg();
            int row = 0;
            try
            {
                string UserName = Request.Cookies["UserInfo"].Values["UserName"].ToString();
                string UserId = Request.Cookies["UserInfo"].Values["UserId"].ToString();               
                Guid SId = new Guid();
                if (string.IsNullOrEmpty(Request.Form["RestaurantId"]) || string.IsNullOrEmpty(Request.Form["RestaurantName"]))
                {
                    srm.ReturnCode = 0;
                    srm.ReturnMsg = "参数传递失败";
                    return Json(srm);
                }
                SId = Guid.Parse(Request.Form["RestaurantId"]);
                string SName = Request.Form["RestaurantName"];

                //新增餐厅
                //if (SId.ToString()== "00000000-0000-0000-0000-000000000000")
                //{
                //    //新增一个餐厅
                //    SId = RestaurantAdd(SName);
                //}
                //else
                //{
                //导入餐厅没有录入人的修改
                Restaurant restaurant = db.Restaurant.Find(SId);
                if (string.IsNullOrEmpty(restaurant.Creator))
                {           
                    restaurant.Creator = HttpUtility.UrlDecode(UserName);
                    restaurant.UserId = Convert.ToInt32(UserId);
                    restaurant.created = DateTime.Now;
                    restaurant.modified = DateTime.Now;

                    db.Entry(restaurant).State = EntityState.Modified;
                    db.SaveChanges();
                }
                string pid = restaurant.DPId == null ? "" :restaurant.DPId;// 
                //}
                var listMenu = menus.Menu;
                //判断输入多个同类型菜名是否重复
                //bool bl1 = listMenu.GroupBy(l => l.Type).Where(g => g.Count() > 1).Count() > 0;
                //bool bl2 = listMenu.GroupBy(l => l.Name).Where(g => g.Count() > 1).Count() > 0;
                //bool bl3 = false;
                
                //int count = 0;
                for (int j = 0; j < listMenu.Count; j++)
                {
                    RestaurantMenu c = listMenu[j];
                    //判断相同菜名相同单位是否重复                    
                    //bl3 = listMenu.Where(l => l.Name == c.Name).GroupBy(l => l.Unit).Where(g => g.Count() > 1).Count() > 0;
                    int count = listMenu.Where(l => l.Name == c.Name && l.Unit == c.Unit).Count();
                   
                    if (count > 1)//验证输入的数据
                    {
                        srm.ReturnCode = 0;
                        srm.ReturnMsg = "该餐厅相同单位的【" + c.Name + "】已存在";
                        return Json(srm);
                    }
                    //if (c.Unit != null)
                    //{                        
                    //    count += 1;
                    //}
                    string unit = c.Unit == null ? "-" : c.Unit;
                    //判断同个餐厅同单位菜名是否重复   
                    var q = from u in db.RestaurantMenu where (u.RestaurantId == SId && u.Name == c.Name && u.Unit == unit) select u;//&& u.Type == c.Type

                    if (q.FirstOrDefault() != null)//验证数据库
                    {
                        srm.ReturnCode = 0;
                        srm.ReturnMsg = "该餐厅相同单位的【" + c.Name + "】已存在";
                        return Json(srm);
                    }                   

                }
                //判断两条不为空的单位是否重复
                //if (count>1)
                //{
                //    bl3 = listMenu.GroupBy(l => l.Unit).Where(g => g.Count() > 1).Count() > 0;
                //}
                for (int i = 0; i < listMenu.Count; i++)
                {
                    RestaurantMenu c = listMenu[i];                    
                    
                    if (!string.IsNullOrEmpty(c.Type) && !string.IsNullOrEmpty(c.Name) && c.Price != 0)
                    {
                        //c.Id = Guid.NewGuid();
                        //c.RestaurantId = SId; //餐厅id
                        //c.RestaurantName = SName;//餐厅名
                        //c.DPId = pid;//点评
                        //c.created = DateTime.Now;
                        //c.Unit = c.Unit == null ? "-" : c.Unit;
                        //c.DPDishId = "";
                        //c.DPRecommand = 0;
                        //c.DPImgSrc = "";
                        //c.Remark = c.Remark == null ? "" : c.Remark;
                        //c.modified = DateTime.Now;
                        //db.RestaurantMenu.Add(c);
                        string sql = "insert into RestaurantMenu (Id,RestaurantId,RestaurantName,DPId,Name,Type,Price,Unit,created,Creator,UserId) "
                            + " values(@Id,@RestaurantId,@RestaurantName,@DPId,@Name,@Type,@Price,@Unit,@created,@Creator,@UserId)";
                        //Remark,
                        var param = new SqlParameter[]
                        {
                            new SqlParameter("@Id",Guid.NewGuid()),
                            new SqlParameter("@RestaurantId",SId),
                            new SqlParameter("@RestaurantName",SName),
                            new SqlParameter("@DPId",pid),
                            new SqlParameter("@Name",c.Name),
                            new SqlParameter("@Type",c.Type),
                            new SqlParameter("@Price",c.Price),
                            new SqlParameter("@Unit",c.Unit == null ? "-" : c.Unit),
                            //new SqlParameter("@Remark",c.Remark==null?"":c.Remark),
                            new SqlParameter("@created",DateTime.Now),
                            new SqlParameter("@Creator",HttpUtility.UrlDecode(UserName)),//录入人
                            new SqlParameter("@UserId",UserId) //录入人id

                        };
                        row = db.Database.ExecuteSqlCommand(sql, param);
                    }
                    else
                    {
                        srm.ReturnCode = 0;
                        srm.ReturnMsg = "类型或菜名或价格为空，请填写完整信息";
                        return Json(srm);
                    }
                }

                //int row = db.SaveChanges();
                srm.ReturnData = SId + "," + SName+"," + pid;
                srm.ReturnCode = row > 0 ? 1 : 0;
                srm.ReturnMsg = row > 0 ? "保存成功！" : "保存失败！";
            }
            catch (Exception ex)
            {
                srm.ReturnCode = 0;
                srm.ReturnMsg = "保存出错," + ex.Message;
            }
            return Json(srm);
        }

        private Guid RestaurantAdd(string sName)
        {
            Restaurant model = new Restaurant();
            
            //var q = from u in db.Restaurant.ToList() select u;
            ////判断餐厅名是否重复
            //q = q.Where(it => it.Name == sName);
            //if (q.FirstOrDefault() == null)
            //{
                model.Id = Guid.NewGuid();
                model.Name = sName;
                model.created = DateTime.Now;
                db.Restaurant.Add(model);
                db.SaveChanges();
            //}            
            return model.Id;
        }

        /// <summary>
        /// 菜单修改
        /// </summary>
        /// <param name="menu"></param>       
        /// <returns></returns>
        [HttpPost]
        public ActionResult MenuEdit(RestaurantMenu menu)
        {
            ServiceReturnMsg srm = new ServiceReturnMsg();
            try
            {
                //价格,备注不判断重复,查不到说明改了价格或者备注
                string field = Request["fld"];
                //List<RestaurantMenu> lst = db.RestaurantMenu.Where(t => t.Id == menu.Id && t.Price == menu.Price ).ToList();//from u in db.RestaurantMenu where (u.Id == menu.Id && u.Price == menu.Price ) select u;               

                //string remark = menu.Remark == null ? "" : menu.Remark;
                //if (lst.Count > 0 && lst[0].Remark == null&&lst.Count==0)
                //{
                //    remark = null;
                //}
                //var q1 = from u in db.RestaurantMenu where (u.Id == menu.Id && u.Remark == remark) select u.Id;
                //List<RestaurantMenu> lst = null;
                //if (menu.Remark!=null)
                //{
                //    lst = db.RestaurantMenu.Where(t => t.Id == menu.Id && t.Price == menu.Price && t.Remark == menu.Remark).ToList();
                //}
                //else
                //{
                //    lst = db.RestaurantMenu.Where(t=>t.Id == menu.Id && (t.Price == menu.Price && t.Remark == menu.Remark || t.Remark == "")).ToList();
                //}
                if (field != "Price" && field != "Remark" && field != "Type")//lst.Count>0 && q1.FirstOrDefault() != null
                {
                    string unit = menu.Unit == null ? "-" : menu.Unit;
                    var qs = from u in db.RestaurantMenu where (u.RestaurantId == menu.RestaurantId && u.Name == menu.Name && u.Unit == unit) select u.Id;
                    if (qs.FirstOrDefault() != null)
                    {
                        srm.ReturnCode = 0;
                        srm.ReturnMsg = "您修改的记录存在相同单位的菜名";
                        return Json(srm);
                    }
                }

                //RestaurantMenu entity = db.RestaurantMenu.Where(it => it.Id == menu.Id).FirstOrDefault<RestaurantMenu>();
                
                //RestaurantMenu rmenu = new RestaurantMenu();
                //rmenu.Id = menu.Id; 
                //var newuser = db.RestaurantMenu.Attach(rmenu);
                
                //if(entity.Name!=menu.Name)
                //    entity.Name = menu.Name;
                //if (entity.Type != menu.Type)
                //    entity.Type = menu.Type;
                //if (entity.Price != menu.Price)
                //    entity.Price = menu.Price;
                //if (entity.Unit != menu.Unit)
                //    entity.Unit = menu.Unit;
                //if (entity.Remark != menu.Remark)
                //    entity.Remark = menu.Remark;
                //entity.modified = DateTime.Now;

                //db.Entry(entity).State = EntityState.Modified;               
                //int row = db.SaveChanges();
                string sql = "update RestaurantMenu set Name=@Name,Type=@Type,Price=@Price,Unit=@Unit,Remark=@Remark,modified=@modified where Id=@Id";
                var param = new SqlParameter[]
                {
                    new SqlParameter("@Name",menu.Name),
                    new SqlParameter("@Type",menu.Type),
                    new SqlParameter("@Price",menu.Price),
                    new SqlParameter("@Unit",menu.Unit == null ? "-" : menu.Unit),
                    new SqlParameter("@Remark",menu.Remark==null?"":menu.Remark),//
                    new SqlParameter("@modified",DateTime.Now),
                    new SqlParameter("@Id",menu.Id)
                };
                int row =db.Database.ExecuteSqlCommand(sql, param);

                srm.ReturnCode = row > 0 ? 1 : 0;
                srm.ReturnMsg = row > 0 ? "保存成功！" : "保存失败！";
            }
            catch (Exception ex)
            {
                srm.ReturnCode = 0;
                srm.ReturnMsg = "保存出错," + ex.Message;
            }

            return Json(srm);
        }

        //菜单删除
        public ActionResult MenuDel(string id)
        {
            ServiceReturnMsg srm = new ServiceReturnMsg();
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    srm.ReturnCode = 0;
                    srm.ReturnMsg = "删除出错:传递参数错误";
                    return Json(srm);
                }
                var Id = Guid.Parse(id);
                RestaurantMenu entity = db.RestaurantMenu.Where(it => it.Id == Id).FirstOrDefault();
                db.Entry(entity).State = EntityState.Deleted;

                int row = db.SaveChanges();
                srm.ReturnCode = row > 0 ? 1 : 0;
                srm.ReturnMsg = row > 0 ? "删除成功！" : "删除失败！";
            }
            catch (Exception ex)
            {
                srm.ReturnCode = 0;
                srm.ReturnMsg = "删除出错," + ex.Message;
            }

            return Json(srm);
        }

        /// <summary>
        /// 餐厅修改
        /// </summary>
        /// <param name="cte"></param>       
        /// <returns></returns>
        [HttpPost]
        public ActionResult RestaurantEdit(Restaurant rt)
        {
            ServiceReturnMsg srm = new ServiceReturnMsg();
            try
            {
                var q = from u in db.Restaurant.ToList() select u;

                q = q.Where(it => it.Name == rt.Name);
                q = q.Where(it=>it.Address==rt.Address);
                if (q.FirstOrDefault() != null)
                {
                    srm.ReturnCode = 0;
                    srm.ReturnMsg = "该餐厅名已存在";
                    return Json(srm);
                }
                Restaurant entity = db.Restaurant.Where(it => it.Id == rt.Id).FirstOrDefault();
                entity.Name = rt.Name;
                entity.Address = rt.Address;
                entity.DPId = rt.DPId;
                entity.modified = DateTime.Now;

                db.Entry(entity).State = EntityState.Modified;
                int row = db.SaveChanges();
                srm.ReturnCode = row > 0 ? 1 : 0;
                srm.ReturnMsg = row > 0 ? "保存成功！" : "保存失败！";
            }
            catch (Exception ex)
            {
                srm.ReturnCode = 0;
                srm.ReturnMsg = "保存出错," + ex.Message;
            }

            return Json(srm);
        }

        //餐厅删除
        public ActionResult RestaurantDel(string id)
        {
            ServiceReturnMsg srm = new ServiceReturnMsg();
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    srm.ReturnCode = 0;
                    srm.ReturnMsg = "删除出错:传递参数错误";
                    return Json(srm);
                }
                var Id = Guid.Parse(id);
                Restaurant entity = db.Restaurant.Where(it => it.Id == Id).FirstOrDefault();
                db.Entry(entity).State = EntityState.Deleted;

                int row = db.SaveChanges();
                //RestaurantMenu restaurant = new RestaurantMenu() { RestaurantId=Id };
                //db.RestaurantMenu.Attach(restaurant);
                //db.RestaurantMenu.Remove(restaurant);
                //row = db.SaveChanges();
                string strsql = "delete from RestaurantMenu where RestaurantId=@RestaurantId";
                SqlParameter[] paras =
                {
                    new SqlParameter("@RestaurantId",Id)
                };           
                db.Database.ExecuteSqlCommand(strsql, paras);
                db.SaveChanges();
                //删除餐厅环境数据
                RestaurantEnvInfo envinfo = db.RestaurantEnvInfo.FirstOrDefault(t=>t.RestaurantId==entity.RestaurantEnvInfoId);
                db.Entry(envinfo).State = EntityState.Deleted;
                db.SaveChanges();
                srm.ReturnCode = row > 0 ? 1 : 0;
                srm.ReturnMsg = row > 0 ? "删除成功！" : "删除失败！";
            }
            catch (Exception ex)
            {
                srm.ReturnCode = 0;
                srm.ReturnMsg = "删除出错," + ex.Message;
            }

            return Json(srm);
        }

        //获取餐厅图片
        public ActionResult GetMenuImageUrls(string dirNo)
        {
            ServiceReturnMsg srm = new ServiceReturnMsg();
            try
            {
                //string dirName = Request.Form["dName"];
                if (string.IsNullOrEmpty(dirNo))
                {
                    srm.ReturnCode = 0;
                    srm.ReturnMsg = "参数不能为空";
                    return Json(srm);
                }
                //图片初始目录
                string imageFile = ConfigManage.ImageFileHead;
                //图片浏览URL
                string imageUrl = ConfigManage.ImageUrlHead;

                //获取该目录所有文件夹
                //string[] dir = Directory.GetDirectories(imageFile+dirNo);
               
                string dtPath = dirNo + "\\大堂照片\\";
                string cdPath = dirNo + "\\菜单照片\\";
                string ImgUrl = "{";
                //获取大堂照片所有文件
                if (Directory.Exists(imageFile + dtPath) && Directory.GetFiles(imageFile + dtPath).Length > 0)
                {
                    string[] files = Directory.GetFiles(imageFile + dtPath);

                    ImgUrl += "\"dt\":[";
                    foreach (var fls in files)
                    {
                        string filename = Path.GetFileName(fls);
                        ImgUrl += "{\"imgsrc\":\"" + imageUrl + dtPath + filename + "\"},";
                    }
                    ImgUrl = ImgUrl.Remove(ImgUrl.Length - 1);
                    ImgUrl = ImgUrl + "],";

                }
                if (Directory.Exists(imageFile + cdPath) && Directory.GetFiles(imageFile + cdPath).Length > 0)
                {
                    //获取菜单照片所有文件
                    string[] files1 = Directory.GetFiles(imageFile + cdPath);

                    ImgUrl += "\"cd\":[";
                    foreach (var fls in files1)
                    {
                        string filename = Path.GetFileName(fls);
                        ImgUrl += "{\"imgsrc\":\"" + imageUrl + cdPath + filename + "\"},";
                    }
                    ImgUrl = ImgUrl.Remove(ImgUrl.Length - 1);
                    ImgUrl = ImgUrl + "]";
                }
                else if (ImgUrl != "{")
                {
                    //只有一组图片数据删除最后一个逗号
                    ImgUrl = ImgUrl.Remove(ImgUrl.Length - 1);
                }

                ImgUrl += "}";
                ImgUrl = ImgUrl.Replace("\\","/");
                srm.ReturnCode = 1;
                srm.ReturnData = ImgUrl;
            }
            catch (Exception ex)
            {
                srm.ReturnCode = 0;
                srm.ReturnMsg = "获取餐厅图片出错," + ex.Message;
            }

            return Json(srm);
        }

        public void ProcessRequest(HttpContext context)
        {

            string parameter = context.Request.QueryString["file"];
            string dirName = Request.Form["dName"];
            //图片初始目录
            string imageFile = ConfigManage.ImageFileHead;
            //获取该目录所有文件夹
            string[] dir= Directory.GetDirectories(imageFile);
            string[] files = { };
            foreach (var dr in dir)
            {
                if (dirName==dr)
                {
                    //获取该目录所有文件

                    files = Directory.GetFiles(imageFile + dirName);
                    foreach (var fls in files)
                    {
                        FileStream fs = new FileStream(parameter, FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(fs);
                        Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                        br.Close();
                        fs.Close();
                        context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            //FileStream fs = new FileStream(parameter, FileMode.Open, FileAccess.Read);
            //BinaryReader br = new BinaryReader(fs);
            //Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            //br.Close();
            //fs.Close();
            //context.Response.OutputStream.Write(bytes, 0, bytes.Length);
        }
    }
}
