using System.Collections.Generic;

using MpWeiXin.Models.Messages.InComingMessages;
using MpWeiXin.Caching;

namespace MpWeiXin.Services.Messages
{
    public class ContextMessageManager
    {
        /// <summary>
        /// 所有上下文消息
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        protected Dictionary<string, ContextMessage> Messages { get; set; }

        /// <summary>
        /// 唯一实例
        /// </summary>
        private static readonly ContextMessageManager _instance;

        /// <summary>
        /// 缓存管理对象
        /// </summary>
        private ICacheManager _cacheMgr;

        /// <summary>
        /// 构造函数
        /// </summary>
        private ContextMessageManager()
        {
            _cacheMgr = new MemoryCacheManager();
            Messages = new Dictionary<string, ContextMessage>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        static ContextMessageManager()
        {
            _instance = new ContextMessageManager();
        }

        /// <summary>
        /// 实例
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static ContextMessageManager Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="contextMessage">The context message.</param>
        public void SetContextMessage(string openId, string context, Message message)
        {
            if (!Messages.ContainsKey(openId))
            {
                var messageList = new ContextMessage();

                messageList.Context = context;
                messageList.Messages.Add(message);

                Messages.Add(openId, messageList);

                _cacheMgr.Set(openId, openId, 15, (args) =>
                {
                    Messages.Remove(openId);
                });
            }
            else
            {
                Messages[openId].Messages.Add(message);
            }
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="openId">The context.</param>
        public void RemoveContextMessage(string openId)
        {
            _cacheMgr.Remove(openId);
        }

        /// <summary>
        /// 获取上下文消息
        /// </summary>
        /// <param name="openId">The context.</param>
        /// <returns></returns>
        public ContextMessage GetContextMessage(string openId)
        {
            if(!Messages.ContainsKey(openId))
            {
                return null;
            }

            return Messages[openId];
        }
    }
}
