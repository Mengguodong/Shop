using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Model.My
{
    /// <summary>
    /// OrderLogisticsEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class MyHL
    { 
        /// <summary>
        /// userId
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 酒豆总数
        /// </summary>
        public decimal countHL { get; set; }
        /// <summary>
        /// 冻结中的酒豆
        /// </summary>
        public decimal freezeHL { get; set; }
        /// <summary>
        /// 可用酒豆
        /// </summary>
        public decimal usableHL { get; set; }
        /// <summary>
        /// 酒豆类型
        /// </summary>
        public int Direction { get; set; }
        /// <summary>
        /// changedHuoLi
        /// </summary>
        public decimal ChangedHuoLi { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalRecord { get; set; }
        /// <summary>
        /// 变化后的酒豆
        /// </summary>
        public decimal CurrentHuoLi { get; set; }
        /// <summary>
        /// 上次剩余的酒豆
        /// </summary>
        public decimal OriginalHuoLi { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string TradeCode { get; set; }

        private static readonly ObjectSchema<MyHL> _schema;
        public string CreateTime { get; set; }

        public DateTime addTime { get; set; }

        static MyHL()
        {
            _schema = new ObjectSchema<MyHL>();

            _schema.AddField(x => x.userId, "userId");

            _schema.AddField(x => x.countHL, "countHL");

            _schema.AddField(x => x.freezeHL, "freezeHL");

            _schema.AddField(x => x.usableHL, "usableHL");

            _schema.AddField(x => x.Direction, "Direction");

            _schema.AddField(x => x.ChangedHuoLi, "ChangedHuoLi");

            _schema.AddField(x => x.Description, "Description");

            _schema.AddField(x => x.TotalRecord, "TotalRecord");

            _schema.AddField(x => x.CurrentHuoLi, "CurrentHuoLi");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.OriginalHuoLi, "OriginalHuoLi");

            _schema.AddField(x => x.TradeCode, "TradeCode");

            _schema.AddField(x => x.addTime, "addTime");
            _schema.Compile();
        }
    }
}