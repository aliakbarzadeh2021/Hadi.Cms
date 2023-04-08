using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت خدمت جدید
    /// </summary>
    public class ServiceCreateCommand
    {
        public ServiceCreateCommand()
        {
            ServiceTagsId = new List<Guid>();
        }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string SectionOneDescription { get; set; }
        public Guid? SectionOneThumbnailImageGuid { get; set; }
        public Guid? SectionOneImageGuid { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string SectionTwoDescription { get; set; }
        public Guid? SectionTwoThumbnailImageGuid { get; set; }
        public Guid? SectionTwoImageGuid { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string SectionThreeDescription { get; set; }
        public Guid? SectionThreeThumbnailImageGuid { get; set; }
        public Guid? SectionThreeImageGuid { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string SectionFourDescription { get; set; }
        public Guid? SectionFourThumbnailImageGuid { get; set; }
        public Guid? SectionFourImageGuid { get; set; }
        public Guid? AttachmentImageId { get; set; }
        public bool IsActive { get; set; }
        public List<Guid> ServiceTagsId { get; set; }
        public Guid? ServiceLogoId { get; set; }
    }
}
