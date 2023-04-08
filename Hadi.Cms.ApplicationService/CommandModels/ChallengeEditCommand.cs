using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش چالش
    /// </summary>
    public class ChallengeEditCommand
    {
        public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false , ErrorMessageResourceType = typeof(Strings),ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string ProblemDescription { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string ProblemSolvingDescription { get; set; }
        public Guid? ImageAttachmentId { get; set; }
        public string ImageSource { get; set; }
        public Guid? VideoAttachmentId { get; set; }
        public string VideoSource { get; set; }

        public List<Guid> ProjectsId { get; set; }
        public bool IsActive { get; set; }
    }
}
