using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SFO2O.BLL.Exceptions;
using SFO2O.DAL.Order;
using SFO2O.Model.Order;
using SFO2O.M.ViewModel;
using SFO2O.M.ViewModel.Order;
using SFO2O.Model.Product;
using SFO2O.BLL.Product;
using SFO2O.Utility;
using SFO2O.DAL.Shopping;
using SFO2O.M.ViewModel.ShoppingCart;
using SFO2O.Model.Shopping;
using SFO2O.Utility.Uitl;
using SFO2O.BLL.Shopping;
using SFO2O.BLL.Account;
using SFO2O.Model.Team;
using SFO2O.BLL.Team;
using SFO2O.BLL.Item;
using SFO2O.BLL.Source;
using SFO2O.BLL.GiftCard;
using SFO2O.Model.GiftCard;
using SFO2O.Model.Enum;

namespace SFO2O.BLL.Order
{
    public class BuyOrderManager : OrderManager
    {
        private readonly ProductBll productBll = new ProductBll();
        private readonly ShoppingCartDal shoppingCartDal = new ShoppingCartDal();
        private readonly OrderInfoLogDal orderInfoLogDal = new OrderInfoLogDal();
        private readonly OrderProductsDal orderProductsDal = new OrderProductsDal();
        private readonly OrderPaymentDal orderPaymentDal = new OrderPaymentDal();
        private AccountBll accountBll = new AccountBll();
        internal ShoppingBll CartBll = new ShoppingBll();
        private TeamBll teamBll = new TeamBll();
        private ItemBll itemBll = new ItemBll();
        private SourceBll sourceBll = new SourceBll();
        private GiftCardBll giftCardBll = new GiftCardBll();

        #region

        #endregion


        /// <summary>
        /// 获取待付款的订单信息
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OrderInfoEntity GetOrderInfoByCode(string orderCode, int userId)
        {
            return this.orderInfoDal.GetOrderInfoByCode(orderCode, userId);
        }
        /// <summary>
        /// 查询最后一单是否是支付宝支付
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OrderInfoEntity isAliPay(int userId)
        {
            return this.orderInfoDal.isAliPay(userId);
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OrderInfoEntity GetOrderInfoByCodeId(string orderCode, int userId)
        {
            return this.orderInfoDal.GetOrderInfoByCodeId(orderCode, userId);
        }

        /// <summary>
        /// 验证待付款订单的税率是否变化
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public bool PayOrderValidation(string orderCode, int userId)
        {
            bool flag = false;
            ProductSkuEntity skuInfo = new ProductSkuEntity();
            var productList = orderProductsDal.productList(orderCode);
            ProductBll productBll = new Product.ProductBll();
            foreach (var productInfo in productList)
            {
                skuInfo = productBll.GetProductBySku(productInfo.Sku, 1);
                if (productInfo.ConsumerTaxRate != skuInfo.ConsumerTaxRate || productInfo.VATTaxRate != skuInfo.VATTaxRate || productInfo.PPATaxRate != skuInfo.PPATaxRate || productInfo.CBEBTaxRate != skuInfo.CBEBTaxRate)
                {
                    flag = true;
                    break;
                }
            }
            //if (flag)
            //{
            //    var entity = this.orderInfoDal.GetOrderInfoByCode(orderCode, userId);
            //    var orderProductList = orderProductsDal.GetOrderProductsByCode(orderCode);
            //    decimal CustomsDuties = 0, mCustomsDuties = 0;
            //    foreach (var item in orderProductList)
            //    {
            //        CustomsDuties += StringUtils.ToAmt(item.UnitPrice * item.NewTaxRate * item.Quantity * entity.ExchangeRate);
            //        if (item.IsBearDuty == 1)
            //        {
            //            mCustomsDuties += StringUtils.ToAmt(item.UnitPrice * item.NewTaxRate * item.Quantity * entity.ExchangeRate);
            //        }
            //    }
            //}
            return flag;
        }


        public void OrderPayOk(string orderCode, decimal paidAmount, string userName)
        {
            try
            {
                var entity = this.orderInfoDal.GetOrderInfoByCodeAndStatus(orderCode);
                var orderProductList = orderProductsDal.GetOrderProductsByCode(orderCode);
                //LogHelper.Error("--------OrderPayAfter----1.7-----两个查询已经完成：");
                List<StockEntity> list = new List<StockEntity>();
                foreach (var item in orderProductList)
                {
                    //查看当前库存
                    var StockEntity = new StockDal().getStockInfo(item.Spu, item.Sku);
                    item.SFQty = item.Quantity;
                    item.MQty = 0;
                    //如果当前下单库存大于stock Qty的话  就从商家拿
                    if (item.Quantity > StockEntity.Qty)
                    {
                            item.SFQty =StockEntity.Qty;
                            item.MQty = item.Quantity - StockEntity.Qty;
                    }
                    list.Add(new StockEntity
                    {
                        Sku = item.Sku,
                        Spu = item.Spu,
                        Qty = item.SFQty,
                        SQty = item.MQty
                    });
                    orderProductsDal.UpdateOrderProduct(item);
                }
                //LogHelper.Error("--------OrderPayAfter----1.8-----库存集合设置已经完成：");
                entity.PaidAmount = paidAmount;
                entity.PayTime = DateTime.Now;
                //if (entity.TotalAmount - (entity.PaidAmount + entity.Huoli + entity.Coupon) != 0)
                //{
                //    LogHelper.Error(string.Format("订单号：{0} 支付金额和订单金额不等,订单金额{1}，支付金额{2}", entity.OrderCode, entity.TotalAmount, paidAmount));
                //}

                //LogHelper.Error("--------OrderPayAfter----1.9-----OrderCode：" + entity.OrderCode);
                //LogHelper.Error("--------OrderPayAfter----1.9-----OrderPayOk处理开始：");
                this.orderInfoDal.OrderPayOk(entity, list);

                OrderInfoLogEntity orderInfoLogEntity = new OrderInfoLogEntity();
                orderInfoLogEntity.OrderCode = entity.OrderCode;
                orderInfoLogEntity.CurrentStatus = entity.OrderStatus;
                orderInfoLogEntity.AfterStatus = 1;
                orderInfoLogEntity.OperateIp = "";
                orderInfoLogEntity.Remark = "支付成功。";
                orderInfoLogEntity.CreateBy = userName;
                orderInfoLogEntity.CreateTime = DateTime.Now;
                orderInfoLogDal.Add(orderInfoLogEntity);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
        }

        public void OrderPayFailure(string orderCode, string userName)
        {
            try
            {
                var entity = this.orderInfoDal.GetOrderInfoByCode(orderCode);

                entity.PayTime = DateTime.Now;
                this.orderInfoDal.UpdateByPay(entity);

                OrderInfoLogEntity orderInfoLogEntity = new OrderInfoLogEntity();
                orderInfoLogEntity.OrderCode = entity.OrderCode;
                orderInfoLogEntity.CurrentStatus = entity.OrderStatus;
                orderInfoLogEntity.AfterStatus = -2;
                orderInfoLogEntity.OperateIp = "";
                orderInfoLogEntity.Remark = "支付成功，订单异常。";
                orderInfoLogEntity.CreateBy = userName;
                orderInfoLogEntity.CreateTime = DateTime.Now;
                orderInfoLogDal.Add(orderInfoLogEntity);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取购物车列表(选择提交订单的)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="salesTerritory"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public IList<ShoppingCartItemEntity> GetOrderProductsByCode(string orderCode, int salesTerritory, int language)
        {
            return orderProductsDal.GetOrderProductsByCode(orderCode, salesTerritory, language);
        }

        /// <summary>
        /// 记录订单申请支付信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddOrderPayment(OrderPaymentEntity entity)
        {
            return orderPaymentDal.Add(entity);
        }

        /// <summary>
        /// 支付成功
        /// </summary>
        public bool UpdatePaysuccess(OrderPaymentEntity entity)
        {
            return orderPaymentDal.UpdatePaysuccess(entity);
        }

        /// <summary>
        /// 通过orderCode 找到最近的一条记录
        /// </summary>
        public OrderPaymentEntity selectOrderCode(string orderCode, int PayPlatform)
        {
            return orderPaymentDal.selectOrderCode(orderCode, PayPlatform);
        }

        /// <summary>
        /// 支付失败
        /// </summary>
        public bool UpdatePayFailure(OrderPaymentEntity entity)
        {
            return orderPaymentDal.UpdatePayFailure(entity);
        }

        /// <summary>
        /// 获取支付信息
        /// </summary>
        /// <param name="payCode"></param>
        /// <returns></returns>
        public OrderPaymentEntity GetOrderPaymentByCode(string payCode)
        {
            return orderPaymentDal.GetOrderPaymentByCode(payCode);
        }

        public OrderPaymentEntity GetOrderPaymentByTradeCode(string tradeCode)
        {
            return orderPaymentDal.GetOrderPaymentByTradeCode(tradeCode);
        }
        public OrderPaymentEntity GetOrderPaymentByPayCode(string tradeCode)
        {
            return orderPaymentDal.GetOrderPaymentByPayCode(tradeCode);
        }
        #region 订单确认

        /// <summary>
        /// 地址验证
        /// </summary>
        /// <returns></returns>
        private ReturnValue AddressValidation()
        {
            ReturnValue returnValue = new ReturnValue();


            return returnValue;
        }
        /// <summary>
        /// 立即购买下单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BuySave(OrderProductInfoModel model, CartViewModel virtualCart, ref string code)
        {
            ReturnValue returnValue = new ReturnValue();
            if (virtualCart == null || virtualCart.Items == null || !virtualCart.Items.Any())
            {
                throw new SFO2OException("提交订单失败:无法获取购买数据！");
            }
            var item = virtualCart.Items.FirstOrDefault();
            if (item == null)
            {
                throw new SFO2OException("提交订单失败:无法获取购买数据！");
            }

            if (item.ForOrderQty < model.Quantity)
            {
                throw new SFO2OException("库存量不足！");
            }

            //--------------------------------------------时令美食相关购买条件验证 开始---------------------------------------------------------------
            // 时令美食购买条件验证
            Dictionary<int, string> ResultMap = productBll.CheckSkuHolidaySpuValidate(
                model.Sku, model.Quantity, string.Empty, model.Language, model.ExchangeRate, true);

            Dictionary<int, string>.KeyCollection types = ResultMap.Keys;

            int HolidaySpuType = -1;
            string HolidaySpuContent = string.Empty;

            foreach (int type in types)
            {
                HolidaySpuType = type;
                HolidaySpuContent = ResultMap[type].ToString();
            }

            // 不符合购买条件
            if (HolidaySpuType == 0)
            {
                throw new SFO2OException(HolidaySpuContent);
            }
            //--------------------------------------------时令美食相关购买条件验证 结束---------------------------------------------------------------

            decimal totalAmount = 0, freight = 0, productTotalAmount = 0, customsDuties = 0;
            decimal exchangeRate = this.commonDal.GetExchangeRateByPattern(model.DeliveryRegion);
            var SalePrice = item.SalePrice * exchangeRate;
            var HKSalePrice = item.SalePrice;
            var promotionsku = itemBll.GetPromotionInfoBySku(item.Sku);
            if (model.pid == 0 && (promotionsku.Count > 0 && promotionsku.FirstOrDefault().PromotionType == 2) && string.IsNullOrEmpty(model.TeamCode))
            {
                SalePrice = item.Price * exchangeRate;
                HKSalePrice = item.Price;
            }
            productTotalAmount = StringUtils.ToAmt(SalePrice) * model.Quantity;
            int realTaxType = Utility.Uitl.TotalTaxHelper.GetRealTaxType(item.ReportStatus, item.IsCrossBorderEBTax, SalePrice);
            var taxPrice = StringUtils.ToAmt(TotalTaxHelper.GetTotalTaxAmount(realTaxType, StringUtils.ToAmt(SalePrice), item.CBEBTaxRate, item.ConsumerTaxRate, item.VATTaxRate, item.PPATaxRate) * model.Quantity);
            customsDuties = taxPrice;
            var gateways = CartBll.GetGatewayBySku(item.Sku);
            List<ProductItem> pros = new List<ProductItem>();
            ProductItem pro = new ProductItem(exchangeRate);
            pro.Sku = item.Sku;
            pro.GatewayCodes = gateways;
            pros.Add(pro);
            //计算港币
            decimal customsDutiesHKD = StringUtils.ToAmt(TotalTaxHelper.GetTotalTaxAmount(realTaxType, HKSalePrice, item.CBEBTaxRate, item.ConsumerTaxRate, item.VATTaxRate, item.PPATaxRate) * model.Quantity);
            if (item.IsDutyOnSeller == 1 || (customsDuties <= 50 && realTaxType == 2))
            {
                customsDuties = 0;
                customsDutiesHKD = 0;
            }
            totalAmount = StringUtils.ToAmt(productTotalAmount + freight + customsDuties);

            if (realTaxType == 2 && model.Quantity > 1 && productTotalAmount > ConfigHelper.OrderLimitValue)
            {
                throw new SFO2OException("单个订单金额不可超过" + ConfigHelper.OrderLimitValue + "元人民币。");
            }
            if (realTaxType == 1 && productTotalAmount > ConfigHelper.ConsolidatedPrice)
            {
                throw new SFO2OException("单个订单金额不可超过" + ConfigHelper.ConsolidatedPrice + "元人民币。");
            }

            string orderCode = this.SetOrderCode();
            code = orderCode;
            DateTime createTime = DateTime.Now;

            List<OrderProductsEntity> list = new List<OrderProductsEntity>();
            OrderProductsEntity orderProductsEntity = new OrderProductsEntity();
            orderProductsEntity.OrderCode = orderCode;
            orderProductsEntity.Spu = item.Spu;
            orderProductsEntity.Sku = item.Sku;
            orderProductsEntity.Quantity = model.Quantity;
            orderProductsEntity.UnitPrice = HKSalePrice;
            orderProductsEntity.PayUnitPrice = StringUtils.ToAmt(SalePrice);
            orderProductsEntity.TaxRate = item.TaxRate * 100;
            orderProductsEntity.PayAmount = orderProductsEntity.PayUnitPrice * model.Quantity;
            orderProductsEntity.TaxAmount = taxPrice;
            orderProductsEntity.IsBearDuty = item.IsDutyOnSeller;
            orderProductsEntity.RefundQuantity = 0;
            orderProductsEntity.VATTaxRate = item.VATTaxRate * 100;
            orderProductsEntity.CBEBTaxRate = item.CBEBTaxRate * 100;
            orderProductsEntity.ConsumerTaxRate = item.ConsumerTaxRate * 100;
            orderProductsEntity.PPATaxRate = item.PPATaxRate * 100;
            orderProductsEntity.Commission = item.CommissionInCHINA;
            orderProductsEntity.PayTaxAmonutRMB = customsDuties;
            orderProductsEntity.PayTaxAmonutHKD = customsDutiesHKD;
            orderProductsEntity.Huoli = item.Huoli * model.Quantity;
            if (item.Promotion != null && item.Promotion.PromotionId > 0)
            {
                orderProductsEntity.PromotionId = item.Promotion.PromotionId;
                orderProductsEntity.OriginalPrice = item.Price;
                orderProductsEntity.OriginalRMBPrice = item.Price * exchangeRate;
            }
            if (model.DeliveryRegion != 1)
            {
                orderProductsEntity.Commission = item.CommissionInHK;
            }
            //如果有优惠券ID，则查出其金额（进数据库检索以免用户在前端篡改id）
            decimal cardValue = 0M;
            if (model.GiftCardId != 0)
            {
                int quanType = Convert.ToInt32(PromotionType.None);
                if (promotionsku.Count > 0)
                {
                    if (promotionsku.FirstOrDefault().PromotionType == 1)
                    {
                        quanType = Convert.ToInt32(PromotionType.Promotion);//1.打折 2.拼团   对应的枚举-->  打折：0x02 拼团0x04
                    }
                    else if (promotionsku.FirstOrDefault().PromotionType == 2)
                    {
                        if (model.pid != 0 || !string.IsNullOrEmpty(model.TeamCode))
                        {
                            quanType = Convert.ToInt32(PromotionType.GroupBuy);
                        }
                    }
                }
                //获取默认优惠券
                //var giftCard = giftCardBll.GetCanUseGiftCardList(model.UserId, orderProductsEntity.PayAmount, quanType).Where(x => x.ID == model.GiftCardId).FirstOrDefault();
                //if (giftCard != null)
                //{
                //    cardValue = (giftCard as GiftCardEntity).CardSum;
                //}
            }
            //orderProductsEntity.GiftCard = cardValue;
            //decimal Huoli = 0;
            //if (model.hasActivity == 1)
            //{
            //    var HuoliEntity = accountBll.GetHuoliEntityByUerId(model.UserId);
            //    Huoli = GetCanUseHuoli(orderProductsEntity.PayAmount, HuoliEntity.HuoLiCurrent, cardValue);
            //    orderProductsEntity.Huoli = Huoli;
            //}

            list.Add(orderProductsEntity);

            OrderInfoEntity orderInfoEntity = new OrderInfoEntity();
            orderInfoEntity.OrderCode = orderCode;
            orderInfoEntity.UserId = model.UserId;
            orderInfoEntity.TotalAmount = totalAmount;
            orderInfoEntity.Freight = freight;
            orderInfoEntity.ProductTotalAmount = productTotalAmount;
            orderInfoEntity.CustomsDuties = customsDuties;
            orderInfoEntity.ExchangeRate = exchangeRate;
            orderInfoEntity.CreateTime = createTime;
            orderInfoEntity.GiftCardId = model.GiftCardId;
            orderInfoEntity.Huoli = list.Sum(n=>n.Huoli);
            //税的类型
            orderInfoEntity.TaxType = realTaxType;
            orderInfoEntity.GatewayCode = GatewayCode(pros);
            orderInfoEntity.TeamCode = model.TeamCode;
            if (!string.IsNullOrEmpty(model.StationSource) && model.ChannelId > 0)
            {
                orderInfoEntity.OrderSourceType = model.StationSourceType;
                orderInfoEntity.OrderSourceValue = model.StationSource;
                orderInfoEntity.DividedPercent = sourceBll.GetSourcePercentById(model.ChannelId).DividedPercent;
                orderInfoEntity.DividedAmount = StringUtils.ToAmt(orderInfoEntity.DividedPercent / 100 * orderInfoEntity.ProductTotalAmount);
            }

            //添加OrderInfo优惠券
            //if (model.GiftCardId != 0)
            //{
            //    orderInfoEntity.Coupon = cardValue;
            //}

            //if (model.hasActivity == 1)
            //{
            //    orderInfoEntity.Huoli = Huoli;
            //}
            this.SetOrderInfo(orderInfoEntity);
            this.SetOrderAddress(orderInfoEntity, model.AddressId, model.DeliveryRegion);

            return CreateOrder(orderInfoEntity, list, model, "BuySave");
        }
        /// <summary>
        /// 购物车下单
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cart"></param>
        /// <param name="code"></param>
        /// <param name="ParentOrderCode"></param>
        /// <param name="one"></param>
        /// <param name="TaxType"></param>
        /// <returns></returns>
        public bool Save(OrderProductInfoModel model, IEnumerable<ProductItem> productItems, ref string code, string ParentOrderCode = "0", int gatewayType = 0)
        {
            ReturnValue returnValue = new ReturnValue();
            if (!productItems.Any())
            {
                throw new SFO2OException("提交订单失败:无法获取购买数据！");
            }

            decimal totalAmount = 0M;
            decimal freight = 0M;
            decimal productTotalAmount = 0M;
            decimal customsDuties = 0M;
            decimal mCustomsDuties = 0M;
            //港币总额
            decimal customsDutiesCountRMB = 0M;

            decimal exchangeRate = this.commonDal.GetExchangeRateByPattern(model.DeliveryRegion);

            List<OrderProductsEntity> list = new List<OrderProductsEntity>();
            string orderCode = this.SetOrderCode();
            code = orderCode;
            DateTime createTime = DateTime.Now;
            var realTaxType = 0;

            foreach (var item in productItems)
            {
                if (item.CartQuantity > item.ForOrderQty)
                {
                    continue;
                }

                OrderProductsEntity orderProductsEntity = new OrderProductsEntity();
                orderProductsEntity.OrderCode = orderCode;
                orderProductsEntity.Spu = item.Spu;
                orderProductsEntity.Sku = item.Sku;
                orderProductsEntity.Quantity = item.CartQuantity;
                orderProductsEntity.UnitPrice = item.SalePrice;//港币销售价（促销价/原价）
                orderProductsEntity.PayUnitPrice = StringUtils.ToAmt(item.SalePrice * exchangeRate);//RMB币销售价（促销价/原价）
                orderProductsEntity.TaxRate = item.TaxRate * 100;
                int realTax = Utility.Uitl.TotalTaxHelper.GetRealTaxType(item.ReportStatus, item.IsCrossBorderEBTax, orderProductsEntity.PayUnitPrice);
                decimal customsDutiesHKD = TotalTaxHelper.GetTotalTaxAmount(realTax, item.SalePrice, item.CBEBTaxRate, item.ConsumerTaxRate, item.VATTaxRate, item.PPATaxRate) * item.CartQuantity;
                orderProductsEntity.PayAmount = orderProductsEntity.PayUnitPrice * item.CartQuantity;
                orderProductsEntity.TaxAmount = item.TotalTaxAmountExchanged;
                orderProductsEntity.IsBearDuty = item.IsDutyOnSeller;
                orderProductsEntity.RefundQuantity = 0;
                orderProductsEntity.Commission = item.CommissionInCHINA;
                orderProductsEntity.PayTaxAmonutRMB = item.IsDutyOnSeller == 1 ? 0 : item.TotalTaxAmountExchanged;
                customsDutiesCountRMB += orderProductsEntity.PayTaxAmonutRMB;
                orderProductsEntity.PayTaxAmonutHKD = item.IsDutyOnSeller == 1 ? 0 : customsDutiesHKD;
                orderProductsEntity.VATTaxRate = item.VATTaxRate * 100;
                orderProductsEntity.CBEBTaxRate = item.CBEBTaxRate * 100;
                orderProductsEntity.PPATaxRate = item.PPATaxRate * 100;
                orderProductsEntity.ConsumerTaxRate = item.ConsumerTaxRate * 100;
                orderProductsEntity.Commission = item.CommissionInCHINA;
                orderProductsEntity.GiftCard = item.GiftCard;
                orderProductsEntity.Huoli = item.Huoli == null ? 0 : item.Huoli;
                realTaxType = item.RealTaxType;
                if (item.Promotion != null && item.Promotion.PromotionId > 0)
                {
                    orderProductsEntity.PromotionId = item.Promotion.PromotionId;
                    orderProductsEntity.OriginalPrice = item.Price;
                    orderProductsEntity.OriginalRMBPrice = item.Price * exchangeRate;
                }

                if (model.DeliveryRegion != 1)
                {
                    orderProductsEntity.Commission = item.CommissionInHK;
                }
                list.Add(orderProductsEntity);

                productTotalAmount += orderProductsEntity.PayAmount;
                //计算商品税
                customsDuties += orderProductsEntity.TaxAmount;
                if (item.IsDutyOnSeller == 1)
                {
                    mCustomsDuties += orderProductsEntity.TaxAmount;
                }
            }
            if (customsDuties <= 50 && productItems.Where(d => d.RealTaxType == 2).Count() > 0)
            {
                customsDuties = 0;
            }
            else
            {
                customsDuties = customsDuties - mCustomsDuties;
            }
            if (customsDuties == 0)
            {
                foreach (var li in list)
                {
                    li.PayTaxAmonutHKD = 0;
                    li.PayTaxAmonutRMB = 0;
                }
            }
            totalAmount = StringUtils.ToAmt(productTotalAmount + freight + customsDuties);

            if (realTaxType == 2 && model.Quantity > 1 && productTotalAmount > ConfigHelper.OrderLimitValue)
            {
                throw new SFO2OException("单个订单金额不可超过" + ConfigHelper.OrderLimitValue + "元人民币。");
            }
            if (realTaxType == 1 && productTotalAmount > ConfigHelper.ConsolidatedPrice)
            {
                throw new SFO2OException("单个订单金额不可超过" + ConfigHelper.ConsolidatedPrice + "元人民币。");
            }

            OrderInfoEntity orderInfoEntity = new OrderInfoEntity();
            orderInfoEntity.OrderCode = orderCode;
            orderInfoEntity.UserId = model.UserId;
            orderInfoEntity.TotalAmount = totalAmount;
            orderInfoEntity.Freight = freight;
            orderInfoEntity.ProductTotalAmount = productTotalAmount;
            orderInfoEntity.CustomsDuties = customsDuties;
            orderInfoEntity.ExchangeRate = exchangeRate;
            orderInfoEntity.CreateTime = createTime;
            orderInfoEntity.GiftCardId = model.GiftCardId;
            if (!string.IsNullOrEmpty(model.StationSource) && model.ChannelId > 0)
            {
                orderInfoEntity.OrderSourceType = model.StationSourceType;
                orderInfoEntity.OrderSourceValue = model.StationSource;

                orderInfoEntity.DividedPercent = sourceBll.GetSourcePercentById(model.ChannelId).DividedPercent;
                orderInfoEntity.DividedAmount = StringUtils.ToAmt(orderInfoEntity.DividedPercent / 100 * orderInfoEntity.ProductTotalAmount);
            }
            //增加一个属性 父的订单号
            orderInfoEntity.ParentOrderCode = ParentOrderCode;
            //税的类型
            orderInfoEntity.TaxType = realTaxType;
            //关口类型
            orderInfoEntity.GatewayCode = gatewayType;
            //添加优惠劵
            orderInfoEntity.Coupon = list.Sum(x => x.GiftCard);
            orderInfoEntity.Huoli = list.Sum(d => d.Huoli*d.Quantity);
            this.SetOrderInfo(orderInfoEntity);
            this.SetOrderAddress(orderInfoEntity, model.AddressId, model.DeliveryRegion);

            bool isOk = CreateOrder(orderInfoEntity, list, model, "Shopping");
            if (isOk)
            {
                shoppingCartDal.DeleteByUserId(model.UserId, model.DeliveryRegion);
            }

            return isOk;

        }


        public bool SaveGift(OrderProductInfoModel model, IEnumerable<ProductItem> productItems, ref string code, int giftType, string ParentOrderCode = "0", int gatewayType = 0)
        {
            ReturnValue returnValue = new ReturnValue();
            if (!productItems.Any())
            {
                throw new SFO2OException("提交订单失败:无法获取购买数据！");
            }

            decimal totalAmount = 0M;
            decimal freight = 0M;
            decimal productTotalAmount = 0M;
            decimal customsDuties = 0M;
            decimal mCustomsDuties = 0M;
            //港币总额
            decimal customsDutiesCountRMB = 0M;

            decimal exchangeRate = this.commonDal.GetExchangeRateByPattern(model.DeliveryRegion);

            List<OrderProductsEntity> list = new List<OrderProductsEntity>();
            string orderCode = this.SetOrderCode();
            code = orderCode;
            DateTime createTime = DateTime.Now;
            var realTaxType = 0;

            foreach (var item in productItems)
            {
                if (item.CartQuantity > item.ForOrderQty)
                {
                    continue;
                }

                OrderProductsEntity orderProductsEntity = new OrderProductsEntity();
                orderProductsEntity.OrderCode = orderCode;
                orderProductsEntity.Spu = item.Spu;
                orderProductsEntity.Sku = item.Sku;
                orderProductsEntity.Quantity = item.CartQuantity;
                orderProductsEntity.UnitPrice = item.SalePrice;//港币销售价（促销价/原价）
                orderProductsEntity.PayUnitPrice = StringUtils.ToAmt(item.SalePrice * exchangeRate);//RMB币销售价（促销价/原价）
                orderProductsEntity.TaxRate = item.TaxRate * 100;
                int realTax = Utility.Uitl.TotalTaxHelper.GetRealTaxType(item.ReportStatus, item.IsCrossBorderEBTax, orderProductsEntity.PayUnitPrice);
                decimal customsDutiesHKD = TotalTaxHelper.GetTotalTaxAmount(realTax, item.SalePrice, item.CBEBTaxRate, item.ConsumerTaxRate, item.VATTaxRate, item.PPATaxRate) * item.CartQuantity;
                orderProductsEntity.PayAmount = orderProductsEntity.PayUnitPrice * item.CartQuantity;
                orderProductsEntity.TaxAmount = item.TotalTaxAmountExchanged;
                orderProductsEntity.IsBearDuty = item.IsDutyOnSeller;
                orderProductsEntity.RefundQuantity = 0;
                orderProductsEntity.Commission = item.CommissionInCHINA;
                orderProductsEntity.PayTaxAmonutRMB = item.IsDutyOnSeller == 1 ? 0 : item.TotalTaxAmountExchanged;
                customsDutiesCountRMB += orderProductsEntity.PayTaxAmonutRMB;
                orderProductsEntity.PayTaxAmonutHKD = item.IsDutyOnSeller == 1 ? 0 : customsDutiesHKD;
                orderProductsEntity.VATTaxRate = item.VATTaxRate * 100;
                orderProductsEntity.CBEBTaxRate = item.CBEBTaxRate * 100;
                orderProductsEntity.PPATaxRate = item.PPATaxRate * 100;
                orderProductsEntity.ConsumerTaxRate = item.ConsumerTaxRate * 100;
                orderProductsEntity.Commission = item.CommissionInCHINA;
                orderProductsEntity.GiftCard = item.GiftCard;
                orderProductsEntity.Huoli = item.Huoli;
                realTaxType = item.RealTaxType;
                if (item.Promotion != null && item.Promotion.PromotionId > 0)
                {
                    orderProductsEntity.PromotionId = item.Promotion.PromotionId;
                    orderProductsEntity.OriginalPrice = item.Price;
                    orderProductsEntity.OriginalRMBPrice = item.Price * exchangeRate;
                }

                if (model.DeliveryRegion != 1)
                {
                    orderProductsEntity.Commission = item.CommissionInHK;
                }
                list.Add(orderProductsEntity);

                productTotalAmount += orderProductsEntity.PayAmount;
                //计算商品税
                customsDuties += orderProductsEntity.TaxAmount;
                if (item.IsDutyOnSeller == 1)
                {
                    mCustomsDuties += orderProductsEntity.TaxAmount;
                }
            }
            if (customsDuties <= 50 && productItems.Where(d => d.RealTaxType == 2).Count() > 0)
            {
                customsDuties = 0;
            }
            else
            {
                customsDuties = customsDuties - mCustomsDuties;
            }
            if (customsDuties == 0)
            {
                foreach (var li in list)
                {
                    li.PayTaxAmonutHKD = 0;
                    li.PayTaxAmonutRMB = 0;
                }
            }
            totalAmount = StringUtils.ToAmt(productTotalAmount + freight + customsDuties);

            if (realTaxType == 2 && model.Quantity > 1 && productTotalAmount > ConfigHelper.OrderLimitValue)
            {
                throw new SFO2OException("单个订单金额不可超过" + ConfigHelper.OrderLimitValue + "元人民币。");
            }
            if (realTaxType == 1 && productTotalAmount > ConfigHelper.ConsolidatedPrice)
            {
                throw new SFO2OException("单个订单金额不可超过" + ConfigHelper.ConsolidatedPrice + "元人民币。");
            }

           decimal totalAmountOne = Convert.ToDecimal(ConfigHelper.GetAppSetting<string>("libao_totalAmountOne"));
            decimal totalAmountTwo = Convert.ToDecimal(ConfigHelper.GetAppSetting<string>("libao_totalAmountTwo"));
            OrderInfoEntity orderInfoEntity = new OrderInfoEntity();
            switch (giftType)
            {
                case 1: orderInfoEntity.TotalAmount = totalAmountOne;
                    orderInfoEntity.ProductTotalAmount = totalAmountOne;
                    orderInfoEntity.Huoli = totalAmountOne;
                    break;

                case 2: orderInfoEntity.TotalAmount = totalAmountOne;
                    orderInfoEntity.ProductTotalAmount = totalAmountOne;
                    orderInfoEntity.Huoli = totalAmountOne;
                    break;
                case 3: orderInfoEntity.TotalAmount = totalAmountTwo;
                    orderInfoEntity.ProductTotalAmount = totalAmountTwo;
                    orderInfoEntity.Huoli = totalAmountTwo;
                    break;
                case 4: orderInfoEntity.TotalAmount = totalAmountTwo;
                    orderInfoEntity.ProductTotalAmount = totalAmountTwo;
                    orderInfoEntity.Huoli = totalAmountTwo;
                    break;
                default:
                    break;
            }
            orderInfoEntity.OrderCode = orderCode;
            orderInfoEntity.UserId = model.UserId;
           
            orderInfoEntity.Freight = freight;
            
            orderInfoEntity.CustomsDuties = customsDuties;
            orderInfoEntity.ExchangeRate = exchangeRate;
            orderInfoEntity.CreateTime = createTime;
            orderInfoEntity.GiftCardId = model.GiftCardId;
            if (!string.IsNullOrEmpty(model.StationSource) && model.ChannelId > 0)
            {
                orderInfoEntity.OrderSourceType = model.StationSourceType;
                orderInfoEntity.OrderSourceValue = model.StationSource;

                orderInfoEntity.DividedPercent = sourceBll.GetSourcePercentById(model.ChannelId).DividedPercent;
                orderInfoEntity.DividedAmount = StringUtils.ToAmt(orderInfoEntity.DividedPercent / 100 * orderInfoEntity.ProductTotalAmount);
            }
            //增加一个属性 父的订单号
            orderInfoEntity.ParentOrderCode = ParentOrderCode;
            //税的类型
            orderInfoEntity.TaxType = realTaxType;
            //关口类型
            orderInfoEntity.GatewayCode = gatewayType;
            //添加优惠劵
            orderInfoEntity.Coupon = list.Sum(x => x.GiftCard);
            
            this.SetOrderInfo(orderInfoEntity);
            this.SetOrderAddress(orderInfoEntity, model.AddressId, model.DeliveryRegion);

            bool isOk = CreateOrder(orderInfoEntity, list, model, "Shopping");
            if (isOk)
            {
                shoppingCartDal.DeleteByUserId(model.UserId, model.DeliveryRegion);
            }

            return isOk;

        }
        /// <summary>
        /// 获取关口
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public static int GatewayCode(IEnumerable<ProductItem> products)
        {
            //判断走哪个关口
            foreach (var pros in products)
            {
                if (pros.GatewayCodes.Count() == 1)
                {
                    return pros.GatewayCodes.FirstOrDefault().Gateway;
                }

            }
            //如果 skucustomreport 表里没数据 返回1   1是广州关口
            if (products.Count() == 0)
            {
                return 1;
            }
            return 3;
        }
        /// <summary>
        /// 获取能够使用的酒豆值
        /// </summary>
        /// <param name="productPrice"></param>
        /// <param name="canUseHuoli"></param>
        /// <returns></returns>
        //public static decimal GetCanUseHuoli(decimal productPrice, decimal canUseHuoli)
        //{
        //    if ((canUseHuoli / 100) >= (productPrice * 0.9M))
        //    {
        //        return Math.Round(productPrice * 0.9M * 100, 0, MidpointRounding.AwayFromZero);                
        //    }
        //    return canUseHuoli;
        //}

        /// <summary>
        /// 获取能够使用的酒豆值
        /// 2016.6.1 重写，可使用的酒豆受到优惠券的影响（商品金额-优惠券）*90%。
        /// </summary>
        /// <param name="productPrice">商品金额</param>
        /// <param name="canUseHuoli">用户当前酒豆</param>
        /// <param name="giftCardValue">优惠券的面值</param>
        /// <returns></returns>
        public static decimal GetCanUseHuoli(decimal productPrice, decimal canUseHuoli, decimal giftCardValue = 0M)
        {
            decimal standardValue = (productPrice - giftCardValue) * 0.9M;
            if ((canUseHuoli / 100) >= standardValue)
            {
                return Math.Round(standardValue * 100, 0, MidpointRounding.AwayFromZero);
            }
            return canUseHuoli;
        }

        /*
        /// <summary>
        /// 购物车下单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Obsolete("未加入促销")]
        public ReturnValue ShoppingCartSubmitOrder(OrderProductInfoModel model)
        {
            ReturnValue returnValue = new ReturnValue();
            var shoppingList = shoppingCartDal.GetShoppingCart(model.UserId, model.DeliveryRegion, model.Language);
            if (shoppingList == null || shoppingList.Count == 0)
            {
                returnValue.HasError = true;
                returnValue.Message = "提交订单失败(获取购物车数据失败)";
                return returnValue;
            }
            decimal TotalAmount = 0, Freight = 0, ProductTotalAmount = 0, CustomsDuties = 0, mCustomsDuties = 0, productCout = 0;
            decimal ExchangeRate = this.commonDal.GetExchangeRateByPattern(model.DeliveryRegion);
            List<OrderProductsEntity> list = new List<OrderProductsEntity>();
            string orderCode = this.SetOrderCode();
            DateTime createTime = DateTime.Now;
            foreach (var item in shoppingList)
            {
                if (item.CartQuantity > item.ForOrderQty)
                {
                    continue;
                }

                OrderProductsEntity orderProductsEntity = new OrderProductsEntity();
                orderProductsEntity.OrderCode = orderCode;
                orderProductsEntity.Spu = item.Spu;
                orderProductsEntity.Sku = item.Sku;
                orderProductsEntity.Quantity = item.CartQuantity;
                orderProductsEntity.UnitPrice = item.Price;
                orderProductsEntity.PayUnitPrice = StringUtils.ToAmt(item.Price * ExchangeRate);
                orderProductsEntity.TaxRate = item.TaxRate * 100;
                orderProductsEntity.PayAmount = StringUtils.ToAmt(item.Price * ExchangeRate) * item.CartQuantity;
                orderProductsEntity.TaxAmount = StringUtils.ToAmt(item.Price * item.TaxRate * ExchangeRate) * item.CartQuantity;
                orderProductsEntity.IsBearDuty = item.IsDutyOnSeller;
                orderProductsEntity.RefundQuantity = 0;
                orderProductsEntity.Commission = item.CommissionInCHINA;
                if (model.DeliveryRegion != 1)
                {
                    orderProductsEntity.Commission = item.CommissionInHK;
                }
                list.Add(orderProductsEntity);

                productCout += item.CartQuantity;
                ProductTotalAmount += orderProductsEntity.PayAmount;
                CustomsDuties += orderProductsEntity.TaxAmount;
                if (item.IsDutyOnSeller == 1)
                {
                    mCustomsDuties += orderProductsEntity.TaxAmount;
                }
            }
            if (CustomsDuties <= 50)
            {
                CustomsDuties = 0;
            }
            else
            {
                CustomsDuties = CustomsDuties - mCustomsDuties;
            }
            TotalAmount = StringUtils.ToAmt(ProductTotalAmount + Freight + CustomsDuties);
            if (model.Quantity > 1 && ProductTotalAmount > model.OrderLimitValue)
            {
                returnValue.HasError = true;
                returnValue.Message = "单个订单金额不可超过" + model.OrderLimitValue + "元人民币。";
                return returnValue;
            }

            OrderInfoEntity orderInfoEntity = new OrderInfoEntity();
            orderInfoEntity.OrderCode = orderCode;
            orderInfoEntity.UserId = model.UserId;
            orderInfoEntity.TotalAmount = TotalAmount;
            orderInfoEntity.Freight = Freight;
            orderInfoEntity.ProductTotalAmount = ProductTotalAmount;
            orderInfoEntity.CustomsDuties = CustomsDuties;
            orderInfoEntity.ExchangeRate = ExchangeRate;
            orderInfoEntity.CreateTime = createTime;
            this.SetOrderInfo(orderInfoEntity);
            this.SetOrderAddress(orderInfoEntity, model.AddressId, model.DeliveryRegion);

            returnValue = CreateOrder(orderInfoEntity, list, model);
            if (!returnValue.HasError)
            {
                shoppingCartDal.DeleteByUserId(model.UserId, model.DeliveryRegion);
            }

            return returnValue;
        }

      

        /// <summary>
        /// 立即购买下单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnValue ImmediatelySubmitOrder(OrderProductInfoModel model)
        {
            ReturnValue returnValue = new ReturnValue();

            var entity = productBll.GetProductOrderBySku(model.Sku, model.Language);
            if (entity.Qty < model.Quantity)
            {
                returnValue.HasError = true;
                returnValue.Message = "库存量不足。";
                return returnValue;
            }
            decimal TotalAmount = 0, Freight = 0, ProductTotalAmount = 0, CustomsDuties = 0;
            decimal ExchangeRate = this.commonDal.GetExchangeRateByPattern(model.DeliveryRegion);
            ProductTotalAmount = StringUtils.ToAmt(entity.ProductPrice * ExchangeRate) * model.Quantity;
            CustomsDuties = StringUtils.ToAmt(entity.ProductPrice * entity.TaxRate * ExchangeRate) * model.Quantity;
            if (entity.IsDutyOnSeller == 1 || CustomsDuties <= 50)
            {
                CustomsDuties = 0;
            }
            TotalAmount = StringUtils.ToAmt(ProductTotalAmount + Freight + CustomsDuties);
            if (model.Quantity > 1 && ProductTotalAmount > model.OrderLimitValue)
            {
                returnValue.HasError = true;
                returnValue.Message = "单个订单金额不可超过" + model.OrderLimitValue + "元人民币。";
                return returnValue;
            }

            string orderCode = this.SetOrderCode();
            DateTime createTime = DateTime.Now;

            List<OrderProductsEntity> list = new List<OrderProductsEntity>();
            OrderProductsEntity orderProductsEntity = new OrderProductsEntity();
            orderProductsEntity.OrderCode = orderCode;
            orderProductsEntity.Spu = entity.Spu;
            orderProductsEntity.Sku = entity.Sku;
            orderProductsEntity.Quantity = model.Quantity;
            orderProductsEntity.UnitPrice = entity.ProductPrice;
            orderProductsEntity.PayUnitPrice = StringUtils.ToAmt(entity.ProductPrice * ExchangeRate);
            orderProductsEntity.TaxRate = entity.TaxRate * 100;
            orderProductsEntity.PayAmount = ProductTotalAmount;
            orderProductsEntity.TaxAmount = StringUtils.ToAmt(entity.ProductPrice * entity.TaxRate * ExchangeRate) * model.Quantity;
            orderProductsEntity.IsBearDuty = entity.IsDutyOnSeller;
            orderProductsEntity.RefundQuantity = 0;
            orderProductsEntity.Commission = entity.CommissionInCHINA;
            if (model.DeliveryRegion != 1)
            {
                orderProductsEntity.Commission = entity.CommissionInHK;
            }
            list.Add(orderProductsEntity);

            OrderInfoEntity orderInfoEntity = new OrderInfoEntity();
            orderInfoEntity.OrderCode = orderCode;
            orderInfoEntity.UserId = model.UserId;
            orderInfoEntity.TotalAmount = TotalAmount;
            orderInfoEntity.Freight = Freight;
            orderInfoEntity.ProductTotalAmount = ProductTotalAmount;
            orderInfoEntity.CustomsDuties = CustomsDuties;
            orderInfoEntity.ExchangeRate = ExchangeRate;
            orderInfoEntity.CreateTime = createTime;
            this.SetOrderInfo(orderInfoEntity);
            this.SetOrderAddress(orderInfoEntity, model.AddressId, model.DeliveryRegion);

            return CreateOrder(orderInfoEntity, list, model);
        }
           */
        /// <summary>
        /// 从新下单===只变更商品税信息，其他信息不变更
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool OrderNewSubmitOrder(string orderCode, int userId)
        {

            var entity = this.orderInfoDal.GetOrderInfoByCode(orderCode, userId);
            var orderProductList = orderProductsDal.GetOrderProductsByCode(orderCode);

            decimal TotalAmount = 0, CustomsDuties = 0, mCustomsDuties = 0;

            List<OrderProductsEntity> list = new List<OrderProductsEntity>();
            DateTime createTime = DateTime.Now;
            foreach (var item in orderProductList)
            {

                OrderProductsEntity orderProductsEntity = new OrderProductsEntity();
                orderProductsEntity.OrderCode = orderCode;
                orderProductsEntity.Spu = item.Spu;
                orderProductsEntity.Sku = item.Sku;
                orderProductsEntity.TaxRate = item.NewTaxRate * 100;
                orderProductsEntity.TaxAmount = StringUtils.ToAmt(item.UnitPrice * item.NewTaxRate * entity.ExchangeRate) * item.Quantity;
                list.Add(orderProductsEntity);

                CustomsDuties += orderProductsEntity.TaxAmount;
                if (item.IsBearDuty == 1)
                {
                    mCustomsDuties += orderProductsEntity.TaxAmount;
                }
            }
            if (CustomsDuties <= 50)
            {
                CustomsDuties = 0;
            }
            else
            {
                CustomsDuties = CustomsDuties - mCustomsDuties;
            }

            TotalAmount = StringUtils.ToAmt(entity.ProductTotalAmount + entity.Freight + CustomsDuties);

            OrderInfoEntity orderInfoEntity = new OrderInfoEntity();
            orderInfoEntity.OrderCode = orderCode;
            orderInfoEntity.UserId = userId;
            orderInfoEntity.TotalAmount = TotalAmount;
            orderInfoEntity.CustomsDuties = CustomsDuties;

            return NewCreateOrder(orderInfoEntity, list);


        }
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="orderInfoEntity"></param>
        /// <param name="list"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool CreateOrder(OrderInfoEntity orderInfoEntity, List<OrderProductsEntity> list, OrderProductInfoModel model, string souce)
        {
            try
            {
                TeamInfoEntity TeamInfo = new TeamInfoEntity();
                TeamInfo.TeamCode = "";
                var insertTeam = false;
                if (model.pid > 0)
                {
                    insertTeam = true;
                    var code = orderInfoEntity.OrderCode.Replace("S", "P").Trim();
                    orderInfoEntity.TeamCode = code;

                    TeamInfo.TeamCode = code;
                    TeamInfo.Sku = list.FirstOrDefault().Sku;
                    TeamInfo.TeamStatus = 0;
                    TeamInfo.PromotionId = model.pid;
                    TeamInfo.UserID = model.UserId;
                    TeamInfo.TeamNumbers = itemBll.GetPromotionInfoByPid(model.pid).FirstOrDefault().TuanNumbers;
                }

                this.orderInfoDal.CreateOrder(orderInfoEntity, list, insertTeam, TeamInfo);
                SFO2O.Utility.Uitl.PerformanceTracer.InvokeAsync(() =>
                {
                    OrderInfoLogEntity entity = new OrderInfoLogEntity();
                    entity.OrderCode = orderInfoEntity.OrderCode;
                    entity.CurrentStatus = orderInfoEntity.OrderStatus;
                    entity.AfterStatus = orderInfoEntity.OrderStatus;
                    entity.OperateIp = model.strIp;
                    entity.Remark = "用户创建订单" + souce;
                    entity.CreateBy = model.UserId.ToString();
                    entity.CreateTime = orderInfoEntity.CreateTime;
                    orderInfoLogDal.Add(entity);
                });
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                return false;
            }
        }


        ///// <summary>
        ///// 创建订单
        ///// </summary>
        ///// <param name="orderInfoEntity"></param>
        ///// <param name="list"></param>
        ///// <returns></returns>
        //[Obsolete("无促销信息")]
        //private bool CreateOrder(OrderInfoEntity orderInfoEntity, List<OrderProductsEntity> list, OrderProductInfoModel model)
        //{
        //    ReturnValue returnValue = new ReturnValue();
        //    try
        //    {
        //        this.orderInfoDal.CreateOrder(orderInfoEntity, list);
        //        OrderInfoLogEntity entity = new OrderInfoLogEntity();
        //        entity.OrderCode = orderInfoEntity.OrderCode;
        //        entity.CurrentStatus = orderInfoEntity.OrderStatus;
        //        entity.AfterStatus = orderInfoEntity.OrderStatus;
        //        entity.OperateIp = model.strIp;
        //        entity.Remark = "用户创建订单";
        //        entity.CreateBy = model.UserId.ToString();
        //        entity.CreateTime = orderInfoEntity.CreateTime;
        //        orderInfoLogDal.Add(entity);
        //        returnValue.HasError = false;
        //        returnValue.Message = "";
        //        returnValue.ReturnString = orderInfoEntity.OrderCode;
        //    }
        //    catch (Exception ex)
        //    {
        //        returnValue.HasError = true;
        //        returnValue.Message = "提交订单失败。";

        //        LogHelper.Error(ex.Message);
        //    }
        //    return false;
        //}

        /// <summary>
        /// 从新下单
        /// </summary>
        /// <param name="orderInfoEntity"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool NewCreateOrder(OrderInfoEntity orderInfoEntity, List<OrderProductsEntity> list)
        {
            ReturnValue returnValue = new ReturnValue();
            try
            {
                this.orderInfoDal.NewCreateOrder(orderInfoEntity, list);
                OrderInfoLogEntity entity = new OrderInfoLogEntity();
                entity.OrderCode = orderInfoEntity.OrderCode;
                entity.CurrentStatus = orderInfoEntity.OrderStatus;
                entity.AfterStatus = orderInfoEntity.OrderStatus;
                entity.OperateIp = "";
                entity.Remark = "从新创建订单";
                entity.CreateBy = "";
                entity.CreateTime = orderInfoEntity.CreateTime;
                orderInfoLogDal.Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                return false;
            }

        }
        #endregion

        /// <summary>
        /// 团订单更新库存和订单信息表
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="paidAmount"></param>
        /// <param name="userName"></param>
        /// <param name="OrderStatus"></param>
        public bool TeamOrderPayOK(string orderCode, decimal paidAmount, string userName, int OrderStatus, TeamInfoEntity teamInfoEntity)
        {
            try
            {
                var entity = this.orderInfoDal.GetOrderInfoByCode(orderCode);
                var orderProductList = orderProductsDal.GetOrderProductsByCode(orderCode);
                List<StockEntity> list = new List<StockEntity>();
                foreach (var item in orderProductList)
                {
                    list.Add(new StockEntity
                    {
                        Sku = item.Sku,
                        Spu = item.Spu,
                        Qty = item.Quantity - item.MQty,
                        SQty = item.MQty
                    });
                }

                entity.PaidAmount = paidAmount;
                entity.PayTime = DateTime.Now;
                entity.OrderStatus = OrderStatus;

                if (entity.TotalAmount - entity.PaidAmount != 0)
                {
                    LogHelper.Error(string.Format("订单号：{0} 支付金额和订单金额不等,订单金额{1}，支付金额{2}", entity.OrderCode, entity.TotalAmount, paidAmount));
                    return false;
                }

                //LogHelper.Info("--------TeamPayAfter----6-----" + entity.OrderStatus);
                this.orderInfoDal.TeamOrderPayOk(entity, list, teamInfoEntity);

                OrderInfoLogEntity orderInfoLogEntity = new OrderInfoLogEntity();
                orderInfoLogEntity.OrderCode = entity.OrderCode;
                orderInfoLogEntity.CurrentStatus = entity.OrderStatus;
                orderInfoLogEntity.AfterStatus = 1;
                orderInfoLogEntity.OperateIp = "";
                orderInfoLogEntity.Remark = "支付成功。";
                orderInfoLogEntity.CreateBy = userName;
                orderInfoLogEntity.CreateTime = DateTime.Now;
                orderInfoLogDal.Add(orderInfoLogEntity);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                return false;
            }
        }

        public void TeamOrderPayOKForStatus(string orderCode, decimal paidAmount, string userName, int OrderStatus, TeamInfoEntity teamInfoEntity)
        {
            try
            {
                //LogHelper.Info("--------TeamPayAfter----4.1.2-----开始" + orderCode);
                var entity = this.orderInfoDal.GetTeamOrderInfoByCode(orderCode);
                //LogHelper.Info("--------TeamPayAfter----4.1.2-----结束" + entity + orderCode);

                entity.PaidAmount = paidAmount;
                entity.PayTime = DateTime.Now;
                entity.OrderStatus = OrderStatus;

                //LogHelper.Info("--------TeamPayAfter----4.1.3-----金额判断" + (entity.TotalAmount - entity.PaidAmount) + orderCode);
                if (entity.TotalAmount - entity.PaidAmount != 0)
                {
                    LogHelper.Error(string.Format("订单号：{0} 支付金额和订单金额不等,订单金额{1}，支付金额{2}", entity.OrderCode, entity.TotalAmount, paidAmount));
                }

                //LogHelper.Info("--------TeamPayAfter----4.1.4-----开始" + orderCode);
                this.orderInfoDal.TeamOrderPayOkForStatus(entity, teamInfoEntity);
                //LogHelper.Info("--------TeamPayAfter----4.1.4-----结束" + orderCode);

                OrderInfoLogEntity orderInfoLogEntity = new OrderInfoLogEntity();
                orderInfoLogEntity.OrderCode = entity.OrderCode;
                orderInfoLogEntity.CurrentStatus = entity.OrderStatus;
                orderInfoLogEntity.AfterStatus = 1;
                orderInfoLogEntity.OperateIp = "";
                orderInfoLogEntity.Remark = "支付成功。";
                orderInfoLogEntity.CreateBy = userName;
                orderInfoLogEntity.CreateTime = DateTime.Now;
                orderInfoLogDal.Add(orderInfoLogEntity);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
        }

        /// <summary>
        /// 团成员订单订单来源相关值的更新
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="paidAmount"></param>
        /// <param name="userName"></param>
        /// <param name="OrderStatus"></param>
        public void TeamMemberOrderUpdatePayOK(OrderInfoSourceEntity orderInfoSource)
        {
            try
            {
                //LogHelper.Info("--------TeamMemberOrderUpdate---1----TeamCode;" + orderInfoSource.TeamCode);
                this.orderInfoDal.TeamMemberOrderUpdatePayOK(orderInfoSource);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
        }

    }
}
