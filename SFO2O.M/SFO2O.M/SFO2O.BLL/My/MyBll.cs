using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.BLL.Exceptions;
using SFO2O.BLL.Settle;
using SFO2O.DAL.My;
using SFO2O.Model.Enum;
using SFO2O.Model.Extensions;
using SFO2O.Model.My;
using SFO2O.Model.Team;
using SFO2O.Model.Product;
using SFO2O.Model.Settle;
using SFO2O.Utility.Extensions;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Order;
using SFO2O.DAL.Order;
using SFO2O.References.SFo2oWCF;
using SFO2O.BLL.Team;
using SFO2O.Model.Account;
using SFO2O.Model.Huoli;
using SFO2O.Model.Information;
using SFO2O.BLL.Information;
using SFO2O.BLL.Order;
using SFO2O.BLL.Common;
using SFO2O.Utility;
using SFO2O.Model.Refund;
namespace SFO2O.BLL.My
{
    public class MyBll
    {

        MyDal dal = new MyDal();
        SettleBll settleBll = new SettleBll();
        private readonly OrderProductsDal orderProductsDal = new OrderProductsDal();
        private readonly OrderManager orderManager = new OrderManager();
        private static readonly InformationBll InformationBll = new InformationBll();
        /// <summary>
        /// 我的订单首页列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public IList<MyOrderInfoEntity> GetMyOrderIndex(int userId, int country, int language)
        {
            return dal.GetMyOrderIndex(userId, country, language);
        }
        /// <summary>
        /// 根据订单状态获取分页列表数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="country"></param>
        /// <param name="language"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public IList<MyOrderInfoDto> GetMyOrderListByStatus(int userId, int country, int language, int pageIndex, int pageSize, Model.Enum.OrderStatusEnum status)
        {

            return dal.GetMyOrderProductInfos(userId, country, language, pageIndex, pageSize, status).AsDtos().OrderByDescending(n => n.CreateTime).ToList();
        }
        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="country"></param>
        /// <param name="language"></param>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public MyOrderInfoDto GetMyOrderInfo(int userId, int country, int language, string orderCode)
        {
            return dal.GetMyOrderInfo(userId, country, language, orderCode).AsDto();
        }
        //通过父订单 查询子订单信息
        public IList<MyOrderSkuInfoEntity> GetMyOrderInfoByParentOrderCode(string ParentOrderCode)
        {
            return dal.GetMyOrderInfoByParentOrderCode(ParentOrderCode);
        }
        //查询申诉的订单
        public IList<RefundInfoModel> GetMyOrderInfoAndRefund(string orderCode)
        {
            return dal.GetMyOrderInfoAndRefund(orderCode);
        }
        /// <summary>
        /// 获取物流信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="country"></param>
        /// <param name="language"></param>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public MyOrderInfoDto GetMyOrderLogisticsInfo(int userId, int country, int language, string orderCode)
        {
            return dal.GetMyOrderInfo(userId, country, language, orderCode).AsDto();
        }
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="country"></param>
        /// <param name="language"></param>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public bool CancelOrder(int userId, int country, int language, string orderCode)
        {
            //LogHelper.Info("--------CancelOrder---1----进入方法:");
            var model = dal.GetMyOrderInfo(userId, country, language, orderCode).AsDto();
            //LogHelper.Info("--------CancelOrder---1----GetMyOrderInfo结束:model=" + model);

            if (model.OrderStatus == OrderStatusEnum.NonPayment.As(0))
            {
                //LogHelper.Info("--------CancelOrder---1----model.OrderStatus:=" + model.OrderStatus);
                var orderProductList = orderProductsDal.GetOrderProductsByCode(orderCode);
                List<StockEntity> list = new List<StockEntity>();

                foreach (var item in orderProductList)
                {
                    list.Add(new StockEntity
                    {
                        Sku = item.Sku,
                        Spu = item.Spu,
                        Qty = item.Quantity
                    });
                }

                bool IsUpdateGiftStatus = false;
                var OrderStatusList = new OrderInfoDal().GetOrderInfoStatus(orderCode);
                if (OrderStatusList.Count == 0 || (OrderStatusList.Where(d => d.OrderStatus == 5).Count() == OrderStatusList.Count-1))
                {
                    IsUpdateGiftStatus = true;
                }

                //LogHelper.Info("--------CancelOrder---1----取消订单开始");
                return dal.CancelOrder(orderCode, list, userId, model.HuoLi, model.CouponId, IsUpdateGiftStatus);
            }
            else
            {
                throw new SFO2OException("订单状态已经改变，无法取消订单");
            }
        }
        /// <summary>
        /// 确认订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="country"></param>
        /// <param name="language"></param>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public bool ConfirmOrder(int userId, int country, int language, string orderCode)
        {
            var model = dal.GetMyOrderInfo(userId, country, language, orderCode).AsDto();
            string startTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinStartTime"].ToString();
            string endTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinEndTime"].ToString();

            string PinHuoLiStartTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinHuoLiStartTime"].ToString();
            string PinHuoLiEndTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinHuoLiEndTime"].ToString();

            string LiveTime = System.Web.Configuration.WebConfigurationManager.AppSettings["HuoLiLiveTime"].ToString();

            bool istrue = dal.getOrderInfoCount(model.UserId, startTime, endTime);
            if (model.OrderStatus == OrderStatusEnum.Shipped.As(0))
            {
                var isOk = dal.ConfirmOrder(orderCode);
                //查询userId 的酒豆值
                MyHL myHL = new MyHL();
                myHL = dal.getMyHL(model.UserId);
                if (myHL == null)
                {
                    dal.insertHuoLiTotal(model.UserId);
                }
                myHL = dal.getMyHL(model.UserId);
                //增加赠送酒豆的判断  如果为团长就增加酒豆
                //if (!string.IsNullOrEmpty(model.TeamCode))
                //{
                //    TeamInfoEntity teamInfo = new TeamInfoEntity();
                //    teamInfo = new TeamBll().GetTeamInfoByTeamCode(model.TeamCode);
                //    //增加判断 是否实在酒豆有效期内 Convert.ToDateTime(startTime)
                //    bool isInHuoLi = teamInfo.StartTime >= Convert.ToDateTime(PinHuoLiStartTime) && teamInfo.StartTime <= Convert.ToDateTime(PinHuoLiEndTime);
                //    if (teamInfo.UserID == model.UserId && !istrue && isInHuoLi)
                //    {
                //        //update 
                //        var isUpdate = dal.updateHuoLiTotal(model.ProductTotalAmount * 100, model.UserId);
                //        //insert into HuoLidetail 表
                //        OrderInfoDal orderdal=new OrderInfoDal();
                //        HuoliDetailEntity huoliDetail=new HuoliDetailEntity();
                //        huoliDetail.UserId = model.UserId;
                //        huoliDetail.AddHuoLi = model.ProductTotalAmount * 100;
                //        huoliDetail.DeadLine = Convert.ToDateTime(LiveTime);
                //        orderdal.insertIntoHuoliDetail(huoliDetail); 

                //        MyHL InsertMyHL=dal.getMyHL(model.UserId);
                //        myHL.userId = model.UserId;
                //        myHL.Direction = 1;
                //        myHL.OriginalHuoLi = InsertMyHL.countHL;
                //        myHL.ChangedHuoLi = model.ProductTotalAmount * 100;
                //        myHL.CurrentHuoLi = InsertMyHL.usableHL;
                //        myHL.Description = "拼生活奖励";
                //        myHL.TradeCode = model.OrderCode;
                //        myHL.addTime = DateTime.Now;
                //        var isTrue = dal.insertHuoliLog(myHL);

                //        LogHelper.Info("--------ConfirmOrder团长获得酒豆----获取订单的商品图片开始---model.OrderCode：" + model.OrderCode);
                //        ProductInfoModel productInfoModel = orderManager.GetOrderImage(model.OrderCode);
                //        LogHelper.Info("--------ConfirmOrder团长获得酒豆----获取订单的商品图片结束---model.OrderCode：" + model.OrderCode);

                //        LogHelper.Info("--------ConfirmOrder团长获得酒豆----设置对象参数开始");
                //        InformationEntity InformationEntity = new InformationEntity();
                //        InformationEntity.InfoType = 1;
                //        InformationEntity.WebInnerType = 4;
                //        InformationEntity.SendDest = CommonBll.GetUserRegion(userId);
                //        InformationEntity.SendUserId = model.UserId;
                //        InformationEntity.TradeCode = model.TeamCode;
                //        InformationEntity.Title = InformationUtils.HuoliTransferedSuccTitle;
                //        InformationEntity.InfoContent = InformationUtils.HuoliTransferedSuccContent_Prefix 
                //                                + Decimal.Parse((model.ProductTotalAmount * 100).ToString("0"))
                //                                + InformationUtils.HuoliTransferedSuccContent_suffix;

                //        if (productInfoModel != null)
                //        {
                //            InformationEntity.ImagePath = productInfoModel.ImagePath;
                //        }
                //        else
                //        {
                //            InformationEntity.ImagePath = null;
                //        }

                //        InformationEntity.Summary = null;
                //        InformationEntity.LinkUrl = "my/myHL";
                //        InformationEntity.StartTime = null;
                //        InformationEntity.EndTime = null;
                //        InformationEntity.LongTerm = 0;
                //        InformationEntity.CreateTime = DateTime.Now;
                //        LogHelper.Info("--------ConfirmOrder团长获得酒豆----设置对象参数结束");

                //        LogHelper.Info("--------ConfirmOrder团长获得酒豆----执行消息插入方法开始");
                //        InformationBll.AddInformation(InformationEntity);
                //        LogHelper.Info("--------ConfirmOrder团长获得酒豆----执行消息插入方法结束");
                //    }
                //}
                if (isOk)
                {
                    try
                    {

                        bool isFinish = GenerateSettleForOrder(userId, language, orderCode);

                        if (isFinish)
                        {
                            return true;
                        }
                        else
                        {
                            LogHelper.Error(string.Format("确认订单生成结算单失败！orderCode:{0}", orderCode));

                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(string.Format("确认订单生成结算单失败！orderCode:{0}", orderCode), ex);
                    }

                }
                return isOk;
            }
            else
            {
                LogHelper.Error(string.Format("订单状态已经改变，无法确认订单！orderCode:{0},status:{1}", orderCode, model.OrderStatus));
                throw new SFO2OException("订单状态已经改变，无法确认订单,请联系客服人员");
            }
        }
        /// <summary>
        /// 获取物流信息
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public List<ExpressInfoEntity> GetOrderLogistics(string orderCode)
        {
            List<ExpressInfoEntity> list = new List<ExpressInfoEntity>();
            try
            {
                using (GetSFDataClient client = new GetSFDataClient())
                {
                    string expressCode = dal.GetExpressCodeByOrderCode(orderCode);
                    if (string.IsNullOrEmpty(expressCode))
                    {
                        return list;
                    }
                    list = client.GetExpressInfo(expressCode);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return list;
        }
        /// <summary>
        /// 生成订单结算单
        /// </summary>
        /// <param name="userId"></param> 
        /// <param name="language"></param>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public bool GenerateSettleForOrder(int userId, int language, string orderCode)
        {
            var skus = dal.GetOrderSkuEntities(userId, language, orderCode);

            if (skus == null || !skus.Any() || skus.FirstOrDefault().OrderStatus != OrderStatusEnum.Complete.As(0))
            {
                LogHelper.Error(string.Format("未能生产结算单 orderCode:{0}", orderCode));
                throw new SFO2OException("无效订单！");
            }
            SettleOrderInfoEntity settle = null;

            List<SettleProductEntity> productList = null;

            var settles = new Dictionary<SettleOrderInfoEntity, List<SettleProductEntity>>();

            int index = 1;
            foreach (var m in skus.GroupBy(n => n.SupplierId))
            {
                var settlementCode = settleBll.BuildSettleCode(orderCode, index);
                index++;
                var order = m.FirstOrDefault();
                if (order == null)
                {
                    LogHelper.Error(string.Format("订单结算单生产失败：订单信息错误！{0}", orderCode));
                    throw new SFO2OException("订单信息错误！");
                }
                var now = DateTime.Now;
                var creater = "webuser(确认订单)";
                productList = new List<SettleProductEntity>();

                settle = new SettleOrderInfoEntity();



                foreach (var sku in m)
                {
                    var product = new SettleProductEntity();
                    product.SettlementCode = settlementCode;
                    product.Spu = sku.Spu;
                    product.Sku = sku.Sku;
                    product.Quantity = sku.Quantity - sku.RefundQuantity;//有效商品数量
                    if (sku.Quantity - sku.RefundQuantity == 0)
                    {
                        continue;
                    }
                    product.UnitPrice = sku.UnitPrice;
                    product.RmbUnitPrice = sku.PayUnitPrice;
                    product.TaxRate = sku.TaxRate;
                    product.RmbAmount = (sku.Quantity - sku.RefundQuantity) * sku.PayUnitPrice;//有效商品数量×人民币单价
                    product.Amount = (sku.Quantity - sku.RefundQuantity) * sku.UnitPrice;//有效商品数量×港币单价

                    //税金（人民币）
                    product.RmbTaxAmount = sku.IsBearDuty == 1 ? (sku.Quantity - sku.RefundQuantity) * sku.PayUnitPrice * sku.TaxRate / 100M : 0;//有效商品【商家承担】×港币单价*税率 :0
                    //税金
                    product.TaxAmount = sku.IsBearDuty == 1 ? (sku.Quantity - sku.RefundQuantity) * sku.UnitPrice * sku.TaxRate / 100M : 0;//有效商品【商家承担】×港币单价*税率 :0

                    //结算金额（人民币）
                    product.RmbSettlementAmount = product.RmbAmount * (1 - sku.Commission / 100M) - product.RmbTaxAmount;
                    //结算金额
                    product.SettlementAmount = product.Amount * (1 - sku.Commission / 100M) - product.TaxAmount;
                    //有促销费用承担
                    if (sku.PromotionId > 0)
                    {
                        product.RmbSettlementAmount += (sku.Quantity - sku.RefundQuantity) * (sku.OriginalRMBPrice - product.RmbUnitPrice) * (1 - (sku.PromotionCost / 100M)) * (1 - (sku.Commission / 100M));
                        product.SettlementAmount += (sku.Quantity - sku.RefundQuantity) * (sku.OriginalPrice - product.UnitPrice) * (1 - (sku.PromotionCost / 100M)) * (1 - (sku.Commission / 100M));
                    }
                    product.Commission = sku.Commission;
                    product.IsBearDuty = sku.IsBearDuty;
                    productList.Add(product);//添加到临时对象
                }

                settle.SettlementCode = settlementCode; //结算单号
                settle.OrderCode = orderCode; //订单编号
                settle.SupplierId = m.Key; //商家id
                settle.SettlementStatus = 2; //结算单状态：1：待确认 2：待付款 3：付款完成
                settle.SettlementType = 1; // 结算单类型：1订单，2退款退货，3仅退款
                settle.ExchangeRate = order.ExchangeRate; // 汇率

                // 商品总金额(人民币)
                settle.RmbProductTotalAmount = m.Sum(n => (n.Quantity - n.RefundQuantity) * n.PayUnitPrice);//有效商品×人民币单价
                // 商品总金额
                settle.ProductTotalAmount = m.Sum(n => (n.Quantity - n.RefundQuantity) * n.UnitPrice);//有效商品×港币单价

                //商家承担商品税总金额（人民币）
                settle.RmbSupplierBearDutyAmount = m.Where(n => n.IsBearDuty == 1).Sum(n => (n.Quantity - n.RefundQuantity) * n.PayUnitPrice * (n.TaxRate / 100));//有效商品【商家承担】×人民币单价*税率
                //商家承担商品税总金额
                settle.SupplierBearDutyAmount = m.Where(n => n.IsBearDuty == 1).Sum(n => (n.Quantity - n.RefundQuantity) * n.UnitPrice * (n.TaxRate / 100));//有效商品【商家承担】×港币单价*税率

                // 结算总金额（人民币）
                settle.RmbSettlementAmount = productList.Sum(n => n.RmbSettlementAmount);//商品总金额+商家承担商品税总金额（人民币）
                // 结算总金额
                settle.SettlementAmount = productList.Sum(n => n.SettlementAmount);//商品总金额+商家承担商品税总金额

                //平台承担商品税总金额
                settle.RmbBearDutyAmount = 0; //m.Where(n => n.IsBearDuty != 1).Sum(n => (n.Quantity - n.RefundQuantity) * n.PayUnitPrice * n.TaxRate);//有效商品【非商家承担】×港币单价*税率
                settle.BearDutyAmount = 0;//m.Where(n => n.IsBearDuty != 1).Sum(n => (n.Quantity - n.RefundQuantity) * n.UnitPrice * n.TaxRate);//有效商品【非商家承担】×人民币单价*税率

                settle.CreateTime = now;
                settle.CreateBy = creater;
                settle.RmbOtherAmount = 0M;
                settle.OtherAmount = -0M;
                settles.Add(settle, productList);//添加到参数
            }
            return settleBll.AddSettleBatch(settles);


        }
        /// <summary>
        /// 获得我的酒豆
        /// </summary>
        /// <param name="userId"></param> 
        /// <returns></returns>
        public MyHL getMyHL(int userId)
        {
            return dal.getMyHL(userId);
        }
        /// <summary>
        /// 我的酒豆详情
        /// </summary>
        public List<MyHL> HLDetail(int userId, int type, int PageSize, int PageIndex)
        {
            return dal.HLDetail(userId, type, PageSize, PageIndex);
        }
        public bool updateIsPush(int userId, int type)
        {
            return dal.updateIsPush(userId, type);
        }
        public CustomerEntity getUserInfo(int userId)
        {
            return dal.getUserInfo(userId);
        }
        public bool getOrderInfoCount(int UserID,string startTime,string endTime)
        {
            return dal.getOrderInfoCount(UserID, startTime, endTime);
        }
        public bool updateActivityInfoVisible(int userId, int type, DateTime currentTime)
        {
            return dal.updateActivityInfoVisible(userId, type, currentTime);
        }
    }
}
