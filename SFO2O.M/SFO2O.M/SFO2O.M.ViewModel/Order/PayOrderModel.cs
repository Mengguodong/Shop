using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFO2O.Model.Shopping;

namespace SFO2O.M.ViewModel.Order
{
    public class PayOrderModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal Totalfee { get; set; }
        /// <summary>
        /// 货币种类
        /// </summary>
        public PayCurrency PayCurrencys { get; set; }

        /// <summary>
        /// 0：网页版（默认），1：手机版
        /// </summary>
        public int Storeoitype { get; set; }

        /// <summary>
        /// ip可以为空
        /// </summary>
        public string Createip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string orderCreateIp { get; set; }

        /// <summary>
        /// 支付货款
        /// </summary>
        public decimal OrderGoodsAmount { get; set; }

        /// <summary>
        /// 支付税款
        /// </summary>
        public decimal OrderTaxAmount { get; set; }

        /// <summary>
        /// 支付运费
        /// </summary>
        public decimal OrderFreight { get; set; }

        /// <summary>
        /// Receiver
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// PassPortType
        /// </summary>
        public int PassPortType { get; set; }

        /// <summary>
        /// PassPortNum
        /// </summary>
        public string PassPortNum { get; set; }

        /// <summary>
        /// Userid
        /// </summary>
        public string Userid { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        ///银行直连参数
        /// </summary>
        public string PayId { get; set; }

        /// <summary>
        ///订单包裹
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        ///父订单号
        /// </summary>
        public string ParentOrderCode { get; set; }

        /// <summary>
        /// TaxType
        /// </summary>
        public int TaxType { get; set; }

        /// <summary>
        ///TeamCode
        /// </summary>
        public string TeamCode { get; set; }

        /// <summary>
        /// PayPlatform
        /// </summary>
        public int PayPlatform { get; set; }

        /// <summary>
        /// 判断是否是支付宝
        /// </summary>
        public bool isAliPay { get; set; }
    }

    
    public enum PayCurrency
    {
        /// <summary>
        /// 人民币：RMB；港币：HKD；美元：USD
        /// </summary>
        RMB,
        /// <summary>
        /// 港币：HKD；
        /// </summary>
        HKD,
        /// <summary>
        /// 港币：HKD；
        /// </summary>
        USD
    }
}
