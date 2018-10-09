using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Supplier.Models.Category
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// CategoryHistoryModel
    /// </summary>
    public class CategoryHistoryModel
    {

        /// <summary>
        /// FCategoryID
        /// </summary>
        [DataMember(Name = "FCategoryID")]
        [Display(Name = "FCategoryID")]
        public int FCategoryID { get; set; }

        /// <summary>
        /// FCategoryName
        /// </summary>
        [DataMember(Name = "FCategoryName")]
        [Display(Name = "FCategoryName")]
        public string FCategoryName { get; set; }

        /// <summary>
        /// SCategoryID
        /// </summary>
        [DataMember(Name = "SCategoryID")]
        [Display(Name = "SCategoryID")]
        public int SCategoryID { get; set; }

        /// <summary>
        /// SCategoryName
        /// </summary>
        [DataMember(Name = "SCategoryName")]
        [Display(Name = "SCategoryName")]
        public string SCategoryName { get; set; }

        /// <summary>
        /// TChategoryID
        /// </summary>
        [DataMember(Name = "TCategoryID")]
        [Display(Name = "TCategoryID")]
        public int TCategoryID { get; set; }

        /// <summary>
        /// TCategoryName
        /// </summary>
        [DataMember(Name = "TCategoryName")]
        [Display(Name = "TCategoryName")]
        public string TCategoryName { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<CategoryHistoryModel> _schema;
        static CategoryHistoryModel()
        {
            _schema = new ObjectSchema<CategoryHistoryModel>();
            _schema.AddField(x => x.FCategoryID, "FCategoryID");

            _schema.AddField(x => x.FCategoryName, "FCategoryName");

            _schema.AddField(x => x.SCategoryID, "SCategoryID");

            _schema.AddField(x => x.SCategoryName, "SCategoryName");

            _schema.AddField(x => x.TCategoryID, "TCategoryID");

            _schema.AddField(x => x.TCategoryName, "TCategoryName");
            _schema.Compile();
        }
    }
}