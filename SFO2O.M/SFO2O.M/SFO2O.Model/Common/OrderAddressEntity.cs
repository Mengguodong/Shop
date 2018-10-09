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
    public class OrderAddressEntity
    {
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
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<OrderAddressEntity> _schema;
        static OrderAddressEntity()
        {
            _schema = new ObjectSchema<OrderAddressEntity>();
            _schema.AddField(x => x.ProvinceName, "ProvinceName");

            _schema.AddField(x => x.CityName, "CityName");

            _schema.AddField(x => x.AreaName, "AreaName");
            _schema.Compile();
        }
    }
}
