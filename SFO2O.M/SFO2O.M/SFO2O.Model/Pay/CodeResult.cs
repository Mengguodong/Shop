using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Pay
{
    /// <summary>
    /// 请求二维码返回实体
    /// </summary>
    public class CodeResult
    {
        /// <summary>
        /// 状态码  0000  二维码地址   0001  传入参数错误  0002 商户无权限 
        ///         0003参数配置错误 0004签名验证错误  0005此订单已经支付成功,无法再次支付
        ///        0006二维码获取失败  0007系统错误   0008支付金额范围超限
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 响应信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string tradeid { get; set; }

    }
}
