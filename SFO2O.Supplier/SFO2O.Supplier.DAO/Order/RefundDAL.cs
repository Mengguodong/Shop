
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using SFO2O.Supplier.Common;
using SFO2O.Supplier.Models;

namespace SFO2O.Supplier.DAO.Order
{
    public class RefundDAL : BaseDao
    {
       
        public PageOf<RefundInfoModel> GetRefundOrderList(RefundQueryInfo queryInfo, PageDTO page, LanguageEnum languageVersion, int receiptCountry)
        {
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            var sql = @"(  select r.OrderCode,r.RefundCode,r.UserId,c.UserName,r.RefundStatus,r.CreateTime,r.RefundType,rp.Spu,rp.Sku,p.Name,s.MainDicValue,
                            s.SubDicValue,s.MainValue,s.SubValue,rp.UnitPrice,rp.RMBUnitPrice,rp.TaxRate,r.TotalAmount,o.ExchangeRate,rp.Quantity,pImg.ImagePath as ProductImagePath
                            FROM RefundOrderInfo r(NOLOCK)
                            inner join RefundOrderProducts rp(NOLOCK) on r.RefundCode= rp.RefundCode
                            inner join SkuInfo s(NOLOCK) on s.Sku= rp.Sku 
                            inner join ProductInfo p(NOLOCK) on p.Id=s.SpuId and p.LanguageVersion=@LanguageVersion
                            left join ProductImage pImg(NOLOCK) ON pImg.Spu = p.Spu AND pImg.SortValue = 1 AND pImg.ImageType = 1 	
                            inner join OrderInfo o(NOLOCK) on r.OrderCode = o.OrderCode
                            inner join customer c(NOLOCK) on c.ID=r.UserId
                            where p.SupplierId= @SupplierId ";

            if (receiptCountry != 0)
            {
                sql = sql + " and o.ReceiptCountry=@ReceiptCountry";
            }

            var query = BindQuery(queryInfo, parameters);

            sql = sql + query;

            sql += ") pp ";

            string SQL = string.Format(@" select * from (select ROW_NUMBER() OVER(order by pp.CreateTime desc) AS RowNum,* from {0}
										) as A where A.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;",
                                  sql);

            SQL += string.Format(@" SELECT COUNT(1) FROM {0};", sql);

            parameters.Append("LanguageVersion", (int)languageVersion);
            parameters.Append("SupplierId", queryInfo.SupplierId);
            parameters.Append("ReceiptCountry", receiptCountry);
            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);

            DataSet ds = db.ExecuteSqlDataSet(SQL, parameters);

            return new PageOf<RefundInfoModel>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                Items = DataMapHelper.DataSetToList<RefundInfoModel>(ds)
            };
        }

        private string BindQuery(RefundQueryInfo queryInfo, ParameterCollection dbParameters)
        {
            var stringBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(queryInfo.refundCode))
            {
                stringBuilder.Append(" and  r.RefundCode=@RefundCode");
                dbParameters.Append("RefundCode", queryInfo.refundCode);
            }
            else
            {
                if (!string.IsNullOrEmpty(queryInfo.orderCode))
                {
                    stringBuilder.Append(" and  r.OrderCode=@OrderCode");
                    dbParameters.Append("OrderCode", queryInfo.orderCode);
                }
                if (queryInfo.refundStatus != -1)
                {
                    stringBuilder.Append(" and  r.RefundStatus=@RefundStatus");
                    dbParameters.Append("RefundStatus", queryInfo.refundStatus);
                }
                if (queryInfo.refundType != 0)
                {
                    stringBuilder.Append(" and  r.RefundType=@RefundType");
                    dbParameters.Append("RefundType", queryInfo.refundType);
                }
                if (queryInfo.startTime != null)
                {
                    stringBuilder.Append(" and  o.CreateTime> @StartTime");
                    dbParameters.Append("StartTime", queryInfo.startTime.ToString("yyyy-MM-dd 00:00:00"));
                }
                if (queryInfo.endTime != null)
                {
                    stringBuilder.Append(" and  o.CreateTime< @EndTime");
                    dbParameters.Append("EndTime", queryInfo.endTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
                }
            }

            return stringBuilder.ToString();
        }

        public RefundTotalInfo GetRefundTotal(RefundQueryInfo queryInfo, LanguageEnum languageEnum)
        {
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            var sql = @"  select count(distinct r.RefundCode) as RefundCount,count (distinct rp.Sku) as ProductCount,sum(r.TotalAmount) as RefundAmountTotal
                          from RefundOrderInfo r (NOLOCK)
                          inner join RefundOrderProducts rp(NOLOCK) on r.RefundCode= rp.RefundCode
                          inner join SkuInfo s(NOLOCK) on s.Sku= rp.Sku 
                          inner join ProductInfo p(NOLOCK) on p.Id=s.SpuId and p.LanguageVersion=@LanguageVersion
                          inner join OrderInfo o(NOLOCK) on r.OrderCode = o.OrderCode 
                          where p.SupplierId= @SupplierId";

            var query = BindQuery(queryInfo, parameters);

            sql = sql + query;

            parameters.Append("LanguageVersion", (int)languageEnum);
            parameters.Append("SupplierId", queryInfo.SupplierId);

            return db.ExecuteSqlFirst<RefundTotalInfo>(sql, parameters);
        }

        public IList<RefundDetailModel> GetRefundInfos(string refundCode, LanguageEnum languageEnum)
        {
            var sql = @"SELECT r.OrderCode,r.RefundCode,r.RefundStatus,r.RmbTotalAmount,r.RmbDutyAmount,r.TotalAmount,r.RefundType,r.RefundReason,r.ProductStatus,r.RefundDescription,r.CreateTime,r.AuditTime
                        ,r.PickupTime,r.CompletionTime,r.ImagePath,c.UserName,o.Receiver,o.ReceiptAddress,o.Phone,rp.Spu,rp.Sku,p.Name,s.MainDicValue,s.BarCode
                        ,s.SubDicValue,s.MainValue,s.SubValue,rp.UnitPrice,rp.RMBUnitPrice,rp.TaxRate,r.TotalAmount,o.ExchangeRate,rp.Quantity,pImg.ImagePath as ProductImagePath
                        ,pro.ProvinceName,ci.CityName,a.AreaName
                        FROM RefundOrderInfo r(NOLOCK) 
                        inner join RefundOrderProducts rp(NOLOCK) on r.RefundCode= rp.RefundCode
                        inner join SkuInfo s(NOLOCK) on s.Sku= rp.Sku 
                        inner join ProductInfo p(NOLOCK) on p.Id=s.SpuId and p.LanguageVersion=@LanguageVersion
                        inner join OrderInfo o(NOLOCK) on r.OrderCode = o.OrderCode
                        inner join Province pro(NOLOCK) on pro.ProvinceId=o.ReceiptProvince and pro.LanguageVersion=@LanguageVersion
                        inner join City ci(NOLOCK) on ci.CityId= o.ReceiptCity  and ci.LanguageVersion=@LanguageVersion
                        inner join Area a(NOLOCK) on a.AreaId= o.ReceiptRegion and a.LanguageVersion=@LanguageVersion
                        inner join Customer c(NOLOCK) on o.UserId=c.ID
                        left join ProductImage pImg(NOLOCK) ON pImg.Spu = p.Spu AND pImg.SortValue = 1 AND pImg.ImageType = 1 	
                        where r.RefundCode=@RefundCode ";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("LanguageVersion", (int)languageEnum);
            parameters.Append("RefundCode", refundCode);

            var ds = db.ExecuteSqlDataSet(sql, parameters);

            return DataMapHelper.DataSetToList<RefundDetailModel>(ds);
        }
    }
}
