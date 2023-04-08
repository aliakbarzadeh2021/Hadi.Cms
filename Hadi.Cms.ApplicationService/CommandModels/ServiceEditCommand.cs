using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش خدمت
    /// </summary>
    public class ServiceEditCommand
    {
        public ServiceEditCommand()
        {
            ServiceTagsId = new List<Guid>();
        }
        public Guid Id { get; set; }
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
        public Guid? SectionOneImageGuid { get; set; }
        public Guid? SectionOneThumbnailImageGuid { get; set; }
        public string SectionOneThumbnailImageSource { get; set; }
        public string SectionOneImageSource { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string SectionTwoDescription { get; set; }
        public Guid? SectionTwoImageGuid { get; set; }
        public Guid? SectionTwoThumbnailImageGuid { get; set; }
        public string SectionTwoThumbnailImageSource { get; set; }
        public string SectionTwoImageSource { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string SectionThreeDescription { get; set; }
        public Guid? SectionThreeImageGuid { get; set; }
        public Guid? SectionThreeThumbnailImageGuid { get; set; }
        public string SectionThreeThumbnailImageSource { get; set; }
        public string SectionThreeImageSource { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string SectionFourDescription { get; set; }
        public Guid? SectionFourImageGuid { get; set; }
        public Guid? SectionFourThumbnailImageGuid { get; set; }
        public string SectionFourThumbnailImageSource { get; set; }
        public string SectionFourImageSource { get; set; }
        public Guid? AttachmentImageId { get; set; }
        public string Source { get; set; }
        public bool IsActive { get; set; }
        public List<Guid> ServiceTagsId { get; set; }
        public Guid? ImageSideOne { get; set; }
        public Guid? ImageSideTwo { get; set; }
        public Guid? ImageSideThree { get; set; }
        public Guid? ImageSideFour { get; set; }
        public Guid? ServiceLogoId { get; set; }
        public string ServiceLogoSource { get; set; }

    }
}
