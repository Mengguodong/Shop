using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SFO2O.Supplier.Businesses.Supplier;
using SFO2O.Supplier.Models;
using SFO2O.Supplier.Common;
using SFO2O.Supplier.Businesses.Category;
using SFO2O.Supplier.Businesses;

namespace SFO2O.Supplier.Web.Controllers
{
    public class BrandController : BaseController
    {
        // GET: /Brand/
        public ActionResult BrandList()
        {
            try
            {
                SupplierBrandBLL supplierBLL = new SupplierBrandBLL();

                var page = new PageDTO() { PageIndex = PageNo, PageSize = DefaultPageSize };

                var result = supplierBLL.GetBrandBySupplierId(CurrentUser.SupplierID, page);

                return View(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View();
        }

        public ActionResult BrandEdit(int brandId = 0)
        {
            try
            {
                SupplierBrandModel model = new SupplierBrandModel();
                SupplierBrandBLL supplierBLL = new SupplierBrandBLL();
                if (brandId > 0)
                {
                    ViewBag.Operation = "edit";
                    model = supplierBLL.GetBrandById(brandId);
                }
                else
                {
                    ViewBag.Operation = "add";
                }
                ViewBag.FirstCategory = CategoryBLL.GetFristLevelCategories(LanguageEnum.TraditionalChinese);
                ViewBag.Country = CommonBLL.GetDicsInfoByKey("CountryOfManufacture");
                ViewBag.Brand = model;
                return View(model);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View();
        }

        public ActionResult BrandView(int brandId = 0)
        {
            try
            {
                SupplierBrandModel model = new SupplierBrandModel();
                SupplierBrandBLL supplierBLL = new SupplierBrandBLL();

                model = supplierBLL.GetBrandById(brandId);
                var page = new PageDTO() { PageIndex = PageNo, PageSize = int.MaxValue };
                PageOf<StoreModel> storeList = supplierBLL.GetStoreListByBrandId(brandId, 0, LanguageEnum.TraditionalChinese, page);

                ViewBag.Brand = model;
                ViewBag.StoreList = storeList;
                return View();

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View();
        }

        [ValidateInput(false)]
        public JsonResult SaveBrand(string brand)
        {
            try
            {
                var result = false;
                SupplierBrandBLL brandBLL = new SupplierBrandBLL();

                SupplierBrandModel brandinfo = JsonHelper.ToObject<SupplierBrandModel>(brand);
                brandinfo.SupplierId = CurrentUser.SupplierID;

                if (brandinfo.Id > 0)
                {
                    result = brandBLL.UpdateById(brandinfo);
                }
                else
                {
                    result = brandBLL.Add(brandinfo);
                }
                return Json(new { result = result, msg = "" });

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { result = false, msg = "" });
            }
        }

        public JsonResult ShelfOffBrand(string brandId)
        {
            try
            {
                var result = false;
                SupplierBrandBLL brandBLL = new SupplierBrandBLL();


                result = brandBLL.ShelfOffBrand(brandId);
               
                return Json(new { result = result, msg = "" });

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { result = false, msg = "" });
            }
        }

        // GET: /Brand/
        public ActionResult StoreList(int brandId = 0, int areaId = 0)
        {
            try
            {
                SupplierBrandBLL supplierBLL = new SupplierBrandBLL();

                var page = new PageDTO() { PageIndex = PageNo, PageSize = DefaultPageSize };

                var result = supplierBLL.GetStoreListByBrandId(brandId, areaId, LanguageEnum.TraditionalChinese, page);

                ViewBag.Brand = supplierBLL.GetBrandById(brandId);

                ViewBag.ProvinceList = CountryBLL.GetProvinceList(852, LanguageEnum.TraditionalChinese);

                ViewBag.AreaId = areaId;

                return View(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View();
        }


        public ActionResult AddStore(int brandId = 0, int id = 0)
        {
            try
            {
                SupplierBrandBLL supplierBLL = new SupplierBrandBLL();

                StoreModel model = supplierBLL.GetStoreById(id);

                ViewBag.Brand = supplierBLL.GetBrandById(brandId);

                ViewBag.ProvinceList = CountryBLL.GetProvinceList(852, LanguageEnum.TraditionalChinese);

                ViewBag.Store = model;

                return View();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View();
        }

        public ActionResult SaveAddress(string address)
        {
            try
            {
                var result = false;
                SupplierBrandBLL brandBLL = new SupplierBrandBLL();

                StoreModel store = JsonHelper.ToObject<StoreModel>(address);
                if (store.Id > 0)
                {
                    result = brandBLL.UpdateStoreById(store);
                }
                else
                {
                    result = brandBLL.AddStore(store);
                }
                return Json(new { result = result, msg = "" });

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { result = false, msg = "" });
            }
        }

        public JsonResult DeleteAddress(int id)
        {
            try
            {
                SupplierBrandBLL supplierBLL = new SupplierBrandBLL();
                bool result = supplierBLL.DeleteAddress(id);

                return Json(new { result = result, msg = "" });

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { result = false, msg = "删除失败" });
            }
        }

    }
}