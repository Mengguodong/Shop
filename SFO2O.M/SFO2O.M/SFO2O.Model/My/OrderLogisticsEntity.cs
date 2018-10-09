using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Model.My
{
    /// <summary>
    /// OrderLogisticsEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class OrderLogisticsEntity
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// OrderCode
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

        /// <summary>
        /// ExpressCompany
        /// </summary>
        [DataMember(Name = "ExpressCompany")]
        [Display(Name = "ExpressCompany")]
        public string ExpressCompany { get; set; }

        /// <summary>
        /// ExpressList
        /// </summary>
        [DataMember(Name = "ExpressList")]
        [Display(Name = "ExpressList")]
        public string ExpressList { get; set; }

        /// <summary>
        /// ExPressStatus
        /// </summary>
        [DataMember(Name = "ExPressStatus")]
        [Display(Name = "ExPressStatus")]
        public int ExPressStatus { get; set; }

        /// <summary>
        /// LogisticsTime
        /// </summary>
        [DataMember(Name = "LogisticsTime")]
        [Display(Name = "LogisticsTime")]
        public DateTime LogisticsTime { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [DataMember(Name = "Remark")]
        [Display(Name = "Remark")]
        public string Remark { get; set; }

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
        private static readonly ObjectSchema<OrderLogisticsEntity> _schema;
        static OrderLogisticsEntity()
        {
            _schema = new ObjectSchema<OrderLogisticsEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.ExpressCompany, "ExpressCompany");

            _schema.AddField(x => x.ExpressList, "ExpressList");

            _schema.AddField(x => x.ExPressStatus, "ExPressStatus");

            _schema.AddField(x => x.LogisticsTime, "LogisticsTime");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.Remark, "Remark");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.CreateBy, "CreateBy");
            _schema.Compile();
        }
    }
}