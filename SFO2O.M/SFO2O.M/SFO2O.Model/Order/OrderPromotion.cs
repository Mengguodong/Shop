using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Model.Order
{
    /// <summary>
    /// OrderPromotion
    /// </summary>
    [Serializable]
    [DataContract]
    public class OrderPromotion
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// OrderCode
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

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
        /// PromotionId
        /// </summary>
        [DataMember(Name = "PromotionId")]
        [Display(Name = "PromotionId")]
        public int PromotionId { get; set; }

        /// <summary>
        /// PromotionPrice
        /// </summary>
        [DataMember(Name = "PromotionPrice")]
        [Display(Name = "PromotionPrice")]
        public decimal PromotionPrice { get; set; }

        /// <summary>
        /// OriginalPrice
        /// </summary>
        [DataMember(Name = "OriginalPrice")]
        [Display(Name = "OriginalPrice")]
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<OrderPromotion> _schema;
        static OrderPromotion()
        {
            _schema = new ObjectSchema<OrderPromotion>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.PromotionId, "PromotionId");

            _schema.AddField(x => x.PromotionPrice, "PromotionPrice");

            _schema.AddField(x => x.OriginalPrice, "OriginalPrice");
            _schema.Compile();
        }
    }
}