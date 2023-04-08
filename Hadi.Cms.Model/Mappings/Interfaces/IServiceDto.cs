using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IServiceDto
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        Guid? ServiceLogoId { get; set; }
        string ServiceLogoSource { get; set; }
        Guid? AttachmentImageId { get; set; }
        string AttachmentImageSource { get; set; }
        string SectionOneDescription { get; set; }
        Guid? SectionOneThumbnailImageGuid { get; set; }
        string SectionOneThumbnailImageSource { get; set; }
        Guid? SectionOneImageGuid { get; set; }
        string SectionOneImageSource { get; set; }
        string SectionTwoDescription { get; set; }
        Guid? SectionTwoImageGuid { get; set; }
        string SectionTwoImageSource { get; set; }
        Guid? SectionTwoThumbnailImageGuid { get; set; }
        string SectionTwoThumbnailImageSource { get; set; }
        string SectionThreeDescription { get; set; }
        Guid SectionThreeImageGuid { get; set; }
        string SectionThreeImageSource { get; set; }
        Guid? SectionThreeThumbnailImageGuid { get; set; }
        string SectionThreeThumbnailImageSource { get; set; }
        string SectionFourDescription { get; set; }
        Guid? SectionFourImageGuid { get; set; }
        string SectionFourImageSource { get; set; }
        Guid? SectionFourThumbnailImageGuid { get; set; }
        string SectionFourThumbnailImageSource { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        bool IsActive { get; set; }
        int ReviewCount { get; set; }
        ICollection<IDepartmentServiceDto> DepartmentServicesDto { get; set; }
        ICollection<IServiceTagDto> ServiceTagDto { get; set; }
        ICollection<IServiceReceiverServiceDto> ServiceReceiverServiceDto { get; set; }
        ICollection<IServiceCommentDto> ServiceCommentDto { get; set; }
    }
}
