using System.Configuration;

namespace MpWeiXin.Models
{
    /// <summary>
    /// 微信配置
    /// </summary>
    internal class WxConfig
    {
        /// <summary>
        /// The ap p_ identifier
        /// </summary>
        public static readonly string AppId = ConfigurationManager.AppSettings["WxAppId"];

        /// <summary>
        /// The ap p_ secret
        /// </summary>
        public static readonly string AppSecret = ConfigurationManager.AppSettings["WxAppSecret"];

        /// <summary>
        /// The token
        /// </summary>
        public static readonly string Token = ConfigurationManager.AppSettings["WxAppToken"];

        /// <summary>
        /// The is develop
        /// </summary>
        public static readonly bool IsDebug = ConfigurationManager.AppSettings["WxDebug"] == "1";
    }
}