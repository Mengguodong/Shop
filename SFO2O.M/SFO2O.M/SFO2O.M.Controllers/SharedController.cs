using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SFO2O.M.Controllers.Extensions;
using SFO2O.Model.Extensions;
using SFO2O.BLL.Team;
using SFO2O.BLL.Common;
using SFO2O.Model.Team;
using SFO2O.Utility.Uitl;
using SFO2O.Utility.Extensions;
using System.Net.Http;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Caching;
using SFO2O.Utility.Cache;
using SFO2O.Utility;
using SFO2O.BLL.Product;
using SFO2O.Model.Product;
using SFO2O.M.ViewModel.Product;
using SFO2O.BLL.Activity;
using System.Configuration;

namespace SFO2O.M.Controllers
{
    public class SharedController : SFO2OBaseController
    {
        private readonly TeamBll teamBll = new TeamBll();
        private readonly CommonBll commonBll = new CommonBll();
        private readonly ProductBll productBll = new ProductBll();
        private readonly ActivityBll activityBll = new ActivityBll();

        public JsonResult GetSharedInfo(string type, string URL)
        {
            /// type值为空
            if (string.IsNullOrEmpty(type))
            {
                return Json(new
                {
                    type = (int)ShareUtils.JsonType.typeFailed,
                    content = ShareUtils.JsonContent_TypeIsNull
                }, JsonRequestBehavior.AllowGet);
            }

            /// URL为空
            if (string.IsNullOrEmpty(URL))
            {
                return Json(new
                {
                    type = (int)ShareUtils.JsonType.typeFailed,
                    content = ShareUtils.JsonContent_UrlIsNull
                }, JsonRequestBehavior.AllowGet);
            }
            LogHelper.Info("------------PinLifeDetail---刚进入方法--URL:" + URL);
            JsonResult json = null;

            /// 团详情页分享
            if (type.Equals(ShareUtils.TeamSharedFlag))
            {
                // 获得团详情页分享Json
                json = TeamJoinDetail(URL);
            }
            // 拼生活详情页分享Json
            else if (type.Equals(ShareUtils.PinLifeDetailSharedFlag))
            {
                // 拼生活详情页分享Json
                json = PinLifeProductDetail(URL);
            }
            // 专题页
            else
            {
                // 根据专题活动标志Key获取专题对象
                var model = activityBll.GetActivityByKey(type);

                if(model == null){
                    json = Json(new
                    {
                        type = (int)ShareUtils.JsonType.typeFailed,
                        content = ShareUtils.JsonContent_JsonIsNull
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string imgPath = model.ImgPath + "shareImg.jpg?v=" + ConfigurationManager.AppSettings["ImgVersion"].ToString();
                    // 专题页Json
                    json = SpecialPage(type, URL
                        , model.Title, model.Discription, imgPath);
                }
            }
            
            return json;
        }
        /// <summary>
        /// 获得专题页分享信息
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Description"></param>
        /// <param name="SharedImage"></param>
        /// <param name="URL"></param>
        /// <returns></returns>
        public JsonResult GetSpecialPageSharedInfo(string Title, string Description, string SharedImage, string URL)
        {
            /// 
            SpecialPageModel SpecialPageModel = new SpecialPageModel();
            SpecialPageModel.Title = Title;
            SpecialPageModel.Description = Description;
            SpecialPageModel.SharedImage = SharedImage;

            /// 获得微信配置信息
            JSSDKModel JsSdkModel = GetWechatParams(URL);
            if (JsSdkModel.type == (int)ShareUtils.JsonType.typeFailed)
            {
                return Json(new
                {
                    type = JsSdkModel.type,
                    content = JsSdkModel.content
                }, JsonRequestBehavior.AllowGet);
            }
            //LogHelper.Info("------------Shared-----Title:" + SpecialPageModel.Title);
            //LogHelper.Info("------------Shared-----ImagePath:" + SpecialPageModel.SharedImage);
            //LogHelper.Info("------------Shared-----Description:" + SpecialPageModel.Description);
            //LogHelper.Info("------------Shared-----Url:" + URL);

            return Json(new
            {
                type = JsSdkModel.type,
                data = new
                {
                    appId = JsSdkModel.appId,
                    timestamp = JsSdkModel.timestamp,
                    nonceStr = JsSdkModel.nonceStr,
                    signature = JsSdkModel.signature,
                    jsApiList = JsSdkModel.jsApiList,
                    Title = SpecialPageModel.Title,
                    ImagePath = SpecialPageModel.SharedImage,
                    Description = SpecialPageModel.Description,
                    Url = URL
                }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获得团详情分享信息
        /// </summary>
        /// <param name="teamDetailList"></param>
        /// <returns></returns>
        public TeamSharedModel GetTeamSharedInfo(IList<TeamDetailEntity> teamDetailList)
        {
            /// 团分享对象Model
            TeamSharedModel teamSharedModel = new TeamSharedModel();

            /// 登录用户ID
            int LoginUserId = 0;

            // 登录用户
            if (base.LoginUser != null)
            {
                LoginUserId = base.LoginUser.UserID;
            }

            teamSharedModel.RestTeamMemberNum = (teamDetailList.First().TeamNumbers - teamDetailList.Count());
            LogHelper.Info("------------RestTeamMemberNum-----RestTeamMemberNum1:" + teamSharedModel.RestTeamMemberNum);

            /// 用户已经登录
            if (LoginUserId > 0)
            {
                /// 团成员
                if (teamDetailList.Where(d => d.UserId == LoginUserId).Count() > 0)
                {
                    /// 标题
                    StringBuilder strbul = new StringBuilder();
                    strbul.Append(ShareUtils.TeamJoinSharedTitle_Joined_prefix).Append(teamDetailList.First().ProductName)
                            .Append(teamDetailList.First().MainValue).Append(teamDetailList.First().SubValue)
                            .Append(teamDetailList.First().NetWeightUnit).Append(ShareUtils.TeamJoinSharedTitle_Joined_suffix);

                    teamSharedModel.Title = strbul.ToString();

                    /// 第一张缩略图（180x180图）
                    teamSharedModel.ImagePath = PathHelper.GetImageSmallUrl(teamDetailList.First().ImagePath);

                    /// 描述
                    if (teamDetailList.Count() < teamDetailList.First().TeamNumbers)
                    {
                        teamSharedModel.Description = ShareUtils.TeamJoinSharedDescription_Joined_prefix 
                            + (teamDetailList.First().TeamNumbers - teamDetailList.Count())
                            + ShareUtils.TeamJoinSharedDescription_Joined_suffix;
                    }
                    else
                    {
                        teamSharedModel.Description = ShareUtils.TeamJoinSharedDescription_Joined_Full;
                    }

                }
                /// 未参团
                else
                {
                    /// 标题
                    StringBuilder strbul = new StringBuilder();
                    strbul.Append(teamDetailList.First().ProductName).Append(teamDetailList.First().MainValue)
                            .Append(teamDetailList.First().SubValue).Append(teamDetailList.First().NetWeightUnit);

                    teamSharedModel.Title = strbul.ToString();

                    /// 第一张缩略图（180x180图）
                    teamSharedModel.ImagePath = PathHelper.GetImageSmallUrl(teamDetailList.First().ImagePath);

                    /// 描述
                    teamSharedModel.Description = ShareUtils.TeamJoinSharedDescription_UnJoined;

                }
            }
            /// 未登录
            else
            {
                /// 标题
                StringBuilder strbul = new StringBuilder();
                strbul.Append(teamDetailList.First().ProductName).Append(teamDetailList.First().MainValue)
                        .Append(teamDetailList.First().SubValue).Append(teamDetailList.First().NetWeightUnit);

                teamSharedModel.Title = strbul.ToString();

                /// 第一张缩略图（180x180图）
                teamSharedModel.ImagePath = PathHelper.GetImageSmallUrl(teamDetailList.First().ImagePath);

                /// 描述
                teamSharedModel.Description = ShareUtils.TeamJoinSharedDescription_UnLogined;

            }

            return teamSharedModel;
        }

        /// <summary>
        /// 拼生活详情页分享信息
        /// </summary>
        /// <param name="productDetailProductList"></param>
        /// <returns></returns>
        public ProductModel GetPinLifeDetailSharedInfo(IList<ProductInfoModel> productDetailProductList)
        {
            /// 团分享对象Model
            ProductModel ProductViewModel = new ProductModel();

            /// 标题
            StringBuilder strbul = new StringBuilder();
            strbul.Append(productDetailProductList.First().Name).Append(productDetailProductList.First().MainValue)
                    .Append(productDetailProductList.First().SubValue).Append(productDetailProductList.First().NetWeightUnit);

            ProductViewModel.Title = strbul.ToString();

            /// 第一张缩略图（180x180图）
            ProductViewModel.ImagePath = PathHelper.GetImageSmallUrl(productDetailProductList.First().ImagePath);

            /// 描述
            ProductViewModel.Description = ShareUtils.PinLifeDetailSharedDescription;

            return ProductViewModel;
        }

        /// <summary>
        /// 获得微信参数
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public JSSDKModel GetWechatParams(string URL)
        {
            /// 微信接口属性
            var appId = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"].ToString();
            var nonceStr = JsonHelper.CreatenNonce_str();
            //LogHelper.Info("------------Shared-----nonceStr:" + nonceStr);
            var timestamp = JsonHelper.CreatenTimestamp();
            //LogHelper.Info("------------Shared-----timestamp:" + timestamp);
            
            string realUrl = URL.Split(new char[] { '#' })[0];
            //LogHelper.Info("------------Shared-----realUrl:" + realUrl);
            
            AccessTokenModel accessTokenModel = GetAccessToken();

            JSSDKModel JsSdkModel = new JSSDKModel();

            if (accessTokenModel.type == (int)ShareUtils.JsonType.typeFailed)
            {
                //LogHelper.Info("------------Shared-----token-error-logic:" + accessTokenModel.type);
                JsSdkModel.type = accessTokenModel.type;
                JsSdkModel.content = accessTokenModel.content;
                return JsSdkModel;
            }

            JsapiTicketModel jsapiTicketModel = GetTickect(accessTokenModel.access_token);

            if (jsapiTicketModel.type == (int)ShareUtils.JsonType.typeFailed)
            {
                //LogHelper.Info("------------Shared-----ticket-error-logic:" + jsapiTicketModel.type);
                JsSdkModel.type = jsapiTicketModel.type;
                JsSdkModel.content = jsapiTicketModel.content;
                return JsSdkModel;
            }

            var string1 = "";
            var signature = JsonHelper.GetSignature(jsapiTicketModel.ticket, nonceStr, timestamp, realUrl, out string1);
            //LogHelper.Info("------------Shared-----signature:" + signature);
            //LogHelper.Info("------------Shared-----string1:" + string1);

            string[] jsApiList = { "onMenuShareTimeline", "onMenuShareAppMessage" };/// 分享朋友和朋友圈

            /// 
            JsSdkModel.appId = appId;
            JsSdkModel.nonceStr = nonceStr;
            JsSdkModel.signature = signature;
            JsSdkModel.timestamp = timestamp;
            JsSdkModel.shareUrl = URL;
            JsSdkModel.jsapiTicket = jsapiTicketModel.ticket;
            JsSdkModel.string1 = string1;
            JsSdkModel.jsApiList = jsApiList;
            JsSdkModel.type = (int)ShareUtils.JsonType.typeSucc;

            return JsSdkModel;
        }

        /// <summary>
        /// 获取jsapi_ticket
        /// jsapi_ticket是公众号用于调用微信JS接口的临时票据。
        /// 正常情况下，jsapi_ticket的有效期为7200秒，通过access_token来获取。
        /// 由于获取jsapi_ticket的api调用次数非常有限，频繁刷新jsapi_ticket会导致api调用受限，影响自身业务，开发者必须在自己的服务全局缓存jsapi_ticket 。
        /// </summary>
        /// <param name="access_token">BasicAPI获取的access_token,也可以通过TokenHelper获取</param>
        /// <returns></returns>
        public AccessTokenModel GetAccessToken()
        {
            /// 第三方用户唯一凭证和第三方用户唯一凭证密钥
            string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"].ToString();
            string secret = System.Web.Configuration.WebConfigurationManager.AppSettings["secret"].ToString(); ;

            /// 请求URL
            var url = string.Format(ShareUtils.WechatTokenRequestUrl, appid, secret);
            var client = new HttpClient();

            /// 声明变量
            string access_token_str = "";
            int type = 0;
            string content = "";

            /// 缓存
            var modules = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, "Key_AccessToken", () =>
            {
                /// 获取access_token
                var responseResult = client.GetAsync(url).Result;
                if (!responseResult.IsSuccessStatusCode) return string.Empty;
                var result = responseResult.Content.ReadAsStringAsync().Result;
                var parseResult = JObject.Parse(result.ToString());

                /// 测试用固定值
                //string json = "{\"access_token\":\"C9-moFZa8Vg4izrX7Ty7SC-cDasK88bxfSTV2DyzD4XsF-z2jmAQB4lUwkgQx5CD0rusKuHWSsd7aihDPpXECzlBaRADp_5eff2vWuzkYQdNLTY1wGfwLDQZiryIeFbjSLNeAGAIBD\",\"expires_in\":7200}";
                //var parseResult = JObject.Parse(json);

                var errcode = parseResult["errcode"];
                //LogHelper.Info("------------Shared-----token-errorcode:" + errcode);
                if (errcode == null)
                {
                    type = (int)ShareUtils.JsonType.typeSucc;
                    var access_token = parseResult["access_token"];
                    //LogHelper.Info("------------Shared-----access_token:" + access_token);
                    if (access_token != null)
                    {
                        access_token_str = access_token.ToString();
                    }

                    var expires_in = parseResult["expires_in"];
                    /*if (expires_in != null)
                    {
                        LogHelper.Info("------------Shared-----token-expires_in:" + expires_in);
                    }*/
                }
                else
                {
                    type = (int)ShareUtils.JsonType.typeFailed;
                    content = parseResult["errcode"].ToString();
                    access_token_str = "";
                }
                
                var data = access_token_str;
                //LogHelper.Info("------------Shared-----autocach-token最后返回的data:" + data);
                return data;
            }, 120);

            //LogHelper.Info("------------Shared-----:token缓存modules" + modules);

            AccessTokenModel accessTokenModel = new AccessTokenModel();

            if (modules == null || modules.Equals(""))
            {
                accessTokenModel.type = type;
                accessTokenModel.content = content;
                accessTokenModel.access_token = "";
            }
            else
            {
                accessTokenModel.type = (int)ShareUtils.JsonType.typeSucc;
                accessTokenModel.content = "";
                accessTokenModel.access_token = modules.ToString();
            }

            return accessTokenModel;
        }

        /// <summary>
        /// 获取jsapi_ticket
        /// jsapi_ticket是公众号用于调用微信JS接口的临时票据。
        /// 正常情况下，jsapi_ticket的有效期为7200秒，通过access_token来获取。
        /// 由于获取jsapi_ticket的api调用次数非常有限，频繁刷新jsapi_ticket会导致api调用受限，影响自身业务，开发者必须在自己的服务全局缓存jsapi_ticket 。
        /// </summary>
        /// <param name="access_token">BasicAPI获取的access_token,也可以通过TokenHelper获取</param>
        /// <returns></returns>
        public JsapiTicketModel GetTickect(string access_token)
        {
            /// 请求URL
            var url = string.Format(ShareUtils.WechatTicketRequestUrl, access_token);
            var client = new HttpClient();

            /// 声明变量
            string ticket_str = "";
            int type = 0;
            string content = "";

            /// 缓存
            var modules = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, "Key_Ticket", () =>
            {
                /// 获取ticket
                var responseResult = client.GetAsync(url).Result;
                if (!responseResult.IsSuccessStatusCode) return string.Empty;
                var result = responseResult.Content.ReadAsStringAsync().Result;
                var parseResult = JObject.Parse(result.ToString());

                //var json = "{\"errcode\":0,\"errmsg\":\"ok\",\"ticket\":\"sM4AOVdWfPE4DxkXGEs8VBnCOGWyCh0yozn7VHLbWf0wR7SqMRVe-h4niB2YDaeVuDMBPYzyNoVJRY4ZZOab8Q\",\"expires_in\":7200}";
                //var parseResult = JObject.Parse(json);

                var errcode = parseResult["errcode"];
                //LogHelper.Info("------------Shared-----ticket-errorcode:" + errcode);
                var errmsg = parseResult["errmsg"];
                var ticket = parseResult["ticket"];
                //LogHelper.Info("------------Shared-----ticket:" + ticket);
                var expires_in = parseResult["expires_in"];
                //LogHelper.Info("------------Shared-----ticket-expires_in:" + expires_in);

                if (errcode != null && errmsg != null)
                {
                    if (Convert.ToInt32(errcode) == 0 && errmsg.ToString().Equals("ok"))
                    {
                        type = (int)ShareUtils.JsonType.typeSucc;
                        if (ticket != null)
                        {
                            ticket_str = ticket.ToString();
                        }

                    }else{
                        type = (int)ShareUtils.JsonType.typeFailed;
                        content = parseResult["errmsg"].ToString();
                        ticket_str = "";
                    }
                }

                var data = ticket_str;
                //LogHelper.Info("------------Shared-----ticket_str:" + ticket_str);
                return data;
            }, 120);

            //LogHelper.Info("------------Shared-----:ticket缓存modules" + modules);
            JsapiTicketModel jsapiTicketModel = new JsapiTicketModel();
            

            if (modules == null || modules.Equals(""))
            {
                jsapiTicketModel.type = type;
                jsapiTicketModel.content = content;
                jsapiTicketModel.ticket = "";
            }
            else
            {
                jsapiTicketModel.type = (int)ShareUtils.JsonType.typeSucc;
                jsapiTicketModel.content = "";
                jsapiTicketModel.ticket = modules.ToString();
            }

            return jsapiTicketModel;
        }

        /// <summary>
        /// 获得团详情页分享Json
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        private JsonResult TeamJoinDetail(string URL)
        {
            string TeamCodeStr = "";
            string OrderCodeStr = "";
            JsonResult json = null;

            Uri uri = new Uri(URL);
            int index = uri.Query.IndexOf("?");
            string urlQuery = uri.Query.Substring(index + 1, uri.Query.Length - 1);
            string[] urlParams = urlQuery.Split('&');
            foreach (string param in urlParams)
            {
                string[] items = param.Split('=');
                if (items[0].Equals("TeamCode"))
                {
                    TeamCodeStr = items[1];
                }
                else if (items[0].Equals("OrderCode"))
                {
                    OrderCodeStr = items[1];
                }

            }
            //LogHelper.Info("------------Shared-----URL1:" + URL);

            IList<TeamDetailEntity> teamDetailList = null;

            if (!string.IsNullOrEmpty(TeamCodeStr))
            {
                /// 获取团详情信息
                teamDetailList = teamBll.GetTeamDetailList(TeamCodeStr);
                //LogHelper.Info("------------Shared-----teamDetailList:" + teamDetailList);
            }
            else if (!string.IsNullOrEmpty(OrderCodeStr))
            {
                TeamInfoEntity teamInfoEntity = teamBll.GetTeamInfoEntity(OrderCodeStr);
                //LogHelper.Info("------------Shared-----teamInfoEntity:" + teamInfoEntity);
                if (teamInfoEntity != null)
                {
                    TeamCodeStr = teamInfoEntity.TeamCode;
                    //LogHelper.Info("------------Shared-----TeamCodeStr:" + TeamCodeStr);

                    /// 获取团详情信息
                    teamDetailList = teamBll.GetTeamDetailList(TeamCodeStr);
                    //LogHelper.Info("------------Shared-----teamDetailList:" + teamDetailList);
                }
            }
            else
            {
                return Json(new
                {
                    type = (int)ShareUtils.JsonType.typeFailed,
                    content = ShareUtils.JsonContent_TeamCodeIsNull
                }, JsonRequestBehavior.AllowGet);
            }

            /// 判断集合是否为空
            if (teamDetailList == null || teamDetailList.Count() == 0)
            {
                return Json(new
                {
                    type = (int)ShareUtils.JsonType.typeFailed,
                    content = ShareUtils.JsonContent_TeamListIsNull
                }, JsonRequestBehavior.AllowGet);
            }

            //LogHelper.Info("------------Shared-----URL2:" + URL);
            //LogHelper.Info("------------Shared-----ImagePath:" + teamDetailList.First().ImagePath);

            // 获得商品净重、净含量信息，再重新装配数据集合
            List<TeamDetailEntity> teamDetailProductList = new List<TeamDetailEntity>();
            foreach (TeamDetailEntity product in teamDetailList)
            {
                string neight = commonBll.getProductDetailName(product.MainDicValue, product.SubDicValue, product.NetWeightUnit);
                product.NetWeightUnit = neight;
                teamDetailProductList.Add(product);
            }

            /// 获得团详情页分享对象
            TeamSharedModel teamSharedModel = GetTeamSharedInfo(teamDetailProductList);

            /// 获得微信配置信息
            JSSDKModel JsSdkModel = GetWechatParams(URL);
            if (JsSdkModel.type == (int)ShareUtils.JsonType.typeFailed)
            {
                return Json(new
                {
                    type = JsSdkModel.type,
                    content = JsSdkModel.content
                }, JsonRequestBehavior.AllowGet);
            }

            //LogHelper.Info("------------Shared-------TeamCode:" + TeamCodeStr);

            // 获得团成员剩余数量
            int value = teamSharedModel.RestTeamMemberNum;
            /*LogHelper.Info("------------Shared----RestTeamMemberNum-----RestTeamMemberNum:" + value);
            LogHelper.Info("------------Shared-----Title:" + teamSharedModel.Title);
            LogHelper.Info("------------Shared-----ImagePath:" + teamSharedModel.ImagePath);
            LogHelper.Info("------------Shared-----Description:" + teamSharedModel.Description);
            LogHelper.Info("------------Shared-----URL3:" + teamSharedModel.Url);*/

            json = Json(new
            {
                type = JsSdkModel.type,
                data = new
                {
                    appId = JsSdkModel.appId,
                    timestamp = JsSdkModel.timestamp,
                    nonceStr = JsSdkModel.nonceStr,
                    signature = JsSdkModel.signature,
                    jsApiList = JsSdkModel.jsApiList,
                    Title = teamSharedModel.Title,
                    ImagePath = teamSharedModel.ImagePath,
                    Description = teamSharedModel.Description,
                    Url = URL
                }
            }, JsonRequestBehavior.AllowGet);

            return json;
        }

        /// <summary>
        /// 拼生活详情页分享Json
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        private JsonResult PinLifeProductDetail(string URL)
        {
            string sku = "";
            string pid = "";
            JsonResult json = null;

            Uri uri = new Uri(URL);
            int index = uri.Query.IndexOf("?");
            string urlQuery = uri.Query.Substring(index + 1, uri.Query.Length - 1);
            string[] urlParams = urlQuery.Split('&');
            foreach (string param in urlParams)
            {
                string[] items = param.Split('=');
                if (items[0].Equals("sku"))
                {
                    sku = items[1];
                }
                else if (items[0].Equals("pid"))
                {
                    pid = items[1];
                }

            }
            LogHelper.Info("------------PinLifeDetail-----URL1:" + URL);

            IList<ProductInfoModel> productsDetail = new List<ProductInfoModel>();

            if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(pid))
            {
                // 拼生活商品详情页
                productsDetail = productBll.GetProductFightDetailForShare(sku, base.ExchangeRate, Int32.Parse(pid));
                LogHelper.Info("------------PinLifeDetail-----productsDetail:" + productsDetail);
            }
            else
            {
                return Json(new
                {
                    type = (int)ShareUtils.JsonType.typeFailed,
                    content = ShareUtils.JsonContent_PinLife_ParamIsNull
                }, JsonRequestBehavior.AllowGet);
            }

            /// 判断集合是否为空
            if (productsDetail == null || productsDetail.Count() == 0)
            {
                return Json(new
                {
                    type = (int)ShareUtils.JsonType.typeFailed,
                    content = ShareUtils.JsonContent_PinLifeDetailListIsNull
                }, JsonRequestBehavior.AllowGet);
            }

            LogHelper.Info("------------PinLifeDetail-----URL2:" + URL);
            LogHelper.Info("------------PinLifeDetail-----ImagePath:" + productsDetail.FirstOrDefault().ImagePath);

            // 获得商品净重、净含量信息，再重新装配数据集合
            List<ProductInfoModel> productDetailProductList = new List<ProductInfoModel>();
            foreach (ProductInfoModel product in productsDetail)
            {
                string neight = commonBll.getProductDetailName(product.MainDicValue, product.SubDicValue, product.NetWeightUnit);
                product.NetWeightUnit = neight;
                productDetailProductList.Add(product);
            }

            // 拼生活详情页分享信息
            ProductModel ProductViewModel = GetPinLifeDetailSharedInfo(productDetailProductList);

            /// 获得微信配置信息
            JSSDKModel JsSdkModel = GetWechatParams(URL);
            if (JsSdkModel.type == (int)ShareUtils.JsonType.typeFailed)
            {
                return Json(new
                {
                    type = JsSdkModel.type,
                    content = JsSdkModel.content
                }, JsonRequestBehavior.AllowGet);
            }

            /*LogHelper.Info("------------PinLifeDetail-------sku:" + sku);
            LogHelper.Info("------------PinLifeDetail-------pid:" + pid);
            LogHelper.Info("------------Shared-----Title:" + ProductViewModel.Title);
            LogHelper.Info("------------Shared-----ImagePath:" + ProductViewModel.ImagePath);
            LogHelper.Info("------------Shared-----Description:" + ProductViewModel.Description);*/

            json = Json(new
            {
                type = JsSdkModel.type,
                data = new
                {
                    appId = JsSdkModel.appId,
                    timestamp = JsSdkModel.timestamp,
                    nonceStr = JsSdkModel.nonceStr,
                    signature = JsSdkModel.signature,
                    jsApiList = JsSdkModel.jsApiList,
                    Title = ProductViewModel.Title,
                    ImagePath = ProductViewModel.ImagePath,
                    Description = ProductViewModel.Description,
                    Url = URL
                }
            }, JsonRequestBehavior.AllowGet);

            return json;
        }

        /// <summary>
        /// 专题页Json
        /// </summary>
        /// <param name="type"></param>
        /// <param name="URL"></param>
        /// <returns></returns>
        private JsonResult SpecialPage(string type, string URL,string SpecialTile,string SpecialDescription,string SpecialImage)
        {
            //LogHelper.Info("------------Shared-----SpecialPage---type:" + type);
            //LogHelper.Info("------------Shared-----SpecialPage---URL:" + URL);

            JsonResult json = null;

            // 设置专题页属性
            string Title = SpecialTile;
            string Description = SpecialDescription;
            string SharedImage = PathHelper.GetImageUrl(SpecialImage);

            // 获得专题页Json
            json = GetSpecialPageSharedInfo(Title, Description, SharedImage, URL);

            return json;
        }

    }
}