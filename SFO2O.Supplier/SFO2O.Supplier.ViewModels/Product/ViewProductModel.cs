using SFO2O.Supplier.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.ViewModels.Product
{
    public class ViewProductModel
    {
        /// <summary>
        /// Product/product_Temp表数据
        /// 繁体
        /// </summary>
        public ProductTempModel ProductInfo_TC;

        /// <summary>
        /// Product/product_Temp表数据
        /// 简体
        /// </summary>
        public ProductTempModel ProductInfo_SC;

        /// <summary>
        /// Product/product_Temp表数据
        /// 英文
        /// </summary>
        public ProductTempModel ProductInfo_EN;
    }

    /// <summary>
    /// 查看产品页面的显示文字
    /// </summary>
    public interface IViewProductDisplayText
    {
        string Language { get; }
        string Risk { get; }
        /// <summary>
        /// 基本信息
        /// </summary>
        string BasicInfo { get; }
        /// <summary>
        /// 品牌
        /// </summary>
        string Brand { get; }
        /// <summary>
        /// 商品名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 商品价格
        /// </summary>
        string Price { get; }
        /// <summary>
        /// 商品单位
        /// </summary>
        string Unit { get; }
        /// <summary>
        /// 商品关税
        /// </summary>
        string Tariff { get; }
        /// <summary>
        /// 卖家承担
        /// </summary>
        string SellerBear { get; }
        /// <summary>
        /// 买家承担
        /// </summary>
        string BuyerBear { get; }
        /// <summary>
        /// 商品属性
        /// </summary>
        string Attributes { get; }
        /// <summary>
        /// 商品描述
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 点击查看
        /// </summary>
        string ClickToView { get; }
        /// <summary>
        /// 商品图片
        /// </summary>
        string Images { get; }
        /// <summary>
        /// 条形码
        /// </summary>
        string SkuBarCode { get; }
        /// <summary>
        /// 价格
        /// </summary>
        string SkuPrice { get; }
        /// <summary>
        /// 库存预警
        /// </summary>
        string SkuAlarmStockQty { get; }
        /// <summary>
        /// 物流包装
        /// </summary>
        string DeliveryPack { get; }
        /// <summary>
        /// 系统属性
        /// </summary>
        string SystemProperties { get; }
        /// <summary>
        /// 商品分类
        /// </summary>
        string Category { get; }
        /// <summary>
        /// 销售区域
        /// </summary>
        string SaleRegion { get; }
        /// <summary>
        /// 佣金
        /// </summary>
        string Commission { get; }
        /// <summary>
        /// 中国大陆
        /// </summary>
        string ChinaMainland { get; }
        /// <summary>
        /// 香港地区
        /// </summary>
        string HongKong { get; }
        /// <summary>
        /// 商品上架时间
        /// </summary>
        string OnSaleTime { get; }
    }
    public class ViewProductDisplayTextTC : IViewProductDisplayText
    {
        public string Language { get { return "TC"; } }
        public string Risk { get { return "："; } }
        public string BasicInfo { get { return "商品基本信息"; } }
        public string Brand { get { return "品牌"; } }

        public string Name { get { return "商品名称"; } }

        public string Price { get { return "商品价格"; } }

        public string Unit { get { return "商品单位"; } }

        public string Tariff { get { return "商品关税"; } }

        public string SellerBear { get { return "卖家承担"; } }

        public string BuyerBear { get { return "买家承担"; } }

        public string Attributes { get { return "商品属性"; } }

        public string Description { get { return "商品描述"; } }

        public string ClickToView { get { return "点击查看"; } }

        public string Images { get { return "商品图片"; } }

        public string SkuBarCode { get { return "条形码"; } }

        public string SkuPrice { get { return "价格"; } }

        public string SkuAlarmStockQty { get { return "库存预警"; } }

        public string DeliveryPack { get { return "物流包装"; } }

        public string SystemProperties { get { return "系统属性"; } }

        public string Category { get { return "商品分类"; } }

        public string SaleRegion { get { return "销售区域"; } }

        public string Commission { get { return "佣金"; } }

        public string ChinaMainland { get { return "中国大陆"; } }

        public string HongKong { get { return "香港地区"; } }

        public string OnSaleTime { get { return "商品上架时间"; } }
    }
    public class ViewProductDisplayTextSC : IViewProductDisplayText
    {
        public string Language { get { return "SC"; } }
        public string Risk { get { return "："; } }
        public string BasicInfo { get { return "商品基本信息"; } }
        public string Brand { get { return "品牌"; } }

        public string Name { get { return "商品名称"; } }

        public string Price { get { return "商品价格"; } }

        public string Unit { get { return "商品单位"; } }

        public string Tariff { get { return "商品关税"; } }

        public string SellerBear { get { return "卖家承担"; } }

        public string BuyerBear { get { return "买家承担"; } }

        public string Attributes { get { return "商品属性"; } }

        public string Description { get { return "商品描述"; } }

        public string ClickToView { get { return "点击查看"; } }

        public string Images { get { return "商品图片"; } }

        public string SkuBarCode { get { return "条形码"; } }

        public string SkuPrice { get { return "价格"; } }

        public string SkuAlarmStockQty { get { return "库存预警"; } }

        public string DeliveryPack { get { return "物流包装"; } }

        public string SystemProperties { get { return "系统属性"; } }

        public string Category { get { return "商品分类"; } }

        public string SaleRegion { get { return "销售区域"; } }

        public string Commission { get { return "佣金"; } }

        public string ChinaMainland { get { return "中国大陆"; } }

        public string HongKong { get { return "香港地区"; } }

        public string OnSaleTime { get { return "商品上架时间"; } }
    }
    public class ViewProductDisplayTextEN : IViewProductDisplayText
    {
        public string Language { get { return "EN"; } }
        public string Risk { get { return ":"; } }
        public string BasicInfo { get { return "basic information"; } }
        public string Brand { get { return "Brand"; } }

        public string Name { get { return "Name"; } }

        public string Price { get { return "Price"; } }

        public string Unit { get { return "Unit"; } }

        public string Tariff { get { return "Tariff"; } }

        public string SellerBear { get { return "Seller Bear"; } }

        public string BuyerBear { get { return "Buyer Bear"; } }

        public string Attributes { get { return "Attributes"; } }

        public string Description { get { return "Description"; } }

        public string ClickToView { get { return "Click To View"; } }

        public string Images { get { return "Images"; } }

        public string SkuBarCode { get { return "BarCode"; } }

        public string SkuPrice { get { return "Price"; } }

        public string SkuAlarmStockQty { get { return "AlarmStockQty"; } }

        public string DeliveryPack { get { return "Delivery Pack"; } }

        public string SystemProperties { get { return "System Properties"; } }

        public string Category { get { return "Category"; } }

        public string SaleRegion { get { return "Sale Region"; } }

        public string Commission { get { return "Commission"; } }

        public string ChinaMainland { get { return "China Mainland"; } }

        public string HongKong { get { return "Hong Kong"; } }

        public string OnSaleTime { get { return "OnSale Time"; } }
    }
}
