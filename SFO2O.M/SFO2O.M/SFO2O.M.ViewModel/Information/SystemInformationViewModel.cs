using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Information
{
    public class SystemInformationViewModel
    {
        //消息集合
        public List<SFO2O.Model.Information.InformationEntity> MessageList { get; set; }

        //页码
        public int PageIndex { get; set; }
        //每页条数
        public int PageSize { get; set; }
        //数据总条数
        public int TotalRecord { get; set; }        
        //总页数
        public int PageCount { get; set; }        
    }
}
