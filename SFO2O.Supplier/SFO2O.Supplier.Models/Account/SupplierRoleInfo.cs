using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Models.Account
{
    /// <summary>
    /// 商家用户分组
    /// </summary>
    [Serializable]
    [DataContract]
    /// <summary>
    /// SupplierUserInfo
    /// </summary>
    public class SupplierRoleInfo
    {
        /// <summary>
        /// RoleID
        /// </summary>
        [DataMember(Name = "RoleID")]
        [Display(Name = "RoleID")]
        public int RoleID { get; set; }

        /// <summary>
        /// RoleName
        /// </summary>
        [DataMember(Name = "RoleName")]
        [Display(Name = "RoleName")]
        public string RoleName { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// SupplierID
        /// </summary>
        [DataMember(Name = "SupplierID")]
        [Display(Name = "SupplierID")]
        public int SupplierID { get; set; }

        public int UserCount { get; set; }

        public IList<int> MenuIdList { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SupplierRoleInfo> _schema;
        static SupplierRoleInfo()
        {
            _schema = new ObjectSchema<SupplierRoleInfo>();
            _schema.AddField(x => x.RoleID, "RoleID");

            _schema.AddField(x => x.RoleName, "RoleName");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.SupplierID, "SupplierID");
            _schema.Compile();
        }
    }
}
