using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Models
{
    [Serializable]
    [DataContract]
   public class OrderExpressEntity
    {
        /// <summary>
        /// RefundCount
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

        [DataMember(Name = "ExpressCompany")]
        [Display(Name = "ExpressCompany")]
        public string ExpressCompany { get; set; }

        /// <summary>
        /// ProductCount
        /// </summary>
        [DataMember(Name = "ExpressList")]
        [Display(Name = "ExpressList")]
        public string ExpressList { get; set; }

        /// <summary>
        /// RefundAmountTotal
        /// </summary>
        [DataMember(Name = "ExPressStatus")]
        [Display(Name = "ExPressStatus")]
        public decimal ExPressStatus { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<OrderExpressEntity> _schema;

        static OrderExpressEntity()
        {
            _schema = new ObjectSchema<OrderExpressEntity>();

            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.ExpressCompany, "ExpressCompany");

            _schema.AddField(x => x.ExpressList, "ExpressList");

            _schema.AddField(x => x.ExPressStatus, "ExPressStatus");

            _schema.Compile();
        }
    }
}
