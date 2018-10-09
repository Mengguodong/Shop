using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Supplier.Models.Category
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// CategoryAttrModel
    /// </summary>
    public class CategoryAttrModel
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// CategoryId
        /// </summary>
        [DataMember(Name = "CategoryId")]
        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }

        /// <summary>
        /// KeyName
        /// </summary>
        [DataMember(Name = "KeyName")]
        [Display(Name = "KeyName")]
        public string KeyName { get; set; }

        /// <summary>
        /// KeyValue
        /// </summary>
        [DataMember(Name = "KeyValue")]
        [Display(Name = "KeyValue")]
        public string KeyValue { get; set; }

        /// <summary>
        /// IsRequire
        /// </summary>
        [DataMember(Name = "IsRequire")]
        [Display(Name = "IsRequire")]
        public int IsRequire { get; set; }

        /// <summary>
        /// VerificationType
        /// </summary>
        [DataMember(Name = "VerificationType")]
        [Display(Name = "VerificationType")]
        public int VerificationType { get; set; }

        /// <summary>
        /// IsFilter
        /// </summary>
        [DataMember(Name = "IsFilter")]
        [Display(Name = "IsFilter")]
        public int IsFilter { get; set; }

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
        /// IsShow
        /// </summary>
        [DataMember(Name = "IsShow")]
        [Display(Name = "IsShow")]
        public int IsShow { get; set; }

        /// <summary>
        /// ShowType
        /// </summary>
        [DataMember(Name = "ShowType")]
        [Display(Name = "ShowType")]
        public int ShowType { get; set; }

        /// <summary>
        /// IsUnitAttr
        /// </summary>
        [DataMember(Name = "IsUnitAttr")]
        [Display(Name = "IsUnitAttr")]
        public int IsUnitAttr { get; set; }

        /// <summary>
        /// InPutType
        /// </summary>
        [DataMember(Name = "InPutType")]
        [Display(Name = "InPutType")]
        public int InPutType { get; set; }

        /// <summary>
        /// MaxInput
        /// </summary>
        [DataMember(Name = "MaxInput")]
        [Display(Name = "MaxInput")]
        public int MaxInput { get; set; }

        /// <summary>
        /// MaxItems
        /// </summary>
        [DataMember(Name = "MaxItems")]
        [Display(Name = "MaxItems")]
        public int MaxItems { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<CategoryAttrModel> _schema;
        static CategoryAttrModel()
        {
            _schema = new ObjectSchema<CategoryAttrModel>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.CategoryId, "CategoryId");

            _schema.AddField(x => x.KeyName, "KeyName");

            _schema.AddField(x => x.KeyValue, "KeyValue");

            _schema.AddField(x => x.IsRequire, "IsRequire");

            _schema.AddField(x => x.VerificationType, "VerificationType");

            _schema.AddField(x => x.IsFilter, "IsFilter");

            _schema.AddField(x => x.IsSkuAttr, "IsSkuAttr");

            _schema.AddField(x => x.IsSkuMainAttr, "IsSkuMainAttr");

            _schema.AddField(x => x.IsShow, "IsShow");

            _schema.AddField(x => x.ShowType, "ShowType");

            _schema.AddField(x => x.IsUnitAttr, "IsUnitAttr");

            _schema.AddField(x => x.InPutType, "InPutType");

            _schema.AddField(x => x.MaxInput, "MaxInput");

            _schema.AddField(x => x.MaxItems, "MaxItems");
            _schema.Compile();
        }
    }
}