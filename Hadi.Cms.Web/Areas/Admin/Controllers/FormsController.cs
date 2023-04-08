using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Web.Utilities;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// مدیریت فرم ها
    /// </summary>
    public class FormsController : Controller
    {
        private readonly FormService _formService;
        private readonly LanguageService _languageService;
        private readonly EventLoger _eventLoger;
        private readonly FormFieldService _formFieldService;
        private readonly FormFieldValueService _formFieldValueService;

        public FormsController()
        {
            _formService = new FormService();
            _languageService = new LanguageService();
            _eventLoger = new EventLoger();
            _formFieldService = new FormFieldService();
            _formFieldValueService = new FormFieldValueService();
        }

        /// <summary>
        /// دریافت لیست فرم ها
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var forms = _formService.GetList(f => !f.IsDeleted && f.CreatedBy == SessionData.Current.User.Id, o => o.OrderByDescending(f => f.CreatedDate));

            foreach (var form in forms)
                form.LanguageTitle = _languageService.Get(l => l.Id == form.LanguageId)?.Name;

            var pagedList = forms.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت قرم جدید
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.Languages = _languageService.GetList();

            return View(new FormCreateCommand());
        }

        /// <summary>
        /// ثبت فرم جدید
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCreateCommand command)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Languages = _languageService.GetList();
                return View(command);
            }

            var existsFormName = _formService.Any(f => f.Name == command.Name);
            if (existsFormName)
            {
                ViewBag.Languages = _languageService.GetList();

                ModelState.AddModelError("Name", Strings.FormModel_Exists_Name);
                return View(command);
            }

            var newFormId = _formService.CreateNewForm(command, SessionData.Current.User.Id);

            if (newFormId != Guid.Empty)
                _formFieldService.InsertRange(command.FormFields, newFormId, SessionData.Current.User.Id);

            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "FormsController", "Create", "Success Create Form", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// صفحه ی ویرایش فرم
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var form = _formService.Get(id);
            if (form == null)
                return RedirectToAction("Index");

            ViewBag.Languages = _languageService.GetList();

            return View(new FormEditCommand()
            {
                Id = form.Id,
                Name = form.Name,
                Title = form.Title,
                DisplayEnName = form.DisplayEnName,
                DisplayFaName = form.DisplayFaName,
                RedirectUrl = form.RedirectUrl,
                FormDataSource = form.FormDataSource,
                LanguageId = form.LanguageId
            });
        }

        /// <summary>
        /// ویرایش فرم
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormEditCommand command)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Languages = _languageService.GetList();
                return View(command);
            }

            var existsFormName = _formService.Any(f => f.Name == command.Name && f.Id != command.Id);
            if (existsFormName)
            {
                ViewBag.Languages = _languageService.GetList();

                ModelState.AddModelError("Name", Strings.FormModel_Exists_Name);
                return View(command);
            }

            var form = _formService.Get(command.Id).MapToEntity();

            _formService.UpdateForm(form, command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "FormsController", "Edit", "Success Edit Form", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index");
        }

        // <summary>
        // حذف فرم
        // </summary>
        // <param name ="id"></ param >
        // < returns ></ returns >
        public ActionResult Delete(Guid id)
        {
            try
            {
                var form = _formService.Get(id);

                var formFields = _formFieldService.GetList(f => f.FormId == form.Id);
                foreach (var formField in formFields)
                {
                    var formFieldsValues = _formFieldValueService.GetList(f => f.FormFieldId == formField.Id);
                    _formFieldValueService.DeleteRange(formFieldsValues);
                    _formFieldService.Delete(formField.Id);
                }
                _formFieldValueService.Save();
                _formFieldService.Save();

                _formService.Delete(form.Id);
                _formService.Save();
                return Json(new
                {
                    Message = Strings.Form_Successfully_Deleted,
                    Success = Strings.Success,
                    Type = "success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = Strings.Global_SystemError,
                    Success = Strings.Error,
                    Type = "error"
                });
            }
        }
    }
}