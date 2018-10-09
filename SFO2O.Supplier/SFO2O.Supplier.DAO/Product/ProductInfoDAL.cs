using SFO2O.Supplier.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using SFO2O.Supplier.Common;
using System.Data;
using SFO2O.Supplier.Models;
using SFO2O.Supplier.Models.Category;

namespace SFO2O.Supplier.DAO.Product
{
    public class ProductInfoDAL : BaseDao
    {

        public PageOf<ProductInfoTemp> GetProductTemps(int supplierId, Models.LanguageEnum languageVersion, PageDTO page)
        {
            var QUERY_SQL = @" (select p.Id,p.Spu,p.CategoryId,p.SupplierId,p.Name,p.ModifyTime,p.ModifyBy
                                    ,(cl0.CategoryName + '>>'+ cl1.CategoryName+ '>>'+ cl2.CategoryName) as  CategoryNames
                                from ProductInfo_temp(NOLOCK) p 
	                            inner join Category(NOLOCK) as t2 on p.CategoryID=t2.CategoryId
	                            inner join Category_LanguageVersion(NOLOCK) cl2 on t2.CategoryKey=cl2.CategoryKey and cl2.LanguageVersion=@LanguageVersion
	                            inner join Category(NOLOCK) AS t1 ON t2.ParentId = t1.CategoryID 
	                            inner join Category_LanguageVersion(NOLOCK) cl1 on t1.CategoryKey=cl1.CategoryKey and cl1.LanguageVersion=@LanguageVersion
							    inner join Category(NOLOCK) AS t0 on t1.ParentId=t0.CategoryID
	                            inner join Category_LanguageVersion(NOLOCK) cl0 on t0.CategoryKey=cl0.CategoryKey and cl0.LanguageVersion=@LanguageVersion
                                where p.SupplierID=@SupplierID and p.Status in (-2,-1) and p.LanguageVersion=@LanguageVersion";

            QUERY_SQL += ") pp ";
            string SQL = string.Format(@" select * from (select ROW_NUMBER() OVER(order by pp.ModifyTime desc) AS RowNum,* from {0}
										) as A where A.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;",
                                  QUERY_SQL);

            SQL += string.Format(@" SELECT COUNT(1) FROM {0};", QUERY_SQL);

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("LanguageVersion", (int)languageVersion);
            parameters.Append("SupplierID", supplierId);
            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);

            DataSet ds = db.ExecuteSqlDataSet(SQL, parameters);

            return new PageOf<ProductInfoTemp>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                Items = DataMapHelper.DataSetToList<ProductInfoTemp>(ds)
            };
        }

        public PageOf<ProductTempModel> GetProductList(ProductListQueryInfo queryInfo, Models.LanguageEnum languageVersion, PageDTO page)
        {
            var QUERY_SQL = @"(select p.Id,p.Spu,p.Name,p.CategoryId,cl.CategoryName,p.ModifyTime,p.Createtime,MinForOrder
                                ,(SELECT TOP 1 ImagePath FROM ProductImage(NOLOCK) i WHERE i.Spu=p.Spu AND i.SortValue=1) AS FirstImage
                                from ProductInfo(NOLOCK) p 
	                            inner join Category(NOLOCK) as c on p.CategoryID=c.CategoryId
	                            inner join Category_LanguageVersion(NOLOCK) cl on c.CategoryKey=cl.CategoryKey and cl.LanguageVersion=@LanguageVersion
                                where p.SupplierID=@SupplierID AND p.LanguageVersion=@LanguageVersion";
            var Filter_SKU = " AND sku.Status IN ( " + string.Join(",", queryInfo.SkuStatus.Select(p => p.ToString())) + ")";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("SupplierID", queryInfo.SupplierID);
            parameters.Append("LanguageVersion", (int)languageVersion);

            if (!string.IsNullOrEmpty(queryInfo.ProductName))
            {
                QUERY_SQL += " AND p.Name LIKE @ProductName";
                parameters.Append("ProductName", "%" + queryInfo.ProductName + "%");
            }
            if (queryInfo.UploadTime.HasValue)
            {
                QUERY_SQL += " AND p.Createtime>=@BeginDate AND p.Createtime<@EndDate";
                parameters.Append("BeginDate", queryInfo.UploadTime.Value);
                parameters.Append("EndDate", queryInfo.UploadTime.Value.AddDays(1));
            }
            if (queryInfo.IsOnSaled.HasValue)
            {
                if (queryInfo.IsOnSaled.Value)
                {
                    QUERY_SQL += @" AND EXISTS
	(
		SELECT 1 FROM SkuInfo s LEFT JOIN Stock t ON t.Sku=s.Sku
		INNER JOIN ProductInfo pInfo ON s.SpuId=pInfo.Id
		WHERE s.[Status]=3 AND pInfo.Id=p.Id AND ISNULL(t.ForOrderQty,0)>0
		GROUP BY pInfo.MinForOrder
		HAVING SUM(t.ForOrderQty)>pInfo.MinForOrder
	)";
                }
                else
                {
                    QUERY_SQL += @" AND NOT EXISTS
	(
		SELECT 1 FROM SkuInfo s LEFT JOIN Stock t ON t.Sku=s.Sku
		INNER JOIN ProductInfo pInfo ON s.SpuId=pInfo.Id
		WHERE s.[Status]=3 AND pInfo.Id=p.Id AND ISNULL(t.ForOrderQty,0)>0
		GROUP BY pInfo.MinForOrder
		HAVING SUM(t.ForOrderQty)>pInfo.MinForOrder
	)";
                }
            }
            if (!string.IsNullOrEmpty(queryInfo.BarCode))
            {
                Filter_SKU += " AND sku.BarCode=@BarCode";
                parameters.Append("BarCode", queryInfo.BarCode);
            }
            if (!string.IsNullOrEmpty(queryInfo.Sku))
            {
                Filter_SKU += " AND sku.Sku=@Sku";
                parameters.Append("Sku", queryInfo.Sku);
            }
            if (queryInfo.HasInventory.HasValue)
            {
                if (queryInfo.HasInventory.Value)
                {
                    Filter_SKU += " AND s.ForOrderQty>0";
                }
                else
                {
                    Filter_SKU += " AND ISNULL(s.ForOrderQty,0)=0";
                }
            }

            if (!string.IsNullOrEmpty(Filter_SKU))
            {
                QUERY_SQL += @" AND EXISTS(select TOP 1 sku.id from SkuInfo(NOLOCK) sku left join Stock(NOLOCK) s on sku.Sku=s.Sku where sku.SpuId=p.Id" + Filter_SKU + ")";
            }
            QUERY_SQL += ") pp ";
            string SQL = string.Format(@"select * from (select ROW_NUMBER() OVER(order by pp.ModifyTime desc) AS RowNum,* from {0}
										) as A where A.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;
                                        SELECT COUNT(1) FROM {0};", QUERY_SQL);

            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);

            DataSet ds = db.ExecuteSqlDataSet(SQL, parameters);

            return new PageOf<ProductTempModel>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                RowCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                Items = DataMapHelper.DataSetToList<ProductTempModel>(ds)
            };
        }
        public IList<SkuTempModel> GetSkuList(ProductListQueryInfo queryInfo, int spuId)
        {
            var QUERY_SQL = @"select p.Name,sku.sku,sku.BarCode,sku.[MainDicValue],sku.MainValue
                                ,sku.[SubDicValue],sku.SubValue,sku.Price,sku.Status,ISNULL(s.ForOrderQty,0) Qty,sku.IsOnSaled
                              from ProductInfo(NOLOCK) p
	                          inner join SkuInfo(NOLOCK) as sku on p.Id=sku.SpuId
	                          left join Stock(NOLOCK) s on sku.Sku=s.Sku
                              where p.SupplierID=@SupplierID AND p.Id=@SpuId
                              AND sku.Status IN ( " + string.Join(",", queryInfo.SkuStatus.Select(p => p.ToString())) + ")";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("SupplierID", queryInfo.SupplierID);
            parameters.Append("SpuId", spuId);
            if (!string.IsNullOrEmpty(queryInfo.BarCode))
            {
                QUERY_SQL += " AND sku.BarCode=@BarCode";
                parameters.Append("BarCode", queryInfo.BarCode);
            }
            if (!string.IsNullOrEmpty(queryInfo.Sku))
            {
                QUERY_SQL += " AND sku.Sku=@Sku";
                parameters.Append("Sku", queryInfo.Sku);
            }
            if (queryInfo.HasInventory.HasValue)
            {
                if (queryInfo.HasInventory.Value)
                {
                    QUERY_SQL += " AND s.ForOrderQty>0";
                }
                else
                {
                    QUERY_SQL += " AND ISNULL(s.ForOrderQty,0)=0";
                }
            }

            DataSet ds = db.ExecuteSqlDataSet(QUERY_SQL, parameters);

            return DataMapHelper.DataSetToList<SkuTempModel>(ds);
        }

        public int ChangeSkuStatus(ChangeSkuStatusRequest request)
        {
            var Update_SQL = @"UPDATE sku SET sku.Status=@Status
FROM SkuInfo sku INNER JOIN ProductInfo p ON p.Id=sku.SpuId
WHERE p.SupplierID=@SupplierID AND sku.Sku=@Sku";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("SupplierID", request.SupplierID);
            parameters.Append("Sku", request.Sku);
            parameters.Append("Status", request.Status);

            return db.ExecuteSqlNonQuery(Update_SQL, parameters);
        }

        public PageOf<ProductTempModel> GetAuditProductList(AuditProductListQueryInfo queryInfo, Models.LanguageEnum languageVersion, PageDTO page)
        {
            var strStatus = "( " + string.Join(",", queryInfo.SkuStatus.Select(p => p.ToString())) + ")";
            var QUERY_SQL = @"(select p.Id,p.Spu,p.Name,p.Status,p.CategoryId,cl.CategoryName,p.ModifyTime,p.Createtime,p.DataSource
                                ,(SELECT TOP 1 ImagePath FROM ProductImage_Temp(NOLOCK) i WHERE i.Spu=p.Spu AND i.SortValue=1) AS FirstImage
                                from ProductInfo_temp(NOLOCK) p 
	                            inner join Category(NOLOCK) as c on p.CategoryID=c.CategoryId
	                            inner join Category_LanguageVersion(NOLOCK) cl on c.CategoryKey=cl.CategoryKey and cl.LanguageVersion=@LanguageVersion
                                where p.SupplierID=@SupplierID AND p.LanguageVersion=@LanguageVersion AND p.Status IN " + strStatus;
            var Filter_SKU = " AND sku.Status IN " + strStatus;

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("SupplierID", queryInfo.SupplierID);
            parameters.Append("LanguageVersion", (int)languageVersion);

            if (!string.IsNullOrEmpty(queryInfo.ProductName))
            {
                QUERY_SQL += " AND p.Name LIKE @ProductName";
                parameters.Append("ProductName", "%" + queryInfo.ProductName + "%");
            }
            if (queryInfo.EditTime.HasValue)
            {
                QUERY_SQL += " AND p.ModifyTime>=@BeginDate AND p.ModifyTime<@EndDate";
                parameters.Append("BeginDate", queryInfo.EditTime.Value);
                parameters.Append("EndDate", queryInfo.EditTime.Value.AddDays(1));
            }
            if (queryInfo.EditType.HasValue)
            {
                QUERY_SQL += " AND p.DataSource=@EditType";
                parameters.Append("EditType", queryInfo.EditType.Value);
            }
            if (!string.IsNullOrEmpty(queryInfo.BarCode))
            {
                Filter_SKU += " AND sku.BarCode=@BarCode";
                parameters.Append("BarCode", queryInfo.BarCode);
            }
            if (!string.IsNullOrEmpty(queryInfo.Sku))
            {
                Filter_SKU += " AND sku.Sku=@Sku";
                parameters.Append("Sku", queryInfo.Sku);
            }
            QUERY_SQL += @" AND EXISTS(select TOP 1 sku.id from SkuInfo_temp(NOLOCK) sku where sku.SpuId=p.Id" + Filter_SKU + ")";

            QUERY_SQL += ") pp ";
            string SQL = string.Format(@"select * from (select ROW_NUMBER() OVER(order by pp.ModifyTime desc) AS RowNum,* from {0}
										) as A where A.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;
                                        SELECT COUNT(1) FROM {0};", QUERY_SQL);

            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);

            DataSet ds = db.ExecuteSqlDataSet(SQL, parameters);

            return new PageOf<ProductTempModel>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                Items = DataMapHelper.DataSetToList<ProductTempModel>(ds)
            };
        }
        public String GetAuditProductReason(string spu, int status)
        {
            var QUERY_SQL = @"SELECT TOP 1 Reason FROM ProductAuditingLog(NOLOCK)
WHERE Spu=@Spu AND Status=@Status ORDER BY CreateTime DESC";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("Spu", spu);
            parameters.Append("Status", status);
            return db.ExecuteSqlScalar<String>(QUERY_SQL, parameters);
        }

        public IList<SkuTempModel> GetAuditSkuList(AuditProductListQueryInfo queryInfo, int spuId)
        {
            var QUERY_SQL = @"select p.Name,sku.sku,sku.BarCode,sku.[MainDicValue],sku.MainValue
                                ,sku.[SubDicValue],sku.SubValue,sku.Price,sku.QtyStatus,sku.Status,sku.IsOnSaled
                              from ProductInfo_temp(NOLOCK) p
	                          inner join SkuInfo_temp(NOLOCK) as sku on p.Id=sku.SpuId
                              where p.SupplierID=@SupplierID AND p.Id=@SpuId
                              AND sku.Status IN ( " + string.Join(",", queryInfo.SkuStatus.Select(p => p.ToString())) + ")";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("SupplierID", queryInfo.SupplierID);
            parameters.Append("SpuId", spuId);
            if (!string.IsNullOrEmpty(queryInfo.BarCode))
            {
                QUERY_SQL += " AND sku.BarCode=@BarCode";
                parameters.Append("BarCode", queryInfo.BarCode);
            }
            if (!string.IsNullOrEmpty(queryInfo.Sku))
            {
                QUERY_SQL += " AND sku.Sku=@Sku";
                parameters.Append("Sku", queryInfo.Sku);
            }

            DataSet ds = db.ExecuteSqlDataSet(QUERY_SQL, parameters);

            return DataMapHelper.DataSetToList<SkuTempModel>(ds);
        }

        public bool CancelEditProduct(string spu)
        {
            var sql = @"begin transaction 
declare @error int
set @error = 0
	if exists (select Spu from ProductInfo_Temp where [Status]=2)
	begin
		update k set [Status]=-3
		from SkuInfo_Temp k
		inner join ProductInfo_Temp p on k.Spu=p.Spu
		 where p.Spu = @spu AND p.[Status]=2
		update ProductInfo_Temp set [Status]=-3
		where Spu = @Spu AND [Status]=2
	end
set @error = @error + @@error	
if @error <> 0  
rollback transaction   
else   
commit transaction";
            try
            {
                var db = DbSFO2OMain;
                var parameters = db.CreateParameterCollection();
                parameters.Append("Spu", spu);
                return db.ExecuteSqlNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }

        public DataSet GetSkuInventoryList(InventoryListQueryInfo queryInfo, Models.LanguageEnum languageVersion, PageDTO page)
        {
            var QUERY_SQL = @"(SELECT p.Spu,p.Name ProductName,s.BarCode,s.Sku,s.MainValue,s.SubValue,ISNULL(t.ForOrderQty,0) Qty,s.AlarmStockQty,
CASE WHEN s.AlarmStockQty>ISNULL(t.ForOrderQty,0) THEN '是' ELSE '否' END AS IsLowStockAlarm,s.[Status]
FROM SkuInfo s
INNER JOIN ProductInfo p ON p.Id=s.SpuId
LEFT JOIN Stock t ON t.Sku=s.Sku WHERE p.SupplierID=@SupplierID AND p.LanguageVersion=@LanguageVersion";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            #region 查询条件

            StringBuilder query = new StringBuilder();
            parameters.Append("SupplierID", queryInfo.SupplierID);
            parameters.Append("LanguageVersion", (int)languageVersion);
            if (!string.IsNullOrEmpty(queryInfo.ProductName))
            {
                QUERY_SQL += " AND p.Name LIKE @ProductName";
                parameters.Append("ProductName", "%" + queryInfo.ProductName + "%");
            }
            if (!String.IsNullOrWhiteSpace(queryInfo.Spu))
            {
                query.Append(" AND p.Spu=@Spu");
                parameters.Append("Spu", queryInfo.Spu);
            }
            if (!String.IsNullOrWhiteSpace(queryInfo.Sku))
            {
                query.Append(" AND s.sku=@SKU");
                parameters.Append("SKU", queryInfo.Sku);
            }
            if (!String.IsNullOrWhiteSpace(queryInfo.BarCode))
            {
                query.Append(" AND s.BarCode = @BarCode");
                parameters.Append("BarCode", queryInfo.BarCode);
            }
            if (queryInfo.SkuStatus.HasValue)
            {
                query.Append(" AND s.Status=@SkuStatus");
                parameters.Append("SkuStatus", queryInfo.SkuStatus.Value);
            }
            else
            {
                query.Append(" AND s.Status IN (1,3,4,5)");
            }
            if (queryInfo.IsLowStockAlarm.HasValue)
            {
                if (queryInfo.IsLowStockAlarm.Value)
                {
                    query.Append(" AND s.AlarmStockQty>ISNULL(t.ForOrderQty,0)");
                }
                else
                {
                    query.Append(" AND s.AlarmStockQty<=ISNULL(t.ForOrderQty,0)");
                }
            }

            #endregion
            QUERY_SQL += query.ToString() + ") t ";
            string SQL = string.Format(@"select * from (select ROW_NUMBER() OVER(order by t.Spu,t.sku) AS RowNum,* from {0}
										) as A where A.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;
                                        SELECT COUNT(1),COUNT(distinct t.Spu) FROM {0};", QUERY_SQL);

            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);

            DataSet ds = db.ExecuteSqlDataSet(SQL, parameters);
            return ds;
        }

        /// <summary>
        /// 删除单个草稿
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool DeleteProductById(int productId, int userId)
        {
            var sql = @"update ProductInfo_Temp set status=-4,ModifyTime=@ModifyTime,ModifyBy=@ModifyBy
                        where id=@ProductId;";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("ProductId", productId);
            parameters.Append("ModifyTime", DateTime.Now);
            parameters.Append("ModifyBy", userId);

            return db.ExecuteSqlNonQuery(sql, parameters) > 0;
        }


        /// <summary>
        /// 删除全部草稿
        /// </summary>
        /// <param name="productIds"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteAllProduct(string productIds, int userId)
        {
            var sql = string.Format(@"
                        update ProductInfo_Temp set status=-4,ModifyTime=@ModifyTime,ModifyBy=@ModifyBy
                        where id in ({0})", productIds);

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("productId", productIds);
            parameters.Append("ModifyTime", DateTime.Now);
            parameters.Append("ModifyBy", userId);

            return db.ExecuteSqlNonQuery(sql, parameters) > 0;
        }


        /// <summary>
        /// 获取图片地址
        /// </summary>
        /// <param name="spu"></param>
        /// <returns></returns>
        public IList<ProductTempImageModel> GetProductImages(string spu)
        {
            var sql = @"SELECT * FROM ProductImage(NOLOCK) WHERE SPU=@Spu AND ImageType=1";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("Spu", spu);

            return db.ExecuteSqlList<ProductTempImageModel>(sql, parameters);
        }


        /// <summary>
        /// 获取图片地址
        /// </summary>
        /// <param name="spu"></param>
        /// <returns></returns>
        public IList<ProductTempImageModel> GetProductImageTemps(string spu)
        {
            var sql = @"SELECT * FROM ProductImage_Temp(NOLOCK) WHERE SPU=@Spu AND ImageType=1";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("Spu", spu);

            return db.ExecuteSqlList<ProductTempImageModel>(sql, parameters);
        }

        public long GetNewSpuOrSku(string startNo, string cType)
        {
            string sql = @"DECLARE @LastNum bigint,@Currdate int
                            SET @Currdate=CONVERT(varchar(10),GETDATE(),12)
	                        update FrameCode
	                        set @LastNum=LastNum=LastNum+1
	                        where Currdate=@Currdate
	                        AND CType=@CType
                            if(@@RowCount=0)
                            BEGIN                           
                            SET @LastNum=0
                            WHILE(len(@LastNum)!=6 or @LastNum>990000)
                            begin
                            select @LastNum =cast(ceiling(rand() * 1000000) as int)
                            end
                            insert INTO FrameCode VALUES(@Currdate,@CType,'',@LastNum)
                            end
                            SELECT @startNo+cast(@Currdate AS varchar(6))+Cast(@LastNum as varchar(6))";
            var db = DbSFO2OMain;
            var paras = db.CreateParameterCollection();
            paras.Append("startNo", startNo, DbType.String);
            paras.Append("CType", cType, DbType.String);

            var result = db.ExecuteSqlScalar<long>(sql, paras);
            return result;
        }

        public SupplierBrandModel GetBrandNameByIdAndSupplierId(int id, int supplierId)
        {
            var sql = @"SELECT * FROM SupplierBrand(NOLOCK) WHERE id=@Id AND SupplierID=@SupplierID";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("Id", id);
            parameters.Append("SupplierID", supplierId);

            return db.ExecuteSqlFirst<SupplierBrandModel>(sql, parameters);
        }

        public IList<CategoryAttrModel> GetCategoryAttrs(int categoryId)
        {
            var sql = @"WITH AttrName AS
                        (
	                        SELECT [Id],[KeyName],[KeyValue],[DicType],[LanguageVersion]
	                        FROM Dics(NOLOCK)
	                        WHERE LanguageVersion=2 AND DicType ='ProductAttributes'
                        )
                        SELECT ca.[Id],[CategoryId],ca.[KeyName],an.KeyValue,[IsRequire],[VerificationType]
                              ,[IsFilter],[IsSkuAttr],[IsSkuMainAttr],[IsShow],ca.[ShowType],[IsUnitAttr]
                              ,[InPutType],[MaxInput],[MaxItems]
                        FROM Category_Attributes(NOLOCK) AS ca INNER JOIN AttrName AS an
                        ON ca.KeyName = an.KeyName
                        WHERE CategoryId=@CategoryId
                        ORDER BY IsSkuAttr DESC ,IsSkuMainAttr DESC";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("CategoryId", categoryId);

            return db.ExecuteSqlList<CategoryAttrModel>(sql, parameters);
        }

        public void CreateProduct(Dictionary<LanguageEnum, ProductTempModel> spuBaseInfo, Dictionary<LanguageEnum, ProductExtTempModel> spuExInfo,
            Dictionary<LanguageEnum, List<SkuTempModel>> skuInfos, List<ProductTempImageModel> spuImags)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    foreach (LanguageEnum le in Enum.GetValues(typeof(LanguageEnum)))
                    {
                        var id = AddProductInfo(spuBaseInfo, db, tran, le);

                        AddProductExtendInfo(spuExInfo, db, tran, le, id);

                        AddSkus(skuInfos, db, tran, le, id);
                    }

                    if (null != spuImags && spuImags.Count > 0)
                    {
                        AddProductImages(spuBaseInfo, spuImags, db, tran);
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



        public void UpdateProduct(int spuSelectStatus, Dictionary<LanguageEnum, ProductTempModel> spuBaseInfo, Dictionary<LanguageEnum, ProductExtTempModel> spuExInfo,
            Dictionary<LanguageEnum, List<SkuTempModel>> skuInfos, List<ProductTempImageModel> spuImags)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    foreach (LanguageEnum le in Enum.GetValues(typeof(LanguageEnum)))
                    {

                        ProductTempModel spuInfo;
                        var id = ModifyProductInfo(spuSelectStatus, spuBaseInfo, db, tran, le, out spuInfo);

                        ModifyProductExtendInfo(spuExInfo, db, tran, le, id);

                        DeleteSkus(db, tran, spuInfo, id);
                        AddSkus(skuInfos, db, tran, le, id);
                    }

                    DeleteProductImages(spuBaseInfo, db, tran);

                    if (null != spuImags && spuImags.Count > 0)
                    {
                        AddProductImages(spuBaseInfo, spuImags, db, tran);
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
        /// 保存线上商品编辑后的信息
        /// </summary>
        /// <param name="spuBaseInfo"></param>
        /// <param name="spuExInfo"></param>
        /// <param name="skuInfos"></param>
        /// <param name="spuImags"></param>
        public void SaveOnlineProduct(Dictionary<LanguageEnum, ProductTempModel> spuBaseInfo, Dictionary<LanguageEnum, ProductExtTempModel> spuExInfo,
            Dictionary<LanguageEnum, List<SkuTempModel>> skuInfos, List<ProductTempImageModel> spuImags)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    CloseSpu(spuBaseInfo[LanguageEnum.TraditionalChinese].Spu, db, tran);

                    foreach (LanguageEnum le in Enum.GetValues(typeof(LanguageEnum)))
                    {
                        var id = AddProductInfo(spuBaseInfo, db, tran, le);

                        AddProductExtendInfo(spuExInfo, db, tran, le, id);

                        AddSkus(skuInfos, db, tran, le, id);
                    }

                    var sInfos = skuInfos[LanguageEnum.TraditionalChinese];
                    var skus = new List<string>();

                    foreach (var sku in sInfos)
                    {
                        skus.Add(sku.Sku);
                    }

                    if (skus.Count > 0)
                    {
                        MergeSkuCustomsReport(db, tran, skus);
                    }

                    DeleteProductImages(spuBaseInfo, db, tran);

                    if (null != spuImags && spuImags.Count > 0)
                    {
                        AddProductImages(spuBaseInfo, spuImags, db, tran);
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

        private static void ModifyProductExtendInfo(Dictionary<LanguageEnum, ProductExtTempModel> spuExInfo, Database db, System.Data.Common.DbTransaction tran, LanguageEnum le, int id)
        {
            var spuEx = spuExInfo[le];
            spuEx.SpuId = id;

            const string insertSpuEx = @"UPDATE [dbo].[ProductInfoExpand_Temp]
                           SET [Materials] = @Materials,[Pattern] = @Pattern,[Flavour] = @Flavour,[Ingredients] = @Ingredients,[StoragePeriod] = @StoragePeriod
                              ,[StoringTemperature] = @StoringTemperature,[SkinType] = @SkinType,[Gender] = @Gender,[AgeGroup] = @AgeGroup,[Model] = @Model
                              ,[BatteryTime] = @BatteryTime,[Voltage] = @Voltage,[Power] = @Power,[Warranty] = @Warranty,[SupportedLanguage] = @SupportedLanguage
                              ,[PetType] = @PetType,[PetAgeUnit] = @PetAgeUnit,[PetAge] = @PetAge,[Location] = @Location,[Weight] = @Weight
                              ,[WeightUnit] = @WeightUnit,[Volume] = @Volume,[VolumeUnit] = @VolumeUnit,[Length] = @Length,[LengthUnit] = @LengthUnit
                              ,[Width] = @Width,[WidthUnit] = @WidthUnit,[Height] = @Height,[HeightUnit] = @HeightUnit,[Flavor] = @Flavor
                         WHERE SpuId=@SpuId";
            var spuExParas = db.CreateParameterCollection();
            spuExParas.Append("@SpuId", id);
            spuExParas.Append("@Materials", spuEx.Materials);
            spuExParas.Append("@Pattern", spuEx.Pattern);
            spuExParas.Append("@Flavour", spuEx.Flavour);
            spuExParas.Append("@Ingredients", spuEx.Ingredients);
            spuExParas.Append("@StoragePeriod", spuEx.StoragePeriod);
            spuExParas.Append("@StoringTemperature", spuEx.StoringTemperature);
            spuExParas.Append("@SkinType", spuEx.SkinType);
            spuExParas.Append("@Gender", spuEx.Gender);
            spuExParas.Append("@AgeGroup", spuEx.AgeGroup);
            spuExParas.Append("@Model", spuEx.Model);
            spuExParas.Append("@BatteryTime", spuEx.BatteryTime);
            spuExParas.Append("@Voltage", spuEx.Voltage);
            spuExParas.Append("@Power", spuEx.Power);
            spuExParas.Append("@Warranty", spuEx.Warranty);
            spuExParas.Append("@SupportedLanguage", spuEx.SupportedLanguage);
            spuExParas.Append("@PetType", spuEx.PetType);
            spuExParas.Append("@PetAgeUnit", spuEx.PetAgeUnit);
            spuExParas.Append("@PetAge", spuEx.PetAge);
            spuExParas.Append("@Location", spuEx.Location);
            spuExParas.Append("@Weight", spuEx.Weight);
            spuExParas.Append("@WeightUnit", spuEx.WeightUnit);
            spuExParas.Append("@Volume", spuEx.Volume);
            spuExParas.Append("@VolumeUnit", spuEx.VolumeUnit);
            spuExParas.Append("@Length", spuEx.Length);
            spuExParas.Append("@LengthUnit", spuEx.LengthUnit);
            spuExParas.Append("@Width", spuEx.Width);
            spuExParas.Append("@WidthUnit", spuEx.WidthUnit);
            spuExParas.Append("@Height", spuEx.Height);
            spuExParas.Append("@HeightUnit", spuEx.HeightUnit);
            spuExParas.Append("@Flavor", spuEx.Flavor);
            db.ExecuteNonQuery(CommandType.Text, insertSpuEx, spuExParas, tran);
        }

        private static int ModifyProductInfo(int spuSelectStatus, Dictionary<LanguageEnum, ProductTempModel> spuBaseInfo, Database db, System.Data.Common.DbTransaction tran, LanguageEnum le, out ProductTempModel spuInfo)
        {
            spuInfo = spuBaseInfo[le];
            const string insertSpu = @"DECLARE @TMP TABLE(ID INT)  
                           UPDATE [dbo].[ProductInfo_Temp]
                           SET [Name] = @Name,[Tag] = @Tag,[Price] = @Price,[Description] = @Description,[BrandId] = @BrandId,[Brand] = @Brand,[CountryOfManufacture] = @CountryOfManufacture
                              ,[SalesTerritory] = @SalesTerritory,[Unit] = @Unit,[IsExchangeInCHINA] = @IsExchangeInCHINA,[IsExchangeInHK] = @IsExchangeInHK
                              ,[IsReturn] = @IsReturn,[MinForOrder] = @MinForOrder,[MinPrice] = @MinPrice,[NetWeight] = @NetWeight,[NetWeightUnit] = @NetWeightUnit
                              ,[NetContent] = @NetContent,[NetContentUnit] = @NetContentUnit,[IsDutyOnSeller] = @IsDutyOnSeller,[Status] = @StatusNew
                              ,[DataSource] = @DataSource,[ModifyTime] = @ModifyTime,[ModifyBy] = @ModifyBy,[PreOnSaleTime] = @PreOnSaleTime
                              ,[CommissionInCHINA] = @CommissionInCHINA,[CommissionInHK] = @CommissionInHK
                         OUTPUT inserted.Id
                         INTO @TMP
                         WHERE Spu=@Spu AND (Status=-2 OR Status=2) AND LanguageVersion=@LanguageVersion
                         SELECT @SpuId=ID FROM @TMP";

            var spuParas = db.CreateParameterCollection();

            spuParas.Append("Spu", spuInfo.Spu);
            spuParas.Append("LanguageVersion", spuInfo.LanguageVersion);
            //spuParas.Append("Status", spuSelectStatus);

            spuParas.Append("Name", spuInfo.Name);
            spuParas.Append("Tag", spuInfo.Tag);
            spuParas.Append("Price", spuInfo.Price);

            spuParas.Append("Description", spuInfo.Description);
            spuParas.Append("Brand", spuInfo.Brand);
            spuParas.Append("BrandId", spuInfo.BrandId);
            spuParas.Append("CountryOfManufacture", spuInfo.CountryOfManufacture);

            spuParas.Append("SalesTerritory", spuInfo.SalesTerritory);
            spuParas.Append("Unit", spuInfo.Unit);
            spuParas.Append("IsExchangeInCHINA", spuInfo.IsExchangeInCHINA);

            spuParas.Append("IsExchangeInHK", spuInfo.IsExchangeInHK);
            spuParas.Append("IsReturn", spuInfo.IsReturn);
            spuParas.Append("MinForOrder", spuInfo.MinForOrder);

            spuParas.Append("MinPrice", spuInfo.MinPrice);

            spuParas.Append("NetContent", spuInfo.NetContent);
            spuParas.Append("NetContentUnit", spuInfo.NetContentUnit);

            spuParas.Append("NetWeight", spuInfo.NetWeight);
            spuParas.Append("NetWeightUnit", spuInfo.NetWeightUnit);

            spuParas.Append("IsDutyOnSeller", spuInfo.IsDutyOnSeller);

            spuParas.Append("ModifyTime", spuInfo.ModifyTime);
            spuParas.Append("ModifyBy", spuInfo.ModifyBy);

            spuParas.Append("PreOnSaleTime", spuInfo.PreOnSaleTime);
            spuParas.Append("StatusNew", spuInfo.Status);
            spuParas.Append("DataSource", spuInfo.DataSource);

            spuParas.Append("CommissionInCHINA", spuInfo.CommissionInCHINA);
            spuParas.Append("CommissionInHK", spuInfo.CommissionInHK);

            spuParas.Append("@SpuId", 0, DbType.Int32, ParameterDirection.InputOutput);
            db.ExecuteNonQuery(CommandType.Text, insertSpu, spuParas, tran);
            var id = 0;
            int.TryParse(spuParas["@SpuId"].Value.ToString(), out id);
            return id;
        }

        private static void CloseSpu(string spu, Database db, System.Data.Common.DbTransaction tran)
        {
            const string insertSpu = @"
                           UPDATE [dbo].[ProductInfo_Temp] SET Status=-3 WHERE Spu=@Spu AND Status <>-3";

            var spuParas = db.CreateParameterCollection();

            spuParas.Append("Spu", spu);

            db.ExecuteNonQuery(CommandType.Text, insertSpu, spuParas, tran);
        }

        private static void DeleteProductImages(Dictionary<LanguageEnum, ProductTempModel> spuBaseInfo, Database db, System.Data.Common.DbTransaction tran)
        {
            const string deleteProductImage = @"DELETE FROM ProductImage_Temp WHERE Spu=@Spu AND ImageType=1";
            var deleProductImagePars = db.CreateParameterCollection();

            deleProductImagePars.Append("@Spu", spuBaseInfo[LanguageEnum.TraditionalChinese].Spu);
            db.ExecuteNonQuery(CommandType.Text, deleteProductImage, deleProductImagePars, tran);
        }

        private static void DeleteSkus(Database db, System.Data.Common.DbTransaction tran, ProductTempModel spuInfo, int id)
        {
            const string deleteSku = @"DELETE FROM SkuInfo_Temp WHERE SpuId=@SpuId AND Spu=@Spu AND Status <> -3 ";
            var deleSkuPars = db.CreateParameterCollection();

            deleSkuPars.Append("@SpuId", id);
            deleSkuPars.Append("@Spu", spuInfo.Spu);
            db.ExecuteNonQuery(CommandType.Text, deleteSku, deleSkuPars, tran);
        }

        private static void AddProductImages(Dictionary<LanguageEnum, ProductTempModel> spuBaseInfo, List<ProductTempImageModel> spuImags, Database db, System.Data.Common.DbTransaction tran)
        {
            const string insertProductImage = @"INSERT INTO dbo.ProductImage_Temp(SPU,ImagePath,ImageType,SortValue,CreateTime,Createby)
                               VALUES";
            StringBuilder sb = new StringBuilder(1024);
            int index = 0;
            var imgParas = db.CreateParameterCollection();

            foreach (var img in spuImags)
            {
                sb.Append("(@SPU" + index + ",@ImagePath" + index + ",@ImageType" + index + ",@SortValue" + index + ",@CreateTime" + index + ",@Createby" + index + "),");

                imgParas.Append("@SPU" + index, img.SPU);
                imgParas.Append("@ImagePath" + index, img.ImagePath);
                imgParas.Append("@ImageType" + index, img.ImageType);
                imgParas.Append("@SortValue" + index, img.SortValue);
                imgParas.Append("@CreateTime" + index, DateTime.Now);
                imgParas.Append("@Createby" + index, spuBaseInfo[LanguageEnum.TraditionalChinese].CreateBy);

                index++;
            }

            db.ExecuteNonQuery(CommandType.Text, insertProductImage + sb.Remove(sb.Length - 1, 1).ToString(), imgParas, tran);
        }

        private static void AddSkus(Dictionary<LanguageEnum, List<SkuTempModel>> skuInfos, Database db, System.Data.Common.DbTransaction tran, LanguageEnum le, int id)
        {
            const string insertSku = @"INSERT INTO dbo.SkuInfo_Temp(SpuId,Spu,Sku,MainDicKey,MainDicValue,SubDicKey,SubDicValue,MainKey,MainValue,SubKey,SubValue
                                ,Price,NetWeight,NetContent,Specifications,Size,Color,AlcoholPercentage,Smell,CapacityRestriction,BarCode,AlarmStockQty
                                ,IsOnSaled,Status,CreateTime,ReportStatus,DataSource)
                                VALUES";

            var skuParas = db.CreateParameterCollection();
            var skus = skuInfos[le];
            var skuValues = "";
            var index = 0;

            if (null == skus)
            {
                return;
            }

            foreach (var sku in skus)
            {
                sku.SpuId = id;

                skuValues += "(@SpuId" + index + ",@Spu" + index + ",@Sku" + index + ",@MainDicKey" + index + ",@MainDicValue" + index + ",@SubDicKey" + index + ",@SubDicValue" + index + ",@MainKey" + index + ",@MainValue" + index + ",@SubKey" + index + ",@SubValue" + index + ",@Price" + index + ",@NetWeight" + index + ",@NetContent" + index + ",@Specifications" + index + ",@Size" + index + ",@Color" + index + ",@AlcoholPercentage" + index + ",@Smell" + index + ",@CapacityRestriction" + index + ",@BarCode" + index + ",@AlarmStockQty" + index + ",@IsOnSaled" + index + ",@Status" + index + ",@CreateTime" + index + ",@ReportStatus" + index + ",@DataSource" + index + "),";

                skuParas.Append("@SpuId" + index, sku.SpuId);
                skuParas.Append("@Spu" + index, sku.Spu);
                skuParas.Append("@Sku" + index, sku.Sku);
                skuParas.Append("@MainDicKey" + index, sku.MainDicKey);
                skuParas.Append("@MainDicValue" + index, sku.MainDicValue);
                skuParas.Append("@SubDicKey" + index, sku.SubDicKey);
                skuParas.Append("@SubDicValue" + index, sku.SubDicValue);
                skuParas.Append("@MainKey" + index, sku.MainKey);
                skuParas.Append("@MainValue" + index, sku.MainValue);
                skuParas.Append("@SubKey" + index, sku.SubKey);
                skuParas.Append("@SubValue" + index, sku.SubValue);
                skuParas.Append("@Price" + index, sku.Price);
                skuParas.Append("@NetWeight" + index, sku.NetWeight);
                skuParas.Append("@NetContent" + index, sku.NetContent);
                skuParas.Append("@Specifications" + index, sku.Specifications);
                skuParas.Append("@Size" + index, sku.Size);
                skuParas.Append("@Color" + index, sku.Color);
                skuParas.Append("@AlcoholPercentage" + index, sku.AlcoholPercentage);
                skuParas.Append("@Smell" + index, sku.Smell);
                skuParas.Append("@CapacityRestriction" + index, sku.CapacityRestriction);
                skuParas.Append("@BarCode" + index, sku.BarCode);
                skuParas.Append("@AlarmStockQty" + index, sku.AlarmStockQty);
                skuParas.Append("@IsOnSaled" + index, sku.IsOnSaled);
                skuParas.Append("@Status" + index, sku.Status);
                skuParas.Append("@CreateTime" + index, sku.CreateTime);
                skuParas.Append("@ReportStatus" + index, sku.ReportStatus);
                skuParas.Append("@DataSource" + index, sku.DataSource);

                index++;
            }

            db.ExecuteNonQuery(CommandType.Text, insertSku + skuValues.TrimEnd(','), skuParas, tran);
        }

        private static void AddProductExtendInfo(Dictionary<LanguageEnum, ProductExtTempModel> spuExInfo, Database db, System.Data.Common.DbTransaction tran, LanguageEnum le, int id)
        {
            var spuEx = spuExInfo[le];
            spuEx.SpuId = id;

            const string insertSpuEx = @"INSERT INTO dbo.ProductInfoExpand_Temp(SpuId,Materials,Pattern,Flavour,Ingredients,StoragePeriod,StoringTemperature
                                ,SkinType,Gender,AgeGroup,Model,BatteryTime,Voltage,Power,Warranty,SupportedLanguage,PetType,PetAgeUnit,PetAge,Location,Weight
                                ,WeightUnit,Volume,VolumeUnit,Length,LengthUnit,Width,WidthUnit,Height,HeightUnit,Flavor)
                                VALUES
                                (@SpuId,@Materials,@Pattern,@Flavour,@Ingredients,@StoragePeriod,@StoringTemperature,@SkinType,@Gender,@AgeGroup,@Model
                                ,@BatteryTime,@Voltage,@Power,@Warranty,@SupportedLanguage,@PetType,@PetAgeUnit,@PetAge,@Location,@Weight,@WeightUnit
                                ,@Volume,@VolumeUnit,@Length,@LengthUnit,@Width,@WidthUnit,@Height,@HeightUnit,@Flavor)";

            var spuExParas = db.CreateParameterCollection();
            spuExParas.Append("@SpuId", spuEx.SpuId);
            spuExParas.Append("@Materials", spuEx.Materials);
            spuExParas.Append("@Pattern", spuEx.Pattern);
            spuExParas.Append("@Flavour", spuEx.Flavour);
            spuExParas.Append("@Ingredients", spuEx.Ingredients);
            spuExParas.Append("@StoragePeriod", spuEx.StoragePeriod);
            spuExParas.Append("@StoringTemperature", spuEx.StoringTemperature);
            spuExParas.Append("@SkinType", spuEx.SkinType);
            spuExParas.Append("@Gender", spuEx.Gender);
            spuExParas.Append("@AgeGroup", spuEx.AgeGroup);
            spuExParas.Append("@Model", spuEx.Model);
            spuExParas.Append("@BatteryTime", spuEx.BatteryTime);
            spuExParas.Append("@Voltage", spuEx.Voltage);
            spuExParas.Append("@Power", spuEx.Power);
            spuExParas.Append("@Warranty", spuEx.Warranty);
            spuExParas.Append("@SupportedLanguage", spuEx.SupportedLanguage);
            spuExParas.Append("@PetType", spuEx.PetType);
            spuExParas.Append("@PetAgeUnit", spuEx.PetAgeUnit);
            spuExParas.Append("@PetAge", spuEx.PetAge);
            spuExParas.Append("@Location", spuEx.Location);
            spuExParas.Append("@Weight", spuEx.Weight);
            spuExParas.Append("@WeightUnit", spuEx.WeightUnit);
            spuExParas.Append("@Volume", spuEx.Volume);
            spuExParas.Append("@VolumeUnit", spuEx.VolumeUnit);
            spuExParas.Append("@Length", spuEx.Length);
            spuExParas.Append("@LengthUnit", spuEx.LengthUnit);
            spuExParas.Append("@Width", spuEx.Width);
            spuExParas.Append("@WidthUnit", spuEx.WidthUnit);
            spuExParas.Append("@Height", spuEx.Height);
            spuExParas.Append("@HeightUnit", spuEx.HeightUnit);
            spuExParas.Append("@Flavor", spuEx.Flavor);

            db.ExecuteNonQuery(CommandType.Text, insertSpuEx, spuExParas, tran);
        }

        private static int AddProductInfo(Dictionary<LanguageEnum, ProductTempModel> spuBaseInfo, Database db, System.Data.Common.DbTransaction tran, LanguageEnum le)
        {
            var spuInfo = spuBaseInfo[le];
            const string insertSpu = @"INSERT INTO [dbo].[ProductInfo_Temp]([Spu],[CategoryId],[SupplierId],[Name],[Tag],[Price],[Description],[Brand],[BrandId]
                                    ,[CountryOfManufacture],[SalesTerritory],[Unit],[IsExchangeInCHINA],[IsExchangeInHK],[IsReturn],[MinForOrder],[MinPrice]
                                    ,[NetWeight],[NetWeightUnit],[NetContent],[NetContentUnit],[IsDutyOnSeller],[LanguageVersion],[Createtime],[CreateBy],[PreOnSaleTime]
                                    ,[Status],[DataSource],[ModifyTime],[ModifyBy],[CommissionInCHINA],[CommissionInHK])
                                    VALUES
                                    (@SPU,@CategoryID,@SupplierId,@Name,@Tag,@Price,@Description,@Brand,@BrandId,@CountryOfManufacture,@SalesTerritory,@Unit,@IsExchangeInCHINA,
                                    @IsExchangeInHK,@IsReturn,@MinForOrder,@MinPrice,@NetWeight,@NetWeightUnit,@NetContent,@NetContentUnit,@IsDutyOnSeller,@LanguageVersion,@Createtime,@CreateBy
                                    ,@PreOnSaleTime,@Status,@DataSource,@ModifyTime,@ModifyBy,@CommissionInCHINA,@CommissionInHK)
                                    SET @SpuId = @@IDENTITY";

            var spuParas = db.CreateParameterCollection();

            spuParas.Append("SPU", spuInfo.Spu);
            spuParas.Append("CategoryID", spuInfo.CategoryId);
            spuParas.Append("SupplierId", spuInfo.SupplierId);

            spuParas.Append("Name", spuInfo.Name);
            spuParas.Append("Tag", spuInfo.Tag);
            spuParas.Append("Price", spuInfo.Price);

            spuParas.Append("Description", spuInfo.Description);
            spuParas.Append("Brand", spuInfo.Brand);
            spuParas.Append("BrandId", spuInfo.BrandId);
            spuParas.Append("CountryOfManufacture", spuInfo.CountryOfManufacture);

            spuParas.Append("SalesTerritory", spuInfo.SalesTerritory);
            spuParas.Append("Unit", spuInfo.Unit);
            spuParas.Append("IsExchangeInCHINA", spuInfo.IsExchangeInCHINA);

            spuParas.Append("IsExchangeInHK", spuInfo.IsExchangeInHK);
            spuParas.Append("IsReturn", spuInfo.IsReturn);
            spuParas.Append("MinForOrder", spuInfo.MinForOrder);

            spuParas.Append("MinPrice", spuInfo.MinPrice);

            spuParas.Append("NetContent", spuInfo.NetContent);
            spuParas.Append("NetContentUnit", spuInfo.NetContentUnit);

            spuParas.Append("NetWeight", spuInfo.NetWeight);
            spuParas.Append("NetWeightUnit", spuInfo.NetWeightUnit);

            spuParas.Append("IsDutyOnSeller", spuInfo.IsDutyOnSeller);
            spuParas.Append("LanguageVersion", spuInfo.LanguageVersion);

            spuParas.Append("Createtime", spuInfo.Createtime);
            spuParas.Append("CreateBy", spuInfo.CreateBy);

            spuParas.Append("ModifyTime", spuInfo.ModifyTime);
            spuParas.Append("ModifyBy", spuInfo.ModifyBy);

            spuParas.Append("PreOnSaleTime", spuInfo.PreOnSaleTime);

            spuParas.Append("Status", spuInfo.Status);
            spuParas.Append("DataSource", spuInfo.DataSource);

            spuParas.Append("CommissionInCHINA", spuInfo.CommissionInCHINA);
            spuParas.Append("CommissionInHK", spuInfo.CommissionInHK);

            spuParas.Append("@SpuId", 0, DbType.Int32, ParameterDirection.InputOutput);
            db.ExecuteNonQuery(CommandType.Text, insertSpu, spuParas, tran);
            var id = 0;
            int.TryParse(spuParas["@SpuId"].Value.ToString(), out id);
            return id;
        }

        /// <summary>
        /// 获取线上ProductInfo表
        /// </summary>
        /// <param name="spu"></param>
        /// <returns></returns>
        public Dictionary<LanguageEnum, ProductTempModel> GetReleasedProductInfo(string spu, int supplierId)
        {
            string sql = @"
             SELECT p.[Id],[Spu],p.[CategoryId],p.[SupplierId],[Name],[Tag],[Price],[Description],
		            (case when LanguageVersion =1 then sb.NameCN
                    else case when LanguageVersion = 2  then sb.NameHK
                    else case when LanguageVersion = 3 then sb.NameEN else '' end end end) as [Brand],p.BrandId BrandId,
                    [CountryOfManufacture],[SalesTerritory],[Unit],[IsExchangeInCHINA],[IsExchangeInHK],[IsReturn],[MinForOrder],[MinPrice],[NetWeight],[NetWeightUnit],
                    [NetContent],[NetContentUnit],[IsDutyOnSeller],[LanguageVersion],p.[Createtime],[CreateBy],[PreOnSaleTime],[CommissionInCHINA],[CommissionInHK]
            FROM [dbo].[ProductInfo](NOLOCK) p 
            left join [SupplierBrand](NOLOCK) sb on p.BrandId=sb.Id
            WHERE p.Spu=@Spu and p.SupplierId=@SupplierId";
            var db = DbSFO2OMain;
            var paras = db.CreateParameterCollection();
            paras.Append("Spu", spu, DbType.String);
            paras.Append("SupplierId", supplierId, DbType.Int16);

            var result = db.ExecuteSqlList<ProductTempModel>(sql, paras);

            var ptms = new Dictionary<LanguageEnum, ProductTempModel>();

            if (null != result && result.Count > 0)
            {
                foreach (var ptm in result)
                {
                    LanguageEnum languageVersion = (LanguageEnum)ptm.LanguageVersion;
                    if (!ptms.ContainsKey(languageVersion))
                    {
                        ptms.Add(languageVersion, ptm);
                    }
                }
            }

            return ptms;
        }

        /// <summary>
        /// 获取线上ProductInfoExpand表
        /// </summary>
        /// <param name="spu"></param>
        /// <returns></returns>
        public Dictionary<LanguageEnum, ProductExtTempModel> GetReleasedProductExtendInfo(string spu)
        {
            string sql = @"
            SELECT [Materials],[Pattern],[Flavour],[Ingredients],[StoragePeriod],[StoringTemperature],[SkinType],[Flavor]
                ,[Gender],[AgeGroup],[Model],[BatteryTime],[Voltage],[Power],[Warranty],[SupportedLanguage],[PetType],[PetAge],[PetAgeUnit]
                ,[Location],[Weight],[WeightUnit],[Volume],[VolumeUnit],[Length],[LengthUnit],[Width],[WidthUnit],[Height],[HeightUnit],[LanguageVersion]
            FROM [ProductInfoExpand](NOLOCK) AS pie
            INNER JOIN ProductInfo(NOLOCK) AS pin
            ON pie.SpuId = pin.Id 
            WHERE pin.Spu=@Spu";
            var db = DbSFO2OMain;
            var paras = db.CreateParameterCollection();
            paras.Append("Spu", spu, DbType.String);

            var result = db.ExecuteSqlList<ProductExtTempModel>(sql, paras);

            var ptmExs = new Dictionary<LanguageEnum, ProductExtTempModel>();

            if (null != result && result.Count > 0)
            {
                foreach (var ptmEx in result)
                {
                    LanguageEnum languageVersion = (LanguageEnum)ptmEx.LanguageVersion;
                    if (!ptmExs.ContainsKey(languageVersion))
                    {
                        ptmExs.Add(languageVersion, ptmEx);
                    }
                }
            }

            return ptmExs;
        }

        /// <summary>
        /// 获取线上SKU表
        /// </summary>
        /// <param name="spu"></param>
        /// <returns></returns>
        public Dictionary<LanguageEnum, List<SkuTempModel>> GetReleasedSkuInfo(string spu)
        {
            string sql = @"
            SELECT [SpuId],si.[Spu],[Sku],[MainDicKey],[MainDicValue],[SubDicKey],[SubDicValue],[MainKey],[MainValue],[SubKey]
                ,[SubValue],si.[NetWeight],si.[NetContent],[Specifications],[Size],[Color],[AlcoholPercentage],[Smell],[CapacityRestriction]
                ,si.[Price],[BarCode],[AlarmStockQty],si.[CreateTime],[IsOnSaled],[Status],[ReportStatus],[LanguageVersion]
            FROM [dbo].[SkuInfo](NOLOCK) AS si
            INNER JOIN ProductInfo(NOLOCK) AS pin
            ON si.SpuId = pin.Id
            WHERE si.Spu=@Spu";
            var db = DbSFO2OMain;
            var paras = db.CreateParameterCollection();
            paras.Append("Spu", spu, DbType.String);

            var result = db.ExecuteSqlList<SkuTempModel>(sql, paras);

            var skus = new Dictionary<LanguageEnum, List<SkuTempModel>>();

            var sku_T = new List<SkuTempModel>();
            var sku_S = new List<SkuTempModel>();
            var sku_E = new List<SkuTempModel>();

            if (null != result && result.Count > 0)
            {
                foreach (var sku in result)
                {
                    LanguageEnum languageVersion = (LanguageEnum)sku.LanguageVersion;

                    switch (languageVersion)
                    {
                        case LanguageEnum.TraditionalChinese:
                            sku_T.Add(sku);
                            break;
                        case LanguageEnum.SimplifiedChinese:
                            sku_S.Add(sku);
                            break;
                        case LanguageEnum.English:
                            sku_E.Add(sku);
                            break;
                    }
                }
            }

            skus.Add(LanguageEnum.TraditionalChinese, sku_T);
            skus.Add(LanguageEnum.SimplifiedChinese, sku_S);
            skus.Add(LanguageEnum.English, sku_E);

            return skus;
        }

        /// <summary>
        /// 获取ProductInfo_Temp表
        /// </summary>
        /// <param name="spu"></param>
        /// <returns></returns>
        public Dictionary<LanguageEnum, ProductTempModel> GetTempProductInfo(string spu, int supplierId, int status)
        {
            string sql = @"
            SELECT p.[Id],[Spu],p.[CategoryId],p.[SupplierId],[Name],[Tag],[Price],[Description],(case when LanguageVersion =1 then sb.NameCN
                    else case when LanguageVersion = 2  then sb.NameHK
                    else case when LanguageVersion = 3 then sb.NameEN else '' end end end) as [Brand],[CountryOfManufacture],[SalesTerritory]
                  ,[Unit],[IsExchangeInCHINA],[IsExchangeInHK],[IsReturn],[MinForOrder],[MinPrice],[NetWeight],[NetWeightUnit],[NetContent],[NetContentUnit]
                  ,[IsDutyOnSeller],[LanguageVersion],p.[Status],[DataSource],p.[Createtime],[CreateBy],[ModifyTime],[ModifyBy],[AuditingTime],[AuditingBy]
                  ,[PreOnSaleTime],[CommissionInCHINA],[CommissionInHK]
            FROM [dbo].[ProductInfo_Temp](NOLOCK) p 
            left join [SupplierBrand](NOLOCK) sb on p.BrandId=sb.Id
            WHERE p.Spu=@Spu AND p.SupplierId= @SupplierId AND p.Status=@Status";
            var db = DbSFO2OMain;
            var paras = db.CreateParameterCollection();
            paras.Append("Spu", spu, DbType.String);
            paras.Append("SupplierId", supplierId);
            paras.Append("Status", status);

            var result = db.ExecuteSqlList<ProductTempModel>(sql, paras);

            var ptms = new Dictionary<LanguageEnum, ProductTempModel>();

            if (null != result && result.Count > 0)
            {
                foreach (var ptm in result)
                {
                    LanguageEnum languageVersion = (LanguageEnum)ptm.LanguageVersion;
                    if (!ptms.ContainsKey(languageVersion))
                    {
                        ptms.Add(languageVersion, ptm);
                    }
                }
            }

            return ptms;
        }

        /// <summary>
        /// 获取ProductInfoExpand_Temp表
        /// </summary>
        /// <param name="spuIds"></param>
        /// <returns></returns>
        public Dictionary<LanguageEnum, ProductExtTempModel> GetTempProductExtendInfo(int[] spuIds)
        {
            string sql = @"
                SELECT [SpuId],[Materials],[Pattern],[Flavour],[Ingredients],[StoragePeriod],[StoringTemperature],[SkinType],[Gender],[Flavor]
                       ,[AgeGroup],[Model],[BatteryTime],[Voltage],[Power],[Warranty],[SupportedLanguage],[PetType],[PetAgeUnit],[PetAge],[Location],[Weight]
                       ,[WeightUnit],[Volume],[VolumeUnit],[Length],[LengthUnit],[Width],[WidthUnit],[Height],[HeightUnit],[LanguageVersion]
                FROM [dbo].[ProductInfoExpand_Temp](NOLOCK)pit
                INNER JOIN ProductInfo_Temp(NOLOCK) AS pin
                ON pit.SpuId = pin.Id 
                WHERE pit.spuid IN";

            var db = DbSFO2OMain;
            var paras = db.CreateParameterCollection();
            StringBuilder sb = new StringBuilder(100);

            int i = 0;
            foreach (var sId in spuIds)
            {
                sb.Append("@SpuId" + i);
                sb.Append(",");
                paras.Append("SpuId" + i, sId);

                i++;
            }

            if (sb.Length > 0)
            {
                sql = sql + "(" + sb.Remove(sb.Length - 1, 1).ToString() + ")";
            }
            else
            {
                sql = sql + "('')";
            }

            var result = db.ExecuteSqlList<ProductExtTempModel>(sql, paras);

            var ptmExs = new Dictionary<LanguageEnum, ProductExtTempModel>();

            if (null != result && result.Count > 0)
            {
                foreach (var ptmEx in result)
                {
                    LanguageEnum languageVersion = (LanguageEnum)ptmEx.LanguageVersion;
                    if (!ptmExs.ContainsKey(languageVersion))
                    {
                        ptmExs.Add(languageVersion, ptmEx);
                    }
                }
            }

            return ptmExs;
        }


        /// <summary>
        /// 获取SKU_Temp表
        /// </summary>
        /// <param name="spu"></param>
        /// <returns></returns>
        public Dictionary<LanguageEnum, List<SkuTempModel>> GetTempSkuInfo(string spu, int[] spuIds)
        {
            string sql = @"
            SELECT [SpuId],sit.[Spu],[Sku],[MainDicKey],[MainDicValue],[SubDicKey],[SubDicValue],[MainKey],[MainValue],[SubKey]
                   ,[SubValue],sit.[NetWeight],sit.[NetContent],[Specifications],[Size],[Color],[AlcoholPercentage]
                   ,[Smell],[CapacityRestriction],sit.[Price],[BarCode],[AlarmStockQty],sit.[CreateTime],[AuditTime],[ShelvesTime]
                   ,[RemovedTime],[IsOnSaled],sit.[Status],[QtyStatus],[ReportStatus],pin.LanguageVersion
            FROM [dbo].[SkuInfo_Temp](NOLOCK) sit
            INNER JOIN ProductInfo_Temp(NOLOCK) AS pin
            ON sit.SpuId = pin.Id
            WHERE sit.Spu=@Spu AND spuid IN";
            var db = DbSFO2OMain;
            var paras = db.CreateParameterCollection();

            paras.Append("Spu", spu, DbType.String);
            StringBuilder sb = new StringBuilder(100);

            int i = 0;
            foreach (var sId in spuIds)
            {
                sb.Append("@SpuId" + i);
                sb.Append(",");
                paras.Append("SpuId" + i, sId);

                i++;
            }

            if (sb.Length > 0)
            {
                sql = sql + "(" + sb.Remove(sb.Length - 1, 1).ToString() + ")";
            }
            else
            {
                sql = sql + "('')";
            }

            var result = db.ExecuteSqlList<SkuTempModel>(sql, paras);
            var skus = new Dictionary<LanguageEnum, List<SkuTempModel>>();

            var sku_T = new List<SkuTempModel>();
            var sku_S = new List<SkuTempModel>();
            var sku_E = new List<SkuTempModel>();

            if (null != result && result.Count > 0)
            {
                foreach (var sku in result)
                {
                    LanguageEnum languageVersion = (LanguageEnum)sku.LanguageVersion;

                    switch (languageVersion)
                    {
                        case LanguageEnum.TraditionalChinese:
                            sku_T.Add(sku);
                            break;
                        case LanguageEnum.SimplifiedChinese:
                            sku_S.Add(sku);
                            break;
                        case LanguageEnum.English:
                            sku_E.Add(sku);
                            break;
                    }
                }
            }

            skus.Add(LanguageEnum.TraditionalChinese, sku_T);
            skus.Add(LanguageEnum.SimplifiedChinese, sku_S);
            skus.Add(LanguageEnum.English, sku_E);

            return skus;
        }

        private static void MergeSkuCustomsReport(Database db, System.Data.Common.DbTransaction tran, List<string> skus)
        {
            const string deleteSku = @"DELETE FROM SkuCustomsReport_Temp WHERE Sku IN ";
            const string mergeSku = @"INSERT INTO SkuCustomsReport_Temp([Sku],[CustomsUnit],[InspectionNo],[HSCode],[UOM],[PrepardNo],[GnoCode],[TaxRate],[TaxCode],[ModelForCustoms])
                                        (SELECT [Sku],[CustomsUnit],[InspectionNo],[HSCode],[UOM],[PrepardNo],[GnoCode],[TaxRate],[TaxCode],[ModelForCustoms] FROM SkuCustomsReport(NOLOCK)
                                        WHERE Sku IN ";

            var cParas = db.CreateParameterCollection();
            var mParas = db.CreateParameterCollection();
            StringBuilder sb = new StringBuilder(100);

            int i = 0;
            foreach (var sku in skus)
            {
                sb.Append("@Sku" + i);
                sb.Append(",");

                cParas.Append("@Sku" + i, sku, DbType.String);
                mParas.Append("@Sku" + i, sku, DbType.String);
                i++;
            }

            var deleSql = deleteSku + "(" + sb.Remove(sb.Length - 1, 1).ToString() + ")";
            var mSql = mergeSku + "(" + sb.ToString() + "))";

            db.ExecuteNonQuery(CommandType.Text, deleSql, cParas, tran);
            db.ExecuteNonQuery(CommandType.Text, mSql, mParas, tran);
        }


        public List<ProductImgModel> GetUnreleasedProudctImg(string spu)
        {
            var sql = @"select * from ProductImage_Temp(NOLOCK) WHERE Spu=@Spu";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("Spu", spu);

            return db.ExecuteSqlList<ProductImgModel>(sql, parameters).ToList();
        }

        public List<ProductImgModel> GetOnlineProudctImg(string spu)
        {
            var sql = @"select * from ProductImage(NOLOCK) WHERE Spu=@Spu";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("Spu", spu);

            return db.ExecuteSqlList<ProductImgModel>(sql, parameters).ToList();
        }

        public bool IsBarCodeRepeat(string barcode, string spu)
        {
            string[] arrBarCodes = null;
            string strQueryinfo = string.Empty;
            if (!string.IsNullOrEmpty(barcode))
            {
                arrBarCodes = barcode.Split(',');
            }
            foreach (var code in arrBarCodes)
            {
                if (!string.IsNullOrEmpty(code))
                {
                    strQueryinfo += "'" + code + "',";
                }
            }
            strQueryinfo = strQueryinfo.TrimEnd(',');

            var sql = string.Format(@"select count(1)
                                      from (select spu,barcode from SkuInfo_temp(NOLOCK)  where Status >-3
                                      union all 
                                      select spu,barcode from SkuInfo(NOLOCK)  where Status >-3) a
                                      where a.Spu <> @Spu  and BarCode<>'' and a.barcode in ({0})", strQueryinfo);

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("Spu", spu);

            return db.ExecuteSqlScalar<int>(sql, parameters) > 0;
        }

        public bool UpdateCategoryHistory(int supplierId, int categoryId)
        {
            const string sql = @"if exists(select * from CategoryHistory where SupplierId=@SupplierId and ThirdCategoryId=@CategoryId)
                                begin
                                    UPDATE CategoryHistory
                                    SET CreateTime = GETDATE()
                                        ,ModeifyTime = GETDATE()
                                    WHERE SupplierId=@SupplierId and ThirdCategoryId=@CategoryId
                                end
                                else
                                begin
	                                INSERT INTO CategoryHistory(SupplierId,ThirdCategoryId,CreateTime,ModeifyTime)
	                                VALUES
	                                (@SupplierId,@CategoryId,GETDATE(),GETDATE())
                                end";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("SupplierId", supplierId);
            parameters.Append("CategoryId", categoryId);

            return db.ExecuteSqlNonQuery(sql, parameters) > 0;
        }

        public Dictionary<string, int> GetSkuCustomReportCount(List<string> skus)
        {
            string sql = @"SELECT sku,count(1) AS RsCount FROM SkuCustomsReport(NOLOCK)
                                WHERE SKU IN ({0})
                                GROUP BY sku";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            StringBuilder sb = new StringBuilder(400);
            int i = 0;
            foreach (var sku in skus)
            {
                sb.Append("@SKU" + i);
                sb.Append(",");

                parameters.Append("SKU" + i, sku);
                i++;
            }

            var result = new Dictionary<string, int>();
            if (sb.Length < 1)
            {
                sql = String.Format(sql, "''");
            }
            else
            {
                sql = String.Format(sql, sb.Remove(sb.Length - 1, 1));
            }
            var ds = db.ExecuteSqlTable(sql, parameters);

            foreach (DataRow dr in ds.Rows)
            {
                result.Add(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()));
            }

            return result;
        }

        public void SaveProductJson(string spu, string productJson, string isRelese, string action)
        {
            const string sql = @"INSERT INTO ProudctInfoJson VALUES(@SPU,@IsRelese,@Action,@ProductInfo)";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("SPU", spu);
            parameters.Append("IsRelese", isRelese);
            parameters.Append("Action", action);
            parameters.Append("ProductInfo", productJson);

            db.ExecuteSqlNonQuery(sql, parameters);
        }

        public bool CheckBrandStatus(string sku)
        {
            var sql = @"select top 1 sb.Status from 
                        ProductInfo p(nolock) inner join SkuInfo s(nolock) on p.Id = s.SpuId
                        inner join SupplierBrand sb(nolock) on p.BrandId = sb.Id 
                        where Sku=@Sku";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@Sku", sku);

            return db.ExecuteSqlScalar<int>(sql, parameters) > 0;
        }
    }
}
