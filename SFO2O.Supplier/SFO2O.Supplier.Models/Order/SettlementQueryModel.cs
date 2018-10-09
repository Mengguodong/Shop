using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Models
{
    public class SettlementQueryModel
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int SettlementStatus { get; set; }

        public string SettlementCode { get; set; }

        public string OrderCode { get; set; }

        public int SupplierId { get; set; }
    }
}
