using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Account
{
    public class RegisterModel
    {
        /// <summary>
        /// 电话区域码 +86，+852
        /// </summary>
        public string RegionCode { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobileCode { get; set; }
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string ValidCode { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// 推荐人号码
        /// </summary>
        public string ReferrerNumber { get; set; }
    }
}
