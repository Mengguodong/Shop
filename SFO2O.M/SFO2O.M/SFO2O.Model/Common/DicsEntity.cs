using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Model.Common
{
    /// <summary>
    /// DicsEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class DicsEntity
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// KeyName
        /// </summary>
        [DataMember(Name = "KeyName")]
        [Display(Name = "KeyName")]
        public string KeyName { get; set; }

        /// <summary>
        /// KeyValue
        /// </summary>
        [DataMember(Name = "KeyValue")]
        [Display(Name = "KeyValue")]
        public string KeyValue { get; set; }

        /// <summary>
        /// DicType
        /// </summary>
        [DataMember(Name = "DicType")]
        [Display(Name = "DicType")]
        public string DicType { get; set; }

        /// <summary>
        /// LanguageVersion
        /// </summary>
        [DataMember(Name = "LanguageVersion")]
        [Display(Name = "LanguageVersion")]
        public int LanguageVersion { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<DicsEntity> _schema;
        static DicsEntity()
        {
            _schema = new ObjectSchema<DicsEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.KeyName, "KeyName");

            _schema.AddField(x => x.KeyValue, "KeyValue");

            _schema.AddField(x => x.DicType, "DicType");

            _schema.AddField(x => x.LanguageVersion, "LanguageVersion");
            _schema.Compile();
        }
    }
}