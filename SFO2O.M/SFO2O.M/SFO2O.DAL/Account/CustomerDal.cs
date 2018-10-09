using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFO2O.Model.Account;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Framework.Extensions;

namespace SFO2O.DAL.Account
{
    public class CustomerDal:BaseDal
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName">手机号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public CustomerEntity GetCustomerEntityByUserName(string userName, string password)
        {
            string sql = @"SELECT [CustomerID]
                              ,[CustomerPhone]
                              ,[CustomerPass]
                              ,[CustomerEmail]
                              ,[CustomerType]
                              ,[CustomerStatus]
                              ,[Creater]
                              ,[CreateTime]
                              ,[Updater]
                              ,[UpdateTime]
                          FROM [SFO2O].[dbo].[Customer] (NOLOCK)
                          WHERE CustomerPhone=@CustomerPhone and CustomerPass=@CustomerPass ";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("CustomerPhone", userName);
            parameters.Append("CustomerPass", password);
            return db.ExecuteSqlFirst<CustomerEntity>(sql, parameters);
        }
    }
}
