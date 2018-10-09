using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Common.EntLib.DataExtensions.DataMapper.Schema;

namespace  SFO2O.Supplier.Models
{
    [Serializable]
    [DataContract]
    public class RefundInfoModel
    {
        /// <summary>
        /// OrderCode
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

        /// <summary>
        /// RefundCode
        /// </summary>
        [DataMember(Name = "RefundCode")]
        [Display(Name = "RefundCode")]
        public string RefundCode { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        public int RefundType { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [DataMember(Name = "UserId")]
        [Display(Name = "UserId")]
        public int UserId { get; set; }


        /// <summary>
        /// UserName
        /// </summary>
        [DataMember(Name = "UserName")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }


        /// <summary>
        /// TotalAmount
        /// </summary>
        [DataMember(Name = "TotalAmount")]
        [Display(Name = "TotalAmount")]
        public decimal TotalAmount { get; set; }


        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember(Name = "ExchangeRate")]
        [Display(Name = "ExchangeRate")]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        [DataMember(Name = "Sku")]
        [Display(Name = "Sku")]
        public string Sku { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember(Name = "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        [DataMember(Name = "Quantity")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// UnitPrice
        /// </summary>
        [DataMember(Name = "UnitPrice")]
        [Display(Name = "UnitPrice")]
        public decimal UnitPrice { get; set; }


        /// <summary>
        /// RMBUnitPrice
        /// </summary>
        [DataMember(Name = "RMBUnitPrice")]
        [Display(Name = "RMBUnitPrice")]
        public decimal RMBUnitPrice { get; set; }

        /// <summary>
        /// TaxRate
        /// </summary>
        [DataMember(Name = "TaxRate")]
        [Display(Name = "TaxRate")]
        public decimal TaxRate { get; set; }

        /// <summary>
        /// MainDicValue
        /// </summary>
        [DataMember(Name = "MainDicValue")]
        [Display(Name = "MainDicValue")]
        public string MainDicValue { get; set; }

        /// <summary>
        /// SubDicValue
        /// </summary>
        [DataMember(Name = "SubDicValue")]
        [Display(Name = "SubDicValue")]
        public string SubDicValue { get; set; }

        /// <summary>
        /// SubValue
        /// </summary>
        [DataMember(Name = "SubValue")]
        [Display(Name = "SubValue")]
        public string SubValue { get; set; }

        /// <summary>
        /// MainValue
        /// </summary>
        [DataMember(Name = "MainValue")]
        [Display(Name = "MainValue")]
        public string MainValue { get; set; }

        /// <summary>
        /// RefundStatus
        /// </summary>
        [DataMember(Name = "RefundStatus")]
        [Display(Name = "RefundStatus")]
        public int RefundStatus { get; set; }

        public string ProductImagePath { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<RefundInfoModel> _schema;
        static RefundInfoModel()
        {
            _schema = new ObjectSchema<RefundInfoModel>();
            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.RefundCode, "RefundCode");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.RefundType, "RefundType");

            _schema.AddField(x => x.UserId, "UserId");

            _schema.AddField(x => x.UserName, "UserName");

            _schema.AddField(x => x.TotalAmount, "TotalAmount");

            _schema.AddField(x => x.ExchangeRate, "ExchangeRate");

            _schema.AddField(x => x.RefundStatus, "RefundStatus");    

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Name, "Name"); 

            _schema.AddField(x => x.Quantity, "Quantity");

            _schema.AddField(x => x.UnitPrice, "UnitPrice");

            _schema.AddField(x => x.RMBUnitPrice, "RMBUnitPrice");

            _schema.AddField(x => x.TaxRate, "TaxRate");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.ProductImagePath, "ProductImagePath");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.Compile();
        }


       
    }
}
