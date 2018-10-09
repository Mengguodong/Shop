using System;
using System.Collections.Generic;
using SFO2O.Utility.Uitl;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WxPayAPI;
using SFO2O.BLL.Order;
using SFO2O.Model.Order;
using LitJson;
namespace SFO2O.M.Controllers
{
    public class WxPayController : SFO2OBaseController
    {
        private readonly BuyOrderManager buyOrderManager = new BuyOrderManager();
        /// <summary>
        /// 调用js获取收货地址时需要传入的参数
        /// 格式：json串
        /// 包含以下字段：
        ///     appid：公众号id
        ///     scope: 填写“jsapi_address”，获得编辑地址权限
        ///     signType:签名方式，目前仅支持SHA1
        ///     addrSign: 签名，由appid、url、timestamp、noncestr、accesstoken参与签名
        ///     timeStamp：时间戳
        ///     nonceStr: 随机字符串
        /// </summary>
        public string wxJsApiParam { get; set; } //H5调起JS API参数
        public string Page_Load(string id, HttpContextBase HttpContext, bool isfalse)
        {
            //LogHelper.Info("page load");
            // System           
            JsApiPay jsApiPay = new JsApiPay(HttpContext);
            try
            {
                //LogHelper.Error("开始获取1 orderId" + id);
                //LogHelper.Error("userId" + this.LoginUser.UserID);
                var entity = buyOrderManager.GetOrderInfoByCode(id, this.LoginUser.UserID);
                //LogHelper.Error("开始获取2");
                string rmb_fee = Convert.ToInt32((entity.TotalAmount - (entity.Huoli / 100) - entity.Coupon) * 100).ToString();
                //LogHelper.Error("rmb_fee= " + rmb_fee);
                //调用【网页授权获取用户信息】接口获取用户的openid和access_token
                //LogHelper.Error("开始获取3 id");
                //LogHelper.Error("Session的openid");
                if (isfalse)
                {
                    //LogHelper.Error("jsApiPay获取openid");
                    jsApiPay.GetOpenidAndAccessToken(id, HttpContext);
                }
                //支付开始
                if (HttpContext.Session["openid"] != null)
                {
                    jsApiPay.openid = HttpContext.Session["openid"].ToString();
                    LogHelper.Error("id=" + id + "rmb_fee=" + rmb_fee);
                    WxPayData wxPay = jsApiPay.GetUnifiedOrderResult(id, rmb_fee);
                }
                wxJsApiParam = jsApiPay.GetJsApiParameters();
                return wxJsApiParam;
            }
            catch (Exception ex)
            {
                Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面加载出错，请重试" + "</span>");
            }
            return null;
        }
    }
}