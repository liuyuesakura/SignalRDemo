using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Macrosage.RabbitMQ.Server.Customer;
using Macrosage.RabbitMQ.Server.Product;

/// <summary>
/// Macrosage.RabbitMQ来自RabbitMQ.NETClient.Tool，一个对RabbitMQ.Client的再包装
/// https://github.com/fanpan26/RabbitMQ.NETClient
/// </summary>
namespace MQ
{
    /// <summary>
    /// 禁止继承
    /// </summary>
    public sealed class ChatQueue
    {
        /// <summary>
        /// 聊天消息队列名称
        /// </summary>
        const string QueueName = "LAYIM_CHAT_MSG_QUEUE";
        /// <summary>
        /// 接收到队列消息，进行处理
        /// </summary>
        public static void StartListeningChat()
        {
            IMessageCustomer customer = new MessageCustomer(QueueName);
            //开始消息监听
            customer.StartListening();
            //接收到消息后的回调
            customer.ReceiveMessageCallback = message => {
                //反序列化消息实体
                //var msgModel = JsonHelper.DeserializeObject<ChatMessageResult>(message);
                //调用方法插入数据库（这里没判断是否具体插入成功，只为测试，默认都按照成功处理）
                //UserBLL.AddMessage(msgModel);
                return true;
            };
        }

        /// <summary>
        /// 队列消息发布
        /// </summary>
        /// <param name="message"></param>
        //public static void PublishMessage(ChatMessageResult message)
        //{
        //    IMessageProduct product = new MessageProduct(QueueName);
        //    //将消息序列化之后，发布到队列
        //    var strMessage = JsonHelper.SerializeObject(message);
        //    product.Publish(strMessage);
        //}
    }
}
