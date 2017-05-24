using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SRD.MongoDao;
using SRD.Core;
using SRD.Repository;

namespace SRD.MongoBLL
{
    public class User
    {
        public bool IsUser(string str, string pwd)
        {
            var result = DBContext.User.Count(p => p.Password == pwd && !string.IsNullOrEmpty(pwd) && 
                    (
                        (p.UserId == str && !string.IsNullOrEmpty(p.UserId))    ||
                        (p.Email == str && !string.IsNullOrEmpty(p.Email))      ||
                        (p.Mobile == str && !string.IsNullOrEmpty(p.Mobile)) 
                    ) 
                ) > 0;
            return result;
        }
        public Model.User IsUserOrCreate(string str, string pwd)
        {
            if (IsUser(str, pwd))
            {
                return GetItem(str,pwd);
            }
            Model.User user = new Model.User() 
            {
                NickName = str,
                Mobile = "",
                Password = pwd,
                Email = ""
            };
            var result = Insert(user);
            return GetItem(str,pwd);
        }

        public Model.User IsUserAndGet(string str, string pwd)
        {
            if (!IsUser(str, pwd))
            {
                return null;
            } 
            return GetItem(str, pwd);
        }
        public bool IsUser_ByEmail(string email,string pwd)
        {
            //return CacheHelper<bool>.Get("IsConsumer" + consumer.ConsumerKey, TimeSpan.FromHours(1), () =>
            //{
            //    var result = DbContext.OAuth_Consumer.Count(p => p.Key == consumer.ConsumerKey) > 0;

            //    return result;
            //});
            var result = DBContext.User.Count(p => (p.Email == email && !string.IsNullOrEmpty(p.Email)) &&
                p.Password == pwd && !string.IsNullOrEmpty(pwd)) > 0;

            return result;
        }
        public bool IsUser_ByMobile(string mobile,string pwd)
        {
            var result = DBContext.User.Count(p => (p.Mobile == mobile && !string.IsNullOrEmpty(p.Mobile)) && 
                p.Password == pwd && !string.IsNullOrEmpty(pwd)) > 0;

            return result;
        }
        public bool Insert(Model.User user)
        {
            var count = DBContext.User.Count(p => 
                        (p.UserId == user.UserId && !string.IsNullOrEmpty(p.UserId)) ||
                        (p.Email == user.Email && !string.IsNullOrEmpty(p.Email))    ||
                        (p.Mobile == user.Mobile && !string.IsNullOrEmpty(p.Mobile))
                    );
            if (count > 0)
                return false;
            var result = DBContext.User.Insert(user) > 0;
            return result;
        }
        public bool Update(Model.User user)
        {
            var result = DBContext.User.InsertOrUpdate(user) > 0;
            return result;
        }
        public Model.User GetItem(string str, string pwd)
        {
            var result = DBContext.User.Get(p => p.Password == pwd && !string.IsNullOrEmpty(pwd) &&
                    (
                       (p.UserId == str && !string.IsNullOrEmpty(p.UserId)) ||
                       (p.Email == str && !string.IsNullOrEmpty(p.Email)) ||
                       (p.Mobile == str && !string.IsNullOrEmpty(p.Mobile))
                    )
                );
            return result;
        }
    }
}