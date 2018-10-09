using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Supplier.Models
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// SupplierBrandModel
    /// </summary>
    public class SupplierBrandModel
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
        /// NameCN
        /// </summary>
        [DataMember(Name = "NameCN")]
        [Display(Name = "NameCN")]
        public string NameCN { get; set; }

        /// <summary>
        /// NameHK
        /// </summary>
        [DataMember(Name = "NameHK")]
        [Display(Name = "NameHK")]
        public string NameHK { get; set; }

        /// <summary>
        /// NameEN
        /// </summary>
        [DataMember(Name = "NameEN")]
        [Display(Name = "NameEN")]
        public string NameEN { get; set; }

        /// <summary>
        /// Logo
        /// </summary>
        [DataMember(Name = "Logo")]
        [Display(Name = "Logo")]
        public string Logo { get; set; }

        /// <summary>
        /// Banner
        /// </summary>
        [DataMember(Name = "Banner")]
        [Display(Name = "Banner")]
        public string Banner { get; set; }

        /// <summary>
        /// IntroductionCN
        /// </summary>
        [DataMember(Name = "IntroductionCN")]
        [Display(Name = "IntroductionCN")]
        public string IntroductionCN { get; set; }

        /// <summary>
        /// IntroductionHK
        /// </summary>
        [DataMember(Name = "IntroductionHK")]
        [Display(Name = "IntroductionHK")]
        public string IntroductionHK { get; set; }

        /// <summary>
        /// IntroductionEN
        /// </summary>
        [DataMember(Name = "IntroductionEN")]
        [Display(Name = "IntroductionEN")]
        public string IntroductionEN { get; set; }

        /// <summary>
        /// CountryId
        /// </summary>
        [DataMember(Name = "CountryId")]
        [Display(Name = "CountryId")]
        public int CountryId { get; set; }

        /// <summary>
        /// CountryName
        /// </summary>
        [DataMember(Name = "CountryName")]
        [Display(Name = "CountryName")]
        public string CountryName { get; set; }

        /// <summary>
        /// CategoryId
        /// </summary>
        [DataMember(Name = "CategoryId")]
        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }

        /// <summary>
        /// CategoryName
        /// </summary>
        [DataMember(Name = "CategoryName")]
        [Display(Name = "CategoryName")]
        public string CategoryName { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// OnSaleCount
        /// </summary>
        [DataMember(Name = "OnSaleCount")]
        [Display(Name = "OnSaleCount")]
        public int OnSaleCount { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SupplierBrandModel> _schema;
        static SupplierBrandModel()
        {
            _schema = new ObjectSchema<SupplierBrandModel>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.SupplierId, "SupplierId");

            _schema.AddField(x => x.NameCN, "NameCN");

            _schema.AddField(x => x.NameHK, "NameHK");

            _schema.AddField(x => x.NameEN, "NameEN");

            _schema.AddField(x => x.Logo, "Logo");

            _schema.AddField(x => x.Banner, "Banner");

            _schema.AddField(x => x.IntroductionCN, "IntroductionCN");

            _schema.AddField(x => x.IntroductionHK, "IntroductionHK");

            _schema.AddField(x => x.IntroductionEN, "IntroductionEN");

            _schema.AddField(x => x.CountryId, "CountryId");

            _schema.AddField(x => x.CountryName, "CountryName");

            _schema.AddField(x => x.CategoryId, "CategoryId");

            _schema.AddField(x => x.CategoryName, "CategoryName");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.OnSaleCount, "OnSaleCount");
            _schema.Compile();
        }
    }
}