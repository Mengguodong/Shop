using SFO2O.M.Controllers.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SFO2O.Model.Refund;
using SFO2O.BLL.Refund;
using SFO2O.M.ViewModel.Refund;
using SFO2O.Utility.Uitl;
using SFO2O.Model.My;
using SFO2O.BLL.My;
using SFO2O.Model.Enum;
using System.Web;
using System.IO;
using SFO2O.Model.Settle;
using SFO2O.BLL.Supplier;
using SFO2O.BLL.Settle;
using SFO2O.Model.Product;
using SFO2O.Utility.Extensions;
using SFO2O.BLL.Product;
using SFO2O.Model.Information;
using SFO2O.BLL.Order;
using SFO2O.BLL.Information;
using SFO2O.BLL.Common;
using SFO2O.Utility;

namespace SFO2O.M.Controllers
{
    public class RefundController : SFO2OBaseController
    {
        private static int pageSize = 10;

        private RefundBll refundBll = new RefundBll();

        private MyBll myOrderBll = new MyBll();

        private SettleBll settleBll = new SettleBll();

        private readonly OrderManager orderManager = new OrderManager();

        private static readonly InformationBll InformationBll = new InformationBll();


        /// <summary>
        /// 退款单列表页
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult Refund()
        {
            return View();
        }
        /// <summary>
        /// Ajax请求退款、退货列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [Login]
        public JsonResult GetRefundList(int? pageIndex)
        {
            try
            {
                int page = pageIndex ?? 1;
                int totalRecord = 0;
                var list = GetRefundList(page, out totalRecord);
                if (list != null)
                {
                    int pcount = totalRecord / pageSize;
                    if (totalRecord % pageSize > 0)
                    {
                        pcount += 1;
                    }
                    foreach (var r in list)
                    {
                        string[] img = r.ImagePath.Split(',').ToArray();
                        if (img != null && img.Length > 0)
                        {
                            for (int i = 0; i < img.Length; i++)
                            {
                                img[i] = img[i].GetImageUrl();
                            }
                        }
                        r.ImagePath = string.Join(",", img);
                    }
                    return Json(new { Type = 1, Data = new { List = list, TotalRecord = totalRecord, PageIndex = pageIndex, PageSize = pageSize, PageCount = pcount } }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 退款/退货单详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Login]
        public ActionResult RefundInfo(string code)
        {
            RefundInfoModel model = new RefundInfoModel();
            try
            {
                model = refundBll.GetRefundInfo(code, LoginUser.UserID);

                if (model != null)
                {
                    if (!string.IsNullOrEmpty(model.RefundImagePath))
                    {
                        List<string> imgList = new List<string>();
                        imgList = model.RefundImagePath.Split(',').ToList();
                        for (int i = 0; i < imgList.Count; i++)
                        {
                            imgList[i] = imgList[i].GetImageSmallUrl();
                        }
                        ViewBag.RefundImages = imgList;

                        ViewBag.RefundProduct = refundBll.GetRefundProducts(code, base.language);
                    }
                }
                else
                {
                    return Redirect("/home/error");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View(model);
        }
        private List<RefundModel> GetRefundList(int pageIndex, out int totalRecord)
        {
            var list = refundBll.GetRefundList(LoginUser.UserID, base.language, pageIndex, pageSize, out totalRecord);
            foreach (var r in list)
            {
                r.RMBUnitPrice = Convert.ToDecimal(r.RMBUnitPrice).ToNumberRoundStringWithPoint();
                r.RMBTotalAmount = Convert.ToDecimal(r.RMBTotalAmount).ToNumberRoundStringWithPoint();
                r.ImagePath = r.ImagePath.GetImageSmallUrl();
            }
            return list;
        }

        #region 申诉
        /// <summary>
        /// 申请页面
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="sku"></param>
        /// <returns></returns>
        [Login]
        public ActionResult Complain(string orderId, string sku)
        {
            if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(sku))
            {
                return Redirect("/home/error");
            }
            var orderInfo = myOrderBll.GetMyOrderInfo(LoginUser.UserID, base.DeliveryRegion, base.language, orderId);

            if (orderInfo == null)
            {
                return Redirect("/home/error");
            }
            if (orderInfo.OrderStatus != 3)
            {
                return Redirect("/home/error");
            }
            //if (!refundBll.IsCanRefund(orderInfo.OrderCode, sku))
            //{
            //    return Json(new { Type = 1, Content = "该商品已全部申诉，不能再进行申诉", LinkUrl = "/Refund/Refund" });

            //}
            //判断订单是否用了优惠券
            ViewBag.isUsedGiftCard = 0;
            if (orderInfo.Coupon > 0)
            {
                ViewBag.isUsedGiftCard = 1;
            }
            ViewBag.OrderCode = orderId;
            ViewBag.Sku = sku;
            ViewBag.IsReturn = orderInfo.SkuInfos.FirstOrDefault(x => x.Sku == sku).IsReturn;
            return View();
        }
        /// <summary>
        /// 修改申诉
        /// </summary>
        /// <param name="refundCode"></param>
        /// <returns></returns>
        [Login]
        public ActionResult EditComplain(string refundCode)
        {
            RefundOrderEntity entity = refundBll.GetRefundOrderInfo(refundCode);
            if (string.IsNullOrEmpty(entity.RefundCode))
            {
                return Redirect("/home/error");
            }
            List<RefundProductEntity> product = refundBll.GetRefundProducts(refundCode, base.language);
            if (product.Any())
            {
                ViewBag.Sku = product.First().Sku;
            }
            ViewBag.isUsedGiftCard = 0;
            if (product.First().Coupon > 0)
            {
                ViewBag.isUsedGiftCard = 1;
            }
            return View(entity);
        }
        /// <summary>
        /// 提交申诉（新增，修改）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Login]
        [HttpPost]
        public JsonResult SubmitComplain(string model, string sku)
        {
            try
            {
                #region 参数验证
                if (string.IsNullOrEmpty(model))
                {
                    return Json(new { Type = 0, Content = "缺少必要参数" }, JsonRequestBehavior.DenyGet);
                }
                RefundOrderEntity entity = JsonHelper.ToObject<RefundOrderEntity>(model);
                if (entity == null)
                {
                    return Json(new { Type = 0, Content = "缺少必要参数" }, JsonRequestBehavior.DenyGet);
                }
                var orderInfo = myOrderBll.GetMyOrderInfo(LoginUser.UserID, base.DeliveryRegion, base.language, entity.OrderCode);
                if (orderInfo == null)
                {
                    return Json(new { Type = 0, Content = "该订单不存在" });
                }
                if (orderInfo.SkuInfos == null)
                {
                    return Json(new { Type = 0, Content = "该订单商品不存在" });
                }
                if (orderInfo.OrderStatus != 3)
                {
                    return Json(new { Type = 0, Content = "该订单申诉时间已过，不能进行申诉" });
                }

                var skuInfo = orderInfo.SkuInfos.FirstOrDefault(s => s.Sku == sku);
                if (skuInfo == null)
                {
                    return Json(new { Type = 0, Content = "该商品不存在" });
                }
                if (entity.RefundType < 0)
                {
                    return Json(new { type = 0, content = "请选择退款类型" });
                }
                if (entity.RefundReason <= 0)
                {
                    return Json(new { Type = 0, Content = "请选择退款原因" });
                }
                if (string.IsNullOrEmpty(entity.RefundDescription))
                {
                    return Json(new { Type = 0, Content = "请输入详细理由" });
                }
                else if (entity.RefundDescription.Length > 1000)
                {
                    return Json(new { Type = 0, Content = "详细理由最多1000字" });
                }
                if (string.IsNullOrEmpty(entity.ImagePath))
                {
                    return Json(new { Type = 0, Content = "请至少上传2张问题图片" });
                }
                else
                {
                    var images = entity.ImagePath.Split(',').ToArray();
                    if (images != null && images.Length > 10)
                    {
                        return Json(new { Type = 0, Content = "问题图片最多上传10张" });
                    }
                }
                #endregion
                if (string.IsNullOrEmpty(entity.RefundCode) || entity.RefundCode == "0")//申诉
                {
                    if (!refundBll.IsCanRefund(orderInfo.OrderCode, skuInfo.Sku))
                    {
                        return Json(new { Type = 1, Content = "该商品已全部申诉，不能再进行申诉", LinkUrl = "/Refund/Refund" });
                    }
                    entity.UserId = LoginUser.UserID;
                    entity.CreateBy = LoginUser.UserName;
                    entity.CreateTime = DateTime.Now;
                    entity.TotalAmount = 0;
                    entity.RMBTotalAmount = 0;
                    entity.RefundStatus = (int)RefundStatus.WaitAudit;
                    entity.RefundCode = BuildRefundCode(orderInfo.OrderCode);
                    entity.OrderCode = orderInfo.OrderCode;
                    entity.ExchangeRate = orderInfo.ExchangeRate;
                    entity.SupplierId = skuInfo.SupplierId;
                    entity.RegionCode = base.DeliveryRegion;
                    entity.Commision = skuInfo.Commission;

                    RefundProductEntity productEntity = new RefundProductEntity();
                    productEntity.RefundCode = entity.RefundCode;
                    productEntity.Quantity = 1;
                    productEntity.UnitPrice = skuInfo.UnitPrice;
                    productEntity.RMBUnitPrice = skuInfo.PayUnitPrice;
                    productEntity.Sku = skuInfo.Sku;
                    productEntity.Spu = skuInfo.Spu;
                    productEntity.TaxRate = skuInfo.TaxRate;
                    productEntity.IsBearDuty = skuInfo.IsBearDuty;
                    //查询OrderProducts表里的HuoLi
                    ProductBll productBll = new ProductBll();
                    ProductInfoModel productInfo = productBll.getProductInfo(orderInfo.OrderCode, skuInfo.Sku);

                    //查询refundProducts的记录
                    List<RefundProductEntity> list = refundBll.getOrderProductCount(orderInfo.OrderCode, skuInfo.Sku);
                    if (productInfo.HuoLi > 0 || productInfo.Coupon > 0)
                    {
                        //退回的酒豆
                        if (productInfo.Qty - list.Count() > 1)
                        {
                            productEntity.HuoLi = (int)(productInfo.HuoLi / productInfo.Qty);
                            productEntity.Coupon = Math.Round(productInfo.Coupon / productInfo.Qty, 2, MidpointRounding.AwayFromZero);
                        }
                        else if (productInfo.Qty - list.Count() <= 1)
                        {
                            productEntity.HuoLi = (int)(productInfo.HuoLi - (int)(productInfo.HuoLi / productInfo.Qty) * (productInfo.Qty - 1));
                            productEntity.Coupon = Math.Round(productInfo.Coupon - Math.Round(Math.Round((productInfo.Coupon / productInfo.Qty), 2, MidpointRounding.AwayFromZero) * (productInfo.Qty - 1), 2, MidpointRounding.AwayFromZero), 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            productEntity.HuoLi = 0;
                            productEntity.Coupon = 0;
                        }
                    }
                    if (refundBll.AddRefund(entity, productEntity))
                    {

                        ProductInfoModel productInfoModel = orderManager.GetOrderImage(orderInfo.OrderCode);

                        InformationEntity InformationEntity = new InformationEntity();
                        InformationEntity.InfoType = 1;
                        InformationEntity.WebInnerType = 3;
                        InformationEntity.SendDest = CommonBll.GetUserRegion(base.LoginUser.UserID);
                        InformationEntity.SendUserId = base.LoginUser.UserID;
                        InformationEntity.TradeCode = entity.RefundCode;
                        InformationEntity.Title = InformationUtils.RefundSubmitSuccTitle;
                        InformationEntity.InfoContent = InformationUtils.RefundSubmitSuccContent;

                        if (productInfoModel != null)
                        {
                            InformationEntity.ImagePath = productInfoModel.ImagePath;
                        }
                        else
                        {
                            InformationEntity.ImagePath = null;
                        }

                        InformationEntity.Summary = null;
                        InformationEntity.LinkUrl = "refund/refundInfo?code=" + entity.RefundCode;
                        InformationEntity.StartTime = null;
                        InformationEntity.EndTime = null;
                        InformationEntity.LongTerm = 0;
                        InformationEntity.CreateTime = DateTime.Now;

                        InformationBll.AddInformation(InformationEntity);

                        return Json(new { Type = 1, Content = "申诉已提交，请耐心等待", LinkUrl = "/Refund/Refund" });
                    }
                    else
                    {
                        return Json(new { Type = 0, Content = "申诉失败" });
                    }
                }
                else
                {
                    //修改申诉
                    RefundOrderEntity oldEntity = refundBll.GetRefundOrderInfo(entity.RefundCode);
                    if (string.IsNullOrEmpty(oldEntity.RefundCode))
                    {
                        return Json(new { Type = 0, Content = "退款单号不正确" });
                    }
                    else
                    {
                        if (oldEntity.RefundStatus != (int)RefundStatus.WaitAudit)
                        {
                            return Json(new { Type = 0, Content = "该退款单已审核通过，不能提交修改" });
                        }
                    }
                    entity.RefundStatus = (int)RefundStatus.WaitAudit;
                    if (refundBll.EditRefund(entity))
                    {
                        return Json(new { Type = 1, Content = "修改申诉成功", LinkUrl = "/Refund/Refund" });
                    }
                    else
                    {
                        return Json(new { Type = 0, Content = "修改申诉失败" });
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Type = 0, Content = "系统错误，请稍后再试" });
            }
        }
        /// <summary>
        /// 取消申诉
        /// </summary>
        /// <param name="refundCode"></param>
        /// <returns></returns>
        [Login]
        public JsonResult CancelRefund(string refundCode)
        {
            try
            {
                SupplierBll supplierBll = new SupplierBll();
                var refundModel = refundBll.GetRefundInfo(refundCode, LoginUser.UserID);

                if (refundModel == null)
                {
                    return Json(new { Type = 0, Content = "该退款单不存在" });
                }
                // var product = refundProducts.First();

                //#region 结算单实体
                //SettleOrderInfoEntity settleOrder = new SettleOrderInfoEntity();
                //settleOrder.CreateBy = LoginUser.UserName;
                //settleOrder.CreateTime = DateTime.Now;
                //settleOrder.OrderCode = refundModel.OrderCode;
                //settleOrder.RefundCode = refundModel.RefundCode;
                //settleOrder.SettlementStatus = 1;
                //settleOrder.SettlementType = refundModel.RefundType==1?2:3;
                //settleOrder.SupplierId = refundModel.SupplierId;
                //settleOrder.ExchangeRate = refundModel.ExchangeRate;
                // settleOrder.RmbProductTotalAmount = product.RMBUnitPrice*product.Quantity;//商品总金额
                //settleOrder.ProductTotalAmount  = product.UnitPrice*product.Quantity;
                //settleOrder.RmbProductRefundAmount =refundModel.RMBRefundTotalAmount;//商品退款总金额
                //settleOrder.ProductRefundAmount = refundModel.RefundTotalAmount;
                ////商品结算金额=(商品总金额-商品退款总金额)*(1-服务费率)
                //decimal productSettleAmount = (settleOrder.ProductTotalAmount-settleOrder.ProductRefundAmount)*(1-refundModel.Commission);
                ////结算金额=商品结算金额-商品税金额
                //settleOrder.SettlementAmount = productSettleAmount-(refundModel.DutyAmount) ;
                //settleOrder.RmbSettlementAmount = settleOrder.SettlementAmount*refundModel.ExchangeRate;
                //settleOrder.OtherAmount = 0;
                //settleOrder.RmbOtherAmount = 0;
                //settleOrder.RmbSupplierBearDutyAmount = product.IsBearDuty==0?0:product.RMBUnitPrice*product.TaxRate;//商家承担商品税总额
                //settleOrder.SupplierBearDutyAmount = product.IsBearDuty==0?0:product.UnitPrice*product.TaxRate;
                //settleOrder.RmbBearDutyAmount = product.IsBearDuty==1?0:product.RMBUnitPrice*product.TaxRate;//买方承担商品税总额
                //settleOrder.BearDutyAmount = product.IsBearDuty==1?0:product.UnitPrice*product.TaxRate;

                //#endregion

                //#region 结算商品实体
                //SettleProductEntity settleProduct = new SettleProductEntity(){
                //        Spu = product.Spu,
                //        Sku = product.Sku,
                //        Quantity = product.Quantity,
                //        UnitPrice = product.UnitPrice,
                //        RmbUnitPrice = product.RMBUnitPrice,
                //        TaxRate = product.TaxRate,
                //        RmbAmount = product.RMBUnitPrice*product.Quantity,
                //        Amount = product.UnitPrice*product.Quantity,
                //        RmbTaxAmount = product.RMBUnitPrice*product.Quantity*product.TaxRate,
                //        TaxAmount = product.UnitPrice*product.Quantity*product.TaxRate,
                //        RmbSettlementAmount = settleOrder.RmbSettlementAmount,
                //        SettlementAmount = settleOrder.SettlementAmount,
                //        IsBearDuty = product.IsBearDuty,
                //        Commision = refundModel.Commission
                //};
                //#endregion




                if (settleBll.CancelRefund(refundCode, refundModel.OrderCode, DateTime.Now, LoginUser.UserName))
                {
                    return Json(new { Type = 1, Content = "申诉取消成功" });
                }
                else
                {
                    return Json(new { Type = 0, Content = "申诉取消失败" });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Type = 0, Content = "系统错误，请稍后再试" });
            }
        }
        /// <summary>
        /// 退款凭证
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ActionResult RefundImage(string path)
        {
            ViewBag.Path = path.GetImageUrl();
            return View();
        }

        /// <summary>
        /// 生成退款单号
        /// </summary>
        /// <returns></returns>
        private string BuildRefundCode(string orderCode)
        {
            string refundCode = "R" + orderCode.Substring(1);
            int number = refundBll.GetRefundCodeNo(orderCode) + 1;
            string orderByNumber = number.ToString().PadLeft(3, '0');
            refundCode += "-" + orderByNumber;
            return refundCode;
        }


        #endregion

        #region 上传申诉图片
        /// <summary>
        /// 上传商品图片
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadImage()
        {
            //相对路径
            string relativePath = string.Empty;
            string url = string.Empty;
            HttpPostedFileBase fileData = Request.Files[0];
            if (fileData == null || fileData.ContentLength == 0)
            {
                return Json(new { Type = 0, Content = "上传的图片无效！" });
            }
            if (fileData.ContentLength > (2 * 1024 * 1024))
            {
                return Json(new { Type = 0, Content = "上传的图片尺寸不能超过2M！" });
            }
            try
            {
                string uploadFileName = fileData.FileName;
                string imageExtension = Path.GetExtension(uploadFileName);

                IList<string> allowFileExts = new List<string> { ".jpg", ".gif", ".png", ".jpeg" }.AsReadOnly();

                if (!allowFileExts.Contains(imageExtension.ToLower()))
                {
                    return Json(new { Type = 0, Content = "不允许上传" + imageExtension + "格式的图片" });
                }
                else
                {
                    var now = DateTime.Now;
                    string relativeDir = string.Format(@"LIB\Refund\{0}\{1}\{2}\{3}\{4}\",
                         now.ToString("yyyy"), now.ToString("MM"), now.ToString("dd"), now.ToString("HH"),
                         StringHelper.GetRandomString(12));

                    //保存到临时路径下
                    //  string tempDir = PathHelper.GetSavePathTemp();

                    if (!Directory.Exists(ConfigHelper.SharePath + relativeDir))
                    {
                        Directory.CreateDirectory(ConfigHelper.SharePath + relativeDir);
                    }

                    //保存原图
                    relativePath = relativeDir + StringHelper.GetRandomString(12) + imageExtension;
                    var absolutePath = ConfigHelper.SharePath + relativePath;
                    fileData.SaveAs(absolutePath);

                    url = ConfigHelper.ImageServer + relativePath.Replace('\\', '/');
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Type = 0, Content = "系统错误" });
            }
            return Json(new { Type = 1, Data = new { Url = url, Path = relativePath } });
        }
        #endregion
    }
}
