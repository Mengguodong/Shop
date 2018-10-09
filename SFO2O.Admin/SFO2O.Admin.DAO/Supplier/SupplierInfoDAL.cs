using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using System.Data;
using SFO2O.Admin.Models.Supplier;
using SFO2O.Admin.ViewModel.Supplier;
using SFO2O.Admin.ViewModel;
using SFO2O.Admin.Common;
using System.Data.Common;
using SFO2O.Admin.Models;

namespace SFO2O.Admin.DAO.Supplier
{
    public class SupplierInfoDAL : BaseDao
    {
        public Dictionary<int, string> GetSupplierNames()
        {
            const string sql = "SELECT SupplierID,CompanyName FROM supplier(NOLOCK) WHERE Status=1";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            var dt = db.ExecuteSqlDataSet(sql);

            if (null == dt || dt.Tables == null || dt.Tables.Count == 0 || dt.Tables[0] == null)
            {
                return null;
            }

            var result = new Dictionary<int, string>();

            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                var CompanyName = dr["CompanyName"] == DBNull.Value ? "" : dr["CompanyName"].ToString();

                result.Add(Convert.ToInt32(dr["SupplierID"]), CompanyName);
            }

            return result;
        }

        public PageOf<SupplierAbstractModel> GetSupplierList(SupplierQueryModel query, int pageSize, int pageNo)
        {
            const string sql = @"
                    WITH SN AS
                    (
                    SELECT a.SupplierId,SUM(a.SkuNumber) AS SkuNumber FROM (
                        SELECT pin.SupplierId,COUNT(distinct si.sku) AS SkuNumber FROM SkuInfo(NOLOCK) si
                        INNER JOIN ProductInfo(NOLOCK) pin
                        ON si.SpuId = pin.id AND pin.LanguageVersion=2
                        LEFT JOIN Stock(NOLOCK) t ON t.Sku=si.Sku
                        WHERE t.ForOrderQty >0 and [Status]=3
                        GROUP BY pin.SupplierId,pin.MinForOrder
                        HAVING SUm(ISNULL(t.ForOrderQty,0)) - pin.MinForOrder >0)a
                        GROUP BY a.SupplierId
                    )
                    SELECT * FROM (
                    SELECT ROW_NUMBER() OVER (ORDER BY s.Createtime DESC) AS RowNumber,s.SupplierID, su.UserName,s.CompanyName,s.CreateTime,Case s.status WHEN 1 THEN '正常' WHEN 2 THEN '凍結' END AS supplierStatus,sn.SkuNumber
                    FROM SupplierUser(NOLOCK) su
                    INNER JOIN Supplier(NOLOCK) s
                    ON s.SupplierID=su.SupplierID AND su.IsAdmin=1
                    LEFT JOIN SN 
                    ON sn.SupplierId=su.SupplierID
                    WHERE s.Createtime>=@StartTime AND s.CreateTime<@EndTime {0}) a
                    WHERE a.RowNumber > (@pageNo-1)*@pageSize AND a.RowNumber <=@pageNo*@pageSize";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            var countParas = db.CreateParameterCollection();

            var startTime = new DateTime(query.CreateTimeStart.Year, query.CreateTimeStart.Month, query.CreateTimeStart.Day);
            var endTime = new DateTime(query.CreateTimeEnd.Year, query.CreateTimeEnd.Month, query.CreateTimeEnd.Day, 23, 59, 59, 999);

            parameters.Append("StartTime", startTime);
            parameters.Append("EndTime", endTime);
            parameters.Append("pageSize", pageSize);
            parameters.Append("pageNo", pageNo);

            countParas.Append("StartTime", startTime);
            countParas.Append("EndTime", endTime);
            countParas.Append("pageSize", pageSize);
            countParas.Append("pageNo", pageNo);

            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(query.AccountName))
            {
                sb.Append(" AND su.UserName like @UserName");
                parameters.Append("UserName", "%" + query.AccountName + "%");
                countParas.Append("UserName", "%" + query.AccountName + "%");
            }

            if (!String.IsNullOrWhiteSpace(query.CompanyName))
            {
                sb.Append(" AND s.CompanyName like @CompanyName");
                parameters.Append("CompanyName", "%" + query.CompanyName + "%");
                countParas.Append("CompanyName", "%" + query.CompanyName + "%");
            }

            if (query.SupplierStatus > 0)
            {
                sb.Append(" AND s.Status=@Status");
                parameters.Append("Status", query.SupplierStatus);
                countParas.Append("Status", query.SupplierStatus);
            }

            var reslut = db.ExecuteSqlList<SupplierAbstractModel>(String.Format(sql, sb.ToString()), parameters);

            const string countSql = @"
                SELECT COUNT(1)
                FROM SupplierUser(NOLOCK) su
                INNER JOIN Supplier(NOLOCK) s
                ON s.SupplierID=su.SupplierID AND su.IsAdmin=1
                WHERE s.Createtime>=@StartTime AND s.CreateTime<@EndTime {0}";

            var dataCount = db.ExecuteSqlScalar<int>(String.Format(countSql, sb.ToString()), countParas);

            return new PageOf<SupplierAbstractModel>()
            {
                Items = reslut,
                PageIndex = pageNo,
                PageSize = pageSize,
                Total = dataCount
            };
        }
        /// <summary>
        /// 根据订单号获取商家信息
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public SupplierAbstractModel GetSupplierInfo(int supplierId)
        {
            var sql = @"
SELECT	s.SupplierID,
		s.UserName,
		s.CompanyName,
		s.CompanyName_Sample,
		s.CompanyName_English,
		s.TrueName
FROM	Supplier s(NOLOCK)
WHERE	s.SupplierID  = @SupplierId
";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@SupplierId", supplierId);
            return db.ExecuteSqlFirst<SupplierAbstractModel>(sql, parameters);
        }

        public SupplierDetailModel GetSupplierDetailInfo(int supplierId)
        {
            const string sql = @"
                WITH SN AS
                (
	                SELECT pin.SupplierId,COUNT(1) AS SkuNumber FROM SkuInfo(NOLOCK) si
	                INNER JOIN ProductInfo(NOLOCK) pin
	                ON si.SpuId = pin.id AND pin.LanguageVersion=2
                    LEFT JOIN Stock(NOLOCK) t ON t.Sku=si.Sku
	                WHERE pin.SupplierId=@SupplierID AND t.ForOrderQty >0 and [Status]=3
	                GROUP BY pin.SupplierId
                )
                SELECT s.*,sn.SkuNumber,ss.StoreLogoPath FROM Supplier(NOLOCK) AS s
                LEFT JOIN SN
                ON sn.SupplierId = s.SupplierID
                INNER JOIN SupplierStore(NOLOCK) AS ss
                ON ss.SupplierID = s.SupplierID
                WHERE s.SupplierID=@SupplierID

                SELECT * FROM SupplierBrand(NOLOCK) WHERE SupplierID=@SupplierID";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@SupplierID", supplierId);

            var ds = db.ExecuteDataSet(CommandType.Text, sql, parameters);

            var sd = new SupplierDetailModel();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                sd.Supplier = DataMapHelper.DataTableToObject<SupplierModel>(ds.Tables[0]);

                if (ds.Tables[1] != null && ds.Tables[1].Rows != null && ds.Tables[1].Rows.Count > 0)
                {
                    sd.Brands = DataMapHelper.DataTableToList<BrandModel>(ds.Tables[1]).ToList();
                }
            }

            return sd;
        }

        public int GetUserNameCount(string userName)
        {
            const string sql = @"SELECT count(1) FROM SupplierUser(NOLOCK) WHERE UserName=@userName";

            var db = DbSFO2OMain;
            var para = db.CreateParameterCollection();
            para.Append("userName", userName);

            return db.ExecuteSqlScalar<int>(sql, para);
        }

        public int GetCompaynNameCount(string companyName)
        {
            const string sql = @"SELECT count(1) FROM Supplier(NOLOCK) WHERE CompanyName=@CompanyName";

            var db = DbSFO2OMain;
            var para = db.CreateParameterCollection();
            para.Append("CompanyName", companyName);

            return db.ExecuteSqlScalar<int>(sql, para);
        }

        public void InsertSupplierInfo(SupplierInfoJsonModel supplierInfo, string userName)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    supplierInfo.SupplierID = InsertSupplier(supplierInfo, userName, db, tran);
                    InsertSupplierUser(supplierInfo, userName, db, tran);
                    InsertSupplierStore(supplierInfo, userName, db, tran);

                    tran.Commit();
                    connection.Close();
                    connection.Dispose();
                }
                catch (Exception ext)
                {
                    tran.Rollback();
                    connection.Close();
                    connection.Dispose();
                    throw ext;
                }
            }
        }


        public void UpdateSupplierInfo(SupplierInfoJsonModel supplierInfo, string userName)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    UpDateSupplier(supplierInfo, userName, db, tran);
                    if (!String.IsNullOrWhiteSpace(supplierInfo.PassWord))
                    {
                        UpdateSupplierUser(supplierInfo, userName, db, tran);
                    }
                    UpdateSupplierUserImgPath(supplierInfo, userName, db, tran);
                    UpdateSupplierStore(supplierInfo, userName, db, tran);

                    tran.Commit();
                    connection.Close();
                    connection.Dispose();
                }
                catch (Exception ext)
                {
                    tran.Rollback();
                    connection.Close();
                    connection.Dispose();
                    throw ext;
                }
            }
        }

        private int InsertSupplier(SupplierInfoJsonModel supplierInfo, string userName, Database db, System.Data.Common.DbTransaction tran)
        {
            const string sql = @"INSERT INTO Supplier(UserName,CompanyName,CompanyName_Sample,CompanyName_English,TrueName,Address,Address_Sample,Address_English,
                ContactPerson,ContactPerson_Sample,ContactPerson_English,ContactTel,ContactPhone,ContactFax,Description,Description_Sample,Description_English,Status
                ,UpdateTime,UpdateBy,CreateTime,CreateBy) 
                                 Values(@UserName,@CompanyName,@CompanyName_Sample,@CompanyName_English,@TrueName,@Address,@Address_Sample,@Address_English,
                @ContactPerson,@ContactPerson_Sample,@ContactPerson_English,@ContactTel,@ContactPhone,@ContactFax,@Description,@Description_Sample,@Description_English,@Status
                ,@UpdateTime,@UpdateBy,@CreateTime,@CreateBy)
                SELECT @@IDENTITY";

            var paras = db.CreateParameterCollection();
            paras.Append("UserName", supplierInfo.UserName);

            paras.Append("CompanyName", supplierInfo.CompanyName);
            paras.Append("CompanyName_Sample", supplierInfo.CompanynameSample);
            paras.Append("CompanyName_English", supplierInfo.CompanyNameEnglish);

            paras.Append("TrueName", "");

            paras.Append("Address", supplierInfo.Address);
            paras.Append("Address_Sample", supplierInfo.AddressSample);
            paras.Append("Address_English", supplierInfo.AddressEnglish);

            paras.Append("ContactPerson", supplierInfo.ConnectPeople);
            paras.Append("ContactPerson_Sample", supplierInfo.ConnectPeopleSample);
            paras.Append("ContactPerson_English", supplierInfo.ConnectPeopleEnglish);

            paras.Append("ContactTel", supplierInfo.ContactTel);
            paras.Append("ContactPhone", supplierInfo.ContactPhone);
            paras.Append("ContactFax", supplierInfo.ContactFax);
            paras.Append("Status", 1);

            paras.Append("Description", supplierInfo.Descript);
            paras.Append("Description_Sample", supplierInfo.DescriptSample);
            paras.Append("Description_English", supplierInfo.DescriptEnglish);

            paras.Append("CreateTime", DateTime.Now);
            paras.Append("CreateBy", userName);

            paras.Append("UpdateTime", DateTime.Now);
            paras.Append("UpdateBy", userName);

            var ds = db.ExecuteDataSet(CommandType.Text, sql, paras, tran);

            var result = 0;

            if (ds != null && ds.Tables.Count > 0)
            {
                result = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }

            return result;
        }

        private void InsertSupplierUser(SupplierInfoJsonModel supplierInfo, string userName, Database db, System.Data.Common.DbTransaction tran)
        {
            const string sql = @"INSERT INTO SupplierUser([SupplierID],[UserName],[Password],[Status],[IsAdmin],[ImageUrl],[CreateTime],[CreateBy],[UpdateTime],[UpdateBy])
            VALUES(@SupplierID,@UserName,@Password,@Status,@IsAdmin,@ImageUrl,@CreateTime,@CreateBy,@UpdateTime,@UpdateBy)";

            var paras = db.CreateParameterCollection();
            paras.Append("SupplierID", supplierInfo.SupplierID);
            paras.Append("UserName", supplierInfo.UserName);

            paras.Append("Password", supplierInfo.PassWord);

            paras.Append("Status", 1);
            paras.Append("IsAdmin", 1);

            paras.Append("ImageUrl", supplierInfo.ImgPath);

            paras.Append("CreateTime", DateTime.Now);
            paras.Append("CreateBy", userName);

            paras.Append("UpdateTime", DateTime.Now);
            paras.Append("UpdateBy", userName);

            db.ExecuteNonQuery(CommandType.Text, sql, paras, tran);
        }

        private void InsertSupplierStore(SupplierInfoJsonModel supplierInfo, string userName, Database db, System.Data.Common.DbTransaction tran)
        {
            const string sql = @"INSERT INTO SupplierStore([SupplierID],[StoreLogoPath],[Status],[CreateTime],[CreateBy],[UpdateTime],[UpdateBy])
            VALUES(@SupplierID,@StoreLogoPath,@Status,@CreateTime,@CreateBy,@UpdateTime,@UpdateBy)";

            var paras = db.CreateParameterCollection();
            paras.Append("SupplierID", supplierInfo.SupplierID);
            paras.Append("StoreLogoPath", supplierInfo.ImgPath);

            paras.Append("Status", 1);
            paras.Append("CreateTime", DateTime.Now);
            paras.Append("CreateBy", userName);

            paras.Append("UpdateTime", DateTime.Now);
            paras.Append("UpdateBy", userName);

            db.ExecuteNonQuery(CommandType.Text, sql, paras, tran);
        }

        private void UpDateSupplier(SupplierInfoJsonModel supplierInfo, string userName, Database db, System.Data.Common.DbTransaction tran)
        {
            const string sql = @"
            UPDATE [Supplier] SET [UserName] = @UserName,[CompanyName] = @CompanyName,[CompanyName_Sample] = @CompanyName_Sample
           ,[CompanyName_English] = @CompanyName_English,[Address] = @Address,[Address_Sample] = @Address_Sample
           ,[Address_English] = @Address_English,[ContactPerson] = @ContactPerson,[ContactPerson_Sample] = @ContactPerson_Sample
           ,[ContactPerson_English] = @ContactPerson_English,[ContactTel] = @ContactTel,[ContactPhone] = @ContactPhone,[ContactFax] = @ContactFax,[Description] = @Description
           ,[Description_Sample] = @Description_Sample,[Description_English] = @Description_English,[UpdateTime] = @UpdateTime
           ,[UpdateBy] = @UpdateBy
            WHERE supplierId = @SupplierId";

            var paras = db.CreateParameterCollection();
            paras.Append("UserName", supplierInfo.UserName);

            paras.Append("CompanyName", supplierInfo.CompanyName);
            paras.Append("CompanyName_Sample", supplierInfo.CompanynameSample);
            paras.Append("CompanyName_English", supplierInfo.CompanyNameEnglish);

            paras.Append("Address", supplierInfo.Address);
            paras.Append("Address_Sample", supplierInfo.AddressSample);
            paras.Append("Address_English", supplierInfo.AddressEnglish);

            paras.Append("ContactPerson", supplierInfo.ConnectPeople);
            paras.Append("ContactPerson_Sample", supplierInfo.ConnectPeopleSample);
            paras.Append("ContactPerson_English", supplierInfo.ConnectPeopleEnglish);

            paras.Append("ContactTel", supplierInfo.ContactTel);
            paras.Append("ContactPhone", supplierInfo.ContactPhone);
            paras.Append("ContactFax", supplierInfo.ContactFax);

            paras.Append("Description", supplierInfo.Descript);
            paras.Append("Description_Sample", supplierInfo.DescriptSample);
            paras.Append("Description_English", supplierInfo.DescriptEnglish);

            paras.Append("UpdateTime", DateTime.Now);
            paras.Append("UpdateBy", userName);

            paras.Append("SupplierId", supplierInfo.SupplierID);

            db.ExecuteDataSet(CommandType.Text, sql, paras, tran);
        }

        private void UpdateSupplierUser(SupplierInfoJsonModel supplierInfo, string userName, Database db, System.Data.Common.DbTransaction tran)
        {
            const string sql = @"
            UPDATE [SupplierUser] SET [Password]=@Password WHERE [SupplierID]=@SupplierID";

            var paras = db.CreateParameterCollection();
            paras.Append("SupplierID", supplierInfo.SupplierID);

            paras.Append("Password", supplierInfo.PassWord);

            paras.Append("UpdateTime", DateTime.Now);
            paras.Append("UpdateBy", userName);

            db.ExecuteNonQuery(CommandType.Text, sql, paras, tran);
        }

        private void UpdateSupplierUserImgPath(SupplierInfoJsonModel supplierInfo, string userName, Database db, System.Data.Common.DbTransaction tran)
        {
            const string sql = @"
            UPDATE [SupplierUser] SET [ImageUrl]=@ImgURL WHERE [SupplierID]=@SupplierID AND IsAdmin=1";

            var paras = db.CreateParameterCollection();
            paras.Append("SupplierID", supplierInfo.SupplierID);
            paras.Append("ImgURL", supplierInfo.ImgPath);

            db.ExecuteNonQuery(CommandType.Text, sql, paras, tran);
        }


        private void UpdateSupplierStore(SupplierInfoJsonModel supplierInfo, string userName, Database db, System.Data.Common.DbTransaction tran)
        {
            const string sql = @"
            UPDATE [SupplierStore] SET [StoreLogoPath]=@StoreLogoPath WHERE [SupplierID]=@SupplierID";

            var paras = db.CreateParameterCollection();
            paras.Append("SupplierID", supplierInfo.SupplierID);

            paras.Append("StoreLogoPath", supplierInfo.ImgPath);

            paras.Append("UpdateTime", DateTime.Now);
            paras.Append("UpdateBy", userName);

            db.ExecuteNonQuery(CommandType.Text, sql, paras, tran);
        }

        public void UpdateSupplierStatus(int supplierId, int status, string userName)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    UpdateSupplierStatus(supplierId, status, userName, db, tran);
                    UpdateSupplierUserStatus(supplierId, status, userName, db, tran);
                    if (status == 2)
                    {
                        UpdateBrandStatus(supplierId, 0, db, tran);
                        UpdateProductStatus(supplierId, 5, userName, db, tran);
                    }

                    tran.Commit();
                    connection.Close();
                    connection.Dispose();
                }
                catch (Exception ext)
                {
                    tran.Rollback();
                    connection.Close();
                    connection.Dispose();
                    throw ext;
                }
            }
        }

        private void UpdateBrandStatus(int supplierId, int status, Database db, DbTransaction tran)
        {
            const string sql = @"update SupplierBrand
                                 set status=@status
                                 where SupplierId =@SupplierID ";

            var paras = db.CreateParameterCollection();
            paras.Append("SupplierID", supplierId);
            paras.Append("Status", status);

            db.ExecuteNonQuery(CommandType.Text, sql, paras, tran);
        }

        private void UpdateSupplierStatus(int supplierId, int status, string userName, Database db, System.Data.Common.DbTransaction tran)
        {
            const string sql = @"
                Update Supplier SET [Status] = @Status WHERE SupplierId = @SupplierId";

            var paras = db.CreateParameterCollection();
            paras.Append("SupplierID", supplierId);

            paras.Append("Status", status);

            paras.Append("UpdateTime", DateTime.Now);
            paras.Append("UpdateBy", userName);

            db.ExecuteNonQuery(CommandType.Text, sql, paras, tran);
        }

        private void UpdateSupplierUserStatus(int supplierId, int status, string userName, Database db, System.Data.Common.DbTransaction tran)
        {
            const string sql = @"
                Update SupplierUser SET [Status] = @Status WHERE SupplierId = @SupplierId";

            var paras = db.CreateParameterCollection();
            paras.Append("SupplierID", supplierId);

            paras.Append("Status", status);

            paras.Append("UpdateTime", DateTime.Now);
            paras.Append("UpdateBy", userName);

            db.ExecuteNonQuery(CommandType.Text, sql, paras, tran);
        }

        private void UpdateProductStatus(int supplierId, int status, string userName, Database db, System.Data.Common.DbTransaction tran)
        {
            const string sql = @"
                UPDATE SkuInfo SET Status=@Status FROM SkuInfo si
                INNER JOIN ProductInfo pin
                ON si.SpuId = pin.Id
                WHERE pin.SupplierId=@SupplierID
            ";

            var paras = db.CreateParameterCollection();
            paras.Append("SupplierID", supplierId);
            paras.Append("Status", status);

            db.ExecuteNonQuery(CommandType.Text, sql, paras, tran);
        }

        public PageOf<Models.StoreModel> GetStoreListByBrandId(int brandId, int areaId, Models.Enums.LanguageEnum languageEnum, int pageSize, int pageIndex)
        {
            var query = @" (SELECT s.Id
                          ,s.BrandId
                          ,s.AreaId
                          ,p.ProvinceName as AreaName
                          ,s.AddressCN
                          ,s.AddressEN
                          ,s.AddressHK
                          ,s.Status
                        FROM Store s(nolock) inner join Province p(nolock) on s.AreaId = p.ProvinceId and p.LanguageVersion=@LanguageVersion
                        WHERE s.BrandId=@BrandId and s.Status=1";

            if (areaId > 0)
            {
                query += " and s.AreaId=@AreaId";
            }

            query += ") pp ";
            string sql = string.Format(@" select * from (select ROW_NUMBER() OVER(order by pp.Id desc) AS RowNum,* from {0}
										) as A where A.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;",
                                  query);

            sql += string.Format(@" SELECT COUNT(1) FROM {0};", query);

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("BrandId", brandId);
            parameters.Append("AreaId", areaId);
            parameters.Append("LanguageVersion", (int)languageEnum);
            parameters.Append("PageIndex", pageIndex);
            parameters.Append("PageSize", pageSize);

            DataSet ds = db.ExecuteSqlDataSet(sql, parameters);

            return new PageOf<StoreModel>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                Items = DataMapHelper.DataSetToList<StoreModel>(ds)
            };
        }

        public BrandModel GetBrandById(int brandId)
        {
            var sql = @"with cte(BrandId,Spu,sku)
                        as
                        (
                        select distinct p.BrandId,p.Spu,s.sku 
                        from ProductInfo p(nolock)
                            inner join SkuInfo(nolock) s on p.Id=s.SpuId and s.Status=3 and p.LanguageVersion=1
                            inner join Stock st(nolock) on s.Sku=st.Sku
                        group by p.BrandId,p.Spu,s.sku,p.MinForOrder 
                        having sum(st.ForOrderQty) >= p.MinForOrder  	
                        )
                        SELECT sb.Id,sb.SupplierId,sb.NameCN,sb.NameHK,sb.NameEN,sb.Logo,sb.Banner,sb.IntroductionCN,sb.IntroductionHK,sb.IntroductionEN,sb.CountryId,d.KeyValue as CountryName,
                        sb.CategoryId,cl.CategoryName,sb.CreateTime,Status,(select count(1) from cte where BrandId = sb.Id) as OnSaleCount
                        FROM SupplierBrand sb(nolock)
                        left join (select KeyName,KeyValue from Dics where DicType = 'CountryOfManufacture' and LanguageVersion=2) as d on sb.CountryId= d.KeyName
                        left join Category_LanguageVersion cl(nolock) on cl.CategoryKey = sb.CategoryId and cl.LanguageVersion=2
                        WHERE sb.Id=@brandId";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("brandId", brandId);

            return db.ExecuteSqlFirst<BrandModel>(sql, parameters);
        }
    }
}
