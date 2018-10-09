using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SFO2O.BLL.Common;
using SFO2O.BLL.Exceptions;
using SFO2O.M.Controllers.Common;
using SFO2O.M.ViewModel.Account;
using SFO2O.Utility.Uitl;

namespace SFO2O.M.Controllers
{
    public class SFO2OBaseController : BaseController
    {
        public int _deliveryRegion = 1;
        public int language = 1;
         
        /// <summary>
        /// 配送区域
        /// </summary>
        public int DeliveryRegion
        {
            get
            {
                //应该根据页面设置从cookie，db或者redis里获取
                return _deliveryRegion;

            }

        }

        private decimal _exchangeRate = ConstClass.ErrorContractDecimal;
        /// <summary>
        /// 汇率
        /// </summary>
        public decimal ExchangeRate
        {
            get
            {
                if (_exchangeRate == ConstClass.ErrorContractDecimal)
                {
                    _exchangeRate = CommonBll.GetExchangeRates(DeliveryRegion);
                }
                return _exchangeRate;
            }
        }

        private LoginUserModel _loginUser = null;
        /// <summary>
        /// 获取session或者redis中的缓存对象
        /// </summary>
        public LoginUserModel LoginUser
        {
            get
            {
                if (_loginUser == null)
                {
                    LoginUserModel tempLoginModel = null;

                    var tempIsLogin = LoginHelper.CheckSession(out tempLoginModel);

                    if (tempIsLogin && tempLoginModel != null)
                    {
                        _loginUser = tempLoginModel;

                        LoginHelper.RefreshSession(_loginUser);//提前五分钟刷新session到期时间
                    }
                    ViewBag.LoginUser = _loginUser;
                }
                return _loginUser;
            }
        }

        /// <summary>
        /// 处理跳转到登录页(同步js在登录页判断returnurl，异步跳转由js控制如需要返回增加returnurl)
        /// </summary>
        /// <returns></returns>
        protected internal ActionResult HandleToLogin()
        {
            var returnUrl = HttpContext.Request.RawUrl;
            if (!this.IsAsync)
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return this.Redirect("/account/login");
                }
                else
                {
                    return this.Redirect("/account/login?returnurl=" + returnUrl);
                }
            }
            else
            {
                Message model = new Message(MessageType.RequireAuthorize, "请登录", "请登录");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 处理错误
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="linkUrl"></param>
        /// <returns></returns>
        protected internal override ActionResult HandleError(MessageType type, string message, string linkUrl = null, object data = null)
        {
            if (type != MessageType.RequireAuthorize)
            {
                return base.HandleError(type, message, linkUrl, data);
            }
            //TODO：统一登录处理
            if (!this.IsAsync)
            {
                return this.Redirect(linkUrl);
            }
            else
            {
                Message model = new Message(MessageType.RequireAuthorize, "请登录", message)
                {
                    LinkUrl = linkUrl
                };

                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 处理错误。
        /// </summary>
        /// <param name="exception">异常信息。</param>
        /// <param name="linkUrl">可以供用户点击下一步的链接地址。</param>
        /// <returns></returns>
        protected internal override ActionResult HandleError(Exception exception, string linkUrl = null)
        {
            SFO2OException sfo2oException = exception as SFO2OException;

            if (sfo2oException == null)
            {
                return base.HandleError(exception, linkUrl);
            }

            // 日志
            if (sfo2oException.InnerException != null)
            {
                LogHelper.Error(sfo2oException.Message, sfo2oException.InnerException);
            }
            else
            {
                LogHelper.Error(sfo2oException.Message);
            }

            // 错误消息
            Message model = new Message("出错了", sfo2oException.Message) { LinkUrl = linkUrl };

            if (!string.IsNullOrWhiteSpace(this.Callback))
            {
                string jsonText = JsonHelper.ToJson(model); //model.SerializeByJsonSerializer(this.Response.ContentEncoding);

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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //ViewBag.LoginUser = LoginUser;
        }

        public ActionResult Error()
        {
            return View(new  Message("出错了",""));
        }
        public ActionResult NotFound()
        {
            return View(new Message("出错了", ""));
        }
        
        public decimal OrderLimitValue
        {
            get { return ConfigHelper.GetAppSetting<decimal>("OrderLimitValue"); }
        }

        public decimal ConsolidatedPrice
        {
            get { return ConfigHelper.GetAppSetting<decimal>("consolidatedPrice"); }
        }
    }
}
