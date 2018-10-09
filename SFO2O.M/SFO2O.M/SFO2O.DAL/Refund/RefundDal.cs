using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Model.Refund;
using SFO2O.EntLib.DataExtensions;
using System.Data;
using SFO2O.Utility.Uitl;

namespace SFO2O.DAL.Refund
{
    public class RefundDal:BaseDal
    {
        /// <summary>
        /// 退款/退货单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IList<RefundModel> GetRefundList(int userId, int language,int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            string sql = @"with tb
                        As
                        (
                        select ROW_NUMBER() over(order by ro.Createtime desc) as rindex,  ro.RefundCode,ro.OrderCode,ro.RefundStatus,ro.RefundType,p.Name,ro.TotalAmount as RefundTotalAmount,ro.RMBTotalAmount,rp.unitPrice,rp.RMBUnitPrice,rp.TaxRate, img.ImagePath,o.TotalAmount as OrderTotalAmount,sk.MainDicValue,sk.MainValue,sk.SubDicValue,sk.SubValue,p.NetWeightUnit,p.NetContentUnit from RefundOrderInfo ro
                        inner join  RefundOrderProducts rp on ro.RefundCode=rp.RefundCode
                        inner join ProductInfo p on p.Spu=rp.Spu and p.LanguageVersion=@LanguageVersion
                        inner join SkuInfo sk on sk.Spu=p.Spu and sk.Sku=rp.Sku  and p.Id=sk.SpuId
                        inner join ProductImage img on img.Spu=rp.Spu
                        inner join OrderInfo o on o.OrderCode=ro.OrderCode
                        where img.SortValue=1 and ro.RefundStatus>0 and ro.UserId=@UserId and o.OrderStatus>=0)

                        select RefundCode,OrderCode,RefundStatus,RefundType,Name,RefundTotalAmount,RMBTotalAmount,unitPrice,RMBUnitPrice,TaxRate, ImagePath, 
                        OrderTotalAmount,MainDicValue,MainValue,SubDicValue,SubValue,NetWeightUnit,NetContentUnit,
                        (select max(rindex) from tb) as TotalRecord 
                        from tb where rindex between @StartIndex and @EndIndex";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@UserId", userId);
            parameters.Append("@StartIndex",startIndex);
            parameters.Append("@EndIndex",endIndex);
            parameters.Append("@LanguageVersion",language);

            var list = DbSFO2ORead.ExecuteSqlList<RefundModel>(sql, parameters);
            if (list.Any())
            {
                totalRecords = list.First().TotalRecord;
            }
            return list;
        }


        /// <summary>
        /// 退款详情 
        /// </summary>
        /// <param name="refundId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public RefundInfoModel GetRefundInfo(string refundId,int userId)
        {
            RefundInfoModel model = new RefundInfoModel();
            string sql = @"select ISNULL(rp.HuoLi,0.00) AS HuoLi,ro.RefundCode,ro.OrderCode,ro.RefundStatus,ro.RefundType,p.Name,ro.TotalAmount as RefundTotalAmount,ro.RMBTotalAmount as RMBRefundTotalAmount , ro.SupplierId,isnull(ro.ExchangeRate,0)ExchangeRate,
                            rp.unitPrice,rp.TaxRate, img.ImagePath,sk.MainDicValue,sk.MainValue,sk.SubDicValue,sk.SubValue ,RefundReason,RefundDescription,ro.ImagePath as RefundImagePath,
                            ro.Commission,DutyAmount,RmbDutyAmount,NoPassReason,isnull(ProductStatus,1)ProductStatus,CompletionTime,ro.CreateTime,PickupTime,ToBePickUpTime,rp.Coupon
                            from RefundOrderInfo ro
                            left join  RefundOrderProducts rp on ro.RefundCode=rp.RefundCode
                            left join ProductInfo p on p.Spu=rp.Spu
                            left join SkuInfo sk on sk.Sku=p.Spu and sk.Sku=rp.Sku
                            left join ProductImage img on img.Spu=rp.Spu
                            left join OrderInfo o on o.OrderCode=ro.OrderCode
                            where img.SortValue=1 and ro.RefundStatus>0 and o.OrderStatus>=0 and ro.RefundCode=@RefundCode and ro.UserId=@UserId";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@UserId", userId);
            parameters.Append("@RefundCode", refundId);
            var list = DbSFO2ORead.ExecuteSqlList<RefundInfoModel>(sql, parameters);
            if (list.Any())
            {
                return list.First();
            }
            return model;
        }
        /// <summary>
        /// 退款商品列表
        /// </summary>
        /// <param name="refundId"></param>
        /// <returns></returns>
        public List<RefundProductEntity> GetRefundProducts(string refundId,int language)
        {
            string sql = @"select rp.*,p.Name,sk.MainDicValue,sk.MainValue,sk.SubDicValue,sk.SubValue,img.ImagePath,p.NetWeightUnit,p.NetContentUnit from RefundOrderProducts rp
                        inner join ProductInfo p on p.Spu=rp.Spu and p.LanguageVersion=@LanguageVersion
                        inner join SkuInfo sk on sk.Spu=rp.Spu and sk.Sku=rp.Sku 
                        inner join ProductImage img on img.Spu=rp.Spu and SortValue=1 Where RefundCode=@RefundCode";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@RefundCode", refundId);
            parameters.Append("@LanguageVersion", language);
            var list = DbSFO2ORead.ExecuteSqlList<RefundProductEntity>(sql, parameters);
            return list.ToList();
        }
        /// <summary>
        /// 生成退款单顺序号
        /// </summary>
        /// <param name="refundCode"></param>
        /// <returns></returns>
        public int GetRefundCodeNo(string orderCode)
        {
            string sql = "Select Count(1) From RefundOrderInfo Where OrderCode=@OrderCode";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);
            object obj = DbSFO2ORead.ExecuteScalar(CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// 新增退款单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddRefund(RefundOrderEntity model)
        {
            string sql = @"Insert Into RefundOrderInfo(RefundCode,RefundType,OrderCode,UserId,RefundReason,RefundDescription,RefundStatus,TotalAmount,RMBTotalAmount,CreateTime,CreateBy,ImagePath,ExchangeRate,SupplierId,RegionCode,ProductStatus,Commission) Values
                            (@RefundCode,@RefundType,@OrderCode,@UserId,@RefundReason,@RefundDescription,@RefundStatus,@TotalAmount,@RMBTotalAmount,@CreateTime,@CreateBy,@ImagePath,@ExchangeRate,@SupplierId,@RegionCode,@ProductStatus,@Commission);";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@RefundCode",model.RefundCode);
            parameters.Append("@RefundType", model.RefundType);
            parameters.Append("@OrderCode",model.OrderCode);
            parameters.Append("@UserId",model.UserId);
            parameters.Append("@RefundReason",model.RefundReason);
            parameters.Append("@RefundDescription", model.RefundDescription);
            parameters.Append("@RefundStatus",model.RefundStatus);
            parameters.Append("@TotalAmount",model.TotalAmount);
            parameters.Append("@RMBTotalAmount",model.RMBTotalAmount);
            parameters.Append("@CreateTime",model.CreateTime);
            parameters.Append("@CreateBy",model.CreateBy);
            parameters.Append("@ImagePath",model.ImagePath);
            parameters.Append("@ExchangeRate",model.ExchangeRate);
            parameters.Append("@SupplierId",model.SupplierId);
            parameters.Append("@RegionCode",model.RegionCode);
            parameters.Append("@ProductStatus", model.ProductStatus);
            parameters.Append("@Commission", model.Commision);
            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;
        }
        /// <summary>
        /// 退款单商品
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool AddRefundProduct(RefundProductEntity model,string orderCode)
        {
            string sql = @"Insert Into RefundOrderProducts(RefundCode,spu,sku,Quantity,UnitPrice,RMBUnitPrice,TaxRate,IsBearDuty,HuoLi,Coupon) Values(@RefundCode,@spu,@sku,@Quantity,@UnitPrice,@RMBUnitPrice,@TaxRate,@IsBearDuty,@HuoLi,@Coupon);";
           // sql += "Update OrderProducts Set RefundQuantity=isnull(RefundQuantity,0)+1 Where OrderCode=@OrderCode And Spu=@spu and Sku=@sku;";
//            sql += @"if exists(select * from OrderProducts where ordercode=@OrderCode and Quantity=RefundQuantity)
//                        begin
//	                        update OrderInfo set OrderStatus=5 where OrderCode=@OrderCode
//                        end";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@RefundCode", model.RefundCode);
            parameters.Append("@spu",model.Spu);
            parameters.Append("@sku",model.Sku);
            parameters.Append("@Quantity",model.Quantity);
            parameters.Append("@UnitPrice",model.UnitPrice);
            parameters.Append("@RMBUnitPrice",model.RMBUnitPrice);
            parameters.Append("@TaxRate",model.TaxRate);
            parameters.Append("@IsBearDuty", model.IsBearDuty);
            parameters.Append("@HuoLi", model.HuoLi);
            parameters.Append("@Coupon", model.Coupon);
            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;
        }
        /// <summary>
        /// 更新退款商品数量
        /// </summary>
        /// <param name="ordercode"></param>
        /// <param name="spu"></param>
        /// <param name="sku"></param>
        /// <returns></returns>
        public bool UpdateRefundQuantity(string ordercode, string spu, string sku)
        {
            string sql = "Update OrderProducts Set RefundQuantity=isnull(RefundQuantity,0)+1 Where OrderCode=@OrderCode And Spu=@spu and Sku=@sku;";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@spu", spu);
            parameters.Append("@sku", sku);
            parameters.Append("@OrderCode", ordercode);
            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;
        }
        /// <summary>
        /// 修改申诉
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool EditRefund(RefundOrderEntity model)
        {
            string sql = @"Update  RefundOrderInfo 
                           Set RefundReason=@RefundReason,RefundType=@RefundType,RefundDescription=@RefundDescription,RefundStatus=@RefundStatus,ImagePath=@ImagePath,ProductStatus=@ProductStatus 
                            Where RefundCode=@RefundCode;";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@RefundCode", model.RefundCode);
            parameters.Append("@RefundType",model.RefundType);
            parameters.Append("@RefundReason", model.RefundReason);
            parameters.Append("@RefundDescription", model.RefundDescription);
            parameters.Append("@RefundStatus", model.RefundStatus);
            parameters.Append("@ImagePath", model.ImagePath);
            parameters.Append("@ProductStatus", model.ProductStatus);
            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;
        }
        /// <summary>
        /// 申诉详情
        /// </summary>
        /// <param name="refundCode">退款单号</param>
        /// <returns></returns>
        public RefundOrderEntity GetRefundOrderInfo(string refundCode)
        {
            string sql = "Select * From RefundOrderInfo Where RefundCode=@RefundCode";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@RefundCode", refundCode);

            var list = DbSFO2ORead.ExecuteSqlList<RefundOrderEntity>(sql, parameters).ToList();
            if (list.Any())
            {
                return list.First();
            }
            return new RefundOrderEntity();
        }
        /// <summary>
        /// 该商品是否还能进行申诉
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="sku"></param>
        /// <returns></returns>
        public bool IsCanRefund(string orderCode,string sku)
        {
            bool isCan = false;
            string sql = @"select Quantity,RefundQuantity from OrderProducts where OrderCode=@OrderCode and sku=@sku";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);
            parameters.Append("@sku", sku);

            DataTable dt = DbSFO2ORead.ExecuteDataSet(CommandType.Text, sql, parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                isCan = Convert.ToInt32(dt.Rows[0][0]) > Convert.ToInt32(dt.Rows[0][1]) ? true : false;
            }
            return isCan;
        }
        public List<RefundProductEntity> getOrderProductCount(string orderCode, string sku)
        {
            string sql = @"SELECT roi.RefundCode FROM RefundOrderInfo AS roi LEFT JOIN RefundOrderProducts AS rop ON rop.RefundCode = roi.RefundCode WHERE rop.Sku=@sku AND roi.OrderCode=@OrderCode and roi.RefundStatus!=-1";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);
            parameters.Append("@sku", sku);
            var list = DbSFO2ORead.ExecuteSqlList<RefundProductEntity>(sql, parameters);
            return list.ToList();
        }
    }
}
