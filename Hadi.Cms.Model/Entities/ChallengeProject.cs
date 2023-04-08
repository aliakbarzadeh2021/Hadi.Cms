using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// چالش خای پروژه
    /// </summary>
    public class ChallengeProject : BaseModel
    {
        public ChallengeProject()
        {
            
        }

        /// <summary>
        /// شناسه چالش
        /// </summary>
        public Guid ChallengeId { get; set; }
        /// <summary>
        /// شناسه پروژه
        /// </summary>
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public Challenge Challenge { get; set; }
    }
}
