using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using SFO2O.M.Controllers.Filters;
using SFO2O.BLL.Shopping;
//using SFO2O.BLL.Product;
using SFO2O.BLL.Exceptions;
using SFO2O.Utility.Uitl;
using SFO2O.M.Controllers.Extensions;
using SFO2O.M.ViewModel.Order;
using SFO2O.Utility.Extensions;
using SFO2O.BLL.Order;
using SFO2O.Payment.BLL;
using SFO2O.Model.Pay;
using SFO2O.Model.Order;
using SFO2O.Utility;
using System.Collections.Specialized;

using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Net;
using SFO2O.BLL.Team;
using SFO2O.Model.Team;
using SFO2O.BLL.My;
using SFO2O.Model.Information;
using SFO2O.Model.Product;
using SFO2O.BLL.Information;
using SFO2O.BLL.Common;
using WxPayAPI;
using SFO2O.BLL.Pay;
using SFO2O.BLL.GreenBll;
using SFO2O.Model;
using SFO2O.Model.Account;
using SFO2O.BLL.Account;

//易宝

namespace SFO2O.M.Controllers
{
    public class PayController : SFO2OBaseController
    {
        private readonly BuyOrderManager buyOrderManager = new BuyOrderManager();
        private readonly AddressBll addressBll = new AddressBll();
        private readonly OrderManager orderManager = new OrderManager();
        private readonly TeamBll teamBll = new TeamBll();
        private readonly MyBll Bll = new MyBll();
        private static readonly InformationBll InformationBll = new InformationBll();
        private readonly PayBll payBll = new PayBll();
        private readonly AccountBll accountBll = new AccountBll();
        /// <summary>
        /// 获取支付二维码
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult TwoDimensionCode(int PayWay, string orderId)
        {
            LogHelper.Info(PayWay + "===TwoDimensionCode开始=====" + orderId);
            try
            {
                Session["payorderCode"] = orderId;

                bool isTrue = false;
                string msg = "";
                string imageUrl = "";
                if (string.IsNullOrWhiteSpace(orderId))
                {
                    return Redirect("/Home/Error");
                }
                else
                {
                    var orderModel = buyOrderManager.GetOrderInfoByCode(orderId, this.LoginUser.UserID);

                    orderModel.PayType = PayWay;


                    //调用获取二维码
                    var codeResult = payBll.WXPayRequest(orderModel);


                    if (codeResult.Code == "0000")// "0000" ：成功
                    {
                        imageUrl = codeResult.Message;
                        isTrue = true;

                        #region    创建支付订单信息

                        OrderPaymentEntity orderPaymentEntity = new OrderPaymentEntity();
                        orderPaymentEntity.PayCode = codeResult.tradeid;
                        orderPaymentEntity.TradeCode = "";
                        orderPaymentEntity.UserId = this.LoginUser.UserID;
                        orderPaymentEntity.OrderType = 1;
                        //订单号
                        orderPaymentEntity.OrderCode = orderModel.OrderCode;
                        //待支付金额
                        orderPaymentEntity.PayAmount = orderModel.TotalAmount;
                        //已支付金额
                        orderPaymentEntity.PaidAmount = 0;
                        //3为：支付宝   4微信
                        if (PayWay == 2)
                        {
                            orderPaymentEntity.PayPlatform = 3;
                        }
                        else if (PayWay == 1)
                        {
                            orderPaymentEntity.PayPlatform = 4;
                        }

                        orderPaymentEntity.PayType = 1;
                        orderPaymentEntity.PayStatus = 1;
                        orderPaymentEntity.PayTerminal = 1;
                        orderPaymentEntity.PayCompleteTime = null;
                        orderPaymentEntity.PayBackRemark = "";
                        orderPaymentEntity.Remark = "";
                        orderPaymentEntity.CreateTime = DateTime.Now;
                        orderPaymentEntity.CreateBy = this.LoginUser.UserName;

                        if (!buyOrderManager.AddOrderPayment(orderPaymentEntity))
                        {
                            LogHelper.Error(string.Format("订单申请支付信息插入OrderPayment失败，订单号{0}，支付流水号{1},申请支付金额{2}", orderModel.OrderCode, orderModel.TotalAmount));
                        }

                        #endregion

                    }
                    else
                    {
                        LogHelper.WriteInfo(typeof(PayController), string.Format("OrderCreate===二维码接口，对象：{0}", JsonHelper.ToJson(codeResult)));
                        msg = "生成二维码失败!";
                    }
                    //将实体返回到前台
                    return Json(new { result = isTrue, Msg = msg, imageUrl = imageUrl });



                }


            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(PayController), "TwoDimensionCode","mgd",new Dictionary<string,object>(),ex);
                return Json(new { result = false, Msg = "异常", imageUrl = "!!" });
            }
        }


        /// <summary>
        /// 聚合富扫码支付回调接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PayCallBackRetunResult(JuHeFuResponseModel model)
        {
            LogHelper.WriteInfo(typeof(PayController), JsonConvertTool.SerializeObject(model));
            try
            {
                //JuHeFuResponseModel model = JsonConvertTool.DeserializeObject<JuHeFuResponseModel>(data);
                bool isOk = payBll.PayCallBackRetunResult(model);
                if (isOk)
                {
                    //成功
                    //return RedirectToAction("ZFBReturnPage");
                }
                else
                {
                    //失败
                    //return RedirectToAction("ZFBReturnPage");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteInfo(typeof(PayController), string.Format("PayCallBackRetunResult,返回实体：{0}，错误：{1}", JsonHelper.ToJson(model), ex.Message));

            }
            return RedirectToAction("ZFBReturnPage");
        }

        /// <summary>
        /// 支付界面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        [Login]
        public ActionResult Index(string id, string package,int isScore=1)
        {
            PayOrderModel model = new PayOrderModel();
            if (string.IsNullOrWhiteSpace(id))
            {
                return Redirect("/Home/Error");
            }
            else
            {
                var orderModel = buyOrderManager.GetOrderInfoByCode(id, this.LoginUser.UserID);
                //判断最后一单是否是支付宝
                OrderInfoEntity orderInfo = buyOrderManager.isAliPay(this.LoginUser.UserID);
                model.isAliPay = false;
                if (orderInfo != null && orderInfo.PayPlatform == 3)
                {
                    model.isAliPay = true;
                }
                if (orderModel != null && orderModel.OrderStatus == 0)
                {
                    model.OrderId = orderModel.OrderCode;
                    model.Totalfee = orderModel.TotalAmount;
                    if ("1".Equals(package))
                    {
                        model.Package = "包裹一";
                        Session["Package"] = 2;
                    }
                    else if ("2".Equals(package))
                    {
                        model.Package = "包裹二";
                        Session["Package"] = 1;
                    }
                    else
                    {
                        model.Package = "";
                        Session["Package"] = "";
                    }

                }
                else
                {
                    return Redirect("/my/detail?orderCode=" + id);

                }
            }
            ViewBag.IsScore = isScore;
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Login]
        public JsonResult Submit(string id)
        {
            Session["payorderCode"] = id;
            string type = "0", message = "支付失败。", url = "";
            if (!string.IsNullOrWhiteSpace(id))
            {
                var entity = buyOrderManager.GetOrderInfoByCode(id, this.LoginUser.UserID);
                if (entity != null)
                {
                    if (buyOrderManager.PayOrderValidation(id, this.LoginUser.UserID))
                    {
                        type = "2";
                        message = "商品税发生变化，需重新确认订单。";
                        url = "";
                        LogHelper.Error(string.Format("订单商品税发生变化，订单号{0}", entity.OrderCode));
                    }
                    else
                    {
                        try
                        {
                            IPay pay = new EPayDollar();
                            PayValue payValue = new PayValue();
                            PayOrderModel payModel = new PayOrderModel();
                            string paycode = DateTime.Now.ToString("yyMMddHHmmsss") + new Random().Next(100, 999);

                            payModel.Note = "订单支付";
                            payModel.PayId = "quickpay";
                            payModel.Createip = "";
                            payModel.OrderId = paycode;
                            payModel.PayCurrencys = PayCurrency.RMB;
                            payModel.Storeoitype = 1;
                            payModel.Totalfee = entity.TotalAmount;
                            payModel.OrderFreight = entity.Freight;
                            payModel.OrderGoodsAmount = entity.ProductTotalAmount;
                            payModel.OrderTaxAmount = entity.CustomsDuties;
                            payModel.Userid = this.LoginUser.UserID.ToString();
                            payModel.Receiver = entity.Receiver;
                            payModel.PassPortType = entity.PassPortType;
                            payModel.PassPortNum = entity.PassPortNum;
                            payModel.Phone = entity.Phone;
                            payValue.ReturnObject = payModel;

                            var result = pay.OrderPay(payValue);
                            if (!result.HasError)
                            {
                                LogHelper.Error(string.Format("订单申请支付信息，订单号{0}，支付流水号{1},申请支付金额{2}", entity.OrderCode, paycode, entity.TotalAmount));

                                OrderPaymentEntity orderPaymentEntity = new OrderPaymentEntity();
                                orderPaymentEntity.PayCode = paycode;
                                orderPaymentEntity.TradeCode = "";
                                orderPaymentEntity.UserId = this.LoginUser.UserID;
                                orderPaymentEntity.OrderType = 1;
                                orderPaymentEntity.OrderCode = entity.OrderCode;
                                orderPaymentEntity.PayAmount = entity.TotalAmount;
                                orderPaymentEntity.PaidAmount = 0;
                                orderPaymentEntity.PayPlatform = 1;
                                orderPaymentEntity.PayType = 1;
                                orderPaymentEntity.PayStatus = 1;
                                orderPaymentEntity.PayTerminal = 1;
                                orderPaymentEntity.PayCompleteTime = null;
                                orderPaymentEntity.PayBackRemark = "";
                                orderPaymentEntity.Remark = "";
                                orderPaymentEntity.CreateTime = DateTime.Now;
                                orderPaymentEntity.CreateBy = this.LoginUser.UserName;

                                if (!buyOrderManager.AddOrderPayment(orderPaymentEntity))
                                {
                                    LogHelper.Error(string.Format("订单申请支付信息插入OrderPayment失败，订单号{0}，支付流水号{1},申请支付金额{2}", entity.OrderCode, paycode, entity.TotalAmount));
                                }
                                type = "1";
                                message = "";
                                url = result.ReturnString;
                                LogHelper.Info(url + "&订单号：" + entity.OrderCode);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(string.Format("订单申请支付异常，订单号{0},错误信息{1}", entity.OrderCode, ex.Message));
                            type = "0";
                            message = "支付异常失败。";
                            url = "";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(url) && url.Length > 10)
            {
                url = System.Web.HttpUtility.UrlEncode(url, Encoding.UTF8);
            }
            return Json(new { Type = type, Content = message, Url = url }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 支付宝支付
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Login]
        public JsonResult ZSubmit(string id, int payType = 1)
        {
            Session["payorderCode"] = id;

            string type = "0", message = "支付失败。", url = "www.discountmassworld.com";
            if (!string.IsNullOrWhiteSpace(id))
            {
                var entity = buyOrderManager.GetOrderInfoByCode(id, this.LoginUser.UserID);
                //判断订单是否失效,如果失效就返回订单状态orderStatus=5
                if (entity != null)
                {
                    if (buyOrderManager.PayOrderValidation(id, this.LoginUser.UserID))
                    {
                        type = "2";
                        message = "商品税发生变化，需重新确认订单。";
                        url = "";
                        //LogHelper.Error(string.Format("订单商品税发生变化，订单号{0}", entity.OrderCode));
                    }
                    else
                    {
                        try
                        {
                            //发给支付宝的支付号
                            string paycode = DateTime.Now.ToString("yyMMddHHmmsss") + new Random().Next(100, 999);
                            string rmb_fee = (entity.TotalAmount - (entity.Huoli / 100) - entity.Coupon).ToString();
                            //支付宝支付
                            if (payType == 1)
                            {
                                string gateway = System.Web.Configuration.WebConfigurationManager.AppSettings["ZFBGateWayUrl"].ToString();	//'支付接口
                                string service = "create_forex_trade_wap";
                                string partner = System.Web.Configuration.WebConfigurationManager.AppSettings["ZFBPartner"].ToString();		//partner		合作伙伴ID			保留字段
                                string sign_type = System.Web.Configuration.WebConfigurationManager.AppSettings["ZFbSignType"].ToString();
                                string subject = id;	//subject		商品名称
                                string body = System.Web.Configuration.WebConfigurationManager.AppSettings["supplier"].ToString(); ;//body			商品描述    
                                string merchant_url = System.Web.Configuration.WebConfigurationManager.AppSettings["merchant_url"].ToString();
                                string currency = System.Web.Configuration.WebConfigurationManager.AppSettings["ZFBCurrency"].ToString();
                                string key = System.Web.Configuration.WebConfigurationManager.AppSettings["ZFBKey"].ToString();              //partner账户的支付宝安全校验码
                                string return_url = System.Web.Configuration.WebConfigurationManager.AppSettings["ZFBReturnUrl"].ToString(); //服务器通知返回接口
                                string notify_url = System.Web.Configuration.WebConfigurationManager.AppSettings["ZFBNotifyUrl"].ToString(); //服务器通知返回接口
                                AliPay ap = new AliPay();
                                url = ap.CreatUrl(
                                    merchant_url,
                                    gateway,
                                    service,
                                    partner,
                                    sign_type,
                                    entity.OrderCode,
                                    subject,
                                    body,
                                    currency,
                                    rmb_fee,
                                    key,
                                    return_url,
                                    notify_url
                                    );
                            }
                            //微信支付
                            if (true)
                            {
                                //LogHelper.Info(string.Format("订单申请支付信息，订单号{0},申请支付金额{1}", entity.OrderCode, entity.TotalAmount));
                                OrderPaymentEntity orderPaymentEntity = new OrderPaymentEntity();
                                orderPaymentEntity.PayCode = paycode;
                                orderPaymentEntity.TradeCode = "";
                                orderPaymentEntity.UserId = this.LoginUser.UserID;
                                orderPaymentEntity.OrderType = 1;
                                //订单号
                                orderPaymentEntity.OrderCode = entity.OrderCode;
                                //待支付金额
                                orderPaymentEntity.PayAmount = entity.TotalAmount - (entity.Huoli / 100) - entity.Coupon;
                                //已支付金额
                                orderPaymentEntity.PaidAmount = 0;
                                //3为：支付宝
                                if (payType == 1)
                                {
                                    orderPaymentEntity.PayPlatform = 3;
                                }
                                else
                                {
                                    orderPaymentEntity.PayPlatform = 4;
                                }
                                orderPaymentEntity.PayType = 1;
                                orderPaymentEntity.PayStatus = 1;
                                orderPaymentEntity.PayTerminal = 1;
                                orderPaymentEntity.PayCompleteTime = null;
                                orderPaymentEntity.PayBackRemark = "";
                                orderPaymentEntity.Remark = "";
                                orderPaymentEntity.CreateTime = DateTime.Now;
                                orderPaymentEntity.CreateBy = this.LoginUser.UserName;

                                if (!buyOrderManager.AddOrderPayment(orderPaymentEntity))
                                {
                                    LogHelper.Error(string.Format("订单申请支付信息插入OrderPayment失败，订单号{0}，支付流水号{1},申请支付金额{2}", entity.OrderCode, entity.TotalAmount));
                                }
                                type = "1";
                                message = "";
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(string.Format("订单申请支付异常，订单号{0},错误信息{1}", entity.OrderCode, ex.Message));
                            type = "0";
                            message = "支付异常失败。";
                            url = "";
                        }
                    }
                    LogHelper.Error("-----订单号:" + entity.OrderCode + " ,URL：" + url + "------");
                }
                else
                {
                    //LogHelper.Error("订单失效");
                    return Json(new { Type = 1, orderStatus = 5 }, JsonRequestBehavior.AllowGet);
                }
            }
            if (!string.IsNullOrEmpty(url) && url.Length > 10)
            {
                //LogHelper.Error("url 转码");
                url = System.Web.HttpUtility.UrlEncode(url, Encoding.UTF8);
            }
            //LogHelper.Error("响应成功  参数=" + type);
            return Json(new { Type = type, Content = message, Url = url }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReturnPage()
        {
            LogHelper.Info("支付同步还回" + Request.Url.ToString());
            ViewBag.Falg = PayResult();
            ViewBag.OrderCode = Session["payorderCode"].ToString();
            // Session["payorderCode"] = null;
            return View();
        }

        public JsonResult NotifyPage()
        {
            LogHelper.Info("支付异步还回" + Request.Url.ToString());
            // Session["payorderCode"] = null;
            PayResult();
            return Json(new { Type = "ok" }, JsonRequestBehavior.AllowGet);
        }
        ///支付宝支付结果 同步
        public ActionResult ZFBReturnPage()
        {

            ViewBag.Falg = true;
            //ViewBag.OrderCode = out_trade_no;
            string out_trade_no = string.Empty;
            //LogHelper.Info("------------ZFBReturnPage---1--Session['payorderCode']:" + out_trade_no);
            if (Session["payorderCode"] != null && Session["payorderCode"].ToString() != "")
            {
                out_trade_no = Session["payorderCode"].ToString();
                //LogHelper.Info("------------ZFBReturnPage---2--Session['payorderCode'].ToString():" + out_trade_no);
            }
            else
            {
                //LogHelper.Info("------------ZFBReturnPage---3--获取不到session值，跳转到我的账户页面。");
                return RedirectToAction("Index", "My");
            }
            // 用户没有登录
            if (base.LoginUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            /// 声明订单号变量
            string OrderCode = "";
            OrderCode = out_trade_no;
            ViewBag.OrderCode = out_trade_no;

            /// 根据支付宝响应信息的OrderCode获得订单信息
            //OrderInfoEntity orderInfoEntity = orderManager.GetOrderInfoByOrderCode(out_trade_no);
            OrderInfoEntity orderInfoEntity = orderManager.GetOrderInfoByOrderCode(OrderCode);
            //LogHelper.Info("------------ZFBReturnPage--4---orderInfoEntity:" + orderInfoEntity);

            if (orderInfoEntity == null)
            {
                return RedirectToAction("Index", "My");
            }

            //LogHelper.Info("------------ZFBReturnPage--5---orderInfoEntity.TeamCode:" + orderInfoEntity.TeamCode);
            /// 普通订单
            if (string.IsNullOrEmpty(orderInfoEntity.TeamCode))
            {
                //LogHelper.Info("------------ZFBReturnPage--6---进入普通订单逻辑------");
                NormalOrderPage(orderInfoEntity);
                return View();
            }
            /// 团订单
            else
            {
                //LogHelper.Info("------------ZFBReturnPage--7---进入团订单逻辑------");
                /// 团订单情况展示团详情页面逻辑
                //var teamDetailList = teamBll.TeamOrderPage(out_trade_no, this.DeliveryRegion);
                //LogHelper.Info("------------ZFBReturnPage--8---OrderCode:" + OrderCode);
                var teamDetailList = teamBll.TeamOrderPage(OrderCode, this.DeliveryRegion);
                //LogHelper.Info("------------ZFBReturnPage--9---teamDetailList:" + teamDetailList);

                if (teamDetailList == null)
                {
                    return RedirectToAction("Index", "My");
                }
                else
                {
                    /// 获得团成员UserId
                    //int TeamUserId = teamBll.GetTeamUserId(out_trade_no, orderInfoEntity.TeamCode);
                    int TeamUserId = teamBll.GetTeamUserId(OrderCode, orderInfoEntity.TeamCode);
                    //LogHelper.Info("------------ZFBReturnPage--10---TeamUserId:" + TeamUserId);

                    /// 登录用户ID
                    int LoginUserId = 0;

                    //LogHelper.Info("------------ZFBReturnPage--11---base.LoginUser:" + base.LoginUser);
                    // 登录用户
                    if (base.LoginUser != null)
                    {
                        /// 登录用户id
                        LoginUserId = base.LoginUser.UserID;
                        //LogHelper.Info("------------ZFBReturnPage--12---LoginUserId:" + LoginUserId);
                    }

                    /// 倒计时秒数
                    double sec = 0.0;

                    DateTime dtStartTime = DateTime.Now;
                    DateTime dtEndTime = Convert.ToDateTime(teamDetailList.First().EndTime);
                    sec = dtEndTime.Subtract(dtStartTime).TotalSeconds;
                    //LogHelper.Info("------------ZFBReturnPage--13---sec:" + sec);

                    /// 团活动过期
                    if (sec < 0)
                    {
                        ViewBag.TeamStatus = 2;
                    }
                    else
                    {
                        //LogHelper.Info("------------ZFBReturnPage--14---TeamStatus:" + teamDetailList.First().TeamStatus);
                        /// 组团成功
                        if (teamDetailList.First().TeamStatus == 3)
                        {
                            ViewBag.TeamStatus = 3;
                        }
                        /// 参团中
                        else if (teamDetailList.First().TeamStatus == 1)
                        {
                            ViewBag.restTime = sec;
                            ViewBag.TeamStatus = teamDetailList.First().TeamStatus;/// 组团状态
                        }
                        else
                        {
                            ViewBag.TeamStatus = teamDetailList.First().TeamStatus;/// 组团状态
                        }
                    }

                    /// 设置页面属性值
                    ViewBag.TeamUserId = TeamUserId;/// 团成员UserId
                    ViewBag.LoginUserId = LoginUserId;/// 登录用户id
                    ViewBag.TeamHead = teamDetailList.First().TeamHead;/// 团长
                    ViewBag.TeamNumbers = teamDetailList.First().TeamNumbers;/// 几人团
                    ViewBag.ListLength = teamDetailList.Count();/// 团详情集合长度
                    //ViewBag.TeamStatus = teamDetailList.First().TeamStatus;/// 组团状态
                    ViewBag.RestNumber = teamDetailList.First().TeamNumbers - teamDetailList.Count();/// 组团剩余人数
                    ViewBag.productPrice = (teamDetailList.First().DiscountPrice * base.ExchangeRate).ToNumberRoundStringWithPoint();/// 拼团价格
                    ViewBag.IsOrderCodeInput = 1;/// OrderCode有值

                    //return View("TeamDetail", teamDetailList);
                    //LogHelper.Info("------------ZFBReturnPage--15---重定向到TeamDetail的OrderCode:" + OrderCode);
                    return RedirectToAction("TeamDetail", "Team", new { OrderCode = OrderCode, Flag = 1 });
                }
            }
        }
        // //支付宝支付结果 异步
        public void ZFBNotifyPage()
        {
            //LogHelper.Info("支付异步还回" + Request.Form);
            AliPay();
            //判断返回结果 1 插入订单orderpayment成功 0 失败
            Session["payorderCode"] = Request.Form["out_trade_no"];

        }
        //易票联支付
        private bool PayResult()
        {
            bool flag = false;
            try
            {
                SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
                string LogInfo = string.Empty;
                foreach (var item in this.Request.QueryString)
                {
                    sArray.Add(item.ToString(), this.Request.QueryString[item.ToString()].ToString());
                    LogInfo += item.ToString() + "：" + this.Request.QueryString[item.ToString()].ToString() + "&";
                }
                LogHelper.Error(LogInfo);
                if (sArray.Count > 0)//判断是否有带返回参数
                {
                    IPay pay = new EPayDollar();
                    PayValue payValue = new PayValue();
                    payValue.ReturnObject = sArray;
                    payValue.ReturnString = Request.QueryString["sign"];
                    var result = pay.SignVerify(payValue);
                    if (!result.HasError)
                    {
                        string partner = Request.QueryString["partner"];
                        string out_trade_no = Request.QueryString["out_trade_no"];
                        string pay_no = Request.QueryString["pay_no"];
                        string amount = Request.QueryString["amount"];
                        string pay_result = Request.QueryString["pay_result"];
                        string pay_time = Request.QueryString["pay_time"];
                        string sett_date = Request.QueryString["sett_date"];
                        string sett_time = Request.QueryString["sett_time"];
                        string base64_memo = Request.QueryString["base64_memo"];
                        byte[] bytes = Convert.FromBase64String(base64_memo);
                        string memo = System.Text.Encoding.Default.GetString(bytes);
                        string sign_type = Request.QueryString["sign_type"];
                        string sign = Request.QueryString["sign"];
                        var entity = buyOrderManager.GetOrderPaymentByCode(out_trade_no);
                        if (entity.PayStatus != 1)
                        {
                            return false;
                        }
                        if (pay_result == "1")
                        {
                            OrderPaymentEntity orderPaymentEntity = new OrderPaymentEntity();
                            orderPaymentEntity.PayCode = out_trade_no;
                            orderPaymentEntity.TradeCode = pay_no;
                            orderPaymentEntity.PaidAmount = StringUtils.ToDecimal(amount);
                            orderPaymentEntity.PayStatus = 2;
                            orderPaymentEntity.PayCompleteTime = DateTime.Now;
                            orderPaymentEntity.PayBackRemark = Request.Url.ToString();

                            buyOrderManager.UpdatePaysuccess(orderPaymentEntity);

                            if (StringUtils.ToDecimal(amount) - entity.PayAmount == 0)
                            {
                                buyOrderManager.OrderPayOk(entity.OrderCode, orderPaymentEntity.PaidAmount, "");
                            }
                            else
                            {
                                buyOrderManager.OrderPayFailure(entity.OrderCode, "");
                                flag = false;
                            }
                            flag = true;
                        }
                        else
                        {
                            OrderPaymentEntity orderPaymentEntity = new OrderPaymentEntity();
                            orderPaymentEntity.PayCode = out_trade_no;
                            orderPaymentEntity.PayStatus = 3;
                            orderPaymentEntity.PayCompleteTime = DateTime.Now;
                            orderPaymentEntity.PayBackRemark = Request.Url.ToString();
                            buyOrderManager.UpdatePayFailure(orderPaymentEntity);
                            flag = false;
                        }
                    }
                    else
                    {
                        LogHelper.Error("签名验证失败");
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(string.Format("支付返回结果处理异常：返回信息{0}，异常信息{1}.", Request.Url.ToString(), ex.Message));
                flag = false;
            }
            return flag;
        }

        //支付宝 ZFBPayResult
        private bool AliPayResult()
        {
            bool flag = false;
            try
            {
                //通知类型
                string notify_type = Request.Form["notify_type"];
                //通知时间
                string notify_time = Request.Form["notify_time"];
                //通知校验id
                string notify_id = Request.Form["notify_id"];
                //签名方式
                string sign_type = Request.Form["sign_type"];
                //签名
                string sign = Request.Form["sign"];
                //通知注册时间
                string notify_reg_time = Request.Form["notify_reg_time"];
                //商户网站唯一订单号
                string out_trade_no = Request.Form["out_trade_no"];
                //支付宝交易号
                string trade_no = Request.Form["trade_no"];
                //交易状态
                string trade_status = Request.Form["trade_status"];
                //外币总额
                string total_fee = Request.Form["total_fee"];
                //外币币种
                string currency = Request.Form["currency"];

                // 支付回写
                flag = payBll.PayReturn(out_trade_no, trade_no, Request.Form.ToString(), base.DeliveryRegion, base.language, 3);
            }
            catch (Exception ex)
            {
                LogHelper.Info(string.Format("订单插入失败", ex.Message));
                flag = false;
            }
            return flag;
        }
        public string WebResponseGet(System.Net.HttpWebResponse webResponse)
        {
            System.IO.StreamReader responseReader = null;
            string responseData = "";
            try
            {
                responseReader = new System.IO.StreamReader(webResponse.GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                webResponse.GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }
            return responseData;
        }

        // 支付宝支付 1  代表成功   0 代表失败
        public void AliPay()
        {
            //*****************************************************************************************
            ///当不知道https的时候，请使用http
            string alipayNotifyURL = System.Web.Configuration.WebConfigurationManager.AppSettings["ZFBGateWayUrl"].ToString() + "service=notify_verify";

            string partner = System.Web.Configuration.WebConfigurationManager.AppSettings["ZFBPartner"].ToString();
            string key = System.Web.Configuration.WebConfigurationManager.AppSettings["ZFBKey"].ToString();

            alipayNotifyURL = alipayNotifyURL + "&partner=" + partner + "&notify_id=" + Request.Form["notify_id"];

            //获取支付宝ATN返回结果，true是正确的订单信息，false 是无效的
            string responseTxt = Get_Http(alipayNotifyURL, 120000);

            //****************************************************************************************
            int i;
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestarr = coll.AllKeys;

            //进行排序；
            string[] Sortedstr = SFO2O.BLL.Order.AliPay.BubbleSort(requestarr);

            //构造待md5摘要字符串 ；
            string prestr = "";
            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (Request.Form[Sortedstr[i]] != "" && Sortedstr[i] != "sign" && Sortedstr[i] != "sign_type")
                {
                    if (i == Sortedstr.Length - 1)
                    {
                        prestr = prestr + Sortedstr[i] + "=" + Request.Form[Sortedstr[i]];
                    }
                    else
                    {
                        prestr = prestr + Sortedstr[i] + "=" + Request.Form[Sortedstr[i]] + "&";
                    }
                }

            }
            prestr = prestr + key;

            string mysign = SFO2O.BLL.Order.AliPay.GetMD5(prestr);


            string sign = Request.Form["sign"];



            if (mysign == sign && responseTxt == "true" && Request.Form["TRADE_STATUS"] == "TRADE_FINISHED")   //check the Page url is send by alipay server and the status of pay
            {
                if (AliPayResult())
                {
                    Response.Write("success");     // returns "success" to alipay
                }
                else
                {
                    LogHelper.Error("插入订单失败");
                    Response.Write("fail");     //if update fail, returns "fail" to alipay
                }
            }

            else
            {
                LogHelper.Error("支付宝加密sign与mysign 不一致 返回的url 是：" + Request.Form);
                Response.Write("fail");     //if update fail, returns "fail" to alipay
            }

        }


        /// <summary>
        /// 易宝支付
        /// </summary>
        /// <param name="id">orderId</param>
        /// <param name="payType"></param>
        /// <returns></returns>
        public JsonResult YeePay(string id, int payType = 1)
        {

            Session["payorderCode"] = id;
            string mServer = ConfigurationManager.AppSettings["MServer"].ToString();
            //LogHelper.Info("===========易宝回调开始Info======" + DateTime.Now.ToString());
            string theme = "提示标语！";
            string msg = "";
            #region 初始化信息
            ////初始化信息
            //merchantaccount.Value = CustomerConfig.merchantAccount;
            //orderid.Value = "pay" + Guid.NewGuid().ToString().Replace("-", "pay").Substring(0, 8);
            //transtime.Value = DateUtil.GetTimeStamp().ToString();
            //amount.Value = "1";//金额
            //productcatalog.Value = "7"; //商品类别码   ----7:实物电商
            //productname.Value = "易宝支付测试商品-易宝收银台Demo";//商品名称  健康绿氧-(商品名称)
            //identitytype.Value = "2";//用户标识类型    2:用户 ID
            //identityid.Value = "test-" + "fengke";
            //terminaltype.Value = "1";
            //terminalid.Value = "123";
            //userip.Value = "127.0.0.0";
            //productdesc.Value = "测试商品";
            //version.Value = "0";//商户可以使用此参数定制调用的网页收银台版本：0：wap  1：pc
            //directpaytype.Value = "3"; //0：默认,1：微信支付,2：支付宝支付,3：一键支付,实现支付工具的直连跳转；
            ////paytool.Value = "2";//2：扫码  
            //currency.Value = "156";
            //orderexpdate.Value = "144000";//订单有效时间
            //fcallbackurl.Value = "http://" + Request.Url.Authority + "/CallBack.aspx";
            //callbackurl.Value = "http://" + Request.Url.Authority + "/CallBackServer.aspx";

            //paytypes.Value = "1";//支付方式 1 借记卡
            //cardno.Value = "6214830157037465";//银行卡号
            //idcardtype.Value = "01";//身份证
            //idcard.Value = "421125199210133016";//身份证类型
            //owner.Value = "夏闯富";//姓名
            //*****************修改的信息********************** 
            #endregion
            //请求参数
            if (!string.IsNullOrWhiteSpace(id))
            {
                var entity = buyOrderManager.GetOrderInfoByCode(id, this.LoginUser.UserID);
                if (entity != null)
                {
                    OrderPaymentEntity orderPaymentEntity = new OrderPaymentEntity();

                    orderPaymentEntity.TradeCode = "密：" + Guid.NewGuid();
                    orderPaymentEntity.UserId = this.LoginUser.UserID;
                    orderPaymentEntity.OrderType = 1;
                    //订单号
                    orderPaymentEntity.OrderCode = entity.OrderCode;
                    //待支付金额
                    orderPaymentEntity.PayAmount = entity.TotalAmount*100;
                    //已支付金额
                    orderPaymentEntity.PaidAmount = 0;
                    //1：支付宝  2：微信  3：易宝
                    orderPaymentEntity.PayPlatform =3;
                    orderPaymentEntity.PayType = 1;
                    orderPaymentEntity.PayStatus = 1;
                    orderPaymentEntity.PayTerminal = 1;
                    orderPaymentEntity.PayCompleteTime = null;
                    orderPaymentEntity.PayBackRemark = "";
                    orderPaymentEntity.Remark = "";
                    orderPaymentEntity.CreateTime = DateTime.Now;
                    orderPaymentEntity.CreateBy = this.LoginUser.UserName;
                    //int amount = Convert.ToInt32(entity.TotalAmount * 100);
                    int amount =(int)orderPaymentEntity.PayAmount;
                    //判断订单是否失效,如果失效就返回订单状态orderStatus=5

                    //参数int类型
                    //"transtime", "amount", "currency", "identitytype", "terminaltype", "orderexpdate", "version", "directpaytype"
                    SortedDictionary<string, object> data_request = new SortedDictionary<string, object>();
                    data_request.Add("merchantaccount", CustomerConfig.merchantAccount);
                    data_request.Add("orderid", orderPaymentEntity.TradeCode);
                    data_request.Add("transtime", Int32.Parse(DateUtil.GetTimeStamp().ToString()));
                    data_request.Add("amount", amount);
                    data_request.Add("productcatalog", "7");//商品类别码   ----7:实物电商
                    data_request.Add("productname", "健康绿氧-茅台酒");
                    data_request.Add("identitytype", Convert.ToInt32(2));
                    data_request.Add("identityid", entity.UserId.ToString());
                    data_request.Add("terminaltype", Convert.ToInt32(1));
                    data_request.Add("terminalid", "123");
                    data_request.Add("userip", "127.0.0.0");
                    data_request.Add("version", Convert.ToInt32(0));//商户可以使用此参数定制调用的网页收银台版本：0：wap  1：pc
                    data_request.Add("directpaytype", Convert.ToInt32(3));
                    data_request.Add("currency", Convert.ToInt32(156));
                    data_request.Add("orderexpdate", Convert.ToInt32(144000));
                    //data_request.Add("fcallbackurl", "http://" + Request.Url.Authority + "/CallBack.aspx");
                    //data_request.Add("callbackurl", "http://" + Request.Url.Authority + "/CallBackServer.aspx");
                    string fcallbackurl = mServer + "/my/list?status=-3";

                    string callbackurl = mServer + "/YeeCallBack.html";
                    //data_request.Add("fcallbackurl", "http://14l8185x70.51mypc.cn/my/list?status=-3");//页面回调
                    //data_request.Add("callbackurl", "http://14l8185x70.51mypc.cn/YeeCallBack.html");

                    data_request.Add("fcallbackurl", fcallbackurl);//页面回调
                    data_request.Add("callbackurl", callbackurl);
                    //==================================重要信息结束
                    data_request.Add("productdesc", "");
                    data_request.Add("paytool", "");
                    data_request.Add("userua", "");
                    data_request.Add("paytypes", "");
                    data_request.Add("cardno", "");
                    data_request.Add("idcardtype", "");
                    data_request.Add("idcard", "");
                    data_request.Add("owner", "");



                    #region 易宝支付
                    //请求链接
                    string url_request = URLConfig.payUrl;
                    //*************************************************

                    //商户私钥
                    string merchantPrivateKey = CustomerConfig.merchantPrivateKey;
                    //易宝公钥
                    string yeepayPublic = CustomerConfig.yeepayPublicKey;
                    //AES密钥
                    string aesKey = AES.GeyRandomAESKey();
                    //加密编码格式
                    string type = "UTF-8";

                    //日志记录
                    StringBuilder log = new StringBuilder();
                    log.Append(DateTime.Now.ToString() + "\n");
                    log.Append("测试功能：" + DateUtil.GetTimeStamp().ToString() + "\n");
                    log.Append("测试商编：" + CustomerConfig.merchantAccount + "\n");
                    log.Append("商户私钥：" + CustomerConfig.merchantPrivateKey + "\n");
                    log.Append("易宝公钥：" + CustomerConfig.yeepayPublicKey + "\n");
                    log.Append("AES密钥：" + AES.GeyRandomAESKey() + "\n");
                    log.Append("请求地址：：" + url_request + "\n");

                    //存储请求信息
                    //SortedDictionary<string, object> data_request = new SortedDictionary<string, object>();

                    //生成RSA签名信息
                    string sign = EncryptUtil.handleRSA(data_request, merchantPrivateKey, type);
                    log.Append("请求数据RSA加密结果：" + sign + "\n");

                    //将信息添加到请求数据中
                    data_request.Add("sign", sign);

                    //请求数据转化成json
                    string json_request = Newtonsoft.Json.JsonConvert.SerializeObject(data_request);
                    log.Append("请求数据AES加密前：" + json_request.ToString() + "\n");

                    //请求数据json进行AES加密
                    string data = AES.Encrypt(json_request, aesKey);
                    log.Append("请求数据json格式AES加密结果data：" + data.ToString() + "\n");

                    //将AES密钥进行加密
                    string encryptkey = EncryptUtil.handleRSA(aesKey, yeepayPublic, type);
                    log.Append("请求数据AES密钥加密结果encryptkey：" + data.ToString() + "\n");

                    //向服务器发出请求
                    //请求数据
                    string param = "data=" + HttpUtil.UrlEncode(data) + "&encryptkey=" + HttpUtil.UrlEncode(encryptkey) + "&merchantaccount=" + CustomerConfig.merchantAccount;
                    log.Append("请求数据：" + param.ToString() + "\n");
                    log.Append("请求链接：" + url_request.ToString() + "?" + param.ToString() + "\n");
                    log.Append("请求方式：" + "Post" + "\n");
                    log.Append("====================================分割线======================================\n");

                    //测试
                    string data_response = HttpRequest.instantPayRequest(url_request, param, true);
                    log.Append("响应的原始数据：" + data_response + "\n");

                    //请求结果反序列化
                    RespondJson respJson = null;
                    try
                    {
                        respJson = Newtonsoft.Json.JsonConvert.DeserializeObject<RespondJson>(data_response);
                    }
                    catch (Exception e1)
                    {
                        respJson = new RespondJson();
                        respJson.data = null;
                    }

                    //返回的json
                    string callback_data = "";

                    if (respJson.data != null)
                    {
                        //解密返回的aeskey
                        string aesKey_response = RSAFromPkcs8.decryptData(respJson.encryptkey, merchantPrivateKey, type);
                        log.Append("返回的AESkey  ： " + aesKey_response + "\n");
                        //解密data+验签
                        callback_data = EncryptUtil.checkYbCallbackResult(respJson.data, respJson.encryptkey, yeepayPublic, merchantPrivateKey, type);
                        log.Append("数据解密：" + callback_data + "\n");

                        ResponseData responseDate = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseData>(callback_data);

                        string msgErry = responseDate.error_msg + "___" + responseDate.error_code + "___" + responseDate.imghexstr + "___" + responseDate.merchantaccount + "___" + responseDate.orderid + "___" + responseDate.payurl + "___" + responseDate.sign;
                        LogHelper.Info(msgErry);

                        if (String.IsNullOrEmpty(responseDate.error_code) && String.IsNullOrEmpty(responseDate.error_msg))
                        {

                            //#region    创建支付订单信息

                            orderPaymentEntity.PayCode = responseDate.yborderid;


                            if (!buyOrderManager.AddOrderPayment(orderPaymentEntity))
                            {
                                LogHelper.Error(string.Format("订单申请支付信息插入OrderPayment失败，订单号{0}，支付流水号{1},申请支付金额{2}", entity.OrderCode, entity.TotalAmount));
                            }

                            //#endregion  

                            //易宝返回路径
                            if (!String.IsNullOrEmpty(responseDate.payurl))
                            {
                                //跳转易宝一键支付页面
                                // Response.Redirect(responseDate.payurl);
                                return Json(new { Type = 1, Content = "易宝重定向页面", Url = responseDate.payurl }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            //出现请求异常错误，跳转页面
                            msg = "请求超时，请刷新页面";
                        }

                    }
                    else
                    {
                        //出现请求异常错误，跳转页面
                        msg = "请求超时，请刷新页面";
                    }

                    #endregion
                }

            }
            string faildcallbackurl = mServer + "/Pay/ZFBReturnPage";
            return Json(new { Type = 1, Content = msg, Url = faildcallbackurl }, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// 易宝回掉
        /// </summary>
        public void YeeCallBack(string data, string encryptkey, string info)
        {
            LogHelper.Info(data + "============" + encryptkey + "===================" + info);
            LogHelper.Info("===========易宝回调开始Info======" + DateTime.Now.ToString());
            //LogHelper.WriteInfo(typeof(PayController), "易宝回调开始WriteInfo");
            //LogHelper.WriteLog(typeof(PayController), "WriteLog", "mmm", new Dictionary<string, object>(), new Exception());
            //LogHelper.ErrorMsg("易宝回调开始ErrorMsg");
            //LogHelper.Error("易宝回调开始ErrorMsg");
            string theme = "";

            //基本信息
            //商户私钥
            string merchantPrivateKey = CustomerConfig.merchantPrivateKey;
            //易宝公钥
            string yeepayPublic = CustomerConfig.yeepayPublicKey;
            //编码格式
            string type = "UTF-8";
            //日志
            StringBuilder log = new StringBuilder();
            log.Append(DateTime.Now.ToString() + "\n");
            log.Append("测试功能：" + theme + "\n");
            log.Append("测试商编：" + CustomerConfig.merchantAccount + "\n");
            log.Append("商户私钥：" + merchantPrivateKey + "\n");
            log.Append("易宝公钥：" + yeepayPublic + "\n");

            ////获取请求参数信息
            //string data = Request["data"];
            //string encryptkey = Request["encryptkey"];
            //string info = Request["info"];
            //支付逻辑处理
            try
            {
                #region 回调流程
                if (data != null && encryptkey != null)
                {
                    log.Append("原始返回信息：" + "\n");
                    log.Append("data        ：" + data + "\n");
                    log.Append("encryptkey  ：" + encryptkey + "\n");
                    //解出AES密钥
                    string aesKey = RSAFromPkcs8.decryptData(encryptkey, merchantPrivateKey, type);
                    log.Append("解密出的AESKey：" + aesKey + "\n");

                    //明文
                    string result = EncryptUtil.checkYbCallbackResult(data, encryptkey, yeepayPublic, merchantPrivateKey, type);
                    //result = {"sign":"Cw/TGUG9w8ult5BgMEvzK/ADdD2lX5v3RbBtIg2IP59+19VuQkyziZkFdZeFJIofIAVD+msw8Ou0EIPOXnNGH9XM1AmiOSNKOO6NtTJXwiRnUsVxaHW7Zz18YE74beFGJC3NNntDlzYNKMa0vy9WtMIZpswGktCDGrhJuyI2Z+s="
                    //,"merchantaccount":"10014981503"
                    //,"cardtype":1,"amount":4,"status":1,"bankcode":"CMBCHINA","bank":"招商银行"
                    //,"orderid":"S17070211110078","yborderid":"411707026753151336","lastno":"7465"}

                    // merchantaccount,cardtype,amount,status,bankcode,bank,orderid,yborderid,lastno

                    log.Append("解密返回信息  ：" + result + "\n");
                    //information.Value = result.Replace(",", "\n").Replace("{", "").Replace("}", "");
                    //typeinfo.Value = "请查看信息";

                    //下边这段日志仅仅是为了方便寻找功能
                    if (Request.HttpMethod == "GET")
                    {
                        log.Append("请求信息info  ：" + "支付功能+页面回调" + "\n");
                    }
                    else
                    {
                        log.Append("请求信息info  ：" + "支付功能+服务器回调" + "\n");
                        //SoftLog.LogStr(log.ToString(), theme);
                        LogHelper.Info(log.ToString());

                            //JuHeFuResponseModel model = new JuHeFuResponseModel();
                            // payBll.OrderReturn(model);
                        //==========================

                        //merorderid   商户订单号  orderid
                        //tradeid      交易流水号  yborderid
                        //successmoney  支付金额   amount
                        //userid        用户编号   
                        //success       订单状态   status
                        YeeCallBackResponseModel yeeCallBackResponseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<YeeCallBackResponseModel>(result);
                        JuHeFuResponseModel model = new JuHeFuResponseModel();
                        model.channeltradeid = yeeCallBackResponseModel.YborderId;
                        model.tradeid = yeeCallBackResponseModel.OrderId;
                        OrderPaymentEntity orderPayment = buyOrderManager.GetOrderPaymentByTradeCode(model.tradeid);
                        OrderInfoEntity orderInfo = orderManager.GetOrderInfoByCodeAndStatus(orderPayment.OrderCode);
                        model.userid = orderInfo.UserId.ToString();
                        model.successmoney = Convert.ToDecimal(yeeCallBackResponseModel.Amount / 100);
                        model.success = yeeCallBackResponseModel.Status.ToString();

                        payBll.OrderReturn(model);
                        Response.Write("SUCCESS");

                        Response.End();
                    }

                }
                //其他逻辑处理
                else if (info != null)
                {
                    //information.Value = data.ToString().Replace(",", "\n").Replace("{", "").Replace("}", "");
                    //typeinfo.Value = info;
                    log.Append("请求信息data  ：" + data + "\n");
                    log.Append("请求信息info  ：" + info + "\n");

                    if (data.Contains(".jpg"))
                    {
                        int start = data.LastIndexOf("imghexstr") + 10;
                        int length = data.IndexOf("orderid") - start - 1;
                        string location = data.Substring(start, length);
                        //picture.ImageUrl = "..\\Log\\img\\" + location;
                        //pictureLabel.Disabled = false;
                    }
                }
                else
                {
                    string typeinfo = "data或者encryptkey参数有问题";
                }

                //SoftLog.LogStr(log.ToString(), theme);
                LogHelper.Info(log.ToString());
                #endregion
            }
            catch (Exception ex)
            {
                log.Append("=========报错=========");
                log.Append(ex);
                //SoftLog.LogStr(log.ToString(), theme);

                LogHelper.Info(log.ToString());

            }


        }


        /// <summary>
        /// 积分支付
        /// </summary>
        /// <returns></returns>
        public JsonResult ScorePay(string orderCode, int payType = 1) 
        {

            string mServer = ConfigurationManager.AppSettings["MServer"].ToString();

            string msg = "支付失败";

            JuHeFuResponseModel model = new JuHeFuResponseModel();
            model.channeltradeid = "ccc_"+DateTime.Now.ToString("yyyyMMdd HH:mm:ss:fff");
            model.tradeid = orderCode;
            OrderPaymentEntity orderPayment = buyOrderManager.GetOrderPaymentByTradeCode(model.tradeid);
            OrderInfoEntity orderInfo = orderManager.GetOrderInfoByCodeAndStatus(orderPayment.OrderCode);
            model.userid = orderInfo.UserId.ToString();
            model.successmoney = orderInfo.PaidAmount;
            model.success = Convert.ToString(1);
            CustomerEntity userInfo = accountBll.GetUserById(ConvertHelper.ZParseInt32(model.userid, 0));
            //查询用户积分信息
            ReturnModel returnModel = GreenGetApiBll.GetUserScore(userInfo.UserName);
       
            //判断积分是否够用
         if (returnModel != null & returnModel.IsTrue & returnModel.ScoreData.Score >= orderInfo.PaidAmount) 
            {
                  //扣减积分流程   1：增加 ，2:扣减
                ReturnModel greenUpdateModel = GreenGetApiBll.UpdateUserScore(userInfo.UserName, orderInfo.PaidAmount, 2);
              if (greenUpdateModel.IsTrue)
              {
                 
                  bool isTrue = payBll.OrderReturn(model);
                  if (isTrue)
                  {
                      msg = "成功！";
                  }
                  else {
                      msg = "订单回写失败！";
                  }
              }
              else { msg = "积分扣减失败"; }
            }
         else { msg = "积分数量不足！"; }

            string faildcallbackurl = mServer + "/Pay/ZFBReturnPage";
            return Json(new { Type = 1, Content = msg, Url = faildcallbackurl }, JsonRequestBehavior.AllowGet);
        }

        //================================
        public String Get_Http(String a_strUrl, int timeout)
        {
            string strResult;
            try
            {

                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(a_strUrl);
                myReq.Timeout = timeout;
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.Default);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }

                strResult = strBuilder.ToString();
            }
            catch (Exception exp)
            {

                strResult = "错误：" + exp.Message;
                LogHelper.Error(strResult = "错误：" + exp.Message);
            }

            return strResult;
        }

        public void NormalOrderPage(OrderInfoEntity orderInfoEntity)
        {
            string taxtypeStr = orderInfoEntity.TaxType.ToString();

            /// 父订单号不为空说明还有包裹需要支付，否则没有下一单支付
            if (orderInfoEntity == null || string.IsNullOrEmpty(orderInfoEntity.ParentOrderCode) || orderInfoEntity.ParentOrderCode == "0")
            {
                ViewBag.parentOrderCode = null;
                //model.OrderCode = ViewBag.OrderCode;
            }
            else
            {
                /// 根据父订单号获得其下还没有支付的子订单号集合
                OrderInfoEntity needToPayOrderCode = orderManager.GetOrderCodeByParentOrderCode(orderInfoEntity.ParentOrderCode);
                if (needToPayOrderCode == null)
                {
                    ViewBag.parentOrderCode = null;
                    //model.OrderCode = ViewBag.OrderCode;
                }
                else
                {
                    ViewBag.parentOrderCode = orderInfoEntity.ParentOrderCode;
                    /// 目前只有两个包裹，所以只取集合的第一个元素
                    ViewBag.NextOrderCode = needToPayOrderCode.OrderCode;
                    ViewBag.package = Session["Package"];/// 以后需要修改获取包裹的逻辑
                }
            }

        }
        public ActionResult payIframe(string orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
        /// <summary>
        /// 微信支付新页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult WxPay(string id)
        {
            if (Session["id"] != null)
            {
                //LogHelper.Error("Session  id" + Session["id"].ToString());
                //LogHelper.Error("id" + id);
                if (Session["id"].ToString().Trim() != id)
                {
                    HttpContext.Session["openid"] = null;
                }
            }

            //LogHelper.Error("Get code 1: " + Request.QueryString["code"]);
            if (!string.IsNullOrEmpty(id))
            {
                //LogHelper.Error("程序开始id为" + id);
                Session["id"] = id;
            }
            //LogHelper.Error("开始id为" + id);
            if (string.IsNullOrWhiteSpace(id) && string.IsNullOrEmpty(Session["id"].ToString()))
            {
                //LogHelper.Error("这出错误了");
                return Redirect("/Home/Error");
            }
            else
            {
                if (string.IsNullOrEmpty(id))
                {
                    id = Session["id"].ToString();
                }
            }
            bool isfalse = false;
            if (HttpContext.Session["openid"] == null)
            {
                isfalse = true;
            }
            //LogHelper.Error("isfalse" + isfalse);
            WxPayController wxPay = new WxPayController();
            ViewBag.WeiXinData = wxPay.Page_Load(id, HttpContext, isfalse);
            return View();
        }
        //手动修改线上订单支付成功，回写失败的问题
        //public void repairOrder(string out_trade_no, string trade_no, string url)
        //{
        //    bool flag = PayBll.PayReturn(out_trade_no, trade_no, url, base.DeliveryRegion, base.language, 3);
        //}
        /// <summary>
        /// 测试专用支付
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult testPay(string id)
        {
            if (testEnvir())
            {
                bool flag = payBll.PayReturn(id, "123456", "mdemo.sfo2o.com", base.DeliveryRegion, base.language, 3);
                return RedirectToAction("ZFBReturnPage");
            }
            return null;
        }
        public static bool testEnvir()
        {
            string isOnline = System.Web.Configuration.WebConfigurationManager.AppSettings["isOnline"].ToString();
            string hosts = System.Web.HttpContext.Current.Request.Url.Host;
            if (isOnline.Equals("0") && (hosts.Equals("mdemo.sfo2o.com") || hosts.Equals("www.wdnzmt9.c1om") || hosts.Equals("www.wdnzmt9.net") || hosts.Equals("www.discountmassworld.com")))
            {
                return true;
            }
            return false;
        }
    }

}