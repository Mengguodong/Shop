using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Model.Product;
namespace SFO2O.Model.Supplier
{
    /// <summary>
    /// BrandEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class BrandEntity
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
        /// CategroyId
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
        /// skuCount
        /// </summary>
        [DataMember(Name = "skuCount")]
        [Display(Name = "skuCount")]
        public int skuCount { get; set; }

        /// <summary>
        /// NationalFlag
        /// </summary>
        [DataMember(Name = "NationalFlag")]
        [Display(Name = "NationalFlag")]
        public string NationalFlag { get; set; }
        /// <summary>
        /// Slogan
        /// </summary>
        [DataMember(Name = "Slogan")]
        [Display(Name = "Slogan")]
        public string Slogan { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        [DataMember(Name = "TotalRecord")]
        [Display(Name = "TotalRecord")]
        public int TotalRecord { get; set; }

        /// <summary>
        /// 明星产品
        /// </summary>
        [DataMember(Name = "TotalRecord")]
        [Display(Name = "TotalRecord")]
        public List<ProductInfoModel> productInfoList { get; set; }
        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<BrandEntity> _schema;
        static BrandEntity()
        {
            _schema = new ObjectSchema<BrandEntity>();
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

            _schema.AddField(x => x.skuCount, "skuCount");

            _schema.AddField(x => x.NationalFlag, "NationalFlag");

            _schema.AddField(x => x.Slogan, "Slogan");
            _schema.AddField(x => x.TotalRecord, "TotalRecord");
            _schema.Compile();
        }
    }

    [Serializable]
    [DataContract]
    public class BrandInfo
    {      
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember(Name = "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

         /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<BrandInfo> _schema;
        static BrandInfo()
        {
            _schema = new ObjectSchema<BrandInfo>();
            _schema.AddField(x => x.Id, "Id");
            _schema.AddField(x => x.Name, "Name");
            _schema.Compile();
        }
    }
}