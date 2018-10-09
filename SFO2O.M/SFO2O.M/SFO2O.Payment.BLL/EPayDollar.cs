using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFO2O.Payment.BLL.EPayLinks;
using SFO2O.Model.Pay;
using SFO2O.M.ViewModel.Order;
using SFO2O.Utility;
using SFO2O.Utility.Uitl;

namespace SFO2O.Payment.BLL
{
    public class EPayDollar:IPay
    {
        public void Pay()
        {
             //=======================请求参数===========================

        //易票联支付网关支付请求示例
        //商户订单号，此处用系统时间加3位随机数作为订单号，商户应根据自己情况调整，确保该订单号在商户系统中的全局唯一
        string num = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(999);
         string out_trade_no = num + ""; //这是采用随机数作为订单号样例
        //支付金额
        string total_fee = "0.01";
        //货币代码，人民币：RMB；港币：HKD；美元：USD （非人民币收单业务，需要与业务人员联系开通）
        string currency_type = "RMB";
        //创建订单的客户端IP（消费者电脑公网IP）
        //string order_create_ip = Request.UserHostAddress;  //创建订单的客户端IP（消费者电脑公网IP，用于防钓鱼支付）
        string order_create_ip = "";
        //签名类型
        string sign_type = "SHA256";
        //交易完成后跳转的URL，用来接收易票联网关的页面转跳即时通知结果
        string return_url = "http://localhost/epaylinks/returnUrl.aspx";
        //接收易票联支付网关异步通知的URL
        string notify_url = "http://localhost/epaylinks/notifyUrl.aspx";
        //直连银行参数（不停留易票联支付网关页面，直接转跳到银行支付页面）
        //string pay_id = "zhaohang";  //这是招商银行参数值例子，各银行具体参数值，请参见文档“银行直连参数表”
        string pay_id = "";
        //订单备注，该信息使用64位编码提交服务器，并将在支付完成后随支付结果原样返回
        string memo = "测试备注信息";
        byte[] bytes = System.Text.Encoding.Default.GetBytes(memo);
        string base64_memo = Convert.ToBase64String(bytes);


        //=======================请求参数结束===========================

        //设置支付请求参数
        SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
        sParaTemp.Add("out_trade_no", out_trade_no);		//商家订单号
        sParaTemp.Add("total_fee", total_fee);			//商品金额,以元为单位
        sParaTemp.Add("/// <summary>", return_url);		    //交易完成后跳转的URL
        sParaTemp.Add("notify_url", notify_url);		    //接收后台通知的URL
        sParaTemp.Add("currency_type", currency_type);	//货币种类
        sParaTemp.Add("order_create_ip", order_create_ip); //创建订单的客户端IP（消费者电脑公网IP，用于防钓鱼支付）
        sParaTemp.Add("sign_type", sign_type);			//签名算法（暂时只支持SHA256）
        sParaTemp.Add("submission_type", "01");			//00-普通支付网关业务  01-电商业务
        sParaTemp.Add("base64_user_info", "01");			//物流贸易规范支付凭证信息

        //可选参数
        sParaTemp.Add("pay_id", pay_id);	        		//直连银行参数，例子是直接转跳到招商银行时的参数
        sParaTemp.Add("base64_memo", base64_memo);		//订单备注的BASE64编码

        //建立请求
        string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "提交订单");
        //Response.Write(sHtmlText);
        }



        public PayValue OrderPay(PayValue payValue)
        {
            PayValue returnValue = new PayValue();
            PayOrderModel payOrderModel = (PayOrderModel)payValue.ReturnObject;

            //备注
            byte[] bytes = System.Text.Encoding.Default.GetBytes(payOrderModel.Note);
            string base64_memo = Convert.ToBase64String(bytes);

            StringBuilder sb = new StringBuilder();
            sb.Append("user_id=");
            sb.Append(payOrderModel.Userid);
            sb.Append("&user_name=");
            sb.Append(payOrderModel.Receiver);
            sb.Append("&user_cert_type=");
            if (payOrderModel.PassPortType == 1)
            {
                sb.Append("00");            
            }
            else if (payOrderModel.PassPortType == 2)
            {
                sb.Append("02");
            }
            else
            {
                sb.Append("99");
            }
            sb.Append("&user_cert_no=");
            sb.Append(payOrderModel.PassPortNum);
            sb.Append("&user_mobile=");
            sb.Append(payOrderModel.Phone);

            byte[] userInfobytes = System.Text.Encoding.Default.GetBytes(sb.ToString());
            String userInfoBase64 = Convert.ToBase64String(userInfobytes);

            //设置支付请求参数
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("out_trade_no", payOrderModel.OrderId);		//商家订单号
            sParaTemp.Add("total_fee", payOrderModel.Totalfee.ToString("f2"));			//商品金额,以元为单位
            sParaTemp.Add("return_url", ConfigHelper.EReturnUrl);		    //交易完成后跳转的URL
            sParaTemp.Add("notify_url", ConfigHelper.ENotifyUrl);		    //接收后台通知的URL
            sParaTemp.Add("currency_type", payOrderModel.PayCurrencys.ToString());	//货币种类
            sParaTemp.Add("order_create_ip", payOrderModel.Createip); //创建订单的客户端IP（消费者电脑公网IP，用于防钓鱼支付）
            sParaTemp.Add("sign_type", ConfigHelper.ESignType);			//签名算法（暂时只支持SHA256）
            sParaTemp.Add("store_oi_type", StringUtils.ToInt(payOrderModel.Storeoitype,1).ToString());

            sParaTemp.Add("goods_amount", payOrderModel.OrderGoodsAmount.ToString("f2"));//支付货款
            sParaTemp.Add("goods_amount_curr", payOrderModel.PayCurrencys.ToString());	//货币种类
            sParaTemp.Add("tax_amount", payOrderModel.OrderTaxAmount.ToString("f2"));//支付税款
            sParaTemp.Add("tax_amount_curr", payOrderModel.PayCurrencys.ToString());	//货币种类
            sParaTemp.Add("freight", payOrderModel.OrderFreight.ToString("f2"));//支付运费
            sParaTemp.Add("freight_curr", payOrderModel.PayCurrencys.ToString());	//货币种类
            sParaTemp.Add("submission_type", "01");
            sParaTemp.Add("base64_user_info", userInfoBase64);			//用户信息
            sParaTemp.Add("pay_id", payOrderModel.PayId);	
            //可选参数
            sParaTemp.Add("base64_memo", base64_memo);		//订单备注的BASE64编码

            //建立请求
            string sHtmlText = Submit.BuildRequestUrl(sParaTemp);

            returnValue.HasError = false;
            returnValue.ReturnString = sHtmlText;
            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payValue"></param>
        /// <returns></returns>
        public PayValue SignVerify(PayValue payValue)
        {
            PayValue returnValue = new PayValue();
            EpayNotify notify = new EpayNotify();

            returnValue.HasError = !notify.Verify((SortedDictionary<string, string>)payValue.ReturnObject, payValue.ReturnString);
            returnValue.ReturnString = "";
            return returnValue;

        }
    }
}
