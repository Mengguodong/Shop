using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.DAL.GiftCard;
using SFO2O.Model.GiftCard;
using SFO2O.M.ViewModel.GiftCard;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Account;

namespace SFO2O.BLL.GiftCard
{
    public class GiftCardBll
    {
        public static readonly GiftCardDal DAL = new GiftCardDal();

        /// <summary>
        /// 获取某人所有优惠券
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public List<GiftCardEntity> GetAllGiftCard(int userId)
        {
            return DAL.GetAllGiftCard(userId);
        }

        #region 获取某人 某状态 优惠券 分页数据
        /// <summary>
        /// 获取某人 某状态 优惠券 分页数据
        /// </summary>
        /// <param name="status">优惠券状态：0未使用，1已过期, 2已使用</param>
        /// <param name="userId">用户ID</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">每页条数</param>
        /// <returns></returns>
        public List<GiftCardEntity> GetGiftCardList(int status, int userId, int pageindex, int pagesize)
        {
            return DAL.GetGiftCardList(status, userId, pageindex, pagesize);
        }
        #endregion

        #region 根据ID获取某个优惠券 面值
        public decimal GetGiftCardValueById(int userId, int cardId)
        {
            return GetGiftCardValueById(userId, cardId);
        }
        #endregion

        #region 获得能够使用的优惠券列表
        /// <summary>
        /// 获得能够使用的优惠券列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="productPrice">商品金额 e.g: 66*3=198</param>
        /// <param name="satisfyProductType"></param>
        /// <returns></returns>
        public List<GiftCardEntity> GetCanUseGiftCardList(int userId, decimal productPrice, int satisfyProductType)
        {
            var list = GetAllNotUsedGiftCard(userId, productPrice, satisfyProductType);
            if (list.Any())
            {
                var cards = list.Where(x => (x.SatisfyProduct & satisfyProductType) == satisfyProductType).OrderByDescending(s => s.CardSum);
                return cards.ToList<GiftCardEntity>();
            }
            return null;
        }
        #endregion

        #region 获取某人所有满额未使用的优惠券
        /// <summary>
        /// 获取某人所有满额未使用的优惠券（满足 某个UserID对应优惠券几个条件：
        /// 第一：未使用
        /// 第二：在有效期内
        /// 第三：大于SatisfyPrice最小满额
        /// 第三：商品类型（SatisfyProduct）大于等于选中的商品类型之和
        /// 调用的地方采取：e.g:sum（枚举运算，取出数据库里的值e.g satisfyProduct然后跟商品（|）的结果进行取&，如果结果依然等于商品和sum）那么这个就是我们想要的数据
        /// if(satisfyProduct&sum==sum)那么就得到一个满足条件的优惠券
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="satisfyPrice">最小满减金额</param>
        /// <param name="satisfyProduct">最小|运算结果值   1|2=3   3&2=2</param>
        /// <returns></returns>
        public List<GiftCardEntity> GetAllNotUsedGiftCard(int userId, decimal satisfyPrice, int satisfyProduct)
        {
            return DAL.GetAllNotUsedGiftCard(userId, satisfyPrice, satisfyProduct);
        }
        #endregion

        //#region 根据触发类型 更改优惠券状态
        ///// <summary>
        ///// 根据触发类型 更改优惠券状态
        ///// </summary>
        ///// <param name="type">1.下单成功事件    2.支付成功事件  3.取消订单事件</param>
        ///// <param name="cid">优惠券ID</param>
        ///// <returns>True操作成功  False操作失败</returns>
        //public bool ChangeGiftCardStatusByEventType(int type, int cid, Database db, DbTransaction tran, string orderCode = null)
        //{
        //    return DAL.ChangeGiftCardStatusByEventType(type, cid, db,tran);
        //}
        //#endregion

        /// <summary>
        /// 注册成功后，获得优惠券批次信息
        /// </summary>
        /// <param name="SatisfyUser"></param>
        /// <param name="RegisterTime"></param>
        /// <returns></returns>
        public List<GiftCardBatchEntity> GetGiftCardBatchForRegisterSucc(DateTime RegisterTime)
        {
            return DAL.GetGiftCardBatchEntityForRegisterSucc(RegisterTime);
        }

        /// <summary>
        /// 插入优惠券信息
        /// </summary>
        /// <param name="GiftCardEntity"></param>
        /// <returns></returns>
        public bool SaveGiftCard(GiftCardEntity GiftCardEntity)
        {
            try
            {
                return DAL.CreateGiftCard(GiftCardEntity);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 设置优惠券属性
        /// </summary>
        /// <param name="GiftCardBatchEntity"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public GiftCardEntity setGiftCardEntity(GiftCardBatchEntity GiftCardBatchEntity, CustomerEntity entity)
        {
            if (GiftCardBatchEntity != null)
            {
                GiftCardEntity GiftCardEntity = new GiftCardEntity();

                GiftCardEntity.BatchId = GiftCardBatchEntity.BatchId;
                GiftCardEntity.BatchName = GiftCardBatchEntity.BatchName;
                GiftCardEntity.UserId = entity.ID;
                GiftCardEntity.CardId = StringHelper.GenerateRandomCode(8);
                GiftCardEntity.CardSum = GiftCardBatchEntity.CardSum;
                GiftCardEntity.CardType = GiftCardBatchEntity.CardType;
                GiftCardEntity.Status = 0;
                GiftCardEntity.BeginTime = entity.CreateTime.Date;
                GiftCardEntity.EndTime = GiftCardEntity.BeginTime.AddDays(GiftCardBatchEntity.ExpiryDays+1).AddSeconds(-1);
                GiftCardEntity.SatisfyPrice = GiftCardBatchEntity.SatisfyPrice;
                GiftCardEntity.SatisfyProduct = GiftCardBatchEntity.SatisfyProduct;
                GiftCardEntity.AddTime = DateTime.Now;

                return GiftCardEntity;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 优惠券实体转化成ViewModel
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public CanUseGiftCardViewModel EntityToViewModel(GiftCardEntity entity)
        {
            if (entity != null)
            {
                CanUseGiftCardViewModel model = new CanUseGiftCardViewModel();
                model.Id = entity.ID;
                model.Name = entity.BatchName;
                model.CardSum = entity.CardSum.ToString("G0");
                model.Type = entity.CardType;
                model.BeginTime = entity.BeginTime.ToString("yyyy.MM.dd");
                model.EndTime = entity.EndTime.ToString("yyyy.MM.dd");
                model.FullCutNum = entity.SatisfyPrice;
                model.Huoli = 0M;
                model.Money = "0";
                return model;
            }
            return null;
        }
    }
}
