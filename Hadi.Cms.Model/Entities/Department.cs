using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// دپارتمان
    /// </summary>
    public class Department :BaseModel
    {
        public Department()
        {
            DepartmentServices = new HashSet<DepartmentService>();
            DepartmentSelectionReasons = new HashSet<DepartmentSelectionReason>();
            Employees = new HashSet<Employee>();
            Methodologies = new HashSet<Methodology>();
        }
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// لینک دکمه تماس با ما
        /// </summary>
        public string ContactUsLink { get; set; }
        /// <summary>
        /// شعار
        /// </summary>
        public string Slogan { get; set; }
        /// <summary>
        /// رنگ
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// شناسه عکس ضمیمه شده
        /// </summary>
        public Guid? AttachmentImageId { get; set; }
        /// <summary>
        /// عکس پس زمینه هدر دپارتمان
        /// </summary>
        public Guid? DepartmentBackgroundHeaderAttachmentImageId { get; set; }
        /// <summary>
        /// شناسه عکس ساختار سازمانی
        /// </summary>
        public Guid? StructureAttachmentImageId { get; set; }
        /// <summary>
        /// شناسه عکس پیوستن به تیم ما
        /// </summary>
        public Guid? JoinOurTeamImageAttachmentId { get; set; }
        /// <summary>
        /// توضیحات پیوستن به تیم ما
        /// </summary>
        [AllowHtml]
        public string JoinOurTeamDescription { get; set; }
        /// <summary>
        /// فعال بودن دکمه ی استخدام
        /// </summary>
        public bool RecruitmentButtonIsActive { get; set; }
        /// <summary>
        /// فعال یودن دکمه ی کارآموزی
        /// </summary>
        public bool InternshipButtonIsActive { get; set; }

        public ICollection<DepartmentService> DepartmentServices { get; set; }
        public ICollection<DepartmentSelectionReason> DepartmentSelectionReasons { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Methodology> Methodologies { get; set; }
    }
}
