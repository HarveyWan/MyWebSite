using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KDMSModel;
using KDMSCommon;
using System.Data.Entity;

namespace KDMSWeb.Controllers
{
    public class SystemController : Controller
    {
        KDMSEntities db = new KDMSEntities();
        // GET: Sys
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ModifyPassWord()
        {
            return View();
        }

        public ActionResult SetPassword(Sys_Users us)
        {
            ServiceReturnMsg srm = new ServiceReturnMsg();
            try
            {
                Sys_Users entity = db.Sys_Users.Where(it => it.Id == us.Id).FirstOrDefault();
                if (entity.Password!=EncryptHelper.AESEncrypt(us.Password))
                {
                    srm.ReturnCode = 0;
                    srm.ReturnMsg = "您的旧密码输入错误";
                    return Json(srm);
                }
                entity.Password = EncryptHelper.AESEncrypt(us.NewPassword);

                db.Entry(entity).State = EntityState.Modified;
                int row = db.SaveChanges();
                srm.ReturnCode = row > 0 ? 1 : 0;
                srm.ReturnMsg = row > 0 ? "密码修改成功！" : "密码修改失败！";
            }
            catch (Exception ex)
            {
                srm.ReturnCode = 0;
                srm.ReturnMsg = "保存出错," + ex.Message;
            }

            return Json(srm);
        }
    }
}