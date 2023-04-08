using System;
using System.Web;
using Hadi.Cms.ApplicationService.Services;
using System.Web.Mvc;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Web.Utilities;

namespace Hadi.Cms.Web.Controllers
{
    public class FormFieldValuesController : Controller
    {
        private readonly FormFieldValueService _formFieldValueService;
        private readonly EventLoger _eventLoger;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly FormFieldService _formFieldService;

        public FormFieldValuesController()
        {
            _formFieldValueService = new FormFieldValueService();
            _eventLoger = new EventLoger();
            _attachmentFileService = new AttachmentFileService();
            _formFieldService = new FormFieldService();
        }

        [HttpPost]
        public ActionResult Create(FormFieldValueCommand command)
        {
            var code = Guid.NewGuid();

            code = command.Code.HasValue ? command.Code.Value : code;

            var ipAddress = IpProvider.GetIpValue();

            if (command.File != null && command.File.ContentLength > 0)
            {
                byte[] fileData;
                var fileSize = command.File.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(command.File.InputStream))
                    fileData = binaryReader.ReadBytes(command.File.ContentLength);

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(command.File.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(fileData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(Guid.Empty, Guid.NewGuid(), System.IO.Path.GetFileName(command.File.FileName),
                    System.IO.Path.GetExtension(command.File.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "FormFile - " + Guid.NewGuid(), "", fileData, DateTime.Now);

                command.Value = attachmentId.ToString();

                var field = _formFieldService.Get(f => f.Name == command.FieldName && f.FormId == command.FormId);
                if (field != null)
                {
                    var newFormFieldValue = new FormFieldValue()
                    {
                        Value = command.Value,
                        FormId = command.FormId,
                        Code = code,
                        FormFieldId = field.Id,
                        IpAddress = ipAddress,
                    };

                    _formFieldValueService.Insert(newFormFieldValue);
                }
            }

            if (command.FormFileValueItemsCommand != null && command.FormFileValueItemsCommand.Count > 0)
            {
                foreach (var item in command.FormFileValueItemsCommand)
                {
                    var field = _formFieldService.Get(f => f.Name == item.FieldName && f.FormId == command.FormId);
                    if (field != null)
                    {
                        var newFormFieldValue = new FormFieldValue()
                        {
                            Value = item.Value,
                            FormId = command.FormId,
                            Code = code,
                            FormFieldId = field.Id,
                            IpAddress = ipAddress,
                        };

                        _formFieldValueService.Insert(newFormFieldValue);
                    }
                }
            }

            _formFieldValueService.Save();
            _eventLoger.LogEvent(EventType.Information, Guid.Empty, "", "FormFieldValuesController", "Create", "Success Create Form Field Value", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

            return Json(new
            {
                Status = true,
                Message = "Done .",
                Code = code
            }, JsonRequestBehavior.AllowGet);
        }
    }
}