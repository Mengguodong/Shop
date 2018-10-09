using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFO2O.Model.Pay;

namespace SFO2O.Payment.BLL
{
    public interface IPay
    {
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="payValue"></param>
        /// <returns></returns>
        PayValue OrderPay(PayValue payValue);


        /// <summary>
        /// 签名验证
        /// </summary>
        /// <param name="payValue"></param>
        /// <returns></returns>
        PayValue SignVerify(PayValue payValue);

    }
}
