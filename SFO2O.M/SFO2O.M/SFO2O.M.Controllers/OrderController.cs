using SFO2O.M.Controllers.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SFO2O.BLL.Shopping;
using SFO2O.BLL.Product;
using SFO2O.BLL.Exceptions;
using SFO2O.BLL.Item;
using SFO2O.Utility.Uitl;
using SFO2O.M.Controllers.Extensions;
using SFO2O.M.ViewModel.Order;
using SFO2O.Utility.Extensions;
using SFO2O.BLL.Order;
using SFO2O.M.Controllers.BaseControllers;
using SFO2O.M.Controllers.Common;
using SFO2O.M.ViewModel;
using SFO2O.M.ViewModel.ShoppingCart;
using SFO2O.BLL.Account;
using SFO2O.BLL.Team;
using SFO2O.Model.Enum;
using SFO2O.M.ViewModel.GiftCard;
using SFO2O.Model.Order;
using SFO2O.Model.Shopping;
using SFO2O.Model.Product;

namespace SFO2O.M.Controllers
{
    public class OrderController : ShoppingBaseController
    {
        private AddressBll addressBll = new AddressBll();
        private BuyOrderManager buyOrderManager = new BuyOrderManager();
        private AccountBll accountBll = new AccountBll();
        private TeamBll teamBll = new TeamBll();
        private readonly OrderManager orderManager = new OrderManager();
        private ProductBll productBll = new ProductBll();


        [Login]
        public ActionResult GiftSubmit(int giftType)
        {


            bool result = false;
            string msg = string.Empty;


            if (giftType != 1 && giftType != 2 && giftType != 3 && giftType != 4)
            {
                return Json(new { result = result, msg = "请求参数错误！" });
            }
            string orderCode = string.Empty;

            //1.获取默认收货地址
            int userId = this.LoginUser.UserID;
            List<AddressModel> list = new List<AddressModel>();

            list = addressBll.GetAddressList(userId, 1, this.language).ToList();

            if (list == null || list.Count <= 0)
            {
                return Json(new { result = result, msg = "您还未填写收货地址，请前往个人中心添加！" });

            }
            var address = list.Where(o => o.IsDefault == 1).FirstOrDefault();

            //2.构建需要传入的model的信息

            OrderProductInfoModel model = new OrderProductInfoModel();

            model.UserId = this.LoginUser.UserID;
            model.Language = this.language;
            model.DeliveryRegion = this.DeliveryRegion;
            model.OrderLimitValue = this.OrderLimitValue;
            model.ExchangeRate = this.ExchangeRate;
            model.AddressId = address.Id;

            //3.根据礼包类型创建商品集合

            List<ProductItem> productList = new List<ProductItem>();

            productList = GetProductListByType(giftType);

            //4.构建订单

            result = buyOrderManager.SaveGift(model, productList, ref orderCode, giftType, null, BuyOrderManager.GatewayCode(productList));


            if (result)
            {
                return Json(new{result=result,msg=orderCode});
            }


            return Json(new { result=result,msg="创建订单失败！"});

        }

        private List<ProductItem> GetProductListByType(int giftType)
        {
            List<ProductItem> list = new List<ProductItem>();

            List<ShoppingCartGatewayEntity> listShoppingCart = new List<ShoppingCartGatewayEntity>();
            ShoppingCartGatewayEntity shoppingCart = new ShoppingCartGatewayEntity();
            shoppingCart.Gateway = 1;

            listShoppingCart.Add(shoppingCart);


<<<<<<< HEAD
            #region 孙健_礼包


            switch (giftType)
            {
                case 1:
                    //660礼包   芙可清2瓶
                    ProductSkuEntity skuFirst1 = new ProductSkuEntity();

                    string skuOne = ConfigHelper.GetAppSetting<string>("libao_sku1");
                    int qitOne = Convert.ToInt32(ConfigHelper.GetAppSetting<string>("libao_sku1_qit"));

=======
            switch (giftType)
            {
                case 1:


                    //6250礼包   虫草6   封坛原浆 2
                    ProductSkuEntity skuFirst1 = new ProductSkuEntity();

                   string skuOne =  ConfigHelper.GetAppSetting<string>("libao_sku1");
                    int qitOne=Convert.ToInt32( ConfigHelper.GetAppSetting<string>("libao_sku1_qit"));
                 
>>>>>>> af8b14e4ac93f9b204a43034b1af3443a30a482a
                    skuFirst1 = productBll.GetProductBySku(skuOne, this.language);
                    ProductItem proFirst1 = new ProductItem(this.ExchangeRate);
                    proFirst1.CartUnitPrice = skuFirst1.ProductPrice;
                    proFirst1.CartQuantity = qitOne;
                    proFirst1.Huoli = 0;
                    proFirst1.Sku = skuFirst1.Sku;
                    proFirst1.Spu = skuFirst1.Spu;
                    proFirst1.GatewayCodes = listShoppingCart;
                    proFirst1.ForOrderQty = 100;
<<<<<<< HEAD
                    proFirst1.Price = 330;
                    list.Add(proFirst1);

                    //string skuTwo = ConfigHelper.GetAppSetting<string>("libao_sku2");
                    //int qitTwo = Convert.ToInt32(ConfigHelper.GetAppSetting<string>("libao_sku2_qit"));

                    //ProductSkuEntity skuFirst2 = new ProductSkuEntity();
                    //skuFirst2 = productBll.GetProductBySku(skuTwo, this.language);
                    //ProductItem proFirst2 = new ProductItem(this.ExchangeRate);
                    //proFirst2.CartUnitPrice = skuFirst2.ProductPrice;
                    //proFirst2.CartQuantity = qitTwo;
                    //proFirst2.Huoli = 0;
                    //proFirst2.Sku = skuFirst2.Sku;
                    //proFirst2.Spu = skuFirst2.Spu;
                    //proFirst2.GatewayCodes = listShoppingCart;
                    //proFirst2.ForOrderQty = 100;
                    //proFirst2.Price = 468;

                    //list.Add(proFirst2);
=======
                    proFirst1.Price = 898;
                    list.Add(proFirst1);

                    string skuTwo = ConfigHelper.GetAppSetting<string>("libao_sku2");
                    int qitTwo = Convert.ToInt32(ConfigHelper.GetAppSetting<string>("libao_sku2_qit"));

                    ProductSkuEntity skuFirst2 = new ProductSkuEntity();
                    skuFirst2 = productBll.GetProductBySku(skuTwo, this.language);
                    ProductItem proFirst2 = new ProductItem(this.ExchangeRate);
                    proFirst2.CartUnitPrice = skuFirst2.ProductPrice;
                    proFirst2.CartQuantity = qitTwo;
                    proFirst2.Huoli = 0;
                    proFirst2.Sku = skuFirst2.Sku;
                    proFirst2.Spu = skuFirst2.Spu;
                    proFirst2.GatewayCodes = listShoppingCart;
                    proFirst2.ForOrderQty = 100;
                    proFirst2.Price = 468;

                    list.Add(proFirst2);
>>>>>>> af8b14e4ac93f9b204a43034b1af3443a30a482a


                    break;
                case 2:
<<<<<<< HEAD
                  

                    //660大礼包  2盒 妙玉康
=======
                    //5000大礼包  4瓶虫草  2瓶国窖30年
>>>>>>> af8b14e4ac93f9b204a43034b1af3443a30a482a
                    ProductSkuEntity skuSecond1 = new ProductSkuEntity();
                    string skuThree = ConfigHelper.GetAppSetting<string>("libao_sku3");
                    int qitThree = Convert.ToInt32(ConfigHelper.GetAppSetting<string>("libao_sku3_qit"));
                    skuSecond1 = productBll.GetProductBySku(skuThree, this.language);

<<<<<<< HEAD

=======
                
>>>>>>> af8b14e4ac93f9b204a43034b1af3443a30a482a

                    ProductItem proSecond1 = new ProductItem(this.ExchangeRate);
                    proSecond1.CartUnitPrice = skuSecond1.ProductPrice;
                    proSecond1.CartQuantity = qitThree;
                    proSecond1.Huoli = 0;
                    proSecond1.Sku = skuSecond1.Sku;
                    proSecond1.Spu = skuSecond1.Spu;
                    proSecond1.GatewayCodes = listShoppingCart;
                    proSecond1.ForOrderQty = 100;

                    proSecond1.Price = 330;
                    list.Add(proSecond1);

<<<<<<< HEAD

                 



                    break;
                case 3:
                    //660大礼包  一瓶芙可清    一盒妙玉康
=======
>>>>>>> af8b14e4ac93f9b204a43034b1af3443a30a482a
                    string skuFour = ConfigHelper.GetAppSetting<string>("libao_sku4");
                    int qitFour = Convert.ToInt32(ConfigHelper.GetAppSetting<string>("libao_sku4_qit"));

                    ProductSkuEntity skuSecond2 = new ProductSkuEntity();
                    skuSecond2 = productBll.GetProductBySku(skuFour, this.language);
                    ProductItem proSecond2 = new ProductItem(this.ExchangeRate);
                    proSecond2.CartUnitPrice = skuSecond2.ProductPrice;
                    proSecond2.CartQuantity = qitFour;
                    proSecond2.Huoli = 0;
                    proSecond2.Sku = skuSecond2.Sku;
                    proSecond2.Spu = skuSecond2.Spu;
                    proSecond2.GatewayCodes = listShoppingCart;
                    proSecond2.ForOrderQty = 100;
                    proSecond2.Price = 330;

                    list.Add(proSecond2);


                    //妙玉康
                    string skuFive = ConfigHelper.GetAppSetting<string>("libao_sku5");
                    int qitFive = Convert.ToInt32(ConfigHelper.GetAppSetting<string>("libao_sku5_qit"));

                  
                    ProductSkuEntity skuThird1 = new ProductSkuEntity();
                    skuThird1 = productBll.GetProductBySku(skuFive, this.language);

<<<<<<< HEAD
=======
                    break;
                case 3:

                    string skuFive = ConfigHelper.GetAppSetting<string>("libao_sku5");
                    int qitFive = Convert.ToInt32(ConfigHelper.GetAppSetting<string>("libao_sku5_qit"));

                    //2000大礼包  1瓶虫草  1瓶封坛原浆 2瓶玛咖
                    ProductSkuEntity skuThird1 = new ProductSkuEntity();
                    skuThird1 = productBll.GetProductBySku(skuFive, this.language);
>>>>>>> af8b14e4ac93f9b204a43034b1af3443a30a482a
                    ProductItem proThird1 = new ProductItem(this.ExchangeRate);
                    proThird1.CartUnitPrice = skuThird1.ProductPrice;
                    proThird1.CartQuantity = qitFive;
                    proThird1.Huoli = 0;
                    proThird1.Sku = skuThird1.Sku;
                    proThird1.Spu = skuThird1.Spu;
                    proThird1.GatewayCodes = listShoppingCart;
                    proThird1.ForOrderQty = 100;
                    proThird1.Price = 330;

                    list.Add(proThird1);
<<<<<<< HEAD

                    //string skuSix = ConfigHelper.GetAppSetting<string>("libao_sku6");
                    //int qitSix = Convert.ToInt32(ConfigHelper.GetAppSetting<string>("libao_sku6_qit"));

                    //ProductSkuEntity skuThird2 = new ProductSkuEntity();
                    //skuThird2 = productBll.GetProductBySku(skuSix, this.language);
                    //ProductItem proThird2 = new ProductItem(this.ExchangeRate);
                    //proThird2.CartUnitPrice = skuThird2.ProductPrice;
                    //proThird2.CartQuantity = qitSix;
                    //proThird2.Huoli = 0;
                    //proThird2.Sku = skuThird2.Sku;
                    //proThird2.Spu = skuThird2.Spu;
                    //proThird2.GatewayCodes = listShoppingCart;
                    //proThird2.ForOrderQty = 100;

                    //proThird2.Price = 468;
                    //list.Add(proThird2);


                    //string skuSeven = ConfigHelper.GetAppSetting<string>("libao_sku7");
                    //int qitSeven = Convert.ToInt32(ConfigHelper.GetAppSetting<string>("libao_sku7_qit"));

                    //ProductSkuEntity skuThird3 = new ProductSkuEntity();
                    //skuThird3 = productBll.GetProductBySku(skuSeven, this.language);
                    //ProductItem proThird3 = new ProductItem(this.ExchangeRate);
                    //proThird3.CartUnitPrice = skuThird3.ProductPrice;
                    //proThird3.CartQuantity = qitSeven;
                    //proThird3.Huoli = 0;
                    //proThird3.Sku = skuThird3.Sku;
                    //proThird3.Spu = skuThird3.Spu;
                    //proThird3.GatewayCodes = listShoppingCart;
                    //proThird3.ForOrderQty = 100;

                    //proThird3.Price = 398;
                    //list.Add(proThird3);


                    break;
                case 4:
                    //330大礼包   1瓶芙可清
                    string skuEight = ConfigHelper.GetAppSetting<string>("libao_sku8");
                    int qitEight = Convert.ToInt32(ConfigHelper.GetAppSetting<string>("libao_sku8_qit"));
                   
=======

                    string skuSix = ConfigHelper.GetAppSetting<string>("libao_sku6");
                    int qitSix = Convert.ToInt32(ConfigHelper.GetAppSetting<string>("libao_sku6_qit"));

                    ProductSkuEntity skuThird2 = new ProductSkuEntity();
                    skuThird2 = productBll.GetProductBySku(skuSix, this.language);
                    ProductItem proThird2 = new ProductItem(this.ExchangeRate);
                    proThird2.CartUnitPrice = skuThird2.ProductPrice;
                    proThird2.CartQuantity = qitSix;
                    proThird2.Huoli = 0;
                    proThird2.Sku = skuThird2.Sku;
                    proThird2.Spu = skuThird2.Spu;
                    proThird2.GatewayCodes = listShoppingCart;
                    proThird2.ForOrderQty = 100;

                    proThird2.Price = 468;
                    list.Add(proThird2);


                    string skuSeven = ConfigHelper.GetAppSetting<string>("libao_sku7");
                    int qitSeven = Convert.ToInt32(ConfigHelper.GetAppSetting<string>("libao_sku7_qit"));

                    ProductSkuEntity skuThird3 = new ProductSkuEntity();
                    skuThird3 = productBll.GetProductBySku(skuSeven, this.language);
                    ProductItem proThird3 = new ProductItem(this.ExchangeRate);
                    proThird3.CartUnitPrice = skuThird3.ProductPrice;
                    proThird3.CartQuantity = qitSeven;
                    proThird3.Huoli = 0;
                    proThird3.Sku = skuThird3.Sku;
                    proThird3.Spu = skuThird3.Spu;
                    proThird3.GatewayCodes = listShoppingCart;
                    proThird3.ForOrderQty = 100;

                    proThird3.Price = 398;
                    list.Add(proThird3);

                   
                    break;
                case 4:

                    string skuEight = ConfigHelper.GetAppSetting<string>("libao_sku8");
                    int qitEight = Convert.ToInt32(ConfigHelper.GetAppSetting<string>("libao_sku8_qit"));
                    //350大礼包   1瓶玛咖
>>>>>>> af8b14e4ac93f9b204a43034b1af3443a30a482a
                    ProductSkuEntity skuFourth3 = new ProductSkuEntity();
                    skuFourth3 = productBll.GetProductBySku(skuEight, this.language);
                    ProductItem proFourth3 = new ProductItem(this.ExchangeRate);
                    proFourth3.CartUnitPrice = skuFourth3.ProductPrice;
                    proFourth3.CartQuantity = qitEight;
                    proFourth3.Huoli = 0;
                    proFourth3.Sku = skuFourth3.Sku;
                    proFourth3.Spu = skuFourth3.Spu;
                    proFourth3.GatewayCodes = listShoppingCart;
                    proFourth3.ForOrderQty = 100;
                    proFourth3.Price = 330;
                    list.Add(proFourth3);
                    break;
                case 5:
                    //330大礼包   1盒妙玉康
                    string skuEight5 = ConfigHelper.GetAppSetting<string>("libao_sku9");
                    int qitEight5 = Convert.ToInt32(ConfigHelper.GetAppSetting<string>("libao_sku9_qit"));
                   
                    ProductSkuEntity skuFourth5 = new ProductSkuEntity();
                    skuFourth5 = productBll.GetProductBySku(skuEight5, this.language);
                    ProductItem proFourth5 = new ProductItem(this.ExchangeRate);
                    proFourth5.CartUnitPrice = skuFourth5.ProductPrice;
                    proFourth5.CartQuantity = qitEight5;
                    proFourth5.Huoli = 0;
                    proFourth5.Sku = skuFourth5.Sku;
                    proFourth5.Spu = skuFourth5.Spu;
                    proFourth5.GatewayCodes = listShoppingCart;
                    proFourth5.ForOrderQty = 100;
                    proFourth5.Price = 330;
                    list.Add(proFourth5);
                    break;

                default:
                    break;
            }
            
            #endregion
            #region 酿造大礼包
            //switch (giftType)
            //{
            //    case 1:


            //        //6250礼包   虫草6   封坛原浆 2
            //        ProductSkuEntity skuFirst1 = new ProductSkuEntity();
            //        skuFirst1 = productBll.GetProductBySku("20170627618237", this.language);
            //        ProductItem proFirst1 = new ProductItem(this.ExchangeRate);
            //        proFirst1.CartUnitPrice = skuFirst1.ProductPrice;
            //        proFirst1.CartQuantity = 6;
            //        proFirst1.Huoli = 0;
            //        proFirst1.Sku = skuFirst1.Sku;
            //        proFirst1.Spu = skuFirst1.Spu;
            //        proFirst1.GatewayCodes = listShoppingCart;
            //        proFirst1.ForOrderQty = 100;
            //        proFirst1.Price = 898;
            //        list.Add(proFirst1);

            //        ProductSkuEntity skuFirst2 = new ProductSkuEntity();
            //        skuFirst2 = productBll.GetProductBySku("20170627618239", this.language);
            //        ProductItem proFirst2 = new ProductItem(this.ExchangeRate);
            //        proFirst2.CartUnitPrice = skuFirst2.ProductPrice;
            //        proFirst2.CartQuantity = 2;
            //        proFirst2.Huoli = 0;
            //        proFirst2.Sku = skuFirst2.Sku;
            //        proFirst2.Spu = skuFirst2.Spu;
            //        proFirst2.GatewayCodes = listShoppingCart;
            //        proFirst2.ForOrderQty = 100;
            //        proFirst2.Price = 468;

            //        list.Add(proFirst2);


            //        break;
            //    case 2:
            //        //5000大礼包  4瓶虫草  2瓶国窖30年
            //        ProductSkuEntity skuSecond1 = new ProductSkuEntity();
            //        skuSecond1 = productBll.GetProductBySku("20170627618236", this.language);
            //        ProductItem proSecond1 = new ProductItem(this.ExchangeRate);
            //        proSecond1.CartUnitPrice = skuSecond1.ProductPrice;
            //        proSecond1.CartQuantity = 4;
            //        proSecond1.Huoli = 0;
            //        proSecond1.Sku = skuSecond1.Sku;
            //        proSecond1.Spu = skuSecond1.Spu;
            //        proSecond1.GatewayCodes = listShoppingCart;
            //        proSecond1.ForOrderQty = 100;

            //        proSecond1.Price = 898;
            //        list.Add(proSecond1);

            //        ProductSkuEntity skuSecond2 = new ProductSkuEntity();
            //        skuSecond2 = productBll.GetProductBySku("20170627618241", this.language);
            //        ProductItem proSecond2 = new ProductItem(this.ExchangeRate);
            //        proSecond2.CartUnitPrice = skuSecond2.ProductPrice;
            //        proSecond2.CartQuantity = 2;
            //        proSecond2.Huoli = 0;
            //        proSecond2.Sku = skuSecond2.Sku;
            //        proSecond2.Spu = skuSecond2.Spu;
            //        proSecond2.GatewayCodes = listShoppingCart;
            //        proSecond2.ForOrderQty = 100;
            //        proSecond2.Price = 798;

            //        list.Add(proSecond2);



            //        break;
            //    case 3:
            //        //2000大礼包  1瓶虫草  1瓶封坛原浆 2瓶玛咖
            //        ProductSkuEntity skuThird1 = new ProductSkuEntity();
            //        skuThird1 = productBll.GetProductBySku("20170627618236", this.language);
            //        ProductItem proThird1 = new ProductItem(this.ExchangeRate);
            //        proThird1.CartUnitPrice = skuThird1.ProductPrice;
            //        proThird1.CartQuantity = 1;
            //        proThird1.Huoli = 0;
            //        proThird1.Sku = skuThird1.Sku;
            //        proThird1.Spu = skuThird1.Spu;
            //        proThird1.GatewayCodes = listShoppingCart;
            //        proThird1.ForOrderQty = 100;
            //        proThird1.Price = 898;

            //        list.Add(proThird1);

            //        ProductSkuEntity skuThird2 = new ProductSkuEntity();
            //        skuThird2 = productBll.GetProductBySku("20170627618239", this.language);
            //        ProductItem proThird2 = new ProductItem(this.ExchangeRate);
            //        proThird2.CartUnitPrice = skuThird2.ProductPrice;
            //        proThird2.CartQuantity = 1;
            //        proThird2.Huoli = 0;
            //        proThird2.Sku = skuThird2.Sku;
            //        proThird2.Spu = skuThird2.Spu;
            //        proThird2.GatewayCodes = listShoppingCart;
            //        proThird2.ForOrderQty = 100;

            //        proThird2.Price = 468;
            //        list.Add(proThird2);

            //        ProductSkuEntity skuThird3 = new ProductSkuEntity();
            //        skuThird3 = productBll.GetProductBySku("20170627618243", this.language);
            //        ProductItem proThird3 = new ProductItem(this.ExchangeRate);
            //        proThird3.CartUnitPrice = skuThird3.ProductPrice;
            //        proThird3.CartQuantity = 2;
            //        proThird3.Huoli = 0;
            //        proThird3.Sku = skuThird3.Sku;
            //        proThird3.Spu = skuThird3.Spu;
            //        proThird3.GatewayCodes = listShoppingCart;
            //        proThird3.ForOrderQty = 100;

            //        proThird3.Price = 398;
            //        list.Add(proThird3);



            //        break;
            //    case 4:
            //        //350大礼包   1瓶玛咖
            //        ProductSkuEntity skuFourth3 = new ProductSkuEntity();
            //        skuFourth3 = productBll.GetProductBySku("20170627618243", this.language);
            //        ProductItem proFourth3 = new ProductItem(this.ExchangeRate);
            //        proFourth3.CartUnitPrice = skuFourth3.ProductPrice;
            //        proFourth3.CartQuantity = 1;
            //        proFourth3.Huoli = 0;
            //        proFourth3.Sku = skuFourth3.Sku;
            //        proFourth3.Spu = skuFourth3.Spu;
            //        proFourth3.GatewayCodes = listShoppingCart;
            //        proFourth3.ForOrderQty = 100;
            //        proFourth3.Price = 398;
            //        list.Add(proFourth3);
            //        break;


            //    default:
            //        break;
            //} 
            #endregion

            #region 酿造大礼包
            //switch (giftType)
            //{
            //    case 1:


            //        //6250礼包   虫草6   封坛原浆 2
            //        ProductSkuEntity skuFirst1 = new ProductSkuEntity();
            //        skuFirst1 = productBll.GetProductBySku("20170627618237", this.language);
            //        ProductItem proFirst1 = new ProductItem(this.ExchangeRate);
            //        proFirst1.CartUnitPrice = skuFirst1.ProductPrice;
            //        proFirst1.CartQuantity = 6;
            //        proFirst1.Huoli = 0;
            //        proFirst1.Sku = skuFirst1.Sku;
            //        proFirst1.Spu = skuFirst1.Spu;
            //        proFirst1.GatewayCodes = listShoppingCart;
            //        proFirst1.ForOrderQty = 100;
            //        proFirst1.Price = 898;
            //        list.Add(proFirst1);

            //        ProductSkuEntity skuFirst2 = new ProductSkuEntity();
            //        skuFirst2 = productBll.GetProductBySku("20170627618239", this.language);
            //        ProductItem proFirst2 = new ProductItem(this.ExchangeRate);
            //        proFirst2.CartUnitPrice = skuFirst2.ProductPrice;
            //        proFirst2.CartQuantity = 2;
            //        proFirst2.Huoli = 0;
            //        proFirst2.Sku = skuFirst2.Sku;
            //        proFirst2.Spu = skuFirst2.Spu;
            //        proFirst2.GatewayCodes = listShoppingCart;
            //        proFirst2.ForOrderQty = 100;
            //        proFirst2.Price = 468;

            //        list.Add(proFirst2);


            //        break;
            //    case 2:
            //        //5000大礼包  4瓶虫草  2瓶国窖30年
            //        ProductSkuEntity skuSecond1 = new ProductSkuEntity();
            //        skuSecond1 = productBll.GetProductBySku("20170627618236", this.language);
            //        ProductItem proSecond1 = new ProductItem(this.ExchangeRate);
            //        proSecond1.CartUnitPrice = skuSecond1.ProductPrice;
            //        proSecond1.CartQuantity = 4;
            //        proSecond1.Huoli = 0;
            //        proSecond1.Sku = skuSecond1.Sku;
            //        proSecond1.Spu = skuSecond1.Spu;
            //        proSecond1.GatewayCodes = listShoppingCart;
            //        proSecond1.ForOrderQty = 100;

            //        proSecond1.Price = 898;
            //        list.Add(proSecond1);

            //        ProductSkuEntity skuSecond2 = new ProductSkuEntity();
            //        skuSecond2 = productBll.GetProductBySku("20170627618241", this.language);
            //        ProductItem proSecond2 = new ProductItem(this.ExchangeRate);
            //        proSecond2.CartUnitPrice = skuSecond2.ProductPrice;
            //        proSecond2.CartQuantity = 2;
            //        proSecond2.Huoli = 0;
            //        proSecond2.Sku = skuSecond2.Sku;
            //        proSecond2.Spu = skuSecond2.Spu;
            //        proSecond2.GatewayCodes = listShoppingCart;
            //        proSecond2.ForOrderQty = 100;
            //        proSecond2.Price = 798;

            //        list.Add(proSecond2);



            //        break;
            //    case 3:
            //        //2000大礼包  1瓶虫草  1瓶封坛原浆 2瓶玛咖
            //        ProductSkuEntity skuThird1 = new ProductSkuEntity();
            //        skuThird1 = productBll.GetProductBySku("20170627618236", this.language);
            //        ProductItem proThird1 = new ProductItem(this.ExchangeRate);
            //        proThird1.CartUnitPrice = skuThird1.ProductPrice;
            //        proThird1.CartQuantity = 1;
            //        proThird1.Huoli = 0;
            //        proThird1.Sku = skuThird1.Sku;
            //        proThird1.Spu = skuThird1.Spu;
            //        proThird1.GatewayCodes = listShoppingCart;
            //        proThird1.ForOrderQty = 100;
            //        proThird1.Price = 898;

            //        list.Add(proThird1);

            //        ProductSkuEntity skuThird2 = new ProductSkuEntity();
            //        skuThird2 = productBll.GetProductBySku("20170627618239", this.language);
            //        ProductItem proThird2 = new ProductItem(this.ExchangeRate);
            //        proThird2.CartUnitPrice = skuThird2.ProductPrice;
            //        proThird2.CartQuantity = 1;
            //        proThird2.Huoli = 0;
            //        proThird2.Sku = skuThird2.Sku;
            //        proThird2.Spu = skuThird2.Spu;
            //        proThird2.GatewayCodes = listShoppingCart;
            //        proThird2.ForOrderQty = 100;

            //        proThird2.Price = 468;
            //        list.Add(proThird2);

            //        ProductSkuEntity skuThird3 = new ProductSkuEntity();
            //        skuThird3 = productBll.GetProductBySku("20170627618243", this.language);
            //        ProductItem proThird3 = new ProductItem(this.ExchangeRate);
            //        proThird3.CartUnitPrice = skuThird3.ProductPrice;
            //        proThird3.CartQuantity = 2;
            //        proThird3.Huoli = 0;
            //        proThird3.Sku = skuThird3.Sku;
            //        proThird3.Spu = skuThird3.Spu;
            //        proThird3.GatewayCodes = listShoppingCart;
            //        proThird3.ForOrderQty = 100;

            //        proThird3.Price = 398;
            //        list.Add(proThird3);



            //        break;
            //    case 4:
            //        //350大礼包   1瓶玛咖
            //        ProductSkuEntity skuFourth3 = new ProductSkuEntity();
            //        skuFourth3 = productBll.GetProductBySku("20170627618243", this.language);
            //        ProductItem proFourth3 = new ProductItem(this.ExchangeRate);
            //        proFourth3.CartUnitPrice = skuFourth3.ProductPrice;
            //        proFourth3.CartQuantity = 1;
            //        proFourth3.Huoli = 0;
            //        proFourth3.Sku = skuFourth3.Sku;
            //        proFourth3.Spu = skuFourth3.Spu;
            //        proFourth3.GatewayCodes = listShoppingCart;
            //        proFourth3.ForOrderQty = 100;
            //        proFourth3.Price = 398;
            //        list.Add(proFourth3);
            //        break;


            //    default:
            //        break;
            //} 
            #endregion

            return list;

        }


        /// <summary>
        /// 立即购买提交订单
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        [Login]
        public ActionResult SubmitBuyNow(string sku, int qty)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                ViewBag.ErrorMsg = "参数错误：缺少商品号。";
            }
            if (qty < 1)
            {
                ViewBag.ErrorMsg = "最少购买一件选定的商品。";
            }
            try
            {
                int currentSaleTerritory = 1;//当前销售区域
                var entity = productBll.GetProductBySku(sku, language);
                if (entity == null)
                {
                    return this.HandleError("该商品不存在或已下架。");
                }
                if (entity.Qty - qty < entity.MinForOrder)
                {
                    ViewBag.ErrorMsg = "您要订购的商品库存量不足。";
                }
                if (entity.SalesTerritory != currentSaleTerritory)
                {
                    ViewBag.ErrorMsg = "您要订购的商品无法在当前区域配送。";
                }
                //todo 生成订单，跳转到支付页面，缺少地址
            }
            catch (Exception ex)
            {
                this.HandleError(ex);

            }
            return View();
        }

        /// <summary>
        /// 立即购买订单提交
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="sku"></param>
        /// <param name="quy"></param>
        /// <param name="addressId"></param>
        /// <returns></returns>
        [Login]
        public ActionResult BuySubmit(string jsonModel)
        {
            try
            {
                OrderSubmitModel entity = new OrderSubmitModel();
                if (!string.IsNullOrWhiteSpace(jsonModel))
                {
                    entity = JsonHelper.ToObject<OrderSubmitModel>(jsonModel);

                    OrderProductInfoModel model = new OrderProductInfoModel();
                    model.AddressId = entity.AddressId;
                    model.Sku = entity.Sku;
                    if (entity.pid > 0 || !string.IsNullOrEmpty(entity.TeamCode))
                    {
                        entity.TeamCode = entity.TeamCode.Trim();
                        entity.Quy = 1;
                    }
                    model.Quantity = entity.Quy;
                    model.strIp = "";
                    model.UserId = this.LoginUser.UserID;
                    model.Language = this.language;
                    model.DeliveryRegion = this.DeliveryRegion;
                    model.OrderLimitValue = this.OrderLimitValue;
                    model.hasActivity = entity.hasActivity;
                    model.GiftCardId = entity.GiftCardId;
                    model.pid = entity.pid;
                    model.TeamCode = entity.TeamCode;
                    model.ExchangeRate = this.ExchangeRate;
                    int ping = 0;
                    int proid = 0;
                    if (!string.IsNullOrEmpty(entity.TeamCode))
                    {
                        var teaminfo = teamBll.GetTeamInfoByTeamCode(entity.TeamCode);
                        ping = teaminfo.TeamStatus;
                        proid = teaminfo.PromotionId;
                    }
                    if (Session["StationSource"] != null && Session["ChannelId"] != null)
                    {
                        model.StationSource = Session["StationSource"].ToString();
                        model.StationSourceType = 2;
                        model.ChannelId = Convert.ToInt32(Session["ChannelId"]);
                    }
                    bool isOk = false;
                    var splitFlag = false;
                    string orderCode = string.Empty;
                    switch (entity.Type)
                    {
                        case 1://立即购买
                            //returnValue = buyOrderManager.ImmediatelySubmitOrder(model);
                            var virtualCart = base.GetBuyVirtualCart(model.Sku, ping, proid);
                            isOk = buyOrderManager.BuySave(model, virtualCart, ref orderCode);
                            break;
                        case 2://购物车下单
                            var cart = base.GetCart(ischecked: true, hasActivity: model.hasActivity, giftCardId: model.GiftCardId);
                            if (cart.HKOneWareHouseItems.Where(d => d.IsChecked == true).Count() > 0)
                            {
                                var firstOrder = new List<ProductItem>();
                                var secOrder = new List<ProductItem>();
                                var oldOrder = cart.HKOneWareHouseItems.Where(d => d.IsChecked == true);
                                foreach (var cartinfo in oldOrder)
                                {
                                    foreach (var gateway in cartinfo.GatewayCodes)
                                    {
                                        if (gateway.Gateway == 1)
                                        {
                                            firstOrder.Add(cartinfo);
                                            break;
                                        }
                                        if (gateway.Gateway == 2 && cartinfo.GatewayCodes.Count() == 1)
                                        {
                                            secOrder.Add(cartinfo);
                                            break;
                                        }
                                    }
                                }
                                if (secOrder.Count == 0)
                                {
                                    isOk = buyOrderManager.Save(model, firstOrder, ref orderCode, null, BuyOrderManager.GatewayCode(oldOrder));
                                }
                                else
                                {
                                    foreach (var first in firstOrder)
                                    {
                                        foreach (var ga in first.GatewayCodes)
                                        {
                                            if (ga.Gateway == 1 && first.GatewayCodes.Count() == 1)
                                            {
                                                splitFlag = true;
                                            }
                                        }
                                    }
                                    if (splitFlag)
                                    {
                                        string ParentOrderCode = DateTime.Now.ToString("yyMMddHHmmsss") + new Random().Next(100, 999);
                                        isOk = buyOrderManager.Save(model, firstOrder, ref orderCode, ParentOrderCode, BuyOrderManager.GatewayCode(oldOrder)) && buyOrderManager.Save(model, secOrder, ref orderCode, ParentOrderCode, BuyOrderManager.GatewayCode(oldOrder));
                                        orderCode = ParentOrderCode;
                                    }
                                    else
                                    {
                                        isOk = buyOrderManager.Save(model, firstOrder.Concat(secOrder), ref orderCode, null, BuyOrderManager.GatewayCode(oldOrder));
                                    }

                                }
                            }
                            else
                            {
                                var items = cart.HKSecWareHouseItems.Where(d => d.IsChecked == true).ToList();
                                isOk = buyOrderManager.Save(model, items, ref orderCode, null, BuyOrderManager.GatewayCode(items));
                            }

                            //添加订单确认页面业务逻辑
                            break;
                        case 3:
                            orderCode = entity.OrderCode;
                            isOk = buyOrderManager.OrderNewSubmitOrder(entity.OrderCode, this.LoginUser.UserID);
                            //添加订单确认页面业务逻辑
                            break;
                    }
                    if (isOk)
                    {
                        if (splitFlag)
                        {
                            return this.HandleJson(MessageType.Normal, "提交成功", data: orderCode);
                        }
                        return this.HandleJson(MessageType.Success, "提交成功", data: orderCode);

                    }
                    else
                    {
                        return this.HandleJson(MessageType.Error, "提交失败", data: orderCode);
                    }
                }
                else
                {
                    return this.HandleJson(MessageType.Error, "提交失败");
                }
            }
            catch (SFO2OException ex)
            {
                return this.HandleJson(MessageType.Error, ex.Message, null);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                return this.HandleJson(MessageType.Error, "提交订单失败。");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="aid"></param>
        /// <returns></returns>
        [Login]
        [FirstOrderAuthorize]
        public ActionResult Submit(string id, int? aid)
        {
            return View(SetOrderSubmitProductModel(id, aid));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Login]
        public ActionResult Tariff(string id = "")
        {
            return View(SetOrderSubmitProductModel(id, 0));
        }


        private OrderSubmitProductModel SetOrderSubmitProductModel(string id, int? aid)
        {
            OrderSubmitProductModel model = new OrderSubmitProductModel(base.ExchangeRate);
            model.OrderCode = id == null ? "" : id;
            model.AddressId = aid.HasValue == false ? 0 : aid.Value;
            if (string.IsNullOrWhiteSpace(id))
            {
                var cart = base.GetCart(ischecked: true);
                model.Items = cart.Items.Where(n => n.IsChecked);
                model.IsFixed = false;
                //-------------------------------------------------------------------优惠券和酒豆部分----------------------------------------------------------------------------------------
                //----优惠券----
                int quanType = Convert.ToInt32(PromotionType.None); //判断是否优惠券是否可用， 默认是4：原价商品
                if (model.Items.Count() > 0)
                {
                    var promotions = model.Items.Where(x => x.Promotion != null).ToList();
                    if (promotions.Count() > 0)
                    {
                        //PromotionType == 1 ? 2 : 4 ;//1.打折 2.拼团   对应的枚举-->  打折：0x02 拼团0x04
                        int promotionType = Convert.ToInt32(PromotionType.Promotion), groupType = Convert.ToInt32(PromotionType.GroupBuy);
                        int i = 0;
                        //全部不是 原价商品
                        if (model.Items.Count() == promotions.Count())
                        {
                            quanType = promotions[0].Promotion.PromotionType == 1 ? promotionType : groupType;
                            i = 1;
                        }
                        for (; i < promotions.Count; i++)
                        {
                            quanType |= promotions[i].Promotion.PromotionType == 1 ? promotionType : promotionType;
                        }
                    }
                }

                //获取默认优惠券
                var GiftCardNotUsedList = giftCardBll.GetCanUseGiftCardList(base.LoginUser.UserID, model.TotalProductPrice, quanType);
                //转换成 ViewModel集合，并进行酒豆操作
                List<CanUseGiftCardViewModel> vmlist = new List<CanUseGiftCardViewModel>();
                //-------------------------------------------------------------------酒豆----------------------------------------------------------------------
                ViewBag.IsShowHuoli = false;
                var HuoliEntity = accountBll.GetHuoliEntityByUerId(this.LoginUser.UserID);
                if (HuoliEntity == null || HuoliEntity.HuoLiCurrent <= 0)
                {
                    ViewBag.IsShowHuoli = true;
                }
                if (!ViewBag.IsShowHuoli)
                {
                    if (GiftCardNotUsedList != null)
                    {
                        ViewBag.HuoliNoUseGiftCard = BuyOrderManager.GetCanUseHuoli(model.TotalProductPrice, HuoliEntity.HuoLiCurrent, 0M);
                        ViewBag.HuoliMoneyNoUseGiftCard = (BuyOrderManager.GetCanUseHuoli(model.TotalProductPrice, HuoliEntity.HuoLiCurrent, 0M) / 100).ToNumberRoundStringWithPoint();
                        var GiftCardEntityDefault = GiftCardNotUsedList.FirstOrDefault();
                        decimal cardValue = 0M;
                        if (GiftCardEntityDefault != null)
                        {
                            cardValue = GiftCardEntityDefault.CardSum;
                        }
                        foreach (var card in GiftCardNotUsedList)
                        {
                            var vm = giftCardBll.EntityToViewModel(card);
                            vm.Huoli = BuyOrderManager.GetCanUseHuoli(model.TotalProductPrice, HuoliEntity.HuoLiCurrent, card.CardSum);
                            vm.Money = (BuyOrderManager.GetCanUseHuoli(model.TotalProductPrice, HuoliEntity.HuoLiCurrent, card.CardSum) / 100).ToNumberRoundStringWithPoint();
                            vmlist.Add(vm);
                        }
                        decimal huoli = BuyOrderManager.GetCanUseHuoli(model.TotalProductPrice, HuoliEntity.HuoLiCurrent, cardValue);
                        ViewBag.Huoli = huoli;
                    }
                    else
                    {
                        ViewBag.Huoli = BuyOrderManager.GetCanUseHuoli(model.TotalProductPrice, HuoliEntity.HuoLiCurrent, 0M);
                    }
                }
                else
                {
                    if (GiftCardNotUsedList != null)
                    {
                        foreach (var card in GiftCardNotUsedList)
                        {
                            var vm = giftCardBll.EntityToViewModel(card);
                            vmlist.Add(vm);
                        }
                    }
                }
                ViewBag.GiftCardList = vmlist;
            }
            else
            {
                //TODO:订单修改逻辑
                var orderModel = buyOrderManager.GetOrderInfoByCodeId(id, this.LoginUser.UserID);
                model.OrderStatus = orderModel.OrderStatus;
                var items = buyOrderManager.GetOrderProductsByCode(id, base.DeliveryRegion, base.language);
                var promotions = itemBll.GetPromotionEntities(items.Select(n => n.Sku).ToArray());
                if (string.IsNullOrEmpty(orderModel.TeamCode))
                {
                    promotions = promotions.Where(d => d.PromotionType != 2).ToList();
                }
                model.Items = items.AsProdcutItems(promotions, base.ExchangeRate);
                model.IsFixed = true;
                model.Receiver = orderModel.Receiver;
                model.Phone = orderModel.Phone;
                model.Address = addressBll.GetOrderAddressById(orderModel.ReceiptProvince, orderModel.ReceiptCity, orderModel.ReceiptRegion, language) + orderModel.ReceiptAddress;
            }
            return model;
        }
        public JsonResult getOrderInfo(string orderCode)
        {

            try
            {
                var orderModel = buyOrderManager.GetOrderInfoByCodeId(orderCode, this.LoginUser.UserID);
                return Json(new { Type = 1, Status = orderModel.OrderStatus }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOrderInfoByOrderCode(string OrderCode)
        {
            OrderInfoEntity orderInfoEntity = orderManager.GetOrderInfoByOrderCode(OrderCode);
            return Json(orderInfoEntity, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActiveIndex()
        {
            ViewBag.LoginUser = base.LoginUser;
            return View();
        }
    }
}
