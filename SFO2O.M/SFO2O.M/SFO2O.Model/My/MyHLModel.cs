using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Model.My
{
    /// <summary>
    /// OrderLogisticsEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class MyHLModel
    {
        public int TotalRecord { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }

        public List<MyHL> MyHLList { get; set; }

    }
}