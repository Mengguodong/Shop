using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SFO2O.Model.Settle;
using SFO2O.DAL.Settle;
using SFO2O.Utility.Uitl;

namespace SFO2O.BLL.Settle
{
    public class SettleBll
    {
        private SettleDal dal = new SettleDal();

        ///// <summary>
        ///// 新增结算单
        ///// </summary>
        ///// <param name="e"></param>
        ///// <param name="productList"></param>
        ///// <returns></returns>
        //public bool AddSettle(SettleOrderInfoEntity e, List<SettleProductEntity> productList)
        //{
        //    try
        //    {
        //        e.SettlementCode = BuildSettleCode(e.OrderCode);
        //        using (TransactionScope tran = new TransactionScope())
        //        {
        //            if (dal.AddSettle(e))
        //            {
        //                bool success = true;
        //                foreach (var p in productList)
        //                {
        //                    p.SettlementCode = e.SettlementCode;
        //                    if (!dal.AddSettleProduct(p))
        //                    {
        //                        success = false;
        //                        break;
        //                    }
        //                }
        //                if (success)
        //                {
        //                    tran.Complete();
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error(ex);
        //    }
        //    return false;
        //}

        /// <summary>
        /// 批量生成结算单
        /// </summary>
        /// <param name="settles"></param>
        /// <returns></returns>
        public bool AddSettleBatch(Dictionary<SettleOrderInfoEntity, List<SettleProductEntity>> settles)
        {

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    bool isOk = true;
                    foreach (var settle in settles)
                    {
                        if (dal.AddSettle(settle.Key))
                        {

                            foreach (var p in settle.Value)
                            {
                                p.SettlementCode = settle.Key.SettlementCode;
                                if (!dal.AddSettleProduct(p))
                                {
                                    isOk = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            isOk = false;
                            break;
                        }
                    }
                    tran.Complete();
                    return isOk;

                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);

                }
                finally
                {
                    tran.Dispose();
                }
            }

            return false;
        }

        /// <summary>
        /// 生成结算单号
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="index">索引流水</param>
        /// <returns></returns>
        public string BuildSettleCode(string orderCode, int index = 1)
        {
            string settleCode = "C" + orderCode.Substring(1);
            string orderByNumber = (dal.GetSettleCodeNo(orderCode)+index).ToString().PadLeft(3, '0');
            settleCode += "-" + orderByNumber;
            return settleCode;
        }
        /// <summary>
        /// 取消退款并生成结算单
        /// </summary>
        /// <param name="refundCode">退款单号</param>
        /// <param name="orderCode">订单号</param>
        /// <param name="cancelTime">取消时间</param>
        /// <param name="cancelBy">取消人</param>
        /// <returns></returns>
        public bool CancelRefund(string refundCode, string orderCode, DateTime cancelTime, string cancelBy)
        {
            try
            {
                string settleCode = BuildSettleCode(orderCode,1);
                return dal.CancelRefund(refundCode, settleCode, cancelTime, cancelBy)>0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return false;
        }
    }
}
