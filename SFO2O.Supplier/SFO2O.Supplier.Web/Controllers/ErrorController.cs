using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFO2O.Supplier.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NoPermission()
        {
            return View();
        }
        public ActionResult PageNotFound()
        {
            return View();
        }

        public ActionResult TokenExpired()
        {
            return View();
        }
    }
}