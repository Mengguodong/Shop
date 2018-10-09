using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Index
{
    
    /// <summary>
    /// IndexModules
    /// </summary>
    [Serializable]
    [DataContract]
    public class IndexModulesEntity
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [DataMember(Name = "Key")]
        [Display(Name = "Key")]
        public int Key { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [DataMember(Name = "Title")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// Unit
        /// </summary>
        [DataMember(Name = "Unit")]
        [Display(Name = "Unit")]
        public string Unit { get; set; }

        /// <summary>
        /// SubTitle1
        /// </summary>
        [DataMember(Name = "SubTitle1")]
        [Display(Name = "SubTitle1")]
        public string SubTitle1 { get; set; }

        /// <summary>
        /// SubTitle2
        /// </summary>
        [DataMember(Name = "SubTitle2")]
        [Display(Name = "SubTitle2")]
        public string SubTitle2 { get; set; }

        /// <summary>
        /// ImagePath
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// LinkUrl
        /// </summary>
        [DataMember(Name = "LinkUrl")]
        [Display(Name = "LinkUrl")]
        public string LinkUrl { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember(Name = "Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// RefId
        /// </summary>
        [DataMember(Name = "RefId")]
        [Display(Name = "RefId")]
        public string RefId { get; set; }

        /// <summary>
        /// Sort
        /// </summary>
        [DataMember(Name = "Sort")]
        [Display(Name = "Sort")]
        public int Sort { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }
        /// <summary>
        /// 折扣价
        /// </summary>
        [DataMember(Name = "DiscountPrice")]
        [Display(Name = "DiscountPrice")]
        public decimal DiscountPrice { get; set; }

        /// <summary>
        /// 折扣比例
        /// </summary>
        [DataMember(Name = "DiscountRate")]
        [Display(Name = "DiscountRate")]
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// 折扣比例
        /// </summary>
        [DataMember(Name = "MinPrice")]
        [Display(Name = "MinPrice")]
        public decimal MinPrice { get; set; }

        /// <summary>
        /// Spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// ForOrderQty
        /// </summary>
        [DataMember(Name = "ForOrderQty")]
        [Display(Name = "ForOrderQty")]
        public int ForOrderQty { get; set; }

        /// <summary>
        /// MinForOrder
        /// </summary>
        [DataMember(Name = "MinForOrder")]
        [Display(Name = "MinForOrder")]
        public int MinForOrder { get; set; }

        /**首页自定义模块自用属性**/
        /// <summary>
        /// Numid
        /// </summary>
        [DataMember(Name = "Numid")]
        [Display(Name = "Numid")]
        public int Numid { get; set; }

        /// BannerNumid
        /// </summary>
        [DataMember(Name = "BannerNumid")]
        [Display(Name = "BannerNumid")]
        public int BannerNumid { get; set; }

        /// <summary>
        /// SpuIndex
        /// </summary>
        [DataMember(Name = "SpuIndex")]
        [Display(Name = "SpuIndex")]
        public int SpuIndex { get; set; }

        /// <summary>
        /// ModuleId
        /// </summary>
        [DataMember(Name = "ModuleId")]
        [Display(Name = "ModuleId")]
        public int ModuleId { get; set; }

        /// <summary>
        /// ModuleName
        /// </summary>
        [DataMember(Name = "ModuleName")]
        [Display(Name = "ModuleName")]
        public string ModuleName { get; set; }

        /// <summary>
        /// MSubTitle
        /// </summary>
        [DataMember(Name = "MSubTitle")]
        [Display(Name = "MSubTitle")]
        public string MSubTitle { get; set; }

        /// <summary>
        /// CmSortValue
        /// </summary>
        [DataMember(Name = "CmSortValue")]
        [Display(Name = "CmSortValue")]
        public int CmSortValue { get; set; }

        /// <summary>
        /// BannerId
        /// </summary>
        [DataMember(Name = "BannerId")]
        [Display(Name = "BannerId")]
        public int BannerId { get; set; }

        /// <summary>
        /// CbModuleId
        /// </summary>
        [DataMember(Name = "CbModuleId")]
        [Display(Name = "CbModuleId")]
        public int CbModuleId { get; set; }

        /// <summary>
        /// CbTitle
        /// </summary>
        [DataMember(Name = "CbTitle")]
        [Display(Name = "CbTitle")]
        public string CbTitle { get; set; }

        /// <summary>
        /// ImageUrl
        /// </summary>
        [DataMember(Name = "ImageUrl")]
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// CbSortValue
        /// </summary>
        [DataMember(Name = "CbSortValue")]
        [Display(Name = "CbSortValue")]
        public int CbSortValue { get; set; }

        /// <summary>
        /// CbDescription
        /// </summary>
        [DataMember(Name = "CbDescription")]
        [Display(Name = "CbDescription")]
        public string CbDescription { get; set; }

        /// <summary>
        /// CbLinkUrl
        /// </summary>
        [DataMember(Name = "CbLinkUrl")]
        [Display(Name = "CbLinkUrl")]
        public string CbLinkUrl { get; set; }

        /// <summary>
        /// CpProductId
        /// </summary>
        [DataMember(Name = "CpProductId")]
        [Display(Name = "CpProductId")]
        public int CpProductId { get; set; }

        /// <summary>
        /// CpBannerId
        /// </summary>
        [DataMember(Name = "CpBannerId")]
        [Display(Name = "CpBannerId")]
        public int CpBannerId { get; set; }

        /// <summary>
        /// CpSortValue
        /// </summary>
        [DataMember(Name = "CpSortValue")]
        [Display(Name = "CpSortValue")]
        public int CpSortValue { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<IndexModulesEntity> _schema;
        static IndexModulesEntity()
        {
            _schema = new ObjectSchema<IndexModulesEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.Key, "Key");

            _schema.AddField(x => x.Title, "Title");

            _schema.AddField(x => x.Unit, "Unit");

            _schema.AddField(x => x.SubTitle1, "SubTitle1");

            _schema.AddField(x => x.SubTitle2, "SubTitle2");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.LinkUrl, "LinkUrl");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.Description, "Description");

            _schema.AddField(x => x.RefId, "RefId");

            _schema.AddField(x => x.Sort, "Sort");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.DiscountPrice, "DiscountPrice");

            _schema.AddField(x => x.DiscountRate, "DiscountRate");

            _schema.AddField(x => x.MinPrice, "MinPrice");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.ForOrderQty, "ForOrderQty");

            _schema.AddField(x => x.MinForOrder, "MinForOrder");

            _schema.AddField(x => x.Numid, "Numid");

            _schema.AddField(x => x.BannerNumid, "BannerNumid");

            _schema.AddField(x => x.SpuIndex, "SpuIndex");

            _schema.AddField(x => x.ModuleId, "ModuleId");

            _schema.AddField(x => x.ModuleName, "ModuleName");

            _schema.AddField(x => x.MSubTitle, "MSubTitle");

            _schema.AddField(x => x.CmSortValue, "CmSortValue");

            _schema.AddField(x => x.BannerId, "BannerId");

            _schema.AddField(x => x.CbModuleId, "CbModuleId");

            _schema.AddField(x => x.CbTitle, "CbTitle");

            _schema.AddField(x => x.ImageUrl, "ImageUrl");

            _schema.AddField(x => x.CbSortValue, "CbSortValue");

            _schema.AddField(x => x.CbLinkUrl, "CbLinkUrl");

            _schema.AddField(x => x.CpProductId, "CpProductId");

            _schema.AddField(x => x.CpBannerId, "CpBannerId");

            _schema.AddField(x => x.CpSortValue, "CpSortValue");

            _schema.Compile();
        }
    }
}