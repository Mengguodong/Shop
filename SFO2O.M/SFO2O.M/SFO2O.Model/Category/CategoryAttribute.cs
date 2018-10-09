using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Category
{
    [Serializable]
    [DataContract]
    public class CategoryAttribute
    {
        

        /// <summary>
        /// 三级分类id
        /// </summary>
        [DataMember(Name = "CategoryId")]
        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        [DataMember(Name = "CategoryName")]
        [Display(Name = "CategoryName")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 属性KeyName
        /// </summary>
        [DataMember(Name = "KeyName")]
        [Display(Name = "KeyName")]
        public string KeyName { get; set; }
        /// <summary>
        /// 筛选属性值 如：Color,Size
        /// </summary>
        [DataMember(Name = "KeyValue")]
        [Display(Name = "KeyValue")]
        public string KeyValue { get; set; }

        //// <summary>
        /// 属性KeyName
        /// </summary>
        [DataMember(Name = "SubKeyName")]
        [Display(Name = "SubKeyName")]
        public string SubKeyName { get; set; }
        /// <summary>
        /// 筛选属性值 如：黑色，白色
        /// </summary>
        [DataMember(Name = "SubKeyValue")]
        [Display(Name = "SubKeyValue")]
        public string SubKeyValue { get; set; }

        [DataMember(Name = "IsSkuMainAttr")]
        [Display(Name = "IsSkuMainAttr")]
        public int IsSkuMainAttr { get; set; }

        [DataMember(Name = "IsSkuAttr")]
        [Display(Name = "IsSkuAttr")]
        public int IsSkuAttr { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<CategoryAttribute> _schema;
        static CategoryAttribute()
        {
            _schema = new ObjectSchema<CategoryAttribute>();


            _schema.AddField(x => x.CategoryId, "CategoryId");

            _schema.AddField(x => x.CategoryName, "CategoryName");

            _schema.AddField(x => x.KeyName, "KeyName");

            _schema.AddField(x => x.KeyValue, "KeyValue");

            _schema.AddField(x => x.SubKeyName, "SubKeyName");

            _schema.AddField(x => x.SubKeyValue, "SubKeyValue");

            _schema.AddField(x => x.IsSkuMainAttr, "IsSkuMainAttr");

            _schema.AddField(x => x.IsSkuAttr, "IsSkuAttr");

            _schema.Compile();
        }
    }
}
