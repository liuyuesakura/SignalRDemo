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
        public bool IsUser_ByEmail(string email,string pwd)
        {
            //return CacheHelper<bool>.Get("IsConsumer" + consumer.ConsumerKey, TimeSpan.FromHours(1), () =>
            //{
            //    var result = DbContext.OAuth_Consumer.Count(p => p.Key == consumer.ConsumerKey) > 0;

            //    return result;
            //});
            var result = DBContext.User.Count(p => p.Email == email && p.Password == pwd) > 0;

            return result;
        }
        public bool IsUser_ByMobile(string mobile,string pwd)
        {
            var result = DBContext.User.Count(p => p.Mobile == mobile && p.Password == pwd) > 0;

            return result;
        }
        public bool Insert(Model.User user)
        {
            var result = DBContext.User.Insert(user) > 0;
            return result;
        }
        public bool Update(Model.User user)
        {
            var result = DBContext.User.InsertOrUpdate(user) > 0;
            return result;
        }
    }
}