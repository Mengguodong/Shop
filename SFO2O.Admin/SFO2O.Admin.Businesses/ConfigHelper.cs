using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Businesses
{
    public static class ConfigHelper
    {
        public static string ImageServer
        {
            get
            {
                return ConfigurationManager.AppSettings["ImageServer"];
            }
        }
    }
}
