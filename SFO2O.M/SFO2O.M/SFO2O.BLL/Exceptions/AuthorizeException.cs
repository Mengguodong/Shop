using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.BLL.Exceptions
{
    public class AuthorizeException : SFO2OException
    {
        public AuthorizeException(string message)
            : base(message)
        { }
    }
}
