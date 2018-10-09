using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Admin.Models.Customer
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// CustomerInfo
    /// </summary>
    public class CustomerInfo
    {

        /// <summary>
        /// ID
        /// </summary>
        [DataMember(Name = "ID")]
        [Display(Name = "ID")]
        public int ID { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        [DataMember(Name = "UserName")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// NickName
        /// </summary>
        [DataMember(Name = "NickName")]
        [Display(Name = "NickName")]
        public string NickName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [DataMember(Name = "Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// ImageUrl
        /// </summary>
        [DataMember(Name = "ImageUrl")]
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Mobile
        /// </summary>
        [DataMember(Name = "Mobile")]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// RegionCode
        /// </summary>
        [DataMember(Name = "RegionCode")]
        [Display(Name = "RegionCode")]
        public string RegionCode { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        [DataMember(Name = "Gender")]
        [Display(Name = "Gender")]
        public int Gender { get; set; }

        /// <summary>
        /// PayPassword
        /// </summary>
        [DataMember(Name = "PayPassword")]
        [Display(Name = "PayPassword")]
        public string PayPassword { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DataMember(Name = "Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [DataMember(Name = "Type")]
        [Display(Name = "Type")]
        public int Type { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// FirstOrderAuthorize
        /// </summary>
        [DataMember(Name = "FirstOrderAuthorize")]
        [Display(Name = "FirstOrderAuthorize")]
        public int FirstOrderAuthorize { get; set; }

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
        /// UpdateBy
        /// </summary>
        [DataMember(Name = "UpdateBy")]
        [Display(Name = "UpdateBy")]
        public string UpdateBy { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        [DataMember(Name = "UpdateTime")]
        [Display(Name = "UpdateTime")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// MemberPoints
        /// </summary>
        [DataMember(Name = "MemberPoints")]
        [Display(Name = "MemberPoints")]
        public int MemberPoints { get; set; }

        /// <summary>
        /// ConsumptionAmount
        /// </summary>
        [DataMember(Name = "ConsumptionAmount")]
        [Display(Name = "ConsumptionAmount")]
        public decimal ConsumptionAmount { get; set; }

        /// <summary>
        /// ConsumptionTimes
        /// </summary>
        [DataMember(Name = "ConsumptionTimes")]
        [Display(Name = "ConsumptionTimes")]
        public int ConsumptionTimes { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<CustomerInfo> _schema;
        static CustomerInfo()
        {
            _schema = new ObjectSchema<CustomerInfo>();
            _schema.AddField(x => x.ID, "ID");

            _schema.AddField(x => x.UserName, "UserName");

            _schema.AddField(x => x.NickName, "NickName");

            _schema.AddField(x => x.Password, "Password");

            _schema.AddField(x => x.ImageUrl, "ImageUrl");

            _schema.AddField(x => x.Mobile, "Mobile");

            _schema.AddField(x => x.RegionCode, "RegionCode");

            _schema.AddField(x => x.Gender, "Gender");

            _schema.AddField(x => x.PayPassword, "PayPassword");

            _schema.AddField(x => x.Email, "Email");

            _schema.AddField(x => x.Type, "Type");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.FirstOrderAuthorize, "FirstOrderAuthorize");

            _schema.AddField(x => x.LastLoginTime, "LastLoginTime");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.UpdateBy, "UpdateBy");

            _schema.AddField(x => x.UpdateTime, "UpdateTime");

            _schema.AddField(x => x.MemberPoints, "MemberPoints");

            _schema.AddField(x => x.ConsumptionAmount, "ConsumptionAmount");

            _schema.AddField(x => x.ConsumptionTimes, "ConsumptionTimes");
            _schema.Compile();
        }
    }
}