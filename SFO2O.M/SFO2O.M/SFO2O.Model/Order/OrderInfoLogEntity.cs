using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

using SFO2O.EntLib.DataExtensions.DataMapper.Schema;


namespace SFO2O.Model.Order
{
    public class OrderInfoLogEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// OrderCode
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

        /// <summary>
        /// CurrentStatus
        /// </summary>
        [DataMember(Name = "CurrentStatus")]
        [Display(Name = "CurrentStatus")]
        public int CurrentStatus { get; set; }

        /// <summary>
        /// AfterStatus
        /// </summary>
        [DataMember(Name = "AfterStatus")]
        [Display(Name = "AfterStatus")]
        public int AfterStatus { get; set; }

        /// <summary>
        /// OperateIp
        /// </summary>
        [DataMember(Name = "OperateIp")]
        [Display(Name = "OperateIp")]
        public string OperateIp { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [DataMember(Name = "Remark")]
        [Display(Name = "Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        [DataMember(Name = "CreateBy")]
        [Display(Name = "CreateBy")]
        public string CreateBy { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<OrderInfoLogEntity> _schema;
        static OrderInfoLogEntity()
        {
            _schema = new ObjectSchema<OrderInfoLogEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.CurrentStatus, "CurrentStatus");

            _schema.AddField(x => x.AfterStatus, "AfterStatus");

            _schema.AddField(x => x.OperateIp, "OperateIp");

            _schema.AddField(x => x.Remark, "Remark");

            _schema.AddField(x => x.CreateBy, "CreateBy");

            _schema.AddField(x => x.CreateTime, "CreateTime");
            _schema.Compile();
        }

    }
}
