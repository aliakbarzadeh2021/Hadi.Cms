using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش پروژه
    /// </summary>
    public class ProjectEditCommand
    {
        public ProjectEditCommand()
        {
            ProjectTagsId = new List<Guid>();
            ToolsId = new List<Guid>();
            TechnologiesId = new List<Guid>();
            SliderImageGuid = new List<Guid>();
        }
        public Guid Id { get; set; }
        public Guid EmployerId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string ProjectType { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string ProjectLink { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
        public List<Guid> ProjectTagsId { get; set; }
        public Guid? PreviewImageGuid { get; set; }
        public string PreviewImageSource { get; set; }
        public Guid? ImageGuid { get; set; }
        public string ImageSource { get; set; }
        public Guid? FirstStepImage { get; set; }
        public string FirstStepImageSource { get; set; }
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
        public List<Guid> SliderImageGuid { get; set; }
        public string ProjectLinkText { get; set; }
        public List<Guid> TechnologiesId { get; set; }
        public List<Guid> ToolsId { get; set; }
        public List<IAttachmentFileDto> AttachmentFiles { get; set; }
    }
}
