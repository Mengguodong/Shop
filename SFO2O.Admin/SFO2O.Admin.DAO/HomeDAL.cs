using SFO2O.Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.EntLib.DataExtensions;
using SFO2O.Admin.Common;

namespace SFO2O.Admin.DAO
{
    public class HomeDAL : BaseDao
    {
        public HomePageViewModel GetHomePageStatistics()
        {
            var endDate = DateTime.Now.Date;
            var prevDate = endDate.AddDays(-1);
            var prevMonth = endDate.AddDays(-31);
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("endDate", endDate);
            parameters.Append("prevDate", prevDate);
            parameters.Append("prevMonth", prevMonth);
            const string sql = @"
DECLARE @T0 TABLE(UserId int,ConsumeCount int)
INSERT INTO @T0
SELECT o.UserId,COUNT(o.OrderCode) ConsumeCount
FROM OrderInfo o(NOLOCK)
WHERE o.PayTime<=@endDate AND ISNULL(o.PaidAmount,0) > 0
GROUP BY o.UserId

SELECT
(
SELECT COUNT(*) FROM Supplier
WHERE-- [Status]=1 AND//是否过滤上架状态？
CreateTime<=@endDate
) TotalSupplierCount
,(SELECT COUNT(*) FROM @T0) TotalConsumeMemberCount
,(SELECT COUNT(*) FROM @T0 WHERE ConsumeCount>1) TotalReConsumeMemberCount
,(SELECT COUNT(DISTINCT Sku) FROM SkuInfo WHERE CreateTime<=@endDate) TotalSkuCount
,(SELECT ISNULL(SUM(C),0) FROM
(
	SELECT COUNT(DISTINCT s.Sku) C
	FROM ProductInfo p
	INNER JOIN SkuInfo s ON s.SpuId=p.Id
	LEFT JOIN Stock t ON t.Sku=s.Sku
	WHERE s.CreateTime<=@endDate AND s.[Status]=3 AND p.LanguageVersion=2 AND ISNULL(t.ForOrderQty,0)>0
	GROUP BY p.Spu,p.MinForOrder
	HAVING SUM(t.ForOrderQty)>p.MinForOrder
) T) TotalSkuSellCount

DECLARE @T1 TABLE(id int identity(1,1),CompleteOrderCount int,SellProductAmount decimal(18, 2),SellSkuCount int)
DECLARE @T2 TABLE(id int identity(1,1),RefundOrderCount int,RefundProductAmount decimal(18, 2))
DECLARE @T3 TABLE(id int identity(1,1),MemberCount int)
INSERT INTO @T1
SELECT COUNT(*),SUM(OrderSkuAmount),SUM(OrderSkuCount)
FROM(
SELECT SUM(op.UnitPrice * op.Quantity) OrderSkuAmount,SUM(op.Quantity) OrderSkuCount
FROM OrderInfo oi
INNER JOIN OrderProducts op ON op.OrderCode = oi.OrderCode
INNER JOIN ProductInfo p ON p.Spu=op.Spu
WHERE p.LanguageVersion=2 AND oi.OrderStatus=4 AND oi.OrderCompletionTime<=@endDate
GROUP BY oi.OrderCode) T1
INSERT INTO @T2
SELECT COUNT(*),SUM(RefundAmount)
FROM(
SELECT SUM(op.UnitPrice * op.Quantity) RefundAmount
FROM RefundOrderInfo oi
INNER JOIN RefundOrderProducts op ON op.RefundCode = oi.RefundCode
INNER JOIN ProductInfo p ON p.Spu=op.Spu
WHERE p.LanguageVersion=2 AND oi.RefundStatus=4 AND oi.CompletionTime<=@endDate
GROUP BY oi.RefundCode
) T2
INSERT INTO @T3
SELECT COUNT(*) FROM Customer WHERE [Status]=1

INSERT INTO @T1
SELECT COUNT(*),SUM(OrderSkuAmount),SUM(OrderSkuCount)
FROM(
SELECT SUM(op.UnitPrice * op.Quantity) OrderSkuAmount,SUM(op.Quantity) OrderSkuCount
FROM OrderInfo oi
INNER JOIN OrderProducts op ON op.OrderCode = oi.OrderCode
INNER JOIN ProductInfo p ON p.Spu=op.Spu
WHERE p.LanguageVersion=2 AND oi.OrderStatus=4 AND oi.OrderCompletionTime>=@prevDate AND oi.OrderCompletionTime<=@endDate
GROUP BY oi.OrderCode) T1
INSERT INTO @T2
SELECT COUNT(*),SUM(RefundAmount)
FROM(
SELECT SUM(op.UnitPrice * op.Quantity) RefundAmount
FROM RefundOrderInfo oi
INNER JOIN RefundOrderProducts op ON op.RefundCode = oi.RefundCode
INNER JOIN ProductInfo p ON p.Spu=op.Spu
WHERE p.LanguageVersion=2 AND oi.RefundStatus=4 AND oi.CompletionTime>=@prevDate AND oi.CompletionTime<=@endDate
GROUP BY oi.RefundCode
) T2
INSERT INTO @T3
SELECT COUNT(*) FROM Customer WHERE [Status]=1 AND CreateTime>=@prevDate AND CreateTime<=@endDate

INSERT INTO @T1
SELECT COUNT(*),SUM(OrderSkuAmount),SUM(OrderSkuCount)
FROM(
SELECT SUM(op.UnitPrice * op.Quantity) OrderSkuAmount,SUM(op.Quantity) OrderSkuCount
FROM OrderInfo oi
INNER JOIN OrderProducts op ON op.OrderCode = oi.OrderCode
INNER JOIN ProductInfo p ON p.Spu=op.Spu
WHERE p.LanguageVersion=2 AND oi.OrderStatus=4 AND oi.OrderCompletionTime>=@prevMonth AND oi.OrderCompletionTime<=@endDate
GROUP BY oi.OrderCode) T1
INSERT INTO @T2
SELECT COUNT(*),SUM(RefundAmount)
FROM(
SELECT SUM(op.UnitPrice * op.Quantity) RefundAmount
FROM RefundOrderInfo oi
INNER JOIN RefundOrderProducts op ON op.RefundCode = oi.RefundCode
INNER JOIN ProductInfo p ON p.Spu=op.Spu
WHERE p.LanguageVersion=2 AND oi.RefundStatus=4 AND oi.CompletionTime>=@prevMonth AND oi.CompletionTime<=@endDate
GROUP BY oi.RefundCode
) T2
INSERT INTO @T3
SELECT COUNT(*) FROM Customer WHERE [Status]=1 AND CreateTime>=@prevMonth AND CreateTime<=@endDate

SELECT ISNULL(CompleteOrderCount,0) CompleteOrderCount,ISNULL(SellProductAmount,0) SellProductAmount
,ISNULL(SellSkuCount,0) SellSkuCount,ISNULL(RefundOrderCount,0) RefundOrderCount
,ISNULL(RefundProductAmount,0) RefundProductAmount,MemberCount
FROM @T1 t1 INNER JOIN @T2 t2 ON t1.id=t2.id INNER JOIN @T3 t3 ON t1.id=t3.id

SELECT TOP 5 CompanyName
,(
	SELECT ISNULL(SUM(SkuCount),0) FROM
	(
		SELECT COUNT(DISTINCT s.Sku) SkuCount
		FROM SkuInfo s
		INNER JOIN ProductInfo pInfo ON s.SpuId=pInfo.Id
		LEFT JOIN Stock t ON t.Sku=s.Sku
		WHERE pInfo.SupplierId=su.SupplierID AND s.CreateTime<=@endDate
		AND s.[Status]=3 AND pInfo.LanguageVersion=2 AND ISNULL(t.ForOrderQty,0)>0
		GROUP BY pInfo.Spu,pInfo.MinForOrder
		HAVING SUM(t.ForOrderQty)>pInfo.MinForOrder
	) T0
) OnSaleCount,ISNULL(SUM(OrderSkuCount),0) SellCount,ISNULL(SUM(OrderSkuAmount),0) SellAmount,COUNT(T1.SupplierID) OrderCount
FROM Supplier su
LEFT JOIN (
	SELECT p.SupplierId,SUM(op.Quantity) OrderSkuCount,SUM(op.UnitPrice * op.Quantity) OrderSkuAmount
	FROM OrderInfo oi
	INNER JOIN OrderProducts op ON op.OrderCode = oi.OrderCode
	INNER JOIN ProductInfo p ON p.Spu=op.Spu
	WHERE p.LanguageVersion=2 AND oi.OrderStatus=4 AND oi.OrderCompletionTime>=@prevMonth AND oi.OrderCompletionTime<=@endDate
	GROUP BY p.SupplierId,oi.OrderCode
) T1 ON su.SupplierID=T1.SupplierId
WHERE-- su.[Status]=1 AND//是否过滤上架状态？
su.CreateTime<=@endDate
GROUP BY su.SupplierID,su.CompanyName
ORDER BY SellCount DESC,su.SupplierID

SELECT TOP 5 Name,CompanyName,SUM(OrderSkuCount) SellCount,SUM(OrderSkuAmount) SellAmount,COUNT(*) OrderCount
FROM
(
	SELECT p.SupplierId SupplierId,p.Id,p.Spu,p.Name,SUM(op.Quantity) OrderSkuCount,SUM(op.UnitPrice * op.Quantity) OrderSkuAmount
	FROM OrderInfo oi
	INNER JOIN OrderProducts op ON op.OrderCode = oi.OrderCode
	INNER JOIN ProductInfo p ON p.Spu=op.Spu
	WHERE p.LanguageVersion=2 AND oi.OrderStatus=4 AND oi.OrderCompletionTime>=@prevMonth AND oi.OrderCompletionTime<=@endDate
	GROUP BY p.SupplierId,p.Id,p.Spu,p.Name,oi.OrderCode
) T
INNER JOIN Supplier s ON s.SupplierID=T.SupplierId
GROUP BY T.SupplierId,T.Id,Spu,Name,CompanyName
ORDER BY SellCount DESC,T.Id

SELECT TOP 5 Name,CompanyName
,ReturnCount
,CASE SellCount WHEN 0 THEN CASE ReturnCount WHEN 0 THEN 0 ELSE ReturnCount END ELSE SellCount END SellCount
FROM Supplier s 
INNER JOIN (
	SELECT p.SupplierId,p.Id,p.Spu,p.Name
	,(
		SELECT COUNT(oi.RefundCode)
		FROM RefundOrderInfo oi
		INNER JOIN RefundOrderProducts op ON op.RefundCode = oi.RefundCode
		INNER JOIN ProductInfo pInfo ON pInfo.Spu=op.Spu
		WHERE pInfo.Spu=p.Spu AND pInfo.LanguageVersion=2 AND oi.RefundStatus=4 AND oi.CompletionTime>=@prevMonth AND oi.CompletionTime<=@endDate
	) ReturnCount
	,(
		SELECT ISNULL(SUM(op.Quantity),0)
		FROM OrderInfo oi
		INNER JOIN OrderProducts op ON op.OrderCode = oi.OrderCode
		INNER JOIN ProductInfo pInfo ON pInfo.Spu=op.Spu
		WHERE pInfo.Spu=p.Spu AND pInfo.LanguageVersion=2 AND oi.OrderStatus=4 AND oi.OrderCompletionTime>=@prevMonth AND oi.OrderCompletionTime<=@endDate
	) SellCount
	FROM ProductInfo p
	WHERE p.LanguageVersion=2 AND p.Createtime<=@endDate
	GROUP BY p.SupplierId,p.Id,p.Spu,p.Name
) T ON s.SupplierID=T.SupplierId
ORDER BY 
	CASE SellCount WHEN 0 THEN CASE ReturnCount WHEN 0 THEN 0 ELSE 1 END
		ELSE CONVERT(float,ReturnCount)/SellCount
	END
DESC,T.Id";
            DataSet ds = db.ExecuteSqlDataSet(sql, parameters);
            var model = ds.Tables[0].Rows[0].ToEntity<HomePageViewModel>();
            var statistics = ds.Tables[1].ToEntityList<HomePageStatistics>();
            model.TopSellCountSupplierList = ds.Tables[2].ToEntityList<SupplierSellRank>();
            model.TopSellCountProductList = ds.Tables[3].ToEntityList<ProductSellRank>();
            model.TopReturnCountProductList = ds.Tables[4].ToEntityList<ProductReturnRank>();
            model.TotalStatistics = statistics[0];
            model.YesterdayStatistics = statistics[1];
            model.PastmonthStatistics = statistics[2];
            return model;
        }

        public List<SupplierSellRank> GetTopSupplierSellRank()
        {
            var endDate = DateTime.Now.Date;
            var prevMonth = endDate.AddDays(-31);
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("endDate", endDate);
            parameters.Append("prevMonth", prevMonth);
            const string sql = @"
SELECT CompanyName
,(
	SELECT ISNULL(SUM(SkuCount),0) FROM
	(
		SELECT COUNT(DISTINCT s.Sku) SkuCount
		FROM SkuInfo s
		INNER JOIN ProductInfo pInfo ON s.SpuId=pInfo.Id
		LEFT JOIN Stock t ON t.Sku=s.Sku
		WHERE pInfo.SupplierId=su.SupplierID AND s.CreateTime<=@endDate
		AND s.[Status]=3 AND pInfo.LanguageVersion=2 AND ISNULL(t.ForOrderQty,0)>0
		GROUP BY pInfo.Spu,pInfo.MinForOrder
		HAVING SUM(t.ForOrderQty)>pInfo.MinForOrder
	) T0
) OnSaleCount,ISNULL(SUM(OrderSkuCount),0) SellCount,ISNULL(SUM(OrderSkuAmount),0) SellAmount,COUNT(T1.SupplierID) OrderCount
FROM Supplier su
LEFT JOIN (
	SELECT p.SupplierId,SUM(op.Quantity) OrderSkuCount,SUM(op.UnitPrice * op.Quantity) OrderSkuAmount
	FROM OrderInfo oi
	INNER JOIN OrderProducts op ON op.OrderCode = oi.OrderCode
	INNER JOIN ProductInfo p ON p.Spu=op.Spu
	WHERE p.LanguageVersion=2 AND oi.OrderStatus=4 AND oi.OrderCompletionTime>=@prevMonth AND oi.OrderCompletionTime<=@endDate
	GROUP BY p.SupplierId,oi.OrderCode
) T1 ON su.SupplierID=T1.SupplierId
WHERE-- su.[Status]=1 AND//是否过滤上架状态？
su.CreateTime<=@endDate
GROUP BY su.SupplierID,su.CompanyName
ORDER BY SellCount DESC,su.SupplierID";
            DataSet ds = db.ExecuteSqlDataSet(sql, parameters);
            return ds.Tables[0].ToEntityList<SupplierSellRank>();
        }

        public List<ProductSellRank> GetTop50ProductSellRank()
        {
            var endDate = DateTime.Now.Date;
            var prevMonth = endDate.AddDays(-31);
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("endDate", endDate);
            parameters.Append("prevMonth", prevMonth);
            const string sql = @"
SELECT TOP 50 Name,CompanyName,SUM(OrderSkuCount) SellCount,SUM(OrderSkuAmount) SellAmount,COUNT(*) OrderCount
FROM
(
	SELECT p.SupplierId SupplierId,p.Id,p.Spu,p.Name,SUM(op.Quantity) OrderSkuCount,SUM(op.UnitPrice * op.Quantity) OrderSkuAmount
	FROM OrderInfo oi
	INNER JOIN OrderProducts op ON op.OrderCode = oi.OrderCode
	INNER JOIN ProductInfo p ON p.Spu=op.Spu
	WHERE p.LanguageVersion=2 AND oi.OrderStatus=4 AND oi.OrderCompletionTime>=@prevMonth AND oi.OrderCompletionTime<=@endDate
	GROUP BY p.SupplierId,p.Id,p.Spu,p.Name,oi.OrderCode
) T
INNER JOIN Supplier s ON s.SupplierID=T.SupplierId
GROUP BY T.SupplierId,T.Id,Spu,Name,CompanyName
ORDER BY SellCount DESC,T.Id";
            DataSet ds = db.ExecuteSqlDataSet(sql, parameters);
            return ds.Tables[0].ToEntityList<ProductSellRank>();
        }

        public List<ProductReturnRank> GetTop50ProductReturnRank()
        {
            var endDate = DateTime.Now.Date;
            var prevMonth = endDate.AddDays(-31);
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("endDate", endDate);
            parameters.Append("prevMonth", prevMonth);
            const string sql = @"
SELECT TOP 50 Name,CompanyName
,ReturnCount
,CASE SellCount WHEN 0 THEN CASE ReturnCount WHEN 0 THEN 0 ELSE ReturnCount END ELSE SellCount END SellCount
FROM Supplier s 
INNER JOIN (
	SELECT p.SupplierId,p.Id,p.Spu,p.Name
	,(
		SELECT COUNT(oi.RefundCode)
		FROM RefundOrderInfo oi
		INNER JOIN RefundOrderProducts op ON op.RefundCode = oi.RefundCode
		INNER JOIN ProductInfo pInfo ON pInfo.Spu=op.Spu
		WHERE pInfo.Spu=p.Spu AND pInfo.LanguageVersion=2 AND oi.RefundStatus=4 AND oi.CompletionTime>=@prevMonth AND oi.CompletionTime<=@endDate
	) ReturnCount
	,(
		SELECT ISNULL(SUM(op.Quantity),0)
		FROM OrderInfo oi
		INNER JOIN OrderProducts op ON op.OrderCode = oi.OrderCode
		INNER JOIN ProductInfo pInfo ON pInfo.Spu=op.Spu
		WHERE pInfo.Spu=p.Spu AND pInfo.LanguageVersion=2 AND oi.OrderStatus=4 AND oi.OrderCompletionTime>=@prevMonth AND oi.OrderCompletionTime<=@endDate
	) SellCount
	FROM ProductInfo p
	WHERE p.LanguageVersion=2 AND p.Createtime<=@endDate
	GROUP BY p.SupplierId,p.Id,p.Spu,p.Name
) T ON s.SupplierID=T.SupplierId
ORDER BY 
	CASE SellCount WHEN 0 THEN CASE ReturnCount WHEN 0 THEN 0 ELSE 1 END
		ELSE CONVERT(float,ReturnCount)/SellCount
	END
DESC,T.Id";
            DataSet ds = db.ExecuteSqlDataSet(sql, parameters);
            return  ds.Tables[0].ToEntityList<ProductReturnRank>();
        }
    }
}
