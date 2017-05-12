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
    /// 聊天消息记录表
    /// 由于设定的对话组ID，不再需要记录接收人
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Message:UpdateEntity
    {
        /// <summary>
        /// 消息记录ID
        /// </summary>
        [BsonId]
        public string MessageId { set; get; }
        /// <summary>
        /// 对话组ID
        /// </summary>
        public string MessageGroupId { set; get; }
        /// <summary>
        /// 消息类型，普通消息还是系统消息
        /// </summary>
        public int MessageKind { set; get; }
        /// <summary>
        /// 消息类型，文本、图片或者其他，消息类型不为文本时，Content中存放消息本体的物理路径
        /// </summary>
        public int MessageType { set; get; }
        /// <summary>
        /// 消息发送人
        /// </summary>
        public string FromUser { set; get; }
        /// <summary>
        /// 聊天内容
        /// </summary>
        public string Content { set; get; }
        public enum MessageKindEnum
        {
            /// <summary>
            /// 普通聊天消息
            /// </summary>
            Common = 0,
            /// <summary>
            /// 系统聊天消息
            /// </summary>
            System = 1,
        }
        public enum MessageTypeEnum
        {
            /// <summary>
            /// 文字
            /// </summary>
            Text = 0,
            /// <summary>
            /// 图片
            /// </summary>
            Img = 1,
            /// <summary>
            /// 语音
            /// </summary>
            Voice = 2,
            /// <summary>
            /// 视频
            /// </summary>
            Video = 3,
        }
        public Message()
        {
            NeedUpdate = true;
            this.MessageId = Guid.NewGuid().ToString();
            MessageType = (int)Model.Message.MessageTypeEnum.Text;
        }
    }
}
