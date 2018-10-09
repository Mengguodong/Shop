using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Models.Enums
{
    public enum AuditProductLogStatusEnum
    {
        /// <summary>
        /// 审核通过
        /// </summary>
        Passing =1,
        /// <summary>
        /// 驳回
        /// </summary>
        Rejection =2,
        /// <summary>
        /// 系统上架
        /// </summary>
        SysOffShelf = 5,
        /// <summary>
        /// 允许上架
        /// </summary>
        AllowOnShelf = 6
    }
}
