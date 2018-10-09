using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.DAL.Supplier;
using SFO2O.Model.Supplier;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Product;
using SFO2O.BLL.Common;
using SFO2O.Utility.Cache;
namespace SFO2O.BLL.Supplier
{
    public class BrandBll
    {
        



        private readonly BrandDal brand = new BrandDal();



        public BrandEntity GetBrandInfoFromAutoCache(int brandId, int language, int deliveryRegion, decimal exchangeRate, int userId, int SourceType)
        {

            try
            {
                var modules = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, ConstClass.RedisKeyBrandEntity + SourceType, () =>
                {
                    var data = GetBrandEntity(brandId, language, deliveryRegion, exchangeRate, userId);
                    return data;
                }, 1440);//缓存24小时
                return modules;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                var modules = GetBrandEntity(brandId, language, deliveryRegion, exchangeRate, userId);
                return modules;
            }
        }
        public BrandEntity GetBrandEntity(int brandId, int language, int deliveryRegion, decimal exchangeRate, int userId)
        {
            try
            {
                var brandInfo = GetBrandInfo(brandId, language);
                //明星商品
                brandInfo.productInfoList = getProductList(userId, exchangeRate, userId);
                return brandInfo;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new BrandEntity() { productInfoList = new List<ProductInfoModel>() };
            }
        }

        
        /// <summary>
        /// 品牌信息
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public BrandEntity GetBrandInfo(int brandId, int language)
        {
            try
            {
                BrandEntity item = brand.GetBrandInfo(brandId, language);
                if (!string.IsNullOrEmpty(item.NationalFlag))
                {
                    item.NationalFlag = item.NationalFlag + PathHelper.NationalFlagImageExtension;
                }
                return item;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return new BrandEntity();
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
            try
            {
                return brand.GetBrandSaleSpuCount(brandId, language, deliveryRegion);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return 0;
        }
        /// <summary>
        /// 取品牌的门店列表
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public IList<StoreEntity> GetStoreListByBrandId(int brandId, int language)
        {
            try
            {
                return brand.GetStoreListByBrandId(brandId, language).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return new List<StoreEntity>();
        }
        /// <summary>
        /// 品牌信息
        /// </summary>
        /// <param name="categoryId">一级类目</param>
        /// <returns></returns>
        public IList<BrandInfo> GetBrandListByCategoryId(string categoryId, int level, int language, int salesTerritory)
        {
            try
            {
                return brand.GetBrandListByCategoryId(categoryId,level,language,salesTerritory).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return new List<BrandInfo>();
        }
        /// <summary>
        /// 品牌街
        /// </summary>
        public List<BrandEntity> getBrandStreetList(int PageIndex, int pagesize)
        {
            try
            {
                return brand.getBrandStreetList(PageIndex, pagesize).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return new List<BrandEntity>();
        }
        /// <summary>
        /// 明星产品
        /// </summary>
        public List<ProductInfoModel> getProductList(int id, decimal ExchangeRate,int userId)
        {
            try
            {
                return brand.getProductList(id, ExchangeRate, userId).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return new List<ProductInfoModel>();
        }
    }
}
