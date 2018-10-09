using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Web.UI.WebControls;
using SFO2O.BLL.Account;
using SFO2O.BLL.Message;
using SFO2O.BLL.Exceptions;
using SFO2O.M.Controllers.Common;
using SFO2O.M.Controllers.Extensions;
using SFO2O.M.ViewModel.Account;
using SFO2O.Utility.Uitl;
using SFO2O.Utility.Extensions;
using SFO2O.Utility.Security;
using SFO2O.M.Controllers.Filters;
using SFO2O.Model.Account;
using SFO2O.BLL.Shopping;
using SFO2O.Model.My;
using SFO2O.BLL.My;
namespace SFO2O.M.Controllers
{

    public class SplitController : SFO2OBaseController
    {
        // 拆单 页面
        public ActionResult split(string orderCode)
        {
            try
            {
                MyBll Bll = new MyBll();
                List<MyOrderInfoDto> myOrderInfoDtoList = new List<MyOrderInfoDto>();
                IList<MyOrderSkuInfoEntity> orderCodeList = Bll.GetMyOrderInfoByParentOrderCode(orderCode);
                foreach (MyOrderSkuInfoEntity i in orderCodeList)
                {
                    if (string.IsNullOrEmpty(i.OrderCode))
                    {
                        return this.HandleError("订单编号不能为空！");
                    }
                    
                    var model = Bll.GetMyOrderInfo(base.LoginUser.UserID, base.DeliveryRegion, base.language, i.OrderCode);
                    myOrderInfoDtoList.Add(model);
                    if (model.SkuInfos.Sum(s => s.RefundQuantity) > 0)
                    {
                        ViewBag.RefundStatus = true;
                    }
                }

                return View(myOrderInfoDtoList);
            }
            catch (Exception ex)
            {
                return this.HandleError(ex);
            }
        }
    }
}
