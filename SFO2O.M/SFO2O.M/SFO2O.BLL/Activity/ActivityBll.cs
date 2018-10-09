using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SFO2O.Model.Common;
using SFO2O.DAL.Information;
using SFO2O.Utility.Uitl;
using SFO2O.BLL.com.hksmspro.api3;
using SFO2O.Model.Activity;
using SFO2O.DAL.Activity;
using System.Xml;
using System.Configuration;

namespace SFO2O.BLL.Activity
{
    public class ActivityBll
    {
        private static readonly ActivityDal ActivityDal = new ActivityDal();

        public ActivityModel GetActivityByKey(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                ActivityModel model = ActivityDal.GetActivityByKey(key);
                return model;
            }
            return null;
        }
    }
}
