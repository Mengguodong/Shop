using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Model.Account;
using SFO2O.Model.Common;
using SFO2O.Model.Product;
using SFO2O.Utility.Extensions;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Category;
using SFO2O.M.ViewModel.Product;

namespace SFO2O.DAL.Product
{
    public class ProductDal : BaseDal
    {
        /// <summary>
        /// 根据product获取所有相关sku信息
        /// SPU状态 -3、操作完成 -2、新建草稿 -1、编辑草稿 0、待审核 1、待上架 2、已驳回 3、 已上架 4、已下架 5、系统下架
        /// </summary>
        /// <param name="spu"></param> 
        /// <returns></returns>
        public DataSet GetProductSku(string spu, int language, int userId)
        {
            if (string.IsNullOrEmpty(spu))
            {
                throw new ArgumentException("spu");
            }
            try
            {
                string str0 = "";
                string str1 = "";
                string str2 = "";
                if (userId != 0)
                {
                    str0 = @",fi.id AS FavoriteId
                            ,fi.spu AS FavoriteSpu
                            ,fi.isDelete AS isDelete
                            ,fi.userId AS fiUserId";

                    str1 = "LEFT JOIN FavoriteInfo AS fi ON fi.spu=p.Spu  AND fi.userId=@userId AND fi.isDelete=0";
                }

                string sql = @"   SELECT p.Id, p.Spu, p.CategoryId, p.SupplierId, p.Name, p.Tag, p.Price AS ProductPrice, p.[Description],
                            p.Brand, p.CountryOfManufacture, p.SalesTerritory, p.Unit,p.IsExchangeInCHINA,p.IsExchangeInHK,p.IsDutyOnSeller,
                            p.IsReturn, p.MinForOrder, p.MinPrice, p.NetWeightUnit, p.LanguageVersion,
                            s.Sku, s.Price, s.BarCode, s.AlarmStockQty, s.CreateTime, s.AuditTime,
                            s.ShelvesTime, s.RemovedTime, s.ReportStatus, s.IsCrossBorderEBTax, s.PPATaxRate/100 AS PPATaxRate, s.CBEBTaxRate/100 AS CBEBTaxRate,
                            s.[Status], s.IsOnSaled,
                            s.MainDicKey, s.MainDicValue, s.SubDicKey,
                            s.SubDicValue, s.MainKey, s.MainValue, s.SubKey,
                            s.Specifications,s.NetWeight,
                            s.SubValue,sk.ForOrderQty as Qty
                            ,p.brandId,sb.NameCN, sb.NameHK, sb.NameEN, sb.Logo, sb.IntroductionCN,d.KeyValue AS CountryName,d.NationalCode AS NationalFlag
                            ,s.ConsumerTaxRate/100 AS ConsumerTaxRate,s.VATTaxRate/100 AS VATTaxRate
                           {0}
                            FROM productInfo p (NOLOCK)
                            LEFT JOIN skuinfo s ON s.SpuId=p.Id  
                            LEFT JOIN stock sk ON s.Sku=sk.Sku 
                            LEFT JOIN SupplierBrand AS sb ON sb.Id=p.BrandId
                            {1}
                            LEFT JOIN Dics AS d ON d.KeyName=sb.CountryId AND d.DicType='CountryOfManufacture'
										                                  AND d.LanguageVersion=1
                            WHERE s.IsOnSaled=1 AND s.[status] = 3 AND p.LanguageVersion=@LanguageVersion AND p.Spu=@spu
                            SELECT [Id]
                                  ,[SPU]
                                  ,[ImagePath]
                                  ,[ImageType]
                                  ,[SortValue]
                                  ,[Createtime]
                                  ,[Createby]
                              FROM [ProductImage] (NOLOCK)
                            WHERE Spu=@spu order by [SortValue]
                    ";
                sql = string.Format(sql, str0, str1);
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@spu", spu);
                parameters.Append("@LanguageVersion", language);
                parameters.Append("@userId", userId);



                var ds = DbSFO2ORead.ExecuteSqlDataSet(sql, parameters);

                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据product获取所有相关sku信息
        /// SPU状态 -3、操作完成 -2、新建草稿 -1、编辑草稿 0、待审核 1、待上架 2、已驳回 3、 已上架 4、已下架 5、系统下架
        /// </summary>
        /// <param name="spu"></param> 
        /// <returns></returns>
        public DataSet GetProductSkuAndStatus(string spu, int language)
        {
            if (string.IsNullOrEmpty(spu))
            {
                throw new ArgumentException("spu");
            }
            try
            {

                string sql = @"   SELECT p.Id, p.Spu, p.CategoryId, p.SupplierId, p.Name, p.Tag, p.Price AS ProductPrice, p.[Description],
                            p.Brand, p.CountryOfManufacture, p.SalesTerritory, p.Unit,p.IsExchangeInCHINA,p.IsExchangeInHK,p.IsDutyOnSeller,
                            p.IsReturn, p.MinForOrder, p.MinPrice, p.NetWeightUnit, p.LanguageVersion,
                            s.Sku, s.Price, s.BarCode, s.AlarmStockQty, s.CreateTime, s.AuditTime,
                            s.ShelvesTime, s.RemovedTime, s.ReportStatus, s.IsCrossBorderEBTax, s.PPATaxRate/100 AS PPATaxRate, s.CBEBTaxRate/100 AS CBEBTaxRate,
                            sr.TaxRate/100 TaxRate, s.[Status], s.IsOnSaled,
                            s.MainDicKey, s.MainDicValue, s.SubDicKey,
                            s.SubDicValue, s.MainKey, s.MainValue, s.SubKey,
                            s.SubValue,sk.ForOrderQty as Qty
                            ,p.brandId,sb.NameCN, sb.NameHK, sb.NameEN, sb.Logo, sb.IntroductionCN,sb.CountryName
                            ,s.ConsumerTaxRate/100 AS ConsumerTaxRate,s.VATTaxRate/100 AS VATTaxRate
                            ,fi.id AS FavoriteId
                            ,fi.spu AS FavoriteSpu
                            FROM productInfo p (NOLOCK)
                            LEFT JOIN skuinfo s ON s.SpuId=p.Id  
                            LEFT JOIN stock sk ON s.Sku=sk.Sku 
                            LEFT JOIN SkuCustomsReport sr on s.Sku=sr.Sku
                            LEFT JOIN SupplierBrand AS sb ON sb.Id=p.BrandId
                            LEFT JOIN FavoriteInfo AS fi ON fi.spu=p.Spu
                            WHERE s.IsOnSaled=1 AND p.LanguageVersion=@LanguageVersion AND p.Spu=@spu 
                            
                            SELECT [Id]
                                  ,[SPU]
                                  ,[ImagePath]
                                  ,[ImageType]
                                  ,[SortValue]
                                  ,[Createtime]
                                  ,[Createby]
                              FROM [ProductImage] (NOLOCK)
                            WHERE Spu=@spu order by [SortValue]
                    ";


                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@spu", spu);
                parameters.Append("@LanguageVersion", language);

                var ds = DbSFO2ORead.ExecuteSqlDataSet(sql, parameters);

                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }
        /// <summary>
        /// 获取商品图片信息
        /// </summary>
        /// <param name="spu"></param>
        /// <returns></returns>
        public IList<ProductImage> GetProductImages(string spu)
        {
            if (string.IsNullOrEmpty(spu))
            {
                throw new ArgumentException("spu");
            }
            try
            {

                string sql = @"  SELECT [Id]
                                  ,[SPU]
                                  ,[ImagePath]
                                  ,[ImageType]
                                  ,[SortValue]
                                  ,[Createtime]
                                  ,[Createby]
                              FROM [ProductImage] (NOLOCK)
                            WHERE Spu=@spu   order by [SortValue]";

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@spu", spu);

                return DbSFO2ORead.ExecuteSqlList<ProductImage>(sql, parameters);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }
        public ProductExpandEntity GetProductExpandEntity(string spu, int language = 1)
        {
            if (string.IsNullOrEmpty(spu))
            {
                throw new ArgumentException("spu");
            }
            try
            {

                string sql = @"  SELECT
                                    sk.spu
                                    ,sk.spuId
                                    ,p.CategoryId
                                    ,[Materials]
                                    ,[Pattern]
                                    ,[Flavour]
                                    ,[Ingredients]
                                    ,[StoragePeriod]
                                    ,[StoringTemperature]
                                    ,[SkinType]
                                    ,[Gender]
                                    ,[AgeGroup]
                                    ,[Model] as [Size]
                                    ,[BatteryTime]
                                    ,[Voltage]
                                    ,[Power]
                                    ,[Warranty]
                                    ,[SupportedLanguage]
                                    ,[PetType]
                                    ,[PetAge]
                                    ,[Location]
                                    ,[Weight]
                                    ,[WeightUnit]
                                    ,[Volume]
                                    ,[VolumeUnit]
                                    ,[Length]
                                    ,[LengthUnit]
                                    ,[Width]
                                    ,[WidthUnit]
                                    ,[Height]
                                    ,[HeightUnit]
                                    ,[Brand]=sb.NameCN
                                    ,[Unit]
                                    ,Flavor
                                    ,p.Tag
                                    ,convert(varchar(8),CONVERT(FLOAT,sk.[NetWeight]))+[NetWeightUnit] as NetWeight
                                    ,convert(varchar(8),CONVERT(FLOAT,sk.[NetContent]))+[NetContentUnit] as NetContent
                                    ,sk.[Color]
                                    ,[CountryOfManufacture]
                                    ,'' as Specifications
                                    ,IsReturn =case p.IsReturn WHEN 1 THEN '是' ELSE '否' end 
                                    ,IsExchangeInCHINA=CASE  p.IsExchangeInCHINA WHEN 1 THEN '中国大陆允许' else '中国大陆不允许' end
                                    ,IsExchangeInHK=CASE  p.IsExchangeInHK WHEN 1 THEN '中华人民共和国大陆地区允许' else '中华人民共和国大陆地区不允许' end                                    
                                     FROM dbo.ProductInfoExpand pe
                                    INNER JOIN ProductInfo AS p ON pe.spuId=p.Id 
                                    inner join SkuInfo sk on sk.SpuId=p.Id
                                    LEFT JOIN  SupplierBrand AS sb ON sb.Id=p.BrandId
                                    WHERE p.Spu=@spu AND p.LanguageVersion=@LanguageVersion";


                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@spu", spu);
                parameters.Append("@LanguageVersion", language);
                return DbSFO2ORead.ExecuteSqlFirst<ProductExpandEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spu"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public string GetProductSpecifications(string spu, int language = 1)
        {
            if (string.IsNullOrEmpty(spu))
            {
                throw new ArgumentException("spu");
            }
            try
            {
                string specifications = "";
                string sql = @"  SELECT DISTINCT sk.Specifications FROM SkuInfo AS sk INNER JOIN ProductInfo AS p ON sk.spuId=p.Id 
                                 WHERE len(Specifications)>0 AND p.Spu=@spu AND p.LanguageVersion=@LanguageVersion ";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@spu", spu);
                parameters.Append("@LanguageVersion", language);
                var ds = DbSFO2ORead.ExecuteSqlDataSet(sql, parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Specifications"].ToString()))
                            specifications += ds.Tables[0].Rows[i]["Specifications"].ToString() + ",";
                    }
                }
                return specifications.TrimEnd(',');
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return "";
            }

        }

        //        /// <summary>
        //        /// 获取spu的元数据和sku信息
        //        /// </summary>
        //        /// <param name="spu"></param>
        //        /// <param name="language"></param>
        //        /// <returns></returns>
        //        public ProductAttributeEntity[] GetAttributeMetaDatas(string spu, int language = 1)
        //        {
        //            if (string.IsNullOrEmpty(spu))
        //            {
        //                throw new ArgumentException("spu");
        //            }
        //            const string sql = @" SELECT DISTINCT   
        //                                av.Id 
        //                                ,av.AttrDicId 
        //                                ,av.Spu 
        //                                ,av.Sku
        //                                ,s.[Status] 
        //                                ,s.Price
        //                                ,isnull(sk.Qty,0) AS Qty 
        //                                ,ISNULL(av.AttrKey,d.KeyName) AS AttrKey
        //                                ,ISNULL(av.AttrValue,d.KeyValue) AS AttrValue
        //                                ,ca.IsSkuAttr
        //                                ,ca.IsSkuMainAttr
        //                                ,ca.Id AS MetaDataId
        //                                ,dd.KeyValue AS MetaDataName
        //                                ,dd.KeyName AS MetaDataCode
        //                                FROM ProductInfo AS p
        //                                LEFT JOIN SkuInfo AS s ON s.SpuId=p.Id
        //                                LEFT JOIN Stock sk ON s.Sku=sk.sku
        //                                INNER JOIN AttributeValues AS AV ON p.Spu=av.Spu
        //                                INNER JOIN Dics AS d ON av.AttrDicId=d.Id
        //                                INNER JOIN Category_Attributes ca ON p.CategoryId=ca.CategoryId AND d.DicType=ca.KeyName 
        //                                LEFT JOIN Dics AS dd ON ca.KeyName=dd.KeyName AND dd.LanguageVersion=d.LanguageVersion
        //                                WHERE p.Spu=@spu AND d.LanguageVersion=@LanguageVersion AND s.IsOnSaled=1 AND s.[Status]>3 ";
        //            try
        //            {
        //                var parameters = DbSFO2ORead.CreateParameterCollection();
        //                parameters.Append("@spu", spu);
        //                parameters.Append("@LanguageVersion", language);
        //                return DbSFO2ORead.ExecuteSqlList<ProductAttributeEntity>(sql, parameters).ToArray();
        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.Error(ex);
        //                return new ProductAttributeEntity[] { };
        //            }

        //        }
        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="categoryId"></param> 
        /// <param name="sort"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="deliveryRegion">销售区域</param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<ProductInfoModel> GetProductList(int categoryId, int level, List<ProductFilterAttrubile> mainKey, int sort, int pageindex, int pagesize, int language, int deliveryRegion, ref List<CategoryAttribute> attributes, out int totalRecords)
        {
            string whereStr = " and p.CategoryId=@CategoryId and (p.SalesTerritory=3 or p.SalesTerritory=@SalesTerritory)";
            if (level == 0)
            {
                whereStr = " and p.Categoryid in(select CategoryId from Category where RootId=@CategoryId and categorylevel=2)";
            }
            else if (level == 1)
            {
                whereStr = " and p.Categoryid in(select CategoryId from Category where ParentId=@CategoryId and categorylevel=2)";
            }
            string mainFilter = string.Empty;


            if (mainKey != null)
            {
                foreach (var f in mainKey)
                {
                    string val = string.Empty;
                    if (!string.IsNullOrEmpty(f.KeyValue))
                    {
                        string other = string.Empty;
                        foreach (string s in f.KeyValue.Split(','))
                        {
                            if (s.Contains("其他") || s.Contains("其它"))//其他自定义属性
                            {
                                other = s;
                            }
                            else
                            {
                                val += "'" + s + "',";
                            }
                        }
                        val = val.TrimEnd(',');

                        var otherAttributes = attributes.FindAll(a => a.KeyName == f.KeyName);
                        string otherAttriString = string.Empty;
                        foreach (var o in otherAttributes)
                        {
                            if (!o.SubKeyValue.Contains("其他") && !o.SubKeyValue.Contains("其它"))//其他自定义属性
                            {
                                otherAttriString += "'" + o.SubKeyValue + "',";
                            }
                        }
                        otherAttriString = otherAttriString.TrimEnd(',');

                        if (f.IsSkuAttr == 1)//从SkuInfo表中筛选
                        {
                            if (!string.IsNullOrEmpty(other))
                            {

                                if (!string.IsNullOrEmpty(val))
                                {
                                    mainFilter += string.Format(" and ( k.{0} in({1}) or k.{0} not in({2}) )", f.KeyName, val, otherAttriString);
                                }
                                else
                                {
                                    mainFilter += string.Format(" and k.{0} not in({1}) ", f.KeyName, otherAttriString);
                                }
                            }
                            else
                            {
                                mainFilter += string.Format(" and k.{0} in({1})", f.KeyName, val);
                            }
                        }
                        else
                        {//从ProductInfo表中筛选
                            if (f.KeyName == "Brand")
                            {
                                mainFilter += string.Format(" and p.{0} in({1})", f.KeyName, val);
                            }
                            else if (f.KeyName == "IsReturn")
                            {
                                if (val.Contains("是") && val.Contains("否"))
                                {
                                    mainFilter += string.Format(" and p.{0} in(0,1)", f.KeyName);
                                }
                                else
                                {
                                    mainFilter += string.Format(" and p.{0} in({1})", f.KeyName, val.Replace("'", "") == "是" ? "1" : "0");
                                }
                            }
                            else
                            {//从ProductInfoExpand表中筛选
                                if (!string.IsNullOrEmpty(other))
                                {
                                    if (!string.IsNullOrEmpty(val))
                                    {
                                        mainFilter += string.Format(" and (pd.{0} in({1}) or pd.{0} not in({2})) ", f.KeyName, val, otherAttriString);
                                    }
                                    else
                                    {
                                        mainFilter += string.Format(" and pd.{0} not in({1})", f.KeyName, otherAttriString);
                                    }
                                }
                                else
                                {
                                    mainFilter += string.Format(" and pd.{0} in({1})", f.KeyName, val);
                                }
                            }
                        }
                    }
                }
            }

            string orderby = string.Empty;
            if (sort == 1)
            {
                orderby = "order by sp.shelvesTime desc";
            }
            else if (sort == 2)
            {
                orderby = "order by p.MinPrice asc";
            }
            else if (sort == 3)
            {
                orderby = "order by p.MinPrice desc";
            }
            int startIndex = (pageindex - 1) * pagesize + 1;
            int endIndex = pageindex * pagesize;

            string sql = @"with sputb(spu,shelvesTime,Qty)
                            As
                            (select spu,shelvesTime,ForOrderQty from (select ROW_NUMBER() over(PARTITION by k.SPU order by k.shelvesTime desc) as rindex,k.spu,k.shelvesTime ,s.ForOrderQty
                            from SkuInfo k
                            left join stock s on s.Spu=k.Spu and s.Sku=k.Sku
                            left join ProductInfoExpand pd on pd.SpuId=k.SpuId
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
                            newtb
                            AS
                            (
	                            select  ROW_NUMBER() over(" + orderby + @") as rindex,
		                                p.Id as ProductId,p.Spu,p.Name,p.MinPrice,p.Brand,'" + DomainHelper.ImageUrl + @"'+ replace(replace(ImagePath,'\','/'),'.','_640.') as ImagePath,p.Unit,p.CategoryId ,(skq.QtyCount-p.MinForOrder) as Qty,1 as IsOnSaled,skq.SkuCount
	                            from ProductInfo p
	                            inner join sputb sp on sp.spu=p.Spu
                                left join skuQty skq on skq.Spu=p.Spu
	                            left join ProductImage  i on i.SPU=p.Spu
		                        where i.SortValue=1 and p.LanguageVersion=@LanguageVersion " + whereStr + @"
	                            )
                             select *,(select count(1) from newtb) as TotalRecord 
                             from newtb
                            where rindex between @StartIndex and @EndIndex";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@LanguageVersion", language);
            parameters.Append("@CategoryId", categoryId);
            parameters.Append("@StartIndex", startIndex);
            parameters.Append("@EndIndex", endIndex);
            parameters.Append("@SalesTerritory", deliveryRegion);

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

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="categoryArray"></param> 
        /// <param name="sort"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="deliveryRegion">销售区域</param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<ProductInfoModel> GetProductListNew(string[] categoryArray, int level, List<int> brandIds, int sort, int pageindex, int pagesize, int language, int deliveryRegion, ref List<CategoryAttribute> attributes, out int totalRecords)
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
            else if (sort == 4)
            {
                orderby = "order by discount asc";
            }
            int startIndex = (pageindex - 1) * pagesize + 1;
            int endIndex = pageindex * pagesize;

            // 分类id条件拼接
            StringBuilder CategoryIdwhere = new StringBuilder();
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@LanguageVersion", language);

            // 分类id有多个的情况
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
            // 分类id一个的情况
            else
            {
                parameters.Append("@CategoryId", categoryArray[0]);
                CategoryIdwhere.Append("p.CategoryId = ").Append("@CategoryId");
            }

            parameters.Append("@StartIndex", startIndex);
            parameters.Append("@EndIndex", endIndex);
            parameters.Append("@SalesTerritory", deliveryRegion);

            string whereStr = string.Empty;
            if (level == 0)
            {
                whereStr = " and p.Categoryid in(SELECT DISTINCT c.CategoryId FROM Category AS c WHERE c.RootId=@CategoryId) and (p.SalesTerritory=3 or p.SalesTerritory=@SalesTerritory)";
            }
            else if (level == 1)
            {
                whereStr = " and p.Categoryid in(select CategoryId from Category where ParentId=@CategoryId and categorylevel=2) and (p.SalesTerritory=3 or p.SalesTerritory=@SalesTerritory)";
            }
            else if (level == 2)
            {
                whereStr = " and " + CategoryIdwhere + " and (p.SalesTerritory=3 or p.SalesTerritory=@SalesTerritory)";
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
	                            select  isnull(psp.DiscountPrice,p.MinPrice) as  DiscountPrice,ISNULL(psp.DiscountRate,10) AS discount,psp.DiscountRate,
		                             p.Id as ProductId,p.Spu,p.Name,case when psp.DiscountPrice is null then p.MinPrice else 
									 case when psp.DiscountPrice>p.MinPrice then p.MinPrice else sk.Price end 
									 end as MinPrice,
                                    p.Brand,'" + DomainHelper.ImageUrl + @"'+ replace(replace(ImagePath,'\','/'),'.','_640.') as ImagePath,
                                    p.Unit,p.CategoryId ,c1.ParentId as ParentCategoryId,(skq.QtyCount-p.MinForOrder) as Qty,1 as IsOnSaled,skq.SkuCount,sp.shelvesTime
	                            from ProductInfo p
                                --inner join supplierbrand brand on brand.id=p.brandid and brand.[Status]=1
	                            inner join sputb sp on sp.spu=p.Spu
                                left join skuQty skq on skq.Spu=p.Spu
	                            left join ProductImage  i on i.SPU=p.Spu
                                left join promotionSpu psp on psp.Spu=p.Spu 
                                left join SkuInfo sk on sk.SpuId=p.Id and sk.Sku=psp.Sku 
                                LEFT JOIN Category AS c1 ON c1.CategoryId=p.CategoryId
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


        /// <summary>
        /// 获取商家商品列表
        /// </summary>
        /// <param name="supplierId"></param> 
        /// <param name="sort"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<ProductInfoModel> GetSupplierProductListById(int supplierId, int sort, int pageindex, int pagesize, int language, int salesTerritory, out int totalRecords)
        {
            string whereStr = " and p.SupplierId=@SupplierId and (p.SalesTerritory=3 or p.SalesTerritory=@SalesTerritory) ";

            string orderby = "order by sp.shelvesTime desc";
            if (sort == 1)
            {
                orderby = "order by sp.shelvesTime desc";
            }
            else if (sort == 2)
            {
                orderby = "order by p.MinPrice asc";
            }
            else if (sort == 3)
            {
                orderby = "order by p.MinPrice desc";
            }
            int startIndex = (pageindex - 1) * pagesize + 1;
            int endIndex = pageindex * pagesize;

            string sql = @"with sputb(spu,shelvesTime,Qty,SkuCount)
                            As
                            (
                                SELECT  SkuInfo.Spu,MAX(shelvesTime),SUM(Stock.ForOrderQty),COUNT(SkuInfo.Sku) 
                                FROM SkuInfo INNER JOIN ProductInfo on ProductInfo.Id=SkuInfo.SpuId and ProductInfo.LanguageVersion=@LanguageVersion
                                     LEFT JOIN Stock on SkuInfo.Spu=Stock.Spu AND SkuInfo.Sku=Stock.Sku
                                WHERE SkuInfo.IsOnSaled=1 AND SkuInfo.Status=3
                                GROUP BY SkuInfo.Spu
                             ) ,
                              pImage(SPU,ImagePath)
                            As
                            (
                             select SPU,ImagePath from (select ROW_NUMBER() over(PARTITION by SPU order by sortValue asc) as rindex,SPU,'" + DomainHelper.ImageUrl + @"'+replace(replace(ImagePath,'\','/'),'.','_640.') as ImagePath
                             From ProductImage) a
                             where rindex<2
                            ), 
                            newtb
                            AS
                            (
	                            select  ROW_NUMBER() over(" + orderby + @") as rindex,
		                                p.Id as ProductId,p.Spu,p.Name,p.MinPrice,p.Brand,i.ImagePath,p.Unit,p.CategoryId ,(sp.Qty-p.MinForOrder) as Qty,1 as IsOnSaled,sp.SkuCount
	                            from ProductInfo p
	                            inner join sputb sp on sp.spu=p.Spu
	                            left join pImage i on i.SPU=p.Spu
		                        where p.LanguageVersion=@LanguageVersion " + whereStr + @"
	                            )
                             select *,(select count(1) from newtb) as TotalRecord 
                             from newtb
                            where rindex between @StartIndex and @EndIndex";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@LanguageVersion", language);
            parameters.Append("@SalesTerritory", salesTerritory);
            parameters.Append("@SupplierId", supplierId);
            parameters.Append("@StartIndex", startIndex);
            parameters.Append("@EndIndex", endIndex);

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


        /// <summary>
        /// 获取扩展字段表信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public IList<DicsEntity> GetExpandFields(int categoryId, int language = 1)
        {
            try
            {
                string sql = @"     SELECT * FROM Dics(NOLOCK) AS d WHERE d.DicType='ProductAttributes' 
                             AND  d.LanguageVersion=@LanguageVersion AND d.KeyName IN( 
                            SELECT KeyName FROM Category_Attributes(NOLOCK) AS ca WHERE ca.CategoryId=@CategoryId AND isshow=1 AND isskuattr=0 )";

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@CategoryId", categoryId);
                parameters.Append("@LanguageVersion", language);
                return DbSFO2ORead.ExecuteSqlList<DicsEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }
        /// <summary>
        /// 获取描述字段信息
        /// </summary>
        /// <param name="spu"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public string GetDescription(string spu, int language)
        {

            try
            {
                string sql = @" SELECT [Description] FROM productInfo WHERE spu=@spu AND LanguageVersion=@LanguageVersion";

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@spu", spu);
                parameters.Append("@LanguageVersion", language);
                return DbSFO2ORead.ExecuteSqlScalar<string>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取商品评价信息
        /// </summary>
        /// <param name="spuId"></param>
        /// <param name="language"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public IList<CommentEntity> GetCommentEntities(string spuId, int language, out int recordCount, int pageIndex = 1, int pageSize = 20)
        {
            recordCount = 0;
            try
            {
                string sql = @";WITH allData AS (
 SELECT    ROW_NUMBER() OVER ( ORDER BY c.CreateTime DESC ) AS rownum ,
                    [CommentId] ,
                    c.[UserId] ,
                    [Title] ,
                    [Comment] ,
                    [Rating] ,
                    c.[CreateTime] ,
                    c.[OrderId] ,
                    s.UserName
                    --od.[Unit] ,
                    --od.[ProductName] ,                    
                    --dbo.GetOrderProductCount(c.OrderId,c.commentobjectid) 'Quantity',
                   -- dbo.getordersize(c.OrderId,c.commentobjectid) 'Size',
                    --od.ProductId,
                    --od.Price,
                    --p.ProductImagePath,
                    --SS.StoreLogoPath,
                    --ss.StoreName,
                   -- s.SupplierType,
                    --ss.StoreDomainName,
					--s.CheckStatus
					,si.MainDicValue, si.SubDicValue, si.MainValue, si.SubValue
           FROM      [Comment] c ( NOLOCK )
                    INNER JOIN Customer s ( NOLOCK ) ON c.UserId = s.ID
                    INNER JOIN SkuInfo si(NOLOCK) ON c.CommentObjectId=si.SpuId
                    --INNER JOIN [OrderDetail] od ( NOLOCK ) ON c.CommentObjectId = od.ProductId
                    --                                          AND od.DetailId = c.DetailId
                    --                                          AND c.CommentType = 2
                    INNER JOIN ProductInfo AS  p ON c.CommentObjectId = p.Id 
          WHERE     C.[Status] = 1
                    --AND S.[Status] = 1
                   AND p.spu=@spu AND p.LanguageVersion=@LanguageVersion ) 
                   select allData.*, (select max(rownum) from allData) as rowsCount from allData
                                                                    where rownum > ( @PageSize * ( @PageIndex - 1 ) )
                                                                    and rownum <= ( @PageSize * @PageIndex )  order by allData.CreateTime desc
 ";

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@spu", spuId);

                parameters.Append("@PageIndex", pageIndex);
                parameters.Append("@PageSize", pageSize);

                parameters.Append("@LanguageVersion", language);
                var list = DbSFO2ORead.ExecuteSqlList<CommentEntity>(sql, parameters);
                if (list.Count > 0)
                {
                    recordCount = list[0].RowsCount.As(0);
                }
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取商家商品列表
        /// </summary> 
        /// <param name="brandId"></param>
        /// <param name="top"></param>
        /// <param name="language"></param>
        /// <param name="deliveryRegion"></param>
        /// <param name="spu"></param>
        /// <returns></returns>
        public IList<ProductInfoModel> GetProductListBySupplier(int brandId, int top, int language, int deliveryRegion, string spu)
        {
            try
            {
                //                DECLARE @SupplierId NVARCHAR(50)
                //DECLARE @Language int
                //DECLARE @Top int
                //SET @SupplierId='2'
                //SET @Language=1
                //SET @Top=10

                //                string sql = @";WITH tbAll AS (
                //                            SELECT TOP(@Top) s.SpuId,s.Spu,MAX(s.ShelvesTime) AS ShelvesTime,sum(st.ForOrderQty) AS Qty,COUNT(DISTINCT s.Sku) SkuCount FROM SkuInfo s
                //                            INNER JOIN Stock AS st ON s.Sku=st.Sku 
                //                            INNER JOIN ProductInfo AS p ON s.SpuId=p.Id and (SalesTerritory=@deliveryRegion or SalesTerritory=3)
                //                            WHERE s.IsOnSaled=1 AND s.[Status]=3 AND p.LanguageVersion=@Language AND p.SupplierId=@SupplierId
                //                            GROUP BY s.SpuId,s.Spu
                //                            )
                //                           SELECT p.id AS ProductId,p.Spu,p.Name,p.MinPrice,p.Brand,p.Unit,p.CategoryId,*,ta.ShelvesTime,ta.Qty,ta.SkuCount
                //                            ,pim.ImagePath,MinForOrder FROM ProductInfo AS p
                //                            LEFT JOIN ProductImage pim  ON pim.SPU=p.Spu  AND pim.SortValue=1
                //                            INNER JOIN tbAll AS ta ON p.Id=ta.spuid  ORDER BY ta.ShelvesTime DESC  ";
                string sql = @";WITH tbAll AS (
                                SELECT TOP(@Top) s.SpuId,s.Spu,MAX(s.ShelvesTime) AS ShelvesTime,sum(st.ForOrderQty) AS Qty,COUNT(DISTINCT s.Sku) SkuCount FROM SkuInfo s
                                INNER JOIN Stock AS st ON s.Sku=st.Sku 
                                INNER JOIN ProductInfo AS p ON s.SpuId=p.Id and (SalesTerritory=@deliveryRegion or SalesTerritory=3)
                                WHERE s.IsOnSaled=1 AND s.[Status]=3 
                                AND p.LanguageVersion=@Language AND p.BrandId=@BrandId AND p.Spu<>@Spu
                                GROUP BY s.SpuId,s.Spu
                                )
                                SELECT p.id AS ProductId,p.Spu,p.Name,p.MinPrice,p.Brand,p.Unit,p.CategoryId,*,ta.ShelvesTime,ta.Qty,ta.SkuCount
                                ,pim.ImagePath,MinForOrder FROM ProductInfo AS p
                                LEFT JOIN ProductImage pim  ON pim.SPU=p.Spu  AND pim.SortValue=1
                                INNER JOIN tbAll AS ta ON p.Id=ta.spuid  

                                ORDER BY ta.ShelvesTime DESC ";

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@Language", language);
                parameters.Append("@Top", top);
                parameters.Append("@BrandId", brandId);
                parameters.Append("@Spu", spu);
                parameters.Append("@deliveryRegion", deliveryRegion);

                return DbSFO2ORead.ExecuteSqlList<ProductInfoModel>(sql, parameters);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<ProductInfoModel>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public ProductSkuEntity GetProductBySku(string sku, int language)
        {
            if (string.IsNullOrEmpty(sku))
            {
                throw new ArgumentException("sku");
            }
            try
            {

                string sql = @"   SELECT p.Id, p.Spu, p.CategoryId, p.SupplierId, p.Name, p.Tag, s.Price AS ProductPrice, p.[Description],
                            p.Brand, p.CountryOfManufacture, p.SalesTerritory, p.Unit,p.IsExchangeInCHINA,p.IsExchangeInHK,p.IsDutyOnSeller,
                            p.IsReturn, p.MinForOrder, p.MinPrice, p.LanguageVersion,
                            s.Sku, s.Price, s.BarCode, s.AlarmStockQty, s.CreateTime, s.AuditTime,
                            s.ShelvesTime, s.RemovedTime, s.[Status], s.IsOnSaled,s.IsCrossBorderEBTax, s.ReportStatus,
                            s.MainDicKey, s.MainDicValue, s.SubDicKey,
                            s.SubDicValue, s.MainKey, s.MainValue, s.SubKey,
                            s.SubValue,sk.ForOrderQty as Qty,pim.ImagePath AS ImagePath
                            ,s.ConsumerTaxRate,
                            s.VATTaxRate,
                            s.CBEBTaxRate
                            ,s.PPATaxRate
                            ,p.CommissionInCHINA,p.CommissionInHK,p.NetWeightUnit,p.NetContentUnit,ISNULL(ps.DiscountPrice,0) AS DiscountPrice
                            FROM productInfo p (NOLOCK)
                            LEFT JOIN skuinfo s ON s.SpuId=p.Id  
                            LEFT JOIN stock sk ON s.Sku=sk.Sku
                            LEFT JOIN ProductImage AS pim ON pim.spu=p.spu AND pim.sortValue=1
                            LEFT JOIN PromotionSku AS ps ON ps.sku = s.sku AND ps.sku = @sku AND ps.PromotionId IN(
								SELECT p.Id FROM Promotions AS p WHERE p.PromotionStatus=2
                            )
                            WHERE s.IsOnSaled=1 AND s.[status]=3  AND p.LanguageVersion=@LanguageVersion   AND s.sku=@sku     ";


                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@sku", sku);
                parameters.Append("@LanguageVersion", language);

                return DbSFO2ORead.ExecuteSqlFirst<ProductSkuEntity>(sql, parameters);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public ProductOrderSkuEntity GetProductOrderBySku(string sku, int language)
        {
            if (string.IsNullOrEmpty(sku))
            {
                throw new ArgumentException("sku");
            }
            try
            {

                string sql = @"   SELECT p.Id, p.Spu, p.CategoryId, p.SupplierId, p.Name, p.Tag, s.Price AS ProductPrice, p.[Description],
                            p.Brand, p.CountryOfManufacture, p.SalesTerritory, p.Unit,p.IsExchangeInCHINA,p.IsExchangeInHK,p.IsDutyOnSeller,
                            p.IsReturn, p.MinForOrder, p.MinPrice, p.LanguageVersion,p.CommissionInCHINA,p.CommissionInHK,
                            s.Sku, s.Price, s.BarCode, s.AlarmStockQty, s.CreateTime, s.AuditTime,
                            s.ShelvesTime, s.RemovedTime, sr.TaxRate/100 as TaxRate, s.[Status], s.IsOnSaled,
                            s.MainDicKey, s.MainDicValue, s.SubDicKey,
                            s.SubDicValue, s.MainKey, s.MainValue, s.SubKey,
                            s.SubValue,sk.ForOrderQty as Qty,pim.ImagePath AS ImagePath
                            FROM productInfo p (NOLOCK)
                            LEFT JOIN skuinfo s ON s.SpuId=p.Id  
                            LEFT JOIN stock sk ON s.Sku=sk.Sku 
                            LEFT JOIN SkuCustomsReport sr on s.Sku=sr.Sku
                            LEFT JOIN ProductImage AS pim ON pim.spu=p.spu AND pim.sortValue=1
                            WHERE s.IsOnSaled=1 AND s.[status]=3  AND p.LanguageVersion=@LanguageVersion   AND s.sku=@sku     ";


                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@sku", sku);
                parameters.Append("@LanguageVersion", language);

                return DbSFO2ORead.ExecuteSqlFirst<ProductOrderSkuEntity>(sql, parameters);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }
        /// <summary>
        /// 取spu下的sku
        /// </summary>
        /// <param name="spuList"></param>
        /// <returns></returns>
        public DataTable GetSkuBySpu(List<string> spuList)
        {
            string spu = string.Empty;
            foreach (var s in spuList)
            {
                spu += string.Format("'{0}',", s);
            }
            if (!string.IsNullOrEmpty(spu))
            {
                spu = spu.TrimEnd(',');
            }
            if (string.IsNullOrEmpty(spu))
            {
                return new DataTable();
            }
            string sql = @"select k.Spu,s.sku from SkuInfo k
                            inner join ProductInfo p on p.Id=k.SpuId and p.LanguageVersion=1
		                    left join Stock		s on s.Spu=k.Spu and k.Sku=s.Sku
							where k.spu in(" + spu + ") and k.IsOnSaled=1 and k.[Status]=3";
            return DbSFO2ORead.ExecuteDataSet(CommandType.Text, sql, null).Tables[0];
        }
        /// <summary>
        /// 取spu下的sku
        /// </summary>
        /// <param name="spuList"></param>
        /// <returns></returns>
        public DataSet GetSkuBySpuID(List<Int32> spuIDList)
        {
            string spuID = string.Empty;
            foreach (var s in spuIDList)
            {
                spuID += string.Format("{0},", s.ToString());
            }
            if (!string.IsNullOrEmpty(spuID))
            {
                spuID = spuID.TrimEnd(',');
            }
            string sql = String.Format(@"
SELECT Id,Spu,MinPrice,MinForOrder FROM ProductInfo WHERE Id IN ({0})
select k.SpuId,s.sku,ISNULL(s.ForOrderQty,0) ForOrderQty from SkuInfo k
inner join ProductInfo p on p.Id=k.SpuId
left join Stock s on s.Spu=k.Spu and k.Sku=s.Sku
where k.SpuId in({0}) and k.IsOnSaled=1 and k.[Status]=3", spuID);

            return DbSFO2ORead.ExecuteDataSet(CommandType.Text, sql, null);
        }
        /// <summary>
        /// 取所有商家品牌
        /// </summary>
        /// <returns></returns>
        public DataTable GetBrand()
        {
            string sql = "select distinct  NameCN ,'Brand' as KeyName,'品牌' as KeyValue from SupplierBrand";
            return DbSFO2ORead.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        }

        /// <summary>
        /// 第一次（同步）获取品牌商品列表
        /// </summary>
        /// <param name="supplierId"></param> 
        /// <param name="sort"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<ProductInfoModel> GetBrandProductListById(int brandId, int sort, int pageindex, int pagesize, int language, int salesTerritory, out int totalRecords)
        {
            string whereStr = " and p.BrandId=@BrandId and (p.SalesTerritory=3 or p.SalesTerritory=@SalesTerritory) ";

            string orderby = "order by newtb.shelvesTime desc";
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

            string sql = @"with sputb(spu,shelvesTime,Qty,SkuCount)
                            As
                            (
                                SELECT  SkuInfo.Spu,MAX(shelvesTime),SUM(Stock.ForOrderQty),COUNT(SkuInfo.Sku) 
                                FROM SkuInfo INNER JOIN ProductInfo on ProductInfo.Id=SkuInfo.SpuId and ProductInfo.LanguageVersion=@LanguageVersion
                                     LEFT JOIN Stock on SkuInfo.Spu=Stock.Spu AND SkuInfo.Sku=Stock.Sku
                                WHERE SkuInfo.IsOnSaled=1 AND SkuInfo.Status=3
                                GROUP BY SkuInfo.Spu
                             ) ,
                              pImage(SPU,ImagePath)
                            As
                            (
                             select SPU,ImagePath from (select ROW_NUMBER() over(PARTITION by SPU order by sortValue asc) as rindex,SPU,'" + DomainHelper.ImageUrl + @"'+replace(replace(ImagePath,'\','/'),'.','_640.') as ImagePath
                             From ProductImage) a
                             where rindex<2
                            ), 
                            promotionSpu
							As(
							   select * from (SELECT b.spu, b.sku,DiscountPrice, DiscountRate, ROW_NUMBER() over(partition by b.spu order by DiscountPrice ASC ) as rownum FROM (
                                SELECT ps.PromotionId, ps.Sku, ps.DiscountRate, ps.DiscountPrice,ps.Spu
                                  FROM PromotionSku AS ps
                                  inner JOIN promotions p ON ps.PromotionId=p.Id  
                                WHERE  p.PromotionStatus=2 AND p.PromotionType=1
                                )b)tt where rownum<2
							),
                            newtb
                            AS
                            (
                                 select  isnull(psp.DiscountPrice,p.MinPrice) as  DiscountPrice,psp.DiscountRate,
		                                p.Id as ProductId,p.Spu,p.Name,case when psp.DiscountPrice is null then p.MinPrice else 
									 case when psp.DiscountPrice>p.MinPrice then p.MinPrice else sk.Price end 
									 end as MinPrice,p.Brand,i.ImagePath,p.Unit,p.CategoryId ,c1.ParentId AS ParentCategoryId,(sp.Qty-p.MinForOrder) as Qty,1 as IsOnSaled,sp.SkuCount,sp.shelvesTime,sk.Huoli as DHuoli
	                            from ProductInfo p
                                --inner join supplierbrand brand on brand.id=p.brandid and brand.[Status]=1
	                            inner join sputb sp on sp.spu=p.Spu
	                            left join pImage i on i.SPU=p.Spu
                                left join promotionSpu psp on psp.Spu=p.Spu
                                left join SkuInfo sk on sk.SpuId=p.Id
                                LEFT JOIN Category AS c1 ON c1.CategoryId=p.CategoryId
		                        where p.LanguageVersion=@LanguageVersion " + whereStr + @"
	                            ),
                            tb
							 As(
								select  ROW_NUMBER() over(" + orderby + @") as rindex,* from newtb
							 )
                             select *,(select count(1) from tb) as TotalRecord 
                             from tb
                            where rindex between @StartIndex and @EndIndex";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@LanguageVersion", language);
            parameters.Append("@SalesTerritory", salesTerritory);
            parameters.Append("@BrandId", brandId);
            parameters.Append("@StartIndex", startIndex);
            parameters.Append("@EndIndex", endIndex);

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
        /// <summary>
        /// （异步）获取品牌商品列表
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="sort"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="language"></param>
        /// <param name="salesTerritory"></param>
        /// <returns></returns>
        public List<ProductInfoModel> GetBrandProductListById(int brandId, int sort, int pageindex, int pagesize, int language, int salesTerritory)
        {
            string whereStr = " and p.BrandId=@BrandId and (p.SalesTerritory=3 or p.SalesTerritory=@SalesTerritory) ";

            string orderby = "order by newtb.shelvesTime desc";
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

            string sql = @"with sputb(spu,shelvesTime,Qty,SkuCount)
                            As
                            (
                                SELECT  SkuInfo.Spu,MAX(shelvesTime),SUM(Stock.ForOrderQty),COUNT(SkuInfo.Sku) 
                                FROM SkuInfo INNER JOIN ProductInfo on ProductInfo.Id=SkuInfo.SpuId and ProductInfo.LanguageVersion=@LanguageVersion
                                     LEFT JOIN Stock on SkuInfo.Spu=Stock.Spu AND SkuInfo.Sku=Stock.Sku
                                WHERE SkuInfo.IsOnSaled=1 AND SkuInfo.Status=3
                                GROUP BY SkuInfo.Spu
                             ) ,
                              pImage(SPU,ImagePath)
                            As
                            (
                             select SPU,ImagePath from (select ROW_NUMBER() over(PARTITION by SPU order by sortValue asc) as rindex,SPU,'" + DomainHelper.ImageUrl + @"'+replace(replace(ImagePath,'\','/'),'.','_640.') as ImagePath
                             From ProductImage) a
                             where rindex<2
                            ), 
                            promotionSpu
							As(
							   select * from (SELECT b.spu, b.sku,DiscountPrice, DiscountRate, ROW_NUMBER() over(partition by b.spu order by DiscountPrice ASC ) as rownum FROM (
                                SELECT ps.PromotionId, ps.Sku, ps.DiscountRate, ps.DiscountPrice,ps.Spu
                                  FROM PromotionSku AS ps
                                  inner JOIN promotions p ON ps.PromotionId=p.Id  
                                WHERE  p.PromotionStatus=2 AND p.PromotionType=1
                                )b)tt where rownum<2
							),
                            newtb
                            AS
                            (
                                 select  isnull(psp.DiscountPrice,p.MinPrice) as  DiscountPrice,psp.DiscountRate,
		                                p.Id as ProductId,p.Spu,p.Name,case when psp.DiscountPrice is null then p.MinPrice else 
									 case when psp.DiscountPrice>p.MinPrice then p.MinPrice else sk.Price end 
									 end as MinPrice,p.Brand,i.ImagePath,p.Unit,p.CategoryId ,c1.ParentId AS ParentCategoryId,(sp.Qty-p.MinForOrder) as Qty,1 as IsOnSaled,sp.SkuCount,sp.shelvesTime,sk.Huoli as DHuoli,sk.SKU 
	                            from ProductInfo p
                                --inner join supplierbrand brand on brand.id=p.brandid and brand.[Status]=1
	                            inner join sputb sp on sp.spu=p.Spu
	                            left join pImage i on i.SPU=p.Spu
                                left join promotionSpu psp on psp.Spu=p.Spu
                                left join SkuInfo sk on sk.SpuId=p.Id
                                LEFT JOIN Category AS c1 ON c1.CategoryId=p.CategoryId
		                        where p.LanguageVersion=@LanguageVersion " + whereStr + @"
	                            ),
                            tb
							 As(
								select  ROW_NUMBER() over(" + orderby + @") as rindex,* from newtb
							 )
                             select *,(select count(1) from tb) as TotalRecord 
                             from tb
                            where rindex between @StartIndex and @EndIndex";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@LanguageVersion", language);
            parameters.Append("@SalesTerritory", salesTerritory);
            parameters.Append("@BrandId", brandId);
            parameters.Append("@StartIndex", startIndex);
            parameters.Append("@EndIndex", endIndex);

            var list = DbSFO2ORead.ExecuteSqlList<ProductInfoModel>(sql, parameters);
            if (!list.Any())
            {
                list = new List<ProductInfoModel>();
            }
            return list.ToList();
        }

        public List<ProductInfoModel> GetPList( int sort, int pageindex, int pagesize, int language, int salesTerritory)
        {
            string whereStr = " and (p.SalesTerritory=3 or p.SalesTerritory=@SalesTerritory) ";

            string orderby = "order by newtb.shelvesTime desc";
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

            string sql = @"with sputb(spu,shelvesTime,Qty,SkuCount)
                            As
                            (
                                SELECT  SkuInfo.Spu,MAX(shelvesTime),SUM(Stock.ForOrderQty),COUNT(SkuInfo.Sku) 
                                FROM SkuInfo INNER JOIN ProductInfo on ProductInfo.Id=SkuInfo.SpuId and ProductInfo.LanguageVersion=@LanguageVersion
                                     LEFT JOIN Stock on SkuInfo.Spu=Stock.Spu AND SkuInfo.Sku=Stock.Sku
                                WHERE SkuInfo.IsOnSaled=1 AND SkuInfo.Status=3
                                GROUP BY SkuInfo.Spu
                             ) ,
                              pImage(SPU,ImagePath)
                            As
                            (
                             select SPU,ImagePath from (select ROW_NUMBER() over(PARTITION by SPU order by sortValue asc) as rindex,SPU,'" + DomainHelper.ImageUrl + @"'+replace(replace(ImagePath,'\','/'),'.','_640.') as ImagePath
                             From ProductImage) a
                             where rindex<2
                            ), 
                            promotionSpu
							As(
							   select * from (SELECT b.spu, b.sku,DiscountPrice, DiscountRate, ROW_NUMBER() over(partition by b.spu order by DiscountPrice ASC ) as rownum FROM (
                                SELECT ps.PromotionId, ps.Sku, ps.DiscountRate, ps.DiscountPrice,ps.Spu
                                  FROM PromotionSku AS ps
                                  inner JOIN promotions p ON ps.PromotionId=p.Id  
                                WHERE  p.PromotionStatus=2 AND p.PromotionType=1
                                )b)tt where rownum<2
							),
                            newtb
                            AS
                            (
                                 select  isnull(psp.DiscountPrice,p.MinPrice) as  DiscountPrice,psp.DiscountRate,
		                                p.Id as ProductId,p.Spu,p.Name,case when psp.DiscountPrice is null then p.MinPrice else 
									 case when psp.DiscountPrice>p.MinPrice then p.MinPrice else sk.Price end 
									 end as MinPrice,p.Brand,i.ImagePath,p.Unit,p.CategoryId ,c1.ParentId AS ParentCategoryId,(sp.Qty-p.MinForOrder) as Qty,1 as IsOnSaled,sp.SkuCount,sp.shelvesTime,sk.Huoli as DHuoli,sk.Sku
	                            from ProductInfo p
                                --inner join supplierbrand brand on brand.id=p.brandid and brand.[Status]=1
	                            inner join sputb sp on sp.spu=p.Spu
	                            left join pImage i on i.SPU=p.Spu
                                left join promotionSpu psp on psp.Spu=p.Spu
                                left join SkuInfo sk on sk.SpuId=p.Id
                                LEFT JOIN Category AS c1 ON c1.CategoryId=p.CategoryId
		                        where p.LanguageVersion=@LanguageVersion " + whereStr + @"
	                            ),
                            tb
							 As(
								select  ROW_NUMBER() over(" + orderby + @") as rindex,* from newtb
							 )
                             select *,(select count(1) from tb) as TotalRecord 
                             from tb
                            where rindex between @StartIndex and @EndIndex";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@LanguageVersion", language);
            parameters.Append("@SalesTerritory", salesTerritory);
            parameters.Append("@StartIndex", startIndex);
            parameters.Append("@EndIndex", endIndex);

            var list = DbSFO2ORead.ExecuteSqlList<ProductInfoModel>(sql, parameters);
            if (!list.Any())
            {
                list = new List<ProductInfoModel>();
            }
            return list.ToList();
        }
        /// <summary>
        /// 获取拼图列表
        /// </summary>
        public List<ProductInfoModel> GetProductFightList(int language, int pageindex, int pagesize, decimal ExchangeRate, string startTime, string endTime, string flagSpu)
        {
            string flag = string.IsNullOrEmpty(flagSpu) ? "and 1=1" : "and ps.Spu=" + flagSpu;
            int startIndex = (pageindex - 1) * pagesize + 1;
            int endIndex = pageindex * pagesize;
            string sql = @"with sputb
                            AS
                            (
                            SELECT *,ROW_NUMBER() over(order by StartTime desc)as rindex FROM 
							(
                            SELECT TOP 100 PERCENT * FROM (SELECT TOP 100 PERCENT p.Id as pid,si.Sku,sk.Qty,ISNULL(sk.ForOrderQty,0)AS ForOrderQty,pi1.BrandId,pi1.Name,replace(sb.Logo,'\','/') AS Logo,p.PromotionStatus,p.PromotionType,pi1.Spu as SPU,p.StartTime,p.EndTime,p.CreateTime,si.Price * @ExchangeRate  as MinPrice,ISNULL(si.SubDicValue,'') as SubDicValue,ISNULL(si.SubValue,'') as SubValue,ISNULL(si.MainDicValue,'') as MainDicValue,ISNULL(si.MainValue,'') as MainValue, replace(replace(pi2.ImagePath,'\','/'),'.','_180.') as ImagePath
                            ,ISNULL(pi1.NetWeight,0.00) AS NetWeight,ISNULL(pi1.NetWeightUnit,'') AS NetWeightUnit,p.TuanNumbers,ps.DiscountPrice * @ExchangeRate as DiscountPrice,ISNULL(sb.NameCN,'')  AS Brand,ISNULL(sb.NameEN,'') AS BrandEN,ISNULL(pi1.CountryOfManufacture,'') as CountryOfManufacture
                            FROM dbo.Promotions AS p 
                            LEFT JOIN PromotionSku AS ps ON ps.PromotionId=p.Id 
                            LEFT JOIN SkuInfo AS si ON PS.Sku=Si.Sku
                            LEFT JOIN ProductInfo AS pi1 ON pi1.id = si.SpuId
                            LEFT JOIN ProductImage AS pi2 ON pi2.Spu = pi1.Spu
                            LEFT JOIN SupplierBrand AS sb ON sb.Id=pi1.BrandId
                            LEFT JOIN Stock AS sk ON sk.Sku=Si.Sku
                            WHERE pi1.LanguageVersion=1  AND si.[Status]=3 AND p.PromotionStatus=2 AND p.PromotionType=2 AND pi2.SortValue=1 AND ISNULL(sk.ForOrderQty,0)!=0 AND p.StartTime>=@startTime AND p.EndTime <=@endTime " + flag + @"
                            AND pi1.MinForOrder < (SELECT isnull(SUM(s.ForOrderQty),0) FROM Stock AS s WHERE s.Spu=sk.Spu)
                            ORDER BY 
                            p.StartTime DESC,p.CreateTime) AS ads
                            ORDER BY StartTime DESC,CreateTime
                            UNION
                            SELECT * FROM (
                            SELECT TOP 100 PERCENT p.Id as pid,si.Sku,sk.Qty,0 AS ForOrderQty,pi1.BrandId,pi1.Name,replace(sb.Logo,'\','/') AS Logo,p.PromotionStatus,p.PromotionType,pi1.Spu as SPU,'1990-01-01' as StartTime,p.EndTime,p.createTime,si.Price * @ExchangeRate  as MinPrice,ISNULL(si.SubDicValue,'') as SubDicValue,ISNULL(si.SubValue,'') as SubValue,ISNULL(si.MainDicValue,'') as MainDicValue,ISNULL(si.MainValue,'') as MainValue, replace(replace(pi2.ImagePath,'\','/'),'.','_180.') as ImagePath
                            ,ISNULL(pi1.NetWeight,0.00) AS NetWeight,ISNULL(pi1.NetWeightUnit,'') AS NetWeightUnit,p.TuanNumbers,ps.DiscountPrice * @ExchangeRate  as DiscountPrice,ISNULL(sb.NameCN,'')  AS Brand,ISNULL(sb.NameEN,'') AS BrandEN,ISNULL(pi1.CountryOfManufacture,'') as CountryOfManufacture
                            FROM dbo.Promotions AS p 
                            LEFT JOIN PromotionSku AS ps ON ps.PromotionId=p.Id 
                            LEFT JOIN SkuInfo AS si ON PS.Sku=Si.Sku
                            LEFT JOIN ProductInfo AS pi1 ON pi1.id = si.SpuId
                            LEFT JOIN ProductImage AS pi2 ON pi2.Spu = pi1.Spu
                            LEFT JOIN SupplierBrand AS sb ON sb.Id=pi1.BrandId
                            LEFT JOIN Stock AS sk ON sk.Sku=Si.Sku
                            WHERE pi1.LanguageVersion=1 AND si.[Status]=3 AND p.PromotionStatus=2 AND p.PromotionType=2 AND pi2.SortValue=1 AND (pi1.MinForOrder >= (SELECT isnull(SUM(s.ForOrderQty),0) FROM Stock AS s WHERE s.Spu=sk.Spu)) AND p.StartTime>=@startTime AND p.EndTime <=@endTime " + flag + @"
                            ORDER BY 
                            p.StartTime DESC,p.CreateTime) 
                            AS sbd
							) AS aaa
                            )
                            select *,(select count(1) from sputb) as TotalRecord 
                            from sputb
                            where rindex between @StartIndex and @EndIndex ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@StartIndex", startIndex);
            parameters.Append("@EndIndex", endIndex);
            parameters.Append("@language", language);
            parameters.Append("@ExchangeRate", ExchangeRate);
            parameters.Append("@startTime", startTime);
            parameters.Append("@endTime", endTime);
            var list = DbSFO2ORead.ExecuteSqlList<ProductInfoModel>(sql, parameters);
            return list.ToList();
        }
        /// <summary>
        /// 拼生活商品详情页
        /// </summary>
        public IList<ProductInfoModel> GetProductFightDetail(string sku, decimal ExchangeRate, string startTime, string endTime, int pid)
        {
            string sql = @"SELECT pi1.IsDutyOnSeller,p.Id as pid,si.Sku,si.IsCrossBorderEBTax,si.ReportStatus
                                  ,si.CBEBTaxRate/100 as CBEBTaxRate,si.ConsumerTaxRate/100 as ConsumerTaxRate
                                  ,si.PPATaxRate/100 as PPATaxRate,si.VATTaxRate/100 as VATTaxRate
                                  ,sk.Qty,sk.ForOrderQty,pi1.BrandId,pi1.Name,sb.Logo,p.PromotionStatus
                                  ,p.PromotionType,pi1.Spu as SPU,p.CreateTime,si.Price * @ExchangeRate as MinPrice
                                  ,ISNULL(si.SubDicValue,'') as SubDicValue,ISNULL(si.SubValue,'') as SubValue
                                  ,ISNULL(si.MainDicValue,'') as MainDicValue,ISNULL(si.MainValue,'') as MainValue
                                  ,replace(replace(pi2.ImagePath,'\','/'),'.','_640.') as ImagePath
                                  ,ROW_NUMBER() over(order by p.Id)as rindex
                                  ,ISNULL(pi1.NetWeight,0.00) AS NetWeight
                                  ,ISNULL(pi1.NetWeightUnit,'') AS NetWeightUnit
                                  ,p.TuanNumbers
                                  ,ps.DiscountPrice * @ExchangeRate as DiscountPrice
                                  ,sb.NameCN AS Brand
                                  ,sb.NameEN AS BrandEN
                                  ,pi1.CountryOfManufacture
                                  ,d.KeyValue AS CountryName
                                  ,d.NationalCode AS NationalFlag
                                  ,CASE 
                                  WHEN (SELECT isnull(SUM(s.ForOrderQty),0) FROM Stock AS s WHERE s.Spu=si.Spu)>
									pi1.MinForOrder THEN 1
                                   ELSE 2 END  as compare
                            FROM dbo.Promotions AS p
                            LEFT JOIN PromotionSku AS ps ON ps.PromotionId=p.Id 
                            LEFT JOIN SkuInfo AS si ON PS.Sku=Si.Sku
                            LEFT JOIN ProductInfo AS pi1 ON pi1.id = si.SpuId
                            LEFT JOIN ProductImage AS pi2 ON pi2.Spu = pi1.Spu
                            LEFT JOIN SupplierBrand AS sb ON sb.Id=pi1.BrandId
                            LEFT JOIN Dics AS d ON d.KeyName=sb.CountryId AND d.DicType='CountryOfManufacture' 
									                AND d.LanguageVersion=1
                            LEFT JOIN Stock AS sk ON sk.Sku=Si.Sku
                            WHERE pi1.LanguageVersion=1  AND si.[Status]  =3 
                                    AND ps.Sku=@sku AND p.PromotionType=2 
                                    AND p.PromotionStatus in (2,3) 
                                    AND p.StartTime>=@startTime 
                                    AND p.EndTime <=@endTime and  p.Id=@pid";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@sku", sku);
            parameters.Append("@ExchangeRate", ExchangeRate);
            parameters.Append("@startTime", startTime);
            parameters.Append("@endTime", endTime);
            parameters.Append("@pid", pid);
            return DbSFO2ORead.ExecuteSqlList<ProductInfoModel>(sql, parameters);
        }

        public IList<ProductInfoModel> GetProductFightDetailForShare(string sku, decimal ExchangeRate, string startTime, string endTime, int pid)
        {
            string sql = @"SELECT pi1.IsDutyOnSeller,p.Id as pid,si.Sku,si.IsCrossBorderEBTax,si.ReportStatus
                                  ,si.CBEBTaxRate/100 as CBEBTaxRate,si.ConsumerTaxRate/100 as ConsumerTaxRate
                                  ,si.PPATaxRate/100 as PPATaxRate,si.VATTaxRate/100 as VATTaxRate
                                  ,sk.Qty,sk.ForOrderQty,pi1.BrandId,pi1.Name,sb.Logo,p.PromotionStatus
                                  ,p.PromotionType,pi1.Spu as SPU,p.CreateTime,si.Price * @ExchangeRate as MinPrice
                                  ,ISNULL(si.SubDicValue,'') as SubDicValue,ISNULL(si.SubValue,'') as SubValue
                                  ,ISNULL(si.MainDicValue,'') as MainDicValue,ISNULL(si.MainValue,'') as MainValue
                                  ,pi2.ImagePath
                                  ,ROW_NUMBER() over(order by p.Id)as rindex
                                  ,ISNULL(pi1.NetWeight,0.00) AS NetWeight
                                  ,ISNULL(pi1.NetWeightUnit,'') AS NetWeightUnit
                                  ,p.TuanNumbers
                                  ,ps.DiscountPrice * @ExchangeRate as DiscountPrice
                                  ,sb.NameCN AS Brand
                                  ,sb.NameEN AS BrandEN
                                  ,pi1.CountryOfManufacture
                            FROM dbo.Promotions AS p
                            LEFT JOIN PromotionSku AS ps ON ps.PromotionId=p.Id 
                            LEFT JOIN SkuInfo AS si ON PS.Sku=Si.Sku
                            LEFT JOIN ProductInfo AS pi1 ON pi1.id = si.SpuId
                            LEFT JOIN ProductImage AS pi2 ON pi2.Spu = pi1.Spu
                            LEFT JOIN SupplierBrand AS sb ON sb.Id=pi1.BrandId
                            LEFT JOIN Stock AS sk ON sk.Sku=Si.Sku
                            WHERE pi1.LanguageVersion=1  AND si.[Status]  =3 
                                    AND ps.Sku=@sku AND p.PromotionType=2 
                                    AND p.PromotionStatus in (2,3) 
                                    AND p.StartTime>=@startTime 
                                    AND p.EndTime <=@endTime and  p.Id=@pid";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@sku", sku);
            parameters.Append("@ExchangeRate", ExchangeRate);
            parameters.Append("@startTime", startTime);
            parameters.Append("@endTime", endTime);
            parameters.Append("@pid", pid);
            return DbSFO2ORead.ExecuteSqlList<ProductInfoModel>(sql, parameters);
        }
        public ProductInfoModel getProductInfo(string orderCode, string sku)
        {
            string sql = @"SELECT op.HuoLi as HuoLi,op.Quantity as Qty,op.Coupon FROM OrderProducts AS op WHERE op.OrderCode=@orderCode AND op.Sku=@sku";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@orderCode", orderCode);
            parameters.Append("@sku", sku);
            return DbSFO2ORead.ExecuteSqlFirst<ProductInfoModel>(sql, parameters);
        }
        /// <summary>
        /// 商品收藏列表页
        /// </summary>
        public List<Favorite> GetFavoriteList(int language, int pageindex, int pagesize, decimal ExchangeRate, int userId)
        {
            int startIndex = (pageindex - 1) * pagesize + 1;
            int endIndex = pageindex * pagesize;
            string sql = @"with sputb
                                      AS
                            (SELECT *,ROW_NUMBER() over(order BY ab.fitype DESC,ab.createTime desc)as rindex
                                                         FROM(
                            select TOP 100 PERCENT * from (SELECT TOP 100 PERCENT fi.spu,pi1.BrandId,pi1.Name AS productName,fi.originalPrice,replace(replace(pi2.ImagePath,'\','/'),'.','_180.') as ImagePath,fi.createTime AS createTime,1 as fitype
                                                             FROM FavoriteInfo AS fi
                            LEFT JOIN ProductInfo AS pi1 ON fi.spu=pi1.Spu
                            LEFT JOIN ProductImage AS pi2 ON pi2.Spu=pi1.Spu
                            WHERE pi1.LanguageVersion=@language and fi.userId=@userId  AND pi2.SortValue=1 AND fi.isDelete=0 and EXISTS (
                             SELECT * FROM SkuInfo AS si 
                             WHERE 
                              si.Spu=fi.spu AND si.[Status]=3 AND si.IsOnSaled=1
                            ) ORDER BY fi.createTime DESC) AS a
                            UNION
                            select TOP 100 PERCENT * from (SELECT TOP 100 PERCENT fi.spu,pi1.BrandId,pi1.Name AS productName,fi.originalPrice,replace(replace(pi2.ImagePath,'\','/'),'.','_180.') as ImagePath,fi.createTime AS createTime,0 as fitype
                                                             FROM FavoriteInfo AS fi
                            LEFT JOIN ProductInfo AS pi1 ON fi.spu=pi1.Spu
                            
                            LEFT JOIN ProductImage AS pi2 ON pi2.Spu=pi1.Spu
                            WHERE pi1.LanguageVersion=@language and fi.userId=@userId AND pi2.SortValue=1 AND fi.isDelete=0  and NOT EXISTS (
                             SELECT * FROM SkuInfo AS si 
                             WHERE  si.spu = fi.spu AND si.[Status]=3 AND si.IsOnSaled=1
                            ) ORDER BY fi.createTime DESC) AS b) AS ab) 
                            select *,(select count(1) from sputb) as TotalRecord 
                            from sputb
                            where rindex between @StartIndex and @EndIndex";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@StartIndex", startIndex);
            parameters.Append("@EndIndex", endIndex);
            parameters.Append("@language", language);
            parameters.Append("@ExchangeRate", ExchangeRate);
            parameters.Append("@userId", userId);

            var list = DbSFO2ORead.ExecuteSqlList<Favorite>(sql, parameters);
            return list.ToList();
        }
        public bool isFavorite(string productCode, string collectionStatus, int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE FavoriteInfo SET isDelete = @isDelete WHERE spu=@productCode AND userId=@userId ");
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@productCode", productCode);
            parameters.Append("@userId", userId);
            if (collectionStatus.Equals("true"))
            {
                parameters.Append("@isDelete", 1);
            }
            else
            {
                parameters.Append("@isDelete", 0);
            }


            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }
        public bool selectSpu(string productCode, int userId)
        {
            string sql = @" SELECT COUNT(1) FROM FavoriteInfo AS fi WHERE fi.userId=@userId AND fi.spu=@productCode and isDelete=0";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@userId", userId);
            parameters.Append("@productCode", productCode);
            object returnValue = DbSFO2ORead.ExecuteScalar(CommandType.Text, sql, parameters);
            return returnValue == null ? false : Convert.ToInt32(returnValue) > 0;
        }
        public bool insertInto(string productCode, int userId, decimal price)
        {

            string sql = @"INSERT INTO FavoriteInfo
                            (
	                            -- id -- this column value is auto-generated
	                            spu,
	                            userId,
	                            originalPrice,
	                            isDelete,
	                            createTime
                            )
                            VALUES
                            (
	                            @productCode,
	                            @userId,
	                            @price,
	                            0,
	                            @createTime
                            )";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@userId", userId);
                parameters.Append("@productCode", productCode);
                parameters.Append("@createTime", DateTime.Now);
                parameters.Append("@price", price);
                return DbSFO2OMain.ExecuteSqlNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }
        public IList<ProductInfoModel> GetProductListBySpus(string spus)
        {
            string sql = @"
                            DECLARE @str NVARCHAR(500)
                                    SET @str=@spus;

                            with sputb(spu,shelvesTime,Qty)
                            As
                            (select spu,shelvesTime,ForOrderQty from (select ROW_NUMBER() over(PARTITION by k.SPU order by k.shelvesTime desc) as rindex,k.spu,k.shelvesTime ,s.ForOrderQty
                            from SkuInfo k
                            left join stock s on s.Spu=k.Spu and s.Sku=k.Sku
                             inner join productinfo p on p.Id=k.SpuId and p.LanguageVersion=1
                            where k.IsOnSaled=1 and k.[Status]=3) a where rindex<2
                             ) ,
			                skuQty(Spu,SkuCount,QtyCount)
			                As(
			                select k.Spu,count(s.sku),sum(isnull(s.ForOrderQty,0)) from SkuInfo k
                            inner join ProductInfo p on p.Id=k.SpuId and p.LanguageVersion=1
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
		                        where i.SortValue=1 and p.LanguageVersion=1
	                            ),
                             tb
							 As(
								select  ROW_NUMBER() over(order by newtb.shelvesTime desc) as rindex,* from newtb
							 )
                             select *,(select count(1) from tb) as TotalRecord
                             from tb
                              INNER JOIN   dbo.func_splitidString(@str,',') AS fs
                             ON tb.spu=fs.c1 ";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@spus", spus);
            return DbSFO2ORead.ExecuteSqlList<ProductInfoModel>(sql, parameters);
        }
        #region 根据SPU获取当前所有在售SKU库存
        /// <param name="spu">SPU</param>
        /// <returns>返回是否已售罄，以及当前SPU下所有在售的SKU</returns>
        public DataTable GetStockInfoBySpu(string spu, int language = 1, int salesTerritory = 1)
        {
            try
            {
                string sql = @"select st.Spu,sk.Price,p.MinPrice,st.Sku,st.ForOrderQty,p.MinForOrder from Stock st
                                    inner join ProductInfo p
                                    on st.Spu=p.Spu and p.Spu=@spu and LanguageVersion=@language and (p.SalesTerritory=3 or p.SalesTerritory=@salesTerritory)
                                    left join SkuInfo sk
                                    on st.sku=sk.Sku and p.Id=sk.SpuId and sk.IsOnSaled=1 and sk.[Status]=3
                                    order by sk.Price";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@spu", spu);
                parameters.Append("@language", language);
                parameters.Append("@salesTerritory", salesTerritory);
                return DbSFO2ORead.ExecuteDataSet(CommandType.Text, sql, parameters).Tables[0];
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 根据sku获取时令美食spu信息
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public ProductExpandEntity GetHolidaySpuInfoBySku(string sku, int language = 1)
        {
            if (string.IsNullOrEmpty(sku))
            {
                throw new ArgumentException("sku");
            }
            try
            {
                string sql = @"SELECT pi1.Spu AS spu
				                    ,pi1.CategoryId as CategoryId
				                    ,ISNULL(pie.[Weight],0) AS [Weight]
				                    ,pie.WeightUnit
		                    FROM SkuInfo AS si
		                    INNER JOIN ProductInfo AS pi1 ON pi1.Id = si.SpuId AND pi1.LanguageVersion = @LanguageVersion
		                    LEFT JOIN ProductInfoExpand AS pie ON pie.SpuId = pi1.Id
		                    WHERE si.Sku=@sku ";

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@sku", sku);
                parameters.Append("@LanguageVersion", language);
                return DbSFO2ORead.ExecuteSqlFirst<ProductExpandEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 判断是否是节日食品   为节日食品
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool isHolidayGoods(string spu, int CategoryId)
        {
            string sql = @"  SELECT count(c.ParentId) FROM Category AS c WHERE c.CategoryId in 
                            (SELECT pi1.CategoryId FROM ProductInfo AS pi1 
                             LEFT JOIN SkuInfo AS si ON si.Spu = pi1.Spu AND si.SpuId = pi1.Id
                             WHERE pi1.Spu=@spu AND pi1.LanguageVersion=1 AND si.[Status]=3)
                            AND  c.ParentId=@CategoryId ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@spu", spu);
            parameters.Append("@CategoryId", CategoryId);
            object returnValue = DbSFO2ORead.ExecuteScalar(CommandType.Text, sql, parameters);
            return returnValue == null ? false : Convert.ToInt32(returnValue) > 0;
        }

        /// <summary>
        /// 根据分类ID获取父ID（二级目录ID）
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public string isHolidayGoods(int CategoryId)
        {  
            try
            {
                string sql = @"  SELECT c.ParentId
                              FROM Category AS c WHERE c.CategoryId=@CategoryId ";

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@CategoryId", CategoryId);
                return DbSFO2ORead.ExecuteSqlScalar<string>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        #region 根据Spu获取商品名称
        public string GetProductNameBySpu(string spu, int language=1)
        {
            try
            {
                string sql = @"SELECT Name FROM ProductInfo AS pi1 WITH(NOLOCK)
                                         WHERE pi1.Spu=@spu AND pi1.LanguageVersion=@language";

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@spu", spu);
                parameters.Append("@language", language);

                return DbSFO2ORead.ExecuteSqlScalar<string>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return "";
            }
        }
        #endregion

        #region 根据Spu获取时令商品 单品页重要属性
        public DataTable GetProductAttributes(string spu, int language = 1, int salesTerritory=1)
        {
            try
            {
                string sql = @"SELECT 
                                        pi1.Id AS SpuId,pi1.Spu,
                                        isnull(si.NetWeight,0)AS NetWeight,
                                        isnull(si.Specifications,'')AS Specifications,
                                        isnull(pie.Flavor,'')AS Flavor,
                                        isnull(pie.[Weight],0)AS [Weight]
                                          FROM ProductInfo AS pi1
                                        INNER JOIN SkuInfo AS si ON pi1.Id=si.SpuId AND pi1.Spu=si.Spu AND si.[Status]=3
                                        INNER JOIN ProductInfoExpand AS pie ON pie.SpuId=pi1.Id
                                        WHERE pi1.Spu=@spu AND pi1.LanguageVersion=@language AND pi1.SalesTerritory IN(@salesTerritory,3)";

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@spu", spu);
                parameters.Append("@language", language);
                parameters.Append("@salesTerritory", salesTerritory);

                return DbSFO2ORead.ExecuteDataSet(CommandType.Text,sql, parameters).Tables[0];
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 获取所有待上架的sku
        /// </summary>
        /// <returns></returns>
        public IList<SkuInfo> GetPreShowSku() 
        {
            List<SkuInfo> list = new List<SkuInfo>();
            try
            {
                string sql = @"select sk.Id, sk.Spu, sk.Sku, sk.MainDicKey, sk.MainDicValue, sk.SubDicKey, sk.SubDicValue, sk.MainKey, sk.MainValue, sk.SubKey, sk.SubValue, sk.NetWeight, sk.NetContent, sk.Specifications, sk.Size, sk.Color, sk.AlcoholPercentage, sk.Smell, sk.CapacityRestriction, sk.Price, sk.BarCode, sk.AlarmStockQty, sk.CreateTime, sk.AuditTime, sk.ShelvesTime, sk.RemovedTime, sk.IsOnSaled, sk.Status, sk.ReportStatus,sp.PreOnSaleTime from SkuInfo(nolock) as sk inner join ProductInfo as sp on sk.SpuId = sp.Id where Status=1 and sp.LanguageVersion=1";

                return DbSFO2ORead.ExecuteSqlList<SkuInfo>(sql);
            }
            catch (Exception ex)
            {

                LogHelper.Error(ex);
            }

            return list;
        }
        /// <summary>
        /// 更新待上架sku为已上架
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool UpdatePreShowSku(SkuInfo item)
        {

            try
            {
                string sql = "update SkuInfo set Status=@Status where Id=@Id";
                var parameters = DbSFO2OMain.CreateParameterCollection();
                parameters.Append("@Status", item.Status);
                parameters.Append("@Id", item.Id);

                return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters)>0;

            }
            catch (Exception ex)
            {

                LogHelper.Error("更新上架状态出错",ex);

                return false;
            }

           
           
        }
        /// <summary>
        /// 更新库存
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool InsertStock(SkuInfo item)
        {

          
            string sql = "insert into stock (spu,sku,qty,updatetime,updateby) values (@spu,@sku,@qty,GETDATE(),@updateby);";

            try
            {
                var parameters = DbSFO2OMain.CreateParameterCollection();
                parameters.Append("@spu", item.Spu);
                parameters.Append("@sku", item.Sku);
                parameters.Append("@qty", 10000000);
                parameters.Append("@updateby", "System");

                return DbSFO2OMain.ExecuteNonQuery(CommandType.Text ,sql, parameters) > 0;

            }
            catch (Exception ex)
            {

                LogHelper.Error("更新库存出错", ex);

                return false;
            }



        }
    }
}
