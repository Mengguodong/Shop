using SFO2O.Admin.Models.Settlement;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Customer;


namespace SFO2O.Admin.DAO.Customer
{
    public class CustomerDao : BaseDao
    {
        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        /// <param name="regionCode"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public PagedList<CustomerInfo> getOrderList(DateTime? startTime, DateTime? endTime, string email, string mobile, int regionCode, PagingModel pModel)
        {
            var sql = @"
;WITH 
OrderStatistics AS
(
	SELECT  o.UserId,
			ConsumptionAmount = SUM(ISNULL(o.PaidAmount,0)/ISNULL(o.ExchangeRate,0)),
			ConsumptionTimes = COUNT(o.OrderCode)
	FROM	OrderInfo o(NOLOCK)
	WHERE	o.PayTime IS NOT NULL 
			AND ISNULL(o.PaidAmount,0) > 0
	GROUP BY o.UserId
)
SELECT	*
FROM	(
			SELECT	ROW_NUMBER() OVER (ORDER BY c.Createtime) AS RowNumber,
					c.*,
					MemberPoints = 0,
					ConsumptionAmount = ISNULL(os.ConsumptionAmount,0),
					ConsumptionTimes = ISNULL(os.ConsumptionTimes,0)
			FROM	Customer c(NOLOCK)
					LEFT JOIN OrderStatistics os ON os.UserId = c.Id
			WHERE	1=1
					{0}
					{1}
					{2}
					{3}
					{4}
) a
WHERE	1=1
        {5}
SELECT	@TotalCount = COUNT(c.Id)
FROM	Customer c(NOLOCK)		
WHERE	1=1
		{0}
		{1}
		{2}
		{3}
		{4}
";

            sql = string.Format(sql, regionCode > -1 ? " AND c.RegionCode = @RegionCode " : "",
                !string.IsNullOrEmpty(mobile) ? " AND c.Mobile LIKE '%'+@Mobile+'%' " : "",
                !string.IsNullOrEmpty(email) ? " AND c.Email LIKE '%'+@Email+'%' " : "",
                startTime.HasValue ? " AND c.CreateTime >= @StartTime" : "",
                endTime.HasValue ? " AND c.CreateTime <=@EndTime" : "",
                pModel.IsPaging ? " AND a.RowNumber >(@PageIndex-1)*@PageSize AND a.RowNumber <= @PageIndex*@PageSize" : "");

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@RegionCode", regionCode);
            parameters.Append("@Email", email);
            parameters.Append("@Mobile", mobile);
            parameters.Append("@StartTime", startTime.Value);
            parameters.Append("@EndTime", endTime.Value.AddDays(1));
            parameters.Append("@TotalCount", 0, System.Data.DbType.Int32, System.Data.ParameterDirection.InputOutput);

            parameters.Append("@PageIndex", pModel.PageIndex);
            parameters.Append("@PageSize", pModel.PageSize);

            var cList = db.ExecuteSqlList<CustomerInfo>(sql, parameters);
            PagedList<CustomerInfo> result = new PagedList<CustomerInfo>();

            var totalCount = parameters["@TotalCount"].Value != null ? Convert.ToInt32(parameters["@TotalCount"].Value) : 0;
            result.ContentList = cList.ToList();
            result.CurrentIndex = pModel.PageIndex;
            result.PageSize = pModel.PageSize;
            result.RecordCount = totalCount;

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CustomerInfo getMemberInfo(int userId)
        {
            var sql = @"
SELECT	c.Id,
		c.UserName,
		c.NickName,
		c.Password,
		c.ImageUrl,
		c.Mobile,
		c.RegionCode,
		c.Gender,
		c.PayPassword,
		c.Email,
		c.Type,
		c.Status,
		c.FirstOrderAuthorize,
		c.LastLoginTime,
		c.CreateTime,
		c.UpdateBy,
		c.UpdateTime
FROM	Customer c(NOLOCK)
WHERE	c.Id=@UserId
";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@UserId", userId);
            return db.ExecuteSqlFirst<CustomerInfo>(sql, parameters);
        }
    }
}
