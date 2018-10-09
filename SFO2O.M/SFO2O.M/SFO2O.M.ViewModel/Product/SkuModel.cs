using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Product
{
    public class SkuModel
    {

        /// <summary>
        /// Sku
        /// </summary> 
        public string Sku { get; set; }

        /// <summary>
        /// Price
        /// </summary> 
        public decimal Price { get; set; }

        /// <summary>
        /// BarCode
        /// </summary> 
        public string BarCode { get; set; }

        /// <summary>
        /// AlarmStockQty
        /// </summary> 
        public int AlarmStockQty { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary> 
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// AuditTime
        /// </summary> 
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// ShelvesTime
        /// </summary> 
        public DateTime ShelvesTime { get; set; }

        /// <summary>
        /// RemovedTime
        /// </summary> 
        public DateTime RemovedTime { get; set; }

        /// <summary>
        /// TaxRate
        /// </summary> 
        public decimal TaxRate { get; set; }

        /// <summary>
        /// Status
        /// </summary> 
        public int Status { get; set; }

        /// <summary>
        /// IsOnSaled
        /// </summary> 
        public bool IsOnSaled { get; set; }

        /// <summary>
        /// Qty
        /// </summary> 
        public int Qty { get; set; }
    }
}
