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
    /// OrderStockOutModel
    /// </summary>
    public class OrderStockOutModel
    {

        /// <summary>
        /// OrderCode
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

        /// <summary>
        /// CompanyName
        /// </summary>
        [DataMember(Name = "CompanyName")]
        [Display(Name = "CompanyName")]
        public string CompanyName { get; set; }

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

        /// <summary>
        /// AreaName
        /// </summary>
        [DataMember(Name = "AreaName")]
        [Display(Name = "AreaName")]
        public string AreaName { get; set; }

        /// <summary>
        /// ReceiptAddress
        /// </summary>
        [DataMember(Name = "ReceiptAddress")]
        [Display(Name = "ReceiptAddress")]
        public string ReceiptAddress { get; set; }

        /// <summary>
        /// BarCode
        /// </summary>
        [DataMember(Name = "BarCode")]
        [Display(Name = "BarCode")]
        public string BarCode { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember(Name = "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// MainValue
        /// </summary>
        [DataMember(Name = "MainValue")]
        [Display(Name = "MainValue")]
        public string MainValue { get; set; }

        /// <summary>
        /// SubValue
        /// </summary>
        [DataMember(Name = "SubValue")]
        [Display(Name = "SubValue")]
        public string SubValue { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        [DataMember(Name = "Quantity")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

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
        /// Mobile
        /// </summary>
        [DataMember(Name = "Mobile")]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<OrderStockOutModel> _schema;
        static OrderStockOutModel()
        {
            _schema = new ObjectSchema<OrderStockOutModel>();
            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.CompanyName, "CompanyName");

            _schema.AddField(x => x.ProvinceName, "ProvinceName");

            _schema.AddField(x => x.CityName, "CityName");

            _schema.AddField(x => x.AreaName, "AreaName");

            _schema.AddField(x => x.ReceiptAddress, "ReceiptAddress");

            _schema.AddField(x => x.BarCode, "BarCode");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.Quantity, "Quantity");

            _schema.AddField(x => x.Receiver, "Receiver");

            _schema.AddField(x => x.Phone, "Phone");

            _schema.AddField(x => x.UnitPrice, "UnitPrice");

            _schema.AddField(x => x.TotalAmountHK, "TotalAmountHK");

            _schema.AddField(x => x.Mobile, "Mobile");
            _schema.Compile();
        }

        public decimal TotalAmountHK { get; set; }

        public decimal UnitPrice { get; set; }
    }
}