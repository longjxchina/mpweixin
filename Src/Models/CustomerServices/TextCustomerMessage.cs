using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MpWeiXin.Models.CustomerServices
{
    /// <summary>
    /// 客服文本消息
    /// </summary>
    /// <seealso cref="MpWeiXin.Models.CustomerServices.CustomerMessage" />
    public class TextCustomerMessage : CustomerMessage
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        public class TextMessage
        {
            public string content { get; set; }
        }

        public TextMessage text { get; set; }
    }
}