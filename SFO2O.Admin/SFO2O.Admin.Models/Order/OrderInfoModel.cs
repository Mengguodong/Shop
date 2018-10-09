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
    /// OrderInfoModel
    /// </summary>
    public class OrderInfoModel
    {

        /// <summary>
        /// OrderCode
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

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
        /// Mobile
        /// </summary>
        [DataMember(Name = "Mobile")]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// NickName
        /// </summary>
        [DataMember(Name = "NickName")]
        [Display(Name = "NickName")]
        public string NickName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DataMember(Name = "Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Receiver
        /// </summary>
        [DataMember(Name = "Receiver")]
        [Display(Name = "Receiver")]
        public string Receiver { get; set; }

        /// <summary>
        /// ReceiptAddress
        /// </summary>
        [DataMember(Name = "ReceiptAddress")]
        [Display(Name = "ReceiptAddress")]
        public string ReceiptAddress { get; set; }

        /// <summary>
        /// PayTime
        /// </summary>
        [DataMember(Name = "PayTime")]
        [Display(Name = "PayTime")]
        public DateTime PayTime { get; set; }

        /// <summary>
        /// DeliveryTime
        /// </summary>
        [DataMember(Name = "DeliveryTime")]
        [Display(Name = "DeliveryTime")]
        public DateTime DeliveryTime { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        [DataMember(Name = "Phone")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// ShippingMethod
        /// </summary>
        [DataMember(Name = "ShippingMethod")]
        [Display(Name = "ShippingMethod")]
        public int ShippingMethod { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// OrderCompletionTime
        /// </summary>
        [DataMember(Name = "OrderCompletionTime")]
        [Display(Name = "OrderCompletionTime")]
        public DateTime OrderCompletionTime { get; set; }

        /// <summary>
        /// OrderStatus
        /// </summary>
        [DataMember(Name = "OrderStatus")]
        [Display(Name = "OrderStatus")]
        public int OrderStatus { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember(Name = "ExchangeRate")]
        [Display(Name = "ExchangeRate")]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// PaidAmount
        /// </summary>
        [DataMember(Name = "PaidAmount")]
        [Display(Name = "PaidAmount")]
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// TotalAmount
        /// </summary>
        [DataMember(Name = "TotalAmount")]
        [Display(Name = "TotalAmount")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// CustomsDuties
        /// </summary>
        [DataMember(Name = "CustomsDuties")]
        [Display(Name = "CustomsDuties")]
        public decimal CustomsDuties { get; set; }

         /// <summary>
        /// ProductTotalAmount
        /// </summary>
        [DataMember(Name = "ProductTotalAmount")]
        [Display(Name = "ProductTotalAmount")]
        public decimal ProductTotalAmount { get; set; }

        /// <summary>
        /// PayType
        /// </summary>
        [DataMember(Name = "PayType")]
        [Display(Name = "PayType")]
        public string PayType { get; set; }

        /// <summary>
        /// PayCode
        /// </summary>
        [DataMember(Name = "PayCode")]
        [Display(Name = "PayCode")]
        public string PayCode { get; set; }

        /// <summary>
        /// AreaName
        /// </summary>
        [DataMember(Name = "AreaName")]
        [Display(Name = "AreaName")]
        public string AreaName { get; set; }

        /// <summary>
        /// ProvinceName
        /// </summary>
        [DataMember(Name = "ProvinceName")]
        [Display(Name = "ProvinceName")]
        public string ProvinceName { get; set; }

        /// <summary>
        /// CityName
        /// </summary>
        [DataMember(Name = "CityName")]
        [Display(Name = "CityName")]
        public string CityName { get; set; }
        public string ExpressCode { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<OrderInfoModel> _schema;
        static OrderInfoModel()
        {
            _schema = new ObjectSchema<OrderInfoModel>();
            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.UserId, "UserId");

            _schema.AddField(x => x.UserName, "UserName");

            _schema.AddField(x => x.Mobile, "Mobile");

            _schema.AddField(x => x.NickName, "NickName");

            _schema.AddField(x => x.Email, "Email");

            _schema.AddField(x => x.Receiver, "Receiver");

            _schema.AddField(x => x.ReceiptAddress, "ReceiptAddress");

            _schema.AddField(x => x.PayTime, "PayTime");

            _schema.AddField(x => x.DeliveryTime, "DeliveryTime");

            _schema.AddField(x => x.Phone, "Phone");

            _schema.AddField(x => x.ShippingMethod, "ShippingMethod");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.OrderCompletionTime, "OrderCompletionTime");

            _schema.AddField(x => x.OrderStatus, "OrderStatus");

            _schema.AddField(x => x.ExchangeRate, "ExchangeRate");

            _schema.AddField(x => x.PaidAmount, "PaidAmount");

            _schema.AddField(x => x.TotalAmount, "TotalAmount");

            _schema.AddField(x => x.CustomsDuties, "CustomsDuties");

            _schema.AddField(x => x.ProductTotalAmount, "ProductTotalAmount");

            _schema.AddField(x => x.PayType, "PayType");

            _schema.AddField(x => x.PayCode, "PayCode");

            _schema.AddField(x => x.CityName, "CityName");

            _schema.AddField(x => x.ProvinceName, "ProvinceName");

            _schema.AddField(x => x.AreaName, "AreaName");
            _schema.Compile();
        }
    }
}