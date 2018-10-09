using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet.Attributes;

namespace SFO2O.M.ViewModel.Search
{
    /// <summary>
    /// 检索返回的产品数据对象
    /// </summary>
    public class ProductSearchModel
    {
        //商品SPU，商品名称Name,品牌BrandId,品牌名称Brand,折扣率DiscountRate,商品单价Price(取sku最小价格？MinPrice),商品详情Description,打折后的价钱DiscountPrice,商品数量Qty,商品创建时间CreateTime,商品上架时间OnSaleTime,商品类目CategoryId和名称CategoryName,近义词和同义词SimilarKeyword             
        [SolrUniqueKey("SPU")]
        public string SPU { get; set; }

        //商品名称Name
        [SolrField("Name")]
        public string Name { get; set; }

        //商品图片地址
        [SolrField("ImagePath")]
        public string ImagePath { get; set; }

        //品牌Id
        [SolrField("BrandId")]
        public int BrandId { get; set; }

        //品牌名称Brand
        [SolrField("Brand")]
        public string Brand { get; set; }

        //折扣率DiscountRate
        [SolrField("DiscountRate")]
        public decimal DiscountRate { get; set; }

        //折扣价DiscountPrice
        [SolrField("DiscountPrice")]
        public decimal DiscountPrice { get; set; }

        //商品单价Price(取sku最小价格：MinPrice)
        [SolrField("MinPrice")]
        public decimal MinPrice { get; set; }

        //商品详情Description
        [SolrField("Description")]
        public string Description { get; set; }

        //商品数量Qty
        [SolrField("Qty")]
        public int Qty { get; set; }

        //商品上架时间OnSaleTime
        [SolrField("OnSaleTime")]
        public DateTime OnSaleTime { get; set; }
        
        //商品类目CategoryId
        [SolrField("CategoryId")]
        public int CategoryId { get; set; }

        //商品类目名称CategoryName
        [SolrField("CategoryName")]
        public string CategoryName { get; set; }

        //近义词和同义词SimilarKeyword,比如：中国：zhongguo,cn    倩碧：Clinique，qianbi
        [SolrField("Tag")]
        public string Tag { get; set; }

        //商品的二级目录分类ID=>ParentId
        [SolrField("ParentId")]
        public int ParentId { get; set; } 
    }

    /// <summary>
    /// 索引库插入的产品数据对象
    /// </summary>
    public class SolrProductSearchModel
    {
        //产品SPU
        [SolrUniqueKey("SPU")]
        public string SPU { get; set; }

        //商品名称Name
        [SolrField("Name")]
        public string Name { get; set; }

        //商品IK名称Name
        [SolrField("Name_IK")]
        public string Name_IK { get; set; }

        //商品图片地址
        [SolrField("ImagePath")]
        public string ImagePath { get; set; }

        //品牌Id
        [SolrField("BrandId")]
        public int BrandId { get; set; }

        //品牌IK名称Brand
        [SolrField("Brand_IK")]
        public string Brand_IK { get; set; }

        //品牌名称Brand
        [SolrField("Brand")]
        public string Brand { get; set; }

        //折扣率DiscountRate
        [SolrField("DiscountRate")]
        public decimal DiscountRate { get; set; }

        //折扣价DiscountPrice
        [SolrField("DiscountPrice")]
        public decimal DiscountPrice { get; set; }

        //商品单价Price(取sku最小价格：MinPrice)
        [SolrField("MinPrice")]
        public decimal MinPrice { get; set; }

        //商品详情Description
        [SolrField("Description")]
        public string Description { get; set; }

        //商品IK详情Description
        [SolrField("Description_IK")]
        public string Description_IK { get; set; }

        //商品数量Qty
        [SolrField("Qty")]
        public int Qty { get; set; }

        //商品上架时间OnSaleTime
        [SolrField("OnSaleTime")]
        public DateTime OnSaleTime { get; set; }

        //商品类目CategoryId
        [SolrField("CategoryId")]
        public int CategoryId { get; set; }

        //商品类目名称CategoryName
        [SolrField("CategoryName")]
        public string CategoryName { get; set; }

        //商品类目IK名称CategoryName
        [SolrField("CategoryName_IK")]
        public string CategoryName_IK { get; set; }

        //产品标签描述
        [SolrField("Tag")]
        public string Tag { get; set; }

        //产品标签IK描述
        [SolrField("Tag_IK")]
        public string Tag_IK { get; set; }
    }

}
