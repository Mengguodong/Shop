using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Team
{
    [Serializable]
    [DataContract]
    public class TeamInfoEntity
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
        [DataMember(Name = "TeamCode")]
        [Display(Name = "TeamCode")]
        public string TeamCode { get; set; }

        /// <summary>
        /// sku
        /// </summary>
        [DataMember(Name = "sku")]
        [Display(Name = "sku")]
        public string Sku { get; set; }

        /// <summary>
        /// TeamStatus
        /// </summary>
        [DataMember(Name = "TeamStatus")]
        [Display(Name = "TeamStatus")]
        public int TeamStatus { get; set; }

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
        /// UserID
        /// </summary>
        [DataMember(Name = "UserID")]
        [Display(Name = "UserID")]
        public int UserID { get; set; }

        /// <summary>
        /// TeamNumbers
        /// </summary>
        [DataMember(Name = "TeamNumbers")]
        [Display(Name = "TeamNumbers")]
        public int TeamNumbers { get; set; }

        /// <summary>
        /// CreatTime
        /// </summary>
        [DataMember(Name = "CreatTime")]
        [Display(Name = "CreatTime")]
        public DateTime? CreatTime { get; set; }

        /// <summary>
        /// SuccTeamTime
        /// </summary>
        [DataMember(Name = "SuccTeamTime")]
        [Display(Name = "SuccTeamTime")]
        public DateTime? SuccTeamTime { get; set; }

        /// <summary>
        /// SuccTeamTime
        /// </summary>
        [DataMember(Name = "PromotionId")]
        [Display(Name = "PromotionId")]
        public int PromotionId { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<TeamInfoEntity> _schema;
        static TeamInfoEntity()
        {
            _schema = new ObjectSchema<TeamInfoEntity>();

            _schema.AddField(x => x.ID, "ID");

            _schema.AddField(x => x.TeamCode, "TeamCode");

            _schema.AddField(x => x.Sku, "sku");

            _schema.AddField(x => x.TeamStatus, "TeamStatus");

            _schema.AddField(x => x.StartTime, "StartTime");

            _schema.AddField(x => x.EndTime, "EndTime");

            _schema.AddField(x => x.UserID, "UserID");

            _schema.AddField(x => x.TeamNumbers, "TeamNumbers");

            _schema.AddField(x => x.CreatTime, "CreatTime");

            _schema.AddField(x => x.SuccTeamTime, "SuccTeamTime");

            _schema.AddField(x => x.PromotionId, "PromotionId");

            _schema.Compile();
        }

    }
}
