using SFO2O.Supplier.DAO.Order;
using SFO2O.Supplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Businesses.Order
{
    public class SettlementBll
    {
        SettlementDAL dal = new SettlementDAL(); 

        /// <summary>
        /// 获取单个结算单信息，包含结算单、结算单支付、结算单明细
        /// </summary>
        /// <param name="settlementCode"></param>
        /// <returns></returns>
        public SettlementInfo getSettlementInfos(string settlementCode)
        {
            var settlementInfo = new SettlementInfo();

            var orderInfo = dal.getOrderInfo(settlementCode);
            var productList = dal.getOrderProducts(settlementCode);
            var paymentInfo = dal.getPaymentInfo(orderInfo.TradeCode);
            settlementInfo.OrderInfo = orderInfo;
            settlementInfo.OrderProducts = productList;
            settlementInfo.PaymentInfo = paymentInfo;

            return settlementInfo;
        }


        /// <summary>
        /// 获取结算单列表
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <param name="pageDTO"></param>
        /// <returns></returns>
        public PageOf<SettlementOrderInfo> getSettlementList(SettlementQueryModel queryInfo, PageDTO pageDTO)
        {
            return dal.getSettlementList(queryInfo, pageDTO);
        }
    }
}
