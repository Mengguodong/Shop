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
    /// OrderListTotalModel
    /// </summary>
    public class OrderListTotalModel
    {

        /// <summary>
        /// OrderCount
        /// </summary>
        [DataMember(Name = "OrderCount")]
        [Display(Name = "OrderCount")]
        public int OrderCount { get; set; }

        /// <summary>
        /// SkuCount
        /// </summary>
        [DataMember(Name = "SkuCount")]
        [Display(Name = "SkuCount")]
        public int SkuCount { get; set; }

        /// <summary>
        /// PaidAmount
        /// </summary>
        [DataMember(Name = "PaidAmount")]
        [Display(Name = "PaidAmount")]
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// CustomsDuties
        /// </summary>
        [DataMember(Name = "CustomsDuties")]
        [Display(Name = "CustomsDuties")]
        public decimal CustomsDuties { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember(Name = "ExchangeRate")]
        [Display(Name = "ExchangeRate")]
        public decimal ExchangeRate { get; set; }

        

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<OrderListTotalModel> _schema;
        static OrderListTotalModel()
        {
            _schema = new ObjectSchema<OrderListTotalModel>();
            _schema.AddField(x => x.OrderCount, "OrderCount");

            _schema.AddField(x => x.SkuCount, "SkuCount");

            _schema.AddField(x => x.PaidAmount, "PaidAmount");

            _schema.AddField(x => x.CustomsDuties, "CustomsDuties");

            _schema.AddField(x => x.ExchangeRate, "ExchangeRate");
            _schema.Compile();
        }
    }
}