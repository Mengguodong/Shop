using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Information
{
    public class ActivityInformationViewModel
    {
        /*数据模型*/
        //   
        //   {
        //      "Type": 1,
        //      "Data": {
        //          "PageIndex": 1,
        //          "PageSize": 10,
        //          "TotalRecord": 37,
        //          "PageCount": 4,
        //          "MsgList": [
        //              {
        //                  "date": "2016-05-12 14:30",
        //                  "title": "放多少发送到发送到发送到放多少",
        //                  "msgCon": "法第三方斯蒂芬都是放多少发送到费第三方第三方第三方放多少法第三方第三方都是放多少但是放多少但是放多少但是的",
        //                  "img":"",
        //                  "url":"#"
        //              }
        //          ]
        //      }
        //  }
        //   

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
