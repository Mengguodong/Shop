using SFO2O.M.Controllers.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SFO2O.BLL.Exceptions;
using SFO2O.BLL.Item;
using SFO2O.Utility.Uitl;
using SFO2O.M.Controllers.Extensions;
using SFO2O.Utility.Extensions;
using SFO2O.M.Controllers.BaseControllers;
using SFO2O.M.Controllers.Common;
using SFO2O.M.ViewModel.Information;
using SFO2O.Model.Information;
using SFO2O.BLL.Information;

namespace SFO2O.M.Controllers
{
    [Login]
    public class InformationController : SFO2OBaseController
    {
        private int PageSize = 10;
        private static readonly InformationBll InformationBll = new InformationBll();

        /// <summary>
        /// 进入消息中心
        /// </summary>
        /// <returns></returns>
        public ActionResult ToInformationCenter()
        {
            /// 声明消息中心页面Model
            InformationViewModel InfoViewModel = new InformationViewModel();

            /// 获得消息最后一条的信息
            IList<InformationEntity> InformationLastList = InformationBll.GetInformationLast(base.LoginUser.UserID);

            if (InformationLastList == null || InformationLastList.Count() == 0)
            {
                /// 没有任何消息
                InfoViewModel.InformationLast = 0;
                return View("InformationCenter", InfoViewModel);
            }
            else
            {
                /// 有消息
                InfoViewModel.InformationLast = 1;

                /// 获得系统消息和站内系统消息
                InformationEntity SystemInfo = InformationLastList.FirstOrDefault(d => d.WebInnerType == 1);
                InformationEntity WebInnerSystemInfo = InformationLastList.FirstOrDefault(d => d.WebInnerType == 4);

                /// 系统消息和站内系统消息都不为空
                if (SystemInfo != null && WebInnerSystemInfo != null)
                {
                    DateTime SystemInfoCreateTime = SystemInfo.CreateTime;
                    DateTime WebInnerSystemInfoCreateTime = WebInnerSystemInfo.CreateTime;

                    if (SystemInfoCreateTime > WebInnerSystemInfoCreateTime)
                    {
                        //string CreateTime = CreatwTimeFormatter(SystemInfoCreateTime);
                        InfoViewModel.SystemInfoLastCreateTime = SystemInfoCreateTime.ToString("MM-dd");
                        InfoViewModel.SystemInfoLastTitle = SystemInfo.Title;
                    }
                    else
                    {
                        //string CreateTime = CreatwTimeFormatter(WebInnerSystemInfoCreateTime);
                        InfoViewModel.SystemInfoLastCreateTime = WebInnerSystemInfoCreateTime.ToString("MM-dd"); 
                        InfoViewModel.SystemInfoLastTitle = WebInnerSystemInfo.Title;
                    }

                    InfoViewModel.SystemInformationLast = 1;
                }
                /// 只有系统消息数据
                else if (SystemInfo != null)
                {
                    InfoViewModel.SystemInformationLast = 1;
                    //string CreateTime = CreatwTimeFormatter(SystemInfo.CreateTime);
                    InfoViewModel.SystemInfoLastCreateTime = SystemInfo.CreateTime.ToString("MM-dd");
                    InfoViewModel.SystemInfoLastTitle = SystemInfo.Title;
                }
                /// 只有站内系统消息数据
                else if (WebInnerSystemInfo != null)
                {
                    InfoViewModel.SystemInformationLast = 1;
                    //string CreateTime = CreatwTimeFormatter(WebInnerSystemInfo.CreateTime);
                    InfoViewModel.SystemInfoLastCreateTime = WebInnerSystemInfo.CreateTime.ToString("MM-dd");
                    InfoViewModel.SystemInfoLastTitle = WebInnerSystemInfo.Title;
                }
                /// 系统消息和站内系统消息都没有数据
                else
                {
                    InfoViewModel.SystemInformationLast = 0;
                }

                /// 获得活动消息
                InformationEntity ActivityInfo = InformationLastList.FirstOrDefault(d => d.WebInnerType == 2);
                if (ActivityInfo != null)
                {
                    InfoViewModel.ActivityInformationLast = 1;
                    InfoViewModel.ActivityInfoLastTitle = ActivityInfo.Title;

                    //string CreateTime = CreatwTimeFormatter(ActivityInfo.CreateTime);
                    InfoViewModel.ActivityInfoLastCreateTime = ActivityInfo.CreateTime.ToString("MM-dd");
                }
                else
                {
                    InfoViewModel.ActivityInformationLast = 0;
                }

                /// 获得订单消息
                InformationEntity OrderInfo = InformationLastList.FirstOrDefault(d => d.WebInnerType == 3);
                if (OrderInfo != null)
                {
                    InfoViewModel.OrderInformationLast = 1;
                    InfoViewModel.OrderInfoLastTitle = OrderInfo.Title;

                    //string CreateTime = CreatwTimeFormatter(OrderInfo.CreateTime);
                    InfoViewModel.OrderInfoLastCreateTime = OrderInfo.CreateTime.ToString("MM-dd");
                }
                else
                {
                    InfoViewModel.OrderInformationLast = 0;
                }
            }

            /// 获得未读消息
            IList<InformationEntity> NotReadInformations =  InformationBll.GetNotReadInfomation(base.LoginUser.UserID);

            if (NotReadInformations == null || NotReadInformations.Count() == 0)
            {
                /// 消息全部已读
                InfoViewModel.InformationRead = 1;
            }
            else
            {
                /// 消息有未读
                InfoViewModel.InformationRead = 0;
            }

            /// 系统消息未读条数总计
            InfoViewModel.SystemInfoTotalNotReadCount = NotReadInformations.Where(d=>d.WebInnerType==1).Sum(d=>d.NotReadInfoCount)
                                          + NotReadInformations.Where(d => d.WebInnerType == 4).Sum(d => d.NotReadInfoCount);
            /*InfoViewModel.SystemInfoTotalNotReadCount = 100;*/
            InfoViewModel.ActivityInfoNotReadCount = NotReadInformations.Where(d => d.WebInnerType == 2).Sum(d => d.NotReadInfoCount);
            /*InfoViewModel.ActivityInfoNotReadCount = 100;*/
            InfoViewModel.OrderInfoNotReadCount = NotReadInformations.Where(d=>d.WebInnerType == 3).Sum(d=>d.NotReadInfoCount);
            /*InfoViewModel.OrderInfoNotReadCount = 100;*/

            return View("InformationCenter",InfoViewModel);
        }

        /// <summary>
        /// 系统消息列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemMsgList()
        {
            return View();
        }

        /// <summary>
        /// 获取系统消息列表数据
        /// <param name="PageIndex">页码</param>
        /// </summary>
        public JsonResult GetSystemMsgList(int PageIndex = 1)
        {
            try
            {
                var MsgList = GetSystemMsg(base.LoginUser.UserID, PageIndex);
                var msglist = from msg in MsgList.MessageList
                                      select new { id=msg.ID,isread = msg.ReadUserId==base.LoginUser.UserID?1:0, date = msg.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"), title = msg.Title, msgCon = msg.InfoContent };
                return Json(new { Type = 1, Data = new { PageIndex = MsgList.PageIndex, PageSize = PageSize, PageCount = MsgList.PageCount, TotalRecords = MsgList.TotalRecord, MsgList = msglist } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0,Content="暂无系统消息！" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取系统消息数据列表
        /// </summary>
        /// <param name="userId">当前登录用户</param>
        /// <param name="pageindex">页码</param>
        /// <returns>系统消息ViewModel</returns>
        public SystemInformationViewModel GetSystemMsg(int userId, int pageindex)
        {
            SystemInformationViewModel viewmodel = new SystemInformationViewModel();
            try
            {
                int totalRecord = 0, pageCount = 0;
                var list = InformationBll.GetSysInfoList(userId, pageindex, PageSize);
                totalRecord = list.FirstOrDefault().TotalRecord;
                pageCount = totalRecord / PageSize;
                if (totalRecord % PageSize > 0)
                {
                    pageCount += 1;
                }                
                viewmodel.MessageList = list;
                viewmodel.PageSize = PageSize;
                viewmodel.PageCount = pageCount;
                viewmodel.TotalRecord = totalRecord;
                viewmodel.PageIndex = pageindex;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
            return viewmodel;
        }

        public ActionResult ActivityMsgList()
        {
            return View();
        }

        public JsonResult GetActivityMsgList(int PageIndex = 1)
        {
            try
            {
                var MsgList = GetActivityMsg(base.LoginUser.UserID, PageIndex);
                var msglist = from msg in MsgList.MessageList
                              select new { 
                                  ReadUserId = msg.ReadUserId, 
                                  date = Convert.ToDateTime(msg.StartTime).ToString("MM-dd HH:mm"), 
                                  title = msg.Title, 
                                  msgCon = msg.InfoContent,
                                  img = PathHelper.GetImageUrl(msg.ImagePath), 
                                  url = msg.LinkUrl
                              };
                /// 回写InformationRead表，将未读的消息存入表中。
                IList<InformationEntity> NotReadInfoList = MsgList.MessageList.Where(d => d.ReadUserId == 0).ToList<InformationEntity>();

                if (NotReadInfoList != null && NotReadInfoList.Count() != 0)
                {
                    foreach(InformationEntity item in NotReadInfoList){
                        InformationBll.AddInformationRead(base.LoginUser.UserID,item.ID);
                    }
                }

                return Json(new { Type = 1, Data = new { PageIndex = MsgList.PageIndex, PageSize = PageSize, PageCount = MsgList.PageCount, TotalRecords = MsgList.TotalRecord, MsgList = msglist } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0, Content = "暂无活动消息！" }, JsonRequestBehavior.AllowGet);
        }

        public ActivityInformationViewModel GetActivityMsg(int userId, int pageindex)
        {
            ActivityInformationViewModel viewmodel = new ActivityInformationViewModel();
            try
            {
                int totalRecord = 0, pageCount = 0;
                var list = InformationBll.GetActivityInfoList(userId, pageindex, PageSize);
                totalRecord = list.FirstOrDefault().TotalRecord;
                pageCount = totalRecord / PageSize;
                if (totalRecord % PageSize > 0)
                {
                    pageCount += 1;
                }
                viewmodel.MessageList = list;
                viewmodel.PageSize = PageSize;
                viewmodel.PageCount = pageCount;
                viewmodel.TotalRecord = totalRecord;
                viewmodel.PageIndex = pageindex;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
            return viewmodel;
        }

        /// <summary>
        /// 系统消息详情页
        /// </summary>
        /// <param name="infoid">消息ID</param>
        /// <returns>消息Model</returns>
        public ActionResult SystemMsgDetail(int infoid)
        {
            InformationEntity model=InformationBll.GetSysInfoById(infoid);
            ViewBag.infoid = infoid;
            return View(model);
        }

        /// <summary>
        /// 订单消息列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderMsgList()
        {
            return View();
        }

        /// <summary>
        /// 获取订单消息列表数据
        /// <param name="PageIndex">页码</param>
        /// </summary>
        public JsonResult GetOrderMsgList(int PageIndex = 1)
        {
            try
            {
                var MsgList = GetOrderMsg(base.LoginUser.UserID, PageIndex);
                var msglist = from msg in MsgList.MessageList
                              select new { id = msg.ID,
                                  unread = msg.ReadUserId == base.LoginUser.UserID ? 1 : 0, 
                                  date = msg.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"), 
                                  title = msg.Title, 
                                  msgCon = msg.InfoContent,
                                  img = PathHelper.GetImageSmallUrl(msg.ImagePath), 
                                  url = msg.LinkUrl, 
                                  infoId = msg.ID};
                return Json(new { Type = 1, Data = new { PageIndex = MsgList.PageIndex, PageSize = PageSize, PageCount = MsgList.PageCount, TotalRecords = MsgList.TotalRecord, MsgList = msglist } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Json(new { Type = 0, Content = "暂无订单消息！" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取订单消息数据列表
        /// </summary>
        /// <param name="userId">当前登录用户</param>
        /// <param name="pageindex">页码</param>
        /// <returns>系统消息ViewModel</returns>
        public OrderInformationViewModel GetOrderMsg(int userId, int pageindex)
        {
            OrderInformationViewModel viewmodel = new OrderInformationViewModel();
            try
            {
                int totalRecord = 0, pageCount = 0;
                var list = InformationBll.GetOrderInfoList(userId, pageindex, PageSize);
                totalRecord = list.FirstOrDefault().TotalRecord;
                pageCount = totalRecord / PageSize;
                if (totalRecord % PageSize > 0)
                {
                    pageCount += 1;
                }
                viewmodel.MessageList = list;
                viewmodel.PageSize = PageSize;
                viewmodel.PageCount = pageCount;
                viewmodel.TotalRecord = totalRecord;
                viewmodel.PageIndex = pageindex;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
            return viewmodel;
        }

        public JsonResult OrderReadMessage(int infoId)
        {
            try
            {
                int result = InformationBll.OrderReadMessage(base.LoginUser.UserID, infoId);

                if(result != -1){
                    return Json(new { Type = 1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Type = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        private string CreatwTimeFormatter(DateTime CreateTime)
        {
            StringBuilder CreateTimeStrBuilder = new StringBuilder();
            if (CreateTime == null)
            {
                string[] parseCreateTime = DateTime.Now.ToShortDateString().ToString().Split('-');
                CreateTimeStrBuilder.Append(parseCreateTime[1]).Append("-").Append(parseCreateTime[2]);
            }
            else
            {
                string[] parseCreateTime = CreateTime.ToShortDateString().ToString().Split('/');
                CreateTimeStrBuilder.Append(parseCreateTime[1]).Append("-").Append(parseCreateTime[2]);
            }
            return CreateTimeStrBuilder.ToString();
        }
    }
}
