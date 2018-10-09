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
    [Serializable]
    [DataContract]
    /// <summary>
    /// SupplierMenuModel
    /// </summary>
    public class SupplierMenuInfo
    {
        /// <summary>
        /// MenuId
        /// </summary>
        [DataMember(Name = "MenuId")]
        [Display(Name = "MenuId")]
        public int MenuId { get; set; }

        /// <summary>
        /// MenuName
        /// </summary>
        [DataMember(Name = "MenuName")]
        [Display(Name = "MenuName")]
        public string MenuName { get; set; }

        /// <summary>
        /// MenuUrl
        /// </summary>
        [DataMember(Name = "MenuUrl")]
        [Display(Name = "MenuUrl")]
        public string MenuUrl { get; set; }

        /// <summary>
        /// MenuUrl
        /// </summary>
        [DataMember(Name = "IsShow")]
        [Display(Name = "IsShow")]
        public bool IsShow { get; set; }

        /// <summary>
        /// DataValue
        /// </summary>
        [DataMember(Name = "DataValue")]
        [Display(Name = "DataValue")]
        public string DataValue { get; set; }

        /// <summary>
        /// ClassName
        /// </summary>
        [DataMember(Name = "ClassName")]
        [Display(Name = "ClassName")]
        public string ClassName { get; set; }

        /// <summary>
        /// MenuDesc
        /// </summary>
        [DataMember(Name = "MenuDesc")]
        [Display(Name = "MenuDesc")]
        public string MenuDesc { get; set; }

        /// <summary>
        /// sortid
        /// </summary>
        [DataMember(Name = "sortid")]
        [Display(Name = "sortid")]
        public int sortid { get; set; }

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
        private static readonly ObjectSchema<SupplierMenuInfo> _schema;
        static SupplierMenuInfo()
        {
            _schema = new ObjectSchema<SupplierMenuInfo>();
            _schema.AddField(x => x.MenuId, "MenuId");

            _schema.AddField(x => x.MenuName, "MenuName");

            _schema.AddField(x => x.MenuUrl, "MenuUrl");

            _schema.AddField(x => x.IsShow, "IsShow");

            _schema.AddField(x => x.DataValue, "DataValue");

            _schema.AddField(x => x.ClassName, "ClassName");

            _schema.AddField(x => x.MenuDesc, "MenuDesc");

            _schema.AddField(x => x.sortid, "sortid");

            _schema.AddField(x => x.Permission, "Permission");

            _schema.AddField(x => x.ParentPermission, "ParentPermission");
            _schema.Compile();
        }
    }
}
