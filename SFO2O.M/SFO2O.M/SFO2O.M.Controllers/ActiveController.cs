using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SFO2O.BLL.Product;
using SFO2O.BLL.Item;
using SFO2O.BLL.Activity;
using SFO2O.Model.Product;
using SFO2O.Utility.Uitl;
using SFO2O.BLL.Common;
using SFO2O.Utility.Cache;
using SFO2O.M.ViewModel.Product;
using SFO2O.Model.Common;
using SFO2O.Model.Extensions;
using System.Data;
using SFO2O.Model.Category;
using SFO2O.Model.Promotion;
using SFO2O.References.IndexingService;
using SFO2O.Utility.Extensions;
using System.Management.Instrumentation;
using SFO2O.M.Controllers.Extensions;
using SFO2O.M.ViewModel.Activity;
using SFO2O.Model.Activity;
using SFO2O.BLL.Supplier;

namespace SFO2O.M.Controllers
{
    public class ActiveController : SFO2OBaseController
    {
        ProductBll bll = new ProductBll();
        ItemBll itemBll = new ItemBll();
        ActivityBll activityBll = new ActivityBll();
        BrandBll brandBll = new BrandBll();
        /// <summary>
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult preheat()
        {
            DateTime dtStartTime = DateTime.Now;
            DateTime dtEndTime = Convert.ToDateTime("2016-04-28 00:00:00");
            double sec = dtEndTime.Subtract(dtStartTime).TotalSeconds;
            if (sec > 0)
            {
                ViewBag.Sec = sec;
            }
            else
            {
                ViewBag.Sec = 0;
            }
            ViewBag.Preheat = 1;
            ViewBag.Description = "港货平台，「爱玖网」强势来袭！港货正品，全场包邮，由物流从中华人民共和国大陆地区直送全国各地。";

            if (DateTime.Now < dtEndTime)
            {
                return View();
            }
            else
            {
                return RedirectToAction("PinEnd", "Product");
            }
        }
        public ActionResult SkuInfo(string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
            {

                return HandleError("无法找到该商品");
            }
            IList<ProductImage> images = null;
            //获取单品页商品
            int userId = 0;
            if (base.LoginUser.UserID != null)
            {
                userId = base.LoginUser.UserID;
            }
            var itemskus = bll.GetItemByProductCode(productCode, base.language, ref images, userId);
            //二期：获取促销信息
            var promotions = itemBll.GetPromotionEntities(itemskus.Select(n => n.Sku).ToArray());
            //二期：获取品牌信息（获取3种语言版本）
            //组装DTO--pageload
            var product = itemskus.ToArray().AsDto(base.ExchangeRate, promotions, false);
            product.Images = images.ToArray();
            return PartialView("_skulist", product);
        }
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
        /// 母亲节活动专题一
        /// </summary>
        /// <returns></returns>
        public ActionResult MotherDay()
        {
            long now = DateTime.Now.Ticks;
            long startTime = DateTime.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["MotherDayStartTime"].ToString()).Ticks;
            long endTime = DateTime.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["MotherDayEndTime"].ToString()).Ticks;

            if ((now > startTime) && (now < endTime))
            {
                return View();
            }
            else
            {
                return RedirectToAction("PinEnd", "Product");
            }
        }

        /// <summary>
        /// 拼生活专题
        /// </summary>
        /// <returns></returns>
        public ActionResult PinLife()
        {
            long now = DateTime.Now.Ticks;
            long startTime = DateTime.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["PinStartTime"].ToString()).Ticks;
            long endTime = DateTime.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["PinEndTime"].ToString()).Ticks;

            if ((now > startTime) && (now < endTime))
            {
                return View();
            }
            else
            {
                return RedirectToAction("PinEnd", "Product");
            }
        }

        /// <summary>
        /// 六一儿童节专题
        /// 添加日期：2016-5-17
        /// </summary>
        /// <returns></returns>
        public ActionResult ChildrenDay()
        {
            var model = activityBll.GetActivityByKey("childrenDayShared");
            if (model != null)
            {
                DateTime now = DateTime.Now;
                DateTime startTime = model.StartTime;
                DateTime endTime = model.EndTime;

                if ((now > startTime) && (now < endTime))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("PinEnd", "Product");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// 2016.6.15日专题
        /// </summary>
        /// <returns></returns>
        public ActionResult Activity0615()
        {
            var model = activityBll.GetActivityByKey("activity0615Shared");
            if (model != null)
            {
                DateTime now = DateTime.Now;
                DateTime startTime = model.StartTime;
                DateTime endTime = model.EndTime;

                if ((now > startTime) && (now < endTime))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("PinEnd", "Product");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// 2016.7.1日专题
        /// </summary>
        /// <returns></returns>
        public ActionResult Activity0701()
        {
            int viewType = ActivityCommon("activity0701Shared");
            switch (viewType)
            {
                case 1://返回活动视图
                    return View();
                case 2://返回过期视图
                    return RedirectToAction("PinEnd", "Product");
                case 3://返回首页
                    return RedirectToAction("Index", "Home");
                default://如果没有此活动，默认返回首页
                    return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// 专题通用方法
        /// </summary>
        /// <param name="activityKey">活动</param>
        /// <returns>1.正常访问本View专题  2.返回不再活动期间页面   3.没有此专题，直接返回首页</returns>
        public int ActivityCommon(string activityKey)
        {
            var model = activityBll.GetActivityByKey(activityKey);
            if (model != null)
            {
                DateTime now = DateTime.Now;
                DateTime startTime = model.StartTime;
                DateTime endTime = model.EndTime;
                //是否在活动期间
                return (now > startTime) && (now < endTime) ? 1 : 2;
            }
            return 3;
        }

        /// <summary>
        /// 模板页母版
        /// </summary>
        /// <param name="activityName">e.g:160726</param>
        /// <returns>对应的专题模板1、2、3</returns>
        public ActionResult ActivityTemplate(string activityName)
        {
            if (!string.IsNullOrEmpty(activityName))
            {
                string activityKey = string.Format("activity{0}Shared", activityName);
                var model = activityBll.GetActivityByKey(activityKey);
                if (model != null)
                {
                    DateTime now = DateTime.Now;
                    DateTime startTime = model.StartTime;
                    DateTime endTime = model.EndTime;                    

                    //是否在活动期间
                    if ((now > startTime) && (now < endTime))
                    {
                        int tempType = model.TempType;
                        var viewmodel = CreateActivityViewModel(model, activityName);
                        return View(viewmodel);
                    }
                    //否则返回活动结束页面
                    return RedirectToAction("PinEnd", "Product");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 创建专题模板视图对象
        /// </summary>
        /// <param name="model">专题活动实体Model</param>
        /// <param name="activityName">活动编号如：0701</param>
        /// <returns>ActivityViewModel</returns>
        public ActivityViewModel CreateActivityViewModel(ActivityModel model, string activityName)
        {
            ViewBag.ExchangeRate = base.ExchangeRate;
            //拆分页面模块，获取模块标题以及对应的spu集合
            ActivityViewModel viewmodel = new ActivityViewModel();
            //Key如："0601"  可以用于找到模板对应的0601素材文件夹
            viewmodel.Key = activityName;
            viewmodel.Title = model.Title;
            viewmodel.HeadTitle = model.HeadTitle;
            viewmodel.Discription = model.Discription;
            viewmodel.ImgPath = model.ImgPath;
            viewmodel.TempType = model.TempType;
            string[] sections = model.SPUs.Split('|');

            //为模板装配模块数据
            List<ActivityModule> modules = new List<ActivityModule>();
            //1品牌模板 和 2分类模板
            if (viewmodel.TempType != 3)
            {
                foreach (var section in sections)
                {
                    string[] sectionSplit = section.Split(':');
                    ActivityModule module = new ActivityModule();
                    if (viewmodel.TempType == 1)
                    {
                        module.ModuleName = sectionSplit[0];
                    }
                    else
                    {
                        var sectionHead = sectionSplit[0].Split(',');//示例：  保健品，product/index?level=0&c=1 以逗号隔开
                        module.ModuleName = sectionHead[0];
                        module.ModuleLinkURL = sectionHead[1];
                    }
                    //module.ProductList = bll.GetProductListBySpus(sectionSplit[1]).ToList();
                    //换做从Redis缓存中各个模块数据
                    module.ProductList=bll.GetProductListBySpusFromCache(sectionSplit[1],activityName,module.ModuleName).ToList();
                    modules.Add(module);
                }
            }
            else//品牌模板
            {
                ActivityBrandViewModel brand = new ActivityBrandViewModel();
                var brandArr = sections[0].Split(',');
                brand.BrandId = brandArr[0].As(0);
                brand.BrandName = brandArr[1];
                brand.BrandDescription = brandBll.GetBrandInfo(brand.BrandId, base.language).IntroductionCN;
                viewmodel.BrandInfo = brand;
                //将主推产品的介绍，加入字典，在后面装配给响应的spu商品
                Dictionary<string, string> mainProductDiscriptions = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(model.MainProductDescription))
                {
                    string[] mpd = model.MainProductDescription.Split('|');
                    foreach (var item in mpd)
                    {
                        string[] dic=item.Split(':');
                        mainProductDiscriptions.Add(dic[0], dic[1]);
                    }
                }
                //装配品牌模板的每个模块数据
                for (int i = 1; i < sections.Length; i++)
                {
                    string[] sectionSplit = sections[i].Split(':');
                    ActivityModule module = new ActivityModule();
                    module.ModuleName = sectionSplit[0];
                    //List<ProductInfoModel> plist = bll.GetProductListBySpus(sectionSplit[1]).ToList();
                    //换做从Redis缓存中各个模块数据
                    List<ProductInfoModel> plist = bll.GetProductListBySpusFromCache(sectionSplit[1], activityName, module.ModuleName).ToList();

                    //为主推产品添加专有描述
                    if (module.ModuleName.Trim()=="主推产品")
                    {
                        plist.ForEach(x=>x.Description=mainProductDiscriptions[x.SPU]);
                    }
                    module.ProductList = plist;
                    modules.Add(module);
                }
            }
            viewmodel.Modules = modules;
            return viewmodel;
        }
    }
}
