using SFO2O.Admin.Common;
using SFO2O.Admin.DAO.Supplier;
using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Supplier;
using SFO2O.Admin.ViewModel;
using SFO2O.Admin.ViewModel.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Businesses.Supplier
{
    public class SupplierBLL
    {
        private readonly SupplierInfoDAL supplierInfoDAL = new SupplierInfoDAL();

        public Dictionary<int, string> GetSupplierNames()
        {
            return supplierInfoDAL.GetSupplierNames();
        }

        public PageOf<SupplierAbstractModel> GetSupplierList(SupplierQueryModel query, int pageSize, int pageNo)
        {
            return supplierInfoDAL.GetSupplierList(query, pageSize, pageNo);
        }
        /// <summary>
        /// 根据订单号获取商家信息
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public SupplierAbstractModel GetSupplierInfo(int supplierId)
        {
            return supplierInfoDAL.GetSupplierInfo(supplierId);
        }

        public SupplierDetailModel GetSupplierDetailInfo(int supplierId)
        {
            return supplierInfoDAL.GetSupplierDetailInfo(supplierId);
        }

        public void InsertSupplierInfo(SupplierInfoJsonModel supplierInfo, string userName)
        {
            supplierInfo.PassWord = MD5Hash.GetMd5String(supplierInfo.PassWord);

            supplierInfoDAL.InsertSupplierInfo(supplierInfo, userName);
        }

        public void UpdateSupplierInfo(SupplierInfoJsonModel supplierInfo, string userName)
        {
            if (!String.IsNullOrWhiteSpace(supplierInfo.PassWord))
            {
                supplierInfo.PassWord = MD5Hash.GetMd5String(supplierInfo.PassWord);
            }

            supplierInfoDAL.UpdateSupplierInfo(supplierInfo, userName);
        }

        public int GetUserNameCount(string userName)
        {
            return supplierInfoDAL.GetUserNameCount(userName);
        }

        public int GetCompanyNameCount(string companyName)
        {
            return supplierInfoDAL.GetCompaynNameCount(companyName);
        }

        public void UpdateSupplierStatus(int supplierId, int status, string userName)
        {
            supplierInfoDAL.UpdateSupplierStatus(supplierId, status, userName);
        }

        public BrandModel GetBrandById(int brandId)
        {
            return supplierInfoDAL.GetBrandById(brandId);
        }

        public PageOf<Models.StoreModel> GetStoreListByBrandId(int brandId, int areaId, Models.Enums.LanguageEnum languageEnum, int pageSize, int pageIndex)
        {
            return supplierInfoDAL.GetStoreListByBrandId(brandId, areaId, languageEnum, pageSize, pageIndex);
        }
    }
}
