using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using SFO2O.Supplier.Models;

namespace SFO2O.Supplier.DAO
{
    public class CountryDAL : BaseDao
    {
        public IList<CountryModel> GetCountryList(Models.LanguageEnum languageVersion)
        {
            var sql = @"SELECT KeyName as CountryID
                            ,KeyValue as CountryName
                        FROM dbo.Dics
                        WHERE LanguageVersion=@LanguageVersion and DicType='CountryOfManufacture'";

            var db = DbSFO2OMain;

            var parameters = db.CreateParameterCollection();
            parameters.Append("LanguageVersion", (int)languageVersion);

            return db.ExecuteSqlList<Models.CountryModel>(sql, parameters);
        }

        public IList<ProvinceModel> GetProvinceList(int countryId, Models.LanguageEnum languageVersion)
        {
            var sql = @"SELECT ID
                            ,ProvinceId
                            ,ProvinceName
                            ,LanguageVersion
                            ,CreateTime
                        FROM Province
                        WHERE LanguageVersion=@LanguageVersion and IsDelete=0 and ParentId=@ParentId";

            var db = DbSFO2OMain;

            var parameters = db.CreateParameterCollection();
            parameters.Append("LanguageVersion", (int)languageVersion);
            parameters.Append("ParentId", countryId);

            return db.ExecuteSqlList<Models.ProvinceModel>(sql, parameters);
        }
    }
}
