using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFO2O.Model.Category;
using SFO2O.DAL.Category;
using SFO2O.M.ViewModel;
using SFO2O.M.ViewModel.Category;
using SFO2O.BLL.Common;
using SFO2O.Utility.Cache;
using SFO2O.Utility.Uitl;
using SFO2O.Utility;
using SFO2O.DAL.Product;
using System.Data;
using SFO2O.DAL.Supplier;

namespace SFO2O.BLL.Category
{
    public class CategoryBll
    {
        private CategoryDal categoryDal = new CategoryDal();
        private ProductDal product = new ProductDal();
        private BrandDal brand = new BrandDal();

        /// <summary>
        /// 
        /// </summary>
        public CategoryBll()
        {
        }

        /// <summary>
        /// 获取一级分类
        /// </summary>
        /// <returns></returns>
        public List<CategoryModel> GetCategoryList(int language)
        {
            List<CategoryModel> categoryModelList = new List<CategoryModel>();
            try
            {
                categoryModelList = SetCategoryLevelList(GetCategoryModules(language));
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                categoryModelList = new List<CategoryModel>();
            }
            return categoryModelList;
        }

        /// <summary>
        /// 获取缓存分类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        private IList<CategoryEntity> GetCategoryModules(int language)
        {
            try
            {
                var modules = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, ConstClass.RedisKeyCategoryModules, () =>
                {
                    var data = GetCategoryAll(language);
                    return data;
                }, 360);
                return modules;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<CategoryEntity>();
            }
        }

        private IList<CategoryEntity> GetCategoryAll(int language)
        {
            return categoryDal.GetCategoryAll(language);
        }

        /// <summary>
        /// 一级分类
        /// </summary>
        /// <param name="categoryList"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<CategoryModel> SetCategoryLevelList(IList<CategoryEntity> categoryList)
        {
            List<CategoryModel> categoryModelList = new List<CategoryModel>();
            foreach (var item in categoryList.Where(s => s.ParentId == 0).OrderBy(s => s.SortValue))
            {
                categoryModelList.Add(new CategoryModel
                {
                    CategoryId = item.CategoryId,
                    CategoryName = item.CategoryName,
                    Items = SetCategoryLevel1List(categoryList, item.CategoryId)
                });
            }
            return categoryModelList;
        }

        /// <summary>
        /// 二级分类
        /// </summary>
        /// <param name="categoryList"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<CategoryLevelModel> SetCategoryLevel1List(IList<CategoryEntity> categoryList, int categoryId)
        {
            List<CategoryLevelModel> categoryModelList = new List<CategoryLevelModel>();
            foreach (var item in categoryList.Where(s => s.ParentId == categoryId).OrderBy(s => s.SortValue))
            {
                categoryModelList.Add(new CategoryLevelModel
                {
                    CategoryId = item.CategoryId,
                    CategoryName = item.CategoryName,
                    Items = SetCategoryLevel2List(categoryList, item.CategoryId)
                });
            }
            return categoryModelList;
        }


        /// <summary>
        /// 三级级分类
        /// </summary>
        /// <param name="categoryList"></param>
        /// <param name="categoryId">一级级分类id</param>
        /// <returns></returns>
        private List<CategoryLevel2Model> SetCategoryLevel2List(IList<CategoryEntity> categoryList, int categoryId)
        {
            List<CategoryLevel2Model> categoryLevelModelList = new List<CategoryLevel2Model>();

            foreach (var item in categoryList.Where(s => s.ParentId == categoryId).OrderBy(s => s.SortValue))
            {
                categoryLevelModelList.Add(new CategoryLevel2Model
                {
                    CategoryId = item.CategoryId,
                    CategoryName = item.CategoryName,
                    ImgUrl = DomainHelper.GetImageUrl(item.AppImgUrl)
                });
            }

            return categoryLevelModelList;
        }

        /// <summary>
        /// 取三级分类的筛选属性
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<CategoryAttribute> GetCategoryAttribute(int categoryId,int language)
        {
            try
            {
                string cacheName = ConstClass.RedisKey4MPrefix+ "_CategoryAttribute_"+categoryId;
                List<CategoryAttribute> list = RedisCacheHelper.Get<List<CategoryAttribute>>(cacheName);
                //if (list == null)
                //{
                    list = new List<CategoryAttribute>();
                    list = categoryDal.GetCategoryAttribute(categoryId,language).ToList();
                //所有商品都有品牌，是否退货选项
                    list.Insert(0, new CategoryAttribute()
                    {
                        CategoryId = categoryId,
                        CategoryName = list.Count > 0 ? list.First().CategoryName : "",
                        KeyName = "IsReturn",
                        KeyValue = "是否可退货",
                        SubKeyName = "否",
                        SubKeyValue = "否",
                        IsSkuAttr = 0,
                        IsSkuMainAttr = 0
                    });
                    list.Insert(0, new CategoryAttribute() { 
                        CategoryId = categoryId,
                        CategoryName = list.Count > 0 ? list.First().CategoryName : "",
                        KeyName = "IsReturn",
                        KeyValue = "是否可退货",
                        SubKeyName = "是",
                        SubKeyValue = "是",
                        IsSkuAttr = 0,
                        IsSkuMainAttr = 0
                    });
                    

                    string noBrandCategoryIds = ConfigHelper.GetAppSetting<string>("NoBrandCategoryIds");

                    if (!string.IsNullOrEmpty(noBrandCategoryIds) &&
                        !noBrandCategoryIds.Contains(categoryId.ToString()))
                    {
                        DataTable dt = product.GetBrand();
                        if (dt != null)
                        {
                            foreach (DataRow r in dt.Rows)
                            {
                                var brand = new CategoryAttribute()
                                {
                                    CategoryId = categoryId,
                                    CategoryName = list.Count > 0 ? list.First().CategoryName : "",
                                    KeyName = "Brand",
                                    KeyValue = "品牌",
                                    SubKeyName = r["NameCN"].ToString(),
                                    SubKeyValue = r["NameCN"].ToString(),
                                    IsSkuAttr = 0,
                                    IsSkuMainAttr = 0
                                };
                                list.Add(brand);
                            }
                        }
                    }
                   
                    
                   // RedisCacheHelper.Add(cacheName, list, 60);
                //}
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<CategoryAttribute>();
            }
        }
        /// <summary>
        /// 获取分类详细信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public CategoryEntity GetCategoryEntity(int categoryId, int level, int language) 
        {
            CategoryEntity entity = new CategoryEntity();
            var list = GetCategoryModules(language);
            if (list != null)
            {
                entity = list.Where(x => x.CategoryId==categoryId && x.CategoryLevel == level).FirstOrDefault();
            }
            return entity;
        }
        /// <summary>
        /// 获取二级或三级分类列表
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public List<CategoryEntity> GetCategorys(int categoryId, int level, int language)
        {
            List<CategoryEntity> categorylist = new List<CategoryEntity>();
            var list = GetCategoryModules(language);
            if (list != null)
            {
                categorylist = list.Where(x => x.ParentId == categoryId).ToList();

                var currentCategoryModel = list.FirstOrDefault(c => c.CategoryId == categoryId);
                if (currentCategoryModel != null)
                {
                    foreach (var cc in list)
                    {
                        cc.ParentName = currentCategoryModel.CategoryName;
                    }
                }
            }
            return categorylist;
        }
        /// <summary>
        /// 获取一级分类，三级分类
        /// </summary>
        /// <returns></returns>
        public List<CategoryModelNew> GetCategoryList1and3(int language, string firstCategory)
        {
            List<CategoryModelNew> categoryModelList = new List<CategoryModelNew>();
            try
            {
                categoryModelList = new List<CategoryModelNew>(); 
                var categoryList = GetCategoryModules(language);

                List<string> cateIds = firstCategory.Split(',').ToList();
                var otherList = new List<CategoryLevel2Model>();
                foreach (string c in cateIds)
                {
                    var item = categoryList.FirstOrDefault(s => s.ParentId == 0&&s.CategoryId==Convert.ToInt32(c));
                    #region 三级分类
                    var level3List = categoryList.Where(s => s.RootId == item.CategoryId&&s.CategoryLevel==2);
                    var temp3List = new List<CategoryLevel2Model>();
                    foreach (var t in level3List)
                    {
                        temp3List.Add(new CategoryLevel2Model()
                        {
                            CategoryId = t.CategoryId,
                            CategoryName = string.IsNullOrEmpty(t.Aliases) == false ? t.Aliases : t.CategoryName,
                            ImgUrl = t.AppImgUrl.GetImageUrl()
                        });
                    }
                    #endregion

                    categoryModelList.Add(new CategoryModelNew
                    {
                        CategoryId = item.CategoryId,
                        CategoryName = item.CategoryName,
                        Items = temp3List,
                        ImgUrl = item.AppImgUrl.GetImageUrl()
                    });

                    #region 其他分类的三级分类
                    var others = categoryList.Where(s => s.RootId != Convert.ToInt32(c) && s.CategoryLevel == 2).OrderBy(s => s.SortValue);
                   
                    foreach (var t in others)
                    {
                        var exist3Category = otherList.FirstOrDefault(x => x.CategoryId == t.CategoryId);
                        if (exist3Category != null)
                        {
                            continue;
                        }
                        otherList.Add(new CategoryLevel2Model()
                        {
                            CategoryId = t.CategoryId,
                            CategoryName = string.IsNullOrEmpty(t.Aliases) == false ? t.Aliases : t.CategoryName,
                            ImgUrl = t.AppImgUrl.GetImageUrl()
                        });
                    }   
                    #endregion
                }
                categoryModelList.Add(new CategoryModelNew
                {
                    CategoryId = 0,
                    CategoryName = "其他分类",
                    Items = otherList,
                    ImgUrl = string.Empty
                });
                return categoryModelList;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                categoryModelList = new List<CategoryModelNew>();
            }
            return categoryModelList;
        }
    }
}
