using SFO2O.Admin.DAO.Promotion;
using SFO2O.Admin.Models.Promotion;
using SFO2O.Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Businesses.Promotion
{
    public class PromotionBLL
    {
        private PromotionDao dao = new PromotionDao();

        public PageOf<PromotionInfoModel> GetPromotionList(PromotionQueryModel query, int pageSize, int pageIndex)
        {
            return dao.GetPromotionList(query, pageSize, pageIndex);
        }

        public int ChangePromotionStatus(int id, int status)
        {
            return dao.ChangePromotionStatus(id, status);
        }

        public PromotionInfoModel GetPromotionInfoModel(int promotionId)
        {
            return dao.GetPromotionInfoModel(promotionId);
        }

        public List<PromotionSkuInfoModel> GetPromotionSkuList(int promotionId)
        {
            return dao.GetPromotionSkuList(promotionId);
        }
    }
}
