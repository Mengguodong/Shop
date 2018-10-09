using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel
{
    public class PromotionQueryModel
    {
        public PromotionQueryModel()
        {
            BeginDate = DateTime.Now.Date;
            EndDate = BeginDate.AddMonths(1);
            PromotionStatusType = -1;
        }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PromotionStatusType { get; set; }
        public int[] PromotionStatus { get; set; }
        public string PromotionName { get; set; }
        public int SupplierID { get; set; }
    }
}
