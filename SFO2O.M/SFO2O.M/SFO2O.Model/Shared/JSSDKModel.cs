using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Team
{
    [Serializable]
    [DataContract]
    public class JSSDKModel
    {
        public string appId { set; get; }
        public long timestamp { set; get; }
        public string nonceStr { set; get; }
        public string jsapiTicket { set; get; }
        public string signature { set; get; }
        public string shareUrl { set; get; }
        public string shareImg { set; get; }
        public string string1 { set; get; }
        public string[] jsApiList { set; get; }
        public int type { set; get; }
        public string content { set; get; }

    }
}
