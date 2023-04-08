using Hadi.Cms.Model.Mappings.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hadi.Cms.Model.QueryModels
{
    public class TemplateDetailModel
    {
        public Guid Id { get; set; }
        public Guid TemplateId { get; set; }
        public string Title { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Source { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }

    public class TemplateDetailsListModel
    {
        public PagedList.IPagedList<ITemplateDetailDto> TemplateDetails { get; set; }
        public Guid TemplateId { get; set; }
        public string TemplateTitle { get; set; }
    }

}
