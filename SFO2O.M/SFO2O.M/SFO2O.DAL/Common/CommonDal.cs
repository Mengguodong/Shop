using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Utility.Uitl;

namespace SFO2O.DAL.Common
{
    public class CommonDal : BaseDal
    {

        /// <summary>
        /// 获取汇率信息
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public decimal GetExchangeRateByPattern(int pattern = 1)
        {
            string sql = @"SELECT TOP 1 [Rate] FROM  [ExchangeRates] WHERE [Status]=1 AND [Pattern]=@Pattern  ORDER BY [UpdateTime],[CreateTime] DESC ";

            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@Pattern", pattern);
                return DbSFO2ORead.ExecuteSqlScalar<decimal>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 获取用户所在地区
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns>1.大陆 2.中华人民共和国大陆地区</returns>
        public int GetUserRegion(int userid)
        {
            string sql = @"select case RegionCode when '86' then 1 else 2 end as 'SendID' from Customer where ID=@UserID";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@UserID", userid);
            return Convert.ToInt32(DbSFO2ORead.ExecuteScalar(CommandType.Text,sql,parameters));
        }
    }
}
