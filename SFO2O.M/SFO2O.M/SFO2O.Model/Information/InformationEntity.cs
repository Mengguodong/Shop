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
    public class InformationEntity
    {
        /// <summary>
        ///消息Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int ID { get; set; }

        /// <summary>
        /// InfoType
        /// </summary>
        [DataMember(Name = "Type")]
        [Display(Name = "InfoType")]
        public int InfoType { get; set; }

        /// <summary>
        /// WebInnerType
        /// </summary>
        [DataMember(Name = "WebInnerType")]
        [Display(Name = "WebInnerType")]
        public int WebInnerType { get; set; }
        
        /// <summary>
        /// SendDest
        /// </summary>
        [DataMember(Name = "SendDest")]
        [Display(Name = "SendDest")]
        public int SendDest { get; set; }

        /// <summary>
        /// SendUserId
        /// </summary>
        [DataMember(Name = "SendUserId")]
        [Display(Name = "SendUserId")]
        public int SendUserId { get; set; }

        /// <summary>
        /// TradeCode
        /// </summary>
        [DataMember(Name = "TradeCode")]
        [Display(Name = "TradeCode")]
        public string TradeCode { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [DataMember(Name = "Title")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// InfoContent
        /// </summary>
        [DataMember(Name = "Content")]
        [Display(Name = "InfoContent")]
        public string InfoContent { get; set; }

        /// <summary>
        /// ImagePath
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// Summary
        /// </summary>
        [DataMember(Name = "Summary")]
        [Display(Name = "Summary")]
        public string Summary { get; set; }

        /// <summary>
        /// LinkUrl
        /// </summary>
        [DataMember(Name = "LinkUrl")]
        [Display(Name = "LinkUrl")]
        public string LinkUrl { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        [DataMember(Name = "StartTime")]
        [Display(Name = "StartTime")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        [DataMember(Name = "EndTime")]
        [Display(Name = "EndTime")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// LongTerm
        /// </summary>
        [DataMember(Name = "LongTerm")]
        [Display(Name = "LongTerm")]
        public int LongTerm { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// NotReadInfoCount
        /// </summary>
        [DataMember(Name = "NotReadInfoCount")]
        [Display(Name = "NotReadInfoCount")]
        public int NotReadInfoCount { get; set; }

        /// <summary>
        /// Row_Number行号
        /// </summary>
        [DataMember(Name = "RIndex")]
        [Display(Name = "RIndex")]
        public int RIndex { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        [DataMember(Name = "TotalRecord")]
        [Display(Name = "TotalRecord")]
        public int TotalRecord { get; set; }

        /// <summary>
        /// 阅读表中的用户ID
        /// </summary>
        public int ReadUserId { get; set; }

        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<InformationEntity> _schema;
        static InformationEntity()
        {
            _schema = new ObjectSchema<InformationEntity>();

            _schema.AddField(x => x.ID, "Id");

            _schema.AddField(x => x.InfoType, "Type");

            _schema.AddField(x => x.WebInnerType, "WebInnerType");

            _schema.AddField(x => x.SendDest, "SendDest");

            _schema.AddField(x => x.SendUserId, "SendUserId");

            _schema.AddField(x => x.TradeCode, "TradeCode");

            _schema.AddField(x => x.Title, "Title");

            _schema.AddField(x => x.InfoContent, "Content");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.Summary, "Summary");

            _schema.AddField(x => x.LinkUrl, "LinkUrl");

            _schema.AddField(x => x.StartTime, "StartTime");

            _schema.AddField(x => x.EndTime, "EndTime");

            _schema.AddField(x => x.LongTerm, "LongTerm");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.RIndex, "RIndex");

            _schema.AddField(x => x.TotalRecord, "TotalRecord");

            _schema.AddField(x=>x.ReadUserId,"ReadUserId");

            _schema.AddField(x => x.NotReadInfoCount, "NotReadInfoCount");

            _schema.Compile();
        }

    }
}