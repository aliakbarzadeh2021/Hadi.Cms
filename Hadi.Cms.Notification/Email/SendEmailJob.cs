using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Configuration;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.scheduler.QuartzService;

namespace Hadi.Cms.Notification.Email
{
    public class SendEmailJob : IJob
    {
        private readonly NlMessageService _nlMessageService;
        private readonly EmailProvider _emailProvider;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly NlMessageEmailService _nlMessageEmailService;

        public SendEmailJob()
        {
            _nlMessageService = new NlMessageService();
            _emailProvider = new EmailProvider();
            _attachmentFileService = new AttachmentFileService();
            _nlMessageEmailService = new NlMessageEmailService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            var id = Guid.Parse(context.JobDetail.JobDataMap["id"].ToString());
            var physicalApplicationPath = context.JobDetail.JobDataMap["PhysicalApplicationPath"].ToString();
            var nlMessage = _nlMessageService.GetList(n => n.Id == id, null, n => n.NlMessageEmails).FirstOrDefault();
            if (nlMessage != null)
            {
                var nlEmailMessages =
                    _nlMessageEmailService.GetList(nm => !nm.Published && nm.NlMessageId == nlMessage.Id, null,
                        nm => nm.NlEmail).MapToEntities();

                var email = nlEmailMessages.Select(nm => nm.NlEmail).ToList().FirstOrDefault();

                if (email == null)
                {
                    context.Scheduler.Shutdown();
                    Settings.QuartzActivated = false;
                    return Task.CompletedTask;
                }

                physicalApplicationPath = physicalApplicationPath +
                                          _attachmentFileService.GetAttachmentSourceValue(nlMessage.AttachmentId,null,false,physicalApplicationPath);

                var sent = _emailProvider.SendMail(email.Email, nlMessage.Subject, nlMessage.Text,
                     nlMessage.AttachmentId.HasValue,
                     nlMessage.AttachmentId.HasValue
                         ? physicalApplicationPath
                         : "");

                if (sent)
                {
                    var nlEmailMessage = nlEmailMessages.FirstOrDefault(nm => nm.NlEmailId == email.Id);
                    nlEmailMessage.Published = true;
                    _nlMessageEmailService.Update(nlEmailMessage);
                    _nlMessageEmailService.Save();
                }


            }
            return Task.CompletedTask;
        }

        public static void StartScheduler<TJob>(string cronExpression, string jobKey, string jobGroupKey, string triggerKey, string triggerGroupKey, Dictionary<string, object> jobDataMap) where TJob : IJob
        {
            var jobExecuter = new JobExecuter<TJob>();
            jobExecuter.ScheduleIt(cronExpression, jobKey, jobGroupKey, triggerKey, triggerGroupKey, jobDataMap);
        }
    }
}
