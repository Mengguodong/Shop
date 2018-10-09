using SFO2O.Admin.DAO;
using SFO2O.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Businesses
{
    public class DicsBLL
    {
        private readonly DicsDAL dal = new DicsDAL();
        public IList<DicsModel> GetAllDicsInfo()
        {
            return dal.GetAllDicsInfo();
        }
    }
}
