using System;
using System.Collections.Generic;
using Quartz;
using Quartz.Util;

namespace Hadi.Cms.scheduler.QuartzService
{
    public class JobExecuter<TJob> : BaseJob<TJob> where TJob : IJob
    {
        public override void ScheduleIt(string cronExpression,
            string jobKey = null,
            string jobGroupKey = null,
            string triggerKey = null,
            string triggerGroupKey = null, Dictionary<string, object> jobDataMap = null)
        {
            try
            {
                base.CronExpression = cronExpression;
                base.JobKey = string.IsNullOrEmpty(jobKey) ? Guid.NewGuid().ToString() : jobKey;
                base.JobGroupKey = string.IsNullOrEmpty(jobGroupKey) ? Guid.NewGuid().ToString() : jobGroupKey;
                base.TriggerKey = string.IsNullOrEmpty(triggerKey) ? Guid.NewGuid().ToString() : triggerKey;
                base.TriggerGroupKey = string.IsNullOrEmpty(triggerGroupKey) ? Guid.NewGuid().ToString() : triggerGroupKey;


                if (string.IsNullOrEmpty(base.CronExpression))
                    throw new ApplicationException("Cron expression cannot be empty .");


                var job = JobBuilder.Create<TJob>()
                    .WithIdentity(jobKey, jobGroupKey)
                    .Build();

                if (jobDataMap != null)
                {
                    foreach (KeyValuePair<string, object> data in jobDataMap)
                        job.JobDataMap.Add(data);

                }

                var trigger = (ICronTrigger)TriggerBuilder.Create()
                    .WithIdentity(triggerKey, triggerGroupKey)
                    .WithCronSchedule(CronExpression)
                    .Build();

                Scheduler.ScheduleJob(job, trigger);
                base.StartJob();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
