using KDMSCommon;
using KDMSModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KDMSWeb.Controllers
{
    public class UserEventController : Controller
    {
        KDMSEntities db = new KDMSEntities();
        // GET: UserEvent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddEvent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUserWork(User_Work work)
        {
            ServiceReturnMsg srm = new ServiceReturnMsg();
            try
            {
                if (ModelState.IsValid)
                {
                    
                    work.Id = Guid.NewGuid();
                    work.created = DateTime.Now;
                    string UserName = Request.Cookies["UserInfo"].Values["UserName"].ToString();
                    string UserId = Request.Cookies["UserInfo"].Values["UserId"].ToString();

                    work.Creator = HttpUtility.UrlDecode(UserName);
                    work.UserId = Convert.ToInt32(UserId);
                    db.User_Work.Add(work);
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

            return Json(srm);
        }
    }
}