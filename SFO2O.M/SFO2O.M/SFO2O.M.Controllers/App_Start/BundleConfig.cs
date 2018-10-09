using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web.Optimization;

namespace SFO2O.M.Controllers
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725

        /// <summary>
        /// ~/bundles/jquery
        /// </summary>
        public const string BundleJquery = "jquery";

        /// <summary>
        /// ~/bundles/jqueryui
        /// </summary>
        public const string Bundlejqueryui = "jqueryui";
        /// <summary>
        /// ~/bundles/jqueryval
        /// </summary>
        public const string BundleJqueryval = "jqueryval";
        /// <summary>
        /// ~/bundles/modernizr
        /// </summary>
        public const string BundleModernizr = "modernizr";

        /// <summary>
        /// common.js
        /// </summary>
        public const string BundleJsCommon = "Common";

        /// <summary>
        /// "~/Scripts/jquery/iscroll.js","~/Scripts/jquery/swipe.js", "~/Scripts/index.js"
        /// </summary>
        public const string BundleJsIndex = "Index";

        public const string BundleJsItem = "Item";
        public const string BundleJsProductPicture = "ProductPicture";
        /// <summary>
        /// 购物车脚本
        /// </summary>
        public const string BundleJsShopping = "Shopping";

        public const string BundleJsMyOrders = "MyOrders";


        public const string BundleJsMyOrderDetail = "MyOrderDetail";
        /// <summary>
        /// ~/Scripts/login.js
        /// </summary>
        public const string BundleJsLogin = "Login";

        /// <summary>
        /// 分类
        /// </summary>
        public const string BundleJsCategory = "CagetoryList";

        /// <summary>
        /// 分类New
        /// </summary>
        public const string BundleJsCategoryNew = "CagetoryListNew";

        /// <summary>
        /// 分类
        /// </summary>
        public const string BundleJsStore = "ShopIntro";
        /// <summary>
        /// ~/Scripts/account/register.js
        /// </summary>
        public const string BundleJsRegister = "Register";
        /// <summary>
        /// /Scripts/productList.js
        /// </summary>
        public const string BundleJsProductList = "ProductList";

        public const string BundleJsAddressList = "AddressList";

        public const string BundleJsConfirmOrder = "ConfirmOrder";

        public const string BundleJsAuthorize = "Authorize";

        public const string BundleJsImportTariff = "ImportTariff";

        public const string BundleJsComplain = "Complain";//申诉

        public const string BundleJsPayType = "PayType";

        public const string BundleJsRefund = "RefundList";//退款单列表

        public const string BundleJsRefundDetail = "RefundDetail";//退款详情

        public const string BundleJsBrandIntro = "BrandIntro";//品牌介绍

        /// <summary>
        /// ~/Content/css
        /// </summary>
        public const string BundleCss = "css";

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            #region Js绑定

            bundles.Add(GetScriptBundle(BundleJquery, new[] { "~/Scripts/jquery/jquery-{version}.js" }));

            bundles.Add(GetScriptBundle(BundleJqueryval, new[] { "~/Scripts/jquery/jquery.unobtrusive*", "~/Scripts/jquery/jquery.validate*" }));

            bundles.Add(GetScriptBundle(BundleModernizr, new[] { "~/Scripts/jquery/modernizr" }));

            bundles.Add(GetScriptBundle(BundleJsCommon, new[] { "~/Scripts/common/common.js", "~/Scripts/jquery/jquery.dialog.js" }));

            //分类
            bundles.Add(GetScriptBundle(BundleJsCategory, new[] {"~/Scripts/cagetoryList.js" }));

            //分类New
            bundles.Add(GetScriptBundle(BundleJsCategoryNew, new[] { "~/Scripts/cagetoryList_n.js" }));

            //店铺
            bundles.Add(GetScriptBundle(BundleJsStore, new[] { "~/Scripts/shop.js" }));

            //首页
            bundles.Add(GetScriptBundle(BundleJsIndex, new[] {"~/Scripts/jquery/TouchSlide.1.1.js", "~/Scripts/jquery/jquery.lazyload.js", "~/Scripts/jquery/jquery.showSearch.js", "~/Scripts/index.js" }));

            //登录页
            bundles.Add(GetScriptBundle(BundleJsLogin, new[] { "~/Scripts/login.js" }));

            //注册&找回密码
            bundles.Add(GetScriptBundle(BundleJsRegister, new[] { "~/Scripts/account/register.js" }));

            //商品列表
            bundles.Add(GetScriptBundle(BundleJsProductList, new[] { "~/Scripts/productList2.js" }));

            //单品页
            //bundles.Add(GetScriptBundle(BundleJsItem, new[] { "~/Scripts/jquery/TouchSlide.1.1.js", "~/Scripts/jquery/jsrender-v1.0.0-beta.js", "~/Scripts/selectSKU.js", "~/Scripts/item.js" })); 
            bundles.Add(GetScriptBundle(BundleJsItem, new[] { "~/Scripts/selectSKU.js", "~/Scripts/item.js" }));

            bundles.Add(GetScriptBundle(BundleJsProductPicture, new[] { "~/Scripts/jquery/TouchSlide.1.1.js", "~/Scripts/productPicture.js"}));   

            //bundles.Add(GetScriptBundle(BundleJsShopping, new[] {  "~/Scripts/selectSKU.js", "~/Scripts/shoppingCart.js" }));

            bundles.Add(GetScriptBundle(BundleJsMyOrders, new[] { "~/Scripts/myOrders.js", "~/Scripts/order.js" }));

            bundles.Add(GetScriptBundle(BundleJsMyOrderDetail, new[] { "~/Scripts/orderDetail.js", "~/Scripts/order.js" }));
            
            
            //收货地址列表
            bundles.Add(GetScriptBundle(BundleJsAddressList, new[] { "~/Scripts/address.js" }));

            bundles.Add(GetScriptBundle(BundleJsConfirmOrder, new[] { "~/Scripts/confirmOrder.js" }));
            bundles.Add(GetScriptBundle(BundleJsImportTariff, new[] { "~/Scripts/importTariff.js" }));

            bundles.Add(GetScriptBundle(BundleJsComplain, new[] { "~/Scripts//jquery/ajaxupload.3.5.js", "~/Scripts/appeal.js" }));

            bundles.Add(GetScriptBundle(BundleJsPayType, new[] { "~/Scripts/payType.js" }));

            bundles.Add(GetScriptBundle(BundleJsRefund, new[] { "~/Scripts/refundList.js" }));

            bundles.Add(GetScriptBundle(BundleJsRefundDetail, new[] { "~/Scripts/refundDetail.js" }));

            bundles.Add(GetScriptBundle(BundleJsAuthorize, new[] { "~/Scripts/authorization.js" }));

            //品牌介绍
            bundles.Add(GetScriptBundle(BundleJsBrandIntro, new[] { "~/Scripts/brandIntro.js" }));

            #endregion

            #region Css样式

            bundles.Add(GetStyleBundle(BundleCss, new[] { "~/Content" }));

            #endregion

#if !DEBUG
            
               BundleTable.EnableOptimizations = true;
            
#endif
        }
        public static string JsDomain
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["JSDomain"]; }
        }
        public static string CssDomain
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["CSSDomain"]; }
        }
        private static string JsVersion
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["JSVersion"]; }
        }
        public static string CssVersion
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["CSSVersion"]; }
        }
        public static string ImgVersion
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["ImgVersion"]; }
        }

        private static Bundle GetScriptBundle(string name, string[] js)
        {
            var temp1 = new ScriptBundle(string.Format("~/{0}_{1}.js", name, JsVersion)).Include(
                               js);

            temp1.CdnPath = string.Format("http://{0}/{1}_{2}.js", JsDomain, name, JsVersion);

            return temp1;
        }
        public static Bundle GetStyleBundle(string name, string[] css)
        {
            var temp1 = new StyleBundle(string.Format("~/{0}_{1}.css", name, CssVersion)).Include(
                               css);

            temp1.CdnPath = string.Format("http://{0}/{1}_{2}.css", CssDomain, name, CssVersion);

            return temp1;
        }



        public static string BuildSrc(string name)
        {
            return string.Format("~/{0}_{1}.js", name, JsVersion);
        }


        public static string BuildCss(string name)
        {
            return string.Format("/{0}?{1}", name, CssVersion);
        }

    }
}