using MpWeiXin.Utils;
using System;

namespace MpWeiXin.Models.Messages.InComingMessages
{
    /// <summary>
    /// 扫描二维码订阅事件通知
    /// </summary>
    [Serializable]
    public class QrCodeScanEventMessage : EventMessage
    {
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public string Ticket { get; set; }

        /// <summary>
        /// 场景
        /// </summary>
        /// <value>
        /// The scene.
        /// </value>
        public int Scene
        {
            get
            {
                var scene = -1;
                var strScene = EventKey;
                var prefix = "qrscene_";

                if (strScene.StartsWith(prefix))
                {
                    Log.Debug("原始值:" + strScene);
                    strScene = strScene.Substring(prefix.Length);
                    Log.Debug("新值:" + strScene);
                }

                int.TryParse(strScene, out scene);

                return scene;
            }
        }

        public QrCodeScanEventMessage()
        {

        }

        public QrCodeScanEventMessage(string originMessage)
            : base(originMessage)
            { }
    }
}