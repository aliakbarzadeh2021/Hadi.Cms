using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// مشتری
    /// </summary>
    class Customer : BaseModel
    {
        public Customer()
        {
            
        }

        /// <summary>
        /// نام
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// شماره تماس
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// آدرس ایمیل
        /// </summary>
        public string EmailAddress { get; set; }
    }
}
