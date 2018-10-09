using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Team
{
    [Serializable]
    [DataContract]
    public class TeamDetailEntity
    {

        /// <summary>
        /// OrderCode
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

        /// <summary>
        /// OrderStatus
        /// </summary>
        [DataMember(Name = "OrderStatus")]
        [Display(Name = "OrderStatus")]
        public int OrderStatus { get; set; }
        
        /// <summary>
        /// TeamCode
        /// </summary>
        [DataMember(Name = "TeamCode")]
        [Display(Name = "TeamCode")]
        public string TeamCode { get; set; }

        /// <summary>
        /// PaidAmount
        /// </summary>
        [DataMember(Name = "PaidAmount")]
        [Display(Name = "PaidAmount")]
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [DataMember(Name = "UserId")]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

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
        /// TeamHead
        /// </summary>
        [DataMember(Name = "TeamHead")]
        [Display(Name = "TeamHead")]
        public int TeamHead { get; set; }

        /// <summary>
        /// TeamNumbers
        /// </summary>
        [DataMember(Name = "TeamNumbers")]
        [Display(Name = "TeamNumbers")]
        public int TeamNumbers { get; set; }

        /// <summary>
        /// sku
        /// </summary>
        [DataMember(Name = "sku")]
        [Display(Name = "sku")]
        public string Sku { get; set; }

        /// <summary>
        /// ImagePath
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// ProductName
        /// </summary>
        [DataMember(Name = "ProductName")]
        [Display(Name = "ProductName")]
        public string ProductName { get; set; }

        /// <summary>
        /// PayTime
        /// </summary>
        [DataMember(Name = "PayTime")]
        [Display(Name = "PayTime")]
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// TeamStatus
        /// </summary>
        [DataMember(Name = "TeamStatus")]
        [Display(Name = "TeamStatus")]
        public int TeamStatus { get; set; }

        
        /// <summary>
        /// StartTime
        /// </summary>
        [DataMember(Name = "StartTime")]
        [Display(Name = "StartTime")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        [DataMember(Name = "EndTime")]
        [Display(Name = "EndTime")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// IsOrderCodeInput
        /// </summary>
        [DataMember(Name = "IsOrderCodeInput")]
        [Display(Name = "IsOrderCodeInput")]
        public int IsOrderCodeInput { get; set; }
        public string MainValue{ get; set; }
        public string SubValue { get; set; }
        public string NetWeightUnit{ get; set; }

        public decimal DiscountPrice { get; set; }

        /// <summary>
        /// PromotionId
        /// </summary>
        [DataMember(Name = "PromotionId")]
        [Display(Name = "PromotionId")]
        public int PromotionId { get; set; }

        /// <summary>
        /// OrderSourceType
        /// </summary>
        [DataMember(Name = "OrderSourceType")]
        [Display(Name = "OrderSourceType")]
        public int OrderSourceType { get; set; }

        /// <summary>
        /// OrderSourceValue
        /// </summary>
        [DataMember(Name = "OrderSourceValue")]
        [Display(Name = "OrderSourceValue")]
        public string OrderSourceValue { get; set; }

        /// <summary>
        /// DividedAmount
        /// </summary>
        [DataMember(Name = "DividedAmount")]
        [Display(Name = "DividedAmount")]
        public decimal DividedAmount { get; set; }
        /// <summary>
        /// MainDicValue
        /// </summary>
        public string MainDicValue{ get; set; }
        /// <summary>
        /// SubDicValue
        /// </summary>
        public string SubDicValue { get; set; }

        /// <summary>
        /// DividedPercent
        /// </summary>
        [DataMember(Name = "DividedPercent")]
        [Display(Name = "DividedPercent")]
        public decimal DividedPercent { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<TeamDetailEntity> _schema;
        static TeamDetailEntity()
        {
            _schema = new ObjectSchema<TeamDetailEntity>();

            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.OrderStatus, "OrderStatus");

            _schema.AddField(x => x.TeamCode, "TeamCode");

            _schema.AddField(x => x.PaidAmount, "PaidAmount");

            _schema.AddField(x => x.UserId, "UserId");

            _schema.AddField(x => x.ImageUrl, "ImageUrl");

            _schema.AddField(x => x.Mobile, "Mobile");

            _schema.AddField(x => x.TeamHead, "TeamHead");

            _schema.AddField(x => x.TeamNumbers, "TeamNumbers");

            _schema.AddField(x => x.Sku, "sku");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.ProductName, "ProductName");

            _schema.AddField(x => x.PayTime, "PayTime");

            _schema.AddField(x => x.TeamStatus, "TeamStatus");

            _schema.AddField(x => x.StartTime, "StartTime");

            _schema.AddField(x => x.EndTime, "EndTime");

            _schema.AddField(x => x.IsOrderCodeInput, "IsOrderCodeInput");
            _schema.AddField(x => x.MainValue, "MainValue");
            _schema.AddField(x => x.SubValue, "SubValue");
            _schema.AddField(x => x.NetWeightUnit, "NetWeightUnit");
            _schema.AddField(x => x.DiscountPrice, "DiscountPrice");
            _schema.AddField(x => x.PromotionId, "PromotionId");
            _schema.AddField(x => x.OrderSourceType, "OrderSourceType");
            _schema.AddField(x => x.OrderSourceValue, "OrderSourceValue");
            _schema.AddField(x => x.DividedAmount, "DividedAmount");
            _schema.AddField(x => x.DividedPercent, "DividedPercent");
            _schema.Compile();
        }

    }
}
