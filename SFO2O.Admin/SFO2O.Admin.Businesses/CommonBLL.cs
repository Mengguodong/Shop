using SFO2O.Admin.Common;
using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Businesses
{
    public static class CommonBLL
    {

        private static string keyDicsInfo = "HT_DicsInfo_HT";

        private static string keySupplierInfo = "ht_SuppliersInfo";

        /// <summary>
        /// 获取行业类型描述列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IList<DicsModel> GetDicsInfoByKey(string key)
        {
            try
            {
                // 全部的Dics 
                IList<DicsModel> lstDics = RedisCacheHelper.Get<List<DicsModel>>(keyDicsInfo);
                if (lstDics == null || lstDics.Where(p=>String.IsNullOrWhiteSpace(p.KeyName)).Count() >0)
                {
                    lstDics = new DicsBLL().GetAllDicsInfo();

                    RedisCacheHelper.Add(keyDicsInfo, lstDics, 60);
                }

                // 根据DicsType 获取相关的字典属性
                var result = (from n in lstDics
                              where n.DicType == key && n.LanguageVersion == (int)LanguageEnum.TraditionalChinese
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

        /// <summary>
        /// 获取全部的商家信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetSuppliers()
        {
            try
            {
                // 全部的Dics 
                Dictionary<int, string> dicSuppliers = RedisCacheHelper.Get<Dictionary<int, string>>(keySupplierInfo);
                if (dicSuppliers == null)
                {
                    dicSuppliers = new Supplier.SupplierBLL().GetSupplierNames();

                    RedisCacheHelper.Add(keyDicsInfo, dicSuppliers, 60);
                }

                return dicSuppliers;
            }
            catch
            {
                return null;
            }
        }

    }
}
