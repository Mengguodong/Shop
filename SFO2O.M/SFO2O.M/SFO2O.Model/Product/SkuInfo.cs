using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Product
{
    [Serializable]
    [DataContract]
    public class SkuInfo
    {
        /// <summary>
        /// skuId
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// spu
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
        /// Price
        /// </summary> 
        [DataMember(Name = "Price")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        /// <summary>
        /// BarCode
        /// </summary> 
        [DataMember(Name = "BarCode")]
        [Display(Name = "BarCode")]
        public string BarCode { get; set; }

        /// <summary>
        /// AlarmStockQty
        /// </summary> 
        [DataMember(Name = "AlarmStockQty")]
        [Display(Name = "AlarmStockQty")]
        public int AlarmStockQty { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary> 
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// AuditTime
        /// </summary> 
        [DataMember(Name = "AuditTime")]
        [Display(Name = "AuditTime")]
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// ShelvesTime
        /// </summary> 
        [DataMember(Name = "ShelvesTime")]
        [Display(Name = "ShelvesTime")]
        public DateTime ShelvesTime { get; set; }

        /// <summary>
        /// RemovedTime
        /// </summary> 
        [DataMember(Name = "RemovedTime")]
        [Display(Name = "RemovedTime")]
        public DateTime RemovedTime { get; set; }


        /// <summary>
        /// Status
        /// </summary> 
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// IsOnSaled
        /// </summary> 
        [DataMember(Name = "IsOnSaled")]
        [Display(Name = "IsOnSaled")]
        public bool IsOnSaled { get; set; }

        /// <summary>
        /// Qty
        /// </summary> 
        [DataMember(Name = "Qty")]
        [Display(Name = "Qty")]
        public int Qty { get; set; }
        /// <summary>
        /// 预上架时间
        /// </summary>
        [DataMember(Name = "PreOnSaleTime")]
        [Display(Name = "PreOnSaleTime")]
        public DateTime PreOnSaleTime { get; set; }

        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SkuInfo> _schema;

        static SkuInfo()
        {
            _schema = new ObjectSchema<SkuInfo>();

            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Price, "Price");

            _schema.AddField(x => x.Status, "SendUserId");

            _schema.AddField(x => x.ShelvesTime, "TradeCode");

            _schema.AddField(x => x.RemovedTime, "Title");

            _schema.AddField(x => x.Qty, "Content");

            _schema.AddField(x => x.PreOnSaleTime, "ImagePath");

            _schema.AddField(x => x.IsOnSaled, "Summary");

            _schema.AddField(x => x.CreateTime, "LinkUrl");

            _schema.AddField(x => x.BarCode, "StartTime");

            _schema.AddField(x => x.AuditTime, "EndTime");

            _schema.AddField(x => x.AlarmStockQty, "LongTerm");

            _schema.Compile();
        }
    }
}
