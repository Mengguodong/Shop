using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Model.Product;

namespace SFO2O.M.ViewModel.Activity
{
    //专题页产品列表模块
    public class ActivityModule
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 模块地址（主要用于类目模板，每个类目对应的有(更多...)链接）
        /// </summary>
        public string ModuleLinkURL { get; set; }

        /// <summary>
        /// 模块产品
        /// </summary>
        public List<ProductInfoModel> ProductList { get; set; }
    }
}
