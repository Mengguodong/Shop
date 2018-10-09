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
    /// Dics
    /// </summary>
    public class ProvinceModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DataMember(Name = "ID")]
        [Display(Name = "ID")]
        public int ID { get; set; }

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
        /// LanguageVersion
        /// </summary>
        [DataMember(Name = "LanguageVersion")]
        [Display(Name = "LanguageVersion")]
        public string LanguageVersion { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ProvinceModel> _schema;
        static ProvinceModel()
        {
            _schema = new ObjectSchema<ProvinceModel>();
            _schema.AddField(x => x.ID, "ID");

            _schema.AddField(x => x.ProvinceId, "ProvinceId");

            _schema.AddField(x => x.ProvinceName, "ProvinceName");

            _schema.AddField(x => x.LanguageVersion, "LanguageVersion");
            _schema.Compile();
        }
    }
}