using Quartz;
using Quartz.Impl;
using System.Collections.Generic;

namespace Hadi.Cms.scheduler.QuartzService
{

    public abstract class BaseJob<TJob> where TJob : IJob
    {
        private readonly ISchedulerFactory _schedulerFactory = new StdSchedulerFactory();
        public IScheduler Scheduler;

        protected string CronExpression = string.Empty;
        protected string JobKey = string.Empty;
        protected string JobGroupKey = string.Empty;
        protected string TriggerKey = string.Empty;
        protected string TriggerGroupKey = string.Empty;

        protected BaseJob()
        {
            Scheduler = _schedulerFactory.GetScheduler().Result;
        }

        public virtual void StartJob()
        {
            Scheduler.Start();
        }

        public virtual void StopJob()
        {
            Scheduler.Shutdown();
        }

        public abstract void ScheduleIt(string cronExpression,
                                                string jobKey = null,
                                                string jobGroupKey = null,
                                                string triggerKey = null,
                                                string triggerGroupKey = null, Dictionary<string, object> jobDataMap = null);

    }

}