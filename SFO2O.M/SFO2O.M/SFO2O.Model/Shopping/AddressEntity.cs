using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Shopping
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// AddressEntity
    /// </summary>
    public class AddressEntity
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [DataMember(Name = "UserId")]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [DataMember(Name = "Type")]
        [Display(Name = "Type")]
        public int Type { get; set; }

        /// <summary>
        /// CountryId
        /// </summary>
        [DataMember(Name = "CountryId")]
        [Display(Name = "CountryId")]
        public string CountryId { get; set; }

        /// <summary>
        /// ProvinceId
        /// </summary>
        [DataMember(Name = "ProvinceId")]
        [Display(Name = "ProvinceId")]
        public string ProvinceId { get; set; }

        /// <summary>
        /// ProvinceName
        /// </summary>
        [DataMember(Name = "ProvinceName")]
        [Display(Name = "ProvinceName")]
        public string ProvinceName { get; set; }

        /// <summary>
        /// CityId
        /// </summary>
        [DataMember(Name = "CityId")]
        [Display(Name = "CityId")]
        public string CityId { get; set; }

        /// <summary>
        /// CityName
        /// </summary>
        [DataMember(Name = "CityName")]
        [Display(Name = "CityName")]
        public string CityName { get; set; }

        /// <summary>
        /// AreaId
        /// </summary>
        [DataMember(Name = "AreaId")]
        [Display(Name = "AreaId")]
        public string AreaId { get; set; }

        /// <summary>
        /// AreaName
        /// </summary>
        [DataMember(Name = "AreaName")]
        [Display(Name = "AreaName")]
        public string AreaName { get; set; }

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
        public int IsDefault { get; set; }

        /// <summary>
        /// PapersType
        /// </summary>
        [DataMember(Name = "PapersType")]
        [Display(Name = "PapersType")]
        public int PapersType { get; set; }

        /// <summary>
        /// PapersCode
        /// </summary>
        [DataMember(Name = "PapersCode")]
        [Display(Name = "PapersCode")]
        public string PapersCode { get; set; }

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
        public int IsEnable { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<AddressEntity> _schema;
        static AddressEntity()
        {
            _schema = new ObjectSchema<AddressEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.UserId, "UserId");

            _schema.AddField(x => x.Type, "Type");

            _schema.AddField(x => x.CountryId, "CountryId");

            _schema.AddField(x => x.ProvinceId, "ProvinceId");

            _schema.AddField(x => x.ProvinceName, "ProvinceName");

            _schema.AddField(x => x.CityId, "CityId");

            _schema.AddField(x => x.CityName, "CityName");

            _schema.AddField(x => x.AreaId, "AreaId");

            _schema.AddField(x => x.AreaName, "AreaName");

            _schema.AddField(x => x.PostCode, "PostCode");

            _schema.AddField(x => x.Address, "Address");

            _schema.AddField(x => x.Receiver, "Receiver");

            _schema.AddField(x => x.Phone, "Phone");

            _schema.AddField(x => x.IsDefault, "IsDefault");

            _schema.AddField(x => x.PapersType, "PapersType");

            _schema.AddField(x => x.PapersCode, "PapersCode");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.CreateBy, "CreateBy");

            _schema.AddField(x => x.UpdateTime, "UpdateTime");

            _schema.AddField(x => x.UpdateBy, "UpdateBy");

            _schema.AddField(x => x.IsEnable, "IsEnable");
            _schema.Compile();
        }
    }
}