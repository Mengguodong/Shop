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
    /// Dics
    /// </summary>
    public class CountryModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DataMember(Name = "ID")]
        [Display(Name = "ID")]
        public int ID { get; set; }

        /// <summary>
        /// CountryID
        /// </summary>
        [DataMember(Name = "CountryID")]
        [Display(Name = "CountryID")]
        public int CountryID { get; set; }

        /// <summary>
        /// CountryName
        /// </summary>
        [DataMember(Name = "CountryName")]
        [Display(Name = "CountryName")]
        public string CountryName { get; set; }

        /// <summary>
        /// LanguageVersion
        /// </summary>
        [DataMember(Name = "LanguageVersion")]
        [Display(Name = "LanguageVersion")]
        public string LanguageVersion { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<CountryModel> _schema;
        static CountryModel()
        {
            _schema = new ObjectSchema<CountryModel>();
            _schema.AddField(x => x.ID, "ID");

            _schema.AddField(x => x.CountryID, "CountryID");

            _schema.AddField(x => x.CountryName, "CountryName");
          
            _schema.AddField(x => x.LanguageVersion, "LanguageVersion");
            _schema.Compile();
        }
    }
}