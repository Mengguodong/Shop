using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Threading.Tasks;
using SFO2O.BLL.Category;
using SFO2O.BLL.Product;
using SFO2O.M.ViewModel.Product;
using SFO2O.Utility.Uitl;
using SFO2O.M.Controllers.Extensions;
using SFO2O.Model.Product;
using SFO2O.Utility.Extensions;
using SFO2O.Utility;
using SFO2O.BLL.Common;
using SFO2O.Model.Category;
using System.Data;

namespace SFO2O.M.Controllers
{
    public class ProductController : SFO2OBaseController
    {
        private readonly CategoryBll categoryBll = new CategoryBll();
        private readonly ProductBll productBll = new ProductBll();
        private readonly CommonBll commonBll = new CommonBll();
        private int PageSize = 50;
        #region 三级商品列表
        ///// <summary>
        ///// 三级商品列表
        ///// </summary>
        ///// <param name="c">分类categoryid</param>
        ///// <param name="level">分类等级categoryLevel</param>
        ///// <param name="attrids">筛选属性ids</param>
        ///// <param name="sort">排序：0上新排序，1升序，2降序</param>
        ///// <returns></returns>
        //public ActionResult List(int c,int level,int sort=1,string attrids="")
        //{
        //    ProductListModel viewmodel = new ProductListModel();
        //    try
        //    {
        //        ViewBag.LoginUser = LoginUser;
        //        var cateModel = categoryBll.GetCategoryEntity(c, level,base.language);
        //        if (cateModel != null)
        //        {
        //            viewmodel.CategoryName = cateModel.CategoryName;
        //        }
        //        #region 分类及分类属性

        //        viewmodel.CategoryId = c;
        //        viewmodel.CategoryLevel = level;
        //        if (level == 0)
        //        {
        //            var list = categoryBll.GetCategorys(c, level,base.language);//取二级
        //            List<CategoryModels> listCategory = new List<CategoryModels>();
        //            if (list != null)
        //            {
        //                foreach (var cc in list)
        //                {
        //                    listCategory.Add(new CategoryModels()
        //                    {

        //                        CategoryId = cc.CategoryId,
        //                        CategoryName = cc.CategoryName,
        //                        CategoryLevel = cc.CategoryLevel,
        //                        ChildCategorys = categoryBll.GetCategorys(cc.CategoryId, 3,base.language)//三级
        //                    });
        //                }
        //            }
        //            viewmodel.CategorysList = listCategory;
        //        }
        //        else if (level == 1)
        //        {
        //            var list = categoryBll.GetCategorys(c, level,base.language);//取三级
        //            List<CategoryModels> listCategory = new List<CategoryModels>();
        //            if (list != null)
        //            {
        //                foreach (var cc in list)
        //                {
        //                    listCategory.Add(new CategoryModels()
        //                    {
        //                        CategoryId = cc.CategoryId,
        //                        CategoryName = cc.CategoryName,
        //                        CategoryLevel = cc.CategoryLevel
        //                    });
        //                }
        //            }
        //            viewmodel.CategorysList = listCategory;
        //        }
        //        else if (level == 2)
        //        {
        //            viewmodel.CategoryAttributes = categoryBll.GetCategoryAttribute(c,base.language);
        //        }
        //        #endregion

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error(ex);
        //        return Redirect("/home/error");
        //    }

        //    return View(viewmodel);
        //}
        /// <summary>
        /// 异步请求商品列表
        /// </summary>
        /// <param name="c"></param>
        /// <param name="level"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="attrids"></param>
        /// <returns></returns>
        public JsonResult ProductList(string c, int level, int page, int sort = 1, string attrData = "")
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
                var model = GetProductListNew(NewCategoryArray, level, page, sort, attrData);
                return Json(new { Type = 1, Data = new { PageIndex = model.PageIndex, PageSize = PageSize, TotalRecord = model.TotalRecord, PageCount = model.PageCount, Products = model.Products } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 属性筛选 暂时不用2016.3.18
        /// </summary>
        /// <param name="c"></param>
        /// <param name="level"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="attrids"></param>
        /// <returns></returns>
        private ProductListModel GetProductList(int c, int level, int page, int sort = 1, string attrids = "")
        {
            ProductListModel viewmodel = new ProductListModel();
            try
            {
                var fileterModel = new List<ProductFilterAttrubile>();
                if (!string.IsNullOrEmpty(attrids))
                {
                    fileterModel = JsonHelper.ToObject<List<ProductFilterAttrubile>>(attrids);
                }

                int totalRecords = 0, pagecount = 0;


                var attributeList = categoryBll.GetCategoryAttribute(c, base.language);
                viewmodel.Products = productBll.GetProductList(c, level, fileterModel, sort, page, PageSize, base.language, base.DeliveryRegion, ref attributeList, out totalRecords);

                if (viewmodel.Products != null && viewmodel.Products.Count > 0)
                {
                    foreach (var p in viewmodel.Products)
                    {
                        p.MinPrice = (p.MinPrice * ExchangeRate).ToNumberRound(2);
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
        /// 筛选项只筛品牌2016.3.18
        /// </summary>
        /// <param name="c"></param>
        /// <param name="level"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="attrids"></param>
        /// <returns></returns>
        private ProductListModel GetProductListNew(string[] c, int level, int page, int sort = 1, string brandIds = "")
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
                var attributeList = categoryBll.GetCategoryAttribute(Convert.ToInt32(c[0]), base.language);
                viewmodel.Products = productBll.GetProductListNew(c, level, ids, sort, page, PageSize, base.language, base.DeliveryRegion, base.ExchangeRate, ref attributeList, out totalRecords);

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
        #endregion

        /// <summary>
        /// 获取子分类
        /// </summary>
        /// <param name="categoryid"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public JsonResult GetSubCategory(int categoryid, int level)
        {
            try
            {
                var list = categoryBll.GetCategorys(categoryid, level, base.language);
                return Json(new { Type = 1, Data = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 取三级分类的筛选项
        /// </summary>
        /// <param name="categoryid"></param>
        /// <returns></returns>
        public JsonResult GetCategoryAttributes(int categoryid)
        {
            try
            {
                var attrbutes = categoryBll.GetCategoryAttribute(categoryid, base.language);

                var attr = attrbutes.GroupBy(x => x.KeyName).Select(a => a.First()).ToList();

                List<CateAttribute> list = new List<CateAttribute>();
                foreach (var a in attr)
                {
                    var item = attrbutes.FindAll(r => r.KeyName == a.KeyName);
                    List<string> valueList = new List<string>();
                    foreach (var i in item)
                    {
                        valueList.Add(i.SubKeyValue);
                    }
                    list.Add(new CateAttribute() { KeyName = a.KeyName, Name = a.KeyValue, KeyValues = valueList, IsSkuAttr = a.IsSkuAttr });
                }

                return Json(new { Type = 1, Data = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 一级或二级商品列表
        /// </summary>
        /// <param name="c">分类categoryid</param>
        /// <param name="level">分类等级categoryLevel</param>
        /// <param name="attrids">筛选属性ids</param>
        /// <param name="sort">排序：0上新排序，1升序，2降序</param>
        /// <returns></returns>
        public ActionResult Index(string c, int level, int sort = 1)
        {
            ProductListModel viewmodel = new ProductListModel();
            try
            {
                ViewBag.LoginUser = LoginUser;

                if (string.IsNullOrEmpty(c))
                {
                    LogHelper.Error("------商品列表Index---1---解析Category参数-----");
                    return Redirect("/home/error");
                }

                // 解析分类id
                string NewCategoryString = c.Replace('|',',');
                int CategoryIdArrayLendth = c.Split('|').Length;

                if (string.IsNullOrEmpty(NewCategoryString))
                {
                    LogHelper.Error("------商品列表Index---1---解析Category参数-----");
                    return Redirect("/home/error");
                }

                // 是否启用别名
                bool aliasesRes = false;

                // 类目别名
                string CategoryAliseName = string.Empty;

                // 多个CategoryId
                if (CategoryIdArrayLendth > 1)
                {
                    // 启用别名
                    aliasesRes = true;

                    // CategoryId映射配置文件存在
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings[NewCategoryString] != null
                        && System.Web.Configuration.WebConfigurationManager.AppSettings[NewCategoryString].Count() != 0)
                    {
                        // CategoryId映射配置文件中包含参数CategoryId
                        if (!string.IsNullOrEmpty(System.Web.Configuration.WebConfigurationManager.AppSettings[NewCategoryString].ToString()))
                        {
                            CategoryAliseName = System.Web.Configuration.WebConfigurationManager.AppSettings[NewCategoryString].ToString();
                        }
                    }
                }

                // 不需要获取分类别名
                if (!aliasesRes)
                {
                    // 获取分类详细信息
                    CategoryEntity cateModel = categoryBll.GetCategoryEntity(Convert.ToInt32(NewCategoryString), level, base.language);
                    if (cateModel != null)
                    {
                        viewmodel.CategoryName = cateModel.CategoryName;
                    }
                }
                else
                {
                    viewmodel.CategoryName = CategoryAliseName;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Redirect("/home/error");
            }

            return View(viewmodel);
        }

        public ActionResult FightIndex(string spu)
        {
            if (!string.IsNullOrEmpty(spu))
            {
                Session["flagSpu"] = spu;
            }
            return View();
        }

        /// <summary>
        /// 拼生活异步请求商品列表
        /// </summary>
        /// <param name="c"></param>
        /// <param name="level"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="attrids"></param>
        /// <returns></returns>
        public JsonResult FightListIndex(int PageIndex = 1)
        {

            try
            {
                string flagSpu = (string)Session["flagSpu"];
                string startTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinStartTime"].ToString();
                string endTime = System.Web.Configuration.WebConfigurationManager.AppSettings["PinEndTime"].ToString();
                //var model = GetProductList(c, level, page, sort, attrData);
                var model = GetProductFightList(PageIndex, base.ExchangeRate, flagSpu);
                Session["flagSpu"] = null;
                string ImageServer = System.Web.Configuration.WebConfigurationManager.AppSettings["ImageServer"].ToString();

                bool IsTrue = DateTime.Now >= Convert.ToDateTime(startTime) && DateTime.Now <= Convert.ToDateTime(endTime);

                return Json(new { Type = 1, ImageServer = ImageServer, IsTrue = IsTrue, Data = new { PageIndex = model.PageIndex, PageSize = PageSize, PageCount = model.PageCount, TotalRecords = model.TotalRecord, Products = model.Products } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 拼生活列表页
        /// </summary>
        /// <param name="c"></param>
        /// <param name="level"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="attrids"></param>
        /// <returns></returns>
        private ProductListModel GetProductFightList(int PageIndex, decimal ExchangeRate, string flagSpu)
        {
            ProductListModel viewmodel = new ProductListModel();
            try
            {
                var fileterModel = new List<ProductFilterAttrubile>();

                int totalRecords = 0, pagecount = 0;
                var products = productBll.GetProductFightList(PageIndex, PageSize, base.language, ExchangeRate, flagSpu);
                if (products != null && products.Count() > 0)
                {
                    totalRecords = products.FirstOrDefault().TotalRecord;
                }
                else
                {
                    totalRecords = 0;
                    products = new List<ProductInfoModel>();
                }
                List<ProductInfoModel> productsList = new List<ProductInfoModel>();
                foreach (ProductInfoModel product in products)
                {
                    string neight = commonBll.getProductDetailName(product.MainDicValue, product.SubDicValue, product.NetWeightUnit);
                    if (!neight.Equals(""))
                    {
                        product.MainValue = Convert.ToDecimal(product.MainValue).ToNumberStringFloat();
                    }
                    product.NetWeightUnit = neight;
                    productsList.Add(product);
                }
                viewmodel.Products = productsList;
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
        /// 拼生活的商品详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult FightDetail(string sku, int pid)
        {
            IList<ProductInfoModel> productsDetail = new List<ProductInfoModel>();
            ProductInfoModel productInfoModel = new ProductInfoModel();
            IList<ProductInfoModel> images = new List<ProductInfoModel>();
            try
            {
                productsDetail = productBll.GetProductFightDetail(sku, base.ExchangeRate, pid);
                productInfoModel = productsDetail.FirstOrDefault();
                if (!string.IsNullOrEmpty(productInfoModel.NationalFlag))
                {
                    productInfoModel.NationalFlag = productInfoModel.NationalFlag + PathHelper.NationalFlagImageExtension;
                }
                string neight = commonBll.getProductDetailName(productInfoModel.MainDicValue, productInfoModel.SubDicValue, productInfoModel.NetWeightUnit);
                if (!neight.Equals(""))
                {
                    productInfoModel.MainValue = Convert.ToDecimal(productInfoModel.MainValue).ToNumberStringFloat();
                }
                productInfoModel.NetWeightUnit = neight;
                int realTaxType = Utility.Uitl.TotalTaxHelper.GetRealTaxType(productInfoModel.ReportStatus, productInfoModel.IsCrossBorderEBTax, StringUtils.ToAmt(productInfoModel.MinPrice));
                var taxPrice = StringUtils.ToAmt(TotalTaxHelper.GetTotalTaxAmount(realTaxType, StringUtils.ToAmt(productInfoModel.MinPrice), productInfoModel.CBEBTaxRate, productInfoModel.ConsumerTaxRate, productInfoModel.VATTaxRate, productInfoModel.PPATaxRate));
                int minRealTaxType = Utility.Uitl.TotalTaxHelper.GetRealTaxType(productInfoModel.ReportStatus, productInfoModel.IsCrossBorderEBTax, StringUtils.ToAmt(productInfoModel.DiscountPrice));
                var minRatePrice = StringUtils.ToAmt(TotalTaxHelper.GetTotalTaxAmount(minRealTaxType, StringUtils.ToAmt(productInfoModel.DiscountPrice), productInfoModel.CBEBTaxRate, productInfoModel.ConsumerTaxRate, productInfoModel.VATTaxRate, productInfoModel.PPATaxRate));

                productInfoModel.realTaxType = realTaxType;
                productInfoModel.minRealTaxType = minRealTaxType;
                productInfoModel.taxPrice = taxPrice;
                productInfoModel.minRatePrice = minRatePrice;
                foreach (var product in productsDetail)
                {
                    images.Add(product);
                }
                productInfoModel.Images = images;
                //var sortProductInfoModel=productInfoModel
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View(productInfoModel); ;
        }
        /// <summary>
        /// 拼生活的规则
        /// </summary>
        public ActionResult pinRule()
        {
            return View();
        }
        /// <summary>
        /// 获取商品详情
        /// </summary>
        public JsonResult checkQty(string sku, int pid)
        {
            IList<ProductInfoModel> productsDetail = new List<ProductInfoModel>();
            ProductInfoModel productInfoModel = new ProductInfoModel();
            productsDetail = productBll.GetProductFightDetail(sku, base.ExchangeRate, pid);
            productInfoModel = productsDetail.FirstOrDefault();
            return Json(new { ForOrderQty = productInfoModel.ForOrderQty }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PinEnd()
        {
            return View();
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
                var skus = new List<string>();
                int qty = 0, minQty = 0;
                decimal minPrice = 0M, discountPrice = 0M;
                var table = productBll.GetStockInfoBySpu(spu, base.language,base._deliveryRegion);
                string[] spus = { spu };
                var promotions = productBll.GetAvaliablePromotionPrice(spus);
                if (table != null && table.Rows.Count > 0)
                {
                    //使用Enumerable方式（选用）
                    //var list = table.AsEnumerable();
                    //var firstSkuModel = list.FirstOrDefault();
                    //minQty = firstSkuModel.Field<int>("MinForOrder");
                    //minPrice = firstSkuModel.Field<decimal>("MinPrice");
                    //qty = list.Sum(x => x.Field<int>("ForOrderQty"));

                    //传统方式
                    minQty = table.Rows[0]["MinForOrder"].As(0);
                    minPrice = table.Rows[0]["MinPrice"].As(0M);
                    skus.Add(table.Rows[0]["Sku"].ToString());                    
                    foreach (DataRow row in table.Rows)
                    {
                        qty += row["ForOrderQty"].As(0);
                    }
                    //如果有促销产品，则进行判断促销产品的打折价跟商品最小价格作对比
                    if (promotions != null && promotions.Count == 1)
                    {
                        discountPrice = promotions.FirstOrDefault().DiscountPrice;
                        if (discountPrice < minPrice)
                        {
                            skus.Clear();
                            skus.Add(promotions.FirstOrDefault().Sku);
                        }
                    }
                    return (qty - minQty > 0) ? (Json(new { Type = 1, Content = "返回成功！", Data = new { IsHaveStock = 1, Skus = skus, SkuCount = table.Rows.Count } }, JsonRequestBehavior.AllowGet)) : (Json(new { Type = 0, Content = "您要订购的商品库存量不足。", Data = new { IsHaveStock = 0, Skus = skus, SkuCount = table.Rows.Count } }, JsonRequestBehavior.AllowGet));
                }
                return Json(new { Type = 0, Content = "对不起，没有找到对应的商品！" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0, Content = "获取数据失败！" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public ActionResult PList(int page = 1, int sort = 1)
        {
            int sourcetype = base.LoginUser == null ? 0 : LoginUser.SourceType;

            //优先从缓存中获取
            ListProductInfoModel listmodel = productBll.GetPListFromCache(sort, page, PageSize, base.language, base.DeliveryRegion, sourcetype);
            return View(listmodel);
        }
    }
}
