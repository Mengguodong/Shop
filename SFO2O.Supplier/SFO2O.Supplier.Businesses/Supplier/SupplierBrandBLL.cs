using SFO2O.Supplier.DAO;
using SFO2O.Supplier.Models;
using SFO2O.Supplier.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Businesses.Supplier
{
    public class SupplierBrandBLL
    {
        private readonly SupplierBrandDAL dal = new SupplierBrandDAL();

        public IList<ProductBrandModel> GetBrandBySupplierId(int supplierId, Models.LanguageEnum languageEnum)
        {
            return dal.GetBrandBySupplierId(supplierId, languageEnum);
        }

        public PageOf<SupplierBrandModel> GetBrandBySupplierId(int supplierId, Models.PageDTO page)
        {
            return dal.GetBrandBySupplierId(supplierId, page);
        }

        public SupplierBrandModel GetBrandById(int brandId)
        {
            return dal.GetBrandById(brandId);
        }

        public bool UpdateById(SupplierBrandModel brandinfo)
        {
            return dal.UpdateById(brandinfo);
        }

        public bool Add(SupplierBrandModel brandinfo)
        {
            return dal.Add(brandinfo);
        }

        public PageOf<StoreModel> GetStoreListByBrandId(int brandId, int areaId, Models.LanguageEnum languageEnum, PageDTO page)
        {
            return dal.GetStoreListByBrandId(brandId, areaId, languageEnum, page);
        }

        public bool UpdateStoreById(Models.StoreModel store)
        {
            return dal.UpdateStoreById(store);
        }

        public bool AddStore(Models.StoreModel store)
        {
            return dal.AddStore(store);
        }

        public Models.StoreModel GetStoreById(int id)
        {
            return dal.GetStoreById(id);
        }

        public bool DeleteAddress(int id)
        {
            return dal.DeleteAddress(id);
        }

        public bool ShelfOffBrand(string brandId)
        {
            return dal.ShelfOffBrand(brandId);
        }
    }
}
