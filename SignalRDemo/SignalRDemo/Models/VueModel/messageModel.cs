using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRDemo.Models.VueModel
{
    /// <summary>
    /// 消息体Model
    /// </summary>
    public class MessageModel
    {
        public string timeStamp { get; set; }
        public bool fromMe { get; set; }
        /// <summary>
        /// 发送人
        /// </summary>
        public string from { get; set; }
        public bool toMe { get; set; }
        public string to { get; set; }
        /// <summary>
        /// 消息体
        /// </summary>
        public string content { get; set; }
    }
}