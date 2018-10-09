using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Models
{
    public class DropDownList
    {
        public string Id { get; set; }

        public string KeyName { get; set; }

        public Dictionary<string, string> SelectList { get; set; }

        public int Type { get; set; }

        public string SelectedValue { get; set; }

        public bool IsOther { get; set; }
    }
}
