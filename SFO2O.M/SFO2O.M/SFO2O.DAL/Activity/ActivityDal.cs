using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Model.Common;
using SFO2O.Utility.Uitl;
using System.Data;
using SFO2O.Model.Activity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;


namespace SFO2O.DAL.Activity
{
    public class ActivityDal:BaseDal
    {
        /// <summary>
        /// 根据专题活动标志Key获取专题对象
        /// </summary>
        /// <param name="key">专题标志，e.g MotherDay</param>
        /// <returns>专题对象</returns>
        public ActivityModel GetActivityByKey(string key)
        {
            try
            {
                string sql = @"select top 1 * from Activity a where a.[Key]=@Key and a.[Status]=1";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@Key", key);

                return DbSFO2ORead.ExecuteSqlFirst<ActivityModel>(sql, parameters); 
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return null;
        }
    }
}
