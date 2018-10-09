using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Model.Common;
using SFO2O.Model.My;
using SFO2O.Model.Product;
using SFO2O.Model.Promotion;
using SFO2O.Model.Shopping;
using SFO2O.Utility.Extensions;
using SFO2O.Utility.Uitl;

namespace SFO2O.Model.Extensions
{
    public static class EntityExtesion
    {
        public static IList<ProductInfoModel> FillPromotions(this IList<ProductInfoModel> list,
            IList<PromotionSpu> promotionEntities)
        {
            if (list == null || !list.Any())
            {
                return list;
            }
            if (promotionEntities == null || !promotionEntities.Any())
            {
                return list;
            }

            foreach (var model in list)
            {

                var promotion = promotionEntities.FirstOrDefault(n => n.Spu == model.SPU);
                if (promotion != null)
                {
                    model.DiscountPrice = promotion.DiscountPrice;
                    model.DiscountRate = promotion.DiscountRate.ToString();
                }
            }
            return list;
        }

        public static IList<MyOrderInfoDto> AsDtos(this IList<MyOrderSkuInfoEntity> entities)
        {
            var rtnList = new List<MyOrderInfoDto>();
            if (entities != null && entities.Any())
            {
                rtnList.AddRange(entities.GroupBy(n => n.OrderCode).Select(skus => skus.AsDto()));
            }
            return rtnList;

        }

        public static MyOrderInfoDto AsDto(this IEnumerable<MyOrderSkuInfoEntity> skuInfoEntities)
        {
            var myOrderSkuInfoEntities = skuInfoEntities as MyOrderSkuInfoEntity[] ?? skuInfoEntities.ToArray();
            if (skuInfoEntities == null || !myOrderSkuInfoEntities.Any())
            {
                return null;
            }
            var p = myOrderSkuInfoEntities.FirstOrDefault();

            MyOrderInfoDto orderInfo = new MyOrderInfoDto
            {
                CouponId = p.CouponId,
                Coupon=p.Coupon,
                CardSum=p.CardSum,
                OrderCode = p.OrderCode,
                CreateTime = p.CreateTime,
                CustomsDuties = p.CustomsDuties.ToNumberRound(),
                Freight = p.Freight,
                OrderStatus = p.OrderStatus,
                ProductTotalAmount = p.ProductTotalAmount,
                RowsCount = p.RowsCount,
                TotalAmount = p.TotalAmount.ToNumberRound(),
                UserId = p.UserId,
                PayStatus = p.PayStatus,
                PayType = p.PayType,
                ExchangeRate = p.ExchangeRate,
                Receiver = p.Receiver,
                Phone = p.Phone,
                PassPortType = p.PassPortType,
                PassPortNum = p.PassPortNum,
                ReceiptAddress = p.ReceiptAddress,
                ReceiptPostalCode = p.ReceiptPostalCode,
                ReceiptRegion = p.ReceiptRegion,
                ReceiptCity = p.ReceiptCity,
                ReceiptProvince = p.ReceiptProvince,
                ReceiptCountry = p.ReceiptCountry,
                ShippingMethod = p.ShippingMethod,
                PayTime = p.PayTime,
                DeliveryTime = p.DeliveryTime,
                ArrivalTime = p.ArrivalTime,
                OrderCompletionTime = p.OrderCompletionTime,
                TaxType = p.TaxType,
                TeamCode=p.TeamCode,
                HuoLi=p.HuoLi,
                TeamUserId=p.TeamUserId,
                TeamStatus =p.TeamStatus,
                TeamStartTime=p.TeamStartTime,
                TeamEndTime=p.TeamEndTime,
                SkuInfos = new List<MyOrderSkuInfoDto>(),
                ExpressList=p.ExpressList,
                ExpressCompany=p.ExpressCompany

            };
            List<MyOrderSkuInfoDto> list = myOrderSkuInfoEntities.Select(m => new MyOrderSkuInfoDto()
            {
                Sku = m.Sku,
                ImagePath = m.ImagePath,
                MainDicValue = m.MainDicValue,
                MainValue = m.MainValue,
                Name = m.Name,
                Quantity = m.Quantity,
                Spu = m.Spu,
                SubDicValue = m.SubDicValue,
                SubValue = m.SubValue,
                PayUnitPrice = m.PayUnitPrice.ToNumberRound(),
                UnitPrice = m.UnitPrice,
                TaxRate = m.TaxRate,
                RefundQuantity = m.RefundQuantity,
                IsBearDuty = m.IsBearDuty,
                Commission = m.Commission,
                IsReturn = m.IsReturn,
                SupplierId = m.SupplierId,
                NetWeightUnit = m.NetWeightUnit,
                NetContentUnit = m.NetContentUnit,
                TeamCode=m.TeamCode,
                TeamUserId=m.TeamUserId,
                TeamStatus=m.TeamStatus,
                TeamStartTime = m.TeamStartTime,
                TeamEndTime = m.TeamEndTime,
                HuoLi = m.HuoLi,
            }).ToList();

            orderInfo.SkuInfos = list;

            return orderInfo;
        }
        /// <summary>
        /// 获取可以销售的商品。
        /// </summary>
        /// <param name="itemEntity"></param>
        /// <returns></returns>
        public static ShoppingCartItemEntity ForSales(this ShoppingCartItemEntity itemEntity, Action<ShoppingCartItemEntity> extra = null)
        {
            if (itemEntity == null || !itemEntity.IsOnSaled)
            {
                return null;
            }

            if (itemEntity.ForOrderQty - itemEntity.CartQuantity < 0)
            {
                return null;
            }

            ShoppingCartItemEntity forSale = itemEntity.Clone();


            if (extra != null)
            {
                extra(forSale);
            }

            return forSale;
        }

        public static IList<SpuAttributeModel> AsSpuAttributeModels(this ProductExpandEntity entity, IList<DicsEntity> fields)
        {
            IList<SpuAttributeModel> rtnList = new List<SpuAttributeModel>();
            if (entity == null)
            {
                return rtnList;
            }
            //  var ps = typeof (ProductExpandEntity).Attributes;
            var propertyInfos = entity.GetType().GetProperties();
            int sortValue = 0;
            DicsEntity fieldExchange = null;
            foreach (var field in fields)
            {
                if (field.KeyName == "IsExchange")
                {
                    fieldExchange = field;
                    continue;
                }

                var pi = propertyInfos.FirstOrDefault(n => n.Name.ToLower() == field.KeyName.ToLower());
                if (pi != null)
                {
                    rtnList.Add(new SpuAttributeModel()
                    {
                        AttributeKey = field.KeyName,
                        AttributeName = field.KeyValue,
                        AttributeValue = pi.GetValue(entity, null).As(""),
                        SortValue = sortValue
                    });

                    sortValue++;
                }
            }
            //特殊处理
            if (fieldExchange != null)
            {
                rtnList.Add(new SpuAttributeModel()
                     {
                         AttributeKey = fieldExchange.KeyName,
                         AttributeName = fieldExchange.KeyValue,
                         AttributeValue = entity.IsExchangeInCHINA,
                         //AttributeValue = string.IsNullOrEmpty(entity.IsExchangeInCHINA) ? entity.IsExchangeInHK : (entity.IsExchangeInHK + "," + entity.IsExchangeInCHINA),
                         SortValue = sortValue
                     });
            }

            return rtnList;
        }

        /// <summary>
        /// 转换为视图使用
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="exchangeRate"></param>
        /// <returns></returns>
        public static ProductDto AsDto(this ProductSkuEntity[] entities, decimal exchangeRate, IList<PromotionEntity> promotionEntities,bool isflag)
        {
            if (entities != null && entities.Any())
            {

                var product = entities.OrderBy(s => s.ProductPrice).First();

                var dto = new ProductDto()
                {
                    //Product部分赋值==TODO：待详细填充
                    Spu = product.Spu,
                    CategoryId = product.CategoryId,
                    Id = product.Id,
                    isflag=isflag,
                    //sku部分product赋值
                    SkuDtos = entities.Select(sku => new SkuDto()
                    {
                        Sku = sku.Sku,
                        Status = sku.Status,
                        CreateTime = sku.CreateTime,
                        IsOnSaled = sku.IsOnSaled,
                        Price = sku.Price * exchangeRate,
                        Qty = sku.Qty,
                        AlarmStockQty = sku.AlarmStockQty,
                        BarCode = sku.BarCode,
                        TaxRate = sku.TaxRate,

                        //销售属性
                        MainKey = sku.MainKey,
                        MainValue = sku.MainValue,
                        MainDicKey = sku.MainDicKey,
                        MainDicValue = sku.MainDicValue,

                        SubKey = sku.SubKey,
                        SubValue = sku.SubValue,
                        SubDicKey = sku.SubDicKey,
                        SubDicValue = sku.SubDicValue,

                        ReportStatus = sku.ReportStatus,
                        IsCrossBorderEBTax = sku.IsCrossBorderEBTax,
                        PPATaxRate = sku.PPATaxRate,
                        CBEBTaxRate = sku.CBEBTaxRate,
                        ConsumerTaxRate = sku.ConsumerTaxRate,
                        VATTaxRate = sku.VATTaxRate

                    }).ToArray(),
                    Brand = product.Brand,
                    CountryOfManufacture = product.CountryOfManufacture,
                    Description = product.Description,
                    IsExchangeCN = product.IsExchangeInCHINA,
                    IsExchangeHK = product.IsExchangeInHK,
                    IsReturn = product.IsReturn,
                    LanguageVersion = product.LanguageVersion,
                    MinForOrder = product.MinForOrder,
                    MinPrice = product.MinPrice * exchangeRate,
                    MinPriceOriginal = product.MinPrice,
                    Name = product.Name,
                    ProductPrice = product.ProductPrice * exchangeRate,
                    SalesTerritory = product.SalesTerritory,
                    SupplierId = product.SupplierId,
                    Tag = product.Tag,
                    SkuForOrder = entities.Sum(n => n.Qty),
                    BrandId = product.BrandId,
                    NameCN = product.NameCN,
                    NameHK = product.NameHK,
                    NameEN = product.NameEN,
                    Logo = product.Logo,
                    CountryName = product.CountryName,
                    IntroductionCN = product.IntroductionCN,
                    IsDutyOnSeller = product.IsDutyOnSeller,
                    NationalFlag = string.IsNullOrEmpty(product.NationalFlag)?"":product.NationalFlag + PathHelper.NationalFlagImageExtension,
                    NetWeightUnit=product.NetWeightUnit
                };

                //二期：增加促销
                if (promotionEntities != null && promotionEntities.Any())
                {
                    var minPromotion = promotionEntities.OrderBy(n => n.DiscountPrice).FirstOrDefault();
                    if (minPromotion != null)
                    {
                        if (minPromotion.DiscountPrice > product.MinPrice)//显示最小价格
                        {
                            dto.Promotion = null;
                        }
                        else
                        {
                            dto.PromotionId = minPromotion.PromotionId;
                            dto.PromotionType = minPromotion.PromotionType;
                            dto.Promotion = minPromotion;
                            dto.PromotionDiscountPrice = dto.Promotion.DiscountPrice * exchangeRate;
                        }
                    }
                    else
                    {
                        dto.Promotion = null;
                    }
                }
                return dto;
            }
            else
            {
                return new ProductDto();
            }
        }
    }
}
