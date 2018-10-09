using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Admin.Models.Order
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// OrderOrderLogisticsModel
    /// </summary>
    public class OrderLogisticsModel
    {

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
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// LogisticsTime
        /// </summary>
        [DataMember(Name = "LogisticsTime")]
        [Display(Name = "LogisticsTime")]
        public DateTime LogisticsTime { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [DataMember(Name = "Remark")]
        [Display(Name = "Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<OrderLogisticsModel> _schema;
        static OrderLogisticsModel()
        {
            _schema = new ObjectSchema<OrderLogisticsModel>();
            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.ExpressCompany, "ExpressCompany");

            _schema.AddField(x => x.ExpressList, "ExpressList");

            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.LogisticsTime, "LogisticsTime");

            _schema.AddField(x => x.Remark, "Remark");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.CreateTime, "CreateTime");
            _schema.Compile();
        }
    }
}