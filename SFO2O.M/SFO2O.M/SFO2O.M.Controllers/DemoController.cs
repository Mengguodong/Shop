using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using SFO2O.BLL.Exceptions;
using SFO2O.Model;
using SFO2O.Utility.Uitl;

namespace SFO2O.M.Controllers
{
    public class DemoController : SFO2OBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            List<SupplierEntity> list = new List<SupplierEntity>();


            for (int i = 1; i < 8; i++)
            {
                list.Add(new SupplierEntity { AgentId = 1, AgentType = 2, ApplyAuthentication = 3, BaiduID = 4 });
            }
            //return base.HandleError("XXX错误");
            return View(list);
        }

        public ActionResult Partial1(List<SFO2O.Model.SupplierEntity> m)
        {
            return PartialView(m);
        }
        public ActionResult Partial2(List<SFO2O.Model.SupplierEntity> m)     
        {
            return PartialView(m);
        }
        public ActionResult Get()
        {

            List<SupplierEntity> list = new List<SupplierEntity>();
            for (int i = 1; i < 8; i++)
            {
                list.Add(new SupplierEntity { AgentId = 1, AgentType = 2, ApplyAuthentication = 3, BaiduID = 4 });
            }
            //return this.HandleSuccess("OK", list);
            try
            {
                throw new SFO2OException("错误测试");
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }


          return  PerformanceTracer.Invoke(() =>
            {
                //TODO：调用并且跟踪指定代码块的执行时间
                return this.HandleError("错误测试");
            }, "跟踪模块名称");

            PerformanceTracer.Invoke(() =>
            {
                //TODO：调用并且跟踪指定代码块的执行时间

            }, "跟踪模块名称");

            PerformanceTracer.InvokeAsync(() =>
            {
                //TODO:通过异步方式调用的方式处理一些不太重要的工作
            }, "跟踪模块名称");


            //return this.HandleError("参数错误！");
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your app description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
        public ActionResult Error()
        {
            ViewBag.Message = "Your contact page.";


            // return this.HandleSuccess("操作成功");
            return View();
        }
    }
}
