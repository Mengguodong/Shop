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
    /// AdminMenuInfo
    /// </summary>
    public class AdminMenuInfo
    {

        /// <summary>
        /// id
        /// </summary>
        [DataMember(Name = "id")]
        [Display(Name = "id")]
        public int id { get; set; }

        /// <summary>
        /// ModuleName
        /// </summary>
        [DataMember(Name = "ModuleName")]
        [Display(Name = "ModuleName")]
        public string ModuleName { get; set; }

        /// <summary>
        /// ModuleURL
        /// </summary>
        [DataMember(Name = "ModuleURL")]
        [Display(Name = "ModuleURL")]
        public string ModuleURL { get; set; }

        /// <summary>
        /// SortValue
        /// </summary>
        [DataMember(Name = "SortValue")]
        [Display(Name = "SortValue")]
        public int SortValue { get; set; }

        /// <summary>
        /// Icon
        /// </summary>
        [DataMember(Name = "Icon")]
        [Display(Name = "Icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Domain
        /// </summary>
        [DataMember(Name = "Domain")]
        [Display(Name = "Domain")]
        public string Domain { get; set; }

        /// <summary>
        /// IsButton
        /// </summary>
        [DataMember(Name = "IsButton")]
        [Display(Name = "IsButton")]
        public int IsButton { get; set; }

        /// <summary>
        /// IsShow
        /// </summary>
        [DataMember(Name = "IsShow")]
        [Display(Name = "IsShow")]
        public bool IsShow { get; set; }

        /// <summary>
        /// Permission
        /// </summary>
        [DataMember(Name = "Permission")]
        [Display(Name = "Permission")]

        public EnumPermission Permission { get; set; }

        /// <summary>
        /// ParentPermission
        /// </summary>
        [DataMember(Name = "ParentPermission")]
        [Display(Name = "ParentPermission")]
        public EnumPermission ParentPermission { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<AdminMenuInfo> _schema;
        static AdminMenuInfo()
        {
            _schema = new ObjectSchema<AdminMenuInfo>();
            _schema.AddField(x => x.id, "id");

            _schema.AddField(x => x.ModuleName, "ModuleName");

            _schema.AddField(x => x.ModuleURL, "ModuleURL");

            _schema.AddField(x => x.SortValue, "SortValue");

            _schema.AddField(x => x.Icon, "Icon");

            _schema.AddField(x => x.Domain, "Domain");

            _schema.AddField(x => x.IsButton, "IsButton");

            _schema.AddField(x => x.IsShow, "IsShow");

            _schema.AddField(x => x.Permission, "Permission");

            _schema.AddField(x => x.ParentPermission, "ParentPermission");
            _schema.Compile();
        }
    }
}