using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت پروژه ی جدید
    /// </summary>
    public class ProjectCreateCommand
    {
        public ProjectCreateCommand()
        {
            SliderImageGuid = new List<Guid>();
        }
        public Guid EmployerId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string ProjectType { get; set; }
        public string ProjectLink { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
        public List<Guid> ProjectTagsId { get; set; }
        public Guid? PreviewImageGuid { get; set; }
        public Guid? ImageGuid { get; set; }
        public Guid? FirstStepImage { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string FirstStepDescription { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string SecondStepDescription { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string FinalStepDescription { get; set; }
        public bool IsActive { get; set; }
        public string ProjectLinkText { get; set; }
        public List<Guid> TechnologiesId { get; set; }
        public List<Guid> ToolsId { get; set; }
        public List<Guid> SliderImageGuid { get; set; }
    }
}
