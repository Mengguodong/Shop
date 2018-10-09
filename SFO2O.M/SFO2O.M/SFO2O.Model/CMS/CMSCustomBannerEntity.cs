using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System.Collections.Generic;

namespace SFO2O.Model.CMS
{
    
    /// <summary>
    /// CMSCustomBannerEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class CMSCustomBannerEntity
    {

        /// <summary>
        /// BannerId
        /// </summary>
        [DataMember(Name = "BannerId")]
        [Display(Name = "BannerId")]
        public int BannerId { get; set; }

        /// <summary>
        /// CbModuleId
        /// </summary>
        [DataMember(Name = "CbModuleId")]
        [Display(Name = "CbModuleId")]
        public int CbModuleId { get; set; }

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
        /// CbSortValue
        /// </summary>
        [DataMember(Name = "CbSortValue")]
        [Display(Name = "CbSortValue")]
        public int CbSortValue { get; set; }

        /// <summary>
        /// CbDescription
        /// </summary>
        [DataMember(Name = "CbDescription")]
        [Display(Name = "CbDescription")]
        public string CbDescription { get; set; }

        /// <summary>
        /// LinkUrl
        /// </summary>
        [DataMember(Name = "LinkUrl")]
        [Display(Name = "LinkUrl")]
        public string LinkUrl { get; set; }

        public IList<CMSCustomProductEntity> CMSCustomProductEntityList { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<CMSCustomBannerEntity> _schema;
        static CMSCustomBannerEntity()
        {
            _schema = new ObjectSchema<CMSCustomBannerEntity>();
            _schema.AddField(x => x.BannerId, "BannerId");

            _schema.AddField(x => x.CbModuleId, "CbModuleId");

            _schema.AddField(x => x.Title, "Title");

            _schema.AddField(x => x.ImageUrl, "ImageUrl");

            _schema.AddField(x => x.CbSortValue, "CbSortValue");

            _schema.AddField(x => x.CbDescription, "CbDescription");

            _schema.AddField(x => x.LinkUrl, "LinkUrl");

            _schema.AddField(x => x.CMSCustomProductEntityList, "CMSCustomProductEntityList");

            _schema.Compile();
        }
    }
}