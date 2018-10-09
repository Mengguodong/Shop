using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Account
{
    public class LoginModel
    {
        /// <summary>
        /// 电话区域码 +86，+852
        /// </summary>
        public string RegionCode { get; set; }
        /// <summary>
        /// 用户名(存储用户注册时的手机号)
        /// </summary> 
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary> 
        public string Password { get; set; }

        /// <summary>
        /// 是否需要返回购物车商品数量
        /// </summary>
        public int Cart { get; set; }
    }
}
