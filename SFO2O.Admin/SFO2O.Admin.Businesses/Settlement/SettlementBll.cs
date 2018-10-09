using SFO2O.Admin.DAO.Settlement;
using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Settlement;
using SFO2O.Admin.ViewModel.Settlement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Businesses.Settlement
{
    public class SettlementBll
    {
        protected SettlementDao Dao
        {
            get { return new SettlementDao(); }
        }
        /// <summary>
        /// 获取结算单列表
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public PagedList<SettlementOrderInfo> getOrderList(SettlementQueryModel queryModel, PagingModel pModel)
        {
            return Dao.getOrderList(queryModel, pModel);
        }
        /// <summary>
        /// 获取单个结算单信息，包含结算单、结算单支付、结算单明细
        /// </summary>
        /// <param name="settlementCode"></param>
        /// <returns></returns>
        public SettlementInfo getSettlementInfos(string settlementCode)
        {
            var settlementInfo = new SettlementInfo();
            var orderInfo = Dao.getOrderInfo(settlementCode);
            var paymentInfo = Dao.getPaymentInfo(orderInfo.TradeCode);
            var productList = Dao.getOrderProducts(settlementCode);
            settlementInfo.OrderInfo = orderInfo;
            settlementInfo.PaymentInfo = paymentInfo;
            settlementInfo.OrderProducts = productList;
            return settlementInfo;
        }
        /// <summary>
        /// 审核结算单
        /// </summary>
        /// <param name="settlementCode"></param>
        /// <param name="settlementAmount"></param>
        /// <param name="rmbSettlementAmount"></param>
        /// <param name="supplierBearDutyAmount"></param>
        /// <param name="rmbSupplierBearDutyAmount"></param>
        /// <param name="bearDutyAmount"></param>
        /// <param name="rmbBearDutyAmount"></param>
        /// <param name="otherAmount"></param>
        /// <param name="rmbOtherAmount"></param>
        /// <param name="auditTime"></param>
        /// <param name="auditor"></param>
        /// <returns></returns>
        public int auditOrder(string settlementCode, decimal settlementAmount, decimal rmbSettlementAmount, decimal supplierBearDutyAmount, decimal rmbSupplierBearDutyAmount, decimal bearDutyAmount, decimal rmbBearDutyAmount,decimal otherAmount,decimal rmbOtherAmount, DateTime auditTime, string auditor)
        {
            return Dao.auditOrder(settlementCode, settlementAmount, rmbSettlementAmount, supplierBearDutyAmount, rmbSupplierBearDutyAmount, bearDutyAmount, rmbBearDutyAmount,otherAmount,rmbOtherAmount, auditTime, auditor);
        }
        /// <summary>
        /// 结算单支付
        /// </summary>
        /// <param name="settlementCodes"></param>
        /// <param name="settlementTime"></param>
        /// <param name="payPlatform"></param>
        /// <param name="tradeCode"></param>
        /// <param name="payAmount"></param>
        /// <param name="payer"></param>
        /// <returns></returns>
        public int payOrder(string settlementCodes,DateTime settlementTime,int payPlatform, string tradeCode, decimal payAmount, string payer)
        {
            return Dao.payOrder(settlementCodes, settlementTime, payPlatform, tradeCode, payAmount, payer);
        }
    }
}
