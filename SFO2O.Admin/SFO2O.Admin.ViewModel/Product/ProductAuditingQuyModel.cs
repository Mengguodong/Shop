using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel.Product
{
    public class ProductAuditingQuyModel
    {
        public DateTime CreateTimeStart { get; set; }

        public DateTime CreateTimeEnd { get; set; }

        public string Spu { get; set; }

        public string Sku { get; set; }

        public string ProductName { get; set; }

        public int ProductStatus { get; set; }

        public int InventoryStatus { get; set; }

        public int IsOnSales { get; set; }

        public int EditType { get; set; }

        public int FstCagegoryId { get; set; }

        public int SndCagegoryId { get; set; }

        public int TrdCagegoryId { get; set; }

        public int ReportStatus { get; set; }

        public int SupplierId { get; set; }

        public int SalesTerritory { get; set; }
    }
}
