using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFO2O.Model;
using SFO2O.EntLib.DataExtensions;

namespace SFO2O.DAL.Account
{
    public class SupplierDal:BaseDal
    {
        public SupplierEntity GetSupplierBaseInfo(int supplierID)
        {
            string sql = @"select * from Supplier WHERE SupplierID = @SupplierID";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("@supplierId", supplierID, System.Data.DbType.Int32);

            return db.ExecuteSqlFirst<SupplierEntity>(sql, parameters);
        }
    }
}
