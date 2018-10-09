using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WxPayAPI;
using SFO2O.Model.Order;
using SFO2O.BLL.Order;
using SFO2O.Utility;
using SFO2O.Utility.Uitl;
using SFO2O.BLL.Pay;
namespace SFO2O.M.Controllers
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    public class ResultNotifyController:NotifyController
    {
        private readonly PayBll PayBll = new PayBll();

        public ResultNotifyController(HttpContextBase context):base(context)
        {
        }
        private readonly BuyOrderManager buyOrderManager = new BuyOrderManager();
        public void ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();
            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //LogHelper.Error("transaction_id 有值" + notifyData.IsSet("transaction_id"));
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                //LogHelper.Error("res.ToXml" + res.ToXml());
                context.Response.Write(res.ToXml());
                context.Response.End();
            }
            string transaction_id = notifyData.GetValue("transaction_id").ToString();
            //LogHelper.Error("transaction_id 的获取值为" + transaction_id);


            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                //Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                context.Response.Write(res.ToXml());
                context.Response.End();
            }
            //查询订单成功
            else
            {
                // 支付回写
                PayBll.PayReturn(notifyData.GetValue("out_trade_no").ToString(), transaction_id
                    , notifyData.ToUrl(), base.DeliveryRegion, base.language,4);

                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                //Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
                context.Response.Write(res.ToXml());
                context.Response.End();
            }
        }

        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}