using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRDemo.Hubs
{
    public class RoomCacheModel
    {
        /// <summary>
        /// SRD.Model.Message 中的MassageGroupId
        /// </summary>
        public string RoomId { set; get; }

        public List<string> UserList { set; get; }
        public int RoomKind { set; get; }
        public DateTime StartTime { set; get; }
        
        public RoomCacheModel()
        {
            StartTime = DateTime.Now;
        }

        public enum RoomKindEnum
        {
            /// <summary>
            /// 私人一对一聊天房间
            /// </summary>
            Private_1to1 = 0,
            /// <summary>
            /// 私有群组
            /// </summary>
            Private_Group = 1,
            /// <summary>
            /// 开放群组
            /// </summary>
            Public_Group = 2
        }
    }
}