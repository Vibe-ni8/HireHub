using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

public class FeedbackConfiguration : BaseEntity
{
    public FeedbackConfiguration() : base("feedback_configuration")
    {
    }

    public int FeedbackConfigId { get; set; }
    public int DriveId { get; set; }
    public bool OverallRatingRequired { get; set; }
    public bool TechnicalSkillRequired { get; set; }
    public bool CommunicationRequired { get; set; }
    public bool ProblemSolvingRequired { get; set; }
    public bool RecommendationRequired { get; set; }
    public bool OverallFeedbackRequired { get; set; }

    // Navigation
    public Drive? Drive { get; set; }
}
