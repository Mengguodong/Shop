using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SFO2O.BLL.My;
using SFO2O.M.Controllers.Filters;
using SFO2O.Model.Enum;
using SFO2O.Utility.Extensions;
using SFO2O.BLL.Account;
using SFO2O.References.SFo2oWCF;
using SFO2O.Model.My;
using SFO2O.Utility.Uitl;
using SFO2O.BLL.Information;
using SFO2O.BLL.Common;
using SFO2O.Utility;
using SFO2O.BLL.GiftCard;
using SFO2O.M.ViewModel.GiftCard;
using SFO2O.Model.GiftCard;
namespace SFO2O.M.Controllers
{
    //优惠券基类
    [NoCache]
    [Login]
    public class GiftCardController : SFO2OBaseController
    {
        public GiftCardBll giftCardBll = new GiftCardBll();
        private int PageSize = 10;
        //优惠券 页面

        public ActionResult Index()
        {
            return View();
        }
        //获取优惠券的列表
        public JsonResult GiftCardList(string type, int pageIndex)
        {

            try
            {
                var model = GetGiftCardList(pageIndex, Convert.ToInt32(type));
                //遍历列表  增加model里面的属性 
                //foreach (giftcardentity giftcardentity in model.giftcardlist)
                //{
                //    switch (giftcardentity.satisfyproduct)
                //    {
                //        case 0:

                //            break;
                //        case 1:

                //            break;
                //        case 2:

                //            break;
                //    }
                //}
                //优惠券的分页
                return Json(new { Type = 1, Data = new { PageIndex = model.PageIndex, PageSize = PageSize, PageCount = model.PageCount, TotalRecords = model.TotalRecord, Products = model.GiftCardList } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 优惠券列表页
        /// </summary>
        /// <param name="c"></param>
        /// <param name="level"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="attrids"></param>
        /// <returns></returns>
        private GiftCardViewModel GetGiftCardList(int PageIndex, int status)
        {
            GiftCardViewModel viewmodel = new GiftCardViewModel();
            try
            {
                int totalRecords = 0, pagecount = 0;
                var products = giftCardBll.GetGiftCardList(status, base.LoginUser.UserID, PageIndex, PageSize);
                if (products != null && products.Count() > 0)
                {
                    totalRecords = products.FirstOrDefault().TotalRecord;
                }
                else
                {
                    totalRecords = 0;
                    products = new List<GiftCardEntity>();
                }
                viewmodel.GiftCardList = products;
                viewmodel.PageSize = PageSize;
                viewmodel.TotalRecord = totalRecords;
                pagecount = totalRecords / PageSize;
                if (totalRecords % PageSize > 0)
                {
                    pagecount += 1;
                }
                viewmodel.PageCount = pagecount;
                viewmodel.PageIndex = PageIndex;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return viewmodel;
        }
    }
}
