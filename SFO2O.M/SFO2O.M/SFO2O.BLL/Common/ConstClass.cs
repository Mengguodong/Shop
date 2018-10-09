using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.BLL.Common
{
    public class ConstClass
    {
        /// <summary>
        /// 
        /// </summary>
        public static int RedisDefaultCacheMinutes = 60;

        public static int RedisCacheMinutesOneDay = 1440;
        /// <summary>
        /// 登录用户cookie
        /// </summary>
        public static readonly string LoginUserCookieKey = "sfo2o_user";

        public static readonly string ShoppingTaxTipCookieKey = "sfo2o_taxtip";


        public const string RedisKey4MPrefix = "wdnzmt9_";
        public const string RedisKeyIndexModules = "Key_IndexModules";
        public const string RedisKeyCategoryModules = "Key_CategoryModules";
        public const string RedisKeyLoginUser = "Key_LoginUser";
        public const string RedisKeyCategroySpuAttributes = "Key_CategroySpuAttributes";
        public const string RedisKey4ExchangeRates = "ExchangeRates";
        public const string RedisKeyHotWords = "Key_SearchHotWords";
        public const string RedisKeyIndexCMSHotProducts = "Key_IndexCMSHotProducts";
        public const string RedisKeyIndexBannerImagesModules = "Key_IndexBannerImagesModules";
        public const string Key_SupermarketBannerImages = "Key_SupermarketBannerImages";
        public const string Key_SupermarketBerserkProduct = "Key_SupermarketBerserkProduct";
        public const string Key_SupermarketSingleBannerImage = "Key_SupermarketSingleBannerImage";
        public const string RedisKeyIndexCMSCustom = "Key_IndexCMSCustom";
        public const string RedisKeyIndexNewProduct = "Key_IndexNewProduct";
        //2017年7月4日新添加
        public const string RedisKeyPList = "Key_PList_=";
        public const string RedisKeyBrandEntity = "Key_BrandEntity_=";
        public const string RedisKeyBrandPList = "Key_BrandPList_";
        public const string RedisKeyItemSPU = "Key_Item_SPU_";
        /// <summary>
        /// 登录用到的Session
        /// </summary>
        public const string SessionKeyMLoginUser = "SFO2O.M.USERID";

        /// <summary>
        /// 默认图片路径
        /// </summary>
        public const string DefaultListImageUrl = "";

        /// <summary>
        /// int类型错误约束
        /// </summary>
        public const decimal ErrorContractDecimal = -1000000M;

    }
}
