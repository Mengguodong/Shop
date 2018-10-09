using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel.Supplier
{
    public class SupplierInfoJsonModel
    {
        [JsonProperty("SupplierID")]
        public int SupplierID;

        [JsonProperty("UserName")]
        public string UserName;

        [JsonProperty("passWord")]
        public string PassWord;

        [JsonProperty("CompanyName")]
        public string CompanyName;

        [JsonProperty("CompanynameSample")]
        public string CompanynameSample;

        [JsonProperty("CompanyNameEnglish")]
        public string CompanyNameEnglish;

        [JsonProperty("ImgPath")]
        public string ImgPath;

        [JsonProperty("Address")]
        public string Address;

        [JsonProperty("AddressSample")]
        public string AddressSample;

        [JsonProperty("AddressEnglish")]
        public string AddressEnglish;

        [JsonProperty("ContactTel")]
        public string ContactTel;

        [JsonProperty("ContactPhone")]
        public string ContactPhone;

        [JsonProperty("ContactFax")]
        public string ContactFax;

        [JsonProperty("ConnectPeople")]
        public string ConnectPeople;

        [JsonProperty("ConnectPeopleSample")]
        public string ConnectPeopleSample;

        [JsonProperty("ConnectPeopleEnglish")]
        public string ConnectPeopleEnglish;

        [JsonProperty("Descript")]
        public string Descript;

        [JsonProperty("DescriptSample")]
        public string DescriptSample;

        [JsonProperty("DescriptEnglish")]
        public string DescriptEnglish;

       
    }
}
