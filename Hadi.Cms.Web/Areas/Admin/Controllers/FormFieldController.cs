using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Web.Utilities;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class FormFieldController : Controller
    {
        private readonly FormFieldService _formFieldService;
        private readonly FormFieldValueService _formFieldValueService;

        public FormFieldController()
        {
            _formFieldService = new FormFieldService();
            _formFieldValueService = new FormFieldValueService();
        }


        public ActionResult Index(Guid id, int pageNumber = 1)
        {
            var pageSize = 5;
            var formFields = _formFieldService.GetList(f =>
                f.FormId == id && f.IsActive && !f.IsDeleted && f.CreatedBy == SessionData.Current.User.Id , f => f.OrderByDescending(o => o.CreatedDate));

            var pagedList = formFields.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(pagedList);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var formField = _formFieldService.Get(id);

            var formFieldValues =
                _formFieldValueService.GetList(f => f.IsActive && !f.IsDeleted && f.FormFieldId == formField.Id);

            _formFieldValueService.DeleteRange(formFieldValues);
            _formFieldValueService.Save();
            _formFieldService.Delete(formField.Id);
            _formFieldService.Save();
            return Json(new
            {
                Message = Strings.FormFieldModel_Delete_Successfully,
                Success = Strings.Success,
                Type = "success"
            },JsonRequestBehavior.AllowGet);
        }
    }
}