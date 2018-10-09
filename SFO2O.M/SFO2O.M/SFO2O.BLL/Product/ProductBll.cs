using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.DAL.Product;
using SFO2O.Model.Product;
using SFO2O.Utility.Uitl;
using SFO2O.BLL.Common;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Utility.Cache;
using SFO2O.M.ViewModel.Product;
using SFO2O.Model.Common;
using SFO2O.Model.Extensions;
using System.Data;
using SFO2O.DAL.Promotion;
using SFO2O.Model.Category;
using SFO2O.Model.Promotion;
using SFO2O.References.IndexingService;
using SFO2O.Utility.Extensions;
using SFO2O.M.ViewModel.Favorite;
using SFO2O.BLL.Item;
using SFO2O.BLL.Holiday;

namespace SFO2O.BLL.Product
{
    public class ProductBll
    {
        ProductDal dal = new ProductDal();
        PromotionDal promotionDal = new PromotionDal();
        ItemBll itemBll = new ItemBll();
        HolidayBll holidayBll = new HolidayBll();
        /// <summary>
        /// 获取单品页信息
        /// </summary>
        /// <param name="spu"></param>
        /// <param name="language"></param>
        /// <param name="images"></param>
        /// <returns></returns>
        public IList<ProductSkuEntity> GetItemByProductCode(string spu, int language, ref IList<ProductImage> images, int userId)
        {
            try
            {
                var ds = dal.GetProductSku(spu, language, userId);

                if (ds != null && ds.Tables.Count > 0)
                {

                    images = ds.Tables[1].ToList<ProductImage>();

                    return ds.Tables[0].ToList<ProductSkuEntity>();

                }
                else
                {
                    images = null;
                    return null;
                }
            }
            catch (Exception ex)
            {
                images = null;
                LogHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 从缓存获取单品页数据
        /// </summary>
        /// <returns></returns>
        public ItemViewSupporter ItemViewSupporterFromAutoCache(string productCode, int userId, int language, string selectedSku = "", decimal exchangeRate = 1)
        {
            try
            {
                var modules = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, ConstClass.RedisKeyItemSPU + productCode, () =>
                {
                    var data = GetFFFFFFFFFFFF(productCode, userId, language, selectedSku, exchangeRate);
                    return data;
                }, 1440);//缓存24小时
                return modules;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                var modules = GetFFFFFFFFFFFF(productCode, userId, language, selectedSku, exchangeRate);
                return modules;
            }
        }
        /// <summary>
        /// 缓存加载单品页数据
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="userId"></param>
        /// <param name="language"></param>
        /// <param name="selectedSku"></param>
        /// <param name="exchangeRate"></param>
        /// <returns></returns>
        //public ProductDto GetProductDto(string productCode,  int userId, int language,string selectedSku = "",decimal exchangeRate=1)
        //{
        //   try
        //    {
        //        IList<ProductImage> images = null;
        //        //获取单品页商品
             
        //        var itemskus = GetItemByProductCode(productCode, language, ref images, userId);
        //        if (itemskus.Count() > 0)
        //        {
        //            string spu = itemskus.FirstOrDefault().Spu;

        //            //获取收藏表的信息
        //            bool isflag = false;
        //            //if (userId != 0)
        //            //{
        //            //    isflag = getFivoriteDetail(spu, userId);
        //            //}

        //            //var promotions = itemBll.GetPromotionEntities(itemskus.Select(n => n.Sku).ToArray());

        //            //判断这个spu是不是在做拼生活活动（用于前端是否显示拼生活图标链接）
        //            var flag = false;
        //            //var promotions = null;
        //            //if (promotions.Where(d => d.PromotionType == 2).Count() > 0)
        //            //{
        //                //flag = true;
        //           // }
        //            //促销商品，过滤掉拼团(PromotionType=2)商品，留下打折商品PromotionType=1
        //            //promotions = promotions.Where(d => d.PromotionType != 2).ToList();
        //            //组装DTO--pageload
        //            var product = itemskus.ToArray().AsDto(1, null, isflag);
        //            //  查询 拼生活里面 是否有这个spu
        //            if (flag)
        //            {
        //                product.isTrue = 1;
        //            }

        //            product.Images = images.ToArray();


        //            //组装skuAttributeViewModel-- for js skuSelected
        //            //var skuAttributeViewModel = itemskus.AsSkuAttributeViewModel(exchangeRate, null, selectedSku, dto: product);
        //            //ViewBag.SkuMetaViewModel = skuAttributeViewModel;


        //            //bool skuFlag = itemskus.GroupBy(d => d.Sku).Count() > 1;
        //            ////当SKU只有1条时候，默认要被选中，要把所有的FLAG重置1
        //            //if (!skuFlag)
        //            //{
        //            //    foreach (var skuAttribute in skuAttributeViewModel.MainAttributes)
        //            //    {
        //            //        skuAttribute.Flag = 1;
        //            //        foreach (var subSkuAttribute in skuAttribute.SubAttributes)
        //            //        {
        //            //            subSkuAttribute.Flag = 1;
        //            //        }
        //            //    }
        //            //    foreach (var subSkuAttribute in skuAttributeViewModel.SubAttributes)
        //            //    {
        //            //        subSkuAttribute.Flag = 1;
        //            //    }
        //            //}
                    
        //            //return PartialView("_Item", product);
        //        }
        //        else
        //        {
        //            //return PartialView("_Item", new ProductDto());
        //             return new ProductDto();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //      LogHelper.WriteInfo(typeof(ProductBll),ex.Message);
        //      return new ProductDto();
        //    }
        //}

        public ItemViewSupporter GetFFFFFFFFFFFF(string productCode,  int userId, int language,string selectedSku = "",decimal exchangeRate=1)
        { 
            IList<ProductImage> images = null;
                //获取单品页商品
             
                var itemskus = GetItemByProductCode(productCode, language, ref images, userId);
                if (itemskus.Count() > 0)
                {
                    string spu = itemskus.FirstOrDefault().Spu;

                    //获取收藏表的信息
                    bool isflag = false;
                    //if (userId != 0)
                    //{
                    //    isflag = getFivoriteDetail(spu, userId);
                    //}

                    //var promotions = itemBll.GetPromotionEntities(itemskus.Select(n => n.Sku).ToArray());

                    //判断这个spu是不是在做拼生活活动（用于前端是否显示拼生活图标链接）
                    var flag = false;
                    //var promotions = null;
                    //if (promotions.Where(d => d.PromotionType == 2).Count() > 0)
                    //{
                    //flag = true;
                    // }
                    //促销商品，过滤掉拼团(PromotionType=2)商品，留下打折商品PromotionType=1
                    //promotions = promotions.Where(d => d.PromotionType != 2).ToList();
                    //组装DTO--pageload
                    var product = itemskus.ToArray().AsDto(1, null, isflag);
                    //  查询 拼生活里面 是否有这个spu
                    if (flag)
                    {
                        product.isTrue = 1;
                    }

                    product.Images = images.ToArray();

                    ItemViewSupporter item = new ItemViewSupporter()
                    {
                        Id = product.Id
                        ,
                        Spu = product.Spu
                        ,
                        CategoryId = product.CategoryId
                        ,
                        SupplierId = product.SupplierId
                        ,
                        Name = product.Name
                        ,
                        Tag = product.Tag
                        ,
                        ProductPrice = product.ProductPrice
                        ,
                        Description = product.Description
                        ,
                        Brand = product.Brand
                        ,
                        CountryOfManufacture = product.CountryOfManufacture
                        ,
                        SalesTerritory = product.SalesTerritory
                        ,
                        Unit = product.Unit
                        ,
                        IsExchangeCN = product.IsExchangeCN
                        ,
                        IsExchangeHK = product.IsExchangeHK
                        ,
                        IsDutyOnSeller = product.IsDutyOnSeller
                        ,
                        IsReturn = product.IsReturn
                        ,
                        MinForOrder = product.MinForOrder
                        ,
                        MinPrice = product.MinPrice
                        ,
                        MinPriceOriginal = product.MinPriceOriginal
                        ,
                        LanguageVersion = product.LanguageVersion
                        ,
                        Images = product.Images
                        ,
                        SkuDtos = product.SkuDtos
                        ,
                        SkuForOrder = product.SkuForOrder
                        ,
                        PromotionDiscountPrice = product.PromotionDiscountPrice
                        ,
                        Promotion = product.Promotion
                        ,
                        BrandId = product.BrandId
                        ,
                        NameCN = product.NameCN
                        ,
                        NameHK = product.NameHK
                        ,
                        NameEN = product.NameEN
                        ,
                        Logo = product.Logo
                        ,
                        CountryName = product.CountryName
                        ,
                        IntroductionCN = product.IntroductionCN
                        ,
                        isTrue = product.isTrue
                        ,
                        PromotionId = product.PromotionId
                        ,
                        PromotionType = product.PromotionType
                        ,
                        isflag = product.isflag
                        ,
                        NationalFlag = product.NationalFlag
                        ,
                        NetWeightUnit = product.NetWeightUnit
                        ,
                        ProductSkuEntity = itemskus as List<ProductSkuEntity>
                    };
                    return item;
                    //组装skuAttributeViewModel-- for js skuSelected
                    //var skuAttributeViewModel = itemskus.AsSkuAttributeViewModel(exchangeRate, null, selectedSku, dto: product);
                    //ViewBag.SkuMetaViewModel = skuAttributeViewModel;


                    //bool skuFlag = itemskus.GroupBy(d => d.Sku).Count() > 1;
                    ////当SKU只有1条时候，默认要被选中，要把所有的FLAG重置1
                    //if (!skuFlag)
                    //{
                    //    foreach (var skuAttribute in skuAttributeViewModel.MainAttributes)
                    //    {
                    //        skuAttribute.Flag = 1;
                    //        foreach (var subSkuAttribute in skuAttribute.SubAttributes)
                    //        {
                    //            subSkuAttribute.Flag = 1;
                    //        }
                    //    }
                    //    foreach (var subSkuAttribute in skuAttributeViewModel.SubAttributes)
                    //    {
                    //        subSkuAttribute.Flag = 1;
                    //    }
                }
                else
                {
                    return null;
                }
        }
        /// <summary>
        /// 商品收藏 查询单页的商品信息
        /// </summary>
        /// <param name="spu"></param>
        /// <param name="language"></param>
        /// <param name="images"></param>
        /// <returns></returns>
        public IList<ProductSkuEntity> GetItemByProductCodeAndStatus(string spu, int language, ref IList<ProductImage> images)
        {
            try
            {
                var ds = dal.GetProductSkuAndStatus(spu, language);

                if (ds != null && ds.Tables.Count > 0)
                {

                    images = ds.Tables[1].ToList<ProductImage>();

                    return ds.Tables[0].ToList<ProductSkuEntity>();

                }
                else
                {
                    images = null;
                    return null;
                }
            }
            catch (Exception ex)
            {
                images = null;
                LogHelper.Error(ex);
                return null;
            }
        }
        /// <summary>
        /// 获取商品图片信息-图片详情页
        /// </summary>
        /// <param name="spu"></param>
        /// <returns></returns>
        public IList<ProductImage> GetProductImages(string spu)
        {
            return dal.GetProductImages(spu);
        }
        /// <summary>
        /// 获取商品扩展属性
        /// </summary>
        /// <param name="spu"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public ProductExpandEntity GetProductExpandEntity(string spu, int language = 1)
        {
            ProductExpandEntity entity = new ProductExpandEntity();
            entity = dal.GetProductExpandEntity(spu, language);
            entity.Specifications = dal.GetProductSpecifications(spu, language);

            return entity;
        }

        /// <summary>
        /// 获取SPU属性集合
        /// </summary>
        /// <param name="spu"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public IList<SpuAttributeModel> GetSpuAttributes(string spu, int language = 1)
        {
            var entity = GetProductExpandEntity(spu, language);
            var categoryId = entity.CategoryId;
            var categoryKey = ConstClass.RedisKeyCategroySpuAttributes + "_category_" + categoryId + "_language_" + language;

            var fields = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, categoryKey, () =>
            {

                var exts = dal.GetExpandFields(categoryId, language);
                int diyCount = 3;
                //是否可退货
                exts.Add(new DicsEntity()
                {
                    Id = int.MaxValue - diyCount,
                    KeyName = "IsReturn",
                    KeyValue = "是否可退货",
                    DicType = "ProductAttributes",
                    LanguageVersion = language

                });
                diyCount = diyCount - 1;
                //是否可换货
                exts.Add(new DicsEntity()
                {
                    Id = int.MaxValue - diyCount,
                    KeyName = "IsExchange",
                    KeyValue = "是否可换货",
                    DicType = "ProductAttributes",
                    LanguageVersion = language

                });
                return exts;
            }, ConstClass.RedisDefaultCacheMinutes);

            if (fields != null)
            {
                var param = entity.AsSpuAttributeModels(fields);
                param.Add(new SpuAttributeModel() { AttributeName = "品牌", AttributeValue = entity.Brand });
                param.Add(new SpuAttributeModel() { AttributeName = "单位", AttributeValue = entity.Unit });
                if (!string.IsNullOrEmpty(entity.Tag))
                {
                    param.Add(new SpuAttributeModel() { AttributeName = "标签", AttributeValue = entity.Tag });
                }
                return param;
            }
            else
            {
                return new List<SpuAttributeModel>();
            }

        }

        private static readonly List<String> hsExpandAttr = new List<string>
        {
            "Materials","Pattern","Flavour","Ingredients","StoragePeriod","StoringTemperature",
            "SkinType","Gender","AgeGroup","Model","BatteryTime","Voltage","Power","Warranty",
            "SupportedLanguage","PetType","PetAge","PetAgeUnit","Location","Weight","WeightUnit",
            "Volume","VolumeUnit","Length","LengthUnit","Width","WidthUnit","Height","HeightUnit"
        };

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="attrIds"></param>
        /// <param name="sort"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<ProductInfoModel> GetProductList(int categoryId, int level, List<ProductFilterAttrubile> attrIds, int sort, int pageindex, int pagesize, int language, int deliveryRegion, ref List<CategoryAttribute> attributes, out int totalRecords)
        {
            List<ProductInfoModel> list = new List<ProductInfoModel>();
            totalRecords = 0;
            try
            {
                string cacheName = ConstClass.RedisKey4MPrefix + "_ProductList_" + categoryId + "_" + sort + "_" + pageindex;
                // list = RedisCacheHelper.Get<List<ProductInfoModel>>(cacheName);
                if (list == null || list.Count == 0)
                {
                    /*
                    var indexingServiceClient = new IndexingServiceClient();
                    var request = new ProductRequestDTO();
                    if (attrIds != null && attrIds.Count > 0)
                    {
                        String strSkuMain = null, strSkuSub = null;
                        if (attributes != null && attributes.Count > 0)
                        {
                            var SkuMain = attributes.Find(p => p.IsSkuMainAttr == 1);
                            if (SkuMain != null)
                            {
                                strSkuMain = SkuMain.KeyName;
                                var SkuSub = attributes.Find(p => p.IsSkuAttr == 1 && p.IsSkuMainAttr == 0);
                                if (SkuSub != null)
                                {
                                    strSkuSub = SkuSub.KeyName;
                                }
                            }
                        }
                        var dic = new Dictionary<String, String>();
                        foreach (var attr in attrIds)
                        {
                            if (strSkuMain != null && String.Equals(strSkuMain, attr.KeyName, StringComparison.OrdinalIgnoreCase))
                            {
                                request.SkuMainValue = attr.KeyValue;
                            }
                            else if (strSkuSub != null && String.Equals(strSkuSub, attr.KeyName, StringComparison.OrdinalIgnoreCase))
                            {
                                request.SkuSubValue = attr.KeyValue;
                            }
                            else if (hsExpandAttr.Exists(p => String.Equals(p, attr.KeyName, StringComparison.OrdinalIgnoreCase)))
                            {
                                dic.Add(attr.KeyName, attr.KeyValue);
                            }
                        }
                        if (dic.Count > 0)
                        {
                            request.FilterQuery = dic;
                        }
                    }
                    request.Page = pageindex;
                    request.RowsCount = pagesize;
                    request.CategoryId = categoryId.ToString();
                    if (sort == 1)
                    {
                        request.OrderBy = "shelvesTime";
                        request.Descending = true;
                    }
                    else if (sort == 2)
                    {
                        request.OrderBy = "MinPrice";
                        request.Descending = false;
                    }
                    else if (sort == 3)
                    {
                        request.OrderBy = "MinPrice";
                        request.Descending = true;
                    }
                    var result = indexingServiceClient.SearchProductResult(request);
                    if (result.HasError == false)
                    {
                        if (result.ProductList!=null && result.ProductList.Length > 0)
                        {
                            list = ConvertProductDTOToProductInfoModel(result.ProductList);
                            DataSet ds = dal.GetSkuBySpuID(list.Select(x => x.ProductId).ToList());
                            DataTable dt0 = ds.Tables[0];
                            DataTable dt1 = ds.Tables[1];
                            if (dt1 != null && dt1.Rows.Count > 0)
                            {
                                foreach (var p in list)
                                {
                                    var qty = 0;
                                    DataRow[] rows = dt1.Select("spuid=" + p.ProductId.ToString());
                                    List<String> skuList = new List<String>();
                                    foreach (DataRow r in rows)
                                    {
                                        skuList.Add(r["Sku"].ToString());
                                        qty += int.Parse(r["ForOrderQty"].ToString());
                                    }
                                    p.Qty = qty;
                                    DataRow row = dt0.Select("id=" + p.ToString()).First();
                                    qty = Int32.Parse(row["MinForOrder"].ToString());
                                    p.SPU = row["Spu"].ToString();
                                    p.MinPrice = Decimal.Parse(row["MinPrice"].ToString());
                                    p.Qty -= qty;
                                    p.SkuList = skuList;
                                }
                            }
                        }
                        totalRecords = result.TotalCount;
                    }*/

                    list = dal.GetProductList(categoryId, level, attrIds, sort, pageindex, pagesize, language, deliveryRegion, ref attributes, out totalRecords);
                    DataTable dt = dal.GetSkuBySpu(list.Select(x => x.SPU).ToList());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (var p in list)
                        {
                            DataRow[] rows = dt.Select("spu='" + p.SPU + "'");
                            List<string> skuList = new List<string>();
                            foreach (DataRow r in rows)
                            {
                                skuList.Add(r["Sku"].ToString());
                            }
                            p.SkuList = skuList;
                        }
                    }
                    #region 折扣信息
                    //List<PromotionSpu> promotionlist = promotionDal.GetAvaliablePromotionPrice(list.Select(x => x.SPU).ToArray()).ToList();
                    //if (promotionlist != null && promotionlist.Any())
                    //{
                    //    foreach (var p in list)
                    //    {
                    //        var tempSpu = promotionlist.FirstOrDefault(a => a.Spu == p.SPU);
                    //        if (tempSpu != null)
                    //        {
                    //            p.DiscountPrice = tempSpu.DiscountPrice;
                    //            p.DiscountRate = tempSpu.DiscountRate.ToString().TrimEnd('0').Replace(".0", "");
                    //        }
                    //    }
                    //}
                    #endregion
                }
                else
                {
                    totalRecords = list.FirstOrDefault().TotalRecord;
                }
                // RedisCacheHelper.Add(cacheName, list, 60);
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return list;
            }
        }

        /// <summary>
        /// 商品列表只筛选品牌2016-03-18
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="attrIds"></param>
        /// <param name="sort"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<ProductInfoModel> GetProductListNew(string[] categoryId, int level, List<int> brandIds, int sort, int pageindex, int pagesize, int language, int deliveryRegion, decimal exchangeRate, ref List<CategoryAttribute> attributes, out int totalRecords)
        {
            List<ProductInfoModel> list = new List<ProductInfoModel>();
            totalRecords = 0;
            try
            {
                string cacheName = ConstClass.RedisKey4MPrefix + "_ProductList_" + categoryId + "_" + sort + "_" + pageindex;
                // list = RedisCacheHelper.Get<List<ProductInfoModel>>(cacheName);
                if (list == null || list.Count == 0)
                {
                    /*
                    var indexingServiceClient = new IndexingServiceClient();
                    var request = new ProductRequestDTO();
                    if (attrIds != null && attrIds.Count > 0)
                    {
                        String strSkuMain = null, strSkuSub = null;
                        if (attributes != null && attributes.Count > 0)
                        {
                            var SkuMain = attributes.Find(p => p.IsSkuMainAttr == 1);
                            if (SkuMain != null)
                            {
                                strSkuMain = SkuMain.KeyName;
                                var SkuSub = attributes.Find(p => p.IsSkuAttr == 1 && p.IsSkuMainAttr == 0);
                                if (SkuSub != null)
                                {
                                    strSkuSub = SkuSub.KeyName;
                                }
                            }
                        }
                        var dic = new Dictionary<String, String>();
                        foreach (var attr in attrIds)
                        {
                            if (strSkuMain != null && String.Equals(strSkuMain, attr.KeyName, StringComparison.OrdinalIgnoreCase))
                            {
                                request.SkuMainValue = attr.KeyValue;
                            }
                            else if (strSkuSub != null && String.Equals(strSkuSub, attr.KeyName, StringComparison.OrdinalIgnoreCase))
                            {
                                request.SkuSubValue = attr.KeyValue;
                            }
                            else if (hsExpandAttr.Exists(p => String.Equals(p, attr.KeyName, StringComparison.OrdinalIgnoreCase)))
                            {
                                dic.Add(attr.KeyName, attr.KeyValue);
                            }
                        }
                        if (dic.Count > 0)
                        {
                            request.FilterQuery = dic;
                        }
                    }
                    request.Page = pageindex;
                    request.RowsCount = pagesize;
                    request.CategoryId = categoryId.ToString();
                    if (sort == 1)
                    {
                        request.OrderBy = "shelvesTime";
                        request.Descending = true;
                    }
                    else if (sort == 2)
                    {
                        request.OrderBy = "MinPrice";
                        request.Descending = false;
                    }
                    else if (sort == 3)
                    {
                        request.OrderBy = "MinPrice";
                        request.Descending = true;
                    }
                    var result = indexingServiceClient.SearchProductResult(request);
                    if (result.HasError == false)
                    {
                        if (result.ProductList!=null && result.ProductList.Length > 0)
                        {
                            list = ConvertProductDTOToProductInfoModel(result.ProductList);
                            DataSet ds = dal.GetSkuBySpuID(list.Select(x => x.ProductId).ToList());
                            DataTable dt0 = ds.Tables[0];
                            DataTable dt1 = ds.Tables[1];
                            if (dt1 != null && dt1.Rows.Count > 0)
                            {
                                foreach (var p in list)
                                {
                                    var qty = 0;
                                    DataRow[] rows = dt1.Select("spuid=" + p.ProductId.ToString());
                                    List<String> skuList = new List<String>();
                                    foreach (DataRow r in rows)
                                    {
                                        skuList.Add(r["Sku"].ToString());
                                        qty += int.Parse(r["ForOrderQty"].ToString());
                                    }
                                    p.Qty = qty;
                                    DataRow row = dt0.Select("id=" + p.ToString()).First();
                                    qty = Int32.Parse(row["MinForOrder"].ToString());
                                    p.SPU = row["Spu"].ToString();
                                    p.MinPrice = Decimal.Parse(row["MinPrice"].ToString());
                                    p.Qty -= qty;
                                    p.SkuList = skuList;
                                }
                            }
                        }
                        totalRecords = result.TotalCount;
                    }*/

                    //list = dal.GetProductList(categoryId, level, attrIds, sort, pageindex, pagesize, language, deliveryRegion, ref attributes, out totalRecords);

                    list = dal.GetProductListNew(categoryId, level, brandIds, sort, pageindex, pagesize, language, deliveryRegion, ref attributes, out totalRecords);
                    DataTable dt = dal.GetSkuBySpu(list.Select(x => x.SPU).ToList());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (var p in list)
                        {
                            DataRow[] rows = dt.Select("spu='" + p.SPU + "'");
                            List<string> skuList = new List<string>();
                            foreach (DataRow r in rows)
                            {
                                skuList.Add(r["Sku"].ToString());
                            }
                            p.SkuList = skuList;
                        }
                    }
                    #region 折扣信息
                    //List<PromotionSpu> promotionlist = promotionDal.GetAvaliablePromotionPrice(list.Select(x => x.SPU).ToArray()).ToList();
                    //if (promotionlist != null && promotionlist.Any())
                    //{
                    //    foreach (var p in list)
                    //    {
                    //        var tempSpu = promotionlist.FirstOrDefault(a => a.Spu == p.SPU);
                    //        if (tempSpu != null)
                    //        {
                    //            p.DiscountPrice = tempSpu.DiscountPrice*exchangeRate;
                    //            p.DiscountRate = tempSpu.DiscountRate.ToNumberStringFloat();
                    //        }
                    //    }
                    //}
                    #endregion
                }
                else
                {
                    totalRecords = list.FirstOrDefault().TotalRecord;
                }
                // RedisCacheHelper.Add(cacheName, list, 60);
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return list;
            }
        }

        private List<ProductInfoModel> ConvertProductDTOToProductInfoModel(ProductDTO[] ProductList)
        {
            var list = new List<ProductInfoModel>(ProductList.Length);
            foreach (var p in ProductList)
            {
                var m = new ProductInfoModel();
                m.CategoryId = p.CategoryIdSource;
                m.ImagePath = p.ImagePath == null ? null : ConfigHelper.ImageServer + p.ImagePath.Replace('\\', '/');
                m.ProductId = p.ProductId;
                m.Name = p.ProductName;
                //m.SupplierID = p.SupplierID;
                list.Add(m);
            }
            return list;
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
            List<ProductInfoModel> list = new List<ProductInfoModel>();
            totalRecords = 0;
            try
            {
                list = dal.GetSupplierProductListById(supplierId, sort, pageindex, pagesize, language, salesTerritory, out totalRecords);
                DataTable dt = dal.GetSkuBySpu(list.Select(x => x.SPU).ToList());
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (var p in list)
                    {
                        DataRow[] rows = dt.Select("spu='" + p.SPU + "'");
                        List<string> skuList = new List<string>();
                        foreach (DataRow r in rows)
                        {
                            skuList.Add(r["Sku"].ToString());
                        }
                        p.SkuList = skuList;
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return list;
            }
        }

        #region 扩展备份
        ///// <summary>
        ///// sku销售属性获取
        ///// </summary>
        ///// <param name="spu"></param>
        ///// <returns></returns>
        //public ProductAttributeEntity[] GetProductAttributeMetadatas(string spu)
        //{
        //    return dal.GetAttributeMetaDatas(spu);
        //} 
        #endregion
        /// <summary>
        /// 获取描述字段信息
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public string GetDescription(string productCode, int language)
        {

            try
            {
                return dal.GetDescription(productCode, language);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return string.Empty;
            }

        }
        /// <summary>
        /// 获取评价信息
        /// </summary>
        /// <param name="spuId"></param>
        /// <param name="language"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<CommentEntity> GetCommentEntities(string spuId, int language, out int recordCount, int pageIndex = 1, int pageSize = 20)
        {
            recordCount = 0;
            try
            {
                return dal.GetCommentEntities(spuId, language, out recordCount, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<CommentEntity>();
            }
        }

        /// <summary>
        /// 获取商家商品列表
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="top"></param>
        /// <param name="language"></param>
        /// <param name="deliveryRegion"></param>
        /// <returns></returns>
        public IList<ProductInfoModel> GetRecommendedProductList(int brandId, int top, int language, int deliveryRegion, string spu)
        {

            try
            {
                var list = dal.GetProductListBySupplier(brandId, top, language, deliveryRegion, spu);
                var spus = list.Select(x => x.SPU).ToArray();
                if (spus != null && spus.Any())
                {
                    List<PromotionSpu> promotionlist = promotionDal.GetAvaliablePromotionPrice(spus,1).ToList();
                    return list.FillPromotions(promotionlist);//补充promotion信息
                }
                else
                {
                    return new List<ProductInfoModel>();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<ProductInfoModel>();
            }
        }
        /// <summary>
        /// 获取sku产品信息
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public ProductSkuEntity GetProductBySku(string sku, int language)
        {
            return dal.GetProductBySku(sku, language);
        }

        /// <summary>
        /// 获取sku产品信息
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public ProductOrderSkuEntity GetProductOrderBySku(string sku, int language)
        {
            return dal.GetProductOrderBySku(sku, language);
        }
        /// <summary>
        /// 获取品牌商品列表
        /// </summary>
        /// <param name="supplierId"></param> 
        /// <param name="sort"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<ProductInfoModel> GetBrandProductListById(int brandId, int sort, int pageindex, int pagesize, int language, int salesTerritory)
        {
            List<ProductInfoModel> list = new List<ProductInfoModel>();
            try
            {
                list = dal.GetBrandProductListById(brandId, sort, pageindex, pagesize, language, salesTerritory);
                if (list != null && list.Any())
                {
                    DataTable dt = dal.GetSkuBySpu(list.Select(x => x.SPU).ToList());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (var p in list)
                        {
                            DataRow[] rows = dt.Select("spu='" + p.SPU + "'");
                            List<string> skuList = new List<string>();
                            foreach (DataRow r in rows)
                            {
                                skuList.Add(r["Sku"].ToString());
                            }
                            p.SkuList = skuList;
                        }
                    }

                    //List<PromotionSpu> promotionlist = promotionDal.GetAvaliablePromotionPrice(list.Select(x => x.SPU).ToArray()).ToList();
                    //if (promotionlist != null && promotionlist.Any())
                    //{
                    //    foreach (var p in list)
                    //    {
                    //        var tempSpu = promotionlist.FirstOrDefault(a => a.Spu == p.SPU);
                    //        if (tempSpu != null)
                    //        {
                    //            p.DiscountPrice = tempSpu.DiscountPrice;
                    //            p.DiscountRate = ((100 - tempSpu.DiscountRate) / 10).ToString().TrimEnd('0').Replace(".0", "");
                    //        }
                    //    }
                    //}
                }
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return list;
            }
        }
        /// <summary>
        /// 加载品牌下所有商品
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="sort"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="language"></param>
        /// <param name="salesTerritory"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<ProductInfoModel> GetBrandProductListFromCache(int brandId, int sort, int pageindex, int pagesize, int language, int salesTerritory, out int totalRecords)
        {
            
            try
            {
                var modules = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, ConstClass.RedisKeyBrandPList, () =>
                {
                    var data = GetBrandProductListById(brandId, sort, pageindex, pagesize, language, salesTerritory);
                    return data;
                }, 1440);//缓存24小时
                totalRecords=modules.Count();
                return modules;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                var modules = GetBrandProductListById(brandId, sort, pageindex, pagesize, language, salesTerritory);
                totalRecords = modules.Count;
                return modules;
            }
        }
        /// <summary>
        /// 拼生活的查询列表
        /// </summary>
        public List<ProductInfoModel> GetProductFightList(int page, int pagesize, int language, decimal ExchangeRate, string flagSpu)
        {
            string startTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinStartTime"].ToString();
            string endTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinEndTime"].ToString();
            List<ProductInfoModel> list = new List<ProductInfoModel>();
            list = dal.GetProductFightList(language, page, pagesize, ExchangeRate, startTime, endTime, flagSpu);
            return list;
        }
        /// <summary>
        /// 拼生活商品详情页
        /// </summary>
        public IList<ProductInfoModel> GetProductFightDetail(string sku, decimal ExchangeRate, int pid)
        {
            string startTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinStartTime"].ToString();
            string endTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinEndTime"].ToString();
            IList<ProductInfoModel> productInfoModel = dal.GetProductFightDetail(sku, ExchangeRate, startTime, endTime, pid);
            return productInfoModel;
        }

        public IList<ProductInfoModel> GetProductFightDetailForShare(string sku, decimal ExchangeRate, int pid)
        {
            string startTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinStartTime"].ToString();
            string endTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinEndTime"].ToString();
            IList<ProductInfoModel> productInfoModel = dal.GetProductFightDetailForShare(sku, ExchangeRate, startTime, endTime, pid);
            return productInfoModel;
        }

        /// <summary>
        /// 通过sku orderCode 获得OrderProducts的酒豆值
        /// </summary>
        public ProductInfoModel getProductInfo(string orderCode, string sku)
        {
            return dal.getProductInfo(orderCode, sku);
        }
        /// <summary>
        ///商品收藏列表页
        /// </summary>

        public List<Favorite> GetFavoriteList(int page, int pagesize, int language, decimal ExchangeRate, int userId)
        {
            List<Favorite> list = new List<Favorite>();
            list = dal.GetFavoriteList(language, page, pagesize, ExchangeRate, userId);
            return list;
        }
        public bool isFavorite(string productCode, string collectionStatus, int userId, ProductDto product)
        {
            //查询收藏表的信息
            bool isture = false;

            if (collectionStatus.Equals("true"))
            {
                //有更新
                isture = dal.isFavorite(productCode, collectionStatus, userId);
            }
            else
            {
                //没有插入
                if (product.Promotion != null)
                {
                    isture = dal.insertInto(productCode, userId, product.PromotionDiscountPrice);
                }
                else
                {
                    isture = dal.insertInto(productCode, userId, product.MinPrice);
                }

            }
            return false;
        }
        public bool getFivoriteDetail(string spu, int userId)
        {
            bool istrue = false;
            istrue = dal.selectSpu(spu, userId);
            return istrue;
        }

        #region 根据SPU获取当前所有在售SKU库存
        /// <param name="spu">SPU</param>
        /// <param name="language">语言</param>
        /// <param name="salesTerritory">销售区域,1.大陆，2.中华人民共和国大陆地区，3.大陆和中华人民共和国大陆地区</param>
        /// <returns>返回是否已售罄，以及当前SPU下所有在售的SKU</returns>
        public DataTable GetStockInfoBySpu(string spu, int language, int salesTerritory)
        {
            return dal.GetStockInfoBySpu(spu, language, salesTerritory);
        }

        /// <summary>
        /// 返回spu和对应的促销价格
        /// </summary>
        /// <param name="spus"></param>
        /// <returns></returns>
        public IList<PromotionSpu> GetAvaliablePromotionPrice(string[] spus)
        {
            return promotionDal.GetAvaliablePromotionPrice(spus, 1);
        }
        #endregion
        public IList<ProductInfoModel> GetProductListBySpus(string spus)
        {
            return dal.GetProductListBySpus(spus);
        }


        #region 获取专题活动母版页某个模块的数据 V2.0
        /// <param name="activityKey">专题唯一标识</param>
        /// <param name="moduleKey">专题模块标识</param>
        /// <param name="spus">模块内产品spu</param>
        public IList<ProductInfoModel> GetProductListBySpusFromCache(string spus, string activityKey, string moduleKey = "")
        {
            try
            {
                string key = string.Format("{0}_{1}", activityKey, moduleKey);
                var products = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, key, () =>
                {
                    var data = dal.GetProductListBySpus(spus);
                    return data;
                }, 1440);

                return products;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                return new List<ProductInfoModel>();
            }
        }
        #endregion

        /// <summary>
        /// 根据sku获取时令美食spu信息
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public ProductExpandEntity GetHolidaySpuInfoBySku(string sku, int language)
        {
            try
            {
                return dal.GetHolidaySpuInfoBySku(sku, language);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 判断是否是节日食品
        /// </summary>
        /// <param name="spu"></param>
        /// <returns></returns>

        public bool isHolidayGoods(string spu)
        {
            return dal.isHolidayGoods(spu, Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["holidayFoodsKey"]));
        }

        /// <summary>
        /// 判断是否是节日食品 只能传三级类目
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>

        public bool isHolidayGoods(int CategoryId)
        {
            return dal.isHolidayGoods(CategoryId).Equals(System.Web.Configuration.WebConfigurationManager.AppSettings["holidayFoodsKey"].ToString());
        }

        /// <summary>
        /// 时令美食购买条件验证
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="qty"></param>
        /// <param name="spu"></param>
        /// <param name="language"></param>
        /// <param name="ExchangeRate"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public Dictionary<int, string> CheckSkuHolidaySpuValidate(
            string sku, int qty, string spu, int language, decimal ExchangeRate, bool IsSubmitOrder, decimal price=0M)
        {

            Dictionary<int, string> ResultMap = new Dictionary<int, string>();

            ProductSkuEntity ProductSkuEntity = null;
            string HolidaySpu = string.Empty;
            
            // 提交订单操作，传入的商品spu为空，重新检索商品spu信息
            if (IsSubmitOrder && string.IsNullOrEmpty(spu))
            {
                // 根据sku获取spu信息
                ProductSkuEntity = GetProductBySku(sku, language);
                if (ProductSkuEntity == null)
                {
                    ResultMap.Add(0, "该商品不存在或已下架。");
                    return ResultMap;
                }

                HolidaySpu = ProductSkuEntity.Spu;

                // 获取商品促销价
                var promotionsku = itemBll.GetPromotionInfoBySku(sku);
                price = ProductSkuEntity.DiscountPrice * ExchangeRate;
                if (ProductSkuEntity.DiscountPrice == 0 || (promotionsku.Count > 0 && promotionsku.FirstOrDefault().PromotionType == 2))
                {
                    price = ProductSkuEntity.MinPrice * ExchangeRate;
                }
            }
            else
            {
                HolidaySpu = spu;
            }

            // 判断是否是节日食品
            bool IsHolidayFoods = isHolidayGoods(HolidaySpu);
            if (IsHolidayFoods)
            {
                // 根据sku获取时令美食spu信息
                ProductExpandEntity HolidaySpuInfo = GetHolidaySpuInfoBySku(sku, language);

                if (HolidaySpuInfo == null)
                {
                    ResultMap.Add(0, "该商品不存在或已下架。");
                    return ResultMap;
                }

                // 时令美食 月饼 购买条件：不能超过两件；不能超过3Kg；不能超过800元（金额判断已经存在，这里只进行数量和重量的判断）
                if (Convert.ToString(HolidaySpuInfo.CategoryId).Equals(System.Web.Configuration.WebConfigurationManager.AppSettings["MoonCakeKey"].ToString()))
                {
                    if (HolidaySpuInfo.Weight <= 0)
                    {
                        ResultMap.Add(0, "该商品的重量数据异常！");
                        return ResultMap;
                    }

                    // 时令美食限购数量验证
                    ResultMap = HolidaySpuBuyNumValidate(IsSubmitOrder, HolidaySpuInfo.Weight, price * ExchangeRate, qty);
                }
            }
            else
            {
                ResultMap.Add(1, "");
            }

            return ResultMap;
        }

        /// <summary>
        ///  取spu下的sku
        /// </summary>
        /// <param name="spuList"></param>
        /// <returns></returns>
        public DataTable GetSkuBySpu(List<string> spuList)
        {
            return dal.GetSkuBySpu(spuList);
        }

        #region 根据Spu获取商品名称
        public string GetProductNameBySpu(string spu, int language = 1)
        {
            return dal.GetProductNameBySpu(spu,language);
        }
        #endregion

        /// <summary>
        /// 时令美食限购数量验证 （清关规定，不能超过两件，不能超过3Kg，不能超过800元）
        /// </summary>
        /// <param name="IsSubmitOrder"></param>
        /// <param name="Weight"></param>
        /// <param name="priceExchange"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public Dictionary<int, string> HolidaySpuBuyNumValidate(bool IsSubmitOrder,decimal Weight,decimal priceExchange,int qty)
        {
            Dictionary<int, string> ResultMap = new Dictionary<int, string>();
            decimal MaxNum = 0M;
            decimal TotalWeight = Weight * qty;

            // 提交订单操作
            if (IsSubmitOrder)
            {
                // 获取限购商品数量 全条件判断
                MaxNum = holidayBll.GetCanBuyNumberByAll(Weight, priceExchange);
                if (TotalWeight > 3 || qty > 2)
                {
                    ResultMap.Add(0, "该商品每次最多限购" + MaxNum.ToNumberStringNoZero() + "件。");
                    return ResultMap;
                }
                else if (priceExchange * qty > 800)
                {
                    ResultMap.Add(0, "单笔订单金额不能超过" + ConfigHelper.GetAppSetting<decimal>("OrderLimitValue") + "元。");
                    return ResultMap;
                }
                else
                {
                    ResultMap.Add(1, "");
                    return ResultMap;
                }
            }
            // 增加商品数量操作
            else
            {
                // 获取限购商品数量 仅重量和件数判断
                MaxNum = holidayBll.GetCanBuyNumberByWeight(Weight);
                if (TotalWeight > 3 || qty > 2)
                {
                    ResultMap.Add(0, "该商品每次最多限购" + MaxNum.ToNumberStringNoZero() + "件。");
                    return ResultMap;
                }
                else
                {
                    ResultMap.Add(1, "");
                    return ResultMap;
                }
            }
        }

        /// <summary>
        /// 获取所有待上架的sku
        /// </summary>
        /// <returns></returns>
        public List<SkuInfo> GetPreShowSku()
        {
            List<SkuInfo> list = new List<SkuInfo>();

            var result = dal.GetPreShowSku();

            list = result.ToList();

            return list ;

        }


        /// <summary>
        /// 更新 待上架商品为已上架
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool UpdatePreShowSku(SkuInfo item)
        {
            bool result = false;
            result = dal.UpdatePreShowSku(item);


            return result;
        }
        /// <summary>
        /// 更新库存
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool InsertStock(SkuInfo item)
        {

            bool result = false;
            result = dal.InsertStock(item);

            return result;
        }
        /// <summary>
        /// 自动缓存商品列表
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="language"></param>
        /// <param name="salesTerritory"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public ListProductInfoModel GetPListFromCache(int sort, int pageindex, int pagesize, int language, int salesTerritory, int SourceType)
        {
            try
            {
                var modules = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, ConstClass.RedisKeyPList + SourceType, () =>
                {
                    var data = GetPList(sort,pageindex,pagesize,language,salesTerritory);
                    return data;
                }, 1440);//缓存24小时
                return modules;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                var modules = GetPList(sort, pageindex, pagesize, language, salesTerritory);
                return modules;
            }
        }
        /// <summary>
        /// 商品列表数据
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="sort"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="language"></param>
        /// <param name="salesTerritory"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public ListProductInfoModel GetPList(int sort, int pageindex, int pagesize, int language, int salesTerritory)
        {
            ListProductInfoModel listmodel = new ListProductInfoModel();

            List<ProductInfoModel> list = new List<ProductInfoModel>();
            try
            {
                list = dal.GetPList(sort, pageindex, pagesize, language, salesTerritory);
                if (list != null && list.Any())
                {
                    DataTable dt = dal.GetSkuBySpu(list.Select(x => x.SPU).ToList());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (var p in list)
                        {
                            DataRow[] rows = dt.Select("spu='" + p.SPU + "'");
                            List<string> skuList = new List<string>();
                            foreach (DataRow r in rows)
                            {
                                skuList.Add(r["Sku"].ToString());
                            }
                            p.SkuList = skuList;
                        }
                    }
                }
                listmodel.ProductInfoModel = list;
                return listmodel;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                listmodel.ProductInfoModel = list;
                return listmodel;
            }
        }

    }
}
