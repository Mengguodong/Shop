using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.GiftCard
{
    [Serializable]
    [DataContract]
    public class GiftCardEntity
    {
        /// <summary>
        /// 优惠券 自增ID，主键
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int ID { get; set; }

        /// <summary>
        /// 优惠券表关联ID
        /// </summary>
        [DataMember(Name = "BatchId")]
        [Display(Name = "BatchId")]
        public int BatchId { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        [DataMember(Name = "CardId")]
        [Display(Name = "CardId")]
        public string CardId { get; set; }

        /// <summary>
        /// 关联的订单号
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

        /// <summary>
        /// 优惠券名称
        /// </summary>
        [DataMember(Name = "BatchName")]
        [Display(Name = "BatchName")]
        public string BatchName { get; set; }

        /// <summary>
        /// 优惠券面值
        /// </summary>
        [DataMember(Name = "CardSum")]
        [Display(Name = "CardSum")]
        public decimal CardSum { get; set; }

        /// <summary>
        /// 优惠券已使用（以备 以后拓展为可充值的消费券）
        /// </summary>
        [DataMember(Name = "UsedSum")]
        [Display(Name = "UsedSum")]
        public decimal UsedSum { get; set; }

        /// <summary>
        /// 优惠券类型 e.g:满减
        /// </summary>
        [DataMember(Name = "CardType")]
        [Display(Name = "CardType")]
        public int CardType { get; set; }

        /// <summary>
        /// 优惠券状态 e.g 未使用、已使用、已锁定、【已过期】
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// 关联的用户ID
        /// </summary>
        [DataMember(Name = "UserId")]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        /// <summary>
        ///优惠券有效 开始时间
        /// </summary>
        [DataMember(Name = "BeginTime")]
        [Display(Name = "BeginTime")]
        public DateTime BeginTime { get; set; }

        /// <summary>
        ///优惠券有效 结束时间
        /// </summary>
        [DataMember(Name = "EndTime")]
        [Display(Name = "EndTime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        ///优惠券有效 开始时间
        /// </summary>
        [DataMember(Name = "BeginTimeToString")]
        [Display(Name = "BeginTimeToString")]
        public string BeginTimeToString { get; set; }

        /// <summary>
        ///优惠券有效 结束时间
        /// </summary>
        [DataMember(Name = "EndTimeToString")]
        [Display(Name = "EndTimeToString")]
        public string EndTimeToString { get; set; }

        /// <summary>
        ///用途
        /// </summary>
        [DataMember(Name = "Remarks")]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        /// <summary>
        /// 获得优惠券  发放（获取）时间 
        /// </summary>
        [DataMember(Name = "AddTime")]
        [Display(Name = "AddTime")]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 冻结时间
        /// </summary>
        [DataMember(Name = "FrozenTime")]
        [Display(Name = "FrozenTime")]
        public DateTime FrozenTime { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        [DataMember(Name = "UsedTime")]
        [Display(Name = "UsedTime")]
        public DateTime UsedTime { get; set; }

        /// <summary>
        /// 使用条件金额 e.g 满 100 减 20中的   100
        /// </summary>
        [DataMember(Name = "SatisfyPrice")]
        [Display(Name = "SatisfyPrice")]
        public decimal SatisfyPrice { get; set; }

        /// <summary>
        /// 适用商品类型 e.g :原价商品、促销商品、拼生活商品
        /// 枚举类型？进行枚举运算
        /// </summary>
        [DataMember(Name = "SatisfyProduct")]
        [Display(Name = "SatisfyProduct")]
        public int SatisfyProduct { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        [DataMember(Name = "TotalRecord")]
        [Display(Name = "TotalRecord")]
        public int TotalRecord { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<GiftCardEntity> _schema;
        static GiftCardEntity()
        {
            _schema = new ObjectSchema<GiftCardEntity>();

            _schema.AddField(x => x.ID, "Id");

            _schema.AddField(x => x.BatchId, "BatchId");

            _schema.AddField(x => x.CardId, "CardId");

            _schema.AddField(x => x.CardSum, "CardSum");

            _schema.AddField(x => x.CardType, "CardType");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.UserId, "UserId");

            _schema.AddField(x => x.BeginTime, "BeginTime");

            _schema.AddField(x => x.EndTime, "EndTime");

            _schema.AddField(x => x.SatisfyPrice, "SatisfyPrice");

            _schema.AddField(x => x.SatisfyProduct, "SatisfyProduct");

            _schema.AddField(x => x.AddTime, "AddTime");

            _schema.AddField(x => x.FrozenTime, "FrozenTime");

            _schema.AddField(x => x.UsedTime, "UsedTime");

            _schema.AddField(x => x.BatchName, "BatchName");

            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.TotalRecord, "TotalRecord");

            _schema.AddField(x => x.BeginTimeToString, "BeginTimeToString");

            _schema.AddField(x => x.EndTimeToString, "EndTimeToString");

            _schema.AddField(x => x.Remarks, "Remarks");


            _schema.Compile();
        }
    }
}
