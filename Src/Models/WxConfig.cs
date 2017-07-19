using System.Configuration;

namespace MpWeiXin.Models
{
    /// <summary>
    /// 微信配置
    /// </summary>
    internal class WxConfig
    {
        /// <summary>
        /// 微信app id
        /// </summary>
        public static readonly string AppId = ConfigurationManager.AppSettings["WxAppId"];

        /// <summary>
        /// 微信app密钥
        /// </summary>
        public static readonly string AppSecret = ConfigurationManager.AppSettings["WxAppSecret"];

        /// <summary>
        /// 微信app token
        /// </summary>
        public static readonly string Token = ConfigurationManager.AppSettings["WxAppToken"];

        /// <summary>
        /// 获取选项是否为开发模式
        /// </summary>
        public static readonly bool IsDebug = ConfigurationManager.AppSettings["WxDebug"] == "1";
        
        /// <summary>
        /// IsDebug为true时设置的AccessToken
        /// </summary>
        public static readonly string WxAccessToken = ConfigurationManager.AppSettings["WxAccessToken"];
    }
}