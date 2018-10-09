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
    /// PromotionSkuListModel
    /// </summary>
    public class PromotionSkuListModel
    {

        [DataMember(Name = "PromotionId")]
        [Display(Name = "PromotionId")]
        public int PromotionId { get; set; }

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
        /// Name
        /// </summary>
        [DataMember(Name = "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// MainValue
        /// </summary>
        [DataMember(Name = "MainValue")]
        [Display(Name = "MainValue")]
        public string MainValue { get; set; }

        /// <summary>
        /// SubValue
        /// </summary>
        [DataMember(Name = "SubValue")]
        [Display(Name = "SubValue")]
        public string SubValue { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [DataMember(Name = "Price")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        /// <summary>
        /// skuProStatus
        /// </summary>
        [DataMember(Name = "skuProStatus")]
        [Display(Name = "skuProStatus")]
        public string skuProStatus { get; set; }


        [DataMember(Name = "PromotionPrice")]
        [Display(Name = "PromotionPrice")]
        public decimal PromotionPrice { get; set; }

        [DataMember(Name = "PromotionRate")]
        [Display(Name = "PromotionRate")]
        public decimal PromotionRate { get; set; }

        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<PromotionSkuListModel> _schema;
        static PromotionSkuListModel()
        {
            _schema = new ObjectSchema<PromotionSkuListModel>();
            _schema.AddField(x => x.spu, "spu");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.Price, "Price");

            _schema.AddField(x => x.skuProStatus, "skuProStatus");

            _schema.AddField(x => x.PromotionPrice, "PromotionPrice");

            _schema.AddField(x => x.PromotionRate, "PromotionRate");

            _schema.AddField(x => x.PromotionId, "PromotionId");

            _schema.AddField(x => x.ImagePath, "ImagePath");
            _schema.Compile();
        }
    }
}