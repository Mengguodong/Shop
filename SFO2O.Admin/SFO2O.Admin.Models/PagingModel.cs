using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Models
{
  public class PagingModel
    {
      public int PageSize { get; set; }
      public int PageIndex { get; set; }
      public int TotalPage { get; set; }
      public bool IsPaging { get; set; }
    }
}
