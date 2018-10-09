using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet;
using SolrNet.Attributes;
using Microsoft.Practices.ServiceLocation;
using SFO2O.M.ViewModel.Search;
using SolrNet.Commands.Parameters;
using SFO2O.DAL.Search;
using SFO2O.Model.Search;
using SFO2O.Utility.Uitl;
using SFO2O.Utility.Cache;
using SFO2O.BLL.Common;
using System.Text.RegularExpressions;
using SFO2O.Utility.Uitl;

namespace SFO2O.BLL.Search
{
    public class SearchBll
    {
        private SearchDal searchDal = new SearchDal();

        /// <summary>
        /// 根据搜索关键词和排序、筛选条件获取(第N页数据)检索数据
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <param name="sort">排序条件：1.新品排序 2.价格升序 3.价格降序 4.折扣升序</param>
        /// <param name="brands">筛选品牌：e.g. ["华为"，"小米"，"iphone"]</param>
        /// <returns>检索结果集合</returns>  
        public SolrQueryResults<ProductSearchModel> GetProductListByOptionsAndPageIndex(string keyword, int sort, string brands, int pageIndex, int pageSize)
        {
            //创建solr对象
            ISolrOperations<ProductSearchModel> solr = ServiceLocator.Current.GetInstance<ISolrOperations<ProductSearchModel>>();

            //判断参数是否为空or所有
            if (!string.IsNullOrEmpty(keyword)&&keyword.Trim()!="*")
            {
                keyword = keyword.Trim().Replace(@"\"," ");
                keyword = StringVerify.StringToPattern(keyword);//转义这些Solr特殊字符 + - && || ! ( ) { } [ ] ^ ” ~ * ? : \
                //string[] keywords = keyword.Split(' ');
                //创建条件集合
                QueryOptions options = new QueryOptions();
                options.Rows = pageSize;
                options.Start = pageSize * (pageIndex - 1);

                List<ISolrQuery> query = new List<ISolrQuery>();

                //构建OR关系条件
                List<ISolrQuery> keyOr = new List<ISolrQuery>();
                //if (keywords.Length > 0)
                //{
                //    foreach (var k in keywords)
                //    {
                //        if (!string.IsNullOrEmpty(k.Trim()))
                //        {
                //            keyOr.Add(new SolrQuery("Name_IK:" + k));
                //            keyOr.Add(new SolrQuery("searchText:" + k));
                //        }
                //    }
                //}
                //else
                //{
                //    keyword=keyword.Replace(" ", "");
                //    keyOr.Add(new SolrQuery("Name_IK:" +keywords));
                //    keyOr.Add(new SolrQuery("searchText:" + keywords));
                //}

                keyOr.Add(new SolrQuery("Name_IK:" + keyword));
                keyOr.Add(new SolrQuery("searchText:" + keyword));
                var ko = new SolrMultipleCriteriaQuery(keyOr, "OR");
                query.Add(ko);

                //排序条件
                switch (sort)
                {
                    case 1:
                        //设定查询结果的排序，按照上架时间降序.[2016.7.20修改需求，上新排序改为默认的权重排序]
                        //options.AddOrder(new SolrNet.SortOrder("OnSaleTime", SolrNet.Order.DESC));
                        break;
                    case 2:
                        //按照打折后的价格升序.
                        options.AddOrder(new SolrNet.SortOrder("DiscountPrice", SolrNet.Order.ASC));
                        break;
                    case 3:
                        //按照打折后的价格降序.
                        options.AddOrder(new SolrNet.SortOrder("DiscountPrice", SolrNet.Order.DESC));
                        break;
                    case 4:
                        //按照折扣升序.（1折、2折、3折...10折）
                        options.AddOrder(new SolrNet.SortOrder("DiscountRate", SolrNet.Order.ASC));
                        break;
                    default:
                        break;
                }
                //品牌筛选
                SolrQueryResults<ProductSearchModel> products;
                if (brands != "")
                {
                    var blist = JsonHelper.ToObject<List<string>>(brands);
                    //品牌筛选（&&操作）由于传过来的值肯定都是完整的品牌名称
                    //所以不用去Brand_IK中检索，而是直接去Brank中匹配
                    //构建OR关系条件
                    List<ISolrQuery> brandOr = new List<ISolrQuery>();
                    for (int i = 0; i < blist.Count; i++)
                    {
                        blist[i]=StringVerify.StringToPattern(blist[i]);//转义这些Solr特殊字符 + - && || ! ( ) { } [ ] ^ ” ~ * ? : \
                        brandOr.Add(new SolrQuery("Brand:" + blist[i]));
                    }
                    //foreach (var brand in blist)
                    //{
                    //    brandOr.Add(new SolrQuery("Brand:" + brand));
                    //}
                    var bo = new SolrMultipleCriteriaQuery(brandOr, "OR");
                    query.Add(bo);

                    //最终跟Query主条件进行构建 AND 关系条件
                    var querys = new SolrMultipleCriteriaQuery(query, "AND");
                    products = solr.Query(querys, options);
                }
                else
                {
                    //输入关键词，检索数据，顺便给品牌分组,返回品牌数组
                    var facet = new FacetParameters
                    {
                        Queries = new[] { new SolrFacetFieldQuery("Brand") }
                    };
                    options.Facet = facet;
                    products = solr.Query(ko, options);
                }
                return products;
            }
            return null;
        }
       
        /// <summary>
        /// 点击搜索按钮，更新热词搜索记录表
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="source">来源:(1.M站   2.PC  3.APP)默认值1</param>
        public bool AddSearchHotWordRecord(string keyword, int source)
        {
            if (keyword != "")
            {
                return searchDal.AddSearchHotWordRecord(keyword, source);
            }
            return false;
        }

        /// <summary>
        /// 获取CMS设置的热词
        /// </summary>
        /// <returns>设置的热门词汇列表</returns>
        public List<CMSHotKeyword> GetHotKeywordsFromCache(int top = 0)
        {
            try
            {
                var hotwords = RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, ConstClass.RedisKeyHotWords, () =>
                {
                    return searchDal.GetHotKeywords();
                },1);

                //不传top值默认获取全部数据
                return top==0?hotwords:hotwords.Take(top).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<CMSHotKeyword>();
            }
        }
    }
}
