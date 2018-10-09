using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SFO2O.BLL.Product;
using SFO2O.Utility.Uitl;
using SFO2O.BLL.Supplier;
using SFO2O.Utility.Extensions;
using SFO2O.M.ViewModel.Brand;
using SFO2O.Model.Supplier;
using SFO2O.M.Controllers.Filters;
namespace SFO2O.M.Controllers
{
    /// <summary>
    /// 品牌信息
    /// </summary>
    public class BrandController : SFO2OBaseController
    {
        private int PageSize = 10;
        private ProductBll productBll = new ProductBll();
        private BrandBll brandBll = new BrandBll();

        /// <summary>
        /// 异步调用加载品牌下所有商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public JsonResult ProductList(int id, int page = 1, int sort = 1)
        {
            int totalRecords = 0, pagecount = 0;
            var model = productBll.GetBrandProductListFromCache(id, sort, page, PageSize, base.language, base.DeliveryRegion, out totalRecords);
            if (model != null && model.Count > 0)
            {
                foreach (var p in model)
                {
                    p.MinPrice = (p.MinPrice * ExchangeRate).ToNumberRound(2);
                    p.DiscountPrice = (p.DiscountPrice * ExchangeRate).ToNumberRound(2);
                    p.DiscountRate = Convert.ToDecimal(p.DiscountRate).ToNumberStringFloat();
                    p.Coupon = LoginUser == null ? 0 : LoginUser.SourceType;
                }
            }
            pagecount = totalRecords / PageSize;
            if (totalRecords % PageSize > 0)
            {
                pagecount += 1;
            }
            return Json(new { Type = 1, Data = new { PageIndex = page, PageSize = PageSize, TotalRecord = totalRecords, PageCount = pagecount, Products = model } }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 品牌页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(int id)
        {
            try
            {
                int userId = LoginUser == null ? 0 : LoginUser.UserID;
                int sourcetype = LoginUser == null ? 0 : LoginUser.SourceType;
                if (id <= 0)
                {
                    return Redirect("/Home/NotFound");
                }
                var brandInfo = brandBll.GetBrandInfoFromAutoCache(id, base.language, base.DeliveryRegion, base.ExchangeRate, userId, sourcetype);
                if (brandInfo == null)
                {
                    return Redirect("/Home/NotFound");
                }
                ViewBag.SpuCount = brandInfo.productInfoList.Count;
                return View(brandInfo);
            }

            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Redirect("/home/error");
            }
            //try
            //{
            //    if (id <= 0)
            //    {
            //        return Redirect("/Home/NotFound");
            //    }
            //    var brandInfo = brandBll.GetBrandInfo(id, base.language);
            //    int spuCount = brandBll.GetBrandSaleSpuCount(id, base.language, base.DeliveryRegion);
            //    if (base.LoginUser != null)
            //    {
            //        brandInfo.productInfoList = brandBll.getProductList(id, base.ExchangeRate, base.LoginUser.UserID);
            //    }
            //    else
            //    {
            //        brandInfo.productInfoList = brandBll.getProductList(id, base.ExchangeRate, 0);
            //    }


            //    ViewBag.SpuCount = spuCount;

            //    if (brandInfo == null)
            //    {
            //        return Redirect("/Home/NotFound");
            //    }
            //    return View(brandInfo);
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error(ex);
            //    return Redirect("/home/error");
            //}
        }
        public ActionResult Intro(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Redirect("/Home/NotFound");
                }
                var brandInfo = brandBll.GetBrandInfo(id, base.language);
                int spuCount = brandBll.GetBrandSaleSpuCount(id, base.language, base.DeliveryRegion);
                ViewBag.SpuCount = spuCount;
                ViewBag.StoreList = brandBll.GetStoreListByBrandId(id, base.language);
                if (brandInfo == null)
                {
                    return Redirect("/Home/NotFound");
                }
                return View(brandInfo);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Redirect("/home/error");
            }
        }
        /// <summary>
        /// 商品列表页筛选用到的品牌列表
        /// </summary>
        /// <param name="cid">一级分类id</param>
        /// <returns></returns>
        public JsonResult GetBrandList(string cid, int level)
        {
            try
            {
                cid = string.Join(",", cid.Split('|'));
                var list = brandBll.GetBrandListByCategoryId(cid, level, base.language, base._deliveryRegion);

                return Json(new { Type = 1, Data = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        //品牌街 页面

        public ActionResult BrandStreetIndex()
        {
            return View();
        }
        /// <summary>
        /// 品牌街列表
        /// </summary>
        public JsonResult GetBrandStreetList(int PageIndex = 1)
        {
            try
            {
                var list = BrandStreet(PageIndex);
                string ImageServer = System.Web.Configuration.WebConfigurationManager.AppSettings["ImageServer"].ToString();
                return Json(new { Type = 1, ImageServer = ImageServer, Data = new { PageIndex = list.PageIndex, PageSize = PageSize, PageCount = list.PageCount, TotalRecords = list.TotalRecord, Products = list.brandList } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 品牌街
        /// </summary>
        /// Brand Street
        public BrandViewModel BrandStreet(int PageIndex)
        {
            BrandViewModel viewmodel = new BrandViewModel();
            try
            {
                int totalRecords = 0, pagecount = 0;
                var products = brandBll.getBrandStreetList(PageIndex, PageSize);
                if (products != null && products.Count() > 0)
                {
                    totalRecords = products.FirstOrDefault().TotalRecord;
                }
                else
                {
                    totalRecords = 0;
                    products = new List<BrandEntity>();
                }
                viewmodel.brandList = products;
                viewmodel.PageSize = PageSize;
                viewmodel.TotalRecord = totalRecords;
                pagecount = totalRecords / PageSize;
                if (totalRecords % PageSize > 0)
                {
                    pagecount += 1;
                }
                viewmodel.PageCount = pagecount;
                viewmodel.PageIndex = PageIndex;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return viewmodel;

        }
    }
}
