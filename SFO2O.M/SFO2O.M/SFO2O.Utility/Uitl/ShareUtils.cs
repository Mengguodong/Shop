using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using SFO2O.Utility.Uitl;

namespace SFO2O.Utility
{
    public static class ShareUtils
    {
        // 微信Token请求Url
        public static readonly string WechatTokenRequestUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        // 微信Ticket请求Url
        public static readonly string WechatTicketRequestUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";


        // Json结果内容-type值为空
        public static readonly string JsonContent_TypeIsNull = "type值为空！";
        // Json结果内容-URL为空
        public static readonly string JsonContent_UrlIsNull = "URL为空！";
        // Json结果内容-Json为空
        public static readonly string JsonContent_JsonIsNull = "Json为空！";
        // Json结果内容-TeamCode为空
        public static readonly string JsonContent_TeamCodeIsNull = "TeamCode为空！";
        // Json结果内容-团集合为空
        public static readonly string JsonContent_TeamListIsNull = "团集合为空！";
        // Json结果内容-拼生活详情页链接参数为空或参数不完整
        public static readonly string JsonContent_PinLife_ParamIsNull = "拼生活详情页链接参数为空或参数不完整！";
        // Json结果内容-拼生活详情页数据集合为空
        public static readonly string JsonContent_PinLifeDetailListIsNull = "拼生活详情页数据集合为空！";


        // 团详情分享标识
        public static readonly string TeamSharedFlag = "TeamShared";
        // 母亲节专题分享标识
        public static readonly string MothersdaySharedFlag = "MothersdayShared";
        // 拼生活专题分享标识
        public static readonly string PinLifeIntroSharedFlag = "PinLifeIntroShared";
        // 拼生活商品详情页分享标识
        public static readonly string PinLifeDetailSharedFlag = "PinLifeDetailSharedFlag";
        // 儿童节专题分享标识
        public static readonly string ChildrenDaySharedFlag = "childrenDayShared";


        // 团详情分享标题前缀-已参加用户
        public static readonly string TeamJoinSharedTitle_Joined_prefix = "我参加了健康绿氧";
        // 团详情分享标题后缀-已参加用户
        public static readonly string TeamJoinSharedTitle_Joined_suffix = "拼单!";
        // 团详情分享描述前缀-已参加用户
        public static readonly string TeamJoinSharedDescription_Joined_prefix = "【还差";
        // 团详情分享描述后缀-已参加用户
        public static readonly string TeamJoinSharedDescription_Joined_suffix = "人】健康绿氧，贵州茅台怀桥酒厂发货，全场包邮，一起实惠一起拼！";
        // 团详情分享描述-已参加用户-人数已满
        public static readonly string TeamJoinSharedDescription_Joined_Full = "团人数已经满了";


        // 团详情分享描述-未参加用户
        public static readonly string TeamJoinSharedDescription_UnJoined = "健康绿氧，贵州茅台怀桥酒厂发货，全场包邮，一起实惠一起拼！";


        // 团详情分享描述-未登录用户
        public static readonly string TeamJoinSharedDescription_UnLogined = "健康绿氧，贵州茅台怀桥酒厂发货，全场包邮，一起实惠一起拼！";

        // 拼生活详情页分享描述
        public static readonly string PinLifeDetailSharedDescription = "健康绿氧，贵州茅台怀桥酒厂发货，全场包邮，一起实惠一起拼！";


        // 母亲节专题标题
        public static readonly string MotherDaySpecialTitle = "母亲节特别活动 – 健康绿氧购物商城";
        // 母亲节专题描述
        public static readonly string MotherDaySpecialDescription = "母亲节之际，健康绿氧平台为天下的妈妈们准备了一份精美礼物，快来选择一份送给自己的妈妈吧。";
        // 母亲节专题图片
        public static readonly string MotherDaySpecialImage = "LIB/Content/Images/active/0508/shareImg.jpg?v=20160414";


        // 拼生活介绍页专题标题
        public static readonly string TeamLifeSpecialTitle = "拼生活，好闺蜜、好基友一起拼！– 健康绿氧购物商城";
        // 拼生活介绍页专题描述
        public static readonly string TeamLifeSpecialDescription = "拼生活，是基于好友关系的组团买卖，获取团购优惠。「拼生活」频道最新上线！港货正品，全场包邮，由物流从中华人民共和国大陆地区直送全国各地。";
        // 拼生活介绍页专题图片
        public static readonly string TeamLifeSpecialImage = "LIB/Content/Images/active/pinIntroduce/shareImg.jpg";


        // 儿童节专题标题
        public static readonly string ChildrenDaySpecialTitle = "欢度六一 礼伴成长 – 健康绿氧购物商城";
        // 儿童节专题描述
        public static readonly string ChildrenDaySpecialDescription = "岁月无情，愿童心不变，为小朋友、大朋友选“对”礼物最重要";
        // 儿童节专题图片
        public static readonly string ChildrenDaySpecialImage = "LIB/Content/Images/active/childrenDay/shareImg.jpg";

        public enum JsonType
        {
            // Json成功
            typeSucc = 1,

            // Json失败
            typeFailed = 2
        }
    }
}
