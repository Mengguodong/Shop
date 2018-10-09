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
    /// CategoryModel
    /// </summary>
    public class CategoryModel
    {

        /// <summary>
        /// CategoryId
        /// </summary>
        [DataMember(Name = "CategoryId")]
        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }

        /// <summary>
        /// RootId
        /// </summary>
        [DataMember(Name = "RootId")]
        [Display(Name = "RootId")]
        public int RootId { get; set; }

        /// <summary>
        /// ParentId
        /// </summary>
        [DataMember(Name = "ParentId")]
        [Display(Name = "ParentId")]
        public int ParentId { get; set; }

        /// <summary>
        /// SortValue
        /// </summary>
        [DataMember(Name = "SortValue")]
        [Display(Name = "SortValue")]
        public int SortValue { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// CategoryLevel
        /// </summary>
        [DataMember(Name = "CategoryLevel")]
        [Display(Name = "CategoryLevel")]
        public int CategoryLevel { get; set; }

        /// <summary>
        /// CategoryName
        /// </summary>
        [DataMember(Name = "CategoryName")]
        [Display(Name = "CategoryName")]
        public string CategoryName { get; set; }

        /// <summary>
        /// AppImgUrl
        /// </summary>
        [DataMember(Name = "AppImgUrl")]
        [Display(Name = "AppImgUrl")]
        public string AppImgUrl { get; set; }

        /// <summary>
        /// SiteImgUrl
        /// </summary>
        [DataMember(Name = "SiteImgUrl")]
        [Display(Name = "SiteImgUrl")]
        public string SiteImgUrl { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<CategoryModel> _schema;
        static CategoryModel()
        {
            _schema = new ObjectSchema<CategoryModel>();
            _schema.AddField(x => x.CategoryId, "CategoryId");

            _schema.AddField(x => x.RootId, "RootId");

            _schema.AddField(x => x.ParentId, "ParentId");

            _schema.AddField(x => x.SortValue, "SortValue");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.CategoryLevel, "CategoryLevel");

            _schema.AddField(x => x.CategoryName, "CategoryName");

            _schema.AddField(x => x.AppImgUrl, "AppImgUrl");

            _schema.AddField(x => x.SiteImgUrl, "SiteImgUrl");
            _schema.Compile();
        }
    }
}