using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.ApplicationService.Services;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Controllers
{
    public class NlEmailController : Controller
    {
        /// <summary>
        /// خبرنامه
        /// </summary>
        private readonly NlEmailService _nlEmailService;
        public NlEmailController()
        {
            _nlEmailService = new NlEmailService();
        }

        /// <summary>
        /// ثبت ایمیل برای خبرنامه
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(NlEmailCreateCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Email))
            {
                return Json(new
                {
                    Message = "لطفا ایمیل خورد را وارد نمایید .",
                    Success = false
                });
            }
            var existsEmail = _nlEmailService.Any(n => n.Email == command.Email);
            if (existsEmail)
            {
                return Json(new
                {
                    Message = "ایمیل قبلا در خبرنامه نامه ثبت شده است .",
                    Success = false
                });
            }
            _nlEmailService.CreateNewEmail(command);
            
            return Json(new
            {
                Message = "ثبت نام با موفقیت انجام شد.",
                Success = true
            });
        }
    }
}