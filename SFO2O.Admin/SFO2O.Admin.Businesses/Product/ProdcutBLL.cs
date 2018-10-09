using SFO2O.Admin.Common;
using SFO2O.Admin.DAO.Product;
using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Enums;
using SFO2O.Admin.Models.Product;
using SFO2O.Admin.ViewModel;
using SFO2O.Admin.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Businesses.Product
{
    public class ProdcutBLL
    {
        private readonly ProductDAL productDAL = new ProductDAL();

        public PageOf<ProductAuditingListModel> GetAuditingProductList(int pageSize, int pageIndex, int languageVersion, ProductAuditingQuyModel queryModel)
        {
            return productDAL.GetAuditingProductList(pageSize, pageIndex, languageVersion, queryModel);
        }

        public PageOf<ProductExportModel> AuditingProductsExport(int languageVersion, ProductAuditingQuyModel queryModel)
        {
            return productDAL.AuditingProductsExport(languageVersion, queryModel);
        }

        public ProductAuditingViewModel GetProductAndCustomInfo(string spu)
        {
            var pcInfo = productDAL.GetProductAndCustomInfo(spu);

            if (pcInfo == null && pcInfo.ProductBaseInfos == null && pcInfo.ProductBaseInfos.Count == 0)
            {
                return new ProductAuditingViewModel();
            }

            return ProductAuditingModelToProductAuditingViewModel(pcInfo);
        }

        public ProductAuditingViewModel GetProductAndCustomInfoOnline(string spu)
        {
            var pcInfo = productDAL.GetProductAndCustomInfoOnline(spu);

            if (pcInfo == null && pcInfo.ProductBaseInfos == null && pcInfo.ProductBaseInfos.Count == 0)
            {
                return new ProductAuditingViewModel();
            }

            return ProductAuditingModelToProductAuditingViewModel(pcInfo);
        }

        private ProductAuditingViewModel ProductAuditingModelToProductAuditingViewModel(ProductAuditingModel proAuditModel)
        {
            if (proAuditModel == null && proAuditModel.ProductBaseInfos == null && proAuditModel.ProductBaseInfos.Count == 0)
            {
                return new ProductAuditingViewModel();
            }

            var result = new ProductAuditingViewModel();

            if (proAuditModel.ProductBaseInfos != null && proAuditModel.ProductBaseInfos.Count > 0)
            {
                result.SpuBaseInfo = new Dictionary<LanguageEnum, ProductBaseInfoModel>();

                foreach (var bi in proAuditModel.ProductBaseInfos)
                {
                    if (result.SpuBaseInfo.ContainsKey((LanguageEnum)bi.LanguageVersion))
                    {
                        result.SpuBaseInfo[(LanguageEnum)bi.LanguageVersion] = bi;
                    }
                    else
                    {
                        result.SpuBaseInfo.Add((LanguageEnum)bi.LanguageVersion, bi);
                    }
                }
            }

            if (proAuditModel.ProductSysInfo != null)
            {
                var sysInfo = proAuditModel.ProductSysInfo;

                result.PackingInfo = new ProductPackingModel()
                {
                    Height = sysInfo.Height,
                    Length = sysInfo.Length,
                    Volume = sysInfo.Volume,
                    Weight = sysInfo.Weight,
                    Width = sysInfo.Width
                };

                result.SysInfo = new ProductSysPropertyModel()
                {
                    CategoryName = sysInfo.CategoryName,
                    CommissionInCHINA = sysInfo.CommissionInCHINA,
                    CommissionInHK = sysInfo.CommissionInHK,

                    CompanyName = sysInfo.CompanyName,
                    IsExchangeInCHINA = sysInfo.IsExchangeInCHINA,
                    IsExchangeInHK = sysInfo.IsExchangeInHK,

                    IsReturn = sysInfo.IsReturn,
                    MinForOrder = sysInfo.MinForOrder,
                    ModifyTime = sysInfo.ModifyTime,

                    PreOnSaleTime = sysInfo.PreOnSaleTime,
                    SalesTerritory = sysInfo.SalesTerritory,
                    Spu = sysInfo.Spu,
                };
            }

            if (proAuditModel.ProductCustomInfos != null && proAuditModel.ProductCustomInfos.Count > 0)
            {
                result.ProductCustomInfos = proAuditModel.ProductCustomInfos;
            }

            if (proAuditModel.ProductAttrsInfos != null && proAuditModel.ProductAttrsInfos.Count > 0)
            {
                result.ProductAttrsInfos = new Dictionary<LanguageEnum, Dictionary<string, string>>();

                foreach (var pa in proAuditModel.ProductAttrsInfos)
                {
                    var dic = ReflectionHelper.ReflectionKeyValue<ProductAttrInfoModel>(pa);

                    if (!result.ProductAttrsInfos.Keys.Contains((LanguageEnum)pa.LanguageVersion))
                    {
                        result.ProductAttrsInfos.Add((LanguageEnum)pa.LanguageVersion, dic);
                    }
                    else
                    {
                        var value = result.ProductAttrsInfos[(LanguageEnum)pa.LanguageVersion];
                        var keyCount = value.Keys.Count();

                        foreach (var key in value.Keys.ToArray())
                        {
                            var tempValue = value[key];
                            var dicValue = dic[key];
                            var valueTmpList = value[key].ToString().Split(',').ToList();
                            if (!String.IsNullOrWhiteSpace(dicValue) && !valueTmpList.Contains(dicValue))
                            {
                                tempValue = tempValue + "," + dicValue;
                                value[key] = tempValue;
                            }
                        }
                    }
                }
            }

            if (proAuditModel.ProductImgs != null && proAuditModel.ProductImgs.Count > 0)
            {
                result.ProductImgs = proAuditModel.ProductImgs;
            }

            return result;
        }

        public void UpDateSpuStatus(string spu, int status)
        {
            var modifyStatus = status == 1 ? -3 : 2;

            productDAL.UpDateSpuStatus(spu, modifyStatus);
        }

        public void OffShelfSku(string spu, int status)
        {
            productDAL.OffShelfSku(spu, status);
        }

        public void InsertProductAuditingLog(string spu, int status, string reason, string createBy)
        {
            productDAL.InsertProductAuditingLog(spu, status, reason, createBy);
        }

        public void InsertTemStorageCusReports(CustomReportJsonModel model, bool isChangeRS)
        {
            productDAL.InsertTemStorageCusReports(model);
            if (isChangeRS == true)
            {
                productDAL.UpdateSkuReportStatus(model);
            }
        }

        public Dictionary<string, SkuReportStatusModel> GetAuditingSkuInfos()
        {
            return productDAL.GetAuditingSkuInfos();
        }

        public Dictionary<string, int> ImportCustomReports(Dictionary<string, SkuReportStatusModel> skus, DataTable excelInfos)
        {
            var result = new Dictionary<string, int>();
            var successCount = 0;
            var failCount = 0;

            if (skus == null || skus.Count == 0)
            {
                return null;
            }

            var skuDic = new Dictionary<string, int>();

            List<CustomReport> importData = new List<CustomReport>();

            Dictionary<string, string> fails = new Dictionary<string, string>();
            List<int> customedSkuLineIDs = new List<int>();

            foreach (DataRow dr in excelInfos.Rows)
            {
                var skuNo = dr[2] == DBNull.Value ? "" : dr[2].ToString();

                if (String.IsNullOrWhiteSpace(skuNo))
                {
                    continue;
                }

                if (!skus.Keys.Contains(skuNo) && !fails.Keys.Contains(skuNo))
                {
                    failCount++;
                    fails.Add(skuNo, "待審核列表中沒有與之匹配的SKU編號");
                    continue;
                }

                var skuInfo = skus[skuNo];

                if (skuInfo.ReportStatus == -1 && !fails.Keys.Contains(skuNo))
                {
                    failCount++;
                    fails.Add(skuNo, "對應的SKU編號不需要報備");
                    continue;
                }

                successCount++;

                var customReport = new CustomReport()
                {
                    Sku = skuNo,
                    CustomsUnit = dr[3] == DBNull.Value ? "" : dr[3].ToString(),
                    ModelForCustoms = dr[4] == DBNull.Value ? "" : dr[4].ToString(),
                    InspectionNo = dr[5] == DBNull.Value ? "" : dr[5].ToString(),
                    PrepardNo = dr[6] == DBNull.Value ? "" : dr[6].ToString(),
                    TaxCode = dr[7] == DBNull.Value ? "" : dr[7].ToString(),
                    HSCode = dr[8] == DBNull.Value ? "" : dr[8].ToString(),
                    GnoCode = dr[9] == DBNull.Value ? "" : dr[9].ToString(),
                    UOM = dr[10] == DBNull.Value ? "" : dr[10].ToString(),
                    TaxRate = dr[11] == DBNull.Value ? "0" : dr[11].ToString(),
                };

                if (HasCustom(customReport))
                {
                    customedSkuLineIDs.Add(skuInfo.Id);
                    customedSkuLineIDs.AddRange(skuInfo.ConnectIDs);
                }

                importData.Add(customReport);

            }

            var jm = new CustomReportJsonModel();
            jm.CustomReports = importData.ToArray();

            productDAL.InsertTemStorageCusReports(jm);
            productDAL.UpdateSkuReportStatus(customedSkuLineIDs);

            var batchNO = productDAL.InsertImportError(fails);

            result.Add("true", successCount);
            result.Add("false", failCount);
            result.Add("batchNO", batchNO);

            return result;
        }

        /// <summary>
        /// 是否已报备
        /// </summary>
        /// <param name="cr"></param>
        /// <returns></returns>
        private bool HasCustom(CustomReport cr)
        {
            var t = new CustomReport();
            Type type = t.GetType();

            bool hasCustom = true;

            foreach (var property in type.GetProperties())
            {
                if (property.GetValue(cr, null) == null)
                {
                    hasCustom = false;
                    break;
                }
            }
            return hasCustom;
        }

        public List<ImportErrorModel> GetImportErrors(int batchNo)
        {
            return productDAL.GetImportErrors(batchNo);
        }

        public PageOf<ProductAuditingListModel> GetProductList(ProductAuditingQuyModel queryInfo, LanguageEnum languageEnum, Models.PagingModel page)
        {
            return productDAL.GetProductList(queryInfo, languageEnum, page);
        }

        public string GetProductDes(string spu, int languageVersion)
        {
            return productDAL.GetProductDes(spu, languageVersion);
        }

        public string GetProductDesOnline(string spu, int languageVersion)
        {
            return productDAL.GetProductDesOnline(spu, languageVersion);
        }

        #region 审核通过传输数据

        public bool TransferProduct(string spu)
        {
            var transInfo = GetImportData(spu);

            if (transInfo == null || transInfo.ProductInfo == null || transInfo.ProductInfo.Count == 0)
            {
                return false;
            }

            if (transInfo.ProductInfo[0].DataSource == 1)
            {
                productDAL.TransferNewSpu(transInfo);
            }
            else
            {
                productDAL.TransferOldSpu(transInfo);
            }

            return true;
        }

        private ImportSpuModel GetImportData(string spu)
        {
            List<ProductAndExpandInfo> prouductInfoSingle = productDAL.GetProductAndExpendInfo(spu);

            List<ProductAndExpandInfo> prouductInfos =new List<ProductAndExpandInfo>();

            prouductInfos.Add(prouductInfoSingle.FirstOrDefault());
            if (prouductInfos == null || prouductInfos.Count() == 0)
            {
                return null;
            }

            var spuIds = prouductInfos.Select(p => p.SpuId).ToList();

            var skuInfos = productDAL.GetSkuInfos(spuIds);

            if (skuInfos == null || skuInfos.Count() == 0)
            {
                return null;
            }

            var imgInfos = productDAL.GetSpuImgInfos(new List<string>() { spu });

            var skus = new List<string>();

            foreach (var sku in skuInfos)
            {
                if (!skus.Contains(sku.Sku))
                {
                    skus.Add(sku.Sku);
                }
            }

            var customInfos = productDAL.GetSkuCustomInfos(skus);

            var result = new ImportSpuModel()
            {
                ProductInfo = prouductInfos,
                SkuInfos = skuInfos,
                Imgs = imgInfos,
                CustomInfos = customInfos
            };

            return result;
        }



        #endregion

        public List<ProductAuditingLog> GetProductAuditLog(string spu)
        {
            return productDAL.ProductAuditingLog(spu);
        }

        public PageOf<SkuInfo> GetProductInventoryList(InventoryListViewModel queryModel, LanguageEnum languageEnum, Models.PagingModel page)
        {
            var ds = productDAL.GetProductInventoryList(queryModel, languageEnum, page);
            return new PageOf<SkuInfo>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                RowCount = Convert.ToInt32(ds.Tables[1].Rows[0][1]),
                Items = DataMapHelper.DataSetToList<SkuInfo>(ds)
            };
        }

        public DataTable GetProductInventoryListData(InventoryListViewModel queryModel, LanguageEnum languageEnum)
        {
            var page = new PagingModel() { PageIndex = 1, PageSize = int.MaxValue };
            var ds = productDAL.GetProductInventoryList(queryModel, languageEnum, page);
            return ds.Tables[0];
        }
    }
}
