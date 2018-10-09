using SFO2O.Admin.Common;
using SFO2O.Admin.DAO.Order;
using SFO2O.Admin.Models.Order;
using SFO2O.Admin.Service.GetSFData;
using SFO2O.Admin.ViewModel;
using SFO2O.Admin.ViewModel.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Businesses.Order
{
    public class OrderBll
    {
        private OrderDao orderDao = new OrderDao();
        /// <summary>
        /// 根据订单号获取订单信息
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public OrderInfo getOrderInfo(string orderCode)
        {
            return orderDao.getOrderInfo(orderCode);
        }

        public OrderModel GetOrderDetailInfo(string orderCode)
        {
            var order = new OrderModel();

            order.MainOrder = orderDao.GetOrderMainInfo(orderCode);
            order.OrderDetails = orderDao.GetOrderDetail(orderCode);
            order.OrderLogistics = GetOrderLogistics(orderCode);
            if (order.OrderLogistics != null && order.OrderLogistics.Count > 0)
            {
                order.MainOrder.ExpressCode = order.OrderLogistics[0].ExpressList;
            }

            return order;
        }

        public List<OrderLogisticsModel> GetOrderLogistics(string orderCode)
        {
            try
            {
                var orderExpress = orderDao.GetOrderExpress(orderCode);
                if (orderExpress == null || string.IsNullOrEmpty(orderExpress.ExpressList))
                {
                    return null;
                }
                var client = new GetSFDataClient();
                var expInfos = client.GetExpressInfo(orderExpress.ExpressList);
                var list = ConvertExpressInfo(expInfos);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        item.ExpressCompany = orderExpress.ExpressCompany;
                        item.ExpressList = orderExpress.ExpressList;
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                return null;
            }
        }

        private List<OrderLogisticsModel> ConvertExpressInfo(ExpressInfoEntity[] arr)
        {
            if (arr == null)
            {
                return null;
            }
            var list = new List<OrderLogisticsModel>(arr.Length);
            foreach (var expressInfo in arr)
            {
                var ol = new OrderLogisticsModel();
                ol.LogisticsTime = expressInfo.AcceptTime;
                ol.Remark = expressInfo.Remark;
                list.Add(ol);
            }
            return list;
        }

        public OrderListTotalModel GetOrderCount(OrderListQueryModel query)
        {
            return orderDao.GetOrderCount(query);
        }

        public PageOf<OrderListModel> GetOrderList(OrderListQueryModel query, int pageSize, int pageIndex)
        {
            return orderDao.GetOrderList(query, pageSize, pageIndex);
        }

        public List<OrderStockOutModel> GetOrderStockOutInfos(DateTime startTime, DateTime endTime)
        {
            return orderDao.GetOrderStockOutInfos(startTime, endTime);
        }
    }
}
