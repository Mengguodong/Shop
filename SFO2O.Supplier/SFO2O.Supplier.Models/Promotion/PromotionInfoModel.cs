using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Supplier.Models.Promotion
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// PromotionInfoModel
    /// </summary>
    public class PromotionInfoModel
    {

        /// <summary>
        /// spu
        /// </summary>
        [DataMember(Name = "spu")]
        [Display(Name = "spu")]
        public string spu { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        [DataMember(Name = "Sku")]
        [Display(Name = "Sku")]
        public string Sku { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// DiscountPrice
        /// </summary>
        [DataMember(Name = "DiscountPrice")]
        [Display(Name = "DiscountPrice")]
        public decimal DiscountPrice { get; set; }

        /// <summary>
        /// DiscountRate
        /// </summary>
        [DataMember(Name = "DiscountRate")]
        [Display(Name = "DiscountRate")]
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<PromotionInfoModel> _schema;
        static PromotionInfoModel()
        {
            _schema = new ObjectSchema<PromotionInfoModel>();
            _schema.AddField(x => x.spu, "spu");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.DiscountPrice, "DiscountPrice");

            _schema.AddField(x => x.DiscountRate, "DiscountRate");
            _schema.Compile();
        }
    }
}