using SFO2O.Supplier.Common;
using SFO2O.Supplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Businesses
{
    public static class CommonBLL
    {

        private static string keyDicsInfo = "ht_DicsInfo";

        /// <summary>
        /// 获取行业类型描述列表,
        /// _xcf 获取单位（1简体，2繁体，3英文）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IList<DicsModel> GetDicsInfoByKey(string key)
        {
            try
            {
                // 全部的Dics 
                IList<DicsModel> lstDics = RedisCacheHelper.Get<List<DicsModel>>(keyDicsInfo);
                if (lstDics == null)
                {
                    lstDics = new DicsBLL().GetAllDicsInfo();

                    RedisCacheHelper.Add(keyDicsInfo, lstDics, 60);
                }

                // 根据DicsType 获取相关的字典属性,
                var result = (from n in lstDics
                              where n.DicType == key && n.LanguageVersion == (int)LanguageEnum.SimplifiedChinese
                              select n).ToList<DicsModel>();

                return result;
            }
            catch
            {
                return null;
            }
        }

        public static IList<DicsModel> GetDicsInfoByKeyAllLanguage(string key)
        {
            try
            {
                // 全部的Dics 
                IList<DicsModel> lstDics = RedisCacheHelper.Get<List<DicsModel>>(keyDicsInfo);
                if (lstDics == null)
                {
                    lstDics = new DicsBLL().GetAllDicsInfo();

                    RedisCacheHelper.Add(keyDicsInfo, lstDics, 60);
                }

                // 根据DicsType 获取相关的字典属性
                var result = (from n in lstDics
                              where n.DicType == key
                              select n).ToList<DicsModel>();

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
