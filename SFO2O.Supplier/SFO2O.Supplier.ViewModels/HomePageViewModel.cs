using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.ViewModels
{
    public class HomePageViewModel
    {
        public HomePageStatistics TotalStatistics { get; set; }
        public HomePageStatistics YesterdayStatistics { get; set; }
        public HomePageStatistics PastmonthStatistics { get; set; }
        public Decimal OnSellProductCount { get; set; }
        public IList<ProductSellRank> TopSellCountProductList { get; set; }
    }

    public class HomePageStatistics
    {
        public Decimal SellProductAmount { get; set; }
        public Int32 SellSkuCount { get; set; }
        public Int32 CompleteOrderCount { get; set; }
        public Decimal RefundProductAmount { get; set; }
        public Int32 RefundOrderCount { get; set; }
    }

    public class ProductSellRank
    {
        public String Name { get; set; }
        public DateTime PreOnSaleTime { get; set; }
        public DateTime LastSellDate { get; set; }
        public Int32 OrderCount { get; set; }
        public Int32 SellCount { get; set; }
        public Decimal SellAmount { get; set; }
    }
}
