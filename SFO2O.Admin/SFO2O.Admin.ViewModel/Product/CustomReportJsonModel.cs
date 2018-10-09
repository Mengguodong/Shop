using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel.Product
{
    public class CustomReportJsonModel
    {
        [JsonProperty("CustomReports")]
        public CustomReport[] CustomReports;
    }

    public class CustomReport
    {

        [JsonProperty("Sku")]
        public string Sku;

        [JsonProperty("CustomsUnit")]
        public string CustomsUnit;

        [JsonProperty("InspectionNo")]
        public string InspectionNo;

        [JsonProperty("HSCode")]
        public string HSCode;

        [JsonProperty("UOM")]
        public string UOM;

        [JsonProperty("PrepardNo")]
        public string PrepardNo;

        [JsonProperty("GnoCode")]
        public string GnoCode;

        [JsonProperty("TaxRate")]
        public string TaxRate;

        [JsonProperty("TaxCode")]
        public string TaxCode;

        [JsonProperty("ModelForCustoms")]
        public string ModelForCustoms;
    }

}
