using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Model.Product;
using SFO2O.Model.Promotion;
using SFO2O.Utility.Uitl;

namespace SFO2O.DAL.Promotion
{
    public class PromotionDal : BaseDal
    {
        /// <summary>
        /// 返回spu和对应的促销价格
        /// </summary>
        /// <param name="spus"></param>
        /// <returns></returns>
        public IList<PromotionSpu> GetAvaliablePromotionPrice(string[] spus, int type = 0)
        {
            if (spus == null || !spus.Any())
            {
                throw new ArgumentException("spus");
            }
            try
            {
                string skusTmp = string.Join(",", spus);
                string sql = @"
                                DECLARE @str NVARCHAR(500)
                                SET @str=@spus
 
                                ;WITH tbSku
                                as (
                                SELECT distinct sku,s.Spu FROM (
                                SELECT
	                                fs.c1
                                FROM
	                                dbo.func_splitidString(@str,',') AS fs) f
	                                INNER JOIN SkuInfo s ON  f.c1=s.Spu 
                                )  
                                SELECT spu,sku,DiscountPrice,t.DiscountRate FROM (
                                SELECT b.spu,b.sku,DiscountPrice, DiscountRate, ROW_NUMBER() over(partition by b.spu order by DiscountPrice ASC ) as rownum FROM (
                                SELECT ps.PromotionId, ps.Sku, ps.DiscountRate, ps.DiscountPrice,p.SupplierId,
                                p.PromotionName, p.StartTime, p.EndTime, p.PromotionLable, p.PromotionCost,
                                p.PromotionStatus, p.PromotionType, p.CreateTime, p.CreateBy,ts.Spu
                                  FROM PromotionSku AS ps
                                  INNER JOIN tbSku AS ts ON ps.Sku=ts.sku
                                  LEFT JOIN promotions p ON ps.PromotionId=p.Id  
                                WHERE  p.PromotionStatus=2 {0}
                                )b)t
                                WHERE t.rownum=1 	";

                sql = string.Format(sql, (type != 0) ? (" and p.PromotionType=" + type) : "");

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@spus", skusTmp);

                return DbSFO2ORead.ExecuteSqlList<PromotionSpu>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }

        }


        /// <summary>
        /// 获取有效的促销信息
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        public IList<PromotionEntity> GetAvaliablePromotionEntities(string[] skus)
        {
            if (skus == null || !skus.Any())
            {
                throw new ArgumentException("skus");
            }
            try
            {
                string skusTmp = string.Join(",", skus);

                string sql = @"DECLARE @str NVARCHAR(500)
                            SET @str=@Skus
                            ;WITH tbSku
                            as ( SELECT fs.c1 FROM dbo.func_splitidString(@str,',') AS fs) 

                            SELECT ps.PromotionId, ps.Sku, ps.DiscountRate, ps.DiscountPrice,p.SupplierId,
                                   p.PromotionName, p.StartTime, p.EndTime, p.PromotionLable, p.PromotionCost,
                                   p.PromotionStatus, p.PromotionType, p.CreateTime, p.CreateBy
                              FROM PromotionSku AS ps
                              INNER JOIN tbSku AS ts ON ps.Sku=ts.c1
                            LEFT JOIN promotions p ON ps.PromotionId=p.Id
                            WHERE p.PromotionStatus=2
                            ";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@Skus", skusTmp);
                return DbSFO2ORead.ExecuteSqlList<PromotionEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }


        }
        /// <summary>
        /// 获取有效的促销信息
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        public IList<PromotionEntity> GetAvaliablePromotionEntitiesTeam(int proid)
        {
            if (proid == 0)
            {
                throw new ArgumentException("proid");
            }
            try
            {
                string sql = @"SELECT ps.PromotionId, ps.Sku, ps.DiscountRate, ps.DiscountPrice,p.SupplierId,
                                   p.PromotionName, p.StartTime, p.EndTime, p.PromotionLable, p.PromotionCost,
                                   p.PromotionStatus, p.PromotionType, p.CreateTime, p.CreateBy
                              FROM PromotionSku AS ps
                            LEFT JOIN promotions p ON ps.PromotionId=p.Id
                            WHERE ps.PromotionId = @PromotionId
                            ORDER BY p.ID desc
                            ";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@PromotionId", proid);
                return DbSFO2ORead.ExecuteSqlList<PromotionEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }


        }
        /// <summary>
        /// 获取有效的促销信息
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        public IList<PromotionEntity> GetPromotionInfoByPid(int pid)
        {
            if (pid == 0)
            {
                throw new ArgumentException("pid");
            }
            try
            {
                string sql = @"SELECT ps.PromotionId,ps.Sku,ps.DiscountRate,ps.DiscountPrice,p.SupplierId,p.PromotionName,p.StartTime,p.EndTime,p.PromotionLable,p.PromotionCost,
                                 p.PromotionStatus,p.PromotionType,p.CreateTime,p.CreateBy,TuanNumbers FROM PromotionSku AS ps INNER JOIN Promotions AS p ON p.id = ps.PromotionId 
                                   WHERE p.Id = @pid AND p.PromotionStatus = 2";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@pid", pid);
                return DbSFO2ORead.ExecuteSqlList<PromotionEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }


        }
        /// <summary>
        /// 获取有效的促销信息
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        public IList<PromotionEntity> GetPromotionInfoBySku(string sku)
        {
            if (string.IsNullOrEmpty(sku))
            {
                throw new ArgumentException("sku");
            }
            try
            {
                string sql = @"SELECT ps.PromotionId,ps.Sku,ps.DiscountRate,ps.DiscountPrice,p.SupplierId,p.PromotionName,p.StartTime,p.EndTime,p.PromotionLable,p.PromotionCost,
                                 p.PromotionStatus,p.PromotionType,p.CreateTime,p.CreateBy,TuanNumbers FROM PromotionSku AS ps INNER JOIN Promotions AS p ON p.id = ps.PromotionId 
                                   WHERE ps.sku = @sku AND p.PromotionStatus = 2";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@sku", sku);
                return DbSFO2ORead.ExecuteSqlList<PromotionEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }


        }

        public IList<PromotionEntity> GetPromotionSPU(int language)
        {
            string sql = @"SELECT pi1.Spu as Spu
                            FROM dbo.Promotions AS p 
                            LEFT JOIN PromotionSku AS ps ON ps.PromotionId=p.Id 
                            LEFT JOIN SkuInfo AS si ON PS.Sku=Si.Sku
                            LEFT JOIN ProductInfo AS pi1 ON pi1.id = si.SpuId
                            LEFT JOIN ProductImage AS pi2 ON pi2.Spu = pi1.Spu
                            WHERE pi1.LanguageVersion=@language AND si.[Status] IN (3,4) AND p.PromotionStatus=2 AND p.PromotionType=2";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@language", language);
            return DbSFO2ORead.ExecuteSqlList<PromotionEntity>(sql, parameters);
        }
    }
}
