using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Model.Product
{
    /// <summary>
    /// ProductAttributeEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class ProductAttributeEntity
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// AttrDicId
        /// </summary>
        [DataMember(Name = "AttrDicId")]
        [Display(Name = "AttrDicId")]
        public int AttrDicId { get; set; }

        /// <summary>
        /// Spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        [DataMember(Name = "Sku")]
        [Display(Name = "Sku")]
        public string Sku { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [DataMember(Name = "Price")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Qty
        /// </summary>
        [DataMember(Name = "Qty")]
        [Display(Name = "Qty")]
        public int Qty { get; set; }

        /// <summary>
        /// AttrKey
        /// </summary>
        [DataMember(Name = "AttrKey")]
        [Display(Name = "AttrKey")]
        public string AttrKey { get; set; }

        /// <summary>
        /// AttrValue
        /// </summary>
        [DataMember(Name = "AttrValue")]
        [Display(Name = "AttrValue")]
        public string AttrValue { get; set; }

        /// <summary>
        /// IsSkuAttr
        /// </summary>
        [DataMember(Name = "IsSkuAttr")]
        [Display(Name = "IsSkuAttr")]
        public int IsSkuAttr { get; set; }

        /// <summary>
        /// IsSkuMainAttr
        /// </summary>
        [DataMember(Name = "IsSkuMainAttr")]
        [Display(Name = "IsSkuMainAttr")]
        public int IsSkuMainAttr { get; set; }

        /// <summary>
        /// MetaDataId
        /// </summary>
        [DataMember(Name = "MetaDataId")]
        [Display(Name = "MetaDataId")]
        public long MetaDataId { get; set; }

        /// <summary>
        /// MetaDataName
        /// </summary>
        [DataMember(Name = "MetaDataName")]
        [Display(Name = "MetaDataName")]
        public string MetaDataName { get; set; }

        /// <summary>
        /// MetaDataCode
        /// </summary>
        [DataMember(Name = "MetaDataCode")]
        [Display(Name = "MetaDataCode")]
        public string MetaDataCode { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ProductAttributeEntity> _schema;
        static ProductAttributeEntity()
        {
            _schema = new ObjectSchema<ProductAttributeEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.AttrDicId, "AttrDicId");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.Price, "Price");

            _schema.AddField(x => x.Qty, "Qty");

            _schema.AddField(x => x.AttrKey, "AttrKey");

            _schema.AddField(x => x.AttrValue, "AttrValue");

            _schema.AddField(x => x.IsSkuAttr, "IsSkuAttr");

            _schema.AddField(x => x.IsSkuMainAttr, "IsSkuMainAttr");

            _schema.AddField(x => x.MetaDataId, "MetaDataId");

            _schema.AddField(x => x.MetaDataName, "MetaDataName");

            _schema.AddField(x => x.MetaDataCode, "MetaDataCode");
            _schema.Compile();
        }
    }
}