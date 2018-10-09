using System;
using log4net;
using System.Text;
using System.Collections.Generic;

namespace SFO2O.Utility.Uitl
{

    /// <summary>
    /// 
    /// </summary>
    public static class LogHelper
    {
        private static string site = Environment.MachineName + "：";
        public static ILog GetLogger(Type obj)
        {
            return log4net.LogManager.GetLogger(obj);
        }

        //  static ILog log = log4net.LogManager.GetLogger(ConfigHelper.Logger);
        public static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        ///// <summary>
        ///// Errors the specified MSG.
        ///// </summary>
        ///// <param name="msg">The MSG.</param>
        //public static void Error(object msg)
        //{
        //    log.Error(msg);
        //}

        ///// <summary>
        ///// Errors the specified ex.
        ///// </summary>
        ///// <param name="ex">The ex.</param>
        public static void Error(Exception ex)
        {
            ILog log = null;

            int a = ex.StackTrace.IndexOf(".cs");

            if (a > -1)
            {
                int b = ex.StackTrace.Substring(0, a).LastIndexOf("\r\n");

                var t = "";

                if (b > -1)
                {
                    t = ex.StackTrace.Substring(b, a - b);

                    int truck = t.IndexOf("位置");
                    a = t.IndexOf("在");

                    var msg = t.Substring(a + 1, truck - a - 1);

                    log = log4net.LogManager.GetLogger(site + msg);
                }
                else
                {
                    log = log4net.LogManager.GetLogger("");
                }
            }
            else
            {
                log = log4net.LogManager.GetLogger("");
            }

            log.Error(ex);
        }

        public static void Error(string title, Exception ex)
        {
            ILog log = null;
            if (!string.IsNullOrEmpty(title))
            {
                log = log4net.LogManager.GetLogger(site + title);
            }
            else
            {
                int a = ex.StackTrace.IndexOf(".cs");

                if (a > -1)
                {
                    int b = ex.StackTrace.Substring(0, a).LastIndexOf("\r\n");

                    var t = "";

                    if (b > -1)
                    {
                        t = ex.StackTrace.Substring(b, a - b);

                        int truck = t.IndexOf("位置");
                        a = t.IndexOf("在");

                        var msg = t.Substring(a + 1, truck - a - 1);

                        log = log4net.LogManager.GetLogger(site + msg);
                    }
                    else
                    {
                        log = log4net.LogManager.GetLogger(site);
                    }
                }
                else
                {
                    log = log4net.LogManager.GetLogger(site);
                }
            }
            log.Error(ex);
        }

        public static void ErrorMsg(string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                //int a = msg.IndexOf(".cs");
                //int b = msg.Substring(0, a).LastIndexOf("\r\n");
                //var t = msg.Substring(b, a - b);
                //int truck = t.IndexOf("位置");
                //a = t.IndexOf("在");
                //var msglogger = t.Substring(a + 1, truck - a - 1);
                ILog log = log4net.LogManager.GetLogger(site);
                log.Error(msg);
            }

        }
        public static void Info(string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                //int a = msg.IndexOf(".cs");
                //int b = msg.Substring(0, a).LastIndexOf("\r\n");
                //var t = msg.Substring(b, a - b);
                //int truck = t.IndexOf("位置");
                //a = t.IndexOf("在");
                //var msglogger = t.Substring(a + 1, truck - a - 1);
                ILog log = log4net.LogManager.GetLogger(site);
                log.Info(msg);
            }

        }

        public static void Error(string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                //int a = msg.IndexOf(".cs");
                //int b = msg.Substring(0, a).LastIndexOf("\r\n");
                //var t = msg.Substring(b, a - b);
                //int truck = t.IndexOf("位置");
                //a = t.IndexOf("在");
                //var msglogger = t.Substring(a + 1, truck - a - 1);
                ILog log = log4net.LogManager.GetLogger(site);
                log.Error(msg);
            }
        }
        public static void Error(string title, string content)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                //int a = msg.IndexOf(".cs");
                //int b = msg.Substring(0, a).LastIndexOf("\r\n");
                //var t = msg.Substring(b, a - b);
                //int truck = t.IndexOf("位置");
                //a = t.IndexOf("在");
                //var msglogger = t.Substring(a + 1, truck - a - 1);
                ILog log = log4net.LogManager.GetLogger(site + title);
                log.Error(content);
            }
            else
            {
                ILog log = log4net.LogManager.GetLogger(content);
                log.Error(content);
            }
        }

        ///// <summary>
        ///// Debugs the specified MSG.
        ///// </summary>
        ///// <param name="msg">The MSG.</param>
        //public static void Debug(object msg)
        //{
        //    log.Debug(msg);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="msg"></param>
        //public static void Info(object msg)
        //{
        //    log.Info(msg);
        //}




        /// <summary>
        ///     写入日志
        ///     2015年1月20日15:06:46
        /// </summary>
        /// <param name="t">当前类</param>
        /// <param name="functionName">方法名</param>
        /// <param name="author">作者</param>
        /// <param name="dic">{"参数名称",参数值}</param>
        /// <param name="ex">错误信息</param>
        public static void WriteLog(Type t, string functionName, string author, Dictionary<string, object> dic,
            Exception ex)
        {
            //类型：Int32参数名称：ssid参数值：3
            var sb = new StringBuilder();
            sb.Append("");
            if (dic != null)
            {
                foreach (var item in dic)
                {
                    sb.AppendFormat("类型：{0} 参数名称：{1} 参数值：{2}", item.Value.GetType(), item.Key,
                        (item.Value == null ? "" : item.Value));
                    sb.Append("\r\n");
                }
            }
            var log = LogManager.GetLogger(t);
            var strInfo = "Error:方法名：" + functionName + "--作者：" + author + "\r\n传入参数：\r\n" + sb + "\r\n错误信息：" +
                          ex.Message + "\r\n" + ex.StackTrace + "\r\n";
            log.Error(strInfo);
            ////开发中暂时注释 edit by liuzq 2015-12-18
            //EmailDelegate dn = new EmailDelegate(new SendEmail().Send);
            //IAsyncResult ias = dn.BeginInvoke("屌丝你程序出错了", strInfo, author.Trim(), null, dn);
            //dn.EndInvoke(ias);
        }


        /// <summary>
        ///     记录普通信息
        /// </summary>
        /// <param name="infomation">信息</param>
        public static void WriteInfo(Type t, string infomation)
        {
            var log = LogManager.GetLogger(t);
            log.Info(infomation);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void ErrorFormat(string format, params object[] args)
        {
            var log = LogManager.GetLogger("Error");
            if (log.IsErrorEnabled)
            {
                log.ErrorFormat(format, args);
            }
        }

    }
}
