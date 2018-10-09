using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Models
{
  public class PageDTO
    {
      public int PageSize { get; set; }
      public int PageIndex { get; set; }
      public int TotalPage { get; set; } 
    }
}
