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
    [Serializable]
    [DataContract]
    /// <summary>
    /// SupplierAbstractModel
    /// </summary>
    public class SupplierAbstractModel
    {
        /// <summary>
        /// UserName
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
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// supplierStatus
        /// </summary>
        [DataMember(Name = "supplierStatus")]
        [Display(Name = "supplierStatus")]
        public string SupplierStatus { get; set; }

        /// <summary>
        /// SkuNumber
        /// </summary>
        [DataMember(Name = "SkuNumber")]
        [Display(Name = "SkuNumber")]
        public int SkuNumber { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SupplierAbstractModel> _schema;
        static SupplierAbstractModel()
        {
            _schema = new ObjectSchema<SupplierAbstractModel>();
            _schema.AddField(x => x.SupplierID, "SupplierID");

            _schema.AddField(x => x.UserName, "UserName");

            _schema.AddField(x => x.CompanyName, "CompanyName");

            _schema.AddField(x => x.CompanyName_Sample, "CompanyName_Sample");

            _schema.AddField(x => x.CompanyName_English, "CompanyName_English");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.SupplierStatus, "supplierStatus");

            _schema.AddField(x => x.SkuNumber, "SkuNumber");
            _schema.Compile();
        }
    }
}