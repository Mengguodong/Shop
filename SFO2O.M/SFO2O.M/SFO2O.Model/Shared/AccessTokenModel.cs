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
    public class AccessTokenModel
    {
        public int type { set; get; }
        public string content { set; get; }
        public string access_token { set; get; }
        
    }
}
