using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using SFO2O.Admin.ViewModel.Product;
using SFO2O.Admin.Models.Product;
using SFO2O.Admin.ViewModel;
using SFO2O.Admin.Common;
using System.Reflection;
using System.Data;
using SFO2O.Admin.Models.Enums;
using SFO2O.Admin.Models;


namespace SFO2O.Admin.DAO.Product
{
    public class ProductDAL : BaseDao
    {
        public PageOf<ProductAuditingListModel> GetAuditingProductList(int pageSize, int pageIndex, int languageVersion, ProductAuditingQuyModel queryModel)
        {
            const string sql = @"WITH categoryNameChain
            AS
            (
	            SELECT rootid.CategoryName+'>'+parentid.CategoryName+'>'+categoryKey.CategoryName AS CategoryName,c.CategoryKey
	            FROM Category(NOLOCK) c
	            INNER JOIN Category_LanguageVersion(NOLOCK) rootid
	            ON c.RootId=rootid.CategoryKey AND rootid.LanguageVersion=@LanguageVersion
	            INNER JOIN Category_LanguageVersion(NOLOCK) parentid
	            ON c.ParentId=parentid.CategoryKey AND parentid.LanguageVersion=@LanguageVersion
	            INNER JOIN Category_LanguageVersion(NOLOCK) categoryKey
	            ON c.CategoryKey=categoryKey.CategoryKey AND categoryKey.LanguageVersion=@LanguageVersion
	            WHERE c.CategoryLevel=2
            )
            select * from (
            SELECT ROW_NUMBER() OVER (ORDER BY pt.Createtime DESC) AS RowNumber, pt.Spu,pt.Name AS ProductName,s.CompanyName AS SupplierName
            ,pt.ModifyTime AS Createtime
            ,CASE pt.SalesTerritory WHEN 1 THEN '中國大陸' WHEN 2 THEN '香港地區' WHEN 3 THEN '中國大陸及香港地區' ELSE '中國大陸' END AS SalesTerritory
            ,CASE pt.DataSource WHEN 1 THEN '新上傳' WHEN 2 THEN '修改' END AS DataSource
            ,st.Sku
            ,CASE st.ReportStatus WHEN 1 THEN '已報備' WHEN 0 THEN '未報備' WHEN -1 THEN '不報備' ELSE '未報備' END AS ReportStatus
            ,CategoryName
            FROM ProductInfo_Temp(NOLOCK) pt
            INNER JOIN SkuInfo_Temp(NOLOCK) st
            ON st.SpuId = pt.Id AND st.Spu= pt.Spu
            INNER JOIN Supplier(NOLOCK) s
            ON s.SupplierID =pt.SupplierId
            INNER JOIN categoryNameChain
            ON categoryNameChain.CategoryKey=pt.CategoryId
            WHERE pt.LanguageVersion=@LanguageVersion AND pt.status=0 AND pt.ModifyTime >= @CreateTime AND pt.ModifyTime < @EndTime {0}) a
            where a.RowNumber >(@PageIndex-1)*@PageSize AND a.RowNumber <= @PageIndex*@PageSize
            ORDER BY a.Createtime DESC

            SELECT COUNT(1),COUNT(distinct pt.Spu)
            FROM ProductInfo_Temp(NOLOCK) pt
            INNER JOIN SkuInfo_Temp(NOLOCK) st
            ON st.SpuId = pt.Id AND st.Spu= pt.Spu
            INNER JOIN Supplier(NOLOCK) s
            ON s.SupplierID =pt.SupplierId
            WHERE pt.LanguageVersion=@LanguageVersion AND pt.status=0 AND pt.Createtime >= @CreateTime AND pt.CreateTime < @EndTime {1}
            
            ";



            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("PageIndex", pageIndex);
            parameters.Append("PageSize", pageSize);

            parameters.Append("CreateTime", queryModel.CreateTimeStart);
            parameters.Append("EndTime", queryModel.CreateTimeEnd.AddDays(1));

            parameters.Append("LanguageVersion", languageVersion);

            #region 查询条件

            StringBuilder query = new StringBuilder();

            if (!String.IsNullOrWhiteSpace(queryModel.Spu))
            {
                query.Append(" AND pt.Spu=@SPU");
                parameters.Append("SPU", queryModel.Spu);
            }

            if (!String.IsNullOrWhiteSpace(queryModel.Sku))
            {
                query.Append(" AND st.sku=@SKU");
                parameters.Append("SKU", queryModel.Sku);
            }

            if (!String.IsNullOrWhiteSpace(queryModel.ProductName))
            {
                var name = "%" + queryModel.ProductName + "%";
                query.Append(" AND pt.Name like @Name");
                parameters.Append("Name", name);
            }

            if (queryModel.SupplierId > 0)
            {
                query.Append(" AND s.SupplierID=@SupplierId");
                parameters.Append("SupplierId", queryModel.SupplierId);
            }

            if (queryModel.EditType != 0)
            {
                query.Append(" AND pt.DataSource=@DataSource");
                parameters.Append("DataSource", queryModel.EditType);
            }

            if (queryModel.ReportStatus > -2)
            {
                query.Append(" AND st.ReportStatus=@ReportStatus");
                parameters.Append("ReportStatus", queryModel.ReportStatus);
            }

            if (queryModel.SalesTerritory > 0)
            {
                switch (queryModel.SalesTerritory)
                {
                    case 1://仅中国大陆
                        query.Append(" AND pt.SalesTerritory IN(1)");
                        break;
                    case 2://仅限香港地区
                        query.Append(" AND pt.SalesTerritory IN(2)");
                        break;
                    case 3://含中国大陆
                        query.Append(" AND pt.SalesTerritory IN(1,3)");
                        break;
                    case 4://含香港地区
                        query.Append(" AND pt.SalesTerritory IN(2,3)");
                        break;
                    case 5://含香港地区及中国大陆
                        query.Append(" AND pt.SalesTerritory IN(3)");
                        break;
                }
            }

            #endregion
            var finallySql = String.Format(sql, query.ToString(), query.ToString());
            var ds = db.ExecuteSqlDataSet(finallySql, parameters);

            return new PageOf<ProductAuditingListModel>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                RowCount = Convert.ToInt32(ds.Tables[1].Rows[0][1]),
                Items = DataMapHelper.DataSetToList<ProductAuditingListModel>(ds)
            };
        }

        public PageOf<ProductExportModel> AuditingProductsExport(int languageVersion, ProductAuditingQuyModel queryModel)
        {
            const string sql = @"
                WITH categoryNameChain
                AS
                (
	                SELECT rootid.CategoryName+'>'+parentid.CategoryName+'>'+categoryKey.CategoryName AS CategoryName,c.CategoryKey
	                FROM Category(NOLOCK) c
	                INNER JOIN Category_LanguageVersion(NOLOCK) rootid
	                ON c.RootId=rootid.CategoryKey AND rootid.LanguageVersion=@LanguageVersion
	                INNER JOIN Category_LanguageVersion(NOLOCK) parentid
	                ON c.ParentId=parentid.CategoryKey AND parentid.LanguageVersion=@LanguageVersion
	                INNER JOIN Category_LanguageVersion(NOLOCK) categoryKey
	                ON c.CategoryKey=categoryKey.CategoryKey AND categoryKey.LanguageVersion=@LanguageVersion
	                WHERE c.CategoryLevel=2
                )
                SELECT pt.[Spu],pt.[SupplierId],categoryNameChain.CategoryName,[Name],[Tag],pt.[Price],[Description],b.NameHK [Brand],[CountryOfManufacture]
                      ,CASE pt.SalesTerritory WHEN 1 THEN '中国大陆' WHEN 2 THEN '香港' WHEN 3 THEN '中国大陆及香港' ELSE '中国大陆' END AS SalesTerritory,[Unit] 
	                  , CASE [IsExchangeInCHINA] WHEN 1 THEN '是' ELSE '否' END AS [IsExchangeInCHINA]
	                  , CASE [IsExchangeInHK] WHEN 1 THEN '是' ELSE '否' END AS [IsExchangeInHK]
	                  , CASE [IsReturn] WHEN 1 THEN '是' ELSE '否' END AS [IsReturn]
                      ,[MinForOrder],[MinPrice],[NetWeightUnit],[NetContentUnit]
	                  , CASE [IsDutyOnSeller] WHEN 1 THEN '是' ELSE '否' END AS [IsDutyOnSeller]
	                  , CASE pt.[DataSource] WHEN 1 THEN '新上传' ELSE '修改' END AS [DataSource]
                      ,pt.[ModifyTime],[CommissionInCHINA],[CommissionInHK]
	                  ,[Materials],[Pattern],[Flavour],[Ingredients],[StoragePeriod]
                      ,[StoringTemperature],[SkinType],[Gender],[AgeGroup],[Model],[BatteryTime]
                      ,[Voltage],[Power],[Warranty],[SupportedLanguage],[PetType],[PetAgeUnit]
                      ,[PetAge],[Location],[Weight],[WeightUnit],[Volume],[VolumeUnit],[Length]
                      ,[LengthUnit],[Width],[WidthUnit],[Height],[HeightUnit],[Flavor]
	                  ,st.[Sku],[MainDicKey],[MainDicValue],[SubDicKey],[SubDicValue],[MainKey]
                      ,[MainValue],[SubKey],[SubValue],st.[NetWeight],st.[NetContent],[Specifications]
                      ,[Size],[Color],[AlcoholPercentage],[Smell],[CapacityRestriction]
                      ,st.[Price] AS skuPrice,[BarCode],[AlarmStockQty]
	                  ,[CustomsUnit],[InspectionNo],[HSCode],[UOM],[PrepardNo],[GnoCode],[TaxRate],[TaxCode],[ModelForCustoms]
                  FROM [ProductInfo_Temp](NOLOCK) pt
                  LEFT JOIN SupplierBrand b ON b.Id = pt.BrandId
                  LEFT JOIN [ProductInfoExpand_Temp](NOLOCK)  pet
                  ON pt.id = pet.SpuId
                  LEFT JOIN [SkuInfo_Temp](NOLOCK)  st
                  ON st.SpuId = pt.Id
                  INNER JOIN categoryNameChain
                  ON categoryNameChain.CategoryKey=pt.CategoryId
                  LEFT JOIN SkuCustomsReport_Temp(NOLOCK) sct
                  ON sct.sku = st.Sku
                  WHERE pt.LanguageVersion=@LanguageVersion AND pt.status=0 AND pt.ModifyTime >= @CreateTime AND pt.ModifyTime < @EndTime {0}
                  ORDER BY pt.Createtime DESC";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("CreateTime", queryModel.CreateTimeStart);
            parameters.Append("EndTime", queryModel.CreateTimeEnd.AddDays(1));

            parameters.Append("LanguageVersion", languageVersion);

            #region 查询条件

            StringBuilder query = new StringBuilder();

            if (!String.IsNullOrWhiteSpace(queryModel.Spu))
            {
                query.Append(" AND pt.Spu=@SPU");
                parameters.Append("SPU", queryModel.Spu);
            }

            if (!String.IsNullOrWhiteSpace(queryModel.Sku))
            {
                query.Append(" AND st.sku=@SKU");
                parameters.Append("SKU", queryModel.Sku);
            }

            if (!String.IsNullOrWhiteSpace(queryModel.ProductName))
            {
                query.Append(" AND pt.Name='%'+ @Name + '%'");
                parameters.Append("Name", queryModel.ProductName);
            }

            if (queryModel.SupplierId > 0)
            {
                query.Append(" AND s.SupplierID=@SupplierId");
                parameters.Append("SupplierId", queryModel.SupplierId);
            }

            if (queryModel.EditType != 0)
            {
                query.Append(" AND pt.DataSource=@DataSource");
                parameters.Append("DataSource", queryModel.EditType);
            }

            if (queryModel.ReportStatus > -2)
            {
                query.Append(" AND st.ReportStatus=@ReportStatus");
                parameters.Append("ReportStatus", queryModel.ReportStatus);
            }

            if (queryModel.SalesTerritory > 0)
            {
                switch (queryModel.SalesTerritory)
                {
                    case 1://仅中国大陆
                        query.Append(" AND pt.SalesTerritory IN(1)");
                        break;
                    case 2://仅限香港地区
                        query.Append(" AND pt.SalesTerritory IN(2)");
                        break;
                    case 3://含中国大陆
                        query.Append(" AND pt.SalesTerritory IN(1,3)");
                        break;
                    case 4://含香港地区
                        query.Append(" AND pt.SalesTerritory IN(2,3)");
                        break;
                    case 5://含香港地区
                        query.Append(" AND pt.SalesTerritory IN(3)");
                        break;
                }
            }

            #endregion
            var finallySql = String.Format(sql, query.ToString(), query.ToString());
            var ds = db.ExecuteSqlDataSet(finallySql, parameters);

            return new PageOf<ProductExportModel>()
            {
                Items = DataMapHelper.DataSetToList<ProductExportModel>(ds)
            };

        }

        /// <summary>
        /// 获取商品信息及其报关信息
        /// </summary>
        /// <param name="spu"></param>
        /// <returns></returns>
        public ProductAuditingModel GetProductAndCustomInfo(string spu)
        {
            const string sql = @"
            SELECT b.NameCN Brand,pt.Name,pt.Price,pt.IsDutyOnSeller,pt.Unit,DataSource,LanguageVersion,SalesTerritory
            FROM ProductInfo_Temp(NOLOCK) AS pt
            LEFT JOIN SupplierBrand b ON b.Id = pt.BrandId
            WHERE spu=@SPU AND pt.status=0
            ORDER BY LanguageVersion;

            WITH categoryNameChain
            AS
            (
	            SELECT rootid.CategoryName+'>'+parentid.CategoryName+'>'+categoryKey.CategoryName AS CategoryName,c.CategoryKey
	            FROM Category(NOLOCK) c
	            INNER JOIN Category_LanguageVersion(NOLOCK) rootid
	            ON c.RootId=rootid.CategoryKey AND rootid.LanguageVersion=2
	            INNER JOIN Category_LanguageVersion(NOLOCK) parentid
	            ON c.ParentId=parentid.CategoryKey AND parentid.LanguageVersion=2
	            INNER JOIN Category_LanguageVersion(NOLOCK) categoryKey
	            ON c.CategoryKey=categoryKey.CategoryKey AND categoryKey.LanguageVersion=2
	            WHERE c.CategoryLevel=2
            )
            SELECT pt.spu,categoryNameChain.CategoryName,s.CompanyName
            ,CASE pt.SalesTerritory WHEN 1 THEN '中國大陸' WHEN 2 THEN '香港' WHEN 3 THEN '中國大陸及香港' ELSE '中國大陸' END AS SalesTerritory
            ,pt.CommissionInCHINA,pt.CommissionInHK,MinForOrder,IsExchangeInCHINA,IsExchangeInHK,IsReturn,PreOnSaleTime,ModifyTime
            ,Pet.Weight,pet.Volume,pet.Length,pet.Width,pet.Height from ProductInfo_Temp(NOLOCK) AS pt 
            INNER JOIN ProductInfoExpand_Temp(NOLOCK) AS pet
            ON pt.id=pet.SpuId
            INNER JOIN Supplier(NOLOCK) s
            ON s.SupplierID = pt.SupplierId
            INNER JOIN categoryNameChain
            ON categoryNameChain.CategoryKey = pt.CategoryId
            WHERE spu=@SPU AND LanguageVersion=2 AND pt.status=0


            SELECT st.Sku,st.BarCode,st.Price,st.AlarmStockQty
            ,st.MainDicValue,st.MainValue,st.SubDicValue,st.SubValue
            ,sct.CustomsUnit,sct.InspectionNo,sct.HSCode,sct.UOM
            ,sct.PrepardNo,sct.GnoCode,sct.TaxRate,sct.TaxCode,sct.ModelForCustoms
             FROM SkuInfo_Temp(NOLOCK) AS ST
            INNER JOIN ProductInfo_Temp(NOLOCK) AS pt
            ON st.SpuId = pt.Id
            LEFT JOIN SkuCustomsReport_Temp(NOLOCK) AS sct
            ON sct.sku = st.Sku
            WHERE pt.spu=@SPU AND pt.LanguageVersion=2 AND pt.status=0


            SELECT st.sku,pt.Tag,pt.CountryOfManufacture,LanguageVersion
            ,pt.NetContentUnit,pt.NetWeightUnit
            ,pet.Materials,pet.Pattern,pet.Flavour,pet.Ingredients
            ,pet.StoragePeriod,pet.StoringTemperature,pet.SkinType
            ,Pet.Gender,pet.AgeGroup,pet.PetAge,pet.Model,pet.BatteryTime
            ,pet.Voltage,pet.Power,pet.Warranty,pet.SupportedLanguage
            ,pet.PetType,pet.PetAgeUnit,pet.Location,st.NetWeight
            ,st.NetContent,st.Specifications,st.Size,st.Color,pet.Flavor
            ,st.AlcoholPercentage,st.Smell,st.CapacityRestriction
             FROM ProductInfo_Temp(NOLOCK) AS PT
            INNER JOIN ProductInfoExpand_Temp(NOLOCK) AS pet
            ON pt.id=pet.SpuId
            INNER JOIn SkuInfo_Temp(NOLOCK) AS st
            ON pt.id=st.SpuId
            WHERE pt.spu=@SPU AND pt.status=0
            ORDER BY pt.LanguageVersion,st.Sku

            SELECT * FROM ProductImage_Temp
            WHERE SPU=@SPU AND ImageType=1
            ORDER BY SortValue";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("SPU", spu, System.Data.DbType.String);

            var ds = db.ExecuteSqlDataSet(sql, parameters);

            if (ds == null || ds.Tables == null || ds.Tables.Count == 0)
            {
                return new ProductAuditingModel();
            }

            var result = new ProductAuditingModel();

            if (ds.Tables[0] != null && ds.Tables[0].Rows != null)
            {
                result.ProductBaseInfos = DataMapHelper.DataTableToList<ProductBaseInfoModel>(ds.Tables[0]).ToList();
            }

            if (ds.Tables[1] != null && ds.Tables[1].Rows != null)
            {
                result.ProductSysInfo = DataMapHelper.DataTableToObject<ProductSysInfoModel>(ds.Tables[1]);
            }

            if (ds.Tables[2] != null && ds.Tables[2].Rows != null)
            {
                result.ProductCustomInfos = DataMapHelper.DataTableToList<ProductSkuCustomInfoModel>(ds.Tables[2]).ToList();
            }

            if (ds.Tables[3] != null && ds.Tables[3].Rows != null)
            {
                result.ProductAttrsInfos = DataMapHelper.DataTableToList<ProductAttrInfoModel>(ds.Tables[3]).ToList();
            }

            if (ds.Tables[4] != null && ds.Tables[4].Rows != null)
            {
                result.ProductImgs = DataMapHelper.DataTableToList<ProductImgInfoModel>(ds.Tables[4]).ToList();
            }

            return result;
        }

        /// <summary>
        /// 获取商品信息及其报关信息_已发布的商品
        /// </summary>
        /// <param name="spu"></param>
        /// <returns></returns>
        public ProductAuditingModel GetProductAndCustomInfoOnline(string spu)
        {
            const string sql = @"
            SELECT b.NameHK Brand,pt.Name,pt.Price,pt.IsDutyOnSeller,pt.Unit,LanguageVersion,SalesTerritory
            FROM ProductInfo(NOLOCK) AS pt
            LEFT JOIN SupplierBrand b ON b.Id = pt.BrandId
            WHERE spu=@SPU
            ORDER BY LanguageVersion;

            WITH categoryNameChain
            AS
            (
	            SELECT rootid.CategoryName+'>'+parentid.CategoryName+'>'+categoryKey.CategoryName AS CategoryName,c.CategoryKey
	            FROM Category(NOLOCK) c
	            INNER JOIN Category_LanguageVersion(NOLOCK) rootid
	            ON c.RootId=rootid.CategoryKey AND rootid.LanguageVersion=2
	            INNER JOIN Category_LanguageVersion(NOLOCK) parentid
	            ON c.ParentId=parentid.CategoryKey AND parentid.LanguageVersion=2
	            INNER JOIN Category_LanguageVersion(NOLOCK) categoryKey
	            ON c.CategoryKey=categoryKey.CategoryKey AND categoryKey.LanguageVersion=2
	            WHERE c.CategoryLevel=2
            )
            SELECT pt.spu,categoryNameChain.CategoryName,s.CompanyName
            ,CASE pt.SalesTerritory WHEN 1 THEN '中國大陸' WHEN 2 THEN '香港' WHEN 3 THEN '中國大陸及香港' ELSE '中國大陸' END AS SalesTerritory
            ,pt.CommissionInCHINA,pt.CommissionInHK,MinForOrder,IsExchangeInCHINA,IsExchangeInHK,IsReturn,PreOnSaleTime,ModifyTime
            ,Pet.Weight,pet.Volume,pet.Length,pet.Width,pet.Height 
            FROM ProductInfo(NOLOCK) AS pt 
            INNER JOIN ProductInfoExpand(NOLOCK) AS pet
            ON pt.id=pet.SpuId
            INNER JOIN Supplier(NOLOCK) s
            ON s.SupplierID = pt.SupplierId
            INNER JOIN categoryNameChain
            ON categoryNameChain.CategoryKey = pt.CategoryId
            WHERE spu=@SPU AND LanguageVersion=2


            SELECT st.Sku,st.BarCode,st.Price,st.AlarmStockQty
            ,st.MainDicValue,st.MainValue,st.SubDicValue,st.SubValue
            ,sct.CustomsUnit,sct.InspectionNo,sct.HSCode,sct.UOM
            ,sct.PrepardNo,sct.GnoCode,sct.TaxRate,sct.TaxCode,sct.ModelForCustoms
             FROM SkuInfo(NOLOCK) AS ST
            INNER JOIN ProductInfo(NOLOCK) AS pt
            ON st.SpuId = pt.Id
            LEFT JOIN SkuCustomsReport(NOLOCK) AS sct
            ON sct.sku = st.Sku
            WHERE pt.spu=@SPU AND pt.LanguageVersion=2


            SELECT st.sku,pt.Tag,pt.CountryOfManufacture,LanguageVersion
            ,pt.NetContentUnit,pt.NetWeightUnit
            ,pet.Materials,pet.Pattern,pet.Flavour,pet.Ingredients
            ,pet.StoragePeriod,pet.StoringTemperature,pet.SkinType
            ,Pet.Gender,pet.AgeGroup,pet.PetAge,pet.Model,pet.BatteryTime
            ,pet.Voltage,pet.Power,pet.Warranty,pet.SupportedLanguage
            ,pet.PetType,pet.PetAgeUnit,pet.Location,st.NetWeight
            ,st.NetContent,st.Specifications,st.Size,st.Color,pet.Flavor
            ,st.AlcoholPercentage,st.Smell,st.CapacityRestriction
             FROM ProductInfo(NOLOCK) AS PT
            INNER JOIN ProductInfoExpand(NOLOCK) AS pet
            ON pt.id=pet.SpuId
            INNER JOIn SkuInfo(NOLOCK) AS st
            ON pt.id=st.SpuId
            WHERE pt.spu=@SPU
            ORDER BY pt.LanguageVersion,st.Sku

            SELECT * FROM ProductImage
            WHERE SPU=@SPU AND ImageType=1
            ORDER BY SortValue";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("SPU", spu, System.Data.DbType.String);

            var ds = db.ExecuteSqlDataSet(sql, parameters);

            if (ds == null || ds.Tables == null || ds.Tables.Count == 0)
            {
                return new ProductAuditingModel();
            }

            var result = new ProductAuditingModel();

            if (ds.Tables[0] != null && ds.Tables[0].Rows != null)
            {
                result.ProductBaseInfos = DataMapHelper.DataTableToList<ProductBaseInfoModel>(ds.Tables[0]).ToList();
            }

            if (ds.Tables[1] != null && ds.Tables[1].Rows != null)
            {
                result.ProductSysInfo = DataMapHelper.DataTableToObject<ProductSysInfoModel>(ds.Tables[1]);
            }

            if (ds.Tables[2] != null && ds.Tables[2].Rows != null)
            {
                result.ProductCustomInfos = DataMapHelper.DataTableToList<ProductSkuCustomInfoModel>(ds.Tables[2]).ToList();
            }

            if (ds.Tables[3] != null && ds.Tables[3].Rows != null)
            {
                result.ProductAttrsInfos = DataMapHelper.DataTableToList<ProductAttrInfoModel>(ds.Tables[3]).ToList();
            }

            if (ds.Tables[4] != null && ds.Tables[4].Rows != null)
            {
                result.ProductImgs = DataMapHelper.DataTableToList<ProductImgInfoModel>(ds.Tables[4]).ToList();
            }

            return result;
        }

        public void UpDateSpuStatus(string spu, int status)
        {
            const string sql = @"
             UPDATE SkuInfo_Temp SET [Status]=@Status FROM SkuInfo_Temp AS st
             INNER JOIN ProductInfo_Temp AS pt
             ON st.SpuId=pt.id
             WHERE pt.Spu=@SPU AND pt.Status=0

            UPDATE ProductInfo_Temp SET Status=@Status
            WHERE Spu=@SPU AND Status=0";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("SPU", spu, DbType.String);
            parameters.Append("Status", status);

            db.ExecuteSqlNonQuery(sql, parameters);
        }

        public void InsertProductAuditingLog(string spu, int status, string reason, string createBy)
        {
            const string sql = @"
                INSERT INTO ProductAuditingLog
                VALUES(@Spu,@Status,@Reason,@CreateTime,@CreateBy)";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("Spu", spu, DbType.String);
            parameters.Append("Status", status);
            parameters.Append("Reason", reason, DbType.String);
            parameters.Append("CreateBy", createBy);
            parameters.Append("CreateTime", DateTime.Now);

            db.ExecuteSqlNonQuery(sql, parameters);
        }

        public void InsertTemStorageCusReports(CustomReportJsonModel model)
        {
            const string sql = @"
            DELETE FROM [dbo].[SkuCustomsReport_Temp] WHERE sku IN ({0});

            INSERT INTO [dbo].[SkuCustomsReport_Temp]
                       ([Sku],[CustomsUnit],[InspectionNo],[HSCode],[UOM],[PrepardNo],[GnoCode],[TaxRate],[TaxCode],[ModelForCustoms])
            VALUES {1}
            ";

            if (null != model && model.CustomReports != null && model.CustomReports.Length > 0)
            {
                var db = DbSFO2OMain;
                var parameters = db.CreateParameterCollection();

                int index = 0;
                StringBuilder sb = new StringBuilder(500);
                StringBuilder skus = new StringBuilder();
                foreach (var cr in model.CustomReports)
                {
                    skus.Append("@Sku" + index + ",");

                    sb.Append("(@Sku" + index + ",@CustomsUnit" + index + ",@InspectionNo" + index + ",@HSCode" + index + ",@UOM" + index + ",@PrepardNo" + index + ",@GnoCode" + index + ",@TaxRate" + index + ",@TaxCode" + index + ",@ModelForCustoms" + index + "),");

                    parameters.Append("@Sku" + index, cr.Sku);
                    parameters.Append("@CustomsUnit" + index, cr.CustomsUnit);
                    parameters.Append("@InspectionNo" + index, cr.InspectionNo);
                    parameters.Append("@HSCode" + index, cr.HSCode);
                    parameters.Append("@UOM" + index, cr.UOM);
                    parameters.Append("@PrepardNo" + index, cr.PrepardNo);

                    parameters.Append("@GnoCode" + index, cr.GnoCode);
                    parameters.Append("@TaxRate" + index, String.IsNullOrWhiteSpace(cr.TaxRate) ? decimal.Zero : Convert.ToDecimal(cr.TaxRate));
                    parameters.Append("@TaxCode" + index, cr.TaxCode);
                    parameters.Append("@ModelForCustoms" + index, cr.ModelForCustoms);

                    index++;
                }

                var inserSql = sb.Remove(sb.Length - 1, 1).ToString();
                var finalSql = String.Format(sql, skus.Remove(skus.Length - 1, 1).ToString(), inserSql);


                db.ExecuteNonQuery(CommandType.Text, finalSql, parameters);
            }
        }


        public void UpdateSkuReportStatus(CustomReportJsonModel model)
        {
            const string sql = @"
            UPDATE SkuInfo_Temp SET ReportStatus =1 WHERE Status=0 AND SKU IN({0})
            ";

            if (null != model && model.CustomReports != null && model.CustomReports.Length > 0)
            {
                var db = DbSFO2OMain;
                var parameters = db.CreateParameterCollection();

                int index = 0;
                StringBuilder skus = new StringBuilder();
                foreach (var cr in model.CustomReports)
                {
                    if (!String.IsNullOrWhiteSpace(cr.CustomsUnit)
                        && !String.IsNullOrWhiteSpace(cr.GnoCode)
                        && !String.IsNullOrWhiteSpace(cr.HSCode)
                        && !String.IsNullOrWhiteSpace(cr.InspectionNo)
                        && !String.IsNullOrWhiteSpace(cr.ModelForCustoms)
                        && !String.IsNullOrWhiteSpace(cr.PrepardNo)
                        && !String.IsNullOrWhiteSpace(cr.TaxCode)
                        && !String.IsNullOrWhiteSpace(cr.TaxRate)
                        && !String.IsNullOrWhiteSpace(cr.UOM)
                        )
                    {
                        skus.Append("@Sku" + index + ",");
                        parameters.Append("@Sku" + index, cr.Sku);
                        index++;
                    }
                }

                if (skus.Length == 0)
                {
                    return;
                }

                var finalSql = String.Format(sql, skus.Remove(skus.Length - 1, 1).ToString());

                db.ExecuteNonQuery(CommandType.Text, finalSql, parameters);
            }
        }

        public Dictionary<string, SkuReportStatusModel> GetAuditingSkuInfos()
        {
            const string sql = @"
                    SELECT st.Id,st.Sku,st.ReportStatus FROM SkuInfo_Temp(NOLOCK) st
                    INNER JOIN ProductInfo_Temp(NOLOCK) pt
                    ON pt.id = st.SpuId AND pt.Status=0
                ";


            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            List<SkuReportStatusModel> skus = db.ExecuteSqlList<SkuReportStatusModel>(sql, parameters).ToList();

            if (skus == null || skus.Count == 0)
            {
                return new Dictionary<string, SkuReportStatusModel>();
            }

            var result = new Dictionary<string, SkuReportStatusModel>();

            foreach (var s in skus)
            {
                if (!result.Keys.Contains(s.Sku))
                {
                    result.Add(s.Sku, s);
                }
                else
                {
                    var sm = result[s.Sku];
                    if (sm.ConnectIDs == null)
                    {
                        sm.ConnectIDs = new List<int>();
                    }

                    sm.ConnectIDs.Add(s.Id);
                    result[s.Sku] = sm;
                }
            }

            return result;
        }

        public void UpdateSkuReportStatus(List<int> customedSkuLineIds)
        {
            const string sql = @"update SkuInfo_Temp SET ReportStatus=1 WHERE Id in({0})";

            if (null != customedSkuLineIds && customedSkuLineIds.Count > 0)
            {
                var db = DbSFO2OMain;
                var parameters = db.CreateParameterCollection();

                int index = 0;
                StringBuilder skus = new StringBuilder();
                foreach (var cr in customedSkuLineIds)
                {
                    skus.Append("@Id" + index + ",");
                    parameters.Append("@Id" + index, cr, DbType.Int32);

                    index++;
                }

                var inserSql = skus.Remove(skus.Length - 1, 1).ToString();
                var finalSql = String.Format(sql, inserSql);


                db.ExecuteNonQuery(CommandType.Text, finalSql, parameters);
            }
        }

        public int InsertImportError(Dictionary<string, string> errors)
        {
            const string sql = @"
                DECLARE @maxBatch int
                SELECT @maxBatch= (ISNULL( max(BatchNo),0) +1) FROM ImportCustomError(NOLOCK)
                INSERT INTO ImportCustomError
                VALUES {0}
                SELECT @maxBatch";

            var batchNo = 0;

            if (errors != null && errors.Count > 0)
            {
                var db = DbSFO2OMain;
                var parameters = db.CreateParameterCollection();

                int index = 0;
                StringBuilder sb = new StringBuilder();
                foreach (var cr in errors)
                {
                    sb.Append("(@maxBatch,");
                    sb.Append("@sku" + index + ",");
                    sb.Append("@Message" + index + "),");

                    parameters.Append("@sku" + index, cr.Key, DbType.String);
                    parameters.Append("@Message" + index, cr.Value, DbType.String);

                    index++;
                }

                var inserSql = sb.Remove(sb.Length - 1, 1).ToString();
                var finalSql = String.Format(sql, inserSql);


                return db.ExecuteSqlScalar<int>(finalSql, parameters);
            }

            return 0;
        }

        public List<ImportErrorModel> GetImportErrors(int batchNo)
        {
            const string sql = @"
                SELECT ROW_NUMBER() OVER (ORDER BY id) AS RowNumber,sku,message 
                FROM ImportCustomError(NOLOCK)
                WHERE batchNo=@BatchNo";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("BatchNo", batchNo);

            return db.ExecuteSqlList<ImportErrorModel>(sql, parameters).ToList();
        }

        public PageOf<ProductAuditingListModel> GetProductList(ProductAuditingQuyModel queryModel, LanguageEnum languageVersion, Models.PagingModel page)
        {
            string sql = @"WITH categoryNameChain
            AS
            (
                select  t0.CategoryID as FstCategoryId, t1.CategoryID as SndCategoryId, t2.CategoryID as TrdCategoryId,
                        (cl0.CategoryName + '>>'+ cl1.CategoryName+ '>>'+ cl2.CategoryName) as  CategoryName,cl2.CategoryKey 
                from Category(NOLOCK) as t2
	            inner join Category_LanguageVersion(NOLOCK) cl2 on t2.CategoryKey=cl2.CategoryKey and cl2.LanguageVersion=2
	            inner join Category(NOLOCK) AS t1 ON t2.ParentId = t1.CategoryID 
	            inner join Category_LanguageVersion(NOLOCK) cl1 on t1.CategoryKey=cl1.CategoryKey and cl1.LanguageVersion=2
				inner join Category(NOLOCK) AS t0 on t1.ParentId=t0.CategoryID
	            inner join Category_LanguageVersion(NOLOCK) cl0 on t0.CategoryKey=cl0.CategoryKey and cl0.LanguageVersion=2
            )
            ,orderCount
            AS 
            (
				select op.Sku,sum(op.Quantity) as SkuOrderQuantity from OrderInfo o inner join orderproducts op on o.ordercode= op.ordercode
				
				where o.orderstatus in (1,2,3,4)
				group by op.Sku
            )
            ,OnSaleStatus
			AS
			(
				SELECT pinfo.Spu,SUm(ISNULL(t.ForOrderQty,0)) AS sumqty,pInfo.MinForOrder FROM SkuInfo s LEFT JOIN Stock t ON t.Sku=s.Sku
				INNER JOIN ProductInfo pInfo ON s.SpuId=pInfo.Id
				WHERE s.[Status]=3 AND pInfo.LanguageVersion=2
				GROUP BY pinfo.Spu,pInfo.MinForOrder
			)
            {0}
            SELECT ROW_NUMBER() OVER (ORDER BY pt.Spu DESC) AS RowNum, pt.Spu,pt.Name AS ProductName,s.CompanyName AS SupplierName
            ,pt.Createtime
            ,CASE pt.SalesTerritory WHEN 1 THEN '中國大陸' WHEN 2 THEN '香港地區' WHEN 3 THEN '中國大陸及香港地區' ELSE '中國大陸' END AS SalesTerritory
            ,CategoryName,oc.SkuOrderQuantity
            ,st.Sku,st.IsOnSaled,SC.ForOrderQty,PT.MinForOrder,st.Status,CASE WHEN sc.Qty>0 THEN 1 ELSE  0  END AS InventoryStatus,sc.qty AS QTY
            FROM ProductInfo(NOLOCK) pt
            INNER JOIN SkuInfo(NOLOCK) st
            ON st.SpuId = pt.Id AND st.Spu= pt.Spu
            LEFT JOIN Stock(NOLOCK) SC ON SC.Sku=ST.Sku
            INNER JOIN Supplier(NOLOCK) s
            ON s.SupplierID =pt.SupplierId
            INNER JOIN categoryNameChain cate
            ON cate.CategoryKey=pt.CategoryId
            LEFT JOIN orderCount oc on oc.Sku=st.Sku
            LEFT JOIN OnSaleStatus ON OnSaleStatus.spu=pt.Spu
            WHERE  pt.LanguageVersion=@LanguageVersion ";


            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);
            parameters.Append("LanguageVersion", languageVersion);

            #region 查询条件

            StringBuilder query = new StringBuilder();
            if (queryModel.CreateTimeStart != null)
            {
                query.Append(" AND pt.Createtime >= @CreateTime");
                parameters.Append("CreateTime", queryModel.CreateTimeStart);
            }
            if (queryModel.CreateTimeStart != null)
            {
                query.Append(" AND pt.CreateTime < @EndTime");
                parameters.Append("EndTime", queryModel.CreateTimeEnd.AddDays(1));
            }
            if (!String.IsNullOrWhiteSpace(queryModel.Spu))
            {
                query.Append(" AND pt.Spu=@Spu");
                parameters.Append("Spu", queryModel.Spu);
            }
            if (!String.IsNullOrWhiteSpace(queryModel.Sku))
            {
                query.Append(" AND st.sku=@SKU");
                parameters.Append("SKU", queryModel.Sku);
            }
            if (!String.IsNullOrWhiteSpace(queryModel.ProductName))
            {
                query.Append(string.Format(" AND pt.Name like '%{0}%'", queryModel.ProductName));
            }
            if (queryModel.SupplierId > 0)
            {
                query.Append(" AND s.SupplierId=@SupplierId");
                parameters.Append("SupplierId", queryModel.SupplierId);
            }
            if (queryModel.IsOnSales != -1)
            {
                query.Append(" AND (OnSaleStatus.sumqty-OnSaleStatus.MinForOrder)>0");
            }
            if (queryModel.ProductStatus > -1)
            {
                query.Append(" AND st.Status=@ProductStatus");
                parameters.Append("ProductStatus", queryModel.ProductStatus);
            }
            if (queryModel.InventoryStatus != -1)
            {
                if (queryModel.InventoryStatus > 0)
                {
                    query.Append(" AND sc.ForOrderQty >0");
                }
                else
                {
                    query.Append(" AND sc.ForOrderQty<=0");
                }
            }
            if (queryModel.TrdCagegoryId > 0)
            {
                query.Append(" AND cate.TrdCategoryId=@TrdCategoryId");
                parameters.Append("TrdCategoryId", queryModel.TrdCagegoryId);
            }
            else if (queryModel.SndCagegoryId > 0)
            {
                query.Append(" AND cate.SndCategoryId=@SndCagegoryId");
                parameters.Append("SndCagegoryId", queryModel.SndCagegoryId);
            }
            else if (queryModel.FstCagegoryId > 0)
            {
                query.Append(" AND cate.FstCategoryId=@FstCagegoryId");
                parameters.Append("FstCagegoryId", queryModel.FstCagegoryId);
            }

            if (queryModel.SalesTerritory > 0)
            {
                switch (queryModel.SalesTerritory)
                {
                    case 1://仅中国大陆
                        query.Append(" AND pt.SalesTerritory IN(1)");
                        break;
                    case 2://仅限香港地区
                        query.Append(" AND pt.SalesTerritory IN(2)");
                        break;
                    case 3://含中国大陆
                        query.Append(" AND pt.SalesTerritory IN(1,3)");
                        break;
                    case 4://含香港地区
                        query.Append(" AND pt.SalesTerritory IN(2,3)");
                        break;
                    case 5:
                        query.Append(" AND pt.SalesTerritory IN(3)");
                        break;
                }
            }

            #endregion
            
            sql = sql + query.ToString();
            var finallySql = string.Empty;

            finallySql = String.Format(sql, "SELECT * FROM (") + ")a WHERE a.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;";

            finallySql += String.Format(sql, "SELECT COUNT(1),COUNT(distinct b.spu) FROM (") + ") b";



            var ds = db.ExecuteSqlDataSet(finallySql, parameters);

            return new PageOf<ProductAuditingListModel>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                RowCount = Convert.ToInt32(ds.Tables[1].Rows[0][1]),
                Items = DataMapHelper.DataSetToList<ProductAuditingListModel>(ds)
            };
        }

        public void OffShelfSku(string spu, int status)
        {
            const string sql = @"
            UPDATE SkuInfo SET Status=@Status
            WHERE Spu=@SPU";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("SPU", spu, DbType.String);
            parameters.Append("Status", status);

            db.ExecuteSqlNonQuery(sql, parameters);
        }

        public string GetProductDes(string spu, int languageVersion)
        {
            const string sql = @"
            SELECT [Description] FROM ProductInfo_Temp(NOLOCK) WHERE Spu=@Spu ANd LanguageVersion=@LanguageVersion AND status=0";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("Spu", spu, DbType.String);
            parameters.Append("LanguageVersion", languageVersion);

            return db.ExecuteSqlScalar<string>(sql, parameters);
        }

        public string GetProductDesOnline(string spu, int languageVersion)
        {
            const string sql = @"
            SELECT [Description] FROM ProductInfo(NOLOCK) WHERE Spu=@Spu ANd LanguageVersion=@LanguageVersion";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("Spu", spu, DbType.String);
            parameters.Append("LanguageVersion", languageVersion);

            return db.ExecuteSqlScalar<string>(sql, parameters);
        }


        #region 审核通过传输数据
        public void TransferNewSpu(ImportSpuModel importData)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    DeleteSkuINfos(db, tran, importData.ProductInfo[0].Spu);

                    foreach (var spuInfo in importData.ProductInfo)
                    {
                        var spuId = 0;

                        if (IsSpuEXISTS(db, tran, spuInfo.Spu, spuInfo.LanguageVersion))
                        {
                            spuId = ModifyProductInfo(db, tran, spuInfo);
                        }
                        else
                        {
                            spuId = InsertProduct(db, tran, spuInfo);//保存SPU
                        }

                        var skuInfos = importData.SkuInfos.Where(p => p.SpuId == spuInfo.Id && p.Spu == spuInfo.Spu).ToList();

                        InsertProductExtend(db, tran, spuInfo, spuId);
                        InsertSkus(db, tran, spuId, skuInfos);
                    }

                    DeleteProductImages(db, tran, importData.ProductInfo[0].Spu);
                    InsertProductImages(db, tran, importData);

                    DeleteCustomsReport(db, tran, importData);
                    InsertCustomsReport(db, tran, importData);

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

        public void TransferOldSpu(ImportSpuModel importData)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    foreach (var spuInfo in importData.ProductInfo)
                    {
                        var spuId = ModifyProductInfo(db, tran, spuInfo);//更新SPU

                        ModifyProductExtendInfo(db, tran, spuInfo, spuId);//保存SPUExtend

                        int i = 0;
                        var skus = importData.SkuInfos.Where(p => p.SpuId == spuInfo.Id && p.Spu == spuInfo.Spu).ToList();

                        foreach (var sku in skus)
                        {
                            ModifySkuInfo(db, tran, spuId, sku, i);
                            i++;
                        }
                    }

                    DeleteProductImages(db, tran, importData.ProductInfo[0].Spu);
                    InsertProductImages(db, tran, importData);

                    DeleteCustomsReport(db, tran, importData);
                    InsertCustomsReport(db, tran, importData);

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

        private bool IsSpuEXISTS(Database db, System.Data.Common.DbTransaction tran, string spu, int languageVersion)
        {
            const string sql = @"SELECT COUNT(spu) FROM ProductInfo(NOLOCK) WHERE spu=@spu AND LanguageVersion=@LanguageVersion";

            var spuParas = db.CreateParameterCollection();

            spuParas.Append("SPU", spu);
            spuParas.Append("LanguageVersion", languageVersion);

            var ds = db.ExecuteDataSet(CommandType.Text, sql, spuParas, tran);

            var result = 0;

            if (ds.Tables != null && ds.Tables[0] != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0] != DBNull.Value)
                {
                    int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out result);
                }
            }

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int InsertProduct(Database db, System.Data.Common.DbTransaction tran, ProductAndExpandInfo productInfo)
        {
            var spuInfo = productInfo;

            const string insertSpu = @"INSERT INTO [dbo].[ProductInfo]([Spu],[CategoryId],[SupplierId],[Name],[Tag],[Price],[Description],[BrandId],[Brand]
                                    ,[CountryOfManufacture],[SalesTerritory],[Unit],[IsExchangeInCHINA],[IsExchangeInHK],[IsReturn],[MinForOrder],[MinPrice]
                                    ,[NetWeight],[NetWeightUnit],[NetContent],[NetContentUnit],[IsDutyOnSeller],[LanguageVersion],[Createtime],[CreateBy],[PreOnSaleTime]
                                    ,[ModifyTime],[ModifyBy],[CommissionInCHINA],[CommissionInHK])
                                    VALUES
                                    (@SPU,@CategoryID,@SupplierId,@Name,@Tag,@Price,@Description,@BrandId,@Brand,@CountryOfManufacture,@SalesTerritory,@Unit,@IsExchangeInCHINA,
                                    @IsExchangeInHK,@IsReturn,@MinForOrder,@MinPrice,@NetWeight,@NetWeightUnit,@NetContent,@NetContentUnit,@IsDutyOnSeller,@LanguageVersion
                                    ,@Createtime,@CreateBy,@PreOnSaleTime,@ModifyTime,@ModifyBy,@CommissionInCHINA,@CommissionInHK)
                                    SET @SpuId = @@IDENTITY";

            var spuParas = db.CreateParameterCollection();

            spuParas.Append("SPU", spuInfo.Spu);
            spuParas.Append("CategoryID", spuInfo.CategoryId);
            spuParas.Append("SupplierId", spuInfo.SupplierId);

            spuParas.Append("Name", spuInfo.Name);
            spuParas.Append("Tag", spuInfo.Tag);
            spuParas.Append("Price", spuInfo.Price);

            spuParas.Append("Description", spuInfo.Description);
            spuParas.Append("BrandId", spuInfo.BrandId);
            spuParas.Append("Brand", spuInfo.Brand);
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

            if (spuInfo.PreOnSaleTime == DateTime.MinValue)
            {
                spuInfo.PreOnSaleTime = spuInfo.ModifyTime.AddDays(7);
            }

            spuParas.Append("PreOnSaleTime", spuInfo.PreOnSaleTime);
            spuParas.Append("CommissionInCHINA", spuInfo.CommissionInCHINA);
            spuParas.Append("CommissionInHK", spuInfo.CommissionInHK);

            spuParas.Append("@SpuId", 0, DbType.Int32, ParameterDirection.InputOutput);
            db.ExecuteNonQuery(CommandType.Text, insertSpu, spuParas, tran);
            var id = 0;
            int.TryParse(spuParas["@SpuId"].Value.ToString(), out id);
            return id;
        }

        private int ModifyProductInfo(Database db, System.Data.Common.DbTransaction tran, ProductAndExpandInfo productInfo)
        {
            var spuInfo = productInfo;
            const string insertSpu = @"DECLARE @TMP TABLE(ID INT)  
                           UPDATE [dbo].[ProductInfo]
                           SET [Name] = @Name,[Tag] = @Tag,[Price] = @Price,[Description] = @Description,[BrandId] = @BrandId,[Brand] = @Brand,[CountryOfManufacture] = @CountryOfManufacture
                              ,[SalesTerritory] = @SalesTerritory,[Unit] = @Unit,[IsExchangeInCHINA] = @IsExchangeInCHINA,[IsExchangeInHK] = @IsExchangeInHK
                              ,[IsReturn] = @IsReturn,[MinForOrder] = @MinForOrder,[MinPrice] = @MinPrice,[NetWeight] = @NetWeight,[NetWeightUnit] = @NetWeightUnit
                              ,[NetContent] = @NetContent,[NetContentUnit] = @NetContentUnit,[IsDutyOnSeller] = @IsDutyOnSeller
                              ,[ModifyTime] = @ModifyTime,[ModifyBy] = @ModifyBy,[PreOnSaleTime] = @PreOnSaleTime
                              ,[CommissionInCHINA] = @CommissionInCHINA,[CommissionInHK] = @CommissionInHK,[AuditingTime]=@AuditingTime,[AuditingBy]=@AuditingBy
                         OUTPUT inserted.Id
                         INTO @TMP
                         WHERE Spu=@Spu AND LanguageVersion=@LanguageVersion
                         SELECT @SpuId=ID FROM @TMP";

            var spuParas = db.CreateParameterCollection();

            spuParas.Append("Spu", spuInfo.Spu);
            spuParas.Append("LanguageVersion", spuInfo.LanguageVersion);

            spuParas.Append("Name", spuInfo.Name);
            spuParas.Append("Tag", spuInfo.Tag);
            spuParas.Append("Price", spuInfo.Price);

            spuParas.Append("Description", spuInfo.Description);
            spuParas.Append("BrandId", spuInfo.BrandId);
            spuParas.Append("Brand", spuInfo.Brand);
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

            spuParas.Append("CommissionInCHINA", spuInfo.CommissionInCHINA);
            spuParas.Append("CommissionInHK", spuInfo.CommissionInHK);

            spuParas.Append("AuditingTime", DateTime.Now);
            spuParas.Append("AuditingBy", spuInfo.AuditingBy);

            spuInfo.PreOnSaleTime = spuInfo.PreOnSaleTime == DateTime.MinValue ? DateTime.Now : spuInfo.PreOnSaleTime;

            spuParas.Append("PreOnSaleTime", spuInfo.PreOnSaleTime);

            spuParas.Append("@SpuId", 0, DbType.Int32, ParameterDirection.InputOutput);
            db.ExecuteNonQuery(CommandType.Text, insertSpu, spuParas, tran);
            var id = 0;
            int.TryParse(spuParas["@SpuId"].Value.ToString(), out id);
            return id;
        }

        private void InsertProductExtend(Database db, System.Data.Common.DbTransaction tran, ProductAndExpandInfo productInfo, int spuId)
        {
            var spuEx = productInfo;

            const string insertSpuEx = @"INSERT INTO dbo.ProductInfoExpand(SpuId,Materials,Pattern,Flavour,Ingredients,StoragePeriod,StoringTemperature
                                ,SkinType,Gender,AgeGroup,Model,BatteryTime,Voltage,Power,Warranty,SupportedLanguage,PetType,PetAgeUnit,PetAge,Location,Weight
                                ,WeightUnit,Volume,VolumeUnit,Length,LengthUnit,Width,WidthUnit,Height,HeightUnit,Flavor)
                                VALUES
                                (@SpuId,@Materials,@Pattern,@Flavour,@Ingredients,@StoragePeriod,@StoringTemperature,@SkinType,@Gender,@AgeGroup,@Model
                                ,@BatteryTime,@Voltage,@Power,@Warranty,@SupportedLanguage,@PetType,@PetAgeUnit,@PetAge,@Location,@Weight,@WeightUnit
                                ,@Volume,@VolumeUnit,@Length,@LengthUnit,@Width,@WidthUnit,@Height,@HeightUnit,@Flavor)";

            var spuExParas = db.CreateParameterCollection();
            spuExParas.Append("@SpuId", spuId);
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

        private void ModifyProductExtendInfo(Database db, System.Data.Common.DbTransaction tran, ProductAndExpandInfo productInfo, int spuId)
        {
            var spuEx = productInfo;

            const string insertSpuEx = @"UPDATE [dbo].[ProductInfoExpand]
                           SET [Materials] = @Materials,[Pattern] = @Pattern,[Flavour] = @Flavour,[Ingredients] = @Ingredients,[StoragePeriod] = @StoragePeriod
                              ,[StoringTemperature] = @StoringTemperature,[SkinType] = @SkinType,[Gender] = @Gender,[AgeGroup] = @AgeGroup,[Model] = @Model
                              ,[BatteryTime] = @BatteryTime,[Voltage] = @Voltage,[Power] = @Power,[Warranty] = @Warranty,[SupportedLanguage] = @SupportedLanguage
                              ,[PetType] = @PetType,[PetAgeUnit] = @PetAgeUnit,[PetAge] = @PetAge,[Location] = @Location,[Weight] = @Weight
                              ,[WeightUnit] = @WeightUnit,[Volume] = @Volume,[VolumeUnit] = @VolumeUnit,[Length] = @Length,[LengthUnit] = @LengthUnit
                              ,[Width] = @Width,[WidthUnit] = @WidthUnit,[Height] = @Height,[HeightUnit] = @HeightUnit,[Flavor] = @Flavor
                         WHERE SpuId=@SpuId";
            var spuExParas = db.CreateParameterCollection();
            spuExParas.Append("@SpuId", spuId);
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

        private void InsertSkus(Database db, System.Data.Common.DbTransaction tran, int id, List<SkuInfo> skus)
        {
            const string insertSku = @"INSERT INTO dbo.SkuInfo(SpuId,Spu,Sku,MainDicKey,MainDicValue,SubDicKey,SubDicValue,MainKey,MainValue,SubKey,SubValue
                                ,Price,NetWeight,NetContent,Specifications,Size,Color,AlcoholPercentage,Smell,CapacityRestriction,BarCode,AlarmStockQty
                                ,IsOnSaled,Status,CreateTime,ReportStatus,ShelvesTime)
                                VALUES";

            var skuParas = db.CreateParameterCollection();
            StringBuilder skuValues = new StringBuilder(1024);
            var index = 0;

            if (null == skus)
            {
                return;
            }

            foreach (var sku in skus)
            {
                sku.SpuId = id;

                skuValues.Append("(@SpuId" + index + ",@Spu" + index + ",@Sku" + index + ",@MainDicKey" + index + ",@MainDicValue" + index + ",@SubDicKey" + index + ",@SubDicValue" + index + ",@MainKey" + index + ",@MainValue" + index + ",@SubKey" + index + ",@SubValue" + index + ",@Price" + index + ",@NetWeight" + index + ",@NetContent" + index + ",@Specifications" + index + ",@Size" + index + ",@Color" + index + ",@AlcoholPercentage" + index + ",@Smell" + index + ",@CapacityRestriction" + index + ",@BarCode" + index + ",@AlarmStockQty" + index + ",@IsOnSaled" + index + ",@Status" + index + ",@CreateTime" + index + ",@ReportStatus" + index + ",@ShelvesTime" + index + "),");

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
                skuParas.Append("@Status" + index, 1);
                skuParas.Append("@CreateTime" + index, sku.CreateTime);
                skuParas.Append("@ReportStatus" + index, sku.ReportStatus);
                skuParas.Append("@ShelvesTime" + index, DateTime.Now);
                index++;
            }

            db.ExecuteNonQuery(CommandType.Text, insertSku + skuValues.Remove(skuValues.Length - 1, 1).ToString(), skuParas, tran);
        }

        private void ModifySkuInfo(Database db, System.Data.Common.DbTransaction tran, int spuId, SkuInfo sku, int i)
        {

            var updateSku = @"if EXISTS(SELECT sku FROM SkuInfo(NOLOCK) WHERE sku=@SKU" + i + " AND SpuId=@SpuId" + i + ")" +
                       @"BEGIN
                         UPDATE [dbo].[SkuInfo]
                           SET [MainDicKey] = @MainDicKey" + i + ",[MainDicValue] = @MainDicValue"
                                + i + ",[SubDicKey] = @SubDicKey" + i + ",[SubDicValue] = @SubDicValue" + i + ",[MainKey] = @MainKey" + i + ",[MainValue] = @MainValue" + i + ",[SubKey] = @SubKey" + i + ",[SubValue] = @SubValue" + i + ",[NetWeight] = @NetWeight" + i + ",[NetContent] = @NetContent" + i + ",[Specifications] = @Specifications" + i + ",[Size] = @Size" + i + ",[Color] = @Color" + i + ",[AlcoholPercentage] = @AlcoholPercentage" + i + ",[Smell] = @Smell" + i + ",[CapacityRestriction] = @CapacityRestriction" + i + ",[Price] = @Price" + i + ",[BarCode] = @BarCode" + i + ",[AlarmStockQty] = @AlarmStockQty" + i + ",[IsOnSaled] = @IsOnSaled" + i + ",[ReportStatus] = @ReportStatus"
                                + i + ",[ShelvesTime]=@ShelvesTime" + i + " WHERE SpuId=@SpuId" + i + " AND sku=@SKU" + i +
                        " END " +
                     @"ELSE 
                        BEGIN  
                            INSERT INTO dbo.SkuInfo(SpuId,Spu,Sku,MainDicKey,MainDicValue,SubDicKey,SubDicValue,MainKey,MainValue,SubKey,SubValue
                                ,Price,NetWeight,NetContent,Specifications,Size,Color,AlcoholPercentage,Smell,CapacityRestriction,BarCode,AlarmStockQty
                                ,IsOnSaled,Status,CreateTime,ReportStatus,ShelvesTime)
                                VALUES
                                (@SpuId" + i + ",@Spu" + i + ",@SKU" + i + ",@MainDicKey" + i + ",@MainDicValue" + i + ",@SubDicKey"
                                         + i + ",@SubDicValue" + i + ",@MainKey" + i + ",@MainValue" + i + ",@SubKey" + i
                                         + ",@SubValue" + i + ",@Price" + i + ",@NetWeight" + i + ",@NetContent" + i
                                         + ",@Specifications" + i + ",@Size" + i + ",@Color" + i + ",@AlcoholPercentage" + i + ",@Smell"
                                         + i + ",@CapacityRestriction" + i + ",@BarCode" + i + ",@AlarmStockQty" + i + ",@IsOnSaled"
                                         + i + ",@Status" + i + ",@CreateTime" + i + ",@ReportStatus" + i + ",@ShelvesTime" + i + ")" +
                        " END "


                    ;

            var paras = db.CreateParameterCollection();
            paras.Append("@SpuId" + i, spuId);
            paras.Append("@SKU" + i, sku.Sku);
            paras.Append("@Spu" + i, sku.Spu);

            paras.Append("@MainDicKey" + i, sku.MainDicKey);
            paras.Append("@MainDicValue" + i, sku.MainDicValue);
            paras.Append("@SubDicKey" + i, sku.SubDicKey);

            paras.Append("@SubDicValue" + i, sku.SubDicValue);
            paras.Append("@MainKey" + i, sku.MainKey);
            paras.Append("@MainValue" + i, sku.MainValue);

            paras.Append("@SubKey" + i, sku.SubKey);
            paras.Append("@SubValue" + i, sku.SubValue);
            paras.Append("@Price" + i, sku.Price);
            paras.Append("@NetWeight" + i, sku.NetWeight);

            paras.Append("@NetContent" + i, sku.NetContent);
            paras.Append("@Specifications" + i, sku.Specifications);
            paras.Append("@Size" + i, sku.Size);

            paras.Append("@Color" + i, sku.Color);
            paras.Append("@AlcoholPercentage" + i, sku.AlcoholPercentage);
            paras.Append("@Smell" + i, sku.Smell);

            paras.Append("@CapacityRestriction" + i, sku.CapacityRestriction);

            paras.Append("@BarCode" + i, sku.BarCode);

            paras.Append("@AlarmStockQty" + i, sku.AlarmStockQty);
            paras.Append("@IsOnSaled" + i, sku.IsOnSaled);
            paras.Append("@ReportStatus" + i, sku.ReportStatus);
            paras.Append("@ShelvesTime" + i, DateTime.Now);

            paras.Append("@Status" + i, 1);
            paras.Append("@CreateTime" + i, sku.CreateTime);

            db.ExecuteNonQuery(CommandType.Text, updateSku, paras, tran);
        }

        private void InsertProductImages(Database db, System.Data.Common.DbTransaction tran, ImportSpuModel importSpuModel)
        {
            const string insertProductImage = @"INSERT INTO dbo.ProductImage(SPU,ImagePath,ImageType,SortValue,CreateTime,Createby)
                               VALUES";
            StringBuilder sb = new StringBuilder(1024);
            int index = 0;
            var imgParas = db.CreateParameterCollection();

            if (importSpuModel.Imgs != null && importSpuModel.Imgs.Count() > 0)
            {
                foreach (var img in importSpuModel.Imgs)
                {
                    sb.Append("(@SPU" + index + ",@ImagePath" + index + ",@ImageType" + index + ",@SortValue" + index + ",@CreateTime" + index + ",@Createby" + index + "),");

                    imgParas.Append("@SPU" + index, img.SPU);
                    imgParas.Append("@ImagePath" + index, img.ImagePath);
                    imgParas.Append("@ImageType" + index, img.ImageType);
                    imgParas.Append("@SortValue" + index, img.SortValue);
                    imgParas.Append("@CreateTime" + index, img.CreateTime);
                    imgParas.Append("@Createby" + index, img.Createby);

                    index++;
                }

                db.ExecuteNonQuery(CommandType.Text, insertProductImage + sb.Remove(sb.Length - 1, 1).ToString(), imgParas, tran);
            }
        }

        private void InsertCustomsReport(Database db, System.Data.Common.DbTransaction tran, ImportSpuModel importSpuModel)
        {
            const string insertCustom = @"INSERT INTO SkuCustomsReport([Sku],[CustomsUnit],[InspectionNo],[HSCode],[UOM],[PrepardNo],[GnoCode],[TaxRate],[TaxCode],[ModelForCustoms])
                                      VALUES ";

            var paras = db.CreateParameterCollection();
            StringBuilder customValues = new StringBuilder(1024);
            var index = 0;

            if (null == importSpuModel || importSpuModel.CustomInfos == null || importSpuModel.CustomInfos.Count() == 0)
            {
                return;
            }

            foreach (var cInfo in importSpuModel.CustomInfos)
            {

                customValues.Append("(@Sku" + index + ",@CustomsUnit" + index + ",@InspectionNo" + index + ",@HSCode" + index + ",@UOM" + index + ",@PrepardNo" + index + ",@GnoCode" + index + ",@TaxRate" + index + ",@TaxCode" + index + ",@ModelForCustoms" + index + "),");

                paras.Append("@Sku" + index, cInfo.Sku);
                paras.Append("@CustomsUnit" + index, cInfo.CustomsUnit);
                paras.Append("@InspectionNo" + index, cInfo.InspectionNo);
                paras.Append("@HSCode" + index, cInfo.HSCode);
                paras.Append("@UOM" + index, cInfo.UOM);

                paras.Append("@PrepardNo" + index, cInfo.PrepardNo);
                paras.Append("@GnoCode" + index, cInfo.GnoCode);
                paras.Append("@TaxRate" + index, cInfo.TaxRate);
                paras.Append("@TaxCode" + index, cInfo.TaxCode);
                paras.Append("@ModelForCustoms" + index, cInfo.ModelForCustoms);


                index++;
            }

            db.ExecuteNonQuery(CommandType.Text, insertCustom + customValues.Remove(customValues.Length - 1, 1).ToString(), paras, tran);
        }

        private void DeleteProductImages(Database db, System.Data.Common.DbTransaction tran, string spu)
        {
            const string deleteProductImage = @"DELETE FROM ProductImage WHERE Spu=@Spu AND ImageType=1";
            var deleProductImagePars = db.CreateParameterCollection();

            deleProductImagePars.Append("@Spu", spu);
            db.ExecuteNonQuery(CommandType.Text, deleteProductImage, deleProductImagePars, tran);
        }

        private void DeleteSkuINfos(Database db, System.Data.Common.DbTransaction tran, string spu)
        {
            const string deleteSkus = @"DELETE FROM SkuInfo WHERE Spu=@Spu";
            var paras = db.CreateParameterCollection();

            paras.Append("@Spu", spu);
            db.ExecuteNonQuery(CommandType.Text, deleteSkus, paras, tran);
        }

        private void DeleteSkuINfosBySpuId(Database db, System.Data.Common.DbTransaction tran, int spuId)
        {
            const string deleteSkus = @"DELETE FROM SkuInfo WHERE SpuId=@SpuId";
            var paras = db.CreateParameterCollection();

            paras.Append("@SpuId", spuId);
            db.ExecuteNonQuery(CommandType.Text, deleteSkus, paras, tran);
        }

        private void DeleteCustomsReport(Database db, System.Data.Common.DbTransaction tran, ImportSpuModel importSpuModel)
        {
            const string deleteCustom = @"DELETE FROM SkuCustomsReport WHERE Sku IN ";

            var paras = db.CreateParameterCollection();
            StringBuilder customValues = new StringBuilder(1024);

            if (null == importSpuModel || importSpuModel.CustomInfos == null || importSpuModel.CustomInfos.Count() == 0)
            {
                return;
            }

            int i = 0;
            foreach (var sku in importSpuModel.CustomInfos)
            {
                customValues.Append("@Sku" + i);
                customValues.Append(",");

                paras.Append("@Sku" + i, sku.Sku, DbType.String);
                i++;
            }

            var deleSql = deleteCustom + "(" + customValues.Remove(customValues.Length - 1, 1).ToString() + ")";

            db.ExecuteNonQuery(CommandType.Text, deleSql, paras, tran);
        }

        public List<ProductAndExpandInfo> GetProductAndExpendInfo(string spu)
        {
            const string sql = @"SELECT pt.[Id],[Spu],pt.[CategoryId],pt.[SupplierId],[Name],[Tag],[Price],[Description],[BrandId],b.NameHK [Brand],[CountryOfManufacture]
                ,[SalesTerritory],[Unit],[IsExchangeInCHINA],[IsExchangeInHK],[IsReturn],[MinForOrder],[MinPrice],[NetWeight]
                ,[NetWeightUnit],[NetContent],[NetContentUnit],[IsDutyOnSeller],[LanguageVersion],pt.[Status],[DataSource]
                ,pt.[Createtime],[CreateBy],[ModifyTime],[ModifyBy],[AuditingTime],[AuditingBy],[PreOnSaleTime]
                ,[CommissionInCHINA],[CommissionInHK],[SpuId],[Materials],[Pattern],[Flavour],[Ingredients],[StoragePeriod]
                ,[StoringTemperature],[SkinType],[Gender],[AgeGroup],[Model],[BatteryTime],[Voltage],[Power],[Warranty],[SupportedLanguage]
                ,[PetType],[PetAgeUnit],[PetAge],[Location],[Weight],[WeightUnit],[Volume],[VolumeUnit],[Length],[LengthUnit]
                ,[Width],[WidthUnit],[Height],[HeightUnit],[Flavor]
                FROM [ProductInfo_Temp](NOLOCK) AS pt
                LEFT JOIN SupplierBrand b ON b.Id = pt.BrandId
                LEFT JOIN [ProductInfoExpand_Temp] AS pet
                ON pt.Id = pet.SpuId
                WHERE pt.Status=0 AND pt.Spu=@Spu";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@Spu", spu);
            return DbSFO2OMain.ExecuteSqlList<ProductAndExpandInfo>(sql, parameters).ToList();
        }

        public List<SkuInfo> GetSkuInfos(List<int> spuIds)
        {
            const string sql = @"SELECT * FROM SkuInfo_Temp(NOLOCK) WHERE spuId IN ({0})";

            if (spuIds != null && spuIds.Count > 0)
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();

                StringBuilder sb = new StringBuilder(1000);
                int index = 0;
                foreach (var spuid in spuIds)
                {
                    sb.Append("@spuid" + index + ",");
                    parameters.Append("@spuid" + index, spuid);
                    index++;
                }

                return DbSFO2OMain.ExecuteSqlList<SkuInfo>(String.Format(sql, sb.Remove(sb.Length - 1, 1).ToString()), parameters).ToList();
            }
            else
            {
                return new List<SkuInfo>();
            }

        }

        public List<SpuImgsInfo> GetSpuImgInfos(List<string> spus)
        {
            const string sql = @"SELECT * FROM ProductImage_Temp(NOLOCK) WHERE SPU IN ({0})";

            if (spus != null && spus.Count > 0)
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();

                StringBuilder sb = new StringBuilder(1000);
                int index = 0;
                foreach (var spu in spus)
                {
                    sb.Append("@spu" + index + ",");
                    parameters.Append("@spu" + index, spu);
                    index++;
                }

                return DbSFO2OMain.ExecuteSqlList<SpuImgsInfo>(String.Format(sql, sb.Remove(sb.Length - 1, 1).ToString()), parameters).ToList();
            }
            else
            {
                return new List<SpuImgsInfo>();
            }
        }

        public List<SkuCustomInfo> GetSkuCustomInfos(List<string> skus)
        {
            const string sql = @"SELECT * FROM SkuCustomsReport_Temp(NOLOCK) WHERE SKU IN ({0})";

            if (skus != null && skus.Count > 0)
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();

                StringBuilder sb = new StringBuilder(1000);
                int index = 0;
                foreach (var sku in skus)
                {
                    sb.Append("@sku" + index + ",");
                    parameters.Append("@sku" + index, sku);
                    index++;
                }

                return DbSFO2OMain.ExecuteSqlList<SkuCustomInfo>(String.Format(sql, sb.Remove(sb.Length - 1, 1).ToString()), parameters).ToList();
            }
            else
            {
                return new List<SkuCustomInfo>();
            }
        }



        #endregion

        public List<Models.ProductAuditingLog> ProductAuditingLog(string spu)
        {
            const string sql = @"
                select id,Spu,Status,Reason,CreateTime,CreateBy
                from ProductAuditingLog
                where Spu=@Spu
                order by CreateTime desc ";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("Spu", spu);

            return db.ExecuteSqlList<ProductAuditingLog>(sql, parameters).ToList();
        }

        public DataSet GetProductInventoryList(InventoryListViewModel queryModel, LanguageEnum languageEnum, Models.PagingModel page)
        {
            var sql = @"(SELECT sp.CompanyName,p.Spu,p.Name ProductName,s.BarCode,s.Sku,s.MainValue,s.SubValue,ISNULL(t.Qty,0) Qty,s.AlarmStockQty,
CASE WHEN s.AlarmStockQty>ISNULL(t.Qty,0) THEN '是' ELSE '否' END AS IsLowStockAlarm,s.[Status]
FROM SkuInfo s
INNER JOIN ProductInfo p ON p.Id=s.SpuId
INNER JOIN Supplier sp ON sp.SupplierID = p.SupplierId
LEFT JOIN Stock t ON t.Sku=s.Sku WHERE p.LanguageVersion=@LanguageVersion";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("LanguageVersion", languageEnum);

            #region 查询条件

            StringBuilder query = new StringBuilder();
            if (queryModel.SupplierID > 0)
            {
                query.Append(" AND sp.SupplierId=@SupplierId");
                parameters.Append("SupplierId", queryModel.SupplierID);
            }
            if (!String.IsNullOrWhiteSpace(queryModel.Spu))
            {
                query.Append(" AND p.Spu=@Spu");
                parameters.Append("Spu", queryModel.Spu);
            }
            if (!String.IsNullOrWhiteSpace(queryModel.Sku))
            {
                query.Append(" AND s.sku=@SKU");
                parameters.Append("SKU", queryModel.Sku);
            }
            if (queryModel.IsLowStockAlarm.HasValue)
            {
                if (queryModel.IsLowStockAlarm.Value)
                {
                    query.Append(" AND s.AlarmStockQty>ISNULL(t.ForOrderQty,0)");
                }
                else
                {
                    query.Append(" AND s.AlarmStockQty<=ISNULL(t.ForOrderQty,0)");
                }
            }
            if (!String.IsNullOrWhiteSpace(queryModel.ProductName))
            {
                query.Append(" AND p.Name like @ProductName");
                parameters.Append("ProductName", "%" + queryModel.ProductName + "%");
            }
            if (!String.IsNullOrWhiteSpace(queryModel.BarCode))
            {
                query.Append(" AND s.BarCode = @BarCode");
                parameters.Append("BarCode", queryModel.BarCode);
            }
            if (queryModel.SkuStatus.HasValue)
            {
                query.Append(" AND s.Status=@SkuStatus");
                parameters.Append("SkuStatus", queryModel.SkuStatus.Value);
            }
            else
            {
                query.Append(" AND s.Status IN (1,3,4,5)");
            }

            #endregion
            sql += query.ToString() + ") t ";
            string finallySql = string.Format(@"select * from (select ROW_NUMBER() OVER(order by t.Spu,t.sku) AS RowNum,* from {0}
										) as A where A.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;
                                        SELECT COUNT(1),COUNT(distinct t.Spu) FROM {0};", sql);

            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);
            return db.ExecuteSqlDataSet(finallySql, parameters);
        }
    }
}
