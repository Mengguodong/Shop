using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFO2O.DAL.Order;
using SFO2O.Model.Order;
using SFO2O.Model.Product;
using SFO2O.BLL.Shopping;
using SFO2O.DAL.Common;
using SFO2O.BLL.Common;
using SFO2O.M.ViewModel.Order;
using SFO2O.Utility.Uitl;

namespace SFO2O.BLL.Order
{
    public class OrderManager
    {
        public readonly OrderInfoDal orderInfoDal = new OrderInfoDal();
        public readonly CommonDal commonDal = new CommonDal();
        public readonly AddressBll addressBll = new AddressBll();


        /// <summary>
        /// 设置订单号
        /// </summary>
        /// <returns></returns>
        public string SetOrderCode()
        {
            return orderInfoDal.GetNewOrderId();
        }
        public string SetTeamOrderCode()
        {
            return orderInfoDal.GetNewTeamOrderId();
        }
        public void SetOrderInfo(OrderInfoEntity orderInfoEntity)
        {
            orderInfoEntity.OrderStatus = 0;
            orderInfoEntity.PaidAmount = 0;
            orderInfoEntity.PayType = 1;
            orderInfoEntity.ShippingMethod = 1;
            orderInfoEntity.PayTime = null;
            orderInfoEntity.DeliveryTime = null;
            orderInfoEntity.ArrivalTime = null;
            orderInfoEntity.OrderCompletionTime = null;
            orderInfoEntity.CancelReason = "";
            orderInfoEntity.Remark = "";
        }

        public void SetOrderAddress(OrderInfoEntity orderInfoEntity, int id,int DeliveryRegion)
        {
            var model = addressBll.GetAddressById(id);
            orderInfoEntity.Receiver = model.Receiver;
            orderInfoEntity.Phone = model.Phone;
            orderInfoEntity.PassPortType = model.PapersType;
            orderInfoEntity.PassPortNum = model.PapersCode;
            orderInfoEntity.ReceiptAddress = model.Address;
            orderInfoEntity.ReceiptPostalCode = model.PostCode;
            orderInfoEntity.ReceiptRegion = model.AreaId;
            orderInfoEntity.ReceiptCity = model.CityId;
            orderInfoEntity.ReceiptProvince = model.ProvinceId;
            orderInfoEntity.ReceiptCountry = DeliveryRegion.ToString();
        }

        /// <summary>
        /// 查询订单信息
        /// </summary>
        /// <returns></returns>
        public OrderInfoEntity GetOrderInfoByOrderCode(string orderCode)
        {
            return orderInfoDal.GetOrderInfoByOrderCode(orderCode);
        }


        public OrderInfoEntity GetOrderInfoByCodeAndStatus(string orderCode)
        {
            return orderInfoDal.GetOrderInfoByCodeAndStatus(orderCode);
        }
        /// <summary>
        /// 获得支付订单相关信息，如税类型、支付平台等
        /// </summary>
        /// <returns></returns>
        public OrderInfoEntity GetOrderInfoByPay(string orderCode)
        {
            return orderInfoDal.GetOrderInfoByPay(orderCode);
        }

        public OrderInfoEntity GetOrderInfoHuoliByOrderCode(string orderCode)
        {
            return orderInfoDal.GetOrderInfoHuoliByOrderCode(orderCode);
        }


        /// <summary>
        /// 查询需要支付的订单号
        /// </summary>
        /// <returns></returns>
        public OrderInfoEntity GetOrderCodeByParentOrderCode(string parentOrderCode)
        {
            return orderInfoDal.GetOrderCodeByParentOrderCode(parentOrderCode);
        }

        /// <summary>
        /// 增加推送IBS订单信息
        /// </summary>
        /// <param name="model"></param>
        public void CreatePushIBSOrderInfo(PushIBSOrderInfoEntity model)
        {
            orderInfoDal.CreatePushIBSOrderInfo(model);
        }

        /// <summary>
        /// 添加需要推送的订单信息到push表中
        /// </summary>
        /// <param name="out_trade_no"></param>
        public void AddPushOrderInfo(String out_trade_no)
        {
            PushIBSOrderInfoEntity model = null;
            int insertFlag = 0;

            /// 创建推送IBS订单信息对象
            model = new PushIBSOrderInfoEntity();
            /// 根据支付宝响应信息的OrderCode获得订单的父订单号
            OrderInfoEntity orderInfoEntity = GetOrderInfoByPay(out_trade_no);
            string taxtypeStr = orderInfoEntity.TaxType.ToString();
            /// 判断订单是否是综合税的
            if (orderInfoEntity.TaxType == 1)
            {
                insertFlag = 1;
            }

            if (insertFlag == 1)
            {
                model.OrderCode = out_trade_no;
                model.PushStatus = 0;
                model.PushSuccTime = null;
                model.GateWayCode = null;
                model.TaxType = 1;
                model.PayType = orderInfoEntity.PayPlatform;
                model.CreateTime = DateTime.Now;
                /// 增加推送IBS订单信息
                CreatePushIBSOrderInfo(model);
            }
        }

        public IList<OrderInfoEntity> GetNeedCloseOrderInfoByCode(string TeamCode)
        {
            IList<OrderInfoEntity> NeedCloseOrderInfoList = orderInfoDal.GetNeedCloseOrderInfoByCode(TeamCode);
            return NeedCloseOrderInfoList;
        }

        public ProductInfoModel GetOrderImage(string OrderCode)
        {
            return orderInfoDal.GetOrderImage(OrderCode);
        }

        public IList<OrderInfoEntity> GetOrderInfoByTeamCode(string TeamCode)
        {
            return orderInfoDal.GetOrderInfoByTeamCode(TeamCode);
        }

        /// <summary>
        ///根据订单号获取活力值
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public decimal GetHuoLiByOrderId(string orderId)
        {
            OrderInfoEntity model = new OrderInfoEntity();

            model = GetOrderInfoHuoliByOrderCode(orderId);

            return model.Huoli;

           
        }
        public decimal GetHuoLiByOrderIdBy(string orderId)
        {
            OrderInfoEntity model = new OrderInfoEntity();

            model = GetOrderInfoHuoliByOrderCode(orderId);

            return model.Huoli;


        }
    }
}
