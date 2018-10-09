using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Admin.Models.Product
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// ImportErrorModel
    /// </summary>
    public class ImportErrorModel
    {

        /// <summary>
        /// RowNumber
        /// </summary>
        [DataMember(Name = "RowNumber")]
        [Display(Name = "RowNumber")]
        public long RowNumber { get; set; }

        /// <summary>
        /// sku
        /// </summary>
        [DataMember(Name = "sku")]
        [Display(Name = "sku")]
        public string sku { get; set; }

        /// <summary>
        /// message
        /// </summary>
        [DataMember(Name = "message")]
        [Display(Name = "message")]
        public string message { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ImportErrorModel> _schema;
        static ImportErrorModel()
        {
            _schema = new ObjectSchema<ImportErrorModel>();
            _schema.AddField(x => x.RowNumber, "RowNumber");

            _schema.AddField(x => x.sku, "sku");

            _schema.AddField(x => x.message, "message");
            _schema.Compile();
        }
    }
}