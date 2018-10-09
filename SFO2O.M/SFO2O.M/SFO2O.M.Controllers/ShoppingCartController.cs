using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.Util;
using SFO2O.BLL.Exceptions;
using SFO2O.BLL.Item;
using SFO2O.BLL.Product;
using SFO2O.BLL.Shopping;
using SFO2O.M.Controllers.BaseControllers;
using SFO2O.M.Controllers.Binder;
using SFO2O.M.Controllers.Common;
using SFO2O.M.Controllers.Extensions;
using SFO2O.M.Controllers.Filters;
using SFO2O.M.ViewModel.Product;
using SFO2O.M.ViewModel.ShoppingCart;
using SFO2O.Model.Extensions;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Product;
using SFO2O.Model.Promotion;
using SFO2O.Model.Shopping;
using SFO2O.Utility.Extensions;


namespace SFO2O.M.Controllers
{
    /// <summary>
    /// 购物车页
    /// </summary>
    [NoCache]
    public class ShoppingCartController : ShoppingBaseController
    {


        [Login]
        public ActionResult Index()
        {
            if (this.LoginUser == null || this.LoginUser.Status != 1)
            {
                return this.HandleError(MessageType.RequireAuthorize, "");
            }
            try
            {
                var itemModel = base.GetCart();
                if (itemModel != null && itemModel.Items.Any())
                {
                    if (itemModel.Items.Where(d => d.IsChecked).Count() > 0)
                    {
                        var firstShopRealTaxType = itemModel.Items.Where(d => d.IsChecked).FirstOrDefault().RealTaxType;
                        if (firstShopRealTaxType == 1)
                        {
                            foreach (var productTwo in itemModel.HKSecWareHouseItems)
                            {
                                CartBll.updateShoppingCartByRealtype(base.LoginUser.UserID, productTwo.Sku, base.DeliveryRegion, false);
                            }
                            itemModel.HKSecWareHouseItems.ToList().FindAll(d => d.IsChecked).ForEach(c => c.IsChecked = false);

                        }
                        if (firstShopRealTaxType == 2)
                        {
                            foreach (var productOne in itemModel.HKOneWareHouseItems)
                            {
                                productOne.IsChecked = false;
                                CartBll.updateShoppingCartByRealtype(base.LoginUser.UserID, productOne.Sku, base.DeliveryRegion, false);
                            }
                            itemModel.HKOneWareHouseItems.ToList().FindAll(d => d.IsChecked).ForEach(c => c.IsChecked = false);
                        }
                    }
                }
                return this.View(itemModel);
            }
            catch (SFO2OException)
            {
                return this.View(new CartViewModel(base.ExchangeRate));
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }

        }
        /// <summary>
        /// 列表子视图信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Items(string sku)
        {
            if (this.LoginUser == null || this.LoginUser.Status != 1)
            {
                return this.HandleError(MessageType.RequireAuthorize, "");
            }
            try
            {
                var cardItem = this.GetCart();
                if (cardItem.HKOneWareHouseItems.Where(d => d.Sku == sku && d.RealTaxType == 1).Count() > 0)
                {
                    return this.PartialView("_Items", this.GetCart().HKOneWareHouseItems);
                }
                return this.PartialView("_Items", this.GetCart().HKSecWareHouseItems);
            }
            catch (SFO2OException)
            {
                return this.PartialView("_Items", new List<ProductItem>());
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }
        }


        /// <summary>
        /// 去结算异步检查
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckOut()
        {
            if (this.LoginUser == null || this.LoginUser.Status != 1)
            {
                return this.HandleError(MessageType.RequireAuthorize, "");
            }
            try
            {
                //var items = CartBll.GetCartItemEntities(base.LoginUser.UserID, base.language, base.DeliveryRegion).AsProdcutItems(base.ExchangeRate);

                CartViewModel model = this.GetCart();//获取购物车数据

                var validitems = model.Items.Where(n => n.IsOnSaled == true && n.IsChecked).ToArray();

                var validOneWareHouseItems = model.HKOneWareHouseItems.Where(n => n.IsOnSaled == true && n.IsChecked).ToArray();
                var validSecWareHouseItems = model.HKSecWareHouseItems.Where(n => n.IsOnSaled == true && n.IsChecked).ToArray();
                if (validOneWareHouseItems.Count() == 0 && validSecWareHouseItems.Count() > 0) 
                {
                    if (model.TotalPrice >= base.OrderLimitValue)
                    {
                        if (validitems.Count() == 1)
                        {
                            if (validitems.Sum(n => n.CartQuantity) == 1)
                            {
                                return this.HandleSuccess("ok");
                            }
                        }
                        //return this.HandleError(string.Format("单个订单金额不可超过{0}元人民币", base.OrderLimitValue));
                        return this.HandleError(MessageType.Normal, "", "", base.OrderLimitValue);
                    }
                    return this.HandleSuccess("ok");
                }
                if (validOneWareHouseItems.Count() > 0 && validSecWareHouseItems.Count() > 0)
                {
                    return this.HandleError(MessageType.Error, "", "", base.OrderLimitValue);
                }
                if (validOneWareHouseItems.Count() > 0 && validSecWareHouseItems.Count() == 0)
                {
                    if (model.TotalPrice <= base.ConsolidatedPrice)
                    {
                        return this.HandleSuccess("ok");
                    }
                }
                return this.HandleError(MessageType.ErrorAddtion, "", "", base.ConsolidatedPrice);
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }
        }
        /// <summary>
        /// 清除提示（生命周期：浏览器会话）
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult ClearTip()
        {
            try
            {
                CookieManager.ClearShoppingTaxTip();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
            return this.HandleSuccess("ok");
        }

        public ActionResult MiniCart()
        {
            if (LoginUser == null || GetShoppingCartItemsCount()==0)
            {
                return Content("0");
            }
            else
            {
                return Content(GetShoppingCartItemsCount().ToString());
            }
        }
        public ActionResult MiniCartOne()
        {
            if (LoginUser == null || GetShoppingCartItemsCount() == 0)
            {
                return Content("");
            }
            else
            {
                return Content("("+GetShoppingCartItemsCount().ToString()+")");
            }
        }

        //[Login]
        [HttpGet]
        public ActionResult ModifyProduct(string productCode, string sku)
        {
            try
            {

                if (string.IsNullOrEmpty(productCode))
                {
                    return this.HandleError("无法找到该商品");
                }
                SkuAttributeViewModel model = base.GetProductItem(productCode, sku);

                if (model == null)
                {
                    return this.HandleError("无法找到该商品");
                }

                return this.HandleSuccess("ok", model);

            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
        /// <summary>
        /// 修改SKU
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="newSku"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        [Login]
        [HttpPost]
        public ActionResult ModifyProduct(string sku, string newSku, int qty)
        {
            try
            {

                ViewBag.sku = newSku;
                if (string.IsNullOrEmpty(sku) || string.IsNullOrEmpty(newSku))
                {
                    return this.HandleError("无法找到该商品");
                }
                if (sku == newSku)
                {
                    return this.HandleError("与当前商品相同");
                }
                var isOk = CartBll.UpdateItemByUserId(newSku, base.LoginUser.UserID, sku, qty, base.language, base.ExchangeRate, base.DeliveryRegion);

                if (isOk)
                {
                    return this.Items(newSku);
                }
                return this.HandleError("商品更新失败");

            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        #region ADD Update Delete

        /// <summary>
        /// 添加普通或积分商品。
        /// </summary>
        /// <param name="sku">商品编码</param>
        /// <param name="qty">订购数量。</param>  
        /// <returns>
        /// 返回购物车我的订单视图。
        /// </returns>
        /// <remarks>
        /// url：/shoppingcart/additem?sku=12345678&qty=1
        /// </remarks>
        [Login]
        public ActionResult AddItem(string sku, int qty)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                return this.HandleError("参数错误：缺少商品号。");
            }

            if (qty < 1)
            {
                return this.HandleError("最少购买一件选定的商品才能将其加入购物车。");
            }

            try
            {
                bool isOk = CartBll.AddItemByUserId(base.LoginUser.UserID, sku, qty, language, base.ExchangeRate, base.DeliveryRegion);//TODO：待增加销售区域
                if (isOk)
                {

                    return this.HandleSuccess("添加成功！", this.GetShoppingCartItemsCount());
                }
                else
                {
                    return this.HandleError("添加失败");
                }
            }
            catch (Exception exception)
            {
                return this.HandleError(exception);
            }
        }
        /// <summary>
        /// 勾选购物车
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        //public ActionResult SelectedItem(string sku, string selected = "1")
        public ActionResult SelectedItems([ModelBinder(typeof(JsonBinder<SelectedItem[]>))]SelectedItem[] skus)
        {
            if (skus == null || !skus.Any())
            {
                return this.HandleError("参数错误：缺少商品号。");
            }

            try
            {
                bool isOk = false;
                isOk = CartBll.SelectedItem(base.LoginUser.UserID, skus, base.DeliveryRegion, base.GetCart());//TODO：待增加销售区域
                if (isOk)
                {

                    return this.HandleSuccess("添加成功！", this.GetShoppingCartItemsCount());
                }
                else
                {
                    return this.HandleError("添加失败");
                }
            }
            catch (Exception exception)
            {
                return this.HandleError(exception);
            }
        }

        /// <summary>
        /// 修改商品数量。
        /// </summary>
        /// <param name="sku">商品编码</param>
        /// <param name="qty">订购数量。</param> 
        /// <returns>
        /// 返回购物车我的订单视图。
        /// </returns>
        /// <remarks>
        /// url：/shoppingcart/updateqty?sku=12345678&qty=1
        /// </remarks> 
        [Login]
        public ActionResult UpdateQty(string sku, int qty)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                return this.HandleError("参数错误：缺少商品号。");
            }

            if (qty < 1)
            {
                return this.HandleError("最少购买一件选定的商品才能将其加入购物车。");
            }

            try
            {
                bool isOk = CartBll.UpdateQtyByUserId(base.LoginUser.UserID, sku, qty, language, base.ExchangeRate, base.DeliveryRegion);//TODO：待增加销售区域
                if (isOk)
                {
                    return this.HandleSuccess("更新成功！");
                }
                else
                {
                    return this.HandleError("更新失败");
                }
            }
            catch (SFO2OException ex)
            {
                if (ex.InnerException != null)
                {
                    return this.HandleError(ex.Message, data: ex.InnerException.Message);
                }
                else
                {
                    return this.HandleError(ex);
                }
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }
        }



        /// <summary>
        /// 删除普通商品。
        /// </summary>
        /// <param name="sku">商品编码，sku编码。</param> 
        /// <returns>
        /// 返回购物车我的订单视图。
        /// </returns>
        /// <remarks>
        /// url：/shoppingcart/deleteitem?skus=12345678
        /// </remarks> 
        [Login]
        [HttpPost]
        public ActionResult DeleteItems(string[] skus)
        {
            if (!skus.Any())
            {
                return this.HandleError("参数错误：缺少商品号。");
            }

            try
            {
                bool isOk = CartBll.DeleteItemByUserId(base.LoginUser.UserID, skus.ToArray(), base.DeliveryRegion);//TODO：待增加销售区域
                if (isOk)
                {
                    return this.HandleSuccess("删除成功！");
                }
                else
                {
                    return this.HandleError("更新失败");
                }
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }
        }
        #endregion

        #region Cart
        /// <summary>
        /// 清空购物车。
        /// </summary>
        /// <returns>
        /// 返回购物车我的订单视图。
        /// </returns>
        /// <remarks>
        /// url：/shoppingcart/clearall
        /// </remarks>
        //[Login]
        //public ActionResult ClearAll()
        //{
        //    try
        //    {
        //        bool isOk = CartBll.ClearAll(base.LoginUser.UserID, base.DeliveryRegion);
        //        if (isOk)
        //        {
        //            return this.HandleSuccess("删除成功！");
        //        }
        //        else
        //        {
        //            return this.HandleError("更新失败");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleError(ex);
        //    }
        //}

        ///// <summary>
        ///// 清除下架商品
        ///// </summary>
        ///// <returns></returns>
        ///// <remarks>
        ///// url：/shoppingcart/ClearInvalidProducts
        ///// </remarks>
        //[Login]
        //public ActionResult ClearInvalidProducts(List<string> skus)
        //{
        //    try
        //    {
        //        bool isOk = CartBll.DeleteItemByUserId(base.LoginUser.UserID, skus.ToArray(), base.DeliveryRegion);//TODO：待增加销售区域
        //        if (isOk)
        //        {
        //            return this.HandleSuccess("删除成功！");
        //        }
        //        else
        //        {
        //            return this.HandleError("更新失败");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleError(ex);
        //    }
        //}
        #endregion

        #region Private

        /// <summary>
        /// 获取购物车总数量
        /// </summary>
        /// <returns></returns>
        private int GetShoppingCartItemsCount()
        {
            try
            {
                return CartBll.GetMiniShoppingCart(base.LoginUser.UserID, base.language, base.DeliveryRegion);
            }
            catch (Exception ex)
            { 
                LogHelper.Error("GetShoppingCartItemsCount", ex);
                return 0;
            }



        }
        #endregion
    }


}
