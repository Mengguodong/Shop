using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Model.Supplier;
using SFO2O.Utility.Uitl;

namespace SFO2O.DAL.SupplierBrand
{
    public class SupplierBrandDal : BaseDal
    {






        /*、
         * 1、	调取同品类下的其他品牌，优先同品类同商家，其次同品类，最多5个（排重）；
         * 2、	按照添加品牌下在售SKU数量倒序排列；
         */

        /// <summary>
        /// 获取相似品牌信息信息
        /// </summary>
        /// <param name="top"></param>
        /// <param name="brandId"></param>
        /// <param name="supplierId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IList<BrandEntity> GeSupplierBrandEntities(int top, int brandId, string supplierId, int categoryId )
        {
            if (brandId == 0)
            {
                throw new ArgumentException("brandId");
            }
            if (string.IsNullOrEmpty(supplierId) )
            {
                throw new ArgumentException("supplierId");
            }
            if (categoryId == 0)
            {
                throw new ArgumentException("categoryId");

            }
            try
            {
                //DECLARE  @CategoryId INT
                // DECLARE  @BrandId INT
                // DECLARE  @Spu INT
                // DECLARE  @supplierId NVARCHAR(50) 
                // SET @supplierId='18'
                string sql = @"  	
	                                ;WITH brandSkus
	                                 AS 
	                                 ( 
	                                SELECT  p.BrandId,COUNT(distinct s.Sku) AS skuCount FROM SkuInfo  s 
	                                INNER JOIN ProductInfo AS p ON s.Spu=p.Spu
	                                 WHERE s.IsOnSaled=1 AND s.[Status]=3   
	                                GROUP BY p.BrandId
	                                 )
	                                --同商家优先
	                                SELECT TOP (@top) sb.[Id]
                                      ,[SupplierId]
                                      ,[NameCN]
                                      ,[NameHK]
                                      ,[NameEN]
                                      ,[Logo]
                                      ,[Banner]
                                      ,[IntroductionCN]
                                      ,[IntroductionHK]
                                      ,[IntroductionEN]
                                      ,[CountryId]
                                      ,d.KeyValue AS CountryName
                                      ,d.NationalCode AS NationalFlag
                                      ,sb.[CategoryId]
                                      ,bc.[CategoryName]
                                      ,sb.[CreateTime]
                                      ,sb.[Status],bs.skuCount FROM SupplierBrand AS sb
	                                LEFT JOIN brandSkus AS bs ON sb.Id=bs.BrandId
                                    INNER JOIN BrandCategory AS bc ON bc.BrandId = sb.Id
                                    inner join Category as c on c.RootId=bc.CategoryId
                                    LEFT JOIN Dics AS d ON d.KeyName=sb.CountryId AND d.DicType='CountryOfManufacture' 
									                        AND d.LanguageVersion=1
	                                 WHERE c.CategoryId=@CategoryId AND sb.id<>@BrandId AND sb.[Status]=1
	                                ORDER BY charindex(','+ltrim(SupplierId)+',',','+@SupplierId+',') DESC  ,bs.skuCount DESC 	 
	                                 ";

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@top", top);
                parameters.Append("@BrandId", brandId);
                parameters.Append("@CategoryId", categoryId);
                parameters.Append("@SupplierId", supplierId);

                return DbSFO2ORead.ExecuteSqlList<BrandEntity>(sql, parameters);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

    }
}
