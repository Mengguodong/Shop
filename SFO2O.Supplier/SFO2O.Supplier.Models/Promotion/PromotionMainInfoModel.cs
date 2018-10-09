using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Supplier.Models.Promotion
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// PromotionMainInfoModel
    /// </summary>
    public class PromotionMainInfoModel
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

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

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<PromotionMainInfoModel> _schema;
        static PromotionMainInfoModel()
        {
            _schema = new ObjectSchema<PromotionMainInfoModel>();
            _schema.AddField(x => x.Id, "Id");

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
            _schema.Compile();
        }
    }
}