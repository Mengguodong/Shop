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
    public class SettlementController : BaseController
    {
        private SettlementBll settlement = new SettlementBll();

        public ActionResult SettlementList(string startTime, string endTime, string orderCode,string settlementCode, int settlementStatus = 0)
        {
            try
            {
                SettlementQueryModel queryInfo = new SettlementQueryModel();

                if (string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
                {
                    queryInfo.StartTime = DateTime.Now.AddMonths(-3);
                    queryInfo.EndTime = DateTime.Now;
                }
                else
                {
                    queryInfo.StartTime = DateTime.Parse(startTime);
                    queryInfo.EndTime = DateTime.Parse(endTime);
                }
                queryInfo.OrderCode = orderCode;
                queryInfo.SettlementCode = settlementCode;
                queryInfo.SupplierId = CurrentUser.SupplierID;
                queryInfo.SettlementStatus = settlementStatus;

                ViewBag.QueryInfo = queryInfo;

                PageOf<SettlementOrderInfo> list = settlement.getSettlementList(queryInfo, new PageDTO() { PageIndex = PageNo, PageSize = 50 });

                 return View(list);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View();
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

        public ActionResult ExportSettlementList(string startTime, string endTime, string companyName, string settlementCode, string orderCode, int settlementStatus)
        {
            var dtNow = DateTime.Now;
            SettlementQueryModel queryInfo = new SettlementQueryModel();

            if (string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
            {
                queryInfo.StartTime = DateTime.Now.AddMonths(-3);
                queryInfo.EndTime = DateTime.Now;
            }
            else
            {
                queryInfo.StartTime = DateTime.Parse(startTime);
                queryInfo.EndTime = DateTime.Parse(endTime);
            }
            queryInfo.OrderCode = orderCode;
            queryInfo.SettlementCode = settlementCode;
            queryInfo.SupplierId = CurrentUser.SupplierID;
            queryInfo.SettlementStatus = settlementStatus;

            try
            {
                PageOf<SettlementOrderInfo> list = settlement.getSettlementList(queryInfo, new PageDTO() { PageIndex = PageNo, PageSize = int.MaxValue });
                var dt = new DataTable();
                dt.Columns.Add("結算單號");
                dt.Columns.Add("生成時間");
                dt.Columns.Add("訂單號/結算單號");
                dt.Columns.Add("結算方");
                dt.Columns.Add("商品總金額");
                dt.Columns.Add("商品退款金額");
                dt.Columns.Add("商家承擔關稅");
                dt.Columns.Add("平台承擔關稅");
                dt.Columns.Add("商家承担商品促销费用");
                dt.Columns.Add("其他費用");
                dt.Columns.Add("結算金額");
                dt.Columns.Add("狀態");

                List<SettlementOrderInfo> result = list.Items.ToList<SettlementOrderInfo>();

                result.ForEach(l =>
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
                    "$"+l.PromotionAmount.ToString("f2"),
                    "$"+l.OtherAmount,
                    "$"+(l.SettlementAmount - l.PromotionAmount).ToString("f2"),
                    EnumHelper.GetDescription(l.SettlementStatus , typeof(SettlementStatus))
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
       
    }
}