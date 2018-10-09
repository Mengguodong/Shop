using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.DAL.Promotion;
using SFO2O.DAL.SupplierBrand;
using SFO2O.Model.Promotion;
using SFO2O.Model.Supplier;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Order;
using SFO2O.BLL.Order;
using SFO2O.Utility;
using SFO2O.Model.Product;
using SFO2O.Model.Information;
using SFO2O.BLL.Common;
using SFO2O.BLL.Information;
using SFO2O.Model.Team;
using SFO2O.BLL.My;
using SFO2O.BLL.Team;
using SFO2O.Model.Pay;
using SFO2O.Utility.Security;
using SFO2O.BLL.Account;
using SFO2O.Model.Account;

namespace SFO2O.BLL.Pay
{
    public class PayBll
    {
        private readonly BuyOrderManager buyOrderManager = new BuyOrderManager();
        private readonly OrderManager orderManager = new OrderManager();
        private static readonly InformationBll InformationBll = new InformationBll();
        private readonly MyBll Bll = new MyBll();
        private readonly TeamBll teamBll = new TeamBll();
        private readonly AccountBll accountBll = new AccountBll();

        private static readonly string wineGameUrl = System.Configuration.ConfigurationManager.AppSettings["WineGameWebApi"];

        /// <summary>
        /// 获得支付信息
        /// </summary>
        /// <param name="OrderCode"></param>
        /// <returns></returns>
        public OrderPaymentEntity GetOrderPaymentInfo(string OrderCode, int PayPlatform)
        {
            // 通过orderCode 找到OrderPayment 的最近一条记录
            OrderPaymentEntity orderPayment = new OrderPaymentEntity();
            orderPayment = buyOrderManager.selectOrderCode(OrderCode, PayPlatform);
            var entity = buyOrderManager.GetOrderPaymentByCode(orderPayment.PayCode);
            return entity;
        }

        /// <summary>
        /// 组件订单支付信息对象
        /// </summary>
        /// <param name="TradeNo"></param>
        /// <param name="entity"></param>
        /// <param name="PayBackRemark"></param>
        /// <returns></returns>
        public OrderPaymentEntity BuildOrderPaymentEntity(string TradeNo, OrderPaymentEntity entity, string PayBackRemark)
        {
            //组建需要更新的字段
            OrderPaymentEntity orderPaymentEntity = new OrderPaymentEntity();
            //orderPaymentEntity.OrderCode = out_trade_no;
            orderPaymentEntity.TradeCode = TradeNo;
            orderPaymentEntity.PaidAmount = StringUtils.ToDecimal(entity.PayAmount);
            orderPaymentEntity.PayStatus = 2;
            orderPaymentEntity.PayCompleteTime = DateTime.Now;
            orderPaymentEntity.PayBackRemark = PayBackRemark;
            orderPaymentEntity.PayCode = entity.PayCode;



            return orderPaymentEntity;
        }

        /// <summary>
        /// 更新库存、支付和订单等信息
        /// </summary>
        /// <param name="TradeNo"></param>
        /// <param name="entity"></param>
        /// <param name="PayBackRemark"></param>
        public void UpdatePayInfo(string TradeNo, OrderPaymentEntity entity, string PayBackRemark, OrderPaymentEntity orderPaymentEntity, int OrderType)
        {
            //LogHelper.Error("--------OrderPayAfter----1.4-----更新orderPayment开始：");
            //更新orderPayment
            buyOrderManager.UpdatePaysuccess(orderPaymentEntity);
            //LogHelper.Error("--------OrderPayAfter----1.4-----更新orderPayment结束：");

            //LogHelper.Error("--------OrderPayAfter----1.5-----OrderType：" + OrderType);
            if (OrderType == 1)
            {
                //LogHelper.Error("--------OrderPayAfter----1.6-----OrderPayOk处理开始：");
                //LogHelper.Error("--------OrderPayAfter----1.6-----" + entity.OrderCode);
                buyOrderManager.OrderPayOk(entity.OrderCode, orderPaymentEntity.PaidAmount, "");
                //LogHelper.Error("--------OrderPayAfter----1.6-----OrderPayOk处理结束：");
            }

        }

        /// <summary>
        /// 添加普通订单消息信息
        /// </summary>
        /// <param name="OrderCode"></param>
        /// <param name="entity"></param>
        public void AddNoemalOrderInformationInfo(string OrderCode, OrderPaymentEntity entity)
        {
            //LogHelper.Error("--------获取订单图片After开始----1-----" + OrderCode);
            ProductInfoModel productInfoModel = orderManager.GetOrderImage(OrderCode);
            //LogHelper.Error("--------获取订单图片After结束----1-----" + productInfoModel);

            //LogHelper.Error("--------消息对象设置参数After开始----1-----" + entity.UserId);

            InformationEntity InformationEntity = new InformationEntity();
            InformationEntity.InfoType = 1;
            InformationEntity.WebInnerType = 3;
            InformationEntity.SendDest = CommonBll.GetUserRegion(entity.UserId);
            InformationEntity.SendUserId = entity.UserId;
            InformationEntity.TradeCode = OrderCode;
            InformationEntity.Title = InformationUtils.UserPaySuccTitle;
            InformationEntity.InfoContent = InformationUtils.UserPaySuccContent_Prefix
                                    + OrderCode + InformationUtils.UserPaySuccContent_suffix;

            if (productInfoModel != null)
            {
                //LogHelper.Error("--------消息对象设置参数After开始----2-----" + productInfoModel.ImagePath);
                InformationEntity.ImagePath = productInfoModel.ImagePath;
            }
            else
            {
                InformationEntity.ImagePath = null;
            }

            InformationEntity.Summary = null;
            InformationEntity.LinkUrl = "my/detail?orderCode=" + OrderCode;
            InformationEntity.StartTime = null;
            InformationEntity.EndTime = null;
            InformationEntity.LongTerm = 0;
            InformationEntity.CreateTime = DateTime.Now;

            //LogHelper.Error("--------插入消息表开始----1-----");
            InformationBll.AddInformation(InformationEntity);
        }

        /// <summary>
        /// 普通订单支付处理
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="OrderCode"></param>
        /// <param name="TradeNo"></param>
        /// <param name="PayBackRemark"></param>
        /// <returns></returns>
        public bool NomalOrderPay(OrderPaymentEntity entity, string OrderCode, string TradeNo, string PayBackRemark, OrderPaymentEntity orderPaymentEntity, int OrderType)
        {
            if (entity.PayStatus != 1)
            {
                //LogHelper.Error("订单异常 发给支付宝的支付号为：" + OrderCode);
                return false;
            }
            else
            {
                // 更新库存、支付和订单等信息
                UpdatePayInfo(TradeNo, entity, PayBackRemark, orderPaymentEntity, OrderType);

                // 添加需要推送的订单信息到push表中
                // orderManager.AddPushOrderInfo(OrderCode);

                // 添加普通订单消息信息
                // AddNoemalOrderInformationInfo(OrderCode, entity);

                return true;
            }
        }

        /// <summary>
        /// 获得参团成功的团员数量
        /// </summary>
        /// <param name="teamDetailList"></param>
        /// <returns></returns>
        public int GetTeamSucStatusCount(IList<TeamDetailEntity> teamDetailList)
        {
            int teamSucStatusCount = 0;
            foreach (TeamDetailEntity teamDetail in teamDetailList)
            {
                if (teamDetail.OrderStatus == 6)
                {
                    teamSucStatusCount = teamSucStatusCount + 1;
                }
            }
            return teamSucStatusCount;
        }

        /// <summary>
        /// 更新订单相关信息和保存Push信息表
        /// </summary>
        /// <param name="OrderInfoList"></param>
        /// <param name="orderPaymentEntity"></param>
        /// <param name="teamInfoEntity"></param>
        public void UpdateAndSavePushOrderInfo(IList<OrderInfoEntity> OrderInfoList, OrderPaymentEntity orderPaymentEntity, TeamInfoEntity teamInfoEntity)
        {
            foreach (OrderInfoEntity orderInfo in OrderInfoList)
            {
                //LogHelper.Info("--------TeamPayAfter----4.1.1-----" + orderInfo.OrderCode);
                //LogHelper.Info("--------TeamPayAfter----4.1.1-----状态：" + orderInfo.OrderStatus);
                if (orderInfo.OrderStatus == 6)
                {
                    // 订单状态设置为1
                    int OrderStatus = 1;
                    /// 团订单更新订单信息表和团信息表状态
                    buyOrderManager.TeamOrderPayOKForStatus(orderInfo.OrderCode, orderPaymentEntity.PaidAmount, "", OrderStatus, teamInfoEntity);
                    /// 添加需要推送的订单信息到push表中
                    orderManager.AddPushOrderInfo(orderInfo.OrderCode);
                }
            }
        }

        /// <summary>
        /// 关闭没有支付的订单
        /// </summary>
        /// <param name="teamInfoEntity"></param>
        /// <param name="DeliveryRegion"></param>
        /// <param name="language"></param>
        public void CloseNotPayOrderInfo(TeamInfoEntity teamInfoEntity, int DeliveryRegion, int language)
        {
            //LogHelper.Info("--------TeamPayAfter----close OrderInfo--111-----开始---团code:" + teamInfoEntity.TeamCode);
            /// 将下了订单没有支付的订单状态置为关闭
            IList<OrderInfoEntity> NeedCloseOrderInfoList = orderManager.GetNeedCloseOrderInfoByCode(teamInfoEntity.TeamCode);

            //LogHelper.Info("--------TeamPayAfter----close OrderInfo-----开始---需要关闭订单的数量:" + NeedCloseOrderInfoList.Count());

            foreach (OrderInfoEntity orderInfo in NeedCloseOrderInfoList)
            {
                var model = Bll.CancelOrder(orderInfo.UserId, DeliveryRegion, language, orderInfo.OrderCode);
                //LogHelper.Info("--------TeamPayAfter----close OrderInfo-----结束---关闭订单结果:" + model);
            }
        }

        /// <summary>
        /// 添加团订单消息信息
        /// </summary>
        /// <param name="teamDetailList"></param>
        public void AddTeamOrderInformationInfo(IList<TeamDetailEntity> teamDetailList)
        {
            //LogHelper.Info("--------TeamPayAfter 消息插入循环进入前----");
            foreach (TeamDetailEntity teamDetail in teamDetailList)
            {
                //LogHelper.Info("--------TeamPayAfter 消息插入循环----" + teamDetail.OrderCode);
                ProductInfoModel productInfoModel = orderManager.GetOrderImage(teamDetail.OrderCode);
                //LogHelper.Info("--------TeamPayAfter 获取商品图片结束----productInfoModel=" + productInfoModel);

                InformationEntity InformationEntityTeam = new InformationEntity();
                InformationEntityTeam.InfoType = 1;
                InformationEntityTeam.WebInnerType = 3;
                InformationEntityTeam.SendDest = CommonBll.GetUserRegion(teamDetail.UserId);
                InformationEntityTeam.SendUserId = teamDetail.UserId;
                InformationEntityTeam.TradeCode = teamDetail.TeamCode;
                InformationEntityTeam.Title = InformationUtils.TeamJoinSuccTitle;
                InformationEntityTeam.InfoContent = InformationUtils.TeamJoinSuccContent;

                if (productInfoModel != null)
                {
                    InformationEntityTeam.ImagePath = productInfoModel.ImagePath;
                }
                else
                {
                    InformationEntityTeam.ImagePath = null;
                }

                InformationEntityTeam.Summary = null;
                InformationEntityTeam.LinkUrl = "team/teamDetail?TeamCode=" + teamDetail.TeamCode + "&Flag=1";
                InformationEntityTeam.StartTime = null;
                InformationEntityTeam.EndTime = null;
                InformationEntityTeam.LongTerm = 0;
                InformationEntityTeam.CreateTime = DateTime.Now;

                //LogHelper.Info("--------TeamPayAfter 消息插入开始----");
                InformationBll.AddInformation(InformationEntityTeam);
            }
        }

        /// <summary>
        /// 团长订单的订单来源类型和订单来源值处理
        /// </summary>
        /// <param name="teamDetailList"></param>
        /// <returns></returns>
        public bool UpdateTeamHeadSource(IList<TeamDetailEntity> teamDetailList)
        {
            //LogHelper.Info("--------TeamPayAfter----开始Source:");
            OrderInfoSourceEntity orderInfoSource = null;

            //LogHelper.Info("--------TeamPayAfter----开始Source:teamDetailList=" + teamDetailList);
            foreach (TeamDetailEntity teamDetail in teamDetailList)
            {
                /// 判断是团长
                if (teamDetail.UserId == teamDetail.TeamHead)
                {
                    /// 团长订单的订单来源类型和订单来源值不为空
                    if (teamDetail.OrderSourceType != 0 && !string.IsNullOrEmpty(teamDetail.OrderSourceValue))
                    {
                        orderInfoSource = new OrderInfoSourceEntity();
                        orderInfoSource.TeamCode = teamDetail.TeamCode;
                        orderInfoSource.OrderSourceType = teamDetail.OrderSourceType;
                        orderInfoSource.OrderSourceValue = teamDetail.OrderSourceValue;
                        orderInfoSource.DividedAmount = teamDetail.DividedAmount;
                        orderInfoSource.DividedPercent = teamDetail.DividedPercent;
                    }
                }
            }
            //LogHelper.Info("--------TeamPayAfter----开始Source:orderInfoSource=" + orderInfoSource);
            /// 订单来源值对象为空
            if (orderInfoSource == null)
            {
                return false;
            }

            //LogHelper.Info("--------TeamPayAfter----开始团成员订单订单来源相关值的更新:teamDetailList=" + teamDetailList);
            foreach (TeamDetailEntity teamDetail in teamDetailList)
            {
                /// 判断是团员
                if (teamDetail.UserId != teamDetail.TeamHead)
                {
                    /// 团成员订单订单来源相关值的更新
                    buyOrderManager.TeamMemberOrderUpdatePayOK(orderInfoSource);
                }
            }

            return true;
        }

        /// <summary>
        /// 团订单最后一人支付
        /// </summary>
        /// <param name="teamInfoEntity"></param>
        /// <param name="entity"></param>
        /// <param name="orderPaymentEntity"></param>
        /// <param name="DeliveryRegion"></param>
        /// <param name="language"></param>
        /// <param name="teamDetailList"></param>
        /// <returns></returns>
        public void TeamOrderLastPay(TeamInfoEntity teamInfoEntity, OrderPaymentEntity entity, OrderPaymentEntity orderPaymentEntity
                        , int DeliveryRegion, int language, IList<TeamDetailEntity> teamDetailList)
        {
            /// 订单状态为6
            int OrderStatus = 6;

            /// 更新TeamInfo表的TeamStatus字段为3，组团成功
            teamInfoEntity.TeamStatus = 3;

            /// 团订单更新库存、订单信息表和团信息表状态
            buyOrderManager.TeamOrderPayOK(entity.OrderCode, orderPaymentEntity.PaidAmount, "", OrderStatus, teamInfoEntity);

            // 根据团Code获得订单信息集合
            IList<OrderInfoEntity> OrderInfoList = orderManager.GetOrderInfoByTeamCode(teamInfoEntity.TeamCode);
            if (OrderInfoList != null && OrderInfoList.Count() != 0)
            {
                //LogHelper.Info("--------TeamPayAfter----4.1-----" + OrderInfoList.Count());

                // 更新订单相关信息和保存Push信息表
                UpdateAndSavePushOrderInfo(OrderInfoList, orderPaymentEntity, teamInfoEntity);

                int count = OrderInfoList.Where(d => d.OrderStatus != 6).Count();
                //LogHelper.Info("--------TeamPayAfter----4.0.1-----状态不是6的集合长度：" + count);
                if (count != 0)
                {
                    // 关闭没有支付的订单
                    CloseNotPayOrderInfo(teamInfoEntity, DeliveryRegion, language);
                }

                // 添加团订单消息信息
                AddTeamOrderInformationInfo(teamDetailList);

                // 团长订单的订单来源类型和订单来源值处理
                UpdateTeamHeadSource(teamDetailList);
            }
        }

        /// <summary>
        /// 团订单不是最后一人支付
        /// </summary>
        /// <param name="teamInfoEntity"></param>
        /// <param name="entity"></param>
        /// <param name="orderPaymentEntity"></param>
        /// <returns></returns>
        public void TeamOrderNotLastPay(TeamInfoEntity teamInfoEntity, OrderPaymentEntity entity, OrderPaymentEntity orderPaymentEntity)
        {
            /// 订单状态为6
            int OrderStatus = 6;

            /// 更新TeamInfo表的TeamStatus字段为1，参团中
            teamInfoEntity.TeamStatus = 1;

            //LogHelper.Info("--------TeamPayAfter----15-----不是最后支付" + teamInfoEntity.TeamStatus);
            /// 团订单更新库存和订单信息表
            buyOrderManager.TeamOrderPayOK(entity.OrderCode, orderPaymentEntity.PaidAmount, "", OrderStatus, teamInfoEntity);
        }

        /// <summary>
        /// 团订单支付处理
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="OrderCode"></param>
        /// <param name="teamInfoEntity"></param>
        /// <param name="TradeNo"></param>
        /// <param name="PayBackRemark"></param>
        /// <param name="DeliveryRegion"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public bool TeamOrderPay(OrderPaymentEntity entity, string OrderCode, TeamInfoEntity teamInfoEntity
            , string TradeNo, string PayBackRemark, int DeliveryRegion, int language)
        {
            if (entity.PayStatus != 1)
            {
                //LogHelper.Error("订单异常 发给支付宝的支付号为：" + OrderCode);
                return false;
            }
            else
            {
                if (teamInfoEntity.TeamStatus == 2 || teamInfoEntity.TeamStatus == 3)
                {
                    //LogHelper.Error("团订单异常 发给支付宝的支付号为：" + OrderCode);
                    return false;
                }

                // 组件订单支付信息对象
                OrderPaymentEntity orderPaymentEntity = BuildOrderPaymentEntity(TradeNo, entity, PayBackRemark);

                // 更新库存、支付和订单等信息
                UpdatePayInfo(TradeNo, entity, PayBackRemark, orderPaymentEntity, 2);

                //LogHelper.Info("--------TeamPayAfter----2-----" + teamInfoEntity);

                /// 获取团详情信息
                var teamDetailList = teamBll.GetTeamDetailListForStatus(teamInfoEntity.TeamCode);
                //LogHelper.Info("--------TeamPayAfter----3-----" + teamDetailList.Count());

                if (teamDetailList == null)
                {
                    return false;
                }

                // 获得参团成功的团员数量
                int teamSucStatusCount = GetTeamSucStatusCount(teamDetailList);

                /// 团订单中最后一人支付成功
                if (teamSucStatusCount == (teamDetailList.First().TeamNumbers - 1))
                {
                    //LogHelper.Info("--------TeamPayAfter----4-----最后一人支付成功" + teamSucStatusCount);

                    // 团订单最后一人支付
                    TeamOrderLastPay(teamInfoEntity, entity, orderPaymentEntity
                                          , DeliveryRegion, language, teamDetailList);
                }
                /// 不是最后一人支付成功
                else
                {
                    //LogHelper.Info("--------TeamPayAfter----5-----" + teamSucStatusCount);

                    // 团订单不是最后一人支付
                    TeamOrderNotLastPay(teamInfoEntity, entity, orderPaymentEntity);
                }

                return true;
            }
        }

        /// <summary>
        /// 支付回写
        /// </summary>
        /// <param name="OrderCode"></param>
        /// <param name="TradeNo"></param>
        /// <param name="PayBackRemark"></param>
        /// <param name="DeliveryRegion"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public bool PayReturn(string OrderCode, string TradeNo, string PayBackRemark, int DeliveryRegion, int language, int PayPlatform)
        {
            LogHelper.Error("-----支付回写1 订单号：" + OrderCode + " ,支付宝回写参数：" + PayBackRemark + "--------");
            bool flag = false;
            // 获得支付信息
            OrderPaymentEntity entity = GetOrderPaymentInfo(OrderCode, PayPlatform);

            ///// 获取订单所属的团信息
            //TeamInfoEntity teamInfoEntity = teamBll.GetTeamInfoEntity(OrderCode);

            /// 普通订单
            //if (teamInfoEntity == null)
            //{
            // 组件订单支付信息对象
            OrderPaymentEntity orderPaymentEntity = BuildOrderPaymentEntity(TradeNo, entity, PayBackRemark);

            /*LogHelper.Error("--------BuildOrderPaymentEntity----1-----赋值前参数PayCode:" + entity.PayCode
                + "--------BuildOrderPaymentEntity----2-----赋值后参数PayCode:" + orderPaymentEntity.PayCode);*/

            // 普通订单支付处理
            flag = NomalOrderPay(entity, OrderCode, TradeNo, PayBackRemark, orderPaymentEntity, 1);
            //}
            //else
            //{
            //    // 团订单支付处理
            //    flag = TeamOrderPay(entity, OrderCode, teamInfoEntity, TradeNo
            //                    , PayBackRemark, DeliveryRegion, language);
            //}
            return flag;
        }

        /// <summary>
        /// 获取支付二维码
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        public CodeResult WXPayRequest(OrderInfoEntity orderModel)
        {
            CodeResult result = new CodeResult();



            string parameterString = @"version=1.0&merid=26100794&mername=北京联友创新科技发展有限公司&merorderid="
                                   + orderModel.OrderCode + "&paymoney=" + orderModel.TotalAmount + "&productname=" +
                                  "&productdesc=&userid=" + orderModel.UserId + "&username=" + orderModel.Phone +
                                  "&email=&phone=&extra=&custom=";
            string md5 = MD5Hash.GetMD5String(parameterString);//md5校验值

            //发送支付请求

            try
            {
                string url = "";
                //微信支付
                if (orderModel.PayType == 1)
                {

                    url = ConfigHelper.GetAppConfigString("WXPayUrl") + @"?version=1.0&merid=26100794&mername=北京联友创新科技发展有限公司&merorderid="
                                      + orderModel.OrderCode + "&paymoney=" + orderModel.TotalAmount + "&productname=" +
                                     "&productdesc=&userid=" + orderModel.UserId + "&username=" + orderModel.Phone +
                                     "&email=&phone=&extra=&custom=&md5=" + md5;
                }
                //支付宝支付
                if (orderModel.PayType == 2)
                {

                    url = ConfigHelper.GetAppConfigString("ZFBPayUrl") + @"?version=1.0&merid=26100794&mername=北京联友创新科技发展有限公司&merorderid="
                                     + orderModel.OrderCode + "&paymoney=" + orderModel.TotalAmount + "&productname=" +
                                    "&productdesc=&userid=" + orderModel.UserId + "&username=" + orderModel.Phone +
                                    "&email=&phone=&extra=&custom=&md5=" + md5;
                }


                LogHelper.Info("url="+url);
                result = HttpClientHelper.GetResponse<CodeResult>(url);


            }
            catch (Exception ex)
            {

                //LogHelper.WriteLog(typeof(WXPayBLL), "WXPayRequest", Engineer.ggg, orderModel, ex);

                LogHelper.WriteInfo(typeof(PayBll), ex.Message);
            }


            return result;

        }
        /// <summary>
        /// 扫码支付回调
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool PayCallBackRetunResult(JuHeFuResponseModel model)
        {
            bool isOk = false;
            try
            {
           
                #region 验证参数

                //验证参数有效性
                if (model == null)
                {
                    //参数有误，记录日志
                    LogHelper.WriteInfo(typeof(PayBll), string.Format("PayCallBackRetunResult支付回调操作失败信息：参数为null---{0}", model));
                    return false;
                }

                if (string.IsNullOrWhiteSpace(model.merid) || string.IsNullOrWhiteSpace(model.merorderid) || string.IsNullOrWhiteSpace(model.tradeid) || string.IsNullOrWhiteSpace(model.success) || model.successmoney == 0 || string.IsNullOrWhiteSpace(model.userid) || string.IsNullOrWhiteSpace(model.sign) || string.IsNullOrWhiteSpace(model.md5))
                {
                    LogHelper.WriteInfo(typeof(PayBll), string.Format("PayCallBackRetunResult支付回调操作失败信息：参数不完整---{0}", JsonHelper.ToJson(model)));
                    //参数有误，记录日志
                    return false;
                }

                string parameterString = string.Format(@"version={0}&merid={1}&merorderid={2}&tradeid={3}&tradedate={4}&success={5}" +
    "&successmoney={6}&paychannel={7}&channeltradeid={8}" +
                                    "&cardid={9}&userid={10}&username={11}&extra={12}&attach={13}&internal={14}", model.version, model.merid, model.merorderid, model.tradeid, model.tradedate, model.success, model.successmoney, model.paychannel, model.channeltradeid, model.cardid, model.userid, model.username, model.extra, model.attach, model.Internal);

                //验证md5、签名信息
                string md5 = MD5Hash.GetMD5String(parameterString);
                isOk = md5.Trim().ToUpper() == model.md5.Trim().ToUpper();

                #endregion
                object objLock = new object();


                lock (objLock)
                {

                    if (!isOk)
                    {

                        LogHelper.WriteInfo(typeof(PayBll), string.Format("回调信息签名信息验证不通过！---------{0}",model));

                        return false;
                    }
                    else
                    {
                        //订单回调
                        isOk = JHFOrderReturn(model);

                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteInfo(typeof(PayBll), string.Format("支付回调出错！-------实体：{0}错误信息：{1}", JsonHelper.ToJson(model), ex.Message));
                return false;
            }
            return isOk;
        }
        /// <summary>
        /// 订单回写
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool OrderReturn(JuHeFuResponseModel model)
        {

            bool isOk = false;
           CustomerEntity userInfo = accountBll.GetUserById(ConvertHelper.ZParseInt32(model.userid,0));

            #region 操作充值、回写订单

            //验证订单号、流水号
           OrderPaymentEntity entity = buyOrderManager.GetOrderPaymentByTradeCode(model.tradeid);
            if (entity == null)
            {
                LogHelper.WriteInfo(typeof(PayBll), string.Format("PayCallBackRetunResult支付回调操作失败信息：订单不存在-----{0}", JsonHelper.ToJson(model)));
                //该订单不存在，记录日志
                return false;
            }
            if (entity.UserId.ToString().Trim() != model.userid.Trim() || entity.PayCode != model.channeltradeid)
            {
                LogHelper.WriteInfo(typeof(PayBll), string.Format("PayCallBackRetunResult支付回调操作失败信息：UserId、Tradeid跟订单信息不匹配----{0}", JsonHelper.ToJson(model)));
                //信息有误，记录日志
                return false;
            }
            if (model.success == "1")
            {
                //调用充值
                if (entity.PayStatus == 1)
                {

                    if (userInfo.SourceType==10)
                    {
                        #region 调用充值接口



                        //获取订单活力
                        decimal coinCount = orderManager.GetHuoLiByOrderId(entity.OrderCode);

                          LogHelper.WriteInfo(typeof(PayBll), string.Format("调用酒币充值前记录！-----用户ID：{0}，充值金额：{1}", userInfo.UserName, coinCount));

                        #endregion

                        
                    }


                    //订单回写

                    OrderPaymentEntity orderPaymentEntity = BuildOrderPaymentEntity(model.tradeid, entity, null);

                    isOk = NomalOrderPay(entity, model.merorderid, model.tradeid, "", orderPaymentEntity, 1);
                    if (!isOk)
                    {
                        LogHelper.WriteInfo(typeof(PayBll), string.Format("PayCallBackRetunResult支付回调操作失败信息：充值成功，但订单状态更新失败！-----{0}", JsonHelper.ToJson(model)));
                        return false;
                    }
                    else
                    {
                        LogHelper.WriteInfo(typeof(PayBll), string.Format("PayCallBackRetunResult支付回调操作成功！-----{0}", JsonHelper.ToJson(model)));
                    }

                }
            }
            //失败
            else if (model.success == "0")
            {

                //记录日志
                LogHelper.WriteInfo(typeof(PayBll), string.Format("PayCallBackRetunResult支付回调操作失败信息：支付失败！----{0}", JsonHelper.ToJson(model)));
                return false;
            }
            else
            {
                //非法参数
                LogHelper.WriteInfo(typeof(PayBll), string.Format("PayCallBackRetunResult支付回调操作失败信息：data.success非法！-----{0}", JsonHelper.ToJson(model)));
                return false;
            }
            return isOk;
            #endregion
        }

        /// <summary>
        /// 订单回写
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool JHFOrderReturn(JuHeFuResponseModel model)
        {

            bool isOk = false;
            CustomerEntity userInfo = accountBll.GetUserById(ConvertHelper.ZParseInt32(model.userid, 0));

            #region 操作充值、回写订单

            //验证订单号、流水号
            OrderPaymentEntity entity = buyOrderManager.GetOrderPaymentByPayCode(model.tradeid);
            if (entity == null)
            {
                LogHelper.WriteInfo(typeof(PayBll), string.Format("PayCallBackRetunResult支付回调操作失败信息：订单不存在-----{0}", JsonHelper.ToJson(model)));
                //该订单不存在，记录日志
                return false;
            }
            if (entity.UserId.ToString().Trim() != model.userid.Trim() || entity.PayCode != model.tradeid)
            {
                LogHelper.WriteInfo(typeof(PayBll), string.Format("PayCallBackRetunResult支付回调操作失败信息：UserId、Tradeid跟订单信息不匹配----{0}", JsonHelper.ToJson(model)));
                //信息有误，记录日志
                return false;
            }
            if (model.success == "1")
            {
                //调用充值
                if (entity.PayStatus == 1)
                {

                    if (userInfo.SourceType == 10)
                    {
                        #region 调用充值接口



                        //获取订单活力
                        decimal coinCount = orderManager.GetHuoLiByOrderId(model.merorderid);
                   
                        #endregion


                    }


                    //订单回写

                    OrderPaymentEntity orderPaymentEntity = BuildOrderPaymentEntity(model.tradeid, entity, null);

                    isOk = NomalOrderPay(entity, model.merorderid, model.tradeid, "", orderPaymentEntity, 1);
                    if (!isOk)
                    {
                        LogHelper.WriteInfo(typeof(PayBll), string.Format("PayCallBackRetunResult支付回调操作失败信息：充值成功，但订单状态更新失败！-----{0}", JsonHelper.ToJson(model)));
                        return false;
                    }
                    else
                    {
                        LogHelper.WriteInfo(typeof(PayBll), string.Format("PayCallBackRetunResult支付回调操作成功！-----{0}", JsonHelper.ToJson(model)));
                    }

                }
            }
            //失败
            else if (model.success == "0")
            {

                //记录日志
                LogHelper.WriteInfo(typeof(PayBll), string.Format("PayCallBackRetunResult支付回调操作失败信息：支付失败！----{0}", JsonHelper.ToJson(model)));
                return false;
            }
            else
            {
                //非法参数
                LogHelper.WriteInfo(typeof(PayBll), string.Format("PayCallBackRetunResult支付回调操作失败信息：data.success非法！-----{0}", JsonHelper.ToJson(model)));
                return false;
            }
            return isOk;
            #endregion
        }
       
    }
}

