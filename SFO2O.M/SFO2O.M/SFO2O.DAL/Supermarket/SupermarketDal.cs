using SFO2O.Model.Product;
using SFO2O.Utility.Uitl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Utility.Extensions;
using System.Collections.Generic;
using System.Data;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Model;
using SFO2O.Model.Enum;
using SFO2O.Model.Index;
using SFO2O.Model.Supermarket;
namespace SFO2O.DAL.Supermarket
{
    public class SupermarketDal : BaseDal
    {
        public List<ProductInfoModel> GetMarketProductListNew(string[] categoryArray, int level, List<int> brandIds, int sort, int pageindex, int pagesize, int language, int deliveryRegion, out int totalRecords)
        {
            string mainFilter = string.Empty;

            if (brandIds != null && brandIds.Any())
            {
                string ids = string.Join(",", brandIds.ToArray());
                mainFilter = " and p.BrandId in(" + ids + ")";
            }

            string orderby = string.Empty;
            if (sort == 1)
            {
                orderby = "order by newtb.shelvesTime desc";
            }
            else if (sort == 2)
            {
                orderby = "order by DiscountPrice asc";
            }
            else if (sort == 3)
            {
                orderby = "order by DiscountPrice desc";
            }
            int startIndex = (pageindex - 1) * pagesize + 1;
            int endIndex = pageindex * pagesize;


            StringBuilder CategoryIdwhere = new StringBuilder();
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@LanguageVersion", language);

            if (categoryArray.Length > 1)
            {
                CategoryIdwhere.Append("(p.CategoryId ");
                for (var i = 0; i < categoryArray.Length; i++)
                {
                    parameters.Append("@CategoryId" + i, categoryArray[i]);
                    if (i > 0)
                    {
                        CategoryIdwhere.Append("OR p.CategoryId ");
                    }


                    CategoryIdwhere.Append("=").Append("@CategoryId" + i).Append(" ");

                    if (i == categoryArray.Length - 1)
                    {
                        CategoryIdwhere.Append(")");
                    }
                }
            }
            else
            {
                parameters.Append("@CategoryId", categoryArray[0]);
                CategoryIdwhere.Append("p.CategoryId = ").Append("@CategoryId");
            }

            parameters.Append("@StartIndex", startIndex);
            parameters.Append("@EndIndex", endIndex);
            parameters.Append("@SalesTerritory", deliveryRegion);

            string whereStr = " and " + CategoryIdwhere + " and (p.SalesTerritory=3 or p.SalesTerritory=@SalesTerritory)";
            if (level == 0)
            {

                whereStr = " and p.Categoryid in(SELECT DISTINCT c.CategoryId FROM Category AS c WHERE c.RootId=@CategoryId) and (p.SalesTerritory=3 or p.SalesTerritory=@SalesTerritory)";
            }
            else if (level == 1)
            {
                whereStr = " and p.Categoryid in(select CategoryId from Category where ParentId=@CategoryId and categorylevel=2) and (p.SalesTerritory=3 or p.SalesTerritory=@SalesTerritory)";
            }

            string sql = @"with sputb(spu,shelvesTime,Qty)
                            As
                            (select spu,shelvesTime,ForOrderQty from (select ROW_NUMBER() over(PARTITION by k.SPU order by k.shelvesTime desc) as rindex,k.spu,k.shelvesTime ,s.ForOrderQty
                            from SkuInfo k
                            left join stock s on s.Spu=k.Spu and s.Sku=k.Sku
                            --left join ProductInfoExpand pd on pd.SpuId=k.SpuId
                            inner join productinfo p on p.Id=k.SpuId and p.LanguageVersion=@LanguageVersion
                            where k.IsOnSaled=1 and k.[Status]=3" + mainFilter + @") a where rindex<2
                             ) ,
			                skuQty(Spu,SkuCount,QtyCount)
			                As(
			                select k.Spu,count(s.sku),sum(isnull(s.ForOrderQty,0)) from SkuInfo k
                            inner join ProductInfo p on p.Id=k.SpuId and p.LanguageVersion=@LanguageVersion
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
	                            select  isnull(psp.DiscountPrice,p.MinPrice) as  DiscountPrice,psp.DiscountRate,
		                             p.Id as ProductId,p.Spu,p.Name,case when psp.DiscountPrice is null then p.MinPrice else 
									 case when psp.DiscountPrice>p.MinPrice then p.MinPrice else sk.Price end 
									 end as MinPrice,
                                    p.Brand,'" + DomainHelper.ImageUrl + @"'+ replace(replace(ImagePath,'\','/'),'.','_640.') as ImagePath,
                                    p.Unit,p.CategoryId ,(skq.QtyCount-p.MinForOrder) as Qty,1 as IsOnSaled,skq.SkuCount,sp.shelvesTime
	                            from ProductInfo p
                                --inner join supplierbrand brand on brand.id=p.brandid and brand.[Status]=1
	                            inner join sputb sp on sp.spu=p.Spu
                                left join skuQty skq on skq.Spu=p.Spu
	                            left join ProductImage  i on i.SPU=p.Spu
                                left join promotionSpu psp on psp.Spu=p.Spu 
                                left join SkuInfo sk on sk.SpuId=p.Id and sk.Sku=psp.Sku 
		                        where i.SortValue=1 and p.LanguageVersion=@LanguageVersion " + whereStr + @"
	                            ),
                             tb
							 As(
								select  ROW_NUMBER() over(" + orderby + @") as rindex,* from newtb
							 )
                             select *,(select count(1) from tb) as TotalRecord 
                             from tb
                            where rindex between @StartIndex and @EndIndex";

            
            var list = DbSFO2ORead.ExecuteSqlList<ProductInfoModel>(sql, parameters);
            if (list.Any())
            {
                totalRecords = list.FirstOrDefault().TotalRecord;
            }
            else
            {
                list = new List<ProductInfoModel>();
                totalRecords = 0;
            }
            return list.ToList();
        }

        public IList<BerserkProductEntity> GetBerserkProduct(int top, int language)
        {
            string sql = @"with prolist as( select top {0}
			p.Name,
			p.Unit,
			im.ImagePath,
			isnull(promotion.DiscountPrice,0)DiscountPrice,
			isnull(promotion.DiscountRate,0)DiscountRate,
			p.MinPrice,
			p.Spu,
			p.Id,
			p.MinForOrder,
			qtylist.Qty as ForOrderQty
			
			from 
				(		
				 select tempProduect.Spu,img.ImagePath,tempProduect.SortValue from 
					(
						select CMSCustomProduct.SortValue,Spu 
						from CMSCustomProduct 
						where BannerId in 
							(select CMSCustomBanner.Id 
						from CMSCustomBanner 
						where ModuleId in (select ID from CMSCustomModule where ChannelId=2)
							)
					) as tempProduect
					left join ProductImage img on tempProduect.Spu=img.Spu and img.SortValue=1
				) im
				left join
				(
				select * 
				from 
				(
				select ROW_NUMBER() over(PARTITION by SPU order by discountPrice asc) rindex,
				pk.* 
				from PromotionSku pk
				inner join
				Promotions po 
				on pk.PromotionId=po.Id 
				and po.PromotionStatus=2 --状态,进行中
				and po.PromotionType=1--折扣类型,打折
				)a
				where rindex<2
				) promotion
			on im.Spu=promotion.spu
			inner join ProductInfo p 
			on p.Spu=im.Spu and p.LanguageVersion=1 
			
			INNER JOIN (
				SELECT s.SpuId,s.Spu,MAX(s.ShelvesTime) AS ShelvesTime,sum(st.ForOrderQty) AS Qty,COUNT(DISTINCT s.Sku) SkuCount FROM SkuInfo s
                INNER JOIN Stock AS st ON s.Sku=st.Sku 
                INNER JOIN ProductInfo AS p ON s.SpuId=p.Id and (SalesTerritory=1 or SalesTerritory=3)
                WHERE s.IsOnSaled=1 AND s.[Status]=3 
                AND p.LanguageVersion=@language
                GROUP BY s.SpuId,s.Spu	
			) qtylist ON qtylist.Spu = im.Spu
			
			WHERE EXISTS (SELECT * FROM SkuInfo AS si WHERE si.Spu=p.Spu AND si.[Status]=3 and si.IsOnSaled=1)
			order by im.SortValue desc )
			select  p.* from prolist as p 
";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("language", language);
            sql = string.Format(sql, top);
            return db.ExecuteSqlList<BerserkProductEntity>(sql, parameters);
        }

        public IList<SingleBannerImagesEntity> GetIndexSingleBannerImage(int channelId=2)
        {
//            string sql = @"SELECT TOP 1 [Id]
//                              ,[ChannelId]
//                              ,[Title]
//                              ,[ImageUrl]
//                              ,[LinkUrl]
//                              ,[CreateTime]
//                              ,[CreateBy]
//                          FROM [SFO2O].[dbo].[CMSSingleBanner] where ChannelId=@channelId";
            string sql = @"SELECT TOP 1 cm.ChannelId,cb.Title,cb.Id,cb.ImageUrl,cb.LinkUrl,cb.CreateTime,cb.CreateBy
                            FROM [CMSCustomBanner](NOLOCK) cb
                            INNER JOIN CMSCustomModule cm ON cm.Id = cb.ModuleId
                            WHERE cm.ChannelId=@channelId";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("channelId", channelId);
            return db.ExecuteSqlList<SingleBannerImagesEntity>(sql, parameters);
        }
    }
}
