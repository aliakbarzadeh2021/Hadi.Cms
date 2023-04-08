using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IFormFieldValueDto
    {
        Guid FormFieldId { get; set; }
        Guid FormId { get; set; }
        Guid Code { get; set; }
        /// <summary>
        /// شناسه
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// نام اینپوت - یونیک
        /// </summary>
        string UniqueName { get; set; }
        /// <summary>
        /// مقدار
        /// </summary>
        string Value { get; set; }
        /// <summary>
        /// ای پی کاربری که این مقدار را به فیلد نسبت داده است
        /// </summary>
        string IpAddress { get; set; }

        DateTime CreatedDate { get; set; }
        IFormFieldDto FormFieldDto { get; set; }

    }
}
