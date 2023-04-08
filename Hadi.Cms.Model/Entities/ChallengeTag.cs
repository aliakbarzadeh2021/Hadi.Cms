using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// تگ های چالش
    /// </summary>
    public class ChallengeTag : BaseModel
    {
        public ChallengeTag()
        {

        }

        /// <summary>
        /// شناسه چالش
        /// </summary>
        public Guid ChallengeId { get; set; }
        /// <summary>
        /// شناسه تگ
        /// </summary>
        public Guid TagId { get; set; }
        public Challenge Challenge { get; set; }
        public Tag Tag { get; set; }
    }
}
