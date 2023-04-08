using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// خدمت
    /// </summary>
    public class Service : BaseModel
    {
        public Service()
        {
            DepartmentServices = new HashSet<DepartmentService>();
            ServiceTags = new HashSet<ServiceTag>();
            ServiceReceiverServices = new HashSet<ServiceReceiverService>();
            ServiceComments = new HashSet<ServiceComment>();
        }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        [AllowHtml]
        public string Description { get; set; }
        /// <summary>
        /// شتاسه لوگوی خدمت
        /// </summary>
        public Guid? ServiceLogoId { get; set; }
        /// <summary>
        /// شناسه عکس ضمیمه شده
        /// </summary>
        public Guid? AttachmentImageId { get; set; }

        [AllowHtml]
        public string SectionOneDescription { get; set; }
        public Guid? SectionOneThumbnailImageGuid { get; set; }
        public Guid? SectionOneImageGuid { get; set; }

        [AllowHtml]
        public string SectionTwoDescription { get; set; }
        public Guid? SectionTwoThumbnailImageGuid { get; set; }
        public Guid? SectionTwoImageGuid { get; set; }

        [AllowHtml]
        public string SectionThreeDescription { get; set; }
        public Guid? SectionThreeThumbnailImageGuid { get; set; }
        public Guid? SectionThreeImageGuid { get; set; }

        [AllowHtml]
        public string SectionFourDescription { get; set; }
        public Guid? SectionFourImageGuid { get; set; }
        public Guid? SectionFourThumbnailImageGuid { get; set; }
        public int ReviewCount { get; set; }

        public ICollection<DepartmentService> DepartmentServices { get; set; }
        public ICollection<ServiceTag> ServiceTags { get; set; }
        public ICollection<ServiceReceiverService> ServiceReceiverServices { get; set; }
        public ICollection<ServiceComment> ServiceComments { get; set; }
    }
}
