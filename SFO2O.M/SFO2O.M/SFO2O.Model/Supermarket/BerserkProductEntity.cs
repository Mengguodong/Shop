using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Supermarket
{
    public class BerserkProductEntity
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember(Name = "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "Unit")]
        [Display(Name = "Unit")]
        public string Unit { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "DiscountPrice")]
        [Display(Name = "DiscountPrice")]
        public decimal DiscountPrice { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "DiscountRate")]
        [Display(Name = "DiscountRate")]
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "MinPrice")]
        [Display(Name = "MinPrice")]
        public decimal MinPrice { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "MinForOrder")]
        [Display(Name = "MinForOrder")]
        public int MinForOrder { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "ForOrderQty")]
        [Display(Name = "ForOrderQty")]
        public int ForOrderQty { get; set; }

        private static readonly ObjectSchema<BerserkProductEntity> _schema;
        static BerserkProductEntity()
        {
            _schema = new ObjectSchema<BerserkProductEntity>();
            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.Unit, "Unit");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.DiscountPrice, "DiscountPrice");

            _schema.AddField(x => x.DiscountRate, "DiscountRate");

            _schema.AddField(x => x.MinPrice, "MinPrice");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.MinForOrder, "MinForOrder");

            _schema.AddField(x => x.ForOrderQty, "ForOrderQty");

            _schema.Compile();
        }
    }
}
