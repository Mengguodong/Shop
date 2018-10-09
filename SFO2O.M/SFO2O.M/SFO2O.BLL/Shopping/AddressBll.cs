using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Model.Shopping;
using SFO2O.DAL.Shopping;
using SFO2O.Utility.Uitl;
using SFO2O.BLL.Common;
using SFO2O.Utility.Cache;
using SFO2O.Model.Common;
using SFO2O.Utility.Extensions;

namespace SFO2O.BLL.Shopping
{
    public class AddressBll
    {
        private AddressDal dal = new AddressDal();

        /// <summary>
        /// 取用户当前区域的地址列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public IList<AddressModel> GetAddressList(int userId, int countryId,int language)
        {
            try
            {
                string key = "Address_"+userId+"_"+countryId;
                //return RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, key, () => dal.GetAddressList(userId, countryId), 5);
                return dal.GetAddressList(userId, countryId,language);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return null;
        }
         /// <summary>
        /// 添加新地址
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddAddress(AddressEntity model)
        {
            try
            {
                return dal.AddAddress(model);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return 0;
        }
        public bool Edit(AddressEntity model)
        {
            try
            {
                return dal.Edit(model);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return false;
        }
        /// <summary>
        /// 设置默认地址
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public bool SetDefaultAddress(int userId, int id, int countryId)
        {
            try
            {
                return dal.SetDefaultAddress(userId, id, countryId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return false;
        }
         /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id,int userId)
        {
            try
            {
                return dal.Delete(id,userId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return false;
        }
        /// <summary>
        /// 查询用户的地址数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public int GetUserAddressCount(int userId, int countryId)
        {
            try
            {
                return dal.GetUserAddressCount(userId,countryId);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return 0;
        }
        /// <summary>
        /// 地址详情 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AddressEntity GetAddressById(int id)
        {
            try
            {
                return dal.GetAddressById(id);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return null;
        }
        /// <summary>
        /// 获取所有省份
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public IList<ProvinceModel> GetAllProvince(string country,int language)
        {
            try
            {
                return RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, "Province_" + language, () => dal.GetAllProvince(country,language), 1440);//1440min=1天
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return null;
        }
        
        /// <summary>
        /// 获取省下所有城市
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public List<CityModel> GetCityListByProvince(string provinceId,int language)
        {
            try
            {
                var list = GetAllCity(language);
                if (list != null && list.ToList().Count > 0)
                {
                    return list.Where(c => c.ParentId == provinceId).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return null;
        }
        /// <summary>
        /// 获取省下所有城市
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public List<AreaModel> GetAreaListByCity( string cityId, int language)
        {
            try
            {
                var list = GetAllArea(language);
                if (list != null && list.ToList().Count > 0)
                {
                    return list.Where(c => c.ParentId == cityId).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return null;
        }
        /// <summary>
        /// 获取所有城市
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        private IList<CityModel> GetAllCity(int language)
        {
            try
            {
                return RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, "City_" + language, () => dal.GetAllCity(language), 1440);//1440min=1天
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return null;
        }
        /// <summary>
        /// 获取所有区县
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        private IList<AreaModel> GetAllArea(int language)
        {
            try
            {
                return RedisCacheHelper.AutoCache(ConstClass.RedisKey4MPrefix, "Area2_" + language, () => dal.GetAllArea(language), 1440);//1440min=1天
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return null;
        }

        /// <summary>
        /// 获取订单配送地址
        /// </summary>
        /// <param name="provinceId"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>

        public string GetOrderAddressById(string provinceId, string cityId, string areaId, int language)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                var entity = dal.GetOrderAddressById(provinceId, cityId, areaId, language);
                if (entity != null)
                {
                    sb.Append(entity.ProvinceName);
                    sb.Append(entity.CityName.Replace("市辖区", ""));
                    sb.Append(entity.AreaName);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("订单获取配送地址异常：{0}",ex.Message));
            }
            return sb.ToString();
        }
    }
}
