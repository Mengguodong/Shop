using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Pay
{
    /// <summary>
    /// 支付回调数据实体
    /// </summary>
    public class JuHeFuResponseModel
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 商户编号
        /// </summary>
        public string merid { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string merorderid { get; set; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string tradeid { get; set; }
        /// <summary>
        /// 交易完成时间
        /// </summary>
        public string tradedate { get; set; }
        /// <summary>
        /// 支付交易结果
        /// </summary>
        public string success { get; set; }
        /// <summary>
        /// 支付交易成功金额
        /// </summary>
        public decimal successmoney { get; set; }
        /// <summary>
        /// 支付渠道编号
        /// </summary>
        public string paychannel { get; set; }
        /// <summary>
        /// 渠道服务提供方交易流水号
        /// </summary>
        public string channeltradeid { get; set; }
        /// <summary>
        /// 支付卡号/手机号  
        /// </summary>
        public string cardid { get; set; }
        /// <summary>
        /// 用户ID  
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 商户附加信息
        /// </summary>
        public string extra { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// MD5校验值
        /// </summary>
        public string md5 { get; set; }
        /// <summary>
        /// 签名信息
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 通知方式
        /// </summary>
        public string Internal { get; set; }


    }
}
