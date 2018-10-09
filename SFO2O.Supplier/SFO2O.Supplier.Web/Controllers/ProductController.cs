using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SFO2O.Supplier.ViewModels;
using SFO2O.Supplier.Businesses.Account;
using SFO2O.Supplier.Common;
using SFO2O.Supplier.Businesses.Category;
using SFO2O.Supplier.Models;
using SFO2O.Supplier.Models.Category;
using SFO2O.Supplier.Models.Product;
using SFO2O.Supplier.ViewModels.Product;
using SFO2O.Supplier.Businesses.Supplier;

namespace SFO2O.Supplier.Web.Controllers
{
    public class ProductController : BaseController
    {
        ProductInfoBLL bll = new ProductInfoBLL();

        #region 待上传商品列表

        public ActionResult ProductList()
        {
            ProductInfoBLL bll = new ProductInfoBLL();

            var page = new PageDTO() { PageIndex = PageNo, PageSize = DefaultPageSize };

            var result = bll.GetProductTemps(CurrentUser.SupplierID, LanguageEnum.TraditionalChinese, page);

            return View(result);
        }

        /// <summary>
        /// 删除商品草稿
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ActionResult DeleteProduct(int productId)
        {
            string message = string.Empty;
            bool result = false;
            if (productId > 0)
            {
                result = bll.DeleteProductById(productId, CurrentUser.UserID);
                message = "删除成功";
            }
            else
            {
                result = false;
                message = "删除失败";
            }

            return new JsonResult()
            {
                Data = new
                {
                    result = result,
                    message = message
                }
            };
        }

        public ActionResult DeleteAllProduct(string productIds)
        {
            string message = string.Empty;
            bool result = false;
            if (!string.IsNullOrEmpty(productIds))
            {
                result = bll.DeleteAllProduct(productIds, CurrentUser.UserID);
                message = "删除成功";
            }
            else
            {
                result = false;
                message = "请勾选删除的商品";
            }

            return new JsonResult()
            {
                Data = new
                {
                    result = result,
                    message = message
                }
            };
        }

        #endregion

        #region 商品上传

        /// <summary>
        /// 上传商品
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public ActionResult ProductUpload(int categoryId, string spu, int status = -2)
        {
            var categoryModel = CategoryBLL.GetCategoriesInfoByID(LanguageEnum.SimplifiedChinese, categoryId);

            var categoryAttr = bll.GetCategoryAttrs(categoryId);

            SupplierBrandBLL brand = new SupplierBrandBLL();

            ViewBag.SupplierBrand = brand.GetBrandBySupplierId(CurrentUser.SupplierID, LanguageEnum.SimplifiedChinese);

            ViewBag.CategoryId = categoryId;

            ViewBag.CategoryModel = categoryModel;
            ViewBag.Action = "upload";
            ProductUpdateBindingModel pmodel = new ProductUpdateBindingModel();

            // 根据spuID 获取相关信息
            if (!string.IsNullOrEmpty(spu))
            {

                pmodel = bll.ReadProductInfo(spu, CurrentUser.SupplierID, status, false);

                if (pmodel == null || pmodel.ProductInfo_T == null)
                {
                    return new TransferResult("/Error/PageNotFound");
                }
                else
                {
                    ViewBag.ProductModel = pmodel;
                }
            }
            else
            {
                ViewBag.ProductModel = pmodel;
            }

            return View(categoryAttr);
        }


        /// <summary>
        /// 已发布的商品 编辑
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="spu"></param>
        /// <returns></returns>
        public ActionResult ProductEdit(int categoryId, string spu)
        {
            var categoryModel = CategoryBLL.GetCategoriesInfoByID(LanguageEnum.SimplifiedChinese, categoryId);

            if (categoryModel == null)
            {
                return View();
            }

            var categoryAttr = bll.GetCategoryAttrs(categoryId);

            SupplierBrandBLL brand = new SupplierBrandBLL();

            ViewBag.SupplierBrand = brand.GetBrandBySupplierId(CurrentUser.SupplierID, LanguageEnum.SimplifiedChinese);

            ViewBag.CategoryId = categoryId;

            ViewBag.CategoryModel = categoryModel;
            ViewBag.Action = "edit";
            ProductUpdateBindingModel pmodel = new ProductUpdateBindingModel();
            // 根据spuID 获取相关信息
            if (!string.IsNullOrEmpty(spu))
            {
                pmodel = bll.ReadProductInfo(spu, CurrentUser.SupplierID, 1, true);
                if (pmodel == null || pmodel.ProductInfo_S == null)
                {
                    return new TransferResult("/Error/PageNotFound");
                }
                else
                {
                    ViewBag.ProductModel = pmodel;
                }
            }

            return View("ProductUpload", categoryAttr);
        }

        /// <summary>
        /// 保存商品
        /// </summary>
        /// <param name="productInfo"></param>
        /// <param name="isRelease"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult SaveProduct(string productInfo, bool isRelease, string action)
        {
            var result = string.Empty;
            try
            {
                var product = JsonHelper.ToObject<ProductJsonModel>(productInfo);

                if (action == "edit")
                {
                    result = bll.SaveReleasedProduct(product, CurrentUser.SupplierID, CurrentUser.UserID);
                }
                else
                {
                    result = bll.SaveUnreleasedProduct(product, CurrentUser.SupplierID, CurrentUser.UserID, isRelease);
                }

                bll.SaveProductJson(result, productInfo, isRelease.ToString(), action);

                if (string.IsNullOrEmpty(result))
                {
                    return Json(new { data = 0, msg = "保存失败" });
                }
                else
                {
                    if (isRelease)
                    {
                        bll.UpdateCategoryHistory(CurrentUser.SupplierID, product.SPU.CategoryId);
                    }
                    return Json(new { data = 1, msg = "保存成功", Spu = result });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { data = 0, msg = "保存失败" });
            }

        }

        
        /// <summary>
        /// 保存商品
        /// </summary>
        /// <param name="productInfo"></param>
        /// <param name="isRelease"></param>
        /// <returns></returns>
        public JsonResult BarCodeRepeat(string barcode, string spu)
        {
            var result = false;
            try
            {
                result = bll.IsBarCodeRepeat(barcode, spu);

                return Json(new { data = result, msg = "驗證成功" });
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { data = result, msg = "驗證失败" });

            }
        }

        /// <summary>
        /// 上傳成功
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public ActionResult UploadSuccess(string oper)
        {
            ViewBag.Action = oper;
            return View();
        }

        #endregion

        #region  查看庫存

        /// <summary>
        /// 查看庫存
        /// </summary>
        /// <returns></returns>
        [RequirePermission(EnumPermission.Product_InventoryList)]
        public ActionResult InventoryList(InventoryListModel model)
        {
            var queryInfo = new InventoryListQueryInfo();
            queryInfo.SupplierID = base.CurrentUser.SupplierID;
            queryInfo.ProductName = model.ProductName.SafeTrim();
            queryInfo.Spu = model.Spu.SafeTrim();
            queryInfo.Sku = model.Sku.SafeTrim();
            queryInfo.BarCode = model.BarCode.SafeTrim();
            queryInfo.SkuStatus = model.SkuStatus;
            queryInfo.IsLowStockAlarm = model.IsLowStockAlarm;
            var page = new PageDTO() { PageIndex = model.PageIndex, PageSize = 50 };
            try
            {
                PageOf<SkuTempModel> dateList = bll.GetSkuInventoryList(queryInfo, LanguageEnum.TraditionalChinese, page);
                ViewBag.DataList = dateList;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View(model);
        }

        /// <summary>
        /// 库存导出 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [RequirePermission(EnumPermission.Product_InventoryList_Export)]
        public FileResult ExportInventoryListToExcel(InventoryListModel model)
        {
            var queryInfo = new InventoryListQueryInfo();
            queryInfo.SupplierID = base.CurrentUser.SupplierID;
            queryInfo.ProductName = model.ProductName.SafeTrim();
            queryInfo.Spu = model.Spu.SafeTrim();
            queryInfo.Sku = model.Sku.SafeTrim();
            queryInfo.BarCode = model.BarCode.SafeTrim();
            queryInfo.SkuStatus = model.SkuStatus;
            queryInfo.IsLowStockAlarm = model.IsLowStockAlarm;
            var fileName = "Inventory" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            try
            {
                var dateTable = bll.GetSkuInventoryListData(queryInfo, LanguageEnum.TraditionalChinese);
                var dtExporter = new DataTableExporter(EnumExcelType.XLSX);
                dtExporter.AddTable<InventoryListExportModel>(dateTable, "SKU庫存");
                return XlsxFile(dtExporter.Export(), fileName);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return XlsxFile(new byte[0], fileName);
        }


        #endregion

        public ActionResult SelectCategory(string productName)
        {
            try
            {
                if (productName == null)
                {
                    ViewBag.FirstList = CategoryBLL.GetFristLevelCategories(LanguageEnum.TraditionalChinese);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(productName))
                    {
                        ViewBag.ProductName = productName;
                        ViewBag.SearchResult = CategoryBLL.GetCategoriesByCategoryName(LanguageEnum.TraditionalChinese, productName);
                    }
                    else
                    {
                        ViewBag.ProductName = "";
                    }
                }
                ViewBag.UsuallyList = CategoryBLL.GetCategoryHistories(LanguageEnum.TraditionalChinese, CurrentUser.SupplierID);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetCategoryList(CategoryLevelEnum level, int parentID)
        {
            object data = null;
            try
            {
                List<CategoryModel> list = CategoryBLL.GetChildrenCategories(LanguageEnum.TraditionalChinese, level, parentID);
                data = list.Select(p => new { Id = p.CategoryId, Name = p.CategoryName });
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(data);
        }

        /// <summary>
        /// 已上傳商品
        /// </summary>
        /// <returns></returns>
        public ActionResult PublishedList(PublishedListModel model)
        {
            var queryInfo = new ProductListQueryInfo();
            queryInfo.SupplierID = base.CurrentUser.SupplierID;
            queryInfo.BarCode = model.BarCode.SafeTrim();
            queryInfo.HasInventory = model.HasInventory;
            queryInfo.IsOnSaled = model.IsOnSaled;
            queryInfo.ProductName = model.ProductName.SafeTrim();
            queryInfo.Sku = model.Sku.SafeTrim();
            if (model.SkuStatus.HasValue)
            {
                queryInfo.SkuStatus = new[] { model.SkuStatus.Value };
            }
            else
            {
                queryInfo.SkuStatus = new[] { 1, 3, 4, 5, 6 };
            }
            queryInfo.UploadTime = model.UploadTime;
            var page = new PageDTO() { PageIndex = model.PageIndex, PageSize = 20 };
            try
            {
                PageOf<ProductTempModel> dateList = bll.GetProductList(queryInfo, LanguageEnum.SimplifiedChinese, page);
                ViewBag.DataList = dateList;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View(model);
        }
        /// <summary>
        /// 上架SKU
        /// </summary>
        /// <param name="skuID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Active(string sku)
        {
            int result = 0;

            var request = new ChangeSkuStatusRequest();
            request.SupplierID = base.CurrentUser.SupplierID;
            request.Sku = sku;
            request.Status = 3;
            try
            {
               
                bool brandStatus = bll.CheckBrandStatus(sku);
                if (brandStatus)
                {
                    var flag = bll.ChangeSkuStatus(request);
                    if (flag)
                    {
                        result = 1;
                    }
                }
                else
                {
                    result = 2;
                }

                return Json(new { Error = result });
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Error = result });
            }
        }
        /// <summary>
        /// 下架SKU
        /// </summary>
        /// <param name="skuID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Inactive(string sku)
        {
            var request = new ChangeSkuStatusRequest();
            request.SupplierID = base.CurrentUser.SupplierID;
            request.Sku = sku;
            request.Status = 4;
            try
            {
                var flag = bll.ChangeSkuStatus(request);
                if (flag)
                {
                    return Json(new { Error = 0 });
                }
                else
                {
                    return Json(new { Error = 1 });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Error = 1 });
            }
        }

        /// <summary>
        /// 查看商品
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewProduct(string spu, int status = 0, bool isOnline = true)
        {
            try
            {
                var productView = bll.GetProductViewInfo(spu, CurrentUser.SupplierID, status, isOnline);
                if (productView == null || productView.ProductInfo_SC == null)
                {
                    return new TransferResult("/Error/PageNotFound");
                }
                return View(productView);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new TransferResult("/Error/PageNotFound");
            }
        }

        /// <summary>
        /// 編輯中商品
        /// </summary>
        /// <returns></returns>
        public ActionResult EditList(EditListModel model)
        {
            var queryInfo = new AuditProductListQueryInfo();
            queryInfo.SupplierID = base.CurrentUser.SupplierID;
            queryInfo.BarCode = model.BarCode.SafeTrim();
            queryInfo.EditType = model.EditType;
            queryInfo.ProductName = model.ProductName.SafeTrim();
            queryInfo.Sku = model.Sku.SafeTrim();
            if (model.SkuStatus.HasValue)
            {
                queryInfo.SkuStatus = new[] { model.SkuStatus.Value };
            }
            else
            {
                queryInfo.SkuStatus = new[] { 0, 2 };
            }
            queryInfo.EditTime = model.EditTime;
            var page = new PageDTO() { PageIndex = model.PageIndex, PageSize = 20 };
            try
            {
                PageOf<ProductTempModel> dateList = bll.GetAuditProductList(queryInfo, LanguageEnum.SimplifiedChinese, page);
                ViewBag.DataList = dateList;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult CancelEdit(string spu)
        {
            try
            {
                if (string.IsNullOrEmpty(spu))
                {
                    return Json(new { Error = 1 });
                }
                var flag = bll.CancelEditProduct(spu);
                if (flag)
                {
                    return Json(new { Error = 0 });
                }
                else
                {
                    return Json(new { Error = 1 });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Error = 1 });
            }
        }


        public void testSaveUnreleased(bool isInsert)
        {
            var json = String.Empty;

            if (isInsert)
            {
                json = "{\"SPU\":{\"Spu\":\"10151231612632\",\"CategoryId\":10022,\"Name\":{\"Content_S\":\"测试商品1\",\"Content_T\":\"測試商品1\",\"Content_E\":\"TestProudct1\"},\"Tag\":[{\"Content_S\":\"1\",\"Content_T\":\"1\",\"Content_E\":\"1\"},{\"Content_S\":\"2\",\"Content_T\":\"2\",\"Content_E\":\"2\"}],\"Price\":12.2,\"Description\":{\"Content_S\":\"舒适\",\"Content_T\":\"舒適\",\"Content_E\":\"Confortable\"},\"BrandId\":1,\"CountryOfManufactureId\":\"2\",\"SalesTerritory\":1,\"Unit\":{\"Key\":\"13\",\"Other\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"}},\"IsExchangeInCHINA\":1,\"IsExchangeInHK\":1,\"IsReturn\":1,\"MinForOrder\":10,\"NetWeightUnitId\":\"1\",\"NetContentUnitId\":\"1\",\"IsDutyOnSeller\":1,\"Images\":[{\"path\":\"abc.jpg\",\"SortId\":1},{\"path\":\"efg.jpg\",\"SortId\":2}],\"PreOnSaleTime\":\"2016-01-01\",\"CommissionInCHINA\":2.1,\"CommissionInHK\":2.1},\"SKUS\":[{\"Sku\":\"20151231739206\",\"MainDicKey\":\"Color\",\"SubDicKey\":\"Size\",\"MainKey\":\"\",\"MainValue\":{\"Content_S\":\"abc\",\"Content_T\":\"abc\",\"Content_E\":\"abc\"},\"SubKey\":\"8\",\"NetWeight\":0,\"NetContent\":1.2,\"Specifications\":{\"Content_S\":\"100*100\",\"Content_T\":\"111*111\",\"Content_E\":\"222*222\"},\"AlcoholPercentage\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Smell\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"CapacityRestriction\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Price\":1.2,\"BarCode\":\"\",\"AlarmStockQty\":1},{\"Sku\":\"\",\"MainDicKey\":\"Color\",\"SubDicKey\":\"Size\",\"MainKey\":\"4\",\"MainValue\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"SubKey\":\"6\",\"NetWeight\":0,\"NetContent\":1.2,\"Specifications\":{\"Content_S\":\"100*100\",\"Content_T\":\"111*111\",\"Content_E\":\"222*222\"},\"AlcoholPercentage\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Smell\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"CapacityRestriction\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Price\":1.2,\"BarCode\":\"\",\"AlarmStockQty\":1}],\"SpuEx\":{\"Materials\":{\"Key\":\"1\",\"Other\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"}},\"Pattern\":{\"Content_S\":\"11\",\"Content_T\":\"11\",\"Content_E\":\"11\"},\"Flavour\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Ingredients\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"StoragePeriod\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"StoringTemperature\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"SkinType\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"GenderId\":1,\"AgeGroup\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Model\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"BatteryTime\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Voltage\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Power\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Warranty\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"SupportedLanguage\":{\"keys\":[\"1\",\"2\",\"3\"],\"Others\":[{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"}]},\"PetAgeUnit\":{\"Key\":\"1\"},\"PetType\":{\"Key\":\"1\",\"Other\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"}},\"PetAge\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Location\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Weight\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"WeightUnit\":{\"Key\":\"1\"},\"Volume\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"VolumeUnit\":{\"Key\":\"1\"},\"Length\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"LengthUnit\":{\"Key\":\"1\"},\"Width\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"WidthUnit\":{\"Key\":\"1\"},\"Height\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"HeightUnit\":{\"Key\":\"1\"}}}";
            }
            else
            {
                json = "{\"SPU\":{\"Spu\":\"\",\"CategoryId\":10022,\"Name\":{\"Content_S\":\"测试商品\",\"Content_T\":\"測試商品\",\"Content_E\":\"TestProudct\"},\"Tag\":[{\"Content_S\":\"1\",\"Content_T\":\"2\",\"Content_E\":\"3\"},{\"Content_S\":\"2\",\"Content_T\":\"2\",\"Content_E\":\"2\"}],\"Price\":12.2,\"Description\":{\"Content_S\":\"舒适\",\"Content_T\":\"舒適\",\"Content_E\":\"Confortable\"},\"BrandId\":1,\"CountryOfManufactureId\":\"2\",\"SalesTerritory\":1,\"Unit\":{\"Key\":\"13\",\"Other\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"}},\"IsExchangeInCHINA\":1,\"IsExchangeInHK\":1,\"IsReturn\":1,\"MinForOrder\":10,\"NetWeightUnitId\":\"1\",\"NetContentUnitId\":\"1\",\"IsDutyOnSeller\":1,\"Images\":[{\"path\":\"abc.jpg\",\"SortId\":1},{\"path\":\"efg.jpg\",\"SortId\":2}],\"PreOnSaleTime\":\"2016-01-01\",\"CommissionInCHINA\":2.1,\"CommissionInHK\":2.1},\"SKUS\":[{\"Sku\":\"\",\"MainDicKey\":\"Color\",\"SubDicKey\":\"Size\",\"MainKey\":\"4\",\"MainValue\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"SubKey\":\"8\",\"NetWeight\":0,\"NetContent\":1.2,\"Specifications\":{\"Content_S\":\"100*100\",\"Content_T\":\"111*111\",\"Content_E\":\"222*222\"},\"AlcoholPercentage\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Smell\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"CapacityRestriction\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Price\":1.2,\"BarCode\":\"\",\"AlarmStockQty\":1},{\"Sku\":\"\",\"MainDicKey\":\"Color\",\"SubDicKey\":\"Size\",\"MainKey\":\"4\",\"MainValue\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"SubKey\":\"7\",\"NetWeight\":0,\"NetContent\":1.2,\"Specifications\":{\"Content_S\":\"100*100\",\"Content_T\":\"111*111\",\"Content_E\":\"222*222\"},\"AlcoholPercentage\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Smell\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"CapacityRestriction\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Price\":1.2,\"BarCode\":\"\",\"AlarmStockQty\":1}],\"SpuEx\":{\"Materials\":{\"Key\":\"1\",\"Other\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"}},\"Pattern\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Flavour\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Ingredients\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"StoragePeriod\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"StoringTemperature\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"SkinType\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"GenderId\":1,\"AgeGroup\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Model\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"BatteryTime\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Voltage\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Power\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Warranty\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"SupportedLanguage\":{\"keys\":[\"1\",\"2\",\"3\"],\"Others\":[{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"}]},\"PetAgeUnit\":{\"Key\":\"1\"},\"PetType\":{\"Key\":\"1\",\"Other\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"}},\"PetAge\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Location\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Weight\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"WeightUnit\":{\"Key\":\"1\"},\"Volume\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"VolumeUnit\":{\"Key\":\"1\"},\"Length\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"LengthUnit\":{\"Key\":\"1\"},\"Width\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"WidthUnit\":{\"Key\":\"1\"},\"Height\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"HeightUnit\":{\"Key\":\"1\"}}}";
            }

            var a = JsonHelper.ToObject<ProductJsonModel>(json);

            bll.SaveUnreleasedProduct(a, 2, 1, false);
        }

        public void testSaveReleasedProduct()
        {
            var json = "{\"SPU\":{\"Spu\":\"\",\"CategoryId\":10022,\"Name\":{\"Content_S\":\"测试商品\",\"Content_T\":\"測試商品\",\"Content_E\":\"TestProudct\"},\"Tag\":[{\"Content_S\":\"1\",\"Content_T\":\"2\",\"Content_E\":\"3\"},{\"Content_S\":\"2\",\"Content_T\":\"2\",\"Content_E\":\"2\"}],\"Price\":12.2,\"Description\":{\"Content_S\":\"舒适\",\"Content_T\":\"舒適\",\"Content_E\":\"Confortable\"},\"BrandId\":1,\"CountryOfManufactureId\":\"2\",\"SalesTerritory\":1,\"Unit\":{\"Key\":\"13\",\"Other\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"}},\"IsExchangeInCHINA\":1,\"IsExchangeInHK\":1,\"IsReturn\":1,\"MinForOrder\":10,\"NetWeightUnitId\":\"1\",\"NetContentUnitId\":\"1\",\"IsDutyOnSeller\":1,\"Images\":[{\"path\":\"abc.jpg\",\"SortId\":1},{\"path\":\"efg.jpg\",\"SortId\":2}],\"PreOnSaleTime\":\"2016-01-01\",\"CommissionInCHINA\":2.1,\"CommissionInHK\":2.1},\"SKUS\":[{\"Sku\":\"\",\"MainDicKey\":\"Color\",\"SubDicKey\":\"Size\",\"MainKey\":\"4\",\"MainValue\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"SubKey\":\"8\",\"NetWeight\":0,\"NetContent\":1.2,\"Specifications\":{\"Content_S\":\"100*100\",\"Content_T\":\"111*111\",\"Content_E\":\"222*222\"},\"AlcoholPercentage\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Smell\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"CapacityRestriction\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Price\":1.2,\"BarCode\":\"\",\"AlarmStockQty\":1},{\"Sku\":\"\",\"MainDicKey\":\"Color\",\"SubDicKey\":\"Size\",\"MainKey\":\"4\",\"MainValue\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"SubKey\":\"7\",\"NetWeight\":0,\"NetContent\":1.2,\"Specifications\":{\"Content_S\":\"100*100\",\"Content_T\":\"111*111\",\"Content_E\":\"222*222\"},\"AlcoholPercentage\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Smell\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"CapacityRestriction\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Price\":1.2,\"BarCode\":\"\",\"AlarmStockQty\":1}],\"SpuEx\":{\"Materials\":{\"Key\":\"1\",\"Other\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"}},\"Pattern\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Flavour\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Ingredients\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"StoragePeriod\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"StoringTemperature\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"SkinType\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"GenderId\":1,\"AgeGroup\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Model\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"BatteryTime\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Voltage\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Power\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Warranty\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"SupportedLanguage\":{\"keys\":[\"1\",\"2\",\"3\"],\"Others\":[{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"}]},\"PetAgeUnit\":{\"Key\":\"1\"},\"PetType\":{\"Key\":\"1\",\"Other\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"}},\"PetAge\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Location\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"Weight\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"WeightUnit\":{\"Key\":\"1\"},\"Volume\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"VolumeUnit\":{\"Key\":\"1\"},\"Length\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"LengthUnit\":{\"Key\":\"1\"},\"Width\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"WidthUnit\":{\"Key\":\"1\"},\"Height\":{\"Content_S\":\"\",\"Content_T\":\"\",\"Content_E\":\"\"},\"HeightUnit\":{\"Key\":\"1\"}}}";

            var a = JsonHelper.ToObject<ProductJsonModel>(json);

            bll.SaveReleasedProduct(a, 2, 1);
        }

        public void testGetProduct(string spu, int status, bool isOnline)
        {
            var a = bll.ReadProductInfo(spu, CurrentUser.SupplierID, status, isOnline);

            if (null == a)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="isNew">是否新建</param>
        /// <param name="isRelease">是否发布，true发布 false草稿</param>
        public bool SaveUnreleasedProduct(string productInfo, bool isRelease)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(productInfo))
                {
                    throw new ArgumentNullException("json", "Json参数为空");
                }

                var productJsonModel = JsonHelper.ToObject<ProductJsonModel>(productInfo);

                bll.SaveUnreleasedProduct(productJsonModel, CurrentUser.SupplierID, CurrentUser.UserID, isRelease);

            }
            catch (ArgumentNullException ext)
            {
                LogHelper.Error(ext);
                return false;
            }
            catch (ArgumentOutOfRangeException ext)
            {
                LogHelper.Error(ext);
                return false;
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
                return false;
            }

            return true;
        }

        public bool SaveReleasedProduct(string json)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(json))
                {
                    throw new ArgumentNullException("json", "Json参数为空");
                }

                var productJsonModel = JsonHelper.ToObject<ProductJsonModel>(json);

                if (String.IsNullOrWhiteSpace(productJsonModel.SPU.Spu))
                {
                    throw new ArgumentNullException("Spu", "Spu参数为空");
                }

                if (productJsonModel.SKUS == null || productJsonModel.SKUS.Length == 0 || String.IsNullOrWhiteSpace(productJsonModel.SKUS[0].Sku))
                {
                    throw new ArgumentNullException("SKUS", "SKUS参数为空");
                }

                bll.SaveUnreleasedProduct(productJsonModel, CurrentUser.SupplierID, CurrentUser.UserID, true);

            }
            catch (ArgumentNullException ext)
            {
                LogHelper.Error(ext);
                return false;
            }
            catch (ArgumentOutOfRangeException ext)
            {
                LogHelper.Error(ext);
                return false;
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
                return false;
            }

            return true;
        }
    }


}