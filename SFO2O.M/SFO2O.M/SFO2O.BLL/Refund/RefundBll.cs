using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Transactions;
using SFO2O.DAL.Refund;
using SFO2O.Model.Refund;
using SFO2O.Utility;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Settle;
using SFO2O.DAL.Settle;
using SFO2O.BLL.Settle;

namespace SFO2O.BLL.Refund
{
    public class RefundBll
    {
        private readonly RefundDal dal = new RefundDal();

        private readonly SettleDal setDal = new SettleDal();
        private readonly SettleBll settleBll = new SettleBll();

        /// <summary>
        /// 退款/退货单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<RefundModel> GetRefundList(int userId,int langauge, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            try
            {
                return dal.GetRefundList(userId,langauge, pageIndex, pageSize, out totalRecords).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return new List<RefundModel>();
        }
        /// <summary>
        /// 退款详情 
        /// </summary>
        /// <param name="refundId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public RefundInfoModel GetRefundInfo(string refundId, int userId)
        {
            return dal.GetRefundInfo(refundId, userId);
        }
        /// <summary>
        /// 生成退款单顺序号
        /// </summary>
        /// <param name="refundCode"></param>
        /// <returns></returns>
        public int GetRefundCodeNo(string orderCode)
        {
            return dal.GetRefundCodeNo(orderCode);
        }
        /// <summary>
        /// 添加申诉
        /// </summary>
        /// <param name="orderEntity"></param>
        /// <param name="productEntity"></param>
        /// <returns></returns>
        public bool AddRefund(RefundOrderEntity orderEntity, RefundProductEntity productEntity)
        {
            try
            {
                using(TransactionScope tran = new TransactionScope())
                {
                    if (dal.AddRefund(orderEntity))
                    {
                        if (dal.AddRefundProduct(productEntity,orderEntity.OrderCode))
                        {
                            if (dal.UpdateRefundQuantity(orderEntity.OrderCode, productEntity.Spu, productEntity.Sku))
                            {
                                tran.Complete();
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return false;
        }
        /// <summary>
        /// 申诉详情
        /// </summary>
        /// <param name="refundCode">退款单号</param>
        /// <returns></returns>
        public RefundOrderEntity GetRefundOrderInfo(string refundCode)
        {
            try
            {
                return dal.GetRefundOrderInfo(refundCode);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return new RefundOrderEntity();
        }
        /// <summary>
        /// 修改申诉
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool EditRefund(RefundOrderEntity model)
        {
            try
            {
                return dal.EditRefund(model);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return false;
        }
        
        /// <summary>
        /// 退款商品列表
        /// </summary>
        /// <param name="refundId"></param>
        /// <returns></returns>
        public List<RefundProductEntity> GetRefundProducts(string refundId,int language)
        {
            try
            {
                return dal.GetRefundProducts(refundId,language);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return new List<RefundProductEntity>();
        }
        /// <summary>
        /// 该商品是否还能进行申诉
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="sku"></param>
        /// <returns></returns>
        public bool IsCanRefund(string orderCode, string sku)
        {
            try
            {
                return dal.IsCanRefund(orderCode, sku);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return false;
        }
        public List<RefundProductEntity> getOrderProductCount(string orderCode, string sku) 
        {
            try
            {
                return dal.getOrderProductCount(orderCode, sku);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return null;
        }
    }
}
