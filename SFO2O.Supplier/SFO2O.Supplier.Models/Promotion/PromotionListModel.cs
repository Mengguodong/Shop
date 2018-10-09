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
    /// PromotionListModel
    /// </summary>
    public class PromotionListModel
    {
        /// <summary>
        /// PromotionName
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// PromotionName
        /// </summary>
        [DataMember(Name = "PromotionName")]
        [Display(Name = "PromotionName")]
        public string PromotionName { get; set; }

        /// <summary>
        /// PromotionStatus
        /// </summary>
        [DataMember(Name = "PromotionStatus")]
        [Display(Name = "PromotionStatus")]
        public int PromotionStatus { get; set; }

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
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<PromotionListModel> _schema;
        static PromotionListModel()
        {
            _schema = new ObjectSchema<PromotionListModel>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.PromotionName, "PromotionName");

            _schema.AddField(x => x.PromotionStatus, "PromotionStatus");

            _schema.AddField(x => x.StartTime, "StartTime");

            _schema.AddField(x => x.EndTime, "EndTime");
            _schema.Compile();
        }
    }
}