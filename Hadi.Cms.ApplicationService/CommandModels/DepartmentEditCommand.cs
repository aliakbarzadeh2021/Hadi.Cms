using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش دپارتمان
    /// </summary>
    public class DepartmentEditCommand
    {
        public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        public string ContactUsLink { get; set; }
        public string Slogan { get; set; }
        public string Color { get; set; }
        public Guid? AttachmentImageId { get; set; }
        public string Source { get; set; }
        public bool IsActive { get; set; }
        public List<Guid> ServicesId { get; set; }
        public Guid? DepartmentBackgroundHeaderAttachmentImageId { get; set; }
        public string DepartmentBackgroundHeaderAttachmentImageSource { get; set; }
        public Guid? StructureAttachmentImageId { get; set; }
        public string StructureAttachmentImageSource { get; set; }
        public Guid? JoinOurTeamImageAttachmentId { get; set; }
        public string JoinOurTeamImageSource { get; set; }
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string JoinOurTeamDescription { get; set; }
        public bool RecruitmentButtonIsActive { get; set; }
        public bool InternshipButtonIsActive { get; set; }
    }
}
