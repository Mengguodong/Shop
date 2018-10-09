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
    public class BannerImagesEntity
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [DataMember(Name = "Title")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// ImageUrl
        /// </summary>
        [DataMember(Name = "ImageUrl")]
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// LinkUrl
        /// </summary>
        [DataMember(Name = "LinkUrl")]
        [Display(Name = "LinkUrl")]
        public string LinkUrl { get; set; }

        /// <summary>
        /// BeginTime
        /// </summary>
        [DataMember(Name = "BeginTime")]
        [Display(Name = "BeginTime")]
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        [DataMember(Name = "EndTime")]
        [Display(Name = "EndTime")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// SortValue
        /// </summary>
        [DataMember(Name = "SortValue")]
        [Display(Name = "SortValue")]
        public int SortValue { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime? CreateTime { get; set; }

       /// <summary>
        /// CreateBy
        /// </summary>
        [DataMember(Name = "CreateBy")]
        [Display(Name = "CreateBy")]
        public string CreateBy { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<BannerImagesEntity> _schema;
        static BannerImagesEntity()
        {
            _schema = new ObjectSchema<BannerImagesEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.Title, "Title");

            _schema.AddField(x => x.ImageUrl, "ImageUrl");

            _schema.AddField(x => x.LinkUrl, "LinkUrl");

            _schema.AddField(x => x.BeginTime, "BeginTime");

            _schema.AddField(x => x.EndTime, "EndTime");

            _schema.AddField(x => x.SortValue, "SortValue");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.CreateBy, "CreateBy");

            _schema.Compile();
        }
    }
}