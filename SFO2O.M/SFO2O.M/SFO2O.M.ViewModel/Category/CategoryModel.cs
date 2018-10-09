using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Category
{
    /// <summary>
    /// 一级分类信息
    /// </summary>
    public class CategoryModel
    {
        public CategoryModel()
        {
            Items = new List<CategoryLevelModel>();
        }

        /// <summary>
        /// 分类id
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 二级分类
        /// </summary>
        public List<CategoryLevelModel> Items { get; set; }

        public string ImagePath { get; set; }
    }

    /// <summary>
    /// 二级分类信息
    /// </summary>
    public class CategoryLevelModel
    {
        public CategoryLevelModel()
        {
            Items = new List<CategoryLevel2Model>();
        }

        /// <summary>
        /// 分类id
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 三级分类
        /// </summary>
        public List<CategoryLevel2Model> Items { get; set; }

    }

    /// <summary>
    /// 三级分类信息
    /// </summary>
    public class CategoryLevel2Model
    {
        public CategoryLevel2Model()
        {
        }

        /// <summary>
        /// 分类id
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 分类图片
        /// </summary>
        public string ImgUrl { get; set; }
    }

    /// <summary>
    /// 一级分类信息（新分类页用）
    /// </summary>
    public class CategoryModelNew
    {
       
        /// <summary>
        /// 分类id
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 三级分类
        /// </summary>
        public List<CategoryLevel2Model> Items { get; set; }

        public string ImgUrl { get; set; }
    }
}
