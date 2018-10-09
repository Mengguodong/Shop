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
    /// <summary>
    /// OrderPaymentInfo
    /// </summary>
    public class OrderPaymentInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// PayCode
        /// </summary>
        [DataMember(Name = "PayCode")]
        [Display(Name = "PayCode")]
        public string PayCode { get; set; }

        /// <summary>
        /// TradeCode
        /// </summary>
        [DataMember(Name = "TradeCode")]
        [Display(Name = "TradeCode")]
        public string TradeCode { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [DataMember(Name = "UserId")]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        /// <summary>
        /// OrderType
        /// </summary>
        [DataMember(Name = "OrderType")]
        [Display(Name = "OrderType")]
        public int OrderType { get; set; }

        /// <summary>
        /// OrderCode
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

        /// <summary>
        /// PayAmount
        /// </summary>
        [DataMember(Name = "PayAmount")]
        [Display(Name = "PayAmount")]
        public decimal PayAmount { get; set; }

        /// <summary>
        /// PaidAmount
        /// </summary>
        [DataMember(Name = "PaidAmount")]
        [Display(Name = "PaidAmount")]
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// PayPlatform
        /// </summary>
        [DataMember(Name = "PayPlatform")]
        [Display(Name = "PayPlatform")]
        public int PayPlatform { get; set; }

        /// <summary>
        /// PayType
        /// </summary>
        [DataMember(Name = "PayType")]
        [Display(Name = "PayType")]
        public int PayType { get; set; }

        /// <summary>
        /// PayStatus
        /// </summary>
        [DataMember(Name = "PayStatus")]
        [Display(Name = "PayStatus")]
        public int PayStatus { get; set; }

        /// <summary>
        /// PayTerminal
        /// </summary>
        [DataMember(Name = "PayTerminal")]
        [Display(Name = "PayTerminal")]
        public int PayTerminal { get; set; }

        /// <summary>
        /// PayCompleteTime
        /// </summary>
        [DataMember(Name = "PayCompleteTime")]
        [Display(Name = "PayCompleteTime")]
        public DateTime PayCompleteTime { get; set; }

        /// <summary>
        /// PayBackRemark
        /// </summary>
        [DataMember(Name = "PayBackRemark")]
        [Display(Name = "PayBackRemark")]
        public string PayBackRemark { get; set; }

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
        private static readonly ObjectSchema<OrderPaymentInfo> _schema;
        static OrderPaymentInfo()
        {
            _schema = new ObjectSchema<OrderPaymentInfo>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.PayCode, "PayCode");

            _schema.AddField(x => x.TradeCode, "TradeCode");

            _schema.AddField(x => x.UserId, "UserId");

            _schema.AddField(x => x.OrderType, "OrderType");

            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.PayAmount, "PayAmount");

            _schema.AddField(x => x.PaidAmount, "PaidAmount");

            _schema.AddField(x => x.PayPlatform, "PayPlatform");

            _schema.AddField(x => x.PayType, "PayType");

            _schema.AddField(x => x.PayStatus, "PayStatus");

            _schema.AddField(x => x.PayTerminal, "PayTerminal");

            _schema.AddField(x => x.PayCompleteTime, "PayCompleteTime");

            _schema.AddField(x => x.PayBackRemark, "PayBackRemark");

            _schema.AddField(x => x.Remark, "Remark");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.CreateBy, "CreateBy");
            _schema.Compile();
        }
    }
}