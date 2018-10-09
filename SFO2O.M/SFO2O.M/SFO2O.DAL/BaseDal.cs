using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.DAL
{
    public abstract class BaseDal
    {
        /// <summary>
        /// 主库
        /// </summary>
        protected Database DbSFO2OMain
        {
            get
            {
                return EnterpriseLibraryContainer.Current.GetInstance<Database>("ConnStringSFO2OMain");
            }
        }
        /// <summary>
        /// 只读
        /// </summary>
        protected Database DbSFO2ORead
        {
            get
            {
                return EnterpriseLibraryContainer.Current.GetInstance<Database>("ConnStringSFO2ORead");
            }
        }
    }
}
