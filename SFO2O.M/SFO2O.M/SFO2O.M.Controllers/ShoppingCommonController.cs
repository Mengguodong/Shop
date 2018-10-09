using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using SFO2O.BLL.Shopping;
using SFO2O.M.Controllers.BaseControllers;
using SFO2O.M.Controllers.Filters;
using SFO2O.Model.Shopping;
using SFO2O.Utility.Extensions;
using SFO2O.Utility.Uitl;
using SFO2O.Utility;
using SFO2O.Model.Enum;
using SFO2O.Model.Product;
using SFO2O.Model.Common;

namespace SFO2O.M.Controllers
{
    public partial class BuyController : ShoppingBaseController
    {
        private AddressBll addressBll = new AddressBll();

        /// <summary>
        /// 取用户的默认地址
        /// </summary>
        /// <returns></returns>
        [Login]
        public PartialViewResult GetDefaultAddress(int chooseId)
        {
            var addressList = addressBll.GetAddressList(LoginUser.UserID, DeliveryRegion,base.language);

            AddressModel defaultAddress = null;
            if (addressList != null)
            {
                defaultAddress = new AddressModel();
                if (chooseId <= 0)
                {
                    defaultAddress = addressList.ToList().Find(a => a.IsDefault == 1);
                }
                else
                {
                    defaultAddress = addressList.ToList().FirstOrDefault(a => a.Id == chooseId);
                }
            }
            return PartialView("~/Views/Buy/_defaultAddress.cshtml", defaultAddress);
        }
        /// <summary>
        /// 添加新地址
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult AddAddress(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.Title = "编辑收货地址";
                ViewBag.AddressModel = addressBll.GetAddressById(id.AsInt32());
            }
            else
            {
                ViewBag.AddressModel = null;
                ViewBag.Title = "添加收货地址";
            }
            return View();
        }
        /// <summary>
        /// 保存地址 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Login]
        public JsonResult SaveAddress(string  model)
        {
            
            if (string.IsNullOrEmpty(model))
            {
                return Json(new { Type=0,Content="参数错误"});
            }
            try
            {
                AddressEntity entity = JsonHelper.ToObject<AddressEntity>(model);
                entity.CountryId = base.DeliveryRegion == 1 ? "86" : "852";

                #region 参数验证
                if (entity == null)
                {
                    return Json(new { Type = 0, Content = "参数错误" });
                }
                if (string.IsNullOrEmpty(entity.Receiver))
                {
                    return Json(new { Type = 0, Content = "收货人姓名为空" });
                }
                else if (StringHelper.getLengthb(entity.Receiver) > 20)
                {
                    return Json(new { Type = 0, Content = "收货人姓名过长" });
                }
                if (string.IsNullOrEmpty(entity.Phone))
                {
                    return Json(new { Type = 0, Content = "手机号码为空" });
                }
                else
                {
                    if (!entity.Phone.IsMobilePhoneNum(entity.CountryId == (base.DeliveryRegion == 1 ? "86" : "852") ? true : false))
                    {
                        return Json(new { Type = 0, Content = "手机号码格式不正确" });
                    }
                }
                if (string.IsNullOrEmpty(entity.ProvinceId) ||
                    string.IsNullOrEmpty(entity.CityId) ||
                    string.IsNullOrEmpty(entity.AreaId))
                {
                    return Json(new { Type = 0, Content = "请选择完整的省市区域" });
                }
                if (string.IsNullOrEmpty(entity.Address))
                {
                    return Json(new { Type = 0, Content = "详细地址为空" });
                }
                else if (StringHelper.getLengthb(entity.Address) > 200)
                {
                    return Json(new { Type = 0, Content = "详细地址过长" });
                }
                if (string.IsNullOrEmpty(entity.PostCode))
                {
                    return Json(new { Type = 0, Content = "邮政编码为空" });
                }
                else if (!StringUtils.IsNumber(entity.PostCode, true))
                {
                    return Json(new { Type = 0, Content = "请输入正确的邮政编码" });
                }
                else if (StringHelper.getLengthb(entity.PostCode) != 6)
                {
                    return Json(new { Type = 0, Content = "请输入正确的邮政编码" });
                }
                if (entity.PapersType <= 0)
                {
                    return Json(new { Type = 0, Content = "请选择证件类型" });
                }
                else
                {
                    if (entity.PapersType == (int)CertificateType.IdCard)
                    {
                        if (StringHelper.getLengthb(entity.PapersCode) != 18)
                        {
                            return Json(new { Type = 0, Content = "证件号码格式不正确" });
                        }
                    }
                    else if (entity.PapersType == (int)CertificateType.Passport)
                    {
                        if (StringHelper.getLengthb(entity.PapersCode) > 80)
                        {
                            return Json(new { Type = 0, Content = "证件号码格式不正确" });
                        }
                    }
                    else if (StringHelper.getLengthb(entity.PapersCode) > 100)
                    {
                        return Json(new { Type = 0, Content = "证件号码格式不正确" });
                    }
                }
                #endregion

                if (entity.Id <= 0)//新增
                {
                    entity.UserId = LoginUser.UserID;
                    entity.CreateTime = DateTime.Now;
                    entity.CreateBy = LoginUser.UserName;
                    entity.IsEnable = 1;

                    entity.Type = base.DeliveryRegion;
                    int addressCount = addressBll.GetUserAddressCount(LoginUser.UserID, base.DeliveryRegion);
                    if (addressCount == 20)
                    {
                        return Json(new { Type = 0, Content = "您已经添加了20个地址，不能再添加！" });
                    }
                    int newId = addressBll.AddAddress(entity);


                    if (newId > 0)
                    {
                        if (addressCount == 0)
                        {
                            addressBll.SetDefaultAddress(LoginUser.UserID, newId, base.DeliveryRegion);
                        }
                        return Json(new { Type = 1, Content = "", LinkUrl = "/Buy/AddressList",aid=newId});
                    }
                    else
                    {
                        return Json(new { Type = 0, Content = "添加失败" });
                    }

                }
                else //编辑
                {
                    entity.UpdateBy = LoginUser.UserName;
                    entity.UserId = LoginUser.UserID;
                    entity.UpdateTime = DateTime.Now;
                    if (addressBll.Edit(entity))
                    {
                        return Json(new { Type = 1, Content = "", LinkUrl = "/Buy/AddressList" });
                    }
                    else
                    {
                        return Json(new { Type = 0, Content = "编辑失败" });
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Type = 0, Content = "系统错误" });
            }
        }
        /// <summary>
        /// 设置默认地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Login]
        public JsonResult SetDefaultAddress(int id)
        {
            try
            {
                if (addressBll.SetDefaultAddress(LoginUser.UserID, id, base.DeliveryRegion))
                {
                    return Json(new { Type = 1, Content = "设置成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Type = 0, Content = "设置失败" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Type = 0, Content = "系统错误" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Login]
        public JsonResult DeleteAddress(int id)
        {
            try
            {
                if (addressBll.Delete(id,LoginUser.UserID))
                {
                    return Json(new { Type = 1,Content = "删除成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Type = 0, Content = "删除失败" },JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Type = 0, Content = "系统错误" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 我的收货地址列表
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult AddressList()
        {
            IList<SFO2O.Model.Shopping.AddressModel> list = new List<SFO2O.Model.Shopping.AddressModel>();
            try
            {
                list = addressBll.GetAddressList(LoginUser.UserID, base.DeliveryRegion,base.language);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Redirect("/home/error");
            }
            return View(list);
        }
        /// <summary>
        /// 选择收货地址列表
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult ChooseAddress( int? chooseId)
        {
            IList<SFO2O.Model.Shopping.AddressModel> list = new List<SFO2O.Model.Shopping.AddressModel>();
            try
            {
                ViewBag.ChooseId = chooseId.AsInt32(0);
                list = addressBll.GetAddressList(LoginUser.UserID, base.DeliveryRegion,base.language);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Redirect("/home/error");
            }
            return View(list);
        }

        /// <summary>
        /// 省
        /// </summary>
        /// <returns></returns>
        public JsonResult GetProvince()
        {
            try
            {
                var list = addressBll.GetAllProvince(base.DeliveryRegion == 1 ? "86" : "852", base.language);
                if (list == null)
                {
                    return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Type = 1,Data=list.ToList() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new {Type=0 },JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 城市
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCity(string province)
        {
            try
            {
                var list = addressBll.GetCityListByProvince(province, base.language);
                
                if (list == null)
                {
                    return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Type = 1, Data = list }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 区县
        /// </summary>
        /// <returns></returns>
        public JsonResult GetArea(string city)
        {
            try
            {
                var list = addressBll.GetAreaListByCity(city,base.language);
                if (list == null)
                {
                    return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Type = 1, Data = list }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 商品税说明
        /// </summary>
        /// <returns></returns>
        public ActionResult TaxDescription()
        {
            ViewBag.OrderLimitValue = ConfigHelper.GetAppSetting<int>("OrderLimitValue");
            return View();
        }
    }
}
