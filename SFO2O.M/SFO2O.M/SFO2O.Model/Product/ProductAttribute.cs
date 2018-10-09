using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Product
{

    public class ProductAttributeDto
    {
        /// <summary>
        /// 销售属性
        /// </summary>
        IList<SkuAttributeMetadata> SaleAttributeMetadatas { get; set; }
        /// <summary>
        /// 其他附属属性
        /// </summary>
        IList<ProductAttributeMetadata> AttributeMetadatas { get; set; }

    }

    public class SkuAttributeMetadata
    {
        /// <summary>
        /// sku值
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// 获取或者设置可定量。
        /// </summary>
        public int ForOrder { get; set; }

        #region 元数据相关信息
        /// <summary>
        /// 主键值(包含语言版本)[DicId]
        /// </summary>
        public int MetadataId { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string MetadataName { get; set; }

        /// <summary>
        /// 是否主要sku
        /// </summary>
        public bool IsSkuMain { get; set; }

        /// <summary>
        /// dictype 元数据编码（可提供页面硬编码）
        /// </summary>
        public string Code { get; set; }

        #endregion

        public IList<SkuAttribute> SkuAttribute { get; set; }
    }
    public class SkuAttribute
    {
        public int MetadataId { get; set; }
        public string DisplayName { get; set; }

        public string Value { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public int SortValue { get; set; }
    }

    /// <summary>
    /// 产品属性元数据
    /// </summary>
    public class ProductAttributeMetadata
    {
        /// <summary>
        /// 主键值(包含语言版本)[DicId]
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// dictype 元数据编码（可提供页面硬编码）
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 当前包含属性值内容
        /// </summary>
        public IList<ProductAttribute> ProductAttributes { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public int SortValue { get; set; }
    }
    /// <summary>
    /// 自定义属性值
    /// </summary>
    public class ProductAttribute
    {
        public int MetaDataId;
        public string DisplayName { get; set; }

        public string Value { get; set; }
        public int SortValue { get; set; }
    }

}
