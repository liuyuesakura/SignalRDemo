using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //检查是否有登录，没有登录的话去登录页
            if (string.IsNullOrWhiteSpace(OnlineUser.Instance.GetCurrentUserID()) )
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        public ActionResult Login()
        {


            return View();
        }
        public JsonResult Reg()
        {
            SRD.Model.User user = new SRD.Model.User() 
            {
                NickName = "user1",
                Mobile = "123456",
                Password = "123456",
                Email = "123456@haha.com"
            };

            var userbll = new SRD.MongoBLL.User();
            bool result = userbll.Insert(user);

            return Json(new { Code = result?0:1,Msg = result?"OK":"Error"},JsonRequestBehavior.AllowGet);
        }
        public JsonResult Redis()
        {
            bool hali1 = SRD.Cache.CacheManager.SetRedisContent("haliluya","haliluya");
            string hali2 = SRD.Cache.CacheManager.GetRedisContent("haliluya");
            return Json(new { Code = hali1?0:1,Msg = hali2},JsonRequestBehavior.AllowGet);
        }
    }
}