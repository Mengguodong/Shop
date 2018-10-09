using System;
using System.Collections.Generic;
using SFO2O.Utility.Uitl;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WxPayAPI;
using SFO2O.BLL.Order;
using SFO2O.Model.Order;
using LitJson;
namespace SFO2O.M.Controllers
{
    public class WxNotifyController : SFO2OBaseController
    {
        public void index()
        {
            ResultNotifyController result = new ResultNotifyController(HttpContext);
            result.ProcessNotify();
        }
    }
}