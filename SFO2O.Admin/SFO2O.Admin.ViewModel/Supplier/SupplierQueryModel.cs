using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel.Supplier
{
    public class SupplierQueryModel
    {
        public DateTime CreateTimeStart { get; set; }

        public DateTime CreateTimeEnd { get; set; }

        public string CompanyName { get; set; }

        public string AccountName { get; set; }

        public int SupplierStatus { get; set; }
    }
}
