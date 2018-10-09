using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Model.Product
{
    public class Favorite
    {
        /// <summary>
        /// id  主键
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// id spu
        /// </summary>
        public string spu { get; set; }
        /// <summary>
        /// userId
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// originalPrice
        /// </summary>
        public decimal originalPrice { get; set; }
        /// <summary>
        /// isDelete
        /// </summary>
        public int isDelete { get; set; }
        /// <summary>
        /// createTime
        /// </summary>
        public DateTime createTime { get; set; }
        /// <summary>
        /// BrandId
        /// </summary>
        public int BrandId { get; set; }
        /// <summary>
        /// price
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// price
        /// </summary>
        public string productName { get; set; }
        /// <summary>
        /// price
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// price
        /// </summary>
        public int rindex { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        [DataMember(Name = "TotalRecord")]
        [Display(Name = "TotalRecord")]
        public int TotalRecord { get; set; }
        /// <summary>
        /// skuStatus
        /// </summary>
        public int skuStatus { get; set; }
        /// <summary>
        /// 是否是打折
        /// </summary>
        public int isDiscount{ get; set; }
        /// <summary>
        /// 打折的钱数
        /// </summary>
        public decimal DiscountPrice { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int fitype { get; set; }
        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<Favorite> _schema;
        static Favorite()
        {
            _schema = new ObjectSchema<Favorite>();
            _schema.AddField(x => x.id, "id");
            _schema.AddField(x => x.spu, "spu");
            _schema.AddField(x => x.userId, "userId");
            _schema.AddField(x => x.originalPrice, "originalPrice");
            _schema.AddField(x => x.isDelete, "isDelete");
            _schema.AddField(x => x.createTime, "createTime");
            _schema.AddField(x => x.BrandId, "BrandId");
            _schema.AddField(x => x.price, "price");
            _schema.AddField(x => x.productName, "productName");
            _schema.AddField(x => x.ImagePath, "ImagePath");
            _schema.AddField(x => x.rindex, "rindex");
            _schema.AddField(x => x.TotalRecord, "TotalRecord");
            _schema.AddField(x => x.skuStatus, "skuStatus");
            _schema.AddField(x => x.isDiscount, "isDiscount");
            _schema.AddField(x => x.DiscountPrice, "DiscountPrice");
            _schema.AddField(x => x.fitype, "fitype");
            _schema.Compile();
        }
    }
}