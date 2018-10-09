using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;

using SFO2O.Utility.Uitl;

namespace SFO2O.Payment.BLL.EPayLinks
{
    /// <summary>
    /// 类名：Notify
    /// 功能：通知处理类
    /// </summary>
    public class EpayNotify
    {
        #region 字段
        private string _partner = "";               //商户编号
        private string _key = "";                   //商户的私钥
        private string _zfbkey = "";                //支付宝商户的私钥
        private string _input_charset = "";         //编码格式
        private string _sign_type = "";             //签名方式

        private string Https_veryfy_url = "";
        #endregion


        /// <summary>
        /// 构造函数
        /// 从配置文件中初始化变量
        public EpayNotify()
        {
            //初始化基础配置信息
            _partner = ConfigHelper.EPartner.Trim();
            _key = ConfigHelper.EKey.Trim();
            _zfbkey = ConfigHelper.ZFBKey.Trim();
            _input_charset = ConfigHelper.EInputCharset.Trim().ToLower();
            _sign_type = ConfigHelper.ESignType.Trim().ToUpper();
        }

        /// <summary>
        ///  验证签名
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <param name="sign">网关服务器生成的签名结果</param>
        /// <returns>验证结果</returns>
        public bool Verify(SortedDictionary<string, string> inputPara, string sign)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>();

            //过滤空值、sign参数
            sPara = Core.FilterPara(inputPara);

            //获取待签名字符串
            string preSignStr = Core.CreateLinkString(sPara);
            //在待签名字符串中加入商户私钥KEY
            preSignStr = preSignStr + "&key=" + _key;

            //获得签名验证结果
            bool isSgin = false;
            isSgin = EpayEncrypt.Verify(preSignStr, sign);

            return isSgin;
        }

        /// <summary>
        /// 获取待签名字符串（调试用）
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <returns>待签名字符串</returns>
        private string GetPreSignStr(SortedDictionary<string, string> inputPara)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>();

            //过滤空值、sign与sign_type参数
            sPara = Core.FilterPara(inputPara);

            //获取待签名字符串
            string preSignStr = Core.CreateLinkString(sPara);

            return preSignStr;
        }

    }
}