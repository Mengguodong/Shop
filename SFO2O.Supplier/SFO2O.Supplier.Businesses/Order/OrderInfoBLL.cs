using SFO2O.Supplier.DAO.Order;
using SFO2O.Supplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Businesses.Order
{
    public class OrderInfoBLL
    {
        OrderInfoDAL dal = new OrderInfoDAL();

        public PageOf<OrderListInfoModel> GetOrderList(Models.OrderQueryInfo queryInfo, Models.PageDTO page, Models.LanguageEnum languageVersion, int receiptCountry)
        {
            return dal.GetOrderList(queryInfo, page, languageVersion,receiptCountry);  
        }

        public OrderTotalInfo GetOrderTotal(OrderQueryInfo queryInfo, LanguageEnum languageEnum, int receiptCountry)
        {
            return dal.GetOrderTotal(queryInfo, languageEnum, receiptCountry);
        }

        public OrderViewModel GetOrderDetailByOrderCode(string orderCode, int supplierId, LanguageEnum languageEnum)
        {
            //订单主信息
            var model = new OrderViewModel();
            model.order = GetOrderModel(orderCode, supplierId,languageEnum);

            //订单明细信息
            model.orderProducts = GetOrderProducts(orderCode, supplierId, languageEnum);

            return model;
        }

        public decimal GetOrderTotalTaxRate(string orderCode, int supplierId)
        {
            return dal.GetOrderTotalTaxRate(orderCode, supplierId);
        }

        public IList<OrderLogistics> GetOrderLogistics(string orderCode, int supplierId)
        {
            return dal.GetOrderLogistics(orderCode, supplierId);
        }

        private IList<Models.OrderProduct> GetOrderProducts(string orderCode, int supplierId, LanguageEnum languageEnum)
        {
            return dal.GetOrderProducts(orderCode, supplierId, languageEnum);
        }

        private Models.Order GetOrderModel(string orderCode, int supplierId, LanguageEnum languageEnum)
        {
            return dal.GetOrderModel(orderCode, supplierId, languageEnum);
        }


        public bool ComfirmSendGoods(string OrderCode, string expressCompany, string expressCode, string freight)
        {
            return dal.ComfirmSendGoods(OrderCode, expressCompany, expressCode, freight);
        }

        public bool UpdateOrderInfo(string orderCode, string expressCompany, string expressCode)
        {
            return dal.UpdateOrderInfo(orderCode, expressCompany, expressCode);
        }

        public OrderExpressEntity GetOrderExpress(string orderId)
        {
            return dal.GetOrderExpress(orderId);
        }
    }
}
