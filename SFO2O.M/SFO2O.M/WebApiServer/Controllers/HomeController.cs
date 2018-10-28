using SFO2O.BLL.Account;
using SFO2O.Model.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebApiServer.Controllers
{
    public class HomeController : ApiController
    {
        AccountBll accountBll = new AccountBll();

        /// <summary>
        /// 电商注册接口
        /// Author：mgd
        /// Date：2018年10月28日20:30:00
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet]
        public bool Register(string mobile,string pwd)
        {
            int userId = 0;
            CustomerEntity entity = new CustomerEntity()

            {
                SourceValue = "",
                SourceType = 0,
                Mobile = mobile,
                UserName = mobile,
                Email ="",
                Password =pwd,
                Gender = 0,
                RegionCode ="+86",
                ChannelId = 0
            };
             userId = accountBll.Insert(entity);

            return userId > 0;


        }
    }
}
