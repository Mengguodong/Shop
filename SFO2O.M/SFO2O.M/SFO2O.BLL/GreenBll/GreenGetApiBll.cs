using Common;
using SFO2O.Model;
using SFO2O.Utility.Uitl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.BLL.GreenBll
{
   public static class GreenGetApiBll
    {
       public static ReturnModel GetUserScore(string userName) 
       {
           ReturnModel model = null;
            try
            {
                string url = string.Format(PubConstant.GreenWebApi + "api/GreenApi/GetUserScore?userName={0}", userName);
                model = HttpClientHelper.GetResponse<ReturnModel>(url);
            }
            catch (Exception ex)
            {
                LogHelper.Info(ex.ToString());
            }
     
           return model;
       }


       public static ReturnModel UpdateUserScore(string userName, decimal score, int plusType = 2)
       {
           ReturnModel model = null;
           try
           {
               string url = string.Format(PubConstant.GreenWebApi + "api/GreenApi/UpdateUserScore?userName={0}&score={1}&plusType={2}", userName, score, plusType);
               model = HttpClientHelper.GetResponse<ReturnModel>(url);
           }
           catch (Exception ex)
           {
               LogHelper.Info(ex.ToString());
           }

           return model;
       }



       public static ReturnModel OrderOverApi(string userName, int count)
       {
           ReturnModel model = null;
           try
           {
               string url = string.Format(PubConstant.GreenWebApi + "api/GreenApi/OrderOverApi?userName={0}&count={1}", userName, count);
               model = HttpClientHelper.GetResponse<ReturnModel>(url);
           }
           catch (Exception ex)
           {
               LogHelper.Info(ex.ToString());
           }

           return model;
       }





    }


    
  
}
