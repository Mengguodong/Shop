using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SFO2O.Admin.Businesses.Order;
using SFO2O.Admin.Businesses.Supplier;
using SFO2O.Admin.Common;
using SFO2O.Admin.Models.Enums;
using SFO2O.Admin.Models.Order;
using SFO2O.Admin.ViewModel.Order;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFO2O.Admin.Web.Controllers
{
    public class OrderController : BaseController
    {
        private OrderBll orderBll = new OrderBll();

        public ActionResult OrderList()
        {
            ViewBag.SallerNames = new SupplierBLL().GetSupplierNames();

            OrderListQueryModel query = new OrderListQueryModel();
            query.CreateTimeEnd = DateTime.Now;
            query.CreateTimeStart = query.CreateTimeEnd.AddMonths(-3);
            query.IsExcludeCloseOrder = 1;
            query.OrderStatus = -2;

            query.OrderCode = Request["OrderCode"] == null ? "" : Request["OrderCode"].ToString();

            return View(query);
        }

        public ActionResult GetOrderList(OrderListQueryModel query, int pageSize, int pageIndex, int isPaging)
        {
            OrderListAndCountModel result = new OrderListAndCountModel();

            try
            {
                result.Total = orderBll.GetOrderCount(query);
                
                result.OrderList = orderBll.GetOrderList(query, pageSize, pageIndex);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }
            return View(result);
        }

        public ActionResult GetOrderLogistics(string orderCode)
        {
            var result = new List<OrderLogisticsModel>();

            try
            {
                result = orderBll.GetOrderLogistics(orderCode);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(result);
        }

        public ActionResult ExportOrderList(DateTime startTime, DateTime endTime, string sku, int orderStatus, string buyerAccount, int supplierId, string orderCode, int isExcludeCloseOrder)
        {
            OrderListQueryModel query = new OrderListQueryModel()
            {
                BuyerAccount = buyerAccount,
                CountryCode = 0,
                CreateTimeEnd = endTime,
                CreateTimeStart = startTime,
                IsExcludeCloseOrder = isExcludeCloseOrder,
                OrderCode = orderCode,
                OrderStatus = orderStatus,
                SellerId = supplierId,
                SKU = sku
            };

            try
            {
                var list = orderBll.GetOrderList(query, Int32.MaxValue, 1);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                Response.AppendHeader("Content-Disposition", "attachment;filename=order_list" + System.DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                OrderListToExcel(list.Items);
                Response.End();
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View();
        }

        public ActionResult OrderDetail(string orderCode)
        {
            var result = new OrderModel()
                {
                    MainOrder = new OrderInfoModel(),
                    OrderLogistics = new List<OrderLogisticsModel>(),
                    OrderDetails = new List<OrderDetailModel>()
                };

            try
            {
                if (!String.IsNullOrWhiteSpace(orderCode))
                {
                    result = orderBll.GetOrderDetailInfo(orderCode);
                }

                if (null == result || result.MainOrder == null)
                {
                    result = new OrderModel()
                    {
                        MainOrder = new OrderInfoModel(),
                        OrderLogistics = new List<OrderLogisticsModel>(),
                        OrderDetails = new List<OrderDetailModel>()
                    };
                }

            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(result);
        }

        private void OrderListToExcel(IList<OrderListModel> list)
        {
            #region 导出Excel
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            var rowIndex = 3;
            ExcelHelper excelHelper = new ExcelHelper();
            //大标题
            excelHelper.SetBigTitle(sheet, workbook, "商品列表", 15);
            //子标题
            excelHelper.SetSubTitle(sheet, workbook, @"訂單號,生成時間,買家賬號,商品名稱,商品主屬性,商品主屬性值,商品子屬性,商品子屬性值,SKU,單價,數量,小計,賣家,商品總金額,實付金額,狀態", 2);
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
                cell = excelHelper.CreateCell(listRow, 0, CellType.String, string.Format("{0}", entity.OrderCode), itemStyle);
                cell = excelHelper.CreateCell(listRow, 1, CellType.String, string.Format("{0}", entity.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")), itemStyle);
                cell = excelHelper.CreateCell(listRow, 2, CellType.String, string.Format("{0}", entity.UserName), itemStyle);
                cell = excelHelper.CreateCell(listRow, 3, CellType.String, string.Format("{0}", entity.Name), itemStyle);
                cell = excelHelper.CreateCell(listRow, 4, CellType.String, string.Format("{0}", entity.MainDicValue), itemStyle);

                cell = excelHelper.CreateCell(listRow, 5, CellType.String, string.Format("{0}", entity.MainValue), itemStyle);
                cell = excelHelper.CreateCell(listRow, 6, CellType.String, string.Format("{0}", entity.SubDicValue), itemStyle);
                cell = excelHelper.CreateCell(listRow, 7, CellType.String, string.Format("{0}", entity.SubValue), itemStyle);
                cell = excelHelper.CreateCell(listRow, 8, CellType.String, string.Format("{0}", entity.Sku), itemStyle);
                cell = excelHelper.CreateCell(listRow, 9, CellType.String, string.Format("{0}", entity.PayUnitPrice), itemStyle);

                cell = excelHelper.CreateCell(listRow, 10, CellType.String, string.Format("{0}", entity.Quantity), itemStyle);
                cell = excelHelper.CreateCell(listRow, 11, CellType.String, string.Format("{0}", entity.PayUnitPrice * entity.Quantity), itemStyle);
                cell = excelHelper.CreateCell(listRow, 12, CellType.String, string.Format("{0}", entity.CompanyName), itemStyle);
                cell = excelHelper.CreateCell(listRow, 13, CellType.String, string.Format("{0}", entity.TotalAmount), itemStyle);
                var paidAmount = entity.PayUnitPrice * entity.Quantity;
                if (entity.CustomsDuties > 0 && entity.IsBearDuty == 0)
                {
                    paidAmount += entity.TaxAmount;
                }
                cell = excelHelper.CreateCell(listRow, 14, CellType.String, string.Format("{0}", paidAmount), itemStyle);

                cell = excelHelper.CreateCell(listRow, 15, CellType.String, string.Format("{0}", EnumUtils.GetEnumDescriptionByText(typeof(OrderStatus), entity.OrderStatus.ToString())), itemStyle);

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

        public ActionResult ExportOrderStockOutInfos(string startTime, string endTime)
        {
            DateTime sTime = new DateTime();
            DateTime eTime = new DateTime();

            if (String.IsNullOrWhiteSpace(startTime) || DateTime.TryParse(startTime, out sTime) == false)
            {
                sTime = DateTime.Now.AddHours(-1);
            }

            if (String.IsNullOrWhiteSpace(endTime) || DateTime.TryParse(endTime, out eTime) == false)
            {
                eTime = DateTime.Now.AddMinutes(-1);
            }
            try
            {
                var infos = orderBll.GetOrderStockOutInfos(sTime, eTime);
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                Response.AppendHeader("Content-Disposition", "attachment;filename=OrderList_" + sTime.ToString("yyyyMMddHHmmdd") + "_" + eTime.ToString("yyyyMMddHHmmdd") + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                OrderStockOutInfosToExcel(infos);
                Response.End();
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View();
        }

        private void OrderStockOutInfosToExcel(List<OrderStockOutModel> orderInfos)
        {
            #region 导出Excel
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            var rowIndex = 1;
            ExcelHelper excelHelper = new ExcelHelper();
            //子标题
            excelHelper.SetSubTitle(sheet, workbook, @"仓库ID,货主ID,月结账号, ,订单类型ID, ,客户支付SF运费方式ID,订单号码,承运商ID,承运商服务ID,收件公司,省,市,区/县,地址,是否货到付款,代收货款金额,是否保价,声明价值,商品编号,预留字段1/商品名称,商品出库数量,价格,订单总金额,收件人,固定电话,手机,订单备注", 0);
            #region 填充数据

            ICellStyle itemStyle = workbook.CreateCellStyle();
            itemStyle.BorderBottom = BorderStyle.Thin;
            itemStyle.BorderLeft = BorderStyle.Thin;
            itemStyle.BorderRight = BorderStyle.Thin;
            itemStyle.BorderTop = BorderStyle.Thin;

            foreach (var info in orderInfos)
            {
                #region
                var listRow = sheet.CreateRow(rowIndex++);
                ICell cell;
                cell = excelHelper.CreateCell(listRow, 0, CellType.String, string.Format("{0}", "852DCB"), itemStyle);
                cell = excelHelper.CreateCell(listRow, 1, CellType.String, string.Format("{0}", "8526691626"), itemStyle);
                cell = excelHelper.CreateCell(listRow, 2, CellType.String, string.Format("{0}", "8526691626"), itemStyle);
                cell = excelHelper.CreateCell(listRow, 3, CellType.String, string.Format("{0}", ""), itemStyle);
                cell = excelHelper.CreateCell(listRow, 4, CellType.String, string.Format("{0}", "销售订单"), itemStyle);

                cell = excelHelper.CreateCell(listRow, 5, CellType.String, string.Format("{0}", ""), itemStyle);
                cell = excelHelper.CreateCell(listRow, 6, CellType.String, string.Format("{0}", "寄付"), itemStyle);

                cell = excelHelper.CreateCell(listRow, 7, CellType.String, string.Format("{0}", info.OrderCode), itemStyle);
                cell = excelHelper.CreateCell(listRow, 8, CellType.String, string.Format("{0}", ""), itemStyle);
                cell = excelHelper.CreateCell(listRow, 9, CellType.String, string.Format("{0}", "标准快递"), itemStyle);

                cell = excelHelper.CreateCell(listRow, 10, CellType.String, string.Format("{0}", info.CompanyName), itemStyle);
                cell = excelHelper.CreateCell(listRow, 11, CellType.String, string.Format("{0}", info.ProvinceName), itemStyle);
                cell = excelHelper.CreateCell(listRow, 12, CellType.String, string.Format("{0}", info.CityName), itemStyle);
                cell = excelHelper.CreateCell(listRow, 13, CellType.String, string.Format("{0}", info.AreaName), itemStyle);
                cell = excelHelper.CreateCell(listRow, 14, CellType.String, string.Format("{0}", info.ReceiptAddress), itemStyle);

                cell = excelHelper.CreateCell(listRow, 15, CellType.String, string.Format("{0}", "否"), itemStyle);
                cell = excelHelper.CreateCell(listRow, 16, CellType.String, string.Format("{0}", ""), itemStyle);
                cell = excelHelper.CreateCell(listRow, 17, CellType.String, string.Format("{0}", "否"), itemStyle);
                cell = excelHelper.CreateCell(listRow, 18, CellType.String, string.Format("{0}", ""), itemStyle);

                cell = excelHelper.CreateCell(listRow, 19, CellType.String, string.Format("{0}", info.BarCode), itemStyle);
                cell = excelHelper.CreateCell(listRow, 20, CellType.String, string.Format("{0}", info.Name + " " + info.MainValue + " " + info.SubValue), itemStyle);
                cell = excelHelper.CreateCell(listRow, 21, CellType.String, string.Format("{0}", info.Quantity), itemStyle);

                cell = excelHelper.CreateCell(listRow, 22, CellType.String, string.Format("{0}", info.UnitPrice), itemStyle);
                cell = excelHelper.CreateCell(listRow, 23, CellType.String, string.Format("{0}", info.TotalAmountHK.ToString("f2")), itemStyle);

                cell = excelHelper.CreateCell(listRow, 24, CellType.String, string.Format("{0}", info.Receiver), itemStyle);
                cell = excelHelper.CreateCell(listRow, 25, CellType.String, string.Format("{0}", info.Phone), itemStyle);
                cell = excelHelper.CreateCell(listRow, 26, CellType.String, string.Format("{0}", info.Mobile), itemStyle);
                cell = excelHelper.CreateCell(listRow, 27, CellType.String, string.Format("{0}", ""), itemStyle);

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
    }
}