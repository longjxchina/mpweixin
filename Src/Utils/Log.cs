using System;
using System.IO;

namespace MpWeiXin.Utils
{
    /// <summary>
    /// 日志
    /// </summary>
    internal static class Log
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static Log()
        {
            Config();
        }

        public static void Config()
        {
            var logConfigFilePath = System.Web.HttpContext.Current.Server.MapPath("~/config/log4net.config");

            if (!File.Exists(logConfigFilePath))
            {
                throw new FileNotFoundException("未找到config/log4net.config日志配置文件");
            }

            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(logConfigFilePath));
            Log.Debug(logConfigFilePath);
        }

        public static void Info(object message, Exception ex = null)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(message, ex);
            }
        }

        public static void Debug(object message, Exception ex = null)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(message, ex);
            }
        }

        public static void Error(object message, Exception ex = null)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(message, ex);
            }
        }

        public static void Fatal(object message, Exception ex = null)
        {
            if (log.IsFatalEnabled)
            {
                log.Fatal(message, ex);
            }
        }
    }
}
