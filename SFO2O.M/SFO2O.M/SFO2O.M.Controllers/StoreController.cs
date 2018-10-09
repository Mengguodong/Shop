using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;

using SFO2O.BLL.Supplier;
using SFO2O.BLL.Product;
using SFO2O.M.ViewModel.Supplier;

namespace SFO2O.M.Controllers
{
    public class StoreController : SFO2OBaseController
    {
        private readonly SupplierBll supplierBll = new SupplierBll();
        private readonly ProductBll productBll = new ProductBll();
        private int PageSize = 10;

        /// <summary>
        /// 商品分类
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int id)
        {
            var model = supplierBll.GetStoreInfoById(id,this.language);
            return View(model);
        }

        public ActionResult Intro(int id)
        {
            var model = supplierBll.GetDefaultBySupplierId(id,this.language);
            return View(model);
        }

        public JsonResult ProductList(int sId, int page=1, int sort = 1)
        {
            int totalRecords = 0, pagecount = 0; 
            var model = productBll.GetSupplierProductListById(sId, sort, page, PageSize,base.language,base.DeliveryRegion,out totalRecords);
            if (model != null && model.Count > 0)
            {
                foreach (var p in model)
                {
                    p.MinPrice = Math.Round(p.MinPrice * ExchangeRate, 2,MidpointRounding.AwayFromZero);
                }
            }
            pagecount = totalRecords / PageSize;
            if (totalRecords % PageSize > 0)
            {
                pagecount += 1;
            }
            return Json(new { Type = 1, Data = new { PageIndex = page, PageSize = PageSize, TotalRecord = totalRecords, PageCount = pagecount, Products = model } }, JsonRequestBehavior.AllowGet);
        }
    }
}
