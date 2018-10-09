using SFO2O.Admin.DAO.Customer;
using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Businesses.Customer
{
    public class CustomerBll
    {
        protected CustomerDao Dao
        {
            get { return new CustomerDao(); }
        }

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
            return Dao.getOrderList(startTime, endTime, email, mobile, regionCode, pModel);
        }
    }
}
