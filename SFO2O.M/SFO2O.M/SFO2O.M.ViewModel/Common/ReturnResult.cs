using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Common
{
    public class ReturnResult
    {
        public string ErrorMessage { get; set; }

        public bool Result { get; set; }

        public int ErrorCode { get; set; }
    }
}
