using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System.Collections.Generic;

namespace SFO2O.Model.CMS
{
    
    /// <summary>
    /// CMSCustomProductEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class CMSCustomProductEntity
    {

        /// <summary>
        /// CpProductId
        /// </summary>
        [DataMember(Name = "CpProductId")]
        [Display(Name = "CpProductId")]
        public int CpProductId { get; set; }

        /// <summary>
        /// ModuleId
        /// </summary>
        [DataMember(Name = "ModuleId")]
        [Display(Name = "ModuleId")]
        public int ModuleId { get; set; }

        /// <summary>
        /// CpBannerId
        /// </summary>
        [DataMember(Name = "CpBannerId")]
        [Display(Name = "CpBannerId")]
        public int CpBannerId { get; set; }

        /// <summary>
        /// Spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// CpSortValue
        /// </summary>
        [DataMember(Name = "CpSortValue")]
        [Display(Name = "CpSortValue")]
        public int CpSortValue { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [DataMember(Name = "Title")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// Unit
        /// </summary>
        [DataMember(Name = "Unit")]
        [Display(Name = "Unit")]
        public string Unit { get; set; }

        /// <summary>
        /// MinPrice
        /// </summary>
        [DataMember(Name = "MinPrice")]
        [Display(Name = "MinPrice")]
        public decimal MinPrice { get; set; }

        /// <summary>
        /// MinForOrder
        /// </summary>
        [DataMember(Name = "MinForOrder")]
        [Display(Name = "MinForOrder")]
        public int MinForOrder { get; set; }

        /// <summary>
        /// ImagePath
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

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
        /// Qty
        /// </summary>
        [DataMember(Name = "Qty")]
        [Display(Name = "Qty")]
        public int Qty { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<CMSCustomProductEntity> _schema;
        static CMSCustomProductEntity()
        {
            _schema = new ObjectSchema<CMSCustomProductEntity>();
            _schema.AddField(x => x.CpProductId, "CpProductId");

            _schema.AddField(x => x.ModuleId, "ModuleId");

            _schema.AddField(x => x.CpBannerId, "CpBannerId");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.CpSortValue, "CpSortValue");

            _schema.AddField(x => x.Title, "Title");

            _schema.AddField(x => x.Unit, "Unit");

            _schema.AddField(x => x.MinPrice, "MinPrice");

            _schema.AddField(x => x.MinForOrder, "MinForOrder");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.DiscountPrice, "DiscountPrice");

            _schema.AddField(x => x.DiscountRate, "DiscountRate");

            _schema.AddField(x => x.Qty, "Qty");

            _schema.Compile();
        }
    }
}