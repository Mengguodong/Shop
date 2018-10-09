using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SFO2O.M.ViewModel.Account
{
    public class LoginUserModel
    {

        /// <summary>
        /// 登录用户ID(后台使用)
        /// </summary>
        public int UserID { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 0   保密   1   男   2   女
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 1为正常用户
        /// </summary>
        public int Status { get; set; }


        /// <summary>
        /// 通知手机
        /// </summary>
        public string Mobile { get; set; }

        public int FirstOrderAuthorize { get; set; }
        /// <summary>
        /// IsPushingInfo
        /// </summary>
        public int IsPushingInfo { get; set; }

        /// <summary>
        /// 用户类型（我的酿造用户10，普通用户0）
        /// </summary>
        public int SourceType { get; set; }
    }
}
