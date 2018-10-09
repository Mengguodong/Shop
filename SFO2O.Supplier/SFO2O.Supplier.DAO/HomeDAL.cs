using SFO2O.Supplier.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.EntLib.DataExtensions;
using System.Data;
using SFO2O.Supplier.Common;
//using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SFO2O.Supplier.DAO
{
    public class HomeDAL : BaseDao
    {
        public HomePageViewModel GetHomePageStatistics(int supplierID)
        {
            var endDate = DateTime.Now.Date;
            var prevDate = endDate.AddDays(-1);
            var prevMonth = endDate.AddDays(-31);
            HomePageViewModel model = new HomePageViewModel();
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("supplierID", supplierID);
            parameters.Append("endDate", endDate);
            parameters.Append("prevDate", prevDate);
            parameters.Append("prevMonth", prevMonth);
            var sql = @"
DECLARE @T1 TABLE(id int identity(1,1),CompleteOrderCount int,SellProductAmount decimal(18, 2),SellSkuCount int)
DECLARE @T2 TABLE(id int identity(1,1),RefundOrderCount int,RefundProductAmount decimal(18, 2))
INSERT INTO @T1
SELECT COUNT(*),SUM(OrderSkuAmount),SUM(OrderSkuCount)
FROM
(
	SELECT SUM(op.UnitPrice * op.Quantity) OrderSkuAmount,SUM(op.Quantity) OrderSkuCount
	FROM OrderInfo oi
	INNER JOIN OrderProducts op ON op.OrderCode = oi.OrderCode
	INNER JOIN ProductInfo p ON p.Spu=op.Spu
	WHERE p.SupplierId=@supplierID AND p.LanguageVersion=1 AND oi.OrderStatus=4 AND oi.OrderCompletionTime<=@endDate
	GROUP BY oi.OrderCode
) T1
INSERT INTO @T2
SELECT COUNT(*),SUM(RefundAmount)
FROM
(
	SELECT SUM(op.UnitPrice * op.Quantity) RefundAmount
	FROM RefundOrderInfo oi
	INNER JOIN RefundOrderProducts op ON op.RefundCode = oi.RefundCode
	INNER JOIN ProductInfo p ON p.Spu=op.Spu
	WHERE p.SupplierId=@supplierID AND p.LanguageVersion=1 AND oi.RefundStatus=4 AND oi.CompletionTime<=@endDate
	GROUP BY oi.RefundCode
) T2

INSERT INTO @T1
SELECT COUNT(*),SUM(OrderSkuAmount),SUM(OrderSkuCount)
FROM
(
	SELECT SUM(op.UnitPrice * op.Quantity) OrderSkuAmount,SUM(op.Quantity) OrderSkuCount
	FROM OrderInfo oi
	INNER JOIN OrderProducts op ON op.OrderCode = oi.OrderCode
	INNER JOIN ProductInfo p ON p.Spu=op.Spu
	WHERE p.SupplierId=@supplierID AND p.LanguageVersion=1 AND oi.OrderStatus=4 AND oi.OrderCompletionTime>=@prevDate AND oi.OrderCompletionTime<=@endDate
	GROUP BY oi.OrderCode
) T1
INSERT INTO @T2
SELECT COUNT(*),SUM(RefundAmount)
FROM
(
	SELECT SUM(op.UnitPrice * op.Quantity) RefundAmount
	FROM RefundOrderInfo oi
	INNER JOIN RefundOrderProducts op ON op.RefundCode = oi.RefundCode
	INNER JOIN ProductInfo p ON p.Spu=op.Spu
	WHERE p.SupplierId=@supplierID AND p.LanguageVersion=1 AND oi.RefundStatus=4 AND oi.CompletionTime>=@prevDate AND oi.CompletionTime<=@endDate
	GROUP BY oi.RefundCode
) T2

INSERT INTO @T1
SELECT COUNT(*),SUM(OrderSkuAmount),SUM(OrderSkuCount)
FROM(
	SELECT SUM(op.UnitPrice * op.Quantity) OrderSkuAmount,SUM(op.Quantity) OrderSkuCount
	FROM OrderInfo oi
	INNER JOIN OrderProducts op ON op.OrderCode = oi.OrderCode
	INNER JOIN ProductInfo p ON p.Spu=op.Spu
	WHERE p.SupplierId=@supplierID AND p.LanguageVersion=1 AND oi.OrderStatus=4 AND oi.OrderCompletionTime>=@prevMonth AND oi.OrderCompletionTime<=@endDate
	GROUP BY oi.OrderCode
) T1
INSERT INTO @T2
SELECT COUNT(*),SUM(RefundAmount)
FROM(
	SELECT SUM(op.UnitPrice * op.Quantity) RefundAmount
	FROM RefundOrderInfo oi
	INNER JOIN RefundOrderProducts op ON op.RefundCode = oi.RefundCode
	INNER JOIN ProductInfo p ON p.Spu=op.Spu
	WHERE p.SupplierId=@supplierID AND p.LanguageVersion=1 AND oi.RefundStatus=4 AND oi.CompletionTime>=@prevMonth AND oi.CompletionTime<=@endDate
	GROUP BY oi.RefundCode
) T2

SELECT ISNULL(CompleteOrderCount,0) CompleteOrderCount,ISNULL(SellProductAmount,0) SellProductAmount
,ISNULL(SellSkuCount,0) SellSkuCount
,ISNULL(RefundOrderCount,0) RefundOrderCount,ISNULL(RefundProductAmount,0) RefundProductAmount
FROM @T1 t1 INNER JOIN @T2 t2 ON t1.id=t2.id

SELECT ISNULL(SUM(SkuCount),0) FROM
(
	SELECT COUNT(DISTINCT s.Sku) SkuCount
	FROM ProductInfo p
	INNER JOIN SkuInfo s ON s.SpuId=p.Id
	LEFT JOIN Stock t ON t.Sku=s.Sku
	WHERE p.SupplierId=@supplierID AND s.CreateTime<=@endDate
	AND s.[Status]=3 AND p.LanguageVersion=1 AND ISNULL(t.ForOrderQty,0)>0
	GROUP BY p.Id,p.MinForOrder
	HAVING SUM(t.ForOrderQty)>p.MinForOrder
) T

SELECT TOP 5 p.Name,p.PreOnSaleTime
,CASE WHEN
(
	EXISTS
	(
		SELECT 1 FROM SkuInfo s LEFT JOIN Stock t ON t.Sku=s.Sku
		INNER JOIN ProductInfo pInfo ON s.SpuId=pInfo.Id
		WHERE s.CreateTime<=@endDate AND s.[Status]=3
		AND pInfo.Id=p.Id AND pInfo.LanguageVersion=1 AND ISNULL(t.ForOrderQty,0)>0
		GROUP BY pInfo.MinForOrder
		HAVING SUM(t.ForOrderQty)>pInfo.MinForOrder
	)
) THEN GETDATE()
ELSE
	(SELECT ISNULL(MAX([RemovedTime]),GETDATE()) FROM SkuInfo WHERE SpuId=p.Id)
END AS LastSellDate
,COUNT(DISTINCT oi.OrderCode) OrderCount,SUM(op.Quantity) SellCount,SUM(op.UnitPrice * op.Quantity) SellAmount
FROM OrderInfo oi
INNER JOIN OrderProducts op ON op.OrderCode = oi.OrderCode
INNER JOIN ProductInfo p ON p.Spu=op.Spu
WHERE p.SupplierId=@supplierID AND p.LanguageVersion=1 AND oi.OrderStatus=4 AND oi.OrderCompletionTime<=@endDate
GROUP BY p.Id,p.Name,p.PreOnSaleTime
ORDER BY SellCount DESC";
            DataSet ds = db.ExecuteSqlDataSet(sql, parameters);
            var StatisticsList = ds.Tables[0].ToEntityList<HomePageStatistics>();
            return new HomePageViewModel()
            {
                TotalStatistics = StatisticsList[0],
                YesterdayStatistics = StatisticsList[1],
                PastmonthStatistics = StatisticsList[2],
                OnSellProductCount= (Int32)ds.Tables[1].Rows[0][0],
                TopSellCountProductList = ds.Tables[2].ToEntityList<ProductSellRank>(),
            };
        }
    }
}
