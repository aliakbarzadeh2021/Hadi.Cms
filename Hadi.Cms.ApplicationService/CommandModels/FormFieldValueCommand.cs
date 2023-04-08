using System;
using System.Collections.Generic;
using System.Web;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت مقدار برای فیلد فرم
    /// </summary>
    public class FormFieldValueCommand
    {
        public Guid FormId { get; set; }
        public List<FormFileValueItemsCommand> FormFileValueItemsCommand { get; set; }
        public string FieldName { get; set; }
        public HttpPostedFileBase File { get; set; }
        public string Value { get; set; }
        public Guid? Code { get; set; }
    }

    public class FormFileValueItemsCommand
    {
        public string FieldName { get; set; }
        public string Value { get; set; }
    }
}
