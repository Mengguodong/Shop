using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Model.Shopping;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Utility.Extensions;
using SFO2O.Model.Common;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Source;

namespace SFO2O.DAL.Source
{
    public class SourceDal:BaseDal
    {
        /// <summary>
        /// 获取所有省份
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public SourceEntity GetSourcePercentById(int cid)
        {
            string sql = "SELECT [Id],[OrderSourceType],[DividedPercent],[Status],[ChannelNo],[ChannelName],[CreateTime],[CreateBy] FROM [DividedPercent] WHERE id=@cid";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@cid", cid);
            return DbSFO2ORead.ExecuteSqlFirst<SourceEntity>(sql, parameters);
        }
    }
}
