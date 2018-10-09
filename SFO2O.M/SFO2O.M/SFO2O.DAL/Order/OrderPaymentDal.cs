using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SFO2O.Model.Order;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Utility.Uitl;

namespace SFO2O.DAL.Order
{
    public class OrderPaymentDal:BaseDal
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Add(OrderPaymentEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into OrderPayment(");
            strSql.Append("PayCode,TradeCode,UserId,OrderType,OrderCode,PayAmount,PaidAmount,PayPlatform,PayType,PayStatus,PayTerminal,PayCompleteTime,PayBackRemark,Remark,CreateTime,CreateBy)");
            strSql.Append(" values (");
            strSql.Append("@PayCode,@TradeCode,@UserId,@OrderType,@OrderCode,@PayAmount,@PaidAmount,@PayPlatform,@PayType,@PayStatus,@PayTerminal,@PayCompleteTime,@PayBackRemark,@Remark,@CreateTime,@CreateBy)");

            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@PayCode", entity.PayCode);
            parameters.Append("@TradeCode", entity.TradeCode);
            parameters.Append("@UserId", entity.UserId);
            parameters.Append("@OrderType", entity.OrderType);
            parameters.Append("@OrderCode", entity.OrderCode);
            parameters.Append("@PayAmount", entity.PayAmount);
            parameters.Append("@PaidAmount", entity.PaidAmount);
            parameters.Append("@PayPlatform", entity.PayPlatform);
            parameters.Append("@PayType", entity.PayType);
            parameters.Append("@PayStatus", entity.PayStatus);
            parameters.Append("@PayTerminal", entity.PayTerminal);
            parameters.Append("@PayCompleteTime", entity.PayCompleteTime);
            parameters.Append("@PayBackRemark", entity.PayBackRemark);
            parameters.Append("@Remark", entity.Remark);
            parameters.Append("@CreateTime", entity.CreateTime);
            parameters.Append("@CreateBy", entity.CreateBy);

            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool UpdatePaysuccess(OrderPaymentEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE OrderPayment ");
            strSql.Append("SET TradeCode=@TradeCode,PaidAmount=@PaidAmount,PayStatus=@PayStatus, ");
            strSql.Append("PayCompleteTime=@PayCompleteTime,PayBackRemark=@PayBackRemark ");
            strSql.Append("WHERE PayCode=@PayCode ");
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@PayCode", entity.PayCode);
            parameters.Append("@TradeCode", entity.TradeCode);
            parameters.Append("@PaidAmount", entity.PaidAmount);
            parameters.Append("@PayStatus", entity.PayStatus);
            parameters.Append("@PayCompleteTime", entity.PayCompleteTime);
            parameters.Append("@PayBackRemark", entity.PayBackRemark);

            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters)>0;
        }


        public OrderPaymentEntity selectOrderCode(string OrderCode,int PayPlatform)
        {
            const string sql = @"SELECT TOP 1 PayCode as PayCode FROM OrderPayment AS op WHERE op.OrderCode=@OrderCode AND op.PayPlatform=@PayPlatform ORDER BY op.CreateTime desc";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@OrderCode", OrderCode);
            parameters.Append("@PayPlatform", PayPlatform);

            return DbSFO2ORead.ExecuteSqlFirst<OrderPaymentEntity>(sql, parameters);
        }
        /// <summary>
        /// 支付失败
        /// </summary>
        public bool UpdatePayFailure(OrderPaymentEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE OrderPayment ");
            strSql.Append("SET TradeCode=@TradeCode,PayStatus=@PayStatus, ");
            strSql.Append("PayCompleteTime=@PayCompleteTime,PayBackRemark=@PayBackRemark ");
            strSql.Append("WHERE PayCode=@PayCode ");
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@PayCode", entity.PayCode);
            parameters.Append("@TradeCode", entity.TradeCode);
            parameters.Append("@PayStatus", entity.PayStatus);
            parameters.Append("@PayCompleteTime", entity.PayCompleteTime);
            parameters.Append("@PayBackRemark", entity.PayBackRemark);

            return  DbSFO2OMain.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters)>0;
        }

        /// <summary>
        /// 获取支付信息
        /// </summary>
        /// <param name="PayCode"></param>
        /// <returns></returns>
        public OrderPaymentEntity GetOrderPaymentByCode(string payCode)
        {
            const string sql = @"SELECT [Id]
                                  ,[PayCode]
                                  ,[TradeCode]
                                  ,[UserId]
                                  ,[OrderType]
                                  ,[OrderCode]
                                  ,[PayAmount]
                                  ,[PaidAmount]
                                  ,[PayPlatform]
                                  ,[PayType]
                                  ,[PayStatus]
                                  ,[PayTerminal]
                                  ,[PayCompleteTime]
                                  ,[PayBackRemark]
                                  ,[Remark]
                                  ,[CreateTime]
                                  ,[CreateBy]
                              FROM [OrderPayment] (NOLOCK)
                              WHERE PayCode=@PayCode ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@PayCode", payCode);

            return DbSFO2ORead.ExecuteSqlFirst<OrderPaymentEntity>(sql, parameters);
        }

        public OrderPaymentEntity GetOrderPaymentByTradeCode(string tradeCode)
        {
            const string sql = @"SELECT [Id]
                                  ,[PayCode]
                                  ,[TradeCode]
                                  ,[UserId]
                                  ,[OrderType]
                                  ,[OrderCode]
                                  ,[PayAmount]
                                  ,[PaidAmount]
                                  ,[PayPlatform]
                                  ,[PayType]
                                  ,[PayStatus]
                                  ,[PayTerminal]
                                  ,[PayCompleteTime]
                                  ,[PayBackRemark]
                                  ,[Remark]
                                  ,[CreateTime]
                                  ,[CreateBy]
                              FROM [OrderPayment] (NOLOCK)
                              WHERE TradeCode=@TradeCode ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@TradeCode", tradeCode);

            return DbSFO2ORead.ExecuteSqlFirst<OrderPaymentEntity>(sql, parameters);
        }
        public OrderPaymentEntity GetOrderPaymentByPayCode(string PayCode)
        {
            const string sql = @"SELECT [Id]
                                  ,[PayCode]
                                  ,[TradeCode]
                                  ,[UserId]
                                  ,[OrderType]
                                  ,[OrderCode]
                                  ,[PayAmount]
                                  ,[PaidAmount]
                                  ,[PayPlatform]
                                  ,[PayType]
                                  ,[PayStatus]
                                  ,[PayTerminal]
                                  ,[PayCompleteTime]
                                  ,[PayBackRemark]
                                  ,[Remark]
                                  ,[CreateTime]
                                  ,[CreateBy]
                              FROM [OrderPayment] (NOLOCK)
                              WHERE PayCode=@PayCode ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@PayCode", PayCode);

            return DbSFO2ORead.ExecuteSqlFirst<OrderPaymentEntity>(sql, parameters);
        }
    }
}
