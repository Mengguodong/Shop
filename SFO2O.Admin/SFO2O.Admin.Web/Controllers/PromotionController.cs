using SFO2O.Admin.Businesses.Promotion;
using SFO2O.Admin.Common;
using SFO2O.Admin.Models.Promotion;
using SFO2O.Admin.ViewModel;
using SFO2O.Admin.ViewModel.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFO2O.Admin.Web.Controllers
{
    public class PromotionController : BaseController
    {
        private PromotionBLL promotionBLL = new PromotionBLL();
        public ActionResult PromotionList(PromotionQueryModel model)
        {
            return View(model);
        }

        public ActionResult GetList(PromotionQueryModel model, int pageSize, int pageIndex)
        {
            if (model.PromotionStatusType == -1)
            {
                model.PromotionStatus = null;
            }
            else if (model.PromotionStatusType == 1)
            {
                model.PromotionStatus = new[] { 1, 2, 3 };
            }
            else
            {
                model.PromotionStatus = new[] { model.PromotionStatusType };
            }
            PageOf<PromotionInfoModel> result;
            try
            {
                result = promotionBLL.GetPromotionList(model, pageSize, pageIndex);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
                result = new PageOf<PromotionInfoModel>();
            }
            return View(result);
        }

        public ActionResult Audit(Int32 pid)
        {
            var mainInfo = new PromotionDetail();

            try
            {
                mainInfo.PromotionInfo = promotionBLL.GetPromotionInfoModel(pid);
                if (mainInfo.PromotionInfo == null)
                {
                    return new TransferResult("/Error/404.html");
                }
                mainInfo.SkuList = promotionBLL.GetPromotionSkuList(pid);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(mainInfo);
        }

        [HttpPost]
        public ActionResult Audit(Int32 pid, Int32 status)
        {
            try
            {
                var result = promotionBLL.ChangePromotionStatus(pid, status);
                return Json(new { Error = 0 });
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
                return Json(new { Error = 1 });
            }
        }

        public ActionResult View(Int32 pid)
        {
            var mainInfo = new PromotionDetail();

            try
            {
                mainInfo.PromotionInfo = promotionBLL.GetPromotionInfoModel(pid);
                if (mainInfo.PromotionInfo == null)
                {
                    return new TransferResult("/Error/404.html");
                }
                mainInfo.SkuList = promotionBLL.GetPromotionSkuList(pid);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(mainInfo);
        }
    }
}