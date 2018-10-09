using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Supplier
{
    public class StoreIntroModel
    {
        public StoreIntroModel()
        {
            StoreAddress = new List<string>();
        }

        /// <summary>
        /// StoreName
        /// </summary
        public string StoreName { get; set; }

        /// <summary>
        /// StoreIntro
        /// </summary>
        public string StoreIntro { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> StoreAddress { get; set; }
    }
}
