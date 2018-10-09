using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Shopping
{
    [Serializable]
    [DataContract]
    public class ShoppingCartGatewayEntity
    {
        /// <summary>
        /// SKU
        /// </summary>
        /// <summary>
        /// ProductId
        /// </summary>
        [DataMember(Name = "sku")]
        [Display(Name = "sku")]
        public string sku { get; set; }

        [DataMember(Name = "Gateway")]
        [Display(Name = "Gateway")]
        /// <summary>
        /// 物流贸易规范编码（1：广州 2：宁波）
        /// </summary>
        public int Gateway { get; set; }

         /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ShoppingCartGatewayEntity> _schema;
        static ShoppingCartGatewayEntity()
        {
            _schema = new ObjectSchema<ShoppingCartGatewayEntity>();
            _schema.AddField(x => x.sku, "sku");
            _schema.AddField(x => x.Gateway, "Gateway");
            _schema.Compile();
        }
    }
}
