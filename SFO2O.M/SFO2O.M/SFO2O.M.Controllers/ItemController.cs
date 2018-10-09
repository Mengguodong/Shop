using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SFO2O.BLL.Item;
using SFO2O.BLL.Product;
using SFO2O.M.Controllers.Extensions;
using SFO2O.Model.Extensions;
using SFO2O.Model.Product;
using SFO2O.Utility.Uitl;

namespace SFO2O.M.Controllers
{
    public class ItemController : SFO2OBaseController
    {
        ProductBll bll = new ProductBll();
        ItemBll itemBll = new ItemBll();
        /// <summary>
        /// PageLoad
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="selectedSku"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult Index(string productCode, string selectedSku = "")
        {
            if (string.IsNullOrEmpty(productCode))
            {
                return HandleError("无法找到该商品");
            }
            //判断是否是节日食品
            bool istrue = bll.isHolidayGoods(productCode);
            ViewBag.Title = bll.GetProductNameBySpu(productCode);
            ViewBag.isHolidayGoods = istrue;
            ViewBag.productCode = productCode;
            ViewBag.selectedSku = selectedSku;
            return View();
        }

        #region 单品页-局部视图
        public ActionResult Item(string productCode, string selectedSku = "")
        {
            try
            {
                int userid = LoginUser == null ? 0 : LoginUser.UserID;
                var item = bll.ItemViewSupporterFromAutoCache(productCode, userid, base.language, selectedSku, base.ExchangeRate);
                if (item != null && item.ProductSkuEntity != null && item.ProductSkuEntity.Count() > 0)
                {
                    bool isflag = false;
                    if (base.LoginUser != null)
                    {
                        isflag = bll.getFivoriteDetail(productCode, base.LoginUser.UserID);
                    }

                    var promotions = itemBll.GetPromotionEntities(item.ProductSkuEntity.Select(n => n.Sku).ToArray());
                    var flag = false;
                    if (promotions.Where(d => d.PromotionType == 2).Count() > 0)
                    {
                        flag = true;
                    }
                    //促销商品，过滤掉拼团(PromotionType=2)商品，留下打折商品PromotionType=1
                    promotions = promotions.Where(d => d.PromotionType != 2).ToList();
                    //组装DTO--pageload
                    var product = item.ProductSkuEntity.ToArray().AsDto(base.ExchangeRate, promotions, isflag);
                    if (flag)
                    {
                        product.isTrue = 1;
                    }

                    var skuAttributeViewModel = item.ProductSkuEntity.AsSkuAttributeViewModelD(base.ExchangeRate, promotions, selectedSku, dto: item);

                    //组装skuAttributeViewModel-- for js skuSelected
                    ViewBag.SkuMetaViewModel = skuAttributeViewModel;
                    bool skuFlag = item.ProductSkuEntity.GroupBy(d => d.Sku).Count() > 1;
                    //当SKU只有1条时候，默认要被选中，要把所有的FLAG重置1
                    if (!skuFlag)
                    {
                        foreach (var skuAttribute in skuAttributeViewModel.MainAttributes)
                        {
                            skuAttribute.Flag = 1;
                            foreach (var subSkuAttribute in skuAttribute.SubAttributes)
                            {
                                subSkuAttribute.Flag = 1;
                            }
                        }
                        foreach (var subSkuAttribute in skuAttributeViewModel.SubAttributes)
                        {
                            subSkuAttribute.Flag = 1;
                        }
                        ViewBag.SkuMetaViewModel = skuAttributeViewModel;
                    }
                    return PartialView("_Item", item);
                }

                else
                {
                    return PartialView("_Item", new ItemViewSupporter());
                }
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        #endregion

        /// <summary>
        /// 图片大图页
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult ProductPicture(string productCode)
        {
            try
            {

                if (string.IsNullOrEmpty(productCode))
                {

                    return HandleError("无法找到该商品");
                }
                //获取单品页商品
                var images = bll.GetProductImages(productCode);





                return View(images);

            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }

        }


        /// <summary>
        /// 商品参数
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public ActionResult Param(string productCode)
        {
            try
            {

                var list = bll.GetSpuAttributes(productCode, language);

                foreach (var item in list.Where(s => s.AttributeKey == "SupportedLanguage" || s.AttributeName == "标签"))
                {
                    item.AttributeValue = item.AttributeValue.Replace("$", "，");
                }
                foreach (var item in list.Where(s => s.AttributeValue != null && s.AttributeValue.IndexOf("0.00") == 0))
                {
                    item.AttributeValue = "";
                }

                return View(list);
            }
            catch (Exception ex)
            {

                return HandleError(ex);
            }
        }

        /// <summary>
        /// 商品详情[富文本内容]
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public ActionResult Detail(string productCode)
        {
            try
            {
                object description = bll.GetDescription(productCode, language);
                return View(description);
            }
            catch (Exception ex)
            {

                return HandleError(ex);
            }
        }

        /// <summary>
        /// 评论--逻辑有问题，待完善
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Comment(string productCode, int pageIndex = 1, int pageSize = 20)
        {
            try
            {
                int recordCount = 0;
                object description = bll.GetCommentEntities(productCode, language, out recordCount, pageIndex, pageSize);
                return View(description);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        /// <summary>
        /// 首页评论数
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult CommentCount(string productCode, int pageIndex = 1, int pageSize = 20)
        {
            try
            {
                int recordCount = 0;
                object description = bll.GetCommentEntities(productCode, language, out recordCount, pageIndex, pageSize);
                return Content(recordCount.ToString());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        /// <summary>
        /// 同店推荐
        /// </summary> 
        /// <param name="brandId"></param>
        /// <param name="spu"></param>
        /// <returns></returns>
        public ActionResult Recommended(int brandId, string spu)
        {
            try
            {
                ViewBag.ExchangeRate = ExchangeRate;
                var productList = bll.GetRecommendedProductList(brandId, 11, language, base.DeliveryRegion, spu);

                return View(productList.Where(s => s.SPU != spu).Take(10));
            }
            catch (Exception ex)
            {

                return HandleError(ex);
            }
        }

        /// <summary>
        /// 相似品牌 -部分视图
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="brandId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public ActionResult SimilarBrand(int supplierId, int brandId, int categoryId)
        {
            try
            {
                var list = itemBll.GetSupplierBrandEntities(5, brandId, supplierId.ToString(), categoryId);
                return PartialView(list);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }




        /// <summary>
        /// 商品税目录下载页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Tariff()
        {
            return View();
        }


        /// <summary>
        /// 下载
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadTariff()
        {
            var ext = "application/pdf";
            var filePath = Server.MapPath("~/File/Tariff.pdf");

            var fileName = "Tariff.pdf";

            return File(filePath, ext, fileName);
        }

        /// <summary>
        /// 根据SPU获取当前所有SKU库存
        /// </summary>
        /// <param name="spu">SPU</param>
        /// <returns>返回是否已售罄，以及当前SPU下所有在售的SKU</returns>
        public JsonResult GetStockInfoBySpu(string spu)
        {
            try
            {

                return Json(new { Type = 1, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0, Content = "获取数据失败！" }, JsonRequestBehavior.AllowGet);
        }
    }
}
