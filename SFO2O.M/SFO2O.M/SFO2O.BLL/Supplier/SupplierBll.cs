using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFO2O.DAL.Supplier;
using SFO2O.Model.Supplier;
using SFO2O.M.ViewModel.Supplier;
using SFO2O.Utility.Uitl;

namespace SFO2O.BLL.Supplier
{
    public class SupplierBll
    {
        SupplierDal supplierDal = new SupplierDal();

        /// <summary>
        /// 获取店铺头信息
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public StoreInfoModel GetStoreInfoById(int supplierId, int language)
        {
            StoreInfoModel storeInfoModel = new StoreInfoModel();
            var model = supplierDal.GetStoreInfoById(supplierId);
            if (model != null)
            {
                storeInfoModel.SupplierID = model.SupplierID;
                storeInfoModel.StorePageDesc = model.StorePageDesc;
                storeInfoModel.Address = model.Address;
                storeInfoModel.Address_English = model.Address_English;
                storeInfoModel.Address_Sample = model.Address_Sample;
                if (!string.IsNullOrEmpty(model.StoreName))
                {
                    storeInfoModel.StoreName = model.StoreName;
                }
                else
                {
                    if (language == 1)
                    {
                        storeInfoModel.StoreName = model.CompanyName_Sample;
                        storeInfoModel.StorePageDesc = model.Description_Sample;
                    }
                    else
                    {
                        storeInfoModel.StorePageDesc = model.Description;
                        storeInfoModel.StoreName = model.CompanyName;
                    }

                }
                storeInfoModel.StoreLogoPath = DomainHelper.GetImageUrl(model.StoreLogoPath);
                storeInfoModel.StoreBannerPath = DomainHelper.GetImageUrl(model.StoreBannerPath);
            }

            return storeInfoModel;
        }

        /// <summary>
        /// 根据用户Id查询收货地址
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public StoreIntroModel GetDefaultBySupplierId(int supplierId, int language)
        {
            StoreIntroModel storeIntroModel = new StoreIntroModel();
            var model = GetStoreInfoById(supplierId, language);
            storeIntroModel.StoreName = model.StoreName;
            storeIntroModel.StoreIntro = model.StorePageDesc;
            switch (language)
            {
                case 1:
                    storeIntroModel.StoreAddress.Add(model.Address_Sample);
                    break;
                case 2:
                    storeIntroModel.StoreAddress.Add(model.Address_English);
                    break;
                default:
                    storeIntroModel.StoreAddress.Add(model.Address_Sample);
                    break;
            }

            return storeIntroModel;
        }

        /// <summary>
        /// 设置地址
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string SetAddress(SupplierAdddressEntity entity)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(entity.ProvinceName);
                if (!entity.CityName.Contains("市辖区"))
                    sb.Append(entity.CityName);
                sb.Append(entity.AreaName);
                sb.Append(entity.Address);
                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }
    }
}
