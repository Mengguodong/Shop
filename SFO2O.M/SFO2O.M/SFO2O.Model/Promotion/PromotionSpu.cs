using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Promotion
{
    public class PromotionSpu
    {
        /// <summary>
        /// spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// sku
        /// </summary>
        [DataMember(Name = "Sku")]
        [Display(Name = "Sku")]
        public string Sku { get; set; }

        /// <summary>
        /// DiscountRate
        /// </summary>
        [DataMember(Name = "DiscountRate")]
        [Display(Name = "DiscountRate")]
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// DiscountPrice
        /// </summary>
        [DataMember(Name = "DiscountPrice")]
        [Display(Name = "DiscountPrice")]
        public decimal DiscountPrice { get; set; }

         /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<PromotionSpu> _schema;
        static PromotionSpu()
        {
            _schema = new ObjectSchema<PromotionSpu>();

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.DiscountRate, "DiscountRate");

            _schema.AddField(x => x.DiscountPrice, "DiscountPrice");

            _schema.Compile();
        }
    }
}
