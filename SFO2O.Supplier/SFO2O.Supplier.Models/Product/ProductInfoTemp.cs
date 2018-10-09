using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Supplier.Models.Product
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// ProductInfoTemp
    /// </summary>
    public class ProductInfoTemp
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// CategoryId
        /// </summary>
        [DataMember(Name = "CategoryId")]
        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }


        /// <summary>
        /// CategoryId
        /// </summary>
        [DataMember(Name = "CategoryNames")]
        [Display(Name = "CategoryNames")]
        public string CategoryNames { get; set; }

        /// <summary>
        /// SupplierId
        /// </summary>
        [DataMember(Name = "SupplierId")]
        [Display(Name = "SupplierId")]
        public int SupplierId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember(Name = "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Tag
        /// </summary>
        [DataMember(Name = "Tag")]
        [Display(Name = "Tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [DataMember(Name = "Price")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember(Name = "Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Brand
        /// </summary>
        [DataMember(Name = "Brand")]
        [Display(Name = "Brand")]
        public string Brand { get; set; }

        /// <summary>
        /// CountryOfManufacture
        /// </summary>
        [DataMember(Name = "CountryOfManufacture")]
        [Display(Name = "CountryOfManufacture")]
        public string CountryOfManufacture { get; set; }

        /// <summary>
        /// SalesTerritory
        /// </summary>
        [DataMember(Name = "SalesTerritory")]
        [Display(Name = "SalesTerritory")]
        public int SalesTerritory { get; set; }

        /// <summary>
        /// Unit
        /// </summary>
        [DataMember(Name = "Unit")]
        [Display(Name = "Unit")]
        public string Unit { get; set; }

        /// <summary>
        /// IsReturn
        /// </summary>
        [DataMember(Name = "IsReturn")]
        [Display(Name = "IsReturn")]
        public int IsReturn { get; set; }

        /// <summary>
        /// MinForOrder
        /// </summary>
        [DataMember(Name = "MinForOrder")]
        [Display(Name = "MinForOrder")]
        public int MinForOrder { get; set; }

        /// <summary>
        /// MinPrice
        /// </summary>
        [DataMember(Name = "MinPrice")]
        [Display(Name = "MinPrice")]
        public decimal MinPrice { get; set; }

        /// <summary>
        /// LanguageVersion
        /// </summary>
        [DataMember(Name = "LanguageVersion")]
        [Display(Name = "LanguageVersion")]
        public int LanguageVersion { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// Createtime
        /// </summary>
        [DataMember(Name = "Createtime")]
        [Display(Name = "Createtime")]
        public DateTime? Createtime { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        [DataMember(Name = "CreateBy")]
        [Display(Name = "CreateBy")]
        public string CreateBy { get; set; }

        /// <summary>
        /// ModifyTime
        /// </summary>
        [DataMember(Name = "ModifyTime")]
        [Display(Name = "ModifyTime")]
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// ModifyBy
        /// </summary>
        [DataMember(Name = "ModifyBy")]
        [Display(Name = "ModifyBy")]
        public string ModifyBy { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ProductInfoTemp> _schema;
        static ProductInfoTemp()
        {
            _schema = new ObjectSchema<ProductInfoTemp>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.CategoryId, "CategoryId");

            _schema.AddField(x => x.CategoryNames, "CategoryNames");

            _schema.AddField(x => x.SupplierId, "SupplierId");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.Tag, "Tag");

            _schema.AddField(x => x.Price, "Price");

            _schema.AddField(x => x.Description, "Description");

            _schema.AddField(x => x.Brand, "Brand");

            _schema.AddField(x => x.CountryOfManufacture, "CountryOfManufacture");

            _schema.AddField(x => x.SalesTerritory, "SalesTerritory");

            _schema.AddField(x => x.Unit, "Unit");

            _schema.AddField(x => x.IsReturn, "IsReturn");

            _schema.AddField(x => x.MinForOrder, "MinForOrder");

            _schema.AddField(x => x.MinPrice, "MinPrice");

            _schema.AddField(x => x.LanguageVersion, "LanguageVersion");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.Createtime, "Createtime");

            _schema.AddField(x => x.CreateBy, "CreateBy");

            _schema.AddField(x => x.ModifyTime, "ModifyTime");

            _schema.AddField(x => x.ModifyBy, "ModifyBy");
            _schema.Compile();
        }
    }
}