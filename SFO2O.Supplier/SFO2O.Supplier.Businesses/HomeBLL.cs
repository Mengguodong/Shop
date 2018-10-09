using SFO2O.Supplier.DAO;
using SFO2O.Supplier.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Businesses
{
    public class HomeBLL
    {
        private static readonly HomeDAL homeDAL = new HomeDAL();
        public HomePageViewModel GetHomePageStatistics(int supplierID)
        {
            return homeDAL.GetHomePageStatistics(supplierID);
        }
    }
}
