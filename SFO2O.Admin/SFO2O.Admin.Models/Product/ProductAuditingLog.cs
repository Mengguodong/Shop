using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Models
{

    [Serializable]
    [DataContract]
    /// <summary>
    /// ProductAuditingLog
    /// </summary>
    public class ProductAuditingLog
    {
        /// <summary>
        /// id
        /// </summary>
        [DataMember(Name = "id")]
        [Display(Name = "id")]
        public int id { get; set; }

        /// <summary>
        /// Spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        [DataMember(Name = "Reason")]
        [Display(Name = "Reason")]
        public string Reason { get; set; }

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
        private static readonly ObjectSchema<ProductAuditingLog> _schema;
        static ProductAuditingLog()
        {
            _schema = new ObjectSchema<ProductAuditingLog>();
            _schema.AddField(x => x.id, "id");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.Reason, "Reason");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.CreateBy, "CreateBy");
            _schema.Compile();
        }
    }
}
