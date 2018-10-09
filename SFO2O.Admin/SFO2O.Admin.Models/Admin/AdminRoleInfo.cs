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
    /// <summary>
    /// 用户分组
    /// </summary>
    [Serializable]
    [DataContract]
    /// <summary>
    /// SupplierUserInfo
    /// </summary>
    public class AdminRoleInfo
    {
        /// <summary>
        /// id
        /// </summary>
        [DataMember(Name = "id")]
        [Display(Name = "id")]
        public int id { get; set; }

        /// <summary>
        /// RoleName
        /// </summary>
        [DataMember(Name = "RoleName")]
        [Display(Name = "RoleName")]
        public string RoleName { get; set; }

        /// <summary>
        /// IsService
        /// </summary>
        [DataMember(Name = "IsService")]
        [Display(Name = "IsService")]
        public bool IsService { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        public int UserCount { get; set; }

        public IList<int> ModuleIDList { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<AdminRoleInfo> _schema;
        static AdminRoleInfo()
        {
            _schema = new ObjectSchema<AdminRoleInfo>();
            _schema.AddField(x => x.id, "id");

            _schema.AddField(x => x.RoleName, "RoleName");

            _schema.AddField(x => x.IsService, "IsService");

            _schema.AddField(x => x.Status, "Status");
            _schema.Compile();
        }
    }
}
