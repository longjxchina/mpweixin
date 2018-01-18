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

        private WxAccessTokenService accessTokenSvc;

        public CustomerService(
            WxAccessTokenService accessTokenSvc)
        {
            this.accessTokenSvc = accessTokenSvc;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">The message.</param>
        public WxError Send(CustomerMessage message)
        {
            var api = string.Format(SEND_API, accessTokenSvc.GetToken());

            return WxHelper.Send<WxError>(api, message);
        }
    }
}