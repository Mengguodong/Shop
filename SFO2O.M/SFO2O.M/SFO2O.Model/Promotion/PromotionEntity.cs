using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Model.Promotion
{
    /// <summary>
    /// PromotionEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class PromotionEntity
    {

        /// <summary>
        /// PromotionId
        /// </summary>
        [DataMember(Name = "PromotionId")]
        [Display(Name = "PromotionId")]
        public int PromotionId { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        [DataMember(Name = "Sku")]
        [Display(Name = "Sku")]
        public string Sku { get; set; }

        /// <summary>
        /// Spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// DiscountRate
        /// </summary>
        [DataMember(Name = "DiscountRate")]
        [Display(Name = "DiscountRate")]
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// DiscountPrice
        /// </summary>
        [DataMember(Name = "DiscountPrice")]
        [Display(Name = "DiscountPrice")]
        public decimal DiscountPrice { get; set; }

        /// <summary>
        /// SupplierId
        /// </summary>
        [DataMember(Name = "SupplierId")]
        [Display(Name = "SupplierId")]
        public int SupplierId { get; set; }

        /// <summary>
        /// PromotionName
        /// </summary>
        [DataMember(Name = "PromotionName")]
        [Display(Name = "PromotionName")]
        public string PromotionName { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        [DataMember(Name = "StartTime")]
        [Display(Name = "StartTime")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        [DataMember(Name = "EndTime")]
        [Display(Name = "EndTime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// PromotionLable
        /// </summary>
        [DataMember(Name = "PromotionLable")]
        [Display(Name = "PromotionLable")]
        public string PromotionLable { get; set; }

        /// <summary>
        /// PromotionCost
        /// </summary>
        [DataMember(Name = "PromotionCost")]
        [Display(Name = "PromotionCost")]
        public int PromotionCost { get; set; }

        /// <summary>
        /// PromotionStatus
        /// </summary>
        [DataMember(Name = "PromotionStatus")]
        [Display(Name = "PromotionStatus")]
        public int PromotionStatus { get; set; }

        /// <summary>
        /// PromotionType
        /// </summary>
        [DataMember(Name = "PromotionType")]
        [Display(Name = "PromotionType")]
        public int PromotionType { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        [DataMember(Name = "CreateBy")]
        [Display(Name = "CreateBy")]
        public string CreateBy { get; set; }

        [DataMember(Name = "TuanNumbers")]
        [Display(Name = "TuanNumbers")]
        public int TuanNumbers { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<PromotionEntity> _schema;
        static PromotionEntity()
        {
            _schema = new ObjectSchema<PromotionEntity>();
            _schema.AddField(x => x.PromotionId, "PromotionId");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.DiscountRate, "DiscountRate");

            _schema.AddField(x => x.DiscountPrice, "DiscountPrice");

            _schema.AddField(x => x.SupplierId, "SupplierId");

            _schema.AddField(x => x.PromotionName, "PromotionName");

            _schema.AddField(x => x.StartTime, "StartTime");

            _schema.AddField(x => x.EndTime, "EndTime");

            _schema.AddField(x => x.PromotionLable, "PromotionLable");

            _schema.AddField(x => x.PromotionCost, "PromotionCost");

            _schema.AddField(x => x.PromotionStatus, "PromotionStatus");

            _schema.AddField(x => x.PromotionType, "PromotionType");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.CreateBy, "CreateBy");
            _schema.AddField(x => x.TuanNumbers, "TuanNumbers");
            _schema.Compile();
        }
    }
}