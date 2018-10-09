using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFO2O.Supplier.DAO.Product;
using SFO2O.Supplier.Common;
using SFO2O.Supplier.Models;
using SFO2O.Supplier.Models.Product;
using SFO2O.Supplier.Models.Category;
using SFO2O.Supplier.ViewModels.Product;
using System.Reflection;
using System.Data;

namespace SFO2O.Supplier.Businesses.Category
{
    public class ProductInfoBLL
    {
        private readonly ProductInfoDAL productDAL = new ProductInfoDAL();

        public PageOf<ProductInfoTemp> GetProductTemps(int supplierId, Models.LanguageEnum languageVersion, PageDTO page)
        {
            return productDAL.GetProductTemps(supplierId, languageVersion, page);
        }

        public List<ProductTempImageModel> GetProductImages(string spu)
        {
            return productDAL.GetProductImages(spu).ToList();
        }

        public long GetNewSpuId()
        {
            return productDAL.GetNewSpuOrSku("10", "Spu");
        }

        public long GetNewSkuId()
        {
            return productDAL.GetNewSpuOrSku("20", "Sku");
        }

        public bool DeleteProductById(int productId, int userId)
        {
            return productDAL.DeleteProductById(productId, userId);
        }

        public bool DeleteAllProduct(string productIds, int userId)
        {
            return productDAL.DeleteAllProduct(productIds, userId);
        }

        public List<CategoryAttrModel> GetCategoryAttrs(int categoryId)
        {
            if (categoryId <= 10000)
            {
                return new List<CategoryAttrModel>();
            }

            return productDAL.GetCategoryAttrs(categoryId).ToList();
        }

        public string SaveUnreleasedProduct(ProductJsonModel pjModel, int supplierId, int userId, bool isRelease)
        {
            if (null == pjModel)
            {
                return String.Empty;
            }

            if (null == pjModel.SPU)
            {
                return String.Empty;
            }

            var isNewProduct = false;//是否是新的商品，新的商品Insert，已存在的Update

            if (String.IsNullOrEmpty(pjModel.SPU.Spu))
            {
                pjModel.SPU.Spu = GetNewSpuId().ToString();
                isNewProduct = true;
            }

            var brandInfo = productDAL.GetBrandNameByIdAndSupplierId(pjModel.SPU.BrandId, supplierId);

            var ptms = GetProductInfoModels(pjModel, supplierId, userId, isRelease, true, brandInfo);

            Dictionary<LanguageEnum, ProductExtTempModel> ptmExs = null;

            if (null != pjModel.SpuEx)
            {
                ptmExs = GetProductExtendModels(pjModel, ptmExs);
            }

            var skus = GetSkuModels(pjModel, isRelease);

            var spuImages = GetProductImgModels(pjModel, userId);

            if (isNewProduct == true)
            {
                productDAL.CreateProduct(ptms, ptmExs, skus, spuImages);
            }
            else
            {
                productDAL.UpdateProduct(-2, ptms, ptmExs, skus, spuImages);
            }

            return ptms[LanguageEnum.SimplifiedChinese].Spu;
        }

        /// <summary>
        /// 保存线上商品的编辑
        /// </summary>
        /// <param name="pjModel"></param>
        /// <param name="supplierId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string SaveReleasedProduct(ProductJsonModel pjModel, int supplierId, int userId)
        {
            if (null == pjModel)
            {
                return String.Empty;
            }

            if (null == pjModel.SPU)
            {
                return String.Empty;
            }

            if (null == pjModel.SKUS || pjModel.SKUS.Length == 0)
            {
                return string.Empty;
            }

            var spu = pjModel.SPU.Spu;

            var productInfos_Online = productDAL.GetReleasedProductInfo(spu, supplierId);

            var brandInfo = productDAL.GetBrandNameByIdAndSupplierId(pjModel.SPU.BrandId, supplierId);
            var ptms = GetProductInfoModels(pjModel, supplierId, userId, true, false, brandInfo);

            foreach (var ptm in ptms)
            {
                ptm.Value.Createtime = productInfos_Online[LanguageEnum.TraditionalChinese].Createtime;
                ptm.Value.CreateBy = productInfos_Online[LanguageEnum.TraditionalChinese].CreateBy;
                ptm.Value.ModifyTime = DateTime.Now;
                ptm.Value.ModifyBy = userId.ToString();
            }

            Dictionary<LanguageEnum, ProductExtTempModel> ptmExs = null;

            if (null != pjModel.SpuEx)
            {
                ptmExs = GetProductExtendModels(pjModel, ptmExs);
            }

            var skus = GetSkuModels(pjModel, true, true);

            var spuImages = GetProductImgModels(pjModel, userId);

            productDAL.SaveOnlineProduct(ptms, ptmExs, skus, spuImages);

            return ptms[LanguageEnum.TraditionalChinese].Spu;
        }

        #region 查看商品

        public ViewProductModel GetProductViewInfo(string spu, int supplierId, int status, bool isOnline)
        {
            var ptms = new Dictionary<LanguageEnum, ProductTempModel>();
            IList<ProductTempImageModel> ImageList = null;
            var ptExs = new Dictionary<LanguageEnum, ProductExtTempModel>();
            var skus = new Dictionary<LanguageEnum, List<SkuTempModel>>();

            if (isOnline == true)//获取线上数据
            {
                ptms = productDAL.GetReleasedProductInfo(spu, supplierId);
                ImageList = productDAL.GetProductImages(spu);
                ptExs = productDAL.GetReleasedProductExtendInfo(spu);
                skus = productDAL.GetReleasedSkuInfo(spu);
            }
            else//获取非线上数据
            {
                ptms = productDAL.GetTempProductInfo(spu, supplierId, status);
                var spuIDs = new List<int>();

                foreach (var spuInfo in ptms)
                {
                    var spuValue = spuInfo.Value;
                    if (null != spuValue)
                    {
                        spuIDs.Add(spuValue.Id);
                    }
                }

                ImageList = productDAL.GetProductImageTemps(spu);
                ptExs = productDAL.GetTempProductExtendInfo(spuIDs.ToArray());
                skus = productDAL.GetTempSkuInfo(spu, spuIDs.ToArray());
            }
            var viewModel = new ViewProductModel();
            if (ptms != null && ptms.Count > 0)
            {
                //viewModel.ProductInfo_TC = ptms[LanguageEnum.TraditionalChinese];
                viewModel.ProductInfo_SC = ptms[LanguageEnum.SimplifiedChinese];
                //viewModel.ProductInfo_EN = ptms[LanguageEnum.English];
            }
            else
            {
                return viewModel;
            }

            CategoryHistoryModel categoryModel;
            //categoryModel = CategoryBLL.GetCategoriesInfoByID(LanguageEnum.TraditionalChinese, viewModel.ProductInfo_TC.CategoryId);
            //viewModel.ProductInfo_TC.CategoryName = string.Format("{0}＞{1}＞{2}", categoryModel.FCategoryName, categoryModel.SCategoryName, categoryModel.TCategoryName);
            categoryModel = CategoryBLL.GetCategoriesInfoByID(LanguageEnum.SimplifiedChinese, viewModel.ProductInfo_SC.CategoryId);
            viewModel.ProductInfo_SC.CategoryName = string.Format("{0}＞{1}＞{2}", categoryModel.FCategoryName, categoryModel.SCategoryName, categoryModel.TCategoryName);
            //categoryModel = CategoryBLL.GetCategoriesInfoByID(LanguageEnum.English, viewModel.ProductInfo_EN.CategoryId);
            //viewModel.ProductInfo_EN.CategoryName = string.Format("{0} > {1} > {2}", categoryModel.FCategoryName, categoryModel.SCategoryName, categoryModel.TCategoryName);

            //viewModel.ProductInfo_TC.ImageList = ImageList;
            viewModel.ProductInfo_SC.ImageList = ImageList;
            //viewModel.ProductInfo_EN.ImageList = ImageList;

            //viewModel.ProductInfo_TC.ExpandInfo = ptExs[LanguageEnum.TraditionalChinese];
            viewModel.ProductInfo_SC.ExpandInfo = ptExs[LanguageEnum.SimplifiedChinese];
            //viewModel.ProductInfo_EN.ExpandInfo = ptExs[LanguageEnum.English];

            //viewModel.ProductInfo_TC.SkuInfoList = skus[LanguageEnum.TraditionalChinese];
            viewModel.ProductInfo_SC.SkuInfoList = skus[LanguageEnum.SimplifiedChinese];
            //viewModel.ProductInfo_EN.SkuInfoList = skus[LanguageEnum.English];

            //AppendProductKeyValueInfo(viewModel.ProductInfo_TC, LanguageEnum.TraditionalChinese);
            AppendProductKeyValueInfo(viewModel.ProductInfo_SC, LanguageEnum.SimplifiedChinese);
            //AppendProductKeyValueInfo(viewModel.ProductInfo_EN, LanguageEnum.English);

            return viewModel;
        }

        #region ExtModelName
        static Dictionary<string, string> ProductExtModelNameDic_TC = new Dictionary<string, string>
        {
            {"Color","顏色"},
            {"CountryOfManufacture","原產地"},
            {"NetWeight","淨重"},
            {"NetContent","淨含量"},
            {"Specifications","規格"},
            {"Size","尺碼"},
            {"Materials","材質"},
            {"Pattern","樣式"},
            {"AlcoholPercentage","酒精度"},
            {"Flavour","酒香型"},
            {"Ingredients","主要成分"},
            {"StoragePeriod","保質期"},
            {"StoringTemperature","儲藏溫度"},
            {"SkinType","皮膚類型"},
            {"Smell","氣味"},
            {"Gender","性別"},
            {"AgeGroup","適合年齡"},
            {"Model","產品型號"},
            {"CapacityRestriction","容量"},
            {"BatteryTime","電池使用時間"},
            {"Voltage","電壓"},
            {"Power","電源"},
            {"Warranty","質保"},
            {"Tag","商品標籤"},
            {"SupportedLanguage","支援語言"},
            {"PetType","寵物類型"},
            {"PetAge","寵物年齡"},
            {"Location","商品適用地域"},
            {"Weight","重量"},
            {"Volume","體積"},
            {"Length","長度"},
            {"Width","寬度"},
            {"Height","高度"},
            {"Flavor","口味"},
        };
        static Dictionary<string, string> ProductExtModelNameDic_SC = new Dictionary<string, string>
        {
            {"Color","颜色"},
            {"CountryOfManufacture","原产地"},
            {"NetWeight","净重"},
            {"NetContent","净含量"},
            {"Specifications","规格"},
            {"Size","尺码"},
            {"Materials","材质"},
            {"Pattern","样式"},
            {"AlcoholPercentage","酒精度"},
            {"Flavour","酒香型"},
            {"Ingredients","主要成分"},
            {"StoragePeriod","保质期"},
            {"StoringTemperature","储藏温度"},
            {"SkinType","皮肤类型"},
            {"Smell","气味"},
            {"Gender","性别"},
            {"AgeGroup","适合年龄"},
            {"Model","产品型号"},
            {"CapacityRestriction","容量"},
            {"BatteryTime","电池使用时间"},
            {"Voltage","电压"},
            {"Power","电源"},
            {"Warranty","质保"},
            {"Tag","商品标签"},
            {"SupportedLanguage","支持语言"},
            {"PetType","宠物类型"},
            {"PetAge","宠物年龄"},
            {"Location","商品适用地域"},
            {"Weight","重量"},
            {"Volume","体积"},
            {"Length","长度"},
            {"Width","宽度"},
            {"Height","高度"},
            {"Flavor","口味"},
        };
        static Dictionary<string, string> ProductExtModelNameDic_EN = new Dictionary<string, string>
        {
            {"Color","Color"},
            {"CountryOfManufacture","CountryOfManufacture"},
            {"NetWeight","NetWeight"},
            {"NetContent","NetContent"},
            {"Specifications","Specifications"},
            {"Size","Size"},
            {"Materials","Materials"},
            {"Pattern","Pattern"},
            {"AlcoholPercentage","AlcoholPercentage"},
            {"Flavour","Flavour"},
            {"Ingredients","Ingredients"},
            {"StoragePeriod","StoragePeriod"},
            {"StoringTemperature","StoringTemperature"},
            {"SkinType","SkinType"},
            {"Smell","Smell"},
            {"Gender","Gender"},
            {"AgeGroup","AgeGroup"},
            {"Model","Model"},
            {"CapacityRestriction","CapacityRestriction"},
            {"BatteryTime","BatteryTime"},
            {"Voltage","Voltage"},
            {"Power","Power"},
            {"Warranty","Warranty"},
            {"Tag","Tag"},
            {"SupportedLanguage","SupportedLanguage"},
            {"PetType","PetType"},
            {"PetAge","PetAge"},
            {"Location","Location"},
            {"Weight","Weight"},
            {"Volume","Volume"},
            {"Length","Length"},
            {"Width","Width"},
            {"Height","Height"},
            {"Flavor","Flavor"},
        };
        static Dictionary<LanguageEnum, Dictionary<String, String>> NameLanguageDic = new Dictionary<LanguageEnum, Dictionary<string, string>>
        {
            {LanguageEnum.TraditionalChinese,ProductExtModelNameDic_TC},
            {LanguageEnum.SimplifiedChinese,ProductExtModelNameDic_SC},
            {LanguageEnum.English,ProductExtModelNameDic_EN},
        };
        #endregion

        #endregion

        private void AppendProductKeyValueInfo(ProductTempModel model, LanguageEnum languageVersion)
        {
            ProductExtTempModel expandInfo = model.ExpandInfo;
            var NameDic = NameLanguageDic[languageVersion];
            var expandDic = new Dictionary<string, string>();
            var strSplit = languageVersion == LanguageEnum.English ? " ; " : "；";
            expandDic.AddIfNotEmpty(NameDic["Tag"], model.Tag);
            expandDic.AddIfNotEmpty(NameDic["CountryOfManufacture"], model.CountryOfManufacture);
            expandDic.AddIfNotEmpty(NameDic["Materials"], expandInfo.Materials);
            if (model.SkuInfoList.Any(p => !string.IsNullOrEmpty(p.Specifications)))
            {
                expandDic.AddIfNotEmpty(NameDic["Specifications"], string.Join(strSplit, model.SkuInfoList.Select(p => p.Specifications).Distinct()));
            }
            expandDic.AddIfNotEmpty(NameDic["Pattern"], expandInfo.Pattern);
            if (model.SkuInfoList.Any(p => !string.IsNullOrEmpty(p.AlcoholPercentage)))
            {
                expandDic.AddIfNotEmpty(NameDic["AlcoholPercentage"], string.Join(strSplit, model.SkuInfoList.Select(p => p.AlcoholPercentage).Distinct()));
            }
            expandDic.AddIfNotEmpty(NameDic["Flavour"], expandInfo.Flavour);
            expandDic.AddIfNotEmpty(NameDic["Flavor"], expandInfo.Flavor);
            expandDic.AddIfNotEmpty(NameDic["Ingredients"], expandInfo.Ingredients);
            expandDic.AddIfNotEmpty(NameDic["StoragePeriod"], expandInfo.StoragePeriod);
            expandDic.AddIfNotEmpty(NameDic["StoringTemperature"], expandInfo.StoringTemperature);
            if (model.SkuInfoList.Any(p => !string.IsNullOrEmpty(p.Size)))
            {
                expandDic.AddIfNotEmpty(NameDic["Size"], string.Join(strSplit, model.SkuInfoList.Select(p => p.Size).Distinct()));
            }
            if (model.SkuInfoList.Any(p => !string.IsNullOrEmpty(p.Color)))
            {
                expandDic.AddIfNotEmpty(NameDic["Color"], string.Join(strSplit, model.SkuInfoList.Select(p => p.Color).Distinct()));
            }
            expandDic.AddIfNotEmpty(NameDic["SkinType"], expandInfo.SkinType);
            expandDic.AddIfNotEmpty(NameDic["Gender"], expandInfo.Gender);
            expandDic.AddIfNotEmpty(NameDic["AgeGroup"], expandInfo.AgeGroup);
            if (model.NetWeight > 0)
            {
                expandDic.AddIfNotEmpty(NameDic["NetWeight"], model.NetWeight.ToString("f2") + model.NetWeightUnit);
            }
            if (model.SkuInfoList.Any(p => p.NetContent > 0))
            {
                expandDic.AddIfNotEmpty(NameDic["NetContent"], model.SkuInfoList.First(p => p.NetContent > 0).NetContent.ToString("f2") + model.NetContentUnit);
            }
            if (model.SkuInfoList.Any(p => !string.IsNullOrEmpty(p.Smell)))
            {
                expandDic.AddIfNotEmpty(NameDic["Smell"], string.Join(strSplit, model.SkuInfoList.Select(p => p.Smell).Distinct()));
            }
            expandDic.AddIfNotEmpty(NameDic["Model"], expandInfo.Model);
            if (model.SkuInfoList.Any(p => !string.IsNullOrEmpty(p.CapacityRestriction)))
            {
                expandDic.AddIfNotEmpty(NameDic["CapacityRestriction"], string.Join(strSplit, model.SkuInfoList.Select(p => p.CapacityRestriction).Distinct()));
            }
            expandDic.AddIfNotEmpty(NameDic["BatteryTime"], expandInfo.BatteryTime);
            expandDic.AddIfNotEmpty(NameDic["Voltage"], expandInfo.Voltage);
            expandDic.AddIfNotEmpty(NameDic["Power"], expandInfo.Power);
            expandDic.AddIfNotEmpty(NameDic["Warranty"], expandInfo.Warranty);
            expandDic.AddIfNotEmpty(NameDic["SupportedLanguage"], expandInfo.SupportedLanguage);
            expandDic.AddIfNotEmpty(NameDic["PetType"], expandInfo.PetType);
            expandDic.AddIfNotEmpty(NameDic["PetAge"], expandInfo.PetAge + expandInfo.PetAgeUnit);
            expandDic.AddIfNotEmpty(NameDic["Location"], expandInfo.Location);
            model.ExpandDic = expandDic;
            var deliveryDic = new Dictionary<string, string>();
            if (expandInfo.Weight > 0)
            {
                deliveryDic.AddIfNotEmpty(NameDic["Weight"], expandInfo.Weight + expandInfo.WeightUnit);
            }
            if (expandInfo.Volume > 0)
            {
                deliveryDic.AddIfNotEmpty(NameDic["Volume"], expandInfo.Volume + expandInfo.VolumeUnit);
            }
            if (expandInfo.Length > 0)
            {
                deliveryDic.AddIfNotEmpty(NameDic["Length"], expandInfo.Length + expandInfo.LengthUnit);
            }
            if (expandInfo.Width > 0)
            {
                deliveryDic.AddIfNotEmpty(NameDic["Width"], expandInfo.Width + expandInfo.WidthUnit);
            }
            if (expandInfo.Height > 0)
            {
                deliveryDic.AddIfNotEmpty(NameDic["Height"], expandInfo.Height + expandInfo.HeightUnit);
            }
            model.DeliveryDic = deliveryDic;
        }

        /// <summary>
        /// 加载商品信息
        /// </summary>
        /// <param name="spu"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ProductUpdateBindingModel ReadProductInfo(string spu, int supplierId, int status, bool isOnline)
        {
            var ptms = new Dictionary<LanguageEnum, ProductTempModel>();
            var ptExs = new Dictionary<LanguageEnum, ProductExtTempModel>();
            var skus = new Dictionary<LanguageEnum, List<SkuTempModel>>();
            var imgs = new List<ProductImgModel>();

            if (isOnline == true)//获取线上数据
            {
                ptms = productDAL.GetReleasedProductInfo(spu, supplierId);
                ptExs = productDAL.GetReleasedProductExtendInfo(spu);
                skus = productDAL.GetReleasedSkuInfo(spu);
                imgs = productDAL.GetOnlineProudctImg(spu);
            }
            else//获取非线上数据
            {
                ptms = productDAL.GetTempProductInfo(spu, supplierId, status);
                var spuIDs = new List<int>();

                foreach (var spuInfo in ptms)
                {
                    var spuValue = spuInfo.Value;
                    if (null != spuValue)
                    {
                        spuIDs.Add(spuValue.Id);
                    }
                }

                ptExs = productDAL.GetTempProductExtendInfo(spuIDs.ToArray());
                skus = productDAL.GetTempSkuInfo(spu, spuIDs.ToArray());
                imgs = productDAL.GetUnreleasedProudctImg(spu);

            }

            return ConvertProductUpdateBinding(ptms, ptExs, skus, imgs);
        }

        public PageOf<ProductTempModel> GetProductList(ProductListQueryInfo queryInfo, Models.LanguageEnum languageVersion, PageDTO page)
        {
            var list = productDAL.GetProductList(queryInfo, languageVersion, page);
            if (list != null && list.Items != null && list.Items.Count > 0)
            {
                foreach (var spu in list.Items)
                {
                    spu.SkuInfoList = productDAL.GetSkuList(queryInfo, spu.Id);
                }
            }
            return list;
        }

        public bool ChangeSkuStatus(ChangeSkuStatusRequest request)
        {
            return productDAL.ChangeSkuStatus(request) > 0;
        }

        public PageOf<ProductTempModel> GetAuditProductList(AuditProductListQueryInfo queryInfo, Models.LanguageEnum languageVersion, PageDTO page)
        {
            var list = productDAL.GetAuditProductList(queryInfo, languageVersion, page);
            if (list != null && list.Items != null && list.Items.Count > 0)
            {
                foreach (var spu in list.Items)
                {
                    spu.SkuInfoList = productDAL.GetAuditSkuList(queryInfo, spu.Id);
                    if (spu.SkuInfoList.Any(p => p.Status == 2))
                    {
                        spu.AuditReason = productDAL.GetAuditProductReason(spu.Spu, 2);
                    }
                }
            }
            return list;
        }

        public PageOf<SkuTempModel> GetSkuInventoryList(InventoryListQueryInfo queryInfo, Models.LanguageEnum languageVersion, PageDTO page)
        {
            var ds = productDAL.GetSkuInventoryList(queryInfo, languageVersion, page);
            return new PageOf<SkuTempModel>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                RowCount = Convert.ToInt32(ds.Tables[1].Rows[0][1]),
                Items = DataMapHelper.DataSetToList<SkuTempModel>(ds)
            };
        }

        public bool CancelEditProduct(string spu)
        {
            return productDAL.CancelEditProduct(spu);
        }

        public DataTable GetSkuInventoryListData(InventoryListQueryInfo queryInfo, Models.LanguageEnum languageVersion)
        {
            var page = new PageDTO() { PageIndex = 1, PageSize = int.MaxValue };
            var ds = productDAL.GetSkuInventoryList(queryInfo, languageVersion, page);
            return ds.Tables[0];
        }

        private ProductUpdateBindingModel ConvertProductUpdateBinding(Dictionary<LanguageEnum, ProductTempModel> ptms
                , Dictionary<LanguageEnum, ProductExtTempModel> ptExs
                , Dictionary<LanguageEnum, List<SkuTempModel>> skus
                , List<ProductImgModel> imgs)
        {
            ProductUpdateBindingModel result = new ProductUpdateBindingModel();

            foreach (var spuInfo in ptms)
            {
                var spuValue = spuInfo.Value;
                if (null == spuValue)
                {
                    continue;
                }

                switch (spuInfo.Key)
                {
                    case LanguageEnum.English:
                        result.ProductInfo_E = ReflectionKeyValue<ProductTempModel>(spuValue);
                        break;
                    case LanguageEnum.SimplifiedChinese:
                        result.ProductInfo_S = ReflectionKeyValue<ProductTempModel>(spuValue);
                        break;
                    case LanguageEnum.TraditionalChinese:
                        result.ProductInfo_T = ReflectionKeyValue<ProductTempModel>(spuValue);
                        break;
                }
            }

            foreach (var spuExInfo in ptExs)
            {
                var spuExInValue = spuExInfo.Value;

                if (null == spuExInValue)
                {
                    continue;
                }

                switch (spuExInfo.Key)
                {
                    case LanguageEnum.English:
                        result.ProductInfoExpand_E = ReflectionKeyValue<ProductExtTempModel>(spuExInValue);
                        break;
                    case LanguageEnum.SimplifiedChinese:
                        result.ProductInfoExpand_S = ReflectionKeyValue<ProductExtTempModel>(spuExInValue);
                        break;
                    case LanguageEnum.TraditionalChinese:
                        result.ProductInfoExpand_T = ReflectionKeyValue<ProductExtTempModel>(spuExInValue);
                        break;
                }
            }

            result.SkuInfo_E = new List<Dictionary<string, string>>();
            result.SkuInfo_S = new List<Dictionary<string, string>>();
            result.SkuInfo_T = new List<Dictionary<string, string>>();

            foreach (var skuInfos in skus)
            {
                if (null == skuInfos.Value)
                {
                    continue;
                }


                foreach (var sInfo in skuInfos.Value)
                {
                    if (null == sInfo)
                    {
                        continue;
                    }

                    switch (skuInfos.Key)
                    {
                        case LanguageEnum.English:
                            result.SkuInfo_E.Add(ReflectionKeyValue<SkuTempModel>(sInfo));
                            break;
                        case LanguageEnum.SimplifiedChinese:
                            result.SkuInfo_S.Add(ReflectionKeyValue<SkuTempModel>(sInfo));
                            break;
                        case LanguageEnum.TraditionalChinese:
                            result.SkuInfo_T.Add(ReflectionKeyValue<SkuTempModel>(sInfo));
                            break;
                    }
                }
            }

            result.Imgs = imgs;

            return result;
        }

        private static List<ProductTempImageModel> GetProductImgModels(ProductJsonModel pjModel, int userId)
        {
            var spuImages = new List<ProductTempImageModel>();

            if (pjModel.SPU.Images != null && pjModel.SPU.Images.Length > 0)
            {
                foreach (var img in pjModel.SPU.Images)
                {
                    var imgModel = new ProductTempImageModel()
                    {
                        SPU = pjModel.SPU.Spu,
                        ImagePath = img.Path,
                        ImageType = 1,
                        SortValue = img.SortId,
                        Createtime = DateTime.Now,
                        Createby = userId.ToString()
                    };

                    spuImages.Add(imgModel);
                }
            }
            return spuImages;
        }

        private Dictionary<LanguageEnum, List<SkuTempModel>> GetSkuModels(ProductJsonModel pjModel, bool isRelease, bool isOnline = false)
        {
            var skus = new Dictionary<LanguageEnum, List<SkuTempModel>>();

            var skuRs = new Dictionary<string, int>();

            if (isRelease == true && pjModel.SPU.SalesTerritory != 2)
            {
                skuRs = GetSkuCustomReportCount(pjModel.SKUS.Select(p => p.Sku).Distinct().ToList());
            }

            skus.Add(LanguageEnum.TraditionalChinese, ConvertSkuModel(pjModel, LanguageEnum.TraditionalChinese, skuRs, isRelease, isOnline));

            skus.Add(LanguageEnum.SimplifiedChinese, ConvertSkuModel(pjModel, LanguageEnum.SimplifiedChinese, skuRs, isRelease, isOnline));

            skus.Add(LanguageEnum.English, ConvertSkuModel(pjModel, LanguageEnum.English, skuRs, isRelease, isOnline));

            return skus;
        }

        private Dictionary<LanguageEnum, ProductExtTempModel> GetProductExtendModels(ProductJsonModel pjModel, Dictionary<LanguageEnum, ProductExtTempModel> ptmExs)
        {
            ptmExs = new Dictionary<LanguageEnum, ProductExtTempModel>();
            ptmExs.Add(LanguageEnum.TraditionalChinese, ConvertPTEModel(pjModel, LanguageEnum.TraditionalChinese));
            ptmExs.Add(LanguageEnum.SimplifiedChinese, ConvertPTEModel(pjModel, LanguageEnum.SimplifiedChinese));
            ptmExs.Add(LanguageEnum.English, ConvertPTEModel(pjModel, LanguageEnum.English));
            return ptmExs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pjModel"></param>
        /// <param name="supplierId"></param>
        /// <param name="userId"></param>
        /// <param name="isRelease"></param>
        /// <param name="isFromCreate">是否为新建或者新建商品的草稿</param>
        /// <param name="brandInfo"></param>
        /// <returns></returns>
        private Dictionary<LanguageEnum, ProductTempModel> GetProductInfoModels(ProductJsonModel pjModel, int supplierId, int userId, bool isRelease, bool isFromCreate, SupplierBrandModel brandInfo)
        {
            var ptms = new Dictionary<LanguageEnum, ProductTempModel>();

            ptms.Add(LanguageEnum.TraditionalChinese, ConvertPTModel(pjModel, supplierId, userId, isRelease, isFromCreate, LanguageEnum.TraditionalChinese));

            ptms.Add(LanguageEnum.SimplifiedChinese, ConvertPTModel(pjModel, supplierId, userId, isRelease, isFromCreate, LanguageEnum.SimplifiedChinese));

            ptms.Add(LanguageEnum.English, ConvertPTModel(pjModel, supplierId, userId, isRelease, isFromCreate, LanguageEnum.English));

            if (brandInfo != null)
            {

                ptms[LanguageEnum.TraditionalChinese].Brand = brandInfo.NameHK;

                ptms[LanguageEnum.SimplifiedChinese].Brand = brandInfo.NameCN;

                ptms[LanguageEnum.English].Brand = brandInfo.NameEN;
            }

            return ptms;
        }

        /// <summary>
        /// 获取即有可选项又有自定义的属性的值
        /// </summary>
        /// <param name="dicType"></param>
        /// <param name="keyName"></param>
        /// <param name="le"></param>
        /// <param name="tds"></param>
        /// <returns></returns>
        private string GetKeyValue(string dicType, string keyName, LanguageEnum le, TripleDes tds)
        {
            if (String.IsNullOrWhiteSpace(dicType))
            {
                return string.Empty;
            }

            var dicItems = CommonBLL.GetDicsInfoByKeyAllLanguage(dicType);

            var dicItem = new DicsModel();

            if (null != dicItems && dicItems.Count > 0)
            {
                foreach (var item in dicItems)
                {
                    if (item.LanguageVersion == (int)le && item.KeyName == keyName)
                    {
                        dicItem = item;
                        break;
                    }
                }
            }

            var keyValue = String.Empty;

            if (null != dicItem)
                keyValue = dicItem.KeyValue;

            if (null != tds)
            {
                var tempValue = GetTripleDes(le, tds);
                keyValue = String.IsNullOrWhiteSpace(tempValue) ? keyValue : tempValue;
            }
            return keyValue;

        }

        private string GetKeyName(string dicType, string keyName)
        {
            if (String.IsNullOrWhiteSpace(dicType))
            {
                return string.Empty;
            }


            if (!String.IsNullOrWhiteSpace(keyName))
            {
                return keyName;
            }

            //var dicItems = CommonBLL.GetDicsInfoByKeyAllLanguage(dicType);

            //var dicItem = new DicsModel();

            //if (null != dicItems && dicItems.Count > 0)
            //{
            //    foreach (var item in dicItems)
            //    {
            //        if (item.LanguageVersion == 2 && item.KeyValue.Contains("其它"))
            //        {
            //            dicItem = item;
            //            break;
            //        }
            //    }
            //}

            //var result = "200";

            //if (null != dicItem)
            //    result = dicItem.KeyName;

            var random = new Random();

            return random.Next(100, 999).ToString();

        }

        private string GetKeyValue(string dicType, KeyAndOther ko, LanguageEnum le)
        {

            if (String.IsNullOrWhiteSpace(dicType))
            {
                return string.Empty;
            }

            if (null == ko)
            {
                return string.Empty;
            }

            return GetKeyValue(dicType, ko.Key, le, ko.Other);
        }

        private string GetAttrUnitName(string dicType, UnitKey uk, LanguageEnum le)
        {
            if (String.IsNullOrWhiteSpace(dicType))
            {
                return string.Empty;
            }

            if (null == uk)
            {
                return string.Empty;
            }

            return GetKeyValue(dicType, uk.Key, le, null);
        }

        private ProductTempModel ConvertPTModel(ProductJsonModel pjModel, int supplierId, int userId, bool isRelease, bool isNewProduct, LanguageEnum le)
        {
            if (null == pjModel.SPU)
            {
                return null;
            }

            var spu = pjModel.SPU;


            var ptm = new ProductTempModel()
            {
                Spu = spu.Spu,
                CategoryId = spu.CategoryId,
                SupplierId = supplierId,
                Name = GetTripleDes(le, spu.Name),

                Price = Convert.ToDecimal(spu.Price),
                Description = GetTripleDes(le, spu.Description),

                BrandId= spu.BrandId,
                CountryOfManufacture = GetKeyValue("CountryOfManufacture", spu.CountryOfManufactureId, le, null),
                SalesTerritory = spu.SalesTerritory,
                Unit = GetKeyValue("Unit", spu.Unit, le),

                IsExchangeInCHINA = spu.IsExchangeInCHINA,
                IsExchangeInHK = spu.IsExchangeInHK,
                IsReturn = spu.IsReturn,

                MinForOrder = spu.MinForOrder,
                MinPrice = (pjModel.SKUS != null && pjModel.SKUS.Length > 0) ? Convert.ToDecimal(pjModel.SKUS.Min(p => p.Price)) : 0,
                NetContentUnit = GetKeyValue("NetContent", spu.NetContentUnitId, le, null),

                NetWeightUnit = GetKeyValue("NetWeight", spu.NetWeightUnitId, le, null),
                IsDutyOnSeller = spu.IsDutyOnSeller,
                LanguageVersion = (int)le,

                Createtime = DateTime.Now,
                CreateBy = userId.ToString(),
                Status = isRelease == true ? 0 : -2,

                ModifyTime = DateTime.Now,
                ModifyBy = userId.ToString(),

                PreOnSaleTime = spu.PreOnSaleTime,
                DataSource = isNewProduct == true ? 1 : 2,

                CommissionInCHINA = String.IsNullOrWhiteSpace(spu.CommissionInCHINA) ? 0 : Convert.ToDecimal(spu.CommissionInCHINA),
                CommissionInHK = String.IsNullOrWhiteSpace(spu.CommissionInHK) ? 0 : Convert.ToDecimal(spu.CommissionInHK)
            };

            StringBuilder tags = new StringBuilder(100);
            foreach (var tag in spu.Tag)
            {
                tags.Append(GetTripleDes(le, tag));
                tags.Append("$");
            }

            ptm.Tag = tags.Length > 0 ? tags.Remove(tags.Length - 1, 1).ToString() : "";

            return ptm;
        }

        private string GetTripleDes(LanguageEnum le, TripleDes tds)
        {
            if (null == tds)
                return string.Empty;

            var result = String.Empty;

            switch (le)
            {
                case LanguageEnum.English:
                    result = tds.ContentE;
                    break;
                case LanguageEnum.SimplifiedChinese:
                    result = tds.ContentS;
                    break;
                case LanguageEnum.TraditionalChinese:
                    result = tds.ContentT;
                    break;
            }

            return result;
        }

        private ProductExtTempModel ConvertPTEModel(ProductJsonModel pjModel, LanguageEnum le)
        {
            if (null == pjModel.SpuEx)
                return null;

            var spuEx = pjModel.SpuEx;

            List<string> supportLanguages = new List<string>();
            if (null != spuEx.SupportedLanguage)
            {
                var dicItems = CommonBLL.GetDicsInfoByKeyAllLanguage("SupportedLanguage");

                foreach (var id in spuEx.SupportedLanguage.Keys)
                {
                    foreach (var item in dicItems)
                    {
                        if (item.LanguageVersion == (int)le && item.KeyName == id.ToString())
                        {
                            supportLanguages.Add(item.KeyValue);
                        }
                    }
                }

                foreach (var other in spuEx.SupportedLanguage.Others)
                {
                    var o = GetTripleDes(le, other);

                    if (!String.IsNullOrWhiteSpace(o))
                    {
                        supportLanguages.Add(o);
                    }
                }
            }

            var ptem = new ProductExtTempModel()
            {
                Materials = GetKeyValue("Materials", spuEx.Materials, le),
                Pattern = GetTripleDes(le, spuEx.Pattern),
                Flavour = GetTripleDes(le, spuEx.Flavour),
                Flavor = GetTripleDes(le, spuEx.Flavor),
                Ingredients = GetTripleDes(le, spuEx.Ingredients),

                StoragePeriod = GetTripleDes(le, spuEx.StoragePeriod),
                StoringTemperature = GetTripleDes(le, spuEx.StoringTemperature),
                SkinType = GetTripleDes(le, spuEx.SkinType),

                Gender = GetKeyValue("Gender", spuEx.GenderId.ToString(), le, null),
                AgeGroup = GetTripleDes(le, spuEx.AgeGroup),
                Model = GetTripleDes(le, spuEx.Model),

                BatteryTime = GetTripleDes(le, spuEx.BatteryTime),
                Voltage = GetTripleDes(le, spuEx.Voltage),
                Power = GetTripleDes(le, spuEx.Power),

                Warranty = GetTripleDes(le, spuEx.Warranty),
                SupportedLanguage = supportLanguages.Count > 0 ? String.Join("$", supportLanguages) : String.Empty,

                PetType = GetKeyValue("PetType", spuEx.PetType, le),
                PetAgeUnit = GetAttrUnitName("PetAge", spuEx.PetAgeUnit, le),
                PetAge = GetTripleDes(le, spuEx.PetAge),

                Location = GetTripleDes(le, spuEx.Location),

                Weight = GetAttrDeciaml(spuEx.Weight, le),
                WeightUnit = GetAttrUnitName("Weight", spuEx.WeightUnit, le),

                Volume = GetAttrDeciaml(spuEx.Volume, le),
                VolumeUnit = GetAttrUnitName("Volume", spuEx.VolumeUnit, le),

                Length = GetAttrDeciaml(spuEx.Length, le),
                LengthUnit = GetAttrUnitName("Length", spuEx.LengthUnit, le),

                Width = GetAttrDeciaml(spuEx.Width, le),
                WidthUnit = GetAttrUnitName("Width", spuEx.WidthUnit, le),

                Height = GetAttrDeciaml(spuEx.Height, le),
                HeightUnit = GetAttrUnitName("Height", spuEx.HeightUnit, le),
            };

            return ptem;
        }

        private List<SkuTempModel> ConvertSkuModel(ProductJsonModel pjModel, LanguageEnum le, Dictionary<string, int> skuRs, bool isRelease, bool isOnline = false)
        {
            if (null == pjModel.SKUS || pjModel.SKUS.Length == 0)
            {
                return null;
            }

            List<SkuTempModel> skus = new List<SkuTempModel>();

            Dictionary<string, string> mainKeyDes = new Dictionary<string, string>();
            Dictionary<string, string> subKeyDes = new Dictionary<string, string>();

            foreach (var skuInfo in pjModel.SKUS)
            {
                var skuId = String.IsNullOrWhiteSpace(skuInfo.Sku) ? GetNewSkuId().ToString() : skuInfo.Sku;
                skuInfo.Sku = skuId;
                var hasSku = skuRs.Keys.Contains(skuId);

                var reportStatus = -1;//不报备

                if (pjModel.SPU.SalesTerritory != 2) //大陆或（大陆及香港）
                {
                    if (skuRs.Keys.Contains(skuId) && skuRs[skuId] > 0)
                    {
                        reportStatus = 1;
                    }
                    else
                    {
                        reportStatus = 0;
                    }

                    //if (isOnline == true && hasSku == true)//编辑线上数据并且有SKU
                    //{
                    //    reportStatus = 1;
                    //}
                    //else if (isOnline == true && hasSku == false)//编辑新商品或者编辑线上数据没有SKU
                    //{
                    //    reportStatus = 0;
                    //}
                    //else //编辑线下商品
                    //{
                    //    reportStatus = 0;
                    //}
                }

                if (skuInfo.MainValue != null && !String.IsNullOrWhiteSpace(skuInfo.MainValue.ContentS))
                {
                    if (mainKeyDes.Keys.Contains(skuInfo.MainValue.ToString()))
                    {
                        skuInfo.MainKey = mainKeyDes[skuInfo.MainValue.ToString()];
                    }
                }

                if (skuInfo.SubValue != null && !String.IsNullOrWhiteSpace(skuInfo.SubValue.ContentS))
                {
                    if (subKeyDes.Keys.Contains(skuInfo.SubValue.ToString()))
                    {
                        skuInfo.SubKey = subKeyDes[skuInfo.SubValue.ToString()];
                    }
                }

                var stModel = new SkuTempModel()
                {
                    Spu = pjModel.SPU.Spu,
                    Sku = skuId,

                    MainDicKey = GetAttrValue(skuInfo.MainDicKey),
                    MainDicValue = GetKeyValue("ProductAttributes", skuInfo.MainDicKey, le, null),

                    SubDicKey = GetAttrValue(skuInfo.SubDicKey),
                    SubDicValue = GetKeyValue("ProductAttributes", skuInfo.SubDicKey, le, null),

                    MainKey = GetKeyName(skuInfo.MainDicKey, skuInfo.MainKey),
                    MainValue = GetKeyValue(skuInfo.MainDicKey, skuInfo.MainKey, le, skuInfo.MainValue),

                    SubKey = GetKeyName(skuInfo.SubDicKey, skuInfo.SubKey),
                    SubValue = GetKeyValue(skuInfo.SubDicKey, skuInfo.SubKey, le, skuInfo.SubValue),

                    NetWeight = Convert.ToDecimal(skuInfo.NetWeight),
                    NetContent = Convert.ToDecimal(skuInfo.NetContent),

                    Specifications = GetTripleDes(le, skuInfo.Specifications),
                    AlcoholPercentage = GetTripleDes(le, skuInfo.AlcoholPercentage),
                    Smell = GetTripleDes(le, skuInfo.Smell),

                    Color = GetKeyValue("Color", skuInfo.Color, le),
                    Size = GetKeyValue("Size", skuInfo.Size, le),

                    CapacityRestriction = GetTripleDes(le, skuInfo.CapacityRestriction),
                    Price = Convert.ToDecimal(skuInfo.Price),

                    BarCode = GetAttrValue(skuInfo.BarCode),
                    AlarmStockQty = skuInfo.AlarmStockQty,

                    CreateTime = DateTime.Now,
                    IsOnSaled = true,
                    Status = isRelease == true ? 0 : -2,

                    ReportStatus = reportStatus,
                    DataSource = isOnline == false ? 1 : (hasSku == false ? 1 : 2)

                };

                if (skuInfo.MainValue != null && !String.IsNullOrWhiteSpace(skuInfo.MainValue.ContentS))
                {
                    if (!mainKeyDes.Keys.Contains(skuInfo.MainValue.ToString()))
                    {
                        mainKeyDes.Add(skuInfo.MainValue.ToString(), stModel.MainKey);
                    }
                }

                if (skuInfo.SubValue != null && !String.IsNullOrWhiteSpace(skuInfo.SubValue.ContentS))
                {
                    if (!subKeyDes.Keys.Contains(skuInfo.SubValue.ToString()))
                    {
                        subKeyDes.Add(skuInfo.SubValue.ToString(), stModel.SubKey);
                    }
                }


                if (String.IsNullOrWhiteSpace(skuInfo.MainKey))
                {
                    skuInfo.MainKey = stModel.MainKey;
                }

                if (String.IsNullOrWhiteSpace(skuInfo.SubKey))
                {
                    skuInfo.SubKey = stModel.SubKey;
                }

                if (stModel.MainDicKey == "Color")
                {
                    if (!string.IsNullOrEmpty(stModel.MainValue))
                    {
                        stModel.Color = stModel.MainValue;
                    }
                    else
                    {
                        stModel.MainValue = stModel.Color;
                    }
                }

                if (stModel.SubDicKey == "Color")
                {
                    if (!string.IsNullOrEmpty(stModel.SubValue))
                    {
                        stModel.Color = stModel.SubValue;
                    }
                    else
                    {
                        stModel.SubValue = stModel.Color;
                    }
                }

                if (stModel.MainDicKey == "Size")
                {
                    if (!string.IsNullOrEmpty(stModel.MainValue))
                    {
                        stModel.Size = stModel.MainValue;
                    }
                    else
                    {
                        stModel.MainValue = stModel.Size;
                    }

                }

                if (stModel.SubDicKey == "Size")
                {
                    if (!string.IsNullOrEmpty(stModel.SubValue))
                    {
                        stModel.Size = stModel.SubValue;
                    }
                    else
                    {
                        stModel.SubValue = stModel.Size;
                    }

                }

                if (stModel.MainDicKey == "AlcoholPercentage")
                {
                    if (!string.IsNullOrEmpty(stModel.MainValue))
                    {
                        stModel.AlcoholPercentage = stModel.MainValue;
                    }
                    else
                    {
                        stModel.MainValue = stModel.AlcoholPercentage;
                    }

                }

                if (stModel.SubDicKey == "AlcoholPercentage")
                {
                    if (!string.IsNullOrEmpty(stModel.SubValue))
                    {
                        stModel.AlcoholPercentage = stModel.SubValue;
                    }
                    else
                    {
                        stModel.SubValue = stModel.AlcoholPercentage;
                    }

                }

                if (stModel.MainDicKey == "NetWeight")
                {
                    stModel.NetWeight = Convert.ToDecimal(stModel.MainValue);
                }

                if (stModel.SubDicKey == "NetWeight")
                {
                    stModel.NetWeight = Convert.ToDecimal(stModel.SubValue);
                }

                if (stModel.MainDicKey == "NetContent")
                {
                    stModel.NetContent = Convert.ToDecimal(stModel.MainValue);
                }

                if (stModel.SubDicKey == "NetContent")
                {
                    stModel.NetContent = Convert.ToDecimal(stModel.SubValue);
                }


                if (stModel.MainDicKey == "Specifications")
                {
                    if (!string.IsNullOrEmpty(stModel.MainValue))
                    {
                        stModel.Specifications = stModel.MainValue;
                    }
                    else
                    {
                        stModel.MainValue = stModel.Specifications;
                    }

                }

                if (stModel.SubDicKey == "Specifications")
                {
                    if (!string.IsNullOrEmpty(stModel.SubValue))
                    {
                        stModel.Specifications = stModel.SubValue;
                    }
                    else
                    {
                        stModel.SubValue = stModel.Specifications;
                    }

                }

                if (stModel.MainDicKey == "Smell")
                {
                    if (!string.IsNullOrEmpty(stModel.MainValue))
                    {
                        stModel.Smell = stModel.MainValue;
                    }
                    else
                    {
                        stModel.MainValue = stModel.Smell;
                    }
                }

                if (stModel.SubDicKey == "Smell")
                {
                    if (!string.IsNullOrEmpty(stModel.SubValue))
                    {
                        stModel.Smell = stModel.SubValue;
                    }
                    else
                    {
                        stModel.SubValue = stModel.Smell;
                    }
                }

                if (stModel.MainDicKey == "CapacityRestriction")
                {
                    if (!string.IsNullOrEmpty(stModel.MainValue))
                    {
                        stModel.CapacityRestriction = stModel.MainValue;
                    }
                    else
                    {
                        stModel.MainValue = stModel.CapacityRestriction;
                    }
                }

                if (stModel.SubDicKey == "CapacityRestriction")
                {
                    if (!string.IsNullOrEmpty(stModel.SubValue))
                    {
                        stModel.CapacityRestriction = stModel.SubValue;
                    }
                    else
                    {
                        stModel.SubValue = stModel.CapacityRestriction;
                    }
                }

                skus.Add(stModel);
            }

            return skus;
        }

        private string GetAttrValue(string attr)
        {
            return String.IsNullOrWhiteSpace(attr) ? String.Empty : attr;
        }

        private decimal GetAttrDeciaml(TripleDes tds, LanguageEnum le)
        {
            if (null == tds)
                return 0;

            decimal tempDecimal = 0;
            Decimal.TryParse(GetTripleDes(le, tds), out tempDecimal);
            return tempDecimal;
        }

        private T ReflectionCopy<T>(T source, T target) where T : new()
        {
            var t = new T();
            Type type = t.GetType();

            foreach (var property in type.GetProperties())
            {
                var sourcev = property.GetValue(source, null);
                property.SetValue(target, sourcev);
            }
            return target;
        }

        private Dictionary<string, string> ReflectionKeyValue<T>(T source)
        {
            Type type = source.GetType();

            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var property in type.GetProperties())
            {
                var sourcev = property.GetValue(source, null);
                if (property.PropertyType == typeof(DateTime))
                {
                    if (sourcev != null)
                    {
                        result.Add(property.Name, ((DateTime)sourcev).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        result.Add(property.Name, "");
                    }

                }
                else
                {
                    result.Add(property.Name, sourcev == null ? "" : sourcev.ToString());
                }
            }
            return result;
        }


        public bool IsBarCodeRepeat(string barcode, string spu)
        {
            return productDAL.IsBarCodeRepeat(barcode, spu);
        }

        public bool UpdateCategoryHistory(int supplierId, int categoryId)
        {
            return productDAL.UpdateCategoryHistory(supplierId, categoryId);
        }

        public Dictionary<string, int> GetSkuCustomReportCount(List<string> skus)
        {
            return productDAL.GetSkuCustomReportCount(skus);
        }

        public void SaveProductJson(string spu, string productJson, string isRelese, string action)
        {
            productDAL.SaveProductJson(spu, productJson, isRelese, action);
        }

        public bool CheckBrandStatus(string sku)
        {
            return productDAL.CheckBrandStatus(sku);
        }
    }
}
