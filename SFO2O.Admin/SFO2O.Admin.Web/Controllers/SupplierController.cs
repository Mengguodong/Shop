using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SFO2O.Admin.Businesses.Supplier;
using SFO2O.Admin.Common;
using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Enums;
using SFO2O.Admin.Models.Supplier;
using SFO2O.Admin.ViewModel;
using SFO2O.Admin.ViewModel.Supplier;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFO2O.Admin.Web.Controllers
{
    public class SupplierController : BaseController
    {
        private SupplierBLL supplierBLL = new SupplierBLL();
        IList<string> allowFileExts = new List<string> { ".jpg", ".gif", ".png", ".jpeg" }.AsReadOnly();

        //
        // GET: /Supplier/
        public ActionResult SupplierQuery()
        {
            var dtNow = DateTime.Now;

            var qm = new SupplierQueryModel()
            {
                AccountName = Request["AccountName"] == null ? "" : Request["AccountName"].ToString(),
                CompanyName = Request["CompanyName"] == null ? "" : Request["CompanyName"].ToString(),
                CreateTimeEnd = Request["EndTime"] == null ? dtNow : Convert.ToDateTime(Request["EndTime"].ToString()),
                CreateTimeStart = Request["StartTime"] == null ? dtNow.AddMonths(-6) : Convert.ToDateTime(Request["StartTime"].ToString()),
                SupplierStatus = Request["SupplierStatus"] == null ? 0 : Convert.ToInt32(Request["SupplierStatus"].ToString()),
            };

            ViewBag.PageNo = Request["PageNo"] == null ? 1 : Convert.ToInt32(Request["PageNo"].ToString());
            ViewBag.PageSize = Request["PageSize"] == null ? 50 : Convert.ToInt32(Request["PageSize"].ToString());

            return View(qm);
        }

        public ActionResult SupplierQueryList(SupplierQueryModel qm)
        {
            PageOf<SupplierAbstractModel> suppliers = null;

            try
            {
                suppliers = supplierBLL.GetSupplierList(qm, this.PageSize, this.PageNo);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(suppliers);
        }

        public ActionResult ExportSupplierQueryList(DateTime startTime, DateTime endTime, string accountName, string companyName, int supplierStatus)
        {
            var dtNow = DateTime.Now;

            SupplierQueryModel qm = new SupplierQueryModel()
            {
                AccountName = accountName,
                CompanyName = companyName,
                CreateTimeEnd = endTime,
                CreateTimeStart = startTime,
                SupplierStatus = supplierStatus,
            };

            PageOf<SupplierAbstractModel> suppliers = null;

            try
            {
                suppliers = supplierBLL.GetSupplierList(qm, int.MaxValue, 1);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                Response.AppendHeader("Content-Disposition", "attachment;filename=supplierlist" + System.DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                SupplierQueryListToExcel(suppliers.Items);
                Response.End();
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View();
        }

        private void SupplierQueryListToExcel(IList<SupplierAbstractModel> list)
        {
            #region 导出Excel
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            var rowIndex = 3;
            ExcelHelper excelHelper = new ExcelHelper();
            //大标题
            excelHelper.SetBigTitle(sheet, workbook, "商家列表", 9);
            //子标题
            excelHelper.SetSubTitle(sheet, workbook, "商家賬號,公司名稱,入駐時間,商家狀態,在售SKU數量", 2);
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
                cell = excelHelper.CreateCell(listRow, 0, CellType.String, string.Format("{0}", entity.UserName), itemStyle);
                cell = excelHelper.CreateCell(listRow, 1, CellType.String, string.Format("{0}", entity.CompanyName), itemStyle);
                cell = excelHelper.CreateCell(listRow, 2, CellType.String, string.Format("{0}", FormatDateTime.ToDateTimeMM(entity.CreateTime)), itemStyle);
                cell = excelHelper.CreateCell(listRow, 3, CellType.String, string.Format("{0}", entity.SupplierStatus), itemStyle);
                cell = excelHelper.CreateCell(listRow, 4, CellType.String, string.Format("{0}", entity.SkuNumber), itemStyle);
                #endregion
            }
            excelHelper.AutoColumnWidth(sheet, 15);

            #endregion

            Stream ouputStream = Response.OutputStream;
            workbook.Write(ouputStream);
            ouputStream.Flush();
            ouputStream.Close();
            #endregion
        }

        public ActionResult ShowSupplierDetail(int supplierId)
        {
            var supplierDetailModel = new SupplierDetailModel();

            try
            {
                supplierDetailModel = supplierBLL.GetSupplierDetailInfo(supplierId);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(supplierDetailModel);
        }

        public ActionResult EditSupplier(int supplierId = 0)
        {
            var supplierDetail = new SupplierDetailModel();

            try
            {
                if (supplierId > 0)
                {
                    supplierDetail = supplierBLL.GetSupplierDetailInfo(supplierId);

                    if (supplierDetail != null && supplierDetail.Supplier != null)
                    {
                        supplierDetail.Supplier.ImgPath = supplierDetail.Supplier.StoreLogoPath;
                        supplierDetail.Supplier.StoreLogoPath = (ConfigHelper.ImageServer + supplierDetail.Supplier.StoreLogoPath).Replace("\\", "//");
                    }
                }

            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(supplierDetail);
        }

        public ActionResult SaveSupplier(string json)
        {
            var isSuccess = true;

            try
            {
                var supplierInfo = JsonHelper.ToObject<SupplierInfoJsonModel>(json);

                if (supplierInfo == null)
                {
                    isSuccess = false;
                }

                if (supplierInfo.SupplierID <= 0)
                {
                    supplierBLL.InsertSupplierInfo(supplierInfo, this.UserName);
                }
                else
                {
                    supplierBLL.UpdateSupplierInfo(supplierInfo, this.UserName);
                }

            }
            catch (Exception ext)
            {
                isSuccess = false;
                LogHelper.Error(ext);
            }

            return Json(new { Success = isSuccess });
        }

        public ActionResult BrandView(int brandId = 0)
        {
            try
            {
                BrandModel model = supplierBLL.GetBrandById(brandId);

                PageOf<StoreModel> storeList = supplierBLL.GetStoreListByBrandId(brandId, 0, LanguageEnum.TraditionalChinese, int.MaxValue, this.PageNo);

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


        public ActionResult CheckOnlyEmail(string email)
        {
            var emailCount = 0;

            try
            {
                if (String.IsNullOrWhiteSpace(email))
                {
                    emailCount = 1;
                }

                emailCount = supplierBLL.GetUserNameCount(email);

            }
            catch (Exception ext)
            {
                emailCount = 1;
                LogHelper.Error(ext);
            }

            return Json(new { EmailCount = emailCount });
        }

        public ActionResult CheckOnlyCompany(string companyName)
        {
            var companyCount = 0;

            try
            {
                if (String.IsNullOrWhiteSpace(companyName))
                {
                    companyCount = 1;
                }

                companyCount = supplierBLL.GetCompanyNameCount(companyName);

            }
            catch (Exception ext)
            {
                companyCount = 1;
                LogHelper.Error(ext);
            }

            return Json(new { CompanyCount = companyCount });
        }

        #region 保存图片

        public string UploadSupplierLogo()
        {
            HttpPostedFileBase fileData = Request.Files[0];
            int newImgWidth = 0;
            int newImgHeight = 0;


            if (!fileData.HasFile())
            {
                return JsonHelper.ToJson(new ImageView() { Error = 1, Message = "上載的圖片无效！" });
            }
            if (fileData.ContentLength > (512 * 1024))
            {
                return JsonHelper.ToJson(new ImageView() { Error = 1, Message = "上載的圖片尺寸不能超过500KB！" });
            }

            string uploadFileName = fileData.FileName;
            string imageExtension = Path.GetExtension(uploadFileName);

            if (!ValidateImageExtension(imageExtension))
            {
                return JsonHelper.ToJson(new ImageView() { Error = 1, Message = "不允許上載“" + imageExtension + "”格式的圖片！" });
            }


            string saveFilePath = string.Empty;
            string saveFileDir = ConfigHelper.SharePath + PathHelper.GetSavePathImg();

            if (!Directory.Exists(saveFileDir))
                Directory.CreateDirectory(saveFileDir);

            saveFilePath = saveFileDir + StringHelper.GetRandomString(12) + imageExtension;
            fileData.SaveAs(saveFilePath);

            Bitmap aImg = new Bitmap(saveFilePath);
            Bitmap b = LTImage.ResizeImage(aImg, 300, 300);
            newImgWidth = b.Width;
            newImgHeight = b.Height;

            if (fileData.ContentLength > (1024 * 500))//如果大于500k，那么就需要重新压缩图片了
            {
                string saveFileDirNew = ConfigHelper.SharePath + PathHelper.GetSavePathImg();

                string saveFilePathNew = saveFileDirNew + StringHelper.GetRandomString(12) + imageExtension;

                ReCompressImage(saveFilePath, saveFilePathNew);

                saveFilePath = saveFilePathNew;
            }

            string saveImg200 = saveFilePath.Replace(imageExtension, "_218" + imageExtension);
            SaveProductImageSizeOld(saveFilePath.Replace(ConfigHelper.SharePath, ""), saveImg200, 218, 218);


            ImageView result = new ImageView()
            {
                Url = saveFilePath.Replace(ConfigHelper.SharePath, ConfigHelper.ImageServer).Replace("\\", "//"),
                Path = saveFilePath.Replace(ConfigHelper.SharePath, ""),
            };


            return JsonHelper.ToJson(result);

        }

        private bool ValidateImageExtension(string imageExtension)
        {
            foreach (var ext in allowFileExts)
            {
                if (string.Equals(imageExtension, ext, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private void ReCompressImage(string imagePath, string imageSave)
        {
            ImageHelper.ZoomSmallPic(imagePath, imageSave, int.MaxValue, int.MaxValue);
        }

        private void SaveProductImageSizeOld(string productImagePath, string savePath = "", int width = 100,
                                            int height = 100)
        {
            string strFileDir = Path.GetDirectoryName(productImagePath);
            var fileName = Path.GetFileName(productImagePath);

            if (fileName != null)
            {
                string[] strFielNameExtName = fileName.Split('.');

                //同路径下，保存100*100的小图
                string productImagePath100 = ConfigHelper.SharePath + strFileDir + "\\" + strFielNameExtName[0] +
                                             "_100." + strFielNameExtName[1];
                if (!string.IsNullOrEmpty(savePath))
                    productImagePath100 = savePath;

                ImageHelper.ZoomSmallPic(ConfigHelper.SharePath + productImagePath, productImagePath100, width, height);
            }
        }

        #endregion

        public void UpdateSupplierStatus(int supplierId, int status)
        {
            try
            {
                supplierBLL.UpdateSupplierStatus(supplierId, status, this.UserName);

            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }
        }
    }
}