using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MpWeiXin.Models.WebAuths
{
    /// <summary>
    /// 通过code换取网页授权access_token请求
    /// </summary>
    public class WebAuthAccessTokenRequest
    {
        public const string grant_type = "authorization_code";
        public static readonly string appid = WxConfig.APP_ID;
        public static readonly string secret = WxConfig.APP_SECRET;
        public string code { get; set; }

        public WebAuthAccessTokenRequest(string requestCode)
        {
            code = requestCode;
        }
    }
}
