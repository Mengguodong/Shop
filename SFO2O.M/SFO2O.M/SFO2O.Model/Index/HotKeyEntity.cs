using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Index
{
    
    /// <summary>
    /// HotKeyModules
    /// </summary>
    [Serializable]
    [DataContract]
    public class HotKeyEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [DataMember(Name = "Content")]
        [Display(Name = "Content")]
        public string Content { get; set; }

        /// <summary>
        /// IsRed
        /// </summary>
        [DataMember(Name = "IsRed")]
        [Display(Name = "IsRed")]
        public bool IsRed { get; set; }

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
        private static readonly ObjectSchema<HotKeyEntity> _schema;
        static HotKeyEntity()
        {
            _schema = new ObjectSchema<HotKeyEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.Content, "Content");

            _schema.AddField(x => x.IsRed, "IsRed");

            _schema.AddField(x => x.SortValue, "SortValue");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.CreateBy, "CreateBy");

            _schema.Compile();
        }
    }
}