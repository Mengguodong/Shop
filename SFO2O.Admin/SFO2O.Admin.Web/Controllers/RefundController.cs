using SFO2O.Admin.Businesses.Refund;
using SFO2O.Admin.Businesses.Supplier;
using SFO2O.Admin.Common;
using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Refund;
using SFO2O.Admin.ViewModel.Refund;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFO2O.Admin.Web.Controllers
{
    public class RefundController : BaseController
    {
        private RefundBll refund = new RefundBll();
        // GET: Refund
        public ActionResult RefundIndex(int isFinance = 0)
        {
            var dtNow = DateTime.Now;
            ViewBag.SallerNames = new SupplierBLL().GetSupplierNames();

            var qm = new RefundQueryModel()
            {
                Sku = Request["Sku"] == null ? "" : Request["Sku"].ToString(),
                RegionCode = Request["RegionCode"] == null ? 1 : Convert.ToInt32(Request["RegionCode"].ToString()),
                RefundType = Request["RefundType"] == null ? -1 : Convert.ToInt32(Request["RefundType"].ToString()),
                RefundStatus = Request["RefundStatus"] == null ? -1 : Convert.ToInt32(Request["RefundStatus"].ToString()),
                RefundCode = Request["RefundCode"] == null ? "" : Request["RefundCode"].ToString(),
                BuyerName = Request["BuyerName"] == null ? "" : Request["BuyerName"].ToString(),
                SellerName = Request["SellerName"] == null ? 0 : Convert.ToInt32(Request["SellerName"].ToString()),
                OrderCode = Request["OrderCode"] == null ? "" : Request["OrderCode"].ToString(),
                EndTime = Request["EndTime"] == null ? dtNow : Convert.ToDateTime(Request["EndTime"].ToString()),
                StartTime = Request["StartTime"] == null ? dtNow.AddMonths(-6) : Convert.ToDateTime(Request["StartTime"].ToString()),
                IsFinance = isFinance
            };

            ViewBag.PageNo = Request["PageNo"] == null ? 1 : Convert.ToInt32(Request["PageNo"].ToString());
            ViewBag.PageSize = Request["PageSize"] == null ? 50 : Convert.ToInt32(Request["PageSize"].ToString());

            return View("RefundIndex", qm);
        }

        public ActionResult FinanceRefundIndex()
        {
            return RefundIndex(1);
        }
        public ActionResult RefundList(RefundQueryModel qm)
        {
            PagedList<RefundOrderListInfo> list = null;

            try
            {
                list = refund.getOrderList(qm, new Models.PagingModel { IsPaging = true, PageIndex = this.PageNo, PageSize = this.PageSize });
                ViewBag.IsFinance = qm.IsFinance;
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(list);
        }
        public ActionResult ExportRefundList(DateTime startTime, DateTime endTime, string sku, string refundCode, string buyerName, string sellerName, string orderCode, int refundType, int refundStatus, int regionCode, int isFinance = 0)
        {
            var dtNow = DateTime.Now;
            PagedList<RefundOrderListInfo> list = null;


            var qm = new RefundQueryModel()
            {
                Sku = string.IsNullOrEmpty(sku) ? "" : sku,
                RegionCode = regionCode == 0 ? 1 : regionCode,
                RefundType = refundType == 0 ? -1 : refundType,
                RefundStatus = refundStatus == 0 ? -1 : refundStatus,
                RefundCode = string.IsNullOrEmpty(refundCode) ? "" : refundCode,
                BuyerName = string.IsNullOrEmpty(buyerName) ? "" : buyerName,
                SellerName = string.IsNullOrEmpty(sellerName) ? 0 : Convert.ToInt32(sellerName),
                OrderCode = string.IsNullOrEmpty(orderCode) ? "" : orderCode,
                EndTime = endTime == null ? dtNow : endTime,
                StartTime = startTime == null ? dtNow.AddMonths(-6) : startTime,
                IsFinance = isFinance
            };
            try
            {
                list = refund.getOrderList(qm, new Models.PagingModel { IsPaging = false, PageIndex = this.PageNo, PageSize = this.PageSize });
                var dt = new DataTable();
                dt.Columns.Add("訂單號");
                dt.Columns.Add("買家");
                dt.Columns.Add("退款單號");
                dt.Columns.Add("生成時間");
                dt.Columns.Add("退款類型");
                dt.Columns.Add("賣家賬號");
                dt.Columns.Add("SKU");
                dt.Columns.Add("商品");
                dt.Columns.Add("商品金額");
                dt.Columns.Add("關稅");
                dt.Columns.Add("退款金額");
                dt.Columns.Add("狀態");
                list.ContentList.ForEach(l =>
                {
                    var attr = " ";
                    if (!string.IsNullOrEmpty(l.MainDicValue))
                    {
                        attr += l.MainDicValue + "：" + l.MainValue + " ";
                    }
                    if (!string.IsNullOrEmpty(l.SubDicValue))
                    {
                        attr += l.SubDicValue + "：" + l.SubValue + " ";
                    }
                    dt.Rows.Add(new object[]{
                    l.OrderCode,
                    l.BuyerName,
                    l.RefundCode,
                    l.CreateTime.ToString("yyyy-MM-dd"),
                    SFO2O.Admin.Common.EnumUtils.GetEnumDescription((SFO2O.Admin.Models.RefundType)l.RefundType),
                    l.SellerName,
                    l.Sku,
                    l.ProductName + attr,
                    "￥"+l.RmbProductAmount,
                    "￥"+l.RmbDutyAmount,
                    "￥"+l.RmbTotalAmount,                   
                    SFO2O.Admin.Common.EnumUtils.GetEnumDescription((SFO2O.Admin.Models.RefundStatus)l.RefundStatus)
                    });
                });
                ExcelHelper.ExportByWeb(dt, "退款單列表", "refundlist" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View();
        }

        public ActionResult RefundDetail(string refundCode, int isAudit = 0)
        {
            RefundDetailInfo refundInfo = null;
            try
            {
                refundInfo = refund.getRefundDetail(refundCode);
                refundInfo.IsAudit = isAudit;
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View("RefundDetail", refundInfo);
        }
        public ActionResult AuditRefundDetail(string refundCode)
        {
            return RefundDetail(refundCode, 1);
        }
        public JsonResult RefuseRefund(string refundCode, string refuseReason)
        {
            var result = refund.refuseRefund(refundCode, refuseReason, DateTime.Now, this.UserName);
            var resultJson = new { IsOk = true, Msg = "操作成功" };
            if (result == 0)
                resultJson = new { IsOk = false, Msg = "操作失敗！" };
            return Json(resultJson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AgreeRefund(string refundCode, int isQualityProblem, int refundType, int isReturnDuty, decimal rmbTotalAmount, decimal totalAmount, decimal rmbDutyAmount, decimal dutyAmount, string collectionCode, DateTime tobePickUpTime)
        {
            var order = new RefundOrderInfo();
            order.IsQualityProblem = isQualityProblem;
            order.RefundType = refundType;
            order.IsReturnDuty = isReturnDuty;
            order.RmbTotalAmount = rmbTotalAmount;
            order.TotalAmount = totalAmount;
            order.RmbDutyAmount = isReturnDuty == 1 ? rmbDutyAmount : 0;
            order.DutyAmount = isReturnDuty == 1 ? dutyAmount : 0;
            order.CollectionCode = collectionCode;
            order.TobePickUpTime = tobePickUpTime;
            order.AuditTime = DateTime.Now;
            order.Auditor = UserName;
            order.RefundStatus = refundType == 1 ? 2 : 3;
            order.RefundCode = refundCode;
            var result = refund.agreeRefund(order);
            var resultJson = new { IsOk = true, Msg = "操作成功" };
            if (result == 0)
                resultJson = new { IsOk = false, Msg = "操作失敗！" };
            return Json(resultJson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RefundPickUp(string refundCode, string expressCompany, string expressList)
        {
            var result = refund.refundPickUp(refundCode, expressCompany, expressList, DateTime.Now, UserName);
            var resultJson = new { IsOk = true, Msg = "操作成功" };
            if (result == 0)
                resultJson = new { IsOk = false, Msg = "操作失敗！" };
            return Json(resultJson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnRefund(string refundCode, decimal rmbTotalAmount, int payPlatform, DateTime settlementTime, string tradeCode)
        {
            var result = refund.returnRefund(refundCode, rmbTotalAmount, payPlatform, settlementTime, tradeCode, UserName);
            var resultJson = new { IsOk = true, Msg = "操作成功" };
            if (result == 0)
                resultJson = new { IsOk = false, Msg = "操作失敗！" };
            return Json(resultJson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RefundPaymentInfo(string refundCode)
        {
            var result = refund.getRefundPaymentInfo(refundCode);
            var resultJson = new { IsOk = false, PayInfo = result, Msg = "操作失敗！" };

            if (result != null)
                resultJson = new { IsOk = true, PayInfo = result, Msg = "操作成功" };
            return Json(resultJson, JsonRequestBehavior.AllowGet);
        }
    }
}