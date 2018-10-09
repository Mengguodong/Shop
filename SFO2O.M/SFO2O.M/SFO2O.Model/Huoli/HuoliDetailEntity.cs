using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
namespace SFO2O.Model.Huoli
{
    public class HuoliDetailEntity
    {
        [DataMember(Name = "UserId")]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        [DataMember(Name = "AddHuoLi")]
        [Display(Name = "AddHuoLi")]
        public decimal AddHuoLi { get; set; }

        /// <summary>
        /// DeadLine
        /// </summary>
        [DataMember(Name = "DeadLine")]
        [Display(Name = "DeadLine")]
        public DateTime DeadLine { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<HuoliDetailEntity> _schema;
        static HuoliDetailEntity()
        {
            _schema = new ObjectSchema<HuoliDetailEntity>();

            _schema.AddField(x => x.UserId, "UserId");

            _schema.AddField(x => x.AddHuoLi, "AddHuoLi");

            _schema.AddField(x => x.DeadLine, "DeadLine");

            _schema.Compile();
        }
    }
}
