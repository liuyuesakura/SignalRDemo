using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SRD.Core;
using MongoDB.Bson.Serialization.Attributes;
namespace SRD.Model
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [BsonIgnoreExtraElements]
    public class User : UpdateEntity
    {
        [BsonId]
        public string UserId { set; get; }
        public string NickName { set; get; }
        public string Password { set; get; }
        public string Email { set; get; }
        public string Mobile { set; get; }
        public DateTime AddTime { set; get; }
        public User()
        {
            this.UserId = Guid.NewGuid().ToString();
            AddTime = DateTime.Now;
        }
    }
}
