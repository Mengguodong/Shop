using System.Collections.Generic;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Model;
using SFO2O.Model.Enum;
using SFO2O.Model.Index;
using SFO2O.Utility.Extensions;
using SFO2O.Utility.Uitl;
using System;

namespace SFO2O.DAL.Index
{
    public class IndexModulesDal : BaseDal
    {

        public IList<IndexModulesEntity> GetIndexModules(IndexModuleType type, int top)
        {
            string sql = @"SELECT TOP ({0}) [Id]
      ,[Key]
      ,[Title]
      ,[SubTitle1]
      ,[SubTitle2]
      ,[ImagePath]
      ,[LinkUrl]
      ,[CreateTime]
      ,[Description]
      ,[RefId]
      ,[Sort]
      ,[Status]
  FROM [IndexModules]
WHERE [Key]=@key and status=1
   ORDER BY [Sort]";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("key", type.As(0));
            sql = string.Format(sql, top);
            return db.ExecuteSqlList<IndexModulesEntity>(sql, parameters);

        }
        public IList<IndexModulesEntity> GetAllAvaliableIndexModules()
        {
            string sql = @"SELECT  [Id]
      ,[Key]
      ,[Title]
      ,[SubTitle1]
      ,[SubTitle2]
      ,[ImagePath]
      ,[LinkUrl]
      ,[CreateTime]
      ,[Description]
      ,[RefId]
      ,[Sort]
      ,[Status]
  FROM [IndexModules]
WHERE   status=1
   ORDER BY [Sort]";

            var db = DbSFO2OMain;
            return db.ExecuteSqlList<IndexModulesEntity>(sql);

        }
        /// <summary>
        /// 取随机数据
        /// </summary>
        /// <returns></returns>

        public IList<IndexModulesEntity> GetIndexNewProduct(int top, int language, int SalesTerritory)

        {
            string sql = @"EXEC	[dbo].[sp_SelectIndexNewProduct]
                            @Top = @Top,
			                @LanguageVersion = @language,
			                @SalesTerritory = @SalesTerritory";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@Top", top);
            parameters.Append("@language", language);
            parameters.Append("@SalesTerritory", SalesTerritory);
            sql = string.Format(sql);
            return db.ExecuteSqlList<IndexModulesEntity>(sql, parameters);

        }
        public IList<BannerImagesEntity> GetIndexBannerImages(int channelId)
        {
            string sql = @"SELECT ci.Id
					            ,ci.Title
					            ,ci.ImageUrl
					            ,ci.LinkUrl
					            ,ci.BeginTime
					            ,ci.EndTime
					            ,ci.SortValue
					            ,ci.CreateTime
					            ,ci.CreateBy
                        FROM CMSBannerImages AS ci
                        WHERE ci.BeginTime <= GETDATE() AND ci.EndTime >= GETDATE()
                        AND ci.ChannelId=@channelId
                        ORDER BY ci.SortValue DESC";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("channelId", channelId);
            return db.ExecuteSqlList<BannerImagesEntity>(sql,parameters);
        }

        /// <summary>
        /// 随机获得热搜词
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public IList<HotKeyEntity> GetHotKeyRandomModules(int top)
        {
            string sql = @"SELECT TOP ({0}) 
						        ck.[Id]
						        ,ck.[Content]
						        ,ck.[IsRed]
						        ,ck.[SortValue]
						        ,ck.[CreateTime]
						        ,ck.[CreateBy]
                       FROM CMSHotKey AS ck
                       ORDER BY NEWID()";

            var db = DbSFO2OMain;
            sql = string.Format(sql, top);
            return db.ExecuteSqlList<HotKeyEntity>(sql);
        }

        public IList<BannerImagesEntity> GetIndexBannerImages()
        {
            string sql = @"SELECT ci.Id
					            ,ci.Title
					            ,ci.ImageUrl
					            ,ci.LinkUrl
					            ,ci.BeginTime
					            ,ci.EndTime
					            ,ci.SortValue
					            ,ci.CreateTime
					            ,ci.CreateBy
                        FROM CMSBannerImages AS ci
                        WHERE ci.BeginTime <= GETDATE() AND ci.EndTime >= GETDATE() AND ci.ChannelId = 1 
                        ORDER BY ci.SortValue DESC";

            var db = DbSFO2OMain;
            return db.ExecuteSqlList<BannerImagesEntity>(sql);
        }

        /// <summary>
        /// 取随机数据
        /// </summary>
        /// <returns></returns>
        public IList<IndexModulesEntity> GetIndexCustom(int language, int SalesTerritory)
        {

            string sql = @"EXEC	[dbo].[sp_SelectIndexCustom]
                            @LanguageVersion = @language,
			                @SalesTerritory = @SalesTerritory";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@language", language);
            parameters.Append("@SalesTerritory", SalesTerritory);
            sql = string.Format(sql);
            return db.ExecuteSqlList<IndexModulesEntity>(sql, parameters);

        }

        #region 获取热门商品模块数据 V2.0
        public IList<IndexModulesProductEntity> GetAllCMSHotProducts(int language = 1, int salesTerritory = 1)
        {
            #region SQL语句
            string sql = @"
                            with sputb(spu,shelvesTime,Qty)
                            As
                            (select spu,shelvesTime,ForOrderQty from (select ROW_NUMBER() over(PARTITION by k.SPU order by k.shelvesTime desc) as rindex,k.spu,k.shelvesTime ,s.ForOrderQty
                            from SkuInfo k
                            left join stock s on s.Spu=k.Spu and s.Sku=k.Sku
                             inner join productinfo p on p.Id=k.SpuId and p.LanguageVersion=@language AND p.SalesTerritory IN(@salesTerritory,3)
                            where k.IsOnSaled=1 and k.[Status]=3) a where rindex<2
                             ),
                            skuQty(Spu,SkuCount,QtyCount)
                            As(
                            select k.Spu,count(s.sku),sum(isnull(s.ForOrderQty,0)) from SkuInfo k
                            inner join ProductInfo p on p.Id=k.SpuId and p.LanguageVersion=@language AND p.SalesTerritory IN(@salesTerritory,3)
                            left join Stock		s on s.Spu=k.Spu and k.Sku=s.Sku
                            group by k.Spu
                            ),
                            promotionSpu
	                        As(
	                           select * from (SELECT b.spu, b.sku,DiscountPrice, DiscountRate, ROW_NUMBER() over(partition by b.spu order by DiscountPrice ASC ) as rownum FROM (
                                SELECT ps.PromotionId, ps.Sku, ps.DiscountRate, ps.DiscountPrice,ps.Spu
                                  FROM PromotionSku AS ps
                                  inner JOIN promotions p ON ps.PromotionId=p.Id  
                                WHERE  p.PromotionStatus=2 and p.PromotionType!=2
                                )b)tt where rownum<2
	                        ),
                            newtb
                            AS
                            (
                                select isnull(psp.DiscountPrice,p.MinPrice) as  DiscountPrice,isnull(psp.DiscountRate,0) AS DiscountRate,
                                     p.Id as ProductId,p.Spu,p.Name,case when psp.DiscountPrice is null then p.MinPrice else 
			                         case when isnull(psp.DiscountPrice,0)>isnull(p.MinPrice,0) then isnull(p.MinPrice,0) else isnull(sk.Price,0) end 
			                         end as MinPrice,p.[Description],
                                    p.Brand,'" + DomainHelper.ImageUrl + @"'+ replace(replace(ImagePath,'\','/'),'.','_640.') as ImagePath,
                                    p.Unit,p.CategoryId ,(skq.QtyCount-p.MinForOrder) as Qty,1 as IsOnSaled,skq.SkuCount,sp.shelvesTime
                                from ProductInfo p
                                --inner join supplierbrand brand on brand.id=p.brandid and brand.[Status]=1
                                inner join sputb sp on sp.spu=p.Spu
                                left join skuQty skq on skq.Spu=p.Spu
                                left join ProductImage  i on i.SPU=p.Spu
                                left join promotionSpu psp on psp.Spu=p.Spu 
                                left join SkuInfo sk on sk.SpuId=p.Id and sk.Sku=psp.Sku
                                where i.SortValue=1 and p.LanguageVersion=@language AND p.SalesTerritory IN(@salesTerritory,3)
                                ),
                             tb
	                         As(
		                        select  ROW_NUMBER() over(order by newtb.shelvesTime desc) as rindex,* from newtb
	                         )
                             select 
		                        cp.CategoryId,
		                        cp.Spu,
		                        cp.IndexValue,
		                        cp.SortValue,
		                        tb.DiscountPrice,
		                        tb.DiscountRate,
		                        tb.MinPrice,
		                        tb.Name,
		                        tb.ImagePath,
		                        tb.Unit,
		                        tb.Qty,
		                        clv.CategoryName
                             from CMSHotProduct AS cp
                             INNER JOIN tb 
                             ON cp.Spu=tb.spu
                             LEFT JOIN Category_LanguageVersion AS clv
                             ON cp.CategoryId=clv.CategoryKey AND clv.LanguageVersion=@language
                             ORDER BY cp.IndexValue ASC,cp.SortValue ASC";
            #endregion
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@language", language);
                parameters.Append("@salesTerritory", salesTerritory);
                return DbSFO2ORead.ExecuteSqlList<IndexModulesProductEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                return new List<IndexModulesProductEntity>();
            }
        }
        #endregion
    }
}
