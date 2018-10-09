using System.Web.Mvc;
using System.Web.Routing;
using LowercaseRoutesMVC;

namespace SFO2O.M.Controllers
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.htm/{*pathInfo}");
            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");
            routes.IgnoreRoute("images/{*pathInfo}");
            routes.IgnoreRoute("log/{*pathInfo}");
            routes.IgnoreRoute("scripts/{*pathInfo}");
            routes.IgnoreRoute("styles/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?" });

            //搜索路由
            routes.MapRoute(
                name: "so",
                url: "so.html",
                defaults: new { controller = "Search", action = "Index", SEOKey = "so" });

            routes.MapRoute(
                name: "searchResult",
                url: "search.html",
                defaults: new { controller = "Search", action = "Search", SEOKey = "searchResult" });

            //单品
            routes.MapRoute(
                name: "item",
                url: "item.html",
                defaults: new { controller = "Item", action = "Index", SEOKey = "item" });

            //通用专题视图
            routes.MapRoute(
                name: "activity",
                url: "activity{activityName}.html",
                defaults: new { controller = "Active", action = "ActivityTemplate", SEOKey = "activity", activityName = UrlParameter.Optional });

            ////分类页
            //routes.MapRoute(
            //    name: "category",
            //    url: "category.html",
            //    defaults: new { controller = "Category", action = "Index", SEOKey = "category" });

            //新分类页
            routes.MapRoute(
                name: "categoryindex",
                url: "category.html",
                defaults: new { controller = "Category", action = "Default", SEOKey = "category" });

            //店铺首页
            routes.MapRoute(
                name: "Store",
                url: "store.html",
                defaults: new { controller = "Store", action = "Index", SEOKey = "Store" });
            //店铺介绍
            routes.MapRoute(
                name: "StoreIntro",
                url: "StoreIntro.html",
                defaults: new { controller = "Store", action = "Intro", SEOKey = "StoreIntro" });

            //订单确认
            routes.MapRoute(
                name: "OrderSumit",
                url: "OrderSumit.html",
                defaults: new { controller = "Order", action = "Submit", SEOKey = "OrderSumit" });

            //税率
            routes.MapRoute(
                name: "OrderTariff",
                url: "OrderTariff.html",
                defaults: new { controller = "Order", action = "Tariff", SEOKey = "OrderTariff" });

            //下单
            routes.MapRoute(
                name: "PlaceOrder",
                url: "PlaceOrder.html",
                defaults: new { controller = "Order", action = "BuySubmit", SEOKey = "PlaceOrder" });

            //下单
            routes.MapRoute(
                name: "FirstOrderAuthorize",
                url: "FirstOrderAuthorize.html",
                defaults: new { controller = "Account", action = "OrderAnuthorization", SEOKey = "FirstOrderAuthorize" });

            //支付
            routes.MapRoute(
                name: "PayPage",
                url: "PayPage.html",
                defaults: new { controller = "Pay", action = "Index", SEOKey = "PayPage", id = UrlParameter.Optional });
            //聚合富生成二维码

            routes.MapRoute(
                name:"TwoDimensionCode",
                url:"TwoDimensionCode.html",
                defaults: new { controller = "Pay", action = "TwoDimensionCode", SEOKey = "TwoDimensionCode" }
                );


            //支付
            routes.MapRoute(
                name: "OrderPay",
                url: "OrderPay.html",
                defaults: new { controller = "Pay", action = "Submit", SEOKey = "OrderPay" });
            //支付宝支付
            routes.MapRoute(
                name: "ZOrderPay",
                url: "ZOrderPay.html",
                defaults: new { controller = "Pay", action = "ZSubmit", SEOKey = "ZOrderPay" });

            //易宝快捷支付
            routes.MapRoute(
                name: "YOrderPay",
                url: "YOrderPay.html",
                defaults: new { controller = "Pay", action = "YeePay", SEOKey = "YOrderPay" });
            //支付结果同步
            routes.MapRoute(
                name: "ReturnPage",
                url: "ReturnPage.html",
                defaults: new { controller = "Pay", action = "ReturnPage", SEOKey = "ReturnPage" });

            //通过source  和 ChannelId 值进来的用户
            routes.MapRoute(
                name: "login",
                url: "account/login.html",
                defaults: new { controller = "Account", action = "toIndex", SEOKey = "login" });

            //支付结果异步
            routes.MapRoute(
                name: "NotifyPage",
                url: "NotifyPage.html",
                defaults: new { controller = "Pay", action = "NotifyPage", SEOKey = "NotifyPage" });

            //支付宝支付结果同步
            routes.MapRoute(
                name: "ZFBReturnPage",
                url: "ZFBReturnPage.html",
                defaults: new { controller = "Pay", action = "ZFBReturnPage", SEOKey = "ZFBReturnPage" });

            //支付宝支付结果异步
            routes.MapRoute(
                name: "ZFBNotifyPage",
                url: "ZFBNotifyPage.html",
                defaults: new { controller = "Pay", action = "ZFBNotifyPage", SEOKey = "ZFBNotifyPage" });
            routes.MapRoute(
                 name: "helpquestion",
                 url: "help/question-{id}.html",
                 defaults:
                     new { controller = "Help", action = "Question", id = UrlParameter.Optional, SEOKey = "question" }
                 );
            routes.MapRoute(
                 name: "helpterms",
                 url: "help/terms-{id}.html",
                 defaults:
                     new { controller = "Help", action = "terms", id = UrlParameter.Optional, SEOKey = "terms" }
                 );
            //品牌页
            routes.MapRoute(
                 name: "brandIndex",
                 url: "brand/{id}.html",
                 defaults:
                     new { controller = "Brand", action = "Index", id = UrlParameter.Optional, SEOKey = "brand" }
                 );

            //拆单支付页面
            routes.MapRoute(
                 name: "split.html",
                 url: "split.html",
                 defaults:
                     new { controller = "Split", action = "split", id = UrlParameter.Optional, SEOKey = "split" }
                 );
            //活动页
            routes.MapRoute(
                 name: "preheat",
                 url: "preheat.html",
                 defaults:
                     new { controller = "Active", action = "preheat" }
                     );
            //活动页-母亲节专题
            routes.MapRoute(
                 name: "motherday",
                 url: "motherday.html",
                 defaults:
                     new { controller = "Active", action = "MotherDay" }
                     );
            //活动页-拼生活专题
            routes.MapRoute(
                 name: "pinlife",
                 url: "pinlife.html",
                 defaults:
                     new { controller = "Active", action = "PinLife" }
                     );
            //活动页-六一儿童节专题
            routes.MapRoute(
                 name: "childrenday",
                 url: "childrenday.html",
                 defaults:
                     new { controller = "Active", action = "ChildrenDay" }
                     );
            //2016.6.15专题活动页
            routes.MapRoute(
                    name: "activity0615",
                    url: "activity0615.html",
                    defaults:
                    new { controller = "Active", action = "activity0615" }
                );
            //2016.7.1专题活动页
            routes.MapRoute(
                    name: "activity0701",
                    url: "activity0701.html",
                    defaults:
                    new { controller = "Active", action = "activity0701" }
                );
            //2016.7.27中秋月饼
            routes.MapRoute(
                    name: "Holiday",
                    url: "Holiday.html",
                    defaults:
                    new { controller = "Holiday", action = "Index" }
                );
                 routes.MapRoute(
                    name: "Productlist",
                    url: "Product.html",
                    defaults:
                    new { controller = "Product", action = "Plist" }
                );
            
            //消息中心

            //系统消息列表页
            routes.MapRoute(
                name: "syetemMsgList",
                url: "sysmsglist.html",
                defaults:
                new { controller = "Information", action = "SystemMsgList", SEOKey = "SystemMsgList" }
                );
            //系统消详情页
            routes.MapRoute(
                name: "systemMsgDetail",
                url: "sysmsg.html",
                defaults:
                new { controller = "Information", action = "SystemMsgDetail", id = UrlParameter.Optional, SEOKey = "SystemMsgList" }
                );

            //活动消息列表页
            routes.MapRoute(
                name: "GetActivityMsgList",
                url: "GetActivityMsgList.html",
                defaults:
                new { controller = "Information", action = "GetActivityMsgList", SEOKey = "GetActivityMsgList" }
                );

            routes.MapRoute(
                name: "activityMsgList",
                url: "activitymsglist.html",
                defaults:
                new { controller = "Information", action = "ActivityMsgList", SEOKey = "ActivityMsgList" }
                );

            routes.MapRoute(
                name: "ToInformationCenter",
                url: "ToInformationCenter.html",
                defaults:
                new { controller = "Information", action = "ToInformationCenter", SEOKey = "ToInformationCenter" }
                );

            //订单消息列表页
            routes.MapRoute(
                name: "orderMsgList",
                url: "ordermsglist.html",
                defaults:
                new { controller = "Information", action = "OrderMsgList", SEOKey = "OrderMsgList" }
                );

            routes.MapRoute(
                name: "GetOrderMsgList",
                url: "GetOrderMsgList.html",
                defaults:
                new { controller = "Information", action = "GetOrderMsgList", SEOKey = "GetOrderMsgList" }
                );

            //团详情页面
            routes.MapRoute(
                 name: "teamDetail",
                 url: "teamDetail.html",
                 defaults:
                     new { controller = "Team", action = "TeamDetail", id = UrlParameter.Optional, SEOKey = "TeamDetail" }
                 );

            //团分享
            routes.MapRoute(
                 name: "GetSharedInfo",
                 url: "GetSharedInfo.html",
                 defaults:
                     new { controller = "Shared", action = "GetSharedInfo", id = UrlParameter.Optional, SEOKey = "GetSharedInfo" }
                 );

            //
            routes.MapRoute(
                 name: "GetAccessToken",
                 url: "GetAccessToken.html",
                 defaults:
                     new { controller = "Team", action = "GetAccessToken", id = UrlParameter.Optional, SEOKey = "GetAccessToken" }
                 );

            //
            routes.MapRoute(
                 name: "GetWechatParams",
                 url: "GetWechatParams.html",
                 defaults:
                     new { controller = "Team", action = "GetWechatParams", id = UrlParameter.Optional, SEOKey = "GetWechatParams" }
                 );
            //微信支付异步回调接口
            routes.MapRoute(
                 name: "WxNotify",
                 url: "WxNotify.html",
                 defaults:
                     new { controller = "WxNotify", action = "index" }
                 );
            //微信支付新页面
            routes.MapRoute(
                 name: "WxPayPage",
                 url: "WxPayPage.html",
                 defaults:
                     new { controller = "Pay", action = "WxPay", id = UrlParameter.Optional }
                 );

            routes.MapRoute(
                name: "supermarketdefault",
                url: "supermarket.html",
                defaults: new { controller = "SuperMarket", action = "Default", id = UrlParameter.Optional }
            );

            //易宝回调页面
            routes.MapRoute(
                name: "YeeCallBack",
                url: "YeeCallBack.html",
                defaults: new { controller = "Pay", action = "YeeCallBack", id = UrlParameter.Optional }
            );

            //易宝回调页面
            routes.MapRoute(
                name: "activehtml",
                url: "active.html",
                defaults: new { controller = "order", action = "activeindex", id = UrlParameter.Optional }
            );
            
            
            //全小写路由
            routes.MapRouteLowercase(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "index", id = UrlParameter.Optional }
            );

        }
    }
}