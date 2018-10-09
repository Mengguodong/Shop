using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.DAL.Product;
using SFO2O.Model.Product;
using SFO2O.Utility.Uitl;
using SFO2O.BLL.Common;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Utility.Cache;
using SFO2O.M.ViewModel.Product;
using SFO2O.Model.Common;
using SFO2O.Model.Extensions;
using System.Data;
using SFO2O.DAL.Promotion;
using SFO2O.Model.Category;
using SFO2O.Model.Promotion;
using SFO2O.References.IndexingService;
using SFO2O.Utility.Extensions;
using SFO2O.Model.Team;
using SFO2O.DAL.Common;

namespace SFO2O.BLL.Team
{
    public class TeamBll
    {
        public readonly TeamDal teamDal = new TeamDal();
        public readonly CommonDal commonDal = new CommonDal();

        /// <summary>
        /// 获取订单所属的团信息
        /// </summary>
        /// <param name="OrderCode"></param>
        /// <returns></returns>
        public TeamInfoEntity GetTeamInfoEntity(string OrderCode)
        {
            TeamInfoEntity entity = new TeamInfoEntity();
            entity = teamDal.GetTeamInfoEntity(OrderCode);

            return entity;
        }

        /// <summary>
        /// 团订单情况展示团详情页面逻辑
        /// </summary>
        /// <param name="OrderCode"></param>
        /// <param name="DeliveryRegion"></param>
        /// <returns></returns>
        public IList<TeamDetailEntity> TeamOrderPage(string OrderCode, int DeliveryRegion)
        {
            /// 获取订单所属的团信息
            TeamInfoEntity teamInfoEntity = GetTeamInfoEntity(OrderCode);

            if (teamInfoEntity == null)
            {
                //LogHelper.Info("----Team----" + "订单号：" + OrderCode + "，查询的团信息为null，teamInfoEntity=" + teamInfoEntity);
                return null;
            }

            /// 获取团详情信息
            var teamDetailList = GetTeamDetailList(teamInfoEntity.TeamCode);

            return teamDetailList;
            
        }

        /// <summary>
        /// 获得团成员UserId
        /// </summary>
        /// <param name="OrderCode"></param>
        /// <param name="TeamCode"></param>
        /// <returns></returns>
        public int GetTeamUserId(string OrderCode, string TeamCode)
        {
            return teamDal.GetTeamUserId(OrderCode,TeamCode);
        }

        /// <summary>
        /// 获取团详情信息
        /// </summary>
        /// <param name="TeamCode"></param>
        /// <param name="exchangeRate"></param>
        /// <returns></returns>
        public IList<TeamDetailEntity> GetTeamDetailList(string TeamCode)
        {
            /// 获取团详情信息
            var teamDetailList = teamDal.GetTeamDetailByCode(TeamCode);
            foreach (var item in teamDetailList)
            {
                /// 手机号格式化
                int lenth = item.Mobile.Length - 2 - 2;
                string replaceStr = item.Mobile.Substring(2, lenth);
                StringBuilder strbul = new StringBuilder();
                for (int i = 0; i < replaceStr.Length; i++)
                {
                    strbul.Append("*");
                }
                string mobileHidden = item.Mobile.Replace(replaceStr, strbul.ToString());

                item.Mobile = mobileHidden;
            }
            return teamDetailList;
        }

        public IList<TeamDetailEntity> GetTeamDetailListForStatus(string TeamCode)
        {
            /// 获取团详情信息
            var teamDetailList = teamDal.GetTeamDetailByCode(TeamCode);
            return teamDetailList;
        }

        public TeamInfoEntity GetTeamInfoByTeamCode(string teamCode)
        {
            return teamDal.GetTeamInfoByTeamCode(teamCode);
        }

        public TeamInfoEntity GetTeamInfoByPid(int Pid)
        {
            return teamDal.GetTeamInfoByPid(Pid);
        }
       
    }
}
