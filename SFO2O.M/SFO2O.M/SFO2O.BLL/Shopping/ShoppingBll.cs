using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SFO2O.BLL.Exceptions;
using SFO2O.DAL.Product;
using SFO2O.DAL.Promotion;
using SFO2O.DAL.Shopping;
using SFO2O.M.ViewModel.Product;
using SFO2O.Model.Product;
using SFO2O.Model.Promotion;
using SFO2O.Model.Shopping;
using SFO2O.M.ViewModel.ShoppingCart;
using SFO2O.Utility.Uitl;

namespace SFO2O.BLL.Shopping
{
    public class ShoppingBll
    {

        ProductDal dal = new ProductDal();
        PromotionDal promotionDal = new PromotionDal();
        ShoppingCartDal cartDal = new ShoppingCartDal();
        /// <summary>
        /// 获取购物车数据列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="language"></param>
        /// <param name="currentSalesTerritory"></param>
        /// <returns></returns>
        public IList<ShoppingCartItemEntity> GetCartItemEntities(int userId, int language, int currentSalesTerritory)
        {
            return cartDal.GetShoppingCart(userId, currentSalesTerritory, language);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="language"></param>
        /// <param name="currentSalesTerritory"></param>
        /// <returns></returns>
        public IList<ShoppingCartGatewayEntity> GetCartGatewayItemEntities(int userId)
        {
            return cartDal.GetShoppingCartGateway(userId);
        }
        /// <summary>
        /// 根据SKU获取关口
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public IList<ShoppingCartGatewayEntity> GetGatewayBySku(string sku)
        {
            return cartDal.GetGatewayBySku(sku);
        }
        /// <summary>
        /// 通过sku组装虚拟购物车信息 for　立即购买
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="language"></param>
        /// <param name="currentSalesTerritory"></param>
        /// <returns></returns>
        public ShoppingCartItemEntity GetVirtualCartItemEntity(string sku, int language, int currentSalesTerritory)
        {
            return cartDal.GetVirtualCartBySku(sku, currentSalesTerritory, language);
        }

        /// <summary>
        /// 获取迷你购物车数量信息
        /// </summary>
        /// <returns></returns>
        public int GetMiniShoppingCart(int userId, int language, int currentSalesTerritory)
        {
            return cartDal.GetMiniShoppingCart(userId, currentSalesTerritory, language);
        }


        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sku"></param>
        /// <param name="qty"></param>
        /// <param name="language"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="currentSalesTerritory"></param>
        /// <returns></returns>
        public bool AddItemByUserId(int userId, string sku, int qty, int language, decimal exchangeRate, int currentSalesTerritory)
        {
            bool isOk = false;

            var entity = this.CheckProductParams(userId, sku, qty, language, currentSalesTerritory);
            //TODO：如果sku存在则更新数量
            var promotions = promotionDal.GetAvaliablePromotionEntities(new[] { sku }).Where(d => d.PromotionType != 2);
            PromotionEntity promotion = null;
            if (promotions != null && promotions.Any())
            {
                promotion = promotions.FirstOrDefault();
            }


            var skuQty = cartDal.IsExistSku(userId, sku, currentSalesTerritory);

            if ((qty + skuQty) > (entity.Qty))
            {
                throw new SFO2OException("您要订购的商品库存量不足");
            }

            bool isExist = skuQty > 0;

            if (isExist)
            {
                isOk = cartDal.UpdateQty(userId, sku, qty, currentSalesTerritory, promotion,true);
            }
            else
            {
                isOk = cartDal.AddSkuInfo(userId, qty, exchangeRate, entity, promotion);
            }
            return isOk;
        }

        /// <summary>
        /// 更新购物车项
        /// </summary>
        /// <param name="newSku"></param>
        /// <param name="userId"></param>
        /// <param name="sku"></param>
        /// <param name="qty"></param>
        /// <param name="language"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="currentSalesTerritory"></param>
        /// <returns></returns>
        public bool UpdateItemByUserId(string newSku, int userId, string sku, int qty, int language,
            decimal exchangeRate, int currentSalesTerritory)
        {

            var entity = this.CheckProductParams(userId, newSku, qty, language, currentSalesTerritory);
            var skuQty = cartDal.IsExistSku(userId, newSku, currentSalesTerritory);
            if ((qty + skuQty) > (entity.Qty))//不做最小spu库存检查 - entity.MinForOrder))
            {
                throw new SFO2OException("您要订购的商品库存量不足");
            }
            bool isExist = skuQty > 0;

            return cartDal.UpdateItem(newSku, userId, sku, qty, currentSalesTerritory, isExist, entity);

        }

        /// <summary>
        /// 勾选购物车项
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skus"></param>
        /// <param name="currentSalesTerritory"></param> 
        /// <returns></returns>
        public bool SelectedItem(int userId, IList<SelectedItem> skus, int currentSalesTerritory, CartViewModel cartViewModel)
        { 
            if (skus == null || !skus.Any())
            {
                throw new ArgumentNullException("skus");
            }
            var firstOrDefault = skus.FirstOrDefault();
            bool isSelected = firstOrDefault != null && firstOrDefault.Selected == 1;
            //判断 选中的sku 是属于哪号仓库的 
            bool one =false;
            foreach (var oneShopping in cartViewModel.HKOneWareHouseItems)
            {
                if (oneShopping.Sku.Equals(firstOrDefault.Sku))
                {
                    one = true;
                }
            }
            //如果 选中的sku 属于一号仓的  更新二号仓 ischecked全部为false
            if (one)
            {
                foreach (var twoShopping in cartViewModel.HKSecWareHouseItems)
                {
                    cartDal.SelectedItem(userId, twoShopping.Sku, currentSalesTerritory, false);
                } 
            }
            bool two =false;
            foreach (var twoShopping in cartViewModel.HKSecWareHouseItems)
            {
                if (twoShopping.Sku.Equals(firstOrDefault.Sku))
                {
                    two = true;
                }
            }
            if (two)
            {
                foreach (var oneShopping in cartViewModel.HKOneWareHouseItems)
                {
                    cartDal.SelectedItem(userId, oneShopping.Sku, currentSalesTerritory, false);
                }
            }
            //如果 选中的sku 属于二号仓的  更新一号仓 ischecked全部为false
            return cartDal.SelectedItem(userId, skus, currentSalesTerritory, isSelected);
        }
        /// <summary>
        /// 更新qty信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sku"></param>
        /// <param name="qty"></param>
        /// <param name="language"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="currentSalesTerritory"></param>
        /// <returns></returns>
        public bool UpdateQtyByUserId(int userId, string sku, int qty, int language, decimal exchangeRate, int currentSalesTerritory)
        {
            var entity = this.CheckProductParams(userId, sku, qty, language, currentSalesTerritory);
            var promotions = promotionDal.GetAvaliablePromotionEntities(new[] { sku });
            PromotionEntity promotion = null;
            if (promotions != null && promotions.Any())
            {
                promotion = promotions.FirstOrDefault();
            }
            return cartDal.UpdateQty(userId, sku, qty, currentSalesTerritory, promotion);
        }
        /// <summary>
        /// 检查产品信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sku"></param>
        /// <param name="qty"></param>
        /// <param name="language"></param>
        /// <param name="currentSalesTerritory"></param>
        /// <returns></returns>
        private ProductSkuEntity CheckProductParams(int userId, string sku, int qty, int language, int currentSalesTerritory)
        {
            if (userId == 0)
            {
                throw new SFO2OException("请登录!");
            }
            var entity = dal.GetProductBySku(sku, language);

            if (entity == null)
            {
                throw new SFO2OException("您要订购的商品不存在或者已下架。");
            }
            //if (entity.Qty - entity.MinForOrder <= 0)
            //{
            //    throw new SFO2OException("您要订购的商品库存量不足。");
            //}
            //if (qty > (entity.Qty - entity.MinForOrder))
            if (qty > entity.Qty)
            {
                //更新购物车中商品数量为剩余库存
                cartDal.UpdateQty(userId, sku, entity.Qty, currentSalesTerritory, null);
                throw new SFO2OException("您要订购的商品库存量不足。", new Exception(entity.Qty.ToString()));
            }
            if (entity.SalesTerritory != 3 && entity.SalesTerritory != currentSalesTerritory)
            {
                throw new SFO2OException("您要订购的商品无法在当前区域配送。");
            }
            if (entity.Price * qty > ConfigHelper.OrderLimitValue)
            {
                throw new SFO2OException("订单金额不能超过50000。");
            }
            return entity;
        }


        public bool DeleteItemByUserId(int userId, string[] sku, int currentSalesTerritory)
        {
            return cartDal.DeleteItem(userId, sku, currentSalesTerritory);
        }

        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currentSalesTerritory"></param>
        /// <returns></returns>
        public bool ClearAll(int userId, int currentSalesTerritory)
        {
            return cartDal.ClearAll(userId, currentSalesTerritory);
        }

        /// <summary>
        /// 获取购物车列表(选择提交订单的)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="salesTerritory"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public IList<ShoppingCartItemEntity> GetShoppingCartCheckOut(int userId, int salesTerritory, int language)
        {
            var entitys = cartDal.GetShoppingCart(userId, salesTerritory, language);
            if (entitys.Any())
            {
                return entitys.Where(n => n.IsChecked == 1 && n.Status == 3).ToList();
            }
            else
            {
                return entitys;
            }
        }
        //public IList<ShoppingCartOrderItemEntity> GetShoppingCartByChecked(int userId, int salesTerritory, int language)
        //{
        //    return cartDal.GetShoppingCartByChecked(userId, salesTerritory, language);
        //}
       public bool updateShoppingCartByRealtype(int userId, string sku, int currentSalesTerritory, bool selected)
       {
           return cartDal.SelectedItem(userId, sku, currentSalesTerritory, selected);
       }
    }
}
