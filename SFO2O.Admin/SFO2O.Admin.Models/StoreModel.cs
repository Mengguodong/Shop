using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Admin.Models
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// StoreModel
    /// </summary>
    public class StoreModel
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// BrandId
        /// </summary>
        [DataMember(Name = "BrandId")]
        [Display(Name = "BrandId")]
        public int BrandId { get; set; }

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
        /// AddressCN
        /// </summary>
        [DataMember(Name = "AddressCN")]
        [Display(Name = "AddressCN")]
        public string AddressCN { get; set; }

        /// <summary>
        /// AddressEN
        /// </summary>
        [DataMember(Name = "AddressEN")]
        [Display(Name = "AddressEN")]
        public string AddressEN { get; set; }

        /// <summary>
        /// AddressHK
        /// </summary>
        [DataMember(Name = "AddressHK")]
        [Display(Name = "AddressHK")]
        public string AddressHK { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<StoreModel> _schema;
        static StoreModel()
        {
            _schema = new ObjectSchema<StoreModel>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.BrandId, "BrandId");

            _schema.AddField(x => x.AreaId, "AreaId");

            _schema.AddField(x => x.AreaName, "AreaName");

            _schema.AddField(x => x.AddressCN, "AddressCN");

            _schema.AddField(x => x.AddressEN, "AddressEN");

            _schema.AddField(x => x.AddressHK, "AddressHK");

            _schema.AddField(x => x.Status, "Status");
            _schema.Compile();
        }
    }
}