using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Antlr.Runtime;
using SFO2O.BLL.Item;
using SFO2O.BLL.Product;
using SFO2O.BLL.Shopping;
using SFO2O.M.Controllers.Common;
using SFO2O.M.Controllers.Extensions;
using SFO2O.M.ViewModel.Product;
using SFO2O.M.ViewModel.ShoppingCart;
using SFO2O.Model.Extensions;
using SFO2O.Model.Product;
using SFO2O.Model.Promotion;
using SFO2O.Model.Shopping;
using SFO2O.BLL.Account;
using SFO2O.BLL.Order;
using SFO2O.Utility;
using SFO2O.BLL.GiftCard;
using SFO2O.Model.GiftCard;
using SFO2O.Model.Enum;
using SFO2O.Utility.Uitl;

namespace SFO2O.M.Controllers.BaseControllers
{
    public class ShoppingBaseController : SFO2OBaseController
    {
        internal ProductBll productBll = new ProductBll();
        internal ShoppingBll CartBll = new ShoppingBll();
        internal ItemBll itemBll = new ItemBll();
        private AccountBll accountBll = new AccountBll();
        internal GiftCardBll giftCardBll = new GiftCardBll();
        protected internal JsonResult HandleJson(MessageType type, string message, string linkUrl = null, object data = null)
        {
            Message model = new Message(type, "出错了", message, data) { LinkUrl = linkUrl };

            return Json(model, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 获取当前购物车对象
        /// </summary>
        /// <returns></returns>
        internal CartViewModel GetCart(bool? ischecked = null, decimal? exchangeRate = null, int? hasActivity = 0, int? giftCardId = 0)
        {
            var cartItemEntities = CartBll.GetCartItemEntities(base.LoginUser.UserID, base.language, base.DeliveryRegion);
            var cartGatewayItemEntities = CartBll.GetCartGatewayItemEntities(base.LoginUser.UserID);
            CartViewModel model = new CartViewModel(base.ExchangeRate);
            IList<PromotionEntity> promotions = null;
            if (cartItemEntities != null && cartItemEntities.Any())
            {
                promotions = itemBll.GetPromotionEntities(cartItemEntities.Select(n => n.Sku).ToArray()).Where(d => d.PromotionType != 2).ToList();
            }

            var items = cartItemEntities.AsProdcutItems(promotions, exchangeRate ?? base.ExchangeRate, cartGatewayItemEntities);

            //判断选中优惠券，走优惠券拆分逻辑  
            decimal cardValue = 0M;             
            //if (giftCardId != 0)
            //{
            //    int quanType = Convert.ToInt32(PromotionType.None); //判断是否优惠券是否可用， 默认是0x01：原价商品
            //    if (promotions.Count > 0)
            //    {
            //        //PromotionType == 1 ? 2 : 4 ;//1.打折 2.拼团   对应的枚举-->  打折：0x02 拼团0x04
            //        int promotionType = Convert.ToInt32(PromotionType.Promotion), groupType = Convert.ToInt32(PromotionType.GroupBuy);
            //        int i = 0;
            //        //全部不是 原价商品
            //        if (items.Count() == promotions.Count())
            //        {
            //            quanType = promotions[0].PromotionType== 1 ? promotionType : groupType;
            //            i = 1;
            //        }
            //        for (; i < promotions.Count; i++)
            //        {
            //            quanType |= promotions[i].PromotionType == 1 ? promotionType : promotionType;
            //        }
            //    }
            //    //获取选中的优惠券
            //    var productTotalAmount = items.Where(d => d.IsChecked == true).Sum(d => StringUtils.ToAmt(d.SalePrice * base.ExchangeRate) * d.CartQuantity);
            //    var CanUseGiftCardList = giftCardBll.GetCanUseGiftCardList(base.LoginUser.UserID, productTotalAmount, quanType);
            //    if (CanUseGiftCardList != null)
            //    {
            //        var GiftCardEntityChecked = CanUseGiftCardList.Where(x => x.ID == giftCardId).FirstOrDefault();
            //        if (GiftCardEntityChecked != null)
            //        {
            //            cardValue = (GiftCardEntityChecked as GiftCardEntity).CardSum;
            //            //拆分优惠券
            //            foreach (var li in items.Where(d => d.IsChecked == true))
            //            {
            //                if (items.Where(d => d.IsChecked == true).Last().Sku == li.Sku)
            //                {
            //                    li.GiftCard = cardValue - items.Where(d => d.IsChecked == true).Sum(d => d.GiftCard);
            //                }
            //                else
            //                {
            //                    li.GiftCard = Math.Round(cardValue * StringUtils.ToAmt(li.SalePrice * base.ExchangeRate) * li.CartQuantity / productTotalAmount, 2, MidpointRounding.AwayFromZero);
            //                }
            //            }
            //        }
            //        else //这种情况只有用户在前端篡改了优惠券ID之后才会出现
            //        {
            //            LogHelper.ErrorMsg("找不到这个优惠券ID对应的数据，一般是用户前端篡改了ID数据！");
            //        }
            //    }
            //}

            //判断选中酒豆，走酒豆拆分逻辑
            //if (hasActivity == 1)
            //{
            //    var HuoliEntity = accountBll.GetHuoliEntityByUerId(base.LoginUser.UserID);
            //    if (HuoliEntity != null && HuoliEntity.HuoLiCurrent >= 0)
            //    {
            //        var productTotalAmount = items.Where(d => d.IsChecked == true).Sum(d => StringUtils.ToAmt(d.SalePrice * base.ExchangeRate) * d.CartQuantity);
            //        var huolitotal = BuyOrderManager.GetCanUseHuoli(productTotalAmount, HuoliEntity.HuoLiCurrent, cardValue);
            //        //orderInfoEntity.Huoli = huolitotal;
            //        foreach (var li in items.Where(d => d.IsChecked == true))
            //        {
            //            if (items.Where(d => d.IsChecked == true).Last().Sku == li.Sku)
            //            {
            //                li.Huoli = huolitotal - items.Where(d => d.IsChecked == true).Sum(d => d.Huoli);
            //            }
            //            else
            //            {
            //                li.Huoli = Math.Round(huolitotal * StringUtils.ToAmt(li.SalePrice * base.ExchangeRate) * li.CartQuantity / productTotalAmount, 0, MidpointRounding.AwayFromZero);
            //            }
            //        }
            //    }
            //}
            var productItems = items as ProductItem[] ?? items.ToArray();
            if (ischecked == null)
            {   //为购物车使用
                model.HKOneWareHouseItems = productItems.Where(d => d.RealTaxType == 1).Where(n => n.IsOnSaled == true).ToArray();
                model.HKSecWareHouseItems = productItems.Where(d => d.RealTaxType == 2).Where(n => n.IsOnSaled == true).ToArray();
                model.Items = items;
            }
            else
            {
                model.HKOneWareHouseItems = productItems.Where(d => d.RealTaxType == 1).Where(n => n.IsOnSaled == true).ToArray();
                model.HKSecWareHouseItems = productItems.Where(d => d.RealTaxType == 2).Where(n => n.IsOnSaled == true).ToArray();
                //为结算使用
                model.Items = productItems.Where(n => n.IsOnSaled == true && n.IsChecked == ischecked).ToArray();
            }
            model.InvalidItems = productItems.Where(n => n.IsOnSaled == false).ToArray();
            model.HKOneWareHouseInvalidItems = items.Where(d => d.RealTaxType == 1).Where(n => n.IsOnSaled == false).ToArray();
            model.HKSecWareHouseInvalidItems = items.Where(d => d.RealTaxType == 2).Where(n => n.IsOnSaled == false).ToArray();
            return model;
        }
        /// <summary>
        /// 获取虚拟购物车物品
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="exchangeRate"></param>
        /// <returns></returns>
        public CartViewModel GetBuyVirtualCart(string sku, int ping, int proid, decimal? exchangeRate = null)
        {
            var entity = CartBll.GetVirtualCartItemEntity(sku, base.language, base.DeliveryRegion);
            IList<PromotionEntity> promotions = null;
            if (entity != null)
            {
                if (ping == 1)
                {
                    promotions = itemBll.GetPromotionEntitiesTeam(proid);
                }
                else
                {
                    promotions = itemBll.GetPromotionEntities(new[] { sku });
                }
            }

            var itemArray = new ShoppingCartItemEntity[] { entity };

            var items = itemArray.AsProdcutItems(promotions, base.ExchangeRate);


            CartViewModel model = new CartViewModel(base.ExchangeRate) { Items = items };

            return model;
        }

        /// <summary>
        /// 立即购买使用
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public CartViewModel GetBuyVirtualCart(ProductSkuEntity entity, int ping, int proid, decimal? exchangeRate = null)
        {

            IList<PromotionEntity> promotions = null;
            if (entity != null)
            {
                if (ping == 1)
                {
                    promotions = itemBll.GetPromotionEntitiesTeam(proid);
                }
                else
                {
                    promotions = itemBll.GetPromotionEntities(new[] { entity.Sku });
                }

            }

            var item = entity.AsProdcutItem(promotions, base.ExchangeRate);

            CartViewModel model = new CartViewModel(base.ExchangeRate) { Items = new ProductItem[] { item } };

            return model;
        }

        internal SkuAttributeViewModel GetProductItem(string spu, string sku)
        {
            IList<ProductImage> images = null;
            //获取单品页商品
            int userId = 0;
            if (base.LoginUser != null)
            {
                userId = base.LoginUser.UserID;
            }
            var itemskus = productBll.GetItemByProductCode(spu, base.language, ref images, userId);
            if (itemskus.Count == 0)
            {
                return null;
            }
            var promotions = itemBll.GetPromotionEntities(itemskus.Select(n => n.Sku).ToArray());

            var product = itemskus.ToArray().AsDto(base.ExchangeRate, promotions, false);

            product.Images = images.ToArray();

            return itemskus.AsSkuAttributeViewModel(base.ExchangeRate, promotions, sku, dto: product);
        }
    }
}
