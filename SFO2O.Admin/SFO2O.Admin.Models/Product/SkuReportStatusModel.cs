using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Admin.Models.Product
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// SkuReportStatusModel
    /// </summary>
    public class SkuReportStatusModel
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        [DataMember(Name = "Sku")]
        [Display(Name = "Sku")]
        public string Sku { get; set; }

        /// <summary>
        /// ReportStatus
        /// </summary>
        [DataMember(Name = "ReportStatus")]
        [Display(Name = "ReportStatus")]
        public int ReportStatus { get; set; }

        public List<int> ConnectIDs { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SkuReportStatusModel> _schema;
        static SkuReportStatusModel()
        {
            _schema = new ObjectSchema<SkuReportStatusModel>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.ReportStatus, "ReportStatus");
            _schema.Compile();
        }
    }
}