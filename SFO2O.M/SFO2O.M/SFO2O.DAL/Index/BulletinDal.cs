using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Model;
using SFO2O.Model.Index;

namespace SFO2O.DAL.Index
{
    /// <summary>
    /// 公告部分DAL
    /// </summary>
    public class BulletinDal : BaseDal
    {
        public IList<BulletinEntity> GetBulletinEntities(int top = 0)
        {
            string sql;
            if (top == 0)
            {
                sql = @"SELECT [Id]
      ,[Title]
      ,[Content]
      ,[CreateTime]
      ,[LinkUrl]
      ,[Sort]
      ,[Status]
  FROM  [Bulletin] WHERE Status=1 order by Sort";
            }
            else
            {

                sql = @"SELECT TOP ({0}) [Id] ,[Title]
      ,[Content]
      ,[CreateTime]
      ,[LinkUrl]
      ,[Sort]
      ,[Status]
  FROM  [Bulletin] WHERE Status=1 order by Sort";

                sql = string.Format(sql, top);
            }

            return DbSFO2OMain.ExecuteSqlList<BulletinEntity>(sql);
        }

    }
}
