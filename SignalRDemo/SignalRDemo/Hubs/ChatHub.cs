using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRDemo.Hubs
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        protected SRD.MongoBLL.Message MsgInstance = new SRD.MongoBLL.Message();
        //public void Hello()
        //{
        //    Clients.All.hello();
        //}
        /// <summary>
        /// 创建会话
        /// </summary>
        /// <param name="userid"></param>
        public void CreateChat(string userid)
        {

            string roomid = string.Empty;
            //写入数据库

            MsgInstance.CreateChat(userid, out roomid);

            Groups.Add(Context.ConnectionId,roomid);
            //调用此连接用户的本地JS(显示房间)
            //Clients.Client(Context.ConnectionId).addRoom(roomid);

            RoomCacheModel cache = new RoomCacheModel()
            {
                RoomId = roomid,
                UserList = new List<string>() { 
                    userid
                }
            };
            SRD.Cache.CacheManager.SetRedisContent<RoomCacheModel>(roomid,cache);
            Clients.Group(roomid, new string[0]).sendMessage("创建房间成功~房间号为" + roomid);
            //异步去拉取一个客服进入聊天
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid">目标用户的USERID</param>
        /// <param name="message">发送给目标用户的信息</param>
        public void CreateChat(string userid, string message)
        {
            string roomid = string.Empty;
            //写入数据库

            MsgInstance.CreateChat(userid, out roomid);

            Groups.Add(Context.ConnectionId, roomid);
            //调用此连接用户的本地JS(显示房间)
            //Clients.Client(Context.ConnectionId).addRoom(roomid);

            string creator = OnlineUser.Instance.GetCurrentUserID();

            RoomCacheModel cache = new RoomCacheModel()
            {
                RoomId = roomid,
                UserList = new List<string>() { 
                    userid,
                    creator
                }
            };
            SRD.Cache.CacheManager.SetRedisContent<RoomCacheModel>(roomid, cache);
            Clients.Group(roomid, new string[1]{creator}).sendMessage(message);
        }
        public void JoinChat(string userid,string roomid)
        {
            RoomCacheModel cache = SRD.Cache.CacheManager.GetRedisContent<RoomCacheModel>(roomid);
            if (cache == null || cache == new RoomCacheModel() || cache == default(RoomCacheModel))
            {
                Clients.Client(Context.ConnectionId).showMessage("房间不存在!");
                return;
            }
            else if (cache.RoomKind != (int)RoomCacheModel.RoomKindEnum.Public_Group)
            {
                Clients.Client(Context.ConnectionId).showMessage("您不能直接加入私人房间!");
                return;
            }
            if (cache.UserList.Contains(userid))
            {
                Clients.Client(Context.ConnectionId).showMessage("您已经在房间里了!");
                return;
            }
            //更新缓存
            cache.UserList.Add(userid);
            SRD.Cache.CacheManager.SetRedisContent<RoomCacheModel>(roomid,cache);
            //添加用户进组
            Groups.Add(Context.ConnectionId,roomid);
            Clients.Group(roomid, new string[0]).sendMessage("欢迎" + userid + "加入本房间" + DateTime.Now.ToShortTimeString());
            //写入数据库
            MsgInstance.JoinChat(userid,roomid);
        }
        public void SendMessage(string room, string userid, string message)
        {
            MsgInstance.Insert(new SRD.Model.Message()
            {
                Content = message,
                FromUser = userid,
                MessageGroupId = room,
                MessageKind = (int)SRD.Model.Message.MessageKindEnum.Common,
                MessageType = (int)SRD.Model.Message.MessageTypeEnum.Text
            });
            Clients.Group(room, new string[0]).sendMessage(message + " " + DateTime.Now.ToShortTimeString());
        }
    }
}