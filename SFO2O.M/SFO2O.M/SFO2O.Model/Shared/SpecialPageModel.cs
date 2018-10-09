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
    public class SpecialPageModel
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public string SharedImage { set; get; }

    }
}
