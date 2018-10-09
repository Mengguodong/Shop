using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.DAO
{
    public abstract class BaseDao
    {
        /// 米兰港WebLog主库
        /// </summary>
        protected Database DbSFO2OWebLogMain
        {
            get
            {
# if DEBUG
                return DbSFO2OWebLogMainTest;
# else
                return EnterpriseLibraryContainer.Current.GetInstance<Database>("ConnStringOfSFO2OLOG");
#endif
            }
        }
        /// <summary>
        /// 米兰港WebLog只读
        /// </summary>
        protected Database DbSFO2OWebLogRead
        {
            get
            {
# if DEBUG
                return DbSFO2OWebLogReadTest;
# else
                return EnterpriseLibraryContainer.Current.GetInstance<Database>("ConnStringOfSFO2OLOG");
#endif
            }
        }
        /// <summary>
        /// 米兰港主库
        /// </summary>
        protected Database DbSFO2OMain
        {
            get
            {
# if DEBUG
                return DbSFO2OMainTest;
# else
                return EnterpriseLibraryContainer.Current.GetInstance<Database>("ConnStringOfSFO2O");
#endif
            }
        }
        /// <summary>
        /// 米兰港只读
        /// </summary>
        protected Database DbSFO2ORead
        {
            get
            {
# if DEBUG
                return DbSFO2OReadTest;
# else
                return EnterpriseLibraryContainer.Current.GetInstance<Database>("ConnStringOfSFO2O");
#endif
            }
        }

        #region Test Connection String
        private static Database DbSFO2OMainTest
        {
            get
            {
                return EnterpriseLibraryContainer.Current.GetInstance<Database>("ConnStringOfSFO2O");
            }
        }
        private static Database DbSFO2OReadTest
        {
            get
            {
                return EnterpriseLibraryContainer.Current.GetInstance<Database>("ConnStringOfSFO2O");
            }
        }

        #endregion


        #region Test Connection String
        private static Database DbSFO2OWebLogMainTest
        {
            get
            {
                return EnterpriseLibraryContainer.Current.GetInstance<Database>("ConnStringOfSFO2OLOG");
            }
        }
        private static Database DbSFO2OWebLogReadTest
        {
            get
            {
                return EnterpriseLibraryContainer.Current.GetInstance<Database>("ConnStringOfSFO2OLOG");
            }
        }


        #endregion
    }
}
