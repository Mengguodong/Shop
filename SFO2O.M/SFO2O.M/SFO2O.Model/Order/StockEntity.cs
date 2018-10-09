using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Order
{
    public class StockEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        [DataMember(Name = "Sku")]
        [Display(Name = "Sku")]
        public string Sku { get; set; }

        /// <summary>
        /// Qty
        /// </summary>
        [DataMember(Name = "Qty")]
        [Display(Name = "Qty")]
        public int Qty { get; set; }

        /// <summary>
        /// SQty
        /// </summary>
        [DataMember(Name = "SQty")]
        [Display(Name = "SQty")]
        public int SQty { get; set; }
        /// <summary>
        /// MinSQty
        /// </summary>
        [DataMember(Name = "MinSQty")]
        [Display(Name = "MinSQty")]
        public int MinSQty { get; set; }

        /// <summary>
        /// LockedQty
        /// </summary>
        [DataMember(Name = "LockedQty")]
        [Display(Name = "LockedQty")]
        public int LockedQty { get; set; }

        /// <summary>
        /// ForOrderQty
        /// </summary>
        [DataMember(Name = "ForOrderQty")]
        [Display(Name = "ForOrderQty")]
        public int ForOrderQty { get; set; }

        /// <summary>
        /// Updatetime
        /// </summary>
        [DataMember(Name = "Updatetime")]
        [Display(Name = "Updatetime")]
        public DateTime Updatetime { get; set; }

        /// <summary>
        /// Updateby
        /// </summary>
        [DataMember(Name = "Updateby")]
        [Display(Name = "Updateby")]
        public string Updateby { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<StockEntity> _schema;
        static StockEntity()
        {
            _schema = new ObjectSchema<StockEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Qty, "Qty");

            _schema.AddField(x => x.SQty, "SQty");

            _schema.AddField(x => x.MinSQty, "MinSQty");

            _schema.AddField(x => x.LockedQty, "LockedQty");

            _schema.AddField(x => x.ForOrderQty, "ForOrderQty");

            _schema.AddField(x => x.Updatetime, "Updatetime");

            _schema.AddField(x => x.Updateby, "Updateby");
            _schema.Compile();
        }

    }
}
