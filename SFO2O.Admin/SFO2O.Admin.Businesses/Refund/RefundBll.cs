using SFO2O.Admin.Businesses.Order;
using SFO2O.Admin.DAO.Order;
using SFO2O.Admin.DAO.Refund;
using SFO2O.Admin.DAO.Supplier;
using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Payment;
using SFO2O.Admin.Models.Refund;
using SFO2O.Admin.ViewModel.Refund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Businesses.Refund
{
    public class RefundBll
    {
        private RefundDao dao = new RefundDao();
        private OrderDao order = new OrderDao();
        private SupplierInfoDAL supplierDao = new SupplierInfoDAL();
        /// <summary>
        /// 获取退款单列表
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public PagedList<RefundOrderListInfo> getOrderList(RefundQueryModel queryModel, PagingModel pModel)
        {
            return dao.getOrderList(queryModel, pModel);
        }
        /// <summary>
        /// 获取退款单明细
        /// </summary>
        /// <param name="refundCode"></param>
        /// <returns></returns>
        public RefundDetailInfo getRefundDetail(string refundCode)
        {
            var detailInfo = new RefundDetailInfo();
            detailInfo.OrderInfo = dao.getOrderInfo(refundCode);
            detailInfo.OrderProducts = dao.getOrderProducts(refundCode);
            detailInfo.BuyerInfo = order.getOrderInfo(detailInfo.OrderInfo.OrderCode);
            detailInfo.SellerInfo = supplierDao.GetSupplierInfo(detailInfo.OrderInfo.SupplierId);

            return detailInfo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="refundCode"></param>
        /// <param name="refuseReason"></param>
        /// <param name="refuseTime"></param>
        /// <param name="refuseBy"></param>
        /// <returns></returns>
        public int refuseRefund(string refundCode, string refuseReason, DateTime refuseTime, string refuseBy)
        {
            return dao.refuseRefund(refundCode, refuseReason, refuseTime, refuseBy);
        }
        /// <summary>
        /// 退款审核通过
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int agreeRefund(RefundOrderInfo order)
        {
            return dao.agreeRefund(order);
        }
        /// <summary>
        /// 上门取件
        /// </summary>
        /// <param name="refundCode"></param>
        /// <param name="expressCompany"></param>
        /// <param name="expressList"></param>
        /// <param name="pickupTime"></param>
        /// <param name="pickupper"></param>
        /// <returns></returns>
        public int refundPickUp(string refundCode, string expressCompany, string expressList, DateTime pickupTime, string pickupper)
        {
            return dao.refundPickUp(refundCode, expressCompany, expressList, pickupTime, pickupper);
        }
        /// <summary>
        /// 退款单退款-修改退款单状态，写支付记录，生成结算单
        /// </summary>
        /// <param name="refundCode"></param>
        /// <param name="rmbTotalAmount"></param>
        /// <param name="payPlatform"></param>
        /// <param name="settlementTime"></param>
        /// <param name="tradeCode"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public int returnRefund(string refundCode, decimal rmbTotalAmount, int payPlatform, DateTime settlementTime, string tradeCode, string UserName)
        {
            return dao.returnRefund(refundCode, rmbTotalAmount, payPlatform, settlementTime, tradeCode, UserName);
        }
        /// <summary>
        /// 获取退款支付信息
        /// </summary>
        /// <param name="refundCode"></param>
        /// <returns></returns>
        public OrderPaymentInfo getRefundPaymentInfo(string refundCode)
        {
            return dao.getRefundPaymentInfo(refundCode);
        }
    }
}
