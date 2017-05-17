using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRDemo.Areas.Ajax.Controllers
{
    public class LoginController : Controller
    {
        // GET: Ajax/Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string userid,string pwd)
        {

            SRD.MongoBLL.User srduser = new SRD.MongoBLL.User();
            SRD.Model.User user = srduser.IsUserOrCreate(userid,pwd);
            bool result = user != null;
            if(result)
                OnlineUser.Instance.Login(user);
            return Json(new { 
                Code = result?0:1,
                msg = result?"登录成功":"登录失败",
                UserId = result?user.UserId:string.Empty,
                NickName = result?user.NickName:string.Empty
            });
        }
    }
}