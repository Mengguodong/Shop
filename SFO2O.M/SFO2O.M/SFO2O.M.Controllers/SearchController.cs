using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SFO2O.Utility.Uitl;
using SFO2O.Utility.Extensions;
using SolrNet;
using SolrNet.Attributes;
using Microsoft.Practices.ServiceLocation;
using SFO2O.BLL.Search;
using SFO2O.M.ViewModel.Search;
using SFO2O.Model.Enum;


namespace SFO2O.M.Controllers
{
    public class SearchController : SFO2OBaseController
    {
        private SearchBll searchBll = new SearchBll();
        private int PageSize = 10;

        /// <summary>
        /// 搜索结果视图Action
        /// </summary>
        public ActionResult Search(string keyword = "")
        {
            ViewBag.Keyword = keyword;
            return View();
        }

        /// <summary>
        ///  根据搜索关键词和排序、筛选条件获取检索数据
        /// </summary>
        /// <param name="keyword">搜索的关键词</param>
        /// <param name="sort">排序条件：1.新品排序 2.价格升序 3.价格降序 4.折扣升序</param>
        /// <param name="brands">筛选品牌：e.g. ["华为"，"小米"，"iphone"]</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="IsFullData">是否根据关键词去Solr服务器取回所有数据</param>
        /// <returns>视图Json对象</returns>
        [HttpPost]
        public JsonResult GetProductListByOptionsAndPageIndex(string keyword, int sort = 1, string brands = "", int pageIndex = 1, bool IsFullData = false)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    keyword = Server.UrlDecode(keyword);
                }
                //直接返回分页数据，减少带宽，加快响应速度
                var model = GetSolrQueryResultsByOptionsAndPageIndex(keyword, sort, brands, pageIndex);
                var products = model.Products == null ? null : model.Products.Select(x => new { x.SPU, x.Name, x.ImagePath, x.DiscountPrice, x.DiscountRate, x.MinPrice, x.Qty,IsHolidayGoods=x.ParentId });
                return Json(
                    new
                    {
                        Type = 1,
                        //Data = model,
                        Data = new
                        {
                            model.PageIndex,
                            model.PageSize,
                            model.PageCount,
                            model.TotalRecord,
                            model.Brands,
                            Products = products
                        }
                    }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
        }

        #region 根据检索条件获取检索数据ViewModel
        /// <summary>
        /// 根据检索条件获取检索数据ViewModel
        /// </summary>
        /// <param name="keyword">搜索的关键词</param>
        /// <param name="sort">排序条件：1.新品排序 2.价格升序 3.价格降序 4.折扣升序</param>
        /// <param name="brands">筛选品牌：e.g. ["华为"，"小米"，"iphone"]</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>视图对象</returns>
        private SearchViewModel GetSolrQueryResultsByOptionsAndPageIndex(string keyword, int sort, string brands, int pageIndex)
        {
            SearchViewModel viewmodel = new SearchViewModel();
            try
            {
                int totalRecord = 0, pageCount = 0;
                var list = searchBll.GetProductListByOptionsAndPageIndex(keyword, sort, brands, pageIndex, PageSize);
                if (list != null)
                {
                    totalRecord = list.NumFound;
                    pageCount = totalRecord / PageSize;
                    if (totalRecord % PageSize > 0)
                    {
                        pageCount += 1;
                    }
                    //处理图片地址和大小 && 以及时令节日产品判断
                    if (list.Count > 0)
                    {
                        var imgServer = ConfigHelper.ImageServer;
                        var holidayCategoryId = ConfigHelper.HolidayCategoryId;
                        list.ForEach(
                            (x) =>
                            {
                                x.ImagePath = string.Format("{0}{1}", imgServer, x.ImagePath.Replace('\\', '/').Replace(".", "_640."));
                                x.ParentId = x.ParentId == holidayCategoryId ? 1 : 0;//用于搜索结果页，只要是搜出来的时令节日产品（如：月饼）不显示购物车图标
                            }
                            );
                    }

                    //最终使用Brand而不是Brand_IK进行分组，解决拆分品牌的问题
                    if (brands == "" && list.FacetFields["Brand"].Count > 0)
                    {
                        viewmodel.Brands = list == null ? null : list.FacetFields["Brand"].Where(x => x.Value > 0).Select(x => x.Key).ToList();
                    }
                    else
                    {
                        viewmodel.Brands = null;//点击品牌进来的，不用返回与关键词有关的所有品牌
                    }
                }

                viewmodel.Products = list;
                viewmodel.PageSize = PageSize;
                viewmodel.PageCount = pageCount;
                viewmodel.TotalRecord = totalRecord;
                viewmodel.PageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                viewmodel.Products = null;
            }
            return viewmodel;
        }
        #endregion

        #region 搜索页
        /// <summary>
        /// 搜索页面
        /// <param name="p">从首页传过来的placeholder</param>
        /// </summary>
        public ActionResult Index(string p)
        {
            //所有热门词汇（默认11个）
            var hotwords = searchBll.GetHotKeywordsFromCache();
            //搜索框中的默认关键词
            var placeholder = string.Empty;
            //随机取出一个热门词汇作为默认关键词
            if (hotwords.Count > 0)
            {
                if (string.IsNullOrEmpty(p))
                {
                    var index = new Random().Next(0, hotwords.Count);
                    placeholder = hotwords[index].Content;
                    hotwords.RemoveAt(index);
                }
                else
                {
                    placeholder = p;
                    hotwords = hotwords.Where(x => x.Content != p.Trim()).ToList();
                }
            }
            ViewBag.HotWords = hotwords;
            ViewBag.PlaceHolder = placeholder;
            return View();
        }
        #endregion

        #region 点击搜索按钮，更新热词搜索记录表 [HttpPost]
        /// <summary>
        /// 点击搜索按钮，更新热词搜索记录表
        /// </summary>
        /// <param name="keyword">关键词</param>
        [HttpPost]
        public JsonResult AddSearchHotWordRecord(string keyword = "", int source = 1)
        {
            try
            {
                int type = searchBll.AddSearchHotWordRecord(keyword, source) ? 1 : 0;
                string content = type == 1 ? "搜索词更新成功!" : "搜索词更新失败!";
                return Json(new { Type = type, Content = content }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0, Content = "搜索词更新失败" }, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region 获取CMS设置的热词
        /// <summary>
        /// 获取CMS设置的热词
        /// </summary>
        /// <returns>设置的热门词汇列表(异步请求2016.07.22)</returns>
        public JsonResult GetHotKeywords(string p)
        {
            //所有热门词汇（默认11个）
            var hotwords = searchBll.GetHotKeywordsFromCache();
            //搜索框中的默认关键词
            var placeholder = string.Empty;
            //随机取出一个热门词汇作为默认关键词
            if (hotwords.Count > 0)
            {
                if (string.IsNullOrEmpty(p))
                {
                    var index = new Random().Next(0, hotwords.Count);
                    placeholder = hotwords[index].Content;
                    hotwords.RemoveAt(index);
                }
                else
                {
                    placeholder = p;
                    hotwords = hotwords.Where(x => x.Content != p.Trim()).ToList();
                }
                //构造json数据返回前端
                var list = hotwords.Select(x => new { hotword = x.Content, isRed = x.IsRed });
                return Json(new { Type = 1, Data = new { placeholder = placeholder, hotwords = list } }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Type = 0, Content = "没有设置关键词" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
