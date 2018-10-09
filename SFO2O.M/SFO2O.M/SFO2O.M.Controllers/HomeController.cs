using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using System.Linq;
using SFO2O.BLL.Exceptions;
using SFO2O.BLL.Index;
using SFO2O.Model;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Enum;
using SFO2O.Model.Index;
using SFO2O.BLL.Account;
using SFO2O.Model.Account;
using SFO2O.Model.GiftCard;
using SFO2O.Utility.Extensions;
using SFO2O.Model.CMS;
using System.Collections;

namespace SFO2O.M.Controllers
{
    public class HomeController : SFO2OBaseController
    {
        private readonly IndexModulesBll indexModuleBll = new IndexModulesBll();
        /// <summary>
        /// 2017年5月22日创建新的index内容
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //新的index内容，原index备份到Original_Index
            ViewBag.LoginUser = base.LoginUser;
            ViewBag.ShowHeader = false;
            ViewBag.ShowFooter = false;

            return View();
        }

        /// <summary>
        /// 首页轮播焦点图
        /// </summary>
        /// <returns></returns>
        public ActionResult ScrollImage()
        {
            IList<BannerImagesEntity> listTopModule = indexModuleBll.GetIndexBannerImagesFromCache();
            return PartialView("_ScrollImage", listTopModule);
        }
        public ActionResult Bulletin(int top)
        {
            List<BulletinEntity> list = indexModuleBll.GetBulletinEntities(top).ToList();
            return PartialView("_Bulletin", list);
        }
        public ActionResult CategoryModule1(int top)
        {
            List<IndexModulesEntity> list = indexModuleBll.GetIndexModulesFromCache(IndexModuleType.CategroyModule1, top).ToList();
            return PartialView("_CategoryModule1", list);
        }
        public ActionResult CategoryModule2(int top)
        {
            List<IndexModulesEntity> list = indexModuleBll.GetIndexModulesFromCache(IndexModuleType.CategroyModule2, top).ToList();
            return PartialView("_CategoryModule2", list);
        }

        /// <summary>
        /// 首页新品推荐
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public ActionResult NewProductRecommend(int top = 5)
        {
            try
            {
                ViewBag.ExchangeRate = base.ExchangeRate;
                var list = indexModuleBll.GetIndexNewProduct(top, base.language, base.DeliveryRegion).ToList();
                return PartialView("_NewProductRecommend", list);
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }

        }

        /// <summary>
        /// 首页自定义模块
        /// </summary>
        /// <returns></returns>
        public ActionResult CMSCustomProduct()
        {
            try
            {
                // 汇率
                ViewBag.ExchangeRate = base.ExchangeRate;
                // 获取首页自定义模块数据
                IList<IndexModulesEntity> AllList = indexModuleBll.GetIndexCustom(base.language, base.DeliveryRegion).ToList();
                Hashtable DataMap = new Hashtable();

                // 构建自定义模块Module
                indexModuleBll.BuildCMSCustomModule(AllList, DataMap);

                // 构建自定义模块Banner
                indexModuleBll.BuildCMSCustomBanner(AllList, DataMap);

                // 构建自定义模块Product
                indexModuleBll.BuildCMSCustomProduct(AllList, DataMap);

                IList<Hashtable> HashtableList = new List<Hashtable>();
                HashtableList.Add(DataMap);
                return PartialView("_CMSCustomProduct", HashtableList);
            }
            catch (Exception ex)
            {
                return this.HandleError(ex); ;
            }
        }

        /// <summary>
        /// V2.0 热门商品区（大/搜/罗）
        /// </summary>
        /// <param name="top">前N条数据</param>
        /// <returns>热门商品集合</returns>
        public ActionResult CMSHotProduct(int top = 20)
        {
            ViewBag.ExchangeRate = base.ExchangeRate;
            List<IndexModulesProductEntity> list = indexModuleBll.GetAllCMSHotProductsFromCache(base.language, base._deliveryRegion).Take(top).ToList();
            return View("_CMSHotProduct", list);
        }

        //手工添加更新
        public ActionResult PiliangUpdate()
        {
            //string[] phones = { "15546799887" };
            //string errorMsg = string.Empty;
            //foreach (string phone in phones)
            //{
            //    if (!phone.IsMobilePhoneNum(true))
            //    {
            //        errorMsg += phone + "&&";
            //        continue;
            //    }
            //    string thisPhone = phone.Trim();
            //    //先判定有无注册用户
            //    AccountBll account = new AccountBll();
            //    int UserId = 0;
            //    Random rd = new Random();
            //    string pwd = rd.Next(100000,999999).ToString();
            //    int userType = 0;
            //    if (!account.IsExistsUserName(thisPhone, "86"))
            //    {
            //        //没有就插入用户表
            //        CustomerEntity entity = new CustomerEntity()
            //        {
            //            Mobile = thisPhone,
            //            UserName = thisPhone,
            //            Email = "",
            //            Password = pwd,
            //            Gender = 0,
            //            RegionCode = "86"
            //        };
            //        UserId = account.Insert(entity);
            //        userType = 0;
            //    }
            //    else
            //    {
            //        UserId = account.GetUserInfoByUserName(thisPhone).ID;
            //        userType = 1;
            //        pwd = string.Empty;
            //    }
            //    if (UserId == 0)
            //    {
            //        errorMsg += thisPhone + "&&";
            //        continue;
            //    }
            //    if (account.GetUserTempByUserName(thisPhone) > 0)
            //    {
            //        errorMsg += phone + "||";
            //        continue;
            //    }
            //    //先写入用户临时表
            //    account.InsertTemp(thisPhone, pwd, userType);
            //    //获取到当前的用户ID然后送优惠券
            //    var loginInfo = new CustomerEntity()
            //    {
            //        ID = UserId,
            //        CreateTime = DateTime.Now
            //    };
            //    GiftCardBatchEntity giftcardBatch = new GiftCardBatchEntity()
            //    {
            //        BatchId = 6,
            //        BatchName = "丰满券",
            //        CardType = 2,
            //        CardSum = 20,
            //        SatisfyPrice = 159,
            //        SatisfyProduct = 3,
            //        ExpiryDays = 6
            //    };
            //    GiftCardEntity GiftCardEntity = account.AddGiftCard(giftcardBatch, loginInfo);
            //}
            AccountBll account = new AccountBll();
            var userInfos = account.GetUserInfo();
            string errorMsg = "成功！";
            foreach(var userInfo in userInfos)
            {
                if (account.GetUserTempByUserName(userInfo.UserName) > 0)
                {
                    errorMsg += userInfo.UserName + "||";
                    continue;
                }
                account.InsertTemp(userInfo.UserName, "", 0);
                var loginInfo = new CustomerEntity()
                {
                    ID = userInfo.ID,
                    CreateTime = DateTime.Now
                };
                GiftCardBatchEntity giftcardBatch = new GiftCardBatchEntity()
                {
                    BatchId = 35,
                    BatchName = "丰满券",
                    CardType = 2,
                    CardSum = 20,
                    SatisfyPrice = 159,
                    SatisfyProduct = 3,
                    ExpiryDays = 6
                };
                GiftCardEntity GiftCardEntity = account.AddGiftCard(giftcardBatch, loginInfo);
            }
            return Content(errorMsg);
        }
        /// <summary>
        /// 2017年5月22日备份原有Index内容
        /// </summary>
        /// <returns></returns>
        public ActionResult Original_Index() {
            ViewBag.LoginUser = base.LoginUser;
          
            return View();
        }
    }
}
