using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRDemo
{
    public sealed class OnlineUser
    {
        public readonly static OnlineUser Instance = new OnlineUser();

        private readonly string KEY_USER = "user_srd";
        SRD.MongoBLL.User srduser = new SRD.MongoBLL.User();

        private OnlineUser() { }

        public void Login(string userid,string pwd)
        {
            LogOut();
            SRD.Model.User user = srduser.GetItem(userid,pwd);
            HttpContext.Current.Session[KEY_USER] = user;
            
        }
        public void Login(SRD.Model.User user)
        {
            LogOut();
            HttpContext.Current.Session[KEY_USER] = user;
        }

        public string GetCurrentUserID()
        {
            var user = (SRD.Model.User)HttpContext.Current.Session[KEY_USER];
            if (user != null)
            {
                return user.UserId;
            }
            return string.Empty;
        }
        public SRD.Model.User GetCurrentUser()
        {
            var user = (SRD.Model.User)HttpContext.Current.Session[KEY_USER];
            if (user != null)
            {
                return user;
            }
            return new SRD.Model.User(); 
        }

        public void LogOut()
        {
            ClearSession();
        }
        public void ClearSession()
        {
            HttpContext.Current.Session[KEY_USER] = null;
        }
    }
}