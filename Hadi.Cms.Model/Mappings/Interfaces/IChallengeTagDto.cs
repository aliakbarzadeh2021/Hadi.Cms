using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IChallengeTagDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// شناسه چالش
        /// </summary>
        Guid ChallengeId { get; set; }
        /// <summary>
        /// شناسه تگ
        /// </summary>
        Guid TagId { get; set; }
        IChallengeDto ChallengeDto { get; set; }
        ITagDto TagDto { get; set; }
    }
}
