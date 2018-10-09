using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.DAL.Promotion;
using SFO2O.DAL.SupplierBrand;
using SFO2O.Model.Promotion;
using SFO2O.Model.Supplier;
using SFO2O.Utility.Uitl;

namespace SFO2O.BLL.Item
{
    public class ItemBll
    {
        PromotionDal promotionDal = new PromotionDal();
        SupplierBrandDal brandDal = new SupplierBrandDal();

        /// <summary>
        /// 获取品牌信息
        /// </summary>
        /// <param name="top"></param>
        /// <param name="brandId"></param>
        /// <param name="supplierId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IList<BrandEntity> GetSupplierBrandEntities(int top, int brandId, string supplierId, int categoryId)
        {
            try
            {
                var list = brandDal.GeSupplierBrandEntities(top, brandId, supplierId, categoryId);
                foreach (BrandEntity item in list)
                {
                    if (!string.IsNullOrEmpty(item.NationalFlag))
                    {
                        item.NationalFlag = item.NationalFlag + PathHelper.NationalFlagImageExtension;
                    }
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
        /// 获取促销信息
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        public IList<PromotionEntity> GetPromotionEntities(string[] skus)
        {
            try
            {
                return promotionDal.GetAvaliablePromotionEntities(skus);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取促销信息
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        public IList<PromotionEntity> GetPromotionEntitiesTeam(int proid)
        {
            try
            {
                return promotionDal.GetAvaliablePromotionEntitiesTeam(proid);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取促销信息
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        public IList<PromotionEntity> GetPromotionInfoByPid(int pid)
        {
            try
            {
                return promotionDal.GetPromotionInfoByPid(pid);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }
        public IList<PromotionEntity> GetPromotionInfoBySku(string sku)
        {
            try
            {
                return promotionDal.GetPromotionInfoBySku(sku);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询promotions 里面的拼生活的spu
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        public IList<PromotionEntity> GetPromotionSPU(int language)
        {
            try
            {
                return promotionDal.GetPromotionSPU(language);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }
    }
}

