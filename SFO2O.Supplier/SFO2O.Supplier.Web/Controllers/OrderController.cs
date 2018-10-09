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
    public class OrderController : BaseController
    {
        OrderInfoBLL bll = new OrderInfoBLL();

        #region 订单管理

        /// <summary>
        /// 确认发货/修改订单状态
        /// </summary>
        /// <param name="expressCompany">物流公司</param>
        /// <param name="expressCode">物流单号</param>
        /// <param name="freight">运费</param>
        /// <returns></returns>
        public JsonResult ComfirmSendGoods(string orderId, string expressCompany, string expressCode, string freight)
        {
            try
            {
                decimal _freight = decimal.Parse(freight);
                bool result = bll.ComfirmSendGoods(orderId, expressCompany, expressCode, freight);
                if (result)
                {
                    return Json(new { result = true, msg = "确认发货成功!" });
                }
                else
                {
                    return Json(new { result = false, msg = "确认发货失败!" });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { result = false, msg = "确认发货失败，请重试或联系管理员。" });
            }
            //return Json(new { result = true, msg = "确认发货失败，请重试或联系管理员。" });
        }

        /// <summary>
        /// 商家更新订单信息（运费、运单号、物流公司）
        /// </summary>
        /// <param name="expressCompany">物流公司</param>
        /// <param name="expressCode">物流单号</param>
        /// <param name="freight">运费</param>
        /// <returns></returns>
        public JsonResult UpdateOrderInfo(string orderId, string expressCompany, string expressCode)
        {
            try
            {
                bool result = bll.UpdateOrderInfo(orderId, expressCompany, expressCode);
                if (result)
                {
                    return Json(new { result = true, msg = "更新成功!" });
                }
                else
                {
                    return Json(new { result = true, msg = "更新失败，请重试或联系管理员。" });
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { result = false, msg = "更新失败，请重试或联系管理员。" });
            }
        }
        /// <summary>
        /// 获取物流单号
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="expressCompany"></param>
        /// <param name="expressCode"></param>
        /// <param name="freight"></param>
        /// <returns></returns>
        public JsonResult GetOrderExpress(string orderId)
        {
            try
            {
                OrderExpressEntity result = bll.GetOrderExpress(orderId);
                if (result != null)
                {
                    return Json(result);
                }
                else
                {
                    return Json(new { result="0"});
                
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { result = false, msg = "查询运单号失败！信息：" + ex.Message });
            }
        }
        public ActionResult OrderList(string startTime, string endTime, string orderCode, int receiptCountry = 0, int orderStatus = 1, int page = 1)
        {

            OrderQueryInfo queryInfo = new OrderQueryInfo();

            if (string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
            {
                queryInfo.startTime = DateTime.Parse("2017-07-16");
                queryInfo.endTime = DateTime.Now;
            }
            else
            {
                queryInfo.startTime = DateTime.Parse(startTime);
                queryInfo.endTime = DateTime.Parse(endTime);
            }
            queryInfo.orderCode = orderCode;
            queryInfo.orderSatus = orderStatus;
            queryInfo.SupplierId = CurrentUser.SupplierID;

            ViewBag.QueryInfo = queryInfo;
            ViewBag.ReceiptCountry = receiptCountry;
            ViewBag.Page = page;

            try
            {
                if (receiptCountry == 1)
                {
                    ViewBag.ChinaOrder = bll.GetOrderList(queryInfo, new PageDTO() { PageIndex = PageNo, PageSize = 20 }, LanguageEnum.SimplifiedChinese, 1);

                   // ViewBag.HKOrder = bll.GetOrderList(queryInfo, new PageDTO() { PageIndex = page, PageSize = 20 }, LanguageEnum.TraditionalChinese, 2);
                }
                else if (receiptCountry == 2)
                {
                    ViewBag.ChinaOrder = bll.GetOrderList(queryInfo, new PageDTO() { PageIndex = page, PageSize = 20 }, LanguageEnum.SimplifiedChinese, 1);

                    //ViewBag.HKOrder = bll.GetOrderList(queryInfo, new PageDTO() { PageIndex = PageNo, PageSize = 20 }, LanguageEnum.TraditionalChinese, 2);
                }
                else
                {
                    ViewBag.ChinaOrder = bll.GetOrderList(queryInfo, new PageDTO() { PageIndex = 1, PageSize = 20 }, LanguageEnum.SimplifiedChinese, 1);

                    //ViewBag.HKOrder = bll.GetOrderList(queryInfo, new PageDTO() { PageIndex = 1, PageSize = 20 }, LanguageEnum.TraditionalChinese, 2);
                }

                var totalCN = bll.GetOrderTotal(queryInfo, LanguageEnum.SimplifiedChinese, 1);
                //var totalHK = bll.GetOrderTotal(queryInfo, LanguageEnum.TraditionalChinese, 2);
                ViewBag.OrderTotalCN = totalCN;
                //ViewBag.OrderTotalHK = totalHK;

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);

            }
            return View();
        }


        public ActionResult OrderDetail(string orderCode)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(orderCode))
                {
                    var model = new OrderViewModel();

                    model = bll.GetOrderDetailByOrderCode(orderCode, CurrentUser.SupplierID, LanguageEnum.SimplifiedChinese);

                    ViewBag.TotalTaxAmount = bll.GetOrderTotalTaxRate(orderCode, CurrentUser.SupplierID);

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View();
        }


        public ActionResult ExportOrderList(string startTime, string endTime, string orderCode, int orderStatus = -1)
        {
            OrderQueryInfo queryInfo = new OrderQueryInfo();

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
            queryInfo.orderSatus = orderStatus;
            queryInfo.SupplierId = CurrentUser.SupplierID;
            try
            {
                PageOf<OrderListInfoModel> list = bll.GetOrderList(queryInfo, new PageDTO() { PageIndex = PageNo, PageSize = int.MaxValue }, LanguageEnum.SimplifiedChinese, 0);

                var dt = new DataTable();

                dt.Columns.Add("订单号");
                dt.Columns.Add("生成时间");
                dt.Columns.Add("商品名称");
                dt.Columns.Add("单价");
                dt.Columns.Add("数量");
                dt.Columns.Add("小计");
                dt.Columns.Add("状态");
                dt.Columns.Add("收货地址");
                dt.Columns.Add("收货人");
                dt.Columns.Add("联系电话");

                List<OrderListInfoModel> result = list.Items.OrderBy(o=>o.Phone).ToList<OrderListInfoModel>();

                result.ForEach(l =>
                {
                    dt.Rows.Add(new object[]{
                    l.OrderCode,
                    l.CreateTime.ToString("yyyy-MM-dd hh:mm:ss"),
                    l.Name,
                    "￥"+l.UnitPrice,
                    l.Quantity,
                    "￥" +l.UnitPrice * l.Quantity,
                    EnumHelper.GetDescription(l.OrderStatus , typeof(OrderStatus)),
                    l.ReceiptAddress,
                    l.Receiver,
                    l.Phone,
                  
                    });
                });

                ExcelHelper.ExportByWeb(dt, "订单列表", "orderlist" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View();
        }

        public ActionResult ShowLogistics(string expressCode)
        {
            List<GetSFData.ExpressInfoEntity> orderLogistics = new List<GetSFData.ExpressInfoEntity>();

            try
            {
                using (GetSFData.GetSFDataClient sf = new GetSFData.GetSFDataClient())
                {
                    if (!string.IsNullOrEmpty(expressCode))
                    {
                        orderLogistics = sf.GetExpressInfo(expressCode);
                    }
                }

                return PartialView(orderLogistics);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);

            }
            return PartialView(orderLogistics);
        }

        #endregion



    }

}