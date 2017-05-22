using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
            List<Models.VueModel.LeftUserModel> leftUserList = new List<Models.VueModel.LeftUserModel>();
            leftUserList.Add(new Models.VueModel.LeftUserModel() { 
                 groupId = "groupid",
                 headImg = "https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_03.jpg",
                  isGroup = false,
                   onlineStatus = "I`m online!",
                    userName = "It`s a username"
            });
            Clients.Caller.setRoomList(Newtonsoft.Json.JsonConvert.SerializeObject(leftUserList));
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

            string creator = "0079"; //OnlineUser.Instance.GetCurrentUserID();

            RoomCacheModel cache = new RoomCacheModel()
            {
                RoomId = roomid,
                UserList = new List<string>() { 
                    userid,
                    creator
                }
            };
            SRD.Cache.CacheManager.SetRedisContent<RoomCacheModel>(roomid, cache);

            GetRoomList(userid);
            Clients.Group(roomid, new string[0]).sendMessage(message);
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
        /// <summary>
        /// 用户上线
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {

            string connectionid = Context.ConnectionId;
            Random r = new Random();
            int checkcodeSeed = r.Next(10000,99999);
            string checkCode = SRD.Helper.AES.Encrypt(checkcodeSeed.ToString());
            UserCacheModel userCM = new UserCacheModel()
            {
                CheckCode = checkCode,
                ConnectionId = Context.ConnectionId,
            };
            SRD.Cache.CacheManager.SetRedisContent(Context.ConnectionId, userCM);

            Clients.Caller.setCheckCode(checkCode);

            //更新其他用户的在线列表 -- 
            return base.OnConnected();
        }
        public void Login(string checkcode,string userid)
        {
            string connectionid = Context.ConnectionId;
            UserCacheModel userCM = SRD.Cache.CacheManager.GetRedisContent<UserCacheModel>(connectionid);
            userCM.HeadImg = "https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_04.jpg";
            userCM.UserId = userid;
            userCM.UserKind = 0;
            userCM.UserName = userid;
            SRD.Cache.CacheManager.SetRedisContent(connectionid,userCM);
            GetFriendList(userid,connectionid);
        }
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            //更新其他用户的在线用户列表
            //string disuser = OnlineUser.Instance.GetCurrentUserID();

            return base.OnDisconnected(true);
        }

        /// <summary>
        /// 聊天房间列表
        /// </summary>
        public void GetRoomList(string userid)
        {
            List<Models.VueModel.LeftUserModel> leftUserList = new List<Models.VueModel.LeftUserModel>();
            leftUserList.Add(new Models.VueModel.LeftUserModel()
            {
                groupId = "groupid",
                headImg = "https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_03.jpg",
                isGroup = false,
                onlineStatus = "I`m online!",
                userName = userid
            });
            Clients.Caller.getRoomList(Newtonsoft.Json.JsonConvert.SerializeObject(leftUserList));
        }
        public void GetFriendList(string friendid,string connectionid)
        {
            List<Models.VueModel.RightFriendModel> rightFriendList = new List<Models.VueModel.RightFriendModel>();
            rightFriendList.Add(new Models.VueModel.RightFriendModel() 
            {
                connectionId = connectionid,
                headImg = "https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_04.jpg",
                 isOnline = true,
                  onlineStatus = "I`m OnLINE!",
                   userId = friendid,
                   userName = friendid
            });
            Clients.AllExcept(new string[1]{connectionid}).getFriendList(Newtonsoft.Json.JsonConvert.SerializeObject(rightFriendList));
        }
    }
}