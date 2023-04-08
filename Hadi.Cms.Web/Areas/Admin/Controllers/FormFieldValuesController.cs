using Hadi.Cms.ApplicationService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PagedList;
using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Configuration;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class FormFieldValuesController : Controller
    {
        private readonly FormFieldValueService _formFieldValueService;
        private readonly FormFieldService _formFieldService;
        private readonly AttachmentFileService _attachmentFileService;

        public FormFieldValuesController()
        {
            _formFieldValueService = new FormFieldValueService();
            _formFieldService = new FormFieldService();
            _attachmentFileService = new AttachmentFileService();
        }

        public ActionResult Index(Guid id, int pageNumber = 1)
        {
            var pageSize = 5;

            var formFieldValues = _formFieldValueService.GetList(f =>
                f.IsActive && !f.IsDeleted && f.FormId == id , f => f.OrderByDescending(o => o.CreatedDate));

            formFieldValues = formFieldValues.DistinctBy(f => f.Code).ToList();

            var pagedList = formFieldValues.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }

        public ActionResult Details(Guid id)
        {
            var formFieldDto = new List<FormFieldDto>();

            var formFieldValues = _formFieldValueService.GetList(f => f.IsActive && !f.IsDeleted && f.Code == id);

            foreach (var formFieldValue in formFieldValues)
            {
                var formField =
                    _formFieldService.Get(f => f.Id == formFieldValue.FormFieldId && f.IsActive && !f.IsDeleted);

                if(formField != null)
                {
                    formFieldDto.Add(new FormFieldDto
                    {
                        Label = formField.Label,
                        Name = formField.Name,
                        Value = formFieldValue.Value,
                        FileLink = Guid.TryParse(formFieldValue.Value,out Guid value) ? _attachmentFileService.GetAttachmentSourceValue(Guid.Parse(formFieldValue.Value)) : ""
                    });
                }
            }

            return View(formFieldDto);
        }
    }
}