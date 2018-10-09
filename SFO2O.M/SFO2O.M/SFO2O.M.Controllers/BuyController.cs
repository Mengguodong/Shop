using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using SFO2O.BLL.Product;
using SFO2O.BLL.Shopping;
using SFO2O.M.Controllers.BaseControllers;
using SFO2O.M.Controllers.Filters;
using SFO2O.Model.Product;
using SFO2O.Utility.Uitl;
using SFO2O.Utility.Extensions;
using SFO2O.BLL.Item;
using SFO2O.BLL.Account;
using SFO2O.BLL.Team;
using SFO2O.BLL.Order;
using SFO2O.BLL.GiftCard;
using SFO2O.Model.GiftCard;
using SFO2O.Model.Team;
using SFO2O.Model.Enum;
using SFO2O.M.ViewModel.GiftCard;
using SFO2O.BLL.Holiday;

namespace SFO2O.M.Controllers
{
    /// <summary>
    /// 立即购买
    /// </summary>
    public partial class BuyController : ShoppingBaseController
    {
        private ProductBll productBll = new ProductBll();
        private ShoppingBll CartBll = new ShoppingBll();
        private ItemBll itemBll = new ItemBll();
        private AccountBll accountBll = new AccountBll();
        private TeamBll teamBll = new TeamBll();
        private GiftCardBll giftCardBll = new GiftCardBll();
        HolidayBll holidayBll = new HolidayBll();

        #region 立即购买

        [Login(Order = 1)]
        [FirstOrderAuthorize(Order = 2)]
        public ActionResult BuyNow(string sku, int qty, int? aid, string teamcode, int pid = 0)
        {
            ProductSkuEntity entity = new ProductSkuEntity();
            ViewBag.BuyQty = qty;
            ViewBag.Pid = pid;
            ViewBag.TeamCode = teamcode;
            if (aid.HasValue)
            {
                ViewBag.ChooseAddressId = aid.AsInt32();
            }
            else
            {
                ViewBag.ChooseAddressId = 0;
            }
            if (string.IsNullOrWhiteSpace(sku))
            {
                return Redirect("/home/error");
            }
            if (qty < 1)
            {
                return Redirect("/home/error");
            }
            try
            {
                entity = new ProductSkuEntity();
                entity = productBll.GetProductBySku(sku, language);
                if (pid != 0)
                {
                    var proList = itemBll.GetPromotionInfoByPid(pid);
                    if (proList.Count() == 0 || proList.FirstOrDefault().Sku != sku)
                    {
                        return Redirect("/product/FightIndex");
                    }
                }
                if (entity == null)
                {
                    return Redirect("/home/error");
                }
                if (entity.Qty < qty)
                {
                    return Redirect("/home/error");
                }
                if (entity.SalesTerritory != base.DeliveryRegion && entity.SalesTerritory != 3)
                {
                    return Redirect("/home/error");
                }
                var ping = 0;
                int proid = 0;
                if (!string.IsNullOrEmpty(teamcode))
                {
                    var teaminfo = teamBll.GetTeamInfoByTeamCode(teamcode);
                    ping = teaminfo.TeamStatus;
                    proid = teaminfo.PromotionId;
                    if (teaminfo.UserID == this.LoginUser.UserID || teaminfo.TeamStatus != 1)
                    {
                        return Redirect("/Team/teamDetail?TeamCode=" + teamcode + "&Flag=1");
                    }
                }
                var cart = base.GetBuyVirtualCart(entity, ping, proid);//计算促销信息
                var item = cart.Items.FirstOrDefault();
                if (item == null)
                {
                    return Redirect("/home/error");
                }


                entity.ProductPrice = item.SalePriceExchanged;
                var promotionsku = itemBll.GetPromotionInfoBySku(item.Sku);
                if (pid == 0 && (promotionsku.Count > 0 && promotionsku.FirstOrDefault().PromotionType == 2) && string.IsNullOrEmpty(teamcode))
                {
                    entity.ProductPrice = item.PriceExchanged;
                }

                ViewBag.IsShowHuoli = false;
                var HuoliEntity = accountBll.GetHuoliEntityByUerId(this.LoginUser.UserID);
                if (HuoliEntity == null)
                {
                    ViewBag.IsShowHuoli = true;
                }
                else
                {
                    if (HuoliEntity.HuoLiCurrent <= 0 || !string.IsNullOrEmpty(teamcode))
                    {
                        ViewBag.IsShowHuoli = true;
                    }
                }
                //========================优惠券===========================
                //如果使用酒豆，则首先检查一下是否有默认可用的优惠券，如果有，则得减掉默认优惠券面值之后再*90%得到可用优惠券
                int quanType = Convert.ToInt32(PromotionType.None);
                if (promotionsku.Count > 0)
                {

                    if (promotionsku.FirstOrDefault().PromotionType == 1)
                    {
                        quanType = Convert.ToInt32(PromotionType.Promotion);//1.打折 2.拼团   对应的枚举-->  打折：0x02 拼团0x04
                    }
                    else if (promotionsku.FirstOrDefault().PromotionType == 2)
                    {
                        if (pid != 0 || !string.IsNullOrEmpty(teamcode))
                        {
                            quanType = Convert.ToInt32(PromotionType.GroupBuy);
                        }
                    }
                }
                //获取默认优惠券
                decimal totleAmount = entity.ProductPrice * qty;

                var GiftCardNotUsedList = giftCardBll.GetCanUseGiftCardList(base.LoginUser.UserID, totleAmount, quanType);
                //转换成 ViewModel集合，并进行酒豆操作
                List<CanUseGiftCardViewModel> vmlist = new List<CanUseGiftCardViewModel>();
                //如果有酒豆
                if (!ViewBag.IsShowHuoli)
                {
                    if (GiftCardNotUsedList != null)
                    {
                        ViewBag.HuoliNoUseGiftCard = BuyOrderManager.GetCanUseHuoli(totleAmount, HuoliEntity.HuoLiCurrent, 0M);
                        ViewBag.HuoliMoneyNoUseGiftCard = (BuyOrderManager.GetCanUseHuoli(totleAmount, HuoliEntity.HuoLiCurrent, 0M) / 100).ToNumberRoundStringWithPoint();
                        var GiftCardEntityDefault = GiftCardNotUsedList.FirstOrDefault();
                        decimal cardValue = 0M;
                        if (GiftCardEntityDefault != null)
                        {
                            cardValue = GiftCardEntityDefault.CardSum;
                        }
                        foreach (var model in GiftCardNotUsedList)
                        {
                            var vm = giftCardBll.EntityToViewModel(model);
                            vm.Huoli = BuyOrderManager.GetCanUseHuoli(totleAmount, HuoliEntity.HuoLiCurrent, model.CardSum);
                            vm.Money = (BuyOrderManager.GetCanUseHuoli(totleAmount, HuoliEntity.HuoLiCurrent, model.CardSum) / 100).ToNumberRoundStringWithPoint();
                            vmlist.Add(vm);
                        }
                        decimal huoli = BuyOrderManager.GetCanUseHuoli(totleAmount, HuoliEntity.HuoLiCurrent, cardValue);
                        ViewBag.Huoli = huoli;
                    }
                    else
                    {
                        ViewBag.Huoli = BuyOrderManager.GetCanUseHuoli(totleAmount, HuoliEntity.HuoLiCurrent, 0M);
                    }
                }
                else//没有酒豆，直接返回可用的优惠券即可
                {
                    if (GiftCardNotUsedList != null)
                    {
                        foreach (var model in GiftCardNotUsedList)
                        {
                            var vm = giftCardBll.EntityToViewModel(model);
                            vmlist.Add(vm);
                        }
                    }
                }
                ViewBag.GiftCardList = vmlist;

                //========================时令美食 获得限购数量 开始===========================
                // 判断是否是节日食品
                bool IsHolidayFoods = productBll.isHolidayGoods(entity.Spu);
                if (IsHolidayFoods)
                {
                    // 根据sku获取时令美食spu信息
                    ProductExpandEntity HolidaySpuInfo = productBll.GetHolidaySpuInfoBySku(sku, language);

                    if (HolidaySpuInfo == null)
                    {
                        return Redirect("/home/error");
                    }

                    // 时令美食 月饼
                    if (Convert.ToString(HolidaySpuInfo.CategoryId).Equals(System.Web.Configuration.WebConfigurationManager.AppSettings["MoonCakeKey"].ToString()))
                    {
                        if (HolidaySpuInfo.Weight <= 0)
                        {
                            return Redirect("/home/error");
                        }

                        // 只根据重量进行限量判断：1、不能超过两件 2、不能超过3Kg
                        decimal MaxNum = holidayBll.GetCanBuyNumberByWeight(HolidaySpuInfo.Weight);

                        ViewBag.HolidayMaxNum = MaxNum;
                    }
                }

                //========================时令美食 获得限购数量 结束===========================

                //entity.ProductPrice * base.ExchangeRate;
                //entity.MinPrice = entity.MinPrice * base.ExchangeRate;
                //entity.Price = entity.Price * base.ExchangeRate;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Redirect("/home/error");
            }
            return View(entity);
        }

        /// <summary>
        /// 立即购买时检查商品
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public JsonResult CheckSku(string sku, int qty)
        {
            ProductSkuEntity entity = new ProductSkuEntity();
            ViewBag.BuyQty = qty;
            if (string.IsNullOrWhiteSpace(sku))
            {
                return Json(new { Type = 0, Content = "参数错误：缺少商品号。" }, JsonRequestBehavior.AllowGet);
            }
            if (qty < 1)
            {
                return Json(new { Type = 0, Content = "最少购买一件选定的商品。" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                entity = productBll.GetProductBySku(sku, language);
                if (entity == null)
                {
                    return Json(new { Type = 0, Content = "该商品不存在或已下架。" }, JsonRequestBehavior.AllowGet);
                }
                if (entity.Qty < qty)
                {
                    return Json(new { Type = 0, Content = "您要订购的商品库存量不足。" }, JsonRequestBehavior.AllowGet);
                }
                if (entity.SalesTerritory != base.DeliveryRegion && entity.SalesTerritory != 3)
                {
                    return Json(new { Type = 0, Content = "您要订购的商品无法在当前区域配送。" }, JsonRequestBehavior.AllowGet);
                }
                var promotionsku = itemBll.GetPromotionInfoBySku(sku);
                var price = entity.DiscountPrice;
                if (entity.DiscountPrice == 0 || (promotionsku.Count > 0 && promotionsku.FirstOrDefault().PromotionType == 2))
                {
                    price = entity.MinPrice;
                }
                int realTaxType = Utility.Uitl.TotalTaxHelper.GetRealTaxType(entity.ReportStatus, entity.IsCrossBorderEBTax, price * base.ExchangeRate);
                if (qty > 1 && realTaxType == 2 && (qty * price * base.ExchangeRate > base.OrderLimitValue))
                {
                    return Json(new { Type = 0, Content = "单笔订单金额不能超过" + base.OrderLimitValue + "元。" }, JsonRequestBehavior.AllowGet);
                }
                if (realTaxType == 1 && (qty * price * base.ExchangeRate > base.ConsolidatedPrice))
                {
                    return Json(new { Type = 0, Content = "单笔订单金额不能超过" + base.ConsolidatedPrice + "元。" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 1, Content = "", LinkUrl = "/buy/buynow?sku=" + sku + "&qty=" + qty }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 立即购买更新Qty 
        /// </summary>
        /// <param name="sku">商品SKU</param>
        /// <param name="qty">数量</param>
        /// <returns></returns>
        [Login]
        public JsonResult BuyNowUpdateQty(string sku, int qty)
        {
            ProductSkuEntity entity = new ProductSkuEntity();
            if (string.IsNullOrWhiteSpace(sku))
            {
                return Json(new { Type = 0, Content = "参数错误：缺少商品号。" }, JsonRequestBehavior.AllowGet);
            }
            if (qty < 1)
            {
                return Json(new { Type = 0, Content = "最少购买一件选定的商品。" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                entity = productBll.GetProductBySku(sku, language);
                if (entity == null)
                {
                    return Json(new { Type = 0, Content = "该商品不存在或已下架。" }, JsonRequestBehavior.AllowGet);
                }
                if (entity.Qty < qty)
                {
                    return Json(new { Type = 0, Content = "您要订购的商品库存量不足。" }, JsonRequestBehavior.AllowGet);
                }
                if (entity.SalesTerritory != base.DeliveryRegion && entity.SalesTerritory != 3)
                {
                    return Json(new { Type = 0, Content = "您要订购的商品无法在当前区域配送。" }, JsonRequestBehavior.AllowGet);
                }

                var promotionsku = itemBll.GetPromotionInfoBySku(sku);
                var price = entity.DiscountPrice;
                if (entity.DiscountPrice == 0 || (promotionsku.Count > 0 && promotionsku.FirstOrDefault().PromotionType == 2))
                {
                    price = entity.MinPrice;
                }

                //--------------------------------------------时令美食相关购买条件验证 开始---------------------------------------------------------------
                // 时令美食购买条件验证
                Dictionary<int, string> ResultMap = productBll.CheckSkuHolidaySpuValidate(
                    sku, qty, entity.Spu, base.language, base.ExchangeRate, false, price);

                Dictionary<int, string>.KeyCollection types = ResultMap.Keys;

                int HolidaySpuType = -1;
                string HolidaySpuContent = string.Empty;

                foreach (int type in types)
                {
                    HolidaySpuType = type;
                    HolidaySpuContent = ResultMap[type].ToString();
                }

                // 不符合购买条件
                if (HolidaySpuType == 0)
                {
                    return Json(new { Type = 0, Content = HolidaySpuContent }, JsonRequestBehavior.AllowGet);
                }
                //--------------------------------------------时令美食相关购买条件验证 结束---------------------------------------------------------------

                int realTaxType = Utility.Uitl.TotalTaxHelper.GetRealTaxType(entity.ReportStatus, entity.IsCrossBorderEBTax, price * base.ExchangeRate);
                decimal taxPrice = Utility.Uitl.TotalTaxHelper.GetTotalTaxAmount(realTaxType, price * base.ExchangeRate, entity.CBEBTaxRate / 100, entity.ConsumerTaxRate / 100, entity.VATTaxRate / 100, entity.PPATaxRate / 100);
                if (qty > 1 && realTaxType == 2 && (qty * price * base.ExchangeRate > base.OrderLimitValue))
                {
                    return Json(new { Type = 0, Content = "单笔订单金额不能超过" + base.OrderLimitValue + "元。" }, JsonRequestBehavior.AllowGet);
                }
                if (realTaxType == 1 && (qty * price * base.ExchangeRate > base.ConsolidatedPrice))
                {
                    return Json(new { Type = 0, Content = "单笔订单金额不能超过" + Math.Round(base.ConsolidatedPrice, 2, MidpointRounding.AwayFromZero) + "元。" }, JsonRequestBehavior.AllowGet);
                }
                if (entity.IsDutyOnSeller == 1)
                {
                    taxPrice = 0;
                }
                else
                {
                    if (realTaxType == 2 && taxPrice * qty < 50)
                    {
                        taxPrice = 0;
                    }
                }
                var taxTemp = Math.Round(taxPrice, 2, MidpointRounding.AwayFromZero);

                //-------------------------------------------------------------------优惠券---------------------------------------------------------------
                //2016.06.3添加：可用优惠券， 默认是1：原价商品 0x01
                int quanType = Convert.ToInt32(PromotionType.None);
                if (promotionsku.Count > 0)
                {
                    if (promotionsku.FirstOrDefault().PromotionType == 1)
                    {
                        quanType = Convert.ToInt32(PromotionType.Promotion);//1.打折 2.拼团   对应的枚举-->  打折：0x02 拼团0x04
                    }
                }
                //获取选中的优惠券
                decimal totleAmount = price * qty * base.ExchangeRate;
                var CanUseGiftCardList = giftCardBll.GetCanUseGiftCardList(base.LoginUser.UserID, totleAmount, quanType);
                var HuoliEntity = accountBll.GetHuoliEntityByUerId(this.LoginUser.UserID);

                decimal huoli = 0M;
                string huoLiMoney = "0";
                List<CanUseGiftCardViewModel> clist = new List<CanUseGiftCardViewModel>();
                if (CanUseGiftCardList != null)
                {
                    if (HuoliEntity != null)
                    {
                        //任性可用酒豆
                        huoli = BuyOrderManager.GetCanUseHuoli(totleAmount, HuoliEntity.HuoLiCurrent, 0M);
                        huoLiMoney = (huoli / 100).ToNumberRoundStringWithPoint();
                        foreach (var card in CanUseGiftCardList)
                        {
                            var vm = giftCardBll.EntityToViewModel(card);
                            vm.Huoli = BuyOrderManager.GetCanUseHuoli(totleAmount, HuoliEntity.HuoLiCurrent, card.CardSum);
                            vm.Money = (BuyOrderManager.GetCanUseHuoli(totleAmount, HuoliEntity.HuoLiCurrent, card.CardSum) / 100).ToNumberRoundStringWithPoint();
                            clist.Add(vm);
                        }
                    }
                    else
                    {
                        foreach (var card in CanUseGiftCardList)
                        {
                            var vm = giftCardBll.EntityToViewModel(card);
                            clist.Add(vm);
                        }
                    }
                }
                else
                {
                    if (HuoliEntity != null)
                    {
                        //任性可用酒豆
                        huoli = BuyOrderManager.GetCanUseHuoli(totleAmount, HuoliEntity.HuoLiCurrent, 0M);
                        huoLiMoney = (huoli / 100).ToNumberRoundStringWithPoint();
                    }
                }

                return Json(new { Type = 1, Content = (taxTemp * qty) == 0 ? "0.00" : (taxTemp * qty).ToString(), Huoli = huoli, Money = huoLiMoney, disList = clist }, JsonRequestBehavior.AllowGet);
                //return Json(new { Type = 1, Content = (taxTemp * qty) == 0 ? "0.00" : (taxTemp * qty).ToString(), Huoli = huoli, Monney = (huoli / 100).ToNumberRoundStringWithPoint() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Type = 0, Content = "系统错误，请稍后再试。" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        /// <summary>
        /// 商品进口商品税页面
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public ActionResult ImportTariff(string sku, int qty, int pid, string teamcode)
        {
            ProductSkuEntity entity = new ProductSkuEntity();
            ViewBag.BuyQty = qty;
            if (string.IsNullOrWhiteSpace(sku))
            {
                return Redirect("/home/error");
            }
            if (qty < 1)
            {
                return Redirect("/home/error");
            }
            try
            {
                entity = productBll.GetProductBySku(sku, base.language);
                if (entity == null)
                {
                    return Redirect("/home/error");
                }
                if (entity.Qty < qty)
                {
                    return Redirect("/home/error");
                }
                var promotionsku = itemBll.GetPromotionInfoBySku(sku);
                var price = entity.DiscountPrice;
                if (entity.DiscountPrice == 0 || (pid == 0 && promotionsku.Count > 0 && promotionsku.FirstOrDefault().PromotionType == 2 && string.IsNullOrEmpty(teamcode)))
                {
                    price = entity.ProductPrice;
                }
                entity.ProductPrice = price * base.ExchangeRate;
                entity.MinPrice = entity.MinPrice * base.ExchangeRate;
                entity.Price = entity.Price * base.ExchangeRate;

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Redirect("/home/error");
            }
            return View(entity);
        }

        /*
        public JsonResult CheckSkuHolidaySpuValidate(string sku, int qty)
        {
            Dictionary<int, string> ResultMap = productBll.CheckSkuHolidaySpuValidate(sku,qty,base.language);

            Dictionary<int, string>.KeyCollection types = ResultMap.Keys;

            int Type = -1;
            string Content = string.Empty;

            foreach(int type in types){
                Type = type;
                Content = ResultMap[type].ToString();
            }

            return Json(new { Type = Type, Content = Content }, JsonRequestBehavior.AllowGet);
        }
         * */
    }
}
