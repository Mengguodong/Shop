using SFO2O.Utility.Uitl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using WinePayService;

namespace LoveWineService
{
    public partial class QuartzShowGoodsService : ServiceBase
    {
        public QuartzShowGoodsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {

                LogHelper.Info("服务OnStart函数开始执行");
                QuartzManaer.GetInstance().StartQuartz();
            }
            catch (Exception ex)
            {
                LogHelper.Error("OnStart", ex);
                var sc = new ServiceController("QuartzShowGoodsService");
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);

                LogHelper.Error("OnStart", ex);
            }
            LogHelper.Info( "服务WineGameService函数：OnStart执行结束");

        }

        protected override void OnStop()
        {
            LogHelper.Info("服务OnStop函数结束执行");
        }
    }
}
