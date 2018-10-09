using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions;
using SFO2O.M.ViewModel.Product;
using SFO2O.Model.Promotion;
using SFO2O.Model.Shopping;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Shopping;

namespace SFO2O.DAL.Shopping
{
    public class ShoppingCartDal : BaseDal
    {
        /// <summary>
        /// 获取购物车列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="salesTerritory"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public IList<ShoppingCartItemEntity> GetShoppingCart(int userId, int salesTerritory, int language)
        {
            string sql = @"  SELECT p.Id AS ProductId, p.Spu, p.SupplierId, p.Name,p.IsDutyOnSeller,
							p.MinPrice, p.LanguageVersion,
                            s.Sku, s.Price,   sc.CreateTime, s.AuditTime,
                            s.[Status], s.IsOnSaled,
                            s.PPATaxRate/100 as PPATaxRate,s.CBEBTaxRate/100 as CBEBTaxRate,
                            s.ConsumerTaxRate/100 as ConsumerTaxRate,s.VATTaxRate/100 as VATTaxRate,
                            s.MainDicValue,s.SubDicValue ,
                            s.MainValue,s.SubValue,s.SubValue,s.ReportStatus,s.IsCrossBorderEBTax,
                            sk.ForOrderQty 
                            ,sc.UnitPrice AS CartUnitPrice
                            , sc.Quantity AS CartQuantity
                            , sc.TaxRate/100 AS CartTaxRate
                            , sc.ExchangeRate AS CartExchangeRate
                            ,sc.ShoppingCartId
                            ,sc.IsChecked
                            ,sc.DiscountPrice AS CartDiscountPrice
                            ,sc.LastTime
                            ,sc.SortTime
                            ,pim.ImagePath AS ImagePath
                            ,p.CommissionInCHINA,p.CommissionInHK,p.NetWeightUnit,p.NetContentUnit
                            ,s.Huoli
                            from ShoppingCart  sc (NOLOCK)
                            INNER JOIN skuinfo s ON s.Sku=sc.Sku 
                            INNER JOIN  productInfo p (NOLOCK) ON p.id=s.SpuId AND p.SalesTerritory=sc.CountryId
                            INNER JOIN stock sk ON s.Sku=sk.Sku
                            LEFT JOIN ProductImage AS pim ON pim.spu=p.spu AND pim.sortValue=1
                            WHERE p.LanguageVersion=@LanguageVersion
                            AND sc.UserId=@UserId AND s.[Status] NOT IN (4,5)
                            AND (sc.CountryId=@CountryId or sc.CountryId=3 ) order by s.CreateTime desc";
            try
            {

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", userId);
                parameters.Append("@CountryId", salesTerritory);
                parameters.Append("@LanguageVersion", language);

                return DbSFO2ORead.ExecuteSqlList<ShoppingCartItemEntity>(sql, parameters);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<ShoppingCartItemEntity>();
            }
        }
        /// <summary>
        /// 获取购物车列表中的关口
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="salesTerritory"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public IList<ShoppingCartGatewayEntity> GetShoppingCartGateway(int userId)
        {
            string sql = @"SELECT sc.sku,ISNULL(scr.GatewayCode,1) as Gateway FROM ShoppingCart AS sc left JOIN SkuCustomsReport AS scr ON scr.Sku = sc.Sku WHERE sc.UserId = @UserId";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", userId);

                return DbSFO2ORead.ExecuteSqlList<ShoppingCartGatewayEntity>(sql, parameters);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<ShoppingCartGatewayEntity>();
            }
        }
        /// <summary>
        /// 获取购物车列表中的关口
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="salesTerritory"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public IList<ShoppingCartGatewayEntity> GetGatewayBySku(string sku)
        {
            string sql = @"SELECT scr.sku,scr.GatewayCode AS Gateway FROM SkuCustomsReport AS scr WHERE scr.Sku = @Sku";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@Sku", sku);

                return DbSFO2ORead.ExecuteSqlList<ShoppingCartGatewayEntity>(sql, parameters);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<ShoppingCartGatewayEntity>();
            }
        }
        /// <summary>
        /// 通过sku组装虚拟购物车信息 for　立即购买
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="salesTerritory"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public ShoppingCartItemEntity GetVirtualCartBySku(string sku, int salesTerritory, int language)
        {
            string sql = @"  
                        SELECT p.Id AS ProductId, p.Spu, p.SupplierId, p.Name,p.IsDutyOnSeller,
							p.MinPrice, p.LanguageVersion,
                            s.Sku, s.Price,    s.AuditTime,
                            0 as TaxRate, s.[Status], s.IsOnSaled,
                            s.PPATaxRate/100 as PPATaxRate,s.ReportStatus,
                            s.MainDicValue,s.SubDicValue ,
                            s.MainValue,s.SubValue,
                            s.ConsumerTaxRate/100 as ConsumerTaxRate,
                            s.VATTaxRate/100 as VATTaxRate,
                            s.CBEBTaxRate/100 as CBEBTaxRate,
                            sk.ForOrderQty,
                            s.IsCrossBorderEBTax  
                            ,pim.ImagePath AS ImagePath
                            ,p.CommissionInCHINA,p.CommissionInHK
                            ,s.Huoli
                         FROM productInfo p (NOLOCK)
                            LEFT JOIN skuinfo s ON s.SpuId=p.Id  
                            LEFT JOIN stock sk ON s.Sku=sk.Sku 
                            LEFT JOIN ProductImage AS pim ON pim.spu=p.spu AND pim.sortValue=1
                            WHERE s.IsOnSaled=1 AND s.[status]=3  AND p.LanguageVersion=@LanguageVersion   AND s.sku=@sku
                            AND (p.SalesTerritory=@CountryId or  p.SalesTerritory=3 )";
            try
            {

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@sku", sku);
                parameters.Append("@CountryId", salesTerritory);
                parameters.Append("@LanguageVersion", language);

                return DbSFO2ORead.ExecuteSqlFirst<ShoppingCartItemEntity>(sql, parameters);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取购物车列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="salesTerritory"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public int GetMiniShoppingCart(int userId, int salesTerritory, int language)
        {
            string sql = @"  SELECT count(1)
                            from ShoppingCart  sc (NOLOCK)
                            INNER JOIN skuinfo s ON s.Sku=sc.Sku AND s.[Status] = 3
                            INNER JOIN  productInfo p (NOLOCK) ON p.id=s.SpuId AND p.SalesTerritory=sc.CountryId
                            WHERE p.LanguageVersion=@LanguageVersion
                            AND sc.UserId=@UserId
                            AND (sc.CountryId=@CountryId  or sc.CountryId=3 )";
            try
            {

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", userId);
                parameters.Append("@CountryId", salesTerritory);
                parameters.Append("@LanguageVersion", language);

                return DbSFO2ORead.ExecuteSqlScalar<int>(sql, parameters);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0;
            }
        }


        /// <summary>
        /// sku是否已存在购物车中
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sku"></param>
        /// <param name="salesTerritory"></param>
        /// <returns></returns>
        public int IsExistSku(int userId, string sku, int salesTerritory)
        {

            try
            {
                string sql = " SELECT Quantity FROM ShoppingCart AS sc WHERE sc.UserId=@UserId AND (sc.CountryId=@CountryId  or sc.CountryId=3) and sku=@sku";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", userId);
                parameters.Append("@CountryId", salesTerritory);
                parameters.Append("@sku", sku);
                return DbSFO2ORead.ExecuteSqlScalar<int>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                throw ex;
            }

        }

        /// <summary>
        /// 添加sku信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="qty"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddSkuInfo(int userId, int qty, decimal exchangeRate, Model.Product.ProductSkuEntity entity, PromotionEntity promotion)
        {
            //TODO：更新购物车的促销价格
            string sql = @"INSERT INTO  [ShoppingCart]( [ShoppingCartId],[UserId] ,[CountryId] ,[Spu] ,[Sku] ,[UnitPrice] ,[Quantity]
                        ,[TaxRate] ,[ExchangeRate] ,[CartStatus] ,[IsChecked] ,[CreateTime] ,[LastTime],[SortTime] {0})     VALUES
                        ( @ShoppingCartId  ,@UserId  ,@CountryId  ,@Spu  ,@Sku  ,@UnitPrice  ,@Quantity  ,@TaxRate  ,@ExchangeRate  ,@CartStatus  ,@IsChecked  ,@CreateTime  ,@LastTime ,@SortTime {1}) ";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@ShoppingCartId", Guid.NewGuid());
                parameters.Append("@UserId", userId);
                parameters.Append("@CountryId", entity.SalesTerritory);
                parameters.Append("@Spu", entity.Spu);
                parameters.Append("@Sku", entity.Sku);
                parameters.Append("@UnitPrice", entity.Price);
                parameters.Append("@Quantity", qty);
                parameters.Append("@TaxRate", entity.TaxRate);
                parameters.Append("@ExchangeRate", exchangeRate);
                parameters.Append("@CartStatus", 1);
                parameters.Append("@IsChecked", 1);
                parameters.Append("@CreateTime", DateTime.Now);
                parameters.Append("@LastTime", DateTime.Now);
                parameters.Append("@SortTime", DateTime.Now);
                if (promotion != null)
                {
                    sql = string.Format(sql, ",DiscountPrice", ",@DiscountPrice");
                    parameters.Append("@DiscountPrice", promotion.DiscountPrice);
                }
                else
                {
                    sql = string.Format(sql, " ", "");
                }
                return DbSFO2OMain.ExecuteSqlNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }

        }

        /// <summary>
        /// 更新已存在的产品数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sku"></param>
        /// <param name="qty"></param>
        /// <param name="salesTerritory"></param>
        /// <returns></returns>
        public bool UpdateQty(int userId, string sku, int qty, int salesTerritory, PromotionEntity promotion, bool isSum = false)
        {
            //TODO：更新购物车的促销价格
            string sqlSum = @"UPDATE  [ShoppingCart]
                            SET     
                           [Quantity] = Quantity+@Quantity 
                          ,[LastTime] = @LastTime
                          ,[SortTime] = @SortTime
                          ,[IsChecked] = 1
                            {0}
                    WHERE [UserId]=@UserId and ( [CountryId]=@CountryId or CountryId=3 ) and [Sku]=@Sku ";
            string sql = @"UPDATE  [ShoppingCart]
                            SET     
                           [Quantity] = @Quantity 
                          ,[LastTime] = @LastTime
                          ,[SortTime] = @SortTime
                             {0}
                    WHERE [UserId]=@UserId and ( [CountryId]=@CountryId or CountryId=3 ) and [Sku]=@Sku ";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", userId);
                parameters.Append("@CountryId", salesTerritory);
                parameters.Append("@Sku", sku);
                parameters.Append("@Quantity", qty);
                parameters.Append("@LastTime", DateTime.Now);
                parameters.Append("@SortTime", DateTime.Now);
                //if (promotion != null)
                //{
                    //sqlSum = string.Format(sqlSum, ",DiscountPrice=@DiscountPrice");
                    //sql = string.Format(sql, ",DiscountPrice=@DiscountPrice");
                    //parameters.Append("@DiscountPrice", promotion.DiscountPrice);
                //}
                //else
                //{
                    sqlSum = string.Format(sqlSum, " ");
                    sql = string.Format(sql, " ");
                //}


                return DbSFO2OMain.ExecuteSqlNonQuery(isSum ? sqlSum : sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }

        public bool UpdateItem(string newSku, int userId, string sku, int qty, int salesTerritory, bool isExistNew, Model.Product.ProductSkuEntity entity)
        {

            var sqlDel = @"DELETE FROM  [ShoppingCart] WHERE  UserId=@UserId AND Sku=@sku and (CountryId=@CountryId or CountryId=3)  ";

            var sqlUpdate = @" update [ShoppingCart]
                                SET Sku = @NewSku	,
                                [LastTime] = GETDATE()	,
                                [Quantity] = {0} @Quantity,
                                UnitPrice=@UnitPrice,
                                TaxRate=@TaxRate
                                WHERE  UserId=@UserId AND Sku=@Sku AND ( CountryId=@CountryId or CountryId=3 ) ";

            sqlUpdate = string.Format(sqlUpdate, isExistNew ? " Quantity +" : "");

            if (!isExistNew)
            {
                try
                {
                    var parametersUpdate = DbSFO2OMain.CreateParameterCollection();
                    parametersUpdate.Append("@NewSku", newSku);
                    parametersUpdate.Append("@Quantity", qty);
                    parametersUpdate.Append("@Sku", sku);
                    parametersUpdate.Append("@UserId", userId);
                    parametersUpdate.Append("@CountryId", salesTerritory);
                    parametersUpdate.Append("@UnitPrice", entity.Price);
                    parametersUpdate.Append("@TaxRate", entity.TaxRate);
                    var result = DbSFO2OMain.ExecuteSqlNonQuery(sqlUpdate, parametersUpdate);
                    return result > 0;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                    return false;
                }
            }

            //已存在新的
            using (var con = DbSFO2OMain.CreateConnection())
            {
                con.Open();
                var trans = con.BeginTransaction();
                try
                {
                    var parametersDel = DbSFO2OMain.CreateParameterCollection();
                    parametersDel.Append("@UserId", userId);
                    parametersDel.Append("@CountryId", salesTerritory);
                    parametersDel.Append("@Sku", sku);

                    var parametersUpdate = DbSFO2OMain.CreateParameterCollection();
                    parametersUpdate.Append("@NewSku", newSku);
                    parametersUpdate.Append("@Quantity", qty);
                    parametersUpdate.Append("@Sku", newSku);
                    parametersUpdate.Append("@UserId", userId);
                    parametersUpdate.Append("@CountryId", salesTerritory);
                    parametersUpdate.Append("@UnitPrice", entity.Price);
                    parametersUpdate.Append("@TaxRate", entity.TaxRate);
                    var delete = DbSFO2OMain.ExecuteSqlNonQuery(sqlDel, parametersDel, trans);

                    var result = DbSFO2OMain.ExecuteSqlNonQuery(sqlUpdate, parametersUpdate, trans);

                    if (delete > 0 && result > 0)
                    {
                        trans.Commit();
                        return true;
                    }
                    else
                    {
                        trans.Rollback();
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                    trans.Rollback();
                    return false;
                }
            }
        }


        public bool DeleteItem(object userId, string[] sku, int currentSalesTerritory)
        {
            string sql = @" DELETE FROM  [ShoppingCart] WHERE  UserId=@UserId AND (CountryId=@CountryId or CountryId=3) {0}";
            StringBuilder sbSql = new StringBuilder();
            try
            {

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", userId);
                parameters.Append("@CountryId", currentSalesTerritory);
                for (int index = 0; index < sku.Length; index++)
                {
                    var s = sku[index];
                    sbSql.Append(string.Format(sql, " AND Sku=@sku" + index + " "));
                    parameters.Append("@Sku" + index, s);
                }
                return DbSFO2OMain.ExecuteSqlNonQuery(sbSql.ToString(), parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 下单后删除购物车
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currentSalesTerritory"></param>
        /// <returns></returns>
        public bool DeleteByUserId(object userId, int currentSalesTerritory)
        {
            string sql = @" DELETE FROM  [ShoppingCart] WHERE (CountryId=@CountryId or CountryId=3) AND IsChecked = 1 AND UserId=@UserId ";
            StringBuilder sbSql = new StringBuilder();
            try
            {

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", userId);
                parameters.Append("@CountryId", currentSalesTerritory);
                return DbSFO2OMain.ExecuteSqlNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }
        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currentSalesTerritory"></param>
        /// <returns></returns>
        public bool ClearAll(int userId, int currentSalesTerritory)
        {
            string sql = @" DELETE FROM  [ShoppingCart] WHERE  UserId=@UserId AND (CountryId=@CountryId or CountryId=3) ";
            StringBuilder sbSql = new StringBuilder();
            try
            {

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", userId);
                parameters.Append("@CountryId", currentSalesTerritory);

                return DbSFO2OMain.ExecuteSqlNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }

        public bool SelectedItem(int userId, IList<SelectedItem> skus, int currentSalesTerritory, bool selected)
        {
            string sql = @"
                        UPDATE ShoppingCart
                        SET
	                        IsChecked = {0},	
	                        LastTime = GETDATE()
	
                        WHERE  
	                        UserId = @UserId AND( CountryId=@CountryId or CountryId=3 ) {1}";
            if (skus == null || !skus.Any())
            {
                throw new ArgumentNullException("skus");
            }

            try
            {

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", userId);
                parameters.Append("@CountryId", currentSalesTerritory);

                string sqlTmp = " ";
                if (skus.Count > 1)
                {
                    var sqlOr = string.Empty;
                    for (int i = 0; i < skus.Count; i++)
                    {
                        var ptmp = "@Sku" + i;
                        if (i == 0)
                        {
                            sqlOr += "  Sku=" + ptmp;
                        }
                        else
                        {
                            sqlOr += " or Sku=" + ptmp;
                        }
                        parameters.Append(ptmp, skus[i].Sku);
                    }
                    sqlTmp = " and (" + sqlOr + ") ";
                }
                else
                {
                    sqlTmp = " and Sku=@Sku ";
                    parameters.Append("@Sku", skus[0].Sku);
                }

                sql = string.Format(sql, selected ? "1" : "0", sqlTmp);

                return DbSFO2OMain.ExecuteSqlNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }
        public bool SelectedItem(int userId, string sku, int currentSalesTerritory, bool selected)
        {
            string sql = @"
                        UPDATE ShoppingCart
                        SET
	                        IsChecked = {0},	
	                        LastTime = GETDATE()
	
                        WHERE  
	                        UserId = @UserId AND( CountryId=@CountryId or CountryId=3 ) AND Sku =@Sku";
            try
            {

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", userId);
                parameters.Append("@CountryId", currentSalesTerritory);
                parameters.Append("@Sku", sku);
                sql = string.Format(sql, selected ? "1" : "0");
                return DbSFO2OMain.ExecuteSqlNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }

        }


        //        /// <summary>
        //        /// 获取购物车列表(选择提交订单的)
        //        /// </summary>
        //        /// <param name="userId"></param>
        //        /// <param name="salesTerritory"></param>
        //        /// <param name="language"></param>
        //        /// <returns></returns>
        //        public IList<ShoppingCartOrderItemEntity> GetShoppingCartByChecked(int userId, int salesTerritory, int language)
        //        {
        //            string sql = @"  SELECT p.Id AS ProductId, p.Spu, p.SupplierId, p.Name,p.IsDutyOnSeller,
        //							p.MinPrice, p.LanguageVersion,p.CommissionInCHINA,p.CommissionInHK,
        //                            s.Sku, s.Price,   s.CreateTime, s.AuditTime,
        //                            sr.TaxRate/100 as TaxRate, s.[Status], s.IsOnSaled,
        //                            s.MainDicValue,s.SubDicValue ,
        //                            s.MainValue,s.SubValue,
        //                            sk.ForOrderQty 
        //                            ,sc.UnitPrice AS CartUnitPrice
        //                            , sc.Quantity AS CartQuantity
        //                            , sc.TaxRate/100 AS CartTaxRate
        //                            , sc.ExchangeRate AS CartExchangeRate
        //                            ,sc.ShoppingCartId
        //                            ,sc.IsChecked
        //                            ,0 AS CartTaxAmount
        //                            ,pim.ImagePath AS ImagePath
        //                            from ShoppingCart  sc (NOLOCK)
        //                            INNER JOIN skuinfo s ON s.Sku=sc.Sku 
        //                            INNER JOIN  productInfo p (NOLOCK) ON p.id=s.SpuId AND p.SalesTerritory=sc.CountryId 
        //                            INNER join SkuCustomsReport sr on s.Sku=sr.Sku
        //                            INNER JOIN stock sk ON s.Sku=sk.Sku
        //                            LEFT JOIN ProductImage AS pim ON pim.spu=p.spu AND pim.sortValue=1
        //                            WHERE p.LanguageVersion=@LanguageVersion AND sc.IsChecked =1
        //                            AND sc.UserId=@UserId and s.[status]=3
        //                            AND (sc.CountryId=@CountryId or sc.CountryId=3)";
        //            try
        //            {

        //                var parameters = DbSFO2ORead.CreateParameterCollection();
        //                parameters.Append("@UserId", userId);
        //                parameters.Append("@CountryId", salesTerritory);
        //                parameters.Append("@LanguageVersion", language);

        //                return DbSFO2ORead.ExecuteSqlList<ShoppingCartOrderItemEntity>(sql, parameters);

        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.Error(ex);
        //                return new List<ShoppingCartOrderItemEntity>();
        //            }
        //        }
    }
}
