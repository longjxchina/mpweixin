using MpWeiXin.Utils;
using MpWeiXin.Models.AccessTokens;
using System.Configuration;

namespace MpWeiXin.Services
{
    /// <summary>
    /// access token服务
    /// </summary>
    public class WxAccessTokenService
    {
        private const string ACCESS_TOKEN_CACHE_KEY = "ACCESS_TOKEN_CACHE_KEY";
        private const string ACCESS_TOKEN_API = "https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}";

        public WxAccessTokenService()
        {
            
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
        {
            var _cacheMgr = new MemoryCacheManager();
            var token = _cacheMgr.Get<string>(ACCESS_TOKEN_CACHE_KEY, 0, () =>
             {
                 var isDebug = ConfigurationManager.AppSettings["IsDevelop"] == "1";

                 if (isDebug)
                 {
                    return ConfigurationManager.AppSettings["WxToken"];
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
                     if (_cacheMgr.IsSet(ACCESS_TOKEN_CACHE_KEY))
                     {
                         _cacheMgr.Remove(ACCESS_TOKEN_CACHE_KEY);
                     }

                     _cacheMgr.Set(ACCESS_TOKEN_CACHE_KEY, tokenResult.access_token, tokenResult.expires_in / 60);
                     Log.Error(string.Format("获取Token：{0}, 过期时间：{1}", tokenResult.access_token, tokenResult.expires_in));
                 }

                 return tokenResult.access_token;
             });

            return token;
        }
    }
}