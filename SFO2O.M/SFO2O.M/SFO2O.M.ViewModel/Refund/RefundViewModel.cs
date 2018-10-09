using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Model.Refund;

namespace SFO2O.M.ViewModel.Refund
{
    public class RefundViewModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecord { get; set; }
        public int PageCount { get; set; }
        public List<RefundModel> RefundList { get; set; }
    }
}
