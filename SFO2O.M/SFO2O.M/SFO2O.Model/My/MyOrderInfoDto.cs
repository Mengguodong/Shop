using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.My
{
    public class MyOrderInfoDto
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
        /// OrderStatus
        /// </summary>
        [DataMember(Name = "OrderStatus")]
        [Display(Name = "OrderStatus")]
        public int OrderStatus { get; set; }

        /// <summary>
        /// TotalAmount
        /// </summary>
        [DataMember(Name = "TotalAmount")]
        [Display(Name = "TotalAmount")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Freight
        /// </summary>
        [DataMember(Name = "Freight")]
        [Display(Name = "Freight")]
        public decimal Freight { get; set; }

        /// <summary>
        /// ProductTotalAmount
        /// </summary>
        [DataMember(Name = "ProductTotalAmount")]
        [Display(Name = "ProductTotalAmount")]
        public decimal ProductTotalAmount { get; set; }

        /// <summary>
        /// CustomsDuties
        /// </summary>
        [DataMember(Name = "CustomsDuties")]
        [Display(Name = "CustomsDuties")]
        public decimal CustomsDuties { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// RowsCount
        /// </summary>
        [DataMember(Name = "RowsCount")]
        [Display(Name = "RowsCount")]
        public int RowsCount { get; set; }
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
        /// 汇率
        /// </summary>
        [DataMember(Name = "ExchangeRate")]
        [Display(Name = "ExchangeRate")]
        public decimal ExchangeRate { get; set; }

        
         

        /// <summary>
        /// Receiver
        /// </summary>
        [DataMember(Name = "Receiver")]
        [Display(Name = "Receiver")]
        public string Receiver { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        [DataMember(Name = "Phone")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// PassPortType
        /// </summary>
        [DataMember(Name = "PassPortType")]
        [Display(Name = "PassPortType")]
        public int PassPortType { get; set; }

        /// <summary>
        /// PassPortNum
        /// </summary>
        [DataMember(Name = "PassPortNum")]
        [Display(Name = "PassPortNum")]
        public string PassPortNum { get; set; }

        /// <summary>
        /// ReceiptAddress
        /// </summary>
        [DataMember(Name = "ReceiptAddress")]
        [Display(Name = "ReceiptAddress")]
        public string ReceiptAddress { get; set; }

        /// <summary>
        /// ReceiptPostalCode
        /// </summary>
        [DataMember(Name = "ReceiptPostalCode")]
        [Display(Name = "ReceiptPostalCode")]
        public string ReceiptPostalCode { get; set; }

        /// <summary>
        /// ReceiptRegion
        /// </summary>
        [DataMember(Name = "ReceiptRegion")]
        [Display(Name = "ReceiptRegion")]
        public string ReceiptRegion { get; set; }

        /// <summary>
        /// ReceiptCity
        /// </summary>
        [DataMember(Name = "ReceiptCity")]
        [Display(Name = "ReceiptCity")]
        public string ReceiptCity { get; set; }

        /// <summary>
        /// ReceiptProvince
        /// </summary>
        [DataMember(Name = "ReceiptProvince")]
        [Display(Name = "ReceiptProvince")]
        public string ReceiptProvince { get; set; }

        /// <summary>
        /// ReceiptCountry
        /// </summary>
        [DataMember(Name = "ReceiptCountry")]
        [Display(Name = "ReceiptCountry")]
        public int ReceiptCountry { get; set; }

        /// <summary>
        /// ShippingMethod
        /// </summary>
        [DataMember(Name = "ShippingMethod")]
        [Display(Name = "ShippingMethod")]
        public int ShippingMethod { get; set; }
         

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
        /// ArrivalTime
        /// </summary>
        [DataMember(Name = "ArrivalTime")]
        [Display(Name = "ArrivalTime")]
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// OrderCompletionTime
        /// </summary>
        [DataMember(Name = "OrderCompletionTime")]
        [Display(Name = "OrderCompletionTime")]
        public DateTime OrderCompletionTime { get; set; }
        public IList<MyOrderSkuInfoDto> SkuInfos { get; set; }


        /// <summary>
        /// IsCrossBorderEBTax
        /// </summary>
        [DataMember(Name = "IsCrossBorderEBTax")]
        [Display(Name = "IsCrossBorderEBTax")]
        public int IsCrossBorderEBTax { get; set; }

        /// <summary>
        /// IsCrossBorderEBTax
        /// </summary>
        [DataMember(Name = "ExpressList")]
        [Display(Name = "ExpressList")]
        public string ExpressList { get; set; }

        [DataMember(Name = "ExpressCompany")]
        [Display(Name = "ExpressCompany")]
        public string ExpressCompany { get; set; }

        /// <summary>
        /// 增加父订单号
        /// </summary>
        public string ParentOrderCode { get; set; }

        //增加税金类型
        public int TaxType { get; set; }
        
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TeamCode { get; set; }
        /// <summary>
        /// 酒豆
        /// </summary>
        public decimal HuoLi { get; set; }
        /// <summary>
        /// TeamUserId
        /// </summary> 
        /// 
        public int TeamUserId { get; set; }

        /// <summary>
        /// TeamStatus
        /// </summary> 
        public int TeamStatus { get; set; }
        public DateTime TeamStartTime { get; set; }
        public DateTime TeamEndTime { get; set; }
        public decimal CardSum { get; set; }
        public decimal Coupon { get; set; }
        public int CouponId { get; set; }
    }
}
