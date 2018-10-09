using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel
{
    public class HomePageViewModel
    {
        public Int32 TotalSupplierCount { get; set; }
        public Int32 TotalConsumeMemberCount { get; set; }
        public Int32 TotalReConsumeMemberCount { get; set; }
        public Int32 TotalSkuCount { get; set; }
        public Int32 TotalSkuSellCount { get; set; }
        public HomePageStatistics TotalStatistics { get; set; }
        public HomePageStatistics YesterdayStatistics { get; set; }
        public HomePageStatistics PastmonthStatistics { get; set; }

        public IList<SupplierSellRank> TopSellCountSupplierList { get; set; }
        public IList<ProductSellRank> TopSellCountProductList { get; set; }
        public IList<ProductReturnRank> TopReturnCountProductList { get; set; }
    }

    public class HomePageStatistics
    {
        public Decimal SellProductAmount { get; set; }
        public Int32 SellSkuCount { get; set; }
        public Int32 CompleteOrderCount { get; set; }
        public Decimal RefundProductAmount { get; set; }
        public Int32 RefundOrderCount { get; set; }
        public Int32 MemberCount { get; set; }
    }

    public class SupplierSellRank
    {
        public String CompanyName { get; set; }
        public Int32 OnSaleCount { get; set; }
        public Int32 SellCount { get; set; }
        public Decimal SellAmount { get; set; }
        public Int32 OrderCount { get; set; }
    }

    public class ProductSellRank
    {
        public String Name { get; set; }
        public String CompanyName { get; set; }
        public Int32 SellCount { get; set; }
        public Decimal SellAmount { get; set; }
        public Int32 OrderCount { get; set; }
    }

    public class ProductReturnRank
    {
        public String Name { get; set; }
        public String CompanyName { get; set; }
        public Int32 ReturnCount { get; set; }
        public Int32 SellCount { get; set; }
    }
}
