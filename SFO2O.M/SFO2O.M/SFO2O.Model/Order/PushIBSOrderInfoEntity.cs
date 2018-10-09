using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Order
{
    [Serializable]
    [DataContract]
    public class PushIBSOrderInfoEntity
    {
        /// <summary>
        /// OrderCode
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

        /// <summary>
        /// MailNo
        /// </summary>
        [DataMember(Name = "MailNo")]
        [Display(Name = "MailNo")]
        public string MailNo { get; set; }

        /// <summary>
        /// PushStatus
        /// </summary>
        [DataMember(Name = "PushStatus")]
        [Display(Name = "PushStatus")]
        public int PushStatus { get; set; }

        /// <summary>
        /// PushFailCount
        /// </summary>
        [DataMember(Name = "PushFailCount")]
        [Display(Name = "PushFailCount")]
        public int PushFailCount { get; set; }

        /// <summary>
        /// PushSuccTime
        /// </summary>
        [DataMember(Name = "PushSuccTime")]
        [Display(Name = "PushSuccTime")]
        public DateTime? PushSuccTime { get; set; }

        /// <summary>
        /// GateWayCode
        /// </summary>
        [DataMember(Name = "GateWayCode")]
        [Display(Name = "GateWayCode")]
        public int? GateWayCode { get; set; }

        /// <summary>
        /// TaxType
        /// </summary>
        [DataMember(Name = "TaxType")]
        [Display(Name = "TaxType")]
        public int TaxType { get; set; }

        /// <summary>
        /// PayType
        /// </summary>
        [DataMember(Name = "PayType")]
        [Display(Name = "PayType")]
        public int PayType { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<PushIBSOrderInfoEntity> _schema;
        static PushIBSOrderInfoEntity()
        {
            _schema = new ObjectSchema<PushIBSOrderInfoEntity>();
            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.MailNo, "MailNo");

            _schema.AddField(x => x.PushStatus, "PushStatus");

            _schema.AddField(x => x.PushFailCount, "PushFailCount");

            _schema.AddField(x => x.PushSuccTime, "PushSuccTime");

            _schema.AddField(x => x.GateWayCode, "GateWayCode");

            _schema.AddField(x => x.TaxType, "TaxType");

            _schema.AddField(x => x.PayType, "PayType");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.Compile();
        }

    }
}
