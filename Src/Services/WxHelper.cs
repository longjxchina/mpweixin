using System;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;

using MpWeiXin.Utils;
using MpWeiXin.Models;
using MpWeiXin.Models.Messages.ReplyMessages;
using MpWeiXin.Models.Messages.InComingMessages;
using MpWeiXin.Models.Messages;

namespace MpWeiXin.Services
{
    /// <summary>
    /// 微信发送帮助
    /// </summary>
    public class WxHelper
    {
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api">The API.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static T Send<T>(string api, object data = null) where T : class
        {
            Action<HttpContent> errCallBack = (httpContent) =>
            {
                try
                {
                    var error = httpContent.ReadAsAsync<WxError>().Result;

                    if (error != null && error.HasError())
                    {
                        var errMsg = string.Format("请求出错信息：错误代码：{0}，错误消息：{1}", error.errcode, error.errmsg);

                        Log.Error(errMsg);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("请求{0}出错", api), ex);
                }
            };

            Action<HttpContent, Exception> exceptionHandler = (httpContent, ex) =>
            {
                string content = null;

                if (httpContent != null)
                {
                    content = httpContent.ReadAsStringAsync().ToJson();
                }

                Log.Error(string.Format("请求出错{0}，错误详情：{1}，内容：{2}", api, ex.Message, content), ex);
            };

            return RequestHelper.Send<T>(api, data, errCallBack, exceptionHandler);
        }

        /// <summary>
        /// 将消息序列化后返回
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        public static string FormatMessage(ReplyMessage msg)
        {
            if (msg == null)
            {
                return string.Empty;
            }

            var type = msg.GetType();
            var serializer = new XmlSerializer(type);
            var sbXml = new StringBuilder();
            var sw = new StringWriter(sbXml);

            try
            {
                serializer.Serialize(sw, msg);
                return ChangeRoot(sbXml);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 修改Xml根节点
        /// 1. 修改根节点名称为xml
        /// 2. 删除根节点的所有属性
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string ChangeRoot(StringBuilder xml)
        {
            var sr = new StringReader(xml.ToString());
            var doc = XDocument.Load(sr);
            var root = doc.Root;
            var typeName = "xml";

            root.Name = typeName;
            root.RemoveAttributes();

            return doc.ToString();
        }

        public static long ChangeTimeFormat(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 回复文本消息
        /// </summary>
        /// <param name="fromMsg">From MSG.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static ReplyMessage ReplyTextMessage(Message fromMsg, string content)
        {
            var msg = new ReplyTextMessage();

            msg.ToUserName = fromMsg.FromUserName;
            msg.FromUserName = fromMsg.ToUserName;
            msg.CreateTime = ChangeTimeFormat(DateTime.Now);
            msg.MsgType = MessageType.Text.ToString().ToLower();
            msg.Content = content;

            return msg;
        }
    }
}