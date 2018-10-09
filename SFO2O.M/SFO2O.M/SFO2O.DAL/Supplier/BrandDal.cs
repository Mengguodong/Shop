using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Model.Supplier;
using System.Data;
using SFO2O.Model.Product;
using SFO2O.Utility.Uitl;

namespace SFO2O.DAL.Supplier
{
    public class BrandDal : BaseDal
    {
        /// <summary>
        /// 品牌信息
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public BrandEntity GetBrandInfo(int brandId,int language)
        {
            string sql = @"Select brand.Id,brand.SupplierId,brand.NameCN,brand.NameHK,brand.NameEN,brand.Logo,brand.Banner,brand.IntroductionCN,
                            brand.IntroductionHK,brand.IntroductionEN,brand.CreateTime,brand.Status, d.KeyValue as CountryName ,d.NationalCode AS NationalFlag,brand.Slogan
                        From SupplierBrand brand
                        inner join Dics d on brand.CountryId=d.KeyName and d.DicType='CountryOfManufacture' and d.LanguageVersion=1
                        --INNER JOIN BrandCategory AS bc ON bc.BrandId=brand.Id
                        where brand.Id=@Id and brand.Status=1";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@Id", brandId);
            parameters.Append("@Language", language);
            return DbSFO2ORead.ExecuteSqlFirst<BrandEntity>(sql, parameters);
        }

        /// <summary>
        /// 品牌信息
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="language"></param>
        /// <returns></returns>

        //            //var parameters = DbSFO2ORead.CreateParameterCollection();
        //            //parameters.Append("@Id", categoryId);
        //            sql = string.Format(sql, categoryId);
        //            DataSet ds = DbSFO2ORead.ExecuteDataSet(CommandType.Text, sql);

        //            IList<BrandInfo> list = new List<BrandInfo>();
        //            if (ds != null && ds.Tables[0].Rows.Count > 0)
        //            {
        //                foreach (DataRow r in ds.Tables[0].Rows)
        //                {
        //                    list.Add(new BrandInfo() { 
        //                        Id = Convert.ToInt32(r["Id"]),
        //                        Name = r["Name"].ToString()
        //                    });
        //                }
        //            }
        //            return list;
        //        }

        public IList<BrandInfo> GetBrandListByCategoryId(string categoryId, int level, int language = 1, int salesTerritory = 1)
        {
            try
            {
                string categoryIds = "select * from cids";
                if (level == 0)
                {
                    categoryIds = "(SELECT DISTINCT c.CategoryId FROM Category AS c WHERE c.RootId in (select * from cids) and categorylevel=2)";
                }
                else if (level == 1)
                {
                    categoryIds = "(select CategoryId from Category where ParentId in (select * from cids) and categorylevel=2)";
                }

                string sql = @"DECLARE @str NVARCHAR(500)
					SET @str='{0}'
					;with cids
					as(
					select * from dbo.func_splitidString(@str,',') AS fs
					)					
					select distinct p.BrandId as Id,sb.NameCN AS Name from productinfo p 
                    inner join SkuInfo k on k.SpuId=p.Id and k.IsOnSaled=1 and k.[Status]=3
                    INNER JOIN SupplierBrand AS sb ON p.BrandId=sb.Id AND sb.[Status]=1 AND sb.NameCN IS NOT NULL
                    where p.CategoryId in({1}) and p.LanguageVersion=@language
                    and (p.SalesTerritory=3 or p.SalesTerritory=@salesTerritory)  
                    and p.BrandId is not null and p.Brand is not null";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@language", language);
                parameters.Append("@salesTerritory", salesTerritory);
                //格式化传入分类IDs
                sql = string.Format(sql, categoryId, categoryIds);

                return DbSFO2ORead.ExecuteSqlList<BrandInfo>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<BrandInfo>();
            }
        }

        /// <summary>
        /// 取品牌的在售SPU数量
        /// </summary>
        /// <param name="brandId">品牌id</param>
        /// <param name="language">语言版本</param>
        /// <param name="deliveryRegion">销售区域</param>
        /// <returns></returns>
        public int GetBrandSaleSpuCount(int brandId, int language, int deliveryRegion)
        {
            string sql = @";with spucnt(spu,spuCount)
                            As
                            (
                            select p.spu,sum(s.ForOrderQty) from ProductInfo p
                            inner join Stock s on s.Spu=p.Spu
                            inner join SkuInfo sk on sk.SpuId=p.Id and sk.Sku=s.Sku
                             where p.LanguageVersion=@Language and sk.[Status] = 3 and (p.SalesTerritory =@Region or p.SalesTerritory=3)
                            group by p.Spu 
                            )

                            select count(p.spu) from ProductInfo p
                            inner join spucnt st on st.spu=p.spu
                            where p.LanguageVersion=@Language and (p.SalesTerritory =@Region or p.SalesTerritory=3) and st.spuCount-p.MinForOrder>0 and BrandId=@BrandId;";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@BrandId", brandId);
            parameters.Append("@Language", language);
            parameters.Append("@Region", deliveryRegion);

            return DbSFO2ORead.ExecuteSqlScalar<int>(sql, parameters);

        }
        /// <summary>
        /// 取品牌的门店列表
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public IList<StoreEntity> GetStoreListByBrandId(int brandId, int language)
        {
            string sql = @"select s.*,a.ProvinceName as AreaName from Store s
                        inner join Province a on a.ProvinceId=s.Areaid and a.LanguageVersion=@Language
                        Where BrandId=@BrandId  And [Status]=1 Order By Id desc";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@BrandId", brandId);
            parameters.Append("@Language", language);
            return DbSFO2ORead.ExecuteSqlList<StoreEntity>(sql, parameters);
        }
        /// <summary>
        /// 品牌街
        /// </summary>
        public IList<BrandEntity> getBrandStreetList(int pageindex, int pagesize)
        {
            int startIndex = (pageindex - 1) * pagesize + 1;
            int endIndex = pageindex * pagesize;
            string sql = @"with sputb
                            AS
                            (SELECT *,ROW_NUMBER() over(order BY  splitType desc, CreateTime DESC)as rindex
                            FROM(
                              
                            select TOP 100 PERCENT  sb.CreateTime,cs.BrandId AS Id,sb.Banner ,sb.NameCN,sb.CountryName,SortValue AS splitType,sb.Slogan
                            FROM CMSBrandShow AS cs
                            LEFT JOIN SupplierBrand AS sb ON sb.Id = cs.BrandId
                            LEFT JOIN ProductInfo AS pi1 ON pi1.BrandId = cs.BrandId
                            LEFT JOIN SkuInfo AS si ON si.SpuId = pi1.Id
                            WHERE EXISTS (SELECT * FROM SkuInfo AS si2 WHERE si2.Spu=si.Spu AND si2.[Status]=3)
                            AND pi1.LanguageVersion=1
                            AND sb.[Status]=1
                            --ORDER BY sb.CreateTime ASC
                            UNION
                            select TOP 100 PERCENT sb.CreateTime,sb.Id AS Id,sb.Banner,sb.NameCN,sb.CountryName,0 AS splitType,sb.Slogan
                            FROM SupplierBrand AS sb
                            LEFT JOIN ProductInfo AS pi1 ON pi1.BrandId = sb.Id
                            LEFT JOIN SkuInfo AS si ON si.SpuId = pi1.Id
                            WHERE EXISTS (SELECT * FROM SkuInfo AS si2 WHERE si2.Spu=si.Spu AND si2.[Status]=3)
                            AND pi1.LanguageVersion=1
                            AND sb.[Status]=1
                            AND sb.Id NOT IN (SELECT cs.BrandId
                                    FROM CMSBrandShow AS cs)
                            ) AS ab) 
                            select *,(select count(1) from sputb) as TotalRecord
                            from sputb
                            where rindex between @StartIndex and @EndIndex 
                            ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@StartIndex", startIndex);
            parameters.Append("@EndIndex", endIndex);
            return DbSFO2ORead.ExecuteSqlList<BrandEntity>(sql, parameters);
        }
        /// <summary>
        /// 明星商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ExchangeRate"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<ProductInfoModel> getProductList(int id, decimal ExchangeRate, int userId)
        {
            string sql = string.Empty;
            if (userId == 0)
            {
                sql = @"SELECT DISTINCT bsp.Spu,bsp.SortValue,ps.DiscountRate,CASE WHEN ISNULL(bsp.ImageUrl,'')  = '' 
                            THEN (SELECT pi3.ImagePath
                                    FROM ProductImage AS pi3 WHERE pi3.Spu=pi2.Spu AND pi3.SortValue=1)
                            ELSE bsp.ImageUrl 
                            END 
                            AS ImagePath,pi2.Name,bsp.[Description] as Description,ps.DiscountPrice * @ExchangeRate as DiscountPrice,pi2.MinPrice * @ExchangeRate as MinPrice,pi2.Spu AS SPU
                           ,CASE 
                            WHEN (SELECT isnull(SUM(s.ForOrderQty),0) FROM Stock AS s WHERE s.Spu=si.Spu)>
                            pi2.MinForOrder THEN 1
                            ELSE 2 END  as compare
                              FROM BrandStarProduct AS bsp
                            LEFT JOIN ProductInfo AS pi2 ON pi2.Spu = bsp.Spu AND pi2.BrandId = bsp.BrandId
                            LEFT JOIN SkuInfo AS si ON si.SpuId = pi2.Id
                            LEFT JOIN Stock AS s ON s.Spu = si.Spu AND s.Sku = si.Sku
                            LEFT JOIN PromotionSku AS ps ON ps.Spu=si.Spu AND ps.Sku = si.Sku
                             AND ps.PromotionId IN(
							SELECT p.Id FROM Promotions AS p WHERE p.PromotionStatus=2 AND p.PromotionType=1
                                                        )
                            WHERE pi2.LanguageVersion=1 AND bsp.BrandId=@id
                            AND EXISTS (SELECT * FROM SkuInfo AS si2 WHERE si2.[Status]=3 AND pi2.LanguageVersion=1 AND si2.Spu=bsp.Spu) ORDER BY bsp.SortValue asc";
            }
            else
            {
                sql = @"SELECT DISTINCT bsp.Spu,bsp.SortValue,fi.spu AS fiSpu,ps.DiscountRate,CASE WHEN ISNULL(bsp.ImageUrl,'')  = '' 
                            THEN (SELECT pi3.ImagePath
                                    FROM ProductImage AS pi3 WHERE pi3.Spu=pi2.Spu AND pi3.SortValue=1)
                            ELSE bsp.ImageUrl 
                            END 
                            AS ImagePath,pi2.Name,bsp.[Description] as Description,ps.DiscountPrice * @ExchangeRate as DiscountPrice,pi2.MinPrice * @ExchangeRate as MinPrice,pi2.Spu AS SPU
                           ,CASE 
                            WHEN (SELECT isnull(SUM(s.ForOrderQty),0) FROM Stock AS s WHERE s.Spu=si.Spu)>
                            pi2.MinForOrder THEN 1
                            ELSE 2 END  as compare
                              FROM BrandStarProduct AS bsp
                            LEFT JOIN ProductInfo AS pi2 ON pi2.Spu = bsp.Spu AND pi2.BrandId = bsp.BrandId
                            LEFT JOIN SkuInfo AS si ON si.SpuId = pi2.Id
                            LEFT JOIN Stock AS s ON s.Spu = si.Spu AND s.Sku = si.Sku
                            LEFT JOIN PromotionSku AS ps ON ps.Spu=si.Spu AND ps.Sku = si.Sku
                             AND ps.PromotionId IN(
							SELECT p.Id FROM Promotions AS p WHERE p.PromotionStatus=2 AND p.PromotionType=1
                                                        )
                            LEFT JOIN FavoriteInfo AS fi ON fi.Spu = pi2.Spu AND fi.userId=@userId AND fi.isDelete=0
                            WHERE pi2.LanguageVersion=1 AND bsp.BrandId=@id
                            AND EXISTS (SELECT * FROM SkuInfo AS si2 WHERE si2.[Status]=3 AND pi2.LanguageVersion=1 AND si2.Spu=bsp.Spu) ORDER BY bsp.SortValue asc";
            }
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@id", id);
            parameters.Append("@ExchangeRate", ExchangeRate);
            parameters.Append("@userId", userId);
            return DbSFO2ORead.ExecuteSqlList<ProductInfoModel>(sql, parameters);
        }
    }
}
