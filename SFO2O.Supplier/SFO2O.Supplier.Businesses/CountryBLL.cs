using SFO2O.Supplier.DAO;
using SFO2O.Supplier.Models;
using SFO2O.Supplier.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Businesses
{
    public class CountryBLL
    {
        private static readonly CountryDAL dal = new CountryDAL();


        public static IList<CountryModel> GetCountryList(Models.LanguageEnum languageEnum)
        {
            return dal.GetCountryList(languageEnum);
        }

        public static IList<ProvinceModel> GetProvinceList(int countryId, Models.LanguageEnum languageEnum)
        {
            return dal.GetProvinceList(countryId,languageEnum);
        }
    }
}
