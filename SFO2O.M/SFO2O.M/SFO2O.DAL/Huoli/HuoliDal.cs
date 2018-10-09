using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SFO2O.Model.Order;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SFO2O.EntLib.DataExtensions;

namespace SFO2O.DAL.Huoli
{
    public class HuoliDal : BaseDal
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static void UpdateByLockedHuoli(int UserId,decimal Huoli, Database db, DbTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE HuoLiTotal SET LockedHuoLi=ISNULL(LockedHuoLi,0)+@LockedHuoLi ");
            strSql.Append("WHERE UserId=@UserId");
            var parameters = db.CreateParameterCollection();
            parameters.Append("@UserId", UserId);
            parameters.Append("@LockedHuoLi", Huoli);

            var icount = db.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters, tran);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static void UpdateByLockedForHuoli(int UserId, decimal Huoli, Database db, DbTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE HuoLiTotal SET LockedHuoLi=ISNULL(LockedHuoLi,0)-@LockedHuoLi ");
            strSql.Append("WHERE UserId=@UserId");
            var parameters = db.CreateParameterCollection();
            parameters.Append("@UserId", UserId);
            parameters.Append("@LockedHuoLi", Huoli);

            var icount = db.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters, tran);
        }
    }
}
