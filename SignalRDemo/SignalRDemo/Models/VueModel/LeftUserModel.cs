using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

///
//---注意VueModel中的Model字段需要使用小驼峰
///
namespace SignalRDemo.Models.VueModel
{
    
    /// <summary>
    /// 用于左侧的当前房间栏
    /// </summary>
    public class LeftUserModel
    {
        public string headImg { set; get; }
        public string userName { set; get; }
        public string onlineStatus { set; get; }
        public bool isGroup { set; get; }
        /// <summary>
        /// isGroup 为 true 时，表示这是一个聊天组；否则是一个一对一的聊天窗口
        /// </summary>
        public string groupId { set; get; }
    }
}