using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using SFO2O.Supplier.Models;
using SFO2O.Supplier.Models.Product;
using System.Data;
using SFO2O.Supplier.Common;

namespace SFO2O.Supplier.DAO
{
    public class SupplierBrandDAL : BaseDao
    {
        public IList<ProductBrandModel> GetBrandBySupplierId(int supplierId, LanguageEnum languageVersion)
        {
            var sql = @" SELECT Id,SupplierId,NameCN as BrandNameSample, NameHK as BrandNameTraditional,NameEN as BrandNameEnglish

                         FROM SupplierBrand(NOLOCK)
                         WHERE SupplierId=@SupplierId and Status=1";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("SupplierId", supplierId);

            return db.ExecuteSqlList<ProductBrandModel>(sql, parameters);
        }

        public PageOf<SupplierBrandModel> GetBrandBySupplierId(int supplierId, PageDTO page)
        {
            var querySql = @"   with cte(BrandId,Spu,sku)
                                as
                                (
                                    select distinct p.BrandId,p.Spu,s.sku 
                                    from ProductInfo p(nolock)
		                                inner join SkuInfo(nolock) s on p.Id=s.SpuId and s.Status=3 and p.LanguageVersion=1
		                                inner join Stock st(nolock) on s.Sku=st.Sku
                                    group by p.BrandId,p.Spu,s.sku,p.MinForOrder 
                                    having sum(st.ForOrderQty) >= p.MinForOrder  	
   
                                )
                                {0}
                                SELECT ROW_NUMBER() OVER(ORDER BY sb.CreateTime desc) AS RowNum,Id,SupplierId,NameCN,NameHK,NameEN,CreateTime,Status,
                                (select count(1) from cte where BrandId = sb.Id) as OnSaleCount
                                FROM SupplierBrand sb(nolock) 
                                WHERE 1=1 and SupplierId=@SupplierId";

            var sql = String.Empty;
            sql = String.Format(querySql, "SELECT * FROM (") + ")a WHERE a.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("SupplierId", supplierId);
            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);

            sql += String.Format(querySql, "SELECT COUNT(1) FROM (") + ") b";

            DataSet ds = db.ExecuteSqlDataSet(sql, parameters);

            return new PageOf<SupplierBrandModel>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                RowCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                Items = DataMapHelper.DataSetToList<SupplierBrandModel>(ds)
            };
        }

        public SupplierBrandModel GetBrandById(int brandId)
        {
            var sql = @"with cte(BrandId,Spu,sku)
                        as
                        (
                        select distinct p.BrandId,p.Spu,s.sku 
                        from ProductInfo p(nolock)
                            inner join SkuInfo(nolock) s on p.Id=s.SpuId and s.Status=3 and p.LanguageVersion=1
                            inner join Stock st(nolock) on s.Sku=st.Sku
                        group by p.BrandId,p.Spu,s.sku,p.MinForOrder 
                        having sum(st.ForOrderQty) >= p.MinForOrder  	
                        )
                        SELECT sb.Id,sb.SupplierId,sb.NameCN,sb.NameHK,sb.NameEN,sb.Logo,sb.Banner,sb.IntroductionCN,sb.IntroductionHK,sb.IntroductionEN,sb.CountryId,d.KeyValue as CountryName,
                        sb.CategoryId,cl.CategoryName,sb.CreateTime,Status,(select count(1) from cte where BrandId = sb.Id) as OnSaleCount
                        FROM SupplierBrand sb(nolock)
                        left join (select KeyName,KeyValue from Dics where DicType = 'CountryOfManufacture' and LanguageVersion=2) as d on sb.CountryId= d.KeyName
                        left join Category_LanguageVersion cl(nolock) on cl.CategoryKey = sb.CategoryId and cl.LanguageVersion=2
                        WHERE sb.Id=@brandId";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("brandId", brandId);

            return db.ExecuteSqlFirst<SupplierBrandModel>(sql, parameters);
        }

        public bool Add(SupplierBrandModel brandinfo)
        {
            var sql = @"INSERT INTO SupplierBrand
                        (SupplierId,NameCN,NameHK,NameEN,Logo,Banner,IntroductionCN,IntroductionHK,IntroductionEN,CountryId,CountryName,CategoryId,CategoryName,CreateTime,Status)
                        VALUES
                        (@SupplierId,@NameCN,@NameHK,@NameEN,@Logo,@Banner,@IntroductionCN,@IntroductionHK,@IntroductionEN,@CountryId,@CountryName,@CategoryId,@CategoryName,@CreateTime,@Status)";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("SupplierId", brandinfo.SupplierId);
            parameters.Append("NameCN", brandinfo.NameCN);
            parameters.Append("NameHK", brandinfo.NameHK);
            parameters.Append("NameEN", brandinfo.NameEN);
            parameters.Append("Logo", brandinfo.Logo);
            parameters.Append("Banner", brandinfo.Banner);
            parameters.Append("IntroductionCN", brandinfo.IntroductionCN);
            parameters.Append("IntroductionHK", brandinfo.IntroductionHK);
            parameters.Append("IntroductionEN", brandinfo.IntroductionEN);
            parameters.Append("CountryId", brandinfo.CountryId);
            parameters.Append("CountryName", brandinfo.CountryName);
            parameters.Append("CategoryId", brandinfo.CategoryId);
            parameters.Append("CategoryName", brandinfo.CategoryName);
            parameters.Append("CreateTime", DateTime.Now);
            parameters.Append("Status", 1);

            return (int)db.ExecuteSqlNonQuery(sql, parameters) > 0;
        }

        public bool UpdateById(SupplierBrandModel brandinfo)
        {
            var sql = @"UPDATE SupplierBrand
                          SET 
                           NameCN = @NameCN
                          ,NameHK = @NameHK
                          ,NameEN = @NameEN
                          ,Logo = @Logo
                          ,Banner = @Banner
                          ,IntroductionCN = @IntroductionCN
                          ,IntroductionHK = @IntroductionHK
                          ,IntroductionEN = @IntroductionEN
                          ,CountryId = @CountryId
                          ,CountryName = @CountryName
                          ,CategoryId = @CategoryId
                          ,CategoryName = @CategoryName
                        WHERE Id=@Id

                          ";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("SupplierId", brandinfo.SupplierId);
            parameters.Append("NameCN", brandinfo.NameCN);
            parameters.Append("NameHK", brandinfo.NameHK);
            parameters.Append("NameEN", brandinfo.NameEN);
            parameters.Append("Logo", brandinfo.Logo);
            parameters.Append("Banner", brandinfo.Banner);
            parameters.Append("IntroductionCN", brandinfo.IntroductionCN);
            parameters.Append("IntroductionHK", brandinfo.IntroductionHK);
            parameters.Append("IntroductionEN", brandinfo.IntroductionEN);
            parameters.Append("CountryId", brandinfo.CountryId);
            parameters.Append("CountryName", brandinfo.CountryName);
            parameters.Append("CategoryId", brandinfo.CategoryId);
            parameters.Append("CategoryName", brandinfo.CategoryName);
            parameters.Append("Id", brandinfo.Id);

            return (int)db.ExecuteSqlNonQuery(sql, parameters) > 0;
        }

        public PageOf<StoreModel> GetStoreListByBrandId(int brandId, int areaId, LanguageEnum languageEnum, PageDTO page)
        {
            var query = @" (SELECT s.Id
                          ,s.BrandId
                          ,s.AreaId
                          ,p.ProvinceName as AreaName
                          ,s.AddressCN
                          ,s.AddressEN
                          ,s.AddressHK
                          ,s.Status
                        FROM Store s(nolock) inner join Province p(nolock) on s.AreaId = p.ProvinceId and p.LanguageVersion=@LanguageVersion
                        WHERE s.BrandId=@BrandId and s.Status=1";

            if (areaId > 0)
            {
                query += " and s.AreaId=@AreaId";
            }

            query += ") pp ";
            string sql = string.Format(@" select * from (select ROW_NUMBER() OVER(order by pp.Id desc) AS RowNum,* from {0}
										) as A where A.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;",
                                  query);

            sql += string.Format(@" SELECT COUNT(1) FROM {0};", query);

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("BrandId", brandId);
            parameters.Append("AreaId", areaId);
            parameters.Append("LanguageVersion", (int)languageEnum);
            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);

            DataSet ds = db.ExecuteSqlDataSet(sql, parameters);

            return new PageOf<StoreModel>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                Items = DataMapHelper.DataSetToList<StoreModel>(ds)
            };
        }

        public bool AddStore(StoreModel store)
        {
            var sql = @" INSERT INTO Store
                               (BrandId
                               ,AreaId
                               ,AddressCN
                               ,AddressEN
                               ,AddressHK
                               ,Status)
                         VALUES
                               (@BrandId
                               ,@AreaId
                               ,@AddressCN
                               ,@AddressEN
                               ,@AddressHK
                               ,@Status)";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("BrandId", store.BrandId);
            parameters.Append("AreaId", store.AreaId);
            parameters.Append("AddressCN", store.AddressCN);
            parameters.Append("AddressEN", store.AddressEN);
            parameters.Append("AddressHK", store.AddressHK);
            parameters.Append("Status", 1);

            return (int)db.ExecuteSqlNonQuery(sql, parameters) > 0;
        }

        public bool UpdateStoreById(StoreModel store)
        {
            var sql = @"UPDATE Store
                        SET 
                           AreaId = @AreaId
                          ,AddressCN = @AddressCN
                          ,AddressEN = @AddressEN
                          ,AddressHK = @AddressHK
                        WHERE Id=@Id";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("AreaId", store.AreaId);
            parameters.Append("AddressCN", store.AddressCN);
            parameters.Append("AddressEN", store.AddressEN);
            parameters.Append("AddressHK", store.AddressHK);
            parameters.Append("Id", store.Id);

            return (int)db.ExecuteSqlNonQuery(sql, parameters) > 0;
        }

        public StoreModel GetStoreById(int id)
        {
            var sql = @"  SELECT Id,BrandId ,AreaId,AddressCN,AddressEN,AddressHK ,Status
                          FROM Store
                          WHERE Id=@Id";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("Id", id);

            return db.ExecuteSqlFirst<StoreModel>(sql, parameters);
        }

        public bool DeleteAddress(int id)
        {
            var sql = @"UPDATE Store
                        SET Status = 0
                        WHERE Id=@Id";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("Id", id);

            return (int)db.ExecuteSqlNonQuery(sql, parameters) > 0;
        }

        public bool ShelfOffBrand(string brandId)
        {
            var sql = @"BEGIN TRY
                          BEGIN TRANSACTION;
	                        update SupplierBrand set Status =0
	                        where Id = @BrandId
                            IF @@ERROR = 0
	                        BEGIN
                                
                                update SkuInfo set Status = 4
		                        from 
		                        ProductInfo p inner join SkuInfo s on p.Id=s.SpuId
		                        where p.BrandId= @BrandId and s.Status=3 

                                update ProductInfo_temp set Status = 4
                                where  BrandId= @BrandId and Status=0

                                update SkuInfo_temp set Status = 4
		                        from 
		                        ProductInfo_temp p inner join SkuInfo_temp s on p.Id=s.SpuId
		                        where p.BrandId= @BrandId and s.Status=0
        
                                IF @@ERROR = 0
		                        BEGIN
			                        SET @Result = 1
		                        END
		                        ELSE 
		                        BEGIN
			                        SET @Result = -1
		                        END
                            END
                            ELSE
                            BEGIN
		                        SET @Result = -1
	                        END
                            IF @Result=1 
                            BEGIN
                                 COMMIT TRANSACTION;
                            END
                            ELSE
                            BEGIN
                                 ROLLBACK TRANSACTION;
                            END
                        END TRY
                        BEGIN CATCH 
                            SET @Result = -1
                            ROLLBACK TRANSACTION;
                        END CATCH";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("BrandId", brandId);
            parameters.Append("@Result", -1, System.Data.DbType.Int32, System.Data.ParameterDirection.InputOutput);
            db.ExecuteSqlNonQuery(sql, parameters);
            bool result = Convert.ToInt32(parameters["@Result"].Value.ToString()) > 0;
            return result;
        }
    }
}
