using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Favorite
{
    public class FavoriteViewModel
    {
        public List<SFO2O.Model.Product.Favorite> Products { get; set; }

        public int TotalRecord { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }

    }
}
