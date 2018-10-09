using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Information
{
    [Serializable]
    [DataContract]
    public class InformationReadEntity
    {
        /// <summary>
        /// UserId
        /// </summary>
        [DataMember(Name = "UserId")]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        /// <summary>
        /// InformationId
        /// </summary>
        [DataMember(Name = "InformationId")]
        [Display(Name = "InformationId")]
        public int InformationId { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<InformationReadEntity> _schema;
        static InformationReadEntity()
        {
            _schema = new ObjectSchema<InformationReadEntity>();
            _schema.AddField(x => x.UserId, "UserId");

            _schema.AddField(x => x.InformationId, "InformationId");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.Compile();
        }

    }
}