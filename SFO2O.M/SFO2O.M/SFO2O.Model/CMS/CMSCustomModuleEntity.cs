using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System.Collections.Generic;

namespace SFO2O.Model.CMS
{
    
    /// <summary>
    /// CMSCustomModuleEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class CMSCustomModuleEntity
    {

        /// <summary>
        /// ModuleId
        /// </summary>
        [DataMember(Name = "ModuleId")]
        [Display(Name = "ModuleId")]
        public int ModuleId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember(Name = "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// MSubTitle
        /// </summary>
        [DataMember(Name = "MSubTitle")]
        [Display(Name = "MSubTitle")]
        public string MSubTitle { get; set; }

        /// <summary>
        /// CmSortValue
        /// </summary>
        [DataMember(Name = "CmSortValue")]
        [Display(Name = "CmSortValue")]
        public int CmSortValue { get; set; }

        public IList<CMSCustomBannerEntity> CMSCustomBannerEntityList { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<CMSCustomModuleEntity> _schema;
        static CMSCustomModuleEntity()
        {
            _schema = new ObjectSchema<CMSCustomModuleEntity>();
            _schema.AddField(x => x.ModuleId, "ModuleId");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.MSubTitle, "MSubTitle");

            _schema.AddField(x => x.MSubTitle, "CmSortValue");

            _schema.AddField(x => x.CMSCustomBannerEntityList, "CMSCustomBannerEntityList");

            _schema.Compile();
        }
    }
}