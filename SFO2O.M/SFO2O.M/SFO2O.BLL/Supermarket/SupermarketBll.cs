using SFO2O.BLL.Common;
using SFO2O.DAL.Index;
using SFO2O.DAL.Product;
using SFO2O.DAL.Supermarket;
using SFO2O.Model.Index;
using SFO2O.Model.Product;
using SFO2O.Model.Supermarket;
using SFO2O.Utility.Cache;
using SFO2O.Utility.Uitl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.BLL.Supermarket
{
    public class SupermarketBll
    {
        public IndexModulesDal IndexDal = new IndexModulesDal();
        public SupermarketDal SupermarketDal = new SupermarketDal();
        public ProductDal dal = new ProductDal();
        private int minutes = 60;
        /// <summary>

        /// <summary>
        /// 从缓存中获得首页轮播焦点图
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public IList<BannerImagesEntity> GetIndexBannerImagesFromCache(int channelId)
        {
            try
            {
                var modules = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, ConstClass.Key_SupermarketBannerImages, () =>
                {
                    var data = GetIndexBannerImages(channelId);
                    return data;
                }, minutes);
                return modules;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                var modules = GetIndexBannerImages(channelId);
                return modules;
            }
        }

        public IList<BannerImagesEntity> GetIndexBannerImages(int channelId)
        {
            return IndexDal.GetIndexBannerImages(channelId);
        }
        public IList<BerserkProductEntity> GetBerserkDay(int top, int language)
        {
            try
            {
                var berserkProduct = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, ConstClass.Key_SupermarketBerserkProduct, () =>
                {
                    return SupermarketDal.GetBerserkProduct(top, language);
                }, minutes);

                return berserkProduct;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return new List<BerserkProductEntity>();
        }

        /// <summary>
        /// 健康绿氧超市页面单banner图
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public SingleBannerImagesEntity GetIndexSingleBannerImagesFromCache(int channelId)
        {
            try
            {
                var modules = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, ConstClass.Key_SupermarketSingleBannerImage, () =>
                {
                    var data = GetIndexSingleBannerImage(channelId);
                    return data;
                }, minutes);
                return modules;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                var modules = GetIndexSingleBannerImage(channelId);
                return modules;
            }
        }

        public List<ProductInfoModel> GetMarketProductListNew(string[] categoryId, int level, List<int> brandIds, int sort, int pageindex, int pagesize, int language, int deliveryRegion, decimal exchangeRate, out int totalRecords)
        {
            List<ProductInfoModel> list = new List<ProductInfoModel>();
            totalRecords = 0;
            try
            {
                string cacheName = ConstClass.RedisKey4MPrefix + "_ProductList_" + categoryId + "_" + sort + "_" + pageindex;
                // list = RedisCacheHelper.Get<List<ProductInfoModel>>(cacheName);
                if (list == null || list.Count == 0)
                {

                    list = SupermarketDal.GetMarketProductListNew(categoryId, level, brandIds, sort, pageindex, pagesize, language, deliveryRegion, out totalRecords);
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

        private SingleBannerImagesEntity GetIndexSingleBannerImage(int channelId)
        {
            IList<SingleBannerImagesEntity> list = SupermarketDal.GetIndexSingleBannerImage(channelId);
            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return new SingleBannerImagesEntity();
            }
        }

    }
}
