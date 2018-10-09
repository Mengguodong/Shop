using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Information
{
    public class InformationViewModel
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public int WebInnerType { get; set; }
        /// <summary>
        /// 未读条数
        /// </summary>
        public int NotReadInfoCount { get; set; }
        /// <summary>
        /// 所有消息的最后一条数量为0
        /// </summary>
        public int InformationLast { get; set; }
        /// <summary>
        /// 系统消息最后一条数量
        /// </summary>
        public int SystemInformationLast { get; set; }
        /// <summary>
        /// 活动消息最后一条数量
        /// </summary>
        public int ActivityInformationLast { get; set; }
        /// <summary>
        /// 订单消息最后一条数量
        /// </summary>
        public int OrderInformationLast { get; set; }
        /// <summary>
        /// 消息全部已读
        /// </summary>
        public int InformationRead { get; set; }
        /// <summary>
        /// 系统公告未读条数
        /// </summary>
        public int SystemInfoNotReadCount { get; set; }
        /// <summary>
        /// 活动消息未读条数
        /// </summary>
        public int ActivityInfoNotReadCount { get; set; }
        /// <summary>
        /// 订单消息未读条数
        /// </summary>
        public int OrderInfoNotReadCount { get; set; }
        /// <summary>
        /// 站内系统消息未读条数
        /// </summary>
        public int WebInnerSystemInfoNotReadCount { get; set; }
        /// <summary>
        /// 系统消息未读条数总计
        /// </summary>
        public int SystemInfoTotalNotReadCount { get; set; }
        /// <summary>
        /// 系统消息最后一条标题
        /// </summary>
        public string SystemInfoLastTitle { get; set; }
        /// <summary>
        /// 系统消息最后一条发送时间
        /// </summary>
        public string SystemInfoLastCreateTime { get; set; }
        /// <summary>
        /// 活动消息最后一条标题
        /// </summary>
        public string ActivityInfoLastTitle { get; set; }
        /// <summary>
        /// 活动消息最后一条发送时间
        /// </summary>
        public string ActivityInfoLastCreateTime { get; set; }
        /// <summary>
        /// 订单消息最后一条标题
        /// </summary>
        public string OrderInfoLastTitle { get; set; }
        /// <summary>
        /// 订单消息最后一条发送时间
        /// </summary>
        public string OrderInfoLastCreateTime { get; set; }
        /// <summary>
        /// 站内系统消息最后一条数量
        /// </summary>
        public int WebInnerSystemInfomationLast { get; set; }
        
        
    }
}
