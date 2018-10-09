using SFO2O.Admin.Models.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel.Promotion
{
    public class PromotionDetail
    {
        public PromotionInfoModel PromotionInfo { get; set; }

        public List<PromotionSkuInfoModel> SkuList { get; set; }
    }
}
