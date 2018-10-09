using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Common
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// 省份
    /// </summary>
    public class ProvinceModel
    {

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
        /// ParentId
        /// </summary>
        [DataMember(Name = "ParentId")]
        [Display(Name = "ParentId")]
        public string ParentId { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ProvinceModel> _schema;
        static ProvinceModel()
        {
            _schema = new ObjectSchema<ProvinceModel>();
            _schema.AddField(x => x.ProvinceId, "ProvinceId");

            _schema.AddField(x => x.ProvinceName, "ProvinceName");

            _schema.AddField(x => x.ParentId, "ParentId");
            _schema.Compile();
        }
    }

    [Serializable]
    [DataContract]
    /// <summary>
    /// 城市
    /// </summary>
    public class CityModel
    {

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
        /// ParentId
        /// </summary>
        [DataMember(Name = "ParentId")]
        [Display(Name = "ParentId")]
        public string ParentId { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<CityModel> _schema;
        static CityModel()
        {
            _schema = new ObjectSchema<CityModel>();
            _schema.AddField(x => x.CityId, "CityId");

            _schema.AddField(x => x.CityName, "CityName");

            _schema.AddField(x => x.ParentId, "ParentId");
            _schema.Compile();
        }
    }

    [Serializable]
    [DataContract]
    /// <summary>
    /// 区县
    /// </summary>
    public class AreaModel
    {

        /// <summary>
        /// AreaId
        /// </summary>
        [DataMember(Name = "AreaId")]
        [Display(Name = "AreaId")]
        public string AreaId { get; set; }

        /// <summary>
        /// 区县名称
        /// </summary>
        [DataMember(Name = "AreaName")]
        [Display(Name = "AreaName")]
        public string AreaName { get; set; }

        /// <summary>
        /// ParentId
        /// </summary>
        [DataMember(Name = "ParentId")]
        [Display(Name = "ParentId")]
        public string ParentId { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<AreaModel> _schema;
        static AreaModel()
        {
            _schema = new ObjectSchema<AreaModel>();
            _schema.AddField(x => x.AreaId, "AreaId");

            _schema.AddField(x => x.AreaName, "AreaName");

            _schema.AddField(x => x.ParentId, "ParentId");
            _schema.Compile();
        }
    }
}
