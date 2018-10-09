using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Source
{
    [Serializable]
    [DataContract]
    public class SourceEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [DataMember(Name = "ID")]
        [Display(Name = "ID")]
        public int ID { get; set; }
        
        /// <summary>
        /// TeamCode
        /// </summary>
        [DataMember(Name = "OrderSourceType")]
        [Display(Name = "OrderSourceType")]
        public int OrderSourceType { get; set; }

        /// <summary>
        /// sku
        /// </summary>
        [DataMember(Name = "DividedPercent")]
        [Display(Name = "DividedPercent")]
        public decimal DividedPercent { get; set; }

        /// <summary>
        /// TeamStatus
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        [DataMember(Name = "ChannelNo")]
        [Display(Name = "ChannelNo")]
        public string ChannelNo { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        [DataMember(Name = "ChannelName")]
        [Display(Name = "ChannelName")]
        public string ChannelName { get; set; }

        /// <summary>
        /// CreatTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// SuccTeamTime
        /// </summary>
        [DataMember(Name = "CreateBy")]
        [Display(Name = "CreateBy")]
        public string CreateBy { get; set; }


        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SourceEntity> _schema;
        static SourceEntity()
        {
            _schema = new ObjectSchema<SourceEntity>();

            _schema.AddField(x => x.ID, "ID");

            _schema.AddField(x => x.OrderSourceType, "OrderSourceType");

            _schema.AddField(x => x.DividedPercent, "DividedPercent");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.ChannelNo, "ChannelNo");

            _schema.AddField(x => x.ChannelName, "ChannelName");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.CreateBy, "CreateBy");

            _schema.Compile();
        }
    }
}
