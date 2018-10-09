using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using SFO2O.Utility.Uitl;

namespace SFO2O.Utility
{
    public static class InformationUtils
    {
        // 用户注册成功消息标题
        public static readonly string UserRegisterSuccTitle = "欢迎加入爱玖网！";
        // 用户找回密码成功消息标题
        public static readonly string UserFindPasswordSuccTitle = "找回密码成功";
        // 用户支付成功，订单变成待发货消息标题
        public static readonly string UserPaySuccTitle = "您已付款成功";
        // 订单变成交易成功消息标题
        public static readonly string OrderTradeSuccTitle = "订单交易成功";
        // 申诉提交成功消息标题
        public static readonly string RefundSubmitSuccTitle = "退货申请提交成功";
        // 拼团成功消息标题
        public static readonly string TeamJoinSuccTitle = "拼团成功通知";
        // 酒豆到账消息标题
        public static readonly string HuoliTransferedSuccTitle = "酒豆到账了，赶快去买买买";


        // 用户注册成功消息内容
        public static readonly string UserRegisterSuccContent = "您已成功注册成为爱玖网一员，您将享受爱玖网的优质服务，欢迎加入！";
        // 用户找回密码成功消息内容
        public static readonly string UserFindPasswordSuccContent = "您使用了找回密码功能，已重新设置新密码，千万不要告诉别人哦~";
        // 用户支付成功，订单变成待发货消息内容前缀
        public static readonly string UserPaySuccContent_Prefix = "您的";
        // 用户支付成功，订单变成待发货消息内容后缀
        public static readonly string UserPaySuccContent_suffix = "订单付款成功，爱玖网已收到通知，紧急备货中";
        // 订单变成交易成功消息内容前缀
        public static readonly string OrderTradeSuccContent_Prefix = "您的";
        // 订单变成交易成功消息内容后缀
        public static readonly string OrderTradeSuccContent_suffix = "交易成功，希望您能喜欢，欢迎再次光临。";
        // 申诉提交成功消息内容
        public static readonly string RefundSubmitSuccContent = "您的退货申请提交成功，爱玖网会于三个工作日内处理您的申请，请耐心等候";
        // 拼团成功消息内容
        public static readonly string TeamJoinSuccContent = "您参与的团，现已拼团成功，爱玖网平台正在紧急备货，您坐等收货吧。";
        // 酒豆到账消息内容前缀
        public static readonly string HuoliTransferedSuccContent_Prefix = "恭喜，您收到";
        // 酒豆到账消息内容后缀
        public static readonly string HuoliTransferedSuccContent_suffix = "酒豆，可以用来购买爱玖网的部分商品，快去用吧。";
    }
}
