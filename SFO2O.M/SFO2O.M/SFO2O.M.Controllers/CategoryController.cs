using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;

using SFO2O.BLL.Category;
using SFO2O.M.ViewModel.Category;
using SFO2O.Utility.Uitl;

namespace SFO2O.M.Controllers
{
    public class CategoryController : SFO2OBaseController
    {
        private readonly CategoryBll categoryBll = new CategoryBll();

        /// <summary>
        /// 商品分类
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var list = categoryBll.GetCategoryList(base.language);


            ViewBag.CategoryList =JsonConvert.SerializeObject( new { Type = "1", Data = list, Content = "", LinkUrl = "", Title =""});


            return View();
        }

        public JsonResult Category()
        {
            var jsonResult = new JsonResult();  
            var list = categoryBll.GetCategoryList(base.language);
            jsonResult.Data = new { Type = "1", Data = list, Content = "", LinkUrl = "", Title =""};
            return jsonResult;
        }
        /// <summary>
        /// 商品分类
        /// </summary>
        /// <returns></returns>
        public ActionResult Default()
        {
            string firstCategory = ConfigHelper.GetAppSetting<string>("FirstCategory");
            var list = categoryBll.GetCategoryList1and3(base.language,firstCategory);


            ViewBag.CategoryList = JsonConvert.SerializeObject(new { Type = "1", Data = list, Content = "", LinkUrl = "", Title = "" });


            return View();
        }

        public JsonResult CategoryList()
        {
            var jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            string firstCategory = ConfigHelper.GetAppSetting<string>("FirstCategory");
            var list = categoryBll.GetCategoryList1and3(base.language,firstCategory);
            jsonResult.Data = new { Type = "1", Data = list, Content = "", LinkUrl = "", Title = "" };
            return jsonResult;
        }
    }
}
