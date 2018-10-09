using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Common.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Supplier.Models
{
    public class OrderQueryInfo
    {
        public DateTime startTime{get ;set ;}

        public DateTime endTime { get; set; }

        public string orderCode { get; set; }

        public int orderSatus { get; set; }

        public int SupplierId { get; set; }

    }

    [Serializable]
    [DataContract]
    public class OrderTotalInfo
    {
        /// <summary>
        /// BuyerCount
        /// </summary>
        [DataMember(Name = "BuyerCount")]
        [Display(Name = "BuyerCount")]
        public int BuyerCount { get; set; }

        /// <summary>
        /// OrderCount
        /// </summary>
        [DataMember(Name = "OrderCount")]
        [Display(Name = "OrderCount")]
        public int OrderCount { get; set; }

        /// <summary>
        /// ProductCount
        /// </summary>
        [DataMember(Name = "ProductCount")]
        [Display(Name = "ProductCount")]
        public int ProductCount { get; set; }

        /// <summary>
        /// OrderAmountTotal
        /// </summary>
        [DataMember(Name = "OrderAmountTotal")]
        [Display(Name = "OrderAmountTotal")]
        public decimal OrderAmountTotal { get; set; }

        /// <summary>
        /// OrderAmountTotal
        /// </summary>
        [DataMember(Name = "FreightTotal")]
        [Display(Name = "FreightTotal")]
        public decimal FreightTotal { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<OrderTotalInfo> _schema;

        static OrderTotalInfo()
        {
            _schema = new ObjectSchema<OrderTotalInfo>();

            _schema.AddField(x => x.BuyerCount, "BuyerCount");

            _schema.AddField(x => x.OrderCount, "OrderCount");

            _schema.AddField(x => x.ProductCount, "ProductCount");

            _schema.AddField(x => x.OrderAmountTotal, "OrderAmountTotal");

            _schema.AddField(x => x.FreightTotal, "FreightTotal");

            _schema.Compile();
        }
    }
}
