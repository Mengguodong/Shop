using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.BLL.Exceptions
{
    /// <summary>
    /// 表示SFO2O中的业务异常类型。建议建立自己的异常类，并且从此类继承。继承自此类的异常将被统一处理，并且提示给网站访问者。
    /// </summary>
    public class SFO2OException : ApplicationException
    {
         /// <summary>
        /// 初始化<see cref="SFO2OException"/>的实例。
        /// </summary>
        /// <param name="message">错误消息。</param>
        public SFO2OException(string message)
            : this(message, null)
        { }

        /// <summary>
        /// 初始化<see cref="SFO2OException"/>的实例。
        /// </summary>
        /// <param name="message">错误消息。</param>
        /// <param name="innerException">内部异常。</param>
        public SFO2OException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
