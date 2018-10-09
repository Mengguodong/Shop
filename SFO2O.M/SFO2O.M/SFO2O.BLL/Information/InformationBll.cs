using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SFO2O.Model.Common;
using SFO2O.DAL.Information;
using SFO2O.Utility.Uitl;
using SFO2O.BLL.com.hksmspro.api3;
using SFO2O.Model.Information;
using System.Xml;
using System.Diagnostics;
using System.Data;

namespace SFO2O.BLL.Information
{
    public class InformationBll
    {
        private static readonly InformationDal informationDal = new InformationDal();


        /// <summary>
        /// 获取某人的系统消息列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">每页条数</param>
        /// <returns>消息List</returns>
        public List<InformationEntity> GetSysInfoList(int userId, int pageindex, int pagesize)
        {
            List<InformationEntity> list = new List<InformationEntity>();
            list = informationDal.GetSysInfoList(userId, pageindex, pagesize);
            return list;
        }        

        /// <summary>
        ///根据系统消息ID获取消息 
        /// </summary>
        /// <param name="infoid">消息id</param>
        /// <returns>消息对象</returns>
        public InformationEntity GetSysInfoById(int infoid)
        {
            return informationDal.GetSysInfoById(infoid);
        }

        /// <summary>
        /// 执行阅读某条消息操作
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="infoid">消息id</param>
        public void ReadMessage(int userid, int infoid)
        {
            informationDal.ReadMessage(userid,infoid);
        }

        /// <summary>
        /// 获得消息最后一条的信息
        /// </summary>
        /// <returns></returns>
        public IList<InformationEntity> GetInformationLast(int UserId)
        {
            IList<InformationEntity> entity = informationDal.GetInformationLast(UserId);
            return entity;
        }

        /// <summary>
        /// 获得未读消息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IList<InformationEntity> GetNotReadInfomation(int UserId)
        {
            IList<InformationEntity> entity = informationDal.GetNotReadInfomation(UserId);
            return entity;
        }

        public List<InformationEntity> GetActivityInfoList(int userId, int pageindex, int pagesize)
        {
            List<InformationEntity> list = new List<InformationEntity>();
            list = informationDal.GetActivityInfoList(userId, pageindex, pagesize);
            return list;
        }

        public void AddInformationRead(int UserId,int InformationId)
        {
            informationDal.AddInformationRead(UserId, InformationId);
        }

        /// <summary>
        /// 获取某人的订单消息列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">每页条数</param>
        /// <returns>消息List</returns>
        public List<InformationEntity> GetOrderInfoList(int userId, int pageindex, int pagesize)
        {
            List<InformationEntity> list = new List<InformationEntity>();
            list = informationDal.GetOrderInfoList(userId, pageindex, pagesize);
            return list;
        }

        public int OrderReadMessage(int userid, int infoid)
        {
            int result = informationDal.OrderReadMessage(userid, infoid);
            return result;
        }

        public void AddInformation(InformationEntity InformationEntity)
        {
            informationDal.AddInformation(InformationEntity);
        }

        /// <summary>
        /// 注册成功后获得活动消息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<InformationEntity> GetActivityListForRegister(int userId)
        {
            List<InformationEntity> list = new List<InformationEntity>();
            list = informationDal.GetActivityListForRegister(userId);
            return list;
        }

        /// <summary>
        /// 获得注册用户可见的活动消息集合
        /// </summary>
        /// <param name="ActivityList"></param>
        /// <param name="RegisterCurrentTime"></param>
        /// <returns></returns>
        public List<InformationEntity> GetActivityListForRegisterSave(List<InformationEntity> ActivityList,DateTime RegisterCurrentTime)
        {
            List<InformationEntity> ResultList = new List<InformationEntity>();
            ResultList = ActivityList.Where(d => d.StartTime > RegisterCurrentTime).ToList();
            return ResultList;
        }

        public DataTable GetTableSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("Id",typeof(int)),
                new DataColumn("UserId",typeof(int)),
                new DataColumn("InformationId",typeof(int)),
                new DataColumn("CreateTime",typeof(DateTime)),
            });
            return dt;
        }

        /// <summary>
        /// 批量插入活动消息到InformationToCustomer表
        /// </summary>
        /// <param name="ResultList"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public int MutiInsertToInfoCustomer(List<InformationEntity> ResultList,int UserId)
        {

            int result = 0;
            if (ResultList != null && ResultList.Count() != 0)
            {
                Stopwatch sw = new Stopwatch();
                DataTable dt = this.GetTableSchema();

                foreach (InformationEntity item in ResultList)
                {
                    DataRow dr = dt.NewRow();
                    dr[1] = UserId;
                    dr[2] = item.ID;
                    dr[3] = DateTime.Now;
                    dt.Rows.Add(dr);
                }

                sw.Start();
                result = informationDal.MutiInsertToInfoCustomer(dt);
                sw.Stop();
                Console.WriteLine(string.Format("Elapsed Time is {0} Milliseconds", sw.ElapsedMilliseconds));
            }
            return result;
        }

        /// <summary>
        /// 用户注册成功保存用户可见的活动消息数据
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RegisterTime"></param>
        /// <returns></returns>
        public int SaveActivityInfoForRegister(int UserId, DateTime RegisterTime)
        {
            // 注册成功后获得活动消息
            List<InformationEntity> ActivityList = GetActivityListForRegister(UserId);

            if (ActivityList == null || ActivityList.Count() == 0)
            {
                return 0;
            }

            // 获得注册用户可见的活动消息集合
            List<InformationEntity> ResultList = GetActivityListForRegisterSave(ActivityList, RegisterTime);

            if (ResultList == null || ResultList.Count() == 0)
            {
                return 0;
            }

            // 批量插入活动消息到InformationToCustomer表
            int result = MutiInsertToInfoCustomer(ResultList, UserId);
            return result;
        }

    }
}
