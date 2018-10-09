using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Search
{
    [Serializable]
    [DataContract]
    public class CMSHotKeyword
    {
        /// <summary>
        ///热词Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int ID { get; set; }

        /// <summary>
        /// 热词
        /// </summary>
        [DataMember(Name = "Content")]
        [Display(Name = "Content")]
        public string Content { get; set; }

        /// <summary>
        /// 是否红色
        /// </summary>
        [DataMember(Name = "IsRed")]
        [Display(Name = "IsRed")]
        public bool IsRed { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        [DataMember(Name = "SortValue")]
        [Display(Name = "SortValue")]
        public int SortValue { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember(Name = "CreateBy")]
        [Display(Name = "CreateBy")]
        public string CreateBy { get; set; }

         /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<CMSHotKeyword> _schema;
        static CMSHotKeyword()
        {
            _schema = new ObjectSchema<CMSHotKeyword>();
            _schema.AddField(x => x.ID, "Id");
            _schema.AddField(x => x.Content, "Content");
            _schema.AddField(x => x.IsRed, "IsRed");
            _schema.AddField(x => x.SortValue, "SortValue");
            _schema.AddField(x => x.CreateTime,"CreateTime");
            _schema.AddField(x => x.CreateBy, "CreateBy");
            _schema.Compile();
        }
    }
}
