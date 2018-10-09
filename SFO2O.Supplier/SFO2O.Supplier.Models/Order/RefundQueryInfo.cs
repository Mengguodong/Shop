using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Common.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Supplier.Models
{
    public class RefundQueryInfo
    {
        public DateTime startTime{get ;set ;}

        public DateTime endTime { get; set; }

        public string refundCode { get; set; }

        public string orderCode { get; set; }

        public int refundStatus { get; set; }

        public int refundType{ get; set; }

        public int SupplierId { get; set; }

    }

    [Serializable]
    [DataContract]
    public class RefundTotalInfo
    {
       
        /// <summary>
        /// RefundCount
        /// </summary>
        [DataMember(Name = "RefundCount")]
        [Display(Name = "RefundCount")]
        public int RefundCount { get; set; }

        /// <summary>
        /// ProductCount
        /// </summary>
        [DataMember(Name = "ProductCount")]
        [Display(Name = "ProductCount")]
        public int ProductCount { get; set; }

        /// <summary>
        /// RefundAmountTotal
        /// </summary>
        [DataMember(Name = "RefundAmountTotal")]
        [Display(Name = "RefundAmountTotal")]
        public decimal RefundAmountTotal { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<RefundTotalInfo> _schema;

        static RefundTotalInfo()
        {
            _schema = new ObjectSchema<RefundTotalInfo>();

            _schema.AddField(x => x.RefundCount, "RefundCount");

            _schema.AddField(x => x.ProductCount, "ProductCount");

            _schema.AddField(x => x.RefundAmountTotal, "RefundAmountTotal");

            _schema.Compile();
        }
    }
}
