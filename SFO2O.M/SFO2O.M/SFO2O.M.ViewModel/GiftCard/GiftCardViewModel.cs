using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Model.GiftCard;

namespace SFO2O.M.ViewModel.GiftCard
{
    public class GiftCardViewModel
    {
        //优惠券列表
        public List<GiftCardEntity> GiftCardList { get; set; }

        //页码
        public int PageIndex { get; set; }

        //每页条数
        public int PageSize { get; set; }
        //数据总条数
        public int TotalRecord { get; set; }
        //总页数
        public int PageCount { get; set; }    
    }
}
