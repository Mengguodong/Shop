using SFO2O.Model.Enum;
using SFO2O.Model.Index;
using SFO2O.Model.Product;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SFO2O.Utility.Extensions;
using SFO2O.BLL.Supermarket;
using SFO2O.M.ViewModel.Product;

namespace SFO2O.M.Controllers
{
    public class SuperMarketController : SFO2OBaseController
    {
        private readonly SupermarketBll supermarketBll = new SupermarketBll();
        private int PageSize = 10;

        /// <summary>
        /// 超市页
        /// </summary>
        /// <returns></returns>
        public ActionResult Default()
        {
            return View();
        }

        /// <summary>
        /// 焦点轮播图
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public ActionResult ScrollImage(int channelId=2)
        {
            List<BannerImagesEntity> listTopModule = supermarketBll.GetIndexBannerImagesFromCache(channelId).ToList();
            return PartialView("_ScrollImage", listTopModule);
        }

        public JsonResult MarketProductList(string c, int level, int page, int sort = 1, string attrData = "")
        {
            try
            {
                // 分类id为空直接返回失败的json串
                if (string.IsNullOrEmpty(c))
                {
                    return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
                }

                // 解析分类id
                string[] NewCategoryArray = c.Split('|');
                if (NewCategoryArray == null || NewCategoryArray.Length == 0)
                {
                    return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
                }

                //var model = GetProductList(c, level, page, sort, attrData);
                var model = GetMarketProductListNew(NewCategoryArray, level, page, sort, attrData);
                return Json(new { Type = 1, Data = new { PageIndex = model.PageIndex, PageSize = PageSize, TotalRecord = model.TotalRecord, PageCount = model.PageCount, Products = model.Products } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
        }

        private ProductListModel GetMarketProductListNew(string[] c, int level, int page, int sort = 1, string brandIds = "")
        {
            ProductListModel viewmodel = new ProductListModel();
            try
            {
                var fileterModel = new List<ProductFilterAttrubile>();
                //if (!string.IsNullOrEmpty(attrids))
                //{
                //    fileterModel = JsonHelper.ToObject<List<ProductFilterAttrubile>>(attrids);
                //}
                List<int> ids = new List<int>();
                if (!string.IsNullOrEmpty(brandIds))
                {
                    ids = JsonHelper.ToObject<List<int>>(brandIds);
                }
                int totalRecords = 0, pagecount = 0;
                //var attributeList = categoryBll.GetCategoryAttribute(c, base.language);
                List<ProductInfoModel> MarketProductList = supermarketBll.GetMarketProductListNew(c, level, ids, sort, page, PageSize, base.language, base.DeliveryRegion, base.ExchangeRate, out totalRecords);
                viewmodel.Products = MarketProductList.Where(item => item.Qty > 0).ToList();

                if (viewmodel.Products != null && viewmodel.Products.Count > 0)
                {
                    foreach (var p in viewmodel.Products)
                    {
                        p.MinPrice = (p.MinPrice * ExchangeRate).ToNumberRound(2);
                        p.DiscountPrice = (p.DiscountPrice * ExchangeRate).ToNumberRound(2);
                        p.DiscountRate = Convert.ToDecimal(p.DiscountRate).ToNumberStringFloat();
                    }
                }
                viewmodel.PageSize = PageSize;
                viewmodel.TotalRecord = totalRecords;
                pagecount = totalRecords / PageSize;
                if (totalRecords % PageSize > 0)
                {
                    pagecount += 1;
                }
                viewmodel.PageCount = pagecount;
                viewmodel.PageIndex = page;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return viewmodel;
        }

        /// <summary>
        /// 今日疯抢
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public ActionResult BerserkDay(int top=10)
        {
            try
            {
                ViewBag.ExchangeRate = base.ExchangeRate;
                var list = supermarketBll.GetBerserkDay(top, base.language).ToList();
                return PartialView("BerserkDay", list);
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }
        }
        /// <summary>
        /// 爱玖网超市页面单banner图
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public ActionResult SingleBanner(int channelId=2)
        {
            SingleBannerImagesEntity SingleBannerModel = supermarketBll.GetIndexSingleBannerImagesFromCache(channelId);
            return PartialView("_SingleBannerImage", SingleBannerModel);
        }
    }
}
