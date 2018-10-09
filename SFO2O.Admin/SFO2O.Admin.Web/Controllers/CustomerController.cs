using SFO2O.Admin.Businesses.Customer;
using SFO2O.Admin.Common;
using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Customer;
using SFO2O.Admin.ViewModel;
using SFO2O.Admin.ViewModel.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFO2O.Admin.Web.Controllers
{
    public class CustomerController : BaseController
    {
        private CustomerBll customer = new CustomerBll();
        // GET: Customer
        public ActionResult Index()
        {
            var dtNow = DateTime.Now;

            var qm = new CustomerQueryModel()
            {
                Mobile = Request["Mobile"] == null ? "" : Request["Mobile"].ToString(),
                Email = Request["Email"] == null ? "" : Request["Email"].ToString(),
                CountryArea = Request["Email"] == null ? -1 : Convert.ToInt32(Request["Email"].ToString()),
                EndTime = Request["EndTime"] == null ? dtNow : Convert.ToDateTime(Request["EndTime"].ToString()),
                StartTime = Request["StartTime"] == null ? dtNow.AddMonths(-6) : Convert.ToDateTime(Request["StartTime"].ToString()),
                
            };

            ViewBag.PageNo = Request["PageNo"] == null ? 1 : Convert.ToInt32(Request["PageNo"].ToString());
            ViewBag.PageSize = Request["PageSize"] == null ? 50 : Convert.ToInt32(Request["PageSize"].ToString());

            return View(qm);
        }
        public ActionResult CustomerList(CustomerQueryModel queryModel)
        {
            PagedList<CustomerInfo> list = null;

            try
            {
                list = customer.getOrderList(queryModel.StartTime, queryModel.EndTime, queryModel.Email, queryModel.Mobile, queryModel.CountryArea, new Models.PagingModel { IsPaging = true, PageIndex = this.PageNo, PageSize = this.PageSize });
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(list);
        }
        public ActionResult ExportCustomerList(DateTime startTime, DateTime endTime, string email, string mobile, int countryArea)
        {
            var dtNow = DateTime.Now;
            PagedList<CustomerInfo> list = null;
            try
            {
                list = customer.getOrderList(startTime, endTime, email, mobile, countryArea, new Models.PagingModel { IsPaging = false, PageIndex = this.PageNo, PageSize = this.PageSize });
                var dt = new DataTable();
                dt.Columns.Add("手機號碼");
                dt.Columns.Add("國家地區");
                dt.Columns.Add("註冊時間");
                dt.Columns.Add("電子郵箱");
                dt.Columns.Add("會員積分");
                dt.Columns.Add("消費總金額");
                dt.Columns.Add("消費次數");
                list.ContentList.ForEach(l => {
                    dt.Rows.Add(new object[]{
                    l.Mobile,
                    l.RegionCode == "86" ? "中國大陸" : "中國香港",
                    l.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    l.Email,
                    l.MemberPoints,
                    "$"+l.ConsumptionAmount.ToString("f2"),
                    l.ConsumptionTimes
                    });
                });
                ExcelHelper.ExportByWeb(dt, "會員列表", "memberlist" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");                    
                  
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View();
        }
    }
}