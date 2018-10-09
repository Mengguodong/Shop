using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Product
{
    public class HolidayProductAttributeViewModel
    {
        /// <summary>
        /// 毛重：是指产品的重量和包装该产品所需的包装用品的重量之和,即：净重=毛重-皮重(包装物的重量) 
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 净重：是单指产品的重量 
        /// </summary>
        public decimal NetWeight { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Specifications { get; set; }

        /// <summary>
        /// 口味
        /// </summary>
        public string Flavor { get; set; }

    }
}
