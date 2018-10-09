using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Supplier.Models.Product
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// ProductBrandModel
    /// </summary>
    public class ProductBrandModel
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// SupplierId
        /// </summary>
        [DataMember(Name = "SupplierId")]
        [Display(Name = "SupplierId")]
        public int SupplierId { get; set; }

        /// <summary>
        /// BrandNameSample
        /// </summary>
        [DataMember(Name = "BrandNameSample")]
        [Display(Name = "BrandNameSample")]
        public string BrandNameSample { get; set; }

        /// <summary>
        /// BrandNameTraditional
        /// </summary>
        [DataMember(Name = "BrandNameTraditional")]
        [Display(Name = "BrandNameTraditional")]
        public string BrandNameTraditional { get; set; }

        /// <summary>
        /// BrandNameEnglish
        /// </summary>
        [DataMember(Name = "BrandNameEnglish")]
        [Display(Name = "BrandNameEnglish")]
        public string BrandNameEnglish { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ProductBrandModel> _schema;
        static ProductBrandModel()
        {
            _schema = new ObjectSchema<ProductBrandModel>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.SupplierId, "SupplierId");

            _schema.AddField(x => x.BrandNameSample, "BrandNameSample");

            _schema.AddField(x => x.BrandNameTraditional, "BrandNameTraditional");

            _schema.AddField(x => x.BrandNameEnglish, "BrandNameEnglish");
            _schema.Compile();
        }
    }
}