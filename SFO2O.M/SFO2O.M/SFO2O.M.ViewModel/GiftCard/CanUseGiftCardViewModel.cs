using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Model.GiftCard;

namespace SFO2O.M.ViewModel.GiftCard
{
    public class CanUseGiftCardViewModel
    {
        #region 共有属性
        /// <summary>
        /// 优惠券ID
        /// </summary>
        public int Id { get; set; }    

        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 优惠券面值
        /// </summary>
        public string CardSum { get; set; }

        /// <summary>
        /// 优惠券类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public string BeginTime { get; set; }

        /// <summary>
        /// 有效期结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 满减条件金额
        /// </summary>
        public decimal FullCutNum { get; set; }

        /// <summary>
        /// 选择某个优惠券之后 对应的可用酒豆值
        /// </summary>
        public decimal Huoli { get; set; }

        /// <summary>
        ///酒豆值 金额
        /// </summary>
        public string Money { get; set; }

        #endregion

        #region 共有方法

        #endregion
    }
}
