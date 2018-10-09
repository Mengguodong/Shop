using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SFO2O.Model.Order;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SFO2O.EntLib.DataExtensions;

namespace SFO2O.DAL.Order
{
    public class OrderInfoLogDal:BaseDal
    {
        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Add(OrderInfoLogEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into OrderInfoLog(");
            strSql.Append("OrderCode,CurrentStatus,AfterStatus,OperateIp,Remark,CreateBy,CreateTime)");
            strSql.Append(" values (");
            strSql.Append("@OrderCode,@CurrentStatus,@AfterStatus,@OperateIp,@Remark,@CreateBy,@CreateTime)");

            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@OrderCode", entity.OrderCode);
            parameters.Append("@CurrentStatus", entity.CurrentStatus);
            parameters.Append("@AfterStatus", entity.AfterStatus);
            parameters.Append("@OperateIp", entity.OperateIp);
            parameters.Append("@Remark", entity.Remark);
            parameters.Append("@CreateBy", entity.CreateBy);
            parameters.Append("@CreateTime", entity.CreateTime);
            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters) > 0;
        }
    }
}
