using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SFO2O.BLL.My;
using SFO2O.M.Controllers.Filters;
using SFO2O.Model.Enum;
using SFO2O.Model.Product;
using SFO2O.Utility.Extensions;
using SFO2O.BLL.Account;
using SFO2O.References.SFo2oWCF;
using SFO2O.Model.My;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Information;
using SFO2O.BLL.Order;
using SFO2O.BLL.Information;
using SFO2O.BLL.Common;
using SFO2O.Utility;
using SFO2O.Model.Refund;
using SFO2O.Utility.Security;
namespace SFO2O.M.Controllers
{
    [NoCache]
    [Login]
    public class MyController : SFO2OBaseController
    {
        private MyBll Bll = new MyBll();
        private AccountBll accountBll = new AccountBll();
        private readonly OrderManager orderManager = new OrderManager();
        private static readonly InformationBll InformationBll = new InformationBll();
        private int PageSize = 20;
        private static readonly string wineGameUrl = System.Configuration.ConfigurationManager.AppSettings["WineGameWebApi"];
        /// <summary>
        /// 我的首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                var items = Bll.GetMyOrderIndex(base.LoginUser.UserID, base.DeliveryRegion, base.language);

                return this.View(items);
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }
        }



        /// <summary>
        /// 我的 订单列表页【异步请求为分页内容】
        /// </summary>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult List(int status = -3, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var orderStatus = status.ToString().ToEnum<OrderStatusEnum>();
                string title = "全部订单";
                if (status >= 0)
                {
                    title = orderStatus.ToDescriptionString();
                }

                var items = Bll.GetMyOrderListByStatus(base.LoginUser.UserID, base.DeliveryRegion, base.language, pageIndex, pageSize, orderStatus);
                if (IsAsync)
                {
                    return PartialView("_Items", items);
                }
                ViewBag.SubTitle = title;
                return this.View(items);
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }
        }

        /// <summary>
        /// 查看订单详情
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public ActionResult Detail(string orderCode)
        {
            try
            {
                ViewBag.RefundStatus = false;

                if (string.IsNullOrEmpty(orderCode))
                {
                    return this.HandleError("订单编号不能为空！");
                }

                var model = Bll.GetMyOrderInfo(base.LoginUser.UserID, base.DeliveryRegion, base.language, orderCode);
                //检查是否有退款单
                //var modelRefund = Bll.GetMyOrderInfoAndRefund(orderCode);


                //检查退款单
                //foreach (RefundInfoModel refundInfoModel in modelRefund)
                //{
                //    if (refundInfoModel.RefundStatus != 6 && refundInfoModel.RefundStatus != 5 && refundInfoModel.RefundStatus != 4)
                //    {
                //        ViewBag.RefundStatus = true;
                //        break;
                //    }
                //}
                //检查用户是否上传了身份证图片
                //if (model.OrderStatus == OrderStatusEnum.Shipped.ToInt())
                //{
                //    ViewBag.IdentityStatus = accountBll.CheckUserIndentity(model.Receiver, model.Phone);
                //}
                return View(model);
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }
        }
        /// <summary>
        /// 异步请求，取消订单
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Cancel(string orderCode)
        {
            try
            {
                if (string.IsNullOrEmpty(orderCode))
                {
                    return this.HandleError("订单编号不能为空！");
                }

                var model = Bll.CancelOrder(base.LoginUser.UserID, base.DeliveryRegion, base.language, orderCode);

                return this.HandleSuccess("ok");
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }
        }
        /// <summary>
        /// 确认订单
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConfirmOrder(string orderCode)
        {
            try
            {
                if (string.IsNullOrEmpty(orderCode))
                {
                    return this.HandleError("订单编号不能为空！");
                }

                var model = Bll.ConfirmOrder(base.LoginUser.UserID, base.DeliveryRegion, base.language, orderCode);
                if (model)
                {
                    LogHelper.Info("--------ConfirmOrder----获取订单的商品图片开始---orderCode：" + orderCode);

                    /// 获取订单的商品图片
                    ProductInfoModel productInfoModel = orderManager.GetOrderImage(orderCode);
                    LogHelper.Info("--------ConfirmOrder----获取订单的商品图片结束---orderCode：" + orderCode);

                    LogHelper.Info("--------ConfirmOrder----设置对象参数开始");
                    InformationEntity InformationEntity = new InformationEntity();
                    InformationEntity.InfoType = 1;
                    InformationEntity.WebInnerType = 3;
                    InformationEntity.SendDest = CommonBll.GetUserRegion(base.LoginUser.UserID);
                    InformationEntity.SendUserId = base.LoginUser.UserID;
                    InformationEntity.TradeCode = orderCode;
                    InformationEntity.Title = InformationUtils.OrderTradeSuccTitle;
                    InformationEntity.InfoContent = InformationUtils.OrderTradeSuccContent_Prefix
                                                + orderCode + InformationUtils.OrderTradeSuccContent_suffix;

                    if (productInfoModel != null)
                    {
                        InformationEntity.ImagePath = productInfoModel.ImagePath;
                    }
                    else
                    {
                        InformationEntity.ImagePath = null;
                    }

                    InformationEntity.Summary = null;
                    InformationEntity.LinkUrl = "my/detail?orderCode=" + orderCode;
                    InformationEntity.StartTime = null;
                    InformationEntity.EndTime = null;
                    InformationEntity.LongTerm = 0;
                    InformationEntity.CreateTime = DateTime.Now;
                    LogHelper.Info("--------ConfirmOrder----设置对象参数结束");

                    LogHelper.Info("--------ConfirmOrder----执行消息表插入方法开始");
                    InformationBll.AddInformation(InformationEntity);
                    LogHelper.Info("--------ConfirmOrder----执行消息表插入方法结束");

                    return this.HandleSuccess("ok");
                }
                else
                {
                    return this.HandleError("系统异常请稍后再试！");
                }
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }
        }

        /// <summary>
        /// 获取物流信息
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public ActionResult LogisticsInfo(string orderCode)
        {
            try
            {
                if (string.IsNullOrEmpty(orderCode))
                {
                    return this.HandleError("订单编号不能为空！");
                }

                var infos = Bll.GetOrderLogistics(orderCode);
                ViewBag.OrderCode = orderCode;
                var orderInfos = Bll.GetMyOrderInfo(base.LoginUser.UserID, 0, base.language, orderCode);
                ViewBag.TransportTime = orderInfos.DeliveryTime.AddDays(9).ToShortDateString().ToString();
                return View(infos);
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }
        }
        public ActionResult LogisticsInfoLast(string orderCode)
        {

            try
            {
                ViewBag.OrderCode = orderCode;
                if (string.IsNullOrEmpty(orderCode))
                {
                    return this.HandleError("订单编号不能为空！");
                }

                var infos = Bll.GetOrderLogistics(orderCode);

                var orderInfos = Bll.GetMyOrderInfo(base.LoginUser.UserID, 0, base.language, orderCode);
                ViewBag.TransportTime = orderInfos.DeliveryTime.AddDays(9).ToShortDateString().ToString();
                if (infos != null && infos.Count > 0)
                {
                    return PartialView(infos.FirstOrDefault());
                }
                else
                {
                    return PartialView();
                }
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }
        }
        #region 我的酒豆原来代码
        ///// <summary>
        ///// 我的酒豆
        ///// </summary>
        ///// <returns></returns>  
        //public ActionResult myHL()
        //{
        //    MyHL myHL = new MyHL();
        //    try
        //    {
        //        myHL = Bll.getMyHL(base.LoginUser.UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.HandleError(ex);
        //    }
        //    return View(myHL);
        //} 

        #endregion

     

        /// <summary>
        /// 酒豆流水页面
        /// </summary>
        public ActionResult HLDetail()
        {
            return View();
        }
        /// <summary>
        /// 我的酒豆水流
        /// </summary>
        public JsonResult HLDetailList(int type, int PageIndex = 1)
        {
            try
            {
                var HLList = HLDetail(base.LoginUser.UserID, type, PageSize, PageIndex);
                return Json(new { Type = 1, Data = new { PageIndex = HLList.PageIndex, PageSize = PageSize, PageCount = HLList.PageCount, TotalRecords = HLList.TotalRecord, Products = HLList.MyHLList } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 拼生活列表页
        /// </summary>
        /// <param name="c"></param>
        /// <param name="level"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="attrids"></param>
        /// <returns></returns>
        private MyHLModel HLDetail(int userId, int type, int PageSize, int PageIndex)
        {
            MyHLModel viewmodel = new MyHLModel();
            try
            {
                int totalRecords = 0, pagecount = 0;
                var products = Bll.HLDetail(base.LoginUser.UserID, type, PageSize, PageIndex);
                totalRecords = products.FirstOrDefault().TotalRecord;
                viewmodel.MyHLList = products;
                viewmodel.PageSize = PageSize;
                viewmodel.TotalRecord = totalRecords;
                pagecount = totalRecords / PageSize;
                if (totalRecords % PageSize > 0)
                {
                    pagecount += 1;
                }
                viewmodel.PageCount = pagecount;
                viewmodel.PageIndex = PageIndex;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return viewmodel;
        }
        public JsonResult updateIsPush(int type)
        {
            try
            {
                bool IsPush = Bll.updateIsPush(base.LoginUser.UserID, type);
                if (IsPush)
                {
                    // 更新用户活动消息可见标识值
                    bool IsVisible = Bll.updateActivityInfoVisible(base.LoginUser.UserID, type, DateTime.Now);
                    if (!IsVisible)
                    {
                        return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { Type = 1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult set()
        {
            return View(Bll.getUserInfo(base.LoginUser.UserID));
        }
        /// <summary>
        /// 酒豆规则
        /// </summary>
        public ActionResult rule()
        {
            return View();
        }
        public bool getOrderInfoCount()
        {
            string startTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinStartTime"].ToString();
            string endTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinEndTime"].ToString();

            string PinHuoLiStartTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinHuoLiStartTime"].ToString();
            string PinHuoLiEndTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinHuoLiEndTime"].ToString();

            bool istrue = Bll.getOrderInfoCount(base.LoginUser.UserID, startTime, endTime);
            //if (Bll.getOrderInfoCount(base.LoginUser.UserID, startTime, endTime) && DateTime.Now >= Convert.ToDateTime(PinHuoLiStartTime) && DateTime.Now <= Convert.ToDateTime(PinHuoLiEndTime))
            // {
            //  istrue = false;
            //   }
            return istrue;
        }

        #region  生成我的推广链接
        /// <summary>
        /// 生成我的推广链接
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateMyLink()
        {
            bool result = false;

            string link = "";
            try
            {
                string userName = this.LoginUser.UserName;
                string token = MD5Hash.CommonMd5Encrypt(userName + MD5Hash.CommonMd5Encrypt(ConfigHelper.GetAppConfigString("LinkTokenKey")));

                //string token = MD5Hash(userInfo.UserName + Auxiliary.Md5Encrypt(Auxiliary.ConfigKey("LinkTokenKey")));

                link = "http://www.discountmassworld.com/account/register" + "?mobilePhone=" + userName + "&token=" + token;

                result = true;

            }
            catch (Exception ex)
            {

              //  LogHelper.WriteInfo(typeof(UserController), "CreateMyLink  UserId:" + _ServiceContext.CurrentUser.UserId, Engineer.ggg, ex);
            }


            return Json(new { result = result, msg = link });




        }

        #endregion
    }
}
