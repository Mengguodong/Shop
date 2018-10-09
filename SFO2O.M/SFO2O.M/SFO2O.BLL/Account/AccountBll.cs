using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.DAL.Account;
using SFO2O.Model.Account;
using SFO2O.Utility.Security;
using SFO2O.Utility.Uitl;
using SFO2O.References.SFo2oWCF;
using SFO2O.Model.Huoli;
using SFO2O.BLL.GiftCard;
using SFO2O.Model.GiftCard;

namespace SFO2O.BLL.Account
{
    public class AccountBll
    {
        public static readonly AccountDal Dal = new AccountDal();
        public static readonly GiftCardBll GiftCardBll = new GiftCardBll();

        private static GetSFDataClient sfWcfClient = new GetSFDataClient();

        /// <summary>
        /// 根据用户名密码获取用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password">明文</param> 
        /// <param name="regionCode"></param>
        /// <returns></returns>
        public CustomerEntity GetUserByPassword(string username, string password, string regionCode)
        {
            password = MD5Hash.Md5Encrypt(password);//加密
            var model = Dal.GetCustomerEntity(username, password, regionCode);
            return model;
        }

        /// <summary>
        /// 用户名是否已经存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsExistsUserName(string userName, string regionCode)
        {
            return Dal.IsExistsUserName(userName, regionCode);
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CustomerEntity GetUserById(int userId)
        {
            return Dal.GetCustomerEntity(userId);
        }
        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(CustomerEntity entity)
        {
            return Dal.Insert(entity);
        }
        /// <summary>
        /// 添加新用户酒豆
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertHuoli(CustomerEntity entity, int UserId, decimal huoli, decimal lockHuoli)
        {
            return Dal.InsertHuoli(entity, UserId, huoli, lockHuoli);
        }
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdatePassword(CustomerEntity entity)
        {
            return Dal.UpdatePassword(entity);
        }

        /// <summary>
        /// 更新是否首次下单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateFirstOrderAuthorize(string userName)
        {
            return Dal.UpdateFirstOrderAuthorize(userName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsFirstOrderAuthorize(string userName)
        {
            return Dal.IsFirstOrderAuthorize(userName);
        }
        /// <summary>
        /// 查询用户证件图片信息
        /// </summary>
        /// <param name="userName">收货人姓名</param>
        /// <param name="phone">联系电话</param>
        /// <returns>0：存在， 1：不存在或未审核， 2：存在但已过期</returns>
        public int CheckUserIndentity(string userName, string phone)
        {
            int result = 0;
            try
            {
                result = sfWcfClient.QueryIndentity(userName, phone);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        public HuoliEntity GetHuoliEntityByUerId(int UserId)
        {
            return Dal.GetHuoliEntityByUerId(UserId);
        }

        public CustomerEntity GetUserInfoByUserName(string UserName)
        {
            return Dal.GetUserInfoByUserName(UserName);
        }
        public bool UpdateVisitedTimes(string StationSource, int ChannelId)
        {
            //查询DividedPercentStationVisitedLog 表中是否有记录
            int count = Dal.selectVisitedLog(StationSource, ChannelId);
            //查询DividedPercentStation 表中是否有此记录
            bool isExist = Dal.GetDividedPercentStationZDID(ChannelId, StationSource);
            //增加isExist判断逻辑
            if (count == 1 && isExist)
            {
                //更新DividedPercentStationVisitedLog 的记录
                Dal.UpdateVisitedLog(StationSource, ChannelId);
            }
            else if (count == 0 && isExist)
            {
                //插入DividedPercentStationVisitedLog记录
                Dal.InsertVisitedLog(StationSource, ChannelId);
                return UpdateVisitedTimes(StationSource, ChannelId);
            }
            return Dal.UpdateVisitedTimes(StationSource, ChannelId);
        }

        /// <summary>
        /// 注册成功返优惠券
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<GiftCardEntity> SendGiftCard(CustomerEntity entity)
        {
            // 注册成功后，获得优惠券批次信息
            List<GiftCardBatchEntity> GiftCardBatchEntityList = GiftCardBll.GetGiftCardBatchForRegisterSucc(entity.CreateTime);

            // 已经发送的优惠券集合
            List<GiftCardEntity> SendGiftCardList = new List<GiftCardEntity>();

            if (GiftCardBatchEntityList != null && GiftCardBatchEntityList.Count != 0)
            {
                foreach (var GiftCardBatchEntity in GiftCardBatchEntityList)
                {
                    GiftCardEntity GiftCardEntity = null;
                    // 普通用户注册
                    if (GiftCardBatchEntity.SatisfyUser == 0)
                    {
                        // 添加优惠券
                        GiftCardEntity = AddGiftCard(GiftCardBatchEntity, entity);
                    }
                    else
                    {
                        //判断注册用户是不是嘿客用户
                        if (entity.ChannelId != 0)
                        {
                            GiftCardEntity = ChooseUserToAddGiftCard(GiftCardBatchEntity, entity);
                        }
                    }
                    if (GiftCardEntity != null)
                    {
                        SendGiftCardList.Add(GiftCardEntity);
                    }
                }
            }

            return SendGiftCardList;
        }

        /// <summary>
        /// 添加优惠券
        /// </summary>
        /// <param name="GiftCardBatchEntity"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public GiftCardEntity AddGiftCard(GiftCardBatchEntity GiftCardBatchEntity, CustomerEntity entity)
        {
            // 设置优惠券属性
            GiftCardEntity GiftCardEntity = GiftCardBll.setGiftCardEntity(GiftCardBatchEntity, entity);

            // 插入优惠券信息
            bool result = GiftCardBll.SaveGiftCard(GiftCardEntity);
            if (result)
            {
                return GiftCardEntity;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 其他用户类型添加优惠券
        /// </summary>
        /// <param name="GiftCardBatchEntity"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public GiftCardEntity ChooseUserToAddGiftCard(GiftCardBatchEntity GiftCardBatchEntity, CustomerEntity entity)
        {
            GiftCardEntity GiftCardEntity = null;

            // 根据具体店铺id查询是否有其店铺存在
            bool IsResult = GetDividedPercentStationZDID(entity.ChannelId, entity.SourceValue);

            // 嘿客店用户
            if (entity.ChannelId.ToString()
                        .Equals(System.Web.Configuration.WebConfigurationManager.AppSettings["HeiKeChannelId"].ToString())
                            && IsResult)
            {
                // 添加优惠券
                GiftCardEntity = AddGiftCard(GiftCardBatchEntity, entity);
            }

            return GiftCardEntity;
        }

        /// <summary>
        /// 根据用户类型发送优惠券逻辑
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<GiftCardEntity> SendGiftCardByUser(CustomerEntity entity)
        {
            // 注册成功返优惠券
            List<GiftCardEntity> SendGiftCardList = SendGiftCard(entity);
            return SendGiftCardList;
        }

        /// <summary>
        /// 根据具体店铺id查询是否有其店铺存在
        /// </summary>
        /// <param name="DPID"></param>
        /// <param name="ZDID"></param>
        /// <returns></returns>
        public bool GetDividedPercentStationZDID(int DPID, string ZDID)
        {
            bool IsResult = Dal.GetDividedPercentStationZDID(DPID, ZDID);
            return IsResult;
        }
        /// <summary>
        /// 插入用户临时表（为了临时手动导入一些优惠券用的）
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="pwd"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        public int InsertTemp(string UserName, string pwd, int UserType)
        {
            return Dal.InsertTemp(UserName, pwd, UserType);
        }

        /// <summary>
        /// 获取用户临时表信息（为了临时手动导入一些优惠券用的）
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public int GetUserTempByUserName(string UserName)
        {
            return Dal.GetUserTempByUserName(UserName);
        }

        /// <summary>
        /// 获取用户信息（为了临时手动导入一些优惠券用的）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<CustomerEntity> GetUserInfo()
        {
            return Dal.GetUserInfo();
        }

        /// <summary>
        /// 获取用户信息是否为空
        /// </summary>
        /// <param name="UnionId"></param>
        /// <returns></returns>
        public bool GetUserInfoIsNull(string UnionId)
        {
            // 根据用户UnionId获取用户信息
            CustomerEntity userInfo = Dal.GetCustomerEntityByUnionId(UnionId);

            if(userInfo != null){
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 创建物流速递用户信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public CustomerEntity CreateSFExpressUser(CustomerEntity entity)
        {
            return Dal.InsertSFExpressCustomer(entity);
        }

        /// <summary>
        /// 根据用户UnionId获取用户信息
        /// </summary>
        /// <param name="UnionId"></param>
        /// <returns></returns>
        public CustomerEntity GetSFExpressUser(string UnionId)
        {
            return Dal.GetCustomerEntityByUnionId(UnionId);
        }

    }
}
