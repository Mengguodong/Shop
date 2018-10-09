using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Activity
{
    //专题页通用视图
    public class ActivityViewModel
    {
        /// <summary>
        /// 专题关键词，e.g:0701
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 专题标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 专题页面显示头部标题
        /// </summary>
        public string HeadTitle { get; set; }

        /// <summary>
        /// 专题活动描述
        /// </summary>
        public string Discription { get; set; }

        /// <summary>
        /// 专题分享图片地址
        /// </summary>
        public string ImgPath { get; set; }

        /// <summary>
        /// 专题模板类型
        /// </summary>
        public int TempType { get; set; }

        /// <summary>
        /// 专题品牌信息（用于品牌专题模板）
        /// </summary>
        public ActivityBrandViewModel BrandInfo { get; set; }

        /// <summary>
        /// 专题产品模块
        /// </summary>
        public List<ActivityModule> Modules { get; set; }
    }
}
