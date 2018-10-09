using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Index
{
    
    /// <summary>
    /// IndexModules
    /// </summary>
    [Serializable]
    [DataContract]
    public class IndexModulesEntity
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "Key")]
        [Display(Name = "Key")]
        public int Key { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [DataMember(Name = "Title")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// SubTitle1
        /// </summary>
        [DataMember(Name = "SubTitle1")]
        [Display(Name = "SubTitle1")]
        public string SubTitle1 { get; set; }

        /// <summary>
        /// SubTitle2
        /// </summary>
        [DataMember(Name = "SubTitle2")]
        [Display(Name = "SubTitle2")]
        public string SubTitle2 { get; set; }

        /// <summary>
        /// ImagePath
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// LinkUrl
        /// </summary>
        [DataMember(Name = "LinkUrl")]
        [Display(Name = "LinkUrl")]
        public string LinkUrl { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember(Name = "Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// RefId
        /// </summary>
        [DataMember(Name = "RefId")]
        [Display(Name = "RefId")]
        public string RefId { get; set; }

        /// <summary>
        /// Sort
        /// </summary>
        [DataMember(Name = "Sort")]
        [Display(Name = "Sort")]
        public int Sort { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<IndexModulesEntity> _schema;
        static IndexModulesEntity()
        {
            _schema = new ObjectSchema<IndexModulesEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.Key, "Key");

            _schema.AddField(x => x.Title, "Title");

            _schema.AddField(x => x.SubTitle1, "SubTitle1");

            _schema.AddField(x => x.SubTitle2, "SubTitle2");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.LinkUrl, "LinkUrl");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.Description, "Description");

            _schema.AddField(x => x.RefId, "RefId");

            _schema.AddField(x => x.Sort, "Sort");

            _schema.AddField(x => x.Status, "Status");
            _schema.Compile();
        }
    }
}