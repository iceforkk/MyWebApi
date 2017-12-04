using log4net;
using System;
using Community.Common;

namespace Community.WebApi.Common
{
    public class LogHelper
    {
        /// <summary>
        /// Log
        /// </summary>
        private static object lockHelper = new object();

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="logName">Log名</param>
        /// <param name="level">Log等级</param>
        /// <param name="formatString">Message</param>
        /// <param name="args"></param>
        /// <param name="ex">异常</param>
        public static void Output(string logName, ErrorLevel level, string formatString, object[] args, Exception ex)
        {
            lock (lockHelper)
            {
                ILog log = LogManager.GetLogger(logName);

                bool bol_HasException = false;

                string message = formatString;

                if (args != null && args.Length > 0)
                {
                    message = string.Format(formatString.Replace("{", "{{").Replace("}", "}}"), args);
                }

                if (ex != null)
                    bol_HasException = true;
                switch (level)
                {
                    case ErrorLevel.INFO:
                        if (log.IsInfoEnabled == false)
                            return;
                        if (bol_HasException == true)
                            log.Info(message, ex);
                        else
                            log.Info(message);
                        break;

                    case ErrorLevel.DEBUG:
                        if (log.IsDebugEnabled == false)
                            return;
                        if (bol_HasException == true)
                            log.Debug(message, ex);
                        else
                            log.Debug(message);
                        break;

                    case ErrorLevel.WARN:
                        if (log.IsWarnEnabled == false)
                            return;
                        if (bol_HasException == true)
                            log.Warn(message, ex);
                        else
                            log.Warn(message);
                        break;

                    case ErrorLevel.ERROR:
                        if (log.IsErrorEnabled == false)
                            return;
                        if (bol_HasException == true)
                            log.Error(message, ex);
                        else
                            log.Error(message);
                        break;

                    case ErrorLevel.FATAL:
                        if (log.IsFatalEnabled == false)
                            return;
                        if (bol_HasException == true)
                            log.Fatal(message, ex);
                        else
                            log.Fatal(message);
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="logName">Log名</param>
        /// <param name="level">Log等级</param>
        /// <param name="prm_string_FormatString">Message</param>
        /// <param name="prm_object_Args"></param>
        /// <param name="prm_exception_ex">异常</param>
        public static void Output(string logName, ErrorLevel level, string message)
        {
            Output(logName, level, message, new string[] { }, null);
        }

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="logName">Log名</param>
        /// <param name="level">Log等级</param>
        /// <param name="prm_exception_ex">异常</param>
        public static void Output(string logName, ErrorLevel level, Exception ex)
        {
            Output(logName, level, string.Empty, new string[] { }, ex);
        }
    }
}