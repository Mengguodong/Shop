using SFO2O.Supplier.Businesses.Order;
using SFO2O.Supplier.Common;
using SFO2O.Supplier.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFO2O.Supplier.Web.Controllers
{
    public class RefundController : BaseController
    {
        RefundBLL bll = new RefundBLL();

        #region 退款单管理

        public ActionResult RefundList(string startTime, string endTime, string orderCode, string refundCode, int refundType = 0, int receiptCountry = 0, int refundStatus = -1, int page = 1)
        {
            RefundQueryInfo queryInfo = new RefundQueryInfo();

            if (string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
            {
                queryInfo.startTime = DateTime.Now.AddMonths(-3);
                queryInfo.endTime = DateTime.Now;
            }
            else
            {
                queryInfo.startTime = DateTime.Parse(startTime);
                queryInfo.endTime = DateTime.Parse(endTime);
            }
            queryInfo.orderCode = orderCode;
            queryInfo.refundCode = refundCode;
            queryInfo.refundStatus = refundStatus;
            queryInfo.SupplierId = CurrentUser.SupplierID;

            ViewBag.QueryInfo = queryInfo;
            ViewBag.ReceiptCountry = receiptCountry;
            ViewBag.Page = page;

            try
            {
                if (receiptCountry == 1)
                {
                    ViewBag.ChinaRefund = bll.GetRefundOrderList(queryInfo, new PageDTO() { PageIndex = PageNo, PageSize = 20 }, LanguageEnum.TraditionalChinese, 1);

                    ViewBag.HKRefund = bll.GetRefundOrderList(queryInfo, new PageDTO() { PageIndex = page, PageSize = 20 }, LanguageEnum.TraditionalChinese, 2);
                }
                else if (receiptCountry == 2)
                {
                    ViewBag.ChinaRefund = bll.GetRefundOrderList(queryInfo, new PageDTO() { PageIndex = page, PageSize = 20 }, LanguageEnum.TraditionalChinese, 1);

                    ViewBag.HKRefund = bll.GetRefundOrderList(queryInfo, new PageDTO() { PageIndex = PageNo, PageSize = 20 }, LanguageEnum.TraditionalChinese, 2);
                }
                else
                {
                    ViewBag.ChinaRefund = bll.GetRefundOrderList(queryInfo, new PageDTO() { PageIndex = 1, PageSize = 20 }, LanguageEnum.TraditionalChinese, 1);

                    ViewBag.HKRefund = bll.GetRefundOrderList(queryInfo, new PageDTO() { PageIndex = 1, PageSize = 20 }, LanguageEnum.TraditionalChinese, 2);
                }

                var total = bll.GetRefundTotal(queryInfo, LanguageEnum.TraditionalChinese);
                ViewBag.RefundTotal = total;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View();
        }


        public ActionResult RefundDetail(string refundCode)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(refundCode))
                {
                    IList<RefundDetailModel> list = new List<RefundDetailModel>();

                    list = bll.GetRefundInfos(refundCode, LanguageEnum.TraditionalChinese);

                    return View(list);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View();
        }


        public ActionResult ExportRefundList(string startTime, string endTime, string orderCode, string refundCode, int refundType, int refundStatus = -1)
        {
            RefundQueryInfo queryInfo = new RefundQueryInfo();

            if (string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
            {
                queryInfo.startTime = DateTime.Now.AddMonths(-3);
                queryInfo.endTime = DateTime.Now;
            }
            else
            {
                queryInfo.startTime = DateTime.Parse(startTime);
                queryInfo.endTime = DateTime.Parse(endTime);
            }
            queryInfo.orderCode = orderCode;
            queryInfo.refundStatus = refundStatus;
            queryInfo.refundCode = refundCode;
            queryInfo.SupplierId = CurrentUser.SupplierID;

            try
            {
                PageOf<RefundInfoModel> list = bll.GetRefundOrderList(queryInfo, new PageDTO() { PageIndex = PageNo, PageSize = int.MaxValue }, LanguageEnum.TraditionalChinese, 0);

                var dt = new DataTable();

                dt.Columns.Add("訂單號");
                dt.Columns.Add("買家");
                dt.Columns.Add("退款單號");
                dt.Columns.Add("生成時間");
                dt.Columns.Add("退款類型");
                dt.Columns.Add("SKU");
                dt.Columns.Add("商品名称");
                dt.Columns.Add("商品金額");
                dt.Columns.Add("關稅");
                dt.Columns.Add("退款金額");
                dt.Columns.Add("狀態");

                List<RefundInfoModel> result = list.Items.ToList<RefundInfoModel>();

                result.ForEach(l =>
                {
                    dt.Rows.Add(new object[]{
                    l.OrderCode,
                    l.UserId,
                    l.RefundCode,
                    l.CreateTime.ToString("yyyy-MM-dd"),
                    EnumHelper.GetDescription(l.RefundType , typeof(RefundType)),
                    l.Sku,
                    l.Name,
                    "￥"+l.UnitPrice*l.Quantity,
                    "",
                    "$"+l.TotalAmount,
                    EnumHelper.GetDescription(l.RefundStatus , typeof(RefundStatus))
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


        #endregion `
    }

}