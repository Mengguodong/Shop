using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFO2O.Model.Shopping;

namespace SFO2O.M.ViewModel.Order
{
    public class ZPayOrderModel
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        public string service { get; set; }
        /// <summary>
        /// 合作者身份ID
        /// </summary>
        public string partner { get; set; }
        /// <summary>
        /// 货币币种
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        ///_input_ charset 
        /// </summary>
        public string _input_charset { get; set; }

        /// <summary>
        /// 签名方式
        /// </summary>
        public string sign_type  { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 服务器异步通知页面路径
        /// </summary>
        public string notify_url { get; set; }

        /// <summary>
        /// 服务器同步通知页面路径
        /// </summary>
        public string return_url  { get; set; }

        /// <summary>
        /// 商户网站唯一订单号
        /// </summary>
        public string out_trade_no  { get; set; }

        /// <summary>
        /// 返回商户链接
        /// </summary>
        public string show_url { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 商品总价
        /// </summary>
        public decimal rmb_fee  { get; set; }
        /// <summary>
        /// 商品总价
        /// </summary>
        public decimal total_fee { get; set; }

        /// <summary>
        /// 供货方
        /// </summary>
        public string supplier { get; set; }

        /// <summary>
        /// 交易超时时间
        /// </summary>
        public string timeout_rule  { get; set; }
        
    }
}
