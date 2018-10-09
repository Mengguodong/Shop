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
    /// 商家子账号
    /// </summary>
    [Serializable]
    [DataContract]
    /// <summary>
    /// SupplierUserInfo
    /// </summary>
    public class SupplierUserInfo
    {

        /// <summary>
        /// ID
        /// </summary>
        [DataMember(Name = "ID")]
        [Display(Name = "ID")]
        public int ID { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        [DataMember(Name = "UserID")]
        [Display(Name = "UserID")]
        public int UserID { get; set; }

        /// <summary>
        /// SupplierID
        /// </summary>
        [DataMember(Name = "SupplierID")]
        [Display(Name = "SupplierID")]
        public int SupplierID { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        [DataMember(Name = "UserName")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [DataMember(Name = "Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// IsAdmin
        /// </summary>
        [DataMember(Name = "IsAdmin")]
        [Display(Name = "IsAdmin")]
        public int IsAdmin { get; set; }

        /// <summary>
        /// StoreName
        /// </summary>
        [DataMember(Name = "StoreName")]
        [Display(Name = "StoreName")]
        public string StoreName { get; set; }

        /// <summary>
        /// AgentType
        /// </summary>
        [DataMember(Name = "AgentType")]
        [Display(Name = "AgentType")]
        public int AgentType { get; set; }

        /// <summary>
        /// InvitationCode
        /// </summary>
        [DataMember(Name = "InvitationCode")]
        [Display(Name = "InvitationCode")]
        public string InvitationCode { get; set; }

        /// <summary>
        /// NickName
        /// </summary>
        [DataMember(Name = "NickName")]
        [Display(Name = "NickName")]
        public string NickName { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        [DataMember(Name = "Gender")]
        [Display(Name = "Gender")]
        public int Gender { get; set; }

        /// <summary>
        /// ImageUrl
        /// </summary>
        [DataMember(Name = "ImageUrl")]
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        [DataMember(Name = "QQ")]
        [Display(Name = "QQ")]
        public string QQ { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DataMember(Name = "Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        [DataMember(Name = "CreateBy")]
        [Display(Name = "CreateBy")]
        public string CreateBy { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        [DataMember(Name = "UpdateTime")]
        [Display(Name = "UpdateTime")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// UpdateBy
        /// </summary>
        [DataMember(Name = "UpdateBy")]
        [Display(Name = "UpdateBy")]
        public string UpdateBy { get; set; }

        /// <summary>
        /// TradeType
        /// </summary>
        [DataMember(Name = "TradeType")]
        [Display(Name = "TradeType")]
        public int TradeType { get; set; }

        /// <summary>
        /// SupplierType
        /// </summary>
        [DataMember(Name = "SupplierType")]
        [Display(Name = "SupplierType")]
        public int SupplierType { get; set; }

        /// <summary>
        /// CompanyName
        /// </summary>
        [DataMember(Name = "CompanyName")]
        [Display(Name = "CompanyName")]
        public string CompanyName { get; set; }

        /// <summary>
        /// TrueName
        /// </summary>
        [DataMember(Name = "TrueName")]
        [Display(Name = "TrueName")]
        public string TrueName { get; set; }

        /// <summary>
        /// IsUseFund
        /// </summary>
        [DataMember(Name = "IsUseFund")]
        [Display(Name = "IsUseFund")]
        public bool IsUseFund { get; set; }

        /// <summary>
        /// HasSFPayAccount
        /// </summary>
        [DataMember(Name = "HasSFPayAccount")]
        [Display(Name = "HasSFPayAccount")]
        public int HasSFPayAccount { get; set; }

        [DataMember(Name = "LastLoginTime")]
        [Display(Name = "LastLoginTime")]
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// SupplierStatus
        /// </summary>
        [DataMember(Name = "SupplierStatus")]
        [Display(Name = "SupplierStatus")]
        public int SupplierStatus { get; set; }
        /// <summary>
        /// 缓存时间
        /// </summary>
        [DataMember(Name = "CacheTime")]
        public DateTime CacheTime { get; set; }

        public IList<int> RoleIDList { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SupplierUserInfo> _schema;
        static SupplierUserInfo()
        {
            _schema = new ObjectSchema<SupplierUserInfo>();
            _schema.AddField(x => x.ID, "ID");

            _schema.AddField(x => x.UserID, "UserID");

            _schema.AddField(x => x.SupplierID, "SupplierID");

            _schema.AddField(x => x.UserName, "UserName");

            _schema.AddField(x => x.Password, "Password");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.IsAdmin, "IsAdmin");

            _schema.AddField(x => x.StoreName, "StoreName");

            _schema.AddField(x => x.AgentType, "AgentType");

            _schema.AddField(x => x.InvitationCode, "InvitationCode");

            _schema.AddField(x => x.NickName, "NickName");

            _schema.AddField(x => x.Gender, "Gender");

            _schema.AddField(x => x.ImageUrl, "ImageUrl");

            _schema.AddField(x => x.QQ, "QQ");

            _schema.AddField(x => x.Email, "Email");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.CreateBy, "CreateBy");

            _schema.AddField(x => x.UpdateTime, "UpdateTime");

            _schema.AddField(x => x.UpdateBy, "UpdateBy");

            _schema.AddField(x => x.TradeType, "TradeType");

            _schema.AddField(x => x.SupplierType, "SupplierType");

            _schema.AddField(x => x.CompanyName, "CompanyName");

            _schema.AddField(x => x.TrueName, "TrueName");

            _schema.AddField(x => x.IsUseFund, "IsUseFund");

            _schema.AddField(x => x.HasSFPayAccount, "HasSFPayAccount");

            _schema.AddField(x => x.LastLoginTime, "LastLoginTime");

            _schema.AddField(x => x.SupplierStatus, "SupplierStatus");
            _schema.Compile();
        }
    }

    /// <summary>
    /// 查询对象类SupplierQueryInfo 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools
    /// Generate Time: 2013-12-24 09:35:47
    /// </summary>
    public class SupplierUserQueryInfo : SupplierUserInfo
    {
        public SupplierUserQueryInfo() { }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int ID { get; set; }

        public int isAdmin = -1;
        public int IsAdmin
        {
            get
            {
                return isAdmin;
            }
            set
            {
                isAdmin = value;
            }
        }

        public int status = -1;
        public int Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public int SupplierID { get; set; }

        public string NickName { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 商家评级
        /// </summary>
        public int Grade { get; set; }


        public List<int> SupplierIds { get; set; }
    }

    /// <summary>
    /// 用户身份验证结果
    /// </summary>
    public enum ValidateResult
    {
        /// <summary>
        /// 用户名不存在
        /// </summary>
        AccountNotExists = 0,

        /// <summary>
        /// 密码错误
        /// </summary>
        PwdError = 1,

        /// <summary>
        /// 状态不正确，已禁用
        /// </summary>
        StatusError = 2,

        /// <summary>
        /// 验证成功
        /// </summary>
        Success = 3,
    }

    public class GetSupplierUserRequest
    {
        public int? IsAdmin { get; set; }
        public int? Status { get; set; }
        public int? SupplierID { get; set; }
        public int? RoleID { get; set; }
        public int? UserID { get; set; }
        public bool IsGetRoleID { get; set; }
    }
}