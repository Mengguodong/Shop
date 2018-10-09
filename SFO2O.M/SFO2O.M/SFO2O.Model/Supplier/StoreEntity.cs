using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Supplier
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// 门店实体
    /// </summary>
    public class StoreEntity
    {
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "BrandId")]
        [Display(Name = "BrandId")]
        public int BrandId { get; set; }

        [DataMember(Name = "AreaId")]
        [Display(Name = "AreaId")]
        public int AreaId { get; set; }

        [DataMember(Name = "AreaName")]
        [Display(Name = "AreaName")]
        public string AreaName { get; set; }

        [DataMember(Name = "AddressCN")]
        [Display(Name = "AddressCN")]
        public string AddressCN { get; set; }

        [DataMember(Name = "AddressEN")]
        [Display(Name = "AddressEN")]
        public string AddressEN { get; set; }

        [DataMember(Name = "AddressHK")]
        [Display(Name = "AddressHK")]
        public string AddressHK { get; set; }


        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }


        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<StoreEntity> _schema;

        static StoreEntity()
        {
            _schema = new ObjectSchema<StoreEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x=>x.BrandId,"BrandId");

            _schema.AddField(x=>x.AreaId,"AreaId");

            _schema.AddField(x => x.AreaName, "AreaName");

            _schema.AddField(x => x.AddressCN, "AddressCN");

            _schema.AddField(x => x.AddressEN, "AddressEN");

            _schema.AddField(x => x.AddressHK, "AddressHK");

            _schema.AddField(x=>x.Status,"Status");

            _schema.Compile();
        }
    }
}
