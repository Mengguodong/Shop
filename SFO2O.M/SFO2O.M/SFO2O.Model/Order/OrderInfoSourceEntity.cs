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
    public class OrderInfoSourceEntity
    {
        /// <summary>
        /// TeamCode
        /// </summary>
        public string TeamCode { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int OrderSourceType { get; set; }

        /// <summary>
        /// Receiver
        /// </summary>
        public string OrderSourceValue { get; set; }

        /// <summary>
        /// TotalAmount
        /// </summary>
        public decimal DividedAmount { get; set; }

        /// <summary>
        /// Freight
        /// </summary>
        public decimal DividedPercent { get; set; }

    }
}