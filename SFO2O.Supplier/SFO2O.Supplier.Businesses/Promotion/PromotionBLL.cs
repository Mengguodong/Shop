using SFO2O.Supplier.DAO.Promotion;
using SFO2O.Supplier.Models;
using SFO2O.Supplier.Models.Promotion;
using SFO2O.Supplier.ViewModels.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Businesses.Promotion
{
    public class PromotionBLL
    {
        private PromotionDAL _dal = new PromotionDAL();
        public PageOf<PromotionListModel> GetPromotionList(int supplierId, PromotionQuery query, PageDTO page)
        {
            return _dal.GetPromotionList(supplierId, query, page);
        }

        public PageOf<PromotionSkuListModel> GetSpuulierSkus(int supplierId, string productName, PageDTO page)
        {
            return _dal.GetSpuulierSkus(supplierId, productName, page);
        }

        public List<PromotionInfoModel> GetPromotionSkus(int promotionId, int supplierId)
        {
            return _dal.GetPromotionSkus(promotionId, supplierId);
        }

        public List<PromotionSkuListModel> GetPromotionSkuInfo(List<string> skus)
        {
            return _dal.GetPromotionSkuInfo(skus);
        }

        public void SavePromotion(List<RedisPromotionSpuModel> promptionSpus, PromotionMainInfoModel promotionMainInfo)
        {
            _dal.SavePromotion(promptionSpus, promotionMainInfo);
        }

        public void CanclePromotion(int supplierId, int promotionId)
        {
            _dal.CanclePromotion(supplierId, promotionId);
        }

        public List<PromotionSkuListModel> ViewPromotionSkus(int supplierId, int promotionId)
        {
            return _dal.ViewPromotionSkus(supplierId, promotionId);
        }

        public PromotionMainInfoModel GetPromotionMainModel(int supplierId, int promotionId)
        {
            return _dal.GetPromotionMainModel(supplierId, promotionId);
        }
    }
}
