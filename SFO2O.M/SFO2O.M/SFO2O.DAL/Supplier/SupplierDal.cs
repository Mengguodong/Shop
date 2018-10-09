using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFO2O.EntLib.DataExtensions;
using SFO2O.Model.Supplier;
using SFO2O.Utility.Uitl;
using System.Data;

namespace SFO2O.DAL.Supplier
{
    public class SupplierDal : BaseDal
    {

        /// <summary>
        /// 获取店铺头信息
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public SupplierEntity GetStoreInfoById(int supplierID)
        {
            string sql = @"
                        SELECT  Supplier.SupplierID, Supplier.CompanyName, Supplier.CompanyName_Sample, Supplier.CompanyName_English, SupplierStore.StoreName, 
                                SupplierStore.StoreLogoPath, SupplierStore.StoreBannerPath, 
                                SupplierStore.StorePageDesc,Supplier.Address, Supplier.Description, Supplier.Description_Sample, Supplier.Description_English
                                ,Supplier.Address_Sample ,Supplier.Address_English
                        FROM    Supplier (nolock) INNER JOIN  SupplierStore (nolock)
                                ON Supplier.SupplierID = SupplierStore.SupplierID
                        WHERE	Supplier.SupplierID =@SupplierID";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@SupplierID", supplierID);

            return DbSFO2ORead.ExecuteSqlFirst<SupplierEntity>(sql, parameters);
        }

        /// <summary>
        /// 根据用户Id查询收货地址
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IList<SupplierAdddressEntity> GetDefaultBySupplierId(int supplierID)
        {
            string sql = @"select  s.Id,s.CustomerId,p.ProvinceName,c.CityName,a.AreaName,s.AddrProvince,s.AddrCity,s.AddrArea,s.PostCode,
                                   s.Address,s.Receiver,s.Phone,s.IsDefault,s.CreateTime,s.CreateBy,s.UpdateTime,s.UpdateBy,s.IsEnable
                           from    SupplierAddress as s (nolock) inner join Province as p (nolock) on s.AddrProvince = p.ProvinceId 
                                   inner join City as c (nolock) on s.AddrCity = c.CityId inner join Area as a (nolock) on s.AddrArea = a.AreaId 
                           where   IsEnable='true' and CustomerId=@SupplierID ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@SupplierID", supplierID);

            return DbSFO2ORead.ExecuteSqlList<SupplierAdddressEntity>(sql, parameters);
        }
    }
}
