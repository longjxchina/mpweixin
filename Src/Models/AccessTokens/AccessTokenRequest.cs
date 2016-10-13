namespace MpWeiXin.Models.AccessTokens
{
    /// <summary>
    /// access_token
    /// </summary>
    public class AccessTokenRequest
    {
        public AccessTokenRequest()
        {
            grant_type = "client_credential";
            appid = WxConfig.AppId;
            secret = WxConfig.AppSecret;
        }

        /// <summary>
        /// 获取access_token填写client_credential
        /// </summary>
        /// <value>
        /// The grant_type.
        /// </value>
        public string grant_type { get; set; }

        /// <summary>
        /// 第三方用户唯一凭证
        /// </summary>
        /// <value>
        /// The appid.
        /// </value>
        public string appid { get; set; }

        /// <summary>
        /// 第三方用户唯一凭证密钥，即appsecret
        /// </summary>
        /// <value>
        /// The secret.
        /// </value>
        public string secret { get; set; }
    }
}