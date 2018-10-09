using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;


namespace SFO2O.Model.Supermarket
{
    public class SingleBannerImagesEntity
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "ChannelId")]
        [Display(Name = "ChannelId")]
        public int ChannelId { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Title")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "ImageUrl")]
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "LinkUrl")]
        [Display(Name = "LinkUrl")]
        public string LinkUrl { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "CreateBy")]
        [Display(Name = "CreateBy")]
        public string CreateBy { get; set; }

        private static readonly ObjectSchema<SingleBannerImagesEntity> _schema;
        static SingleBannerImagesEntity()
        {
            _schema = new ObjectSchema<SingleBannerImagesEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.ChannelId, "ChannelId");

            _schema.AddField(x => x.Title, "Title");

            _schema.AddField(x => x.ImageUrl, "ImageUrl");

            _schema.AddField(x => x.LinkUrl, "LinkUrl");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.CreateBy, "CreateBy");
            _schema.Compile();
        }
    }
}
