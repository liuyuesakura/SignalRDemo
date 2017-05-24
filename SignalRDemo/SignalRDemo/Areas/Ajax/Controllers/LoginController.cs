using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRDemo.Areas.Ajax.Controllers
{
    public class LoginController : Controller
    {
        private SRD.MongoBLL.User bll_user = new SRD.MongoBLL.User();
        public JsonResult Test()
        {
            return Json(new { Code= 0,Message = "Test Success"},JsonRequestBehavior.AllowGet);
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
                Msg = result?"登录成功":"登录失败",
                UserId = result?user.UserId:string.Empty,
                NickName = result?user.NickName:string.Empty
            });
        }
        [HttpPost]
        public JsonResult Login(string userid, string pwd, string checkcode)
        {
            //处理验证码
            var code = this.HttpContext.Session["logincheckcode"];
            if (code == null)
            {
                return Json(new { 
                    Code = 1,
                    Msg = "没有获取到验证码"
                });
            }
            SRD.Model.User user_model = bll_user.IsUserAndGet(userid,pwd);
            if (user_model == null)
                return Json(new { 
                    Code = 1,
                    msg = "登录失败"
                });
            OnlineUser.Instance.Login(user_model);
            return Json(new { Code = 0,
                Msg = "登录成功"
            });
        }
    }
}