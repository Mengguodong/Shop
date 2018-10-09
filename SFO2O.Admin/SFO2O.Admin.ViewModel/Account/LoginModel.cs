using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel
{
    public class LoginModel
    {
        public String UserName { get; set; }
        public String ErrorInfoForUserName { get; set; }
        public String Password { get; set; }
        public String ErrorInfoForPassword { get; set; }
        public String ReturnUrl { get; set; }
    }
}
