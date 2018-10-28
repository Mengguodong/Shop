using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SFO2O.BLL.Account;
using SFO2O.BLL.Message;
using SFO2O.M.Controllers.Common;
using SFO2O.M.Controllers.Extensions;
using SFO2O.M.ViewModel.Account;
using SFO2O.Utility.Uitl;
using SFO2O.Utility.Extensions;
using SFO2O.M.Controllers.Filters;
using SFO2O.Model.Account;
using SFO2O.BLL.Shopping;
using SFO2O.Model.Information;
using SFO2O.BLL.Information;
using SFO2O.BLL.Source;
using SFO2O.BLL.Common;
using SFO2O.Utility;
using SFO2O.Model.GiftCard;
using SFO2O.Model.SMS;

namespace SFO2O.M.Controllers
{

    public class AccountController : SFO2OBaseController
    {
        public AccountBll BLL = new AccountBll();
        private MessageBll messageBll = new MessageBll();
        private SourceBll sourceBll = new SourceBll();
        private static readonly InformationBll InformationBll = new InformationBll();

        /// <summary>
        /// 注册 手机验证码，发送短信
        /// </summary>
        //private string SMSRegistTip = @"【健康绿氧】您正在注册健康绿氧会员，校验码为:{0}，请于10分钟内输入。";

        //private string SMSFindPasswordTip = @"【健康绿氧】您正在找回密码，校验码为:{0}，请于10分钟内输入。";


        #region 用户注册

        /// <summary>
        /// 第一步 填写手机号码 
        /// </summary>
        /// <param name="t">0注册，1找回密码</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Register(int t = 0, string mobilePhone = "", string token = "")
        {
            Session["backUrl"] = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : string.Empty;
            ViewBag.Type = t;
            ViewBag.mobilePhone = mobilePhone;
            ViewBag.token = token;
            return View();
        }


        /// <summary>
        /// 第二步，填写校验码
        /// </summary>
        /// <param name="t">0注册，1找回密码</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Register2(int t = 0, string mobilePhone = "", string token = "")
        {
            ViewBag.Type = t;
            ViewBag.mobilePhone = mobilePhone;
            ViewBag.token = token;
            return View();
        }
        /// <summary>
        /// 第三步，设置密码
        /// </summary>
        /// <param name="t">0注册，1找回密码</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Register3(int t = 0, string mobilePhone = "", string token = "")
        {
            ViewBag.Type = t;
            ViewBag.mobilePhone = mobilePhone;
            ViewBag.token = token;
            return View();
        }
        /// <summary>
        /// url: '/Account/Register',
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult RegisterSave(string jsonModel, string token = "")
        {
            try
            {
                if (string.IsNullOrEmpty(jsonModel))
                {
                    return Json(new { Type = "0", Content = "请填写完整后提交数据" }, JsonRequestBehavior.AllowGet);
                }
                RegisterModel model = JsonHelper.ToObject<RegisterModel>(jsonModel);
                var msg = "";
                if (ModelState.IsValid == false) //验证失败的信息
                {
                    var errorMessage = ModelState.FirstOrDefault(status => status.Value.Errors.Count > 0).Value.Errors[0].ErrorMessage;
                    return Json(new { Type = "0", Content = errorMessage }, JsonRequestBehavior.AllowGet);
                }
                #region 基本参数验证

                //手机号码model
                if (!model.MobileCode.IsMobilePhoneNum(model.RegionCode == "86" ? true : false))
                {
                    msg = "手机号码格式不正确";
                    return Json(new { Type = "0", Content = msg }, JsonRequestBehavior.AllowGet);
                }

                //手机号码session
                var mobile = Session["Mobile0"];
                if (mobile == null || mobile.ToString() != model.MobileCode)
                {
                    msg = "校验码错误";
                    return Json(new { Type = "0", Content = msg }, JsonRequestBehavior.AllowGet);
                }

                //验证码
                var code = Session[ConfigHelper.RandomCode + "0"];
                if (code == null || code.ToString().ToLower() != model.ValidCode.ToLower())
                {
                    msg = "校验码错误";
                    return Json(new { Type = "0", Content = msg }, JsonRequestBehavior.AllowGet);
                }

                //验证码过期
                var createTime = Session[ConfigHelper.RandomCodeCreateTime + "0"];
                var timeSpan = DateTime.Now - DateTime.Parse(createTime.ToString());
                if (timeSpan.TotalMinutes > 10)
                {
                    ClearSession(0);
                    msg = "校验码错误";
                    return Json(new { Type = "0", Content = msg }, JsonRequestBehavior.AllowGet);
                }

                //邮箱验证
                if (ObjectExtension.IsValidEmail(model.Email))
                {
                    msg = "电子邮箱格式不正确";
                    return Json(new { Type = "0", Content = msg }, JsonRequestBehavior.AllowGet);
                }

                //密码pass 确认密码
                if (model.Password != model.ConfirmPassword || string.IsNullOrEmpty(model.Password))
                {
                    msg = "两次密码输入不一致";
                    return Json(new { Type = "0", Content = msg }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                bool isExists = BLL.IsExistsUserName(model.MobileCode, model.RegionCode);

                if (isExists)
                {
                    msg = "手机号不可以重复注册";
                    return Json(new { Type = "0", Content = msg }, JsonRequestBehavior.AllowGet);
                }
                string SourceValue = "";
                int SourceType = 0;
                int ChannelId = 0;
                if (!string.IsNullOrEmpty((string)Session["StationSource"]))
                {
                    SourceValue = (string)Session["StationSource"];
                }
                if (!string.IsNullOrEmpty((string)Session["ChannelId"]))
                {
                    ChannelId = Convert.ToInt32((string)Session["ChannelId"]);
                    if (sourceBll.GetSourcePercentById(ChannelId) != null)
                    {
                        SourceType = sourceBll.GetSourcePercentById(ChannelId).OrderSourceType;
                    }
                }
                CustomerEntity entity = new CustomerEntity()

                {
                    SourceValue = SourceValue,
                    SourceType = SourceType,
                    Mobile = model.MobileCode,
                    UserName = model.MobileCode,
                    Email = model.Email,
                    Password = model.Password,
                    Gender = 0,
                    RegionCode = model.RegionCode,
                    ChannelId = ChannelId
                };
                int UserId = BLL.Insert(entity);
                bool result = BLL.InsertHuoli(entity, UserId, 0, 0);
                if (UserId > 0 && result)
                {
                    ClearSession(0);
                    //注册完成自动登陆
                    var loginInfo = BLL.GetUserByPassword(model.MobileCode, model.Password, model.RegionCode);
                    LoginHelper.SetLoginUserSession(loginInfo.AsLoginUserModel());//Session记录登录状态
                    //登录后返回BackUrl
                    string backUrl = Session["backUrl"] != null ? Session["backUrl"].ToString() : string.Empty;
                    if (backUrl.ToLower().Contains("/account/login"))
                    {
                        backUrl = string.Empty;
                    }
                    Session["backUrl"] = null;

                    InformationEntity InformationEntity = new InformationEntity();
                    InformationEntity.InfoType = 1;
                    InformationEntity.WebInnerType = 4;
                    InformationEntity.SendDest = CommonBll.GetUserRegion(UserId);
                    InformationEntity.SendUserId = UserId;
                    InformationEntity.TradeCode = null;
                    InformationEntity.Title = InformationUtils.UserRegisterSuccTitle;
                    InformationEntity.InfoContent = InformationUtils.UserRegisterSuccContent;
                    InformationEntity.ImagePath = null;
                    InformationEntity.Summary = null;
                    InformationEntity.LinkUrl = null;
                    InformationEntity.StartTime = null;
                    InformationEntity.EndTime = null;
                    InformationEntity.LongTerm = 0;
                    InformationEntity.CreateTime = DateTime.Now;

                    InformationBll.AddInformation(InformationEntity);

                    // 用户注册成功保存用户可见的活动消息数据
                    int resultFlag = InformationBll.SaveActivityInfoForRegister(UserId, loginInfo.CreateTime);
                    LogHelper.Info("----------Register---------注册完成后，批量插入活动消息后结果值：" + resultFlag);

                    // 根据用户类型发送优惠券逻辑
                    List<GiftCardEntity> SendGiftCardList = BLL.SendGiftCardByUser(loginInfo);
                    if (SendGiftCardList.Count() != 0)
                    {
                        return Json(new
                        {
                            Type = "1",
                            Content = msg,
                            LinkUrl = backUrl
                            ,
                            ReturnGift = "1",
                            GiftNum = SendGiftCardList.Count(),
                            GiftAmount = SendGiftCardList.Sum(n => n.CardSum)
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Type = "1",
                            Content = msg,
                            LinkUrl = backUrl
                            ,
                            ReturnGift = "0",
                            GiftNum = 0,
                            GiftAmount = 0
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Type = "0", Content = "注册失败，请稍后重试！" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Type = "0", Content = "注册失败，请稍后重试！" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 验证码
        /// url: '/Account/RegistGeneratorCode',
        /// url: '/Account/RegistGeneratorCode?mobile=' + mobile,
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="regionCode">手机区域类型：86，大陆手机号；852，中华人民共和国大陆地区手机号；</param>
        /// <param name="source">0注册，1找回密码</param>
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult RegistGeneratorCode(string mobile, int source, string regionCode = "86", string mobilePhone = "", string token = "")
        {
            //TODO:严格验证手机号码4
            if (!mobile.IsMobilePhoneNum(regionCode == "86" ? true : false))
            {
                return Json(new { Type = 0, Content = "手机号码格式不正确" }, JsonRequestBehavior.AllowGet);
            }
            bool result = BLL.IsExistsUserName(mobile, regionCode);
            if (result)
            {
                if (source == 0)
                {
                    return Json(new { Type = 0, Content = "手机号已存在，不可重复注册" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int sendStatus = GeneratorCode(regionCode, mobile, source);
                    if (sendStatus == 0)
                    {
                        return Json(new { Type = sendStatus, Content = "暂时无法获取验证码，请稍后重试！" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Type = sendStatus, LinkUrl = "/Account/Register2?t=" + source + "&mobile=" + mobile + "&regionCode=" + regionCode + "&mobilePhone=" + mobilePhone + "&token=" + token }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (source == 0)
                {
                    int sendStatus = GeneratorCode(regionCode, mobile, source);
                    return Json(new { Type = sendStatus, LinkUrl = "/Account/Register2?t=" + source + "&mobile=" + mobile + "&regionCode=" + regionCode + "&mobilePhone=" + mobilePhone + "&token=" + token }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Type = 0, Content = "手机号不存在" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        private int GeneratorCode(string regionCode, string mobile, int source)
        {
            //TODO:严格验证手机号码5
            if (!mobile.IsMobilePhoneNum(regionCode == "86" ? true : false))
            {
                return 0;
            }
            // 手机验证码只有数字
            var code = StringHelper.CreateVerifyCode(6, true);
            try
            {
                Session["RegionCode" + source.ToString()] = regionCode;
                Session["Mobile" + source.ToString()] = mobile;
                Session[ConfigHelper.RandomCode + source.ToString()] = ConfigHelper.RealVerifyCode ? code : "123456";
                Session[ConfigHelper.RandomCodeCreateTime + source.ToString()] = DateTime.Now;
                if (ConfigHelper.RealVerifyCode)
                {
                    //string Content = string.Format(SMSRegistTip, code);
                    //if (source == 1)
                    //{
                    //    Content = string.Format(SMSFindPasswordTip, code);
                    //}
                    //List<string> list = new List<string>();
                    //list.Add(mobile);
                    string defaultSMSReceiver = System.Configuration.ConfigurationManager.AppSettings["DefaultSMSReceiver"];
                    if (defaultSMSReceiver == "1")
                    {
                        //if (messageBll.SendSms(regionCode, Content, list, 0, 0, 0, 0))
                        //{
                        //    return 1;
                        //}
                        //else
                        //{
                        //    return 0;
                        //}

                        SMSCodeRequest model = new SMSCodeRequest() { Code = code, Phone = mobile };
                        if (messageBll.SendSmsNew(model))
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0;
            }
        }
        /// <summary>
        /// 验证校验码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="validcode"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult CheckValidateCode(string mobile, string validcode, int source, string regionCode = "86", string mobilePhone = "", string token = "")
        {
            try
            {
                if (string.IsNullOrEmpty(mobile))
                {
                    return Json(new { Type = 0, Content = "手机号为空" });
                }
                if (string.IsNullOrEmpty(validcode))
                {
                    return Json(new { Type = 0, Content = "校验码为空" });
                }
                var smobile = Session["Mobile" + source.ToString()];
                if (smobile == null || smobile.ToString() != mobile)
                {
                    ClearSession(source);
                    return Json(new { Type = "0", Content = "校验码错误" }, JsonRequestBehavior.AllowGet);
                }
                var code = Session[ConfigHelper.RandomCode + source.ToString()];
                if (code == null || code.ToString().ToLower() != validcode.ToLower())
                {
                    return Json(new { Type = "0", Content = "校验码错误" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                ClearSession(source);
                return Json(new { Type = "0", Content = "校验码错误" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Type = "1", Content = "", LinkUrl = "/Account/Register3?t=" + source + "&mobile=" + mobile + "&regionCode=" + regionCode + "&validCode=" + validcode + "&mobilePhone=" + mobilePhone + "&token=" + token }, JsonRequestBehavior.AllowGet);
        }
        private void ClearSession(int source)
        {
            Session["RegionCode" + source.ToString()] = null;
            Session["Mobile" + source.ToString()] = null;
            Session[ConfigHelper.RandomCode + source.ToString()] = null;
            Session[ConfigHelper.RandomCodeCreateTime + source.ToString()] = null;
        }

        #endregion

        #region 找回密码

        [AllowAnonymous]
        public JsonResult FindPassword(string jsonModel)
        {
            try
            {
                if (string.IsNullOrEmpty(jsonModel))
                {
                    return Json(new { Type = "0", Content = "请填写完整后提交数据" }, JsonRequestBehavior.AllowGet);
                }
                RegisterModel model = JsonHelper.ToObject<RegisterModel>(jsonModel);
                var msg = "";
                if (ModelState.IsValid == false) //验证失败的信息
                {
                    var errorMessage = ModelState.FirstOrDefault(status => status.Value.Errors.Count > 0).Value.Errors[0].ErrorMessage;
                    return Json(new { Type = "0", Content = errorMessage }, JsonRequestBehavior.AllowGet);
                }
                #region 基本参数验证

                //手机号码model
                if (!model.MobileCode.IsMobilePhoneNum(model.RegionCode == "86" ? true : false))
                {
                    msg = "手机号码格式不正确";
                    return Json(new { Type = "0", Content = msg }, JsonRequestBehavior.AllowGet);
                }

                //手机号码session
                var mobile = Session["Mobile1"];
                if (mobile == null || mobile.ToString() != model.MobileCode)
                {
                    msg = "校验码错误";
                    return Json(new { Type = "0", Content = msg }, JsonRequestBehavior.AllowGet);
                }

                //验证码
                var code = Session[ConfigHelper.RandomCode + "1"];
                if (code == null || code.ToString().ToLower() != model.ValidCode.ToLower())
                {
                    msg = "校验码错误";
                    return Json(new { Type = "0", Content = msg }, JsonRequestBehavior.AllowGet);
                }

                //验证码过期
                var createTime = Session[ConfigHelper.RandomCodeCreateTime + "1"];
                var timeSpan = DateTime.Now - DateTime.Parse(createTime.ToString());
                if (timeSpan.TotalMinutes > 10)
                {
                    ClearSession(1);
                    msg = "校验码错误";
                    return Json(new { Type = "0", Content = msg }, JsonRequestBehavior.AllowGet);
                }
                //密码pass 确认密码
                if (model.Password != model.ConfirmPassword || string.IsNullOrEmpty(model.Password))
                {
                    msg = "两次密码输入不一致";
                    return Json(new { Type = "0", Content = msg }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                bool isExists = BLL.IsExistsUserName(model.MobileCode, model.RegionCode);

                if (!isExists)
                {
                    msg = "不存在此用户";
                    return Json(new { Type = "0", Content = msg }, JsonRequestBehavior.AllowGet);
                }
                CustomerEntity entity = new CustomerEntity()
                {
                    Mobile = model.MobileCode,
                    UserName = model.MobileCode,
                    Password = model.Password
                };

                bool result = BLL.UpdatePassword(entity);

                if (result)
                {
                    ClearSession(1);
                    CustomerEntity UserInfo = BLL.GetUserInfoByUserName(model.MobileCode);

                    InformationEntity InformationEntity = new InformationEntity();
                    InformationEntity.InfoType = 1;
                    InformationEntity.WebInnerType = 4;
                    InformationEntity.SendDest = CommonBll.GetUserRegion(UserInfo.ID);
                    InformationEntity.SendUserId = UserInfo.ID;
                    InformationEntity.TradeCode = null;
                    InformationEntity.Title = InformationUtils.UserFindPasswordSuccTitle;
                    InformationEntity.InfoContent = InformationUtils.UserFindPasswordSuccContent;
                    InformationEntity.ImagePath = null;
                    InformationEntity.Summary = null;
                    InformationEntity.LinkUrl = null;
                    InformationEntity.StartTime = null;
                    InformationEntity.EndTime = null;
                    InformationEntity.LongTerm = 0;
                    InformationEntity.CreateTime = DateTime.Now;

                    InformationBll.AddInformation(InformationEntity);

                    //修改密码后跳转到登录页，重新登录
                    return Json(new { Type = "1", Content = msg, LinkUrl = "/Account/Login" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Type = "0", Content = "密码设置失败，请稍后重试！" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Type = "0", Content = "密码设置失败，请稍后重试！" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 用户登录

        /// <summary>
        /// 进入登录页
        /// </summary> 
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model)
        {
            try
            {

                if (model == null || model.UserName.IsNullOrEmpty() || model.Password.IsNullOrEmpty())
                {
                    return this.HandleError("用户名或密码不能为空");
                }
                if (model.RegionCode.IsNullOrEmpty())
                {
                    return this.HandleError("请选择登陆区域！");
                }
                //TODO:严格验证手机号码1

                if (!model.UserName.IsMobilePhoneNum())
                {
                    return this.HandleError("手机号码格式错误");
                }

                //bool isExists = BLL.IsExistsUserName(model.UserName);
                //if (isExists)
                //{

                //}
                //后台验证
                var loginUser = BLL.GetUserByPassword(model.UserName, model.Password, model.RegionCode);

                if (loginUser == null)
                {
                    return this.HandleError("用户名或密码错误，请重新输入！");
                }
                if (loginUser.Status == 0)
                {
                    return this.HandleError("您的账户已被禁用！请联系客服！");
                }
                //Session记录登录状态
                LoginHelper.SetLoginUserSession(loginUser.AsLoginUserModel());

                if (model.Cart == 1)
                {
                    int num = new ShoppingBll().GetMiniShoppingCart(base.LoginUser.UserID, base.language, base.DeliveryRegion);
                    return this.HandleSuccess("登陆成功！", num);
                }
                return this.HandleSuccess("登陆成功！");
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }

        }
        /// <summary>
        /// 商城自动登录
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ShopLogin(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return this.HandleError("用户名不能为空");
            }
            var userInfo = BLL.GetUserInfoByUserName(userName);

            if (userInfo!=null)
            {
                LoginHelper.SetLoginUserSession(userInfo.AsLoginUserModel());
                return Redirect("/home/index");
            }
            else
            {
                return this.HandleError("用户不存在");
            }

        }


        #endregion

        #region 注销退出
        /// <summary>
        /// 注销退出
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SignOut()
        {
            try
            {
                LoginHelper.RemoveLoginSession();
                return Redirect(DomainHelper.MUrl);//此处退出都返回首页
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Redirect(DomainHelper.MUrl);//此处退出都返回首页
            }
        }

        #endregion

        #region 首单授权

        [Login]
        public ActionResult Anuthorization()
        {
            return View();
        }

        [Login]
        public JsonResult OrderAnuthorization()
        {
            try
            {
                BLL.UpdateFirstOrderAuthorize(this.LoginUser.UserName);
            }
            catch (Exception ex)
            {
            }
            return Json(new { Type = 1, Content = "", LinkUrl = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult toIndex(string StationSource, int ChannelId = 0)
        {
            if (!string.IsNullOrEmpty(StationSource) && ChannelId != 0)
            {
                bool update = BLL.UpdateVisitedTimes(StationSource, ChannelId);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
