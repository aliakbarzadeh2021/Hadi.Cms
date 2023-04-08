using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// تماس با ما
    /// </summary>
    public class ContactUs
    {
        public ContactUs()
        {
            Id = Guid.NewGuid();
            CreatedWhen = DateTime.Now;
        }

        public Guid Id { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public string UserIp { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserMobile { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
    }
}
