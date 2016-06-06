using System.Collections.Generic;

using MpWeiXin.Models.Messages.InComingMessages;

namespace MpWeiXin.Services
{
    public interface IMessageContext
    {
        /// <summary>
        /// 添加消息 
        /// </summary>
        /// <param name="openId">The open identifier.</param>
        /// <param name="message">The message.</param>
        void AddMessage(string openId, ContextMessage message);

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="opendId">The opend identifier.</param>
        /// <param name="message">The message.</param>
        void RemoveMessage(string opendId, ContextMessage message);

        /// <summary>
        /// 获取上下文消息列表
        /// </summary>
        /// <param name="openId">The open identifier.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        IList<ContextMessage> GetMessages(string openId, string context);
    }
}
