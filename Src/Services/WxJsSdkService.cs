//-----------------------------------------------------------------------
// <copyright file="WxJsSdkService.cs" company="long">
//     Copyright (c) long. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using MpWeiXin.Caching;
using MpWeiXin.Models;
using MpWeiXin.Models.JsSdks;
using MpWeiXin.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace MpWeiXin.Services
{
    /// <summary>
    /// 微信js-sdk服务
    /// </summary>
    public class WxJsSdkService
    {
        /// <summary>
        /// The jsapi ticket API
        /// </summary>
        private const string JSAPI_TICKET_API = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";

        /// <summary>
        /// The jsapi ticket cache key
        /// </summary>
        private const string JSAPI_TICKET__CACHE_KEY = "JSAPI_TICKET__CACHE_KEY";

        private WxAccessTokenService accessTokenSvc;

        private ICacheManager cacheMgr;

        public WxJsSdkService(
            WxAccessTokenService accessTokenSvc,
            ICacheManager cacheMgr)
        {
            this.accessTokenSvc = accessTokenSvc;
            this.cacheMgr = cacheMgr;
        }

        /// <summary>
        /// 获取js api ticket
        /// </summary>
        /// <returns>ticket</returns>
        public string GetJsApiTicket()
        {
            var token = cacheMgr.Get(JSAPI_TICKET__CACHE_KEY, 0, () =>
            {
                var api = string.Format(JSAPI_TICKET_API, accessTokenSvc.GetToken());
                var ticket = WxHelper.Send<JsApiTicket>(api, null, HttpMethod.Get);

                if (ticket != null)
                {
                    if (cacheMgr.IsSet(JSAPI_TICKET__CACHE_KEY))
                    {
                        cacheMgr.Remove(JSAPI_TICKET__CACHE_KEY);
                    }

                    string theTicket = ticket.ticket;

                    cacheMgr.Set(JSAPI_TICKET__CACHE_KEY, theTicket, ticket.expires_in / 60);

                    Log.Info(string.Format("获取JsApiTicket：{0}", theTicket));

                    return ticket.ticket;
                }

                return null;
            });

            return token;
        }

        /// <summary>
        /// Gets the sign.
        /// </summary>
        /// <returns></returns>
        public JsApiSign GetSignature(string url)
        {
            var sign = new JsApiSign
            {                
                NonceStr = Guid.NewGuid().ToString("N"),
                TimeStamp = GetTimeStamp(),
            };

            var ticket = GetJsApiTicket();
            var dicts = new Dictionary<string, string>();

            dicts.Add("noncestr", sign.NonceStr);
            dicts.Add("jsapi_ticket", ticket);
            dicts.Add("timestamp", sign.TimeStamp.ToString());

            if (!string.IsNullOrEmpty(url))
            {
                url = url.Split('#')[0];
            }

            dicts.Add("url", url);

            var arrSignature = dicts
                .OrderBy(m => m.Key, StringComparer.Ordinal)
                .Select(m =>
                {
                    return string.Format("{0}={1}", m.Key, m.Value);
                })
                .ToArray();
            var forSign = string.Join("&", arrSignature);

            sign.Signature = ShaHelper.Sha1(forSign);
            sign.AppId = WxConfig.AppId;

            return sign;
        }

        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <returns></returns>
        private int GetTimeStamp()
        {
            var now = DateTime.Now;
            DateTime initTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1));
            TimeSpan span = now - initTime;

            return Convert.ToInt32(span.TotalSeconds);
        }
    }
}
