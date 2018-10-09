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
    /// CommentEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class CommentEntity
    {

        /// <summary>
        /// RowNum
        /// </summary>
        [DataMember(Name = "RowNum")]
        [Display(Name = "RowNum")]
        public long RowNum { get; set; }

        /// <summary>
        /// CommentId
        /// </summary>
        [DataMember(Name = "CommentId")]
        [Display(Name = "CommentId")]
        public int CommentId { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [DataMember(Name = "UserId")]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [DataMember(Name = "Title")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        [DataMember(Name = "Comment")]
        [Display(Name = "Comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Rating
        /// </summary>
        [DataMember(Name = "Rating")]
        [Display(Name = "Rating")]
        public int Rating { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// OrderId
        /// </summary>
        [DataMember(Name = "OrderId")]
        [Display(Name = "OrderId")]
        public long OrderId { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        [DataMember(Name = "UserName")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// ImageUrl
        /// </summary>
        [DataMember(Name = "ImageUrl")]
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// MainDicValue
        /// </summary>
        [DataMember(Name = "MainDicValue")]
        [Display(Name = "MainDicValue")]
        public string MainDicValue { get; set; }

        /// <summary>
        /// SubDicValue
        /// </summary>
        [DataMember(Name = "SubDicValue")]
        [Display(Name = "SubDicValue")]
        public string SubDicValue { get; set; }

        /// <summary>
        /// MainValue
        /// </summary>
        [DataMember(Name = "MainValue")]
        [Display(Name = "MainValue")]
        public string MainValue { get; set; }

        /// <summary>
        /// SubValue
        /// </summary>
        [DataMember(Name = "SubValue")]
        [Display(Name = "SubValue")]
        public string SubValue { get; set; }

        /// <summary>
        /// RowsCount
        /// </summary>
        [DataMember(Name = "RowsCount")]
        [Display(Name = "RowsCount")]
        public long RowsCount { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<CommentEntity> _schema;
        static CommentEntity()
        {
            _schema = new ObjectSchema<CommentEntity>();
            _schema.AddField(x => x.RowNum, "RowNum");

            _schema.AddField(x => x.CommentId, "CommentId");

            _schema.AddField(x => x.UserId, "UserId");

            _schema.AddField(x => x.Title, "Title");

            _schema.AddField(x => x.Comment, "Comment");

            _schema.AddField(x => x.Rating, "Rating");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.OrderId, "OrderId");

            _schema.AddField(x => x.UserName, "UserName");

            _schema.AddField(x => x.ImageUrl, "ImageUrl");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.RowsCount, "RowsCount");
            _schema.Compile();
        }
    }
}