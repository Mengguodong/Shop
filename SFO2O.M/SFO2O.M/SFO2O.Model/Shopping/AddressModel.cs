using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Shopping
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// AddressModel
    /// </summary>
    public class AddressModel
    {

        /// <summary>
        /// 地址id
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
        /// 1顾客、2商家
        /// </summary>
        [DataMember(Name = "Type")]
        [Display(Name = "Type")]
        public int Type { get; set; }

        /// <summary>
        /// ProvinceId
        /// </summary>
        [DataMember(Name = "ProvinceId")]
        [Display(Name = "ProvinceId")]
        public int ProvinceId { get; set; }

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
        public int CityId { get; set; }

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
        public int AreaId { get; set; }

        /// <summary>
        /// AreaName
        /// </summary>
        [DataMember(Name = "AreaName")]
        [Display(Name = "AreaName")]
        public string AreaName { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [DataMember(Name = "PostCode")]
        [Display(Name = "PostCode")]
        public string PostCode { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [DataMember(Name = "Address")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        [DataMember(Name = "Receiver")]
        [Display(Name = "Receiver")]
        public string Receiver { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [DataMember(Name = "Phone")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// 1默认地址
        /// </summary>
        [DataMember(Name = "IsDefault")]
        [Display(Name = "IsDefault")]
        public int IsDefault { get; set; }

        /// <summary>
        /// 1身份证、2护照、3其他
        /// </summary>
        [DataMember(Name = "PapersType")]
        [Display(Name = "PapersType")]
        public int PapersType { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        [DataMember(Name = "PapersCode")]
        [Display(Name = "PapersCode")]
        public string PapersCode { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<AddressModel> _schema;
        static AddressModel()
        {
            _schema = new ObjectSchema<AddressModel>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.UserId, "UserId");

            _schema.AddField(x => x.Type, "Type");

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
            _schema.Compile();
        }
    }
}
