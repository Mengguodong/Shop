using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Search
{
    public class SearchViewModel
    {
        //页码
        public int PageIndex { get; set; }
        //每页条数
        public int PageSize { get; set; }
        //数据总条数
        public int TotalRecord { get; set; }
        //总页数
        public int PageCount { get; set; }
        //品牌
        public List<string> Brands { get; set; }
        //搜索结果集
        public List<ProductSearchModel> Products { get; set; }
    }
}
