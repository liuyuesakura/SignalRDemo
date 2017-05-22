using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRDemo.Hubs
{
    /// <summary>
    /// 在线用户信息缓存Model
    /// </summary>
    public class UserCacheModel
    {
        public string UserId { get; set; }
        public string UserName { get;set; }
        public string HeadImg { get; set; }
        /// <summary>
        /// 本次连接的 SignalR Hub ConnectionId
        /// </summary>
        public string ConnectionId{get;set;}
        public int UserKind { get; set; }
        /// <summary>
        /// 一个随机的验证串号
        /// </summary>
        public string CheckCode { get; set; }

        public enum UserKindEnum
        {
            /// <summary>
            /// 系统管理员
            /// </summary>
            Admin = 0,
            /// <summary>
            /// 客户
            /// </summary>
            Costumer = 1,
            /// <summary>
            /// 客服
            /// </summary>
            Service = 2,
            /// <summary>
            /// 监听器
            /// </summary>
            Spy = 3,
        }
    }
}