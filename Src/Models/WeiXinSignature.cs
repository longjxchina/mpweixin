using System;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace MpWeiXin.Models
{
    public class WeiXinSignature
    {
        private HttpRequestBase _request;        

        public WeiXinSignature(HttpRequestBase request)
        {
            _request = request;
        }

        #region 变量区

        /// <summary>
        /// 查询字符参参数 signature
        /// </summary>
        public const string QS_SIGNATURE = "signature";

        /// <summary>
        /// 查询字符参参数 timestamp
        /// </summary>
        public const string QS_TIMESTAMP = "timestamp";

        /// <summary>
        /// 查询字符参参数 nonce
        /// </summary>
        public const string QS_NONCE = "nonce";

        /// <summary>
        /// 查询字符参参数 echostr
        /// </summary>
        public const string QS_ECHOSTR = "echostr";

        /// <summary>
        /// 获取 signature.
        /// </summary>
        /// <value>
        /// The signature.
        /// </value>
        public string Signature
        {
            get
            {
                return _request.QueryString[QS_SIGNATURE];
            }
        }

        /// <summary>
        /// 获取 Nonce.
        /// </summary>
        /// <value>
        /// The Nonce.
        /// </value>
        public string Nonce
        {
            get
            {
                return _request.QueryString[QS_NONCE];
            }
        }

        /// <summary>
        /// 获取 Timestamp.
        /// </summary>
        /// <value>
        /// The Timestamp.
        /// </value>
        public string Timestamp
        {
            get
            {
                return _request.QueryString[QS_TIMESTAMP];
            }
        }

        /// <summary>
        /// 获取 Echostr.
        /// </summary>
        /// <value>
        /// The Echostr.
        /// </value>
        public string Echostr
        {
            get
            {
                return _request.QueryString[QS_ECHOSTR];
            }
        }

        #endregion

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <returns></returns>
        public bool ValidateSignature()
        {
            var arrParams = new string[] { WxConfig.TOKEN, Timestamp, Nonce };

            Array.Sort(arrParams);

            var content = string.Join(string.Empty, arrParams);
            var encrypted = Encrypt(content);

            return encrypted.Equals(Signature);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        private string Encrypt(string source)
        {
            var bytes = Encoding.Default.GetBytes(source);
            var sha = new SHA1CryptoServiceProvider();

            bytes = sha.ComputeHash(bytes);

            var res = new StringBuilder();

            foreach (var b in bytes)
            {
                res.AppendFormat("{0:x2}", b);
            }

            return res.ToString();
        }
    }
}