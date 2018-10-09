using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Model.Common;
using SFO2O.Utility.Uitl;
using System.Data;
using SFO2O.Model.Information;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using SFO2O.Model.Search;

namespace SFO2O.DAL.Search
{
    public class SearchDal : BaseDal
    {
        /// <summary>
        /// 点击搜索按钮，更新热词搜索记录表
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="source">来源:(1.M站   2.PC  3.APP)默认值1</param>
        public bool AddSearchHotWordRecord(string keyword, int source)
        {
            try
            {
                //将来如果需要区分开来源统计的话
                //需要在where条件里添加source属性：where Keyword=@keyword and DATEDIFF(day,SearchDate,GETDATE())=0) and [Source]=@source
                string sql = @"DECLARE @Id int,@Currdate int
                                        set @Id=(select top 1 ID from HotWordSearchLog where Keyword=@keyword and DATEDIFF(day,SearchDate,GETDATE())=0)
                                        print @Id
                                        if(@Id>0)
                                        begin
                                        --print '有结果:'+cast(@Id as varchar(10))+'开始执行update操作'
                                        update HotWordSearchLog set SearchCount=SearchCount+1 where ID=@Id
                                        end
                                        else
                                        begin
                                        --print '木有结果,开始执行Insert操作'
                                        INSERT INTO [HotWordSearchLog]([Keyword],[SearchDate],[SearchCount],[Source])
                                             VALUES(@keyword,convert(datetime,convert(varchar(10),getdate(),120)),1,@source)
                                        end";
                var parameters = DbSFO2OMain.CreateParameterCollection();
                parameters.Append("@keyword", keyword);
                parameters.Append("@source", source);
                if (DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters) == 1)
                {
                    //如果考虑到关键词搜索数量太大的话，那就不用写这个了，这个log有个好处是能统计某个时段的搜索热门词汇
                    //LogHelper.Info("关键词更新成功！关键词：" + keyword);
                    return true;
                }
                else
                {
                    LogHelper.ErrorMsg("关键词更新失败，关键词：" + keyword);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorMsg(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取CMS设置的热词
        /// </summary>
        /// <returns>设置的热门词汇列表</returns>
        public List<CMSHotKeyword> GetHotKeywords()
        {
            string sql = @"select * from CMSHotKey order by SortValue DESC";
            try
            {
                return DbSFO2ORead.ExecuteSqlList<CMSHotKeyword>(sql).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.ErrorMsg(ex.Message);
                return new List<CMSHotKeyword>();
            }
        }
    }
}
