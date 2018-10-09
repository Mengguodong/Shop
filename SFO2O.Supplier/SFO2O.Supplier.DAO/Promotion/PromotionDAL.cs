using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using SFO2O.Supplier.Models.Promotion;
using SFO2O.Supplier.ViewModels.Promotion;
using SFO2O.Supplier.Models;
using System.Data;
using SFO2O.Supplier.Common;

namespace SFO2O.Supplier.DAO.Promotion
{
    public class PromotionDAL : BaseDao
    {
        public PageOf<PromotionListModel> GetPromotionList(int supplierId, PromotionQuery query, PageDTO page)
        {
            var querySql = @" SELECT ROW_NUMBER() OVER(ORDER BY createtime DESC) AS RowNum,Id,PromotionName,PromotionStatus,StartTime,EndTime FROM Promotions(NOLOCK)
                            WHERE SupplierId=@SupplierId AND StartTime >=@StartTime AND StartTime <=@EndTime ";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("SupplierId", supplierId);
            parameters.Append("StartTime", query.StartTime);
            parameters.Append("EndTime", query.EndTime);


            if (!String.IsNullOrWhiteSpace(query.PromotionName))
            {
                querySql += " AND PromotionName LIKE @PromotionName";
                parameters.Append("PromotionName", "%" + query.PromotionName + "%");
            }

            if (query.PromotionStatus >= 0)
            {
                querySql += " AND PromotionStatus=@PromotionStatus";
                parameters.Append("PromotionStatus", query.PromotionStatus);
            }

            var sql = String.Empty;
            sql += "SELECT * FROM (" + querySql + ")a" + " WHERE a.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum";
            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);

            sql += string.Format(@" SELECT COUNT(1) FROM ({0}) b;", querySql);

            DataSet ds = db.ExecuteSqlDataSet(sql, parameters);

            return new PageOf<PromotionListModel>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                RowCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                Items = DataMapHelper.DataSetToList<PromotionListModel>(ds)
            };
        }

        public PageOf<PromotionSkuListModel> GetSpuulierSkus(int supplierId, string productName, PageDTO page)
        {
            var querySql = @" WITH pSku
                            AS
                            (
	                            SELECT DISTINCT p.Id,ps.sku,1 AS IsInPromotion,ps.DiscountPrice,ps.DiscountRate FROM Promotions(NOLOCK)p
	                            INNER JOIN PromotionSku(NOLOCK) AS PS
	                            ON p.Id = ps.PromotionId 
	                            WHERE p.PromotionStatus NOT IN(3,4) AND p.SupplierId=@SupplierId
                            )
                            {0}
                            SELECT ROW_NUMBER() OVER(ORDER BY p.ModifyTime DESC, s.spu,s.sku) AS RowNum,s.spu,s.Sku,p.Name,s.MainValue,s.SubValue,s.Price,CASE psku.IsInPromotion WHEN 1 THEN 'true' ELSE 'false' END AS skuProStatus
                            ,pSku.Id AS PromotionId,pSku.DiscountPrice AS PromotionPrice,pSku.DiscountRate AS PromotionRate,pImg.ImagePath
                            FROM ProductInfo(NOLOCK) p
                            INNER JOIN SkuInfo(NOLOCK) s
                            ON p.Id=s.SpuId AND p.LanguageVersion=2 AND p.SupplierId=@SupplierId AND s.Status=3
                            LEFT JOIN ProductImage(NOLOCK) pImg
                            ON pImg.Spu = p.Spu AND pImg.SortValue=1
                            LEFT JOIN pSku
                            ON pSku.Sku = s.Sku
                            WHERE 1=1 ";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("SupplierId", supplierId);


            if (!String.IsNullOrWhiteSpace(productName))
            {
                querySql += " AND name LIKE @Name";
                parameters.Append("Name", "%" + productName + "%");
            }

            var sql = String.Empty;
            sql = String.Format(querySql, "SELECT * FROM (") + ")a WHERE a.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;";

            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);

            sql += String.Format(querySql, "SELECT COUNT(1) FROM (") + ") b";

            DataSet ds = db.ExecuteSqlDataSet(sql, parameters);

            return new PageOf<PromotionSkuListModel>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                RowCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                Items = DataMapHelper.DataSetToList<PromotionSkuListModel>(ds)
            };
        }

        public List<PromotionInfoModel> GetPromotionSkus(int promotionId, int supplierId)
        {
            var querySql = @" SELECT ps.spu,ps.Sku,p.CreateTime,ps.DiscountPrice,ps.DiscountRate FROM Promotions(NOLOCK) p
                                INNER JOIN PromotionSku(NOLOCK) ps
                                ON p.Id = ps.PromotionId
                                WHERE p.SupplierId=@supplierId AND p.Id = @PromotionId
                                ORDER BY p.CreateTime DESC, ps.id DESC";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            parameters.Append("PromotionId", promotionId);
            parameters.Append("SupplierId", supplierId);

            return db.ExecuteSqlList<PromotionInfoModel>(querySql, parameters).ToList();
        }

        public List<PromotionSkuListModel> GetPromotionSkuInfo(List<string> skus)
        {
            var sql = @"SELECT s.spu,s.Sku,p.Name,s.MainValue,s.SubValue,s.Price
                    ,pImg.ImagePath
                    FROM ProductInfo(NOLOCK) p
                    INNER JOIN SkuInfo(NOLOCK) s
                    ON p.Id=s.SpuId AND p.LanguageVersion=2 AND s.Status=3
                    LEFT JOIN ProductImage(NOLOCK) pImg
                    ON pImg.Spu = p.Spu AND pImg.SortValue=1
                    WHERE s.sku IN ({0})";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            StringBuilder sb = new StringBuilder(1000);
            int index = 0;
            foreach (var sku in skus)
            {
                sb.Append("@sku" + index + ",");
                parameters.Append("@sku" + index, sku);
                index++;
            }

            return DbSFO2OMain.ExecuteSqlList<PromotionSkuListModel>(String.Format(sql, sb.Remove(sb.Length - 1, 1).ToString()), parameters).ToList();
        }

        public void SavePromotion(List<RedisPromotionSpuModel> promptionSpus, PromotionMainInfoModel promotionMainInfo)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    var promotionId = 0;

                    if (promotionMainInfo.Id <= 0)
                    {
                        promotionId = SavePromotionMainInfo(db, tran, promotionMainInfo);
                    }
                    else
                    {
                        promotionId = promotionMainInfo.Id;
                    }

                    var index = 0;
                    foreach (var spu in promptionSpus)
                    {
                        if (spu.Skus != null && spu.Skus.Count > 0)
                        {
                            foreach (var sku in spu.Skus)
                            {
                                SavePromotionDetail(db, tran, sku, spu.Spu, promotionId, index);
                                index++;
                            }
                        }
                    }

                    tran.Commit();
                    connection.Close();
                    connection.Dispose();
                }
                catch (Exception ext)
                {
                    tran.Rollback();
                    connection.Close();
                    connection.Dispose();
                    throw ext;
                }
            }
        }

        /// <summary>
        /// 保存促销主要信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tran"></param>
        /// <param name="promotionId"></param>
        /// <param name="promotionName"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="promotionLable"></param>
        /// <param name="promotionCost"></param>
        public int SavePromotionMainInfo(Database db, System.Data.Common.DbTransaction tran, PromotionMainInfoModel promotionMainInfo)
        {
            var sql = @"IF EXISTS(SELECT id FROM Promotions(NOLOCK) WHERE id=@PromotionId)
                        BEGIN
	                        UPDATE Promotions SET SupplierId=@supplierID,PromotionName=@PromotionName,StartTime=@StartTime,EndTime=@EndTime,
	                        PromotionLable=@PromotionLable,PromotionCost=@PromotionCost
	                        WHERE id=@PromotionId
                            SET @pId=@PromotionId
                        END
                        ELSE
                        BEGIN
	                        INSERT INTO Promotions VALUES(@supplierID,@PromotionName,@StartTime,@EndTime,@PromotionLable,@PromotionCost,0,1,GETDATE(),@SupplierName)
                            SET @pId = @@IDENTITY
                        END";

            var paras = db.CreateParameterCollection();
            paras.Append("@PromotionId", promotionMainInfo.Id);

            paras.Append("@supplierID", promotionMainInfo.SupplierId);
            paras.Append("@PromotionName", promotionMainInfo.PromotionName);

            paras.Append("@StartTime", promotionMainInfo.StartTime);
            paras.Append("@EndTime", promotionMainInfo.EndTime);

            paras.Append("@PromotionLable", promotionMainInfo.PromotionLable);
            paras.Append("@PromotionCost", promotionMainInfo.PromotionCost);

            paras.Append("@SupplierName", promotionMainInfo.CreateBy);
            paras.Append("@pId", 0, DbType.Int32, ParameterDirection.InputOutput);

            db.ExecuteNonQuery(CommandType.Text, sql, paras, tran);
            var id = 0;
            int.TryParse(paras["@pId"].Value.ToString(), out id);

            return id;
        }

        /// <summary>
        /// 保存促销商品信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tran"></param>
        /// <param name="skuModel"></param>
        /// <param name="spu"></param>
        /// <param name="promotionId"></param>
        /// <param name="i"></param>
        public void SavePromotionDetail(Database db, System.Data.Common.DbTransaction tran, RedisPromotionSkuModel skuModel, string spu, int promotionId, int i)
        {
            var sql = @"IF EXISTS(SELECT id FROM PromotionSku(NOLOCK) WHERE PromotionId=@PromotionId" + i + " AND Spu=@Spu" + i + " AND Sku=@Sku" + i + @")
                        BEGIN
	                        UPDATE PromotionSku SET DiscountPrice=@DiscountPrice" + i + ",DiscountRate=@DiscountRate" + i + @"
	                        WHERE PromotionId=@PromotionId" + i + " AND Spu=@Spu" + i + " AND Sku=@Sku" + i + @"
                        END
                        ELSE
                        BEGIN
	                        INSERT INTO PromotionSku VALUES(@PromotionId" + i + ",@Spu" + i + ",@Sku" + i + ",@DiscountRate" + i + ",@DiscountPrice" + i + @")
                        END";

            var paras = db.CreateParameterCollection();
            paras.Append("@PromotionId" + i, promotionId);

            paras.Append("@Spu" + i, spu);
            paras.Append("@Sku" + i, skuModel.sku);

            paras.Append("@DiscountPrice" + i, skuModel.PromotionPrice);
            paras.Append("@DiscountRate" + i, skuModel.PromotionRate);

            db.ExecuteNonQuery(CommandType.Text, sql, paras, tran);
        }


        /// <summary>
        /// 终止促销
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="promotionId"></param>
        public void CanclePromotion(int supplierId, int promotionId)
        {
            var sql = @"UPDATE Promotions SET PromotionStatus=3
                    WHERE SupplierId=@SupplierID AND id=@PromotionId";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("PromotionId", promotionId);
            parameters.Append("SupplierID", supplierId);

            db.ExecuteNonQuery(CommandType.Text, sql, parameters);
        }



        public List<PromotionSkuListModel> ViewPromotionSkus(int supplierId, int promotionId)
        {
            var sql = @"SELECT  s.spu,s.Sku,pinfo.Name,s.MainValue,s.SubValue,s.Price
                    ,pImg.ImagePath,ps.DiscountPrice AS PromotionPrice,ps.DiscountRate AS PromotionRate
                    FROM Promotions(NOLOCK) p
                    INNER JOIN PromotionSku(NOLOCK) ps
                    ON p.Id=ps.PromotionId
                    INNER JOIN ProductInfo(NOLOCK) pinfo
                    ON pinfo.Spu=ps.spu AND pinfo.LanguageVersion=2
                    INNER JOIN SkuInfo(NOLOCK) s
                    ON s.SpuId=pinfo.Id AND s.Sku=ps.Sku
                    LEFT JOIN ProductImage(NOLOCK) pImg
                    ON pImg.Spu = pinfo.Spu AND pImg.SortValue=1
                    WHERE p.Id=@PromotionId AND p.SupplierId = @SupplierId";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("PromotionId", promotionId);
            parameters.Append("SupplierId", supplierId);

            return db.ExecuteSqlList<PromotionSkuListModel>(sql, parameters).ToList();
        }

        public PromotionMainInfoModel GetPromotionMainModel(int supplierId, int promotionId)
        {
            var sql = @"SELECT * FROM Promotions(NOLOCK)
                    WHERE Id=@PromotionId AND SupplierId = @SupplierId";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("PromotionId", promotionId);
            parameters.Append("SupplierId", supplierId);

            return db.ExecuteSqlFirst<PromotionMainInfoModel>(sql, parameters);
        }
    }
}
