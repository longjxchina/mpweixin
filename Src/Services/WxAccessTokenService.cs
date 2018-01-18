
using MpWeiXin.Caching;
using MpWeiXin.Models;
using MpWeiXin.Models.AccessTokens;
using MpWeiXin.Utils;

namespace MpWeiXin.Services
{
    /// <summary>
    /// access token服务
    /// </summary>
    public class WxAccessTokenService
    {
        private const string ACCESS_TOKEN_CACHE_KEY = "ACCESS_TOKEN_CACHE_KEY";
        private const string ACCESS_TOKEN_API = "https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}";

        /// <summary>
        /// The cache MGR
        /// </summary>
        private ICacheManager cacheMgr;

        /// <summary>
        /// Initializes a new instance of the <see cref="WxAccessTokenService"/> class.
        /// </summary>
        /// <param name="cacheMgr">The cache MGR.</param>
        public WxAccessTokenService(
            ICacheManager cacheMgr)
        {
            this.cacheMgr = cacheMgr;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            var token = cacheMgr.Get(ACCESS_TOKEN_CACHE_KEY, 0, () =>
             {
                 var isDebug = WxConfig.IsDebug;

                 if (isDebug)
                 {
                     var result = WxConfig.WxAccessToken;

                     if (!string.IsNullOrEmpty(result))
                     {
                         return result;
                     }
                 }

                 var request = new AccessTokenRequest();
                 var api = string.Format(ACCESS_TOKEN_API,
                                         request.grant_type,
                                         request.appid,
                                         request.secret);
                 var tokenResult = WxHelper.Send<AccessTokenResponse>(api);

                 if (tokenResult == null)
                 {
                     return null;
                 }
                 else
                 {
                     if (cacheMgr.IsSet(ACCESS_TOKEN_CACHE_KEY))
                     {
                         cacheMgr.Remove(ACCESS_TOKEN_CACHE_KEY);
                     }

                     cacheMgr.Set(ACCESS_TOKEN_CACHE_KEY, tokenResult.access_token, tokenResult.expires_in / 60);

                     Log.Info(string.Format("获取Token：{0}, 过期时间：{1}", tokenResult.access_token, tokenResult.expires_in));
                 }

                 return tokenResult.access_token;
             });

            return token;
        }
    }
}