using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SFO2O.M.Controllers
{
    public class HelpController : SFO2OBaseController
    {
        /// <summary>
        /// 常见问题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Question(int id = 1)
        {
            string viewName = "Question";
            viewName = viewName + id.ToString();

            return View(viewName);
        }
        /// <summary>
        /// 使用条款及细则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Terms(int id = 1)
        {
            string viewName = "Terms";
            viewName = viewName + id.ToString();

            return View(viewName);
        }
        /// <summary>
        /// 使用条款及细则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Service(int id = 1)
        {
            string viewName = "Service";
            viewName = viewName + id.ToString();

            return View(viewName);
        }
        /// <summary>
        /// 常见问题列表
        /// </summary>
        /// <returns></returns>
        public ActionResult TermsList()
        {
            return View();
        }
        /// <summary>
        /// 常见问题列表
        /// </summary>
        /// <returns></returns>
        public ActionResult QuestionList()
        {
            return View();
        }
        /// <summary>
        /// 关于我们 
        /// </summary>
        /// <returns></returns>
        public ActionResult AboutUs()
        {
            return View();
        }
        /// <summary>
        /// 配送及售后服务
        /// </summary>
        /// <returns></returns>
        public ActionResult ServiceList()
        {
            return View();
        }

        public ActionResult Agreement()
        {
            return View("");
        }

        public ActionResult Terms2()
        {
            return View();
        }
        public ActionResult Terms1()
        {
            return View();
        }

        public ActionResult UPloadIdentity()
        {
            return View();
        }
        public ActionResult LiquorCulture()
        {
            return View();
        }
        public ActionResult LiquorTasting()
        {
            ViewBag.ShowFooter = false;
            return View();
        }
    }
}
