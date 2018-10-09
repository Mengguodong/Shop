using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SFO2O.M.Controllers.Filters;
using SFO2O.BLL.Shopping;
using SFO2O.BLL.Exceptions;
using SFO2O.Utility.Uitl;
using SFO2O.M.Controllers.Extensions;
using SFO2O.Utility.Extensions;
using SFO2O.Payment.BLL;
using SFO2O.Utility;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Net;
using SFO2O.BLL.My;
using SFO2O.Model.Product;
using SFO2O.M.ViewModel.Favorite;
using SFO2O.BLL.Product;
using SFO2O.Model.Extensions;
using SFO2O.BLL.Item;
namespace SFO2O.M.Controllers
{
    public class FavoriteController : SFO2OBaseController
    {
        private readonly ProductBll productBll = new ProductBll();
        private int PageSize = 10;
        ProductBll bll = new ProductBll();
        ItemBll itemBll = new ItemBll();

        public ActionResult Index()
        {
            return View();
        }
        [Login]
        public JsonResult FavoriteListIndex(int PageIndex = 1)
        {
            try
            {
                var model = GetFavoriteList(PageIndex, base.ExchangeRate);
                string ImageServer = System.Web.Configuration.WebConfigurationManager.AppSettings["ImageServer"].ToString();
                return Json(new { Type = 1, ImageServer = ImageServer, Data = new { PageIndex = model.PageIndex, PageSize = PageSize, PageCount = model.PageCount, TotalRecords = model.TotalRecord, Products = model.Products } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 收藏列表页
        /// </summary>
        /// <param name="c"></param>
        /// <param name="level"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="attrids"></param>
        /// <returns></returns>
        private FavoriteViewModel GetFavoriteList(int PageIndex, decimal ExchangeRate)
        {
            FavoriteViewModel viewmodel = new FavoriteViewModel();
            try
            {

                int totalRecords = 0, pagecount = 0;
                //得到收藏的列表
                var products = productBll.GetFavoriteList(PageIndex, PageSize, base.language, base.ExchangeRate, base.LoginUser.UserID);

                foreach(var spuList in products)
                {
                   IList<ProductImage> images = null;
                  // 获取单品页商品
                   var itemskus = bll.GetItemByProductCodeAndStatus(spuList.spu, base.language, ref images);
                   string spu=itemskus.FirstOrDefault().Spu;
                    
                 //二期：获取促销信息
                 var promotions = itemBll.GetPromotionEntities(itemskus.Select(n => n.Sku).ToArray());
                 promotions = promotions.Where(d => d.PromotionType != 2).ToList();
                 //组装DTO--pageload
                 var product = itemskus.ToArray().AsDto(base.ExchangeRate, promotions,false);
                 if (product.Promotion != null)
                 {

                     if (spuList.originalPrice > Convert.ToDecimal(product.PromotionDiscountPrice.ToNumberRoundStringWithPoint(2)))
                     {
                         spuList.isDiscount = 1;
                         spuList.DiscountPrice = spuList.originalPrice - product.PromotionDiscountPrice;
                         spuList.originalPrice = product.PromotionDiscountPrice;
                     }

                 }
                 else if (spuList.originalPrice > Convert.ToDecimal(product.MinPrice.ToNumberRoundStringWithPoint(2)))
                 {
                     spuList.isDiscount = 1;
                     spuList.DiscountPrice = spuList.originalPrice - product.MinPrice;
                     spuList.originalPrice = product.MinPrice;
                 }
                 else if (spuList.originalPrice < Convert.ToDecimal(product.MinPrice.ToNumberRoundStringWithPoint(2)))
                 {
                     spuList.originalPrice = product.MinPrice;
                 }
                }

                if (products != null)
                {
                    totalRecords = products.FirstOrDefault().TotalRecord;
                }
                else
                {
                    totalRecords = 0;
                    products = new List<Favorite>();
                }

                viewmodel.Products = products;
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
        /// <summary>
        /// 收藏功能
        /// </summary>
        [Login]
        public JsonResult isFavorite(string productCode, string collectionStatus)
        {
            try
            {
                IList<ProductImage> images = null;
                //获取单品页商品
                var itemskus = bll.GetItemByProductCodeAndStatus(productCode, base.language, ref images);
                string spu = itemskus.FirstOrDefault().Spu;


                //二期：获取促销信息
                var promotions = itemBll.GetPromotionEntities(itemskus.Select(n => n.Sku).ToArray());
                //二期：获取品牌信息（获取3种语言版本）
                promotions = promotions.Where(d => d.PromotionType != 2).ToList();
                //组装DTO--pageload
                var product = itemskus.ToArray().AsDto(base.ExchangeRate, promotions,false);

                var model = bll.isFavorite(productCode, collectionStatus, base.LoginUser.UserID,product);
                return Json(new { Type = 1,Status=1}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0}, JsonRequestBehavior.AllowGet); 
        }
    }
}
