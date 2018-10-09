using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using SFO2O.M.Controllers.Common;
using SFO2O.Utility.Extensions;
using SFO2O.Utility.Uitl;

namespace SFO2O.M.Controllers
{
    public class BaseController : Controller
    {

        /// <summary>
        /// 获取当前是否是异步请求。
        /// </summary>
        public bool IsAsync
        {
            get
            {
                return this.ViewBag.IsAsync;
            }
            protected set
            {
                this.ViewBag.IsAsync = value;
            }
        }
        /// <summary>
        /// 获取页面回调方法(仅对异步请求有效)。
        /// </summary>
        public string Callback
        {
            get
            {
                return this.ViewBag.Callback;
            }
            protected set
            {
                this.ViewBag.Callback = value;
            }
        }
        /// <summary>
        /// 获取图片基路径。
        /// </summary>
        public string ImageServer
        {
            get
            {
                return this.ViewBag.ImageServer;
            }
            protected set
            {
                this.ViewBag.ImageServer = value;
            }
        }
        /// <summary>
        /// 获取当前是否为子Action。
        /// </summary>
        public bool IsChildAction
        {
            get
            {
                return this.ViewBag.IsChildAction;
            }
            protected set
            {
                this.ViewBag.IsChildAction = value;
            }
        }

        public string Source
        {
            get;
            set;
        }

        public string StationSource
        {
            get;
            set;
        }

        public int ChannelId
        {
            get;
            set;
        }

        /// <summary>
        /// 获取当前请求的引用页。
        /// </summary>
        protected string UrlReferrer
        {
            get { return this.Request.UrlReferrer != null ? this.Request.UrlReferrer.ToString() : string.Empty; }
        }

        #region 重写的方法
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (!string.IsNullOrEmpty(Request.QueryString["Async"]) && Request.QueryString["Async"].ToUpper() == "TRUE" || this.Request.IsAjaxRequest())
            {
                this.IsAsync = true;
                this.Callback = Request.QueryString["Callback"];
            }
            else
            {
                this.IsAsync = false;
            }

            if (this.ControllerContext.IsChildAction)
            {
                this.IsChildAction = true;
            }
            else
            {
                this.IsChildAction = false;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["wi"]))
            {
                this.Source = Request.QueryString["wi"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["StationSource"]))
            {
                Session["StationSource"] = Request.QueryString["StationSource"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["ChannelId"]))
            {
                Session["ChannelId"] = Request.QueryString["ChannelId"];
            }
            
            this.ImageServer = ConfigHelper.GetAppSetting("ImageServer", "");

        }
        #endregion


        #region 错误处理
        protected internal ActionResult HandleSuccess(string message, object data = null, string linkUrl = null)
        {
            Message model = new Message("成功", message, data) { LinkUrl = linkUrl };

            if (!string.IsNullOrWhiteSpace(this.Callback))
            {
                string jsonText = JsonHelper.ToJson(model);

                return this.JavaScript(this.Callback + "(" + jsonText + ")");
            }

            if (this.IsAsync)
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.View("Error", model);
            }
        }

        /// <summary>
        /// 处理错误。
        /// </summary>
        /// <param name="message">错误消息。</param>
        /// <param name="linkUrl">可以供用户点击下一步的链接地址。</param>
        /// <returns></returns>
        protected internal ActionResult HandleError(string message, string linkUrl = null, object data = null)
        {
            return this.HandleError(MessageType.Error, message, linkUrl, data);
        }

        /// <summary>
        /// 处理错误。
        /// </summary>
        /// <param name="type">错误类型。</param>
        /// <param name="message">错误消息。</param>
        /// <param name="linkUrl">可以供用户点击下一步的链接地址。</param>
        /// <returns></returns>
        protected internal virtual ActionResult HandleError(MessageType type, string message, string linkUrl = null, object data = null)
        {
            //#if DEBUG   
            //            LogHelper.Info(message);
            //#endif

            Message model = new Message(type, "出错了", message, data) { LinkUrl = linkUrl };

            if (!string.IsNullOrWhiteSpace(this.Callback))
            {
                string jsonText = JsonHelper.ToJson(model);

                return this.JavaScript(this.Callback + "(" + jsonText + ")");
            }

            if (this.IsAsync)
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.View("Error", model);
            }
        } 
        /// <summary>
        /// 处理错误。
        /// </summary>
        /// <param name="exception">异常信息。</param>
        /// <param name="linkUrl">可以供用户点击下一步的链接地址。</param>
        /// <returns></returns>
        protected internal virtual ActionResult HandleError(Exception exception, string linkUrl = null)
        {
            //#if DEBUG
            //throw exception;
            //#else
            //if (this.IsDebug && (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["SecretKeyForDebug"]) || ConfigurationManager.AppSettings["SecretKeyForDebug"].ToUpper() == Request.QueryString["Key"].ToUpper()))
            //{
            //    throw exception;
            //}

            // 日志
            LogHelper.Error(exception);

            // 错误消息
            Message model = new Message(MessageType.Error, "出错了", "我们的系统出现了一点小问题，暂时无法处理您的请求，对您带来的不便深表歉意。") { LinkUrl = linkUrl };

            if (!string.IsNullOrWhiteSpace(this.Callback))
            {
                string jsonText = model.SerializeByJsonSerializer(this.Response.ContentEncoding);

                return this.JavaScript(this.Callback + "(" + jsonText + ")");
            }

            if (this.IsAsync)
            {
                // return this.Content(model.SerializeByJsonSerializer(this.Response.ContentEncoding), "application/json");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //return this.View("Error", model);
                return this.RedirectToAction("Index", "Error");
            }
            //#endif
        }
        #endregion
    }
}
