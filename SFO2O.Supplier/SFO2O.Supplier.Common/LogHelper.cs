using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Common
{
    public static class LogHelper
    {
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

                    log = log4net.LogManager.GetLogger(msg);
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
                ILog log = log4net.LogManager.GetLogger(msg);
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
                ILog log = log4net.LogManager.GetLogger(msg);
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
                ILog log = log4net.LogManager.GetLogger(msg);
                log.Error(msg);
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
    }  
}
