using System.Collections.Generic;
namespace SFO2O.Framework.Uitl
{
    /// <summary>
    /// 返回对象
    /// </summary>
    public class ReturnHelper
    {
        private readonly Dictionary<string, object> _valueList = null;//键值对集合

        public ReturnHelper(bool isSuccess, string errorMsg = null)
        {
            IsSuccess = isSuccess;//是否成功
            StatusCode = -1;//状态码
            ExecutionTime = 0;//运行时间
            ExecutionTime2 = 0;//运行时间
            IpAddress = "";

            ErrorMsg = string.Empty;//错误信息
            if (IsSuccess)
                StatusCode = 1;
            else
                ErrorMsg = errorMsg;

            this._valueList = new Dictionary<string, object>();//键值对列表
        }

        public ReturnHelper(bool isSuccess, object data, string errorMsg = null)
            : this(isSuccess, errorMsg)
        {
            Data = data;
        }

        public ReturnHelper(object data)
        {
            Data = data;

            if (data == null) return;

            IsSuccess = true;
            StatusCode = 1;
        }

        public bool IsSuccess { set; get; }
        public string ErrorMsg { set; get; }
        public int StatusCode { set; get; }
        public int ExecutionTime { set; get; }
        public long ExecutionTime2 { set; get; }
        public object Data { set; get; }
        public string IpAddress { set; get; }

        public object this[string index]
        {
            set { this._valueList[index] = value; }
            get { return this._valueList[index]; }
        }

        /// <summary>
        /// 转化Json字符串
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonHelper.ToJson(this);
        }

    }
}
