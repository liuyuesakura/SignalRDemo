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
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

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
    }
}