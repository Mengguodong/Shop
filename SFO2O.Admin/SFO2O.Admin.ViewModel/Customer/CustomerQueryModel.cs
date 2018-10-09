using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel.Customer
{
    public class CustomerQueryModel
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public int CountryArea { get; set; }

    }
}
