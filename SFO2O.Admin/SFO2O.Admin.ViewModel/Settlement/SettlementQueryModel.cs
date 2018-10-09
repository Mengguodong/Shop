using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel.Settlement
{
    public class SettlementQueryModel
    {
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int SettlementStatus { get; set; }

        public string SettlementCode { get; set; }

        public string OrderCode { get; set; }

        public string CompanyName { get; set; }

        public int IsFinance { get; set; }

    }
}
