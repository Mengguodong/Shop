using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Supplier
{
    public class StoreInfoModel
    {
        /// <summary>
        /// SupplierID
        /// </summary
        public int SupplierID { get; set; }

        /// <summary>
        /// StoreName
        /// </summary
        public string StoreName { get; set; }

        /// <summary>
        /// Address
        /// </summary
        public string Address { get; set; }


        /// <summary>
        /// Address_Sample
        /// </summary
        public string Address_Sample { get; set; }

        /// <summary>
        /// Address_English
        /// </summary
        public string Address_English { get; set; }

        /// <summary>
        /// StoreLogoPath
        /// </summary
        public string StoreLogoPath { get; set; }

        /// <summary>
        /// StoreBannerPath
        /// </summary>
        public string StoreBannerPath { get; set; }

        /// <summary>
        /// StorePageDesc
        /// </summary>
        public string StorePageDesc { get; set; }
    }
}
