using LoveWineService;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using SFO2O.Utility.Uitl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Logging;

namespace WinePayService
{
    /// <summary>
    /// 定时任务.
    /// </summary>
    public sealed class QuartzManaer
    {
        private static readonly QuartzManaer quartzManager = new QuartzManaer();
        private string cronTriggerLambda = "59 59 23 * * ?";//每天23点执行时间,"秒 分 时 天 月 年"

        private QuartzManaer()
        {
            cronTriggerLambda = ConfigHelper.GetAppConfigString("CronTriggerLambda");
        }



        public static QuartzManaer GetInstance()
        {
            return quartzManager;
        }
        public void StartQuartz()
        {
            try
            {
                LogHelper.Info("QuartzManaer执行");

               
                IScheduler sched;
                ISchedulerFactory sf = new StdSchedulerFactory();
                sched = sf.GetScheduler();


                //====================每隔120分钟执行任务===================
                JobDetail jobShowGoods = new JobDetail("job1", "group1", typeof(QuartzShowGoodsJob));//IndexJob为实现了IJob接口的类
                DateTime tsShowGoods = TriggerUtils.GetNextGivenSecondDate(null, 0);//7天以后第一次运行
                TimeSpan intervaShowGoods = TimeSpan.FromMinutes(5);//每隔120分钟执行一次
                Trigger triggertShowGoods = new SimpleTrigger("QuartzShowGoodsJob", "group1", "job1", "group1", tsShowGoods, null,
                                                        SimpleTrigger.RepeatIndefinitely, intervaShowGoods);//每若干小时运行一次，小时间隔由appsettings中的IndexIntervalHour参数指定

                sched.AddJob(jobShowGoods, true);
                sched.ScheduleJob(triggertShowGoods);
                sched.Start();
            }
            catch (Exception ex)
            {
                LogHelper.Error("QuartzManaer异常",ex);
            }

           
        }

    }
}


