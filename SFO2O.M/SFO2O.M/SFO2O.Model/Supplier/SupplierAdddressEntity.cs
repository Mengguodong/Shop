using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Supplier
{
    [Serializable]
    [DataContract]
    public class SupplierAdddressEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// CustomerId
        /// </summary>
        [DataMember(Name = "CustomerId")]
        [Display(Name = "CustomerId")]
        public int CustomerId { get; set; }

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
        /// AddrProvince
        /// </summary>
        [DataMember(Name = "AddrProvince")]
        [Display(Name = "AddrProvince")]
        public string AddrProvince { get; set; }

        /// <summary>
        /// AddrCity
        /// </summary>
        [DataMember(Name = "AddrCity")]
        [Display(Name = "AddrCity")]
        public string AddrCity { get; set; }

        /// <summary>
        /// AddrArea
        /// </summary>
        [DataMember(Name = "AddrArea")]
        [Display(Name = "AddrArea")]
        public string AddrArea { get; set; }

        /// <summary>
        /// PostCode
        /// </summary>
        [DataMember(Name = "PostCode")]
        [Display(Name = "PostCode")]
        public string PostCode { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        [DataMember(Name = "Address")]
        [Display(Name = "Address")]
        public string Address { get; set; }

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
        /// IsDefault
        /// </summary>
        [DataMember(Name = "IsDefault")]
        [Display(Name = "IsDefault")]
        public Boolean IsDefault { get; set; }

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
        /// UpdateTime
        /// </summary>
        [DataMember(Name = "UpdateTime")]
        [Display(Name = "UpdateTime")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// UpdateBy
        /// </summary>
        [DataMember(Name = "UpdateBy")]
        [Display(Name = "UpdateBy")]
        public string UpdateBy { get; set; }

        /// <summary>
        /// IsEnable
        /// </summary>
        [DataMember(Name = "IsEnable")]
        [Display(Name = "IsEnable")]
        public Boolean IsEnable { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SupplierAdddressEntity> _schema;
        static SupplierAdddressEntity()
        {
            _schema = new ObjectSchema<SupplierAdddressEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.CustomerId, "CustomerId");

            _schema.AddField(x => x.ProvinceName, "ProvinceName");

            _schema.AddField(x => x.CityName, "CityName");

            _schema.AddField(x => x.AreaName, "AreaName");

            _schema.AddField(x => x.AddrProvince, "AddrProvince");

            _schema.AddField(x => x.AddrCity, "AddrCity");

            _schema.AddField(x => x.AddrArea, "AddrArea");

            _schema.AddField(x => x.PostCode, "PostCode");

            _schema.AddField(x => x.Address, "Address");

            _schema.AddField(x => x.Receiver, "Receiver");

            _schema.AddField(x => x.Phone, "Phone");

            _schema.AddField(x => x.IsDefault, "IsDefault");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.CreateBy, "CreateBy");

            _schema.AddField(x => x.UpdateTime, "UpdateTime");

            _schema.AddField(x => x.UpdateBy, "UpdateBy");

            _schema.AddField(x => x.IsEnable, "IsEnable");
            _schema.Compile();
        }
    }

}
