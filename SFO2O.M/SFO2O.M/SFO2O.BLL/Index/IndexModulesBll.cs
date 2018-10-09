using System;
using System.Collections.Generic;
using System.Linq;
using SFO2O.BLL.Common;
using SFO2O.DAL.Index;
using SFO2O.Model;
using SFO2O.Model.Enum;
using SFO2O.Model.Index;
using SFO2O.Utility.Cache;
using SFO2O.Utility.Extensions;
using SFO2O.Utility.Uitl;
using System.Collections;
using SFO2O.Model.CMS;

namespace SFO2O.BLL.Index
{
    public class IndexModulesBll
    {
        public IndexModulesDal IndexDal = new IndexModulesDal();
        public BulletinDal BullDal = new BulletinDal();
        /// <summary>
        /// 根据类型获取模块数据信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public IList<IndexModulesEntity> GetIndexModules(IndexModuleType type, int top)
        {
            return IndexDal.GetIndexModules(type, top);

        }
        /// <summary>
        /// 获取首页新品区数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public IList<IndexModulesEntity> GetIndexNewProduct(int top, int language, int SalesTerritory)
        {
            try
            {
                var modules = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, ConstClass.RedisKeyIndexNewProduct, () =>
                {
                    return IndexDal.GetIndexNewProduct(top, language, SalesTerritory);
                }, 60);

                return modules;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return new List<IndexModulesEntity>();
        }
        /// <summary>
        /// 从缓存中获取数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public IList<IndexModulesEntity> GetIndexModulesFromCache(IndexModuleType type, int top)
        {
            try
            {
                var modules = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, ConstClass.RedisKeyIndexModules, () =>
                    {
                        var data = GetAllIndexModules();
                        return data;
                    }, 60);
                return GetIndexModules(type, top, modules);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                var modules = GetAllIndexModules();
                return GetIndexModules(type, top, modules);
            }
        }


        /// <summary>
        /// 获取所有模块数据信息
        /// </summary>
        /// <returns></returns>
        public IList<IndexModulesEntity> GetAllIndexModules()
        {
            return IndexDal.GetAllAvaliableIndexModules();
        }
        /// <summary>
        /// 获取公告信息
        /// </summary>
        /// <param name="top">0为获取全部</param>
        /// <returns></returns>
        public IList<BulletinEntity> GetBulletinEntities(int top = 0)
        {
            return BullDal.GetBulletinEntities(top);

        }

        private IList<IndexModulesEntity> GetIndexModules(IndexModuleType type, int top, IList<IndexModulesEntity> modules)
        {
            if (modules == null || modules.Count == 0)
            {
                return new List<IndexModulesEntity>();
            }
            return modules.Where(n => n.Key == type.As(0)).Take(top).ToList();
        }

        /// <summary>
        /// 随机获得热搜词
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public IList<HotKeyEntity> GetHotKeyForRandom(int top)
        {
            return IndexDal.GetHotKeyRandomModules(top);
        }

        /// <summary>
        /// 从缓存中获得首页轮播焦点图
        /// </summary>
        /// <param name="type"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public IList<BannerImagesEntity> GetIndexBannerImagesFromCache()
        {
            try
            {
                var modules = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, ConstClass.RedisKeyIndexBannerImagesModules, () =>
                {
                    var data = GetIndexBannerImages();
                    return data;
                }, 60);
                return modules;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                var modules = GetIndexBannerImages();
                return modules;
            }
        }

        /// <summary>
        /// 获得首页轮播焦点图
        /// </summary>
        /// <returns></returns>
        public IList<BannerImagesEntity> GetIndexBannerImages()
        {
            return IndexDal.GetIndexBannerImages();
        }

        /// <summary>
        /// 获取首页自定义模块数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public IList<IndexModulesEntity> GetIndexCustom(int language, int SalesTerritory)
        {
            try
            {
                var modules = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, ConstClass.RedisKeyIndexCMSCustom, () =>
                {
                    return IndexDal.GetIndexCustom(language, SalesTerritory);
                }, 60);

                return modules;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return new List<IndexModulesEntity>();
        }

        #region 获取热门商品模块数据 V2.0
        public IList<IndexModulesProductEntity> GetAllCMSHotProductsFromCache(int language, int salesTerritory)
        {
            try
            {
                var products = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, ConstClass.RedisKeyIndexCMSHotProducts, () =>
                {
                    var data = IndexDal.GetAllCMSHotProducts(language, salesTerritory);
                    return data;
                }, 60);

                return products;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                return new List<IndexModulesProductEntity>();
            }
        }
        #endregion

        /// <summary>
        /// 构建自定义模块Module
        /// </summary>
        /// <param name="AllList"></param>
        /// <param name="DataMap"></param>
        public void BuildCMSCustomModule(IList<IndexModulesEntity> AllList, Hashtable DataMap)
        {
            // 自定义模块 获得Module数据
            var CustomModuleList = AllList.Where(item => item.Numid == 1).ToList();
            if (CustomModuleList != null && CustomModuleList.Count() != 0)
            {
                IList<CMSCustomModuleEntity> CustomModuleEntityList = new List<CMSCustomModuleEntity>();
                foreach (IndexModulesEntity item in CustomModuleList)
                {
                    CMSCustomModuleEntity CustomModule = new CMSCustomModuleEntity();
                    CustomModule.ModuleId = item.ModuleId;
                    CustomModule.Name = item.ModuleName;
                    CustomModule.MSubTitle = item.MSubTitle;
                    CustomModule.CmSortValue = item.CmSortValue;
                    CustomModuleEntityList.Add(CustomModule);
                }

                DataMap.Add(ConfigHelper.GetAppConfigString("CustomModule"), CustomModuleEntityList);
            }
            else
            {
                DataMap.Add(ConfigHelper.GetAppConfigString("CustomModule"), null);
            }
        }

        /// <summary>
        /// 构建自定义模块Banner
        /// </summary>
        /// <param name="AllList"></param>
        /// <param name="DataMap"></param>
        public void BuildCMSCustomBanner(IList<IndexModulesEntity> AllList, Hashtable DataMap)
        {
            // 自定义模块 获得Banner数据
            var CustomBannerList = AllList.Where(item => item.BannerNumid == 1).ToList();
            if (CustomBannerList != null && CustomBannerList.Count() != 0)
            {
                IList<CMSCustomBannerEntity> CustomBannerEntityList = new List<CMSCustomBannerEntity>();
                foreach (IndexModulesEntity item in CustomBannerList)
                {
                    CMSCustomBannerEntity CMSCustomBannerEntity = new CMSCustomBannerEntity();
                    CMSCustomBannerEntity.BannerId = item.BannerId;
                    CMSCustomBannerEntity.CbModuleId = item.CbModuleId;
                    CMSCustomBannerEntity.Title = item.CbTitle;
                    CMSCustomBannerEntity.ImageUrl = item.ImageUrl;
                    CMSCustomBannerEntity.CbSortValue = item.CbSortValue;
                    CMSCustomBannerEntity.CbDescription = item.CbDescription;
                    CMSCustomBannerEntity.LinkUrl = item.CbLinkUrl;
                    CustomBannerEntityList.Add(CMSCustomBannerEntity);
                }

                DataMap.Add(ConfigHelper.GetAppConfigString("CustomBanner"), CustomBannerEntityList);
            }
            else
            {
                DataMap.Add(ConfigHelper.GetAppConfigString("CustomBanner"), null);
            }
        }

        /// <summary>
        /// 构建自定义模块Product
        /// </summary>
        /// <param name="AllList"></param>
        /// <param name="DataMap"></param>
        public void BuildCMSCustomProduct(IList<IndexModulesEntity> AllList, Hashtable DataMap)
        {
            // 自定义模块 获得商品数据
            var CustomProductList = AllList.Where(item => item.CpBannerId != 0).ToList();
            if (CustomProductList != null && CustomProductList.Count() != 0)
            {
                IList<CMSCustomProductEntity> CustomProductEntityList = new List<CMSCustomProductEntity>();
                foreach (IndexModulesEntity item in CustomProductList)
                {
                    CMSCustomProductEntity CMSCustomProductEntity = new CMSCustomProductEntity();
                    CMSCustomProductEntity.CpProductId = item.CpProductId;
                    CMSCustomProductEntity.ModuleId = item.ModuleId;
                    CMSCustomProductEntity.CpBannerId = item.CpBannerId;
                    CMSCustomProductEntity.Spu = item.Spu;
                    CMSCustomProductEntity.CpSortValue = item.CpSortValue;
                    CMSCustomProductEntity.Title = item.Title;
                    CMSCustomProductEntity.Unit = item.Unit;
                    CMSCustomProductEntity.MinPrice = item.MinPrice;
                    CMSCustomProductEntity.MinForOrder = item.MinForOrder;
                    CMSCustomProductEntity.ImagePath = item.ImagePath;
                    CMSCustomProductEntity.DiscountPrice = item.DiscountPrice;
                    CMSCustomProductEntity.DiscountRate = item.DiscountRate;
                    CMSCustomProductEntity.Qty = item.ForOrderQty;
                    CustomProductEntityList.Add(CMSCustomProductEntity);
                }

                DataMap.Add(ConfigHelper.GetAppConfigString("CustomProduct"), CustomProductEntityList);
            }
            else
            {
                DataMap.Add(ConfigHelper.GetAppConfigString("CustomProduct"), null);
            }
        }
    }
}
