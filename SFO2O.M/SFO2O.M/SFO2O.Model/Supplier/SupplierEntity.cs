using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Supplier
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// SupplierEntity
    /// </summary>
    public class SupplierEntity
    {

        /// <summary>
        /// SupplierID
        /// </summary>
        [DataMember(Name = "SupplierID")]
        [Display(Name = "SupplierID")]
        public int SupplierID { get; set; }

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
        /// StoreName
        /// </summary>
        [DataMember(Name = "StoreName")]
        [Display(Name = "StoreName")]
        public string StoreName { get; set; }

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
        /// StoreLogoPath
        /// </summary>
        [DataMember(Name = "StoreLogoPath")]
        [Display(Name = "StoreLogoPath")]
        public string StoreLogoPath { get; set; }

        /// <summary>
        /// StoreBannerPath
        /// </summary>
        [DataMember(Name = "StoreBannerPath")]
        [Display(Name = "StoreBannerPath")]
        public string StoreBannerPath { get; set; }

        /// <summary>
        /// StorePageDesc
        /// </summary>
        [DataMember(Name = "StorePageDesc")]
        [Display(Name = "StorePageDesc")]
        public string StorePageDesc { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember(Name = "Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Description_Sample
        /// </summary>
        [DataMember(Name = "Description_Sample")]
        [Display(Name = "Description_Sample")]
        public string Description_Sample { get; set; }

        /// <summary>
        /// Description_English
        /// </summary>
        [DataMember(Name = "Description_English")]
        [Display(Name = "Description_English")]
        public string Description_English { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SupplierEntity> _schema;
        static SupplierEntity()
        {
            _schema = new ObjectSchema<SupplierEntity>();
            _schema.AddField(x => x.SupplierID, "SupplierID");

            _schema.AddField(x => x.CompanyName, "CompanyName");

            _schema.AddField(x => x.CompanyName_Sample, "CompanyName_Sample");

            _schema.AddField(x => x.CompanyName_English, "CompanyName_English");

            _schema.AddField(x => x.StoreName, "StoreName");

            _schema.AddField(x => x.Address, "Address");

            _schema.AddField(x => x.Address_Sample, "Address_Sample");

            _schema.AddField(x => x.Address_English, "Address_English");

            _schema.AddField(x => x.StoreLogoPath, "StoreLogoPath");

            _schema.AddField(x => x.StoreBannerPath, "StoreBannerPath");

            _schema.AddField(x => x.StorePageDesc, "StorePageDesc");
            _schema.AddField(x => x.Description, "Description");
            _schema.AddField(x => x.Description_Sample, "Description_Sample");
            _schema.AddField(x => x.Description_English, "Description_English");
            _schema.Compile();
        }
    }

}
