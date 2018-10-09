using System.Runtime.Serialization;

namespace SFO2O.M.Controllers.Common
{
    /// <summary>
    /// 消息提示。
    /// </summary>
    [DataContract]
    public class Message
    {
        /// <summary>
        /// 获取错误类型。
        /// </summary>
        [DataMember(Order = 1)]
        public MessageType Type { get; private set; }

        //// 为序列化而生，误删!
        //[DataMember(Name = "Status", Order = 1)]
        //private string TypeFixed { get; set; }

        /// <summary>
        /// 获取错误标题。
        /// </summary>
        [DataMember(Order = 2)]
        public string Title { get; private set; }

        /// <summary>
        /// 获取错误消息。
        /// </summary>
        [DataMember(Order = 3)]
        public string Content { get; private set; }
        
        /// <summary>
        /// 返回数据
        /// </summary>
        [DataMember(Order = 4)]
        public object Data { get; private set; }

        /// <summary>
        /// 获取下一步的链接地址。
        /// </summary>
        [DataMember(Order = 5)]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 初始化<see cref="Message"/>的实例。
        /// </summary>
        /// <param name="title">错误标题。</param>
        /// <param name="content">错误消息。</param>
        public Message(string title, string content)
            : this(MessageType.Normal, title, content)
        { }

        public Message(string title, string content, object data)
            : this(MessageType.Success, title, content)
        {
            this.Data = data;
        }
        /// <summary>
        /// 初始化<see cref="Message"/>的实例。
        /// </summary>
        /// <param name="type">错误类型。</param>
        /// <param name="title">错误标题。</param>
        /// <param name="content">错误消息。</param>
        public Message(MessageType type, string title, string content)
        {
            this.Type = type; 
            this.Title = title;
            this.Content = content;
        }
        /// <summary>
        /// 自定义消息类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="data"></param>
        public Message(MessageType type, string title, string content,object data)
        {
            this.Type = type;
            this.Title = title;
            this.Content = content;
            this.Data = data;
        }
    }
    /// <summary>
    /// 表示错误的类型。
    /// </summary>
    public enum MessageType : byte
    {
        /// <summary>
        /// 指定为错误信息。
        /// </summary>
        Error = 0,
        /// <summary>
        /// 操作成功。
        /// </summary>
        Success = 1,
        /// <summary>
        /// 指定为普通信息。
        /// </summary>
        Normal = 2,
        /// <summary>
        /// 指定为需要认证。
        /// </summary>
        RequireAuthorize = 3,
        /// <summary>
        /// 指定为需要认证。
        /// </summary>
        ErrorAddtion = 4

    }
}
