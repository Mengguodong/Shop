using SFO2O.Supplier.Businesses.Promotion;
using SFO2O.Supplier.Common;
using SFO2O.Supplier.Models;
using SFO2O.Supplier.Models.Promotion;
using SFO2O.Supplier.ViewModels.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFO2O.Supplier.Web.Controllers
{
    public class PromotionController : BaseController
    {
        private PromotionBLL _bll = new PromotionBLL();
        public ActionResult PromotionList()
        {
            PromotionQuery pQuery = new PromotionQuery();
            pQuery.StartTime = DateTime.Now;
            pQuery.EndTime = pQuery.StartTime.AddMonths(1);
            pQuery.PromotionStatus = -1;

            ViewBag.PromotionQuery = pQuery;

            return View();
        }

        public ActionResult GetPromotionList(PromotionQuery query)
        {
            var promotionList = new PageOf<PromotionListModel>();

            try
            {
                var page = new PageDTO()
                {
                    PageIndex = this.PageNo,
                    PageSize = this.PageSize
                };

                promotionList = _bll.GetPromotionList(this.CurrentUser.SupplierID, query, page);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            return View(promotionList);
        }

        public ActionResult CreatePromotion()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            ViewBag.RedisNo = this.CurrentUser.SupplierID + "_" + this.CurrentUser.UserID + "_" + Convert.ToInt64(ts.TotalSeconds);
            ViewBag.PromotionId = 0;


            return View();
        }

        public ActionResult GetSupplierSkus(string productName, string redisNo, int promotionId)
        {
            var page = new PageDTO()
                {
                    PageIndex = this.PageNo,
                    PageSize = this.PageSize
                };

            var model = CacheHelper.AutoCache<List<RedisPromotionSpuModel>>(
                       "SFO2O.SJ_" + redisNo, "", () =>
                       {
                           return new List<RedisPromotionSpuModel>();
                       });

            var skus = _bll.GetSpuulierSkus(this.CurrentUser.SupplierID, productName, page);


            foreach (var skuItem in skus.Items)
            {
                if (skuItem.skuProStatus == "true" && skuItem.PromotionId != promotionId)//不是当前促销的商品
                {
                    skuItem.PromotionPrice = -1;
                    skuItem.PromotionRate = -1;
                    continue;
                }

                if (promotionId <= 0 && model.Count == 0)//新建促销
                {
                    skuItem.PromotionPrice = -1;
                    skuItem.PromotionRate = -1;
                    continue;
                }

                if (model != null && model.Count > 0)
                {
                    var sku = skuItem.Sku;

                    var promotionSpu = model.FirstOrDefault(p => p.Spu == skuItem.spu);

                    if (promotionSpu != null)
                    {
                        var promotionSku = promotionSpu.Skus.FirstOrDefault(p => p.sku == sku);

                        if (promotionSku != null)
                        {
                            skuItem.PromotionPrice = promotionSku.PromotionPrice;
                            skuItem.PromotionRate = promotionSku.PromotionRate;
                        }
                    }
                }
            }


            return View(skus);
        }

        public void AddPromotionSku(string redisNo, string spu, string sku, decimal promotionPrice, decimal promotionRate)
        {
            var model = CacheHelper.AutoCache<List<RedisPromotionSpuModel>>(
                       "SFO2O.SJ_" + redisNo, "", () =>
                       {
                           return new List<RedisPromotionSpuModel>();
                       });

            var spuModel = model.FirstOrDefault(p => p.Spu == spu);

            if (spuModel != null)
            {
                spuModel.AddTime = DateTime.Now;
                var skuModel = spuModel.Skus.FirstOrDefault(p => p.sku == sku);

                if (skuModel != null)
                {
                    skuModel.PromotionPrice = promotionPrice;
                    skuModel.PromotionRate = promotionRate;
                }
                else
                {
                    spuModel.Skus.Add(new RedisPromotionSkuModel()
                    {
                        PromotionPrice = promotionPrice,
                        PromotionRate = promotionRate,
                        sku = sku
                    });
                }
            }


            if (spuModel == null || spuModel.Skus == null)
            {
                spuModel = new RedisPromotionSpuModel();
                spuModel.Skus = new List<RedisPromotionSkuModel>();

                spuModel.Spu = spu;
                spuModel.AddTime = DateTime.Now;
                spuModel.Skus.Add(new RedisPromotionSkuModel()
                {
                    PromotionPrice = promotionPrice,
                    PromotionRate = promotionRate,
                    sku = sku
                });

                model.Add(spuModel);
            }

            RedisCacheHelper.Add("SFO2O.SJ_" + redisNo, model, 30);
        }

        public void RemovePromotionSku(string redisNo, string spu, string sku)
        {
            var model = CacheHelper.AutoCache<List<RedisPromotionSpuModel>>(
                       "SFO2O.SJ_" + redisNo, "", () =>
                       {
                           return new List<RedisPromotionSpuModel>();
                       });

            var spuModel = model.FirstOrDefault(p => p.Spu == spu);

            if (spuModel != null)
            {
                spuModel.AddTime = DateTime.Now;
                var skuModel = spuModel.Skus.FirstOrDefault(p => p.sku == sku);

                if (skuModel != null)
                {
                    spuModel.Skus.Remove(skuModel);
                }
            }

            if (spuModel.Skus.Count == 0)
            {
                model.Remove(spuModel);
            }

            RedisCacheHelper.Add("SFO2O.SJ_" + redisNo, model, 30);
        }

        public int GetPromotionSkus(int promotionId, string redisNo)
        {
            var pSkus = new List<PromotionInfoModel>();

            if (promotionId > 0)
            {
                pSkus = _bll.GetPromotionSkus(promotionId, this.CurrentUser.SupplierID);
            }

            if (pSkus.Count() > 0)
            {
                var spu = "";
                List<RedisPromotionSpuModel> model = new List<RedisPromotionSpuModel>();

                foreach (var sku in pSkus)
                {
                    var spuModel = model.FirstOrDefault(p => p.Spu == sku.spu);

                    if (spuModel != null)
                    {
                        var skuModel = spuModel.Skus.FirstOrDefault(p => p.sku == sku.Sku);

                        if (skuModel != null)
                        {
                            skuModel.PromotionPrice = sku.DiscountPrice;
                            skuModel.PromotionRate = sku.DiscountRate;
                        }
                        else
                        {
                            spuModel.Skus.Add(new RedisPromotionSkuModel()
                            {
                                PromotionPrice = sku.DiscountPrice,
                                PromotionRate = sku.DiscountRate,
                                sku = sku.Sku
                            });
                        }
                    }


                    if (spuModel == null || spuModel.Skus == null)
                    {
                        spuModel = new RedisPromotionSpuModel();
                        spuModel.Skus = new List<RedisPromotionSkuModel>();

                        spuModel.Spu = sku.spu;
                        spuModel.AddTime = sku.CreateTime;
                        spuModel.Skus.Add(new RedisPromotionSkuModel()
                        {
                            PromotionPrice = sku.DiscountPrice,
                            PromotionRate = sku.DiscountRate,
                            sku = sku.Sku
                        });

                        model.Add(spuModel);
                    }
                }

                RedisCacheHelper.Add("SFO2O.SJ_" + redisNo, model, 30);
            }

            return pSkus.Count();
        }

        public ActionResult GetSelectPromotionSkus(string redisNo)
        {
            var model = CacheHelper.AutoCache<List<RedisPromotionSpuModel>>(
                       "SFO2O.SJ_" + redisNo, "", () =>
                       {
                           return new List<RedisPromotionSpuModel>();
                       });

            List<string> skuNo = new List<string>();

            foreach (var m in model)
            {
                foreach (var sku in m.Skus)
                {
                    if (!skuNo.Contains(sku.sku))
                    {
                        skuNo.Add(sku.sku);
                    }
                }
            }

            List<PromotionSkuListModel> skuInfos = new List<PromotionSkuListModel>();

            if (skuNo.Count > 0)
            {
                skuInfos = _bll.GetPromotionSkuInfo(skuNo);
            }

            if (model.Count > 0 && skuInfos.Count > 0)
            {
                foreach (var sku in skuInfos)
                {
                    var spuModel = model.FirstOrDefault(p => p.Spu == sku.spu);

                    if (spuModel == null || spuModel.Skus == null || spuModel.Skus.Count == 0)
                    {
                        continue;
                    }

                    var skuModel = spuModel.Skus.FirstOrDefault(p => p.sku == sku.Sku);

                    if (skuModel == null)
                    {
                        continue;
                    }

                    sku.PromotionPrice = skuModel.PromotionPrice;
                    sku.PromotionRate = skuModel.PromotionRate;
                }
            }

            return View(skuInfos);
        }

        /// <summary>
        /// 保存促销
        /// </summary>
        /// <param name="redisNo">redis缓存ID</param>
        /// <param name="promotionId">促销ID</param>
        /// <param name="promotionName">促销名称</param>
        /// <param name="startTime">促销开始时间</param>
        /// <param name="endTime">促销结束时间</param>
        /// <param name="promotionLable">促销标签</param>
        /// <param name="promotionCost">促销费用</param>
        /// <returns></returns>
        public ActionResult SavePromotion(string redisNo, int promotionId, string promotionName, DateTime startTime, DateTime endTime, string promotionLable, int promotionCost)
        {
            var isSuccess = true;
            var message = "";

            try
            {
                var promotionMainInfo = new PromotionMainInfoModel()
                {
                    EndTime = endTime,
                    Id = promotionId,
                    PromotionCost = promotionCost,
                    PromotionLable = promotionLable,
                    PromotionName = promotionName,
                    StartTime = startTime,
                    SupplierId = this.CurrentUser.SupplierID,
                    CreateBy = this.CurrentUser.UserName
                };

                var model = CacheHelper.AutoCache<List<RedisPromotionSpuModel>>(
                       "SFO2O.SJ_" + redisNo, "", () =>
                       {
                           return new List<RedisPromotionSpuModel>();
                       });

                if (model == null || model.Count == 0)
                {
                    message = "促銷信息已過期";
                    isSuccess = false;
                }
                else
                {
                    _bll.SavePromotion(model, promotionMainInfo);
                }
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
                message = "申請失败";
                isSuccess = false;
            }

            return Json(new { IsSuccess = isSuccess, Message = message });
        }

        public ActionResult CanclePromotion(int promotionId)
        {
            var isSuccess = true;
            var message = "";

            try
            {
                _bll.CanclePromotion(this.CurrentUser.SupplierID, promotionId);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
                isSuccess = false;
                message = "操作失败";
            }

            return Json(new { IsSuccess = isSuccess, Message = message });
        }

        public ActionResult PromotionDetail(int promotionId)
        {
            var mainInfo = new PromotionMainInfoModel();

            try
            {
                mainInfo = _bll.GetPromotionMainModel(this.CurrentUser.SupplierID, promotionId);
                ViewBag.Skus = _bll.ViewPromotionSkus(this.CurrentUser.SupplierID, promotionId);

            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }

            if (mainInfo == null)
            {
                mainInfo = new PromotionMainInfoModel();
                mainInfo.Id = 0;
                mainInfo.PromotionCost = 0;
                mainInfo.PromotionLable = "促銷價";
                mainInfo.PromotionName = "";
                mainInfo.StartTime = DateTime.Now;
                mainInfo.EndTime = mainInfo.StartTime;
            }

            return View(mainInfo);
        }

        public ActionResult EditPromotion(int promotionId)
        {
            var mainInfo = new PromotionMainInfoModel();
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var redisNo = this.CurrentUser.SupplierID + "_" + this.CurrentUser.UserID + "_" + Convert.ToInt64(ts.TotalSeconds);
            ViewBag.RedisNo = redisNo;
            ViewBag.PromotionId = promotionId;

            GetPromotionSkus(promotionId, redisNo);

            try
            {
                mainInfo = _bll.GetPromotionMainModel(this.CurrentUser.SupplierID, promotionId);
            }
            catch (Exception ext)
            {
                LogHelper.Error(ext);
            }


            if (mainInfo == null)
            {
                mainInfo = new PromotionMainInfoModel();
                mainInfo.Id = 0;
                mainInfo.PromotionCost = 0;
                mainInfo.PromotionLable = "促銷價";
                mainInfo.PromotionName = "";
                mainInfo.StartTime = DateTime.Now;
                mainInfo.EndTime = mainInfo.StartTime;
            }

            return View(mainInfo);
        }
    }
}