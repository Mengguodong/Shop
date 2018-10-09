using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet.Attributes;

namespace SFO2O.M.ViewModel.Search
{
    public class SearchModel
    {
        [SolrUniqueKey("id")]
        public int id { get; set; }

        [SolrField("name")]
        public string name { get; set; }

        [SolrField("title")]
        public string title { get; set; }

        [SolrField("category")]
        public int category { get; set; }

        [SolrField("content")]
        public string content { get; set; }

        [SolrField("price")]
        public decimal price { get; set; }

        [SolrField("color")]
        public string color { get; set; }

        [SolrField("updatetime")]
        public DateTime updatetime { get; set; }

        [SolrField("orderBy")]
        public int orderBy { get; set; }
    }
}
