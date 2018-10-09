using SFO2O.Supplier.DAO.Order;
using SFO2O.Supplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Businesses.Order
{
    public class RefundBLL
    {
        RefundDAL dal = new RefundDAL();

        public PageOf<RefundInfoModel> GetRefundOrderList(RefundQueryInfo queryInfo, PageDTO page, LanguageEnum languageVersion, int receiptCountry)
        {
            return dal.GetRefundOrderList(queryInfo, page, languageVersion, receiptCountry);
        }

        public RefundTotalInfo GetRefundTotal(RefundQueryInfo queryInfo, LanguageEnum languageEnum)
        {
            return dal.GetRefundTotal(queryInfo, languageEnum);
        }

        public IList<RefundDetailModel> GetRefundInfos(string refundCode, LanguageEnum languageEnum)
        {
            return dal.GetRefundInfos(refundCode,languageEnum);
        }
    }
}
