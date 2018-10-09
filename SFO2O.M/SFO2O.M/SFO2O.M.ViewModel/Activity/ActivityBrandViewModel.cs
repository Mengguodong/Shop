using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Activity
{
    //专题页品牌视图
    public class ActivityBrandViewModel
    {
        /// <summary>
        /// 品牌ID
        /// </summary>
        public int BrandId { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 品牌描述
        /// </summary>
        public string BrandDescription { get; set; }

        /// <summary>
        /// 品牌页地址
        /// </summary>
        public string BrandLinkURL { get; set; }
    }
}
