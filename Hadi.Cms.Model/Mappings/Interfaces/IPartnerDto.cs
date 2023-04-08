using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IPartnerDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// نام
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// تول تیپ
        /// </summary>
        string ToolTip { get; set; }
        /// <summary>
        /// لینک
        /// </summary>
        string Link { get; set; }

        /// <summary>
        /// شناسه عکس
        /// </summary>
        Guid? AttachmentImageGuid { get; set; }

        /// <summary>
        /// منبع عکس
        /// </summary>
        string AttachmentImageSource { get; set; }

        Guid CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        bool IsActive { get; set; }
    }
}
