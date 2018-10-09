using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model
{
    /// <summary>
    /// SupplierEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class SupplierEntity
    {

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
        /// SupplierType
        /// </summary>
        [DataMember(Name = "SupplierType")]
        [Display(Name = "SupplierType")]
        public int SupplierType { get; set; }

        /// <summary>
        /// TradeType
        /// </summary>
        [DataMember(Name = "TradeType")]
        [Display(Name = "TradeType")]
        public int TradeType { get; set; }

        /// <summary>
        /// BusinessLevel
        /// </summary>
        [DataMember(Name = "BusinessLevel")]
        [Display(Name = "BusinessLevel")]
        public int BusinessLevel { get; set; }

        /// <summary>
        /// Grade
        /// </summary>
        [DataMember(Name = "Grade")]
        [Display(Name = "Grade")]
        public decimal Grade { get; set; }

        /// <summary>
        /// Credit
        /// </summary>
        [DataMember(Name = "Credit")]
        [Display(Name = "Credit")]
        public decimal Credit { get; set; }

        /// <summary>
        /// BrandList
        /// </summary>
        [DataMember(Name = "BrandList")]
        [Display(Name = "BrandList")]
        public string BrandList { get; set; }

        /// <summary>
        /// ContactPerson
        /// </summary>
        [DataMember(Name = "ContactPerson")]
        [Display(Name = "ContactPerson")]
        public string ContactPerson { get; set; }

        /// <summary>
        /// TelPhone
        /// </summary>
        [DataMember(Name = "TelPhone")]
        [Display(Name = "TelPhone")]
        public string TelPhone { get; set; }

        /// <summary>
        /// ContactTel
        /// </summary>
        [DataMember(Name = "ContactTel")]
        [Display(Name = "ContactTel")]
        public string ContactTel { get; set; }

        /// <summary>
        /// Fax
        /// </summary>
        [DataMember(Name = "Fax")]
        [Display(Name = "Fax")]
        public string Fax { get; set; }

        /// <summary>
        /// IdCardsType
        /// </summary>
        [DataMember(Name = "IdCardsType")]
        [Display(Name = "IdCardsType")]
        public int IdCardsType { get; set; }

        /// <summary>
        /// IdCardsNo
        /// </summary>
        [DataMember(Name = "IdCardsNo")]
        [Display(Name = "IdCardsNo")]
        public string IdCardsNo { get; set; }

        /// <summary>
        /// OrganizationNo
        /// </summary>
        [DataMember(Name = "OrganizationNo")]
        [Display(Name = "OrganizationNo")]
        public string OrganizationNo { get; set; }

        /// <summary>
        /// BusinessNo
        /// </summary>
        [DataMember(Name = "BusinessNo")]
        [Display(Name = "BusinessNo")]
        public string BusinessNo { get; set; }

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
        /// RegTime
        /// </summary>
        [DataMember(Name = "RegTime")]
        [Display(Name = "RegTime")]
        public DateTime RegTime { get; set; }

        /// <summary>
        /// EnterTime
        /// </summary>
        [DataMember(Name = "EnterTime")]
        [Display(Name = "EnterTime")]
        public DateTime EnterTime { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        [DataMember(Name = "Latitude")]
        [Display(Name = "Latitude")]
        public decimal Latitude { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        [DataMember(Name = "Longitude")]
        [Display(Name = "Longitude")]
        public decimal Longitude { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// CheckStatus
        /// </summary>
        [DataMember(Name = "CheckStatus")]
        [Display(Name = "CheckStatus")]
        public int CheckStatus { get; set; }

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
        /// Version
        /// </summary>
        [DataMember(Name = "Version")]
        [Display(Name = "Version")]
        public byte[] Version { get; set; }

        /// <summary>
        /// BaiduID
        /// </summary>
        [DataMember(Name = "BaiduID")]
        [Display(Name = "BaiduID")]
        public int BaiduID { get; set; }

        /// <summary>
        /// ServiceAttitude
        /// </summary>
        [DataMember(Name = "ServiceAttitude")]
        [Display(Name = "ServiceAttitude")]
        public decimal ServiceAttitude { get; set; }

        /// <summary>
        /// IsManufacturer
        /// </summary>
        [DataMember(Name = "IsManufacturer")]
        [Display(Name = "IsManufacturer")]
        public int IsManufacturer { get; set; }

        /// <summary>
        /// IsUseFund
        /// </summary>
        [DataMember(Name = "IsUseFund")]
        [Display(Name = "IsUseFund")]
        public Boolean IsUseFund { get; set; }

        /// <summary>
        /// RecommendSupplierId
        /// </summary>
        [DataMember(Name = "RecommendSupplierId")]
        [Display(Name = "RecommendSupplierId")]
        public int RecommendSupplierId { get; set; }

        /// <summary>
        /// AgentId
        /// </summary>
        [DataMember(Name = "AgentId")]
        [Display(Name = "AgentId")]
        public long AgentId { get; set; }

        /// <summary>
        /// IsAuthentication
        /// </summary>
        [DataMember(Name = "IsAuthentication")]
        [Display(Name = "IsAuthentication")]
        public int IsAuthentication { get; set; }

        /// <summary>
        /// ApplyAuthentication
        /// </summary>
        [DataMember(Name = "ApplyAuthentication")]
        [Display(Name = "ApplyAuthentication")]
        public int ApplyAuthentication { get; set; }

        /// <summary>
        /// IsSync
        /// </summary>
        [DataMember(Name = "IsSync")]
        [Display(Name = "IsSync")]
        public int IsSync { get; set; }

        /// <summary>
        /// InvitationCode
        /// </summary>
        [DataMember(Name = "InvitationCode")]
        [Display(Name = "InvitationCode")]
        public string InvitationCode { get; set; }

        /// <summary>
        /// AgentType
        /// </summary>
        [DataMember(Name = "AgentType")]
        [Display(Name = "AgentType")]
        public int AgentType { get; set; }

        /// <summary>
        /// IsSFBussinessMan
        /// </summary>
        [DataMember(Name = "IsSFBussinessMan")]
        [Display(Name = "IsSFBussinessMan")]
        public int IsSFBussinessMan { get; set; }

        /// <summary>
        /// OpenStoreStatus
        /// </summary>
        [DataMember(Name = "OpenStoreStatus")]
        [Display(Name = "OpenStoreStatus")]
        public int OpenStoreStatus { get; set; }

        /// <summary>
        /// OpenStoreTime
        /// </summary>
        [DataMember(Name = "OpenStoreTime")]
        [Display(Name = "OpenStoreTime")]
        public DateTime OpenStoreTime { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SupplierEntity> _schema;
        static SupplierEntity()
        {
            _schema = new ObjectSchema<SupplierEntity>();
            _schema.AddField(x => x.SupplierID, "SupplierID");

            _schema.AddField(x => x.UserName, "UserName");

            _schema.AddField(x => x.CompanyName, "CompanyName");

            _schema.AddField(x => x.TrueName, "TrueName");

            _schema.AddField(x => x.SupplierType, "SupplierType");

            _schema.AddField(x => x.TradeType, "TradeType");

            _schema.AddField(x => x.BusinessLevel, "BusinessLevel");

            _schema.AddField(x => x.Grade, "Grade");

            _schema.AddField(x => x.Credit, "Credit");

            _schema.AddField(x => x.BrandList, "BrandList");

            _schema.AddField(x => x.ContactPerson, "ContactPerson");

            _schema.AddField(x => x.TelPhone, "TelPhone");

            _schema.AddField(x => x.ContactTel, "ContactTel");

            _schema.AddField(x => x.Fax, "Fax");

            _schema.AddField(x => x.IdCardsType, "IdCardsType");

            _schema.AddField(x => x.IdCardsNo, "IdCardsNo");

            _schema.AddField(x => x.OrganizationNo, "OrganizationNo");

            _schema.AddField(x => x.BusinessNo, "BusinessNo");

            _schema.AddField(x => x.QQ, "QQ");

            _schema.AddField(x => x.Email, "Email");

            _schema.AddField(x => x.RegTime, "RegTime");

            _schema.AddField(x => x.EnterTime, "EnterTime");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.Latitude, "Latitude");

            _schema.AddField(x => x.Longitude, "Longitude");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.CheckStatus, "CheckStatus");

            _schema.AddField(x => x.UpdateTime, "UpdateTime");

            _schema.AddField(x => x.UpdateBy, "UpdateBy");

            _schema.AddField(x => x.Version, "Version");

            _schema.AddField(x => x.BaiduID, "BaiduID");

            _schema.AddField(x => x.ServiceAttitude, "ServiceAttitude");

            _schema.AddField(x => x.IsManufacturer, "IsManufacturer");

            _schema.AddField(x => x.IsUseFund, "IsUseFund");

            _schema.AddField(x => x.RecommendSupplierId, "RecommendSupplierId");

            _schema.AddField(x => x.AgentId, "AgentId");

            _schema.AddField(x => x.IsAuthentication, "IsAuthentication");

            _schema.AddField(x => x.ApplyAuthentication, "ApplyAuthentication");

            _schema.AddField(x => x.IsSync, "IsSync");

            _schema.AddField(x => x.InvitationCode, "InvitationCode");

            _schema.AddField(x => x.AgentType, "AgentType");

            _schema.AddField(x => x.IsSFBussinessMan, "IsSFBussinessMan");

            _schema.AddField(x => x.OpenStoreStatus, "OpenStoreStatus");

            _schema.AddField(x => x.OpenStoreTime, "OpenStoreTime");
            _schema.Compile();
        }
    }

}
