using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PagedList;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Configuration;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Notification.Email;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class NlEmailController : Controller
    {
        private readonly NlEmailService _nlEmailService;

        public NlEmailController()
        {
            _nlEmailService = new NlEmailService();
        }

        /// <summary>
        /// دریافت لیست ایمیل ها
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 20;
            var emails = _nlEmailService.GetList(n => n.IsActive && !n.IsDeleted , n=> n.OrderByDescending(o => o.CreatedDate));

            foreach (var email in emails)
                email.ShamsiCreatedDate = email.CreatedDate.ToPersianDate();

            var pagedList = emails.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            return View(pagedList);
        }

        /// <summary>
        /// حذف ایمیل
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var nlEmail = _nlEmailService.GetById(id);
            if (nlEmail == null)
            {
                return Json(new
                {
                    Message = Strings.DeleteOperationFailed,
                    Success = Strings.Error,
                    Type = "error"
                });
            }

            _nlEmailService.Delete(nlEmail.Id);
            _nlEmailService.Save();

            return Json(new
            {
                Message = Strings.NlEmail_DeleteNlEmailSuccessfully,
                Success = Strings.Success,
                Type = "success"
            });
        }

        /// <summary>
        /// ارسال خبرنامه به ایمیل های موجود در سامانه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SendEmail(Guid id)
        {
            if (Settings.QuartzActivated.HasValue && Settings.QuartzActivated == false)
            {
                Settings.QuartzActivated = true;

                var data = new Dictionary<string, object>();
                data.Add("id", id);
                data.Add("PhysicalApplicationPath", System.Web.HttpContext.Current.Request.PhysicalApplicationPath);
                SendEmailJob.StartScheduler<SendEmailJob>("0 0/5 * * * ?", "SendEmailKey", "SendGroup", "SendEmailTrigger", "SendGroup", data);
            }
            return Redirect("/Admin/NlMessages");
        }
    }
}