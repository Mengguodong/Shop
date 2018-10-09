using SFO2O.Supplier.DAO;
using SFO2O.Supplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Businesses
{
    public class DicsBLL
    {
        private readonly DicsDAL dal = new DicsDAL();
        //public IList<DicsModel> GetAllDicsInfo(Models.LanguageEnum languageVersion)
        //{
        //    return dal.GetAllDicsInfo(languageVersion);
        //}

        public IList<DicsModel> GetAllDicsInfo()
        {
            return dal.GetAllDicsInfo();
        }
    }
}
