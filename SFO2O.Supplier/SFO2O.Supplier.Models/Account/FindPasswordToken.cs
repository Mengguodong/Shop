using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Models.Account
{
    /// <summary>
    /// 找回密码凭证
    /// </summary>
    public class FindPasswordToken
    {
        public String Token { get; set; }
        public DateTime ExpiredTime { get; set; }
    }

    public class SupplierCounter
    {
        public int ID { get; set; }
        public int ObjectID { get; set; }
        public int CountType { get; set; }
        public DateTime CountDate { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Value { get; set; }
    }

    public enum EnumCountType
    {
        LoginFail = 0,
        ForgetPassword = 1
    }
}
