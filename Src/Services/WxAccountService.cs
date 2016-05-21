using MpWeiXin.Utils;
using MpWeiXin.Utils;
using MpWeiXin.Models.Accounts;
using System.Collections.Generic;

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

        private const string QR_CODE_SECENE = "QR_CODE_SECENE_{0}";
        private ICacheManager _cachMgr;

        public WxAccountService()
        {
            _cachMgr = new MemoryCacheManager();
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
            var accountSvc = new WxAccountService();
            var response = WxHelper.Send<QrCodeResponse>(api, request);            

            if (response == null)
            {
                return null;
            }

            #region 缓存场景id

            var key = string.Format(QR_CODE_SECENE, sceneId);

            if (_cachMgr.IsSet(key))
            {
                _cachMgr.Remove(key);
            }

            _cachMgr.Set(key, sceneId, expire);

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
            var isValid = _cachMgr.IsSet(key);

            if (isValid)
            {
                _cachMgr.Remove(key);
            }

            return isValid;
        }

        #endregion
    }
}