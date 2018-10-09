using SFO2O.Admin.Businesses.Settlement;
using SFO2O.Admin.Common;
using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Settlement;
using SFO2O.Admin.ViewModel.Settlement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFO2O.Admin.Web.Controllers
{
    public class SettlementController : BaseController
    {
        private SettlementBll settlement = new SettlementBll();
        // GET: Settlement
        public ActionResult SettlementIndex(int isFinance = 0)
        {
            var dtNow = DateTime.Now;

            var qm = new SettlementQueryModel()
            {
                CompanyName = Request["CompanyName"] == null ? "" : Request["CompanyName"].ToString(),
                SettlementCode = Request["SettlementCode"] == null ? "" : Request["SettlementCode"].ToString(),
                OrderCode = Request["OrderCode"] == null ? "" : Request["OrderCode"].ToString(),
                EndTime = Request["EndTime"] == null ? dtNow : Convert.ToDateTime(Request["EndTime"].ToString()),
                StartTime = Request["StartTime"] == null ? dtNow.AddMonths(-6) : Convert.ToDateTime(Request["StartTime"].ToString()),
                IsFinance = isFinance
            };

            ViewBag.PageNo = Request["PageNo"] == null ? 1 : Convert.ToInt32(Request["PageNo"].ToString());
            ViewBag.PageSize = Request["PageSize"] == null ? 50 : Convert.ToInt32(Request["PageSize"].ToString());

            return View("SettlementIndex", qm);
        }
        public ActionResult FinanceSettlementIndex()
        {
            return SettlementIndex(1);
        }
        public ActionResult SettlementList(SettlementQueryModel qm)
        {
            PagedList<SettlementOrderInfo> list = null;

            try
            {
                list = settlement.getOrderList(qm, new Models.PagingModel { IsPaging = true, PageIndex = this.PageNo, PageSize = this.PageSize });
                ViewBag.IsFinance = qm.IsFinance;
                ViewBag.SettlementStatus = qm.SettlementStatus;
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(list);
        }
        public ActionResult SettlementDetail(string settlementCode)
        {
            SettlementInfo settlementInfo = null;
            try
            {
                settlementInfo = settlement.getSettlementInfos(settlementCode);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(settlementInfo);
        }
        public ActionResult ExportSettlementList(DateTime startTime, DateTime endTime, string companyName, string settlementCode, string orderCode, int settlementStatus)
        {
            var dtNow = DateTime.Now;
            PagedList<SettlementOrderInfo> list = null;

            var qm = new SettlementQueryModel()
            {
                CompanyName = string.IsNullOrEmpty(companyName) ? "" : companyName,
                SettlementCode = string.IsNullOrEmpty(settlementCode) ? "" : settlementCode,
                OrderCode = string.IsNullOrEmpty(orderCode) ? "" : orderCode,
                EndTime = endTime == null ? dtNow : endTime,
                StartTime = startTime == null ? dtNow.AddMonths(-6) : startTime,
                SettlementStatus = settlementStatus == 0 ? -1 : settlementStatus
            };
            try
            {
                list = settlement.getOrderList(qm, new Models.PagingModel { IsPaging = false, PageIndex = this.PageNo, PageSize = this.PageSize });
                var dt = new DataTable();
                dt.Columns.Add("結算單號");
                dt.Columns.Add("生成時間");
                dt.Columns.Add("訂單號/結算單號");
                dt.Columns.Add("結算方");
                dt.Columns.Add("商品總金額");
                dt.Columns.Add("商品退款金額");
                dt.Columns.Add("商家承擔關稅");
                dt.Columns.Add("平台承擔關稅");
                dt.Columns.Add("商家承擔商品促銷費用");
                dt.Columns.Add("平台承擔商品促銷費用");
                dt.Columns.Add("其他費用");
                dt.Columns.Add("結算金額");
                dt.Columns.Add("狀態");
                list.ContentList.ForEach(l =>
                {
                    dt.Rows.Add(new object[]{
                    l.SettlementCode,
                    l.CreateTime.ToString("yyyy-MM-dd"),
                    l.OrderCode ,
                    l.CompanyName,
                    "$"+l.ProductTotalAmount,
                    "$"+l.ProductRefundAmount,
                    "$"+l.SupplierBearDutyAmount,
                    "$"+l.BearDutyAmount,
                    "$"+l.SupplierPromotionDutyAmount.ToString("f2"),
                    "$"+l.PromotionDutyAmount,
                    "$"+l.OtherAmount,
                    "$"+(l.SettlementAmount - l.SupplierPromotionDutyAmount).ToString("f2"),
                    SFO2O.Admin.Common.EnumUtils.GetEnumDescription((SFO2O.Admin.Models.SettlementStatus)l.SettlementStatus)
                    });
                });
                ExcelHelper.ExportByWeb(dt, "結算單列表", "settlementlist" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View();
        }

        public JsonResult ConfirmSettlement(string settlementCode, decimal rmbProductSettlementAmount, decimal rmbSupplierBearDutyAmount, decimal rmbDutyAmount, decimal dutyAmount, decimal exchangeRate, decimal otherAmount)
        {
            var rmbSettlementAmount = rmbProductSettlementAmount - rmbSupplierBearDutyAmount - otherAmount * exchangeRate;
            var settlementAmount = (rmbProductSettlementAmount - rmbSupplierBearDutyAmount) / exchangeRate - otherAmount;
            var supplierBearDutyAmount = rmbSupplierBearDutyAmount / exchangeRate;
            var rmbBearDutyAmount = rmbDutyAmount - rmbSupplierBearDutyAmount;
            var bearDutyAmount = dutyAmount - supplierBearDutyAmount;
            var rmbOtherAmount = otherAmount / exchangeRate;
            var result = settlement.auditOrder(settlementCode, settlementAmount, rmbSettlementAmount, supplierBearDutyAmount, rmbSupplierBearDutyAmount, bearDutyAmount, rmbBearDutyAmount, otherAmount, rmbOtherAmount, DateTime.Now, UserName);
            var resultJson = new { IsOk = true, Msg = "操作成功" };
            if (result == 0)
                resultJson = new { IsOk = false, Msg = "操作失敗！" };
            return Json(resultJson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SettlementPay(string settlementCodes, DateTime settlementTime, int payPlatform, string tradeCode, decimal settlementAmount)
        {
            var result = settlement.payOrder(settlementCodes, settlementTime, payPlatform, tradeCode, settlementAmount, UserName);
            var resultJson = new { IsOk = false, Msg = "操作失敗！" };

            if (result == 1)
                resultJson = new { IsOk = true, Msg = "操作成功" };
            return Json(resultJson, JsonRequestBehavior.AllowGet);
        }
    }
}