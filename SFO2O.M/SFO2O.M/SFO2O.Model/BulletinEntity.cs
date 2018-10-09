using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model
{
    /// <summary>
    /// BulletinEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class BulletinEntity
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
        /// Content
        /// </summary>
        [DataMember(Name = "Content")]
        [Display(Name = "Content")]
        public string Content { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// LinkUrl
        /// </summary>
        [DataMember(Name = "LinkUrl")]
        [Display(Name = "LinkUrl")]
        public string LinkUrl { get; set; }

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
        private static readonly ObjectSchema<BulletinEntity> _schema;
        static BulletinEntity()
        {
            _schema = new ObjectSchema<BulletinEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.Title, "Title");

            _schema.AddField(x => x.Content, "Content");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.LinkUrl, "LinkUrl");

            _schema.AddField(x => x.Sort, "Sort");

            _schema.AddField(x => x.Status, "Status");
            _schema.Compile();
        }
    }
}