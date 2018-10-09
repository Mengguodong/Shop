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
    public class HuoliEntity
    {
        [DataMember(Name = "UserId")]
        [Display(Name = "UserId")]
        public int UserId { get; set; }
        [DataMember(Name = "Huoli")]
        [Display(Name = "Huoli")]
        public decimal Huoli { get; set; }
        [DataMember(Name = "LockedHuoLi")]
        [Display(Name = "LockedHuoLi")]
        public decimal LockedHuoLi { get; set; }

        [DataMember(Name = "HuoLiCurrent")]
        [Display(Name = "HuoLiCurrent")]
        public decimal HuoLiCurrent { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<HuoliEntity> _schema;
        static HuoliEntity()
        {
            _schema = new ObjectSchema<HuoliEntity>();

            _schema.AddField(x => x.UserId, "UserId");

            _schema.AddField(x => x.Huoli, "Huoli");

            _schema.AddField(x => x.LockedHuoLi, "LockedHuoLi");

            _schema.AddField(x => x.HuoLiCurrent, "HuoLiCurrent");

            _schema.Compile();
        }
    }
}
