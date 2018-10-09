using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SFO2O.Admin.Businesses;
using SFO2O.Admin.Businesses.Product;
using SFO2O.Admin.Businesses.Supplier;
using SFO2O.Admin.Common;
using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Enums;
using SFO2O.Admin.Models.Product;
using SFO2O.Admin.ViewModel;
using SFO2O.Admin.ViewModel.Product;
using SFO2O.Admin.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFO2O.Admin.Web.Controllers
{
    public class ProductController : BaseController
    {
        private ProdcutBLL productBll = new ProdcutBLL();
        private SupplierBLL supplierBLL = new SupplierBLL();

        //
        // GET: /Product/
        public ActionResult ProductAuditingIndex()
        {
            var dtNow = DateTime.Now;

            var pqm = new ProductAuditingQuyModel()
            {
                EditType = Request["EditType"] == null ? 0 : Convert.ToInt32(Request["EditType"].ToString()),
                ProductName = Request["ProductName"] == null ? "" : Request["ProductName"].ToString(),
                CreateTimeEnd = Request["EndTime"] == null ? dtNow : Convert.ToDateTime(Request["EndTime"].ToString()),
                CreateTimeStart = Request["StartTime"] == null ? dtNow.AddMonths(-6) : Convert.ToDateTime(Request["StartTime"].ToString()),
                ReportStatus = Request["ReportStatus"] == null ? -2 : Convert.ToInt32(Request["ReportStatus"].ToString()),
                SalesTerritory = Request["SalesTerritory"] == null ? 0 : Convert.ToInt32(Request["SalesTerritory"].ToString()),
                Sku = Request["Sku"] == null ? "" : Request["Sku"].ToString(),
                Spu = Request["Spu"] == null ? "" : Request["Spu"].ToString(),
                SupplierId = Request["SupplierId"] == null ? 0 : Convert.ToInt32(Request["SupplierId"].ToString())
            };

            ViewBag.PageNo = Request["PageNo"] == null ? 1 : Convert.ToInt32(Request["PageNo"].ToString());
            ViewBag.PageSize = Request["PageSize"] == null ? 50 : Convert.ToInt32(Request["PageSize"].ToString());

            ViewBag.SupplierNames = supplierBLL.GetSupplierNames();

            return View(pqm);
        }

        public ActionResult ProductAuditingList(ProductAuditingQuyModel queryModel)
        {
            PageOf<ProductAuditingListModel> list = null;

            try
            {
                //2繁体
                //list = productBll.GetAuditingProductList(this.PageSize, this.PageNo, 2, queryModel);
                //1简体
                list = productBll.GetAuditingProductList(this.PageSize, this.PageNo, 1, queryModel);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(list);
        }

        public ActionResult ExcportProductAuditingList(DateTime startTime, DateTime endTime, string spu, string sku, string productName,
                int editType, int reportStatus, int supplierid, int salesTerritory)
        {
            var dtNow = DateTime.Now;

            ProductAuditingQuyModel qm = new ProductAuditingQuyModel()
            {
                Spu = spu,
                Sku = sku,
                CreateTimeEnd = endTime,
                CreateTimeStart = startTime,
                EditType = editType,
                ReportStatus = reportStatus,
                SupplierId = supplierid,
                SalesTerritory = salesTerritory
            };

            PageOf<ProductExportModel> list = null;

            try
            {
                list = productBll.AuditingProductsExport(1, qm);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                Response.AppendHeader("Content-Disposition", "attachment;filename=declare_customs" + System.DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                ProductAuditingListToExcel(list.Items);
                Response.End();
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View();
        }

        private void ProductAuditingListToExcel(IList<ProductExportModel> list)
        {
            #region 导出Excel

            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");

            var rowIndex = 3;
            ExcelHelper excelHelper = new ExcelHelper();
            //大标题
            excelHelper.SetBigTitle(sheet, workbook, "商品列表", 75);
            //子标题
            excelHelper.SetSubTitle(sheet, workbook, @"SPU,SKU,商品名称,商品分类,商品标签
                ,商品价格,SKU价格,条形码,商品详情,商品品牌
                ,产地,销售区域,商品单位,大陆是否支持换货,香港是否支持换货
                ,是否支持退货,最小库存量,预警库存,最低价格,商家承担关税
                ,编辑类型,大陆地区佣金比例,香港地区佣金比例,编辑时间
                ,材质,样式,酒香型,主要成分,保质期,储藏温度,皮肤类型
                ,适用性别,适合年龄,产品型号,电池使用时间,电压
                ,电源,质保,支持语言,宠物类型,宠物年龄单位,宠物年龄
                ,商品适合地域,重量单位,重量,体积单位,体积,长度单位
                ,长度,宽度单位,宽度,高度单位,高度,净重单位,净重,净含量单位
                ,净含量,主属性类别,子属性类别,主属性值,子属性值,规格,尺码
                ,颜色,酒精度,气味,存储容量,海关单位,商品国检编号,HS编码
                ,计量单位,商品备案编号,国条码,税率,行邮税号,海关型号", 2);
            #region 填充数据

            ICellStyle itemStyle = workbook.CreateCellStyle();
            itemStyle.BorderBottom = BorderStyle.Thin;
            itemStyle.BorderLeft = BorderStyle.Thin;
            itemStyle.BorderRight = BorderStyle.Thin;
            itemStyle.BorderTop = BorderStyle.Thin;

            foreach (var entity in list)
            {
                #region
                var listRow = sheet.CreateRow(rowIndex++);

                ICell cell;
                cell = excelHelper.CreateCell(listRow, 0, CellType.String, string.Format("{0}", entity.Spu), itemStyle);
                cell = excelHelper.CreateCell(listRow, 1, CellType.String, string.Format("{0}", entity.Sku), itemStyle);
                cell = excelHelper.CreateCell(listRow, 2, CellType.String, string.Format("{0}", entity.Name), itemStyle);
                cell = excelHelper.CreateCell(listRow, 3, CellType.String, string.Format("{0}", entity.CategoryName), itemStyle);
                cell = excelHelper.CreateCell(listRow, 4, CellType.String, string.Format("{0}", entity.Tag), itemStyle);

                cell = excelHelper.CreateCell(listRow, 5, CellType.String, string.Format("{0}", entity.Price), itemStyle);
                cell = excelHelper.CreateCell(listRow, 6, CellType.String, string.Format("{0}", entity.skuPrice), itemStyle);
                cell = excelHelper.CreateCell(listRow, 7, CellType.String, string.Format("{0}", entity.BarCode), itemStyle);
                cell = excelHelper.CreateCell(listRow, 8, CellType.String, string.Format("{0}", entity.Description), itemStyle);
                cell = excelHelper.CreateCell(listRow, 9, CellType.String, string.Format("{0}", entity.Brand), itemStyle);

                cell = excelHelper.CreateCell(listRow, 10, CellType.String, string.Format("{0}", entity.CountryOfManufacture), itemStyle);
                cell = excelHelper.CreateCell(listRow, 11, CellType.String, string.Format("{0}", entity.SalesTerritory), itemStyle);
                cell = excelHelper.CreateCell(listRow, 12, CellType.String, string.Format("{0}", entity.Unit), itemStyle);
                cell = excelHelper.CreateCell(listRow, 13, CellType.String, string.Format("{0}", entity.IsExchangeInCHINA), itemStyle);
                cell = excelHelper.CreateCell(listRow, 14, CellType.String, string.Format("{0}", entity.IsExchangeInHK), itemStyle);

                cell = excelHelper.CreateCell(listRow, 15, CellType.String, string.Format("{0}", entity.IsReturn), itemStyle);
                cell = excelHelper.CreateCell(listRow, 16, CellType.String, string.Format("{0}", entity.MinForOrder), itemStyle);
                cell = excelHelper.CreateCell(listRow, 17, CellType.String, string.Format("{0}", entity.AlarmStockQty), itemStyle);
                cell = excelHelper.CreateCell(listRow, 18, CellType.String, string.Format("{0}", entity.MinPrice), itemStyle);
                cell = excelHelper.CreateCell(listRow, 19, CellType.String, string.Format("{0}", entity.IsDutyOnSeller), itemStyle);

                cell = excelHelper.CreateCell(listRow, 20, CellType.String, string.Format("{0}", entity.DataSource), itemStyle);
                cell = excelHelper.CreateCell(listRow, 21, CellType.String, string.Format("{0}", entity.CommissionInCHINA.ToString() + "%"), itemStyle);
                cell = excelHelper.CreateCell(listRow, 22, CellType.String, string.Format("{0}", entity.CommissionInHK.ToString() + "%"), itemStyle);
                cell = excelHelper.CreateCell(listRow, 23, CellType.String, string.Format("{0}", entity.ModifyTime.ToString("yyyy-MM-dd HH:mm:ss")), itemStyle);
                cell = excelHelper.CreateCell(listRow, 24, CellType.String, string.Format("{0}", entity.Materials), itemStyle);

                cell = excelHelper.CreateCell(listRow, 25, CellType.String, string.Format("{0}", entity.Pattern), itemStyle);
                cell = excelHelper.CreateCell(listRow, 26, CellType.String, string.Format("{0}", entity.Flavour), itemStyle);
                cell = excelHelper.CreateCell(listRow, 27, CellType.String, string.Format("{0}", entity.Ingredients), itemStyle);
                cell = excelHelper.CreateCell(listRow, 28, CellType.String, string.Format("{0}", entity.StoragePeriod), itemStyle);
                cell = excelHelper.CreateCell(listRow, 29, CellType.String, string.Format("{0}", entity.StoringTemperature), itemStyle);

                cell = excelHelper.CreateCell(listRow, 30, CellType.String, string.Format("{0}", entity.SkinType), itemStyle);
                cell = excelHelper.CreateCell(listRow, 31, CellType.String, string.Format("{0}", entity.Gender), itemStyle);
                cell = excelHelper.CreateCell(listRow, 32, CellType.String, string.Format("{0}", entity.AgeGroup), itemStyle);
                cell = excelHelper.CreateCell(listRow, 33, CellType.String, string.Format("{0}", entity.Model), itemStyle);
                cell = excelHelper.CreateCell(listRow, 34, CellType.String, string.Format("{0}", entity.BatteryTime), itemStyle);

                cell = excelHelper.CreateCell(listRow, 35, CellType.String, string.Format("{0}", entity.Voltage), itemStyle);
                cell = excelHelper.CreateCell(listRow, 36, CellType.String, string.Format("{0}", entity.Power), itemStyle);
                cell = excelHelper.CreateCell(listRow, 37, CellType.String, string.Format("{0}", entity.Warranty), itemStyle);
                cell = excelHelper.CreateCell(listRow, 38, CellType.String, string.Format("{0}", entity.SupportedLanguage), itemStyle);
                cell = excelHelper.CreateCell(listRow, 39, CellType.String, string.Format("{0}", entity.PetType), itemStyle);

                cell = excelHelper.CreateCell(listRow, 40, CellType.String, string.Format("{0}", entity.PetAgeUnit), itemStyle);
                cell = excelHelper.CreateCell(listRow, 41, CellType.String, string.Format("{0}", entity.PetAge), itemStyle);
                cell = excelHelper.CreateCell(listRow, 42, CellType.String, string.Format("{0}", entity.Location), itemStyle);
                cell = excelHelper.CreateCell(listRow, 43, CellType.String, string.Format("{0}", entity.WeightUnit), itemStyle);
                cell = excelHelper.CreateCell(listRow, 44, CellType.String, string.Format("{0}", entity.Weight), itemStyle);

                cell = excelHelper.CreateCell(listRow, 45, CellType.String, string.Format("{0}", entity.VolumeUnit), itemStyle);
                cell = excelHelper.CreateCell(listRow, 46, CellType.String, string.Format("{0}", entity.Volume), itemStyle);
                cell = excelHelper.CreateCell(listRow, 47, CellType.String, string.Format("{0}", entity.LengthUnit), itemStyle);
                cell = excelHelper.CreateCell(listRow, 48, CellType.String, string.Format("{0}", entity.Length), itemStyle);
                cell = excelHelper.CreateCell(listRow, 49, CellType.String, string.Format("{0}", entity.WidthUnit), itemStyle);

                cell = excelHelper.CreateCell(listRow, 50, CellType.String, string.Format("{0}", entity.Width), itemStyle);
                cell = excelHelper.CreateCell(listRow, 51, CellType.String, string.Format("{0}", entity.HeightUnit), itemStyle);
                cell = excelHelper.CreateCell(listRow, 52, CellType.String, string.Format("{0}", entity.Height), itemStyle);
                cell = excelHelper.CreateCell(listRow, 53, CellType.String, string.Format("{0}", entity.NetWeightUnit), itemStyle);
                cell = excelHelper.CreateCell(listRow, 54, CellType.String, string.Format("{0}", entity.NetWeight), itemStyle);

                cell = excelHelper.CreateCell(listRow, 55, CellType.String, string.Format("{0}", entity.NetContentUnit), itemStyle);
                cell = excelHelper.CreateCell(listRow, 56, CellType.String, string.Format("{0}", entity.NetContent), itemStyle);
                cell = excelHelper.CreateCell(listRow, 57, CellType.String, string.Format("{0}", entity.MainDicValue), itemStyle);
                cell = excelHelper.CreateCell(listRow, 58, CellType.String, string.Format("{0}", entity.SubDicValue), itemStyle);
                cell = excelHelper.CreateCell(listRow, 59, CellType.String, string.Format("{0}", entity.MainValue), itemStyle);

                cell = excelHelper.CreateCell(listRow, 60, CellType.String, string.Format("{0}", entity.SubValue), itemStyle);
                cell = excelHelper.CreateCell(listRow, 61, CellType.String, string.Format("{0}", entity.Specifications), itemStyle);
                cell = excelHelper.CreateCell(listRow, 62, CellType.String, string.Format("{0}", entity.Size), itemStyle);
                cell = excelHelper.CreateCell(listRow, 63, CellType.String, string.Format("{0}", entity.Color), itemStyle);
                cell = excelHelper.CreateCell(listRow, 64, CellType.String, string.Format("{0}", entity.AlarmStockQty), itemStyle);

                cell = excelHelper.CreateCell(listRow, 65, CellType.String, string.Format("{0}", entity.Smell), itemStyle);
                cell = excelHelper.CreateCell(listRow, 66, CellType.String, string.Format("{0}", entity.CapacityRestriction), itemStyle);
                cell = excelHelper.CreateCell(listRow, 67, CellType.String, string.Format("{0}", entity.CustomsUnit), itemStyle);
                cell = excelHelper.CreateCell(listRow, 68, CellType.String, string.Format("{0}", entity.InspectionNo), itemStyle);
                cell = excelHelper.CreateCell(listRow, 69, CellType.String, string.Format("{0}", entity.HSCode), itemStyle);

                cell = excelHelper.CreateCell(listRow, 70, CellType.String, string.Format("{0}", entity.UOM), itemStyle);
                cell = excelHelper.CreateCell(listRow, 71, CellType.String, string.Format("{0}", entity.PrepardNo), itemStyle);
                cell = excelHelper.CreateCell(listRow, 72, CellType.String, string.Format("{0}", entity.GnoCode), itemStyle);
                cell = excelHelper.CreateCell(listRow, 73, CellType.String, string.Format("{0}", entity.TaxRate.ToString() + "%"), itemStyle);
                cell = excelHelper.CreateCell(listRow, 74, CellType.String, string.Format("{0}", entity.TaxCode), itemStyle);

                cell = excelHelper.CreateCell(listRow, 75, CellType.String, string.Format("{0}", entity.ModelForCustoms), itemStyle);
                #endregion
            }
            excelHelper.AutoColumnWidth(sheet, 76);

            #endregion

            Stream ouputStream = Response.OutputStream;
            workbook.Write(ouputStream);
            ouputStream.Flush();
            ouputStream.Close();
            #endregion
        }

        public ActionResult AuditingProduct(string spu)
        {
            var dtNow = DateTime.Now;

            var pqm = new ProductAuditingQuyModel()
            {
                EditType = Request["EditType"] == null ? 0 : Convert.ToInt32(Request["EditType"].ToString()),
                ProductName = Request["ProductName"] == null ? "" : Request["ProductName"].ToString(),
                CreateTimeEnd = Request["EndTime"] == null ? dtNow : Convert.ToDateTime(Request["EndTime"].ToString()),
                CreateTimeStart = Request["StartTime"] == null ? dtNow.AddMonths(-6) : Convert.ToDateTime(Request["StartTime"].ToString()),
                ReportStatus = Request["ReportStatus"] == null ? 0 : Convert.ToInt32(Request["ReportStatus"].ToString()),
                SalesTerritory = Request["SalesTerritory"] == null ? 0 : Convert.ToInt32(Request["SalesTerritory"].ToString()),
                Sku = Request["Sku"] == null ? "" : Request["Sku"].ToString(),
                Spu = Request["SpuNo"] == null ? "" : Request["SpuNo"].ToString(),
                SupplierId = Request["SupplierId"] == null ? 0 : Convert.ToInt32(Request["SupplierId"].ToString())
            };

            ViewBag.PageNo = Request["PageNo"] == null ? 1 : Convert.ToInt32(Request["PageNo"].ToString());
            ViewBag.PageSize = Request["PageSize"] == null ? 50 : Convert.ToInt32(Request["PageSize"].ToString());
            ViewBag.QueryParas = pqm;


            ProductAuditingViewModel result = new ProductAuditingViewModel();
            ViewBag.SpuId = spu;
            try
            {
                ViewBag.KeyNames = CommonBLL.GetDicsInfoByKey("ProductAttributes");

                result = productBll.GetProductAndCustomInfo(spu);

                if (result == null || result.SpuBaseInfo == null)
                {
                    result.PackingInfo = new ProductPackingModel();
                    result.ProductAttrsInfos = new Dictionary<Models.Enums.LanguageEnum, Dictionary<string, string>>();
                    result.ProductCustomInfos = new List<ProductSkuCustomInfoModel>();
                    result.ProductImgs = new List<ProductImgInfoModel>();
                    result.SpuBaseInfo = new Dictionary<Models.Enums.LanguageEnum, ProductBaseInfoModel>();
                    result.SysInfo = new ProductSysPropertyModel();
                }
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(result);
        }

        public ActionResult AuditingProductOnline(string spu)
        {
            ProductAuditingViewModel result = new ProductAuditingViewModel();
            try
            {
                ViewBag.KeyNames = CommonBLL.GetDicsInfoByKey("ProductAttributes");

                result = productBll.GetProductAndCustomInfoOnline(spu);

                if (result == null || result.SpuBaseInfo == null)
                {
                    result.PackingInfo = new ProductPackingModel();
                    result.ProductAttrsInfos = new Dictionary<Models.Enums.LanguageEnum, Dictionary<string, string>>();
                    result.ProductCustomInfos = new List<ProductSkuCustomInfoModel>();
                    result.ProductImgs = new List<ProductImgInfoModel>();
                    result.SpuBaseInfo = new Dictionary<Models.Enums.LanguageEnum, ProductBaseInfoModel>();
                    result.SysInfo = new ProductSysPropertyModel();
                }
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(result);
        }

        public void UpdateSpuStatus(string spu, int status, string reason)
        {
            try
            {
                if (status == 1)
                {
                    productBll.TransferProduct(spu);
                }

                productBll.UpDateSpuStatus(spu, status);
                productBll.InsertProductAuditingLog(spu, status, reason, this.UserName);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }
        }

        public void CustomReportTempStorage(string json, bool isChangeRS)
        {
            try
            {
                var model = JsonHelper.ToObject<CustomReportJsonModel>(json);
                productBll.InsertTemStorageCusReports(model, isChangeRS);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }
        }

        public ActionResult ImportCustomReport()
        {
            try
            {
                if (Request.Files.Count <= 0)
                {
                    var result = new
                    {
                        Success = false,
                        Message = "文件數量為空"
                    };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var excel = HttpContext.Request.Files[0];

                var fileName = excel.FileName.Split('.');

                if (fileName.Length < 2 && (fileName[fileName.Length - 1] != "xlsx" || fileName[fileName.Length - 1] != "xls"))
                {
                    var result = new
                    {
                        Success = false,
                        Message = "文件格式不正確"
                    };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var currentName = DateTime.Now.ToString("yyyyMMddmmss") + "." + fileName[fileName.Length - 1];

                excel.SaveAs(HttpContext.Server.MapPath("/InputExcel/" + currentName));

                var excelTable = ExcelHelper.Import(HttpContext.Server.MapPath("/InputExcel/" + currentName), 2);

                var importResult = productBll.ImportCustomReports(productBll.GetAuditingSkuInfos(), excelTable);

                if (importResult == null)
                {
                    var result = new
                    {
                        Success = false,
                        Message = "文件為空"
                    };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var resultJson = new
                {
                    Success = true,
                    SuccessCount = importResult["true"],
                    FailCount = importResult["false"],
                    BatchNo = importResult["batchNO"],
                };

                return Json(resultJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);

                var result = new
                    {
                        Success = false,
                        Message = "上传失败"
                    };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetImportError(int batchNo)
        {
            List<ImportErrorModel> result = new List<ImportErrorModel>();

            try
            {
                result = productBll.GetImportErrors(batchNo);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(result);
        }


        #region 管理商品

        /// <summary>
        /// 已上傳商品
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductInfoList(string startTime, string endTime, string spu, string sku, string productName, int supplierId = 0, int productStatus = -1, int salesTerritory = -1,
            int inventoryStatus = -1, int isOnSales = -1, int fstCagegoryId = 0, int sndCagegoryId = 0, int trdCagegoryId = 0)
        {
            var queryInfo = new ProductAuditingQuyModel();
            if (string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
            {
                queryInfo.CreateTimeStart = DateTime.Now.AddMonths(-3);
                queryInfo.CreateTimeEnd = DateTime.Now;
            }
            else
            {
                queryInfo.CreateTimeStart = DateTime.Parse(startTime);
                queryInfo.CreateTimeEnd = DateTime.Parse(endTime);
            }
            queryInfo.Spu = spu;
            queryInfo.Sku = sku;
            queryInfo.ProductName = productName;
            queryInfo.ProductStatus = productStatus;
            queryInfo.InventoryStatus = inventoryStatus;
            queryInfo.SupplierId = supplierId;
            queryInfo.IsOnSales = isOnSales;
            queryInfo.SalesTerritory = salesTerritory;
            queryInfo.FstCagegoryId = fstCagegoryId;
            queryInfo.SndCagegoryId = sndCagegoryId;
            queryInfo.TrdCagegoryId = trdCagegoryId;

            ViewBag.QueryInfo = queryInfo;

            PageOf<ProductAuditingListModel> dataList = null;

            try
            {
                var page = new PagingModel() { PageIndex = PageNo, PageSize = 50 };

                ViewBag.FirCategoryId = CategoryBLL.GetChildrenCategories(LanguageEnum.TraditionalChinese, 0, 0);
                List<CategoryModel> second = new List<CategoryModel>();
                if (fstCagegoryId > 0)
                {
                    second = CategoryBLL.GetChildrenCategories(LanguageEnum.TraditionalChinese, 1, fstCagegoryId);
                }
                ViewBag.SndCategoryId = second;

                List<CategoryModel> thrid = new List<CategoryModel>();
                if (sndCagegoryId > 0)
                {
                    thrid = CategoryBLL.GetChildrenCategories(LanguageEnum.TraditionalChinese, 2, sndCagegoryId);
                }
                ViewBag.TrdCategoryId = thrid;

                dataList = productBll.GetProductList(queryInfo, LanguageEnum.TraditionalChinese, page);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View(dataList);
        }


        public ActionResult ExportProductInfoList(string startTime, string endTime, string spu, string sku, string productName, int supplierId = 0, int productStatus = -1, int salesTerritory = -1,
            int inventoryStatus = -1, int isOnSales = -1, int fstCagegoryId = 0, int sndCagegoryId = 0, int trdCagegoryId = 0)
        {

            var queryInfo = new ProductAuditingQuyModel();
            if (string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
            {
                queryInfo.CreateTimeStart = DateTime.Now.AddMonths(-3);
                queryInfo.CreateTimeEnd = DateTime.Now;
            }
            else
            {
                queryInfo.CreateTimeStart = DateTime.Parse(startTime);
                queryInfo.CreateTimeEnd = DateTime.Parse(endTime);
            }
            queryInfo.Spu = spu;
            queryInfo.Sku = sku;
            queryInfo.ProductName = productName;
            queryInfo.ProductStatus = productStatus;
            queryInfo.InventoryStatus = inventoryStatus;
            queryInfo.SupplierId = supplierId;
            queryInfo.IsOnSales = isOnSales;
            queryInfo.SalesTerritory = salesTerritory;
            queryInfo.FstCagegoryId = fstCagegoryId;
            queryInfo.SndCagegoryId = sndCagegoryId;
            queryInfo.TrdCagegoryId = trdCagegoryId;

            try
            {
                PageOf<ProductAuditingListModel> dataList = productBll.GetProductList(queryInfo, LanguageEnum.TraditionalChinese, new PagingModel() { PageIndex = PageNo, PageSize = int.MaxValue });

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                Response.AppendHeader("Content-Disposition", "attachment;filename=ProductInfoList" + System.DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                ProductInfotoExcel(dataList.Items);
                Response.End();
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View();

        }

        private void ProductInfotoExcel(IList<ProductAuditingListModel> list)
        {

            #region 导出Excel
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            var rowIndex = 3;
            ExcelHelper excelHelper = new ExcelHelper();
            //大标题
            excelHelper.SetBigTitle(sheet, workbook, "商品管理列表", 11);
            //子标题
            excelHelper.SetSubTitle(sheet, workbook, @"商品SPU編號,商品名稱,商品分類,商家名稱,提交時間,銷售區域,商品SKU編號,商品狀態,庫存狀態,已售數量,是否在售", 2);
            #region 填充数据

            ICellStyle itemStyle = workbook.CreateCellStyle();
            itemStyle.BorderBottom = BorderStyle.Thin;
            itemStyle.BorderLeft = BorderStyle.Thin;
            itemStyle.BorderRight = BorderStyle.Thin;
            itemStyle.BorderTop = BorderStyle.Thin;

            foreach (var entity in list)
            {
                #region
                var listRow = sheet.CreateRow(rowIndex++);
                ICell cell;
                cell = excelHelper.CreateCell(listRow, 0, CellType.String, string.Format("{0}", entity.Spu), itemStyle);
                cell = excelHelper.CreateCell(listRow, 1, CellType.String, string.Format("{0}", entity.ProductName), itemStyle);
                cell = excelHelper.CreateCell(listRow, 2, CellType.String, string.Format("{0}", entity.CategoryName), itemStyle);
                cell = excelHelper.CreateCell(listRow, 3, CellType.String, string.Format("{0}", entity.SupplierName), itemStyle);
                cell = excelHelper.CreateCell(listRow, 4, CellType.String, string.Format("{0}", entity.Createtime), itemStyle);
                cell = excelHelper.CreateCell(listRow, 5, CellType.String, string.Format("{0}", entity.SalesTerritory), itemStyle);
                cell = excelHelper.CreateCell(listRow, 6, CellType.String, string.Format("{0}", entity.Sku), itemStyle);
                cell = excelHelper.CreateCell(listRow, 7, CellType.String, string.Format("{0}", EnumUtils.GetEnumDescription((ProductStatus)entity.Status)), itemStyle);
                cell = excelHelper.CreateCell(listRow, 8, CellType.String, string.Format("{0}", EnumUtils.GetEnumDescription((InventoryStatus)entity.InventoryStatus)), itemStyle);
                cell = excelHelper.CreateCell(listRow, 9, CellType.String, string.Format("{0}", 0), itemStyle);
                string onSale = "";
                if (entity.IsOnSaled)
                {
                    onSale = "是";
                }
                else
                {
                    onSale = "否";
                }
                cell = excelHelper.CreateCell(listRow, 10, CellType.String, string.Format("{0}", onSale), itemStyle);

                #endregion
            }
            excelHelper.AutoColumnWidth(sheet, 11);

            #endregion

            Stream ouputStream = Response.OutputStream;
            workbook.Write(ouputStream);
            ouputStream.Flush();
            ouputStream.Close();
            #endregion
        }


        [HttpPost]
        public JsonResult GetCategoryList(int level, int parentID)
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

        public ActionResult ProductInfoDetail(string spu)
        {
            ProductAuditingViewModel result = new ProductAuditingViewModel();
            ViewBag.SpuId = spu;
            try
            {
                ViewBag.KeyNames = CommonBLL.GetDicsInfoByKey("ProductAttributes");

                result = productBll.GetProductAndCustomInfoOnline(spu);

                if (result == null || result.SpuBaseInfo == null)
                {
                    result.PackingInfo = new ProductPackingModel();
                    result.ProductAttrsInfos = new Dictionary<Models.Enums.LanguageEnum, Dictionary<string, string>>();
                    result.ProductCustomInfos = new List<ProductSkuCustomInfoModel>();
                    result.ProductImgs = new List<ProductImgInfoModel>();
                    result.SpuBaseInfo = new Dictionary<Models.Enums.LanguageEnum, ProductBaseInfoModel>();
                    result.SysInfo = new ProductSysPropertyModel();
                }
                ViewBag.AuditLog = productBll.GetProductAuditLog(spu);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(result);
        }


        public JsonResult SystemOffShelf(string spu, string reason, int status)
        {
            try
            {
                productBll.OffShelfSku(spu, status);
                productBll.InsertProductAuditingLog(spu, status, reason, this.UserName);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
                return Json(new { data = false, msg = "" });
            }
            return Json(new { data = true, msg = "" });
        }

        #endregion

        public ActionResult ShowProductDes(string spu, int languageVersion, bool isOnline)
        {
            try
            {
                if (isOnline == true)
                {
                    ViewBag.Des = productBll.GetProductDesOnline(spu, languageVersion);
                }
                else
                {
                    ViewBag.Des = productBll.GetProductDes(spu, languageVersion);
                }
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View("ShowProductDes");
        }

        /// <summary>
        /// 查看库存
        /// </summary>
        /// <returns></returns>
        [RequirePermission(EnumPermission.Product_InventoryList)]
        public ActionResult InventoryList(InventoryListViewModel Model)
        {
            try
            {
                var pageSetting = new PagingModel() { PageIndex = Model.PageIndex, PageSize = 50 };
                ViewBag.DataList = productBll.GetProductInventoryList(Model, LanguageEnum.TraditionalChinese, pageSetting);
                ViewBag.SupplierNames = supplierBLL.GetSupplierNames();
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }
            return View(Model);
        }

        [RequirePermission(EnumPermission.Product_InventoryList_Export)]
        public FileResult ExportInventoryListToExcel(InventoryListViewModel Model)
        {
            var fileName = "Inventory" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            try
            {
                var dateTable = productBll.GetProductInventoryListData(Model, LanguageEnum.TraditionalChinese);
                var dtExporter = new DataTableExporter(EnumExcelType.XLSX);
                dtExporter.AddTable<InventoryListExportModel>(dateTable, "SKU庫存");
                return XlsxFile(dtExporter.Export(), fileName);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }
            return XlsxFile(new byte[0], fileName);
        }
    }
}