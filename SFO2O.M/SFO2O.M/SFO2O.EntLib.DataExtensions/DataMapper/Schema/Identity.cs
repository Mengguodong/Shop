using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFO2O.EntLib.DataExtensions.DataMapper.Schema
{
    /// <summary>
    /// 增量标识
    /// </summary>
    public class Identity
    {
        /// <summary>
        /// 成员字段
        /// </summary>
        public string Field { get; private set; }
        
        /// <summary>
        /// 增长标识的名称（如:oracle中的自增长是用序列来控制，此名称就是其序列的名称）
        /// </summary>
        public string Name { get; private set; }

        public Identity(string field)
        {
            this.Field = field;
        }

        public Identity(string field, string name)
        {
            this.Field = field;
            this.Name = name;
        }
    }
}
