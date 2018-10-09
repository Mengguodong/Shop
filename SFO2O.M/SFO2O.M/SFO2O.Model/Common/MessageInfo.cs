using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Utility.Uitl;

namespace SFO2O.Model.Common
{
    /// <summary>
    /// 实体类MessageInfo 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools
    /// Generate Time: 2014-01-04 11:43:47
    /// </summary>
    public class MessageInfo
    {
        public MessageInfo() { }
        /// <summary>
        /// 消息ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人(发送者)
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 发送方式
        /// </summary>
        public int SendWay { get; set; }
        /// <summary>
        /// 发送类型
        /// </summary>
        public int SendType { get; set; }
        /// <summary>
        /// 1 手机短信
        /// </summary>
        public int MessageType { get; set; }
        /// <summary>
        /// 消息分组
        /// </summary>
        public int MessageGroup { get; set; }
        /// <summary>
        /// 自动发送的时间点
        /// </summary>
        public DateTime Timing { get; set; }
        /// <summary>
        /// 手机号码  或者 商家类别名称
        /// </summary>
        public string MesasgeTypeContent { get; set; }
        /// <summary>
        /// 接收者信息
        /// </summary>
        public string Receiver { get; set; }
        /// <summary>
        /// 创建者类型:用户
        /// </summary>
        public int CreateUserType { get; set; }
        /// <summary>
        /// 消息状态：0待发送 1 发送成功 2发送失败,3作废
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 用来显示生成时间
        /// </summary>
        public string CreateTimeStr
        {
            get { return CreateTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public int Total { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 消息是否已读
        /// </summary>
        public int IsRead { get; set; }
        public string SendWayDesc
        {
            get { return EnumUtils.GetEnumDescriptionByText(typeof(SendWay), SendWay.ToString()); }
        }
        public string StatusDesc
        {
            get { return EnumUtils.GetEnumDescriptionByText(typeof(MessageStatus), Status.ToString()); }
        }
        public string MessageTypeDesc
        {
            get { return EnumUtils.GetEnumDescriptionByText(typeof(MessageType), MessageType.ToString()); }
        }
        /// <summary>
        /// 发送消息的商家ID
        /// </summary>
        public int Sender { get; set; }

        public string SenderBy { get; set; }

        public int ContentType { get; set; }

        public int PushType { get; set; }
        public long ObjectId { get; set; }
    }
}
