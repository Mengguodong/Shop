using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SFO2O.Model.Order;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SFO2O.EntLib.DataExtensions;

namespace SFO2O.DAL.Order
{
    public class StockDal : BaseDal
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static void UpdateByLockedQty(string spu, string sku, int qty, Database db, DbTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Stock  SET LockedQty=ISNULL(LockedQty,0)+@LockedQty ");
            strSql.Append("WHERE Spu=@Spu AND Sku=@Sku ");
            var parameters = db.CreateParameterCollection();
            parameters.Append("@Spu", spu);
            parameters.Append("@Sku", sku);
            parameters.Append("@LockedQty", qty);

            var icount = db.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters, tran);
        }

        /// <summary>
        /// 支付完成减库存
        /// </summary>
        public static void UpdateByPay(string spu, string sku, int qty, Database db, DbTransaction tran, int SQty = 0)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE Stock SET LockedQty=(CASE WHEN ISNULL(LockedQty,0)-@LockedQty<=0 
                            THEN 0 ELSE LockedQty-@LockedQty END
                            ),
                            Qty=(
	                            CASE WHEN ISNULL(Qty,0)-@Qty<=0 
	                            THEN 0 ELSE Qty-@Qty END
                            ),
                            SQty = (
	                            CASE WHEN ISNULL(SQty,0)-@SQty<=0 
	                            THEN 0 ELSE SQty-@SQty END
                            )");
            strSql.Append("WHERE Spu=@Spu AND Sku=@Sku ");
            var parameters = db.CreateParameterCollection();
            parameters.Append("@Spu", spu);
            parameters.Append("@Sku", sku);
            parameters.Append("@LockedQty", qty + SQty);
            parameters.Append("@SQty", SQty);
            parameters.Append("@Qty", qty);

            db.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters, tran);
        }

        /// <summary>
        /// 取消订单退库存
        /// </summary>
        public static void UpdateByForOrderQty(string spu, string sku, int qty, Database db, DbTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Stock  SET LockedQty=(CASE WHEN ISNULL(LockedQty,0)-@LockedQty<=0 THEN 0 ELSE LockedQty-@LockedQty END) ");
            strSql.Append("WHERE Spu=@Spu AND Sku=@Sku ");
            var parameters = db.CreateParameterCollection();
            parameters.Append("@Spu", spu);
            parameters.Append("@Sku", sku);
            parameters.Append("@LockedQty", qty);

            db.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters, tran);
        }
        /// <summary>
        /// 查看当前sku qty数量 必须是上架中的商品
        /// </summary>
        /// <param name="spu"></param>
        /// <param name="sku"></param>
        /// <returns></returns>
        public StockEntity getStockInfo(string spu, string sku)
        {
            string sql = @"  SELECT s.Spu,s.Sku,s.Qty,s.SQty,s.MinSQty,ISNULL(s.LockedQty,0) AS  LockedQty,s.ForOrderQty,s.Updatetime,s.Updateby
                                FROM Stock AS s (NOLOCK)  WHERE s.Spu=@spu AND s.Sku=@sku ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@spu", spu);
            parameters.Append("@sku", sku);
            return DbSFO2ORead.ExecuteSqlFirst<StockEntity>(sql, parameters);
        }
    }
}
