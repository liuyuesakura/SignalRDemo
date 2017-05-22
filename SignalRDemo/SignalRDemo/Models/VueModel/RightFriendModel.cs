using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRDemo.Models.VueModel
{
    /// <summary>
    /// 右侧的好友列表
    /// </summary>
    public class RightFriendModel
    {
        public string headImg { set; get; }
        public string userName { set; get; }
        public string onlineStatus { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string userId { set; get; }
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool isOnline { set; get; }
        public string connectionId { set; get; }
    }
}