using MpWeiXin.Models.WebAuths;

namespace MpWeiXin.Services
{
    /// <summary>
    /// 微信网页授权服务
    /// </summary>
    public class WxWebAuthService
    {
        private const string ACCESS_TOKEN_API = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type={3}";

        /// <summary>
        /// 获取Access Token
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static WebAuthAccessTokenResponse GetAccessToken(string code)
        {
            var request = new WebAuthAccessTokenRequest(code);
            var requestUrl = string.Format(ACCESS_TOKEN_API
                                           , WebAuthAccessTokenRequest.appid
                                           , WebAuthAccessTokenRequest.secret
                                           , request.code
                                           , WebAuthAccessTokenRequest.grant_type);

            var response = WxHelper.Send<WebAuthAccessTokenResponse>(requestUrl);

            return response;
        }
    }
}
