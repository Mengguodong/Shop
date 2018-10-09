using SFO2O.BLL.Product;
using SFO2O.M.ViewModel.Product;
using SFO2O.Model.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SFO2O.BLL.Category;
using SFO2O.Utility.Uitl;
using SFO2O.M.Controllers.Extensions;
using SFO2O.Model.Product;
using SFO2O.Utility.Extensions;
using SFO2O.Utility;
using SFO2O.BLL.Common;
using System.Data;
using SFO2O.Model.Extensions;
using SFO2O.BLL.Item;
using SFO2O.BLL.Holiday;

namespace SFO2O.M.Controllers
{
    /// <summary>
    /// 节日<当前：中秋月饼>2016.07.26
    /// </summary>
    public class HolidayController : SFO2OBaseController
    {
        ProductBll productBll = new ProductBll();
        ItemBll itemBll = new ItemBll();
        HolidayBll holidayBll = new HolidayBll();

        //默认加载页
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="cid">类目ID</param>
        /// <param name="level">类目级别</param>
        /// <param name="sort">排序方式</param>
        /// <param name="pageindex">第几页</param>
        /// <param name="pagesize">每页显示多少条数据</param>
        /// <returns></returns>
        public ActionResult GetProductList(string cid, int level, int sort, int pageindex, int pagesize)
        {
            ProductListModel viewmodel = new ProductListModel();
            List<CategoryAttribute> attributeList=new List<CategoryAttribute>();
            try
            {
                int totalRecords = 0;
                string[] c = new string[] { cid };
                viewmodel.Products = productBll.GetProductListNew(c, level, new List<int>(), sort, pageindex, pagesize, base.language, base.DeliveryRegion, base.ExchangeRate, ref attributeList, out totalRecords);
                viewmodel.TotalRecord = totalRecords;
                List<string> spus = viewmodel.Products.Select(x => x.SPU).ToList();
                var spuSkus=productBll.GetSkuBySpu(spus).Select();
                if (viewmodel.Products != null && viewmodel.Products.Count > 0)
                {
                    foreach (var p in viewmodel.Products)
                    {
                        p.MinPrice = (p.MinPrice * ExchangeRate).ToNumberRound(2);
                        p.DiscountPrice = (p.DiscountPrice * ExchangeRate).ToNumberRound(2);
                        p.DiscountRate = Convert.ToDecimal(p.DiscountRate).ToNumberStringFloat();
                        p.Sku = Convert.ToString(spuSkus.Where(x => Convert.ToString(x["Spu"]) == p.SPU).FirstOrDefault()["sku"]);
                    }
                }
            }
            catch(Exception ex)
            {
                LogHelper.Error(ex);
            }
            return this.PartialView("ProductList", viewmodel);
        }

        #region 时令产品---单品页
        public ActionResult Item(string productCode, string selectedSku = "")
        {
            try
            {
                IList<ProductImage> images = null;
                //获取单品页商品
                int userId = 0;
                if (base.LoginUser != null)
                {
                    userId = base.LoginUser.UserID;
                }
                var itemskus = productBll.GetItemByProductCode(productCode, base.language, ref images, userId);
                if (itemskus.Count() > 0)
                {
                    var item = itemskus.FirstOrDefault();
                    string spu = item.Spu;

                    //获取收藏表的信息
                    bool isflag = false;
                    if (base.LoginUser != null)
                    {
                        isflag = productBll.getFivoriteDetail(spu, base.LoginUser.UserID);
                    }

                    //获取促销信息
                    var promotions = itemBll.GetPromotionEntities(itemskus.Select(n => n.Sku).ToArray());
                    //判断这个spu是不是在做拼生活活动（用于前端是否显示拼生活图标链接）
                    var flag = false;
                    if (promotions.Where(d => d.PromotionType == 2).Count() > 0)
                    {
                        flag = true;
                    }
                    //促销商品，过滤掉拼团(PromotionType=2)商品，留下打折商品PromotionType=1
                    promotions = promotions.Where(d => d.PromotionType != 2).ToList();
                    //组装DTO--pageload
                    var product = itemskus.ToArray().AsDto(base.ExchangeRate, promotions, isflag);
                    //  查询 拼生活里面 是否有这个spu
                    if (flag)
                    {
                        product.isTrue = 1;
                    }

                    product.Images = images.ToArray();

                    //组装skuAttributeViewModel-- for js skuSelected
                    var skuAttributeViewModel = itemskus.AsSkuAttributeViewModel(base.ExchangeRate, promotions, selectedSku, dto: product);

                    bool skuFlag = itemskus.GroupBy(d => d.Sku).Count() > 1;
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
                    }
                    //装配节日时令产品的：展示属性（净重、规格、口味）以及（毛重Weight用于判断能卖几件商品）=>e.g月饼清关规定：不能超过3Kg,不能超过两件，不能超过800元
                    HolidayProductAttributeViewModel holidayProductAttrVM = new HolidayProductAttributeViewModel();
                    var productExpand = productBll.GetProductExpandEntity(spu, base.language);
                    if (productExpand != null)
                    {
                        holidayProductAttrVM.Weight = productExpand.Weight;
                        holidayProductAttrVM.Flavor = productExpand.Flavor;
                        holidayProductAttrVM.Specifications = item.Specifications;
                        holidayProductAttrVM.NetWeight = item.NetWeight;
                    }
                    //装配ViewBag
                    ViewBag.HolidayAttributeViewModel = holidayProductAttrVM;
                    ViewBag.SkuMetaViewModel = skuAttributeViewModel;
                    ViewBag.CanBuyNum = holidayBll.GetCanBuyNumberByWeight(holidayProductAttrVM.Weight);
                    return PartialView("_Item", product);
                }
                else
                {
                    return PartialView("_Item", new ProductDto());
                }
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
        #endregion
    

        #region 计算限购数量,(判断依据：毛重&&金额)
        public decimal GetCanBuyNumber(HolidayProductAttributeViewModel attrModel, ProductDto proModel)
        {
            decimal price = 0M;
            //根据价格判断（虽然一般月饼 客单价不会超过这个数（800元）还是加个判断比较靠谱）
            if (proModel.Promotion != null)
            {
                price = proModel.PromotionDiscountPrice;
            }
            else
            {
                price = proModel.MinPrice;
            }
            return holidayBll.GetCanBuyNumber(attrModel.Weight, price);
        }
        #endregion
    }
}
