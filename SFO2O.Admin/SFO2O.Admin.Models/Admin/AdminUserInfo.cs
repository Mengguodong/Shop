using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Admin.Models.Admin
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// AdminUserInfo
    /// </summary>
    public class AdminUserInfo
    {

        /// <summary>
        /// id
        /// </summary>
        [DataMember(Name = "id")]
        [Display(Name = "id")]
        public int id { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DataMember(Name = "Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        [DataMember(Name = "UserName")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// password
        /// </summary>
        [DataMember(Name = "password")]
        [Display(Name = "password")]
        public string password { get; set; }

        /// <summary>
        /// IsAdmin
        /// </summary>
        [DataMember(Name = "IsAdmin")]
        [Display(Name = "IsAdmin")]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// TrueName
        /// </summary>
        [DataMember(Name = "TrueName")]
        [Display(Name = "TrueName")]
        public string TrueName { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// HaveUpdate
        /// </summary>
        [DataMember(Name = "HaveUpdate")]
        [Display(Name = "HaveUpdate")]
        public bool HaveUpdate { get; set; }

        /// <summary>
        /// HaveDelet
        /// </summary>
        [DataMember(Name = "HaveDelet")]
        [Display(Name = "HaveDelet")]
        public bool HaveDelet { get; set; }

        /// <summary>
        /// LastLoginTime
        /// </summary>
        [DataMember(Name = "LastLoginTime")]
        [Display(Name = "LastLoginTime")]
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// createby
        /// </summary>
        [DataMember(Name = "createby")]
        [Display(Name = "createby")]
        public string createby { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        [DataMember(Name = "UpdateTime")]
        [Display(Name = "UpdateTime")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// updateby
        /// </summary>
        [DataMember(Name = "updateby")]
        [Display(Name = "updateby")]
        public string updateby { get; set; }
        /// <summary>
        /// 缓存时间
        /// </summary>
        [DataMember(Name = "CacheTime")]
        public DateTime CacheTime { get; set; }

        public IList<int> RoleIDList { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<AdminUserInfo> _schema;
        static AdminUserInfo()
        {
            _schema = new ObjectSchema<AdminUserInfo>();
            _schema.AddField(x => x.id, "id");

            _schema.AddField(x => x.Email, "Email");

            _schema.AddField(x => x.UserName, "UserName");

            _schema.AddField(x => x.password, "password");

            _schema.AddField(x => x.IsAdmin, "IsAdmin");

            _schema.AddField(x => x.TrueName, "TrueName");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.HaveUpdate, "HaveUpdate");

            _schema.AddField(x => x.HaveDelet, "HaveDelet");

            _schema.AddField(x => x.LastLoginTime, "LastLoginTime");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.createby, "createby");

            _schema.AddField(x => x.UpdateTime, "UpdateTime");

            _schema.AddField(x => x.updateby, "updateby");
            _schema.Compile();
        }
    }
}