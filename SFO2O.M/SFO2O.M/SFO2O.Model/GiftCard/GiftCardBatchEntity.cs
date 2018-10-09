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
    public class GiftCardBatchEntity
    {
        /// <summary>
        /// 优惠券批次编号ID
        /// </summary>
        [DataMember(Name = "BatchId")]
        [Display(Name = "BatchId")]
        public int BatchId { get; set; }

        /// <summary>
        /// 优惠券批次名称
        /// </summary>
        [DataMember(Name = "BatchName")]
        [Display(Name = "BatchName")]
        public string BatchName { get; set; }

        /// <summary>
        /// 优惠券卡金额
        /// </summary>
        [DataMember(Name = "CardSum")]
        [Display(Name = "CardSum")]
        public decimal CardSum { get; set; }

        /// <summary>
        /// 优惠券数量 扩展
        /// </summary>
        [DataMember(Name = "CardNumber")]
        [Display(Name = "CardNumber")]
        public int CardNumber { get; set; }

        /// <summary>
        /// 优惠券类型 e.g:满减
        /// </summary>
        [DataMember(Name = "CardType")]
        [Display(Name = "CardType")]
        public int CardType { get; set; }

        /// <summary>
        ///优惠券发放开始时间
        /// </summary>
        [DataMember(Name = "BeginTime")]
        [Display(Name = "BeginTime")]
        public DateTime BeginTime { get; set; }

        /// <summary>
        ///优惠券发放结束时间
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
        /// 优惠券持续天数,如果当前值为默认值，则使用startime和endtime为优惠券的有效期时间
        /// </summary>
        [DataMember(Name = "ExpiryDays")]
        [Display(Name = "ExpiryDays")]
        public int ExpiryDays { get; set; }

        /// <summary>
        /// 使用该礼品卡需达到的购物金额，默认为0，表示不限制
        /// </summary>
        [DataMember(Name = "SatisfyPrice")]
        [Display(Name = "SatisfyPrice")]
        public decimal SatisfyPrice { get; set; }

        /// <summary>
        /// 发放用户类型
        /// </summary>
        [DataMember(Name = "SatisfyUser")]
        [Display(Name = "SatisfyUser")]
        public int SatisfyUser { get; set; }

        /// <summary>
        /// 适用产品，枚举运算
        /// 枚举类型？进行枚举运算
        /// </summary>
        [DataMember(Name = "SatisfyProduct")]
        [Display(Name = "SatisfyProduct")]
        public int SatisfyProduct { get; set; }

        /// <summary>
        ///用途
        /// </summary>
        [DataMember(Name = "Remarks")]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        /// <summary>
        /// 优惠券下载次数 扩展
        /// </summary>
        [DataMember(Name = "DownloadCounts")]
        [Display(Name = "DownloadCounts")]
        public int DownloadCounts { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [DataMember(Name = "Enable")]
        [Display(Name = "Enable")]
        public bool Enable { get; set; }

        /// <summary>
        /// 创建时间 
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///创建人
        /// </summary>
        [DataMember(Name = "Creater")]
        [Display(Name = "Creater")]
        public string Creater { get; set; }
        
        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<GiftCardBatchEntity> _schema;
        static GiftCardBatchEntity()
        {
            _schema = new ObjectSchema<GiftCardBatchEntity>();


            _schema.AddField(x => x.BatchId, "BatchId");

            _schema.AddField(x => x.BatchName, "BatchName");

            _schema.AddField(x => x.CardSum, "CardSum");

            _schema.AddField(x => x.CardNumber, "CardNumber");

            _schema.AddField(x => x.CardType, "CardType");

            _schema.AddField(x => x.BeginTime, "BeginTime");

            _schema.AddField(x => x.EndTime, "EndTime");

            _schema.AddField(x => x.ExpiryDays, "ExpiryDays");

            _schema.AddField(x => x.SatisfyPrice, "SatisfyPrice");

            _schema.AddField(x => x.SatisfyUser, "SatisfyUser");

            _schema.AddField(x => x.SatisfyProduct, "SatisfyProduct");

            _schema.AddField(x => x.Remarks, "Remarks");

            _schema.AddField(x => x.DownloadCounts, "DownloadCounts");

            _schema.AddField(x => x.Enable, "Enable");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.Creater, "Creater");

            _schema.AddField(x => x.BeginTimeToString, "BeginTimeToString");

            _schema.AddField(x => x.EndTimeToString, "EndTimeToString");

            _schema.Compile();
        }
    }
}
