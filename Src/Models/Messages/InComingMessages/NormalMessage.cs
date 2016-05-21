using System.IO;

namespace MpWeiXin.Models.Messages.InComingMessages
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class NormalMessage : Message
    {
        public NormalMessage(string originMessage)
            : base(originMessage)
            { }
    }
}