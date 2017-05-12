using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SRD.MongoDao;
using SRD.Core;
using SRD.Repository;
using SRD.Model;

namespace SRD.MongoBLL
{
    public class Message
    {
        /// <summary>
        /// 用户创建会话
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool CreateChat(string userid,out string msgGroupId)
        {
            msgGroupId = string.Empty;
            Model.Message m = new Model.Message() 
            {
                Content = "创建会话",
                FromUser = userid,
                MessageKind = (int)Model.Message.MessageKindEnum.System,
            };
            m.LastUpdateTime = DateTime.Now;
            m.MessageGroupId = m.MessageId;
            var result = DBContext.Message.Insert(m) > 0;
            if (result)
                msgGroupId = m.MessageId;
            return result;
        }
        /// <summary>
        /// 加入会话
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool JoinChat(string userid,string msgGroupId)
        {
            Model.Message m = new Model.Message()
            {
                Content = string.Format("用户{0}加入会话",userid),
                FromUser = userid,
                MessageKind = (int)Model.Message.MessageKindEnum.System,
                MessageGroupId = msgGroupId,
                LastUpdateTime = DateTime.Now
            };
            var result = DBContext.Message.Insert(m) > 0;
            if (result)
                msgGroupId = m.MessageId;
            return result;
        }
        /// <summary>
        /// 离开会话
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool LeaveChat(string userid, string msgGroupId)
        {
            Model.Message m = new Model.Message()
            {
                Content = string.Format("用户{0}离开会话", userid),
                FromUser = userid,
                MessageKind = (int)Model.Message.MessageKindEnum.System,
            };
            m.LastUpdateTime = DateTime.Now;
            m.MessageGroupId = m.MessageId;
            var result = DBContext.Message.Insert(m) > 0;
            return result;
        }
        /// <summary>
        /// 关闭会话
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="msgGroupId"></param>
        /// <returns></returns>
        public bool CloseChat(string userid, string msgGroupId)
        {
            Model.Message m = new Model.Message()
            {
                Content = "关闭会话",
                FromUser = userid,
                MessageKind = (int)Model.Message.MessageKindEnum.System,
            };
            m.LastUpdateTime = DateTime.Now;
            m.MessageGroupId = m.MessageId;
            var result = DBContext.Message.Insert(m) > 0;
            return result;
        }
        public bool Invite(string inviter, string invitee, string msgGroupId)
        {
            Model.Message m = new Model.Message()
            {
                Content = string.Format("{0}邀请{1}加入会话", inviter,invitee),
                FromUser = inviter,
                MessageKind = (int)Model.Message.MessageKindEnum.System,
            };
            m.LastUpdateTime = DateTime.Now;
            m.MessageGroupId = m.MessageId;
            var result = DBContext.Message.Insert(m) > 0;
            return result;
        }
        public bool Insert(Model.Message item)
        {
            var result = DBContext.Message.Insert(item) > 0;
            return result;
        }
        /// <summary>
        /// 查询会话涉及到的所有用户
        /// </summary>
        /// <param name="msgGroupId"></param>
        /// <returns></returns>
        public List<string> GetRelevantUserList(string msgGroupId)
        {
            var result = DBContext.Message.List<string>(p=>p.MessageGroupId == msgGroupId,y=>y.FromUser).Distinct().ToList();
            return result;
        }
        /// <summary>
        /// 获取会话中的全部聊天记录
        /// </summary>
        /// <param name="msgGroupId"></param>
        /// <returns></returns>
        public List<Model.Message> GetChat(string msgGroupId)
        {
            var result = DBContext.Message.List<Model.Message>(p=>p.MessageGroupId == msgGroupId,y=>y);
            return result;
        }
    }
}
