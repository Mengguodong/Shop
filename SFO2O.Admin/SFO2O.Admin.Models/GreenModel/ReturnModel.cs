using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Models.GreenModel
{

    public class ReturnModel
    {
        public bool IsTrue { get; set; }
        public string Msg { get; set; }
        public UserScore ScoreData { get; set; }
    }
    public class UserScore
    {
        public string UserName { get; set; }
        public decimal Score { get; set; }

        public int Level { get; set; }
    }
}
