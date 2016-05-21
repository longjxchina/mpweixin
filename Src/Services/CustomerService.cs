using MpWeiXin.Models;
using MpWeiXin.Models.CustomerServices;

namespace MpWeiXin.Services
{
    /// <summary>
    /// 客服服务
    /// </summary>
    public class CustomerService
    {
        public const string SEND_API = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">The message.</param>
        public static WxError Send(CustomerMessage message)
        {
            var api = string.Format(SEND_API, WxAccessTokenService.GetToken());

            return WxHelper.Send<WxError>(api, message);
        }
    }
}