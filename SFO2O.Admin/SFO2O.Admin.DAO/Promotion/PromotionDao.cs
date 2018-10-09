using SFO2O.Admin.Models.Promotion;
using SFO2O.Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.EntLib.DataExtensions;
using SFO2O.Admin.Common;

namespace SFO2O.Admin.DAO.Promotion
{
    public class PromotionDao : BaseDao
    {
        public PageOf<PromotionInfoModel> GetPromotionList(PromotionQueryModel query, int pageSize, int pageIndex)
        {
            var sql = @"(SELECT p.[Id],p.[SupplierId],p.[PromotionName],p.[StartTime],p.[EndTime],p.[PromotionLable],p.[PromotionCost]
,p.[PromotionStatus],p.[PromotionType],p.[CreateTime],p.[CreateBy],s.CompanyName 
FROM [Promotions] p
INNER JOIN [Supplier] s ON s.SupplierID = p.[SupplierId]";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            #region 查询条件

            var sb = new StringBuilder();
            sb.Append(" AND ((@StartTime<=p.StartTime AND @EndTime>=p.StartTime) OR (@StartTime<=p.EndTime AND @EndTime>=p.EndTime))");
            parameters.Append("StartTime", query.BeginDate);
            parameters.Append("EndTime", query.EndDate);

            if (query.PromotionStatus != null && query.PromotionStatus.Length > 0)
            {
                if (query.PromotionStatus.Length == 1)
                {
                    sb.Append(" AND p.PromotionStatus=@PromotionStatus");
                    parameters.Append("PromotionStatus", query.PromotionStatus[0]);
                }
                else
                {
                    sb.Append(" AND p.PromotionStatus IN (" + String.Join(",", query.PromotionStatus) + ")");
                }
            }

            if (!string.IsNullOrWhiteSpace(query.PromotionName))
            {
                sb.Append(" AND p.PromotionName LIKE @PromotionName");
                parameters.Append("PromotionName", "%" + query.PromotionName.Trim() + "%");
            }

            if (query.SupplierID > 0)
            {
                sb.Append(" AND p.SupplierId=@SupplierId");
                parameters.Append("SupplierId", query.SupplierID);
            }

            #endregion
            sql += sb.ToString() + ") t ";
            string finallySql = string.Format(@"select * from (select ROW_NUMBER() OVER(order by t.CreateTime DESC) AS RowNum,* from {0}
										) as A where A.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;
                                        SELECT COUNT(1) FROM {0};", sql);

            parameters.Append("PageSize", pageSize);
            parameters.Append("PageIndex", pageIndex);
            var ds = db.ExecuteSqlDataSet(finallySql, parameters);

            return new PageOf<PromotionInfoModel>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                RowCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                Items = ds.Tables[0].ToEntityList<PromotionInfoModel>()
            };
        }

        public int ChangePromotionStatus(int id, int status)
        {
            var sql = "UPDATE [Promotions] SET [PromotionStatus]=@PromotionStatus WHERE Id=@Id";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("PromotionStatus",status);
            parameters.Append("Id", id);
            return db.ExecuteSqlNonQuery(sql, parameters);
        }

        public PromotionInfoModel GetPromotionInfoModel(int promotionId)
        {
            var sql = @"SELECT * FROM Promotions(NOLOCK)
                    WHERE Id=@PromotionId";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("PromotionId", promotionId);

            return db.ExecuteSqlFirst<PromotionInfoModel>(sql, parameters);
        }

        public List<PromotionSkuInfoModel> GetPromotionSkuList(int promotionId)
        {
            var sql = @"SELECT  s.spu,s.Sku,pinfo.Name,s.MainValue,s.SubValue,s.Price
                    ,pImg.ImagePath,ps.DiscountPrice AS PromotionPrice,ps.DiscountRate AS PromotionRate
                    FROM Promotions(NOLOCK) p
                    INNER JOIN PromotionSku(NOLOCK) ps ON p.Id=ps.PromotionId
                    INNER JOIN ProductInfo(NOLOCK) pinfo ON pinfo.Spu=ps.spu AND pinfo.LanguageVersion=2
                    INNER JOIN SkuInfo(NOLOCK) s ON s.SpuId=pinfo.Id AND s.Sku=ps.Sku
                    LEFT JOIN ProductImage(NOLOCK) pImg ON pImg.Spu = pinfo.Spu AND pImg.SortValue=1
                    WHERE p.Id=@PromotionId";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("PromotionId", promotionId);

            return db.ExecuteSqlList<PromotionSkuInfoModel>(sql, parameters).ToList();
        }
    }
}
