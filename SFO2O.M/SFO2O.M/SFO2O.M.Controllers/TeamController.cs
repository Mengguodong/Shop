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

namespace SFO2O.M.Controllers
{
    public class TeamController : SFO2OBaseController
    {
        private readonly TeamBll teamBll = new TeamBll();
        private readonly CommonBll commonBll = new CommonBll();

        /// <summary>
        /// 团详情页
        /// </summary>
        /// <param name="TeamCode"></param>
        /// <returns></returns>
        public ActionResult TeamDetail(string TeamCode,string OrderCode,int Flag)
        {
            try
            {
                string teamCodeStr = "";

                if (OrderCode != null)
                {
                    /// 获取订单所属的团信息
                    TeamInfoEntity teamInfoEntity = teamBll.GetTeamInfoEntity(OrderCode);
                    teamCodeStr = teamInfoEntity.TeamCode;

                    /// 获得团成员UserId
                    int TeamUserId = teamBll.GetTeamUserId(OrderCode, teamCodeStr);
                    ViewBag.TeamUserId = TeamUserId;/// 团成员UserId
                    ViewBag.IsOrderCodeInput = 1;/// OrderCode有值
                }
                else
                {
                    teamCodeStr = TeamCode;
                }

                if (string.IsNullOrEmpty(teamCodeStr))
                {
                    return View("Error");
                }

                /// 获取团详情信息
                var teamDetailListInfo = teamBll.GetTeamDetailList(teamCodeStr);
                List<TeamDetailEntity> teamDetailList = new List<TeamDetailEntity>();
                foreach (TeamDetailEntity product in teamDetailListInfo)
                {
                    string neight=commonBll.getProductDetailName(product.MainDicValue, product.SubDicValue, product.NetWeightUnit);
                    product.NetWeightUnit = neight;
                    teamDetailList.Add(product);
                }

                /// 登录用户ID
                int LoginUserId = 0;
                
                if (base.LoginUser != null)
                {
                    LoginUserId = base.LoginUser.UserID;
                }

                /// 倒计时秒数
                double sec = 0.0;

                DateTime dtStartTime = DateTime.Now;
                DateTime dtEndTime = Convert.ToDateTime(teamDetailList.First().EndTime);
                sec = dtEndTime.Subtract(dtStartTime).TotalSeconds;

                /// 组团成功
                if (teamDetailList.First().TeamStatus == 3)
                {
                    ViewBag.TeamStatus = 3;
                }
                /// 参团中
                else if (teamDetailList.First().TeamStatus == 1)
                {
                    if (sec > 0)
                    {
                        ViewBag.restTime = sec;
                    }
                    else
                    {
                        ViewBag.restTime = 0;
                    }
                    ViewBag.TeamStatus = teamDetailList.First().TeamStatus;/// 组团状态
                }
                /// 参团失败
                else
                {
                    ViewBag.TeamStatus = teamDetailList.First().TeamStatus;/// 组团状态
                }

                /// 设置页面属性值
                ViewBag.LoginUserId = LoginUserId;
                ViewBag.TeamHead = teamDetailList.First().TeamHead;/// 团长
                ViewBag.TeamNumbers = teamDetailList.First().TeamNumbers;/// 几人团
                ViewBag.ListLength = teamDetailList.Count();/// 团详情集合长度
                ViewBag.productPrice = (teamDetailList.First().DiscountPrice * base.ExchangeRate).ToNumberRoundStringWithPoint();/// 拼团价格
                ViewBag.RestNumber = teamDetailList.First().TeamNumbers - teamDetailList.Count();/// 组团剩余人数
                ViewBag.Flag = Flag;
                ///ViewBag.restTime = restTime;
                return View(teamDetailList);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }

        }

        public JsonResult GetSharedInfo(string TeamCode,string URL)
        {
            /// 获取团详情信息
            var teamDetailList = teamBll.GetTeamDetailList(TeamCode);
            
            if(teamDetailList.Count() == 0){
                return null;
            }

            LogHelper.Info("------------TeamDetail-----URL:" + URL);
            LogHelper.Info("------------TeamDetail-----ImagePath:" + teamDetailList.First().ImagePath);

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
                    strbul.Append("我参加了爱玖网").Append(teamDetailList.First().ProductName)
                            .Append(teamDetailList.First().MainValue).Append(teamDetailList.First().SubValue)
                            .Append(teamDetailList.First().NetWeightUnit).Append("拼单!");

                    teamSharedModel.Title = strbul.ToString();

                    /// 第一张缩略图（180x180图）
                    teamSharedModel.ImagePath = PathHelper.GetImageSmallUrl(teamDetailList.First().ImagePath);

                    /// 描述
                    if (teamDetailList.Count() < teamDetailList.First().TeamNumbers)
                    {
                        teamSharedModel.Description = "【还差"
                            + (teamDetailList.First().TeamNumbers - teamDetailList.Count())
                            + "x人】爱玖网，贵州茅台怀桥酒厂发货，全场包邮，一起实惠一起拼！";
                        
                    }
                    else
                    {
                        teamSharedModel.Description = "团人数已经满了";
                    }

                    /// 团详情页链接
                    /*teamSharedModel.Url = System.Web.Configuration.WebConfigurationManager.AppSettings["sharedUrl"].ToString()
                                                + teamDetailList.First().TeamCode + "&Flag=1";*/
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
                    teamSharedModel.Description = "爱玖网，贵州茅台怀桥酒厂发货，全场包邮，一起实惠一起拼！";

                    /// 团详情页链接
                    /*teamSharedModel.Url = System.Web.Configuration.WebConfigurationManager.AppSettings["sharedUrl"].ToString()
                                                + teamDetailList.First().TeamCode + "&Flag=1";*/
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
                teamSharedModel.Description = "爱玖网，贵州茅台怀桥酒厂发货，全场包邮，一起实惠一起拼！";

                /// 团详情页链接
                /*teamSharedModel.Url = System.Web.Configuration.WebConfigurationManager.AppSettings["sharedUrl"].ToString()
                                            + teamDetailList.First().TeamCode + "&Flag=1";*/
            }

            /// 微信接口属性
            var appId = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"].ToString();
            var nonceStr = JsonHelper.CreatenNonce_str();
            LogHelper.Info("------------TeamDetail-----nonceStr:" + nonceStr);
            var timestamp = JsonHelper.CreatenTimestamp();
            LogHelper.Info("------------TeamDetail-----timestamp:" + timestamp);
            //var domain = System.Configuration.ConfigurationManager.AppSettings["MServer"];
            //var url = domain + Request.Url.PathAndQuery;
            //var url = Request.Url.AbsoluteUri;
            string realUrl = URL.Split(new char[] { '#' })[0];
            teamSharedModel.Url = realUrl;
            LogHelper.Info("------------TeamDetail-----realUrl:" + realUrl);
           // var url = Request.Url.AbsoluteUri;
           
           // LogHelper.Info("------------TeamDetail-----url:" + url);
            AccessTokenModel accessTokenModel = GetAccessToken();

            if (accessTokenModel.type == 2)
            {
                LogHelper.Info("------------TeamDetail-----token-error-logic:" + accessTokenModel.type);
                return Json(new
                {
                    type = accessTokenModel.type,
                    content = accessTokenModel.content
                }, JsonRequestBehavior.AllowGet);
            }

            JsapiTicketModel jsapiTicketModel = GetTickect(accessTokenModel.access_token);

            if (jsapiTicketModel.type == 2)
            {
                LogHelper.Info("------------TeamDetail-----ticket-error-logic:" + jsapiTicketModel.type);
                return Json(new
                {
                    type = jsapiTicketModel.type,
                    content = jsapiTicketModel.content
                }, JsonRequestBehavior.AllowGet);
            }

            var string1 = "";
            var signature = JsonHelper.GetSignature(jsapiTicketModel.ticket, nonceStr, timestamp, realUrl, out string1);
            LogHelper.Info("------------TeamDetail-----signature:" + signature);
            LogHelper.Info("------------TeamDetail-----string1:" + string1);
            string[] jsApiList = {"onMenuShareTimeline","onMenuShareAppMessage"};/// 分享朋友和朋友圈

            /// 微信接口必填参数Model
            var model = new JSSDKModel
            {
                appId = appId,
                nonceStr = nonceStr,
                signature = signature,
                timestamp = timestamp,
                shareUrl = realUrl,
                jsapiTicket = jsapiTicketModel.ticket,
                //shareImg = domain + Url.Content("/images/ad.jpg"),
                string1 = string1,
                jsApiList = jsApiList,
                type = 1
            };
            LogHelper.Info("------------RestTeamMemberNum-----TeamCode:" + TeamCode);
            int value = teamSharedModel.RestTeamMemberNum;
            LogHelper.Info("------------RestTeamMemberNum-----RestTeamMemberNum2:" + value);


            LogHelper.Info("------------TeamDetil-----Title:" + teamSharedModel.Title);
            LogHelper.Info("------------TeamDetil-----ImagePath:" + teamSharedModel.ImagePath);
            LogHelper.Info("------------TeamDetil-----Description:" + teamSharedModel.Description);
            LogHelper.Info("------------TeamDetil-----Url:" + teamSharedModel.Url);

            return Json(new
            {
                type = model.type,
                data = new
                {
                    appId = appId,
                    timestamp = timestamp,
                    nonceStr = nonceStr,
                    signature = signature,
                    jsApiList = jsApiList
                    ,
                    Title = teamSharedModel.Title,
                    ImagePath = teamSharedModel.ImagePath,
                    
                    Description = teamSharedModel.Description
                    ,
                    RestTeamMemberNum = value,
                    
                    Url = teamSharedModel.Url
                }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查看团的状态
        /// </summary>
        public JsonResult checkTeamStatus(string orderCode)
        {
            //判断团的状态
            TeamInfoEntity teamInfoEntity = teamBll.GetTeamInfoEntity(orderCode);
            if (teamInfoEntity != null)
            {
                return Json(new { Type = 1, status = teamInfoEntity.TeamStatus }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Type = 1, status = 0 }, JsonRequestBehavior.AllowGet);
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
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
            var client = new HttpClient();

            /// 声明变量
            string access_token_str = "";
            int expires_in_value = 7200;
            int type = 0;
            string content = "";

            LogHelper.Info("------------TeamDetail-----执行autocach_token开始");
            /// 缓存
            var modules = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, "Key_AccessToken", () =>
            {
                LogHelper.Info("------------TeamDetail-----进入autocach_token开始");
                LogHelper.Info("------------TeamDetail-----获取token开始");
                /// 获取access_token
                var responseResult = client.GetAsync(url).Result;
                LogHelper.Info("------------TeamDetail-----获取token结束");
                if (!responseResult.IsSuccessStatusCode) return string.Empty;
                var result = responseResult.Content.ReadAsStringAsync().Result;
                var parseResult = JObject.Parse(result.ToString());

                /// 测试用固定值
                //string json = "{\"access_token\":\"C9-moFZa8Vg4izrX7Ty7SC-cDasK88bxfSTV2DyzD4XsF-z2jmAQB4lUwkgQx5CD0rusKuHWSsd7aihDPpXECzlBaRADp_5eff2vWuzkYQdNLTY1wGfwLDQZiryIeFbjSLNeAGAIBD\",\"expires_in\":7200}";
                //var parseResult = JObject.Parse(json);

                var errcode = parseResult["errcode"];
                LogHelper.Info("------------TeamDetail-----token-errorcode:" + errcode);
                if (errcode == null)
                {
                    type = 1;
                    var access_token = parseResult["access_token"];
                    LogHelper.Info("------------TeamDetail-----access_token:" + access_token);
                    if (access_token != null)
                    {
                        access_token_str = access_token.ToString();
                    }

                    var expires_in = parseResult["expires_in"];
                    if (expires_in != null)
                    {
                        LogHelper.Info("------------TeamDetail-----token-expires_in:" + expires_in);
                        expires_in_value = Convert.ToInt32(expires_in);
                        LogHelper.Info("------------TeamDetail-----token-expires_in_ToInt32:" + expires_in_value);
                    }
                }
                else
                {
                    type = 2;
                    content = parseResult["errcode"].ToString();
                    access_token_str = "";
                }
                
                var data = access_token_str;
                LogHelper.Info("------------TeamDetail-----autocach-token最后返回的data:" + data);
                LogHelper.Info("------------TeamDetail-----进入autocach_token结束");
                return data;
            }, 7200);


            LogHelper.Info("------------TeamDetail-----执行autocach_token结束");
            LogHelper.Info("------------TeamDetail-----:token缓存modules" + modules);

            AccessTokenModel accessTokenModel = new AccessTokenModel();

            if (modules == null || modules.Equals(""))
            {
                accessTokenModel.type = type;
                accessTokenModel.content = content;
                accessTokenModel.access_token = "";
            }
            else
            {
                accessTokenModel.type = 1;
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
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", access_token);
            var client = new HttpClient();

            /// 声明变量
            string ticket_str = "";
            int expires_in_value = 7200;
            int type = 0;
            string content = "";

            LogHelper.Info("------------TeamDetail-----执行autocach-ticket开始");
            /// 缓存
            var modules = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, "Key_Ticket", () =>
            {
                LogHelper.Info("------------TeamDetail-----进入autocach-ticket开始");
                LogHelper.Info("------------TeamDetail-----获取ticket开始");
                /// 获取ticket
                var responseResult = client.GetAsync(url).Result;
                LogHelper.Info("------------TeamDetail-----获取ticket开始");
                if (!responseResult.IsSuccessStatusCode) return string.Empty;
                var result = responseResult.Content.ReadAsStringAsync().Result;
                var parseResult = JObject.Parse(result.ToString());

                //var json = "{\"errcode\":0,\"errmsg\":\"ok\",\"ticket\":\"sM4AOVdWfPE4DxkXGEs8VBnCOGWyCh0yozn7VHLbWf0wR7SqMRVe-h4niB2YDaeVuDMBPYzyNoVJRY4ZZOab8Q\",\"expires_in\":7200}";
                //var parseResult = JObject.Parse(json);

                var errcode = parseResult["errcode"];
                LogHelper.Info("------------TeamDetail-----ticket-errorcode:" + errcode);
                var errmsg = parseResult["errmsg"];
                var ticket = parseResult["ticket"];
                LogHelper.Info("------------TeamDetail-----ticket:" + ticket);
                var expires_in = parseResult["expires_in"];
                LogHelper.Info("------------TeamDetail-----ticket-expires_in:" + expires_in);

                if (errcode != null && errmsg != null)
                {
                    if (Convert.ToInt32(errcode) == 0 && errmsg.ToString().Equals("ok"))
                    {
                        type = 1;
                        if (ticket != null)
                        {
                            ticket_str = ticket.ToString();
                        }

                        if (expires_in != null)
                        {
                            expires_in_value = Convert.ToInt32(expires_in);
                        }
                    }else{
                        type = 2;
                        content = parseResult["errmsg"].ToString();
                        ticket_str = "";
                    }
                }

                var data = ticket_str;
                LogHelper.Info("------------TeamDetail-----ticket_str:" + ticket_str);
                LogHelper.Info("------------TeamDetail-----进入autocach-ticket结束");
                return data;
            }, 7200);

            LogHelper.Info("------------TeamDetail-----执行autocach-ticket结束");
            LogHelper.Info("------------TeamDetail-----:ticket缓存modules" + modules);
            JsapiTicketModel jsapiTicketModel = new JsapiTicketModel();
            

            if (modules == null || modules.Equals(""))
            {
                jsapiTicketModel.type = type;
                jsapiTicketModel.content = content;
                jsapiTicketModel.ticket = "";
            }
            else
            {
                jsapiTicketModel.type = 1;
                jsapiTicketModel.content = "";
                jsapiTicketModel.ticket = modules.ToString();
            }

            return jsapiTicketModel;
        }

    }
}