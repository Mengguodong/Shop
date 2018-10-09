using SFO2O.Admin.DAO;
using SFO2O.Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Businesses
{
    public class HomeBLL
    {
        private static readonly HomeDAL homeDAL = new HomeDAL();
        public HomePageViewModel GetHomePageStatistics()
        {
            return homeDAL.GetHomePageStatistics();
        }

        public List<SupplierSellRank> GetTopSupplierSellRank()
        {
            return homeDAL.GetTopSupplierSellRank();
        }
        public List<ProductSellRank> GetTop50ProductSellRank()
        {
            return homeDAL.GetTop50ProductSellRank();
        }
        public List<ProductReturnRank> GetTop50ProductReturnRank()
        {
            return homeDAL.GetTop50ProductReturnRank();
        }
    }
}
