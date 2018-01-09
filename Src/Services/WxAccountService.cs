using MpWeiXin.Caching;
using MpWeiXin.Models.Accounts;

namespace MpWeiXin.Services
{
    /// <summary>
    /// 微信账号服务
    /// </summary>
    public class WxAccountService
    {
        #region 生成带参数的二维码

        /// <summary>
        /// 获取临时二维码ticket api
        /// </summary>
        private const string TEMP_QR_CODE_TICKET_API = " https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";

        /// <summary>
        /// 获取二维码
        /// </summary>
        private const string QR_CODE_API = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";

        /// <summary>
        /// The qr code secene
        /// </summary>
        private const string QR_CODE_SECENE = "QR_CODE_SECENE_{0}";
        /// <summary>
        /// The cach MGR
        /// </summary>
        private ICacheManager cachMgr;
        /// <summary>
        /// The account SVC
        /// </summary>
        private WxAccountService accountSvc;

        /// <summary>
        /// Initializes a new instance of the <see cref="WxAccountService" /> class.
        /// </summary>
        /// <param name="cachMgr">The cach MGR.</param>
        /// <param name="accountSvc">The account SVC.</param>
        public WxAccountService(
            ICacheManager cachMgr,
            WxAccountService accountSvc)
        {
            this.cachMgr = cachMgr;
            this.accountSvc = accountSvc;
        }

        /// <summary>
        /// 获取临时二维码
        /// </summary>
        /// <param name="sceneId">The scene identifier.</param>
        /// <returns></returns>
        public string GetQrCode(int sceneId)
        {
            var ticket = GetQrCodeTicket(sceneId);
            
            return string.Format(QR_CODE_API, ticket);
        }

        /// <summary>
        /// 创建二维码ticket, 临时二维码请求ticket
        /// </summary>
        public string GetQrCodeTicket(int sceneId)
        {
            const int expire = 10;
            var api = string.Format(TEMP_QR_CODE_TICKET_API, WxAccessTokenService.GetToken());
            var request = new QrCodeRequest()
            {   
                expire_seconds = expire,
                action_name = "QR_SCENE",
                action_info = new QrCodeActionInfo()
                {
                    scene = new Scene()
                    {
                        scene_id = sceneId
                    }
                }
            };
            var response = WxHelper.Send<QrCodeResponse>(api, request);            

            if (response == null)
            {
                return null;
            }

            #region 缓存场景id

            var key = string.Format(QR_CODE_SECENE, sceneId);

            if (cachMgr.IsSet(key))
            {
                cachMgr.Remove(key);
            }

            cachMgr.Set(key, sceneId, expire);

            #endregion

            return response.ticket;
        }

        /// <summary>
        /// 检查场景是否匹配
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <returns></returns>
        public bool CheckScene(int scene)
        {
            var key = string.Format(QR_CODE_SECENE, scene);
            var isValid = cachMgr.IsSet(key);

            if (isValid)
            {
                cachMgr.Remove(key);
            }

            return isValid;
        }

        #endregion
    }
}