using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Product
{
    /// <summary>
    /// SPU属性Model
    /// </summary>
    public class SpuAttributeModel
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string AttributeName { get; set; }
        /// <summary>
        /// 展示排序
        /// </summary>
        public int SortValue { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string AttributeValue { get; set; }
        /// <summary>
        /// 属性编码
        /// </summary>
        public string AttributeKey { get; set; }
        /// <summary>
        /// 后缀
        /// </summary>
        public string AttributeValueExt { get; set; }
    }
}
