using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SFO2O.BLL.Account;
using SFO2O.BLL.Common;
using SFO2O.M.Controllers.Extensions;
using SFO2O.M.ViewModel.Account;
using SFO2O.Utility.Cache;
using SFO2O.Utility.Extensions;
using SFO2O.Utility.Uitl;
using SFO2O.Utility.Security;
using SFO2O.Model.Account;
using System.Web.Security;

namespace SFO2O.M.Controllers.Common
{
    public class LoginHelper : System.Web.HttpSessionStateBase
    {
        // 物流速递用户校验秘钥
        public static readonly string SFExpressKey = "sf-o2o&union";

        /// <summary>
        /// rediskey 模板
        /// </summary>
        private const string RedisKeyTemplate = ConstClass.RedisKey4MPrefix + ConstClass.RedisKeyLoginUser + ":{0}|{1}";

        protected static AccountBll accountBll = new AccountBll();
        public static bool CheckSession(out LoginUserModel userModel)
        {
            // 为物流速递用户创建登录Session
            CreateSFExpressUserSession();

            bool temp = CheckSessionBase(out userModel);

            if (temp == false)
            {
                userModel = null;
            }
            return temp;
        }

        /// <summary>
        /// 设置用户登录后的session
        /// </summary>
        /// <param name="user"></param>
        public static void SetLoginUserSession(LoginUserModel user)
        {
            var session = HttpContext.Current.Session;
            session[ConstClass.SessionKeyMLoginUser] = user.UserID;

            RedisCacheHelper.Add(GetLoginUserRedisKey(user.UserID), user, DateTime.Now.AddMinutes(session.Timeout));

            if (user.Status == 1)
            {
                CookieHelper.SetCookie(ConstClass.LoginUserCookieKey
               , dic: new Dictionary<string, string>{
                    { "username",user.UserName},
                    { "Id",user.UserID.ToString()}
                });
            }
        }


        public static void RefreshSession(LoginUserModel user)
        {
            var session = HttpContext.Current.Session;
            if (user != null)
            {
                if (user.LoginTime.AddMinutes(session.Timeout - 5) < DateTime.Now)
                {

                    RedisCacheHelper.Remove(GetLoginUserRedisKey(user.UserID));
                    session[ConstClass.SessionKeyMLoginUser] = null;
                    session[ConstClass.SessionKeyMLoginUser] = user.UserID;

                    //user.LoginTime = DateTime.Now;

                    RedisCacheHelper.Add(GetLoginUserRedisKey(user.UserID), user, session.Timeout);
                }
            }
        }

        public static void RemoveLoginSession()
        {
            var session = HttpContext.Current.Session;
            if (session[ConstClass.SessionKeyMLoginUser] != null)
            {
                var id = session[ConstClass.SessionKeyMLoginUser].As(0);
                RedisCacheHelper.Remove(GetLoginUserRedisKey(id));
                session[ConstClass.SessionKeyMLoginUser] = null;
                CookieHelper.ClearCookie(ConstClass.LoginUserCookieKey);
            }
        }

        #region 私有方法
        /// <summary>
        /// 检查登陆状态
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        private static bool CheckSessionBase(out LoginUserModel userModel)
        {
            userModel = null;
            if (System.Web.HttpContext.Current.Session != null
                && System.Web.HttpContext.Current.Session[ConstClass.SessionKeyMLoginUser] != null)
            {
                int userId = System.Web.HttpContext.Current.Session[ConstClass.SessionKeyMLoginUser].As(0);

                try
                {
                    if (userId <= 0)
                    {
                        System.Web.HttpContext.Current.Session[ConstClass.SessionKeyMLoginUser] = null;
                        System.Web.HttpContext.Current.Session.Clear();
                        return false;
                    }
                    var tempuser = RedisCacheHelper.AutoCache<LoginUserModel>("", GetLoginUserRedisKey(userId), () => accountBll.GetUserById(userId).AsLoginUserModel(), ConfigHelper.SessionExpireMinutes);
                    if (tempuser == null || tempuser.Status != 1)
                    {
                        //清错误缓存
                        RedisCacheHelper.Remove(GetLoginUserRedisKey(userId));
                        System.Web.HttpContext.Current.Session[ConstClass.SessionKeyMLoginUser] = null;
                        System.Web.HttpContext.Current.Session.Clear();
                        return false;
                    }
                    userModel = tempuser;
                    return true;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                    RedisCacheHelper.Remove(GetLoginUserRedisKey(userId));
                    System.Web.HttpContext.Current.Session[ConstClass.SessionKeyMLoginUser] = null;
                    return false;
                }
            }
          
            return false;
            
        }
        /// <summary>
        /// 获取登录用户的RedisKey
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static string GetLoginUserRedisKey(int userId)
        {
            var sessionId = HttpContext.Current.Session.SessionID;
            return string.Format(RedisKeyTemplate, userId, sessionId);
        }
        #endregion



        internal static string GetLoginUrl()
        {
            return DomainHelper.MUrl + "/account/login";
        }

        /// <summary>
        /// 为物流速递用户创建登录Session
        /// </summary>
        private static void CreateSFExpressUserSession()
        {
            // 物流速递传递过来的参数不为空
            if (System.Web.HttpContext.Current.Request["unionid"] != null
                && System.Web.HttpContext.Current.Request["tokenId"] != null
                && System.Web.HttpContext.Current.Request["sourcetype"] != null)
            {
                if (System.Web.HttpContext.Current.Session == null
                || System.Web.HttpContext.Current.Session[ConstClass.SessionKeyMLoginUser] == null)
                {
                    // 传递参数
                    string UnionId = System.Web.HttpContext.Current.Request["unionid"].ToString();
                    string SFId = System.Web.HttpContext.Current.Request["sfid"] != null ? System.Web.HttpContext.Current.Request["sfid"].ToString() : string.Empty;
                    string TokenId = System.Web.HttpContext.Current.Request["tokenId"].ToString();
                    string Password = TokenId;

                    // 获取SourceType值
                    int SourceType = GetSourceType(System.Web.HttpContext.Current.Request["sourcetype"].ToString());
                    if (SourceType == -1)
                    {
                        return;
                    }

                    // 参数MD5与TokenId比较
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append(UnionId).Append(SourceType).Append(SFExpressKey);
                    string MD5Encoding = FormsAuthentication.HashPasswordForStoringInConfigFile(strBuilder.ToString(), "MD5");
                    //if (!MD5Encoding.Equals(TokenId))
                    //{
                    //    System.Web.HttpContext.Current.Session[ConstClass.SessionKeyMLoginUser] = null;
                    //    System.Web.HttpContext.Current.Session.Clear();
                    //    return;
                    //}

                    CustomerEntity CustomerEntity = new CustomerEntity();
                    CustomerEntity.Password = Password;
                    CustomerEntity.SourceType = SourceType;
                    CustomerEntity.SFId = SFId;
                    CustomerEntity.UnionId = UnionId;

                    // 获取用户信息是否为空
                    bool IsNullFlag = accountBll.GetUserInfoIsNull(UnionId);

                    int ReturnUserId = 1;
                    CustomerEntity ReturnCustomerEntity = null;

                    // 用户信息为空
                    if (IsNullFlag)
                    {
                        // 插入用户信息
                        ReturnCustomerEntity = accountBll.CreateSFExpressUser(CustomerEntity);
                    }
                    else
                    {
                        // 获取用户信息
                        ReturnCustomerEntity = accountBll.GetSFExpressUser(UnionId);
                    }

                    if (ReturnCustomerEntity != null)
                    {
                        ReturnUserId = ReturnCustomerEntity.ID;
                        CustomerEntity.ID = ReturnCustomerEntity.ID;
                        CustomerEntity.UserName = ReturnCustomerEntity.UserName;
                        CustomerEntity.Status = ReturnCustomerEntity.Status;
                    }
                    else
                    {
                        ReturnUserId = 0;
                    }

                    // 给用户登录状态
                    if (ReturnUserId > 0)
                    {
                        //Session记录登录状态
                        LoginHelper.SetLoginUserSession(CustomerEntity.AsLoginUserModel());
                    }
                }
            }
        }

        /// <summary>
        ///  获取SourceType值
        /// </summary>
        /// <param name="SourceType"></param>
        /// <returns></returns>
        private static int GetSourceType(string SourceType)
        {
            try
            {
                int SourceTypeInt = Convert.ToInt32(SourceType);
                return SourceTypeInt;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return -1;
            }
        }
    }
}
