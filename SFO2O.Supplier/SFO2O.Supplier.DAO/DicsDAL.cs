using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using SFO2O.Supplier.Common;
using SFO2O.Supplier.Models;

namespace SFO2O.Supplier.DAO
{
    public class DicsDAL : BaseDao
    {
        public IList<Models.DicsModel> GetAllDicsInfo(Models.LanguageEnum languageVersion)
        {
            var sql = @"SELECT Id
                            ,KeyName
                            ,KeyValue
                            ,DicType
                            ,LanguageVersion
                        FROM Dics
                        WHERE LanguageVersion=@LanguageVersion";

            var db = DbSFO2OMain;

            var parameters = db.CreateParameterCollection();
            parameters.Append("LanguageVersion", (int)languageVersion);

            return db.ExecuteSqlList<Models.DicsModel>(sql, parameters);
        }

        public IList<Models.DicsModel> GetAllDicsInfo()
        {
            var sql = @"SELECT Id
                            ,KeyName
                            ,KeyValue
                            ,DicType
                            ,LanguageVersion
                        FROM Dics";

            var db = DbSFO2OMain;

            var parameters = db.CreateParameterCollection();

            return db.ExecuteSqlList<Models.DicsModel>(sql, parameters);
        }
    }
}
