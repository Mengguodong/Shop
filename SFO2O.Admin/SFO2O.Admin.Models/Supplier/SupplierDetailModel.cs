using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Admin.Models.Supplier
{
    public class SupplierDetailModel
    {
        public SupplierModel Supplier { get; set; }

        public List<BrandModel> Brands { get; set; }
    }


    [Serializable]
    [DataContract]
    /// <summary>
    /// SupplierModel
    /// </summary>
    public class SupplierModel
    {

        /// <summary>
        /// SupplierID
        /// </summary>
        [DataMember(Name = "SupplierID")]
        [Display(Name = "SupplierID")]
        public int SupplierID { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        [DataMember(Name = "UserName")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        [DataMember(Name = "Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }


        /// <summary>
        /// CompanyName
        /// </summary>
        [DataMember(Name = "CompanyName")]
        [Display(Name = "CompanyName")]
        public string CompanyName { get; set; }

        /// <summary>
        /// CompanyName_Sample
        /// </summary>
        [DataMember(Name = "CompanyName_Sample")]
        [Display(Name = "CompanyName_Sample")]
        public string CompanyName_Sample { get; set; }

        /// <summary>
        /// CompanyName_English
        /// </summary>
        [DataMember(Name = "CompanyName_English")]
        [Display(Name = "CompanyName_English")]
        public string CompanyName_English { get; set; }

        /// <summary>
        /// TrueName
        /// </summary>
        [DataMember(Name = "TrueName")]
        [Display(Name = "TrueName")]
        public string TrueName { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        [DataMember(Name = "Address")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// Address_Sample
        /// </summary>
        [DataMember(Name = "Address_Sample")]
        [Display(Name = "Address_Sample")]
        public string Address_Sample { get; set; }

        /// <summary>
        /// Address_English
        /// </summary>
        [DataMember(Name = "Address_English")]
        [Display(Name = "Address_English")]
        public string Address_English { get; set; }

        /// <summary>
        /// ContactPerson
        /// </summary>
        [DataMember(Name = "ContactPerson")]
        [Display(Name = "ContactPerson")]
        public string ContactPerson { get; set; }

        /// <summary>
        /// ContactPerson_Sample
        /// </summary>
        [DataMember(Name = "ContactPerson_Sample")]
        [Display(Name = "ContactPerson_Sample")]
        public string ContactPerson_Sample { get; set; }

        /// <summary>
        /// ContactPerson_English
        /// </summary>
        [DataMember(Name = "ContactPerson_English")]
        [Display(Name = "ContactPerson_English")]
        public string ContactPerson_English { get; set; }

        /// <summary>
        /// ContactTel
        /// </summary>
        [DataMember(Name = "ContactTel")]
        [Display(Name = "ContactTel")]
        public string ContactTel { get; set; }

        /// <summary>
        /// ContactPhone
        /// </summary>
        [DataMember(Name = "ContactPhone")]
        [Display(Name = "ContactPhone")]
        public string ContactPhone { get; set; }

        /// <summary>
        /// ContactFax
        /// </summary>
        [DataMember(Name = "ContactFax")]
        [Display(Name = "ContactFax")]
        public string ContactFax { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        [DataMember(Name = "UpdateTime")]
        [Display(Name = "UpdateTime")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// UpdateBy
        /// </summary>
        [DataMember(Name = "UpdateBy")]
        [Display(Name = "UpdateBy")]
        public string UpdateBy { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        [DataMember(Name = "CreateBy")]
        [Display(Name = "CreateBy")]
        public DateTime CreateBy { get; set; }

        /// <summary>
        /// SkuNumber
        /// </summary>
        [DataMember(Name = "SkuNumber")]
        [Display(Name = "SkuNumber")]
        public int SkuNumber { get; set; }

        /// <summary>
        /// StoreLogoPath
        /// </summary>
        [DataMember(Name = "StoreLogoPath")]
        [Display(Name = "StoreLogoPath")]
        public string StoreLogoPath { get; set; }


        /// <summary>
        /// Description
        /// </summary>
        [DataMember(Name = "Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }


        /// <summary>
        /// StoreLogoPath
        /// </summary>
        [DataMember(Name = "Description_Sample")]
        [Display(Name = "Description_Sample")]
        public string Description_Sample { get; set; }


        /// <summary>
        /// StoreLogoPath
        /// </summary>
        [DataMember(Name = "Description_English")]
        [Display(Name = "Description_English")]
        public string Description_English { get; set; }

        /// <summary>
        /// ImgPath
        /// </summary>
        [DataMember(Name = "ImgPath")]
        [Display(Name = "ImgPath")]
        public string ImgPath { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SupplierModel> _schema;
        static SupplierModel()
        {
            _schema = new ObjectSchema<SupplierModel>();
            _schema.AddField(x => x.SupplierID, "SupplierID");

            _schema.AddField(x => x.UserName, "UserName");

            _schema.AddField(x => x.CompanyName, "CompanyName");

            _schema.AddField(x => x.CompanyName_Sample, "CompanyName_Sample");

            _schema.AddField(x => x.CompanyName_English, "CompanyName_English");

            _schema.AddField(x => x.TrueName, "TrueName");

            _schema.AddField(x => x.Address, "Address");

            _schema.AddField(x => x.Address_Sample, "Address_Sample");

            _schema.AddField(x => x.Address_English, "Address_English");

            _schema.AddField(x => x.ContactPerson, "ContactPerson");

            _schema.AddField(x => x.ContactPerson_Sample, "ContactPerson_Sample");

            _schema.AddField(x => x.ContactPerson_English, "ContactPerson_English");

            _schema.AddField(x => x.ContactTel, "ContactTel");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.UpdateTime, "UpdateTime");

            _schema.AddField(x => x.UpdateBy, "UpdateBy");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.CreateBy, "CreateBy");

            _schema.AddField(x => x.SkuNumber, "SkuNumber");

            _schema.AddField(x => x.StoreLogoPath, "StoreLogoPath");

            _schema.AddField(x => x.Password, "Password");

            _schema.AddField(x => x.Description, "Description");

            _schema.AddField(x => x.Description_Sample, "Description_Sample");

            _schema.AddField(x => x.Description_English, "Description_English");
            _schema.Compile();
        }
    }

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
        private static readonly ObjectSchema<SupplierBrandModel> _schema;
        static SupplierBrandModel()
        {
            _schema = new ObjectSchema<SupplierBrandModel>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.SupplierId, "SupplierId");

            _schema.AddField(x => x.BrandNameSample, "BrandNameSample");

            _schema.AddField(x => x.BrandNameTraditional, "BrandNameTraditional");

            _schema.AddField(x => x.BrandNameEnglish, "BrandNameEnglish");
            _schema.Compile();
        }
    }

}