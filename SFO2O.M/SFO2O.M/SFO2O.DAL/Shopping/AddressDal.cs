using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Model.Shopping;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Utility.Extensions;
using SFO2O.Model.Common;
using SFO2O.Utility.Uitl;

namespace SFO2O.DAL.Shopping
{
    public class AddressDal:BaseDal
    {
        /// <summary>
        /// 获取用户的所有地址
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public IList<AddressModel> GetAddressList(int userId, int countryId,int language)
        {
            string sql = @"select a.Id,UserId,[Type],p.ProvinceId,ProvinceName, c.CityId,c.CityName,ar.AreaId,ar.AreaName,PostCode,Address,Receiver,Phone,
                            IsDefault,PapersType,PapersCode 
                            from AddressInfo a
                            inner join Province p on p.ProvinceId=a.ProvinceId and p.LanguageVersion=@LanguageVersion
                            inner join City c on c.CityId=a.CityId  and c.LanguageVersion=@LanguageVersion
                            inner join Area ar on ar.AreaId=a.AreaId  and ar.LanguageVersion=@LanguageVersion
                            where a.CountryId=(select top 1 CountryId From Country Where Id=@CountryId) and IsEnable=1 and UserId=@UserId Order by IsDefault desc,Id desc";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@CountryId",countryId);
            parameters.Append("@UserId",userId);
            parameters.Append("@LanguageVersion",language);

            var list = DbSFO2ORead.ExecuteSqlList<AddressModel>(sql, parameters);
            return list;
        }
        /// <summary>
        /// 添加新地址
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddAddress(AddressEntity model)
        {
            string sql = @"Insert into AddressInfo(UserId,[Type],CountryId,ProvinceId,CityId,AreaId,PostCode,[Address],Receiver,Phone,IsDefault,PapersType,PapersCode,CreateTime,CreateBy,IsEnable)
                           Values(@UserId,@Type,@CountryId,@ProvinceId,@CityId,@AreaId,@PostCode,@Address,@Receiver,@Phone,@IsDefault,@PapersType,@PapersCode,@CreateTime,@CreateBy,@IsEnable);select @@Identity;";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@UserId", model.UserId);
            parameters.Append("@Type", model.Type);
            parameters.Append("@CountryId", model.CountryId);
            parameters.Append("@ProvinceId", model.ProvinceId);
            parameters.Append("@CityId", model.CityId);
            parameters.Append("@AreaId", model.AreaId);
            parameters.Append("@PostCode",model.PostCode);
            parameters.Append("@Address",model.Address);
            parameters.Append("@Receiver",model.Receiver);
            parameters.Append("@Phone",model.Phone);
            parameters.Append("@IsDefault",model.IsDefault);
            parameters.Append("@PapersType",model.PapersType);
            parameters.Append("@PapersCode",model.PapersCode);
            parameters.Append("@CreateTime",model.CreateTime);
            parameters.Append("@CreateBy",model.CreateBy);
            parameters.Append("@IsEnable",model.IsEnable);

            object id = DbSFO2OMain.ExecuteScalar(CommandType.Text, sql, parameters);
            return id==null?0:id.AsInt32();
        }
        /// <summary>
        /// 编辑地址
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Edit(AddressEntity model)
        {
            string sql = @"Update AddressInfo Set ProvinceId=@ProvinceId,CityId=@CityId,AreaId=@AreaId,PostCode=@PostCode,Address=@Address,Receiver=@Receiver,Phone=@Phone,
                           PapersType=@PapersType,PapersCode=@PapersCode,UpdateTime=@UpdateTime,UpdateBy=@UpdateBy
                           Where Id=@Id And UserId=@UserId;";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@ProvinceId", model.ProvinceId);
            parameters.Append("@CityId", model.CityId);
            parameters.Append("@AreaId", model.AreaId);
            parameters.Append("@PostCode", model.PostCode);
            parameters.Append("@Address", model.Address);
            parameters.Append("@Receiver", model.Receiver);
            parameters.Append("@Phone", model.Phone);
            parameters.Append("@PapersType", model.PapersType);
            parameters.Append("@PapersCode", model.PapersCode);
            parameters.Append("@UpdateTime", model.UpdateTime);
            parameters.Append("@UpdateBy", model.UpdateBy);
            parameters.Append("@Id", model.Id);
            parameters.Append("@UserId", model.UserId);

            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;
        }
        /// <summary>
        /// 设置默认地址
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public bool SetDefaultAddress(int userId,int id,int countryId)
        {
            string sql = @"Update AddressInfo Set IsDefault=0 Where UserId=@UserId and CountryId=(select top 1 CountryId From Country Where Id=@CountryId);
                           Update AddressInfo Set IsDefault=1 Where Id=@Id;";

            var parameters = DbSFO2OMain.CreateParameterCollection();

            parameters.Append("@UserId", userId);
            parameters.Append("@CountryId", countryId);
            parameters.Append("@Id",id);
            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;
        }
        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id,int userId)
        {
            string sql = "Update AddressInfo Set IsEnable=0 Where Id=@Id And UserId=@UserId";
            var parameters = DbSFO2OMain.CreateParameterCollection();

            parameters.Append("@Id", id);
            parameters.Append("@UserId", userId);
            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;
        }
        public AddressEntity GetAddressById(int id)
        {
            AddressEntity model = new AddressEntity();
            string sql = @"Select a.*,p.ProvinceName,c.CityName,aa.AreaName From AddressInfo (nolock) a
	                        inner join Province p on p.ProvinceId=a.ProvinceId
	                        inner join City c on c.CityId=a.CityId
	                        inner join area aa on aa.AreaId=a.areaId
	                         Where a.Id=@Id";
            var parameters = DbSFO2OMain.CreateParameterCollection();

            parameters.Append("@Id", id);
            IList<AddressEntity> list = DbSFO2ORead.ExecuteSqlList<AddressEntity>(sql, parameters);
            if (list != null && list.Count > 0)
            {
                return list.First();
            }
            return model;
        }
        /// <summary>
        /// 查询用户的地址数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public int GetUserAddressCount(int userId, int countryId)
        {
            string sql = "Select count(1) From AddressInfo (nolock) Where UserId=@UserId And CountryId=(select top 1 CountryId From Country Where Id=@CountryId)  and IsEnable=1";
            var parameters = DbSFO2OMain.CreateParameterCollection();

            parameters.Append("@UserId", userId);
            parameters.Append("@CountryId", countryId);
            object cnt = DbSFO2OMain.ExecuteScalar(CommandType.Text, sql, parameters);
            return cnt == null ? 0 : cnt.AsInt32();
        }
        /// <summary>
        /// 获取所有省份
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public IList<ProvinceModel> GetAllProvince(string country,int language)
        {
            string sql = "select ProvinceId,ProvinceName,ParentId from province where IsDelete=0 and LanguageVersion=@LanguageVersion And ParentId=@ParentId AND IsSFSupport=1";
            var parameters = DbSFO2OMain.CreateParameterCollection();

            parameters.Append("@LanguageVersion", language);
            parameters.Append("@ParentId", country);

            return DbSFO2ORead.ExecuteSqlList<ProvinceModel>(sql,parameters);
        }
        /// <summary>
        /// 获取所有城市 
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public IList<CityModel> GetAllCity( int language)
        {
            string sql = "select CityId,CityName,ParentId from City where IsDelete=0  and LanguageVersion=@LanguageVersion AND IsSFSupport=1";

            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@LanguageVersion", language);
            return DbSFO2ORead.ExecuteSqlList<CityModel>(sql,parameters);
        }
        /// <summary>
        /// 获取所有区县 
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public IList<AreaModel> GetAllArea( int language)
        {
            string sql = "select AreaId,AreaName,ParentId from Area where IsDelete=0 and LanguageVersion=@LanguageVersion AND IsSFSupport=1";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@LanguageVersion", language);

            return DbSFO2ORead.ExecuteSqlList<AreaModel>(sql,parameters);
        }

        /// <summary>
        /// 获取订单配送地址
        /// </summary>
        /// <param name="provinceId"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>

        /// <param name="language"></param>
        public OrderAddressEntity GetOrderAddressById(string provinceId, string cityId, string areaId, int language)
        {
            try
            {
                const string sql = @"SELECT 
                                    (SELECT TOP 1 ProvinceName FROM dbo.Province WHERE ProvinceId=@ProvinceId AND LanguageVersion=@LanguageVersion) as ProvinceName,
                                    (SELECT TOP 1 CityName FROM dbo.City WHERE CityId=@CityId AND LanguageVersion=@LanguageVersion) as CityName,
                                    (SELECT TOP 1 AreaName FROM dbo.Area WHERE AreaId=@AreaId AND LanguageVersion=@LanguageVersion) as AreaName";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@ProvinceId", provinceId);
                parameters.Append("@CityId", cityId);
                parameters.Append("@AreaId", areaId);
                parameters.Append("@LanguageVersion", language);

                return DbSFO2ORead.ExecuteSqlFirst<OrderAddressEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }
    }
}
