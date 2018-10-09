using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.M.ViewModel.Account;
using SFO2O.M.ViewModel.Product;
using SFO2O.M.ViewModel.Promotion;
using SFO2O.M.ViewModel.ShoppingCart;
using SFO2O.Model.Account;
using SFO2O.Model.Product;
using SFO2O.Model.Promotion;
using SFO2O.Model.Shopping;
using SFO2O.Utility.Extensions;
using SFO2O.Utility.Uitl;
using SFO2O.Utility;

namespace SFO2O.M.Controllers.Extensions
{
    /// <summary>
    /// 所有2位小数展示均在前台控制统一调用方法
    /// </summary>
    public static class EntityExtension
    {
        /// <summary>
        /// 计算促销和汇率
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="promotions"></param>
        /// <param name="exchangeRate"></param>
        /// <returns></returns>
        public static IEnumerable<ProductItem> AsProdcutItems(this IEnumerable<ShoppingCartItemEntity> entities, IList<PromotionEntity> promotions, decimal exchangeRate, IList<ShoppingCartGatewayEntity> shopingCartGateway = null)
        {
            var shoppingCartItemEntities = entities as ShoppingCartItemEntity[] ?? entities.ToArray();
            var rtnList = new List<ProductItem>();
            if (shoppingCartItemEntities.Any())
            {
                rtnList.AddRange(shoppingCartItemEntities.OrderByDescending(n => n.SortTime).Select(entity => new ProductItem(exchangeRate)
                {
                    ProductId = entity.ProductId,
                    CartExchangeRate = entity.CartExchangeRate,
                    CartQuantity = entity.CartQuantity,
                    CartTaxRate = entity.CartTaxRate,
                    CartUnitPrice = entity.CartUnitPrice,
                    CartDiscountPrice = entity.CartDiscountPrice,
                    ForOrderQty = entity.ForOrderQty,
                    IsChecked = entity.IsChecked == 1,
                    IsDutyOnSeller = entity.IsDutyOnSeller,
                    MainDicValue = entity.MainDicValue,
                    MainValue = entity.MainValue,
                    SubDicValue = entity.SubDicValue,
                    SubValue = entity.SubValue,
                    Sku = entity.Sku,
                    Spu = entity.Spu,
                    SupplierId = entity.SupplierId,
                    ImagePath = entity.ImagePath,
                    Price = entity.Price,//原价
                    PriceExchanged = entity.Price * exchangeRate,//Math.Round(, 2),//汇后原价  
                    Name = entity.Name,
                    IsOnSaled = entity.Status == 3 && (entity.ForOrderQty - entity.CartQuantity) >= 0,
                    PPATaxRate = entity.PPATaxRate,
                    CBEBTaxRate = entity.CBEBTaxRate,
                    ConsumerTaxRate = entity.ConsumerTaxRate,
                    VATTaxRate = entity.VATTaxRate,
                    Promotion = GetPromotionItem(promotions.FirstOrDefault(m => m.Sku == entity.Sku), exchangeRate),
                    CreateTime = entity.CreateTime,
                    CommissionInCHINA = entity.CommissionInCHINA,
                    CommissionInHK = entity.CommissionInHK,
                    NetContentUnit = entity.NetContentUnit,
                    NetWeightUnit = entity.NetWeightUnit,
                    //关口类型 1：广州 2; 宁波
                    GatewayCodes = shopingCartGateway != null ? shopingCartGateway.Where(d => d.sku == entity.Sku) : null,
                    //是否报关
                    ReportStatus = entity.ReportStatus,
                    //是否电商
                    IsCrossBorderEBTax = entity.IsCrossBorderEBTax,
                    SortTime = entity.SortTime,
                    RealTaxType = Utility.Uitl.TotalTaxHelper.GetRealTaxType(entity.ReportStatus, entity.IsCrossBorderEBTax, entity.CartDiscountPrice == 0 ? entity.Price * exchangeRate : entity.CartDiscountPrice),
                    Huoli=entity.Huoli
                }));
            }
            return rtnList;
        }
        public static ProductItem AsProdcutItem(this ProductSkuEntity entity, IList<PromotionEntity> promotions, decimal exchangeRate)
        {
            var rtnList = new List<ProductItem>();
            if (entity != null)
            {
                return new ProductItem(exchangeRate)
                {
                    ProductId = entity.Id,

                    ForOrderQty = entity.Qty,

                    IsDutyOnSeller = entity.IsDutyOnSeller,
                    MainDicValue = entity.MainDicValue,
                    MainValue = entity.MainValue,
                    SubDicValue = entity.SubDicValue,
                    SubValue = entity.SubValue,
                    Sku = entity.Sku,
                    Spu = entity.Spu,
                    SupplierId = entity.SupplierId,
                    Price = entity.Price, //原价
                    PriceExchanged = entity.Price * exchangeRate, //Math.Round(, 2),//汇后原价  
                    Name = entity.Name,
                    IsOnSaled = entity.Status == 3 && (entity.MinForOrder - entity.AlarmStockQty) >= 0,
                    TaxRate = entity.TaxRate,
                    Promotion = GetPromotionItem(promotions.FirstOrDefault(m => m.Sku == entity.Sku), exchangeRate),
                    CreateTime = entity.CreateTime,
                };
            }
            else
            {
                return null;
            }
        }
        //public static IEnumerable<ProductItem> AsOrderProdcutItems(this IEnumerable<ShoppingCartOrderItemEntity> entities, decimal exchangeRate)
        //{
        //    var shoppingCartItemEntities = entities as ShoppingCartOrderItemEntity[] ?? entities.ToArray();
        //    var rtnList = new List<ProductItem>();
        //    if (shoppingCartItemEntities.Any())
        //    {
        //        rtnList.AddRange(shoppingCartItemEntities.Select(entity => new ProductItem(exchangeRate)
        //        {
        //            ProductId = entity.ProductId,
        //            CartExchangeRate = entity.CartExchangeRate,
        //            CartQuantity = entity.CartQuantity,
        //            CartTaxRate = entity.CartTaxRate,
        //            CartUnitPrice = entity.CartUnitPrice,
        //            ForOrderQty = entity.ForOrderQty,
        //            IsChecked = entity.IsChecked == 1,
        //            IsDutyOnSeller = entity.IsDutyOnSeller,
        //            MainDicValue = entity.MainDicValue,
        //            MainValue = entity.MainValue,
        //            SubDicValue = entity.SubDicValue,
        //            SubValue = entity.SubValue,
        //            Sku = entity.Sku,
        //            Spu = entity.Spu,
        //            SupplierId = entity.SupplierId,
        //            ImagePath = entity.ImagePath,
        //            Price = entity.Price,
        //            //PriceDisplay = StringUtils.ToAmt(entity.Price * exchangeRate),
        //            //TaxAmountDisplay = entity.CartTaxAmount,
        //            Name = entity.Name,
        //            IsOnSaled = entity.Status == 3 && (entity.ForOrderQty - entity.CartQuantity) >= 0,
        //            TaxRate = entity.TaxRate * 100
        //        }));
        //    }
        //    return rtnList;
        //}

        //public static IEnumerable<ProductItem> AsProdcutItems(this IEnumerable<ShoppingCartOrderItemEntity> entities, decimal exchangeRate)
        //{
        //    var shoppingCartItemEntities = entities as ShoppingCartOrderItemEntity[] ?? entities.ToArray();
        //    if (shoppingCartItemEntities.Any())
        //    {
        //        foreach (var entity in shoppingCartItemEntities)
        //        {
        //            yield return new ProductItem(exchangeRate)
        //            {
        //                ProductId = entity.ProductId,
        //                CartExchangeRate = entity.CartExchangeRate,
        //                CartQuantity = entity.CartQuantity,
        //                CartTaxRate = entity.CartTaxRate,
        //                CartUnitPrice = entity.CartUnitPrice,
        //                ForOrderQty = entity.ForOrderQty,
        //                IsChecked = entity.IsChecked == 1,
        //                IsDutyOnSeller = entity.IsDutyOnSeller,
        //                MainDicValue = entity.MainDicValue,
        //                MainValue = entity.MainValue,
        //                SubDicValue = entity.SubDicValue,
        //                SubValue = entity.SubValue,
        //                Sku = entity.Sku,
        //                Spu = entity.Spu,
        //                SupplierId = entity.SupplierId,
        //                ImagePath = entity.ImagePath,
        //                Price = entity.Price,
        //                //PriceDisplay = StringUtils.ToAmt(entity.Price * exchangeRate),
        //                //TaxAmountDisplay = StringUtils.ToAmt((entity.TaxRate * entity.Price * exchangeRate)),
        //                Name = entity.Name,
        //                IsOnSaled = entity.Status == 3 && (entity.ForOrderQty - entity.CartQuantity) >= 0,
        //                TaxRate = entity.TaxRate * 100
        //            };
        //        }


        //    }
        //    else
        //    {
        //        yield return new ProductItem(exchangeRate);
        //    }
        //}

        public static LoginUserModel AsLoginUserModel(this CustomerEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            LoginUserModel model = new LoginUserModel();

            model.UserID = entity.ID;
            model.UserName = entity.UserName;
            model.Mobile = entity.Mobile;
            model.Status = entity.Status;
            model.Gender = entity.Gender;
            model.Email = entity.Email;
            model.NickName = entity.NickName;
            model.FirstOrderAuthorize = entity.FirstOrderAuthorize;
            model.IsPushingInfo = entity.IsPushingInfo;
            model.SourceType = entity.SourceType;
            return model;
        }

        /// <summary>
        /// 转换为SKU销售属性视图 dto 为带汇率价格
        /// </summary> 
        /// <returns></returns>
        public static SkuAttributeViewModel AsSkuAttributeViewModel(this IList<ProductSkuEntity> entities, decimal exchangeRate, IList<PromotionEntity> promotions, string selectedSku = "", ProductDto dto = null)
        {
            if (entities == null || !entities.Any())
            {
                return null;
            }

            SkuAttributeViewModel model = new SkuAttributeViewModel();

            #region 元数据组装

            var template = entities.FirstOrDefault();
            if (template == null)
            {
                return null;
            }
            if (dto != null)
            {
                model.ProductName = dto.Name;
                var firstOrDefault = dto.Images.FirstOrDefault();
                if (firstOrDefault != null)
                    model.ImgUrl = firstOrDefault.ImagePath.GetImageSmallUrl();

                model.InitPrice = promotions.FirstOrDefault(m => m.Sku == template.Sku).GetSalePrice(dto.MinPriceOriginal, exchangeRate).ToNumberRoundString();
                //(GetDisplayPrice(promotions.FirstOrDefault(m => m.Sku == template.Sku), (template.MinPrice)) *exchangeRate).ToNumberRoundString(); //dto.MinPrice.ToNumberRoundString();
                //dto.SkuDtos.ToList().Min(d=>d.Price)
                var skuDto = dto.SkuDtos.ToList().OrderBy(d => d.Price).FirstOrDefault();

                model.TaxAmount = TotalTaxHelper.GetTotalTaxAmount(TotalTaxHelper.GetRealTaxType(skuDto.ReportStatus, skuDto.IsCrossBorderEBTax,
                            promotions.FirstOrDefault(m => m.Sku == skuDto.Sku).GetSalePrice(skuDto.Price, null)),
                            promotions.FirstOrDefault(m => m.Sku == skuDto.Sku).GetSalePrice(skuDto.Price, null),
                            skuDto.CBEBTaxRate, skuDto.ConsumerTaxRate, skuDto.VATTaxRate, skuDto.PPATaxRate).ToNumberRound();
                model.IsDutyOnSeller = dto.IsDutyOnSeller;
                model.RealTaxType = TotalTaxHelper.GetRealTaxType(skuDto.ReportStatus, skuDto.IsCrossBorderEBTax,
                            promotions.FirstOrDefault(m => m.Sku == skuDto.Sku).GetSalePrice(skuDto.Price, exchangeRate));
            }
            else
            {
                model.ProductName = template.Name;
                model.ImgUrl = "";
                model.InitPrice = promotions.FirstOrDefault(m => m.Sku == template.Sku).GetSalePrice(template.MinPrice, exchangeRate).ToNumberRoundString();
                //(GetDisplayPrice(promotions.FirstOrDefault(m => m.Sku == template.Sku), (template.MinPrice)) *exchangeRate).ToNumberRoundString(); //dto.MinPrice.ToNumberRoundString();//template.MinPrice.ToNumberRoundString();
            }
            model.MainName = template.MainDicValue;
            model.MainCode = template.MainDicKey;
            model.SubName = template.SubDicValue;
            model.SubCode = template.SubDicKey;

            #endregion

            List<MainSkuAttribute> mainList = new List<MainSkuAttribute>();
            List<SubSkuAttribute> subList = new List<SubSkuAttribute>();
            //只有一个销售属性
            if (string.IsNullOrEmpty(model.SubCode) && string.IsNullOrEmpty(model.SubName))
            {
                mainList = entities.DistinctBy(d => new { d.MainKey, d.MainValue }).Select(n => new MainSkuAttribute()
                {
                    MetaCode = n.MainDicKey,
                    Id = n.MainKey,
                    Name = n.MainValue,
                    NetWeightUnit = n.NetWeightUnit,
                    Sku = n.Sku,
                    Flag = n.Qty == 0 ? -1 : (!string.IsNullOrEmpty(selectedSku) && selectedSku == n.Sku) == true ? 1 : 0,
                    ForOrder = n.Qty,
                    Price = promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate),//(GetDisplayPrice(promotions.FirstOrDefault(m => m.Sku == n.Sku), (n.Price)) * exchangeRate).ToNumberRound(),
                    OriginalPrice = (n.Price * exchangeRate).ToNumberRound(),
                    Promotion = GetPromotionItem(promotions.FirstOrDefault(m => m.Sku == n.Sku), exchangeRate),

                    TaxAmount = TotalTaxHelper.GetTotalTaxAmount(TotalTaxHelper.GetRealTaxType(n.ReportStatus, n.IsCrossBorderEBTax,
                            promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate)),
                            promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate),
                            n.CBEBTaxRate, n.ConsumerTaxRate, n.VATTaxRate, n.PPATaxRate).ToNumberRound(),
                    IsDutyOnSeller = n.IsDutyOnSeller,
                    RealTaxType = TotalTaxHelper.GetRealTaxType(n.ReportStatus, n.IsCrossBorderEBTax,
                            promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate)),

                    SubAttributes = new List<SubSkuAttribute>(0)

                }).OrderBy(n => n.Id.As(0)).ToList();
            }
            else
            {
                var comparer1 = Equality<SubSkuAttribute>.CreateComparer(p => p.Id);
                subList = entities.DistinctBy(d => new { d.SubKey, d.SubValue }).Select(ss => new SubSkuAttribute()
                {
                    Flag = -1,
                    Id = ss.SubKey,
                    MetaCode = ss.SubDicKey,
                    Name = ss.SubValue,
                }).OrderBy(nn => nn.Id.As(0)).ToList();

                mainList = entities.DistinctBy(d => new { d.MainKey, d.MainValue }).Select(n => new MainSkuAttribute()
              {
                  MetaCode = n.MainDicKey,
                  Id = n.MainKey,
                  Name = n.MainValue,
                  Flag = entities.Where(s => s.MainKey == n.MainKey && s.MainDicKey == n.MainDicKey).Any(nn => nn.Sku == selectedSku) ? 1 : 0,
                  NetWeightUnit = n.NetWeightUnit,
                  Sku = "",
                  ForOrder = n.Qty,
                  Price = promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate),
                  OriginalPrice = (n.Price * exchangeRate).ToNumberRound(),
                  Promotion = GetPromotionItem(promotions.FirstOrDefault(m => m.Sku == n.Sku), exchangeRate),
                  TaxAmount = TotalTaxHelper.GetTotalTaxAmount(TotalTaxHelper.GetRealTaxType(n.ReportStatus, n.IsCrossBorderEBTax,
                            promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate)),
                            promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate),
                            n.CBEBTaxRate, n.ConsumerTaxRate, n.VATTaxRate, n.PPATaxRate).ToNumberRound(),
                  IsDutyOnSeller = n.IsDutyOnSeller,
                  RealTaxType = TotalTaxHelper.GetRealTaxType(n.ReportStatus, n.IsCrossBorderEBTax,
                          promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate)),



                  SubAttributes = entities.Where(s => s.MainKey == n.MainKey && s.MainDicKey == n.MainDicKey).Select(ss => new SubSkuAttribute()
                  {
                      Flag = ss.Qty == 0 ? -1 : (!string.IsNullOrEmpty(selectedSku) && selectedSku == ss.Sku) == true ? 1 : 0,
                      ForOrder = ss.Qty,
                      Id = ss.SubKey,
                      MetaCode = ss.SubDicKey,
                      Name = ss.SubValue,
                      Price = promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(ss.Price, exchangeRate),//(GetDisplayPrice(promotions.FirstOrDefault(m => m.Sku == n.Sku), (ss.Price))).ToNumberRound(),
                      OriginalPrice = (ss.Price * exchangeRate).ToNumberRound(),
                      Promotion = GetPromotionItem(promotions.FirstOrDefault(m => m.Sku == n.Sku), exchangeRate),
                      Sku = ss.Sku,
                      TaxAmount = TotalTaxHelper.GetTotalTaxAmount(TotalTaxHelper.GetRealTaxType(ss.ReportStatus, ss.IsCrossBorderEBTax,
                            promotions.FirstOrDefault(m => m.Sku == ss.Sku).GetSalePrice(ss.Price, exchangeRate)),
                            promotions.FirstOrDefault(m => m.Sku == ss.Sku).GetSalePrice(ss.Price, exchangeRate),
                            ss.CBEBTaxRate, ss.ConsumerTaxRate, ss.VATTaxRate, ss.PPATaxRate).ToNumberRound(),
                      IsDutyOnSeller = ss.IsDutyOnSeller,
                      RealTaxType = TotalTaxHelper.GetRealTaxType(ss.ReportStatus, ss.IsCrossBorderEBTax,
                          promotions.FirstOrDefault(m => m.Sku == ss.Sku).GetSalePrice(ss.Price, exchangeRate))

                  }).ToList().Union<SubSkuAttribute>(subList, comparer1).OrderBy(nn => nn.Id.As(0)).ToList()
              }).ToList();
            }

            model.MainAttributes = mainList;
            model.SubAttributes = subList;
            //如果为净重   增加单位
            if (model.MainName != null && model.MainName != "")
            {
                if (model.MainName.Equals("净重") || model.MainName.Equals("淨重") || model.MainName.Equals("净含量") || model.MainName.Equals("凈含量"))
                {
                    for (int i = 0; i < model.MainAttributes.Count; i++)
                    {
                        var mainAttribute = model.MainAttributes[i];

                        //当数值带有小数点，且末尾有0，则清小数点后空末尾处的0，然后再拼接单位
                        string number = mainAttribute.Name;
                        if (string.IsNullOrEmpty(number))
                        {
                            number = "0";
                        }
                        double result = 0;
                        if (Double.TryParse(number, out result))
                        {
                            mainAttribute.Name = string.Format("{0}{1}", result, mainAttribute.NetWeightUnit);
                        }
                        else
                        {
                            mainAttribute.Name = "抱歉，数据有误！";
                        };

                    }
                }
            }

            return model;
        }

        public static SkuAttributeViewModel AsSkuAttributeViewModelD(this IList<ProductSkuEntity> entities, decimal exchangeRate, IList<PromotionEntity> promotions, string selectedSku = "", ItemViewSupporter dto = null)
        {
            if (entities == null || !entities.Any())
            {
                return null;
            }

            SkuAttributeViewModel model = new SkuAttributeViewModel();

            #region 元数据组装

            var template = entities.FirstOrDefault();
            if (template == null)
            {
                return null;
            }
            if (dto != null)
            {
                model.ProductName = dto.Name;
                var firstOrDefault = dto.Images.FirstOrDefault();
                if (firstOrDefault != null)
                    model.ImgUrl = firstOrDefault.ImagePath.GetImageSmallUrl();

                model.InitPrice = promotions.FirstOrDefault(m => m.Sku == template.Sku).GetSalePrice(dto.MinPriceOriginal, exchangeRate).ToNumberRoundString();
                //(GetDisplayPrice(promotions.FirstOrDefault(m => m.Sku == template.Sku), (template.MinPrice)) *exchangeRate).ToNumberRoundString(); //dto.MinPrice.ToNumberRoundString();
                //dto.SkuDtos.ToList().Min(d=>d.Price)
                var skuDto = dto.SkuDtos.ToList().OrderBy(d => d.Price).FirstOrDefault();

                model.TaxAmount = TotalTaxHelper.GetTotalTaxAmount(TotalTaxHelper.GetRealTaxType(skuDto.ReportStatus, skuDto.IsCrossBorderEBTax,
                            promotions.FirstOrDefault(m => m.Sku == skuDto.Sku).GetSalePrice(skuDto.Price, null)),
                            promotions.FirstOrDefault(m => m.Sku == skuDto.Sku).GetSalePrice(skuDto.Price, null),
                            skuDto.CBEBTaxRate, skuDto.ConsumerTaxRate, skuDto.VATTaxRate, skuDto.PPATaxRate).ToNumberRound();
                model.IsDutyOnSeller = dto.IsDutyOnSeller;
                model.RealTaxType = TotalTaxHelper.GetRealTaxType(skuDto.ReportStatus, skuDto.IsCrossBorderEBTax,
                            promotions.FirstOrDefault(m => m.Sku == skuDto.Sku).GetSalePrice(skuDto.Price, exchangeRate));
            }
            else
            {
                model.ProductName = template.Name;
                model.ImgUrl = "";
                model.InitPrice = promotions.FirstOrDefault(m => m.Sku == template.Sku).GetSalePrice(template.MinPrice, exchangeRate).ToNumberRoundString();
                //(GetDisplayPrice(promotions.FirstOrDefault(m => m.Sku == template.Sku), (template.MinPrice)) *exchangeRate).ToNumberRoundString(); //dto.MinPrice.ToNumberRoundString();//template.MinPrice.ToNumberRoundString();
            }
            model.MainName = template.MainDicValue;
            model.MainCode = template.MainDicKey;
            model.SubName = template.SubDicValue;
            model.SubCode = template.SubDicKey;

            #endregion

            List<MainSkuAttribute> mainList = new List<MainSkuAttribute>();
            List<SubSkuAttribute> subList = new List<SubSkuAttribute>();
            //只有一个销售属性
            if (string.IsNullOrEmpty(model.SubCode) && string.IsNullOrEmpty(model.SubName))
            {
                mainList = entities.DistinctBy(d => new { d.MainKey, d.MainValue }).Select(n => new MainSkuAttribute()
                {
                    MetaCode = n.MainDicKey,
                    Id = n.MainKey,
                    Name = n.MainValue,
                    NetWeightUnit = n.NetWeightUnit,
                    Sku = n.Sku,
                    Flag = n.Qty == 0 ? -1 : (!string.IsNullOrEmpty(selectedSku) && selectedSku == n.Sku) == true ? 1 : 0,
                    ForOrder = n.Qty,
                    Price = promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate),//(GetDisplayPrice(promotions.FirstOrDefault(m => m.Sku == n.Sku), (n.Price)) * exchangeRate).ToNumberRound(),
                    OriginalPrice = (n.Price * exchangeRate).ToNumberRound(),
                    Promotion = GetPromotionItem(promotions.FirstOrDefault(m => m.Sku == n.Sku), exchangeRate),

                    TaxAmount = TotalTaxHelper.GetTotalTaxAmount(TotalTaxHelper.GetRealTaxType(n.ReportStatus, n.IsCrossBorderEBTax,
                            promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate)),
                            promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate),
                            n.CBEBTaxRate, n.ConsumerTaxRate, n.VATTaxRate, n.PPATaxRate).ToNumberRound(),
                    IsDutyOnSeller = n.IsDutyOnSeller,
                    RealTaxType = TotalTaxHelper.GetRealTaxType(n.ReportStatus, n.IsCrossBorderEBTax,
                            promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate)),

                    SubAttributes = new List<SubSkuAttribute>(0)

                }).OrderBy(n => n.Id.As(0)).ToList();
            }
            else
            {
                var comparer1 = Equality<SubSkuAttribute>.CreateComparer(p => p.Id);
                subList = entities.DistinctBy(d => new { d.SubKey, d.SubValue }).Select(ss => new SubSkuAttribute()
                {
                    Flag = -1,
                    Id = ss.SubKey,
                    MetaCode = ss.SubDicKey,
                    Name = ss.SubValue,
                }).OrderBy(nn => nn.Id.As(0)).ToList();

                mainList = entities.DistinctBy(d => new { d.MainKey, d.MainValue }).Select(n => new MainSkuAttribute()
                {
                    MetaCode = n.MainDicKey,
                    Id = n.MainKey,
                    Name = n.MainValue,
                    Flag = entities.Where(s => s.MainKey == n.MainKey && s.MainDicKey == n.MainDicKey).Any(nn => nn.Sku == selectedSku) ? 1 : 0,
                    NetWeightUnit = n.NetWeightUnit,
                    Sku = "",
                    ForOrder = n.Qty,
                    Price = promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate),
                    OriginalPrice = (n.Price * exchangeRate).ToNumberRound(),
                    Promotion = GetPromotionItem(promotions.FirstOrDefault(m => m.Sku == n.Sku), exchangeRate),
                    TaxAmount = TotalTaxHelper.GetTotalTaxAmount(TotalTaxHelper.GetRealTaxType(n.ReportStatus, n.IsCrossBorderEBTax,
                              promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate)),
                              promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate),
                              n.CBEBTaxRate, n.ConsumerTaxRate, n.VATTaxRate, n.PPATaxRate).ToNumberRound(),
                    IsDutyOnSeller = n.IsDutyOnSeller,
                    RealTaxType = TotalTaxHelper.GetRealTaxType(n.ReportStatus, n.IsCrossBorderEBTax,
                            promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(n.Price, exchangeRate)),



                    SubAttributes = entities.Where(s => s.MainKey == n.MainKey && s.MainDicKey == n.MainDicKey).Select(ss => new SubSkuAttribute()
                    {
                        Flag = ss.Qty == 0 ? -1 : (!string.IsNullOrEmpty(selectedSku) && selectedSku == ss.Sku) == true ? 1 : 0,
                        ForOrder = ss.Qty,
                        Id = ss.SubKey,
                        MetaCode = ss.SubDicKey,
                        Name = ss.SubValue,
                        Price = promotions.FirstOrDefault(m => m.Sku == n.Sku).GetSalePrice(ss.Price, exchangeRate),//(GetDisplayPrice(promotions.FirstOrDefault(m => m.Sku == n.Sku), (ss.Price))).ToNumberRound(),
                        OriginalPrice = (ss.Price * exchangeRate).ToNumberRound(),
                        Promotion = GetPromotionItem(promotions.FirstOrDefault(m => m.Sku == n.Sku), exchangeRate),
                        Sku = ss.Sku,
                        TaxAmount = TotalTaxHelper.GetTotalTaxAmount(TotalTaxHelper.GetRealTaxType(ss.ReportStatus, ss.IsCrossBorderEBTax,
                              promotions.FirstOrDefault(m => m.Sku == ss.Sku).GetSalePrice(ss.Price, exchangeRate)),
                              promotions.FirstOrDefault(m => m.Sku == ss.Sku).GetSalePrice(ss.Price, exchangeRate),
                              ss.CBEBTaxRate, ss.ConsumerTaxRate, ss.VATTaxRate, ss.PPATaxRate).ToNumberRound(),
                        IsDutyOnSeller = ss.IsDutyOnSeller,
                        RealTaxType = TotalTaxHelper.GetRealTaxType(ss.ReportStatus, ss.IsCrossBorderEBTax,
                            promotions.FirstOrDefault(m => m.Sku == ss.Sku).GetSalePrice(ss.Price, exchangeRate))

                    }).ToList().Union<SubSkuAttribute>(subList, comparer1).OrderBy(nn => nn.Id.As(0)).ToList()
                }).ToList();
            }

            model.MainAttributes = mainList;
            model.SubAttributes = subList;
            //如果为净重   增加单位
            if (model.MainName != null && model.MainName != "")
            {
                if (model.MainName.Equals("净重") || model.MainName.Equals("淨重") || model.MainName.Equals("净含量") || model.MainName.Equals("凈含量"))
                {
                    for (int i = 0; i < model.MainAttributes.Count; i++)
                    {
                        var mainAttribute = model.MainAttributes[i];

                        //当数值带有小数点，且末尾有0，则清小数点后空末尾处的0，然后再拼接单位
                        string number = mainAttribute.Name;
                        if (string.IsNullOrEmpty(number))
                        {
                            number = "0";
                        }
                        double result = 0;
                        if (Double.TryParse(number, out result))
                        {
                            mainAttribute.Name = string.Format("{0}{1}", result, mainAttribute.NetWeightUnit);
                        }
                        else
                        {
                            mainAttribute.Name = "抱歉，数据有误！";
                        };

                    }
                }
            }

            return model;
        }

        /// <summary>
        /// 获取促销价格逻辑
        /// </summary>
        /// <param name="promotionEntity"></param>
        /// <param name="originalPrice"></param>
        /// <param name="exchangeRate"></param>
        /// <returns></returns>
        public static decimal GetSalePrice(this PromotionEntity promotionEntity, decimal originalPrice, decimal? exchangeRate = null)
        {
            var rtnValue = originalPrice;
            if (promotionEntity != null)
            {
                rtnValue = promotionEntity.DiscountPrice;

            }

            if (exchangeRate != null)
            {
                rtnValue = rtnValue * exchangeRate.Value;
            }

            return rtnValue.ToNumberRound();
        }


        private static PromotionItem GetPromotionItem(PromotionEntity p, decimal exchangeRate)
        {
            if (p == null)
            {
                return null;
            }
            else
            {
                return new PromotionItem()
                {
                    Sku = p.Sku,
                    PromotionId = p.PromotionId,
                    DiscountPrice = p.DiscountPrice,
                    DiscountPriceExchanged = p.DiscountPrice * exchangeRate,
                    DiscountRate = p.DiscountRate,
                    EndTime = p.EndTime,
                    PromotionLable = p.PromotionLable,
                    PromotionName = p.PromotionName,
                    PromotionType=p.PromotionType
                };
            }
        }
    }
}
