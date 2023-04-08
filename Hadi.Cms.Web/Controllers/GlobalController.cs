using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Model.Entities;
using System;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Controllers
{
    public class GlobalController : Controller
    {
        private ContactUsService _contactUsService;

        public GlobalController()
        {
            _contactUsService = new ContactUsService();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Rules()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendContactUsForm(ContactUsDto model)
        {
            if (ModelState.IsValid == true)
            {
                ContactUs newContactUs = new ContactUs()
                {
                    CreatedWhen = DateTime.Now,
                    Subject = model.Subject,
                    Text = model.Text,
                    UserEmail = model.UserEmail,
                    UserMobile = model.UserMobile,
                    UserName = model.UserName,
                    UserIp = Request.UserHostAddress
                };

                _contactUsService.Insert(newContactUs);
                _contactUsService.Save();

                //ViewBag.ErrorMessage = Strings.ContactUsForm_SuccessRegister;
                return View("ContactUs");
            }
            else
            {
                //ViewBag.ErrorMessage = Strings.ContactUsForm_ErrorInRegister;
                return View("ContactUs");
            }
        }
    }
}